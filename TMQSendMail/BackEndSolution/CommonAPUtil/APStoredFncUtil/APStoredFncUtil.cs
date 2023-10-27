using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonSTDUtil.CommonLogger;

using APComUtil = APCommonUtil.APCommonUtil.APCommonUtil;
using APDao = CommonAPUtil.APCommonUtil.APCommonUtilDataClass;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using EntityDao = CommonAPUtil.APCommonUtil.APCommonDataClass;

namespace CommonAPUtil.APStoredFncUtil
{
    /// <summary>
    /// 共通ファンクションクラス
    /// </summary>
    public class APStoredFncUtil
    {
        #region クラス内変数
        /// <summary>ログ出力</summary>
        private static CommonLogger logger = CommonLogger.GetInstance("logger");
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public APStoredFncUtil()
        {
        }
        #endregion

        /// <summary>
        /// スポット区分を取得
        /// </summary>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specCd">仕様コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得したスポット区分</returns>
        public static int? FncGetSpotDivision(string itemCd, string specCd,
            ComDB db)
        {
            try
            {
                // SQL分生成
                var sql = "";
                sql = sql + "select item_specification.spot_division ";
                sql = sql + "  from item ";
                sql = sql + "       left outer join item_specification on ";
                sql = sql + "           item_specification.item_cd = item.item_cd ";
                sql = sql + "       and item_specification.specification_cd = @SpecificationCd ";
                sql = sql + "       and item_specification.active_date = item.active_date ";
                sql = sql + " where item.item_cd = @ItemCd";

                // SQL実行
                EntityDao.ItemSpecificationEntity results = db.GetEntity<EntityDao.ItemSpecificationEntity>(sql, new { ItemCd = itemCd, SpecificationCd = specCd });
                //int? results = db.GetEntity<int>(sql);
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    //return int.Parse(results.ToString());
                    return results.SpotDivision;
                }

                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 締め処理済みフラグを返す
        /// </summary>
        /// <param name="division">処理区分</param>
        /// <param name="accountYears">締め年月</param>
        /// <param name="procDate">処理日付</param>
        /// <param name="execDate">締め処理日付</param>
        /// <param name="closeDateFrom">締処理対象日付(開始)</param>
        /// <param name="closeDateTo">締処理対象日付(終了)</param>
        /// <param name="type">呼び出し元の種類</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>締め処理フラグ</returns>
        public static int FncGetCloseRsult(string division, string accountYears, object procDate, object execDate, object closeDateFrom, object closeDateTo, string type,
            ComDB db)
        //public int FncGetCloseRsult(string division, string accountYears, DateTime procDate, DateTime execDate, DateTime closeDateFrom, DateTime closeDateTo, string type)
        {
            string accounts = ""; // 勘定年月

            try
            {
                // 勘定年月の設定
                if (procDate != null)
                {
                    // procDateがnullではない場合、その年月を勘定年月とする
                    //accounts = procDate.ToString("yyyyMM");
                    accounts = string.Format("{0:yyyyMM}", procDate);
                }
                else
                {
                    // accountYearsを勘定年月に設定する
                    accounts = accountYears;
                }

                // SQL分生成
                // ★テーブル削除のためコメントアウト
                //var sql = "";
                //sql = sql + "select close_flg ";
                //sql = sql + "  from close_result ";
                //sql = sql + "  where division = '" + division + "'";
                //sql = sql + "  and account_years = '" + accounts + "'";

                //// SQL実行
                //EntityDao.CloseResultEntity results = db.GetEntity<EntityDao.CloseResultEntity>(sql);

                //if (results != null)
                //{
                //    IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                //    return int.Parse(results.CloseFlg.ToString());
                //}

                // 渡した勘定年月と処理区分がCLOSE_RESULTに存在しない場合、未処理として扱う
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 有効在庫チェック *P dicresultを修正する
        /// </summary>
        /// <param name="procDivision">処理区分</param>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specCd">仕様コード</param>
        /// <param name="locationCd">ロケーションコード</param>
        /// <param name="lotNo">ロット番号</param>
        /// <param name="checkQty">比較数量</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:在庫なし 1:在庫あり</returns>
        public static string FncCheckInventoryQty(string procDivision, string itemCd, string specCd, string locationCd, string lotNo, decimal checkQty,
            ComDB db)
        {
            decimal availableQty;
            int cnt = 0;

            try
            {
                // SQL文生成
                var sql = "";
                sql = sql + "select count(*) as cnt ";
                sql = sql + "  from lot_inventory ";
                sql = sql + " where item_cd = @ItemCd";
                sql = sql + "   and spacification_cd = @SpecificationCd";
                sql = sql + "   and location_cd = @LocationCd";
                sql = sql + "   and lot_no = @LotNo";

                // SQL実行
                int results = db.GetEntity<int>(sql, new { ItemCd = itemCd, SpecificationCd = specCd, LocationCd = locationCd, LotNo = lotNo });

                if (results > 0)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    cnt = results;
                }

                // 在庫が足りない
                if (cnt == 0)
                {
                    return "0";
                }

                results = 0;

                // ロット在庫の情報を取得
                sql = "";
                sql = sql + "select lot_inventory.available_qty ";
                sql = sql + " from v_lot_inventory ";
                sql = sql + " where item_cd = @ItemCd";
                sql = sql + "   and spacification_cd = @SpecificationCd";
                sql = sql + "   and location_cd = @LocationCd";
                sql = sql + "   and lot_no = @LotNo";

                // SQL実行
                EntityDao.LotInventoryEntity lotResult = db.GetEntity<EntityDao.LotInventoryEntity>(sql,
                    new { ItemCd = itemCd, SpecificationCd = specCd, LocationCd = locationCd, LotNo = lotNo });

                if (lotResult != null)
                {
                    //IDictionary<string, object> dicResult = lotResult as IDictionary<string, object>;
                    availableQty = decimal.Parse(lotResult.InventoryQty.ToString());

                    // 処理区分によって分岐
                    if (procDivision == "INVENTORY")
                    {
                        // 在庫のみチェックする場合
                        if (checkQty > availableQty)
                        {
                            // 在庫が足りない
                            return "0";
                        }
                        else
                        {
                            // 在庫が足りている
                            return "1";
                        }
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 小数点丸め処理
        /// </summary>
        /// <param name="value">フォーマット対象の数値</param>
        /// <param name="unitDivision">単位区分</param>
        /// <param name="venderDivision">取引先区分</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="roundDivision">強制端数区分(0:なし 1:切り捨て 2:四捨五入 3:切り上げ)</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>丸め処理済みの数値</returns>
        public static decimal FncCheckRound(decimal value, string unitDivision, string venderDivision, string venderCd, string roundDivision,
            ComDB db)
        {
            decimal tempCount = 0;
            string tempNumberChkDisit_UnitDivision;
            string tempNumberChkDisit_VenderDivision;
            string tempNumberChkDisit_VenderCd;
            decimal tempNumberChkDisit_MaxLength;
            decimal tempNumberChkDisit_IntegerLength;
            decimal tempNumberChkDisit_SmallNumLength = 0;
            decimal tempNumberChkDisit_RoundDivision = 0;
            decimal tempNumberChkDisit_LowerLimit;
            decimal tempNumberChkDisit_UpperLimit;
            DateTime tempNumberChkDisit_InputDate;
            string tempNumberChkDisit_InputorCd;
            DateTime tempNumberChkDisit_UpdateDate;
            string tempNumberChkDisit_UpdatorCd;
            decimal tempRound;
            decimal tempUnit;
            decimal tempRetValue;
            string temp = "";

            try
            {
                tempRetValue = value;

                // 数値桁数チェックマスタテーブルを検索
                // SQL文生成
                var sql = "";
                sql = sql + "select count(*) as cnt ";
                sql = sql + "  from number_chkdisit ";
                sql = sql + " where number_chkdisit.unit_division = @UnitDivision";
                sql = sql + "   and number_chkdisit.vender_division = @VenderDivision";
                sql = sql + "   and number_chkdisit.vender_cd = @VenderCd";

                // SQL実行
                int results = db.GetEntity<int>(sql, new { UnitDivision = unitDivision, VenderDivision = venderDivision, VenderCd = venderCd });

                if (results > 0)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    tempCount = results;
                }

                // 取引先データで検索してデータがあるかないかで分岐
                if (tempCount == 0)
                {
                    // データがない場合
                    // 自社データで検索
                    sql = "";
                    sql = sql + "select count(*) as cnt ";
                    sql = sql + "  from number_chkdisit ";
                    sql = sql + " where number_chkdisit.unit_division = @UnitDivision";
                    sql = sql + "   and number_chkdisit.vender_division = ' '";
                    sql = sql + "   and number_chkdisit.vender_cd = ' '";

                    results = 0;
                    // SQL実行
                    results = db.GetEntity<int>(sql, new { UnitDivision = unitDivision });

                    if (results > 0)
                    {
                        //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                        tempCount = results;

                        if (tempCount == 0)
                        {
                            temp = "単位区分:" + unitDivision + " 取引先区分:" + venderDivision + " 取引先コード:" + venderCd + " 数値チェックマスタに存在しません。";
                            return tempRetValue;
                        }
                        else
                        {
                            sql = "";
                            sql = sql + "select number_chkdisit.unit_division ";
                            sql = sql + "      ,number_chkdisit.vender_division ";
                            sql = sql + "      ,number_chkdisit.vender_cd ";
                            sql = sql + "      ,number_chkdisit.max_length ";
                            sql = sql + "      ,number_chkdisit.integer_length ";
                            sql = sql + "      ,number_chkdisit.smallnum_length ";
                            sql = sql + "      ,number_chkdisit.round_division ";
                            sql = sql + "      ,number_chkdisit.lower_limit ";
                            sql = sql + "      ,number_chkdisit.upper_limit ";
                            sql = sql + "      ,number_chkdisit.input_date ";
                            sql = sql + "      ,number_chkdisit.input_user_id ";
                            sql = sql + "      ,number_chkdisit.update_date ";
                            sql = sql + "      ,number_chkdisit.update_user_id ";
                            sql = sql + "  from number_chkdisit ";
                            sql = sql + " where number_chkdisit.unit_division = @UnitDivision";
                            sql = sql + "   and number_chkdisit.vender_division = ' '";
                            sql = sql + "   and number_chkdisit.vender_cd = ' '";

                        }
                    }
                    else
                    {
                        temp = "単位区分:" + unitDivision + " 取引先区分:" + venderDivision + " 取引先コード:" + venderCd + " 数値チェックマスタに存在しません。";
                        return tempRetValue;
                    }
                }
                else
                {
                    // データがある場合
                    sql = "";
                    sql = sql + "select number_chkdisit.unit_division ";
                    sql = sql + "      ,number_chkdisit.vender_division ";
                    sql = sql + "      ,number_chkdisit.vender_cd ";
                    sql = sql + "      ,number_chkdisit.max_length ";
                    sql = sql + "      ,number_chkdisit.integer_length ";
                    sql = sql + "      ,number_chkdisit.smallnum_length ";
                    sql = sql + "      ,number_chkdisit.round_division ";
                    sql = sql + "      ,number_chkdisit.lower_limit ";
                    sql = sql + "      ,number_chkdisit.upper_limit ";
                    sql = sql + "      ,number_chkdisit.input_date ";
                    sql = sql + "      ,number_chkdisit.input_user_id ";
                    sql = sql + "      ,number_chkdisit.update_date ";
                    sql = sql + "      ,number_chkdisit.update_user_id ";
                    sql = sql + "  from number_chkdisit ";
                    sql = sql + " where number_chkdisit.unit_division = @UnitDivision";
                    sql = sql + "   and number_chkdisit.vender_division = ' '";
                    sql = sql + "   and number_chkdisit.vender_cd = ' '";
                }

                results = 0;
                // SQL実行
                EntityDao.NumberChkdisitEntity numResults = db.GetEntity<EntityDao.NumberChkdisitEntity>(sql, new { UnitDivision = unitDivision });

                if (results > 0)
                {
                    // dicresultを変更する
                    //IDictionary<string, object> dicResult = numResults as IDictionary<string, object>;
                    tempNumberChkDisit_UnitDivision = numResults.UnitDivision;
                    tempNumberChkDisit_VenderDivision = numResults.VenderDivision;
                    tempNumberChkDisit_VenderCd = numResults.VenderCd;
                    tempNumberChkDisit_MaxLength = decimal.Parse(numResults.MaxLength.ToString());
                    tempNumberChkDisit_IntegerLength = decimal.Parse(numResults.IntegerLength.ToString());
                    tempNumberChkDisit_SmallNumLength = decimal.Parse(numResults.SmallnumLength.ToString());
                    tempNumberChkDisit_RoundDivision = decimal.Parse(numResults.RoundDivision.ToString());
                    tempNumberChkDisit_LowerLimit = decimal.Parse(numResults.LowerLimit.ToString());
                    tempNumberChkDisit_UpperLimit = decimal.Parse(numResults.UpperLimit.ToString());
                    tempNumberChkDisit_InputDate = (DateTime)numResults.InputDate;
                    tempNumberChkDisit_InputorCd = numResults.InputUserId;
                    tempNumberChkDisit_UpdateDate = (DateTime)numResults.UpdateDate;
                    tempNumberChkDisit_UpdatorCd = numResults.UpdateUserId;

                }

                // 強制端数区分が入力されているかどうかで分岐
                if (roundDivision == null)
                {
                    // 強制端数区分入力無し
                    tempRound = tempNumberChkDisit_RoundDivision;
                }
                else
                {
                    // 強制端数区分入力有
                    tempRound = decimal.Parse(roundDivision);
                }

                // 小数点以下桁数を取得
                tempUnit = tempNumberChkDisit_SmallNumLength;

                tempRetValue = FncRound(value, tempUnit, tempRound);

                return tempRetValue;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 仕訳データチェック
        /// </summary>
        /// <param name="shiwakePtnCd">仕訳パターンコード</param>
        /// <param name="shiwakePtnDiv">仕訳パターン区分</param>
        /// <param name="slipNo">伝票番号(UA1:仕入 UA2:売上 UA3:入金 U4:在庫入出庫)</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常 -1:日付エラー -2:取引先エラー -3:サイトエラー</returns>
        public static int FncCheckShiwakeError(string shiwakePtnCd, string shiwakePtnDiv, string slipNo, ComDB db)
        {
            string accountYearsMst = "";  // 自社マスタの当月勘定年月の値を取得する。
            string accountYears;          // 各伝票の勘定年月
            string venderCd;              // 仕入先/得意先コード
            string paymentInvoiceCd;      // 支払先/請求先コード
            string debitSectionCd;        // 借方部門
            string debitTitleCd;          // 借方科目
            string creditSectionCd;       // 貸方部門
            string creditTitleCd;         // 借方科目
            string venderCdAir = "";      // 仕入先/得意先コード
            int spplSite;                 // 手形サイト
            decimal stockingAmount;       // 仕入金額
            DateTime stockingDate;        // 仕入日付
            string paymentScheDate;       // 支払予定日
            int noteSightDivision = 0;
            int creditDivision = 0;
            //DateTime creditDate;          // 入金日付

            try
            {
                // SQL文生成
                var sql = "";
                sql = sql + "select account_years ";
                sql = sql + "  from company_setting ";
                sql = sql + " where company_setting.company_cd = '000001'";

                // SQL実行
                EntityDao.CompanySettingEntity results = db.GetEntity<EntityDao.CompanySettingEntity>(sql);

                if (results != null)
                {
                    IDictionary<string, object> dicResult = results as IDictionary<string, object>;

                    accountYearsMst = results.AccountYears.ToString();
                }

                accountYears = "200001";
                spplSite = 0;

                // UA1:仕入番号に紐づく部門科目を取得
                if (shiwakePtnCd == "UA1")
                {
                    //sql = "";
                    //sql = sql + "select trn1.account_years ";             // 各伝票の勘定年月
                    //sql = sql + "      ,trn1.vender_cd ";                 // 仕入先/得意先コード
                    //sql = sql + "      ,trn1.payee_cd ";                  // 支払先/請求先コード
                    //sql = sql + "      ,trn1.account_debit_section_cd";   // 借方部門
                    //sql = sql + "      ,trn1.debit_title_cd ";             // 借方科目
                    //sql = sql + "      ,trn1.account_credit_section_cd "; // 貸方部門
                    //sql = sql + "      ,trn1.credit_title_cd ";            // 貸方科目
                    //sql = sql + "      ,trn1.stocking_amount ";            // 仕入金額
                    //sql = sql + "      ,trn1.stocking_date ";              // 仕入日付
                    //sql = sql + "      ,right(air.lifnr, 6) as lifnr ";
                    //sql = sql + "  from purchase_subcontract trn1 ";
                    //sql = sql + "  left join external_if.air_mst_sppl_comp_t air "; //★★[akapif].[dbo].AIR_MST_SPPL_COMP_T
                    //sql = sql + "     on bukrs = ( ";
                    //sql = sql + "            select air_bukrs ";
                    //sql = sql + "              from company ";
                    //sql = sql + "            ) ";
                    //sql = sql + "       and lifnr like '1351%' ";
                    //sql = sql + "       and trn1.vender_cd = right(lifnr, 6) ";
                    //sql = sql + " where trn1.slip_no = " + slipNo;
                    //sql = sql + "   and trn1.slip_row_no = " + slipRowNo;

                    sql = "";
                    sql = sql + "select trn1.account_years ";             // 各伝票の勘定年月
                    sql = sql + "      ,trn1.vender_cd ";                 // 仕入先/得意先コード
                    sql = sql + "      ,trn1.supplier_cd ";                  // 支払先/請求先コード
                    sql = sql + "      ,trn1.account_debit_section_cd";   // 借方部門
                    sql = sql + "      ,trn1.debit_title_cd ";             // 借方科目
                    sql = sql + "      ,trn1.account_credit_section_cd "; // 貸方部門
                    sql = sql + "      ,trn1.credit_title_cd ";            // 貸方科目
                    sql = sql + "      ,trn1.stocking_amount ";            // 仕入金額
                    sql = sql + "      ,trn1.stocking_date ";              // 仕入日付
                    sql = sql + "      ,right(air.lifnr, 6) as lifnr ";
                    sql = sql + "  from proof.stocking_proof trn1   ";
                    sql = sql + "left join external_if.air_mst_sppl_comp_t air      ";
                    sql = sql + "       on bukrs = (select air_bukrs from company )";
                    sql = sql + "       and lifnr like '1351%'";
                    sql = sql + " where trn1.stocking_no = @StockingNo";
                    //sql = sql + "   and trn1.stocking_row_no = " + slipRowNo;

                    results = null;
                    // SQL実行
                    EntityDao.PurchaseSubcontractEntity purchaseResults = db.GetEntity<EntityDao.PurchaseSubcontractEntity>(sql, new { StockingNo = slipNo });

                    if (results != null)
                    {
                        IDictionary<string, object> dicResult = purchaseResults as IDictionary<string, object>;

                        accountYears = purchaseResults.AccountYears.ToString();                  // 各伝票の勘定年月
                        venderCd = purchaseResults.VenderCd;                                     // 仕入先/得意先コード
                        paymentInvoiceCd = purchaseResults.PayeeCd;                              // 支払先/請求先コード
                        debitSectionCd = purchaseResults.AccountDebitSectionCd;                  // 借方部門
                        debitTitleCd = purchaseResults.DebitTitleCd;                             // 借方科目
                        creditSectionCd = purchaseResults.AccountCreditSectionCd;                // 貸方部門
                        creditTitleCd = purchaseResults.CreditSubTitleCd;                        // 貸方科目

                        if (dicResult["lifnr"] == null || dicResult["lifnr"].ToString().Length == 0)
                        {
                            venderCdAir = "999999";                                              // 仕入先/得意先コード

                        }
                        else
                        {
                            venderCdAir = dicResult["lifnr"].ToString();                         // 仕入先/得意先コード
                        }
                        stockingAmount = decimal.Parse(purchaseResults.StockingAmount.ToString()); // 仕入金額
                        stockingDate = DateTime.Parse(purchaseResults.StockingDate.ToString());    // 仕入日付
                        paymentScheDate = FunGetPaymentScheDate(venderCd, string.Format("{0:yyyyMMdd}", stockingDate), stockingAmount, db);  // 支払予定日
                        spplSite = FunGetSite("SI", venderCd, stockingAmount, DateTime.Parse(paymentScheDate), out noteSightDivision, out creditDivision, db);  // 手形サイト
                    }
                }

                // UA2:売上番号に紐づく部門科目を取得
                if (shiwakePtnCd == "UA2")
                {
                    //sql = "";
                    //sql = sql + "select trn1.account_years ";              // 各伝票の勘定年月
                    //sql = sql + "      ,trn1.vender_cd ";                  // 仕入先/得意先コード
                    //sql = sql + "      ,trn1.invoice_cd ";                 // 支払先/請求先コード
                    //sql = sql + "      ,trn1.account_debit_section_cd";    // 借方部門
                    //sql = sql + "      ,trn1.debit_title_cd ";             // 借方科目
                    //sql = sql + "      ,trn1.account_credit_section_cd ";  // 貸方部門
                    //sql = sql + "      ,trn1.credit_title_cd ";            // 貸方科目
                    //sql = sql + "      ,right(air.kunnr, 6) as kunnr ";
                    //sql = sql + "  from sales trn1 ";
                    //sql = sql + "  left join external_if.air_mst_cust_comp_t air "; //★★[akapif].[dbo].AIR_MST_CUST_COMP_T
                    //sql = sql + "     on bukrs = ( ";
                    //sql = sql + "            select air_bukrs ";
                    //sql = sql + "              from company ";
                    //sql = sql + "            ) ";
                    //sql = sql + "       and kunnr like '1300%' ";
                    //sql = sql + "       and trn1.vender_cd = right(kunnr, 6) ";
                    //sql = sql + " where trn1.sales_no = " + slipNo;
                    //sql = sql + "   and trn1.row_no = " + slipRowNo;

                    sql = "";
                    sql = sql + "select trn1.account_years";                         // 各伝票の勘定年月
                    sql = sql + "      ,trn1.vender_cd";                             // 仕入先/得意先コード
                    sql = sql + "      ,trn1.invoice_cd";                            // 支払先/請求先コード
                    sql = sql + "      ,trn1.account_debit_section_cd";              // 借方部門
                    sql = sql + "      ,trn1.debit_title_cd";                        // 借方科目
                    sql = sql + "      ,trn1.account_credit_section_cd";             // 貸方部門
                    sql = sql + "      ,trn1.credit_title_cd";                       // 貸方科目
                    sql = sql + "      ,right(air.kunnr, 6) as kunnr";
                    sql = sql + "  from proof.sales_header_proof trn1   ";
                    sql = sql + "left join external_if.air_mst_cust_comp_t air      ";
                    sql = sql + "       on bukrs = (select air_bukrs from company )";
                    sql = sql + "       and kunnr like '1300%'";
                    //sql = sql + "left join proof.sales_detail_proof trn2 ";
                    //sql = sql + "       on trn1.vender_cd = right(kunnr, 6)";
                    //sql = sql + "      and trn2.sales_row_no = "+ slipRowNo;
                    sql = sql + " where trn1.sales_no = @SalesNo";

                    results = null;
                    // SQL実行
                    EntityDao.SalesEntity salesResults = db.GetEntity<EntityDao.SalesEntity>(sql, new { SalesNo = slipNo });

                    if (results != null)
                    {
                        IDictionary<string, object> dicResult = salesResults as IDictionary<string, object>;

                        accountYears = salesResults.AccountYears.ToString();            // 各伝票の勘定年月
                        venderCd = salesResults.VenderCd.ToString();                        // 仕入先/得意先コード
                        paymentInvoiceCd = salesResults.InvoiceCd;                   // 支払先/請求先コード
                        debitSectionCd = salesResults.AccountDebitSectionCd;       // 借方部門
                        debitTitleCd = salesResults.DebitTitleCd;                   // 借方科目
                        creditSectionCd = salesResults.AccountCreditSectionCd;     // 貸方部門
                        creditTitleCd = salesResults.CreditTitleCd;                 // 貸方科目
                        if (dicResult["kunnr"] == null || dicResult["kunnr"].ToString().Length == 0)
                        {
                            venderCdAir = "999999";                                              // 仕入先/得意先コード

                        }
                        else
                        {
                            venderCdAir = dicResult["kunnr"].ToString();                         // 仕入先/得意先コード
                        }
                    }
                }

                // UA3:入金番号に紐づく部門科目を取得
                if (shiwakePtnCd == "UA3")
                {
                    //sql = "";
                    //sql = sql + "select trn1.credit_date ";                // 各伝票の勘定年月
                    //sql = sql + "      ,trn1.vender_cd ";                  // 仕入先/得意先コード, 支払先/請求先コード
                    //sql = sql + "      ,trn1.debit_section_cd";            // 借方部門
                    //sql = sql + "      ,trn1.debit_title_cd ";             // 借方科目
                    //sql = sql + "      ,trn1.credit_section_cd ";          // 貸方部門
                    //sql = sql + "      ,trn1.credit_title_cd ";            // 貸方科目
                    //sql = sql + "      ,right(air.kunnr, 6) as kunnr ";
                    //sql = sql + "  from credit trn1 ";
                    //sql = sql + "  left join external_if.air_mst_cust_comp_t air "; //★★[akapif].[dbo].AIR_MST_CUST_COMP_T
                    //sql = sql + "     on bukrs = ( ";
                    //sql = sql + "            select air_bukrs ";
                    //sql = sql + "              from company ";
                    //sql = sql + "            ) ";
                    //sql = sql + "       and kunnr like '1300%' ";
                    //sql = sql + "       and trn1.vender_cd = right(kunnr, 6) ";
                    //sql = sql + " where trn1.sales_no = '" + slipNo + "'";
                    //sql = sql + "   and trn1.row_no = " + slipRowNo;

                    sql = "";
                    sql = sql + "select trn1.credit_date ";                // 各伝票の勘定年月
                    sql = sql + "      ,trn1.vender_cd ";                  // 仕入先/得意先コード, 支払先/請求先コード
                    sql = sql + "      ,trn1.debit_section_cd";            // 借方部門
                    sql = sql + "      ,trn1.debit_title_cd ";             // 借方科目
                    sql = sql + "      ,trn1.credit_section_cd ";          // 貸方部門
                    sql = sql + "      ,trn1.credit_title_cd ";            // 貸方科目
                    sql = sql + "      ,right(air.kunnr, 6) as kunnr ";
                    sql = sql + "  from proof.credit_proof trn1   ";
                    sql = sql + "left join external_if.air_mst_cust_comp_t air      ";
                    sql = sql + "       on bukrs = (select air_bukrs from company )";
                    sql = sql + "       and kunnr like '1300%'";
                    sql = sql + "       and trn1.vender_cd = right(kunnr, 6)";
                    sql = sql + " where trn1.credit_no = @CreditNo";
                    //sql = sql + "   and trn1.credit_row_no = " + slipRowNo;

                    results = null;
                    // SQL実行
                    EntityDao.CreditEntity creditResult = db.GetEntity<EntityDao.CreditEntity>(sql, new { CreditNo = slipNo });

                    if (results != null)
                    {
                        IDictionary<string, object> dicResult = creditResult as IDictionary<string, object>;

                        accountYears = string.Format("{0:yyyyMM}", creditResult.CreditDate.ToString());                    // 各伝票の勘定年月
                        venderCd = creditResult.VenderCd;                            // 仕入先/得意先コード
                        paymentInvoiceCd = creditResult.VenderCd;                    // 支払先/請求先コード
                        debitSectionCd = creditResult.DebitSectionCd;               // 借方部門
                        debitTitleCd = creditResult.DebitTitleCd;                   // 借方科目
                        creditSectionCd = creditResult.CreditSectionCd;             // 貸方部門
                        creditTitleCd = creditResult.CreditTitleCd;                 // 貸方科目
                        if (dicResult["kunnr"] == null || dicResult["kunnr"].ToString().Length == 0)
                        {
                            venderCdAir = "999999";                                              // 仕入先/得意先コード

                        }
                        else
                        {
                            venderCdAir = dicResult["kunnr"].ToString();                         // 仕入先/得意先コード
                        }
                    }
                }

                // エラー判定ブロック
                if (accountYears.CompareTo(accountYearsMst) < 0)
                {
                    return -1;
                }
                if (venderCdAir == "999999")
                {
                    return -2;
                }
                // サイト取得不可の場合エラー
                if (spplSite == 999)
                {
                    return -3;
                }

                return 0;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 数値の端数処理を行う
        /// </summary>
        /// <param name="qty">端数処理を行う数値</param>
        /// <param name="fractionPoint">端数処理を行う場所</param>
        /// <param name="fractionDivision">端数処理を行う区分(TRUNC：切り捨て ROUNDUP：切り上げ ROUNDS：四捨五入)</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>端数処理を行った数値を返す</returns>
        public static decimal FncFractionProc(decimal qty, int fractionPoint, string fractionDivision,
            ComDB db)
        {
            decimal ret;
            decimal ope;

            try
            {
                // 処理区分で処理を分岐
                if (fractionDivision == "TRUNC")
                {
                    // 切捨　処理を行う
                    ope = decimal.Parse(Math.Pow(10, fractionPoint).ToString());
                    ret = Math.Floor(qty * ope) / ope;
                }
                else if (fractionDivision == "ROUNDUP")
                {
                    // 切上　処理を行う
                    ope = decimal.Parse(Math.Pow(10, fractionPoint).ToString());
                    ret = Math.Ceiling(qty * ope) / ope;
                }
                else if (fractionDivision == "ROUNDS")
                {
                    // 四捨五入の場合
                    ret = Math.Round(qty, fractionPoint, MidpointRounding.AwayFromZero);
                }
                else
                {
                    // それ以外の場合そのまま返す
                    ret = qty;
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 運送会社略称を取得する
        /// </summary>
        /// <param name="keyCd">運送会社コード</param>
        /// <param name="languageId">言語コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得した運送会社略称</returns>
        public static string FncGetAbbreviation(string keyCd, string languageId,
            ComDB db)
        {
            string nameValue = "";

            try
            {
                var sql = "";
                sql = sql + "select abbreviation ";
                sql = sql + "  from v_carry ";
                sql = sql + " where language_id = @LanguageId";
                sql = sql + "   and carry_cd = @CarryCd";

                // SQL実行
                EntityDao.CarryEntity carryResults = db.GetEntity<EntityDao.CarryEntity>(sql, new { LanguageId = languageId, CarryCd = keyCd });

                if (carryResults != null)
                {
                    // IDictionary<string, object> dicResult = carryResults as IDictionary<string, object>;

                    nameValue = carryResults.Abbreviation;
                }

                return nameValue;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 文字列をSQL中で使用可能な日付に変換する処理
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>SQLで使用可能な日付の文字列</returns>
        private static string getSqlDate(DateTime date)
        {
            return " to_timestamp('" + date + "','YYYY-MM-DD HH24:MI:SS') ";
        }

        /// <summary>
        /// 勘定年月を取得する処理
        /// </summary>
        /// <param name="orderNo">受注番号</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="venderActiveDate">取引先開始有効日</param>
        /// <param name="salesDate">売上日</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>勘定年月</returns>
        public static string FncGetAccountYears(string orderNo, string venderCd, DateTime venderActiveDate, string salesDate,
            ComDB db)
        {
            DateTime tempAccountDate = DateTime.Now;   // 納入予定日
            string tempAccountYears;                   // 勘定年月
            decimal tempCount = 0;                     // カウンタ
            DateTime deliveryExpectedDate;             // 受注ヘッダ
            decimal tempVender_ClosingDate = 0;
            string tempVender_PaymentInvoiceCd = null;
            string tempPayment_InvoiceCd;
            var sql = "";
            dynamic results;

            try
            {
                if ((venderCd == null) || (venderCd.Length <= 0))
                {
                    // 請求先コードがない場合
                    return null;
                }
                else
                {
                    // 請求先がある場合
                    sql = "";
                    sql = sql + "select count(*) as cnt ";
                    sql = sql + "  from vender ";
                    sql = sql + " where vender.vender_cd = @VenderCd";
                    sql = sql + "   and vender.vender_division = 'TS' ";
                    sql = sql + "   and vender.active_date = " + getSqlDate(venderActiveDate);

                    // SQL実行
                    results = db.GetEntity<int>(sql, new { VenderCd = venderCd });

                    if (results > 0)
                    {
                        //  IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                        tempCount = results;
                    }

                    if (tempCount == 0)
                    {
                        return null;
                    }
                    else
                    {
                        results = 0;
                        sql = "";
                        sql = sql + "select vender.closing_date ";
                        sql = sql + "      ,vender.payment_invoice_cd ";
                        sql = sql + "  from vender ";
                        sql = sql + " where vender.vender_cd = @VenderCd";
                        sql = sql + "   and vender.vender_division = 'TS' ";
                        sql = sql + "   and vender.active_date = " + getSqlDate(venderActiveDate);

                        // SQL実行
                        EntityDao.VenderEntity venderResults = db.GetEntity<EntityDao.VenderEntity>(sql, new { VenderCd = venderCd });

                        if (venderResults != null)
                        {
                            //IDictionary<string, object> dicResult = venderResults as IDictionary<string, object>;
                            tempVender_ClosingDate = decimal.Parse(venderResults.ClosingDate.ToString());
                            tempPayment_InvoiceCd = venderResults.PaymentInvoiceCd;
                        }

                        if (tempVender_PaymentInvoiceCd == null)
                        {
                            // 請求先コードがない場合得意先コードとする
                            tempPayment_InvoiceCd = venderCd;
                        }
                        else
                        {
                            tempPayment_InvoiceCd = tempVender_PaymentInvoiceCd;

                            // 請求先の情報を取得(必要なのは開始有効日なので言語は関係ない)
                            var invoiceVender = APComUtil.GetVenderInfoByActiveDate<ComDao.VenderEntity>(APConstants.APConstants.VENDER_DIVISION.TS, tempPayment_InvoiceCd, DateTime.Now, "ja", db);

                            results = 0;
                            tempCount = 0;
                            sql = "";
                            sql = sql + "select count(*) as cnt ";
                            sql = sql + "  from vender ";
                            sql = sql + " where vender.vender_cd = @VenderCd";
                            sql = sql + "   and vender.vender_division = 'TS' ";
                            sql = sql + "   and vender.active_date = " + getSqlDate(invoiceVender.ActiveDate);

                            // SQL実行
                            results = db.GetEntity<int>(sql, new { VenderCd = tempPayment_InvoiceCd });

                            if (results > 0)
                            {
                                //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                                tempCount = results;
                            }
                            if (tempCount == 0)
                            {
                                return null;
                            }
                            else
                            {
                                venderResults = null;
                                sql = "";
                                sql = sql + "select vender.closing_date ";
                                sql = sql + "      ,vender.payment_invoice_cd ";
                                sql = sql + "  from vender ";
                                sql = sql + " where vender.vender_cd = @VenderCd";
                                sql = sql + "   and vender.vender_division = 'TS' ";
                                sql = sql + "   and vender.active_date = " + getSqlDate(invoiceVender.ActiveDate);

                                // SQL実行
                                venderResults = db.GetEntity<EntityDao.VenderEntity>(sql, new { VenderCd = tempPayment_InvoiceCd });

                                if (venderResults != null)
                                {
                                    //IDictionary<string, object> dicResult = venderResults as IDictionary<string, object>;
                                    tempVender_ClosingDate = decimal.Parse(venderResults.ClosingDate.ToString());
                                    tempPayment_InvoiceCd = venderResults.PaymentInvoiceCd;
                                }
                            }
                        }
                    }
                }

                // 受注番号の有無で分岐
                if (orderNo != null)
                {
                    // 受注番号が有る場合納入予定日を取得
                    // 受注トランザクションがない場合
                    tempCount = 0;
                    results = null;
                    sql = "";
                    sql = sql + "select count(*) as cnt ";
                    sql = sql + "  from order_head ";
                    sql = sql + " where order_head.order_no = @OrderNo ";

                    // SQL実行
                    results = db.GetEntity<int>(sql, new { OrderNo = orderNo });

                    if (results > 0)
                    {
                        // IDictionary<string, object> dicResult = results as IDictionary<string, object>;

                        tempCount = results;
                    }
                    if (tempCount == 0)
                    {
                        return null;
                    }
                    else
                    {

                        sql = "";
                        sql = sql + "select order_head.delivery_expected_date ";
                        sql = sql + "  from order_head ";
                        sql = sql + " where order_head.order_no = @OrderNo ";

                        // SQL実行
                        EntityDao.OrderHeadEntity orderResults = db.GetEntity<EntityDao.OrderHeadEntity>(sql, new { OrderNo = orderNo });
                        if (orderResults != null)
                        {
                            //IDictionary<string, object> dicResult = orderResults as IDictionary<string, object>;
                            deliveryExpectedDate = DateTime.Parse(orderResults.DeliveryExpectedDate.ToString());

                            // 納入予定日を計算
                            tempAccountDate = deliveryExpectedDate;
                        }

                    }
                }
                else
                {
                    // 受注番号がない場合(売上新規)
                    // 売上日がない場合
                    if (salesDate == null)
                    {
                        return null;
                    }
                    else
                    {
                        // 売上日の入力フォーマットにより分岐
                        if (salesDate.Length == 6)
                        {
                            // YYMMDDで入力した場合
                            tempAccountDate = DateTime.Parse("20" + salesDate.Substring(0, 2) + "/" + salesDate.Substring(2, 2) + "/" + salesDate.Substring(4, 2)); // 20を頭に付ける
                        }
                        else
                        {

                            tempAccountDate = DateTime.Parse(salesDate);
                        }
                    }
                }

                // 締め日がおかしい
                if (tempVender_ClosingDate <= 0)
                {
                    return null;
                }

                // 締日の設定で分岐
                if (tempVender_ClosingDate == 99)
                {
                    // 請求先の締日が月末(=99)の時、納入予定年月（出荷予定日+ﾘｰﾄﾞﾀｲﾑ）をｾｯﾄ。
                    tempAccountYears = tempAccountDate.ToString("yyyyMM");
                }
                else
                {
                    // 納入予定日（出荷予定日+ﾘｰﾄﾞﾀｲﾑ）≦締日　ならば　納入予定年月をｾｯﾄ。
                    //if (string.Format("{0:'dd", tempAccountDate).CompareTo(tempVender_ClosingDate) <= 0)
                    if (tempAccountDate.Day <= tempVender_ClosingDate)
                    {
                        tempAccountYears = tempAccountDate.ToString("yyyyMM");
                    }
                    else
                    {
                        tempAccountYears = tempAccountDate.AddMonths(1).ToString("yyyyMM");
                    }
                }

                return tempAccountYears;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 文字列追加処理
        /// </summary>
        /// <param name="addChar">発番時に付ける文字列</param>
        /// <param name="nowVal">発番した番号</param>
        /// <param name="maxVal">発番の最大値</param>
        /// <param name="seqDate">連番取得日付</param>
        /// <returns>処理後の文字列</returns>
        public static string FncGetAddCharProc(string addChar, decimal nowVal, decimal maxVal, DateTime? seqDate)
        {
            string retValue = ""; // 戻り値
            string strNowVal = "";
            string strMaxVal = "";

            try
            {
                // 設定した文字列毎に処理を分岐
                if (addChar.StartsWith("*") && addChar.EndsWith("*"))
                {

                    // *でくくられている場合特殊処理を行う
                    if (addChar == "*SEQ*")
                    {
                        // 連番の場合
                        // 連番を取得 最大値までゼロで埋めて取得する
                        strNowVal = nowVal.ToString();
                        strMaxVal = maxVal.ToString();
                        retValue = nowVal.ToString().PadLeft(strMaxVal.Length, '0');

                        // そのほかがある場合ここに記載する
                    }
                    else
                    {
                        // 連番以外は日付の場合
                        retValue = string.Format("{0:" + addChar.Replace("*", "") + "}", seqDate);
                    }
                }
                else
                {
                    // 上記以外の場合はそのまま返す
                    retValue = addChar;
                }
                return retValue;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 品目毎の有効な単価を取得する
        /// </summary>
        /// <param name="procDivision">処理区分(ORDER:受注と売上 PURCHASE:発注と仕入)</param>
        /// <param name="procDate">処理日(受注日、売上日、発注日、仕入日を設定)</param>
        /// <param name="procAmount">金額</param>
        /// <returns>1:次承認のみ 2:2次承認必要</returns>
        public static string FncGetApprovalRank(string procDivision, string procDate, decimal procAmount)
        {
            return "0";
        }

        /// <summary>
        /// 入金の締め処理済みフラグを返す
        /// </summary>
        /// <param name="creditNo">入金番号</param>
        /// <param name="creditDate">勘定年月</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>締め処理済みフラグ(0:正常 -1:月次/買掛の両方締め処理済み -2:月次締め処理済み -3:買掛締め処理済み -9:その他エラー)</returns>
        public static int FncGetCloseResultCredit(string creditNo, DateTime creditDate,
            ComDB db)
        {
            int inoutCloseFlg; // 月次締処理済フラグ
            int depositCloseFlg; // 売掛締処理済フラグ
            int closeFlg;        // 月次・買掛の両方を考慮した締処理フラグ

            try
            {
                // 入金日の年月が月次更新処理済みかフラグ取得
                inoutCloseFlg = FncGetCloseRsult("M_INOUT", null, creditDate, null, null, null, "CREDIT", db);
                // 入金日の年月が売掛更新処理済みかフラグ取得
                depositCloseFlg = FncGetCloseRsult("DEPOSIT", null, creditDate, null, null, null, "CREDIT", db);

                if (inoutCloseFlg == 0 && depositCloseFlg == 0)
                {
                    // 月次・売掛ともに未処理の場合正常終了
                    closeFlg = 0;
                }
                else if (inoutCloseFlg == 1 && depositCloseFlg == 1)
                {
                    // 月次・売掛ともに処理済みの場合
                    closeFlg = -1;
                }
                else if (inoutCloseFlg == 1)
                {
                    // 月次のみ処理済みの場合
                    closeFlg = -2;
                }
                else if (depositCloseFlg == 1)
                {
                    // 売掛のみ処理済みの場合
                    closeFlg = -3;
                }
                else
                {
                    // その他のエラーの場合
                    closeFlg = -9;
                }

                return closeFlg;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 受入の締め処理済みフラグを返す
        /// </summary>
        /// <param name="purchaseNo">購入番号</param>
        /// <param name="purchaseDate">受入日</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>締め処理済みフラグ(0:正常 -1:月次/買掛の両方締め処理済み -2:月次締め処理済み -3:買掛締め処理済み -9:その他エラー)</returns>
        public static int FncGetCloseResultPurchase(string purchaseNo, DateTime purchaseDate,
            ComDB db)
        {
            int inoutCloseFlg; // 月次締処理済フラグ
            int paymentCloseFlg; // 買掛処理済フラグ
            int closeFlg;        // 月次・買掛の両方を考慮した締処理フラグ

            try
            {
                // 受入日・仕入日の年月が月次更新処理済みかフラグ取得
                inoutCloseFlg = FncGetCloseRsult("M_INOUT", null, purchaseDate, null, null, null, "PURCHASE", db);
                // 受入日・仕入日の年月が買掛更新処理済みかフラグ取得
                paymentCloseFlg = FncGetCloseRsult("PAYABLE", null, purchaseDate, null, null, null, "PURCHASE", db);

                if (inoutCloseFlg == 0 && paymentCloseFlg == 0)
                {
                    // 月次・買掛ともに未処理の場合正常終了
                    closeFlg = 0;
                }
                else if (inoutCloseFlg == 1 && paymentCloseFlg == 1)
                {
                    // 月次・買掛ともに処理済みの場合
                    closeFlg = -1;
                }
                else if (inoutCloseFlg == 1)
                {
                    // 月次のみ処理済みの場合
                    closeFlg = -2;
                }
                else if (paymentCloseFlg == 1)
                {
                    // 買掛のみ処理済みの場合
                    closeFlg = -3;
                }
                else
                {
                    // その他のエラーの場合
                    closeFlg = -9;
                }

                return closeFlg;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 売上の締め処理済みフラグを返す
        /// </summary>
        /// <param name="saleNo">売上番号</param>
        /// <param name="accountYears">勘定年月</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>締め処理済みフラグ(0:正常 -1:月次/売掛の両方締め処理済み -2:月次締め処理済み -3:売掛締め処理済み -9:その他エラー)</returns>
        public static int FncGetCloseResultSales(string saleNo, string accountYears,
            ComDB db)
        {
            int inoutCloseFlg;   // 月次締処理済フラグ
            int depositCloseFlg; // 売掛締処理済フラグ
            int closeFlg;        // 月次・売掛の両方を考慮した締処理フラグ

            try
            {
                // 出荷実績日の年月が月次更新処理済みかフラグ取得
                inoutCloseFlg = FncGetCloseRsult("M_INOUT", accountYears, null, null, null, null, "SALES", db);
                // 出荷実績日の年月が売掛更新処理済みかフラグ取得
                depositCloseFlg = FncGetCloseRsult("DEPOSIT", accountYears, null, null, null, null, "SALES", db);

                if (inoutCloseFlg == 0 && depositCloseFlg == 0)
                {
                    // 月次・売掛ともに未処理の場合正常終了
                    closeFlg = 0;
                }
                else if (inoutCloseFlg == 1 && depositCloseFlg == 1)
                {
                    // 月次・売掛ともに処理済みの場合
                    closeFlg = -1;
                }
                else if (inoutCloseFlg == 1)
                {
                    // 月次のみ処理済みの場合
                    closeFlg = -2;
                }
                else if (depositCloseFlg == 1)
                {
                    // 売掛のみ処理済みの場合
                    closeFlg = -3;
                }
                else
                {
                    // その他のエラーの場合
                    closeFlg = -9;
                }

                return closeFlg;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 出荷実績の締め処理済みフラグを返す
        /// </summary>
        /// <param name="shippingNo">出荷番号</param>
        /// <param name="shippingResultDate">出荷完了日</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>締め処理済みフラグ(0:正常 -1:月次/売掛の両方締め処理済み -2:月次締め処理済み -3:売掛締め処理済み -9:その他エラー)</returns>
        public static int FncGetCloseResultShipping(string shippingNo, DateTime shippingResultDate,
            ComDB db)
        {
            int inoutCloseFlg;   // 月次締処理フラグ
            int depositCloseFlg; // 売掛締処理フラグ
            int closeFlg;        // 月次・売掛の両方を考慮した締処理済フラグ

            try
            {
                // 出荷実績日の年月が月次更新処理済みかフラグ取得
                inoutCloseFlg = FncGetCloseRsult("M_INOUT", null, shippingResultDate, null, null, null, "SHIPPING", db);
                // 出荷実績日の年月が売掛更新処理済みかフラグ取得
                depositCloseFlg = FncGetCloseRsult("DEPOSIT", null, shippingResultDate, null, null, null, "SHIPPING", db);

                if (inoutCloseFlg == 0 && depositCloseFlg == 0)
                {
                    // 月次・売掛ともに未処理の場合正常終了
                    closeFlg = 0;
                }
                else if (inoutCloseFlg == 1 && depositCloseFlg == 1)
                {
                    // 月次・売掛ともに処理済みの場合
                    closeFlg = -1;
                }
                else if (inoutCloseFlg == 1)
                {
                    // 月次のみ処理済みの場合
                    closeFlg = -2;
                }
                else if (depositCloseFlg == 1)
                {
                    // 売掛のみ処理済みの場合
                    closeFlg = -3;
                }
                else
                {
                    // その他のエラーの場合
                    closeFlg = -9;
                }

                return closeFlg;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 入金番号が既に取消処理済みかチェックする
        /// </summary>
        /// <param name="creditNo">入金番号</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>存在チェックの結果を返す 0:正常 -1:異常</returns>
        public static int FncCheckCanselCredit(string creditNo,
            ComDB db)
        {
            int returnValue;
            int cnt = 0;
            try
            {
                // ★★入金の履歴テーブル作成予定。履歴テーブルの入金番号に取消した入金番号があるかどうかチェック※※
                var sql = "";
                sql = sql + "select count(*) as cnt ";
                sql = sql + "  from credit ";
                sql = sql + " where credit.credit_no = @CreditNo ";
              //  sql = sql + "   and vender_cd = '" + pVenderCd + "' ";
                // SQL実行
                int results = db.GetEntity<int>(sql, new { CreditNo = creditNo });
                if (results > 0)
                {
                    cnt = int.Parse(results.ToString());
                }
                // 0件(取消入金番号に存在しない)なら正常、それ以外は異常(取消不可)
                if (cnt == 0)
                {
                    returnValue = 0;
                }
                else
                {
                    returnValue = -1;
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 製造リードタイム先の日付の計算を行う
        /// </summary>
        /// <param name="baseDate">日付</param>
        /// <param name="interval">インターバル</param>
        /// <param name="companyCd">会社コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>リードタイム先の日付</returns>
        public static object FncGetDateadd(DateTime baseDate, int interval, string companyCd,
            ComDB db)
        {
            object returnValue = null;
            string workCalNo = "";
            var sql = "";
            EntityDao.CalEntity results;

            try
            {
                // ※生産計画ではカレンダー考慮なし
                if (companyCd != null)
                {
                    sql = "";
                    sql = sql + "select cal_cd ";
                    sql = sql + "  from company ";
                    sql = sql + " where company_cd = @CompanyCd";

                    // SQL実行
                    results = db.GetEntity<EntityDao.CalEntity>(sql, new { CompanyCd = companyCd });

                    if (results != null)
                    {
                        // IDictionary<string, object> dicResult = results as IDictionary<string, object>;

                        workCalNo = results.CalCd;
                    }
                }

                if (workCalNo == "")
                {
                    // カレンダーコード未設定の場合、カレンダーなしの日付計算
                    returnValue = baseDate.AddDays(interval);
                }
                else
                {
                    results = null;
                    // カレンダーコード設定の場合、カレンダー考慮した日付計算
                    if (interval > 0)
                    {
                        // インターバルがプラスの場合、未来の日付を取得
                        sql = "";
                        sql = sql + "select max(aa.cal_date) as caldate ";
                        sql = sql + "  from ( ";
                        sql = sql + "      select cal_date, row_number() over (order by cal_date) as rownum ";
                        sql = sql + "        from cal ";
                        sql = sql + "       where cal_cd = @CalCd";
                        sql = sql + "         and cal_date > @CalDate";
                        sql = sql + "   and cal_holiday = 0 "; // 0:平日
                        sql = sql + " ) as aa ";
                        sql = sql + " where aa.rownum <= " + interval;

                        // SQL実行
                        results = db.GetEntity<EntityDao.CalEntity>(sql, new { CalCd = workCalNo, CalDate = baseDate });
                    }
                    else
                    {
                        if (interval < 0)
                        {
                            // インターバルがマイナスの場合、過去の日付を取得
                            sql = "";
                            sql = sql + "select min(aa.cal_date) as caldate ";
                            sql = sql + "  from ( ";
                            sql = sql + "      select cal_date, row_number() over (order by cal_date desc) as rownum ";
                            sql = sql + "        from cal ";
                            sql = sql + "       where cal_cd = (select cal_cd from company where campany_cd = @CompanyCd) ";
                            sql = sql + "         and cal_date < @CalDate";
                            sql = sql + "         and cal_holiday = 0 "; // 0:平日
                            sql = sql + "     ) as aa ";
                            sql = sql + " where aa.rownum <= " + Math.Abs(interval);
                        }
                        else
                        {
                            // インターバルが0の場合、当日が休日の場合は稼働日を取得
                            sql = "";
                            sql = sql + "select max(aa.cal_date) as caldate ";
                            sql = sql + "  from ( ";
                            sql = sql + "      select cal_date, row_number() over (order by cal_date) as rownum ";
                            sql = sql + "        from cal ";
                            sql = sql + "       where cal_cd = (select cal_cd from company where campany_cd = @CompanyCd) ";
                            sql = sql + "         and cal_date >= @CalDate";
                            sql = sql + "         and cal_holiday = 0 "; // 0:平日
                            sql = sql + "     ) as aa ";
                            sql = sql + " where aa.rownum <= 1";
                        }

                        // SQL実行
                        results = db.GetEntity<EntityDao.CalEntity>(sql, new { CompanyCd = companyCd, CalDate = baseDate });
                    }

                    if (results != null)
                    {
                        //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                        returnValue = DateTime.Parse(results.CalDate.ToString());
                    }

                }

                return returnValue;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 納入先略称を取得する
        /// </summary>
        /// <param name="keyCd">納入先コード</param>
        /// <param name="languageId">言語コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>納入先略称</returns>
        public static string FncGetDeliveryShortName(string keyCd, string languageId,
            ComDB db)
        {
            try
            {
                var sql = "";
                sql = sql + "select delivery_short_name ";
                sql = sql + "  from v_delivery ";
                sql = sql + " where language_id = @LanguageId ";
                sql = sql + "   and delivery_cd = @DeliveryCd ";

                EntityDao.DeliveryEntity results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.DeliveryEntity>(sql, new { LanguageId = languageId, DeliveryCd = keyCd });
                if (results != null)
                {
                    // IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    return results.DeliveryShortName;
                }

                return "";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ロット番号自動付番(未使用)
        /// </summary>
        /// <param name="productionLine">生産ラインコード</param>
        /// <param name="directionNo">製造指図番号</param>
        /// <returns>ロット番号</returns>
        public static string FncGetDirLotno(string productionLine, string directionNo)
        {
            int maxlen = directionNo.Length;

            try
            {
                // 自動付番 システム日(YYMMDD)+生産ラインコード(左10桁0埋め)+製造指図番号(右8桁)
                return string.Format("{0:yyMMdd}", DateTime.Now) + productionLine.PadLeft(10, '0') + directionNo.Substring(maxlen - 8, 8);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 言語IDを取得する
        /// </summary>
        /// <param name="updatorCd">更新者ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>言語ID</returns>
        public static string FncGetLangId(string updatorCd,
            ComDB db)
        {
            try
            {
                var sql = "";
                sql = sql + "select language_id ";
                sql = sql + "  from login ";
                sql = sql + " where user_id = @UserId ";

                EntityDao.LoginEntity results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.LoginEntity>(sql, new { UserId = updatorCd });
                if (results != null)
                {
                    // IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    return results.LanguageId;
                }
                return "";
            }
            catch (Exception ex)
            {
                string errmsg;
                errmsg = ex.Message;

                // エラー時の戻り値設定
                return "ja";
            }
        }

        /// <summary>
        /// ログイン者の所属部署コードを取得する
        /// </summary>
        /// <param name="tantoCd">担当者コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>ログイン者の所属部署コード</returns>
        public static string FncGetLoginOrganizationCd(string tantoCd,
            ComDB db)
        {
            try
            {
                var sql = "";
                sql = sql + "select organization_cd ";
                sql = sql + "  from belong ";
                sql = sql + " where user_id = @UserId ";
                sql = sql + "   and belong_division = '1' "; // 主所属

                EntityDao.BelongEntity results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.BelongEntity>(sql, new { UserId = tantoCd });
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;

                    return results.OrganizationCd;
                }
                return "";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// デフォルトロット番号取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <returns>デフォルトロット番号</returns>
        public static string FncGetLotDummy(ComDB db)
        {
            try
            {
                var sql = "";
                sql = sql + "select default_lot_no ";
                sql = sql + "  from company ";

                EntityDao.CompanyEntity results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.CompanyEntity>(sql);
                if (results != null)
                {
                    // IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    return results.DefaultLotNo;
                }
                return "";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 月の最終日を取得する
        /// </summary>
        /// <param name="yyyymm">日付(yyyy/MM/dd)</param>
        /// <returns>指定年月の最終日の日付</returns>
        public static DateTime FncGetMonthLastDay(DateTime yyyymm)
        {
            string calcYYYYMM;
            DateTime calcDate;
            DateTime lastDate;

            try
            {
                // その日の壱日を求める
                calcYYYYMM = string.Format("{0:yyyy/MM}", yyyymm);
                calcDate = DateTime.Parse(calcYYYYMM + "/01");
                // １か月後の前日を求める
                calcDate = calcDate.AddMonths(1);
                lastDate = calcDate.AddDays(-1);

                // 最終日として返す
                return lastDate;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 名称マスタの区分「CLNG」の値を取得する
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <returns>名称マスタの区分「CLNG」の名称３(カンマ区切りで連結)</returns>
        public static string FncGetNamesClng(ComDB db)
        {
            string retLangId = "";

            try
            {
                var sql = "";
                sql = sql + "select names.name03 ";
                sql = sql + "  from names ";
                sql = sql + " where names.name_division = 'CLNG' ";

                if (retLangId == "")
                {
                    retLangId = ",ja,en";
                }

                IList<EntityDao.NamesEntity> resultList = null;
                EntityDao.NamesEntity results = null;
                // SQL実行
                resultList = db.GetList<EntityDao.NamesEntity>(sql);
                if (resultList != null)
                {
                    for (int i = 0; i < resultList.Count; i++)
                    {
                        results = resultList[i];

                        //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                        retLangId = retLangId + "," + results.Name03;
                    }
                }

                return retLangId.Substring(1);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                // エラー時の戻り値設定
                return "ja,en";
            }
        }

        /// <summary>
        /// 名称マスタのNAME_CDの値を取得する
        /// </summary>
        /// <param name="nameDivision">名称区分</param>
        /// <param name="mecode1">拡張コード１</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>名称コード</returns>
        public static string FncGetNameCd(string nameDivision, string mecode1,
            ComDB db)
        {
            try
            {
                var sql = "";
                sql = sql + "select name_cd ";
                sql = sql + "  from names ";
                sql = sql + " where name_division = @NameDivision ";
                sql = sql + "   and mecode1 = @Mecode1 ";

                EntityDao.NamesEntity results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.NamesEntity>(sql, new { NameDivision = nameDivision, Mecode1 = mecode1 });
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    return results.NameCd;
                }

                return "";

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// V_OPERATION_MASTERのOPERATION_NAMEを取得する
        /// </summary>
        /// <param name="keyCd">工程コード</param>
        /// <param name="languageId">言語コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>工程名称</returns>
        public static string FncGetOperationName(string keyCd, string languageId,
            ComDB db)
        {
            try
            {
                var sql = "";
                sql = sql + "select operation_name ";
                sql = sql + "  from v_operation ";
                sql = sql + " where language_id = @LanguageId ";
                sql = sql + "   and operation_cd = @OperationCd ";

                EntityDao.OperationEntity results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.OperationEntity>(sql, new { LanguageId = languageId, OperationCd = keyCd });
                if (results != null)
                {
                    // IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    return results.OperationName;
                }

                return "";

            }
            catch (Exception ex)
            {
                Console.Write(ex);

                // エラー時の戻り値設定
                return "";
            }
        }

        /// <summary>
        /// 受注金額取得
        /// </summary>
        /// <param name="balanceCd">帳合コード</param>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specificationCode">仕様コード</param>
        /// <param name="orderQty">数量</param>
        /// <param name="validDate">有効開始日</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>受注金額</returns>
        public static decimal FncGetOrderAmount(string balanceCd, string itemCd, string specificationCode, decimal orderQty, DateTime validDate,
            ComDB db)
        {
            decimal unitprice = 0;                // 単価
            int unitpriceDivision;                // 単価区分
            string unitOfOperationManagement;     // 運用管理単位
            decimal kilogramOfFractionManagement; // 換算係数
            decimal allUpWeight = 1;              // 総重量
            decimal amount = 0;                   // 金額

            try
            {
                var sql = "";
                sql = sql + "select unitprice ";
                sql = sql + "      ,unitprice_division ";
                sql = sql + "      ,unit_of_operation_management ";
                sql = sql + "      ,kg_of_fraction_management ";
                sql = sql + "      ,all_up_weight ";
                sql = sql + " from ( ";
                sql = sql + "     select unitprice.unitprice ";
                sql = sql + "           ,unitprice.unitprice_division ";
                sql = sql + "           ,spec.unit_of_operation_management ";
                sql = sql + "           ,spec.kg_of_fraction_management ";
                sql = sql + "           ,spec.all_up_weight ";
                sql = sql + "      from ( ";
                sql = sql + "          select subup12.* ";
                sql = sql + "            from ( ";
                sql = sql + "                select row_number() over (order by subup11.valid_date desc) as seqno ";
                sql = sql + "                      ,subup11.* ";
                sql = sql + "                  from unitprice subup11";
                sql = sql + "                 where subup11.balance_cd = @BalanceCd ";
                sql = sql + "                   and subup11.item_cd = @ItemCd ";
                sql = sql + "                   and subup11.specification_cd = @SpecificationCd ";
                sql = sql + "                   and subup11.quantity_from <= @OrderQty";
                sql = sql + "                   and subup11.quantity_to >= @OrderQty";
                sql = sql + "                   and subup11.valid_date <= cast(@ValidDate as timestamp)";
                sql = sql + "                   and subup11.approval_status = 3 "; // 承認済
                sql = sql + "                   and subup11.unitprice_division = 1 "; // 1:個あたり
                sql = sql + "            ) subup12 ";
                sql = sql + "           where subup12.seqno = 1 ";
                sql = sql + "        ) unitprice ";
                sql = sql + " left join balance ";
                sql = sql + "     on unitprice.balance_cd = balance.balance_cd ";
                sql = sql + " left join vender ";
                sql = sql + "     on balance.vender_division = vender.vender_division ";
                sql = sql + "      and balance.vender_division = 'TS' ";
                sql = sql + "      and balance.vender_cd = vender.vender_cd ";
                sql = sql + " inner join item ";
                sql = sql + "     on unitprice.item_cd = item.item_cd ";
                sql = sql + " inner join item_specification spec ";
                sql = sql + "     on item.item_cd = spec.item_cd ";
                sql = sql + "      and unitprice.specification_cd = spec.specification_cd ";
                sql = sql + " union all ";
                sql = sql + " select unitprice.unitprice ";
                sql = sql + "       ,unitprice.unitprice_division ";
                sql = sql + "       ,unitprice.unit_of_operation_management ";
                sql = sql + "       ,unitprice.kg_of_fraction_management ";
                sql = sql + "       ,unitprice.all_up_weight ";
                sql = sql + "   from ( ";
                sql = sql + "       select subup22.*";
                sql = sql + "            from ( ";
                sql = sql + "                select row_number() over (order by subup21.valid_date desc) as seqno ";
                sql = sql + "                      ,subup21.* ";
                sql = sql + "                      ,spec.unit_of_operation_management ";
                sql = sql + "                      ,spec.kg_of_fraction_management ";
                sql = sql + "                      ,spec.all_up_weight ";
                sql = sql + "                  from unitprice subup21";
                sql = sql + "                 inner join item ";
                sql = sql + "                     on subup21.item_cd = item.item_cd ";
                sql = sql + "                 inner join item_specification spec ";
                sql = sql + "                     on item.item_cd = spec.item_cd ";
                sql = sql + "                      and subup21.specification_cd = spec.specification_cd ";
                sql = sql + "                 where subup21.balance_cd = @BalanceCd ";
                sql = sql + "                   and subup21.item_cd = @ItemCd ";
                sql = sql + "                   and subup21.specification_cd = @SpecificationCd ";
                sql = sql + "                   and subup21.quantity_from <= @OrderQty";
                sql = sql + "                         * (case spec.kg_of_fraction_management ";
                sql = sql + "                            when 0 then 1 ";
                sql = sql + "                            else spec.kg_of_fraction_management ";
                sql = sql + "                            end) ";
                sql = sql + "                         * (case spec.all_up_weight ";
                sql = sql + "                            when 0 then 1 ";
                sql = sql + "                            else spec.all_up_weight ";
                sql = sql + "                            end) ";
                sql = sql + "                   and subup21.quantity_to >= @OrderQty";
                sql = sql + "                         * (case spec.kg_of_fraction_management ";
                sql = sql + "                            when 0 then 1 ";
                sql = sql + "                            else spec.kg_of_fraction_management ";
                sql = sql + "                            end) ";
                sql = sql + "                         * (case spec.all_up_weight ";
                sql = sql + "                            when 0 then 1 ";
                sql = sql + "                            else spec.all_up_weight ";
                sql = sql + "                            end) ";
                sql = sql + "                   and subup21.valid_date <= to_date(@ValidDate, 'YYYY/MM/DD')";
                sql = sql + "                   and subup21.approval_status = 3 "; // 承認済
                sql = sql + "                   and subup21.unitprice_division = 2 "; // 2:Kgあたり
                sql = sql + "            ) subup22 ";
                sql = sql + "           where subup22.seqno = 1 ";
                sql = sql + "        ) unitprice ";
                sql = sql + " left join balance ";
                sql = sql + "     on unitprice.balance_cd = balance.balance_cd ";
                sql = sql + " left join vender ";
                sql = sql + "     on balance.vender_division = vender.vender_division ";
                sql = sql + "      and balance.vender_division = 'TS'";
                sql = sql + "      and balance.vender_cd = vender.vender_cd ";
                sql = sql + " ) unitprice ";

                APDao.UnitSpecification results = null;
                // SQL実行
                results = db.GetEntity<APDao.UnitSpecification>(sql,
                    new { BalanceCd = balanceCd, ItemCd = itemCd, SpecificationCd = specificationCode, OrderQty = orderQty, ValidDate = string.Format("{0:yyyy/MM/dd}", validDate) });
                if (results != null)
                {
                    // 確認
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    unitprice = decimal.Parse(results.Unitprice.ToString());
                    unitpriceDivision = int.Parse(results.UnitpriceDivision.ToString());
                    unitOfOperationManagement = results.UnitOfOperationManagement.ToString();
                    kilogramOfFractionManagement = decimal.Parse(results.KgOfFractionManagement.ToString());
                    allUpWeight = decimal.Parse(results.AllUpWeight.ToString());

                    if (unitpriceDivision == 1)
                    {
                        if (unitOfOperationManagement == "2")
                        {
                            // 個あたり&Kg(金額=数量(Kg)/総重量/換算係数x(単価-値引))
                            amount = (orderQty / allUpWeight / kilogramOfFractionManagement) * unitprice;
                        }
                        else
                        {
                            // 個あたり&個数(金額=数量(個)x(単価-値引))
                            amount = orderQty * unitprice;
                        }
                    }
                    else if (unitOfOperationManagement == "2")
                    {
                        // Kgあたり&Kg(金額=数量(Kg)x(単価-値引))
                        amount = orderQty * unitprice;
                    }
                    else
                    {
                        // Kgあたり&個数(金額=数量(個)x換算係数x総重量x(単価-値引き))
                        amount = (orderQty * kilogramOfFractionManagement * allUpWeight) * unitprice;
                    }
                }

                return amount;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 受注金額取得処理クラス
        /// </summary>
        public class FncGetOrderAmountInfo
        {
            /// <summary>Gets 受注金額情報格納リスト</summary>
            /// <value>受注金額情報格納リスト</value>
            public Dictionary<string, decimal> OrderAmountList { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public FncGetOrderAmountInfo()
            {
                // 受注金額情報リストを初期化
                this.OrderAmountList = new Dictionary<string, decimal>();
            }

            /// <summary>
            /// 受注金額取得
            /// </summary>
            /// <param name="balanceCd">帳合コード</param>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCode">仕様コード</param>
            /// <param name="orderQty">数量</param>
            /// <param name="validDate">有効開始日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>受注金額</returns>
            public decimal GetOrderAmount(string balanceCd, string itemCd, string specificationCode, decimal orderQty, DateTime validDate, ComDB db)
            {
                // 帳合コード、品目コード、仕様コード、数量、有効開始日をKey情報に設定する
                var keyInfo = "";
                keyInfo += balanceCd != null ? balanceCd : "";
                keyInfo += "|";
                keyInfo += itemCd != null ? itemCd : "";
                keyInfo += "|";
                keyInfo += specificationCode != null ? specificationCode : "";
                keyInfo += "|";
                keyInfo += orderQty.ToString();
                keyInfo += "|";
                keyInfo += validDate.ToString(); // キー情報を生成
                if (OrderAmountList.ContainsKey(keyInfo))
                {
                    return OrderAmountList[keyInfo];
                }
                else
                {
                    // 検索を行い、ディクショナリーに退避
                    decimal results = FncGetOrderAmount(balanceCd, itemCd, specificationCode, orderQty, validDate, db);
                    OrderAmountList.Add(keyInfo, results);
                    return results;
                }
            }
        }

        /// <summary>
        /// V_ORGANIZATION_MASTERのORGANIZATION_NAMEを取得する
        /// </summary>
        /// <param name="keyCd">部署コード</param>
        /// <param name="languageId">言語コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>部署名称</returns>
        public static string FncGetOrganizationName(string keyCd, string languageId,
            ComDB db)
        {
            try
            {

                var sql = "";
                sql = sql + "select organization_name ";
                sql = sql + "  from v_organization_master ";
                sql = sql + " where language_id = @LanguageId ";
                sql = sql + "   and organization_cd = @OrganizationCd ";

                EntityDao.OrganizationMasterEntity results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.OrganizationMasterEntity>(sql, new { LanguageId = languageId, OrganizationCd = keyCd });
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    return results.OrganizationName;
                }

                return "";

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 支払先・請求先を取得する
        /// </summary>
        /// <param name="venderDivision">取引先区分</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>請求先コード</returns>
        public static string FncGetPaymentInvoiceCd(string venderDivision, string venderCd,
            ComDB db)
        {
            string ret = "";

            try
            {
                // 取引先マスタを検索し、請求先・支払先を取得する
                var sql = "";
                sql = sql + "select payment_invoice_cd ";
                sql = sql + "  from vender ";
                sql = sql + " where vender_division = @VenderDivision ";
                sql = sql + "   and vender_cd = @VenderCd ";

                EntityDao.VenderEntity results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.VenderEntity>(sql, new { VenderDivision = venderDivision, VenderCd = venderCd });
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    ret = results.PaymentInvoiceCd;
                    if (ret == null || ret.Length > 0)
                    {
                        // 取得出来た場合、支払先・請求先を返す
                        return ret;
                    }
                }

                // 取引先マスタをみてない場合は取引先コードを返す(取引先に請求・支払いをする)
                return venderCd;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 生産ライン名称取得(未使用)
        /// </summary>
        /// <param name="keyCd">生産ラインコード</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>生産ライン名称</returns>
        public static string FncGetProductionLineName(string keyCd, string languageId,
            ComDB db)
        {
            try
            {
                var sql = "";
                sql = sql + "select production_line_name ";
                sql = sql + "  from v_line ";
                sql = sql + " where language_id = @LanguageId ";
                sql = sql + "   and production_line = @ProductionLine ";

                EntityDao.LineEntity results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.LineEntity>(sql, new { LanguageId = languageId, ProductionLine = keyCd });
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    return results.ProductionLineName;
                }

                return "";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }
        /* purcahse_subcontructが廃止されたのでコメントアウト
        /// <summary>
        /// 発注データの金額基準に則した承認担当者を取得する処理
        /// </summary>
        /// <param name="purchaseNo">発注番号</param>
        /// <param name="buySubcontractOrderNo">購入依頼番号（未使用※画面からの設定はあり）</param>
        /// <param name="detailRowNo">明細行番号（未使用※画面からの設定はあり）</param>
        /// <param name="outsourcingRowNo">外注行番号（未使用※画面からの設定はあり）</param>
        /// <param name="division">処理区分（未使用）</param>
        /// <param name="orderDivideNo">分納枝番（未使用）</param>
        /// <param name="qty">数量（未使用）</param>
        /// <param name="qtySub">数量（未使用）</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>チェック結果 承認担当者</returns>
        public static string FncGetPurchaseApprovalUser(string purchaseNo, string buySubcontractOrderNo, int detailRowNo, int outsourcingRowNo,
                                                     string division, string orderDivideNo, object qty, object qtySub
            ,ComDB db)
        {
            string mbuySubcontractOrderNo = "";  // 購入依頼番号
            string mpurchaseDivision = "";       // 購入品番号
            decimal msupplierOrdAmount = 0;      // 発注数量
            int mpostId;                         // 役職
            int mpostPower = 0;                  // 役職権限
            string mtantoCd = "";                // 購買責任者
            string mtantoCd1st = "";             // 購買責任者
            string mtantoCd2nd = "";             // 購買責任者

            try
            {
                // 購入品区分と金額を取得
                var sql = "";
                sql = sql + "select item_specification.purchase_division ";
                sql = sql + "      ,supplier_ord_amount ";
                sql = sql + "      ,purchase_subcontract.buy_subcontract_order_no ";
                sql = sql + "  from purchase_subcontract ";
                sql = sql + " left join item_specification ";
                sql = sql + "   on purchase_subcontract.item_cd = item_specification.item_cd ";
                sql = sql + "   and purchase_subcontract.specification_code = item_specification.specification_code ";
                sql = sql + " where purchase_subcontract.purchase_no = '" + purchaseNo + "' ";

                EntityDao.PurchaseSubcontractEntity results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.PurchaseSubcontractEntity>(sql);
                if (results != null)
                {
                    // IDictionary<string, object> dicResult = results as IDictionary<string, object>;

                    mpurchaseDivision = results.purchaseDivision;//["purchase_division"].ToString();
                    msupplierOrdAmount = decimal.Parse(results.SupplierOrdAmount.ToString());
                    mbuySubcontractOrderNo = results.BuySubcontractOrderNo;
                }

                // 承認権限管理テーブルから役職を取得
                sql = "";
                sql = sql + "select tbl.post_id ";
                sql = sql + "      ,tbl.post_power ";
                sql = sql + "  from ( ";
                sql = sql + "      select post.post_id ";
                sql = sql + "            ,post_power ";
                sql = sql + "            ,row_number() over (order by post.post_id) as seqno ";
                sql = sql + "        from approval_authority ";
                sql = sql + "        left join post ";
                sql = sql + "          on approval_authority.post_id = post.post_id ";
                sql = sql + "       where approval_authority.authority_cd = 'PURCHASE' ";
                sql = sql + "         and approval_authority.division_cd = '" + mpurchaseDivision + "' ";
                sql = sql + "         and approval_authority.ammount_from <= " + msupplierOrdAmount;
                sql = sql + "         and approval_authority.ammount_to >= " + msupplierOrdAmount;
                sql = sql + "     ) tbl ";
                sql = sql + " where tbl.seqno = 1 ";
                results = null;
                // SQL実行
                results = db.GetEntity<EntityDao.auth(sql);
                if (results != null)
                {
                    // IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    mpostId = int.Parse(dicResult["post_id"].ToString());
                    mpostPower = int.Parse(dicResult["post_power"].ToString());
                }

                // 購買ヘッダから発注の承認者を取得
                sql = "";
                sql = sql + "select purchase_head.ordering_approval_tanto_cd ";
                sql = sql + "      ,purchase_head.ordering_approval_tanto_cd_1st ";
                sql = sql + "      ,purchase_head.ordering_approval_tanto_cd_2nd ";
                sql = sql + "  from purchase_head";
                sql = sql + " where purchase_head.buy_subcontract_order_no = '" + mbuySubcontractOrderNo + "' ";
                results = null;
                // SQL実行
                 EntityDao.PurchaseHeadEntity itemSpecificationResults = db.GetEntity<EntityDao.PurchaseHeadEntity>(sql);
                if (results != null)
                {
                    // IDictionary<string, object> dicResult = results as IDictionary<string, object>;

                    mtantoCd = itemSpecificationResults.ordering["ordering_approval_tanto_cd"].ToString();
                    mtantoCd1st = itemSpecificationResults.order["ordering_approval_tanto_cd_1st"].ToString();
                    mtantoCd2nd = dicResult["ordering_approval_tanto_cd_2nd"].ToString();
                }

                if (mpostPower == 1)
                {
                    return mtantoCd2nd; // 管理部長を返す
                }
                else if (mpostPower == 3)
                {
                    return mtantoCd1st; // 課長を返す
                }
                else if (mpostPower == 4)
                {
                    return mtantoCd;    // 購買担当者を返す
                }

                return mtantoCd;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }*/

        /// <summary>
        /// 売上・仕入の区分から消費税コードを取得する
        /// </summary>
        /// <param name="category">用途</param>
        /// <param name="taxDate">有効開始日</param>
        /// <param name="taxCategory">軽減税率</param>
        /// <param name="ex1">拡張用文字列1</param>
        /// <param name="ex2">拡張用文字列2</param>
        /// <param name="ex3">拡張用文字列3</param>
        /// <param name="ex4">拡張用文字列4</param>
        /// <param name="ex5">拡張用文字列5</param>
        /// <param name="taxDivision">課税区分</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>1:通常 2:在庫表除外 3:更新除外</returns>
        public static string FncGetTaxCdFromDate(string category, string taxDate, string taxCategory, string ex1, string ex2, string ex3, string ex4, string ex5, string taxDivision,
            ComDB db)
        {
            try
            {
                var sql = "";
                if (taxDivision == "3" || taxDivision == "5" || taxDivision == "6")
                {
                    sql = sql + "select tax_master.tax_cd ";
                    sql = sql + "  from tax_master";
                    sql = sql + " where tax_master.category = @Category ";
                    sql = sql + " and tax_master.valid_date <= @ValidDate ";
                    sql = sql + " and tax_master.tax_category = @TaxCategory ";
                    //    and(tax_master.ex_string_01 in (@i_s_tax_diviion)
                    //    or      tax_master.ex_string_02 in (@i_s_tax_diviion))
                    sql = sql + " order by valid_date desc, sort asc ";
                    sql = sql + " limit 1 ";
                }
                else
                {
                    sql = sql + "select tax_master.tax_cd ";
                    sql = sql + "  from tax_master";
                    sql = sql + " where tax_master.category = @Category ";
                    sql = sql + " and tax_master.valid_date <= @ValidDate ";
                    sql = sql + " and tax_master.tax_category = @TaxCategory ";
                    sql = sql + " order by valid_date desc, sort asc ";
                    sql = sql + " limit 1 ";
                }
                // SQL実行
                EntityDao.TaxMasterEntity results = db.GetEntity<EntityDao.TaxMasterEntity>(sql,
                    new { Category = category, ValidDate = DateTime.Parse(taxDate), TaxCategory = int.Parse(taxCategory) });
                if (results != null)
                {
                    return results.TaxCd;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 引数の調合コードにより上位の調合情報を取得する
        /// </summary>
        /// <param name="division">区分(CD:得意先コードを返す NAME:得意先名称を返す)</param>
        /// <param name="level">階層</param>
        /// <param name="balanceCd">調合コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>引数の区分に応じた取引先</returns>
        public static string FncGetUpperVenderData(string division, int level, string balanceCd,
            ComDB db)
        {
            int tempCount; // 検索結果有無用カウント変数
            string tempRetVenderCd = null;  // 戻り値の得意先コード
            string tempRetVenderName;  // 戻り値の得意先名称
            string tempUpperBalance;   // 上位調合コード
            //string tempRet;            // 戻り値の得意先コード
            int tempLv = 0;            // ループカウント
            int shopLevel = 0;         // 次店

            EntityDao.BalanceEntity results = null;

            try
            {
                var sql = "";
                sql = sql + "select balance.shop_level ";
                sql = sql + "  from balance ";
                sql = sql + " where balance.balance_cd = @BalanceCd ";
                // SQL実行
                results = db.GetEntity<EntityDao.BalanceEntity>(sql, new { BalanceCd = balanceCd });
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    tempLv = int.Parse(results.ShopLevel.ToString());
                }

                if (tempLv < level)
                {
                    // 引数の階層が引数の帳合の階層より遅い場合、空白値を返す
                    return null;
                }
                else if (tempLv == level)
                {
                    // 引数と同じ階層の場合、値を返す
                    sql = "";
                    sql = sql + "select balance.vender_cd ";
                    sql = sql + "  from balance ";
                    sql = sql + " where balance.balance_cd = @BalanceCd ";
                    // SQL実行
                    results = null;
                    results = db.GetEntity<EntityDao.BalanceEntity>(sql, new { BalanceCd = balanceCd });
                    if (results != null)
                    {
                        //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                        tempRetVenderCd = results.VenderCd;
                    }

                    if (division == "CD")
                    {
                        return tempRetVenderCd;
                    }
                    else
                    {
                        sql = "";
                        sql = sql + "select concat(vender.vender_name1, vender.vender_name2) as vender_nm ";
                        sql = sql + "  from vender ";
                        sql = sql + " where vender.vender_division = 'TS' ";
                        sql = sql + "  and vender.vender_cd = @VenderCd";
                        // SQL実行
                        results = null;
                        EntityDao.VenderEntity venderResults = db.GetEntity<EntityDao.VenderEntity>(sql, new { VenderCd = tempRetVenderCd });
                        if (results != null)
                        {
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            tempRetVenderName = venderResults.VenderName1;
                        }
                    }
                }

                // 上位調合を取得する
                sql = "";
                sql = sql + "select balance.upper_balance_cd ";
                sql = sql + "  from balance ";
                sql = sql + " where balance.balance_cd = @BalanceCd ";
                // SQL実行
                results = null;
                EntityDao.BalanceEntity balanceResults = db.GetEntity<EntityDao.BalanceEntity>(sql, new { BalanceCd = balanceCd });
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    tempUpperBalance = balanceResults.UpperBalanceCd;
                }
                else
                {
                    // 上位帳合いのデータが無い場合、空白を返して処理終了
                    return "";
                }

                // 上位帳合取得ループ（規定回数以上は無限ループを防ぐため処理しない）
                while (1 == 1)
                {
                    tempCount = 0;
                    // 上位帳合いのデータが無い場合、空白を返して処理終了
                    sql = "";
                    sql = sql + "select count(*) as cnt ";
                    sql = sql + "  from balance ";
                    sql = sql + " where balance.balance_cd = @BalanceCd ";
                    // SQL実行
                    results = null;
                    int countResults = db.GetEntity<int>(sql, new { BalanceCd = tempUpperBalance });
                    if (results != null)
                    {
                        //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                        tempCount = int.Parse(countResults.ToString());
                    }
                    if (tempCount == 0)
                    {
                        return "";
                    }

                    tempUpperBalance = "";
                    // 上位の帳合いのデータを取得
                    sql = "";
                    sql = sql + "select balance.shop_level ";
                    sql = sql + "      ,balance.upper_balance_cd ";
                    sql = sql + "      ,balance.vender_cd ";
                    sql = sql + "  from balance ";
                    sql = sql + " where balance.balance_cd = @BalanceCd ";
                    // SQL実行
                    results = null;
                    results = db.GetEntity<EntityDao.BalanceEntity>(sql, new { BalanceCd = tempUpperBalance });
                    if (results != null)
                    {
                        //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                        shopLevel = int.Parse(results.ShopLevel.ToString());
                        tempUpperBalance = results.UpperBalanceCd.ToString();
                        tempRetVenderCd = results.VenderCd;
                    }
                    if (shopLevel == level)
                    {
                        // 指定の次店に到達した場合、ループを抜ける
                        if (division == "CD")
                        {
                            return tempRetVenderCd;
                        }
                        else
                        {
                            sql = "";
                            sql = sql + "select concat(vender.vender_name1, vender.vender_name2) as vender_nm ";
                            sql = sql + "  from vender ";
                            sql = sql + " where vender.vender_division = 'TS' ";
                            sql = sql + "  and vender.vender_cd = @VenderCd";
                            // SQL実行
                            results = null;
                            EntityDao.VenderEntity venderResults = db.GetEntity<EntityDao.VenderEntity>(sql, new { VenderCd = tempRetVenderCd });
                            if (results != null)
                            {
                                //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                                return venderResults.VenderName1;
                            }
                        }
                    }
                    else
                    {
                        if (tempUpperBalance == "")
                        {
                            return "";
                        }
                    }

                    tempLv++;
                    if (tempLv > 10)
                    {
                        // 無限ループを防ぐため指定回数を超えた場合抜ける
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// nullか空欄ではなかった場合、第一引数、それ以外は第二引数を返す
        /// </summary>
        /// <param name="str">nullか空欄をチェックする文字列</param>
        /// <param name="changeStr">nullか空欄であった場合置き換える文字列</param>
        /// <returns>nullか空欄ではなかった場合、第一引数、それ以外は第二引数</returns>
        public static string FncIsNullOrEmpty(string str, string changeStr)
        {
            try
            {
                if (str != null || str == "")
                {
                    // nullか空欄であった場合第二引数の変換文字を返す
                    return changeStr;
                }
                // nullか空欄でない場合第一引数を返す
                return str;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 数量チェック　受入予定数量に対する受入数量の超過チェック
        /// </summary>
        /// <param name="buySubcontractOrderNo">購入依頼番号（未使用※画面からの設定はあり）</param>
        /// <param name="division">処理区分（未使用）</param>
        /// <param name="orderDivideNo">発注番号分納枝番（未使用）</param>
        /// <param name="detailRowNo">明細行番号（未使用）</param>
        /// <param name="outsourcingRowNo">外注行番号（未使用）</param>
        /// <param name="qty">受入予定数量合計</param>
        /// <param name="qtySub">受入数量合計</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>チェック結果（0:正常 -1:異常）</returns>
        public static int FncQtyCheckPurchaseInstructOver(string buySubcontractOrderNo, string division,
            string orderDivideNo, int detailRowNo, int outsourcingRowNo, decimal qty, decimal qtySub,
            ComDB db)
        {
            string setting = ""; // 自社設定テーブルからの設定

            try
            {
                // 自社マスタの設定を取得し、チェックする設定以外の場合、正常のゼロを返す（自社マスタのコードは固定）
                var sql = "";
                sql = sql + "select qty_check_purchase_instruct_over ";
                sql = sql + "  from company_setting";
                sql = sql + " where company_setting.company_cd = '000001' ";
                // SQL実行
                EntityDao.CompanySettingEntity results = db.GetEntity<EntityDao.CompanySettingEntity>(sql);
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    setting = results.QtyCheckPurchaseInstructOver.ToString();
                }
                if (setting != "1")
                {
                    return 0;
                }

                if (qty < qtySub)
                {
                    // 受入予定数量　＜　受入数量の場合エラーとする
                    return -1;
                }
                else
                {
                    // 受入予定数量　≧　受入数量の場合正常
                    return 0;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 数量チェック　発注数に対する出荷指図累計数の超過
        /// </summary>
        /// <param name="buySubcontractOrderNo">購入依頼番号</param>
        /// <param name="division">処理区分（未使用）</param>
        /// <param name="orderDivideNo">分納枝番</param>
        /// <param name="detailRowNo">明細行番号</param>
        /// <param name="outsourcingRowNo">外注行番号</param>
        /// <param name="qty">受入予定数量の数量合計</param>
        /// <param name="qtySub">数量補助（未使用）</param>
        /// <returns>チェック結果（0:正常 -1:異常）</returns>
        public static int FncQtyCheckPurchaseOrderOver(string buySubcontractOrderNo, string division, string orderDivideNo, int detailRowNo, int outsourcingRowNo, decimal qty, decimal qtySub)
        {
            return 0;
        }

        /// <summary>
        /// 数量チェック　出荷指図数量に対する実績量の超過チェック
        /// </summary>
        /// <param name="shippingNo">出荷指図番号（未使用※画面からの設定はあり）</param>
        /// <param name="division">処理区分（未使用）</param>
        /// <param name="refNo">指図番号参照用（未使用）</param>
        /// <param name="refRow">指図行番号（未使用）</param>
        /// <param name="qty">指図数量合計</param>
        /// <param name="qtySub">実績数量合計</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>チェック結果（0:正常 -1:異常）</returns>
        public static int FncQtyCheckShippingInstructOver(string shippingNo, string division, string refNo, int refRow, decimal qty, decimal qtySub,
            ComDB db)
        {
            string setting = ""; // 自社設定テーブルからの設定

            try
            {
                // 自社マスタの設定を取得し、チェックする設定以外の場合、正常のゼロを返す（自社マスタのコードは固定）
                var sql = "";
                sql = sql + "select qty_check_shipping_instruct_over ";
                sql = sql + "  from company_setting";
                sql = sql + " where company_setting.company_cd = '000001' ";
                // SQL実行
                EntityDao.CompanySettingEntity results = db.GetEntity<EntityDao.CompanySettingEntity>(sql);
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    setting = results.QtyCheckShippingInstructOver.ToString();
                }
                if (setting != "1")
                {
                    return 0;
                }

                if (qty < qtySub)
                {
                    // 出荷指図数量　＜　出荷実績数量の場合エラーとする
                    return -1;
                }
                else
                {
                    // 出荷指図数量　≧　出荷実績数量の場合正常
                    return 0;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 数量チェック　受注数に対する出荷指図累計数の超過
        /// </summary>
        /// <param name="shippingNo">出荷指図番号</param>
        /// <param name="division">処理区分（未使用）</param>
        /// <param name="refNo">指図番号参照用（未使用）</param>
        /// <param name="refRow">指図番号行番号（未使用）</param>
        /// <param name="qty">出荷指図画面のロット選択画面のロット毎の数量合計</param>
        /// <param name="qtySub">数量補助（未使用）</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>チェック結果（0:正常 -1:異常）</returns>
        public static int FncQtyCheckShippingOrderOver(string shippingNo, string division, string refNo, int refRow, decimal qty, decimal qtySub,
            ComDB db)
        {
            decimal orderQty = 0;    // 受注数量
            decimal shippingQty = 0; // 受注番号に紐づく出荷数量の合計
            string orderNo = "";     // 受注番号
            int orderRowNo = 0;      // 受注行番号
            string setting = "";     // 自社設定テーブルからの設定

            try
            {
                // 自社マスタの設定を取得し、チェックする設定以外の場合、正常のゼロを返す（自社マスタのコードは固定）
                var sql = "";
                sql = sql + "select qty_check_shipping_order_over ";
                sql = sql + "  from company_setting";
                sql = sql + " where company_setting.company_cd = '000001' ";
                // SQL実行
                EntityDao.CompanySettingEntity results = db.GetEntity<EntityDao.CompanySettingEntity>(sql);
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    setting = results.QtyCheckShippingInstructOver.ToString();
                }
                if (setting != "1")
                {
                    return 0;
                }

                // 出荷番号に紐づく受注数量、受注番号、受注行番号を取得
                results = null;
                sql = "";
                sql = sql + "select order_detail.order_qty ";
                sql = sql + "      ,order_detail.order_no ";
                sql = sql + "      ,order_detail.order_row_no ";
                sql = sql + "  from shipping";
                sql = sql + "  left join order_detail ";
                sql = sql + "     on order_detail.order_no = shipping.order_no ";
                sql = sql + "    and order_detail.order_row_no = shipping.order_row_no ";
                sql = sql + " where shipping.shipping_no = @ShippingNo ";
                // SQL実行
                EntityDao.OrderDetailEntity orderResults = db.GetEntity<EntityDao.OrderDetailEntity>(sql, new { ShippingNo = shippingNo });
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    orderQty = decimal.Parse(orderResults.OrderQty.ToString());
                    orderNo = orderResults.OrderNo.ToString();
                    orderRowNo = int.Parse(orderResults.OrderRowNo.ToString());
                }

                // 受注番号に紐づく出荷指図量の累計を取得する(自分の指図は無視する。自分の指図の数量は引数となる。)
                results = null;
                sql = "";
                sql = sql + "select sum(shipping_detail.shipping_instruction) as sumqty ";
                sql = sql + "  from shipping_detail ";
                sql = sql + "  left join shipping ";
                sql = sql + "   on shipping.shipping_no = shipping_detail.shipping_no ";
                sql = sql + " where shipping.order_no = @OrderNo ";
                sql = sql + "   and shipping.order_row_no = @OrderRowNo";
                sql = sql + "   and shipping.shipping_no <> @ShippingNo ";
                // SQL実行
                decimal shippingResults = db.GetEntity<decimal>(sql, new { OrderNo = orderNo, OrderRowNo = orderRowNo, ShippingNo = shippingNo });
                if (results != null)
                {
                    // FFF sumをdecimalで受け取る処理　確認
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    shippingQty = decimal.Parse(shippingResults.ToString());
                }

                // 自分自身の画面で入力した指図数量を加算する
                shippingQty = shippingQty + qty;

                if (orderQty < shippingQty)
                {
                    // 受注数量より出荷数量が多い場合、エラーとする
                    return -1;
                }
                else
                {
                    // 受注数量より出荷数量の方が少ない場合、正常
                    return 0;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 数量チェック　受注数に対する出荷実績累計数の超過
        /// </summary>
        /// <param name="shippingNo">出荷指図番号</param>
        /// <param name="division">処理区分（未使用）</param>
        /// <param name="refNo">指図番号参照用（未使用）</param>
        /// <param name="refRowNo">指図番号行番号（未使用）</param>
        /// <param name="qty">出荷実績の実績数量合計</param>
        /// <param name="qtySub">数量補助（未使用）</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常　-1:エラー</returns>
        public static int FncQtyCheckShippingResultOver(string shippingNo, string division, string refNo, int refRowNo, decimal qty, decimal qtySub,
            ComDB db)
        {
            decimal orderQty = 0;    // 受注数量
            decimal shippingQty = 0; // 受注番号に紐づく出荷数量の合計
            string orderNo = "";     // 受注番号
            int orderRowNo = 0;      // 受注行番号
            string setting = "";     // 自社設定テーブルからの設定

            try
            {
                // 自社マスタの設定を取得し、チェックする設定以外の場合、正常のゼロを返す（自社マスタのコードは固定）
                var sql = "";
                sql = sql + "select qty_check_shipping_result_over ";
                sql = sql + "  from company_setting";
                sql = sql + " where company_setting.company_cd = '000001' ";
                // SQL実行
                EntityDao.CompanySettingEntity results = db.GetEntity<EntityDao.CompanySettingEntity>(sql);
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    setting = results.QtyCheckShippingResultOver.ToString();
                }
                if (setting != "1")
                {
                    return 0;
                }

                // 出荷番号に紐づく受注数量、受注番号、受注行番号を取得
                results = null;
                sql = "";
                sql = sql + "select order_detail.order_qty ";
                sql = sql + "      ,order_detail.order_no ";
                sql = sql + "      ,order_detail.order_row_no ";
                sql = sql + "  from shipping";
                sql = sql + "  left join order_detail ";
                sql = sql + "     on order_detail.order_no = shipping.order_no ";
                sql = sql + "    and order_detail.order_row_no = shipping.order_row_no ";
                sql = sql + " where shipping.shipping_no = @ShippingNo ";
                // SQL実行
                EntityDao.OrderDetailEntity orderResults = db.GetEntity<EntityDao.OrderDetailEntity>(sql, new { ShippingNo = shippingNo });
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    orderQty = decimal.Parse(orderResults.OrderQty.ToString());
                    orderNo = orderResults.OrderNo.ToString();
                    orderRowNo = int.Parse(orderResults.OrderRowNo.ToString());
                }

                // 受注番号に紐づく出荷実績量の累計を取得する(自分の指図は無視する。自分の指図の数量は引数となる。)
                results = null;
                sql = "";
                sql = sql + "select sum(shipping_detail.shipping_result_quantity) as sumqty ";
                sql = sql + "  from shipping_detail ";
                sql = sql + "  left join shipping ";
                sql = sql + "   on shipping.shipping_no = shipping_detail.shipping_no ";
                sql = sql + " where shipping.order_no = @OrderNo ";
                sql = sql + "   and shipping.order_row_no = @OrderRowNo";
                sql = sql + "   and shipping.shipping_no <> @ShippingNo ";
                // SQL実行
                decimal sumResult = db.GetEntity<decimal>(sql, new { OrderNo = orderNo, OrderRowNo = orderRowNo, ShippingNo = shippingNo });
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    shippingQty = decimal.Parse(sumResult.ToString());
                }

                // 自分自身の画面で入力した指図数量を加算する
                shippingQty = shippingQty + qty;

                if (orderQty < shippingQty)
                {
                    // 受注数量より出荷実績量が多い場合、エラーとする
                    return -1;
                }
                else
                {
                    // 受注数量より出荷実績量の方が少ない場合、正常
                    return 0;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 端数処理
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="precision">小数点以下桁数</param>
        /// <param name="roundMode">端数区分(0:なし 1:切り捨て 2:四捨五入 3:切り上げ)</param>
        /// <returns>処理後の値</returns>
        public static decimal FncRound(decimal value, decimal precision, decimal roundMode)
        {
            decimal tempRetValue;  // 端数処理結果
            decimal tempTruncUp;   // 切り上げ処理用
            decimal ope;

            try
            {
                tempRetValue = value;

                // 小数点以下の区分
                if (precision == 0)
                {
                    tempTruncUp = 0.9m;
                }
                else if (precision == 1)
                {
                    tempTruncUp = 0.09m;
                }
                else if (precision == 2)
                {
                    tempTruncUp = 0.009m;
                }
                else if (precision == 3)
                {
                    tempTruncUp = 0.0009m;
                }
                else if (precision == 4)
                {
                    tempTruncUp = 0.00009m;
                }
                else if (precision == 5)
                {
                    tempTruncUp = 0.000009m;
                }
                else if (precision == 6)
                {
                    tempTruncUp = 0.0000009m;
                }
                else
                {
                    tempTruncUp = 0;
                }

                // 端数処理計算
                if (roundMode == 1)
                {
                    // 切捨
                    ope = decimal.Parse(Math.Pow(10, (int)precision).ToString());
                    return Math.Floor(value * ope) / ope;
                }
                else if (roundMode == 2)
                {
                    // 四捨五入
                    return Math.Round(value, (int)precision, MidpointRounding.AwayFromZero);
                }
                else if (roundMode == 3)
                {
                    // 切上
                    //return Math.Round(value + tempTruncUp, (int)precision);
                    return Trunc(decimal.Add(value, tempTruncUp), (int)precision);
                }
                else
                {
                    // その他
                    return value;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 切り上げ
        /// </summary>
        /// <param name="number">数値</param>
        /// <param name="decimals">有効桁数</param>
        /// <returns>処理後の値</returns>
        private static decimal ceiling(decimal number, int decimals)
        {
            // MathクラスのPow静的メソッドを使用して指定された小数の有効桁数までの整数にする
            decimal d = (decimal)Math.Pow(10, decimals);
            decimal value = number * d;
            value = Math.Ceiling(value);
            // 整数を小数に戻す
            return value / d;
        }

        /// <summary>
        /// 数値の書式設定
        /// </summary>
        /// <param name="unitDivision">区分</param>
        /// <param name="venderDivision">取引先区分</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="value">数値</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="activeDate">取引先有効開始日 ※省略可</param>
        /// <returns>書式設定された文字列</returns>
        public static string FncValueFormat(string unitDivision, string venderDivision, string venderCd, decimal value,
            ComDB db, DateTime? activeDate = null)
        {
            int smallnumLength = 0;
            int roundDivision = 0;
            string formatValue = "#,##0";
            int i = 1;
            int isMinus = 0;

            try
            {
                // 数値がnullの場合は処理中止
                if (value.ToString() == null || value.ToString().Length <= 0)
                {
                    return null;
                }
                // 区分がnullの場合は空白に置換
                if (unitDivision == null)
                {
                    unitDivision = " ";
                }
                // 取引先区分がnullの場合は空白に置換
                if (venderDivision == null)
                {
                    venderDivision = " ";
                }
                // 取引先コードがnullの場合は空白に置換
                if (venderCd == null)
                {
                    venderCd = " ";
                }
                // 数値桁数チェックM検索
                var sql = "";
                sql = sql + "select smallnum_length ";
                sql = sql + "      ,round_division ";
                sql = sql + "  from number_chkdisit ";
                sql = sql + " where unit_division = @UnitDivision ";
                sql = sql + "   and vender_division = @VenderDivision ";
                sql = sql + "   and vender_cd = @VenderCd ";
                if (activeDate != null)
                {
                    sql += "    and active_date = @ActiveDate ";
                }
                // SQL実行
                EntityDao.NumberChkdisitEntity results = new ComDao.NumberChkdisitEntity();
                if (activeDate != null)
                {
                    results = db.GetEntity<EntityDao.NumberChkdisitEntity>(sql,
                        new { UnitDivision = unitDivision, VenderDivision = venderDivision, VenderCd = venderCd, ActiveDate = activeDate });
                }
                else
                {
                    results = db.GetEntity<EntityDao.NumberChkdisitEntity>(sql,
                        new { UnitDivision = unitDivision, VenderDivision = venderDivision, VenderCd = venderCd });
                }
                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    smallnumLength = int.Parse(results.SmallnumLength.ToString());
                    roundDivision = int.Parse(results.RoundDivision.ToString());
                }

                if ((smallnumLength.ToString() == null || smallnumLength.ToString() == "") ||
                      (roundDivision.ToString() == null || roundDivision.ToString() == ""))
                {
                    // 数値桁数チェックM検索
                    results = null;
                    sql = "";
                    sql = sql + "select smallnum_length ";
                    sql = sql + "      ,round_division ";
                    sql = sql + "  from number_chkdisit ";
                    sql = sql + " where unit_division = @UnitDivision ";
                    sql = sql + "   and vender_division = ' ' ";
                    sql = sql + "   and vender_cd = ' ' ";
                    // SQL実行
                    results = db.GetEntity<EntityDao.NumberChkdisitEntity>(sql, new { UnitDivision = unitDivision });
                    if (results != null)
                    {
                        //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                        smallnumLength = int.Parse(results.SmallnumLength.ToString());
                        roundDivision = int.Parse(results.RoundDivision.ToString());
                    }
                }

                if (smallnumLength.ToString() == null)
                {
                    smallnumLength = 0;
                }
                if (roundDivision.ToString() == null)
                {
                    roundDivision = 0;
                }

                if (smallnumLength > 0)
                {
                    formatValue = formatValue + ".";

                    while (i <= smallnumLength)
                    {
                        formatValue = formatValue + "0";
                        i++;
                    }
                }

                if (value < 0)
                {
                    isMinus = 1;
                    value = value * (-1);
                }
                value = FncRound(value, smallnumLength, roundDivision);

                if (isMinus == 1)
                {
                    value = value * (-1);
                }

                return string.Format("{0:" + formatValue + "}", value);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 数理の書式設定処理クラス
        /// </summary>
        public class FncValueFormatInfo
        {
            /// <summary>Gets 受注金額情報格納リスト</summary>
            /// <value>受注金額情報格納リスト</value>
            public Dictionary<string, EntityDao.NumberChkdisitEntity> FncValueFormatList { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public FncValueFormatInfo()
            {
                // 受注金額情報リストを初期化
                this.FncValueFormatList = new Dictionary<string, EntityDao.NumberChkdisitEntity>();
            }

            /// <summary>
            /// 数値の書式設定
            /// </summary>
            /// <param name="unitDivision">区分</param>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="value">数値</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="activeDate">有効開始日</param>
            /// <returns>書式設定された文字列</returns>
            public string FncValueFormat(string unitDivision, string venderDivision, string venderCd, decimal value, ComDB db, DateTime? activeDate = null)
            {
                // 数値がnullの場合、処理を中止
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }

                // 区分、取引先区分、取引先コードをKey情報に設定する
                var keyInfo = "";
                keyInfo += unitDivision != null ? unitDivision : "";
                keyInfo += "|";
                keyInfo += venderDivision != null ? venderDivision : "";
                keyInfo += "|";
                keyInfo += venderCd != null ? venderCd : "";
                keyInfo += "|";
                keyInfo += activeDate != null ? ((DateTime)activeDate).ToString("yyyy/MM/dd HH:mm:ss") : ""; // キー情報を生成
                if (FncValueFormatList.ContainsKey(keyInfo))
                {
                    return fncValueFormat(FncValueFormatList[keyInfo], value);
                }
                else
                {
                    // 数値桁数を取得
                    EntityDao.NumberChkdisitEntity bean = getNumberChkDisit(unitDivision, venderDivision, venderCd, db, activeDate);
                    FncValueFormatList.Add(keyInfo, bean);
                    return fncValueFormat(bean, value);
                }
            }

            /// <summary>
            /// 数値桁数取得
            /// </summary>
            /// <param name="unitDivision">区分</param>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="activeDate">有効開始日</param>
            /// <returns>数値桁数</returns>
            private EntityDao.NumberChkdisitEntity getNumberChkDisit(string unitDivision, string venderDivision, string venderCd, ComDB db, DateTime? activeDate)
            {
                // 区分がnullの場合、空白に置換
                if (unitDivision == null)
                {
                    unitDivision = " ";
                }
                // 取引先区分がnullの場合、空白に置換
                if (venderDivision == null)
                {
                    venderDivision = " ";
                }
                // 取引先コードがnullの場合、空白に置換
                if (venderCd == null)
                {
                    venderCd = " ";
                }

                // SQL文生成
                string sql = string.Empty;
                sql += "select * ";
                sql += "  from number_chkdisit ";
                sql += " where unit_division = @UnitDivision ";
                sql += "   and vender_division = @VenderDivision ";
                sql += "   and vender_cd = @VenderCd ";
                if (activeDate != null)
                {
                    sql += " and active_date = @ActiveDate ";
                }
                sql += " order by unit_division, vender_division desc, vender_cd ";

                // SQL実行
                EntityDao.NumberChkdisitEntity results = new ComDao.NumberChkdisitEntity();
                if (activeDate != null)
                {
                    results = db.GetEntity<EntityDao.NumberChkdisitEntity>(sql, new { UnitDivision = unitDivision, VenderDivision = venderDivision, VenderCd = venderCd, ActiveDate = activeDate });
                }
                else
                {
                    results = db.GetEntity<EntityDao.NumberChkdisitEntity>(sql, new { UnitDivision = unitDivision, VenderDivision = venderDivision, VenderCd = venderCd });
                }

                if (results != null)
                {
                    return results;
                }

                // SQL再作成（有効開始日を検索条件から除外）
                sql = string.Empty;
                sql += "select * ";
                sql += "  from number_chkdisit ";
                sql += " where unit_division = @UnitDivision ";
                sql += "   and vender_division = @VenderDivision ";
                sql += "   and vender_cd = @VenderCd ";
                sql += " order by unit_division, vender_division desc, vender_cd ";
                return db.GetEntity<EntityDao.NumberChkdisitEntity>(sql, new { UnitDivision = unitDivision, VenderDivision = " ", VenderCd = " " });
            }

            /// <summary>
            /// 数値の書式設定
            /// </summary>
            /// <param name="bean">数値桁数情報</param>
            /// <param name="value">数値</param>
            /// <returns>書式設定された文字列</returns>
            private string fncValueFormat(EntityDao.NumberChkdisitEntity bean, decimal value)
            {
                int smallnumLength = 0;
                int roundDivision = 0;
                string formatValue = "#,##0";

                if (bean != null)
                {
                    // 数値桁数情報が存在する場合、小数点桁数、端数区分を設定
                    smallnumLength = bean.SmallnumLength != null ? (int)bean.SmallnumLength : 0;
                    roundDivision = bean.RoundDivision != null ? (int)bean.RoundDivision : 0;
                }

                if (smallnumLength > 0)
                {
                    // 小数点桁数を設定
                    formatValue += ".";
                    int i = 1;
                    while (i <= smallnumLength)
                    {
                        formatValue += "0";
                        i++;
                    }
                }

                bool isMinus = true;
                if (value < 0)
                {
                    isMinus = false;
                    value *= -1;
                }
                value = FncRound(value, smallnumLength, roundDivision);

                if (!isMinus)
                {
                    value *= -1;
                }

                return string.Format("{0:" + formatValue + "}", value);
            }
        }

        /// <summary>
        /// 月単位手形サイト日数取得
        /// </summary>
        /// <param name="paramCreditSchduledDate">支払基準日</param>
        /// <param name="paramNoteSight">日単位手形サイト日数</param>
        /// <returns>月単位手形サイト日数</returns>
        public static int FunGetNoteSight(DateTime paramCreditSchduledDate, int paramNoteSight)
        {
            DateTime noteDate; // 満期日
            int noteSight;     // 手形サイト日数

            try
            {
                // 満期日 = 支払基準日 + サイト日数
                noteDate = paramCreditSchduledDate.AddDays(paramNoteSight);

                // 末日満期日
                noteDate = FncGetMonthLastDay(noteDate);

                // 手形サイト日数 = 末日満期日 - 支払基準日
                TimeSpan interval = noteDate - paramCreditSchduledDate;
                noteSight = interval.Days;

                return noteSight;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 支払予定日取得処理
        /// </summary>
        /// <param name="paramVenderCd">取引先コード</param>
        /// <param name="paramPayableDate">支払締日(yyyyMMdd)</param>
        /// <param name="paramPayableAmount">支払残高</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>支払予定日(yyyyMMdd)</returns>
        public static string FunGetPaymentScheDate(string paramVenderCd, string paramPayableDate, decimal paramPayableAmount, ComDB db)
        {
            string paramCreditScheDate;             // 支払日付
            const int constCreditMonthDiv0 = 1;     // 支払月区分(当月)
            const int constCreditMonthDiv1 = 2;     // 支払月区分(翌月)
            const int constCreditMonthDiv2 = 3;     // 支払月区分(翌々月)
            const int constCreditMonthDiv3 = 4;     // 支払月区分(3ヶ月)
            const int constCreditMonthDiv4 = 5;     // 支払月区分(4ヶ月)
            const int constCreditMonthDiv5 = 6;     // 支払月区分(5ヶ月)
            const int constHolidayFlg0 = 1;         // 休日指定フラグ(前倒し)
            const int constHolidayFlg1 = 2;         // 休日指定フラグ(先送り)
            const int constHolidayFlg2 = 3;         // 休日指定フラグ(休日を考慮しない)
            const int constWeekHolidayFlg0 = 0;     // 平日休日フラグ(平日)
            //const int cWeekHolidayFlg1 = 1;       // 平日休日フラグ(休日)
            const string constCatDivPYNOTE = "1";   // 分類マスタ分類コード(支払:手形)
            int numCreditMonthDiv = 0;              // 支払月区分
            int numCreditScheDay = 0;               // 支払予定日(DD)(取引先マスタ)
            string varCreditScheDay;                // 支払予定日(DD)(文字列)
            int numHolidayFlg = 0;                  // 休日指定フラグ
            DateTime datePayableDate;               // 支払締め日
            //string nvarPayableDateYM;               // 支払締め年月(YYYYMM)
            DateTime datePaymentScheDate;           // 支払予定日
            DateTime dateTmpPaymentScheDate;        // 支払予定日(仮)
            DateTime dateCalDate;                   // 年月日
            int? numCalHoliday = null;              // 休日区分 0:平日 1:休日
            string numCreditDiv = string.Empty;     // 支払区分
            string condition = string.Empty;
            //int cnt = 0;
            StringBuilder sql;

            try
            {
                // 文字型をDate型へキャスト
                datePayableDate = DateTime.Parse(paramPayableDate);

                //// 支払締め年月(YYYYMM)
                //nvarPayableDateYM = paramPayableDate.Substring(0, 6);

                // ★取引先マスタ検索
                // 支払締め日直近の有効データ取得
                var vender = APComUtil.GetVenderInfoByActiveDate<ComDao.VenderEntity>(APConstants.APConstants.VENDER_DIVISION.SI, paramVenderCd, datePayableDate, "ja", db);
                if (vender == null)
                {
                    throw new Exception();
                }
                condition = string.Format(" (vender_division : @VenderDivision, vender_cd : @VenderCd, active_date : @ActiveDate)", new { VenderDivision = APConstants.APConstants.VENDER_DIVISION.SI, VenderCd = paramVenderCd, ActiveDate = vender.ActiveDate });

                // ★入金支払条件マスタ検索
                sql = new StringBuilder();
                sql.AppendLine(" select seq");
                sql.AppendLine("       ,credit_month_division"); // 入金月区分
                sql.AppendLine("       ,bound_amount");          // 境界額
                sql.AppendLine("       ,credit_division");       // 入金･支払区分
                sql.AppendLine("       ,credit_scheduled_date"); // 入金予定日
                sql.AppendLine(" from   vender_credit_detail");
                sql.AppendLine(" where  vender_division = @VenderDivision");
                sql.AppendLine(" and    vender_cd = @VenderCd");
                sql.AppendLine(" and    active_date = @ActiveDate");
                sql.AppendLine(" and    credit_division is not null");
                sql.AppendLine(" order by bound_amount, seq");
                // SQL実行
                IList<EntityDao.VenderCreditDetailEntity> resultList = db.GetListByDataClass<EntityDao.VenderCreditDetailEntity>(sql.ToString(),
                    new { VenderDivision = APConstants.APConstants.VENDER_DIVISION.SI, VenderCd = paramVenderCd, ActiveDate = vender.ActiveDate });
                if (resultList == null || resultList.Count == 0)
                {
                    string message = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.GetPropertiesMessage(APCommonUtil.APResources.ID.MB10186);
                    throw new Exception(message + condition);
                }
                if (resultList != null)
                {
                    foreach (var result in resultList)
                    {
                        // 金額による選定
                        if ((result.BoundAmount ?? 0) >= paramPayableAmount)
                        {
                            numCreditMonthDiv = result.CreditMonthDivision ?? 0;
                            numCreditScheDay = result.CreditScheduledDate ?? 0;
                            numCreditDiv = result.CreditDivision.ToString();
                            break;
                        }
                    }
                }

                // 設定に当てはまらない場合
                if (numCreditMonthDiv.Equals(0))
                {
                    string message = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.GetPropertiesMessage(APCommonUtil.APResources.ID.M10249, APCommonUtil.APResources.ID.I20142);
                    throw new Exception(message + condition);
                }

                // 支払月区分により支払予定日設定
                if (numCreditMonthDiv == constCreditMonthDiv0)
                {
                    // 当月 daPaymentScheDate = daPaymentScheDate
                }
                else if (numCreditMonthDiv == constCreditMonthDiv1)
                {
                    datePayableDate = datePayableDate.AddMonths(1); // 翌月
                }
                else if (numCreditMonthDiv == constCreditMonthDiv2)
                {
                    datePayableDate = datePayableDate.AddMonths(2); // 翌々月
                }
                else if (numCreditMonthDiv == constCreditMonthDiv3)
                {
                    datePayableDate = datePayableDate.AddMonths(3); // 3ヶ月
                }
                else if (numCreditMonthDiv == constCreditMonthDiv4)
                {
                    datePayableDate = datePayableDate.AddMonths(4); // 4ヶ月
                }
                else if (numCreditMonthDiv == constCreditMonthDiv5)
                {
                    datePayableDate = datePayableDate.AddMonths(5); // 5ヶ月
                }

                // 支払予定日設定
                // 月末指定(99)の場合
                if (numCreditScheDay == 99)
                {
                    // 支払予定日=支払締め日の月末日付
                    datePaymentScheDate = FncGetMonthLastDay(datePayableDate);
                }
                else
                {
                    // 支払予定日(DD2桁)
                    varCreditScheDay = numCreditScheDay.ToString().PadLeft(2, '0');

                    // 支払締め日の月末日＜支払予定日(DD2桁)
                    int chkDay;
                    chkDay = FncGetMonthLastDay(datePayableDate).Day;
                    if (chkDay < int.Parse(varCreditScheDay))
                    {
                        // 支払予定日＝支払締め日の月末日付
                        datePaymentScheDate = FncGetMonthLastDay(datePayableDate);
                    }
                    else
                    {
                        // 支払予定日＝支払締め年月+支払予定日(DD2桁)
                        datePaymentScheDate = DateTime.Parse(datePayableDate.ToString("yyyy/MM") + "/" + varCreditScheDay);
                        //datePaymentScheDate = DateTime.Parse(nvarPayableDateYM.Substring(0, 4) + "/" + nvarPayableDateYM.Substring(4, 2) + "/" + varCreditScheDay);
                    }
                }

                //// 支払月区分により支払予定日設定
                //if (numCreditMonthDiv == constCreditMonthDiv0)
                //{
                //    // 当月 daPaymentScheDate = daPaymentScheDate
                //}
                //else if (numCreditMonthDiv == constCreditMonthDiv1)
                //{
                //    datePaymentScheDate = datePaymentScheDate.AddMonths(1); // 翌月
                //}
                //else if (numCreditMonthDiv == constCreditMonthDiv2)
                //{
                //    datePaymentScheDate = datePaymentScheDate.AddMonths(2); // 翌々月
                //}
                //else if (numCreditMonthDiv == constCreditMonthDiv3)
                //{
                //    datePaymentScheDate = datePaymentScheDate.AddMonths(3); // 3ヶ月
                //}
                //else if (numCreditMonthDiv == constCreditMonthDiv4)
                //{
                //    datePaymentScheDate = datePaymentScheDate.AddMonths(4); // 4ヶ月
                //}
                //else if (numCreditMonthDiv == constCreditMonthDiv5)
                //{
                //    datePaymentScheDate = datePaymentScheDate.AddMonths(5); // 5ヶ月
                //}

                // 支払予定日＜支払締め日の場合
                if (datePaymentScheDate < datePayableDate)
                {
                    datePaymentScheDate = datePaymentScheDate.AddMonths(1); // ＋１ヶ月

                }

                // 休日指定フラグ=「休日を考慮しない」場合
                if (numHolidayFlg == constHolidayFlg2)
                {
                    //daPaymentScheDate = daPaymentScheDate;
                }
                // 支払区分が1:手形の場合、休日指定フラグを「前倒し」とする。
                if (numCreditDiv == constCatDivPYNOTE)
                {
                    numHolidayFlg = constHolidayFlg0; // 休日指定フラグ=「前倒し」
                }

                // カレンダーマスタ検索
                sql = new StringBuilder();
                sql.AppendLine(" select cal_date");     // 年月日
                sql.AppendLine("       ,cal_holiday");  // 休日
                sql.AppendLine(" from   cal");
                sql.AppendLine(" where  cal_cd = @CalCd");
                sql.AppendLine(" and    cal_date = to_date(@CalDate, 'YYYY/MM/DD')");
                // SQL実行
                EntityDao.CalEntity calResult = db.GetEntity<EntityDao.CalEntity>(sql.ToString(), new { CalCd = vender.CalendarCd, CalDate = string.Format("{0:yyyy/MM/dd}", datePaymentScheDate) });
                if (calResult != null)
                {
                    dateCalDate = calResult.CalDate;
                    numCalHoliday = calResult.CalHoliday;
                }

                // 平日の場合
                if (numCalHoliday == constWeekHolidayFlg0)
                {
                    //daPaymentScheDate = daPaymentScheDate;
                }

                // 以降、支払予定日=休日の場合
                // 休日指定フラグ=「前倒し」場合
                if (numHolidayFlg == constHolidayFlg0)
                {
                    dateTmpPaymentScheDate = datePaymentScheDate;

                    while (1 == 1)
                    {

                        dateTmpPaymentScheDate = dateTmpPaymentScheDate.AddDays(-1);

                        // ★カレンダーマスタ再検索
                        // SQL実行
                        calResult = db.GetEntity<EntityDao.CalEntity>(sql.ToString(), new { CalCd = vender.CalendarCd, CalDate = string.Format("{0:yyyy/MM/dd}", dateTmpPaymentScheDate) });
                        if (calResult != null)
                        {
                            dateCalDate = calResult.CalDate;
                            numCalHoliday = calResult.CalHoliday;
                        }

                        // 平日の場合、ループ終了
                        if (numCalHoliday == constWeekHolidayFlg0)
                        {
                            break;
                        }
                    }
                    // 支払予定日＜支払締め日の場合
                    if (dateTmpPaymentScheDate < datePayableDate)
                    {
                        // 「先送り」の処理へ
                        numHolidayFlg = constHolidayFlg1; // 先送り
                    }
                    else
                    {
                        datePaymentScheDate = dateTmpPaymentScheDate;
                    }
                }

                // 休日指定フラグ=「先送り」場合
                if (numHolidayFlg == constHolidayFlg1)
                {
                    dateTmpPaymentScheDate = datePaymentScheDate;

                    while (1 == 1)
                    {

                        dateTmpPaymentScheDate = dateTmpPaymentScheDate.AddDays(1);

                        // ★カレンダーマスタ再検索
                        // SQL実行
                        calResult = db.GetEntity<EntityDao.CalEntity>(sql.ToString(), new { CalCd = vender.CalendarCd, CalDate = string.Format("{0:yyyy/MM/dd}", dateTmpPaymentScheDate) });
                        if (calResult != null)
                        {
                            dateCalDate = calResult.CalDate;
                            numCalHoliday = calResult.CalHoliday;
                        }

                        // 平日の場合、ループ終了
                        if (numCalHoliday == constWeekHolidayFlg0)
                        {
                            break;
                        }
                    }
                    datePaymentScheDate = dateTmpPaymentScheDate;
                }

                //// 支払予定日=仕入日の月末日付
                //if (numCreditScheDay == 99)
                //{
                //    datePaymentScheDate = FncGetMonthLastDay(datePaymentScheDate);
                //}
                paramCreditScheDate = string.Format("{0:yyyyMMdd}", datePaymentScheDate);

                return paramCreditScheDate;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                // エラー時の戻り値設定
                return "19000101";
            }
        }

        /// <summary>
        /// 入金予定日取得処理
        /// </summary>
        /// <param name="paramVenderCd">請求先コード</param>
        /// <param name="paramCreditDate">請求締め日(yyyyMMdd)</param>
        /// <param name="paramClaimAmount">今回請求額(差引繰越額含まず)</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>入金予定日(yyyyMMdd)</returns>
        public static string FunGetCreditScheDate(string paramVenderCd, string paramCreditDate, decimal paramClaimAmount, ComDB db)
        {
            string paramCreditScheDate;             // 入金日付
            const int constCreditMonthDiv0 = 1;     // 入金月区分(当月)
            const int constCreditMonthDiv1 = 2;     // 入金月区分(翌月)
            const int constCreditMonthDiv2 = 3;     // 入金月区分(翌々月)
            const int constCreditMonthDiv3 = 4;     // 入金月区分(3ヶ月)
            const int constCreditMonthDiv4 = 5;     // 入金月区分(4ヶ月)
            const int constCreditMonthDiv5 = 6;     // 入金月区分(5ヶ月)
            const int constHolidayFlg0 = 1;         // 休日指定フラグ(前倒し)
            const int constHolidayFlg1 = 2;         // 休日指定フラグ(先送り)
            const int constHolidayFlg2 = 3;         // 休日指定フラグ(休日を考慮しない)
            const int constWeekHolidayFlg0 = 0;     // 平日休日フラグ(平日)
            //const int cWeekHolidayFlg1 = 1;       // 平日休日フラグ(休日)
            const string constCatDivPYNOTE = "1";   // 分類マスタ分類コード(入金:手形)
            int numCreditMonthDiv = 0;              // 入金月区分
            int numCreditScheDay = 0;               // 入金予定日(DD)(取引先マスタ)
            string varCreditScheDay;                // 入金予定日(DD)(文字列)
            int numHolidayFlg = 0;                  // 休日指定フラグ
            DateTime dateCreditDate;                // 入金締め日
            //string nvarCreditDateYM;                // 入金締め年月(YYYYMM)
            DateTime dateCreditScheDate;            // 入金予定日
            DateTime dateTmpCreditScheDate;         // 入金予定日(仮)
            DateTime dateCalDate;                   // 年月日
            int? numCalHoliday = null;              // 休日区分 0:平日 1:休日
            string numCreditDiv = string.Empty;     // 入金区分
            string condition = string.Empty;
            //int cnt = 0;

            try
            {
                // 文字型をDate型へキャスト
                dateCreditDate = DateTime.Parse(paramCreditDate);

                //// 入金締め年月(YYYYMM)
                //nvarCreditDateYM = dateCreditDate.ToString("yyyyMM");

                // ★取引先マスタ検索
                // 入金締め日直近の有効データ取得
                var vender = APComUtil.GetVenderInfoByActiveDate<ComDao.VenderEntity>(APConstants.APConstants.VENDER_DIVISION.TS, paramVenderCd, dateCreditDate, "ja", db);
                if (vender == null)
                {
                    throw new Exception();
                }
                condition = string.Format(" (vender_division : @VenderDivision, vender_cd : @VenderCd, active_date : @ActiveDate)", new { VenderDivision = APConstants.APConstants.VENDER_DIVISION.SI, VenderCd = paramVenderCd, ActiveDate = vender.ActiveDate });

                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" select seq");
                sql.AppendLine("       ,credit_month_division"); // 入金月区分
                sql.AppendLine("       ,bound_amount");          // 境界額
                sql.AppendLine("       ,credit_division");       // 入金･支払区分
                sql.AppendLine("       ,credit_scheduled_date"); // 入金予定日
                sql.AppendLine(" from   vender_credit_detail");
                sql.AppendLine(" where  vender_division = @VenderDivision");
                sql.AppendLine(" and    vender_cd = @VenderCd");
                sql.AppendLine(" and    active_date = @ActiveDate");
                sql.AppendLine(" and    credit_division is not null");
                sql.AppendLine(" order by bound_amount, seq");

                // ★入金条件マスタ検索
                // SQL実行
                IList<EntityDao.VenderCreditDetailEntity> resultList = db.GetListByDataClass<EntityDao.VenderCreditDetailEntity>(sql.ToString(),
                    new { VenderDivision = APConstants.APConstants.VENDER_DIVISION.TS, VenderCd = paramVenderCd, ActiveDate = vender.ActiveDate });
                if (resultList == null || resultList.Count == 0)
                {
                    string message = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.GetPropertiesMessage(APCommonUtil.APResources.ID.MB10186);
                    throw new Exception(message + condition);
                }
                if (resultList != null)
                {
                    foreach (var result in resultList)
                    {
                        // 金額による選定
                        if ((result.BoundAmount ?? 0) >= paramClaimAmount)
                        {
                            numCreditMonthDiv = result.CreditMonthDivision ?? 0;
                            numCreditScheDay = result.CreditScheduledDate ?? 0;
                            numCreditDiv = result.CreditDivision.ToString();
                            break;
                        }
                    }
                }

                // 設定に当てはまらない場合
                if (numCreditMonthDiv.Equals(0))
                {
                    string message = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.GetPropertiesMessage(APCommonUtil.APResources.ID.M10249, APCommonUtil.APResources.ID.I20142);
                    throw new Exception(message + condition);
                }

                // 入金月区分により入金予定日設定
                if (numCreditMonthDiv == constCreditMonthDiv0)
                {
                    // 当月 daCreditScheDate = daCreditScheDate
                }
                else if (numCreditMonthDiv == constCreditMonthDiv1)
                {
                    dateCreditDate = dateCreditDate.AddMonths(1); // 翌月
                }
                else if (numCreditMonthDiv == constCreditMonthDiv2)
                {
                    dateCreditDate = dateCreditDate.AddMonths(2); // 翌々月
                }
                else if (numCreditMonthDiv == constCreditMonthDiv3)
                {
                    dateCreditDate = dateCreditDate.AddMonths(3); // 3ヶ月
                }
                else if (numCreditMonthDiv == constCreditMonthDiv4)
                {
                    dateCreditDate = dateCreditDate.AddMonths(4); // 4ヶ月
                }
                else if (numCreditMonthDiv == constCreditMonthDiv5)
                {
                    dateCreditDate = dateCreditDate.AddMonths(5); // 5ヶ月
                }

                // 入金予定日設定
                // 月末指定(99)の場合
                if (numCreditScheDay == 99)
                {
                    // 入金予定日=入金締め日の月末日付
                    dateCreditScheDate = FncGetMonthLastDay(dateCreditDate);
                }
                else
                {
                    // 入金予定日(DD2桁)
                    varCreditScheDay = numCreditScheDay.ToString().PadLeft(2, '0');

                    // 入金締め日の月末日＜入金予定日(DD2桁)
                    int chkDay;
                    chkDay = FncGetMonthLastDay(dateCreditDate).Day;
                    if (chkDay < int.Parse(varCreditScheDay))
                    {
                        // 入金予定日＝入金締め日の月末日付
                        dateCreditScheDate = FncGetMonthLastDay(dateCreditDate);
                    }
                    else
                    {
                        // 入金予定日＝入金締め年月+入金予定日(DD2桁)
                        dateCreditScheDate = DateTime.Parse(dateCreditDate.ToString("yyyy/MM") + "/" + varCreditScheDay);
                        //dateCreditScheDate = DateTime.Parse(nvarCreditDateYM.Substring(0, 4) + "/" + nvarCreditDateYM.Substring(4, 2) + "/" + varCreditScheDay);
                    }
                }

                //// 入金月区分により入金予定日設定
                //if (numCreditMonthDiv == constCreditMonthDiv0)
                //{
                //    // 当月 daCreditScheDate = daCreditScheDate
                //}
                //else if (numCreditMonthDiv == constCreditMonthDiv1)
                //{
                //    dateCreditScheDate = dateCreditScheDate.AddMonths(1); // 翌月
                //}
                //else if (numCreditMonthDiv == constCreditMonthDiv2)
                //{
                //    dateCreditScheDate = dateCreditScheDate.AddMonths(2); // 翌々月
                //}
                //else if (numCreditMonthDiv == constCreditMonthDiv3)
                //{
                //    dateCreditScheDate = dateCreditScheDate.AddMonths(3); // 3ヶ月
                //}
                //else if (numCreditMonthDiv == constCreditMonthDiv4)
                //{
                //    dateCreditScheDate = dateCreditScheDate.AddMonths(4); // 4ヶ月
                //}
                //else if (numCreditMonthDiv == constCreditMonthDiv5)
                //{
                //    dateCreditScheDate = dateCreditScheDate.AddMonths(5); // 5ヶ月
                //}

                // 入金予定日＜入金締め日の場合
                if (dateCreditScheDate < dateCreditDate)
                {
                    dateCreditScheDate = dateCreditScheDate.AddMonths(1); // ＋１ヶ月

                }

                // 休日指定フラグ=「休日を考慮しない」場合
                if (numHolidayFlg == constHolidayFlg2)
                {
                    //daCreditScheDate = daCreditScheDate;
                }
                // 入金区分が1:手形の場合、休日指定フラグを「前倒し」とする。
                if (numCreditDiv == constCatDivPYNOTE)
                {
                    numHolidayFlg = constHolidayFlg0; // 休日指定フラグ=「前倒し」
                }

                // カレンダーマスタ検索
                sql = new StringBuilder();
                sql.AppendLine(" select cal_date");     // 年月日
                sql.AppendLine("       ,cal_holiday");  // 休日
                sql.AppendLine(" from   cal");
                sql.AppendLine(" where  cal_cd = @CalCd");
                sql.AppendLine(" and    cal_date = to_date(@CalDate, 'YYYY/MM/DD')");
                // SQL実行
                EntityDao.CalEntity calResult = db.GetEntityByDataClass<EntityDao.CalEntity>(sql.ToString(), new { CalCd = vender.CalendarCd, CalDate = string.Format("{0:yyyy/MM/dd}", dateCreditScheDate) });
                if (calResult != null)
                {
                    dateCalDate = calResult.CalDate;
                    numCalHoliday = calResult.CalHoliday;
                }

                // 平日の場合
                if (numCalHoliday == constWeekHolidayFlg0)
                {
                    //daCreditScheDate = daCreditScheDate;
                }

                // 以降、入金予定日=休日の場合
                // 休日指定フラグ=「前倒し」場合
                if (numHolidayFlg == constHolidayFlg0)
                {
                    dateTmpCreditScheDate = dateCreditScheDate;

                    while (1 == 1)
                    {

                        dateTmpCreditScheDate = dateTmpCreditScheDate.AddDays(-1);

                        // ★カレンダーマスタ再検索
                        // SQL実行
                        calResult = db.GetEntity<EntityDao.CalEntity>(sql.ToString(), new { CalCd = vender.CalendarCd, CalDate = string.Format("{0:yyyy/MM/dd}", dateTmpCreditScheDate) });
                        if (calResult != null)
                        {
                            dateCalDate = calResult.CalDate;
                            numCalHoliday = calResult.CalHoliday;
                        }

                        // 平日の場合、ループ終了
                        if (numCalHoliday == constWeekHolidayFlg0)
                        {
                            break;
                        }
                    }
                    // 入金予定日＜入金締め日の場合
                    if (dateTmpCreditScheDate < dateCreditDate)
                    {
                        // 「先送り」の処理へ
                        numHolidayFlg = constHolidayFlg1; // 先送り
                    }
                    else
                    {
                        dateCreditScheDate = dateTmpCreditScheDate;
                    }
                }

                // 休日指定フラグ=「先送り」場合
                if (numHolidayFlg == constHolidayFlg1)
                {
                    dateTmpCreditScheDate = dateCreditScheDate;

                    while (1 == 1)
                    {

                        dateTmpCreditScheDate = dateTmpCreditScheDate.AddDays(1);

                        // ★カレンダーマスタ再検索
                        // SQL実行
                        calResult = db.GetEntity<EntityDao.CalEntity>(sql.ToString(), new { CalCd = vender.CalendarCd, CalDate = string.Format("{0:yyyy/MM/dd}", dateTmpCreditScheDate) });
                        if (calResult != null)
                        {
                            dateCalDate = calResult.CalDate;
                            numCalHoliday = calResult.CalHoliday;
                        }

                        // 平日の場合、ループ終了
                        if (numCalHoliday == constWeekHolidayFlg0)
                        {
                            break;
                        }
                    }
                    dateCreditScheDate = dateTmpCreditScheDate;
                }

                //// 入金予定日=仕入日の月末日付
                //if (numCreditScheDay == 99)
                //{
                //    dateCreditScheDate = FncGetMonthLastDay(dateCreditScheDate);
                //}
                paramCreditScheDate = string.Format("{0:yyyyMMdd}", dateCreditScheDate);

                return paramCreditScheDate;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                // エラー時の戻り値設定
                return "19000101";
            }
        }

        /// <summary>
        /// 入金/支払サイト取得処理
        /// </summary>
        /// <param name="paramVenderDivision">取引先区分</param>
        /// <param name="paramVenderCd">取引先コード</param>
        /// <param name="paramAmount">取引先金額</param>
        /// <param name="paramCreditScheduledDate">支払基準日</param>
        /// <param name="paramNoteSightDivision">手形サイト区分</param>
        /// <param name="paramCreditDivision">入金支払区分</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>手形サイト</returns>
        public static int FunGetSite(string paramVenderDivision, string paramVenderCd, decimal paramAmount, DateTime paramCreditScheduledDate, out int paramNoteSightDivision, out int paramCreditDivision,
            ComDB db)
        {
            int cnt = 0;                  // 取引先件数チェック用:カウント結果
            int numCreditDiv = 0;         // 入金/支払区分
            int numCreditMonthDiv = 0;    // 入金/支払月区分
            int numCreditScheDay = 0;     // 入金/支払予定日(0-99)
            int numNoteSite = 0;          // 入金/支払サイト(日単位)
            int numNoteSiteMonth = 0;     // 入金/支払サイト(月単位)
            int numNoteSightDivision = 0; // 手形サイト区分
            string condition = string.Empty;
            StringBuilder sql;

            paramNoteSightDivision = numNoteSightDivision;
            paramCreditDivision = 0;

            try
            {
                // 支払締め日直近の有効データ取得
                var vender = APComUtil.GetVenderInfoByActiveDate<ComDao.VenderEntity>(paramVenderDivision, paramVenderCd, paramCreditScheduledDate, "ja", db);
                if (vender == null)
                {
                    throw new Exception();
                }
                condition = string.Format(" (vender_division : @VenderDivision, vender_cd : @VenderCd, active_date : @ActiveDate)", new { VenderDivision = APConstants.APConstants.VENDER_DIVISION.SI, VenderCd = paramVenderCd, ActiveDate = vender.ActiveDate });

                // ★手形サイト単位
                numNoteSightDivision = vender.NoteSightDivision;

                // ★支払条件マスタ検索
                sql = new StringBuilder();
                sql.AppendLine(" select seq");
                sql.AppendLine("       ,credit_month_division");    // 入金/支払月区分
                sql.AppendLine("       ,note_sight");               // 入金/支払サイト(日単位)
                sql.AppendLine("       ,note_sight_month");         // 入金/支払サイト(月単位)
                sql.AppendLine("       ,bound_amount");             // 境界額
                sql.AppendLine("       ,credit_division");          // 入金/支払区分
                sql.AppendLine("       ,credit_scheduled_date");    // 入金/支払予定日(0-99)
                sql.AppendLine(" from   vender_credit_detail");
                sql.AppendLine(" where  vender_division = @VenderDivision");
                sql.AppendLine(" and    vender_cd = @VenderCd");
                sql.AppendLine(" and    active_date = @ActiveDate");
                sql.AppendLine(" and    credit_division is not null");
                sql.AppendLine(" order by bound_amount");
                // SQL実行
                IList<EntityDao.VenderCreditDetailEntity> resultList = db.GetListByDataClass<EntityDao.VenderCreditDetailEntity>(sql.ToString(),
                    new { VenderDivision = paramVenderDivision, VenderCd = paramVenderCd, ActiveDate = vender.ActiveDate });
                if (resultList == null || resultList.Count == 0)
                {
                    string message = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.GetPropertiesMessage(APCommonUtil.APResources.ID.MB10186);
                    throw new Exception(message + condition);
                }
                if (resultList != null)
                {
                    foreach (var result in resultList)
                    {
                        // 最終データもしくは金額による選定
                        if ((result.BoundAmount ?? 0) >= paramAmount)
                        {
                            numCreditMonthDiv = result.CreditMonthDivision ?? 0;
                            numCreditScheDay = result.CreditScheduledDate ?? 0;
                            numCreditDiv = result.CreditDivision ?? 0;
                            numCreditMonthDiv = result.CreditMonthDivision ?? 0;
                            numCreditScheDay = result.CreditScheduledDate ?? 0;
                            numNoteSite = result.NoteSight ?? 0;
                            numNoteSiteMonth = result.NoteSightMonth ?? 0;
                            if (numNoteSightDivision != 1)
                            {
                                numNoteSite = numNoteSiteMonth;     // 入金/支払サイト(月単位)
                            }
                            if (resultList.Count == cnt && (result.BoundAmount ?? 0) < paramAmount)
                            {
                                // 境界額以上の額を締めようとした場合
                            }
                            break;
                        }
                    }
                }
                // 設定に当てはまらない場合
                if (numCreditMonthDiv.Equals(0))
                {
                    string message = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.GetPropertiesMessage(APCommonUtil.APResources.ID.M10249, APCommonUtil.APResources.ID.I20142);
                    throw new Exception(message + condition);
                }

                if (numNoteSightDivision == 2)
                {
                    // 月単位
                    numNoteSite = FunGetNoteSight(paramCreditScheduledDate, numNoteSiteMonth);
                }

                paramNoteSightDivision = numNoteSightDivision;
                paramCreditDivision = numCreditDiv;

                return numNoteSite;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                // エラー時の戻り値設定
                return 999;
            }
        }

        /// <summary>
        /// システム時刻を取得する
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <returns>システム時刻</returns>
        public static DateTime GetActiveLoginTime(ComDB db)
        {
            int procid = -1;
            DateTime loginTime = System.DateTime.Now;
            DateTime? currentLoginTime = null;
            var sql = "";
            EntityDao.DbStorageEntity results = null;

            try
            {
                switch (db.DbType)
                {
                    case ComDB.DBType.SQLServer:
                        sql = "";
                        sql = sql + "select login_time from master..sysprocesses where spid=@@spid";
                        // SQL実行
                        results = db.GetEntity<EntityDao.DbStorageEntity>(sql);
                        if (results != null)
                        {
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            loginTime = DateTime.Parse(results.LoginTime.ToString());
                        }
                        results = null;
                        sql = "";
                        sql = sql + "select v_value as procid from dbo.db_storage with (READCOMMITTEDLOCK) ";
                        sql = sql + " where login_time= cast(@LoginTime as datetime) and spid=@@spid and name='$login_time$'";
                        sql = sql + "  and exists (select * from dbo.db_storage with (READCOMMITTEDLOCK) where ";
                        sql = sql + "  login_time= cast(@LoginTime as datetime) and spid=@@spid and name='$spid$')";
                        // SQL実行
                        results = db.GetEntity<EntityDao.DbStorageEntity>(sql, new {LoginTime = string.Format("{0:yyyy/MM/dd hh:mm:ss}", loginTime) });
                        if (results != null)
                        {
                            // 確認
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            procid = int.Parse(results.VValue.ToString());
                        }

                        break;
                    //case ComDB.DBType.Oracle: Orcleは使用しないためコメントアウト
                    //    sql = "";
                    //    sql = sql + "select  logon_time ";
                    //    sql = sql + "  from v$session ";
                    //    sql = sql + " where AUDSID=userenv('SESSIONID') ";
                    //    // SQL実行
                    //    results = db.GetEntity<EntityDao.DbStorageEntity>(sql);
                    //    if (results != null)
                    //    {
                    //        //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    //        loginTime = DateTime.Parse(results.logon["logon_time"].ToString());
                    //    }
                    //    results = "";
                    //    sql = "";
                    //    sql = sql + "select v_value  as procid from dbo.db_storage ";
                    //    sql = sql + " where login_time=cast('" + string.Format("{0:yyyy/MM/dd hh:mm:ss}", loginTime) + "' as timestamp) ";
                    //    sql = sql + " and spid=userenv('SESSIONID') and name='$login_time$'";
                    //    sql = sql + "  and exists (select * from dbo.db_storage ";
                    //    sql = sql + "               where login_time=cast('" + string.Format("{0:yyyy/MM/dd hh:mm:ss}", loginTime) + "' as timestamp) ";
                    //    sql = sql + "                 and spid=userenv('SESSIONID') and name='$spid$')";
                    //    // SQL実行
                    //    results = db.GetEntity<EntityDao.DbStorageEntity>(sql);
                    //    if (results != null)
                    //    {
                    //        //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    //        currentLoginTime = DateTime.Parse(results.currentlo["current_login_time"].ToString());
                    //    }
                    //    break;
                    case ComDB.DBType.PostgreSQL:
                        sql = "";
                        sql = sql + "select backend_start ";
                        sql = sql + "  from pg_stat_activity ";
                        sql = sql + " where pid = pg_backend_pid() ";
                        // SQL実行
                        string backendResults = db.GetEntity<string>(sql);
                        if (results != null)
                        {
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            currentLoginTime = DateTime.Parse(backendResults.ToString());
                        }
                        backendResults = "";
                        sql = "";
                        sql = sql + "select v_value  as procid from dbo.db_storage ";
                        sql = sql + " where login_time=cast(@LoginTime as timestamp) ";
                        sql = sql + " and spid=pg_backend_pid and name='$login_time$'";
                        sql = sql + "  and exists (select * from dbo.db_storage ";
                        sql = sql + "               where login_time=cast(@LoginTime as timestamp) ";
                        sql = sql + "                 and spid=pg_backend_pid() and name='$spid$')";
                        // SQL実行
                        string stringResults = db.GetEntity<string>(sql, new {LoginTime = string.Format("{0:yyyy/MM/dd hh:mm:ss}", loginTime) });
                        if (results != null)
                        {
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            currentLoginTime = DateTime.Parse(stringResults);
                        }
                        break;
                }

                return (DateTime)currentLoginTime;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// サーバープロセスIDを取得する
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <returns>サーバープロセスID</returns>
        public static int GetActiveSpid(ComDB db)
        {
            int procid = -1;
            DateTime? loginTime = null;
            var sql = "";
            EntityDao.DbStorageEntity results = null;

            try
            {
                switch (db.DbType)
                {
                    case ComDB.DBType.SQLServer:
                        sql = "";
                        sql = sql + "select login_time from master..sysprocesses where spid=@@spid";
                        // SQL実行
                        string loginTimeResults = db.GetEntity<string>(sql);
                        if (results != null)
                        {
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            loginTime = DateTime.Parse(loginTimeResults.ToString());
                        }

                        sql = "";
                        sql = sql + "select v_value as procid from dbo.db_storage with (READCOMMITTEDLOCK) ";
                        sql = sql + " where login_time= cast(@LoginTime as datetime) ";
                        sql = sql + "   and spid=@@spid and name='$spid$'";
                        sql = sql + "  and exists (select * from dbo.db_storage with (READCOMMITTEDLOCK) ";
                        sql = sql + "               where login_time=cast(@LoginTime as datetime) ";
                        sql = sql + "                 and spid=@@spid and name='$login_time$')";
                        // SQL実行
                        results = db.GetEntity<EntityDao.DbStorageEntity>(sql, new {LoginTime = string.Format("{0:yyyy/MM/dd hh:mm:ss}", loginTime) });
                        if (results != null)
                        {
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            procid = int.Parse(results.VValue.ToString());
                        }

                        break;
                    case ComDB.DBType.Oracle:
                        sql = "";
                        sql = sql + "select  logon_time ";
                        sql = sql + "  from v$session ";
                        sql = sql + " where AUDSID=userenv('SESSIONID') ";
                        // SQL実行
                        string logonResults = db.GetEntity<string>(sql);
                        if (results != null)
                        {
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            loginTime = DateTime.Parse(logonResults.ToString());
                        }

                        sql = "";
                        sql = sql + "select v_value  as procid from dbo.db_storage ";
                        sql = sql + " where login_time=cast(@LoginTime as timestamp) ";
                        sql = sql + " and spid=userenv('SESSIONID') and name='$spid$'";
                        sql = sql + "  and exists (select * from dbo.db_storage ";
                        sql = sql + "               where login_time=cast(@LoginTime as timestamp) ";
                        sql = sql + "                 and spid=userenv('SESSIONID') and name='$login_time$')";
                        // SQL実行
                        results = db.GetEntity<EntityDao.DbStorageEntity>(sql, new {LoginTime = string.Format("{0:yyyy/MM/dd hh:mm:ss}", loginTime) });
                        if (results != null)
                        {
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            procid = int.Parse(results.VValue.ToString());
                        }
                        break;
                    case ComDB.DBType.PostgreSQL:
                        sql = "";
                        sql = sql + "select backend_start ";
                        sql = sql + "  from pg_stat_activity ";
                        sql = sql + " where pid = pg_backend_pid() ";
                        // SQL実行
                        string backendResults = db.GetEntity<string>(sql);
                        if (results != null)
                        {
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            loginTime = DateTime.Parse(backendResults.ToString());
                        }

                        sql = "";
                        sql = sql + "select v_value  as procid from dbo.db_storage ";
                        sql = sql + " where login_time=cast(@LoginTime as timestamp) ";
                        sql = sql + " and spid=pg_backend_pid and name='$spid$'";
                        sql = sql + "  and exists (select * from dbo.db_storage ";
                        sql = sql + "               where login_time=cast(@LoginTime as timestamp) ";
                        sql = sql + "                 and spid=pg_backend_pid() and name='$login_time$')";
                        // SQL実行
                        string valueResults = db.GetEntity<string>(sql, new {LoginTime = string.Format("{0:yyyy/MM/dd hh:mm:ss}", loginTime) });
                        if (results != null)
                        {
                            //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                            procid = int.Parse(valueResults.ToString());
                        }
                        break;
                }

                return procid;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 数値の切り捨てを行う
        /// </summary>
        /// <param name="x">値</param>
        /// <param name="n">乗数</param>
        /// <returns>処理後の値</returns>
        public static decimal Trunc(decimal x, int n = 0)
        {
            decimal retval;
            decimal ope;

            try
            {
                ope = decimal.Parse(Math.Pow(10, n).ToString());

                if (x > 0)
                {
                    retval = Math.Floor(x * ope) / ope;
                }
                else
                {
                    retval = Math.Ceiling(x * ope) / ope;
                }

                return retval;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ロット番号の一部分取得
        /// </summary>
        /// <param name="lotNo">ロット番号</param>
        /// <returns>変換後ロット番号</returns>
        public static string FncGetBagNo(string lotNo)
        {
            string retVal = "";

            try
            {
                // ロット番号から一部分を取得
                retVal = lotNo.Substring(14, 6);
                return retVal;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ロット番号の一部分取得
        /// </summary>
        /// <param name="lotNo">ロット番号</param>
        /// <param name="styleOfPacking">梱包形態</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>変換後ロット番号</returns>
        public static string FncGetLotNo(string lotNo, string styleOfPacking, ComDB db)
        {
            var sql = "";
            EntityDao.NamesEntity results = null;
            string code = "";
            try
            {
                sql = sql + "select mecode1 ";
                sql = sql + " from names";
                sql = sql + " where name_division = 'CIFS' ";
                sql = sql + " and name_cd = @NameCd";

                results = db.GetEntity<EntityDao.NamesEntity>(sql, new { NameCd = styleOfPacking });

                if (results != null)
                {
                    //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                    code = results.Mecode1;

                    if (code == "FLECON")
                    {
                        return lotNo.Substring(0, 14);
                    }
                }

                return lotNo;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return lotNo;
            }
        }
    }
}
