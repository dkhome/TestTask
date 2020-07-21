using Searchfight.Core;
using System.Collections.Generic;

namespace Searchfight.Domain.Interfaces
{
    public interface ISearchStatisticsPresenter
    {
        void ShowData(IEnumerable<SearchResult> result);
    }
}
