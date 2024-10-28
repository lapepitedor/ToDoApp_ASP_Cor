using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using ToDoApp.Misc;
using ToDoApp.Models;


namespace ToDoApp
{
    public class Startup
    {
        private IConfiguration config;
        public Startup(IConfiguration configuration) { this.config = configuration; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MailSettings>(options => config.GetSection("MailSettings").Bind(options));
            services.Configure<GeneralSettings>(options => config.GetSection("GeneralSettings").Bind(options));
            services.AddSingleton<IToDoRepository, ToDoMemoryRepository>();
            services.AddSingleton<IUserRepository, UserMemoryRepository>();
            services.AddSingleton<PasswordHelper>();
            services.AddSingleton<EmailQueue>();
            services.AddSingleton<EmailService>();
            services.AddHostedService<BackgroundEmailSender>();

            services.AddMvc();
            services.AddProblemDetails();
            services.AddAuthentication(options =>
            {
                 options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
            options.LoginPath = "/Authentication/Login";
            options.AccessDeniedPath = "/Authentication/Login";
             });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // app.UseExceptionHandler();

            app.UseStaticFiles();
            app.UseRouting();
            

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Authentication}/{action=Login}/{id?}");
            });
            ;
        }
    }
}
