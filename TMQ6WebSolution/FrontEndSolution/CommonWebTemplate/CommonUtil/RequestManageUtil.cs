
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　リクエスト管理クラス
/// 説明　　　：　リクエストの管理および操作の共通処理を実装します。
/// 
/// 履歴　　　：　2018.05.10 河村純子　新規作成
///</summary>

namespace CommonWebTemplate.CommonUtil
{
    public class RequestManageUtil
    {
        #region === 定数定義 ===
        ///<summary>siteMinderから受領するRequest Key</summary>
        public static class RequestKey
        {
            /// <summary>
            /// SiteMinderで認証されたユーザのログイン名
            /// </summary>
            public const string SM_USER = @"HTTP_SM_USER";
        }

        ///<summary>ｱﾌﾟﾘ内で管理するSession Key</summary>
        public static class SessionKey
        {
            /// <summary>
            /// ユーザー管理情報
            /// </summary>
            public const string CIM_USER_INFO = @"CIM_USER_INFO";
            /// <summary>
            /// 遷移元URL（ﾛｸﾞｲﾝ時の遷移元：※ｻｲﾄﾏｲﾝﾀﾞｰ用）
            /// </summary>
            public const string CIM_TRANS_SRC_URL = @"CIM_TRANS_SRC_URL";
            /// <summary>
            /// URL直接起動時遷移キー(※シングルサインオン用)
            /// </summary>
            public const string TMQ_SSO_ACCESS_KEY = "TMQ_SSO_ACCESS_KEY";
        }
        #endregion

        #region === public static 処理 ===
        #endregion

        #region === private static 処理 ===
        #endregion
    }

    /// <summary>
    /// ユーザー管理情報
    /// </summary>
    public class UserInfoDef
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// ログインID
        /// </summary>
        public string LoginId { get; set; }
        /// <summary>
        /// ユーザー表示名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 言語ID
        /// </summary>
        public string LanguageId { get; set; }
        /// <summary>
        /// ユーザー権限レベルID
        /// </summary>
        public int AuthorityLevelId { get; set; }
        /// <summary>
        /// ユーザー権限機能ﾏｽﾀﾘｽﾄ
        /// ※ﾕｰｻﾞｰに権限のある機能が対象
        /// </summary>
        public IList<CommonConductMst> UserAuthConducts { get; set; }
        /// <summary>
        /// GUID
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// 所属工場ID（先頭が本務工場、以下兼務工場）
        /// </summary>
        public List<int> FactoryIdList { get; set; }

        /// <summary>
        /// 所属情報
        /// </summary>
        public BelongingInfo BelongingInfo { get; set; }

        #region === コンストラクタ ===
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserInfoDef()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ユーザID</param>
        public UserInfoDef(string id)
        {
            UserId = id;
        }
        #endregion
    }

    /// <summary>
    /// セッション操作拡張クラス
    /// </summary>
    public static class SessionExtensions
    {

        /// <summary>
        /// セッションにオブジェクトを設定する
        /// </summary>
        /// <typeparam name="T">オブジェクトの型</typeparam>
        /// <param name="session">セッション</param>
        /// <param name="key">セッションキー</param>
        /// <param name="obj">対象オブジェクト</param>
        public static void SetObject<T>(this ISession session, string key, T obj)
        {
            //2024.09 .NET8バージョンアップ対応 start
            //var json = JsonSerializer.Serialize<T>(obj, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), });
            var json = JsonSerializer.Serialize<T>(obj, JsonSerializerOptionsDefine.JsOptionsForEncode);
            //2024.09 .NET8バージョンアップ対応 end
            session.SetString(key, json);
        }

        /// <summary>
        /// セッションからオブジェクトを取得する
        /// </summary>
        /// <typeparam name="T">オブジェクトの型</typeparam>
        /// <param name="session">セッション</param>
        /// <param name="key">セッションキー</param>
        /// <returns></returns>
        public static T GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return string.IsNullOrEmpty(json)
                ? default(T)
                : JsonSerializer.Deserialize<T>(json);
        }
    }
}