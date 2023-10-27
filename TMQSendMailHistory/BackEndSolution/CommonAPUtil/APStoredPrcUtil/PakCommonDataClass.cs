using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace CommonAPUtil.APStoredPrcUtil
{

    /// <summary>
    /// PakCommonで使用するデータクラス
    /// </summary>
    public class PakCommonDataClass
    {
        #region 固定値

        #endregion

        /// <summary>
        /// 【共通項目】パラメータ
        /// </summary>
        public class ComParamInfo
        {
            /// <summary>Gets or sets プログラムID(Assemblyname)</summary>
            /// <value>プログラムID(Assemblyname)</value>
            public string PgmId { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
            /// <summary>Gets or sets IPアドレス </summary>
            /// /// <value>IPアドレス</value>
            public string TerminalNo { get; set; }
            /// <summary>Gets or sets ユーザーID </summary>
            /// /// <value>ユーザーID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 機能ID</summary>
            /// /// <value>機能ID</value>
            public string ConductId { get; set; }
            /// <summary>Gets or sets 画面№ </summary>
            /// /// <value>画面№</value>
            public string FormNo { get; set; }
            /// <summary>Gets or sets 実行要求日付</summary>
            /// /// <value>実行要求日付</value>
            public string ExecDate { get; set; }
            /// <summary>Gets or sets 実行要求時刻 </summary>
            /// /// <value>実行要求時刻</value>
            public string ExecTime { get; set; }

        }

        /// <summary>
        /// １レベル展開情報
        /// </summary>
        public class TempPartsStructure
        {
            /// <summary>Gets or sets 子品目コード</summary>
            /// <value>子品目コード</value>
            public string ChildItemCd { get; set; }
            /// <summary>Gets or sets 子仕様コード</summary>
            /// <value>子仕様コード</value>
            public string ChilSpecificationCd { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime StartDate { get; set; }
            /// <summary>Gets or sets 有効終了日</summary>
            /// <value>有効終了日</value>
            public DateTime EndDate { get; set; }
            /// <summary>Gets or sets 使用量</summary>
            /// <value>使用量</value>
            public decimal UseQty { get; set; }
            /// <summary>Gets or sets 在庫管理単位</summary>
            /// <value>在庫管理単位</value>
            public string StockDivision { get; set; }
            /// <summary>Gets or sets 基準保管場所</summary>
            /// <value>基準保管場所</value>
            public string DefaultLocation { get; set; }
            /// <summary>Gets or sets リードタイム</summary>
            /// <value>リードタイム</value>
            public int LeadTime { get; set; }
        }

        /// <summary>
        /// 部品構成情報取得結果
        /// </summary>
        public class PartsStructureOrder
        {

            /// <summary>Gets or sets レシピID</summary>
            /// <value>レシピID</value>
            public long RecipeId { get; set; }
            /// <summary>Gets or sets 親品目コード</summary>
            /// <value>親品目コード</value>
            public string ParentItemCd { get; set; }
            /// <summary>Gets or sets 親仕様コード</summary>
            /// <value>親仕様コード</value>
            public string ParentSpecificationCd { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime StartDate { get; set; }
            /// <summary>Gets or sets 有効終了日</summary>
            /// <value>有効終了日</value>
            public DateTime? EndDate { get; set; }
            /// <summary>Gets or sets 仕上品標準生産量</summary>
            /// <value>仕上品標準生産量</value>
            public decimal StdQty { get; set; }
            /// <summary>Gets or sets 工程区分</summary>
            /// <value>工程区分</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets 実績区分</summary>
            /// <value>実績区分</value>
            public int ResultDivision { get; set; }
            /// <summary>Gets or sets シーケンスNO</summary>
            /// <value>シーケンスNO</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets 子品目コード</summary>
            /// <value>子品目コード</value>
            public string ChildItemCd { get; set; }
            /// <summary>Gets or sets 子仕様コード</summary>
            /// <value>子仕様コード</value>
            public string ChildSpecificationCd { get; set; }
            /// <summary>Gets or sets 使用量</summary>
            /// <value>使用量</value>
            public decimal Qty { get; set; }
            /// <summary>Gets or sets 品目仕様：品目コード</summary>
            /// <value>品目仕様：品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目仕様：品目仕様コード</summary>
            /// <value>品目仕様：品目仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 品目仕様：換算係数</summary>
            /// <value>品目仕様：換算係数</value>
            public decimal KgOfFractionManagement { get; set; }
            /// <summary>Gets or sets 品目仕様：在庫管理単位</summary>
            /// <value>品目仕様：在庫管理単位</value>
            public string UnitOfStockCtrl { get; set; }
            /// <summary>Gets or sets 品目仕様：運用管理単位</summary>
            /// <value>品目仕様：運用管理単位</value>
            public string UnitOfOperationManagement { get; set; }
            /// <summary>Gets or sets 品目仕様：基準保管場所</summary>
            /// <value>品目仕様：基準保管場所</value>
            public string DefaultLocation { get; set; }
            /// <summary>Gets or sets 品目仕様：計画区分（旧：原価区分）</summary>
            /// <value>品目仕様：計画区分（旧：原価区分）</value>
            public int PlanDivision { get; set; }
            /// <summary>Gets or sets 品目仕様：在庫管理単位</summary>
            /// <value>品目仕様：在庫管理単位</value>
            public int StockDivision { get; set; }
            /// <summary>Gets or sets 品目仕様：品目コード</summary>
            /// <value>品目在庫：品目コード</value>
            public string InventoryItemCd { get; set; }
            /// <summary>Gets or sets 品目仕様：品目仕様コード</summary>
            /// <value>品目在庫：品目仕様コード</value>
            public string InventorySpecificationCd { get; set; }

        }

    }
}
