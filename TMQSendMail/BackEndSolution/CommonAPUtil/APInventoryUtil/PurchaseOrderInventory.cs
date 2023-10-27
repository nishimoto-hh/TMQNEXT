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
    /// 在庫更新：発注用
    /// </summary>
    public class PurchaseOrderInventory
    {
        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>購入依頼受払ソース番号リスト </summary>
        private static List<InvDao.InventoryParameter> requestInfoList;

        /// <summary>購入依頼受払ソース番号リスト(取消) </summary>
        private static List<string> requestInouTSouceNoDelete;

        /// <summary>
        /// 発注画面用（オーダークローズ以外）：在庫更新処理
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
            returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10083, ComID.MB10091 }, paramLanguage);
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
            requestInfoList = new List<InvDao.InventoryParameter>();
            requestInouTSouceNoDelete = new List<string>();
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

                // ---　受払ソース更新　----
                if ((paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.ADD)
                    || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE))
                {
                    // 承認・登録の場合
                    retVal = regist(db,  paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }
                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CANCEL
                    || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE_CANCEL))
                {
                    // 承認取消・取消の場合
                    retVal = registCanCel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
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

            // 退避した　購入依頼受払ソースNo単位に確定処理を実施
            // ※登録・承認処理が設定されていた場合（１件でも・・・）
            foreach (var row in requestInfoList)
            {
                //購入依頼情報を受払ソース（確定）に登録
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_ADD, row, strLanguage, ref returnMessage);
                if (!retVal)
                {
                    return new InvDao.InventoryReturnInfo(false, returnMessage);
                }
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
                || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE)
                || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE_CANCEL)))
            {
                //@1行目の処理区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10102 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 受払区分(値チェック)
            if (paramInfo.InoutDivision != ComConst.INOUT_DIVISION.ORDER_REGIST)
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

            // 伝票行番号１(0以外)
            if (paramInfo.ResultOrderLineNo1 == 0)
            {
                //@1行目の伝票行番号１を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10112 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 伝票行番号２(0以外)
            if (paramInfo.ResultOrderLineNo2 == 0)
            {
                //@1行目の伝票行番号２を設定してください。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10113 }, paramLanguageId);
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
        /// 購入依頼受払ソース番号取得（受払ソース（受払予定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getInouTSouceNoRequest(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {
            bool retVal = true;

            //購入依頼：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = InvCom.GetInoutNo(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.OrderDivision,
                paramInfo.OrderNo, paramInfo.OrderLineNo1, paramInfo.OrderLineNo2,
                ref paramInOutSourceNo);
            if (!retVal)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 購入依頼受払ソース番号取得（受払ソース（確定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getFinalInouTSouceNoRequest(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {
            bool retVal = true;

            //購入依頼：受払ソース番号取得　対象：受払ソース（確定）
            retVal = InvCom.GetInoutNoFinal(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.OrderDivision,
                paramInfo.OrderNo, paramInfo.OrderLineNo1, paramInfo.OrderLineNo2,
                ref paramInOutSourceNo);
            if (!retVal)
            {
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
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getInouTSouceNoPurchaseOrder(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {
            bool retVal = true;

            //発注：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = InvCom.GetInoutNo(db, paramLanguageId, paramInfo.OrderDivision, paramInfo.OrderNo,
                paramInfo.OrderLineNo1, paramInfo.OrderLineNo2, paramInfo.ResultOrderDivision,
                paramInfo.ResultOrderNo, paramInfo.ResultOrderLineNo1, paramInfo.ResultOrderLineNo2,
                ref paramInOutSourceNo);
            if (!retVal)
            {
                return false;
            }

            return true;

        }
        #endregion

        #region 登録処理
        /// <summary>
        /// 承認・登録　(処理区分=0:通常データ)
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

            // 購入依頼：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNoRequest(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 購入依頼：受払ソース番号が取得時
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                //購入依頼情報：複写
                InvDao.InventoryParameter requestInfo = new InvDao.InventoryParameter(paramInfo);
                requestInfo.ResultOrderDivision = paramInfo.OrderDivision;
                requestInfo.ResultOrderNo = paramInfo.OrderNo;
                requestInfo.ResultOrderLineNo1 = paramInfo.OrderLineNo1;
                requestInfo.ResultOrderLineNo2 = paramInfo.OrderLineNo2;
                requestInfo.LocationCd = string.Empty;
                requestInfo.LotNo = "*";
                requestInfo.SubLotNo1 = "*";
                requestInfo.SubLotNo2 = "*";

                requestInfo.InoutSourceNo = inOutSourceNo;

                // 受払ソース（受払予定）受払数を更新
                //  *計算後の値が０未満の場合は0にする
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_PURCHASE_SUBTRACTION, requestInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

                // 購入依頼受払ソースNo退避(オーダー番号の受払予定数：購入依頼数＝０の場合)
                //  *購入依頼数　<= 発注数の場合
                if (requestInfo.PlanInoutQty <= 0)
                {
                    InvDao.InventoryParameter result = requestInfoList.Find(n => n.InoutSourceNo == inOutSourceNo);
                    if (result == null)
                    {
                        requestInfoList.Add(requestInfo);
                    }
                }
            }

            // 発注情報の受払ソース（受払予定）登録
            retVal = InvInout.InOutSourceRegist(db, InvInout.INOUT_MAKE_FLG.ADD, paramInfo, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 承認取消・取消
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
            string inOutSourceNoRequest = string.Empty;
            string inOutSourceNoFinal = string.Empty;
            string inOutSourceNoOrder = string.Empty;
            string message = string.Empty;

            // --- 取消・承認取消の場合 [ 確定情報 → 受払予定情報 ]
            // 購入依頼：受払ソース番号取得　対象：受払ソース（確定）
            retVal = getFinalInouTSouceNoRequest(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNoFinal, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            //受払ソース番号が存在する場合
            if (!string.IsNullOrWhiteSpace(inOutSourceNoFinal))
            {
                //退避リストに存在しない場合のみ　予定情報登録
                if (!requestInouTSouceNoDelete.Contains(inOutSourceNoFinal))
                {

                    //購入依頼情報：複写
                    InvDao.InventoryParameter requestFinalInfo = new InvDao.InventoryParameter(paramInfo);
                    requestFinalInfo.ResultOrderDivision = paramInfo.OrderDivision;
                    requestFinalInfo.ResultOrderNo = paramInfo.OrderNo;
                    requestFinalInfo.ResultOrderLineNo1 = paramInfo.OrderLineNo1;
                    requestFinalInfo.ResultOrderLineNo2 = paramInfo.OrderLineNo2;
                    requestFinalInfo.LocationCd = string.Empty;
                    requestFinalInfo.LotNo = "*";
                    requestFinalInfo.SubLotNo1 = "*";
                    requestFinalInfo.SubLotNo2 = "*";
                    requestFinalInfo.InoutSourceNo = inOutSourceNoFinal;

                    //購入依頼情報を受払ソース（予定）に登録
                    retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_DELETE, requestFinalInfo, paramLanguageId, ref paramMessage);
                    if (!retVal)
                    {
                        return false;
                    }

                    // 退避情報追加
                    requestInouTSouceNoDelete.Add(inOutSourceNoFinal);
                }

            }

            // --- 購入依頼情報の受払数更新
            // 購入依頼：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNoRequest(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNoRequest, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            //購入依頼：受払ソース番号（受払予定）が存在する場合
            if (!string.IsNullOrWhiteSpace(inOutSourceNoRequest))
            {

                //購入依頼情報：複写
                InvDao.InventoryParameter requestInfo = new InvDao.InventoryParameter(paramInfo);
                requestInfo.ResultOrderDivision = paramInfo.OrderDivision;
                requestInfo.ResultOrderNo = paramInfo.OrderNo;
                requestInfo.ResultOrderLineNo1 = paramInfo.OrderLineNo1;
                requestInfo.ResultOrderLineNo2 = paramInfo.OrderLineNo2;
                requestInfo.LocationCd = string.Empty;
                requestInfo.LotNo = "*";
                requestInfo.SubLotNo1 = "*";
                requestInfo.SubLotNo2 = "*";
                requestInfo.InoutSourceNo = inOutSourceNoRequest;

                // 受払ソース（受払予定）取消　（受払数更新）
                //  *計算後の受払数　＞　購入依頼数よの場合　購入依頼数にする
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_PURCHASE_ADDITION, requestInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            // --- 発注情報の受払数更新
            // 発注：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNoPurchaseOrder(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNoOrder, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(inOutSourceNoOrder))
            {
                paramInfo.InoutSourceNo = inOutSourceNoOrder;
                // 発注情報の受払ソース(受払予定)を削除
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.DELETE, paramInfo, paramLanguageId, ref paramMessage);
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
