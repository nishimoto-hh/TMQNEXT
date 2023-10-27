using CommonExcelUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonSTDUtil.CommonDBManager;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using static CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using ComBase = CommonSTDUtil.CommonDataBaseClass;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = CommonTMQUtil.CommonTMQUtilDataClass;
using ExcelUtil = CommonExcelUtil.CommonExcelUtil;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace CommonTMQUtil
{
    /// <summary>
    /// TMQ用共通ユーティリティクラス
    /// </summary>
    public static partial class CommonTMQUtil
    {
        #region ExcelPort共通処理
        /// <summary>
        /// ダウンロード条件クラス
        /// </summary>
        public class ExcelPortDownloadCondition : ComDao.SearchCommonClass
        {
            /// <summary>機能ID(隠し項目)</summary>
            public string HideConductId { get; set; }
            /// <summary>シート番号(隠し項目)</summary>
            public int HideSheetNo { get; set; }
            /// <summary>機能ID</summary>
            public string ConductId { get { return this.HideConductId; } }
            /// <summary>シート番号</summary>
            public int SheetNo { get { return this.HideSheetNo; } }
            /// <summary>追加条件区分</summary>
            public int? AddCondition { get; set; }
            /// <summary>メンテナンス対象</summary>
            public string MaintenanceTarget { get; set; }
            /// <summary>工場ID</summary>
            public int? FactoryId { get; set; }
            /// <summary>工場単一選択必須区分</summary>
            public int? FactorySingleSelectionDivision { get; set; }
            /// <summary>発生日(From)</summary>
            public DateTime OccurrenceDateFrom { get; set; }
            /// <summary>発生日(To)</summary>
            public DateTime OccurrenceDateTo { get; set; }
            /// <summary>着工予定日(From)</summary>
            public DateTime ExpectedConstructionDateFrom { get; set; }
            /// <summary>着工予定日(To)</summary>
            public DateTime ExpectedConstructionDateTo { get; set; }
            /// <summary>完了日(From)</summary>
            public DateTime CompletionDateFrom { get; set; }
            /// <summary>完了日(To)</summary>
            public DateTime CompletionDateTo { get; set; }
            /// <summary>完了区分</summary>
            public int CompletionDivision { get; set; }
        }

        /// <summary>
        /// アップロード条件クラス
        /// </summary>
        public class ExcelPortUploadCondition
        {
            /// <summary>ExcelPortバージョン</summary>
            public decimal ExcelPortVersion { get; set; }
            /// <summary>機能ID</summary>
            public string ConductId { get; set; }
            /// <summary>シート番号</summary>
            public int SheetNo { get; set; }
        }

        /// <summary>
        /// ExcelPort共通
        /// </summary>
        public class ComExcelPort
        {
            #region 定数
            public static class ControlGroupId
            {
                /// <summary>コントロールグループID：条件フォーマット</summary>
                public const string ConditionFmt = "BODY_000_00_LST_{0}";
                /// <summary>コントロールグループID：アップロード</summary>
                public const string Upload = "LIST_000_1";
            }

            /// <summary>
            /// プログラムID
            /// </summary>
            public static class ProgramId
            {
                /// <summary>プログラムID：ExcelPortダウンロード</summary>
                public const string Download = "EP0001";
                /// <summary>プログラムID：ExcelPortアップロード</summary>
                public const string Upload = "EP0002";
            }

            /// <summary>
            /// ExcelPort用テンプレートファイル
            /// </summary>
            public static class Template
            {
                /// <summary>工場ID：ExcelPort用テンプレートファイル</summary>
                public const int FactoryId = 0;
                /// <summary>帳票ID：ExcelPort用テンプレートファイル</summary>
                public const string ReportId = "RP1000";
                /// <summary>テンプレートID：ExcelPort用テンプレートファイル</summary>
                public const int TemplateId = 1;
                /// <summary>出力パターンID：ExcelPort用テンプレートファイル</summary>
                public const int PatternId = 1;
                /// <summary>出力時シート番号：ExcelPort用テンプレートファイル</summary>
                public const int OutputSheetNo = 1;
            }

            /// <summary>
            /// シート番号
            /// </summary>
            public static class SheetNo
            {
                /// <summary>シート番号：エラー情報シート</summary>
                public const int ErrorInfo = 9;
                /// <summary>シート番号：レイアウト定義情報シート</summary>
                public const int DefineInfo = 10;
                /// <summary>シート番号：アイテム情報シート</summary>
                public const int ItemInfo = 11;
                /// <summary>シート番号：翻訳情報シート</summary>
                public const int TranslationInfo = 12;

                /// <summary>シート番号：エラー情報シート(ダウンロード後)</summary>
                public const int ErrorInfoDownloaded = 2;
            }

            /// <summary>
            /// シート名
            /// </summary>
            public static class SheetName
            {
                /// <summary>シート番号：エラー情報シート</summary>
                public const string ErrorInfo = "Sheet_Error";
                /// <summary>シート番号：レイアウト定義情報シート</summary>
                public const string DefineInfo = "Sheet_Define";
                /// <summary>シート番号：アイテム情報シート</summary>
                public const string ItemInfo = "Sheet_Item";
                /// <summary>シート番号：翻訳情報シート</summary>
                public const string TranslationInfo = "Sheet_Message";
            }

            /// <summary>
            /// 列種類
            /// </summary>
            public static class ColumnType
            {
                /// <summary>列種類：文字列</summary>
                public const int Text = 1;
                /// <summary>列種類：数値</summary>
                public const int Numeric = 2;
                /// <summary>列種類：日付</summary>
                public const int Date = 3;
                /// <summary>列種類：時刻</summary>
                public const int Time = 4;
                /// <summary>列種類：コンボボックス</summary>
                public const int ComboBox = 5;
                /// <summary>列種類：複数選択リストボックス</summary>
                public const int MultiListBox = 6;
                /// <summary>列種類：チェックボックス</summary>
                public const int CheckBox = 7;
                /// <summary>列種類：画面選択</summary>
                public const int FormSelect = 8;
                /// <summary>列種類：テキストエリア</summary>
                public const int TextArea = 9;
            }

            /// <summary>
            /// SQL名
            /// </summary>
            public static class SqlName
            {
                /// <summary>ExcelPort用SQL格納先サブディレクトリ名</summary>
                public const string SubDirName = @"ExcelPort";
                /// <summary>SQL名：対象構成ID上下全階層登録用一時テーブル生成</summary>
                public const string CreateTempStructureAll = "Create_TempStructureAll";
                /// <summary>SQL名：対象構成ID上下全階層登録</summary>
                public const string InsertStructureList = "Insert_StructureList";
                /// <summary>SQL名：ファイル取込項目定義情報取得用SQL</summary>
                public const string GetInputControlDefineForExcelPort = "GetInputControlDefineForExcelPort";
                /// <summary>SQL名：ExcelPortバージョン取得用SQL</summary>
                public const string GetExcelPortVersion = "GetExcelPortVersion";
                /// <summary>SQL名：ExcelPort対象情報取得用SQL</summary>
                public const string GetExcelPortTargetInfo = "GetExcelPortTargetInfo";

                /// <summary>コンボボックスデータ取得用SQL格納先サブディレクトリ名</summary>
                public const string SubDirNameForCombo = @"Common\ExcelPort";

                /// <summary>マッピング情報取得用SQL格納先サブディレクトリ名</summary>
                public const string SubDirMapping = "Common";
                /// <summary>SQL名：マッピング情報一覧取得</summary>
                public const string GetMappingInfoList = "MappingInfo_GetList";
            }

            /// <summary>
            /// DBカラム名
            /// </summary>
            public static class ColName
            {
                /// <summary>カラム名：列種類</summary>
                public const string ColumnType = "column_type";
                /// <summary>カラム名：選択列グループID</summary>
                public const string GrpId = "ep_select_group_id";
                /// <summary>カラム名：選択列関連情報ID</summary>
                public const string RelationId = "ep_relation_id";
                /// <summary>カラム名：選択列関連情報パラメータ</summary>
                public const string RelationParam = "ep_relation_parameters";
                /// <summary>カラム名：エラー有無</summary>
                public const string ErrorExist = "error_exist";
                /// <summary>カラム名：工場ID</summary>
                public const string FactoryId = "factory_id";
            }

            /// <summary>
            /// 条件データ項目名
            /// </summary>
            public static class ConditionValName
            {
                /// <summary>項目名：場所階層IDリスト</summary>
                public const string LocationIdList = "locationIdList";
                /// <summary>項目名：職種IDリスト</summary>
                public const string JobIdList = "jobIdList";

                /// <summary>項目名：対象機能ID</summary>
                public const string TargetConductId = "TargetConductId";
                /// <summary>項目名：対象シート番号</summary>
                public const string TargetSheetNo= "TargetSheetNo";
            }

            /// <summary>
            /// レイアウト定義シート情報
            /// </summary>
            public static class DefineSheetInfo
            {
                /// <summary>列番号：ヘッダー値</summary>
                public const int ColNoHeaderVal = 15;
                /// <summary>行番号：ExcelPortバージョン番号</summary>
                public const int RowNoVersion = 1;
                /// <summary>行番号：対象機能ID</summary>
                public const int RowNoConductId = 3;
                /// <summary>行番号：対象シート番号</summary>
                public const int RowNoSheetNo = 4;

            }

            /// <summary>
            /// 列区分
            /// </summary>
            public static class ColumnDivision
            {
                /// <summary>列区分：KEY列</summary>
                public const int Key = 1;
                /// <summary>列区分：送信時処理ID列</summary>
                public const int SendProcId = 2;
                /// <summary>列区分：エラー有無列</summary>
                public const int Error = 3;
                /// <summary>列区分：工場ID列</summary>
                public const int FactoryId = 4;
                /// <summary>列区分：選択項目ID列</summary>
                public const int SelectId = 5;
            }

            /// <summary>
            /// 行番号
            /// </summary>
            public static class RowNo
            {
                /// <summary>行番号：エラー情報シートデータ開始行</summary>
                public const int ErrorInfoSheetDataStart = 3;
            }

            /// <summary>
            /// HTMLカラー値
            /// </summary>
            public static class ColorValues
            {
                /// <summary>エラー</summary>
                public const string Error = "#FF9999";
            }
            #endregion

            #region コンストラクタ
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="db"DB接続></param>
            /// <param name="userId">ユーザID</param>
            /// <param name="belongingInfo">所属情報</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="condition">実行条件</param>
            /// <param name="msgRes">メッセージリソース</param>
            public ComExcelPort(
                CommonDBManager db,
                string userId,
                BelongingInfo belongingInfo,
                string languageId,
                int formNo,
                List<Dictionary<string, object>> condition,
                ComUtil.MessageResources msgRes)
            {
                this.db = db;
                this.userId = userId;
                this.belongingInfo = belongingInfo;
                this.languageId = languageId;
                this.formNo = formNo;
                this.condition = condition;
                this.msgResources = msgRes;

                this.TargetLocationInfoList = new List<StructureInfo>();
                this.TargetJobInfoList = new List<StructureInfo>();
                this.TargetFactoryIdList = new List<int>();
                this.TargetLocationInfoListAll = new List<StructureInfo>();
                this.TargetJobInfoListAll = new List<StructureInfo>();
                this.TargetFactoryIdListAll = new List<int>();

                this.ErrorInfoList = new List<ComBase.UploadErrorInfo>();
            }
            #endregion

            #region privateメンバ変数
            /// <summary>DB接続</summary>
            CommonDBManager db;
            /// <summary>場所階層条件リスト</summary>
            private List<int> locationIdList;
            /// <summary>職種条件条件リスト</summary>
            private List<int> jobIdList;
            /// <summary>シート定義情報リスト</summary>
            private List<ReportDao.MsOutputReportSheetDefineEntity> sheetDefineList;
            /// <summary>マッピング情報リスト</summary>
            private List<CommonExcelPrtInfo> mappingInfoList;
            /// <summary>コマンド情報リスト</summary>
            private List<CommonExcelCmdInfo> cmdInfoList;
            /// <summary>テンプレート情報</summary>
            private ReportDao.MsOutputTemplateEntity templateInfo;
            /// <summary>ユーザID</summary>
            private string userId;
            /// <summary>所属情報</summary>
            private BelongingInfo belongingInfo;
            /// <summary>言語情報</summary>
            private string languageId;
            /// <summary>画面番号</summary>
            private int formNo;
            /// <summary>条件辞書</summary>
            private List<Dictionary<string, object>> condition;
            /// <summary>メッセージリソース</summary>
            private ComUtil.MessageResources msgResources;
            #endregion

            #region プロパティ
            /// <summary>ダウンロード条件コントロールグループID</summary>
            public string ConditionControlGroupId { get { return string.Format(ControlGroupId.ConditionFmt, formNo); } }
            /// <summary>ダウンロード条件</summary>
            public ExcelPortDownloadCondition DownloadCondition { get; set; }
            /// <summary>アップロード条件</summary>
            public ExcelPortUploadCondition UploadCondition { get; set; }
            /// <summary>対象場所階層情報リスト</summary>
            public List<StructureInfo> TargetLocationInfoList { get; set; }
            /// <summary>対象職種情報リスト</summary>
            public List<StructureInfo> TargetJobInfoList { get; set; }
            /// <summary>対象工場IDリスト</summary>
            public List<int> TargetFactoryIdList { get; set; }
            /// <summary>全対象場所階層情報リスト</summary>
            public List<StructureInfo> TargetLocationInfoListAll { get; set; }
            /// <summary>全対象職種情報リスト</summary>
            public List<StructureInfo> TargetJobInfoListAll { get; set; }
            /// <summary>全対象工場IDリスト</summary>
            public List<int> TargetFactoryIdListAll { get; set; }
            /// <summary>ExcelPort利用可能工場IDリスト</summary>
            public List<int> ExcelPortFactoryIdList { get; set; }
            /// <summary>変更履歴管理対象工場IDリスト</summary>
            public List<int> ApprovalFactoryIdList { get; set; }
            /// <summary>Excelコマンド処理クラス</summary>
            public CommonExcelCmd ExcelCmd { get; set; }
            /// <summary>エラー情報リスト</summary>
            public List<ComBase.UploadErrorInfo> ErrorInfoList { get; set; }

            /// <summary>エラー有無列番号</summary>
            public int ErrorColNo { get; set; }
            /// <summary>エラー有無列文字列</summary>
            public string ErrorColLetter { get
                {
                    return this.ErrorColNo > 0 ? ToAlphabet(this.ErrorColNo) : string.Empty;
                }
            }
            #endregion

            #region publicメソッド
            /// <summary>
            /// DBマッピング情報リストの取得
            /// </summary>
            /// <param name="ctrlId">コントロールID</param>
            /// <returns></returns>
            public List<ComUtil.DBMappingInfo> GetDBMappingList()
            {
                var mappingList = this.db.GetListByOutsideSql<ComUtil.DBMappingInfo>(
                    SqlName.GetMappingInfoList, SqlName.SubDirMapping, new { PgmId = ProgramId.Download, LanguageId = this.languageId }).ToList();

                return mappingList;
            }

            /// <summary>
            /// ExcelPort用テンプレートファイル情報初期化
            /// </summary>
            /// <param name="resultMsg">結果メッセージ</param>
            /// <param name="detailMsg">詳細メッセージ</param>
            /// <param name="isUpload"></param>
            /// <returns></returns>
            public bool InitializeExcelPortTemplateFile(out string resultMsg, out string detailMsg, bool isUpload = false, Dictionary<string, object>dic = null)
            {
                //==========
                // 初期化
                //==========
                resultMsg = string.Empty;
                detailMsg = string.Empty;

                // ExcelPort用マッピング情報の取得
                var mapInfoList = GetDBMappingList();

                string ctrlGrpId = this.ConditionControlGroupId;
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.condition, ctrlGrpId);
                if (isUpload) {
                    // アップロード条件の取得
                    ExcelPortUploadCondition ulCondition = new();
                    ComUtil.SetConditionByDataClass(ctrlGrpId, mapInfoList, ulCondition, targetDic, ComUtil.ConvertType.Execute);
                    this.UploadCondition = ulCondition;
                }
                else
                {
                    // ダウンロード条件の取得
                    ExcelPortDownloadCondition dlCondition = new();
                    ComUtil.SetConditionByDataClass(ctrlGrpId, mapInfoList, dlCondition, targetDic, ComUtil.ConvertType.Execute);
                    this.DownloadCondition = dlCondition;
                }

                this.locationIdList = getConditionList<int>(condition, ConditionValName.LocationIdList);
                this.jobIdList = getConditionList<int>(condition, ConditionValName.JobIdList);

                // 出力帳票シート定義情報
                this.sheetDefineList = new List<ReportDao.MsOutputReportSheetDefineEntity>();
                // マッピング情報
                this.mappingInfoList = new List<CommonExcelPrtInfo>();
                // コマンド情報
                // セルの結合や罫線を引く等のコマンド実行が必要な場合はここでセットする。不要な場合はnullでOK
                this.cmdInfoList = new List<CommonExcelCmdInfo>();

                // 場所階層条件から対象工場、対象場所階層情報を取得
                HistoryManagement history = new HistoryManagement(
                    this.db, this.userId, this.languageId, DateTime.Now, CommonTMQConstants.MsStructure.StructureId.ApplicationConduct.None);
                if (this.locationIdList == null || this.locationIdList.Count == 0 || isUpload)
                {
                    // 場所階層条件が未指定またはアップロードの場合、所属情報から取得
                    this.TargetFactoryIdList.AddRange(this.belongingInfo.BelongingFactoryIdList);
                    this.TargetLocationInfoList.AddRange(this.belongingInfo.LocationInfoList);
                }
                else
                {
                    foreach (var id in this.locationIdList)
                    {
                        var tmpId = history.getFactoryId(id);
                        if (!this.TargetFactoryIdList.Contains(tmpId))
                        {
                            this.TargetFactoryIdList.Add(tmpId);
                        }
                        this.TargetLocationInfoList.Add(new StructureInfo() { FactoryId = tmpId, StructureId = id });
                    }
                }
                this.TargetFactoryIdListAll.AddRange(this.belongingInfo.BelongingFactoryIdList);
                this.TargetLocationInfoListAll.AddRange(this.belongingInfo.LocationInfoList);

                // 職種条件から対象職種情報を取得
                if (this.jobIdList == null || this.jobIdList.Count == 0 || isUpload)
                {
                    // 職種条件が未指定またはアップロードの場合、所属情報から取得
                    this.TargetJobInfoList.AddRange(this.belongingInfo.JobInfoList);
                }
                else
                {
                    foreach (var id in this.jobIdList)
                    {
                        var tmpId = history.getFactoryIdByStructureId(id);
                        this.TargetJobInfoList.Add(new StructureInfo() { FactoryId = tmpId, StructureId = id });
                    }
                }
                this.TargetJobInfoListAll.AddRange(this.belongingInfo.JobInfoList);

                // ExcelPort対象工場の取得
                this.ExcelPortFactoryIdList = getExcelPortTargetFactoryIdList();
                var excludedFactoryIdList = new List<int>();
                if (this.ExcelPortFactoryIdList.Count == 0)
                {
                    // ExcelPort利用可能工場が存在しない場合
                    if (isUpload)
                    {
                        // 「ExcelPort利用可能工場以外の工場データは登録できません。」
                        resultMsg = GetResMessage(ComRes.ID.ID141040004, this.languageId, this.msgResources);
                    }
                    else
                    {
                        // 「ExcelPort利用可能工場以外の工場データは出力できません。」
                        resultMsg = GetResMessage(ComRes.ID.ID141040003, this.languageId, this.msgResources);
                    }
                    return false;
                }
                else
                {
                    var contains = false;
                    for (int i = this.TargetFactoryIdList.Count - 1; i >= 0; i--)
                    {
                        var id = this.TargetFactoryIdList[i];
                        if (!this.ExcelPortFactoryIdList.Contains(id))
                        {
                            // ExcelPort利用可能工場以外の場合、対象外
                            excludedFactoryIdList.Add(id);  // 除外工場
                            TargetFactoryIdList.RemoveAt(i);
                            contains = true;
                        }
                    }
                    for (int i = this.TargetFactoryIdListAll.Count - 1; i >= 0; i--)
                    {
                        if (!this.ExcelPortFactoryIdList.Contains(this.TargetFactoryIdListAll[i]))
                        {
                            // ExcelPort利用可能工場以外の場合、対象外
                            TargetFactoryIdListAll.RemoveAt(i);
                        }
                    }
                    if (!isUpload)
                    {
                        if (TargetFactoryIdList.Count == 0)
                        {
                            // 対象工場にExcelPort利用可能工場が存在しない場合
                            // 「ExcelPort利用可能工場以外の工場データは出力できません。」
                            resultMsg = GetResMessage(ComRes.ID.ID141040003, this.languageId, this.msgResources);
                            return false;
                        }
                        if (contains)
                        {
                            // ExcelPort利用可能工場以外の工場が含まれる場合
                            // 「ダウンロードされたExcelファイルにExcelPort利用可能工場以外の工場データは出力されていません。」
                            resultMsg = GetResMessage(ComRes.ID.ID141160018, this.languageId, this.msgResources);
                        }
                    }
                }

                // 変更管理対象工場の取得
                this.ApprovalFactoryIdList = history.GetUserApprovalFactoryList();
                var excludedApprovalCnt = 0;
                if (this.ApprovalFactoryIdList.Count > 0)
                {
                    var contains = false;
                    for (int i = this.TargetFactoryIdList.Count - 1; i >= 0; i--)
                    {
                        if (this.ApprovalFactoryIdList.Contains(this.TargetFactoryIdList[i]))
                        {
                            // 変更管理対象工場の場合、対象外
                            this.TargetFactoryIdList.RemoveAt(i);
                            contains = true;
                        }
                    }
                    for (int i = this.TargetFactoryIdListAll.Count - 1; i >= 0; i--)
                    {
                        if (this.ApprovalFactoryIdList.Contains(this.TargetFactoryIdListAll[i]))
                        {
                            // 変更管理対象工場の場合、対象外
                            this.TargetFactoryIdListAll.RemoveAt(i);
                        }
                    }
                    if (!isUpload)
                    {
                        foreach (var id in excludedFactoryIdList)
                        {
                            if (this.ApprovalFactoryIdList.Contains(id))
                            {
                                // ExcelPort利用可能工場以外の工場に変更管理対象工場が含まれる場合
                                excludedApprovalCnt++;
                                contains = true;
                            }
                        }
                        if (this.TargetFactoryIdList.Count == 0)
                        {
                            // 対象工場が変更管理対象工場のみの場合
                            // 「変更履歴管理対象の工場データは出力できません。」
                            resultMsg = GetResMessage(ComRes.ID.ID141290001, this.languageId, this.msgResources);
                            return false;
                        }
                        else
                        {
                            if (contains)
                            {
                                if (excludedApprovalCnt == excludedFactoryIdList.Count)
                                {
                                    // 対象外の工場が変更管理対象工場のみの場合
                                    // 「ダウンロードされたExcelファイルに変更履歴管理対象の工場データは出力されていません。」
                                    resultMsg = GetResMessage(ComRes.ID.ID141160017, this.languageId, this.msgResources);
                                }
                                else
                                {
                                    // 対象工場にExcelPort利用工場以外の工場と変更管理対象工場の両方が含まれる場合
                                    // 「ダウンロードされたExcelファイルにExcelPort利用可能工場以外の工場データおよび変更履歴管理対象の工場データは出力されていません。」
                                    resultMsg = GetResMessage(ComRes.ID.ID141160019, this.languageId, this.msgResources);
                                }
                            }
                        }
                    }
                }
                // 対象工場で対象場所階層情報と対象職種情報を絞り込む
                this.TargetLocationInfoList = this.TargetLocationInfoList.Where(x => this.TargetFactoryIdList.Contains(x.FactoryId)).ToList();
                this.TargetJobInfoList = this.TargetJobInfoList.Where(x => 
                    this.TargetFactoryIdList.Contains(x.FactoryId) || x.FactoryId == STRUCTURE_CONSTANTS.CommonFactoryId).ToList();
                this.TargetLocationInfoListAll = this.TargetLocationInfoListAll.Where(x => this.TargetFactoryIdListAll.Contains(x.FactoryId)).ToList();
                this.TargetJobInfoListAll = this.TargetJobInfoListAll.Where(x => 
                    this.TargetFactoryIdListAll.Contains(x.FactoryId) || x.FactoryId == STRUCTURE_CONSTANTS.CommonFactoryId).ToList();

                // 場所階層、職種機種絞り込み用一時テーブルを生成
                if (!createTempTableForTargetStructureInfo(true))
                {
                    detailMsg = "Failed to create the temporary table.";
                    return false;
                }
                if (!createTempTableForTargetStructureInfo(false))
                {
                    detailMsg = "Failed to create the temporary table.";
                    return false;
                }

                if (isUpload && dic != null)
                {
                    // 個別実装用データからアップロード条件を取得
                    if (!dic.ContainsKey(ConditionValName.TargetConductId) || !dic.ContainsKey(ConditionValName.TargetSheetNo))
                    {
                        // 「指定されたEXCELから更新対象機能が特定できません。」
                        resultMsg = GetResMessage(ComRes.ID.ID141120012, this.languageId, this.msgResources);
                        return false;
                    }
                    this.UploadCondition.ConductId = dic[ConditionValName.TargetConductId].ToString();
                    this.UploadCondition.SheetNo = Convert.ToInt32(dic[ConditionValName.TargetSheetNo]);
                }

                return true;
            }

            /// <summary>
            /// ExcelPortテンプレートファイル出力処理
            /// </summary>
            /// <param name="db">DB接続</param>
            /// <param name="sheetNo">対象シート番号</param>
            /// <param name="dataList">出力データ</param>
            /// <param name="fileType">ファイル種類</param>
            /// <param name="fileName">ファイル名</param>
            /// <param name="memoryStream">出力ストリーム</param>
            /// <param name="detailMsg">詳細メッセージ</param>
            /// <returns></returns>
            public bool OutputExcelPortTemplateFile(
                IList<Dictionary<string, object>> dataList,
                out string fileType,
                out string fileName,
                out MemoryStream memoryStream,
                out string detailMsg)
            {
                //==========
                // 初期化
                //==========
                int factoryId = Template.FactoryId;
                string reportId = Template.ReportId;
                int templateId = Template.TemplateId;
                int patternId = Template.PatternId;
                int sheetNo = this.DownloadCondition.SheetNo;

                // ファイルタイプ
                fileType = ComConsts.REPORT.FILETYPE.EXCEL_MACRO;
                // ダウンロードファイル名
                fileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", reportId, DateTime.Now) + ComConsts.REPORT.EXTENSION.EXCEL_MACRO_BOOK;
                // メモリストリーム
                memoryStream = new MemoryStream();
                // メッセージ
                detailMsg = "";
                // シートデータ件数
                int sheetDataCount = 0;

                // 対象機能IDとシート番号をセット
                Option option = new Option();
                option.TargetConductId = this.DownloadCondition.ConductId;
                option.TargetSheetNo = this.DownloadCondition.SheetNo;

                // 構成マスタからExcelPortバージョンを取得
                option.Version = getExcelPortVersion();

                // テンプレート情報を取得
                this.templateInfo = new ReportDao.MsOutputTemplateEntity().GetEntity(factoryId, reportId, templateId, db);
                if (this.templateInfo == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Template information does not exist. [ReportId:{0}]", reportId);
                    return false;
                }

                // 出力帳票シート定義のリストを取得
                this.sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                    ComReport.GetReportSheetDefine,
                    ExcelPath,
                    new { FactoryId = factoryId, ReportId = reportId },
                    db);
                if (sheetDefineList == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Sheet definition information does not exist. [ReportId:{0}]", reportId);
                    return false;
                }

                // レイアウト定義情報のシート定義を取得
                var sheetDefine = this.sheetDefineList.Where(x => x.SheetNo == SheetNo.DefineInfo).FirstOrDefault();
                if (sheetDefine == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Layout definition sheet information does not exist. [SheetNo:{0}]", SheetNo.DefineInfo);
                    return false;
                }

                // 対象SQLファイルにてSQLを実行し、該当シート出力用データを取得する
                string targetSql = sheetDefine.TargetSql;
                var keyList = new List<SelectKeyData>();
                keyList.Add(new SelectKeyData() { Key1 = this.DownloadCondition.SheetNo, Key2 = Template.OutputSheetNo });
                var tmpDataList = GetReportData(keyList, targetSql, db, userId, languageId, null);
                // マッピングデータ作成
                List<CommonExcelPrtInfo> mappingDataList = CreateMappingList(
                                                            factoryId,
                                                            ProgramId.Download,
                                                            reportId,
                                                            sheetDefine.SheetNo,
                                                            templateId,
                                                            patternId,
                                                            tmpDataList,
                                                            templateInfo.TemplateFileName,
                                                            templateInfo.TemplateFilePath,
                                                            languageId,
                                                            db,
                                                            out int optionRowCount,
                                                            out int optionColomnCount,
                                                            option);
                // マッピング情報リストに追加
                this.mappingInfoList.AddRange(mappingDataList);
                // 非表示シートのため、罫線は不要。必要な場合は以下のコメントを外す
                //// 一覧罫線用にデータ件数を退避
                //sheetDataCount = dataList.Count;
                //// 一覧フラグの場合
                //if (sheetDefine.ListFlg == true)
                //{
                //    // 罫線設定セル範囲を取得
                //    string range = GetTargetCellRange(factoryId, reportId, templateId, patternId, sheetDefine.SheetNo, sheetDataCount, sheetDefine.RecordCount, db, optionRowCount, optionColomnCount);
                //    if (range != null)
                //    {
                //        //範囲が取得できた場合、罫線を引く
                //        var sheetName = getDefaultSheetName(sheetDefine.SheetNo);
                //        cmdInfoList.AddRange(CommandLineBox(range, sheetDefine.SheetName));
                //    }
                //}

                // アイテム定義情報のシート定義を取得
                sheetDefine = sheetDefineList.Where(x => x.SheetNo == SheetNo.ItemInfo).FirstOrDefault();
                if (sheetDefine == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Item definition sheet information does not exist. [SheetNo:{0}]", SheetNo.ItemInfo);
                    return false;
                }

                // レイアウト定義情報からアイテム定義取得用データを抽出する
                var selectItemList = tmpDataList.Where(x =>
                    (int)((IDictionary<string, object>)x)[ColName.ColumnType] == ColumnType.ComboBox ||
                    (int)((IDictionary<string, object>)x)[ColName.ColumnType] == ColumnType.MultiListBox ||
                    (int)((IDictionary<string, object>)x)[ColName.ColumnType] == ColumnType.FormSelect).ToList();
                var itemDataList = new List<Dictionary<string, object>>();
                if (selectItemList.Count() > 0)
                {
                    var factoryIdList = new List<int>();
                    factoryIdList.AddRange(this.TargetFactoryIdList);
                    //システム共通の階層も併せて取得する
                    if (!factoryIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
                    {
                        factoryIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
                    }
                    foreach (var selectItem in selectItemList)
                    {
                        var dic = (IDictionary<string, object>)selectItem;
                        string grpId = dic[ColName.GrpId].ToString();
                        string relationId = dic[ColName.RelationId].ToString();
                        string relationParam = dic[ColName.RelationParam].ToString();
                        // コンボ選択アイテムデータの取得
                        var resultList = getComboBoxData(grpId, relationId, relationParam, factoryIdList);
                        if (resultList.Count > 0)
                        {
                            itemDataList.AddRange(resultList);
                        }
                    }
                    // マッピングデータ作成
                    mappingDataList = CreateMappingList(
                                        factoryId,
                                        ProgramId.Download,
                                        reportId,
                                        sheetDefine.SheetNo,
                                        templateId,
                                        patternId,
                                        itemDataList,
                                        templateInfo.TemplateFileName,
                                        templateInfo.TemplateFilePath,
                                        languageId,
                                        db,
                                        out optionRowCount,
                                        out optionColomnCount);
                    // マッピング情報リストに追加
                    mappingInfoList.AddRange(mappingDataList);

                    // 非表示シートのため、罫線は不要。必要な場合は以下のコメントを外す
                    //// 一覧罫線用にデータ件数を退避
                    //sheetDataCount = itemDataList.Count;
                    //// 一覧フラグの場合
                    //if (sheetDefine.ListFlg == true)
                    //{
                    //    // 罫線設定セル範囲を取得
                    //    string range = GetTargetCellRange(factoryId, reportId, templateId, patternId, sheetDefine.SheetNo, sheetDataCount, sheetDefine.RecordCount, db, optionRowCount, optionColomnCount);
                    //    if (range != null)
                    //    {
                    //        //範囲が取得できた場合、罫線を引く
                    //        var sheetName = getDefaultSheetName(sheetDefine.SheetNo);
                    //        cmdInfoList.AddRange(CommandLineBox(range, sheetDefine.SheetName));
                    //    }
                    //}
                }

                // 翻訳定義情報のシート定義を取得
                sheetDefine = sheetDefineList.Where(x => x.SheetNo == SheetNo.TranslationInfo).FirstOrDefault();
                if (sheetDefine == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Translation definition sheet information does not exist. [SheetNo:{0}]", SheetNo.TranslationInfo);
                    return false;
                }

                // 対象SQLファイルにてSQLを実行し、該当シート出力用データを取得する
                targetSql = sheetDefine.TargetSql;
                tmpDataList = GetReportData(new List<SelectKeyData>(), targetSql, db, userId, languageId, null);
                mappingDataList = CreateMappingList(
                                    factoryId,
                                    ProgramId.Download,
                                    reportId,
                                    sheetDefine.SheetNo,
                                    templateId,
                                    patternId,
                                    tmpDataList,
                                    templateInfo.TemplateFileName,
                                    templateInfo.TemplateFilePath,
                                    languageId,
                                    db,
                                    out optionRowCount,
                                    out optionColomnCount);
                // マッピング情報リストに追加
                mappingInfoList.AddRange(mappingDataList);

                // 非表示シートのため、罫線は不要。必要な場合は以下のコメントを外す
                //// 一覧罫線用にデータ件数を退避
                //sheetDataCount = dataList.Count;
                //// 一覧フラグの場合
                //if (sheetDefine.ListFlg == true)
                //{
                //    // 罫線設定セル範囲を取得
                //    string range = GetTargetCellRange(factoryId, reportId, templateId, patternId, sheetDefine.SheetNo, sheetDataCount, sheetDefine.RecordCount, db, optionRowCount, optionColomnCount);
                //    if (range != null)
                //    {
                //        //範囲が取得できた場合、罫線を引く
                //        var sheetName = getDefaultSheetName(sheetDefine.SheetNo);
                //        cmdInfoList.AddRange(CommandLineBox(range, sheetDefine.SheetName));
                //    }
                //}

                // 機能個別シートのレイアウト定義情報のシート定義を取得
                sheetDefine = this.sheetDefineList.Where(x => x.SheetNo == sheetNo).FirstOrDefault();
                if (sheetDefine == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Sheet definition information does not exist. [SheetNo:{0}]", sheetNo);
                    return false;
                }

                if (dataList == null && !string.IsNullOrEmpty(sheetDefine.TargetSql))
                {
                    // 指定データがnullで対象SQLが設定されている場合、
                    // 対象SQLファイルにてSQLを実行し、該当シート出力用データを取得する
                    targetSql = sheetDefine.TargetSql;
                    dataList = (IList<Dictionary<string, object>>)GetReportData(new List<SelectKeyData>(), targetSql, db, userId, languageId, null);
                }

                // 場所階層と職種情報を設定する
                if (dataList != null)
                {
                    // 階層情報の取得
                    setStructureLayerInfo(dataList);
                }
                else
                {
                    dataList = new List<Dictionary<string, object>>();
                }
                // マッピングデータ作成
                mappingDataList = CreateMappingList(
                                    factoryId,
                                    ProgramId.Download,
                                    reportId,
                                    sheetDefine.SheetNo,
                                    templateId,
                                    patternId,
                                    dataList,
                                    templateInfo.TemplateFileName,
                                    templateInfo.TemplateFilePath,
                                    languageId,
                                    db,
                                    out optionRowCount,
                                    out optionColomnCount);
                // マッピング情報リストに追加
                this.mappingInfoList.AddRange(mappingDataList);

                // 一覧罫線用にデータ件数を退避
                sheetDataCount = dataList.Count;
                var sheetName = getDefaultSheetName(sheetNo);
                // 一覧フラグの場合
                if (sheetDefine.ListFlg == true)
                {
                    // 罫線設定セル範囲を取得
                    string range = GetTargetCellRange(factoryId, reportId, templateId, patternId, sheetDefine.SheetNo, sheetDataCount, sheetDefine.RecordCount, db, optionRowCount, optionColomnCount);
                    if (range != null)
                    {
                        //範囲が取得できた場合、罫線を引く
                        cmdInfoList.AddRange(CommandLineBox(range, sheetName));
                    }
                }

                // シート名変更コマンドを追加
                CommonExcelCmdInfo cmdInfo = new CommonExcelCmdInfo();
                var newSheetName = GetSheetName(sheetDefine, factoryId, this.languageId, db);
                string[] param = new string[] { sheetName, newSheetName };  // シート名、変更後シート名
                cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdChangeSheetName, param);
                this.cmdInfoList.Add(cmdInfo);

                // 不要シートの削除＆作業用シートの非表示
                deleteOrHideUnnecessarySheets(sheetNo, factoryId);

                // エクセルファイル作成
                ExcelUtil.CreateExcelFile(templateInfo.TemplateFileName, templateInfo.TemplateFilePath, userId, mappingInfoList, cmdInfoList, ref memoryStream, ref detailMsg);
                return true;
            }

            /// <summary>
            /// ExcelPortアップロード条件のチェック
            /// </summary>
            /// <param name="file">Excelファイル</param>
            /// <param name="msg">メッセージ</param>
            /// <returns></returns>
            public bool CheckUploadCondition(IFormFile file, out string msg, out string conductId, out int sheetNo)
            {
                conductId = string.Empty;
                sheetNo = -1;

                // Excel読み込み
                this.ExcelCmd = TMQUtil.FileOpen(file.OpenReadStream());

                // Excelファイルから各条件を取得

                // ExcelPortバージョン番号を取得
                var versionNo = getCellValue(SheetName.DefineInfo, DefineSheetInfo.ColNoHeaderVal, DefineSheetInfo.RowNoVersion);
                if (string.IsNullOrEmpty(versionNo))
                {
                    // 「指定されたEXCELのフォーマットが不正です。」
                    msg = GetResMessage(ComRes.ID.ID141120010, this.languageId, this.msgResources);
                    return false;
                }

                // 対象機能IDを取得
                conductId = getCellValue(SheetName.DefineInfo, DefineSheetInfo.ColNoHeaderVal, DefineSheetInfo.RowNoConductId);
                if (string.IsNullOrEmpty(conductId))
                {
                    // 「指定されたEXCELのフォーマットが不正です。」
                    msg = GetResMessage(ComRes.ID.ID141120010, this.languageId, this.msgResources);
                    return false;
                }

                // 対象シート番号を取得
                var sheetNoStr = getCellValue(SheetName.DefineInfo, DefineSheetInfo.ColNoHeaderVal, DefineSheetInfo.RowNoSheetNo);
                if (string.IsNullOrEmpty(sheetNoStr))
                {
                    // 「指定されたEXCELのフォーマットが不正です。」
                    msg = GetResMessage(ComRes.ID.ID141120010, this.languageId, this.msgResources);
                    return false;
                }
                sheetNo = Convert.ToInt32(sheetNoStr);

                // 構成マスタからExcelPortバージョン番号を取得
                decimal versionNo2 = getExcelPortVersion();
                if(Convert.ToDecimal(versionNo) != versionNo2)
                {
                    // バージョン番号が一致しない場合
                    // 「指定されたEXCELはバージョンが最新ではありません。最新バージョンをダウンロードしてください。」
                    msg = GetResMessage(ComRes.ID.ID141120011, this.languageId, this.msgResources);
                    return false;
                }

                // 構成マスタから対象機能名を取得(拡張項目1=対象機能ID、拡張項目2=対象シート番号のデータ)
                string targetName = getTargetConductName(conductId, sheetNo);
                if (string.IsNullOrEmpty(targetName))
                {
                    // 「指定されたEXCELから更新対象機能が特定できません。」
                    msg = GetResMessage(ComRes.ID.ID141120012, this.languageId, this.msgResources);
                    return false;
                }

                // 「{0}データを登録します。よろしいですか？」
                msg = GetResMessage(new string[] { ComRes.ID.ID141190004, targetName }, this.languageId, this.msgResources);

                return true;
            }

            /// <summary>
            /// ExcelPortアップロード用データの取得
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="file"></param>
            /// <param name="condition"></param>
            /// <param name="resultList"></param>
            /// <param name="errorMsg"></param>
            /// <returns></returns>
            public bool GetUploadDataList<T>(IFormFile file, Dictionary<string, object> condition, string ctrlGrpId, out List<T> resultList, out string errorMsg, 
                ref string fileType, ref string fileName, ref MemoryStream ms)
            {
                resultList = new List<T>();
                errorMsg = string.Empty;

                // Excel読み込み
                if (this.ExcelCmd == null)
                {
                    this.ExcelCmd = TMQUtil.FileOpen(file.OpenReadStream());
                }

                // エラー情報シートをクリア
                long lastRowNo = this.ExcelCmd.GetLastRowNo(SheetNo.ErrorInfoDownloaded.ToString());
                this.ExcelCmd.Clear(new string[] { RowNo.ErrorInfoSheetDataStart.ToString() + ":" + lastRowNo.ToString(), "1", SheetNo.ErrorInfoDownloaded.ToString() });

                // 入力チェック＆変換
                var errorInfoList = checkUploadCondition<T>(ctrlGrpId, ref resultList, ref errorMsg);
                if(!string.IsNullOrEmpty(errorMsg))
                {
                    return false;
                }
                else if(errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// ExcelPort出力可能最大行数チェック
            /// </summary>
            /// <param name="dataCount"></param>
            /// <returns>false:最大行数オーバー</returns>
            public bool CheckDownloadMaxCnt(int dataCount)
            {
                // 構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                // 構成グループID
                param.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.ExcelPortMaxCount; // ExcelPort出力可能最大行数
                // 連番
                param.Seq = 1;
                // 構成アイテム、アイテム拡張マスタ情報取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
                if (list != null && list.Count > 0)
                {
                    // 取得情報から拡張データを取得
                    var result = list.Select(x => x.ExData).FirstOrDefault();
                    if(dataCount > int.Parse(result))
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// エラー情報シートへの設定
            /// </summary>
            /// <param name="file">ファイルデータ</param>
            /// <param name="errorInfoList">エラー情報リスト</param>
            /// <param name="fileType">ファイル種類</param>
            /// <param name="fileName">ファイル名</param>
            /// <param name="ms">メモリストリーム</param>
            /// <returns></returns>
            public bool SetErrorInfoSheet(IFormFile file, List<ComBase.UploadErrorInfo> errorInfoList, ref string fileType, ref string fileName, ref MemoryStream ms)
            {
                // エラー情報シートへ設定
                int startDataIdx = this.ErrorInfoList.Count;
                this.ErrorInfoList.AddRange(errorInfoList);
                return setErrorInfoSheet(file, startDataIdx, ref fileType, ref fileName, ref ms);
            }
            #endregion

            #region privateメソッド
            /// <summary>
            /// 条件文字列の取得
            /// </summary>
            /// <param name="condition">出力条件</param>
            /// <param name="valName">条件名</param>
            /// <returns></returns>
            private string getConditionString(List<Dictionary<string, object>> condition, string valName)
            {
                var val = condition.Where(x => x.ContainsKey(valName)).Select(y => y[valName]).FirstOrDefault();
                return val.ToString();
            }

            /// <summary>
            /// 条件文字列の取得
            /// </summary>
            /// <param name="condition">出力条件</param>
            /// <param name="valName">条件名</param>
            /// <returns></returns>
            private List<T> getConditionList<T>(List<Dictionary<string, object>> condition, string valName)
            {
                var val = condition.Where(x => x.ContainsKey(valName)).Select(y => y[valName]).FirstOrDefault();
                return val as List<T>;
            }

            /// <summary>
            /// ExcelPort対象工場を取得
            /// </summary>
            /// <returns>ExcelPort対象工場のリスト</returns>
            private List<int> getExcelPortTargetFactoryIdList()
            {
                var factoryIdList = new List<int>();
                // 拡張項目5の値がセットされた工場のリストを取得
                // 検索条件
                StructureItemEx.StructureItemExInfo param = new();
                param.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.Location; // 場所階層
                param.Seq = 5; // 拡張5が"1"のデータ
                param.ExData = "1";
                param.LanguageId = this.languageId;
                List<StructureItemEx.StructureItemExInfo> locationList = StructureItemEx.GetStructureItemExData(param, this.db);
                if (locationList == null || locationList.Count == 0)
                {
                    // 無い場合終了
                    return factoryIdList;
                }

                // 有る場合は階層番号が工場のものを絞り込み
                factoryIdList = locationList.Where(x => x.StructureLayerNo == (int)TMQConsts.MsStructure.StructureLayerNo.Location.Factory).Select(x => x.StructureId).ToList();

                return factoryIdList;
            }

            /// <summary>
            /// 対象場所階層/職種情報(上下階層)を一時テーブルへ登録
            /// </summary>
            /// <returns></returns>
            private bool createTempTableForTargetStructureInfo(bool isAll)
            {
                string unComment = isAll ? "All" : "Selected";

                // 一時テーブル生成用のSQL文字列を取得
                if (!ComUtil.GetFixedSqlStatement(SqlName.SubDirName, SqlName.CreateTempStructureAll, out string createSql, new List<string> { unComment }))
                {
                    return false;
                }

                // 一時テーブル登録用のSQL文字列を取得
                if (!ComUtil.GetFixedSqlStatement(SqlName.SubDirName, SqlName.InsertStructureList, out string insertSql, new List<string> { unComment }))
                {
                    return false;
                }

                // 対象の構成IDをカンマ区切りの文字列にする
                string structureIdList;
                if (isAll)
                {
                    structureIdList = string.Join(',', this.TargetLocationInfoListAll.Select(x => x.StructureId));
                    structureIdList += ',' + string.Join(',', this.TargetJobInfoListAll.Select(x => x.StructureId));
                }
                else
                {
                    structureIdList = string.Join(',', this.TargetLocationInfoList.Select(x => x.StructureId));
                    structureIdList += ',' + string.Join(',', this.TargetJobInfoList.Select(x => x.StructureId));
                }

                // 一時テーブルを生成
                this.db.Regist(createSql);
                // 一時テーブルへ構成IDを登録
                this.db.Regist(insertSql, new { StructureIdList = structureIdList, LanguageId = this.languageId });
                return true;
            }

            /// <summary>
            /// 構成マスタからExcelPortバージョンを取得
            /// </summary>
            /// <returns>ExelPortバージョン番号</returns>
            private decimal getExcelPortVersion()
            {
                // 構成マスタからExcelPortバージョンを取得
                var entity = TMQUtil.SqlExecuteClass.SelectEntity<AutoCompleteEntity>(SqlName.GetExcelPortVersion, SqlName.SubDirName, null, db);
                if (entity != null)
                {
                    return Convert.ToDecimal(entity.Exparam1);
                }
                else
                {
                    return decimal.MinValue;
                }
            }

            /// <summary>
            /// 構成マスタからExcelPort対象機能名を取得
            /// </summary>
            /// <returns>ExcelPort対象機能名</returns>
            private string getTargetConductName(string conductId, int sheetNo)
            {
                // 構成マスタからExcelPort対象機能名を取得
                var entity = TMQUtil.SqlExecuteClass.SelectEntity<AutoCompleteEntity>(
                    SqlName.GetExcelPortTargetInfo, SqlName.SubDirName, new { ConductId = conductId, SheetNo = sheetNo, LanguageId = this.languageId }, db);
                if (entity != null)
                {
                    return entity.Labels;
                }
                else
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// コンボボックス用SQL実行
            /// </summary>
            /// <param name="conditionDictionary"></param>
            /// <param name="rootPath"></param>
            /// <param name="list"></param>
            /// <param name="languageId"></param>
            /// <returns></returns>
            private List<Dictionary<string, object>> getComboBoxData(string grpId, string sqlId, string sqlParam, List<int> factoryIdList)
            {
                var resultList = new List<Dictionary<string, object>>();
                var condition = new ExpandoObject();
                int index = 1;
                string param = "";

                // パラメータを設定する
                if (!string.IsNullOrEmpty(sqlParam))
                {
                    string[] paramList = sqlParam.Split(','); // カンマ区切りで配列に挿入

                    for (int i = 0; i < paramList.Count(); i++)
                    {
                        param = "param" + index.ToString();
                        ((IDictionary<string, object>)condition).Add(param, paramList[i]);
                        index++;
                    }
                }

                // SQL格納フォルダ
                string sqlDir = SqlName.SubDirNameForCombo;

                // 工場IDリストの設定
                ((IDictionary<string, object>)condition).Add("factoryIdList", factoryIdList);

                // 言語コードを設定
                ((IDictionary<string, object>)condition).Add("languageId", this.languageId);

                // ログインユーザIDを設定
                ((IDictionary<string, object>)condition).Add("userId", this.userId);

                // 検索実行
                var results = this.db.GetListByOutsideSql(sqlId, sqlDir, condition);
                if (results == null || results.Count == 0)
                {
                    return resultList;
                }
                // 検索結果を結果リストへ登録する
                foreach (var result in results)
                {
                    var item = (IDictionary<string, object>)result;
                    var dic = new Dictionary<string, object>();
                    dic.Add(ColName.GrpId, grpId);
                    foreach (var key in item.Keys)
                    {
                        object value = item[key];
                        if (CommonUtil.IsNullOrEmpty(value))
                        {
                            // 値が空の場合
                            if (key.ToUpper().StartsWith("EXPARAM"))
                            {
                                // 拡張項目の場合は結果リストに格納しない
                                continue;
                            }
                            else
                            {
                                // 拡張項目以外の場合は空文字をセットして返す
                                value = "";
                            }
                        }
                        dic.Add(key, value);
                    }
                    resultList.Add(dic);
                }
                return resultList;
            }

            /// <summary>
            /// シート名の初期値を取得
            /// </summary>
            /// <param name="sheetNo">シート番号</param>
            /// <returns></returns>
            private string getDefaultSheetName(int sheetNo)
            {
                string sheetName = string.Empty;

                switch (sheetNo)
                {
                    case SheetNo.ErrorInfo:
                        sheetName = SheetName.ErrorInfo;
                        break;
                    case SheetNo.DefineInfo:
                        sheetName = SheetName.DefineInfo;
                        break;
                    case SheetNo.ItemInfo:
                        sheetName = SheetName.ItemInfo;
                        break;
                    case SheetNo.TranslationInfo:
                        sheetName = SheetName.TranslationInfo;
                        break;
                    default:
                        sheetName = string.Format("Sheet{0}", sheetNo);
                        break;
                }
                return sheetName;
            }

            /// <summary>
            /// 階層情報の設定
            /// </summary>
            /// <param name="dataList">出力データリスト</param>
            private void setStructureLayerInfo(IList<Dictionary<string, object>> dataList)
            {
                foreach (var data in dataList)
                {
                    // 機能場所階層IDと職種機種階層IDから上位の階層を設定

                    List<StructureLayerInfo.StructureType> typeLst = new List<StructureLayerInfo.StructureType>();
                    typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
                    IList<StructureLocationInfoForReport> locationInfoList = new List<StructureLocationInfoForReport>();
                    StructureLocationInfoForReport locationInfo = new StructureLocationInfoForReport();

                    var rowDic = (IDictionary<string, object>)data;
                    if (rowDic.ContainsKey(ColumnName.LocationStructureId))
                    {
                        var val = rowDic[ColumnName.LocationStructureId];
                        if (!ComUtil.IsNullOrEmpty(val))
                        {
                            locationInfo.LocationStructureId = Convert.ToInt32(val);
                            locationInfoList.Add(locationInfo);
                            StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureLocationInfoForReport>(ref locationInfoList, typeLst, db, languageId);
                            // 関連情報の設定
                            if (locationInfoList != null)
                            {
                                // 地区
                                rowDic[ColumnName.DistrictId] = locationInfoList[0].DistrictId;
                                rowDic[ColumnName.DistrictName] = locationInfoList[0].DistrictName;
                                // 工場
                                rowDic[ColumnName.FactoryId] = locationInfoList[0].FactoryId;
                                rowDic[ColumnName.FactoryName] = locationInfoList[0].FactoryName;
                                // プラント
                                rowDic[ColumnName.PlantId] = locationInfoList[0].PlantId;
                                rowDic[ColumnName.PlantName] = locationInfoList[0].PlantName;
                                // 系列
                                rowDic[ColumnName.SeriesId] = locationInfoList[0].SeriesId;
                                rowDic[ColumnName.SeriesName] = locationInfoList[0].SeriesName;
                                // 工程
                                rowDic[ColumnName.StrokeId] = locationInfoList[0].StrokeId;
                                rowDic[ColumnName.StrokeName] = locationInfoList[0].StrokeName;
                                // 設備
                                rowDic[ColumnName.FacilityId] = locationInfoList[0].FacilityId;
                                rowDic[ColumnName.FacilityName] = locationInfoList[0].FacilityName;
                            }
                        }
                    }

                    // 職種機種情報の設定
                    typeLst = new List<StructureLayerInfo.StructureType>();
                    typeLst.Add(StructureLayerInfo.StructureType.Job);

                    IList<StructureJobInfoForReport> jobInfoList = new List<StructureJobInfoForReport>();
                    StructureJobInfoForReport jobInfo = new StructureJobInfoForReport();

                    if (rowDic.ContainsKey(ColumnName.JobStructureId))
                    {
                        var val = rowDic[ColumnName.JobStructureId];
                        if (!ComUtil.IsNullOrEmpty(val))
                        {
                            jobInfo.JobStructureId = Convert.ToInt32(val);
                            jobInfoList.Add(jobInfo);
                            StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureJobInfoForReport>(ref jobInfoList, typeLst, db, languageId);
                            // 関連情報の設定
                            if (jobInfoList != null)
                            {
                                // 職種
                                rowDic[ColumnName.JobId] = jobInfoList[0].JobId;
                                rowDic[ColumnName.JobName] = jobInfoList[0].JobName;
                                // 機種大分類
                                rowDic[ColumnName.LargeClassficationId] = jobInfoList[0].LargeClassficationId;
                                rowDic[ColumnName.LargeClassficationName] = jobInfoList[0].LargeClassficationName;
                                // 機種中分類
                                rowDic[ColumnName.MiddleClassficationId] = jobInfoList[0].MiddleClassficationId;
                                rowDic[ColumnName.MiddleClassficationName] = jobInfoList[0].MiddleClassficationName;
                                // 機種小分類
                                rowDic[ColumnName.SmallClassficationId] = jobInfoList[0].SmallClassficationId;
                                rowDic[ColumnName.SmallClassficationName] = jobInfoList[0].SmallClassficationName;

                                // 機種名称設定(RP0060)
                                if (!string.IsNullOrEmpty(jobInfoList[0].SmallClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].SmallClassficationName;
                                }
                                else if (!string.IsNullOrEmpty(jobInfoList[0].MiddleClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].MiddleClassficationName;
                                }
                                else if (!string.IsNullOrEmpty(jobInfoList[0].LargeClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].LargeClassficationName;
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// 不要シートの削除＆作業用シートの非表示
            /// </summary>
            /// <param name="sheetNo">シート番号</param>
            /// <param name="factoryId">工場ID</param>
            private void deleteOrHideUnnecessarySheets(int sheetNo, int factoryId)
            {
                // 不要シートの削除＆作業用シートの非表示
                // シート定義情報リストから最大シート番号を取得
                var maxSheetNo = this.sheetDefineList.Max(x => x.SheetNo);
                var moveSheetNo = sheetNo;
                var moveSheet = false;
                for (int i = 1; i <= maxSheetNo; i++)
                {
                    if (i == sheetNo)
                    {
                        // 出力対象シートの場合スキップ
                        continue;
                    }

                    var sheetDefine = this.sheetDefineList.Where(x => x.SheetNo == i).FirstOrDefault();
                    var cmdInfo = new CommonExcelCmdInfo();
                    string sheetName;
                    string[] param;
                    if (sheetDefine == null)
                    {
                        // シート定義情報がない場合
                        // シート削除コマンドを追加
                        cmdInfo = new CommonExcelCmdInfo();
                        sheetName = string.Format("Sheet{0}", i);
                        param = new string[] { sheetName }; // シート名
                        cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdDeleteSheet, param);
                        this.cmdInfoList.Add(cmdInfo);
                        if (i < sheetNo) { moveSheetNo--; }
                        continue;
                    }

                    sheetName = getDefaultSheetName(sheetDefine.SheetNo);
                    var newSheetName = string.Empty;
                    cmdInfo = new CommonExcelCmdInfo();
                    switch (i)
                    {
                        case SheetNo.ErrorInfo:
                            // シート非表示コマンドを追加
                            param = new string[] { sheetName }; // シート名
                            cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdHiddenSheet, param);
                            this.cmdInfoList.Add(cmdInfo);
                            // シート名変更コマンドを追加
                            cmdInfo = new CommonExcelCmdInfo();
                            newSheetName = GetSheetName(sheetDefine, factoryId, this.languageId, db);
                            param = new string[] { sheetName, newSheetName };  // シート名、変更後シート名
                            cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdChangeSheetName, param);
                            this.cmdInfoList.Add(cmdInfo);
                            // 対象シート番号がエラー情報シートより後ろの場合、最後に先頭へ移動する
                            moveSheet = sheetNo > i;
                            break;
                        case SheetNo.DefineInfo:
                        case SheetNo.ItemInfo:
                        case SheetNo.TranslationInfo:
                            // シート非表示コマンドを追加
                            param = new string[] { sheetName };    // シート名
                            cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdHiddenSheet, param);
                            this.cmdInfoList.Add(cmdInfo);
                            break;
                        default:
                            // シート削除コマンドを追加
                            sheetName = string.Format("Sheet{0}", i);
                            param = new string[] { sheetName }; // シート名
                            cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdDeleteSheet, param);
                            this.cmdInfoList.Add(cmdInfo);
                            if (i < sheetNo) { moveSheetNo--; }
                            break;
                    }
                }
                if (moveSheet)
                {
                    // 対象シートを先頭へ移動する
                    string[] param = new string[] { moveSheetNo.ToString() };
                    var cmdInfo = new CommonExcelCmdInfo();
                    cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdMoveSheet, param);
                    this.cmdInfoList.Add(cmdInfo);
                }
            }

            /// <summary>
            /// アップロード条件のチェック
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="resultList"></param>
            /// <param name="errorMsg"></param>
            /// <param name="checkFlg"></param>
            /// <returns></returns>
            private List<ComBase.UploadErrorInfo> checkUploadCondition<T>(string ctrlGrpId, ref List<T> resultList, ref string errorMsg, bool checkFlg = true)
            {
                // エラー内容格納クラス
                List<ComBase.UploadErrorInfo> errorInfo = new List<ComBase.UploadErrorInfo>();

                // ファイル入力項目定義情報を取得
                int sheetNo = this.UploadCondition.SheetNo;
                ComBase.InputDefineCondition param = new ComBase.InputDefineCondition()
                {
                    ReportId = Template.ReportId,
                    SheetNo = sheetNo,
                    ControlGroupId = ctrlGrpId,
                    LanguageId = this.languageId,
                    FactoryId = TMQConsts.CommonFactoryId
                };
                var reportInfoList = TMQUtil.SqlExecuteClass.SelectList<InputDefineForExcelPort>(SqlName.GetInputControlDefineForExcelPort, SqlName.SubDirName, param, db);
                if (reportInfoList == null || reportInfoList.Count == 0)
                {
                    // 取得できない場合、処理を戻す
                    // 「指定されたEXCELのフォーマットが不正です。」
                    errorMsg = GetResMessage(ComRes.ID.ID141120010, this.languageId, this.msgResources);
                    return errorInfo;
                }

                // 検索結果クラスのプロパティを列挙
                var properites = typeof(T).GetProperties();
                // 1レコード分の行数を取得する
                int addRow = reportInfoList[0].RecordCount;
                // 入力方式を取得
                int dataDirection = reportInfoList[0].DataDirection;
                // 開始行番号を取得
                int rowNo = reportInfoList[0].StartRowNo;
                // エラー有無の列番号を取得
                this.ErrorColNo = reportInfoList.Where(x => x.EpColumnDivision == ColumnDivision.Error).Select(x => x.StartColumnNo).FirstOrDefault();

                // 先頭シートの最終行を取得
                long lastRowNo = this.ExcelCmd.GetLastRowNo();
                string rowRange = string.Format("{0}:{1}", rowNo, lastRowNo);
                // 先頭シートのコメントをクリア
                this.ExcelCmd.ClearCellComment(new string[] { rowRange, "1" });
                // 先頭シートのエラー背景色をクリア
                this.ExcelCmd.BackgroundColor(new string[] { rowRange, "NOCOLOR" });
                // 先頭シートのエラー有無列の値のみをクリア
                this.ExcelCmd.Clear(new string[] { string.Format("{0}{1}:{0}{2}", this.ErrorColLetter, rowNo, lastRowNo), "2" });

                // キー項目、送信時処理ID、工場IDの項目定義を取得
                var keyDefines = reportInfoList.Where(x => x.EpColumnDivision == ColumnDivision.Key).ToList();
                var sendProcIdDefine = reportInfoList.Where(x => x.EpColumnDivision == ColumnDivision.SendProcId).FirstOrDefault();
                var sendProcNameDefine = reportInfoList.Where(x => x.EpSelectIdColumnNo == sendProcIdDefine.StartColumnNo).FirstOrDefault();
                var factoryIdDefine = reportInfoList.Where(x => x.EpColumnDivision == ColumnDivision.FactoryId).FirstOrDefault();

                // 選択項目の出力定義を抽出
                var selectItemDefineList = reportInfoList.Where(
                    x => x.ColumnType == ColumnType.ComboBox ||
                    x.ColumnType == ColumnType.MultiListBox ||
                    x.ColumnType == ColumnType.FormSelect).ToList();

                var factoryIdList = new List<int>();
                factoryIdList.AddRange(this.TargetFactoryIdList);
                //システム共通の階層も併せて取得する
                if (!factoryIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
                {
                    factoryIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
                }

                var itemDataDic = new Dictionary<string, List<Dictionary<string, object>>>();
                foreach (var define in selectItemDefineList)
                {
                    string grpId = define.EpSelectGroupId;
                    string relationId = define.EpRelationId;
                    string relationParam = define.EpRelationParameters;
                    // コンボ選択アイテムデータの取得
                    var selectItemList = getComboBoxData(grpId, relationId, relationParam, factoryIdList);
                    if (selectItemList.Count > 0)
                    {
                        itemDataDic.Add(grpId, selectItemList);
                    }
                }
                int index = 0;
                while (true)
                {
                    // エラー内容一時格納クラス
                    List<ComBase.UploadErrorInfo> tmpErrorInfo = new List<ComBase.UploadErrorInfo>();

                    bool flg = false; // データ存在チェックフラグ
                    object tmpResult = Activator.CreateInstance<T>();

                    if(index > 0)
                    {
                        rowNo += addRow;
                    }

                    // 送信時処理IDを取得
                    string sendProcIdStr = getCellValueBySheetNo(sheetNo, sendProcIdDefine.StartColumnNo, rowNo);

                    // キー列値を取得
                    List<string> keys = new List<string>();
                    foreach (var define in keyDefines)
                    {
                        var key = getCellValueBySheetNo(sheetNo, define.StartColumnNo, rowNo);
                        if (!string.IsNullOrEmpty(key))
                        {
                            // キー列値をデータクラスへセット
                            setCellValueToDataClass<T>(define, properites, tmpResult, key);
                            keys.Add(key);
                        }
                    }
                    if (string.IsNullOrEmpty(sendProcIdStr))
                    {
                        // 送信時処理ID列が空の場合
                        if (keys.Count == 0)
                        {
                            // キー列が空の場合はデータ未設定行とみなし、処理終了
                            break;
                        }

                        // 処理対象外のためスキップ
                        index++;
                        continue;
                    }
                    var sendProcId = Convert.ToInt32(sendProcIdStr);
                    if (keys.Count < keyDefines.Count)
                    {
                        // 新規行の場合
                        if(sendProcId != TMQConsts.SendProcessId.Regist)
                        {
                            // 送信時処理IDが登録でない場合、送信時処理列にメッセージをセットする
                            // 「新規追加データに対して内容更新・削除は指定できません。」141120014
                            var msg = GetResMessage(ComRes.ID.ID141120014, languageId, msgResources);
                            tmpErrorInfo.Add(setTmpErrorInfo(rowNo, sendProcNameDefine.StartColumnNo, sendProcNameDefine.TranslationText, msg, dataDirection));
                            setErrorInfo(ref errorInfo, tmpErrorInfo);
                            index++;
                            continue;
                        }
                    }
                    else
                    {
                        // 既存行の場合
                        if(sendProcId != TMQConsts.SendProcessId.Update && sendProcId != TMQConsts.SendProcessId.Delete)
                        {
                            // 「既存データに対して登録は指定できません。」
                            var msg = GetResMessage(ComRes.ID.ID141070006, languageId, msgResources);
                            tmpErrorInfo.Add(setTmpErrorInfo(rowNo, sendProcNameDefine.StartColumnNo, sendProcNameDefine.TranslationText, msg, dataDirection));
                            setErrorInfo(ref errorInfo, tmpErrorInfo);
                            index++;
                            continue;
                        }
                    }
                    // 送信時処理IDをデータクラスへセット
                    setCellValueToDataClass<T>(sendProcIdDefine, properites, tmpResult, sendProcId.ToString());

                    var targetFactoryId = -1;
                    // 取得できた項目定義分処理を行う
                    foreach (InputDefineForExcelPort reportInfo in reportInfoList)
                    {
                        if (reportInfo.EpColumnDivision != null)
                        {
                            // 列区分が設定されている列は別にチェックを行うためスキップ
                            continue;
                        }

                        // 設定値を取得
                        reportInfo.StartRowNo = rowNo;
                        string val = getCellValueBySheetNo(sheetNo, reportInfo.StartColumnNo, reportInfo.StartRowNo);

                        if (string.IsNullOrEmpty(val))
                        {
                            // 値が取得できない場合
                            if (reportInfo.ColumnType != ColumnType.ComboBox &&
                                reportInfo.ColumnType != ColumnType.MultiListBox &&
                                reportInfo.ColumnType != ColumnType.FormSelect)
                            {
                                // 選択項目でない場合、スキップ
                                if (checkFlg)
                                {
                                    if (reportInfo.RequiredFlg != null && (bool)reportInfo.RequiredFlg)
                                    {
                                        // 必須入力項目の場合、エラー内容を設定
                                        // 「必須項目です。入力してください。」
                                        var msg = GetResMessage(ComRes.ID.ID941270001, languageId, msgResources);
                                        tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, msg, dataDirection));
                                    }
                                }
                                continue;
                            }
                        }
                        else
                        {
                            // 入力項目が存在する場合、フラグをたてる
                            flg = true;
                        }

                        if (reportInfo.ColumnType != ColumnType.ComboBox &&
                            reportInfo.ColumnType != ColumnType.MultiListBox &&
                            reportInfo.ColumnType != ColumnType.FormSelect)
                        {
                            if (checkFlg)
                            {
                                // アップロード共通チェック実行
                                if (!ExecuteCommonUploadCheck(reportInfo, val, dataDirection, languageId, msgResources, this.db, tmpErrorInfo))
                                {
                                    continue;
                                }
                            }

                            if (reportInfo.ColumnType == ColumnType.Text)
                            {
                                // 文字列の場合、改行コード(\r,\n)を削除
                                val = val.Replace("\r", "").Replace("\n", "");
                            }
                            else if (reportInfo.ColumnType == ColumnType.TextArea)
                            {
                                // テキストエリアの場合、改行コード(\r)を削除
                                val = val.Replace("\r", "");
                            }
                        }
                        else
                        {
                            // コンボボックス、複数選択リストボックス、画面選択列の場合
                            // 対象行の選択項目IDを取得
                            var selectId = getCellValueBySheetNo(sheetNo, reportInfo.EpSelectIdColumnNo, reportInfo.StartRowNo);
                            var selectIdDefine = reportInfoList.Where(x => x.StartColumnNo == reportInfo.EpSelectIdColumnNo).FirstOrDefault();
                            if (targetFactoryId < 0)
                            {
                                // 対象行の工場IDを取得
                                if (factoryIdDefine != null)
                                {
                                    var tmpId = getCellValueBySheetNo(sheetNo, factoryIdDefine.StartColumnNo, reportInfo.StartRowNo);
                                    if (!string.IsNullOrEmpty(tmpId))
                                    {
                                        targetFactoryId = Convert.ToInt32(tmpId);
                                    }
                                }
                                if (targetFactoryId < 0)
                                {
                                    targetFactoryId = TMQConsts.CommonFactoryId;
                                }
                            }
                            if (itemDataDic.ContainsKey(reportInfo.EpSelectGroupId))
                            {
                                var itemList = itemDataDic[reportInfo.EpSelectGroupId];
                                if (itemList.Count > 0)
                                {
                                    // 選択項目の場合、ここで必須チェックする
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        object transText = null;
                                        if (!string.IsNullOrEmpty(selectId))
                                        {
                                            if (!selectId.Contains("|"))
                                            {
                                                // 選択項目IDと工場IDから翻訳を取得
                                                transText = getItemValue(itemList, targetFactoryId, "id", selectId, "name");
                                            }
                                            else
                                            {
                                                // バー区切りの場合、分割して取得
                                                var selectIds = selectId.Split("|");
                                                foreach(var tmpId in selectIds)
                                                {
                                                    var text = getItemValue(itemList, targetFactoryId, "id", tmpId, "name");
                                                    if (!ComUtil.IsNullOrEmpty(text))
                                                    {
                                                        if (transText != null)
                                                        {
                                                            transText += ",";
                                                        }
                                                        transText += text.ToString();
                                                    }
                                                }
                                            }
                                        }
                                        if (ComUtil.IsNullOrEmpty(transText) || val != transText.ToString())
                                        {
                                            // 対象工場の翻訳でない場合、翻訳からIDを取得
                                            object tmpId = null;
                                            if (!val.Contains(","))
                                            {
                                                tmpId = getItemValue(itemList, targetFactoryId, "name", val, "id");
                                            }
                                            else
                                            {
                                                // カンマ区切りの場合、分割して取得
                                                var vals = val.Split(",");
                                                foreach(var tmpVal in vals)
                                                {
                                                    var id = getItemValue(itemList, targetFactoryId, "name", tmpVal, "id");
                                                    if (!ComUtil.IsNullOrEmpty(id))
                                                    {
                                                        if (tmpId != null)
                                                        {
                                                            tmpId += "|";
                                                        }
                                                        tmpId += id.ToString();
                                                    }
                                                }
                                            }
                                            if (!ComUtil.IsNullOrEmpty(tmpId))
                                            {
                                                // 選択項目IDが取得できた場合、Excelへ値をセット
                                                selectId = tmpId.ToString();
                                                setCellValueBySheetNo(sheetNo, selectIdDefine.StartColumnNo, reportInfo.StartRowNo, selectId);
                                            }
                                            else
                                            {
                                                selectId = string.Empty;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // 選択値が空の場合
                                        if (true.Equals(reportInfo.RequiredFlg))
                                        {
                                            // 必須項目の場合
                                            if (itemList.Count == 1)
                                            {
                                                // 選択候補が1件の場合、その値をセットする
                                                val = itemList[0]["name"].ToString();
                                                selectId = itemList[0]["id"].ToString();
                                                // 入力項目が存在する場合、フラグをたてる
                                                flg = true;
                                            }
                                            else
                                            {
                                                if (checkFlg)
                                                {
                                                    // 選択項目でない場合、エラー内容を設定
                                                    // 「必須項目です。入力してください。」
                                                    var msg = GetResMessage(ComRes.ID.ID941270001, languageId, msgResources);
                                                    tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, msg, dataDirection));
                                                }
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                selectId = string.Empty;
                            }
                            if (!string.IsNullOrEmpty(selectId))
                            {
                                // 選択項目IDをデータクラスへセット
                                setCellValueToDataClass<T>(selectIdDefine, properites, tmpResult, selectId);
                            }
                            else
                            {
                                // 選択項目IDが空の場合
                                if (checkFlg && !string.IsNullOrEmpty(val))
                                {
                                    // 選択項目が空でない場合
                                    // 「選択内容が不正です。」
                                    var msg = GetResMessage(ComRes.ID.ID141140004, languageId, msgResources);
                                    tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, msg, dataDirection));
                                }
                                continue;
                            }
                        }

                        // 値をデータクラスに設定
                        setCellValueToDataClass<T>(reportInfo, properites, tmpResult, val);
                    }

                    // データが1件も取得できなかった場合、処理を抜ける
                    if (!flg)
                    {
                        break;
                    }

                    //エラーがある場合、エラーフラグを立てる
                    var errProp = properites.FirstOrDefault(x => x.Name.ToUpper().Equals("ERRORFLG"));
                    if (errProp != null && tmpErrorInfo.Count > 0)
                    {
                        ComUtil.SetPropertyValue<T>(errProp, (T)tmpResult, true);
                    }

                    // データが存在する場合、リストに追加する
                    setErrorInfo(ref errorInfo, tmpErrorInfo);
                    resultList.Add((T)tmpResult);
                    index++;
                }
                return errorInfo;
            }

            /// <summary>
            /// 選択項目値の取得
            /// </summary>
            /// <param name="itemList">選択項目リスト</param>
            /// <param name="targetFactoryId">対象工場ID</param>
            /// <param name="keyName">キー項目名</param>
            /// <param name="keyVal">キー項目値</param>
            /// <param name="valName">取得対象項目名</param>
            /// <returns></returns>
            private object getItemValue(List<Dictionary<string, object>> itemList, int targetFactoryId, string keyName, string keyVal, string valName)
            {
                return itemList.Where(x =>
                    x[keyName].ToString() == keyVal &&
                    (x["factory_id"].ToString() == targetFactoryId.ToString() ||
                    x["factory_id"].ToString() == TMQConsts.CommonFactoryId.ToString()))
                    .Select(x => x[valName]).FirstOrDefault();
            }

            /// <summary>
            /// Excelセルの値を取得(シート名指定)
            /// </summary>
            /// <param name="sheetName">シート名</param>
            /// <param name="colNo">列番号</param>
            /// <param name="rowNo">行番号</param>
            /// <returns></returns>
            private string getCellValue(string sheetName, int colNo, int rowNo)
            {
                string error = string.Empty;
                string msg = string.Empty;
                string[,] vals = null;

                // マッピングセルを設定
                string address = ToAlphabet(colNo) + rowNo;
                // セル単位でデータを取得する
                if (!this.ExcelCmd.ReadExcel(sheetName, address, ref vals, ref msg))
                {
                    // 読込失敗した場合、nullを返す
                    return null;
                }
                return vals[0, 0]; // セル単位で取得しているので先頭を対象データとみなす。
            }

            /// <summary>
            /// Excelセルの値を取得(シート番号指定)
            /// </summary>
            /// <param name="sheetNo"></param>
            /// <param name="colNo"></param>
            /// <param name="rowNo"></param>
            /// <returns></returns>
            private string getCellValueBySheetNo(int sheetNo, int colNo, int rowNo)
            {
                string error = string.Empty;
                string msg = string.Empty;
                string[,] vals = null;

                // マッピングセルを設定
                string address = ToAlphabet(colNo) + rowNo;
                // セル単位でデータを取得する
                if (!this.ExcelCmd.ReadExcelBySheetNo(sheetNo, address, ref vals, ref msg))
                {
                    // 読込失敗した場合、nullを返す
                    return null;
                }
                return vals[0, 0]; // セル単位で取得しているので先頭を対象データとみなす。
            }

            /// <summary>
            /// Excelセルの値を設定(シート名指定)
            /// </summary>
            /// <param name="sheetName">シート名</param>
            /// <param name="colNo">列番号</param>
            /// <param name="rowNo">行番号</param>
            /// <param name="val">設定値</param>
            /// <returns></returns>
            private void setCellValue(string sheetName, int colNo, int rowNo, string val)
            {
                // マッピングセルを設定
                string address = ToAlphabet(colNo) + rowNo;
                // セルに値を設定する
                this.ExcelCmd.SetCellValue(new string[] { address, val, sheetName });
            }

            /// <summary>
            /// Excelセルの値を設定(シート番号指定)
            /// </summary>
            /// <param name="sheetNo">シート番号</param>
            /// <param name="colNo">列番号</param>
            /// <param name="rowNo">行番号</param>
            /// <param name="val">設定値</param>
            /// <returns></returns>
            private void setCellValueBySheetNo(int sheetNo, int colNo, int rowNo, string val)
            {
                // マッピングセルを設定
                string address = ToAlphabet(colNo) + rowNo;
                // セルに値を設定する
                this.ExcelCmd.SetCellValue(new string[] { address, val, sheetNo.ToString() });
            }

            /// <summary>
            /// Excelセルの値をデータクラス変数に設定
            /// </summary>
            /// <typeparam name="T">対象クラス型</typeparam>
            /// <param name="reportInfo">定義情報</param>
            /// <param name="properites">対象クラスのプロパティ情報</param>
            /// <param name="target">設定対象クラス変数</param>
            /// <param name="val">設定値</param>
            /// <returns></returns>
            private bool setCellValueToDataClass<T>(InputDefineForExcelPort reportInfo, PropertyInfo[] properites, object target, string val)
            {
                // 値をデータクラスに設定
                string pascalItemName = ComUtil.SnakeCaseToPascalCase(reportInfo.AliasName != null ? reportInfo.AliasName : reportInfo.ColumnName).ToUpper();
                var prop = properites.FirstOrDefault(x => x.Name.ToUpper().Equals(pascalItemName));
                if (prop == null)
                {
                    // 該当する項目が存在しない場合、スキップ
                    return false;
                }
                ComUtil.SetPropertyValue<T>(prop, (T)target, val);
                return true;
            }

            /// <summary>
            /// エラー情報シートへの設定
            /// </summary>
            /// <param name="file">ファイルデータ</param>
            /// <param name="startDataIdx">エラー情報開始インデックス</param>
            /// <param name="fileType">ファイル種類</param>
            /// <param name="fileName">ファイル名</param>
            /// <param name="ms">メモリストリーム</param>
            /// <returns></returns>
            private bool setErrorInfoSheet(IFormFile file, int startDataIdx, ref string fileType, ref string fileName, ref MemoryStream ms)
            {
                string reportId = Template.ReportId;

                // ファイルタイプ
                fileType = ComConsts.REPORT.FILETYPE.EXCEL_MACRO;
                // ダウンロードファイル名
                fileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", reportId, DateTime.Now) + ComConsts.REPORT.EXTENSION.EXCEL_MACRO_BOOK;

                // 先頭シート、エラー情報シートのシート名を取得
                var sheetName = this.ExcelCmd.GetSheetName("1");
                var sheetNameErr = this.ExcelCmd.GetSheetName(SheetNo.ErrorInfoDownloaded.ToString());

                // エラー有無文字列「あり」
                string errText = ComUtil.GetPropertiesMessage(ComRes.ID.ID111010021, this.languageId, this.msgResources);

                // エラー情報シートへエラー情報を設定

                var mappingList = new List<CommonExcelUtil.MappingInfo>();
                int rowNo = 1;

                for (int idx = startDataIdx; idx < this.ErrorInfoList.Count; idx++)
                {
                    var info = this.ErrorInfoList[idx];
                    int colNo = 0;
                    for (int i = 0; i < info.ColumnNo.Count; i++)
                    {
                        rowNo++;

                        // シート名
                        mappingList.Add(new CommonExcelUtil.MappingInfo()
                        {
                            X = colNo++,
                            Y = rowNo,
                            Value = sheetName
                        });
                        // 行
                        mappingList.Add(new CommonExcelUtil.MappingInfo()
                        {
                            X = colNo++,
                            Y = rowNo,
                            Value = info.ColumnNo[i]
                        });
                        // 列
                        mappingList.Add(new CommonExcelUtil.MappingInfo()
                        {
                            X = colNo++,
                            Y = rowNo,
                            Value = info.RowNo[i]
                        });
                        // 処理区分名
                        mappingList.Add(new CommonExcelUtil.MappingInfo()
                        {
                            X = colNo++,
                            Y = rowNo,
                            Value = info.ProcDivName
                        });
                        // エラー情報
                        mappingList.Add(new CommonExcelUtil.MappingInfo()
                        {
                            X = colNo++,
                            Y = rowNo,
                            Value = info.ErrorInfo
                        });

                        // 機能別シート上セル位置
                        string rangeTo = ToAlphabet(info.ColumnNo[i]) + info.RowNo[i];
                        // コメント(メモ)を設定
                        this.ExcelCmd.SetCellComment(new string[] {
                            rangeTo, info.ErrorInfo });
                        // 背景色を設定
                        this.ExcelCmd.BackgroundColor(new string[] {
                            rangeTo, ColorValues.Error });
                        // エラー有無列に「あり」設定
                        string rangeErr = this.ErrorColLetter + info.RowNo[i];
                        this.ExcelCmd.SetCellValue(new string[] {
                            rangeErr, errText });

                        // エラー情報シート上セル位置
                        rangeErr = ToAlphabet(colNo) + (rowNo + 1);
                        // ハイパーリンクを設定
                        this.ExcelCmd.SetHyperLink(new string[] {
                            rangeErr, rangeTo, sheetNameErr, sheetName });
                        // 罫線を設定
                        this.ExcelCmd.LineBox(new string[] {
                            string.Format("A{0}:{1}", rowNo + 1, rangeErr), "IO", "", sheetNameErr});
                    }
                }
                // エラー情報シートへエラー情報を設定
                this.ExcelCmd.SetValue(mappingList, "", SheetNo.ErrorInfoDownloaded);

                string[] errSheetParam = new string[] { SheetNo.ErrorInfoDownloaded.ToString() };
                // エラー情報シートを表示
                this.ExcelCmd.ShowSheet(errSheetParam);
                // エラー情報シートをアクティブ化
                this.ExcelCmd.ActivateSheet(errSheetParam);

                // メモリストリームへ保存
                if (ms == null)
                {
                    ms = new MemoryStream();
                }
                this.ExcelCmd.Save(ref ms);

                return true;
            }
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// ExcelPort用ファイル入出力管理クラス
    /// </summary>
    public class InoutDefineForExcelPort : Dao.InoutDefine
    {
        /// <summary>必須項目区分(ExcelPort用)</summary>
        public bool EpRequiredFlg { get; set; }

        /// <summary>関連情報ID(ExcelPort選択項目生成用)</summary>
        public string EpRelationId { get; set; }

        /// <summary>関連情報パラメータ(ExcelPort選択項目生成用)</summary>
        public string EpRelationParameters { get; set; }

        /// <summary>選択項目グループID(ExcelPort用)</summary>
        public string EpSelectGroupId { get; set; }

        /// <summary>選択項目ID格納先列番号(ExcelPort用)</summary>
        public int EpSelectIdColumnNo { get; set; }

        /// <summary>選択項目連動元列番号(ExcelPort用)</summary>
        public int EpSelectLinkColumnNo { get; set; }
    }

    /// <summary>
    /// ExcelPort用ファイル入力管理クラス
    /// </summary>
    public class InputDefineForExcelPort : ComBase.InputDefine
    {
        /// <summary>列種類(ExcelPort用)</summary>
        public int ColumnType { get; set; }
        /// <summary>関連情報ID(ExcelPort選択項目生成用)</summary>
        public string EpRelationId { get; set; }

        /// <summary>関連情報パラメータ(ExcelPort選択項目生成用)</summary>
        public string EpRelationParameters { get; set; }

        /// <summary>選択項目グループID(ExcelPort用)</summary>
        public string EpSelectGroupId { get; set; }

        /// <summary>選択項目ID格納先列番号(ExcelPort用)</summary>
        public int EpSelectIdColumnNo { get; set; }

        /// <summary>選択項目連動元列番号(ExcelPort用)</summary>
        public int EpSelectLinkColumnNo { get; set; }
        /// <summary>自動表示拡張列番号(ExcelPort用)</summary>
        public int EpAutoExtentionColumnNo { get; set; }
        /// <summary>列区分(ExcelPort用)</summary>
        public int? EpColumnDivision { get; set; }
    }
}
