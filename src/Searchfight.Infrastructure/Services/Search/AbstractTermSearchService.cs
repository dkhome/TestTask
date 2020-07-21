using Searchfight.Core;
using Searchfight.Domain.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Searchfight.Infrastructure.Services.Search
{
    public abstract class AbstractTermSearchService : ITermSearchService
    {
        public async Task<SearchResult> GetResultsCountAsync(string term)
        {
            var requestMessage = CreateSearchMessage(term);
            var count = await GetCountAsync(requestMessage);
            return new SearchResult
            {
                Count = count,
                Source = EngineName,
                Term = term
            };
        }

        abstract protected HttpRequestMessage CreateSearchMessage(string term);
        abstract protected Task<decimal> GetCountAsync(HttpRequestMessage requestMessage);
        abstract protected string EngineName { get; }

    }
}
