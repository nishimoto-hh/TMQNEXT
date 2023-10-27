using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonSTDUtil.CommonLogger;

using ComConst = APConstants.APConstants;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComID = CommonAPUtil.APCommonUtil.APResources.ID;
using ComST = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using InvCom = CommonAPUtil.APInventoryUtil.InvCommon;
using InvConst = CommonAPUtil.APInventoryUtil.InventoryConst;
using InvDao = CommonAPUtil.APInventoryUtil.InventoryDataClass;
using InvInout = CommonAPUtil.APInventoryUtil.InvTranInOutSource;
using InvRecord = CommonAPUtil.APInventoryUtil.InvTranInOutRecord;
using LotInv = CommonAPUtil.APInventoryUtil.InvTranLotInventory;

namespace CommonAPUtil.APInventoryUtil
{
    /// <summary>
    /// 在庫更新：棚卸
    /// </summary>
    public class InventoryInput
    {

        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// 棚卸：在庫更新処理
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramListInOutParam">パラメータリスト</param>
        /// <param name="paramLanguage">言語区分</param>
        /// <returns>True:正常 False:エラー</returns>
        public static InvDao.InventoryReturnInfo InventoryRegist(ComDB db,
            List<InvDao.InventoryParameter> paramListInOutParam, string paramLanguage)
        {
            bool retVal = true;
            string returnMessage = string.Empty;
            string strLanguage = string.Empty;
            int inIdx = 0;

            // ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(InvConst.LOG_NAME);
            //"売上：在庫更新処理"
            returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10083, ComID.MB10099 }, paramLanguage);
            logger.Info(returnMessage);
            returnMessage = string.Empty;

            strLanguage = paramLanguage;
            if (paramLanguage.Equals(string.Empty))
            {
                strLanguage = "ja";
            }

            // パラメータチェック
            foreach (var paramInfo in paramListInOutParam)
            {
                // パラメータログ出力
                InvCom.LogOutparamInfo(paramInfo, inIdx);

                // 必須チェック
                retVal = requiredItemCheck(paramInfo, inIdx, strLanguage, ref returnMessage);
                if (!retVal)
                {
                    break;
                }

                // カウントアップ
                inIdx++;
            }

            // チェックエラーの場合は処理中断
            if (!retVal)
            {
                return new InvDao.InventoryReturnInfo(false, returnMessage);
            }

            // ★★メイン処理　処理分岐
            inIdx = 0;
            foreach (var paramInfo in paramListInOutParam)
            {

                // 在庫管理区分判定
                int? stockDivision = null;
                int result = InvCom.CheckStockDivision(db, paramInfo.ItemCd,
                    paramInfo.SpecificationCd, paramInfo.LocationCd, inIdx, strLanguage, ref stockDivision);
                if (result == InvConst.STOCK_DIVISION_RESULT.ERROR)
                {
                    retVal = false;
                    break;
                }

                paramInfo.StockDivision = stockDivision;
                if (result == InvConst.STOCK_DIVISION_RESULT.NOT_APPLICABLE)
                {
                    // 在庫更新対象外
                    continue;
                }

                // 受払履歴登録
                retVal = InvRecord.InOutRecordRegist(db, paramInfo, strLanguage, ref returnMessage);
                if (!retVal)
                {
                    break;
                }

                // ロット在庫更新
                retVal = LotInv.LotInventoryRegist(db, paramInfo, strLanguage, ref returnMessage);
                if (!retVal)
                {
                    break;
                }

                //カウントアップ
                inIdx++;
            }

            // 更新時にエラーが発生した場合は処理中断
            if (!retVal)
            {
                return new InvDao.InventoryReturnInfo(false, returnMessage);
            }

            return new InvDao.InventoryReturnInfo();
        }

        #region Privateメソッド

        #region 共通
        /// <summary>
        /// パラメータチェック
        /// </summary>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool requiredItemCheck(InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {

            // 処理区分(必須)
            if (paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.ADD)
            {
                //@1行目の処理区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10102 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 受払区分(値チェック)
            if (paramInfo.InoutDivision != ComConst.INOUT_DIVISION.INVENTORY && paramInfo.InoutDivision != ComConst.INOUT_DIVISION.INVENTORYROLLBACK)
            {
                //@行目の受払区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10103 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 勘定年月（必須）
            if (string.IsNullOrWhiteSpace(paramInfo.AccountYears))
            {
                //@行目の勘定年月を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10104 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 受払予定日（必須）
            if (paramInfo.InoutDate == null)
            {
                //@1行目の受払予定日を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10105 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            //品目コード（必須）
            if (string.IsNullOrWhiteSpace(paramInfo.ItemCd))
            {
                //@1行目の品目コードを設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10114 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            //仕様コード（必須）
            if (string.IsNullOrWhiteSpace(paramInfo.SpecificationCd))
            {
                //@1行目の仕様コードを設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10115 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // ロケーション(必須)
            if (string.IsNullOrWhiteSpace(paramInfo.LocationCd))
            {
                //@1行目のユーザIDを設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10121 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // ロット番号(必須)
            if (string.IsNullOrWhiteSpace(paramInfo.LotNo))
            {
                //@1行目のユーザIDを設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10122 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 受払数(0以外)
            if (paramInfo.InoutQty == 0)
            {
                //@1行目の受払数を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10116 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // メニュー番号（必須）
            if (string.IsNullOrWhiteSpace(paramInfo.InputMenuId))
            {
                //@1行目のメニュー番号を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10117 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // タブ番号(必須)
            if (paramInfo.InputDisplayId == null)
            {
                //@1行目のタブ番号を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10118 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 操作番号(必須)
            if (paramInfo.InputControlId == null)
            {
                //@1行目の操作番号を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10119 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // ユーザ番号(必須)
            if (string.IsNullOrWhiteSpace(paramInfo.UserId))
            {
                //@1行目のユーザIDを設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10120 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            return true;

        }
        #endregion

        #endregion
    }
}
