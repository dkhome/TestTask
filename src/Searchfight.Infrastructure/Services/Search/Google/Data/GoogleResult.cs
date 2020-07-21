using System.Text.Json.Serialization;

namespace Searchfight.Infrastructure.Services.Search.Google.Data
{
    internal class GoogleResult
    {
        internal class SearchStatistics
        {
            [JsonPropertyName("totalResults")]
            [JsonConverter(typeof(TotalResultsConverter))]
            public ulong TotalResults { get; set; }
        }

        [JsonPropertyName("searchInformation")]
        public SearchStatistics Statistics { get; set; }
    }
}
