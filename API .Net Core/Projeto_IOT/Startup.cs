using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Projeto_IOT.Context;
using Projeto_IOT.Helper;
using Projeto_IOT.Repository;
using Projeto_IOT.Repository.IRepository;
using Projeto_IOT.Services;
using Projeto_IOT.Services.IServices;
using SendGrid;

namespace Projeto_IOT
{
    public class Startup
    {
        public Startup (IHostingEnvironment env, IConfiguration configuration) {
            var builder = new ConfigurationBuilder ()
                .SetBasePath (env.ContentRootPath)
                .AddJsonFile ("appsettings.json", optional : true, reloadOnChange : true)
                .AddJsonFile ($"appsettings.{env.EnvironmentName}.json", optional : true)
                .AddEnvironmentVariables ();
            Configuration = builder.Build ();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString ("Default");
            AppDbContext.ConnectionString = connectionString;

            services.AddMemoryCache ();
            services.AddOptions ();
            services.Configure<Config> (Configuration);
            services.AddSingleton<IConfiguration> (Configuration);

            services.AddDbContext<AppDbContext> ( a => 
                a.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        
            services.AddSingleton<ISendGridClient> (provider => new SendGridClient (Configuration["SendGridApiKey"]));
            services.AddTransient<IEmail, Email> ();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAlertaRepository, AlertaRepository>();


            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAlertaService, AlertaService>();

            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
