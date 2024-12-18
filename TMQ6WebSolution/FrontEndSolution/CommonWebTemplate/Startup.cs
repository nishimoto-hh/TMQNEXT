using CommonWebTemplate.CommonUtil;
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
using Microsoft.Extensions.Hosting.Internal;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CommonWebTemplate.CommonDefinitions;
using Microsoft.Extensions.Caching.Memory;

namespace CommonWebTemplate
{
    public class Startup
    {
        private static CommonLogger logger = CommonLogger.GetInstance();
        private static CommonMemoryData comMemoryData = CommonMemoryData.GetInstance();

        public Startup(IConfiguration configuration)
        {
            AppCommonObject.SetConfiguration(configuration);
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var defaultConfig = AppCommonObject.Config.ConnectionSettings.GetDefaultConnectionStringSetting();
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

            // シングルサインオン機能の有効化
            if (AppCommonObject.Config.AppSettings.AzureADLogin)
            {
                services.AddAuthentication("scheme")
                    .AddSaml2(options =>
                    {
                        options.SPOptions.EntityId = new EntityId(AppCommonObject.Config.AppSettings.TMQEntityId);
                        options.SPOptions.PublicOrigin = new Uri(AppCommonObject.Config.AppSettings.TMQPublicOrigin);

                        //使用する証明書の設定（本来はローカルファイルではなく証明書ストアを利用する）
                        //options.SPOptions.ServiceCertificates.Add(new X509Certificate2("stubidp.sustainsys.com.cer"));

                        //Idp設定
                        IdentityProvider idp = new IdentityProvider(
	                        new EntityId(AppCommonObject.Config.AppSettings.AzureADEntityId), options.SPOptions)
	                        {
	                            LoadMetadata = true,
	                            SingleSignOnServiceUrl = new Uri(AppCommonObject.Config.AppSettings.AzureADSingleSignOnServiceUrl),
	                            SingleLogoutServiceUrl = new Uri(AppCommonObject.Config.AppSettings.AzureADSingleLogoutServiceUrl),
	                            MetadataLocation = AppCommonObject.Config.AppSettings.AzureADMetadataLocation,
	                        };
	                        options.IdentityProviders.Add(idp);
                    }
                ).AddCookie("scheme");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery, IMemoryCache cache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                app.UseExceptionHandler("/Home");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.Use(async (context, next) =>
            //{
            //    string path = context.Request.Path.Value;
            //    if (path != null && path.ToLower().Contains("/api"))
            //    {
            //        // WebAPIのリクエストの場合、偽造防止トークンをCookieへセット
            //        var tokens = antiforgery.GetAndStoreTokens(context);
            //        context.Response.Cookies.Append("XSRF-TOKEN",
            //          tokens.RequestToken, new CookieOptions
            //          {
            //              HttpOnly = false,
            //              Secure = true
            //          }
            //        );
            //    }
            //    await next();
            //});

            app.UseAuthentication();
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

            //app.UseStatusCodePagesWithRedirects("~/Error?code={0}");
            app.UseStatusCodePagesWithRedirects("~/Home/Error?code={0}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{key?}");

                //endpoints.MapHub<CommonWebTemplate.PushHub>("/push");

            });

            //★インメモリ化対応 start
            // 共通定義データの取得
            getCommonDefineData();
            //★インメモリ化対応 end
        }

        //★インメモリ化対応 start
        /// <summary>
        /// 共通定義データの取得
        /// </summary>
        private void getCommonDefineData()
        {
            try
            {
                BusinessLogicIO blogic = new BusinessLogicIO(new CommonProcData());
                var result = blogic.CallDllBusinessLogic_GetCommonDefineInfo(out Dictionary<string, object> defines);
                if (result.STATUS != 0 || defines.Count == 0) { return; }

                foreach (var define in defines)
                {
                    comMemoryData.SetData(define.Key, define.Value);
                }

            }
            catch (Exception ex)
            {
                logger.WriteLog(ex.ToString());
            }
        }
        //★インメモリ化対応 end

    }
}
