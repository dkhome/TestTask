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
                .Select(termGrouping => //calculate sum per term
                    new 
                    { 
                        Term = termGrouping.Key, 
                        Sum = termGrouping.Sum(tg => tg.Count) 
                    })
                .GroupBy(s => s.Sum) //group by sum, there can be several terms with the same sum
                .OrderByDescending(grouped => grouped.Key) //order by key = Sum
                .First().Select(tw => tw.Term); //Take first group, select terms
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
                        .GroupBy(t => t.Count) //group by count because there could be several terms with the same results count
                        .OrderByDescending(t => t.Key) 
                        .First().Select(t => t.Term) //Take top group ordered by Key = count
                    ));
        }
    }
}
