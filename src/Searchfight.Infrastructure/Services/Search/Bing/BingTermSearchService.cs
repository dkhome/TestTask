using Microsoft.Extensions.Options;
using Searchfight.Core;
using Searchfight.Domain.Interfaces;
using Searchfight.Infrastructure.Interfaces;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Searchfight.Infrastructure.Services.Search.Bing
{
    public class BingTermSearchService : ITermSearchService
    {
        private const string Name = "Bing";
        private const string SearchEngineBase = "https://api.cognitive.microsoft.com/bing/v7.0/search";
        private const string HeaderKey = "Ocp-Apim-Subscription-Key";

        private readonly HttpClient httpClient;
        private readonly BingConfig config;

        public BingTermSearchService(IOptions<BingConfig> configOption, IHttpClientAccessor clientAccessor)
        {
            httpClient = clientAccessor.Client;
            config = configOption.Value;
        }

        public async Task<SearchResult> GetResultsCountAsync(string term)
        {
            var responseMessage = await httpClient.SendAsync(CreateSearchMessage(term));
            var searchResult = await JsonSerializer.DeserializeAsync<BingResult>(
                await responseMessage.Content.ReadAsStreamAsync());
            
            return new SearchResult 
            { 
                Count = searchResult.Statistics.Count, 
                Source = Name, 
                Term = term 
            };
        }

        private HttpRequestMessage CreateSearchMessage(string term)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, CreateTermSearchUri(term));
            message.Headers.Add(HeaderKey, config.Key);
            return message;
        }

        private string CreateTermSearchUri(string term)
        {
            return $"{SearchEngineBase}?q={term}";
        }
    }
}
