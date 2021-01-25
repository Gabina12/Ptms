using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PTMS.Client.SDK;
//using Nustache.Core;

namespace TestRendering
{
	class Program
	{
		private static IServiceProvider serviceProvider;
		private static IConfiguration configuration;

		static async Task Main(string[] args)
		{
			var builder = new ConfigurationBuilder()
			  .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false);

			configuration = builder.Build();
			var services = new ServiceCollection();
			services.AddOptions();

			services.AddPtmsClient(options =>
			{
				options.BaseAddress = new Uri("https://localhost:5001/");
			});

			serviceProvider = services.BuildServiceProvider();


			var todoItem = new ToDo { SMS_PAN = $"lasha" };

			var ptms = serviceProvider.GetRequiredService<IPTMSClient>();
			var resp = await ptms.GetTemplate("v1", "2d00f1bf-d22c-409b-8466-95f5a812832f", todoItem);

            Console.WriteLine(resp);


			Console.ReadKey();

		}

	}

	public class ToDo
    {
        public string SMS_PAN { get; set; }
    }


}
