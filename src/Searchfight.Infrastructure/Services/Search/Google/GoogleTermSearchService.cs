using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System;
using System.Web;
using Searchfight.Infrastructure.Interfaces;
using System.IO;

namespace Searchfight.Infrastructure.Services.Search.Google
{
    public class GoogleTermSearchService : AbstractTermSearchService
    {
        private const string ApiKey = "key";
        private const string EngineIdKey = "cx";
        private const string QueryKey = "q";

        private readonly GoogleConfig config;
        protected override string SearchEngineName => "Google";

        public GoogleTermSearchService(IOptions<GoogleConfig> configOption, IHttpClientAccessor clientAccessor)
            :base(clientAccessor.Client)
        {
            config = configOption.Value;
        }

        protected override HttpRequestMessage CreateRequestMessage(string term)
        {
            var uriBuilder = new UriBuilder(config.Url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[ApiKey] = config.ApiKey;
            query[EngineIdKey] = config.EngineId;
            query[QueryKey] = term;
            uriBuilder.Query = query.ToString();
            return new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
        }

        protected override async Task<decimal> GetCountFromResponseAsync(Stream responseStream)
            => (await JsonSerializer.DeserializeAsync<GoogleResult>(responseStream)).Statistics.Count;
    }
}
