using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPUtil.APShiwakeSendUtil
{
    /// <summary>
    /// 仕訳更新にて使用するデータクラス
    /// </summary>
    public class ShiwakeSendDataClass
    {
        /// <summary>
        /// 処理状況区分
        /// </summary>
        public static class ProcessDivison
        {
            /// <summary>承認</summary>
            public const int APPROVAL = 1;
            /// <summary>承認取消</summary>
            public const int APPROVAL_CANCEL = 2;
            /// <summary>取消</summary>
            public const int CANCEL = 3;
        }

        /// <summary>
        /// 仕訳区分
        /// </summary>
        public static class ShiwakeDivision
        {
            /// <summary>仕訳コード_売上</summary>
            public const string SalesShiwakeCd = "UA2";
            /// <summary>仕訳区分_売上</summary>
            public const string SalesShiwakeDiv = "01";
            /// <summary>仕訳区分_売上(返品)</summary>
            public const string SalesRtnShiwakeDiv = "03";
            /// <summary>仕訳区分_売上_消費税</summary>
            public const string SalesTaxShiwakeDiv = "02";
            /// <summary>仕訳区分_売上_消費税(返品)</summary>
            public const string SalesTaxRtnShiwakeDiv = "07";

            /// <summary>仕訳コード_仕入</summary>
            public const string StockingShiwakeCd = "UA1";
            /// <summary>仕訳区分_仕入</summary>
            public const string StockingShiwakeDiv = "01";
            /// <summary>仕訳区分_仕入(返品)</summary>
            public const string StockingRtnShiwakeDiv = "03";
            /// <summary>仕訳区分_仕入_消費税</summary>
            public const string StockingTaxShiwakeDiv = "02";
            /// <summary>仕訳区分_仕入_消費税(返品)</summary>
            public const string StockingRtnTaxShiwakeDiv = "04";

            /// <summary>仕訳コード_入金</summary>
            public const string CreditShiwakeCd = "UA3";
            /// <summary>仕訳区分_入金</summary>x
            public const string CreditShiwakeDiv = "01";

            /// <summary>仕訳コード_支払</summary>
            public const string PaymentShiwakeCd = "UA1";
            /// <summary>仕訳区分_支払</summary>
            public const string PaymentShiwakeDiv = "07";

            /// <summary>仕訳コード_入金消込</summary>
            public const string CreditErazerShiwakeCd = "UA4";
            /// <summary>仕訳区分_入金消込</summary>
            public const string CreditErazerShiwakeDiv = "01";

            /// <summary>仕訳コード_支払消込</summary>
            public const string PaymentErazerShiwakeCd = "UA4";
            /// <summary>仕訳区分_支払消込</summary>
            public const string PaymentErazerShiwakeDiv = "02";

            /// <summary>仕訳コード_相殺</summary>
            public const string OffsetShiwakeCd = "UA5";
            /// <summary>仕訳区分_相殺</summary>
            public const string OffsetShiwakeDiv = "01";
        }

        /// <summary>
        /// 結果情報（在庫更新戻り値)
        /// </summary>
        public class ShiwakeSendReturnInfo
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ShiwakeSendReturnInfo()
            {
                ResultValue = true;
                Message = string.Empty;
            }

            /// <summary>
            /// コンストラクタ(ﾊﾟﾗﾒｰﾀあり)
            /// </summary>
            /// <param name="paramResult">結果</param>
            /// <param name="paramMessage">エラーメッセージ</param>
            public ShiwakeSendReturnInfo(bool paramResult, string paramMessage)
            {
                ResultValue = paramResult;
                Message = paramMessage;
            }

            /// <summary>Gets or sets a value indicating whether gets or sets 結果</summary>
            /// <value>True:正常 false:異常</value>
            public bool ResultValue { get; set; }
            /// <summary>Gets or sets エラーメッセージ</summary>
            /// <value>正常時：空白　異常時：エラーメッセージ</value>
            public string Message { get; set; }
        }

        /// <summary>
        /// 在庫更新用パラメータデータクラス
        /// </summary>
        public class ShiwakeSendParameter
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ShiwakeSendParameter()
            {
                //初期値設定

            }

            /// <summary>
            /// コンストラクタ(コピー機能)
            /// </summary>
            /// <param name="paramData">在庫更新用パラメータデータクラス</param>
            public ShiwakeSendParameter(ShiwakeSendParameter paramData)
            {
                //初期値設定
                SalesNo = paramData.SalesNo;
                Procsess = paramData.Procsess;

            }

            /// <summary>Gets or sets 処理状況</summary>
            /// <value>処理状況</value>
            public int Procsess { get; set; }
            /// <summary>Gets or sets  売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets  仕入番号</summary>
            /// <value>仕入番号</value>
            public string StockingNo { get; set; }
            /// <summary>Gets or sets  仕入行番号</summary>
            /// <value>仕入行番号</value>
            public int StockingRowNo { get; set; }
            /// <summary>Gets or sets  入金番号</summary>
            /// <value>入金番号</value>
            public string CreditNo { get; set; }
            /// <summary>Gets or sets  入金行番号</summary>
            /// <value>入金/行番号</value>
            public int CreditRowNo { get; set; }
            /// <summary>Gets or sets  支払伝票番号</summary>
            /// <value>支払伝票番号</value>
            public string PaymentSlipNo { get; set; }
            /// <summary>Gets or sets  支払行番号</summary>
            /// <value>支払行番号</value>
            public int PaymentSlipRowNo { get; set; }
            /// <summary>Gets or sets  消込番号</summary>
            /// <value>消込番号</value>
            public string EraserNo { get; set; }
            /// <summary>Gets or sets  相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
        }

        /// <summary>
        /// 売上テーブル項目
        /// </summary>
        public class SalesTableItem
        {
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets SEQ番号</summary>
            /// <value>SEQ番号</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets 処理状況</summary>
            /// <value>処理状況</value>
            public int Process { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int Status { get; set; }
            /// <summary>Gets or sets 連携FLG</summary>
            /// <value>連携FLG</value>
            public int LinkFlg { get; set; }
            /// <summary>Gets or sets 仕訳コード</summary>
            /// <value>仕訳コード</value>
            public string ShiwakeCd { get; set; }
            /// <summary>Gets or sets 仕訳番号</summary>
            /// <value>仕訳番号</value>
            public string ShiwakeNo { get; set; }
            /// <summary>Gets or sets 仕訳番号_返品</summary>
            /// <value>仕訳番号_返品</value>
            public string ShiwakeRtnNo { get; set; }
            /// <summary>Gets or sets 仕訳番号_消費税</summary>
            /// <value>仕訳番号_消費税</value>
            public string ShiwakeTaxNo { get; set; }
            /// <summary>Gets or sets 仕訳番号_消費税_返品</summary>
            /// <value>仕訳番号_消費税_返品</value>
            public string ShiwakeTaxRtnNo { get; set; }
            /// <summary>Gets or sets 売上日_勘定年月</summary>
            /// <value>売上日_勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 売上日_転記日付</summary>
            /// <value>売上日_転記日付</value>
            public DateTime SalesDate { get; set; }
            /// <summary>Gets or sets 借方会計部門コード_売上</summary>
            /// <value>借方会計部門コード_売上</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方勘定科目コード_売上</summary>
            /// <value>借方勘定科目コード_売上</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方会計部門コード_売上</summary>
            /// <value>貸方会計部門コード_売上</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方勘定科目コード_売上</summary>
            /// <value>貸方勘定科目コード_売上</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 貸方勘定科目コード_消費税</summary>
            /// <value>貸方勘定科目コード_消費税</value>
            public string CreditTitleCdTax { get; set; }
            /// <summary>Gets or sets 得意先コード</summary>
            /// <value>得意先コード</value>
            public string Kunner { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 発生金額_売上</summary>
            /// <value>発生金額_売上</value>
            public decimal SalesAmount { get; set; }
            /// <summary>Gets or sets 発生金額_消費税</summary>
            /// <value>発生金額_消費税</value>
            public decimal TaxAmount { get; set; }
            /// <summary>Gets or sets 借方税コード</summary>
            /// <value>借方税コード</value>
            public string DebitTaxCd { get; set; }
            /// <summary>Gets or sets 貸方税コード</summary>
            /// <value>貸方税コード</value>
            public string CreditTaxCd { get; set; }
            /// <summary>Gets or sets 貸方税コード_消費税</summary>
            /// <value>貸方税コード_消費税</value>
            public string CreditTaxCdTax { get; set; }
            /// <summary>Gets or sets エラー判定</summary>
            /// <value>エラー判定</value>
            public int ShiwakeError { get; set; }
            /// <summary>Gets or sets エラー判定_売上_返品</summary>
            /// <value>エラー判定_売上_返品</value>
            public int ShiwakeErrorRtn { get; set; }
            /// <summary>Gets or sets エラー判定_売上_消費税</summary>
            /// <value>エラー判定_売上_消費税</value>
            public int ShiwakeErrorTax { get; set; }
            /// <summary>Gets or sets エラー判定_売上_消費税_返品</summary>
            /// <value>エラー判定_売上_消費税_返品</value>
            public int ShiwakeErrorTaxRtn { get; set; }
        }

        /// <summary>
        /// 仕入テーブル項目
        /// </summary>
        public class StockingTableItem
        {
            /// <summary>Gets or sets 仕入番号</summary>
            /// <value>仕入番号</value>
            public string StockingNo { get; set; }
            /// <summary>Gets or sets 仕入行番号</summary>
            /// <value>仕入行番号</value>
            public int StockingRowNo { get; set; }
            /// <summary>Gets or sets SEQ番号</summary>
            /// <value>SEQ番号</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets 処理状況</summary>
            /// <value>処理状況</value>
            public int Process { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int Status { get; set; }
            /// <summary>Gets or sets 連携FLG</summary>
            /// <value>連携FLG</value>
            public int LinkFlg { get; set; }
            /// <summary>Gets or sets 仕訳コード</summary>
            /// <value>仕訳コード</value>
            public string ShiwakeCd { get; set; }
            /// <summary>Gets or sets 仕訳番号</summary>
            /// <value>仕訳番号</value>
            public string ShiwakeNo { get; set; }
            /// <summary>Gets or sets 仕訳番号_返品</summary>
            /// <value>仕訳番号_返品</value>
            public string ShiwakeRtnNo { get; set; }
            /// <summary>Gets or sets 仕訳番号_消費税</summary>
            /// <value>仕訳番号_消費税</value>
            public string ShiwakeTaxNo { get; set; }
            /// <summary>Gets or sets 仕訳番号_消費税_返品</summary>
            /// <value>仕訳番号_消費税_返品</value>
            public string ShiwakeTaxRtnNo { get; set; }
            /// <summary>Gets or sets 受入検査承認日_勘定年月</summary>
            /// <value>受入検査承認日_勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 受入日_転記日付</summary>
            /// <value>受入日_転記日付</value>
            public DateTime StockingDate { get; set; }
            /// <summary>Gets or sets 受入日_支払基準日</summary>
            /// <value>受入日_支払基準日</value>
            public string PaymentScheDate { get; set; }
            /// <summary>Gets or sets 借方会計部門コード_受入</summary>
            /// <value>借方会計部門コード_受入</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方勘定科目コード_受入</summary>
            /// <value>借方勘定科目コード_受入</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 借方勘定科目コード_消費税</summary>
            /// <value>借方勘定科目コード_消費税</value>
            public string DebitTitleCdTax { get; set; }
            /// <summary>Gets or sets 貸方会計部門コード_受入</summary>
            /// <value>貸方会計部門コード_受入</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方会計部門コード_受入</summary>
            /// <value>貸方会計部門コード_受入</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 仕入先コード</summary>
            /// <value>仕入先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 発生金額_受入</summary>
            /// <value>発生金額_受入</value>
            public decimal StockingAmount { get; set; }
            /// <summary>Gets or sets 発生金額_消費税</summary>
            /// <value>発生金額_消費税</value>
            public decimal TaxAmonut { get; set; }
            /// <summary>Gets or sets 借方税コード</summary>
            /// <value>借方税コード</value>
            public string DebitTaxCd { get; set; }
            /// <summary>Gets or sets 借方税コード_消費税</summary>
            /// <value>借方税コード_消費税</value>
            public string DebitTaxCdTax { get; set; }
            /// <summary>Gets or sets 貸方税コード</summary>
            /// <value>貸方税コード</value>
            public string CreditTaxCd { get; set; }
            /// <summary>Gets or sets サイト</summary>
            /// <value>サイト</value>
            public int Site { get; set; }
            /// <summary>Gets or sets エラー判定</summary>
            /// <value>エラー判定</value>
            public int ShiwakeError { get; set; }
            /// <summary>Gets or sets エラー判定_仕入_返品</summary>
            /// <value>エラー判定_仕入_返品</value>
            public int ShiwakeErrorRtn { get; set; }
            /// <summary>Gets or sets エラー判定_仕入_消費税</summary>
            /// <value>エラー判定_仕入_消費税</value>
            public int ShiwakeErrorTax { get; set; }
            /// <summary>Gets or sets エラー判定_仕入_消費税_返品</summary>
            /// <value>エラー判定_仕入_消費税_返品</value>
            public int ShiwakeErrorTaxRtn { get; set; }
        }

        /// <summary>
        /// 入金テーブル項目
        /// </summary>
        public class CreditTableItem
        {
            /// <summary>Gets or sets 入金番号</summary>
            /// <value>入金番号</value>
            public string CreditNo { get; set; }
            /// <summary>Gets or sets 入金行番号</summary>
            /// <value>入金行番号</value>
            public int CreditRowNo { get; set; }
            /// <summary>Gets or sets SEQ番号</summary>
            /// <value>SEQ番号</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets 処理状況</summary>
            /// <value>処理状況</value>
            public int Process { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int Status { get; set; }
            /// <summary>Gets or sets 連携FLG</summary>
            /// <value>連携FLG</value>
            public int LinkFlg { get; set; }
            /// <summary>Gets or sets 仕訳コード</summary>
            /// <value>仕訳コード</value>
            public string ShiwakeCd { get; set; }
            /// <summary>Gets or sets 仕訳番号</summary>
            /// <value>仕訳番号</value>
            public string ShiwakeNo { get; set; }
            /// <summary>Gets or sets 入金日_勘定年月</summary>
            /// <value>入金日_勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 入金日_転記日付</summary>
            /// <value>入金日_転記日付</value>
            public DateTime CreditDate { get; set; }
            /// <summary>Gets or sets 借方会計部門コード_入金T</summary>
            /// <value>借方会計部門コード_入金T</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方勘定科目コード_入金T</summary>
            /// <value>借方勘定科目コード_入金T</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方会計部門コード_入金T</summary>
            /// <value>貸方会計部門コード_入金T</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方勘定科目コード_入金T</summary>
            /// <value>貸方勘定科目コード_入金T</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 支店コード</summary>
            /// <value>支店コード</value>
            public string BranchCd { get; set; }
            /// <summary>Gets or sets 預金種別</summary>
            /// <value>預金種別</value>
            public string DepositType { get; set; }
            /// <summary>Gets or sets 預金勘定</summary>
            /// <value>預金勘定</value>
            public string DepositAccount { get; set; }
            /// <summary>Gets or sets 発生金額_入金</summary>
            /// <value>発生金額_入金</value>
            public decimal CreditAmount { get; set; }
            /// <summary>Gets or sets 借方税コード</summary>
            /// <value>借方税コード</value>
            public string DebitTaxCd { get; set; }
            /// <summary>Gets or sets 貸方税コード</summary>
            /// <value>貸方税コード</value>
            public string CreditTaxCd { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets エラー判定</summary>
            /// <value>エラー判定</value>
            public int ShiwakeError { get; set; }
        }

        /// <summary>
        /// 支払テーブル項目
        /// </summary>
        public class PaymentTableItem
        {
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentSlipNo { get; set; }
            /// <summary>Gets or sets 支払伝票行番号</summary>
            /// <value>支払伝票行番号</value>
            public int PaymentSlipRowNo { get; set; }
            /// <summary>Gets or sets SEQ番号</summary>
            /// <value>SEQ番号</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets 処理状況</summary>
            /// <value>処理状況</value>
            public int Process { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int Status { get; set; }
            /// <summary>Gets or sets 連携FLG</summary>
            /// <value>連携FLG</value>
            public int LinkFlg { get; set; }
            /// <summary>Gets or sets 仕訳コード</summary>
            /// <value>仕訳コード</value>
            public string ShiwakeCd { get; set; }
            /// <summary>Gets or sets 仕訳番号</summary>
            /// <value>仕訳番号</value>
            public string ShiwakeNo { get; set; }
            /// <summary>Gets or sets 支払日_勘定年月</summary>
            /// <value>支払日_勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 支払日_転記日付</summary>
            /// <value>支払日_転記日付</value>
            public DateTime PaymentDate { get; set; }
            /// <summary>Gets or sets 借方会計部門コード_支払T</summary>
            /// <value>借方会計部門コード_支払T</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方勘定科目コード_支払T</summary>
            /// <value>借方勘定科目コード_支払T</value>
            public string DebitAccountsCd { get; set; }
            /// <summary>Gets or sets 貸方会計部門コード_支払T</summary>
            /// <value>貸方会計部門コード_支払T</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方勘定科目コード_支払T</summary>
            /// <value>貸方勘定科目コード_支払T</value>
            public string CreditAccountsCd { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 支店コード</summary>
            /// <value>支店コード</value>
            public string BranchCd { get; set; }
            /// <summary>Gets or sets 預金種別</summary>
            /// <value>預金種別</value>
            public string DepositType { get; set; }
            /// <summary>Gets or sets 預金勘定</summary>
            /// <value>預金勘定</value>
            public string DepositAccount { get; set; }
            /// <summary>Gets or sets 発生金額_支払</summary>
            /// <value>発生金額_支払</value>
            public decimal PaymentAmount { get; set; }
            /// <summary>Gets or sets 借方税コード</summary>
            /// <value>借方税コード</value>
            public string DebitTaxCd { get; set; }
            /// <summary>Gets or sets 貸方税コード</summary>
            /// <value>貸方税コード</value>
            public string CreditTaxCd { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets エラー判定</summary>
            /// <value>エラー判定</value>
            public int ShiwakeError { get; set; }
        }

        /// <summary>
        /// 入金消込テーブル項目
        /// </summary>
        public class ErazerCreditTableItem
        {
            /// <summary>Gets or sets 消込番号</summary>
            /// <value>消込番号</value>
            public string EraserNo { get; set; }
            /// <summary>Gets or sets SEQ番号</summary>
            /// <value>SEQ番号</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets 処理状況</summary>
            /// <value>処理状況</value>
            public int Process { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int Status { get; set; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 入金番号</summary>
            /// <value>入金番号</value>
            public string CreditNo { get; set; }
            /// <summary>Gets or sets 入金行番号</summary>
            /// <value>入金行番号</value>
            public int CreditRowNo { get; set; }
            /// <summary>Gets or sets 連携FLG</summary>
            /// <value>連携FLG</value>
            public int LinkFlg { get; set; }
            /// <summary>Gets or sets 仕訳コード</summary>
            /// <value>仕訳コード</value>
            public string ShiwakeCd { get; set; }
            /// <summary>Gets or sets 仕訳番号</summary>
            /// <value>仕訳番号</value>
            public string ShiwakeNo { get; set; }
            /// <summary>Gets or sets 仕訳番号</summary>
            /// <value>仕訳番号</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 消込日付_転記日付</summary>
            /// <value>消込日付_転記日付</value>
            public DateTime EraserDate { get; set; }
            /// <summary>Gets or sets 発生金額_消込</summary>
            /// <value>発生金額_消込</value>
            public decimal EraserAmount { get; set; }
            /// <summary>Gets or sets エラー判定</summary>
            /// <value>エラー判定</value>
            public int ShiwakeError { get; set; }
        }

        /// <summary>
        /// 支払消込テーブル項目
        /// </summary>
        public class ErazerPaymentTableItem
        {
            /// <summary>Gets or sets 消込番号</summary>
            /// <value>消込番号</value>
            public string EraserNo { get; set; }
            /// <summary>Gets or sets SEQ番号</summary>
            /// <value>SEQ番号</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets 処理状況</summary>
            /// <value>処理状況</value>
            public int Process { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int Status { get; set; }
            /// <summary>Gets or sets 仕入番号</summary>
            /// <value>仕入番号</value>
            public string StockingNo { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentSlipNo { get; set; }
            /// <summary>Gets or sets 支払行番号</summary>
            /// <value>支払行番号</value>
            public int PaymentSlipRowNo { get; set; }
            /// <summary>Gets or sets 連携FLG</summary>
            /// <value>連携FLG</value>
            public int LinkFlg { get; set; }
            /// <summary>Gets or sets 仕訳コード</summary>
            /// <value>仕訳コード</value>
            public string ShiwakeCd { get; set; }
            /// <summary>Gets or sets 仕訳番号</summary>
            /// <value>仕訳番号</value>
            public string ShiwakeNo { get; set; }
            /// <summary>Gets or sets 仕訳番号</summary>
            /// <value>仕訳番号</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 消込日付_転記日付</summary>
            /// <value>消込日付_転記日付</value>
            public DateTime EraserDate { get; set; }
            /// <summary>Gets or sets 発生金額_消込</summary>
            /// <value>発生金額_消込</value>
            public decimal EraserAmount { get; set; }
            /// <summary>Gets or sets エラー判定</summary>
            /// <value>エラー判定</value>
            public int ShiwakeError { get; set; }
        }

        /// <summary>
        /// 相殺テーブル項目
        /// </summary>
        public class OffsetTableItem
        {
            /// <summary>Gets or sets 相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
            /// <summary>Gets or sets SEQ番号</summary>
            /// <value>SEQ番号</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets 処理状況</summary>
            /// <value>処理状況</value>
            public int Process { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int Status { get; set; }
            /// <summary>Gets or sets 連携FLG</summary>
            /// <value>連携FLG</value>
            public int LinkFlg { get; set; }
            /// <summary>Gets or sets 仕訳コード</summary>
            /// <value>仕訳コード</value>
            public string ShiwakeCd { get; set; }
            /// <summary>Gets or sets 仕訳番号</summary>
            /// <value>仕訳番号</value>
            public string ShiwakeNo { get; set; }
            /// <summary>Gets or sets 売上日_勘定年月</summary>
            /// <value>売上日_勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 売上日_転記日付</summary>
            /// <value>売上日_転記日付</value>
            public DateTime OffsetDate { get; set; }
            /// <summary>Gets or sets 借方会計部門コード_相殺</summary>
            /// <value>借方会計部門コード_相殺</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方勘定科目コード_相殺</summary>
            /// <value>借方勘定科目コード_相殺</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方会計部門コード_相殺 </summary>
            /// <value>貸方会計部門コード_相殺</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方勘定科目コード_相殺</summary>
            /// <value>貸方勘定科目コード_相殺</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 得意先/仕入先コード</summary>
            /// <value>得意先/仕入先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal OffsetAmount { get; set; }
            /// <summary>Gets or sets 買掛金、未払金相殺金額</summary>
            /// <value>買掛金、未払金相殺金額</value>
            public decimal PayableOffsetAmount { get; set; }
            /// <summary>Gets or sets 買掛金、未払金相殺金額</summary>
            /// <value>買掛金、未払金相殺金額</value>
            public decimal DepositOffsetAmount { get; set; }
            /// <summary>Gets or sets エラー判定</summary>
            /// <value>エラー判定</value>
            public int ShiwakeError { get; set; }
        }
    }
}
