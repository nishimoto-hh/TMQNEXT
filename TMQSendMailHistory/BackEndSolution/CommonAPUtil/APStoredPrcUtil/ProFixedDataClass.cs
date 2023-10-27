using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>
    /// 確定処理データクラス
    /// </summary>
    public class ProFixedDataClass
    {
        /// <summary>
        /// 製造オーダー確定
        /// </summary>
        public class FixProductionOrder
        {
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 納期</summary>
            /// <value>納期</value>
            public DateTime? DeliveryDate { get; set; }
            /// <summary>Gets or sets 発注計画番号==部品展開処理などで自動採番</summary>
            /// <value>発注計画番号==部品展開処理などで自動採番</value>
            public string PlanNo { get; set; }
            /// <summary>Gets or sets 手続区分==0:初期値 1:製造 2:購買 (計画データは 1 or 2)</summary>
            /// <value>手続区分==0:初期値 1:製造 2:購買 (計画データは 1 or 2)</value>
            public int? ProcedureDivision { get; set; }
            /// <summary>Gets or sets オーダー発行区分==1:入庫予定 3:生産計画 4:独立需要 5:計画･引当 6:計画･出庫予定 10:出庫予定 12:引当 13:発注予測</summary>
            /// <value>オーダー発行区分==1:入庫予定 3:生産計画 4:独立需要 5:計画･引当 6:計画･出庫予定 10:出庫予定 12:引当 13:発注予測</value>
            public string OrderPublishDivision { get; set; }
            /// <summary>Gets or sets 計画発注日</summary>
            /// <value>計画発注日</value>
            public DateTime? OrderDate { get; set; }
            /// <summary>Gets or sets 発注基準==0:発注点 1:M発注点 2:ダブルビン（未使用） 3:定期 4:確定引取（未使用） 5:かんばん（未使用） 6:個別（未使用） 7:コック（未使用）</summary>
            /// <value>発注基準==0:発注点 1:M発注点 2:ダブルビン（未使用） 3:定期 4:確定引取（未使用） 5:かんばん（未使用） 6:個別（未使用） 7:コック（未使用）</value>
            public int? OrderRule { get; set; }
            /// <summary>Gets or sets 計画納期</summary>
            /// <value>計画納期</value>
            public DateTime? Deliverlimit { get; set; }
            /// <summary>Gets or sets 計画数量</summary>
            /// <value>計画数量</value>
            public decimal? PlanQty { get; set; }
            /// <summary>Gets or sets 生産計画番号==生産計画ファイルを元に作成されたデータのみ</summary>
            /// <value>生産計画番号==生産計画ファイルを元に作成されたデータのみ</value>
            public string ProductionPlanNo { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }
            /// <summary>Gets or sets 納入ロケーション区分==0:ロケーション 1:仕入先 2:作業区</summary>
            /// <value>納入ロケーション区分==0:ロケーション 1:仕入先 2:作業区</value>
            public decimal? LocationDivisionCd { get; set; }
            /// <summary>Gets or sets 納入ロケーション</summary>
            /// <value>納入ロケーション</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets オーダー先コード==手続区分=製造：作業区コード 購買：仕入先コード</summary>
            /// <value>オーダー先コード==手続区分=製造：作業区コード 購買：仕入先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 品目.品目コード</summary>
            /// <value>品目.品目コード</value>
            public string ImCode { get; set; }
            /// <summary>Gets or sets 品目.品目名称</summary>
            /// <value>品目.品目名称</value>
            public string ImHinnm { get; set; }
            /// <summary>Gets or sets 品目在庫.品目コード</summary>
            /// <value>品目在庫.品目コード</value>
            public string IzCode { get; set; }
            /// <summary>Gets or sets 取引先.取引先コード</summary>
            /// <value>取引先.取引先コード</value>
            public string TsCode { get; set; }
            /// <summary>Gets or sets 取引先.担当</summary>
            /// <value>取引先.担当</value>
            public string TsTanto { get; set; }
            /// <summary>Gets or sets 取引先.担当コード</summary>
            /// <value>取引先.担当コード</value>
            public string TaCode { get; set; }
            /// <summary>Gets or sets 取引先.部門コード</summary>
            /// <value>取引先.部門コード</value>
            public string TaBumon { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime? ActiveDate { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 仕入先名称</summary>
            /// <value>仕入先名称</value>
            public string VenderName { get; set; }
            /// <summary>Gets or sets 基準保管場所</summary>
            /// <value>基準保管場所</value>
            public string DefaultLocation { get; set; }
            /// <summary>Gets or sets 基準保管場所名称</summary>
            /// <value>基準保管場所名称</value>
            public string DefaultLocationName { get; set; }
            /// <summary>Gets or sets 品目タイプ</summary>
            /// <value>品目タイプ</value>
            public int? ItemType { get; set; }
        }

        /// <summary>
        /// 親部品情報
        /// </summary>
        public class TrnOwn
        {
            /// <summary>Gets or sets 手続区分</summary>
            /// <value>手続区分</value>
            public int? ProcedureDivision { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 計画納期</summary>
            /// <value>計画納期</value>
            public DateTime? DeliverLimit { get; set; }
            /// <summary>Gets or sets 計画発注日</summary>
            /// <value>計画発注日</value>
            public DateTime? OrderDate { get; set; }
            /// <summary>Gets or sets 計画数量</summary>
            /// <value>計画数量</value>
            public decimal? PlanQty { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }
            /// <summary>Gets or sets オーダー番号</summary>
            /// <value>オーダー番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets オーダー先コード</summary>
            /// <value>オーダー先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 丸めフラグ</summary>
            /// <value>丸めフラグ</value>
            public bool? MarumeFlg { get; set; }
        }

        /// <summary>
        /// 子部品情報
        /// </summary>
        public class TrnKo
        {
            /// <summary>Gets or sets 子品目</summary>
            /// <value>子品目</value>
            public string ChildItemCd { get; set; }

            /// <summary>Gets or sets 基盤ID</summary>
            /// <value>基盤ID</value>
            public string PartsCd { get; set; }

            /// <summary>Gets or sets 出庫区分</summary>
            /// <value>出庫区分</value>
            public int? OutPrepatationDivision { get; set; }

            /// <summary>Gets or sets 出庫ID</summary>
            /// <value>出庫ID</value>
            public string OutId { get; set; }

            /// <summary>Gets or sets 有効日</summary>
            /// <value>有効日</value>
            public DateTime? DateOfValidity { get; set; }

            /// <summary>Gets or sets 失効日</summary>
            /// <value>失効日</value>
            public DateTime? DateOfInvalidity { get; set; }

            /// <summary>Gets or sets 使用数区分</summary>
            /// <value>使用数区分</value>
            public int? UseDivision { get; set; }

            /// <summary>Gets or sets 使用数１</summary>
            /// <value>使用数１</value>
            public decimal? UseQty1 { get; set; }

            /// <summary>Gets or sets 使用数２</summary>
            /// <value>使用数２</value>
            public decimal? UseQty2 { get; set; }

            /// <summary>Gets or sets 支給区分</summary>
            /// <value>支給区分</value>
            public int? SupplyDivision { get; set; }

            /// <summary>Gets or sets 支給単価</summary>
            /// <value>支給単価</value>
            public decimal? SupplyUnitprice { get; set; }

            /// <summary>Gets or sets 在庫管理単位</summary>
            /// <value>在庫管理単位</value>
            public int? StockDivision { get; set; }

            /// <summary>Gets or sets 基準ロケーション区分</summary>
            /// <value>基準ロケーション区分</value>
            public int? DefaultLocationDivision { get; set; }

            /// <summary>Gets or sets 基準ロケーション</summary>
            /// <value>基準ロケーション</value>
            public string DefaultLocation { get; set; }

            /// <summary>Gets or sets 仕様変更フラグ</summary>
            /// <value>仕様変更フラグ</value>
            public int? ChangeSpecFlg { get; set; }

            /// <summary>Gets or sets 仕様変更区分</summary>
            /// <value>仕様変更区分</value>
            public int? ChangeSpecDivision { get; set; }

            /// <summary>Gets or sets 仕様変更番号</summary>
            /// <value>仕様変更番号</value>
            public string ChangeSpecNo { get; set; }

            /// <summary>Gets or sets 変更後子品目</summary>
            /// <value>変更後子品目</value>
            public string AfterChildItemCd { get; set; }

            /// <summary>Gets or sets 変更後基盤ID</summary>
            /// <value>変更後基盤ID</value>
            public string AfterPartsCd { get; set; }

            /// <summary>Gets or sets 変更後出庫区分</summary>
            /// <value>変更後出庫区分</value>
            public int? AfterOutPrepatationDivision { get; set; }

            /// <summary>Gets or sets 変更後出庫ID</summary>
            /// <value>変更後出庫ID</value>
            public string AfterOutId { get; set; }

            /// <summary>Gets or sets 変更後使用区分</summary>
            /// <value>変更後使用区分</value>
            public int? AfterUseDivision { get; set; }

            /// <summary>Gets or sets 変更後使用数1</summary>
            /// <value>変更後使用数1</value>
            public decimal? AfterUseQty1 { get; set; }

            /// <summary>Gets or sets 変更後使用数2</summary>
            /// <value>変更後使用数2</value>
            public decimal? AfterUseQty2 { get; set; }

            /// <summary>Gets or sets 変更後支給区分</summary>
            /// <value>変更後支給区分</value>
            public int? AfterSupplyDivision { get; set; }

            /// <summary>Gets or sets 変更後支給単価</summary>
            /// <value>変更後支給単価</value>
            public decimal? AfterSupplyUnitprice { get; set; }

            /// <summary>Gets or sets 変更後在庫管理単位</summary>
            /// <value>変更後在庫管理単位</value>
            public int? AfterStockDivision { get; set; }

            /// <summary>Gets or sets 変更後ロケーション区分</summary>
            /// <value>変更後ロケーション区分</value>
            public int? AfterLocationDivision { get; set; }

            /// <summary>Gets or sets 変更後ロケーションID</summary>
            /// <value>変更後ロケーションID</value>
            public string AfterLocationcd { get; set; }

            /// <summary>Gets or sets 仕様変更適要日</summary>
            /// <value>仕様変更適要日</value>
            public DateTime? ApplyDate { get; set; }

            /// <summary>Gets or sets 変更後使用区分</summary>
            /// <value>変更後使用区分</value>
            public int? ObsoleteDivision { get; set; }

        }

        /// <summary>
        /// BOM検索条件
        /// </summary>
        public class SearchBomCondition
        {
            /// <summary>Gets or sets 品目</summary>
            /// <value>品目</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様</summary>
            /// <value>仕様</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime ValidDate { get; set; }
        }

        /// <summary>
        /// BOM検索結果格納クラス
        /// </summary>
        public class SearchBom
        {
            /// <summary>Gets or sets レシピコード</summary>
            /// <value>レシピコード</value>
            public string RecipeCd { get; set; }
            /// <summary>Gets or sets 生産ライン</summary>
            /// <value>生産ライン</value>
            public string ProductionLine { get; set; }
        }

    }
}
