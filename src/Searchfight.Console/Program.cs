using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Searchfight.Infrastructure.Interfaces;
using Searchfight.Infrastructure.Services;
using Searchfight.Infrastructure.Services.Search.Google;
using Searchfight.Infrastructure.Services.Search.Bing;
using Searchfight.Domain.Interfaces;
using Searchfight.Domain.Services;
using Searchfight.Domain.Validators;
using Searchfight.Domain.Statistics.Interfaces;
using Searchfight.Domain.Statistics.Services;
using Searchfight.Domain;
using Searchfight.UI;

namespace Searchfight
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await ConfigureServices()
                .BuildServiceProvider()
                .GetService<ApplicationController>()
                .Run(args);
        }

        private static IServiceCollection ConfigureServices()
        {
            //TODO: check is var needed
            var configuration = GetConfiguration();

            return new ServiceCollection()
                .AddOptions()

                .Configure<GoogleConfig>(configuration.GetSection(nameof(GoogleConfig)))

                .AddSingleton<ApplicationController>()
                .AddSingleton<IHttpClientAccessor, DefaultHttpClientAccessor>()

                //TODO: I don't think we need all singletones here
                .AddSingleton<IInputValidator, InputValidator>()
                .AddSingleton<ITermSearchService, GoogleTermSearchService>()
                .AddSingleton<ITermSearchService, BingTermSearchService>()
                .AddSingleton<ISearchStatisticsService, SearchStatisticsService>()
                .AddSingleton<IStatisticsService, StatisticsService>()
                .AddSingleton<ISearchStatisticsPresenter, SearchStatisticsConsolePresenter>();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }
    }
}
