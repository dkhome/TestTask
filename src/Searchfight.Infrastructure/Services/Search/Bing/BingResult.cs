using System.Text.Json.Serialization;

namespace Searchfight.Infrastructure.Services.Search.Bing
{
    public class BingResult
    {
        public class WebPageStatistics
        {
            [JsonPropertyName("totalEstimatedMatches")]
            public decimal Count { get; set; }
        }

        [JsonPropertyName("webPages")]
        public WebPageStatistics Statistics { get; set; }
    }
}