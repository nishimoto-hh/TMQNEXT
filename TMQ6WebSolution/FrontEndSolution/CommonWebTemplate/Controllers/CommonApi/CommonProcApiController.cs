///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　共通処理用WebAPIController
/// 説明　　　：　指定された共通処理を行います。
/// 　　　　　　　処理区分：
///　　　　　　　　-1:データ取得
/// 　　　　　　　　0:検索
/// 　　　　　　　　1:実行
/// 　　　　　　　　4:出力
///　　　　　　　　 5:EXCEL出力
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net;

using CommonWebTemplate.Models.Common;
using CommonWebTemplate.CommonUtil;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace CommonWebTemplate.Controllers.CommonApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonProcApiController : BaseApiController
    {
        #region === メンバ変数 ===
        #region === private変数 ===
        private CommonDataEntities context;
        #endregion
        /// <summary>ログ出力</summary>
        protected CommonLogger logger = CommonLogger.GetInstance();
        #endregion

        #region === コンストラクタ ===
        public CommonProcApiController(CommonDataEntities dbContext) : base()
        {
            this.context = dbContext;
        }
        #endregion

        #region === ｲﾍﾞﾝﾄ処理 ===
        /// <summary>
        /// 【GET】
        /// 処理区分に応じた共通処理
        /// 　-1:データ取得
        /// 　4:出力(※ベタ表)
        /// </summary>
        /// <param name="id">処理区分</param>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ
        /// 　ConductId:機能ID
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID
        /// 　FormNo:画面NO
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID
        /// 　　※ﾃﾞｰﾀ取得を行う一覧のｺﾝﾄﾛｰﾙID
        /// 　PageNo:ﾍﾟｰｼﾞ番号
        /// 　　※-1:ﾃﾞｰﾀ取得の場合、先頭ﾍﾟｰｼﾞ
        /// 　PageCount:1ﾍﾟｰｼﾞ表示行数
        /// 　　※-1:ﾃﾞｰﾀ取得の場合、全件取得
        /// </param>
        /// <returns>結果データ</returns>
        /// <remarks>
        /// 取得用URL:http://localhost:****/api/CommonProcApi/[処理区分]
        /// </remarks>
        /// <example>
        /// (実装例)
        /// [-1:データ取得]
        ///    $.ajax({
        ///        url: '../../api/CommonProcApi/-1',
        ///        type: 'get',
        ///        data: {
        ///           conductId: "SR01720",
        ///           pgmId: "SR01720",
        ///           formNo: 0,
        ///           ctrlId: "WORK0",
        ///           pageNo: 2,
        ///           pageCount: 5,
        ///        },
        ///    });
        /// [4:出力(※ベタ表) - 非同期の場合]
        ///    $.ajax({
        ///        url: "http://localhost:****/api/CommonProcApi/5",
        ///        type: "get",
        ///        data: {
        ///            conductId: "KG00040",
        ///            pgmId: "KG00040",
        ///            formNo: 0,
        ///            ctrlId: "Report",
        ///            val1:"Taro",val2:"Tokyo",
        ///        },
        ///    });
        /// </example>
        [HttpGet("{id}")]
        public IActionResult Get(int id,
            [FromQuery] CommonProcData procData)
        {
            try
            {
                //共通区分に応じた処理に分岐
                return processExecute(id, procData);

            }
            catch (Exception ex)
            {
                //実行時例外ｴﾗｰ
                return returnActionResult(ex);
            }

        }

        ///<summary>
        /// 【POST】
        /// 処理区分に応じた共通処理
        ///   0:検索
        /// 　1:実行
        /// 　5:Excel出力
        ///</summary>
        ///<param name="id">処理区分</param>
        ///<param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ
        /// 　ConductId:機能ID
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID
        /// 　FormNo:画面NO
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID
        /// 　　※ｲﾍﾞﾝﾄﾎﾞﾀﾝのｺﾝﾄﾛｰﾙID
        /// 　ListData(val1～200):明細ﾃﾞｰﾀ一覧
        ///</param>
        ///<returns>実行結果</returns>
        ///<remarks>
        /// 取得用URL:http://localhost:****/api/CommonProcApi/[処理区分]
        ///</remarks>
        /// <example>
        /// (実装例)
        /// [0:検索]
        ///    var postdata = {
        ///        conductId: "SR01720",
        ///        pgmId: "SR01720",
        ///        formNo: 0,
        ///        ctrlId: "Search",
        ///        conditionData: {{ ctrlId: "sprCondition", rowNo: "1", val1: "Taro", val2: "Tokyo" },{...},⇒template2.0：ﾘｽﾄ型に対応
        ///        pageCount: 20,
        ///    };
        /// [1:実行]
        ///    var postdata = {
        ///        conductId: "SR01720",
        ///        pgmId: "SR01720",
        ///        formNo: 0,
        ///        ctrlId: "Regist",
        ///        listData: [
        ///            { ctrlId: "sprDetail10", rowNo: "1", val1: "Taro", val2: "Tokyo" },
        ///            { ctrlId: "sprDetail10", rowNo: "2", val1: "Taro2", val2: "Tokyo2" },
        ///        ],
        ///    };
        /// [5:Excel出力 - 非同期の場合]
        ///    var postdata = {
        ///        conductId: "SR01720",
        ///        pgmId: "SR01720",
        ///        formNo: 0,
        ///        ctrlId: "Report",
        ///        conditionData: { ctrlId: "sprCondition", rowNo: "1", val1: "Taro", val2: "Tokyo" },,{...},⇒template2.0：ﾘｽﾄ型に対応
        ///    };
        ///    
        ///    $.ajax({
        ///        url: '../../api/CommonProcApi/0',
        ///        type: 'post',
        ///        dataType: 'json',
        ///        contentType: 'application/json',
        ///        data: JSON.stringify(postdata),
        ///        traditional: true,
        ///    };
        /// </example>
        [ValidateAntiForgeryToken]
        [HttpPost("{id}")]
        public IActionResult Post(int id, [FromBody] CommonProcData procData)
        {
            try
            {
                //共通区分に応じた処理に分岐
                return processExecute(id, procData);

            }
            catch (Exception ex)
            {
                //実行時例外ｴﾗｰ
                return returnActionResult(ex);
            }
        }
        #endregion

        #region === private処理 ===
        /// <summary>
        /// 共通区分に応じた処理に分岐します。
        /// </summary>
        /// <param name="processId">共通処理区分</param>
        /// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        /// <returns>処理結果</returns>
        private ActionResult processExecute(int processId, CommonProcData procData)
        {

            BusinessLogicUtil blogic = new BusinessLogicUtil();

            //ｱｸｾｽ許可ﾁｪｯｸ
            ActionResult actionResult = accessCheck(ref blogic, procData);
            if (actionResult != null)
            {
                //※ｱｸｾｽｴﾗｰ
                return actionResult;
            }

            /* ボタン権限制御 切替 start ==================================================================== */
            ////処理権限ﾁｪｯｸ
            //if (procData.ButtonDefines != null && procData.ButtonDefines.Count > 0)
            //{
            //    switch ((short)processId)
            //    {
            //        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Search:
            //        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Execute:
            //        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Delete:
            //        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComBatExec:
            //        //case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComUpload:
            //        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComGetUserId:
            //        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Report:
            //            //※ﾎﾞﾀﾝｱｸｼｮﾝの場合に処理する

            //            //ﾕｰｻﾞｰ処理権限を取得
            //            Dictionary<string, int> authShoris = null;
            //            CommonProcReturn returnInfo = blogic.GetUserAuthShoriInConduct(procData, out authShoris);
            //            if (returnInfo.IsProcEnd())
            //            {
            //                return Content<CommonProcReturn>(HttpStatusCode.BadRequest, returnInfo);
            //            }

            //            //ﾎﾞﾀﾝの処理権限をﾁｪｯｸ
            //            if (!blogic.IsAuthShori(procData.CtrlId, authShoris))
            //            {
            //                //※処理権限なし
            //                return Content<CommonProcReturn>(HttpStatusCode.BadRequest,
            //                    new CommonProcReturn(CommonProcReturn.ProcStatus.Error, MessageConstants.Warning.W09)); //【CW00000.W09】処理権限がありません。
            //            }
            //            break;

            //        default:
            //            break;
            //    }
            //}
            /* ボタン権限制御 切替 end ==================================================================== */

            //ｱｸｼｮﾝ区分をｾｯﾄ
            procData.ActionKbn = (short)processId;

            //共通処理区分に応じた処理に分岐
            if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ListSort)
            {
                //-2:一覧ｿｰﾄ 
                object results = null;
                CommonProcReturn returnInfo = blogic.ListSortProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    return BadRequest(returnInfo);
                }

                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス 
                retObj.Add(results);        //[1]:結果データ 
                return Ok(retObj);
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.DataGet)
            {
                //-1:ﾃﾞｰﾀ取得
                object results = null;
                CommonProcReturn returnInfo = blogic.GetDataProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    return BadRequest(returnInfo);
                }

                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                retObj.Add(results);        //[1]:結果データ
                return Ok(retObj);
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Search)
            {
                //0:検索
                object results = null;
                CommonProcReturn returnInfo = blogic.SearchProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    //エラーの場合、エラー情報を返却
                    //return Content<CommonProcReturn>(HttpStatusCode.BadRequest, returnInfo);

                    IList<object> retObj2 = new List<object>();
                    retObj2.Add(returnInfo);                     //[0]:処理ステータス
                    retObj2.Add(results);                        //[1]:結果データ
                    return BadRequest(retObj2);
                }

                /* ボタン権限制御 切替 start ================================================ */
                ////ﾕｰｻﾞｰ処理権限を取得
                //Dictionary<string, int> authShoris = new Dictionary<string, int>();
                //if (procData.ButtonDefines != null && procData.ButtonDefines.Count > 0)
                //{
                //    CommonProcReturn returnInfoW = blogic.GetUserAuthShoriInConduct(procData, out authShoris);
                //    if (returnInfo.IsProcEnd())
                //    {
                //        return Content<CommonProcReturn>(HttpStatusCode.BadRequest, returnInfo);
                //    }
                //    blogic.MargeReturnInfo(ref returnInfo, returnInfoW);
                //}

                //IList<object> retObj = new List<object>();
                //retObj.Add(returnInfo);                     //[0]:処理ステータス
                //retObj.Add(results);                        //[1]:結果データ
                //retObj.Add(authShoris);                     //[2]:処理権限
                //return Ok(retObj);
                /* ========================================================================== */
                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);                     //[0]:処理ステータス
                retObj.Add(results);                        //[1]:結果データ
                return Ok(retObj);
                /* ボタン権限制御 切替 end ================================================== */
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Execute ||
                processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Delete)
            {
                //実行⇒再検索
                // 1:実行
                object results = null;
                CommonProcReturn returnInfo = blogic.ExecuteProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    IList<object> retObj2 = new List<object>();
                    retObj2.Add(returnInfo);                     //[0]:処理ステータス
                    retObj2.Add(results);                        //[1]:結果データ
                    return BadRequest(retObj2);
                }

                /* ボタン権限制御 切替 start ================================================ */
                ////ﾕｰｻﾞｰ処理権限を取得
                //Dictionary<string, int> authShoris = new Dictionary<string, int>();
                //if (procData.ButtonDefines != null && procData.ButtonDefines.Count > 0)
                //{
                //    CommonProcReturn returnInfoW = blogic.GetUserAuthShoriInConduct(procData, out authShoris);
                //    if (returnInfo.IsProcEnd())
                //    {
                //        return Content<CommonProcReturn>(HttpStatusCode.BadRequest, returnInfo);
                //    }
                //    blogic.MargeReturnInfo(ref returnInfo, returnInfoW);
                //}

                //IList<object> retObj = new List<object>();
                //retObj.Add(returnInfo);     //[0]:処理ステータス
                //retObj.Add(results);        //[1]:結果データ
                //retObj.Add(authShoris);     //[2]:処理権限
                //return Ok(retObj);
                /* ========================================================================== */

                if (returnInfo.UpdateUserInfo)
                {
                    // ユーザー権限情報の更新が必要な場合
                    UserInfoDef userInfo = null;
                    CommonProcReturn returnInfoW = blogic.GetUserAuthorizationInfo(procData, this.context, ref userInfo);
                    if (returnInfoW.IsProcEnd())
                    {
                        IList<object> retObj2 = new List<object>();
                        retObj2.Add(returnInfoW);                    //[0]:処理ステータス
                        retObj2.Add(results);                        //[1]:結果データ
                        return BadRequest(retObj2);
                    }

                    // セッションにユーザ管理情報を設定
                    HttpContext.Session.SetObject<UserInfoDef>(RequestManageUtil.SessionKey.CIM_USER_INFO, userInfo);

                    blogic.MargeReturnInfo(ref returnInfo, returnInfoW);
                }

                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                retObj.Add(results);        //[1]:結果データ
                return Ok(retObj);
                /* ボタン権限制御 切替 end ================================================== */

            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComBatExec)
            {
                //90:【共通 - バッチ機能】バッチ実行
                //バッチ実行
                object results = null;
                CommonProcReturn returnInfo = blogic.ComBatProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    //エラーの場合、エラー情報を返却
                    //return Content<CommonProcReturn>(HttpStatusCode.BadRequest, returnInfo);
                    IList<object> retObj2 = new List<object>();
                    retObj2.Add(returnInfo);                     //[0]:処理ステータス
                    retObj2.Add(results);                        //[1]:結果データ
                    return BadRequest(retObj2);
                }

                /* ボタン権限制御 切替 start ================================================ */
                ////ﾕｰｻﾞｰ処理権限を取得
                //Dictionary<string, int> authShoris = new Dictionary<string, int>();
                //if (procData.ButtonDefines != null && procData.ButtonDefines.Count > 0)
                //{
                //    CommonProcReturn returnInfoW = blogic.GetUserAuthShoriInConduct(procData, out authShoris);
                //    if (returnInfo.IsProcEnd())
                //    {
                //        return Content<CommonProcReturn>(HttpStatusCode.BadRequest, returnInfo);
                //    }
                //    blogic.MargeReturnInfo(ref returnInfo, returnInfoW);
                //}

                //IList<object> retObj = new List<object>();
                //retObj.Add(returnInfo);     //[0]:処理ステータス
                //retObj.Add(results);        //[1]:結果データ⇒★javascript側で1件取り出し
                //retObj.Add(authShoris);     //[2]:処理権限
                //return Ok(retObj);
                /* ========================================================================== */
                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                retObj.Add(results);        //[1]:結果データ⇒★javascript側で1件取り出し
                return Ok(retObj);
                /* ボタン権限制御 切替 end ================================================== */
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComBatRefresh)
            {
                //91:【共通 - バッチ機能】バッチ再表示
                object results = null;
                CommonProcReturn returnInfo = blogic.SearchProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    //エラーの場合、エラー情報を返却
                    return BadRequest(returnInfo);
                }

                /* ボタン権限制御 切替 start ================================================ */
                ////ﾕｰｻﾞｰ処理権限を取得
                //Dictionary<string, int> authShoris = new Dictionary<string, int>();
                //if (procData.ButtonDefines != null && procData.ButtonDefines.Count > 0)
                //{
                //    CommonProcReturn returnInfoW = blogic.GetUserAuthShoriInConduct(procData, out authShoris);
                //    if (returnInfo.IsProcEnd())
                //    {
                //        return Content<CommonProcReturn>(HttpStatusCode.BadRequest, returnInfo);
                //    }
                //    blogic.MargeReturnInfo(ref returnInfo, returnInfoW);
                //}

                //IList<object> retObj = new List<object>();
                //retObj.Add(returnInfo);     //[0]:処理ステータス
                //retObj.Add(results);        //[1]:結果データ⇒★javascript側で1件取り出し
                //retObj.Add(authShoris);     //[2]:処理権限
                //return Ok(retObj);
                /* ========================================================================== */
                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                retObj.Add(results);        //[1]:結果データ⇒★javascript側で1件取り出し
                return Ok(retObj);
                /* ボタン権限制御 切替 end ================================================== */
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComInitForm)
            {
                //94:【共通 - 画面初期化機能】初期値データ取得

                object results = null;
                CommonProcReturn returnInfo = blogic.InitProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    //エラーの場合、エラー情報を返却
                    return BadRequest(returnInfo);
                }

                /* ボタン権限制御 切替 start ================================================ */
                ////ﾕｰｻﾞｰ処理権限を取得
                //Dictionary<string, int> authShoris = new Dictionary<string, int>();
                //if (procData.ButtonDefines != null && procData.ButtonDefines.Count > 0)
                //{
                //    CommonProcReturn returnInfoW = blogic.GetUserAuthShoriInConduct(procData, out authShoris);
                //    if (returnInfo.IsProcEnd())
                //    {
                //        return Content<CommonProcReturn>(HttpStatusCode.BadRequest, returnInfo);
                //    }
                //    blogic.MargeReturnInfo(ref returnInfo, returnInfoW);
                //}

                //IList<object> retObj = new List<object>();
                //retObj.Add(returnInfo);                     //[0]:処理ステータス
                //retObj.Add(results);                        //[1]:結果データ
                //retObj.Add(authShoris);                     //[2]:処理権限
                //return Ok(retObj);
                /* ========================================================================== */

                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);                     //[0]:処理ステータス
                retObj.Add(results);                        //[1]:結果データ
                return Ok(retObj);
                /* ボタン権限制御 切替 end ================================================== */
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComPassChangeExec)
            {
                //84:【共通 - パスワード変更】パスワード変更実行
                BusinessLogicIO logicIO = new BusinessLogicIO(procData);
                CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_ChangePassword(procData);
                if (returnInfo.IsProcEnd())
                {
                    //エラーの場合、エラー情報を返却
                    return BadRequest(returnInfo);

                    //※個別エラー 後日対応予定
                    //IList<object> retObj2 = new List<object>();
                    //retObj2.Add(returnInfo);                     //[0]:処理ステータス
                    //retObj2.Add(results);                        //[1]:結果データ
                    //return BadRequest(retObj2);
                }

                return Ok(returnInfo);      //処理ステータス
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComSetDispItemSave)
            {
                //27:【共通 - 一覧表示列設定】保存実行

                CommonDataUtil commonDataUtil = new CommonDataUtil(this.context);
                CommonProcReturn returnInfo = new CommonProcReturn();

                //画面情報を共通＿一覧項目ユーザ定義マスタに登録
                using (IDbContextTransaction transaction = commonDataUtil.DbBeginTrans())
                {
                    try
                    {
                        //画面情報を中間ﾃｰﾌﾞﾙに保存
                        //※Delete⇒Insert
                        returnInfo = commonDataUtil.SaveItemUsertbl(procData);

                        transaction.Commit();
                    }
                    catch (DbException ex)
                    {
                        transaction.Rollback();

                        //実行時例外ｴﾗｰ
                        throw;
                    }
                }

                //処理ｽﾃｰﾀｽ：正常
                // - 処理完了ﾒｯｾｰｼﾞを設定
                if (returnInfo.STATUS == CommonProcReturn.ProcStatus.Valid &&
                    string.IsNullOrEmpty(returnInfo.MESSAGE))
                {
                    //メッセージ取得
                    blogic.GetResourceName(procData, new List<string> { "941120012" }, out IDictionary<string, string> resources);
                    //処理が完了しました。
                    returnInfo.MESSAGE = blogic.ConvertResourceName("941120012", resources);
                }
                return Ok(returnInfo);
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComGetUserId)
            {
                //96:【共通 - 双方向通信】ユーザーID取得
                CommonProcReturn returnInfo = new CommonProcReturn();
                returnInfo.STATUS = CommonProcReturn.ProcStatus.Valid;

                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                retObj.Add(procData.LoginUserId);     //[1]:ﾕｰｻﾞｰID
                return Ok(retObj);
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Report)
            {
                // Excel出力(非同期)
                object results = null;
                CommonProcReturn returnInfo = blogic.ReportProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    //エラーの場合、エラー情報を返却
                    //return Content<CommonProcReturn>(HttpStatusCode.BadRequest, returnInfo);
                    IList<object> retObj2 = new List<object>();
                    retObj2.Add(returnInfo);                     //[0]:処理ステータス
                    retObj2.Add(results);                        //[1]:結果データ
                    return BadRequest(retObj2);
                }

                // ファイル情報をセッションに設定、再リクエストの際のパラメータをそのキーに変更
                var session = HttpContext.Session;
                blogic.SetSessionFileInfo(ref session, ref returnInfo, procData.LoginUserId);

                /* ボタン権限制御 切替 start ================================================ */
                ////ﾕｰｻﾞｰ処理権限を取得
                //Dictionary<string, int> authShoris = new Dictionary<string, int>();
                //if (procData.ButtonDefines != null && procData.ButtonDefines.Count > 0)
                //{
                //    CommonProcReturn returnInfoW = blogic.GetUserAuthShoriInConduct(procData, out authShoris);
                //    if (returnInfo.IsProcEnd())
                //    {
                //        return Content<CommonProcReturn>(HttpStatusCode.BadRequest, returnInfo);
                //    }
                //    blogic.MargeReturnInfo(ref returnInfo, returnInfoW);
                //}

                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                retObj.Add(results);        //[1]:結果データ
                return Ok(retObj);
                /* ボタン権限制御 切替 end ================================================== */
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ExcelPortDownload)
            {
                // ExcelPortダウンロード
                object results = null;
                CommonProcReturn returnInfo = blogic.ReportProcess(procData, out results);
                if (returnInfo.IsProcEnd())
                {
                    //エラーの場合、エラー情報を返却
                    IList<object> retObj2 = new List<object>();
                    retObj2.Add(returnInfo);                     //[0]:処理ステータス
                    retObj2.Add(results);                        //[1]:結果データ
                    return BadRequest(retObj2);
                }

                // ファイル情報をセッションに設定、再リクエストの際のパラメータをそのキーに変更
                var session = HttpContext.Session;
                blogic.SetSessionFileInfo(ref session, ref returnInfo, procData.LoginUserId);

                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                retObj.Add(results);        //[1]:結果データ
                return Ok(retObj);
            }
            //else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ExcelPortUpload)
            //{
            //    // ExcelPortアップロード
            //    object results = null;
            //    CommonProcReturn returnInfo = blogic.ReportProcess(procData, out results);
            //    if (returnInfo.IsProcEnd())
            //    {
            //        //エラーの場合、エラー情報を返却
            //        IList<object> retObj2 = new List<object>();
            //        retObj2.Add(returnInfo);                     //[0]:処理ステータス
            //        retObj2.Add(results);                        //[1]:結果データ

            //        // ファイル情報をセッションに設定、再リクエストの際のパラメータをそのキーに変更
            //        var session = HttpContext.Session;
            //        blogic.SetSessionFileInfo(ref session, ref returnInfo, procData.LoginUserId);

            //        return BadRequest(retObj2);
            //    }

            //    IList<object> retObj = new List<object>();
            //    retObj.Add(returnInfo);     //[0]:処理ステータス
            //    retObj.Add(results);        //[1]:結果データ
            //    return Ok(retObj);
            //}
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComGetStructureList)
            {
                //【共通 - 階層ツリー】構成マスタ情報取得
                var structureList = new Dictionary<string, List<CommonTreeViewInfo>>();
                CommonProcReturn returnInfo = blogic.GetStructureList(procData, ref structureList);
                if (returnInfo.IsProcEnd())
                {
                    //エラーの場合、エラー情報を返却
                    return BadRequest(returnInfo);
                }

                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                retObj.Add(structureList);  //[1]:構成マスタ情報
                retObj.Add(procData.BelongingInfo.DutyFactoryId);   //[2]:本務工場ID
                return Ok(retObj);
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComItemCustomizeApply)
            {
                //【共通】項目カスタマイズ情報登録
                var structureList = new Dictionary<string, List<CommonTreeViewInfo>>();
                CommonProcReturn returnInfo = blogic.SaveCustomizeListInfo(procData);
                if (returnInfo.IsProcEnd())
                {
                    //エラーの場合、エラー情報を返却
                    return BadRequest(returnInfo);
                }

                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                return Ok(retObj);
            }
            else if (processId == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ChangeLanguage)
            {
                //【共通】言語切り替え

                //セッションのユーザ情報を取得
                UserInfoDef userInfo = ControllerContext.HttpContext.Session.GetObject<UserInfoDef>(RequestManageUtil.SessionKey.CIM_USER_INFO);
                //言語を設定
                userInfo.LanguageId = procData.SelectLanguageId;
                procData.LanguageId = procData.SelectLanguageId;

                //ユーザ権限機能情報リスト
                IList<CommonConductMst> resultsMenu = null;
                CommonProcReturn returnInfo = blogic.GetUserConductAuthorityList(procData, this.context, out resultsMenu);
                if (returnInfo.IsProcEnd())
                {
                    //エラーの場合、エラー情報を返却
                    return BadRequest(returnInfo);
                }
                if (resultsMenu == null)
                {
                    userInfo.UserAuthConducts = new List<CommonConductMst>();   //初期化
                }
                else
                {
                    userInfo.UserAuthConducts = resultsMenu;
                }
                //セッションにユーザ情報を保存
                HttpContext.Session.SetObject<UserInfoDef>(RequestManageUtil.SessionKey.CIM_USER_INFO, userInfo);

                IList<object> retObj = new List<object>();
                retObj.Add(returnInfo);     //[0]:処理ステータス
                return Ok(retObj);
            }
            else
            {
                //ﾘｸｴｽﾄｴﾗｰ

                //メッセージ取得
                blogic.GetResourceName(procData, new List<string> { "941400001" }, out IDictionary<string, string> resources);
                //リクエストが不正です。
                string message = blogic.ConvertResourceName("941400001", resources);
                return BadRequest(new CommonProcReturn(CommonProcReturn.ProcStatus.InValid, message));
            }

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
        private ActionResult accessCheck(ref BusinessLogicUtil blogic, CommonProcData procData)
        {
            //ｾｯｼｮﾝﾀｲﾑｱｳﾄﾁｪｯｸ
            if (!HttpContext.Session.IsAvailable)
            {
                //※ｾｯｼｮﾝﾀｲﾑｱｳﾄ

                //エラー情報を返却
                return BadRequest(new CommonProcReturn(CommonProcReturn.ProcStatus.TimeOut, string.Empty));
            }

            //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
            SetRequestInfo(ref procData);

            if (procData.LanguageId == null)
            {
                //ブラウザの言語を取得
                procData.LanguageId = GetBrowserLanguage();
            }

            //ﾘｸｴｽﾄﾊﾟﾗﾒｰﾀのﾃﾞｰﾀ検証
            CommonProcReturn returnInfo = blogic.ValidateRequestParameters(procData);

            //戻り値を設定
            if (returnInfo != null)
            {
                //エラー情報を返却
                return BadRequest(returnInfo);
            }

            return null;
        }

        /// <summary>
        /// webｱﾌﾟﾘ内例外ｴﾗｰ発生時の戻り値作成
        /// </summary>
        /// <param name="exParam"></param>
        /// <returns></returns>
        private ActionResult returnActionResult(Exception exParam)
        {
            //処理に失敗しました。
            string messageStr = getMessage();
            try
            {
                //実行時例外ｴﾗｰ

                //ﾛｸﾞ用のﾒｯｾｰｼﾞ生成
                StringBuilder errLogMsg = new StringBuilder();
                errLogMsg.Append(exParam.ToString());
                if (exParam.InnerException != null)
                {
                    errLogMsg.AppendLine("").Append(" > " + exParam.InnerException.ToString());
                }
                //ﾛｸﾞ出力
                logger.WriteLog(errLogMsg.ToString());

                return BadRequest(new CommonProcReturn(CommonProcReturn.ProcStatus.InValid, messageStr));

            }
            catch
            {
                // ログ出力で例外が発生した場合はメッセージを返すのみ
                return BadRequest(new CommonProcReturn(CommonProcReturn.ProcStatus.InValid, messageStr));
            }

            //メッセージ取得
            string getMessage()
            {
                try
                {
                    //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
                    CommonProcData procData = new CommonProcData();
                    SetRequestInfo(ref procData);
                    var languageId = Request.GetTypedHeaders().AcceptLanguage.OrderByDescending(x => x.Quality ?? 1).Select(x => x.Value.ToString()).ToList().FirstOrDefault();
                    if (procData.LanguageId == null)
                    {
                        procData.LanguageId = languageId;
                    }

                    BusinessLogicUtil blogic = new BusinessLogicUtil();
                    //メッセージ取得
                    blogic.GetResourceName(procData, new List<string> { "941120007" }, out IDictionary<string, string> resources);
                    //処理に失敗しました。
                    return blogic.ConvertResourceName("941120007", resources);
                }
                catch
                {
                    // メッセージ取得で例外発生時は固定メッセージを返す
                    //処理に失敗しました。
                    return "Processing failed.";
                }
            }
        }
        #endregion

    }
}