using Searchfight.Infrastructure.Services.Search.Utils;
using System.Text.Json.Serialization;

namespace Searchfight.Infrastructure.Services.Search.Google
{
    public class GoogleResult
    {
        public class SearchStatistics
        {
            [JsonPropertyName("totalResults")]
            [JsonConverter(typeof(StringToDecimalConverter))]
            public decimal Count { get; set; }
        }

        [JsonPropertyName("searchInformation")]
        public SearchStatistics Statistics { get; set; }
    }
}
