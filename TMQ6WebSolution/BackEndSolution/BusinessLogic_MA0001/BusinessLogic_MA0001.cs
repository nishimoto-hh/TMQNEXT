using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MA0001.BusinessLogicDataClass_MA0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;

namespace BusinessLogic_MA0001
{
    /// <summary>
    /// 保全活動
    /// </summary>
    public partial class BusinessLogic_MA0001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Maintenance";
            /// <summary>SQL格納先サブディレクトリ名：機器台帳</summary>
            public const string SubDirMachine = @"Machine";

            /// <summary>
            /// 一覧画面で使用するSQL
            /// </summary>
            public static class List
            {
                /// <summary>SQL名：一覧取得</summary>
                public const string GetList = "GetMaintenanceList";
                /// <summary>SQL名：一覧取得(件名単位に取得するSQL)</summary>
                public const string AddGetList = "ADD_GetMaintenanceList";
                /// <summary>SQL名：ユーザ役割取得</summary>
                public const string GetUserRole = "GetUserRole";
            }

            /// <summary>
            /// 詳細画面で使用するSQL
            /// </summary>
            public static class Detail
            {
                /// <summary>SQL名：件名情報、作業性格情報取得</summary>
                public const string GetSummaryInfo = "GetSummaryInfo";
                /// <summary>SQL名：点検：対象機器一覧取得</summary>
                public const string GetInspectionMachineList = "GetInspectionMachineList";
                /// <summary>SQL名：故障：対象機器一覧取得</summary>
                public const string GetFailureMachineList = "GetFailureMachineList";
                /// <summary>SQL名：故障情報取得</summary>
                public const string GetFailureInfo = "GetFailureInfo";
                /// <summary>SQL名：依頼概要、依頼申請情報取得</summary>
                public const string GetRequestInfo = "GetRequestInfo";
                /// <summary>SQL名：保全計画情報取得</summary>
                public const string GetPlanInfo = "GetPlanInfo";
                /// <summary>SQL名：保全履歴情報取得</summary>
                public const string GetHistoryInfo = "GetHistoryInfo";
                /// <summary>SQL名：保全履歴情報(個別工場)取得</summary>
                public const string GetHistoryIndividualInfo = "GetHistoryIndividualInfo";
                /// <summary>SQL名：保全活動件名情報削除</summary>
                public const string DeleteSummary = "DeleteSummary";
                /// <summary>SQL名：保全依頼情報削除</summary>
                public const string DeleteRequest = "DeleteRequest";
                /// <summary>SQL名：保全計画情報削除</summary>
                public const string DeletePlan = "DeletePlan";
                /// <summary>SQL名：保全履歴情報削除</summary>
                public const string DeleteHistory = "DeleteHistory";
                /// <summary>SQL名：保全履歴機器削除</summary>
                public const string DeleteHistoryMachine = "DeleteHistoryMachine";
                /// <summary>SQL名：保全履歴機器部位削除</summary>
                public const string DeleteHistoryInspectionSite = "DeleteHistoryInspectionSite";
                /// <summary>SQL名：保全履歴点検内容削除</summary>
                public const string DeleteHistoryInspectionContent = "DeleteHistoryInspectionContent";
                /// <summary>SQL名：保全履歴故障情報削除</summary>
                public const string DeleteHistoryFailure = "DeleteHistoryFailure";
                /// <summary>SQL名：添付情報取得</summary>
                public const string GetAttachmentInfo = "GetAttachmentInfo";
                /// <summary>SQL名：添付情報削除</summary>
                public const string DeleteAttachment = "DeleteAttachment";
                /// <summary>SQL名：故障分析情報取得</summary>
                public const string GetFailureAnalyzeInfo = "GetFailureAnalyzeInfo";
                /// <summary>SQL名：故障分析情報(個別工場)取得</summary>
                public const string GetFailureAnalyzeIndividualInfo = "GetFailureAnalyzeIndividualInfo";
                /// <summary>SQL名：保全スケジュール詳細 保全活動件名ID更新</summary>
                public const string UpdateScheduleSummaryId = "UpdateScheduleSummaryId";
                /// <summary>指示検収票ボタン押下時の出力ファイル情報の取得SQL</summary>
                public const string GetOutputFileInfo = "GetOutputFileInfo";
            }

            /// <summary>
            /// 新規登録、修正画面で使用するSQL
            /// </summary>
            public static class Regist
            {
                /// <summary>SQL名：依頼番号採番取得</summary>
                public const string GetRequestNumbering = "GetRequestNumberingForLock";
                /// <summary>SQL名：依頼番号採番登録</summary>
                public const string InsertRequestNumbering = "InsertRequestNumbering";
                /// <summary>SQL名：依頼番号採番更新</summary>
                public const string UpdateRequestNumbering = "UpdateRequestNumbering";
                /// <summary>SQL名：保全活動件名登録</summary>
                public const string InsertSummary = "InsertSummaryInfo";
                /// <summary>SQL名：保全活動件名更新</summary>
                public const string UpdateSummary = "UpdateSummaryInfo";
                /// <summary>SQL名：保全依頼登録</summary>
                public const string InsertRequest = "InsertRequestInfo";
                /// <summary>SQL名：保全依頼更新</summary>
                public const string UpdateRequest = "UpdateRequestInfo";
                /// <summary>SQL名：保全計画登録</summary>
                public const string InsertPlan = "InsertPlanInfo";
                /// <summary>SQL名：保全計画更新</summary>
                public const string UpdatePlan = "UpdatePlanInfo";
                /// <summary>SQL名：保全履歴登録</summary>
                public const string InsertHistory = "InsertHistoryInfo";
                /// <summary>SQL名：保全履歴更新</summary>
                public const string UpdateHistory = "UpdateHistoryInfo";
                /// <summary>SQL名：保全履歴機器取得</summary>
                public const string GetHistoryMachine = "GetHistoryMachineInfo";
                /// <summary>SQL名：保全履歴機器登録</summary>
                public const string InsertHistoryMachine = "InsertHistoryMachineInfo";
                /// <summary>SQL名：保全履歴機器更新</summary>
                public const string UpdateHistoryMachine = "UpdateHistoryMachineInfo";
                /// <summary>SQL名：保全履歴機器部位取得</summary>
                public const string GetHistoryInspectionSite = "GetHistoryInspectionSiteInfo";
                /// <summary>SQL名：保全履歴機器部位登録</summary>
                public const string InsertHistoryInspectionSite = "InsertHistoryInspectionSiteInfo";
                /// <summary>SQL名：保全履歴機器部位件数取得</summary>
                public const string GetCountHistoryInspectionSite = "GetCountHistoryInspectionSiteInfo";
                /// <summary>SQL名：保全履歴点検内容件数取得</summary>
                public const string GetCountHistoryInspectionContent = "GetCountHistoryInspectionContentInfo";
                /// <summary>SQL名：保全履歴点検内容登録</summary>
                public const string InsertHistoryInspectionContent = "InsertHistoryInspectionContentInfo";
                /// <summary>SQL名：保全履歴点検内容更新</summary>
                public const string UpdateHistoryInspectionContent = "UpdateHistoryInspectionContentInfo";
                /// <summary>SQL名：保全履歴故障情報取得</summary>
                public const string GetFailureInfoForHistoryId = "GetFailureInfoForHistoryId";
                /// <summary>SQL名：保全履歴故障情報登録</summary>
                public const string InsertHistoryFailure = "InsertHistoryFailureInfo";
                /// <summary>SQL名：保全履歴故障情報更新</summary>
                public const string UpdateHistoryFailure = "UpdateHistoryFailureInfo";
                /// <summary>SQL名：保全スケジュール詳細 件数取得</summary>
                public const string GetCountMaintainanceScheduleDetail = "GetCountMaintainanceScheduleDetail";
                /// <summary>SQL名：保全スケジュール詳細 完了日更新</summary>
                public const string UpdateComplition = "UpdateComplition";
                /// <summary>SQL名：保全スケジュール詳細 スケジュール日更新対象の件数取得</summary>
                public const string GetCountTargetSchedule = "GetCountTargetSchedule";
                /// <summary>SQL名：保全スケジュール詳細 スケジュール日更新</summary>
                public const string UpdateScheduleDate = "UpdateScheduleDate";
                /// <summary>SQL名：件名別長期計画・機器別長期計画の白丸「○」リンクから遷移してきた際の初期値検索</summary>
                public const string GetScheduleFromLongPlan = "GetScheduleFromLongPlan";
                /// <summary>SQL名：件名別長期計画・機器別長期計画の白丸「○」リンクから遷移してきた際の対象機器一覧初期値検索</summary>
                public const string GetMachineListFromLongPlan = "GetMachineListFromLongPlan";
                /// <summary>SQL名：保全スケジュール詳細 保全活動件名ID更新</summary>
                public const string UpdateScheduleDetailSummaryId = "UpdateScheduleDetailSummaryId";
                /// <summary>SQL名：保全スケジュール詳細IDより長期計画件名IDを取得</summary>
                public const string GetLongPlanIdByScheduleDetailId = "GetLongPlanIdByScheduleDetailId";
                /// <summary>SQL名：保全スケジュール詳細IDより最大更新日時を取得</summary>
                public const string GetMaxUpdateDateByScheduleDetailId = "GetMaxUpdateDateByScheduleDetailId";
            }

            /// <summary>
            /// 機器交換画面で使用するSQL
            /// </summary>
            public static class Replace
            {
                /// <summary>SQL名：機器交換情報取得</summary>
                public const string GetReplaceMachineInfo = "GetReplaceMachineInfo";
                /// <summary>SQL名：機器情報更新</summary>
                public const string UpdateEquipmentInfo = "UpdateEquipmentInfo";
                /// <summary>SQL名：保全履歴機器更新</summary>
                public const string UpdateHistoryMachineInfoAll = "UpdateHistoryMachineInfoAll";
            }

            /// <summary>
            /// 機器検索画面で使用するSQL
            /// </summary>
            public static class Search
            {
                /// <summary>SQL名：機器検索一覧取得</summary>
                public const string GetSearchMachineList = "GetSearchMachineList";
            }

            /// <summary>
            /// 機器選択画面で使用するSQL
            /// </summary>
            public static class SelectMachine
            {
                /// <summary>SQL名：機器一覧情報取得</summary>
                public const string GetSelectMachineList = "GetSelectMachineList";
            }
        }

        /// <summary>
        /// フォーム、グループ、コントロールの親子関係を表現した場合の定数クラス
        /// </summary>
        private static class ConductInfo
        {
            /// <summary>
            /// 一覧画面
            /// </summary>
            public static class FormList
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 0;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// フィルター、非表示項目の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string HiddenItemId = "BODY_010_00_LST_0";
                    /// <summary>
                    /// 検索結果の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string SearchResult = "BODY_020_00_LST_0";
                }
                public static class Button
                {
                    /// <summary>点検情報登録</summary>
                    public const string NewInspection = "NewInspection";
                    /// <summary>故障情報登録</summary>
                    public const string NewFailure = "NewFailure";
                }

                /// <summary>
                /// 件名別長期計画・機器別長期計画の白丸「○」リンクから遷移してきた際の情報
                /// </summary>
                public static class ParamFromLongPlan
                {
                    /// <summary>タブ番号(遷移元で設定される値)</summary>
                    public const int TabNo = -1;
                    /// <summary>グローバル変数に格納する際のキー</summary>
                    public const string GlobalKey = "MakeScheduleFromLongPlan";
                }
            }

            /// <summary>
            /// 詳細画面
            /// </summary>
            public static class FormDetail
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 1;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 場所階層、職種の画面定義テーブルのコントロールID
                    /// </summary>
                    public const string StructureId = "BODY_030_00_LST_1";
                    /// <summary>
                    /// 件名情報、作業性格情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> DetailInfoIds { get; } = new[] { "BODY_010_00_LST_1", "BODY_020_00_LST_1", StructureId, "BODY_040_00_LST_1" }.ToList().AsReadOnly();
                    /// <summary>
                    /// 対象機器の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string MachineList = "BODY_050_00_LST_1";
                    /// <summary>
                    /// 故障情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> FailureInfoIds { get; } = new[] { "BODY_060_00_LST_1", "BODY_070_00_LST_1", "BODY_080_00_LST_1", "BODY_090_00_LST_1" }.ToList().AsReadOnly();
                    /// <summary>
                    /// 保全依頼情報タブ 依頼概要、依頼申請情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> RequestInfoIds { get; } = new[] { "BODY_120_00_LST_1", "BODY_130_00_LST_1", "BODY_140_00_LST_1", "BODY_150_00_LST_1", "BODY_160_00_LST_1", "BODY_170_00_LST_1" }.ToList().AsReadOnly();
                    /// <summary>
                    /// 保全計画情報タブ 保全計画概要の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string PlanId = "BODY_180_00_LST_1";
                    /// <summary>
                    /// 保全履歴情報タブ 保全履歴概要、作業時間情報、費用情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> HistoryInfoIds { get; } = new[] { "BODY_190_00_LST_1", "BODY_200_00_LST_1", "BODY_210_00_LST_1", "BODY_220_00_LST_1" }.ToList().AsReadOnly();
                    /// <summary>
                    /// 保全履歴情報(個別工場)タブ 保全履歴情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> HistoryIndividualInfoIds { get; } = new[] { "BODY_230_00_LST_1", "BODY_250_00_LST_1", "BODY_260_00_LST_1", "BODY_270_00_LST_1", "BODY_280_00_LST_1" }.ToList().AsReadOnly();
                    /// <summary>
                    /// 故障分析情報タブ 故障原因分析情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string FailureAnalyzeInfoId = "BODY_300_00_LST_1";
                    /// <summary>
                    /// 故障分析情報(個別工場)タブ 故障状況、故障原因、復旧処置、再発防止対策の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> FailureAnalyzeIndividualInfoIds { get; } = new[] { "BODY_320_00_LST_1", "BODY_330_00_LST_1", "BODY_340_00_LST_1", "BODY_350_00_LST_1", "BODY_360_00_LST_1", "BODY_370_00_LST_1", "BODY_380_00_LST_1" }.ToList().AsReadOnly();

                }
                /// <summary>
                /// ボタン
                /// </summary>
                public static class Button
                {
                    /// <summary>
                    /// 指示検収票出力のボタンコントロールID
                    /// </summary>
                    public const string AcceptanceSlipOutput = "AcceptanceSlipOutput";
                    /// <summary>
                    /// 依頼票出力のボタンコントロールID
                    /// </summary>
                    public const string RequestSlipOutput = "RequestSlipOutput";
                    /// <summary>
                    /// フォロー計画のボタンコントロールID
                    /// </summary>
                    public const string Follow = "Follow";
                    /// <summary>
                    /// 故障原因分析書出力のボタンコントロールID
                    /// </summary>
                    public const string FailureAnalyzeOutput = "FailureAnalyzeOutput";
                    /// <summary>
                    /// 故障原因分析書出力のボタンコントロールID
                    /// </summary>
                    public const string FailureAnalyzeIndividualOutput = "FailureAnalyzeIndividualOutput";
                }
            }

            /// <summary>
            /// 新規登録、修正画面
            /// </summary>
            public static class FormRegist
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 2;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 場所階層の画面定義テーブルのコントロールID
                    /// </summary>
                    public const string StructureId = "BODY_020_00_LST_2";
                    /// <summary>
                    /// 作業性格情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string WorkInfoId = "BODY_030_00_LST_2";
                    /// <summary>
                    /// 件名情報、作業性格情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> DetailInfoIds { get; } = new[] { "BODY_000_00_LST_2", "BODY_010_00_LST_2", StructureId, WorkInfoId }.ToList().AsReadOnly();
                    /// <summary>
                    /// 対象機器の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string MachineList = "BODY_040_00_LST_2";
                    /// <summary>
                    /// 故障情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> FailureInfoIds { get; } = new[] { "BODY_050_00_LST_2", "BODY_060_00_LST_2", "BODY_070_00_LST_2", "BODY_080_00_LST_2" }.ToList().AsReadOnly();
                    /// <summary>
                    /// 保全依頼情報タブ 依頼概要、依頼申請情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> RequestInfoIds { get; } = new[] { "BODY_100_00_LST_2", "BODY_110_00_LST_2", "BODY_120_00_LST_2", "BODY_130_00_LST_2", "BODY_140_00_LST_2", "BODY_150_00_LST_2" }.ToList().AsReadOnly();
                    /// <summary>
                    /// 保全計画情報タブ 保全計画概要の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string PlanId = "BODY_170_00_LST_2";
                    /// <summary>
                    /// 保全履歴情報タブ 保全履歴概要、作業時間情報、費用情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> HistoryInfoIds { get; } = new[] { "BODY_190_00_LST_2", "BODY_200_00_LST_2", "BODY_210_00_LST_2", "BODY_220_00_LST_2" }.ToList().AsReadOnly();
                    /// <summary>
                    /// 保全履歴情報(個別工場)タブ 保全履歴情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> HistoryIndividualInfoIds { get; } = new[] { "BODY_240_00_LST_2", "BODY_260_00_LST_2", "BODY_270_00_LST_2", "BODY_280_00_LST_2", "BODY_290_00_LST_2" }.ToList().AsReadOnly();
                    /// <summary>
                    /// 故障分析情報タブ 故障原因分析情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string FailureAnalyzeId = "BODY_310_00_LST_2";
                    /// <summary>
                    /// 故障分析情報(個別工場)タブ 故障状況、故障原因、復旧処置、再発防止対策の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> FailureAnalyzeIndividualInfoIds { get; } = new[] { "BODY_330_00_LST_2", "BODY_340_00_LST_2", "BODY_350_00_LST_2", "BODY_360_00_LST_2", "BODY_370_00_LST_2", "BODY_380_00_LST_2", "BODY_390_00_LST_2" }.ToList().AsReadOnly();
                    /// <summary>
                    /// 件名別長期計画・機器別長期計画の白丸「○」リンクから遷移してきた際に初期値を設定するコントロールID
                    /// </summary>
                    public static ReadOnlyCollection<string> MakeMaintenanceFromLongPlan { get; } = new[] { "BODY_010_00_LST_2", "BODY_020_00_LST_2",  "BODY_110_00_LST_2", "BODY_170_00_LST_2", "BODY_190_00_LST_2", "BODY_240_00_LST_2" }.ToList().AsReadOnly();
                }
                /// <summary>
                /// グループ番号
                /// </summary>
                public static class GroupNo
                {
                    /// <summary>件名情報</summary>
                    public const short SummaryInfo = 601;
                    /// <summary>作業性格情報</summary>
                    public const short WorkPersonalityInfo = 602;
                    /// <summary>故障情報</summary>
                    public const short FailureInfo = 604;
                    /// <summary>依頼情報タブ：依頼概要</summary>
                    public const short RequestInfo = 605;
                    /// <summary>依頼情報タブ：依頼申請情報</summary>
                    public const short RequestApplicationInfo = 606;
                    /// <summary>計画情報タブ：保全計画情報</summary>
                    public const short PlanInfo = 607;
                    /// <summary>履歴情報タブ：保全履歴概要</summary>
                    public const short HistoryInfo = 608;
                    /// <summary>履歴情報タブ：作業時間情報</summary>
                    public const short WorkTimeInfo = 609;
                    /// <summary>履歴情報タブ：費用情報</summary>
                    public const short CostInfo = 610;
                    /// <summary>履歴情報(個別工場)タブ：保全履歴情報</summary>
                    public const short HistoryIndividualInfo = 611;
                    /// <summary>故障分析情報タブ：故障原因分析情報</summary>
                    public const short FailureAnalyzeInfo = 612;
                    /// <summary>故障分析情報(個別工場)タブ：故障状況</summary>
                    public const short FailureStatusInfo = 613;
                    /// <summary>故障分析情報(個別工場)タブ：故障原因</summary>
                    public const short FailureCauseInfo = 614;
                    /// <summary>故障分析情報(個別工場)タブ：復旧処置</summary>
                    public const short RecoveryActionInfo = 615;
                    /// <summary>故障分析情報(個別工場)タブ：再発防止対策</summary>
                    public const short ImprovementMeasureInfo = 616;
                }
            }
            /// <summary>
            /// 機器交換画面
            /// </summary>
            public static class FormReplaceMachine
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 3;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 機器交換
                    /// </summary>
                    public static class Replace
                    {
                        /// <summary>
                        /// 現場機器情報の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public static ReadOnlyCollection<string> CurrentInfoIds { get; } = new[] { "BODY_000_00_LST_3", "BODY_010_00_LST_3" }.ToList().AsReadOnly();
                        /// <summary>
                        /// 代替機器情報の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public static ReadOnlyCollection<string> ReplaceInfoIds { get; } = new[] { "BODY_030_00_LST_3", "BODY_040_00_LST_3" }.ToList().AsReadOnly();

                    }
                    /// <summary>
                    /// 機器交換確認
                    /// </summary>
                    public static class Confirm
                    {
                        /// <summary>
                        /// 現場機器情報の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public static ReadOnlyCollection<string> CurrentInfoIds { get; } = new[] { "BODY_060_00_LST_3", "BODY_070_00_LST_3" }.ToList().AsReadOnly();
                        /// <summary>
                        /// 代替機器情報の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public static ReadOnlyCollection<string> ReplaceInfoIds { get; } = new[] { "BODY_080_00_LST_3", "BODY_090_00_LST_3" }.ToList().AsReadOnly();
                    }
                }
                /// <summary>
                /// グループ番号
                /// </summary>
                public static class GroupNo
                {
                    /// <summary>機器交換情報</summary>
                    public const short ReplaceMachineInfo = 630;
                    /// <summary>機器交換確認情報</summary>
                    public const short ConfirmMachineInfo = 631;
                }
            }
            /// <summary>
            /// 機器検索画面
            /// </summary>
            public static class FormSearchMachine
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 4;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 検索条件 場所階層の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string StructureCondition = "COND_010_00_LST_4";
                    /// <summary>
                    /// 検索条件 職種機種の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string JobCondition = "COND_020_00_LST_4";
                    /// <summary>
                    /// 検索結果の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string SearchResult = "BODY_050_00_LST_4";
                }
                /// <summary>
                /// グループ番号
                /// </summary>
                public static class GroupNo
                {
                    /// <summary>検索条件</summary>
                    public const short SearchCondition = 401;
                }
            }
            /// <summary>
            /// 機器選択画面
            /// </summary>
            public static class FormSelectMachine
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 5;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 検索条件 場所階層の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string StructureCondition = "COND_000_00_LST_5";
                    /// <summary>
                    /// 検索条件 職種機種の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string JobCondition = "COND_010_00_LST_5";
                    /// <summary>
                    /// 機器一覧の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string List = "BODY_050_00_LST_5";

                }
                /// <summary>
                /// グループ番号
                /// </summary>
                public static class GroupNo
                {
                    /// <summary>検索条件</summary>
                    public const short SearchCondition = 501;
                }
            }
        }

        /// <summary>
        /// ユーザ役割
        /// </summary>
        private static class UserRole
        {
            /// <summary>製造権限</summary>
            public const string Manufacturing = "10";
            /// <summary>保全権限</summary>
            public const string Maintenance = "20";
        }

        /// <summary>
        /// 保全活動区分
        /// </summary>
        private static class MaintenanceDivision
        {
            /// <summary>点検情報</summary>
            public const short Inspection = 1;
            /// <summary>故障情報</summary>
            public const short Failure = 2;
        }

        /// <summary>
        /// 系停止：なし
        /// </summary>
        private static class StopSystemDivision
        {
            /// <summary>連番</summary>
            public const short Seq = 1;
            /// <summary>データタイプ</summary>
            public const short DataType = 1;
            /// <summary>拡張データ</summary>
            public const string ExData = "0";
        }

        /// <summary>
        /// MQ分類：突発区分の必須フラグ
        /// </summary>
        private static class MqClassDivision
        {
            /// <summary>連番</summary>
            public const short Seq = 3;
            /// <summary>データタイプ</summary>
            public const short DataType = 2;
            /// <summary>拡張データ</summary>
            public const string ExData = "0";
        }

        /// <summary>
        /// MQ分類：作業性格分類コード
        /// </summary>
        private static class MqClassBudgetPersonalityDivision
        {
            /// <summary>連番</summary>
            public const short Seq = 2;
        }

        /// <summary>
        /// MQ分類：拡張データ識別コード
        /// </summary>
        private static class MqClassDiscriminationDivision
        {
            /// <summary>連番</summary>
            public const short Seq = 4;
        }

        /// <summary>
        /// 依頼番号採番パターン
        /// </summary>
        private static class RequestNumberingPattern
        {
            /// <summary>連番(アイテムマスタ拡張からの取得条件)</summary>
            public const short Seq = 1;
            /// <summary>採番パターン1</summary>
            public const string Pattern1 = "1";
            /// <summary>採番パターン2</summary>
            public const string Pattern2 = "2";
            /// <summary>採番パターン3</summary>
            public const string Pattern3 = "3";
            /// <summary>採番パターン4</summary>
            public const string Pattern4 = "4";
        }

        /// <summary>
        /// 新規登録画面に設定する保全活動件名ID
        /// </summary>
        private static long newSummaryId = -1;

        /// <summary>
        /// フォロー内容の入力可能文字数
        /// </summary>
        private static int followContentLength = 400;

        /// <summary>
        /// 保全履歴：個別工場表示フラグ
        /// </summary>
        private static class HistoryIndividualDivision
        {
            /// <summary>連番</summary>
            public const short Seq = 1;
            /// <summary>データタイプ</summary>
            public const short DataType = 2;
        }

        /// <summary>
        /// 故障分析：個別工場表示フラグ
        /// </summary>
        private static class FailureIndividualDivision
        {
            /// <summary>連番</summary>
            public const short Seq = 2;
            /// <summary>データタイプ</summary>
            public const short DataType = 2;
        }

        /// <summary>
        /// 個別工場表示フラグの値
        /// </summary>
        private static class IndividualDivision
        {
            /// <summary>個別工場表示対象外</summary>
            public const string Hide = "0";
            /// <summary>個別工場表示対象</summary>
            public const string Show = "1";
        }

        /// <summary>
        /// 呼出
        /// </summary>
        private static class CallDivision
        {
            /// <summary>連番</summary>
            public const short Seq = 1;
            /// <summary>データタイプ</summary>
            public const short DataType = 2;
        }

        /// <summary>
        /// 突発区分
        /// </summary>
        private static class SuddenDivision
        {
            /// <summary>連番</summary>
            public const short Seq = 1;
        }

        /// <summary>
        /// 管理基準
        /// </summary>
        private static class ManagementDivision
        {
            /// <summary>管理基準外</summary>
            public const string NotManagement = "0";
            /// <summary>管理基準</summary>
            public const string Management = "1";

            /// <summary>機器別管理基準から選択する場合、SQLをアンコメントするキー</summary>
            public const string UncommentKey = "SelectManagementStandards";
        }

        /// <summary>
        /// グローバル変数キー
        /// </summary>
        private static class GlobalKey
        {
            // グローバル変数のキー、機器検索画面で選択した機番ID
            public const string MA0001SelectMachineId = "MA0001_SelectMachineId";
            // グローバル変数のキー、構成グループID
            public const string MA0001GroupId = "MA0001_GroupId";
            // グローバル変数のキー、拡張データ
            public const string MA0001ExtensionData = "MA0001_ExtensionData";
            // グローバル変数のキー、構成ID
            public const string MA0001StructureId = "MA0001_StructureId";
            // グローバル変数のキー、職種ID
            public const string MA0001JobId = "MA0001_JobId";
        }

        /// <summary>
        /// 保全実績評価から保全活動への遷移時の検索条件(職種)に設定する文字列
        /// </summary>
        private static string jobAll = "All";

        /// <summary>
        /// 帳票情報
        /// </summary>
        private static class ReportInfo
        {
            // 処理対象帳票ID 依頼表
            public const string ReportIdRP0130 = "RP0130";
            // 処理対象帳票ID 予実算情報
            public const string ReportIdRP0150 = "RP0150";
            // 処理対象帳票ID 故障原因分析書
            public const string ReportIdRP0160 = "RP0160";
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_MA0001() : base()
        {
        }
        #endregion

        #region オーバーライドメソッド
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int InitImpl()
        {
            this.ResultList = new();
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsBack())
            {
                // 戻る
                return InitSearch();
            }

            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo:     // 一覧検索
                case ConductInfo.FormDetail.FormNo:   //詳細
                case ConductInfo.FormReplaceMachine.FormNo:  //機器交換
                    return InitSearch();
                case ConductInfo.FormRegist.FormNo:   //新規登録・修正
                    if (compareId.IsNew() || compareId.IsStartId(ConductInfo.FormDetail.Button.Follow))
                    {
                        //点検情報登録、故障情報登録、フォロー計画
                        return initNew(compareId);
                    }
                    return InitSearch();
                case ConductInfo.FormSearchMachine.FormNo:  //機器検索
                case ConductInfo.FormSelectMachine.FormNo:  //機器選択
                    if (!initSelectMachine())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                default:
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo:     // 一覧検索
                    if (!searchList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormDetail.FormNo:   //詳細
                case ConductInfo.FormRegist.FormNo:   //修正
                    if (!searchDetailList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormReplaceMachine.FormNo: //機器交換
                    if (!searchReplaceMachineList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormSearchMachine.FormNo:  //機器検索
                    if (!searchMachineList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormSelectMachine.FormNo:  //機器選択
                    if (!searchSelectMachine())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                default:
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ExecuteImpl()
        {
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsRegist())
            {
                // 登録の場合
                // 登録処理実行
                return Regist();
            }
            else if (compareId.IsDelete())
            {
                // 削除の場合
                // 削除処理実行
                return Delete();
            }
            else if (compareId.IsStartId("GetStructureIdList"))
            {
                // 拡張データから構成IDを取得する場合（保全実績評価から一覧画面へ遷移時）
                // 拡張データより構成IDを取得する処理実行
                return GetStructureIdList();
            }
            else if (compareId.IsStartId("GetIndividualFlg"))
            {
                // 個別工場表示フラグ取得の場合（ツリー選択ラベルにて工場変更時）
                // 個別工場表示フラグ取得処理実行
                return GetIndividualFlg();
            }
            else if (compareId.IsStartId("GetSelectMachineData"))
            {
                // 機器選択の場合（機器選択画面で選択されたデータを取得する）
                // 機器取得処理実行
                return GetSelectMachineData();
            }
            // この部分は到達不能なので、エラーを返す
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse

            // 登録ボタンが複数の画面に無い場合、分岐は不要
            // 処理を実行する画面Noの値により処理を分岐する
            switch (this.FormNo)
            {
                case ConductInfo.FormRegist.FormNo:
                    // 登録・更新処理
                    resultRegist = executeRegistEdit();
                    break;
                case ConductInfo.FormReplaceMachine.FormNo:
                    // 機器交換の更新処理
                    resultRegist = executeRegistReplace();
                    break;
                default:
                    // 処理が想定される場合は、分岐に条件を追加して処理を記載すること
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }
            // 登録処理結果によりエラー処理を行う
            if (!resultRegist)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    //「登録処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                }
                return ComConsts.RETURN_RESULT.NG;
            }
            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「登録処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200003 });
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            this.ResultList = new();

            //削除
            if (!deleteDetail())
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    //「削除処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911110001 });
                }
                return ComConsts.RETURN_RESULT.NG;
            }

            // 再検索処理
            this.NeedsTotalCntCheck = false;

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「削除処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911110001 });
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            // ファイルダウンロードの場合
            if (compareId.IsDownload())
            {
                // ダウンロード情報取得
                var info = TMQUtil.GetFileDownloadInfo(this.searchConditionDictionary, this.db, out bool isError);
                if (isError)
                {
                    // エラーの場合は終了
                    OutputFileDownloadError();
                    return ComConsts.RETURN_RESULT.NG;
                }
                // ファイルをダウンロード
                if (!OutputDownloadFile(info.FileName, info.FilePath))
                {
                    // エラーの場合は終了
                    return ComConsts.RETURN_RESULT.NG;
                }
                return ComConsts.RETURN_RESULT.OK;
            }

            int result = 0;
            this.ResultList = new();
            int reportFactoryId = 0;
            string reportId = "";
            Dictionary<int, List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData>> dicSelectKeyDataList = new Dictionary<int, List<SelectKeyData>>();

            //// 実装の際は、不要な帳票に対する分岐は削除して構いません

            switch (this.CtrlId)
            {
                case ConductInfo.FormDetail.Button.AcceptanceSlipOutput:
                    //指示検収票
                    if (!outputFiles())
                    {
                        // エラーの場合
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                    // 出力処理が完了しました。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, ComRes.ID.ID911120006 });
                    return ComConsts.RETURN_RESULT.OK;
                    break;
                case ConductInfo.FormDetail.Button.RequestSlipOutput:
                    //依頼票出力
                    reportId = ReportInfo.ReportIdRP0130;
                    // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                    // ユーザの本務工場取得
                    int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                    reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

                    // 帳票定義取得
                    // 出力帳票シート定義のリストを取得
                    var sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                        TMQUtil.ComReport.GetReportSheetDefine,
                        TMQUtil.ExcelPath,
                        new { FactoryId = reportFactoryId, ReportId = reportId },
                        db);
                    if (sheetDefineList == null)
                    {
                        // 取得できない場合、処理を戻す
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    // シート定義毎にループ
                    foreach (var sheetDefine in sheetDefineList)
                    {
                        // 検索条件フラグの場合はスキップする
                        if (sheetDefine.SearchConditionFlg == true)
                        {
                            continue;
                        }
                        Key keyInfo = getKeyInfoByTargetSqlParams(sheetDefine.TargetSqlParams);

                        // 帳票用選択キーデータ取得
                        // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                        List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport(
                            ConductInfo.FormDetail.ControlId.RequestInfoIds[0],     // 依頼情報のコントールID
                            keyInfo,                     // 設定したキー情報
                            this.resultInfoDictionary,
                            false);  // 画面データ

                        // シートNoをキーとして帳票用選択キーデータを保存する
                        dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                    }

                    //// 検索条件データ取得
                    TMQUtil.CommonOutputExcel(
                        reportFactoryId,             // 工場ID
                        this.PgmId,                  // プログラムID
                        dicSelectKeyDataList,        // シートごとのパラメータでの選択キー情報リスト
                        null,                        // 検索条件
                        reportId,                    // 帳票ID
                        1,                           // テンプレートID
                        1,                           // 出力パターンID
                        this.UserId,                 // ユーザID
                        this.LanguageId,             // 言語ID
                        this.conditionSheetLocationList,    // 場所階層構成IDリスト
                        this.conditionSheetJobList,         // 職種機種構成IDリスト
                        this.conditionSheetNameList,        // 検索条件項目名リスト
                        this.conditionSheetValueList,       // 検索条件設定値リスト
                        out string fileType,         // ファイルタイプ
                        out string fileName,         // ファイル名
                        out MemoryStream memStream,  // メモリストリーム
                        out string message,          // メッセージ
                        db);

                    // OUTPUTパラメータに設定
                    this.OutputFileType = fileType;
                    this.OutputFileName = fileName;
                    this.OutputStream = memStream;

                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                    // 出力処理が完了しました。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, ComRes.ID.ID911120006 });
                    return ComConsts.RETURN_RESULT.OK;
                    break;

                case "Report":
                    reportId = ReportInfo.ReportIdRP0150;
                    // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                    userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                    reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

                    // ページ情報取得
                    var pageInfo = GetPageInfo(
                        ConductInfo.FormList.ControlId.HiddenItemId,     // 一覧のコントールID
                        this.pageInfoList);          // ページ情報リスト

                    // 検索条件データ取得
                    getSearchConditionForReport(pageInfo, out dynamic searchCondition);
                    // 帳票定義取得
                    // 出力帳票シート定義のリストを取得
                    sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                        TMQUtil.ComReport.GetReportSheetDefine,
                        TMQUtil.ExcelPath,
                        new { FactoryId = reportFactoryId, ReportId = reportId },
                        db);
                    if (sheetDefineList == null)
                    {
                        // 取得できない場合、処理を戻す
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    // シート定義毎にループ
                    foreach (var sheetDefine in sheetDefineList)
                    {
                        // 検索条件フラグの場合はスキップする
                        if (sheetDefine.SearchConditionFlg == true)
                        {
                            continue;
                        }
                        Key keyInfo = getKeyInfoByTargetSqlParams(sheetDefine.TargetSqlParams);

                        // 帳票用選択キーデータ取得
                        // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                        List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport(
                            ConductInfo.FormList.ControlId.SearchResult,     // 一覧のコントールID
                            keyInfo,                     // 設定したキー情報
                            this.resultInfoDictionary);  // 画面データ

                        // シートNoをキーとして帳票用選択キーデータを保存する
                        dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                    }

                    TMQUtil.CommonOutputExcel(
                        reportFactoryId,             // 工場ID
                        this.PgmId,                  // プログラムID
                        dicSelectKeyDataList,        // シートごとのパラメータでの選択キー情報リスト
                        searchCondition,             // 検索条件
                        reportId,                    // 帳票ID
                        1,                           // テンプレートID
                        1,                           // 出力パターンID
                        this.UserId,                 // ユーザID
                        this.LanguageId,             // 言語ID
                        this.conditionSheetLocationList,    // 場所階層構成IDリスト
                        this.conditionSheetJobList,         // 職種機種構成IDリスト
                        this.conditionSheetNameList,        // 検索条件項目名リスト
                        this.conditionSheetValueList,       // 検索条件設定値リスト
                        out string fileType1,         // ファイルタイプ
                        out string fileName1,         // ファイル名
                        out MemoryStream memStream1,  // メモリストリーム
                        out string message1,          // メッセージ
                        db);

                    // OUTPUTパラメータに設定
                    this.OutputFileType = fileType1;
                    this.OutputFileName = fileName1;
                    this.OutputStream = memStream1;

                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                    // 出力処理が完了しました。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, ComRes.ID.ID911120006 });
                    return ComConsts.RETURN_RESULT.OK;
                    break;
                case ConductInfo.FormDetail.Button.FailureAnalyzeOutput:
                case ConductInfo.FormDetail.Button.FailureAnalyzeIndividualOutput:
                    //故障原因分析書出力
                    reportId = ReportInfo.ReportIdRP0160;
                    // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                    userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                    reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

                    // 帳票定義取得
                    // 出力帳票シート定義のリストを取得
                    sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                        TMQUtil.ComReport.GetReportSheetDefine,
                        TMQUtil.ExcelPath,
                        new { FactoryId = reportFactoryId, ReportId = reportId },
                        db);
                    if (sheetDefineList == null)
                    {
                        // 取得できない場合、処理を戻す
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    // シート定義毎にループ
                    foreach (var sheetDefine in sheetDefineList)
                    {
                        // 検索条件フラグの場合はスキップする
                        if (sheetDefine.SearchConditionFlg == true)
                        {
                            continue;
                        }
                        Key keyInfo = getKeyInfoByTargetSqlParams(sheetDefine.TargetSqlParams);

                        // 帳票用選択キーデータ取得
                        // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                        List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport(
                            ConductInfo.FormDetail.ControlId.DetailInfoIds[0],     // 依頼情報のコントールID
                            keyInfo,                     // 設定したキー情報
                            this.resultInfoDictionary,
                            false);  // 画面データ

                        // シートNoをキーとして帳票用選択キーデータを保存する
                        dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                    }

                    //// 検索条件データ取得
                    TMQUtil.CommonOutputExcel(
                        reportFactoryId,             // 工場ID
                        this.PgmId,                  // プログラムID
                        dicSelectKeyDataList,        // シートごとのパラメータでの選択キー情報リスト
                        null,                        // 検索条件
                        reportId,                    // 帳票ID
                        1,                           // テンプレートID
                        1,                           // 出力パターンID
                        this.UserId,                 // ユーザID
                        this.LanguageId,             // 言語ID
                        this.conditionSheetLocationList,    // 場所階層構成IDリスト
                        this.conditionSheetJobList,         // 職種機種構成IDリスト
                        this.conditionSheetNameList,        // 検索条件項目名リスト
                        this.conditionSheetValueList,       // 検索条件設定値リスト
                        out fileType,         // ファイルタイプ
                        out fileName,         // ファイル名
                        out memStream,  // メモリストリーム
                        out message,          // メッセージ
                        db);

                    // OUTPUTパラメータに設定
                    this.OutputFileType = fileType;
                    this.OutputFileName = fileName;
                    this.OutputStream = memStream;

                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                    // 出力処理が完了しました。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, ComRes.ID.ID911120006 });
                    return ComConsts.RETURN_RESULT.OK;
                    break;
                default:
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「コントロールIDが不正です。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID911100001 });

                    // エラーログ出力
                    writeErrorLog(this.MsgId);
                    return ComConsts.RETURN_RESULT.NG;
            }

            return result;

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
        protected override int ExcelPortDownloadImpl(ref string fileType, ref string fileName, ref MemoryStream ms, ref string resultMsg, ref string detailMsg)
        {
            // ExcelPortクラスの生成
            var excelPort = new TMQUtil.ComExcelPort(
                this.db, this.UserId, this.BelongingInfo, this.LanguageId, this.FormNo, this.searchConditionDictionary, this.messageResources);

            // ExcelPortテンプレートファイル情報初期化
            this.Status = CommonProcReturn.ProcStatus.Valid;
            if (!excelPort.InitializeExcelPortTemplateFile(out resultMsg, out detailMsg))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }
            else if (!string.IsNullOrEmpty(resultMsg))
            {
                // 正常終了時、詳細メッセージがセットされている場合、警告メッセージ
                this.Status = CommonProcReturn.ProcStatus.Warning;
            }

            //TODO: 個別データ検索処理
            IList<Dictionary<string, object>> dataList = null;
            if (dataList == null || dataList.Count == 0)
            {
                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「該当データがありません。」
                resultMsg = GetResMessage(ComRes.ID.ID941060001);
                return ComConsts.RETURN_RESULT.NG;
            }

            // 個別シート出力処理
            if (!excelPort.OutputExcelPortTemplateFile(dataList, out fileType, out fileName, out ms, out detailMsg))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 出力帳票シート定義の対象sqlパラメータよりキー情報を取得する
        /// </summary>
        /// <param name="targetSqlParams">対象sqlパラメータ</param>
        /// <returns>キー情報</returns>
        private Key getKeyInfoByTargetSqlParams(string targetSqlParams)
        {
            string keyParam1 = string.Empty;
            string keyParam2 = string.Empty;
            string keyParam3 = string.Empty;
            if (targetSqlParams != null && string.IsNullOrEmpty(targetSqlParams) == false)
            {
                string[] sqlParams = targetSqlParams.Split("|");
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    if (i == 0)
                    {
                        keyParam1 = sqlParams[i];
                    }
                    else if (i == 1)
                    {
                        keyParam2 = sqlParams[i];
                    }
                    if (i == 2)
                    {
                        keyParam3 = sqlParams[i];
                    }
                }
            }
            return new Key(keyParam1, keyParam2, keyParam3);
        }

        #endregion

    }
}