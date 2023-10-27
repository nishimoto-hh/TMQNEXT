using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommonSTDUtil.CommonLogger;

using ComConst = APConstants.APConstants;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComID = CommonAPUtil.APCommonUtil.APResources.ID;
using ComST = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using InvConst = CommonAPUtil.APInventoryUtil.InventoryConst;
using InvDao = CommonAPUtil.APInventoryUtil.InventoryDataClass;

namespace CommonAPUtil.APInventoryUtil
{
    /// <summary>
    /// 在庫更新：受払ソース登録
    /// </summary>
    public class InvTranInOutSource
    {
        #region 固定値

        /// <summary>
        /// 手続き区分
        /// </summary>
        public static class INOUT_MAKE_FLG
        {
            /// <summary>10:受払ソース(受払予定)登録・更新</summary>
            public const int ADD = 10;
            /// <summary>20:受払ソース(受払予定)削除</summary>
            public const int DELETE = 20;
            /// <summary>21:受払ソース(受払予定)削除:オーダー番号単位</summary>
            public const int DELETE_ORDER = 21;
            /// <summary>30:納期回答</summary>
            public const int ANSWER = 30;
            /// <summary>31:納期回答(オーダー番号単位)</summary>
            public const int ANSWER_ORDER = 31;
            /// <summary>40:受払数減算（引当）（受払数＋ロケーション情報更新）</summary>
            public const int UPDATE_QTY_SUBTRACTION_OTHER = 40;
            /// <summary>41:受払数減算（引当）（受払数のみ更新）</summary>
            public const int UPDATE_QTY_ONLY_SUBTRACTION = 41;
            /// <summary>42:受払数減算（引当）（発注用：購入依頼数計算時：受払数のみ更新）</summary>
            public const int UPDATE_QTY_PURCHASE_SUBTRACTION = 42;
            /// <summary>50:受払数加算（引当取消）（受払数＋ロケーション情報更新）</summary>
            public const int UPDATE_QTY_ADDITION_OTHER = 50;
            /// <summary>51:受払数加算（引当取消）（受払数のみ更新）</summary>
            public const int UPDATE_QTY_ONLY_ADDITION = 51;
            /// <summary>52:受払数加算（引当取消）（発注用：購入依頼数計算時：受払数のみ更新）</summary>
            public const int UPDATE_QTY_PURCHASE_ADDITION = 52;
            /// <summary>60:受払ソース確定登録　</summary>
            public const int FINAL_ADD = 60;
            /// <summary>61:受払ソース確定登録(クローズ用：存在する場合は更新)　</summary>
            public const int FINAL_ADD_UPDATE_ALL = 61;
            /// <summary>62:受払ソース確定登録(オーダー番号単位)　</summary>
            public const int FINAL_ADD_ORDER = 62;
            /// <summary>63:受払ソース確定登録(受払数＝０)　</summary>
            public const int FINAL_ADD_ZERO = 63;
            /// <summary>70:受払ソース確定更新　</summary>
            public const int FINAL_UPDATE = 70;
            /// <summary>80:受払ソース確定取消　</summary>
            public const int FINAL_DELETE = 80;
            /// <summary>81:受払ソース確定取消(存在する場合は更新)　</summary>
            public const int FINAL_DELETE_UPDATE = 81;
        }

        #endregion

        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// 受払ソース(受払予定)（登録・更新・削除）受払ソース（確定）（登録・更新・削除）
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramMakeFlg">手続き区分</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool InOutSourceRegist(ComDB db, int paramMakeFlg, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;

            // ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(InvConst.LOG_NAME);

            switch (paramMakeFlg)
            {
                case INOUT_MAKE_FLG.ADD:
                    //受払ソース(受払予定)：登録・更新
                    retVal = addInOutSource(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.DELETE:
                    //受払ソース(受払予定)：受払ソース番号単位の削除
                    retVal = deleteInOutSource(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.DELETE_ORDER:
                    //受払ソース(受払予定)：オーダー番号単位の削除
                    retVal = deleteInOutSourceOrderNo(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.ANSWER:
                    //受払ソース(受払予定)：納期回答（受払予定日更新）
                    retVal = updateInOutSourceInoutDate(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.ANSWER_ORDER:
                    //受払ソース(受払予定)：納期回答（受払予定日更新:オーダー番号単位）
                    retVal = updateInOutSourceInoutDateForOrderNo(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.UPDATE_QTY_SUBTRACTION_OTHER:
                    //// (未使用)
                    ////受払ソース(受払予定)：受払数減算：ﾛｹｰｼｮﾝ他更新
                    //retVal = updateInOutSourceQtySubtraction(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION:
                    //受払ソース(受払予定)：受払数減算：受払数のみ更新
                    retVal = updateInOutSourceQtyOnlySubtraction(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.UPDATE_QTY_PURCHASE_SUBTRACTION:
                    //受払ソース(受払予定)：受払数減算：発注画面：購入依頼数計算用：受払数のみ更新
                    retVal = updateInOutSourceQtyPurchaseSubtraction(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.UPDATE_QTY_ADDITION_OTHER:
                    //// (未使用)
                    ////受払ソース(受払予定)：受払数加算：ﾛｹｰｼｮﾝ他更新
                    //retVal = updateInOutSourceQtyAddition(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION:
                    //受払ソース(受払予定)：受払数加算：受払数のみ更新
                    retVal = updateInOutSourceQtyOnlyAddition(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.UPDATE_QTY_PURCHASE_ADDITION:
                    //受払ソース(受払予定)：受払数加算：発注画面：購入依頼数計算用：受払数のみ更新
                    retVal = updateInOutSourceQtyPurchaseAddition(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.FINAL_ADD:
                    // 受払ソース(完了)：受払ソース確定登録
                    //  受払ソース(受払予定)→受払ソース(完了)
                    retVal = addConfirmedInOutSourceFinal(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.FINAL_ADD_UPDATE_ALL:
                    //受払ソース(完了)：受払ソース確定登録
                    //  受払ソース(受払予定)→受払ソース(完了)
                    //  ※受払ソース(完了)が存在する場合は更新
                    // 受払ソース（受払予定）の数量で登録更新する
                    retVal = mergeAllConfirmedInOutSourceFinal(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.FINAL_ADD_ORDER:
                    // 受払ソース(完了)：受払ソース確定登録(オーダー番号単位)
                    //  受払ソース(受払予定)→受払ソース(完了)
                    retVal = addConfirmedInOutSourceFinalOrder(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.FINAL_ADD_ZERO:
                    // 受払ソース(完了)：受払ソース確定登録(受払数＝０の場合)
                    //  受払ソース(受払予定)→受払ソース(完了)
                    retVal = addConfirmedInOutSourceFinalZero(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.FINAL_UPDATE:
                    //// (未使用)
                    ////受払ソース(完了)：受払ソース確定更新
                    //retVal = updateInOutSourceFinal(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.FINAL_DELETE:
                    // 受払ソース(完了)：受払ソース確定取消
                    //  受払ソース(完了)→受払ソース(受払予定)
                    //  ※受払ソース(受払予定)が存在する場合は更新
                    retVal = addResetInOutSourceFinal(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;

                case INOUT_MAKE_FLG.FINAL_DELETE_UPDATE:
                    // 受払ソース(完了)：受払ソース確定取消
                    //  受払ソース(完了)→受払ソース(受払予定)
                    //  ※受払ソース(受払予定)が存在する場合は更新
                    retVal = mergeResetInOutSourceFinal(db, paramInfo, paramLanguageId, ref paramMessage);
                    break;
                default:
                    break;
            }

            return retVal;

        }

        #region Privateメソッド

        /// <summary>
        /// 受払ソース（予定）登録・更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool addInOutSource(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {
            string sql = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(paramInfo.InoutSourceNo))
                {

                    //受払ソース番号採番
                    paramInfo.InoutSourceNo =
                        CommonAPUtil.APStoredPrcUtil.APStoredPrcUtil.ProSeqGetNo(db, ComConst.SEQUENCE_PROC_NAME.INOUT_SOURCE_NO);

                    //受払ソース（予定）新規登録
                    sql = "";
                    sql = sql + " insert ";
                    sql = sql + " into inout_source( ";
                    sql = sql + "  inout_source_no ";
                    sql = sql + "  ,inout_division ";
                    sql = sql + "  ,inout_date ";
                    sql = sql + "  ,order_division ";
                    sql = sql + "  ,order_no ";
                    sql = sql + "  ,order_line_no_1 ";
                    sql = sql + "  ,order_line_no_2 ";
                    sql = sql + "  ,result_order_division ";
                    sql = sql + "  ,result_order_no ";
                    sql = sql + "  ,result_order_line_no_1 ";
                    sql = sql + "  ,result_order_line_no_2 ";
                    sql = sql + "  ,item_cd ";
                    sql = sql + "  ,specification_cd ";
                    sql = sql + "  ,location_cd ";
                    sql = sql + "  ,lot_no ";
                    sql = sql + "  ,sub_lot_no_1 ";
                    sql = sql + "  ,sub_lot_no_2 ";
                    sql = sql + "  ,inout_qty ";
                    sql = sql + "  ,reference_no ";
                    sql = sql + "  ,assign_flg ";
                    sql = sql + "  ,input_menu_id ";
                    sql = sql + "  ,input_display_id ";
                    sql = sql + "  ,input_control_id ";
                    sql = sql + "  ,update_menu_id ";
                    sql = sql + "  ,update_display_id ";
                    sql = sql + "  ,update_control_id ";
                    sql = sql + "  ,input_date ";
                    sql = sql + "  ,input_user_id ";
                    sql = sql + "  ,update_date ";
                    sql = sql + "  ,update_user_id ";
                    sql = sql + " ) ";
                    sql = sql + " values ( ";
                    sql = sql + "    @InoutSourceNo ";
                    sql = sql + "  , @InoutDivision ";
                    sql = sql + "  , to_date(@InoutDate, 'YYYY/MM/DD')";
                    sql = sql + "  , @OrderDivision ";
                    sql = sql + "  , @OrderNo ";
                    sql = sql + "  , @OrderLineNo1 ";
                    sql = sql + "  , @OrderLineNo2 ";
                    sql = sql + "  , @ResultOrderDivision ";
                    sql = sql + "  , @ResultOrderNo ";
                    sql = sql + "  , @ResultOrderLineNo1 ";
                    sql = sql + "  , @ResultOrderLineNo2 ";
                    sql = sql + "  , @ItemCd ";
                    sql = sql + "  , @SpecificationCd ";
                    sql = sql + "  , @LocationCd ";
                    sql = sql + "  , @LotNo ";
                    sql = sql + "  , @SubLotNo1 ";
                    sql = sql + "  , @SubLotNo2 ";
                    sql = sql + "  , @InoutQty ";
                    sql = sql + "  , @ReferenceNo ";
                    sql = sql + "  , @AssignFlg ";
                    sql = sql + "  , @InputMenuId ";
                    sql = sql + "  , @InputDisplayId ";
                    sql = sql + "  , @InputControlId ";
                    sql = sql + "  , @InputMenuId ";
                    sql = sql + "  , @InputDisplayId ";
                    sql = sql + "  , @InputControlId ";
                    sql = sql + "  , CURRENT_TIMESTAMP   ";
                    sql = sql + "  , @UserId ";
                    sql = sql + "  , CURRENT_TIMESTAMP  ";
                    sql = sql + "  , @UserId ";
                    sql = sql + " )";

                    int regFlg;
                    regFlg = db.Regist(
                        sql,
                        new
                        {
                            InoutSourceNo = paramInfo.InoutSourceNo,
                            InoutDivision = paramInfo.InoutDivision,
                            InoutDate = string.Format("{0:yyyy/MM/dd}", paramInfo.InoutDate),
                            OrderDivision = paramInfo.OrderDivision,
                            OrderNo = paramInfo.OrderNo,
                            OrderLineNo1 = paramInfo.OrderLineNo1,
                            OrderLineNo2 = paramInfo.OrderLineNo2,
                            ResultOrderDivision = paramInfo.ResultOrderDivision,
                            ResultOrderNo = paramInfo.ResultOrderNo,
                            ResultOrderLineNo1 = paramInfo.ResultOrderLineNo1,
                            ResultOrderLineNo2 = paramInfo.ResultOrderLineNo2,
                            ItemCd = paramInfo.ItemCd,
                            SpecificationCd = paramInfo.SpecificationCd,
                            LocationCd = paramInfo.LocationCd,
                            LotNo = paramInfo.LotNo,
                            SubLotNo1 = paramInfo.SubLotNo1,
                            SubLotNo2 = paramInfo.SubLotNo2,
                            InoutQty = paramInfo.InoutQty,
                            ReferenceNo = paramInfo.ReferenceNo,
                            AssignFlg = paramInfo.AssignFlg,
                            InputMenuId = paramInfo.InputMenuId,
                            InputDisplayId = paramInfo.InputDisplayId,
                            InputControlId = paramInfo.InputControlId,
                            UserId = paramInfo.UserId
                        });
                    if (regFlg < 0)
                    {
                        // ログ出力：受払ソース（受払予定）登録に失敗しました
                        paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10129, ComID.MB10133 }, paramLanguageId);
                        paramMessage += paramInfo.OrderDivision + "," + paramInfo.OrderNo + "," + paramInfo.OrderLineNo1 + "," + paramInfo.OrderLineNo2
                            + "," + paramInfo.ResultOrderDivision + "," + paramInfo.ResultOrderNo + "," + paramInfo.ResultOrderLineNo1 + "," + paramInfo.ResultOrderLineNo2;
                        logger.Error(paramMessage);
                        logger.Error(sql);
                        return false;
                    }

                }
                else
                {
                    //受払ソース（予定）更新
                    sql = "";
                    sql = sql + " update inout_source ";
                    sql = sql + "set ";
                    sql = sql + "  inout_division = @InoutDivision ";
                    sql = sql + "  ,inout_date = to_date(@InoutDate, 'YYYY/MM/DD')";
                    sql = sql + "  ,order_division = @OrderDivision ";
                    sql = sql + "  ,order_no = @OrderNo ";
                    sql = sql + "  ,order_line_no_1 = @OrderLineNo1 ";
                    sql = sql + "  ,order_line_no_2 = @OrderLineNo2 ";
                    sql = sql + "  ,result_order_division = @ResultOrderDivision ";
                    sql = sql + "  ,result_order_no = @ResultOrderNo ";
                    sql = sql + "  ,result_order_line_no_1 = @ResultOrderLineNo1 ";
                    sql = sql + "  ,result_order_line_no_2 = @ResultOrderLineNo2 ";
                    sql = sql + "  ,item_cd = @ItemCd ";
                    sql = sql + "  ,specification_cd = @SpecificationCd ";
                    sql = sql + "  ,location_cd = @LocationCd ";
                    sql = sql + "  ,lot_no = @LotNo ";
                    sql = sql + "  ,sub_lot_no_1 = @SubLotNo1 ";
                    sql = sql + "  ,sub_lot_no_2 = @SubLotNo2 ";
                    //sql = sql + "  ,inout_qty  = " + paramInfo.InoutQty + " ";
                    sql = sql + "  ,update_menu_id = @InputMenuId ";
                    sql = sql + "  ,update_display_id = @InputDisplayId ";
                    sql = sql + "  ,update_control_id = @InputControlId ";
                    sql = sql + "  ,update_date = now() ";
                    sql = sql + "  ,update_user_id = @UserId ";
                    sql = sql + "where ";
                    sql = sql + "    inout_source_no = @InoutSourceNo ";

                    int regFlg;
                    regFlg = db.Regist(
                        sql,
                        new
                        {
                            InoutDivision = paramInfo.InoutDivision,
                            InoutDate = string.Format("{0:yyyy/MM/dd}", paramInfo.InoutDate),
                            OrderDivision = paramInfo.OrderDivision,
                            OrderNo = paramInfo.OrderNo,
                            OrderLineNo1 = paramInfo.OrderLineNo1,
                            OrderLineNo2 = paramInfo.OrderLineNo2,
                            ResultOrderDivision = paramInfo.ResultOrderDivision,
                            ResultOrderNo = paramInfo.ResultOrderNo,
                            ResultOrderLineNo1 = paramInfo.ResultOrderLineNo1,
                            ResultOrderLineNo2 = paramInfo.ResultOrderLineNo2,
                            ItemCd = paramInfo.ItemCd,
                            SpecificationCd = paramInfo.SpecificationCd,
                            LocationCd = paramInfo.LocationCd,
                            LotNo = paramInfo.LotNo,
                            SubLotNo1 = paramInfo.SubLotNo1,
                            SubLotNo2 = paramInfo.SubLotNo2,
                            InputMenuId = paramInfo.InputMenuId,
                            InputDisplayId = paramInfo.InputDisplayId,
                            InputControlId = paramInfo.InputControlId,
                            UserId = paramInfo.UserId,
                            InoutSourceNo = paramInfo.InoutSourceNo
                        });
                    if (regFlg < 0)
                    {
                        // ログ出力：受払ソース（受払予定）更新に失敗しました
                        paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10130, ComID.MB10133 }, paramLanguageId);
                        paramMessage += paramInfo.OrderDivision + "," + paramInfo.OrderNo + "," + paramInfo.OrderLineNo1 + "," + paramInfo.OrderLineNo2
                            + "," + paramInfo.ResultOrderDivision + "," + paramInfo.ResultOrderNo + "," + paramInfo.ResultOrderLineNo1 + "," + paramInfo.ResultOrderLineNo2;

                        logger.Error(paramMessage);
                        logger.Error(sql);
                        return false;
                    }

                }

                return true;

            }
            catch (Exception ex)
            {
                // ログ出力：受払ソース（受払予定）登録・更新に失敗しました
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10131, ComID.MB10133 }, paramLanguageId);
                paramMessage += paramInfo.OrderDivision + "," + paramInfo.OrderNo + "," + paramInfo.OrderLineNo1 + "," + paramInfo.OrderLineNo2
                    + "," + paramInfo.ResultOrderDivision + "," + paramInfo.ResultOrderNo + "," + paramInfo.ResultOrderLineNo1 + "," + paramInfo.ResultOrderLineNo2;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 受払ソース（受払予定）削除
        /// ※受払ソース番号単位の削除
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool deleteInOutSource(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {
                sql = "";
                sql = sql + " delete from inout_source ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：受払ソース（受払予定）受払番号情報の削除に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10132, ComID.MB10133, ComID.MB10135 }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：受払ソース（受払予定）受払番号情報の削除に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10132, ComID.MB10133, ComID.MB10135 }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 受払ソース（受払予定）削除
        /// ※オーダー番号単位の削除
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool deleteInOutSourceOrderNo(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {
                sql = "";
                sql = sql + " delete from inout_source ";
                sql = sql + "where ";
                sql = sql + "    order_division = @OrderDivision ";
                sql = sql + "and order_no = @OrderNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        OrderDivision = paramInfo.OrderDivision,
                        OrderNo = paramInfo.OrderNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：受払ソース（受払予定）オーダー番号情報の削除に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10132, ComID.MB10133, ComID.MB10136 }, paramLanguageId);
                    paramMessage += paramInfo.OrderNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：受払ソース（受払予定）オーダー番号情報の削除に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10132, ComID.MB10133, ComID.MB10136 }, paramLanguageId);
                paramMessage += paramInfo.OrderNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 納期回答
        /// ※受払予定日更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool updateInOutSourceInoutDate(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {
                //受払ソース（予定）：受払予定日のみ更新
                sql = "";
                sql = sql + " update inout_source ";
                sql = sql + "set ";
                sql = sql + "   inout_date = to_date(@InoutDate, 'YYYY/MM/DD')";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutDate = string.Format("{0:yyyy/MM/dd}", paramInfo.InoutDate),
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId,
                        InoutSourceNo =paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：受払ソース（受払予定） 受払予定日の更新に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10137, ComID.MB10133, ComID.MB10138 }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：受払ソース（受払予定） 受払予定日の更新に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10137, ComID.MB10133, ComID.MB10138 }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 納期回答
        /// ※受払予定日更新(オーダー番号単位)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool updateInOutSourceInoutDateForOrderNo(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {
                //受払ソース（予定）：受払予定日のみ更新
                sql = "";
                sql = sql + " update inout_source ";
                sql = sql + "set ";
                sql = sql + "   inout_date = to_date(@InoutDate, 'YYYY/MM/DD')";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "     order_division = @OrderDivision ";
                sql = sql + " and order_no = @OrderNo ";
                sql = sql + " and result_order_division = @ResultOrderDivision ";
                sql = sql + " and result_order_no = @ResultOrderNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutDate = string.Format("{0:yyyy/MM/dd}", paramInfo.InoutDate),
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId,
                        OrderDivision = paramInfo.OrderDivision,
                        OrderNo = paramInfo.OrderNo,
                        ResultOrderDivision = paramInfo.ResultOrderDivision,
                        ResultOrderNo = paramInfo.ResultOrderNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：受払ソース（受払予定） 受払予定日の更新に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10137, ComID.MB10133, ComID.MB10138 }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：受払ソース（受払予定） 受払予定日の更新に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10137, ComID.MB10133, ComID.MB10138 }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 受払数更新処理（減算）
        /// ※受払日・ロケーション・ロット・サブロット更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool updateInOutSourceQtySubtraction(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {
                // 数量＝０の場合は更新しない
                if (paramInfo.InoutQty == 0)
                {
                    return true;
                }

                //受払ソース（予定）：受払数－（引当数）更新
                sql = "";
                sql = sql + " update inout_source ";
                sql = sql + "set ";
                sql = sql + "   inout_qty  = inout_qty - (@InoutQty) ";
                sql = sql + "  ,inout_date = to_date(@InoutDate, 'YYYY/MM/DD')";
                sql = sql + "  ,location_cd = @LocationCd ";
                sql = sql + "  ,lot_no = @LotNo ";
                sql = sql + "  ,sub_lot_no_1 = @SubLotNo1 ";
                sql = sql + "  ,sub_lot_no_2 = @SubLotNo2 ";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutQty = paramInfo.InoutQty,
                        InoutDate = string.Format("{0:yyyy/MM/dd}", paramInfo.InoutDate),
                        LocationCd = paramInfo.LocationCd,
                        LotNo = paramInfo.LotNo,
                        SubLotNo1 = paramInfo.SubLotNo1,
                        SubLotNo2 = paramInfo.SubLotNo2,
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId,
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[受払数(減算)] 受払ソース（受払予定） 受払数の更新に失敗しました。
                    paramMessage = "[UpdateInOutSourceQtySubtraction]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10140, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[受払数更新（減算）] 受払ソース（受払予定） 受払数の更新に失敗しました。
                paramMessage = "[UpdateInOutSourceQtySubtraction]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10140, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 受払数更新処理（減算）
        /// ※受払数のみ更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool updateInOutSourceQtyOnlySubtraction(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {
                // 数量＝０の場合は更新しない
                if (paramInfo.InoutQty == 0)
                {
                    return true;
                }

                //受払ソース（予定）：受払数－（引当数）更新
                sql = "";
                sql = sql + " update inout_source ";
                sql = sql + "set ";
                sql = sql + "   inout_qty  = inout_qty - (@InoutQty) ";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutQty = paramInfo.InoutQty,
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId,
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[受払数(減算)] 受払ソース（受払予定） 受払数の更新に失敗しました。
                    paramMessage = "[UpdateInOutSourceQtyOnlySubtraction]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10140, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[受払数更新（減算）] 受払ソース（受払予定） 受払数の更新に失敗しました。
                paramMessage = "[UpdateInOutSourceQtyOnlySubtraction]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10140, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 受払数更新処理（減算）
        /// ※発注画面：購入依頼数更新用：受払数のみ更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool updateInOutSourceQtyPurchaseSubtraction(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {
                // 数量＝０の場合は更新しない
                if (paramInfo.InoutQty == 0)
                {
                    return true;
                }

                //受払ソース（予定）：受払数－（引当数）更新
                //　発注画面：購入依頼数更新時　０未満になる場合は０に置き換える
                sql = "";
                sql = sql + " update inout_source ";
                sql = sql + "set ";
                sql = sql + "   inout_qty  = (case when (inout_qty - (@InoutQty)) < 0 then 0 else inout_qty - (@InoutQty)  end)";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutQty = paramInfo.InoutQty,
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId,
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[受払数(減算)] 受払ソース（受払予定） 受払数の更新に失敗しました。
                    paramMessage = "[UpdateInOutSourceQtyPurchaseSubtraction]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10140, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[受払数更新（減算）] 受払ソース（受払予定） 受払数の更新に失敗しました。
                paramMessage = "[UpdateInOutSourceQtyPurchaseSubtraction]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10140, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 受払数更新（加算）
        /// ※受払日・ロケーション・ロット・サブロット更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool updateInOutSourceQtyAddition(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {
                // 数量＝０の場合は更新しない
                if (paramInfo.InoutQty == 0)
                {
                    return true;
                }

                //受払ソース（予定）：受払数＋引当数　更新
                sql = "";
                sql = sql + " update inout_source ";
                sql = sql + "set ";
                sql = sql + "   inout_qty  = inout_qty + (@InoutQty) ";
                sql = sql + "  ,inout_date = to_date(@InoutDate, 'YYYY/MM/DD')";
                sql = sql + "  ,location_cd = @LocationCd ";
                sql = sql + "  ,lot_no = @LotNo ";
                sql = sql + "  ,sub_lot_no_1 = @SubLotNo1 ";
                sql = sql + "  ,sub_lot_no_2 = @SubLotNo2 ";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutQty = paramInfo.InoutQty,
                        InoutDate = string.Format("{0:yyyy/MM/dd}", paramInfo.InoutDate),
                        LocationCd = paramInfo.LocationCd,
                        LotNo = paramInfo.LotNo,
                        SubLotNo1 = paramInfo.SubLotNo1,
                        SubLotNo2 = paramInfo.SubLotNo2,
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId,
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[受払数更新（加算）]受払ソース（受払予定） 受払数の更新に失敗しました。
                    paramMessage = "[UpdateInOutSourceQtyAddition]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10141, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[受払数更新（加算）]受払ソース（受払予定） 受払数の更新に失敗しました。
                paramMessage = "[UpdateInOutSourceQtyAddition]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10141, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 受払数更新（加算）
        /// ※受払数のみ更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool updateInOutSourceQtyOnlyAddition(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {
                // 数量＝０の場合は更新しない
                if (paramInfo.InoutQty == 0)
                {
                    return true;
                }

                //受払ソース（予定）：受払数＋引当数　更新
                sql = "";
                sql = sql + " update inout_source ";
                sql = sql + "set ";
                sql = sql + "   inout_qty  = inout_qty + (@InoutQty) ";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutQty = paramInfo.InoutQty,
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId,
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[受払数更新（加算）]受払ソース（受払予定） 受払数の更新に失敗しました。
                    paramMessage = "[UpdateInOutSourceQtyOnlyAddition]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10141, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[受払数更新（加算）]受払ソース（受払予定） 受払数の更新に失敗しました。
                paramMessage = "[UpdateInOutSourceQtyOnlyAddition]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10141, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 受払数更新（加算）
        /// ※発注画面：購入依頼数更新用：受払数のみ更新
        /// ※計算結果＞購入依頼数の場合は　購入依頼数にする
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool updateInOutSourceQtyPurchaseAddition(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {
                // 数量＝０の場合は更新しない
                if (paramInfo.InoutQty == 0)
                {
                    return true;
                }

                //受払ソース（予定）：受払数＋引当数　更新
                sql = "";
                sql = sql + " update inout_source ";
                sql = sql + "set ";
                sql = sql + "   inout_qty  = (case when (inout_qty + (@InoutQty)) > @PlanInoutQty "
                          + " then @PlanInoutQty "
                          + " else inout_qty + (@InoutQty) end)";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutQty = paramInfo.InoutQty,
                        PlanInoutQty = paramInfo.PlanInoutQty,
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId,
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[受払数更新（加算）]受払ソース（受払予定） 受払数の更新に失敗しました。
                    paramMessage = "[UpdateInOutSourceQtyPurchaseAddition]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10141, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[受払数更新（加算）]受払ソース（受払予定） 受払数の更新に失敗しました。
                paramMessage = "[UpdateInOutSourceQtyPurchaseAddition]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10141, ComID.MB10133, ComID.MB10142 }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 確定処理（Insert）
        /// ※受払番号単位登録
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool addConfirmedInOutSourceFinal(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {

                // 受払ソース（受払予定）→受払ソース（確定）
                sql = "";
                sql = sql + " insert ";
                sql = sql + " into inout_source_final( ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " ) ";
                sql = sql + " select  ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " from   ";
                sql = sql + "   inout_source   ";
                sql = sql + " where   ";
                sql = sql + "   inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定処理] 受払ソース（確定）登録に失敗しました
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10147, ComID.MB10134, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                // 受払ソース（受払予定）削除
                sql = "";
                sql = sql + " delete from inout_source ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定]受払ソース（受払予定）情報の削除に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10146, ComID.MB10147, ComID.MB10133, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[確定処理] 受払ソース（確定）登録に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10147, ComID.MB10134, string.Empty }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 確定処理（Merge）
        /// ※既存情報が存在する場合は更新
        /// ※受払ソース(受払予定)の受払数で登録更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool mergeAllConfirmedInOutSourceFinal(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {

                // 受払ソース（受払予定）→受払ソース（確定）
                sql = "";
                sql = sql + " insert ";
                sql = sql + " into inout_source_final( ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " ) ";
                sql = sql + " select  ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " from   ";
                sql = sql + "   inout_source   ";
                sql = sql + " where   ";
                sql = sql + "   inout_source_no = @InoutSourceNo ";
                sql = sql + " on conflict (inout_source_no) ";
                sql = sql + " do update set ";
                sql = sql + "   inout_qty = inout_source_final.inout_qty + excluded.inout_qty";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo,
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定処理(Merge)] 受払ソース（確定）登録に失敗しました
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10148, ComID.MB10134, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                // 受払ソース（受払予定）削除
                sql = "";
                sql = sql + " delete from inout_source ";
                sql = sql + "where ";
                sql = sql + "     inout_source_no = @InoutSourceNo ";

                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定]受払ソース（受払予定）情報の削除に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10146, ComID.MB10147, ComID.MB10133, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[確定処理(Merge] 受払ソース（確定）新規登録に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10148, ComID.MB10134, string.Empty }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 確定処理（Insert）
        /// ※オーダー番号登録
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool addConfirmedInOutSourceFinalOrder(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {

                // 受払ソース（受払予定）→受払ソース（確定）
                sql = "";
                sql = sql + " insert ";
                sql = sql + " into inout_source_final( ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " ) ";
                sql = sql + " select  ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " from   ";
                sql = sql + "   inout_source   ";
                sql = sql + " where   ";
                sql = sql + "     order_division = @OrderDivision ";
                sql = sql + " and order_no = @OrderNo ";
                sql = sql + " and result_order_division = @ResultOrderDivision ";
                sql = sql + " and result_order_no = @ResultOrderNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        OrderDivision = paramInfo.OrderDivision,
                        OrderNo = paramInfo.OrderNo,
                        ResultOrderDivision = paramInfo.ResultOrderDivision,
                        ResultOrderNo = paramInfo.ResultOrderNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定処理] 受払ソース（確定）登録に失敗しました
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10147, ComID.MB10134, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                // 受払ソース（受払予定）削除
                sql = "";
                sql = sql + " delete from inout_source ";
                sql = sql + "where ";
                sql = sql + "     order_division = @OrderDivision ";
                sql = sql + " and order_no = @OrderNo ";
                sql = sql + " and result_order_division = @ResultOrderDivision ";
                sql = sql + " and result_order_no = @ResultOrderNo ";

                regFlg = db.Regist(
                    sql,
                    new
                    {
                        OrderDivision = paramInfo.OrderDivision,
                        OrderNo = paramInfo.OrderNo,
                        ResultOrderDivision = paramInfo.ResultOrderDivision,
                        ResultOrderNo = paramInfo.ResultOrderNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定]受払ソース（受払予定）情報の削除に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10146, ComID.MB10147, ComID.MB10133, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[確定処理] 受払ソース（確定）登録に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10147, ComID.MB10134, string.Empty }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 確定処理（Insert）
        /// ※受払数=0
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool addConfirmedInOutSourceFinalZero(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {

                // 受払ソース（受払予定）→受払ソース（確定）
                sql = "";
                sql = sql + " insert ";
                sql = sql + " into inout_source_final( ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " ) ";
                sql = sql + " select  ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " from   ";
                sql = sql + "   inout_source   ";
                sql = sql + " where   ";
                sql = sql + "     inout_source_no = @InoutSourceNo ";
                sql = sql + " and inout_qty = 0 ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定処理] 受払ソース（確定）登録に失敗しました
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10147, ComID.MB10134, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                // 受払ソース（受払予定）削除
                sql = "";
                sql = sql + " delete from inout_source ";
                sql = sql + "where ";
                sql = sql + "     inout_source_no = @InoutSourceNo ";
                sql = sql + " and inout_qty = 0 ";

                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定]受払ソース（受払予定）情報の削除に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10146, ComID.MB10147, ComID.MB10133, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[確定処理] 受払ソース（確定）登録に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10147, ComID.MB10134, string.Empty }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 確定情報更新
        /// ※受払数更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool updateInOutSourceFinal(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;
            string messageVal = string.Empty;

            try
            {
                //受払ソース（予定）：受払数－（引当数）更新
                sql = "";
                sql = sql + " update inout_source_final ";
                sql = sql + "set ";
                sql = sql + "   inout_qty  = inout_qty - (@InoutQty) ";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutQty = paramInfo.InoutQty,
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId,
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定更新] 受払ソース（受払予定） 受払数の更新に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10153, ComID.MB10134, ComID.MB10142 }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[確定更新] 受払ソース（受払予定） 受払数の更新に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10139, ComID.MB10153, ComID.MB10134, ComID.MB10142 }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 確定取消処理（Insert）
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool addResetInOutSourceFinal(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {

                // 受払ソース（確定）→受払ソース（受払予定）
                sql = "";
                sql = sql + " insert ";
                sql = sql + " into inout_source ( ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " ) ";
                sql = sql + " select  ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " from   ";
                sql = sql + "   inout_source_final   ";
                sql = sql + " where   ";
                sql = sql + "   inout_source_no = @InoutSourceNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定取消処理] 受払ソース（受払予定）登録に失敗しました
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10149, ComID.MB10133, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                // 受払ソース（確定）削除
                sql = "";
                sql = sql + " delete from inout_source_final ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定取消処理]受払ソース（確定）情報の削除に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10146, ComID.MB10149, ComID.MB10134, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[確定取消処理] 受払ソース（確定）登録に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10149, ComID.MB10133, string.Empty }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 確定取消処理（Merge）
        /// ※既存情報が存在する場合は更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool mergeResetInOutSourceFinal(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {

            string sql = string.Empty;

            try
            {

                // 受払ソース（確定）→受払ソース（受払予定）
                sql = "";
                sql = sql + " insert ";
                sql = sql + " into inout_source( ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " ) ";
                sql = sql + " select  ";
                sql = sql + "  inout_source_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,inout_date ";
                sql = sql + "  ,order_division ";
                sql = sql + "  ,order_no ";
                sql = sql + "  ,order_line_no_1 ";
                sql = sql + "  ,order_line_no_2 ";
                sql = sql + "  ,result_order_division ";
                sql = sql + "  ,result_order_no ";
                sql = sql + "  ,result_order_line_no_1 ";
                sql = sql + "  ,result_order_line_no_2 ";
                sql = sql + "  ,item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inout_qty ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,assign_flg ";
                sql = sql + "  ,input_menu_id ";
                sql = sql + "  ,input_display_id ";
                sql = sql + "  ,input_control_id ";
                sql = sql + "  ,update_menu_id ";
                sql = sql + "  ,update_display_id ";
                sql = sql + "  ,update_control_id ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " from   ";
                sql = sql + "   inout_source_final   ";
                sql = sql + " where   ";
                sql = sql + "   inout_source_no = @InoutSourceNo ";
                sql = sql + " on conflict (inout_source_no) ";
                sql = sql + " do update set ";
                sql = sql + "   inout_qty = inout_source.inout_qty + excluded.inout_qty ";
                sql = sql + "  ,update_menu_id = @InputMenuId ";
                sql = sql + "  ,update_display_id = @InputDisplayId ";
                sql = sql + "  ,update_control_id = @InputControlId ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo,
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定取消処理(Merge)] 受払ソース（受払予定）登録に失敗しました
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10150, ComID.MB10133, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                // 受払ソース（確定）削除
                sql = "";
                sql = sql + " delete from inout_source_final ";
                sql = sql + "where ";
                sql = sql + "    inout_source_no = @InoutSourceNo ";

                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InoutSourceNo = paramInfo.InoutSourceNo
                    });
                if (regFlg < 0)
                {
                    // ログ出力：[確定取消処理]受払ソース（確定）情報の削除に失敗しました。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10146, ComID.MB10149, ComID.MB10134, string.Empty }, paramLanguageId);
                    paramMessage += paramInfo.InoutSourceNo;
                    logger.Error(paramMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：[確定取消処理(Merge] 受払ソース（受払予定）登録に失敗しました。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10143, ComID.MB10150, ComID.MB10133, string.Empty }, paramLanguageId);
                paramMessage += paramInfo.InoutSourceNo;
                logger.Error(paramMessage);
                logger.Error(ex.ToString());
                throw ex;
            }

        }

        #endregion

    }
}
