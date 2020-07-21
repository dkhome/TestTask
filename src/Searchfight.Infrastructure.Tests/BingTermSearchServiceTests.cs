using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Searchfight.Domain.Interfaces;
using Searchfight.Infrastructure.Interfaces;
using Searchfight.Infrastructure.Services.Search.Bing;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Searchfight.Infrastructure.Tests
{
    public class BingTermSearchServiceTests
    {
        public class GoogleTermSearchServiceTests
        {
            public ITermSearchService SetupService(HttpStatusCode code, BingResult result)
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

                var options = Options.Create(new BingConfig {  Key = "1234", Url = "http://bing.com/" });

                return new BingTermSearchService(options, accessor.Object);
            }

            [Test]
            public async Task TestHappyFlow()
            {
                var term = ".net";
                var service = SetupService(HttpStatusCode.OK, DefaultResult());
                var result = await service.GetResultsCountAsync(term);
                Assert.AreEqual(12, result.Count);
                Assert.AreEqual(term, result.Term);
                Assert.AreEqual("Bing", result.Source);
            }

            [Test]
            public void TestHttpError()
            {
                var term = "cobol";
                var service = SetupService(HttpStatusCode.NotFound, DefaultResult());
                var exception = Assert.ThrowsAsync<Exception>(async () => await service.GetResultsCountAsync(term));
                Assert.AreEqual($"Search failed on Bing for {term} with HTTP error status: {HttpStatusCode.NotFound}.",
                    exception.Message);
            }

            private static BingResult DefaultResult()
            {
                return new BingResult { Statistics = new BingResult.WebPageStatistics { Count = 12m } };
            }
        }
    }
}
