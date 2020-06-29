using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStartupFilter>(new StartupFilter<Foo>());
            services.AddSingleton<IStartupFilter>(new StartupFilter<Bar>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<Baz>();
            app.UseMiddleware<Gux>();
            app.Run(async ctx =>
            {
                await ctx.Response.WriteAsync("End");
            });
        }
    }

}
