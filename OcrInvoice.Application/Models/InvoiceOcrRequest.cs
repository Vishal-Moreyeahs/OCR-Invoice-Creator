using System.Text.Json.Serialization;

namespace OcrInvoice.Application.Models
{
    public class InvoiceOcrRequest
    {
        public List<Worksheet1Item> Worksheet1 { get; set; }
        public List<Worksheet2Item> Worksheet2 { get; set; }
    }

    public class Worksheet1Item
    {
        [JsonPropertyName("sender_name")]
        public string? SenderName { get; set; }

        [JsonPropertyName("sender_address")]
        public string? SenderAddress { get; set; }

        [JsonPropertyName("sender_gst_number")]
        public string? SenderGSTNumber { get; set; }

        [JsonPropertyName("receiver_name")]
        public string? ReceiverName { get; set; }

        [JsonPropertyName("receiver_address")]
        public string? ReceiverAddress { get; set; }

        [JsonPropertyName("receiver_gst_number")]
        public string? ReceiverGSTNumber { get; set; }

        [JsonPropertyName("sender_address_cityname")]
        public string? SenderCity { get; set; }

        [JsonPropertyName("sender_address_countryname")]
        public string? SenderCountry { get; set; }

        [JsonPropertyName("sender_address_zipcode")]
        public string? SenderZipCode { get; set; }

        [JsonPropertyName("invoice_number")]
        public string? InvoiceNumber { get; set; }

        [JsonPropertyName("invoice_date")]
        public string? InvoiceDate { get; set; }

        [JsonPropertyName("invoice_category")]
        public string? InvoiceCategory { get; set; }

        [JsonPropertyName("total_amount")]
        public string? TotalAmount { get; set; }

        [JsonPropertyName("cgst")]
        public string? CGST { get; set; }

        [JsonPropertyName("sgst")]
        public string? SGST { get; set; }

        [JsonPropertyName("gst")]
        public string? GST { get; set; }

        [JsonPropertyName("cgst%")]
        public string? CgstPercentage { get; set; }

        [JsonPropertyName("sgst%")]
        public string? SgstPercentage { get; set; }

        [JsonPropertyName("gst%")]
        public string? GstPercentage { get; set; }

        [JsonPropertyName("clarity_percentage")]
        public string? OcrPercentage { get; set; }
    }

    public class Worksheet2Item
    {
        [JsonPropertyName("item_name")]
        public string? ItemName { get; set; }

        [JsonPropertyName("item_quantity")]
        public int? ItemQuantity { get; set; }

        [JsonPropertyName("item_rate")]
        public string? ItemRate { get; set; }

        [JsonPropertyName("item_tax")]
        public string? ItemTax { get; set; }

        [JsonPropertyName("item_amount")]
        public string? ItemAmount { get; set; }
    }
}
