using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System;
using System.Web;
using Searchfight.Infrastructure.Interfaces;

namespace Searchfight.Infrastructure.Services.Search.Google
{
    public class GoogleTermSearchService : AbstractTermSearchService
    {
        private readonly HttpClient httpClient;
        private readonly GoogleConfig config;

        protected override string EngineName => "Google";

        public GoogleTermSearchService(IOptions<GoogleConfig> configOption, IHttpClientAccessor clientAccessor)
        {
            config = configOption.Value;
            httpClient = clientAccessor.Client;
        }

        protected override async Task<decimal> GetCountAsync(HttpRequestMessage message)
        {
            var responseMessage = await httpClient.SendAsync(message);
            var queryStream = await responseMessage.Content.ReadAsStreamAsync();
            var searchResult = await JsonSerializer.DeserializeAsync<GoogleResult>(queryStream);
            return searchResult.Statistics.Count;
        }

        protected override HttpRequestMessage CreateSearchMessage(string term)
        {
            var uriBuilder = new UriBuilder(config.Url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["key"] = config.ApiKey;
            query["cx"] = config.EngineId;
            query["q"] = term;
            uriBuilder.Query = query.ToString();
            return new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
        }
    }
}
