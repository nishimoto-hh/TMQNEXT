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
    /// 在庫更新：購買_受入検収用
    /// </summary>
    public class AcceptanceCheckInventory
    {
        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>受入受払ソース番号リスト(取消) </summary>
        private static List<string> acceptanceInouTSouceNoDelete;

        /// <summary>
        /// 受入画面用（オーダークローズ以外）：在庫更新処理
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
            //"受注：在庫更新処理"
            returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10083, ComID.MB10094 }, paramLanguage);
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
            acceptanceInouTSouceNoDelete = new List<string>();
            foreach (var paramInfo in paramListInOutParam)
            {

                // 在庫管理区分判定
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
                    // 在庫更新対象外
                    continue;
                }

                // ---　受払ソース更新・受払履歴登録・ロット在庫更新　----
                if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.ADD)
                {
                    // 登録の場合
                    retVal = regist(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }
                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CANCEL)
                {
                    // 取消の場合
                    retVal = registCanCel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }
                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.DIFFERENCE)
                {
                    // 完納時：差分登録の場合
                    retVal = differenceRegist(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.DIFFERENCE_CANCEL)
                {
                    // 完納時：差分取消登録の場合
                    retVal = differencefRegistCanCel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
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
            if (!((paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.ADD)
                || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CANCEL)
                || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.DIFFERENCE)
                || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.DIFFERENCE_CANCEL)))
            {
                //@1行目の処理区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10102 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 受払区分(値チェック)
            if (!((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.PURCHASE_ACCEPT_CHECK)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.PURCHASE_ACCEPT_CHECK_CANCEL)))
            {
                //@行目の受払区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10103 }, paramLanguageId);
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

            // オーダー行番号１(0以外)
            if (paramInfo.OrderLineNo1 == 0)
            {
                //@1行目の/オーダー行番号１を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10108 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // オーダー行番号２(0以外)
            if (paramInfo.OrderLineNo2 == 0)
            {
                //@1行目の/オーダー行番号２を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10109 }, paramLanguageId);
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
                //@1行目のロット番号を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10122 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // サブロット番号１(必須)
            if (string.IsNullOrWhiteSpace(paramInfo.SubLotNo1))
            {
                //@1行目のサブロット番号１を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10175 }, paramLanguageId);
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

        /// <summary>
        /// 発注受払ソース番号取得（受払ソース（受払予定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramQty">数量</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getInouTSouceInfo(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref decimal paramQty, ref string paramMessage)
        {

            //発注：受払ソース番号取得　対象：受払ソース（受払予定）
            ComDao.InoutSourceEntity inoutSourceInfo = null;
            if (!InvCom.GetInoutOrderInfo(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, ref inoutSourceInfo))
            {
                return false;
            }

            paramInOutSourceNo = string.Empty;
            if (inoutSourceInfo != null)
            {
                paramInOutSourceNo = inoutSourceInfo.InoutSourceNo;
                paramQty = inoutSourceInfo.InoutQty ?? 0;
            }

            return true;

        }

        /// <summary>
        /// 受入受払ソース番号取得（受払ソース（受払予定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getInouTSouceNoAcceptance(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {
            //受入：受払ソース番号取得　対象：受払ソース（受払予定）
            return InvCom.GetInoutNo(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.ResultOrderDivision,
                paramInfo.ResultOrderNo, paramInfo.ResultOrderLineNo1, paramInfo.ResultOrderLineNo2,
                ref paramInOutSourceNo);
        }

        /// <summary>
        /// 発注受払ソース番号取得（受払ソース（確定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getFinalInouTSouceNoPurchaseOrder(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {
            //発注：受払ソース番号取得　対象：受払ソース（確定）
            return  InvCom.GetInoutNoOrderInfoFinal(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2,  ref paramInOutSourceNo);
        }

        /// <summary>
        /// 受入受払ソース番号取得（受払ソース（確定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getFinalInouTSouceNoAcceptance(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {
            //受入：受払ソース番号取得　対象：受払ソース（受払予定）
            return  InvCom.GetInoutNoFinal(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.ResultOrderDivision,
                paramInfo.ResultOrderNo, paramInfo.ResultOrderLineNo1, paramInfo.ResultOrderLineNo2,
                ref paramInOutSourceNo);
        }
        #endregion

        #region 登録処理
        /// <summary>
        /// 登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool regist(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 受入：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNoAcceptance(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10178, ComID.MB10094 }, paramLanguageId);
                paramMessage = paramMessage + " inout_source_no:" + inOutSourceNo;
                return false;
            }

            // 受入情報の受払ソース（受払予定）受払数を更新
            paramInfo.InoutSourceNo = inOutSourceNo;
            retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

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

            // 完納時（完納フラグ＝１）
            if (paramInfo.CompleteFlg == 1)
            {
                //受入情報を受払ソース（確定）に登録
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_ADD, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // 完全完納出ない場合はここで終了
                if (!paramInfo.ExtendedString1.Equals("1"))
                {
                    return true;
                }

                // ---< 発注情報 >---
                // 発注：受払ソース番号取得　対象：受払ソース（受払予定）
                decimal qty = 0;
                inOutSourceNo = string.Empty;
                retVal = getInouTSouceInfo(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref qty, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                //発注受払ソース番号が設定　かつ　受払数=０
                if ((!string.IsNullOrWhiteSpace(inOutSourceNo)) && (qty == 0))
                {
                    //発注情報：複写
                    InvDao.InventoryParameter orderInfo = new InvDao.InventoryParameter(paramInfo);
                    orderInfo.ResultOrderDivision = paramInfo.OrderDivision;
                    orderInfo.InoutSourceNo = inOutSourceNo;

                    //発注情報を受払ソース（確定）に登録
                    retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_ADD_ZERO, orderInfo, paramLanguageId, ref paramMessage);
                    if (!retVal)
                    {
                        return false;
                    }
                }

            }

            return true;

        }

        /// <summary>
        /// 取消
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
            string inOutSourceNoFinal = string.Empty;
            string inOutSourceNoOrder = string.Empty;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            //完納取消時
            if (paramInfo.CompleteFlg == 1)
            {
                // 完全完納でなくなった場合
                if (paramInfo.ExtendedString1.Equals("1"))
                {
                    // ---< 発注情報 >---
                    // --- 受払ソース（確定）より受払ソース番号を取得
                    retVal = getFinalInouTSouceNoPurchaseOrder(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNoOrder, ref paramMessage);
                    if (!retVal)
                    {
                        return false;
                    }

                    // --- 取消の場合 [ 確定情報 → 受払予定情報 ]
                    if (!string.IsNullOrWhiteSpace(inOutSourceNoOrder))
                    {

                        //発注情報：複写
                        InvDao.InventoryParameter orderInfo = new InvDao.InventoryParameter(paramInfo);
                        orderInfo.InoutSourceNo = inOutSourceNoOrder;

                        //発注情報を受払ソース（予定）に登録
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_DELETE, orderInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                    }
                }

                // ---< 受入情報 >---
                // --- 取消の場合 [ 確定情報 → 受払予定情報 ]
                // 受入：受払ソース番号取得　対象：受払ソース（確定）
                retVal = getFinalInouTSouceNoAcceptance(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNoFinal, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                //　受払ソース番号が存在する場合
                paramInfo.InoutSourceNo = inOutSourceNoFinal;
                if (!string.IsNullOrWhiteSpace(inOutSourceNoFinal))
                {
                    //退避リストに存在しない場合のみ　予定情報登録
                    if (!acceptanceInouTSouceNoDelete.Contains(inOutSourceNoFinal))
                    {

                        //受入情報を受払ソース（予定）に登録
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_DELETE, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                        // 退避情報追加
                        acceptanceInouTSouceNoDelete.Add(inOutSourceNoFinal);
                    }

                }
            }
            else
            {
                // --- 受入情報の受払数更新
                // 受入：受払ソース番号取得　対象：受払ソース（受払予定）
                retVal = getInouTSouceNoAcceptance(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                //　受入：受払ソース番号（受払予定）が存在する場合
                paramInfo.InoutSourceNo = inOutSourceNo;
                if (!string.IsNullOrWhiteSpace(inOutSourceNo))
                {

                    // 受払ソース（受払予定）取消　（受払数更新）
                    retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                    if (!retVal)
                    {
                        return false;
                    }
                }
                else
                {
                    //受払ソース番号が取得できない場合はエラー
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10178, ComID.MB10094 }, paramLanguageId);
                    paramMessage = paramMessage + " inout_source_no:" + inOutSourceNo;
                    return false;
                }

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

            return true;

        }

        #endregion

        #region 差分登録処理
        /// <summary>
        /// 差分登録　(処理区分=70:差分データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paeamIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool differenceRegist(ComDB db, InvDao.InventoryParameter paramInfo, int paeamIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            decimal qty = 0;

            string message = string.Empty;

            // -- 発注の受払ソース番号取得
            //発注情報：複写
            InvDao.InventoryParameter orderInfo = new InvDao.InventoryParameter(paramInfo);

            // 受払ソース（受払予定）より受払ソース番号取得
            retVal = getInouTSouceInfo(db, paramInfo, paeamIdx, paramLanguageId, ref inOutSourceNo, ref qty, ref paramMessage);
            if (!retVal)
            {
                return false;
            }
            orderInfo.InoutSourceNo = inOutSourceNo;

            // 存在しない場合は　受払ソース（確定）より　受払ソース番号取得
            if (string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                // 受払ソース（確定）より受払ソース番号取得
                retVal = getFinalInouTSouceNoPurchaseOrder(db, paramInfo, paeamIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                //　--- [確定情報 → 受払予定情報]
                if (!string.IsNullOrWhiteSpace(inOutSourceNo))
                {

                    orderInfo.InoutSourceNo = inOutSourceNo;

                    //発注情報を受払ソース（予定）に登録
                    retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_DELETE, orderInfo, paramLanguageId, ref paramMessage);
                    if (!retVal)
                    {
                        return false;
                    }

                }
            }

            //　発注：受払ソース番号が存在する場合
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {

                // 受払ソース（受払予定）差分登録　（受払数更新）
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, orderInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// 差分登録取消
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool differencefRegistCanCel(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;
            decimal qty = 0;

            // ---< 発注情報 >---

            //発注情報：複写
            InvDao.InventoryParameter orderInfo = new InvDao.InventoryParameter(paramInfo);

            // --- 発注：受払ソース（受払予定）より受払ソース番号取得
            retVal = getInouTSouceInfo(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref qty, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                orderInfo.InoutSourceNo = inOutSourceNo;

                // 受払ソース（受払予定）差分取消　（受払数更新）
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, orderInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                //// 受払ソース番号取得時の受払数ーパラメータの受払数=0の場合
                ////  [ 受払予定情報 → 確定情報 ]
                //if ((qty-orderInfo.InoutQty) == 0)
                //{
                //    // 発注情報を受払ソース（予定）に登録
                //    retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_ADD, orderInfo, pLanguageId, ref pMessage);
                //    if (!retVal) {
                //        return false;
                //    }
                //}
            }

            return true;

        }

        #endregion

        #endregion
    }
}
