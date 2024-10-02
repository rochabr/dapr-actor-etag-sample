using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WorkerClient.Models
{
    public class OrderSummary
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        [JsonPropertyName("orderValue")]
        public string OrderValue { get; set; }
    }
}