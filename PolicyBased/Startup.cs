using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PolicyBased
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddRazorRuntimeCompilation();

            services.AddAuthorization(options =>
            {
                var userPermissions = new List<UserPermission> {
                              new UserPermission {
                                  Url="/",
                                  UserName="gsw"
                              },
                              new UserPermission {
                                  Url="/home/permissionadd",
                                  UserName="gsw"
                              },
                              new UserPermission {
                                  Url="/",
                                  UserName="aaa"
                              },
                              new UserPermission {
                                  Url="/home/contact",
                                  UserName="aaa"
                              }
                          };

                options.AddPolicy("Permission", policy =>
                {
                    policy.Requirements.Add(new PermissionRequirement("/home/denied", userPermissions));
                });
            })
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/home/login");
                    options.AccessDeniedPath = new PathString("/home/denied");
                });

            //×¢Èëhandler
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
