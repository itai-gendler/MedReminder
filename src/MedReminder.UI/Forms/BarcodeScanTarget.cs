using MedReminder.Application.Catalogue;
using MedReminder.Domain.Catalogue;
using MedReminder.Domain.Prescriptions;
using MedReminder.UI.Camera;
using Microsoft.Extensions.Logging;

namespace MedReminder.UI.Forms;

// What BarcodeScanDialog reads: the texts it shows, the symbologies the
// webcam decodes and the rule that accepts a payload. Offer runs on the
// UI thread for the scanner and on the camera worker thread for the
// webcam. Payloads are never logged: only their length and shape.
internal abstract class BarcodeScanTarget
{
    public abstract string TitleKey { get; }
    public abstract string PromptKey { get; }
    public abstract string ScannerHintKey { get; }
    public abstract string WebcamHintKey { get; }
    public abstract string UnrecognizedKey { get; }

    // Shown when a part of a code printed in parts was read; null when
    // the target has no such codes.
    public virtual string? PartReadKey => null;

    public abstract IReadOnlyCollection<BarcodeSymbology> Symbologies { get; }

    // Minimum keystrokes for an unterminated scanner payload to be
    // submitted on idle: the length of the shortest accepted shape.
    public abstract int MinimumIdleKeystrokes { get; }

    public abstract BarcodeScanOffer Offer(RawBarcode raw);

    public abstract void LogRead(ILogger log, RawBarcode raw, bool webcam, bool onIdle, TimeSpan elapsed);
}

internal enum BarcodeScanOffer
{
    Rejected,
    // One part of a code printed in parts; the dialog keeps reading.
    Partial,
    Accepted,
}

// A medicine pack: Code 32, EAN-13 or GS1 DataMatrix, parsed into the
// national code or GTIN (docs/analysis/ANALYSIS-A2-BARCODE-SCAN.md).
internal sealed class MedicineBarcodeTarget(IBarcodeParser parser) : BarcodeScanTarget
{
    public BarcodeContent? Content { get; private set; }

    public override string TitleKey => "Ui.BarcodeScanDialog.Title";
    public override string PromptKey => "Ui.BarcodeScanDialog.Prompt";
    public override string ScannerHintKey => "Ui.BarcodeScanDialog.ScannerHint";
    public override string WebcamHintKey => "Ui.BarcodeScanDialog.WebcamHint";
    public override string UnrecognizedKey => "Ui.BarcodeScanDialog.Unrecognized";
    public override IReadOnlyCollection<BarcodeSymbology> Symbologies => FrameBarcodeDecoder.PackSymbologies;

    // The 6-character Code 32 form.
    public override int MinimumIdleKeystrokes => ItalianPharmacode.Code32Length;

    public override BarcodeScanOffer Offer(RawBarcode raw)
    {
        var content = parser.Parse(raw);
        if (!content.HasLookupKey) return BarcodeScanOffer.Rejected;
        Content = content;
        return BarcodeScanOffer.Accepted;
    }

    // Never log the payload: it may carry an FMD serial number.
    public override void LogRead(ILogger log, RawBarcode raw, bool webcam, bool onIdle, TimeSpan elapsed)
    {
        if (webcam)
        {
            log.LogInformation(
                "Barcode read: webcam, symbology {Symbology}, length {Length}, national code {HasNationalCode}, GTIN {HasGtin}, {Seconds:0.0} s.",
                raw.Symbology, raw.Payload.Length, Content?.NationalCode is not null,
                Content?.Gtin is not null, elapsed.TotalSeconds);
        }
        else
        {
            log.LogInformation(
                "Barcode read: scanner, length {Length}, national code {HasNationalCode}, GTIN {HasGtin}, idle {OnIdle}.",
                raw.Payload.Length, Content?.NationalCode is not null, Content?.Gtin is not null, onIdle);
        }
    }
}

// A prescription slip: the code of the reference country's slips
// (PrescriptionBarcodeReader), from one barcode or from several read
// one after the other, by scanner, webcam or both. The texts follow the
// kind of code: linear barcodes (Italy) or a 2D code (Germany, France).
internal sealed class PrescriptionCodeTarget(PrescriptionBarcodeReader reader) : BarcodeScanTarget
{
    private readonly object _lock = new();
    private readonly string _variant = reader.IsTwoDimensional ? ".TwoDimensional" : string.Empty;

    public string? Code { get; private set; }

    public override string TitleKey => "Ui.PrescriptionScan.Title" + _variant;
    public override string PromptKey => "Ui.PrescriptionScan.Prompt" + _variant;
    public override string ScannerHintKey => "Ui.PrescriptionScan.ScannerHint" + _variant;
    public override string WebcamHintKey => "Ui.PrescriptionScan.WebcamHint" + _variant;
    public override string UnrecognizedKey => "Ui.PrescriptionScan.Unrecognized" + _variant;
    public override string? PartReadKey => reader.IsTwoDimensional ? null : "Ui.PrescriptionScan.PartRead";
    public override IReadOnlyCollection<BarcodeSymbology> Symbologies => reader.Symbologies;
    public override int MinimumIdleKeystrokes => reader.MinimumPayloadLength;

    public override BarcodeScanOffer Offer(RawBarcode raw)
    {
        lock (_lock)
        {
            var read = reader.Offer(raw.Payload);
            switch (read.Step)
            {
                case PrescriptionBarcodeStep.Complete:
                    Code = read.Code;
                    return BarcodeScanOffer.Accepted;
                case PrescriptionBarcodeStep.Partial:
                    return BarcodeScanOffer.Partial;
                default:
                    return BarcodeScanOffer.Rejected;
            }
        }
    }

    // Never log the payload or the code: a prescription code.
    public override void LogRead(ILogger log, RawBarcode raw, bool webcam, bool onIdle, TimeSpan elapsed)
        => log.LogInformation(
            "Prescription code read: {Source}, symbology {Symbology}, length {Length}, idle {OnIdle}, {Seconds:0.0} s.",
            webcam ? "webcam" : "scanner", raw.Symbology, Code?.Length ?? 0, onIdle, elapsed.TotalSeconds);
}
