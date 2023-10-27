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
    /// 在庫更新：製造指図用
    /// </summary>
    public class ManufactureOrderInventory
    {

        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// 製造指図用：在庫更新処理
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
            //"製造指図：在庫更新処理
            returnMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10083, ComID.MB10096 }, paramLanguage);
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

                // ---　受払ソース更新　----
                if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.ADD)
                {
                    // 確定・承認の場合

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

                    // 部品の場合：払出しの為　マイナス
                    if (paramInfo.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_APPROVAL_PARTS)
                    {
                        paramInfo.InoutQty = paramInfo.InoutQty * -1;
                    }

                    // 受払ソース登録
                    retVal = InvInout.InOutSourceRegist(db, InvInout.INOUT_MAKE_FLG.ADD, paramInfo, strLanguage, ref returnMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                else if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CANCEL)
                {
                    // 確定取消・承認取消の場合　指図番号単位に受払ソースを削除する
                    retVal = InvInout.InOutSourceRegist(db, InvInout.INOUT_MAKE_FLG.DELETE_ORDER, paramInfo, strLanguage, ref returnMessage);
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

            // 確定・承認時
            if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.ADD)
            {
                // 受払区分(値チェック)
                if ((paramInfo.InoutDivision != ComConst.INOUT_DIVISION.DIRECTION_APPROVAL_PARTS)
                    && (paramInfo.InoutDivision != ComConst.INOUT_DIVISION.DIRECTION_APPROVAL_FINISH))
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

                // オーダー行番号１(0以外)※製造指図フォ－ミュラの実績区分
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

                // 伝票行番号１(0以外) ※製造指図フォ－ミュラの実績区分
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
            }

            // 確定取消・承認取消時
            if (paramInfo.ProcessDivision == InvConst.INVENTORY_PROCESS_DIVISION.CANCEL)
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

        #region 呼び出し対象機能向け　引数リスト作成クラス
        /// <summary>パラメータリスト作成クラス</summary>
        /// <remark>多機能より呼び出すので、共通化のためにパラメータ取得クラスを作成</remark>
        public class GetParameterClass
        {
            /// <summary>SQL名：在庫更新処理のパラメータ取得</summary>
            private const string GetInventoryRegistParam = "ManDir_GetInventoryRegistParam";
            /// <summary>SQL格納先サブディレクトリ名</summary>
            private const string SubDir = "ManufacturingDirection\\common";

            /// <summary>Gets or sets DB接続</summary>
            /// <value>DB接続</value>
            private ComDB db { get; set; }
            /// <summary>メニュー番号</summary>
            private string inputMenuId;
            /// <summary>タブ番号</summary>
            private string inputDisplayId;
            /// <summary>操作番号</summary>
            private int? inputControlId;
            /// <summary>ユーザID</summary>
            private string userId;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="inDb">DB接続</param>
            /// <param name="inInputMenuId">メニュー番号に設定する値</param>
            /// <param name="inInputDisplayId">タブ番号に設定する値</param>
            /// <param name="inInputControlId">操作番号に設定する値</param>
            /// <param name="inUserId">ユーザIDに設定する値</param>
            public GetParameterClass(ComDB inDb, string inInputMenuId, string inInputDisplayId, int? inInputControlId, string inUserId)
            {
                // 引数の値を設定
                this.db = inDb;
                this.inputMenuId = inInputMenuId;
                this.inputDisplayId = inInputDisplayId;
                this.inputControlId = inInputControlId;
                this.userId = inUserId;
            }

            /// <summary>
            /// 確定処理のパラメータリストを取得する処理
            /// </summary>
            /// <param name="inDirectionDivision">指図区分</param>
            /// <param name="inDirectionNo">指図番号</param>
            /// <returns>確定処理のパラメータリスト</returns>
            public List<InvDao.InventoryParameter> GetConfirmParamList(int inDirectionDivision, string inDirectionNo)
            {
                // 設定するパラメータリスト
                List<InvDao.InventoryParameter> paramList = new List<InvDao.InventoryParameter>();
                // 指図区分、番号より、パラメータに設定する内容を取得
                IList<ResultGetInventoryRegistParam> formulaList = getFormulaListForParam(inDirectionDivision, inDirectionNo);

                foreach (var formula in formulaList)
                {
                    var param = new InvDao.InventoryParameter();
                    param.ProcessDivision = InvConst.INVENTORY_PROCESS_DIVISION.ADD;
                    // フォーミュラの実績区分より部品かどうかを判定
                    bool isParts = formula.ResultDivision == ComConst.DIRECTION.FORMULA.RESULT_DIVISION.PARTS;
                    param.InoutDivision = isParts ? ComConst.INOUT_DIVISION.DIRECTION_APPROVAL_PARTS : ComConst.INOUT_DIVISION.DIRECTION_APPROVAL_FINISH;
                    param.InoutDate = formula.InoutDate ?? formula.PlanedEndDate;
                    param.OrderDivision = formula.DirectionDivision;
                    param.OrderNo = formula.DirectionNo;
                    param.OrderLineNo1 = formula.ResultDivision;
                    param.OrderLineNo2 = formula.SeqNo;
                    param.ResultOrderDivision = formula.DirectionDivision;
                    param.ResultOrderNo = formula.DirectionNo;
                    param.ResultOrderLineNo1 = formula.ResultDivision;
                    param.ResultOrderLineNo2 = formula.SeqNo;
                    param.ItemCd = formula.ItemCd;
                    param.SpecificationCd = formula.SpecificationCd;
                    param.LocationCd = formula.LocationCd;
                    param.LotNo = formula.LotNo;
                    param.SubLotNo1 = formula.SubLotNo1;
                    param.SubLotNo2 = formula.SubLotNo2;
                    param.InoutQty = formula.Qty;
                    // 共通の値を設定
                    setMemberValue(ref param);
                    // リストに追加
                    paramList.Add(param);
                }

                return paramList;
            }

            /// <summary>
            /// 確定取消処理のパラメータリストを取得する処理
            /// </summary>
            /// <param name="inDirectionDivision">指図区分</param>
            /// <param name="inDirectionNo">指図番号</param>
            /// <returns>確定取消処理のパラメータリスト(1件)</returns>
            public List<InvDao.InventoryParameter> GetConfirmCancelParamList(int inDirectionDivision, string inDirectionNo)
            {
                // 指図区分、番号より、パラメータに設定する内容を取得
                IList<ResultGetInventoryRegistParam> formulaList = getFormulaListForParam(inDirectionDivision, inDirectionNo);

                var param = new InvDao.InventoryParameter();
                param.ProcessDivision = InvConst.INVENTORY_PROCESS_DIVISION.CANCEL;
                // 完了取消の場合は、ヘッダ単位なので先頭を取得
                var formula = formulaList[0];
                param.InoutDate = formula.PlanedEndDate;
                param.OrderDivision = formula.DirectionDivision;
                param.OrderNo = formula.DirectionNo;
                param.ResultOrderDivision = formula.DirectionDivision;
                param.ResultOrderNo = formula.DirectionNo;
                // 共通の値を設定
                setMemberValue(ref param);

                return new List<InvDao.InventoryParameter> { param };
            }

            /// <summary>
            /// パラメータに設定する内容を製造指図テーブルより取得する処理
            /// </summary>
            /// <param name="inDirectionDivision">指図区分</param>
            /// <param name="inDirerctionNo">指図番号</param>
            /// <returns>製造指図より取得した、パラメータに設定する内容のリスト</returns>
            private IList<ResultGetInventoryRegistParam> getFormulaListForParam(int inDirectionDivision, string inDirerctionNo)
            {
                // フォーミュラの内容を取得
                ComDao.DirectionHeaderEntity condFormula = new ComDao.DirectionHeaderEntity();
                condFormula.DirectionDivision = inDirectionDivision;
                condFormula.DirectionNo = inDirerctionNo;
                var formulaList = this.db.GetListByOutsideSqlByDataClass<ResultGetInventoryRegistParam>(GetInventoryRegistParam, SubDir, condFormula);
                return formulaList;
            }

            /// <summary>
            /// 確定と確定取消で共通に設定する、メンバの項目を設定する処理
            /// </summary>
            /// <param name="param">ref 設定を行うデータクラス</param>
            private void setMemberValue(ref InvDao.InventoryParameter param)
            {
                param.InputMenuId = this.inputMenuId;
                param.InputDisplayId = this.inputDisplayId;
                param.InputControlId = this.inputControlId;
                param.UserId = this.userId;
            }

            /// <summary>
            /// 製造指図テーブルより取得した、パラメータに設定する内容のデータクラス
            /// </summary>
            private class ResultGetInventoryRegistParam
            {
                // 製造指図ヘッダより取得
                /// <summary>Gets or sets 終了予定日時</summary>
                /// <value>終了予定日時</value>
                public DateTime PlanedEndDate { get; set; }

                // 製造指図フォーミュラより取得
                /// <summary>Gets or sets 指図区分</summary>
                /// <value>指図区分</value>
                public int DirectionDivision { get; set; }
                /// <summary>Gets or sets 指図番号</summary>
                /// <value>指図番号</value>
                public string DirectionNo { get; set; }
                /// <summary>Gets or sets 実績区分</summary>
                /// <value>実績区分</value>
                public int ResultDivision { get; set; }
                /// <summary>Gets or sets シーケンスNO</summary>
                /// <value>シーケンスNO</value>
                public int SeqNo { get; set; }
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets ロケーションコード</summary>
                /// <value>ロケーションコード</value>
                public string LocationCd { get; set; }
                /// <summary>Gets or sets ロット番号</summary>
                /// <value>ロット番号</value>
                public string LotNo { get; set; }
                /// <summary>Gets or sets サブロット番号1</summary>
                /// <value>サブロット番号1</value>
                public string SubLotNo1 { get; set; }
                /// <summary>Gets or sets サブロット番号2</summary>
                /// <value>サブロット番号2</value>
                public string SubLotNo2 { get; set; }
                /// <summary>Gets or sets 数量</summary>
                /// <value>数量</value>
                public decimal? Qty { get; set; }
                /// <summary>Gets or sets 受払日</summary>
                /// <value>受払日</value>
                public DateTime? InoutDate { get; set; }
            }
        }
        #endregion
    }
}
