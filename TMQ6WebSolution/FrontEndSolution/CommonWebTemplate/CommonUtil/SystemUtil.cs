///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　システム操作クラス
/// 説明　　　：　システム操作の共通処理を実装します。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonWebTemplate.CommonUtil
{
    public class SystemUtil
    {
        #region === 定数定義 ===
        ///<summary>ContentType(Excelﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ)</summary>
        public static class ContentType
        {
            public const string ExcelFile = @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public const string ExcelonMacro = @"application/vnd.ms-excel.sheet.macroEnabled.12";
            public const string CsvFile = @"text/csv";
            public const string PdfFile = @"application/pdf";
            public const string ZipFile = @"application/zip";
        }
        #endregion

        #region === public static 処理 ===
        /// <summary>
        /// ﾌｧｲﾙ拡張子からContentTypeを取得
        /// </summary>
        /// <param name="filePath">ﾌｧｲﾙ名、またはﾌｧｲﾙﾊﾟｽ</param>
        /// <returns></returns>
        public static string GetContentType(string filePath)
        {
            //ﾌｧｲﾙ拡張子を取得
            string fileExt = FileUtil.GetFileExtension(filePath);
            // - 『.』を除去
            fileExt = fileExt.TrimStart(@".".ToArray());
            // - 小文字に変換
            fileExt = fileExt.ToUpper();

            if (FileUtil.GetExtensionStr(FileUtil.Extension.ExcelFile).ToUpper().Equals(fileExt))
            {
                //Excelﾌﾞｯｸ(.xlsx)の場合
                return ContentType.ExcelFile;
            }
            else if (FileUtil.GetExtensionStr(FileUtil.Extension.ExcelonMacro).ToUpper().Equals(fileExt))
            {
                //Excelﾏｸﾛ有効ﾌﾞｯｸ(.xlsm)の場合
                return ContentType.ExcelonMacro;
            }
            else if (FileUtil.GetExtensionStr(FileUtil.Extension.Csv).ToUpper().Equals(fileExt))
            {
                //CSVﾌｧｲﾙ(.csv)の場合
                return ContentType.CsvFile;
            }
            else if (FileUtil.GetExtensionStr(FileUtil.Extension.Pdf).ToUpper().Equals(fileExt))
            {
                //PDFﾌｧｲﾙ(.pdf)の場合
                return ContentType.PdfFile;
            }
            else if (FileUtil.GetExtensionStr(FileUtil.Extension.Zip).ToUpper().Equals(fileExt))
            {
                //Zipﾌｧｲﾙ(.zip)の場合
                return ContentType.ZipFile;
            }
            // 未登録の場合はファイルパスより取得する
            new FileExtensionContentTypeProvider().TryGetContentType(filePath, out string contentType);
            return contentType ?? "application/octet-stream";
        }

        /// <summary>
        /// ﾘｸｴｽﾄ情報から先頭のUrlﾊﾟｽを取得
        ///  - Controller用
        /// </summary>
        /// <param name="request">ﾘｸｴｽﾄ情報</param>
        /// <returns>先頭のUrlﾊﾟｽ</returns>
        public static string GetBaseUrlFromRequest(HttpRequest request)
        {
            return getBaseUrl(request);
        }
        /// <summary>
        /// ﾘｸｴｽﾄ情報から先頭のUrlﾊﾟｽを取得
        ///  - WebApi Controller用
        /// </summary>
        /// <param name="request">ﾘｸｴｽﾄ情報</param>
        /// <returns>先頭のUrlﾊﾟｽ</returns>
        public static string GetBaseUrlFromRequest(ControllerContext controllerContext)
        {
            return getBaseUrl(controllerContext.HttpContext.Request);
        }
        /// <summary>
        /// ﾘｸｴｽﾄ情報から先頭のUrlﾊﾟｽを取得
        ///  - 双方向通信用
        /// </summary>
        /// <param name="request">ﾘｸｴｽﾄ情報</param>
        /// <returns>先頭のUrlﾊﾟｽ</returns>
        public static string GetBaseUrlFromRequest(HttpContext context)
        {
            return getBaseUrl(context.Request);
        }

        /// <summary>
        /// ｸﾗｲｱﾝﾄIPｱﾄﾞﾚｽを取得する
        ///  - Controller用
        /// </summary>
        /// <param name="context">ﾘｸｴｽﾄ情報</param>
        /// <returns>IPｱﾄﾞﾚｽ文字列</returns>
        public static string GetClientIpAdress(HttpContext context)
        {

            var clientIp = "";
            var xForwardedFor = context.Request.Headers["HTTP_X_FORWARDED_FOR"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                clientIp = xForwardedFor.Split(',').GetValue(0).ToString().Trim();
            }
            else
            {
                clientIp = context.Connection.LocalIpAddress.ToString();
            }

            if (clientIp != "::1"/*localhost*/)
            {
                clientIp = clientIp.Split(':').GetValue(0).ToString().Trim();
            }

            return clientIp.ToString();
        }
        /// <summary>
        /// ｸﾗｲｱﾝﾄIPｱﾄﾞﾚｽを取得する
        ///  - WebApi Controller用
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns>IPｱﾄﾞﾚｽ文字列</returns>
        public static string GetClientIpAdress(ControllerContext controllerContext)
        {

            //IEnumerable<string> headerValues;
            StringValues headerValues;
            var clientIp = "";
            //if (controllerContext.Request.Headers.TryGetValues("X-Forwarded-For", out headerValues) == true)
            if (controllerContext.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out headerValues) == true)
            {
                var xForwardedFor = headerValues.FirstOrDefault();
                clientIp = xForwardedFor.Split(',').GetValue(0).ToString().Trim();
            }
            else
            {
                //if (controllerContext.Request.Properties.ContainsKey("MS_HttpContext"))
                //{
                //    clientIp = ((HttpContextWrapper)controllerContext.Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                //}
                clientIp = controllerContext.HttpContext.Connection.LocalIpAddress.ToString();
            }

            if (clientIp != "::1"/*localhost*/)
            {
                clientIp = clientIp.Split(':').GetValue(0).ToString().Trim();
            }

            return clientIp.ToString();
        }
        #endregion

        #region === private static 処理 ===
        /// <summary>
        /// 先頭のUrlﾊﾟｽを取得
        /// </summary>
        /// <param name="request">ﾘｸｴｽﾄ情報</param>
        /// <returns></returns>
        //private static string getBaseUrl(Uri requestUri)
        //{
        //    //Scheme⇒http or https
        //    string scheme = requestUri.Scheme;
        //    //Authority⇒localhost:8080 等
        //    string authority = requestUri.Authority;
        //    //AppDomainAppVirtualPath⇒CommonWebTemplate 等
        //    string domain = HttpRuntime.AppDomainAppVirtualPath;
        //    if ("/".Equals(domain))
        //    {
        //        domain = "";
        //    }
        //    //AbsolutePath⇒Common/Index 等
        //    string path = requestUri.AbsolutePath;

        //    //例）http://localhost:8080
        //    return string.Format("{0}://{1}{2}/", scheme, authority, domain);
        //    //return string.Format("{0}://{1}{2}", scheme, authority, path);
        //}

        private static string getBaseUrl(HttpRequest request)
        {
            //Scheme⇒http or https
            string scheme = request.Scheme;
            //Authority⇒localhost:8080 等
            string authority = request.Host.ToUriComponent();
            //AppDomainAppVirtualPath⇒CommonWebTemplate 等
            string domain = request.PathBase.ToUriComponent();
            if ("/".Equals(domain))
            {
                domain = "";
            }

            //例）http://localhost:8080
            return string.Format("{0}://{1}{2}/", scheme, authority, domain);
        }
        #endregion
    }
}