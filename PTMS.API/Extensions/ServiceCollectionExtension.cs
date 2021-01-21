using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PTMS.API.Helpers;
using PTMS.Core.Models;
using PTMS.Core.Repositories;
using PTMS.Core.Services.Implementations;
using PTMS.Core.Services.Interfaces;
using PTMS.Infrastructure;
using PTMS.Infrastructure.Postgre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTMS.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPtmsModulesWithMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.Configure<StoreDatabaseSettings>(configuration.GetSection(nameof(StoreDatabaseSettings)));
            services.AddSingleton<IStoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<StoreDatabaseSettings>>().Value);

            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<ITemplateRepository, TemplateRepository>();
            services.AddSingleton<IPartialsRepository, PartialsRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ITemplateService, TemplateService>();
            services.AddSingleton<IPartialsService, PartialsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IRenderingService, RenderingService>();
            return services;
        }

        public static IServiceCollection AddPtmsModulesWithPostgre(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Infrastructure.Postgre.Data.PtmsDataStore>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ptms"), sql => sql.MigrationsAssembly(typeof(Infrastructure.Postgre.Data.PtmsDataStore).Assembly.FullName));
            });

            services.AddScoped<ICategoryRepository, CategoryPostgreRepository>();
            services.AddScoped<ITemplateRepository, TemplatePostgreRepository>();
            services.AddScoped<IPartialsRepository, PartialsPostgreRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IPartialsService, PartialsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRenderingService, RenderingService>();

            return services;
        }
    }
}
