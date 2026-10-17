using FluentAssertions;
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

    [Theory]
    [InlineData("RX-2026/000123", "RX-2026/000123")]
    [InlineData(" *ABC123* ", "ABC123")]
    public void Generic_single_barcode_is_taken_as_the_code(string payload, string expected)
    {
        var read = PrescriptionBarcodeReader.Generic().Offer(payload);

        read.Step.Should().Be(PrescriptionBarcodeStep.Complete);
        read.Code.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("https://example.org/rx?id=1")]
    public void Generic_reader_rejects_empty_or_unexpected_characters(string? payload)
    {
        PrescriptionBarcodeReader.Generic().Offer(payload).Step.Should().Be(PrescriptionBarcodeStep.Rejected);
    }

    [Fact]
    public void Generic_reader_rejects_codes_longer_than_the_stored_code()
    {
        PrescriptionBarcodeReader.Generic().Offer(new string('A', PrescriptionRules.MaxCodeLength + 1))
            .Step.Should().Be(PrescriptionBarcodeStep.Rejected);
    }
}
