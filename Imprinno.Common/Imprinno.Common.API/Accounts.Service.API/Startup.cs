using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounts.Service.API.Helpers;
using Accounts.Service.API.Services;
using Imprinno.DataAccess;
using Imprinno.DataAccess.Repositories.Interface;
using Imprinno.DataAccess.Repositories.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.Service.API
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
            // configure strongly typed settings object
            services.Configure<TokensSettings>(Configuration.GetSection("TokensSettings"));

            // "recreate" the strongly typed settings and manually bind them
            var tokensSettings = new TokensSettings();
            Configuration.GetSection("TokensSettings").Bind(tokensSettings);

            var connection = Configuration.GetConnectionString("ImprinnoDatabase");
            services.AddDbContextPool<EntitiesDbContext>(options => options.UseNpgsql(connection, b => b.MigrationsAssembly("Accounts.Service.API")));

            services.AddSingleton<EntitiesDbContext>();

            // configure DI for application repositories
            services.AddScoped<IAccountsRepository, AccountsRepository>();

            // services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // configure DI for application services
            services.AddScoped<IAccountsService, AccountsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
