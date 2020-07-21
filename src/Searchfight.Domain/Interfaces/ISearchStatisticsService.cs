using Searchfight.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Searchfight.Domain.Interfaces
{
    public interface ISearchStatisticsService
    {
        IEnumerable<Task<SearchResult>> CollectStatistics(IEnumerable<string> terms);
    }
}