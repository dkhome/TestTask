using Searchfight.Core;
using Searchfight.Domain.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Searchfight.Domain.Statistics.Services
{
    public class StatisticsService : IStatisticsService
    {
        public IEnumerable<string> GetTotalWinners(IEnumerable<SearchResult> results)
        {
            if (!results.Any())
                return new string[0];

            return results.GroupBy(s => s.Term)
                .Select(termGrouping => 
                    new 
                    { 
                        Term = termGrouping.Key, 
                        Sum = termGrouping.Sum(tg => tg.Count) 
                    })
                .GroupBy(s => s.Sum)
                .OrderByDescending(grouped => grouped.Key)
                .First().Select(tw => tw.Term);
        }

        public IEnumerable<IGrouping<string, SearchResult>> GetResultsGroupedByTerm(IEnumerable<SearchResult> results) 
            => results.GroupBy(r => r.Term);

        public IEnumerable<Tuple<string, IEnumerable<string>>> GetWinnersByEngine(IEnumerable<SearchResult> results)
        {
            if (!results.Any())
                return new Tuple<string, IEnumerable<string>>[0];

            return results.GroupBy(r => r.Source)
                .Select(sourceGrouping =>
                new Tuple<string, IEnumerable<string>>(
                    sourceGrouping.Key,
                    sourceGrouping
                        .GroupBy(t => t.Count)
                        .OrderByDescending(t => t.Key)
                        .First()
                        .Select(t => t.Term)
                    ));
        }
    }
}
