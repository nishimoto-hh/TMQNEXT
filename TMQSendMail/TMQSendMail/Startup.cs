using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMQSendMail
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            CommonWebTemplate.AppCommonObject.SetConfiguration(configuration);
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var defaultConfig = CommonWebTemplate.AppCommonObject.Config.ConnectionSettings.GetDefaultConnectionStringSetting();
            switch (defaultConfig.Name)
            {
                case "SqlServer":
                    services.AddDbContext<CommonDataEntities>(option =>
                    {
                        option.UseSqlServer(defaultConfig.ConnectionString);
                    });
                    break;
                case "PostgreSql":
                    services.AddDbContext<CommonDataEntities>(option =>
                    {
                        option.UseNpgsql(defaultConfig.ConnectionString);
                    });
                    break;
                case "Oracle":
                    services.AddDbContext<CommonDataEntities>(option =>
                    {
                        option.UseOracle(defaultConfig.ConnectionString);
                    });
                    break;
            }

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.MaxAge = TimeSpan.FromHours(12);
                options.Cookie.IsEssential = true;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            // ↓↓ソリューションを1つに統合する場合はこちら↓↓
            services.AddControllersWithViews()
                    .AddSessionStateTempDataProvider()
                    .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);
            // ↑↑ソリューションを1つに統合する場合はこちら↑↑

            // ↓↓フロントエンドのソリューションを分離する場合はこちら↓↓
            //var frontAssembly = typeof(CommonFrontTemplate.Startup).Assembly;
            //services.AddControllersWithViews()
            //        .AddApplicationPart(frontAssembly)
            //        .AddSessionStateTempDataProvider();
            // ↑↑フロントエンドのソリューションを分離する場合はこちら↑↑

            //↓↓.NET5版ではプッシュ通知は未実装↓↓
            //services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                string path = context.Request.Path.Value;
                if (path != null && path.ToLower().Contains("/api"))
                {
                    // WebAPIのリクエストの場合、偽造防止トークンをCookieへセット
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN",
                      tokens.RequestToken, new CookieOptions
                      {
                          HttpOnly = false,
                          Secure = true
                      }
                    );
                }
                await next();
            });

            app.UseAuthorization();
            app.UseSession();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                // 常にCookieのSecure属性を有効に
                Secure = CookieSecurePolicy.Always,
                // CookieのSameSite属性:None=別ドメインからの遷移時にもCookieを有効とする
                MinimumSameSitePolicy = SameSiteMode.None,
                HttpOnly = HttpOnlyPolicy.Always
            });

            app.UseStatusCodePagesWithRedirects("~/Error?code={0}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{key?}");

                //endpoints.MapHub<CommonWebTemplate.PushHub>("/push");

            });
        }
    }
}
