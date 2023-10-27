using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDao = CommonSTDUtil.CommonDataBaseClass;
using TmqDao = CommonTMQUtil.TMQCommonDataClass;
using HistoryManagementDao = CommonTMQUtil.CommonTMQUtilDataClass;

namespace BusinessLogic_HM0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_HM0001
    {
        /// <summary>
        /// 一覧画面の検索条件
        /// </summary>
        public class searchCondition
        {
            /// <summary>Gets or sets 自分の件名のみ表示</summary>
            /// <value>自分の件名のみ表示</value>
            public int DispOnlyMySubject { get; set; }
            /// <summary>Gets or sets 承認権限有無</summary>
            /// <value>承認権限有無</value>
            public bool IsApprovalUser { get; set; }
        }

        /// <summary>
        /// 詳細画面の検索条件
        /// </summary>
        public class detailSearchCondition
        {
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long MachineId { get; set; }
            /// <summary>Gets or sets 処理モード(0:トランザクションモード,1:変更管理モード)</summary>
            /// <value>処理モード(0:トランザクションモード,1:変更管理モード)</value>
            public int ProcessMode { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
            /// <summary>Gets or sets ログインユーザID</summary>
            /// <value>ログインユーザID</value>
            public int UserId { get; set; }
        }

        /// <summary>
        /// 一覧画面・詳細画面・詳細編集画面の検索結果
        /// </summary>
        public class searchResult : TmqDao.HmMcMachineEntity, HistoryManagementDao.IHistoryManagementCommon
        {
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
            /// <summary>Gets or sets 製造日</summary>
            /// <value>製造日</value>
            public DateTime? DateOfManufacture { get; set; }
            /// <summary>Gets or sets 納期</summary>
            /// <value>納期</value>
            public int? DeliveryDate { get; set; }
            /// <summary>Gets or sets 機器メモ</summary>
            /// <value>機器メモ</value>
            public string EquipmentNote { get; set; }
            /// <summary>Gets or sets 適用法規</summary>
            /// <value>適用法規</value>
            public string ApplicableLawsStructureId { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public int? UseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 固定資産番号</summary>
            /// <value>固定資産番号</value>
            public string FixedAssetNo { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public int? InspectionSiteConservationStructureId { get; set; }
            /// <summary>Gets or sets 循環対象</summary>
            /// <value>循環対象</value>
            public bool CirculationTargetFlg { get; set; }
            /// <summary>Gets or sets 点検種別毎管理</summary>
            /// <value>点検種別毎管理</value>
            public bool MaintainanceKindManage { get; set; }
            /// <summary>Gets or sets 機器添付有無</summary>
            /// <value>機器添付有無</value>
            public string FileLinkEquipment { get; set; }
            /// <summary>Gets or sets 機番添付有無</summary>
            /// <value>機番添付有無</value>
            public string FileLinkMachine { get; set; }
            /// <summary>Gets or sets 機器別管理基準変更有無</summary>
            /// <value>機器別管理基準変更有無</value>
            public int IsChangedComponent { get; set; }
            /// <summary>Gets or sets 申請機能</summary>
            /// <value>申請機能</value>
            public string ConductName { get; set; }
            /// <summary>Gets or sets 申請者</summary>
            /// <value>申請者</value>
            public string ApplicationUserName { get; set; }
            /// <summary>Gets or sets 承認者</summary>
            /// <value>承認者</value>
            public string ApprovalUserName { get; set; }
            /// <summary>Gets or sets 申請日</summary>
            /// <value>申請日</value>
            public DateTime? ApplicationDate { get; set; }
            /// <summary>Gets or sets 承認日</summary>
            /// <value>承認日</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets 申請理由</summary>
            /// <value>申請理由</value>
            public string ApplicationReason { get; set; }
            /// <summary>Gets or sets 否認理由</summary>
            /// <value>否認理由</value>
            public string RejectionReason { get; set; }
            /// <summary>Gets or sets 申請区分</summary>
            /// <value>申請区分</value>
            public int ApplicationDivisionId { get; set; }
            /// <summary>Gets or sets 申請区分(拡張項目)</summary>
            /// <value>申請区分(拡張項目)</value>
            public string ApplicationDivisionCode { get; set; }
            /// <summary>Gets or sets 申請状況</summary>
            /// <value>申請状況</value>
            public int ApplicationStatusId { get; set; }
            /// <summary>Gets or sets 申請状況(拡張項目)</summary>
            /// <value>申請状況(拡張項目)</value>
            public string ApplicationStatusCode { get; set; }
            /// <summary>Gets or sets 変更のあった項目(District_10|Series_30...)</summary>
            /// <value>変更のあった項目(District_10|Series_30...)</value>
            public string ValueChanged { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            /// <summary>Gets or sets 処理モード(0：トランザクションモード、1：変更管理モード)</summary>
            /// <value>処理モード(0：トランザクションモード、1：変更管理モード)</value>
            public int ProcessMode { get; set; }
            /// <summary>Gets or sets 承認者フラグ(0：承認権限無し、承認権限有り)</summary>
            /// <value>承認者フラグ(0：承認権限無し、承認権限有り)</value>
            public int AbleApproval { get; set; }
            /// <summary>Gets or sets ボタン非表示制御フラグ(申請の申請者かシステム管理者の場合はTrue)</summary>
            /// <value>ボタン非表示制御フラグ(申請の申請者かシステム管理者の場合はTrue)</value>
            public bool IsCertified { get; set; }
            /// <summary>Gets or sets ボタン表示制御フラグ(変更管理IDが紐付く機番情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDの場合はTrue)</summary>
            /// <value>ボタン表示制御フラグ(変更管理IDが紐付く機番情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDの場合はTrue)</value>
            public bool IsCertifiedFactory { get; set; }
            /// <summary>Gets or sets 実行処理区分</summary>
            /// <value>実行処理区分</value>
            public int ExecutionDivision { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long EquipmentId { get; set; }
            /// <summary>Gets or sets 更新シリアルID(機番情報)</summary>
            /// <value>更新シリアルID(機番情報)</value>
            public int McUpdateSerialId { get; set; }
            /// <summary>Gets or sets 更新シリアルID(機器情報)</summary>
            /// <value>更新シリアルID(機器情報)</value>
            public int EqUpdateSerialId { get; set; }
            /// <summary>Gets or sets 画面表示タイプ(詳細編集画面で使用)</summary>
            /// <value>画面表示タイプ(詳細編集画面で使用)</value>
            public int DispType { get; set; }
            /// <summary>Gets or sets 機器情報変更管理ID</summary>
            /// <value>機器情報変更管理ID</value>
            public long HmEquipmentId { get; set; }
            /// <summary>Gets or sets キーIDに紐付く「承認済」以外のデータ有無</summary>
            /// <value>キーIDに紐付く「承認済」以外のデータ有無</value>
            public bool StatusCntExceptApproved { get; set; }

            #region 翻訳
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public string EquipmentLevel { get; set; }
            /// <summary>Gets or sets 重要度</summary>
            /// <value>重要度</value>
            public string ImportanceName { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public string InspectionSiteConservationName { get; set; }
            /// <summary>Gets or sets 適用法規</summary>
            /// <value>適用法規</value>
            public string ApplicableLawsName { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string ManufacturerName { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public string UseSegmentName { get; set; }
            /// <summary>Gets or sets 申請状況</summary>
            /// <value>申請状況</value>
            public string ApplicationStatusName { get; set; }
            /// <summary>Gets or sets 申請区分</summary>
            /// <value>申請区分</value>
            public string ApplicationDivisionName { get; set; }
            #endregion

            #region 地区・職種機種(変更管理テーブル)
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
            #endregion

            #region 地区・職種機種(トランザクションテーブル)
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? OldLocationStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? OldJobStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? OldDistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string OldDistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? OldFactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string OldFactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? OldPlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string OldPlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? OldSeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string OldSeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? OldStrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string OldStrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? OldFacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string OldFacilityName { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int OldJobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string OldJobName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? OldLargeClassficationId { get; set; }
            /// <summary>Gets or sets 機種大分類名称</summary>
            /// <value>機種大分類名称</value>
            public string OldLargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? OldMiddleClassficationId { get; set; }
            /// <summary>Gets or sets 機種中分類名称</summary>
            /// <value>機種中分類名称</value>
            public string OldMiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? OldSmallClassficationId { get; set; }
            /// <summary>Gets or sets 機種小分類名称</summary>
            /// <value>機種小分類名称</value>
            public string OldSmallClassficationName { get; set; }
            #endregion
        }

        /// <summary>
        /// 保全項目一覧のデータクラス
        /// </summary>
        public class managementStandardsResult : ComDao.CommonTableItem
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
            /// <summary>Gets or sets 申請区分(拡張項目)</summary>
            /// <value>申請区分(拡張項目)</value>
            public string ApplicationDivisionCode { get; set; }
            /// <summary>Gets or sets 変更のあった項目</summary>
            /// <value>変更のあった項目</value>
            public string ValueChanged { get; set; }
            /// <summary>Gets or sets 機器別管理基準部位変更管理ID</summary>
            /// <value>機器別管理基準部位変更管理ID</value>
            public long HmManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容変更管理ID</summary>
            /// <value>機器別管理基準内容変更管理ID</value>
            public long HmManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 実行処理区分</summary>
            /// <value>実行処理区分</value>
            public int ExecutionDivision { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            /// <summary>Gets or sets 保全スケジュール変更管理ID</summary>
            /// <value>保全スケジュール変更管理ID</value>
            public long HmMaintainanceScheduleId { get; set; }
        }

        /// <summary>
        /// 変更管理詳細情報
        /// </summary>
        public class historyManagmentDetail : TmqDao.HmHistoryManagementEntity
        {
            /// <summary>Gets or sets 実行処理区分</summary>
            /// <value>実行処理区分</value>
            public int ExecutionDivision { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long MachineId { get; set; }
            /// <summary>Gets or sets 機番情報変更管理ID</summary>
            /// <value>機番情報変更管理ID</value>
            public long HmMachineId { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long EquipmentId { get; set; }
            /// <summary>Gets or sets 機器情報変更管理ID</summary>
            /// <value>機器情報変更管理ID</value>
            public long HmEquipmentId { get; set; }
            /// <summary>Gets or sets 適用法規</summary>
            /// <value>適用法規</value>
            public string ApplicableLawsStructureId { get; set; }
            /// <summary>Gets or sets 機器別管理基準部位ID</summary>
            /// <value>機器別管理基準部位ID</value>
            public long ManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 機器別管理基準部位変更管理ID</summary>
            /// <value>機器別管理基準部位変更管理ID</value>
            public long HmManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容変更管理ID</summary>
            /// <value>機器別管理基準内容変更管理ID</value>
            public long HmManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 保全スケジュールID</summary>
            /// <value>保全スケジュールID</value>
            public long MaintainanceScheduleId { get; set; }
            /// <summary>Gets or sets 保全スケジュール変更管理ID</summary>
            /// <value>保全スケジュール変更管理ID</value>
            public long HmMaintainanceScheduleId { get; set; }
        }

        /// <summary>
        /// 保全項目一覧の登録・削除処理で使用する条件
        /// </summary>
        public class managementStandardsCondition
        {
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long MachineId { get; set; }
            /// <summary>Gets or sets 機器別管理基準部位ID</summary>
            /// <value>機器別管理基準部位ID</value>
            public long ManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long ManagementStandardsContentId { get; set; }
        }
    }
}
