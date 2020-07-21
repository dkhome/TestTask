using Searchfight.Core;
using Searchfight.Domain.Interfaces;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Searchfight.Infrastructure.Services.Search
{
    public abstract class AbstractTermSearchService : ITermSearchService
    {
        protected readonly HttpClient httpClient;

        public AbstractTermSearchService(HttpClient client)
        {
            httpClient = client;
        }

        public async Task<SearchResult> GetResultsCountAsync(string term)
        {
            var requestMessage = CreateRequestMessage(term);
            var responseMessage = await httpClient.SendAsync(requestMessage);
            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var count = await GetCountFromResponseAsync(responseStream);

            return new SearchResult
            {
                Count = count,
                Source = SearchEngineName,
                Term = term
            };
        }

        abstract protected Task<decimal> GetCountFromResponseAsync(Stream responseStream);
        abstract protected HttpRequestMessage CreateRequestMessage(string term);
        abstract protected string SearchEngineName { get; }

    }
}
