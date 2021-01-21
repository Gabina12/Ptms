using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bondx.Extention.Redis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PTMS.API.Controllers;
using PTMS.API.Helpers;
using PTMS.Core.Models;
using PTMS.Core.Repositories;
using PTMS.Core.Services.Implementations;
using PTMS.Core.Services.Implementations.DistributedCacheImpl;
using PTMS.Core.Services.Interfaces;
using PTMS.Infrastructure;

namespace PTMS.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder => {
				builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					//.AllowAnyOrigin()
					.WithOrigins("http://localhost:8080", "https://ptms.01.test.lb.ge")
					.AllowCredentials();
			}));

			services.AddDbContext<Infrastructure.Postgre.Data.PtmsDataStore>(options =>
			{
				options.UseNpgsql(Configuration.GetConnectionString("ptms"), sql => sql.MigrationsAssembly(typeof(Infrastructure.Postgre.Data.PtmsDataStore).Assembly.FullName));
			});

			services.AddAuthentication("BasicAuthentication")
				.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

			services.Configure<StoreDatabaseSettings>(Configuration.GetSection(nameof(StoreDatabaseSettings)));
			services.AddSingleton<IStoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<StoreDatabaseSettings>>().Value);

			services.AddSingleton<ICategoryRepository, CategoryRepository>();
			services.AddSingleton<ITemplateRepository, TemplateRepository>();
			services.AddSingleton<IPartialsRepository, PartialsRepository>();
			services.AddSingleton<IUserRepository, UserRepository>();
			services.AddSingleton<ITemplateService, TemplateService>();
			services.AddSingleton<IPartialsService, PartialsService>();
			services.AddScoped<IUserService, UserService>();
			services.AddSingleton<IRenderingService, RenderingService>();

            if (Configuration.GetSection("UseRedis").Get<bool>()) {
				services.AddSingleton(typeof(IGeneralCache<>), typeof(RedisGeneralCache<>));
				services.AddSingleton(typeof(ICache<>), typeof(RedisCache<>));
				services.AddRedisStore(Configuration, "Redis");
				services.AddSingleton<IRedisStore, RedisStore>();
            }else {
				services.AddSingleton(typeof(IGeneralCache<>), typeof(MemoryGeneralCache<>));
				services.AddSingleton(typeof(ICache<>), typeof(MemCache<>));
				services.AddMemoryCache();
			}
			
			services.AddControllers()
				.AddNewtonsoftJson();


			services.AddSwaggerGen();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			app.UseCors("ApiCorsPolicy");
			app.UseHttpsRedirection();

			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});


			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});
		}
	}
}
