using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WorkerActorN.Models
{
    public class OrderSummary
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }
    }
}