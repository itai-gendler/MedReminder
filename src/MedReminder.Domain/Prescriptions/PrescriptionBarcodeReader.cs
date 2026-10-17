namespace MedReminder.Domain.Prescriptions;

// Builds the prescription code from the barcodes read off a prescription
// slip, one barcode at a time (scanner or webcam). The shapes depend on
// the country, so the caller picks the reader of the reference country:
//
//  - Italian: the NRE (NreCode, 15 characters) is printed either as one
//    barcode or as two (5 letters or digits, then 10 digits) read in any
//    order and joined; the NRBE of a prescription outside the national
//    health service (12 characters: a letter, a letter or digit, then 10
//    digits; Sistema TS technical annex) is one barcode. Any other
//    barcode on the slip, such as the patient's tax code (16 characters),
//    is rejected, so it never lands in the prescription code.
//  - Generic: no shape is known, so any single barcode of letters,
//    digits and common separators up to PrescriptionRules.MaxCodeLength
//    is taken as the code; the user checks it in the editor.
//
// A Code 39 start / stop asterisk and an AIM symbology identifier sent
// by a scanner are removed. Not thread-safe: the caller serialises
// Offer. The payloads are prescription codes: never log them.
public abstract class PrescriptionBarcodeReader
{
    private const int AimIdentifierLength = 3;

    public static PrescriptionBarcodeReader Italian() => new ItalianReader();

    public static PrescriptionBarcodeReader Generic() => new GenericReader();

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

        private string? _firstPart;
        private string? _secondPart;

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

    private sealed class GenericReader : PrescriptionBarcodeReader
    {
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
