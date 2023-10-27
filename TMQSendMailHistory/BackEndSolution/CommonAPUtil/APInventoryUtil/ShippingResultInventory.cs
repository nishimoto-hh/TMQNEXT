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
    /// 在庫更新：出荷実績画面用
    /// </summary>
    public class ShippingResultInventory
    {

        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// 出荷実績画面用：在庫更新処理
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
            //"出荷実績画面用：在庫更新処理"
            returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10083, ComID.MB10088}, paramLanguage);
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

                //カウントアップ
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

                // ---　 受払ソース更新・受払履歴更新・ロット在庫更新　----
                if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE)
                {
                    // 　【出荷基準 or 預り在庫　or 検収基準】一括完了　(処理区分=80:完了)
                    //     オーダー番号単位に登録
                    // 完了時：受払ソース（予定）→受払ソース（確定）
                    retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_ADD_ORDER, paramInfo, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }
                }
                else
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

                    // 受払数はマイナス：(受払数*-1）
                    paramInfo.InoutQty = paramInfo.InoutQty * -1;

                    if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING_STANDARD_REGIST)
                        || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_REGIST))
                    {
                        // 　【出荷基準 or 預り在庫】登録　(処理区分=0:通常データ)
                        retVal = regist(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }

                    }
                    else if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING_STANDARD_CANCEL)
                        || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_CANCEL))
                    {
                        // 　【出荷基準 or 預り在庫】登録　(処理区分=9:取消データ)
                        retVal = registCanCel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }

                    }
                    else if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING_STANDARD_COMPLETED)
                        || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_COMPLETED))
                    {
                        // 　【出荷基準 or 預り在庫】完了　(処理区分=0:通常データ)
                        retVal = complete(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }

                    }
                    else if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING_STANDARD_BEFORE_COMPLETE_CANCEL)
                        || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_BEFORE_COMPLETE_CANCEL))
                    {
                        // 　【出荷基準 or 預り在庫】完了　(処理区分=9:取消データ)
                        retVal = completeBeforeCancel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }

                    }
                    else if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING_STANDARD_COMPLETE_CANCEL)
                        || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_COMPLETE_CANCEL))
                    {
                        // 　【出荷基準 or 預り在庫】完了取消
                        retVal = completeCanCel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }

                    }
                    else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_REGIST)
                    {
                        // 　【検収基準】登録　(処理区分=0:通常データ)
                        retVal = acceptanceRegist(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }

                    }
                    else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_CANCEL)
                    {
                        // 　【検収基準】登録　(処理区分=9:取消データ)
                        retVal = acceptanceRegistCanCel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }

                    }
                    else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_COMPLETED)
                    {
                        // 　【検収基準】完了　(処理区分=0:通常データ)
                        retVal = acceptanceComplete(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }

                    }
                    else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_BEFORE_COMPLETE_CANCEL)
                    {
                        // 　【検収基準】完了　(処理区分=9:取消データ)
                        retVal = acceptanceCompleteBeforeCancel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }
                    }
                    else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_COMPLETE_CANCEL)
                    {
                        // 　【検収基準】完了取消
                        retVal = acceptanceCompleteCanCel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                        if (!retVal)
                        {
                            break;
                        }

                    }
                }
                //カウントアップ
                inIdx++;
            }

            // 受払ソース更新時にエラーが発生した場合は処理中断
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
            if ((paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.ADD)
                && (paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.CANCEL)
                && (paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE))
            {
                //@1行目の処理区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10102 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 受払区分(値チェック)
            if (!((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING_STANDARD_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING_STANDARD_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING_STANDARD_COMPLETED)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING_STANDARD_BEFORE_COMPLETE_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING_STANDARD_COMPLETE_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_COMPLETED)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_BEFORE_COMPLETE_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_COMPLETE_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_COMPLETED)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_BEFORE_COMPLETE_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_COMPLETE_CANCEL)))
            {
                //@行目の受払区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10103 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            //預り在庫以外のチェック
            if (!((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_COMPLETED)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_BEFORE_COMPLETE_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_COMPLETE_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_COMPLETED)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_BEFORE_COMPLETE_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_CUSTODY_COMPLETE_CANCEL)))
            {

                // オーダー区分(0以外)
                if (paramInfo.OrderDivision == 0)
                {
                    //@1行目のオーダー区分を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10106 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // オーダー番号(0以外)
                if (paramInfo.OrderNo.Equals("0"))
                {
                    //@1行目のオーダー番号を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10107 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                //一括完了以外の場合チェック
                if (paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE)
                {
                    // オーダー行番号１(0以外)
                    if (paramInfo.OrderLineNo1 == 0)
                    {
                        //@1行目の/オーダー行番号１を設定してください。
                        paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10108 }, paramLanguageId);
                        logger.Error(paramMessage);
                        return false;
                    }
                }

            }

            // 伝票番号(0以外)
            if (paramInfo.ResultOrderNo.Equals("0"))
            {
                //@1行目の伝票番号を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10111 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            //一括完了以外の場合チェック
            if (paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE)
            {
                // 伝票行番号１(0以外)
                if (paramInfo.ResultOrderLineNo1 == 0)
                {
                    //@1行目の伝票行番号１を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10112 }, paramLanguageId);
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
                    //@1行目のロケーションを設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10121 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // ロット番号(必須)
                if (string.IsNullOrWhiteSpace(paramInfo.LotNo))
                {
                    //@1行目のロット版g脳を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10122 }, paramLanguageId);
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

            // 検収基準の場合のみチェック
            if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_COMPLETED)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_BEFORE_COMPLETE_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.ACCEPTANCE_CRITERIA_COMPLETE_CANCEL))
            {
                // 変更前ロケーション(必須)
                if (string.IsNullOrWhiteSpace(paramInfo.BeforeLocationCd))
                {
                    //@1行目のロケーションを設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), "[BeforeLocationCd]" }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// 受注受払ソース番号取得（受払ソース（受払予定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getInouTSouceNoOrder(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {

            //受注：受払ソース番号取得　対象：受払ソース（受払予定）
            return InvCom.GetInoutNo(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.OrderDivision,
                paramInfo.OrderNo, paramInfo.OrderLineNo1, paramInfo.OrderLineNo2,
                ref paramInOutSourceNo);

        }

        /// <summary>
        /// 出荷指図受払ソース番号取得（受払ソース（確定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getFinalInouTSouceNoOrder(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {

            //受注：受払ソース番号取得　対象：受払ソース（確定）
            paramInOutSourceNo = string.Empty;
            return InvCom.GetInoutNoFinal(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.OrderDivision,
                paramInfo.OrderNo, paramInfo.OrderLineNo1, paramInfo.OrderLineNo2,
                ref paramInOutSourceNo);
        }

        /// <summary>
        /// 出荷指図受払ソース番号取得（受払ソース（受払予定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getInouTSouceNoShipping(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {

            //出荷指図：受払ソース番号取得　対象：受払ソース（受払予定）
            paramInOutSourceNo = string.Empty;
            return InvCom.GetInoutNo(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.ResultOrderDivision,
                paramInfo.ResultOrderNo, paramInfo.ResultOrderLineNo1, paramInfo.ResultOrderLineNo2,
                ref paramInOutSourceNo);
        }

        /// <summary>
        /// 出荷指図受払ソース番号取得（受払ソース（確定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getFinalInouTSouceNoShipping(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {

            //出荷指図：受払ソース番号取得　対象：受払ソース（確定）
            paramInOutSourceNo = string.Empty;
            return InvCom.GetInoutNoFinal(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.ResultOrderDivision,
                paramInfo.ResultOrderNo, paramInfo.ResultOrderLineNo1, paramInfo.ResultOrderLineNo2,
                ref paramInOutSourceNo);
        }
        #endregion

        #region 【出荷基準 or 預り在庫】処理
        /// <summary>
        /// 【出荷基準 or 預り在庫】登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool regist(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId,  ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 出荷指図：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 受払ソース番号が取得時（実績画面で新規明細追加情報は対象外)
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                // 受払ソース（受払予定）受払数を更新
                paramInfo.InoutSourceNo = inOutSourceNo;
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

            }

            //変更前（移動元ロケーション）が未設定の場合　通常出荷
            // 上記以外は　在庫移動を行う
            if (string.IsNullOrWhiteSpace(paramInfo.BeforeLocationCd))
            {
                // 受払履歴登録
                retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新
                retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }
            else
            {
                //出荷基準　かつ　預り先へ移動する場合

                // ==== 移動元から出庫 ====
                // 移動元ロケーション情報：パラメータ情報設定
                InvDao.InventoryParameter beforeLocationInfo = new InvDao.InventoryParameter(paramInfo);
                beforeLocationInfo.LocationCd = paramInfo.BeforeLocationCd;

                // 受払履歴登録
                retVal = InvRecord.InOutRecordRegist(db, beforeLocationInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新
                retVal = LotInv.LotInventoryRegist(db, beforeLocationInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ==== 預り先へ入庫 ====
                // 受払履歴登録
                paramInfo.InoutQty = paramInfo.InoutQty * -1;
                retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新
                retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// 【出荷基準 or 預り在庫】登録　(処理区分=9:取消データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool registCanCel(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 出荷指図：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                // 受払ソース（受払予定）引当取消：引当取消　（受払数更新）
                paramInfo.InoutSourceNo = inOutSourceNo;
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

            }

            //変更前（移動元ロケーション）が未設定の場合　通常出荷取消
            // 上記以外は　在庫移動取消を行う
            if (string.IsNullOrWhiteSpace(paramInfo.BeforeLocationCd))
            {

                // 受払履歴登録（取消情報）
                paramInfo.InoutQty = paramInfo.InoutQty * -1;
                retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新（取消情報）
                retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

            }
            else
            {
                //出荷基準　かつ　預り先へ移動する場合

                // ==== 移動元へ入庫 ====
                // 移動元ロケーション情報：パラメータ情報設定
                InvDao.InventoryParameter beforeLocationInfo = new InvDao.InventoryParameter(paramInfo);
                beforeLocationInfo.LocationCd = paramInfo.BeforeLocationCd;
                beforeLocationInfo.InoutQty = beforeLocationInfo.InoutQty * -1;

                // 受払履歴登録（取消情報）
                retVal = InvRecord.InOutRecordRegist(db, beforeLocationInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新（取消情報）
                retVal = LotInv.LotInventoryRegist(db, beforeLocationInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ==== 預り先より出庫 ====
                // 受払履歴登録（取消情報）
                retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新（取消情報）
                retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// 【出荷基準 or 預り在庫】完了　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool complete(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 出荷指図：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 受払ソース番号取得時（実績画面で新規明細追加情報は対象外)
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                // 受払ソース（受払予定）受払数を更新
                paramInfo.InoutSourceNo = inOutSourceNo;
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            //変更前（移動元ロケーション）が未設定の場合　通常出荷
            // 上記以外は　在庫移動を行う
            if (string.IsNullOrWhiteSpace(paramInfo.BeforeLocationCd))
            {
                // 受払履歴登録
                retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新
                retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

            }
            else
            {
                //出荷基準　かつ　預り先へ移動する場合

                // ==== 移動元から出庫 ====
                // 移動元ロケーション情報：パラメータ情報設定
                InvDao.InventoryParameter beforeLocationInfo = new InvDao.InventoryParameter(paramInfo);
                beforeLocationInfo.LocationCd = paramInfo.BeforeLocationCd;

                // 受払履歴登録
                retVal = InvRecord.InOutRecordRegist(db, beforeLocationInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新
                retVal = LotInv.LotInventoryRegist(db, beforeLocationInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ==== 預り先へ入庫 ====
                // 受払履歴登録
                paramInfo.InoutQty = paramInfo.InoutQty * -1;
                retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新
                retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// 【出荷基準 or 預り在庫】完了　(処理区分=9:取消データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool completeBeforeCancel(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string inOutSourceNoFinal = string.Empty;
            string message = string.Empty;

            // -- 出荷指図：受払ソース番号取得
            // 対象：受払ソース（受払予定）
            retVal = getInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 対象：受払ソース（確定）
            retVal = getFinalInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNoFinal, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(inOutSourceNo) && (!string.IsNullOrWhiteSpace(inOutSourceNoFinal)))
            {
                // 予定なし・確定ありの場合　＜確定情報 →　予定情報＞
                paramInfo.InoutSourceNo = inOutSourceNoFinal;
                inOutSourceNo = inOutSourceNoFinal;
                // 受払ソース（確定）確定取消処理　（（受払ソース（受払予定）復活）
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_DELETE, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            // 受払ソース（受払予定）の受払数更新
            paramInfo.InoutSourceNo = inOutSourceNo;
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {

                // 受払ソース（受払予定）引当取消：引当取消　（受払数更新）
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            // 変更前（移動元ロケーション）が未設定の場合 通常出荷取消
            // 上記以外は　在庫移動取消を行う
            if (string.IsNullOrWhiteSpace(paramInfo.BeforeLocationCd))
            {

                // 受払履歴登録（取消情報）
                paramInfo.InoutQty = paramInfo.InoutQty * -1;
                retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新（取消情報）
                retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

            }
            else
            {
                //出荷基準　かつ　預り先へ移動する場合

                // ==== 移動元へ入庫 ====
                // 移動元ロケーション情報：パラメータ情報設定
                InvDao.InventoryParameter beforeLocationInfo = new InvDao.InventoryParameter(paramInfo);
                beforeLocationInfo.LocationCd = paramInfo.BeforeLocationCd;
                beforeLocationInfo.InoutQty = beforeLocationInfo.InoutQty * -1;

                // 受払履歴登録（取消情報）
                retVal = InvRecord.InOutRecordRegist(db, beforeLocationInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新（取消情報）
                retVal = LotInv.LotInventoryRegist(db, beforeLocationInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ==== 預り先より出庫 ====
                // 受払履歴登録（取消情報）
                retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // ロット在庫更新（取消情報）
                retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// 【出荷基準 or 預り在庫】完了取消
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool completeCanCel(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 出荷指図：受払ソース番号取得　対象：受払ソース（確定）
            retVal = getFinalInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 受払ソース番号が取得時（実績画面で新規明細追加情報は対象外)
            paramInfo.InoutSourceNo = inOutSourceNo;
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                // 受払ソース（確定）確定取消処理　（（受払ソース（受払予定）復活）
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_DELETE, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            return true;

        }
        #endregion

        #region 【検収基準】処理
        /// <summary>
        /// 【検収基準】登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool acceptanceRegist(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 出荷指図：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 受払ソース番号が取得時（実績画面で新規明細追加情報は対象外)
            paramInfo.InoutSourceNo = inOutSourceNo;
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                // 受払ソース（受払予定）受払数を更新
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            // 帳簿内ロケーション用：パラメータ情報設定
            InvDao.InventoryParameter bookLocationInfo = new InvDao.InventoryParameter(paramInfo);
            bookLocationInfo.LocationCd = paramInfo.BeforeLocationCd;

            // -- 帳簿内ロケーションより出庫情報登録 --
            // 受払履歴登録
            retVal = InvRecord.InOutRecordRegist(db, bookLocationInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // ロット在庫更新
            retVal = LotInv.LotInventoryRegist(db, bookLocationInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // -- 出荷ロケーションへ入庫情報登録 --
            paramInfo.InoutQty = paramInfo.InoutQty * -1;
            // 受払履歴登録
            retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // ロット在庫更新
            retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 【検収基準】登録　(処理区分=9:取消データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool acceptanceRegistCanCel(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 出荷指図：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                // 受払ソース（受払予定）引当取消：引当取消　（受払数更新）
                paramInfo.InoutSourceNo = inOutSourceNo;
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            // 帳簿内ロケーション用：パラメータ情報設定
            InvDao.InventoryParameter bookLocationInfo = new InvDao.InventoryParameter(paramInfo);
            bookLocationInfo.LocationCd = paramInfo.BeforeLocationCd;
            bookLocationInfo.InoutQty = bookLocationInfo.InoutQty * -1;

            // -- 帳簿内ロケーションへ入庫情報登録 --
            // 受払履歴登録
            retVal = InvRecord.InOutRecordRegist(db, bookLocationInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // ロット在庫更新
            retVal = LotInv.LotInventoryRegist(db, bookLocationInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // -- 出荷ロケーションより出庫情報登録 --
            // 受払履歴登録
            retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // ロット在庫更新
            retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 【検収基準】完了　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool acceptanceComplete(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 出荷指図：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 受払ソース番号が取得時（実績画面で新規明細追加情報は対象外)
            paramInfo.InoutSourceNo = inOutSourceNo;
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                // 受払ソース（受払予定）受払数を更新
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

            }

            // 帳簿内ロケーション用：パラメータ情報設定
            InvDao.InventoryParameter bookLocationInfo = new InvDao.InventoryParameter(paramInfo);
            bookLocationInfo.LocationCd = paramInfo.BeforeLocationCd;

            // -- 帳簿内ロケーションより出庫情報登録 --
            // 受払履歴登録
            retVal = InvRecord.InOutRecordRegist(db, bookLocationInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // ロット在庫更新
            retVal = LotInv.LotInventoryRegist(db, bookLocationInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // -- 出荷ロケーションへ入庫情報登録 --
            paramInfo.InoutQty = paramInfo.InoutQty * -1;
            // 受払履歴登録
            retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // ロット在庫更新
            retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 【検収基準】完了　(処理区分=9:取消データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool acceptanceCompleteBeforeCancel(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string inOutSourceNoFinal = string.Empty;
            string message = string.Empty;

            // -- 出荷指図：受払ソース番号取得
            // 対象：受払ソース（受払予定）
            retVal = getInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 対象：受払ソース（確定）
            retVal = getFinalInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNoFinal, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(inOutSourceNo) && (!string.IsNullOrWhiteSpace(inOutSourceNoFinal)))
            {
                // 予定なし・確定ありの場合　＜確定情報 →　予定情報＞
                paramInfo.InoutSourceNo = inOutSourceNoFinal;
                inOutSourceNo = inOutSourceNoFinal;
                // 受払ソース（確定）確定取消処理　（（受払ソース（受払予定）復活）
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_DELETE, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

            }

            // 受払ソース（受払予定）の受払数更新
            paramInfo.InoutSourceNo = inOutSourceNo;
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {

                // 受払ソース（受払予定）引当取消：引当取消　（受払数更新）
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            // 帳簿内ロケーション用：パラメータ情報設定
            InvDao.InventoryParameter bookLocationInfo = new InvDao.InventoryParameter(paramInfo);
            bookLocationInfo.LocationCd = paramInfo.BeforeLocationCd;
            bookLocationInfo.InoutQty = bookLocationInfo.InoutQty * -1;

            // -- 帳簿内ロケーションへ入庫情報登録 --
            // 受払履歴登録
            retVal = InvRecord.InOutRecordRegist(db, bookLocationInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // ロット在庫更新
            retVal = LotInv.LotInventoryRegist(db, bookLocationInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // -- 出荷ロケーションより出庫情報登録 --
            // 受払履歴登録
            retVal = InvRecord.InOutRecordRegist(db, paramInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // ロット在庫更新
            retVal = LotInv.LotInventoryRegist(db, paramInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 【検収基準】完了取消
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool acceptanceCompleteCanCel(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 出荷指図：受払ソース番号取得　対象：受払ソース（確定）
            retVal = getFinalInouTSouceNoShipping(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 受払ソース番号が取得時（実績画面で新規明細追加情報は対象外)
            paramInfo.InoutSourceNo = inOutSourceNo;
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                // 受払ソース（確定）確定取消処理　（（受払ソース（受払予定）復活）
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_DELETE, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

            }

            return true;

        }
        #endregion

        #endregion
    }
}
