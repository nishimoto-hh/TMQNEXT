using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMQDTCls = TMQSendMail.TMQSendMailDataClass;
//using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;

namespace TMQSendMail
{
    class program
    {
        public IConfiguration Configuration { get; }

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            Console.WriteLine("TMQSendMail Start");

            try
            {
                IConfigurationBuilder config = new ConfigurationBuilder();

                config.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                config.AddJsonFile("connectionsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile("connectionsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                      .AddJsonFile("logsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile("logsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile("appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();
                Startup sUp = new Startup(config.Build());

            }

            catch
            {
                Console.WriteLine("ファイル読み込み時に例外が発生しました。");
            }

            // メール送信処理
            SendMailUtil sendMUtil = new SendMailUtil();

            Console.WriteLine("TMQSendMail End");
            //Console.WriteLine("キーを押すと終了します");
            //Console.ReadKey();

        }
    }
}
