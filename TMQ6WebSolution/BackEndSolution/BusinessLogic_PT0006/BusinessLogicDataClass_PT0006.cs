using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_PT0006
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_PT0006
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }

            /// <summary>Gets or sets 新旧区分</summary>
            /// <value>新旧区分</value>
            public string OldNewStructureId { get; set; }

            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public string DepartmentStructureId { get; set; }

            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public string AccountStructureId { get; set; }

            /// <summary>Gets or sets 受払履歴IDリスト</summary>
            /// <value>受払履歴IDリスト</value>
            public string IdList { get; set; }

            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public long WorkNo { get; set; }

            /// <summary>Gets or sets 入出庫No</summary>
            /// <value>入出庫No</value>
            public long InoutNo { get; set; }

            /// <summary>Gets or sets 受払区分</summary>
            /// <value>受払区分</value>
            public long InoutDivisionStructureId { get; set; }

            /// <summary>Gets or sets ロット管理ID</summary>
            /// <value>ロット管理ID</value>
            public long LotControlId { get; set; }

            /// <summary>Gets or sets 受払履歴ID</summary>
            /// <value>受払履歴ID</value>
            public long InoutHistoryId { get; set; }

            /// <summary>Gets or sets 受払履歴日時</summary>
            /// <value>受払履歴日時</value>
            public DateTime? InoutDatetime { get; set; }

            /// <summary>Gets or sets 更新日時</summary>
            /// <value>更新日時</value>
            public DateTime? UpdateDatetime { get; set; }

            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public long PartsLocationId { get; set; }

            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string? PartsLocationDetailNo { get; set; }

            /// <summary>Gets or sets 小数点以下桁数(数量)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int UnitDigit { get; set; }

            /// <summary>Gets or sets 小数点以下桁数(金額)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int CurrencyDigit { get; set; }

            /// <summary>Gets or sets 丸め処理区分(数量)</summary>
            /// <value>丸め処理区分(数量)</value>
            public int UnitRoundDivision { get; set; }

            /// <summary>Gets or sets 丸め処理区分(金額)</summary>
            /// <value>丸め処理区分(金額)</value>
            public int CurrencyRoundDivision { get; set; }

            /// <summary>Gets or sets 制御用フラグ</summary>
            /// <value>制御用フラグ</value>
            public string ControlFlag { get; set; }
            /// <summary>Gets or sets 数量管理単位ID</summary>
            /// <value>数量管理単位ID</value>
            public long UnitStructureId { get; set; }

            /// <summary>Gets or sets 出庫数</summary>
            /// <value>出庫数</value>
            public long NumberShipments { get; set; }

        }

        /// <summary>
        /// 予備品情報
        /// </summary>
        public class searchResultSpares : ComDao.CommonTableItem
        {
            // SQLの検索結果の列を定義してください。
            // 品目マスタから多くの列を取得する場合は、品目マスタのデータクラスを継承することで、それらの定義を省くことができます。

            /// <summary>Gets or sets 品目名称(テスト)</summary>
            /// <value>品目名称(テスト)</value>
            public string ItemNameTest { get; set; }

            /// <summary>Gets or sets 予備品No</summary>
            /// <value>予備品No</value>
            public string PartsNo { get; set; }

            /// <summary>Gets or sets 予備品名称</summary>
            /// <value>予備品名称</value>
            public string PartsName { get; set; }

            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string ManufacturerName { get; set; }

            /// <summary>Gets or sets 型式</summary>
            /// <value>型式</value>
            public string ModelType { get; set; }

            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string Dimensions { get; set; }

            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public decimal Inventry { get; set; }

            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public string StockQuantity { get; set; }

            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public string StockQuantityDisplay { get; set; }

            /// <summary>Gets or sets 単位</summary>
            /// <value>単位</value>
            public string Unit { get; set; }

            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(数量)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int UnitDigit { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(金額)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int CurrencyDigit { get; set; }
            /// <summary>Gets or sets 丸め処理区分(数量)</summary>
            /// <value>丸め処理区分(数量)</value>
            public int UnitRoundDivision { get; set; }
            /// <summary>Gets or sets 丸め処理区分(金額)</summary>
            /// <value>丸め処理区分(金額)</value>
            public int CurrencyRoundDivision { get; set; }
            /// <summary>Gets or sets 表示年度(From)</summary>
            /// <value>表示年度(From)</value>
            public string DispYearFrom { get; set; }
            /// <summary>Gets or sets 表示年度(To)</summary>
            /// <value>表示年度(To)</value>
            public string DispYearTo { get; set; }
        }

        /// <summary>
        /// 部門在庫情報
        /// </summary>
        public class searchResultDepartmentInfo : searchResultSpares
        {
            /// <summary>Gets or sets 新旧区分</summary>
            /// <value>新旧区分</value>
            public string OldNewStructureId { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCd { get; set; }
            /// <summary>Gets or sets 部門名</summary>
            /// <value>部門名</value>
            public string DepartmentNm { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string SubjectCd { get; set; }
            /// <summary>Gets or sets 勘定科目名</summary>
            /// <value>勘定科目名</value>
            public string SubjectNm { get; set; }
            /// <summary>Gets or sets 新旧区分(在庫一覧検索用)</summary>
            /// <value>新旧区分(在庫一覧検索用)</value>
            public long OldNewNm { get; set; }
            /// <summary>Gets or sets 部門(在庫一覧検索用)</summary>
            /// <value>部門(在庫一覧検索用)</value>
            public long ToDepartmentNm { get; set; }
            /// <summary>Gets or sets 勘定科目(在庫一覧検索用)</summary>
            /// <value>勘定科目(在庫一覧検索用)</value>
            public long ToSubjectNm { get; set; }
            /// <summary>Gets or sets 棚番ID</summary>
            /// <value>棚番ID</value>
            public long PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚番</summary>
            /// <value>棚番</value>
            public string PartsLocationName { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 数量管理単位ID</summary>
            /// <value>数量管理単位ID</value>
            public long UnitStructureId { get; set; }
        }

        /// <summary>
        /// 出庫情報入力
        /// </summary>
        public class registIssue : searchResultInventory
        {
            /// <summary>Gets or sets 出庫日</summary>
            /// <value>出庫日</value>
            public DateTime? InoutDatetime { get; set; }

            /// <summary>Gets or sets 出庫数(受払数)</summary>
            /// <value>出庫数(受払数)</value>
            public decimal? NumberShipments { get; set; }

            /// <summary>Gets or sets 出庫区分</summary>
            /// <value>出庫区分</value>
            public int ShippingDivisionStructureId { get; set; }

            /// <summary>Gets or sets 単位</summary>
            /// <value>単位</value>
            public string UnitTranslationText { get; set; }

            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public string PartsId { get; set; }

            /// <summary>Gets or sets 画面タイプ</summary>
            /// <value>画面タイプ</value>
            public int FormType { get; set; }

            /// <summary>Gets or sets 小数点以下桁数</summary>
            /// <value>小数点以下桁数</value>
            public int Digit { get; set; }

            /// <summary>Gets or sets 丸め処理区分</summary>
            /// <value>丸め処理区分</value>
            public int RoundDivision { get; set; }
        }

        /// <summary>
        /// 在庫一覧
        /// </summary>
        public class searchResultInventory : searchResultDepartmentInfo
        {
            /// <summary>Gets or sets ロットNo</summary>
            /// <value>ロットNo</value>
            public long lotNo { get; set; }

            /// <summary>Gets or sets 入庫日</summary>
            /// <value>入庫日</value>
            public string ReceivingDatetime { get; set; }

            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public long PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚</summary>
            /// <value>棚</value>
            public string PartsLocationCd { get; set; }
            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public string PartsLocationDisplay { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNo { get; set; }

            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public string UnitPrice { get; set; }

            /// <summary>Gets or sets 金額単位名称</summary>
            /// <value>金額単位名称</value>
            public string CurrencyName { get; set; }

            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public string Unit { get; set; }

            /// <summary>Gets or sets 数量単位名称</summary>
            /// <value>数量単位名称</value>
            public string UnitName { get; set; }

            /// <summary>Gets or sets 出庫数(受払数)</summary>
            /// <value>出庫数(受払数)</value>
            public string IssueQuantity { get; set; }

            /// <summary>Gets or sets 管理No</summary>
            /// <value>管理No</value>
            public string ManagementNo { get; set; }

            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }

            /// <summary>Gets or sets ロット管理ID</summary>
            /// <value>ロット管理ID</value>
            public string LotControlId { get; set; }

            /// <summary>Gets or sets 在庫管理ID</summary>
            /// <value>在庫管理ID</value>
            public string InventoryControlId { get; set; }

            /// <summary>Gets or sets 受払区分</summary>
            /// <value>受払区分</value>
            public int InoutDivisionStructureId { get; set; }

            /// <summary>Gets or sets 作業区分</summary>
            /// <value>作業区分</value>
            public int WorkDivisionStructureId { get; set; }

            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public long WorkNo { get; set; }

            /// <summary>Gets or sets 棚卸確定日時</summary>
            /// <value>棚卸確定日時</value>
            public DateTime InventoryDatetime { get; set; }

            /// <summary>Gets or sets 更新シリアルID</summary>
            /// <value>更新シリアルID</value>
            public int UpdateSerialid { get; set; }

            /// <summary>Gets or sets 件数</summary>
            /// <value>件数</value>
            public int Count { get; set; }

            /// <summary>
            /// 数量・金額の単位結合、丸め処理
            /// </summary>
            public void JoinStrAndRound()
            {
                this.DepartmentNm = TMQUtil.CombineNumberAndUnit(this.DepartmentCd, this.DepartmentNm, true); // 部門
                this.SubjectNm = TMQUtil.CombineNumberAndUnit(this.SubjectCd, this.SubjectNm, true);          // 勘定科目

                this.UnitPrice = TMQUtil.roundDigit(this.UnitPrice, this.CurrencyDigit, this.CurrencyRoundDivision);   // 入庫単価
                this.Unit = TMQUtil.roundDigit(this.Inventry.ToString(), this.UnitDigit, this.UnitRoundDivision);      // 在庫数
                this.IssueQuantity = TMQUtil.roundDigit(this.IssueQuantity, this.UnitDigit, this.UnitRoundDivision);   // 出庫数

                this.UnitPrice = TMQUtil.CombineNumberAndUnit(this.UnitPrice, this.CurrencyName, false);               // 入庫単価 + 単位
                this.StockQuantityDisplay = TMQUtil.CombineNumberAndUnit(this.Unit, this.UnitName, false);             // 在庫数 + 単位
                this.IssueQuantity = TMQUtil.CombineNumberAndUnit(this.IssueQuantity, this.UnitName, false);           // 出庫数 + 単位
            }
        }

        /// <summary>
        /// 丸め処理区分取得用
        /// </summary>
        public class RoundDigit
        {
            /// <summary>Gets or sets 丸め処理区分</summary>
            /// <value>丸め処理区分</value>
            public int UnitRoundDivision { get; set; }
        }
    }
}
