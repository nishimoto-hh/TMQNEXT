///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　画面制御用基底Controller 
/// 説明　　　：　画面制御に必要な共通制御等を実装します。
/// 
/// 履歴　　　：　2018.05.16 河村純子　新規作成
///</summary>

using System;
using System.Collections.Generic;
using System.Net;

using CommonWebTemplate.Models.Common;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using static CommonWebTemplate.CommonUtil.RequestManageUtil;
using System.Linq;

namespace CommonWebTemplate.CommonUtil
{

    public class BaseController : Controller
    {

        #region === public定数 ===
        /// <summary>
        /// ｻｲﾄBaseUrl指定文字列
        /// </summary>
        public const string SiteBaseURL = @"~/";
        /// <summary>
        /// サイトTOPへのリダイレクトURL文字列
        /// </summary>
        public const string RedirectSiteTopURL = @"~/Common/RedirectSiteTop";
        /// <summary>
        /// 偽造防止トークン用Formキー名
        /// </summary>
        public const string VerificationTokenFormKeyName = "__RequestVerificationToken";
        /// <summary>
        /// 偽造防止トークン用Cookieキー名
        /// </summary>
        public const string VerificationTokenCookieKeyName = "XSRF-TOKEN";
        #endregion

        #region === private定数 ===
        /// <summary>
        /// 戻り値の状態の種類
        /// </summary>
        public enum ReturnType
        {
            /// <summary>
            /// 通常（CommonProcReturn）
            /// ※業務ﾌﾟﾛｼｰｼﾞｬ内等の例外ｴﾗｰはここで管理
            /// </summary>
            Normal,
            /// <summary>
            /// 通常(ﾒｯｾｰｼﾞ設定用)
            /// </summary>
            NormalMessage,
            /// <summary>
            /// 例外ｴﾗｰ（Webｱﾌﾟﾘ内）
            /// </summary>
            Error,
            /// <summary>
            /// ﾛｸﾞｲﾝｴﾗｰ
            /// </summary>
            LoginError,
            /// <summary>
            /// ﾀｲﾑｱｳﾄｴﾗｰ
            /// </summary>
            TimeoutError,
            /// <summary>
            /// ｱｸｾｽ不可ｴﾗｰ
            /// </summary>
            AccessError,
        };

        /// <summary>
        /// ﾛｸﾞ番号ﾒｯｾｰｼﾞﾌｫｰﾏｯﾄ文字列
        /// </summary>
        private const string logNoMessageFormat = "=== 問い合わせ番号[{0}] ===";

        protected IWebHostEnvironment _hostingEnvironment = null;
        #endregion

        #region === メンバ変数 ===
        /// <summary>ログ出力</summary>
        protected static CommonLogger logger = CommonLogger.GetInstance();
        /// <summary>DbContext</summary>
        protected CommonDataEntities _context;
        #endregion

        public BaseController(IWebHostEnvironment hostingEnvironment)
        {
            //IHostingEnvironment をフィールドに保持
            this._hostingEnvironment = hostingEnvironment;
        }

        public BaseController(IWebHostEnvironment hostingEnvironment, CommonDataEntities context) : this(hostingEnvironment)
        {
            this._context = context;
        }

        #region === Actionｲﾍﾞﾝﾄ処理 ===
        /// <summary>
        /// POST:サイトTOP画面に遷移
        /// </summary>
        public ActionResult RedirectSiteTop()
        {
            try
            {
                //機能ID：サイトTOP
                string conductIdTop = AppCommonObject.Config.AppSettings.AppTopConductId;

                // サイトTop画面に遷移⇒POSTメソッドを維持したままリダイレクト
                //return RedirectToAction("Index", "Common", new { ConductId = cuctIdTop });
                //return RedirectToActionPreserveMethod("Index", "Common", new { ConductId = conductIdTop });

                // 上記だと機能IDがブラウザのURLに表示されるため、以下のリダイレクト方法に変更
                // POSTデータの生成
                Dictionary<string, object> postData = new Dictionary<string, object>();
                postData.Add("ConductId", conductIdTop);
                // 偽造防止トークンを設定
                var token = getRequestVerificationToken(Request);
                if (!string.IsNullOrEmpty(token))
                {
                    postData.Add(VerificationTokenFormKeyName, token);
                }

                // CommonコントローラのIndexアクションへPOSTメソッドでリダイレクト
                // (Indexはデフォルトアクションのため指定しないでOK)
                var url = getRedirectUrl(Request, "/Common/");
                var result = ActionResultEx.RedirectAndPost(url, postData);
                return (ActionResult)result;

            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        /// <summary>
        /// POST:ｱｸｾｽｴﾗｰ制御
        /// </summary>
        /// <returns>ｴﾗｰ画面に遷移する</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RedirectAccessError(CommonProcData procData, int status)
        {
            try
            {
                BusinessLogicUtil blogic = new BusinessLogicUtil();

                //=== ｱｸｾｽ許可ﾁｪｯｸ ===
                ActionResult actionResult = accessCheck(ref blogic, procData);
                if (actionResult != null)
                {
                    //ｱｸｾｽｴﾗｰ画面に遷移
                    return actionResult;
                }

                //メッセージ取得
                blogic.GetResourceName(procData, new List<string> { "941010004" }, out IDictionary<string, string> resources);
                //アクセスが不正です。
                string message = blogic.ConvertResourceName("941010004", resources);
                return returnActionResult(ReturnType.Error, new List<string> { message });   //【CW00000.E07】アクセスが不正です。
            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        /// <summary>
        /// (Excel)ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ
        /// </summary>
        /// <param name="fileName">
        /// ﾀﾞｳﾝﾛｰﾄﾞ対象ﾌｧｲﾙ名(※ﾊﾟｽを除く)
        /// 　※ﾌｧｲﾙﾊﾟｽは添付ﾌｫﾙﾀﾞ固定
        /// </param>
        /// <param name="fileDownloadName">ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ名</param>
        /// <param name="deleteFlg">ﾀﾞｳﾝﾛｰﾄﾞ対象ﾌｧｲﾙ削除ﾌﾗｸﾞ</param>
        /// <returns>
        /// FileContentResult
        ///  - 指定したｻｰﾊﾞｰ上のﾌｧｲﾙをﾀﾞｳﾝﾛｰﾄﾞ
        /// </returns>
        /// <remarks>
        /// WebApiのﾌｧｲﾙ作成⇒出力処理ｺｰﾙ時、FileDownloadSet = 1:Hidoukiに設定した場合はここの処理を実行
        ///  ※作成ﾌｧｲﾙ情報をﾌｧｲﾙで生成⇒ﾀﾞｳﾝﾛｰﾄﾞ
        /// </remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FileDownload(string fileName, string fileDownloadName, int deleteFlg = 1)
        {
            //代表ﾒｯｾｰｼﾞを初期化
            ViewBag.ErrorMessage = new List<string>();
            ViewBag.Message = string.Empty;

            string filePath = String.Empty;
            try
            {
                //Excelﾀﾞｳﾝﾛｰﾄﾞ

                // - ﾀﾞｳﾝﾛｰﾄﾞ対象ﾌｧｲﾙﾊﾟｽ
                var rootPath = this._hostingEnvironment.ContentRootPath;
                filePath = FileUtil.GetTempFilePath(rootPath, fileName);
                // - ﾀﾞｳﾝﾛｰﾄﾞ対象ﾌｧｲﾙをﾊﾞｲﾄ配列に読込んで取得
                byte[] bs = FileUtil.GetFileToByteData(filePath);

                // - ContentType
                string contentType = SystemUtil.GetContentType(fileDownloadName);
                // - ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ
                return File(bs,
                    contentType,
                    fileDownloadName);
            }
            catch (Exception ex)
            {
                //代表ﾒｯｾｰｼﾞを初期化
                ViewBag.ErrorMessage.Add(ex.Message);

                //ﾛｸﾞ用のﾒｯｾｰｼﾞ生成
                StringBuilder errLogMsg = new StringBuilder();
                errLogMsg.Append(ex.ToString());
                if (ex.InnerException != null)
                {
                    errLogMsg.AppendLine("").Append(" > " + ex.InnerException.ToString());
                }
                //ﾛｸﾞ出力
                logger.WriteLog(errLogMsg.ToString());

                //実行時例外ｴﾗｰ
                ViewBag.ReturnInfo = new CommonProcReturn(CommonProcReturn.ProcStatus.InValid, "");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                if (deleteFlg == 1)
                {
                    //ﾀﾞｳﾝﾛｰﾄﾞﾌｧｲﾙを削除
                    FileUtil.DeleteFile(filePath);
                }
            }
        }
        #endregion

        #region === protected処理 ===
        /// <summary>
        /// SiteMinderからのLoginか？
        /// </summary>
        /// <returns>treu:SiteMinderからのﾛｸﾞｲﾝ</returns>
        protected bool isSiteMinder()
        {
            //SiteMinderからのLoginか？
            return AppCommonObject.Config.AppSettings.SiteMinderLogin;
        }
        /// <summary>
        /// AzureADからのLoginか？
        /// </summary>
        /// <returns>treu:AzureADからのﾛｸﾞｲﾝ</returns>
        protected bool isAzureAD()
        {
            //AzureADからのLoginか？
            return AppCommonObject.Config.AppSettings.AzureADLogin;
        }
        /// <summary>
        /// ｻｲﾄ内ｱｸｾｽ可能か検証する
        ///  - ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得する
        ///  - ﾘｸｴｽﾄﾊﾟﾗﾒｰﾀのﾃﾞｰﾀ検証を行う
        /// </summary>
        /// <param name="blogic">業務ﾛｼﾞｯｸｸﾗｽ</param>
        /// <param name="procData">ﾘｸｴｽﾄﾊﾟﾗﾒｰﾀ</param>
        /// <returns></returns>
        /// <remarks>
        /// ※ﾛｸﾞｲﾝ画面のｱｸｾｽ許可ﾁｪｯｸには使用しない
        /// </remarks>
        protected ActionResult accessCheck(ref BusinessLogicUtil blogic, CommonProcData procData)
        {
            //ｾｯｼｮﾝﾀｲﾑｱｳﾄﾁｪｯｸ
            if (!HttpContext.Session.IsAvailable || HttpContext.Session.Keys.Count() == 0)
            {
                //※ｾｯｼｮﾝﾀｲﾑｱｳﾄ

                //ﾛｸﾞｱｳﾄ処理を実施
                string sourceURL = logoutExecute();

                if (!isSiteMinder())
                {
                    //※SiteMinderからのﾛｸﾞｲﾝでない場合

                    sourceURL = SiteBaseURL;    //戻り先URL:(仮)ﾛｸﾞｲﾝﾍﾟｰｼﾞ
                }

                return returnActionResult(ReturnType.TimeoutError, null, sourceURL);
            }

            //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
            SetRequestInfo(ref procData);
            if (procData.LanguageId == null)
            {
                //ブラウザの言語を取得
                procData.LanguageId = GetBrowserLanguage();
            }

            //ﾘｸｴｽﾄﾊﾟﾗﾒｰﾀを検証
            ActionResult validate = validateRequestParameters(ref blogic, procData);
            if (validate != null)
            {
                //※ﾊﾟﾗﾒｰﾀ不正

                //ｴﾗｰ画面に遷移
                return validate;
            }

            //表示用にﾕｰｻﾞｰ名を一時変数に設定
            ViewBag.UserName = procData.LoginUserName;
            return null;
        }

        /// <summary>
        /// ﾛｸﾞｱｳﾄ処理を実施する
        /// </summary>
        /// <returns>ﾛｸﾞｲﾝ時遷移元URL</returns>
        protected string logoutExecute()
        {
            //ｾｯｼｮﾝにﾕｰｻﾞｰ情報が保持されている場合
            var session = HttpContext.Session;
            string sourceURL = string.Empty;
            UserInfoDef userInfo = session.GetObject<UserInfoDef>(RequestManageUtil.SessionKey.CIM_USER_INFO);
            if (userInfo != null)
            {
                //ﾛｸﾞｲﾝ時遷移元URL
                sourceURL = session.GetString(RequestManageUtil.SessionKey.CIM_TRANS_SRC_URL);

                // 共通機能対応依頼 【No.69】ATTS三原 start
                string LogoutURL = string.Empty;
                LogoutURL = AppCommonObject.Config.AppSettings.LogOutURL;

                // ログアウト用のURLが設定されている場合は設定を優先する
                if (LogoutURL != string.Empty)
                {
                    sourceURL = LogoutURL;
                }
                // 共通機能対応依頼 【No.69】ATTS三原 end

                // ログアウト処理
                AuthenticationUtil authU = new AuthenticationUtil(this._context);
                authU.Logout(userInfo);

            }

            // セッション破棄
            session.Clear();
            // すべてのCookieを破棄
            expireAllCookies();

            return sourceURL;
        }

        /// <summary>
        /// ﾘｸｴｽﾄﾊﾟﾗﾒｰﾀのﾃﾞｰﾀ検証を行う
        /// </summary>
        /// <param name="procData">ﾘｸｴｽﾄﾊﾟﾗﾒｰﾀ</param>
        /// <returns></returns>
        protected ActionResult validateRequestParameters(ref BusinessLogicUtil blogic, CommonProcData procData)
        {
            //ﾘｸｴｽﾄﾊﾟﾗﾒｰﾀのﾃﾞｰﾀ検証
            CommonProcReturn returnInfo = blogic.ValidateRequestParameters(procData);

            //戻り値を設定
            if (returnInfo != null)
            {
                ReturnType type;
                string sourceUrl = string.Empty;

                switch (returnInfo.STATUS)
                {
                    case CommonProcReturn.ProcStatus.LoginError:
                        type = ReturnType.LoginError;
                        break;
                    case CommonProcReturn.ProcStatus.TimeOut:
                        type = ReturnType.TimeoutError;
                        break;
                    default:
                        type = ReturnType.AccessError;
                        break;
                }
                if (!isSiteMinder())
                {
                    sourceUrl = SiteBaseURL;
                }

                return returnActionResult(type, null, new List<string>() { returnInfo.MESSAGE }, null, sourceUrl);
            }

            return null;
        }

        /// <summary>
        /// 戻り値の検証、および生成を行う
        /// </summary>
        /// <param name="type">
        /// 戻り値の状態の種類
        ///  type = Normalの場合、returnInfoの値を見て戻り値を決定する
        /// </param>
        /// <param name="returnInfo">処理結果</param>
        /// <returns>
        /// ｺﾝﾄﾛｰﾗｰからの戻り値を生成して返す
        ///  nullの場合は戻り先で通常画面表示
        /// </returns>
        protected ActionResult returnActionResult(Exception ex)
        {
            //☆
            ////ｴﾗｰ情報を生成
            ////☆ﾛｸﾞの生成
            //string logNo = DateTime.Now.ToString("yyyyMMddHHmmssff");
            ////LogUtil
            ////☆共通ﾒｯｾｰｼﾞとﾛｸﾞ番号を設定
            //CommonProcReturn returnInfo = new CommonProcReturn(
            //    CommonProcReturn.ProcStatus.InValid,    //例外ｴﾗｰ
            //    MessageConstants.Error.E01);            //【CW00000.E01】処理に失敗しました。
            //returnInfo.LOGNO = logNo;                   //問い合わせ番号

            ///例外ｴﾗｰ画面に遷移
            //return returnActionResult(returnType.Error, returnInfo);

            //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
            CommonProcData procData = new CommonProcData();
            BusinessLogicUtil blogic = new BusinessLogicUtil();
            SetRequestInfo(ref procData);
            if (procData.LanguageId == null)
            {
                //ブラウザの言語を取得
                procData.LanguageId = GetBrowserLanguage();
            }
            //メッセージ取得
            //システムエラーが発生しました。管理者に問い合わせて下さい。
            //連絡先１：○○株式会社○○部○○課
            //連絡先２：00-0000-0000
            //連絡先３：
            var messageIds = new List<string> { "941120013", "941420001", "941420002", "941420003" };
            blogic.GetResourceName(procData, messageIds, out IDictionary<string, string> resources);
            var messages = new List<string>();
            var additionalInfo = new List<string>();
            for(int i=0; i< messageIds.Count; i++)
            {
               var message = blogic.ConvertResourceName(messageIds[i], resources);
                if (!string.IsNullOrEmpty(message))
                {
                    if (i == 0)
                    {
                        messages.Add(message);
                    }
                    else
                    {
                        additionalInfo.Add(message);
                    }
                }
            }

            //ﾛｸﾞ用のﾒｯｾｰｼﾞ生成
            StringBuilder errLogMsg = new StringBuilder();
            errLogMsg.Append(ex.ToString());
            if (ex.InnerException != null)
            {
                errLogMsg.AppendLine("").Append(" > " + ex.InnerException.ToString());
            }
            //ﾛｸﾞ出力
            logger.WriteLog(errLogMsg.ToString());

            return returnActionResult(ReturnType.Error, messages, additionalInfo);
        }

        protected ActionResult returnActionResult(ReturnType type, CommonProcReturn returnInfo = null, string sourceURL = "")
        {
            return returnActionResult(type, null, null, returnInfo, sourceURL);
        }
        protected ActionResult returnActionResult(ReturnType type, string message)
        {
            return returnActionResult(type, message, null, null);
        }
        protected ActionResult returnActionResult(ReturnType type, IList<string> errorMessages, IList<string>additionalInfo = null, string sourceURL = "")
        {
            return returnActionResult(type, null, errorMessages, null, sourceURL, additionalInfo);
        }

        /// <summary>
        /// 戻り値の検証、および生成を行う
        /// 　※実処理部分実装
        /// </summary>
        /// <param name="type">
        /// 戻り値の状態の種類
        ///  type = Normalの場合、returnInfoの値を見て戻り値を決定する
        /// </param>
        /// <param name="message">メッセージ</param>
        /// <param name="errorMessages">エラーメッセージ</param>
        /// <param name="returnInfo">処理結果</param>
        /// <param name="sourceURL">戻り先URL</param>
        /// <
        /// <returns>
        /// ｺﾝﾄﾛｰﾗｰからの戻り値を生成して返す
        ///  nullの場合は戻り先で通常画面表示
        /// </returns>
        //protected ActionResult returnActionResult(ReturnType type, string message, IList<string> errorMessages, CommonProcReturn returnInfo, string sourceURL = "")
        protected ActionResult returnActionResult(ReturnType type, string message, IList<string> errorMessages, CommonProcReturn returnInfo, string sourceURL = "", IList<string> additionalInfo = null)
        {
            //代表ﾒｯｾｰｼﾞを初期化
            ViewBag.Message = message;
            ViewBag.ErrorMessage = errorMessages;
            ViewBag.SourceURL = "";
            ViewBag.AdditionalInfo = additionalInfo;

            if (ViewBag.ErrorMessage == null)
            {
                ViewBag.ErrorMessage = new List<string>();
            }
            if (ViewBag.AdditionalInfo == null)
            {
                ViewBag.AdditionalInfo = new List<string>();
            }

            //処理結果をViewBagに設定
            if (returnInfo != null)
            {

                if (returnInfo.IsProcEnd())
                {
                    //例外エラー、入力等機能エラー、問合せの場合、処理を中断
                    if (returnInfo.STATUS == CommonProcReturn.ProcStatus.Confirm)
                    {
                        //メッセージを確認表示
                        ViewBag.ConfirmMessage = returnInfo.MESSAGE;
                    }
                    else
                    {
                        //メッセージをエラー表示
                        ViewBag.ErrorMessage.Add(returnInfo.MESSAGE);
                        if (!string.IsNullOrEmpty(returnInfo.LOGNO))
                        {
                            string logMsgStr = getMessageFormatLogNo(returnInfo.LOGNO);
                            ViewBag.ErrorMessage.Add(logMsgStr);
                        }
                    }
                    //ﾛｸﾞ出力
                    logger.WriteLog(string.Join('\n', ViewBag.ErrorMessage.ToArray()));
                }
                else
                {
                    //警告時、
                    if (returnInfo.STATUS == CommonProcReturn.ProcStatus.Warning ||
                        returnInfo.STATUS == CommonProcReturn.ProcStatus.WarnDisp)
                    {
                        //メッセージをエラー表示
                        ViewBag.ErrorMessage.Add(returnInfo.MESSAGE);
                        if (!string.IsNullOrEmpty(returnInfo.LOGNO))
                        {
                            string logMsgStr = getMessageFormatLogNo(returnInfo.LOGNO);
                            ViewBag.ErrorMessage.Add(logMsgStr);
                        }
                        //ﾛｸﾞ出力
                        logger.WriteLog(string.Join('\n', ViewBag.ErrorMessage.ToArray()));
                    }
                    else
                    {
                        //メッセージをお知らせ表示
                        ViewBag.Message = returnInfo.MESSAGE;
                    }

                    //※正常時、通常画面を表示する
                }
            }

            //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
            CommonProcData procData = new CommonProcData();
            BusinessLogicUtil blogic = new BusinessLogicUtil();
            SetRequestInfo(ref procData);

            if (procData.LanguageId == null)
            {
                //ブラウザの言語を取得
                procData.LanguageId = GetBrowserLanguage();
            }

            IDictionary<string, string> resources = null;

            switch (type)
            {
                case ReturnType.Error:          //例外ｴﾗｰ
                    //☆
                    //return View("Error");
                    
                    //メッセージ取得
                    blogic.GetResourceName(procData, new List<string> { "911120022", "911200006" }, out resources);

                    ViewBag.Title = blogic.ConvertResourceName("911120022", resources); //システムエラー
                    ViewBag.SourceURL = RedirectSiteTopURL;      //戻り先URL:SiteMinderからのﾛｸﾞｲﾝ時、ﾛｸﾞｲﾝｴﾗｰの場合に設定される
                    ViewBag.BackTitle = blogic.ConvertResourceName("911200006", resources); //TOP画面へ遷移

                    return View("Error");

                case ReturnType.LoginError:     //ﾛｸﾞｲﾝｴﾗｰ
                    //☆
                    //return View("LoginError");

                    //メッセージ取得
                    blogic.GetResourceName(procData, new List<string> { "911430005", "911430006" }, out resources);

                    ViewBag.Title = blogic.ConvertResourceName("911430005", resources); //ログインエラー
                    ViewBag.SourceURL = sourceURL;      //戻り先URL:SiteMinderからのﾛｸﾞｲﾝ時、ﾛｸﾞｲﾝｴﾗｰの場合に設定される
                    ViewBag.BackTitle = blogic.ConvertResourceName("911430006", resources); //ログイン画面へ遷移

                    return View("Error");

                case ReturnType.TimeoutError:   //ﾀｲﾑｱｳﾄｴﾗｰ
                    //☆
                    //return View("TimeoutError");

                    //メッセージ取得
                    blogic.GetResourceName(procData, new List<string> { "911160002", "911430006" }, out resources);

                    ViewBag.Title = blogic.ConvertResourceName("911160002", resources); //タイムアウトエラー
                    ViewBag.SourceURL = sourceURL;      //戻り先URL:SiteMinderからのﾛｸﾞｲﾝ時、ﾛｸﾞｲﾝｴﾗｰの場合に設定される
                    ViewBag.BackTitle = blogic.ConvertResourceName("911430006", resources); //ログイン画面へ遷移

                    return View("Error");

                case ReturnType.AccessError:   //ｱｸｾｽ不可ｴﾗｰ
                    //☆
                    //return View("AccessError");
                    
                    //メッセージ取得
                    blogic.GetResourceName(procData, new List<string> { "911010006", "911200006", "911430006" }, out resources);

                    ViewBag.Title = blogic.ConvertResourceName("911010006", resources); //アクセスエラー
                    if (string.IsNullOrEmpty(sourceURL))
                    {
                        ViewBag.SourceURL = SiteBaseURL;
                        ViewBag.BackTitle = blogic.ConvertResourceName("911430006", resources); //ログイン画面へ遷移
                    }
                    else
                    {
                        ViewBag.SourceURL = sourceURL;      //戻り先URL:SiteMinderからのﾛｸﾞｲﾝ時、ﾛｸﾞｲﾝｴﾗｰの場合に設定される
                        ViewBag.BackTitle = blogic.ConvertResourceName("911200006", resources); //TOP画面へ遷移
                    }

                    return View("Error");

                case ReturnType.NormalMessage:     //通常ﾒｯｾｰｼﾞ
                    //※ViewBagのﾒｯｾｰｼﾞ設定をして抜ける
                    break;
                case ReturnType.Normal:     //通常
                    if (returnInfo == null)
                    {
                        //戻り値不正

                        //メッセージ取得
                        blogic.GetResourceName(procData, new List<string> { "941120014" }, out resources);
                        ViewBag.ErrorMessage.Add(blogic.ConvertResourceName("941120014", resources));   //処理結果が不正です。

                        //例外ｴﾗｰ画面に遷移
                        return returnActionResult(ReturnType.Error, ViewBag.ErrorMessage);
                    }

                    if (returnInfo.STATUS == CommonProcReturn.ProcStatus.InValid)
                    {
                        //例外ｴﾗｰ時、例外ｴﾗｰ画面に遷移
                        return returnActionResult(ReturnType.Error, ViewBag.ErrorMessage);
                    }

                    break;
            }

            return null;
        }

        /// <summary>
        /// ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を設定して返す
        ///  - Controller用
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="procData">業務ﾛｼﾞｯｸﾃﾞｰﾀ(I/O)</param>
        protected void SetRequestInfo(ref CommonProcData procData)

        {
            HttpRequest request = HttpContext.Request;

            if (request.HasFormContentType)
            {
                //JSON形式のFormﾃﾞｰﾀを手動で取得
                //FORMNO
                if (request.Form.ContainsKey("FORMNO"))
                {
                    string jsonStr = request.Form["FORMNO"];
                    if (jsonStr.Length > 0 && jsonStr != "null")
                    {
                        procData.FormNo = JsonSerializer.Deserialize<short>(jsonStr);
                    }
                }
                //条件ﾃﾞｰﾀ
                if (request.Form.ContainsKey("ConditionData"))
                {
                    string jsonStr = request.Form["ConditionData"];
                    if (jsonStr.Length > 0 && jsonStr !="null")
                    {
                        //procData.ConditionData = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
                        procData.ConditionData = CommonDefinitions.CommonUtil.JsonToDictionaryList(jsonStr);
                    }
                }

                //条件ﾃﾞｰﾀ
                if (request.Form.ContainsKey("FactoryIdList"))
                {
                    string jsonStr = request.Form["FactoryIdList"];
                    if (jsonStr.Length > 0 && jsonStr != "null")
                    {
                        procData.FactoryIdList = JsonSerializer.Deserialize<List<int>>(jsonStr);
                    }
                }

                //登録ﾃﾞｰﾀ(アップロードの場合は"RegistData")
                if (request.Form.ContainsKey("RegistData"))
                {
                    string jsonStr = request.Form["RegistData"];
                    if (jsonStr.Length > 0 && jsonStr != "null")
                    {
                        procData.ListData = CommonDefinitions.CommonUtil.JsonToDictionaryList(jsonStr);
                    }

                    // 追加で登録データがある場合は追加
                    if (request.Form.ContainsKey("AddRegistData"))
                    {
                        jsonStr = request.Form["AddRegistData"];
                        if (jsonStr.Length > 0 && jsonStr != "null")
                        {
                            foreach (var data in CommonDefinitions.CommonUtil.JsonToDictionaryList(jsonStr))
                            {
                                procData.ListData.Add(data);
                            }
                        }
                    }
                }
                else if(request.Form.ContainsKey("ListData"))
                {
                    string jsonStr = request.Form["ListData"];
                    if (jsonStr.Length > 0 && jsonStr != "null")
                    {
                        //procData.ListData = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
                        procData.ListData = CommonDefinitions.CommonUtil.JsonToDictionaryList(jsonStr);
                    }
                }

                //一覧定義
                if (request.Form.ContainsKey("ListDefines"))
                {
                    string jsonStr = request.Form["ListDefines"];
                    if (jsonStr.Length > 0 && jsonStr != "null")
                    {
                        //procData.ListDefines = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
                        procData.ListDefines = CommonDefinitions.CommonUtil.JsonToDictionaryList(jsonStr);
                    }
                }
                //ﾎﾞﾀﾝ定義
                if (request.Form.ContainsKey("ButtonDefines"))
                {
                    string jsonStr = request.Form["ButtonDefines"];
                    if (jsonStr.Length > 0 && jsonStr != "null")
                    {
                        //procData.ButtonDefines = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
                        procData.ButtonDefines = CommonDefinitions.CommonUtil.JsonToDictionaryList(jsonStr);
                    }
                }
                //個別実装用ﾃﾞｰﾀﾘｽﾄ
                if (request.Form.ContainsKey("ListIndividual"))
                {
                    string jsonStr = request.Form["ListIndividual"];
                    if (jsonStr.Length > 0 && jsonStr != "null")
                    {
                        //procData.ListIndividual = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonStr);
                        procData.ListIndividual = CommonDefinitions.CommonUtil.JsonToDictionary(jsonStr);
                    }
                }
                // アップロードファイルデータ
                if (request.Form.Files != null && request.Form.Files.Count > 0)
                {
                    procData.UploadFile = request.Form.Files.ToArray();
                }

            }

            //IPｱﾄﾞﾚｽを取得
            if ("test".Equals(AppCommonObject.Config.AppSettings.DeployMode) ||
                "prot".Equals(AppCommonObject.Config.AppSettings.DeployMode))
            {
                //プロト作成モード
                procData.TerminalNo = @"::1";
            }
            else
            {
                //Releaseモード
                if (string.IsNullOrEmpty(procData.TerminalNo))
                {
                    procData.TerminalNo = SystemUtil.GetClientIpAdress(HttpContext);
                }
            }

            //Urlﾊﾟｽを取得
            procData.BaseUrl = SystemUtil.GetBaseUrlFromRequest(HttpContext);

            //ｾｯｼｮﾝに保持しているﾕｰｻﾞｰ情報を取得
            if (HttpContext.Session.Keys.Contains(SessionKey.CIM_USER_INFO))
            {
                UserInfoDef userInfo = HttpContext.Session.GetObject<UserInfoDef>(SessionKey.CIM_USER_INFO);
                procData.LoginUserId = userInfo.UserId;
                procData.LoginId = userInfo.LoginId;
                procData.LoginUserName = userInfo.UserName;
                procData.LanguageId = userInfo.LanguageId;
                procData.GUID = userInfo.GUID;
                procData.AuthorityLevelId = userInfo.AuthorityLevelId;
                procData.BelongingInfo = userInfo.BelongingInfo;

                //=== 権限情報 ==
                //ﾕｰｻﾞｰ機能権限のある機能ﾏｽﾀﾘｽﾄ
                procData.UserAuthConducts = userInfo.UserAuthConducts;
            }
        }

        /// <summary>
        /// ブラウザの言語を取得
        /// </summary>
        /// <returns>ブラウザの言語</returns>
        protected string GetBrowserLanguage()
        {
            return Request.GetTypedHeaders().AcceptLanguage.OrderByDescending(x => x.Quality ?? 1).Select(x => x.Value.ToString()).ToList().FirstOrDefault();
        }

        /// <summary>
        /// 遷移元URLを取得
        /// </summary>
        /// <returns>遷移元URL</returns>
        protected string GetSourceURL()
        {
            string sourceURL = string.Empty;
            if (!string.IsNullOrEmpty(Request.Headers.Referer))
            {
                sourceURL = Request.Headers.Referer;
            }
            return sourceURL;
        }
        #endregion

        #region === private処理 ===
        /// <summary>
        /// ﾛｸﾞ番号のﾒｯｾｰｼﾞ文字列ﾌｫｰﾏｯﾄを行う
        /// </summary>
        /// <param name="logNo">ﾛｸﾞ番号</param>
        /// <returns>ﾛｸﾞ番号のﾒｯｾｰｼﾞ文字列</returns>
        private string getMessageFormatLogNo(string logNo)
        {
            return string.Format(logNoMessageFormat, logNo);
        }

        /// <summary>
        /// すべてのCookieを破棄
        /// </summary>
        private void expireAllCookies()
        {
            if (HttpContext.Response.Cookies != null)
            {
                foreach (var cookie in HttpContext.Request.Cookies)
                {
                    if (cookie.Value != null)
                    {
                        var cookieOptions = new CookieOptions()
                        {
                            // 有効期限を過去の日付に設定
                            Expires = DateTime.Now.AddDays(-1),
                        };
                        // Cookieの有効期限を上書き
                        HttpContext.Response.Cookies.Append(cookie.Key, cookie.Value, cookieOptions);
                    }
                }
                // サーバーサイドのCookieを削除
                HttpContext.Request.Cookies = null;
            }
        }

        /// <summary>
        /// リダイレクト先のURLを取得
        /// </summary>
        /// <param name="request">HTTPRequestオブジェクト</param>
        /// <param name="actionUrl">アクションURL</param>
        /// <returns></returns>
        protected string getRedirectUrl(HttpRequest request, string actionUrl)
        {
            var url = string.Concat(
                request.Scheme,
                "://",
                request.Host.ToUriComponent(),
                request.PathBase.ToUriComponent(),
                actionUrl);
            return url;
        }

        /// <summary>
        /// リクエスト情報から偽造防止トークンを取得
        /// </summary>
        /// <param name="request">HTTPRequestオブジェクト</param>
        /// <returns></returns>
        protected string getRequestVerificationToken(HttpRequest request)
        {
            var token = string.Empty;
            if (request.HasFormContentType && Request.Form.ContainsKey(VerificationTokenFormKeyName))
            {
                // Formから偽造防止トークンを取得
                token = Request.Form[VerificationTokenFormKeyName];
            }
            else if (request.Cookies.ContainsKey(VerificationTokenCookieKeyName))
            {
                // RequestのCookieから偽造防止トークンを取得
                token = Request.Cookies[VerificationTokenCookieKeyName];
            }
            else
            {
                // ResponseのCookieから偽造防止トークンを取得
                var setCookies = Response.GetTypedHeaders().SetCookie;
                if (setCookies != null)
                {
                    var setCookie = setCookies.FirstOrDefault(x => x.Name == VerificationTokenCookieKeyName);
                    if (setCookie != null)
                    {
                        token = setCookie.Value.ToString();
                    }
                }
            }
            return token;
        }

        /// <summary>
        /// Commonコントローラの指定機能へ遷移
        /// </summary>
        /// <param name="request">HTTPRequestオブジェクト</param>
        /// <param name="procData">業務ロジックデータ</param>
        /// <param name="key">遷移先キー(アンダーバー区切りの文字列)</param>
        /// <param name="token">偽造防止トークン</param>
        /// <returns></returns>
        protected ActionResult redirectToCommonAction(HttpRequest request, CommonProcData procData, string key, string token)
        {
            BusinessLogicUtil blogic = new BusinessLogicUtil();
            try
            {
                // 遷移先キーの暗号化は行わない
                //// 暗号化されたアクセスキーを復号化
                //// バックエンドの復号化データ取得処理を呼び出す
                //CommonProcReturn returnInfo = blogic.GetDecryptedData(procData, key, out string decryptedKey);
                //if (returnInfo.IsProcEnd() || string.IsNullOrEmpty(decryptedKey))
                //{
                //    // 復号化失敗
                //    throw new Exception(string.Format("Failed to decrypt access key. [{0}]", key));
                //}

                //// JSON文字列のアクセスキーをDictionaryに変換
                //Dictionary<string, object> dicKey = null;
                //dicKey = CommonDefinitions.CommonUtil.JsonToDictionary(decryptedKey);

                // 遷移キーをアンダーバー区切りの文字列とする
                // [機能ID]_[画面NO]_[データキー(複数存在する場合はアンダーバー区切り)]
                Dictionary<string, object> dicKey = new Dictionary<string, object>();
                var keys = key.Split("_");
                if (keys.Length > 0 && !string.IsNullOrEmpty(keys[0]))
                {
                    dicKey.Add("ID", keys[0]);
                }
                if (keys.Length > 1 && !string.IsNullOrEmpty(keys[1]))
                {
                    dicKey.Add("NO", Convert.ToInt32(keys[1]));
                }
                var dataKeys = new List<string>();
                if (keys.Length > 2)
                {
                    for (int i = 2; i < keys.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(keys[i]))
                        {
                            dataKeys.Add(keys[i]);
                        }
                    }
                    if (dataKeys.Count > 0)
                    {
                        dicKey.Add("KEY", string.Join(",", dataKeys));
                    }
                }

                Dictionary<string, object> postData = null;
                if (dicKey.ContainsKey("ID") && dicKey.ContainsKey("NO") && dicKey.ContainsKey("KEY"))
                {
                    // POSTデータを生成
                    postData = new Dictionary<string, object>();
                    postData.Add("ConductId", dicKey["ID"]);
                    postData.Add("FormNo", dicKey["NO"]);
                    postData.Add("KEY", dicKey["KEY"]);
                    postData.Add(VerificationTokenFormKeyName, token);
                }
                else
                {
                    // アクセスキー不備
                    throw new Exception(string.Format("Access key is invalid. [{0}]", key));
                }

                if (postData == null)
                {
                    // アクセスキー不備
                    throw new Exception(string.Format("Access key is invalid. [{0}]", key));
                }

                // CommonコントローラのIndexアクションへリダイレクト
                // (Indexはデフォルトアクションのため指定しないでOK)
                var url = getRedirectUrl(Request, "/Common/");
                return (ActionResult)ActionResultEx.RedirectAndPost(url, postData);
            }
            catch (Exception ex)
            {
                // エラーログ出力
                if(ex is System.Text.Json.JsonException)
                {
                    logger.WriteLog(string.Format("Access key is invalid. [{0}]\n{1}", key, ex.ToString()));
                }
                else
                {
                    logger.WriteLog(ex.ToString());
                }

                // メッセージ取得
                blogic.GetResourceName(procData, new List<string> { "941010004" }, out IDictionary<string, string> resources);
                // アクセスが不正です。
                string message = blogic.ConvertResourceName("941010004", resources);
                return returnActionResult(ReturnType.AccessError, new List<string> { message }, sourceURL: RedirectSiteTopURL); // TOP画面へ遷移
            }
        }
        #endregion

    }
}
