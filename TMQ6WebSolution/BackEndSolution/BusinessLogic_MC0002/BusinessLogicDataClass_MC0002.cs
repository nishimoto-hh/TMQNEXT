using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicBase = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase;
using ComDao = CommonSTDUtil.CommonDataBaseClass;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using HistoryManagementDao = CommonTMQUtil.CommonTMQUtilDataClass;
using IListAccessor = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase.AccessorUtil.IListAccessor;
using TmqDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_MC0002
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_MC0002
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 機器別管理基準標準ID</summary>
            /// <value>機器別管理基準標準ID</value>
            public long ManagementStandardsId { get; set; }
            /// <summary>Gets or sets 機器別管理基準標準詳細ID</summary>
            /// <value>機器別管理基準標準詳細ID</value>
            public long? ManagementStandardsDetailId { get; set; }
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 最大更新日時</summary>
            /// <value>最大更新日時</value>
            public DateTime? MaxUpdateDatetime { get; set; }
        }

        /// <summary>
        /// 一覧画面 検索結果のデータクラス
        /// </summary>
        public class searchResultList : TmqDao.McManagementStandardsEntity, IListAccessor
        {
            /// <summary>Gets or sets 地区</summary>
            /// <value>地区</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場</summary>
            /// <value>工場</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets 職種</summary>
            /// <value>職種</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 機種大分類</summary>
            /// <value>機種大分類</value>
            public string LargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類</summary>
            /// <value>機種中分類</value>
            public string MiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類</summary>
            /// <value>機種小分類</value>
            public string SmallClassficationName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int JobId { get; set; }
            /// <summary>Gets or sets 最大更新日時</summary>
            /// <value>最大更新日時</value>
            public DateTime? MaxUpdateDatetime { get; set; }

            /// <summary>
            /// 一時テーブルレイアウト作成処理(性能改善対応)
            /// </summary>
            /// <param name="mapDic">マッピング情報のディクショナリ</param>
            /// <returns>一時テーブルレイアウト</returns>
            public dynamic GetTmpTableData(Dictionary<string, ComUtil.DBMappingInfo> mapDic)
            {
                dynamic paramObj;

                paramObj = new ExpandoObject() as IDictionary<string, object>;

                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ManagementStandardsName, nameof(this.ManagementStandardsName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.Memo, nameof(this.Memo), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DistrictName, nameof(this.DistrictName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FactoryName, nameof(this.FactoryName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobName, nameof(this.JobName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LargeClassficationName, nameof(this.LargeClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MiddleClassficationName, nameof(this.MiddleClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SmallClassficationName, nameof(this.SmallClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ManagementStandardsId, nameof(this.ManagementStandardsId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationFactoryStructureId, nameof(this.LocationFactoryStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateSerialid, nameof(this.UpdateSerialid), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.InsertDatetime, nameof(this.InsertDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.InsertUserId, nameof(this.InsertUserId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateDatetime, nameof(this.UpdateDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateUserId, nameof(this.UpdateUserId), mapDic);

                return paramObj;
            }
        }

        /// <summary>
        /// 詳細画面 保全項目一覧のデータクラス
        /// </summary>
        public class searchResultDetail : TmqDao.McManagementStandardsDetailEntity
        {
            /// <summary>Gets or sets 部位重要度</summary>
            /// <value>部位重要度</value>
            public string InspectionSiteImportanceName { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public string InspectionSiteConservationName { get; set; }
            /// <summary>Gets or sets 保全区分</summary>
            /// <value>保全区分</value>
            public string MaintainanceDivisionName { get; set; }
            /// <summary>Gets or sets 点検種別</summary>
            /// <value>点検種別</value>
            public string MaintainanceKindName { get; set; }
            /// <summary>Gets or sets スケジュール管理基準</summary>
            /// <value>スケジュール管理基準</value>
            public string ScheduleTypeName { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 点検内容ID</summary>
            /// <value>点検内容ID</value>
            public int? InspectionContentStructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public long FactoryId { get; set; }
        }

        /// <summary>
        /// 標準割当画面の検索条件のデータクラス
        /// </summary>
        public class searchConditionQuota : ComDao.SearchCommonClass
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
            /// <summary>Gets or sets 検索条件の場所階層IDをカンマ区切りにしたものを格納</summary>
            /// <value>検索条件の場所階層IDをカンマ区切りにしたものを格納</value>
            public string StrLocationStructureIdList { get; set; }
            /// <summary>Gets or sets 検索条件の職種機種階層IDをカンマ区切りにしたものを格納</summary>
            /// <value>検索条件の職種機種階層IDをカンマ区切りにしたものを格納</value>
            public string StrJobStcuctureIdList { get; set; }
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
            /// <summary>Gets or sets 点検種別毎管理</summary>
            /// <value>点検種別毎管理</value>
            public int? MaintainanceKindManageStructureId { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public int? ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets メーカー型式</summary>
            /// <value>メーカー型式</value>
            public string ManufacturerType { get; set; }
            /// <summary>Gets or sets 型式コード</summary>
            /// <value>型式コード</value>
            public string ModelNo { get; set; }
            /// <summary>Gets or sets 製造番号</summary>
            /// <value>製造番号</value>
            public string SerialNo { get; set; }
            /// <summary>Gets or sets 設置年月(From)</summary>
            /// <value>設置年月(From)</value>
            public DateTime? DateOfManufactureFrom { get; set; }
            /// <summary>Gets or sets 設置年月(To)</summary>
            /// <value>設置年月(To)</value>
            public DateTime? DateOfManufactureTo { get; set; }
            /// <summary>Gets or sets 重要度</summary>
            /// <value>重要度</value>
            public int? ImportanceStructureId { get; set; }
            /// <summary>Gets or sets 機器別管理基準</summary>
            /// <value>機器別管理基準</value>
            public int IsManagementStandards { get; set; }
            /// <summary>Gets or sets 機器別管理基準標準ID</summary>
            /// <value>機器別管理基準標準ID</value>
            public long ManagementStandardsId { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public int? UseSegmentStructureId { get; set; }
        }

        /// <summary>
        /// 場所階層・職種機種の階層設定用
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
        /// 標準割当画面 機器一覧のデータクラス
        /// </summary>
        public class MachineList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long MachineId { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public string LargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public string MiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public string SmallClassficationName { get; set; }
            /// <summary>Gets or sets 重要度</summary>
            /// <value>重要度</value>
            public string ImportanceName { get; set; }
            /// <summary>Gets or sets 開始日</summary>
            /// <value>開始日</value>
            public DateTime? StartDate { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 点検種別毎管理</summary>
            /// <value>点検種別毎管理</value>
            public bool MaintainanceKindManage { get; set; }
            /// <summary>Gets or sets 機器別管理基準有無</summary>
            /// <value>機器別管理基準有無</value>
            public string IsManagementStandardsExists { get; set; }
        }

        /// <summary>
        /// 部位IDと保全項目IDの組み合わせチェック用
        /// </summary>
        public class SiteAndContent
        {

            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long MachineId { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 保全項目ID</summary>
            /// <value>保全項目ID</value>
            public int? InspectionContentStructureId { get; set; }
        }

        /// <summary>
        /// 部位IDと保全項目IDの組み合わせチェック用
        /// </summary>
        public class CheckForUpdateScheduleType : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets スケジュール管理基準ID</summary>
            /// <value>スケジュール管理基準ID</value>
            public int? ScheduleTypeStructureId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long MachineId { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 点検種別</summary>
            /// <value>点検種別</value>
            public int? MaintainanceKindStructureId { get; set; }
        }

        /// <summary>
        /// 同一点検種別データの保全スケジュール詳細データ再作成用
        /// </summary>
        public class RemakeSchedule
        {
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long MachineId { get; set; }
            /// <summary>Gets or sets 点検種別</summary>
            /// <value>点検種別</value>
            public int? MaintainanceKindStructureId { get; set; }
            /// <summary>Gets or sets 保全スケジュールID</summary>
            /// <value>保全スケジュールID</value>
            public long MaintainanceScheduleId { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 開始日</summary>
            /// <value>開始日</value>
            public DateTime? StartDate { get; set; }
            /// <summary>Gets or sets 周期(年)</summary>
            /// <value>周期(年)</value>
            public int? CycleYear { get; set; }
            /// <summary>Gets or sets 周期(月)</summary>
            /// <value>周期(月)</value>
            public int? CycleMonth { get; set; }
            /// <summary>Gets or sets 周期(日)</summary>
            /// <value>周期(日)</value>
            public int? CycleDay { get; set; }
        }
    }
}
