using System.Net;

namespace Searchfight.Infrastructure.Services.Search.Utils
{
    public static class ErrorMessageHelper
    {
        public static string CreateResponseStatusErrorMessage(string searchEngine, string term, HttpStatusCode httpStatusCode)
            => $"Search failed on {searchEngine} for {term} with HTTP error status: {httpStatusCode}.";
    }
}
