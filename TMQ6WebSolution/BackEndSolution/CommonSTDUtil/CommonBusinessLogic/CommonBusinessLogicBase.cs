using CommonSTDUtil.CommonSTDUtil;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using static CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComOpt = CommonSTDUtil.CommonSTDUtil.OptimisticExclusive;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using UpdTag = CommonWebTemplate.Models.Common.TMPTBL_CONSTANTS.UPDTAG;

namespace CommonSTDUtil.CommonBusinessLogic
{
    public abstract partial class CommonBusinessLogicBase : MarshalByRefObject
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL名：マッピング情報一覧取得</summary>
            public const string GetMappingInfoList = "MappingInfo_GetList";
            /// <summary>DBカラム情報用SQL</summary>
            public const string GetColumnInfo = "ColumnInfo_GetList";
            /// <summary>一時テーブル件数取得用SQL</summary>
            public const string GetTmpDataCnt = "TempData_GetCount";
            /// <summary>一時テーブルデータ取得用SQL</summary>
            public const string GetTmpDataList = "TempData_GetList";
            /// <summary>ボタンコントロールID判定用SQL</summary>
            public const string CheckBtnCtrlId = "CheckBtnControlid";
            /// <summary>配下の構成IDの取得用SQL</summary>
            public const string GetStructureLowerList = "Structure_GetLowerList";
            /// <summary>予備品の棚の取得用SQL</summary>
            public const string GetPartsLocationList = "Structure_GetPartsLocationList";
            /// <summary>場所階層IDを保存する一時テーブル作成用SQL</summary>
            public const string CreateTableTempLocation = "CreateTableTempLocation";
            /// <summary>場所階層IDを保存する一時テーブルへの登録用SQL</summary>
            public const string InsertTempLocation = "InsertTempLocation";
            /// <summary>職種機種IDを保存する一時テーブル作成用SQL</summary>
            public const string CreateTableTempJob = "CreateTableTempJob";
            /// <summary>職種機種IDを保存する一時テーブルへの登録用SQL</summary>
            public const string InsertTempJob = "InsertTempJob";
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = "Common";
        }

        protected static class PageNo
        {
            /// <summary>ページNO ※先頭ページ</summary>
            public const string HeaderPage = "1";
        }

        /// <summary>
        /// 更新フラグの種類を列挙型で定義
        /// </summary>
        /// <remarks>convertUpdtagValueで変換</remarks>
        protected enum Updtag
        {
            /// <summary>編集中（入力値）</summary>
            Input,
            ///// <summary>選択中</summary>
            //Select,
            ///// <summary>登録済み</summary>
            //Update,
            /// <summary>一時テーブルへ登録済み</summary>
            UpdatedToTmp,
            /// <summary>登録済み</summary>
            Updated,
            /// <summary>指定なし</summary>
            None
        }

        protected static class CommonColumnName
        {
            /// <summary>場所階層構成ID</summary>
            public const string LocationId = "location_structure_id";
            /// <summary>職種機種構成ID</summary>
            public const string JobId = "job_structure_id";
            /// <summary>予備品管理工場ID</summary>
            public const string PartsLocationId = "parts_factory_id";
        }

        /// <summary>
        /// 場所階層ID、職種機種IDを保存する一時テーブル名
        /// </summary>
        protected static class TempTableName
        {
            /// <summary>場所階層IDを保存する一時テーブル</summary>
            public const string Location = "#temp_location";
            /// <summary>職種機種IDを保存する一時テーブル</summary>
            public const string Job = "#temp_job";

        }
        #endregion

        #region private変数
        /// <summary>ルート物理パス</summary>
        protected string rootPath;
        /// <summary>実行条件(Dictionary)</summary>
        protected List<Dictionary<string, object>> conditionDictionary;
        protected List<Dictionary<string, object>> searchConditionDictionary;
        protected List<Dictionary<string, object>> resultInfoDictionary;
        protected List<Dictionary<string, object>> buttonInfoDictionary;
        protected List<int> conditionSheetLocationList;
        protected List<int> conditionSheetJobList;
        protected List<string> conditionSheetNameList;
        protected List<string> conditionSheetValueList;
        /// <summary>ページ情報リスト</summary>
        protected List<PageInfo> pageInfoList;
        /// <summary>マッピング情報リスト</summary>
        protected IList<ComUtil.DBMappingInfo> mapInfoList;
        /// <summary>DB操作クラス</summary>
        protected CommonDBManager.CommonDBManager db;
        /// <summary>メッセージリソース</summary>
        protected ComUtil.MessageResources messageResources;
        /// <summary>グローバルリスト</summary>
        protected Dictionary<string, object> IndividualDictionary;
        /// <summary>ページ情報設定用</summary>
        protected Dictionary<string, object> outParamPageInfoList;
        /// <summary>実行結果リスト</summary>
        protected List<Dictionary<string, object>> ResultList;

        /// <summary>ログ出力</summary>
        protected static CommonLogger.CommonLogger logger = CommonLogger.CommonLogger.GetInstance("logger");
        #endregion

        #region コンストラクタ
        public CommonBusinessLogicBase()
        {
            this.db = new CommonDBManager.CommonDBManager();

            this.Status = CommonProcReturn.ProcStatus.Valid;
            this.MsgId = string.Empty;
            this.LogNo = string.Empty;
            this.JsonResult = string.Empty;
            this.JsonPushTarget = string.Empty;
            this.JsonIndividual = string.Empty;
            this.OutputStream = null;
            this.NeedsTotalCntCheck = true;
            this.ResultList = new List<Dictionary<string, object>>();
        }
        #endregion

        #region プロパティ
        /// <summary>端末IPアドレス</summary>
        public string TerminalNo { get; set; }
        /// <summary>機能ID</summary>
        public string ConductId { get; set; }
        /// <summary>プログラムID</summary>
        public string PgmId { get; set; }
        /// <summary>Form番号</summary>
        public short FormNo { get; set; }
        /// <summary>登録者ID</summary>
        public string UserId { get; set; }
        /// <summary>ユーザー権限レベルID</summary>
        public int AuthorityLevelId { get; set; }
        /// <summary>ユーザー所属情報</summary>
        public BelongingInfo BelongingInfo { get; set; }
        /// <summary>ユーザー本務工場ID</summary>
        public string FactoryId { get; set; }
        /// <summary>コントロールID</summary>
        public string CtrlId { get; set; }
        /// <summary>アクション区分</summary>
        public short ActionKbn { get; set; }
        /// <summary>画面遷移アクション区分</summary>
        public short TransActionDiv { get; set; }
        /// <summary>入力ファイル情報(HttpPostedFileBase)</summary>
        public IFormFile[] InputStream { get; protected set; }
        /// <summary>言語ID</summary>
        public string LanguageId { get; set; }
        /// <summary>実行結果(JSON文字列)</summary>
        public string JsonResult { get; protected set; }
        /// <summary>プッシュ通知結果(JSON文字列)</summary>
        public string JsonPushTarget { get; protected set; }
        /// <summary>グローバルリスト(JSON文字列)</summary>
        public string JsonIndividual { get; protected set; }
        /// <summary>GUID</summary>
        public string GUID { get; set; }
        /// <summary>ブラウザタブ識別番号</summary>
        public string BrowserTabNo { get; set; }

        /// <summary>ステータス</summary>
        protected int Status { get; set; }
        /// <summary>メッセージコード</summary>
        protected string MsgId { get; set; }
        /// <summary>ログ問合せ番号</summary>
        protected string LogNo { get; set; }
        /// <summary>出力ファイル情報(Stream)</summary>
        public Stream OutputStream { get; protected set; }
        /// <summary>出力ファイル種類(1:Excel/2:CSV/3:PDF)</summary>
        protected string OutputFileType { get; set; }
        /// <summary>出力ファイル名</summary>
        protected string OutputFileName { get; set; }
        /// <summary>検索総件数チェックが必要かどうか(true:必要/false:不要)</summary>
        protected bool NeedsTotalCntCheck { get; set; }
        #endregion

        #region publicメソッド
        public int ExecuteBusinessLogic(CommonProcParamIn inParam, out CommonProcParamOut outParam)
        {
            int result = 0;
            outParam = new CommonProcParamOut();

            try
            {
                // パラメータチェック
                if (!CheckParameters(inParam))
                {
                    // アクセスログ出力
                    writeAccessLog(inParam);

                    outParam.Status = this.Status;
                    outParam.MsgId = this.MsgId;
                    outParam.LogNo = this.LogNo;
                    return -1;
                }
                // アクセスログ出力
                writeAccessLog(inParam);

                // 検索条件取得
                if (inParam.ConditionList != null && inParam.ConditionList.Count > 0)
                {
                    this.searchConditionDictionary =
                        CommonUtil.JsonElementDictionaryListToObjectDictionaryList(inParam.ConditionList);
                }
                else
                {
                    this.searchConditionDictionary = new List<Dictionary<string, object>>();
                }
                // ページ情報取得
                outParamPageInfoList = new Dictionary<string, object>();
                if (inParam.PageInfoList != null)
                {
                    this.pageInfoList = inParam.PageInfoList;
                    // 存在する場合、ヘッダ設定用のページ情報リストに値を設定
                    foreach (var pageInfo in this.pageInfoList)
                    {
                        if (!outParamPageInfoList.ContainsKey(pageInfo.CtrlId))
                        {
                            outParamPageInfoList.Add(pageInfo.CtrlId, PageNo.HeaderPage); // AP2.0のデフォルトは先頭ページ
                        }

                    }
                }
                else
                {
                    this.pageInfoList = new List<PageInfo>();
                }
                // 登録情報取得
                if (inParam.ResultList != null && inParam.ResultList.Count > 0)
                {
                    this.resultInfoDictionary =
                        CommonUtil.JsonElementDictionaryListToObjectDictionaryList(inParam.ResultList);
                }
                else
                {
                    this.resultInfoDictionary = new List<Dictionary<string, object>>();
                }
                // グローバルリスト
                if (inParam.Individual != null && inParam.Individual.Count > 0)
                {
                    this.IndividualDictionary =
                        CommonUtil.JsonElementDictionaryToObjectDictionary(inParam.Individual);
                    outParam.Individual = inParam.Individual;
                }
                else
                {
                    this.IndividualDictionary = new Dictionary<string, object>();
                }

                // 場所階層構成IDリスト(帳票出力用)
                if (inParam.ConditionSheetLocationList != null && inParam.ConditionSheetLocationList.Count > 0)
                {
                    this.conditionSheetLocationList = inParam.ConditionSheetLocationList;
                }
                else
                {
                    this.conditionSheetLocationList = new List<int>();
                }

                // 職種機種構成IDリスト(帳票出力用)
                if (inParam.ConditionSheetJobList != null && inParam.ConditionSheetJobList.Count > 0)
                {
                    this.conditionSheetJobList = inParam.ConditionSheetJobList;
                }
                else
                {
                    this.conditionSheetJobList = new List<int>();
                }

                // 検索条件項目名リスト(帳票出力用)
                if (inParam.ConditionSheetNameList != null && inParam.ConditionSheetNameList.Count > 0)
                {
                    this.conditionSheetNameList = inParam.ConditionSheetNameList;
                }
                else
                {
                    this.conditionSheetNameList = new List<string>();
                }

                // 検索条件設定値リスト(帳票出力用)
                if (inParam.ConditionSheetValueList != null && inParam.ConditionSheetValueList.Count > 0)
                {
                    this.conditionSheetValueList = inParam.ConditionSheetValueList;
                }
                else
                {
                    this.conditionSheetValueList = new List<string>();
                }

                // ボタン情報
                if (inParam.ButtonStatusList != null && inParam.ButtonStatusList.Count > 0)
                {
                    this.buttonInfoDictionary =
                        CommonUtil.JsonElementDictionaryListToObjectDictionaryList(inParam.ButtonStatusList);
                }
                else
                {
                    this.buttonInfoDictionary = new List<Dictionary<string, object>>();
                }

                //// パラメータをデバッグログへ出力
                //string text = string.Format("登録者ID:{0}, GUID:{1}, 機能ID:{2}, Form番号:{3}, 処理名:{4}\n検索条件:{5}", this.UserId, this.GUID, this.ConductId, this.FormNo, this.CtrlId, JsonSerializer.Serialize(inParam.ConditionList));
                //logger.Debug(text);

                try
                {
                    // DB接続
                    this.db = new CommonDBManager.CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                    if (!this.db.Connect())
                    {
                        outParam.Status = CommonProcReturn.ProcStatus.Error;
                        outParam.LogNo = string.Empty;
                        // DB接続エラーログ＆メッセージ設定
                        setDBConnectionErrorLogAndMessage();
                        return -1;
                    }

                    // 項目定義からマッピング情報を取得
                    GetDBMappingList();

                    // メッセージリソースリストを取得
                    getMessageResources(this.LanguageId, new List<int> { this.BelongingInfo.DutyFactoryId });

                    // 各処理に分岐する前に、実行、削除、出力(同期、非同期)の場合、現状のページ情報を一時テーブルに保存する
                    if (this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Execute ||
                        this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Delete ||
                        this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Report ||
                        this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ReportHidoki
                        )
                    {
                        registTmpTable();
                    }

                    // ボタンコントロールの場合、実行権限が存在するか確認を行う
                    if (!this.CtrlId.Equals("CheckAuthority")) // 初期権限チェックの場合は処理を行わない
                    {
                        if (!this.ExecBtnActionCheck())
                        {
                            // 【CJ00000.W02】処理権限がありません。
                            outParam.Status = CommonProcReturn.ProcStatus.Error;
                            outParam.MsgId = GetResMessage("941120004");
                            writeErrorLog(string.Format("{0} [CtrlID:{1}]", outParam.MsgId, this.CtrlId));
                            return -1;
                        }
                    }

                    // アクション区分に応じて処理を振り分け
                    switch (this.ActionKbn)
                    {
                        // 初期値データ取得
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComInitForm:
                            result = Init();
                            break;
                        // データ取得、検索、ソート
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.DataGet:
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Search:
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ListSort:
                            result = Search();
                            break;
                        // 実行
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Execute:
                            result = Execute();
                            break;
                        // 削除
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Delete:
                            result = Delete();
                            break;
                        // ファイル出力
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Report:
                            result = Report();
                            break;
                        // ファイル取込
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComUpload:
                            result = Upload();
                            break;
                        // ExcelPort(ダウンロード)
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ExcelPortDownload:
                            result = ExcelPortDownload();
                            break;
                        // ExcelPort(アップロード)
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ExcelPortUpload:
                            result = ExcelPortUpload();
                            break;
                        // 共通処理
                        case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.None:
                            if (this.CtrlId.Equals("CheckAuthority"))
                            {
                                result = CheckAuthority();
                            }
                            else
                            {
                                Type type1 = this.GetType();
                                MethodInfo method1 = type1.GetMethod(this.CtrlId);
                                if (method1 != null)
                                {
                                    result = (int)method1.Invoke(this, new object[] { });
                                }
                                else
                                {
                                    outParam.Status = CommonProcReturn.ProcStatus.Error;
                                    // 「ビジネスロジックに指定された処理が存在しません。」
                                    outParam.MsgId = GetResMessage(new string[] { "941220004", "911270002", "911120003" });
                                    writeErrorLog(string.Format("{0} [CtrlID:{1}]", outParam.MsgId, this.CtrlId));
                                    return -1;
                                }
                            }
                            break;
                        // その他
                        default:
                            Type type = this.GetType();
                            MethodInfo method = type.GetMethod(this.CtrlId);
                            if (method != null)
                            {
                                result = (int)method.Invoke(this, new object[] { });
                            }
                            else
                            {
                                outParam.Status = CommonProcReturn.ProcStatus.Error;
                                // 「ビジネスロジックに指定された処理が存在しません。」
                                outParam.MsgId = GetResMessage(new string[] { "941220004", "911270002", "911120003" });
                                writeErrorLog(string.Format("{0} [CtrlID:{1}]", outParam.MsgId, this.CtrlId));
                                return -1;
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    outParam.Status = CommonProcReturn.ProcStatus.Error;
                    // 「処理の実行に失敗しました。」
                    outParam.MsgId = GetResMessage(new string[] { "941220002", "911120009" });
                    outParam.LogNo = string.Empty;

                    writeErrorLog(this.MsgId, ex);
                    return -1;
                }
                finally
                {
                    if (this.db != null)
                    {
                        // DB切断
                        this.db.Close();
                    }
                }

                outParam.Status = this.Status;
                outParam.MsgId = this.MsgId;
                outParam.LogNo = this.LogNo;
                outParam.ResultList = this.ResultList;

                var jsonOptions = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                };
                outParam.Individual = this.IndividualDictionary;
                outParam.ButtonStatusList = this.buttonInfoDictionary;

                outParam.JsonPushTarget = this.JsonPushTarget;
                outParam.OutputStream = this.OutputStream;
                outParam.FileType = this.OutputFileType;
                outParam.FileName = this.OutputFileName;

                //logger.Debug(string.Format("ResultJson : {0}", JsonSerializer.Serialize(this.ResultList)));

                return result;
            }
            catch (Exception ex)
            {
                outParam.Status = CommonProcReturn.ProcStatus.Error;
                // 「ビジネスロジックの実行の受け付けに失敗しました。」
                outParam.MsgId = GetResMessage(new string[] { "941220002", "911270003" });

                writeErrorLog(this.MsgId, ex);

                return -2;
            }
            //finally
            //{
            //    logger.Debug(string.Format("{0}, {1}, {2}, {3} end.", this.UserId, this.ConductId, this.PgmId, this.CtrlId));
            //}
        }
        /// <summary>
        /// 帳票用選択キーデータ取得
        /// </summary>
        /// <typeparam name="T">データクラス</typeparam>
        /// <param name="listId">一覧ID</param>
        /// <param name="keyInfo">キー情報</param>
        /// <param name="formDatas">画面の情報</param>
        /// <param name="selectFlag">選択フラグ</param>
        /// <returns>true:正常　false:エラー</returns>
        public List<SelectKeyData> getSelectKeyDataForReport<T>(
            string listId,
            Key keyInfo,
            List<Dictionary<string, object>> formDatas,
            bool selectFlag = true
            ) where T : new()
        {
            List<Dictionary<string, object>> selectedDics = new List<Dictionary<string, object>>();
            if (selectFlag == true)
            {
                // 画面情報から一覧からの選択行のみを取得
                selectedDics = getSelectedRowsByList(formDatas, listId);
            }
            else
            {
                // 選択行も含めて一覧の情報を取得
                selectedDics = ComUtil.GetDictionaryListByCtrlId(formDatas, listId);
            }

            // データクラスに変換した結果を格納
            List<T> dataList = new();

            // 選択行を繰り返し処理　データクラスに変換しリストへ追加
            foreach (var rowDic in selectedDics)
            {
                T rowData = new();
                SetDataClassFromDictionary(rowDic, listId, rowData);
                dataList.Add(rowData);
            }

            // 検索結果クラスのプロパティを列挙
            var properites = typeof(T).GetProperties();

            // 選択キーデータリスト初期化
            List<SelectKeyData> selectKeyDataList = new List<SelectKeyData>();
            // 選択データ分ループ
            foreach (var data in dataList)
            {
                SelectKeyData selectKeyData = new SelectKeyData();
                // キー１のプロパティを取得し、設定
                var prop1 = properites.FirstOrDefault(x => x.Name.Equals(keyInfo.Key1));
                if (prop1 != null)
                {
                    selectKeyData.Key1 = prop1.GetValue(data);
                }
                // キー２のプロパティを取得し、設定
                var prop2 = properites.FirstOrDefault(x => x.Name.Equals(keyInfo.Key2));
                if (prop2 != null)
                {
                    selectKeyData.Key2 = prop2.GetValue(data);
                }
                // キー３のプロパティを取得し、設定
                var prop3 = properites.FirstOrDefault(x => x.Name.Equals(keyInfo.Key3));
                if (prop3 != null)
                {
                    selectKeyData.Key3 = prop3.GetValue(data);
                }
                // 選択キーデータリストに追加
                selectKeyDataList.Add(selectKeyData);
            }
            // 選択キーデータリストを返却
            return selectKeyDataList;
        }
        /// <summary>
        /// 帳票用選択キーデータ取得
        /// 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
        /// </summary>
        /// <param name="listId">一覧ID</param>
        /// <param name="keyInfo">キー情報</param>
        /// <param name="formDatas">画面の情報</param>
        /// <param name="isTargetSelectFlg">選択データのみとするか（デフォルト：選択データのみ）</param>
        /// <returns>true:正常　false:エラー</returns>
        public List<SelectKeyData> getSelectKeyDataForReport(
            string listId,
            Key keyInfo,
            List<Dictionary<string, object>> formDatas,
            bool isTargetSelectFlg = true
        )
        {
            // 画面情報から一覧からの選択行のみを取得
            var selectedDics = getSelectedRowsByList(formDatas, listId);

            // 未選択行も含める場合
            if (isTargetSelectFlg == false)
            {
                selectedDics = ComUtil.GetDictionaryListByCtrlId(formDatas, listId);
            }

            // 選択キーデータリスト初期化
            List<SelectKeyData> selectKeyDataList = new List<SelectKeyData>();

            // 選択行を繰り返し処理　データクラスに変換しリストへ追加
            foreach (var rowDic in selectedDics)
            {
                SelectKeyData selectKeyData = new SelectKeyData();

                List<ComUtil.DBMappingInfo> mappingList;
                List<string> paramList = null;
                if (paramList == null || paramList.Count == 0)
                {
                    mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(listId)).ToList();
                }
                else
                {
                    mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(listId) && paramList.Contains(x.ParamName)).ToList();
                }

                // キー１の設定値を取得し、設定
                var mapInfo = mappingList.Where(x => x.ParamName.Equals(keyInfo.Key1)).FirstOrDefault();
                if (mapInfo != null && string.IsNullOrEmpty(keyInfo.Key1) == false)
                {
                    // 対象キーが設定済で数値変換可能かをチェックして登録する
                    int wkInt;
                    if (rowDic[mapInfo.ValName] != null && int.TryParse(rowDic[mapInfo.ValName].ToString(), out wkInt) == true)
                    {
                        selectKeyData.Key1 = rowDic[mapInfo.ValName];
                    }
                }

                // キー２の設定値を取得し、設定
                mapInfo = mappingList.Where(x => x.ParamName.Equals(keyInfo.Key2)).FirstOrDefault();
                // 対象キーが設定済で数値変換可能かをチェックして登録する
                if (mapInfo != null && string.IsNullOrEmpty(keyInfo.Key2) == false)
                {
                    // 対象キーが設定済で数値変換可能かをチェックして登録する
                    int wkInt;
                    if (rowDic[mapInfo.ValName] != null && int.TryParse(rowDic[mapInfo.ValName].ToString(), out wkInt) == true)
                    {
                        selectKeyData.Key2 = rowDic[mapInfo.ValName];
                    }
                }
                // キー３の設定値を取得し、設定
                mapInfo = mappingList.Where(x => x.ParamName.Equals(keyInfo.Key3)).FirstOrDefault();
                if (mapInfo != null && string.IsNullOrEmpty(keyInfo.Key3) == false)
                {
                    // 対象キーが設定済で数値変換可能かをチェックして登録する
                    int wkInt;
                    if (rowDic[mapInfo.ValName] != null && int.TryParse(rowDic[mapInfo.ValName].ToString(), out wkInt) == true)
                    {
                        selectKeyData.Key3 = rowDic[mapInfo.ValName];
                    }
                }

                // キーが取得できた場合のみ
                if (selectKeyData.Key1 != null || selectKeyData.Key2 != null || selectKeyData.Key3 != null)
                {
                    // 選択キーデータリストに追加
                    selectKeyDataList.Add(selectKeyData);
                }
            }

            // 選択キーデータリストを返却
            return selectKeyDataList;
        }
        /// <summary>
        /// 帳票用検索条件データ取得
        /// </summary>
        /// 
        public bool getSearchConditionForReport(PageInfo pageInfo, out dynamic param)
        {
            param = new ExpandoObject();
            var dicResult = param as IDictionary<string, object>;

            // 場所階層構成IDリストを取得
            List<int> locationIdList = new List<int>();
            var keyNameLocation = STRUCTURE_CONSTANTS.CONDITION_KEY.Location;
            var dicLocation = this.searchConditionDictionary.Where(x => x.ContainsKey(keyNameLocation)).FirstOrDefault();
            if (dicLocation != null && dicLocation.ContainsKey(keyNameLocation))
            {
                // 選択された構成IDリストから配下の構成IDをすべて取得
                locationIdList = dicLocation[keyNameLocation] as List<int>;
                if (locationIdList != null && locationIdList.Count > 0)
                {
                    locationIdList = GetLowerStructureIdList(locationIdList);
                    dicResult.Add("LocationIdList", locationIdList);
                }
            }

            // 職種機種構成IDリストを取得
            List<int> jobIdList = new List<int>();
            var keyNameJob = STRUCTURE_CONSTANTS.CONDITION_KEY.Job;
            var dicJob = this.searchConditionDictionary.Where(x => x.ContainsKey(keyNameJob)).FirstOrDefault();
            if (dicJob != null && dicJob.ContainsKey(keyNameJob))
            {
                // 選択された構成IDリストから配下の構成IDをすべて取得
                jobIdList = dicJob[keyNameJob] as List<int>;
                if (jobIdList != null && jobIdList.Count > 0)
                {
                    dicResult.Add("JobIdList", jobIdList);
                }
            }

            // 詳細検索条件辞書を取得
            var list = this.searchConditionDictionary.Where(x => x.ContainsKey("IsDetailCondition") && x.ContainsKey("CTRLID")).ToList();
            if (list != null && list.Count > 0)
            {
                var dic = list.Where(x => x["IsDetailCondition"].Equals(true) && x["CTRLID"].Equals(pageInfo.CtrlId)).FirstOrDefault();
                var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(pageInfo.CtrlId)).ToList();

                var detailSearchList = new List<Dictionary<string, object>>();

                foreach (var data in dic)
                {
                    if (!data.Key.StartsWith("VAL") || ComUtil.IsNullOrEmpty(data.Value)) { continue; }
                    var mapInfo = mappingList.Where(x => x.ValName.Equals(data.Key)).FirstOrDefault();
                    if (mapInfo == null) { continue; }
                    //var sqlW = ComUtil.GetDetailSearchConditionSqlAndParam(data.Value.ToString(), mapInfo, ref dicResult);
                    var detailSearch = new Dictionary<string, object>();
                    detailSearch.Add(mapInfo.ParamName, data.Value.ToString());
                    detailSearchList.Add(detailSearch);
                }
                if (detailSearchList != null && detailSearchList.Count > 0)
                {
                    dicResult.Add("detailSearchList", detailSearchList);
                }
            }
            return true;
        }
        /// <summary>
        /// 帳票用検索条件データ取得
        /// </summary>
        /// 
        public bool getSearchConditionByTargetCtrlIdForReport(string ctrlId, out dynamic param)
        {
            param = new ExpandoObject();
            var dicResult = param as IDictionary<string, object>;

            // 場所階層構成IDリストを取得
            List<int> locationIdList = new List<int>();
            var keyNameLocation = STRUCTURE_CONSTANTS.CONDITION_KEY.Location;
            var dicLocation = this.searchConditionDictionary.Where(x => x.ContainsKey(keyNameLocation)).FirstOrDefault();
            if (dicLocation != null && dicLocation.ContainsKey(keyNameLocation))
            {
                // 選択された構成IDリストから配下の構成IDをすべて取得
                locationIdList = dicLocation[keyNameLocation] as List<int>;
                if (locationIdList != null && locationIdList.Count > 0)
                {
                    locationIdList = GetLowerStructureIdList(locationIdList);
                    dicResult.Add("LocationIdList", locationIdList);
                }
            }

            // 職種機種構成IDリストを取得
            List<int> jobIdList = new List<int>();
            var keyNameJob = STRUCTURE_CONSTANTS.CONDITION_KEY.Job;
            var dicJob = this.searchConditionDictionary.Where(x => x.ContainsKey(keyNameJob)).FirstOrDefault();
            if (dicJob != null && dicJob.ContainsKey(keyNameJob))
            {
                // 選択された構成IDリストから配下の構成IDをすべて取得
                jobIdList = dicJob[keyNameJob] as List<int>;
                if (jobIdList != null && jobIdList.Count > 0)
                {
                    dicResult.Add("JobIdList", jobIdList);
                }
            }

            // 詳細検索条件辞書を取得
            var list = this.searchConditionDictionary.Where(x => x.ContainsKey("IsDetailCondition") && x.ContainsKey("CTRLID")).ToList();
            if (list != null && list.Count > 0)
            {
                var dic = list.Where(x => x["IsDetailCondition"].Equals(true) && x["CTRLID"].Equals(ctrlId)).FirstOrDefault();
                var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();

                var detailSearchList = new List<Dictionary<string, object>>();

                foreach (var data in dic)
                {
                    if (!data.Key.StartsWith("VAL") || ComUtil.IsNullOrEmpty(data.Value)) { continue; }
                    var mapInfo = mappingList.Where(x => x.ValName.Equals(data.Key)).FirstOrDefault();
                    if (mapInfo == null) { continue; }
                    //var sqlW = ComUtil.GetDetailSearchConditionSqlAndParam(data.Value.ToString(), mapInfo, ref dicResult);
                    var detailSearch = new Dictionary<string, object>();
                    detailSearch.Add(mapInfo.ParamName, data.Value.ToString());
                    detailSearchList.Add(detailSearch);
                }
                if (detailSearchList != null && detailSearchList.Count > 0)
                {
                    dicResult.Add("detailSearchList", detailSearchList);
                }
            }
            return true;
        }
        #endregion

        #region 仮想メソッド
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected virtual int Init()
        {
            try
            {
                // アクション区分=初期化処理、且つステータスが確認でない場合は総件数チェックが必要
                this.NeedsTotalCntCheck = this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComInitForm &&
                    this.Status < CommonProcReturn.ProcStatus.Confirm;

                // 初期化処理
                return InitImpl();
            }
            catch (Exception ex)
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「初期化処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911120004" });
                this.LogNo = string.Empty;

                writeErrorLog(this.MsgId, ex);
                return -1;
            }
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected virtual int Search()
        {
            try
            {
                this.JsonResult = string.Empty;

                // アクション区分=検索、且つステータスが確認でない場合は総件数チェックが必要
                this.NeedsTotalCntCheck = this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Search &&
                    this.Status < CommonProcReturn.ProcStatus.Confirm;

                int returnValue = -1;
                // ページ情報取得
                var pageInfo = GetPageInfo("", this.pageInfoList);

                if (pageInfo.CtrlType != FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn2 || this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Search)
                {
                    // トランザクションテーブル検索個別処理実行
                    if (this.ActionKbn != LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ListSort)
                    {
                        returnValue = SearchImpl();
                    }
                    else
                    {
                        returnValue = ResultDataListSort(pageInfo);
                    }
                }
                else
                {
                    if (this.resultInfoDictionary.Count() > 0)
                    {
                        // 明細データが存在する場合、更新行を取得
                        DateTime updateDate = DateTime.Now;
                        int addRowCnt = 0;
                        var updateList = GetUpdateRowDataList(this.resultInfoDictionary, updateDate, ref addRowCnt);
                        if (updateList.Count > 0)
                        {
                            // 更新行データを一時テーブルへ保存
                            if (!UpdateSearchResultsOfTmpTable(updateList, addRowCnt, pageInfo))
                            {
                                this.Status = CommonProcReturn.ProcStatus.Error;
                                return returnValue;
                            }
                        }
                    }
                    if (this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.DataGet)
                    {
                        setPageNo(this.CtrlId);
                        // 一時テーブル検索共通処理(ページ遷移時)実行
                        returnValue = SearchTmpTableForDataGetImpl(pageInfo);
                    }
                    else
                    {
                        // 一時テーブル検索共通処理(一覧ソート時)実行
                        returnValue = SearchTmpTableForListSortImpl(pageInfo);
                    }
                }

                // 検索結果データが存在しない場合、エラーを表示 ※確認メッセージの場合、処理を行わない
                if (dispMessageAfterSearch())
                {
                    return -1;
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「検索処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                this.LogNo = string.Empty;

                writeErrorLog(this.MsgId, ex);
                return -1;
            }
        }

        /// <summary>
        /// 一時テーブル検索(ページ遷移時)
        /// </summary>
        /// <returns></returns>
        protected virtual int SearchTmpTableForDataGetImpl(PageInfo pageInfo)
        {
            return SearchTmpTable(pageInfo);
        }

        /// <summary>
        /// 一時テーブル検索(一覧ソート時)
        /// </summary>
        /// <returns></returns>
        protected virtual int SearchTmpTableForListSortImpl(PageInfo pageInfo)
        {
            return SearchTmpTable(pageInfo);
        }

        /// <summary>
        /// 一覧ソート(メモリ上データに対して)
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        protected int ResultDataListSort(PageInfo pageInfo)
        {
            var resultList = new List<Dictionary<string, object>>();
            var tmpResultList = this.resultInfoDictionary;

            // 先頭に総件数行を追加
            var pramObj = new Dictionary<string, object> {
                { "GUID", this.GUID },
                { "TABNO", this.BrowserTabNo },
                { "CTRLID", pageInfo.CtrlId },
                { "ROWNO", 0 },
                { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit },
                { "PAGENO", this.getPageNo(pageInfo.CtrlId) },
                { "VAL1", tmpResultList.Count }
            };
            resultList.Add(pramObj);

            // ソート処理を実施
            string sort = "ROWNO";
            if (!string.IsNullOrEmpty(pageInfo.SortColName)) { sort = pageInfo.SortColName; }
            tmpResultList = tmpResultList.OrderBy(x => x[sort].ToString()).ToList();
            // 結果を反映
            resultList.AddRange(tmpResultList);

            this.JsonResult = this.SerializeToJson(resultList);
            return 0;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns>実行件数(0:実行対象無し、0未満:NG)</returns>
        protected virtual int Execute()
        {
            try
            {
                // 実行個別処理
                return ExecuteImpl();
            }
            catch (Exception ex)
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「実行処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911120005" });
                this.LogNo = string.Empty;
                writeErrorLog(this.MsgId, ex);
                return -1;
            }
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行件数(0:実行対象無し、0未満:NG)</returns>
        protected virtual int Delete()
        {
            int result = 0;

            // トランザクション開始
            this.db.BeginTransaction();

            try
            {
                // 削除処理実行
                result = DeleteImpl();

                if (result > 0)
                {
                    // コミット
                    this.db.Commit();
                }
                else
                {
                    // ロールバック
                    this.db.RollBack();
                }
            }
            catch (Exception ex)
            {
                // ロールバック
                this.db.RollBack();

                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「削除処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911110001" });
                this.LogNo = string.Empty;

                writeErrorLog(this.MsgId, ex);
                return -1;
            }
            finally
            {
                // トランザクション終了
                this.db.EndTransaction();
            }

            return result;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <remarks>
        /// ExecuteImpl()から登録処理を呼び出す際のベースメソッド
        /// (個別にオーバーライド可)
        /// </remarks>
        /// <returns>実行件数(0:実行対象無し、0未満:NG)</returns>
        protected virtual int Regist()
        {
            int result = 0;

            // トランザクション開始
            this.db.BeginTransaction();
            try
            {
                // 登録処理実行
                result = RegistImpl();

                if (result > 0)
                {
                    // コミット
                    this.db.Commit();
                }
                else
                {
                    // ロールバック
                    this.db.RollBack();
                }
            }
            catch (Exception ex)
            {
                // ロールバック
                this.db.RollBack();

                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「登録処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911200003" });
                this.LogNo = string.Empty;

                writeErrorLog(this.MsgId, ex);
                return -1;
            }
            finally
            {
                // トランザクション終了
                this.db.EndTransaction();
            }

            return result;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected virtual int Report()
        {
            try
            {
                // 出力個別処理
                return ReportImpl();
            }
            catch (Exception ex)
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「出力処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911120006" });
                this.LogNo = string.Empty;
                writeErrorLog(this.MsgId, ex);
                return -1;
            }
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected virtual int Upload()
        {
            int result = 0;

            // トランザクション開始
            this.db.BeginTransaction();
            try
            {
                // 取込処理実行
                result = UploadImpl();

                if (result > 0)
                {
                    // コミット
                    this.db.Commit();
                }
                else
                {
                    // ロールバック
                    this.db.RollBack();
                }
            }
            catch (Exception ex)
            {
                // ロールバック
                this.db.RollBack();

                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「登録処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911200003" });
                this.LogNo = string.Empty;

                writeErrorLog(this.MsgId, ex);
                return -1;
            }
            finally
            {
                // トランザクション終了
                this.db.EndTransaction();
            }
            return result;
        }

        /// <summary>
        /// 出力個別処理
        /// </summary>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected virtual int ReportImpl() { return 0; }

        /// <summary>
        /// 取込個別処理
        /// </summary>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected virtual int UploadImpl() { return 0; }

        /// <summary>
        /// ExcelPortダウンロード処理
        /// </summary>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected virtual int ExcelPortDownload()
        {
            this.LogNo = string.Empty;
            try
            {
                string fileType = string.Empty;
                string fileName = string.Empty;
                MemoryStream ms = null;
                string resultMsg = string.Empty;
                string detailMsg = string.Empty;

                // 一時テーブル生成のためトランザクション開始
                this.db.BeginTransaction();

                // ExcelPortダウンロード個別処理
                var result = ExcelPortDownloadImpl(ref fileType, ref fileName, ref ms, ref resultMsg, ref detailMsg);
                if (result == ComConsts.RETURN_RESULT.OK)
                {
                    if (string.IsNullOrEmpty(resultMsg))
                    {
                        // 結果メッセージがセットされていない場合
                        //「ダウンロード処理が完了しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, ComRes.ID.ID911160003 });
                    }
                    else
                    {
                        this.MsgId = resultMsg;
                    }

                    // OUTPUTパラメータに設定
                    this.OutputFileType = fileType;
                    this.OutputFileName = fileName;
                    this.OutputStream = ms;
                }
                else
                {
                    if (string.IsNullOrEmpty(resultMsg))
                    {
                        // 結果メッセージがセットされていない場合
                        // 「ダウンロード処理に失敗しました。」
                        string errMsg = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                        // エラーログ出力
                        writeErrorLog(errMsg);
                        this.MsgId = errMsg;
                    }
                    else
                    {
                        this.MsgId = resultMsg;
                    }
                    if (!string.IsNullOrEmpty(detailMsg))
                    {
                        // 詳細メッセージがセットされている場合、ログ出力
                        writeErrorLog(detailMsg);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「ダウンロード処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                writeErrorLog(this.MsgId, ex);
                return ComConsts.RETURN_RESULT.NG;
            }
            finally
            {
                // 一時テーブル削除のためロールバック
                this.db.RollBack();
                this.db.EndTransaction();
            }
        }

        /// <summary>
        /// ExcelPortダウンロード処理
        /// </summary>
        /// <param name="fileType">ファイル種類</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="ms">メモリストリーム</param>
        /// <param name="resultMsg">結果メッセージ</param>
        /// <param name="detailMsg">詳細メッセージ</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected virtual int ExcelPortDownloadImpl(ref string fileType, ref string fileName, ref MemoryStream ms, ref string resultMsg, ref string detailMsg)
        { return ComConsts.RETURN_RESULT.OK; }

        /// <summary>
        /// ExcelPortアップロード処理
        /// </summary>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected virtual int ExcelPortUpload()
        {
            string fileType = string.Empty;
            string fileName = string.Empty;
            MemoryStream ms = null;
            string resultMsg = string.Empty;
            string detailMsg = string.Empty;

            // ファイル読み込み&入力チェック
            if (this.InputStream == null)
            {
                // 「アップロード可能なファイルがありません。」
                this.MsgId = GetResMessage(ComRes.ID.ID941010006);
                return ComConsts.RETURN_RESULT.NG;
            }
            //ファイル情報
            var file = this.InputStream[0];
            // ファイル拡張子チェック
            string extension = Path.GetExtension(file.FileName);
            if (extension != ComUtil.FileExtension.Excel || extension != ComUtil.FileExtension.ExcelMacro)
            {
                // 「ファイル形式が有効ではありません。」
                this.MsgId = GetResMessage(ComRes.ID.ID941280004);
                return ComConsts.RETURN_RESULT.NG;
            }

            // トランザクション開始
            this.db.BeginTransaction();
            try
            {
                // ExcelPortアップロード個別処理実行
                var result = ExcelPortUploadImpl(file, ref fileType, ref fileName, ref ms, ref resultMsg, ref detailMsg);

                if (result == ComConsts.RETURN_RESULT.OK)
                {
                    // コミット
                    this.db.Commit();

                    if (string.IsNullOrEmpty(resultMsg))
                    {
                        // 結果メッセージがセットされていない場合
                        //「アップロード処理が完了しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, ComRes.ID.ID911010007 });
                    }
                    else
                    {
                        this.MsgId = resultMsg;
                    }
                }
                else
                {
                    // ロールバック
                    this.db.RollBack();

                    if (string.IsNullOrEmpty(resultMsg))
                    {
                        // 結果メッセージがセットされていない場合
                        // 「アップロード処理に失敗しました。」
                        string errMsg = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911010007 });
                        // エラーログ出力
                        writeErrorLog(errMsg);
                        this.MsgId = errMsg;
                    }
                    else
                    {
                        this.MsgId = resultMsg;
                    }
                    if (!string.IsNullOrEmpty(detailMsg))
                    {
                        // 詳細メッセージがセットされている場合、ログ出力
                        writeErrorLog(detailMsg);
                    }
                    if(ms != null)
                    {
                        // OUTPUTパラメータに設定
                        this.OutputFileType = fileType;
                        this.OutputFileName = fileName;
                        this.OutputStream = ms;
                    }
                }
            }
            catch (Exception ex)
            {
                // ロールバック
                this.db.RollBack();

                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「アップロード処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911010007 });

                writeErrorLog(this.MsgId, ex);
                return ComConsts.RETURN_RESULT.NG;
            }
            finally
            {
                // トランザクション終了
                this.db.EndTransaction();
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// ExcelPortアップロード個別処理
        /// </summary>
        /// <param name="file">アップロード対象ファイル</param>
        /// <param name="fileType">ファイル種類</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="ms">メモリストリーム</param>
        /// <param name="resultMsg">結果メッセージ</param>
        /// <param name="detailMsg">詳細メッセージ</param>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected virtual int ExcelPortUploadImpl(IFormFile file, ref string fileType, ref string fileName, ref MemoryStream ms, ref string resultMsg, ref string detailMsg)
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 個別処理
        /// </summary>
        /// <param name="methodName">メソッド名</param>
        /// <param name="processName">処理名 ※未指定可</param>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        /// <remarks>アクション区分が実行で、登録、削除、取込以外のメソッドにてトランザクションが必要な場合</remarks>
        protected virtual int ComExecImpl(string methodName, string processName = null)
        {
            int result = 0;
            // トランザクション開始
            this.db.BeginTransaction();
            try
            {
                // 処理実行
                Type type = this.GetType();
                MethodInfo method = type.GetMethod(methodName);
                if (method != null)
                {
                    result = (int)method.Invoke(this, new object[] { });
                }
                else
                {
                    throw new Exception();
                }

                if (result > 0)
                {
                    // コミット
                    this.db.Commit();
                }
                else
                {
                    // ロールバック
                    this.db.RollBack();
                }
            }
            catch (Exception ex)
            {
                // ロールバック
                this.db.RollBack();

                this.Status = CommonProcReturn.ProcStatus.Error;
                if (string.IsNullOrEmpty(processName))
                {
                    // 処理の実行に失敗しました。
                    this.MsgId = GetResMessage(new string[] { "941220002", "911120009" });
                }
                else
                {
                    // {0}に失敗しました。
                    this.MsgId = GetResMessage(new string[] { "941220002", processName });
                }
                this.LogNo = string.Empty;

                writeErrorLog(this.MsgId, ex);
                return -1;
            }
            finally
            {
                // トランザクション終了
                this.db.EndTransaction();
            }

            return result;
        }

        /// <summary>
        /// 権限チェック
        /// </summary>
        /// <returns></returns>
        protected virtual int CheckAuthority()
        {
            try
            {
                var list = new List<Dictionary<string, object>>();

                // ボタンの処理パターンを取得
                int ptn = this.getProcessPtn();

                // 処理パターンに応じて処理を分岐
                int result = -1;
                switch (ptn)
                {
                    case CONDUCT_MST_CONSTANTS.PTN.Bat:
                        // バッチの場合
                        result = ComUtil.GetCheckAuthorityForBatch(this.db, this.UserId, this.ConductId, COM_CTRL_CONSTANTS.CTRLID.ComBatExec, COM_CTRL_CONSTANTS.CTRLID.ComBatRefresh, ref list);
                        break;
                    default:
                        // 上記以外
                        result = ComUtil.GetCheckAuthority(this.db, this.BelongingInfo, this.UserId, this.AuthorityLevelId, this.ConductId, this.searchConditionDictionary, ref list);
                        break;
                }

                //if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                //{
                //int result = ComUtil.GetCheckAuthority(this.db, this.UserId, this.ConductId, this.searchConditionDictionary, ref list);
                if (result < 0)
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「権限チェック処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { "941220002", "911090002" });
                    return -1;
                }

                if (list != null && list.Count > 0)
                {
                    //this.resultInfoDictionary = list;
                    SetJsonResult(list);
                }
                //}

                // 権限チェック個別処理
                return CheckAuthorityImpl();
            }
            catch (Exception ex)
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「権限チェック処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911090002" });
                this.LogNo = string.Empty;
                writeErrorLog(this.MsgId, ex);
                return -1;
            }
        }

        /// <summary>
        /// 処理パターン取得
        /// </summary>
        /// <returns>処理パターン</returns>
        /// <remarks>10:画面、11:バッチ、20:帳票、30:マスタ</remarks>
        private int getProcessPtn()
        {
            //var sql = "select ptn from com_conduct_mst where conductid = @Conductid";
            var sql = "select process_pattern from cm_conduct where conduct_id = @Conductid";
            return this.db.GetEntity<int>(sql, new { Conductid = this.ConductId });
        }

        /// <summary>
        /// 権限チェック個別処理
        /// </summary>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected virtual int CheckAuthorityImpl() { return 0; }
        #endregion

        #region 抽象メソッド
        /// <summary>
        /// 初期処理個別処理
        /// </summary>
        /// <returns>初期処理結果(0:OK/0未満:NG)</returns>
        protected abstract int InitImpl();
        /// <summary>
        /// 検索個別処理
        /// </summary>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected abstract int SearchImpl();
        /// <summary>
        /// 実行個別処理
        /// </summary>
        /// <returns>実行件数(0:実行対象無し、0未満:NG)</returns>
        protected abstract int ExecuteImpl();
        /// <summary>
        /// 登録個別処理
        /// </summary>
        /// <returns>登録件数(0:登録対象無し、0未満:NG)</returns>
        protected abstract int RegistImpl();
        /// <summary>
        /// 削除個別処理
        /// </summary>
        /// <returns>削除件数(0:削除対象無し、0未満:NG)</returns>
        protected abstract int DeleteImpl();
        #endregion

        #region privateメソッド
        protected bool CheckParameters(dynamic param)
        {
            if (IsPropertyExists(param, "LanguageId"))
            {
                this.LanguageId = param.LanguageId;
            }
            else
            {
                // 言語IDが未指定の場合は日本語
                this.LanguageId = "ja";
            }

            if (!IsPropertyExists(param, "FactoryId"))
            {
                // ユーザー本務工場IDが指定されている場合は設定する
                this.FactoryId = param.FactoryId;
            }

            if (!IsPropertyExists(param, "ConductId") || string.IsNullOrEmpty(param.ConductId))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータに機能IDが設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911070002" });
                this.LogNo = string.Empty;
                return false;
            }
            if (!IsPropertyExists(param, "PgmId") || string.IsNullOrEmpty(param.PgmId))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータにプログラムIDが設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911070002" });
                this.LogNo = string.Empty;
                return false;
            }
            if (!IsPropertyExists(param, "FormNo") || param.FormNo < 0)
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータにフォーム番号が設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911280001" });
                this.LogNo = string.Empty;
                return false;
            }
            if (!IsPropertyExists(param, "ActionKbn") || param.ActionKbn < 0)
            {
                if (param.ActionKbn != -1 && param.ActionKbn != -2) // ※-1, -2はページング/ソートの際の処理のため例外的にエラーにしない
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「パラメータにアクション区分が設定されていません。」
                    this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911010002" });
                    this.LogNo = string.Empty;
                    return false;
                }
            }
            if (!IsPropertyExists(param, "CtrlId") || string.IsNullOrEmpty(param.CtrlId))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータにコントロールIDが設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911100001" });
                this.LogNo = string.Empty;
                return false;
            }
            if (!IsPropertyExists(param, "UserId") || string.IsNullOrEmpty(param.UserId))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータにログインユーザIDが設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911430003" });
                this.LogNo = string.Empty;
                return false;
            }
            if (!IsPropertyExists(param, "RootPath") || string.IsNullOrEmpty(param.RootPath))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータにルートパスが設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911410001" });
                this.LogNo = string.Empty;
                return false;
            }

            // メンバ変数へパラメータを設定
            this.TerminalNo = param.TerminalNo;
            this.ConductId = param.ConductId;
            this.PgmId = param.PgmId;
            this.FormNo = param.FormNo;
            this.UserId = param.UserId;
            this.CtrlId = param.CtrlId;
            this.ActionKbn = param.ActionKbn;
            this.rootPath = param.RootPath;
            if (IsPropertyExists(param, "TransActionDiv"))
            {
                this.TransActionDiv = param.TransActionDiv;
            }
            if (IsPropertyExists(param, "Status"))
            {
                this.Status = param.Status;
            }
            if (IsPropertyExists(param, "InputStream"))
            {
                this.InputStream = param.InputStream;
            }
            if (IsPropertyExists(param, "GUID"))
            {
                this.GUID = param.GUID;
            }
            if (IsPropertyExists(param, "BrowserTabNo"))
            {
                this.BrowserTabNo = param.BrowserTabNo;
            }

            return true;
        }

        protected bool CheckParameters(CommonProcParamIn param)
        {
            // ユーザー権限レベルID
            this.AuthorityLevelId = param.AuthorityLevelId;
            // ユーザー所属情報を設定
            this.BelongingInfo = param.BelongingInfo;
            // ユーザー本務工場IDを設定
            this.FactoryId = param.FactoryId;

            if (!string.IsNullOrEmpty(param.LanguageId))
            {
                this.LanguageId = param.LanguageId;
            }
            else
            {
                // 言語IDが未指定の場合は日本語
                this.LanguageId = "ja";
            }

            if (string.IsNullOrEmpty(param.ConductId))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータに機能IDが設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911070002" });
                this.LogNo = string.Empty;
                return false;
            }
            if (string.IsNullOrEmpty(param.PgmId))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータにプログラムIDが設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911070002" });
                this.LogNo = string.Empty;
                return false;
            }
            if (param.FormNo < 0)
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータにフォーム番号が設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911280001" });
                this.LogNo = string.Empty;
                return false;
            }
            if (param.ActionKbn < 0)
            {
                if (param.ActionKbn != -1 && param.ActionKbn != -2) // ※-1, -2はページング/ソートの際の処理のため例外的にエラーにしない
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「パラメータにアクション区分が設定されていません。」
                    this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911010002" });
                    this.LogNo = string.Empty;
                    return false;
                }
            }
            if (string.IsNullOrEmpty(param.CtrlId))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータにコントロールIDが設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911100001" });
                this.LogNo = string.Empty;
                return false;
            }
            if (string.IsNullOrEmpty(param.UserId))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータにログインユーザIDが設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911430003" });
                this.LogNo = string.Empty;
                return false;
            }
            if (string.IsNullOrEmpty(param.RootPath))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「パラメータにルートパスが設定されていません。」
                this.MsgId = GetResMessage(new string[] { "941220003", "911260002", "911410001" });
                this.LogNo = string.Empty;
                return false;
            }

            // メンバ変数へパラメータを設定
            this.TerminalNo = param.TerminalNo;
            this.ConductId = param.ConductId;
            this.PgmId = param.PgmId;
            this.FormNo = param.FormNo;
            this.UserId = param.UserId;
            this.CtrlId = param.CtrlId;
            this.ActionKbn = param.ActionKbn;
            this.TransActionDiv = param.TransActionDiv;
            this.rootPath = param.RootPath;
            this.Status = param.Status;
            this.InputStream = param.InputStream;
            this.GUID = param.GUID;
            this.BrowserTabNo = param.BrowserTabNo;

            return true;
        }

        /// <summary>
        /// DB接続エラーメッセージ設定
        /// </summary>
        protected void setDBConnectionErrorLogAndMessage()
        {
            // 「DB接続に失敗しました。」
            logger.ErrorLog(this.FactoryId, this.UserId, ComUtil.GetPropertiesMessage(CommonResources.ID.ID941190004, this.LanguageId));
            // エラーが発生しました。システム管理者に問い合わせてください。
            this.MsgId = ComUtil.GetPropertiesMessage(CommonResources.ID.ID941040004, this.LanguageId);

        }

        /// <summary>
        /// 更新フラグの値を列挙型から定数に変換
        /// </summary>
        /// <param name="updtag">列挙型の値</param>
        /// <returns>定数の値</returns>
        private string convertUpdtagValue(Updtag updtag)
        {
            switch (updtag)
            {
                case Updtag.Input:
                    return TMPTBL_CONSTANTS.UPDTAG.Input;
                //case Updtag.Select:
                //    return TMPTBL_CONSTANTS.UPDTAG.Select;
                //case Updtag.Update:
                //    return TMPTBL_CONSTANTS.UPDTAG.Update;
                case Updtag.UpdatedToTmp:
                    return TMPTBL_CONSTANTS.UPDTAG.UpdatedToTmp;
                case Updtag.Updated:
                    return TMPTBL_CONSTANTS.UPDTAG.Updated;
                case Updtag.None:
                    // 指定なしの場合、Null
                    return null;
                default:
                    // 到達不能
                    return null;
            }
        }

        /// <summary>
        /// 再描画ページNOを取得
        /// </summary>
        /// <param name="ctrlid">コントロールID</param>
        /// <returns>ページNO</returns>
        protected string getPageNo(string ctrlid)
        {
            // 該当のコントロールIDが含まれている場合、ページNOを戻す
            if (this.outParamPageInfoList.ContainsKey(ctrlid))
            {
                return outParamPageInfoList[ctrlid].ToString();
            }
            else
            {
                // 含まれない場合、
                return "";
            }
        }

        /// <summary>
        /// 再描画ページNOを設定
        /// </summary>
        /// <param name="ctrlid">コントロールID</param>
        /// <param name="pageNo">ページNO ※デフォルト FWから連携されたページNO</param>
        protected void setPageNo(string ctrlid, string pageNo = null)
        {
            // 該当のコントロールIDが含まれている場合、ページNOを設定
            if (this.outParamPageInfoList.ContainsKey(ctrlid))
            {
                if (string.IsNullOrEmpty(pageNo))
                {
                    // ページNOが未設定の場合
                    foreach (var pageInfo in this.pageInfoList)
                    {
                        if (!pageInfo.CtrlId.Equals(ctrlid)) { continue; } // コントロールIDが異なる場合、スキップ
                        pageNo = pageInfo.PageNo.ToString(); // フロントから連携されたページNOを指定
                        break; // 取得できた場合、処理を抜ける
                    }
                }
                outParamPageInfoList[ctrlid] = pageNo;
            }

            // 含まれない場合、処理をしない
            return;
        }

        /// <summary>
        /// ページ取得箇所を設定
        /// </summary>
        /// <param name="ctrlid">コントロールID</param>
        /// <param name="pageNo">ページNO ※デフォルト FWから連携されたページNO</param>
        protected void changePageNo(string ctrlid, int pageNo = -1)
        {
            if (pageNo == -1) { pageNo = int.Parse(PageNo.HeaderPage); }
            // ページ情報分処理を回す
            foreach (var pageInfo in this.pageInfoList)
            {
                // 該当のコントロールIDが含まれている場合、ページNOを設定
                if (pageInfo.CtrlId != ctrlid) { continue; } // コントロールIDが異なる場合、スキップ
                // 一致している場合、ページ取得箇所を変更する
                pageInfo.PageNo = pageNo;
            }

            // 含まれない場合、処理をしない
            return;
        }

        /// <summary>
        /// 一時テーブルから該当レコードを取得する
        /// </summary>
        /// <param name="ctrlidList">コントロールIDリスト</param>
        /// <param name="paramUpdtag">更新フラグ 未指定 or "None"の場合は条件として扱わない</param>
        /// <param name="isSelected">選択フラグ 未指定 or Falseの場合は条件として扱わない</param>
        /// <returns></returns>
        protected List<Dictionary<string, object>> GetTmptblData(string[] ctrlidList, Updtag paramUpdtag = Updtag.None, bool isSelected = false)
        {
            // 引数の値を実際に使用する文字列の値に変換
            string updtag = convertUpdtagValue(paramUpdtag);
            string seltag = isSelected ? "1" : string.Empty;

            // 結果格納用リストを設定
            var tmpList = new List<Dictionary<string, object>>();
            var tmpDic = new Dictionary<string, object>();
            foreach (var ctrlid in ctrlidList)
            {
                // 一時テーブルから該当のレコードを取得
                var condition = new TmpTableSearchCondition();
                condition.GUID = this.GUID;
                condition.TabNo = this.BrowserTabNo;
                condition.Ctrlid = ctrlid;
                if (!string.IsNullOrEmpty(updtag)) { condition.Updtag = updtag; } // 指定されている場合、条件として含む
                if (!string.IsNullOrEmpty(seltag)) { condition.Seltag = seltag; } // 指定されている場合、条件として含む

                IList<Dao.ComTempDataEntity> results = db.GetListByOutsideSqlByDataClass<Dao.ComTempDataEntity>(SqlName.GetTmpDataList, SqlName.SubDir, condition);

                Type t = typeof(Dao.ComTempDataEntity);
                var properties = typeof(Dao.ComTempDataEntity).GetProperties();
                if (results == null || results.Count == 0) { continue; } // 1件も取得できない場合、処理をスキップする
                foreach (var result in results)
                {
                    //var tmpResult = result as IDictionary<string, object>;
                    tmpDic = new Dictionary<string, object>();
                    tmpDic.Add("FORMNO", this.FormNo); // 画面NO
                    tmpDic.Add("CTRLID", ctrlid);      // コントロール
                    tmpDic.Add("ROWSTATUS", result.Rowstatus);//tmpResult["rowstatus"]); // 行ステータス
                    tmpDic.Add("UPDTAG", result.Updtag);//tmpResult["updtag"]); // 更新フラグ
                    tmpDic.Add("SELTAG", result.Seltag);//tmpResult["seltag"] != null ? tmpResult["seltag"] : "0"); // 選択フラグ
                    //object lockData = null;
                    //var tmpResultDic = this.resultInfoDictionary.FirstOrDefault(x => x["CTRLID"].ToString().Equals(ctrlid));
                    //if (tmpResultDic != null) { lockData = tmpResultDic["lockData"]; }
                    //tmpDic.Add("lockData", lockData); // ロック情報
                    tmpDic.Add("lockData", result.Lockdata);//tmpResult["seltag"] != null ? tmpResult["seltag"] : "0"); // 選択フラグ
                    var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlid)).ToList();
                    foreach (var mapInfo in mappingList)
                    {
                        foreach (MemberInfo prop in properties)
                        {
                            if (prop.Name.ToLower() != (mapInfo.ValName.ToLower())) { continue; } // VAL情報が存在しない場合、処理をスキップ
                            PropertyInfo pr = t.GetProperty(prop.Name);
                            object resobj = pr.GetValue(result, null);
                            tmpDic.Add(mapInfo.ValName.ToUpper(), resobj);
                            break; // 設定した場合、次の項目へ
                        }
                    }
                    tmpList.Add(tmpDic);
                }
            }
            // 一時テーブルから該当のレコードを取得する
            return tmpList;
        }

        /// <summary>
        /// 初期表示処理で検索を行う際の処理
        /// </summary>
        /// <returns>-1:異常、0以上:正常</returns>
        protected int InitSearch()
        {
            int returnValue = SearchImpl();
            // 検索結果データが存在しない場合、エラーを表示 ※確認メッセージの場合、処理を行わない
            if (dispMessageAfterSearch())
            {
                return -1;
            }
            return returnValue;
        }

        /// <summary>
        /// 一時テーブルへの登録が必要かどうか
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <returns>true:必要/false:不要</returns>
        protected bool IsRequiredRegistTmpTable(PageInfo pageInfo)
        {
            //return (this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Search ||
            //    this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComInitForm) &&
            //    pageInfo.CtrlType == 102;
            return pageInfo.CtrlType == FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn2;
        }

        /// <summary>
        /// ページ指定が必要かどうか
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="isBeforeSelect">検索前かどうか(true:検索前/false:検索後)</param>
        /// <returns>true:必要/false:不要</returns>
        protected bool IsRequiredPageInfoSetting(PageInfo pageInfo, bool isBeforeSelect = true)
        {
            // 全件検索かどうか
            bool isSearchAll = (this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Search || this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComInitForm) &&
                pageInfo != null && pageInfo.CtrlType != 101;

            return (!isSearchAll || !isBeforeSelect) && pageInfo.PageSize > 0 && pageInfo.PageNo > 0;
        }

        /// <summary>
        /// プロパティの存在チェック
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected bool IsPropertyExists(dynamic obj, string name)
        {
            if (obj is ExpandoObject)
                return ((IDictionary<string, object>)obj).ContainsKey(name);

            return obj.GetType().GetProperty(name) != null;
        }

        /// <summary>
        /// JSON文字列へのシリアライズ
        /// </summary>
        /// <param name="dic">Dictionary</param>
        /// <returns>JSON文字列</returns>
        protected string SerializeToJson<T>(List<T> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    var jsonOptions = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    };
                    return JsonSerializer.Serialize(list, jsonOptions);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// JSON文字列をデシリアライズ
        /// </summary>
        /// <param name="dic">Dictionary</param>
        /// <returns>JSON文字列</returns>
        protected List<Dictionary<string, object>> DeserializeFromJson(string json)
        {
            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);
                }
                else
                {
                    return new List<Dictionary<string, object>>();
                }
            }
            catch
            {
                return new List<Dictionary<string, object>>();
            }
        }

        /// <summary>
        /// JSON文字列からのデシリアライズ
        /// </summary>
        /// <typeparam name="T">デシリアライズ先の型</typeparam>
        /// <param name="json">シリアライズしたJSON文字列</param>
        /// <returns>デシリアライズを行った変数</returns>
        public List<T> DeserializeFromJson<T>(string json)
        {
            // JSON文字列の実行条件をデシリアライズ
            return JsonSerializer.Deserialize<List<T>>(json);
        }

        /// <summary>
        /// 条件の設定
        /// </summary>
        /// <param name="namesPair"></param>
        protected bool SetConditionObj(Dictionary<string, object> dic, Dictionary<string, object> namesPair, dynamic condition)
        {
            foreach (var item in namesPair)
            {
                if (dic.ContainsKey(item.Key))
                {
                    // 指定した条件名が条件辞書に存在する場合、条件オブジェクトへ追加
                    var key = item.Value;
                    var val = dic[item.Key];
                    if (val != null)
                    {
                        if (val is string && key is object[])
                        {
                            // パラメータ値が文字列でキー値が配列の場合、マッチパターン指定
                            var keys = key as object[];
                            string param = val.ToString();
                            if (keys != null && keys.Length > 1)
                            {
                                switch ((MatchPattern)keys[1])
                                {
                                    case MatchPattern.ForwardMatch: // 前方一致
                                        param = param + "%";
                                        break;
                                    case MatchPattern.BackwardMatch:    // 後方一致
                                        param = "%" + param;
                                        break;
                                    case MatchPattern.PartialMatch: // 部分一致
                                        param = "%" + param + "%";
                                        break;
                                    default:    // 完全一致
                                        break;
                                }
                                ((IDictionary<string, object>)condition).Add((string)keys[0], param);
                                continue;
                            }
                        }
                        ((IDictionary<string, object>)condition).Add((string)key, val);
                    }
                }
                else
                {
                    // 指定した条件名が条件辞書に存在しない場合、NG
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 条件の設定
        /// </summary>
        /// <param name="namesPair"></param>
        protected bool SetConditionObj(List<Dictionary<string, object>> list, List<Dictionary<string, object>> namesPair, dynamic condition)
        {
            for (int i = 0; i < namesPair.Count; i++)
            {
                if (list.Count > i)
                {
                    SetConditionObj(list[i], namesPair[i], condition);
                }
            }
            // 言語ID
            condition.LanguageId = this.LanguageId;

            return true;
        }

        /// <summary>
        /// 検索条件の設定
        /// </summary>
        /// <param name="namesPair">カラム名の変換辞書</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="pageInfo">ページ情報</param>
        protected bool SetSearchCondition(List<Dictionary<string, object>> list, List<Dictionary<string, object>> namesPair, dynamic condition, PageInfo pageInfo)
        {
            SetConditionObj(list, namesPair, condition);

            if (IsRequiredPageInfoSetting(pageInfo))
            {
                // ページ情報指定が必要な場合、実行条件に追加
                condition.PageSize = pageInfo.PageSize;
                condition.PageNumber = pageInfo.PageNo;
                condition.Offset = pageInfo.Offset;
            }
            return true;
        }

        /// <summary>
        /// 実行条件の設定
        /// </summary>
        /// <param name="namesPair">カラム名の変換辞書</param>
        /// <param name="obj">条件オブジェクト</param>
        /// <param name="date">実行日時</param>
        /// <param name="updatorCd">更新者ID</param>
        /// <param name="inputorCd">登録者ID</param>
        protected bool SetExecuteCondition(Dictionary<string, object> dic, Dictionary<string, object> namesPair, dynamic condition, DateTime date, string updatorCd, string inputorCd = "")
        {
            SetConditionObj(dic, namesPair, condition);

            // 更新者IDと更新日時を設定
            condition.UpdateDate = date;
            condition.UpdatorCd = updatorCd;
            if (!string.IsNullOrEmpty(inputorCd))
            {
                // 登録者IDと登録日時を設定
                condition.InputDate = date;
                condition.InputorCd = inputorCd;
            }

            return true;
        }

        /// <summary>
        /// 一時テーブル検索
        /// </summary>
        /// <returns></returns>
        protected int SearchTmpTable(PageInfo pageInfo)
        {
            // 検索件数取得条件生成
            var condition = new TmpTableSearchCondition();
            condition.GUID = this.GUID;
            condition.TabNo = this.BrowserTabNo;

            // 総件数を取得
            int cnt = db.GetCountByOutsideSql(SqlName.GetTmpDataCnt, SqlName.SubDir, condition);
            if (cnt < 0) { return -1; }

            // ページ条件追加
            if (pageInfo.PageSize > 0 && pageInfo.PageNo > 0)
            {
                //condition.StartRowNo = pageInfo.StartRowNo;
                //condition.EndRowNo = pageInfo.EndRowNo;
                condition.PageSize = pageInfo.PageSize;
                condition.Offset = pageInfo.Offset;
            }
            string sortColName;
            if (string.IsNullOrEmpty(pageInfo.SortColName))
            {
                sortColName = "rowno";
            }
            else
            {
                sortColName = pageInfo.SortColName;
            }

            // 検索SQL生成
            var sql = GetSelectSqlForTmpTable(pageInfo, sortColName);

            // 一時テーブル検索
            IList<Dao.ComTempDataEntity> results = this.db.GetList<Dao.ComTempDataEntity>(sql, condition);
            if (results.Count > 0)
            {
                if (setTmpSearchResults<Dao.ComTempDataEntity>(pageInfo, results, cnt))
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                }
            }

            return results.Count;
        }

        /// <summary>
        /// dynamic型の検索結果をDictionary型へ変換
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="results">検索結果</param>
        /// <param name="namesPair">カラム名の変換辞書</param>
        /// <param name="cnt">総件数</param>
        /// <returns></returns>
        protected List<Dictionary<string, object>> ConvertToSearchResultDictionary(string ctrlId, dynamic condition, dynamic results, Dictionary<string, string> namesPair, int cnt = -1)
        {
            var list = new List<Dictionary<string, object>>();
            var dicList = new Dictionary<string, object>();
            var rowNo = 0;
            int offset = 0;

            if (cnt >= 0)
            {
                // パラメータに総件数が設定されている場合、先頭に総件数行を追加
                dicList = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", rowNo++ },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit },
                    { "PAGENO", this.getPageNo(ctrlId) },
                    { "VAL1", cnt.ToString() }
                };
                list.Add(dicList);

                // 検索条件からオフセット件数を取得
                if (condition != null)
                {
                    if (((IDictionary<string, object>)condition).ContainsKey("Offset"))
                    {
                        offset = condition.Offset;
                    }
                }
            }
            else
            {
                rowNo = 1;
            }

            foreach (var result in results)
            {
                dicList = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", offset + rowNo++ },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit }
                };

                IDictionary<string, object> dicResult = result as IDictionary<string, object>;
                foreach (var item in namesPair)
                {
                    //if (dicResult.ContainsKey(item.Key))
                    if (dicResult.Any(x => x.Key.ToUpper() == item.Key.ToUpper()))
                    {
                        // 指定したカラム名が検索結果に存在する場合、結果オブジェクトへ追加
                        dicList.Add(item.Value, dicResult[item.Key]);
                    }
                }
                list.Add(dicList);
            }
            return list;
        }

        #region マッピング情報リスト使用メソッド

        /// <summary>
        /// コントロールIDで絞り込んだDictionaryと列のKEY_NAMEより値を取得する
        /// </summary>
        /// <param name="dictionary">コントロールIDで絞り込んだDictionary</param>
        /// <param name="columnKeyName">一覧項目定義拡張テーブルのKEY_NAMEの値</param>
        /// <returns>DictionaryのキーがKEY_NAMEの値</returns>
        protected string getDictionaryKeyValue(Dictionary<string, object> dictionary, string columnKeyName)
        {
            // マッピング情報を取得するためのコントロールIDはディクショナリと同じものを使用
            var ctrlId = dictionary["CTRLID"].ToString();
            // マッピング情報を取得(変更しないからDirect)
            var info = getResultMappingInfoDirect(ctrlId);
            // VALnを取得
            var valName = info.getValName(columnKeyName);

            // ディクショナリより値を取得
            return getDictionaryValue(dictionary, valName);
        }

        /// <summary>
        /// ディクショナリリストと対象のコントロールID、列のKEY_NAMEより値を取得する
        /// </summary>
        /// <param name="dictionaryList">Dictionaryのリスト</param>
        /// <param name="ctrlId">DIctionaryを絞り込むコントロールID(一意に絞り込むこと、明細は不可)</param>
        /// <param name="columnKeyName">一覧項目定義拡張テーブルのKEY_NAMEの値</param>
        /// <returns>dictionaryListのコントロールIDがキーがctrlIdのディクショナリで、キーがKEY_NAMEの値</returns>
        protected string getDictionaryKeyValueByList(List<Dictionary<string, object>> dictionaryList, string ctrlId, string columnKeyName)
        {
            // ディクショナリリストからコントロールIDが一致するディクショナリを取得する
            var dictionary = dictionaryList.FirstOrDefault(x => ctrlId.Equals(x["CTRLID"].ToString()));
            // ディクショナリとKEY_NAMEより値を取得
            return getDictionaryKeyValue(dictionary, columnKeyName);
        }

        /// <summary>
        /// 項目マスタより取得した画面の項目のマッピング情報ユーティリティクラス(最大単位はグループ番号)
        /// </summary>
        protected class MappingInfo
        {
            /// <summary>
            /// マッピング情報
            /// </summary>
            public List<ComUtil.DBMappingInfo> Value { get; }

            /// <summary>
            /// 自身のグループ番号
            /// </summary>
            public short GrpNo
            {
                get
                {
                    return this.Value.First().GrpNo;
                }
            }

            /// <summary>
            /// 自身のコントロールIDリスト
            /// </summary>
            public List<string> CtrlIdList
            {
                get
                {
                    return this.Value.Select(x => x.CtrlId).Distinct().ToList();
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="mappingInfo">対象のマッピング情報</param>
            public MappingInfo(List<ComUtil.DBMappingInfo> mappingInfo)
            {
                this.Value = mappingInfo;
            }

            /// <summary>
            /// PKeyNameの値よりValNameを取得する
            /// </summary>
            /// <param name="keyName">key_nameに設定された値</param>
            /// <returns>ValName(VALn)の値</returns>
            public string getValName(string keyName)
            {
                return this.Value.First(x => x.KeyName.Equals(keyName)).ValName;
            }

            /// <summary>
            /// コントロールIDでさらに絞り込む
            /// </summary>
            /// <param name="ctrlId"></param>
            /// <returns></returns>
            public MappingInfo selectByCtrlId(string ctrlId)
            {
                return new MappingInfo(this.Value.Where(x => x.CtrlId == ctrlId).ToList());
            }
        }

        /// <summary>
        /// マッピング情報リストよりコントロールIDを指定して、対象のマッピング情報を取得する(複製)
        /// </summary>
        /// <param name="ctrlId">取得したいマッピング情報のコントロールID</param>
        /// <returns>対象のコントロールIDのマッピング情報</returns>
        protected MappingInfo getResultMappingInfo(string ctrlId)
        {
            // マッピング情報よりコントロールIDで抽出
            var temp = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();
            // 複製して渡すから操作されても元々のマッピング情報に変更はない
            return new MappingInfo(new List<ComUtil.DBMappingInfo>(temp));
        }

        /// <summary>
        /// マッピング情報リストよりコントロールIDを指定して、対象のマッピング情報を取得する(取得)
        /// </summary>
        /// <param name="ctrlId">取得したいマッピング情報のコントロールID</param>
        /// <returns>対象のコントロールIDのマッピング情報</returns>
        protected MappingInfo getResultMappingInfoDirect(string ctrlId)
        {
            // マッピング情報よりコントロールIDで抽出
            var temp = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();
            // そのまま渡すので変更しないこと
            return new MappingInfo(temp);
        }

        /// <summary>
        /// マッピング情報リストよりグループ番号を指定して、対象のマッピング情報を取得する(複製)
        /// </summary>
        /// <param name="grpNo">取得したいマッピング情報のグループ番号</param>
        /// <returns>対象のコントロールIDのマッピング情報</returns>
        protected MappingInfo getResultMappingInfoByGrpNo(int grpNo)
        {
            // マッピング情報よりグループ番号で抽出
            var temp = this.mapInfoList.Where(x => x.GrpNo == grpNo).ToList();
            // 複製して渡すから操作されても元々のマッピング情報に変更はない
            return new MappingInfo(new List<ComUtil.DBMappingInfo>(temp));
        }

        /// <summary>
        /// マッピング情報リストよりグループ番号を指定して、対象のマッピング情報を取得する(取得)
        /// </summary>
        /// <param name="grpNo">取得したいマッピング情報のグループ番号</param>
        /// <returns>対象のコントロールIDのマッピング情報</returns>
        protected MappingInfo getResultMappingInfoDirectByGrpNo(int grpNo)
        {
            // マッピング情報よりグループ番号で抽出
            var temp = this.mapInfoList.Where(x => x.GrpNo == grpNo).ToList();
            // そのまま渡すので変更しないこと
            return new MappingInfo(temp);
        }

        /// <summary>
        /// 条件の設定
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="dic">変換対象辞書</param>
        /// <param name="mappingList">マッピング情報リスト</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="convType">変換対象条件種類(検索/実行/検索結果)</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        private bool setConditionObj(string ctrlId, Dictionary<string, object> dic, dynamic condition, ComUtil.ConvertType convType, List<string> paramList = null)
        {
            List<ComUtil.DBMappingInfo> mappingList;
            if (paramList == null || paramList.Count == 0)
            {
                mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();
            }
            else
            {
                mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId) && paramList.Contains(x.ParamName)).ToList();
            }
            var tblNameList = mapInfoList.Where(x => !string.IsNullOrEmpty(x.TblName)).Select(x => x.TblName).Distinct().ToList();

            foreach (var mapInfo in mappingList)
            {
                // VAL名またはカラム名が未設定の場合はスキップ
                if (string.IsNullOrEmpty(mapInfo.ValName) || string.IsNullOrEmpty(mapInfo.ColName)) { continue; }

                if (dic.ContainsKey(mapInfo.ValName))
                {
                    // 指定した条件名が条件辞書に存在する場合、条件オブジェクトへ追加
                    var key = mapInfo.ParamName;
                    var val = dic[mapInfo.ValName];
                    if (val != null)
                    {
                        ((IDictionary<string, object>)condition).Add(key, val);
                    }
                }
                else
                {
                    // 指定した条件名が条件辞書に存在しない場合、NG
                    return false;
                }
            }

            foreach (string tblName in tblNameList)
            {
                // テーブル毎に型変換を実行
                var mapList = mappingList.Where(x => x.TblName.Equals(tblName)).ToList();
                //var colInfoList = GetColumnInfoList(tblName);
                //ComUtil.ConvertColumnType(mapList, colInfoList, condition, convType);
                ComUtil.ConvertColumnType(mapList, condition, convType);
            }

            return true;
        }

        /// <summary>
        /// 条件の設定
        /// </summary>
        /// <param name="list">変換対象辞書リスト</param>
        /// <param name="mappingList">マッピング情報リスト</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="convType">変換対象条件種類(検索/実行/検索結果)</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        private bool setConditionObj(string ctrlId, List<Dictionary<string, object>> list, dynamic condition, ComUtil.ConvertType convType, List<string> paramList = null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list.Count > i)
                {
                    setConditionObj(ctrlId, list[i], condition, convType, paramList);
                }
            }
            // 言語ID
            condition.LanguageId = this.LanguageId;

            return true;
        }

        // ↓↓↓2022/12/13 CommonSTDUtilへ移植↓↓↓
        ///// <summary>
        ///// 条件の設定
        ///// </summary>
        ///// <param name="ctrlId">コントロールID</param>
        ///// <param name="dic">変換対象辞書</param>
        ///// <param name="mappingList">マッピング情報リスト</param>
        ///// <param name="condition">条件オブジェクト</param>
        ///// <param name="convType">変換対象条件種類(検索/実行/検索結果)</param>
        ///// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        //private bool setConditionByDataClass(string ctrlId, dynamic condition, Dictionary<string, object> dic, ComUtil.ConvertType convType, List<string> paramList = null)
        //{
        //    List<ComUtil.DBMappingInfo> mappingList;
        //    if (paramList == null || paramList.Count == 0)
        //    {
        //        mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();
        //    }
        //    else
        //    {
        //        mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId) && paramList.Contains(x.ParamName)).ToList();
        //    }

        //    return setConditionByDataClass(mappingList, condition, dic, convType, paramList);
        //}

        ///// <summary>
        ///// 条件の設定
        ///// </summary>
        ///// <param name="mappingList">マッピング情報リスト</param>
        ///// <param name="dic">変換対象辞書</param>
        ///// <param name="mappingList">マッピング情報リスト</param>
        ///// <param name="condition">条件オブジェクト</param>
        ///// <param name="convType">変換対象条件種類(検索/実行/検索結果)</param>
        ///// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        //private bool setConditionByDataClass(List<ComUtil.DBMappingInfo> mappingList, dynamic condition, Dictionary<string, object> dic, ComUtil.ConvertType convType, List<string> paramList = null)
        //{
        //    foreach (var mapInfo in mappingList)
        //    {
        //        // VAL名またはカラム名が未設定の場合はスキップ
        //        if (string.IsNullOrEmpty(mapInfo.ValName) || string.IsNullOrEmpty(mapInfo.ColName)) { continue; }
        //        if (dic.ContainsKey(mapInfo.ValName))
        //        {
        //            // 指定した条件名が条件辞書に存在する場合、条件オブジェクトへ追加
        //            var key = mapInfo.ParamName;
        //            var val = dic[mapInfo.ValName];

        //            if (CommonUtil.IsNullOrEmpty(val))
        //            {
        //                if (mapInfo.IsInClause)
        //                {
        //                    // IN句の場合、Listに初期値を設定
        //                    PropertyInfo prop = condition.GetType().GetProperty(key + "List");
        //                    if (prop == null) { continue; }
        //                    if (prop.PropertyType == typeof(List<decimal>))
        //                    {
        //                        List<decimal> list = new List<decimal>();
        //                        setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                        continue;
        //                    }
        //                    else if (prop.PropertyType == typeof(List<int>))
        //                    {
        //                        List<int> list = new List<int>();
        //                        setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                        continue;
        //                    }
        //                    else if (prop.PropertyType == typeof(List<long>))
        //                    {
        //                        List<long> list = new List<long>();
        //                        setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                        continue;
        //                    }
        //                    else if (prop.PropertyType == typeof(List<string>))
        //                    {
        //                        List<string> list = new List<string>();
        //                        setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                        continue;
        //                    }
        //                    else if (prop.PropertyType == typeof(List<DateTime>))
        //                    {
        //                        List<DateTime> list = new List<DateTime>();
        //                        setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        List<object> list = new List<object>();
        //                        setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                        continue;
        //                    }
        //                }
        //                continue;
        //            }

        //            string value = val.ToString();
        //            // FromTo分割の場合、分割後の値で設定を行う
        //            if (mapInfo.IsFromTo)
        //            {
        //                // デリミタ文字が含まれていない場合、そのまま設定する
        //                if (!value.Contains(ComUtil.FromToDelimiter.ToString()))
        //                {
        //                    setValueToClassCon(mapInfo, key, value, condition, convType);
        //                    continue;
        //                }

        //                var values = value.Split(ComUtil.FromToDelimiter);
        //                int count = 0;
        //                foreach (var data in values)
        //                {
        //                    var setValue = data;
        //                    if (data.Contains(ComUtil.NumberUnitDelimiter))
        //                    {
        //                        setValue = data.Split(ComUtil.NumberUnitDelimiter)[0];
        //                    }
        //                    var tmpKey = key;
        //                    if (count == 0) { tmpKey += "From"; } else { tmpKey += "To"; }
        //                    setValueToClassCon(mapInfo, tmpKey, setValue, condition, convType);
        //                    count++;
        //                }
        //                continue;
        //            }

        //            // IN句パラメータの場合、配列に格納しなおし、設定を行う
        //            if (mapInfo.IsInClause)
        //            {
        //                PropertyInfo prop = condition.GetType().GetProperty(key + "List");
        //                if (prop == null) { continue; }

        //                if (prop.PropertyType == typeof(List<decimal>))
        //                {
        //                    List<decimal> list = new List<decimal>();
        //                    if (value.Contains(ComUtil.FromToDelimiter))
        //                    {
        //                        var data = value.Split(ComUtil.FromToDelimiter);
        //                        for (int i = 0; i < data.Count(); i++)
        //                        {
        //                            list.Add(decimal.Parse(data[i]));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        list.Add(decimal.Parse(value));
        //                    }
        //                    setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                    setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
        //                    continue;
        //                }
        //                else if (prop.PropertyType == typeof(List<int>))
        //                {
        //                    List<int> list = new List<int>();
        //                    if (value.Contains(ComUtil.FromToDelimiter))
        //                    {
        //                        var data = value.Split(ComUtil.FromToDelimiter);
        //                        for (int i = 0; i < data.Count(); i++)
        //                        {
        //                            list.Add(int.Parse(data[i]));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        list.Add(int.Parse(value));
        //                    }
        //                    setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                    setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
        //                    continue;
        //                }
        //                else if (prop.PropertyType == typeof(List<long>))
        //                {
        //                    List<long> list = new List<long>();
        //                    if (value.Contains(ComUtil.FromToDelimiter))
        //                    {
        //                        var data = value.Split(ComUtil.FromToDelimiter);
        //                        for (int i = 0; i < data.Count(); i++)
        //                        {
        //                            list.Add(long.Parse(data[i]));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        list.Add(long.Parse(value));
        //                    }
        //                    setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                    setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
        //                    continue;
        //                }
        //                else if (prop.PropertyType == typeof(List<string>))
        //                {
        //                    List<string> list = new List<string>();
        //                    if (value.Contains(ComUtil.FromToDelimiter))
        //                    {
        //                        var data = value.Split(ComUtil.FromToDelimiter);
        //                        for (int i = 0; i < data.Count(); i++)
        //                        {
        //                            list.Add(data[i]);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        list.Add(value);
        //                    }
        //                    setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                    setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
        //                    continue;
        //                }
        //                else if (prop.PropertyType == typeof(List<DateTime>))
        //                {
        //                    List<DateTime> list = new List<DateTime>();
        //                    if (value.Contains(ComUtil.FromToDelimiter))
        //                    {
        //                        var data = value.Split(ComUtil.FromToDelimiter);
        //                        for (int i = 0; i < data.Count(); i++)
        //                        {
        //                            list.Add(DateTime.Parse(data[i]));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        list.Add(DateTime.Parse(value));
        //                    }
        //                    setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                    setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
        //                    continue;
        //                }
        //                else
        //                {
        //                    List<object> list = new List<object>();
        //                    if (value.Contains(ComUtil.FromToDelimiter))
        //                    {
        //                        var data = value.Split(ComUtil.FromToDelimiter);
        //                        for (int i = 0; i < data.Count(); i++)
        //                        {
        //                            list.Add(data[i]);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        list.Add(value);
        //                    }
        //                    setValueToClassCon(mapInfo, key + "List", list, condition, convType);
        //                    setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
        //                    continue;
        //                }
        //            }

        //            // セルタイプが数値で 数値+単位分割文字が設定されている場合
        //            if (mapInfo.CtrlType == LISTITEM_DEFINE_CONSTANTS.CELLTYPE.Number && value.Contains(ComUtil.NumberUnitDelimiter))
        //            {
        //                var values = value.Split(ComUtil.NumberUnitDelimiter);
        //                setValueToClassCon(mapInfo, key, values[0], condition, convType);
        //                continue;
        //            }

        //            setValueToClassCon(mapInfo, key, val, condition, convType);
        //        }
        //    }
        //    return true;
        //}
        // ↑↑↑2022/12/13 CommonSTDUtilへ移植↑↑↑

        /// <summary>
        /// ページ情報を一時テーブルに保存する
        /// </summary>
        public void registTmpTable()
        {
            // 結果情報が存在しない場合、処理を戻す
            if (this.resultInfoDictionary.Count < 0) { return; }

            // ページ情報を取得
            foreach (var pageInfo in this.pageInfoList)
            {
                // コントロールタイプが"102"以外の場合、処理をスキップする
                if (pageInfo.CtrlType != FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn2) { continue; }

                // 該当のコントロールIDの結果を取得
                var tmpList = this.resultInfoDictionary.Where(x => x["CTRLID"].Equals(pageInfo.CtrlId)).ToList();
                if (tmpList.Count < 1) { continue; } // 1件も存在しない場合、処理をスキップ

                // 明細データが存在する場合、更新行を取得
                int addRowCnt = 0;
                var updateList = GetUpdateRowDataList(tmpList, DateTime.Now, ref addRowCnt);
                if (updateList.Count > 0)
                {
                    // 更新行データを一時テーブルに保存
                    if (!UpdateSearchResultsOfTmpTable(updateList, addRowCnt, pageInfo))
                    {
                        // 処理が失敗した場合、処理をスキップする
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// 型付処理結果をDictionary型へ変換
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="result">処理結果(データ型付)</param>
        /// <param name="cnt">総件数</param>
        /// <param name="startRowNo">開始行番号</param>
        /// <returns>Dictionary型格納リスト</returns>
        protected List<Dictionary<string, object>> convertDataClassToResultDictionary<T>(string ctrlId, T result, int cnt, int startRowNo = 0)
        {
            // 編集行で呼び出し
            return convertDataClassToResultDictionaryCommon<T>(TMPTBL_CONSTANTS.ROWSTATUS.Edit, ctrlId, result, cnt, startRowNo);
        }

        /// <summary>
        /// 型付処理結果をDictionary型へ変換(新規データ)
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="result">処理結果(データ型付)</param>
        /// <param name="cnt">総件数</param>
        /// <param name="startRowNo">開始行番号</param>
        /// <returns>Dictionary型格納リスト</returns>
        protected List<Dictionary<string, object>> convertDataClassToDictionaryForNewData<T>(string ctrlId, T result, int cnt, int startRowNo = 0)
        {
            // 追加行で呼び出し
            return convertDataClassToResultDictionaryCommon<T>(TMPTBL_CONSTANTS.ROWSTATUS.New, ctrlId, result, cnt, startRowNo);
        }

        /// <summary>
        /// 型付処理結果をDictionary型へ変換(共通)
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="rowStatus">追加する処理結果のステータス</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="result">処理結果(データ型付)</param>
        /// <param name="cnt">総件数</param>
        /// <param name="startRowNo">開始行番号</param>
        /// <returns>Dictionary型格納リスト</returns>
        private List<Dictionary<string, object>> convertDataClassToResultDictionaryCommon<T>(short rowStatus, string ctrlId, T result, int cnt, int startRowNo = 0)
        {
            // マッピング情報を取得
            var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();

            var list = new List<Dictionary<string, object>>();
            var dicList = new Dictionary<string, object>();
            var rowNo = startRowNo + 1;

            if (cnt >= 0)
            {
                // パラメータに総件数が設定されている場合、先頭に総件数行を追加
                dicList = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", 0 },
                    { "ROWSTATUS", rowStatus},
                    { "PAGENO", this.getPageNo(ctrlId) },
                    { "VAL1", cnt.ToString() }
                };
                list.Add(dicList);
            }
            else
            {
                rowNo = 1;
            }

            // 検索結果クラスのプロパティを列挙
            var properties = typeof(T).GetProperties();

            // 初期化
            dicList = new Dictionary<string, object>
            {
                { "CTRLID", ctrlId },
                { "ROWNO", rowNo++ },
                { "ROWSTATUS",rowStatus }
            };

            foreach (var mapInfo in mappingList)
            {
                var paramName = mapInfo.ParamName;
                var format = mapInfo.Format;
                var prop = properties.FirstOrDefault(x => x.Name.ToUpper().Equals(paramName.ToUpper()));
                if (prop != null)
                {
                    var targetValue = prop.GetValue(result);
                    // フォーマット指定されている場合、処理を行う
                    if (!string.IsNullOrEmpty(format))
                    {
                        if (targetValue is DateTime)
                        {
                            DateTime val = (DateTime)prop.GetValue(result);
                            dicList.Add(mapInfo.ValName, val.ToString(format));
                        }
                        else if (targetValue is decimal)
                        {
                            decimal val = (decimal)prop.GetValue(result);
                            dicList.Add(mapInfo.ValName, val.ToString(format));
                        }
                        else
                        {
                            // 指定したパラメータ名が検索結果クラスに存在する場合、結果オブジェクトへ追加
                            dicList.Add(mapInfo.ValName, prop.GetValue(result));
                        }
                    }
                    else
                    {
                        // 指定したパラメータ名が検索結果クラスに存在する場合、結果オブジェクトへ追加
                        dicList.Add(mapInfo.ValName, prop.GetValue(result));
                    }
                }
            }

            list.Add(dicList);
            return list;
        }

        /// <summary>
        /// 型付処理結果をDictionary型へ変換
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="result">処理結果(データ型付)</param>
        /// <param name="cnt">総件数</param>
        /// <param name="startRowNo">開始行番号</param>
        /// <returns>Dictionary型格納リスト</returns>
        protected List<Dictionary<string, object>> convertDataClassToResultDictionary<T>(string ctrlId, IList<T> results, int cnt, int startRowNo = 0)
        {
            // マッピング情報を取得
            var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();

            var list = new List<Dictionary<string, object>>();
            var dicList = new Dictionary<string, object>();
            var rowNo = startRowNo + 1;

            if (cnt >= 0)
            {
                // パラメータに総件数が設定されている場合、先頭に総件数行を追加
                dicList = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", 0 },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit },
                    { "PAGENO", this.getPageNo(ctrlId) },
                    { "VAL1", cnt.ToString() }
                };
                list.Add(dicList);
            }
            else
            {
                rowNo = 1;
            }

            // 検索結果クラスのプロパティを列挙
            var properties = typeof(T).GetProperties();

            foreach (var result in results)
            {
                // 初期化
                dicList = new Dictionary<string, object>
                {
                    { "CTRLID", ctrlId },
                    { "ROWNO", rowNo++ },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit }
                };

                foreach (var mapInfo in mappingList)
                {
                    var paramName = mapInfo.ParamName;
                    var format = mapInfo.Format;
                    var prop = properties.FirstOrDefault(x => x.Name.ToUpper().Equals(paramName.ToUpper()));
                    if (prop != null)
                    {
                        var targetValue = prop.GetValue(result);
                        // フォーマット指定されている場合、処理を行う
                        if (!string.IsNullOrEmpty(format))
                        {
                            if (targetValue is DateTime)
                            {
                                DateTime val = (DateTime)prop.GetValue(result);
                                dicList.Add(mapInfo.ValName, val.ToString(format));
                            }
                            else if (targetValue is decimal)
                            {
                                decimal val = (decimal)prop.GetValue(result);
                                dicList.Add(mapInfo.ValName, val.ToString(format));
                            }
                            else
                            {
                                // 指定したパラメータ名が検索結果クラスに存在する場合、結果オブジェクトへ追加
                                dicList.Add(mapInfo.ValName, prop.GetValue(result));
                            }
                        }
                        else
                        {
                            // 指定したパラメータ名が検索結果クラスに存在する場合、結果オブジェクトへ追加
                            dicList.Add(mapInfo.ValName, prop.GetValue(result));
                        }
                    }
                }
                list.Add(dicList);
            }

            return list;
        }

        /// <summary>
        /// データクラスに値を設定する
        /// </summary>
        /// <param name="mappingInfo">マッピング情報</param>
        /// <param name="key">キー情報</param>
        /// <param name="val">設定値</param>
        /// <param name="condition">条件データ</param>
        /// <param name="convType">検索条件</param>
        protected void setValueToClassCon(ComUtil.DBMappingInfo mappingInfo, string key, object val, dynamic condition, ComUtil.ConvertType convType)
        {
            // プロパティを取得
            PropertyInfo property = condition.GetType().GetProperty(key);
            if (property == null) { return; }

            // 文字型で検索条件の場合、マッチパターン指定
            if (!CommonUtil.IsNullOrEmpty(val) && property.PropertyType == typeof(string) && convType == ComUtil.ConvertType.Search)
            {
                switch (mappingInfo.LikePatternEnum)
                {
                    case MatchPattern.ForwardMatch:
                        val = val + "%";
                        break;
                    case MatchPattern.BackwardMatch:
                        val = "%" + val;
                        break;
                    case MatchPattern.PartialMatch:
                        val = "%" + val + "%";
                        break;
                    default:
                        break;
                }
            }

            if (property != null)
            {
                ComUtil.SetPropertyValue(property, condition, val);
            }
            return;
        }

        ///// <summary>
        ///// Dictionary型のリストを型付リストへ変換
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="ctrlId"></param>
        ///// <param name="dicList"></param>
        ///// <returns></returns>
        //protected List<T> ConvertDictionaryToClass<T>(string ctrlId, List<Dictionary<string, object>> dicList) where T : new()
        //{
        //    // マッピング情報を取得
        //    var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();

        //    // 検索結果クラスのプロパティを列挙
        //    var properties = typeof(T).GetProperties();

        //    var list = new List<T>();
        //    foreach(var dic in dicList)
        //    {

        //    }
        //    return list;
        //}

        // ↓↓↓2022/12/13 CommonSTDUtilへ移植↓↓↓
        ///// <summary>
        ///// 条件の設定
        ///// </summary>
        ///// <param name="list">変換対象辞書リスト</param>
        ///// <param name="mappingList">マッピング情報リスト</param>
        ///// <param name="condition">条件オブジェクト</param>
        ///// <param name="convType">変換対象条件種類(検索/実行/検索結果)</param>
        ///// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        //private bool setConditionByDataClass(string ctrlId, List<Dictionary<string, object>> list, dynamic condition, ComUtil.ConvertType convType, List<string> paramList = null)
        //{
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        if (list.Count > i)
        //        {
        //            setConditionByDataClass(ctrlId, condition, list[i], convType, paramList);
        //        }
        //    }
        //    // 言語ID
        //    condition.LanguageId = this.LanguageId;

        //    return true;
        //}
        // ↑↑↑2022/12/13 CommonSTDUtilへ移植↑↑↑

        /// <summary>
        /// 条件を個別に設定
        /// </summary>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="paramName">条件のキー</param>
        /// <param name="value">条件に設定する値</param>
        /// <returns>true</returns>
        /// <remarks>型変換を行わないのでvalueの型は適切に設定すること</remarks>
        protected bool setConditionObj(dynamic condition, string paramName, object value)
        {
            // conditionをDictionaryにキャスト
            IDictionary<string, object> dicCondition = condition;
            // 追加
            dicCondition.Add(paramName, value);
            return true;
        }

        /// <summary>
        /// 検索条件の設定
        /// </summary>
        /// <param name="list">変換対象辞書リスト</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        protected bool SetSearchCondition(List<Dictionary<string, object>> list, string ctrlId, dynamic condition,
            PageInfo pageInfo, List<string> paramList = null)
        {
            setConditionObj(ctrlId, list, condition, ComUtil.ConvertType.Search, paramList);

            if (IsRequiredPageInfoSetting(pageInfo))
            {
                // ページ情報指定が必要な場合、実行条件に追加
                condition.PageSize = pageInfo.PageSize;
                condition.PageNumber = pageInfo.PageNo;
                condition.Offset = pageInfo.Offset;
            }

            return true;
        }

        /// <summary>
        /// 検索条件の設定
        /// </summary>
        /// <param name="list">変換対象辞書リスト</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        protected bool SetSearchConditionByDataClass(List<Dictionary<string, object>> list, string ctrlId, dynamic condition, PageInfo pageInfo, List<string> paramList = null)
        {
            ComUtil.SetConditionByDataClass(ctrlId, this.mapInfoList, list, condition, ComUtil.ConvertType.Search, this.LanguageId, paramList);

            if (IsRequiredPageInfoSetting(pageInfo))
            {
                // ページ情報指定が必要な場合、実行条件に追加
                condition.PageSize = pageInfo.PageSize;
                condition.PageNumber = pageInfo.PageNo;
                condition.Offset = pageInfo.Offset;
            }

            return true;
        }

        /// <summary>
        /// 実行条件の設定
        /// </summary>
        /// <param name="dic">変換対象辞書</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="date">実行日時</param>
        /// <param name="updatorCd">更新者ID</param>
        /// <param name="inputorCd">登録者ID</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        protected bool SetExecuteCondition(Dictionary<string, object> dic, string ctrlId, dynamic condition, DateTime date,
            string updatorCd, string inputorCd = "", List<string> paramList = null)
        {
            setConditionObj(ctrlId, dic, condition, ComUtil.ConvertType.Execute, paramList);

            // 更新者IDと更新日時を設定
            condition.UpdateDate = date;
            condition.UpdatorCd = updatorCd;
            if (!string.IsNullOrEmpty(inputorCd))
            {
                // 登録者IDと登録日時を設定
                condition.InputDate = date;
                condition.InputorCd = inputorCd;
            }

            return true;
        }

        /// <summary>
        /// 実行条件の設定
        /// </summary>
        /// <typeparam name="T">実行条件の型、更新日時などを設定するためテーブル共通クラスの継承が必要</typeparam>
        /// <param name="dic">変換対象辞書</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="date">実行日時</param>
        /// <param name="updatorCd">更新者ID　内部で数値に変換</param>
        /// <param name="inputorCd">登録者ID　内部で数値に変換　省略可能</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        /// <returns>エラーの場合False</returns>
        protected bool SetExecuteConditionByDataClass<T>(Dictionary<string, object> dic, string ctrlId, T condition, DateTime date,
            string updatorCd, string inputorCd = "-1", List<string> paramList = null) where T : CommonDataBaseClass.CommonTableItem
        {
            // 更新者ID、登録者IDの変換
            bool chkUpd = int.TryParse(updatorCd, out int updatorCdNum);
            bool chkInp = int.TryParse(inputorCd, out int inputorCdNum);
            // いずれかが変換エラーの場合、エラーを返す
            if (!chkUpd || !chkInp) { return false; }
            ComUtil.SetConditionByDataClass(ctrlId, this.mapInfoList, condition, dic, ComUtil.ConvertType.Execute, paramList);
            // 共通の更新日時などを設定
            setExecuteConditionByDataClassCommon<T>(ref condition, date, updatorCdNum, inputorCdNum);

            return true;
        }

        /// <summary>
        /// 実行条件の設定で、共通の更新日時などを設定する処理
        /// </summary>
        /// <typeparam name="T">実行条件の型、更新日時などを設定するためテーブル共通クラスの継承が必要</typeparam>
        /// <param name="condition">ref 実行条件を設定するデータクラス</param>
        /// <param name="date">実行日時</param>
        /// <param name="updatorCd">更新者ID</param>
        /// <param name="inputorCd">登録者ID</param>
        protected void setExecuteConditionByDataClassCommon<T>(ref T condition, DateTime date, int updatorCd, int inputorCd)
            where T : CommonDataBaseClass.CommonTableItem
        {
            // 更新者IDと更新日時を設定
            condition.UpdateDatetime = date;
            condition.UpdateUserId = updatorCd;
            if (inputorCd != -1)
            {
                // 登録者IDと登録日時、更新シリアルIDを設定
                condition.InsertDatetime = date;
                condition.InsertUserId = inputorCd;
                condition.UpdateSerialid = 0;
            }
        }

        /// <summary>
        /// 実行条件の設定
        /// </summary>
        /// <typeparam name="T">実行条件の型、更新日時などを設定するためテーブル共通クラスの継承が必要</typeparam>
        /// <param name="dic">変換対象辞書</param>
        /// <param name="mappingList">マッピング情報リスト</param>
        /// <param name="condition">条件を設定するデータクラス</param>
        /// <param name="date">実行日時</param>
        /// <param name="updatorCd">更新者ID　内部で数値に変換</param>
        /// <param name="inputorCd">登録者ID　内部で数値に変換　省略可能</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        /// <returns>エラーの場合False</returns>
        protected bool SetExecuteConditionByDataClass<T>(Dictionary<string, object> dic, List<ComUtil.DBMappingInfo> mappingList, T condition, DateTime date,
            string updatorCd, string inputorCd = "-1", List<string> paramList = null) where T : CommonDataBaseClass.CommonTableItem
        {
            // 更新者ID、登録者IDの変換
            bool chkUpd = int.TryParse(updatorCd, out int updatorCdNum);
            bool chkInp = int.TryParse(inputorCd, out int inputorCdNum);
            // いずれかが変換エラーの場合、エラーを返す
            if (!chkUpd || !chkInp) { return false; }
            ComUtil.SetConditionByDataClass(mappingList, condition, dic, ComUtil.ConvertType.Execute, paramList);
            // 共通の更新日時などを設定
            setExecuteConditionByDataClassCommon<T>(ref condition, date, updatorCdNum, inputorCdNum);

            return true;
        }

        /// <summary>
        /// 削除条件の設定
        /// </summary>
        /// <param name="dic">変換対象辞書</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        protected bool SetDeleteCondition(Dictionary<string, object> dic, string ctrlId, dynamic condition, List<string> paramList = null)
        {
            setConditionObj(ctrlId, dic, condition, ComUtil.ConvertType.Execute, paramList);

            return true;
        }

        /// <summary>
        /// 削除条件の設定
        /// </summary>
        /// <param name="dic">変換対象辞書</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        protected bool SetDeleteConditionByDataClass(Dictionary<string, object> dic, string ctrlId, dynamic condition, List<string> paramList = null)
        {
            ComUtil.SetConditionByDataClass(ctrlId, this.mapInfoList, condition, dic, ComUtil.ConvertType.Execute, paramList);

            return true;
        }

        /// <summary>
        /// Dictionaryより対応する内容をDAOクラスへセット
        /// </summary>
        /// <param name="dic">変換対象辞書</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="target">辞書の内容をセットするDAOクラス</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        protected bool SetDataClassFromDictionary(Dictionary<string, object> dic, string ctrlId, dynamic target, List<string> paramList = null)
        {
            ComUtil.SetConditionByDataClass(ctrlId, this.mapInfoList, target, dic, ComUtil.ConvertType.Result, paramList);

            return true;
        }

        /// <summary>
        /// 総件数チェック
        /// </summary>
        /// <param name="cnt"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        protected bool CheckSearchTotalCount(int cnt, PageInfo pageInfo)
        {
            if (cnt <= 0) { return false; }

            if (!this.NeedsTotalCntCheck) { return true; }

            if (cnt > 0)
            {
                if (pageInfo != null && pageInfo.MaxCnt > 0)
                {
                    if (cnt > pageInfo.MaxCnt)
                    {
                        // MAX件数を超えている場合、確認メッセージを設定して処理を終了
                        this.Status = CommonProcReturn.ProcStatus.Confirm;
                        // 「データ件数が最大件数を超えています。表示しますか？」
                        //this.MsgId = GetResMessage("941190001");
                        this.MsgId = GetResMessage(new string[] { "941190003", pageInfo.MaxCnt.ToString() });
                        this.LogNo = "999";
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// dynamic型検索結果の設定
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="results">検索結果</param>
        /// <param name="cnt">総件数</param>
        /// <returns></returns>
        protected bool SetSearchResults(PageInfo pageInfo, dynamic results, int cnt)
        {
            // 検索結果を一時テーブルレイアウトへ変換
            var tmpList = ConvertResultsToTmpTableList(pageInfo, results);
            if (tmpList == null) { return false; }

            // OUTパラメータへ検索結果を設定
            return setSearchResults(pageInfo, tmpList, cnt);
        }

        /// <summary>
        /// 型付検索結果の設定
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="results">検索結果</param>
        /// <param name="cnt">総件数</param>
        /// <param name="isDetailConditionApplied">詳細検索条件適用フラグ</param>
        /// <returns></returns>
        protected bool SetSearchResultsByDataClass<T>(PageInfo pageInfo, IList<T> results, int cnt, bool isDetailConditionApplied = false)
        {
            // 検索結果を一時テーブルレイアウトへ変換
            var tmpList = ConvertResultsToTmpTableListByDataClass(pageInfo, results);
            //if (tmpList == null) { return false; }
            // 一時テーブル保存が必要かどうか
            if (pageInfo.CtrlType != FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn2)
            {
                // 一時テーブル保存が不必要な場合
                // OUTパラメータへ検索結果を設定
                return setSearchResults2(pageInfo, tmpList, cnt, isDetailConditionApplied);
            }
            else
            {
                // 一時テーブル保存が必要な場合
                // OUTパラメータへ検索結果を設定
                return setSearchResults(pageInfo, tmpList, cnt);
            }
        }

        /// <summary>
        /// 型付検索結果の設定
        /// </summary>
        /// <param name="results">ページ情報/検索結果</param>
        /// <returns></returns>
        protected bool SetSearchResultsByDataClass(Dictionary<PageInfo, IList<ExpandoObject>> results)
        {
            List<ExpandoObject> list = new List<ExpandoObject>();
            foreach (var result in results)
            {
                PageInfo pageInfo = result.Key;
                IList<ExpandoObject> tmpResults = result.Value;
                // 検索結果を一時テーブルレイアウトへ変換
                var tmpList = ConvertResultsToTmpTableListByDataClass(pageInfo, tmpResults);
                if (tmpList == null) { return false; }

                // OUTパラメータへ検索結果を設定
                List<ExpandoObject> subList = setDetailResults(pageInfo, tmpList, tmpResults.Count);
                if (subList == null) { return false; }
                list.AddRange(subList);
            }

            if (list == null || list.Count == 0) { return false; }

            SetJsonResult(list);
            return true;
        }

        /// <summary>
        /// 検索結果の設定
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="list">検索結果リスト</param>
        /// <returns>処理結果(true:成功/false:失敗)</returns>
        private bool setSearchResults(PageInfo pageInfo, List<ExpandoObject> list, int totalCnt)
        {
            if (list == null) { return false; }

            // 画面表示情報に排他ロック用JSON文字列を設定 ※一時テーブル格納のため、ロック情報を設定
            SetExclusiveInfo(list, pageInfo.CtrlId);

            if (IsRequiredRegistTmpTable(pageInfo))
            {
                // 一時テーブルへ登録
                if (!RegistSearchResultsToTmpTable(pageInfo, list))
                {
                    return false;
                }
            }

            // 該当ページデータの抽出
            if (IsRequiredPageInfoSetting(pageInfo, false))
            {
                // ページ情報指定が必要な場合、該当ページデータのみ抽出する
                int startIdx = pageInfo.PageSize * (pageInfo.PageNo - 1);
                int lastIdx = startIdx + pageInfo.PageSize - 1;

                int cnt = list.Count > lastIdx ? pageInfo.PageSize : pageInfo.PageSize - (lastIdx - list.Count + 1);
                list = list.GetRange(startIdx, cnt);
            }

            if (totalCnt >= 0)
            {
                // パラメータに総件数が設定されている場合、先頭に総件数行を追加
                dynamic pramObj = new ExpandoObject();
                pramObj.GUID = this.GUID;
                pramObj.TABNO = this.BrowserTabNo;
                pramObj.CTRLID = pageInfo.CtrlId;
                pramObj.ROWNO = 0;
                pramObj.ROWSTATUS = TMPTBL_CONSTANTS.ROWSTATUS.Edit;
                pramObj.PAGENO = this.getPageNo(pageInfo.CtrlId);
                pramObj.VAL1 = totalCnt.ToString();
                list.Insert(0, pramObj);
            }

            //// 画面表示情報に排他ロック用JSON文字列を設定
            //SetExclusiveInfo(list, pageInfo.CtrlId);

            // 検索結果の設定
            SetJsonResult(list);

            return true;
        }

        /// <summary>
        /// 検索結果の設定
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="list">検索結果リスト</param>
        /// <param name="isDetailConditionApplied">詳細検索条件適用フラグ</param>
        /// <returns>処理結果(true:成功/false:失敗)</returns>
        /// <remarks>一時テーブル更新不要の際、こちらを使用</remarks>
        private bool setSearchResults2(PageInfo pageInfo, List<ExpandoObject> list, int totalCnt, bool isDetailConditionApplied = false)
        {
            if (totalCnt == 0)
            {
                list = new List<ExpandoObject>();
            }

            // 先頭に総件数行を追加
            dynamic pramObj = new ExpandoObject();
            pramObj.GUID = this.GUID;
            pramObj.TABNO = this.BrowserTabNo;
            pramObj.CTRLID = pageInfo.CtrlId;
            pramObj.ROWNO = 0;
            pramObj.ROWSTATUS = TMPTBL_CONSTANTS.ROWSTATUS.Edit;
            pramObj.PAGENO = this.getPageNo(pageInfo.CtrlId);
            pramObj.VAL1 = totalCnt.ToString();
            if (isDetailConditionApplied)
            {
                pramObj.IsDetailConditionApplied = isDetailConditionApplied;
            }
            list.Insert(0, pramObj);

            if (totalCnt > 0)
            {
                // 画面表示情報に排他ロック用JSON文字列を設定
                SetExclusiveInfo(list, pageInfo.CtrlId);
            }

            // 検索結果の設定
            SetJsonResult(list);

            return true;
        }

        /// <summary>
        /// 検索結果の設定
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="list">検索結果リスト</param>
        /// <returns>処理結果(true:成功/false:失敗)</returns>
        private List<ExpandoObject> setDetailResults(PageInfo pageInfo, List<ExpandoObject> list, int totalCnt)
        {
            if (IsRequiredRegistTmpTable(pageInfo))
            {
                // 一時テーブルへ登録
                if (!RegistSearchResultsToTmpTable(pageInfo, list))
                {
                    return null;
                }
            }

            // 該当ページデータの抽出
            if (IsRequiredPageInfoSetting(pageInfo, false))
            {
                // ページ情報指定が必要な場合、該当ページデータのみ抽出する
                int startIdx = pageInfo.PageSize * (pageInfo.PageNo - 1);
                int lastIdx = startIdx + pageInfo.PageSize - 1;

                int cnt = list.Count > lastIdx ? pageInfo.PageSize : pageInfo.PageSize - (lastIdx - list.Count + 1);
                list = list.GetRange(startIdx, cnt);
            }

            if (totalCnt >= 0)
            {
                // パラメータに総件数が設定されている場合、先頭に総件数行を追加
                dynamic pramObj = new ExpandoObject();
                pramObj.GUID = this.GUID;
                pramObj.TABNO = this.BrowserTabNo;
                pramObj.CTRLID = pageInfo.CtrlId;
                pramObj.ROWNO = 0;
                pramObj.ROWSTATUS = TMPTBL_CONSTANTS.ROWSTATUS.Edit;
                pramObj.PAGENO = this.getPageNo(pageInfo.CtrlId);
                pramObj.VAL1 = totalCnt.ToString();
                list.Insert(0, pramObj);
            }

            // 画面表示情報に排他ロック用JSON文字列を設定
            SetExclusiveInfo(list, pageInfo.CtrlId);

            return list;
        }

        /// <summary>
        /// 検索結果の設定
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="list">検索結果リスト</param>
        /// <returns>処理結果(true:成功/false:失敗)</returns>
        private bool setTmpSearchResults<T>(PageInfo pageInfo, IList<T> list, int cnt)
        {
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            // 先頭に総件数行を追加
            var pramObj = new Dictionary<string, object> {
                { "GUID", this.GUID },
                { "TABNO", this.BrowserTabNo },
                { "CTRLID", pageInfo.CtrlId },
                { "ROWNO", 0 },
                { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit },
                { "PAGENO", this.getPageNo(pageInfo.CtrlId) },
                { "VAL1", cnt.ToString() }
            };
            resultList.Add(pramObj);

            var properties = typeof(T).GetProperties();
            foreach (var data in list)
            {
                var dic = new Dictionary<string, object>();
                foreach (var prop in properties)
                {
                    var obj = prop.GetValue(data);
                    dic.Add(prop.Name.ToUpper(), obj);
                }
                resultList.Add(dic);
            }
            // 画面表示情報に排他ロック用JSON文字列を設定
            SetExclusiveInfo(resultList, pageInfo.CtrlId);

            // 検索結果の設定
            SetJsonResult(resultList);

            return true;
        }

        /// <summary>
        /// dynamic型の検索結果をDictionary型へ変換
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="condition">検索条件</param>
        /// <param name="results">検索結果</param>
        /// <param name="cnt">総件数</param>
        /// <param name="startRowNo">開始行番号</param>
        /// <returns></returns>
        protected List<Dictionary<string, object>> ConvertToSearchResultDictionary(string ctrlId, dynamic condition, dynamic results, int cnt = -1, int startRowNo = 0)
        {
            var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();
            var tblNameList = mappingList.Where(x => !string.IsNullOrEmpty(x.TblName)).Select(x => x.TblName).Distinct().ToList();
            //var colInfoDic = GetColumnInfoDictionary(tblNameList);

            var list = new List<Dictionary<string, object>>();
            var dicList = new Dictionary<string, object>();
            var rowNo = startRowNo + 1;
            int offset = 0;

            if (cnt >= 0)
            {
                // パラメータに総件数が設定されている場合、先頭に総件数行を追加
                dicList = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", 0 },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit },
                    { "PAGENO", this.getPageNo(ctrlId) },
                    { "VAL1", cnt.ToString() }
                };
                list.Add(dicList);

                // 検索条件からオフセット件数を取得
                if (condition != null)
                {
                    if (((IDictionary<string, object>)condition).ContainsKey("Offset"))
                    {
                        offset = condition.Offset;
                    }
                }
            }
            else
            {
                rowNo = 1;
            }

            foreach (var result in results)
            {
                dicList = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", offset + rowNo++ },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit }
                };

                foreach (string tblName in tblNameList)
                {
                    // テーブル毎に型変換を実行
                    var mapInfoList = mappingList.Where(x => x.TblName.Equals(tblName)).ToList();
                    //var colInfoList = colInfoDic[tblName];
                    //ComUtil.ConvertColumnType(mapInfoList, colInfoList, result, ComUtil.ConvertType.Result);
                    ComUtil.ConvertColumnType(mapInfoList, result, ComUtil.ConvertType.Result);
                }

                IDictionary<string, object> dicResult = result as IDictionary<string, object>;
                foreach (var mapInfo in mappingList)
                {
                    var colName = mapInfo.ColName;
                    if (dicResult.Any(x => x.Key.ToUpper().Equals(colName.ToUpper())))
                    {
                        // 指定したカラム名が検索結果に存在する場合、結果オブジェクトへ追加
                        dicList.Add(mapInfo.ValName, dicResult[colName]);
                    }
                }
                list.Add(dicList);
            }
            return list;
        }

        /// <summary>
        /// 型付検索結果をDictionary型へ変換
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="results">検索結果</param>
        /// <param name="mappingList">マッピング情報</param>
        /// <param name="cnt">総件数</param>
        /// <returns></returns>
        protected List<Dictionary<string, object>> ConvertDataClassToSearchResultDictionary<T>(string ctrlId, dynamic condition, IList<T> results, int cnt = -1, int startRowNo = 0)
        {
            var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();

            var list = new List<Dictionary<string, object>>();
            var dicList = new Dictionary<string, object>();
            var rowNo = startRowNo + 1;
            int offset = 0;

            if (cnt >= 0)
            {
                // パラメータに総件数が設定されている場合、先頭に総件数行を追加
                dicList = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", 0 },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit },
                    { "PAGENO", this.getPageNo(ctrlId) },
                    { "VAL1", cnt.ToString() }
                };
                list.Add(dicList);

                // 検索条件からオフセット件数を取得
                if (condition != null)
                {
                    Type type = condition.GetType();
                    PropertyInfo property = type.GetProperty("Offset");
                    if (property != null && property.GetValue(condition) != null)
                    {
                        offset = property.GetValue(condition);
                    }
                }
            }
            else
            {
                rowNo = 1;
            }

            // 検索結果クラスのプロパティを列挙
            var properties = typeof(T).GetProperties();

            foreach (var result in results)
            {
                dicList = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", offset + rowNo++ },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit }
                };

                foreach (var mapInfo in mappingList)
                {
                    var paramName = mapInfo.ParamName;
                    var format = mapInfo.Format;
                    var prop = properties.FirstOrDefault(x => x.Name.ToUpper().Equals(paramName.ToUpper()));
                    if (prop != null)
                    {
                        var targetValue = prop.GetValue(result);
                        // フォーマット指定されている場合、処理を行う
                        if (!string.IsNullOrEmpty(format))
                        {
                            if (targetValue is DateTime)
                            {
                                DateTime val = (DateTime)prop.GetValue(result);
                                dicList.Add(mapInfo.ValName, val.ToString(format));
                            }
                            else if (prop.GetValue(result) is decimal)
                            {
                                decimal val = (decimal)prop.GetValue(result);
                                prop.SetValue(result, val.ToString(format));
                            }
                            else
                            {
                                // 指定したパラメータ名が検索結果クラスに存在する場合、結果オブジェクトへ追加
                                dicList.Add(mapInfo.ValName, prop.GetValue(result));
                            }
                        }
                        else
                        {
                            // 指定したパラメータ名が検索結果クラスに存在する場合、結果オブジェクトへ追加
                            dicList.Add(mapInfo.ValName, prop.GetValue(result));
                        }
                    }
                }
                list.Add(dicList);
            }
            return list;
        }

        /// <summary>
        /// 型付検索結果をDictionary型へ変換
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="result">検索結果</param>
        /// <param name="mappingList">マッピング情報</param>
        /// <param name="cnt">総件数</param>
        /// <returns></returns>
        protected List<Dictionary<string, object>> ConvertDataClassToSearchResultDictionary<T>(string ctrlId, dynamic condition, T result, int cnt = -1, int startRowNo = 0)
        {
            var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();

            var list = new List<Dictionary<string, object>>();
            var dicList = new Dictionary<string, object>();
            var rowNo = startRowNo + 1;
            int offset = 0;

            if (cnt >= 0)
            {
                // パラメータに総件数が設定されている場合、先頭に総件数行を追加
                dicList = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", 0 },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit },
                    { "PAGENO", this.getPageNo(ctrlId) },
                    { "VAL1", cnt.ToString() }
                };
                list.Add(dicList);

                // 検索条件からオフセット件数を取得
                if (condition != null)
                {
                    Type type = condition.GetType();
                    PropertyInfo property = type.GetProperty("Offset");
                    if (property != null && property.GetValue(condition) != null)
                    {
                        offset = property.GetValue(condition);
                    }
                }
            }
            else
            {
                rowNo = 1;
            }

            // 検索結果クラスのプロパティを列挙
            var properties = typeof(T).GetProperties();

            dicList = new Dictionary<string, object> {
                { "CTRLID", ctrlId },
                { "ROWNO", offset + rowNo++ },
                { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit }
            };

            foreach (var mapInfo in mappingList)
            {
                var paramName = mapInfo.ParamName;
                var format = mapInfo.Format;
                var prop = properties.FirstOrDefault(x => x.Name.ToUpper().Equals(paramName.ToUpper()));
                if (prop != null)
                {
                    var targetValue = prop.GetValue(result);
                    // フォーマット指定されている場合、処理を行う
                    if (!string.IsNullOrEmpty(format))
                    {
                        if (targetValue is DateTime)
                        {
                            DateTime val = (DateTime)prop.GetValue(result);
                            dicList.Add(mapInfo.ValName, val.ToString(format));
                        }
                        else if (prop.GetValue(result) is decimal)
                        {
                            decimal val = (decimal)prop.GetValue(result);
                            prop.SetValue(result, val.ToString(format));
                        }
                        else
                        {
                            // 指定したパラメータ名が検索結果クラスに存在する場合、結果オブジェクトへ追加
                            dicList.Add(mapInfo.ValName, prop.GetValue(result));
                        }
                    }
                    else
                    {
                        // 指定したパラメータ名が検索結果クラスに存在する場合、結果オブジェクトへ追加
                        dicList.Add(mapInfo.ValName, prop.GetValue(result));
                    }
                }
            }

            list.Add(dicList);
            return list;
        }

        /// <summary>
        /// dynamic型の検索結果を一時テーブルレイアウトデータへ変換
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="results">検索結果</param>
        /// <param name="mappingList">マッピング情報</param>
        /// <returns></returns>
        protected List<ExpandoObject> ConvertResultsToTmpTableList(PageInfo pageInfo, dynamic results)
        {
            var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(pageInfo.CtrlId)).ToList();
            //var tblNameList = mappingList.Where(x => !string.IsNullOrEmpty(x.TblName)).Select(x => x.TblName).Distinct().ToList();
            //var colInfoDic = GetColumnInfoDictionary(tblNameList);

            var list = new List<ExpandoObject>();
            dynamic pramObj;
            var rowNo = 1;
            var updateDate = DateTime.Now;

            foreach (var result in results)
            {
                pramObj = new ExpandoObject() as IDictionary<string, object>;
                pramObj.GUID = this.GUID;
                pramObj.TABNO = this.BrowserTabNo;
                pramObj.CTRLID = pageInfo.CtrlId;
                pramObj.ROWNO = rowNo++;
                pramObj.ROWSTATUS = TMPTBL_CONSTANTS.ROWSTATUS.Edit;

                IDictionary<string, object> dicResult = result as IDictionary<string, object>;
                foreach (var mapInfo in mappingList)
                {
                    var colName = mapInfo.ColName;
                    if (dicResult.Any(x => x.Key.ToUpper().Equals(colName.ToUpper())))
                    {
                        // 指定したカラム名が検索結果に存在する場合、結果オブジェクトへ追加
                        var val = !CommonUtil.IsNullOrEmpty(dicResult[colName]) ? dicResult[colName] : "";
                        ((IDictionary<string, object>)pramObj).Add(mapInfo.ValName, val);
                    }
                }
                if (IsRequiredRegistTmpTable(pageInfo))
                {
                    pramObj.user_id = this.UserId;
                    pramObj.update_date = updateDate;
                }

                list.Add(pramObj);
            }
            return list;
        }

        /// <summary>
        /// 型付検索結果を一時テーブルレイアウトデータへ変換
        /// </summary>
        /// <param name="ctrlId">ページ情報</param>
        /// <param name="results">検索結果</param>
        /// <returns></returns>
        protected List<ExpandoObject> ConvertResultsToTmpTableListByDataClass<T>(PageInfo pageInfo, IList<T> results)
        {
            if (results == null) { return null; }

            var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(pageInfo.CtrlId)).ToList();

            var list = new List<ExpandoObject>();
            dynamic pramObj;
            var rowNo = 1;
            var updateDate = DateTime.Now;

            // 検索結果クラスのプロパティを列挙
            var properties = typeof(T).GetProperties();

            foreach (var result in results)
            {
                pramObj = new ExpandoObject() as IDictionary<string, object>;
                pramObj.GUID = this.GUID;
                pramObj.TABNO = this.BrowserTabNo;
                pramObj.CTRLID = pageInfo.CtrlId;
                pramObj.ROWNO = rowNo++;
                pramObj.ROWSTATUS = TMPTBL_CONSTANTS.ROWSTATUS.Edit;

                //IDictionary<string, object> dicResult = result as IDictionary<string, object>;
                foreach (var mapInfo in mappingList)
                {
                    var paramName = mapInfo.ParamName;
                    var prop = properties.FirstOrDefault(x => x.Name.ToUpper().Equals(paramName.ToUpper()));
                    if (prop != null)
                    {
                        // 指定したカラム名が検索結果に存在する場合、結果オブジェクトへ追加
                        // フォーマットが指定されている場合、処理を行う
                        if (!string.IsNullOrEmpty(mapInfo.Format))
                        {
                            if (prop.GetValue(result) is DateTime)
                            {
                                DateTime val = (DateTime)prop.GetValue(result);
                                ((IDictionary<string, object>)pramObj).Add(mapInfo.ValName, val.ToString(mapInfo.Format));
                            }
                            else if (prop.GetValue(result) is decimal)
                            {
                                decimal val = (decimal)prop.GetValue(result);
                                string formatVal = val.ToString(mapInfo.Format);
                                ((IDictionary<string, object>)pramObj).Add(mapInfo.ValName, formatVal == "" ? val : formatVal);
                            }
                            else
                            {
                                ((IDictionary<string, object>)pramObj).Add(mapInfo.ValName, prop.GetValue(result));
                            }
                        }
                        else
                        {
                            ((IDictionary<string, object>)pramObj).Add(mapInfo.ValName, prop.GetValue(result));
                        }
                    }
                }
                if (IsRequiredRegistTmpTable(pageInfo))
                {
                    pramObj.user_id = this.UserId;
                    pramObj.update_date = updateDate;
                }

                list.Add(pramObj);
            }
            return list;
        }

        /// <summary>
        /// 検索結果を一時テーブルへ登録
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="results">検索結果</param>
        /// <returns>実行結果(true:成功/false:失敗)</returns>
        protected bool RegistSearchResultsToTmpTable(PageInfo pageInfo, List<ExpandoObject> list)
        {
            // トランザクションが開始されているか判定を行う
            if (!this.db.IsExistsTransaction)
            {
                // 開始されていない場合
                return this.RegistSearchResultsToTmpTableMain1(pageInfo, list);
            }
            else
            {
                // 開始されている場合
                return this.RegistSearchResultsToTmpTableMain2(pageInfo, list);
            }
        }

        /// <summary>
        /// 検索結果を一時テーブルへ登録 ※トランザクションが開始されていない場合
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="list">検索結果</param>
        /// <returns>実行結果(true:成功/false:失敗)</returns>
        protected bool RegistSearchResultsToTmpTableMain1(PageInfo pageInfo, List<ExpandoObject> list)
        {
            try
            {
                int result = 0;

                // トランザクション開始
                this.db.BeginTransaction();
                try
                {
                    // 一時テーブルデータの削除
                    if (ComUtil.DeleteTmpTableData(this.db, this.UserId, -1, this.GUID, this.BrowserTabNo) < 0)
                    {
                        // 「検索処理に失敗しました。」
                        this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                        return false;
                    }

                    // リストが取得できた場合のみ中間テーブルに反映する
                    if (list != null && list.Count != 0)
                    {
                        // 登録用SQL文の生成
                        var sql = GetInsertSqlForTmpTable(list[0]);
                        foreach (var param in list)
                        {
                            // 登録処理実行
                            result = this.db.Regist(sql, param);
                            if (result <= 0)
                            {
                                // ロールバック
                                this.db.RollBack();

                                // 「検索処理に失敗しました。」
                                this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                                return false;
                            }
                        }
                    }
                    // コミット
                    this.db.Commit();
                }
                catch (Exception ex)
                {
                    // ロールバック
                    this.db.RollBack();

                    // 「検索処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                    writeErrorLog(this.MsgId, ex);
                    return false;
                }
                finally
                {
                    // トランザクション終了
                    this.db.EndTransaction();
                }

                return true;
            }
            catch (Exception ex)
            {
                // 「検索処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                writeErrorLog(this.MsgId, ex);
                return false;
            }
        }

        /// <summary>
        /// 検索結果を一時テーブルへ登録 ※トランザクションが開始されている場合
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="list">検索結果</param>
        /// <returns>実行結果(true:成功/false:失敗)</returns>
        protected bool RegistSearchResultsToTmpTableMain2(PageInfo pageInfo, List<ExpandoObject> list)
        {
            int result = 0;
            try
            {
                // 一時テーブルデータの削除
                if (ComUtil.DeleteTmpTableData(this.db, this.UserId, -1, this.GUID, this.BrowserTabNo) < 0)
                {
                    // 「検索処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                    return false;
                }

                // リストが取得できた場合のみ中間テーブルに反映する
                if (list != null && list.Count != 0)
                {
                    // 登録用SQL文の生成
                    var sql = GetInsertSqlForTmpTable(list[0]);
                    foreach (var param in list)
                    {
                        // 登録処理実行
                        result = this.db.Regist(sql, param);
                        if (result <= 0)
                        {
                            // 「検索処理に失敗しました。」
                            this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 「検索処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                writeErrorLog(this.MsgId, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新行データの取得
        /// </summary>
        /// <param name="list">登録データ</param>
        /// <returns>更新行データ</returns>
        protected List<ExpandoObject> GetUpdateRowDataList(List<Dictionary<string, object>> list, DateTime updateDate, ref int addRowCnt)
        {
            var conditionList = new List<ExpandoObject>();
            foreach (var cond in list)
            {
                if (!cond.ContainsKey("UPDTAG") || !cond["UPDTAG"].Equals(TMPTBL_CONSTANTS.UPDTAG.Input))
                {
                    // 編集中データ以外は対象外
                    continue;
                }
                if (cond.ContainsKey("ROWSTATUS") && cond["ROWSTATUS"].Equals(TMPTBL_CONSTANTS.ROWSTATUS.New))
                {
                    // 新規行データ数をインクリメント
                    addRowCnt++;
                }

                dynamic conditionObj = new ExpandoObject();
                foreach (var item in cond)
                {
                    if (item.Key.ToUpper().Equals("UPDTAG"))
                    {
                        // UPDTAGの値を一時テーブル登録済へ更新
                        ((IDictionary<string, object>)conditionObj).Add(item.Key, TMPTBL_CONSTANTS.UPDTAG.UpdatedToTmp);
                    }
                    else if (item.Key.ToUpper().Equals("ROWNO"))
                    {
                        ((IDictionary<string, object>)conditionObj).Add(item.Key, Convert.ToInt32(item.Value));
                    }
                    else
                    {
                        ((IDictionary<string, object>)conditionObj).Add(item.Key, item.Value);
                    }
                }
                conditionObj.guid = this.GUID;
                conditionObj.tabno = this.BrowserTabNo;
                conditionObj.update_date = updateDate;

                conditionList.Add(conditionObj);
            }
            return conditionList;
        }

        /// <summary>
        /// 一時テーブルの検索結果を更新
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="results">検索結果</param>
        /// <returns>実行結果(true:成功/false:失敗)</returns>
        protected bool UpdateSearchResultsOfTmpTable(List<ExpandoObject> updateList, int addRowCnt, PageInfo pageInfo)
        {
            try
            {
                int result = 0;

                // トランザクション開始
                this.db.BeginTransaction();
                try
                {
                    string sql;
                    var updateSql = GetUpdateSqlForTmpTable(updateList[0]);
                    var insertSql = GetInsertSqlForTmpTable(updateList[0]);

                    if (addRowCnt > 0)
                    {
                        // 更新用SQL文の生成
                        sql = GetUpdateRowNoSqlForTmpTable();
                        // 更新対象ROWNO開始値を取得
                        int startRowNo = pageInfo.PageSize * (pageInfo.PageNo) + 1; // 現ページの次のページの1件目のROWNO
                        dynamic condition = updateList[0];
                        condition.addRowCnt = addRowCnt;
                        condition.startRowNo = startRowNo;

                        // 更新処理実行
                        result = this.db.Regist(sql, condition);
                        if (result < 0)
                        {
                            // ロールバック
                            this.db.RollBack();

                            // 「検索処理に失敗しました。」
                            this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                            return false;
                        }
                    }

                    foreach (var param in updateList)
                    {
                        // 更新処理実行
                        result = this.db.Regist(updateSql, param);
                        if (result == 0)
                        {
                            // 更新対象行が存在しない場合、登録処理実行
                            result = this.db.Regist(insertSql, param);
                        }
                        if (result <= 0)
                        {
                            // ロールバック
                            this.db.RollBack();

                            // 「検索処理に失敗しました。」
                            this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                            return false;
                        }
                    }
                    // コミット
                    this.db.Commit();
                }
                catch (Exception ex)
                {
                    // ロールバック
                    this.db.RollBack();

                    // 「検索処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                    writeErrorLog(this.MsgId, ex);
                    return false;
                }
                finally
                {
                    // トランザクション終了
                    this.db.EndTransaction();
                }

                return true;
            }
            catch (Exception ex)
            {
                // 「検索処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911090001" });
                writeErrorLog(this.MsgId, ex);
                return false;
            }
        }

        /// <summary>
        /// 一時テーブルからのデータ取得用SELECT文の取得
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="orderByColName">ソート対象カラム名</param>
        /// <returns></returns>
        protected string GetSelectSqlForTmpTable(PageInfo pageInfo, string sortColName)
        {
            var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(pageInfo.CtrlId)).ToList();

            // SQL文の生成
            StringBuilder sbSql = new StringBuilder();
            if (mappingList.Count > 0)
            {
                sbSql.AppendLine("select guid, tabno, ctrlid, rowno, rowstatus, updtag, seltag");
                // マッピング情報が登録されている列のみ取得する
                foreach (var mapInfo in mappingList)
                {
                    sbSql.Append(",").AppendLine(mapInfo.ValName);
                }
            }
            else
            {
                // マッピング情報が取得できない場合は全ての列を取得する
                sbSql.AppendLine("select *");
            }
            sbSql.AppendLine("from com_temp_data");
            sbSql.AppendLine("where guid = @GUID");
            sbSql.AppendLine("and tabno = @TabNo");
            sbSql.Append("order by ").AppendLine(sortColName);
            if (pageInfo.PageSize > 0 && pageInfo.PageNo > 0)
            {
                // 1ページ当たりの件数とページ番号が指定されている場合、該当ページデータのみ返す
                sbSql.AppendLine("FETCH NEXT @PageSize ROWS ONLY");
                sbSql.AppendLine("OFFSET @Offset ROWS");
            }

            return sbSql.ToString();
        }

        /// <summary>
        /// 一時テーブルへのデータ登録用INSERT文の取得
        /// </summary>
        /// <param name="obj">登録対象データ(カラム名取得用)</param>
        /// <returns></returns>
        protected string GetInsertSqlForTmpTable(ExpandoObject obj)
        {
            // SQL文の生成
            StringBuilder sbSql = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            sbSql.AppendLine("insert into com_temp_data (");
            sbValues.AppendLine(") values (");
            foreach (var item in obj)
            {
                sbSql.Append(item.Key).Append(",");
                sbValues.Append("@").Append(item.Key).Append(",");
            }
            sbSql = sbSql.Remove(sbSql.Length - 1, 1);
            sbValues = sbValues.Remove(sbValues.Length - 1, 1);
            sbValues.Append(")");
            sbSql.Append(sbValues);

            return sbSql.ToString();
        }

        /// <summary>
        /// 一時テーブルへのデータ更新用UPDATE文の取得
        /// </summary>
        /// <param name="obj">更新対象データ(カラム名取得用)</param>
        /// <returns></returns>
        protected string GetUpdateSqlForTmpTable(ExpandoObject obj)
        {
            // SQL文の生成
            StringBuilder sbSql = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            sbSql.AppendLine("update com_temp_data set ");
            foreach (var item in obj)
            {
                if (item.Key.ToUpper().StartsWith("VAL") ||
                    item.Key.ToUpper().StartsWith("SELTAG") ||
                    item.Key.ToUpper().Equals("UPDTAG") ||
                    item.Key.ToUpper().Equals("UPDATE_DATE") ||
                    item.Key.ToUpper().Equals("LOCKDATA"))
                {
                    sbSql.Append(item.Key).Append("=@").Append(item.Key).Append(",");
                }
            }
            sbSql = sbSql.Remove(sbSql.Length - 1, 1);
            sbSql.AppendLine("").Append("where guid=@guid and tabno=@tabno and rowno=@rowno");

            return sbSql.ToString();
        }

        /// <summary>
        /// 一時テーブルへのデータ更新用UPDATE文の取得
        /// </summary>
        /// <returns></returns>
        protected string GetUpdateRowNoSqlForTmpTable()
        {
            // SQL文の生成
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendLine("update com_temp_data set ");
            sbSql.AppendLine("rowno = rowno + @addRowCnt,");
            sbSql.AppendLine("update_date = @updateDate");
            sbSql.AppendLine("where guid=@guid and tabno=@tabno and rowno>@startRowNo");

            return sbSql.ToString();
        }

        /// <summary>
        /// Dictionary型のリストを型付リストへ変換
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="results">検索結果</param>
        /// <param name="dicList"> Dictionary型のリスト</param>
        /// <returns>型付リスト</returns>
        protected List<T> ConvertDictionaryToClass<T>(string ctrlId, List<Dictionary<string, object>> dicList) where T : new()
        {
            var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();

            // 検索結果クラスのプロパティを列挙
            var properties = typeof(T).GetProperties();

            var list = new List<T>();
            foreach (var dic in dicList)
            {
                T classData = new T();
                foreach (var data in dic)
                {
                    var paramName = mappingList.Where(x => x.ValName.Equals(data.Key)).Select(x => x.ParamName).FirstOrDefault();
                    if (string.IsNullOrEmpty(paramName)) { continue; }

                    var prop = properties.FirstOrDefault(x => x.Name.ToUpper().Equals(paramName.ToUpper()));
                    if (prop == null) { continue; }

                    // 該当する項目番号のデータがDictionaryに存在する場合、クラスのプロパティへ値を設定
                    ComUtil.SetPropertyValue(prop, classData, data.Value);
                }
                list.Add(classData);
            }
            return list;
        }

        /// <summary>
        /// カラム情報リストの取得
        /// </summary>
        /// <param name="tblName"></param>
        /// <returns></returns>
        private IList<ComUtil.DBColumnInfo> GetColumnInfoList(string tblName)
        {
            // 対象テーブルのカラム情報を取得
            var colInfo = this.db.GetListByOutsideSql<ComUtil.DBColumnInfo>(
                SqlName.GetColumnInfo, SqlName.SubDir, new { TableName = tblName });

            return colInfo;
        }

        /// <summary>
        /// カラム情報リスト辞書の取得
        /// </summary>
        /// <param name="tblNameList">テーブル名リスト</param>
        /// <returns></returns>
        private Dictionary<string, List<ComUtil.DBColumnInfo>> GetColumnInfoDictionary(List<string> tblNameList)
        {
            var dic = new Dictionary<string, List<ComUtil.DBColumnInfo>>();
            foreach (var tblName in tblNameList)
            {
                // 対象テーブルのカラム情報を取得
                var colInfo = GetColumnInfoList(tblName);
                if (colInfo != null)
                {
                    dic.Add(tblName, colInfo.ToList());
                }
            }
            return dic;
        }

        /// <summary>
        /// 排他ロックデータマッピング情報の取得
        /// </summary>
        /// <param name="ctrlId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<ComUtil.DBMappingInfo> getLockDataMaps(string ctrlId, LockType type)
        {
            return this.mapInfoList.Where(
                x => x.CtrlId == ctrlId && x.LockTypeEnum == type).ToList();
        }

        /// <summary>
        /// 排他ロック値マッピング情報の取得
        /// </summary>
        /// <param name="ctrlId"></param>
        /// <returns></returns>
        protected List<ComUtil.DBMappingInfo> GetLockValMaps(string ctrlId)
        {
            return getLockDataMaps(ctrlId, LockType.Value);
        }

        /// <summary>
        /// 排他ロックキーマッピング情報の取得
        /// </summary>
        /// <param name="ctrlId"></param>
        /// <returns></returns>
        protected List<ComUtil.DBMappingInfo> GetLockKeyMaps(string ctrlId)
        {
            return getLockDataMaps(ctrlId, LockType.Key);
        }

        /// <summary>
        /// 排他ロックデータマッピング情報の取得
        /// </summary>
        /// <param name="grpNo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<ComUtil.DBMappingInfo> getLockDataMapsByGrpNo(short grpNo, LockType type)
        {
            return this.mapInfoList.Where(
                x => x.GrpNo == grpNo && x.LockTypeEnum == type).ToList();
        }

        /// <summary>
        /// 排他ロック値マッピング情報の取得
        /// </summary>
        /// <param name="grpNo"></param>
        /// <returns></returns>
        protected List<ComUtil.DBMappingInfo> GetLockValMapsByGrpNo(short grpNo)
        {
            return getLockDataMapsByGrpNo(grpNo, LockType.Value);
        }

        /// <summary>
        /// 排他ロックキーマッピング情報の取得
        /// </summary>
        /// <param name="grpNo"></param>
        /// <returns></returns>
        protected List<ComUtil.DBMappingInfo> GetLockKeyMapsByGrpNo(short grpNo)
        {
            return getLockDataMapsByGrpNo(grpNo, LockType.Key);
        }

        /// <summary>
        /// 排他ロックデータマッピング情報の取得(テーブル指定)
        /// </summary>
        /// <param name="ctrlId">画面のコントロールID</param>
        /// <param name="type">キーor値</param>
        /// <param name="tableName">取得対象のテーブル名</param>
        /// <returns>排他ロックデータマッピング情報</returns>
        private List<ComUtil.DBMappingInfo> getLockDataMapsByTable(string ctrlId, LockType type, string tableName)
        {
            return getLockDataMaps(ctrlId, type).Where(x => x.TblName == tableName).ToList();
        }

        /// <summary>
        /// 排他ロック値マッピング情報の取得(テーブル指定)
        /// </summary>
        /// <param name="ctrlId">画面のコントロールID</param>
        /// <param name="tableName">取得対象のテーブル名</param>
        /// <returns>排他ロック値マッピング情報</returns>
        protected List<ComUtil.DBMappingInfo> GetLockValMapsByTable(string ctrlId, string tableName)
        {
            return getLockDataMapsByTable(ctrlId, LockType.Value, tableName);
        }

        /// <summary>
        /// 排他ロックキーマッピング情報の取得(テーブル指定)
        /// </summary>
        /// <param name="ctrlId">画面のコントロールID</param>
        /// <param name="tableName">取得対象のテーブル名</param>
        /// <returns>排他ロックキーマッピング情報</returns>
        protected List<ComUtil.DBMappingInfo> GetLockKeyMapsByTable(string ctrlId, string tableName)
        {
            return getLockDataMapsByTable(ctrlId, LockType.Key, tableName);
        }

        /// <summary>
        /// 排他ロック用JSON文字列の設定
        /// </summary>
        /// <param name="list">設定対象表示情報リスト</param>
        /// <param name="ctrlId">コントロールID</param>
        protected void SetExclusiveInfo(List<Dictionary<string, object>> list, string ctrlId)
        {
            ComOpt.SetLockDataString(list, this.mapInfoList, ctrlId);
        }

        /// <summary>
        /// 排他ロック用JSON文字列の設定
        /// </summary>
        /// <param name="list">設定対象表示情報リスト</param>
        /// <param name="ctrlId">コントロールID</param>
        protected void SetExclusiveInfo(List<ExpandoObject> list, string ctrlId)
        {
            ComOpt.SetLockDataString(list, this.mapInfoList, ctrlId);
        }

        ///// <summary>
        ///// 排他状態チェック
        ///// </summary>
        ///// <param name="dic">チェック対象データ</param>
        ///// <param name="lockValMaps">ロック値マッピング情報</param>
        ///// <param name="lockKeyMaps">ロックキーマッピング情報</param>
        ///// <returns>チェック結果(true:OK/false:NG)</returns>
        //protected bool CheckExclusiveStatus(Dictionary<string, object> dic, List<ComUtil.DBMappingInfo> lockValMaps, List<ComUtil.DBMappingInfo> lockKeyMaps)
        //{
        //    var tblNames = lockValMaps.Select(x => x.LockTblName).Distinct().ToList();
        //    var colInfoDic = GetColumnInfoDictionary(tblNames);

        //    if (!ComOpt.CheckLockData(this.db, dic, tblNames, lockValMaps, lockKeyMaps, colInfoDic))
        //    {
        //        // エラー終了
        //        this.Status = CommonProcReturn.ProcStatus.Error;
        //        // 「編集していたデータは他のユーザにより更新されました。もう一度編集する前にデータを再表示してください。」
        //        this.MsgId = GetResMessage("941290001");
        //        return false;
        //    }
        //    return true;
        //}
        /// <summary>
        /// 排他状態チェック
        /// </summary>
        /// <param name="dic">チェック対象データ</param>
        /// <param name="lockValMaps">ロック値マッピング情報</param>
        /// <param name="lockKeyMaps">ロックキーマッピング情報</param>
        /// <returns>チェック結果(true:OK/false:NG)</returns>
        protected bool CheckExclusiveStatus(Dictionary<string, object> dic, List<ComUtil.DBMappingInfo> lockValMaps, List<ComUtil.DBMappingInfo> lockKeyMaps)
        {
            var tblNames = lockValMaps.Select(x => x.LockTblName).Distinct().ToList();

            if (!ComOpt.CheckLockData(this.db, dic, tblNames, lockValMaps, lockKeyMaps))
            {
                setExclusiveError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 排他チェックエラー時のエラーメッセージ、ステータスの設定
        /// </summary>
        protected void setExclusiveError()
        {
            // エラー終了
            this.Status = CommonProcReturn.ProcStatus.Error;
            // 「編集していたデータは他のユーザにより更新されました。もう一度編集する前にデータを再表示してください。」
            this.MsgId = GetResMessage(CommonResources.ID.ID941290001);
        }

        /// <summary>
        /// 排他チェック、表示時と処理実行時の最大の更新日時を比較し、排他エラーかどうか判定する
        /// </summary>
        /// <param name="savedUpdateTime">表示時に取得した更新日時、画面から取得した値</param>
        /// <param name="getUpdateTime">処理実行時に取得した更新日時、DBから取得した値</param>
        /// <returns>エラーの場合False</returns>
        protected bool CheckExclusiveStatusByUpdateDatetime(DateTime? savedUpdateTime, DateTime? getUpdateTime)
        {
            bool result = compareDate();
            if (!result)
            {
                setExclusiveError();
            }
            return result;

            bool compareDate()
            {
                if (savedUpdateTime != null)
                {
                    // 排他チェック日時がある場合(詳細あり)
                    if (getUpdateTime != null)
                    {
                        // 排他チェック日時が有り、最新日時が有る場合、比較を行い、同じならチェックOK、異なればチェックNG
                        return savedUpdateTime == getUpdateTime;
                    }
                    else
                    {
                        // 排他チェック日時が有り、最新日時が無い場合、他の操作によって詳細が削除されたので、チェックNG
                        return false;
                    }
                }
                else
                {
                    // 排他チェック日時が無い場合(詳細なし)
                    if (getUpdateTime == null)
                    {
                        // 排他チェック日時が無く、最新日時が無い場合、チェックOK
                        return true;
                    }
                    else
                    {
                        // 排他チェック日時が無く、最新日時がある場合、他の操作によって詳細が追加されたので、チェックNG
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 排他チェック(単項目)
        /// </summary>
        /// <param name="ctrlId">一覧のコントロールID</param>
        /// <param name="target">対象のディクショナリ</param>
        /// <returns>エラーの場合False</returns>
        protected bool checkExclusiveSingle(string ctrlId, Dictionary<string, object> target = null)
        {
            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(ctrlId);
            var lockKeyMaps = GetLockKeyMaps(ctrlId);
            var targetDic = target != null ? target : ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
            if (!CheckExclusiveStatus(targetDic, lockValMaps, lockKeyMaps))
            {
                // 排他エラー
                return false;
            }
            return true;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <param name="ctrlId">一覧のコントロールID</param>
        /// <param name="targetList">排他チェックを行う一覧</param>
        /// <returns>エラーの場合False</returns>
        protected bool checkExclusiveList(string ctrlId, List<Dictionary<string, object>> targetList)
        {
            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(ctrlId);
            var lockKeyMaps = GetLockKeyMaps(ctrlId);

            // 排他チェック
            foreach (var targetDic in targetList)
            {
                if (!CheckExclusiveStatus(targetDic, lockValMaps, lockKeyMaps))
                {
                    // 排他エラー
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 排他チェック(単項目、チェック対象テーブル指定)
        /// </summary>
        /// <param name="ctrlId">一覧のコントロールID</param>
        /// <param name="targetList">排他チェックを行う一覧</param>
        /// <param name="target">対象のディクショナリ</param>
        /// <returns>エラーの場合False</returns>
        protected bool checkExclusiveSingleForTable(string ctrlId, List<string> tableList, Dictionary<string, object> target = null)
        {
            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(ctrlId);
            var lockKeyMaps = GetLockKeyMaps(ctrlId);
            var targetDic = target != null ? target : ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);

            List<ComUtil.DBMappingInfo> lockValMapsForTarget = new();
            List<ComUtil.DBMappingInfo> lockKeyMapsForTarget = new();

            // チェック対象のテーブル名で絞り込む
            if (tableList.Count > 0)
            {
                // 指定テーブル以外の排他チェックは除く(ロック値)
                foreach (ComUtil.DBMappingInfo mapInfo in lockValMaps)
                {
                    if (tableList.Contains(mapInfo.LockTblName))
                    {
                        lockValMapsForTarget.Add(mapInfo);
                    }
                }

                // 指定テーブル以外の排他チェックは除く(ロックキー)
                foreach (ComUtil.DBMappingInfo mapInfo in lockKeyMaps)
                {
                    if (tableList.Contains(mapInfo.LockTblName))
                    {
                        lockKeyMapsForTarget.Add(mapInfo);
                    }
                }
            }

            // 排他チェック
            if (!CheckExclusiveStatus(targetDic, lockValMapsForTarget, lockKeyMapsForTarget))
            {
                // 排他エラー
                return false;
            }

            return true;
        }

        /// <summary>
        /// 排他チェック(チェック対象テーブル指定)
        /// </summary>
        /// <param name="ctrlId">一覧のコントロールID</param>
        /// <param name="targetList">排他チェックを行う一覧</param>
        /// <returns>エラーの場合False</returns>
        protected bool checkExclusiveListForTable(string ctrlId, List<Dictionary<string, object>> targetList, List<string> tableList)
        {
            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(ctrlId);
            var lockKeyMaps = GetLockKeyMaps(ctrlId);

            List<ComUtil.DBMappingInfo> lockValMapsForTarget = new();
            List<ComUtil.DBMappingInfo> lockKeyMapsForTarget = new();

            // チェック対象のテーブル名で絞り込む
            if (tableList.Count > 0)
            {
                // 指定テーブル以外の排他チェックは除く(ロック値)
                foreach (ComUtil.DBMappingInfo mapInfo in lockValMaps)
                {
                    if (tableList.Contains(mapInfo.LockTblName))
                    {
                        lockValMapsForTarget.Add(mapInfo);
                    }
                }

                // 指定テーブル以外の排他チェックは除く(ロックキー)
                foreach (ComUtil.DBMappingInfo mapInfo in lockKeyMaps)
                {
                    if (tableList.Contains(mapInfo.LockTblName))
                    {
                        lockKeyMapsForTarget.Add(mapInfo);
                    }
                }
            }

            // 排他チェック
            foreach (var targetDic in targetList)
            {
                if (!CheckExclusiveStatus(targetDic, lockValMapsForTarget, lockKeyMapsForTarget))
                {
                    // 排他エラー
                    return false;
                }
            }

            return true;
        }
        #endregion

        /// <summary>
        /// dynamic型の検索結果をList型のマッピング情報へ変換
        /// </summary>
        /// <param name="list">対象データ</param>
        /// <param name="colNameList">出力対象カラム名リスト</param>
        /// <param name="needsHeader">ヘッダー出力の有無</param>
        /// <param name="needsSerialNo">通し番号の有無</param>
        /// <returns>マッピング情報リスト</returns>
        protected List<object[]> ConvertToReportMappingList(dynamic results, List<string[]> colNameList,
            bool needsHeader = false, bool needsSerialNo = false)
        {
            List<object[]> mappingList = new List<object[]>();
            int colCount = colNameList.Count;
            int startColNo = 0;
            if (needsSerialNo)
            {
                startColNo++;
            }

            if (needsHeader)
            {
                // ヘッダー出力有りの場合
                object[] values = new object[colCount];
                for (int i = 0; i < colCount; i++)
                {
                    var names = colNameList[i];
                    if (names.Length > 1)
                    {
                        // ヘッダー文字列が指定された場合
                        values[i] = names[1];
                    }
                    else
                    {
                        // ヘッダー文字列が未指定の場合はカラム名をセット
                        values[i] = names[0];
                    }
                }
                mappingList.Add(values);
            }

            // 出力対象カラム名に該当するデータをマッピング情報リストへ追加する
            int row = 0;
            foreach (var result in results)
            {
                IDictionary<string, object> dicResult = result as IDictionary<string, object>;
                object[] values = new object[colCount];
                if (needsSerialNo)
                {
                    values[0] = row + 1;
                }
                for (int i = startColNo; i < colCount; i++)
                {
                    var colName = colNameList[i][0];
                    var tmpVal = dicResult.FirstOrDefault(x => x.Key.ToUpper() == colName.ToUpper());
                    values[i] = tmpVal.Value;
                }
                mappingList.Add(values);
                row++;
            }
            return mappingList;
        }

        /// <summary>
        /// クラスの検索結果をList型のマッピング情報へ変換
        /// </summary>
        /// <param name="results">対象データ(クラスのリスト)</param>
        /// <param name="colNameList">出力対象カラム名リスト</param>
        /// <param name="needsHeader">ヘッダー出力の有無</param>
        /// <param name="needsSerialNo">通し番号の有無</param>
        /// <returns>マッピング情報リスト</returns>
        protected List<object[]> ConvertToReportMappingListByDataClass(dynamic results, List<string[]> colNameList,
            bool needsHeader = false, bool needsSerialNo = false)
        {
            // ↓ここから
            List<object[]> mappingList = new List<object[]>();
            int colCount = colNameList.Count;
            int startColNo = 0;
            if (needsSerialNo)
            {
                startColNo++;
            }

            if (needsHeader)
            {
                // ヘッダー出力有りの場合
                object[] values = new object[colCount];
                for (int i = 0; i < colCount; i++)
                {
                    var names = colNameList[i];
                    if (names.Length > 1)
                    {
                        // ヘッダー文字列が指定された場合
                        values[i] = names[1];
                    }
                    else
                    {
                        // ヘッダー文字列が未指定の場合はカラム名をセット
                        values[i] = names[0];
                    }
                }
                mappingList.Add(values);
            }
            // ↑ここまでオリジナルと一緒

            // 出力対象カラム名に該当するデータをマッピング情報リストへ追加する
            int row = 0;
            foreach (var result in results)
            {
                object[] values = new object[colCount];
                if (needsSerialNo)
                {
                    values[0] = row + 1;
                }
                for (int i = startColNo; i < colCount; i++)
                {
                    var colName = colNameList[i][0];
                    // resultがDictionaryでなくクラスなので、列名のメンバの値を取得
                    //values[i] = result.GetType().GetProperty(ComUtil.SnakeCaseToPascalCase(colName)).GetValue(result);
                    var prop = result.GetType().GetProperty(ComUtil.SnakeCaseToPascalCase(colName));
                    values[i] = prop != null ? prop.GetValue(result) : "";
                }
                mappingList.Add(values);
                row++;
            }
            return mappingList;
        }

        /// <summary>
        /// 実行結果の設定
        /// </summary>
        /// <param name="resultList">実行結果リスト</param>
        protected void SetJsonResult(List<Dictionary<string, object>> resultList)
        {
            //// 既に存在する場合
            //if (!string.IsNullOrEmpty(this.JsonResult))
            //{
            //    // JSON文字列をデシリアライズ
            //    var existingResult = DeserializeFromJson(this.JsonResult);

            //    resultList.AddRange(existingResult);
            //}

            //// 検索結果をJSON文字列へシリアライズ
            //this.JsonResult = SerializeToJson(resultList);

            // 既に存在する場合
            if (this.ResultList != null && this.ResultList.Count > 0)
            {
                resultList.AddRange(this.ResultList);
            }
            this.ResultList = resultList;
        }

        /// <summary>
        /// 実行結果の設定
        /// </summary>
        /// <param name="resultList">実行結果リスト</param>
        protected void SetJsonResult(List<ExpandoObject> resultList)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            foreach (var obj in resultList)
            {
                dicList.Add(new Dictionary<string, object>(obj as IDictionary<string, object>));
            }

            SetJsonResult(dicList);
        }

        /// <summary>
        /// 出力用マッピング情報リストの取得
        /// </summary>
        /// <param name="list">対象データ</param>
        /// <param name="colNameList">出力対象カラム名リスト</param>
        /// <param name="needsHeader">ヘッダー出力の有無</param>
        /// <param name="needsSerialNo">通し番号の有無</param>
        /// <returns>マッピング情報リスト</returns>
        protected List<object[]> GetReportMappingList(List<Dictionary<string, object>> list, List<string[]> colNameList,
            bool needsHeader = false, bool needsSerialNo = false)
        {
            List<object[]> mappingList = new List<object[]>();
            int colCount = colNameList.Count;
            int startColNo = 0;
            if (needsSerialNo)
            {
                colCount++;
                startColNo++;
            }

            if (needsHeader)
            {
                // ヘッダー出力有りの場合
                object[] values = new object[colCount];
                for (int i = startColNo; i < colCount; i++)
                {
                    var names = colNameList[i];
                    if (names.Length > 1)
                    {
                        // ヘッダー文字列が指定された場合
                        values[i] = names[1];
                    }
                    else
                    {
                        // ヘッダー文字列が未指定の場合はカラム名をセット
                        values[i] = names[0];
                    }
                }
            }

            // 出力対象カラム名に該当するデータをマッピング情報リストへ追加する
            for (int row = 0; row < list.Count; row++)
            {
                var target = list[row];
                object[] values = new object[colCount];
                if (needsSerialNo)
                {
                    values[0] = row + 1;
                }
                for (int i = startColNo; i < colCount; i++)
                {
                    var colName = colNameList[i][0];
                    if (target.ContainsKey(colName))
                    {
                        values[i] = target[colName];
                    }
                    else
                    {
                        values[i] = null;
                    }
                }
                mappingList.Add(values);
            }
            return mappingList;
        }

        /// <summary>
        /// 取込結果配列をDictionary型へ変換
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="results">取込結果配列</param>
        /// <returns></returns>
        protected List<Dictionary<string, object>> ConvertToUploadResultDictionary(string ctrlId, List<string[,]> results)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> dic;
            int rowNo = 1;

            // コントロールIDが指定されている場合は表示用データ
            bool isForDisplay = !string.IsNullOrEmpty(ctrlId);

            foreach (var array in results)
            {
                for (int row = 0; row < array.GetLength(0); row++)
                {
                    rowNo++;
                    dic = new Dictionary<string, object>();
                    if (isForDisplay)
                    {
                        // 表示用データの場合
                        dic.Add("CTRLID", ctrlId);
                        dic.Add("ROWNO", rowNo++);
                        dic.Add("ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.New);   // 新規登録
                    }
                    for (int col = 0; col < array.GetLength(1); col++)
                    {
                        dic.Add("VAL" + (col + 1).ToString(), array[row, col]);
                    }
                    list.Add(dic);
                }
            }

            if (isForDisplay)
            {
                // 表示用データの場合、先頭に総件数を挿入
                dic = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", 0 },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.New },
                    { "VAL1", list.Count.ToString() }
            };
                list.Insert(0, dic);
            }

            return list;
        }

        /// <summary>
        /// リソースメッセージ取得
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetResMessage(string key)
        {
            return ComUtil.GetPropertiesMessage(key, this.LanguageId, this.messageResources);
        }

        /// <summary>
        /// リソースメッセージ取得
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        protected string GetResMessage(string[] keys)
        {
            return ComUtil.GetPropertiesMessage(keys, this.LanguageId, this.messageResources);
        }

        /// <summary>
        /// アクセスログ出力
        /// </summary>
        /// <param name="msg"></param>
        protected void writeAccessLog(CommonProcParamIn param)
        {
            if (!this.CtrlId.Equals("CheckAuthority"))
            {
                // 初期権限チェックの場合はログ出力を行わない
                logger.ActionLog(param.FactoryId, param.UserId, param.ConductId, param.CtrlId);
            }
        }

        /// <summary>
        /// エラーログ出力
        /// </summary>
        /// <param name="msg">エラーメッセージ</param>
        /// <param name="ex">例外オブジェクト(省略可)</param>
        protected void writeErrorLog(string msg, Exception ex = null)
        {
            logger.ErrorLog(this.FactoryId, this.UserId, msg, ex);
        }

        /// <summary>
        /// DB接続エラーログ出力
        /// </summary>
        /// <param name="msg">エラーメッセージ</param>
        /// <param name="ex">例外オブジェクト(省略可)</param>
        protected void writeDBConnectionErrorLog()
        {
            // 「DB接続に失敗しました。」
            logger.ErrorLog(this.FactoryId, this.UserId, ComUtil.GetPropertiesMessage(CommonResources.ID.ID941190004, this.LanguageId));
        }

        /// <summary>
        /// デバッグログ出力
        /// </summary>
        /// <param name="msg"></param>
        protected void writeDebugLog(string msg)
        {
            dynamic callerInfo = getCallerInfo(2);

            // [クラス名][メソッド名][メッセージ]
            logger.DebugLog(this.FactoryId, this.UserId, string.Format("[{0}.{1}] {2}", callerInfo.ClassName, callerInfo.MethodName, msg));
        }

        /// <summary>
        /// 呼び元のクラス名とメソッド名を取得する
        /// </summary>
        /// <param name="skipFrames">遡るフレーム数(0は自分自身、1は直接呼び出したメソッド)</param>
        /// <returns>呼び元の情報(メソッド名：MethodName、クラス名：ClassName)</returns>
        protected dynamic getCallerInfo(int skipFrames)
        {
            dynamic info = new ExpandoObject();

            // StackFrameクラスをインスタンス化する
            StackFrame sf = new StackFrame(skipFrames);
            // 呼び出し元のメソッド名を取得する
            info.MethodName = sf.GetMethod().Name;
            // 呼び出し元のクラス名を取得する
            info.ClassName = sf.GetMethod().ReflectedType.Name;

            return info;
        }

        /// <summary>
        /// エラー設定
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="ctrlid"></param>
        /// <param name="rowNo"></param>
        /// <param name="namesPair"></param>
        /// <param name="targetCd"></param>
        /// <param name="error"></param>
        protected void addError(ref Dictionary<string, object> dic, Dictionary<string, object> namesPair, string targetCd, string error)
        {
            // エラー内容を設定
            foreach (var item in namesPair)
            {
                if (item.Value.ToString().ToUpper().Equals(targetCd.ToUpper()))
                {
                    if (!dic.ContainsKey(item.Key))
                    {
                        dic.Add(item.Key, error);
                    }
                    else
                    {
                        dic[item.Key] = dic[item.Key] + "\n" + error;
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// エラー設定
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="ctrlid"></param>
        /// <param name="rowNo"></param>
        /// <param name="namesPair"></param>
        /// <param name="targetCd"></param>
        /// <param name="error"></param>
        /// <remarks>※FromTo日付の場合、こちらを使用</remarks>
        protected void addError(ref Dictionary<string, object> dic, Dictionary<string, object> namesPair, string targetCd, Dictionary<string, object> error)
        {
            // エラー内容を設定
            foreach (var item in namesPair)
            {
                if (item.Value.ToString().ToUpper().Equals(targetCd.ToUpper()))
                {
                    if (!dic.ContainsKey(item.Key))
                    {
                        dic.Add(item.Key, error);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// エラー設定
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="flg">0:From、1:To</param>
        /// <param name="error">エラー文字列</param>
        protected void addErrorForFromToDate(ref Dictionary<string, object> dic, int flg, string error)
        {
            // FromTo設定
            String key = flg < 1 ? "From" : "To";

            // 未設定の場合
            if (!dic.ContainsKey(key))
            {
                dic.Add(key, error);
            }
            else
            {
                dic[key] = dic[key].ToString() + "\n" + error;
            }
        }

        /// <summary>
        /// ページ情報取得
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="pageInfoList"></param>
        /// <returns></returns>
        protected PageInfo GetPageInfo(string ctrlId, List<PageInfo> pageInfoList)
        {
            if (pageInfoList == null || pageInfoList.Count == 0) { return new PageInfo(); }

            // コントロールID未指定時は先頭を返す
            if (string.IsNullOrEmpty(ctrlId)) { return pageInfoList[0]; }

            // ページング情報を取得
            foreach (var pageInfo in pageInfoList)
            {
                if (pageInfo.CtrlId.Equals(ctrlId))
                {
                    return pageInfo;
                }
            }
            // 一致するものがない場合はデフォルトを返す
            return new PageInfo();
        }

        /// <summary>
        /// DBマッピング情報リストの取得
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <returns></returns>
        protected List<ComUtil.DBMappingInfo> GetDBMappingList(string ctrlId = "")
        {
            if (this.mapInfoList == null || this.mapInfoList.Count == 0 || string.IsNullOrEmpty(ctrlId))
            {
                // マッピング情報リストが空、またはコントロールID未指定の場合、マッピング情報を取得
                this.mapInfoList = this.db.GetListByOutsideSql<ComUtil.DBMappingInfo>(
                SqlName.GetMappingInfoList, SqlName.SubDir, new { PgmId = this.PgmId, LanguageId = this.LanguageId });
            }
            if (this.mapInfoList == null || this.mapInfoList.Count == 0 || string.IsNullOrEmpty(ctrlId))
            { return null; }

            return this.mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();
        }

        /// <summary>
        /// 指定機能IDのマッピング情報をDBより取得し、現在管理しているマッピング情報に追加する処理
        /// </summary>
        /// <param name="pgmId">機能ID</param>
        /// <remarks>共通系画面では自身の画面の定義しかマッピング情報を持っていないので、これで追加する</remarks>
        protected void AddMappingListOtherPgmId(string pgmId)
        {
            // DBより取得
            var addMapInfoList = this.db.GetListByOutsideSql<ComUtil.DBMappingInfo>(SqlName.GetMappingInfoList, SqlName.SubDir, new { PgmId = pgmId, LanguageId = this.LanguageId });
            // 現在のマッピング情報の有無に応じて切り替え
            if (this.mapInfoList == null || this.mapInfoList.Count == 0)
            {
                // 無い場合は置き換え
                this.mapInfoList = addMapInfoList;
            }
            else
            {
                // ある場合は追加
                this.mapInfoList = this.mapInfoList.Concat(addMapInfoList).ToList();
            }
        }

        /// <summary>
        /// メッセージリソースの取得
        /// </summary>
        /// <param name="languageId">対象の言語の言語ID、省略した場合は全言語を取得</param>
        /// <returns>指定した言語のメッセージリソース</returns>
        private ComUtil.MessageResources getMessageResources(string languageId = "", List<int> factoryIdList = null)
        {
            if (this.messageResources == null)
            {
                // データベースより対象の言語の翻訳マスタのレコードを取得
                this.messageResources = ComUtil.GetMessageResourceFromDb(this.db, languageId, factoryIdList: factoryIdList);
            }

            // 戻り値
            if (string.IsNullOrEmpty(languageId))
            {
                //言語未指定の場合は全言語
                return this.messageResources;
            }
            //言語指定の場合は言語で絞り込み
            return this.messageResources.GetLanguageResources(languageId);
        }

        /// <summary>
        /// 検索結果ヘッダ情報を取得する
        /// </summary>
        /// <param name="resultList">条件格納リスト</param>
        /// <param name="targetCtrlId">コンロトールID ※未指定の場合、すべてが対象</param>
        /// <remarks>検索結果一覧に対応</remarks>
        protected void ResultDataHeaderInfo(ref List<Dictionary<string, object>> resultList, List<Dictionary<string, object>> targetDictionary = null, string targetCtrlId = null)
        {
            // コントロールID
            var ctrlId = "";
            // ヘッダ情報を設定する
            if (targetDictionary == null) { targetDictionary = this.resultInfoDictionary; }
            foreach (var dicResult in targetDictionary)
            {
                if (dicResult.ContainsKey("CTRLID") && dicResult["CTRLID"].ToString() != null)
                {
                    var workCtrlId = dicResult["CTRLID"].ToString();
                    if (!string.IsNullOrEmpty(targetCtrlId) && !targetCtrlId.Equals(workCtrlId)) { continue; }
                    if (string.IsNullOrEmpty(ctrlId) || !ctrlId.Equals(workCtrlId))
                    {
                        var dicList = this.resultInfoDictionary.Where(x =>
                            workCtrlId.Equals(x["CTRLID"].ToString()));

                        var headInfo = new Dictionary<string, object>
                        {
                            { "CTRLID", workCtrlId },
                            { "ROWNO", 0 },
                            { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit },
                            { "PAGENO", this.getPageNo(workCtrlId) },
                            { "VAL1", dicList.Count() }
                        };
                        resultList.Add(headInfo);

                        ctrlId = workCtrlId;
                    }
                }
                resultList.Add(dicResult);
            }
        }

        /// <summary>
        /// グローバルリストから対象値を取得
        /// </summary>
        /// <param name="targetKey">対象キー</param>
        /// <param name="delFlg">true:グローバルリストから削除、false:削除しない</param>
        /// <returns>対象値</returns>
        public object GetGlobalData(string targetKey, bool delFlg = false)
        {
            object value = null;
            if (this.IndividualDictionary != null && this.IndividualDictionary.Count > 0)
            {
                if (!this.IndividualDictionary.ContainsKey(targetKey))
                {
                    return null;
                }

                value = this.IndividualDictionary[targetKey];
                if (delFlg)
                {
                    this.IndividualDictionary.Remove(targetKey);
                }
            }
            return value;
        }

        /// <summary>
        /// グローバルリストに対象値を設定
        /// </summary>
        /// <param name="targetKey">対象キー</param>
        /// <param name="targetValue">設定値</param>
        protected void SetGlobalData(string targetKey, object targetVal)
        {
            // 含まれている場合、いったんクリアする
            if (this.IndividualDictionary.ContainsKey(targetKey))
            {
                this.IndividualDictionary.Remove(targetKey);
            }
            this.IndividualDictionary.Add(targetKey, targetVal);
            return;
        }

        /// <summary>
        /// ディクショナリーから値を取得
        /// </summary>
        /// <param name="condition">ディクショナリー</param>
        /// <param name="key">対象キー</param>
        /// <returns>結果値</returns>
        /// <remarks>要素がない、もしくはnullの場合は空文字を返す</remarks>
        protected string getDictionaryValue(Dictionary<string, object> condition, string key)
        {
            if (!condition.ContainsKey(key))
            {
                return "";
            }
            if (condition[key] == null)
            {
                return "";
            }
            return condition[key].ToString();
        }

        /// <summary>
        /// 入力変更チェック
        /// </summary>
        /// <param name="target1">比較対象１</param>
        /// <param name="target2">比較対象２</param>
        /// <returns>true:変更あり、false:変更なし</returns>
        protected bool inputChangeCheck(Dictionary<string, object> target1, Dictionary<string, object> target2)
        {
            // 画面表示値のみ抽出
            var workDic1 = target1.Where(x => x.Key.IndexOf("VAL") >= 0).ToArray();
            var workDic2 = target2.Where(x => x.Key.IndexOf("VAL") >= 0).ToArray();

            // 件数が異なる場合は、変更あり
            if (workDic1.Count() != workDic2.Count()) { return true; }
            for (int i = 0; i < workDic1.Count(); i++)
            {
                // 値を取得
                var key = workDic1[i].Key;
                var val = workDic1[i].Value;

                for (int j = 0; j < workDic2.Count(); j++)
                {
                    if (!workDic2[j].Key.Equals(key)) { continue; } // キー要素が同一の場合、比較を行う
                    if (!workDic2[j].Value.Equals(val)) { return true; } // 値が異なる場合、変更あり
                    else { break; } // 値が同じ場合、次の比較へ
                }
            }
            return false;
        }

        /// <summary>
        /// エラー情報管理クラス
        /// </summary>
        protected class ErrorInfo
        {
            /// <summary>
            /// Gets resultInfoDictionaryにセットする、単一の画面項目に対するエラー情報
            /// </summary>
            /// <value>
            /// ResultInfoDictionaryにセットする、単一の画面項目に対するエラー情報
            /// </value>
            public Dictionary<string, object> Result { get; }

            /// <summary>
            /// Gets or Sets 項目でなく、画面に表示するメッセージ
            /// </summary>
            /// <value>
            /// 項目でなく、画面に表示するメッセージ
            /// </value>
            public string FormMessage { get; set; }

            /// <summary>
            /// コンストラクタ(対象の画面項目)
            /// </summary>
            /// <param name="resultInfoDictionary">対象の画面項目</param>
            public ErrorInfo(Dictionary<string, object> resultInfoDictionary)
            {
                // 情報をコピー
                this.Result = new Dictionary<string, object>(resultInfoDictionary);
                // エラー情報を設定
                this.Result["DATATYPE"] = TMPTBL_CONSTANTS.DATATYPE.ErrorDetail;

                // VALnはエラー情報を設定するので不要、削除する
                List<string> keyList = this.Result.Keys.Where(x => x.StartsWith("VAL")).ToList();
                keyList.ForEach(x => this.Result.Remove(x));
            }

            /// <summary>
            /// エラー情報セット
            /// </summary>
            /// <param name="errorMessage">エラーメッセージ</param>
            /// <param name="valId">エラーメッセージをセットする項目ID、resultInfoDictionaryのVALn</param>
            public void setError(string errorMessage, string valId)
            {
                if (!this.Result.ContainsKey(valId))
                {
                    // VALnが存在しない場合、新規追加
                    this.Result.Add(valId, errorMessage);
                    return;
                }
                // 存在する場合、既存のメッセージに追加
                this.Result[valId] += "\n" + errorMessage;
            }

            /// <summary>
            /// エラー情報セット
            /// </summary>
            /// <param name="errorMessage">エラーメッセージ</param>
            /// <param name="valId">エラーメッセージをセットする項目ID、resultInfoDictionaryのVALn</param>
            /// <param name="flg">0:From、1:To</param>
            public void setErrorByFromTo(string errorMessage, string valId, int flg)
            {
                // FromTo設定
                String key = flg < 1 ? "From" : "To";
                var dicInfo = new Dictionary<string, object>();

                // 未設定の場合
                if (!this.Result.ContainsKey(valId))
                {
                    dicInfo.Add(key, errorMessage);
                    this.Result.Add(valId, dicInfo);
                }
                else
                {
                    dicInfo = (Dictionary<string, object>)this.Result[valId];
                    if (!dicInfo.ContainsKey(key))
                    {
                        dicInfo.Add(key, errorMessage);
                    }
                    else
                    {
                        dicInfo[key] = dicInfo[key].ToString() + "\n" + errorMessage;
                    }
                    this.Result[valId] = dicInfo;
                }
            }

            /// <summary>
            /// フォーム用エラーメッセージセット
            /// </summary>
            /// <param name="errorMessage">エラーメッセージ</param>
            public void setFormError(string errorMessage)
            {
                if (CommonUtil.IsNullOrEmpty(this.FormMessage))
                {
                    // 新規の場合
                    this.FormMessage = errorMessage;
                    return;
                }
                // TODO 複数行のエラーメッセージ出力は可能？
                // 既にメッセージのある場合、改行して追加
                this.FormMessage += "\n" + errorMessage;
            }
        }

        /// <summary>
        /// プッシュ通知設定
        /// </summary>
        /// <param name="pushInfo">プッシュ通知の情報</param>
        public void SetPushTarget(PushInfo pushInfo)
        {
            // JSON文字列へシリアライズ
            this.JsonPushTarget = JsonSerializer.Serialize(pushInfo.value);
        }

        /// <summary>
        /// プッシュ通知情報
        /// </summary>
        public class PushInfo
        {
            /// <summary>プッシュ通知に設定する値　ユーザIDをキーに、件数を値に持つ辞書</summary>
            public Dictionary<string, int> value { get; set; }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PushInfo()
            {
                value = new Dictionary<string, int>();
            }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="userIdList">通知対象ユーザIDのリスト</param>
            public PushInfo(List<string> userIdList)
            {
                // 件数は仕様検討中のため、1固定
                value = new Dictionary<string, int>();
                userIdList.ForEach(x => value.Add(x, 1));
            }
        }

        /// <summary>
        /// 画面の明細行の更新フラグが編集中かを調べる
        /// </summary>
        /// <param name="row">画面の明細の1行</param>
        /// <returns>更新フラグが編集中の場合True</returns>
        protected bool isUpdatedTableRow(Dictionary<string, object> row)
        {
            return getDictionaryValue(row, "UPDTAG").Equals(UpdTag.Input);
        }

        #region ボタン表示活性制御関連
        /// <summary>
        /// 呼び出されたコントロールIDが実行可能かチェックを行う
        /// </summary>
        /// <returns>true:実行可能、false:実行不能</returns>
        protected bool ExecBtnActionCheck()
        {
            // 該当のコントロールがボタンかどうか判定を行う
            int result = db.GetCountByOutsideSql(SqlName.CheckBtnCtrlId, SqlName.SubDir,
                new { Pgmid = this.ConductId, FormNo = this.FormNo, BtnCtrlid = this.CtrlId, FactoryIdList = this.BelongingInfo.BelongingFactoryIdList });
            if (result < 1) { return true; } // ボタンコントロールIDではない場合、実行チェックを行わない

            // ボタン権限情報を取得
            var dispDiv = this.buttonInfoDictionary.Where(x => (
                x.ContainsKey("CTRLID") && x["CTRLID"].ToString() == this.CtrlId &&
                x.ContainsKey("FORMNO") && x["FORMNO"].ToString() == this.FormNo.ToString()))
                .Select(x => x["DISPKBN"]).FirstOrDefault();

            if (dispDiv == null || Convert.ToInt32(dispDiv) != USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active)
            {
                // 権限情報がない、または表示区分が活性でない場合は実行不可
                return false;
            }
            return true;
        }

        /// <summary>
        /// ボタン表示切替処理
        /// </summary>
        /// <param name="btnCtrlid">ボタンコントロールID</param>
        /// <param name="dispKbn">Hide:非表示、Active:表示/活性、Disabled:非活性 ※定数"USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF"を用いること。</param>
        /// <param name="formNo">画面NO</param>
        /// <returns>true:正常、false:異常</returns>
        protected bool BtnDispSwitching(string btnCtrlid, short dispKbn, short formNo = -1)
        {
            // 該当のコントロールIDが存在するかチェック
            foreach (var btnInfo in buttonInfoDictionary)
            {
                if (btnInfo.ContainsKey("CTRLID"))
                {
                    // 画面NOが指定されている場合、その画面NOのみ対象とする
                    if (formNo != -1 && !btnInfo["FORMNO"].ToString().Equals(formNo.ToString())) { continue; }
                    // 大文字同士で比較
                    if (!btnInfo["CTRLID"].ToString().ToUpper().Equals(btnCtrlid.ToUpper())) { continue; } // ボタンコントロールIDが異なる場合、スキップ
                    // 該当のコントロールIDが存在し、表示区分を書き換えたら処理を戻す
                    if (btnInfo.ContainsKey("DISPKBN")) { btnInfo["DISPKBN"] = dispKbn; return true; }
                }
            }

            // 該当のコントロールIDが存在しないとみなし、処理を戻す
            return false;
        }

        /// <summary>
        /// 指定したボタンを活性にする
        /// </summary>
        /// <param name="btnCtrlid">ボタンコントロールID</param>
        /// <param name="formNo">画面NO</param>
        /// <returns>true:正常、false:異常</returns>
        protected bool BtnActive(string btnCtrlId, short formNo = -1)
        {
            return BtnDispSwitching(btnCtrlId, USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active, formNo);
        }
        /// <summary>
        /// 指定したボタンを非表示にする
        /// </summary>
        /// <param name="btnCtrlid">ボタンコントロールID</param>
        /// <param name="formNo">画面NO</param>
        /// <returns>true:正常、false:異常</returns>
        protected bool BtnHide(string btnCtrlId, short formNo = -1)
        {
            return BtnDispSwitching(btnCtrlId, USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Hide, formNo);
        }
        /// <summary>
        /// 指定したボタンを非活性にする
        /// </summary>
        /// <param name="btnCtrlid">ボタンコントロールID</param>
        /// <param name="formNo">画面NO</param>
        /// <returns>true:正常、false:異常</returns>
        protected bool BtnDisabled(string btnCtrlId, short formNo = -1)
        {
            return BtnDispSwitching(btnCtrlId, USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Disabled, formNo);
        }

        /// <summary>
        /// 処理中のボタンを非活性にする
        /// </summary>
        protected void ActiveBtnDisabled()
        {
            this.BtnDisabled(this.CtrlId);
        }

        /// <summary>
        /// すべてボタンを活性にする ※非表示ボタン、権限管理しないボタンを除く
        /// </summary>
        /// <param name="formNo">画面NO</param>
        protected void AllBtnActive(short formNo = -1)
        {
            foreach (var btnInfo in this.buttonInfoDictionary)
            {
                if (btnInfo.ContainsKey("CTRLID") && btnInfo.ContainsKey("DISPKBN"))
                {
                    // 画面NOが指定されている場合、その画面NOのみ対象とする
                    if (formNo != -1 && !btnInfo["FORMNO"].ToString().Equals(formNo.ToString())) { continue; }
                    // ボタンが既に非表示の場合、処理をスキップする
                    if (btnInfo["DISPKBN"].ToString().Equals(USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Hide.ToString())) { continue; }
                    // ボタン権限管理区分が"0:権限管理しない"の場合、処理をスキップする
                    if (btnInfo["AUTHFLG"].ToString().Equals(LISTITEM_DEFINE_CONSTANTS.BTN_AUTHCONTROLKBN.Free.ToString())) { continue; }
                    // ボタンコントロールIDが存在する場合、非活性にする
                    btnInfo["DISPKBN"] = USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active;
                }
            }
        }

        /// <summary>
        /// すべてボタンを非活性にする ※非表示ボタン、権限管理しないボタンを除く
        /// </summary>
        /// <param name="formNo">画面NO</param>
        protected void AllBtnDisabled(short formNo = -1)
        {
            foreach (var btnInfo in this.buttonInfoDictionary)
            {
                if (btnInfo.ContainsKey("CTRLID") && btnInfo.ContainsKey("DISPKBN"))
                {
                    // 画面NOが指定されている場合、その画面NOのみ対象とする
                    if (formNo != -1 && !btnInfo["FORMNO"].ToString().Equals(formNo.ToString())) { continue; }
                    // ボタンが既に非表示の場合、処理をスキップする
                    if (btnInfo["DISPKBN"].ToString().Equals(USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Hide.ToString())) { continue; }
                    // ボタン権限管理区分が"0:権限管理しない"の場合、処理をスキップする
                    if (btnInfo["AUTHFLG"].ToString().Equals(LISTITEM_DEFINE_CONSTANTS.BTN_AUTHCONTROLKBN.Free.ToString())) { continue; }
                    // ボタンコントロールIDが存在する場合、非活性にする
                    btnInfo["DISPKBN"] = USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Disabled;
                }
            }
        }

        /// <summary>
        /// ボタンの表示状態を取得する処理
        /// </summary>
        /// <param name="btnCtrlId">状態を取得するボタンのコントロールID、非表示で定義されていないボタン</param>
        /// <returns>ボタンの表示状態　USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF_ENUM</returns>
        protected USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF_ENUM GetBtnDispKbn(string btnCtrlId)
        {
            // 表示区分の値
            short dispKbn = -1;
            // ボタン情報で繰り返して、表示区分の値を取得する
            foreach (var btnInfo in this.buttonInfoDictionary)
            {
                // CTRLIDの要素を持つ者のみ
                if (!btnInfo.ContainsKey("CTRLID")) { continue; }
                // コントロールIDを取得
                string targetBtnCtrlId = btnInfo["CTRLID"].ToString();
                if (btnCtrlId.ToUpper().Equals(targetBtnCtrlId.ToUpper()))
                {
                    // コントロールIDが一致する場合、表示区分を取得
                    if (!btnInfo.ContainsKey("DISPKBN"))
                    {
                        // 取得できない場合、None
                        return USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF_ENUM.None;
                    }
                    // 表示区分の値を取得して、数値に変換
                    bool result = short.TryParse(btnInfo["DISPKBN"].ToString(), out dispKbn);
                    if (!result)
                    {
                        // 変換できない場合、None
                        return USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF_ENUM.None;
                    }
                    break;
                }
            }
            // 表示区分の値を取得できた場合、数値の値を列挙型に変換する
            switch (dispKbn)
            {
                case USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active:
                    return USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF_ENUM.Active;
                case USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Disabled:
                    return USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF_ENUM.Disabled;
                case USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Hide:
                    return USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF_ENUM.Hide;
                default:
                    return USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF_ENUM.None;
            }
        }

        #endregion

        /// <summary>
        /// ワークフローの処理対象となるコントロールIDを取得する処理
        /// </summary>
        /// <returns>処理対象のコントロールID(APConstants.WORKFLOW.ButtonCtrlId)</returns>
        protected string getWorkflowCtrlId()
        {
            return this.GetGlobalData("AWFCommon_CtrlId", true).ToString();
        }

        /// <summary>
        /// 検索後、結果が無い場合のメッセージを表示する処理
        /// </summary>
        /// <returns>この処理を終了した後、呼出元の処理を-1で終了する場合、True</returns>
        private bool dispMessageAfterSearch()
        {
            // 検索結果データが存在しない場合、エラーを表示 ※確認メッセージの場合、処理を行わない
            //if (!this.MsgId.StartsWith(GetResMessage("941190001")) && (this.ResultList == null || this.ResultList.Count() == 0))
            // 条件が複雑なので分解
            bool isNotConfirmMsg = !this.MsgId.StartsWith(GetResMessage("941190001")); // 確認メッセージの表示を行っていない
            bool isNullResultList = this.ResultList == null || this.ResultList.Count() == 0; // 検索結果が存在しない
            bool isResult0 = this.ResultList.Count() == 1 && this.ResultList[0].ContainsKey("VAL1") && this.ResultList[0]["VAL1"].ToString() == "0"; // 検索結果の総件数が0件である
            /*if (!this.MsgId.StartsWith(GetResMessage("941190001")) && (this.ResultList == null ||
                this.ResultList.Count() == 1 && this.ResultList[0]["VAL1"].ToString() == "0"))*/
            if (isNotConfirmMsg && (isNullResultList || isResult0))
            {
                // 警告メッセージで終了
                this.Status = CommonProcReturn.ProcStatus.Warning;
                if (string.IsNullOrEmpty(this.MsgId) || !string.IsNullOrEmpty(this.MsgId) && this.MsgId.Equals(GetResMessage(new string[] { "941060002", "911090001" })))
                {
                    // 「該当データがありません。」
                    this.MsgId = GetResMessage("941060001");
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// コントロールID比較用クラス
        /// </summary>
        protected class CompareCtrlIdClass
        {
            /// <summary>
            /// 比較用　大文字のコントロールID
            /// </summary>
            protected string UpperCtrlId;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="ctrlId">比較の対象となるコントロールID this.ctrlId</param>
            public CompareCtrlIdClass(string ctrlId)
            {
                // 大文字に変換
                this.UpperCtrlId = ctrlId.ToUpper();
            }

            /// <summary>
            /// 引数のIDとコントロールIDを大文字同士で比較し、前方一致ならTrueを返す
            /// </summary>
            /// <param name="target">比較するID</param>
            /// <returns>画面のコントロールIDと前方一致の場合はTrue</returns>
            public bool IsStartId(string target)
            {
                // 大文字に変換
                string UpperTargetId = target.ToUpper();
                // 前方一致ならTrue
                return UpperCtrlId.StartsWith(UpperTargetId);
            }

            /// <summary>
            /// ID比較：戻る
            /// </summary>
            /// <returns>BackまたはCancelから始まるならTrue</returns>
            public bool IsBack()
            {
                return IsStartId("Back") || IsStartId("Cancel");
            }
            /// <summary>
            /// ID比較：初期表示
            /// </summary>
            /// <returns>INITから始まるならTrue</returns>
            public bool IsInit()
            {
                return IsStartId("Init");
            }
            /// <summary>
            /// ID比較：登録
            /// </summary>
            /// <returns>Registから始まるならTrue</returns>
            public bool IsRegist()
            {
                return IsStartId("Regist");
            }
            /// <summary>
            /// ID比較：削除
            /// </summary>
            /// <returns>Deleteから始まるならTrue</returns>
            public bool IsDelete()
            {
                return IsStartId("Delete");
            }
            /// <summary>
            /// ID比較：新規
            /// </summary>
            /// <returns>Newから始まるならTrue</returns>
            public bool IsNew()
            {
                return IsStartId("New");
            }
            /// <summary>
            /// ID比較：複写
            /// </summary>
            /// <returns>Copyから始まるならTrue</returns>
            public bool IsCopy()
            {
                return IsStartId("Copy");
            }
            /// <summary>
            /// ID比較：修正
            /// </summary>
            /// <returns>Updateから始まるならTrue</returns>
            public bool IsUpdate()
            {
                return IsStartId("Update");
            }
            /// <summary>
            /// ID比較：一覧行削除(共通・Ajax)
            /// </summary>
            /// <returns>ComDeleteRowから始まる場合True</returns>
            public bool IsComDeleteRow()
            {
                return IsStartId("ComDeleteRow");
            }
            /// <summary>
            /// ID比較：アップロード
            /// </summary>
            /// <returns>ComDeleteRowから始まる場合True</returns>
            public bool IsUpload()
            {
                return IsStartId("Upload");
            }
            /// <summary>
            /// ID比較：帳票出力
            /// </summary>
            /// <returns>Reportから始まる場合True</returns>
            public bool IsReport()
            {
                return IsStartId("Report");
            }
            /// <summary>
            /// ID比較：ダウンロード
            /// </summary>
            /// <returns>Downloadから始まる場合True</returns>
            public bool IsDownload()
            {
                return IsStartId("Download");
            }
        }
        #endregion

        #region 検索条件取得関連
        /// <summary>
        /// 検索条件(場所階層＋職種機種＋詳細検索条件)のWHERE句とパラメータの取得
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="tableNameList">テーブル名リスト</param>
        /// <param name="sql">WHERE句文字列</param>
        /// <param name="param">WHERE句パラメータ</param>
        /// <returns>WHERE句生成結果</returns>
        protected bool GetWhereClauseAndParam(PageInfo pageInfo, List<string> tableNameList, out string sql, out dynamic param)
        {
            sql = string.Empty;
            param = new ExpandoObject();
            if (tableNameList == null || tableNameList.Count == 0) { return true; }

            // 場所階層構成IDリストを取得
            var sbSql = new StringBuilder();
            List<int> locationIdList = new List<int>();
            List<int> jobIdList = new List<int>();
            string keyName = STRUCTURE_CONSTANTS.CONDITION_KEY.Location;
            var dic = this.searchConditionDictionary.Where(x => x.ContainsKey(keyName)).FirstOrDefault();
            if (dic != null && dic.ContainsKey(keyName))
            {
                // 選択された構成IDリストから配下の構成IDをすべて取得
                locationIdList = dic[keyName] as List<int>;
                if (locationIdList != null && locationIdList.Count > 0)
                {
                    locationIdList = GetLowerStructureIdList(locationIdList);

                    // WHERE句を生成
                    sbSql.AppendLine("where");
                    for (int i = 0; i < tableNameList.Count; i++)
                    {
                        if (i > 0)
                        {
                            sbSql.AppendLine("and");
                        }
                        sbSql.Append(tableNameList[i]).AppendLine(".location_structure_id in @LocationIdList");
                    }
                    param.LocationIdList = locationIdList;
                }
            }

            // 職種機種構成IDリストを取得
            keyName = STRUCTURE_CONSTANTS.CONDITION_KEY.Job;
            dic = this.searchConditionDictionary.Where(x => x.ContainsKey(keyName)).FirstOrDefault();
            if (dic != null && dic.ContainsKey(keyName))
            {
                // 選択された構成IDリストから配下の構成IDをすべて取得
                jobIdList = dic[keyName] as List<int>;
                if (jobIdList != null && jobIdList.Count > 0)
                {
                    jobIdList = GetLowerStructureIdList(jobIdList);
                    if (sbSql.Length > 0)
                    {
                        sbSql.AppendLine("and");
                    }
                    else
                    {
                        sbSql.AppendLine("where");
                    }
                    for (int i = 0; i < tableNameList.Count; i++)
                    {
                        if (i > 0)
                        {
                            sbSql.AppendLine("and");
                        }
                        sbSql.Append(tableNameList[i]).AppendLine(".job_structure_id in @JobIdList");
                    }
                    param.JobIdList = jobIdList;
                }
            }

            // 詳細検索条件辞書を取得
            var list = this.searchConditionDictionary.Where(x => x.ContainsKey("IsDetailCondition") && x.ContainsKey("CTRLID")).ToList();
            if (list != null && list.Count > 0)
            {
                dic = list.Where(x => x["IsDetailCondition"].Equals(true) && x["CTRLID"].Equals(pageInfo.CtrlId)).FirstOrDefault();
            }

            sql = sbSql.ToString();

            return true;
        }

        /// <summary>
        /// 検索条件(場所階層＋職種機種＋詳細検索条件)のWHERE句とパラメータの取得
        /// </summary>
        /// <param name="pageInfo">ページ情報</param>
        /// <param name="selectSql">SELECT句文字列</param>
        /// <param name="sql">WHERE句文字列</param>
        /// <param name="param">WHERE句パラメータ</param>
        /// <param name="isDetailConditionApplied">詳細検索条件適用フラグ</param>
        /// <param name="unUseLocation">予備品用の場所階層を使用する場合はTrue(デフォルトFalse)</param>
        /// <returns>WHERE句生成結果</returns>
        protected bool GetWhereClauseAndParam2(PageInfo pageInfo, string selectSql, out string sql, out dynamic param, out bool isDetailConditionApplied, bool unUseLocation = false)
        {
            sql = string.Empty;
            param = new ExpandoObject();
            var dicResult = param as IDictionary<string, object>;
            isDetailConditionApplied = false;

            // 場所階層構成IDリストを取得
            var sbSql = new StringBuilder();
            var existsWhere = false;
            if (selectSql.Contains(CommonColumnName.LocationId) && !unUseLocation)
            {
                bool useBelongingInfo = false;
                List<int> locationIdList = getIdListBySearchCond(STRUCTURE_CONSTANTS.CONDITION_KEY.Location);
                if ((locationIdList == null || locationIdList.Count == 0) &&
                    (this.BelongingInfo.LocationInfoList != null && this.BelongingInfo.LocationInfoList.Count > 0))
                {
                    // 場所階層の指定なしの場合、所属場所階層を渡す
                    locationIdList = getBelongLocationIdList();
                    useBelongingInfo = true;
                }

                if (locationIdList.Count > 0)
                {
                    //配下の構成IDを取得
                    locationIdList = GetLowerStructureIdList(locationIdList);
                    if (!useBelongingInfo && this.BelongingInfo.LocationInfoList != null && this.BelongingInfo.LocationInfoList.Count > 0)
                    {
                        //場所階層の指定ありの場合

                        //所属場所階層の配下の構成IDを取得
                        List<int> belongLocationIdList = GetLowerStructureIdList(getBelongLocationIdList());
                        //所属場所階層と一致するもののみ抽出
                        locationIdList = locationIdList.Where(x => belongLocationIdList.Contains(x)).ToList();
                    }
                    if (locationIdList.Count == 0)
                    {
                        //所属に無いもののみが指定されていた場合、データを表示しない
                        locationIdList.Add(-1);
                    }

                    // WHERE句を生成
                    existsWhere = true;
                    sbSql.Append("WHERE ");

                    // SQL取得
                    GetFixedSqlStatement(SqlName.SubDir, SqlName.CreateTableTempLocation, out string createSql);
                    // 場所階層用の一時テーブル作成
                    this.db.Regist(createSql);
                    // SQL取得
                    GetFixedSqlStatement(SqlName.SubDir, SqlName.InsertTempLocation, out string insertSql);
                    //カンマ区切りの文字列にする
                    string locationIds = string.Join(',', locationIdList);
                    // 場所階層用の一時テーブルへ構成IDを登録
                    this.db.Regist(insertSql, new { LocationIds = locationIds });
                    // EXISTS句追加
                    sbSql.Append(getExistsText(TempTableName.Location, CommonColumnName.LocationId));
                }
            }

            // 予備品場所階層構成IDリストを取得
            if (selectSql.Contains(CommonColumnName.PartsLocationId))
            {
                List<int> locationIdList = getIdListBySearchCond(STRUCTURE_CONSTANTS.CONDITION_KEY.Location);
                if ((locationIdList == null || locationIdList.Count == 0) &&
                   (this.BelongingInfo.LocationInfoList != null && this.BelongingInfo.LocationInfoList.Count > 0))
                {
                    // 場所階層の指定なしの場合、所属場所階層を渡す
                    locationIdList = this.BelongingInfo.LocationInfoList.Select(x => x.StructureId).ToList();
                }

                if (locationIdList.Count > 0)
                {
                    locationIdList = GetPartsStructureIdList(locationIdList);

                    // WHERE句を生成
                    existsWhere = true;
                    sbSql.Append("WHERE ");

                    // SQL取得
                    GetFixedSqlStatement(SqlName.SubDir, SqlName.CreateTableTempLocation, out string createSql);
                    // 予備品場所階層用の一時テーブル作成
                    this.db.Regist(createSql);
                    // SQL取得
                    GetFixedSqlStatement(SqlName.SubDir, SqlName.InsertTempLocation, out string insertSql);
                    //カンマ区切りの文字列にする
                    string locationIds = string.Join(',', locationIdList);
                    // 予備品場所階層用の一時テーブルへ構成IDを登録
                    this.db.Regist(insertSql, new { LocationIds = locationIds });
                    // EXISTS句追加
                    sbSql.Append(getExistsText(TempTableName.Location, CommonColumnName.PartsLocationId));
                }
            }

            // 職種機種構成IDリストを取得
            if (selectSql.Contains(CommonColumnName.JobId))
            {
                bool useBelongingInfo = false;
                List<int> jobIdList = getIdListBySearchCond(STRUCTURE_CONSTANTS.CONDITION_KEY.Job);

                if ((jobIdList == null || jobIdList.Count == 0) &&
                    (this.BelongingInfo.JobInfoList != null && this.BelongingInfo.JobInfoList.Count > 0))
                {
                    // 職種機種の指定なしの場合、所属職種機種を渡す
                    jobIdList = this.BelongingInfo.JobInfoList.Select(x => x.StructureId).ToList();
                    useBelongingInfo = true;
                }

                if (jobIdList.Count > 0)
                {
                    jobIdList = GetLowerStructureIdList(jobIdList);
                    if (!useBelongingInfo && this.BelongingInfo.JobInfoList != null && this.BelongingInfo.JobInfoList.Count > 0)
                    {
                        //職種機種の指定ありの場合

                        //所属職種の配下の構成IDを取得
                        List<int> belongJobIdList = GetLowerStructureIdList(this.BelongingInfo.JobInfoList.Select(x => x.StructureId).ToList());
                        //所属職種と一致するもののみ抽出
                        jobIdList = jobIdList.Where(x => belongJobIdList.Contains(x)).ToList();
                    }
                    if (jobIdList.Count == 0)
                    {
                        //所属に無いもののみが指定されていた場合、データを表示しない
                        jobIdList.Add(-1);
                    }
                    if (existsWhere)
                    {
                        sbSql.Append("AND ");
                    }
                    else
                    {
                        existsWhere = true;
                        sbSql.Append("WHERE ");
                    }

                    // SQL取得
                    GetFixedSqlStatement(SqlName.SubDir, SqlName.CreateTableTempJob, out string createSql);
                    // 職種機種用の一時テーブル作成
                    this.db.Regist(createSql);
                    // SQL取得
                    GetFixedSqlStatement(SqlName.SubDir, SqlName.InsertTempJob, out string insertSql);
                    //カンマ区切りの文字列にする
                    string locationIds = string.Join(',', jobIdList);
                    // 職種機種用の一時テーブルへ構成IDを登録
                    this.db.Regist(insertSql, new { LocationIds = locationIds });
                    // EXISTS句追加
                    sbSql.Append(getExistsText(TempTableName.Job, CommonColumnName.JobId));
                }
            }

            // 詳細検索条件辞書を取得
            var list = this.searchConditionDictionary.Where(x => x.ContainsKey("IsDetailCondition") && x.ContainsKey("CTRLID")).ToList();
            if (list != null && list.Count > 0)
            {
                var dic = list.Where(x => x["IsDetailCondition"].Equals(true) && x["CTRLID"].Equals(pageInfo.CtrlId)).FirstOrDefault();
                var mappingList = this.mapInfoList.Where(x => x.CtrlId.Equals(pageInfo.CtrlId)).ToList();
                foreach (var data in dic)
                {
                    var key = data.Key;
                    var val = data.Value;
                    var isNullSearch = false;
                    if (!key.StartsWith("VAL") || ComUtil.IsNullOrEmpty(val)) { continue; }
                    if (key.EndsWith("_nullSearch") && Convert.ToBoolean(val))
                    {
                        // NULL検索有りの場合
                        key = data.Key.Replace("_nullSearch", "");
                        val = dic[key];
                        isNullSearch = true;
                    }
                    var mapInfo = mappingList.Where(x => x.ValName.Equals(key)).FirstOrDefault();
                    if (mapInfo == null) { continue; }
                    var sqlW = ComUtil.GetDetailSearchConditionSqlAndParam(val.ToString(), mapInfo, ref dicResult);
                    if (string.IsNullOrEmpty(sqlW))
                    {
                        if (!isNullSearch)
                        {
                            continue;
                        }
                        else
                        {
                            if (mapInfo.DataType != DBColumnDataType.String)
                            {
                                //sqlW = string.Format("{0} IS NULL", mapInfo.ColName);
                                sqlW = string.Format("{0} IS NULL", mapInfo.DetailSearchColName);
                            }
                            else
                            {
                                //sqlW = string.Format("({0} IS NULL OR {0} = '')", mapInfo.ColName);
                                sqlW = string.Format("({0} IS NULL OR {0} = '')", mapInfo.DetailSearchColName);
                            }
                        }
                    }
                    if (existsWhere)
                    {
                        sbSql.Append("AND ");
                    }
                    else
                    {
                        existsWhere = true;
                        sbSql.Append("WHERE ");
                    }
                    sbSql.AppendLine(sqlW);
                    isDetailConditionApplied = true;
                }
            }
            sql = sbSql.ToString();
            return true;

            // 検索条件ディクショナリから指定したキーでIDのリストを取得する処理
            List<int> getIdListBySearchCond(string keyName)
            {
                List<int> results = new();
                var dic = this.searchConditionDictionary.Where(x => x.ContainsKey(keyName)).FirstOrDefault();
                if (dic != null && dic.ContainsKey(keyName))
                {
                    // 選択された構成IDリストから配下の構成IDをすべて取得
                    results = dic[keyName] as List<int>;
                }
                return results;
            }

            // 一時テーブルに保存した構成IDと一致するデータを絞り込むEXISTS句を生成
            string getExistsText(string tempTableName, string columnName)
            {
                string sqlText = "";
                if(!string.IsNullOrEmpty(tempTableName) && !string.IsNullOrEmpty(columnName))
                {
                    sqlText = "EXISTS(SELECT * FROM " + tempTableName + " temp WHERE " + columnName + " = temp.structure_id)";
                }

                return sqlText;
            }
        }

        /// <summary>
        /// 所属場所階層を取得
        /// </summary>
        /// <returns></returns>
        protected List<int> getBelongLocationIdList()
        {
            List<int> results = new();
            //本務工場
            int factoryId = this.BelongingInfo.LocationInfoList.Where(x => x.DutyFlg).Select(x => x.FactoryId).FirstOrDefault();
            //本務工場配下の所属場所階層が設定されているか
            int count = this.BelongingInfo.LocationInfoList.Where(x => !x.DutyFlg && x.FactoryId == factoryId).Count();
            if (factoryId > 0 && count > 0)
            {
                //本務工場配下の所属場所階層が設定されている場合、本務フラグ=trueのデータを除外
                results = this.BelongingInfo.LocationInfoList.Where(x => !x.DutyFlg).Select(x => x.StructureId).ToList();
            }
            else
            {
                //本務工場配下は全て参照可
                results = this.BelongingInfo.LocationInfoList.Select(x => x.StructureId).ToList();
            }
            return results;
        }

        /// <summary>
        /// 選択された構成IDリストから配下の構成IDをすべて取得
        /// </summary>
        /// <param name="baseIdList">構成IDリスト</param>
        /// <returns>配下の構成IDリスト</returns>
        public List<int> GetLowerStructureIdList(List<int> baseIdList)
        {
            //カンマ区切りの文字列にする
            string ids = string.Join(',', baseIdList);

            // SQL取得
            GetFixedSqlStatement(SqlName.SubDir, SqlName.GetStructureLowerList, out string sql);

            // 選択された構成IDリストから配下の構成IDをすべて取得
            IList<int> list = this.db.GetList<int>(sql, new { StructureIdList = ids });
            return list.ToList();
        }

        /// <summary>
        /// 選択された構成IDリストから予備品棚の構成IDをすべて取得
        /// </summary>
        /// <param name="baseIdList">構成IDリスト</param>
        /// <returns>棚の構成IDリスト</returns>
        public List<int> GetPartsStructureIdList(List<int> baseIdList)
        {
            //カンマ区切りの文字列にする
            string ids = string.Join(',', baseIdList);

            // SQL取得
            GetFixedSqlStatement(SqlName.SubDir, SqlName.GetPartsLocationList, out string sql);

            // 選択された構成IDリストから予備品棚の構成IDをすべて取得
            IList<int> list = this.db.GetList<int>(sql, new { UserId = this.UserId, StructureIdList = ids });
            return list.ToList();
        }

        /// <summary>
        /// 場所階層ツリーの値リストを取得
        /// </summary>
        /// <returns>場所階層ツリーの値リスト</returns>
        protected List<int> GetLocationTreeValues()
        {
            return getTreeValues(STRUCTURE_CONSTANTS.CONDITION_KEY.Location);
        }

        /// <summary>
        /// 職種階層ツリーの値リストを取得
        /// </summary>
        /// <returns>職種階層ツリーの値リスト</returns>
        protected List<int> GetJobTreeValues()
        {
            return getTreeValues(STRUCTURE_CONSTANTS.CONDITION_KEY.Job);
        }

        /// <summary>
        /// 指定された階層ツリーの値リストを取得
        /// </summary>
        /// <param name="keyName">ツリーの検索条件のキー名</param>
        /// <returns>階層ツリーの値リスト</returns>
        private List<int> getTreeValues(string keyName)
        {
            // 検索条件よりキー名に合致する内容を取得
            var dic = this.searchConditionDictionary.Where(x => x.ContainsKey(keyName)).FirstOrDefault();
            if (dic != null && dic.ContainsKey(keyName))
            {
                return dic[keyName] as List<int>;
            }
            return null;
        }
        #endregion

        #region スケジュール一覧関連
        /// <summary>
        /// 検索結果の一覧とスケジュールの一覧の紐づけに用いるキー値列のVAL値を取得
        /// </summary>
        /// <param name="listCtrlId">検索結果の一覧のID</param>
        /// <returns>キー値の列のVAL値</returns>
        protected string GetValKeyIdForSchedule(string listCtrlId)
        {
            // 一覧のキー値のVAL値を取得(スケジュールとの紐付に使用)
            return getValName(listCtrlId, "KeyId");
        }
        /// <summary>
        /// 検索結果の一覧とスケジュールの一覧の紐づけに用いる同一スケジュールキー値列のVAL値を取得
        /// </summary>
        /// <param name="listCtrlId">検索結果の一覧のID</param>
        /// <returns>キー値の列のVAL値</returns>
        protected string GetValSameMarkKeyForSchedule(string listCtrlId)
        {
            // 一覧の同一スケジュールキー値のVAL値を取得(スケジュールとの紐付に使用)
            return getValName(listCtrlId, "SameMarkKey");
        }

        /// <summary>
        /// 一覧の項目キー名からVAL値を取得
        /// </summary>
        /// <param name="listCtrlId">検索結果の一覧のID</param>
        /// <param name="name">VAL値を取得する項目キー名</param>
        /// <returns>VAL値</returns>
        private string getValName(string listCtrlId, string name)
        {
            var mapInfo = getResultMappingInfo(listCtrlId);
            var valKeyId = mapInfo.getValName(name);
            return valKeyId;
        }
        /// <summary>
        /// 検索結果の一覧に、スケジュールの一覧を追加
        /// </summary>
        /// <param name="scheduleData">スケジュールの一覧</param>
        /// <param name="listCtrlId">一覧のコントロールID</param>
        /// <remarks>一覧はKeyIdをKeyNameに持つこと</remarks>
        protected void SetScheduleDataToResult(Dictionary<string, Dictionary<string, string>> scheduleData, string listCtrlId)
        {
            // 一覧のキー値のVAL値を取得(スケジュールとの紐付に使用)
            var valKeyId = GetValKeyIdForSchedule(listCtrlId);
            // 一覧に設定されているディクショナリの内容を取得
            var targetDics = ComUtil.GetDictionaryListByCtrlId(this.ResultList, listCtrlId);
            var rowDics = targetDics.Where(x => int.Parse(x["ROWNO"].ToString()) > 0).ToList(); // ROWNO=0は一覧の情報なので除外
            // 一覧を繰り返し、一覧とスケジュールデータのキー値が同じところにスケジュールデータを追加
            foreach (var row in rowDics)
            {
                // 一覧のキー値
                string keyValue = row[valKeyId].ToString();
                // スケジュールデータに存在しない場合、次の行へ
                if (!scheduleData.ContainsKey(keyValue))
                {
                    continue;
                }
                // スケジュールデータに存在する場合
                // スケジュールデータのキー値で取得したディクショナリ(列の年月とマークの情報)
                var scheduleRow = scheduleData[keyValue];
                foreach (var item in scheduleRow)
                {
                    // 一覧に追加
                    row.Add(item.Key, item.Value);
                }
            }
        }
        #endregion

        #region 汎用的に使用できる造りだが、今のところスケジュール表示のみ
        /// <summary>
        /// システム年度設定処理
        /// </summary>
        /// <typeparam name="T">システム年度を設定する一覧のクラス CommonDataBaseClass.ISysFiscalYearを実装すること</typeparam>
        /// <param name="ctrlId">システム年度を設定する一覧のID</param>
        /// <param name="monthStartNendo">年度開始月</param>
        protected void SetSysFiscalYear<T>(string ctrlId, int monthStartNendo)
          where T : CommonDataBaseClass.ISysFiscalYear, new()
        {
            // 条件から指定された一覧(スケジュール表示条件)を取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);
            bool isExecute = false; // 処理実施フラグ
            // メンバで繰り返し
            foreach (var item in targetDic)
            {
                // VALから始まらない場合は次へ
                if (string.IsNullOrEmpty(item.Key)) { continue; }
                if (!item.Key.StartsWith("VAL")) { continue; }
                // 値が無い場合は次へ
                if (item.Value == null) { continue; }
                // 値がシステム年度でない場合は次へ
                string orgValue = item.Value.ToString();
                if (!isExistsSysYear(orgValue)) { continue; }

                // システム年度の文言を計算した年度の値に置き換える
                string updValue = string.Empty; // 新たに設定する値
                var list = orgValue.Split(CommonConstants.SeparatorFromTo); // From-Toの場合は分割
                // システム日付から年度を取得(年度開始日の年)
                int sysFiscalYear = ComUtil.GetNendoStartDay(DateTime.Now, monthStartNendo).Year;
                // 分割したリストの件数分繰り返し(From-Toでない場合は1回)
                foreach (var member in list.Select((value, index) => new { value, index }))
                {
                    if (!isExistsSysYear(member.value))
                    {
                        // 値がシステム年度でない場合は今の値を設定して次へ
                        // 固定～システム年度　のような場合にありうる
                        updValue = getNewValue(member.value, member.index, updValue);
                        continue;
                    }
                    // +-で計算する場合があるので、計算
                    int setYear = calcYear(sysFiscalYear, member.value);
                    updValue = getNewValue(setYear.ToString(), member.index, updValue);
                }
                targetDic[item.Key] = updValue; // ディクショナリの値を上書き
                isExecute = true; // 処理実施フラグを立てる
            }

            if (!isExecute)
            {
                // 処理未実施(初期化していない)なら終了
                return;
            }

            // ページ情報を取得(条件エリアの場合はページ遷移前処理で条件エリアの定義を含めること)
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);
            // データクラスに変換
            T cond = new();
            SetDataClassFromDictionary(targetDic, ctrlId, cond);

            // From-Toは|区切りなので値を設定する
            cond.JoinFromTo();

            // 画面に設定
            SetSearchResultsByDataClass(pageInfo, new List<T>() { cond }, 1);

            // 特に意味は無いけど以降のメソッドと処理を混在しないために追加
            return;

            // 値にシステム年度の文字列が含まれているか判定
            bool isExistsSysYear(string value)
            {
                return value.Contains(CommonConstants.SystemFiscalYearText);
            }
            // 追加する値と現在の値から設定する値を取得
            // addValue 追加する、現在のアイテムの値
            // memberIndex 現在のアイテムが値を|で分割したリストのインデックス
            // oldValue 設定する値、これに現在のアイテムの値を追加して返す
            string getNewValue(string addValue, int memberIndex, string oldValue)
            {
                string result = oldValue;
                if (memberIndex > 0)
                {
                    // 先頭でなければ"|"を付ける、From-To以外ないはず
                    result += CommonConstants.SeparatorFromTo;
                }
                return result + addValue;
            }
            // 初期値がシステム年度より計算する場合、計算した結果を返す処理
            // sysYear システム年度の値
            // value 初期値の値 SysFiscalYear+5 のような値
            int calcYear(int sysYear, string value)
            {
                int calcYear = sysYear;
                // +と-の位置
                int plusIndex = value.IndexOf("+");
                int minusIndex = value.IndexOf("-");
                if (plusIndex > -1 || minusIndex > -1)
                {
                    // +-いずれかがある場合、計算。どちらもある場合は無い想定
                    bool isPlus = plusIndex > minusIndex; // インデックスのどちらかが-1なので、大きい方が有効な符号
                    int index = isPlus ? plusIndex : minusIndex;
                    // 計算する値は、有効な富豪の次の文字
                    int calcValue = int.Parse(value.Substring(index + 1));
                    // 計算、マイナスなら-1をかけて足し算
                    calcYear += (calcValue * (isPlus ? +1 : -1));
                }
                return calcYear;
            }
        }
        #endregion

        #region ファイルダウンロード関連
        /// <summary>
        /// 画面で指定したパスをZIP圧縮しダウンロード可能にする
        /// </summary>
        /// <param name="targetZipFilePath">ZIP圧縮したファイルを置く場所のパス</param>
        /// <param name="sourceZipPath">ZIP圧縮するフォルダ(ファイル)のパス</param>
        /// <param name="downloadZipName">ダウンロードするZIPファイルの名称</param>
        /// <returns>エラーの場合False</returns>
        protected bool SetDownloadZip(string targetZipFilePath, string sourceZipPath, string downloadZipName)
        {
            try
            {
                // ZIP圧縮
                if (!ComUtil.FileZip(sourceZipPath, targetZipFilePath, string.Empty))
                {
                    // エラー
                    return false;
                }

                // ZIP圧縮したファイルをダウンロードさせる
                var zipStream = new MemoryStream();
                using (FileStream file = new(targetZipFilePath, FileMode.Open, FileAccess.Read))
                {
                    file.CopyTo(zipStream);
                }
                // 画面の出力へ設定
                this.OutputFileType = CommonConstants.REPORT.FILETYPE.ZIP;
                this.OutputFileName = downloadZipName;
                this.OutputStream = zipStream;
            }
            catch (Exception ex)
            {
                // 例外発生時、ZIPファイルを作成している場合は削除して終了
                if (File.Exists(targetZipFilePath))
                {
                    File.Delete(targetZipFilePath);
                }
                throw ex;
            }


            return true;
        }
        #endregion

        #region ファイルダウンロードリンク関連
        /// <summary>
        /// ファイルダウンロード　エラー情報設定
        /// </summary>
        protected void OutputFileDownloadError()
        {
            // ダウンロードに失敗しました。
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID111160052 });
            this.Status = CommonProcReturn.ProcStatus.Error;
        }

        /// <summary>
        /// ファイルダウンロード　ダウンロードファイル出力
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="filePath">ファイルのフルパス</param>
        /// <returns>エラーの場合False</returns>
        protected bool OutputDownloadFile(string fileName, string filePath)
        {
            if (!File.Exists(filePath))
            {
                // ファイルが存在しない場合
                OutputFileDownloadError();
                return false;
            }

            // ZIP圧縮したファイルをダウンロードさせる
            var fileStream = new MemoryStream();
            using (FileStream file = new(filePath, FileMode.Open, FileAccess.Read))
            {
                file.CopyTo(fileStream);
            }
            // 画面の出力へ設定
            this.OutputFileType = CommonConstants.REPORT.FILETYPE.UNDEFINED;
            this.OutputFileName = fileName;
            this.OutputStream = fileStream;

            return true;
        }
        #endregion

        /// <summary>
        /// URL直接起動の場合、参照できるデータかチェック
        /// </summary>
        /// <param name="locationStructureId">場所階層ID</param>
        /// <param name="jobStructureId">職種機種階層ID</param>
        /// <returns>チェックOKの場合true、NGの場合false</returns>
        protected bool CheckAccessUserBelong(int locationStructureId, int jobStructureId)
        {
            bool checkFlg = true;
            //所属場所階層の配下の構成IDを取得
            List<int> belongLocationIdList = GetLowerStructureIdList(getBelongLocationIdList());
            if (!belongLocationIdList.Contains(locationStructureId))
            {
                //所属場所階層に参照データの場所階層IDが含まれていない場合、NG
                checkFlg = false;
            }

            //所属職種の配下の構成IDを取得
            List<int> belongJobIdList = GetLowerStructureIdList(this.BelongingInfo.JobInfoList.Select(x => x.StructureId).ToList());
            if (!belongJobIdList.Contains(jobStructureId))
            {
                //所属職種に参照データの職種機種階層IDが含まれていない場合、NG
                checkFlg = false;
            }
            if (!checkFlg)
            {
                this.Status = CommonProcReturn.ProcStatus.Warning;
                //権限がありません。
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941090002 });
            }

            return checkFlg;
        }
		
		//↓↓↓2022/12/14 CommonSTDUtilへ移植↓↓↓
        ///// <summary>
        ///// 固定SQL文の取得
        ///// </summary>
        ///// <param name="subDir">Resources\sql配下のサブディレクトリパス(サブディレクトリが複数階層の場合、パス区切り文字は"\"ではなく".")</param>
        ///// <param name="fileName">SQLテキストファイル名</param>
        ///// <param name="sql">out 取得したSQL文</param>
        ///// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
        ///// <returns>取得結果(true:取得OK/false:取得NG )</returns>
        //private static bool GetFixedSqlStatement(string subDir, string fileName, out string sql, List<string> listUnComment = null)
        //{
        //    sql = string.Empty;
        //    string assemblyName = CommonWebTemplate.AppCommonObject.Config.AppSettings.FixedSqlStatementAssemblyName;

        //    // リソース名を生成
        //    StringBuilder resourceName = new StringBuilder();
        //    resourceName.Append(assemblyName).Append(".").Append(CommonWebTemplate.AppCommonObject.Config.AppSettings.FixedSqlStatementDir).Append(".");
        //    if (!string.IsNullOrEmpty(subDir))
        //    {
        //        // サブディレクトリパスの追加(念のためパス区切り文字を"."に変換しておく)
        //        resourceName.Append(subDir.Replace(@"\", ".")).Append(".");
        //    }
        //    resourceName.Append(fileName).Append(".sql");

        //    // 埋め込みリソースからSQL文を取得
        //    bool result = ComUtil.GetEmbeddedResourceStr(assemblyName, resourceName.ToString(), out sql);

        //    // SQLの動的制御　コメントアウトを解除
        //    if (listUnComment != null && listUnComment.Count > 0)
        //    {
        //        foreach (string wordUnComment in listUnComment)
        //        {
        //            // @+指定された文字列がコメントアウトの前後に付いているので除去
        //            // /*@Hoge と @Hoge*/ を除去すれば、囲われたSQLが有効になる
        //            Regex startReplace = new Regex("\\/\\*@" + wordUnComment + "[^a-zA-Z\\d_]");
        //            sql = startReplace.Replace(sql, string.Empty).Replace("@" + wordUnComment + "*/", string.Empty);
        //        }
        //    }

        //    return result;
        //}
		//↑↑↑2022/12/14 CommonSTDUtilへ移植↑↑↑
    }

    /// <summary>
    /// 一時テーブル検索条件
    /// </summary>
    public class TmpTableSearchCondition
    {
        public string UserId { get; set; }
        public string GUID { get; set; }
        public string TabNo { get; set; }
        public int PageSize { get; set; }
        public int Offset { get; set; }
        public int StartRowNo { get; set; }
        public int EndRowNo { get; set; }
        //public string SortColName { get; set; }
        public string Ctrlid { get; set; }
        public string Updtag { get; set; }
        public string Seltag { get; set; }
        public string LockData { get; set; }
    }

    #region 共通機能対応
    /// <summary>
    /// 共通機能から、呼出元機能へ画面の情報を渡す必要のある項目
    /// </summary>
    /// <remarks>各機能で、CommonBusinessLogicBaseを継承した状態でないと使用できない変数もあるので、各共通機能でコピー処理は実装してください。</remarks>
    public class CommonMemberInput
    {
        /// <summary>ページ情報リスト</summary>
        public List<PageInfo> pageInfoList { get; set; }
        /// <summary>マッピング情報リスト</summary>
        public IList<ComUtil.DBMappingInfo> mapInfoList { get; set; }
        /// <summary>DB操作クラス</summary>
        public CommonDBManager.CommonDBManager db { get; set; }
        /// <summary>メッセージリソース</summary>
        public ComUtil.MessageResources messageResources { get; set; }
        /// <summary>入力ファイル情報(HttpPostedFileBase)</summary>
        public IFormFile[] InputStream { get; set; }
        /// <summary>言語ID</summary>
        public string LanguageId { get; set; }
        /// <summary>機能ID</summary>
        public string ConductId { get; set; }
        /// <summary>ユーザID</summary>
        public string UserId { get; set; }
    }
    /// <summary>
    /// 呼出元機能から、共通機能機能へ画面の情報を返す必要のある項目
    /// </summary>
    /// <remarks>各機能で、CommonBusinessLogicBaseを継承した状態でないと使用できない変数もあるので、各共通機能でコピー処理は実装してください。</remarks>
    public class CommonMemberOutput
    {
        /// <summary>ステータス</summary>
        public int Status { get; set; }
        /// <summary>メッセージコード</summary>
        public string MsgId { get; set; }
        /// <summary>処理の戻り値</summary>
        public int returnValue { get; set; }
    }
    #endregion
    #region エクセル出力
    /// <summary>
    /// 呼び出し側から、どの列がキー列なのかを渡すためのクラス
    /// クラスのプロパティ名("LongPlanId"など)を設定、無しなら空、2が無ければ以降は無し
    /// </summary>
    public class Key
    {
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }
        public Key(string key1, string key2 = "", string key3 = "")
        {
            this.Key1 = key1;
            this.Key2 = key2;
            this.Key3 = key3;
        }
    }
    public class SelectKeyData
    {
        public object Key1 { get; set; }
        public object Key2 { get; set; }
        public object Key3 { get; set; }
    }
    #endregion

}