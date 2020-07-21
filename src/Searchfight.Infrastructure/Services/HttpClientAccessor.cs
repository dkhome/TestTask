using Searchfight.Infrastructure.Interfaces;
using System.Net.Http;

namespace Searchfight.Infrastructure.Services
{
    public class DefaultHttpClientAccessor : IHttpClientAccessor
    {
        public HttpClient Client { get; }

        public DefaultHttpClientAccessor()
        {
            Client = new HttpClient();
        }
    }
}
