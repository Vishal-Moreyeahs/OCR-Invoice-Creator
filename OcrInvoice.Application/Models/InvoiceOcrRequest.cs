using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

        [JsonPropertyName("invoice_number")]
        public string? InvoiceNumber { get; set; }

        [JsonPropertyName("invoice_date")]
        public string? InvoiceDate { get; set; }

        [JsonPropertyName("invoice_category")]
        public string? InvoiceCategory { get; set; }

        [JsonPropertyName("total_amount")]
        public double? TotalAmount { get; set; }

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
        public double? ItemRate { get; set; }

        [JsonPropertyName("item_tax")]
        public double? ItemTax { get; set; }

        [JsonPropertyName("item_amount")]
        public double? ItemAmount { get; set; }
    }
}
