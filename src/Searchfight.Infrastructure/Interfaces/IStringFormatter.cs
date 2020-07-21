using Searchfight.Core;
using System.Collections.Generic;

namespace Searchfight.Infrastructure.Interfaces
{
    public interface IStringFormatter
    {
        string FormatResults(IEnumerable<SearchResult> results);
        string FormatTotalWinners(IEnumerable<SearchResult> results);
        string FormatWinnersBySearchEngine(IEnumerable<SearchResult> results);
    }
}
