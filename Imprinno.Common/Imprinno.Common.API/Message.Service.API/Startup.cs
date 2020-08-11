using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imprinno.DataAccess.Repositories.Interface;
using Imprinno.DataAccess.Repositories.Repository;
using Message.Service.API.Helpers;
using Message.Service.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Message.Service.API
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
            services.AddScoped<IMessageRepository, MessageRepository>();

            services.AddControllers();

            // configure strongly typed settings object
            services.Configure<MessageSettings>(Configuration.GetSection("MessageSettings"));
            services.Configure<SendGridSettings>(Configuration.GetSection("SendGridSettings"));
            services.Configure<TwilioSettings>(Configuration.GetSection("TwilioSettings"));

            // configure DI for application services
            services.AddScoped<IMessageService, MessageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
