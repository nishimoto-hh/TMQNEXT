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
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using FuncUtil = CommonAPUtil.APStoredFncUtil.APStoredFncUtil;
using ProCalcTax = CommonAPUtil.APStoredPrcUtil.ProCalcTax;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>売上計上処理</summary>
    /// <remarks>現行AP標準版「PRO_MAKE_SALES_RECORD」より移行</remarks>
    public class ProMakeSalesRecord
    {
        /// <summary>売上テーブルのシーケンスの管理テーブルのキー値</summary>
        private const string SeqProcName = "SALES_NO";
        /// <summary>SQLファイル</summary>
        private static class SqlName
        {
            /// <summary>SQL格納サブディレクトリ名</summary>
            public const string SubDir = "Shipping\\common\\ProMakeSakesRecord";

            /// <summary>処理対象の出荷詳細を取得する処理</summary>
            public const string GetTargetShippingDetail = "ProMakeSalesRecord_GetTargetShipingDetail";
            /// <summary>処理対象の出荷詳細を取得する処理</summary>
            public const string GetIteminfo = "ProMakeSalesRecord_GetIteminfo";
            /// <summary>ユーザの所属組織を取得する処理</summary>
            public const string GetOrganizationCd = "ProMakeSalesRecord_GetOrganizationCd";
            /// <summary>取引先別単価マスタの有無を取得する処理</summary>
            public const string GetUnitPriceExists = "ProMakeSalesRecord_GetUnitPriceExists";
            /// <summary>売上ヘッダ登録処理</summary>
            public const string InsertSalesHeader = "ProMakeSalesRecord_InsertSalesHeader";
            /// <summary>売上詳細登録処理</summary>
            public const string InsertSalesDetail = "ProMakeSalesRecord_InsertSalesDetail";

            /// <summary>税コード取得</summary>
            public const string GetTaxCd = "ProMakeSalesRecord_GetTaxCd";
            /// <summary>税率取得</summary>
            public const string GetTaxRate = "ProMakeSalesRecord_GetTaxRate";
            /// <summary>税コード／税率取得</summary>
            public const string GetTaxInfo = "ProMakeSalesRecord_GetTaxInfo";

            // 売上テーブルプルーフ登録
            /// <summary>売上ヘッダ</summary>
            public const string ProofSalesHeader = "Sales_Header_Proof";
            /// <summary>売上詳細</summary>
            public const string ProofSalesDetail = "Sales_Detail_Proof";
        }

        /// <summary>DB接続</summary>
        private ComDB db;
        /// <summary>言語ID</summary>
        private string languageId;
        /// <summary>メッセージリソース</summary>
        private ComUtil.MessageResources msgResource;
        /// <summary>返却用エラーメッセージ</summary>
        private string errorMessage;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="inDb">DB接続</param>
        /// <param name="inLanguageId">言語ID</param>
        /// <param name="inMsgResource">メッセージ</param>
        public ProMakeSalesRecord(ComDB inDb, string inLanguageId, ComUtil.MessageResources inMsgResource)
        {
            this.db = inDb;
            this.languageId = inLanguageId;
            this.msgResource = inMsgResource;
        }

        /// <summary>
        /// メイン処理
        /// </summary>
        /// <param name="inShippingNo">出荷番号</param>
        /// <param name="inIsRed">赤黒区分、Trueの場合赤</param>
        /// <param name="inUserId">ユーザID</param>
        /// <param name="outErrorMessage">out エラーメッセージ</param>
        /// <returns>エラーの場合False</returns>
        public bool Execute(string inShippingNo, bool inIsRed, string inUserId, out string outErrorMessage)
        {
            outErrorMessage = string.Empty;

            if (!tempExecute(inShippingNo, inIsRed, inUserId))
            {
                outErrorMessage = this.errorMessage;
                return false;
            }

            return true;
        }

        /// <summary>
        /// メイン処理(エラー処理を統一したいためラップ)
        /// </summary>
        /// <param name="inShippingNo">出荷番号</param>
        /// <param name="inIsRed">赤黒区分、Trueの場合赤</param>
        /// <param name="inUserId">ユーザID</param>
        /// <returns>エラーの場合False</returns>
        private bool tempExecute(string inShippingNo, bool inIsRed, string inUserId)
        {
            bool isError = false;
            // 処理対象の出荷詳細を取得
            var sumShipDetail = getSumShipDetail(inShippingNo, out isError);
            if (isError)
            {
                return false;
            }

            // 処理対象の受注を取得
            ComDao.OrderDetailEntity orderDetail = getOrderDetailByShip(inShippingNo, out isError);
            if (isError)
            {
                return false;
            }

            // 出荷ヘッダ
            var shipHead = new ComDao.ShippingEntity().GetEntity(inShippingNo, this.db);

            // 出荷ヘッダにより品目の情報を取得
            DataClass.ItemInfo itemInfo = this.db.GetEntityByOutsideSqlByDataClass<DataClass.ItemInfo>(SqlName.GetIteminfo, SqlName.SubDir, shipHead);

            // 支払・請求先コードにより取引先の情報を取得
            string paymentInvoiceCd = getPaymentInvoiceCd(shipHead); // 支払・請求先コードを取得
            var invoiceVender = APComUtil.GetVenderInfoByActiveDate<ComDao.VenderEntity>(shipHead.VenderDivision, paymentInvoiceCd, DateTime.Now, this.languageId, this.db);

            // 組織コードを取得
            string organizationCd = getOrganizationCdByUserId(inUserId);

            // 受注ヘッダを取得
            var orderHead = new ComDao.OrderHeadEntity().GetEntity(orderDetail.OrderNo, this.db);

            // 現在日時を取得
            DateTime now = DateTime.Now;

            // パラメータクラスを作成
            var paramInfo = new ParamTableInfo(shipHead, sumShipDetail, orderHead, orderDetail, invoiceVender, itemInfo);
            //// 売上Noを取得
            //string salesNo = APComUtil.GetNumber(this.db, SeqProcName);
            string salesNo = string.Empty;
            // ヘッダ
            ComDao.SalesHeaderEntity salesHeader = getRegistSalesHeader(salesNo, inIsRed, paramInfo, organizationCd, inUserId, now);

            // 本締め済チェック
            // 請求
            // SQL文生成
            string sql = string.Empty;
            // 月次処理管理テーブルの締処理区分によって、クローズされている最大勘定年月を取得する
            sql += "select to_char(claimhed.claim_date, 'YYYY/MM/DD') ";
            sql += "  from claim_header claimhed ";
            sql += "  inner join vender vender ";
            sql += "  on claimhed.invoice_cd = vender.payment_invoice_cd  ";
            sql += " where vender.vender_division = @VenderDivison ";
            sql += " and vender.vender_cd = @InvoiceCode ";
            sql += " and vender.active_date = cast(@VenderActiveDate as timestamp) ";
            sql += "order by claimhed.claim_date desc limit 1 ";
            // 最大請求締め日を取得
            string maxAccountYears = db.GetEntity<string>(sql, new { VenderDivison = salesHeader.InvoiceDivision, InvoiceCode = salesHeader.InvoiceCd, VenderActiveDate = salesHeader.InvoiceActiveDate });
            // 今回登録分請求締め日
            // 日付変換できない場合、nullで返す
            var salesDate = salesHeader.SalesDate.ToString();
            DateTime date;
            if (!DateTime.TryParse(salesDate, out date))
            {
                return false;
            }
            var preAccountYears = new DateTime(date.Year, date.Month, date.Day).ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(maxAccountYears))
            {
                int compare = ComUtil.CompareDate(preAccountYears, maxAccountYears);
                if (compare <= 0)
                {
                    // 「月次締処理されているためyyyy/MM/dd以前の日付は更新できません。」
                    this.errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.M10182, APResources.ID.I10276, maxAccountYears }, this.languageId, this.msgResource, this.db);
                    return false;
                }
            }

            // 売掛
            // SQL文生成
            sql = string.Empty;
            // 月次処理管理テーブルの締処理区分によって、クローズされている最大勘定年月を取得する
            sql += "select to_char(max(dephed.credit_date), 'YYYY/MM/DD') ";
            sql += "  from deposit_header dephed ";
            sql += "  inner join vender vender ";
            sql += "  on dephed.invoice_cd = vender.payment_invoice_cd  ";
            sql += " where vender.vender_division = @VenderDivison ";
            sql += " and vender.vender_cd = @InvoiceCode ";
            sql += " and vender.active_date = cast(@VenderActiveDate as timestamp) ";
            // 最大売掛締め日を取得
            string maxcreditdate = db.GetEntity<string>(sql, new { VenderDivison = salesHeader.InvoiceDivision, InvoiceCode = salesHeader.InvoiceCd, VenderActiveDate = salesHeader.InvoiceActiveDate });

            if (!DateTime.TryParse(salesDate, out date))
            {
                return false;
            }
            var preCreditDate = new DateTime(date.Year, date.Month, date.Day).ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(maxcreditdate))
            {
                int compare = ComUtil.CompareDate(preCreditDate, maxcreditdate);
                if (compare <= 0)
                {
                    // 「月次締処理されているためyyyy/MM/dd以前の日付は更新できません。」
                    this.errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.M10182, APResources.ID.I10276, maxcreditdate }, this.languageId, this.msgResource, this.db);
                    return false;
                }
            }

            // 売上Noを取得
            salesNo = APComUtil.GetNumber(this.db, SeqProcName);
            salesHeader.SalesNo = salesNo;
            if (!insertSales<ComDao.SalesHeaderEntity>(true, salesHeader, inUserId))
            {
                // 売上作成処理に失敗しました。
                this.errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00005, APResources.ID.M10136 }, this.languageId, this.msgResource, this.db);
                return false;
            }
            // 詳細
            ComDao.SalesDetailEntity salesDetail = getRegistSalesDetail(salesNo, paramInfo, salesHeader.TaxDivision, salesHeader.TaxRatio, inUserId, now);
            if (!insertSales<ComDao.SalesDetailEntity>(false, salesDetail, inUserId))
            {
                // 売上作成処理に失敗しました。
                this.errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00005, APResources.ID.M10136 }, this.languageId, this.msgResource, this.db);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 出荷番号より出荷詳細の合計を取得する処理
        /// </summary>
        /// <param name="shippingNo">出荷番号</param>
        /// <param name="isError">out エラーの場合True</param>
        /// <returns>取得した出荷詳細</returns>
        private DataClass.SumShipDetail getSumShipDetail(string shippingNo, out bool isError)
        {
            isError = false;
            var condShip = new ComDao.ShippingEntity();
            condShip.ShippingNo = shippingNo;
            DataClass.SumShipDetail sumShipDetail = this.db.GetEntityByOutsideSqlByDataClass<DataClass.SumShipDetail>(SqlName.GetTargetShippingDetail, SqlName.SubDir, condShip);
            if (sumShipDetail == null)
            {
                // 出荷詳細が取得できない場合、エラー
                // 出荷番号が出荷詳細に存在しません。
                this.errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.M00306, APResources.ID.M10135, APResources.ID.M10134 }, this.languageId, this.msgResource, this.db);
                isError = true;
                return null;
            }

            return sumShipDetail;
        }

        /// <summary>
        /// 出荷番号より受注詳細を取得する処理
        /// </summary>
        /// <param name="shippingNo">出荷番号</param>
        /// <param name="isError">out エラーの場合True</param>
        /// <returns>取得した受注詳細</returns>
        private ComDao.OrderDetailEntity getOrderDetailByShip(string shippingNo, out bool isError)
        {
            isError = false;
            // 出荷の情報を取得
            var ship = new ComDao.ShippingEntity().GetEntity(shippingNo, this.db);
            if (string.IsNullOrEmpty(ship.OrderNo) || ship.OrderRowNo == null)
            {
                // 受注番号が無い場合、エラー
                // 受注番号が出荷ヘッダに存在しません。
                this.errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.M00306, APResources.ID.I10078, APResources.ID.M10133 }, this.languageId, this.msgResource, this.db);
                isError = true;
                return null;
            }
            // 受注の情報を取得
            var orderDetail = new ComDao.OrderDetailEntity().GetEntity(ship.OrderNo, ship.OrderRowNo ?? -1, this.db);
            if (orderDetail == null)
            {
                // 受注が無い場合、エラー
                // 受注番号が受注詳細に存在しません。
                this.errorMessage = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.M00306, APResources.ID.I10078, APResources.ID.M10030 }, this.languageId, this.msgResource, this.db);
                isError = true;
            }
            return orderDetail;
        }

        /// <summary>
        /// 出荷ヘッダ情報より取引先マスタの支払・請求先コードを取得する処理
        /// </summary>
        /// <param name="shipping">出荷ヘッダ情報</param>
        /// <returns>支払・請求先コード</returns>
        private string getPaymentInvoiceCd(ComDao.ShippingEntity shipping)
        {
            // 出荷ヘッダより取引先を取得
            var vender = new ComDao.VenderEntity().GetEntity(shipping.VenderDivision, shipping.VenderCd, shipping.VenderActiveDate ?? DateTime.Now, this.db);
            if (vender != null && string.IsNullOrEmpty(vender.PaymentInvoiceCd))
            {
                // 取引先が取得できない、または支払・請求先コードがNullの場合、出荷の取引先コードを返す
                return shipping.VenderCd;
            }
            else
            {
                // 取引先より取得した支払・請求先コード
                return vender.PaymentInvoiceCd;
            }
        }

        /// <summary>
        /// ユーザIDより所属する組織の組織コードを取得
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <returns>組織コード</returns>
        private string getOrganizationCdByUserId(string userId)
        {
            var condLogin = new ComDao.LoginEntity();
            condLogin.UserId = userId;
            string organizationCd = this.db.GetEntityByOutsideSqlByDataClass<string>(SqlName.GetOrganizationCd, SqlName.SubDir, condLogin);
            return organizationCd;
        }

        /// <summary>
        /// 登録する売上ヘッダの内容を取得
        /// </summary>
        /// <param name="salesNo">売上No</param>
        /// <param name="isRed">赤黒フラグ、赤の場合True</param>
        /// <param name="paramInfo">取得した情報をまとめているパラメータクラス</param>
        /// <param name="organizationCd">ユーザの所属組織コード</param>
        /// <param name="userId">登録ユーザID</param>
        /// <param name="now">システム日時</param>
        /// <returns>売上ヘッダ</returns>
        private ComDao.SalesHeaderEntity getRegistSalesHeader(string salesNo, bool isRed, ParamTableInfo paramInfo, string organizationCd, string userId, DateTime now)
        {
            var regist = new ComDao.SalesHeaderEntity();

            regist.SalesNo = salesNo;
            regist.OrderNo = paramInfo.OrderDetail.OrderNo;
            regist.OrderRowNo = paramInfo.OrderDetail.OrderRowNo;
            regist.ShippingNo = paramInfo.ShipHead.ShippingNo;
            regist.RecordSalesBasis = paramInfo.OrderHead.RecordSalesBasis;
            regist.Status = APConsts.SALES.HEAD.STATUS.REGISTED;
            regist.SlipPublishComp = APConsts.SALES.HEAD.SLIP_PUBLISH_COMP.UN_ISSUE;
            regist.SlipPublishDate = DateTime.Parse(now.ToString("yyyy/MM/dd")); // 伝票発行日＝売上伝票登録日
            // 出荷時預り品の場合は預り売上、他は出荷売上 [課題No.320]
            if (1.Equals(paramInfo.ShipHead.KeepInventry ?? 0))
            {
                regist.SalesKind = APConsts.SALES.HEAD.SALES_KIND.KEEP;
            }
            else
            {
                regist.SalesKind = APConsts.SALES.HEAD.SALES_KIND.SHIPPING;
            }
            // 受注の売上計上基準が検収基準の場合、納入予定日。出荷基準の場合、現在日×⇒出荷実績日○
            regist.SalesDate = paramInfo.OrderHead.RecordSalesBasis == APConsts.ORDER_HEAD.RECORD_SALES_BASIS.INSPECTION ? paramInfo.OrderHead.DeliveryExpectedDate ?? now : paramInfo.ShipDetail.MaxShippingResultDate;
            if (APConsts.ORDER_HEAD.RECORD_SALES_BASIS.INSPECTION.Equals(paramInfo.OrderHead.RecordSalesBasis))
            {
                // 検収基準の出荷売上における売上日について、出荷実績日が納期を超えている場合は出荷実績日を売上日として計上すること
                if (paramInfo.ShipDetail.MaxShippingResultDate != null && regist.SalesDate < paramInfo.ShipDetail.MaxShippingResultDate)
                {
                    regist.SalesDate = DateTime.Parse(paramInfo.ShipDetail.MaxShippingResultDate.ToString("yyyy/MM/dd"));
                }
            }
            // 勘定年月を取得
            string shipResultdate = ComUtil.ConvertDatetimeToYmdString(paramInfo.ShipHead.ShippingResultDate); // 出荷実績日をyyyymmddに変換
            // 取引先開始有効日はNullは無いので適当
            string accountYears = FuncUtil.FncGetAccountYears(null, paramInfo.ShipHead.VenderCd, paramInfo.ShipHead.VenderActiveDate ?? now, regist.SalesDate.ToString("yyyy/MM/dd"), this.db);
            //string accountYears = FuncUtil.FncGetAccountYears(paramInfo.OrderHead.OrderNo, paramInfo.ShipHead.VenderCd, paramInfo.ShipHead.VenderActiveDate ?? now, shipResultdate, this.db);
            if (string.IsNullOrEmpty(accountYears))
            {
                accountYears = ComUtil.ConvertDatetimeToYmdString(paramInfo.OrderHead.ScheduledShippingDate);
                accountYears = accountYears.Substring(0, 6);
            }
            regist.AccountYears = accountYears;
            regist.DataType = 1; // 売上
            regist.DataTotalDivision = 1;
            regist.CategoryDivision = 1;
            regist.SalesUserId = userId;
            regist.OrganizationCd = organizationCd;
            regist.BalanceCd = paramInfo.OrderHead.BalanceCd;
            regist.VenderDivision = paramInfo.ShipHead.VenderDivision;
            regist.VenderCd = paramInfo.ShipHead.VenderCd;
            regist.VenderActiveDate = paramInfo.ShipHead.VenderActiveDate;
            regist.InvoiceDivision = paramInfo.InvoiceVender.VenderDivision;
            regist.InvoiceCd = paramInfo.InvoiceVender.VenderCd;
            regist.InvoiceActiveDate = paramInfo.InvoiceVender.ActiveDate;
            regist.ChargeOrganizationCd = paramInfo.OrderHead.OrganizationCd;
            regist.DeliveryCd = paramInfo.ShipHead.DeliveryCd;
            if (isRed)
            {
                // 赤の場合、請求先が貸方、販売品の売上部門が借方
                regist.AccountCreditSectionCd = paramInfo.InvoiceVender.SectionCd;
                regist.CreditTitleCd = paramInfo.InvoiceVender.AccountsCd;
                regist.AccountDebitSectionCd = paramInfo.ItemInfo.ArticleSectionCd;
                regist.DebitTitleCd = paramInfo.ItemInfo.ArticleAccountsCd;
            }
            else
            {
                // 黒の場合は、売上が貸方、請求先が借方
                regist.AccountCreditSectionCd = paramInfo.ItemInfo.ArticleSectionCd;
                regist.CreditTitleCd = paramInfo.ItemInfo.ArticleAccountsCd;
                regist.AccountDebitSectionCd = paramInfo.InvoiceVender.SectionCd;
                regist.DebitTitleCd = paramInfo.InvoiceVender.AccountsCd;
            }
            regist.Remark = paramInfo.OrderHead.DeliverySlipRemark;
            regist.CurrencyCd = paramInfo.OrderDetail.CurrencyCd;
            regist.CurrencyRate = convertOrderExRateDecimal(paramInfo.OrderDetail.ExRate);
            // 受注のレート適用開始日はNull許容だが、必ず登録時に値が入るため、Nullはありえない
            regist.RateValidDate = paramInfo.OrderDetail.ExValidDate ?? now;
            regist.InputUserId = userId;
            regist.InputDate = now;
            regist.UpdateUserId = userId;
            regist.UpdateDate = now;

            // 取引先関連
            ComDao.VenderEntity vender = new ComDao.VenderEntity().GetEntity(regist.VenderDivision, regist.VenderCd, (DateTime)regist.VenderActiveDate, db);
            if (vender != null)
            {
                //// 会計部門コード
                //if (!ComUtil.IsNullOrEmpty(vender.SectionCd))
                //{
                //    regist.AccountDebitSectionCd = vender.SectionCd;
                //}
                //// 科目コード
                //if (!ComUtil.IsNullOrEmpty(vender.AccountsCd))
                //{
                //    regist.DebitTitleCd = vender.AccountsCd;
                //}
                // 担当部署
                if (!ComUtil.IsNullOrEmpty(vender.OrganizationCd))
                {
                    regist.ChargeOrganizationCd = vender.OrganizationCd;
                }
            }

            // 消費税関連
            int taxDivision = 0;
            decimal taxRatio = 0;
            string taxCd = string.Empty;
            regist.TaxDivision = paramInfo.OrderDetail.TaxDivision ?? taxDivision;
            regist.TaxRatio = paramInfo.OrderDetail.TaxRatio ?? taxRatio;
            regist.TaxCd = taxCd;
            if (getTaxRate(paramInfo, regist.SalesDate, ref taxDivision, ref taxRatio, ref taxCd))
            {
                regist.TaxRatio = taxRatio;
                regist.TaxCd = taxCd;
                if (taxRatio == 0)
                {
                    // 税率0%なので課税区分:非課税とする
                    regist.TaxDivision = APConsts.VENDER.TAX_DIVISION.FREE;
                }
                else
                {
                    regist.TaxDivision = taxDivision;
                }
            }

            return regist;
        }

        /// <summary>
        /// 税率を取得する ※消費税区分マスタから取得
        /// </summary>
        /// <param name="validDate">対象日付</param>
        /// <param name="accountsCd">勘定科目コード</param>
        /// <param name="taxCd">税区分コード</param>
        /// <returns>税率 ※取得できない場合、0を戻す</returns>
        private decimal getTaxRateFromTaxMaster(DateTime validDate, string accountsCd, ref string taxCd)
        {
            // 日付が取得できない場合、処理を戻す
            if (validDate == null)
            {
                return decimal.Zero;
            }

            // 税コードを取得
            taxCd = db.GetEntityByOutsideSql<string>(SqlName.GetTaxCd, SqlName.SubDir, new { AccountsCd = accountsCd, ValidDate = validDate });

            // 税コードが取得できない場合、処理を戻す
            if (string.IsNullOrEmpty(taxCd))
            {
                return decimal.Zero;
            }

            // 消費税区分マスタからデータを取得する
            ComDao.TaxMasterEntity taxMasterInfo = new ComDao.TaxMasterEntity();
            taxMasterInfo = taxMasterInfo.GetEntity(taxCd, APConsts.TAX_MASTER.CATEGORY.SALES, db);
            // 取得できない場合、処理を戻す
            if (taxMasterInfo == null)
            {
                return decimal.Zero;
            }

            return taxMasterInfo.TaxRatio;
        }

        /// <summary>
        /// 税率を設定
        /// </summary>
        /// <param name="paramInfo">画面情報</param>
        /// <param name="validDate">対象日付</param>
        /// <param name="taxDivision">消費税区分</param>
        /// <param name="taxRatio">税率</param>
        /// <param name="taxCd">税区分コード</param>
        /// <returns>true:取得、false:未取得</returns>
        private bool getTaxRate(ParamTableInfo paramInfo, DateTime validDate,  ref int taxDivision, ref decimal taxRatio, ref string taxCd)
        {
            // 税率を初期化
            taxDivision = APConsts.VENDER.TAX_DIVISION.COMPANY;
            taxRatio = 0;

            // 取引先マスタから消費税区分を取得
            var vender = new ComDao.VenderEntity();
            vender = vender.GetEntity(paramInfo.ShipHead.VenderDivision, paramInfo.ShipHead.VenderCd, (DateTime)paramInfo.ShipHead.VenderActiveDate, this.db);
            if (vender != null)
            {
                taxDivision = vender.TaxDivision ?? APConsts.VENDER.TAX_DIVISION.COMPANY;
                //decimal taxRatio = decimal.Zero;
                if (APConsts.VENDER.TAX_DIVISION.COMPANY.Equals(taxDivision))
                {
                    var company = APComUtil.GetCompanyData(this.db);
                    if (company != null)
                    {
                        taxDivision = company.TaxDivision ?? APConsts.VENDER.TAX_DIVISION.EXCLUDED;
                    }
                }
            }

            // 品目コード、仕様コード、有効開始日が取得できない場合、処理を返す
            if (paramInfo.ShipHead.ItemCd == null && paramInfo.ShipHead.SpecificationCd == null && paramInfo.ShipHead.ItemActiveDate == null)
            {
                taxRatio = 0;
            }
            else
            {
                var condition = new { ValidDate = validDate, CategoryCd = APConsts.TAX_MASTER.CATEGORY.SALES, ItemCd = paramInfo.ShipHead.ItemCd, SpecificationCd = paramInfo.ShipHead.SpecificationCd, ActiveDate = paramInfo.ShipHead.ItemActiveDate, CurrencyCd = vender.CurrencyCd };
                var tax = this.db.GetEntityByOutsideSql<TaxEntity>(SqlName.GetTaxInfo, SqlName.SubDir, condition);
                if (tax != null)
                {
                    taxRatio = tax.TaxRatio;
                    taxCd = tax.TaxCd;

                    return true;
                }

                //// 品目マスタ_販売品扱い属性から売上科目コードを取得し、設定されている場合、税率を取得する
                //ComDao.ItemArticleAttributeEntity itemArticle = new ComDao.ItemArticleAttributeEntity();
                //itemArticle = itemArticle.GetEntity(paramInfo.ShipHead.ItemCd, paramInfo.ShipHead.SpecificationCd, paramInfo.ShipHead.ItemActiveDate, db);
                //// 売上科目コードが取得できた場合、税率マスタから税率を取得
                //if (!string.IsNullOrEmpty(itemArticle.ArticleAccountsCd))
                //{
                //    taxRatio = this.getTaxRateFromTaxMaster(validDate, itemArticle.ArticleAccountsCd, ref taxCd);
                //    return true;
                //}
                //else
                //{
                //    // 上記以外、通貨マスタを参照
                //    if (validDate != null)
                //    {
                //        ComDao.CurrencyCtlCtlEntity currencyInfo = new ComDao.CurrencyCtlCtlEntity();
                //        currencyInfo = db.GetEntityByOutsideSql<ComDao.CurrencyCtlCtlEntity>(SqlName.GetTaxRate, SqlName.SubDir,
                //            new { CurrencyCd = vender.CurrencyCd, ExValidDate = validDate });
                //        if (currencyInfo != null)
                //        {
                //            taxRatio = currencyInfo.TaxRatio ?? 0;
                //            return true;
                //        }
                //    }
                //}
                ////// 品目仕様マスタから勘定科目コードを取得し、設定されている場合、税率を取得する
                ////ComDao.ItemSpecificationEntity itemSpecInfo = new ComDao.ItemSpecificationEntity();
                ////itemSpecInfo = itemSpecInfo.GetEntity(paramInfo.ShipHead.ItemCd, paramInfo.ShipHead.SpecificationCd, paramInfo.ShipHead.ItemActiveDate, db);
                ////// 勘定科目コードが取得できた場合、税率マスタから税率を取得
                ////if (!string.IsNullOrEmpty(itemSpecInfo.AccountsCd))
                ////{
                ////    taxRatio = this.getTaxRateFromTaxMaster(validDate, itemSpecInfo.AccountsCd);
                ////    return true;
                ////}
                ////else
                ////{
                ////    // 上記以外、通貨マスタを参照
                ////    if (validDate != null)
                ////    {
                ////        ComDao.CurrencyCtlCtlEntity currencyInfo = new ComDao.CurrencyCtlCtlEntity();
                ////        currencyInfo = db.GetEntityByOutsideSql<ComDao.CurrencyCtlCtlEntity>(SqlName.GetTaxRate, SqlName.SubDir,
                ////            new { CurrencyCd = vender.CurrencyCd, ExValidDate = validDate });
                ////        if (currencyInfo != null)
                ////        {
                ////            taxRatio = currencyInfo.TaxRatio ?? 0;
                ////            return true;
                ////        }
                ////    }
                ////}

            }

            // 処理を返す
            return false;
        }

        /// <summary>
        /// 登録する売上詳細の内容を取得
        /// </summary>
        /// <param name="salesNo">売上No</param>
        /// <param name="paramInfo">取得した情報をまとめているパラメータクラス</param>
        /// <param name="taxDivision">消費税課税区分</param>
        /// <param name="taxRatio">消費税率</param>
        /// <param name="userId">登録ユーザID</param>
        /// <param name="now">システム日時</param>
        /// <returns>売上詳細</returns>
        private ComDao.SalesDetailEntity getRegistSalesDetail(string salesNo, ParamTableInfo paramInfo, int taxDivision, decimal taxRatio, string userId, DateTime now)
        {
            var regist = new ComDao.SalesDetailEntity();
            regist.SalesNo = salesNo;
            regist.SalesRowNo = 1;
            regist.ItemCd = paramInfo.ShipHead.ItemCd;
            regist.ItemActiveDate = paramInfo.ShipHead.ItemActiveDate;
            regist.ItemName = paramInfo.OrderDetail.ItemName;
            regist.SpecificationCd = paramInfo.ShipHead.SpecificationCd;
            var specInfo = new ComDao.ItemSpecificationEntity().GetEntity(paramInfo.ShipHead.ItemCd, paramInfo.ShipHead.SpecificationCd, paramInfo.ShipHead.ItemActiveDate, this.db);
            regist.SpecificationActiveDate = specInfo.SpecificationActiveDate;
            regist.SpecificationName = paramInfo.OrderDetail.SpecificationName;
            regist.SalesQuantity = paramInfo.ShipDetail.SumShippingResultQuantity;
            regist.SalesUnitprice = paramInfo.OrderDetail.OrderUnitprice;
            regist.TmpUnitpriceFlg = paramInfo.OrderDetail.TmpUnitpriceFlg ?? 0;
            regist.InputUserId = userId;
            regist.InputDate = now;
            regist.UpdateUserId = userId;
            regist.UpdateDate = now;

            decimal salesAmount = decimal.Zero;
            decimal taxAmount = decimal.Zero;
            salesAmount = getSalesAmount(paramInfo, taxDivision, taxRatio, ref taxAmount);
            regist.SalesAmount = salesAmount;
            regist.TaxAmount = taxAmount;

            return regist;
        }

        /// <summary>
        /// 売上金額を計算
        /// </summary>
        /// <param name="paramInfo">取得した情報をまとめているパラメータクラス</param>
        /// <param name="taxDivision">消費税課税区分</param>
        /// <param name="taxRatio">消費税率</param>
        /// <param name="taxAmount">消費税額</param>
        /// <returns>売上金額</returns>
        private decimal getSalesAmount(ParamTableInfo paramInfo, int taxDivision, decimal taxRatio, ref decimal taxAmount)
        {
            //// 換算係数と重量を、Nullや0なら1に置き換える
            //if ((paramInfo.ItemInfo.KgOfFractionManagement ?? 0) == 0)
            //{
            //    paramInfo.ItemInfo.KgOfFractionManagement = 1;
            //}
            //if ((paramInfo.ItemInfo.AllUpWeight ?? 0) == 0)
            //{
            //    paramInfo.ItemInfo.AllUpWeight = 1;
            //}
            //// decimalに変換(Nullは上記でありえないので適当)
            //decimal modKgOfFractionManagement = paramInfo.ItemInfo.KgOfFractionManagement ?? -1;
            //decimal modAllUpWeight = paramInfo.ItemInfo.AllUpWeight ?? -1;

            //// 重量計算
            //decimal weight = paramInfo.ShipDetail.SumShippingResultQuantity;
            //if (!paramInfo.ItemInfo.isItemUnitWeight())
            //{
            //    // 単位が重量以外の場合、重量 = 受注数 * 換算係数 * 総重量
            //    weight = weight * modKgOfFractionManagement * modAllUpWeight;
            //}
            //// 取引先別単価マスタを取得
            //bool isExistsUnitPrice = getIsExistsUnitPrice(paramInfo.OrderHead.BalanceCd, paramInfo.ShipHead, paramInfo.ShipDetail);

            decimal salesAmount = 0;
            // 全区分算出
            //// 受注区分が1(通常),2(有償サンプル)なら計算(そうでない場合は0)
            //List<string> orderDivisions = new List<string> { APConsts.ORDER_DIVISION.STANDARD, APConsts.ORDER_DIVISION.PAID_SAMPLE };
            //if (orderDivisions.Contains(paramInfo.OrderHead.OrderDivision.ToString()))
            //{
            // ATS
            // 金額 = 数量(個) * (単価 - 値引)
            salesAmount = paramInfo.ShipDetail.SumShippingResultQuantity * paramInfo.OrderDetail.OrderUnitprice;

            // 丸め処理
            salesAmount = roundUrikingaku(salesAmount, paramInfo.ShipHead.VenderCd, paramInfo.ShipHead.VenderActiveDate);
            //orderDetail.ExRateと掛け算
            salesAmount = salesAmount * convertOrderExRateDecimal(paramInfo.OrderDetail.ExRate);
            //丸め処理
            salesAmount = roundUrikingaku(salesAmount, paramInfo.ShipHead.VenderCd, paramInfo.ShipHead.VenderActiveDate);

                // AKAP
                //if (isExistsUnitPrice)
                //{
                //    // 取引先別単価マスタが取得できた場合、個あたり
                //    if (paramInfo.ItemInfo.isItemUnitWeight())
                //    {
                //        // Kg
                //        // 金額 = (数量(Kg) / 総重量 / 換算係数)  単価
                //        salesAmount = (weight / modAllUpWeight / modKgOfFractionManagement) * paramInfo.OrderDetail.OrderUnitprice;
                //    }
                //    else
                //    {
                //        // 個数
                //        // 金額 = 数量(個) * 単価
                //        salesAmount = paramInfo.ShipDetail.SumShippingResultQuantity * paramInfo.OrderDetail.OrderUnitprice;
                //    }
                //}
                //else
                //{
                //    // 取得できない場合、Kgあたり
                //    if (paramInfo.ItemInfo.isItemUnitWeight())
                //    {
                //        // Kg
                //        // 金額 = 数量(Kg) * 単価
                //        salesAmount = weight * paramInfo.OrderDetail.OrderUnitprice;
                //    }
                //    else
                //    {
                //        // 個
                //        // 金額 = 数量(個) * 換算係数 * 総重量 * 単価
                //        salesAmount = weight * modKgOfFractionManagement * modAllUpWeight * paramInfo.OrderDetail.OrderUnitprice;
                //    }
                //}
            //}

            taxAmount = 0;
            if (APConsts.VENDER.TAX_DIVISION.EXCLUDED.Equals(taxDivision) || APConsts.VENDER.TAX_DIVISION.INCLUDED.Equals(taxDivision))
            {
                // 外税、内税

                // 税計算処理
                if (APConsts.VENDER.TAX_DIVISION.EXCLUDED.Equals(taxDivision))
                {
                    // 金額 * 税率(%の値) / 100
                    taxAmount = salesAmount * taxRatio / 100;
                }
                else
                {
                    // 金額 * 税率(%の値) / (税率(%の値) + 100)
                    taxAmount = salesAmount * taxRatio / (taxRatio + 100);
                }
                //丸め処理
                taxAmount = roundTax(taxAmount, paramInfo.ShipHead.VenderCd, paramInfo.ShipHead.VenderActiveDate);

                if (APConsts.VENDER.TAX_DIVISION.INCLUDED.Equals(taxDivision))
                {
                    // 内税
                    salesAmount -= taxAmount;
                }
            }

            return salesAmount;
        }

        /// <summary>
        /// 取引先別単価マスタが取得可能かを取得
        /// </summary>
        /// <param name="balanceCd">帳合コード</param>
        /// <param name="shipHead">出荷ヘッダ</param>
        /// <param name="shipDetail">出荷詳細</param>
        /// <returns>取得可能ならTrue</returns>
        private bool getIsExistsUnitPrice(string balanceCd, ComDao.ShippingEntity shipHead, DataClass.SumShipDetail shipDetail)
        {
            // 単位区分を取得
            var condUnitPrice = new ComDao.UnitpriceEntity();
            condUnitPrice.BalanceCd = balanceCd;
            condUnitPrice.ItemCd = shipHead.ItemCd;
            condUnitPrice.SpecificationCd = shipHead.SpecificationCd;
            condUnitPrice.QuantityFrom = shipDetail.SumShippingResultQuantity;
            condUnitPrice.QuantityTo = shipDetail.SumShippingResultQuantity;
            condUnitPrice.ValidDate = shipHead.ShippingResultDate ?? DateTime.Now;
            int count = this.db.GetEntityByOutsideSqlByDataClass<int>(SqlName.GetUnitPriceExists, SqlName.SubDir, condUnitPrice);
            // 取得できた場合、個数当たり、出来なかった場合、Kg当たり
            bool isPerCount = count > 0;
            return isPerCount;
        }

        /// <summary>
        /// 売上金額の丸め処理
        /// </summary>
        /// <param name="amount">売上金額</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="activeDate">取引先有効開始日</param>
        /// <returns>丸めた売上金額</returns>
        private decimal roundUrikingaku(decimal amount, string venderCd, DateTime? activeDate)
        {
            return ComUtil.ConvertDecimalToZero(CommonAPUtil.APCheckDigitUtil.APCheckDigitUtil.Format(APConsts.NUMBER_CHKDISIT.URKINGAKU, APConsts.VENDER_DIVISION.TS, venderCd, amount, this.db, activeDate));
            //return FuncUtil.FncCheckRound(amount, "URKINGAKU", "TS", venderCd, null, this.db);
        }

        /// <summary>
        /// 売上金額の丸め処理
        /// </summary>
        /// <param name="amount">売上金額</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="activeDate">取引先有効開始日</param>
        /// <returns>丸めた売上金額</returns>
        private decimal roundTax(decimal amount, string venderCd, DateTime? activeDate)
        {
            return ComUtil.ConvertDecimalToZero(CommonAPUtil.APCheckDigitUtil.APCheckDigitUtil.Format(APConsts.NUMBER_CHKDISIT.TAX_AMOUNT, APConsts.VENDER_DIVISION.TS, venderCd, amount, this.db));
            //return FuncUtil.FncCheckRound(amount, "URKINGAKU", "TS", venderCd, null, this.db);
        }

        /// <summary>
        /// 基準通貨レートがNull許容なのでデフォルト値を設定する処理
        /// </summary>
        /// <param name="orderExRate">受注ヘッダ．基準通貨レート</param>
        /// <returns>Nullならデフォルト値を設定</returns>
        private decimal convertOrderExRateDecimal(decimal? orderExRate)
        {
            // 基準通貨レートのデフォルト値は売上ヘッダによると、1
            return orderExRate ?? 1;
        }

        /// <summary>
        /// 売上ヘッダ/詳細登録処理
        /// </summary>
        /// <typeparam name="T">売上ヘッダ/詳細のデータクラスの型</typeparam>
        /// <param name="isHeader">ヘッダの場合True</param>
        /// <param name="condition">登録するテーブルの内容</param>
        /// <param name="userId">ユーザID</param>
        /// <returns>エラーの場合、False</returns>
        private bool insertSales<T>(bool isHeader, T condition, string userId)
        {
            string insertSql = string.Empty;  // INSERTのSQL
            string proofSql = string.Empty; // プルーフのSQL
            if (isHeader)
            {
                // ヘッダの登録とプルーフのSQL
                insertSql = SqlName.InsertSalesHeader;
                proofSql = SqlName.ProofSalesHeader;
            }
            else
            {
                // 詳細の登録とプルーフのSQL
                insertSql = SqlName.InsertSalesDetail;
                proofSql = SqlName.ProofSalesDetail;
            }
            // INSERT
            int result = this.db.RegistByOutsideSql(insertSql, SqlName.SubDir, condition);
            // エラーの場合
            if (result < 0)
            {
                return false;
            }
            // プルーフ登録
            return APComUtil.CreateProof<T>(this.db, condition, proofSql, userId, APConsts.PROOF_STATUS.NEW);
        }

        /// <summary>
        /// SQLに使用するデータクラス
        /// </summary>
        private class DataClass
        {
            /// <summary>
            /// ProMakeSalesRecord_GetIteminfoの取得結果
            /// </summary>
            public class ItemInfo
            {
                // 品目仕様より取得
                /// <summary>Gets or sets 換算係数(在庫)</summary>
                /// <value>換算係数(在庫)</value>
                public decimal? KgOfFractionManagement { get; set; }
                /// <summary>Gets or sets 運用管理単位</summary>
                /// <value>運用管理単位</value>
                public string UnitOfOperationManagement { get; set; }
                /// <summary>Gets or sets 重量</summary>
                /// <value>重量</value>
                public decimal? AllUpWeight { get; set; }

                // 品目マスタ_販売品扱い属性より取得
                /// <summary>Gets or sets 売上部門コード</summary>
                /// <value>売上部門コード</value>
                public string ArticleSectionCd { get; set; }
                /// <summary>Gets or sets 売上科目コード</summary>
                /// <value>売上科目コード</value>
                public string ArticleAccountsCd { get; set; }

                /// <summary>
                /// 運用管理単位がKgかどうかチェック
                /// </summary>
                /// <returns>true:Kg、false:左記以外</returns>
                public bool IsItemUnitWeight()
                {
                    return this.UnitOfOperationManagement.Equals(APConsts.UNIT_CODE.KG);
                }
            }

            /// <summary>
            /// ProMakeSalesRecord_GetTargetShipingDetailの取得結果
            /// </summary>
            public class SumShipDetail
            {
                /// <summary>Gets or sets 出荷番号</summary>
                /// <value>出荷番号</value>
                public string ShippingNo { get; set; }
                /// <summary>Gets or sets 出荷実績数合計</summary>
                /// <value>出荷実績数合計</value>
                public decimal SumShippingResultQuantity { get; set; }
                /// <summary>Gets or sets 出荷実績数合計</summary>
                /// <value>出荷実績日</value>
                public DateTime MaxShippingResultDate { get; set; }
            }
        }

        /// <summary>
        /// 売上ヘッダ、詳細の内容を取得するのに使用するデータ群
        /// </summary>
        /// <remarks>メソッドの引数にたくさん指定するのが大変なので作成</remarks>
        private class ParamTableInfo
        {
            /// <summary>Gets or sets 出荷ヘッダ</summary>
            /// <value>出荷ヘッダ</value>
            public ComDao.ShippingEntity ShipHead { get; set; }
            /// <summary>Gets or sets 出荷詳細の合計</summary>
            /// <value>出荷詳細の合計</value>
            public DataClass.SumShipDetail ShipDetail { get; set; }
            /// <summary>Gets or sets 受注ヘッダ</summary>
            /// <value>受注ヘッダ</value>
            public ComDao.OrderHeadEntity OrderHead { get; set; }
            /// <summary>Gets or sets 受注詳細</summary>
            /// <value>受注詳細</value>
            public ComDao.OrderDetailEntity OrderDetail { get; set; }
            /// <summary>Gets or sets 請求先コードで取得した取引先</summary>
            /// <value>請求先コードで取得した取引先</value>
            public ComDao.VenderEntity InvoiceVender { get; set; }
            /// <summary>Gets or sets 取得した品目の情報</summary>
            /// <value>取得した品目の情報</value>
            public DataClass.ItemInfo ItemInfo { get; set; }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="inShipHead">出荷ヘッダ</param>
            /// <param name="inShipDetail">出荷詳細</param>
            /// <param name="inOrderHead">受注ヘッダ</param>
            /// <param name="inOrderDetail">受注詳細</param>
            /// <param name="inInvoiceVender">請求先コードで取得した取引先</param>
            /// <param name="inItemInfo">取得した品目の情報</param>
            public ParamTableInfo(ComDao.ShippingEntity inShipHead, DataClass.SumShipDetail inShipDetail,
                ComDao.OrderHeadEntity inOrderHead, ComDao.OrderDetailEntity inOrderDetail,
                ComDao.VenderEntity inInvoiceVender, DataClass.ItemInfo inItemInfo)
            {
                ShipHead = inShipHead;
                ShipDetail = inShipDetail;
                OrderHead = inOrderHead;
                OrderDetail = inOrderDetail;
                InvoiceVender = inInvoiceVender;
                ItemInfo = inItemInfo;
            }
        }

        /// <summary>
        /// 消費税情報
        /// </summary>
        public class TaxEntity
        {
            /// <summary>Gets or sets 消費税率.</summary>
            /// <value>消費税率.</value>
            public decimal TaxRatio { get; set; }

            /// <summary>Gets or sets 消費税区分コード.</summary>
            /// <value>消費税区分コード.</value>
            public string TaxCd { get; set; }
        }
    }
}
