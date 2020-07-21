using Searchfight.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Searchfight.Domain.Statistics.Interfaces
{
    public interface IStatisticsService
    {
        IEnumerable<IGrouping<string, SearchResult>> GetResultsGroupedByTerm(IEnumerable<SearchResult> results);
        IEnumerable<string> GetTotalWinners(IEnumerable<SearchResult> results);
        IEnumerable<Tuple<string, IEnumerable<string>>> GetWinnersByEngine(IEnumerable<SearchResult> results);

    }
}