///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　ユーザー権限情報クラス
/// 説明　　　：　ユーザーの機能権限、処理権限情報を定義します。
/// 
/// 履歴　　　：　2018.02.22 河村純子　新規作成
///</summary>
using System.Collections.Generic;

namespace CommonWebTemplate.Models.Common
{
    public class CommonUserAuth
    {
        /// <summary>
        /// 共通_ユーザー機能権限マスタ
        /// </summary>
        public COM_USER_AUTH_CONDUCT_V USERAUTH_CONDUCT { get; set; }

        /// <summary>
        /// 共通_ユーザー処理権限マスタ
        /// </summary>
        public IList<COM_USER_AUTH_SHORI_V> USERAUTH_SHORIS { get; set; }

        #region === ｺﾝｽﾄﾗｸﾀ ===
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public CommonUserAuth(COM_USER_AUTH_CONDUCT_V userAuthConduct)
        {
            USERAUTH_CONDUCT = userAuthConduct;
        }
        #endregion

    }
}