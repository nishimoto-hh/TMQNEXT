using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using APComUtil = APCommonUtil.APCommonUtil.APCommonUtil;
using APResources = CommonAPUtil.APCommonUtil.APResources;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>
    /// 取引先チェッククラス
    /// </summary>
    public class ProCheckVenderTax
    {
        /// <summary>
        /// SQL定義
        /// </summary>
        public static class SqlName
        {
            /// <summary>SQL格納サブディレクトリ名</summary>
            public const string SubDir = "Common\\ProCheckVenderTax";
            /// <summary>SQL名：PRO_請求先コード取得</summary>
            public const string GetVenderInfo = "ProCheckVenderTax_GetVenderInfo";
            /// <summary>SQL名：PRO_請求先存在確認</summary>
            public const string GetInvoiceCount = "ProCheckVenderTax_CountInvoice";
            /// <summary>SQL名：PRO_消費税計算情報取得</summary>
            public const string ProVenderGetTaxInfo = "ProCheckVenderTax_GetTaxInfo";
        }

        /// <summary>
        /// チェック処理戻り値
        /// </summary>
        public enum ReturnStatus
        {
            /// <summary>正常</summary>
            NoError,
            /// <summary>取引先存在なし</summary>
            NoVenderError,
            /// <summary>請求先存在なし</summary>
            NoInvoiceError,
            /// <summary>消費税情報存在なし</summary>
            NoTaxError
        }

        /// <summary>
        /// 取引先チェック処理
        /// </summary>
        /// <param name="venderKey">チェックする取引先マスタのキー</param>
        /// <param name="db">DB接続</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="errorMessage">out エラーの場合はエラーメッセージを返す</param>
        /// <returns>正常orエラーの種類</returns>
        public static ReturnStatus CheckVender(ComDao.VenderEntity.PrimaryKey venderKey, ComDB db, string languageId, out string errorMessage)
        {
            errorMessage = string.Empty; // 正常の場合用に、エラーメッセージを設定
            //取引先存在チェック
            var venderInfo = db.GetEntityByOutsideSqlByDataClass<ComDao.VenderEntity>(SqlName.GetVenderInfo, SqlName.SubDir, venderKey);
            //取引先が存在しなければエラー
            if (venderInfo == null)
            {
                // 取引先コードが存在しません
                errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.M10109, APResources.ID.I00611 }, languageId, db: db);
                return ReturnStatus.NoVenderError; //取引先存在なし
            }

            //請求先存在チェック
            int invoiceCount = db.GetEntityByOutsideSqlByDataClass<int>(SqlName.GetInvoiceCount, SqlName.SubDir, venderKey);
            //請求先が存在しなければエラー
            if (invoiceCount == 0)
            {
                // 取引先コードに紐づく請求先が存在しません
                errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.M10109, APResources.ID.M10110 }, languageId, db: db);
                return ReturnStatus.NoInvoiceError; //請求先存在なし
            }
            // 請求先情報取得
            var invoiceVender = APComUtil.GetVenderInfoByActiveDate<ComDao.VenderEntity>(venderInfo.VenderDivision, venderInfo.PaymentInvoiceCd, DateTime.Now, languageId, db);
            //請求先が存在しなければエラー
            if (invoiceVender == null)
            {
                // 取引先コードに紐づく請求先が存在しません
                errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.M10109, APResources.ID.M10110 }, languageId, db: db);
                return ReturnStatus.NoInvoiceError; //請求先存在なし
            }

            // 検索条件設定
            var param = new paramTaxInfo();
            param.VenderCd = venderInfo.VenderCd;
            param.VenderDivision = venderInfo.VenderDivision;
            param.ActiveDate = venderInfo.ActiveDate;
            param.PaymentInvoiceCd = venderInfo.PaymentInvoiceCd;
            param.InvoiceActiveDate = invoiceVender.ActiveDate;

            //請求先の消費税計算情報取得
            TaxInfo taxInfo = db.GetEntityByOutsideSqlByDataClass<TaxInfo>(SqlName.ProVenderGetTaxInfo, SqlName.SubDir, param);

            //請求先の消費税計算情報が存在しない場合エラー
            if (string.IsNullOrEmpty(taxInfo.TaxDivision) ||
                string.IsNullOrEmpty(taxInfo.TaxCalcDivision) ||
                string.IsNullOrEmpty(taxInfo.TaxRoundup) ||
                string.IsNullOrEmpty(taxInfo.TaxRoundupUnit))
            {
                // 取引先コードの請求先の消費税計算情報が設定されていません
                errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.M10117 }, languageId, db: db);
                return ReturnStatus.NoTaxError; //消費税計算情報存在なし
            }
            // エラーなし
            return ReturnStatus.NoError;
        }

        /// <summary>
        /// 請求先消費税計算情報の検索条件
        /// </summary>
        private class paramTaxInfo : ComDao.VenderEntity
        {
            /// <summary>Gets or sets 請求先取引先の開始有効日</summary>
            /// <value>請求先取引先の開始有効日</value>
            public DateTime InvoiceActiveDate { get; set; }
        }

        /// <summary>
        /// 請求先消費税計算情報
        /// </summary>
        private class TaxInfo
        {
            /// <summary>Gets or sets 消費税課税区分</summary>
            /// <value>消費税課税区分</value>
            public string TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税算出区分</summary>
            /// <value>消費税算出区分</value>
            public string TaxCalcDivision { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public string TaxRoundup { get; set; }
            /// <summary>Gets or sets 消費税端数単位</summary>
            /// <value>消費税端数単位</value>
            public string TaxRoundupUnit { get; set; }
        }
    }
}
