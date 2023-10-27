///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　業務ﾛｼﾞｯｸ処理クラス
/// 説明　　　：　業務ﾛｼﾞｯｸの共通処理を実装します。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

using CommonWebTemplate.Models.Common;
using System.Runtime.Serialization.Json;
using static CommonWebTemplate.CommonUtil.RequestManageUtil;
using Microsoft.AspNetCore.Http;

namespace CommonWebTemplate.CommonUtil
{
    public class BusinessLogicUtil
    {
        #region === 定数定義 ===
        public enum FormId
        {
            /// <summary>一覧画面データ</summary>
            Index = 0,
            /// <summary>部分画面データ</summary>
            Partial = 1,
            /// <summary>明細入力画面データ</summary>
            Edit
        }
        #endregion

        #region === Public処理 ===
        ///// <summary>
        ///// ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を設定して返す
        /////  - Controller用
        ///// </summary>
        ///// <param name="controllerContext"></param>
        ///// <param name="procData">業務ﾛｼﾞｯｸﾃﾞｰﾀ(I/O)</param>
        //public void SetRequestInfo(HttpRequestBase request, HttpSessionStateBase session, ref CommonProcData procData)

        //{
        //    //JSON形式のFormﾃﾞｰﾀを手動で取得
        //    //FORMNO
        //    if (request.Form.AllKeys.Contains("FORMNO"))
        //    {
        //        string jsonStr = request.Form["FORMNO"];
        //        if (jsonStr.Length > 0)
        //        {
        //            var serializer = new JavaScriptSerializer();
        //            procData.FormNo = serializer.Deserialize<short>(jsonStr);
        //        }
        //    }
        //    //条件ﾃﾞｰﾀ
        //    if (request.Form.AllKeys.Contains("ConditionData"))
        //    {
        //        string jsonStr = request.Form["ConditionData"];
        //        if (jsonStr.Length > 0)
        //        {
        //            var serializer = new JavaScriptSerializer();
        //            procData.ConditionData = serializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
        //        }
        //    }
        //    //登録ﾃﾞｰﾀ
        //    if (request.Form.AllKeys.Contains("ListData"))
        //    {
        //        string jsonStr = request.Form["ListData"];
        //        if (jsonStr.Length > 0)
        //        {
        //            var serializer = new JavaScriptSerializer();
        //            procData.ListData = serializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
        //        }
        //    }
        //    //一覧定義
        //    if (request.Form.AllKeys.Contains("ListDefines"))
        //    {
        //        string jsonStr = request.Form["ListDefines"];
        //        if (jsonStr.Length > 0)
        //        {
        //            var serializer = new JavaScriptSerializer();
        //            procData.ListDefines = serializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
        //        }
        //    }
        //    //ﾎﾞﾀﾝ定義
        //    if (request.Form.AllKeys.Contains("ButtonDefines"))
        //    {
        //        string jsonStr = request.Form["ButtonDefines"];
        //        if (jsonStr.Length > 0)
        //        {
        //            var serializer = new JavaScriptSerializer();
        //            procData.ButtonDefines = serializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
        //        }
        //    }
        //    //個別実装用ﾃﾞｰﾀﾘｽﾄ
        //    if (request.Form.AllKeys.Contains("ListIndividual"))
        //    {
        //        string jsonStr = request.Form["ListIndividual"];
        //        if (jsonStr.Length > 0)
        //        {
        //            var serializer = new JavaScriptSerializer();
        //            procData.ListIndividual = serializer.Deserialize<Dictionary<string, object>>(jsonStr);
        //        }
        //    }

        //    //IPｱﾄﾞﾚｽを取得
        //    if ("test".Equals(System.Configuration.ConfigurationManager.AppSettings["DeployMode"]) ||
        //        "prot".Equals(System.Configuration.ConfigurationManager.AppSettings["DeployMode"]))
        //    {
        //        //プロト作成モード
        //        procData.TerminalNo = @"::1";
        //    }
        //    else
        //    {
        //        //Releaseモード
        //        if (String.IsNullOrEmpty(procData.TerminalNo))
        //        {
        //            procData.TerminalNo = SystemUtil.GetClientIpAdress(request);
        //        }
        //    }

        //    //Urlﾊﾟｽを取得
        //    procData.BaseUrl = SystemUtil.GetBaseUrlFromRequest(request);

        //    //ｾｯｼｮﾝに保持しているﾕｰｻﾞｰ情報を取得
        //    if (session[SessionKey.CIM_USER_INFO] != null)
        //    {
        //        UserInfoDef userInfo = (UserInfoDef)session[SessionKey.CIM_USER_INFO];
        //        procData.LoginUserId = userInfo.UserId;
        //        procData.LoginUserName = userInfo.UserName;
        //        procData.LanguageId = userInfo.LanguageId;
        //        procData.GUID = userInfo.GUID;

        //        //=== 権限情報 ==
        //        //ﾕｰｻﾞｰ機能権限のある機能ﾏｽﾀﾘｽﾄ
        //        procData.UserAuthConducts = userInfo.UserAuthConducts;
        //    }
        //}

        ///// <summary>
        ///// ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を設定して返す
        /////  - WebApi Controller用
        ///// </summary>
        ///// <param name="controllerContext"></param>
        ///// <param name="procData">業務ﾛｼﾞｯｸﾃﾞｰﾀ(I/O)</param>
        //public void SetRequestInfo(HttpControllerContext controllerContext,IHttpSessionState session, ref CommonProcData procData)
        //{
        //    //IPｱﾄﾞﾚｽを取得
        //    if ("test".Equals(System.Configuration.ConfigurationManager.AppSettings["DeployMode"]) ||
        //        "prot".Equals(System.Configuration.ConfigurationManager.AppSettings["DeployMode"]))
        //    {
        //        //プロト作成モード
        //        procData.TerminalNo = @"::1";
        //    }
        //    else
        //    {
        //        if (String.IsNullOrEmpty(procData.TerminalNo))
        //        {
        //            procData.TerminalNo = SystemUtil.GetClientIpAdress(controllerContext);
        //        }
        //    }
        //    //Urlﾊﾟｽを取得
        //    procData.BaseUrl = SystemUtil.GetBaseUrlFromRequest(controllerContext);

        //    //ｾｯｼｮﾝに保持しているﾕｰｻﾞｰ情報を取得
        //    procData.LoginUserId = null;
        //    procData.LoginUserName = null;
        //    if (session[SessionKey.CIM_USER_INFO] != null)
        //    {
        //        UserInfoDef userInfo = (UserInfoDef)session[SessionKey.CIM_USER_INFO];
        //        procData.LoginUserId = userInfo.UserId;
        //        procData.LoginUserName = userInfo.UserName;
        //        procData.LanguageId = userInfo.LanguageId;
        //        procData.GUID = userInfo.GUID;

        //        //=== 権限情報 ==
        //        //ﾕｰｻﾞｰ機能権限のある機能ﾏｽﾀﾘｽﾄ
        //        procData.UserAuthConducts = userInfo.UserAuthConducts;
        //    }
        //}

        /* ボタン権限制御 切替 start ==================================================================== */
        ///// <summary>
        ///// 該当機能IDの処理権限区分ﾘｽﾄを取得する
        ///// </summary>
        ///// <param name="procData">ﾘｸｴｽﾄ情報</param>
        ///// <param name="getFormNo">画面番号</param>
        ///// <param name="authShoris">ﾎﾞﾀﾝ権限情報＞KEY：ﾎﾞﾀﾝCTRLID、VALUE：ﾎﾞﾀﾝ状態(0(非表示),1(表示・活性),2(表示・非活性))</param>
        ///// <returns>処理ステータス</returns>
        //public CommonProcReturn GetUserAuthShoriInConduct(CommonProcData procData, out Dictionary<string, int> authShoris)
        //{
        //    authShoris = new Dictionary<string, int>();

        //    //①業務ﾛｼﾞｯｸ機能単位のDLL - 権限チェック用処理
        //    //②ｴﾗｰの場合、処理を中断
        //    CommonProcData procDataW = new CommonProcData();
        //    procDataW.ConductId = procData.ConductId;       //IN:機能ID
        //    procDataW.PgmId = procData.PgmId;               //IN:PGMID
        //    procDataW.FormNo = procData.FormNo;             //IN:Form番号
        //    procDataW.LoginUserId = procData.LoginUserId;   //IN:登録者ID
        //    procDataW.CtrlId = BusinessLogicIO.dllProcName_CheckAuthority;  //IN：起動処理名（CheckAuthority）
        //    procDataW.LanguageId = procData.LanguageId;     //IN:言語ID

        //    //IN:実行条件
        //    procDataW.ConditionData = procData.ButtonDefines;

        //    object results = null;  //Dictionary<string,object>
        //    CommonProcReturn returnInfo = businessLogicExec(procDataW, out results);
        //    if (returnInfo.IsProcEnd())
        //    {
        //        //エラーの場合、処理を中断
        //        return returnInfo;
        //    }

        //    if (results != null)
        //    {
        //        Dictionary<string, object> dicResults = (Dictionary<string, object>)results;
        //        if (dicResults.ContainsKey("Result"))
        //        {
        //            List<Dictionary<string, object>> rResult = dicResults["Result"] as List<Dictionary<string, object>>;
        //            foreach (var result in rResult)
        //            {
        //                //例）{ "ctrlId":"Back", "status":"1" },
        //                int status = -1;
        //                int.TryParse(result["status"].ToString(), out status);

        //                //↓↓ﾎﾞﾀﾝ権限情報＞KEY：ﾎﾞﾀﾝCTRLID、VALUE：ﾎﾞﾀﾝ状態
        //                authShoris.Add(result["ctrlId"].ToString(), status);
        //            }
        //        }
        //    }

        //    //処理結果
        //    return returnInfo;
        //}
        /* =========================================================================================================================== */
        /// <summary>
        /// 該当機能IDの処理権限区分ﾘｽﾄを取得する
        /// </summary>
        /// <param name="procData">ﾘｸｴｽﾄ情報</param>
        /// <param name="authShoris">ﾎﾞﾀﾝ権限情報ﾘｽﾄ</param>
        /// <remarks> - ﾎﾞﾀﾝ権限情報 KEYS</remarks>
        /// <remarks> - FORMNO  ：画面NO</remarks>
        /// <remarks> - CTRLID  ：ﾎﾞﾀﾝｺﾝﾄﾛｰﾙID</remarks>
        /// <remarks> - DISPKBN ：ﾎﾞﾀﾝ表示区分(USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF)＞Hide(0：非表示),Active(1：活性),Disabled(2：非活性)</remarks>
        /// <remarks> - AUTHKBN ：承認区分＞11(承認依頼),12(承認依頼取消),21(承認),22(承認取消),8(否認),-(承認系ﾎﾞﾀﾝ以外)</remarks>
        /// <returns>処理ステータス</returns>
        public CommonProcReturn GetUserAuthShoriInConduct(CommonProcData procData, out List<Dictionary<string, object>> authShoris)
        {
            authShoris = new List<Dictionary<string, object>>();

            //①業務ﾛｼﾞｯｸ機能単位のDLL - 権限チェック用処理
            //②ｴﾗｰの場合、処理を中断
            CommonProcData procDataW = new CommonProcData();
            procDataW.AuthorityLevelId = procData.AuthorityLevelId; //IN:権限レベルID
            procDataW.BelongingInfo = procData.BelongingInfo;   //IN:所属情報
            procDataW.ConductId = procData.ConductId;       //IN:機能ID
            procDataW.PgmId = procData.PgmId;               //IN:PGMID
            procDataW.FormNo = procData.FormNo;             //IN:Form番号
            procDataW.LoginUserId = procData.LoginUserId;   //IN:登録者ID
            procDataW.CtrlId = BusinessLogicIO.dllProcName_CheckAuthority;  //IN：起動処理名（CheckAuthority）
            procDataW.LanguageId = procData.LanguageId;     //IN:言語ID

            //IN:実行条件
            procDataW.ConditionData = procData.ButtonDefines;

            object results = null;  //Dictionary<string,object>
            CommonProcReturn returnInfo = businessLogicExec(procDataW, out results);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }

            if (results != null)
            {
                Dictionary<string, object> dicResults = (Dictionary<string, object>)results;
                if (dicResults.ContainsKey("Result"))
                {
                    authShoris = dicResults["Result"] as List<Dictionary<string, object>>;
                }
            }

            //処理結果
            return returnInfo;
        }
        /* ボタン権限制御 切替 end ==================================================================== */

        /// <summary>
        /// ﾘｸｴｽﾄﾊﾟﾗﾒｰﾀのﾃﾞｰﾀ検証を行う
        /// </summary>
        /// <param name="procData">ﾘｸｴｽﾄﾊﾟﾗﾒｰﾀ</param>
        /// <returns></returns>
        public CommonProcReturn ValidateRequestParameters(CommonProcData procData)
        {
            //=== 不正アクセス制御 ===

            // - ユーザー情報不正
            if (string.IsNullOrEmpty(procData.LoginUserId))
            {
                //メッセージ取得
                GetResourceName(procData, new List<string> { "941430005" }, out IDictionary<string, string> resources);
                //ログインしてください。
                string message = ConvertResourceName("941430005", resources);
                if (string.IsNullOrEmpty(procData.ConductId))
                {
                    // 機能IDが空の場合、ログインエラー
                    return new CommonProcReturn(CommonProcReturn.ProcStatus.LoginError.GetHashCode(), message);
                }
                else
                {
                    // 機能IDが空でない場合、タイムアウトエラー
                    return new CommonProcReturn(CommonProcReturn.ProcStatus.TimeOut.GetHashCode(), message);
                }
            }

            // - 機能ID：必須ﾁｪｯｸ
            if (String.IsNullOrEmpty(procData.ConductId))
            {
                //メッセージ取得
                GetResourceName(procData, new List<string> { "941340001" }, out IDictionary<string, string> resources);
                //メニューから選択してください。
                string message = ConvertResourceName("941340001", resources);
                return new CommonProcReturn(CommonProcReturn.ProcStatus.AccessError.GetHashCode(), message);
            }

            // - ﾕｰｻﾞｰ機能権限不正
            bool isAccept = false;

            // ※サイトTOP：ｱｸｾｽ許可
            string conductIdTop = AppCommonObject.Config.AppSettings.AppTopConductId;     //機能ID：サイトTOP
            if (conductIdTop.Equals(procData.ConductId))
            {
                isAccept = true;
            }
            else
            {
                // ※ﾕｰｻﾞｰ機能権限あり：ｱｸｾｽ許可
                isAccept = isAcceptConduct(procData.ConductId, procData.UserAuthConducts);
            }

            if (!isAccept)
            {
                // メニュー機能権限無しの場合、個別に機能権限チェックを実行する
                isAccept = checkUserConductAuthority(procData);
                if (!isAccept)
                {
                    //※ｱｸｾｽ不可
                    //メッセージ取得
                    GetResourceName(procData, new List<string> { "941070007" }, out IDictionary<string, string> resources);
                    //機能参照権限がありません。
                    string message = ConvertResourceName("941070007", resources);
                    return new CommonProcReturn(CommonProcReturn.ProcStatus.AccessError.GetHashCode(), message);
                }
            }

            return null;
        }

        /// <summary>
        /// 処理権限をﾁｪｯｸする
        /// </summary>
        /// <param name="ctrlId">処理ﾎﾞﾀﾝCTRLID</param>
        /// <param name="userAuthConductShori">
        /// KEY：ﾎﾞﾀﾝCTRLID,VALUE：0:非表示、1:活性（表示）、2:非活性（表示）
        /// </param>>
        /// <returns>true:権限あり、false:権限なし</returns>
        public bool IsAuthShori(string ctrlId, Dictionary<string, int> userAuthConductShori)
        {
            foreach (var item in userAuthConductShori)
            {
                if (ctrlId.Equals(item.Key))
                {
                    int status = item.Value;
                    if (1 == status)    //1:活性（表示）
                    {
                        //※権限あり
                        return true;
                    }
                    break;
                }
            }
            //※権限なし
            return false;
            //return true;
        }

        // ★ t-kudo del start
        ///// <summary>
        ///// SQLIDで管理されたSQL文のﾃﾞｰﾀを取得する
        ///// 　※DBﾊﾟｯｹｰｼﾞ：COM_SQL_KANRIで管理
        ///// </summary>
        ///// <param name="sqlId">SQLID</param>
        ///// <param name="param">ﾊﾟﾗﾒｰﾀ文字列</param>
        ///// <param name="results">結果データ(O)</param>
        ///// <returns>処理ｽﾃｰﾀｽ情報</returns>
        //public CommonProcReturn GetSqlKnrData(string sqlId, string param, out IList<CommonSqlKanriData> results)
        //{
        //    results = null;

        //    CommonDataUtil commonDataUtil = new CommonDataUtil();
        //    //★暫定対応
        //    //results = commonDataUtil.GetSqlKnrData(sqlId, param);
        //    results = new List<CommonSqlKanriData>();
        //    for (int i = 0; i < 5; i++)
        //    {
        //        CommonSqlKanriData data = new CommonSqlKanriData();
        //        data.VALUE1 = (i + 1).ToString();
        //        data.VALUE2 = "項目" + (i + 1).ToString();
        //        results.Add(data);
        //    }

        //    //処理ｽﾃｰﾀｽ：正常
        //    return new CommonProcReturn();
        //}
        // ★ t-kudo del end

        /// <summary>
        /// 画面表示に必要なﾃﾞｰﾀを取得
        ///  - 画面定義
        ///  - 条件ﾃﾞｰﾀ
        /// </summary>
        /// <param name="formId">Index or Edit</param>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　FormNo:画面NO(*)
        /// </param>
        /// <returns></returns>
        public CommonProcReturn GetViewData(CommonDataEntities context, FormId formId, CommonProcData procData, out List<CommonConductMst> results)
        {
            CommonProcReturn returnInfo = new CommonProcReturn();
            results = new List<CommonConductMst>();

            CommonDataUtil commonDataUtil = new CommonDataUtil(context);

            //先頭ﾃﾞｰﾀに画面定義ﾃﾞｰﾀをｾｯﾄ
            CommonConductMst resultView = null;
            switch (formId)
            {
                case FormId.Index:
                case FormId.Partial:
                    //一覧画面
                    returnInfo = GetViewDataIndex(ref commonDataUtil, procData, formId == FormId.Partial, out resultView);
                    break;
                //★del template2.0 廃止
                //case FormId.Edit:
                //    //明細入力画面
                //    returnInfo = GetViewDataEdit(ref commonDataUtil, procData, int.Parse(param1.ToString()), out resultView);
                //    break;
                //★del
                default:
                    //※ここにはこない
                    break;
            }
            if (resultView == null)
            {
                return returnInfo;
            }

            // 画面ごとに権限情報を取得して格納
            CommonProcData procDataW = CommonProcData.CopyKey(procData);
            if (!getAuthInfo(resultView, procDataW, ref returnInfo))
            {
                return returnInfo;
            }

            // 共通機能ごとに権限情報を取得して格納
            if (resultView.CM_CONDUCTMSTS != null)
            {
                foreach (var conductMst in resultView.CM_CONDUCTMSTS)
                {
                    procDataW = CommonProcData.CopyKey(procData);
                    procDataW.ConductId = conductMst.CONDUCTMST.CONDUCTID;
                    procDataW.PgmId = conductMst.CONDUCTMST.PGMID;

                    if (!getAuthInfo(conductMst, procDataW, ref returnInfo))
                    {
                        return returnInfo;
                    }
                }
            }

            //機能リストに追加
            results.Add(resultView);

            if (formId == FormId.Partial)
            {
                return returnInfo;
            }

            //以降、ﾒﾆｭｰ用ﾃﾞｰﾀをｾｯﾄ
            //※ﾒﾆｭｰ権限対応
            IList<CommonConductMst> resultsMenu = procData.UserAuthConducts;    //※ﾛｸﾞｲﾝ時にまとめて取得、ﾕｰｻﾞｰに応じた言語翻訳済み
            if (resultsMenu != null)
            {
                results.AddRange(resultsMenu);
            }

            return returnInfo;
        }

        /// <summary>
        /// ボタン権限情報取得
        /// </summary>
        /// <param name="conductMstW">機能マスタ情報</param>
        /// <param name="procDataW">業務ロジック用データ</param>
        /// <param name="returnInfo">処理結果情報</param>
        /// <returns></returns>
        private bool getAuthInfo(CommonConductMst conductMstW, CommonProcData procDataW, ref CommonProcReturn returnInfo)
        {
            /* ボタン権限制御 切替 start ==================================================================== */
            //conductMstW.UserAuthConductShoris = new Dictionary<short, Dictionary<string, int>>();

            //var formNos = conductMstW.FORMDEFINES.Select(y => y.FORMDEFINE.FORMNO).Distinct().ToList();
            //foreach (var curFormNo in formNos)
            //{
            //    procDataW.FormNo = curFormNo;
            //    //ﾎﾞﾀﾝ情報
            //    //例）{ "ctrlId":"Back",          "authKbn":"" }, 
            //    procDataW.ButtonDefines = new List<Dictionary<string, object>>();

            //    // コントロールグループの一覧項目定義を取得
            //    var ctrlGrpDefines = conductMstW.FORMDEFINES.Where(y =>
            //        y.FORMDEFINE.FORMNO == curFormNo &&
            //        y.FORMDEFINE.CTRLTYPE == FORM_DEFINE_CONSTANTS.CTRLTYPE.ControlGroup &&
            //        y.FORMDEFINE.DELFLG == false)
            //        .Select(y => y.LISTITEMDEFINES).ToList();
            //    foreach (var ctrlGrpDefine in ctrlGrpDefines)
            //    {
            //        // 一覧項目定義からボタンの項目定義を取得
            //        var buttonDefines = ctrlGrpDefine.Where(y =>
            //            y.CELLTYPE == LISTITEM_DEFINE_CONSTANTS.CELLTYPE.Button &&
            //            y.DELFLG == false).ToList();
            //        foreach (var btnDefine in buttonDefines)
            //        {
            //            procDataW.ButtonDefines.Add(
            //                new Dictionary<string, object>() {
            //                { "ctrlId", btnDefine.BTN_CTRLID},
            //                { "authKbn", btnDefine.BTN_AUTHCONTROLKBN},
            //                });
            //        }
            //    }

            //    //ﾕｰｻﾞｰのﾎﾞﾀﾝ処理権限を取得
            //    Dictionary<string, int> authShoris = new Dictionary<string, int>();
            //    if (procDataW.ButtonDefines.Count > 0)
            //    {
            //        //ﾎﾞﾀﾝ権限情報＞KEY：ﾎﾞﾀﾝCTRLID、VALUE：0(非表示),1(表示・活性),2(表示・非活性)
            //        CommonProcReturn returnInfoW = GetUserAuthShoriInConduct(procDataW, out authShoris);
            //        if (returnInfoW.IsProcEnd())
            //        {
            //            //※画面表示時に権限情報が取得できない場合は、画面表示されたらNG
            //            //　⇒例外エラーとする
            //            returnInfo.STATUS = CommonProcReturn.ProcStatus.InValid;
            //            return false;
            //        }
            //        MargeReturnInfo(ref returnInfo, returnInfoW);
            //    }

            //    conductMstW.UserAuthConductShoris.Add(curFormNo, authShoris);
            //}
            /* =============================================================================================== */
            conductMstW.UserAuthConductShoris = new List<Dictionary<string, object>>();

            //ﾕｰｻﾞｰのﾎﾞﾀﾝ処理権限を取得
            List<Dictionary<string, object>> authShoris = new List<Dictionary<string, object>>();
            //ﾎﾞﾀﾝ権限情報＞KEY：ﾎﾞﾀﾝCTRLID、VALUE：0(非表示),1(表示・活性),2(表示・非活性)
            CommonProcReturn returnInfoW = GetUserAuthShoriInConduct(procDataW, out authShoris);
            if (returnInfoW.IsProcEnd())
            {
                //※画面表示時に権限情報が取得できない場合は、画面表示されたらNG
                //　⇒例外エラーとする
                returnInfo.STATUS = CommonProcReturn.ProcStatus.InValid;
                return false;
            }
            MargeReturnInfo(ref returnInfo, returnInfoW);
            procDataW.ButtonDefines = authShoris;
            conductMstW.UserAuthConductShoris = authShoris;
            /* ボタン権限制御 切替 end ==================================================================== */
            return true;
        }

        /// <summary>
        /// 画面の翻訳情報ﾘｽﾄを取得する
        /// </summary>
        /// <param name="procData">ﾘｸｴｽﾄ情報</param>
        /// <param name="resourceIds">翻訳Idﾘｽﾄ</param>
        /// <param name="resources">ﾘｿｰｽ翻訳情報＞KEY：翻訳Id、VALUE：翻訳名</param>
        /// <returns>処理ステータス</returns>
        public CommonProcReturn GetResourceName(CommonProcData procData, IList<string> resourceIds, out IDictionary<string, string> resources)
        {
            resources = null;

            //①業務ﾛｼﾞｯｸ機能単位のDLL - 権限チェック用処理
            //②ｴﾗｰの場合、処理を中断
            CommonProcData procDataW = new CommonProcData();
            procDataW.TerminalNo = procData.TerminalNo;     //IN:端末ID
            procDataW.AuthorityLevelId = procData.AuthorityLevelId; //IN:権限レベルID
            procDataW.BelongingInfo = procData.BelongingInfo;   //IN:所属情報
            procDataW.LoginUserId = procData.LoginUserId;   //IN:登録者ID
            procDataW.LanguageId = procData.LanguageId;     //IN:言語ID
            procDataW.ConductId = procData.ConductId;       //IN:機能ID
            procDataW.FormNo = procData.FormNo;             //IN:Form番号

            // - 業務ロジックコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetResourcesValue
            BusinessLogicIO logicIO = new BusinessLogicIO(procDataW);
            Dictionary<string, string> resourceNames = null;  //Dictionary<string,string >
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_GetResourcesValue(resourceIds, out resourceNames);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }

            resources = resourceNames;

            //処理結果
            return returnInfo;
        }
        /// <summary>
        /// 翻訳IDを翻訳名に置き換えて返却
        /// </summary>
        /// <param name="resourseId">翻訳ID</param>
        /// <param name="resources">ﾘｿｰｽ翻訳情報＞KEY：翻訳Id、VALUE：翻訳名</param>
        /// <returns>翻訳名</returns>
        /// <remarks>※翻訳IDが翻訳情報に含まれない場合は、翻訳IDの文字列のまま返却します</remarks>
        public string ConvertResourceName(string resourseId, IDictionary<string, string> resources)
        {
            string resourseName = resourseId;   //ﾃﾞﾌｫﾙﾄ：resourseId

            //翻訳情報に含まれる場合、翻訳名に変換
            if (!string.IsNullOrEmpty(resourseId) && resources.Keys.Contains(resourseId))
            {
                resourseName = resources[resourseId];
            }

            return resourseName;
        }

        /// <summary>
        /// 翻訳IDを翻訳名に置き換えて返却
        /// </summary>
        /// <param name="resourseIds">翻訳ID配列(先頭がフォーマット用ID、その他はパラメータ用ID</param>
        /// <param name="resources">ﾘｿｰｽ翻訳情報＞KEY：翻訳Id、VALUE：翻訳名</param>
        /// <returns>翻訳名</returns>
        /// <remarks>※翻訳IDが翻訳情報に含まれない場合は、翻訳IDの文字列のまま返却します</remarks>
        public string ConvertFormattedResourceName(string[] resourseIds, IDictionary<string, string> resources)
        {
            if (resourseIds == null || resourseIds.Length == 0) { return string.Empty; }

            List<string> idList = new List<string>();
            foreach (var id in resourseIds)
            {
                //翻訳情報に含まれる場合、翻訳名に変換
                if (!string.IsNullOrEmpty(id) && resources.Keys.Contains(id))
                {
                    idList.Add(resources[id]);
                }
                else
                {
                    idList.Add(id);
                }
            }
            // フォーマット変換して返却
            return idList.Count > 1 ? string.Format(idList[0], idList.GetRange(1, idList.Count - 1)) : idList[0];
        }

        /// <summary>
        /// 画面表示に必要なﾃﾞｰﾀを取得
        ///  - 画面定義
        ///  - 条件ﾃﾞｰﾀ
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　FormNo:画面NO(*)
        /// </param>
        /// <param name="viewType">Index or Edit</param>
        /// <param name="rowNo">行番号(viewType = Editの場合のみ)</param>
        /// <returns></returns>
        public CommonProcReturn GetViewDataIndex(ref CommonDataUtil commonDataUtil,
            CommonProcData procData, bool isPartial, out CommonConductMst result)
        {
            CommonProcReturn returnInfo = new CommonProcReturn();
            result = null;

            //⓪リソース翻訳を取得
            IDictionary<string, string> resources = null;
            returnInfo = GetResourceName(procData, null, out resources);
            if (returnInfo.IsProcEnd())
            {
                //※画面表示時に権限情報が取得できない場合は、画面表示されたらNG
                //　⇒例外エラーとする
                returnInfo.STATUS = CommonProcReturn.ProcStatus.InValid;
                return returnInfo;
            }

            //①画面定義を取得
            //※子画面一覧からの戻りの場合、親画面の画面NOに差し替え
            short areaKbn = FORM_DEFINE_CONSTANTS.AREAKBN.None;      //定義区分(0:条件 1:明細 2:単票)
            short formNo = procData.FormNo;
            result = commonDataUtil.GetFormInfo(procData, ref formNo, areaKbn);
            if (result == null)
            {
                //機能ﾏｽﾀに存在しない場合
                //「機能IDがマスタに存在しません。」
                string msg = ConvertFormattedResourceName(new string[] { "941060004", "911070002" }, resources);
                return new CommonProcReturn(CommonProcReturn.ProcStatus.InValid, msg);
            }
            procData.PgmId = result.CONDUCTMST.PGMID;
            procData.FormNo = formNo;

            // 翻訳辞書を設定
            result.TransDictionary = (Dictionary<string, string>)resources;

            if (result.CONDUCTMST.PTN == CONDUCT_MST_CONSTANTS.PTN.Bat)
            {
                //バッチ処理の場合
                //※実行ボタン用の画面定義をデフォルト追加
                //コントロールID：「ComBatExec」(固定)
                COM_FORM_DEFINE define = createFormDefineInstance(result.CONDUCTMST.PGMID, 0, COM_CTRL_CONSTANTS.CTRLID.ComBatExec);
                define.CTRLNAME = "バッチ実行";
                define.AREAKBN = FORM_DEFINE_CONSTANTS.AREAKBN.Condition;   // 条件エリア
                define.CTRLTYPE = FORM_DEFINE_CONSTANTS.CTRLTYPE.ControlGroup;
                define.CTR_POSITIONKBN = FORM_DEFINE_CONSTANTS.CTR_POSITIONKBN.Lower;

                CommonFormDefine defineW = new CommonFormDefine(define);
                defineW.LISTITEMDEFINES = new List<COM_LISTITEM_DEFINE>();

                COM_LISTITEM_DEFINE btnDefine = createListItemDefineInstance(define.PGMID, define.FORMNO, define.CTRLID);
                btnDefine.DEFINETYPE = LISTITEM_DEFINE_CONSTANTS.DEFINETYPE.DataRow;    // 1固定
                btnDefine.ITEMNO = 1;
                btnDefine.ITEMNAME = "911120001"; // 翻訳ID：実行
                btnDefine.DISPKBN = LISTITEM_DEFINE_CONSTANTS.DISPKBN.Both;
                btnDefine.CELLTYPE = LISTITEM_DEFINE_CONSTANTS.CELLTYPE.Button;
                btnDefine.BTN_CTRLID = COM_CTRL_CONSTANTS.CTRLID.ComBatExec;
                btnDefine.BTN_ACTIONKBN = LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComBatExec;    // バッチ実行
                btnDefine.BTN_AUTHCONTROLKBN = LISTITEM_DEFINE_CONSTANTS.BTN_AUTHCONTROLKBN.Control;    //権限制御あり

                defineW.LISTITEMDEFINES.Add(btnDefine);
                result.FORMDEFINES.Add(defineW);

                //※処理結果一覧用の画面定義をデフォルト追加
                //コントロールID：「ComBatStatus」(固定)
                define = createFormDefineInstance(result.CONDUCTMST.PGMID, 0, COM_CTRL_CONSTANTS.CTRLID.ComBatStatus);
                define.CTRLNAME = "処理ステータス";
                define.CTRLTYPE = FORM_DEFINE_CONSTANTS.CTRLTYPE.BatStatus;   // バッチステータス用定型一覧
                define.AREAKBN = FORM_DEFINE_CONSTANTS.AREAKBN.List;    // 明細エリア

                defineW = new CommonFormDefine(define);
                //result.FORMDEFINES.Add(defineW);
                defineW.CTR_FORMDEFINES = new List<CommonFormDefine>();

                //※再表示ボタン用の画面定義をデフォルト追加
                //コントロールID：「ComBatRefresh」(固定)
                define = createFormDefineInstance(result.CONDUCTMST.PGMID, 0, COM_CTRL_CONSTANTS.CTRLID.ComBatRefresh);
                define.CTRLNAME = "バッチ再表示";
                define.AREAKBN = FORM_DEFINE_CONSTANTS.AREAKBN.List;    // 明細エリア
                define.CTRLTYPE = FORM_DEFINE_CONSTANTS.CTRLTYPE.ControlGroup;   // コントロールグループ
                define.CTR_RELATIONCTRLID = COM_CTRL_CONSTANTS.CTRLID.ComBatStatus;     // 処理ステータス一覧に紐づけ
                define.CTR_POSITIONKBN = FORM_DEFINE_CONSTANTS.CTR_POSITIONKBN.Lower;   // 処理ステータス一覧下部に配置

                CommonFormDefine defineW2 = new CommonFormDefine(define);
                defineW2.LISTITEMDEFINES = new List<COM_LISTITEM_DEFINE>();

                btnDefine = createListItemDefineInstance(define.PGMID, define.FORMNO, define.CTRLID);
                btnDefine.DEFINETYPE = LISTITEM_DEFINE_CONSTANTS.DEFINETYPE.DataRow;    // 1固定
                btnDefine.ITEMNO = 1;
                btnDefine.ITEMNAME = "111110015"; // 翻訳ID：再表示
                btnDefine.DISPKBN = LISTITEM_DEFINE_CONSTANTS.DISPKBN.Both;
                btnDefine.CELLTYPE = LISTITEM_DEFINE_CONSTANTS.CELLTYPE.Button;
                btnDefine.BTN_CTRLID = COM_CTRL_CONSTANTS.CTRLID.ComBatRefresh;
                btnDefine.BTN_ACTIONKBN = LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComBatRefresh;    // バッチ再表示

                defineW2.LISTITEMDEFINES.Add(btnDefine);
                defineW.CTR_FORMDEFINES.Add(defineW2);
                result.FORMDEFINES.Add(defineW);
            }

            //②翻訳を取得して画面定義情報に反映
            convertDefineResourceName(result, resources);

            //共通機能の画面定義情報に翻訳を反映
            if (result.CM_CONDUCTMSTS != null && result.CM_CONDUCTMSTS.Count > 0)
            {
                foreach (var comDefine in result.CM_CONDUCTMSTS)
                {
                    convertDefineResourceName(comDefine, resources);
                }
            }

            ////場所階層リストを取得
            //List<CommonStructure> structureList = null;
            //procData.StructureGroupList = new List<int>() { STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Location };
            //returnInfo = GetStructureList(procData, ref structureList, resources);

            ////職種機種リストを取得（階層は場所階層の選択値により作成するため、共通階層のみ取得）
            //List<CommonStructure> jobStructureList = null;
            //procData.StructureGroupList = new List<int>() { STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Job };
            //procData.FactoryIdList = new List<int>() { 0 };
            //returnInfo = GetStructureList(procData, ref jobStructureList, resources);

            //structureList.AddRange(jobStructureList);
            //result.StructureList = structureList;

            if (isPartial)
            {
                // 部分ビュー取得時はコンボデータ取得は不要
                return returnInfo;
            }

            // 1ページ当たりの行数コンボ取得
            GetComboRowsPerPage(procData, out List<int> rowsPerPage);
            result.PageRowsList = rowsPerPage;

            // 言語コンボ取得
            GetLanguageItemList(procData, out List<LanguageInfo> languageItemList);
            result.LanguageComboList = languageItemList;

            return returnInfo;
        }

        /// <summary>
        /// 機能情報、画面定義情報に翻訳を反映
        /// </summary>
        /// <param name="result">画面定義</param>
        /// <param name="resources">翻訳</param>
        private void convertDefineResourceName(CommonConductMst result, IDictionary<string, string> resources)
        {
            if (result.FORMDEFINES != null && result.FORMDEFINES.Count > 0)
            {
                //※画面項目定義あり

                //画面定義情報に翻訳を反映
                foreach (var define in result.FORMDEFINES)
                {
                    convertResourceNames(define, resources);
                }

            }
            //画面タイトル
            result.CONDUCTMST.NAME = ConvertResourceName(result.CONDUCTMST.NAME, resources);
            result.CONDUCTMST.RYAKU = ConvertResourceName(result.CONDUCTMST.RYAKU, resources);
        }

        /// <summary>
        /// 画面定義情報に翻訳を反映
        /// </summary>
        /// <param name="define">画面定義</param>
        /// <param name="resources">翻訳</param>
        private void convertResourceNames(CommonFormDefine define, IDictionary<string, string> resources)
        {
            //・画面定義情報に翻訳を反映
            //// + 画面項目定義ﾏｽﾀ - コントロール名
            define.FORMDEFINE.CTRLNAME = ConvertResourceName(define.FORMDEFINE.CTRLNAME, resources);
            // + 画面項目定義ﾏｽﾀ - 画面タイトル
            define.FORMDEFINE.DAT_FORMTITLE = ConvertResourceName(define.FORMDEFINE.DAT_FORMTITLE, resources);
            // + 画面項目定義ﾏｽﾀ - タブ名
            define.FORMDEFINE.TABNAME = ConvertResourceName(define.FORMDEFINE.TABNAME, resources);
            // + 画面項目定義ﾏｽﾀ - グループ名
            define.FORMDEFINE.CTRLGRPNAME = ConvertResourceName(define.FORMDEFINE.CTRLGRPNAME, resources);
            // + 画面項目定義ﾏｽﾀ - 一覧情報名
            define.FORMDEFINE.DAT_TITLE = ConvertResourceName(define.FORMDEFINE.DAT_TITLE, resources);
            // + 画面項目定義ﾏｽﾀ - ﾂｰﾙﾁｯﾌﾟ文言
            define.FORMDEFINE.TOOLTIP = ConvertResourceName(define.FORMDEFINE.TOOLTIP, resources);

            if (define.LISTITEMDEFINES != null && define.LISTITEMDEFINES.Count > 0)
            {
                foreach (var item in define.LISTITEMDEFINES)
                {
                    // ++ 一覧項目定義ﾏｽﾀ - 項目名
                    item.ITEMNAME = ConvertResourceName(item.ITEMNAME, resources);
                    // ++ 一覧項目定義ﾏｽﾀ - セルタイプがボタンの場合
                    if (item.CELLTYPE.Equals(LISTITEM_DEFINE_CONSTANTS.CELLTYPE.Button))
                    {
                        // ++ 一覧項目定義ﾏｽﾀ - 初期値(セルタイプがボタンの場合、ボタン文言)
                        item.INITVAL = ConvertResourceName(item.INITVAL, resources);
                    }
                    // ++ 一覧項目定義ﾏｽﾀ - 初期表示ｶﾞｲﾄﾞ
                    item.TXT_PLACEHOLDER = ConvertResourceName(item.TXT_PLACEHOLDER, resources);
                    // ++ 一覧項目定義ﾏｽﾀ - ﾂｰﾙﾁｯﾌﾟ文言
                    item.TOOLTIP = ConvertResourceName(item.TOOLTIP, resources);
                    // ++ 一覧項目定義ﾏｽﾀ - 表示書式
                    item.FORMAT = ConvertResourceName(item.FORMAT, resources);
                }
            }

            if (define.CTR_FORMDEFINES != null && define.CTR_FORMDEFINES.Count > 0)
            {
                foreach (var ctrDefine in define.CTR_FORMDEFINES)
                {
                    convertResourceNames(ctrDefine, resources);
                }
            }
        }

        ///// <summary>
        ///// 場所構成リストに翻訳を反映
        ///// </summary>
        ///// <param name="define">場所構成リスト</param>
        ///// <param name="resources">翻訳</param>
        //private void convertResourceStructure(List<CommonStructure> structureList, IDictionary<string, string> resources)
        //{
        //    if (structureList != null && structureList.Count > 0)
        //    {
        //        foreach (var item in structureList)
        //        {
        //            item.ItemTranslationStr = ConvertResourceName(item.ItemTranslationId.ToString(), resources);
        //            item.TitleTranslationStr = ConvertResourceName(item.TitleTranslationId.ToString(), resources);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 画面表示に必要なﾃﾞｰﾀを取得(明細入力画面用)
        /////  - 画面定義
        /////  - 条件ﾃﾞｰﾀ
        ///// </summary>
        ///// <param name="procData">
        ///// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        ///// 　ConductId:機能ID(*)
        ///// 　FormNo:画面NO(*)
        ///// 　CtrlId:
        ///// 　  rowNo=0(新規ﾎﾞﾀﾝ押下時)、新規ﾎﾞﾀﾝCtrlId
        ///// 　  rowNo>0(NOﾘﾝｸ押下時)、一覧CtrlId
        ///// </param>
        ///// <param name="viewType">Index or Edit</param>
        ///// <param name="rowNo">行番号(viewType = Editの場合のみ)</param>
        ///// <returns></returns>
        //public CommonProcReturn GetViewDataEdit(ref CommonDataUtil commonDataUtil, 
        //    CommonProcData procData, int rowNo, out CommonConductMst result)

        //{
        //    CommonProcReturn returnInfo = new CommonProcReturn();
        //    result = null;

        //    //画面表示用明細ﾃﾞｰﾀ
        //    // - 行番号：<= 0 の場合、DATATYPE=2の初期値ﾃﾞｰﾀ
        //    // - 行番号：> 0 の場合、DATATYPE=0の値ﾃﾞｰﾀ
        //    object datas = null;

        //    if (rowNo <= 0)
        //    {
        //        //①新規ﾃﾞｰﾀ作成用業務ﾛｼﾞｯｸﾌﾟﾛｼｰｼﾞｬｺｰﾙ
        //        //  (例)PF00030.New0
        //        //②ｴﾗｰの場合、処理を中断
        //        returnInfo = businessLogicExec(procData, out datas);    //画面データの保存なし
        //        if (returnInfo.IsProcEnd())
        //        {
        //            //エラーの場合、処理を中断
        //            return returnInfo;
        //        }

        //        //処理引数：CtrlIdを初期化⇒一覧CtrlIdで上書き
        //        // AP2.0 Mod start
        //        //procData.CtrlId = "";
        //        // AP2.0 Mod end

        //        ////新規ﾃﾞｰﾀ行を取得
        //        // datas = commonDataUtil.GetTmpTblData(
        //        //    procData, false, TMPTBL_CONSTANTS.DATATYPE.New, 0, false, true).ToList();   //DATATYPE=2(新規行ﾃﾞｰﾀ)
        //        if (datas != null)
        //        {
        //            foreach (COM_TMPTBL_DATA data in datas as IList<COM_TMPTBL_DATA>)
        //            {
        //                data.ROWNO = 0;
        //                //処理引数：ﾎﾞﾀﾝCtrlIdを一覧CtrlIdで上書き
        //                procData.CtrlId = data.CTRLID;
        //            }

        //        }
        //        else
        //        {
        //            // AP2.0 Mod start
        //            procData.CtrlId = "";
        //            // AP2.0 Mod end
        //            datas = new List<COM_TMPTBL_DATA>();
        //        }

        //    }

        //    //画面定義を取得(入力画面)
        //    short areaKbn = FORM_DEFINE_CONSTANTS.AREAKBN.Detail;     //定義区分(1:一覧 2:入力)
        //    short formNo = procData.FormNo;
        //    result = commonDataUtil.GetFormInfo(procData, ref formNo, areaKbn, procData.CtrlId);
        //    if (result == null)
        //    {
        //        //機能ﾏｽﾀに存在しない場合
        //        return new CommonProcReturn(CommonProcReturn.ProcStatus.InValid, "機能マスタに存在しません。");
        //    }

        //    return returnInfo;

        //}

        ///// <summary>
        ///// 再検索処理
        ///// </summary>
        ///// <param name="procData">
        ///// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        ///// 　ConductId:機能ID(*)
        ///// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        ///// 　FormNo:画面NO(*)
        ///// 　PageNo:ﾍﾟｰｼﾞ番号
        ///// 　 ※未設定の場合、先頭ﾍﾟｰｼﾞを取得
        ///// 　PageCount:1ﾍﾟｰｼﾞ表示行数
        ///// 　 ※未設定の場合、全件を取得
        ///// </param>
        ///// <param name="results">結果データ(O)</param>
        ///// <returns>処理ｽﾃｰﾀｽ情報</returns>
        ///// <remarks>
        ///// 　共通使用区分=0:検索のｺﾝﾄﾛｰﾙIDのﾌﾟﾛｼｰｼﾞｬをｺｰﾙ
        ///// 　存在しない場合は明細ﾃﾞｰﾀ取得のみ（検索ﾌﾟﾛｼｰｼﾞｬをｺｰﾙ）しない
        ///// </remarks>
        //public void ReSearchProcess(ref CommonDataUtil commonDataUtil,
        //    CommonProcData procData,
        //    int rowNo,
        //    int backFlg,
        //    ref CommonProcReturn returnInfo,
        //    out IList<COM_TMPTBL_DATA> retResults,
        //    int pageNo = 1)
        //{

        //    if (rowNo >= 0)
        //    {
        //        //2:明細入力画面 - 実行時再検索の場合
        //        //明細ﾃﾞｰﾀを取得
        //        retResults = commonDataUtil.GetTmpTblData(
        //            procData, true, TMPTBL_CONSTANTS.DATATYPE.Input, rowNo, false, true).ToList();    //1:入力値
        //    }
        //    else
        //    {
        //        //1:一覧画面 - 実行時再検索の場合

        //        if (backFlg == 4 || backFlg == 5)
        //        {
        //            //backFlg == 4
        //            // 【共通 - マスタ機能】ファイル取込時
        //            // 【共通 - マスタ機能】登録時
        //            //backFlg == 5
        //            // 画面遷移時、ｴﾗｰ発生時等画面遷移しない場合

        //            //⑦結果用中間ﾃﾞｰﾀ取得
        //            // - ﾃﾞｰﾀ取得
        //            //※先頭ﾍﾟｰｼﾞを取得
        //            //※画面上のすべてのｺﾝﾄﾛｰﾙIDの明細一覧ﾃﾞｰﾀを取得
        //            //※明細一覧(CtrlId)ごとに先頭ﾃﾞｰﾀに全ﾃﾞｰﾀ件数をｾｯﾄ
        //            retResults = commonDataUtil.GetTmpTblDataforSearch(procData,
        //                TMPTBL_CONSTANTS.DATATYPE.Input);  //1:入力値
        //        }
        //        else if (string.IsNullOrEmpty(procData.CtrlId))
        //        {
        //            //検索ﾎﾞﾀﾝなしの場合、明細ﾃﾞｰﾀ初期値を取得
        //            //⑦結果用中間ﾃﾞｰﾀ取得
        //            // - ﾃﾞｰﾀ取得
        //            //※先頭ﾍﾟｰｼﾞを取得
        //            //※画面上のすべてのｺﾝﾄﾛｰﾙIDの明細一覧ﾃﾞｰﾀを取得
        //            //※明細一覧(CtrlId)ごとに先頭ﾃﾞｰﾀに全ﾃﾞｰﾀ件数をｾｯﾄ
        //            retResults = commonDataUtil.GetTmpTblDataforSearch(procData,
        //                TMPTBL_CONSTANTS.DATATYPE.Result, pageNo);  //0:結果ﾃﾞｰﾀ
        //        }
        //        else
        //        {
        //            //再検索（※画面の条件データ保存なし）
        //            bool isSearch = true;
        //            if (backFlg != 0)
        //            {
        //                isSearch = false;
        //            }
        //            CommonProcReturn returnInfoW = SearchProcess(procData, out retResults, isSearch);

        //            //処理結果をマージ
        //            MargeReturnInfo(ref returnInfo, returnInfoW);
        //        }
        //    }

        //    if (retResults == null)
        //    {
        //        //※念のため、空のリストを返却
        //        retResults = new List<COM_TMPTBL_DATA>();
        //    }

        //}

        /// <summary>
        /// 業務ﾛｼﾞｯｸの画面初期化処理
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// </param>
        /// <param name="retResults">取得ﾃﾞｰﾀ</param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn InitProcess(CommonProcData procData, out object retResults)
        {
            //※処理状態に応じて、クライアント側から下記CTRLIDで呼び出される
            //画面初期化処理の場合、CTRLID：「Init」
            //戻り時、画面再表示処理の場合、CTRLID：「InitBack」

            //①画面初期化処理用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
            //  (例)画面初期化処理の場合、KG00040.Init0
            //  (例)戻り時、画面再表示処理の場合、KG00040.InitBack0
            //②ｴﾗｰの場合、処理を中断
            CommonProcReturn returnInfo = businessLogicExec(procData, out retResults);    //画面データの保存なし

            //処理ｽﾃｰﾀｽ：正常
            return returnInfo;
        }

        /// <summary>
        /// 処理区分に応じた共通処理
        /// 　-2:一覧ソート
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID(*)
        /// 　　※ﾃﾞｰﾀ取得を行う一覧のｺﾝﾄﾛｰﾙID
        /// 　PageNo:ﾍﾟｰｼﾞ番号(*)
        /// 　　※未設定の場合、先頭ﾍﾟｰｼﾞを取得)
        /// 　PageCount:1ﾍﾟｰｼﾞ表示行数(*)
        /// </param>
        /// <param name="results">結果データ(O)</param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn ListSortProcess(CommonProcData procData, out object results)

        {
            CommonProcReturn returnInfo = new CommonProcReturn();
            results = null;

            //⑤ﾍﾟｰｼﾞﾃﾞｰﾀ取得用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
            //  (例)KG00040.Search0
            //⑥ｴﾗｰの場合、処理を中断
            returnInfo = businessLogicExec(procData, out results);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }

            //処理ｽﾃｰﾀｽ：正常
            return returnInfo;
        }

        /// <summary>
        /// 処理区分に応じた共通処理
        /// 　-1:データ取得
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID(*)
        /// 　　※ﾃﾞｰﾀ取得を行う一覧のｺﾝﾄﾛｰﾙID
        /// 　PageNo:ﾍﾟｰｼﾞ番号(*)
        /// 　　※未設定の場合、先頭ﾍﾟｰｼﾞを取得)
        /// 　PageCount:1ﾍﾟｰｼﾞ表示行数(*)
        /// </param>
        /// <param name="results">結果データ(O)</param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn GetDataProcess(CommonProcData procData, out object results)

        {
            CommonProcReturn returnInfo = new CommonProcReturn();
            results = null;

            //⑤ﾍﾟｰｼﾞﾃﾞｰﾀ取得用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
            //  (例)KG00040.Search0
            //⑥ｴﾗｰの場合、処理を中断
            returnInfo = businessLogicExec(procData, out results);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }

            //処理ｽﾃｰﾀｽ：正常
            return returnInfo;
        }

        /// <summary>
        /// 処理区分に応じた共通処理
        /// 　0:検索
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID(*)
        /// 　　※ｲﾍﾞﾝﾄﾎﾞﾀﾝのｺﾝﾄﾛｰﾙID
        /// 　ConditionData(val1～100):条件ﾃﾞｰﾀ(*)
        /// 　PageNo:ﾍﾟｰｼﾞ番号
        /// 　 ※未設定の場合、先頭ﾍﾟｰｼﾞを取得
        /// 　PageCount:1ﾍﾟｰｼﾞ表示行数
        /// 　 ※未設定の場合、全件を取得
        /// </param>
        /// <param name="results">結果データ(O)</param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn SearchProcess(CommonProcData procData, out object retResults)
        {
            CommonProcReturn returnInfo = new CommonProcReturn();
            retResults = null;

            //②画面.条件を共通＿条件用中間テーブルに登録
            //③条件ﾁｪｯｸ用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
            //  (例)KG00040.SearchCheck0
            //④ｴﾗｰの場合、処理を中断
            //⑤結果ﾃﾞｰﾀ取得用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
            //  (例)KG00040.Search0
            //⑥ｴﾗｰの場合、処理を中断
            returnInfo = businessLogicExec(procData, out retResults);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }

            //処理ｽﾃｰﾀｽ：正常
            return returnInfo;
        }

        /// <summary>
        /// 処理区分に応じた共通処理
        /// 　1:実行
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID(*)
        /// 　　※ｲﾍﾞﾝﾄﾎﾞﾀﾝのｺﾝﾄﾛｰﾙID
        /// 　ListData(val1～200):明細ﾃﾞｰﾀ(*)
        /// </param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn ExecuteProcess(CommonProcData procData, out object retResults)
        {
            //②画面.入力内容を共通＿結果用中間テーブルに登録
            //③入力ﾁｪｯｸ用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
            //  (例)KG00040.RegistCheck0
            //④ｴﾗｰの場合、処理を中断
            //⑤実行用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
            //  (例)KG00040.Regist0
            //⑥ｴﾗｰの場合、処理を中断
            CommonProcReturn returnInfo = businessLogicExec(procData, out retResults);
            if (returnInfo.IsProcEnd())
            {
                return returnInfo;
            }

            return returnInfo;
        }

        /// <summary>
        /// 処理区分に応じた共通処理
        /// 　5:Excel出力
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID(*)
        /// 　　※ｲﾍﾞﾝﾄﾎﾞﾀﾝのｺﾝﾄﾛｰﾙID
        /// 　ConditionData(val1～100):条件ﾃﾞｰﾀ(*)
        /// </param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn ReportProcess(CommonProcData procData, out object retResults)
        {
            MemoryStream stream = null;
            try
            {
                retResults = null;

                //処理結果を初期化
                object logicResult = null;

                //②画面.条件を共通＿条件用中間テーブルに登録
                //③出力条件ﾁｪｯｸ用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
                //  (例)KG00040.ReportCheck0
                //④ｴﾗｰの場合、処理を中断
                //⑤出力ﾃﾞｰﾀ取得用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
                //  (例)KG00040.Report0
                //⑥ｴﾗｰの場合、処理を中断
                CommonProcReturn returnInfo = businessLogicExec(procData, out logicResult);
                if (returnInfo.IsProcEnd())
                {
                    retResults = logicResult;
                    return returnInfo;
                }

                Dictionary<string, object> resultsW = logicResult as Dictionary<string, object>;
                if (resultsW.ContainsKey("OutputStream") && resultsW["OutputStream"] != null)
                {
                    // OutputStreamが設定されている場合のみ出力処理を行う
                    stream = resultsW["OutputStream"] as MemoryStream;
                    var fileType = resultsW["fileType"].ToString();
                    var fileName = resultsW["fileName"].ToString();

                    //⑦以降、ファイル出力処理
                    //※4：出力②～の処理に同じ

                    // ファイル保存処理
                    // - ﾌｧｲﾙを一時ﾌｫﾙﾀﾞに保存
                    //作成ファイル情報設定
                    FileUtil.Extension ext = FileUtil.GetFileExtFromFileType(fileType);      //ファイル拡張子
                    CommonProcReturn returnInfoW = setCreateFileInfo(procData, stream.ToArray(), fileName, ext);
                    if (returnInfoW.IsProcEnd())
                    {
                        return returnInfoW;
                    }

                    //処理結果をマージ
                    MargeReturnInfo(ref returnInfo, returnInfoW);
                }


                //処理ｽﾃｰﾀｽ：正常
                // - 処理完了ﾒｯｾｰｼﾞを設定
                if (returnInfo.STATUS == CommonProcReturn.ProcStatus.Valid &&
                    string.IsNullOrEmpty(returnInfo.MESSAGE))
                {
                    //メッセージ取得
                    GetResourceName(procData, new List<string> { "941120012" }, out IDictionary<string, string> resources);
                    //処理が完了しました。
                    returnInfo.MESSAGE = ConvertResourceName("941120012", resources);
                }

                Dictionary<string, object> results = new Dictionary<string, object>();
                if (resultsW.ContainsKey("Result"))
                {
                    results.Add("Result", resultsW["Result"]);
                }
                if (resultsW.ContainsKey("Individual"))
                {
                    results.Add("Individual", resultsW["Individual"]);
                }
                if (resultsW.ContainsKey("ButtonStatus"))
                {
                    results.Add("ButtonStatus", resultsW["ButtonStatus"]);
                }
                if (results.Count > 0)
                {
                    retResults = results;
                }

                return returnInfo;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// ファイルダウンロード ランダムなキーを取得し、ファイル名とファイルパスをセッションにセット。戻り値はキー値に変更する。
        /// </summary>
        /// <param name="session">変更するセッション</param>
        /// <param name="returnInfo">画面戻り値、ファイルパスとファイル名をキーに変更</param>
        /// <param name="userId">ユーザID キーに使用</param>
        public void SetSessionFileInfo(ref ISession session, ref CommonProcReturn returnInfo, string userId)
        {
            if (string.IsNullOrEmpty(returnInfo.FILEDOWNLOADNAME) || string.IsNullOrEmpty(returnInfo.FILEPATH))
            {
                // ファイルが無いときは処理を行わない
                return;
            }

            // セッションのキー キー重複判定に使用
            List<string> sessionKeys = session.Keys.ToList();

            // ファイル名
            string sessionKey = getOriginKey(sessionKeys);
            session.SetString(sessionKey, returnInfo.FILEDOWNLOADNAME);
            returnInfo.FILEDOWNLOADNAME = sessionKey;
            sessionKeys.Add(sessionKey);
            // ファイルパス
            sessionKey = getOriginKey(sessionKeys);
            session.SetString(sessionKey, returnInfo.FILEPATH);
            returnInfo.FILEPATH = sessionKey;

            // キーを取得し、重複していなければそのキーを使用する
            string getOriginKey(List<string> sessionKeys)
            {
                while (true)
                {
                    string keyValue = getKeyValue();
                    if (!sessionKeys.Contains(keyValue))
                    {
                        return keyValue;
                    }
                }
            }
            // キーをランダムに取得する
            string getKeyValue()
            {
                Random random = new(DateTime.Now.Second); // ランダム生成元(Seedを変えるために現在の秒)
                string numValue = random.Next(1, 99999999 + 1).ToString(); // 8桁
                return userId + "_" + numValue;
            }
        }

        /// <summary>
        /// 処理区分に応じた共通処理
        /// 　5:Excel出力
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID(*)
        /// 　　※ｲﾍﾞﾝﾄﾎﾞﾀﾝのｺﾝﾄﾛｰﾙID
        /// 　ConditionData(val1～100):条件ﾃﾞｰﾀ(*)
        /// </param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn ReportProcessExcelOut(CommonProcData procData)
        {

            CommonProcReturn returnInfo = new CommonProcReturn();

            //⑤Excel出力ﾌｧｲﾙ作成用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
            //  (例)KG00040.Search0
            //⑥ｴﾗｰの場合、処理を中断
            object results = null;
            returnInfo = businessLogicExec(procData, out results);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }
            Dictionary<string, object> results2 = results as Dictionary<string, object>;
            MemoryStream result = results2["OutputStream"] as MemoryStream;
            var fileType = results2["fileType"];
            var fileName = results2["fileName"];

            //⑦以降、ファイル出力処理
            //※4：出力②～の処理に同じ

            // ファイル保存処理
            // - ファイルを一時ﾌｫﾙﾀﾞに保存
            //作成ファイル情報設定
            FileUtil.Extension ext = FileUtil.GetFileExtFromFileType((string)fileType);      //ファイル拡張子
            CommonProcReturn returnInfoW = setCreateFileInfo(procData, result.ToArray(), (string)fileName, ext);
            if (returnInfoW.IsProcEnd())
            {
                return returnInfoW;
            }

            //処理結果をマージ
            MargeReturnInfo(ref returnInfo, returnInfoW);
            //処理ｽﾃｰﾀｽ：正常
            return returnInfo;
        }

        //★
        ///// <summary>
        ///// 処理区分に応じた共通処理
        ///// 　7:行追加処理
        ///// </summary>
        ///// <param name="procData">
        ///// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        ///// 　ConductId:機能ID(*)
        ///// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        ///// 　FormNo:画面NO(*)
        ///// 　CtrlId:行追加ﾎﾞﾀﾝｺﾝﾄﾛｰﾙID(*)
        ///// 　PageNo:ﾍﾟｰｼﾞ番号(*)
        ///// 　　※最終ﾍﾟｰｼﾞを指定
        ///// 　PageCount:1ﾍﾟｰｼﾞ表示行数(*)
        ///// 　listDefines
        ///// 　  CtrlId:ｺﾝﾄﾛｰﾙID(*)
        ///// 　　  ※ﾃﾞｰﾀ取得を行う一覧のｺﾝﾄﾛｰﾙID
        ///// 　listData
        ///// 　　※ﾃﾞｰﾀ取得を行う一覧の入力値ﾊﾞｯｸｱｯﾌﾟﾃﾞｰﾀ
        ///// </param>
        ///// <param name="results">結果データ(O)</param>
        ///// <returns>処理ｽﾃｰﾀｽ情報</returns>
        //public CommonProcReturn AddRowProcess(CommonProcData procData, out IList<COM_TMPTBL_DATA> results)

        //{
        //    results = null;

        //    CommonDataUtil commonDataUtil = new CommonDataUtil();

        //    //②画面.入力内容を共通＿結果用中間テーブルに登録
        //    //③入力ﾁｪｯｸ用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
        //    //  (例)KK00180.AddRowCheck0
        //    //④ｴﾗｰの場合、処理を中断
        //    //⑤実行用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
        //    //  (例)KK00180.AddRow0
        //    //⑥ｴﾗｰの場合、処理を中断
        //    CommonProcReturn returnInfo = businessLogicExec(commonDataUtil,
        //        procData, FORM_DEFINE_CONSTANTS.AREAKBN.Detail);
        //    if (returnInfo.IsProcEnd())
        //    {
        //        return returnInfo;
        //    }

        //    //結果用中間ﾃﾞｰﾀ取得
        //    //※指定されたｺﾝﾄﾛｰﾙIDのﾃﾞｰﾀが対象
        //    //※指定されたﾍﾟｰｼﾞ番号から1ﾍﾟｰｼﾞの行数分が対象
        //    procData.CtrlId = procData.ListDefines[0].CTRLID;
        //    int pageCount = -1;
        //    int.TryParse(procData.ListDefines[0].ROWNO.ToString(), out pageCount);
        //    procData.PageCount = pageCount;

        //    procData.PageNo = -1;
        //    if (pageCount > 0)
        //    {
        //        //改ﾍﾟｰｼﾞありの場合、最終ﾍﾟｰｼﾞを表示
        //        int dataCount = commonDataUtil.GetTmpTblDataCount(procData, true,
        //            TMPTBL_CONSTANTS.DATATYPE.Result);  //0:結果ﾃﾞｰﾀ
        //        double pageAllCount = Math.Ceiling(((double)dataCount / pageCount));
        //        int pageNo = -1;
        //        int.TryParse(pageAllCount.ToString(), out pageNo);
        //        procData.PageNo = pageNo;
        //    }

        //    results = commonDataUtil.GetTmpTblData(procData, true,
        //        TMPTBL_CONSTANTS.DATATYPE.Result, 0, true, true);  //0:結果ﾃﾞｰﾀ

        //    //処理ｽﾃｰﾀｽ：正常
        //    return returnInfo;
        //}
        //★

        /// <summary>
        /// 処理区分に応じた共通処理
        /// 　90:バッチ実行処理
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID(*)
        /// 　　※ｲﾍﾞﾝﾄﾎﾞﾀﾝのｺﾝﾄﾛｰﾙID
        /// 　ConditionData(val1～100):条件ﾃﾞｰﾀ(*)
        /// </param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn ComBatProcess(CommonProcData procData, out object results)
        {
            results = null;

            //②画面.条件を共通＿条件用中間テーブルに登録
            //③ﾊﾞｯﾁﾘｸｴｽﾄ用共通ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
            //④ｴﾗｰの場合、処理を中断
            CommonProcReturn returnInfo = businessLogicExec(procData, out results);

            //処理ｽﾃｰﾀｽ：正常
            // - 処理完了ﾒｯｾｰｼﾞを設定
            if (string.IsNullOrEmpty(returnInfo.MESSAGE))
            {
                returnInfo.MESSAGE = "バッチ実行リクエスト処理が完了しました。";      //『バッチ実行リクエスト処理が完了しました。』
            }
            return returnInfo;
        }

        /// <summary>
        /// 処理区分に応じた共通処理
        /// 　93:CSVファイル取り込み処理
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID(*)
        /// 　　※ｲﾍﾞﾝﾄﾎﾞﾀﾝのｺﾝﾄﾛｰﾙID
        /// 　ConditionData(val1～100):条件ﾃﾞｰﾀ(*)
        /// </param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn ComUploadProcess(CommonProcData procData, out object result)
        {
            result = null;
            //ビジネスロジック実行
            //①ファイルを読込み⇒COM_TMPTBL_UPLOAD
            //③取り込み処理
            //⑥ｴﾗｰの場合、処理を中断
            CommonProcReturn returnInfo = businessLogicExec(procData, out result);    //ﾃﾞｰﾀ保存なし

            //処理ｽﾃｰﾀｽ：正常
            // - 処理完了ﾒｯｾｰｼﾞを設定
            if (string.IsNullOrEmpty(returnInfo.MESSAGE))
            {
                //メッセージ取得
                GetResourceName(procData, new List<string> { "941120012" }, out IDictionary<string, string> resources);
                //処理が完了しました。
                returnInfo.MESSAGE = ConvertResourceName("941120012", resources);
            }
            return returnInfo;
        }

        /// <summary>
        /// 処理区分に応じた共通処理
        /// 　93:CSVファイル取り込み処理
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID(*)
        /// 　　※ｲﾍﾞﾝﾄﾎﾞﾀﾝのｺﾝﾄﾛｰﾙID
        /// 　ConditionData(val1～100):条件ﾃﾞｰﾀ(*)
        /// </param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn ExcelPortUploadProcess(CommonProcData procData, out object retResults)
        {
            retResults = null;

            //処理結果を初期化
            object logicResult = null;
            MemoryStream stream = null;
            try
            {
                //ビジネスロジック実行
                //①ファイルを読込み⇒COM_TMPTBL_UPLOAD
                //③取り込み処理
                //⑥ｴﾗｰの場合、処理を中断
                CommonProcReturn returnInfo = businessLogicExec(procData, out logicResult);    //ﾃﾞｰﾀ保存なし
                                                                                          //処理結果を初期化
                if (returnInfo.IsProcEnd())
                {
                    Dictionary<string, object> resultsW = logicResult as Dictionary<string, object>;
                    if (resultsW.ContainsKey("OutputStream") && resultsW["OutputStream"] != null)
                    {
                        //ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞを同期に設定
                        procData.FileDownloadSet = AppConstants.FileDownloadSet.Hidouki.GetHashCode();

                        // メッセージ取得
                        GetResourceName(procData, new List<string> { "141220007" }, out IDictionary<string, string> resources);
                        // 「入力エラーが存在します。ダウンロードされたEXCELよりエラー内容を確認してください。」
                        returnInfo.MESSAGE = ConvertResourceName("141220007", resources);

                        // OutputStreamが設定されている場合のみ出力処理を行う
                        stream = resultsW["OutputStream"] as MemoryStream;
                        var fileType = resultsW["fileType"].ToString();
                        var fileName = resultsW["fileName"].ToString();

                        //⑦以降、ファイル出力処理
                        //※4：出力②～の処理に同じ

                        // ファイル保存処理
                        // - ﾌｧｲﾙを一時ﾌｫﾙﾀﾞに保存
                        //作成ファイル情報設定
                        FileUtil.Extension ext = FileUtil.GetFileExtFromFileType(fileType);      //ファイル拡張子
                        CommonProcReturn returnInfoW = setCreateFileInfo(procData, stream.ToArray(), fileName, ext);
                        if (returnInfoW.IsProcEnd())
                        {
                            return returnInfoW;
                        }

                        //処理結果をマージ
                        MargeReturnInfo(ref returnInfo, returnInfoW);

                        Dictionary<string, object> results = new Dictionary<string, object>();
                        if (resultsW.ContainsKey("Result"))
                        {
                            results.Add("Result", resultsW["Result"]);
                        }
                        if (resultsW.ContainsKey("Individual"))
                        {
                            results.Add("Individual", resultsW["Individual"]);
                        }
                        if (resultsW.ContainsKey("ButtonStatus"))
                        {
                            results.Add("ButtonStatus", resultsW["ButtonStatus"]);
                        }
                        if (results.Count > 0)
                        {
                            retResults = results;
                        }
                    }
                    else
                    {
                        retResults = logicResult;
                    }
                    return returnInfo;
                }

                //処理ｽﾃｰﾀｽ：正常
                // - 処理完了ﾒｯｾｰｼﾞを設定
                if (string.IsNullOrEmpty(returnInfo.MESSAGE))
                {
                    //メッセージ取得
                    GetResourceName(procData, new List<string> { "941120012" }, out IDictionary<string, string> resources);
                    //処理が完了しました。
                    returnInfo.MESSAGE = ConvertResourceName("941120012", resources);
                }
                return returnInfo;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// ツリービューの構成リストを取得
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID(*)
        /// 　　※ｲﾍﾞﾝﾄﾎﾞﾀﾝのｺﾝﾄﾛｰﾙID
        /// 　ConditionData(val1～100):条件ﾃﾞｰﾀ(*)
        /// </param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn GetStructureList(CommonProcData procData, ref Dictionary<string, List<CommonTreeViewInfo>> treeViewDic)
        {
            //構成リストを取得
            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            Dictionary<string, List<CommonStructure>> structureDic;
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_GetStructureList(procData, out structureDic);
            if (returnInfo.IsProcEnd())
            {
                returnInfo.STATUS = CommonProcReturn.ProcStatus.InValid;
                return returnInfo;
            }

            foreach (var grpId in procData.StructureGroupList)
            {
                var treeviewList = new List<CommonTreeViewInfo>();
                if (structureDic.ContainsKey(grpId.ToString()))
                {
                    var structureList = structureDic[grpId.ToString()];
                    var rootId = 0;
                    foreach (var structureInfo in structureList)
                    {
                        var info = new CommonTreeViewInfo(structureInfo);
                        if (structureInfo.StructureLayerNo == -1)
                        {
                            // ルートの親ノードIDは"#"
                            info.Parent = "#";
                            info.State.Opened = true;
                            rootId = structureInfo.StructureId;
                        }
                        else if (structureInfo.StructureLayerNo == 0)
                        {
                            // ルートの構成IDを1階層目の親ノードIDにセット
                            info.Parent = rootId.ToString();
                        }
                        treeviewList.Add(info);
                    }
                    treeViewDic.Add(grpId.ToString(), treeviewList);
                }
            }
            return returnInfo;
        }

        /// <summary>
        /// 1ページ当たりの行数コンボを取得
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　LoginUserId:ログインユーザID(*)
        /// </param>
        /// <param name="comboValuesRowsPerPage">out 1ページ当たりの行数コンボの値</param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn GetComboRowsPerPage(CommonProcData procData, out List<int> comboValuesRowsPerPage)
        {
            // - 業務ロジックコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetResourcesValue
            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_GetComboRowsPerPage(procData, out comboValuesRowsPerPage);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }

            //処理結果
            return returnInfo;
        }

        /// <summary>
        /// 項目カスタマイズ情報を登録
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　LoginUserId:ログインユーザID(*)
        /// </param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn SaveCustomizeListInfo(CommonProcData procData)
        {
            // - 業務ロジックコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：SaveCustomizeListInfo
            // ここで渡ってくるprocDataのコントロールIDは一覧のコントロールIDのため、
            // 起動処理のコントロールIDを設定し直す
            var ctrlId = procData.CtrlId;
            procData.CtrlId = "SaveCustomizeListInfo";

            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_SaveCustomizeListInfo(procData);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }

            //処理結果
            return returnInfo;
        }

        /// <summary>
        /// 言語コンボを取得
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　LoginUserId:ログインユーザID(*)
        /// </param>
        /// <param name="comboLanguageItemList">out 言語コンボの値</param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn GetLanguageItemList(CommonProcData procData, out List<LanguageInfo> comboLanguageItemList)
        {
            // - 業務ロジックコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetLanguageItemList
            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_GetLanguageItemList(procData, out comboLanguageItemList);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }

            //処理結果
            return returnInfo;
        }

        /// <summary>
        /// ユーザ権限機能IDリストを取得
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　LoginUserId:ログインユーザID(*)
        /// </param>
        /// <param name="resultsMenu">機能リスト</param>
        /// <returns></returns>
        public CommonProcReturn GetUserConductAuthorityList(CommonProcData procData, CommonDataEntities context, out IList<CommonConductMst> resultsMenu)
        {
            resultsMenu = null;

            // - 業務ロジックコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetLanguageItemList
            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_GetUserConductAuthorityList(procData, out List<string> conductIdList);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }
            BusinessLogicUtil blogic = new BusinessLogicUtil();
            AuthenticationUtil authU = new AuthenticationUtil(context);
            returnInfo = authU.getUserMenuInfoTranslated(conductIdList, procData, ref blogic, out resultsMenu);

            //処理結果
            return returnInfo;
        }

        //★del
        ///// <summary>
        ///// 画面入力値を中間ﾃｰﾌﾞﾙにﾊﾞｯｸｱｯﾌﾟ保存
        ///// </summary>
        ///// <param name="procData">
        ///// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        ///// 　ConductId:機能ID(*)
        ///// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        ///// 　FormNo:画面NO(*)
        ///// 　ConditionData(val1～100):条件ﾃﾞｰﾀ(*)
        ///// </param>
        ///// <param name="results">結果データ(O)</param>
        ///// <returns>処理ｽﾃｰﾀｽ情報</returns>
        //public CommonProcReturn SaveDataProcess(ref CommonDataUtil commonDataUtil,
        //    CommonProcData procData, byte areaKbn = FORM_DEFINE_CONSTANTS.AREAKBN.Condition)
        //{
        //    CommonProcReturn returnInfo = new CommonProcReturn();

        //    //②画面.条件を共通＿条件用中間テーブルに登録
        //    using (DbContextTransaction transaction = commonDataUtil.DbBeginTrans())
        //    {
        //        try
        //        {
        //            //②画面.入力値を中間ﾃｰﾌﾞﾙに保存
        //            //※Delete⇒Insert
        //            returnInfo = commonDataUtil.SaveTmptbl(procData, areaKbn);

        //            transaction.Commit();
        //        }
        //        catch (DbException ex)
        //        {
        //            transaction.Rollback();

        //            //実行時例外ｴﾗｰ
        //            throw ex;
        //        }
        //    }

        //    //処理ｽﾃｰﾀｽ：正常
        //    return returnInfo;
        //}

        /// <summary>
        /// 処理結果１に処理結果２をマージする
        /// </summary>
        /// <param name="retinfo1"></param>
        /// <param name="retinfo2"></param>
        /// <returns></returns>
        public void MargeReturnInfo(ref CommonProcReturn retinfo, CommonProcReturn retinfoW)
        {
            if (retinfo == null)
            {
                retinfo = retinfoW;
                return;
            }

            //if (retinfo.STATUS == CommonProcReturn.ProcStatus.Valid)
            //{
            //    //前処理が正常の場合、後処理で上書き
            //    //※ﾒｯｾｰｼﾞを連結
            //    string message = String.Empty;

            //    IList<string> messages = new List<string>();
            //    if (!String.IsNullOrEmpty(retinfo.MESSAGE))
            //    {
            //        messages.Add(retinfo.MESSAGE);
            //    }
            //    if (!String.IsNullOrEmpty(retinfoW.MESSAGE))
            //    {
            //        messages.Add(retinfoW.MESSAGE);
            //    }
            //    if (messages.Count > 0)
            //    {
            //        message = String.Join(" / ", messages.ToArray());
            //    }

            //    retinfo = retinfoW;
            //}
            //else
            //{
            //    //前処理が正常以外の場合、処理中断ステータスが優先
            //    //-- ステータス
            //    if (retinfoW.IsProcEnd())
            //    {
            //        retinfo.STATUS = retinfoW.STATUS;
            //    }
            //    else
            //    {
            //        //その他情報はマージ
            //        if (retinfoW.FILEDATA != null)
            //        {
            //            retinfo.FILEDATA = retinfoW.FILEDATA;
            //        }
            //        if (!string.IsNullOrEmpty(retinfoW.FILENAME))
            //        {
            //            retinfo.FILENAME = retinfoW.FILENAME;
            //        }
            //        if (!string.IsNullOrEmpty(retinfoW.FILEDOWNLOADNAME))
            //        {
            //            retinfo.FILEDOWNLOADNAME = retinfoW.FILEDOWNLOADNAME;
            //        }

            //    }

            //    //-- メッセージは追加
            //    if (!string.IsNullOrEmpty(retinfoW.MESSAGE))
            //    {
            //        if (string.IsNullOrEmpty(retinfo.MESSAGE))
            //        {
            //            retinfo.MESSAGE = retinfoW.MESSAGE;
            //        }
            //        else
            //        {
            //            retinfo.MESSAGE += (" / " + retinfoW.MESSAGE);
            //        }
            //    }

            //    //-- ログは追加
            //    if (!string.IsNullOrEmpty(retinfoW.LOGNO))
            //    {
            //        if (string.IsNullOrEmpty(retinfo.LOGNO))
            //        {
            //            retinfo.LOGNO = retinfoW.LOGNO;
            //        }
            //        else
            //        {
            //            retinfo.LOGNO += ("," + retinfoW.LOGNO);
            //        }
            //    }

            //}
            // 処理中断ステータスの場合、ステータスを上書き
            if (retinfoW.IsProcEnd())
            {
                retinfo.STATUS = retinfoW.STATUS;
            }

            //その他情報はマージ
            if (retinfoW.FILEDATA != null)
            {
                retinfo.FILEDATA = retinfoW.FILEDATA;
            }
            if (!string.IsNullOrEmpty(retinfoW.FILENAME))
            {
                retinfo.FILENAME = retinfoW.FILENAME;
            }
            if (!string.IsNullOrEmpty(retinfoW.FILEPATH))
            {
                retinfo.FILEPATH = retinfoW.FILEPATH;
            }
            if (!string.IsNullOrEmpty(retinfoW.FILEDOWNLOADNAME))
            {
                retinfo.FILEDOWNLOADNAME = retinfoW.FILEDOWNLOADNAME;
            }

            //-- メッセージは追加
            if (!string.IsNullOrEmpty(retinfoW.MESSAGE))
            {
                if (string.IsNullOrEmpty(retinfo.MESSAGE))
                {
                    retinfo.MESSAGE = retinfoW.MESSAGE;
                }
                else
                {
                    retinfo.MESSAGE += (" / " + retinfoW.MESSAGE);
                }
            }

            //-- ログは追加
            if (!string.IsNullOrEmpty(retinfoW.LOGNO))
            {
                if (string.IsNullOrEmpty(retinfo.LOGNO))
                {
                    retinfo.LOGNO = retinfoW.LOGNO;
                }
                else
                {
                    retinfo.LOGNO += ("," + retinfoW.LOGNO);
                }
            }

            return;
        }

        /// <summary>
        /// Excelﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ用の応答ﾒｯｾｰｼﾞ生成
        /// </summary>
        /// <returns>Excelﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ用応答ﾒｯｾｰｼﾞ</returns>
        /// <remarks>AppConstants.FileDownloadSetting = Doukiの場合に使用</remarks>
        public HttpResponseMessage ExcelDownload(byte[] fileData, string fileDownloadName)
        {
            //④Excelダウンロード
            // - ContentType
            string contentType = SystemUtil.GetContentType(fileDownloadName);

            // - 応答ﾒｯｾｰｼﾞ生成
            //HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            ////Content作成
            //response.Content = new ByteArrayContent(fileData);
            ////Contentﾍｯﾀﾞ設定
            //response.Content.Headers.Add("Content-Type", contentType);
            //response.Content.Headers.Add("Content-Disposition", "attachment; filename=Sample.xlsx");

            //return response;

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                //Content作成
                Content = new ByteArrayContent(fileData)
                {
                    Headers =
                    {
                        //Contentﾍｯﾀﾞ設定
                        ContentType = new MediaTypeHeaderValue(contentType),
                        ContentDisposition = new ContentDispositionHeaderValue("attatchment")
                        {
                            FileName = fileDownloadName
                        }
                    },
                }
            };

        }

        /// <summary>
        /// 画像表示情報を取得
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　LoginUserId:ログインユーザID(*)
        /// </param>
        /// <param name="fileInfo">表示する画像の情報</param>
        /// <param name="filePath">表示する画像のファイルパス</param>
        /// <returns>処理ステータス情報</returns>
        public CommonProcReturn GetImageFileInfo(CommonProcData procData, string fileInfo, out string filePath)
        {
            procData.CtrlId = "GetImageFileInfo";
            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            // 画像ファイル情報取得
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_GetImageFileInfo(procData, fileInfo, out filePath);

            //処理結果
            return returnInfo;
        }

        /// <summary>
        /// 復号化データを取得
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　LoginUserId:ログインユーザID(*)
        /// </param>
        /// <param name="decryptedData">復号化データ</param>
        /// <returns>処理ステータス情報</returns>
        public CommonProcReturn GetDecryptedData(CommonProcData procData, string encryptedData, out string decryptedData)
        {
            procData.CtrlId = "GetDecryptedData";
            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            // 復号化データ取得
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_GetDecryptedData(procData, encryptedData, out decryptedData);

            //処理結果
            return returnInfo;
        }

        /// <summary>
        /// メールアドレスからユーザIDを取得
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　mailAdress:メールアドレス(*)
        /// </param>
        /// <param name="userId">ユーザID</param>
        /// <returns>処理ステータス情報</returns>
        public CommonProcReturn GetUserIdByMailAdress(CommonProcData procData, string mailAdress, out List<int> userIdList)
        {
            procData.CtrlId = "GetUserIdByMailAdress";
            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            // ユーザID取得
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_GetUserIdByMailAdress(procData, mailAdress, out userIdList);

            //処理結果
            return returnInfo;
        }

        /// <summary>
        /// ユーザー権限情報取得
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　LoginUserId:ログインユーザID(*)
        /// </param>
        /// <param name="userId">ユーザID</param>
        /// <returns>処理ステータス情報</returns>
        public CommonProcReturn GetUserAuthorizationInfo(CommonProcData procData, CommonDataEntities context, ref UserInfoDef userInfo)
        {
            //== ﾛｸﾞｲﾝ認証 ==
            AuthenticationUtil authU = new AuthenticationUtil(context);
            BusinessLogicUtil blogic = new BusinessLogicUtil();

            userInfo = null;

            bool isCheckOK = false;
            CommonUserMst loginData = new CommonUserMst(procData.LoginId, string.Empty, string.Empty, procData.LoginUserId);
            CommonProcReturn returnInfo = authU.LoginAuthentication(loginData, false, procData, out isCheckOK, ref userInfo, true);
            if (returnInfo.IsProcEnd())
            {
                //ﾕｰｻﾞｰ認証NG
                return returnInfo;
            }
            if (false == isCheckOK)
            {
                //ﾕｰｻﾞｰ認証NG
                blogic.GetResourceName(procData, new List<string> { CommonSTDUtil.CommonResources.ID.ID941430004 }, out IDictionary<string, string> resources);
                return new CommonProcReturn(CommonProcReturn.ProcStatus.Error, blogic.ConvertResourceName(CommonSTDUtil.CommonResources.ID.ID941430004, resources));   // ログイン認証に失敗しました。ユーザー権限がありません。
            }

            //処理結果
            return returnInfo;
        }
        #endregion

        #region === Private処理 ===
        /// <summary>
        /// ﾕｰｻﾞｰ機能権限に沿ってｱｸｾｽ可能な機能かﾁｪｯｸする
        /// </summary>
        /// <param name="conductId">ﾁｪｯｸ機能Id</param>
        /// <param name="userAuthConducts">ﾕｰｻﾞｰ機能権限ﾏｽﾀ(※ﾒﾆｭｰ構成で保持)</param>
        /// <returns></returns>
        private bool isAcceptConduct(string conductId, IList<CommonConductMst> userAuthConducts)
        {
            foreach (CommonConductMst userAuthConduct in userAuthConducts)
            {
                //共通機能
                if (userAuthConduct.CM_CONDUCTMSTS != null &&
                    userAuthConduct.CM_CONDUCTMSTS.Count > 0)
                {
                    if (userAuthConduct.CM_CONDUCTMSTS.Where(x => x.CONDUCTMST.CONDUCTID == conductId).Count() > 0)
                    {
                        return true;    //ｱｸｾｽ許可
                    }
                }
                if (userAuthConduct.CHILDCONDUCTMST != null &&
                    userAuthConduct.CHILDCONDUCTMST.Count > 0)
                {
                    //※自分が処理ｸﾞﾙｰﾌﾟ

                    if (userAuthConduct.CHILDCONDUCTMST[0].CHILDCONDUCTMST != null &&
                        userAuthConduct.CHILDCONDUCTMST[0].CHILDCONDUCTMST.Count > 0)
                    {
                        //子機能が処理ｸﾞﾙｰﾌﾟの場合、子機能をﾁｪｯｸ
                        bool isAccept = isAcceptConduct(conductId, userAuthConduct.CHILDCONDUCTMST);
                        if (isAccept)
                        {
                            return true;    //ｱｸｾｽ許可
                        }

                    }
                    else
                    {
                        //子機能が機能ﾏｽﾀの場合、一致する機能Idが存在するかﾁｪｯｸ
                        if (userAuthConduct.CHILDCONDUCTMST.Where(x => x.CONDUCTMST.CONDUCTID == conductId).Count() > 0)
                        {
                            return true;    //ｱｸｾｽ許可
                        }
                    }
                }
                else
                {
                    //※自分が機能ﾏｽﾀ

                    //機能Idが一致するかﾁｪｯｸ
                    if (userAuthConduct.CONDUCTMST.CONDUCTID.Equals(conductId))
                    {
                        return true;    //ｱｸｾｽ許可
                    }

                }
            }

            return false;   //ｱｸｾｽ不可
        }

        /// <summary>
        /// 該当機能IDのユーザー機能権限有無を取得
        /// </summary>
        /// <param name="procData">ﾘｸｴｽﾄ情報</param>
        /// <returns>権限有無(true:権限有り/false:権限無し)</returns>
        private bool checkUserConductAuthority(CommonProcData procData)
        {
            var hasAuthority = false;

            //①業務ﾛｼﾞｯｸ機能単位のDLL - 権限チェック用処理
            //②ｴﾗｰの場合、処理を中断
            CommonProcData procDataW = new CommonProcData();
            procDataW.AuthorityLevelId = procData.AuthorityLevelId; //IN:権限レベルID
            procDataW.BelongingInfo = procData.BelongingInfo;   //IN:所属情報
            procDataW.ConductId = procData.ConductId;       //IN:機能ID
            procDataW.LoginUserId = procData.LoginUserId;   //IN:登録者ID
            procDataW.CtrlId = BusinessLogicIO.dllProcName_CheckConductAuthority;  //IN：起動処理名（CheckConductAuthority）
            procDataW.LanguageId = procData.LanguageId;     //IN:言語ID

            BusinessLogicIO logicIO = new BusinessLogicIO(procDataW);
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_CheckUserConductAuthority(procDataW, ref hasAuthority);
            if (returnInfo.IsProcEnd())
            {
                // 機能権限無し
                return false;
            }
            return hasAuthority;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸ実行処理
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※必須項目:(*))
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID
        /// 　ConditionData(val1～100):条件ﾃﾞｰﾀ
        /// 　ListData(val1～200):明細ﾃﾞｰﾀ一覧
        /// </param>
        /// <param name="retResults">(OUT)結果リスト(JSON文字列)</param>
        /// <returns>実行ステータス</returns>
        /// <remarks>
        /// ①画面.入力値を中間ﾃｰﾌﾞﾙに保存
        /// ②入力ﾁｪｯｸ用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
        /// ③実行用ﾌﾟﾛｼｰｼﾞｬｺｰﾙ
        /// </remarks>
        private CommonProcReturn businessLogicExec(CommonProcData procData, out object retResults)
        {
            //業務ロジック機能DLLをコール
            // DLL名：「)BusinessLogic_」+ [ﾌﾟﾛｸﾞﾗﾑID].dll
            // (例)BusinessLogic_SR01720.dll
            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic(procData, out retResults);
            if (returnInfo.IsProcEnd())
            {
                //ｴﾗｰ情報を返す
                return returnInfo;
            }

            //正常ｽﾃｰﾀｽを返す
            return (returnInfo == null ? new CommonProcReturn() : returnInfo);
        }

        //↓↓.NET5版ではプッシュ通知は未実装↓↓
        ///// <summary>
        ///// ﾌﾟｯｼｭ通知呼出
        ///// </summary>
        ///// <param name="targets">Dictionary{対象ｸﾞﾙｰﾌﾟ名(ﾕｰｻﾞｰID), 通知の蓄積件数}</param>
        //public void callPush(Dictionary<string, int> targets)
        //{
        //    PushNotify(targets);
        //}

        ///// <summary>
        ///// ﾌﾟｯｼｭ通知
        ///// </summary>
        ///// <param name="targets">Dictionary{対象ｸﾞﾙｰﾌﾟ名(ﾕｰｻﾞｰID), 通知の蓄積件数}</param>
        //private static async void PushNotify(Dictionary<string, int> targets)
        //{
        //    //var hubContext = GlobalHost.ConnectionManager.GetHubContext<PushHub>();
        //    //if (targets != null)
        //    //{
        //    //    foreach (var target in targets)
        //    //    {
        //    //        string userId = target.Key;
        //    //        int count = target.Value;
        //    //        await hubContext.Clients.Group(userId).pushNotify(count);
        //    //    }
        //    //}
        //}
        //↑↑.NET5版ではプッシュ通知は未実装↑↑

        /// <summary>
        /// COM_FORM_DEFINEインスタンス生成
        /// </summary>
        /// <param name="pgmId">プログラムID</param>
        /// <param name="formNo">画面番号</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <returns></returns>
        private COM_FORM_DEFINE createFormDefineInstance(string pgmId, short formNo, string ctrlId)
        {
            var define = new COM_FORM_DEFINE();
            define.PGMID = pgmId;
            define.FORMNO = formNo;
            define.CTRLID = ctrlId;
            define.DISPORDER = 0;
            define.TABNO = 0;
            define.CTRLGRPNO = 0;
            define.DAT_TRANSPTN = FORM_DEFINE_CONSTANTS.DAT_TRANSPTN.None;
            define.DAT_TRANSDISPPTN = FORM_DEFINE_CONSTANTS.DAT_TRANSDISPPTN.None;
            define.DAT_TRANSTARGET = "-";
            define.DAT_TRANSICONKBN = FORM_DEFINE_CONSTANTS.DAT_TRANSICONKBN.None;
            define.DAT_EDITPTN = FORM_DEFINE_CONSTANTS.DAT_EDITPTN.None;
            define.DAT_DIRECTION = FORM_DEFINE_CONSTANTS.DAT_DIRECTION.None;
            define.DAT_HEADERDISPKBN = FORM_DEFINE_CONSTANTS.DAT_HEADERDISPKBN.Hide;
            define.DAT_SWITCHKBN = FORM_DEFINE_CONSTANTS.DAT_SWITCHKBN.None;
            define.DAT_ROWADDKBN = FORM_DEFINE_CONSTANTS.DAT_ROWADDKBN.None;
            define.DAT_ROWDELKBN = FORM_DEFINE_CONSTANTS.DAT_ROWDELKBN.None;
            define.DAT_ROWSELKBN = FORM_DEFINE_CONSTANTS.DAT_ROWSELKBN.None;
            define.DAT_COLSELKBN = FORM_DEFINE_CONSTANTS.DAT_COLSELKBN.None;
            define.DAT_ROWSORTKBN = FORM_DEFINE_CONSTANTS.DAT_ROWSORTKBN.None;
            define.DAT_HEIGHT = 0;
            define.CTR_RELATIONCTRLID = "-";
            define.CTR_POSITIONKBN = FORM_DEFINE_CONSTANTS.CTR_POSITIONKBN.None;
            define.DELFLG = false;

            return define;
        }

        /// <summary>
        /// COM_LISTITEM_DEFINEインスタンス生成
        /// </summary>
        /// <param name="pgmId">プログラムID</param>
        /// <param name="formNo">画面番号</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <returns></returns>
        private COM_LISTITEM_DEFINE createListItemDefineInstance(string pgmId, short formNo, string ctrlId)
        {
            var define = new COM_LISTITEM_DEFINE();
            define.PGMID = pgmId;
            define.FORMNO = formNo;
            define.CTRLID = ctrlId;
            define.DEFINETYPE = LISTITEM_DEFINE_CONSTANTS.DEFINETYPE.DataRow;
            define.ITEMNO = 1;
            define.ROWNO = 1;
            define.COLNO = 1;
            define.ROWSPAN = 1;
            define.COLSPAN = 1;
            define.POSITION = LISTITEM_DEFINE_CONSTANTS.HEADER_ALIGN.None;
            define.COLWIDTH = "0";
            define.FROMTOKBN = LISTITEM_DEFINE_CONSTANTS.FROMTOKBN.None;
            define.ITEM_CNT = 1;
            define.NULLCHKKBN = LISTITEM_DEFINE_CONSTANTS.NULLCHKKBN.None;
            define.TXT_AUTOCOMPKBN = LISTITEM_DEFINE_CONSTANTS.TXT_AUTOCOMPKBN.None;
            define.BTN_CTRLID = "-";
            define.BTN_ACTIONKBN = LISTITEM_DEFINE_CONSTANTS.BTN_AFTEREXECKBN.None;
            define.BTN_AUTHCONTROLKBN = LISTITEM_DEFINE_CONSTANTS.BTN_AUTHCONTROLKBN.Free;
            define.BTN_AFTEREXECKBN = LISTITEM_DEFINE_CONSTANTS.BTN_AFTEREXECKBN.None;
            define.RELATIONID = "-";
            define.UNCHANGEABLEKBN = LISTITEM_DEFINE_CONSTANTS.UNCHANGEABLEKBN.Changeable;
            define.COLFIXKBN = LISTITEM_DEFINE_CONSTANTS.COLFIXKBN.None;
            define.EXP_LIKE_PATTERN = LISTITEM_DEFINE_CONSTANTS.LIKE_PATTERN.PerfectMatch;
            define.EXP_IN_CLAUSE_KBN = LISTITEM_DEFINE_CONSTANTS.IN_CLAUSE_KBN.None;
            define.EXP_LOCK_TYPE = LISTITEM_DEFINE_CONSTANTS.LOCK_TYPE.None;
            define.DELFLG = false;

            return define;
        }

        /// <summary>
        /// 作成ﾌｧｲﾙ情報設定
        ///  - 結果情報に設定して返却
        /// </summary>
        /// <returns></returns>
        /// <remarks>作成ﾌｧｲﾙ情報を設定した結果情報</remarks>
        private CommonProcReturn setCreateFileInfo(CommonProcData procData,
            byte[] outputDatas, string fileName, FileUtil.Extension fileExt)
        {
            CommonProcReturn returnInfo = new CommonProcReturn();

            if (procData.IsFileDownloadHidouki())
            {
                //※非同期の場合

                try
                {
                    //作成するExcelのﾌｧｲﾙ名を設定
                    string createFileName = fileName;
                    FileUtil.SetFileExtension(
                        fileName,
                        fileExt);

                    string createFilePath = "";
                    GetTemporaryFolderPath(procData, out createFilePath);
                    createFilePath = Path.Combine(createFilePath, createFileName);

                    //保存ﾌｫﾙﾀﾞを作成
                    FileUtil.CreateDirectory(createFilePath);

                    //ﾌｧｲﾙに書き込む
                    using (FileStream fs = new FileStream(createFilePath,
                        FileMode.Create,
                        FileAccess.Write))
                    {
                        fs.Write(outputDatas, 0, outputDatas.Length);
                    }

                    //作成ﾌｧｲﾙ名を設定
                    returnInfo.FILENAME = createFileName;
                    returnInfo.FILEPATH = createFilePath;

                }
                catch (Exception ex)
                {
                    return new CommonProcReturn(CommonProcReturn.ProcStatus.Error, ex.Message);
                }

            }
            else
            {
                //※同期の場合

                //作成ﾌｧｲﾙﾃﾞｰﾀを設定
                returnInfo.FILEDATA = outputDatas;

            }

            // - ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ情報設定
            string fileDownloadName = FileUtil.SetFileExtension(
                fileName,
                fileExt);
            returnInfo.FILEDOWNLOADNAME = fileDownloadName;

            return returnInfo;
        }

        /// <summary>
        /// テンポラリフォルダのパスを取得
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※以下、使用項目、内(*)：必須項目)
        /// 　LoginUserId:ログインユーザID(*)
        /// </param>
        /// <param name="tempFolderPath">out テンポラリフォルダのパス</param>
        /// <returns>処理ｽﾃｰﾀｽ情報</returns>
        public CommonProcReturn GetTemporaryFolderPath(CommonProcData procData, out string tempFolderPath)
        {
            // - 業務ロジックコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetTemporaryFolderPath
            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_GetTemporaryFolderPath(procData, out tempFolderPath);
            if (returnInfo.IsProcEnd())
            {
                //エラーの場合、処理を中断
                return returnInfo;
            }

            //処理結果
            return returnInfo;
        }
        #endregion

    }
}