using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using APComUtil = APCommonUtil.APCommonUtil.APCommonUtil;
using APConsts = APConstants.APConstants;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>
    /// 受注クローズ処理
    /// </summary>
    /// <remarks>現行プロシージャ「PRO_ORDER_CLOSE」</remarks>
    public class ProOrderClose
    {
        /// <summary>
        /// SQLファイルの定義
        /// </summary>
        public static class SqlName
        {
            /// <summary>SQL格納サブディレクトリ</summary>
            public const string SubDir = "Order\\proOrderClose";
            /// <summary>SQL名：受注残取得SQL</summary>
            public const string GetOrderRemainQty = "ProOrderClose_GetOrderRemainQty";
            /// <summary>SQL名：受注に紐づく出荷指図データのステータス取得SQL</summary>
            public const string GetShippingStatus = "ProOrderClose_GetShippingStatus";
            /// <summary>SQL名：受注残取得SQL</summary>
            public const string UpdateOrderUnClose = "ProOrderClose_OrderUnClose";
            /// <summary>SQL名：受注残取得SQL</summary>
            public const string UpdateOrderClose = "ProOrderClose_OrderClose";
            /// <summary>SQL名：受注プルーフ登録</summary>
            public const string OrderProof = "Order_Head_Proof";
        }

        /// <summary>
        /// 受注クローズ処理実行
        /// </summary>
        /// <param name="orderNo">対象の受注番号</param>
        /// <param name="db">DB接続</param>
        /// <param name="userId">更新ユーザID</param>
        /// <returns>エラーの場合False</returns>
        public static bool Execute(string orderNo, ComDB db, string userId)
        {
            DateTime now = DateTime.Now;
            // 受注ヘッダを取得
            ComDao.OrderHeadEntity order = new ComDao.OrderHeadEntity().GetEntity(orderNo, db);
            if (order == null)
            {
                // 存在しない場合終了
                return true;
            }
            // 受注の各行番号の受注残を取得
            IList<OrderQtyCheckResult> orderRowQtyList = db.GetListByOutsideSqlByDataClass<OrderQtyCheckResult>(SqlName.GetOrderRemainQty, SqlName.SubDir, order);
            if (orderRowQtyList == null || orderRowQtyList.Count == 0)
            {
                // 存在しない場合終了
                return true;
            }
            // クローズ許可フラグ
            // 受注残が0より大きい受注行がある場合、クローズ不可
            bool closeFlg = orderRowQtyList.Count(x => x.Qty > 0) == 0;

            // 受注に紐づく出荷指図ステータスを取得し、出荷完了以外が存在する場合、クローズ不可
            IList<int> shippingStatusList = db.GetListByOutsideSqlByDataClass<int>(SqlName.GetShippingStatus, SqlName.SubDir, order);
            if (shippingStatusList != null && shippingStatusList.Count > 1)
            {
                // 複数存在する場合、出荷完了以外があるのでクローズ不可
                closeFlg = false;
            }
            else if (shippingStatusList != null && shippingStatusList.Count == 1 && !shippingStatusList[0].Equals(APConsts.SHIPPING.HEAD.SHIPPING_STATUS.SHIPPING_COMPLERTE))
            {
                // 1件の場合に出荷完了以外の場合は、クローズ不可
                closeFlg = false;
            }

            // 更新SQL
            string updateSql;
            // 更新条件
            ComDao.OrderHeadEntity updateCond = new ComDao.OrderHeadEntity();

            if (closeFlg && order.Status != APConsts.ORDER_STATUS.ORDER_CLOSE)
            {
                // クローズ可能な場合、ステータスがクローズ以外ならクローズする
                // TODO 受注キャンセルもクローズしてよい？
                updateSql = SqlName.UpdateOrderClose;
                updateCond.Status = APConsts.ORDER_STATUS.ORDER_CLOSE;
                updateCond.LastStatus = order.Status;
                updateCond.OrderCloseDate = now;
            }
            else if (!closeFlg && order.Status == APConsts.ORDER_STATUS.ORDER_CLOSE)
            {
                // クローズ不可能で現在のステータスがクローズの場合、ステータスをひとつ前に戻す
                updateSql = SqlName.UpdateOrderUnClose;
                updateCond.Status = order.LastStatus;
                // 最終ステータスとクローズ日時は更新しなくてよい？
            }
            else if (!closeFlg && order.Status == APConsts.ORDER_STATUS.ORDER_SHIPPING)
            {
                // 出荷指図入力中の場合は出荷実績入力中に変更する
                updateSql = SqlName.UpdateOrderClose;
                updateCond.Status = APConsts.ORDER_STATUS.ORDER_SHIPPING_RESULT;
                updateCond.LastStatus = order.Status;
            }
            else
            {
                // 上記以外の場合は更新を行わないため終了
                return true;
            }
            // 共通の更新条件を設定
            updateCond.OrderNo = orderNo;
            updateCond.UpdateDate = now;
            updateCond.UpdateUserId = userId;
            // 更新
            return updateOrderHead(updateSql, updateCond, db, userId);
        }

        /// <summary>
        /// 受注ヘッダテーブル更新処理
        /// </summary>
        /// <param name="sqlName">実行するSQLファイル</param>
        /// <param name="condition">更新する内容</param>
        /// <param name="db">DB接続</param>
        /// <param name="userId">更新ユーザID</param>
        /// <returns>エラーの場合False</returns>
        private static bool updateOrderHead(string sqlName, ComDao.OrderHeadEntity condition, ComDB db, string userId)
        {
            int result = 0;
            // 更新前プルーフ登録
            if (!APComUtil.CreateProof<ComDao.OrderHeadEntity>(db, condition, SqlName.OrderProof, userId, APConsts.PROOF_STATUS.PRE_UPDATE))
            {
                return false;
            }
            result = db.RegistByOutsideSql(sqlName, SqlName.SubDir, condition);

            if (result < 0)
            {
                // 更新エラー
                return false;
            }
            // 更新後プルーフ登録
            return APComUtil.CreateProof<ComDao.OrderHeadEntity>(db, condition, SqlName.OrderProof, userId, APConsts.PROOF_STATUS.POST_UPDATE);
        }

        /// <summary>
        /// 受注の数量検索結果格納クラス
        /// </summary>
        private class OrderQtyCheckResult
        {
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int OrderRowNo { get; set; }
            /// <summary>Gets or sets 受注数より出荷数と売上数を引いた値</summary>
            /// <value>受注数より出荷数と売上数を引いた値</value>
            public decimal Qty { get; set; }
        }
    }
}
