using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;

namespace BusinessLogic_LN0002
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_LN0002
    {
        /// <summary>
        /// 一覧画面
        /// </summary>
        public class FormList
        {
            /// <summary>
            /// 一覧
            /// </summary>
            public class List
            {
                /// <summary>Gets or sets 一覧グループID</summary>
                /// <value>一覧グループID</value>
                /// <remarks>一覧の折り畳み単位ごとに同じ値が入る</remarks>
                public long ListGroupId { get; set; }

                // 長期計画
                /// <summary>Gets or sets 長期計画件名ID</summary>
                /// <value>長期計画件名ID</value>
                public long LongPlanId { get; set; }
                /// <summary>Gets or sets 予算性格区分</summary>
                /// <value>予算性格区分</value>
                public int? BudgetPersonalityStructureId { get; set; }

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

                // 機器別管理基準部位
                /// <summary>Gets or sets 機器別管理基準部位ID</summary>
                /// <value>機器別管理基準部位ID</value>
                public long ManagementStandardsComponentId { get; set; }
                /// <summary>Gets or sets 部位ID</summary>
                /// <value>部位ID</value>
                public int? InspectionSiteStructureId { get; set; }
                /// <summary>Gets or sets 部位重要度</summary>
                /// <value>部位重要度</value>
                public int? InspectionSiteImportanceStructureId { get; set; }
                /// <summary>Gets or sets 部位保全方式</summary>
                /// <value>部位保全方式</value>
                public int? InspectionSiteConservationStructureId { get; set; }

                // 機器別管理基準内容
                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                public long ManagementStandardsContentId { get; set; }
                /// <summary>Gets or sets 点検内容ID</summary>
                /// <value>点検内容ID</value>
                public int? InspectionContentStructureId { get; set; }

                // 保全スケジュール
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
                /// <summary>Gets or sets 開始日</summary>
                /// <value>開始日</value>
                public DateTime? StartDate { get; set; }
                /// <summary>Gets or sets スケジュール紐付け用キーID</summary>
                /// <value>スケジュール紐付け用キーID</value>
                public string KeyId { get; set; }

                #region メモリ改善対応
                /// <summary>Gets or sets 部位</summary>
                /// <value>部位</value>
                public string InspectionSiteName { get; set; }
                /// <summary>Gets or sets 部位重要度</summary>
                /// <value>部位重要度</value>
                public string ImportanceName { get; set; }
                /// <summary>Gets or sets 保全方式</summary>
                /// <value>保全方式</value>
                public string InspectionSiteConservationName { get; set; }
                /// <summary>Gets or sets 保全項目</summary>
                /// <value>保全項目</value>
                public string InspectionContentName { get; set; }
                /// <summary>Gets or sets 予算性格区分</summary>
                /// <value>予算性格区分</value>
                public string BudgetPersonalityName { get; set; }
                /// <summary>Gets or sets 周期(年)</summary>
                /// <value>周期(年)</value>
                public string CycleYearDisplay { get; set; }
                /// <summary>Gets or sets 周期(月)</summary>
                /// <value>周期(月)</value>
                public string CycleMonthDisplay { get; set; }
                /// <summary>Gets or sets 周期(日)</summary>
                /// <value>周期(日)</value>
                public string CycleDayDisplay { get; set; }
                #endregion

                /// <summary>Gets or sets ヘッダフラグ</summary>
                /// <value>ヘッダフラグ</value>
                public bool HeaderFlg { get; set; }

                /// <summary>
                /// このクラスを元にしたグループヘッダ行を作成する処理
                /// </summary>
                /// <returns>新しいグループヘッダ行</returns>
                public List MakeGroupHead()
                {
                    // ヘッダ用に新しく作成
                    List rtn = new();
                    // キーIDはグループキー
                    rtn.KeyId = this.ListGroupId.ToString();
                    rtn.ListGroupId = this.ListGroupId;
                    rtn.MachineId = this.MachineId;
                    rtn.MachineName = this.MachineName;
                    rtn.MachineNo = this.MachineNo;
                    rtn.LongPlanId = this.LongPlanId;
                    rtn.HeaderFlg = true;
                    return rtn;
                }
            }
        }

        /// <summary>
        /// スケジュール関連
        /// </summary>
        public class Schedule
        {
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

        /// <summary>
        /// 長期計画ID情報
        /// </summary>
        public class LongPlanIdInfo
        {
            // 長期計画
            /// <summary>Gets or sets 長期計画件名ID</summary>
            /// <value>長期計画件名ID</value>
            public long LongPlanId { get; set; }
        }
    }
}
