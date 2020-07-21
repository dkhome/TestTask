using Searchfight.Core;
using System.Threading.Tasks;

namespace Searchfight.Domain.Interfaces
{
    public interface ITermSearchService
    {
        Task<SearchResult> GetResultsCountAsync(string term);
    }
}
