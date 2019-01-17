using APIGround.Context;
using APIGround.Entity;
using APIGround.Models.Read;
using APIGround.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;


namespace APIGround
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o =>
                {
                    o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                })
                .AddJsonOptions(o=> {
                    if (o.SerializerSettings.ContractResolver != null)
                    {
                        var castResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
                        castResolver.NamingStrategy = null;
                    }
                });

            services.AddDbContext<CityContext>(o => o.UseSqlServer(Configuration.GetConnectionString("CityDatabase")));
            services.AddScoped<ICityRepository, CityRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            //Adding NLog
            loggerFactory.AddNLog(); //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }
            //app.UseStatusCodePages();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<City, LimitedCityInfoDTO>();
            });
            app.UseMvc();
        }
    }
}
