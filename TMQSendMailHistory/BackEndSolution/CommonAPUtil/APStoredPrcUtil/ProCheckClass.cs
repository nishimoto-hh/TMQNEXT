using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>
    /// トランザクション使用チェッククラス
    /// </summary>
    public class ProCheckClass
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProCheckClass()
        {
        }
        #endregion
        /*
        #region トランザクション使用判定メソッド
        /// <summary>
        /// トランザクション使用判定（地区）
        /// </summary>
        /// <param name="areaCd">地区コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckAreaCd(string areaCd,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "        select count(area_cd) cnt ";
                sql = sql + "          from location ";
                sql = sql + "         where area_cd = '" + areaCd + "'";
                sql = sql + "        union all ";
                sql = sql + "        select count(from_area_cd) cnt ";
                sql = sql + "          from transfer ";
                sql = sql + "         where from_area_cd = '" + areaCd + "'";
                sql = sql + "        union all ";
                sql = sql + "        select count(to_area_cd) cnt ";
                sql = sql + "          from transfer ";
                sql = sql + "         where to_area_cd = '" + areaCd + "'";
                sql = sql + "        union all ";
                sql = sql + "        select count(area_cd) cnt ";
                sql = sql + "          from vender_queue ";
                sql = sql + "         where area_cd = '" + areaCd + "'";
                sql = sql + "       ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定(帳合)
        /// </summary>
        /// <param name="balanceCd">帳合コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckBalanceCd(string balanceCd,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(balance_cd) cnt";
                sql = sql + "       from order_head ";
                sql = sql + "       where balance_cd = '" + balanceCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(balance_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where balance_cd = '" + balanceCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(upper_balance_cd) cnt";
                sql = sql + "       from balance";
                sql = sql + "       where upper_balance_cd = '" + balanceCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(balance_cd) cnt";
                sql = sql + "       from unitprice";
                sql = sql + "       where vender_division = 'TS'";
                sql = sql + "       and balance_cd = '" + balanceCd + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（銀行）
        /// </summary>
        /// <param name="bankMasterCd">銀行コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckBankMasterCd(string bankMasterCd,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(bank_master_cd1) cnt";
                sql = sql + "       from company";
                sql = sql + "       where bank_master_cd1 = '" + bankMasterCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(bank_master_cd2) cnt";
                sql = sql + "       from company";
                sql = sql + "       where bank_master_cd2 = '" + bankMasterCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(bank_master_cd3) cnt";
                sql = sql + "       from company";
                sql = sql + "       where bank_master_cd3 = '" + bankMasterCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(bank_master_cd4) cnt";
                sql = sql + "       from company";
                sql = sql + "       where bank_master_cd4 = '" + bankMasterCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(bank_master_cd) cnt";
                sql = sql + "       from company";
                sql = sql + "       where bank_master_cd = '" + bankMasterCd + "'";
                sql = sql + "   ) tbl ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（BOM）
        /// </summary>
        /// <param name="bomCd">BOMコード</param>
        /// <param name="version">バージョン</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckBomCd(string bomCd, decimal version,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(recipe_cd) cnt";
                sql = sql + "       from direction_header";
                sql = sql + "       where recipe_cd = '" + bomCd + "'";
                sql = sql + "       and recipe_version = " + version;
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（通貨）
        /// </summary>
        /// <param name="currencyCd">通貨コード</param>
        /// <param name="subCode">日付</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckCurrencyCd(string currencyCd, string subCode,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                // 請求ヘッダー
                sql = sql + "       select count(currency_code) cnt";
                sql = sql + "       from claim_header";
                sql = sql + "       where currency_code = '" + currencyCd + "'";
                sql = sql + "       and " + DateTime.Parse(subCode) + " <= ex_valid_date";
                sql = sql + "       union all";
                // 入金トランザクション
                sql = sql + "       select count(currency_code) cnt";
                sql = sql + "       from credit";
                sql = sql + "       where currency_code = '" + currencyCd + "'";
                sql = sql + "       and " + DateTime.Parse(subCode) + " <= ex_valid_date";
                sql = sql + "       union all";
                // 売掛ヘッダー
                sql = sql + "       select count(currency_code) cnt";
                sql = sql + "       from deposit_header";
                sql = sql + "       where currency_code = '" + currencyCd + "'";
                sql = sql + "       and " + DateTime.Parse(subCode) + " <= ex_valid_date";
                sql = sql + "       union all";
                // 消込トランザクション
                sql = sql + "       select count(currency_code) cnt";
                sql = sql + "       from eraser_csm";
                sql = sql + "       where currency_code = '" + currencyCd + "'";
                sql = sql + "       and " + DateTime.Parse(subCode) + " <= ex_valid_date";
                sql = sql + "       union all";
                // 受注明細
                sql = sql + "       select count(currency_code) cnt";
                sql = sql + "       from order_detail";
                sql = sql + "       where currency_code = '" + currencyCd + "'";
                sql = sql + "       and " + DateTime.Parse(subCode) + " <= ex_valid_date";
                sql = sql + "       union all";
                // 購買
                sql = sql + "       select count(currency_code) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where currency_code = '" + currencyCd + "'";
                sql = sql + "       and " + DateTime.Parse(subCode) + " <= ex_valid_date";
                sql = sql + "       union all";
                // 売上トランザクション
                sql = sql + "       select count(currency_code) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where currency_code = '" + currencyCd + "'";
                sql = sql + "       and " + DateTime.Parse(subCode) + " <= ex_valid_date";
                sql = sql + "       union all";
                // 取引先
                sql = sql + "       select count(currency_code) cnt";
                sql = sql + "       from vender_queue";
                sql = sql + "       where currency_code = '" + currencyCd + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（納入先マスタ）
        /// </summary>
        /// <param name="deliveryCd">納入先コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckDeliveryCd(string deliveryCd,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(delivery_cd) cnt";
                sql = sql + "       from order_head";
                sql = sql + "       where delivery_cd = '" + deliveryCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(delivery_cd) cnt";
                sql = sql + "       from remark";
                sql = sql + "       where delivery_cd = '" + deliveryCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(delivery_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where delivery_cd = '" + deliveryCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(delivery_cd) cnt";
                sql = sql + "       from shipping";
                sql = sql + "       where delivery_cd = '" + deliveryCd + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（品目分類）
        /// </summary>
        /// <param name="code">品目分類コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckItemCategory(string code,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(item_category) cnt";
                sql = sql + "       from item_specification_queue";
                sql = sql + "       where item_category = '" + code + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（品目）
        /// </summary>
        /// <param name="itemCd">品目コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckItemCd(string itemCd, ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from direction_formula";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from direction_header";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from inventory_count";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from inventory_inout_direction";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from lot_inventory";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from monthly_inout_record";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from order_detail";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from recipe_formula";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from recipe_header";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from remark";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from shipping";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from transfer";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(item_cd) cnt";
                sql = sql + "       from unitprice";
                sql = sql + "       where item_cd = '" + itemCd + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（仕様）
        /// </summary>
        /// <param name="itemSubCd">品目コード</param>
        /// <param name="subCode">仕様コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckItemSubCd(string itemSubCd, string subCode,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from direction_formula ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from direction_header ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from inventory_count ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from lot_inventory ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from monthly_inout_record ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from order_detail ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from purchase_subcontract ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from recipe_formula ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from recipe_header ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from remark ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from sales ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from shipping ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from transfer ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(specification_code) cnt";
                sql = sql + "       from unitprice ";
                sql = sql + "       where item_cd = '" + itemSubCd + "'";
                sql = sql + "       and specification_code = '" + subCode + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（ロケーション）
        /// </summary>
        /// <param name="locationCd">ロケーションコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckLocationCd(string locationCd,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(location_cd) cnt";
                sql = sql + "       from direction_formula";
                sql = sql + "       where location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(location_cd) cnt";
                sql = sql + "       from inventory_count";
                sql = sql + "       where location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(location_cd) cnt";
                sql = sql + "       from inventory_inout_direction";
                sql = sql + "       where location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(upper_location_cd) cnt";
                sql = sql + "       from location";
                sql = sql + "       where upper_location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(location_cd) cnt";
                sql = sql + "       from lot_inventory";
                sql = sql + "       where location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(location_cd) cnt";
                sql = sql + "       from monthly_inout_record";
                sql = sql + "       where location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(location_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(housing_location_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where housing_location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(location_cd) cnt";
                sql = sql + "       from sales_inout_record";
                sql = sql + "       where location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(housing_location_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where housing_location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(location_cd) cnt";
                sql = sql + "       from shipping_detail";
                sql = sql + "       where location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(from_location_cd) cnt";
                sql = sql + "       from transfer";
                sql = sql + "       where from_location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(to_location_cd) cnt";
                sql = sql + "       from transfer";
                sql = sql + "       where to_location_cd = '" + locationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(default_location) cnt";
                sql = sql + "       from item_specification";
                sql = sql + "       where default_location = '" + locationCd + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（名称）
        /// </summary>
        /// <param name="nameCd">名称コード</param>
        /// <param name="subCd">区分コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckNameCd(string nameCd, string subCd,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";

                // 名称コードで作成するSQLを判定
                switch (nameCd)
                {
                    case "CICD":
                        sql = sql + "       select count(comparison) cnt";
                        sql = sql + "       from direction_inspection_value ";
                        sql = sql + "       where comparison = '" + subCd + "'";
                        sql = sql + "       union all";
                        sql = sql + "       select count(comparison) cnt";
                        sql = sql + "       from recipe_inspection_value ";
                        sql = sql + "       where comparison = '" + subCd + "'";
                        sql = sql + "   ) tbl ";
                        break;
                    case "CIDV":
                        sql = sql + "       select count(division) cnt";
                        sql = sql + "       from direction_inspection_value ";
                        sql = sql + "       where division = '" + subCd + "'";
                        sql = sql + "       union all";
                        sql = sql + "       select count(division) cnt";
                        sql = sql + "       from recipe_inspection_value ";
                        sql = sql + "       where division = '" + subCd + "'";
                        sql = sql + "   ) tbl ";
                        break;
                    case "CTND":
                        sql = sql + "       select count(count_division) cnt";
                        sql = sql + "       from inventory_count ";
                        sql = sql + "       where count_division = '" + subCd + "'";
                        sql = sql + "       union all";
                        sql = sql + "       select count(count_division) cnt";
                        sql = sql + "       from location ";
                        sql = sql + "       where count_division = '" + subCd + "'";
                        sql = sql + "   ) tbl ";
                        break;
                    case "INSP":
                        sql = sql + "       select count(inspection_cd) cnt";
                        sql = sql + "       from direction_inspection ";
                        sql = sql + "       where inspection_cd = '" + subCd + "'";
                        sql = sql + "       union all";
                        sql = sql + "       select count(inspection_cd) cnt";
                        sql = sql + "       from recipe_inspection ";
                        sql = sql + "       where inspection_cd = '" + subCd + "'";
                        sql = sql + "   ) tbl ";
                        break;
                    case "UNIT":
                        sql = sql + "       select count(unit_of_stock_control) cnt";
                        sql = sql + "       from item_queue ";
                        sql = sql + "       where unit_of_stock_control = '" + subCd + "'";
                        sql = sql + "       union all";
                        sql = sql + "       select count(unit_of_operation_management) cnt";
                        sql = sql + "       from item_queue ";
                        sql = sql + "       where unit_of_operation_management = '" + subCd + "'";
                        sql = sql + "       union all";
                        sql = sql + "       select count(unit_of_stock_control) cnt";
                        sql = sql + "       from item_specification_queue ";
                        sql = sql + "       where unit_of_stock_control = '" + subCd + "'";
                        sql = sql + "       union all";
                        sql = sql + "       select count(unit_of_operation_management) cnt";
                        sql = sql + "       from item_specification_queue ";
                        sql = sql + "       where unit_of_operation_management = '" + subCd + "'";
                        sql = sql + "       union all";
                        sql = sql + "       select count(filling_unit) cnt";
                        sql = sql + "       from recipe_formula ";
                        sql = sql + "       where filling_unit = '" + subCd + "'";
                        sql = sql + "   ) tbl ";
                        break;
                    default:
                        sql = "";
                        break;
                }

                if (sql != "")
                {
                    // SQL実行
                    dataCount = db.GetCount(sql);
                }

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（工程）
        /// </summary>
        /// <param name="operationCd">工程コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckOperationCd(string operationCd,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(operation_cd) cnt";
                sql = sql + "       from recipe_procedure";
                sql = sql + "       where operation_cd = '" + operationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(operation_cd) cnt";
                sql = sql + "       from direction_procedure";
                sql = sql + "       where operation_cd = '" + operationCd + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（工程グループ）
        /// </summary>
        /// <param name="operationGroupCd">工程グループコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckOperationGroupCd(string operationGroupCd,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(operation_group_cd) cnt";
                sql = sql + "       from recipe_resouce_group";
                sql = sql + "       where operation_group_cd = '" + operationGroupCd + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（部署）
        /// </summary>
        /// <param name="organizationCd">部署コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckOrganizationCd(string organizationCd,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(organization_cd) cnt";
                sql = sql + "       from belong_role";
                sql = sql + "       where organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(organization_cd) cnt";
                sql = sql + "       from belong";
                sql = sql + "       where organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(organization_cd) cnt";
                sql = sql + "       from claim_header";
                sql = sql + "       where organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(organization_cd) cnt";
                sql = sql + "       from credit";
                sql = sql + "       where organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(organization_cd) cnt";
                sql = sql + "       from deposit_header";
                sql = sql + "       where organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(organization_cd) cnt";
                sql = sql + "       from eraser_csm";
                sql = sql + "       where organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(organization_cd) cnt";
                sql = sql + "       from order_head";
                sql = sql + "       where organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(charge_organization_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where charge_organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(organization_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(organization_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(charge_organization_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where charge_organization_cd = '" + organizationCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(organization_cd) cnt";
                sql = sql + "       from vender_queue";
                sql = sql + "       where organization_cd = '" + organizationCd + "'";
                sql = sql + "   ) tbl ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（役職）
        /// </summary>
        /// <param name="postId">役職コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckPostId(string postId,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(post_id) cnt";
                sql = sql + "       from belong_role ";
                sql = sql + "       where post_id = " + int.Parse(postId);
                sql = sql + "       union all";
                sql = sql + "       select count(post_id) cnt";
                sql = sql + "       from belong";
                sql = sql + "       where post_id = " + int.Parse(postId);
                sql = sql + "   ) tbl ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（生産ライン）
        /// </summary>
        /// <param name="code">生産ラインコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckProductionLine(string code, ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(production_line) cnt";
                sql = sql + "       from direction_header ";
                sql = sql + "       where production_line = '" + code + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(production_line) cnt";
                sql = sql + "       from recipe_header";
                sql = sql + "       where production_line = '" + code + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(production_line) cnt";
                sql = sql + "       from recipe_resouce";
                sql = sql + "       where production_line = '" + code + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（設備）
        /// </summary>
        /// <param name="resouceCd">設備コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckResouceCd(string resouceCd, ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(resouce_cd) cnt";
                sql = sql + "       from recipe_procedure ";
                sql = sql + "       where resouce_cd = '" + resouceCd + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（設備グループ）
        /// </summary>
        /// <param name="resouceGroupCd">設備グループコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckResouceGroupCd(string resouceGroupCd, ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(resouce_group_cd) cnt";
                sql = sql + "       from recipe_resouce ";
                sql = sql + "       where resouce_group_cd = '" + resouceGroupCd + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（ロール）
        /// </summary>
        /// <param name="roleId">ロールコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckRoleId(string roleId, ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(role_id) cnt";
                sql = sql + "       from belong_role ";
                sql = sql + "       where role_id = '" + roleId + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(role_id) cnt";
                sql = sql + "       from tanto_role";
                sql = sql + "       where role_id = '" + roleId + "'";
                sql = sql + "   ) tbl ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（理由）
        /// </summary>
        /// <param name="ryCd">理由コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckRyCd(string ryCd, ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(reason_cd) cnt";
                sql = sql + "       from inout_record ";
                sql = sql + "       where reason_cd = '" + ryCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(reason_cd) cnt";
                sql = sql + "       from inventory_inout_direction";
                sql = sql + "       where reason_cd = '" + ryCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(ry_cd) cnt";
                sql = sql + "       from transfer";
                sql = sql + "       where ry_cd = '" + ryCd + "'";
                sql = sql + "   ) tbl_a ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// トランザクション使用判定（担当者）
        /// </summary>
        /// <param name="tantoCd">担当者コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckTantoCd(string tantoCd, ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";
                sql = sql + "       select count(tanto_cd) cnt";
                sql = sql + "       from belong ";
                sql = sql + "       where tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(receive_tanto_cd) cnt";
                sql = sql + "       from credit";
                sql = sql + "       where receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_request_tanto_cd) cnt";
                sql = sql + "       from credit";
                sql = sql + "       where approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(tanto_cd) cnt";
                sql = sql + "       from delivery";
                sql = sql + "       where tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(stamp_tanto_cd) cnt";
                sql = sql + "       from direction_header";
                sql = sql + "       where stamp_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(last_app_tanto_cd) cnt";
                sql = sql + "       from direction_header";
                sql = sql + "       where last_app_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_request_tanto_cd) cnt";
                sql = sql + "       from inventory_inout_direction";
                sql = sql + "       where approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_tanto_cd) cnt";
                sql = sql + "       from inventory_inout_direction";
                sql = sql + "       where approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(receive_tanto_cd) cnt";
                sql = sql + "       from item_queue";
                sql = sql + "       where receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_request_tanto_cd) cnt";
                sql = sql + "       from item_queue";
                sql = sql + "       where approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_tanto_cd) cnt";
                sql = sql + "       from item_queue";
                sql = sql + "       where approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(receive_tanto_cd) cnt";
                sql = sql + "       from item_specification_queue";
                sql = sql + "       where receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_request_tanto_cd) cnt";
                sql = sql + "       from item_specification_queue";
                sql = sql + "       where approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_tanto_cd) cnt";
                sql = sql + "       from item_specification_queue";
                sql = sql + "       where approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(input_tanto_cd) cnt";
                sql = sql + "       from order_head";
                sql = sql + "       where input_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(sales_tanto_cd) cnt";
                sql = sql + "       from order_head";
                sql = sql + "       where sales_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_request_tanto_cd) cnt";
                sql = sql + "       from order_head";
                sql = sql + "       where approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_tanto_cd) cnt";
                sql = sql + "       from order_head";
                sql = sql + "       where approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(receive_tanto_cd) cnt";
                sql = sql + "       from order_head";
                sql = sql + "       where receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(order_sheel_tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where order_sheel_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(purchase_approval_request_tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where purchase_approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(purchase_approval_tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where purchase_approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(ordering_approval_request_tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where ordering_approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(ordering_approval_tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where ordering_approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(buying_approval_request_tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where buying_approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(buying_approval_tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where buying_approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(purchase_receive_tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where purchase_receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(ordering_receive_tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where ordering_receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(buying_receive_tanto_cd) cnt";
                sql = sql + "       from purchase_subcontract";
                sql = sql + "       where buying_receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(receive_tanto_cd) cnt";
                sql = sql + "       from recipe_header";
                sql = sql + "       where receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_request_tanto_cd) cnt";
                sql = sql + "       from recipe_header";
                sql = sql + "       where approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_tanto_cd) cnt";
                sql = sql + "       from recipe_header";
                sql = sql + "       where approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(input_tanto_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where input_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(sales_tanto_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where sales_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(receive_tanto_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_request_tanto_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_tanto_cd) cnt";
                sql = sql + "       from sales";
                sql = sql + "       where approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_request_tanto_cd) cnt";
                sql = sql + "       from unitprice";
                sql = sql + "       where approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_tanto_cd) cnt";
                sql = sql + "       from unitprice";
                sql = sql + "       where approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(receive_tanto_cd) cnt";
                sql = sql + "       from unitprice";
                sql = sql + "       where receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(tanto_cd) cnt";
                sql = sql + "       from vender_queue";
                sql = sql + "       where tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_request_tanto_cd) cnt";
                sql = sql + "       from vender_queue";
                sql = sql + "       where approval_request_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(receive_tanto_cd) cnt";
                sql = sql + "       from vender_queue";
                sql = sql + "       where receive_tanto_cd = '" + tantoCd + "'";
                sql = sql + "       union all";
                sql = sql + "       select count(approval_tanto_cd) cnt";
                sql = sql + "       from vender_queue";
                sql = sql + "       where approval_tanto_cd = '" + tantoCd + "'";
                sql = sql + "   ) tbl ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// 取引先使用判定
        /// </summary>
        /// <param name="venderCd">取引先区分</param>
        /// <param name="subCd">取引先コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static bool ProCheckVenderCd(string venderCd, string subCd,
            ComDB db)
        {
            try
            {
                // 件数を初期化
                int dataCount = 0;

                // SQL文生成
                var sql = "";
                sql = sql + "select sum(cnt) ";
                sql = sql + "  from ( ";

                if (venderCd == "TS")
                {
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from vender_queue ";
                    sql = sql + "       where vender_division = '" + venderCd + "'";
                    sql = sql + "       and payment_invoice_cd = '" + subCd + "'";
                    sql = sql + "       and vender_cd <> payment_invoice_cd";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from balance ";
                    sql = sql + "       where vender_division = '" + venderCd + "'";
                    sql = sql + "       and vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from claim_header ";
                    sql = sql + "       where vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from credit ";
                    sql = sql + "       where vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from deposit_header ";
                    sql = sql + "       where vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from order_head ";
                    sql = sql + "       where vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from product_attribute_queue ";
                    sql = sql + "       where vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from remark ";
                    sql = sql + "       where vender_division = '" + venderCd + "'";
                    sql = sql + "       and vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from sales ";
                    sql = sql + "       where vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from shipping ";
                    sql = sql + "       where vender_division = '" + venderCd + "'";
                    sql = sql + "       and vender_cd = '" + subCd + "'";
                }
                else
                {
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from vender_queue ";
                    sql = sql + "       where vender_division = '" + venderCd + "'";
                    sql = sql + "       and payment_invoice_cd = '" + subCd + "'";
                    sql = sql + "       and vender_cd <> payment_invoice_cd";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from direction_header ";
                    sql = sql + "       where vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from purchase_attribute_queue ";
                    sql = sql + "       where vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from purchase_subcontract ";
                    sql = sql + "       where vender_cd = '" + subCd + "'";
                    sql = sql + "       union all";
                    sql = sql + "       select count(*) cnt";
                    sql = sql + "       from remark ";
                    sql = sql + "       where vender_division = '" + venderCd + "'";
                    sql = sql + "       and vender_cd = '" + subCd + "'";
                }
                sql = sql + "   ) vender ";

                // SQL実行
                dataCount = db.GetCount(sql);

                // データの件数が１件以上あればtrueを返す
                if (dataCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// 取引先データ判定
        /// </summary>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="venderDiv">取引先区分</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns></returns>
        public static int ProCheckVenderTax(string venderCd, string venderDiv,
            ComDB db)
        {
            try
            {
                int? tempTaxDivision = null;          // 消費税課税区分
                int? tempTaxCalcDivision = null;      // 消費税算出区分
                //int? tempTaxRatio = null;             // 消費税率
                int? tempTaxRoundup = null;           // 消費税端数区分
                int? tempTaxRoundupUnit = null;       // 消費税端数単位
                string tempInvoiceCd = "";                // 請求先コード
                int dataCount = 0;                // 検索結果有無用カウント変数


                // 取引先有無チェックSQL文生成
                var sql = "";
                sql = sql + "   select count(*)";
                sql = sql + "   from vender ";
                sql = sql + "   where vender.vender_cd = '" + venderCd + "'";
                sql = sql + "   and vender.vender_division = '" + venderDiv + "'";

                // 取引先有無チェックSQL実行
                dataCount = db.GetCount(sql);

                if (dataCount == 0)
                {
                    return 1;
                }

                // 件数を初期化
                dataCount = 0;

                // 請求先有無チェックSQL文生成
                sql = "";
                sql = sql + "   select count(*)";
                sql = sql + "   from vender ";
                sql = sql + "   where vender.vender_cd in (";
                sql = sql + "      select vender.payment_invoice_cd";
                sql = sql + "      from vender";
                sql = sql + "      where vender.vender_cd = '" + venderCd + "'";
                sql = sql + "      and vender.vender_division = '" + venderDiv + "')";

                // 請求先有無チェックSQL実行
                dataCount = db.GetCount(sql);

                if (dataCount == 0)
                {
                    return 2;
                }

                // 請求先コード取得SQL文生成
                sql = "";
                sql = sql + "   select vender.payment_invoice_cd";
                sql = sql + "   from vender ";
                sql = sql + "   where vender.vender_cd = '" + venderCd + "'";
                sql = sql + "   and vender.vender_division = '" + venderDiv + "'";

                // 請求先コード取得SQL実行
                //dynamic results = db.GetEntity(sql);
                //IDictionary<string, object> dicResult = results as IDictionary<string, object>;
                //if (dicResult != null)
                //{
                //    tempInvoiceCd = dicResult["payment_invoice_cd"].ToString();
                //}
                tempInvoiceCd = db.GetEntityByDataClass<string>(sql);

                // 請求先の消費税計算情報の有無チェックSQL文生成
                sql = "";
                sql = sql + "   select ";
                // 消費税区分
                sql = sql + "   case when vender.tax_division = 4 then (";
                sql = sql + "       select vender.tax_division";
                sql = sql + "       from vender";
                sql = sql + "       where vender.vender_cd = '" + venderCd + "'";
                sql = sql + "       and vender.vender_division = '" + venderDiv + "')";
                sql = sql + "   else vender.tax_division";
                sql = sql + "   end,";
                // 消費税算出区分
                sql = sql + "   case when vender.calc_division = 4 then (";
                sql = sql + "       select vender.calc_division";
                sql = sql + "       from vender";
                sql = sql + "       where vender.vender_cd = '" + venderCd + "'";
                sql = sql + "       and vender.vender_division = '" + venderDiv + "')";
                sql = sql + "   else vender.calc_division";
                sql = sql + "   end,";
                // 消費税率
                //sql = sql + "   case when vender.tax_division = 4 then (";
                //sql = sql + "       select vender.tax_ratio";
                //sql = sql + "       from vender";
                //sql = sql + "       where vender.vender_cd = '" + venderCd + "'";
                //sql = sql + "       and vender.vender_division = '" + venderDiv + "')";
                //sql = sql + "   else vender.tax_ratio";
                //sql = sql + "   end,";
                // 消費税率端数区分
                sql = sql + "   case when vender.tax_roundup = 4 then (";
                sql = sql + "       select vender.tax_roundup";
                sql = sql + "       from vender";
                sql = sql + "       where vender.vender_cd = '" + venderCd + "'";
                sql = sql + "       and vender.vender_division = '" + venderDiv + "')";
                sql = sql + "   else vender.tax_roundup";
                sql = sql + "   end,";
                // 消費税端数単位
                sql = sql + "   case when vender.tax_roundup_unit = 8 then (";
                sql = sql + "       select vender.tax_roundup_unit";
                sql = sql + "       from vender";
                sql = sql + "       where vender.vender_cd = '" + venderCd + "'";
                sql = sql + "       and vender.vender_division = '" + venderDiv + "')";
                sql = sql + "   else vender.tax_roundup_unit";
                sql = sql + "   end";
                sql = sql + "   from (";
                sql = sql + "           select";
                sql = sql + "           vender.tax_division,";
                sql = sql + "           vender.calc_division,";
                //sql = sql + "           vender.tax_ratio,";
                sql = sql + "           vender.tax_roundup,";
                sql = sql + "           vender.tax_roundup_unit";
                sql = sql + "           from vender";
                sql = sql + "           where vender.vender_division = '" + venderDiv + "'";
                sql = sql + "           and vender.vender_cd = '" + tempInvoiceCd + "'";
                sql = sql + "         ) as vender";

                // 請求先の消費税計算情報の有無チェックSQL実行
                //results = db.GetEntity(sql);
                //dicResult = results as IDictionary<string, object>;

                //if (dicResult != null)
                //{
                //    tempTaxDivision = decimal.Parse(dicResult["tax_division"].ToString());
                //    tempTaxCalcDivision = decimal.Parse(dicResult["calc_division"].ToString());
                //    tempTaxRatio = decimal.Parse(dicResult["tax_ratio"].ToString());
                //    tempTaxRoundup = decimal.Parse(dicResult["tax_roundup"].ToString());
                //    tempTaxRoundupUnit = decimal.Parse(dicResult["tax_round_unit"].ToString());
                //}
                ComDao.VenderEntity results = new ComDao.VenderEntity();
                results = db.GetEntity<ComDao.VenderEntity>(sql);
                if (results != null)
                {
                    tempTaxDivision = results.TaxDivision;
                    tempTaxCalcDivision = results.CalcDivision;
                    //tempTaxRatio = results.TaxRatio ;
                    tempTaxRoundup = results.TaxRoundup;
                    tempTaxRoundupUnit = results.TaxRoundupUnit;
                }

                // 各変数null判定
                if (tempTaxDivision == null ||
                    tempTaxCalcDivision == null ||
                    //tempTaxRatio == null ||
                    tempTaxRoundup == null ||
                    tempTaxRoundupUnit == null
                    )
                {
                    return 3;
                }

                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return 1;
            }
        }
        #endregion
        */
    }
}
