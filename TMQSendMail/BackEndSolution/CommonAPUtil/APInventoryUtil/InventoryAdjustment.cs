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
    /// 在庫更新：在庫調整
    /// </summary>
    public class InventoryAdjustment
    {

        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// 在庫調整用：在庫更新処理
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
            returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10083, ComID.MB10098 }, paramLanguage);
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

                // ---　 受払履歴更新・ロット在庫更新　----
                if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_RECEIPT_REGIST)
                {
                    // 　【在庫入庫入力】承認(+)
                    retVal = inventoryAddition(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_RECEIPT_CANCEL)
                {
                    // 　【在庫入庫入力】承認取消(-)
                    retVal = inventorySubtraction(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_DELIVERY_REGIST)
                {
                    // 　【在庫出庫入力】承認(-)
                    retVal = inventorySubtraction(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_DELIVERY_CANCEL)
                {
                    // 　【在庫出庫入力】承認取消(+)
                    retVal = inventoryAddition(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }
                }
                else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_MOVEMENT_REGIST)
                {
                    // 　【在庫移動入力】承認
                    retVal = inventoryMovementRegist(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_MOVEMENT_CANCEL)
                {
                    // 　【在庫移動入力】承認取消
                    retVal = inventoryMovementCancel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_ITEM_TRANSFER_REGIST)
                {
                    // 　【品目振替入力】承認
                    retVal = itemTransferRegist(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_ITEM_TRANSFER_CANCEL)
                {
                    // 　【品目振替入力】承認取消
                    retVal = itemTransferCancel(db, paramInfo, inIdx, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

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
            if ((paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.ADD)
                && (paramInfo.ProcessDivision != InvConst.INVENTORY_PROCESS_DIVISION.CANCEL))
            {
                //@1行目の処理区分が正しくありません。
                paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10100, (paramIdx + 1).ToString(), ComID.MB10102 }, paramLanguageId);
                logger.Error(paramMessage);
                return false;
            }

            // 受払区分(値チェック)
            if (!((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_RECEIPT_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_RECEIPT_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_DELIVERY_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_DELIVERY_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_MOVEMENT_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_MOVEMENT_CANCEL)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_ITEM_TRANSFER_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_ITEM_TRANSFER_CANCEL)))
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

            //// オーダー区分(0以外)
            //if (paramInfo.OrderDivision == 0) {
            //    //@1行目のオーダー区分を設定してください。
            //    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10106 }, paramLanguageId);
            //    logger.Error(paramMessage);
            //    return false;
            //}

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

            // 在庫移動独自
            if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_MOVEMENT_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_MOVEMENT_CANCEL))
            {
                // 変更前ロケーション
                if (string.IsNullOrWhiteSpace(paramInfo.BeforeLocationCd))
                {
                    //@1行目の変更前ロケーションを設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10161}, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }
            }

            // 品目振替独自
            if ((paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_ITEM_TRANSFER_REGIST)
                || (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.INVENTORY_ITEM_TRANSFER_CANCEL))
            {
                // 振替前ロケーション
                if (string.IsNullOrWhiteSpace(paramInfo.BeforeLocationCd))
                {
                    //@1行目の振替前ロケーションを設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10162 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 振替前品目コード
                if (string.IsNullOrWhiteSpace(paramInfo.TransferItemCd))
                {
                    //@1行目の振替前品目コードを設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10163 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 振替前仕様コード
                if (string.IsNullOrWhiteSpace(paramInfo.TransferSpecificationCd))
                {
                    //@1行目の振替前仕様コードを設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10164 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 振替前ロットNo
                if (string.IsNullOrWhiteSpace(paramInfo.TransferLotNo))
                {
                    //@1行目の振替前ロット番号を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10165 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 振替前サブロット番号１
                if (string.IsNullOrWhiteSpace(paramInfo.TransferSubLotNo1))
                {
                    //@1行目の振替前サブロット番号１を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10166 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 振替前サブロット番号２
                if (string.IsNullOrWhiteSpace(paramInfo.TransferSubLotNo2))
                {
                    //@1行目の振替前サブロット番号２を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10167 }, paramLanguageId);
                    logger.Error(paramMessage);
                    return false;
                }

                // 振替受払数(0以外)
                if (paramInfo.InoutQty == 0)
                {
                    //@1行目の受払数を設定してください。
                    paramMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10101, (paramIdx + 1).ToString(), ComID.MB10168 }, paramLanguageId);
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

        #region 【在庫数更新】処理
        /// <summary>
        /// 在庫数加算処理
        ///   ** 在庫入庫入力承認・在庫出庫入力承認取消
        ///   ** 在庫移動入力移動先承認・在庫移動入力移動元承認取消
        ///   ** 品目振替入力振替先承認・品目振替入力振替元承認取消
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool inventoryAddition(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

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
        /// 在庫数減算処理
        ///   ** 在庫入庫入力承認取消・在庫出庫入力承認
        ///   ** 在庫移動入力移動元承認・在庫移動入力移動先承認取消
        ///   ** 品目振替入力振替元承認・品目振替入力振替先承認取消
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool inventorySubtraction(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;

            //受払数マイナス
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

        #endregion

        #region 【在庫移動】処理
        /// <summary>
        /// 在庫移動処理：承認
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool inventoryMovementRegist(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 移動元情報コピー
            InvDao.InventoryParameter beforeInfo = new InvDao.InventoryParameter(paramInfo);
            beforeInfo.LocationCd = paramInfo.BeforeLocationCd;

            // 移動先登録(+)
            retVal =　inventoryAddition(db, paramInfo, paramIdx, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 移動元登録(-)
            retVal = inventorySubtraction(db, beforeInfo, paramIdx, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 在庫移動処理：承認取消
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool inventoryMovementCancel(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;

            // 移動元情報コピー
            InvDao.InventoryParameter beforeInfo = new InvDao.InventoryParameter(paramInfo);
            beforeInfo.LocationCd = paramInfo.BeforeLocationCd;

            // 移動先登録(-)
            retVal = inventorySubtraction(db, paramInfo, paramIdx, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 移動元登録(+)
            retVal = inventoryAddition(db, beforeInfo, paramIdx, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            return true;

        }

        #endregion

        #region 【品目振替】処理
        /// <summary>
        /// 在庫移動処理：承認
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool itemTransferRegist(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;
            string inOutSourceNo = string.Empty;
            string message = string.Empty;

            // 振替元情報コピー
            InvDao.InventoryParameter beforeInfo = new InvDao.InventoryParameter(paramInfo);
            beforeInfo.ItemCd = paramInfo.TransferItemCd;
            beforeInfo.SpecificationCd = paramInfo.TransferSpecificationCd;
            beforeInfo.LocationCd = paramInfo.BeforeLocationCd;
            beforeInfo.LotNo = paramInfo.TransferLotNo;
            beforeInfo.SubLotNo1 = paramInfo.TransferSubLotNo1;
            beforeInfo.SubLotNo2 = paramInfo.TransferSubLotNo2;
            beforeInfo.InoutQty = paramInfo.TransferInoutQty;

            // 振替先登録(+)
            retVal = inventoryAddition(db, paramInfo, paramIdx, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 振替元登録(-)
            retVal = inventorySubtraction(db, beforeInfo, paramIdx, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 品目振替処理：承認取消
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="paramInfo">受払パラメータ</param>
        /// <param name="paramIdx">受払パラメータリストIndex</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramMessage">正常時:空白 エラー時：該当メッセージ</param>
        /// <returns>true:正常 false：エラー</returns>
        private static bool itemTransferCancel(ComDB db, InvDao.InventoryParameter paramInfo, int paramIdx,
            string paramLanguageId, ref string paramMessage)
        {
            bool retVal = true;

            // 振替元情報コピー
            InvDao.InventoryParameter beforeInfo = new InvDao.InventoryParameter(paramInfo);
            beforeInfo.ItemCd = paramInfo.TransferItemCd;
            beforeInfo.SpecificationCd = paramInfo.TransferSpecificationCd;
            beforeInfo.LocationCd = paramInfo.BeforeLocationCd;
            beforeInfo.LotNo = paramInfo.TransferLotNo;
            beforeInfo.SubLotNo1 = paramInfo.TransferSubLotNo1;
            beforeInfo.SubLotNo2 = paramInfo.TransferSubLotNo2;
            beforeInfo.InoutQty = paramInfo.TransferInoutQty;

            // 振替先登録(-)
            retVal = inventorySubtraction(db, paramInfo, paramIdx, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            // 振替元登録(+)
            retVal = inventoryAddition(db, beforeInfo, paramIdx, paramLanguageId, ref paramMessage);
            if (!retVal)
            {
                return false;
            }

            return true;

        }

        #endregion

        #endregion
    }
}
