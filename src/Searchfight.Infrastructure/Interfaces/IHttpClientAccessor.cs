using System.Net.Http;

namespace Searchfight.Infrastructure.Interfaces
{
    public interface IHttpClientAccessor
    {
        HttpClient Client { get; }
    }
}
