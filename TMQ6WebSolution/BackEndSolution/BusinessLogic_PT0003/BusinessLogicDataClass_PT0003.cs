using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_PT0003
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_PT0003
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 対象年月(月初)</summary>
            /// <value>対象年月(月初)</value>
            public DateTime TargetYearMonth { get; set; }
            /// <summary>Gets or sets 対象年月(月末)</summary>
            /// <value>対象年月(月末)</value>
            public DateTime TargetYearMonthNext { get; set; }
            /// <summary>Gets or sets 予備品倉庫ID</summary>
            /// <value>予備品倉庫ID</value>
            public int StorageLocationId { get; set; }
            /// <summary>Gets or sets 棚番ID</summary>
            /// <value>棚番ID</value>
            public List<int> PartsLocationIdList { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public List<long> DepartmentIdList { get; set; }
            /// <summary>Gets or sets 準備状況</summary>
            /// <value>準備状況</value>
            public int? ReadyStatus { get; set; }
            /// <summary>Gets or sets 工場IDリスト</summary>
            /// <value>工場IDリスト</value>
            public List<int> FactoryIdList { get; set; }
            /// <summary>Gets or sets 職種IDリスト</summary>
            /// <value>職種IDリスト</value>
            public List<int> JobIdList { get; set; }
            /// <summary>Gets or sets 棚差調整ID</summary>
            /// <value>棚差調整ID</value>
            public long InventoryDifferenceId { get; set; }
            /// <summary>Gets or sets 棚卸ID(取込時の一覧表示時に使用)</summary>
            /// <value>棚卸ID</value>
            public string InventoryId { get; set; }
            /// <summary>Gets or sets 棚卸IDリスト(取込時の一覧表示時に使用)</summary>
            /// <value>棚卸IDリスト</value>
            public List<long> InventoryIdList { get; set; }
            /// <summary>Gets or sets 棚卸ID指定フラグ(取込時の一覧表示時に使用)</summary>
            /// <value>棚卸ID指定フラグ</value>
            public bool InventoryIdFlg { get; set; }
            /// <summary>Gets or sets 部門ID(取込時に使用)</summary>
            /// <value>部門ID</value>
            public string DepartmentId { get; set; }
        }

        /// <summary>
        /// 棚卸一覧のデータクラス
        /// </summary>
        public class searchInventoryResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 棚番ID</summary>
            /// <value>棚番ID</value>
            public long PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 棚番(表示用)</summary>
            /// <value>棚番(表示用)</value>
            public string PartsLocationDisp { get; set; }
            /// <summary>Gets or sets 予備品№</summary>
            /// <value>予備品№</value>
            public string PartsNo { get; set; }
            /// <summary>Gets or sets 予備品名称</summary>
            /// <value>予備品名称</value>
            public string PartsName { get; set; }
            /// <summary>Gets or sets 新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public int OldNewStructureId { get; set; }
            /// <summary>Gets or sets 新旧区分(表示用)</summary>
            /// <value>新旧区分(表示用)</value>
            public string OldNewNm { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 部門(表示用)</summary>
            /// <value>部門(表示用)</value>
            public string DepartmentNm { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCd { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long AccountStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目(表示用)</summary>
            /// <value>勘定科目(表示用)</value>
            public string SubjectNm { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string SubjectCd { get; set; }
            /// <summary>Gets or sets 在庫あり</summary>
            /// <value>在庫あり</value>
            public bool StockQuantityFlg { get; set; }
            /// <summary>Gets or sets 棚差あり</summary>
            /// <value>棚差あり</value>
            public bool InventoryDiffFlg { get; set; }
            /// <summary>Gets or sets 在庫数+単位(表示用)</summary>
            /// <value>在庫数+単位(表示用)</value>
            public string StockQuantityDisplay { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public decimal StockQuantity { get; set; }
            /// <summary>Gets or sets 棚卸準備日時</summary>
            /// <value>棚卸準備日時</value>
            public DateTime? PreparationDatetime { get; set; }
            /// <summary>Gets or sets 棚卸日時</summary>
            /// <value>棚卸日時</value>
            public DateTime? InventoryDatetime { get; set; }
            /// <summary>Gets or sets 棚卸調整日時</summary>
            /// <value>棚卸調整日時</value>
            public DateTime? DifferenceDatetime { get; set; }
            /// <summary>Gets or sets 棚卸数</summary>
            /// <value>棚卸数</value>
            public decimal? InventoryQuantity { get; set; }
            /// <summary>Gets or sets 棚差調整数</summary>
            /// <value>棚差調整数</value>
            public decimal? InoutQuantity { get; set; }
            /// <summary>Gets or sets 棚差</summary>
            /// <value>棚差</value>
            public decimal? InventoryDiff { get; set; }
            /// <summary>Gets or sets 棚卸確定日時</summary>
            /// <value>棚卸確定日時</value>
            public DateTime? FixedDatetime { get; set; }
            /// <summary>Gets or sets メーカーID</summary>
            /// <value>メーカーID</value>
            public long? ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets メーカー(表示用)</summary>
            /// <value>メーカー(表示用)</value>
            public string ManufacturerName { get; set; }
            /// <summary>Gets or sets 材質</summary>
            /// <value>材質</value>
            public string Materials { get; set; }
            /// <summary>Gets or sets 型式</summary>
            /// <value>型式</value>
            public string ModelType { get; set; }
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets 数量管理単位ID</summary>
            /// <value>数量管理単位ID</value>
            public long UnitStructureId { get; set; }
            /// <summary>Gets or sets 数量単位名称</summary>
            /// <value>数量単位名称</value>
            public string Unit { get; set; }
            /// <summary>Gets or sets 金額管理単位ID</summary>
            /// <value>金額管理単位ID</value>
            public long CurrencyStructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 棚卸ID</summary>
            /// <value>棚卸ID</value>
            public long? InventoryId { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(数量)</summary>
            /// <value>小数点以下桁数(数量)</value>
            public int UnitDigit { get; set; }
            /// <summary>Gets or sets 丸め処理区分(数量)</summary>
            /// <value>丸め処理区分(数量)</value>
            public int UnitRoundDivision { get; set; }

            //下記は取込時に使用する
            /// <summary>Gets or sets エラーフラグ</summary>
            /// <value>エラーフラグ</value>
            public bool ErrorFlg { get; set; }

        }

        /// <summary>
        /// 入庫/出庫一覧のデータクラス
        /// </summary>
        public class inoutList : inoutHistryList
        {
            /// <summary>Gets or sets 入庫日/出庫日</summary>
            /// <value>入庫日/出庫日</value>
            public DateTime InoutDatetime { get; set; }
            /// <summary>Gets or sets ロットNo</summary>
            /// <value>ロットNo</value>
            public long? LotNo { get; set; }
            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string StandardSize { get; set; }
            /// <summary>Gets or sets 棚卸調整数</summary>
            /// <value>棚卸調整数</value>
            public string InoutQuantityDisplay { get; set; }
            /// <summary>Gets or sets 入庫金額</summary>
            /// <value>入庫金額</value>
            public decimal AmountMoney { get; set; }
            /// <summary>Gets or sets 棚差調整ID</summary>
            /// <value>棚差調整ID</value>
            public long InventoryDifferenceId { get; set; }
            /// <summary>Gets or sets 管理No</summary>
            /// <value>管理No</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 制御用フラグ</summary>
            /// <value>制御用フラグ</value>
            public int ControlFlag { get; set; }
            /// <summary>Gets or sets 制御用文字列</summary>
            /// <value>制御用文字列</value>
            public string ControlChar { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public int DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public int AccountStructureId { get; set; }

        }

        /// <summary>
        /// 入庫単価入力画面のデータクラス
        /// </summary>
        public class enterInput : searchInventoryResult
        {
            /// <summary>Gets or sets 仮</summary>
            /// <value>仮</value>
            public string InoutQuantityDisplay { get; set; }
            /// <summary>Gets or sets 数量単位</summary>
            /// <value>数量単位</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public decimal? UnitPrice { get; set; }
            /// <summary>Gets or sets 金額単位</summary>
            /// <value>金額単位</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 棚差調整ID</summary>
            /// <value>棚差調整ID</value>
            public long InventoryDifferenceId { get; set; }
            /// <summary>Gets or sets 更新シリアルID</summary>
            /// <value>更新シリアルID</value>
            public int UpdateSerialid { get; set; }
        }

        /// <summary>
        /// 棚卸(受払履歴)のデータクラス
        /// </summary>
        public class inoutHistryList : searchInventoryResult
        {
            /// <summary>対象年月 or sets 対象年月</summary>
            /// <value>年度</value>
            public DateTime TargetMonth { get; set; }
            /// <summary>Gets or sets 年度</summary>
            /// <value>年度</value>
            public string Year { get; set; }
            /// <summary>Gets or sets 年月</summary>
            /// <value>年月</value>
            public DateTime MonthYear { get; set; }
            /// <summary>Gets or sets 作業区分(入出庫区分)</summary>
            /// <value>作業区分(入出庫区分)</value>
            public long? workDivisionStructureId { get; set; }
            /// <summary>Gets or sets 作業区分(入出庫区分)(表示用)</summary>
            /// <value>作業区分(入出庫区分)(表示用)</value>
            public string workDivisionName { get; set; }
            /// <summary>Gets or sets 日付</summary>
            /// <value>日付</value>
            public DateTime Date { get; set; }
            /// <summary>Gets or sets 入出庫No</summary>
            /// <value>入出庫No</value>
            public long? InoutNo { get; set; }
            /// <summary>Gets or sets 入庫数(表示用)</summary>
            /// <value>入庫数(表示用)</value>
            public string? InventoryQuantityDisplay { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public decimal? UnitPrice { get; set; }
            /// <summary>Gets or sets 入庫単価(表示用)</summary>
            /// <value>入庫単価(表示用)</value>
            public string UnitPriceDisplay { get; set; }
            /// <summary>Gets or sets 数量単位</summary>
            /// <value>数量単位</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 入庫金額</summary>
            /// <value>入庫金額</value>
            public decimal? InventoryAmount { get; set; }
            /// <summary>Gets or sets 入庫金額(表示用)</summary>
            /// <value>入庫金額(表示用)</value>
            public string InventoryAmountDisplay { get; set; }
            /// <summary>Gets or sets 金額単位</summary>
            /// <value>金額単位</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 出庫数</summary>
            /// <value>出庫数</value>
            public decimal? IssueQuantity { get; set; }
            /// <summary>Gets or sets 出庫数(表示用)</summary>
            /// <value>出庫数(表示用)</value>
            public string? IssueQuantityDisplay { get; set; }
            /// <summary>Gets or sets 出庫金額</summary>
            /// <value>出庫金額</value>
            public decimal? IssueAmount { get; set; }
            /// <summary>Gets or sets 出庫金額(表示用)</summary>
            /// <value>出庫金額(表示用)</value>
            public string? IssueAmountDisplay { get; set; }
            /// <summary>Gets or sets 在庫金額</summary>
            /// <value>在庫金額</value>
            public decimal? StockAmount { get; set; }
            /// <summary>Gets or sets 在庫金額(表示用)</summary>
            /// <value>在庫金額(表示用)</value>
            public string? StockAmountDisplay { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int Status { get; set; }
            /// <summary>Gets or sets 作成日</summary>
            /// <value>作成日</value>
            public DateTime? CleateDate { get; set; }
            /// <summary>Gets or sets 遷移区分</summary>
            /// <value>遷移区分</value>
            public int TransitionDivision { get; set; }
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
            /// <summary>Gets or sets 入庫入力</summary>
            /// <value>入庫入力</value>
            public int Enter { get; set; }
            /// <summary>Gets or sets 出庫入力</summary>
            /// <value>出庫入力</value>
            public int Issue { get; set; }
            /// <summary>Gets or sets 棚番移庫入力</summary>
            /// <value>棚番移庫入力</value>
            public int Shed { get; set; }
            /// <summary>Gets or sets 部門移庫入力</summary>
            /// <value>部門移庫入力</value>
            public int Category { get; set; }
            /// <summary>Gets or sets 受払履歴ID</summary>
            /// <value>受払履歴ID</value>
            public long InoutHistoryId { get; set; }
            /// <summary>Gets or sets 作業区分(js制御用)</summary>
            /// <value>作業区分(js制御用)</value>
            public string ControlFlag { get; set; }
            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No(js制御用)</value>
            public long WorkNo { get; set; }
        }

        /// <summary>
        /// 取込画面のデータクラス
        /// </summary>
        public class uploadInfo : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets エラー内容</summary>
            /// <value>エラー内容</value>
            public string ErrorMessage { get; set; }
            /// <summary>Gets or sets 取込フラグ</summary>
            /// <value>取込フラグ</value>
            public bool UploadFlg { get; set; }
            /// <summary>Gets or sets 棚卸ID</summary>
            /// <value>棚卸ID</value>
            public string InventoryId { get; set; }
            /// <summary>Gets or sets 部門ID(パイプ区切り)</summary>
            /// <value>部門ID</value>
            public string DepartmentId { get; set; }
        }

        /// <summary>
        /// 棚卸一覧　棚卸数の小数部以下桁数のチェック用データクラス
        /// </summary>
        public class checkUnitDigit
        {
            /// <summary>Gets or sets 小数点以下桁数</summary>
            /// <value>小数点以下桁数</value>
            public int UnitDigit { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public List<int> RowNo { get; set; }
        }

        /// <summary>
        /// ロット情報、在庫データのデータクラス
        /// </summary>
        public class lotStockInfo : ComDao.PtLotEntity
        {
            /// <summary>Gets or sets 在庫管理ID</summary>
            /// <value>在庫管理ID</value>
            public long InventoryControlId { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public decimal StockQuantity { get; set; }
        }

        /// <summary>
        /// 新規登録画面のデータクラス
        /// </summary>
        public class registInfo : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 予備品倉庫ID</summary>
            /// <value>予備品倉庫ID</value>
            public int StorageLocationId { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int DistrictId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 工場IDリスト</summary>
            /// <value>工場IDリスト</value>
            public string FactoryIdList { get; set; }
            /// <summary>Gets or sets 棚と棚枝番を結合する文字列</summary>
            /// <value>棚と棚枝番を結合する文字列</value>
            public string JoinString { get; set; }
            /// <summary>Gets or sets 棚番ID</summary>
            /// <value>棚番ID</value>
            public long? PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 予備品№</summary>
            /// <value>予備品№</value>
            public string PartsNo { get; set; }
            /// <summary>Gets or sets 新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public int OldNewStructureId { get; set; }
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
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public decimal StockQuantity { get; set; }
            /// <summary>Gets or sets 棚卸準備日時</summary>
            /// <value>棚卸準備日時</value>
            public DateTime? PreparationDatetime { get; set; }
            /// <summary>Gets or sets 棚卸日時</summary>
            /// <value>棚卸日時</value>
            public DateTime? InventoryDatetime { get; set; }
            /// <summary>Gets or sets 棚卸数</summary>
            /// <value>棚卸数</value>
            public decimal? InventoryQuantity { get; set; }
            /// <summary>Gets or sets 棚差</summary>
            /// <value>棚差</value>
            public decimal? InventoryDiff { get; set; }
            /// <summary>Gets or sets 数量管理単位ID</summary>
            /// <value>数量管理単位ID</value>
            public long UnitStructureId { get; set; }
            /// <summary>Gets or sets 金額管理単位ID</summary>
            /// <value>金額管理単位ID</value>
            public long CurrencyStructureId { get; set; }
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets 丸め処理区分</summary>
            /// <value>丸め処理区分</value>
            public string RoundDivision { get; set; }
        }
    }
}
