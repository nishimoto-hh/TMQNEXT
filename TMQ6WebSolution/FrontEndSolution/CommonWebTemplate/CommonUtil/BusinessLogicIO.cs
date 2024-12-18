using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;

//2024.09 .NET8バージョンアップ対応 start
//using Microsoft.Extensions.PlatformAbstractions;
//2024.09 .NET8バージョンアップ対応 end
using static CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;

namespace CommonWebTemplate.CommonUtil
{
    /// <summary>
    /// 業務ロジックDLLコール IOクラス
    /// </summary>
    public class BusinessLogicIO
    {
        #region === 定数 ===
        /// <summary>
        /// 実行モード
        /// </summary>
        enum execModeDef
        {
            /// <summary>
            /// Releaseモード
            /// </summary>
            Default = 0,
            /// <summary>
            /// プロトタイプ作成モード
            /// </summary>
            Proto = 1,
            /// <summary>
            /// メンテナンスモード（共通FW用)
            /// </summary>
            Test = 3,
        }

        /// <summary>
        /// DLL名（開発テスト用）【DLL名固定】
        /// </summary>
        const string dllName_Test = "BusinessLogic_Test";
        /// <summary>
        /// DLL名（プロトモード用）【DLL名固定】
        /// </summary>
        const string dllName_Proto = "BusinessLogicProtoType";
        /// <summary>
        /// DLL名（共通処理用）【DLL名固定】
        /// </summary>
        const string dllName_Common = "BusinessLogic_Common";
        /// <summary>
        /// DLL名接頭語（通常用）【DLL名はPGMID単位】
        /// </summary>
        /// <remarks>DLL名命名規則：DLL名接頭語+PGMID（例：BusinessLogic_SR01720）</remarks>
        const string dllName_DefaultPrefix = "BusinessLogic_";

        /// <summary>
        /// 業務ロジックDLLコール関数名【関数名固定】
        /// </summary>
        const string dllMethodName_Default = "ExecuteBusinessLogic";

        /// 起動処理名（機能単位DLL：ボタン権限チェック）
        /// </summary>
        public const string dllProcName_CheckAuthority = "CheckAuthority";
        /// 起動処理名（機能単位DLL：機能権限チェック）
        /// </summary>
        public const string dllProcName_CheckConductAuthority = "CheckConductAuthority";
        /// 起動処理名（機能単位DLL：リソース翻訳取得）
        /// </summary>
        public const string dllProcName_GetResourcesValue = "GetResourcesValue";
        /// <summary>
        /// 起動処理名（共通処理DLL：オートコンプリート、コンボボックス）
        /// </summary>
        const string dllProcName_CtrlSql = "CTRLSQL";
        /// <summary>
        /// 起動処理名（共通処理DLL：パスワード変更）
        /// </summary>
        const string dllProcName_ChangePass = "ChangePassword";
        /// <summary>
        /// 起動処理名（共通処理DLL：ログイン認証）
        /// </summary>
        const string dllProcName_LoginCheck = "LoginAuthentication";
        /// <summary>
        /// 起動処理名（共通処理DLL：ログアウト）
        /// </summary>
        const string dllProcName_Logout = "Logout";
        /// <summary>
        /// 起動処理名（共通処理DLL：場所階層リスト取得）
        /// </summary>
        const string dllProcName_GetStructureList = "GetStructureList";
        /// <summary>
        /// 起動処理名（共通処理DLL：ページ当たりの行数コンボ取得）
        /// </summary>
        const string dllProcName_GetComboRowsPerPage = "GetComboRowsPerPage";
        /// <summary>
        /// 起動処理名（共通処理DLL：一覧カスタマイズ情報登録＆一覧レイアウトデータ取得）
        /// </summary>
        const string dllProcName_SaveCustomizeListInfo = "SaveCustomizeListInfo";
        /// <summary>
        /// 起動処理名（共通処理DLL：言語コンボ取得）
        /// </summary>
        const string dllProcName_GetLanguageItemList = "GetLanguageItemList";
        /// <summary>
        /// 起動処理名（共通処理DLL：機能IDリスト取得）
        /// </summary>
        const string dllProcName_GetUserConductAuthorityList = "GetUserConductAuthorityList";
        /// <summary>
        /// 起動処理名（共通処理DLL：テンポラリフォルダパス取得）
        /// </summary>
        const string dllProcName_GetTemporaryFolderPath = "GetTemporaryFolderPath";
        /// <summary>
        /// 起動処理名（共通処理DLL：画像表示情報取得）
        /// </summary>
        const string dllProcName_GetImageFileInfo = "GetImageFileInfo";
        /// <summary>
        /// 起動処理名（共通処理DLL：復号化データ取得）
        /// </summary>
        const string dllProcName_GetDecryptedData = "GetDecryptedData";
        /// <summary>
        /// 起動処理名（共通処理DLL：ユーザID取得）
        /// </summary>
        const string dllProcName_GetUserIdByMailAdress = "GetUserIdByMailAdress";
        //★インメモリ化対応 start
        /// 起動処理名（共通処理DLL：共通定義データ取得）
        /// </summary>
        const string dllProcName_GetCommonDefineInfo = "GetCommonDefineInfo";
        /// <summary>
        /// 起動処理名（共通処理DLL：ユーザカスタマイズ情報取得）
        /// </summary>
        const string dllProcName_GetUserCustomizeInfo = "GetUserCustomizeInfo";
        /// <summary>
        /// 起動処理名（共通処理DLL：共有メモリコンボボックスデータ更新）
        /// </summary>
        const string dllProcName_UpdateComboBoxData = "UpdateComboBoxData";
        //★インメモリ化対応 end
        #endregion

        #region === メンバ変数 ===
        /// <summary>
        /// プロトモードフラグ
        /// </summary>
        /// <remarks>true:プロトモード</remarks>
        execModeDef execMode = execModeDef.Default;

        /// <summary>
        /// DLL名ファイルパス
        /// </summary>
        string dllFolder;

        /// <summary>
        /// 業務ロジックDLLファイル名
        /// </summary>
        string dllFileName;

        /// <summary>
        /// 業務ロジックDLLコール
        ///  - INパラメータ
        /// </summary>
        /// <remarks>DLLファンクション第1引数</remarks>
        /// <param name="TerminalNo">端末IPアドレス</param>
        /// <param name="UserId">登録者ID</param>
        /// <param name="LanguageId">言語ID</param>
        /// <param name="ConductId">機能ID</param>
        /// <param name="FormNo">Form番号</param>
        /// <param name="CtrlId">起動処理名(コントロールID)</param>
        /// <param name="ActionKbn">アクション区分</param>
        /// <param name="RootPath">ルート物理パス</param>
        /// <param name="Status">ステータス</param>
        /// <param name="JsonCondition">実行条件(JSON文字列)</param>
        /// <param name="JsonPageInfo">ページ情報(JSON文字列)</param>
        /// <param name="JsonResult">登録データ(JSON文字列)</param>
        /// <param name="InputStream">ファイル情報（Stream</param>
        /// <param name="JsonButtonStatus">ボタンステータス(JSON文字列)</param>
        /// <param name="JsonIndividual">個別実装用データ(JSON文字列)</param>
        //dynamic inParam;
        CommonProcParamIn inParam;

        /// <summary>
        /// 業務ロジックDLLコール
        ///  - OUTパラメータ
        /// </summary>
        /// <remarks>DLLファンクション第2引数</remarks>
        /// <param name="Status">ステータス</param>
        /// <param name="MsgId">メッセージ</param>
        /// <param name="LogNo">ログ問合せ番号</param>
        /// <param name="JsonResult">実行結果(JSON文字列)</param>
        /// <param name="OutputStream">ファイル情報（Stream）</param>
        /// <param name="JsonIndividual">個別実装用データ(JSON文字列)</param>
        //dynamic outParam;
        CommonProcParamOut outParam;

        /// <summary>
        /// 言語コード
        /// </summary>
        string LanguageId;
        #endregion

        #region === コンストラクタ ===
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogicIO(CommonProcData procData)
        {
            this.LanguageId = AppCommonObject.Config.AppSettings.LanguageIdDefault;

            //実行モードによる設定
            switch (AppCommonObject.Config.AppSettings.DeployMode)
            {
                case "test":    //メンテナンスモード
                    this.execMode = execModeDef.Test;
                    //DLLファイル名
                    this.dllFileName = dllName_Test;   //メンテナンスモード用DLL
                    break;
                case "prot":    //プロト作成モード
                    this.execMode = execModeDef.Proto;
                    //DLLファイル名
                    this.dllFileName = dllName_Proto;   //プロト用DLL
                    break;
                default:        //Releaseモード
                    this.execMode = execModeDef.Default;
                    //DLLファイル名
                    //(例)BusinessLogic_SR01720
                    //※DLLはPGMID単位
                    this.dllFileName = dllName_DefaultPrefix + procData.PgmId;  //アクション処理用DLL
                    break;
            }

            //INパラメータにセット
            //this.inParam = new ExpandoObject();
            this.inParam = new CommonProcParamIn();
            this.inParam.TerminalNo = procData.TerminalNo;          //端末IPアドレス
            this.inParam.UserId = procData.LoginUserId;             //登録者ID
            this.inParam.LanguageId = this.LanguageId;              //言語ID ※デフォルト値
            if (false == string.IsNullOrEmpty(procData.LanguageId))
            {
                this.inParam.LanguageId = procData.LanguageId;      //ﾕｰｻﾞｰの言語ID
            }
            this.inParam.AuthorityLevelId = procData.AuthorityLevelId;
            if (procData.BelongingInfo != null)
            {
                this.inParam.BelongingInfo = procData.BelongingInfo;
                if (procData.BelongingInfo != null)
                {
                    // ユーザ本務工場ID
                    this.inParam.FactoryId = procData.BelongingInfo.DutyFactoryId.ToString();
                }
            }
            this.inParam.ConductId = procData.ConductId;            //機能ID
            this.inParam.PgmId = procData.PgmId;                    //プログラムID
            this.inParam.FormNo = procData.FormNo;                  //Form番号
            this.inParam.CtrlId = procData.CtrlId;                  //起動処理名(コントロールID)
            this.inParam.ActionKbn = procData.ActionKbn;            //アクション区分
            this.inParam.TransActionDiv = procData.TransActionDiv;  //画面遷移アクション区分
            //this.inParam.RootPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;  //ルート物理パス
            this.inParam.RootPath = getRootPath();  //ルート物理パス
            this.inParam.Status = procData.ConfirmNo;               //ステータス(確認番号)

            this.inParam.GUID = procData.GUID;                      // GUID
            this.inParam.BrowserTabNo = procData.BrowserTabNo;      //ブラウザタブ識別番号
            //★インメモリ化対応 start
            this.inParam.CustomizeList = procData.CustomizeList;    // ユーザカスタマイズ情報
            this.inParam.ComboDataAcquiredFlg = procData.ComboDataAcquiredFlg;  // コンボボックスデータ取得済みフラグ
            //★インメモリ化対応 end

            //this.inParam.JsonCondition = String.Empty;              //実行条件(JSON文字列)※初期化
            //this.inParam.JsonPageInfo = String.Empty;               //ページ情報(JSON文字列)※初期化
            //this.inParam.JsonResult = String.Empty;                 //登録データ(JSON文字列)※初期化
            //this.inParam.InputStream = null;                        //ファイル情報（Stream）※初期化
            //this.inParam.JsonButtonStatus = String.Empty;           //ﾎﾞﾀﾝｽﾃｰﾀｽ(JSON文字列)※初期化
            //this.inParam.JsonIndividual = String.Empty;             //個別実装用データ(JSON文字列)※初期化

            this.inParam.ConditionList = new List<Dictionary<string, object>>();    //実行条件(JSON文字列)※初期化
            this.inParam.PageInfoList = new List<PageInfo>();                       //ページ情報(JSON文字列)※初期化
            this.inParam.ResultList = new List<Dictionary<string, object>>();       //登録データ(JSON文字列)※初期化
            this.inParam.InputStream = null;                                        //ファイル情報（Stream）※初期化
            this.inParam.ButtonStatusList = new List<Dictionary<string, object>>(); //ﾎﾞﾀﾝｽﾃｰﾀｽ(JSON文字列)※初期化
            this.inParam.Individual = new Dictionary<string, object>();             //個別実装用データ(JSON文字列)※初期化

            //OUTパラメータを初期化
            //this.outParam = new ExpandoObject();
            this.outParam = new CommonProcParamOut();

            //DLLファイルフォルダパス
            this.dllFolder = Path.Combine(this.inParam.RootPath,
                    AppCommonObject.Config.AppSettings.LogicDllPathRef);
        }
        private string getRootPath()
        {
            //2024.09 .NET8バージョンアップ対応 start
            //var path = PlatformServices.Default.Application.ApplicationBasePath;
            var path = AppContext.BaseDirectory;
            //2024.09 .NET8バージョンアップ対応 end
            var idx = path.LastIndexOf("bin");
            if (idx >= 0)
            {
                path = path.Substring(0, idx);
            }
            return path;
        }
        #endregion

        #region === Public処理 ===
        ///// <summary>
        ///// 業務ﾛｼﾞｯｸdllｺｰﾙ（※各種ｱｸｼｮﾝ用）
        ///// </summary>
        ///// <param name="procData">
        ///// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※必須項目:(*))
        ///// 　ConductId:機能ID(*)
        ///// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        ///// 　FormNo:画面NO(*)
        ///// 　CtrlId:ｺﾝﾄﾛｰﾙID
        ///// 　ConditionData(val1～100):条件ﾃﾞｰﾀ
        ///// 　ListData(val1～200):明細ﾃﾞｰﾀ一覧
        ///// </param>
        ///// <returns></returns>
        //public CommonProcReturn CallDllBusinessLogic(CommonProcData procData, out object retResults)
        //{

        //    retResults = null;
        //    var jsonOptions = new JsonSerializerOptions
        //    {
        //        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        //    };

        //    if (execModeDef.Default.Equals(this.execMode))
        //    {
        //        //※Releaseモードの場合

        //        //DLLファイル名
        //        //(例)BusinessLogic_SR01720
        //        //※DLLはPGMID単位
        //        this.dllFileName = dllName_DefaultPrefix + procData.PgmId;  //アクション処理用DLL
        //    }

        //    // 業務ロジックコールに必要な実行条件をINパラメータに設定
        //    // ※List<Dictionary<string,object>>
        //    //var serializer = new JavaScriptSerializer();

        //    //実行条件(JSON文字列)～条件ｴﾘｱ一覧の画面値
        //    // CTRLID：一覧CTRLID
        //    // FORMNO：画面NO
        //    // ROWNO：1～連番
        //    // VAL1～：画面値
        //    if (procData.ConditionData != null && procData.ConditionData.Count > 0) 
        //        this.inParam.JsonCondition = JsonSerializer.Serialize(procData.ConditionData, jsonOptions);

        //    //ページ情報(JSON文字列)～明細ｴﾘｱ一覧のページ情報
        //    //※一覧単位に1件
        //    // CTRLID：一覧CTRLID
        //    // FORMNO：画面NO
        //    // VAL1：先頭ページ番号
        //    // VAL2：MAX件数
        //    // VAL3：1ページ当たりの件数
        //    if (procData.ListDefines != null && procData.ListDefines.Count > 0) 
        //        this.inParam.JsonPageInfo = JsonSerializer.Serialize(procData.ListDefines, jsonOptions);

        //    //登録データ(JSON文字列)～明細ｴﾘｱ一覧の画面値
        //    // CTRLID：一覧CTRLID
        //    // FORMNO：画面NO
        //    // ROWNO：1～連番
        //    // ROWSTATUS：0(表示のみ行)、1(編集行)、2(新規行)、9(削除行)
        //    // UPDTAG：1(編集中)、3(登録済み)★未対応
        //    // VAL1～：画面値
        //    if (procData.ListData != null && procData.ListData.Count > 0) 
        //        this.inParam.JsonResult = JsonSerializer.Serialize(procData.ListData, jsonOptions);

        //    //ボタン定義(JSON文字列)～表示中画面NOのボタン権限情報
        //    // ctrlId：ボタンCTRLID
        //    // authKbn：権限区分
        //    if (procData.ButtonDefines != null && procData.ButtonDefines.Count > 0) 
        //        this.inParam.JsonButtonStatus = JsonSerializer.Serialize(procData.ButtonDefines, jsonOptions);

        //    //個別実装用ﾃﾞｰﾀ
        //    if (procData.ListIndividual != null && procData.ListIndividual.Count > 0) 
        //        this.inParam.JsonIndividual = JsonSerializer.Serialize(procData.ListIndividual, jsonOptions);

        //    switch (procData.ActionKbn)
        //    {
        //        case (short)LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComUpload:
        //            //※取込の場合(【共通 - 取込結果表示機能】取込データ取得)

        //            //ファイル情報（Stream）
        //            // Stream[]：ファイルリスト
        //            if (procData.UploadFile != null && procData.UploadFile.Length > 0)
        //            {
        //                this.inParam.InputStream = procData.UploadFile.ToArray();
        //            }

        //            break;
        //        default:
        //            break;
        //    }

        //    //業務ロジックDLLコール
        //    // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
        //    object results;
        //    CommonProcReturn returnInfo = callDllBusinessLogicIO(out results);

        //    //実行結果情報を受け取り
        //    switch (procData.ActionKbn)
        //    {
        //        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Report:     //EXCEL出力
        //            // -作成ファイル（Stream）
        //            retResults = results;
        //            break;
        //        default:
        //            // -実行結果リスト
        //            // ※List<Dictionary<string,object>>
        //            //
        //            // CTRLID：一覧CTRLID
        //            // FORMNO：画面NO
        //            // ROWNO：1～連番
        //            // ROWSTATUS：0(表示のみ行)、1(編集行)、2(新規行)、9(削除行)
        //            // UPDTAG：1(編集中)、3(登録済み)
        //            // VAL1～：画面値
        //            retResults = results;
        //            break;
        //    }

        //    //実行ステータスを返却
        //    return returnInfo;
        //}

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※各種ｱｸｼｮﾝ用）
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
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic(CommonProcData procData, out object retResults)
        {

            retResults = null;
            //2024.09 .NET8バージョンアップ対応 start
            // 未使用のため削除
            //var jsonOptions = new JsonSerializerOptions
            //{
            //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            //};
            //2024.09 .NET8バージョンアップ対応 end

            if (execModeDef.Default.Equals(this.execMode))
            {
                //※Releaseモードの場合

                //DLLファイル名
                //(例)BusinessLogic_SR01720
                //※DLLはPGMID単位
                this.dllFileName = dllName_DefaultPrefix + procData.PgmId;  //アクション処理用DLL
            }

            // 業務ロジックコールに必要な実行条件をINパラメータに設定
            // ※List<Dictionary<string,object>>
            //var serializer = new JavaScriptSerializer();

            // ユーザー権限レベルID
            this.inParam.AuthorityLevelId = procData.AuthorityLevelId;
            // ユーザ所属情報
            this.inParam.BelongingInfo = procData.BelongingInfo;
            // ユーザ本務工場ID
            if (procData.BelongingInfo != null)
            {
                this.inParam.FactoryId = procData.BelongingInfo.DutyFactoryId.ToString();
            }

            //実行条件(JSON文字列)～条件ｴﾘｱ一覧の画面値
            // CTRLID：一覧CTRLID
            // FORMNO：画面NO
            // ROWNO：1～連番
            // VAL1～：画面値
            if (procData.ConditionData != null && procData.ConditionData.Count > 0)
            {
                this.inParam.ConditionList = procData.ConditionData;
            }
            else
            {
                this.inParam.ConditionList = new List<Dictionary<string, object>>();
            }
            // 場所階層・職種機種の条件を設定
            var condition = new Dictionary<string, object>(){
                { "locationIdList", procData.LocationIdList },
                { "jobIdList", procData.JobIdList },
                { "FileDownloadInfo", procData.FileDownloadInfo }
            };
            this.inParam.ConditionList.Add(condition);

            // 場所階層構成IDリスト(帳票出力用)
            if (procData.LocationIdList != null && procData.LocationIdList.Count > 0)
            {
                this.inParam.ConditionSheetLocationList = procData.LocationIdList;
            }
            else
            {
                this.inParam.ConditionSheetLocationList = new List<int>();
            }

            // 職種機種構成IDリスト(帳票出力用)
            if (procData.JobIdList != null && procData.JobIdList.Count > 0)
            {
                this.inParam.ConditionSheetJobList = procData.JobIdList;
            }
            else
            {
                this.inParam.ConditionSheetNameList = new List<string>();
            }

            // 検索条件項目名リスト(帳票出力用)
            if (procData.ListConditionName != null && procData.ListConditionName.Count > 0)
            {
                this.inParam.ConditionSheetNameList = procData.ListConditionName;
            }
            else
            {
                this.inParam.ConditionSheetNameList = new List<string>();
            }

            // 検索条件設定値リスト(帳票出力用)
            if (procData.ListConditionValue != null && procData.ListConditionValue.Count > 0)
            {
                this.inParam.ConditionSheetValueList = procData.ListConditionValue;
            }
            else
            {
                this.inParam.ConditionSheetNameList = new List<string>();
            }

            //ページ情報(JSON文字列)～明細ｴﾘｱ一覧のページ情報
            //※一覧単位に1件
            // CTRLID：一覧CTRLID
            // FORMNO：画面NO
            // VAL1：先頭ページ番号
            // VAL2：MAX件数
            // VAL3：1ページ当たりの件数
            if (procData.ListDefines != null && procData.ListDefines.Count > 0)
                this.inParam.PageInfoList = procData.ListDefines.Select(x => new PageInfo(x)).ToList();

            //登録データ(JSON文字列)～明細ｴﾘｱ一覧の画面値
            // CTRLID：一覧CTRLID
            // FORMNO：画面NO
            // ROWNO：1～連番
            // ROWSTATUS：0(表示のみ行)、1(編集行)、2(新規行)、9(削除行)
            // UPDTAG：1(編集中)、3(登録済み)★未対応
            // VAL1～：画面値
            if (procData.ListData != null && procData.ListData.Count > 0)
                this.inParam.ResultList = procData.ListData;

            //ボタン定義(JSON文字列)～表示中画面NOのボタン権限情報
            // ctrlId：ボタンCTRLID
            // authKbn：権限区分
            if (procData.ButtonDefines != null && procData.ButtonDefines.Count > 0)
                this.inParam.ButtonStatusList = procData.ButtonDefines;

            //個別実装用ﾃﾞｰﾀ
            if (procData.ListIndividual != null && procData.ListIndividual.Count > 0)
                this.inParam.Individual = procData.ListIndividual;

            switch (procData.ActionKbn)
            {
                case (short)LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComUpload:
                case (short)LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ExcelPortUpload:
                    //※取込(【共通 - 取込結果表示機能】取込データ取得)、ExcelPortアップロードの場合

                    //ファイル情報（Stream）
                    // Stream[]：ファイルリスト
                    if (procData.UploadFile != null && procData.UploadFile.Length > 0)
                    {
                        this.inParam.InputStream = procData.UploadFile.ToArray();
                    }

                    break;
                default:
                    break;
            }

            // 翻訳工場IDリスト
            this.inParam.TransFactoryId = procData.TransFactoryId;

            //業務ロジックDLLコール
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            object results;
            CommonProcReturn returnInfo = callDllBusinessLogicIO(out results);

            //実行結果情報を受け取り
            switch (procData.ActionKbn)
            {
                case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Report:     //EXCEL出力
                    // -作成ファイル（Stream）
                    retResults = results;
                    break;
                default:
                    // -実行結果リスト
                    // ※List<Dictionary<string,object>>
                    //
                    // CTRLID：一覧CTRLID
                    // FORMNO：画面NO
                    // ROWNO：1～連番
                    // ROWSTATUS：0(表示のみ行)、1(編集行)、2(新規行)、9(削除行)
                    // UPDTAG：1(編集中)、3(登録済み)
                    // VAL1～：画面値
                    retResults = results;
                    break;
            }

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※ｺﾝﾎﾞﾃﾞｰﾀ取得用）
        /// </summary>
        /// <param name="id">SQLID</param>
        /// <param name="param">SQL実行引数</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_CtrlSql(string id, string param, string code, string input, string factoryId, bool reset, out object retResults)
        {
            retResults = null;

            //INパラメータ
            // - 実行条件(JSON文字列)
            //
            // CTRLSQLID：SQLID
            // CTRLSQLPARAM：SQL実行引数
            //
            var conditionList = new List<Dictionary<string, object>>();
            var condition = new Dictionary<string, object>(){
                { "CTRLSQLID", id },
                { "CTRLSQLPARAM", param },
                { "CTRLCODE", code },
                { "CTRLINPUT", input },
                { "FACTORYID", factoryId },
                { "RESET", reset },		//★インメモリ化対応
            };
            conditionList.Add(condition);

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：CTRLSQL
            object results = null;
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_CtrlSql, conditionList, out results);

            // -実行結果
            // ※List<Dictionary<string,object>>
            // 
            // VALUE1：コード
            // VALUE2：名称
            // ....
            retResults = results;

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※パスワード変更用用）
        /// </summary>
        /// <param name="id">SQLID</param>
        /// <param name="param">SQL実行引数</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_ChangePassword(CommonProcData procData)
        {
            //INパラメータ
            // - 実行条件(JSON文字列)
            //
            //userId: ユーザーID
            //userOldPassword: 現在のパスワード
            //userNewPassword1: 新しいパスワード
            //userNewPassword2: 新しいパスワード（確認）
            //

            // セッションのログインユーザIDを条件として使用する
            string userId = procData.LoginUserId;
            List<Dictionary<string, object>> conditions = new List<Dictionary<string, object>>();
            Dictionary<string, object> con = new Dictionary<string, object>();
            if (procData.ConditionData.Count > 0)
            {
                con = procData.ConditionData[0];
                List<string> keyList = new List<string>(con.Keys);
                foreach (string key in keyList)
                {
                    if (key.Equals("userId"))
                    {
                        // 
                        con[key] = userId;
                        break;
                    }
                }
                conditions.Add(con);
                procData.ConditionData = conditions;
            }

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：ChangePassword
            object results = null;
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_ChangePass, procData.ConditionData, out results);

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※ﾛｸﾞｲﾝ認証用）
        /// </summary>
        /// <param name="loginId">ﾛｸﾞｲﾝID</param>
        /// <param name="loginPassword">ﾛｸﾞｲﾝﾊﾟｽﾜｰﾄﾞ</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="isPasswordCheck">true：ﾊﾟｽﾜｰﾄﾞﾁｪｯｸする/false：しない</param>
        /// <param name="isNewLogin">true：新規ログイン/false：ログイン済み</param>
        /// <param name="isCheckOK">(OUT)true：認証OK/false：認証NG</param>
        /// <param name="retUserId">(OUT)ﾕｰｻﾞｰID</param>
        /// <param name="retUserName">(OUT)ﾕｰｻﾞｰ表示名</param>
        /// <param name="retLanguageId">(OUT)言語ID</param>
        /// <param name="retConductIds">(OUT)機能IDﾘｽﾄ</param>
        /// <param name="retGuid">(OUT)GUID</param>
        /// <param name="retFactoryIdList">(OUT)工場IDリスト</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_LoginCheck(string loginId, string loginPassword, string userId, bool isPasswordCheck, bool isNewLogin,
            out bool isCheckOK, out UserInfoDef userInfo, out List<string> retConductIds)
        {
            //初期化
            isCheckOK = false;
            retConductIds = new List<string>();
            userInfo = new UserInfoDef();

            //INパラメータ
            // - 登録者ID
            // - 新規ログインフラグ
            this.inParam.LoginId = loginId;
            this.inParam.UserId = userId;
            this.inParam.IsNewLogin = isNewLogin;

            //INパラメータ
            // - 実行条件(JSON文字列)
            //
            // userPassword：ﾊﾟｽﾜｰﾄﾞ
            // passwordCheckFlg：1(ﾊﾟｽﾜｰﾄﾞﾁｪｯｸする)、0(ﾊﾟｽﾜｰﾄﾞﾁｪｯｸしない)
            //
            var conditionList = new List<Dictionary<string, object>>();
            var condition = new Dictionary<string, object>(){
                { "userPassword", loginPassword },
                { "passwordCheckFlg", (isPasswordCheck) ? 1 : 0 },
            };
            conditionList.Add(condition);

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：LoginCheck
            object results = null;
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_LoginCheck, conditionList, out results);

            // -実行結果
            // ※Dictionary<string,object>
            // 
            // result
            // userName
            // conductIdList
            // ....
            if (results != null)
            {
                Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
                if (dictionaryResult.ContainsKey("Result"))
                {
                    IList<Dictionary<string, object>> dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;

                    if (dicResults.Count > 0)
                    {
                        IDictionary<string, object> dicResult = dicResults[0];

                        //認証結果
                        int checkOkFlg = 0;
                        if (dicResult.ContainsKey("result") && dicResult["result"] != null && int.TryParse(dicResult["result"].ToString(), out checkOkFlg))
                        {
                            if (1 == checkOkFlg)
                            {
                                isCheckOK = true;
                            }
                        }
                        //ﾕｰｻﾞｰID
                        if (dicResult.ContainsKey("userId") && dicResult["userId"] != null)
                        {
                            userInfo.UserId = dicResult["userId"].ToString();
                        }
                        //ﾛｸﾞｲﾝID
                        if (dicResult.ContainsKey("loginId") && dicResult["loginId"] != null)
                        {
                            userInfo.LoginId = dicResult["loginId"].ToString();
                        }
                        //ﾕｰｻﾞｰ表示名
                        if (dicResult.ContainsKey("userName") && dicResult["userName"] != null)
                        {
                            userInfo.UserName = dicResult["userName"].ToString();
                        }
                        //言語Id
                        if (dicResult.ContainsKey("languageId") && dicResult["languageId"] != null)
                        {
                            userInfo.LanguageId = dicResult["languageId"].ToString();
                        }

                        //権限レベルID
                        if (dicResult.ContainsKey("authorityLevelId") && dicResult["authorityLevelId"] != null)
                        {
                            userInfo.AuthorityLevelId = Convert.ToInt32(dicResult["authorityLevelId"]);
                        }
                        //機能IDﾘｽﾄ
                        if (dicResult.ContainsKey("conductIdList") && dicResult["conductIdList"] != null)
                        {
                            retConductIds = dicResult["conductIdList"] as List<string>;
                        }

                        //GUID
                        if (dicResult.ContainsKey("guid") && dicResult["guid"] != null)
                        {
                            userInfo.GUID = dicResult["guid"].ToString();
                        }

                        //工場IDﾘｽﾄ
                        if (dicResult.ContainsKey("factoryIdList") && dicResult["factoryIdList"] != null)
                        {
                            userInfo.FactoryIdList = dicResult["factoryIdList"] as List<int>;
                        }

                        // 所属情報
                        if (dicResult.ContainsKey("belongingInfo") && dicResult["belongingInfo"] != null)
                        {
                            userInfo.BelongingInfo = dicResult["belongingInfo"] as BelongingInfo;
                        }
                        //★インメモリ化対応 start
                        // ユーザカスタマイズ情報
                        if (dicResult.ContainsKey("customizeList") && dicResult["customizeList"] != null)
                        {
                            userInfo.CustomizeList = dicResult["customizeList"] as List<COM_LISTITEM_USER_CUSTOMIZE>;
                        }
                        //★インメモリ化対応 end
                    }
                }
                else
                {
                    if (!isNewLogin)
                    {
                        // ログイン済みの場合、チェックOKとする
                        isCheckOK = true;
                    }
                }
            }

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※ﾘｿｰｽ翻訳取得用）
        /// </summary>
        /// <param name="resourceIds">翻訳IDﾘｽﾄ</param>
        /// <param name="resourceNames">(OUT)翻訳名情報</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_GetResourcesValue(IList<string> resourceIds, out Dictionary<string, string> resourceNames)
        {
            //初期化
            resourceNames = new Dictionary<string, string>();

            //INパラメータ
            // - 実行条件(JSON文字列)
            //
            // name：翻訳IDﾘｽﾄ List<string>
            //
            var conditionList = new List<Dictionary<string, object>>();
            if (resourceIds != null && resourceIds.Count > 0)
            {
                var condition = new Dictionary<string, object>(){
                    { "name", resourceIds },
                };
                conditionList.Add(condition);
            }

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetResourcesValue
            object results = null;
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_GetResourcesValue, conditionList, out results);

            // -実行結果
            // ※Dictionary<string, string>
            // 
            // KEY：翻訳ID
            // VALUE：翻訳名
            //
            if (results != null)
            {
                Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
                if (dictionaryResult.ContainsKey("Result"))
                {
                    IList<Dictionary<string, object>> dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
                    if (dicResults.Count > 0)
                    {
                        foreach (var result in dicResults)
                        {
                            //翻訳名
                            foreach (var item in result)
                            {
                                var val = "";
                                if (item.Value != null)
                                {
                                    val = item.Value.ToString();
                                }
                                // ※翻訳ID単位に標準翻訳、工場個別翻訳の順にソートされているため、
                                // ※工場個別翻訳が存在する場合、そちらが採用される
                                resourceNames[item.Key] = val;
                            }
                        }

                    }
                }
            }

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※ログアウト用）
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="guid">GUID</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_Logout()
        {

            //INパラメータ
            // - 登録者ID
            // - GUID
            //⇒呼び元でprocDataの値をセット済

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：Logout
            object results = null;
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_Logout, null, out results);

            //実行ステータスを返却
            return returnInfo;
        }
        #endregion

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※場所階層リスト取得用）
        /// </summary>
        /// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        /// <param name="retStructureList">構成リスト</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_GetStructureList(CommonProcData procData, out Dictionary<string, List<CommonStructure>> retStructureList)
        {
            //初期化
            retStructureList = new Dictionary<string, List<CommonStructure>>();

            //INパラメータ
            // - ユーザID
            this.inParam.UserId = procData.LoginUserId;

            //INパラメータ
            // - 実行条件(JSON文字列)
            // structureGroupList：構成グループIDリスト
            // factoryIdList：工場IDリスト
            var conditionList = new List<Dictionary<string, object>>();
            var factoryIdList = new List<int>();
            var structureIdList = new List<int>();
            if (procData.FactoryIdList != null)
            {
                // 工場ID指定の場合
                factoryIdList.AddRange(procData.FactoryIdList);
            }
            if (procData.AuthorityLevelId != USER_CONSTANTS.AUTHORITY_LEVEL.Administrator && 
                !procData.StructureGroupList.Contains(STRUCTURE_CONSTANTS.STRUCTURE_GROUP.SpareLocation))
            {
                // システム管理者以外かつ予備品場所階層以外の場合
                // 所属場所階層IDを設定
                structureIdList.AddRange(procData.BelongingInfo.BelongingFactoryIdList);
                var locationInfoList = procData.BelongingInfo.LocationInfoList.Where(x => x.LayerNo > STRUCTURE_CONSTANTS.LAYER_NO.Factory).ToList();
                if (locationInfoList != null && locationInfoList.Count > 0)
                {
                    // 工場配下の場所階層指定有りの場合
                    foreach (var info in locationInfoList)
                    {
                        structureIdList.Add(info.StructureId);
                        var idx = structureIdList.IndexOf(info.FactoryId);
                        if (idx >= 0)
                        {
                            // 対象の構成IDから工場IDを削除
                            structureIdList.RemoveAt(idx);
                        }
                    }

                }
                // 所属職種IDを設定
                structureIdList.AddRange(procData.BelongingInfo.BelongingJobIdList);
            }

            var condition = new Dictionary<string, object>(){
                { "structureGroupList", procData.StructureGroupList },
                { "factoryIdList", procData.FactoryIdList != null ? procData.FactoryIdList : procData.BelongingInfo.BelongingFactoryIdList },
                { "structureIdList", structureIdList },
            };
            conditionList.Add(condition);

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetStructureList
            object results = null;
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_GetStructureList, conditionList, out results);

            // -実行結果
            if (results != null)
            {
                Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
                if (dictionaryResult.ContainsKey("Result"))
                {
                    var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
                    if (dicResults != null && dicResults.Count > 0)
                    {
                        var dicResult = dicResults[0];
                        //構成リスト
                        if (dicResult.ContainsKey("structureList") && dicResult["structureList"] != null)
                        {
                            // 一旦JSONのシリアライズ/デシリアライズを利用してフロント側用クラスへ変換する
                            //★2024/06/26 TMQ応急対応 SQL側でマージ処理実行 Mod start
                            //var json = JsonSerializer.Serialize(dicResult["structureList"]);
                            //var list = JsonSerializer.Deserialize<List<CommonStructure>>(json);
                            //2024.09 .NET8バージョンアップ対応 start
                            //var options = new JsonSerializerOptions
                            //{
                            //    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                            //};
                            var options = JsonSerializerOptionsDefine.JsOptionsForNull;
                            //2024.09 .NET8バージョンアップ対応 end
                            var json = JsonSerializer.Serialize(dicResult["structureList"], options);
                            var list = JsonSerializer.Deserialize<List<CommonStructure>>(json, options);
                            //★2024/06/26 TMQ応急対応 SQL側でマージ処理実行 Mod end
                            // 構成グループ毎に結果辞書に格納
                            foreach (var grpId in procData.StructureGroupList)
                            {
                                var listPerGrp = list.Where(x => x.StructureGroupId.Value == grpId).ToList();
                                if (grpId == STRUCTURE_CONSTANTS.STRUCTURE_GROUP.SpareLocation)
                                {
                                    // 予備品場所階層の場合、場所階層のデータも同一結果に含める
                                    listPerGrp.AddRange(list.Where(x => x.StructureGroupId == STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Location).ToList());
                                    retStructureList.Add(grpId.ToString(), listPerGrp);
                                }
                                //★2024/06/26 TMQ応急対応 SQL側でマージ処理実行 Mod start
                                //★2024/06/12 TMQ応急対応 C#側でマージ処理実行 ADD start
                                //else if (grpId == STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Job)
                                //{
                                //    // 職種機種の場合、マージ処理を実行する
                                //    var mergedList = mergeList(listPerGrp, 0);
                                //    retStructureList.Add(grpId.ToString(), mergedList);
                                //}
                                //★2024/06/12 TMQ応急対応 C#側でマージ処理実行 ADD end
                                //★2024/06/26 TMQ応急対応 SQL側でマージ処理実行 Mod start
                                else
                                {
                                    retStructureList.Add(grpId.ToString(), listPerGrp);
                                }
                            }
                        }
                    }
                }
            }

            //★2024/06/12 TMQ応急対応 C#側でマージ処理実行 ADD start
            List<CommonStructure> mergeList(List<CommonStructure> list, int layerNo)
            {
                var resultList = list;

                // 同一階層のデータを取得
                var sameLayerList = resultList.Where(x => x.StructureLayerNo == layerNo);
                if (!sameLayerList.Any()) { return resultList; }

                // 一階層下のデータを取得
                var lowerLayerList = resultList.Where(x => x.StructureLayerNo == layerNo + 1).ToList();

                // 同一parentでグループ化
                var groupByParentList = sameLayerList.GroupBy(x=>x.ParentStructureId).ToList();

                // 同一parent単位でマージ
                foreach(var groupByParent in groupByParentList)
                {
                    // 同一parentのリストを取得
                    var sameParentList = groupByParentList.Single(x => x.Key == groupByParent.Key).ToList();
                    // 同一翻訳でグループ化
                    var groupByTextList = sameParentList.GroupBy(x => x.TranslationText).ToList();
                    foreach (var groupByText in groupByTextList)
                    {
                        var sameTextList = groupByTextList.Single(x => x.Key == groupByText.Key).ToList();
                        if (sameTextList.Count <= 1) { continue; }

                        var structureId = 0;
                        for (int i = 0; i < sameTextList.Count; i++)
                        {
                            var item = sameTextList[i];
                            var idx = resultList.IndexOf(item);
                            if (i == 0)
                            {
                                // 先頭データに抽出した工場IDと構成IDの配列を設定
                                structureId = item.StructureId;
                                resultList[idx].FactoryIdList = sameTextList.Select(x => x.FactoryId.Value).ToArray();
                                resultList[idx].StructureIdList = sameTextList.Select(x => x.StructureId).ToArray();
                                resultList[idx].FactoryId = null;
                            }
                            else
                            {
                                // 1階層下のデータから親構成IDと構成IDが一致するデータを抽出
                                if (lowerLayerList.Any())
                                {
                                    var lowerItems = lowerLayerList.Where(x => x.ParentStructureId == item.StructureId).ToList();
                                    foreach (var lowerItem in lowerItems)
                                    {
                                        var lowerIdx = resultList.IndexOf(lowerItem);
                                        // 先頭データの構成IDで更新
                                        resultList[lowerIdx].ParentStructureId = structureId;
                                    }
                                }
                                // マージ後リストから削除
                                resultList.RemoveAt(idx);
                            }
                        }
                    }
                }
                // 再帰的に下位の階層のマージ処理を呼び出す
                return mergeList(resultList, layerNo + 1);
            }
            //★2024/06/12 TMQ応急対応 C#側でマージ処理実行 ADD end

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procData"></param>
        /// <param name="comboValuesRowsPerPage"></param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_GetComboRowsPerPage(CommonProcData procData, out List<int> comboValuesRowsPerPage)
        {
            // 初期化
            comboValuesRowsPerPage = new();

            // INパラメータ
            this.inParam.UserId = procData.LoginUserId;

            //INパラメータ
            // - 実行条件(JSON文字列)
            // なし

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetComboRowsPerPage
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_GetComboRowsPerPage, new(), out object results);

            // 実行結果より戻り値を取得
            if (results == null)
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }
            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                var dicResult = dicResults[0];
                // コンボの内容をリストで取得できる場合、戻り値に設定する
                if (dicResult.ContainsKey("ComboRows") && dicResult["ComboRows"] != null)
                {
                    comboValuesRowsPerPage = dicResult["ComboRows"] as List<int>;
                }
            }

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※項目カスタマイズ情報登録用）
        /// </summary>
        /// <param name="procData"></param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_SaveCustomizeListInfo(CommonProcData procData)
        {
            // INパラメータ
            this.inParam.UserId = procData.LoginUserId;

            //INパラメータ
            // - 実行条件(JSON文字列)
            // factoryIdList：工場IDリスト
            // customizeList：項目カスタマイズ情報リスト
            var conditionList = new List<Dictionary<string, object>>();
            // 工場IDの指定は行わない
            //var factoryIdList = new List<int>();
            //if (procData.FactoryIdList != null && procData.FactoryIdList.Count > 0)
            //{
            //    // 工場指定有りの場合
            //    factoryIdList = procData.FactoryIdList;
            //}
            //else
            //{
            //    // 工場指定なしの場合、すべての所属工場を指定
            //    factoryIdList = procData.BelongingInfo.BelongingFactoryIdList;
            //}
            //if (!factoryIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
            //{
            //    factoryIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
            //}

            var condition = new Dictionary<string, object>(){
                //{ "factoryIdList", factoryIdList },
                { "customizeList", CommonDefinitions.CommonUtil.JsonElementDictionaryListToObjectDictionaryList(procData.ConditionData) }
            };
            conditionList.Add(condition);

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：SaveCustomizeListInfo
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_SaveCustomizeListInfo, conditionList, out object results);

            //★インメモリ化対応 start
            // 実行結果より戻り値を取得
            if (results == null)
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }
            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                var dicResult = dicResults[0];
                // ユーザカスタマイズ情報をリストで取得できる場合、戻り値に設定する
                var key = nameof(COM_LISTITEM_USER_CUSTOMIZE);
                if (dicResult.ContainsKey(key) && dicResult[key] != null)
                {
                    procData.CustomizeList = dicResult[key] as List<COM_LISTITEM_USER_CUSTOMIZE>;
                }
            }
            //★インメモリ化対応 end

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※言語リスト取得用）
        /// </summary>
        /// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        /// <param name="comboLanguageItemList">言語リスト</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_GetLanguageItemList(CommonProcData procData, out List<LanguageInfo> comboLanguageItemList)
        {
            // 初期化
            comboLanguageItemList = new();

            // INパラメータ
            this.inParam.UserId = procData.LoginUserId;

            //INパラメータ
            // - 実行条件(JSON文字列)
            // なし

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetLanguageItemList
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_GetLanguageItemList, new(), out object results);

            // 実行結果より戻り値を取得
            if (results == null)
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }
            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                var dicResult = dicResults[0];
                // コンボの内容をリストで取得できる場合、戻り値に設定する
                if (dicResult.ContainsKey("LanguageItemList") && dicResult["LanguageItemList"] != null)
                {
                    // 一旦JSONのシリアライズ/デシリアライズを利用してフロント側用クラスへ変換する
                    var json = JsonSerializer.Serialize(dicResult["LanguageItemList"]);
                    comboLanguageItemList = JsonSerializer.Deserialize<List<LanguageInfo>>(json);
                }
            }

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※ﾕｰｻﾞｰ権限機能IDリスト取得用）
        /// </summary>
        /// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        /// <param name="comboLanguageItemList">言語リスト</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_GetUserConductAuthorityList(CommonProcData procData, out List<string> conductIdList)
        {
            // 初期化
            conductIdList = new();

            // INパラメータ
            this.inParam.UserId = procData.LoginUserId;
            this.inParam.AuthorityLevelId = procData.AuthorityLevelId;

            //INパラメータ
            // - 実行条件(JSON文字列)
            // なし

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetLanguageItemList
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_GetUserConductAuthorityList, new(), out object results);

            // 実行結果より戻り値を取得
            if (results == null)
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }
            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                var dicResult = dicResults[0];
                // 機能IDリストが取得できる場合、戻り値に設定する
                if (dicResult.ContainsKey("ConductIdList") && dicResult["ConductIdList"] != null)
                {
                    conductIdList = dicResult["ConductIdList"] as List<string>;
                }
            }

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※ﾕｰｻﾞｰ機能権限有無取得用）
        /// </summary>
        /// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        /// <param name="hasAuthority">機能権限有無</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_CheckUserConductAuthority(CommonProcData procData, ref bool hasAuthority)
        {
            // 初期化
            hasAuthority = false;

            // INパラメータ
            this.inParam.UserId = procData.LoginUserId;
            this.inParam.AuthorityLevelId = procData.AuthorityLevelId;
            this.inParam.BelongingInfo = procData.BelongingInfo;
            this.inParam.ConductId = procData.ConductId;

            //INパラメータ
            // - 実行条件(JSON文字列)
            // なし

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetLanguageItemList
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_CheckConductAuthority, new(), out object results);

            // 実行結果より戻り値を取得
            if (results == null)
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }
            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                var dicResult = dicResults[0];
                // 機能IDリストが取得できる場合、戻り値に設定する
                if (dicResult.ContainsKey("HasAuthority") && dicResult["HasAuthority"] != null)
                {
                    hasAuthority = Convert.ToBoolean(dicResult["HasAuthority"]);
                }
            }

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※テンポラリフォルダパス取得用）
        /// </summary>
        /// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        /// <param name="tempFolderPath">テンポラリフォルダパス</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_GetTemporaryFolderPath(CommonProcData procData, out string tempFolderPath)
        {
            // 初期化
            tempFolderPath = "";

            // 初期化
            List<TempFolderPathInfo> tmpFolderPathList = new();

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetTemporaryFolderPath
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_GetTemporaryFolderPath, new(), out object results);

            // 実行結果より戻り値を取得
            if (results == null)
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }
            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                var dicResult = dicResults[0];
                // テンポラリフォルダパスをリストで取得できる場合、戻り値に設定する
                if (dicResult.ContainsKey("TemporaryFolderPath") && dicResult["TemporaryFolderPath"] != null)
                {
                    // 一旦JSONのシリアライズ/デシリアライズを利用してフロント側用クラスへ変換する
                    var json = JsonSerializer.Serialize(dicResult["TemporaryFolderPath"]);
                    tmpFolderPathList = JsonSerializer.Deserialize<List<TempFolderPathInfo>>(json);
                }
            }

            // 先頭１件からテンポラリフォルダパスを取得し、戻り値にセット
            tempFolderPath = tmpFolderPathList[0].TempFolderPath.ToString();

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※画像表示情報取得用）
        /// </summary>
        /// <param name="procData"></param>
        /// <param name="fileInfo">認証するファイルの情報</param>
        /// <param name="filePath">out 出力するファイルのファイルパス</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_GetImageFileInfo(CommonProcData procData, string fileInfo,out string filePath)
        {
            filePath = string.Empty;

            // INパラメータ
            this.inParam.UserId = procData.LoginUserId;

            //INパラメータ
            // - 実行条件(JSON文字列)
            // fileInfo：認証するファイル情報
            var conditionList = new List<Dictionary<string, object>>();
            var condition = new Dictionary<string, object>(){
                { "ImageFileInfo", fileInfo }
            };
            conditionList.Add(condition);

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetImageFileInfo
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_GetImageFileInfo, conditionList, out object results);

            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                var dicResult = dicResults[0];
                // 画像ファイルパスをリストで取得できる場合、戻り値に設定する
                if (dicResult.ContainsKey("ImageFilePath") && dicResult["ImageFilePath"] != null)
                {
                    filePath = dicResult["ImageFilePath"].ToString();
                }
            }

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※復号化データ取得用）
        /// </summary>
        /// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        /// <param name="encryptedData">暗号化データ</param>
        /// <param name="decryptedData">復号化データ</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_GetDecryptedData(CommonProcData procData, string encryptedData, out string decryptedData)
        {
            // 初期化
            decryptedData = "";

            //INパラメータ
            // - 実行条件(JSON文字列)
            // encryptedData：暗号化データ
            var conditionList = new List<Dictionary<string, object>>();
            var condition = new Dictionary<string, object>(){
                { "EncryptedData", encryptedData }
            };
            conditionList.Add(condition);

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetDecryptedData
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_GetDecryptedData, conditionList, out object results);

            // 実行結果より戻り値を取得
            if (results == null)
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }
            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                var dicResult = dicResults[0];
                // 復号化データが取得できる場合、戻り値に設定する
                if (dicResult.ContainsKey("DecryptedData") && dicResult["DecryptedData"] != null)
                {
                    decryptedData = dicResult["DecryptedData"].ToString();
                }
            }

            //実行ステータスを返却
            return returnInfo;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※ユーザID取得用）
        /// </summary>
        /// <param name="procData">業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ</param>
        /// <param name="mailAdress">メールアドレス</param>
        /// <param name="userId">ユーザID</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_GetUserIdByMailAdress(CommonProcData procData, string mailAdress, out List<int> userIdList)
        {
            // 初期化
            userIdList = new List<int>();

            //INパラメータ
            // - 実行条件(JSON文字列)
            // encryptedData：暗号化データ
            var conditionList = new List<Dictionary<string, object>>();
            var condition = new Dictionary<string, object>(){
                { "MailAdress", mailAdress }
            };
            conditionList.Add(condition);

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetUserIdByMailAdress
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_GetUserIdByMailAdress, conditionList, out object results);

            // 実行結果より戻り値を取得
            if (results == null)
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }
            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                var dicResult = dicResults[0];
                // ユーザIDが取得できる場合、戻り値に設定する
                if (dicResult.ContainsKey("UserIdList") && dicResult["UserIdList"] != null)
                {
                    userIdList = (List<int>)dicResult["UserIdList"];
                }
            }

            //実行ステータスを返却
            return returnInfo;
        }

        //★インメモリ化対応 start
        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※共通定義データ取得用）
        /// </summary>
        /// <param name="defines">(OUT)翻訳名情報</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_GetCommonDefineInfo(out Dictionary<string, object> defines)
        {
            //初期化
            defines = new Dictionary<string, object>();

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：GetCommonDefineInfo
            object results = null;
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_GetCommonDefineInfo, null, out results);

            // 実行結果より戻り値を取得
            if (results == null)
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }
            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            // -実行結果
            // ※Dictionary<string, object>
            // 
            // KEY：データクラス名
            // VALUE：共通定義データ
            //
            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                defines = dicResults[0];
            }

            //実行ステータスを返却
            return returnInfo;
        }
        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※共有メモリコンボボックスデータ更新用）
        /// </summary>
        /// <param name="defines">(OUT)翻訳名情報</param>
        /// <returns></returns>
        public CommonProcReturn CallDllBusinessLogic_UpdateComboBoxData(CommonProcData procData, out Dictionary<string, object> defines)
        {
            //初期化
            defines = new Dictionary<string, object>();

            //業務ロジックDLLコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：UpdateComboBoxData
            object results = null;
            CommonProcReturn returnInfo = callDllBusinessLogic_Common(dllProcName_UpdateComboBoxData, procData.ConditionData, out results);

            // 実行結果より戻り値を取得
            if (results == null)
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }
            Dictionary<string, object> dictionaryResult = results as Dictionary<string, object>;
            if (!dictionaryResult.ContainsKey("Result"))
            {
                // 実行結果が存在しない場合処理終了、実行ステータスを返却
                // ネストを浅くするためreturn
                return returnInfo;
            }

            // -実行結果
            // ※Dictionary<string, object>
            // 
            // KEY：データクラス名
            // VALUE：共通定義データ
            //
            var dicResults = dictionaryResult["Result"] as List<Dictionary<string, object>>;
            if (dicResults != null && dicResults.Count > 0)
            {
                defines = dicResults[0];
            }

            //実行ステータスを返却
            return returnInfo;
        }
        //★インメモリ化対応 end

        #region === protected処理 ===
        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ（※共通処理用）
        /// </summary>
        /// <param name="procName">起動処理名</param>
        /// <param name="conditionList">実行条件</param>
        /// <param name="resultJosnStr">(OUT)実行結果（Dictionaryリスト）</param>
        /// <returns></returns>
        /// <remarks>BusinessLogic_Common.dllコールをサポートする処理</remarks>
        protected CommonProcReturn callDllBusinessLogic_Common(string procName, List<Dictionary<string, object>> conditionList, out object results)
        {

            results = null;

            if (execModeDef.Default.Equals(this.execMode))
            {
                //※Releaseモードの場合

                //DLLファイル名
                // 【固定】BusinessLogic_Common.dll
                this.dllFileName = dllName_Common;  //共通処理用DLL
            }

            //INパラメータに設定
            // - 起動処理名
            this.inParam.CtrlId = procName;
            // - 実行条件(JSON文字列)
            if (conditionList != null && conditionList.Count > 0)
            {
                //var jsonOptions = new JsonSerializerOptions
                //{
                //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                //};
                //this.inParam.JsonCondition = JsonSerializer.Serialize(conditionList, jsonOptions);
                this.inParam.ConditionList = conditionList;
            }

            //業務ロジックDLLコール
            // - 関数名：CTRLSQL
            CommonProcReturn returnInfo = callDllBusinessLogicIO(out results);

            //実行ステータスを返却
            return returnInfo;
        }
        #endregion

        #region === Private処理 ===
        /// <summary>
        /// 業務ﾛｼﾞｯｸdllｺｰﾙ(※呼び出し共通処理)
        /// </summary>
        /// <param name="retResult">実行結果（Dictionaryリスト）、または作成ファイル（Stream）</param>
        /// <returns></returns>
        public CommonProcReturn callDllBusinessLogicIO(out object retResult)
        {

            CommonProcReturn returnInfo = new CommonProcReturn();
            retResult = null;

            //(例)[アプリフォルダ]\Logic\BusinessLogic_SR01720.dll
            string dllPath = Path.Combine(this.dllFolder, this.dllFileName + ".dll");

            dynamic logic = null;

            // アセンブリ名定義
            Assembly asm = null;
            try
            {
                //実行モードによる設定
                switch (AppCommonObject.Config.AppSettings.DeployMode)
                {
                    case "test":   // 開発モード
                        try
                        {
                            // 業務ロジッククラスの生成
                            asm = Assembly.Load(this.dllFileName);
                        }
                        catch
                        {
                            // 業務ロジックDLLのロード ※命名規則：業務ロジッククラス名 = DLL名
                            asm = Assembly.Load(System.IO.File.ReadAllBytes(dllPath));
                        }
                        break;
                    case "debug":   // 開発モード
                        // 業務ロジッククラスの生成
                        asm = Assembly.Load(this.dllFileName);
                        break;
                    case "prot":    // プロトモード
                        // 業務ロジッククラスの生成
                        asm = Assembly.Load(this.dllFileName);
                        break;
                    default:        // Releaseモード
                        // 業務ロジックDLLのロード ※命名規則：業務ロジッククラス名 = DLL名
                        asm = Assembly.Load(System.IO.File.ReadAllBytes(dllPath));
                        break;
                }

                if (asm != null)
                {
                    var t = asm.GetType(this.dllFileName + "." + this.dllFileName, true);
                    logic = Activator.CreateInstance(t);
                }

                if (logic != null)
                {
                    // 業務ロジックの実行
                    var result = logic.ExecuteBusinessLogic(this.inParam, out this.outParam);

                    //戻り値の設定
                    // -実行ステータス
                    returnInfo.STATUS = this.outParam.Status;   //ステータス
                    returnInfo.MESSAGE = this.outParam.MsgId;   //メッセージ
                    returnInfo.LOGNO = this.outParam.LogNo;     //ログ問合せ番号
                    returnInfo.UpdateUserInfo = this.outParam.UpdateUserInfo;   //ユーザ情報更新有無

                    // -実行結果を取得
                    //short kbn = -1;
                    //if (((IDictionary<string, object>)this.inParam).ContainsKey("ActionKbn"))
                    //{
                    //    kbn = this.inParam.ActionKbn;
                    //}
                    short kbn = this.inParam.ActionKbn;

                    // Excel出力時も実行結果を取得する
                    //switch (kbn)
                    //{
                    //    case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Report:    //EXCEL出力
                    //        // -作成ファイル（Stream）
                    //        if (((IDictionary<string, object>)this.outParam).ContainsKey("OutputStream"))
                    //        {
                    //            Dictionary<string, object> retResult2 = new Dictionary<string, object>();  // 戻り値セット用
                    //            // ファイル種類、ファイル名
                    //            if (((IDictionary<string, object>)this.outParam).ContainsKey("FileType") && ((IDictionary<string, object>)this.outParam).ContainsKey("FileName"))
                    //            {
                    //                // ファイル(Stream)、ファイル種類、ファイル名を戻り値にセット
                    //                retResult2.Add("OutputStream", this.outParam.OutputStream);
                    //                retResult2.Add("fileType", this.outParam.FileType);
                    //                retResult2.Add("fileName", this.outParam.FileName);
                    //                retResult = retResult2;
                    //            }
                    //        }
                    //        break;
                    //    default:
                    //        Dictionary<string, object> retResultW = new Dictionary<string, object>();  // 戻り値セット用

                    //        // -実行結果(JSON文字列)
                    //        if (((IDictionary<string, object>)this.outParam).ContainsKey("JsonResult"))
                    //        {
                    //            // ※List<Dictionary<string,object>>でクライアントに戻す
                    //            string jsonStr = String.Empty;
                    //            if (this.outParam.JsonResult != null)
                    //            {
                    //                jsonStr = this.outParam.JsonResult;
                    //                if (!string.IsNullOrEmpty(jsonStr))
                    //                {
                    //                    List<Dictionary<string, object>> rResult = new List<Dictionary<string, object>>();
                    //                    var serializer = new JavaScriptSerializer();
                    //                    rResult = serializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
                    //                    retResultW.Add("Result", rResult);
                    //                }
                    //            }
                    //        }

                    //        // -個別実装用汎用項目(JSON文字列)
                    //        if (((IDictionary<string, object>)this.outParam).ContainsKey("JsonIndividual")) {
                    //            // ※Dictionary<string,object>でクライアントに戻す
                    //            string jsonStr = String.Empty;
                    //            if (this.outParam.JsonIndividual != null)
                    //            {
                    //                jsonStr = this.outParam.JsonIndividual;
                    //                if (!string.IsNullOrEmpty(jsonStr))
                    //                {
                    //                    Dictionary<string, object> rIndividual = new Dictionary<string, object>();
                    //                    var serializer = new JavaScriptSerializer();
                    //                    rIndividual = serializer.Deserialize<Dictionary<string, object>>(jsonStr);
                    //                    retResultW.Add("Individual", rIndividual);
                    //                }
                    //            }
                    //        }

                    //        // -ﾌﾟｯｼｭ通知(JSON文字列)
                    //        if (((IDictionary<string, object>)this.outParam).ContainsKey("JsonPushTarget"))
                    //        {
                    //            // ※Dictionary<string, int>にしてﾌﾟｯｼｭ通知ﾒｿｯﾄﾞに渡す
                    //            string jsonStr = String.Empty;
                    //            if (this.outParam.JsonPushTarget != null)
                    //            {
                    //                jsonStr = this.outParam.JsonPushTarget;
                    //                if (!string.IsNullOrEmpty(jsonStr))
                    //                {
                    //                    var serializer = new JavaScriptSerializer();
                    //                    Dictionary<string, int> targets = serializer.Deserialize<Dictionary<string, int>>(jsonStr);
                    //                    //ﾌﾟｯｼｭ通知
                    //                    BusinessLogicUtil blogic = new BusinessLogicUtil();
                    //                    blogic.callPush(targets);
                    //                }
                    //            }
                    //        }

                    //        retResult = retResultW;
                    //        break;
                    //}

                    Dictionary<string, object> retResultW = new Dictionary<string, object>();  // 戻り値セット用

                    if (kbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Report || 
                        kbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ExcelPortDownload || 
                        kbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ExcelPortUpload)
                    {
                        // -作成ファイル（Stream）
                        //if (((IDictionary<string, object>)this.outParam).ContainsKey("OutputStream"))
                        //{                                                                                                       // ファイル種類、ファイル名
                        //    if (((IDictionary<string, object>)this.outParam).ContainsKey("FileType") && ((IDictionary<string, object>)this.outParam).ContainsKey("FileName"))
                        //    {
                        //        // ファイル(Stream)、ファイル種類、ファイル名を戻り値にセット
                        //        retResultW.Add("OutputStream", this.outParam.OutputStream);
                        //        retResultW.Add("fileType", this.outParam.FileType);
                        //        retResultW.Add("fileName", this.outParam.FileName);
                        //    }
                        //}
                        if (this.outParam.OutputStream != null)
                        {                                                                                                       // ファイル種類、ファイル名
                            if (!string.IsNullOrEmpty(this.outParam.FileType) && !string.IsNullOrEmpty(this.outParam.FileName))
                            {
                                // ファイル(Stream)、ファイル種類、ファイル名を戻り値にセット
                                retResultW.Add("OutputStream", this.outParam.OutputStream);
                                retResultW.Add("fileType", this.outParam.FileType);
                                retResultW.Add("fileName", this.outParam.FileName);
                            }
                        }
                    }

                    // -実行結果(JSON文字列)
                    //if (((IDictionary<string, object>)this.outParam).ContainsKey("JsonResult"))
                    //{
                    //    // ※List<Dictionary<string,object>>でクライアントに戻す
                    //    string jsonStr = String.Empty;
                    //    if (this.outParam.JsonResult != null)
                    //    {
                    //        jsonStr = this.outParam.JsonResult;
                    //        if (!string.IsNullOrEmpty(jsonStr))
                    //        {
                    //            List<Dictionary<string, object>> rResult = new List<Dictionary<string, object>>();
                    //            rResult = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
                    //            retResultW.Add("Result", rResult);
                    //        }
                    //    }
                    //}
                    if (this.outParam.ResultList != null && this.outParam.ResultList.Count > 0)
                    {
                        retResultW.Add("Result", this.outParam.ResultList);
                    }

                    /* ボタン権限制御 切替 start ================================================ */
                    // -ﾎﾞﾀﾝ権限情報(JSON文字列)
                    //if (((IDictionary<string, object>)this.outParam).ContainsKey("JsonButtonStatus"))
                    //{
                    //    // ※List<Dictionary<string,object>>でクライアントに戻す
                    //    string jsonStr = String.Empty;
                    //    if (this.outParam.JsonButtonStatus != null)
                    //    {
                    //        jsonStr = this.outParam.JsonButtonStatus;
                    //        if (!string.IsNullOrEmpty(jsonStr))
                    //        {
                    //            List<Dictionary<string, object>> ButtonStatus = new List<Dictionary<string, object>>();
                    //            ButtonStatus = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonStr);
                    //            retResultW.Add("ButtonStatus", ButtonStatus);
                    //        }
                    //    }
                    //}
                    if (this.outParam.ButtonStatusList != null && this.outParam.ButtonStatusList.Count > 0)
                    {
                        retResultW.Add("ButtonStatus", this.outParam.ButtonStatusList);
                    }
                    /* ボタン権限制御 切替 end ================================================== */

                    // -個別実装用汎用項目(JSON文字列)
                    //if (((IDictionary<string, object>)this.outParam).ContainsKey("JsonIndividual"))
                    //{
                    //    // ※Dictionary<string,object>でクライアントに戻す
                    //    string jsonStr = String.Empty;
                    //    if (this.outParam.JsonIndividual != null)
                    //    {
                    //        jsonStr = this.outParam.JsonIndividual;
                    //        if (!string.IsNullOrEmpty(jsonStr))
                    //        {
                    //            Dictionary<string, object> rIndividual = new Dictionary<string, object>();
                    //            rIndividual = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonStr);
                    //            retResultW.Add("Individual", rIndividual);
                    //        }
                    //    }
                    //}
                    if (this.outParam.Individual != null && this.outParam.Individual.Count > 0)
                    {
                        retResultW.Add("Individual", this.outParam.Individual);
                    }

                    // -ﾌﾟｯｼｭ通知(JSON文字列)
                    //if (((IDictionary<string, object>)this.outParam).ContainsKey("JsonPushTarget"))
                    if (!string.IsNullOrEmpty(this.outParam.JsonPushTarget))
                    {
                        // ※Dictionary<string, int>にしてﾌﾟｯｼｭ通知ﾒｿｯﾄﾞに渡す
                        string jsonStr = String.Empty;
                        if (this.outParam.JsonPushTarget != null)
                        {
                            jsonStr = this.outParam.JsonPushTarget;
                            if (!string.IsNullOrEmpty(jsonStr))
                            {
                                Dictionary<string, int> targets = JsonSerializer.Deserialize<Dictionary<string, int>>(jsonStr);
                                //ﾌﾟｯｼｭ通知
                                //BusinessLogicUtil blogic = new BusinessLogicUtil();
                                //blogic.callPush(targets);
                            }
                        }
                    }

                    if (this.outParam.DefineTransList != null && this.outParam.DefineTransList.Count > 0)
                    {
                        retResultW.Add("DefineTransList", this.outParam.DefineTransList);
                    }

                    // コンボボックスアイテム(共有メモリから取得用)
                    if (this.outParam.ComboBoxMemoryInfo != null && this.outParam.ComboBoxMemoryInfo.Count > 0)
                    {
                        retResultW.Add("ComboBoxMemoryInfo", this.outParam.ComboBoxMemoryInfo);
                    }

                    retResult = retResultW;

                }
                else
                {
                    returnInfo.STATUS = CommonProcReturn.ProcStatus.Error;
                    returnInfo.MESSAGE = string.Format("<DLLに業務ロジッククラスが存在しません。{0}>", this.dllFileName);
                    throw new Exception(returnInfo.MESSAGE);
                }
            }
            catch (Exception ex)
            {
                returnInfo.STATUS = CommonProcReturn.ProcStatus.Error;
                if (execModeDef.Default.Equals(this.execMode))
                {
                    returnInfo.MESSAGE = string.Format("<DLLが存在しません。{0}>", dllPath);
                }
                else
                {
                    returnInfo.MESSAGE = string.Format("<DLLが存在しません。{0}>", this.dllFileName);
                }
                throw new Exception(returnInfo.MESSAGE, ex);
            }

            //実行ステータスを返却
            return returnInfo;
        }
        #endregion
    }
}