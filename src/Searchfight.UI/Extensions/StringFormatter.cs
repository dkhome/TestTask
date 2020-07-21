using Searchfight.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Searchfight.UI.Extensions
{
    public static class StringFormatter
    {
        public static string FormatStatisticsByTermToString(this IEnumerable<IGrouping<string, SearchResult>> results)
        {
            var sb = new StringBuilder();
            sb.AppendJoin(Environment.NewLine,
                results.Select(termGrouping => $"{termGrouping.Key}: {string.Join(" ", termGrouping.Select(FormatSourceAndCount))}"));
            return sb.ToString();
        }

        public static string FormatWinnersBySearchEngineToString(this IEnumerable<Tuple<string, IEnumerable<string>>> winnerByEngine)
        {
            var sb = new StringBuilder();
            sb.AppendJoin(Environment.NewLine,
                winnerByEngine.Select(wbe =>
                {
                    if (wbe.Item2.Count() > 1)
                    {
                        return $"{wbe.Item1} winners: {string.Join(", ", wbe.Item2)}";
                    }
                    return $"{wbe.Item1} winner: {wbe.Item2.First()}";
                }));
            return sb.ToString();
        }

        public static string FormatTotalWinnersToString(this IEnumerable<string> totalWinners)
        {
            if (totalWinners.Count() > 1)
            {
                return($"Total winners: {string.Join(", ", totalWinners.Select(tw => tw))}");
            }
            else
            {
                return ($"Total winner: {totalWinners.First()}");
            }
        }

        private static string FormatSourceAndCount(SearchResult sr)
        {
            return $"{sr.Source}: {sr.Count}";
        }
    }
}
