///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　ログイン画面制御用Controller 
/// 説明　　　：　ログイン、ログアウト処理の制御等を行います。
/// 
/// 履歴　　　：　2018.05.16 河村純子　新規作成
///</summary>

using CommonWebTemplate.CommonUtil;
using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using CommonSTDUtil;
using System.Linq;
using System.Security.Claims;

namespace CommonWebTemplate.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HomeController(IWebHostEnvironment hostingEnvironment, CommonDataEntities dbContext) : base(hostingEnvironment, dbContext)
        {
        }

        #region === Actionｲﾍﾞﾝﾄ処理 ===
        /// <summary>
        /// GET:ログイン画面／SiteMinderからの遷移ﾍﾟｰｼﾞ
        /// </summary>
        /// <returns>
        /// ログイン認証：OKの場合、TOPﾍﾟｰｼﾞに遷移
        /// </returns>
        public ActionResult Index()
        {
            try
            {
                //SiteMinderからのLoginか？
                if (isSiteMinder())
                {
                    //※SiteMinderからのLogin

                    //=== 遷移元のURL取得 ===
                    string sourceURL = GetSourceURL();

                    //=== ｱｸｾｽ許可ﾁｪｯｸ ===
                    //ｱｸｾｽ許可URL
                    string siteMinderUrl = AppCommonObject.Config.AppSettings.SiteMinderURL;
                    CommonProcData procData = new CommonProcData();

                    //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
                    BusinessLogicUtil blogic = new BusinessLogicUtil();
                    SetRequestInfo(ref procData);
                    if (!string.IsNullOrEmpty(siteMinderUrl))
                    {
                        //※ｱｸｾｽ許可URLが設定されている場合、ﾁｪｯｸする

                        bool isAccessOK = false;
                        if (!string.IsNullOrEmpty(sourceURL) && siteMinderUrl.Equals(sourceURL))
                        {
                            isAccessOK = true;
                        }
                        if (!isAccessOK)
                        {
                            //※ｱｸｾｽ不正

                            if (procData.LanguageId == null)
                            {
                                //ブラウザの言語を取得
                                procData.LanguageId = GetBrowserLanguage();
                            }
                            //許可されてないURLからのアクセスです。
                            blogic.GetResourceName(procData, new List<string> { CommonResources.ID.ID941070006 }, out IDictionary<string, string> resources);
                            var srcUrl = RedirectSiteTopURL;    // TOP画面へ遷移
                            return returnActionResult(ReturnType.AccessError, new List<string>() { blogic.ConvertResourceName(CommonResources.ID.ID941070006, resources) }, null, srcUrl);
                        }
                    }

                    //=== ﾘｸｴｽﾄ情報からﾛｸﾞｲﾝﾕｰｻﾞｰIDを取り出し ===
                    string userId = "";
                    if (Request.Headers.ContainsKey(RequestManageUtil.RequestKey.SM_USER))
                    {
                        userId = Request.Headers[RequestManageUtil.RequestKey.SM_USER].ToString();
                    }

                    //=== ﾛｸﾞｲﾝ処理 ===
                    return Login(null, null, userId, string.Empty, sourceURL);

                }
                //AzureADからのLoginか？
                else if (isAzureAD())
                {
                    //※AzureADからのLogin

                    UserInfoDef userInfo = HttpContext.Session.GetObject<UserInfoDef>(RequestManageUtil.SessionKey.CIM_USER_INFO);
                    if(userInfo != null)
                    {
                        //※ログイン済み

                        //=== ｻｲﾄTopに遷移 ===
                        return RedirectSiteTop();
                    }

                    //=== 遷移元のURL取得 ===
                    string sourceURL = GetSourceURL();
                    if (string.IsNullOrEmpty(sourceURL))
                    {
                        //※遷移元が空の場合、初回アクセス

                        //=== 初期値ﾃﾞｰﾀを作成 ===
                        CommonUserMst loginData = new CommonUserMst();
                        setLoginFormData();

                        //=== ﾛｸﾞｲﾝ画面を表示 ===
                        return View(loginData);
                    }

                    //Idpからのレスポンスが来たという想定

                    //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
                    BusinessLogicUtil blogic = new BusinessLogicUtil();
                    CommonProcData procData = new CommonProcData();
                    SetRequestInfo(ref procData);

                    //=== ｱｸｾｽ許可ﾁｪｯｸ ===
                    // シングルサインオンURL
                    string AzureADSingleSignOnServiceUrl = AppCommonObject.Config.AppSettings.AzureADSingleSignOnServiceUrl;
                    bool isAccessOK = false;
                    string userId = string.Empty;
                    if (!string.IsNullOrEmpty(AzureADSingleSignOnServiceUrl))
                    {
                        //※シングルサインオンURLが設定されている場合、チェックする
                        if (AzureADSingleSignOnServiceUrl.StartsWith(sourceURL))
                        {
                            isAccessOK = true;

                            // IdpのAttributeからユーザ情報を取得する
                            // ClaimTypes.Name : http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name
                            string email = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                            if (!string.IsNullOrEmpty(email))
                            {
                                // メールアドレスをTMQのDBと照合し、取得できれば認証OKとする
                                blogic.GetUserIdByMailAdress(procData, email, out userId);
                            }
                        }
                    }

                    if (!isAccessOK || string.IsNullOrEmpty(userId))
                    {
                        if (procData.LanguageId == null)
                        {
                            //ブラウザの言語を取得
                            procData.LanguageId = GetBrowserLanguage();
                        }

                        string resourceId = string.Empty;
                        ReturnType type;
                        if (!isAccessOK)
                        {
                            //※ｱｸｾｽ不正
                            // 『許可されてないURLからのアクセスです。』
                            resourceId = CommonResources.ID.ID941070006;
                            type = ReturnType.AccessError;
                        }
                        else
                        {
                            //※認証NG
                            // 『ログイン認証に失敗しました。ユーザー権限がありません。』
                            resourceId = CommonResources.ID.ID941070006;
                            type = ReturnType.AccessError;
                        }
                        blogic.GetResourceName(procData, new List<string> { resourceId }, out IDictionary<string, string> resources);
                        return returnActionResult(type, new List<string>() { blogic.ConvertResourceName(resourceId, resources) });
                    }

                    //=== ﾛｸﾞｲﾝ処理 ===
                    return Login(null, null, userId, string.Empty, sourceURL);

                }
                else
                {
                    //※通常ﾛｸﾞｲﾝ

                    //=== 初期値ﾃﾞｰﾀを作成 ===
                    CommonUserMst loginData = new CommonUserMst();
                    setLoginFormData();

                    //=== ﾛｸﾞｲﾝ画面を表示 ===
                    return View(loginData);
                }

            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        /// <summary>
        /// POST:ログイン処理
        /// </summary>
        /// <param name="loginUser">入力値：ログインID</param>
        /// <param name="loginPassword">入力値：パスワード</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="accessKey">遷移先キー</param>
        /// <param name="sourceURL">遷移元URL</param>
        /// <returns>
        /// ログイン認証：OKの場合、TOPﾍﾟｰｼﾞまたは指定機能に遷移
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Produces("application/json")]
        public ActionResult Login(string loginUser, string loginPassword, string userId = "", string accessKey = "", string sourceURL = "")
        {
            try
            {
                //ﾛｸﾞｲﾝﾃﾞｰﾀを生成
                CommonUserMst loginData = new CommonUserMst(loginUser, loginPassword, string.Empty, userId);

                if (isAzureAD() && string.IsNullOrEmpty(userId))
                {
                    //※シングルサインオンでユーザID未取得の場合

                    //=== AzureADログインへ302でリダイレクト ===
                    // ★以下は仮処理
                    return Redirect(AppCommonObject.Config.AppSettings.AzureADSingleSignOnServiceUrl);
                }

                //=== ﾛｸﾞｲﾝ認証 ===
                CommonProcReturn returnInfo = loginAuthentication(loginData, isSiteMinder(), isAzureAD(), sourceURL);

                //=== 戻り値を検証 ===
                ActionResult actionResult = returnActionResult(ReturnType.Normal, returnInfo);
                if (actionResult != null)
                {
                    return actionResult;
                }

                if (returnInfo.IsProcEnd())
                {
                    //※通常ｴﾗｰ時

                    if (isSiteMinder())
                    {
                        //※SiteMinderからのLogin

                        // 遷移元に戻る
                        return returnActionResult(ReturnType.LoginError, returnInfo, sourceURL);
                    }
                    else
                    {
                        //※通常またはAzureADからのﾛｸﾞｲﾝ

                        //ﾛｸﾞｲﾝ画面を表示
                        return View("Index", loginData);
                    }
                }

                //※ﾛｸﾞｲﾝ認証：OK
                if (!string.IsNullOrEmpty(accessKey))
                {
                    // 遷移先キーが指定されている場合

                    //=== 指定機能に遷移 ===
                    CommonProcData procData = new CommonProcData();
                    var token = getRequestVerificationToken(Request);
                    return redirectToCommonAction(Request, procData, accessKey, token);
                }

                //=== ｻｲﾄTopに遷移 ===
                return RedirectSiteTop();
            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        /// <summary>
        /// POST:ログアウト処理
        /// </summary>
        /// <returns>
        /// ログアウト後、ログイン画面に遷移
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Produces("application/json")]
        public ActionResult Logout()
        {
            try
            {
                //ﾛｸﾞｱｳﾄ実施
                string sourceURL = logoutExecute();

                //SiteMinderからのLoginか？
                if (isSiteMinder())
                {
                    //SiteMinderからのﾛｸﾞｲﾝの場合、遷移元に戻る
                    if (!string.IsNullOrEmpty(sourceURL))
                    {
                        return Redirect(sourceURL);
                    }
                    else
                    {
                        //ﾛｸﾞｲﾝｴﾗｰﾍﾟｰｼﾞに遷移

                        //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
                        CommonProcData procData = new CommonProcData();
                        BusinessLogicUtil blogic = new BusinessLogicUtil();

                        if (procData.LanguageId == null)
                        {
                            //ブラウザの言語を取得
                            procData.LanguageId = GetBrowserLanguage();
                        }
                        //SiteMinderログインページが見つかりません。
                        blogic.GetResourceName(procData, new List<string> { CommonResources.ID.ID941110002 }, out IDictionary<string, string> resources);
                        return returnActionResult(ReturnType.LoginError, new List<string>() { blogic.ConvertResourceName(CommonResources.ID.ID941110002, resources) });
                    }
                }

                //ﾛｸﾞｲﾝ画面に戻る
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        //  [ADD_20190826_01][内部課題_No.93] start
        /// <summary>
        /// POST:ログアウト処理
        /// </summary>
        /// <returns>
        /// ログイン認証：OKの場合、TOPﾍﾟｰｼﾞに遷移
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Produces("application/json")]
        public ActionResult IdChange()
        {
            try
            {
                //ﾛｸﾞｱｳﾄ実施
                string sourceURL = logoutExecute();

                // ＩＤ切替画面のURLを取得
                string sourceURL2 = AppCommonObject.Config.AppSettings.IdChangeURL;

                //ＩＤ切替画面の遷移先が設定されている場合、遷移する
                if (!string.IsNullOrEmpty(sourceURL2))
                {
                    return Redirect(sourceURL2);
                }
                // 設定されていない場合は通常のログアウト処理を実行
                else
                {
                    //SiteMinderからのLoginか？
                    if (isSiteMinder())
                    {
                        //SiteMinderからのﾛｸﾞｲﾝの場合、遷移先に戻る
                        if (!string.IsNullOrEmpty(sourceURL))
                        {
                            return Redirect(sourceURL);
                        }
                        else
                        {
                            //ﾛｸﾞｲﾝｴﾗｰﾍﾟｰｼﾞに遷移

                            //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
                            CommonProcData procData = new CommonProcData();
                            BusinessLogicUtil blogic = new BusinessLogicUtil();

                            if (procData.LanguageId == null)
                            {
                                //ブラウザの言語を取得
                                procData.LanguageId = GetBrowserLanguage();
                            }
                            //SiteMinderログインページが見つかりません。
                            blogic.GetResourceName(procData, new List<string> { CommonResources.ID.ID941110002 }, out IDictionary<string, string> resources);
                            return returnActionResult(ReturnType.LoginError, new List<string>() { blogic.ConvertResourceName(CommonResources.ID.ID941110002, resources) });
                        }
                    }

                    //ﾛｸﾞｲﾝ画面に戻る
                    return RedirectToAction("Index");
                }       
            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }
        //  [ADD_20190826_01][内部課題_No.93] end
        #endregion

        #region === private処理 ===
        /// <summary>
        /// ログイン認証
        /// </summary>
        /// <param name="loginData">
        /// ﾛｸﾞｲﾝ情報
        ///  - ﾕｰｻﾞｰID
        ///  - ﾊﾟｽﾜｰﾄﾞ
        ///  ※nullの場合、SiteMinderからのLogin
        /// </param>
        /// <param name="timeoutMinutes">セッションタイムアウト時間(分)</param>
        /// <param name="sourceURL">遷移元URL(※SiteMinderLogin時に使用)</param>
        /// <returns>
        /// 処理ｽﾃｰﾀｽ（CommonProcReturn）
        /// </returns>
        private CommonProcReturn loginAuthentication(CommonUserMst loginData, bool isSiteMinderLogin = false, bool isSSOLogin = false, string sourceURL = "")
        {
            UserInfoDef userInfo = HttpContext.Session.GetObject<UserInfoDef>(RequestManageUtil.SessionKey.CIM_USER_INFO);
            bool isNewLogin = true;
            bool isStandardLogin = !isSiteMinderLogin && !isSSOLogin;

            //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
            CommonProcData procData = new CommonProcData();

            BusinessLogicUtil blogic = new BusinessLogicUtil();
            SetRequestInfo(ref procData);

            if (procData.LanguageId == null)
            {
                //ブラウザの言語を取得
                procData.LanguageId = GetBrowserLanguage();
            }

            //ﾛｸﾞｲﾝ済みﾁｪｯｸ
            if (userInfo != null)
            {
                //※ﾛｸﾞｲﾝ済み
                if ((!string.IsNullOrEmpty(loginData.LoginUser) && !loginData.LoginUser.Equals(userInfo.LoginId)) ||
                    (!string.IsNullOrEmpty(loginData.UserId) && !loginData.UserId.Equals(userInfo.UserId)))
                {
                    // ログイン済みのユーザIDと異なる場合
                    blogic.GetResourceName(procData, new List<string> { CommonResources.ID.ID941130003 }, out IDictionary<string, string> resources);
                    return new CommonProcReturn(CommonProcReturn.ProcStatus.Error, blogic.ConvertResourceName(CommonResources.ID.ID941130003, resources));   //【CW00000.W10】すでにログインしています。一旦ログアウトしてから再ログインしてください。
                }
                if (!isStandardLogin)
                {
                    // SiteMinderまたはSSOからのLoginの場合はログイン認証をスキップ
                    return new CommonProcReturn();
                }
                isNewLogin = false; // ログイン済み
            }

            //※通常ﾛｸﾞｲﾝ時、ﾊﾟｽﾜｰﾄﾞﾁｪｯｸも行う
            bool isPasswordCheck = false;

            //=== 引数ﾁｪｯｸ ===
            if (isStandardLogin)
            {
                //※通常ﾛｸﾞｲﾝ時

                //[ﾕｰｻﾞｰ]
                // - 必須ﾁｪｯｸ
                if (string.IsNullOrEmpty(loginData.LoginUser))
                {
                    blogic.GetResourceName(procData, new List<string> { CommonResources.ID.ID941370003 }, out IDictionary<string, string> resources);
                    return new CommonProcReturn(CommonProcReturn.ProcStatus.Error, blogic.ConvertResourceName(CommonResources.ID.ID941370003, resources));   // ユーザーを入力してください。
                }

                //[ﾊﾟｽﾜｰﾄﾞ]
                // - 必須ﾁｪｯｸ
                if (string.IsNullOrEmpty(loginData.LoginPassword))
                {
                    blogic.GetResourceName(procData, new List<string> { CommonResources.ID.ID941260028 }, out IDictionary<string, string> resources);
                    return new CommonProcReturn(CommonProcReturn.ProcStatus.Error, blogic.ConvertResourceName(CommonResources.ID.ID941260028, resources));   // パスワードを入力してください。
                }
                isPasswordCheck = true;
            }

            //== ﾛｸﾞｲﾝ認証 ==
            AuthenticationUtil authU = new AuthenticationUtil(this._context);

            bool isCheckOK = false;
            CommonProcReturn returnInfo = authU.LoginAuthentication(loginData, isPasswordCheck, procData, out isCheckOK, ref userInfo, isNewLogin);
            if (returnInfo.IsProcEnd())
            {
                //ﾕｰｻﾞｰ認証NG
                return returnInfo;
            }
            if (false == isCheckOK)
            {
                //ﾕｰｻﾞｰ認証NG
                blogic.GetResourceName(procData, new List<string> { CommonResources.ID.ID941430004 }, out IDictionary<string, string> resources);
                return new CommonProcReturn(CommonProcReturn.ProcStatus.Error, blogic.ConvertResourceName(CommonResources.ID.ID941430004, resources));   // ログイン認証に失敗しました。ユーザー権限がありません。
            }

            if (!isNewLogin)
            {
                return returnInfo;
            }

            //== ｾｯｼｮﾝ管理 ==
            //ｾｯｼｮﾝにﾕｰｻﾞｰ情報を保存
            HttpContext.Session.SetObject<UserInfoDef>(RequestManageUtil.SessionKey.CIM_USER_INFO, userInfo);
            //ｾｯｼｮﾝに遷移元URLを保存
            HttpContext.Session.SetString(RequestManageUtil.SessionKey.CIM_TRANS_SRC_URL, sourceURL);

            return new CommonProcReturn();
        }

        /// <summary>
        /// ログイン画面データの設定
        /// </summary>
        private void setLoginFormData()
        {
            if (Request.HasFormContentType && Request.Form.ContainsKey("ACCESS_KEY"))
            {
                ViewBag.AccessKey = Request.Form["ACCESS_KEY"];
            }
            if (Request.HasFormContentType && Request.Form.ContainsKey("MESSAGE"))
            {
                ViewBag.ErrorMessage = new List<string>() { Request.Form["MESSAGE"] };
            }
        }
        #endregion
    }
}