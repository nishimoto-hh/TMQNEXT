using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonWebTemplate.CommonUtil
{
    public class ActionResultEx : ActionResult
    {
        public string Url { get; private set; }

        public Dictionary<string, object> PostData { get; private set; }

        public string Encoding { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="url">リダイレクトURL</param>
        /// <param name="postData">POSTデータ</param>
        /// <param name="encoding">文字コード</param>
        public ActionResultEx(string url, Dictionary<string, object> postData, string encoding = null)
        {
            this.Url = url;
            this.PostData = postData ?? new Dictionary<string, object>();
            this.Encoding = encoding ?? "utf-8";
        }

        /// <summary>
        /// POST用Formの生成
        /// </summary>
        /// <param name="Url">リダイレクトURL</param>
        /// <param name="postData">POSTデータ</param>
        /// <param name="encoding">文字コード</param>
        /// <returns></returns>
        private string BuildPostForm(string url, Dictionary<string, object> postData, string encoding = null)
        {
            string formId = "__PostForm";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<html>");
            sb.AppendLine(@"<head>");
            sb.AppendLine(@"<meta http-equiv='content-type' content='text/html; charset=" + encoding + "'>");
            sb.AppendLine(@"</head>");

            sb.AppendLine(@"<body onload=""document.charset='" + encoding + @"';document.forms[0].submit();"">");
            sb.AppendLine(string.Format(@"<form id='{0}' name='{0}' action='{1}' method='POST' accept-charset='" + encoding + "'>", formId, Url));
            foreach (var item in postData)
            {
                sb.AppendLine(string.Format(@"<input type='hidden' name='{0}' value='{1}'/>", item.Key, item.Value));
            }
            sb.AppendLine(@"</form>");
            sb.AppendLine(@"</body>");
            sb.AppendLine(@"</html>");

            return sb.ToString();
        }

        /// <summary>
        /// POSTメソッドによるリダイレクト実行
        /// </summary>
        /// <param name="url">リダイレクトURL</param>
        /// <param name="postData">POSTデータ</param>
        /// <param name="encoding">文字コード</param>
        /// <returns></returns>
        public static ActionResultEx RedirectAndPost(string url, Dictionary<string, object> postData, string encoding = null)
        {
            return new ActionResultEx(url, postData, encoding);
        }

        /// <summary>
        /// ExecuteResultAsyncのオーバーライド
        /// </summary>
        /// <param name="context"></param>
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var strHtml = this.BuildPostForm(this.Url, this.PostData, this.Encoding);
            byte[] buffer = System.Text.Encoding.GetEncoding(this.Encoding).GetBytes(strHtml);
            await context.HttpContext.Response.Body.WriteAsync(buffer);
        }
    }
}
