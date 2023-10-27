using CommonWebTemplate.CommonDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonSTDUtil.CommonSTDUtil
{
    public class ReportDataClass_MC0001
    {
        /// <summary>
        /// シート１：機器台帳一覧
        /// </summary>
        public class Sheet1
        {
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 職種</summary>
            /// <value>職種</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 工場</summary>
            /// <value>工場</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラント</summary>
            /// <value>プラント</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列</summary>
            /// <value>系列</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程</summary>
            /// <value>工程</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備</summary>
            /// <value>設備</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 重要度</summary>
            /// <value>重要度</value>
            public string ImportanceStructureId { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public string InspectionSiteConservationStructureId { get; set; }
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public string EquipmentLevel { get; set; }
            /// <summary>Gets or sets 設置場所</summary>
            /// <value>設置場所</value>
            public string InstallationLocation { get; set; }
            /// <summary>Gets or sets 設置台数</summary>
            /// <value>設置台数</value>
            public int? NumberOfInstallation { get; set; }
            /// <summary>Gets or sets 設置年月</summary>
            /// <value>設置年月</value>
            public string DateOfInstallation { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public string UseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 循環対象</summary>
            /// <value>循環対象</value>
            public string CirculationTargetFlg { get; set; }
            /// <summary>Gets or sets 固定資産番号</summary>
            /// <value>固定資産番号</value>
            public string FixedAssetNo { get; set; }
            /// <summary>Gets or sets 適用法規１</summary>
            /// <value>適用法規１</value>
            public string ApplicableLawsName1 { get; set; }
            /// <summary>Gets or sets 適用法規２</summary>
            /// <value>適用法規２</value>
            public string ApplicableLawsName2 { get; set; }
            /// <summary>Gets or sets 適用法規３</summary>
            /// <value>適用法規３</value>
            public string ApplicableLawsName3 { get; set; }
            /// <summary>Gets or sets 適用法規４</summary>
            /// <value>適用法規４</value>
            public string ApplicableLawsName4 { get; set; }
            /// <summary>Gets or sets 適用法規５</summary>
            /// <value>適用法規５</value>
            public string ApplicableLawsName5 { get; set; }
            /// <summary>Gets or sets 機番メモ</summary>
            /// <value>機番メモ</value>
            public string MachineNote { get; set; }
            /// <summary>Gets or sets 機種大分類</summary>
            /// <value>機種大分類</value>
            public string LargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類</summary>
            /// <value>機種中分類</value>
            public string MiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類</summary>
            /// <value>機種小分類</value>
            public string SmallClassficationName { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string Maker { get; set; }
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
            public string DateOfManufacture { get; set; }
            /// <summary>Gets or sets 機器メモ</summary>
            /// <value>機器メモ</value>
            public string EquipmentNote { get; set; }
            /// <summary>Gets or sets ISO区分</summary>
            /// <value>ISO区分</value>
            public string IsoDivision { get; set; }
            /// <summary>Gets or sets 性能</summary>
            /// <value>性能</value>
            public string Performance { get; set; }
        }
        /// <summary>
        /// シート２：検索条件
        /// </summary>
        public class Sheet2
        {
            /// <summary>Gets or sets 職種</summary>
            /// <value>職種</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 工場</summary>
            /// <value>工場</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラント</summary>
            /// <value>プラント</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列</summary>
            /// <value>系列</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程</summary>
            /// <value>工程</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備</summary>
            /// <value>設備</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 重要度</summary>
            /// <value>重要度</value>
            public string ImportanceName { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public string ConservationName { get; set; }
            /// <summary>Gets or sets 適用法規１</summary>
            /// <value>適用法規１</value>
            public string ApplicableLawsName1 { get; set; }
            /// <summary>Gets or sets 適用法規２</summary>
            /// <value>適用法規２</value>
            public string ApplicableLawsName2 { get; set; }
            /// <summary>Gets or sets 適用法規３</summary>
            /// <value>適用法規３</value>
            public string ApplicableLawsName3 { get; set; }
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public string EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 機種大分類</summary>
            /// <value>機種大分類</value>
            public string LargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類</summary>
            /// <value>機種中分類</value>
            public string MiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類</summary>
            /// <value>機種小分類</value>
            public string SmallClassficationName { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public string UseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 循環対象</summary>
            /// <value>循環対象</value>
            public string CirculationTargetFlg { get; set; }
            /// <summary>Gets or sets 固定資産番号</summary>
            /// <value>固定資産番号</value>
            public string FixedAssetNo { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string Maker { get; set; }
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
            public string DateOfManufacture { get; set; }
            /// <summary>Gets or sets 部品</summary>
            /// <value>部品</value>
            public string PartsId { get; set; }
            /// <summary>Gets or sets 機能場所分類６</summary>
            /// <value>機能場所分類６</value>
            public string Location6 { get; set; }
            /// <summary>Gets or sets 機能場所分類７</summary>
            /// <value>機能場所分類７</value>
            public string Location7 { get; set; }
            /// <summary>Gets or sets 機能場所分類８</summary>
            /// <value>機能場所分類８</value>
            public string Location8 { get; set; }
            /// <summary>Gets or sets 機能場所分類９</summary>
            /// <value>機能場所分類９</value>
            public string Location9 { get; set; }
            /// <summary>Gets or sets 機能場所分類１０</summary>
            /// <value>機能場所分類１０</value>
            public string Location10 { get; set; }
            /// <summary>Gets or sets 長計チェック</summary>
            /// <value>長計チェック</value>
            public string LongPlanCheck { get; set; }
            /// <summary>Gets or sets ITEM.No</summary>
            /// <value>ITEM.No</value>
            public string ItemNo { get; set; }
            /// <summary>Gets or sets 社番</summary>
            /// <value>社番</value>
            public string CompanyNumber { get; set; }
            /// <summary>Gets or sets 購入先</summary>
            /// <value>購入先</value>
            public string Retailer { get; set; }
            /// <summary>Gets or sets 機器添付</summary>
            /// <value>機器添付</value>
            public string MachineAttachment { get; set; }
            /// <summary>Gets or sets 機番添付</summary>
            /// <value>機番添付</value>
            public string MachineNoAttachment { get; set; }
            /// <summary>Gets or sets 工事機械調書出力対象</summary>
            /// <value>工事機械調書出力対象</value>
            public string ConstructionMachineRecordOutputTarget { get; set; }
            /// <summary>Gets or sets PIDAS該当</summary>
            /// <value>PIDAS該当</value>
            public string PidasApplicable { get; set; }
            /// <summary>Gets or sets 高圧ガス施設名</summary>
            /// <value>高圧ガス施設名</value>
            public string HighPressureGasFacilityName { get; set; }
            /// <summary>Gets or sets 劣化感受性</summary>
            /// <value>劣化感受性</value>
            public string DeteriorationSensitivity { get; set; }
            /// <summary>Gets or sets 保全レベル</summary>
            /// <value>保全レベル</value>
            public string ConservationLevel { get; set; }
            /// <summary>Gets or sets 変更者</summary>
            /// <value>変更者</value>
            public string UpdateUserName { get; set; }
            /// <summary>Gets or sets 修正日</summary>
            /// <value>修正日</value>
            public string UpdateDate { get; set; }
            /// <summary>Gets or sets 承認者</summary>
            /// <value>承認者</value>
            public string ApprovalUserName { get; set; }
            /// <summary>Gets or sets 承認日</summary>
            /// <value>承認日</value>
            public string ApprovalDate { get; set; }
            /// <summary>Gets or sets 承認状況</summary>
            /// <value>承認状況</value>
            public string ApprovalStatus { get; set; }
            /// <summary>Gets or sets 部品種別</summary>
            /// <value>部品種別</value>
            public string PartsType { get; set; }
            /// <summary>Gets or sets 部品規格</summary>
            /// <value>部品規格</value>
            public string PartsStandard { get; set; }
            /// <summary>Gets or sets 型式名称</summary>
            /// <value>型式名称</value>
            public string ModelName { get; set; }
            /// <summary>Gets or sets 部品名</summary>
            /// <value>部品名</value>
            public string PartsName { get; set; }
            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string Dimensions { get; set; }

        }
    }
}
