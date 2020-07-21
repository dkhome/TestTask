using Searchfight.Core;
using Searchfight.Domain.Interfaces;
using Searchfight.Domain.Statistics.Interfaces;
using Searchfight.UI.Extensions;
using System;
using System.Collections.Generic;

namespace Searchfight.UI
{
    public class SearchStatisticsConsolePresenter : ISearchStatisticsPresenter
    {
        private readonly IStatisticsService statisticsService;

        public SearchStatisticsConsolePresenter(IStatisticsService statService)
        {
            statisticsService = statService;
        }

        public void ShowData(IEnumerable<SearchResult> results)
        {
            Console.WriteLine(statisticsService.GetResultsGroupedByTerm(results)
                .FormatStatisticsByTermToString());

            Console.WriteLine(statisticsService.GetWinnersByEngine(results)
                .FormatWinnersBySearchEngineToString());

            Console.WriteLine(statisticsService.GetTotalWinners(results)
                .FormatTotalWinnersToString());
        }
    }
}
