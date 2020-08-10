using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BudgetIn.SharedKernel.Interfaces;
using BudgetIn.Infrastructure.Data;
using BudgetIn.SharedKernel;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;

namespace BudgetIn.API
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
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DatabaseConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>();


            // Возврат ошибки 401 вместо редиректа на страницу Login при попытке доступа неавторизовавшись
            // https://stackoverflow.com/questions/45270467/what-is-the-replacement-for-cookies-applicationcookie-automaticchallenge-false

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;

                    return Task.CompletedTask;
                };
            });

            services.AddControllersWithViews()
                .AddControllersAsServices();
            services.AddRazorPages();

            services.AddSwaggerGen();

            // dependencies
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILogoRepository, LogoRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // SWAGGER
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BudgetIn API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseCors();

            // SWAGGER
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BudgetIn API V1");
            });

            //app.UseCors(pol => pol.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); // Добавил для возврата 401 вместо редиректа при unauth
            app.UseAuthentication();
            app.UseAuthorization();

            // Добавил для возврата 401 вместо редиректа при unauth
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
                {
                    if (!context.HttpContext.Response.ContentLength.HasValue || context.HttpContext.Response.ContentLength == 0)
                    {
                        // You can change ContentType as json serialize
                        context.HttpContext.Response.ContentType = "text/plain";
                        await context.HttpContext.Response.WriteAsync($"Status Code: {context.HttpContext.Response.StatusCode}");
                    }
                }
                else
                {
                    // You can ignore redirect
                    context.HttpContext.Response.Redirect($"/error?code={context.HttpContext.Response.StatusCode}");
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

        }
    }
}
