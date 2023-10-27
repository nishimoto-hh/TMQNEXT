using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonSTDUtil.CommonLogger;
using System.Globalization;

using APDao = CommonAPUtil.APCommonUtil.APCommonUtilDataClass;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using EntityDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using fncUtil = CommonAPUtil.APStoredFncUtil.APStoredFncUtil;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>
    /// 共通ファンクションクラス
    /// </summary>
    public class APStoredPrcUtil
    {
        #region クラス内変数
        /// <summary>ログ出力</summary>
        private static CommonLogger logger = CommonLogger.GetInstance("logger");
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public APStoredPrcUtil()
        {
        }
        #endregion

        ///// <summary>
        ///// 品目有効化
        ///// </summary>
        ///// <param name="itemCd">品目コード</param>
        ///// <param name="db">DB操作クラス</param>
        ///// <returns>true:正常終了 false:異常終了</returns>
        ///// <remarks>未使用、仕様変更により使用不能と思われる</remarks>
        //public static bool CopyItem(string itemCd, ComDB db)
        //{
        //    string workItemCd;
        //    int workVersion;
        //    int regFlg = 0;

        //    try
        //    {
        //        // 品目の操作
        //        var sql = "";
        //        sql = sql + "select distinct item_queue.item_cd ";
        //        sql = sql + "  from item_queue ";
        //        sql = sql + " where item_queue.status = 3 ";
        //        sql = sql + "   and item_queue.active_date <= current_timestamp ";
        //        sql = sql + "   and ( ";
        //        sql = sql + "       ('" + itemCd + "' = '')"; // 引数の品目コードがなければ、全レコード対象
        //        sql = sql + "       or ( ";
        //        sql = sql + "           '" + itemCd + "' is not nulll ";
        //        sql = sql + "           and item_queue.itemcd = '" + itemCd + "' ";
        //        sql = sql + "          )"; // 引数の品目コードがあれば、その品目のみ対象
        //        sql = sql + "       )";

        //        IList<string> resultList = null;
        //        dynamic results = null;
        //        // SQL実行
        //        resultList = db.GetList<string>(sql);
        //        if (resultList != null)
        //        {
        //            for (int i = 0; i < resultList.Count; i++)
        //            {
        //                results = resultList[i];

        //                // FFF
        //                IDictionary<string, object> dicResult = results as IDictionary<string, object>;
        //                workItemCd = dicResult["item_cd"].ToString();

        //                try
        //                {
        //                    sql = "";
        //                    sql = sql + "select a.version ";
        //                    sql = sql + "  from item_queue as a";
        //                    sql = sql + " where a.item_cd = '" + itemCd + "' ";
        //                    sql = sql + "   and a.status = 3 ";
        //                    sql = sql + "   and a.active_date = (";
        //                    sql = sql + "       select max(b.active_date) as expr ";
        //                    sql = sql + "         from item_queue as b ";
        //                    sql = sql + "        where b.active_date <= current_timestamp ";
        //                    sql = sql + "          and b.item_cd = '" + itemCd + "' ";
        //                    sql = sql + "          and b.status = 3 ";
        //                    results = null;
        //                    // SQL実行
        //                    results = db.GetEntity<EntityDao.ItemEntity>(sql);
        //                    if (results != null)
        //                    {
        //                        workVersion = int.Parse(dicResult["version"].ToString());

        //                        // "ITEM_CD:" + itemCd + " ITEM START";

        //                        // いったん削除
        //                        sql = "";
        //                        sql = sql + "delete from item ";
        //                        sql = sql + " where item_cd = '" + workItemCd + "' ";
        //                        regFlg = db.Regist(sql);
        //                        if (regFlg < 0)
        //                        {
        //                            // 異常終了
        //                            return false;
        //                        }

        //                        // 現在日時で有効なもののみインサート
        //                        sql = "";
        //                        sql = sql + "insert item into ( ";
        //                        sql = sql + "    item_cd ";
        //                        sql = sql + "    , version ";
        //                        sql = sql + "    , item_name ";
        //                        sql = sql + "    , item_name_common ";
        //                        sql = sql + "    , item_sub_name ";
        //                        sql = sql + "    , item_sub_name_common ";
        //                        sql = sql + "    , active_date ";
        //                        sql = sql + "    , unit_of_stock_control ";
        //                        sql = sql + "    , unit_of_operation_management ";
        //                        sql = sql + "    , status ";
        //                        sql = sql + "    , receive_tanto_cd ";
        //                        sql = sql + "    , approval_request_tanto_cd ";
        //                        sql = sql + "    , approval_request_date ";
        //                        sql = sql + "    , approval_tanto_cd ";
        //                        sql = sql + "    , approval_date ";
        //                        sql = sql + "    , parent_item_cd ";
        //                        sql = sql + "    , activate_flag ";
        //                        sql = sql + "    , item_type ";
        //                        sql = sql + "    , input_date ";
        //                        sql = sql + "    , inputor_cd ";
        //                        sql = sql + "    , update_date ";
        //                        sql = sql + "    , updator_cd ";
        //                        sql = sql + "    ) values ( ";
        //                        sql = sql + "    select a.item_cd ";
        //                        sql = sql + "    , a.version ";
        //                        sql = sql + "    , a.item_name ";
        //                        sql = sql + "    , a.item_name_common ";
        //                        sql = sql + "    , a.item_sub_name ";
        //                        sql = sql + "    , a.item_sub_name_common ";
        //                        sql = sql + "    , a.active_date ";
        //                        sql = sql + "    , a.unit_of_stock_control ";
        //                        sql = sql + "    , a.unit_of_operation_management ";
        //                        sql = sql + "    , a.status ";
        //                        sql = sql + "    , a.receive_tanto_cd ";
        //                        sql = sql + "    , a.approval_request_tanto_cd ";
        //                        sql = sql + "    , a.approval_request_date ";
        //                        sql = sql + "    , a.approval_tanto_cd ";
        //                        sql = sql + "    , a.approval_date ";
        //                        sql = sql + "    , a.parent_item_cd ";
        //                        sql = sql + "    , a.activate_flag ";
        //                        sql = sql + "    , a.item_type ";
        //                        sql = sql + "    , a.input_date ";
        //                        sql = sql + "    , a.inputor_cd ";
        //                        sql = sql + "    , current_timestamp ";
        //                        sql = sql + "    , a.updator_cd ";
        //                        sql = sql + "     from item_queue as a ";
        //                        sql = sql + "    where a.item_cd = '" + workItemCd + "' ";
        //                        sql = sql + "      and a.version = " + workVersion;
        //                        sql = sql + "    ) ";

        //                        regFlg = db.Regist(sql);
        //                        if (regFlg < 0)
        //                        {
        //                            // 異常終了
        //                            return false;
        //                        }

        //                        // "ITEM_CD:" + itemCd + " ITEM END";
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    logger.Error(ex.Message);
        //                    throw ex;
        //                }
        //            }
        //        }

        //        // 品目仕様の操作
        //        sql = "";
        //        sql = sql + "select distinct item_specification_queue.item_cd ";
        //        sql = sql + "  from item_specification_queue ";
        //        sql = sql + " where item_specification_queue.status = 3 ";
        //        sql = sql + "   and item_specification_queue.active_date <= current_timestamp ";
        //        sql = sql + "   and ( ";
        //        sql = sql + "       ('" + itemCd + "' = '')"; // 引数の品目コードがなければ、全レコード対象
        //        sql = sql + "       or ( ";
        //        sql = sql + "           '" + itemCd + "' is not nulll ";
        //        sql = sql + "           and item_specification_queue.itemcd = '" + itemCd + "' ";
        //        sql = sql + "          )"; // 引数の品目コードがあれば、その品目のみ対象
        //        sql = sql + "       )";

        //        resultList = null;
        //        results = null;
        //        // SQL実行
        //        resultList = db.GetList<string>(sql);
        //        if (resultList != null)
        //        {
        //            for (int i = 0; i < resultList.Count - 1; i++)
        //            {
        //                results = resultList[i];

        //                IDictionary<string, object> dicResult = results as IDictionary<string, object>;
        //                workItemCd = dicResult["item_cd"].ToString();

        //                try
        //                {
        //                    // "ITEM_CD:" + itemCd + " SPEC START";

        //                    // いったん削除
        //                    sql = "";
        //                    sql = sql + "delete from item_specification ";
        //                    sql = sql + " where item_cd = '" + workItemCd + "' ";
        //                    regFlg = db.Regist(sql);
        //                    if (regFlg < 0)
        //                    {
        //                        // 異常終了
        //                        return false;
        //                    }

        //                    // 現在日時で有効なもののみインサート
        //                    sql = "";
        //                    sql = sql + "insert item_specification into ( ";
        //                    sql = sql + "    item_cd ";
        //                    sql = sql + "    , specification_code ";
        //                    sql = sql + "    , version ";
        //                    sql = sql + "    , specification_name ";
        //                    sql = sql + "    , specification_name_c ";
        //                    sql = sql + "    , specification_sub_name ";
        //                    sql = sql + "    , specification_sub_name_c ";
        //                    sql = sql + "    , spec_value1 ";
        //                    sql = sql + "    , spec_value2 ";
        //                    sql = sql + "    , spec_value3 ";
        //                    sql = sql + "    , item_type ";
        //                    sql = sql + "    , product_division ";
        //                    sql = sql + "    , article_division ";
        //                    sql = sql + "    , purchase_division ";
        //                    sql = sql + "    , inspection_type ";
        //                    sql = sql + "    , kg_of_fraction_management ";
        //                    sql = sql + "    , unit_of_stock_control ";
        //                    sql = sql + "    , unit_of_operation_management ";
        //                    sql = sql + "    , number_of_insertions ";
        //                    sql = sql + "    , item_category ";
        //                    sql = sql + "    , stock_division ";
        //                    sql = sql + "    , default_location ";
        //                    sql = sql + "    , active_date ";
        //                    sql = sql + "    , all_up_weight ";
        //                    sql = sql + "    , style_of_packing ";
        //                    sql = sql + "    , lot_division ";
        //                    sql = sql + "    , spot_division ";
        //                    sql = sql + "    , cost_accounts ";
        //                    sql = sql + "    , receive_tanto_cd ";
        //                    sql = sql + "    , approval_request_tanto_cd ";
        //                    sql = sql + "    , approval_request_date ";
        //                    sql = sql + "    , approval_tanto_cd ";
        //                    sql = sql + "    , approval_date ";
        //                    sql = sql + "    , status ";
        //                    sql = sql + "    , llc ";
        //                    sql = sql + "    , kg_conversion_opration ";
        //                    sql = sql + "    , kg_conversion_coefficient ";
        //                    sql = sql + "    , unit_of_fraction_management ";
        //                    sql = sql + "    , unit_of_useal ";
        //                    sql = sql + "    , activate_flag ";
        //                    sql = sql + "    , input_date ";
        //                    sql = sql + "    , inputor_cd ";
        //                    sql = sql + "    , update_date ";
        //                    sql = sql + "    , updator_cd ";
        //                    sql = sql + "    ) ( ";
        //                    sql = sql + "    select isq.item_cd ";
        //                    sql = sql + "    , isq.specification_code ";
        //                    sql = sql + "    , isq.version ";
        //                    sql = sql + "    , isq.specification_name ";
        //                    sql = sql + "    , isq.specification_name_c ";
        //                    sql = sql + "    , isq.specification_sub_name ";
        //                    sql = sql + "    , isq.specification_sub_name_c ";
        //                    sql = sql + "    , isq.spec_value1 ";
        //                    sql = sql + "    , isq.spec_value2 ";
        //                    sql = sql + "    , isq.spec_value3 ";
        //                    sql = sql + "    , isq.item_type ";
        //                    sql = sql + "    , isq.product_division ";
        //                    sql = sql + "    , isq.article_division ";
        //                    sql = sql + "    , isq.purchase_division ";
        //                    sql = sql + "    , isq.inspection_type ";
        //                    sql = sql + "    , isq.kg_of_fraction_management ";
        //                    sql = sql + "    , isq.unit_of_stock_control ";
        //                    sql = sql + "    , isq.unit_of_operation_management ";
        //                    sql = sql + "    , isq.number_of_insertions ";
        //                    sql = sql + "    , isq.item_category ";
        //                    sql = sql + "    , isq.stock_division ";
        //                    sql = sql + "    , isq.default_location ";
        //                    sql = sql + "    , isq.active_date ";
        //                    sql = sql + "    , isq.all_up_weight ";
        //                    sql = sql + "    , isq.style_of_packing ";
        //                    sql = sql + "    , isq.lot_division ";
        //                    sql = sql + "    , isq.spot_division ";
        //                    sql = sql + "    , isq.cost_accounts ";
        //                    sql = sql + "    , isq.receive_tanto_cd ";
        //                    sql = sql + "    , isq.approval_request_tanto_cd ";
        //                    sql = sql + "    , isq.approval_request_date ";
        //                    sql = sql + "    , isq.approval_tanto_cd ";
        //                    sql = sql + "    , isq.approval_date ";
        //                    sql = sql + "    , isq.status ";
        //                    sql = sql + "    , null ";
        //                    sql = sql + "    , isq.kg_conversion_opration ";
        //                    sql = sql + "    , isq.kg_conversion_coefficient ";
        //                    sql = sql + "    , isq.unit_of_fraction_management ";
        //                    sql = sql + "    , isq.unit_of_useal ";
        //                    sql = sql + "    , isq.activate_flag ";
        //                    sql = sql + "    , isq.input_date ";
        //                    sql = sql + "    , isq.inputor_cd ";
        //                    sql = sql + "    , current_timestamp ";
        //                    sql = sql + "    , isq.updator_cd ";
        //                    sql = sql + "    from item_specification_queue isq inner join item c ";
        //                    sql = sql + "      on isq.item_cd = c.item_cd ";
        //                    sql = sql + "      and isq.version = c.version "; //品目が有効なバージョンのみに限定
        //                    sql = sql + "    where isq.active_date <= current_timestamp ";
        //                    sql = sql + "      and isq.item_cd = '" + workItemCd + "' ";
        //                    sql = sql + "      and isq.status = 3 ";
        //                    sql = sql + "       ) ";

        //                    regFlg = db.Regist(sql);
        //                    if (regFlg < 0)
        //                    {
        //                        // 異常終了
        //                        return false;
        //                    }

        //                    // "ITEM_CD:" + itemCd + " SPEC END";
        //                }
        //                catch (Exception ex)
        //                {
        //                    logger.Error(ex.Message);
        //                    throw ex;
        //                }
        //            }
        //        }

        //        // 正常終了
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 取引先有効化
        ///// </summary>
        ///// <param name="venderDivision">取引先区分</param>
        ///// <param name="venderCd">取引先コード</param>
        ///// <param name="rtnCd">戻値</param>
        ///// <param name="errCd">エラーコード</param>
        ///// <param name="errMessage">エラーメッセージ</param>
        ///// <param name="db">DB操作クラス</param>
        ///// <returns>true:正常終了 false:異常終了</returns>
        //public static bool CopyVender(string venderDivision, string venderCd, out int rtnCd, out int errCd, out string errMessage, ComDB db)
        //{
        //    string curVenderDivision;        // 取引先区分
        //    string curVenderCd;              // 取引先コード
        //    int curSeqNo;                    // 連番
        //    DateTime curValidDate;           // 有効開始日
        //    int curTaxRoundup;               // 消費税端数区分
        //    int curTaxRoundupUnit;           // 消費税端数単位
        //    int curTaxIntegerLength;         // 全体桁数(消費税)
        //    int curTaxSmallnumLength;        // 整数部桁数(消費税)
        //    int curRoundup;                  // 端数処理区分
        //    int curRoundupUnit;              // 端数処理単位
        //    int curIntegerLength;            // 全体桁数(端数)
        //    int curSmallnumLength;           // 整数部桁数(端数)
        //    int curSalesPurchaseRoundup;     // 売上仕入金額端数処理
        //    int curSalesPurchaseRoundupUnit; // 売上仕入金額端数単位
        //    int curSalesIntegerLength;       // 全体桁数(売上)
        //    int curSalesSmallnumLength;      // 整数部桁数(売上)
        //    int curUnitpriceRoundup;         // 単価端数処理
        //    int curUnitpriceRoundupUnit;     // 単価端数単位
        //    int curUnitpriceIntegerLength;   // 全体桁数(単価)
        //    int curUnitpriceSmallnumLength;  // 整数部桁数(単価)
        //    DateTime curInputDate;           // 登録日時
        //    string curInputorCd;             // 登録者ID
        //    DateTime curUpdateDate;          // 更新日時
        //    string curUpdatorCd;             // 更新者ID

        //    var sql = "";
        //    int regFlg = 0;

        //    rtnCd = 0;
        //    errCd = 0;
        //    errMessage = "";

        //    try
        //    {
        //        // 過去直近有効開始日で承認済みの VENDER_QUEUE が VENDER にないもの
        //        // 数値桁数チェックマスタ登録用の情報も取得
        //        var inssql = "";
        //        inssql = inssql + "select vq_a.* ";
        //        inssql = inssql + "     , vq_b.seq_no ";
        //        inssql = inssql + "     , vq_b.input_date ";
        //        inssql = inssql + "     , vq_b.inptor_cd ";
        //        inssql = inssql + "     , vq_b.update_date ";
        //        inssql = inssql + "     , vq_b.updator_cd ";
        //        inssql = inssql + "  from ";
        //        inssql = inssql + "   (select vender_division ";
        //        inssql = inssql + "         , vender_cd ";
        //        inssql = inssql + "         , max(valid_date) as valid_date ";
        //        inssql = inssql + "         , tax_roundup ";
        //        inssql = inssql + "         , tax_roundup_unit ";
        //        inssql = inssql + "         , (case tax_roundup_unit when 1 then 18 else 18 - tax_roundup_unit end) as tax_integer_length ";
        //        inssql = inssql + "         , (case tax_roundup_unit when 1 then 0 else tax_roundup - 1 end) as tax_smallnum_length ";
        //        inssql = inssql + "         , sales_purchase_roundup ";
        //        inssql = inssql + "         , sales_purchase_rountup_unit ";
        //        inssql = inssql + "         , (case sales_purchase_rountup_unit when 1 then 18 else 18 - sales_purchase_rountup_unit end) as sales_integer_length ";
        //        inssql = inssql + "         , (case sales_purchase_rountup_unit when 1 then 0 else sales_purchase_rountup_unit - 1 end) as sales_smallnum_length ";
        //        inssql = inssql + "         , unitprice_roundup ";
        //        inssql = inssql + "         , unitprice_roundup_unit ";
        //        inssql = inssql + "         , (case unitprice_roundup_unit when 1 then 18 else 18 - unitprice_roundup_unit end) as unitprice_integer_length ";
        //        inssql = inssql + "         , (case unitprice_roundup_unit when 1 then 0 else unitprice_roundup_unit - 1 end) as unitprice_smallnum_length ";
        //        inssql = inssql + "     from vender_queue ";
        //        inssql = inssql + "    where valid_date <= current_timestamp ";
        //        inssql = inssql + "      and approval_status = 3 ";
        //        inssql = inssql + "   group by vender_division ";
        //        inssql = inssql + "          , vender_cd ";
        //        inssql = inssql + "          , tax_roundup ";
        //        inssql = inssql + "          , tax_roundup_unit ";
        //        inssql = inssql + "          , roundup ";
        //        inssql = inssql + "          , roundup_unit ";
        //        inssql = inssql + "          , sales_purchase_roundup ";
        //        inssql = inssql + "          , sales_purchase_roundup_unit ";
        //        inssql = inssql + "          , unitprice_roundup ";
        //        inssql = inssql + "          , unitprice_roundup_unit ";
        //        inssql = inssql + "   ) vq_a ";
        //        inssql = inssql + "  left join vender_queue vq_b ";
        //        inssql = inssql + "     on vq_a.vender_division = vq_b.vender_division ";
        //        inssql = inssql + "     and vq_a.vender_cd = vq_b.vender_cd ";
        //        inssql = inssql + "     and vq_a.valid_date = vq_b.valid_date ";
        //        inssql = inssql + " where not exists (select 1 from vender where vq_a.vender_division = vq_b.vender_division and vq_a.vender_cd = vq_b.vender_cd and vq_a.seq_no = vq_b.vender_cd ";

        //        // 引数の指定がある場合は個別対象
        //        if (venderDivision != null)
        //        {
        //            inssql = inssql + "  and vq_a.vender_division = '" + venderDivision + "' ";

        //        }
        //        if (venderCd != null)
        //        {
        //            inssql = inssql + "  and vq_a.vender_cd = '" + venderCd + "' ";

        //        }

        //        // VENDER にあって、過去直近有効開始日で承認済みの VENDER_QUEUE がないもの
        //        // (VENDER_QUEUE から 削除されたもの
        //        // (VENDER_QUEUE で承認取消しされたもの)
        //        var delsql = "";
        //        delsql = delsql + "select distinct a.vender_division ";
        //        delsql = delsql + "     , a.vender_cd ";
        //        delsql = delsql + "  from vender a ";
        //        delsql = delsql + " where not exists (";
        //        delsql = delsql + "     select 1 ";
        //        delsql = delsql + "       from vender_queue b ";
        //        delsql = delsql + "      where a.vender_division = b.vender_division ";
        //        delsql = delsql + "        and a.seq_no = b.seq_no ";
        //        delsql = delsql + "        and a.vender_cd = b.vender_cd ";
        //        delsql = delsql + "     )";
        //        // 引数の指定がある場合は個別対象
        //        if (venderDivision != null)
        //        {
        //            delsql = delsql + " and a.vender_division = '" + venderDivision + "' ";
        //        }
        //        if (venderCd != null)
        //        {
        //            delsql = delsql + " and a.vender_cd = '" + venderCd + "' ";
        //        }
        //        delsql = delsql + " union all ";
        //        delsql = delsql + " select distinct a.vender_division ";
        //        delsql = delsql + "      , a.vender_cd ";
        //        delsql = delsql + "   from vender_queue a ";
        //        delsql = delsql + "  where exists (";
        //        delsql = delsql + "      select 1 ";
        //        delsql = delsql + "        from vender b ";
        //        delsql = delsql + "       where b.vender_division = a.vender_division ";
        //        delsql = delsql + "        and a.seq_no = b.seq_no ";
        //        delsql = delsql + "        and a.vender_cd = b.vender_cd ";
        //        delsql = delsql + "     )";
        //        delsql = delsql + "    and a.approval_status != 3 ";
        //        delsql = delsql + "    and a.valid_date <= current_timestamp ";
        //        // 引数の指定がある場合は個別対象
        //        if (venderDivision != null)
        //        {
        //            delsql = delsql + " and a.vender_division = '" + venderDivision + "' ";
        //        }
        //        if (venderCd != null)
        //        {
        //            delsql = delsql + " and a.vender_cd = '" + venderCd + "' ";
        //        }

        //        errMessage = "削除処理開始 > ";

        //        // ************************************
        //        // 削除処理
        //        // ************************************
        //        IList<int> resultList = null;
        //        dynamic results = null;
        //        // SQL実行
        //        resultList = db.GetList<int>(delsql);
        //        if (resultList != null)
        //        {
        //            // 削除対象取引先マスタレコードのループ処理で削除する
        //            for (int i = 0; i < resultList.Count; i++)
        //            {
        //                results = resultList[i];

        //                IDictionary<string, object> dicResult = results as IDictionary<string, object>;
        //                curVenderDivision = dicResult["vender_division"].ToString();
        //                curVenderCd = dicResult["vender_cd"].ToString();

        //                errMessage = "取引先マスタ削除開始 > ";
        //                // 取引先マスタ
        //                sql = "";
        //                sql = sql + "delete from vender ";
        //                sql = sql + " where vender_division = '" + curVenderDivision + "' ";
        //                sql = sql + "   and vender_cd = '" + curVenderCd + "' ";
        //                // SQL実行
        //                regFlg = db.Regist(sql);
        //                if (regFlg < 0)
        //                {
        //                    // 異常終了
        //                    throw new Exception();
        //                }
        //            }
        //        }

        //        errMessage = "作成処理開始 > ";

        //        // ************************************
        //        // 作成処理
        //        // ************************************
        //        resultList = null;
        //        results = null;
        //        // SQL実行
        //        resultList = db.GetList<int>(inssql);
        //        if (resultList != null)
        //        {
        //            // 有効化対象取引先マスタレコードのループ処理で作成する
        //            // (DELETE / INSERT)
        //            for (int i = 0; i < resultList.Count; i++)
        //            {
        //                results = resultList[i];

        //                IDictionary<string, object> dicResult = results as IDictionary<string, object>;
        //                curVenderDivision = dicResult["vender_division"].ToString();
        //                curVenderCd = dicResult["vender_cd"].ToString();
        //                curValidDate = DateTime.Parse(dicResult["valid_date"].ToString());
        //                curTaxRoundup = int.Parse(dicResult["tax_roundup"].ToString());
        //                curTaxRoundupUnit = int.Parse(dicResult["tax_roundup_unit"].ToString());
        //                curTaxIntegerLength = int.Parse(dicResult["tax_integer_length"].ToString());
        //                curTaxSmallnumLength = int.Parse(dicResult["tax_smallnum_length"].ToString());
        //                curRoundup = int.Parse(dicResult["roundup"].ToString());
        //                curRoundupUnit = int.Parse(dicResult["roundup_unit"].ToString());
        //                curIntegerLength = int.Parse(dicResult["integer_length"].ToString());
        //                curSmallnumLength = int.Parse(dicResult["smallnum_length"].ToString());
        //                curSalesPurchaseRoundup = int.Parse(dicResult["sales_purchase_roundup"].ToString());
        //                curSalesPurchaseRoundupUnit = int.Parse(dicResult["sales_purchase_roundup_unit"].ToString());
        //                curSalesIntegerLength = int.Parse(dicResult["sales_integer_length"].ToString());
        //                curSalesSmallnumLength = int.Parse(dicResult["sales_smallnum_length"].ToString());
        //                curUnitpriceRoundup = int.Parse(dicResult["unitprice_roundup"].ToString());
        //                curUnitpriceRoundupUnit = int.Parse(dicResult["unitprice_roundup_unit"].ToString());
        //                curUnitpriceIntegerLength = int.Parse(dicResult["unitprice_integer_length"].ToString());
        //                curUnitpriceSmallnumLength = int.Parse(dicResult["unitprice_smallnum_length"].ToString());
        //                curSeqNo = int.Parse(dicResult["seq_no"].ToString());
        //                curInputDate = DateTime.Parse(dicResult["input_date"].ToString());
        //                curInputorCd = dicResult["inputor_cd"].ToString();
        //                curUpdateDate = DateTime.Parse(dicResult["update_date"].ToString());
        //                curUpdatorCd = dicResult["updator_cd"].ToString();

        //                // ++++++++++++++++++++++++++++++++
        //                // 取引先マスタ
        //                // ++++++++++++++++++++++++++++++++
        //                errMessage = "有効化対象取引先情報の事前削除 > ";
        //                sql = "";
        //                sql = sql + "delete from vender ";
        //                sql = sql + " where vender_division = '" + curVenderDivision + "' ";
        //                sql = sql + "   and vender_cd = '" + curVenderCd + "' ";
        //                // SQL実行
        //                regFlg = db.Regist(sql);
        //                if (regFlg < 0)
        //                {
        //                    // 異常終了
        //                    throw new Exception();
        //                }

        //                errMessage = "取引先マスタ作成開始 > ";
        //                sql = "";
        //                sql = sql + "insert into vender ";
        //                sql = sql + " select vender_division ";
        //                sql = sql + "  , vender_cd ";
        //                sql = sql + "  , seq_no ";
        //                sql = sql + "  , vender_name1 ";
        //                sql = sql + "  , vender_name2 ";
        //                sql = sql + "  , vender_shorted_name ";
        //                sql = sql + "  , payment_invoice_cd ";
        //                sql = sql + "  , organization_cd ";
        //                sql = sql + "  , gi_division ";
        //                sql = sql + "  , zipcode_no ";
        //                sql = sql + "  , address1 ";
        //                sql = sql + "  , address2 ";
        //                sql = sql + "  , address3 ";
        //                sql = sql + "  , tel_no ";
        //                sql = sql + "  , fax_no ";
        //                sql = sql + "  , mail ";
        //                sql = sql + "  , currency_code ";
        //                sql = sql + "  , vender_tanto_name ";
        //                sql = sql + "  , area_cd ";
        //                sql = sql + "  , represent_role ";
        //                sql = sql + "  , represent_person ";
        //                sql = sql + "  , tanto_cd ";
        //                sql = sql + "  , closing_date ";
        //                sql = sql + "  , note_sight_division ";
        //                sql = sql + "  , subcontract_law ";
        //                sql = sql + "  , advance_division ";
        //                sql = sql + "  , section_cd ";
        //                sql = sql + "  , accounts_cd ";
        //                sql = sql + "  , bill_publish ";
        //                sql = sql + "  , slip_publish ";
        //                sql = sql + "  , holiday_flg ";
        //                sql = sql + "  , calendar_cd ";
        //                sql = sql + "  , bank_cd ";
        //                sql = sql + "  , account_division ";
        //                sql = sql + "  , account_no ";
        //                sql = sql + "  , account_stockhold ";
        //                sql = sql + "  , other_bank_cd ";
        //                sql = sql + "  , other_account_division ";
        //                sql = sql + "  , other_account_no ";
        //                sql = sql + "  , other_account_stockhold ";
        //                sql = sql + "  , tax_division ";
        //                sql = sql + "  , calc_division ";
        //                sql = sql + "  , tax_roundup ";
        //                sql = sql + "  , tax_roundup_unit ";
        //                sql = sql + "  , roundup ";
        //                sql = sql + "  , roundup_unit ";
        //                sql = sql + "  , sales_purchase_roundup ";
        //                sql = sql + "  , sales_purchase_roundup_unit ";
        //                sql = sql + "  , unitprice_roundup ";
        //                sql = sql + "  , unitprice_roundup_unit ";
        //                sql = sql + "  , customer_tanto_name1 ";
        //                sql = sql + "  , customer_tanto_name2 ";
        //                sql = sql + "  , customer_impression1 ";
        //                sql = sql + "  , customer_impression2 ";
        //                sql = sql + "  , remarks ";
        //                sql = sql + "  , vender_name1_common ";
        //                sql = sql + "  , vender_name2_common ";
        //                sql = sql + "  , vender_shorted_name_common ";
        //                sql = sql + "  , slip_send_name1 ";
        //                sql = sql + "  , slip_send_name2 ";
        //                sql = sql + "  , slip_send_zipcode_no ";
        //                sql = sql + "  , slip_send_address ";
        //                sql = sql + "  , slip_send_tel_no ";
        //                sql = sql + "  , slip_send_fax_no ";
        //                sql = sql + "  , slip_send_mail ";
        //                sql = sql + "  , tax_ratio ";
        //                sql = sql + "  , credit_limit_price ";
        //                sql = sql + "  , transfer_commission_load ";
        //                sql = sql + "  , input_date ";
        //                sql = sql + "  , inputor_cd ";
        //                sql = sql + "  , current_timestamp ";
        //                sql = sql + "  , updator_cd ";
        //                sql = sql + "   from vender_queue ";
        //                sql = sql + "   where vender_division = '" + curVenderDivision + "' ";
        //                sql = sql + "       and vender_cd = '" + curVenderCd + "' ";
        //                sql = sql + "       and seq_no = " + curSeqNo;
        //                // SQL実行
        //                regFlg = db.Regist(sql);
        //                if (regFlg < 0)
        //                {
        //                    // 異常終了
        //                    throw new Exception();
        //                }

        //                // ++++++++++++++++++++++++++++++++
        //                // ログテーブル
        //                // ++++++++++++++++++++++++++++++++
        //                errMessage = "ログデータ作成開始 > ";
        //                sql = "";
        //                sql = sql + "insert into vender_temp_log ";
        //                sql = sql + " values ( ";
        //                sql = sql + "    current_timestamp ";
        //                sql = sql + "  , '" + curVenderDivision + "' ";
        //                sql = sql + "  , '" + curVenderCd + "' ";
        //                sql = sql + "  ," + curSeqNo;
        //                sql = sql + ") ";
        //                // SQL実行
        //                regFlg = db.Regist(sql);
        //                if (regFlg < 0)
        //                {
        //                    // 異常終了
        //                    throw new Exception();
        //                }

        //                // ++++++++++++++++++++++++++++++++
        //                // 入金条件
        //                // ++++++++++++++++++++++++++++++++
        //                errMessage = "入金条件情報の事前削除 > ";
        //                sql = "";
        //                sql = sql + "delete from vender_credit_detail ";
        //                sql = sql + " where vender_division = '" + curVenderDivision + "' ";
        //                sql = sql + "   and vender_cd = '" + curVenderCd + "' ";
        //                // SQL実行
        //                regFlg = db.Regist(sql);
        //                if (regFlg < 0)
        //                {
        //                    // 異常終了
        //                    throw new Exception();
        //                }

        //                errMessage = "入金条件作成開始 > ";
        //                sql = "";
        //                sql = sql + "insert into vender_credit_detail ";
        //                sql = sql + " select vender_division ";
        //                sql = sql + "      , vender_cd ";
        //                sql = sql + "      , seq ";
        //                sql = sql + "      , seq_no ";
        //                sql = sql + "      , credit_month_division ";
        //                sql = sql + "      , note_sight ";
        //                sql = sql + "      , note_sight_month ";
        //                sql = sql + "      , bound_amount ";
        //                sql = sql + "      , credit_division ";
        //                sql = sql + "      , credit_scheduled_date ";
        //                sql = sql + "      , input_date ";
        //                sql = sql + "      , inputor_cd ";
        //                sql = sql + "      , current_timestamp ";
        //                sql = sql + "      , updator_cd ";
        //                sql = sql + "   from vender_credit_detail_queue ";
        //                sql = sql + "  where vender_division = '" + curVenderDivision + "' ";
        //                sql = sql + "    and vender_cd = '" + curVenderCd + "' ";
        //                sql = sql + "    and seq_no = " + curSeqNo;
        //                // SQL実行
        //                regFlg = db.Regist(sql);
        //                if (regFlg < 0)
        //                {
        //                    // 異常終了
        //                    throw new Exception();
        //                }

        //                rtnCd = 1; // 有効化あり
        //            }
        //        }

        //        // プロシジャ実行日を登録する
        //        sql = "";
        //        sql = sql + "select count(1) ";
        //        sql = sql + "  from copy_last_update ";
        //        sql = sql + " where copy_division = 1";
        //        int cnt = db.GetCount(sql);
        //        if (cnt == 0)
        //        {
        //            // データが無かった場合は、INSERT
        //            sql = "";
        //            sql = sql + "insert into copy_last_update ";
        //            sql = sql + " ( ";
        //            sql = sql + "    copy_division ";
        //            sql = sql + "  , copy_update_date ";
        //            sql = sql + " ) values ";
        //            sql = sql + " ( ";
        //            sql = sql + "    1 ";
        //            sql = sql + "  , current_timestamp ";
        //            sql = sql + " )";
        //        }
        //        else
        //        {
        //            // データがあった場合は、UPDATE
        //            sql = "";
        //            sql = sql + "update copy_last_update set ";
        //            sql = sql + " copy_update_date = current_timestamp ";
        //            sql = sql + " where copy_division = 1 ";
        //        }
        //        // SQL実行
        //        regFlg = db.Regist(sql);
        //        if (regFlg < 0)
        //        {
        //            // 異常終了
        //            throw new Exception();
        //        }

        //        // 正常終了
        //        errCd = 0;
        //        errMessage = "M00450"; // 正常処理完了
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // 異常終了
        //        rtnCd = 9;
        //        errMessage = ex.Message + errMessage;
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// シーケンス処理(OracleでのNEXTVALの代替処理)
        ///// </summary>
        ///// <param name="id">シーケンスID</param>
        ///// <param name="returnValue">戻り値</param>
        ///// <param name="db">DB操作クラス</param>
        ///// <returns>現在値</returns>
        //public static bool GetNextSequenceValue(string id, out int returnValue, ComDB db)
        //{
        //    returnValue = 0;

        //    int cnt = 0;
        //    int maxValue;
        //    int minValue;
        //    int incrementValue;
        //    int startValue;
        //    int? seqValue;
        //    int? valueWork;

        //    try
        //    {
        //        // トランザクション開始
        //        db.Connection.BeginTransaction();

        //        var sql = "";
        //        sql = sql + "select count(*) ";
        //        sql = sql + "  from sequence seq ";
        //        sql = sql + " where seq.id = '" + id + "' ";
        //        cnt = db.GetCount(sql);
        //        if (cnt > 0)
        //        {

        //            switch (db.DbType)
        //            {
        //                case ComDB.DBType.SQLServer:
        //                    sql = "";
        //                    sql = sql + "select seq.max_value ";
        //                    sql = sql + "     , seq.min_value ";
        //                    sql = sql + "     , seq.increment_value ";
        //                    sql = sql + "     , seq.start_value ";
        //                    sql = sql + "     , seq.seq_value ";
        //                    sql = sql + "  from sequence seq with(rowlock, updlock) ";
        //                    sql = sql + "  where seq.id = '" + id + "' ";
        //                    break;

        //                default:
        //                    sql = "";
        //                    sql = sql + "select seq.max_value ";
        //                    sql = sql + "     , seq.min_value ";
        //                    sql = sql + "     , seq.increment_value ";
        //                    sql = sql + "     , seq.start_value ";
        //                    sql = sql + "     , seq.seq_value ";
        //                    sql = sql + "  from sequence seq ";
        //                    sql = sql + "  where seq.id = '" + id + "' ";
        //                    sql = sql + " for update ";
        //                    break;
        //            }
        //            // SQL実行
        //            APDao.SequencesEntity results = db.GetEntity<APDao.SequencesEntity>(sql);
        //            if (results != null)
        //            {
        //                // FFF
        //                IDictionary<string, object> dicResult = results as IDictionary<string, object>;
        //                maxValue = int.Parse(dicResult["max_value"].ToString());
        //                minValue = int.Parse(dicResult["min_value"].ToString());
        //                incrementValue = int.Parse(dicResult["increment_value"].ToString());
        //                startValue = int.Parse(dicResult["start_value"].ToString());
        //                seqValue = int.Parse(dicResult["seq_value"].ToString());

        //                if (seqValue == null || minValue > seqValue)
        //                {
        //                    valueWork = startValue;
        //                }
        //                else
        //                {
        //                    valueWork = seqValue + incrementValue;
        //                }
        //                if (valueWork > maxValue)
        //                {
        //                    seqValue = minValue;
        //                }

        //                sql = "";
        //                sql = sql + "update sequence set ";
        //                sql = sql + "  seq_value = " + valueWork;
        //                sql = sql + " where id = '" + id + "' ";

        //                returnValue = (int)valueWork;
        //            }

        //        }

        //        // ★★TODO:コミット処理★★

        //        // 正常終了
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // ★★TODO:ロールバック処理★★

        //        logger.Error(ex.Message);
        //        throw ex;
        //    }
        //    finally
        //    {

        //    }
        //}

        ///// <summary>
        ///// エラーログ
        ///// </summary>
        ///// <param name="moduleId">ID</param>
        ///// <param name="client">クライアント</param>
        ///// <param name="date">日付</param>
        ///// <param name="log">ログ</param>
        ///// <param name="sqlStr">順序</param>
        ///// <param name="db">DB操作クラス</param>
        ///// <returns>true:正常　false:エラー</returns>
        //public static bool ProErrorLog(string moduleId, string client, DateTime date, string log, string sqlStr, ComDB db)
        //{
        //    int regResult;

        //    try
        //    {
        //        // トランザクション開始
        //        db.Connection.BeginTransaction();

        //        var sql = "";
        //        sql = sql + "insert into error_log (";
        //        sql = sql + " module_id, ";
        //        sql = sql + " client, ";
        //        sql = sql + " error_date, ";
        //        sql = sql + " error_mes, ";
        //        sql = sql + " sql_str ";
        //        sql = sql + " ) values ( ";
        //        sql = sql + " '" + moduleId.Substring(0, 40) + "', ";
        //        sql = sql + " '" + client.Substring(0, 20) + "', ";
        //        sql = sql + " cast(" + date + " as timestamp), ";
        //        sql = sql + " '" + log.Substring(0, 100) + "', ";
        //        sql = sql + " '" + sqlStr.Substring(0, 2000) + "' ";
        //        sql = sql + " ) ";

        //        regResult = db.Regist(sql);
        //        if (regResult < 0)
        //        {
        //            // ★★TODO:ロールバック★★
        //            // 異常終了
        //            return false;
        //        }

        //        // ★★TODO:コミット★★
        //        // 正常終了
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        // ★★TODO:ロールバック★★
        //        logger.Error(ex.Message);
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 指定した種類の連番を取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inSeqName">連番名称</param>
        /// <param name="inSeqDate">連番取得日付</param>
        /// <param name="inItemCd">品目コード</param>
        /// <param name="inSpecificationCode">仕様コード</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="inVenderDivision">取引先区分</param>
        /// <param name="inDeliveryCd">納入先コード</param>
        /// <returns>連番</returns>
        public static string ProSeqGetNo(ComDB db, string inSeqName, DateTime? inSeqDate = null, string inItemCd = null,
            string inSpecificationCode = null, string inVenderCd = null, string inVenderDivision = null, string inDeliveryCd = null)
        {
            string retVallue = ""; // 戻り値
            //int sqlNumber = 0;     // 取得連番
            decimal sqlNumber = 0;     // 取得連番
            string division = "";  // 連番取得処理制御テーブル区分
            string startTime = ""; // 開始タイミング
            string seqName = "";   // シーケンス名称
            string seqTerm = "";   // 連番期間
            string addChar1 = "";  // 固定文字列1
            string addChar2 = "";  // 固定文字列2
            string addChar3 = "";  // 固定文字列3
            string addChar4 = "";  // 固定文字列4
            string addChar5 = "";  // 固定文字列5
            //string seqDate = ""; // 連番取得日
            //int startVal = 0;      // 発番連番値の開始値
            //int maxVal = 0;        // 発番連番値の終了値
            decimal startVal = 0;      // 発番連番値の開始値
            decimal maxVal = 0;        // 発番連番値の終了値

            //int searchCount;     // 検索件数
            string remark = "";    // 備考

            string sql;       // 動的SQL

            // 引数の連番処理名称から連番取得処理制御テーブルを取得する
            sql = "";
            sql = sql + "select division, ";
            sql = sql + "       seq_name, ";
            sql = sql + "       seq_term, ";
            sql = sql + "       start_time, ";
            sql = sql + "       add_char1, ";
            sql = sql + "       add_char2, ";
            sql = sql + "       add_char3, ";
            sql = sql + "       add_char4, ";
            sql = sql + "       add_char5, ";
            sql = sql + "       start_val, ";
            sql = sql + "       max_val, ";
            sql = sql + "       remark ";
            sql = sql + "  from make_sequence_ctl ";
            sql = sql + " where process_name = @ProcName";

            EntityDao.MakeSequenceCtlEntity result = db.GetEntity<EntityDao.MakeSequenceCtlEntity>(sql, new { ProcName = inSeqName });

            if (result != null)
            {
                // FFF
                //IDictionary<string, object> dicResult = result as IDictionary<string, object>;
                //division = dicResult["division"] != null ? dicResult["division"].ToString() : "";
                //seqName = dicResult["seq_name"] != null ? dicResult["seq_name"].ToString() : "";
                //seqTerm = dicResult["seq_term"] != null ? dicResult["seq_term"].ToString() : "";
                //startTime = dicResult["start_time"] != null ? dicResult["start_time"].ToString() : "";
                //addChar1 = dicResult["add_char1"] != null ? dicResult["add_char1"].ToString() : "";
                //addChar2 = dicResult["add_char2"] != null ? dicResult["add_char2"].ToString() : "";
                //addChar3 = dicResult["add_char3"] != null ? dicResult["add_char3"].ToString() : "";
                //addChar4 = dicResult["add_char4"] != null ? dicResult["add_char4"].ToString() : "";
                //addChar5 = dicResult["add_char5"] != null ? dicResult["add_char5"].ToString() : "";
                //startVal = dicResult["start_val"] != null ? int.Parse(dicResult["start_val"].ToString()) : 0;
                //maxVal = dicResult["max_val"] != null ? int.Parse(dicResult["max_val"].ToString()) : 0;
                //startVal = dicResult["start_val"] != null ? decimal.Parse(dicResult["start_val"].ToString()) : 0;
                //maxVal = dicResult["max_val"] != null ? decimal.Parse(dicResult["max_val"].ToString()) : 0;
                //remark = dicResult["remark"] != null ? dicResult["remark"].ToString() : "";

                division = result.Division != null ? result.Division : "";
                seqName = result.SeqName != null ? result.SeqName : "";
                seqTerm = result.SeqTerm != null ? result.SeqTerm : "";
                startTime = result.StartTime != null ? result.StartTime : "";
                addChar1 = result.AddChar1 != null ? result.AddChar1 : "";
                addChar2 = result.AddChar2 != null ? result.AddChar2 : "";
                addChar3 = result.AddChar3 != null ? result.AddChar3 : "";
                addChar4 = result.AddChar4 != null ? result.AddChar4 : "";
                addChar5 = result.AddChar5 != null ? result.AddChar5 : "";
                startVal = result.StartVal != null ? decimal.Parse(result.StartVal.ToString()) : 0;
                maxVal = result.MaxVal != null ? decimal.Parse(result.MaxVal.ToString()) : 0;
                remark = result.Remark != null ? result.Remark.ToString() : "";
            }

            try
            {
                // 連番区分によって処理を分岐する
                switch (division)
                {
                    case "1":
                        // 連番区分がシーケンスである場合、設定されている連番でシーケンス番号を取得
                        sql = ""; // 念のため、初期化
                        sql = "select nextval('" + seqName + "');";

                        //result = db.GetEntity<EntityDao.MakeSequenceCtlEntity>(sql);
                        string nextVal = db.GetEntity<string>(sql);
                        if (!string.IsNullOrEmpty(nextVal))
                        {
                            //var dic = result as IDictionary<string, object>;
                            //sqlNumber = int.Parse(dic["nextval"].ToString());
                            //sqlNumber = decimal.Parse(dic["nextval"].ToString());
                            sqlNumber = decimal.Parse(nextVal);
                            if (maxVal.ToString().Equals(retVallue))
                            {
                                // シーケンスから取得した番号が最大値の場合、開始値から取得するように修正
                                sql = ""; // 念のため、初期化
                                sql = "alter sequence " + seqName + " restart with " + (startVal + 1) + ";";
                                db.Regist(sql);
                            }
                        }
                        break;
                    case "2":
                        // 連番区分が日付の場合
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < 5; i++)
                {
                    var addChar = "";
                    if (i == 0)
                    {
                        addChar = addChar1;
                    }
                    else if (i == 1)
                    {
                        addChar = addChar2;
                    }
                    else if (i == 2)
                    {
                        addChar = addChar3;
                    }
                    else if (i == 3)
                    {
                        addChar = addChar4;
                    }
                    else
                    {
                        addChar = addChar5;
                    }
                    retVallue = retVallue + fncUtil.FncGetAddCharProc(addChar, sqlNumber, maxVal, inSeqDate);
                }
            }
            catch
            {
                // 特に何もしない
            }

            return retVallue;
        }

    }
}
