using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;

namespace BusinessLogic_MC0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_MC0001
    {
        /// <summary>
        /// 機器台帳一覧のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            // SQLの検索結果の列を定義してください。
            // 品目マスタから多くの列を取得する場合は、品目マスタのデータクラスを継承することで、それらの定義を省くことができます。

            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long? EquipmentId { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int? EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 機器レベル名称</summary>
            /// <value>機器レベル名称</value>
            public string EquipmentLevel { get; set; }
            /// <summary>Gets or sets 重要度</summary>
            /// <value>重要度</value>
            public int? ImportanceStructureId { get; set; }
            /// <summary>Gets or sets 重要度名称</summary>
            /// <value>重要度名称</value>
            public string ImportanceName { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public int? InspectionSiteConservationStructureId { get; set; }
            /// <summary>Gets or sets 保全方式名称</summary>
            /// <value>保全方式名称</value>
            public string InspectionSiteConservationName { get; set; }
            /// <summary>Gets or sets 設置場所</summary>
            /// <value>設置場所</value>
            public string InstallationLocation { get; set; }
            /// <summary>Gets or sets 設置台数</summary>
            /// <value>設置台数</value>
            public decimal? NumberOfInstallation { get; set; }
            /// <summary>Gets or sets 設置年月</summary>
            /// <value>設置年月</value>
            public DateTime? DateOfInstallation { get; set; }
            /// <summary>Gets or sets 適用法規</summary>
            /// <value>適用法規</value>
            public string ApplicableLawsStructureId { get; set; }
            /// <summary>Gets or sets 適用法規名称</summary>
            /// <value>適用法規名称</value>
            public string ApplicableLawsName { get; set; }
            /// <summary>Gets or sets 機番メモ</summary>
            /// <value>機番メモ</value>
            public string MachineNote { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string ManufacturerName { get; set; }
            /// <summary>Gets or sets メーカー型式</summary>
            /// <value>メーカー型式</value>
            public string ManufacturerType { get; set; }
            /// <summary>Gets or sets 型式コード</summary>
            /// <value>型式コード</value>
            public string ModelNo { get; set; }
            /// <summary>Gets or sets 製造番号</summary>
            /// <value>製造番号</value>
            public string SerialNo { get; set; }
            /// <summary>Gets or sets 製造年月</summary>
            /// <value>製造年月</value>
            public DateTime? DateOfManufacture { get; set; }
            /// <summary>Gets or sets 納期</summary>
            /// <value>納期</value>
            public int? DeliveryDate { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public int? UseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 使用区分名称</summary>
            /// <value>使用区分名称</value>
            public string UseSegmentName { get; set; }
            /// <summary>Gets or sets 機番添付有無</summary>
            /// <value>機番添付有無</value>
            public string FileLinkMachine { get; set; }
            /// <summary>Gets or sets 機器添付有無</summary>
            /// <value>機器添付有無</value>
            public string FileLinkEquip { get; set; }
            /// <summary>Gets or sets 固定資産番号</summary>
            /// <value>固定資産番号</value>
            public string FixedAssetNo { get; set; }
            /// <summary>Gets or sets 循環対象</summary>
            /// <value>循環対象</value>
            public bool CirculationTargetFlg { get; set; }
            /// <summary>Gets or sets 点検種別毎管理</summary>
            /// <value>点検種別毎管理</value>
            public bool MaintainanceKindManage { get; set; }
            /// <summary>Gets or sets 機器メモ</summary>
            /// <value>機器メモ</value>
            public string EquipmentNote { get; set; }
            /// <summary>Gets or sets 更新シリアルID(機番情報)</summary>
            /// <value>更新シリアルID</value>
            public string McUpdateSerialId { get; set; }
            /// <summary>Gets or sets 更新シリアルID(機器情報)</summary>
            /// <value>更新シリアルID</value>
            public string EqUpdateSerialId { get; set; }
            /// <summary>Gets or sets 場所階層</summary>
            /// <value>場所階層</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層</summary>
            /// <value>職種機種階層</value>
            public int JobStructureId { get; set; }
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
            public int JobId { get; set; }
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
            public int TabNo { get; set; }
        }

        /// <summary>
        /// 保全項目一覧のデータクラス
        /// </summary>
        public class managementStandardResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機器別管理基準部位ID</summary>
            /// <value>機器別管理基準部位ID</value>
            public long ManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int? EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 点検種別毎管理</summary>
            /// <value>点検種別毎管理</value>
            public bool MaintainanceKindManage { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 部位重要度</summary>
            /// <value>部位重要度</value>
            public int? InspectionSiteImportanceStructureId { get; set; }
            /// <summary>Gets or sets 部位保全方式</summary>
            /// <value>部位保全方式</value>
            public int? InspectionSiteConservationStructureId { get; set; }
            /// <summary>Gets or sets 機器別管理基準フラグ</summary>
            /// <value>機器別管理基準フラグ</value>
            public bool? IsManagementStandardConponent { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 点検内容ID</summary>
            /// <value>点検内容ID</value>
            public int? InspectionContentStructureId { get; set; }
            /// <summary>Gets or sets 保全区分</summary>
            /// <value>保全区分</value>
            public int? MaintainanceDivision { get; set; }
            /// <summary>Gets or sets 点検種別</summary>
            /// <value>点検種別</value>
            public int? MaintainanceKindStructureId { get; set; }
            /// <summary>Gets or sets 予算金額</summary>
            /// <value>予算金額</value>
            public decimal? BudgetAmount { get; set; }
            /// <summary>Gets or sets 準備期間(日)</summary>
            /// <value>準備期間(日)</value>
            public int? PreparationPeriod { get; set; }
            /// <summary>Gets or sets 長計件名ID</summary>
            /// <value>長計件名ID</value>
            public int? LongPlanId { get; set; }
            /// <summary>Gets or sets 並び順</summary>
            /// <value>並び順</value>
            public int? OrderNo { get; set; }
            /// <summary>Gets or sets スケジュール管理基準ID</summary>
            /// <value>スケジュール管理基準ID</value>
            public int? ScheduleTypeStructureId { get; set; }
            /// <summary>Gets or sets 保全スケジュールID</summary>
            /// <value>保全スケジュールID</value>
            public long MaintainanceScheduleId { get; set; }
            /// <summary>Gets or sets 周期ありフラグ</summary>
            /// <value>周期ありフラグ</value>
            public bool? IsCyclic { get; set; }
            /// <summary>Gets or sets 周期(年)</summary>
            /// <value>周期(年)</value>
            public int? CycleYear { get; set; }
            /// <summary>Gets or sets 周期(月)</summary>
            /// <value>周期(月)</value>
            public int? CycleMonth { get; set; }
            /// <summary>Gets or sets 周期(日)</summary>
            /// <value>周期(日)</value>
            public int? CycleDay { get; set; }
            /// <summary>Gets or sets 表示周期</summary>
            /// <value>表示周期</value>
            public string DispCycle { get; set; }
            /// <summary>Gets or sets 開始日</summary>
            /// <value>開始日</value>
            public DateTime? StartDate { get; set; }
            /// <summary>Gets or sets 添付ファイル</summary>
            /// <value>添付ファイル</value>
            public string AttachmentFile { get; set; }
            /// <summary>Gets or sets 最大更新日時</summary>
            /// <value>最大更新日時</value>
            public DateTime? MaxUpdateDatetime { get; set; }
            /// <summary>Gets or sets 添付ファイル削除用キーID</summary>
            /// <value>添付ファイル削除用キーID</value>
            public int? KeyId { get; set; }
            /// <summary>Gets or sets 機能タイプID</summary>
            /// <value>機能タイプID</value>
            public int? FunctionTypeId { get; set; }
        }

        /// <summary>
        /// 様式１一覧のデータクラス
        /// </summary>
        public class format1ListResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機器別管理基準部位ID</summary>
            /// <value>機器別管理基準部位ID</value>
            public long ManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int? EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 重要度</summary>
            /// <value>重要度</value>
            public int? ImportanceStructureId { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 部位重要度</summary>
            /// <value>部位重要度</value>
            public int? InspectionSiteImportanceStructureId { get; set; }
            /// <summary>Gets or sets 部位保全方式</summary>
            /// <value>部位保全方式</value>
            public int? InspectionSiteConservationStructureId { get; set; }
            /// <summary>Gets or sets 機器別管理基準フラグ</summary>
            /// <value>機器別管理基準フラグ</value>
            public bool? IsManagementStandardConponent { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 点検内容ID</summary>
            /// <value>点検内容ID</value>
            public int? InspectionContentStructureId { get; set; }
            /// <summary>Gets or sets 保全区分</summary>
            /// <value>保全区分</value>
            public int? MaintainanceDivision { get; set; }
            /// <summary>Gets or sets 点検種別</summary>
            /// <value>点検種別</value>
            public int? MaintainanceKindStructureId { get; set; }
            /// <summary>Gets or sets 予算金額</summary>
            /// <value>予算金額</value>
            public decimal? BudgetAmount { get; set; }
            /// <summary>Gets or sets 準備期間(日)</summary>
            /// <value>準備期間(日)</value>
            public int? PreparationPeriod { get; set; }
            /// <summary>Gets or sets 長計件名ID</summary>
            /// <value>長計件名ID</value>
            public int? LongPlanId { get; set; }
            /// <summary>Gets or sets 並び順</summary>
            /// <value>並び順</value>
            public int? OrderNo { get; set; }
            /// <summary>Gets or sets スケジュール管理基準ID</summary>
            /// <value>スケジュール管理基準ID</value>
            public int? ScheduleTypeStructureId { get; set; }
            /// <summary>Gets or sets 保全スケジュールID</summary>
            /// <value>保全スケジュールID</value>
            public long MaintainanceScheduleId { get; set; }
            /// <summary>Gets or sets 周期ありフラグ</summary>
            /// <value>周期ありフラグ</value>
            public bool? IsCyclic { get; set; }
            /// <summary>Gets or sets 周期(年)</summary>
            /// <value>周期(年)</value>
            public int? CycleYear { get; set; }
            /// <summary>Gets or sets 周期(月)</summary>
            /// <value>周期(月)</value>
            public int? CycleMonth { get; set; }
            /// <summary>Gets or sets 周期(日)</summary>
            /// <value>周期(日)</value>
            public int? CycleDay { get; set; }
            /// <summary>Gets or sets 表示周期</summary>
            /// <value>表示周期</value>
            public string DispCycle { get; set; }
            /// <summary>Gets or sets 開始日</summary>
            /// <value>開始日</value>
            public DateTime? StartDate { get; set; }
            /// <summary>Gets or sets 定期検査：内容</summary>
            /// <value>定期検査：内容ID(保全項目)</value>
            public int? PeriodicInspection { get; set; }
            /// <summary>Gets or sets 定期検査：周期</summary>
            /// <value>定期検査：周期(表示周期)</value>
            public string PeriodicCycle { get; set; }
            /// <summary>Gets or sets 定期修理：内容</summary>
            /// <value>定期修理：内容ID(保全項目)</value>
            public int? RepairInspection { get; set; }
            /// <summary>Gets or sets 定期修理：周期</summary>
            /// <value>定期修理：周期(表示周期)</value>
            public string RepairCycle { get; set; }
            /// <summary>Gets or sets 日常点検：内容</summary>
            /// <value>日常点検：内容ID(保全項目)</value>
            public int? DailyInspection { get; set; }
            /// <summary>Gets or sets 日常点検：周期</summary>
            /// <value>日常点検：周期(表示周期)</value>
            public string DailyCycle { get; set; }

        }

        /// <summary>
        /// スケジューリングのデータクラス
        /// </summary>
        public class schedeluListResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機器別管理基準部位ID</summary>
            /// <value>機器別管理基準部位ID</value>
            public long ManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int? EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 重要度</summary>
            /// <value>重要度</value>
            public int? ImportanceStructureId { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 部位重要度</summary>
            /// <value>部位重要度</value>
            public int? InspectionSiteImportanceStructureId { get; set; }
            /// <summary>Gets or sets 部位保全方式</summary>
            /// <value>部位保全方式</value>
            public int? InspectionSiteConservationStructureId { get; set; }
            /// <summary>Gets or sets 機器別管理基準フラグ</summary>
            /// <value>機器別管理基準フラグ</value>
            public bool? IsManagementStandardConponent { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 点検内容ID</summary>
            /// <value>点検内容ID</value>
            public int? InspectionContentStructureId { get; set; }
            /// <summary>Gets or sets 保全区分</summary>
            /// <value>保全区分</value>
            public int? MaintainanceDivision { get; set; }
            /// <summary>Gets or sets 点検種別</summary>
            /// <value>点検種別</value>
            public int? MaintainanceKindStructureId { get; set; }
            /// <summary>Gets or sets 予算金額</summary>
            /// <value>予算金額</value>
            public decimal? BudgetAmount { get; set; }
            /// <summary>Gets or sets 準備期間(日)</summary>
            /// <value>準備期間(日)</value>
            public int? PreparationPeriod { get; set; }
            /// <summary>Gets or sets 長計件名ID</summary>
            /// <value>長計件名ID</value>
            public int? LongPlanId { get; set; }
            /// <summary>Gets or sets 並び順</summary>
            /// <value>並び順</value>
            public int? OrderNo { get; set; }
            /// <summary>Gets or sets スケジュール管理基準ID</summary>
            /// <value>スケジュール管理基準ID</value>
            public int? ScheduleTypeStructureId { get; set; }
            /// <summary>Gets or sets 保全スケジュールID</summary>
            /// <value>保全スケジュールID</value>
            public long MaintainanceScheduleId { get; set; }
            /// <summary>Gets or sets 周期ありフラグ</summary>
            /// <value>周期ありフラグ</value>
            public bool? IsCyclic { get; set; }
            /// <summary>Gets or sets 周期(年)</summary>
            /// <value>周期(年)</value>
            public int? CycleYear { get; set; }
            /// <summary>Gets or sets 周期(月)</summary>
            /// <value>周期(月)</value>
            public int? CycleMonth { get; set; }
            /// <summary>Gets or sets 周期(日)</summary>
            /// <value>周期(日)</value>
            public int? CycleDay { get; set; }
            /// <summary>Gets or sets 表示周期</summary>
            /// <value>表示周期</value>
            public string DispCycle { get; set; }
            /// <summary>Gets or sets 開始日</summary>
            /// <value>開始日</value>
            public DateTime? StartDate { get; set; }
            /// <summary>Gets or sets スケジュール紐付け用キーID</summary>
            /// <value>スケジュール紐付け用キーID</value>
            public string KeyId { get; set; }
            /// <summary>Gets or sets グループキーID</summary>
            /// <value>グループキーID</value>
            public string GroupKey { get; set; }
            /// <summary>Gets or sets スケジュール部分更新日時</summary>
            /// <value>スケジュール部分更新日時</value>
            public DateTime UpdateDatetimeSch { get; set; }
            /// <summary>Gets or sets 同一マークキー</summary>
            /// <value>同一マークキー</value>
            /// <remarks>スケジュールと同じ、排他キーに使用</remarks>
            public string SameMarkKey { get; set; }
        }

        /// <summary>
        /// 長期計画一覧のデータクラス
        /// </summary>
        public class longPlanListResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機器別管理基準部位ID</summary>
            /// <value>機器別管理基準部位ID</value>
            public long ManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 点検内容ID</summary>
            /// <value>点検内容ID</value>
            public int? InspectionContentStructureId { get; set; }
            /// <summary>Gets or sets 長計件名ID</summary>
            /// <value>長計件名ID</value>
            public int? LongPlanId { get; set; }
            /// <summary>Gets or sets スケジュール管理基準ID</summary>
            /// <value>スケジュール管理基準ID</value>
            public int? ScheduleTypeStructureId { get; set; }
            /// <summary>Gets or sets 保全スケジュールID</summary>
            /// <value>保全スケジュールID</value>
            public long MaintainanceScheduleId { get; set; }
            /// <summary>Gets or sets 周期(年)</summary>
            /// <value>周期(年)</value>
            public int? CycleYear { get; set; }
            /// <summary>Gets or sets 周期(月)</summary>
            /// <value>周期(月)</value>
            public int? CycleMonth { get; set; }
            /// <summary>Gets or sets 周期(日)</summary>
            /// <value>周期(日)</value>
            public int? CycleDay { get; set; }
            /// <summary>Gets or sets 表示周期</summary>
            /// <value>表示周期</value>
            public string DispCycle { get; set; }
            /// <summary>Gets or sets 開始日</summary>
            /// <value>開始日</value>
            public DateTime? StartDate { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets 行番号(Noリンク)</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets スケジュール紐付け用キーID</summary>
            /// <value>スケジュール紐付け用キーID</value>
            public string KeyId { get; set; }

        }

        /// <summary>
        /// スケジュール表示条件：月度のデータクラス
        /// </summary>
        public class ScheduleCondInfo
        {
            /// <summary>Gets or sets スケジュール表示単位</summary>
            /// <value>スケジュール表示単位</value>
            public int? ScheduleCondType { get; set; }
            /// <summary>Gets or sets 月度ID</summary>
            /// <value>月度ID</value>
            public int? ScheduleCondMonthStructureId { get; set; }
            /// <summary>Gets or sets スケジュール表示年度</summary>
            /// <value>スケジュール表示年度</value>
            public int? ScheduleCondYear { get; set; }
            /// <summary>Gets or sets スケジュール表示期間</summary>
            /// <value>スケジュール表示期間</value>
            public string? scheduleCondSpan { get; set; }
        }

        /// <summary>
        /// 保全活動一覧のデータクラス
        /// </summary>
        public class maintainanceActivityListResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 行番号(Noリンク)</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long SummaryId { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets 完了日</summary>
            /// <value>完了日</value>
            public DateTime? CompletionDate { get; set; }
            /// <summary>Gets or sets 実績金額</summary>
            /// <value>実績金額</value>
            public decimal? Expenditure { get; set; }
            /// <summary>Gets or sets 保全部位</summary>
            /// <value>保全部位</value>
            public string MaintenanceSite { get; set; }
            /// <summary>Gets or sets 保全内容</summary>
            /// <value>保全内容</value>
            public string MaintenanceContent { get; set; }
            /// <summary>Gets or sets 作業時間</summary>
            /// <value>作業時間</value>
            public decimal? TotalWorkingTime { get; set; }
            /// <summary>Gets or sets 原因階層ID</summary>
            /// <value>原因階層ID</value>
            public int? FailureCauseStructureId { get; set; }
            /// <summary>Gets or sets 原因性格ID</summary>
            /// <value>原因性格ID</value>
            public int? FailureCausePersonalityStructureId { get; set; }
            /// <summary>Gets or sets 処置・対策ID</summary>
            /// <value>処置・対策ID</value>
            public int? TreatmentMeasureStructureId { get; set; }
            /// <summary>Gets or sets 作業計画・実施内容</summary>
            /// <value>作業計画・実施内容</value>
            public string PlanImplementationContent { get; set; }

        }

        /// <summary>
        /// 使用部品一覧のデータクラス
        /// </summary>
        public class usePartsListResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機番使用部品情報ID</summary>
            /// <value>機番使用部品情報ID</value>
            public long MachineUsePartsId { get; set; }
            /// <summary>Gets or sets 部品名</summary>
            /// <value>部品名</value>
            public long? PartsId { get; set; }
            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string Dimensions { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string Maker { get; set; }
            /// <summary>Gets or sets 使用個数</summary>
            /// <value>使用個数</value>
            public decimal? UseQuantity { get; set; }
            /// <summary>Gets or sets 品目在庫数</summary>
            /// <value>品目在庫数</value>
            public decimal? Stock { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
        }

        /// <summary>
        /// 親子構成一覧のデータクラス
        /// </summary>
        public class constitutionListResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public string EquipmentLevel { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int JobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 親子構成ID</summary>
            /// <value>親子構成ID</value>
            public int ParentId { get; set; }
            /// <summary>Gets or sets ループID</summary>
            /// <value>ループID</value>
            public int LoopId { get; set; }
            /// <summary>Gets or sets 子フラグ</summary>
            /// <value>子フラグ</value>
            public string FlgChild { get; set; }
        }

        /// <summary>
        /// 構成機器タブ、機器選択画面 非表示一覧のデータクラス
        /// </summary>
        public class constitutionHideList : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int EquipmentLevel { get; set; }
            /// <summary>Gets or sets 一覧フラグ</summary>
            /// <value>一覧フラグ</value>
            public int? ListFlg { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
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
        /// 機器選択画面 検索条件
        /// </summary>
        public class selectMachineSearchCondition : ComDao.SearchCommonClass
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
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public int? UseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 循環対象</summary>
            /// <value>循環対象</value>
            public int? CirculationTargetFlg { get; set; }
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
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
        }

        /// <summary>
        /// 機器選択画面　検索結果一覧のデータクラス
        /// </summary>
        public class selectMachineList : ComDao.CommonTableItem
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
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
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
            /// <summary>Gets or sets 機器重要度</summary>
            /// <value>機器重要度</value>
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
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public string InspectionSiteConservationName { get; set; }
            /// <summary>Gets or sets 製造番号</summary>
            /// <value>製造番号</value>
            public string SerialNo { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
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
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public string UseSegmentName { get; set; }
            /// <summary>Gets or sets 設置年月</summary>
            /// <value>設置年月</value>
            public DateTime? DateOfInstallation { get; set; }
            /// <summary>Gets or sets 親子構成ID</summary>
            /// <value>親子構成ID</value>
            public int ParentId { get; set; }
            /// <summary>Gets or sets 更新シリアルID(親子構成)</summary>
            /// <value>更新シリアルID</value>
            public string ParentUpdateSerialId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets ループの機番ID</summary>
            /// <value>ループの機番ID</value>
            public long? LoopMachineId { get; set; }
        }

        /// <summary>
        /// MP情報 非表示一覧 データクラス
        /// </summary>
        public class mpInfoHideList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
        }

        /// <summary>
        /// MP情報一覧 データクラス
        /// </summary>
        public class mpInfoList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets MP情報</summary>
            /// <value>MP情報</value>
            public string MpInformation { get; set; }
            /// <summary>Gets or sets 関連ファイル</summary>
            /// <value>関連ファイル</value>
            public string FileLinkInformation { get; set; }
            /// <summary>Gets or sets MP情報ID</summary>
            /// <value>MP情報ID</value>
            public int? MpInformationId { get; set; }
            /// <summary>Gets or sets 更新シリアルID(MP情報)</summary>
            /// <value>更新シリアルID(MP情報)</value>
            public int InfoUpdateSerialid { get; set; }
            /// <summary>Gets or sets 最大更新日時</summary>
            /// <value>最大更新日時</value>
            public DateTime? MaxUpdateDatetime { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 添付ファイル削除用キーID</summary>
            /// <value>添付ファイル削除用キーID</value>
            public int? KeyId { get; set; }
            /// <summary>Gets or sets 機能タイプID</summary>
            /// <value>機能タイプID</value>
            public int? FunctionTypeId { get; set; }
        }

        /// <summary>
        /// 機種別仕様クラス
        /// </summary>
        public class EquipmentSpecLayout : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機種別仕様関連付ID</summary>
            /// <value>機種別仕様関連付ID</value>
            public int MachineSpecRelationId { get; set; }
            /// <summary>Gets or sets 機能場所階層ID(工場ID)</summary>
            /// <value>機能場所階層ID(工場ID)</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
            /// <summary>Gets or sets 仕様項目ID</summary>
            /// <value>仕様項目ID</value>
            public int? SpecId { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DisplayOrder { get; set; }
            /// <summary>Gets or sets 仕様項目入力形式ID</summary>
            /// <value>仕様項目入力形式ID</value>
            public int? SpecTypeId { get; set; }
            /// <summary>Gets or sets 仕様単位種別ID</summary>
            /// <value>仕様単位種別ID</value>
            public int? SpecUnitTypeId { get; set; }
            /// <summary>Gets or sets 仕様単位ID</summary>
            /// <value>仕様単位ID</value>
            public int? SpecUnitId { get; set; }
            /// <summary>Gets or sets 設定値(数値)小数点以下桁数</summary>
            /// <value>設定値(数値)小数点以下桁数</value>
            public int? SpecNumDecimalPlaces { get; set; }
            /// <summary>Gets or sets 翻訳ID</summary>
            /// <value>翻訳ID</value>
            public int? TranslationId { get; set; }
            /// <summary>Gets or sets 翻訳</summary>
            /// <value>翻訳</value>
            public string TranslationText { get; set; }
            /// <summary>Gets or sets 拡張データ(仕様項目区分)</summary>
            /// <value>拡張データ(仕様項目区分)</value>
            public string ExtensionData { get; set; }
            /// <summary>Gets or sets 単位翻訳</summary>
            /// <value>単位翻訳</value>
            public string UnitTranslationText { get; set; }
            /// <summary>Gets or sets 設定値(テキスト)</summary>
            /// <value>設定値(テキスト)</value>
            public string SpecValue { get; set; }
            /// <summary>Gets or sets 設定値(選択)</summary>
            /// <value>設定値(選択)</value>
            public int? SpecStructureId { get; set; }
            /// <summary>Gets or sets 設定値(数値)</summary>
            /// <value>設定値(数値)</value>
            public decimal? SpecNum { get; set; }
            /// <summary>Gets or sets 設定値(数値(範囲))最小値</summary>
            /// <value>設定値(数値(範囲))最小値</value>
            public decimal? SpecNumMin { get; set; }
            /// <summary>Gets or sets 設定値(数値(範囲))最大値</summary>
            /// <value>設定値(数値(範囲))最大値</value>
            public decimal? SpecNumMax { get; set; }
            /// <summary>Gets or sets 機種別仕様ID</summary>
            /// <value>機種別仕様ID</value>
            public long? EquipmentSpecId { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long? EquipmentId { get; set; }
        }
        /// <summary>
        /// スケジュール関連
        /// </summary>
        public class Schedule
        {
            /// <summary>
            /// 更新SQLの条件
            /// </summary>
            public class UpdateCondition : ComDao.CommonTableItem
            {
                /// <summary>Gets or sets 加算月数</summary>
                /// <value>加算月数</value>
                public int AddMonth { get; set; }

                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                public long ManagementStandardsContentId { get; set; }

                /// <summary>Gets or sets 更新対象月開始日</summary>
                /// <value>更新対象月開始日</value>
                public DateTime MonthStartDate { get; set; }

                /// <summary>Gets or sets 更新対象月終了日</summary>
                /// <value>更新対象月終了日</value>
                public DateTime MonthEndDate { get; set; }

                /// <summary>Gets or sets 構成マスタ検索対象の工場ID</summary>
                /// <value>構成マスタ検索対象の工場ID</value>
                public List<int> FactoryIdList { get; set; }
            }

            /// <summary>
            /// スケジュールの検索条件(工場IDのみ)
            /// </summary>
            public class SearchCondition : TMQDao.ScheduleList.GetCondition
            {
                /// <summary>Gets or sets 構成マスタ検索対象の工場ID</summary>
                /// <value>構成マスタ検索対象の工場ID</value>
                public List<int> FactoryIdList { get; set; }
                /// <summary>Gets or sets 言語ID</summary>
                /// <value>言語ID</value>
                public string LanguageId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="condition">画面の検索条件</param>
                /// <param name="monthStartNendo">年度開始月</param>
                /// <param name="languageId">言語ID</param>
                public SearchCondition(TMQDao.ScheduleList.Condition condition, int monthStartNendo, string languageId) : base(condition, monthStartNendo)
                {
                    this.LanguageId = languageId;
                }
            }
            /// <summary>
            /// スケジュールの検索条件(工場ID+機番ID)
            /// </summary>
            public class SearchConditionMachineId : SearchCondition
            {
                /// <summary>Gets or sets 機番ID</summary>
                /// <value>機番ID</value>
                public long MachineId { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="condition">画面の検索条件</param>
                /// <param name="machineId">機番ID</param>
                /// <param name="monthStartNendo">年度開始月</param>
                /// <param name="languageId">言語ID</param>
                public SearchConditionMachineId(TMQDao.ScheduleList.Condition condition, long machineId, int monthStartNendo, string languageId) : base(condition, monthStartNendo, languageId)
                {
                    this.MachineId = machineId;
                }
            }
        }

        /// <summary>
        /// 詳細画面検索条件
        /// </summary>
        public class DetailCondition
        {
            /// <summary>Gets or sets 長期計画件名ID</summary>
            /// <value>長期計画件名ID</value>
            public long LongPlanId { get; set; }

            /// <summary>Gets or sets 構成マスタ検索対象の工場ID</summary>
            /// <value>構成マスタ検索対象の工場ID</value>
            public List<int> FactoryIdList { get; set; }
        }
    }
}