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
using Microsoft.AspNetCore.Http;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MA0001.BusinessLogicDataClass_MA0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;
using System.Reflection.PortableExecutable;

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
            /// <summary>SQL格納先サブディレクトリ名：共通</summary>
            public const string CommonDir = @"Common";

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
                /// <summary>SQL名：ユーザ所属工場取得</summary>
                public const string GetUserBelongingList = "UserBelonging_GetList";

                /// <summary>SQL名：一時テーブル作成：一覧取得用</summary>
                public const string CreateTempForGetList = "CreateTableTempGetMaintenanceList";
                /// <summary>SQL名：一時テーブル登録：一覧取得用</summary>
                public const string InsertTempForGetList = "InsertTempGetMaintenanceList";
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
                /// <summary>SQL名：点検：対象機器一覧取得(ExcelPort点検情報-対象機器の入力チェックで使用するSQL)</summary>
                public const string GetInspectionMachineListForExcelPort = "GetInspectionMachineListForExcelPort";
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
                /// <summary>SQL名：保全履歴機器部位更新</summary>
                public const string UpdateHistoryInspectionSite = "UpdateHistoryInspectionSite";
                /// <summary>SQL名：保全履歴機器部位件数取得</summary>
                public const string GetCountHistoryInspectionSite = "GetCountHistoryInspectionSiteInfo";
                /// <summary>SQL名：保全履歴点検内容件数取得</summary>
                public const string GetCountHistoryInspectionContent = "GetCountHistoryInspectionContentInfo";
                /// <summary>SQL名：保全履歴機器に紐づくデータ件数取得</summary>
                public const string GetCountRelationHistoryMachine = "GetCountRelationHistoryMachine";
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

            /// <summary>
            /// ExcelPortで使用するSQL
            /// </summary>
            public static class ExcelPort
            {
                /// <summary>SQL名：ExcelPort 保全活動取得</summary>
                public const string GetExcelPortMaintenance = "GetExcelPortMaintenance";
                /// <summary>SQL名：ExcelPort 故障情報取得</summary>
                public const string GetExcelPortHistoryFailure = "GetExcelPortHistoryFailure";
                /// <summary>SQL名：ExcelPort 点検情報（対象機器）取得</summary>
                public const string GetExcelPortInspectionMachine = "GetExcelPortInspectionMachine";
                /// <summary>SQL名：選択工場の個別工場フラグ取得</summary>
                public const string GetIndividualFlg = "GetIndividualFlg";

                /// <summary>SQL名：一時テーブル作成：保全活動取得用</summary>
                public const string CreateTableTempGetExcelPortMaintenance = "CreateTableTempGetExcelPortMaintenance";
                /// <summary>SQL名：一時テーブル登録：保全活動取得用</summary>
                public const string InsertTempGetExcelPortMaintenance = "InsertTempGetExcelPortMaintenance";
                /// <summary>SQL名：一時テーブル作成：故障情報取得用</summary>
                public const string CreateTableTempGetExcelPortHistoryFailure = "CreateTableTempGetExcelPortHistoryFailure";
                /// <summary>SQL名：一時テーブル登録：故障情報取得用</summary>
                public const string InsertTempGetExcelPortHistoryFailure = "InsertTempGetExcelPortHistoryFailure";
                /// <summary>SQL名：一時テーブル作成：点検情報（対象機器）取得用</summary>
                public const string CreateTableTempGetExcelPortInspectionMachine = "CreateTableTempGetExcelPortInspectionMachine";
                /// <summary>SQL名：一時テーブル登録：点検情報（対象機器）取得用</summary>
                public const string InsertTempGetExcelPortInspectionMachine = "InsertTempGetExcelPortInspectionMachine";

                /// <summary>SQL名：保全履歴フォロー有無更新</summary>
                public const string UpdateHistoryFollowFlg = "UpdateHistoryFollowFlg";
                /// <summary>SQL名：保全履歴ID取得</summary>
                public const string GetHistoryIdBySummaryId = "GetHistoryIdBySummaryId";
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
                    /// 非表示項目の画面項目定義テーブルのコントロールID
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
                    /// <summary>建材日報出力</summary>
                    public const string DailyReportOutput = "DailyReportOutput";
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
                    public static ReadOnlyCollection<string> MakeMaintenanceFromLongPlan { get; } = new[] { "BODY_010_00_LST_2", "BODY_020_00_LST_2", "BODY_110_00_LST_2", "BODY_170_00_LST_2", "BODY_190_00_LST_2", "BODY_240_00_LST_2" }.ToList().AsReadOnly();
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

            /// <summary>
            /// ExcelPort
            /// </summary>
            public static class ExcelPort
            {
                /// <summary>ExcelPortアップロード</summary>
                public const string ExcelPortUpload = "LIST_000_1";

                /// <summary>
                /// 列番号
                /// </summary>
                public static class ColumnNo
                {
                    /// <summary>
                    /// 保全活動シート
                    /// </summary>
                    public static class Maintenance
                    {
                        /// <summary>送信時処理</summary>
                        public const int Process = 3;
                        /// <summary>作業計画・実施内容</summary>
                        public const int ActivityDivision = 20;
                        /// <summary>作業計画・実施内容</summary>
                        public const int PlanImplementationContent = 22;
                        /// <summary>MQ分類</summary>
                        public const int MqClass = 25;
                        /// <summary>突発区分</summary>
                        public const int SuddenDivision = 33;
                        /// <summary>系停止時間(Hr)</summary>
                        public const int StopTime = 36;
                        /// <summary>依頼内容</summary>
                        public const int RequestContent = 42;
                        /// <summary>実施件名</summary>
                        public const int PlanSubject = 67;
                        /// <summary>着工予定日</summary>
                        public const int ExpectedConstructionDate = 69;
                        /// <summary>完了予定日</summary>
                        public const int ExpectedCompletionDate = 70;
                    }

                    /// <summary>
                    /// 保全活動_故障情報シート
                    /// </summary>
                    public static class HistoryFailure
                    {
                        /// <summary>送信時処理</summary>
                        public const int Process = 3;
                        /// <summary>保全部位</summary>
                        public const int MaintenanceSite = 35;
                        /// <summary>保全内容</summary>
                        public const int MaintenanceContent = 36;
                    }

                    /// <summary>
                    /// 保全活動_点検情報（対象機器）シート
                    /// </summary>
                    public static class InspectionMachine
                    {
                        /// <summary>保全活動件名</summary>
                        public const int Subject = 17;
                        /// <summary>機器</summary>
                        public const int Machine = 22;
                        /// <summary>保全部位</summary>
                        public const int InspectionSite = 38;
                        /// <summary>保全内容</summary>
                        public const int InspectionContent = 40;
                        /// <summary>フォロー有無</summary>
                        public const int FollowFlg = 42;
                        /// <summary>フォロー予定年月</summary>
                        public const int FollowPlanDate = 43;
                        /// <summary>フォロー内容</summary>
                        public const int FollowContent = 44;
                        /// <summary>フォロー完了日</summary>
                        public const int FollowCompletionDate = 45;
                        /// <summary>機器使用期間</summary>
                        public const int UseDays = 46;
                    }
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
            /// <summary>採番パターン5</summary>
            public const string Pattern5 = "5";
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
        /// ExcelPort保全活動完了区分
        /// </summary>
        private static class CompletionDivision
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
            // 処理対象帳票ID 建材日報出力
            public const string ReportIdRP0440 = "RP0440";
        }

        /// <summary>
        /// ExcelPortの場合、SQLをアンコメントするキー
        /// </summary>
        private static string uncommentExcelPort = "ExcelPort";

        /// <summary>
        /// 日報出力フラグ
        /// </summary>
        private static class DailyReportOutputFlg
        {
            /// <summary>連番</summary>
            public const short Seq = 6;
            /// <summary>拡張データ</summary>
            public const string ExData = "1";
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

                    //保全活動件名IDより工場IDを取得
                    ComDao.MaSummaryEntity param = GetFormDataByCtrlId<ComDao.MaSummaryEntity>(ConductInfo.FormDetail.ControlId.DetailInfoIds[0], true);
                    ComDao.MaSummaryEntity summary = new ComDao.MaSummaryEntity().GetEntity(param.SummaryId, this.db);
                    int factoryId = summary.LocationFactoryStructureId ?? -1;
                    // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する(ユーザの本務工場ではなく対象データの工場)
                    reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(factoryId, this.PgmId, reportId, this.db) ? factoryId : 0;

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
                        //Key keyInfo = getKeyInfoByTargetSqlParams(sheetDefine.TargetSqlParams);

                        // 帳票用選択キーデータ取得
                        // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                        //AEC shiraishi mod start 2023/09/03
                        List<SelectKeyData> selectKeyDataList = new List<SelectKeyData>();
                        SelectKeyData selectKeyData = new SelectKeyData();
                        selectKeyData.Key1 = param.SummaryId;
                        selectKeyDataList.Add(selectKeyData);
                        /*                        List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport(
                            ConductInfo.FormDetail.ControlId.RequestInfoIds[0],     // 依頼情報のコントールID
                            keyInfo,                     // 設定したキー情報
                            this.resultInfoDictionary,
                            false);  // 画面データ
                        */
                        //AEC shiraishi mod end 2023/09/03
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
                    int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
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
                case ConductInfo.FormList.Button.DailyReportOutput:
                    // 建材日報出力
                    reportId = ReportInfo.ReportIdRP0440;
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
                            ConductInfo.FormList.ControlId.SearchResult, // 一覧のコントールID
                            keyInfo,                     // 設定したキー情報
                            this.resultInfoDictionary);  // 画面データ

                        // シートNoをキーとして帳票用選択キーデータを保存する
                        dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                    }

                    // エクセル出力共通処理
                    TMQUtil.CommonOutputDailyReportExcel(
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

            IList<Dictionary<string, object>> dataList = null;
            if (excelPort.DownloadCondition.SheetNo == TMQUtil.ComExcelPort.SheetNo.Maintenance)
            {
                //保全活動シート データ取得
                dataList = getExcelPortMaintenanceData(excelPort);
            }
            else if (excelPort.DownloadCondition.SheetNo == TMQUtil.ComExcelPort.SheetNo.HistoryFailure)
            {
                //保全活動_故障情報シート データ取得
                dataList = getExcelPortHistoryFailureData(excelPort, ref resultMsg);
            }
            else if (excelPort.DownloadCondition.SheetNo == TMQUtil.ComExcelPort.SheetNo.InspectionMachine)
            {
                //保全活動_点検情報（対象機器）シート データ取得
                dataList = GetExcelPortInspectionMachineData(excelPort);
            }
            if (dataList == null)
            {
                //NULLでなく0件の場合はExcel出力する

                this.Status = CommonProcReturn.ProcStatus.Warning;
                return ComConsts.RETURN_RESULT.NG;
            }
            // 出力最大データ数チェック
            if (!excelPort.CheckDownloadMaxCnt(dataList.Count))
            {
                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「出力可能上限データ数を超えているため、ダウンロードできません。」
                resultMsg = GetResMessage(ComRes.ID.ID141120013);
                return ComConsts.RETURN_RESULT.NG;
            }

            // 個別シート出力処理
            if (!excelPort.OutputExcelPortTemplateFile(dataList, out fileType, out fileName, out ms, out detailMsg, ref resultMsg))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
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
        protected override int ExcelPortUploadImpl(IFormFile file, ref string fileType, ref string fileName, ref MemoryStream ms, ref string resultMsg, ref string detailMsg)
        {
            // ExcelPortクラスの生成
            var excelPort = new TMQUtil.ComExcelPort(
                this.db, this.UserId, this.BelongingInfo, this.LanguageId, this.FormNo, this.searchConditionDictionary, this.messageResources);

            // ExcelPortテンプレートファイル情報初期化
            this.Status = CommonProcReturn.ProcStatus.Valid;
            if (!excelPort.InitializeExcelPortTemplateFile(out resultMsg, out detailMsg, true, this.IndividualDictionary))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // ExcelPortアップロードデータの取得＆登録
            if (excelPort.UploadCondition.SheetNo == TMQUtil.ComExcelPort.SheetNo.Maintenance)
            {
                //保全活動シート
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, ConductInfo.ExcelPort.ExcelPortUpload,
                    out List<Dao.excelPortMaintenance> resultList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }
                //IListに変換する
                IList<Dao.excelPortMaintenance> list = resultList as IList<Dao.excelPortMaintenance>;
                //最下層の構成IDを取得して機能場所階層ID、職種機種階層IDにセットする
                TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.excelPortMaintenance>(ref list, new List<StructureType> { StructureType.Location, StructureType.Job }, true);
                resultList = list.ToList();

                // ユーザ役割の取得
                Dao.userRole role = new Dao.userRole();
                setUserRole<Dao.userRole>(role);

                // エラー情報リスト
                List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();
                // チェック処理
                if (!checkExcelPortRegistMaintenance(ref resultList, ref errorInfoList, role))
                {
                    if (errorInfoList.Count > 0)
                    {
                        // エラー情報シートへ設定
                        excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    }
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 登録処理
                if (!executeExcelPortRegistMaintenance(resultList, role))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else if (excelPort.UploadCondition.SheetNo == TMQUtil.ComExcelPort.SheetNo.HistoryFailure)
            {
                //保全活動_故障情報シート
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, ConductInfo.ExcelPort.ExcelPortUpload,
                    out List<Dao.excelPortHistoryFailure> resultList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }
                //IListに変換する
                IList<Dao.excelPortHistoryFailure> list = resultList as IList<Dao.excelPortHistoryFailure>;
                //最下層の構成IDを取得して原因性格IDにセットする
                TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.excelPortHistoryFailure>(ref list, new List<StructureType> { StructureType.FailureCause }, true);
                resultList = list.ToList();

                // ユーザ役割の取得
                Dao.userRole role = new Dao.userRole();
                setUserRole<Dao.userRole>(role);

                // エラー情報リスト
                List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();
                // チェック処理
                if (!checkExcelPortRegistHistoryFailure(ref resultList, ref errorInfoList, role))
                {
                    if (errorInfoList.Count > 0)
                    {
                        // エラー情報シートへ設定
                        excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    }
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 登録処理
                if (!executeExcelPortRegistHistoryFailure(resultList, role))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else if (excelPort.UploadCondition.SheetNo == TMQUtil.ComExcelPort.SheetNo.InspectionMachine)
            {
                //保全活動_点検情報（対象機器）シート
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, ConductInfo.ExcelPort.ExcelPortUpload,
                    out List<Dao.excelPortInspectionMachine> resultList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // ユーザ役割の取得
                Dao.userRole role = new Dao.userRole();
                setUserRole<Dao.userRole>(role);

                // エラー情報リスト
                List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();
                // チェック処理(機器の重複チェックは登録時に行う)
                checkExcelPortRegistInspectionMachine(ref resultList, ref errorInfoList, role);

                // 登録処理
                if (!executeExcelPortRegistInspectionMachine(resultList, ref errorInfoList, role))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }
            }

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 年度開始月を取得する処理
        /// </summary>
        /// <param name="factoryId">工場ID 省略時はユーザの本務工場</param>
        /// <returns>年度開始月</returns>
        private int getYearStartMonth(int? factoryId = null)
        {
            int startMonth;
            if (factoryId == null)
            {
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                startMonth = TMQUtil.GetYearStartMonth(this.db, userFactoryId);
            }
            else
            {
                startMonth = TMQUtil.GetYearStartMonth(this.db, factoryId ?? -1);
            }
            return startMonth;
        }

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

        /// <summary>
        /// 活動区分IDにより点検or故障を判定
        /// </summary>
        /// <param name="activityDivision">活動区分ID</param>
        /// <returns>点検の場合True</returns>
        private bool isInspection(int? activityDivision)
        {
            return activityDivision == MaintenanceDivision.Inspection;
        }
        /// <summary>
        /// 保全活動シートのデータ取得
        /// </summary>
        /// <param name="excelPort">ExcelPortクラス</param>
        /// <returns>取得データ</returns>
        private IList<Dictionary<string, object>> getExcelPortMaintenanceData(TMQUtil.ComExcelPort excelPort)
        {
            //検索条件で指定された条件をSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = getExcelPortUnCommentList(excelPort.DownloadCondition);

            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
            // 翻訳用一時テーブル登録
            // 翻訳する構成グループのリスト
            var structuregroupList = new List<GroupId>
            {
                GroupId.MqClass, GroupId.StopSystem, GroupId.Sudden, GroupId.BudgetManagement, GroupId.BudgetPersonality,
                GroupId.Season, GroupId.DiscoveryMethods, GroupId.ActualResult, GroupId.Phenomenon, GroupId.FailureCause,
                GroupId.TreatmentMeasure, GroupId.Progress, GroupId.Location, GroupId.FailureCausePersonality,
                GroupId.SiteMaster, GroupId.InspectionDetails, GroupId.RepairCostClass, GroupId.ChangeManagement,
                GroupId.EnvSafetyManagement, GroupId.Urgency, GroupId.RequestDepartmentClerk, GroupId.ConstructionDivision,
                GroupId.Responsibility, GroupId.EffectProduction, GroupId.EffectQuality, GroupId.ActivityDivision,
                GroupId.WorkFailureDivision, GroupId.Call, GroupId.CheckBoxItemDivision, GroupId.MaintenanceDepartmentClerk
            };
            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList); // 各グループ
            // 職種は職種階層のみなので別で指定
            listPf.GetInsertLayerOnly(GroupId.Job, (int)Const.MsStructure.StructureLayerNo.Job.Job, true);
            // 機能で作成するテーブル
            listPf.AddTempTable(SqlName.SubDir, SqlName.ExcelPort.CreateTableTempGetExcelPortMaintenance, SqlName.ExcelPort.InsertTempGetExcelPortMaintenance);
            listPf.RegistTempTable(); // 登録

            //言語
            excelPort.DownloadCondition.LanguageId = this.LanguageId;

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.ExcelPort.GetExcelPortMaintenance, out string baseSql, listUnComment);

            // 一覧検索SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, null);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY");
            selectSql.AppendLine("occurrence_date desc");
            selectSql.AppendLine(",summary_id desc");

            // 一覧検索実行
            IList<Dao.excelPortMaintenance> results = db.GetListByDataClass<Dao.excelPortMaintenance>(selectSql.ToString(), excelPort.DownloadCondition);
            if (results == null)
            {
                //NULLでなく0件の場合はExcel出力する
                return null;
            }
            if (results.Count == 0)
            {
                results.Add(new Dao.excelPortMaintenance());
            }
            //保全履歴情報の表示切替フラグを設定
            results[0].ControlFlag = getIndividualFlg(excelPort, HistoryIndividualDivision.Seq);
            // ユーザ役割の設定（ExcelPortの表示列の設定に使用）
            setUserRole<Dao.excelPortMaintenance>(results[0]);

            // Dicitionalyに変換
            IList<Dictionary<string, object>> dataList = ComUtil.ConvertClassToDictionary<Dao.excelPortMaintenance>(results);
            return dataList;
        }

        /// <summary>
        /// 保全活動_故障情報シートのデータ取得
        /// </summary>
        /// <param name="excelPort">ExcelPortクラス</param>
        /// <param name="resultMsg">結果メッセージ</param>
        /// <returns>取得データ</returns>
        private IList<Dictionary<string, object>> getExcelPortHistoryFailureData(TMQUtil.ComExcelPort excelPort, ref string resultMsg)
        {
            Dao.userRole role = new Dao.userRole();
            // ユーザ役割の取得
            setUserRole<Dao.userRole>(role);
            if (!role.Maintenance)
            {
                //保全権限が無い場合、データは出力しない
                // 「権限がありません。」
                resultMsg = GetResMessage(ComRes.ID.ID941090002);
                return null;
            }

            //検索条件で指定された条件をSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = getExcelPortUnCommentList(excelPort.DownloadCondition);
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
            // 翻訳用一時テーブル登録
            // 翻訳する構成グループのリスト
            var structuregroupList = new List<GroupId>
            {
                GroupId.Location, GroupId.Job, GroupId.FailureCausePersonality, GroupId.MachineLevel, GroupId.Conservation, GroupId.Importance, GroupId.Phenomenon,
                GroupId.FailureCause, GroupId.TreatmentMeasure, GroupId.FailureAnalysis, GroupId.FailurePersonalityFactor, GroupId.FailurePersonalityClass,
                GroupId.TreatmentStatus, GroupId.NecessityMeasure, GroupId.MeasureClass1, GroupId.MeasureClass2, GroupId.CheckBoxItemDivision
            };
            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            // 機能で作成するテーブル
            listPf.AddTempTable(SqlName.SubDir, SqlName.ExcelPort.CreateTableTempGetExcelPortHistoryFailure, SqlName.ExcelPort.InsertTempGetExcelPortHistoryFailure);
            listPf.RegistTempTable(); // 登録

            //言語
            excelPort.DownloadCondition.LanguageId = this.LanguageId;

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.ExcelPort.GetExcelPortHistoryFailure, out string baseSql, listUnComment);

            // 一覧検索SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, null);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY");
            selectSql.AppendLine("subject");

            // 一覧検索実行
            IList<Dao.excelPortHistoryFailure> results = db.GetListByDataClass<Dao.excelPortHistoryFailure>(selectSql.ToString(), excelPort.DownloadCondition);
            if (results == null)
            {
                //NULLでなく0件の場合はExcel出力する
                return null;
            }
            if (results.Count == 0)
            {
                results.Add(new Dao.excelPortHistoryFailure());
            }
            //故障情報の表示切替フラグを設定
            results[0].ControlFlag = getIndividualFlg(excelPort, FailureIndividualDivision.Seq);

            // 原因性格IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.excelPortHistoryFailure>(ref results, new List<StructureType> { StructureType.FailureCause }, this.db, this.LanguageId);

            // Dicitionalyに変換
            IList<Dictionary<string, object>> dataList = ComUtil.ConvertClassToDictionary<Dao.excelPortHistoryFailure>(results);
            return dataList;
        }

        /// <summary>
        /// 保全活動_点検情報（対象機器）シートのデータ取得
        /// </summary>
        /// <param name="excelPort">ExcelPortクラス</param>
        /// <returns>取得データ</returns>
        private IList<Dictionary<string, object>> GetExcelPortInspectionMachineData(TMQUtil.ComExcelPort excelPort)
        {
            //検索条件で指定された条件をSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = getExcelPortUnCommentList(excelPort.DownloadCondition);

            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
            // 翻訳用一時テーブル登録
            // 翻訳する構成グループのリスト
            var structuregroupList = new List<GroupId>
            {
                GroupId.Location, GroupId.Job, GroupId.MachineLevel, GroupId.Conservation, GroupId.Importance, GroupId.SiteMaster, GroupId.InspectionDetails, GroupId.CheckBoxItemDivision
            };
            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            // 機能で作成するテーブル
            listPf.AddTempTable(SqlName.SubDir, SqlName.ExcelPort.CreateTableTempGetExcelPortInspectionMachine, SqlName.ExcelPort.InsertTempGetExcelPortInspectionMachine);
            listPf.RegistTempTable(); // 登録

            //言語
            excelPort.DownloadCondition.LanguageId = this.LanguageId;

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.ExcelPort.GetExcelPortInspectionMachine, out string baseSql, listUnComment);

            // 一覧検索SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, null);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY");
            selectSql.AppendLine("subject");

            // 一覧検索実行
            IList<Dao.excelPortInspectionMachine> results = db.GetListByDataClass<Dao.excelPortInspectionMachine>(selectSql.ToString(), excelPort.DownloadCondition);
            if (results == null)
            {
                //NULLでなく0件の場合はExcel出力する
                return null;
            }

            // Dicitionalyに変換
            IList<Dictionary<string, object>> dataList = ComUtil.ConvertClassToDictionary<Dao.excelPortInspectionMachine>(results);
            return dataList;
        }

        /// <summary>
        /// 選択工場が一般工場、個別工場か取得し保全履歴情報・故障分析情報の表示切替フラグを取得
        /// </summary>
        /// <param name="excelPort">ExcelPortクラス</param>
        /// <returns>一般工場のみの場合0、個別工場のみの場合1、混在する場合NULL</returns>
        private string getIndividualFlg(TMQUtil.ComExcelPort excelPort, int sequenceNo)
        {
            //選択工場IDをカンマ区切りの文字列で取得
            string factoryIdList = string.Join(',', excelPort.TargetLocationInfoList.Select(x => x.FactoryId).Distinct());
            //選択工場が一般工場のみまたは個別工場のみの場合1件、混在している場合2件取得
            List<string> list = TMQUtil.SqlExecuteClass.SelectList<string>(SqlName.ExcelPort.GetIndividualFlg, SqlName.SubDir, new { FactoryIdList = factoryIdList, SequenceNo = sequenceNo }, this.db);
            if (list == null || list.Count != 1)
            {
                //一般工場と個別工場が混在する場合、表示切替は無し
                return null;
            }
            //一般工場（NULL、0）または個別工場（1）
            string value = list[0];
            return string.IsNullOrEmpty(value) ? IndividualDivision.Hide : value;
        }

        /// <summary>
        /// 検索条件で指定された条件をSQLの検索条件に含めるので、メンバ名を取得
        /// </summary>
        /// <param name="condition">ダウンロード検索条件</param>
        /// <returns>アンコメントリスト</returns>
        private List<string> getExcelPortUnCommentList(TMQUtil.ExcelPortDownloadCondition condition)
        {
            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = ComUtil.GetNotNullNameByClass<TMQUtil.ExcelPortDownloadCondition>(condition);
            // 完了区分のアイテム情報を取得
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = (int)Const.MsStructure.GroupId.CompletionDivision;
            //連番
            param.Seq = CompletionDivision.Seq;
            List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            //構成IDより拡張データを取得
            int extensionData = list.Where(x => x.StructureId == condition.CompletionDivision).Select(x => Convert.ToInt32(x.ExData)).FirstOrDefault();
            switch (extensionData)
            {
                case (int)Const.MsStructure.StructureId.CompletionDivision.Incomplete: //未完了
                    listUnComment.Add(Const.MsStructure.StructureId.CompletionDivision.Incomplete.ToString());
                    break;
                case (int)Const.MsStructure.StructureId.CompletionDivision.Completion: //完了
                    listUnComment.Add(Const.MsStructure.StructureId.CompletionDivision.Completion.ToString());
                    break;
            }
            return listUnComment;
        }

        /// <summary>
        /// ExcelPort保全活動チェック処理
        /// </summary>
        /// <param name="resultList">ExcelPortアップロードデータ</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <param name="role">ユーザ役割</param>
        /// <returns>エラーがある場合False</returns>
        private bool checkExcelPortRegistMaintenance(ref List<Dao.excelPortMaintenance> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dao.userRole role)
        {
            // 入力チェック
            bool errFlg = true;
            foreach (Dao.excelPortMaintenance result in resultList)
            {
                // 送信時処理IDが設定されているもののみ
                if (result.ProcessId == null)
                {
                    continue;
                }
                if (result.ProcessId == Const.SendProcessId.Regist || result.ProcessId == Const.SendProcessId.Update)
                {
                    //登録・更新

                    if (result.ProcessId == Const.SendProcessId.Update)
                    {
                        //更新時、件名区分が変更されている場合エラー
                        ComDao.MaSummaryEntity summary = new ComDao.MaSummaryEntity().GetEntity(result.SummaryId ?? -1, this.db);
                        if (summary.ActivityDivision != result.ActivityDivision)
                        {
                            // 件名区分は変更できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.Maintenance.ActivityDivision, GetResMessage("111090073"), GetResMessage(ComRes.ID.ID141090005), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                    }

                    //保全計画情報、保全履歴情報、保全履歴情報（個別工場）、故障情報、故障分析情報、故障分析情報（個別工場）のいずれかの項目に入力がある場合、件名情報の「作業計画・実施内容」は必須
                    if (role.Maintenance && (Convert.ToBoolean(result.PlanInputFlg) || Convert.ToBoolean(result.HistoryInputFlg)) && result.PlanImplementationContent == null)
                    {
                        // 入力してください。
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.Maintenance.PlanImplementationContent, GetResMessage("111110014"), GetResMessage(ComRes.ID.ID941220009), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                        errFlg = false;
                    }

                    //保全履歴情報または保全履歴情報（個別工場）の完了日に入力がある場合、「MQ分類」は必須
                    if (role.Maintenance && (result.CompletionDate != null || result.CompletionDateIndividual != null) && result.MqClassStructureId == null)
                    {
                        // 入力してください。
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.Maintenance.MqClass, GetResMessage("111040002"), GetResMessage(ComRes.ID.ID941220009), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                        errFlg = false;
                    }

                    //保全依頼情報に対する入力が行われている場合、「依頼内容」は必須
                    if (role.Manufacturing && Convert.ToBoolean(result.RequestInputFlg) && result.RequestContent == null)
                    {
                        // 入力してください。
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.Maintenance.RequestContent, GetResMessage("111020001"), GetResMessage(ComRes.ID.ID941220009), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                        errFlg = false;
                    }
                    //保全計画情報に対する入力が行われている場合、「実施件名」「着工予定日」「完了予定日」は必須
/*                    if (role.Maintenance && Convert.ToBoolean(result.PlanInputFlg))
                    {
                        if (result.PlanSubject == null)
                        {
                            // 入力してください。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.Maintenance.PlanSubject, GetResMessage("111120025"), GetResMessage(ComRes.ID.ID941220009), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                        if (result.ExpectedConstructionDate == null)
                        {
                            // 入力してください。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.Maintenance.ExpectedConstructionDate, GetResMessage("111170003"), GetResMessage(ComRes.ID.ID941220009), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                        if (result.ExpectedCompletionDate == null)
                        {
                            // 入力してください。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.Maintenance.ExpectedCompletionDate, GetResMessage("111060010"), GetResMessage(ComRes.ID.ID941220009), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                    }
*/
                    //MQ分類が「設備工事」「撤去工事」以外の場合、「突発区分」は必須
                    if (result.MqClassStructureId != null)
                    {
                        //「設備工事」「撤去工事」の構成IDを取得
                        string[] structureIds = setMqNotRequiredStructureId().Split(',');
                        if (!structureIds.Contains(result.MqClassStructureId.ToString()) && result.SuddenDivisionStructureId == null)
                        {
                            // 入力してください。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.Maintenance.SuddenDivision, GetResMessage("111200001"), GetResMessage(ComRes.ID.ID941220009), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                    }

                    //系停止「なし」が選択されていて、系停止時間に「0 or 空白」でない値が設定されていた場合、エラー
                    if (result.StopSystemStructureId != null)
                    {
                        //構成アイテムを取得するパラメータ設定
                        TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                        //構成グループID
                        param.StructureGroupId = (int)Const.MsStructure.GroupId.StopSystem;
                        //連番
                        param.Seq = StopSystemDivision.Seq;
                        //拡張データ
                        param.ExData = StopSystemDivision.ExData;

                        //系停止「なし」の構成ID取得
                        List<TMQUtil.StructureItemEx.StructureItemExInfo> stopSystemList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
                        if (result.StopSystemStructureId == stopSystemList[0].StructureId && result.StopTime != null && result.StopTime != 0)
                        {
                            // 系停止「なし」の場合、系停止時間は設定できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.Maintenance.StopTime, GetResMessage("111090033"), GetResMessage(ComRes.ID.ID141090004), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                    }

                }
                else if (result.ProcessId == Const.SendProcessId.Delete)
                {
                    //削除

                    if (!role.Maintenance)
                    {
                        //保全権限が含まれず、進捗状況が「保全受付」「完了済」の場合、エラー
                        //「保全受付」：ma_summary.completion_dateがNULLかつma_history.construction_personnel_idがNULLでない
                        //「完了済」：ma_summary.completion_dateがNULLでない

                        //保全履歴の情報を取得
                        Dao.historyInfo historyInfo = TMQUtil.SqlExecuteClass.SelectEntity<Dao.historyInfo>(SqlName.Detail.GetHistoryInfo, SqlName.SubDir, result, this.db);
                        if (historyInfo.CompletionDate != null || (historyInfo.CompletionDate == null && historyInfo.ConstructionPersonnelId != null))
                        {
                            // 保全受付または完了済のため、削除できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.Maintenance.Process, GetResMessage("111150019"), GetResMessage(ComRes.ID.ID141300008), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }

                    }
                }
            }

            return errFlg;
        }

        /// <summary>
        /// ExcelPort保全活動_故障情報チェック処理
        /// </summary>
        /// <param name="resultList">ExcelPortアップロードデータ</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <param name="role">ユーザ役割</param>
        /// <returns>エラーがある場合False</returns>
        private bool checkExcelPortRegistHistoryFailure(ref List<Dao.excelPortHistoryFailure> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dao.userRole role)
        {
            // 入力チェック
            bool errFlg = true;
            foreach (Dao.excelPortHistoryFailure result in resultList)
            {
                // 送信時処理IDが設定されているもののみ
                if (result.ProcessId == null)
                {
                    continue;
                }
                if (result.ProcessId == Const.SendProcessId.Regist)
                {
                    //登録（Excelで登録は選択不可だが念のためチェックする）
                    // 新規追加はできません。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.HistoryFailure.Process, GetResMessage("111150019"), GetResMessage(ComRes.ID.ID141120015), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                    errFlg = false;
                }
                else if (result.ProcessId == Const.SendProcessId.Update || result.ProcessId == Const.SendProcessId.Delete)
                {
                    //更新・削除
                    if (!role.Maintenance)
                    {
                        //保全権限が無い場合
                        // 権限が無いため変更できません。
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.HistoryFailure.Process, GetResMessage("111150019"), GetResMessage(ComRes.ID.ID141090006), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                        errFlg = false;
                    }
                    else if (result.ProcessId == Const.SendProcessId.Update)
                    {
                        //機器の入力がある場合、「保全部位」は必須
                        if (result.MachineId != null && result.MaintenanceSite == null)
                        {
                            // 入力してください。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.HistoryFailure.MaintenanceSite, GetResMessage("111300003"), GetResMessage(ComRes.ID.ID941220009), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                        //機器の入力がある場合、「保全内容」は必須
                        if (result.MachineId != null && result.MaintenanceContent == null)
                        {
                            // 入力してください。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.HistoryFailure.MaintenanceContent, GetResMessage("111300023"), GetResMessage(ComRes.ID.ID941220009), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                    }
                }
            }

            return errFlg;
        }

        /// <summary>
        /// ExcelPort保全活動_点検情報（対象機器）チェック処理
        /// </summary>
        /// <param name="resultList">ExcelPortアップロードデータ</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <param name="role">ユーザ役割</param>
        /// <returns>エラーがある場合False</returns>
        private bool checkExcelPortRegistInspectionMachine(ref List<Dao.excelPortInspectionMachine> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dao.userRole role)
        {
            // 入力チェック
            bool errFlg = true;

            foreach (Dao.excelPortInspectionMachine result in resultList)
            {
                // 送信時処理IDが設定されているもののみ
                if (result.ProcessId == null)
                {
                    continue;
                }
                if (result.ProcessId == Const.SendProcessId.Regist)
                {
                    //登録

                    //保全権限が無く「フォロー有無」「フォロー予定年月」「フォロー内容」「フォロー完了日」に対する入力が行われている場合、エラー
                    if (!role.Maintenance)
                    {
                        if (result.FollowFlg != null)
                        {
                            // 権限が無いため変更できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.FollowFlg, GetResMessage("111280026"), GetResMessage(ComRes.ID.ID141090006), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                        if (result.FollowPlanDate != null)
                        {
                            // 権限が無いため変更できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.FollowPlanDate, GetResMessage("111280028"), GetResMessage(ComRes.ID.ID141090006), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                        if (result.FollowContent != null)
                        {
                            // 権限が無いため変更できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.FollowContent, GetResMessage("111280009"), GetResMessage(ComRes.ID.ID141090006), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                        if (result.FollowCompletionDate != null)
                        {
                            // 権限が無いため変更できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.FollowCompletionDate, GetResMessage("111280029"), GetResMessage(ComRes.ID.ID141090006), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                    }

                    // 機器使用期間の入力チェック
                    if (!checkUseDaysForExcelport(result, resultList, ref errorInfoList))
                    {
                        errFlg = false;
                    }
                }
                else if (result.ProcessId == Const.SendProcessId.Update)
                {
                    //更新

                    if (result.OldSummaryId != result.SummaryId)
                    {
                        //保全活動件名は変更できません。
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.Subject, GetResMessage("111300033"), GetResMessage(ComRes.ID.ID141300009), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                        errFlg = false;
                    }

                    //変更前の保全履歴点検内容を取得
                    ComDao.MaHistoryInspectionContentEntity content = new ComDao.MaHistoryInspectionContentEntity().GetEntity(result.HistoryInspectionContentId ?? -1, this.db);

                    //保全権限が無く「フォロー有無」「フォロー予定年月」「フォロー内容」「フォロー完了日」に対する変更が行われている場合、エラー
                    if (!role.Maintenance)
                    {
                        if ((result.FollowFlg ?? 0) != Convert.ToInt32(content.FollowFlg ?? false))
                        {
                            // 権限が無いため変更できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.FollowFlg, GetResMessage("111280026"), GetResMessage(ComRes.ID.ID141090006), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                        if (result.FollowPlanDate != content.FollowPlanDate)
                        {
                            // 権限が無いため変更できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.FollowPlanDate, GetResMessage("111280028"), GetResMessage(ComRes.ID.ID141090006), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                        if (result.FollowContent != content.FollowContent)
                        {
                            // 権限が無いため変更できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.FollowContent, GetResMessage("111280009"), GetResMessage(ComRes.ID.ID141090006), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                        if (result.FollowCompletionDate != content.FollowCompletionDate)
                        {
                            // 権限が無いため変更できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.FollowCompletionDate, GetResMessage("111280029"), GetResMessage(ComRes.ID.ID141090006), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = false;
                        }
                    }

                    // 機器使用期間の入力チェック
                    if (!checkUseDaysForExcelport(result, resultList, ref errorInfoList))
                    {
                        errFlg = false;
                    }
                }
            }

            return errFlg;
        }

        /// <summary>
        /// ExcelPort点検情報(対象機器)のアップロード時に同一機器には同一の機器使用期間を入力させるための入力チェック
        /// </summary>
        /// <param name="result">チェック対象レコード</param>
        /// <param name="resultList">送信時処理が設定されているレコード</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool checkUseDaysForExcelport(Dao.excelPortInspectionMachine result, List<Dao.excelPortInspectionMachine> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            // バックエンドに渡ってきたデータより
            // 自身のレコードと保全活動件名ID、機番IDが同一で、異なる機器使用期間が入力されているデータの件数を取得(削除対象行は除く)
            int differenceUseDaysCnt = resultList.Where(x => x.SummaryId == result.SummaryId &&
                                                    x.MachineId == result.MachineId &&
                                                    x.UsedDaysMachine != result.UsedDaysMachine &&
                                                    x.ProcessId != Const.SendProcessId.Delete).ToList().Count();

            // 件数が1件でもある場合は同一機器に対して全て同一の機器使用期間が入力されていないということなのでエラーとする
            if (differenceUseDaysCnt > 0)
            {
                // 同一機器の機器使用期間にはすべて同じ値を入力して下さい。
                errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.UseDays, GetResMessage("111070031"), GetResMessage(ComRes.ID.ID141200010), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                return false;
            }

            return true;
        }

        /// <summary>
        /// ExcelPort点検情報(対象機器)のアップロード時に同一機器には同一の機器使用期間を入力させるための入力チェック
        /// ※DBに検索をかけるチェック
        /// </summary>
        /// <param name="result">チェック対象レコード</param>
        /// <param name="resultList">送信時処理が設定されているレコード</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool checkUseDaysForExcelportExistsDB(Dao.excelPortInspectionMachine result, List<Dao.excelPortInspectionMachine> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            // 機器使用期間の入力チェックで使用するSQL
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetInspectionMachineListForExcelPort, out string checkSqlForUseDays))
            {
                return false;
            }

            // ①自身のレコードと保全活動件名ID、機番IDが同一で、バックエンドに渡ってきた保全履歴点検内容IDリストを作成(削除対象行は除く)
            List<long?> contentIdList = resultList.Where(x => x.SummaryId == result.SummaryId &&
                                                        (x.MachineId == result.MachineId || x.MachineIdBefore == result.MachineId) &&
                                                         x.ProcessId != Const.SendProcessId.Delete &&
                                                         x.HistoryInspectionContentId != null)
                                                         .Select(x => x.HistoryInspectionContentId).ToList();

            // ② ①で作成した保全履歴点検内容ID以外のデータ(送信時処理が設定されていないデータ)をDBより取得する
            IList<Dao.detailMachine> results = db.GetListByDataClass<Dao.detailMachine>(checkSqlForUseDays.ToString(), new { SummaryId = result.SummaryId, MachineId = result.MachineId, ContentIdList = contentIdList, UsedDaysMachine = result.UsedDaysMachine == null ? -1 : result.UsedDaysMachine });
            if (results != null && results.Count > 0)
            {
                // 同一機器で異なる機器使用期間が設定されています。すべて同じ値を入力して下さい。
                errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.UseDays, GetResMessage("111280029"), GetResMessage(ComRes.ID.ID141200011), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                return false;
            }

            return true;
        }

        /// <summary>
        /// アイテム拡張マスタから拡張データ(一般工場、個別工場)のリストを取得する
        /// </summary>
        /// <param name="seq">連番</param>
        /// <returns>拡張データ</returns>
        private List<TMQUtil.StructureItemEx.StructureItemExInfo> getFactoryExDataList(short seq)
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = (int)Const.MsStructure.GroupId.Location;
            //連番
            param.Seq = seq;
            //構成アイテム、アイテム拡張マスタ情報取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);

            return list;
        }

        /// <summary>
        /// ExcelPort保全活動登録処理
        /// </summary>
        /// <param name="resultList">ExcelPortアップロードデータ</param>
        /// <param name="role">ユーザ役割</param>
        /// <returns>true:正常終了</returns>
        private bool executeExcelPortRegistMaintenance(List<Dao.excelPortMaintenance> resultList, Dao.userRole role)
        {
            // システム日時
            DateTime now = DateTime.Now;
            //各工場の保全履歴個別工場フラグを取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> list = getFactoryExDataList(HistoryIndividualDivision.Seq);
            foreach (Dao.excelPortMaintenance result in resultList)
            {
                // 送信時処理IDが設定されているもののみ
                if (result.ProcessId == null)
                {
                    continue;
                }

                if (result.ProcessId == Const.SendProcessId.Regist)
                {
                    //登録
                    if (!registExcelPortMaintenance(result, true, role, list, now))
                    {
                        return false;
                    }
                }
                else if (result.ProcessId == Const.SendProcessId.Update)
                {
                    //更新
                    if (!registExcelPortMaintenance(result, false, role, list, now))
                    {
                        return false;
                    }
                }
                else if (result.ProcessId == Const.SendProcessId.Delete)
                {
                    //削除
                    if (!deleteExcelPortMaintenance(result, role, now))
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// ExcelPort保全活動シート 新規登録・更新処理
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="isRegist">新規登録の場合True</param>
        /// <param name="role">ユーザ役割</param>
        /// <param name="historyIndividualList">各工場の保全履歴個別工場フラグ</param>
        /// <param name="now">システム日時</param>
        /// <returns>true:正常終了</returns>
        private bool registExcelPortMaintenance(Dao.excelPortMaintenance result, bool isRegist, Dao.userRole role, List<TMQUtil.StructureItemEx.StructureItemExInfo> historyIndividualList, DateTime now)
        {
            if (isRegist)
            {
                Dao.detailSummaryInfo summaryInfo = new();
                summaryInfo.LocationStructureId = result.LocationStructureId;
                //getRequestNo()内ではツリー選択ラベルを想定しているため、NameにIDを設定しておく
                summaryInfo.FactoryName = result.FactoryId == null ? "0" : result.FactoryId.ToString();

                //依頼番号を取得
                string requestNo = getRequestNo(summaryInfo, now);
                if (requestNo == null)
                {
                    return false;
                }
                result.RequestNo = requestNo;
            }

            // 共通の更新日時などを設定
            bool chkUpd = int.TryParse(this.UserId, out int userId);
            setExecuteConditionByDataClassCommon<Dao.excelPortMaintenance>(ref result, now, userId, userId);

            //保全履歴個別工場フラグ
            string historyIndividualFlg = IndividualDivision.Hide;
            if (historyIndividualList != null)
            {
                //対象工場の保全履歴個別工場フラグを取得
                historyIndividualFlg = historyIndividualList.Where(x => x.FactoryId == result.FactoryId).Select(x => x.ExData).FirstOrDefault() ?? IndividualDivision.Hide;
            }

            //トランザクションを分けるため、依頼番号取得後にデータを登録する（新規登録の場合、ユーザ役割の権限が無くてもレコードは作成しておく）
            // 保全活動件名登録
            if (historyIndividualFlg == IndividualDivision.Show)
            {
                //個別工場の場合、保全履歴(個別工場)に設定された完了日を登録する
                result.CompletionDate = result.CompletionDateIndividual;
            }
            bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long summaryId, isRegist ? SqlName.Regist.InsertSummary : SqlName.Regist.UpdateSummary, SqlName.SubDir, result, this.db);
            if (!returnFlag)
            {
                return false;
            }
            result.SummaryId = summaryId;

            // 保全依頼登録
            if (role.Manufacturing || isRegist)
            {
                //製造権限がない場合は依頼情報の更新は行わない（新規登録の場合はレコードは作成しておく）
                if (!registRequest())
                {
                    return false;
                }
            }

            if (!role.Maintenance && !isRegist)
            {
                //保全権限がない場合は依頼情報以外の更新は行わない（新規登録の場合はレコードは作成しておく）
                return true;
            }

            //保全計画登録
            if (!registPlan())
            {
                return false;
            }

            // 保全履歴登録
            (bool returnFlag, long historyId) resultHistory = registExcelPortMaintenanceHistory(result, isRegist, historyIndividualFlg, now);
            if (!resultHistory.returnFlag)
            {
                return false;
            }

            //故障情報登録
            if (result.ActivityDivision == MaintenanceDivision.Failure)
            {
                returnFlag = registExcelPortMaintenanceHistoryFailure(result, isRegist, resultHistory.historyId, now);
                if (!returnFlag)
                {
                    return false;
                }
            }

            //保全スケジュール詳細更新
            if (!updateSchedule(isRegist, result.CompletionDate, summaryId, now))
            {
                return false;
            }

            return true;

            //保全依頼登録
            bool registRequest()
            {
                if (!isRegist)
                {
                    //更新の場合、依頼IDを取得
                    ComDao.MaRequestEntity requestInfo = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.MaRequestEntity>(SqlName.Detail.GetRequestInfo, SqlName.SubDir, result, this.db);
                    result.RequestId = requestInfo.RequestId;
                }
                return TMQUtil.SqlExecuteClass.Regist(isRegist ? SqlName.Regist.InsertRequest : SqlName.Regist.UpdateRequest, SqlName.SubDir, result, this.db);
            }

            //保全計画登録
            bool registPlan()
            {
                //保全計画の件名は「PlanSubject」に保持しているが、SQLでは「Subject」を使用するので置き換える
                result.Subject = result.PlanSubject;
                if (!isRegist)
                {
                    //更新の場合、保全計画IDを取得
                    ComDao.MaPlanEntity planInfo = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.MaPlanEntity>(SqlName.Detail.GetPlanInfo, SqlName.SubDir, result, this.db);
                    result.PlanId = planInfo.PlanId;
                }
                return TMQUtil.SqlExecuteClass.Regist(isRegist ? SqlName.Regist.InsertPlan : SqlName.Regist.UpdatePlan, SqlName.SubDir, result, this.db);
            }
        }

        /// <summary>
        /// ExcelPort保全履歴の登録・更新
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="isRegist">新規登録の場合True</param>
        /// <param name="historyIndividualFlg">保全履歴個別工場表示フラグ</param>
        /// <param name="now">システム日時</param>
        /// <returns>returnFlag:エラーの場合False、historyId:履歴ID</returns>
        private (bool returnFlag, long historyId) registExcelPortMaintenanceHistory(Dao.excelPortMaintenance result, bool isRegist, string historyIndividualFlg, DateTime now)
        {
            ComDao.MaHistoryEntity history = new();
            history.SummaryId = result.SummaryId;
            if (historyIndividualFlg == IndividualDivision.Hide)
            {
                //一般工場の場合

                //呼出に登録する値は拡張データを登録する
                history.CallCount = result.CallCountId;
                history.MaintenanceSeasonStructureId = result.MaintenanceSeasonStructureId;
                history.ConstructionCompany = result.ConstructionCompany;
                history.ConstructionPersonnelId = result.ConstructionPersonnelId;
                history.ActualResultStructureId = result.ActualResultStructureId;
                history.LossAbsence = result.LossAbsence;
                history.LossAbsenceTypeCount = result.LossAbsenceTypeCount;
                history.MaintenanceOpinion = result.MaintenanceOpinion;
                history.WorkingTimeSelf = result.WorkingTimeSelf;
                history.WorkingTimeCompany = result.WorkingTimeCompany;
                history.TotalWorkingTime = result.TotalWorkingTime;
                history.CostNote = result.CostNote;
                history.Expenditure = result.Expenditure;
                history.PartsExistenceFlg = false;
            }
            else
            {
                //個別工場の場合、保全履歴(個別工場)の列に設定された値を登録する
                history.ConstructionPersonnelId = result.ConstructionPersonnelIdIndividual;
                history.ManufacturingPersonnelId = result.ManufacturingPersonnelId;
                history.Expenditure = result.ExpenditureIndividual;
                history.WorkFailureDivisionStructureId = result.WorkFailureDivisionStructureId;
                history.OccurrenceTime = result.OccurrenceTime;
                history.CallCount = result.CallCount;
                history.StopCount = result.StopCount;
                history.DiscoveryPersonnel = result.DiscoveryPersonnel;
                history.EffectProductionStructureId = result.EffectProductionStructureId;
                history.EffectQualityStructureId = result.EffectQualityStructureId;
                history.FailureSite = result.FailureSite;
                history.TotalWorkingTime = result.TotalWorkingTimeIndividual;
                history.WorkingTimeResearch = result.WorkingTimeResearch;
                history.WorkingTimeProcure = result.WorkingTimeProcure;
                history.WorkingTimeRepair = result.WorkingTimeRepair;
                history.WorkingTimeTest = result.WorkingTimeTest;
                history.WorkingTimeCompany = result.WorkingTimeCompanyIndividual;
                history.RankStructureId = null;//将来復活
                //予備品有無がNULLの場合、FALSEを設定
                history.PartsExistenceFlg = result.PartsExistenceFlg == 1;
            }

            long historyId = -1;
            if (!isRegist)
            {
                //更新の場合、保全履歴IDを取得
                historyId = TMQUtil.SqlExecuteClass.SelectEntity<long>(SqlName.ExcelPort.GetHistoryIdBySummaryId, SqlName.SubDir, result, this.db);
                history.HistoryId = historyId;

                //フォロー有無を取得
                ComDao.MaHistoryEntity entity = new ComDao.MaHistoryEntity().GetEntity(historyId, this.db);
                history.FollowFlg = entity.FollowFlg;
            }
            //新規登録の場合、フォロー有無にFalseを設定
            history.FollowFlg = history.FollowFlg ?? false;
            // 共通の更新日時などを設定
            bool chkUpd = int.TryParse(this.UserId, out int userId);
            setExecuteConditionByDataClassCommon<ComDao.MaHistoryEntity>(ref history, now, userId, userId);

            // 保全履歴登録
            //保全権限がない場合は依頼情報以外の更新は行わない（新規登録の場合はレコードは作成しておく）
            bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out historyId, isRegist ? SqlName.Regist.InsertHistory : SqlName.Regist.UpdateHistory, SqlName.SubDir, history, this.db);
            return (returnFlag, historyId);
        }

        /// <summary>
        /// ExcelPort故障情報の登録・更新
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="isRegist">新規登録の場合True</param>
        /// <param name="historyId">履歴ID</param>
        /// <param name="now">システム日時</param>
        /// <returns>エラーの場合False</returns>
        private bool registExcelPortMaintenanceHistoryFailure(Dao.excelPortMaintenance result, bool isRegist, long historyId, DateTime now)
        {
            if (isRegist)
            {
                //新規登録の場合は、レコード作成する
                ComDao.MaHistoryFailureEntity historyFailure = new();
                historyFailure.HistoryId = historyId;
                historyFailure.FollowFlg = false;
                // 共通の更新日時などを設定
                bool chkUpd = int.TryParse(this.UserId, out int userId);
                setExecuteConditionByDataClassCommon<ComDao.MaHistoryFailureEntity>(ref historyFailure, now, userId, userId);
                bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.InsertHistoryFailure, SqlName.SubDir, historyFailure, this.db);
                if (!returnFlag)
                {
                    return false;
                }
            }
            else
            {
                //更新の場合、故障情報の更新は保全活動_故障情報シートで行うため機器使用期間のみ更新

                //保全履歴故障情報IDを取得
                long? historyFailureId = TMQUtil.SqlExecuteClass.SelectEntity<long?>(SqlName.Regist.GetFailureInfoForHistoryId, SqlName.SubDir, new { HistoryId = historyId }, this.db);
                if (historyFailureId == null)
                {
                    return false;
                }
                ComDao.MaHistoryFailureEntity historyFailure = new ComDao.MaHistoryFailureEntity().GetEntity(historyFailureId ?? -1, this.db);

                //機器使用期間の設定
                int days = setUsedDaysMachine(result.CompletionDate, historyFailure.MachineId ?? -1);
                historyFailure.UsedDaysMachine = days >= 0 ? days : null;
                // 共通の更新日時などを設定
                bool chkUpd = int.TryParse(this.UserId, out int userId);
                setExecuteConditionByDataClassCommon<ComDao.MaHistoryFailureEntity>(ref historyFailure, now, userId, -1);
                bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryFailure, SqlName.SubDir, historyFailure, this.db);
                if (!returnFlag)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// ExcelPort保全活動シート 削除処理
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="role">ユーザ役割</param>
        /// <param name="now">システム日時</param>
        /// <returns>true:正常終了</returns>
        private bool deleteExcelPortMaintenance(Dao.excelPortMaintenance result, Dao.userRole role, DateTime now)
        {
            //保全活動件名削除
            bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteSummary, SqlName.SubDir, result, this.db);
            if (!returnFlag)
            {
                return false;
            }

            //依頼IDを取得
            ComDao.MaRequestEntity requestInfo = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.MaRequestEntity>(SqlName.Detail.GetRequestInfo, SqlName.SubDir, result, this.db);
            result.RequestId = requestInfo.RequestId;
            //保全依頼削除
            returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteRequest, SqlName.SubDir, result, this.db);
            if (!returnFlag)
            {
                return false;
            }

            //保全計画IDを取得
            ComDao.MaPlanEntity planInfo = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.MaPlanEntity>(SqlName.Detail.GetPlanInfo, SqlName.SubDir, result, this.db);
            result.PlanId = planInfo.PlanId;
            //保全計画削除
            returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeletePlan, SqlName.SubDir, result, this.db);
            if (!returnFlag)
            {
                return false;
            }

            //保全履歴IDを取得
            long historyId = TMQUtil.SqlExecuteClass.SelectEntity<long>(SqlName.ExcelPort.GetHistoryIdBySummaryId, SqlName.SubDir, result, this.db);
            result.HistoryId = historyId;
            if (result.ActivityDivision == MaintenanceDivision.Inspection)
            {
                //点検情報の場合
                //保全履歴機器、保全履歴機器部位、保全履歴点検内容削除
                if (!deleteMachineList())
                {
                    return false;
                }
            }
            else
            {
                //故障情報の場合
                //保全履歴故障情報IDを取得
                long? historyFailureId = TMQUtil.SqlExecuteClass.SelectEntity<long?>(SqlName.Regist.GetFailureInfoForHistoryId, SqlName.SubDir, new { HistoryId = historyId }, this.db);
                if (historyFailureId == null)
                {
                    return false;
                }
                result.HistoryFailureId = historyFailureId;
                //保全履歴故障情報削除
                returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteHistoryFailure, SqlName.SubDir, new { HistoryFailureId = historyFailureId }, this.db);
                if (!returnFlag)
                {
                    return false;
                }
            }

            //保全履歴削除
            returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteHistory, SqlName.SubDir, result, this.db);
            if (!returnFlag)
            {
                return false;
            }

            //添付情報削除
            if (!deleteAttachment())
            {
                return false;
            }

            //保全スケジュール詳細の更新
            ComDao.McMaintainanceScheduleDetailEntity detail = new ComDao.McMaintainanceScheduleDetailEntity();
            detail.SummaryId = Convert.ToInt64(result.SummaryId);
            // 共通の更新日時などを設定
            bool chkUpd = int.TryParse(this.UserId, out int updUserId);
            setExecuteConditionByDataClassCommon<ComDao.McMaintainanceScheduleDetailEntity>(ref detail, DateTime.Now, updUserId, -1);
            //保全活動件名IDをクリアする
            TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateScheduleSummaryId, SqlName.SubDir, detail, this.db);

            return true;

            //保全履歴機器、保全履歴機器部位、保全履歴点検内容削除
            bool deleteMachineList()
            {
                //保全履歴機器、保全履歴機器部位、保全履歴点検内容を取得(保全履歴削除前に取得する)
                List<Dao.detailMachine> list = TMQUtil.SqlExecuteClass.SelectList<Dao.detailMachine>(SqlName.Detail.GetInspectionMachineList, SqlName.SubDir, result, this.db);
                if (list == null)
                {
                    //削除対象なし
                    return true;
                }
                //保全履歴機器IDリスト
                List<long> historyMachineIdList = list.Where(x => x.HistoryMachineId != null).Select(x => (long)x.HistoryMachineId).Distinct().ToList();
                //保全履歴機器部位IDリスト
                List<long> historyInspectionSiteIdList = list.Where(x => x.HistoryInspectionSiteId != null).Select(x => (long)x.HistoryInspectionSiteId).Distinct().ToList();
                //保全履歴点検内容IDリスト
                List<long> historyInspectionContentIdList = list.Where(x => x.HistoryInspectionContentId != null).Select(x => (long)x.HistoryInspectionContentId).Distinct().ToList();
                foreach (long id in historyMachineIdList)
                {
                    //保全履歴機器削除
                    returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteHistoryMachine, SqlName.SubDir, new { HistoryMachineId = id }, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }
                }
                foreach (long id in historyInspectionSiteIdList)
                {
                    //保全履歴機器部位削除
                    returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteHistoryInspectionSite, SqlName.SubDir, new { HistoryInspectionSiteId = id }, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }
                }
                foreach (long id in historyInspectionContentIdList)
                {
                    //保全履歴点検内容削除
                    returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteHistoryInspectionContent, SqlName.SubDir, new { HistoryInspectionContentId = id }, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }
                }
                return true;
            }

            //添付情報削除
            bool deleteAttachment()
            {
                //添付情報
                Dao.AttachmentInfo attachment = new Dao.AttachmentInfo();
                attachment.FunctionTypeIdList = new List<int>() { (int)Const.Attachment.FunctionTypeId.Summary }; //件名添付
                attachment.KeyId = result.SummaryId ?? -1;
                //削除する添付情報を取得
                List<ComDao.AttachmentEntity> list = TMQUtil.SqlExecuteClass.SelectList<ComDao.AttachmentEntity>(SqlName.Detail.GetAttachmentInfo, SqlName.SubDir, attachment, this.db);
                if (list != null)
                {
                    //削除対象の件名添付が存在する場合、削除
                    if (!new ComDao.AttachmentEntity().DeleteByKeyId(Const.Attachment.FunctionTypeId.Summary, Convert.ToInt64(result.SummaryId), this.db))
                    {
                        return false;
                    }
                }
                if (result.ActivityDivision == MaintenanceDivision.Failure)
                {
                    //故障情報の場合

                    Dao.AttachmentInfo failureAttachment = new Dao.AttachmentInfo();
                    failureAttachment.FunctionTypeIdList = new List<int>()
                    {
                        (int)Const.Attachment.FunctionTypeId.HistoryFailureDiagram, //故障分析情報タブ-略図添付
                        (int)Const.Attachment.FunctionTypeId.HistoryFailureAnalyze, //故障分析情報タブ-故障原因分析書添付
                        (int)Const.Attachment.FunctionTypeId.HistoryFailureFactDiagram, //故障分析情報(個別工場)タブ-略図添付
                        (int)Const.Attachment.FunctionTypeId.HistoryFailureFactAnalyze //故障分析情報(個別工場)タブ-故障原因分析書添付
                    };
                    failureAttachment.KeyId = (long)result.HistoryFailureId;
                    //削除する添付情報を取得
                    list = TMQUtil.SqlExecuteClass.SelectList<ComDao.AttachmentEntity>(SqlName.Detail.GetAttachmentInfo, SqlName.SubDir, failureAttachment, this.db);
                    if (list != null)
                    {
                        //削除対象の略図、故障原因分析書の添付情報が存在する場合、削除
                        bool returnFlg = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteAttachment, SqlName.SubDir, failureAttachment, this.db);
                        if (!returnFlg)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// ExcelPort保全活動_故障情報登録処理
        /// </summary>
        /// <param name="resultList">ExcelPortアップロードデータ</param>
        /// <param name="role">ユーザ役割</param>
        /// <returns>true:正常終了</returns>
        private bool executeExcelPortRegistHistoryFailure(List<Dao.excelPortHistoryFailure> resultList, Dao.userRole role)
        {
            // システム日時
            DateTime now = DateTime.Now;
            //各工場の故障情報個別工場フラグを取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> list = getFactoryExDataList(FailureIndividualDivision.Seq);
            foreach (Dao.excelPortHistoryFailure result in resultList)
            {
                // 送信時処理IDが設定されているもののみ
                if (result.ProcessId == null)
                {
                    continue;
                }

                if (result.ProcessId == Const.SendProcessId.Update)
                {
                    //更新
                    if (!registExcelPortHistoryFailure(result, list, now))
                    {
                        return false;
                    }
                }
                else if (result.ProcessId == Const.SendProcessId.Delete)
                {
                    //削除
                    if (!deleteExcelPortHistoryFailure(result, now))
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// ExcelPort保全活動_故障情報シート 更新処理
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="historyFailureIndividualList">各工場の故障情報個別工場フラグ</param>
        /// <param name="now">システム日時</param>
        /// <returns>true:正常終了</returns>
        private bool registExcelPortHistoryFailure(Dao.excelPortHistoryFailure result, List<TMQUtil.StructureItemEx.StructureItemExInfo> historyFailureIndividualList, DateTime now)
        {
            // 共通の更新日時などを設定
            bool chkUpd = int.TryParse(this.UserId, out int userId);
            setExecuteConditionByDataClassCommon<Dao.excelPortHistoryFailure>(ref result, now, userId, userId);

            //故障情報個別工場フラグ
            string historyFailureIndividualFlg = IndividualDivision.Hide;
            if (historyFailureIndividualList != null)
            {
                //対象工場の故障情報個別工場フラグを取得
                historyFailureIndividualFlg = historyFailureIndividualList.Where(x => x.FactoryId == result.FactoryId).Select(x => x.ExData).FirstOrDefault() ?? IndividualDivision.Hide;
            }

            //保全活動件名の情報を取得
            ComDao.MaSummaryEntity summary = new ComDao.MaSummaryEntity().GetEntity(result.SummaryId, this.db);

            //機器使用期間の設定
            int days = setUsedDaysMachine(summary.CompletionDate, result.MachineId ?? -1, result.UsedDaysMachine);
            if (days >= 0)
            {
                result.UsedDaysMachine = days;
            }

            if (historyFailureIndividualFlg == IndividualDivision.Hide)
            {
                //一般工場の場合、故障情報の列に設定された値のみを登録する（個別工場の列に設定された値は登録しない）
                result.FailureAnalysisStructureId = null;
                result.FailurePersonalityFactorStructureId = null;
                result.FailurePersonalityClassStructureId = null;
                result.TreatmentStatusStructureId = null;
                result.NecessityMeasureStructureId = null;
                result.MeasurePlanDate = null;
                result.MeasureClass1StructureId = null;
                result.MeasureClass2StructureId = null;
            }
            else
            {
                //個別工場の場合、故障情報(個別工場)の列に設定された値を登録する
                result.FailureStatus = result.FailureStatusIndividual;
                result.FailureCauseAdditionNote = result.FailureCauseAdditionNoteIndividual;
                result.PreviousSituation = result.PreviousSituationIndividual;
                result.RecoveryAction = result.RecoveryActionIndividual;
                result.ImprovementMeasure = result.ImprovementMeasureIndividual;
                result.Lesson = result.LessonIndividual;
                result.FailureNote = result.FailureNoteIndividual;
                //一般工場の列に設定された値は登録しない
                result.SystemFeedBack = null;
            }
            //フォロー有無がNULLの場合、FALSEを設定
            result.FollowFlg = result.FollowFlg ?? 0;

            //故障情報更新
            bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryFailure, SqlName.SubDir, result, this.db);
            if (!returnFlag)
            {
                return false;
            }

            //保全履歴のフォロー有無を更新
            ComDao.MaHistoryFailureEntity entity = new ComDao.MaHistoryFailureEntity().GetEntity(result.HistoryFailureId, this.db);
            setExecuteConditionByDataClassCommon<ComDao.MaHistoryFailureEntity>(ref entity, now, userId, -1);
            returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.ExcelPort.UpdateHistoryFollowFlg, SqlName.SubDir, entity, this.db);
            if (!returnFlag)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ExcelPort保全活動_故障情報シート 削除処理
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="now">システム日時</param>
        /// <returns>true:正常終了</returns>
        private bool deleteExcelPortHistoryFailure(Dao.excelPortHistoryFailure result, DateTime now)
        {
            //保全活動件名の削除は保全活動シートで行うので、ここでは故障情報のクリアを行う（レコードは削除しない）

            ComDao.MaHistoryFailureEntity historyFailure = new();
            historyFailure.HistoryFailureId = result.HistoryFailureId;
            //フォロー有無にFALSEを設定
            result.FollowFlg = result.FollowFlg ?? 0;
            // 共通の更新日時などを設定
            bool chkUpd = int.TryParse(this.UserId, out int userId);
            setExecuteConditionByDataClassCommon<ComDao.MaHistoryFailureEntity>(ref historyFailure, now, userId, -1);
            //故障情報更新
            bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryFailure, SqlName.SubDir, historyFailure, this.db);
            if (!returnFlag)
            {
                return false;
            }

            //保全履歴のフォロー有無を更新
            ComDao.MaHistoryFailureEntity entity = new ComDao.MaHistoryFailureEntity().GetEntity(result.HistoryFailureId, this.db);
            entity.FollowFlg = false;
            setExecuteConditionByDataClassCommon<ComDao.MaHistoryFailureEntity>(ref entity, now, userId, -1);
            returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.ExcelPort.UpdateHistoryFollowFlg, SqlName.SubDir, entity, this.db);
            if (!returnFlag)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ExcelPort保全活動_点検情報（対象機器）登録処理
        /// </summary>
        /// <param name="resultList">ExcelPortアップロードデータ</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <param name="role">ユーザ役割</param>
        /// <returns>true:正常終了</returns>
        private bool executeExcelPortRegistInspectionMachine(List<Dao.excelPortInspectionMachine> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dao.userRole role)
        {
            // システム日時
            DateTime now = DateTime.Now;
            foreach (Dao.excelPortInspectionMachine result in resultList)
            {
                // 送信時処理IDが設定されているもののみ
                if (result.ProcessId == null)
                {
                    continue;
                }

                if (result.ProcessId == Const.SendProcessId.Regist)
                {
                    // 機器使用期間の入力チェック
                    if (!checkUseDaysForExcelportExistsDB(result, resultList, ref errorInfoList))
                    {
                        continue;
                    }

                    //登録
                    if (!registExcelPortInspectionMachine(result, ref errorInfoList, now))
                    {
                        return false;
                    }
                }
                else if (result.ProcessId == Const.SendProcessId.Update)
                {
                    //重複チェック
                    if (!checkDuplicationMachine(result, ref errorInfoList))
                    {
                        continue;
                    }

                    // 機器使用期間の入力チェック
                    if (!checkUseDaysForExcelportExistsDB(result, resultList, ref errorInfoList))
                    {
                        continue;
                    }

                    //更新
                    if (!updateExcelPortInspectionMachine(result, now))
                    {
                        return false;
                    }
                }
                else if (result.ProcessId == Const.SendProcessId.Delete)
                {
                    //削除
                    if (!deleteExcelPortInspectionMachine(result, now))
                    {
                        return false;
                    }
                }

            }
            //エラーがある場合、終了
            if (errorInfoList.Count > 0)
            {
                return true;
            }

            //保全活動件名IDを取得
            List<long> summaryIdList = resultList.Select(x => x.SummaryId).Distinct().ToList();
            foreach (long summaryId in summaryIdList)
            {
                //件名IDに紐づく対象機器を取得
                List<Dao.excelPortInspectionMachine> machineList = TMQUtil.SqlExecuteClass.SelectList<Dao.excelPortInspectionMachine>(SqlName.Detail.GetInspectionMachineList, SqlName.SubDir, new { SummaryId = summaryId }, this.db);
                if (machineList == null || machineList.Count == 0)
                {
                    continue;
                }

                //保全履歴IDを取得
                long historyId = TMQUtil.SqlExecuteClass.SelectEntity<long>(SqlName.ExcelPort.GetHistoryIdBySummaryId, SqlName.SubDir, new { SummaryId = summaryId }, this.db);
                //保全履歴のフォロー有無を更新
                ComDao.MaHistoryEntity entity = new();
                entity.HistoryId = historyId;
                entity.FollowFlg = machineList.Select(x => x.FollowFlg ?? 0).Any(x => x == 1);
                // 共通の更新日時などを設定
                bool chkUpd = int.TryParse(this.UserId, out int userId);
                setExecuteConditionByDataClassCommon<ComDao.MaHistoryEntity>(ref entity, now, userId, -1);
                bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.ExcelPort.UpdateHistoryFollowFlg, SqlName.SubDir, entity, this.db);
                if (!returnFlag)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// ExcelPort保全活動_点検情報（対象機器）シート 登録処理
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <param name="now">システム日時</param>
        /// <returns>true:正常終了</returns>
        private bool registExcelPortInspectionMachine(Dao.excelPortInspectionMachine result, ref List<ComDao.UploadErrorInfo> errorInfoList, DateTime now)
        {
            // 共通の更新日時などを設定
            bool chkUpd = int.TryParse(this.UserId, out int userId);
            setExecuteConditionByDataClassCommon<Dao.excelPortInspectionMachine>(ref result, now, userId, userId);

            //保全活動件名の情報を取得
            ComDao.MaSummaryEntity summary = new ComDao.MaSummaryEntity().GetEntity(result.SummaryId, this.db);
            //保全履歴IDを取得
            long historyId = TMQUtil.SqlExecuteClass.SelectEntity<long>(SqlName.ExcelPort.GetHistoryIdBySummaryId, SqlName.SubDir, result, this.db);
            result.HistoryId = historyId;

            //フォロー有無がNULLの場合、FALSEを設定
            result.FollowFlg = result.FollowFlg ?? 0;

            // 保全履歴機器が登録済みかチェック
            ComDao.MaHistoryMachineEntity resultHistoryMachine = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.MaHistoryMachineEntity>(SqlName.Regist.GetHistoryMachine, SqlName.SubDir, result, this.db);
            if (resultHistoryMachine == null)
            {
                //保全履歴機器が未登録の場合、保全履歴機器、保全履歴機器部位、保全履歴点検内容を登録

                //保全履歴機器
                //機器使用期間の設定
                int days = setUsedDaysMachine(summary.CompletionDate, result.MachineId ?? -1, result.UsedDaysMachine);
                if (days >= 0)
                {
                    result.UsedDaysMachine = days;
                }

                //登録
                long val = -1;
                bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out val, SqlName.Regist.InsertHistoryMachine, SqlName.SubDir, result, this.db);
                if (!returnFlag)
                {
                    return false;
                }

                //保全履歴機器部位
                result.HistoryMachineId = val;
                //登録
                val = -1;
                returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out val, SqlName.Regist.InsertHistoryInspectionSite, SqlName.SubDir, result, this.db);
                if (!returnFlag)
                {
                    return false;
                }

                //保全履歴点検内容
                result.HistoryInspectionSiteId = val;
                //登録
                bool registHistoryInspectionContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.InsertHistoryInspectionContent, SqlName.SubDir, result, this.db);
                if (!registHistoryInspectionContent)
                {
                    return false;
                }
            }
            else
            {
                //保全履歴機器は登録済み

                //重複チェック
                if (!checkDuplicationMachine(result, ref errorInfoList))
                {
                    //次のデータへ(エラーはまとめて表示する)
                    return true;
                }

                //保全履歴機器
                result.HistoryMachineId = resultHistoryMachine.HistoryMachineId;
                //機器使用期間の設定
                int days = setUsedDaysMachine(summary.CompletionDate, result.MachineId ?? -1, result.UsedDaysMachine);
                if (days >= 0)
                {
                    result.UsedDaysMachine = days;
                    //機器使用期間の更新
                    bool registHistoryMachine = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryMachine, SqlName.SubDir, result, this.db);
                    if (!registHistoryMachine)
                    {
                        return false;
                    }
                }

                //保全履歴機器部位が登録済みかチェック
                Dao.detailMachine resultInspectionSite = TMQUtil.SqlExecuteClass.SelectEntity<Dao.detailMachine>(SqlName.Regist.GetHistoryInspectionSite, SqlName.SubDir, result, this.db);
                if (resultInspectionSite == null)
                {
                    //保全履歴機器部位が未登録の場合、保全履歴機器部位、保全履歴点検内容を登録

                    //保全履歴機器部位
                    //登録
                    bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long val, SqlName.Regist.InsertHistoryInspectionSite, SqlName.SubDir, result, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }

                    result.HistoryInspectionSiteId = val;
                }
                else
                {
                    //保全履歴機器部位は登録済み

                    result.HistoryInspectionSiteId = resultInspectionSite.HistoryInspectionSiteId;
                }

                //保全履歴点検内容登録
                bool registHistoryInspectionContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.InsertHistoryInspectionContent, SqlName.SubDir, result, this.db);
                if (!registHistoryInspectionContent)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// ExcelPort保全活動_点検情報（対象機器）シート 更新処理
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="now">システム日時</param>
        /// <returns>true:正常終了</returns>
        private bool updateExcelPortInspectionMachine(Dao.excelPortInspectionMachine result, DateTime now)
        {
            // 共通の更新日時などを設定
            bool chkUpd = int.TryParse(this.UserId, out int userId);
            setExecuteConditionByDataClassCommon<Dao.excelPortInspectionMachine>(ref result, now, userId, userId);

            //保全活動件名の情報を取得
            ComDao.MaSummaryEntity summary = new ComDao.MaSummaryEntity().GetEntity(result.SummaryId, this.db);
            //保全履歴IDを取得
            long historyId = TMQUtil.SqlExecuteClass.SelectEntity<long>(SqlName.ExcelPort.GetHistoryIdBySummaryId, SqlName.SubDir, result, this.db);
            result.HistoryId = historyId;

            //機器の変更有無
            bool isChangeMachine = false;
            //更新の場合、機器が変更されているか確認
            ComDao.MaHistoryMachineEntity historyMachine = new ComDao.MaHistoryMachineEntity().GetEntity(result.HistoryMachineId ?? -1, this.db);
            if (historyMachine.MachineId != result.MachineId || historyMachine.EquipmentId != result.EquipmentId)
            {
                //機器の変更ありの場合
                if (!updateExcelPortChangeMachine(result, summary))
                {
                    return false;
                }
            }
            else
            {
                //機器の変更なしの場合
                if (!updateExcelPortNotChangeMachine(result, summary))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 機器の重複チェック
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合false</returns>
        private bool checkDuplicationMachine(Dao.excelPortInspectionMachine result, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            //件名IDに紐づく対象機器を取得
            List<Dao.excelPortInspectionMachine> machineList = TMQUtil.SqlExecuteClass.SelectList<Dao.excelPortInspectionMachine>(SqlName.Detail.GetInspectionMachineList, SqlName.SubDir, new { SummaryId = result.SummaryId }, this.db);
            //処理対象データを除く
            machineList.RemoveAll(x => x.HistoryInspectionSiteId == result.HistoryInspectionSiteId && x.HistoryInspectionContentId == result.HistoryInspectionContentId);
            if (machineList == null || machineList.Count == 0)
            {
                return true;
            }

            //機番ID、機器ID、部位ID、点検内容IDが重複している件数を取得
            var count = machineList.Where(x => x.MachineId == result.MachineId && x.EquipmentId == result.EquipmentId && x.InspectionSiteStructureId == result.InspectionSiteStructureId && x.InspectionContentStructureId == result.InspectionContentStructureId).GroupBy(x => new { x.MachineId, x.EquipmentId, x.InspectionSiteStructureId, x.InspectionContentStructureId }).Count();
            if (count > 0)
            {
                // 対象機器が重複しています。
                errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.Machine, GetResMessage("111070019"), GetResMessage(ComRes.ID.ID141160001), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.InspectionSite, GetResMessage("111300003"), GetResMessage(ComRes.ID.ID141160001), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPort.ColumnNo.InspectionMachine.InspectionContent, GetResMessage("111300023"), GetResMessage(ComRes.ID.ID141160001), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));

                return false;
            }
            return true;
        }

        /// <summary>
        /// ExcelPort更新行 機器変更ありの場合の登録処理
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="summary">保全活動件名情報</param>
        /// <returns>エラーの場合false</returns>
        private bool updateExcelPortChangeMachine(Dao.excelPortInspectionMachine result, ComDao.MaSummaryEntity summary)
        {
            // 変更後の保全履歴機器が登録済みかチェック
            ComDao.MaHistoryMachineEntity resultHistoryMachine = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.MaHistoryMachineEntity>(SqlName.Regist.GetHistoryMachine, SqlName.SubDir, result, this.db);
            // 変更前の保全履歴機器に紐づく保全履歴機器部位、保全履歴点検内容の存在チェック
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetCountRelationHistoryMachine, out string sql);
            // 件数を取得
            int cnt = db.GetCount(sql, result);
            if (cnt > 0)
            {
                //変更前の保全履歴機器に紐づく保全履歴機器部位、保全履歴点検内容が他に存在するため、対象の保全履歴機器のレコードは更新しない
                if (resultHistoryMachine == null)
                {
                    //変更後の保全履歴機器が未登録の場合
                    if (!notExistsAfterHistoryMachine())
                    {
                        return false;
                    }
                }
                else
                {
                    //変更後の保全履歴機器が登録済の場合
                    if (!existsAfterHistoryMachine())
                    {
                        return false;
                    }
                }
            }
            else
            {
                //変更前の保全履歴機器に紐づく保全履歴機器部位、保全履歴点検内容は対象データのみ

                if (resultHistoryMachine == null)
                {
                    //変更後の保全履歴機器は未登録

                    //保全履歴機器を更新
                    //機器使用期間の設定
                    int days = setUsedDaysMachine(summary.CompletionDate, result.MachineId ?? -1, result.UsedDaysMachine);
                    if (days >= 0)
                    {
                        result.UsedDaysMachine = days;
                    }
                    bool registHistoryMachine = TMQUtil.SqlExecuteClass.Regist(SqlName.Replace.UpdateHistoryMachineInfoAll, SqlName.SubDir, result, this.db);
                    if (!registHistoryMachine)
                    {
                        return false;
                    }

                    //保全履歴機器部位を更新
                    bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryInspectionSite, SqlName.SubDir, result, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }
                }
                else
                {
                    //変更後の保全履歴機器は登録済

                    //変更前の保全履歴機器を削除
                    bool returnFlag = new ComDao.MaHistoryMachineEntity().DeleteByPrimaryKey(result.HistoryMachineId ?? -1, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }
                    //変更後の保全履歴機器に保全履歴機器部位、保全履歴点検内容を紐づける
                    result.HistoryMachineId = resultHistoryMachine.HistoryMachineId;

                    //変更後の保全履歴機器部位が存在するかチェック
                    Dao.detailMachine resultInspectionSite = TMQUtil.SqlExecuteClass.SelectEntity<Dao.detailMachine>(SqlName.Regist.GetHistoryInspectionSite, SqlName.SubDir, result, this.db);
                    if (resultInspectionSite != null)
                    {
                        //存在する場合、変更前の保全履歴機器部位を削除
                        returnFlag = new ComDao.MaHistoryInspectionSiteEntity().DeleteByPrimaryKey(result.HistoryInspectionSiteId ?? -1, this.db);
                        if (!returnFlag)
                        {
                            return false;
                        }
                        result.HistoryInspectionSiteId = resultInspectionSite.HistoryInspectionSiteId;
                    }
                    else
                    {
                        //存在しない場合、保全履歴機器部位を更新（保全履歴機器との紐付けを更新）
                        returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryInspectionSite, SqlName.SubDir, result, this.db);
                        if (!returnFlag)
                        {
                            return false;
                        }
                    }
                }

                //保全履歴点検内容の更新
                //フォロー有無がNULLの場合、FALSEを設定
                result.FollowFlg = result.FollowFlg ?? 0;
                bool registHistoryInspectionContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryInspectionContent, SqlName.SubDir, result, this.db, "", new List<string> { uncommentExcelPort });
                if (!registHistoryInspectionContent)
                {
                    return false;
                }
            }
            return true;

            //変更後の保全履歴機器が未登録の場合の処理
            bool notExistsAfterHistoryMachine()
            {
                //変更後の保全履歴機器を新規登録
                //機器使用期間の設定
                int days = setUsedDaysMachine(summary.CompletionDate, result.MachineId ?? -1, result.UsedDaysMachine);
                if (days >= 0)
                {
                    result.UsedDaysMachine = days;
                }

                //登録
                long val = -1;
                bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out val, SqlName.Regist.InsertHistoryMachine, SqlName.SubDir, result, this.db);
                if (!returnFlag)
                {
                    return false;
                }
                result.HistoryMachineId = val;

                //変更前の保全履歴機器部位に紐づく保全履歴点検内容が他に存在するかチェック
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetCountHistoryInspectionContent, out sql);
                // 件数を取得
                cnt = db.GetCount(sql, result);
                if (cnt <= 1)
                {
                    //変更対象データ以外が紐づかない場合は、対象の保全履歴機器部位を更新
                    returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryInspectionSite, SqlName.SubDir, result, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }
                }
                else
                {
                    //保全履歴点検内容が他に存在する場合は、対象の保全履歴機器部位のレコードは更新せずに新規登録

                    //登録
                    val = -1;
                    returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out val, SqlName.Regist.InsertHistoryInspectionSite, SqlName.SubDir, result, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }
                    result.HistoryInspectionSiteId = val;
                }

                //保全履歴点検内容の更新
                //フォロー有無がNULLの場合、FALSEを設定
                result.FollowFlg = result.FollowFlg ?? 0;
                bool registHistoryInspectionContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryInspectionContent, SqlName.SubDir, result, this.db, "", new List<string> { uncommentExcelPort });
                if (!registHistoryInspectionContent)
                {
                    return false;
                }
                return true;
            }

            //変更後の保全履歴機器が登録済の場合の処理
            bool existsAfterHistoryMachine()
            {
                //変更後の保全履歴機器に保全履歴機器部位、保全履歴点検内容を紐づける
                result.HistoryMachineId = resultHistoryMachine.HistoryMachineId;

                //保全履歴機器部位の更新
                if (!updateExcelPortHistoryInspectionSite(result))
                {
                    return false;
                }

                //保全履歴点検内容の更新
                //フォロー有無がNULLの場合、FALSEを設定
                result.FollowFlg = result.FollowFlg ?? 0;
                bool registHistoryInspectionContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryInspectionContent, SqlName.SubDir, result, this.db, "", new List<string> { uncommentExcelPort });
                if (!registHistoryInspectionContent)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// ExcelPort更新行 機器変更なしの場合の登録処理
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="summary">保全活動件名情報</param>
        /// <returns>エラーの場合false</returns>
        private bool updateExcelPortNotChangeMachine(Dao.excelPortInspectionMachine result, ComDao.MaSummaryEntity summary)
        {
            //機器使用期間の設定
            int days = setUsedDaysMachine(summary.CompletionDate, result.MachineId ?? -1, result.UsedDaysMachine);
            if (days >= 0)
            {
                result.UsedDaysMachine = days;
                //機器使用期間の更新
                bool registHistoryMachine = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryMachine, SqlName.SubDir, result, this.db);
                if (!registHistoryMachine)
                {
                    return false;
                }
            }
            //部位の更新前の値を取得
            ComDao.MaHistoryInspectionSiteEntity inspectionSite = new ComDao.MaHistoryInspectionSiteEntity().GetEntity(result.HistoryInspectionSiteId ?? -1, this.db);
            if (inspectionSite.InspectionSiteStructureId != result.InspectionSiteStructureId)
            {
                //部位が変更されている場合

                //保全履歴機器部位の更新
                if (!updateExcelPortHistoryInspectionSite(result))
                {
                    return false;
                }
            }

            //保全履歴点検内容の更新
            //フォロー有無がNULLの場合、FALSEを設定
            result.FollowFlg = result.FollowFlg ?? 0;
            bool registHistoryInspectionContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryInspectionContent, SqlName.SubDir, result, this.db, "", new List<string> { uncommentExcelPort });
            if (!registHistoryInspectionContent)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ExcelPort 保全履歴機器部位の更新処理
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <returns>エラーの場合False</returns>
        private bool updateExcelPortHistoryInspectionSite(Dao.excelPortInspectionMachine result)
        {
            //変更前の保全履歴機器部位に紐づく保全履歴点検内容が他に存在するかチェック
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetCountHistoryInspectionContent, out string sql);
            // 件数を取得
            int cnt = db.GetCount(sql, result);
            if (cnt <= 1)
            {
                //変更対象データ以外が紐づかない場合

                //変更後の保全履歴機器部位が存在するかチェック
                Dao.detailMachine resultInspectionSite = TMQUtil.SqlExecuteClass.SelectEntity<Dao.detailMachine>(SqlName.Regist.GetHistoryInspectionSite, SqlName.SubDir, result, this.db);
                if (resultInspectionSite != null)
                {
                    //存在する場合、変更前の保全履歴機器部位を削除
                    bool returnFlag = new ComDao.MaHistoryInspectionSiteEntity().DeleteByPrimaryKey(result.HistoryInspectionSiteId ?? -1, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }
                    result.HistoryInspectionSiteId = resultInspectionSite.HistoryInspectionSiteId;
                }
                else
                {
                    //存在しない場合、対象の保全履歴機器部位を更新
                    bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryInspectionSite, SqlName.SubDir, result, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }
                }
            }
            else
            {
                //紐づく保全履歴点検内容が他に存在する場合

                //変更後の保全履歴機器部位が存在するかチェック
                Dao.detailMachine resultInspectionSite = TMQUtil.SqlExecuteClass.SelectEntity<Dao.detailMachine>(SqlName.Regist.GetHistoryInspectionSite, SqlName.SubDir, result, this.db);
                if (resultInspectionSite != null)
                {
                    //存在する場合、処理なし（変更前の保全履歴機器部位のレコードは更新しない）
                    result.HistoryInspectionSiteId = resultInspectionSite.HistoryInspectionSiteId;
                }
                else
                {
                    //存在しない場合、変更後の保全履歴機器部位を登録
                    bool registHistoryInspectionSite = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long val, SqlName.Regist.InsertHistoryInspectionSite, SqlName.SubDir, result, this.db);
                    if (!registHistoryInspectionSite)
                    {
                        return false;
                    }
                    result.HistoryInspectionSiteId = val;
                }
            }
            return true;
        }

        /// <summary>
        /// ExcelPort保全活動_点検情報（対象機器）シート 削除処理
        /// </summary>
        /// <param name="result">ExcelPortアップロードデータ</param>
        /// <param name="now">システム日時</param>
        /// <returns>true:正常終了</returns>
        private bool deleteExcelPortInspectionMachine(Dao.excelPortInspectionMachine result, DateTime now)
        {
            //保全履歴点検内容の削除
            bool resultFlg = new ComDao.MaHistoryInspectionContentEntity().DeleteByPrimaryKey(result.HistoryInspectionContentId ?? -1, this.db);
            if (!resultFlg)
            {
                return false;
            }

            //保全履歴機器部位に紐づく保全履歴点検内容が存在するかチェック
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetCountHistoryInspectionContent, out string sql);
            // 件数を取得
            int cnt = db.GetCount(sql, result);
            if (cnt > 0)
            {
                //保全履歴機器部位に紐づく保全履歴点検内容が存在するため、保全履歴機器部位は削除しない
                return true;
            }

            //保全履歴機器部位の削除
            resultFlg = new ComDao.MaHistoryInspectionSiteEntity().DeleteByPrimaryKey(result.HistoryInspectionSiteId ?? -1, this.db);
            if (!resultFlg)
            {
                return false;
            }

            //保全履歴機器に紐づく保全履歴機器部位が存在するかチェック
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetCountHistoryInspectionSite, out sql);
            // 件数を取得
            cnt = db.GetCount(sql, result);
            if (cnt > 0)
            {
                //保全履歴機器部位に紐づく保全履歴点検内容が存在するため、保全履歴機器部位は削除しない
                return true;
            }

            //保全履歴機器の削除
            resultFlg = new ComDao.MaHistoryMachineEntity().DeleteByPrimaryKey(result.HistoryMachineId ?? -1, this.db);
            if (!resultFlg)
            {
                return false;
            }

            return true;
        }
        #endregion

    }
}