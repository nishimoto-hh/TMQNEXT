using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>
    /// 部品展開処理で使用するデータクラス
    /// </summary>
    public class ProPartsStructureDataClass
    {

        #region 固定値
        #endregion

        #region DataClass
        /// <summary>
        /// 画面条件情報
        /// </summary>
        public class FormCodition
        {
            /// <summary>Gets or sets 展開日From</summary>
            /// <value>展開日From</value>
            public string DeploymentDateFrom { get; set; }
            /// <summary>Gets or sets 展開日To</summary>
            /// <value>展開日To</value>
            public string DeploymentDateTo { get; set; }
            /// <summary>Gets or sets リードタイム区分</summary>
            /// <value>リードタイム区分(0:固定 2:日当数)</value>
            public int LeadTimeType { get; set; }
            /// <summary>Gets or sets 安全在庫</summary>
            /// <value>安全在庫</value>
            public int SafteyStockType { get; set; }
        }

        /// <summary>
        /// 品目在庫情報
        /// </summary>
        public class TempItemInventory
        {
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public decimal InventoryQty { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="paramItemCd">品目コード</param>
            /// <param name="paramSpecificationCd">仕様コード</param>
            /// <param name="paramQty">在庫数</param>
            public TempItemInventory(string paramItemCd, string paramSpecificationCd, decimal paramQty)
            {
                ItemCd = paramItemCd;
                SpecificationCd = paramSpecificationCd;
                InventoryQty = paramQty;
            }
        }

        /// <summary>
        /// バックオーダ情報
        /// </summary>
        public class TempBackOrder
        {

            /// <summary>Gets or sets 受払区分</summary>
            /// <value>受払区分</value>
            public string InoutDivision { get; set; }
            /// <summary>Gets or sets 受払予定日</summary>
            /// <value>受払予定日</value>
            public DateTime? InoutDate { get; set; }
            /// <summary>Gets or sets オーダー区分</summary>
            /// <value>オーダー区分</value>
            public int OrderDivision { get; set; }
            /// <summary>Gets or sets オーダー番号</summary>
            /// <value>オーダー番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets 受払数</summary>
            /// <value>受払数</value>
            public decimal InoutQty { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }
            /// <summary>Gets or sets オーダー先コード</summary>
            /// <value>オーダー先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 親品目コード</summary>
            /// <value>親品目コード</value>
            public string ParentItemCd { get; set; }
            /// <summary>Gets or sets 親仕様コード</summary>
            /// <value>親仕様コード</value>
            public string ParentSpecificationCd { get; set; }
            /// <summary>Gets or sets システム日付</summary>
            /// <value>システム日付</value>
            public DateTime SystemDate { get; set; }

        }

        /// <summary>
        /// 品目情報
        /// </summary>
        public class PartsItemInfo : APCommonUtil.APCommonDataClass.ProductionPlanEntity
        {
            /// <summary>Gets or sets 品目仕様有効開始日</summary>
            /// <value>品目仕様有効開始日</value>
            public DateTime? SpecificationActiveDate { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 基準保管場所</summary>
            /// <value>基準保管場所</value>
            public string DefaultLocation { get; set; }
            /// <summary>Gets or sets 計画区分</summary>
            /// <value>計画区分</value>
            public int PlanDivision { get; set; }
            /// <summary>Gets or sets 運用管理区分</summary>
            /// <value>運用管理区分</value>
            public string UnitOfOperationManagement { get; set; }
            /// <summary>Gets or sets 購入リードタイム</summary>
            /// <value>購入リードタイム</value>
            public int? PurchaseLeadTime { get; set; }
            /// <summary>Gets or sets 購入安全リードタイム</summary>
            /// <value>購入安全リードタイム</value>
            public int? PurchaseSafetyLeadTime { get; set; }
            /// <summary>Gets or sets 発注基準点</summary>
            /// <value>発注基準点</value>
            public int PurchaseTrigger { get; set; }
            /// <summary>Gets or sets 標準日別生産量</summary>
            /// <value>標準日別生産量</value>
            public decimal? StandardRatePerday { get; set; }
            /// <summary>Gets or sets 製造リードタイム</summary>
            /// <value>製造リードタイム</value>
            public int? ProductionLeadTime { get; set; }
            /// <summary>Gets or sets 製造安全リードタイム</summary>
            /// <value>製造安全リードタイム</value>
            public int? ProductSafetyLeadTime { get; set; }
            /// <summary>Gets or sets 単位生産量</summary>
            /// <value>単位生産量</value>
            public decimal? UnitQty { get; set; }
            /// <summary>Gets or sets 最小生産量</summary>
            /// <value>最小生産量</value>
            public decimal? MinQty { get; set; }
            /// <summary>Gets or sets 生産計画区分</summary>
            /// <value>生産計画区分</value>
            public int ProductionPlan { get; set; }
            /// <summary>Gets or sets システム日付</summary>
            /// <value>システム日付</value>
            public DateTime SysDate { get; set; }
            /// <summary>Gets or sets 在庫管理区分</summary>
            /// <value>在庫管理区分</value>
            public int? StockDivision { get; set; }
        }

        /// <summary>
        /// 発注点情報
        /// </summary>
        public class OrderPoint : APCommonUtil.APCommonDataClass.ItemPurchaseAttributeEntity
        {
            /// <summary>Gets or sets 有効在庫数</summary>
            /// <value>有効在庫数</value>
            public decimal AvailableQty { get; set; }
            /// <summary>Gets or sets 基準保管場所</summary>
            /// <value>基準保管場所</value>
            public string DefaultLocation { get; set; }
            /// <summary>Gets or sets 丸め日数</summary>
            /// <value>丸め日数</value>
            public decimal RoundOffDays { get; set; }
        }

        /// <summary>
        /// 部品展開用カレンダー情報
        /// </summary>
        public class CalParts
        {
            /// <summary>Gets or sets カレンダー日</summary>
            /// <value>カレンダー日</value>
            public DateTime CalDate { get; set; }
        }

        /// <summary>
        /// 品目仕様：生産計画生産順
        /// </summary>
        public class MaxLevelInfo
        {
            /// <summary>Gets or sets 最大生産計画生産順</summary>
            /// <value>最大生産計画生産順</value>
            public int MaxLevel { get; set; }
        }

        /// <summary>
        /// 子品目展開時：対象品目情報
        /// </summary>
        public class TargetItem
        {
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 生産計画生産順</summary>
            /// <value>生産計画生産順</value>
            public int MaxLLC { get; set; }
        }

        /// <summary>
        /// 子品目展開時：対象品目毎のMRP結果情報
        /// </summary>
        public class TargetMRPResult
        {
            /// <summary>Gets or sets 計画番号</summary>
            /// <value>計画番号</value>
            public string PlanNo { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 納期</summary>
            /// <value>納期</value>
            public DateTime DeliveryDate { get; set; }
            /// <summary>Gets or sets 計画数量</summary>
            /// <value>計画数量</value>
            public decimal PlanQty { get; set; }
            /// <summary>Gets or sets オーダ発行区分</summary>
            /// <value>オーダ発行区分</value>
            public string OrderPublishDivision { get; set; }
            /// <summary>Gets or sets 生産計画番号</summary>
            /// <value>生産計画番号</value>
            public string ProductionPlanNo { get; set; }
            /// <summary>Gets or sets 計画納期</summary>
            /// <value>計画納期</value>
            public DateTime Deliverlimit { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 購入依頼日</summary>
            /// <value>購入依頼日</value>
            public DateTime OrderDate { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }
            /// <summary>Gets or sets 引当数</summary>
            /// <value>引当数</value>
            public decimal AllocatedQty { get; set; }
            /// <summary>Gets or sets 当初納期</summary>
            /// <value>当初納期</value>
            public DateTime DeliveryDateBefore { get; set; }
            /// <summary>Gets or sets 開始有効日(品目仕様)</summary>
            /// <value>開始有効日(品目仕様)</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 在庫管理単位</summary>
            /// <value>在庫管理単位</value>
            public string UnitOfStockCtrl { get; set; }
            /// <summary>Gets or sets 運用管理単位</summary>
            /// <value>運用管理単位</value>
            public string UnitOfOperationManagement { get; set; }
            /// <summary>Gets or sets 丸め日数</summary>
            /// <value>丸め日数</value>
            public int? RoundOffDays { get; set; }
            /// <summary>Gets or sets 換算係数</summary>
            /// <value>換算係数</value>
            public decimal KgOfFractionManagement { get; set; }
            /// <summary>Gets or sets 基準保管場所</summary>
            /// <value>基準保管場所</value>
            public string DefaultLocation { get; set; }
            /// <summary>Gets or sets 計画区分</summary>
            /// <value>計画区分</value>
            public int PlanDivision { get; set; }
            /// <summary>Gets or sets 発注基準</summary>
            /// <value>発注基準</value>
            public int PurchaseTrigger { get; set; }
            /// <summary>Gets or sets 発注単位</summary>
            /// <value>発注単位</value>
            public decimal PurchaseOrderUnitQty { get; set; }
            /// <summary>Gets or sets 最小発注数</summary>
            /// <value>最小発注数</value>
            public decimal PurchaseOrderMinQty { get; set; }
            /// <summary>Gets or sets 最大発注数</summary>
            /// <value>最大発注数</value>
            public decimal PurchaseOrderMaxQty { get; set; }
            /// <summary>Gets or sets 購買リードタイム</summary>
            /// <value>購買リードタイム</value>
            public int? PurchaseLeadTime { get; set; }
            /// <summary>Gets or sets 購買安全リードタイム</summary>
            /// <value>購買安全リードタイム</value>
            public int? PurchaseSafetyLeadTime { get; set; }
            /// <summary>Gets or sets 基準仕入れ先コード</summary>
            /// <value>基準仕入れ先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 生産計画区分</summary>
            /// <value>生産計画区分</value>
            public int ProductionPlan { get; set; }
            /// <summary>Gets or sets 安全在庫</summary>
            /// <value>安全在庫</value>
            public decimal ProductOrderPoint { get; set; }
            /// <summary>Gets or sets 標準日別生産量</summary>
            /// <value>標準日別生産量</value>
            public decimal? StandardRatePerday { get; set; }
            /// <summary>Gets or sets 製造リードタイム</summary>
            /// <value>製造リードタイム</value>
            public int? ProductionLeadTime { get; set; }
            /// <summary>Gets or sets 製造安全リードタイム</summary>
            /// <value>製造安全リードタイム</value>
            public int? ProductSafetyLeadTime { get; set; }
            /// <summary>Gets or sets 標準生産量(発注単位)</summary>
            /// <value>標準生産量(発注単位)</value>
            public decimal StdQty { get; set; }
            /// <summary>Gets or sets 最小生産量</summary>
            /// <value>最小生産量</value>
            public decimal MinQty { get; set; }
            /// <summary>Gets or sets 最大生産量</summary>
            /// <value>最大生産量</value>
            public decimal MaxQty { get; set; }
            /// <summary>Gets or sets システム日付</summary>
            /// <value>システム日付</value>
            public DateTime SysDate { get; set; }
            /// <summary>Gets or sets 在庫管理区分</summary>
            /// <value>在庫管理区分</value>
            public int? StockDivision { get; set; }

        }

        /// <summary>
        /// LeadTime情報
        /// </summary>
        public class LeadTimeInfo
        {
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 計画区分</summary>
            /// <value>計画区分</value>
            public int PlanDivision { get; set; }
            /// <summary>Gets or sets 購買リードタイム</summary>
            /// <value>購買リードタイム</value>
            public int PurchaseLeadTime { get; set; }
            /// <summary>Gets or sets 購買安全リードタイム</summary>
            /// <value>購買安全リードタイム</value>
            public int PurchaseSafetyLeadTime { get; set; }
            /// <summary>Gets or sets 製造リードタイム</summary>
            /// <value>製造リードタイム</value>
            public int ProductionLeadTime { get; set; }
            /// <summary>Gets or sets 製造安全リードタイム</summary>
            /// <value>製造安全リードタイム</value>
            public int ProductSafetyLeadTime{ get; set; }

        }

        #endregion

    }
}
