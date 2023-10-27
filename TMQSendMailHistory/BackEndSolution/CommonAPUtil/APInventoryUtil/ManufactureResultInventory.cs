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
    /// 製造実績：出荷実績画面用
    /// </summary>
    public class ManufactureResultInventory
    {

        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// 製造実績用：在庫更新処理
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
            //"製造実績：在庫更新処理"
            returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10083, ComID.MB10097 }, paramLanguage);
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

                // 在庫管理区分判定
                int? stockDivision = null;
                int result = InvCom.CheckStockDivision(db, paramInfo.ItemCd,
                    paramInfo.SpecificationCd, paramInfo.LocationCd, inIdx, strLanguage, ref stockDivision);
                if (result == InvConst.STOCK_DIVISION_RESULT.ERROR)
                {
                    returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10123, (inIdx + 1).ToString(), ComID.MB10124 }, strLanguage);
                    retVal = false;
                    break;
                }

                paramInfo.StockDivision = stockDivision;
                if (result == InvConst.STOCK_DIVISION_RESULT.NOT_APPLICABLE)
                {
                    // 在庫更新対象外
                    continue;
                }

                // ---　 受払ソース更新・受払履歴更新・ロット在庫更新　----
                if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_FINISH_REGIST)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_FINISH_REGIST))
               {
                    // 　【部品・仕上】登録　(処理区分=0:通常データ)
                    retVal = regist(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_FINISH_CANCEL)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_FINISH_CANCEL))
                {
                    // 　【部品・仕上】登録　(処理区分=9:取消データ)
                    retVal = registCanCel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE)
                {
                    // 　【部品・仕上】完了　(処理区分=0:通常データ)
                    retVal = complete(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE_CANCEL)
                {
                    // 　【部品・仕上】完了取消
                    retVal = completeCanCel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
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

        /// <summary>
        /// 製造実績用：在庫更新処理 ※製造実績（実績取込）用
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramListInOutParam">パラメータリスト</param>
        /// <param name="paramLanguage">言語区分</param>
        /// <returns>True:正常 False:エラー</returns>
        public static InvDao.InventoryReturnInfo InventoryRegistForRDirection(ComDB db,
            List<InvDao.InventoryParameter> paramListInOutParam, string paramLanguage)
        {
            bool retVal = true;
            string returnMessage = string.Empty;
            string strLanguage = string.Empty;
            int inIdx = 0;

            // ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(InvConst.LOG_NAME);
            //"製造実績：在庫更新処理"
            returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10083, ComID.MB10097 }, paramLanguage);
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

                // 在庫管理区分判定
                int? stockDivision = null;
                int result = InvCom.CheckStockDivision(db, paramInfo.ItemCd,
                    paramInfo.SpecificationCd, paramInfo.LocationCd, inIdx, strLanguage, ref stockDivision);
                if (result == InvConst.STOCK_DIVISION_RESULT.ERROR)
                {
                    returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10123, (inIdx + 1).ToString(), ComID.MB10124 }, strLanguage);
                    retVal = false;
                    break;
                }

                paramInfo.StockDivision = stockDivision;
                if (result == InvConst.STOCK_DIVISION_RESULT.NOT_APPLICABLE)
                {
                    // 在庫更新対象外
                    continue;
                }

                // ---　 受払ソース更新・受払履歴更新・ロット在庫更新　----
                if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_FINISH_REGIST)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_FINISH_REGIST))
                {
                    // 　【部品・仕上】登録　(処理区分=0:通常データ)
                    retVal = registForRDirection(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_FINISH_CANCEL)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_FINISH_CANCEL))
                {
                    // 　【部品・仕上】登録　(処理区分=9:取消データ)
                    retVal = registCanCelForRDirection(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE)
                {
                    // 　【部品・仕上】完了　(処理区分=0:通常データ)
                    retVal = complete(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE_CANCEL)
                {
                    // 　【部品・仕上】完了取消
                    retVal = completeCanCel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
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
            if ((paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.ADD)
                && (paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.CANCEL)
                && (paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE)
                && (paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE_CANCEL))
            {
                //@1行目の処理区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10102 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 登録・登録変更の場合
            if ((paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.ADD)
                || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CANCEL))
            {
                // 受払区分(値チェック)
                if (!((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_FINISH_REGIST)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_FINISH_CANCEL)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_FINISH_REGIST)
                    || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_FINISH_CANCEL)))
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

                // オーダー行番号１※製造指図フォ－ミュラの実績区分
                if (!((paramInfo.OrderLineNo1 == ComConst.DIRECTION.FORMULA.RESULT_DIVISION.PARTS)
                    || (paramInfo.OrderLineNo1 == ComConst.DIRECTION.FORMULA.RESULT_DIVISION.FINISH)))
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

                // 伝票行番号１ ※製造指図フォ－ミュラの実績区分
                if (!((paramInfo.ResultOrderLineNo1 == ComConst.DIRECTION.FORMULA.RESULT_DIVISION.PARTS)
                    || (paramInfo.ResultOrderLineNo1 == ComConst.DIRECTION.FORMULA.RESULT_DIVISION.FINISH)))
                {
                    //@1行目の/オーダー行番号１を設定してください。
                    //@1行目の伝票行番号１を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10112 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 伝票行番号２(0以外)
                if (paramInfo.ResultOrderLineNo2 == 0)
                {
                    //@1行目の伝票行番号２を設定してください。
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

                //// 受払数(0以外)
                //if (paramInfo.InoutQty == 0) {
                //    //@1行目の受払数を設定してください。
                //    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10116 }, paramLanguageId);
                //    logger.Error(paramMessage);
                //    return false;
                //}
            }

            // 完了・完了取消の場合
            if ((paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE)
                || (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.COMPLETE_CANCEL))
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

                // オーダー行番号１※製造指図フォ－ミュラの実績区分
                if (!((paramInfo.OrderLineNo1 == ComConst.DIRECTION.FORMULA.RESULT_DIVISION.PARTS)
                    || (paramInfo.OrderLineNo1 == ComConst.DIRECTION.FORMULA.RESULT_DIVISION.FINISH)))
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

                // 伝票行番号１ ※製造指図フォ－ミュラの実績区分
                if (!((paramInfo.ResultOrderLineNo1 == ComConst.DIRECTION.FORMULA.RESULT_DIVISION.PARTS)
                    || (paramInfo.ResultOrderLineNo1 == ComConst.DIRECTION.FORMULA.RESULT_DIVISION.FINISH)))
                {
                    //@1行目の/オーダー行番号１を設定してください。
                    //@1行目の伝票行番号１を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10112 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 伝票行番号２(0以外)
                if (paramInfo.ResultOrderLineNo2 == 0)
                {
                    //@1行目の伝票行番号２を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10112 }, paramLanguageId);
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

        /// <summary>
        /// 受払ソース番号取得（受払ソース（受払予定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getInouTSouceNo(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {
            bool retVal = true;

            //受注：受払ソース番号取得　対象：受払ソース（受払予定）
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
        /// 受払ソース番号取得（受払ソース（確定））
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramInOutSourceNo">受払ソース番号</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool getFinalInouTSouceNo(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramInOutSourceNo, ref string paramMessage)
        {

            bool retVal = true;

            //受注：受払ソース番号取得　対象：受払ソース（完了）
            paramInOutSourceNo = string.Empty;
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
        #endregion

        #region 【確定 or 承認】処理
        /// <summary>
        /// 【確定 or 承認】登録　(処理区分=0:通常データ)
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

            //受払数=０の場合　処理対象外
            if (paramInfo.InoutQty == 0)
            {
                return true;
            }

            if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST))
            {
                //部品の場合は　払出の為　数量をマイナス
                paramInfo.InoutQty = paramInfo.InoutQty * -1;
            }

            // --- 受払ソース（受払予定）受払数を更新
            // 受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNo(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 受払ソース番号が取得時（実績画面で新規明細追加情報は対象外)
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                paramInfo.InoutSourceNo = inOutSourceNo;
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
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

            return true;

        }

        /// <summary>
        /// 【確定 or 承認】登録　(処理区分=0:通常データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool registForRDirection(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            //受払数=０の場合　処理対象外
            if (paramInfo.InoutQty == 0)
            {
                return true;
            }

            // --- 受払ソース（受払予定）受払数を更新
            // 受払ソース番号取得　対象：受払ソース（受払予定）
            // 指図番号、品目コード、仕様コードが同一の受払ソースを取得
            IList<RDirectionInventoryInfo> inventorySourceList = getInventoryList(db, paramInfo);
            var index = 1;
            decimal? workInoutQty = paramInfo.InoutQty;
            decimal? beforeInoutQty = paramInfo.InoutQty;

            // 同一ロット情報の受払ソースが存在する場合、優先して引当を実施
            foreach (var inventorySource in inventorySourceList)
            {
                // 受払数(入力値)がnullまたは"0"の場合、処理を抜ける
                if (workInoutQty == null || workInoutQty.Equals(decimal.Zero))
                {
                    break;
                }

                // 同一ロット情報でない場合、スキップ
                if (!inventorySource.LotNo.Equals(paramInfo.LotNo) ||
                    !inventorySource.SubLotNo1.Equals(paramInfo.SubLotNo1) ||
                    !inventorySource.SubLotNo2.Equals(paramInfo.SubLotNo2))
                {
                    index++;
                    continue;
                }

                // 受払数(DB値)が0の場合、次の受払ソースを参照
                if (inventorySource.InoutQty.Equals(decimal.Zero))
                {
                    index++;
                    continue;
                }

                if (!index.Equals(inventorySourceList.Count))
                {
                    decimal workCompareInoutQty = inventorySource.InoutQty;
                    if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST) ||
                        (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST))
                    {
                        //部品の場合は　払出の為　数量をマイナス
                        workCompareInoutQty = workCompareInoutQty * -1;
                    }
                    // 最終処理行でない場合
                    if (decimal.Compare(workCompareInoutQty, (decimal)workInoutQty) >= 0)
                    {
                        // 受払数(DB値) >= 受払数(入力値)
                        paramInfo.InoutSourceNo = inventorySource.InoutSourceNo;
                        paramInfo.InoutQty = workInoutQty;              // 受払数
                        if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST) ||
                            (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST))
                        {
                            //部品の場合は　払出の為　数量をマイナス
                            paramInfo.InoutQty = paramInfo.InoutQty * -1;
                        }

                        // 更新処理を実施
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                        // 後続処理をスキップするため、受払数(入力値)をゼロに
                        workInoutQty = decimal.Zero;

                        // 処理を抜ける
                        break;
                    }
                    else
                    {
                        // 受払数(DB値) < 受払数(入力値)
                        workInoutQty = decimal.Subtract((decimal)workInoutQty, workCompareInoutQty);

                        paramInfo.InoutSourceNo = inventorySource.InoutSourceNo; // 受払ソース番号
                        paramInfo.InoutQty = inventorySource.InoutQty;           // 受払数

                        // 更新処理を実施
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                        inventorySource.InoutQty = decimal.Subtract(inventorySource.InoutQty, (decimal)paramInfo.InoutQty);
                    }
                }
                else
                {
                    // 最終処理行の場合
                    decimal tmpInoutQty = (decimal)workInoutQty;
                    paramInfo.InoutSourceNo = inventorySource.InoutSourceNo; // 受払ソース番号
                    // 過剰の場合、「0」になるように更新を実施
                    if (Math.Abs(inventorySource.InoutQty) < tmpInoutQty)
                    {
                        tmpInoutQty = Math.Abs(inventorySource.InoutQty);
                    }
                    paramInfo.InoutQty = tmpInoutQty; // 受払数
                    if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST) ||
                        (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST))
                    {
                        //部品の場合は　払出の為　数量をマイナス
                        paramInfo.InoutQty = paramInfo.InoutQty * -1;
                    }

                    // 更新処理を実施
                    retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                    if (!retVal)
                    {
                        return false;
                    }

                    workInoutQty = decimal.Subtract((decimal)workInoutQty, Math.Abs(tmpInoutQty));
                    inventorySource.InoutQty = decimal.Subtract(inventorySource.InoutQty, (decimal)paramInfo.InoutQty);
                }

                // 加算
                index++;
            }

            index = 1;
            foreach (var inventorySource in inventorySourceList)
            {
                // 受払数(入力値)がnullまたは"0"の場合、処理を抜ける
                if (workInoutQty == null || workInoutQty.Equals(decimal.Zero))
                {
                    break;
                }

                // 受払数(DB値)が0の場合、次の受払ソースを参照
                if (inventorySource.InoutQty.Equals(decimal.Zero))
                {
                    index++;
                    continue;
                }

                if (!index.Equals(inventorySourceList.Count))
                {
                    decimal workCompareInoutQty = inventorySource.InoutQty;
                    if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST) ||
                        (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST))
                    {
                        //部品の場合は　払出の為　数量をマイナス
                        workCompareInoutQty = workCompareInoutQty * -1;
                    }
                    // 最終処理行でない場合
                    if (decimal.Compare(workCompareInoutQty, (decimal)workInoutQty) >= 0)
                    {
                        // 受払数(DB値) >= 受払数(入力値)
                        paramInfo.InoutSourceNo = inventorySource.InoutSourceNo;
                        paramInfo.InoutQty = workInoutQty;              // 受払数
                        if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST) ||
                            (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST))
                        {
                            //部品の場合は　払出の為　数量をマイナス
                            paramInfo.InoutQty = paramInfo.InoutQty * -1;
                        }

                        // 更新処理を実施
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                        // 処理を抜ける
                        break;
                    }
                    else
                    {
                        // 受払数(DB値) < 受払数(入力値)
                        workInoutQty = decimal.Subtract((decimal)workInoutQty, workCompareInoutQty);

                        paramInfo.InoutSourceNo = inventorySource.InoutSourceNo; // 受払ソース番号
                        paramInfo.InoutQty = inventorySource.InoutQty;           // 受払数

                        // 更新処理を実施
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // 最終処理行の場合
                    paramInfo.InoutSourceNo = inventorySource.InoutSourceNo; // 受払ソース番号
                    // 過剰の場合、「0」になるように更新を実施
                    if (Math.Abs(inventorySource.InoutQty) < workInoutQty)
                    {
                        workInoutQty = Math.Abs(inventorySource.InoutQty);
                    }
                    paramInfo.InoutQty = workInoutQty;                       // 受払数
                    if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST) ||
                        (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST))
                    {
                        //部品の場合は　払出の為　数量をマイナス
                        paramInfo.InoutQty = paramInfo.InoutQty * -1;
                    }

                    // 更新処理を実施
                    retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_SUBTRACTION, paramInfo, paramLanguageId, ref paramMessage);
                    if (!retVal)
                    {
                        return false;
                    }
                }

                // 加算
                index++;
            }

            paramInfo.InoutQty = beforeInoutQty;
            if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST))
            {
                //部品の場合は　払出の為　数量をマイナス
                paramInfo.InoutQty = paramInfo.InoutQty * -1;
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

            return true;

        }

        /// <summary>
        /// 【確定 or 承認】登録　(処理区分=9:取消データ)
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

            //受払数=０の場合　処理対象外
            if (paramInfo.InoutQty == 0)
            {
                return true;
            }

            if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL))
            {
                //部品の場合は　払出の為　数量をマイナス
                paramInfo.InoutQty = paramInfo.InoutQty * -1;
            }

            // 出荷指図：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNo(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {
                // 受払ソース（受払予定）受払数を更新
                paramInfo.InoutSourceNo = inOutSourceNo;
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            //部品の場合は　払出の為　数量をプラスに戻す
            //仕上げ品の場合は　数量をマイナス
            paramInfo.InoutQty = paramInfo.InoutQty * -1;

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

            return true;

        }

        /// <summary>
        /// 【確定 or 承認】登録　(処理区分=9:取消データ)
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool registCanCelForRDirection(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            //受払数=０の場合　処理対象外
            if (paramInfo.InoutQty == 0)
            {
                return true;
            }

            var index = 1;
            decimal? workInoutQty = paramInfo.InoutQty;
            decimal? beforeInoutQty = paramInfo.InoutQty;

            // 指図番号、品目コード、仕様コードが同一の受払ソースを取得
            IList<RDirectionInventoryInfo> inventorySourceList = getInventoryList(db, paramInfo).Reverse().ToList();
            decimal sumPlanedQty = decimal.Zero;
            // 予定数の累計を取得
            foreach (var inventorySource in inventorySourceList)
            {
                sumPlanedQty = decimal.Add(sumPlanedQty, inventorySource.Qty);
            }

            // 受払履歴の同一指図・品目・仕様の数量を取得
            decimal sumInoutRecordQty = getInoutRecordInoutQty(db, paramInfo);
            if (Math.Abs(sumInoutRecordQty) > sumPlanedQty)
            {
                workInoutQty = decimal.Subtract((decimal)workInoutQty, decimal.Subtract(Math.Abs(sumInoutRecordQty), sumPlanedQty));
                // 値がマイナスになっている場合、更新不要
                if (workInoutQty < 0)
                {
                    // 受払ソース更新しないので、ゼロに変更
                    workInoutQty = decimal.Zero;
                }
            }

            // 同一ロット情報の受払ソースが存在する場合、優先して引当を実施
            foreach (var inventorySource in inventorySourceList)
            {
                // 受払数(入力値)がnullまたは"0"の場合、処理を抜ける
                if (workInoutQty == null || workInoutQty.Equals(decimal.Zero))
                {
                    break;
                }

                // 同一ロット情報でない場合、スキップ
                if (!inventorySource.LotNo.Equals(paramInfo.LotNo) ||
                    !inventorySource.SubLotNo1.Equals(paramInfo.SubLotNo1) ||
                    !inventorySource.SubLotNo2.Equals(paramInfo.SubLotNo2))
                {
                    index++;
                    continue;
                }

                // 受払数(DB値)が予定数量の場合、次の受払ソースを参照
                if (inventorySource.InoutQty.Equals(inventorySource.Qty))
                {
                    index++;
                    continue;
                }

                // 最終処理行でない場合
                decimal workCompareInoutQty = inventorySource.InoutQty;
                if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL) ||
                    (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL))
                {
                    //部品の場合は　払出の為　数量をマイナス
                    workCompareInoutQty = workCompareInoutQty * -1;
                }
                if (!index.Equals(inventorySourceList.Count))
                {
                    if (decimal.Compare(decimal.Subtract(inventorySource.Qty, workCompareInoutQty), (decimal)workInoutQty) >= 0)
                    {
                        // 数量(予定) - 受払数(DB値) >= 受払数(入力値)
                        paramInfo.InoutSourceNo = inventorySource.InoutSourceNo;
                        paramInfo.InoutQty = workInoutQty;
                        if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL) ||
                            (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL))
                        {
                            //部品の場合は　払出の為　数量をマイナス
                            paramInfo.InoutQty = paramInfo.InoutQty * -1;
                        }

                        // 更新処理を実施
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                        // 後続処理をスキップするため、受払数(入力値)をゼロに
                        workInoutQty = decimal.Zero;

                        // 処理を抜ける
                        break;
                    }
                    else
                    {
                        // 数量(予定) - 受払数(DB値) < 受払数(入力値)
                        // 受払数(入力値) - (数量(予定) - 受払数(DB値))
                        workInoutQty = decimal.Subtract((decimal)workInoutQty, decimal.Subtract(inventorySource.Qty, workCompareInoutQty));

                        paramInfo.InoutSourceNo = inventorySource.InoutSourceNo; // 受払ソース番号
                        paramInfo.InoutQty = decimal.Subtract(inventorySource.Qty, workCompareInoutQty); // 受払数
                        if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL) ||
                            (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL))
                        {
                            //部品の場合は　払出の為　数量をマイナス
                            paramInfo.InoutQty = paramInfo.InoutQty * -1;
                        }

                        // 更新処理を実施
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                        inventorySource.InoutQty = decimal.Add(inventorySource.InoutQty, (decimal)paramInfo.InoutQty);
                    }
                }
                else
                {
                    if (decimal.Compare(decimal.Subtract(inventorySource.Qty, workCompareInoutQty), (decimal)workInoutQty) >= 0)
                    {
                        // 数量(予定) - 受払数(DB値) >= 受払数(入力値)
                        paramInfo.InoutSourceNo = inventorySource.InoutSourceNo;
                        paramInfo.InoutQty = workInoutQty;
                        if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL) ||
                            (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL))
                        {
                            //部品の場合は　払出の為　数量をマイナス
                            paramInfo.InoutQty = paramInfo.InoutQty * -1;
                        }

                        // 更新処理を実施
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                        // 後続処理をスキップするため、受払数(入力値)をゼロに
                        workInoutQty = decimal.Zero;

                        // 処理を抜ける
                        break;
                    }
                    else
                    {
                        // 数量(予定) - 受払数(DB値) < 受払数(入力値)
                        // 受払数(入力値) - (数量(予定) - 受払数(DB値))
                        decimal tmpWorkInoutQty = decimal.Subtract(inventorySource.Qty, workCompareInoutQty); // 受払数
                        workInoutQty = decimal.Subtract((decimal)workInoutQty, tmpWorkInoutQty);

                        // 最終処理行の場合
                        paramInfo.InoutSourceNo = inventorySource.InoutSourceNo; // 受払ソース番号
                        paramInfo.InoutQty = tmpWorkInoutQty;                    // 受払数
                        if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL) ||
                                (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL))
                        {
                            //部品の場合は　払出の為　数量をマイナス
                            paramInfo.InoutQty = paramInfo.InoutQty * -1;
                        }

                        // 更新処理を実施
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                        inventorySource.InoutQty = decimal.Add(inventorySource.InoutQty, (decimal)paramInfo.InoutQty);
                    }
                }

                // 加算
                index++;
            }

            index = 1; // 初期化
            foreach (var inventorySource in inventorySourceList)
            {
                // 受払数(入力値)がnullまたは"0"の場合、処理を抜ける
                if (workInoutQty == null || workInoutQty.Equals(decimal.Zero))
                {
                    break;
                }

                // 受払数(DB値)が予定数量の場合、次の受払ソースを参照
                if (inventorySource.InoutQty.Equals(inventorySource.Qty))
                {
                    index++;
                    continue;
                }

                if (!index.Equals(inventorySourceList.Count))
                {
                    // 最終処理行でない場合
                    decimal workCompareInoutQty = inventorySource.InoutQty;
                    if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL) ||
                        (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL))
                    {
                        //部品の場合は　払出の為　数量をマイナス
                        workCompareInoutQty = workCompareInoutQty * -1;
                    }

                    if (decimal.Compare(decimal.Subtract(inventorySource.Qty, workCompareInoutQty), (decimal)workInoutQty) >= 0)
                    {
                        // 数量(予定) - 受払数(DB値) >= 受払数(入力値)
                        paramInfo.InoutSourceNo = inventorySource.InoutSourceNo;
                        paramInfo.InoutQty = workInoutQty;
                        if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL) ||
                            (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL))
                        {
                            //部品の場合は　払出の為　数量をマイナス
                            paramInfo.InoutQty = paramInfo.InoutQty * -1;
                        }

                        // 更新処理を実施
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                        // 処理を抜ける
                        break;
                    }
                    else
                    {
                        // 数量(予定) - 受払数(DB値) < 受払数(入力値)
                        // 受払数(入力値) - (数量(予定) - 受払数(DB値))
                        workInoutQty = decimal.Subtract((decimal)workInoutQty, decimal.Subtract(inventorySource.Qty, workCompareInoutQty));

                        paramInfo.InoutSourceNo = inventorySource.InoutSourceNo; // 受払ソース番号
                        paramInfo.InoutQty = decimal.Subtract(inventorySource.Qty, workCompareInoutQty); // 受払数
                        if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL) ||
                            (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL))
                        {
                            //部品の場合は　払出の為　数量をマイナス
                            paramInfo.InoutQty = paramInfo.InoutQty * -1;
                        }

                        // 更新処理を実施
                        retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                        if (!retVal)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // 最終処理行の場合
                    paramInfo.InoutSourceNo = inventorySource.InoutSourceNo; // 受払ソース番号
                    paramInfo.InoutQty = workInoutQty;                       // 受払数
                    if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_CANCEL) ||
                            (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_CANCEL))
                    {
                        //部品の場合は　払出の為　数量をマイナス
                        paramInfo.InoutQty = paramInfo.InoutQty * -1;
                    }

                    // 更新処理を実施
                    retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.UPDATE_QTY_ONLY_ADDITION, paramInfo, paramLanguageId, ref paramMessage);
                    if (!retVal)
                    {
                        return false;
                    }
                }

                // 加算
                index++;
            }

            paramInfo.InoutQty = beforeInoutQty;
            //仕上げ品の場合は　数量をマイナス
            if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_FINISH_CANCEL) ||
                (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_FINISH_CANCEL))
            {
                paramInfo.InoutQty = paramInfo.InoutQty * -1;
            }

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

            return true;

        }

        /// <summary>
        /// 【完了】完了　(処理区分=0:通常データ)
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

            //受払数=０の場合　処理対象外
            //if (paramInfo.InoutQty == 0) {
            //    return true;
            //}

            // 出荷指図：受払ソース番号取得　対象：受払ソース（受払予定）
            retVal = getInouTSouceNo(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 受払ソース番号が取得時（実績画面で新規明細追加情報は対象外)
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {

                // 完了時：受払ソース（予定）→受払ソース（完了）
                paramInfo.InoutSourceNo = inOutSourceNo;
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_ADD, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// 【完了】完了取消
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

            //受払数=０の場合　処理対象外
            //if (paramInfo.InoutQty == 0) {
            //    return true;
            //}

            // 出荷指図：受払ソース番号取得　対象：受払ソース（完了）
            retVal = getFinalInouTSouceNo(db, paramInfo, paramIdx, paramLanguageId, ref inOutSourceNo, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 受払ソース番号が取得時（実績画面で新規明細追加情報は対象外)
            if (!string.IsNullOrWhiteSpace(inOutSourceNo))
            {

                // 受払ソース（完了）確定取消処理　（（受払ソース（受払予定）復活）
                paramInfo.InoutSourceNo = inOutSourceNo;
                retVal = InvInout.InOutSourceRegist(db, InvTranInOutSource.INOUT_MAKE_FLG.FINAL_DELETE, paramInfo, paramLanguageId, ref paramMessage);
                if (!retVal)
                {
                    return false;
                }

            }

            return true;

        }

        /// <summary>
        /// 受払ソースリスト取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <returns>受払ソースリスト</returns>
        private static IList<RDirectionInventoryInfo> getInventoryList(ComDB db, InvDao.InventoryParameter paramInfo)
        {
            // SQL文生成
            string sql = string.Empty;
            sql += "select inout_source.inout_source_no, "; // 受払ソース番号
            sql += "       inout_source.inout_qty, ";       // 受払数
            sql += "       inout_source.lot_no, ";          // ロット番号
            sql += "       inout_source.sub_lot_no_1, ";    // サブロット番号１
            sql += "       inout_source.sub_lot_no_2, ";    // サブロット番号２
            sql += "       formula.qty ";                   // 数量(予定)
            sql += "  from inout_source ";
            sql += "       inner join direction_formula formula on ";
            sql += "           inout_source.order_division = formula.direction_division ";
            sql += "       and inout_source.order_no = formula.direction_no ";
            sql += "       and inout_source.order_line_no_1 = formula.result_division ";
            sql += "       and inout_source.order_line_no_2 = formula.seq_no ";
            sql += " where inout_source.order_no = @OrderNo ";
            sql += "   and inout_source.item_cd = @ItemCd ";
            sql += "   and inout_source.specification_cd = @SpecificationCd ";
            sql += "   and inout_source.inout_source_no is not null ";
            sql += "   and formula.result_division = @ResultDivision ";
            // ソート
            if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_RESULT_PARTS_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_IMPORT_PARTS_REGIST))
            {
                // 部品の場合、ロット番号・サブロット１・サブロット２の昇順でソートを実施
                sql += "order by inout_source.lot_no, inout_source.sub_lot_no_1, inout_source.sub_lot_no_2, inout_source.inout_qty desc";
            }
            else
            {
                // 仕上の場合、仕上予定日・ロット番号・サブロット１・サブロット２の昇順でソートを実施
                sql += "order by formula.scheduled_accounting_date, inout_source.lot_no, inout_source.sub_lot_no_1, inout_source.sub_lot_no_2, inout_source.inout_qty";
            }

            // SQL実行
            IList<RDirectionInventoryInfo> inventorySourceList = db.GetListByDataClass<RDirectionInventoryInfo>(sql,
                new { OrderNo = paramInfo.OrderNo, ItemCd = paramInfo.ItemCd, SpecificationCd = paramInfo.SpecificationCd, ResultDivision = paramInfo.ResultOrderLineNo1 });

            return inventorySourceList;
        }

        /// <summary>
        /// 製造実績在庫更新用クラス
        /// </summary>
        private class RDirectionInventoryInfo
        {
            /// <summary>Gets or sets 受払ソース番号</summary>
            /// <value>受払ソース番号</value>
            public string InoutSourceNo { get; set; }
            /// <summary>Gets or sets 受払数</summary>
            /// <value>受払数</value>
            public decimal InoutQty { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号１</summary>
            /// <value>サブロット番号１</value>
            public string SubLotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号２</summary>
            /// <value>サブロット番号２</value>
            public string SubLotNo2 { get; set; }
            /// <summary>Gets or sets 数量(予定)</summary>
            /// <value>数量(予定)</value>
            public decimal Qty { get; set; }
        }

        /// <summary>
        /// 受払ソースリスト取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <returns>受払ソースリスト</returns>
        private static decimal getInoutRecordInoutQty(ComDB db, InvDao.InventoryParameter paramInfo)
        {
            // SQL文生成
            string sql = string.Empty;
            sql += "select sum(inout_qty) ";
            sql += "  from inout_record ";
            sql += " where inout_record.inout_division like 'MAN%' ";
            sql += "   and inout_record.order_division = @OrderDivision ";
            sql += "   and inout_record.order_no = @OrderNo ";
            sql += "   and inout_record.result_order_division = @OrderDivision ";
            sql += "   and inout_record.result_order_no = @OrderNo ";
            sql += "   and inout_record.item_cd = @ItemCd ";
            sql += "   and inout_record.specification_cd = @SpecificationCd ";
            sql += "   and inout_record.order_line_no_1 = @OrderLineNo1 ";

            // SQL実行
            decimal? sumInourQty = db.GetEntity<decimal?>(sql,
                new { OrderDivision = paramInfo.OrderDivision, OrderNo = paramInfo.OrderNo, ItemCd = paramInfo.ItemCd, SpecificationCd = paramInfo.SpecificationCd, OrderLineNo1 = paramInfo.ResultOrderLineNo1 });

            return sumInourQty ?? decimal.Zero;
        }
        #endregion

        #endregion
    }
}
