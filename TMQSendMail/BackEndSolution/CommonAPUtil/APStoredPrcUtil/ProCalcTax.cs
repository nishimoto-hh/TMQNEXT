using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using APComUtil = APCommonUtil.APCommonUtil.APCommonUtil;
using APConsts = APConstants.APConstants;
using APResources = CommonAPUtil.APCommonUtil.APResources;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComSTDUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using FuncUtil = CommonAPUtil.APStoredFncUtil.APStoredFncUtil;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>
    /// 消費税算出処理
    /// </summary>
    /// <remarks>現行プロシージャ「PRO_CALC_TAX」</remarks>
    public class ProCalcTax
    {
        /// <summary>
        /// SQLファイル名
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQLサブディレクトリ </summary>
            public const string SubDir = "Common\\ProCalcTax";
            /// <summary>消費税情報取得SQL</summary>
            public const string GetTaxInfo = "ProCalcTax_GetTaxInfo";
            /// <summary>通貨マスタ消費税取得用SQL</summary>
            public const string GetTaxRate = "ProCalcTax_GetTaxRate";
        }

        /// <summary>
        /// 消費税取得処理
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <param name="returnValue">戻り値</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="languageId">言語ID</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool Execute(ParamProCalcTax param, out OutProCalcTax returnValue, ComDB db, string languageId)
        {
            returnValue = new OutProCalcTax();

            var taxParam = new DataClass.SearchConditionGetTaxInfo(param.VenderKey, APConsts.COMPANY.COMPANY_CD, param.Category);
            DataClass.ResultGetTaxInfo taxInfo = db.GetEntityByOutsideSqlByDataClass<DataClass.ResultGetTaxInfo>(SqlName.GetTaxInfo, SqlName.SubDir, taxParam);

            string accountsCd = string.Empty;

            // 品目仕様
            if (param.Category == APConsts.TAX_MASTER.CATEGORY.SALES)
            {
                ComDao.ItemArticleAttributeEntity itemAttr = new ComDao.ItemArticleAttributeEntity().GetEntity(param.ItemSpecKey.ItemCd, param.ItemSpecKey.SpecificationCd, param.ItemSpecKey.ActiveDate, db);
                if (itemAttr == null)
                {
                    // 仕様コードがマスタに存在しません
                    returnValue.ErrorMessage = ComSTDUtil.GetPropertiesMessage(new string[] { APResources.ID.M00308, APResources.ID.I00594 }, languageId, db: db);
                    return false;
                }
                accountsCd = itemAttr.ArticleAccountsCd;
            }
            else if (param.Category == APConsts.TAX_MASTER.CATEGORY.STOCKING)
            {
                ComDao.ItemPurchaseAttributeEntity itemAttr = new ComDao.ItemPurchaseAttributeEntity().GetEntity(param.ItemSpecKey.ItemCd, param.ItemSpecKey.SpecificationCd, param.ItemSpecKey.ActiveDate, db);
                if (itemAttr == null)
                {
                    // 仕様コードがマスタに存在しません
                    returnValue.ErrorMessage = ComSTDUtil.GetPropertiesMessage(new string[] { APResources.ID.M00308, APResources.ID.I00594 }, languageId, db: db);
                    return false;
                }
                accountsCd = itemAttr.PurchaseAccountsCd;
            }

            string taxCd = string.Empty;
            decimal taxRatio = -1;
            if (!ComSTDUtil.IsNullOrEmpty(accountsCd))
            {
                IList<ComDao.AccountsTaxEntity> accountsTax = db.GetList<ComDao.AccountsTaxEntity>("select * from accounts_tax where accounts_cd = @AccountsCd and valid_date <= @ValidDate and category = @Category and del_flg = 0 order by valid_date desc",
                new { AccountsCd = accountsCd, ValidDate = param.CalcDate, Category = param.Category });
                if (accountsTax != null && accountsTax.Count > 0)
                {
                    taxCd = accountsTax[0].TaxCd;
                    ComDao.TaxMasterEntity taxMaster = new ComDao.TaxMasterEntity().GetEntity(accountsTax[0].TaxCd, param.Category, db);
                    if (taxMaster != null)
                    {
                        taxRatio = taxMaster.TaxRatio;
                    }
                }
            }

            if (taxRatio.Equals(-1))
            {
                // 上記以外、通貨マスタを参照
                ComDao.CurrencyCtlCtlEntity currency = new ComDao.CurrencyCtlCtlEntity();
                currency = db.GetEntityByOutsideSql<ComDao.CurrencyCtlCtlEntity>(SqlName.GetTaxRate, SqlName.SubDir,
                    new { CurrencyCd = param.CurrencyCd, ExValidDate = param.CalcDate });
                if (currency == null)
                {
                    taxRatio = 0;
                }
                else
                {
                    taxRatio = currency.TaxRatio ?? 0;

                    IList<ComDao.TaxMasterEntity> taxMaster = db.GetList<ComDao.TaxMasterEntity>("select * from tax_master where category = @Category and valid_date <= @ValidDate and tax_ratio = @TaxRatio and del_flg = 0 order by valid_date desc",
                    new { Category = param.Category, ValidDate = param.CalcDate, TaxRatio = taxRatio });
                    if (taxMaster != null && taxMaster.Count > 0)
                    {
                        taxCd = taxMaster[0].TaxCd;
                    }
                }
            }
            //if (ComSTDUtil.IsNullOrEmpty(accountsCd))
            //{
            //    ComDao.ItemSpecificationEntity itemSpec = new ComDao.ItemSpecificationEntity().GetEntity(param.ItemSpecKey.ItemCd, param.ItemSpecKey.SpecificationCd, param.ItemSpecKey.ActiveDate, db);
            //    if (itemSpec == null)
            //    {
            //        // 仕様コードがマスタに存在しません
            //        returnValue.ErrorMessage = ComSTDUtil.GetPropertiesMessage(new string[] { APResources.ID.M00308, APResources.ID.I00594 }, languageId, db: db);
            //        return false;
            //    }
            //    accountsCd = itemSpec.AccountsCd;
            //}

            //if (ComSTDUtil.IsNullOrEmpty(accountsCd))
            //{
            //    // 勘定科目コードがマスタに存在しません
            //    returnValue.ErrorMessage = ComSTDUtil.GetPropertiesMessage(new string[] { APResources.ID.M00308, APResources.ID.I10019 }, languageId, db: db);
            //    return false;
            //}

            //IList<ComDao.AccountsTaxEntity> accountsTax = db.GetList<ComDao.AccountsTaxEntity>("select * from accounts_tax where accounts_cd = @AccountsCd and valid_date <= @ValidDate and category = @Category and del_flg = 0 order by valid_date desc",
            //new { AccountsCd = accountsCd, ValidDate = param.CalcDate, Category = param.Category });
            //if (accountsTax == null || accountsTax.Count <= 0)
            //{
            //    // 勘定科目コードがマスタに存在しません
            //    returnValue.ErrorMessage = ComSTDUtil.GetPropertiesMessage(new string[] { APResources.ID.M00308, APResources.ID.I10019 }, languageId, db: db);
            //    return false;
            //}
            //ComDao.TaxMasterEntity taxMaster = new ComDao.TaxMasterEntity().GetEntity(accountsTax[0].TaxCd, param.Category, db);
            //if (taxMaster == null)
            //{
            //    // 消費税区分コードがマスタに存在しません
            //    returnValue.ErrorMessage = ComSTDUtil.GetPropertiesMessage(new string[] { APResources.ID.M00308, APResources.ID.I20060 }, languageId, db: db);
            //    return false;
            //}

            //decimal taxRatio = taxMaster.TaxRatio;
            decimal outAmount = (decimal)param.InAmount;
            decimal taxAmount = 0;

            // 端数
            outAmount = FuncUtil.FncRound(outAmount, taxInfo.GetAmountRoundUpNum(), taxInfo.AmountRoundup);

            if (APConsts.VENDER.TAX_DIVISION.EXCLUDED.Equals(taxInfo.TaxDivision))
            {
                // 外税
                taxAmount = outAmount * taxRatio / 100;
                // 丸め処理
                taxAmount = FuncUtil.FncRound(taxAmount, taxInfo.GetTaxRoundUpNum(), taxInfo.TaxRoundup);
            }
            else if (APConsts.VENDER.TAX_DIVISION.INCLUDED.Equals(taxInfo.TaxDivision))
            {
                // 内税
                taxAmount = outAmount * taxRatio / (taxRatio + 100);
                // 丸め処理
                taxAmount = FuncUtil.FncRound(taxAmount, taxInfo.GetTaxRoundUpNum(), taxInfo.TaxRoundup);
                // 減算
                outAmount -= taxAmount;
            }

            returnValue.TaxDivision = taxInfo.TaxDivision;
            returnValue.TaxRate = taxRatio;
            returnValue.TaxCd = taxCd;
            returnValue.TaxAmount = taxAmount;
            returnValue.OutAmount = outAmount;

            return true;
        }

        /// <summary>
        /// 取引先から取得した税区分が内税の場合、引数の金額から税額を引いた金額を取得する処理
        /// </summary>
        /// <param name="amount">計算対象の金額</param>
        /// <param name="venderKey">取引先のキー情報</param>
        /// <param name="category">用途</param>
        /// <param name="db">DB接続</param>
        /// <returns>内税なら税額を引いた金額、そうでなければ引数の金額</returns>
        public static decimal GetTaxIncludedAmount(decimal amount, ComDao.VenderEntity.PrimaryKey venderKey, string category, ComDB db)
        {
            decimal returnAmount = amount;

            // 取引先または自社マスタより税情報を取得
            var taxParam = new DataClass.SearchConditionGetTaxInfo(venderKey, APConsts.COMPANY.COMPANY_CD, category);
            DataClass.ResultGetTaxInfo taxInfo = db.GetEntityByOutsideSqlByDataClass<DataClass.ResultGetTaxInfo>(SqlName.GetTaxInfo, SqlName.SubDir, taxParam);

            // 消費税算出区分が明細単位の場合、消費税算出
            if (taxInfo.CalcDivision == APConsts.VENDER.CALC_DIVISION.DETAIL)
            {
                // 消費税課税区分が内税の場合、消費税を引いた金額を取得する
                if (taxInfo.TaxDivision == APConsts.VENDER.TAX_DIVISION.INCLUDED)
                {
                    // 未使用かもしれないので、その場合10%とする
                    decimal taxRatio = APComUtil.GetCompanyData(db).TaxRatio ?? 10;
                    // 税額 = 金額 * 税率(%) / (税率(%) + 100)
                    decimal taxAmount = amount * taxRatio / (taxRatio + 100);
                    // 丸め処理
                    taxAmount = FuncUtil.FncRound(taxAmount, taxInfo.GetTaxRoundUpNum(), taxInfo.TaxRoundup);
                    // 税額を引く
                    returnAmount = amount - taxAmount;
                }
            }
            return returnAmount;
        }

        /// <summary>実行時引数クラス</summary>
        public class ParamProCalcTax
        {
            /// <summary>Gets or sets 取引先マスタのキー</summary>
            /// <value>取引先マスタのキー</value>
            public ComDao.VenderEntity.PrimaryKey VenderKey { get; set; }
            /// <summary>Gets or sets 品目仕様マスタのキー</summary>
            /// <value>品目仕様マスタのキー</value>
            public ComDao.ItemSpecificationEntity.PrimaryKey ItemSpecKey { get; set; }
            /// <summary>Gets or sets 受入日</summary>
            /// <value>受入日</value>
            public DateTime CalcDate { get; set; }
            /// <summary>Gets or sets 計算前金額</summary>
            /// <value>計算前金額</value>
            public decimal InAmount { get; set; }
            /// <summary>Gets or sets 消費税課税区分</summary>
            /// <value>消費税課税区分</value>
            public int TaxDivision { get; set; }
            /// <summary>Gets or sets 用途(TAX_MASTER.CATEGORY:SALES/CREDIT/STOCKING)</summary>
            /// <value>用途(TAX_MASTER.CATEGORY:SALES/CREDIT/STOCKING)</value>
            public string Category { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
        }

        /// <summary>実行時戻り値クラス</summary>
        public class OutProCalcTax
        {
            /// <summary>Gets or sets エラーメッセージ</summary>
            /// <value>エラーメッセージ</value>
            public string ErrorMessage { get; set; }
            /// <summary>Gets or sets 計算後金額</summary>
            /// <value>計算後金額</value>
            public decimal OutAmount { get; set; }
            /// <summary>Gets or sets 消費税課税区分</summary>
            /// <value>消費税課税区分</value>
            public int TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal TaxAmount { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal TaxRate { get; set; }
            /// <summary>Gets or sets 消費税区分コード</summary>
            /// <value>消費税区分コード</value>
            public string TaxCd { get; set; }
        }

        /// <summary>
        /// データクラス
        /// </summary>
        private class DataClass
        {
            /// <summary>
            /// SQL:GetTaxInfoの検索条件
            /// </summary>
            public class SearchConditionGetTaxInfo
            {
                /// <summary>
                /// Gets or sets 取引先区分
                /// </summary>
                /// <value>
                /// 取引先区分
                /// </value>
                public string VenderDivision { get; set; }
                /// <summary>
                /// Gets or sets 取引先コード
                /// </summary>
                /// <value>
                /// 取引先コード
                /// </value>
                public string VenderCd { get; set; }
                /// <summary>
                /// Gets or sets 適用開始日
                /// </summary>
                /// <value>
                /// 適用開始日
                /// </value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// Gets or sets 自社コード
                /// </summary>
                /// <value>
                /// 自社コード
                /// </value>
                public string CompanyCd { get; set; }
                /// <summary>
                /// Gets or sets 用途
                /// </summary>
                /// <value>
                /// 用途
                /// </value>
                public string Category { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="venderKey">取引先のキー情報</param>
                /// <param name="paramCompanyCd">会社コード</param>
                /// <param name="paramCategory">用途(TAX_MASTER.CATEGORY:SALES/CREDIT/STOCKING)</param>
                public SearchConditionGetTaxInfo(ComDao.VenderEntity.PrimaryKey venderKey, string paramCompanyCd, string paramCategory)
                {
                    VenderDivision = venderKey.VenderDivision;
                    VenderCd = venderKey.VenderCd;
                    ActiveDate = venderKey.ActiveDate;
                    this.CompanyCd = paramCompanyCd;
                    this.Category = paramCategory;
                }
            }
            /// <summary>
            /// SQL:GetTaxInfoの検索結果
            /// </summary>
            public class ResultGetTaxInfo
            {
                /// <summary>Gets or sets 消費税課税区分</summary>
                /// <value>消費税課税区分</value>
                public int TaxDivision { get; set; }
                /// <summary>Gets or sets 消費税算出区分</summary>
                /// <value>消費税算出区分</value>
                public int CalcDivision { get; set; }
                /// <summary>Gets or sets 消費税端数区分</summary>
                /// <value>消費税端数区分</value>
                public int TaxRoundup { get; set; }
                /// <summary>Gets or sets 消費税端数単位</summary>
                /// <value>消費税端数単位</value>
                public int TaxRoundupUnit { get; set; }
                /// <summary>Gets or sets 端数区分</summary>
                /// <value>端数区分</value>
                public int AmountRoundup { get; set; }
                /// <summary>Gets or sets 端数単位</summary>
                /// <value>端数単位</value>
                public int AmountRoundupUnit { get; set; }

                /// <summary>
                /// 消費税端数単位を取得
                /// </summary>
                /// <returns>消費税端数単位</returns>
                public int GetTaxRoundUpNum()
                {
                    if (this.TaxRoundupUnit == APConsts.VENDER.TAX_ROUNDUP_INIT.COMPANY)
                    {
                        // 自社マスタの場合、そのままの値を使用(現行踏襲だが、発生する？)
                        return this.TaxRoundupUnit;
                    }
                    // 取得した値-1を返す
                    // 1→1、2→0.1、3→0.01のため
                    return this.TaxRoundupUnit - 1;
                }

                /// <summary>
                /// 端数単位を取得
                /// </summary>
                /// <returns>端数単位</returns>
                public int GetAmountRoundUpNum()
                {
                    if (this.AmountRoundupUnit == APConsts.VENDER.TAX_ROUNDUP_INIT.COMPANY)
                    {
                        // 自社マスタの場合、そのままの値を使用(現行踏襲だが、発生する？)
                        return this.AmountRoundupUnit;
                    }
                    // 取得した値-1を返す
                    // 1→1、2→0.1、3→0.01のため
                    return this.AmountRoundupUnit - 1;
                }
            }
        }
    }
}
