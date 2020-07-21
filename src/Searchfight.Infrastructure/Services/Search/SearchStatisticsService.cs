using Microsoft.Extensions.Logging;
using Searchfight.Core;
using Searchfight.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Searchfight.Infrastructure.Services.Search
{
    public class SearchStatisticsService : ISearchStatisticsService
    {
        private readonly IEnumerable<ITermSearchService> termSearchers;
        private readonly ILogger logger;

        public SearchStatisticsService(IEnumerable<ITermSearchService> searchers, ILoggerFactory loggerFactory)
        {
            termSearchers = searchers;
            logger = loggerFactory.CreateLogger(typeof(SearchStatisticsService));
        }

        public async Task<IEnumerable<SearchResult>> CollectStatistics(IEnumerable<string> terms)
        {
            var searchTasks = terms
                .SelectMany(t => termSearchers.Select(s => s.GetResultsCountAsync(t)))
                .ToArray();
            var aggregateTask = Task.WhenAll(searchTasks);

            try
            {
                return await aggregateTask;
            }
            catch
            {
                if (aggregateTask.IsFaulted) 
                {
                    //log failed tasks exceptions
                    foreach (var ex in aggregateTask.Exception.InnerExceptions)
                    {
                        logger.LogError(ex.Message);
                    }
                }
            }

            //continue working with received data
            return searchTasks
                .Where(t => t.Status == TaskStatus.RanToCompletion)
                .Select(t => t.Result)
                .ToArray();
        }
    }
}
