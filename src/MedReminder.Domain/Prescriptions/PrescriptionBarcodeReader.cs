using System.Text.RegularExpressions;
using MedReminder.Domain.Catalogue;

namespace MedReminder.Domain.Prescriptions;

// Builds the prescription code from the barcodes read off a prescription
// slip, one barcode at a time (scanner or webcam). The shapes depend on
// the country, so the caller picks the reader of the reference country
// (ForCountry); a country without a reader offers no scan:
//
//  - Italian: the NRE (NreCode, 15 characters) is printed either as one
//    barcode or as two (5 letters or digits, then 10 digits) read in any
//    order and joined; the NRBE of a prescription outside the national
//    health service (12 characters: a letter, a letter or digit, then 10
//    digits; Sistema TS technical annex) is one barcode. Any other
//    barcode on the slip, such as the patient's tax code (16 characters),
//    is rejected, so it never lands in the prescription code.
//  - German: the E-Rezept token (gematik api-erp,
//    docs/erp_versicherte.adoc, handover to the pharmacy) is a
//    DataMatrix or QR code holding
//    {"urls": ["Task/<Rezept-ID>/$accept?ac=<AccessCode>"]}. Only the
//    Rezept-ID (22 characters: 3.3.3.3.3.2 digits, the last two ISO
//    7064 MOD 97-10 check digits) is kept. The AccessCode authorises the
//    retrieval of the prescription: it is never stored. A code that
//    groups several prescriptions is rejected, so the user scans the
//    code of the single prescription. A Rezept-ID typed by hand is
//    accepted.
//  - French: the electronic prescription (ordonnance numerique) carries
//    a QR code with its unique prescription number printed below it.
//    The QR payload layout is not public (GIE SESAM-Vitale
//    specification) and may also hold the patient's identity, so only a
//    payload that is one bare identifier (letters, digits and common
//    separators, up to PrescriptionRules.MaxCodeLength) is taken;
//    anything else is rejected and the user types the printed number.
//
// A Code 39 start / stop asterisk and an AIM symbology identifier sent
// by a scanner are removed. Not thread-safe: the caller serialises
// Offer. The payloads are prescription codes: never log them.
public abstract partial class PrescriptionBarcodeReader
{
    private const int AimIdentifierLength = 3;

    public static PrescriptionBarcodeReader Italian() => new ItalianReader();

    public static PrescriptionBarcodeReader German() => new GermanReader();

    public static PrescriptionBarcodeReader French() => new FrenchReader();

    // The reader of the reference country, or null when its slips carry
    // no code MedReminder can read (Spain, the United States, the EU
    // scope and any other country). An empty or invalid value means
    // Italy, as everywhere else the reference country is read.
    public static PrescriptionBarcodeReader? ForCountry(string? referenceCountry)
    {
        var country = CountryCode.TryParse(referenceCountry, out var parsed) ? parsed.Value : "IT";
        return country switch
        {
            "IT" => Italian(),
            "DE" => German(),
            "FR" => French(),
            _ => null,
        };
    }

    // Symbologies the webcam decodes for this country's slips.
    public abstract IReadOnlyCollection<BarcodeSymbology> Symbologies { get; }

    // Whether the code is a 2D code (QR or DataMatrix) rather than linear
    // barcodes, which changes the instructions shown.
    public abstract bool IsTwoDimensional { get; }

    // Shortest payload worth submitting when a scanner sends no suffix.
    public abstract int MinimumPayloadLength { get; }

    public abstract PrescriptionBarcodeRead Offer(string? payload);

    // Forgets a part read earlier.
    public virtual void Reset()
    {
    }

    private static string Normalize(string payload)
    {
        var text = payload.Trim();
        if (text.Length > AimIdentifierLength && text[0] == ']') text = text[AimIdentifierLength..];
        return text.Trim('*', ' ', '\t', '\r', '\n');
    }

    private sealed class ItalianReader : PrescriptionBarcodeReader
    {
        private const int FirstPartLength = 5;
        private const int SecondPartLength = NreCode.Length - FirstPartLength;
        private const int NrbeLength = 12;

        // Code 39 carries the NRE, Code 128 the NRBE.
        private static readonly IReadOnlyCollection<BarcodeSymbology> SlipSymbologies =
            [BarcodeSymbology.Code39, BarcodeSymbology.Code128];

        private string? _firstPart;
        private string? _secondPart;

        public override IReadOnlyCollection<BarcodeSymbology> Symbologies => SlipSymbologies;
        public override bool IsTwoDimensional => false;
        public override int MinimumPayloadLength => FirstPartLength;

        public override PrescriptionBarcodeRead Offer(string? payload)
        {
            if (payload is null) return PrescriptionBarcodeRead.Rejected;
            var text = Normalize(payload).ToUpperInvariant();

            if (IsNrbe(text) || (text.Length == NreCode.Length && NreCode.TryParse(text, out _)))
            {
                Reset();
                return PrescriptionBarcodeRead.Complete(text);
            }
            if (text.Length == FirstPartLength && IsLettersOrDigits(text)) _firstPart = text;
            else if (text.Length == SecondPartLength && IsDigits(text)) _secondPart = text;
            else return PrescriptionBarcodeRead.Rejected;

            if (_firstPart is null || _secondPart is null) return PrescriptionBarcodeRead.Partial;
            var code = _firstPart + _secondPart;
            Reset();
            return PrescriptionBarcodeRead.Complete(code);
        }

        public override void Reset()
        {
            _firstPart = null;
            _secondPart = null;
        }

        private static bool IsNrbe(string text)
            => text.Length == NrbeLength
               && char.IsAsciiLetter(text[0])
               && char.IsAsciiLetterOrDigit(text[1])
               && IsDigits(text.AsSpan(2));

        private static bool IsLettersOrDigits(ReadOnlySpan<char> text)
        {
            foreach (var c in text)
            {
                if (!char.IsAsciiLetterOrDigit(c)) return false;
            }
            return true;
        }

        private static bool IsDigits(ReadOnlySpan<char> text)
        {
            foreach (var c in text)
            {
                if (!char.IsAsciiDigit(c)) return false;
            }
            return true;
        }
    }

    private sealed partial class GermanReader : PrescriptionBarcodeReader
    {
        private const int RezeptIdLength = 22;
        private const int RezeptIdDigits = 17;

        // The printout carries DataMatrix codes; the gematik app may show
        // either symbology.
        private static readonly IReadOnlyCollection<BarcodeSymbology> TokenSymbologies =
            [BarcodeSymbology.DataMatrix, BarcodeSymbology.QrCode];

        public override IReadOnlyCollection<BarcodeSymbology> Symbologies => TokenSymbologies;
        public override bool IsTwoDimensional => true;
        public override int MinimumPayloadLength => RezeptIdLength;

        public override PrescriptionBarcodeRead Offer(string? payload)
        {
            if (payload is null) return PrescriptionBarcodeRead.Rejected;
            var text = Normalize(payload);
            if (RezeptIdPattern().IsMatch(text))
                return IsValidRezeptId(text) ? PrescriptionBarcodeRead.Complete(text) : PrescriptionBarcodeRead.Rejected;

            // Only the Rezept-ID is captured: the AccessCode never leaves
            // the payload.
            string? id = null;
            foreach (Match match in TaskUrlPattern().Matches(text))
            {
                var candidate = match.Groups[1].Value;
                if (id is not null && id != candidate) return PrescriptionBarcodeRead.Rejected;
                id = candidate;
            }
            return id is not null && IsValidRezeptId(id)
                ? PrescriptionBarcodeRead.Complete(id)
                : PrescriptionBarcodeRead.Rejected;
        }

        // ISO 7064 MOD 97-10 over the 17 digits: the remainder is 1.
        private static bool IsValidRezeptId(string id)
        {
            var remainder = 0;
            var digits = 0;
            foreach (var c in id)
            {
                if (c == '.') continue;
                remainder = (remainder * 10 + (c - '0')) % 97;
                digits++;
            }
            return digits == RezeptIdDigits && remainder == 1;
        }

        [GeneratedRegex(@"^[0-9]{3}(?:\.[0-9]{3}){4}\.[0-9]{2}$", RegexOptions.CultureInvariant)]
        private static partial Regex RezeptIdPattern();

        [GeneratedRegex(@"Task/([0-9]{3}(?:\.[0-9]{3}){4}\.[0-9]{2})/\$accept\?ac=[0-9A-Fa-f]+", RegexOptions.CultureInvariant)]
        private static partial Regex TaskUrlPattern();
    }

    private sealed class FrenchReader : PrescriptionBarcodeReader
    {
        private static readonly IReadOnlyCollection<BarcodeSymbology> QrOnly = [BarcodeSymbology.QrCode];

        public override IReadOnlyCollection<BarcodeSymbology> Symbologies => QrOnly;
        public override bool IsTwoDimensional => true;
        public override int MinimumPayloadLength => 4;

        public override PrescriptionBarcodeRead Offer(string? payload)
        {
            if (payload is null) return PrescriptionBarcodeRead.Rejected;
            var text = Normalize(payload);
            if (text.Length == 0 || text.Length > PrescriptionRules.MaxCodeLength) return PrescriptionBarcodeRead.Rejected;
            foreach (var c in text)
            {
                if (!char.IsAsciiLetterOrDigit(c) && c is not ('-' or '/' or '.' or ' ')) return PrescriptionBarcodeRead.Rejected;
            }
            return PrescriptionBarcodeRead.Complete(text);
        }
    }
}

public enum PrescriptionBarcodeStep
{
    // Not a barcode of a prescription code.
    Rejected,
    // One part of a code printed in parts; the others are still missing.
    Partial,
    Complete,
}

// Code is set when Step is Complete.
public readonly record struct PrescriptionBarcodeRead(PrescriptionBarcodeStep Step, string? Code)
{
    public static readonly PrescriptionBarcodeRead Rejected = new(PrescriptionBarcodeStep.Rejected, null);
    public static readonly PrescriptionBarcodeRead Partial = new(PrescriptionBarcodeStep.Partial, null);

    public static PrescriptionBarcodeRead Complete(string code) => new(PrescriptionBarcodeStep.Complete, code);
}
