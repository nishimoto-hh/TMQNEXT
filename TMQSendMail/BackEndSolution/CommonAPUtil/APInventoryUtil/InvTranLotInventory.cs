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
    /// 在庫更新：ロット在庫更新
    /// </summary>
    public class InvTranLotInventory
    {

        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// ロット在庫更新
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払ソースパラメータ</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool LotInventoryRegist(ComDB db, InvDao.InventoryParameter paramInfo, string paramLanguageId, ref string paramMessage)
        {
            string sql = string.Empty;
            string messageVal = string.Empty;

            // ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(InvConst.LOG_NAME);

            try
            {

                // 数量＝０の場合は更新しない
                if (paramInfo.InoutQty == 0)
                {
                    return true;
                }

                // ロット在庫登録
                sql = "";
                sql = sql + " insert ";
                sql = sql + " into lot_inventory ( ";
                sql = sql + "  item_cd ";
                sql = sql + "  ,specification_cd ";
                sql = sql + "  ,location_cd ";
                sql = sql + "  ,lot_no ";
                sql = sql + "  ,sub_lot_no_1 ";
                sql = sql + "  ,sub_lot_no_2 ";
                sql = sql + "  ,inventory_qty ";
                sql = sql + "  ,input_date ";
                sql = sql + "  ,input_user_id ";
                sql = sql + "  ,update_date ";
                sql = sql + "  ,update_user_id ";
                sql = sql + " ) ";
                sql = sql + " values ( ";
                sql = sql + "    @ItemCd ";
                sql = sql + "  , @SpecificationCd ";
                sql = sql + "  , @LocationCd ";
                sql = sql + "  , @LotNo ";
                sql = sql + "  , @SubLotNo1 ";
                sql = sql + "  , @SubLotNo2 ";
                sql = sql + "  , (@InoutQty) ";
                sql = sql + "  , CURRENT_TIMESTAMP   ";
                sql = sql + "  , @UserId ";
                sql = sql + "  , CURRENT_TIMESTAMP  ";
                sql = sql + "  , @UserId ";
                sql = sql + " ) ";
                sql = sql + " on conflict (item_cd,specification_cd,location_cd,lot_no,sub_lot_no_1,sub_lot_no_2) ";
                sql = sql + " do update set ";
                sql = sql + "   inventory_qty = lot_inventory.inventory_qty + (@InoutQty) ";
                sql = sql + "  ,update_date = now() ";
                sql = sql + "  ,update_user_id = @UserId ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        ItemCd = paramInfo.ItemCd,
                        SpecificationCd = paramInfo.SpecificationCd,
                        LocationCd = paramInfo.LocationCd,
                        LotNo = paramInfo.LotNo,
                        SubLotNo1 = paramInfo.SubLotNo1,
                        SubLotNo2 = paramInfo.SubLotNo2,
                        InoutQty = paramInfo.InoutQty,
                        UserId = paramInfo.UserId
                    });
                if (regFlg < 0)
                {
                    // ログ出力： ロット在庫の更新に失敗しました
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10144, ComID.MB10152 }, paramLanguageId);
                    logger.Error(paramMessage);
                    logger.Error(sql);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                // ログ出力：ロット在庫の更新に失敗しました。。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10144, ComID.MB10152 }, paramLanguageId);
                logger.Error(paramMessage + "【LotInventoryRegist】:" + ex.ToString());
                throw ex;
            }

        }
    }
}
