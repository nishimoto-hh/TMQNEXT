using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_PT0005
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_PT0005
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets 受払履歴ID</summary>
            /// <value>受払履歴ID</value>
            public long InoutHistoryId { get; set; }
            /// <summary>Gets or sets 制御用フラグ</summary>
            /// <value>制御用フラグ</value>
            public string ControlFlag { get; set; }
            /// <summary>Gets or sets ロット管理ID</summary>
            /// <value>ロット管理ID</value>
            public long LotControlId { get; set; }
            /// <summary>Gets or sets 新旧区分</summary>
            /// <value>新旧区分</value>
            public long OldNewStructureId { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public string AccountStructureId { get; set; }
            /// <summary>Gets or sets 受払履歴日時</summary>
            /// <value>受払履歴日時</value>
            public DateTime InoutDatetime { get; set; }
            /// <summary>Gets or sets 更新日時</summary>
            /// <value>更新日時</value>
            public DateTime UpdateDatetime { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 棚番ID</summary>
            /// <value>棚番ID</value>
            public long PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 工場IDリスト</summary>
            /// <value>工場IDリスト</value>
            public string FactoryIdList { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス(予備品情報)
        /// </summary>
        public class searchResultSpareInfo : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 標準棚</summary>
            /// <value>標準棚</value>
            public long PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 標準棚(表示用)</summary>
            /// <value>標準棚(表示用)</value>
            public string PartsLocationDisp { get; set; }
            /// <summary>Gets or sets 予備品No.</summary>
            /// <value>予備品No.</value>
            public string PartsNo { get; set; }
            /// <summary>Gets or sets 予備品名</summary>
            /// <value>予備品名</value>
            public string PartsName { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public long? ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets 型式</summary>
            /// <value>型式</value>
            public string ModelType { get; set; }
            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string Dimensions { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public decimal StockQuantity { get; set; }
            /// <summary>Gets or sets 在庫数(表示用)</summary>
            /// <value>在庫数(表示用)</value>
            public string StockQuantityDisp { get; set; }
            /// <summary>Gets or sets 数量管理単位</summary>
            /// <value>数量管理単位</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(数量)</summary>
            /// <value>小数点以下桁数(数量)</value>
            public int UnitDigit { get; set; }
            /// <summary>Gets or sets 丸め処理区分</summary>
            /// <value>丸め処理区分</value>
            public int RoundDivision { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string ManufacturerName { get; set; }
            /// <summary>Gets or sets 表示年度(From)</summary>
            /// <value>表示年度(From)</value>
            public string DispYearFrom { get; set; }
            /// <summary>Gets or sets 表示年度(To)</summary>
            /// <value>表示年度(To)</value>
            public string DispYearTo { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス(入庫情報)
        /// </summary>
        public class searchResultStorageInfo : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 入庫日</summary>
            /// <value>入庫日</value>
            public DateTime InoutDatetime { get; set; }
            /// <summary>Gets or sets 予備品倉庫ID</summary>
            /// <value>予備品倉庫ID</value>
            public long? StorageLocationId { get; set; }
            /// <summary>Gets or sets 棚番ID</summary>
            /// <value>棚番ID</value>
            public long? PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚番コード</summary>
            /// <value>棚番コード</value>
            public string PartsLocationCd { get; set; }
            /// <summary>Gets or sets 棚番翻訳</summary>
            /// <value>棚番翻訳</value>
            public string PartsLocationName { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 予備品倉庫ID(棚情報)</summary>
            /// <value>予備品倉庫ID(棚情報)</value>
            public long? PartsStorageLocationId { get; set; }
            /// <summary>Gets or sets 新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public long OldNewStructureId { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCd { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long AccountStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string AccountCd { get; set; }
            /// <summary>Gets or sets 勘定科目の新旧区分</summary>
            /// <value>勘定科目の新旧区分</value>
            public string AccountOldNewDivision { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 管理No.</summary>
            /// <value>管理No.</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 仕入先ID</summary>
            /// <value>仕入先ID</value>
            public long? VenderId { get; set; }
            /// <summary>Gets or sets 仕入先ID</summary>
            /// <value>仕入先ID</value>
            public long? VenderStructureId { get; set; }
            /// <summary>Gets or sets 仕入先コード</summary>
            /// <value>仕入先名</value>
            public string VenderName { get; set; }
            /// <summary>Gets or sets 入庫数</summary>
            /// <value>入庫数</value>
            public decimal? StorageQuantity { get; set; }
            /// <summary>Gets or sets 入庫数(表示用)</summary>
            /// <value>入庫数(表示用)</value>
            public string? StorageQuantityDisp { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public decimal? UnitPrice { get; set; }
            /// <summary>Gets or sets 入庫単価(表示用)</summary>
            /// <value>入庫単価(表示用)</value>
            public string UnitPriceDisp { get; set; }
            /// <summary>Gets or sets 入庫金額</summary>
            /// <value>入庫金額</value>
            public decimal? StorageAmount { get; set; }
            /// <summary>Gets or sets 入庫金額(表示用)</summary>
            /// <value>入庫金額(表示用)</value>
            public string StorageAmountDisp { get; set; }
            /// <summary>Gets or sets 数量管理単位</summary>
            /// <value>数量管理単位</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 金額管理単位</summary>
            /// <value>金額管理単位</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(数量)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int UnitDigit { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(金額)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int CurrencyDigit { get; set; }
            /// <summary>Gets or sets 丸め処理区分</summary>
            /// <value>丸め処理区分</value>
            public int RoundDivision { get; set; }
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public long FactoryId { get; set; }
            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public long? WorkNo { get; set; }
            /// <summary>Gets or sets 画面タイプ</summary>
            /// <value>画面タイプ</value>
            public int FormType { get; set; }
            /// <summary>Gets or sets 受払履歴ID</summary>
            /// <value>受払履歴ID</value>
            public long InoutHistoryId { get; set; }
            /// <summary>Gets or sets 棚番・棚枝番の結合文字列</summary>
            /// <value>棚番・棚枝番の結合文字列</value>
            public string JoinString { get; set; }
            /// <summary>Gets or sets 管理工場ID</summary>
            /// <value>管理工場ID</value>
            public long PartsFactoryId { get; set; }
            /// <summary>Gets or sets 新品購入時の最新のロットの入庫単価</summary>
            /// <value>新品購入時の最新のロットの入庫単価</value>
            public string UnitPriceByNewestLot { get; set; }
        }

        /// <summary>
        /// 予備品情報とその関連情報を取得(初期表示する際に使用)
        /// </summary>
        public class initValByPartsInfo
        {
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCode { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long AccountStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string AccountCode { get; set; }
            /// <summary>Gets or sets 新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public long OldNewStructureId { get; set; }
            /// <summary>Gets or sets 新旧区分ID</summary>
            /// <value>新旧区分</value>
            public string OldNewDivision { get; set; }
        }
    }
}
