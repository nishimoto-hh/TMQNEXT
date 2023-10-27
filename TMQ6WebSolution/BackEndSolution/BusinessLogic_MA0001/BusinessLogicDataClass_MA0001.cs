using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_MA0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_MA0001
    {
        /// <summary>
        /// 一覧画面 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long? SummaryId { get; set; }
            /// <summary>Gets or sets 活動区分ID</summary>
            /// <value>活動区分ID</value>
            public int? ActivityDivision { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets タブ番号</summary>
            /// <value>タブ番号</value>
            public int? TabNo { get; set; }
        }

        /// <summary>
        /// 一覧画面 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            // SQLの検索結果の列を定義してください。
            // 品目マスタから多くの列を取得する場合は、品目マスタのデータクラスを継承することで、それらの定義を省くことができます。

            /// <summary>Gets or sets 発生日</summary>
            /// <value>発生日</value>
            public DateTime? OccurrenceDate { get; set; }
            /// <summary>Gets or sets 完了日</summary>
            /// <value>完了日</value>
            public DateTime? CompletionDate { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets MQ分類ID</summary>
            /// <value>MQ分類ID</value>
            public int? MqClassStructureId { get; set; }
            /// <summary>Gets or sets MQ分類(表示用)</summary>
            /// <value>MQ分類(表示用)</value>
            public string MqClassName { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 職種機種階層ID(カンマ区切り)</summary>
            /// <value>職種機種階層ID(カンマ区切り)</value>
            public string JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public string JobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public string LargeClassficationId { get; set; }
            /// <summary>Gets or sets 機種大分類名称</summary>
            /// <value>機種大分類名称</value>
            public string LargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public string MiddleClassficationId { get; set; }
            /// <summary>Gets or sets 機種中分類名称</summary>
            /// <value>機種中分類名称</value>
            public string MiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public string SmallClassficationId { get; set; }
            /// <summary>Gets or sets 機種小分類名称</summary>
            /// <value>機種小分類名称</value>
            public string SmallClassficationName { get; set; }
            /// <summary>Gets or sets 系停止ID</summary>
            /// <value>系停止ID</value>
            public int? StopSystemStructureId { get; set; }
            /// <summary>Gets or sets 系停止(表示用)</summary>
            /// <value>系停止(表示用)</value>
            public string StopSystemName { get; set; }
            /// <summary>Gets or sets 系停止時間(Hr)</summary>
            /// <value>系停止時間(Hr)</value>
            public decimal? StopTime { get; set; }
            /// <summary>Gets or sets 系停止時間(Hr)(表示用)</summary>
            /// <value>系停止時間(Hr)</value>
            public string StopTimeDisp { get; set; }
            /// <summary>Gets or sets 費用メモ</summary>
            /// <value>費用メモ</value>
            public string CostNote { get; set; }
            /// <summary>Gets or sets 突発区分ID</summary>
            /// <value>突発区分ID</value>
            public int? SuddenDivisionStructureId { get; set; }
            /// <summary>Gets or sets 突発区分(表示用)</summary>
            /// <value>突発区分(表示用)</value>
            public string SuddenDivisionName { get; set; }
            /// <summary>Gets or sets 着工予定日</summary>
            /// <value>着工予定日</value>
            public DateTime? ExpectedConstructionDate { get; set; }
            /// <summary>Gets or sets 故障原因分析書</summary>
            /// <value>故障原因分析書</value>
            public string FileLinkFailure { get; set; }
            /// <summary>Gets or sets 予算管理区分ID</summary>
            /// <value>予算管理区分ID</value>
            public int? BudgetManagementStructureId { get; set; }
            /// <summary>Gets or sets 予算管理区分(表示用)</summary>
            /// <value>予算管理区分(表示用)</value>
            public string BudgetManagementName { get; set; }
            /// <summary>Gets or sets 予算性格区分ID</summary>
            /// <value>予算性格区分ID</value>
            public int? BudgetPersonalityStructureId { get; set; }
            /// <summary>Gets or sets 予算性格区分(表示用)</summary>
            /// <value>予算性格区分(表示用)</value>
            public string BudgetPersonalityName { get; set; }
            /// <summary>Gets or sets 予算金額(k円)</summary>
            /// <value>予算金額(k円)</value>
            public decimal? TotalBudgetCost { get; set; }
            /// <summary>Gets or sets 予算金額(k円)(表示用)</summary>
            /// <value>予算金額(k円)(表示用)</value>
            public string TotalBudgetCostDisp { get; set; }
            /// <summary>Gets or sets 保全時期ID</summary>
            /// <value>保全時期ID</value>
            public int? MaintenanceSeasonStructureId { get; set; }
            /// <summary>Gets or sets 保全時期(表示用)</summary>
            /// <value>保全時期(表示用)</value>
            public string MaintenanceSeasonName { get; set; }
            /// <summary>Gets or sets 依頼担当</summary>
            /// <value>依頼担当</value>
            public string RequestPersonnelName { get; set; }
            /// <summary>Gets or sets 施工担当者</summary>
            /// <value>施工担当者</value>
            public string ConstructionPersonnelName { get; set; }
            /// <summary>Gets or sets 作業時間(Hr)</summary>
            /// <value>作業時間(Hr)</value>
            public decimal? TotalWorkingTime { get; set; }
            /// <summary>Gets or sets 作業時間(Hr)(表示用)</summary>
            /// <value>作業時間(Hr)(表示用)</value>
            public string TotalWorkingTimeDisp { get; set; }
            /// <summary>Gets or sets 自係(Hr)</summary>
            /// <value>自係(Hr)</value>
            public decimal? WorkingTimeSelf { get; set; }
            /// <summary>Gets or sets 自係(Hr)(表示用)</summary>
            /// <value>自係(Hr)(表示用)</value>
            public string WorkingTimeSelfDisp { get; set; }
            /// <summary>Gets or sets 発見方法ID</summary>
            /// <value>発見方法ID</value>
            public int? DiscoveryMethodsStructureId { get; set; }
            /// <summary>Gets or sets 発見方法(表示用)</summary>
            /// <value>発見方法(表示用)</value>
            public string DiscoveryMethodsName { get; set; }
            /// <summary>Gets or sets 実績結果</summary>
            /// <value>実績結果</value>
            public int? ActualResultStructureId { get; set; }
            /// <summary>Gets or sets 実績結果(表示用)</summary>
            /// <value>実績結果(表示用)</value>
            public string ActualResultName { get; set; }
            /// <summary>Gets or sets 施工会社</summary>
            /// <value>施工会社</value>
            public string ConstructionCompany { get; set; }
            /// <summary>Gets or sets カウント件数</summary>
            /// <value>カウント件数</value>
            public int? MaintenanceCount { get; set; }
            /// <summary>Gets or sets 作業計画・実施内容</summary>
            /// <value>作業計画・実施内容</value>
            public string PlanImplementationContent { get; set; }
            /// <summary>Gets or sets 件名メモ</summary>
            /// <value>件名メモ</value>
            public string SubjectNote { get; set; }
            /// <summary>Gets or sets 件名添付</summary>
            /// <value>件名添付</value>
            public string FileLinkSubject { get; set; }
            /// <summary>Gets or sets 保全部位</summary>
            /// <value>保全部位</value>
            public string MaintenanceSiteName { get; set; }
            /// <summary>Gets or sets 保全内容</summary>
            /// <value>保全内容</value>
            public string MaintenanceContentName { get; set; }
            /// <summary>Gets or sets 実績金額(k円)</summary>
            /// <value>実績金額(k円)</value>
            public decimal? Expenditure { get; set; }
            /// <summary>Gets or sets 実績金額(k円)(表示用)</summary>
            /// <value>実績金額(k円)(表示用)</value>
            public string ExpenditureDisp { get; set; }
            /// <summary>Gets or sets 現象ID</summary>
            /// <value>現象ID</value>
            public int? PhenomenonStructureId { get; set; }
            /// <summary>Gets or sets 現象(表示用)</summary>
            /// <value>現象(表示用)</value>
            public string PhenomenonName { get; set; }
            /// <summary>Gets or sets 現象補足</summary>
            /// <value>現象補足</value>
            public string PhenomenonNote { get; set; }
            /// <summary>Gets or sets 原因ID</summary>
            /// <value>原因ID</value>
            public int? FailureCauseStructureId { get; set; }
            /// <summary>Gets or sets 原因(表示用)</summary>
            /// <value>原因(表示用)</value>
            public string FailureCauseName { get; set; }
            /// <summary>Gets or sets 原因補足</summary>
            /// <value>原因補足</value>
            public string FailureCauseNote { get; set; }
            /// <summary>Gets or sets 原因性格階層ID</summary>
            /// <value>原因性格階層ID</value>
            public int? FailureCausePersonalityStructureId { get; set; }
            /// <summary>Gets or sets 原因性格1ID</summary>
            /// <value>原因性格1ID</value>
            public int? FailureCausePersonality1StructureId { get; set; }
            /// <summary>Gets or sets 原因性格1名称</summary>
            /// <value>原因性格1名称</value>
            public string FailureCausePersonality1StructureName { get; set; }
            /// <summary>Gets or sets 原因性格2ID</summary>
            /// <value>原因性格2ID</value>
            public int? FailureCausePersonality2StructureId { get; set; }
            /// <summary>Gets or sets 原因性格2名称</summary>
            /// <value>原因性格2名称</value>
            public string FailureCausePersonality2StructureName { get; set; }
            /// <summary>Gets or sets 性格補足</summary>
            /// <value>性格補足</value>
            public string FailureCausePersonalityNote { get; set; }
            /// <summary>Gets or sets 処置対策ID</summary>
            /// <value>処置対策ID</value>
            public int? TreatmentMeasureStructureId { get; set; }
            /// <summary>Gets or sets 処置対策(表示用)</summary>
            /// <value>処置対策(表示用)</value>
            public string TreatmentMeasureName { get; set; }
            /// <summary>Gets or sets 故障原因</summary>
            /// <value>故障原因</value>
            public string FailureCauseAdditionNote { get; set; }
            /// <summary>Gets or sets 故障状況</summary>
            /// <value>故障状況</value>
            public string FailureStatus { get; set; }
            /// <summary>Gets or sets 故障前の保全実施状況</summary>
            /// <value>故障前の保全実施状況</value>
            public string PreviousSituation { get; set; }
            /// <summary>Gets or sets 復旧措置</summary>
            /// <value>復旧措置</value>
            public string RecoveryAction { get; set; }
            /// <summary>Gets or sets 改善対策</summary>
            /// <value>改善対策</value>
            public string ImprovementMeasure { get; set; }
            /// <summary>Gets or sets 保全システムのフィードバック</summary>
            /// <value>保全システムのフィードバック</value>
            public string SystemFeedBack { get; set; }
            /// <summary>Gets or sets 教訓</summary>
            /// <value>教訓</value>
            public string Lesson { get; set; }
            /// <summary>Gets or sets 特記（メモ）</summary>
            /// <value>特記（メモ）</value>
            public string FailureNote { get; set; }
            /// <summary>Gets or sets フォロー有無</summary>
            /// <value>フォロー有無</value>
            public bool? FollowFlg { get; set; }
            /// <summary>Gets or sets フォロー予定年月</summary>
            /// <value>フォロー予定年月</value>
            public DateTime? FollowPlanDate { get; set; }
            /// <summary>Gets or sets フォロー予定年月(表示用)</summary>
            /// <value>フォロー予定年月(表示用)</value>
            public string FollowPlanDateDisp { get; set; }
            /// <summary>Gets or sets フォロー内容</summary>
            /// <value>フォロー内容</value>
            public string FollowContent { get; set; }
            /// <summary>Gets or sets 依頼No.</summary>
            /// <value>依頼No.</value>
            public string RequestNo { get; set; }
            /// <summary>Gets or sets 進捗状況ID</summary>
            /// <value>進捗状況ID</value>
            public int? ProgressId { get; set; }
            /// <summary>Gets or sets 進捗状況(表示用)</summary>
            /// <value>進捗状況(表示用)</value>
            public string ProgressName { get; set; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long SummaryId { get; set; }
        }

        /// <summary>
        /// ユーザ役割のデータクラス
        /// </summary>
        public class userRole
        {
            /// <summary>Gets or sets 製造権限有無</summary>
            /// <value>製造権限有無</value>
            public bool Manufacturing { get; set; }
            /// <summary>Gets or sets 保全権限有無</summary>
            /// <value>保全権限有無</value>
            public bool Maintenance { get; set; }
        }

        /// <summary>
        /// 詳細画面(件名情報、作業性格情報)のデータクラス
        /// </summary>
        public class detailSummaryInfo : ComDao.MaSummaryEntity
        {
            // 検索条件に使用する場合は、検索条件格納共通クラスを継承してください。

            /// <summary>Gets or sets 依頼No.</summary>
            /// <value>依頼No.</value>
            public string RequestNo { get; set; }
            /// <summary>Gets or sets 完了日(非表示)</summary>
            /// <value>完了日(非表示)</value>
            public DateTime? HideCompletionDate { get; set; }
            /// <summary>Gets or sets フォロー計画有無</summary>
            /// <value>フォロー計画有無</value>
            public int? FollowPlanFlg { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets MQ分類（突発区分の必須チェックに使用する構成ID）</summary>
            /// <value>MQ分類（突発区分の必須チェックに使用する構成ID）</value>
            public string MqNotRequiredStructureId { get; set; }
            /// <summary>Gets or sets 製造権限有無</summary>
            /// <value>製造権限有無</value>
            public bool Manufacturing { get; set; }
            /// <summary>Gets or sets 保全権限有無</summary>
            /// <value>保全権限有無</value>
            public bool Maintenance { get; set; }
            /// <summary>Gets or sets 保全履歴個別工場表示フラグ</summary>
            /// <value>保全履歴個別工場表示フラグ</value>
            public string HistoryIndividualFlg { get; set; }
            /// <summary>Gets or sets 故障分析個別工場表示フラグ</summary>
            /// <value>故障分析個別工場表示フラグ</value>
            public string FailureIndividualFlg { get; set; }
            /// <summary>Gets or sets タブ番号</summary>
            /// <value>タブ番号</value>
            public int? TabNo { get; set; }
            /// <summary>Gets or sets 保全スケジュール詳細ID</summary>
            /// <value>保全スケジュール詳細ID</value>
            public string MaintainanceScheduleDetailId { get; set; }
            /// <summary>Gets or sets 最大更新日時</summary>
            /// <value>最大更新日時</value>
            public DateTime? MaxUpdateDatetimeSchedule { get; set; }
        }

        /// <summary>
        /// 詳細画面(対象機器)のデータクラス
        /// </summary>
        public class detailMachine : ComDao.McMachineEntity
        {
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? LargeClassficationId { get; set; }
            /// <summary>Gets or sets 機種大分類名称</summary>
            /// <value>機種大分類名称</value>
            public string LargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? MiddleClassficationId { get; set; }
            /// <summary>Gets or sets 機種中分類名称</summary>
            /// <value>機種中分類名称</value>
            public string MiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? SmallClassficationId { get; set; }
            /// <summary>Gets or sets 機種小分類名称</summary>
            /// <value>機種小分類名称</value>
            public string SmallClassficationName { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 点検内容ID</summary>
            /// <value>点検内容ID</value>
            public int? InspectionContentStructureId { get; set; }
            /// <summary>Gets or sets フォロー有無</summary>
            /// <value>フォロー有無</value>
            public bool? FollowFlg { get; set; }
            /// <summary>Gets or sets フォロー予定日</summary>
            /// <value>フォロー予定日</value>
            public DateTime? FollowPlanDate { get; set; }
            /// <summary>Gets or sets フォロー内容</summary>
            /// <value>フォロー内容</value>
            public string FollowContent { get; set; }
            /// <summary>Gets or sets フォロー完了日</summary>
            /// <value>フォロー完了日</value>
            public DateTime? FollowCompletionDate { get; set; }
            /// <summary>Gets or sets 機器使用期間</summary>
            /// <value>機器使用期間</value>
            public int? UsedDaysMachine { get; set; }
            /// <summary>Gets or sets グレーアウトフラグ</summary>
            /// <value>グレーアウトフラグ</value>
            public int GrayOutFlg { get; set; }
            /// <summary>Gets or sets 保全履歴機器ID</summary>
            /// <value>保全履歴機器ID</value>
            public long? HistoryMachineId { get; set; }
            /// <summary>Gets or sets 更新シリアルID（保全履歴機器）</summary>
            /// <value>更新シリアルID（保全履歴機器）</value>
            public int? UpdateSerialidHm { get; set; }
            /// <summary>Gets or sets 保全履歴機器部位ID</summary>
            /// <value>保全履歴機器部位ID</value>
            public long? HistoryInspectionSiteId { get; set; }
            /// <summary>Gets or sets 更新シリアルID（保全履歴機器部位）</summary>
            /// <value>更新シリアルID（保全履歴機器部位）</value>
            public int? UpdateSerialidHis { get; set; }
            /// <summary>Gets or sets 保全履歴点検内容ID</summary>
            /// <value>保全履歴点検内容ID</value>
            public long? HistoryInspectionContentId { get; set; }
            /// <summary>Gets or sets 更新シリアルID（保全履歴点検内容）</summary>
            /// <value>更新シリアルID（保全履歴点検内容）</value>
            public int? UpdateSerialidHic { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long EquipmentId { get; set; }
            /// <summary>Gets or sets 保全スケジュール詳細ID</summary>
            /// <value>保全スケジュール詳細ID</value>
            public string MaintainanceScheduleDetailId { get; set; }
        }

        /// <summary>
        /// 詳細画面(故障情報)のデータクラス
        /// </summary>
        public class historyFailure : ComDao.MaHistoryFailureEntity
        {
            /// <summary>Gets or sets 原因性格階層ID</summary>
            /// <value>原因性格階層ID</value>
            public int? FailureCausePersonalityStructureId { get; set; }
            /// <summary>Gets or sets 原因性格1ID</summary>
            /// <value>原因性格1ID</value>
            public int? FailureCausePersonality1StructureId { get; set; }
            /// <summary>Gets or sets 原因性格1名称</summary>
            /// <value>原因性格1名称</value>
            public string FailureCausePersonality1StructureName { get; set; }
            /// <summary>Gets or sets 原因性格2ID</summary>
            /// <value>原因性格2ID</value>
            public int? FailureCausePersonality2StructureId { get; set; }
            /// <summary>Gets or sets 原因性格2名称</summary>
            /// <value>原因性格2名称</value>
            public string FailureCausePersonality2StructureName { get; set; }
        }

        /// <summary>
        /// 詳細画面(保全履歴概要、作業時間情報、費用情報)のデータクラス
        /// </summary>
        public class historyInfo : ComDao.MaHistoryEntity
        {
            /// <summary>Gets or sets 着工日</summary>
            /// <value>着工日</value>
            public DateTime? ConstructionDate { get; set; }
            /// <summary>Gets or sets 完了日</summary>
            /// <value>完了日</value>
            public DateTime? CompletionDate { get; set; }
            /// <summary>Gets or sets 完了時刻</summary>
            /// <value>完了時刻</value>
            public DateTime? CompletionTime { get; set; }
            /// <summary>Gets or sets 発生日</summary>
            /// <value>発生日</value>
            public DateTime? OccurrenceDate { get; set; }
        }

        /// <summary>
        /// 添付情報
        /// </summary>
        public class AttachmentInfo : ComDao.AttachmentEntity
        {
            /// <summary>Gets or sets 機能タイプIDリスト</summary>
            /// <value>機能タイプIDリスト</value>
            public List<int> FunctionTypeIdList { get; set; }
        }

        /// <summary>
        /// 登録・更新画面(件名情報、作業性格情報)のデータクラス
        /// </summary>
        public class StopSystemInfo
        {
            /// <summary>Gets or sets 系停止ID</summary>
            /// <value>系停止ID</value>
            public int? StopSystemStructureId { get; set; }
            /// <summary>Gets or sets 活動区分ID</summary>
            /// <value>活動区分ID</value>
            public int? ActivityDivision { get; set; }
            /// <summary>Gets or sets 予算性格区分ID</summary>
            /// <value>予算性格区分ID</value>
            public int? BudgetPersonalityStructureId { get; set; }
            /// <summary>Gets or sets 予算管理区分ID</summary>
            /// <value>予算管理区分ID</value>
            public int? BudgetManagementStructureId { get; set; }
        }

        /// <summary>
        /// 詳細画面(故障分析情報タブ)のデータクラス
        /// </summary>
        public class FailureAnalyzeInfo : ComDao.MaHistoryFailureEntity
        {
            /// <summary>Gets or sets 略図</summary>
            /// <value>略図</value>
            public string FileLinkDiagram { get; set; }
            /// <summary>Gets or sets 故障原因分析書</summary>
            /// <value>故障原因分析書</value>
            public string FileLinkFailure { get; set; }
        }

        /// <summary>
        /// 機器交換画面のデータクラス
        /// </summary>
        public class ReplaceMachineInfo : ComDao.McMachineEntity
        {
            /// <summary>Gets or sets 製造番号</summary>
            /// <value>製造番号</value>
            public string SerialNo { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public int? UseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 交換後の製造番号</summary>
            /// <value>交換後の製造番号</value>
            public string AfterSerialNo { get; set; }
            /// <summary>Gets or sets 交換後の使用区分</summary>
            /// <value>交換後の使用区分</value>
            public int? AfterUseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long? EquipmentId { get; set; }
            /// <summary>Gets or sets 保全履歴機器ID</summary>
            /// <value>保全履歴機器ID</value>
            public long? HistoryMachineId { get; set; }
            /// <summary>Gets or sets 保全履歴故障情報ID</summary>
            /// <value>保全履歴故障情報ID</value>
            public long? HistoryFailureId { get; set; }
        }

        /// <summary>
        /// 地区・職種の階層設定用
        /// </summary>
        /// <remarks>検索条件クラスは画面定義の関係で名称をIDとして使用しているので、共通処理が使用できないため</remarks>
        public class StructureLayerCondition
        {
            #region 共通 職種・地区設定用
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantId { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesId { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeId { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityId { get; set; }

            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobId { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? LargeClassficationId { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? MiddleClassficationId { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? SmallClassficationId { get; set; }
            #endregion
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
        }

        /// <summary>
        /// 機器検索、機器選択画面 検索条件のデータクラス
        /// </summary>
        public class searchMachineSearchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityName { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? LargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? MiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? SmallClassficationName { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public List<int> LocationStructureIdList { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public List<int> JobStructureIdList { get; set; }
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int? EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 設置年月(From)</summary>
            /// <value>設置年月(From)</value>
            public DateTime? DateOfInstallationFrom { get; set; }
            /// <summary>Gets or sets 設置年月(To)</summary>
            /// <value>設置年月(To)</value>
            public DateTime? DateOfInstallationTo { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 循環対象</summary>
            /// <value>循環対象</value>
            public int? CirculationTargetStructureId { get; set; }
            /// <summary>Gets or sets 固定資産番号</summary>
            /// <value>固定資産番号</value>
            public string FixedAssetNo { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public int? ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets メーカー型式</summary>
            /// <value>メーカー型式</value>
            public string ManufacturerType { get; set; }
            /// <summary>Gets or sets 型式コード</summary>
            /// <value>型式コード</value>
            public string ModelNo { get; set; }
            /// <summary>Gets or sets シリアル番号</summary>
            /// <value>シリアル番号</value>
            public string SerialNo { get; set; }
            /// <summary>Gets or sets 製造年月(From)</summary>
            /// <value>製造年月(From)</value>
            public DateTime? DateOfManufactureFrom { get; set; }
            /// <summary>Gets or sets 製造年月(To)</summary>
            /// <value>製造年月(To)</value>
            public DateTime? DateOfManufactureTo { get; set; }
            /// <summary>Gets or sets 点検種別毎管理</summary>
            /// <value>点検種別毎管理</value>
            public int? MaintainanceKindManageStructureId { get; set; }

            //機器選択画面で使用
            /// <summary>Gets or sets 活動区分ID</summary>
            /// <value>活動区分ID</value>
            public int? ActivityDivision { get; set; }
            /// <summary>Gets or sets 管理基準</summary>
            /// <value>管理基準の場合1、管理基準外の場合0</value>
            public string ManagementStandard { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 保全項目ID</summary>
            /// <value>保全項目ID</value>
            public int? InspectionContentStructureId { get; set; }
        }

        /// <summary>
        /// 機器検索画面　検索結果一覧のデータクラス
        /// </summary>
        public class searchMachineList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int? EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 機器レベル(表示用)</summary>
            /// <value>機器レベル(表示用)</value>
            public string EquipmentLevel { get; set; }
            /// <summary>Gets or sets 職種機種階層</summary>
            /// <value>職種機種階層</value>
            public int? JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 場所階層</summary>
            /// <value>場所階層</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 機器重要度</summary>
            /// <value>機器重要度</value>
            public string ImportanceStructureId { get; set; }
            /// <summary>Gets or sets 機器重要度(表示用)</summary>
            /// <value>機器重要度(表示用)</value>
            public string ImportanceName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? LargeClassficationId { get; set; }
            /// <summary>Gets or sets 機種大分類名称</summary>
            /// <value>機種大分類名称</value>
            public string LargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? MiddleClassficationId { get; set; }
            /// <summary>Gets or sets 機種中分類名称</summary>
            /// <value>機種中分類名称</value>
            public string MiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? SmallClassficationId { get; set; }
            /// <summary>Gets or sets 機種小分類名称</summary>
            /// <value>機種小分類名称</value>
            public string SmallClassficationName { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public int? InspectionSiteConservationStructureId { get; set; }
            /// <summary>Gets or sets 保全方式(表示用)</summary>
            /// <value>保全方式(表示用)</value>
            public string InspectionSiteConservationName { get; set; }
            /// <summary>Gets or sets 製造番号</summary>
            /// <value>製造番号</value>
            public string SerialNo { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets メーカー(表示用)</summary>
            /// <value>メーカー(表示用)</value>
            public string ManufacturerName { get; set; }
            /// <summary>Gets or sets メーカー型式</summary>
            /// <value>メーカー型式</value>
            public string ManufacturerType { get; set; }
            /// <summary>Gets or sets 製造年月</summary>
            /// <value>製造年月</value>
            public DateTime? DateOfManufacture { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public int? UseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 使用区分(表示用)</summary>
            /// <value>使用区分(表示用)</value>
            public string UseSegmentName { get; set; }
            /// <summary>Gets or sets 設置年月</summary>
            /// <value>設置年月</value>
            public DateTime? DateOfInstallation { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
        }

        /// <summary>
        /// 機器選択画面　初期表示時の検索条件（場所階層）
        /// </summary>
        public class SelectMachineStructureCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
        }

        /// <summary>
        /// 機器選択画面の検索結果
        /// </summary>
        public class SelectMachineResult : detailMachine
        {
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 重要度(表示用)</summary>
            /// <value>重要度(表示用)</value>
            public string ImportanceName { get; set; }
        }

        /// <summary>
        /// 依頼票用のデータクラス
        /// </summary>
        public class requestReport
        {
            /// <summary>Gets or sets 依頼ID</summary>
            /// <value>依頼ID</value>
            public string RequestId { get; set; }
        }

        /// <summary>
        /// 故障原因分析書用のデータクラス
        /// </summary>
        public class failureReport
        {
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public string SummaryId { get; set; }
        }

        /// <summary>
        /// 件名別長期計画・機器別長期計画の白丸「○」リンクから遷移時の初期値のデータクラス
        /// </summary>
        public class searchResultFromLongPlan
        {
            /// <summary>Gets or sets 保全スケジュール詳細ID</summary>
            /// <value>保全スケジュール詳細ID</value>
            public string MaintainanceScheduleDetailId { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets 件名メモ</summary>
            /// <value>件名メモ</value>
            public string SubjectNote { get; set; }
            /// <summary>Gets or sets 予算性格区分ID</summary>
            /// <value>予算性格区分ID</value>
            public int? BudgetPersonalityStructureId { get; set; }
            /// <summary>Gets or sets 予算管理区分ID</summary>
            /// <value>予算管理区分ID</value>
            public int? BudgetManagementStructureId { get; set; }
            /// <summary>Gets or sets 発行日</summary>
            /// <value>発行日</value>
            public DateTime? IssueDate { get; set; }
            /// <summary>Gets or sets 保全時期ID</summary>
            /// <value>保全時期ID</value>
            public int? MaintenanceSeasonStructureId { get; set; }
            /// <summary>Gets or sets 施工担当者ID</summary>
            /// <value>施工担当者ID</value>
            public int? ConstructionPersonnelId { get; set; }
            /// <summary>Gets or sets 最大更新日時</summary>
            /// <value>最大更新日時</value>
            public DateTime? MaxUpdateDatetimeSchedule { get; set; }

            #region 場所・職種情報
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? LargeClassficationId { get; set; }
            /// <summary>Gets or sets 機種大分類名称</summary>
            /// <value>機種大分類名称</value>
            public string LargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? MiddleClassficationId { get; set; }
            /// <summary>Gets or sets 機種中分類名称</summary>
            /// <value>機種中分類名称</value>
            public string MiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? SmallClassficationId { get; set; }
            /// <summary>Gets or sets 機種小分類名称</summary>
            /// <value>機種小分類名称</value>
            public string SmallClassficationName { get; set; }
            #endregion
        }

        /// <summary>
        /// ExcelPort 保全活動のデータクラス
        /// </summary>
        public class excelPortMaintenance : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public string JobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 依頼No.</summary>
            /// <value>依頼No.</value>
            public string RequestNo { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets 作業計画・実施内容</summary>
            /// <value>作業計画・実施内容</value>
            public string PlanImplementationContent { get; set; }
            /// <summary>Gets or sets 件名メモ</summary>
            /// <value>件名メモ</value>
            public string SubjectNote { get; set; }
            /// <summary>Gets or sets MQ分類ID</summary>
            /// <value>MQ分類ID</value>
            public int? MqClassStructureId { get; set; }
            /// <summary>Gets or sets MQ分類(表示用)</summary>
            /// <value>MQ分類(表示用)</value>
            public string MqClassName { get; set; }
            /// <summary>Gets or sets 修繕費分類ID</summary>
            /// <value>修繕費分類ID</value>
            public int? RepairCostClassStructureId { get; set; }
            /// <summary>Gets or sets 修繕費分類(表示用)</summary>
            /// <value>修繕費分類(表示用)</value>
            public string RepairCostClassName { get; set; }
            /// <summary>Gets or sets 予算管理区分ID</summary>
            /// <value>予算管理区分ID</value>
            public int? BudgetManagementStructureId { get; set; }
            /// <summary>Gets or sets 予算管理区分(表示用)</summary>
            /// <value>予算管理区分(表示用)</value>
            public string BudgetManagementName { get; set; }
            /// <summary>Gets or sets 予算性格区分ID</summary>
            /// <value>予算性格区分ID</value>
            public int? BudgetPersonalityStructureId { get; set; }
            /// <summary>Gets or sets 予算性格区分(表示用)</summary>
            /// <value>予算性格区分(表示用)</value>
            public string BudgetPersonalityName { get; set; }
            /// <summary>Gets or sets 突発区分ID</summary>
            /// <value>突発区分ID</value>
            public int? SuddenDivisionStructureId { get; set; }
            /// <summary>Gets or sets 突発区分(表示用)</summary>
            /// <value>突発区分(表示用)</value>
            public string SuddenDivisionName { get; set; }
            /// <summary>Gets or sets 系停止ID</summary>
            /// <value>系停止ID</value>
            public int? StopSystemStructureId { get; set; }
            /// <summary>Gets or sets 系停止(表示用)</summary>
            /// <value>系停止(表示用)</value>
            public string StopSystemName { get; set; }
            /// <summary>Gets or sets 系停止時間(Hr)</summary>
            /// <value>系停止時間(Hr)</value>
            public decimal? StopTime { get; set; }
            /// <summary>Gets or sets カウント件数</summary>
            /// <value>カウント件数</value>
            public int? MaintenanceCount { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public int? ChangeManagementStructureId { get; set; }
            /// <summary>Gets or sets 変更管理(表示用)</summary>
            /// <value>変更管理(表示用)</value>
            public string ChangeManagementName { get; set; }
            /// <summary>Gets or sets 環境安全管理区分ID</summary>
            /// <value>環境安全管理区分ID</value>
            public int? EnvSafetyManagementStructureId { get; set; }
            /// <summary>Gets or sets 環境安全管理区分(表示用)</summary>
            /// <value>環境安全管理区分(表示用)</value>
            public string EnvSafetyManagementName { get; set; }
            /// <summary>Gets or sets 依頼内容</summary>
            /// <value>依頼内容</value>
            public string RequestContent { get; set; }
            /// <summary>Gets or sets 発行日</summary>
            /// <value>発行日</value>
            public DateTime? IssueDate { get; set; }
            /// <summary>Gets or sets 緊急度ID</summary>
            /// <value>緊急度ID</value>
            public int? UrgencyStructureId { get; set; }
            /// <summary>Gets or sets 緊急度(表示用)</summary>
            /// <value>緊急度(表示用)</value>
            public string UrgencyStructureName { get; set; }
            /// <summary>Gets or sets 発見方法ID</summary>
            /// <value>発見方法ID</value>
            public int? DiscoveryMethodsStructureId { get; set; }
            /// <summary>Gets or sets 発見方法(表示用)</summary>
            /// <value>発見方法(表示用)</value>
            public string DiscoveryMethodsName { get; set; }
            /// <summary>Gets or sets 着工希望日</summary>
            /// <value>着工希望日</value>
            public DateTime? DesiredStartDate { get; set; }
            /// <summary>Gets or sets 完了希望日</summary>
            /// <value>完了希望日</value>
            public DateTime? DesiredEndDate { get; set; }
            /// <summary>Gets or sets 依頼部課係ID</summary>
            /// <value>依頼部課係ID</value>
            public int? RequestDepartmentClerkId { get; set; }
            /// <summary>Gets or sets 依頼部課係(表示用)</summary>
            /// <value>依頼部課係(表示用)</value>
            public string RequestDepartmentClerkName { get; set; }
            /// <summary>Gets or sets 依頼担当者ID</summary>
            /// <value>依頼担当者ID</value>
            public int? RequestPersonnelId { get; set; }
            /// <summary>Gets or sets 依頼担当者名</summary>
            /// <value>依頼担当者名</value>
            public string RequestPersonnelName { get; set; }
            /// <summary>Gets or sets 依頼担当者TEL</summary>
            /// <value>依頼担当者TEL</value>
            public string RequestPersonnelTel { get; set; }
            /// <summary>Gets or sets 依頼係長ID</summary>
            /// <value>依頼係長ID</value>
            public int? RequestDepartmentChiefId { get; set; }
            /// <summary>Gets or sets 依頼係長名</summary>
            /// <value>依頼係長名</value>
            public string RequestDepartmentChiefName { get; set; }
            /// <summary>Gets or sets 依頼課長ID</summary>
            /// <value>依頼課長ID</value>
            public int? RequestDepartmentManagerId { get; set; }
            /// <summary>Gets or sets 依頼課長名</summary>
            /// <value>依頼課長名</value>
            public string RequestDepartmentManagerName { get; set; }
            /// <summary>Gets or sets 依頼職長ID</summary>
            /// <value>依頼職長ID</value>
            public int? RequestDepartmentForemanId { get; set; }
            /// <summary>Gets or sets 依頼職長名</summary>
            /// <value>依頼職長名</value>
            public string RequestDepartmentForemanName { get; set; }
            /// <summary>Gets or sets 保全部課係ID</summary>
            /// <value>保全部課係ID</value>
            public int? MaintenanceDepartmentClerkId { get; set; }
            /// <summary>Gets or sets 保全部課係(表示用)</summary>
            /// <value>保全部課係(表示用)</value>
            public string MaintenanceDepartmentClerkName { get; set; }
            /// <summary>Gets or sets 依頼事由</summary>
            /// <value>依頼事由</value>
            public string RequestReason { get; set; }
            /// <summary>Gets or sets 件名検討結果</summary>
            /// <value>件名検討結果</value>
            public string ExaminationResult { get; set; }
            /// <summary>Gets or sets 工事区分ID</summary>
            /// <value>工事区分ID</value>
            public int? ConstructionDivisionStructureId { get; set; }
            /// <summary>Gets or sets 工事区分(表示用)</summary>
            /// <value>工事区分(表示用)</value>
            public string ConstructionDivisionStructureName { get; set; }
            /// <summary>Gets or sets 実施件名</summary>
            /// <value>実施件名</value>
            public string PlanSubject { get; set; }
            /// <summary>Gets or sets 発生日</summary>
            /// <value>発生日</value>
            public DateTime? OccurrenceDate { get; set; }
            /// <summary>Gets or sets 着工予定日</summary>
            /// <value>着工予定日</value>
            public DateTime? ExpectedConstructionDate { get; set; }
            /// <summary>Gets or sets 完了予定日</summary>
            /// <value>完了予定日</value>
            public DateTime? ExpectedCompletionDate { get; set; }
            /// <summary>Gets or sets 全体予算金額</summary>
            /// <value>全体予算金額</value>
            public decimal? TotalBudgetCost { get; set; }
            /// <summary>Gets or sets 予定工数</summary>
            /// <value>予定工数</value>
            public decimal? PlanManHour { get; set; }
            /// <summary>Gets or sets 自・他責ID</summary>
            /// <value>自・他責ID</value>
            public int? ResponsibilityStructureId { get; set; }
            /// <summary>Gets or sets 自・他責(表示用)</summary>
            /// <value>自・他責(表示用)</value>
            public string ResponsibilityName { get; set; }
            /// <summary>Gets or sets 故障影響</summary>
            /// <value>故障影響</value>
            public string FailureEffect { get; set; }
            /// <summary>Gets or sets 着工日</summary>
            /// <value>着工日</value>
            public DateTime? ConstructionDate { get; set; }
            /// <summary>Gets or sets 完了日</summary>
            /// <value>完了日</value>
            public DateTime? CompletionDate { get; set; }
            /// <summary>Gets or sets 保全時期ID</summary>
            /// <value>保全時期ID</value>
            public int? MaintenanceSeasonStructureId { get; set; }
            /// <summary>Gets or sets 保全時期(表示用)</summary>
            /// <value>保全時期(表示用)</value>
            public string MaintenanceSeasonName { get; set; }
            /// <summary>Gets or sets 呼出回数</summary>
            /// <value>呼出回数</value>
            public int? CallCount { get; set; }
            /// <summary>Gets or sets 施工会社</summary>
            /// <value>施工会社</value>
            public string ConstructionCompany { get; set; }
            /// <summary>Gets or sets 施工担当者ID</summary>
            /// <value>施工担当者ID</value>
            public int? ConstructionPersonnelId { get; set; }
            /// <summary>Gets or sets 施工担当者名 </summary>
            /// <value>施工担当者名 </value>
            public string ConstructionPersonnelName { get; set; }
            /// <summary>Gets or sets 実績結果</summary>
            /// <value>実績結果</value>
            public int? ActualResultStructureId { get; set; }
            /// <summary>Gets or sets 実績結果(表示用)</summary>
            /// <value>実績結果(表示用)</value>
            public string ActualResultName { get; set; }
            /// <summary>Gets or sets 休損量</summary>
            /// <value>休損量</value>
            public int? LossAbsence { get; set; }
            /// <summary>Gets or sets 休損型数</summary>
            /// <value>休損型数</value>
            public int? LossAbsenceTypeCount { get; set; }
            /// <summary>Gets or sets 保全見解</summary>
            /// <value>保全見解</value>
            public string MaintenanceOpinion { get; set; }
            /// <summary>Gets or sets 自係(Hr)</summary>
            /// <value>自係(Hr)</value>
            public decimal? WorkingTimeSelf { get; set; }
            /// <summary>Gets or sets 作業時間(施工会社)</summary>
            /// <value>作業時間(施工会社)</value>
            public decimal? WorkingTimeCompany { get; set; }
            /// <summary>Gets or sets 総計(Hr)</summary>
            /// <value>総計(Hr)</value>
            public decimal? TotalWorkingTime { get; set; }
            /// <summary>Gets or sets 費用メモ</summary>
            /// <value>費用メモ</value>
            public string CostNote { get; set; }
            /// <summary>Gets or sets 実績金額</summary>
            /// <value>実績金額</value>
            public decimal? Expenditure { get; set; }
            /// <summary>Gets or sets 製造担当者ID</summary>
            /// <value>製造担当者ID</value>
            public int? ManufacturingPersonnelId { get; set; }
            /// <summary>Gets or sets 製造担当者名</summary>
            /// <value>製造担当者名</value>
            public string ManufacturingPersonnelName { get; set; }
            /// <summary>Gets or sets 作業・故障区分ID</summary>
            /// <value>作業・故障区分ID</value>
            public int? WorkFailureDivisionStructureId { get; set; }
            /// <summary>Gets or sets 作業・故障区分(表示用)</summary>
            /// <value>作業・故障区分(表示用)</value>
            public string WorkFailureDivisionName { get; set; }
            /// <summary>Gets or sets 発生時刻</summary>
            /// <value>発生時刻</value>
            public DateTime? OccurrenceTime { get; set; }
            /// <summary>Gets or sets 完了時刻</summary>
            /// <value>完了時刻</value>
            public DateTime? CompletionTime { get; set; }
            /// <summary>Gets or sets 系停止回数</summary>
            /// <value>系停止回数</value>
            public int? StopCount { get; set; }
            /// <summary>Gets or sets 発見者</summary>
            /// <value>発見者</value>
            public string DiscoveryPersonnel { get; set; }
            /// <summary>Gets or sets 生産への影響ID</summary>
            /// <value>生産への影響ID</value>
            public int? EffectProductionStructureId { get; set; }
            /// <summary>Gets or sets 生産への影響(表示用)</summary>
            /// <value>生産への影響(表示用)</value>
            public string EffectProductionName { get; set; }
            /// <summary>Gets or sets 品質への影響ID</summary>
            /// <value>品質への影響ID</value>
            public int? EffectQualityStructureId { get; set; }
            /// <summary>Gets or sets 品質への影響(表示用)</summary>
            /// <value>品質への影響(表示用)</value>
            public string EffectQualityName { get; set; }
            /// <summary>Gets or sets 故障部位</summary>
            /// <value>故障部位</value>
            public string FailureSite { get; set; }
            /// <summary>Gets or sets 予備品有無</summary>
            /// <value>予備品有無</value>
            public bool? PartsExistenceFlg { get; set; }
            /// <summary>Gets or sets 調査時間</summary>
            /// <value>調査時間</value>
            public decimal? WorkingTimeResearch { get; set; }
            /// <summary>Gets or sets 調達時間</summary>
            /// <value>調達時間</value>
            public decimal? WorkingTimeProcure { get; set; }
            /// <summary>Gets or sets 修復時間</summary>
            /// <value>修復時間</value>
            public decimal? WorkingTimeRepair { get; set; }
            /// <summary>Gets or sets 試運転時間</summary>
            /// <value>試運転時間</value>
            public decimal? WorkingTimeTest { get; set; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long SummaryId { get; set; }
            /// <summary>Gets or sets 活動区分ID</summary>
            /// <value>活動区分ID</value>
            public int? ActivityDivision { get; set; }
            /// <summary>Gets or sets 活動区分(表示用)</summary>
            /// <value>活動区分(表示用)</value>
            public string ActivityDivisionName { get; set; }
        }

    }
}
