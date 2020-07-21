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
            var services = new ServiceCollection();
            
            SetUpConfiguration(services);

            return services.AddSingleton<ApplicationController>()
                .AddSingleton<IHttpClientAccessor, DefaultHttpClientAccessor>()
                .AddSingleton<IInputValidator, InputValidator>()
                .AddTransient<ITermSearchService, GoogleTermSearchService>()
                .AddTransient<ITermSearchService, BingTermSearchService>()
                .AddTransient<ISearchStatisticsService, SearchStatisticsService>()
                .AddTransient<IStatisticsService, StatisticsService>()
                .AddTransient<ISearchStatisticsPresenter, SearchStatisticsConsolePresenter>();
        }

        private static void SetUpConfiguration(IServiceCollection services)
        {
            var configuration = GetConfiguration();

            services
                .AddOptions()
                .Configure<GoogleConfig>(configuration.GetSection(nameof(GoogleConfig)))
                .Configure<BingConfig>(configuration.GetSection(nameof(BingConfig)));
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
