using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using APConsts = APConstants.APConstants;
using ComBusBase = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase;

namespace CommonAPUtil.APCommonUtil
{
    /// <summary>
    /// AP共通：共通画面汎用クラス
    /// </summary>
    public class APCommonFormClass
    {
        /// <summary>
        /// COM0050_在庫検索画面
        /// </summary>
        public static class Com0050
        {
            /// <summary>
            /// 在庫検索画面で登録を行ったかどうかを取得する処理
            /// </summary>
            /// <param name="form">画面情報(this)</param>
            /// <returns>登録を行った場合、True</returns>
            public static bool GetIsRegist(ComBusBase form)
            {
                // グローバルリストから、登録を行ったかどうかの値を取得(取得後、削除)
                var isRegist = form.GetGlobalData(APConsts.COMMON_FORM.COM0050.GlobalKeyIsRegist, true);
                if (isRegist == null)
                {
                    // 取得できなかった場合、登録を行っていない
                    return false;
                }
                // 取得した値をboolに変換して返す
                return bool.Parse(isRegist.ToString());
            }
        }

    }
}
