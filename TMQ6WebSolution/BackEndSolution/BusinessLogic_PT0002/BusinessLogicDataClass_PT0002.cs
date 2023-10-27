using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_PT0002
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_PT0002
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            // 検索条件に使用する場合は、検索条件格納共通クラスを継承してください。

            /// <summary>Gets or sets 作業日From</summary>
            /// <value>作業日From</value>
            public string WorkingDayFrom { get; set; }
            /// <summary>Gets or sets 作業日To</summary>
            /// <value>作業日To</value>
            public string WorkingDayTo { get; set; }
            /// <summary>Gets or sets 作業日</summary>
            /// <value>作業日</value>
            public DateTime WorkingDay { get; set; }
            /// <summary>Gets or sets 作業日翌日</summary>
            /// <value>作業日翌日</value>
            public DateTime WorkingDayNext { get; set; }
            /// <summary>Gets or sets 工場IDリスト</summary>
            /// <value>工場IDリスト</value>
            public List<int> FactoryIdList { get; set; }
            /// <summary>Gets or sets 職種IDリスト</summary>
            /// <value>職種IDリスト</value>
            public List<int> JobIdList { get; set; }
            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public List<int> WorkNoList { get; set; }
            /// <summary>Gets or sets 予備品倉庫検索用棚ID</summary>
            /// <value>予備品倉庫検索用棚ID</value>
            public int PartsLocationId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス(入庫)
        /// </summary>
        public class searchResultEnter : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 入庫日</summary>
            /// <value>入庫日</value>
            public string ReceivingDatetime { get; set; }
            /// <summary>Gets or sets 入庫No</summary>
            /// <value>入庫No</value>
            public int LotNo { get; set; }
            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public string PartsLocationName { get; set; }
            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public long PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 予備品No</summary>
            /// <value>予備品No</value>
            public string PartsNo { get; set; }
            /// <summary>Gets or sets 予備品名称</summary>
            /// <value>予備品名称</value>
            public string PartsName { get; set; }
            /// <summary>Gets or sets 新旧区分</summary>
            /// <value>新旧区分</value>
            public string OldNewStructureName { get; set; }
            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string Dimensions { get; set; }
            /// <summary>Gets or sets 受払数(入庫数)</summary>
            /// <value>受払数(入庫数)</value>
            public string InoutQuantity { get; set; }
            /// <summary>Gets or sets 数量単位名称</summary>
            /// <value>数量単位名称</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public string UnitPrice { get; set; }
            /// <summary>Gets or sets 入庫単価(単位)</summary>
            /// <value>入庫単価(単位)</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 入庫金額</summary>
            /// <value>入庫金額</value>
            public string AmountMoney { get; set; }
            /// <summary>Gets or sets 部門CD</summary>
            /// <value>部門CD</value>
            public string DepartmentCd { get; set; }
            /// <summary>Gets or sets 部門名</summary>
            /// <value>部門名</value>
            public string DepartmentNm { get; set; }
            /// <summary>Gets or sets 勘定科目CD</summary>
            /// <value>勘定科目CD</value>
            public string SubjectCd { get; set; }
            /// <summary>Gets or sets 勘定科目名</summary>
            /// <value>勘定科目名</value>
            public string SubjectNm { get; set; }
            /// <summary>Gets or sets 管理No</summary>
            /// <value>管理No</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int PartsFactoryId { get; set; }

            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public int PartsId { get; set; }
            /// <summary>Gets or sets フラグ</summary>
            /// <value>フラグ</value>
            public string TransitionFlg { get; set; }
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
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int JobStructureId { get; set; }
            /// <summary>Gets or sets 受払履歴ID</summary>
            /// <value>受払履歴ID</value>
            public long InoutHistoryId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス(出庫：親)
        /// </summary>
        public class searchResultIssueParents : searchResultEnter
        {
            /// <summary>Gets or sets 出庫日</summary>
            /// <value>入庫日</value>
            public string InoutDatetime { get; set; }
            /// <summary>Gets or sets 出庫No</summary>
            /// <value>出庫No</value>
            public int WorkNo { get; set; }
            /// <summary>Gets or sets 出庫区分</summary>
            /// <value>出庫区分</value>
            public string ShippingDivisionName { get; set; }
            /// <summary>Gets or sets 出庫数</summary>
            /// <value>出庫数</value>
            public string IssueQuantity { get; set; }
            /// <summary>Gets or sets 出庫金額</summary>
            /// <value>出庫金額</value>
            public string IssueMonney { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス（移庫）
        /// </summary>
        public class searchResultTransfer : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 移庫日</summary>
            /// <value>移庫日</value>
            public string RelocationDate { get; set; }

            /// <summary>Gets or sets 移庫品No</summary>
            /// <value>移庫No</value>
            public int TransferNo { get; set; }

            /// <summary>Gets or sets 予備品No</summary>
            /// <value>予備品No</value>
            public string PartsNo { get; set; }

            /// <summary>Gets or sets 予備品名</summary>
            /// <value>予備品名</value>
            public string PartsName { get; set; }

            /// <summary>Gets or sets 新旧区分</summary>
            /// <value>新旧区分</value>
            public string OldNewStructureName { get; set; }

            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string Dimensions { get; set; }

            /// <summary>Gets or sets 移庫数</summary>
            /// <value>移庫数</value>
            public string TransferCount { get; set; }

            /// <summary>Gets or sets 数量管理単位</summary>
            /// <value>数量管理単位</value>
            public string UnitName { get; set; }

            /// <summary>Gets or sets 入庫No.</summary>
            /// <value>入庫No.</value>
            public int LotNo { get; set; }

            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public string UnitPrice { get; set; }

            /// <summary>Gets or sets 金額管理単位</summary>
            /// <value>金額管理単位</value>
            public string CurrencyName { get; set; }

            /// <summary>Gets or sets 移庫金額</summary>
            /// <value>移庫金額</value>
            public string TransferAmount { get; set; }

            /// <summary>Gets or sets 部門CD</summary>
            /// <value>部門CD</value>
            public string DepartmentCd { get; set; }

            /// <summary>Gets or sets 部門名</summary>
            /// <value>部門名</value>
            public string DepartmentNm { get; set; }

            /// <summary>Gets or sets 勘定科目CD</summary>
            /// <value>勘定科目CD</value>
            public string SubjectCd { get; set; }

            /// <summary>Gets or sets 勘定科目</summary>
            /// <value>勘定科目</value>
            public string SubjectNm { get; set; }

            /// <summary>Gets or sets 管理No.</summary>
            /// <value>管理No.</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int PartsFactoryId { get; set; }
            /// <summary>Gets or sets 予備品倉庫構成ID</summary>
            /// <value>予備品倉庫構成ID</value>
            public int? LocationStructureId { get; set; }
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
            /// <summary>Gets or sets 翻訳テキスト</summary>
            /// <value>翻訳テキスト</value>
            public string TranslationText { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス（棚番移庫）
        /// </summary>
        public class searchResultShed : searchResultTransfer
        {
            /// <summary>Gets or sets 移庫元　予備品倉庫</summary>
            /// <value>移庫元　予備品倉庫</value>
            public int StorageLocationId { get; set; }
            /// <summary>Gets or sets 移庫元　予備品倉庫名称</summary>
            /// <value>移庫元　予備品倉庫名称</value>
            public string StorageLocationName { get; set; }
            /// <summary>Gets or sets 移庫元　棚番</summary>
            /// <value>移庫元　棚番</value>
            public string LocationId { get; set; }
            /// <summary>Gets or sets 移庫元　棚枝番</summary>
            /// <value>移庫元　棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 移庫先　予備品倉庫</summary>
            /// <value>移庫先　予備品倉庫</value>
            public int ToStorageLocationId { get; set; }
            /// <summary>Gets or sets 移庫先　予備品倉庫名称</summary>
            /// <value>移庫先　予備品倉庫名称</value>
            public string ToStorageLocationName { get; set; }
            /// <summary>Gets or sets 移庫先　棚番</summary>
            /// <value>移庫先　棚番</value>
            public string ToLocationId { get; set; }
            /// <summary>Gets or sets 移庫先　棚枝番</summary>
            /// <value>移庫先　棚枝番</value>
            public string ToPartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 予備品ID(入力画面絞込みのため)</summary>
            /// <value>予備品ID</value>
            public int PartsId { get; set; }
            /// <summary>Gets or sets 作業No(入力画面絞込みのため)</summary>
            /// <value>作業No</value>
            public int WorkNo { get; set; }
            /// <summary>Gets or sets フラグ</summary>
            /// <value>フラグ</value>
            public string TransitionFlg { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス（部門移庫）
        /// </summary>
        public class searchResultCategory : searchResultTransfer
        {
            /// <summary>Gets or sets 部門CD</summary>
            /// <value>部門CD</value>
            public string ToDepartmentCd { get; set; }
            /// <summary>Gets or sets 部門名</summary>
            /// <value>部門名</value>
            public string ToDepartmentNm { get; set; }
            /// <summary>Gets or sets 勘定科目CD</summary>
            /// <value>勘定科目CD</value>
            public string ToSubjectCd { get; set; }
            /// <summary>Gets or sets 移庫先　勘定科目</summary>
            /// <value>移庫先　勘定科目</value>
            public string ToSubjectNm { get; set; }
            /// <summary>Gets or sets 移庫先　管理No.</summary>
            /// <value>移庫先　管理No.</value>
            public string ToManagementNo { get; set; }
            /// <summary>Gets or sets 移庫先　管理区分</summary>
            /// <value>移庫先　管理区分</value>
            public string ToManagementDivision { get; set; }
            /// <summary>Gets or sets 予備品ID(入力画面絞込みのため)</summary>
            /// <value>予備品ID</value>
            public int PartsId { get; set; }
            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public int WorkNo { get; set; }
            /// <summary>Gets or sets フラグ</summary>
            /// <value>フラグ</value>
            public string TransitionFlg { get; set; }
        }

    }
}
