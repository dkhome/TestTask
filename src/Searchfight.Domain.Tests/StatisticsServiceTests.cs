using NUnit.Framework;
using Searchfight.Core;
using Searchfight.Domain.Statistics.Interfaces;
using Searchfight.Domain.Statistics.Services;
using System.Collections.Generic;
using System.Linq;

namespace Searchfight.Domain.Tests
{
    public class StatisticsServiceTests
    {
        private IStatisticsService statisticsService;

        [SetUp]
        public void Setup()
        {
            statisticsService = new StatisticsService();
        }

        [Test]
        public void TestSingleWinner()
        {
            List<SearchResult> results = GetSingleWinnerResults();
            var result = statisticsService.GetTotalWinners(results);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void TestSingleWinnerResult()
        {
            List<SearchResult> results = GetSingleWinnerResults();
            var result = statisticsService.GetTotalWinners(results);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Languages.Java, result.First());
        }

        [Test]
        public void TestTwoWinners()
        {
            List<SearchResult> results = GetMultipleWinnerResults();
            var result = statisticsService.GetTotalWinners(results);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void TestTwoWinnerResults()
        {
            List<SearchResult> results = GetMultipleWinnerResults();
            var result = statisticsService.GetTotalWinners(results);
            Assert.AreEqual(2, result.Count());
            Assert.That(result, Contains.Item(Languages.Java));
            Assert.That(result, Contains.Item(Languages.Dotnet));
        }

        [Test]
        public void TestEmptyResults()
        {
            Assert.IsEmpty(statisticsService.GetTotalWinners(new SearchResult[0]));
        }
        [Test]
        public void TestWinnersCountBySearchEngine()
        {
            List<SearchResult> results = GetResultsWithSingleWinners();
            var result = statisticsService.GetWinnersByEngine(results);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void TestWinnersAreCorrect()
        {
            List<SearchResult> results = GetResultsWithSingleWinners();
            var result = statisticsService.GetWinnersByEngine(results);
            Assert.AreEqual(2, result.Count());
            Assert.That(result.First(i => i.Item1 == SearchEngines.Google).Item2, Contains.Item(Languages.Java));
            Assert.That(result.First(i => i.Item1 == SearchEngines.Bing).Item2, Contains.Item(Languages.Cobol));
        }

        [Test]
        public void TestMultipleWinnersCountBySearchEngine()
        {
            List<SearchResult> results = GetResultsWithMultipleWinners();
            var result = statisticsService.GetWinnersByEngine(results);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(2, result.First(r => r.Item1 == SearchEngines.Bing).Item2.Count());
        }

        [Test]
        public void TestMultipleWinnersAreCorrect()
        {
            List<SearchResult> results = GetResultsWithMultipleWinners();
            var result = statisticsService.GetWinnersByEngine(results);
            Assert.AreEqual(2, result.Count());
            Assert.That(result.First(i => i.Item1 == SearchEngines.Google).Item2, Contains.Item(Languages.Java));
            var bingResult = result.First(i => i.Item1 == SearchEngines.Bing);
            Assert.That(bingResult.Item2, Contains.Item(Languages.Cobol));
            Assert.That(bingResult.Item2, Contains.Item(Languages.Dotnet));
        }

        [Test]
        public void TestWithEmptyResults()
        {
            Assert.IsEmpty(statisticsService.GetWinnersByEngine(new SearchResult[0]));
        }

        private static List<SearchResult> GetResultsWithSingleWinners()
        {
            return new List<SearchResult>
            {
                new SearchResult { Count = 2, Source = SearchEngines.Google, Term = Languages.Dotnet },
                new SearchResult { Count = 5, Source = SearchEngines.Google, Term = Languages.Java },
                new SearchResult { Count = 1, Source = SearchEngines.Google, Term = Languages.Cobol },
                new SearchResult { Count = 4, Source = SearchEngines.Bing, Term = Languages.Java },
                new SearchResult { Count = 5, Source = SearchEngines.Bing, Term = Languages.Cobol }
            };
        }

        private static List<SearchResult> GetResultsWithMultipleWinners()
        {
            return new List<SearchResult>
            {
                new SearchResult { Count = 2, Source = SearchEngines.Google, Term = Languages.Dotnet },
                new SearchResult { Count = 5, Source = SearchEngines.Google, Term = Languages.Java },
                new SearchResult { Count = 1, Source = SearchEngines.Google, Term = Languages.Cobol },
                new SearchResult { Count = 4, Source = SearchEngines.Bing, Term = Languages.Java },
                new SearchResult { Count = 5, Source = SearchEngines.Bing, Term = Languages.Dotnet },
                new SearchResult { Count = 5, Source = SearchEngines.Bing, Term = Languages.Cobol }
            };
        }

        private static List<SearchResult> GetSingleWinnerResults()
        {
            return new List<SearchResult>
            {
                new SearchResult { Count = 2, Source = SearchEngines.Google, Term = Languages.Dotnet },
                new SearchResult { Count = 5, Source = SearchEngines.Google, Term = Languages.Java },
                new SearchResult { Count = 1, Source = SearchEngines.Google, Term = Languages.Cobol },
                new SearchResult { Count = 4, Source = SearchEngines.Bing, Term = Languages.Java },
                new SearchResult { Count = 5, Source = SearchEngines.Bing, Term = Languages.Dotnet }
            };
        }

        private static List<SearchResult> GetMultipleWinnerResults()
        {
            return new List<SearchResult>
            {
                new SearchResult { Count = 12, Source = SearchEngines.Google, Term = Languages.Dotnet },
                new SearchResult { Count = 5, Source = SearchEngines.Google, Term = Languages.Java },
                new SearchResult { Count = 1, Source = SearchEngines.Google, Term = Languages.Cobol },
                new SearchResult { Count = 4, Source = SearchEngines.Bing, Term = Languages.Java },
                new SearchResult { Count = 5, Source = SearchEngines.Bing, Term = Languages.Dotnet },
                new SearchResult { Count = 8, Source = SearchEngines.Yandex, Term = Languages.Java },
                new SearchResult { Count = 5, Source = SearchEngines.Yandex, Term = Languages.Cobol }
            };
        }


    }
}