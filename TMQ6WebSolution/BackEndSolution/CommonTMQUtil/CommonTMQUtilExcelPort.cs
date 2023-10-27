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
    /// TMQ用ExcelPort共通クラス
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
            public DateTime? OccurrenceDateFrom { get; set; }
            /// <summary>発生日(To)</summary>
            public DateTime? OccurrenceDateTo { get; set; }
            /// <summary>着工予定日(From)</summary>
            public DateTime? ExpectedConstructionDateFrom { get; set; }
            /// <summary>着工予定日(To)</summary>
            public DateTime? ExpectedConstructionDateTo { get; set; }
            /// <summary>完了日(From)</summary>
            public DateTime? CompletionDateFrom { get; set; }
            /// <summary>完了日(To)</summary>
            public DateTime? CompletionDateTo { get; set; }
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
            /// <summary>出力日時</summary>
            public DateTime OutputDate { get; set; }
            /// <summary>スケジュール開始年月</summary>
            public DateTime ScheduleDateFrom { get; set; }
            /// <summary>スケジュール終了年月</summary>
            public DateTime ScheduleDateTo { get; set; }
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

                // 機能個別シート

                /// <summary>長期計画</summary>
                public const int LongPlan = 3;
                /// <summary>長期計画 機器別管理基準</summary>
                public const int ManagementStandards = 4;
                /// <summary>保全活動</summary>
                public const int Maintenance = 5;
                /// <summary>保全活動_故障情報</summary>
                public const int HistoryFailure = 6;
                /// <summary>保全活動_点検情報（対象機器）</summary>
                public const int InspectionMachine = 7;
                /// <summary>シート番号：マスタメンテナンス「標準アイテム未使用」</summary>
                public const int UnuseSheetNo = 13;
                /// <summary>シート番号：マスタメンテナンス「並び順」</summary>
                public const int OrdeerSheetNo = 14;
                /// <summary>シート番号：マスタメンテナンス「場所階層」</summary>
                public const int SheetNoOfStructureGroup1000 = 20;
                /// <summary>シート番号：マスタメンテナンス「職種機種」</summary>
                public const int SheetNoOfStructureGroup1010 = 63;
                /// <summary>シート番号：マスタメンテナンス「予備品ロケーション」</summary>
                public const int SheetNoOfStructureGroup1040 = 62;
                /// <summary>シート番号：マスタメンテナンス「部門」</summary>
                public const int SheetNoOfStructureGroup1760 = 56;
                /// <summary>シート番号：マスタメンテナンス「仕様項目」</summary>
                public const int SpecSheetNo = 15;
                /// <summary>シート番号：マスタメンテナンス「仕様項目選択肢」</summary>
                public const int SpecItemSheetNo = 16;
                /// <summary>シート番号：マスタメンテナンス「機種別仕様関連付け」</summary>
                public const int SpecRelarionSheetNo = 17;
                /// <summary>シート番号：マスタメンテナンス「処置対策」</summary>
                public const int TreatmentMeasureSheetNo = 48;
                /// <summary>シート番号：マスタメンテナンス「予算性格区分」</summary>
                public const int BudgetPersonalitySheetNo = 61;
                /// <summary>シート番号：マスタメンテナンス「緊急度」</summary>
                public const int UrgencySheetNo = 28;
                /// <summary>シート番号：マスタメンテナンス「発見方法」</summary>
                public const int DiscoveryMethodsSheetNo = 54;
                /// <summary>シート番号：マスタメンテナンス「変更管理」</summary>
                public const int ChangeManagementSheetNo = 64;
                /// <summary>シート番号：マスタメンテナンス「環境安全管理」</summary>
                public const int EnvSafetyManagementSheetNo = 26;
                /// <summary>シート番号：マスタメンテナンス「系停止」</summary>
                public const int StopSystemSheetNo = 29;
                /// <summary>シート番号：マスタメンテナンス「実績結果」</summary>
                public const int ActualResultSheetNo = 44;
                /// <summary>シート番号：マスタメンテナンス「メーカー」</summary>
                public const int ManufacturerSheetNo = 22;
                /// <summary>シート番号：マスタメンテナンス「適用法規」</summary>
                public const int ApplicableLawsSheetNo = 53;
                /// <summary>シート番号：マスタメンテナンス「機器レベル」</summary>
                public const int MachineLevelSheetNo = 18;
                /// <summary>シート番号：マスタメンテナンス「部位マスタ」</summary>
                public const int SiteMasterSheetNo = 55;
                /// <summary>シート番号：マスタメンテナンス「重要度」</summary>
                public const int ImportanceSheetNo = 46;
                /// <summary>シート番号：マスタメンテナンス「使用区分」</summary>
                public const int UseSegmentSheetNo = 39;
                /// <summary>シート番号：マスタメンテナンス「保全項目(点検内容)」</summary>
                public const int InspectionDetailsSheetNo = 58;
                /// <summary>シート番号：マスタメンテナンス「保全区分」</summary>
                public const int MaintainanceDivisionSeetNo = 57;
                /// <summary>シート番号：マスタメンテナンス「作業項目」</summary>
                public const int WorkItemSeetNo = 42;
                /// <summary>シート番号：マスタメンテナンス「作業目的」</summary>
                public const int PurposeSeetNo = 37;
                /// <summary>シート番号：マスタメンテナンス「予算管理区分」</summary>
                public const int BudgetManagementSeetNo = 60;
                /// <summary>シート番号：マスタメンテナンス「処置区分」</summary>
                public const int TreatmentSeetNo = 47;
                /// <summary>シート番号：マスタメンテナンス「設備区分」</summary>
                public const int FacilitySeetNo = 50;
                /// <summary>シート番号：マスタメンテナンス「時期」</summary>
                public const int SeasonSeetNo = 41;
                /// <summary>シート番号：マスタメンテナンス「修繕費分類」</summary>
                public const int RepairCostClassSeetNo = 45;
                /// <summary>シート番号：マスタメンテナンス「作業区分」</summary>
                public const int WorkClassSeetNo = 36;
                /// <summary>シート番号：マスタメンテナンス「依頼部課係」</summary>
                public const int RequestDepartmentClerkSeetNo = 24;
                /// <summary>シート番号：マスタメンテナンス「工事区分」</summary>
                public const int ConstructionDivisionSeetNo = 33;
                /// <summary>シート番号：マスタメンテナンス「自・他責」</summary>
                public const int ResponsibilitySeetNo = 43;
                /// <summary>シート番号：マスタメンテナンス「施工会社」</summary>
                public const int CompanySeetNo = 40;
                /// <summary>シート番号：マスタメンテナンス「故障性格分類」</summary>
                public const int FailurePersonalityClassSeetNo = 30;
                /// <summary>シート番号：マスタメンテナンス「故障性格要因」</summary>
                public const int FailurePersonalityFactorSeetNo = 31;
                /// <summary>シート番号：マスタメンテナンス「依頼番号採番パターン」</summary>
                public const int RequestNumberingPatternSeetNo = 23;
                /// <summary>シート番号：マスタメンテナンス「作業/故障区分」</summary>
                public const int WorkFailureDivisionSeetNo = 35;
                /// <summary>シート番号：マスタメンテナンス「故障分析」</summary>
                public const int FailureAnalysisSeetNo = 32;
                /// <summary>シート番号：マスタメンテナンス「仕入先」</summary>
                public const int VenderSeetNo = 38;
                /// <summary>シート番号：マスタメンテナンス「数量管理単位」</summary>
                public const int UnitSeetNo = 19;
                /// <summary>シート番号：マスタメンテナンス「金額管理単位」</summary>
                public const int CurrencySeetNo = 21;
                /// <summary>シート番号：マスタメンテナンス「部門(工場・部門)」</summary>
                public const int DepartmentSeetNo = 56;
                /// <summary>シート番号：マスタメンテナンス「勘定科目」</summary>
                public const int AccountSeetNo = 25;
                /// <summary>シート番号：マスタメンテナンス「対策分類１」</summary>
                public const int MeasureClass1SeetNo = 51;
                /// <summary>シート番号：マスタメンテナンス「対策分類２」</summary>
                public const int MeasureClass2SeetNo = 52;
                /// <summary>シート番号：マスタメンテナンス「保全部課係」</summary>
                public const int MaintenanceDepartmentClerkSeetNo = 59;
                /// <summary>シート番号：マスタメンテナンス「新旧区分」</summary>
                public const int OldNewDivitionSeetNo = 49;
                /// <summary>シート番号：マスタメンテナンス「工場毎年度期首月」</summary>
                public const int BeginningMonthSeetNo = 34;
                /// <summary>シート番号：マスタメンテナンス「丸め処理区分」</summary>
                public const int RoundDivisionSeetNo = 27;

                /// <summary>
                /// マスタのアップロード時に「ExcelPort利用可能工場以外のデータは登録できません。」が表示されないようにするためのリスト
                /// </summary>
                public static List<int> NormalMasterSheetNoList = new()
                {
                    TreatmentMeasureSheetNo,          // 処置対策
                    BudgetPersonalitySheetNo,         // 予算性格区分
                    UrgencySheetNo,                   // 緊急度
                    DiscoveryMethodsSheetNo,          // 発見方法
                    ChangeManagementSheetNo,          // 変更管理
                    EnvSafetyManagementSheetNo,       // 環境安全管理
                    StopSystemSheetNo,                // 系停止
                    ActualResultSheetNo,              // 実績結果
                    ManufacturerSheetNo,              // メーカー
                    ApplicableLawsSheetNo,            // 適用法規
                    MachineLevelSheetNo,              // 機器レベル
                    SiteMasterSheetNo,                // 部位マスタ
                    ImportanceSheetNo,                // 重要度
                    UseSegmentSheetNo,                // 使用区分
                    InspectionDetailsSheetNo,         // 保全項目(点検内容)
                    MaintainanceDivisionSeetNo,       // 保全区分
                    WorkItemSeetNo,                   // 作業項目
                    PurposeSeetNo,                    // 作業目的
                    BudgetManagementSeetNo,           // 予算管理区分
                    TreatmentSeetNo,                  // 処置区分
                    FacilitySeetNo,                   // 設備区分
                    SeasonSeetNo,                     // 時期
                    RepairCostClassSeetNo,            // 修繕費分類
                    WorkClassSeetNo,                  // 作業区分
                    RequestDepartmentClerkSeetNo,     // 依頼部課係
                    ConstructionDivisionSeetNo,       // 工事区分
                    ResponsibilitySeetNo,             // 自・他責
                    CompanySeetNo,                    // 施工会社
                    FailurePersonalityClassSeetNo,    // 故障性格分類
                    FailurePersonalityFactorSeetNo,   // 故障性格要因
                    RequestNumberingPatternSeetNo,    // 依頼番号採番パターン
                    WorkFailureDivisionSeetNo,        // 作業/故障区分
                    FailureAnalysisSeetNo,            // 故障分析
                    VenderSeetNo,                     // 仕入先
                    UnitSeetNo,                       // 数量管理単位
                    CurrencySeetNo,                   // 金額管理単位
                    DepartmentSeetNo,                 // 部門(工場・部門)
                    AccountSeetNo,                    // 勘定科目
                    MeasureClass1SeetNo,              // 対策分類１
                    MeasureClass2SeetNo,              // 対策分類２
                    MaintenanceDepartmentClerkSeetNo, // 保全部課係
                    OldNewDivitionSeetNo,             // 新旧区分
                    BeginningMonthSeetNo,             // 工場毎年度期首月
                    RoundDivisionSeetNo               // 丸め処理区分
                };
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
                /// <summary>カラム名：シート番号</summary>
                public const string SheetNo = "sheet_no";
                /// <summary>カラム名：列番号</summary>
                public const string ColumnNo = "column_no";
                /// <summary>カラム名：シート番号_列番号</summary>
                public const string SheetNoAndColumnNo = "sheet_no_and_column_no";
                /// <summary>カラム名：項目番号</summary>
                public const string ItemId = "item_id";
                /// <summary>カラム名：列種類</summary>
                public const string ColumnType = "column_type";
                /// <summary>カラム名：カラム名</summary>
                public const string ColumnName = "column_name";
                /// <summary>カラム名：列表示名</summary>
                public const string ItemName = "item_name";
                /// <summary>カラム名：選択列グループID</summary>
                public const string GrpId = "ep_select_group_id";
                /// <summary>カラム名：選択列関連情報ID</summary>
                public const string RelationId = "ep_relation_id";
                /// <summary>カラム名：選択列関連情報パラメータ</summary>
                public const string RelationParam = "ep_relation_parameters";
                /// <summary>カラム名：選択ID値格納先列番号</summary>
                public const string SelectIdColumnNo = "ep_select_id_column_no";
                /// <summary>カラム名：エラー有無</summary>
                public const string ErrorExist = "error_exist";
                /// <summary>カラム名：工場ID</summary>
                public const string FactoryId = "factory_id";

                /// <summary>スケジュール</summary>
                public const string Schedule = "schedule";
                /// <summary>スケジュールID</summary>
                public const string ScheduleId = "schedule_id";

                /// <summary>ID</summary>
                public const string Id = "id";
                /// <summary>表示名</summary>
                public const string Name = "name";

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
                public const string TargetSheetNo = "TargetSheetNo";
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
                /// <summary>行番号：出力日時</summary>
                public const int RowNoOutputDate = 2;
                /// <summary>行番号：対象機能ID</summary>
                public const int RowNoConductId = 3;
                /// <summary>行番号：対象シート番号</summary>
                public const int RowNoSheetNo = 4;

            }

            /// <summary>
            /// エラー情報シート情報
            /// </summary>
            public static class ErrorSheetInfo
            {
                /// <summary>列番号：シート名</summary>
                public const int ColNoSheetName = 1;
                /// <summary>列番号：行</summary>
                public const int ColNoCol = 2;
                /// <summary>列番号：列</summary>
                public const int ColNoRow = 3;
                /// <summary>列番号：処理区分</summary>
                public const int ColNoSendProcName = 4;
                /// <summary>列番号：エラー情報</summary>
                public const int ColNoErrorInfo = 5;
                /// <summary>行番号：データ開始行</summary>
                public const int RowNoStart = 3;

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
                /// <summary>ハイパーリンク</summary>
                public const string HyperLink = "#0563C1";
            }

            /// <summary>
            /// フォーマット文字列
            /// </summary>
            public static class Format
            {
                /// <summary></summary>
                public const string FileName = "{0}_{1:yyyyMMddHHmmssfff}(Ver.{2:#.00}){3}";

                /// <summary></summary>
                public const string ScheduleDate = "yyyy/MM";

                /// <summary></summary>
                public const string SheetNo = "Sheet{0}";
            }

            /// <summary>
            /// スケジュール生成月数
            /// </summary>
            public const int ScheduleMonths = 120;  // 12ヶ月×10年＝120ヶ月

            /// <summary>
            /// マスタメンテナンスの列情報(アップロード時に使用)
            /// </summary>
            public class MasterColumnInfo
            {
                /// <summary>
                /// マスタメンテナンス 「標準アイテム未使用」シートの工場の列番号
                /// </summary>
                public const int UnuseFactoryColNo = 8;

                /// <summary>
                /// 構成グループ1000(場所階層)
                /// </summary>
                public class StructureGroup1000
                {
                    /// <summary>
                    /// 地区
                    /// </summary>
                    public class District
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_1";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 1;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 7;
                    }

                    /// <summary>
                    /// 工場
                    /// </summary>
                    public class Factory
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_2";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 9;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 15;
                    }

                    /// <summary>
                    /// プラント
                    /// </summary>
                    public class Plant
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_3";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 17;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 27;
                    }

                    /// <summary>
                    /// 系列
                    /// </summary>
                    public class Series
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_4";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 29;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 39;
                    }

                    /// <summary>
                    /// 工程
                    /// </summary>
                    public class Stroke
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_5";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 41;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 51;
                    }

                    /// <summary>
                    /// 設備
                    /// </summary>
                    public class Facility
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_6";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 53;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 63;
                    }
                }

                /// <summary>
                /// 構成グループ1010(職種機種)
                /// </summary>
                public class StructureGroup1010
                {
                    /// <summary>
                    /// 地区
                    /// </summary>
                    public class District
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_1";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 1;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 7;
                    }

                    /// <summary>
                    /// 工場
                    /// </summary>
                    public class Factory
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_2";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 9;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 15;
                    }

                    /// <summary>
                    /// 職種
                    /// </summary>
                    public class Job
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_3";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 17;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 29;
                    }

                    /// <summary>
                    /// 機種大分類
                    /// </summary>
                    public class Large
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_4";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 31;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 41;
                    }

                    /// <summary>
                    /// 機種中分類
                    /// </summary>
                    public class Middle
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_5";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 43;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 53;
                    }

                    /// <summary>
                    /// 機種中分類
                    /// </summary>
                    public class Small
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_6";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 55;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 65;
                    }
                }

                /// <summary>
                /// 構成グループ1040(予備品ロケーション)
                /// </summary>
                public class StructureGroup1040
                {
                    /// <summary>
                    /// 地区
                    /// </summary>
                    public class District
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_1";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 1;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 7;
                    }

                    /// <summary>
                    /// 工場
                    /// </summary>
                    public class Factory
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_2";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 9;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 15;
                    }

                    /// <summary>
                    /// 倉庫
                    /// </summary>
                    public class Warehouse
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_3";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 17;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 27;
                    }

                    /// <summary>
                    /// 棚
                    /// </summary>
                    public class Rack
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_4";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 29;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 39;
                    }
                }

                /// <summary>
                /// 構成グループ1760(部門)
                /// </summary>
                public class StructureGroup1760
                {
                    /// <summary>
                    /// 地区
                    /// </summary>
                    public class District
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_1";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 1;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 7;
                    }

                    /// <summary>
                    /// 工場
                    /// </summary>
                    public class Factory
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_2";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 9;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 15;
                    }

                    /// <summary>
                    /// 部門
                    /// </summary>
                    public class Department
                    {
                        /// <summary>
                        /// コントロールグループID
                        /// </summary>
                        public const string ControlGroupId = "LIST_000_3";
                        /// <summary>
                        /// 開始列
                        /// </summary>
                        public const int StartCol = 17;
                        /// <summary>
                        /// 終了列
                        /// </summary>
                        public const int EndCol = 30;
                    }
                }
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
                this.ErrorColNoDic = new Dictionary<string, int>();
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
            /// <summary>送信時処理用翻訳リスト</summary>
            private List<Dictionary<string, object>> procDivItemList;
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
            /// <summary>エラー有無列番号辞書(キーはコントロールグループID)</summary>
            public Dictionary<string, int> ErrorColNoDic { get; set; }
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
            public bool InitializeExcelPortTemplateFile(out string resultMsg, out string detailMsg, bool isUpload = false, Dictionary<string, object> dic = null)
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
                if (isUpload)
                {
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
                this.ApprovalFactoryIdList = history.GetHistoryManagementFactoryIdList();
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
                out string detailMsg, ref string resultMsg)
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

                // ダウンロードファイル名
                fileName = string.Format(Format.FileName, reportId, DateTime.Now, option.Version, ComConsts.REPORT.EXTENSION.EXCEL_MACRO_BOOK);

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
                List<CommonExcelPrtInfo> mappingDataList = CreateMappingListForExcelPort(
                                                            factoryId,
                                                            ProgramId.Download,
                                                            reportId,
                                                            templateId,
                                                            patternId,
                                                            sheetDefine,
                                                            ((IList<object>)tmpDataList).Select(x => (IDictionary<string, object>)x).ToList(),
                                                            templateInfo.TemplateFileName,
                                                            templateInfo.TemplateFilePath,
                                                            languageId,
                                                            out int optionRowCount,
                                                            out int optionColomnCount,
                                                            out detailMsg,
                                                            ref resultMsg,
                                                            option);
                if (mappingDataList == null)
                {
                    // 取得できない場合、処理を戻す
                    // Excel最大行数を超えた場合は出力しない
                    return false;
                }

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
                //var itemDataList = new List<object>();
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
                        if (itemDataList.Where(x => grpId.Equals(x[ColName.GrpId].ToString())).Count() > 0)
                        {
                            // 取得済みのものはスキップ
                            continue;
                        }

                        // コンボ選択アイテムデータの取得
                        var resultList = getComboBoxData(grpId, relationId, relationParam, factoryIdList);
                        if (resultList.Count > 0)
                        {
                            itemDataList.AddRange(resultList);
                        }
                    }
                    // マッピングデータ作成
                    mappingDataList = CreateMappingListForExcelPort(
                                        factoryId,
                                        ProgramId.Download,
                                        reportId,
                                        templateId,
                                        patternId,
                                        sheetDefine,
                                        itemDataList.ToList<IDictionary<string, object>>(),
                                        templateInfo.TemplateFileName,
                                        templateInfo.TemplateFilePath,
                                        languageId,
                                        out optionRowCount,
                                        out optionColomnCount,
                                        out detailMsg,
                                        ref resultMsg);
                    if (mappingDataList == null)
                    {
                        // 取得できない場合、処理を戻す
                        // Excel最大行数を超えた場合は出力しない
                        return false;
                    }
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
                // マッピングデータ作成
                mappingDataList = CreateMappingListForExcelPort(
                                    factoryId,
                                    ProgramId.Download,
                                    reportId,
                                    templateId,
                                    patternId,
                                    sheetDefine,
                                    ((IList<object>)tmpDataList).Select(x => (IDictionary<string, object>)x).ToList(),
                                    templateInfo.TemplateFileName,
                                    templateInfo.TemplateFilePath,
                                    languageId,
                                    out optionRowCount,
                                    out optionColomnCount,
                                    out detailMsg,
                                    ref resultMsg);
                if (mappingDataList == null)
                {
                    // 取得できない場合、処理を戻す
                    // Excel最大行数を超えた場合は出力しない
                    return false;
                }
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
                mappingDataList = CreateMappingListForExcelPort(
                                    factoryId,
                                    ProgramId.Download,
                                    reportId,
                                    templateId,
                                    patternId,
                                    sheetDefine,
                                    dataList.ToList<IDictionary<string, object>>(),
                                    templateInfo.TemplateFileName,
                                    templateInfo.TemplateFilePath,
                                    languageId,
                                    out optionRowCount,
                                    out optionColomnCount,
                                    out detailMsg,
                                    ref resultMsg);
                if (mappingDataList == null)
                {
                    // 取得できない場合、処理を戻す
                    // Excel最大行数を超えた場合は出力しない
                    return false;
                }
                // マッピング情報リストに追加
                this.mappingInfoList.AddRange(mappingDataList);

                var sheetName = getDefaultSheetName(sheetNo);
                // 罫線が必要な場合、以下のコメントを外す
                //// 一覧フラグの場合
                //if (sheetDefine.ListFlg == true)
                //{
                //    // 一覧罫線用にデータ件数を退避
                //    sheetDataCount = dataList.Count;
                //    // 罫線設定セル範囲を取得
                //    string range = GetTargetCellRange(factoryId, reportId, templateId, patternId, sheetDefine.SheetNo, sheetDataCount, sheetDefine.RecordCount, db, optionRowCount, optionColomnCount);
                //    if (range != null)
                //    {
                //        //範囲が取得できた場合、罫線を引く
                //        cmdInfoList.AddRange(CommandLineBox(range, sheetName));
                //    }
                //}

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
                try
                {
                    this.ExcelCmd = TMQUtil.FileOpen(file.OpenReadStream());
                }
                catch
                {
                    // ※画像や図形が貼り付けられたファイルの場合、ClosedXMLでの読み込みで例外が発生することがある
                    // ※ExcelPortでは上記のようなファイルは想定していないためフォーマット不正とする

                    // 「指定されたEXCELのフォーマットが不正です。」
                    msg = GetResMessage(ComRes.ID.ID141120010, this.languageId, this.msgResources);
                    return false;
                }

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
                if (Convert.ToDecimal(versionNo) != versionNo2)
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
                    // シート番号を判定
                    if (sheetNo == SheetNo.OrdeerSheetNo)
                    {
                        // シート番号が「マスタ並び順設定」の場合
                        targetName = ComRes.ID.ID141210001;
                    }
                    else if (sheetNo == SheetNo.UnuseSheetNo)
                    {
                        // シート番号が「マスタ標準アイテム未使用設定」の場合
                        targetName = ComRes.ID.ID141270005;
                    }
                    else
                    {
                        // 「指定されたEXCELから更新対象機能が特定できません。」
                        msg = GetResMessage(ComRes.ID.ID141120012, this.languageId, this.msgResources);
                        return false;
                    }
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

                // 定義情報シートからExcelPortバージョン番号を取得
                var versionNo = getCellValue(SheetName.DefineInfo, DefineSheetInfo.ColNoHeaderVal, DefineSheetInfo.RowNoVersion);
                if (!string.IsNullOrEmpty(versionNo))
                {
                    this.UploadCondition.ExcelPortVersion = Convert.ToDecimal(versionNo);
                }

                // 定義情報シートから出力日時、スケジュール開始年月を取得
                var outputDate = getCellValue(SheetName.DefineInfo, DefineSheetInfo.ColNoHeaderVal, DefineSheetInfo.RowNoOutputDate);
                if (!string.IsNullOrEmpty(outputDate))
                {
                    this.UploadCondition.OutputDate = DateTime.Parse(outputDate);
                    this.UploadCondition.ScheduleDateFrom = new DateTime(this.UploadCondition.OutputDate.Year, this.UploadCondition.OutputDate.Month, 1);
                    this.UploadCondition.ScheduleDateTo = this.UploadCondition.ScheduleDateFrom.AddMonths(ScheduleMonths - 1);
                }

                // エラー情報シートの最終行を取得
                int lastRowNo = this.ExcelCmd.GetLastRowNo(SheetNo.ErrorInfoDownloaded.ToString());
                List<string> errorRangeList = new();
                if (RowNo.ErrorInfoSheetDataStart <= lastRowNo)
                {
                    // エラー情報シートからエラー設定先のセル範囲情報を取得
                    errorRangeList = getErrorCellRangeList(lastRowNo);
                    // エラー情報シートをクリア
                    this.ExcelCmd.Clear(new string[] { RowNo.ErrorInfoSheetDataStart.ToString() + ":" + lastRowNo.ToString(), "1", SheetNo.ErrorInfoDownloaded.ToString() });
                }

                // 入力チェック＆変換
                var errorInfoList = checkUploadCondition<T>(ctrlGrpId, errorRangeList, ref resultList, ref errorMsg);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return false;
                }
                else if (errorInfoList.Count > 0)
                {
                    // シート番号取得
                    int sheetNo = int.Parse(condition["TargetSheetNo"].ToString());

                    // 階層系は独自の入力チェックをするので共通側でエラー情報シートにセットしない
                    if (sheetNo == SheetNo.SheetNoOfStructureGroup1000 || // 場所階層
                        sheetNo == SheetNo.SheetNoOfStructureGroup1010 || // 職種機種
                        sheetNo == SheetNo.SheetNoOfStructureGroup1040 || // 予備品ロケーション
                        sheetNo == SheetNo.SheetNoOfStructureGroup1760)   // 部門
                    {
                        return true;
                    }

                    // エラー情報シートへ設定
                    SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    return false;
                }
                else if (resultList == null || resultList.Count == 0)
                {
                    // 「該当データがありません。」
                    errorMsg = GetResMessage(ComRes.ID.ID941060001, this.languageId, this.msgResources);
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
                    if (dataCount > int.Parse(result))
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

            /// <summary>
            /// ファイル マッピング情報
            /// </summary>
            /// <typeparam name="T">データクラス</typeparam>
            /// <param name="factoryId">工場ID</param>
            /// <param name="programId">プログラムID</param>
            /// <param name="reportId">帳票ID</param>
            /// <param name="sheetNo">シート番号</param>
            /// <param name="templateId">テンプレートID</param>
            /// <param name="outputPattenId">出力パターンID</param>
            /// <param name="sheetDefine">シート定義</param>
            /// <param name="list">出力結果</param>
            /// <param name="templateFileName">テンプレートファイル名</param>
            /// <param name="templateFilePath">テンプレートファイルパス</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="optionRowCount">オプション行数</param>
            /// <param name="optionColumnCount">オプション行数</param>
            /// <param name="option">オプションクラス</param>
            /// <param name="optionDataList">オプションクラス</param>
            /// <returns>ファイル マッピング情報リスト</returns>
            public List<CommonExcelPrtInfo> CreateMappingListForExcelPort(
                int factoryId,
                string programId,
                string reportId,
                int templateId,
                int outputPattenId,
                ReportDao.MsOutputReportSheetDefineEntity sheetDefine,
                IList<IDictionary<string, object>> list,
                string templateFileName,
                string templateFilePath,
                string languageId,
                out int optionRowCount,
                out int optionColumnCount,
                out string detailMsg,
                ref string resultMsg,
                Option option = null)
            {
                // 現在日時取得
                DateTime now = DateTime.Now;

                // 初期化
                var mappingList = new List<CommonExcelPrtInfo>();

                optionRowCount = 0;
                optionColumnCount = 0;
                detailMsg = "";

                // 出力最大データ数チェック
                if (!CheckDownloadExcelMaxRowCnt(list.Count, this.db))
                {
                    // 「出力可能上限データ数を超えているため、ダウンロードできません。」
                    string msg = GetResMessage(ComRes.ID.ID141120013, languageId, msgResources);
                    resultMsg = msg;
                    detailMsg = msg;
                    return null;
                }

                int sheetNo = sheetDefine.SheetNo;

                // 帳票定義を取得
                var reportDefine = new ReportDao.MsOutputReportDefineEntity().GetEntity(factoryId, programId, reportId, this.db);

                // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
                TMQUtil.GetFixedSqlStatement(ExcelPath, ComReport.GetComReportInfo, out string baseSql);

                // 項目定義を取得
                IList<Dao.InoutDefine> reportInfoList = db.GetListByDataClass<Dao.InoutDefine>(
                    baseSql,
                    new { FactoryId = factoryId, LanguageId = languageId, ReportId = reportId, SheetNo = sheetNo, TemplateId = templateId, OutputPatternId = outputPattenId });
                if (reportInfoList == null || reportInfoList.Count == 0)
                {
                    // 取得できない場合、処理を戻す
                    return null;
                }

                if (this.DownloadCondition.SheetNo != SheetNo.ManagementStandards)
                {
                    // 開始行番号と開始列番号を設定
                    SetStartInfoExcelPort(ref reportInfoList, reportDefine.OutputItemType);
                }
                else
                {
                    // 長期計画 機器別管理基準シートの場合、スケジュール列の定義を設定(追加)
                    setScheduleRowReportInfo(sheetNo, templateFileName, templateFilePath, reportDefine, sheetDefine, reportInfoList, list, now);
                }
                // 共通用タイトル、共通用日付、共通用時刻をマッピング
                // 項目定義
                foreach (var reportInfo in reportInfoList)
                {
                    // 開始行番号、開始列番号が未指定の場合、スキップ
                    if (reportInfo.StartRowNo == null || reportInfo.StartColNo == null)
                    {
                        continue;
                    }
                    // カラム名が共通用タイトル、共通用日付、共通用時刻以外の場合、スキップ
                    if (!ColumnName.IsCommonColumn(reportInfo.ColumnName))
                    {
                        continue;
                    }

                    // 初期化
                    var info = new CommonExcelPrtInfo();
                    info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
                    info.SetSheetNo(sheetNo); // シート番号に対象のシート番号を設定
                    bool setTitle = false;
                    // マッピングセルを設定
                    string address = ToAlphabet((int)reportInfo.StartColNo) + reportInfo.StartRowNo;
                    // フォーマット設定
                    string format = ComReport.StrFormat;
                    object val;
                    // カラム名ごとに値を設定
                    switch (reportInfo.ColumnName)
                    {
                        // 共通タイトルの場合
                        case (ColumnName.ComTitle):
                            // 帳票名を設定
                            val = GetTranslationText(reportDefine.ReportNameTranslationId, languageId, db);
                            break;
                        // 共通日付の場合
                        case (ColumnName.ComDate):
                            // フォーマット：曜日の完全名, 月の省略名 月の日にち 年４桁 ※カルチャをen-US(英語/アメリカ合衆国)に指定
                            val = DateTime.Now.ToString("dddd, MMM d yyyy", new System.Globalization.CultureInfo("en-US"));
                            break;
                        // 共通時刻の場合
                        case (ColumnName.ComTime):
                            val = DateTime.Now.ToString("HH:mm:ss");
                            break;

                        // 共通シートタイトルの場合
                        case (ColumnName.ComSheetTitle):
                            // シート名を設定
                            val = GetTranslationText(sheetDefine.SheetNameTranslationId, languageId, db);
                            break;
                        // 共通バージョンの場合
                        case ColumnName.ComVersion:
                            val = option.Version.ToString(reportInfo.FormatText);
                            setTitle = true;
                            break;
                        // 共通日時の場合
                        case ColumnName.ComDateTime:
                            val = DateTime.Now.ToString(reportInfo.FormatText);
                            setTitle = true;
                            break;
                        // 共通対象機能IDの場合
                        case ColumnName.ComConductId:
                            val = option.TargetConductId;
                            setTitle = true;
                            break;
                        // 共通対象シート番号の場合
                        case ColumnName.ComSheetNo:
                            val = option.TargetSheetNo;
                            setTitle = true;
                            break;
                        default:
                            continue;
                    }

                    // マッピング情報設定
                    info.SetExlSetValueByAddress(address, val, format);
                    info.SetAddress(address);
                    info.SetColData(new[] { val });
                    // マッピングリストに追加
                    mappingList.Add(info);
                    if (setTitle)
                    {
                        // タイトル列に項目名をセット
                        address = ToAlphabet((int)reportInfo.StartColNo - 1) + reportInfo.StartRowNo;
                        val = reportInfo.ItemName;

                        // マッピング情報設定
                        info = new CommonExcelPrtInfo();
                        info.SetSheetName(null);
                        info.SetSheetNo(sheetNo);
                        info.SetExlSetValueByAddress(address, val, format);
                        info.SetAddress(address);
                        info.SetColData(new[] { val });
                        // マッピングリストに追加
                        mappingList.Add(info);
                    }
                }

                // 出力項目種別が3:出力項目固定（出力パターン指定あり※ベタ票）の場合、ヘッダをマッピング
                if (reportDefine.OutputItemType == ComReport.OutputItemType3)
                {
                    // 項目定義
                    foreach (var reportInfo in reportInfoList)
                    {
                        // 開始行番号、開始列番号が未指定の場合、スキップ
                        if (reportInfo.StartRowNo == null || reportInfo.StartColNo == null)
                        {
                            continue;
                        }
                        // 初期化
                        var info = new CommonExcelPrtInfo();
                        info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
                        info.SetSheetNo(sheetNo); // シート番号に対象のシート番号を設定
                                                  // 出力方式
                        int startColNo = 0;
                        int startRowNo = 0;
                        switch (reportInfo.OutputMethod.GetValueOrDefault())
                        {
                            // 1:単一セルの場合
                            case (ComReport.SingleCell):
                                continue; // 何もしない
                                          // 2:縦方向連続の場合
                            case (ComReport.LongitudinalDirection):
                                startColNo = (int)reportInfo.StartColNo;
                                startRowNo = reportInfo.StartRowNo.GetValueOrDefault() - 1; // 行がマイナス１
                                break;
                            // 3:横方向連続の場合
                            case (ComReport.LateralDirection):
                                startColNo = (int)reportInfo.StartColNo - 1;
                                startRowNo = reportInfo.StartRowNo.GetValueOrDefault(); // 行がマイナス１
                                break;
                            default:
                                continue; // 何もしない
                        }

                        // マッピングセルを設定
                        string address = ToAlphabet(startColNo) + startRowNo;
                        object val;
                        // 項目名がヘッダタイトル
                        val = reportInfo.ItemName;
                        // マッピング情報設定
                        info.SetAddress(address);
                        info.SetColData(new[] { val });
                        // マッピングリストに追加
                        mappingList.Add(info);
                    }

                }

                // 項目定義
                foreach (var reportInfo in reportInfoList)
                {
                    // 開始行番号、開始列番号が未指定の場合、スキップ
                    if (reportInfo.StartRowNo == null || reportInfo.StartColNo == null)
                    {
                        continue;
                    }
                    // カラム名が共通用タイトル、共通用日付、共通用時刻の場合、スキップ
                    if (ColumnName.IsCommonColumn(reportInfo.ColumnName))
                    {
                        continue;
                    }

                    // 初期化
                    var info = new CommonExcelPrtInfo();
                    info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
                    info.SetSheetNo(sheetNo); // シート番号に対象のシート番号を設定

                    //対象項目を取得する
                    object[] rowData = null;
                    string columnName = null;
                    if (list.Any(x => x.ContainsKey(reportInfo.ColumnName)))
                    {
                        columnName = reportInfo.ColumnName;
                    }
                    else if (list.Any(x => x.ContainsKey(ComUtil.SnakeCaseToPascalCase(reportInfo.ColumnName))))
                    {
                        // パスカルケースも
                        columnName = ComUtil.SnakeCaseToPascalCase(reportInfo.ColumnName);
                    }
                    if (!string.IsNullOrEmpty(columnName))
                    {
                        // カラム名ごとに値を設定

                        object val = list.Where(x => x.ContainsKey(columnName) && x[columnName] != null).Select(x => x[columnName]).FirstOrDefault();
                        if (val != null)
                        {
                            //型を取得
                            Type type = val.GetType();
                            //フォーマットした値を取得
                            if (type == typeof(DateTime))
                            {
                                //古すぎる日付の設定がエラーになるため、フォーマットして文字列として設定する
                                rowData = list.Select(x => x.ContainsKey(columnName) ? (x[columnName] != null && !string.IsNullOrEmpty(reportInfo.FormatText) ? ((DateTime)x[columnName]).ToString(reportInfo.FormatText) : x[columnName]) : null).ToArray();
                            }
                            else
                            {
                                rowData = list.Select(x => x.ContainsKey(columnName) ? x[columnName] : null).ToArray();
                            }
                        }
                    }
                    if (rowData != null)
                    {
                        switch (reportInfo.OutputMethod)
                        {
                            case ComReport.SingleCell:
                                // 単一セルの場合
                                rowData = new[] { rowData[0] };
                                break;
                            case ComReport.LongitudinalDirection:
                                // 縦方向連続の場合
                                break;
                            case ComReport.LateralDirection:
                                // 横方向連続の場合
                                rowData = new[] { rowData };
                                break;
                            default:
                                // 入出力方式が未設定の場合、スキップ
                                break;
                        }
                    }
                    // マッピングセルを設定
                    string address = ToAlphabet((int)reportInfo.StartColNo) + reportInfo.StartRowNo;
                    // フォーマット設定
                    string format = null;

                    if (reportInfo.DataType != null && reportInfo.DataType == ComReport.DataTypeStr)
                    {
                        format = ComReport.StrFormat;
                    }
                    // マッピング情報設定
                    info.SetExlSetValueByAddress(address, null, format);
                    info.SetAddress(address);
                    info.SetColData(rowData);
                    // マッピングリストに追加
                    mappingList.Add(info);
                }
                // マッピング情報リストを返却
                return mappingList;
            }

            /// <summary>
            /// エラー有無列番号文字列
            /// </summary>
            /// <param name="grpId">コントロールグループID</param>
            /// <returns></returns>
            public string GetErrorColLetter(string grpId)
            {
                if (this.ErrorColNoDic.ContainsKey(grpId))
                {
                    return ToAlphabet(this.ErrorColNoDic[grpId]);
                }
                else if (this.ErrorColNoDic.Count == 1)
                {
                    return ToAlphabet(this.ErrorColNoDic.Values.ToList()[0]);
                }
                else
                {
                    return string.Empty;
                }
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
                    SqlName.GetExcelPortTargetInfo, SqlName.SubDirName, new { ConductId = conductId, SheetNo = sheetNo.ToString(), LanguageId = this.languageId }, db);
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
                        string[] inParamList = paramList[i].Split('|');
                        ((IDictionary<string, object>)condition).Add(param, inParamList);
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
                        sheetName = string.Format(Format.SheetNo, sheetNo);
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
                        sheetName = string.Format(Format.SheetNo, i);
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
                            sheetName = string.Format(Format.SheetNo, i);
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
            /// エラー情報設定先セル範囲情報の取得
            /// </summary>
            /// <param name="lastRowNo">最終行番号</param>
            /// <returns></returns>
            private List<string> getErrorCellRangeList(int lastRowNo)
            {
                List<string> rangeList = new();

                // エラー情報設定先の行/列番号の取得
                string[,] values = getCellValuesBySheetNo(SheetNo.ErrorInfoDownloaded, ErrorSheetInfo.ColNoCol, ErrorSheetInfo.ColNoRow, ErrorSheetInfo.RowNoStart, lastRowNo);
                if (values.Length > 0)
                {
                    for (int i = 0; i < values.GetLength(0); i++)
                    {
                        if (values.GetLength(1) > 1)
                        {
                            if (!string.IsNullOrEmpty(values[i, 0]) && !string.IsNullOrEmpty(values[i, 1]))
                            {
                                rangeList.Add(values[i, 1] + values[i, 0]);
                            }
                        }
                    }
                }
                return rangeList;
            }

            /// <summary>
            /// アップロード条件のチェック
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="ctrlGrpId"></param>
            /// <param name="resultList"></param>
            /// <param name="errorMsg"></param>
            /// <param name="checkFlg"></param>
            /// <returns></returns>
            private List<ComBase.UploadErrorInfo> checkUploadCondition<T>(string ctrlGrpId, List<string> errorRangeList, ref List<T> resultList, ref string errorMsg, bool checkFlg = true)
            {
                // エラー内容格納クラス
                List<ComBase.UploadErrorInfo> errorInfo = new List<ComBase.UploadErrorInfo>();

                // ファイル入力項目定義情報を取得
                ComBase.InputDefineCondition param = new ComBase.InputDefineCondition()
                {
                    ReportId = Template.ReportId,
                    SheetNo = this.UploadCondition.SheetNo,
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

                // 先頭シートの最終行を取得
                int lastRowNo = this.ExcelCmd.GetLastRowNo();
                string rowRange = string.Format("{0}:{1}", rowNo, lastRowNo);
                // エラー情報が設定されているセルのみクリアする
                //// 先頭シートのコメントをクリア
                //this.ExcelCmd.ClearCellComment(new string[] { "ALL" });
                //// 先頭シートのエラー背景色をクリア
                //this.ExcelCmd.BackgroundColor(new string[] { rowRange, "NOCOLOR" });
                foreach (string range in errorRangeList)
                {
                    // 先頭シートのコメントをクリア
                    this.ExcelCmd.ClearCellComment(new string[] { range });
                    // 先頭シートのエラー背景色をクリア
                    this.ExcelCmd.BackgroundColor(new string[] { range, "NOCOLOR" });
                }
                // 先頭シートのエラー有無列をクリア
                var errorColInfo = reportInfoList.Where(x => x.EpColumnDivision == ColumnDivision.Error).ToList();
                if (errorColInfo != null)
                {
                    foreach (var errorCol in errorColInfo)
                    {
                        // エラー有無の列番号を取得
                        this.ErrorColNoDic.Add(errorCol.ControlGroupId, errorCol.StartColumnNo);
                        string errorColLetter = GetErrorColLetter(errorCol.ControlGroupId);
                        if (!string.IsNullOrEmpty(errorColLetter))
                        {
                            // 値のみをクリア
                            this.ExcelCmd.Clear(new string[] { string.Format("{0}{1}:{0}{2}", errorColLetter, rowNo, lastRowNo), "2" });
                        }
                    }
                }

                // キー項目、送信時処理ID、工場IDの項目定義を取得
                var keyDefines = reportInfoList.Where(x => x.EpColumnDivision == ColumnDivision.Key).ToList();
                var sendProcIdDefine = reportInfoList.Where(x => x.EpColumnDivision == ColumnDivision.SendProcId).FirstOrDefault();
                var sendProcNameDefine = reportInfoList.Where(x => x.EpSelectIdColumnNo == sendProcIdDefine.StartColumnNo).FirstOrDefault();
                var factoryIdDefine = reportInfoList.Where(x => x.EpColumnDivision == ColumnDivision.FactoryId).FirstOrDefault();

                // マスタ-場所階層などはfactoryIdDefineがNULLになるので、factoryIdDefineがNULLの場合はfactoryNameDefineもNULLとする
                var factoryNameDefine = factoryIdDefine != null ? reportInfoList.Where(x => x.EpSelectIdColumnNo == factoryIdDefine.StartColumnNo).FirstOrDefault() : null;

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
                    if (itemDataDic.ContainsKey(grpId))
                    {
                        // 取得済みの場合、スキップ
                        continue;
                    }
                    // コンボ選択アイテムデータの取得
                    var selectItemList = getComboBoxData(grpId, relationId, relationParam, factoryIdList);
                    if (selectItemList.Count > 0)
                    {
                        itemDataDic.Add(grpId, selectItemList);
                    }
                }

                // 送信時処理用翻訳リストを取得
                if (itemDataDic.ContainsKey(sendProcNameDefine.EpSelectGroupId))
                {
                    this.procDivItemList = itemDataDic[sendProcNameDefine.EpSelectGroupId];
                }

                int index = 0;
                int sheetNo = Template.OutputSheetNo;
                // マスタの「標準アイテム未使用」かどうか(該当する場合true)
                bool isMasterUnuse = this.UploadCondition.SheetNo == SheetNo.UnuseSheetNo;

                // 長期計画_機器別管理基準シートの場合、スケジュール列の取得＆チェックを行う
                bool needsScheduleCheck = this.UploadCondition.SheetNo == SheetNo.ManagementStandards;
                List<ScheduleInfo> scheduleInfoList = null;

                while (true)
                {
                    // エラー内容一時格納クラス
                    List<ComBase.UploadErrorInfo> tmpErrorInfo = new List<ComBase.UploadErrorInfo>();

                    bool flg = false; // データ存在チェックフラグ
                    object tmpResult = Activator.CreateInstance<T>();
                    bool rowCheckFlg = checkFlg; // 行の入力チェック実施フラグ

                    if (index > 0)
                    {
                        rowNo += addRow;
                    }

                    // 送信時処理IDを取得
                    string sendProcIdStr = getCellValueBySheetNo(sheetNo, sendProcIdDefine.StartColumnNo, rowNo);

                    // 送信時処理によらず、全件登録する機能があるため、該当のシート番号の場合は送信時処理を設定する
                    if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1000 || // 場所階層
                        this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1010 || // 職種機種
                        this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1040 || // 予備品ロケーション
                        this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1760    // 部門
                        )
                    {
                        sendProcIdStr = getCellValueBySheetNoMaster(sheetNo, sendProcIdDefine.StartColumnNo, rowNo, ctrlGrpId);
                    }

                    string sendProcIdName = getCellValueBySheetNo(sheetNo, sendProcNameDefine.StartColumnNo, rowNo);
                    if (!string.IsNullOrEmpty(sendProcIdName))
                    {
                        //ラベルを正とする
                        var tmpId = getItemValue(procDivItemList, TMQConsts.CommonFactoryId, ColName.Name, sendProcIdName, ColName.Id).ToString();
                        if (tmpId != null)
                        {
                            sendProcIdStr = tmpId.ToString();
                        }
                    }
                    else
                    {
                        //ラベルが空の場合、ラベルを正として処理対象外とする
                        //ただし、場所階層・職種機種・予備品ロケーション・部門は全件登録のため処理対象とする
                        if (this.UploadCondition.SheetNo != SheetNo.SheetNoOfStructureGroup1000 &&
                            this.UploadCondition.SheetNo != SheetNo.SheetNoOfStructureGroup1010 &&
                            this.UploadCondition.SheetNo != SheetNo.SheetNoOfStructureGroup1040 &&
                            this.UploadCondition.SheetNo != SheetNo.SheetNoOfStructureGroup1760
                            )
                        {
                            sendProcIdStr = string.Empty;
                        }
                    }

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
                        if (sendProcId != TMQConsts.SendProcessId.Regist)
                        {
                            // 送信時処理IDが登録でない場合、送信時処理列にメッセージをセットする
                            // 「新規追加データに対して内容更新・削除は指定できません。」
                            var msg = GetResMessage(ComRes.ID.ID141120014, languageId, msgResources);
                            tmpErrorInfo.Add(setTmpErrorInfo(rowNo, sendProcNameDefine.StartColumnNo, sendProcNameDefine.TranslationText, msg, dataDirection, sendProcIdStr, sendProcIdName));
                            setErrorInfo(ref errorInfo, tmpErrorInfo);
                            index++;
                        }
                    }
                    else
                    {
                        // 既存行の場合
                        if (sendProcId != TMQConsts.SendProcessId.Update && sendProcId != TMQConsts.SendProcessId.Delete)
                        {
                            // 「既存データに対して登録は指定できません。」
                            var msg = GetResMessage(ComRes.ID.ID141070006, languageId, msgResources);
                            tmpErrorInfo.Add(setTmpErrorInfo(rowNo, sendProcNameDefine.StartColumnNo, sendProcNameDefine.TranslationText, msg, dataDirection, sendProcIdStr, sendProcIdName));
                            setErrorInfo(ref errorInfo, tmpErrorInfo);
                            index++;
                        }
                    }
                    if (sendProcId == TMQConsts.SendProcessId.Delete)
                    {
                        //削除行は入力チェックしない
                        rowCheckFlg = false;
                    }

                    // 工場IDを取得
                    var targetFactoryId = -1;
                    if (factoryIdDefine != null)
                    {
                        string factoryIdStr = getCellValueBySheetNo(sheetNo, factoryIdDefine.StartColumnNo, rowNo);
                        if (factoryNameDefine != null)
                        {
                            //ラベルを正として工場IDを取得する
                            string factoryName = getCellValueBySheetNo(sheetNo, factoryNameDefine.StartColumnNo, rowNo);
                            if (!string.IsNullOrEmpty(factoryName))
                            {
                                var tmpId = getItemValueWithoutFactoryId(itemDataDic[factoryNameDefine.EpSelectGroupId], ColName.Name, factoryName, ColName.Id);
                                if (tmpId != null)
                                {
                                    factoryIdStr = tmpId.ToString();
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(factoryIdStr) && factoryIdDefine.EpSelectLinkColumnNo > 0 && factoryIdDefine.EpAutoExtentionColumnNo != null)
                        {
                            //工場IDが取得出来ていないかつ他の列の拡張により工場IDが決まる場合、そのアイテムデータから工場IDを取得する

                            InputDefineForExcelPort reportInfo = reportInfoList.Where(x => x.EpSelectIdColumnNo == factoryIdDefine.EpSelectLinkColumnNo).FirstOrDefault();
                            // 行番号をセット
                            reportInfo.StartRowNo = rowNo;

                            // 設定値を取得
                            string val = getCellValueBySheetNo(sheetNo, reportInfo.StartColumnNo, reportInfo.StartRowNo);
                            // 連動元列番号値を取得
                            string linkColumnVal = getLinkColumnVal(reportInfo, tmpErrorInfo);
                            // 連動元による絞り込み有無（連動元が工場IDの場合は除外）
                            bool linkColumnFlg;

                            // 階層系マスタの場合は対象外
                            if (isStructureMaster())
                            {
                                linkColumnFlg = false;
                            }
                            else
                            {
                                linkColumnFlg = reportInfo.EpSelectLinkColumnNo != factoryIdDefine.StartColumnNo && reportInfo.EpSelectLinkColumnNo > 0;
                            }
                            // コンボボックス、複数選択リストボックス、画面選択列の場合
                            // 対象行の選択項目IDを取得
                            var selectId = getCellValueBySheetNo(sheetNo, reportInfo.EpSelectIdColumnNo, reportInfo.StartRowNo);
                            var selectIdDefine = reportInfoList.Where(x => x.StartColumnNo == reportInfo.EpSelectIdColumnNo).FirstOrDefault();

                            if (itemDataDic.ContainsKey(reportInfo.EpSelectGroupId))
                            {
                                var itemList = itemDataDic[reportInfo.EpSelectGroupId];
                                if (itemList.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        if (!val.Contains(","))
                                        {
                                            //翻訳に紐づくアイテムデータが複数存在するかチェック(工場IDによる絞り込みは行わない)
                                            List<Dictionary<string, object>> targetItemList = getItemList(itemList, targetFactoryId, ColName.Name, val, linkColumnFlg, linkColumnVal, false);
                                            if (targetItemList != null && targetItemList.Count > 0)
                                            {
                                                if (targetItemList.Count == 1)
                                                {
                                                    //1件の場合
                                                    //工場IDが設定されている拡張列のキー（exparam1等）
                                                    string exparamKey = targetItemList[0].Select(x => x.Key).Where(x => x.ToUpper().Equals("EXPARAM" + factoryIdDefine.EpAutoExtentionColumnNo)).FirstOrDefault();
                                                    factoryIdStr = targetItemList[0].ContainsKey(exparamKey) ? targetItemList[0][exparamKey].ToString() : null;
                                                }
                                                else
                                                {
                                                    //複数ヒットした場合は設定されているIDが正しいかチェックする
                                                    bool existsFlg = targetItemList.Exists(x => x[ColName.Id].ToString() == selectId);
                                                    if (existsFlg)
                                                    {
                                                        //設定されているIDが正しい場合、そのアイテムデータを取得(工場IDによる絞り込みは行わない)
                                                        targetItemList = getItemList(targetItemList, targetFactoryId, ColName.Id, selectId, linkColumnFlg, linkColumnVal, false);
                                                        if (targetItemList != null && targetItemList.Count == 1)
                                                        {
                                                            //IDにより１件に絞れた場合
                                                            //工場IDが設定されている拡張列のキー（exparam1等）
                                                            string exparamKey = targetItemList[0].Select(x => x.Key).Where(x => x.ToUpper().Equals("EXPARAM" + factoryIdDefine.EpAutoExtentionColumnNo)).FirstOrDefault();
                                                            factoryIdStr = targetItemList[0].ContainsKey(exparamKey) ? targetItemList[0][exparamKey].ToString() : null;
                                                        }
                                                    }
                                                }
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
                                                //工場IDが設定されている拡張列のキー（exparam1等）
                                                string exparamKey = itemList[0].Select(x => x.Key).Where(x => x.ToUpper().Equals("EXPARAM" + factoryIdDefine.EpAutoExtentionColumnNo)).FirstOrDefault();
                                                factoryIdStr = itemList[0].ContainsKey(exparamKey) ? itemList[0][exparamKey].ToString() : null;

                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var msg = string.Empty;

                        if (!string.IsNullOrEmpty(factoryIdStr))
                        {
                            // マスタ 標準アイテム未使用 の場合は工場IDがパイプ「|」区切りになっているので分割
                            if (isMasterUnuse)
                            {
                                var unuseFactoryIdList = factoryIdStr.Split("|", StringSplitOptions.RemoveEmptyEntries);
                                foreach (string unuseFactoryId in unuseFactoryIdList)
                                {
                                    var factoryId = Convert.ToInt32(unuseFactoryId);
                                    if (this.ApprovalFactoryIdList.Count > 0 && this.ApprovalFactoryIdList.Contains(factoryId))
                                    {
                                        // 変更履歴管理対象工場の場合
                                        // 「変更履歴管理対象の工場データは登録できません。」
                                        msg = GetResMessage(ComRes.ID.ID141290007, languageId, msgResources);
                                        break;
                                    }
                                    else if (!this.ExcelPortFactoryIdList.Contains(factoryId))
                                    {
                                        // ExcelPort利用対象工場でない場合
                                        // 「ExcelPort利用可能工場以外のデータは登録できません。」
                                        msg = GetResMessage(ComRes.ID.ID141040004, languageId, msgResources);
                                        break;
                                    }
                                }

                                if (!string.IsNullOrEmpty(msg))
                                {
                                    tmpErrorInfo.Add(setTmpErrorInfo(rowNo, MasterColumnInfo.UnuseFactoryColNo, sendProcNameDefine.TranslationText, msg, dataDirection, sendProcIdStr, sendProcIdName));
                                    setErrorInfo(ref errorInfo, tmpErrorInfo);
                                    index++;
                                    continue;
                                }
                            }
                            else
                            {
                                // 工場IDが取得できた場合のみ以下のチェックを実行
                                var factoryId = Convert.ToInt32(factoryIdStr);
                                if (this.ApprovalFactoryIdList.Count > 0 && this.ApprovalFactoryIdList.Contains(factoryId))
                                {
                                    // 変更履歴管理対象工場の場合
                                    // 「変更履歴管理対象の工場データは登録できません。」
                                    msg = GetResMessage(ComRes.ID.ID141290007, languageId, msgResources);
                                }
                                else if (!this.ExcelPortFactoryIdList.Contains(factoryId))
                                {
                                    // マスタのアップロードで標準工場アイテムが登録されることがあるので指定されたシート番号の場合は下記メッセージを設定しない
                                    if (!SheetNo.NormalMasterSheetNoList.Contains(this.UploadCondition.SheetNo))
                                    {
                                        // ExcelPort利用対象工場でない場合
                                        // 「ExcelPort利用可能工場以外のデータは登録できません。」
                                        msg = GetResMessage(ComRes.ID.ID141040004, languageId, msgResources);
                                    }
                                }

                                if (!string.IsNullOrEmpty(msg))
                                {
                                    tmpErrorInfo.Add(setTmpErrorInfo(rowNo, sendProcNameDefine.StartColumnNo, sendProcNameDefine.TranslationText, msg, dataDirection, sendProcIdStr, sendProcIdName));
                                    setErrorInfo(ref errorInfo, tmpErrorInfo);
                                    index++;
                                    continue;
                                }
                                targetFactoryId = factoryId;
                            }
                            // 工場IDをデータクラスへセット
                            setCellValueToDataClass<T>(factoryIdDefine, properites, tmpResult, factoryIdStr);
                        }
                    }
                    if (targetFactoryId < 0)
                    {
                        targetFactoryId = TMQConsts.CommonFactoryId;
                    }

                    // 送信時処理IDをデータクラスへセット
                    setCellValueToDataClass<T>(sendProcIdDefine, properites, tmpResult, sendProcIdStr);

                    // 取得できた項目定義分処理を行う
                    foreach (InputDefineForExcelPort reportInfo in reportInfoList)
                    {
                        if (reportInfo.EpColumnDivision != null)
                        {
                            // 列区分が設定されている列は別にチェックを行うためスキップ
                            continue;
                        }
                        if (reportInfo.EpSelectLinkColumnNo > 0 && reportInfo.EpAutoExtentionColumnNo != null)
                        {
                            // 自動表示拡張列番号が設定されている列は連動元のチェック時に値を設定するためスキップ
                            continue;
                        }

                        // 行番号をセット
                        reportInfo.StartRowNo = rowNo;

                        if (needsScheduleCheck)
                        {
                            var colName = reportInfo.AliasName != null ? reportInfo.AliasName : reportInfo.ColumnName;
                            // スケジュール列のチェックが必要な場合
                            /*if (colName.Equals(ColName.ScheduleId))
                            {
                                // スケジュールID列の場合スキップ
                                continue;
                            }
                            else */
                            if (colName.Equals(ColName.Schedule))
                            {
                                // スケジュールチェック処理実行
                                scheduleInfoList = new List<ScheduleInfo>();
                                checkScheduleColumns(rowCheckFlg, sheetNo, targetFactoryId, dataDirection, sendProcIdStr, sendProcIdName,
                                    reportInfo, itemDataDic[reportInfo.EpSelectGroupId], this.UploadCondition.OutputDate, scheduleInfoList, tmpErrorInfo);
                                continue;
                            }
                        }

                        // 設定値を取得
                        string val = getCellValueBySheetNo(sheetNo, reportInfo.StartColumnNo, reportInfo.StartRowNo);
                        // 連動元列番号値を取得
                        string linkColumnVal = getLinkColumnVal(reportInfo, tmpErrorInfo);
                        // 連動元による絞り込み有無（連動元が工場IDの場合は除外）
                        bool linkColumnFlg;

                        // 階層系マスタの場合は対象外
                        if (isStructureMaster())
                        {
                            linkColumnFlg = false;
                        }
                        else
                        {
                            linkColumnFlg = factoryIdDefine != null && reportInfo.EpSelectLinkColumnNo != factoryIdDefine.StartColumnNo && reportInfo.EpSelectLinkColumnNo > 0;
                        }

                        if (string.IsNullOrEmpty(val))
                        {
                            // 値が取得できない場合
                            if (reportInfo.ColumnType != ColumnType.ComboBox &&
                                reportInfo.ColumnType != ColumnType.MultiListBox &&
                                reportInfo.ColumnType != ColumnType.FormSelect)
                            {
                                // 選択項目でない場合、スキップ
                                if (rowCheckFlg)
                                {
                                    if (reportInfo.RequiredFlg != null && (bool)reportInfo.RequiredFlg)
                                    {
                                        // 必須入力項目の場合、エラー内容を設定
                                        // 「必須項目です。入力してください。」
                                        var msg = GetResMessage(ComRes.ID.ID941270001, languageId, msgResources);
                                        tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, msg, dataDirection, sendProcIdStr, sendProcIdName));
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

                        //コンボボックス、複数選択リストボックス、画面選択列の場合の対象アイテムデータ
                        Dictionary<string, object> targetItem = null;

                        if (reportInfo.ColumnType != ColumnType.ComboBox &&
                            reportInfo.ColumnType != ColumnType.MultiListBox &&
                            reportInfo.ColumnType != ColumnType.FormSelect)
                        {
                            // 階層系マスタか判定
                            if (isStructureMaster())
                            {
                                // 入力チェックを個別に行うため値をデータクラスに設定
                                setCellValueToDataClass<T>(reportInfo, properites, tmpResult, val);

                                //拡張項目の値をデータクラスに設定
                                setExParamValueToDataClass(reportInfo, targetItem, tmpResult);
                            }

                            if (rowCheckFlg)
                            {
                                // アップロード共通チェック実行
                                if (!ExecuteCommonUploadCheck(reportInfo, val, dataDirection, languageId, msgResources, this.db, tmpErrorInfo))
                                {
                                    tmpErrorInfo[tmpErrorInfo.Count - 1].ProcDiv = sendProcIdStr;
                                    tmpErrorInfo[tmpErrorInfo.Count - 1].ProcDivName = sendProcIdName;
                                    continue;
                                }

                                //書式チェック
                                if (!string.IsNullOrEmpty(reportInfo.FormatText))
                                {
                                    if (!checkFormat(reportInfo, val))
                                    {
                                        // 書式チェックエラーの場合、エラーを設定し、スキップ
                                        // 「{0}で入力してください。」
                                        string error = GetResMessage(new string[] { ComRes.ID.ID941190002, reportInfo.FormatText }, languageId, msgResources);
                                        tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection, sendProcIdStr, sendProcIdName));
                                        continue;
                                    }

                                }
                            }
                            else
                            {
                                //型チェックのみ行う（データクラスへの設定時にエラーとならないようにする）
                                string error = string.Empty;
                                if (!checkCellType(reportInfo, val, languageId, msgResources, ref error))
                                {
                                    // 型が異なる場合、スキップ
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
                            else if (reportInfo.DataType == ComReport.DataTypeInt || reportInfo.DataType == ComReport.DataTypeNum)
                            {
                                // 数値の場合、桁区切りを削除
                                if (ComUtil.ConvertDecimal(val) != null)
                                {
                                    val = ComUtil.ConvertDecimal(val).ToString();
                                }
                            }
                        }
                        else
                        {
                            // コンボボックス、複数選択リストボックス、画面選択列の場合
                            // 対象行の選択項目IDを取得
                            var selectId = getCellValueBySheetNo(sheetNo, reportInfo.EpSelectIdColumnNo, reportInfo.StartRowNo);
                            var selectIdDefine = reportInfoList.Where(x => x.StartColumnNo == reportInfo.EpSelectIdColumnNo).FirstOrDefault();

                            //画面選択列のエラー設定フラグ
                            bool formSelectError = false;

                            //// 標準アイテム未使用 の場合は初期設定値に戻す
                            //if (isMasterUnuse)
                            //{
                            //    targetFactoryId = -1;
                            //}

                            //if (targetFactoryId < 0)
                            //{
                            //    // 対象行の工場IDを取得
                            //    if (factoryIdDefine != null)
                            //    {
                            //        var tmpId = getCellValueBySheetNo(sheetNo, factoryIdDefine.StartColumnNo, reportInfo.StartRowNo);
                            //        if (!string.IsNullOrEmpty(tmpId))
                            //        {
                            //            // 標準アイテム未使用 の場合は工場IDがパイプ「|」区切りになっていて数値に変換できない為「0」
                            //            if (isMasterUnuse)
                            //            {
                            //                targetFactoryId = TMQConsts.CommonFactoryId;
                            //            }
                            //            else
                            //            {
                            //                targetFactoryId = Convert.ToInt32(tmpId);
                            //            }          
                            //        }
                            //    }
                            //    if (targetFactoryId < 0)
                            //    {
                            //        targetFactoryId = TMQConsts.CommonFactoryId;
                            //    }
                            //}
                            if (itemDataDic.ContainsKey(reportInfo.EpSelectGroupId))
                            {
                                var itemList = itemDataDic[reportInfo.EpSelectGroupId];
                                if (itemList.Count > 0)
                                {
                                    // 選択項目の場合、ここで必須チェックする
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        //object transText = null;
                                        //if (!string.IsNullOrEmpty(selectId))
                                        //{
                                        //    if (!selectId.Contains("|"))
                                        //    {
                                        //        // 標準アイテム未使用 の場合はtargetFactoryIdを「0」としているため、tmpIdを代入
                                        //        if (isMasterUnuse)
                                        //        {
                                        //            targetFactoryId = Convert.ToInt32(selectId);
                                        //        }

                                        //        // 選択項目IDと工場IDから翻訳を取得
                                        //        transText = getItemValue(itemList, targetFactoryId, ColName.Id, selectId, ColName.Name, linkColumnFlg, linkColumnVal);
                                        //    }
                                        //    else
                                        //    {
                                        //        // バー区切りの場合、分割して取得
                                        //        var selectIds = selectId.Split("|");
                                        //        foreach (var tmpId in selectIds)
                                        //        {
                                        //            // 標準アイテム未使用 の場合はtargetFactoryIdを「0」としているため、tmpIdを代入
                                        //            if (isMasterUnuse)
                                        //            {
                                        //                targetFactoryId = Convert.ToInt32(tmpId);
                                        //            }

                                        //            var text = getItemValue(itemList, targetFactoryId, ColName.Id, tmpId, ColName.Name, linkColumnFlg, linkColumnVal);
                                        //            if (!ComUtil.IsNullOrEmpty(text))
                                        //            {
                                        //                if (transText != null)
                                        //                {
                                        //                    transText += ",";
                                        //                }
                                        //                transText += text.ToString();
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        //if (ComUtil.IsNullOrEmpty(transText) || val != transText.ToString())
                                        //{
                                        //    // 対象工場の翻訳でない場合、翻訳からIDを取得

                                        //翻訳を正として、翻訳からIDを取得
                                        object tmpId = null;
                                        // 複数選択項目の場合、いずれかが不正なアイテムであればエラー
                                        bool multErrorFlg = false;
                                        if (!val.Contains(","))
                                        {
                                            if (targetFactoryId == TMQConsts.CommonFactoryId)
                                            {
                                                //工場IDが取得できていない場合

                                                //翻訳に紐づくアイテムデータが複数存在するかチェック(工場IDによる絞り込みは行わない)
                                                List<Dictionary<string, object>> targetItemList = getItemList(itemList, targetFactoryId, ColName.Name, val, linkColumnFlg, linkColumnVal, false);
                                                if (targetItemList != null && targetItemList.Count > 0)
                                                {
                                                    if (targetItemList.Count == 1)
                                                    {
                                                        targetItem = targetItemList.FirstOrDefault();
                                                        tmpId = targetItem[ColName.Id].ToString();
                                                    }
                                                    else
                                                    {
                                                        //複数ヒットした場合は設定されているIDが正しいかチェックする
                                                        bool existsFlg = targetItemList.Exists(x => x[ColName.Id].ToString() == selectId);
                                                        if (existsFlg)
                                                        {
                                                            //設定されているIDが正しい場合、そのアイテムデータを取得(工場IDによる絞り込みは行わない)
                                                            targetItemList = getItemList(targetItemList, targetFactoryId, ColName.Id, selectId, linkColumnFlg, linkColumnVal, false);
                                                            if (targetItemList != null && targetItemList.Count == 1)
                                                            {
                                                                //IDにより１件に絞れた場合
                                                                targetItem = targetItemList.FirstOrDefault();
                                                                tmpId = selectId;
                                                            }
                                                        }

                                                        if (ComUtil.IsNullOrEmpty(tmpId))
                                                        {
                                                            //１件に絞れないためエラー
                                                            // 「名称により項目を絞り込むことができません。画面より項目を指定してください。」
                                                            var msg = GetResMessage(ComRes.ID.ID141340001, languageId, msgResources);
                                                            tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, msg, dataDirection, sendProcIdStr, sendProcIdName));
                                                            formSelectError = true;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //翻訳に紐づくアイテムデータが複数存在するかチェック
                                                List<Dictionary<string, object>> targetItemList = getItemList(itemList, targetFactoryId, ColName.Name, val, linkColumnFlg, linkColumnVal);
                                                if (targetItemList != null && targetItemList.Count > 0)
                                                {
                                                    if (targetItemList.Count == 1)
                                                    {
                                                        targetItem = targetItemList.FirstOrDefault();
                                                        tmpId = targetItem[ColName.Id].ToString();
                                                    }
                                                    else
                                                    {
                                                        //複数ヒットした場合は設定されているIDが正しいかチェックする
                                                        object checkId = getItemValue(targetItemList, targetFactoryId, ColName.Id, selectId, ColName.Id);
                                                        if (!ComUtil.IsNullOrEmpty(checkId))
                                                        {
                                                            //設定されているIDが正しい場合、そのアイテムデータを取得
                                                            targetItem = getItemList(targetItemList, targetFactoryId, ColName.Id, selectId).FirstOrDefault();
                                                            tmpId = checkId;
                                                        }
                                                        else
                                                        {
                                                            //設定されているIDが正しくない場合、翻訳に一致する先頭アイテムデータを取得
                                                            targetItem = targetItemList.FirstOrDefault();
                                                            tmpId = targetItem[ColName.Id].ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // カンマ区切りの場合、分割して取得
                                            var vals = val.Split(",");
                                            foreach (var tmpVal in vals)
                                            {
                                                if (targetFactoryId == TMQConsts.CommonFactoryId)
                                                {
                                                    //工場IDが取得できていない場合

                                                    //翻訳に紐づくアイテムデータが複数存在するかチェック(工場IDによる絞り込みは行わない)
                                                    List<Dictionary<string, object>> targetItemList = getItemList(itemList, targetFactoryId, ColName.Name, tmpVal, linkColumnFlg, linkColumnVal, false);
                                                    if (targetItemList != null && targetItemList.Count > 0)
                                                    {
                                                        if (targetItemList.Count == 1)
                                                        {
                                                            if (tmpId != null)
                                                            {
                                                                tmpId += "|";
                                                            }
                                                            targetItem = targetItemList.FirstOrDefault();
                                                            tmpId += targetItem[ColName.Id].ToString();
                                                        }
                                                        else
                                                        {
                                                            //複数ヒットした場合は設定されているIDが正しいかチェックする
                                                            string tmpSelectId = targetItemList.Where(x => selectId.Split("|").Contains(x[ColName.Id].ToString())).Select(x => x[ColName.Id].ToString()).FirstOrDefault();
                                                            if (!string.IsNullOrEmpty(tmpSelectId))
                                                            {
                                                                //設定されているIDが正しい場合、そのアイテムデータを取得(工場IDによる絞り込みは行わない)
                                                                targetItemList = getItemList(targetItemList, targetFactoryId, ColName.Id, tmpSelectId, linkColumnFlg, linkColumnVal, false);
                                                                if (targetItemList != null && targetItemList.Count == 1)
                                                                {
                                                                    //IDにより１件に絞れた場合
                                                                    if (tmpId != null)
                                                                    {
                                                                        tmpId += "|";
                                                                    }
                                                                    targetItem = targetItemList.FirstOrDefault();
                                                                    tmpId += tmpSelectId;
                                                                }
                                                            }

                                                            if (string.IsNullOrEmpty(tmpSelectId) || targetItemList == null || targetItemList.Count != 1)
                                                            {
                                                                //１件に絞れないためエラー
                                                                // 「名称により項目を絞り込むことができません。画面より項目を指定してください。」
                                                                var msg = GetResMessage(ComRes.ID.ID141340001, languageId, msgResources);
                                                                tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, msg, dataDirection, sendProcIdStr, sendProcIdName));
                                                                formSelectError = true;
                                                                multErrorFlg = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // 選択項目IDが取得できない場合
                                                        multErrorFlg = true;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    //翻訳に紐づくアイテムデータが複数存在するかチェック
                                                    List<Dictionary<string, object>> targetItemList = getItemList(itemList, targetFactoryId, ColName.Name, tmpVal, linkColumnFlg, linkColumnVal);
                                                    if (targetItemList != null && targetItemList.Count > 0)
                                                    {
                                                        if (targetItemList.Count == 1)
                                                        {
                                                            if (tmpId != null)
                                                            {
                                                                tmpId += "|";
                                                            }
                                                            targetItem = targetItemList.FirstOrDefault();
                                                            tmpId += targetItem[ColName.Id].ToString();
                                                        }
                                                        else
                                                        {
                                                            //複数ヒットした場合は設定されているIDが正しいかチェックする
                                                            string tmpSelectId = targetItemList.Where(x => selectId.Split("|").Contains(x[ColName.Id].ToString())).Select(x => x[ColName.Id].ToString()).FirstOrDefault();
                                                            if (!ComUtil.IsNullOrEmpty(tmpSelectId))
                                                            {
                                                                if (tmpId != null)
                                                                {
                                                                    tmpId += "|";
                                                                }
                                                                //設定されているIDが正しい場合、そのアイテムデータを取得
                                                                targetItem = getItemList(targetItemList, targetFactoryId, ColName.Id, tmpSelectId).FirstOrDefault();
                                                                tmpId += tmpSelectId;
                                                            }
                                                            else
                                                            {
                                                                //設定されているIDが正しくない場合、翻訳に一致する先頭アイテムデータを取得
                                                                targetItem = targetItemList.FirstOrDefault();
                                                                tmpId += targetItem[ColName.Id].ToString();
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // 選択項目IDが取得できない場合
                                                        multErrorFlg = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        if (!ComUtil.IsNullOrEmpty(tmpId) && !multErrorFlg)
                                        {
                                            // 選択項目IDが取得できた場合、Excelへ値をセット
                                            selectId = tmpId.ToString();
                                            setCellValueBySheetNo(sheetNo, selectIdDefine.StartColumnNo, reportInfo.StartRowNo, selectId);

                                            //拡張項目の値をExcelに設定
                                            setExParamValueToExcel(reportInfo, targetItem);
                                        }
                                        else
                                        {
                                            selectId = string.Empty;
                                        }
                                        //}
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
                                                val = itemList[0][ColName.Name].ToString();
                                                selectId = itemList[0][ColName.Id].ToString();
                                                targetItem = itemList[0];
                                                // 入力項目が存在する場合、フラグをたてる
                                                flg = true;
                                            }
                                            else
                                            {
                                                if (rowCheckFlg)
                                                {
                                                    // 選択項目でない場合、エラー内容を設定
                                                    // 「必須項目です。入力してください。」
                                                    var msg = GetResMessage(ComRes.ID.ID941270001, languageId, msgResources);
                                                    tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, msg, dataDirection, sendProcIdStr, sendProcIdName));
                                                }
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            selectId = string.Empty;
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
                                if (rowCheckFlg && !string.IsNullOrEmpty(val) && !formSelectError)
                                {
                                    // 選択項目が空でない場合
                                    // 「選択内容が不正です。」
                                    var msg = GetResMessage(ComRes.ID.ID141140004, languageId, msgResources);
                                    tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, msg, dataDirection, sendProcIdStr, sendProcIdName));
                                }

                                // 階層系マスタか判定
                                if (isStructureMaster())
                                {
                                    // 入力チェックを個別に行うため値をデータクラスに設定
                                    setCellValueToDataClass<T>(reportInfo, properites, tmpResult, val);

                                    //拡張項目の値をデータクラスに設定
                                    setExParamValueToDataClass(reportInfo, targetItem, tmpResult);
                                }

                                continue;
                            }
                        }

                        // 値をデータクラスに設定
                        setCellValueToDataClass<T>(reportInfo, properites, tmpResult, val);

                        //拡張項目の値をデータクラスに設定
                        setExParamValueToDataClass(reportInfo, targetItem, tmpResult);
                    }

                    if (scheduleInfoList != null && scheduleInfoList.Count > 0)
                    {
                        // スケジュールリストが空でない場合、データクラスに設定
                        flg = true;
                        setDataClassValue(properites, tmpResult, "SCHEDULELIST", scheduleInfoList);
                    }

                    // データが1件も取得できなかった場合、処理を抜ける
                    if (!flg)
                    {
                        break;
                    }

                    // 行番号を設定する
                    setDataClassValue(properites, tmpResult, "ROWNO", rowNo);

                    // エラーがある場合、エラーフラグを立てる
                    if (tmpErrorInfo.Count > 0)
                    {
                        setDataClassValue(properites, tmpResult, "ERRORFLG", true);
                    }

                    // データが存在する場合、リストに追加する
                    setErrorInfo(ref errorInfo, tmpErrorInfo);
                    resultList.Add((T)tmpResult);
                    index++;
                }
                return errorInfo;

                // 連動元の値を取得
                string getLinkColumnVal(InputDefineForExcelPort reportInfo,  List<ComBase.UploadErrorInfo> tmpErrorInfo)
                {
                    // 連動元の値を取得
                    string linkColumnVal = getCellValueBySheetNo(sheetNo, reportInfo.EpSelectLinkColumnNo, reportInfo.StartRowNo);

                    //連動元は基本ID列なので、ラベル列の定義を取得
                    InputDefineForExcelPort linkReportInfo = reportInfoList.Where(x => x.EpSelectIdColumnNo == reportInfo.EpSelectLinkColumnNo).FirstOrDefault();
                    if (linkReportInfo == null)
                    {
                        //ラベル列が無い場合は連動元列番号に設定された列を取得
                        linkReportInfo = reportInfoList.Where(x => x.StartColumnNo == reportInfo.EpSelectLinkColumnNo).FirstOrDefault();
                    }
                    //連動元にエラーがあるか
                    bool linkErrorFlg = tmpErrorInfo.Any(x => x.RowNo.Contains(rowNo) && x.ColumnNo.Contains(linkReportInfo.StartColumnNo));
                    if (linkErrorFlg)
                    {
                        //連動元にエラーがある場合（IDが設定されていて、ラベルが適切でない等）、連動元の値は空とする
                        linkColumnVal = string.Empty;
                    }
                    return linkColumnVal;
                }

                //自動表示拡張列の値をデータクラスに設定
                void setExParamValueToDataClass(InputDefineForExcelPort reportInfo, Dictionary<string, object> targetItem, object tmpResult)
                {
                    //対象の連動元列番号が設定されている列定義を取得
                    List<InputDefineForExcelPort> exReportInfoList = reportInfoList.Where(x => x.EpSelectLinkColumnNo == reportInfo.EpSelectIdColumnNo && x.EpAutoExtentionColumnNo != null).ToList();
                    foreach (InputDefineForExcelPort exReportInfo in exReportInfoList)
                    {
                        //値が設定されている拡張列のキー（exparam1等）
                        string exparamKey = targetItem.Select(x => x.Key).Where(x => x.ToUpper().Equals("EXPARAM" + exReportInfo.EpAutoExtentionColumnNo)).FirstOrDefault();
                        if (string.IsNullOrEmpty(exparamKey))
                        {
                            continue;
                        }
                        //値を取得
                        string exVal = targetItem.ContainsKey(exparamKey) ? targetItem[exparamKey].ToString() : null;
                        //値をデータクラスに設定
                        setCellValueToDataClass<T>(exReportInfo, properites, tmpResult, exVal);
                    }
                }

                //自動表示拡張列の値をExcelに設定
                void setExParamValueToExcel(InputDefineForExcelPort reportInfo, Dictionary<string, object> targetItem)
                {
                    //対象の連動元列番号が設定されている列定義を取得
                    List<InputDefineForExcelPort> exReportInfoList = reportInfoList.Where(x => x.EpSelectLinkColumnNo == reportInfo.EpSelectIdColumnNo && x.EpAutoExtentionColumnNo != null).ToList();
                    foreach (InputDefineForExcelPort exReportInfo in exReportInfoList)
                    {
                        // 行番号をセット
                        exReportInfo.StartRowNo = rowNo;

                        //値が設定されている拡張列のキー（exparam1等）
                        string exparamKey = targetItem.Select(x => x.Key).Where(x => x.ToUpper().Equals("EXPARAM" + exReportInfo.EpAutoExtentionColumnNo)).FirstOrDefault();
                        if (string.IsNullOrEmpty(exparamKey))
                        {
                            continue;
                        }
                        //値を取得
                        string exVal = targetItem.ContainsKey(exparamKey) ? targetItem[exparamKey].ToString() : null;
                        //値をExcelに設定
                        setCellValueBySheetNo(sheetNo, exReportInfo.StartColumnNo, exReportInfo.StartRowNo, exVal);
                    }
                }
            }

            /// <summary>
            /// 選択項目値の取得
            /// </summary>
            /// <param name="itemList">選択項目リスト</param>
            /// <param name="targetFactoryId">対象工場ID</param>
            /// <param name="keyName">キー項目名</param>
            /// <param name="keyVal">キー項目値</param>
            /// <param name="linkColumnFlg">選択項目連動元の値による絞り込み有無</param>
            /// <param name="linkColumnVal">選択項目連動元の値</param>
            /// <param name="useFactoryId">工場IDによる絞り込み有無</param>
            /// <returns></returns>
            private List<Dictionary<string, object>> getItemList(List<Dictionary<string, object>> itemList, int targetFactoryId, string keyName, string keyVal, bool linkColumnFlg = false, string linkColumnVal = null, bool useFactoryId = true)
            {
                return itemList.Where(x =>
                    x[keyName].ToString() == keyVal &&
                    (useFactoryId ? (x["factory_id"].ToString() == targetFactoryId.ToString() || x["factory_id"].ToString() == TMQConsts.CommonFactoryId.ToString()) : true) &&
                    (linkColumnFlg ? x["parent_id"].ToString() == linkColumnVal : true) &&
                    (x.ContainsKey("unuse_factory_id") ? !x["unuse_factory_id"].ToString().Split("|").Contains(targetFactoryId.ToString()) : true)) //未使用標準アイテムは除外
                    .ToList();
            }

            /// <summary>
            /// 選択項目値の取得
            /// </summary>
            /// <param name="itemList">選択項目リスト</param>
            /// <param name="targetFactoryId">対象工場ID</param>
            /// <param name="keyName">キー項目名</param>
            /// <param name="keyVal">キー項目値</param>
            /// <param name="valName">取得対象項目名</param>
            /// <param name="linkColumnFlg">選択項目連動元の値による絞り込み有無</param>
            /// <param name="linkColumnVal">選択項目連動元の値</param>
            /// <returns></returns>
            private object getItemValue(List<Dictionary<string, object>> itemList, int targetFactoryId, string keyName, string keyVal, string valName, bool linkColumnFlg = false, string linkColumnVal = null)
            {
                return itemList.Where(x =>
                    x[keyName].ToString() == keyVal &&
                    (x["factory_id"].ToString() == targetFactoryId.ToString() ||
                    x["factory_id"].ToString() == TMQConsts.CommonFactoryId.ToString()) &&
                    (linkColumnFlg ? x["parent_id"].ToString() == linkColumnVal : true) &&
                    (x.ContainsKey("unuse_factory_id") ? !x["unuse_factory_id"].ToString().Split("|").Contains(targetFactoryId.ToString()) : true)) //未使用標準アイテムは除外
                    .Select(x => x[valName]).FirstOrDefault();
            }

            /// <summary>
            /// 選択項目値の取得(工場ID未指定)
            /// </summary>
            /// <param name="itemList">選択項目リスト</param>
            /// <param name="keyName">キー項目名</param>
            /// <param name="keyVal">キー項目値</param>
            /// <param name="valName">取得対象項目名</param>
            /// <returns></returns>
            private object getItemValueWithoutFactoryId(List<Dictionary<string, object>> itemList, string keyName, string keyVal, string valName)
            {
                return itemList.Where(x =>
                    x[keyName].ToString() == keyVal)
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
            /// Excel範囲の値を取得(シート名指定)
            /// </summary>
            /// <param name="sheetName">シート名</param>
            /// <param name="colNoFrom">開始列番号</param>
            /// <param name="colNoTo">終了列番号</param>
            /// <param name="rowNoFrom">開始行番号</param>
            /// <param name="rowNoTo">終了行番号</param>
            /// <returns></returns>
            private string[,] getCellValues(string sheetName, int colNoFrom, int colNoTo, int rowNoFrom, int rowNoTo)
            {
                string error = string.Empty;
                string msg = string.Empty;
                string[,] vals = null;

                // マッピングセルを設定
                string address = ToAlphabet(colNoFrom) + rowNoFrom + ":" + ToAlphabet(colNoTo) + rowNoTo;
                // 指定範囲でデータを取得する
                if (!this.ExcelCmd.ReadExcel(sheetName, address, ref vals, ref msg))
                {
                    // 読込失敗した場合、nullを返す
                    return null;
                }
                return vals;
            }

            /// <summary>
            /// Excel範囲の値を取得(シート番号指定)
            /// </summary>
            /// <param name="sheetNo"></param>
            /// <param name="colNoFrom">開始列番号</param>
            /// <param name="colNoTo">終了列番号</param>
            /// <param name="rowNoFrom">開始行番号</param>
            /// <param name="rowNoTo">終了行番号</param>
            /// <returns></returns>
            private string[,] getCellValuesBySheetNo(int sheetNo, int colNoFrom, int colNoTo, int rowNoFrom, int rowNoTo)
            {
                string error = string.Empty;
                string msg = string.Empty;
                string[,] vals = null;

                // マッピングセルを設定
                string address = ToAlphabet(colNoFrom) + rowNoFrom + ":" + ToAlphabet(colNoTo) + rowNoTo;
                // セル単位でデータを取得する
                if (!this.ExcelCmd.ReadExcelBySheetNo(sheetNo, address, ref vals, ref msg))
                {
                    // 読込失敗した場合、nullを返す
                    return null;
                }
                return vals;
            }

            /// <summary>
            /// Excelセルの値を取得(マスタ用)
            /// </summary>
            /// <param name="sheetNo"></param>
            /// <param name="colNo"></param>
            /// <param name="rowNo"></param>
            /// <returns></returns>
            private string getCellValueBySheetNoMaster(int sheetNo, int colNo, int rowNo, string ctrlGrpId)
            {
                string error = string.Empty;
                string msg = string.Empty;
                string[,] vals = null;

                int startColNo = 0;
                int endColNo = 0;

                if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1000 && ctrlGrpId == MasterColumnInfo.StructureGroup1000.District.ControlGroupId)
                {
                    // 場所階層(地区)
                    startColNo = MasterColumnInfo.StructureGroup1000.District.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1000.District.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1000 && ctrlGrpId == MasterColumnInfo.StructureGroup1000.Factory.ControlGroupId)
                {
                    // 場所階層(工場)
                    startColNo = MasterColumnInfo.StructureGroup1000.Factory.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1000.Factory.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1000 && ctrlGrpId == MasterColumnInfo.StructureGroup1000.Plant.ControlGroupId)
                {
                    // 場所階層(プラント)
                    startColNo = MasterColumnInfo.StructureGroup1000.Plant.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1000.Plant.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1000 && ctrlGrpId == MasterColumnInfo.StructureGroup1000.Series.ControlGroupId)
                {
                    // 場所階層(系列)
                    startColNo = MasterColumnInfo.StructureGroup1000.Series.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1000.Series.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1000 && ctrlGrpId == MasterColumnInfo.StructureGroup1000.Stroke.ControlGroupId)
                {
                    // 場所階層(工程)
                    startColNo = MasterColumnInfo.StructureGroup1000.Stroke.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1000.Stroke.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1000 && ctrlGrpId == MasterColumnInfo.StructureGroup1000.Facility.ControlGroupId)
                {
                    // 場所階層(設備)
                    startColNo = MasterColumnInfo.StructureGroup1000.Facility.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1000.Facility.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1010 && ctrlGrpId == MasterColumnInfo.StructureGroup1010.District.ControlGroupId)
                {
                    // 職種機種(地区)
                    startColNo = MasterColumnInfo.StructureGroup1010.District.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1010.District.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1010 && ctrlGrpId == MasterColumnInfo.StructureGroup1010.Factory.ControlGroupId)
                {
                    // 職種機種(工場)
                    startColNo = MasterColumnInfo.StructureGroup1010.Factory.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1010.Factory.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1010 && ctrlGrpId == MasterColumnInfo.StructureGroup1010.Job.ControlGroupId)
                {
                    // 職種機種(職種)
                    startColNo = MasterColumnInfo.StructureGroup1010.Job.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1010.Job.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1010 && ctrlGrpId == MasterColumnInfo.StructureGroup1010.Large.ControlGroupId)
                {
                    // 職種機種(機種大分類)
                    startColNo = MasterColumnInfo.StructureGroup1010.Large.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1010.Large.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1010 && ctrlGrpId == MasterColumnInfo.StructureGroup1010.Middle.ControlGroupId)
                {
                    // 職種機種(機種中分類)
                    startColNo = MasterColumnInfo.StructureGroup1010.Middle.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1010.Middle.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1010 && ctrlGrpId == MasterColumnInfo.StructureGroup1010.Small.ControlGroupId)
                {
                    // 職種機種(機種中分類)
                    startColNo = MasterColumnInfo.StructureGroup1010.Small.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1010.Small.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1040 && ctrlGrpId == MasterColumnInfo.StructureGroup1040.District.ControlGroupId)
                {
                    // 予備品ロケーション(地区)
                    startColNo = MasterColumnInfo.StructureGroup1040.District.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1040.District.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1040 && ctrlGrpId == MasterColumnInfo.StructureGroup1040.Factory.ControlGroupId)
                {
                    // 予備品ロケーション(地区)
                    startColNo = MasterColumnInfo.StructureGroup1040.Factory.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1040.Factory.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1040 && ctrlGrpId == MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId)
                {
                    // 予備品ロケーション(倉庫)
                    startColNo = MasterColumnInfo.StructureGroup1040.Warehouse.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1040.Warehouse.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1040 && ctrlGrpId == MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId)
                {
                    // 予備品ロケーション(棚)
                    startColNo = MasterColumnInfo.StructureGroup1040.Rack.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1040.Rack.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1760 && ctrlGrpId == MasterColumnInfo.StructureGroup1760.District.ControlGroupId)
                {
                    // 部門(地区)
                    startColNo = MasterColumnInfo.StructureGroup1760.District.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1760.District.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1760 && ctrlGrpId == MasterColumnInfo.StructureGroup1760.Factory.ControlGroupId)
                {
                    // 部門(部門)
                    startColNo = MasterColumnInfo.StructureGroup1760.Factory.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1760.Factory.EndCol;
                }
                else if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1760 && ctrlGrpId == MasterColumnInfo.StructureGroup1760.Department.ControlGroupId)
                {
                    // 部門(部門)
                    startColNo = MasterColumnInfo.StructureGroup1760.Department.StartCol;
                    endColNo = MasterColumnInfo.StructureGroup1760.Department.EndCol;
                }


                // 1レコード全ての列が空欄(= 処理終了)かどうか判定
                string address = string.Empty;
                for (int i = startColNo; i <= endColNo; i++)
                {
                    // マッピングセルを設定
                    address = ToAlphabet(i) + rowNo;

                    // セル単位でデータを取得する
                    if (!this.ExcelCmd.ReadExcelBySheetNo(sheetNo, address, ref vals, ref msg))
                    {
                        // 読込失敗した場合、nullを返す
                        return null;
                    }

                    if (!string.IsNullOrEmpty(vals[0, 0]))
                    {
                        // 空欄でない列がある場合は送信時処理「更新」を返す
                        return TMQConsts.SendProcessId.Update.ToString();
                    }

                }

                return string.Empty;

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

            private void setDataClassValue<T>(PropertyInfo[] properites, T target, string propName, object val)
            {
                var prop = properites.FirstOrDefault(x => x.Name.ToUpper().Equals(propName));
                if (prop != null)
                {
                    ComUtil.SetPropertyValue<T>(prop, (T)target, val);
                }

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
                fileName = string.Format(Format.FileName,
                    reportId, DateTime.Now, this.UploadCondition.ExcelPortVersion, ComConsts.REPORT.EXTENSION.EXCEL_MACRO_BOOK);

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
                    for (int i = 0; i < info.RowNo.Count; i++)
                    {
                        int colNo = 0;
                        for (int j = 0; j < info.ColumnNo.Count; j++)
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
                                Value = info.RowNo[i]
                            });
                            // 列
                            mappingList.Add(new CommonExcelUtil.MappingInfo()
                            {
                                X = colNo++,
                                Y = rowNo,
                                Value = ToAlphabet(info.ColumnNo[j])
                            });
                            // 処理区分名
                            var procDivName = info.ProcDivName;
                            if (string.IsNullOrEmpty(procDivName))
                            {
                                procDivName = this.procDivItemList.Where(x => x[ColName.Id].ToString().Equals(info.ProcDiv)).Select(x => x[ColName.Name].ToString()).FirstOrDefault();
                            }
                            mappingList.Add(new CommonExcelUtil.MappingInfo()
                            {
                                X = colNo++,
                                Y = rowNo,
                                Value = procDivName
                            });
                            // エラー情報
                            mappingList.Add(new CommonExcelUtil.MappingInfo()
                            {
                                X = colNo++,
                                Y = rowNo,
                                Value = info.ErrorInfo
                            });

                            // 機能別シート上セル位置
                            string rangeTo = ToAlphabet(info.ColumnNo[j]) + info.RowNo[i];
                            // コメント(メモ)を設定(既存のコメントに追加する)
                            this.ExcelCmd.SetCellComment(new string[] {
                                rangeTo, info.ErrorInfo, "", "1" });
                            // 背景色を設定
                            this.ExcelCmd.BackgroundColor(new string[] {
                                rangeTo, ColorValues.Error });

                            string rangeErr;
                            string errorColLetter = GetErrorColLetter(info.CtrlGrpId);
                            if (!string.IsNullOrEmpty(errorColLetter))
                            {
                                // エラー有無列に「あり」設定
                                rangeErr = errorColLetter + info.RowNo[i];
                                this.ExcelCmd.SetCellValue(new string[] {
                                rangeErr, errText });
                            }
                            // エラー情報シート上セル位置
                            rangeErr = ToAlphabet(colNo) + (rowNo + 1);
                            // ハイパーリンクを設定
                            this.ExcelCmd.SetHyperLink(new string[] {
                                rangeErr, rangeTo, sheetNameErr, sheetName });
                            // ハイパーリンクの文字色を設定
                            this.ExcelCmd.FontColor(new string[] {
                                rangeErr, ColorValues.HyperLink, sheetNameErr });
                            // 罫線を設定
                            this.ExcelCmd.LineBox(new string[] {
                                string.Format("A{0}:{1}", rowNo + 1, rangeErr), "IO", "", sheetNameErr});
                        }
                    }
                }
                // エラー情報シートへエラー情報を設定
                this.ExcelCmd.SetValue(mappingList, "", SheetNo.ErrorInfoDownloaded);

                string[] errSheetParam = new string[] { SheetNo.ErrorInfoDownloaded.ToString() };
                // エラー情報シートを表示
                this.ExcelCmd.ShowSheet(errSheetParam);
                // エラー情報シートをアクティブ化
                this.ExcelCmd.ActivateSheet(errSheetParam);

                // 先頭シートの選択を解除
                this.ExcelCmd.UnSelectSheet(new string[] { });

                // メモリストリームへ保存
                if (ms == null)
                {
                    ms = new MemoryStream();
                }
                this.ExcelCmd.Save(ref ms);

                return true;
            }

            /// <summary>
            /// 開始情報設定(ExcelPort用)
            /// </summary>
            /// <param name="reportInfoList">シート番号</param>
            /// <param name="templateFileName">テンプレートファイル名</param>
            /// <param name="templateFilePath">テンプレートファイルパス</param>
            /// <param name="sheetNo">シート番号</param>
            /// <param name="sheetDefineMaxRow">シート定義最大行</param>
            /// <param name="sheetDefineMaxColumn">シート定義最大列</param>
            /// <param name="outputItemType">出力項目種別</param>
            public static void SetStartInfoExcelPort(ref IList<Dao.InoutDefine> reportInfoList, int outputItemType)
            {
                foreach (var reportInfo in reportInfoList)
                {
                    // 出力マスタに開始情報が設定されている場合
                    if (reportInfo.OutputDefaultCellRowNo != null && reportInfo.OutputDefaultCellColumnNo != null)
                    {
                        // 出力項目種別が3:出力項目固定（出力パターン指定あり※ベタ票）の場合
                        if (outputItemType == ComReport.OutputItemType3)
                        {
                            // 開始情報を設定する
                            reportInfo.StartRowNo = reportInfo.OutputDefaultCellRowNo;
                            reportInfo.StartColNo = reportInfo.OutputDefaultCellColumnNo;
                            continue;
                        }

                    }
                    // 定義マスタに開始情報が設定されている場合
                    if (reportInfo.DefaultCellRowNo != null && reportInfo.DefaultCellColumnNo != null)
                    {
                        // 出力項目種別が3:出力項目固定（出力パターン指定あり※ベタ票）の場合
                        if (outputItemType == ComReport.OutputItemType3)
                        {
                            // 開始情報を設定する
                            reportInfo.StartRowNo = reportInfo.DefaultCellRowNo;
                            reportInfo.StartColNo = reportInfo.DefaultCellColumnNo;
                            continue;
                        }
                    }
                }
            }

            /// <summary>
            /// スケジュール列項目定義設定処理
            /// </summary>
            /// <param name="sheetNo">シート番号</param>
            /// <param name="templateFileName">テンプレートファイル名</param>
            /// <param name="templateFilePath">テンプレートファイルパス</param>
            /// <param name="reportDefine">帳票定義</param>
            /// <param name="sheetDefine">シート定義</param>
            /// <param name="reportInfoList">項目定義リスト</param>
            /// <param name="list">出力データリスト</param>
            /// <param name="now">現在日時</param>
            private void setScheduleRowReportInfo(
                int sheetNo,
                string templateFileName,
                string templateFilePath,
                ReportDao.MsOutputReportDefineEntity reportDefine,
                ReportDao.MsOutputReportSheetDefineEntity sheetDefine,
                IList<Dao.InoutDefine> reportInfoList,
                IList<IDictionary<string, object>> list,
                DateTime now)
            {

                // スケジュール列の項目定義を取得
                var scheduleIdReportInfoList = reportInfoList.Where(x => x.ColumnName.StartsWith(ColName.Schedule)).ToList();
                var scheduleTxReportInfo = reportInfoList.Where(x => x.ColumnName.Equals(ColName.Schedule)).FirstOrDefault();
                if (scheduleIdReportInfoList != null && scheduleIdReportInfoList.Count > 0)
                {
                    // スケジュール列が存在する場合、対象期間のスケジュール列データを作成
                    string itemName;
                    for (int i = 1; i < ScheduleMonths; i++)
                    {
                        itemName = now.AddMonths(i).ToString(Format.ScheduleDate);
                        foreach (var reportInfo in scheduleIdReportInfoList)
                        {
                            // スケジュール列データをコピー
                            var tmpInfo = reportInfo.Copy();
                            tmpInfo.ItemId += i;
                            tmpInfo.ColumnName += (i + 1).ToString();
                            tmpInfo.DefaultCellColumnNo += (i * 2);
                            tmpInfo.OutputDefaultCellColumnNo += (i * 2);
                            tmpInfo.ItemName = itemName;
                            tmpInfo.ItemDisplayName = itemName;
                            reportInfoList.Add(tmpInfo);
                        }
                    }
                    itemName = now.ToString(Format.ScheduleDate);
                    foreach (var reportInfo in scheduleIdReportInfoList)
                    {
                        // 先頭のスケジュール列のカラム名に通し番号を付加
                        reportInfo.ColumnName += "1";
                        reportInfo.ItemName = itemName;
                        reportInfo.ItemDisplayName = itemName;
                    }
                }

                // 開始行番号と開始列番号を設定
                SetStartInfoExcelPort(ref reportInfoList, reportDefine.OutputItemType);

                // スケジュール列のデータを取得
                var scheduleIdData = list.Where(x => x.ContainsKey(ColName.ColumnName) && ColName.ScheduleId.Equals(x[ColName.ColumnName].ToString())).FirstOrDefault();
                var scheduleTxData = list.Where(x => x.ContainsKey(ColName.ColumnName) && ColName.Schedule.Equals(x[ColName.ColumnName].ToString())).FirstOrDefault();
                if (scheduleIdData != null && scheduleTxData != null)
                {
                    // スケジュール列が存在する場合、対象期間のスケジュール列データを作成

                    // 出力対象シート番号取得
                    var sheetNoSc = scheduleIdData[ColName.SheetNo];
                    string itemName;
                    string fmtColName = "{0}{1}";
                    string fmtSheetNoAndColNo = "{0}_{1}";
                    for (int i = 1; i < ScheduleMonths; i++)
                    {
                        // スケジュールID列データをコピー
                        var idData = new Dictionary<string, object>(scheduleIdData);

                        // ヘッダ表示名はスケジュール年月
                        itemName = now.AddMonths(i).ToString(Format.ScheduleDate);

                        // 項目ID
                        idData[ColName.ItemId] = Convert.ToInt32(idData[ColName.ItemId]) + (i * 2);
                        // カラム名
                        idData[ColName.ColumnName] = string.Format(fmtColName, idData[ColName.ColumnName], i + 1);
                        // 列番号
                        var colNoId = Convert.ToInt32(idData[ColName.ColumnNo]) + (i * 2);
                        idData[ColName.ColumnNo] = colNoId;
                        // シート番号_列番号
                        idData[ColName.SheetNoAndColumnNo] = string.Format(fmtSheetNoAndColNo, sheetNoSc, colNoId);
                        // ヘッダ表示名
                        idData[ColName.ItemName] = itemName;
                        list.Add(idData);

                        // スケジュール文字列列データをコピー
                        var txData = new Dictionary<string, object>(scheduleTxData);
                        var colNoTx = Convert.ToInt32(txData[ColName.ColumnNo]) + (i * 2);

                        // 項目ID
                        txData[ColName.ItemId] = Convert.ToInt32(txData[ColName.ItemId]) + (i * 2);
                        // カラム名
                        txData[ColName.ColumnName] = string.Format(fmtColName, txData[ColName.ColumnName], i + 1);
                        // 列番号
                        txData[ColName.ColumnNo] = colNoTx;
                        // シート番号_列番号
                        txData[ColName.SheetNoAndColumnNo] = string.Format(fmtSheetNoAndColNo, sheetNoSc, colNoTx);
                        // ヘッダ表示名
                        txData[ColName.ItemName] = itemName;
                        // 選択ID値格納先列番号
                        txData[ColName.SelectIdColumnNo] = colNoId;
                        list.Add(txData);
                    }
                    // カラム名
                    scheduleIdData[ColName.ColumnName] = string.Format(fmtColName, scheduleIdData[ColName.ColumnName], 1);
                    scheduleTxData[ColName.ColumnName] = string.Format(fmtColName, scheduleTxData[ColName.ColumnName], 1);
                    // ヘッダ表示名
                    itemName = now.ToString(Format.ScheduleDate);
                    scheduleIdData[ColName.ItemName] = itemName;
                    scheduleTxData[ColName.ItemName] = itemName;
                }
            }

            /// <summary>
            /// スケジュール列のチェック
            /// </summary>
            /// <param name="checkFlg">チェックフラグ</param>
            /// <param name="sheetNo">シート番号</param>
            /// <param name="factoryId">工場ID</param>
            /// <param name="dataDirection">データの方向</param>
            /// <param name="sendProcIdStr">送信時処理ID</param>
            /// <param name="sendProcIdName">送信時処理名</param>
            /// <param name="reportInfo">ファイル入力管理クラス</param>
            /// <param name="itemList"></param>
            /// <param name="outputDate">Excelファイル出力日時</param>
            /// <param name="scheduleList">スケジュール情報リスト</param>
            /// <param name="errorInfoList">エラー情報リスト</param>
            private void checkScheduleColumns(
                bool checkFlg, int sheetNo, int factoryId, int dataDirection, string sendProcIdStr, string sendProcIdName,
                InputDefineForExcelPort reportInfo, List<Dictionary<string, object>> itemList, DateTime outputDate,
                List<ScheduleInfo> scheduleList, List<ComBase.UploadErrorInfo> errorInfoList)
            {
                DateTime dateFrom = new DateTime(outputDate.Year, outputDate.Month, 1);
                for (int i = 0; i < ScheduleMonths; i++)
                {
                    var colNoId = reportInfo.EpSelectIdColumnNo + i * 2;
                    var colNoText = reportInfo.StartColumnNo + i * 2;
                    var scheduleId = getCellValueBySheetNo(sheetNo, colNoId, reportInfo.StartRowNo);
                    var scheduleText = getCellValueBySheetNo(sheetNo, colNoText, reportInfo.StartRowNo);

                    if (string.IsNullOrEmpty(scheduleText))
                    {
                        // スケジュール文字列が空の場合はスキップ
                        continue;
                    }

                    // スケジュールIDから翻訳を取得
                    var transText = getItemValue(itemList, factoryId, ColName.Id, scheduleId, ColName.Name);
                    if (ComUtil.IsNullOrEmpty(transText) || !scheduleText.Equals(transText))
                    {
                        // 対象工場の翻訳でない場合、翻訳からIDを取得
                        object tmpId = getItemValue(itemList, factoryId, ColName.Name, scheduleText, ColName.Id);
                        if (!ComUtil.IsNullOrEmpty(tmpId))
                        {
                            // 選択項目IDが取得できた場合、Excelへ値をセット
                            scheduleId = tmpId.ToString();
                            setCellValueBySheetNo(sheetNo, colNoId, reportInfo.StartRowNo, scheduleId);
                        }
                        else
                        {
                            scheduleId = string.Empty;
                        }
                        //if (!string.IsNullOrEmpty(scheduleId))
                        //{
                        //    // スケジュールをリストへ追加
                        //    scheduleList.Add(new ScheduleInfo()
                        //    {
                        //        ColumnNo = colNoText + i,
                        //        ScheduleId = Convert.ToInt32(scheduleId),
                        //        Schedule = scheduleText,
                        //        ScheduleDate = dateFrom.AddMonths(i),
                        //    });
                        //}
                        //else
                        if (string.IsNullOrEmpty(scheduleId))
                        {
                            // スケジュールIDが空の場合
                            if (checkFlg && !string.IsNullOrEmpty(scheduleText))
                            {
                                // スケジュール文字列が空でない場合
                                // 「選択内容が不正です。」
                                var msg = GetResMessage(ComRes.ID.ID141140004, languageId, msgResources);
                                errorInfoList.Add(setTmpErrorInfo(reportInfo.StartRowNo, colNoText, reportInfo.TranslationText, msg, dataDirection, sendProcIdStr, sendProcIdName));
                            }
                            continue;
                        }

                    }
                    if (!string.IsNullOrEmpty(scheduleId))
                    {
                        // スケジュールをリストへ追加
                        scheduleList.Add(new ScheduleInfo()
                        {
                            ColumnNo = colNoText,
                            ScheduleId = Convert.ToInt32(scheduleId),
                            Schedule = scheduleText,
                            ScheduleDate = dateFrom.AddMonths(i),
                        });
                    }
                }
            }

            /// <summary>
            /// 階層系マスタか判定
            /// </summary>
            /// <returns></returns>
            private bool isStructureMaster()
            {
                // 階層系マスタかどうかを判定
                if (this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1000 || // 場所階層
                    this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1010 || // 職種機種
                    this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1040 || // 予備品ロケーション
                    this.UploadCondition.SheetNo == SheetNo.SheetNoOfStructureGroup1760)   // 部門
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 取込処理 書式チェック
            /// </summary>
            /// <param name="reportInfoList">ファイル入力項目定義情報</param>
            /// <param name="val">文字列</param>
            /// <returns>true:正常、false:異常</returns>
            private static bool checkFormat(InputDefineForExcelPort reportInfo, string val)
            {
                if (reportInfo.ColumnType == ColumnType.Numeric)
                {
                    //数値###
                    if (val.IndexOf(".") >= 0)
                    {
                        //小数点以下がある場合、チェック

                        if (reportInfo.FormatText.IndexOf(".") < 0)
                        {
                            //書式に小数点以下がない場合、エラー
                            return false;
                        }
                        decimal? workVal = ComUtil.ConvertDecimal(val);
                        if (workVal == null)
                        {
                            //変換できない場合、正常で処理を戻す ※型チェックなどのエラーは他で判定しているため
                            return true;
                        }
                        //値の小数部桁数
                        int inputLen = workVal.ToString().Length - workVal.ToString().IndexOf(".") - 1;
                        //書式の小数部桁数
                        int formatLen = reportInfo.FormatText.Length - reportInfo.FormatText.IndexOf(".") - 1;
                        if (inputLen > formatLen)
                        {
                            //小数部の桁数が大きい場合、エラー
                            return false;
                        }
                    }
                }
                else if (reportInfo.ColumnType == ColumnType.Date || reportInfo.ColumnType == ColumnType.Time)
                {
                    //日付、時刻
                    if (!ComUtil.IsDateTimeFormat(val, reportInfo.FormatText))
                    {
                        return false;
                    }
                }
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
        public int? EpAutoExtentionColumnNo { get; set; }
        /// <summary>列区分(ExcelPort用)</summary>
        public int? EpColumnDivision { get; set; }
    }

    /// <summary>
    /// スケジュール情報
    /// </summary>
    public class ScheduleInfo
    {
        /// <summary>列番号</summary>
        public int ColumnNo { get; set; }
        /// <summary>保全スケジュール詳細ID</summary>
        public int MaintainanceScheduleDetailId { get; set; }
        /// <summary>スケジュールID</summary>
        public int ScheduleId { get; set; }
        /// <summary>スケジュール文字列</summary>
        public string Schedule { get; set; }
        /// <summary>スケジュール年月</summary>
        public DateTime ScheduleDate { get; set; }
    }
}
