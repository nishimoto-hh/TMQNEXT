using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonTMQUtil.TMQCommonDataClass;
using IListAccessor = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase.AccessorUtil.IListAccessor;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using BusinessLogicBase = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase;

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
        public class searchResult : ComDao.CommonTableItem, IListAccessor
        {
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
            /// <summary>Gets or sets 場所</summary>
            /// <value>場所</value>
            public string ConstructionPlace { get; set; }
            /// <summary>Gets or sets 施工担当者</summary>
            /// <value>施工担当者</value>
            public string FailureEquipmentModelStructureId { get; set; }
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
            /// <summary>Gets or sets 発行日</summary>
            /// <value>発行日</value>
            public DateTime IssueDate { get; set; }



            /// <summary>
            /// 一時テーブルレイアウト作成処理(性能改善対応)
            /// </summary>
            /// <param name="mapDic">マッピング情報のディクショナリ</param>
            /// <returns>一時テーブルレイアウト</returns>
            public dynamic GetTmpTableData(Dictionary<string, ComUtil.DBMappingInfo> mapDic)
            {
                dynamic paramObj;

                paramObj = new ExpandoObject() as IDictionary<string, object>;
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.OccurrenceDate, nameof(this.OccurrenceDate), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.CompletionDate, nameof(this.CompletionDate), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.Subject, nameof(this.Subject), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MqClassStructureId, nameof(this.MqClassStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MqClassName, nameof(this.MqClassName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MachineNo, nameof(this.MachineNo), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MachineName, nameof(this.MachineName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationStructureId, nameof(this.LocationStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DistrictId, nameof(this.DistrictId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DistrictName, nameof(this.DistrictName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FactoryId, nameof(this.FactoryId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FactoryName, nameof(this.FactoryName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PlantId, nameof(this.PlantId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PlantName, nameof(this.PlantName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SeriesId, nameof(this.SeriesId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SeriesName, nameof(this.SeriesName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StrokeId, nameof(this.StrokeId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StrokeName, nameof(this.StrokeName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FacilityId, nameof(this.FacilityId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FacilityName, nameof(this.FacilityName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobStructureId, nameof(this.JobStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobId, nameof(this.JobId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobName, nameof(this.JobName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LargeClassficationId, nameof(this.LargeClassficationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LargeClassficationName, nameof(this.LargeClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MiddleClassficationId, nameof(this.MiddleClassficationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MiddleClassficationName, nameof(this.MiddleClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SmallClassficationId, nameof(this.SmallClassficationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SmallClassficationName, nameof(this.SmallClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StopSystemStructureId, nameof(this.StopSystemStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StopSystemName, nameof(this.StopSystemName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StopTime, nameof(this.StopTime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StopTimeDisp, nameof(this.StopTimeDisp), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.CostNote, nameof(this.CostNote), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SuddenDivisionStructureId, nameof(this.SuddenDivisionStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SuddenDivisionName, nameof(this.SuddenDivisionName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ExpectedConstructionDate, nameof(this.ExpectedConstructionDate), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FileLinkFailure, nameof(this.FileLinkFailure), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.BudgetManagementStructureId, nameof(this.BudgetManagementStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.BudgetManagementName, nameof(this.BudgetManagementName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.BudgetPersonalityStructureId, nameof(this.BudgetPersonalityStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.BudgetPersonalityName, nameof(this.BudgetPersonalityName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TotalBudgetCost, nameof(this.TotalBudgetCost), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TotalBudgetCostDisp, nameof(this.TotalBudgetCostDisp), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MaintenanceSeasonStructureId, nameof(this.MaintenanceSeasonStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MaintenanceSeasonName, nameof(this.MaintenanceSeasonName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.RequestPersonnelName, nameof(this.RequestPersonnelName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ConstructionPersonnelName, nameof(this.ConstructionPersonnelName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TotalWorkingTime, nameof(this.TotalWorkingTime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TotalWorkingTimeDisp, nameof(this.TotalWorkingTimeDisp), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.WorkingTimeSelf, nameof(this.WorkingTimeSelf), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.WorkingTimeSelfDisp, nameof(this.WorkingTimeSelfDisp), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DiscoveryMethodsStructureId, nameof(this.DiscoveryMethodsStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DiscoveryMethodsName, nameof(this.DiscoveryMethodsName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ActualResultStructureId, nameof(this.ActualResultStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ActualResultName, nameof(this.ActualResultName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ConstructionCompany, nameof(this.ConstructionCompany), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MaintenanceCount, nameof(this.MaintenanceCount), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PlanImplementationContent, nameof(this.PlanImplementationContent), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SubjectNote, nameof(this.SubjectNote), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FileLinkSubject, nameof(this.FileLinkSubject), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MaintenanceSiteName, nameof(this.MaintenanceSiteName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MaintenanceContentName, nameof(this.MaintenanceContentName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.Expenditure, nameof(this.Expenditure), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ExpenditureDisp, nameof(this.ExpenditureDisp), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PhenomenonStructureId, nameof(this.PhenomenonStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PhenomenonName, nameof(this.PhenomenonName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PhenomenonNote, nameof(this.PhenomenonNote), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureCauseStructureId, nameof(this.FailureCauseStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureCauseName, nameof(this.FailureCauseName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureCauseNote, nameof(this.FailureCauseNote), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureCausePersonalityStructureId, nameof(this.FailureCausePersonalityStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureCausePersonality1StructureId, nameof(this.FailureCausePersonality1StructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureCausePersonality1StructureName, nameof(this.FailureCausePersonality1StructureName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureCausePersonality2StructureId, nameof(this.FailureCausePersonality2StructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureCausePersonality2StructureName, nameof(this.FailureCausePersonality2StructureName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureCausePersonalityNote, nameof(this.FailureCausePersonalityNote), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TreatmentMeasureStructureId, nameof(this.TreatmentMeasureStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TreatmentMeasureName, nameof(this.TreatmentMeasureName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureCauseAdditionNote, nameof(this.FailureCauseAdditionNote), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureStatus, nameof(this.FailureStatus), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PreviousSituation, nameof(this.PreviousSituation), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.RecoveryAction, nameof(this.RecoveryAction), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ImprovementMeasure, nameof(this.ImprovementMeasure), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SystemFeedBack, nameof(this.SystemFeedBack), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.Lesson, nameof(this.Lesson), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FailureNote, nameof(this.FailureNote), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FollowFlg, nameof(this.FollowFlg), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FollowPlanDate, nameof(this.FollowPlanDate), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FollowPlanDateDisp, nameof(this.FollowPlanDateDisp), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FollowContent, nameof(this.FollowContent), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.RequestNo, nameof(this.RequestNo), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ProgressId, nameof(this.ProgressId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ProgressName, nameof(this.ProgressName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SummaryId, nameof(this.SummaryId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateSerialid, nameof(this.UpdateSerialid), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.InsertDatetime, nameof(this.InsertDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.InsertUserId, nameof(this.InsertUserId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateDatetime, nameof(this.UpdateDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateUserId, nameof(this.UpdateUserId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DeleteFlg, nameof(this.DeleteFlg), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LanguageId, nameof(this.LanguageId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ConstructionPlace, nameof(this.ConstructionPlace), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.IssueDate, nameof(this.IssueDate), mapDic);



                return paramObj;
            }
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
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            /// <remarks>原因性格の翻訳取得に使用</remarks>
            public int? FactoryId { get; set; }
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
            /// <summary>Gets or sets ランク</summary>
            /// <value>ランク</value>
            public int? RankStructureId { get; set; }
            /// <summary>Gets or sets 故障機器</summary>
            /// <value>故障機器</value>
            public int? FailureEquipmentModelStructureId { get; set; }
            /// <summary>Gets or sets 履歴重要度</summary>
            /// <value>履歴重要度</value>
            public int? HistoryImportanceStructureId { get; set; }
            /// <summary>Gets or sets 履歴保全方式</summary>
            /// <value>履歴保全方式</value>
            public int? HistoryConservationStructureId { get; set; }



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
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long? SummaryId { get; set; }
            /// <summary>Gets or sets 長期計画件名ID</summary>
            /// <value>長期計画件名ID</value>
            public long? LongPlanId { get; set; }
            /// <summary>Gets or sets 活動区分ID</summary>
            /// <value>活動区分ID</value>
            public int? ActivityDivision { get; set; }
            /// <summary>Gets or sets フォロー計画キーID</summary>
            /// <value>フォロー計画キーID</value>
            public long? FollowPlanKeyId { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets 作業計画・実施内容</summary>
            /// <value>作業計画・実施内容</value>
            public string PlanImplementationContent { get; set; }
            /// <summary>Gets or sets 件名メモ</summary>
            /// <value>件名メモ</value>
            public string SubjectNote { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? LocationDistrictStructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? LocationFactoryStructureId { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? LocationPlantStructureId { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? LocationSeriesStructureId { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? LocationStrokeStructureId { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? LocationFacilityStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
            /// <summary>Gets or sets MQ分類ID</summary>
            /// <value>MQ分類ID</value>
            public int? MqClassStructureId { get; set; }
            /// <summary>Gets or sets 修繕費分類ID</summary>
            /// <value>修繕費分類ID</value>
            public int? RepairCostClassStructureId { get; set; }
            /// <summary>Gets or sets 予算性格区分ID</summary>
            /// <value>予算性格区分ID</value>
            public int? BudgetPersonalityStructureId { get; set; }
            /// <summary>Gets or sets 予算管理区分ID</summary>
            /// <value>予算管理区分ID</value>
            public int? BudgetManagementStructureId { get; set; }
            /// <summary>Gets or sets 突発区分ID</summary>
            /// <value>突発区分ID</value>
            public int? SuddenDivisionStructureId { get; set; }
            /// <summary>Gets or sets 系停止ID</summary>
            /// <value>系停止ID</value>
            public int? StopSystemStructureId { get; set; }
            /// <summary>Gets or sets 系停止時間</summary>
            /// <value>系停止時間</value>
            public decimal? StopTime { get; set; }
            /// <summary>Gets or sets カウント件数</summary>
            /// <value>カウント件数</value>
            public int? MaintenanceCount { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public int? ChangeManagementStructureId { get; set; }
            /// <summary>Gets or sets 環境安全管理区分ID</summary>
            /// <value>環境安全管理区分ID</value>
            public int? EnvSafetyManagementStructureId { get; set; }
            /// <summary>Gets or sets 着工日</summary>
            /// <value>着工日</value>
            public DateTime? ConstructionDate { get; set; }
            /// <summary>Gets or sets 完了日</summary>
            /// <value>完了日</value>
            public DateTime? CompletionDate { get; set; }
            /// <summary>Gets or sets 完了時刻</summary>
            /// <value>完了時刻</value>
            public DateTime? CompletionTime { get; set; }
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
            /// <summary>Gets or sets 依頼No.</summary>
            /// <value>依頼No.</value>
            public string RequestNo { get; set; }
            /// <summary>Gets or sets MQ分類(表示用)</summary>
            /// <value>MQ分類(表示用)</value>
            public string MqClassName { get; set; }
            /// <summary>Gets or sets 修繕費分類(表示用)</summary>
            /// <value>修繕費分類(表示用)</value>
            public string RepairCostClassName { get; set; }
            /// <summary>Gets or sets 予算管理区分(表示用)</summary>
            /// <value>予算管理区分(表示用)</value>
            public string BudgetManagementName { get; set; }
            /// <summary>Gets or sets 予算性格区分(表示用)</summary>
            /// <value>予算性格区分(表示用)</value>
            public string BudgetPersonalityName { get; set; }
            /// <summary>Gets or sets 突発区分(表示用)</summary>
            /// <value>突発区分(表示用)</value>
            public string SuddenDivisionName { get; set; }
            /// <summary>Gets or sets 系停止(表示用)</summary>
            /// <value>系停止(表示用)</value>
            public string StopSystemName { get; set; }
            /// <summary>Gets or sets 変更管理(表示用)</summary>
            /// <value>変更管理(表示用)</value>
            public string ChangeManagementName { get; set; }
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
            public string UrgencyName { get; set; }
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
            public string ConstructionDivisionName { get; set; }
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
            /// <summary>Gets or sets 完了日(個別工場)</summary>
            /// <value>完了日(個別工場)</value>
            public DateTime? CompletionDateIndividual { get; set; }
            /// <summary>Gets or sets 保全時期ID</summary>
            /// <value>保全時期ID</value>
            public int? MaintenanceSeasonStructureId { get; set; }
            /// <summary>Gets or sets 保全時期(表示用)</summary>
            /// <value>保全時期(表示用)</value>
            public string MaintenanceSeasonName { get; set; }
            /// <summary>Gets or sets 呼出回数</summary>
            /// <value>呼出回数</value>
            public int? CallCount { get; set; }
            /// <summary>Gets or sets 呼出ID</summary>
            /// <value>呼出ID</value>
            public int? CallCountId { get; set; }
            /// <summary>Gets or sets 呼出(表示用)</summary>
            /// <value>呼出(表示用)</value>
            public string CallCountName { get; set; }
            /// <summary>Gets or sets 施工会社</summary>
            /// <value>施工会社</value>
            public string ConstructionCompany { get; set; }
            /// <summary>Gets or sets 施工担当者ID</summary>
            /// <value>施工担当者ID</value>
            public int? ConstructionPersonnelId { get; set; }
            /// <summary>Gets or sets 施工担当者ID(個別工場)</summary>
            /// <value>施工担当者ID(個別工場)</value>
            public int? ConstructionPersonnelIdIndividual { get; set; }
            /// <summary>Gets or sets 施工担当者名</summary>
            /// <value>施工担当者名</value>
            public string ConstructionPersonnelName { get; set; }
            /// <summary>Gets or sets 施工担当者名(個別工場)</summary>
            /// <value>施工担当者名(個別工場)</value>
            public string ConstructionPersonnelNameIndividual { get; set; }
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
            /// <summary>Gets or sets 作業時間(施工会社)(個別工場)</summary>
            /// <value>作業時間(施工会社)(個別工場)</value>
            public decimal? WorkingTimeCompanyIndividual { get; set; }
            /// <summary>Gets or sets 総計(Hr)</summary>
            /// <value>総計(Hr)</value>
            public decimal? TotalWorkingTime { get; set; }
            /// <summary>Gets or sets 修理時間(Hr)(個別工場)</summary>
            /// <value>修理時間(Hr)(個別工場)</value>
            public decimal? TotalWorkingTimeIndividual { get; set; }
            /// <summary>Gets or sets 費用メモ</summary>
            /// <value>費用メモ</value>
            public string CostNote { get; set; }
            /// <summary>Gets or sets 実績金額</summary>
            /// <value>実績金額</value>
            public decimal? Expenditure { get; set; }
            /// <summary>Gets or sets 実績金額(任意)(個別工場)</summary>
            /// <value>実績金額(任意)(個別工場)</value>
            public decimal? ExpenditureIndividual { get; set; }
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
            public int? PartsExistenceFlg { get; set; }
            /// <summary>Gets or sets 予備品有無(表示用)</summary>
            /// <value>予備品有無(表示用)</value>
            public string PartsExistenceName { get; set; }
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
            /// <summary>Gets or sets 活動区分(表示用)</summary>
            /// <value>活動区分(表示用)</value>
            public string ActivityDivisionName { get; set; }
            /// <summary>Gets or sets 個別工場制御用フラグ</summary>
            /// <value>個別工場制御用フラグ</value>
            public string ControlFlag { get; set; }
            /// <summary>Gets or sets 製造権限有無</summary>
            /// <value>製造権限有無</value>
            public int Manufacturing { get; set; }
            /// <summary>Gets or sets 保全権限有無</summary>
            /// <value>保全権限有無</value>
            public int Maintenance { get; set; }
            /// <summary>Gets or sets 保全依頼情報入力有無フラグ</summary>
            /// <value>保全依頼情報入力有無フラグ</value>
            public int RequestInputFlg { get; set; }
            /// <summary>Gets or sets 保全計画情報入力有無フラグ</summary>
            /// <value>保全計画情報入力有無フラグ</value>
            public int PlanInputFlg { get; set; }
            /// <summary>Gets or sets 保全履歴情報入力有無フラグ</summary>
            /// <value>保全履歴情報入力有無フラグ</value>
            public int HistoryInputFlg { get; set; }
            /// <summary>Gets or sets 場所</summary>
            /// <value>場所</value>
            public int ConstructionPlace { get; set; }
            /// <summary>Gets or sets 故障機種</summary>
            /// <value>故障機種</value>
            public int FailureEquipmentModelStructureId { get; set; }

            #region 登録時に必要な項目定義
            /// <summary>Gets or sets 承認者１ID</summary>
            /// <value>承認者１ID</value>
            public int? RequestAuthorizer1Id { get; set; }
            /// <summary>Gets or sets 承認者２ID</summary>
            /// <value>承認者２ID</value>
            public int? RequestAuthorizer2Id { get; set; }
            /// <summary>Gets or sets 承認者３ID</summary>
            /// <value>承認者３ID</value>
            public int? RequestAuthorizer3Id { get; set; }
            /// <summary>Gets or sets 依頼ID</summary>
            /// <value>依頼ID</value>
            public long? RequestId { get; set; }
            /// <summary>Gets or sets 保全計画ID</summary>
            /// <value>保全計画ID</value>
            public long? PlanId { get; set; }
            /// <summary>Gets or sets 履歴ID</summary>
            /// <value>履歴ID</value>
            public long? HistoryId { get; set; }
            /// <summary>Gets or sets 保全履歴故障情報ID</summary>
            /// <value>保全履歴故障情報ID</value>
            public long? HistoryFailureId { get; set; }
            /// <summary>Gets or sets フォロー有無</summary>
            /// <value>フォロー有無</value>
            public bool? FollowFlg { get; set; }
            #endregion
        }

        /// <summary>
        /// ExcelPort 保全活動_故障情報のデータクラス
        /// </summary>
        public class excelPortHistoryFailure : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            #region 地区・職種機種(保全活動件名)
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
            #endregion
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
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long? EquipmentId { get; set; }
            /// <summary>Gets or sets 機器番号 機器名称</summary>
            /// <value>機器番号 機器名称</value>
            public string Machine { get; set; }
            #region 地区・職種機種(機器)
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? MachineLocationStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? MachineJobStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? MachineDistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string MachineDistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? MachineFactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string MachineFactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? MachinePlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string MachinePlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? MachineSeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string MachineSeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? MachineStrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string MachineStrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? MachineFacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string MachineFacilityName { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int MachineJobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string MachineJobName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? MachineLargeClassficationId { get; set; }
            /// <summary>Gets or sets 機種大分類名称</summary>
            /// <value>機種大分類名称</value>
            public string MachineLargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? MachineMiddleClassficationId { get; set; }
            /// <summary>Gets or sets 機種中分類名称</summary>
            /// <value>機種中分類名称</value>
            public string MachineMiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? MachineSmallClassficationId { get; set; }
            /// <summary>Gets or sets 機種小分類名称</summary>
            /// <value>機種小分類名称</value>
            public string MachineSmallClassficationName { get; set; }
            #endregion
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int? EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 機器レベル(表示用)</summary>
            /// <value>機器レベル(表示用)</value>
            public string EquipmentLevelName { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public int? ConservationStructureId { get; set; }
            /// <summary>Gets or sets 保全方式(表示用)</summary>
            /// <value>保全方式(表示用)</value>
            public string ConservationName { get; set; }
            /// <summary>Gets or sets 重要度ID</summary>
            /// <value>重要度ID</value>
            public int? ImportanceStructureId { get; set; }
            /// <summary>Gets or sets 重要度(表示用)</summary>
            /// <value>重要度(表示用)</value>
            public string ImportanceName { get; set; }
            /// <summary>Gets or sets 機器使用日数</summary>
            /// <value>機器使用日数</value>
            public int? UsedDaysMachine { get; set; }
            /// <summary>Gets or sets 保全部位</summary>
            /// <value>保全部位</value>
            public string MaintenanceSite { get; set; }
            /// <summary>Gets or sets 保全内容</summary>
            /// <value>保全内容</value>
            public string MaintenanceContent { get; set; }
            /// <summary>Gets or sets フォロー有無</summary>
            /// <value>フォロー有無</value>
            public int? FollowFlg { get; set; }
            /// <summary>Gets or sets フォロー有無(表示用)</summary>
            /// <value>フォロー有無(表示用)</value>
            public string FollowFlgName { get; set; }
            /// <summary>Gets or sets フォロー予定日</summary>
            /// <value>フォロー予定日</value>
            public DateTime? FollowPlanDate { get; set; }
            /// <summary>Gets or sets フォロー内容</summary>
            /// <value>フォロー内容</value>
            public string FollowContent { get; set; }
            /// <summary>Gets or sets フォロー完了日</summary>
            /// <value>フォロー完了日</value>
            public DateTime? FollowCompletionDate { get; set; }
            /// <summary>Gets or sets 現象ID</summary>
            /// <value>現象ID</value>
            public int? PhenomenonStructureId { get; set; }
            /// <summary>Gets or sets 現象(表示用)</summary>
            /// <value>現象(表示用)</value>
            public string PhenomenonName { get; set; }
            /// <summary>Gets or sets 現象メモ</summary>
            /// <value>現象メモ</value>
            public string PhenomenonNote { get; set; }
            /// <summary>Gets or sets 原因階層ID</summary>
            /// <value>原因階層ID</value>
            public int? FailureCauseStructureId { get; set; }
            /// <summary>Gets or sets 原因階層(表示用)</summary>
            /// <value>原因階層(表示用)</value>
            public string FailureCauseName { get; set; }
            /// <summary>Gets or sets 原因メモ</summary>
            /// <value>原因メモ</value>
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
            /// <summary>Gets or sets 原因性格メモ</summary>
            /// <value>原因性格メモ</value>
            public string FailureCausePersonalityNote { get; set; }
            /// <summary>Gets or sets 処置・対策ID</summary>
            /// <value>処置・対策ID</value>
            public int? TreatmentMeasureStructureId { get; set; }
            /// <summary>Gets or sets 処置・対策(表示用)</summary>
            /// <value>処置・対策(表示用)</value>
            public string TreatmentMeasureName { get; set; }
            /// <summary>Gets or sets 処置・対策メモ</summary>
            /// <value>処置・対策メモ</value>
            public string TreatmentMeasureNote { get; set; }
            /// <summary>Gets or sets 故障状況</summary>
            /// <value>故障状況</value>
            public string FailureStatus { get; set; }
            /// <summary>Gets or sets 故障状況(個別工場)</summary>
            /// <value>故障状況(個別工場)</value>
            public string FailureStatusIndividual { get; set; }
            /// <summary>Gets or sets 故障原因補足</summary>
            /// <value>故障原因補足</value>
            public string FailureCauseAdditionNote { get; set; }
            /// <summary>Gets or sets 故障原因補足(個別工場)</summary>
            /// <value>故障原因補足(個別工場)</value>
            public string FailureCauseAdditionNoteIndividual { get; set; }
            /// <summary>Gets or sets 故障前の保全状況</summary>
            /// <value>故障前の保全状況</value>
            public string PreviousSituation { get; set; }
            /// <summary>Gets or sets 故障前の保全状況(個別工場)</summary>
            /// <value>故障前の保全状況(個別工場)</value>
            public string PreviousSituationIndividual { get; set; }
            /// <summary>Gets or sets 復旧処置</summary>
            /// <value>復旧処置</value>
            public string RecoveryAction { get; set; }
            /// <summary>Gets or sets 復旧処置(個別工場)</summary>
            /// <value>復旧処置(個別工場)</value>
            public string RecoveryActionIndividual { get; set; }
            /// <summary>Gets or sets 改善対策</summary>
            /// <value>改善対策</value>
            public string ImprovementMeasure { get; set; }
            /// <summary>Gets or sets 改善対策(個別工場)</summary>
            /// <value>改善対策(個別工場)</value>
            public string ImprovementMeasureIndividual { get; set; }
            /// <summary>Gets or sets 保全システムへのフィードバック</summary>
            /// <value>保全システムへのフィードバック</value>
            public string SystemFeedBack { get; set; }
            /// <summary>Gets or sets 教訓</summary>
            /// <value>教訓</value>
            public string Lesson { get; set; }
            /// <summary>Gets or sets 教訓(個別工場)</summary>
            /// <value>教訓(個別工場)</value>
            public string LessonIndividual { get; set; }
            /// <summary>Gets or sets 特記(メモ)</summary>
            /// <value>特記(メモ)</value>
            public string FailureNote { get; set; }
            /// <summary>Gets or sets 特記(メモ)(個別工場)</summary>
            /// <value>特記(メモ)(個別工場)</value>
            public string FailureNoteIndividual { get; set; }
            /// <summary>Gets or sets 故障分析ID</summary>
            /// <value>故障分析ID</value>
            public int? FailureAnalysisStructureId { get; set; }
            /// <summary>Gets or sets 故障分析(表示用)</summary>
            /// <value>故障分析(表示用)</value>
            public string FailureAnalysisName { get; set; }
            /// <summary>Gets or sets 故障性格要因ID</summary>
            /// <value>故障性格要因ID</value>
            public int? FailurePersonalityFactorStructureId { get; set; }
            /// <summary>Gets or sets 故障性格要因(表示用)</summary>
            /// <value>故障性格要因(表示用)</value>
            public string FailurePersonalityFactorName { get; set; }
            /// <summary>Gets or sets 故障性格分類ID</summary>
            /// <value>故障性格分類ID</value>
            public int? FailurePersonalityClassStructureId { get; set; }
            /// <summary>Gets or sets 故障性格分類(表示用)</summary>
            /// <value>故障性格分類(表示用)</value>
            public string FailurePersonalityClassName { get; set; }
            /// <summary>Gets or sets 処置状況ID</summary>
            /// <value>処置状況ID</value>
            public int? TreatmentStatusStructureId { get; set; }
            /// <summary>Gets or sets 処置状況(表示用)</summary>
            /// <value>処置状況(表示用)</value>
            public string TreatmentStatusName { get; set; }
            /// <summary>Gets or sets 対策要否ID</summary>
            /// <value>対策要否ID</value>
            public int? NecessityMeasureStructureId { get; set; }
            /// <summary>Gets or sets 対策要否(表示用)</summary>
            /// <value>対策要否(表示用)</value>
            public string NecessityMeasureName { get; set; }
            /// <summary>Gets or sets 対策実施予定日</summary>
            /// <value>対策実施予定日</value>
            public DateTime? MeasurePlanDate { get; set; }
            /// <summary>Gets or sets 対策分類1階層ID</summary>
            /// <value>対策分類1階層ID</value>
            public int? MeasureClass1StructureId { get; set; }
            /// <summary>Gets or sets 対策分類1階層(表示用)</summary>
            /// <value>対策分類1階層(表示用)</value>
            public string MeasureClass1Name { get; set; }
            /// <summary>Gets or sets 対策分類2階層ID</summary>
            /// <value>対策分類2階層ID</value>
            public int? MeasureClass2StructureId { get; set; }
            /// <summary>Gets or sets 対策分類2階層(表示用)</summary>
            /// <value>対策分類2階層(表示用)</value>
            public string MeasureClass2Name { get; set; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long SummaryId { get; set; }
            /// <summary>Gets or sets 保全履歴故障情報ID</summary>
            /// <value>保全履歴故障情報ID</value>
            public long HistoryFailureId { get; set; }
            /// <summary>Gets or sets 個別工場制御用フラグ</summary>
            /// <value>個別工場制御用フラグ</value>
            public string ControlFlag { get; set; }
        }

        /// <summary>
        /// ExcelPort 保全活動_点検情報（対象機器）のデータクラス
        /// </summary>
        public class excelPortInspectionMachine : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            #region 地区・職種機種(保全活動件名)
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
            #endregion
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
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long? EquipmentId { get; set; }
            /// <summary>Gets or sets 機器番号 機器名称</summary>
            /// <value>機器番号 機器名称</value>
            public string Machine { get; set; }
            #region 地区・職種機種(機器)
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? MachineLocationStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? MachineJobStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? MachineDistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string MachineDistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? MachineFactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string MachineFactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? MachinePlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string MachinePlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? MachineSeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string MachineSeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? MachineStrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string MachineStrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? MachineFacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string MachineFacilityName { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int MachineJobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string MachineJobName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? MachineLargeClassficationId { get; set; }
            /// <summary>Gets or sets 機種大分類名称</summary>
            /// <value>機種大分類名称</value>
            public string MachineLargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? MachineMiddleClassficationId { get; set; }
            /// <summary>Gets or sets 機種中分類名称</summary>
            /// <value>機種中分類名称</value>
            public string MachineMiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? MachineSmallClassficationId { get; set; }
            /// <summary>Gets or sets 機種小分類名称</summary>
            /// <value>機種小分類名称</value>
            public string MachineSmallClassficationName { get; set; }
            #endregion
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int? EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 機器レベル(表示用)</summary>
            /// <value>機器レベル(表示用)</value>
            public string EquipmentLevelName { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public int? ConservationStructureId { get; set; }
            /// <summary>Gets or sets 保全方式(表示用)</summary>
            /// <value>保全方式(表示用)</value>
            public string ConservationName { get; set; }
            /// <summary>Gets or sets 重要度ID</summary>
            /// <value>重要度ID</value>
            public int? ImportanceStructureId { get; set; }
            /// <summary>Gets or sets 重要度(表示用)</summary>
            /// <value>重要度(表示用)</value>
            public string ImportanceName { get; set; }
            /// <summary>Gets or sets 機器使用日数</summary>
            /// <value>機器使用日数</value>
            public int? UsedDaysMachine { get; set; }
            /// <summary>Gets or sets 保全部位ID</summary>
            /// <value>保全部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 保全部位(表示用)</summary>
            /// <value>保全部位(表示用)</value>
            public string InspectionSiteName { get; set; }
            /// <summary>Gets or sets 保全内容ID</summary>
            /// <value>保全内容ID</value>
            public int? InspectionContentStructureId { get; set; }
            /// <summary>Gets or sets 保全内容(表示用)</summary>
            /// <value>保全内容(表示用)</value>
            public string InspectionContentName { get; set; }
            /// <summary>Gets or sets フォロー有無</summary>
            /// <value>フォロー有無</value>
            public int? FollowFlg { get; set; }
            /// <summary>Gets or sets フォロー有無(表示用)</summary>
            /// <value>フォロー有無(表示用)</value>
            public string FollowFlgName { get; set; }
            /// <summary>Gets or sets フォロー予定日</summary>
            /// <value>フォロー予定日</value>
            public DateTime? FollowPlanDate { get; set; }
            /// <summary>Gets or sets フォロー内容</summary>
            /// <value>フォロー内容</value>
            public string FollowContent { get; set; }
            /// <summary>Gets or sets フォロー完了日</summary>
            /// <value>フォロー完了日</value>
            public DateTime? FollowCompletionDate { get; set; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long SummaryId { get; set; }
            /// <summary>Gets or sets 保全活動件名ID(変更前)</summary>
            /// <value>保全活動件名ID(変更前)</value>
            public long OldSummaryId { get; set; }
            /// <summary>Gets or sets 保全履歴機器部位ID</summary>
            /// <value>保全履歴機器部位ID</value>
            public long? HistoryInspectionSiteId { get; set; }
            /// <summary>Gets or sets 保全履歴点検内容ID</summary>
            /// <value>保全履歴点検内容ID</value>
            public long? HistoryInspectionContentId { get; set; }
            /// <summary>Gets or sets 履歴ID</summary>
            /// <value>履歴ID</value>
            public long? HistoryId { get; set; }
            /// <summary>Gets or sets 保全履歴機器ID</summary>
            /// <value>保全履歴機器ID</value>
            public long? HistoryMachineId { get; set; }
        }
    }
}
