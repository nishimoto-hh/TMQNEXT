using CommonWebTemplate.CommonUtil;
using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebTemplate.Controllers
{
    [Route("[controller]")]
    public class ErrorController　: Controller
    {
        public IActionResult Error(CommonProcData procData, int code)
        {
            BusinessLogicUtil blogic = new BusinessLogicUtil();
            //ブラウザの言語を取得
            var languageId = Request.GetTypedHeaders().AcceptLanguage.OrderByDescending(x => x.Quality ?? 1).Select(x => x.Value.ToString()).ToList().FirstOrDefault();
            if (procData.LanguageId == null)
            {
                procData.LanguageId = languageId;
            }
            string errorTitleId = string.Empty;
            string errorMsgId = "941430005";    //ログインしてください。
            string errorCtrlId = "911430006";   //ログイン画面へ遷移
            string url = BaseController.SiteBaseURL;
            switch (code)
            {
                case 400:
                    if (string.IsNullOrEmpty(procData.ConductId))
                    {
                        // 機能IDが設定されていない場合は未ログイン
                        errorTitleId = "911430005"; // ログインエラー
                    }
                    else
                    {
                        errorTitleId = "911160002"; // タイムアウトエラー
                    }
                    break;
                case 404:
                    errorTitleId = "911010006"; // アクセスエラー
                    break;
                default:
                    errorTitleId = "911120022"; // システムエラー
                    break;
            }

            IDictionary<string, string> resources = null;
            //メッセージ取得
            blogic.GetResourceName(procData, new List<string> { errorTitleId, errorMsgId, errorCtrlId }, out resources);

            ViewBag.Title = blogic.ConvertResourceName(errorTitleId, resources);
            ViewBag.SourceURL = url;
            ViewBag.ErrorMessage = new List<string> { blogic.ConvertResourceName(errorMsgId, resources) };
            ViewBag.BackTitle = blogic.ConvertResourceName(errorCtrlId, resources);

            return View();
        }

    }
}
