using Microsoft.Extensions.Options;
using Searchfight.Core;
using Searchfight.Domain.Interfaces;
using Searchfight.Infrastructure.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Searchfight.Infrastructure.Services.Search.Bing
{
    public class BingTermSearchService : ITermSearchService
    {
        private const string Name = "Bing";
        private const string searchEngineBase = "https://customsearch.googleapis.com/customsearch/v1";


        private readonly HttpClient httpClient;
        public BingTermSearchService(IOptions<BingConfig> config, IHttpClientAccessor clientAccessor)
        {
            //searchRequestBase = CreateSearchRequestBase(config.Value);
            httpClient = clientAccessor.Client;
        }
        /*
            GET https://api.cognitive.microsoft.com/bing/v7.0/search?q=.net HTTP/1.1
            Host: api.cognitive.microsoft.com
            Ocp-Apim-Subscription-Key: ••••••••••••••••••••••••••••••••

         * */

        public Task<SearchResult> GetResultsCountAsync(string term)
        {
            //httpClient.DefaultRequestHeaders.Add
            return Task.FromResult(new SearchResult { Count = 1, Source = Name, Term = term });
        }
    }
}
