using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using CommonSTDUtil.CommonDBManager;
using CommonSTDUtil.CommonLogger;

using APConsts = APConstants.APConstants;
using APResources = CommonAPUtil.APCommonUtil.APResources;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ShiDao = CommonAPUtil.APShiwakeSendUtil.ShiwakeSendDataClass;
using StoDao = CommonAPUtil.APStoredFncUtil.APStoredFncUtil;

namespace CommonAPUtil.APShiwakeSendUtil
{
    /// <summary>
    /// 仕訳連携クラス
    /// </summary>
    public class APShiwakeSendUtil
    {
        #region クラス内変数
        /// <summary>ログ出力</summary>
        private static CommonLogger logger = CommonLogger.GetInstance("logger");
        #endregion

        #region 売上

        /// <summary>
        /// 売上承認登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">仕訳送信パラメータ</param>
        /// <returns>true:正常 false：エラー</returns>
        public static bool SalesApprovalRegist(ComDB db, ShiDao.ShiwakeSendParameter paramInfo)
        {
            System.Text.StringBuilder sql;
            int regFlg = 0;     // 更新フラグ

            string kunnr; //得意先コード

            // パラメータチェック（下記項目が未入力の場合）
            // パラメータ.売上番号
            // パラメータ.処理状況
            // パラメータ.ステータス
            if (string.IsNullOrEmpty(paramInfo.SalesNo))
            {
                // エラーログを出力する。
                // 「引数が設定されていません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MB00005));
                return false;
            }

            //仕訳CSV作成用テーブル作成
            try
            {
                //現在の履歴テーブルより、SEQ番号を取得
                sql = new System.Text.StringBuilder();
                sql.AppendLine("select max(seq) as Seq");
                sql.AppendLine("  from external_if.if_shiwake_sales");
                sql.AppendLine(" where sales_no = @SalesNo");

                // SQL実行
                ShiDao.SalesTableItem seqresult = db.GetEntity<ShiDao.SalesTableItem>(
                    sql.ToString(),
                    new
                    {
                        SalesNo = paramInfo.SalesNo
                    });

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    seqresult.Seq += 1;
                }

                //TAXコード取得
                sql = new System.Text.StringBuilder();
                sql.AppendLine("select credit_accounts_cd as CreditTitleCdTax");
                sql.AppendLine("  from shiwake_ptn_master mst1");
                sql.AppendLine(" where mst1.journal_pattern_cd = @SalesShiwakeCd");
                sql.AppendLine("   and mst1.journal_pattern_division = @SalesTaxShiwakeDiv");

                // SQL実行
                ShiDao.SalesTableItem taxCdresult = db.GetEntity<ShiDao.SalesTableItem>(
                    sql.ToString(),
                    new
                    {
                        SalesShiwakeCd = ShiDao.ShiwakeDivision.SalesShiwakeCd,
                        SalesTaxShiwakeDiv = ShiDao.ShiwakeDivision.SalesTaxShiwakeDiv
                    });

                sql = new System.Text.StringBuilder();
                sql.AppendLine(" select credit_accounts_cd as CreditTitleCdTax");
                sql.AppendLine("  from shiwake_ptn_master mst1");
                sql.AppendLine(" where mst1.journal_pattern_cd = @SalesShiwakeCd");
                sql.AppendLine("   and mst1.journal_pattern_division = @SalesTaxRtnShiwakeDiv");

                // SQL実行
                ShiDao.SalesTableItem taxCdRtnresult = db.GetEntity<ShiDao.SalesTableItem>(
                    sql.ToString(),
                    new
                    {
                        SalesShiwakeCd = ShiDao.ShiwakeDivision.SalesShiwakeCd,
                        SalesTaxRtnShiwakeDiv = ShiDao.ShiwakeDivision.SalesTaxRtnShiwakeDiv
                    });

                //売上承認登録
                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select max(proof_seq) as max_proof_seq from proof.sales_header_proof where sales_no = @SalesNo");
                    int proofSeq = db.GetEntity<int>(
                        sql.ToString(),
                        new
                        {
                            SalesNo = paramInfo.SalesNo
                        });

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select sales_date from proof.sales_header_proof where sales_no = @SalesNo and proof_seq = @ProofSeq ");
                    DateTime salesDate = db.GetEntity<DateTime>(
                        sql.ToString(),
                        new
                        {
                            SalesNo = paramInfo.SalesNo,
                            ProofSeq = proofSeq
                        });

                    var param = new { SalesShiwakeCd = ShiDao.ShiwakeDivision.SalesShiwakeCd, SalesTaxShiwakeDiv = ShiDao.ShiwakeDivision.SalesTaxShiwakeDiv, SalesTaxRtnShiwakeDiv = ShiDao.ShiwakeDivision.SalesTaxRtnShiwakeDiv, SalesNo = paramInfo.SalesNo, ValidDate = salesDate, ProofSeq = proofSeq, Category = "SALES", AccountsCd = taxCdresult.CreditTitleCdTax };
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select right(trn1.account_years, 6) as AccountYears");
                    sql.AppendLine("     , trn1.sales_date as SalesDate");
                    sql.AppendLine("     , left(trn1.account_debit_section_cd, 6) as AccountDebitSectionCd");
                    sql.AppendLine("     , left(trn1.debit_title_cd, 5) as DebitTitleCd");
                    sql.AppendLine("     , left(trn1.account_credit_section_cd, 6) as AccountCreditSectionCd");
                    sql.AppendLine("     , left(trn1.credit_title_cd, 5) as CreditTitleCd");
                    sql.AppendLine("     , left(mst1.credit_accounts_cd, 5) as CreditTitleCdTax");
                    sql.AppendLine("     , air.kunnr as Kunner");
                    sql.AppendLine("     , trn1.invoice_cd as InvoiceCd");
                    sql.AppendLine("     , trn2.sales_amount as SalesAmount");
                    sql.AppendLine("     , trn2.tax_amount as TaxAmount");
                    sql.AppendLine("     , mst2.tax_cd as DebitTaxCd");
                    sql.AppendLine("     , mst3.tax_cd as CreditTaxCd");
                    sql.AppendLine("     , mst4.tax_cd as CreditTaxCdTax");
                    sql.AppendLine("     , trn1.sales_no as SalesNo");
                    sql.AppendLine("     , trn1.status as Status");
                    sql.AppendLine("     , trn1.vender_cd as VenderCd");
                    sql.AppendLine("  from proof.sales_header_proof trn1");
                    sql.AppendLine("  left outer join external_if.air_mst_cust_comp_t air");
                    sql.AppendLine("               on bukrs = (select air_bukrs from company)");
                    sql.AppendLine("              and kunnr like '1300%'");
                    sql.AppendLine("              and trn1.vender_cd = right(kunnr, 6)");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst2");
                    sql.AppendLine("               on trn1.debit_title_cd  = mst2.accounts_cd and mst2.lv = 1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst3");
                    sql.AppendLine("               on trn1.credit_title_cd = mst3.accounts_cd and mst3.lv = 1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where accounts_cd = @AccountsCd and valid_date <= @ValidDate and category = @Category) mst4");
                    sql.AppendLine("               on mst4.lv = 1");
                    sql.AppendLine("  ,(select sum(sales_amount) as sales_amount, sum(tax_amount) as tax_amount from sales_detail where sales_no = @SalesNo) trn2");
                    sql.AppendLine("  ,(select * from shiwake_ptn_master where journal_pattern_cd = @SalesShiwakeCd and journal_pattern_division = @SalesTaxShiwakeDiv) mst1");
                    sql.AppendLine(" where trn1.category_division in (1, -2, -3, 4, 5, 9)");
                    sql.AppendLine("   and trn1.sales_no = @SalesNo");
                    sql.AppendLine("   and trn1.proof_seq = @ProofSeq");
                    sql.AppendLine(" union all ");
                    sql.AppendLine(" select right(trn1.account_years, 6) as AccountYears");
                    sql.AppendLine("     , trn1.sales_date as SalesDate");
                    sql.AppendLine("     , left(trn1.account_debit_section_cd, 6) as AccountDebitSectionCd");
                    sql.AppendLine("     , left(trn1.debit_title_cd, 5) as DebitTitleCd");
                    sql.AppendLine("     , left(trn1.account_credit_section_cd, 6) as AccountCreditSectionCd");
                    sql.AppendLine("     , left(trn1.credit_title_cd, 5) as CreditTitleCd");
                    sql.AppendLine("     , left(mst1.credit_accounts_cd, 5) as CreditTitleCdTax");
                    sql.AppendLine("     , air.kunnr as Kunner");
                    sql.AppendLine("     , trn1.invoice_cd as InvoiceCd");
                    sql.AppendLine("     , trn2.sales_amount as SalesAmount");
                    sql.AppendLine("     , trn2.tax_amount as TaxAmount");
                    sql.AppendLine("     , mst2.tax_cd as DebitTaxCd");
                    sql.AppendLine("     , mst3.tax_cd as CreditTaxCd");
                    sql.AppendLine("     , mst4.tax_cd as CreditTaxCdTax");
                    sql.AppendLine("     , trn1.sales_no as SalesNo");
                    sql.AppendLine("     , trn1.status as Status");
                    sql.AppendLine("     , trn1.vender_cd as VenderCd");
                    sql.AppendLine("  from proof.sales_header_proof trn1");
                    sql.AppendLine("  left outer join external_if.air_mst_cust_comp_t air");
                    sql.AppendLine("               on bukrs = (select air_bukrs from company)");
                    sql.AppendLine("              and kunnr like '1300%'");
                    sql.AppendLine("              and trn1.vender_cd = right(kunnr, 6)");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst2");
                    sql.AppendLine("               on trn1.debit_title_cd  = mst2.accounts_cd and mst2.lv = 1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst3");
                    sql.AppendLine("               on trn1.credit_title_cd = mst3.accounts_cd and mst3.lv = 1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where accounts_cd = @AccountsCd and valid_date <= @ValidDate and category = @Category) mst4");
                    sql.AppendLine("               on mst4.lv = 1");
                    sql.AppendLine("  ,(select sum(sales_amount) as sales_amount, sum(tax_amount) as tax_amount from sales_detail where sales_no = @SalesNo) trn2");
                    sql.AppendLine("  ,(select * from shiwake_ptn_master where journal_pattern_cd = @SalesShiwakeCd and journal_pattern_division = @SalesTaxRtnShiwakeDiv) mst1");
                    sql.AppendLine(" where trn1.category_division in (-1, 2, 3, -4, -5, -9)");
                    sql.AppendLine("   and trn1.sales_no = @SalesNo");
                    sql.AppendLine("   and trn1.proof_seq = @ProofSeq");

                    //sql = sql + "select right(trn1.account_years, 4) as account_years";
                    //sql = sql + "     , trn1.sales_date as sales_date";
                    //sql = sql + "     , left(trn1.account_debit_section_cd, 6) as account_debit_section_cd";
                    //sql = sql + "     , left(trn1.debit_title_cd, 5) as debit_title_cd";
                    //sql = sql + "     , left(trn1.account_credit_section_cd, 6) as account_credit_section_cd";
                    //sql = sql + "     , left(trn1.credit_title_cd, 5) as credit_title_cd";
                    //sql = sql + "     , left((select credit_accounts_cd";
                    //sql = sql + "               from shiwake_ptn_master mst1";
                    //sql = sql + "              where mst1.journal_pattern_cd = '" + ShiDao.ShiwakeDivision.SalesShiwakeCd + "'";
                    //sql = sql + "                and mst1.journal_pattern_division = '" + ShiDao.ShiwakeDivision.SalesTaxShiwakeDiv + "'), 5) as credit_title_cd_tax";
                    //sql = sql + "     , air.kunnr as kunner";
                    //sql = sql + "     , trn1.invoice_cd as invoice_cd";
                    //sql = sql + "     , (select sum(sales_amount) ";
                    //sql = sql + "             from sales_detail trn3";
                    //sql = sql + "            where trn3.sales_no = '" + paramInfo.SalesNo + "') as sales_amount";
                    //sql = sql + "     , (select sum(tax_amount) ";
                    //sql = sql + "             from sales_detail trn4";
                    //sql = sql + "            where trn4.sales_no = '" + paramInfo.SalesNo + "') as tax_amount";
                    //sql = sql + "     , (select tax_cd";
                    //sql = sql + "          from accounts_tax mst2";
                    //sql = sql + "         where trn1.debit_title_cd = mst2.accounts_cd) as debit_tax_cd";
                    //sql = sql + "     , (select tax_cd";
                    //sql = sql + "          from accounts_tax mst3";
                    //sql = sql + "         where trn1.credit_title_cd = mst3.accounts_cd) as credit_tax_cd";
                    //sql = sql + "     , (select tax_cd";
                    //sql = sql + "          from accounts_tax mst4";
                    //sql = sql + "         where mst4.accounts_cd = '" + TaxCdresult.credit_title_cd_tax + "'";
                    //sql = sql + "           and category = 'SALES' ) as credit_tax_cd_tax";
                    //sql = sql + "     , trn1.sales_no as sales_no";
                    //sql = sql + "     , trn1.status as status";
                    //sql = sql + "     , trn1.vender_cd as vender_cd";
                    //sql = sql + "  from proof.sales_header_proof trn1";
                    //sql = sql + "  left outer join external_if.air_mst_cust_comp_t air";
                    //sql = sql + "               on bukrs = (select air_bukrs from company)";
                    //sql = sql + "              and kunnr like '1300%'";
                    //sql = sql + "              and trn1.vender_cd = right(kunnr, 6)";
                    //sql = sql + " where trn1.category_division in (1, -2, -3, 4, 5, 9)";
                    //sql = sql + "   and trn1.sales_no ='" + paramInfo.SalesNo + "'";
                    //sql = sql + "   and trn1.proof_seq = (select max(proof_seq) from proof.sales_header_proof trn5 where trn5.sales_no ='" + paramInfo.SalesNo + "')";
                    //sql = sql + "union all ";
                    //sql = sql + "select right(trn1.account_years, 4) as account_years";
                    //sql = sql + "     , trn1.sales_date as sales_date";
                    //sql = sql + "     , left(trn1.account_debit_section_cd, 6) as account_debit_section_cd";
                    //sql = sql + "     , left(trn1.debit_title_cd, 5) as debit_title_cd";
                    //sql = sql + "     , left(trn1.account_credit_section_cd, 6) as account_credit_section_cd";
                    //sql = sql + "     , left(trn1.credit_title_cd, 5) as credit_title_cd";
                    //sql = sql + "     , left((select credit_accounts_cd";
                    //sql = sql + "               from shiwake_ptn_master mst1";
                    //sql = sql + "              where mst1.journal_pattern_cd = '" + ShiDao.ShiwakeDivision.SalesShiwakeCd + "'";
                    //sql = sql + "                and mst1.journal_pattern_division = '" + ShiDao.ShiwakeDivision.SalesTaxRtnShiwakeDiv + "'), 5) as credit_title_cd_tax";
                    //sql = sql + "     , air.kunnr as kunner";
                    //sql = sql + "     , trn1.invoice_cd as invoice_cd";
                    //sql = sql + "     , (select sum(sales_amount) ";
                    //sql = sql + "              from sales_detail trn3";
                    //sql = sql + "             where trn3.sales_no = '" + paramInfo.SalesNo + "') as sales_amount";
                    //sql = sql + "     , (select sum(tax_amount)";
                    //sql = sql + "             from sales_detail trn4";
                    //sql = sql + "            where trn4.sales_no = '" + paramInfo.SalesNo + "') as tax_amount";
                    //sql = sql + "     , (select tax_cd";
                    //sql = sql + "          from accounts_tax mst2";
                    //sql = sql + "         where trn1.debit_title_cd = mst2.accounts_cd) as debit_tax_cd";
                    //sql = sql + "     , (select tax_cd";
                    //sql = sql + "          from accounts_tax mst3";
                    //sql = sql + "         where trn1.credit_title_cd = mst3.accounts_cd) as credit_tax_cd";
                    //sql = sql + "     , (select tax_cd";
                    //sql = sql + "          from accounts_tax mst4";
                    //sql = sql + "         where mst4.accounts_cd = '" + TaxCdresult.credit_title_cd_tax + "'";
                    //sql = sql + "           and category = 'SALES' ) as credit_tax_cd_tax";
                    //sql = sql + "     , trn1.sales_no as sales_no";
                    //sql = sql + "     , trn1.status as status";
                    //sql = sql + "     , trn1.vender_cd as vender_cd";
                    //sql = sql + "  from proof.sales_header_proof trn1";
                    //sql = sql + "  left outer join external_if.air_mst_cust_comp_t air";
                    //sql = sql + "               on bukrs = (select air_bukrs from company)";
                    //sql = sql + "              and kunnr like '1300%'";
                    //sql = sql + "              and trn1.vender_cd = right(kunnr, 6)";
                    //sql = sql + " where trn1.category_division in (-1, 2, 3, -4, -5, -9)";
                    //sql = sql + "   and trn1.sales_no ='" + paramInfo.SalesNo + "'";
                    //sql = sql + "   and trn1.proof_seq = (select max(proof_seq) from proof.sales_header_proof trn5 where trn5.sales_no ='" + paramInfo.SalesNo + "')";
                    // SQL実行
                    IList<ShiDao.SalesTableItem> resultList = db.GetList<ShiDao.SalesTableItem>(sql.ToString(), param);

                    foreach (ShiDao.SalesTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            if (result.Kunner == null || result.Kunner.ToString().Length == 0)
                            {
                                kunnr = "999999";                                              //得意先コード
                            }
                            else
                            {
                                kunnr = result.Kunner.ToString();                         //得意先コード
                            }

                            result.ShiwakeCd = ShiDao.ShiwakeDivision.SalesShiwakeCd;
                            result.ShiwakeNo = ShiDao.ShiwakeDivision.SalesShiwakeDiv;
                            result.ShiwakeRtnNo = ShiDao.ShiwakeDivision.SalesRtnShiwakeDiv;
                            result.ShiwakeTaxNo = ShiDao.ShiwakeDivision.SalesTaxShiwakeDiv;
                            result.ShiwakeTaxRtnNo = ShiDao.ShiwakeDivision.SalesTaxRtnShiwakeDiv;
                            result.Seq = seqresult.Seq;
                            result.Process = paramInfo.Procsess;
                            result.CreditTitleCdTax = taxCdresult.CreditTitleCdTax;
                            result.Kunner = kunnr;

                            // エラー判定
                            result.ShiwakeError = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeNo, result.SalesNo, db);
                            result.ShiwakeErrorRtn = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeRtnNo, result.SalesNo, db);
                            result.ShiwakeErrorTax = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeTaxNo, result.SalesNo, db);
                            result.ShiwakeErrorTaxRtn = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeTaxRtnNo, result.SalesNo, db);

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine(" insert into external_if.if_shiwake_sales ( ");
                            sql.AppendLine(" sales_no,");
                            sql.AppendLine(" seq,");
                            sql.AppendLine(" process,");
                            sql.AppendLine(" status,");
                            sql.AppendLine(" link_flg,");
                            sql.AppendLine(" shiwake_cd,");
                            sql.AppendLine(" shiwake_no,");
                            sql.AppendLine(" shiwake_rtn_no,");
                            sql.AppendLine(" shiwake_tax_no,");
                            sql.AppendLine(" shiwake_tax_rtn_no,");
                            sql.AppendLine(" account_years,");
                            sql.AppendLine(" sales_date,");
                            sql.AppendLine(" account_debit_section_cd,");
                            sql.AppendLine(" debit_title_cd,");
                            sql.AppendLine(" account_credit_section_cd,");
                            sql.AppendLine(" credit_title_cd,");
                            sql.AppendLine(" credit_title_cd_tax,");
                            sql.AppendLine(" kunner,");
                            sql.AppendLine(" invoice_cd,");
                            sql.AppendLine(" sales_amount,");
                            sql.AppendLine(" tax_amount,");
                            sql.AppendLine(" debit_tax_cd,");
                            sql.AppendLine(" credit_tax_cd,");
                            sql.AppendLine(" credit_tax_cd_tax,");
                            sql.AppendLine(" shiwake_error,");
                            sql.AppendLine(" shiwake_error_rtn,");
                            sql.AppendLine(" shiwake_error_tax,");
                            sql.AppendLine(" shiwake_error_tax_rtn");
                            sql.AppendLine(" ) values ( ");
                            sql.AppendLine(" @SalesNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @ShiwakeRtnNo ,");
                            sql.AppendLine(" @ShiwakeTaxNo ,");
                            sql.AppendLine(" @ShiwakeTaxRtnNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @SalesDate ,");
                            sql.AppendLine(" @AccountDebitSectionCd ,");
                            sql.AppendLine(" @DebitTitleCd ,");
                            sql.AppendLine(" @AccountCreditSectionCd ,");
                            sql.AppendLine(" @CreditTitleCd ,");
                            sql.AppendLine(" @CreditTitleCdTax ,");
                            sql.AppendLine(" @Kunner ,");
                            sql.AppendLine(" @InvoiceCd ,");
                            sql.AppendLine(" @SalesAmount ,");
                            sql.AppendLine(" @TaxAmount ,");
                            sql.AppendLine(" @DebitTaxCd ,");
                            sql.AppendLine(" @CreditTaxCd ,");
                            sql.AppendLine(" @CreditTaxCdTax ,");
                            sql.AppendLine(" @ShiwakeError ,");
                            sql.AppendLine(" @ShiwakeErrorRtn ,");
                            sql.AppendLine(" @ShiwakeErrorTax ,");
                            sql.AppendLine(" @ShiwakeErrorTaxRtn ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    SalesNo = result.SalesNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    ShiwakeRtnNo = result.ShiwakeRtnNo,
                                    ShiwakeTaxNo = result.ShiwakeTaxNo,
                                    ShiwakeTaxRtnNo = result.ShiwakeTaxRtnNo,
                                    AccountYears = result.AccountYears,
                                    SalesDate = result.SalesDate,
                                    AccountDebitSectionCd = result.AccountDebitSectionCd,
                                    DebitTitleCd = result.DebitTitleCd,
                                    AccountCreditSectionCd = result.AccountCreditSectionCd,
                                    CreditTitleCd = result.CreditTitleCd,
                                    CreditTitleCdTax = result.CreditTitleCdTax,
                                    Kunner = result.Kunner,
                                    InvoiceCd = result.InvoiceCd,
                                    SalesAmount = result.SalesAmount,
                                    TaxAmount = result.TaxAmount,
                                    DebitTaxCd = result.DebitTaxCd,
                                    CreditTaxCd = result.CreditTaxCd,
                                    CreditTaxCdTax = result.CreditTaxCdTax,
                                    ShiwakeError = result.ShiwakeError,
                                    ShiwakeErrorRtn = result.ShiwakeErrorRtn,
                                    ShiwakeErrorTax = result.ShiwakeErrorTax,
                                    ShiwakeErrorTaxRtn = result.ShiwakeErrorTaxRtn
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }

                //売上承認取消
                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL_CANCEL)
                {
                    //承認取消
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" delete from external_if.if_shiwake_sales ");
                    sql.AppendLine("  where sales_no = @SalesNo ");
                    sql.AppendLine("    and seq = @Seq ");
                    // 更新処理実行
                    regFlg = db.Regist(
                        sql.ToString(),
                        new
                        {
                            SalesNo = paramInfo.SalesNo,
                            Seq = seqresult.Seq
                        });
                    if (regFlg < 0)
                    {
                        // 異常終了
                        return false;
                    }
                }

                //売上取消
                if (paramInfo.Procsess == ShiDao.ProcessDivison.CANCEL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select");
                    sql.AppendLine(" sales_no as SalesNo,");
                    sql.AppendLine(" seq as Seq,");
                    sql.AppendLine(" process as Process,");
                    sql.AppendLine(" status as Status,");
                    sql.AppendLine(" link_flg as LinkFlg,");
                    sql.AppendLine(" shiwake_cd as ShiwakeCd,");
                    sql.AppendLine(" shiwake_no as ShiwakeNo,");
                    sql.AppendLine(" shiwake_rtn_no as ShiwakeRtnNo,");
                    sql.AppendLine(" shiwake_tax_no as ShiwakeTaxNo,");
                    sql.AppendLine(" shiwake_tax_rtn_no as ShiwakeTaxRtnNo,");
                    sql.AppendLine(" account_years as AccountYears,");
                    sql.AppendLine(" sales_date as SalesDate,");
                    sql.AppendLine(" account_debit_section_cd as AccountDebitSectionCd,");
                    sql.AppendLine(" debit_title_cd as DebitTitleCd,");
                    sql.AppendLine(" account_credit_section_cd as AccountCreditSectionCd,");
                    sql.AppendLine(" credit_title_cd as CreditTitleCd,");
                    sql.AppendLine(" credit_title_cd_tax as CreditTitleCdTax,");
                    sql.AppendLine(" kunner as Kunner,");
                    sql.AppendLine(" invoice_cd as Invoice,");
                    sql.AppendLine(" sales_amount as Sales_amount,");
                    sql.AppendLine(" tax_amount as TaxAmount,");
                    sql.AppendLine(" debit_tax_cd as DebitTaxCd,");
                    sql.AppendLine(" credit_tax_cd as CreditTaxCd,");
                    sql.AppendLine(" credit_tax_cd_tax as CreditTaxCdTax,");
                    sql.AppendLine(" shiwake_error as ShiwakeError,");
                    sql.AppendLine(" shiwake_error_rtn as ShiwakeErrorRtn,");
                    sql.AppendLine(" shiwake_error_tax as ShiwakeErrorTax,");
                    sql.AppendLine(" shiwake_error_tax_rtn as ShiwakeErrorTaxRtn");
                    sql.AppendLine("  from external_if.if_shiwake_sales");
                    sql.AppendLine(" where sales_no = @SalesNo ");
                    sql.AppendLine("   and seq = @Seq ");

                    // SQL実行
                    IList<ShiDao.SalesTableItem> resultList = db.GetList<ShiDao.SalesTableItem>(
                        sql.ToString(),
                        new
                        {
                            SalesNo = paramInfo.SalesNo,
                            Seq = seqresult.Seq
                        });

                    foreach (ShiDao.SalesTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            //売上額をマイナス変換
                            result.SalesAmount = -result.SalesAmount;
                            result.TaxAmount = -result.TaxAmount;
                            result.Seq = seqresult.Seq + 1;
                            result.Process = paramInfo.Procsess;
                            result.LinkFlg = 0;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_sales ( ");
                            sql.AppendLine(" sales_no,");
                            sql.AppendLine(" seq,");
                            sql.AppendLine(" process,");
                            sql.AppendLine(" status,");
                            sql.AppendLine(" link_flg,");
                            sql.AppendLine(" shiwake_cd,");
                            sql.AppendLine(" shiwake_no,");
                            sql.AppendLine(" shiwake_rtn_no,");
                            sql.AppendLine(" shiwake_tax_no,");
                            sql.AppendLine(" shiwake_tax_rtn_no,");
                            sql.AppendLine(" account_years,");
                            sql.AppendLine(" sales_date,");
                            sql.AppendLine(" account_debit_section_cd,");
                            sql.AppendLine(" debit_title_cd,");
                            sql.AppendLine(" account_credit_section_cd,");
                            sql.AppendLine(" credit_title_cd,");
                            sql.AppendLine(" credit_title_cd_tax,");
                            sql.AppendLine(" kunner,");
                            sql.AppendLine(" invoice_cd,");
                            sql.AppendLine(" sales_amount,");
                            sql.AppendLine(" tax_amount,");
                            sql.AppendLine(" debit_tax_cd,");
                            sql.AppendLine(" credit_tax_cd,");
                            sql.AppendLine(" credit_tax_cd_tax,");
                            sql.AppendLine(" shiwake_error,");
                            sql.AppendLine(" shiwake_error_rtn,");
                            sql.AppendLine(" shiwake_error_tax,");
                            sql.AppendLine(" shiwake_error_tax_rtn");
                            sql.AppendLine(" ) values  ( ");
                            sql.AppendLine(" @SalesNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @ShiwakeRtnNo ,");
                            sql.AppendLine(" @ShiwakeTaxNo ,");
                            sql.AppendLine(" @ShiwakeTaxRtnNo,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @SalesDate ,");
                            sql.AppendLine(" @AccountDebitSectionCd ,");
                            sql.AppendLine(" @DebitTitleCd ,");
                            sql.AppendLine(" @AccountCreditSectionCd ,");
                            sql.AppendLine(" @CreditTitleCd ,");
                            sql.AppendLine(" @CreditTitleCdTax ,");
                            sql.AppendLine(" @Kunner ,");
                            sql.AppendLine(" @InvoiceCd ,");
                            sql.AppendLine(" @SalesAmount ,");
                            sql.AppendLine(" @TaxAmount ,");
                            sql.AppendLine(" @DebitTaxCd ,");
                            sql.AppendLine(" @CreditTax_Cd ,");
                            sql.AppendLine(" @CreditTaxCdTax ,");
                            sql.AppendLine(" @ShiwakeError,");
                            sql.AppendLine(" @ShiwakeErrorRtn ,");
                            sql.AppendLine(" @ShiwakeErrorTax ,");
                            sql.AppendLine(" @ShiwakeErrorTaxRtn ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    SalesNo = result.SalesNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    ShiwakeRtnNo = result.ShiwakeRtnNo,
                                    ShiwakeTaxNo = result.ShiwakeTaxNo,
                                    ShiwakeTaxRtnNo = result.ShiwakeTaxRtnNo,
                                    AccountYears = result.AccountYears,
                                    SalesDate = result.SalesDate,
                                    AccountDebitSectionCd = result.AccountDebitSectionCd,
                                    DebitTitleCd = result.DebitTitleCd,
                                    AccountCreditSectionCd = result.AccountCreditSectionCd,
                                    CreditTitleCd = result.CreditTitleCd,
                                    CreditTitleCdTax = result.CreditTitleCdTax,
                                    Kunner = result.Kunner,
                                    InvoiceCd = result.InvoiceCd,
                                    SalesAmount = result.SalesAmount,
                                    TaxAmount = result.TaxAmount,
                                    DebitTaxCd = result.DebitTaxCd,
                                    CreditTax_Cd = result.CreditTaxCd,
                                    CreditTaxCdTax = result.CreditTaxCdTax,
                                    ShiwakeError = result.ShiwakeError,
                                    ShiwakeErrorRtn = result.ShiwakeErrorRtn,
                                    ShiwakeErrorTax = result.ShiwakeErrorTax,
                                    ShiwakeErrorTaxRtn = result.ShiwakeErrorTaxRtn
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }

            return true;
        }
        #endregion

        #region 仕入
        /// <summary>
        /// 仕入承認登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">仕訳送信パラメータ</param>
        /// <returns>true:正常 false：エラー</returns>
        public static bool StockingApprovalRegist(ComDB db, ShiDao.ShiwakeSendParameter paramInfo)
        {
            System.Text.StringBuilder sql;
            int regFlg = 0;     // 更新フラグ
            string strStockingDate; //売上日_転記日付
            string paymentScheDate; //受入日_支払い基準日
            DateTime datePaymentScheDate;
            int site; //サイト
            int noteSiteDivision;
            int creditDivision;

            // パラメータチェック（下記項目が未入力の場合）
            // パラメータ.売上番号
            // パラメータ.処理状況
            // パラメータ.ステータス
            if (string.IsNullOrEmpty(paramInfo.StockingNo) || paramInfo.StockingRowNo <= 0)
            {
                // エラーログを出力する。
                // 「引数が設定されていません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MB00005));
                return false;
            }

            //仕訳CSV作成用テーブル作成
            try
            {
                //現在の履歴テーブルより、SEQ番号を取得
                sql = new System.Text.StringBuilder();
                sql.AppendLine("select max(seq) as Seq");
                sql.AppendLine("  from external_if.if_shiwake_stocking");
                sql.AppendLine(" where stocking_no = @StockingNo ");
                sql.AppendLine("   and stocking_row_no = @StockingRowNo ");

                // SQL実行
                ShiDao.StockingTableItem seqresult = db.GetEntity<ShiDao.StockingTableItem>(
                    sql.ToString(),
                    new
                    {
                        StockingNo = paramInfo.StockingNo,
                        StockingRowNo = paramInfo.StockingRowNo
                    });

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    seqresult.Seq += 1;
                }

                //TAXコード取得
                sql = new System.Text.StringBuilder();
                sql.AppendLine("select debit_accounts_cd as DebitTitleCdTax");
                sql.AppendLine("  from shiwake_ptn_master mst1");
                sql.AppendLine(" where mst1.journal_pattern_cd = @StockingShiwakeCd ");
                sql.AppendLine("   and mst1.journal_pattern_division = @StockingTaxShiwakeDiv ");

                // SQL実行
                ShiDao.StockingTableItem taxCdresult = db.GetEntity<ShiDao.StockingTableItem>(
                    sql.ToString(),
                    new
                    {
                        StockingShiwakeCd = ShiDao.ShiwakeDivision.StockingShiwakeCd,
                        StockingTaxShiwakeDiv = ShiDao.ShiwakeDivision.StockingTaxShiwakeDiv
                    });

                sql = new System.Text.StringBuilder();
                sql.AppendLine("select debit_accounts_cd as DebitTitleCdTax");
                sql.AppendLine("  from shiwake_ptn_master mst1");
                sql.AppendLine(" where mst1.journal_pattern_cd = @StockingShiwakeCd ");
                sql.AppendLine("   and mst1.journal_pattern_division = @StockingRtnTaxShiwakeDiv ");

                // SQL実行
                ShiDao.StockingTableItem taxCdRtnresult = db.GetEntity<ShiDao.StockingTableItem>(
                    sql.ToString(),
                    new
                    {
                        StockingShiwakeCd = ShiDao.ShiwakeDivision.StockingShiwakeCd,
                        StockingRtnTaxShiwakeDiv = ShiDao.ShiwakeDivision.StockingRtnTaxShiwakeDiv
                    });

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select max(proof_seq) as max_proof_seq from proof.stocking_proof where stocking_no = @StockingNo and stocking_row_no = @StockingRowNo ");
                    int proofSeq = db.GetEntity<int>(
                        sql.ToString(),
                        new
                        {
                            StockingNo = paramInfo.StockingNo,
                            StockingRowNo = paramInfo.StockingRowNo
                        });

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select stocking_date from proof.stocking_proof where stocking_no = @StockingNo and stocking_row_no = @StockingRowNo and proof_seq = @ProofSeq ");
                    DateTime stockingDate = db.GetEntity<DateTime>(
                        sql.ToString(),
                        new
                        {
                            StockingNo = paramInfo.StockingNo,
                            StockingRowNo = paramInfo.StockingRowNo,
                            ProofSeq =proofSeq
                        });

                    var param = new { StockingShiwakeCd = ShiDao.ShiwakeDivision.StockingShiwakeCd, StockingTaxShiwakeDiv = ShiDao.ShiwakeDivision.StockingTaxShiwakeDiv, StockingRtnTaxShiwakeDiv = ShiDao.ShiwakeDivision.StockingRtnTaxShiwakeDiv, Stocking = paramInfo.StockingNo, StockingRowNo = paramInfo.StockingRowNo, ValidDate = stockingDate, ProofSeq = proofSeq, Category = "STOCKING", AccountsCd = taxCdresult.DebitTitleCdTax };
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("select right(trn1.account_years, 6) as AccountYears");
                    sql.AppendLine("     , trn1.stocking_date as StockingDate");
                    sql.AppendLine("     , left(trn1.account_debit_section_cd, 6) as AccountDebitSectionCd");
                    sql.AppendLine("     , left(trn1.debit_title_cd, 5) as DebitTitleCd");
                    sql.AppendLine("     , left(mst1.debit_accounts_cd, 5) as DebitTitleCdTax");
                    sql.AppendLine("     , left(trn1.account_credit_section_cd, 6) as AccountCreditSectionCd");
                    sql.AppendLine("     , left(trn1.credit_title_cd, 5) as CreditTitleCd");
                    sql.AppendLine("     , trn1.stocking_amount as StockingAmount");
                    sql.AppendLine("     , trn1.tax_amonut as TaxAmonut");
                    sql.AppendLine("     , mst2.tax_cd as DebitTaxCd");
                    sql.AppendLine("     , mst3.tax_cd as DebitTaxCdTax");
                    sql.AppendLine("     , mst4.tax_cd as CreditTaxCd");
                    sql.AppendLine("     , trn1.stocking_no as StockingNo");
                    sql.AppendLine("     , trn1.stocking_row_no as StockingRowNo");
                    sql.AppendLine("     , trn1.stocking_status as Status");
                    sql.AppendLine("     , trn1.vender_cd as VenderCd");
                    sql.AppendLine("  from proof.stocking_proof trn1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst2");
                    sql.AppendLine("               on trn1.debit_title_cd  = mst2.accounts_cd and mst2.lv = 1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where accounts_cd = @AccountsCd and valid_date <= @ValidDate and category = @Category) mst3");
                    sql.AppendLine("               on mst3.lv = 1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst4");
                    sql.AppendLine("               on trn1.credit_title_cd = mst4.accounts_cd and mst4.lv = 1");
                    sql.AppendLine("     , (select * from shiwake_ptn_master mst1 where mst1.journal_pattern_cd = @StockingShiwakeCd and mst1.journal_pattern_division = @StockingTaxShiwakeDiv) mst1");
                    sql.AppendLine(" where trn1.category_division in (1, -2, -3, 4, 9)");
                    sql.AppendLine("   and trn1.stocking_no = @Stocking");
                    sql.AppendLine("   and trn1.stocking_row_no = @StockingRowNo");
                    sql.AppendLine("   and trn1.proof_seq = @ProofSeq");
                    sql.AppendLine("union all ");
                    sql.AppendLine("select right(trn1.account_years, 6) as AccountYears");
                    sql.AppendLine("     , trn1.stocking_date as StockingDate");
                    sql.AppendLine("     , left(trn1.account_debit_section_cd, 6) as AccountDebitSectionCd");
                    sql.AppendLine("     , left(trn1.debit_title_cd, 5) as DebitTitleCd");
                    sql.AppendLine("     , left(mst1.debit_accounts_cd, 5) as DebitTitleCdTax");
                    sql.AppendLine("     , left(trn1.account_credit_section_cd, 6) as AccountCreditSectionCd");
                    sql.AppendLine("     , left(trn1.credit_title_cd, 5) as CreditTitleCd");
                    sql.AppendLine("     , trn1.stocking_amount as StockingAmount");
                    sql.AppendLine("     , trn1.tax_amonut as TaxAmonut");
                    sql.AppendLine("     , mst2.tax_cd as DebitTaxCd");
                    sql.AppendLine("     , mst3.tax_cd as DebitTaxCdTax");
                    sql.AppendLine("     , mst4.tax_cd as CreditTaxCd");
                    sql.AppendLine("     , trn1.stocking_no as StockingNo");
                    sql.AppendLine("     , trn1.stocking_row_no as StockingRowNo");
                    sql.AppendLine("     , trn1.stocking_status as Status");
                    sql.AppendLine("     , trn1.vender_cd as VenderCd");
                    sql.AppendLine("  from proof.stocking_proof trn1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst2");
                    sql.AppendLine("               on trn1.debit_title_cd  = mst2.accounts_cd and mst2.lv = 1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where accounts_cd = @AccountsCd and valid_date <= @ValidDate and category = @Category) mst3");
                    sql.AppendLine("               on mst3.lv = 1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst4");
                    sql.AppendLine("               on trn1.credit_title_cd = mst4.accounts_cd and mst4.lv = 1");
                    sql.AppendLine("     , (select * from shiwake_ptn_master mst1 where mst1.journal_pattern_cd = @StockingShiwakeCd and mst1.journal_pattern_division = @StockingRtnTaxShiwakeDiv) mst1");
                    sql.AppendLine(" where trn1.category_division in (-1, 2, 3, -4, -9)");
                    sql.AppendLine("   and trn1.stocking_no = @Stocking");
                    sql.AppendLine("   and trn1.stocking_row_no = @StockingRowNo");
                    sql.AppendLine("   and trn1.proof_seq = @ProofSeq");

                    //tax修正前
                    //sql.AppendLine("select right(trn1.account_years, 4) as account_years");
                    //sql.AppendLine("     , trn1.stocking_date as stocking_date");
                    //sql.AppendLine("     , left(trn1.account_debit_section_cd, 6) as account_debit_section_cd");
                    //sql.AppendLine("     , left(trn1.debit_title_cd, 5) as debit_title_cd");
                    //sql.AppendLine("     , left((select debit_accounts_cd");
                    //sql.AppendLine("               from shiwake_ptn_master mst1");
                    //sql.AppendLine("              where mst1.journal_pattern_cd = '" + ShiDao.ShiwakeDivision.StockingShiwakeCd + "'");
                    //sql.AppendLine("                and mst1.journal_pattern_division = '" + ShiDao.ShiwakeDivision.StockingTaxShiwakeDiv + "'), 5) as debit_title_cd_tax");
                    //sql.AppendLine("     , left(trn1.account_credit_section_cd, 6) as account_credit_section_cd");
                    //sql.AppendLine("     , left(trn1.credit_title_cd, 5) as credit_title_cd");
                    //sql.AppendLine("     , trn1.stocking_amount as stocking_amount");
                    //sql.AppendLine("     , trn1.tax_amonut as tax_amonut");
                    //sql.AppendLine("     , (select tax_cd,valid_date");
                    //sql.AppendLine("          from accounts_tax mst2");
                    //sql.AppendLine("         where trn1.debit_title_cd = mst2.accounts_cd");
                    //sql.AppendLine("           and category = 'STOCKING'");
                    //sql.AppendLine("           and valid_date <= now()");
                    //sql.AppendLine("           order by valid_date desc");
                    //sql.AppendLine("           offset 0 limit 1) as debit_tax_cd");
                    //sql.AppendLine("     , (select tax_cd,valid_date");
                    //sql.AppendLine("          from accounts_tax mst3");
                    //sql.AppendLine("         where mst3.accounts_cd = '" + TaxCdresult.debit_tax_cd + "'");
                    //sql.AppendLine("           and category = 'STOCKING' ");
                    //sql.AppendLine("           and valid_date <= now()");
                    //sql.AppendLine("           order by valid_date desc");
                    //sql.AppendLine("           offset 0 limit 1) as debit_tax_cd_tax");
                    //sql.AppendLine("     , (select tax_cd");
                    //sql.AppendLine("          from accounts_tax mst4");
                    //sql.AppendLine("         where trn1.credit_title_cd = mst4.accounts_cd");
                    //sql.AppendLine("           and category = 'STOCKING') as credit_tax_cd");
                    //sql.AppendLine("     , trn1.stocking_no as stocking_no");
                    //sql.AppendLine("     , trn1.stocking_row_no as stocking_row_no");
                    //sql.AppendLine("     , trn1.stocking_status as status");
                    //sql.AppendLine("     , trn1.vender_cd as vender_cd");
                    //sql.AppendLine("  from proof.stocking_proof trn1");
                    //sql.AppendLine(" where trn1.category_division in (1, -2, -3, 4, 9)");
                    //sql.AppendLine("   and trn1.stocking_no ='" + paramInfo.StockingNo + "'");
                    //sql.AppendLine("   and trn1.proof_seq = (select max(proof_seq) from proof.stocking_proof trn2 where trn2.stocking_no ='" + paramInfo.StockingNo + "')");

                    //sql.AppendLine("union all ");
                    //sql.AppendLine("select right(trn1.account_years, 4) as account_years");
                    //sql.AppendLine("     , trn1.stocking_date as stocking_date");
                    //sql.AppendLine("     , left(trn1.account_debit_section_cd, 6) as account_debit_section_cd");
                    //sql.AppendLine("     , left(trn1.debit_title_cd, 5) as debit_title_cd");
                    //sql.AppendLine("     , left((select debit_accounts_cd");
                    //sql.AppendLine("               from shiwake_ptn_master mst1");
                    //sql.AppendLine("              where mst1.journal_pattern_cd = '" + ShiDao.ShiwakeDivision.StockingShiwakeCd + "'");
                    //sql.AppendLine("                and mst1.journal_pattern_division = '" + ShiDao.ShiwakeDivision.StockingRtnTaxShiwakeDiv + "'), 5) as debit_title_cd_tax");
                    //sql.AppendLine("     , left(trn1.account_credit_section_cd, 6) as account_credit_section_cd");
                    //sql.AppendLine("     , left(trn1.credit_title_cd, 5) as credit_title_cd");
                    //sql.AppendLine("     , trn1.stocking_amount as sales_amount");
                    //sql.AppendLine("     , trn1.tax_amonut as tax_amonut");
                    //sql.AppendLine("     , (select tax_cd");
                    //sql.AppendLine("          from accounts_tax mst2");
                    //sql.AppendLine("         where trn1.debit_title_cd = mst2.accounts_cd");
                    //sql.AppendLine("           and category = 'STOCKING') as debit_tax_cd");
                    //sql.AppendLine("     , (select tax_cd");
                    //sql.AppendLine("          from accounts_tax mst3");
                    //sql.AppendLine("         where mst3.accounts_cd = '" + TaxCdresult.debit_tax_cd + "'");
                    //sql.AppendLine("           and category = 'STOCKING' ) as debit_tax_cd_tax");
                    //sql.AppendLine("     , (select tax_cd");
                    //sql.AppendLine("          from accounts_tax mst4");
                    //sql.AppendLine("         where trn1.credit_title_cd = mst4.accounts_cd");
                    //sql.AppendLine("           and category = 'STOCKING') as credit_tax_cd");
                    //sql.AppendLine("     , trn1.stocking_no as stocking_no");
                    //sql.AppendLine("     , trn1.stocking_row_no as stocking_row_no");
                    //sql.AppendLine("     , trn1.stocking_status as status");
                    //sql.AppendLine("     , trn1.vender_cd as vender_cd");

                    //sql.AppendLine("  from proof.stocking_proof trn1");

                    //sql.AppendLine(" where trn1.category_division in (-1, 2, 3, -4, -9)");
                    //sql.AppendLine("   and trn1.stocking_no ='" + paramInfo.StockingNo + "'");
                    //sql.AppendLine("   and trn1.proof_seq = (select max(proof_seq) from proof.stocking_proof trn2 where trn2.stocking_no ='" + paramInfo.StockingNo + "')");

                    // SQL実行
                    IList<ShiDao.StockingTableItem> resultList = db.GetList<ShiDao.StockingTableItem>(sql.ToString(), param);

                    foreach (ShiDao.StockingTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            strStockingDate = string.Format("{0:yyyy/MM/dd}", result.StockingDate); // 売上日_転記日付
                            paymentScheDate = StoDao.FunGetPaymentScheDate(result.VenderCd, strStockingDate, result.StockingAmount, db);
                            DateTime.TryParseExact(paymentScheDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out datePaymentScheDate);
                            site = StoDao.FunGetSite("SI", result.VenderCd, result.StockingAmount, datePaymentScheDate, out noteSiteDivision, out creditDivision, db);

                            result.Seq = seqresult.Seq;
                            result.Process = paramInfo.Procsess;
                            result.PaymentScheDate = paymentScheDate;
                            result.DebitTitleCdTax = taxCdresult.DebitTaxCd;
                            result.Site = site;
                            result.ShiwakeCd = ShiDao.ShiwakeDivision.StockingShiwakeCd;
                            result.ShiwakeNo = ShiDao.ShiwakeDivision.StockingShiwakeDiv;
                            result.ShiwakeRtnNo = ShiDao.ShiwakeDivision.StockingRtnShiwakeDiv;
                            result.ShiwakeTaxNo = ShiDao.ShiwakeDivision.StockingTaxShiwakeDiv;
                            result.ShiwakeTaxRtnNo = ShiDao.ShiwakeDivision.StockingRtnTaxShiwakeDiv;

                            // エラー判定
                            result.ShiwakeError = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeNo, result.StockingNo, db);
                            result.ShiwakeErrorRtn = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeRtnNo, result.StockingNo, db);
                            result.ShiwakeErrorTax = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeTaxNo, result.StockingNo, db);
                            result.ShiwakeErrorTaxRtn = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeTaxRtnNo, result.StockingNo, db);

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_stocking");
                            sql.AppendLine("( ");
                            sql.AppendLine("stocking_no,");
                            sql.AppendLine("stocking_row_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("stocking_date,");
                            sql.AppendLine("payment_sche_date,");
                            sql.AppendLine("account_debit_section_cd,");
                            sql.AppendLine("debit_title_cd,");
                            sql.AppendLine("debit_title_cd_tax,");
                            sql.AppendLine("account_credit_section_cd,");
                            sql.AppendLine("credit_title_cd,");
                            sql.AppendLine("vender_cd,");
                            sql.AppendLine("stocking_amount,");
                            sql.AppendLine("tax_amount,");
                            sql.AppendLine("debit_tax_cd,");
                            sql.AppendLine("debit_tax_cd_tax,");
                            sql.AppendLine("credit_tax_cd,");
                            sql.AppendLine("site,");
                            sql.AppendLine("shiwake_error,");
                            sql.AppendLine("shiwake_error_rtn,");
                            sql.AppendLine("shiwake_error_tax,");
                            sql.AppendLine("shiwake_error_tax_rtn");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @StockingNo ,");
                            sql.AppendLine(" @StockingRowNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @StockingDate ,");
                            sql.AppendLine(" @PaymentScheDate ,");
                            sql.AppendLine(" @AccountDebitSectionCd ,");
                            sql.AppendLine(" @DebitTitleCd ,");
                            sql.AppendLine(" @DebitTitleCdTax ,");
                            sql.AppendLine(" @AccountCreditSectionCd ,");
                            sql.AppendLine(" @CreditTitleCd ,");
                            sql.AppendLine(" @VenderCd ,");
                            sql.AppendLine(" @StockingAmount ,");
                            sql.AppendLine(" @TaxAmount ,");
                            sql.AppendLine(" @DebitTaxCd ,");
                            sql.AppendLine(" @DebitTaxCdTax ,");
                            sql.AppendLine(" @CreditTaxCd ,");
                            sql.AppendLine(" @Site ,");
                            sql.AppendLine(" @ShiwakeError ,");
                            sql.AppendLine(" @ShiwakeErrorRtn ,");
                            sql.AppendLine(" @ShiwakeErrorTax ,");
                            sql.AppendLine(" @ShiwakeErrorTaxRtn ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    StockingNo = result.StockingNo,
                                    StockingRowNo = result.StockingRowNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    StockingDate = result.StockingDate,
                                    PaymentScheDate = result.PaymentScheDate,
                                    AccountDebitSectionCd = result.AccountDebitSectionCd,
                                    DebitTitleCd = result.DebitTitleCd,
                                    DebitTitleCdTax =  result.DebitTitleCdTax,
                                    AccountCreditSectionCd = result.AccountCreditSectionCd,
                                    CreditTitleCd = result.CreditTitleCd,
                                    VenderCd = result.VenderCd,
                                    StockingAmount = result.StockingAmount,
                                    TaxAmount = result.TaxAmonut,
                                    DebitTaxCd = result.DebitTaxCd,
                                    DebitTaxCdTax = result.DebitTaxCdTax,
                                    CreditTaxCd = result.CreditTaxCd,
                                    Site = result.Site,
                                    ShiwakeError = result.ShiwakeError,
                                    ShiwakeErrorRtn = result.ShiwakeErrorRtn,
                                    ShiwakeErrorTax = result.ShiwakeErrorTax,
                                    ShiwakeErrorTaxRtn = result.ShiwakeErrorTaxRtn
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL_CANCEL)
                {
                    //承認取消
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("delete from external_if.if_shiwake_stocking ");
                    sql.AppendLine("where stocking_no = @StockingNo ");
                    sql.AppendLine("  and stocking_row_no = @StockingRowNo ");
                    sql.AppendLine("  and      seq = @Seq ");
                    // 更新処理実行
                    regFlg = db.Regist(
                        sql.ToString(),
                        new
                        {
                            StockingNo = paramInfo.StockingNo,
                            StockingRowNo = paramInfo.StockingRowNo,
                            Seq = seqresult.Seq
                        });
                    if (regFlg < 0)
                    {
                        // 異常終了
                        return false;
                    }
                }

                //取消
                if (paramInfo.Procsess == ShiDao.ProcessDivison.CANCEL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select");
                    sql.AppendLine(" stocking_no as StockingNo,");
                    sql.AppendLine(" stocking_row_no as StockingRowNo,");
                    sql.AppendLine(" seq as Seq,");
                    sql.AppendLine(" process as Process,");
                    sql.AppendLine(" status as Status,");
                    sql.AppendLine(" link_flg as LinkFlg,");
                    sql.AppendLine(" shiwake_cd as ShiwakeCd,");
                    sql.AppendLine(" shiwake_no as ShiwakeNo,");
                    sql.AppendLine(" account_years as AccountYears,");
                    sql.AppendLine(" stocking_date as StockingDate,");
                    sql.AppendLine(" payment_sche_date as PaymentSche,");
                    sql.AppendLine(" account_debit_section_cd as AccountDebitSecitonCd,");
                    sql.AppendLine(" debit_title_cd as DebitTitleCd,");
                    sql.AppendLine(" debit_title_cd_tax as DebitTitleCdTax,");
                    sql.AppendLine(" account_credit_section_cd as AccountCreditSectionCd,");
                    sql.AppendLine(" credit_title_cd as CreditTitleCd,");
                    sql.AppendLine(" vender_cd as VenderCd,");
                    sql.AppendLine(" stocking_amount as StockingAmount,");
                    sql.AppendLine(" tax_amount as TaxAmount,");
                    sql.AppendLine(" debit_tax_cd as DebitTaxCd,");
                    sql.AppendLine(" debit_tax_cd_tax as DebitTaxCdTax,");
                    sql.AppendLine(" credit_tax_cd as CreditTaxCd,");
                    sql.AppendLine(" site as Site,");
                    sql.AppendLine(" shiwake_error as ShiwakeError,");
                    sql.AppendLine(" shiwake_error_rtn as ShiwaleError,");
                    sql.AppendLine(" shiwake_error_tax as ShiwakeErrorTax,");
                    sql.AppendLine(" shiwake_error_tax_rtn as ShiwakeErrorTaxRtn");
                    sql.AppendLine("  from external_if.if_shiwake_stocking");
                    sql.AppendLine(" where stocking_no = @StockingNo ");
                    sql.AppendLine("   and  stocking_row_no = @StockingRowNo ");
                    sql.AppendLine("   and      seq = @Seq ");

                    // SQL実行
                    IList<ShiDao.StockingTableItem> resultList = db.GetList<ShiDao.StockingTableItem>(
                        sql.ToString(),
                        new
                        {
                            StockingNo = paramInfo.StockingNo,
                            StockingRowNo = paramInfo.StockingRowNo,
                            Seq = seqresult.Seq
                        });

                    foreach (ShiDao.StockingTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            //仕入額をマイナス変換
                            result.StockingAmount = -result.StockingAmount;
                            result.TaxAmonut = -result.TaxAmonut;
                            result.Seq = seqresult.Seq + 1;
                            result.Process = paramInfo.Procsess;
                            result.LinkFlg = 0;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_stocking");
                            sql.AppendLine("( ");
                            sql.AppendLine("stocking_no,");
                            sql.AppendLine("stocking_row_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("stocking_date,");
                            sql.AppendLine("payment_sche_date,");
                            sql.AppendLine("account_debit_section_cd,");
                            sql.AppendLine("debit_title_cd,");
                            sql.AppendLine("debit_title_cd_tax,");
                            sql.AppendLine("account_credit_section_cd,");
                            sql.AppendLine("credit_title_cd,");
                            sql.AppendLine("vender_cd,");
                            sql.AppendLine("stocking_amount,");
                            sql.AppendLine("tax_amount,");
                            sql.AppendLine("debit_tax_cd,");
                            sql.AppendLine("debit_tax_cd_tax,");
                            sql.AppendLine("credit_tax_cd,");
                            sql.AppendLine("site,");
                            sql.AppendLine("shiwake_error,");
                            sql.AppendLine("shiwake_error_rtn,");
                            sql.AppendLine("shiwake_error_tax,");
                            sql.AppendLine("shiwake_error_tax_rtn");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @StockingNo ,");
                            sql.AppendLine(" @StockingRowNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @StockingDate ,");
                            sql.AppendLine(" @PaymentScheDate ,");
                            sql.AppendLine(" @AccountDebitSectionCd ,");
                            sql.AppendLine(" @DebitTitleCd ,");
                            sql.AppendLine(" @DebitTitleCdTax ,");
                            sql.AppendLine(" @AccountCreditSectionCd ,");
                            sql.AppendLine(" @CreditTitleCd ,");
                            sql.AppendLine(" @VenderCd ,");
                            sql.AppendLine(" @StockingAmount ,");
                            sql.AppendLine(" @TaxAmount ,");
                            sql.AppendLine(" @DebitTaxCd ,");
                            sql.AppendLine(" @DebitTaxCdTax ,");
                            sql.AppendLine(" @CreditTaxCd ,");
                            sql.AppendLine(" @Site ,");
                            sql.AppendLine(" @ShiwakeError ,");
                            sql.AppendLine(" @ShiwakeErrorRtn ,");
                            sql.AppendLine(" @ShiwakeErrorTax ,");
                            sql.AppendLine(" @ShiwakeErrorTaxRtn ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    StockingNo = result.StockingNo,
                                    StockingRowNo = result.StockingRowNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    StockingDate = result.StockingDate,
                                    PaymentScheDate = result.PaymentScheDate,
                                    AccountDebitSectionCd = result.AccountDebitSectionCd,
                                    DebitTitleCd = result.DebitTitleCd,
                                    DebitTitleCdTax = result.DebitTitleCdTax,
                                    AccountCreditSectionCd = result.AccountCreditSectionCd,
                                    CreditTitleCd = result.CreditTitleCd,
                                    VenderCd = result.VenderCd,
                                    StockingAmount = result.StockingAmount,
                                    TaxAmount = result.TaxAmonut,
                                    DebitTaxCd = result.DebitTaxCd,
                                    DebitTaxCdTax = result.DebitTaxCdTax,
                                    CreditTaxCd =  result.CreditTaxCd,
                                    Site = result.Site,
                                    ShiwakeError = result.ShiwakeError,
                                    ShiwakeErrorRtn = result.ShiwakeErrorRtn,
                                    ShiwakeErrorTax = result.ShiwakeErrorTax,
                                    ShiwakeErrorTaxRtn = result.ShiwakeErrorTaxRtn
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

        #region 入金
        /// <summary>
        /// 入金承認登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">仕訳送信パラメータ</param>
        /// <returns>true:正常 false：エラー</returns>
        public static bool CreditApprovalRegist(ComDB db, ShiDao.ShiwakeSendParameter paramInfo)
        {
            System.Text.StringBuilder sql;
            int regFlg = 0;

            string accountYears;
            int shiwakeError; //エラー判定

            // パラメータチェック（下記項目が未入力の場合）
            // パラメータ.売上番号
            // パラメータ.処理状況
            // パラメータ.ステータス
            if (string.IsNullOrEmpty(paramInfo.CreditNo) || paramInfo.CreditRowNo <= 0)
            {
                // エラーログを出力する。
                // 「引数が設定されていません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MB00005));
                return false;
            }

            //仕訳CSV作成用テーブル作成
            try
            {
                //現在の履歴テーブルより、SEQ番号を取得
                sql = new System.Text.StringBuilder();
                sql.AppendLine("select max(seq) as Seq");
                sql.AppendLine("  from external_if.if_shiwake_credit");
                sql.AppendLine(" where credit_no = @CreditNo ");
                sql.AppendLine("   and credit_row_no = @CreditRowNo ");

                // SQL実行
                ShiDao.CreditTableItem seqresult = db.GetEntity<ShiDao.CreditTableItem>(
                    sql.ToString(),
                    new
                    {
                        CreditNo = paramInfo.CreditNo,
                        CreditRowNo = paramInfo.CreditRowNo
                    });

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    seqresult.Seq += 1;
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select max(proof_seq) as max_proof_seq from proof.credit_proof where credit_no = @CreditNo and credit_row_no = @CreditRowNo ");
                    int proofSeq = db.GetEntity<int>(
                        sql.ToString(),
                        new
                        {
                            CreditNo = paramInfo.CreditNo,
                            CreditRowNo = paramInfo.CreditRowNo
                        });

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select credit_date from proof.credit_proof where credit_no = @CreditNo and credit_row_no = @CreditRowNo and proof_seq = @ProofSeq ");
                    DateTime creditDate = db.GetEntity<DateTime>(
                        sql.ToString(),
                        new
                        {
                            CreditNo = paramInfo.CreditNo,
                            CreditRowNo = paramInfo.CreditRowNo,
                            ProofSeq = proofSeq
                        });

                    var param = new { CreditShiwakeCd = ShiDao.ShiwakeDivision.CreditShiwakeCd, CreditShiwakeDiv = ShiDao.ShiwakeDivision.CreditShiwakeDiv, CreditNo = paramInfo.CreditNo, CreditRowNo = paramInfo.CreditRowNo, ValidDate = creditDate, ProofSeq = proofSeq, Category = "CREDIT" };

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("select mst1.journal_pattern_cd as ShiwakeCd");
                    sql.AppendLine("     , mst1.journal_pattern_division as ShiwakeNo");
                    sql.AppendLine("     , trn1.credit_date as CreditDate");
                    sql.AppendLine("     , left(trn1.debit_section_cd, 6) as DebitSectionCd");
                    sql.AppendLine("     , left(trn1.debit_title_cd, 5) as DebitTitleCd");
                    sql.AppendLine("     , left(trn1.credit_section_cd, 6) as CreditSectionCd");
                    sql.AppendLine("     , left(trn1.credit_title_cd, 5) as CreditTitleCd");
                    sql.AppendLine("     , trn1.bank_cd as BankCd");
                    sql.AppendLine("     , trn1.credit_amount as CreditAmount");
                    //sql.AppendLine("     , (select tax_cd");
                    //sql.AppendLine("          from accounts_tax mst2");
                    sql.AppendLine("     , mst2.tax_cd as DebitTaxCd");
                    sql.AppendLine("     , mst3.tax_cd as CreditTaxCd");
                    sql.AppendLine("     , trn1.credit_no as CreditNo");
                    sql.AppendLine("     , trn1.credit_row_no as CreditRowNo");
                    sql.AppendLine("     , trn1.status as Status");
                    sql.AppendLine("     , trn1.vender_cd as VenderCd");
                    sql.AppendLine("  from proof.credit_proof trn1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst2");
                    sql.AppendLine("               on trn1.debit_title_cd  = mst2.accounts_cd and mst2.lv = 1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst3");
                    sql.AppendLine("               on trn1.credit_title_cd  = mst3.accounts_cd and mst2.lv = 1");
                    sql.AppendLine("  ,(select * from shiwake_ptn_master where journal_pattern_cd = @CreditShiwakeCd and journal_pattern_division = @CreditShiwakeDiv) mst1");
                    sql.AppendLine(" where trn1.credit_no = @CreditNo");
                    sql.AppendLine("   and trn1.credit_row_no = @CreditRowNo ");
                    sql.AppendLine("   and trn1.proof_seq = @ProofSeq");

                    // SQL実行
                    IList<ShiDao.CreditTableItem> resultList = db.GetList<ShiDao.CreditTableItem>(sql.ToString(), param);

                    foreach (ShiDao.CreditTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            accountYears = string.Format("{0:yyyyMM}", result.CreditDate);
                            shiwakeError = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeNo, result.CreditNo, db);  // 手形サイト
                            shiwakeError = 0;
                            result.Seq = seqresult.Seq;
                            result.Process = paramInfo.Procsess;
                            result.AccountYears = accountYears;
                            result.ShiwakeError = shiwakeError;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_credit");
                            sql.AppendLine("( ");
                            sql.AppendLine("credit_no,");
                            sql.AppendLine("credit_row_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("credit_date,");
                            sql.AppendLine("debit_section_cd,");
                            sql.AppendLine("debit_title_cd,");
                            sql.AppendLine("credit_section_cd,");
                            sql.AppendLine("credit_title_cd,");
                            sql.AppendLine("bank_cd,");
                            sql.AppendLine("branch_cd,");
                            sql.AppendLine("deposit_type,");
                            sql.AppendLine("deposit_account,");
                            sql.AppendLine("credit_amount,");
                            sql.AppendLine("debit_tax_cd,");
                            sql.AppendLine("credit_tax_cd,");
                            sql.AppendLine("vender_cd,");
                            sql.AppendLine("shiwake_error");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine("@CreditNo ,");
                            sql.AppendLine("@CreditRowNo ,");
                            sql.AppendLine("@Seq ,");
                            sql.AppendLine("@Process ,");
                            sql.AppendLine("@Status ,");
                            sql.AppendLine("@LinkFlg ,");
                            sql.AppendLine("@ShiwakeCd ,");
                            sql.AppendLine("@ShiwakeNo ,");
                            sql.AppendLine("@AccountYears ,");
                            sql.AppendLine("@CreditDate ,");
                            sql.AppendLine("@DebitSectionCd ,");
                            sql.AppendLine("@DebitTitleCd ,");
                            sql.AppendLine("@CreditSectionCd ,");
                            sql.AppendLine("@CreditTitleCd ,");
                            sql.AppendLine("@BankCd ,");
                            sql.AppendLine("@BranchCd ,");
                            sql.AppendLine("@DepositType ,");
                            sql.AppendLine("@DepositAccount ,");
                            sql.AppendLine("@CreditAmount ,");
                            sql.AppendLine("@DebitTaxCd ,");
                            sql.AppendLine("@CreditTaxCd ,");
                            sql.AppendLine("@VenderCd ,");
                            sql.AppendLine("@ShiwakeError");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    CreditNo = result.CreditNo,
                                    CreditRowNo = result.CreditRowNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    CreditDate = result.CreditDate,
                                    DebitSectionCd = result.DebitSectionCd,
                                    DebitTitleCd = result.DebitTitleCd,
                                    CreditSectionCd = result.CreditSectionCd,
                                    CreditTitleCd = result.CreditTitleCd,
                                    BankCd = result.BankCd,
                                    BranchCd = result.BranchCd,
                                    DepositType = result.DepositType,
                                    DepositAccount = result.DepositAccount,
                                    CreditAmount = result.CreditAmount,
                                    DebitTaxCd = result.DebitTaxCd,
                                    CreditTaxCd = result.CreditTaxCd,
                                    VenderCd = result.VenderCd,
                                    ShiwakeError = result.ShiwakeError
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL_CANCEL)
                {

                    //承認取消
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("delete from external_if.if_shiwake_credit ");
                    sql.AppendLine("where credit_no = @CreditNo ");
                    sql.AppendLine("  and credit_row_no = @CreditRowNo ");
                    sql.AppendLine("  and      seq = @Seq ");
                    // 更新処理実行
                    try
                    {
                        regFlg = db.Regist(
                            sql.ToString(),
                            new
                            {
                                CreditNo = paramInfo.CreditNo,
                                CreditRowNo = paramInfo.CreditRowNo,
                                Seq = seqresult.Seq
                            });
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        return false;
                    }
                    if (regFlg < 0)
                    {
                        // 異常終了
                        return false;
                    }
                }

                //取消
                if (paramInfo.Procsess == ShiDao.ProcessDivison.CANCEL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select");
                    sql.AppendLine(" credit_no as CreditNo,");
                    sql.AppendLine(" credit_row_no as CreditRowNo,");
                    sql.AppendLine(" seq as Seq,");
                    sql.AppendLine(" process as Process,");
                    sql.AppendLine(" status as Status,");
                    sql.AppendLine(" link_flg as LinkFlg,");
                    sql.AppendLine(" shiwake_cd as ShiwakeCd,");
                    sql.AppendLine(" shiwake_no as ShiwakeNo,");
                    sql.AppendLine(" account_years as AccountYears,");
                    sql.AppendLine(" credit_date as CreditDate,");
                    sql.AppendLine(" debit_section_cd as DebitSectionCd,");
                    sql.AppendLine(" debit_title_cd as DebitTitleCd,");
                    sql.AppendLine(" credit_section_cd as CreditSectionCd,");
                    sql.AppendLine(" credit_title_cd as CreditTitleCd,");
                    sql.AppendLine(" bank_cd as BankCd,");
                    sql.AppendLine(" branch_cd as BranchCd,");
                    sql.AppendLine(" deposit_type as DepositType,");
                    sql.AppendLine(" deposit_account as DepositAccount,");
                    sql.AppendLine(" credit_amount as CreditAmount,");
                    sql.AppendLine(" debit_tax_cd as DebitTaxCd,");
                    sql.AppendLine(" credit_tax_cd as CreditTaxCd,");
                    sql.AppendLine(" vender_cd as VenderCd,");
                    sql.AppendLine(" shiwake_error as ShiwakeError");
                    sql.AppendLine("  from external_if.if_shiwake_credit");
                    sql.AppendLine(" where credit_no = @CreditNo ");
                    sql.AppendLine("   and credit_row_no = @CreditRowNo ");
                    sql.AppendLine("   and      seq = @Seq ");

                    // SQL実行
                    IList<ShiDao.CreditTableItem> resultList = db.GetList<ShiDao.CreditTableItem>(
                        sql.ToString(),
                        new
                        {
                            CreditNo  = paramInfo.CreditNo,
                            CreditRowNo = paramInfo.CreditRowNo,
                            Seq = seqresult.Seq
                        });

                    foreach (ShiDao.CreditTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            //売上額をマイナス変換
                            result.CreditAmount = -result.CreditAmount;
                            result.Seq = seqresult.Seq + 1;
                            result.Process = paramInfo.Procsess;
                            result.LinkFlg = 0;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_credit");
                            sql.AppendLine("( ");
                            sql.AppendLine("credit_no,");
                            sql.AppendLine("credit_row_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("credit_date,");
                            sql.AppendLine("debit_section_cd,");
                            sql.AppendLine("debit_title_cd,");
                            sql.AppendLine("credit_section_cd,");
                            sql.AppendLine("credit_title_cd,");
                            sql.AppendLine("bank_cd,");
                            sql.AppendLine("branch_cd,");
                            sql.AppendLine("deposit_type,");
                            sql.AppendLine("deposit_account,");
                            sql.AppendLine("credit_amount,");
                            sql.AppendLine("debit_tax_cd,");
                            sql.AppendLine("credit_tax_cd,");
                            sql.AppendLine("vender_cd,");
                            sql.AppendLine("shiwake_error");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @CreditNo ,");
                            sql.AppendLine(" @CreditRowNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @CreditDate ,");
                            sql.AppendLine(" @DebitSectionCd ,");
                            sql.AppendLine(" @DebitTitleCd ,");
                            sql.AppendLine(" @CreditSectionCd ,");
                            sql.AppendLine(" @CreditTitleCd ,");
                            sql.AppendLine(" @BankCd ,");
                            sql.AppendLine(" @BranchCd ,");
                            sql.AppendLine(" @DepositType ,");
                            sql.AppendLine(" @DepositAccount ,");
                            sql.AppendLine(" @CreditAmount ,");
                            sql.AppendLine(" @DebitTaxCd ,");
                            sql.AppendLine(" @CreditTaxCd ,");
                            sql.AppendLine(" @VenderCd ,");
                            sql.AppendLine(" @ShiwakeError ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    CreditNo = result.CreditNo,
                                    CreditRowNo = result.CreditRowNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    CreditDate = result.CreditDate,
                                    DebitSectionCd = result.DebitSectionCd,
                                    DebitTitleCd = result.DebitTitleCd,
                                    CreditSectionCd = result.CreditSectionCd,
                                    CreditTitleCd = result.CreditTitleCd,
                                    BankCd = result.BankCd,
                                    BranchCd = result.BranchCd,
                                    DepositType = result.DepositType,
                                    DepositAccount = result.DepositAccount,
                                    CreditAmount = result.CreditAmount,
                                    DebitTaxCd = result.DebitTaxCd,
                                    CreditTaxCd = result.CreditTaxCd,
                                    VenderCd = result.VenderCd,
                                    ShiwakeError = result.ShiwakeError
                                });

                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

        #region 支払
        /// <summary>
        /// 支払承認登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">仕訳送信パラメータ</param>
        /// <returns>true:正常 false：エラー</returns>
        public static bool PaymentApprovalRegist(ComDB db, ShiDao.ShiwakeSendParameter paramInfo)
        {
            System.Text.StringBuilder sql;
            int regFlg = 0;

            string accountYears;
            int shiwakeError; //エラー判定

            // パラメータチェック（下記項目が未入力の場合）
            // パラメータ.売上番号
            // パラメータ.処理状況
            // パラメータ.ステータス
            if (string.IsNullOrEmpty(paramInfo.PaymentSlipNo) || paramInfo.PaymentSlipRowNo <= 0)
            {
                // エラーログを出力する。
                // 「引数が設定されていません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MB00005));
                return false;
            }

            //仕訳CSV作成用テーブル作成
            try
            {
                //現在の履歴テーブルより、SEQ番号を取得
                sql = new System.Text.StringBuilder();
                sql.AppendLine("select max(seq) as Seq");
                sql.AppendLine("  from external_if.if_shiwake_payment");
                sql.AppendLine(" where payment_slip_no = @PaymentSlipNo ");
                sql.AppendLine("   and payment_slip_row_no = @PaymentSlipRowNo ");

                // PaymentTableItem
                ShiDao.PaymentTableItem seqresult = db.GetEntity<ShiDao.PaymentTableItem>(
                    sql.ToString(),
                    new
                    {
                        PaymentSlipNo = paramInfo.PaymentSlipNo,
                        PaymentSlipRowNo = paramInfo.PaymentSlipRowNo
                    });

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    seqresult.Seq += 1;
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select max(proof_seq) as max_proof_seq from proof.payment_proof where payment_slip_no = @PaymentSlipNo and payment_slip_row_no = @PaymentSlipRowNo ");
                    int proofSeq = db.GetEntity<int>(
                        sql.ToString(),
                        new
                        {
                            PaymentSlipNo = paramInfo.PaymentSlipNo,
                            PaymentSlipRowNo = paramInfo.PaymentSlipRowNo
                        });

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select payment_date from proof.payment_proof where payment_slip_no = @PaymentSlipNo and proof_seq = @ProofSeq and payment_slip_row_no = @PaymentSlipRowNo ");
                    DateTime paymentDate = db.GetEntity<DateTime>(
                        sql.ToString(),
                        new
                        {
                            PaymentSlipNo = paramInfo.PaymentSlipNo,
                            PaymentSlipRowNo = paramInfo.PaymentSlipRowNo,
                            ProofSeq = proofSeq
                        });

                    var param = new { PaymentShiwakeCd = ShiDao.ShiwakeDivision.PaymentShiwakeCd, PaymentShiwakeDiv = ShiDao.ShiwakeDivision.PaymentShiwakeDiv, PaymentSlipNo = paramInfo.PaymentSlipNo, PaymentSlipRowNo = paramInfo.PaymentSlipRowNo, ValidDate = paymentDate, ProofSeq = proofSeq, Category = "PAYMENT" };

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("select mst1.journal_pattern_cd as ShiwakeCd");
                    sql.AppendLine("     , mst1.journal_pattern_division as ShiwakeNo");
                    sql.AppendLine("     , trn1.payment_date as PaymentDate");
                    sql.AppendLine("     , left(trn1.debit_section_cd, 6) as DebitSectionCd");
                    sql.AppendLine("     , left(trn1.debit_title_cd, 5) as DebitAccountsCd");
                    sql.AppendLine("     , left(trn1.credit_section_cd, 6) as CreditSectionCd");
                    sql.AppendLine("     , left(trn1.credit_title_cd, 5) as CreditAccountsCd");
                    sql.AppendLine("     , trn1.bank_cd as BankCd");
                    sql.AppendLine("     , trn1.payment_amount as PaymentAmount");
                    sql.AppendLine("     , mst2.tax_cd as DebitTaxCd");
                    sql.AppendLine("     , mst3.tax_cd as CreditTaxCd");
                    sql.AppendLine("     , trn1.payment_slip_no as PaymentSlipNo");
                    sql.AppendLine("     , trn1.payment_slip_row_no as PaymentSlipRowNo");
                    sql.AppendLine("     , trn1.status as Status");
                    sql.AppendLine("     , trn1.supplier_cd as SupplierCd");

                    sql.AppendLine("     , trn1.branch_cd as BranchCd");
                    sql.AppendLine("     , trn1.deposit_type as DepositType");
                    sql.AppendLine("     , trn1.deposit_account as DepositAccount");

                    sql.AppendLine("  from proof.payment_proof trn1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst2");
                    sql.AppendLine("               on trn1.debit_title_cd  = mst2.accounts_cd and mst2.lv = 1");
                    sql.AppendLine("  left join (select *, rank() over (partition by accounts_cd order by valid_date) as lv from accounts_tax where valid_date <= @ValidDate and category = @Category) mst3");
                    sql.AppendLine("               on trn1.credit_title_cd  = mst3.accounts_cd and mst2.lv = 1");
                    sql.AppendLine("     ,(select * from shiwake_ptn_master where journal_pattern_cd = @PaymentShiwakeCd and journal_pattern_division = @PaymentShiwakeDiv) mst1");
                    sql.AppendLine(" where trn1.payment_slip_no = @PaymentSlipNo");
                    sql.AppendLine("   and trn1.payment_slip_row_no = @PaymentSlipRowNo ");
                    sql.AppendLine("   and trn1.proof_seq = @ProofSeq");

                    // SQL実行
                    IList<ShiDao.PaymentTableItem> resultList = db.GetList<ShiDao.PaymentTableItem>(sql.ToString(), param);

                    foreach (ShiDao.PaymentTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            //IDictionary<string, object> dicResult = result as IDictionary<string, object>;

                            accountYears = string.Format("{0:yyyyMM}", result.PaymentDate);
                            shiwakeError = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeNo, result.PaymentSlipNo, db);  // 手形サイト
                            shiwakeError = 0;
                            result.Seq = seqresult.Seq;
                            result.Process = paramInfo.Procsess;
                            result.AccountYears = accountYears;
                            result.ShiwakeError = shiwakeError;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_payment");
                            sql.AppendLine("( ");
                            sql.AppendLine("payment_slip_no,");
                            sql.AppendLine("payment_slip_row_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("payment_date,");
                            sql.AppendLine("debit_section_cd,");
                            sql.AppendLine("debit_accounts_cd,");
                            sql.AppendLine("credit_section_cd,");
                            sql.AppendLine("credit_accounts_cd,");
                            sql.AppendLine("bank_cd,");

                            sql.AppendLine("branch_cd,");
                            sql.AppendLine("deposit_type,");
                            sql.AppendLine("deposit_account,");

                            sql.AppendLine("payment_amount,");
                            sql.AppendLine("debit_tax_cd,");
                            sql.AppendLine("credit_tax_cd,");
                            sql.AppendLine("supplier_cd,");
                            sql.AppendLine("shiwake_error");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @PaymentSlipNo ,");
                            sql.AppendLine(" @PaymentSlipRowNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @PaymentDate ,");
                            sql.AppendLine(" @DebitSectionCd ,");
                            sql.AppendLine(" @DebitAccountsCd ,");
                            sql.AppendLine(" @CreditSectionCd ,");
                            sql.AppendLine(" @CreditAccountsCd ,");
                            sql.AppendLine(" @BankCd ,");

                            sql.AppendLine(" @BranchCd ,");
                            sql.AppendLine(" @DepositType ,");
                            sql.AppendLine(" @DepositAccount ,");

                            sql.AppendLine(" @PaymentAmount ,");
                            sql.AppendLine(" @DebitTaxCd ,");
                            sql.AppendLine(" @CreditTaxCd ,");
                            sql.AppendLine(" @SupplierCd ,");
                            sql.AppendLine(" @ShiwakeError");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    PaymentSlipNo = result.PaymentSlipNo,
                                    PaymentSlipRowNo = result.PaymentSlipRowNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    PaymentDate = result.PaymentDate,
                                    DebitSectionCd = result.DebitSectionCd,
                                    DebitAccountsCd = result.DebitAccountsCd,
                                    CreditSectionCd = result.CreditSectionCd,
                                    CreditAccountsCd = result.CreditAccountsCd,
                                    BankCd = result.BankCd,

                                    BranchCd = result.BranchCd,
                                    DepositType = result.DepositType,
                                    DepositAccount = result.DepositAccount,

                                    PaymentAmount = result.PaymentAmount,
                                    DebitTaxCd = result.DebitTaxCd,
                                    CreditTaxCd = result.CreditTaxCd,
                                    SupplierCd = result.SupplierCd,
                                    ShiwakeError = result.ShiwakeError
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL_CANCEL)
                {
                    //承認取消
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("delete from external_if.if_shiwake_payment ");
                    sql.AppendLine("where payment_slip_no = @PaymentSlipNo ");
                    sql.AppendLine("  and payment_slip_row_no = @PaymentSlipRowNo ");
                    sql.AppendLine("  and             seq = @Seq");
                    // 更新処理実行
                    regFlg = db.Regist(
                        sql.ToString(),
                        new
                        {
                            PaymentSlipNo = paramInfo.PaymentSlipNo,
                            PaymentSlipRowNo = paramInfo.PaymentSlipRowNo,
                            Seq = seqresult.Seq
                        });

                    if (regFlg < 0)
                    {
                        // 異常終了
                        return false;
                    }
                }

                //取消
                if (paramInfo.Procsess == ShiDao.ProcessDivison.CANCEL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select");
                    sql.AppendLine(" payment_slip_no as PaymentSlipNo,");
                    sql.AppendLine(" payment_slip_row_no as PayMentSlipRowNo,");
                    sql.AppendLine(" seq as Seq,");
                    sql.AppendLine(" process as Process,");
                    sql.AppendLine(" status as Status,");
                    sql.AppendLine(" link_flg as LinkFlg,");
                    sql.AppendLine(" shiwake_cd as ShiwakeCd,");
                    sql.AppendLine(" shiwake_no as ShiwakeNo,");
                    sql.AppendLine(" account_years as AccountYears,");
                    sql.AppendLine(" payment_date as PayMentDate,");
                    sql.AppendLine(" debit_section_cd as DebitSectionCd,");
                    sql.AppendLine(" debit_accounts_cd as DebitAccountsCd,");
                    sql.AppendLine(" credit_section_cd as CreditSectionCd,");
                    sql.AppendLine(" credit_accounts_cd as Credit,");
                    sql.AppendLine(" bank_cd as BankCd,");
                    sql.AppendLine(" branch_cd as BranchCd,");
                    sql.AppendLine(" deposit_type as DepositType,");
                    sql.AppendLine(" deposit_account as DepositAccount,");
                    sql.AppendLine(" payment_amount as PaymentAmount,");
                    sql.AppendLine(" debit_tax_cd as DebitTaxCd,");
                    sql.AppendLine(" credit_tax_cd as CreditTaxCd,");
                    sql.AppendLine(" supplier_cd as SupplierCd,");
                    sql.AppendLine(" shiwake_error as ShiwakeError");
                    sql.AppendLine("  from external_if.if_shiwake_payment");
                    sql.AppendLine(" where payment_slip_no = @PaymentSlipNo ");
                    sql.AppendLine("  and payment_slip_row_no = @PaymentSlipRowNo ");
                    sql.AppendLine("  and      seq = @Seq ");

                    // SQL実行
                    IList<ShiDao.PaymentTableItem> resultList = db.GetList<ShiDao.PaymentTableItem>(
                        sql.ToString(),
                        new
                        {
                            PaymentSlipNo = paramInfo.PaymentSlipNo,
                            PaymentSlipRowNo = paramInfo.PaymentSlipRowNo,
                            Seq = seqresult.Seq
                        });

                    foreach (ShiDao.PaymentTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            //売上額をマイナス変換
                            result.PaymentAmount = -result.PaymentAmount;
                            result.Seq = seqresult.Seq + 1;
                            result.Process = paramInfo.Procsess;
                            result.LinkFlg = 0;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_payment");
                            sql.AppendLine("( ");
                            sql.AppendLine("payment_slip_no,");
                            sql.AppendLine("payment_slip_row_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("payment_date,");
                            sql.AppendLine("debit_section_cd,");
                            sql.AppendLine("debit_accounts_cd,");
                            sql.AppendLine("credit_section_cd,");
                            sql.AppendLine("credit_accounts_cd,");
                            sql.AppendLine("bank_cd,");
                            sql.AppendLine("branch_cd,");
                            sql.AppendLine("deposit_type,");
                            sql.AppendLine("deposit_account,");
                            sql.AppendLine("payment_amount,");
                            sql.AppendLine("debit_tax_cd,");
                            sql.AppendLine("credit_tax_cd,");
                            sql.AppendLine("supplier_cd,");
                            sql.AppendLine("shiwake_error");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @PaymentSlipNo ,");
                            sql.AppendLine(" @PaymentSlipRowNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @PaymentDate ,");
                            sql.AppendLine(" @DebitSectionCd ,");
                            sql.AppendLine(" @DebitAccountsCd ,");
                            sql.AppendLine(" @CreditSectionCd ,");
                            sql.AppendLine(" @CreditAccountsCd ,");
                            sql.AppendLine(" @BankCd ,");
                            sql.AppendLine(" @BranchCd ,");
                            sql.AppendLine(" @DepositType ,");
                            sql.AppendLine(" @DepositAccount ,");
                            sql.AppendLine(" @PaymentAmount ,");
                            sql.AppendLine(" @DebitTaxCd ,");
                            sql.AppendLine(" @CreditTaxCd ,");
                            sql.AppendLine(" @SupplierCd ,");
                            sql.AppendLine(" @ShiwakeError ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    PaymentSlipNo = result.PaymentSlipNo,
                                    PaymentSlipRowNo = result.PaymentSlipRowNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    PaymentDate = result.PaymentDate,
                                    DebitSectionCd = result.DebitSectionCd,
                                    DebitAccountsCd = result.DebitAccountsCd,
                                    CreditSectionCd = result.CreditSectionCd,
                                    CreditAccountsCd = result.CreditAccountsCd,
                                    BankCd = result.BankCd,
                                    BranchCd = result.BranchCd,
                                    DepositType = result.DepositType,
                                    DepositAccount = result.DepositAccount,
                                    PaymentAmount = result.PaymentAmount,
                                    DebitTaxCd = result.DebitTaxCd,
                                    CreditTaxCd = result.CreditTaxCd,
                                    SupplierCd = result.SupplierCd,
                                    ShiwakeError = result.ShiwakeError
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

        #region 入金消込
        /// <summary>
        /// 入金消込承認登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">仕訳送信パラメータ</param>
        /// <returns>true:正常 false：エラー</returns>
        public static bool ErazerDetailCreditApprovalRegist(ComDB db, ShiDao.ShiwakeSendParameter paramInfo)
        {
            System.Text.StringBuilder sql;
            int regFlg = 0;
            int shiwakeError;

            string accountYears;

            // パラメータチェック（下記項目が未入力の場合）
            // パラメータ.売上番号
            // パラメータ.処理状況
            // パラメータ.ステータス
            if (string.IsNullOrEmpty(paramInfo.EraserNo))
            {
                // エラーログを出力する。
                // 「引数が設定されていません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MB00005));
                return false;
            }

            //仕訳CSV作成用テーブル作成
            try
            {
                //現在の履歴テーブルより、SEQ番号を取得
                sql = new System.Text.StringBuilder();
                sql.AppendLine("select max(seq) as Seq");
                sql.AppendLine("  from external_if.if_shiwake_eraser_detail_credit");
                sql.AppendLine(" where eraser_no = @EraserNo ");

                // PaymentTableItem
                ShiDao.ErazerCreditTableItem seqresult = db.GetEntity<ShiDao.ErazerCreditTableItem>(
                    sql.ToString(),
                    new
                    {
                        EraserNo = paramInfo.EraserNo
                    });

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    seqresult.Seq += 1;
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select max(proof_seq) as max_proof_seq from proof.eraser_detail_credit_proof where eraser_no = @EraserNo");
                    int proofSeq = db.GetEntity<int>(
                        sql.ToString(),
                        new
                        {
                            EraserNo = paramInfo.EraserNo,
                        });

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select eraser_date from proof.eraser_detail_credit_proof where eraser_no = @EraserNo and proof_seq = @ProofSeq");
                    DateTime erazerDate = db.GetEntity<DateTime>(
                        sql.ToString(),
                        new
                        {
                            EraserNo = paramInfo.EraserNo,
                            ProofSeq = proofSeq
                        });

                    var param = new { CreditErazerShiwakeCd = ShiDao.ShiwakeDivision.CreditErazerShiwakeCd, CreditErazerShiwakeDiv = ShiDao.ShiwakeDivision.CreditErazerShiwakeDiv, EraserNo = paramInfo.EraserNo, ValidDate = erazerDate, ProofSeq = proofSeq };

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("select mst1.journal_pattern_cd as ShiwakeCd");
                    sql.AppendLine("     , mst1.journal_pattern_division as ShiwakeNo");
                    sql.AppendLine("     , trn1.eraser_date as EraserDate");
                    sql.AppendLine("     , trn1.eraser_amount as EraserAmount");
                    sql.AppendLine("     , trn1.eraser_no as EraserNo");
                    sql.AppendLine("     , trn1.status as Status");
                    sql.AppendLine("     , trn1.sales_no as SalesNo");
                    sql.AppendLine("     , trn1.credit_no as CreditNo");
                    sql.AppendLine("     , trn1.credit_row_no as CreditRowNo");
                    sql.AppendLine("  from proof.eraser_detail_credit_proof trn1");
                    sql.AppendLine("     ,(select * from shiwake_ptn_master where journal_pattern_cd = @CreditErazerShiwakeCd and journal_pattern_division = @CreditErazerShiwakeDiv) mst1");
                    sql.AppendLine(" where trn1.eraser_no = @EraserNo");
                    sql.AppendLine("   and trn1.proof_seq = @ProofSeq");

                    // SQL実行
                    IList<ShiDao.ErazerCreditTableItem> resultList = db.GetList<ShiDao.ErazerCreditTableItem>(sql.ToString(), param);

                    foreach (ShiDao.ErazerCreditTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            accountYears = string.Format("{0:yyyyMM}", result.EraserDate);
                            shiwakeError = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeNo, result.CreditNo, db);  // 手形サイト
                            shiwakeError = 0;
                            result.Seq = seqresult.Seq;
                            result.Process = paramInfo.Procsess;
                            result.AccountYears = accountYears;
                            result.ShiwakeError = shiwakeError;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_eraser_detail_credit");
                            sql.AppendLine("( ");
                            sql.AppendLine("eraser_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("sales_no,");
                            sql.AppendLine("credit_no,");
                            sql.AppendLine("credit_row_no,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("eraser_date,");
                            sql.AppendLine("eraser_amount,");
                            sql.AppendLine("shiwake_error");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @EraserNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @SalesNo ,");
                            sql.AppendLine(" @CreditNo ,");
                            sql.AppendLine(" @CreditRowNo ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @EraserDate ,");
                            sql.AppendLine(" @EraserAmount ,");
                            sql.AppendLine(" @ShiwakeError ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    EraserNo = result.EraserNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    SalesNo = result.SalesNo,
                                    CreditNo = result.CreditNo,
                                    CreditRowNo = result.CreditRowNo,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    EraserDate = result.EraserDate,
                                    EraserAmount = result.EraserAmount,
                                    ShiwakeError = result.ShiwakeError
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL_CANCEL)
                {
                    //承認取消
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("delete from external_if.if_shiwake_eraser_detail_credit ");
                    sql.AppendLine(" where eraser_no = @EraserNo ");
                    sql.AppendLine("   and       seq = @Seq ");
                    // 更新処理実行
                    regFlg = db.Regist(
                        sql.ToString(),
                        new
                        {
                            EraserNo = paramInfo.EraserNo,
                            Seq = seqresult.Seq
                        });
                    if (regFlg < 0)
                    {
                        // 異常終了
                        return false;
                    }
                }

                //取消
                if (paramInfo.Procsess == ShiDao.ProcessDivison.CANCEL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select");
                    sql.AppendLine(" eraser_no as EraserNo,");
                    sql.AppendLine(" seq as Seq,");
                    sql.AppendLine(" process as Process,");
                    sql.AppendLine(" status as Status,");
                    sql.AppendLine(" sales_no as SalesNo,");
                    sql.AppendLine(" credit_no as CreditNo,");
                    sql.AppendLine(" credit_row_no as CreditRowNo,");
                    sql.AppendLine(" link_flg as LinkFlg,");
                    sql.AppendLine(" shiwake_cd as ShiwakeCd,");
                    sql.AppendLine(" shiwake_no as ShiwakeNo,");
                    sql.AppendLine(" account_years as AccountYears,");
                    sql.AppendLine(" eraser_date as EraserDate,");
                    sql.AppendLine(" eraser_amount as EraserAmount,");
                    sql.AppendLine(" shiwake_error as ShiwakeError");
                    sql.AppendLine("  from external_if.if_shiwake_eraser_detail_credit");
                    sql.AppendLine(" where eraser_no = @EraserNo ");
                    sql.AppendLine("   and      seq = @Seq ");

                    // SQL実行
                    IList<ShiDao.ErazerCreditTableItem> resultList = db.GetList<ShiDao.ErazerCreditTableItem>(
                        sql.ToString(),
                        new
                        {
                            EraserNo = paramInfo.EraserNo,
                            Seq = seqresult.Seq
                        });

                    foreach (ShiDao.ErazerCreditTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            //売上額をマイナス変換
                            result.EraserAmount = -result.EraserAmount;
                            result.Seq = seqresult.Seq + 1;
                            result.Process = paramInfo.Procsess;
                            result.LinkFlg = 0;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_eraser_detail_credit");
                            sql.AppendLine("( ");
                            sql.AppendLine("eraser_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("sales_no,");
                            sql.AppendLine("credit_no,");
                            sql.AppendLine("credit_row_no,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("eraser_date,");
                            sql.AppendLine("eraser_amount,");
                            sql.AppendLine("shiwake_error");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @EraserNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @SalesNo ,");
                            sql.AppendLine(" @CreditNo ,");
                            sql.AppendLine(" @CreditRowNo ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @EraserDate ,");
                            sql.AppendLine(" @EraserAmount ,");
                            sql.AppendLine(" @ShiwakeError ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                   EraserNo = result.EraserNo,
                                   Seq = result.Seq,
                                   Process = result.Process,
                                   Status = result.Status,
                                   SalesNo = result.SalesNo,
                                   CreditNo = result.CreditNo,
                                   CreditRowNo = result.CreditRowNo,
                                   LinkFlg = result.LinkFlg,
                                   ShiwakeCd = result.ShiwakeCd,
                                   ShiwakeNo = result.ShiwakeNo,
                                   AccountYears = result.AccountYears,
                                   EraserDate = result.EraserDate,
                                   EraserAmount = result.EraserAmount,
                                   ShiwakeError = result.ShiwakeError
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

        #region 支払消込
        /// <summary>
        /// 支払消込承認登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">仕訳送信パラメータ</param>
        /// <returns>true:正常 false：エラー</returns>
        public static bool ErazerDetailPaymentApprovalRegist(ComDB db, ShiDao.ShiwakeSendParameter paramInfo)
        {
            System.Text.StringBuilder sql;
            int regFlg = 0;
            int shiwakeError;

            string accountYears;

            // パラメータチェック（下記項目が未入力の場合）
            // パラメータ.売上番号
            // パラメータ.処理状況
            // パラメータ.ステータス
            if (string.IsNullOrEmpty(paramInfo.EraserNo))
            {
                // エラーログを出力する。
                // 「引数が設定されていません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MB00005));
                return false;
            }

            //仕訳CSV作成用テーブル作成
            try
            {
                //現在の履歴テーブルより、SEQ番号を取得
                sql = new System.Text.StringBuilder();
                sql.AppendLine("select max(seq) as Seq");
                sql.AppendLine("  from external_if.if_shiwake_eraser_detail_payment");
                sql.AppendLine(" where eraser_no = @EraserNo ");

                // PaymentTableItem
                ShiDao.ErazerPaymentTableItem seqresult = db.GetEntity<ShiDao.ErazerPaymentTableItem>(
                    sql.ToString(),
                    new
                    {
                        EraserNo = paramInfo.EraserNo
                    });

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    seqresult.Seq += 1;
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select max(proof_seq) as max_proof_seq from proof.eraser_detail_payment_proof where eraser_no = @EraserNo");
                    int proofSeq = db.GetEntity<int>(
                        sql.ToString(),
                        new
                        {
                            EraserNo = paramInfo.EraserNo,
                        });

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select eraser_date from proof.eraser_detail_payment_proof where eraser_no = @EraserNo and proof_seq = @ProofSeq ");
                    DateTime erazerDate = db.GetEntity<DateTime>(
                        sql.ToString(),
                        new
                        {
                            EraserNo = paramInfo.EraserNo,
                            ProofSeq = proofSeq
                        });

                    var param = new { PaymentErazerShiwakeCd = ShiDao.ShiwakeDivision.PaymentErazerShiwakeCd, PaymentErazerShiwakeDiv = ShiDao.ShiwakeDivision.PaymentErazerShiwakeDiv, EraserNo = paramInfo.EraserNo, ValidDate = erazerDate, ProofSeq = proofSeq };

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("select mst1.journal_pattern_cd as ShiwakeCd");
                    sql.AppendLine("     , mst1.journal_pattern_division as ShiwakeNo");
                    sql.AppendLine("     , trn1.eraser_date as EraserDate");
                    sql.AppendLine("     , trn1.eraser_amount as EraserAmount");
                    sql.AppendLine("     , trn1.eraser_no as EraserNo");
                    sql.AppendLine("     , trn1.status as Status");
                    sql.AppendLine("     , trn1.stocking_no as StockingNo");
                    sql.AppendLine("     , trn1.payment_slip_no as PaymentSlipNo");
                    sql.AppendLine("     , trn1.payment_slip_row_no as PaymentSlipRowNo");
                    sql.AppendLine("  from proof.eraser_detail_payment_proof trn1");
                    sql.AppendLine("     ,(select * from shiwake_ptn_master where journal_pattern_cd = @PaymentErazerShiwakeCd and journal_pattern_division = @PaymentErazerShiwakeDiv) mst1");
                    sql.AppendLine(" where trn1.eraser_no = @EraserNo ");
                    sql.AppendLine("   and trn1.proof_seq = @ProofSeq");

                    // SQL実行
                    IList<ShiDao.ErazerPaymentTableItem> resultList = db.GetList<ShiDao.ErazerPaymentTableItem>(sql.ToString(), param);

                    foreach (ShiDao.ErazerPaymentTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            //IDictionary<string, object> dicResult = result as IDictionary<string, object>;

                            accountYears = string.Format("{0:yyyyMM}", result.EraserDate);
                            shiwakeError = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeNo, result.PaymentSlipNo, db);  // 手形サイト
                            shiwakeError = 0;
                            result.Seq = seqresult.Seq;
                            result.Process = paramInfo.Procsess;
                            result.AccountYears = accountYears;
                            result.ShiwakeError = shiwakeError;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_eraser_detail_payment");
                            sql.AppendLine("( ");
                            sql.AppendLine("eraser_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("stocking_no,");
                            sql.AppendLine("payment_slip_no,");
                            sql.AppendLine("payment_slip_row_no,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("eraser_date,");
                            sql.AppendLine("eraser_amount,");
                            sql.AppendLine("shiwake_error");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @EraserNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @StockingNo ,");
                            sql.AppendLine(" @PaymentSlipNo ,");
                            sql.AppendLine(" @PaymentSlipRowNo ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @EraserDate ,");
                            sql.AppendLine(" @EraserAmount ,");
                            sql.AppendLine(" @ShiwakeError ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    EraserNo = result.EraserNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    StockingNo = result.StockingNo,
                                    PaymentSlipNo = result.PaymentSlipNo,
                                    PaymentSlipRowNo = result.PaymentSlipRowNo,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    EraserDate = result.EraserDate,
                                    EraserAmount = result.EraserAmount,
                                    ShiwakeError = result.ShiwakeError
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL_CANCEL)
                {
                    //承認取消
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("delete from external_if.if_shiwake_eraser_detail_payment ");
                    sql.AppendLine(" where eraser_no = @EraserNo ");
                    sql.AppendLine("   and       seq = @Seq");
                    // 更新処理実行
                    regFlg = db.Regist(
                        sql.ToString(),
                        new
                        {
                            EraserNo = paramInfo.EraserNo,
                            Seq = seqresult.Seq
                        });
                    if (regFlg < 0)
                    {
                        // 異常終了
                        return false;
                    }
                }

                //取消
                if (paramInfo.Procsess == ShiDao.ProcessDivison.CANCEL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select");
                    sql.AppendLine(" eraser_no as EraserNo,");
                    sql.AppendLine(" seq as Seq,");
                    sql.AppendLine(" process as Process,");
                    sql.AppendLine(" status as Status,");
                    sql.AppendLine(" stocking_no as StockingNo,");
                    sql.AppendLine(" payment_slip_no as PaymentSlipNo,");
                    sql.AppendLine(" payment_slip_row_no as PaymentSlipRowNo,");
                    sql.AppendLine(" link_flg as LinkFlg,");
                    sql.AppendLine(" shiwake_cd as ShiwakeCd,");
                    sql.AppendLine(" shiwake_no as ShiwakeNo,");
                    sql.AppendLine(" account_years as AccountYears,");
                    sql.AppendLine(" eraser_date as EraserDate,");
                    sql.AppendLine(" eraser_amount as EraserAmount,");
                    sql.AppendLine(" shiwake_error as ShiwakeError");
                    sql.AppendLine("  from external_if.if_shiwake_eraser_detail_payment");
                    sql.AppendLine(" where eraser_no = @EraserNo ");
                    sql.AppendLine("  and      seq = @Seq ");

                    // SQL実行
                    IList<ShiDao.ErazerPaymentTableItem> resultList = db.GetList<ShiDao.ErazerPaymentTableItem>(
                        sql.ToString(),
                        new
                        {
                            EraserNo = paramInfo.EraserNo,
                            Seq = seqresult.Seq
                        });

                    foreach (ShiDao.ErazerPaymentTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            //売上額をマイナス変換
                            result.EraserAmount = -result.EraserAmount;
                            result.Seq = seqresult.Seq + 1;
                            result.Process = paramInfo.Procsess;
                            result.LinkFlg = 0;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_eraser_detail_payment");
                            sql.AppendLine("( ");
                            sql.AppendLine("eraser_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("stocking_no,");
                            sql.AppendLine("payment_slip_no,");
                            sql.AppendLine("payment_slip_row_no,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("eraser_date,");
                            sql.AppendLine("eraser_amount,");
                            sql.AppendLine("shiwake_error");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @EraserNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @StockingNo ,");
                            sql.AppendLine(" @PaymentSlipNo ,");
                            sql.AppendLine(" @PaymentSlipRowNo ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @EraserDate ,");
                            sql.AppendLine(" @EraserAmount ,");
                            sql.AppendLine(" @ShiwakeError ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    EraserNo = result.EraserNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    StockingNo = result.StockingNo,
                                    PaymentSlipNo = result.PaymentSlipNo,
                                    PaymentSlipRowNo = result.PaymentSlipRowNo,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    EraserDate = result.EraserDate,
                                    EraserAmount = result.EraserAmount,
                                    ShiwakeError = result.ShiwakeError
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

        #region 相殺
        /// <summary>
        /// 相殺承認登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">仕訳送信パラメータ</param>
        /// <returns>true:正常 false：エラー</returns>
        public static bool OffsetGroupDataApprovalRegist(ComDB db, ShiDao.ShiwakeSendParameter paramInfo)
        {
            System.Text.StringBuilder sql;
            int regFlg = 0;
            int shiwakeError;

            string accountYears;

            // パラメータチェック（下記項目が未入力の場合）
            // パラメータ.売上番号
            // パラメータ.処理状況
            // パラメータ.ステータス
            if (string.IsNullOrEmpty(paramInfo.OffsetNo))
            {
                // エラーログを出力する。
                // 「引数が設定されていません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MB00005));
                return false;
            }

            //仕訳CSV作成用テーブル作成
            try
            {
                //現在の履歴テーブルより、SEQ番号を取得
                sql = new System.Text.StringBuilder();
                sql.AppendLine("select max(seq) as Seq");
                sql.AppendLine("  from external_if.if_shiwake_offset_group_data");
                sql.AppendLine(" where offset_no = @OffsetNo ");

                ShiDao.OffsetTableItem seqresult = db.GetEntity<ShiDao.OffsetTableItem>(
                    sql.ToString(),
                    new
                    {
                        OffsetNo = paramInfo.OffsetNo
                    });

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    seqresult.Seq += 1;
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select max(proof_seq) as max_proof_seq from proof.offset_group_header_proof where offset_no = @OffsetNo ");
                    int proofSeq = db.GetEntity<int?>(
                        sql.ToString(),
                        new
                        {
                            OffsetNo = paramInfo.OffsetNo
                        }) ?? 1;

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select offset_date from proof.offset_group_header_proof where offset_no = @OffsetNo and proof_seq = @ProofSeq ");
                    DateTime offsetDate = db.GetEntity<DateTime>(
                        sql.ToString(),
                        new
                        {
                            OffsetNo = paramInfo.OffsetNo,
                            ProofSeq = proofSeq
                        });

                    var param = new { OffsetShiwakeCd = ShiDao.ShiwakeDivision.OffsetShiwakeCd, OffsetShiwakeDiv = ShiDao.ShiwakeDivision.OffsetShiwakeDiv, OffsetNo = paramInfo.OffsetNo, ValidDate = offsetDate, ProofSeq = proofSeq };

                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("select mst1.journal_pattern_cd as ShiwakeCd");
                    sql.AppendLine("     , mst1.journal_pattern_division as ShiwakeNo");
                    sql.AppendLine("     , trn1.offset_date as OffsetDate");
                    sql.AppendLine("     , left(trn2.debit_section_cd, 6) as AccountDebitSectionCd");
                    sql.AppendLine("     , left(trn2.debit_title_cd, 5) as DebitTitleCd");
                    sql.AppendLine("     , left(trn2.credit_section_cd, 6) as AccountCreditSectionCd");
                    sql.AppendLine("     , left(trn2.credit_title_cd, 5) as CreditTitleCd");
                    sql.AppendLine("     , trn2.vender_cd as VenderCd");
                    sql.AppendLine("     , trn2.vender_division as VenderDivision");
                    sql.AppendLine("     , trn1.offset_amount as OffsetAmount");
                    sql.AppendLine("     , trn2.payable_offset_amount as PayableOffsetAmount");
                    sql.AppendLine("     , trn2.deposit_offset_amount as DepositOffsetAmount");
                    sql.AppendLine("     , trn1.offset_no as OffsetNo");
                    sql.AppendLine("     , trn1.offset_status as Status");
                    sql.AppendLine("  from proof.offset_group_header_proof trn1");
                    sql.AppendLine("  left outer join proof.offset_group_data_proof trn2");
                    sql.AppendLine("               on trn2.offset_no = trn1.offset_no");
                    sql.AppendLine("              and trn2.update_date = trn1.update_date");
                    sql.AppendLine("              and trn2.proof_status = trn1.proof_status");
                    sql.AppendLine("     ,(select * from shiwake_ptn_master where journal_pattern_cd = @OffsetShiwakeCd and journal_pattern_division = @OffsetShiwakeDiv) mst1");
                    sql.AppendLine(" where trn1.offset_no = @OffsetNo");
                    sql.AppendLine("   and trn1.proof_seq = @ProofSeq");

                    //sql = string.Empty;
                    //sql = sql + "select mst1.journal_pattern_cd as shiwake_cd";
                    //sql = sql + "     , mst1.journal_pattern_division as shiwake_no";
                    //sql = sql + "     , trn1.offset_date as offset_date";
                    //sql = sql + "     , left(trn2.debit_section_cd, 6) as account_debit_section_cd";
                    //sql = sql + "     , left(trn2.debit_title_cd, 5) as debit_title_cd";
                    //sql = sql + "     , left(trn2.credit_section_cd, 6) as account_credit_section_cd";
                    //sql = sql + "     , left(trn2.credit_title_cd, 5) as credit_title_cd";
                    //sql = sql + "     , trn2.vender_cd as vender_cd";
                    //sql = sql + "     , trn2.vender_division as vender_division";
                    //sql = sql + "     , trn1.offset_amount as offset_amount";
                    //sql = sql + "     , trn2.payable_offset_amount as payable_offset_amount";
                    //sql = sql + "     , trn2.deposit_offset_amount as deposit_offset_amount";
                    //sql = sql + "     , trn1.offset_no as offset_no";
                    //sql = sql + "     , trn1.offset_status as status";

                    //sql = sql + "  from proof.offset_group_header_proof trn1";
                    //sql = sql + "  left outer join proof.offset_group_data_proof trn2";
                    //sql = sql + "               on trn2.offset_no = trn1.offset_no";
                    //sql = sql + "     , shiwake_ptn_master mst1";

                    //sql = sql + " where mst1.journal_pattern_cd = '" + ShiDao.ShiwakeDivision.OffsetShiwakeCd + "'";
                    //sql = sql + "   and mst1.journal_pattern_division = '" + ShiDao.ShiwakeDivision.OffsetShiwakeDiv + "'";
                    //sql = sql + "   and trn1.offset_no ='" + paramInfo.OffsetNo + "'";
                    //sql = sql + "   and trn1.proof_seq = (select max(proof_seq) from proof.offset_group_data_proof trn3 where trn3.offset_no ='" + paramInfo.OffsetNo + "')";

                    // SQL実行
                    IList<ShiDao.OffsetTableItem> resultList = db.GetList<ShiDao.OffsetTableItem>(sql.ToString(), param);

                    foreach (ShiDao.OffsetTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            accountYears = string.Format("{0:yyyyMM}", result.OffsetDate);
                            shiwakeError = StoDao.FncCheckShiwakeError(result.ShiwakeCd, result.ShiwakeNo, result.OffsetNo, db);  // 手形サイト
                            shiwakeError = 0;
                            result.Seq = seqresult.Seq;
                            result.Process = paramInfo.Procsess;
                            result.AccountYears = accountYears;
                            result.ShiwakeError = shiwakeError;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_offset_group_data");
                            sql.AppendLine("( ");
                            sql.AppendLine("offset_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("offset_date,");
                            sql.AppendLine("account_debit_section_cd,");
                            sql.AppendLine("debit_title_cd,");
                            sql.AppendLine("account_credit_section_cd,");
                            sql.AppendLine("credit_title_cd,");
                            sql.AppendLine("vender_cd,");
                            sql.AppendLine("vender_division,");
                            sql.AppendLine("offset_amount,");
                            sql.AppendLine("payable_offset_amount,");
                            sql.AppendLine("deposit_offset_amount,");
                            sql.AppendLine("shiwake_error");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @OffsetNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @OffsetDate ,");
                            sql.AppendLine(" @AccountDebitSectionCd ,");
                            sql.AppendLine(" @DebitTitleCd ,");
                            sql.AppendLine(" @AccountCreditSectionCd ,");
                            sql.AppendLine(" @CreditTitleCd ,");
                            sql.AppendLine(" @VenderCd ,");
                            sql.AppendLine(" @VenderDivision ,");
                            sql.AppendLine(" @OffsetAmount ,");
                            sql.AppendLine(" @PayableOffsetAmount ,");
                            sql.AppendLine(" @DepositOffsetAmount ,");
                            sql.AppendLine(" @ShiwakeError ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    OffsetNo = result.OffsetNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    OffsetDate = result.OffsetDate,
                                    AccountDebitSectionCd = result.AccountDebitSectionCd,
                                    DebitTitleCd = result.DebitTitleCd,
                                    AccountCreditSectionCd = result.AccountCreditSectionCd,
                                    CreditTitleCd = result.CreditTitleCd,
                                    VenderCd = result.VenderCd,
                                    VenderDivision = result.VenderDivision,
                                    OffsetAmount = result.OffsetAmount,
                                    PayableOffsetAmount = result.PayableOffsetAmount,
                                    DepositOffsetAmount = result.DepositOffsetAmount,
                                    ShiwakeError = result.ShiwakeError
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }

                if (paramInfo.Procsess == ShiDao.ProcessDivison.APPROVAL_CANCEL)
                {
                    //承認取消
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine("delete from external_if.if_shiwake_offset_group_data ");
                    sql.AppendLine("where offset_no = @OffsetNo ");
                    sql.AppendLine("  and       seq = @Seq ");
                    // 更新処理実行
                    regFlg = db.Regist(
                        sql.ToString(),
                        new
                        {
                            OffsetNo = paramInfo.OffsetNo,
                            Seq = seqresult.Seq
                        });
                    if (regFlg < 0)
                    {
                        // 異常終了
                        return false;
                    }
                }

                //取消
                if (paramInfo.Procsess == ShiDao.ProcessDivison.CANCEL)
                {
                    sql = new System.Text.StringBuilder();
                    sql.AppendLine(" select");
                    sql.AppendLine(" offset_no as OffsetNo,");
                    sql.AppendLine(" seq as Seq,");
                    sql.AppendLine(" process as Process,");
                    sql.AppendLine(" status as Status,");
                    sql.AppendLine(" link_flg as LinkFlg,");
                    sql.AppendLine(" shiwake_cd as ShiwakeCd,");
                    sql.AppendLine(" shiwake_no as ShiwakeNo,");
                    sql.AppendLine(" account_years as AccountYears,");
                    sql.AppendLine(" offset_date as OffsetDate,");
                    sql.AppendLine(" account_debit_section_cd as AccountDebitSectionCd,");
                    sql.AppendLine(" debit_title_cd as DebitTitleCd,");
                    sql.AppendLine(" account_credit_section_cd as AccountCreditSectionCd,");
                    sql.AppendLine(" credit_title_cd as CreditTitleCd,");
                    sql.AppendLine(" vender_cd as VenderCd,");
                    sql.AppendLine(" vender_division as VenderDivision,");
                    sql.AppendLine(" offset_amount as OffsetAmount,");
                    sql.AppendLine(" payable_offset_amount as PayableOffsetAmount,");
                    sql.AppendLine(" deposit_offset_amount as DepositOffsetAmount,");
                    sql.AppendLine(" shiwake_error as ShiwakeError");
                    sql.AppendLine("  from external_if.if_shiwake_offset_group_data");
                    sql.AppendLine(" where offset_no = @OffsetNo ");
                    sql.AppendLine("  and        seq = @Seq ");

                    // SQL実行
                    IList<ShiDao.OffsetTableItem> resultList = db.GetList<ShiDao.OffsetTableItem>(
                        sql.ToString(),
                        new
                        {
                            OffsetNo = paramInfo.OffsetNo,
                            Seq = seqresult.Seq
                        });

                    foreach (ShiDao.OffsetTableItem result in resultList)
                    {
                        if (result != null)
                        {
                            //売上額をマイナス変換
                            result.OffsetAmount = -result.OffsetAmount;
                            result.PayableOffsetAmount = -result.PayableOffsetAmount;
                            result.DepositOffsetAmount = -result.DepositOffsetAmount;
                            result.Seq = seqresult.Seq + 1;
                            result.Process = paramInfo.Procsess;
                            result.LinkFlg = 0;

                            sql = new System.Text.StringBuilder();
                            sql.AppendLine("insert into external_if.if_shiwake_offset_group_data");
                            sql.AppendLine("( ");
                            sql.AppendLine("offset_no,");
                            sql.AppendLine("seq,");
                            sql.AppendLine("process,");
                            sql.AppendLine("status,");
                            sql.AppendLine("link_flg,");
                            sql.AppendLine("shiwake_cd,");
                            sql.AppendLine("shiwake_no,");
                            sql.AppendLine("account_years,");
                            sql.AppendLine("offset_date,");
                            sql.AppendLine("account_debit_section_cd,");
                            sql.AppendLine("debit_title_cd,");
                            sql.AppendLine("account_credit_section_cd,");
                            sql.AppendLine("credit_title_cd,");
                            sql.AppendLine("vender_cd,");
                            sql.AppendLine("vender_division,");
                            sql.AppendLine("offset_amount,");
                            sql.AppendLine("payable_offset_amount,");
                            sql.AppendLine("deposit_offset_amount,");
                            sql.AppendLine("shiwake_error");
                            sql.AppendLine(") ");
                            sql.AppendLine(" values ");
                            sql.AppendLine("( ");
                            sql.AppendLine(" @OffsetNo ,");
                            sql.AppendLine(" @Seq ,");
                            sql.AppendLine(" @Process ,");
                            sql.AppendLine(" @Status ,");
                            sql.AppendLine(" @LinkFlg ,");
                            sql.AppendLine(" @ShiwakeCd ,");
                            sql.AppendLine(" @ShiwakeNo ,");
                            sql.AppendLine(" @AccountYears ,");
                            sql.AppendLine(" @OffsetDate ,");
                            sql.AppendLine(" @AccountDebitSectionCd ,");
                            sql.AppendLine(" @DebitTitleCd ,");
                            sql.AppendLine(" @AccountCreditSectionCd ,");
                            sql.AppendLine(" @CreditTitleCd ,");
                            sql.AppendLine(" @VenderCd ,");
                            sql.AppendLine(" @VenderDivision ,");
                            sql.AppendLine(" @OffsetAmount ,");
                            sql.AppendLine(" @PayableOffsetAmount ,");
                            sql.AppendLine(" @DepositOffsetAmount ,");
                            sql.AppendLine(" @ShiwakeError ");
                            sql.AppendLine(") ");
                            // 更新処理
                            regFlg = db.Regist(
                                sql.ToString(),
                                new
                                {
                                    OffsetNo = result.OffsetNo,
                                    Seq = result.Seq,
                                    Process = result.Process,
                                    Status = result.Status,
                                    LinkFlg = result.LinkFlg,
                                    ShiwakeCd = result.ShiwakeCd,
                                    ShiwakeNo = result.ShiwakeNo,
                                    AccountYears = result.AccountYears,
                                    OffsetDate = result.OffsetDate,
                                    AccountDebitSectionCd = result.AccountDebitSectionCd,
                                    DebitTitleCd = result.DebitTitleCd,
                                    AccountCreditSectionCd = result.AccountCreditSectionCd,
                                    CreditTitleCd = result.CreditTitleCd,
                                    VenderCd = result.VenderCd,
                                    VenderDivision = result.VenderDivision,
                                    OffsetAmount = result.OffsetAmount,
                                    PayableOffsetAmount = result.PayableOffsetAmount,
                                    DepositOffsetAmount = result.DepositOffsetAmount,
                                    ShiwakeError = result.ShiwakeError
                                });
                            if (regFlg < 0)
                            {
                                // 更新処理に失敗した場合、エラーで返す
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

    }
}