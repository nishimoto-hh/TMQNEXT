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

namespace CommonAPUtil.APInventoryUtil
{
    /// <summary>
    /// 在庫更新：受注オーダークローズ
    /// </summary>
    public class OrderCloseInventory
    {
        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// 受注画面オーダークローズ用：在庫更新処理
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

            //ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(InvConst.LOG_NAME);
            //"受注クローズ：在庫更新処理"
            returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10083, ComID.MB10085 }, paramLanguage);
            logger.Info(returnMessage);
            returnMessage = string.Empty;

            strLanguage = paramLanguage;
            if (paramLanguage.Equals(string.Empty))
            {
                strLanguage = "ja";
            }

            //パラメータチェック
            foreach (var paramInfo in paramListInOutParam)
            {
                //パラメータログ出力
                InvCom.LogOutparamInfo(paramInfo, inIdx);

                //必須チェック
                retVal = requiredItemCheck(paramInfo, inIdx, strLanguage, ref returnMessage);
                if (!retVal)
                {
                    break;
                }

                //カウントアップ
                inIdx++;
            }

            //チェックエラーの場合は処理中断
            if (!retVal)
            {
                return new InvDao.InventoryReturnInfo(false, returnMessage);
            }

            //★★メイン処理　処理分岐
            inIdx = 0;
            foreach (var paramInfo in paramListInOutParam)
            {

                if (!string.IsNullOrWhiteSpace(paramInfo.ItemCd))
                {
                    //--　在庫管理区分判定--
                    int? stockDivision = null;
                    int result = InvCom.CheckStockDivision(db, paramInfo.ItemCd,
                        paramInfo.SpecificationCd, paramInfo.LocationCd, inIdx, strLanguage, ref stockDivision);
                    if (result == InvConst.STOCK_DIVISION_RESULT.ERROR)
                    {
                        break;
                    }

                    paramInfo.StockDivision = stockDivision;
                    if (result == InvConst.STOCK_DIVISION_RESULT.NOT_APPLICABLE)
                    {
                        //在庫更新対象外
                        continue;
                    }
                }

                //---　受払ソース更新　----
                if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CLOSE)
                {
                    //オーダークローズの場合(行番号単位)

                    //受払ソース番号取得
                    string inOutSourceNo = string.Empty;
                    retVal = InvCom.GetInoutNo(db, strLanguage, paramInfo.OrderDivision, paramInfo.OrderNo,
                        paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.ResultOrderDivision,
                        paramInfo.ResultOrderNo, paramInfo.ResultOrderLineNo1, paramInfo.ResultOrderLineNo2,
                        ref inOutSourceNo);

                    if (!retVal)
                    {
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(inOutSourceNo))
                    {
                        paramInfo.InoutSourceNo = inOutSourceNo;

                        //確定処理
                        retVal = InvInout.InOutSourceRegist(db, InvInout.INOUT_MAKE_FLG.FINAL_ADD, paramInfo, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }
                    }
                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CLOSE_CANCEL)
                {
                    //オーダークローズ取消の場合(行番号単位)

                    //受払ソース番号取得(確定情報)
                    string inOutSourceNo = string.Empty;
                    retVal = InvCom.GetInoutNoFinal(db, strLanguage, paramInfo.OrderDivision, paramInfo.OrderNo,
                        paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.ResultOrderDivision,
                        paramInfo.ResultOrderNo, paramInfo.ResultOrderLineNo1, paramInfo.ResultOrderLineNo2,
                        ref inOutSourceNo);

                    if (!retVal)
                    {
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(inOutSourceNo))
                    {
                        paramInfo.InoutSourceNo = inOutSourceNo;

                        //確定取消
                        retVal = InvInout.InOutSourceRegist(db, InvInout.INOUT_MAKE_FLG.FINAL_DELETE, paramInfo, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }
                    }
                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CLOSE_ORDER)
                {
                    //確定処理(オーダー番号単位)
                    retVal = InvInout.InOutSourceRegist(db, InvInout.INOUT_MAKE_FLG.FINAL_ADD_ORDER, paramInfo, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }
                }

                //カウントアップ
                inIdx++;
            }

            //受払ソース更新時にエラーが発生した場合は処理中断
            if (!retVal)
            {
                return new InvDao.InventoryReturnInfo(false, returnMessage);
            }

            return new InvDao.InventoryReturnInfo();
        }

        #region Privateメソッド

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

            //処理区分(必須)
            if (!((paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CLOSE)
                || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CLOSE_ORDER)
                || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CLOSE_CANCEL)))
            {
                //@1行目の処理区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10102 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 受払区分(値チェック)
            if (paramInfo.InoutDivision != ComConst.INOUT_DIVISION.ORDER_CLOSE)
            {
                //@行目の受払区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10103 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            if ((paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CLOSE)
                || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CLOSE_CANCEL))
            {

                // 【明細単位のオーダークローズ・取消の場合】

                //オーダー区分(0以外)
                if (paramInfo.OrderDivision == 0)
                {
                    //@1行目のオーダー区分を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10106 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                //オーダー番号(0以外)
                if (paramInfo.OrderNo.Equals("0"))
                {
                    //@1行目のオーダー番号を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10107 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                //オーダー行番号１(0以外)
                if (paramInfo.OrderLineNo1 == 0)
                {
                    //@1行目の/オーダー行番号１を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10108 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 伝票区分(0以外)
                if (paramInfo.ResultOrderDivision == 0)
                {
                    //@1行目の伝票区分を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10110 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 伝票番号(0以外)
                if (paramInfo.ResultOrderNo.Equals("0"))
                {
                    //@1行目の伝票番号を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10111 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                //伝票行番号１(0以外)
                if (paramInfo.ResultOrderLineNo1 == 0)
                {
                    //@1行目の伝票行番号１を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10112 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }
            }
            else
            {
                // 【オーダー番号単位のオーダークローズの場合】
                //オーダー区分(0以外)
                if (paramInfo.OrderDivision == 0)
                {
                    //@1行目のオーダー区分を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10106 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                //オーダー番号(0以外)
                if (paramInfo.OrderNo.Equals("0"))
                {
                    //@1行目のオーダー番号を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10107 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 伝票区分(0以外)
                if (paramInfo.ResultOrderDivision == 0)
                {
                    //@1行目の伝票区分を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10110 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 伝票番号(0以外)
                if (paramInfo.ResultOrderNo.Equals("0"))
                {
                    //@1行目の伝票番号を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10111 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }
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
    }
}
