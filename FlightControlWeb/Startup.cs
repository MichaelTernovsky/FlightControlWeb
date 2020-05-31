using System;
using System.Collections.Generic;
using FlightControlWeb.Model.ConcreteObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlightControlWeb
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
            services.AddRouting();
            services.AddControllers();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache cache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // creating the lists
            List<Server> serversList = new List<Server>();
            List<Flight> flightsList = new List<Flight>();
            Dictionary<string, FlightPlan> flightsDict = new Dictionary<string, FlightPlan>();
            Dictionary<string, string> flightSource = new Dictionary<string, string>();

            // adding the lists to the cache
            cache.Set("servers", serversList);
            cache.Set("flights", flightsList);
            cache.Set("flightsDict", flightsDict);
            cache.Set("flightSource", flightSource);
        }
    }
}