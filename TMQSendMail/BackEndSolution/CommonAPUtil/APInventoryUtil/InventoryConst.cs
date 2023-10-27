using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPUtil.APInventoryUtil
{
    /// <summary>
    /// 在庫更新：固定値
    /// </summary>
    public class InventoryConst
    {
        #region 固定値
        /// <summary>
        /// 在庫更新用処理区分
        /// </summary>
        public static class INVENTORY_PROCESS_DIVISION
        {
            /// <summary>0:登録・承認</summary>
            public const int ADD = 0;
            /// <summary>9:取消・削除・承認取消</summary>
            public const int CANCEL = 9;
            /// <summary>70:差分登録</summary>
            public const int DIFFERENCE = 70;
            /// <summary>79:差分登録取消</summary>
            public const int DIFFERENCE_CANCEL = 79;
            /// <summary>80:完了</summary>
            public const int COMPLETE = 80;
            /// <summary>89:完了取消</summary>
            public const int COMPLETE_CANCEL = 89;
            /// <summary>90:クローズ(受注・発注　他)</summary>
            public const int CLOSE = 90;
            /// <summary>91:クローズ(オーダー番号単位)</summary>
            public const int CLOSE_ORDER = 91;
            /// <summary>92:クローズ(発注クローズ用：購入依頼もクローズする場合使用)</summary>
            public const int CLOSE_ALL = 92;
            /// <summary>99:クローズ取消</summary>
            public const int CLOSE_CANCEL = 99;
        }

        /// <summary>
        /// 在庫管理区分判定用
        /// </summary>
        public static class STOCK_DIVISION_RESULT
        {
            /// <summary>0:更新対象</summary>
            public const int TARGET = 0;
            /// <summary>3:更新対象外</summary>
            public const int NOT_APPLICABLE = 3;
            /// <summary>9:エラー</summary>
            public const int ERROR = 9;
        }

        /// <summary>
        /// ログファイル名
        /// </summary>
        public const string LOG_NAME = "Inventory";

        #endregion
    }
}
