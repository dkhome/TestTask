using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System;
using System.Web;
using Searchfight.Infrastructure.Interfaces;
using Searchfight.Core;
using Searchfight.Domain.Interfaces;

namespace Searchfight.Infrastructure.Services.Search.Google
{
    public class GoogleTermSearchService : ITermSearchService
    {
        private const string searchEngineName = "Google";
        private const string searchEngineBase = "https://customsearch.googleapis.com/customsearch/v1";

        private readonly Uri searchRequestBase;
        private readonly HttpClient httpClient;

        public GoogleTermSearchService(IOptions<GoogleConfig> config, IHttpClientAccessor clientAccessor)
        {
            searchRequestBase = CreateSearchRequestBase(config.Value);
            httpClient = clientAccessor.Client;
        }

        public async Task<SearchResult> GetResultsCountAsync(string term)
        {
            var resultCounts = await GetCountAsync(term);
            return new SearchResult { Count = resultCounts, Source = searchEngineName, Term = term };
        }

        private Uri CreateSearchRequestBase(GoogleConfig settings)
        {
            var uriBuilder = new UriBuilder(searchEngineBase);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["key"] = settings.ApiKey;
            query["cx"] = settings.EngineId;
            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }

        private async Task<decimal> GetCountAsync(string t)
        {
            string requestUri = CreateTermSearchUri(t);
            var queryStream = await httpClient.GetStreamAsync(requestUri);
            var searchResult = await JsonSerializer.DeserializeAsync<GoogleResult>(queryStream);
            return searchResult.Statistics.TotalResults;
        }

        private string CreateTermSearchUri(string term)
        {
            return $"{searchRequestBase}&q={term}";
        }
    }
}
