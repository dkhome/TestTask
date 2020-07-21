using Searchfight.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Searchfight.Domain.Interfaces
{
    public interface ISearchStatisticsService
    {
        Task<IEnumerable<SearchResult>> CollectStatistics(IEnumerable<string> terms);
    }
}