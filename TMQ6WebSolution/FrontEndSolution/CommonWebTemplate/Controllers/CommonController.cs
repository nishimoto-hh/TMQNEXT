///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　画面制御用Controller 
/// 説明　　　：　画面定義取得、画面遷移先の制御等を行います。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

using System;
using System.Collections.Generic;

using CommonWebTemplate.Models.Common;
using CommonWebTemplate.CommonUtil;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.IO;

namespace CommonWebTemplate.Controllers.Common
{

    public class CommonController : BaseController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonController(IWebHostEnvironment hostingEnvironment, CommonDataEntities dbContext)
            : base(hostingEnvironment, dbContext)
        {
        }

        #region === Actionｲﾍﾞﾝﾄ処理 ===
        /// <summary>
        /// GET/POST:一覧画面
        /// </summary>
        /// <param name="ctrlId">検索時のCTRLID</param>
        /// <returns>
        /// CommonConductMst(ﾘｽﾄ)
        ///  - 機能ﾏｽﾀ
        ///  - 画面定義
        ///    - 一覧項目定義
        ///    - 条件用中間ﾃﾞｰﾀ
        ///  - ﾒﾆｭｰ情報
        ///  </returns>
        [ValidateAntiForgeryToken]
        [Produces("application/json")]
        public ActionResult Index(CommonProcData procData, int rowNo = 0, byte childno = 0, string key = "")
        {
            try
            {
                ActionResult actionResult = null;
                BusinessLogicUtil blogic = new BusinessLogicUtil();

                //=== ｱｸｾｽ許可ﾁｪｯｸ ===
                actionResult = accessCheck(ref blogic, procData);
                if (actionResult != null)
                {
                    //ｱｸｾｽｴﾗｰ画面に遷移
                    return actionResult;
                }

                //処理結果を初期化
                CommonProcReturn returnInfo = null;

                //=== 画面表示用処理 ==
                // - 画面定義
                // - 条件ﾃﾞｰﾀ
                List<CommonConductMst> results = null;
                if (!string.IsNullOrEmpty(key))
                {
                    if(procData.ConditionData == null)
                    {
                        procData.ConditionData = new List<Dictionary<string, object>>();
                    }
                    procData.ConditionData.Add(new Dictionary<string, object>() { { "KEY", key.Split(',') } });
                }
                CommonProcReturn returnInfoW = blogic.GetViewData(this._context, BusinessLogicUtil.FormId.Index, procData, out results);

                //処理結果をマージ
                blogic.MargeReturnInfo(ref returnInfo, returnInfoW);

                //=== 戻り値を検証 ===
                actionResult = returnActionResult(ReturnType.Normal, returnInfo);
                if (actionResult != null)
                {
                    //例外ｴﾗｰ画面に遷移
                    return actionResult;
                }

                //=== 一覧画面を表示 ===
                ViewBag.CtrlId = "";
                if (false == string.IsNullOrEmpty(procData.CtrlId))
                {
                    ViewBag.CtrlId = procData.CtrlId;
                }
                //他機能遷移時のﾊﾟﾗﾒｰﾀ
                ViewBag.ParamFormNo = procData.FormNo;

                // JSON文字列に変換して渡す
                //ViewBag.ParamConditionData = procData.ConditionData;
                //ViewBag.ParamBackConditions = procData.ListIndividual;
                ViewBag.TransDictionaryJson = JsonSerializer.Serialize(results[0].TransDictionary); //翻訳ﾃﾞｰﾀ
                ViewBag.ParamConditionData = JsonSerializer.Serialize(procData.ConditionData);      //検索条件ﾃﾞｰﾀ
                ViewBag.ParamBackConditions = JsonSerializer.Serialize(procData.ListIndividual);    //個別変数ﾃﾞｰﾀ

                //ﾎﾞﾀﾝ権限ﾃﾞｰﾀ
                // - Index.cshtml同じ形式に詰めなおす
                var UserAuthConductShoris = new Dictionary<string, object>();   //ﾎﾞﾀﾝ権限情報を機能IDをKeyに保持
                UserAuthConductShoris.Add(results[0].CONDUCTMST.CONDUCTID, results[0].UserAuthConductShoris);
                if (results[0].CM_CONDUCTMSTS != null)
                {
                    foreach (var conductMst in results[0].CM_CONDUCTMSTS)
                    {
                        UserAuthConductShoris.Add(conductMst.CONDUCTMST.CONDUCTID, conductMst.UserAuthConductShoris);
                    }
                }
                // - JSON形式に変換してｾｯﾄ
                ViewBag.UserAuthConductShorisJson = JsonSerializer.Serialize(UserAuthConductShoris);

                return View(results);
            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        //★del by 2021.04.30 template2.0 廃止(Indexﾋﾞｭｰ内、_EditFormPartialに移植
        ///// <summary>
        ///// POST:明細入力画面 - 明細データ取得処理
        ///// </summary>
        ///// <returns>
        ///// CommonConductMst(ﾘｽﾄ)
        /////  - 機能ﾏｽﾀ
        /////  - 画面定義
        /////    - 一覧項目定義
        /////    - 条件用中間ﾃﾞｰﾀ
        /////  - ﾒﾆｭｰ情報
        /////  </returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(CommonProcData procData, int rowNo = 0)
        //{
        //    try
        //    {
        //        ActionResult actionResult = null;
        //        BusinessLogicUtil blogic = new BusinessLogicUtil();

        //        //=== ｱｸｾｽ許可ﾁｪｯｸ ===
        //        actionResult = accessCheck(ref blogic, procData);
        //        if (actionResult != null)
        //        {
        //            //ｱｸｾｽｴﾗｰ画面に遷移
        //            return actionResult;
        //        }

        //        //=== 画面表示用処理 ==
        //        // - 画面定義
        //        // - 条件ﾃﾞｰﾀ
        //        List<CommonConductMst> results = null;
        //        CommonProcReturn returnInfo = blogic.GetViewData(BusinessLogicUtil.FormId.Edit, procData, rowNo, out results);

        //        //=== 戻り値を検証 ===
        //        actionResult = returnActionResult(ReturnType.Normal, returnInfo);
        //        if (actionResult != null)
        //        {
        //            //例外ｴﾗｰ画面に遷移
        //            return actionResult;
        //        }

        //        //=== 明細入力画面を表示 ===
        //        return View(results);
        //    }
        //    catch (Exception ex)
        //    {
        //        //例外ｴﾗｰ画面に遷移
        //        return returnActionResult(ex);
        //    }
        //}
        //★del by 2021.04.30

        /// <summary>
        /// POST:Excel出力（待ち）画面 - 非同期処理、新規ﾀﾌﾞで処理完了を待ち受け
        /// </summary>
        /// <returns>
        /// CommonConductMst(ﾘｽﾄ)
        ///  - 機能ﾏｽﾀ
        ///  - 画面定義
        ///    - 一覧項目定義
        ///    - 条件用中間ﾃﾞｰﾀ
        ///  - ﾒﾆｭｰ情報
        ///  </returns>
        [ValidateAntiForgeryToken]
        public ActionResult Report(CommonProcData procData, int output = 0, List<Dictionary<string, object>> buttonDefines = null, string fileName = "", string filePath = "")
        {
            try
            {
                ActionResult actionResult = null;
                BusinessLogicUtil blogic = new BusinessLogicUtil();
                CommonProcReturn returnInfo = null;

                //=== ｱｸｾｽ許可ﾁｪｯｸ ===
                actionResult = accessCheck(ref blogic, procData);
                if (actionResult != null)
                {
                    //ｱｸｾｽｴﾗｰ画面に遷移
                    return actionResult;
                    //★
                    //return Content("Error", "text/plain");
                }

                /* ボタン権限制御 切替 start ===================================================== */
                ////ﾕｰｻﾞｰ処理権限を取得
                //Dictionary<string, int> authShoris = null;
                ////List<Dictionary<string, object>> authShoris = null;
                //returnInfo = blogic.GetUserAuthShoriInConduct(procData, out authShoris);
                ////戻り値を検証
                //actionResult = returnActionResult(ReturnType.Normal, returnInfo);
                //if (actionResult != null)
                //{
                //    //例外ｴﾗｰ画面に遷移
                //    return actionResult;
                //}
                ////ﾎﾞﾀﾝの処理権限をﾁｪｯｸ
                //if (!blogic.IsAuthShori(procData.CtrlId, authShoris))
                //{
                //    //※処理権限なし

                //    returnInfo = new CommonProcReturn(CommonProcReturn.ProcStatus.Error, MessageConstants.Warning.W09); //【CW00000.W09】処理権限がありません。
                //                                                                                                        //戻り値を検証
                //    actionResult = returnActionResult(ReturnType.AccessError, returnInfo);
                //    if (actionResult != null)
                //    {
                //        //例外ｴﾗｰ画面に遷移
                //        return actionResult;
                //    }
                //}
                /* ボタン権限制御 切替 end ======================================================= */

                if (output == 1)
                {
                    //※帳票出力時

                    // 5:Excel出力
                    //⇒ﾎﾞﾀﾝのACTIONKBN = 51:Excel出力(非同期)の場合もprocessId=5でｺｰﾙされる
                    procData.ActionKbn = LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Report;

                    //Excel作成（出力Check／出力ﾌﾟﾛｼｰｼﾞｬｺｰﾙのみ）
                    object results = null;
                    returnInfo = blogic.ReportProcess(procData, out results);
                    //戻り値を検証
                    actionResult = returnActionResult(ReturnType.Normal, returnInfo);
                    if (actionResult != null)
                    {
                        //例外ｴﾗｰ画面に遷移
                        return actionResult;
                    }

                    //※正常終了の場合

                    //Excelをﾀﾞｳﾝﾛｰﾄﾞ
                    string contentType = SystemUtil.GetContentType(returnInfo.FILEDOWNLOADNAME);
                    return File(returnInfo.FILEDATA,
                        contentType,
                        returnInfo.FILEDOWNLOADNAME);
                }
                else if (output == 2)
                {
                    // パラメータで取得したファイルパスとファイル名のセッションキーより値を取得する
                    var session = HttpContext.Session;
                    string orgFileName = fileName;
                    fileName = session.GetString(fileName);
                    session.Remove(orgFileName);
                    string orgFilePath = filePath;
                    filePath = session.GetString(filePath);
                    session.Remove(orgFilePath);

                    try
                    {
                        //Excelをﾀﾞｳﾝﾛｰﾄﾞ
                        string contentType = SystemUtil.GetContentType(filePath);

                        // ASP.NET Coreでは物理パス指定の場合、File()メソッドを使えないのでバイト配列に変換してから
                        //return File(filePath,
                        //    contentType,
                        //    fileName);
                        //var fileData = System.IO.File.ReadAllBytes(filePath);
                        //return File(fileData,
                        //    contentType,
                        //    fileName);
                        //ﾌｧｲﾙ読込み
                        byte[] btf_fata = null;
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var memStream = new MemoryStream())
                            {
                                // 取込ファイルのバイト配列をコピー
                                fs.CopyTo(memStream);
                                btf_fata = memStream.ToArray();
                            }

                        }

                        string downloadFileName = fileName;
                        //ﾌｧｲﾙ名からﾕｰｻﾞｰIDを除去
                        //string downloadFileName = fileName.TrimStart(procData.LoginUserId.ToCharArray());
                        if (fileName.IndexOf(procData.LoginUserId) == 0)
                        {
                            downloadFileName = fileName.Remove(0, procData.LoginUserId.Length);
                        }
                        //不要な「_」(アンダーバー)を除去
                        downloadFileName = downloadFileName.TrimStart("_".ToCharArray());

                        //読み込んだﾌｧｲﾙのﾊﾞｲﾄ配列を渡す
                        return File(btf_fata,
                            contentType,
                            downloadFileName);
                    }
                    finally
                    {
                        //ﾀﾞｳﾝﾛｰﾄﾞﾌｧｲﾙを削除
                        FileUtil.DeleteFile(filePath);
                    }
                }
                else
                {
                    //※画面表示時

                    //=== 画面表示用処理 ===
                    //正常ﾒｯｾｰｼﾞを設定
                    returnActionResult(ReturnType.NormalMessage, "Excel作成中...　しばらくお待ちください。");
                    //Excel出力（待ち）画面を表示
                    return View();
                }

            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        /// <summary>
        /// POST:Excel出力（専用帳票）
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="procData"></param>
        /// <returns>
        /// FileContentResult
        ///  - 生成したEXCELﾌｧｲﾙをﾀﾞｳﾝﾛｰﾄﾞ
        /// </returns>
        /// <remarks>
        /// POSTによる同期処理
        ///  ※作成ﾌｧｲﾙ情報をByte配列で生成⇒ﾀﾞｳﾝﾛｰﾄﾞ
        /// </remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportOut(CommonProcData procData)
        {
            //代表ﾒｯｾｰｼﾞを初期化
            ViewBag.ErrorMessage = new List<string>();
            ViewBag.Message = string.Empty;

            string filePath = string.Empty;
            try
            {
                //ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞを同期に設定
                procData.FileDownloadSet = AppConstants.FileDownloadSet.Douki.GetHashCode();

                ActionResult actionResult = null;
                BusinessLogicUtil blogic = new BusinessLogicUtil();

                //=== ｱｸｾｽ許可ﾁｪｯｸ ===
                actionResult = accessCheck(ref blogic, procData);
                if (actionResult != null)
                {
                    //ｱｸｾｽｴﾗｰ画面に遷移
                    return actionResult;
                }

                //=== Excel帳票作成処理 ===
                object results = null;
                CommonProcReturn returnInfo = blogic.ReportProcess(procData, out results);

                //=== 戻り値を検証 ===
                actionResult = returnActionResult(ReturnType.Normal, returnInfo);
                if (actionResult != null)
                {
                    //例外ｴﾗｰ画面に遷移
                    return actionResult;
                }
                if (returnInfo.IsProcEnd())
                {
                    //ｴﾗｰﾍﾟｰｼﾞに遷移
                    return returnActionResult(ReturnType.Error, returnInfo);
                }

                //※正常終了の場合

                //=== Excelをﾀﾞｳﾝﾛｰﾄﾞ ===
                string contentType = SystemUtil.GetContentType(returnInfo.FILEDOWNLOADNAME);
                return File(returnInfo.FILEDATA,
                    contentType,
                    returnInfo.FILEDOWNLOADNAME);

            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
            finally
            {
                ////ﾀﾞｳﾝﾛｰﾄﾞﾌｧｲﾙを削除
                //FileUtil.DeleteFile(filePath);
            }
        }

        /// <summary>
        /// GET/POST:一覧画面
        /// </summary>
        /// <param name="ctrlId">検索時のCTRLID</param>
        /// <returns>
        /// CommonConductMst(ﾘｽﾄ)
        ///  - 機能ﾏｽﾀ
        ///  - 画面定義
        ///    - 一覧項目定義
        ///    - 条件用中間ﾃﾞｰﾀ
        ///  - ﾒﾆｭｰ情報
        ///  </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FileUpload(CommonProcData procData)
        {
            //代表ﾒｯｾｰｼﾞを初期化
            ViewBag.ErrorMessage = new List<string>();
            ViewBag.Message = string.Empty;

            try
            {
                ActionResult actionResult = null;
                BusinessLogicUtil blogic = new BusinessLogicUtil();

                //=== ｱｸｾｽ許可ﾁｪｯｸ ===
                //アップロードファイルデータはリクエスト情報から取得する
                actionResult = accessCheck(ref blogic, procData);
                if (actionResult != null)
                {
                    //ｱｸｾｽｴﾗｰ画面に遷移
                    return actionResult;
                }

                //【共通 - 取り込み機能】取り込みボタン押下時処理
                procData.ActionKbn = LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComUpload;

                //ファイル取り込み
                object results = null;
                CommonProcReturn returnInfo = blogic.ComUploadProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    //=== 処理結果を返す ===
                    IList<object> retObj_err = new List<object>();
                    retObj_err.Add(returnInfo);                     //[0]:処理ステータス
                    retObj_err.Add(results);                        //[1]:結果データ
                    //return Json(retObj_err, "application/json");
                    return Json(retObj_err);
                }

                /* ボタン権限制御 切替 start ================================================ */
                //Dictionary<string, int> authShoris = null;
                ////=== 処理結果を返す ===
                //IList<object> retObj = new List<object>();
                //retObj.Add(returnInfo);     //[0]:処理ステータス
                //retObj.Add(results);        //[1]:結果データ
                //retObj.Add(authShoris);     //[2]:処理権限
                //return Json(retObj, "application/json");
                /* ========================================================================== */
                //=== 処理結果を返す ===
                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                retObj.Add(results);        //[1]:結果データ
                //return Json(retObj, "application/json");
                return Json(retObj);
                /* ボタン権限制御 切替 end ================================================== */
            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        /// <summary>
        /// GET/POST:ExcelPortアップロード
        /// </summary>
        /// <param name="procData"></param>
        /// <returns>
        /// CommonConductMst(ﾘｽﾄ)
        ///  - 機能ﾏｽﾀ
        ///  - 画面定義
        ///    - 一覧項目定義
        ///    - 条件用中間ﾃﾞｰﾀ
        ///  - ﾒﾆｭｰ情報
        ///  </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcelPortUpload(CommonProcData procData)
        {
            //代表ﾒｯｾｰｼﾞを初期化
            ViewBag.ErrorMessage = new List<string>();
            ViewBag.Message = string.Empty;

            try
            {
                ActionResult actionResult = null;
                BusinessLogicUtil blogic = new BusinessLogicUtil();

                //=== ｱｸｾｽ許可ﾁｪｯｸ ===
                //アップロードファイルデータはリクエスト情報から取得する
                actionResult = accessCheck(ref blogic, procData);
                if (actionResult != null)
                {
                    //ｱｸｾｽｴﾗｰ画面に遷移
                    return actionResult;
                }

                //【ExcelPortアップロード】アップロードボタン押下時処理
                procData.ActionKbn = LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ExcelPortUpload;

                //ファイル取り込み
                object results = null;
                CommonProcReturn returnInfo = blogic.ComUploadProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    //=== 処理結果を返す ===
                    IList<object> retObj_err = new List<object>();
                    retObj_err.Add(returnInfo);                     //[0]:処理ステータス
                    retObj_err.Add(results);                        //[1]:結果データ
                    return Json(retObj_err);
                }

                //=== 処理結果を返す ===
                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                retObj.Add(results);        //[1]:結果データ
                return Json(retObj);
            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        /// <summary>
        /// GET:URL直接入力-画面遷移
        /// </summary>
        /// <param name="key">遷移先キー(JSON文字列)</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Move(string key)
        {
            try
            {
                BusinessLogicUtil blogic = new BusinessLogicUtil();

                // セッションからログイン情報を取得
                CommonProcData procData = new CommonProcData();
                SetRequestInfo(ref procData);

                if (string.IsNullOrEmpty(key))
                {
                    // アクセスエラー画面に遷移
                    // アクセスが不正です。
                    blogic.GetResourceName(procData, new List<string> { "941010004" }, out IDictionary<string, string> resources);
                    string message = blogic.ConvertResourceName("941010004", resources);
                    return returnActionResult(ReturnType.AccessError, new List<string>() { message });
                }

                // 偽造防止トークンを取得
                var token = getRequestVerificationToken(Request);
                if (string.IsNullOrEmpty(procData.LoginUserId) || string.IsNullOrEmpty(token))
                {
                    // 未ログインの場合、ログイン画面へ遷移
                    //メッセージ取得
                    blogic.GetResourceName(procData, new List<string> { "941430005" }, out IDictionary<string, string> resources);

                    //ログインしてください。
                    string message = blogic.ConvertResourceName("941430005", resources);
                    // POSTパラメータの生成
                    Dictionary<string, object> postData = new Dictionary<string, object>();
                    postData.Add("ACCESS_KEY", key);
                    postData.Add("MESSAGE", message);

                    // HomeコントローラのIndexアクションへPOSTメソッドでリダイレクト
                    var url = getRedirectUrl(Request, "/Home/");
                    return (ActionResult)ActionResultEx.RedirectAndPost(url, postData);
                }
                else
                {
                    // ログイン済みの場合、指定機能へ遷移
                    return redirectToCommonAction(Request, procData, key, token);
                }

            }
            catch (Exception ex)
            {
                //　例外エラー画面に遷移
                return returnActionResult(ex);
            }
        }

        /// <summary>
        /// GET:画像表示
        /// </summary>
        /// <param name="file">画像表示キー dbo.get_img_keyタグで判定</param>
        /// <returns>表示する画像ファイル</returns>
        [HttpGet]
        public ActionResult Image(string file)
        {
            try
            {
                // バックエンドの画像ファイル情報取得処理を呼び出す
                BusinessLogicUtil blogic = new BusinessLogicUtil();
                CommonProcData procData = createProcData();
                CommonProcReturn returnInfo = blogic.GetImageFileInfo(procData, file, out string filePath);

                if (returnInfo.IsProcEnd() || string.IsNullOrEmpty(filePath))
                {
                    // エラーまたは出力ファイル情報が無い場合、終了
                    return null;
                }
                // 取得したファイルの情報を返す
                string fileName = Path.GetFileName(filePath);
                string contentType = SystemUtil.GetContentType(filePath);
                //ﾌｧｲﾙ読込み
                byte[] btf_fata = null;
                using (FileStream fs = new (filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var memStream = new MemoryStream())
                    {
                        // 取込ファイルのバイト配列をコピー
                        fs.CopyTo(memStream);
                        btf_fata = memStream.ToArray();
                    }

                }

                //読み込んだﾌｧｲﾙのﾊﾞｲﾄ配列を渡す
                return File(btf_fata, contentType, fileName);
            }
            catch (Exception ex)
            {
                //　例外エラー画面に遷移
                return returnActionResult(ex);
            }

            CommonProcData createProcData()
            {
                CommonProcData rtnData = new();
                return rtnData;
            }
        }
        #endregion

        /// <summary>
        /// POST:一覧カスタマイズ情報登録＆レイアウトデータ取得
        /// </summary>
        /// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        /// <returns>部分ビュー</returns>
        [HttpPost]
        public ActionResult ItemCustomize(CommonProcData procData)
        {
            try
            {
                ActionResult actionResult = null;
                BusinessLogicUtil blogic = new BusinessLogicUtil();

                //=== ｱｸｾｽ許可ﾁｪｯｸ ===
                actionResult = accessCheck(ref blogic, procData);
                if (actionResult != null)
                {
                    //ｱｸｾｽｴﾗｰ画面に遷移
                    return actionResult;
                }

                //=== 項目カスタマイズ情報の登録 ===
                CommonProcReturn returnInfo = blogic.SaveCustomizeListInfo(procData);
                //=== 戻り値を検証 ===
                actionResult = returnActionResult(ReturnType.Normal, returnInfo);
                if (actionResult != null)
                {
                    //例外ｴﾗｰ画面に遷移
                    return actionResult;
                }

                //=== 画面レイアウトデータの取得 ===
                List<CommonConductMst> results = null;
                CommonProcReturn returnInfoW = blogic.GetViewData(this._context, BusinessLogicUtil.FormId.Index, procData, out results);

                //処理結果をマージ
                blogic.MargeReturnInfo(ref returnInfo, returnInfoW);

                //=== 戻り値を検証 ===
                actionResult = returnActionResult(ReturnType.Normal, returnInfo);
                if (actionResult != null)
                {
                    //例外ｴﾗｰ画面に遷移
                    return actionResult;
                }

                //=== 一覧画面を表示 ===
                ViewBag.CtrlId = "";
                if (false == string.IsNullOrEmpty(procData.CtrlId))
                {
                    ViewBag.CtrlId = procData.CtrlId;
                }
                //他機能遷移時のﾊﾟﾗﾒｰﾀ
                ViewBag.ParamFormNo = procData.FormNo;

                // JSON文字列に変換して渡す
                //ViewBag.ParamConditionData = procData.ConditionData;
                //ViewBag.ParamBackConditions = procData.ListIndividual;
                ViewBag.TransDictionaryJson = JsonSerializer.Serialize(results[0].TransDictionary); //翻訳ﾃﾞｰﾀ
                ViewBag.ParamConditionData = JsonSerializer.Serialize(procData.ConditionData);      //検索条件ﾃﾞｰﾀ
                ViewBag.ParamBackConditions = JsonSerializer.Serialize(procData.ListIndividual);    //個別変数ﾃﾞｰﾀ

                //ﾎﾞﾀﾝ権限ﾃﾞｰﾀ
                // - Index.cshtml同じ形式に詰めなおす
                var UserAuthConductShoris = new Dictionary<string, object>();   //ﾎﾞﾀﾝ権限情報を機能IDをKeyに保持
                UserAuthConductShoris.Add(results[0].CONDUCTMST.CONDUCTID, results[0].UserAuthConductShoris);
                if (results[0].CM_CONDUCTMSTS != null)
                {
                    foreach (var conductMst in results[0].CM_CONDUCTMSTS)
                    {
                        UserAuthConductShoris.Add(conductMst.CONDUCTMST.CONDUCTID, conductMst.UserAuthConductShoris);
                    }
                }
                // - JSON形式に変換してｾｯﾄ
                ViewBag.UserAuthConductShorisJson = JsonSerializer.Serialize(UserAuthConductShoris);

                return View("Index", results);

            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        /// <summary>
        /// POST:部分画面レイアウトデータ取得
        /// </summary>
        /// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        /// <returns>部分ビュー</returns>
        [HttpPost]
        public ActionResult GetPartialView([FromBody] CommonProcData procData)
        {
            try
            {
                ActionResult actionResult = null;
                BusinessLogicUtil blogic = new BusinessLogicUtil();

                //=== ｱｸｾｽ許可ﾁｪｯｸ ===
                actionResult = accessCheck(ref blogic, procData);
                if (actionResult != null)
                {
                    //ｱｸｾｽｴﾗｰ画面に遷移
                    return actionResult;
                }

                //=== 画面レイアウトデータの取得 ===
                List<CommonConductMst> results = null;
                // 親機能ID
                string parentConductId = procData.ConditionData[0]["parentConductId"].ToString();
                ViewBag.ParentConductId = parentConductId;
                CommonProcReturn returnInfo = blogic.GetViewData(this._context, BusinessLogicUtil.FormId.Partial, procData, out results);

                //=== 戻り値を検証 ===
                actionResult = returnActionResult(ReturnType.Normal, returnInfo);
                if (actionResult != null)
                {
                    //例外ｴﾗｰ画面に遷移
                    return actionResult;
                }

                //=== 画面レイアウトデータを返却 ===
                ViewBag.CtrlId = "";
                if (false == string.IsNullOrEmpty(procData.CtrlId))
                {
                    ViewBag.CtrlId = procData.CtrlId;
                }

                // JSON文字列に変換して渡す
                ViewBag.ParamConditionData = JsonSerializer.Serialize(procData.ConditionData);      //検索条件ﾃﾞｰﾀ
                ViewBag.ParamBackConditions = JsonSerializer.Serialize(procData.ListIndividual);    //個別変数ﾃﾞｰﾀ

                //ﾎﾞﾀﾝ権限ﾃﾞｰﾀ
                // - Index.cshtml同じ形式に詰めなおす
                var UserAuthConductShoris = new Dictionary<string, object>();   //ﾎﾞﾀﾝ権限情報を機能IDをKeyに保持
                UserAuthConductShoris.Add(results[0].CONDUCTMST.CONDUCTID, results[0].UserAuthConductShoris);
                if (results[0].CM_CONDUCTMSTS != null)
                {
                    foreach (var conductMst in results[0].CM_CONDUCTMSTS)
                    {
                        UserAuthConductShoris.Add(conductMst.CONDUCTMST.CONDUCTID, conductMst.UserAuthConductShoris);
                    }
                }
                // - JSON形式に変換してｾｯﾄ
                ViewBag.UserAuthConductShorisJson = JsonSerializer.Serialize(UserAuthConductShoris);

                return PartialView("_ArticlePartial", results[0]);

            }
            catch (Exception ex)
            {
                //例外ｴﾗｰ画面に遷移
                return returnActionResult(ex);
            }
        }

        ///// <summary>
        ///// POST:ツリービューの表示
        ///// </summary>
        ///// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        ///// <returns>部分ビュー</returns>
        //[HttpPost]
        //public ActionResult GetPartialView([FromBody] CommonProcData procData)
        //{
        //    try
        //    {   //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
        //        SetRequestInfo(ref procData);

        //        BusinessLogicUtil blogic = new BusinessLogicUtil();
        //        CommonConductMst result = new CommonConductMst();
        //        List<CommonStructure> structureList = new List<CommonStructure>();
        //        CommonProcReturn returnInfo = blogic.GetStructureList(procData, ref structureList, null);

        //        if (returnInfo.IsProcEnd())
        //        {
        //            return BadRequest(returnInfo);
        //        }

        //        return PartialView("_TreeViewPartial", structureList);

        //    }
        //    catch (Exception ex)
        //    {
        //        //例外ｴﾗｰ画面に遷移
        //        return returnActionResult(ex);
        //    }
        //}

        #region === private処理 ===
        #endregion
    }
}
