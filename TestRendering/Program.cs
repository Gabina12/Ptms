using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PTMS.Client.SDK;
using System.Collections.Generic;
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

			JsonElement el = new JsonElement();
			


			var todoItem = new ToDo { SMS_PAN = $"lasha", Details = new TodoDetails
			{
				Name = "vax vax ra Name",
				Title = "Title iyo aq",
				Items = new List<TodoDetailsItem>
                {
					new TodoDetailsItem{ Prop1 = "lasha", Prop2 = "gabinashvili"},
					new TodoDetailsItem{ Prop1 = "toro", Prop2 = "giorgidze"}
                }
			}
			};

			var ptms = serviceProvider.GetRequiredService<IPTMSClient>();
			var resp = await ptms.GetTemplate("v1", "bcac6f90-04d0-4345-ab6b-1ab2db7d3c93", todoItem);
  
            Console.WriteLine(resp);


			Console.ReadKey();

		}

	}

	public class ToDo
    {
        public string SMS_PAN { get; set; }
		public TodoDetails Details { get; set; }
    }

	public class TodoDetails
    {
        public string Name { get; set; }
        public string Title { get; set; }

		public List<TodoDetailsItem> Items { get; set; }
    }

	public class TodoDetailsItem
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
    }

}
