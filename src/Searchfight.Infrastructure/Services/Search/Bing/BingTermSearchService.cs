using Microsoft.Extensions.Options;
using Searchfight.Infrastructure.Interfaces;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Searchfight.Infrastructure.Services.Search.Bing
{
    public class BingTermSearchService : AbstractTermSearchService
    {
        private const string HeaderKey = "Ocp-Apim-Subscription-Key";

        private readonly HttpClient httpClient;
        private readonly BingConfig config;

        protected override string EngineName => "Bing";

        public BingTermSearchService(IOptions<BingConfig> configOption, IHttpClientAccessor clientAccessor)
        {
            httpClient = clientAccessor.Client;
            config = configOption.Value;
        }

        protected override async Task<decimal> GetCountAsync(HttpRequestMessage requestMessage)
        {
            var responseMessage = await httpClient.SendAsync(requestMessage);
            var searchResult = await JsonSerializer.DeserializeAsync<BingResult>(
                await responseMessage.Content.ReadAsStreamAsync());
            return searchResult.Statistics.Count;
        }

        protected override HttpRequestMessage CreateSearchMessage(string term)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"{config.Url}?q={term}");
            message.Headers.Add(HeaderKey, config.Key);
            return message;
        }
    }
}
