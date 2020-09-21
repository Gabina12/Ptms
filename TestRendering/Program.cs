using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using Nustache.Core;

namespace TestRendering
{
	class Program
	{
		static void Main(string[] args)
		{

			var stop = new Stopwatch();
			stop.Start();

			HttpClient http = new HttpClient();
			http.BaseAddress = new Uri("https://localhost:5001/");
			var todoItem = new { name = $"lasha" };

			Parallel.For(0, 1001, (state, a) => {
				var todoItemJson = new StringContent(JsonSerializer.Serialize(todoItem), Encoding.UTF8, "application/json");


				using var httpResponse =
					 http.PostAsync("/api/templates/49ec99c45cc8ee0a0b766eb57b001cb7/render/txt", todoItemJson).Result;

				httpResponse.EnsureSuccessStatusCode();
				Console.WriteLine(httpResponse.Content.ReadAsStringAsync().Result);
			});


			stop.Stop();
			Console.WriteLine($"elipse: {stop.ElapsedMilliseconds}");
			Console.ReadKey();

		}

	}


}
