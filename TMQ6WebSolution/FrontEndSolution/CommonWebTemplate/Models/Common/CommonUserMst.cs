///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　ユーザー情報クラス
/// 説明　　　：　共通_ユーザーマスタとそれに紐づく権限マスタ情報を定義します。
/// 
/// 履歴　　　：　2018.02.22 河村純子　新規作成
///</summary>
using System.Collections.Generic;

namespace CommonWebTemplate.Models.Common
{
    public class CommonUserMst
    {

        /// <summary>
        /// ﾛｸﾞｲﾝﾕｰｻﾞｰID
        /// </summary>
        public string LoginUser;
        /// <summary>
        /// ﾛｸﾞｲﾝﾊﾟｽﾜｰﾄﾞ
        /// </summary>
        public string LoginPassword;

        /// <summary>
        /// ﾕｰｻﾞｰ表示名
        /// </summary>
        public string GetUserName { get; set; }

        /// <summary>
        /// ユーザID
        /// </summary>
        public string UserId;

        #region === ｺﾝｽﾄﾗｸﾀ ===
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public CommonUserMst()
        {
        }
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public CommonUserMst(string loginUser, string loginPassword = "", string userName = "", string userId = "")
        {
            LoginUser = loginUser;
            LoginPassword = loginPassword;
            GetUserName = userName;
            UserId = userId;
        }
        #endregion

    }
}