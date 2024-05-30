using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChainResource.Models
{
    public class ExchangeRateList
    {
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal>? Rates { get; set; }
    }
}
