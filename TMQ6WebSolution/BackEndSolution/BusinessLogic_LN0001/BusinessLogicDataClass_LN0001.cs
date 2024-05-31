using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using IListAccessor = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase.AccessorUtil.IListAccessor;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using BusinessLogicBase = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase;

namespace BusinessLogic_LN0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_LN0001
    {
        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class ListSearchResult : ComDao.LnLongPlanEntity, IListAccessor
        {
            /// <summary>Gets or sets 機器添付</summary>
            /// <value>機器添付</value>
            public string FileLinkEquip { get; set; }
            /// <summary>Gets or sets 件名添付</summary>
            /// <value>件名添付</value>
            public string FileLinkSubject { get; set; }
            /// <summary>Gets or sets 担当(名前)</summary>
            /// <value>担当(名前)</value>
            public string PersonName { get; set; }
            /// <summary>Gets or sets スケジュール紐付け用キーID</summary>
            /// <value>スケジュール紐付け用キーID</value>
            public string KeyId { get; set; }
            /// <summary>Gets or sets 準備対象</summary>
            /// <value>準備対象</value>
            public bool PreparationFlg { get; set; }

            #region 共通　地区・職種設定用
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

            #region 翻訳
            /// <summary>Gets or sets 保全時期(翻訳)</summary>
            /// <value>保全時期(翻訳)</value>
            public string MaintenanceSeasonName { get; set; }
            /// <summary>Gets or sets 作業項目(翻訳)</summary>
            /// <value>作業項目(翻訳)</value>
            public string WorkItemName { get; set; }
            /// <summary>Gets or sets 予算管理区分(翻訳)</summary>
            /// <value>予算管理区分(翻訳)</value>
            public string BudgetManagementName { get; set; }
            /// <summary>Gets or sets 予算性格区分(翻訳)</summary>
            /// <value>予算性格区分(翻訳)</value>
            public string BudgetPersonalityName { get; set; }
            /// <summary>Gets or sets 目的区分(翻訳)</summary>
            /// <value>目的区分(翻訳)</value>
            public string PurposeName { get; set; }
            /// <summary>Gets or sets 作業区分(翻訳)</summary>
            /// <value>作業区分(翻訳)</value>
            public string WorkClassName { get; set; }
            /// <summary>Gets or sets 処置区分(翻訳)</summary>
            /// <value>処置区分(翻訳)</value>
            public string TreatmentName { get; set; }
            /// <summary>Gets or sets 設備区分(翻訳)</summary>
            /// <value>設備区分(翻訳)</value>
            public string FacilityStructureName { get; set; }
            /// <summary>Gets or sets 設備区分(翻訳)</summary>
            /// <value>設備区分(翻訳)</value>
            public int? LongPlanDivisionStructureId { get; set; }
            /// <summary>Gets or sets 設備区分(翻訳)</summary>
            /// <value>設備区分(翻訳)</value>
            public int? LongPlanGroupStructureId { get; set; }
            /// <summary>Gets or sets 長計区分(翻訳)</summary>
            /// <value>長計区分(翻訳)</value>
            public string LongPlanDivisionName { get; set; }
            /// <summary>Gets or sets 長計グループ(翻訳)</summary>
            /// <value>長計グループ(翻訳)</value>
            public string LongPlanGroupName { get; set; }

            #endregion

            #region 参照画面
            /// <summary>Gets or sets 参照画面　保全情報一覧(点検種別)の表示フラグ</summary>
            /// <value>参照画面　保全情報一覧(点検種別)の表示フラグ</value>
            public bool IsDisplayMaintainanceKind { get; set; }

            #region 排他チェック
            // 機器別管理基準内容(排他チェック)
            /// <summary>Gets or sets 機器別管理基準内容の最大の更新日時</summary>
            /// <value>機器別管理基準内容の最大の更新日時</value>
            public DateTime? McManStConUpdateDatetime { get; set; }
            // スケジュール詳細(排他チェック)
            /// <summary>Gets or sets スケジュール詳細の最大の更新日時</summary>
            /// <value>スケジュール詳細の最大の更新日時</value>
            public DateTime? ScheDetailUpdateDatetime { get; set; }
            // 添付情報(排他チェック)
            /// <summary>Gets or sets 添付情報の最大の更新日時</summary>
            /// <value>添付情報の最大の更新日時</value>
            public DateTime? AttachmentUpdateDatetime { get; set; }
            #endregion
            #endregion

            /// <summary>
            /// 一時テーブルレイアウト作成処理(性能改善対応)
            /// </summary>
            /// <param name="mapDic">マッピング情報のディクショナリ</param>
            /// <returns>一時テーブルレイアウト</returns>
            public dynamic GetTmpTableData(Dictionary<string, ComUtil.DBMappingInfo> mapDic)
            {
                dynamic paramObj;

                paramObj = new ExpandoObject() as IDictionary<string, object>;
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FileLinkEquip, nameof(this.FileLinkEquip), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FileLinkSubject, nameof(this.FileLinkSubject), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PersonName, nameof(this.PersonName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.KeyId, nameof(this.KeyId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PreparationFlg, nameof(this.PreparationFlg), mapDic);
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
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobId, nameof(this.JobId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobName, nameof(this.JobName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LargeClassficationId, nameof(this.LargeClassficationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LargeClassficationName, nameof(this.LargeClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MiddleClassficationId, nameof(this.MiddleClassficationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MiddleClassficationName, nameof(this.MiddleClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SmallClassficationId, nameof(this.SmallClassficationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SmallClassficationName, nameof(this.SmallClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MaintenanceSeasonName, nameof(this.MaintenanceSeasonName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.WorkItemName, nameof(this.WorkItemName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.BudgetManagementName, nameof(this.BudgetManagementName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.BudgetPersonalityName, nameof(this.BudgetPersonalityName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PurposeName, nameof(this.PurposeName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.WorkClassName, nameof(this.WorkClassName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TreatmentName, nameof(this.TreatmentName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FacilityStructureName, nameof(this.FacilityStructureName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.IsDisplayMaintainanceKind, nameof(this.IsDisplayMaintainanceKind), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.McManStConUpdateDatetime, nameof(this.McManStConUpdateDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ScheDetailUpdateDatetime, nameof(this.ScheDetailUpdateDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.AttachmentUpdateDatetime, nameof(this.AttachmentUpdateDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TableName, nameof(this.TableName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LongPlanId, nameof(this.LongPlanId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.Subject, nameof(this.Subject), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationStructureId, nameof(this.LocationStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationDistrictStructureId, nameof(this.LocationDistrictStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationFactoryStructureId, nameof(this.LocationFactoryStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationPlantStructureId, nameof(this.LocationPlantStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationSeriesStructureId, nameof(this.LocationSeriesStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationStrokeStructureId, nameof(this.LocationStrokeStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationFacilityStructureId, nameof(this.LocationFacilityStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobStructureId, nameof(this.JobStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobKindStructureId, nameof(this.JobKindStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobLargeClassficationStructureId, nameof(this.JobLargeClassficationStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobMiddleClassficationStructureId, nameof(this.JobMiddleClassficationStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobSmallClassficationStructureId, nameof(this.JobSmallClassficationStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SubjectNote, nameof(this.SubjectNote), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PersonId, nameof(this.PersonId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.WorkItemStructureId, nameof(this.WorkItemStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.BudgetManagementStructureId, nameof(this.BudgetManagementStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.BudgetPersonalityStructureId, nameof(this.BudgetPersonalityStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MaintenanceSeasonStructureId, nameof(this.MaintenanceSeasonStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PurposeStructureId, nameof(this.PurposeStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.WorkClassStructureId, nameof(this.WorkClassStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TreatmentStructureId, nameof(this.TreatmentStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FacilityStructureId, nameof(this.FacilityStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateSerialid, nameof(this.UpdateSerialid), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.InsertDatetime, nameof(this.InsertDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.InsertUserId, nameof(this.InsertUserId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateDatetime, nameof(this.UpdateDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateUserId, nameof(this.UpdateUserId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DeleteFlg, nameof(this.DeleteFlg), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LanguageId, nameof(this.LanguageId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LongPlanDivisionName, nameof(this.LongPlanDivisionName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LongPlanGroupName, nameof(this.LongPlanGroupName), mapDic);
                //                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LongPlanDivisionStructureId, nameof(this.LongPlanDivisionStructureId), mapDic);
                //                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LongPlanGroupStructureId, nameof(this.LongPlanGroupStructureId), mapDic);

                return paramObj;
            }
        }

        /// <summary>
        /// ExelPort用データクラス
        /// </summary>
        public class ExcelPort
        {
            /// <summary>
            /// 実行条件
            /// </summary>
            public class Condition
            {
                /// <summary>Gets or sets 長期計画件名ID</summary>
                /// <value>長期計画件名ID</value>
                public string LongPlanIdList { get; set; }
                /// <summary>Gets or sets 機器別管理基準内容IDリスト</summary>
                /// <value>機器別管理基準内容IDリスト</value>
                public string ManagementStandardsContentIdList { get; set; }
                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                public long? ManagementStandardsContentId { get; set; }
                /// <summary>Gets or sets 保全スケジュールID</summary>
                /// <value>保全スケジュールID</value>
                public long? MaintainanceScheduleId { get; set; }
                /// <summary>Gets or sets スケジュール開始年月</summary>
                /// <value>スケジュール開始年月</value>
                /// <remarks>"yyyyMM"フォーマット</remarks>
                public string ScheduleDateFrom { get; set; }
                /// <summary>Gets or sets スケジュール終了年月</summary>
                /// <value>スケジュール終了年月</value>
                /// <remarks>"yyyyMM"フォーマット</remarks>
                public string ScheduleDateTo { get; set; }
                /// <summary>Gets or sets 言語ID</summary>
                /// <value>言語ID</value>
                public string LanguageId { get; set; }
            }

            /// <summary>
            /// スケジュール詳細取得結果一覧
            /// </summary>
            public class Result : InputCheck.Result
            {
                /// <summary>Gets or sets 長期計画件名ID</summary>
                /// <value>長期計画件名ID</value>
                public long LongPlanId { get; set; }
            }

            /// <summary>
            /// 長期計画一覧
            /// </summary>
            public class List : ListSearchResult
            {
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int? RowNo { get; set; }
                /// <summary>Gets or sets 送信時処理ID</summary>
                /// <value>送信時処理ID</value>
                public long? ProcessId { get; set; }
            }

            /// <summary>
            /// 機器別管理基準一覧
            /// </summary>
            public class DeitalList: ComDao.CommonTableItem
            {
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int? RowNo { get; set; }
                /// <summary>Gets or sets 送信時処理ID</summary>
                /// <value>送信時処理ID</value>
                public long? ProcessId { get; set; }

                // 機番情報
                /// <summary>Gets or sets 機器番号</summary>
                /// <value>機器番号</value>
                public string MachineNo { get; set; }
                /// <summary>Gets or sets 機器名称</summary>
                /// <value>機器名称</value>
                public string MachineName { get; set; }
                /// <summary>Gets or sets 場所階層ID</summary>
                /// <value>場所階層ID</value>
                public int? LocationStructureId { get; set; }
                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int? FactoryId { get; set; }
                /// <summary>Gets or sets 地区ID</summary>
                /// <value>地区ID</value>
                public int? DistrictId { get; set; }
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
                /// <summary>Gets or sets 職種機種ID</summary>
                /// <value>職種機種ID</value>
                public int? JobStructureId { get; set; }
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

                // 機器別管理基準内容
                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                public long ManagementStandardsContentId { get; set; }
                /// <summary>Gets or sets 点検内容ID</summary>
                /// <value>点検内容ID</value>
                public int? InspectionContentStructureId { get; set; }

                // 機器別管理基準部位
                /// <summary>Gets or sets 機器別管理基準部位ID</summary>
                /// <value>機器別管理基準部位ID</value>
                public long ManagementStandardsComponentId { get; set; }
                /// <summary>Gets or sets 保全部位ID</summary>
                /// <value>保全部位ID</value>
                public int? InspectionSiteId { get; set; }

                // 長期計画件名情報
                /// <summary>Gets or sets 長期計画件名ID</summary>
                /// <value>長期計画件名ID</value>
                public new long? LongPlanId { get; set; }
                /// <summary>Gets or sets 件名</summary>
                /// <value>件名</value>
                public string Subject { get; set; }
                /// <summary>Gets or sets 件名メモ</summary>
                /// <value>件名メモ</value>
                public string SubjectNote { get; set; }
                /// <summary>Gets or sets 担当者名</summary>
                /// <value>担当者名</value>
                public string PersonName { get; set; }

                // 保全スケジュール情報
                /// <summary>Gets or sets 保全スケジュールID</summary>
                /// <value>保全スケジュールID</value>
                public long? MaintainanceScheduleId { get; set; }

                /// <summary>Gets or sets 開始日</summary>
                /// <value>開始日</value>
                public DateTime StartDate { get; set; }

                // 翻訳
                /// <summary>Gets or sets 地区名称</summary>
                /// <value>地区名称</value>
                public string DistrictName { get; set; }
                /// <summary>Gets or sets 工場名称</summary>
                /// <value>工場名称</value>
                public string FactoryName { get; set; }
                /// <summary>Gets or sets プラント名称</summary>
                /// <value>プラント名称</value>
                public string PlantName { get; set; }
                /// <summary>Gets or sets 系列名称</summary>
                /// <value>系列名称</value>
                public string SeriesName { get; set; }
                /// <summary>Gets or sets 工程名称</summary>
                /// <value>工程名称</value>
                public string StrokeName { get; set; }
                /// <summary>Gets or sets 設備名称</summary>
                /// <value>設備名称</value>
                public string FacilityName { get; set; }
                /// <summary>Gets or sets 職種名称</summary>
                /// <value>職種名称</value>
                public string JobName { get; set; }
                /// <summary>Gets or sets 機種大分類名称</summary>
                /// <value>機種大分類名称</value>
                public string LargeClassficationName { get; set; }
                /// <summary>Gets or sets 機種中分類名称</summary>
                /// <value>機種中分類名称</value>
                public string MiddleClassficationName { get; set; }
                /// <summary>Gets or sets 機種小分類名称</summary>
                /// <value>機種小分類名称</value>
                public string SmallClassficationName { get; set; }

                /// <summary>Gets or sets 保全部位名称</summary>
                /// <value>保全部位名称</value>
                public string InspectionSiteName { get; set; }
                /// <summary>Gets or sets 保全項目名称</summary>
                /// <value>保全項目名称</value>
                public string InspectionContentName { get; set; }

                /// <summary>Gets or sets スケジュールリスト</summary>
                /// <value>スケジュールリスト</value>
                public List<CommonTMQUtil.ScheduleInfo> ScheduleList { get; set; }
            }
        }

        /// <summary>
        /// 非表示項目設定用のデータクラス
        /// </summary>
        public class HiddenInfo
        {
            /// <summary>Gets or sets 変更管理の制御用フラグ</summary>
            /// <value>変更管理の制御用フラグ</value>
            /// <remarks>変更管理を行う工場の場合1</remarks>
            public int IsHistoryManagementFlg { get; set; }
        }

        /// <summary>
        /// 参照画面
        /// </summary>
        public class Detail
        {
            /// <summary>
            /// 保全情報一覧
            /// </summary>
            public class List
            {
                // 長期計画
                /// <summary>Gets or sets 長期計画件名ID</summary>
                /// <value>長期計画件名ID</value>
                public long LongPlanId { get; set; }
                /// <summary>Gets or sets 件名</summary>
                /// <value>件名</value>
                public string Subject { get; set; }
                /// <summary>Gets or sets 件名メモ</summary>
                /// <value>件名メモ</value>
                public string SubjectNote { get; set; }
                /// <summary>Gets or sets 担当者名</summary>
                /// <value>担当者名</value>
                public string PersonName { get; set; }

                // 機器別管理基準内容
                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                public long ManagementStandardsContentId { get; set; }
                /// <summary>Gets or sets 更新シリアルID</summary>
                /// <value>更新シリアルID</value>
                public int UpdateSerialidContent { get; set; }
                /// <summary>Gets or sets 点検内容ID</summary>
                /// <value>点検内容ID</value>
                public int? InspectionContentStructureId { get; set; }
                /// <summary>Gets or sets 予算金額</summary>
                /// <value>予算金額</value>
                public decimal? BudgetAmount { get; set; }
                /// <summary>Gets or sets 点検種別</summary>
                /// <value>点検種別</value>
                public int? MaintainanceKindStructureId { get; set; }
                /// <summary>Gets or sets スケジュール管理基準ID</summary>
                /// <value>スケジュール管理基準ID</value>
                public int? ScheduleTypeStructureId { get; set; }
                // 機器別管理基準部位
                /// <summary>Gets or sets 機器別管理基準部位ID</summary>
                /// <value>機器別管理基準部位ID</value>
                public long ManagementStandardsComponentId { get; set; }
                /// <summary>Gets or sets 更新シリアルID</summary>
                /// <value>更新シリアルID</value>
                public int UpdateSerialidComponent { get; set; }
                /// <summary>Gets or sets 部位ID</summary>
                /// <value>部位ID</value>
                public int? InspectionSiteStructureId { get; set; }

                // 機番情報
                /// <summary>Gets or sets 機番ID</summary>
                /// <value>機番ID</value>
                public long MachineId { get; set; }
                /// <summary>Gets or sets 機器番号</summary>
                /// <value>機器番号</value>
                public string MachineNo { get; set; }
                /// <summary>Gets or sets 機器名称</summary>
                /// <value>機器名称</value>
                public string MachineName { get; set; }
                /// <summary>Gets or sets 重要度</summary>
                /// <value>重要度</value>
                public int? ImportanceStructureId { get; set; }
                /// <summary>Gets or sets 機器重要度名称</summary>
                /// <value>機器重要度名称</value>
                public string ImportanceName { get; set; }

                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int? FactoryId { get; set; }
                /// <summary>Gets or sets 場所階層ID</summary>
                /// <value>場所階層ID</value>
                public int? LocationStructureId { get; set; }
                /// <summary>Gets or sets 職種機種ID</summary>
                /// <value>職種機種ID</value>
                public int? JobStructureId { get; set; }

                // 保全スケジュール
                /// <summary>Gets or sets 保全スケジュールID</summary>
                /// <value>保全スケジュールID</value>
                public long? MaintainanceScheduleId { get; set; }
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
                /// <summary>Gets or sets スケジュール日</summary>
                /// <value>スケジュール日</value>
                public DateTime? ScheduleDate { get; set; }
                /// <summary>Gets or sets スケジュール日(変更前)</summary>
                /// <value>スケジュール日(変更前)</value>
                public DateTime? ScheduleDateBefore { get; set; }

                // 添付情報(排他チェック)
                /// <summary>Gets or sets 添付情報の最大の更新日時</summary>
                /// <value>添付情報の最大の更新日時</value>
                public DateTime? AttachmentUpdateDatetime { get; set; }

                /// <summary>Gets or sets スケジュール紐付け用キーID</summary>
                /// <value>スケジュール紐付け用キーID</value>
                public string KeyId { get; set; }

                // スケジュール(排他チェック)
                /// <summary>Gets or sets スケジュールヘッダの最大の更新日時</summary>
                /// <value>スケジュールヘッダの最大の更新日時</value>
                public DateTime? ScheduleHeadUpdtime { get; set; }
                /// <summary>Gets or sets スケジュール詳細の最大の更新日時</summary>
                /// <value>スケジュール詳細の最大の更新日時</value>
                public DateTime? ScheduleDetailUpdtime { get; set; }

                /// <summary>Gets or sets 同一マークキー</summary>
                /// <value>同一マークキー</value>
                /// <remarks>スケジュールと同じ、排他キーに使用</remarks>
                public string SameMarkKey { get; set; }

                #region 機器別管理基準選択 メモリ改善対応
                /// <summary>Gets or sets 予算金額</summary>
                /// <value>予算金額</value>
                public string BudgetAmountFormat { get; set; }
                /// <summary>Gets or sets 周期(年)</summary>
                /// <value>周期(年)</value>
                public string CycleYearFormat { get; set; }
                /// <summary>Gets or sets 周期(月)</summary>
                /// <value>周期(月)</value>
                public string CycleMonthFormat { get; set; }
                /// <summary>Gets or sets 周期(日)</summary>
                /// <value>周期(日)</value>
                public string CycleDayFormat { get; set; }
                /// <summary>Gets or sets 保全部位</summary>
                /// <value>保全部位</value>
                public string InspectionSiteName { get; set; }
                /// <summary>Gets or sets 保全項目</summary>
                /// <value>保全項目</value>
                public string InspectionContentName { get; set; }
                /// <summary>Gets or sets スケジュール管理</summary>
                /// <value>スケジュール管理</value>
                public string ScheduleTypeName { get; set; }
                /// <summary>Gets or sets 点検種別</summary>
                /// <value>点検種別</value>
                public string MaintainanceKindName { get; set; }
                #endregion
            }

            /// <summary>
            /// スケジュール一覧
            /// </summary>
            public class ScheduleList
            {
                // 機器別管理基準内容
                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                public long ManagementStandardsContentId { get; set; }

                // 保全スケジュール
                /// <summary>Gets or sets 保全スケジュールID</summary>
                /// <value>保全スケジュールID</value>
                public long MaintainanceScheduleId { get; set; }

                // 保全スケジュール詳細
                /// <summary>Gets or sets 保全スケジュール詳細ID</summary>
                /// <value>保全スケジュール詳細ID</value>
                public long MaintainanceScheduleDetailId { get; set; }
                /// <summary>Gets or sets スケジュール日</summary>
                /// <value>スケジュール日</value>
                public DateTime ScheduleDate { get; set; }
                /// <summary>Gets or sets 完了フラグ</summary>
                /// <value>完了フラグ</value>
                public bool Completion { get; set; }

                /// <summary>Gets or sets スケジュールID</summary>
                /// <value>スケジュールID</value>
                public int ScheduleId { get; set; }
                /// <summary>Gets or sets スケジュール文字列</summary>
                /// <value>スケジュール文字列</value>
                public string Schedule { get; set; }
            }

            /// <summary>
            /// 表示条件
            /// </summary>
            public class Condition : TMQDao.ScheduleList.Condition
            {
                /// <summary>Gets or sets スケジュール表示計画内容</summary>
                /// <value>スケジュール表示計画内容</value>
                public int SchedulePlanContent { get; set; }

                /// <summary>Gets or sets スケジュール表示計画内容(拡張項目の値)</summary>
                /// <value>スケジュール表示計画内容(拡張項目の値)</value>
                public int SchedulePlanContentExtension { get; set; }
            }

        }

        /// <summary>
        /// 入力チェックで使用するデータクラス
        /// </summary>
        public class InputCheck
        {
            /// <summary>
            /// 入力チェック用SQLの取得結果
            /// </summary>
            public class Result
            {
                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                public long ManagementStandardsContentId { get; set; }
                /// <summary>Gets or sets 保全スケジュール詳細ID</summary>
                /// <value>保全スケジュール詳細ID</value>
                public long MaintainanceScheduleDetailId { get; set; }
                /// <summary>Gets or sets スケジュール日</summary>
                /// <value>スケジュール日</value>
                public DateTime ScheduleDate { get; set; }
                /// <summary>Gets or sets 保全活動件名ID</summary>
                /// <value>保全活動件名ID</value>
                /// <remarks>ma_summaryのINSERTの戻り値</remarks>
                public int? SummaryId { get; set; }
                /// <summary>Gets or sets スケジュール日の年月</summary>
                /// <value>スケジュール日の年月</value>
                public string ScheduleYm { get; set; }
                /// <summary>Gets or sets 完了フラグ</summary>
                /// <value>完了フラグ</value>
                public bool? Complition { get; set; }
            }
            /// <summary>
            /// 入力チェックの引数
            /// </summary>
            public class Condition
            {
                /// <summary>Gets or sets 長期計画件名ID</summary>
                /// <value>長期計画件名ID</value>
                /// <remarks>一覧画面で選択された値</remarks>
                public long LongPlanId { get; set; }
                /// <summary>Gets or sets スケジュール日</summary>
                /// <value>スケジュール日</value>
                /// <remarks>画面の入力値</remarks>
                public DateTime ScheduleDateFrom { get; set; }
                /// <summary>Gets or sets スケジュール日</summary>
                /// <value>スケジュール日</value>
                /// <remarks>画面の入力値</remarks>
                public DateTime ScheduleDateTo { get; set; }

                // 保全活動作成画面
                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                /// <remarks>一覧の選択行</remarks>
                public List<long> ManagementStandardsContentIdList { get; set; }
            }
        }

        /// <summary>
        /// 計画一括作成画面で使用するデータクラス
        /// </summary>
        public class MakePlan
        {
            /// <summary>
            /// 計画一括作成画面で登録に使用するデータクラス
            /// </summary>
            public class Condition : ComDao.CommonTableItem
            {
                /// <summary>Gets or sets 長期計画件名ID</summary>
                /// <value>長期計画件名ID</value>
                /// <remarks>一覧画面で選択された値</remarks>
                public long LongPlanId { get; set; }
                /// <summary>Gets or sets 施工担当者ID</summary>
                /// <value>施工担当者ID</value>
                /// <remarks>画面の入力値</remarks>
                public int? ConstructionPersonnelId { get; set; }
                /// <summary>Gets or sets スケジュール日</summary>
                /// <value>スケジュール日</value>
                /// <remarks>画面の入力値</remarks>
                public DateTime ScheduleDateFrom { get; set; }
                /// <summary>Gets or sets スケジュール日</summary>
                /// <value>スケジュール日</value>
                /// <remarks>画面の入力値</remarks>
                public DateTime ScheduleDateTo { get; set; }

                /// <summary>Gets or sets 保全活動件名ID</summary>
                /// <value>保全活動件名ID</value>
                /// <remarks>ma_summaryのINSERTの戻り値</remarks>
                public long SummaryId { get; set; }
                /// <summary>Gets or sets 保全履歴ID</summary>
                /// <value>保全履歴ID</value>
                /// <remarks>ma_historyのINSERTの戻り値</remarks>
                public long HistoryId { get; set; }
                /// <summary>Gets or sets 保全履歴機器ID</summary>
                /// <value>保全履歴機器ID</value>
                /// <remarks>ma_history_machineのINSERTの戻り値</remarks>
                public long HistoryMachineId { get; set; }
                /// <summary>Gets or sets 保全履歴機器部位ID</summary>
                /// <value>保全履歴機器部位ID</value>
                /// <remarks>ma_history_inspection_siteのINSERTの戻り値</remarks>
                public long HistoryInspectionSiteId { get; set; }

                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                /// <remarks>処理対象の値</remarks>
                public long ManagementStandardsContentId { get; set; }
                /// <summary>Gets or sets 保全スケジュール詳細ID</summary>
                /// <value>保全スケジュール詳細ID</value>
                /// <remarks>処理対象の値</remarks>
                public long MaintainanceScheduleDetailId { get; set; }

                /// <summary>Gets or sets 構成マスタ検索対象の工場ID</summary>
                /// <value>構成マスタ検索対象の工場ID</value>
                public List<int> FactoryIdList { get; set; }
                /// <summary>Gets or sets 言語ID</summary>
                /// <value>言語ID</value>
                public string LanguageId { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="languageId">言語ID</param>
                public Condition(string languageId)
                {
                    this.LanguageId = languageId;
                }
            }
        }

        /// <summary>
        /// 保全計画作成画面
        /// </summary>
        public class MakeMaintainance
        {
            /// <summary>
            /// 保全計画作成画面で登録に使用するデータクラス
            /// </summary>
            public class Condition : MakePlan.Condition
            {
                /// <summary>Gets or sets 件名</summary>
                /// <value>件名</value>
                public string Subject { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="languageId">言語ID</param>
                public Condition(string languageId) : base(languageId)
                {
                }
            }
        }

        /// <summary>
        /// 機器別管理基準選択画面
        /// </summary>
        public class SelectStandard
        {
            /// <summary>
            /// 入力チェック用
            /// </summary>
            public class InputCheck
            {
                /// <summary>
                /// 検索条件
                /// </summary>
                public class CondIdList
                {
                    /// <summary>Gets or sets キーIDリスト</summary>
                    /// <value>キーIDリスト</value>
                    public List<long> KeyIdList { get; set; }
                }
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
            /// 検索条件
            /// </summary>
            public class SearchCondition : ComDao.SearchCommonClass
            {
                #region 共通　地区・職種設定用
                // IDなのに変数名が名称だが、これは画面定義を共用している関係。変数名以外はIDと同じとする。
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
                #endregion
                // 実際に検索で用いる値、上記共通の項目より取得
                /// <summary>Gets or sets 機能場所階層ID</summary>
                /// <value>機能場所階層ID</value>
                public List<int> LocationStructureIdList { get; set; }
                /// <summary>Gets or sets 職種機種階層ID</summary>
                /// <value>職種機種階層ID</value>
                public List<int> JobStructureIdList { get; set; }

                // 機番情報
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

                // 機器情報
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

                // 機器別管理基準内容
                /// <summary>Gets or sets スケジュール管理基準ID</summary>
                /// <value>スケジュール管理基準ID</value>
                public int? ScheduleTypeStructureId { get; set; }

                /// <summary>Gets or sets 点検種別毎管理</summary>
                /// <value>点検種別毎管理</value>
                public int? MaintainanceKindManageStructureId { get; set; }
                /// <summary>Gets or sets 部位ID</summary>
                /// <value>部位ID</value>
                public int? InspectionSiteStructureId { get; set; }
                /// <summary>Gets or sets 保全項目ID</summary>
                /// <value>保全項目ID</value>
                public int? InspectionContentStructureId { get; set; }
                /// <summary>Gets or sets 言語ID</summary>
                /// <value>言語ID</value>
                public string LanguageId { get; set; }
                /// <summary>Gets or sets 検索条件の場所階層IDをカンマ区切りにしたものを格納</summary>
                /// <value>検索条件の場所階層IDをカンマ区切りにしたものを格納</value>
                public string StrLocationStructureIdList { get; set; }
                /// <summary>Gets or sets 検索条件の職種機種階層IDをカンマ区切りにしたものを格納</summary>
                /// <value>検索条件の職種機種階層IDをカンマ区切りにしたものを格納</value>
                public string StrJobStcuctureIdList { get; set; }
            }
        }

        /// <summary>
        /// 予定作業一括延期画面
        /// </summary>
        public class Postpone
        {
            /// <summary>
            /// 条件エリア
            /// </summary>
            public class Condition
            {
                /// <summary>Gets or sets 延期対象年月</summary>
                /// <value>延期対象年月</value>
                public DateTime PostponeDate { get; set; }

                /// <summary>Gets or sets 延期月数</summary>
                /// <value>延期月数</value>
                public int PostponeMonths { get; set; }
            }
            /// <summary>
            /// 一括延期処理SQLの実行条件
            /// </summary>
            public class SqlCondition : ComDao.CommonTableItem
            {
                /// <summary>Gets or sets 長期計画件名ID</summary>
                /// <value>長期計画件名ID</value>
                /// <remarks>一覧画面で選択された値</remarks>
                public long LongPlanId { get; set; }

                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                /// <remarks>一覧の選択行</remarks>
                public List<long> ManagementStandardsContentIdList { get; set; }

                /// <summary>Gets or sets 延期対象年月</summary>
                /// <value>延期対象年月</value>
                public DateTime PostponeDate { get; set; }

                /// <summary>Gets or sets 延期月数</summary>
                /// <value>延期月数</value>
                public int PostponeMonths { get; set; }
            }
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
                /// <summary>Gets or sets 長期計画IDリスト(カンマ区切り)</summary>
                /// <value>長期計画IDリスト(カンマ区切り)</value>
                public string LongPlanIdList { get; set; }

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
            /// スケジュールの検索条件(工場ID+長期計画件名ID)
            /// </summary>
            public class SearchConditionLongPlanId : SearchCondition
            {
                /// <summary>Gets or sets 長期計画件名ID</summary>
                /// <value>長期計画件名ID</value>
                public long LongPlanId { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="condition">画面の検索条件</param>
                /// <param name="longPlanId">長期計画件名ID</param>
                /// <param name="monthStartNendo">年度開始月</param>
                /// <param name="languageId">言語ID</param>
                public SearchConditionLongPlanId(TMQDao.ScheduleList.Condition condition, long longPlanId, int monthStartNendo, string languageId) : base(condition, monthStartNendo, languageId)
                {
                    this.LongPlanId = longPlanId;
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
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="languageId">言語ID</param>
            public DetailCondition(string languageId)
            {
                this.LanguageId = languageId;
            }
        }

        /// <summary>
        /// 予算出力スケジュール表示条件
        /// </summary>
        public class BudgetOutputScheduleCondition
        {
            /// <summary>Gets or sets スケジュール表示単位</summary>
            /// <value>スケジュール表示単位</value>
            public int? ScheduleUnit { get; set; }
            /// <summary>Gets or sets スケジュール表示期間</summary>
            /// <value>スケジュール表示期間</value>
            public string ScheduleYear { get; set; }
            /// <summary>Gets or sets スケジュール表示期間From</summary>
            /// <value>スケジュール表示期間From</value>
            public int? ScheduleYearFrom { get; set; }
            /// <summary>Gets or sets スケジュール表示期間To</summary>
            /// <value>スケジュール表示期間To</value>
            public int? ScheduleYearTo { get; set; }
            /// <summary>Gets or sets スケジュール表示年度</summary>
            /// <value>スケジュール表示年度</value>
            public int? ScheduleStartYear { get; set; }
            /// <summary>Gets or sets 汎用_拡張項目格納用</summary>
            /// <value>汎用_拡張項目格納用</value>
            public string ExtensionData { get; set; }
        }
        /// <summary>
        /// 予算出力出力条件
        /// </summary>
        public class BudgetOutputOutputCondition
        {
            /// <summary>Gets or sets 出力期間</summary>
            /// <value>出力期間</value>
            public string OutputPeriod { get; set; }
            /// <summary>Gets or sets 出力期間From</summary>
            /// <value>出力期間From</value>
            public DateTime? OutputPeriodFrom { get; set; }
            /// <summary>Gets or sets 出力期間To</summary>
            /// <value>出力期間To</value>
            public DateTime? OutputPeriodTo { get; set; }
        }
    }
}
