using Microsoft.Extensions.Options;
using Searchfight.Infrastructure.Interfaces;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Searchfight.Infrastructure.Services.Search.Bing
{
    public class BingTermSearchService : AbstractTermSearchService
    {
        private const string HeaderKey = "Ocp-Apim-Subscription-Key";

        private readonly BingConfig config;

        protected override string SearchEngineName => "Bing";

        public BingTermSearchService(IOptions<BingConfig> configOption, IHttpClientAccessor clientAccessor)
            :base(clientAccessor.Client)
        {
            config = configOption.Value;
        }

        protected override HttpRequestMessage CreateRequestMessage(string term)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"{config.Url}?q={term}");
            message.Headers.Add(HeaderKey, config.Key);
            return message;
        }

        protected override async Task<decimal> GetCountFromResponseAsync(Stream responseStream) =>
            (await JsonSerializer.DeserializeAsync<BingResult>(responseStream)).Statistics.Count;
    }
}
