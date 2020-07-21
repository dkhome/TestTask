using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Searchfight.Domain.Interfaces;
using Searchfight.Infrastructure.Interfaces;
using Searchfight.Infrastructure.Services.Search.Google;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Searchfight.Infrastructure.Tests
{
    public class GoogleTermSearchServiceTests
    {
        public ITermSearchService SetupService(HttpStatusCode code, GoogleResult result)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   Content = new StringContent(JsonSerializer.Serialize(result)),
                   StatusCode = code,
               });

            var mockedHttpClient = new HttpClient(handlerMock.Object);

            var accessor = new Mock<IHttpClientAccessor>();
            accessor.SetupGet(m => m.Client).Returns(mockedHttpClient);
            var options = Options.Create(
                new GoogleConfig { ApiKey = "key", EngineId = "12", Url = "http://google.com/" });

            return new GoogleTermSearchService(options, accessor.Object);
        }

        [Test]
        public async Task TestHappyFlow()
        {
            var service = SetupService(HttpStatusCode.OK, GetDefaultResult());
            var result = await service.GetResultsCountAsync("java");
            Assert.AreEqual(12, result.Count);
            Assert.AreEqual("java", result.Term);
            Assert.AreEqual("Google", result.Source);
        }

        [Test]
        public void TestHttpError()
        {
            var term = "java";
            var service = SetupService(HttpStatusCode.NotFound, GetDefaultResult());
            var exception = Assert.ThrowsAsync<Exception>(async () => await service.GetResultsCountAsync(term));
            Assert.AreEqual($"Search failed on Google for {term} with HTTP error status: {HttpStatusCode.NotFound}.",
                exception.Message);
        }

        private static GoogleResult GetDefaultResult()
        {
            return new GoogleResult { Statistics = new GoogleResult.SearchStatistics { Count = 12m } };
        }

    }
}