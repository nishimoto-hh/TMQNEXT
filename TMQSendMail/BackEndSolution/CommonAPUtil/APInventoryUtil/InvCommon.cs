using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonSTDUtil.CommonLogger;

using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComID = CommonAPUtil.APCommonUtil.APResources.ID;
using ComST = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using InvConst = CommonAPUtil.APInventoryUtil.InventoryConst;
using InvDao = CommonAPUtil.APInventoryUtil.InventoryDataClass;

namespace CommonAPUtil.APInventoryUtil
{
    /// <summary>
    /// 在庫更新：共通クラス
    /// </summary>
    public class InvCommon
    {
        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// ログ出力(パラメータ情報)
        /// </summary>
        /// <param name="paramInfo">出力対象行</param>
        /// <param name="paramIdx">インデックス番号</param>
        public static void LogOutparamInfo(InvDao.InventoryParameter paramInfo, int paramIdx)
        {
            //ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(InvConst.LOG_NAME);

            string logVal = string.Empty;
            logVal += "【parameter】";
            logVal += "LineNo:" + (paramIdx + 1).ToString() + " | ";
            logVal += "ProcessDivision:" + paramInfo.ProcessDivision.ToString() + ",";
            logVal += "InoutDivision:" + paramInfo.InoutDivision + ",";
            logVal += "AccountYears:" + paramInfo.AccountYears + ",";
            logVal += "InoutDate:" + paramInfo.InoutDate?.ToString("yyyy/MM/dd") + ",";
            logVal += "OrderDivision:" + paramInfo.OrderDivision.ToString() + ",";
            logVal += "OrderNo:" + paramInfo.OrderNo + ",";
            logVal += "OrderLineNo1:" + paramInfo.OrderLineNo1.ToString() + ",";
            logVal += "OrderLineNo2:" + paramInfo.OrderLineNo2.ToString() + ",";
            logVal += "ResultOrderDivision:" + paramInfo.ResultOrderDivision.ToString() + ",";
            logVal += "ResultOrderNo:" + paramInfo.ResultOrderNo + ",";
            logVal += "ResultOrderLineNo1:" + paramInfo.ResultOrderLineNo1.ToString() + ",";
            logVal += "ResultOrderLineNo2:" + paramInfo.ResultOrderLineNo2.ToString() + ",";
            logVal += "ItemCd:" + paramInfo.ItemCd + ",";
            logVal += "SpecificationCd:" + paramInfo.SpecificationCd + ",";
            logVal += "LocationCd:" + paramInfo.LocationCd + ",";
            logVal += "LotNo:" + paramInfo.LotNo + ",";
            logVal += "SubLotNo1:" + paramInfo.SubLotNo1 + ",";
            logVal += "SubLotNo2:" + paramInfo.SubLotNo2 + ",";
            logVal += "InoutQty:" + paramInfo.InoutQty.ToString() + ",";
            logVal += "ReferenceNo:" + paramInfo.ReferenceNo + ",";
            logVal += "ReferenceLineNo:" + paramInfo.ReferenceLineNo.ToString() + ",";
            logVal += "AssignFlg:" + paramInfo.AssignFlg.ToString() + ",";
            logVal += "OverFlg:" + paramInfo.OverFlg.ToString() + ",";
            logVal += "InputMenuId:" + paramInfo.InputMenuId + ",";
            logVal += "InputDisplayId:" + paramInfo.InputDisplayId.ToString() + ",";
            logVal += "InputControlId:" + paramInfo.InputControlId.ToString() + ",";
            logVal += "CompleteFlg:" + paramInfo.CompleteFlg.ToString() + ",";
            logVal += "Remark:" + paramInfo.Remark + ",";
            logVal += "RyCd:" + paramInfo.RyCd + ",";
            logVal += "Reason:" + paramInfo.Reason + ",";
            logVal += "BeforeLocationCd:" + paramInfo.BeforeLocationCd + ",";
            logVal += "TransferItemCd:" + paramInfo.TransferItemCd + ",";
            logVal += "TransferSpecificationCd:" + paramInfo.TransferSpecificationCd + ",";
            logVal += "TransferLotNo:" + paramInfo.TransferLotNo + ",";
            logVal += "TransferSubLotNo1:" + paramInfo.TransferSubLotNo1 + ",";
            logVal += "TransferSubLotNo2:" + paramInfo.TransferSubLotNo2 + ",";
            logVal += "UserId:" + paramInfo.UserId + ",";
            logVal += "InoutSourceNo:" + paramInfo.InoutSourceNo + ",";
            logVal += "StockDivision:" + paramInfo.StockDivision + ",";

            logger.Info(logVal);
        }

        /// <summary>
        /// 在庫管理区分チェック
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramItemCd">検索対象：品目コード</param>
        /// <param name="paramSpecificationCd">検索対象：仕様コード</param>
        /// <param name="paramLocationCd">検索対象：ロケーッションコード</param>
        /// <param name="paramIdx">パラメータリスト位置</param>
        /// <param name="paramLanguageId">言語ID</param>
        /// <param name="paramStockDivision">取得：在庫管理区分</param>
        /// <returns>0:更新対象　3：更新対象外　9：エラー</returns>
        public static int CheckStockDivision(ComDB db, string paramItemCd,
            string paramSpecificationCd, string paramLocationCd, int paramIdx, string paramLanguageId, ref int? paramStockDivision)
        {

            bool retVal = true;
            string message = string.Empty;

            try
            {

                //在庫管理区分判定
                int? stockDivision = null;
                retVal = getStockDivision(db, paramItemCd, paramSpecificationCd, ref stockDivision);
                if (!retVal)
                {
                    //@1行目の在庫管理区分の取得に失敗しました。
                    message = ComST.GetPropertiesMessage(new string[] { ComID.MB10123, (paramIdx + 1).ToString(), ComID.MB10124 }, paramLanguageId);
                    logger.Error(message);
                    return InvConst.STOCK_DIVISION_RESULT.ERROR;
                }

                paramStockDivision = stockDivision;
                if (stockDivision == 3)
                {
                    //在庫更新対象外
                    return InvConst.STOCK_DIVISION_RESULT.NOT_APPLICABLE;
                }

                //ロケーションマスタの在庫区分判定
                stockDivision = null;
                if (!string.IsNullOrWhiteSpace(paramLocationCd))
                {
                    retVal = getLocationStockDivision(db, paramLocationCd, ref stockDivision);
                    if (!retVal)
                    {
                        //@1行目のロケーション在庫管理区分の取得に失敗しました。
                        message = ComST.GetPropertiesMessage(new string[] { ComID.MB10123, (paramIdx + 1).ToString(), ComID.MB10125 }, paramLanguageId);
                        logger.Error(message);
                        return InvConst.STOCK_DIVISION_RESULT.ERROR;
                    }
                }
                if (stockDivision == 3)
                {
                    //在庫更新対象外
                    return InvConst.STOCK_DIVISION_RESULT.NOT_APPLICABLE;
                }

                return InvConst.STOCK_DIVISION_RESULT.TARGET;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 在庫管理区分取得
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramItemCd">検索対象：品目コード</param>
        /// <param name="paramSpecificationCd">検索対象：仕様コード</param>
        /// <param name="paramStockDivision">取得：在庫管理区分</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool getStockDivision(ComDB db, string paramItemCd,
            string paramSpecificationCd, ref int? paramStockDivision)
        {
            try
            {
                string sql;
                sql = "";
                sql = sql + "select spec.stock_division ";
                sql = sql + "from ";
                sql = sql + "    v_item_specification_regist spec ";
                sql = sql + "where ";
                sql = sql + "    spec.item_cd = @ItemCd ";
                sql = sql + "and spec.specification_cd = @SpecificationCd ";

                paramStockDivision = db.GetEntityByDataClass<int>(
                    sql,
                    new
                    {
                        ItemCd = paramItemCd,
                        SpecificationCd = paramSpecificationCd,
                    });
                if (paramStockDivision == null)
                {
                    paramStockDivision = 0;
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ロケーションマスタ在庫管理区分取得
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramLocationCd">検索対象：ロケーッションコード</param>
        /// <param name="paramStockDivision">取得：在庫管理区分</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool getLocationStockDivision(ComDB db, string paramLocationCd, ref int? paramStockDivision)
        {
            try
            {
                string sql;
                sql = "";
                sql = sql + "select loc.stock_division ";
                sql = sql + "from ";
                sql = sql + "    location loc ";
                sql = sql + "where ";
                sql = sql + "    loc.location_cd = @LocationCd";

                paramStockDivision = db.GetEntityByDataClass<int>(
                    sql,
                    new
                    {
                        LocationCd = paramLocationCd
                    });
                if (paramStockDivision == null)
                {
                    paramStockDivision = 0;
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 受払ソース番号取得（受払ソース（予定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramOrderDivision">オーダー区分</param>
        /// <param name="paramOrderNo">オーダー番号</param>
        /// <param name="paramOrderLineNo1">オーダー行番号１</param>
        /// <param name="paramOrderLineNo2">オーダー行番号２</param>
        /// <param name="paramResultOrderDivision">伝票区分</param>
        /// <param name="paramResultOrderNo">伝票番号</param>
        /// <param name="paramResultOrderLineNo1">伝票行番号１</param>
        /// <param name="paramResultOrderLineNo2">伝票行番号２</param>
        /// <param name="paramInoutSourceNo">受払ソース番号</param>
        /// <returns>true:正常　false:異常</returns>
        public static bool GetInoutNo(ComDB db, string paramLanguageId, int? paramOrderDivision, string paramOrderNo,
            int? paramOrderLineNo1, int? paramOrderLineNo2, int? paramResultOrderDivision,
            string paramResultOrderNo, int? paramResultOrderLineNo1, int? paramResultOrderLineNo2,
            ref string paramInoutSourceNo)
        {
            string sql = string.Empty;

            try
            {
                sql = "";
                sql = sql + "select io.inout_source_no ";
                sql = sql + " from ";
                sql = sql + "     inout_source io ";
                sql = sql + " where ";
                sql = sql + "     io.order_division = @OrderDivision ";
                sql = sql + " and io.order_no = @OrderNo ";
                sql = sql + " and io.order_line_no_1 = @OrderLineNo1 ";
                sql = sql + " and io.order_line_no_2 = @OrderLineNo2 ";
                sql = sql + " and io.result_order_division = @ResultOrderDivision ";
                sql = sql + " and io.result_order_no = @ResultOrderNo ";
                sql = sql + " and io.result_order_line_no_1 = @ResultOrderLineNo1 ";
                sql = sql + " and io.result_order_line_no_2 = @ResultOrderLineNo2 ";

                paramInoutSourceNo = db.GetEntityByDataClass<string>(
                    sql,
                    new
                    {
                        OrderDivision = paramOrderDivision,
                        OrderNo = paramOrderNo,
                        OrderLineNo1 = paramOrderLineNo1,
                        OrderLineNo2 = paramOrderLineNo2,
                        ResultOrderDivision = paramResultOrderDivision,
                        ResultOrderNo = paramResultOrderNo,
                        ResultOrderLineNo1 = paramResultOrderLineNo1,
                        ResultOrderLineNo2 = paramResultOrderLineNo2
                    });
                if ((paramInoutSourceNo == null) || paramInoutSourceNo.Equals(string.Empty))
                {
                    paramInoutSourceNo = string.Empty;
                }

                return true;

            }
            catch (Exception ex)
            {

                //受払ソース番号の取得に失敗しました。。
                logger.Error(sql);
                string message = ComST.GetPropertiesMessage(new string[] { ComID.MB10126, ComID.MB10127 }, paramLanguageId);
                message = "[GetInoutNoFinal]" + paramOrderDivision.ToString() + ", " + paramOrderNo + ", " +
                            paramOrderLineNo1.ToString() + ", " + paramOrderLineNo2.ToString() + ", " +
                            paramResultOrderDivision.ToString() + ", " + paramResultOrderNo + ", " +
                            paramResultOrderLineNo1.ToString() + ", " + paramResultOrderLineNo2.ToString();
                logger.Error(message);
                throw ex;
            }
        }

        /// <summary>
        /// 受入・受入検収時：受払ソース情報取得（受払ソース（予定））
        /// *受入・受入検収の発注情報取得
        /// (発注情報の受払ソース：オーダー区分～オーダー行番号２までは購入依頼情報
        /// 　　　　　　　　　　 　伝票番号～伝票番号行番号２までは発注情報)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramOrderDivision">オーダー区分</param>
        /// <param name="paramOrderNo">オーダー番号</param>
        /// <param name="paramOrderLineNo1">オーダー行番号１</param>
        /// <param name="paramOrderLineNo2">オーダー行番号２</param>
        /// <param name="paramInoutSource">受払ソース番（予定情報）</param>
        /// <returns>true:正常　false:異常</returns>
        public static bool GetInoutOrderInfo(ComDB db, string paramLanguageId, int? paramOrderDivision, string paramOrderNo,
            int? paramOrderLineNo1, int? paramOrderLineNo2, ref ComDao.InoutSourceEntity paramInoutSource)
        {
            string sql = string.Empty;

            try
            {
                sql = "";
                sql = sql + "select io.* ";
                sql = sql + " from ";
                sql = sql + "     inout_source io ";
                sql = sql + " where ";
                sql = sql + "     io.result_order_division = @ResultOrderDivision ";
                sql = sql + " and io.result_order_no = @ResultOrderNo ";
                sql = sql + " and io.result_order_line_no_1 = @ResultOrderLineNo1 ";
                sql = sql + " and io.result_order_line_no_2 = @ResultOrderLineNo2 ";

                paramInoutSource = db.GetEntityByDataClass<ComDao.InoutSourceEntity>(
                    sql,
                    new
                    {
                        ResultOrderDivision = paramOrderDivision,
                        ResultOrderNo = paramOrderNo,
                        ResultOrderLineNo1 = paramOrderLineNo1,
                        ResultOrderLineNo2 = paramOrderLineNo2
                    });
                if (paramInoutSource == null)
                {
                    return true;
                }

                return true;

            }
            catch (Exception ex)
            {

                //受払ソース番号の取得に失敗しました。。
                logger.Error(sql);
                string message = "[GetInoutNoOrderInfo]" +  ComST.GetPropertiesMessage(new string[] { ComID.MB10126, ComID.MB10127 }, paramLanguageId);
                message = paramOrderDivision.ToString() + ", " + paramOrderNo + ", " +
                            paramOrderLineNo1.ToString() + ", " + paramOrderLineNo2.ToString();
                logger.Error(message);

                throw ex;
            }
        }

        /// <summary>
        /// 受入受払ソース番号取得（受払ソース（予定））
        /// *発注クローズ・受入検収の発注取消処理時
        /// (受入情報の受払ソース：オーダー区分～オーダー行番号２までは発注情報
        /// 　　　　　　　　　　 　伝票番号～伝票番号行番号２までは受入情報)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramResultOrderDivision">伝票区分</param>
        /// <param name="paramResultOrderNo">伝票行番号</param>
        /// <param name="paramResultOrderLineNo1">伝票行番号１</param>
        /// <param name="paramResultOrderLineNo2">伝票行番号２</param>
        /// <param name="paramListInoutSourceNo">受払ソース番号リスト</param>
        /// <returns>true:正常　false:異常</returns>
        public static bool GetInoutNoResultInfo(ComDB db, string paramLanguageId, int? paramResultOrderDivision,
            string paramResultOrderNo, int? paramResultOrderLineNo1, int? paramResultOrderLineNo2,
            ref IList<string> paramListInoutSourceNo)
        {
            string sql = string.Empty;

            try
            {
                sql = "";
                sql = sql + "select io.inout_source_no ";
                sql = sql + " from ";
                sql = sql + "     inout_source io ";
                sql = sql + " where ";
                sql = sql + "     io.order_division = @OrderDivision ";
                sql = sql + " and io.order_no = @OrderNo ";
                sql = sql + " and io.order_line_no_1 = @OrderLineNo ";
                sql = sql + " and io.order_line_no_2 = @OrderLineNo2 ";

                paramListInoutSourceNo = db.GetListByDataClass<string>(
                    sql,
                    new
                    {
                        OrderDivision = paramResultOrderDivision,
                        OrderNo = paramResultOrderNo,
                        OrderLineNo = paramResultOrderLineNo1,
                        OrderLineNo2 = paramResultOrderLineNo2
                    });

                return true;

            }
            catch (Exception ex)
            {

                //受払ソース番号の取得に失敗しました。。
                logger.Error(sql);
                string message = "[GetInoutNoResultInfo]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10126, ComID.MB10127 }, paramLanguageId);
                message = paramResultOrderDivision.ToString() + ", " + paramResultOrderNo + ", " +
                            paramResultOrderLineNo1.ToString() + ", " + paramResultOrderLineNo2.ToString();
                logger.Error(message);

                throw ex;
            }
        }

        /// <summary>
        /// 受払ソース番号取得（受払ソース（確定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramOrderDivision">オーダー区分</param>
        /// <param name="paramOrderNo">オーダー番号</param>
        /// <param name="paramOrderLineNo1">オーダー行番号１</param>
        /// <param name="paramOrderLineNo2">オーダー行番号２</param>
        /// <param name="paramResultOrderDivision">伝票区分</param>
        /// <param name="paramResultOrderNo">伝票行番号</param>
        /// <param name="paramResultOrderLineNo1">伝票行番号１</param>
        /// <param name="paramResultOrderLineNo2">伝票行番号２</param>
        /// <param name="paramInoutSourceNo">受払ソース番号</param>
        /// <returns>true:正常　false:異常</returns>
        public static bool GetInoutNoFinal(ComDB db, string paramLanguageId, int? paramOrderDivision, string paramOrderNo,
            int? paramOrderLineNo1, int? paramOrderLineNo2, int? paramResultOrderDivision,
            string paramResultOrderNo, int? paramResultOrderLineNo1, int? paramResultOrderLineNo2,
            ref string paramInoutSourceNo)
        {
            string sql = string.Empty;

            try
            {
                sql = "";
                sql = sql + "select io.inout_source_no ";
                sql = sql + " from ";
                sql = sql + "     inout_source_final io ";
                sql = sql + " where ";
                sql = sql + "     io.order_division = @OrderDivision ";
                sql = sql + " and io.order_no = @OrderNo ";
                sql = sql + " and io.order_line_no_1 = @OrderLineNo1 ";
                sql = sql + " and io.order_line_no_2 = @OrderLineNo2 ";
                sql = sql + " and io.result_order_division = @ResultOrderDivision ";
                sql = sql + " and io.result_order_no = @ResultOrderNo ";
                sql = sql + " and io.result_order_line_no_1 = @ResultOrderLineNo1 ";
                sql = sql + " and io.result_order_line_no_2 = @ResultOrderLineNo2 ";

                paramInoutSourceNo = db.GetEntityByDataClass<string>(
                    sql,
                    new
                    {
                        OrderDivision = paramOrderDivision,
                        OrderNo = paramOrderNo,
                        OrderLineNo1 = paramOrderLineNo1,
                        OrderLineNo2 = paramOrderLineNo2,
                        ResultOrderDivision = paramResultOrderDivision,
                        ResultOrderNo = paramResultOrderNo,
                        ResultOrderLineNo1 = paramResultOrderLineNo1,
                        ResultOrderLineNo2 = paramResultOrderLineNo2
                    });
                if ((paramInoutSourceNo == null) || paramInoutSourceNo.Equals(string.Empty))
                {
                    paramInoutSourceNo = string.Empty;
                }

                return true;

            }
            catch (Exception ex)
            {

                //受払ソース番号(完了)の取得に失敗しました。
                logger.Error(sql);
                string message = ComST.GetPropertiesMessage(new string[] { ComID.MB10126, ComID.MB10128 }, paramLanguageId);
                message = "[GetInoutNoFinal]" + paramOrderDivision.ToString() + ", " + paramOrderNo + ", " +
                        paramOrderLineNo1.ToString() + ", " + paramOrderLineNo2.ToString() + ", " +
                        paramResultOrderDivision.ToString() + ", " + paramResultOrderNo + ", " +
                        paramResultOrderLineNo1.ToString() + ", " + paramResultOrderLineNo2.ToString();
                logger.Error(message);

                throw ex;
            }
        }

        /// <summary>
        /// 受入・受入検収時：受払ソース番号取得（受払ソース（確定））
        /// *受入・受入検収の発注情報取得
        /// (発注情報の受払ソース：オーダー区分～オーダー行番号２までは購入依頼情報
        /// 　　　　　　　　　　 　伝票番号～伝票番号行番号２までは発注情報)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramOrderDivision">オーダー区分</param>
        /// <param name="paramOrderNo">受注番号</param>
        /// <param name="paramOrderLineNo1">受注行番号1</param>
        /// <param name="paramOrderLineNo2">受注行番号2</param>
        /// <param name="paramInoutSourceNo">受払ソース番号</param>
        /// <returns>true:正常　false:異常</returns>
        public static bool GetInoutNoOrderInfoFinal(ComDB db, string paramLanguageId, int? paramOrderDivision, string paramOrderNo,
            int? paramOrderLineNo1, int? paramOrderLineNo2, ref string paramInoutSourceNo)
        {
            string sql = string.Empty;

            try
            {
                sql = "";
                sql = sql + "select io.inout_source_no ";
                sql = sql + " from ";
                sql = sql + "     inout_source_final io ";
                sql = sql + " where ";
                sql = sql + "     io.result_order_division = @ResultOrderDivision ";
                sql = sql + " and io.result_order_no = @ResultOrderNo ";
                sql = sql + " and io.result_order_line_no_1 = @ResultOrderLineNo1 ";
                sql = sql + " and io.result_order_line_no_2 = @ResultOrderLineNo2 ";

                paramInoutSourceNo = db.GetEntityByDataClass<string>(
                    sql,
                    new
                    {
                        ResultOrderDivision = paramOrderDivision,
                        ResultOrderNo = paramOrderNo,
                        ResultOrderLineNo1 = paramOrderLineNo1,
                        ResultOrderLineNo2 = paramOrderLineNo2
                    });
                if ((paramInoutSourceNo == null) || paramInoutSourceNo.Equals(string.Empty))
                {
                    paramInoutSourceNo = string.Empty;
                }

                return true;

            }
            catch (Exception ex)
            {

                //受払ソース番号の取得に失敗しました。。
                logger.Error(sql);
                string message = "[GetInoutNoOrderInfoFinal]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10126, ComID.MB10127 }, paramLanguageId);
                message = paramOrderDivision.ToString() + ", " + paramOrderNo + ", " +
                            paramOrderLineNo1.ToString() + ", " + paramOrderLineNo2.ToString();
                logger.Error(message);

                throw ex;
            }
        }

    }
}
