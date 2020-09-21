using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TPCM.Core.Models;
using TPCM.Core.Repositories;
using TPCM.Core.Services.Interfaces;

namespace TPCM.API {
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			using (var scope = host.Services.CreateScope()) {
				var services = scope.ServiceProvider;
				var users = services.GetRequiredService<IUserRepository>();
				await users.Migrate();
				var templates = services.GetService<IGeneralCache<Template>>();
				var templatesRepo = services.GetService<ITemplateRepository>();
				await templates.Restore(templatesRepo);
                var _cache = services.GetService<ICache<PartialCacheItem>>();
                var partialsRepo = services.GetService<IPartialsRepository>();
				var items = await partialsRepo.Get();
                foreach (var item in items) {
					await _cache.Appand("partials", new PartialCacheItem {
						Id = item.Id,
						Template = item.TemplateBody
					});

				}
            }
			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			.ConfigureLogging(logging =>
			{
				logging.ClearProviders();
				logging.AddConsole();
			})
				.ConfigureWebHostDefaults(webBuilder => {

					webBuilder.UseStartup<Startup>();
				});
	}
}
