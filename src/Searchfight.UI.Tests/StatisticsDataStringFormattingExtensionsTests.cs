using NUnit.Framework;
using Searchfight.Core;
using Searchfight.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Searchfight.UI.Tests
{
    public class StatisticsDataStringFormattingExtensionsTests
    {
        [Test]
        public void TotalWinnersWithEmptyData()
        {
            var winners = new List<string>();
            Assert.That(winners.FormatTotalWinnersToString(), Is.Empty);
        }

        [Test]
        public void TotalWinnersWithNull()
        {
            List<string> winners = null;
            Assert.That(winners.FormatTotalWinnersToString(), Is.Empty);
        }

        [Test]
        public void sSingleWinnerIsSingle()
        {
            var winners = new List<string> { "Winner" };
            Assert.AreEqual($"Total winner: Winner", winners.FormatTotalWinnersToString());
        }

        [Test]
        public void MultipleTotalWinnersIsPlural()
        {
            var winners = new List<string> { "Winner", "Looser" };
            Assert.AreEqual($"Total winners: Winner, Looser", winners.FormatTotalWinnersToString());
        }

        [Test]
        public void SearchEngineWinnersWithEmptyData()
        {
            var winners = new List<Tuple<string, IEnumerable<string>>>();
            Assert.That(winners.FormatWinnersBySearchEngineToString(), Is.Empty);
        }

        [Test]
        public void SearchEngineWinnersNullData()
        {
            List<Tuple<string, IEnumerable<string>>> winners = null;
            Assert.That(winners.FormatWinnersBySearchEngineToString(), Is.Empty);
        }

        [Test]
        public void SearchEngineWinnersWithSingleWinner()
        {
            var winners = new List<Tuple<string, IEnumerable<string>>>
            { 
                new Tuple<string, IEnumerable<string>>("Google", new List<string>{ ".net" })
            };
            Assert.AreEqual("Google winner: .net", winners.FormatWinnersBySearchEngineToString());
        }

        [Test]
        public void SearchEngineWinnersWithMultipleWinners()
        {
            var winners = new List<Tuple<string, IEnumerable<string>>>
            {
                new Tuple<string, IEnumerable<string>>("Google", new List<string>{ ".net", "java" })
            };
            Assert.AreEqual("Google winners: .net, java", winners.FormatWinnersBySearchEngineToString());
        }

        [Test]
        public void SearchEngineWinnersCombination()
        {
            var winners = new List<Tuple<string, IEnumerable<string>>>
            {
                new Tuple<string, IEnumerable<string>>("Google", new List<string>{ ".net", "java" }),
                new Tuple<string, IEnumerable<string>>("Bing", new List<string>{ "cobol" })
            };
            Assert.AreEqual($"Google winners: .net, java{Environment.NewLine}Bing winner: cobol", winners.FormatWinnersBySearchEngineToString());
        }

        [Test]
        public void TermWinnersWithEmptyData()
        {
            var winners = new List<IGrouping<string, SearchResult>>();
            Assert.That(winners.FormatStatisticsByTermToString(), Is.Empty);
        }

        [Test]
        public void TermWinnersWithNull()
        {
            List<IGrouping<string, SearchResult>> winners = null;
            Assert.That(winners.FormatStatisticsByTermToString(), Is.Empty);
        }

        [Test]
        public void TermWinners()
        {
            var list = new List<SearchResult>
            {
                new SearchResult {Term = "C#", Source = "Google", Count = 23},
                new SearchResult {Term = "C#", Source = "Bing", Count = 2},
            };
            Assert.AreEqual("C#: Google: 23 Bing: 2", list.GroupBy(sr => sr.Term).FormatStatisticsByTermToString());
        }
    }
}