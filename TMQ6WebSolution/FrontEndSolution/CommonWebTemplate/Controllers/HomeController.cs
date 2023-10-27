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
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.IdentityModel.Tokens.Saml2;
using System.IO;
using System.IO.Compression;

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
                    CommonProcData procData = new CommonProcData();
                    CommonUserMst loginData = new CommonUserMst();
                    UserInfoDef userInfo = HttpContext.Session.GetObject<UserInfoDef>(RequestManageUtil.SessionKey.CIM_USER_INFO);
                    if(userInfo != null)
                    {
                        //※ログイン済み

                        var accessKey = HttpContext.Session.GetString(RequestManageUtil.SessionKey.TMQ_SSO_ACCESS_KEY);
                        if (!string.IsNullOrEmpty(accessKey))
                        {
                            var token = getRequestVerificationToken(Request);
                            if (string.IsNullOrEmpty(token))
                            {
                                // 偽造防止トークンが取得できない場合、ログイン画面を経由して指定機能へ遷移
                                setLoginFormData(loginData, accessKey, userInfo.UserId);

                                //=== ﾛｸﾞｲﾝ画面を表示 ===
                                return View(loginData);
                            }

                            //=== 指定機能に遷移 ===
                            procData.LoginUserId = userInfo.UserId;
                            return redirectToCommonAction(Request, procData, accessKey, token);
                        }

                        //=== ｻｲﾄTopに遷移 ===
                        return RedirectSiteTop();
                    }

                    //=== 初期値ﾃﾞｰﾀを作成 ===
                    SetRequestInfo(ref procData);
                    setLoginFormData(loginData);

                    //=== ﾛｸﾞｲﾝ画面を表示 ===
                    return View(loginData);
                }
                else
                {
                    //※通常ﾛｸﾞｲﾝ

                    //=== 初期値ﾃﾞｰﾀを作成 ===
                    CommonUserMst loginData = new CommonUserMst();
                    setLoginFormData(loginData);

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
                    //===外部(SAML2)ログインへ302でリダイレクト ===
                    return Redirect("ExternalLogin");
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
        /// GET:外部(SAML2)ログイン処理
        /// SAMLログインのため、Idpにリダイレクトする
        /// </summary>
        /// <returns>
        /// Idpへの認証要求付きリダイレクトレスポンス
        /// </returns>
        [HttpGet]
        public void ExternalLogin()
        {
            //ID作成処理
            int length = 32;
            StringBuilder sb = new StringBuilder(length);
            Random r = new Random();
            string passwordChars = "0123456789abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < length; i++)
            {
                //文字の位置をランダムに選択
                int pos = r.Next(passwordChars.Length);
                //選択された位置の文字を取得
                char c = passwordChars[pos];
                //パスワードに追加
                sb.Append(c);
            }

            DateTime dt = DateTime.Now;
            string IssueInstant = dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");


            var TMQEntityId = AppCommonObject.Config.AppSettings.TMQEntityId;
            var AzureADSingleSignOnServiceUrl = AppCommonObject.Config.AppSettings.AzureADSingleSignOnServiceUrl;
            var TMQReturnUrl = AppCommonObject.Config.AppSettings.TMQReturnUrl;

            string samlRequest1 = "<saml2p:AuthnRequest xmlns:saml2p = \"urn:oasis:names:tc:SAML:2.0:protocol\" xmlns:saml2 = \"urn:oasis:names:tc:SAML:2.0:assertion\" ID = \"";
            string id = "id" + sb.ToString();
            string samlRequest2 = "\" Version = \"2.0\" IssueInstant = \"" + IssueInstant + "\" Destination = \"" + AzureADSingleSignOnServiceUrl + "\" AssertionConsumerServiceURL = \"" + TMQReturnUrl + "\" >\r\n  <saml2:Issuer >"+ TMQEntityId + "</saml2:Issuer >\r\n</saml2p:AuthnRequest >";

            string redirecturlfull = samlRequest1 + id + samlRequest2 ;

            byte[] source = Encoding.UTF8.GetBytes(redirecturlfull);

            // 入出力用のストリームを生成します 
            MemoryStream ms = new MemoryStream();
            DeflateStream CompressedStream = new DeflateStream(ms, CompressionMode.Compress, true);
            // ストリームに圧縮するデータを書き込みます 
            CompressedStream.Write(source, 0, source.Length);
            CompressedStream.Close();
            // 圧縮されたデータを バイト配列で取得します 
            byte[] destination = ms.ToArray();

            //Base64で文字列に変換
            string base64String;
            base64String = System.Convert.ToBase64String(destination, Base64FormattingOptions.InsertLineBreaks);

            var arr = AzureADSingleSignOnServiceUrl.Split('/').ToList();

            var host = arr[0] + "//" + arr[2] + "/";
            var path = String.Join(
                "/",
                arr.GetRange(3, arr.Count - 3).Select(s => System.Net.WebUtility.UrlEncode(s))
            );
            path = path.TrimEnd('/');

            // + base64String;
            base64String = System.Net.WebUtility.UrlEncode(base64String);

            Response.Redirect(host + path + "?SAMLRequest=" + base64String);
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
        /// POST:ID変更処理
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

        /// <summary>
        /// POST:メールアドレス取得処理
        /// </summary>
        /// <returns>
        /// ログイン認証：OKの場合、TOPﾍﾟｰｼﾞに遷移
        /// </returns>
        [HttpPost]
        public ActionResult GetMailAddress()
        {
            try
            {
                //=== 初期値ﾃﾞｰﾀを作成 ===
                CommonUserMst loginData = new CommonUserMst();
                setLoginFormData(loginData);

                //AzureADからのLoginか？
                if (isAzureAD())
                {
                    //=== 遷移元のURL取得 ===
                    string sourceURL = GetSourceURL();
                    if (string.IsNullOrEmpty(sourceURL))
                    {
                        //※遷移元が空の場合、初回アクセス
                        // ﾛｸﾞｲﾝ画面を表示
                        return View("Index", loginData);
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
                    List<int> userIdList = null;
                    string userId = string.Empty;
                    string resourceId = string.Empty;
                    if (!string.IsNullOrEmpty(AzureADSingleSignOnServiceUrl))
                    {
                        //※シングルサインオンURLが設定されている場合、チェックする
                        if (AzureADSingleSignOnServiceUrl.StartsWith(sourceURL))
                        {
                            isAccessOK = true;

                            // IdpのAttributeからユーザ情報を取得する
                            // ClaimTypes.Name : http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name
                            string email = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

                            if (string.IsNullOrEmpty(email) && Request.Form.ContainsKey("SamlResponse"))
                            {
                                // 上記で取得できない場合はリクエスト情報を解析して直接取得する
                                var bResponse = Convert.FromBase64String(Request.Form["SamlResponse"]);
                                Encoding enc = Encoding.UTF8;
                                string strResponse = enc.GetString(bResponse);
                                XElement xml = XElement.Parse(strResponse);

                                // 以下のタグ構成を前提に取得する(名前空間は省略)
                                // <Response>
                                //   <Assertion>
                                //     <AttributeStatement>
                                //       <Attribute Name="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name">
                                //         <AttributeValue>メールアドレス</AttributeValue>
                                //       </Attribute>
                                //     </AttributeStatement>
                                //   </Assertion>
                                // </Response>
                                var assertion = xml.Elements().Where(x => x.Name.LocalName.Equals("Assertion")).FirstOrDefault();
                                var attributeStatement = assertion?.Elements().Where(x => x.Name.LocalName.Equals("AttributeStatement")).FirstOrDefault();
                                var attributes = attributeStatement?.Elements().Where(x => x.Name.LocalName.Equals("Attribute"));
                                var attribute = attributes?.Where(x => x.Attributes().Any(y => y.Name.LocalName.Equals("Name") && y.Value.Equals(ClaimTypes.Name))).FirstOrDefault();
                                email = attribute?.Elements().Where(x => x.Name.LocalName.Equals("AttributeValue")).Select(x => x.Value).FirstOrDefault();
                            }

                            if (!string.IsNullOrEmpty(email))
                            {
                                // メールアドレスをTMQのDBと照合し、取得できれば認証OKとする
                                blogic.GetUserIdByMailAdress(procData, email, out userIdList);
                                if (userIdList != null && userIdList.Count == 1)
                                {
                                    userId = userIdList[0].ToString();
                                    loginData.UserId = userId;
                                    // 『ログイン処理中です。しばらくお待ちください』
                                    resourceId = CommonResources.ID.ID941430006;
                                    blogic.GetResourceName(procData, new List<string> { resourceId }, out IDictionary<string, string> resources);
                                    ViewBag.Messages = new List<string> { blogic.ConvertResourceName(resourceId, resources) };
                                }
                            }
                            // 遷移先をログイン画面へ変更
                            sourceURL = AppCommonObject.Config.AppSettings.TMQPublicOrigin;
                        }
                    }

                    if (!isAccessOK || string.IsNullOrEmpty(userId))
                    {
                        if (procData.LanguageId == null)
                        {
                            //ブラウザの言語を取得
                            procData.LanguageId = GetBrowserLanguage();
                        }

                        ReturnType type;
                        if (!isAccessOK)
                        {
                            //※ｱｸｾｽ不正
                            // 『許可されてないURLからのアクセスです。』
                            resourceId = CommonResources.ID.ID941070006;
                            type = ReturnType.AccessError;
                        }
                        else if (userIdList == null || userIdList.Count == 0)
                        {
                            //※該当メールアドレスが未登録
                            // 『メールアドレスがTMQユーザーマスタに登録されていません。』
                            resourceId = CommonResources.ID.ID941340002;
                            type = ReturnType.LoginError;
                        }
                        else if (userIdList.Count > 1)
                        {
                            //※該当メールアドレスが一意でない
                            // 『メールアドレスがTMQユーザーマスタに複数登録されています。』
                            resourceId = CommonResources.ID.ID941340003;
                            type = ReturnType.LoginError;
                        }
                        else
                        {
                            //※認証NG
                            // 『ログイン認証に失敗しました。ユーザー権限がありません。』
                            resourceId = CommonResources.ID.ID941430004;
                            type = ReturnType.LoginError;
                        }
                        blogic.GetResourceName(procData, new List<string> { resourceId }, out IDictionary<string, string> resources);
                        return returnActionResult(type, new List<string>() { blogic.ConvertResourceName(resourceId, resources) }, sourceURL: sourceURL);
                    }

                    // セッションから遷移キーを取得
                    var accessKey = HttpContext.Session.GetString(RequestManageUtil.SessionKey.TMQ_SSO_ACCESS_KEY);
                    if (!string.IsNullOrEmpty(accessKey))
                    {
                        // ※遷移キーが存在する場合
                        // 遷移キーを設定
                        setLoginFormData(loginData, accessKey);
                    }

                    // ﾛｸﾞｲﾝ画面を表示
                    return View("Index", loginData);

                }
                else
                {
                    //※通常ﾛｸﾞｲﾝ
                    // ﾛｸﾞｲﾝ画面を表示
                    return View("Index", loginData);
                }

            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
            finally
            {
                if (isAzureAD())
                {
                    // セッションから遷移キーを削除
                    HttpContext.Session.Remove(RequestManageUtil.SessionKey.TMQ_SSO_ACCESS_KEY);
                }
            }
        }

        /// <summary>
        /// エラー処理
        /// </summary>
        /// <param name="procData"></param>
        /// <param name="code"></param>
        /// <remarks>ログイン画面への遷移を行うケースが存在するため、HomeControllerへErrorアクションを配置する</remarks>
        /// <returns></returns>
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
            bool autoLogin = false;
            UserInfoDef userInfo = null;
            switch (code)
            {
                case 400:

                    // セッション情報を取得
                    userInfo = HttpContext.Session.GetObject<UserInfoDef>(RequestManageUtil.SessionKey.CIM_USER_INFO);
                    if (userInfo != null)
                    {
                        // セッション情報が存在する場合、ログイン画面へ遷移
                        autoLogin = true;
                        errorMsgId = "941430006";    //ログイン処理中です。しばらくお待ちください。
                    }
                    else if (string.IsNullOrEmpty(procData.ConductId))
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

            if (autoLogin)
            {
                // ログイン情報を生成
                CommonUserMst loginData = new CommonUserMst(string.Empty, string.Empty, string.Empty, userInfo.UserId);
                ViewBag.Messages = new List<string> { blogic.ConvertResourceName(errorMsgId, resources) };
                ViewBag.AutoLogin = "1";   // 自動ログイン
                return View("Index", loginData);
            }

            ViewBag.Title = blogic.ConvertResourceName(errorTitleId, resources);
            ViewBag.SourceURL = url;
            if (!string.IsNullOrEmpty(errorMsgId))
            {
                ViewBag.ErrorMessage = new List<string> { blogic.ConvertResourceName(errorMsgId, resources) };
            }
            ViewBag.BackTitle = blogic.ConvertResourceName(errorCtrlId, resources);

            // エラー画面へ遷移
            return View("Error");
        }
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
                // ログインの種類に関係なく、セッションにログイン情報が存在する場合はログイン認証をスキップする
                //if (!isStandardLogin)
                //{
                //    // SiteMinderまたはSSOからのLoginの場合はログイン認証をスキップ
                //    return new CommonProcReturn();
                //}
                //isNewLogin = false; // ログイン済み

                // ログイン認証をスキップ
                return new CommonProcReturn();
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
        private void setLoginFormData(CommonUserMst loginData, string accsessKey = "", string userId = "")
        {
            if (Request.HasFormContentType && Request.Form.ContainsKey("ACCESS_KEY"))
            {
                if (Request.Form.ContainsKey("AUTO_LOGIN"))
                {
                    ViewBag.AutoLogin = Request.Form["AUTO_LOGIN"];
                }
                if (Request.Form.ContainsKey("ACCESS_KEY"))
                {
                    ViewBag.AccessKey = Request.Form["ACCESS_KEY"];
                }
                if (Request.Form.ContainsKey("MESSAGE"))
                {
                    ViewBag.Messages = new List<string>() { Request.Form["MESSAGE"] };
                }
                if (Request.Form.ContainsKey("ERROR_MESSAGE"))
                {
                    ViewBag.ErrorMessage = new List<string>() { Request.Form["ERROR_MESSAGE"] };
                }
                if (Request.Form.ContainsKey("UserId"))
                {
                    loginData.UserId = Request.Form["UserId"];
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(accsessKey))
                {
                    ViewBag.AccessKey = accsessKey;
                    ViewBag.AutoLogin = "1";
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    loginData.UserId = userId;
                }
            }
        }
        #endregion
    }
}