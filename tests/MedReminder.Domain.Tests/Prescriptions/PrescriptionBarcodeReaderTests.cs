using FluentAssertions;
using MedReminder.Domain.Catalogue;
using MedReminder.Domain.Prescriptions;
using Xunit;

namespace MedReminder.Domain.Tests.Prescriptions;

public class PrescriptionBarcodeReaderTests
{
    [Theory]
    [InlineData("1200A", "4970794479")]
    [InlineData("4970794479", "1200A")]
    public void Italian_nre_in_two_barcodes_is_joined_in_slip_order(string first, string second)
    {
        var reader = PrescriptionBarcodeReader.Italian();

        reader.Offer(first).Step.Should().Be(PrescriptionBarcodeStep.Partial);
        var read = reader.Offer(second);

        read.Step.Should().Be(PrescriptionBarcodeStep.Complete);
        read.Code.Should().Be("1200A4970794479");
    }

    [Fact]
    public void Italian_part_read_twice_waits_for_the_other_part()
    {
        var reader = PrescriptionBarcodeReader.Italian();

        reader.Offer("1200A").Step.Should().Be(PrescriptionBarcodeStep.Partial);
        reader.Offer("1200A").Step.Should().Be(PrescriptionBarcodeStep.Partial);
        reader.Offer("4970794479").Code.Should().Be("1200A4970794479");
    }

    [Fact]
    public void Italian_later_part_of_the_same_kind_replaces_the_earlier_one()
    {
        var reader = PrescriptionBarcodeReader.Italian();

        reader.Offer("1200A");
        reader.Offer("0900B");
        reader.Offer("4970794479").Code.Should().Be("0900B4970794479");
    }

    [Fact]
    public void Italian_reset_forgets_the_part_read()
    {
        var reader = PrescriptionBarcodeReader.Italian();

        reader.Offer("1200A");
        reader.Reset();
        reader.Offer("4970794479").Step.Should().Be(PrescriptionBarcodeStep.Partial);
    }

    [Theory]
    [InlineData("T00003548358", "T00003548358")]
    [InlineData("t00003548358", "T00003548358")]
    [InlineData("AB0003548358", "AB0003548358")]
    [InlineData("1200A4970794479", "1200A4970794479")]
    [InlineData("*1200A4970794479*", "1200A4970794479")]
    [InlineData("]C0T00003548358\r\n", "T00003548358")]
    public void Italian_single_barcode_codes_are_complete(string payload, string expected)
    {
        var read = PrescriptionBarcodeReader.Italian().Offer(payload);

        read.Step.Should().Be(PrescriptionBarcodeStep.Complete);
        read.Code.Should().Be(expected);
    }

    [Fact]
    public void Italian_code_scanned_by_parts_with_asterisks_is_joined()
    {
        var reader = PrescriptionBarcodeReader.Italian();

        reader.Offer("*1200A*");
        reader.Offer("]A0*4970794479*").Code.Should().Be("1200A4970794479");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("RSSMRA80A01H501U")]
    [InlineData("0D4YD5")]
    [InlineData("8001234567897")]
    [InlineData("12345678901")]
    [InlineData("1T0003548358")]
    [InlineData("1200-")]
    [InlineData("497079447X")]
    public void Italian_other_barcodes_are_rejected(string? payload)
    {
        PrescriptionBarcodeReader.Italian().Offer(payload).Step.Should().Be(PrescriptionBarcodeStep.Rejected);
    }

    [Fact]
    public void Italian_rejected_barcode_keeps_the_part_read()
    {
        var reader = PrescriptionBarcodeReader.Italian();

        reader.Offer("1200A");
        reader.Offer("RSSMRA80A01H501U").Step.Should().Be(PrescriptionBarcodeStep.Rejected);
        reader.Offer("4970794479").Code.Should().Be("1200A4970794479");
    }

    private const string AccessCode = "777bea0e13cc9c42ceec14aec3ddee2263325dc2c6c699db115f58fe423607ea";

    [Theory]
    [InlineData("{\"urls\": [\"Task/160.346.135.722.516.16/$accept?ac=" + AccessCode + "\"]}", "160.346.135.722.516.16")]
    [InlineData("{\"urls\":[\"Task/160.000.033.491.280.78/$accept?ac=" + AccessCode + "\"]}\r\n", "160.000.033.491.280.78")]
    [InlineData("160.880.966.157.248.22", "160.880.966.157.248.22")]
    public void German_token_yields_the_rezept_id_without_the_access_code(string payload, string expected)
    {
        var read = PrescriptionBarcodeReader.German().Offer(payload);

        read.Step.Should().Be(PrescriptionBarcodeStep.Complete);
        read.Code.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    // Check digits do not match.
    [InlineData("160.346.135.722.516.17")]
    [InlineData("{\"urls\": [\"Task/160.346.135.722.516.17/$accept?ac=" + AccessCode + "\"]}")]
    // A code grouping two prescriptions.
    [InlineData("{\"urls\": [\"Task/160.346.135.722.516.16/$accept?ac=" + AccessCode + "\", \"Task/160.880.966.157.248.22/$accept?ac=" + AccessCode + "\"]}")]
    [InlineData("<script>alert(1)</script>")]
    [InlineData("1200A4970794479")]
    public void German_reader_rejects_anything_but_one_valid_token(string? payload)
    {
        PrescriptionBarcodeReader.German().Offer(payload).Step.Should().Be(PrescriptionBarcodeStep.Rejected);
    }

    [Theory]
    [InlineData("RX-2026/000123", "RX-2026/000123")]
    [InlineData(" ABC123DEF456 ", "ABC123DEF456")]
    public void French_bare_identifier_is_taken_as_the_code(string payload, string expected)
    {
        var read = PrescriptionBarcodeReader.French().Offer(payload);

        read.Step.Should().Be(PrescriptionBarcodeStep.Complete);
        read.Code.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("https://example.org/rx?id=1")]
    // A structured payload, which may carry the patient's identity.
    [InlineData("ID=ABC123;NOM=DUPONT;NE=01011980")]
    public void French_reader_rejects_empty_or_structured_payloads(string? payload)
    {
        PrescriptionBarcodeReader.French().Offer(payload).Step.Should().Be(PrescriptionBarcodeStep.Rejected);
    }

    [Fact]
    public void French_reader_rejects_codes_longer_than_the_stored_code()
    {
        PrescriptionBarcodeReader.French().Offer(new string('A', PrescriptionRules.MaxCodeLength + 1))
            .Step.Should().Be(PrescriptionBarcodeStep.Rejected);
    }

    [Theory]
    [InlineData("IT", false, new[] { BarcodeSymbology.Code39, BarcodeSymbology.Code128 })]
    [InlineData(null, false, new[] { BarcodeSymbology.Code39, BarcodeSymbology.Code128 })]
    [InlineData("DE", true, new[] { BarcodeSymbology.DataMatrix, BarcodeSymbology.QrCode })]
    [InlineData("fr", true, new[] { BarcodeSymbology.QrCode })]
    public void Reader_follows_the_reference_country(string? country, bool twoDimensional, BarcodeSymbology[] symbologies)
    {
        var reader = PrescriptionBarcodeReader.ForCountry(country);

        reader.Should().NotBeNull();
        reader!.IsTwoDimensional.Should().Be(twoDimensional);
        reader.Symbologies.Should().BeEquivalentTo(symbologies);
    }

    [Theory]
    [InlineData("ES")]
    [InlineData("US")]
    [InlineData("GB")]
    [InlineData("EU")]
    public void Scan_is_not_offered_for_other_countries(string country)
    {
        PrescriptionBarcodeReader.ForCountry(country).Should().BeNull();
    }
}
