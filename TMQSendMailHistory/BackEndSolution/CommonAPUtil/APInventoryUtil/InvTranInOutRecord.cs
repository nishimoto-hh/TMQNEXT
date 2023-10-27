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
    /// 在庫更新：受払履歴登録
    /// </summary>
    public class InvTranInOutRecord
    {
        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// 受払履歴登録
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">ﾒｯｾｰｼﾞ</param>
        /// <returns>true:正常　false:エラー</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool InOutRecordRegist(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {
            string sql = string.Empty;

            // ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(InvConst.LOG_NAME);

            try
            {

                // 数量＝０の場合は更新しない
                if (paramInfo.InoutQty == 0)
                {
                    return true;
                }

                //受払履歴番号採番
                string inoutNo =
                    CommonAPUtil.APStoredPrcUtil.APStoredPrcUtil.ProSeqGetNo(db, ComConst.SEQUENCE_PROC_NAME.INOUT_RECORD_NO);

                //受払ソース（予定）新規登録
                sql = "";
                sql = sql + " insert ";
                sql = sql + " into inout_record( ";
                sql = sql + "  inout_no ";
                sql = sql + "  ,inout_division ";
                sql = sql + "  ,account_years ";
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
                sql = sql + "  ,inout_price ";
                sql = sql + "  ,inout_cost ";
                sql = sql + "  ,reference_no ";
                sql = sql + "  ,remark ";
                sql = sql + "  ,ry_cd ";
                sql = sql + "  ,reason ";
                sql = sql + "  ,inout_update_date ";
                sql = sql + "  ,inventory_update_date ";
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
                sql = sql + "    @InoutNo ";
                sql = sql + "  , @InoutDivision ";
                sql = sql + "  , @AccountYears ";
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
                sql = sql + "  , 0 ";
                sql = sql + "  , 0 ";
                // ReferenceNo
                if (string.IsNullOrWhiteSpace(paramInfo.ReferenceNo))
                {
                    sql = sql + "  , null ";
                }
                else
                {
                    sql = sql + "  , @ReferenceNo";
                }
                // Remark
                if (string.IsNullOrWhiteSpace(paramInfo.Remark))
                {
                    sql = sql + "  , null ";
                }
                else
                {
                    sql = sql + "  , @Remark";
                }
                // RyCd
                if (string.IsNullOrWhiteSpace(paramInfo.RyCd))
                {
                    sql = sql + "  , '' ";
                }
                else
                {
                    sql = sql + "  , @RyCd";
                }
                // Reason
                if (string.IsNullOrWhiteSpace(paramInfo.Reason))
                {
                    sql = sql + "  , null ";
                }
                else
                {
                    sql = sql + "  , @Reason";
                }
                sql = sql + "  , null ";
                sql = sql + "  , null ";
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
                        InoutNo = inoutNo,
                        InoutDivision = paramInfo.InoutDivision,
                        AccountYears = paramInfo.AccountYears,
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
                        Remark = paramInfo.Remark,
                        RyCd = paramInfo.RyCd,
                        Reason = paramInfo.Reason,
                        InputMenuId = paramInfo.InputMenuId,
                        InputDisplayId = paramInfo.InputDisplayId,
                        InputControlId = paramInfo.InputControlId,
                        UserId = paramInfo.UserId
                    });
                if (regFlg < 0)
                {
                    // 受払履歴の登録に失敗しました
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10145, ComID.MB10151}, paramLanguageId);
                    logger.Error(paramMessage);
                    logger.Error(sql);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                // ログ出力：受払履歴の登録に失敗しました
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10145, ComID.MB10151}, paramLanguageId);
                logger.Error(paramMessage + "【InOutRecordRegist】:" + ex.ToString());
                throw ex;
            }

        }

    }
}
