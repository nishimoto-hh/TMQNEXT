using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;
namespace BusinessLogic_CM00001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_CM00001
    {
        private const int noDisplay = -1;

        /// <summary>
        /// 申請中件数
        /// </summary>
        public class ApplicationCount
        {
            /// <summary>機器台帳・申請データ作成中件数</summary>
            public int McMakingCount { get; set; }
            /// <summary>機器台帳・承認依頼中件数</summary>
            public int McRequestCount { get; set; }
            /// <summary>機器台帳・差戻中件数</summary>
            public int McReturnCount { get; set; }
            /// <summary>長期計画・申請データ作成中件数</summary>
            public int LpMakingCount { get; set; }
            /// <summary>長期計画・承認依頼中件数</summary>
            public int LpRequestCount { get; set; }
            /// <summary>長期計画・差戻中件数</summary>
            public int LpReturnCount { get; set; }

            /// <summary>
            /// 非表示の場合の値を返す
            /// </summary>
            /// <returns>非表示の場合の値</returns>
            public static ApplicationCount GetNoDisplay()
            {
                ApplicationCount result = new();
                result.McMakingCount = noDisplay;
                result.McRequestCount = noDisplay;
                result.McReturnCount = noDisplay;
                result.LpMakingCount = noDisplay;
                result.LpRequestCount = noDisplay;
                result.LpReturnCount = noDisplay;
                return result;
            }
        }

        /// <summary>
        /// 承認待ち件数
        /// </summary>
        public class AprovalCount
        {
            /// <summary>機器台帳の件数</summary>
            public int McAppReqCount { get; set; }
            /// <summary>長期計画の件数</summary>
            public int LpAppReqCount { get; set; }

            /// <summary>
            /// 非表示の場合の値を返す
            /// </summary>
            /// <returns>非表示の場合の値</returns>
            public static AprovalCount GetNoDisplay()
            {
                AprovalCount result = new();
                result.McAppReqCount = noDisplay;
                result.LpAppReqCount = noDisplay;
                return result;
            }
        }
    }
}
