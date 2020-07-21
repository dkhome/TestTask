using Searchfight.Core;
using Searchfight.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Searchfight.Domain.Services
{
    public class SearchStatisticsService : ISearchStatisticsService
    {
        private readonly IEnumerable<ITermSearchService> termSearchers;

        public SearchStatisticsService(IEnumerable<ITermSearchService> searchers)
        {
            termSearchers = searchers;
        }

        public async Task<IEnumerable<SearchResult>> CollectStatistics(IEnumerable<string> terms)
        {
            return await Task.WhenAll(
                terms.SelectMany(t => termSearchers.Select(s => s.GetResultsCountAsync(t))));
        }
    }
}
