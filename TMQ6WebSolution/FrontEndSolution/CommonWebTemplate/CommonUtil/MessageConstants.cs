///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　共通ﾒｯｾｰｼﾞ定数ｸﾗｽ
/// 説明　　　：　共通WEB画面で使用するﾒｯｾｰｼﾞを定数定義します。
/// 
/// 履歴　　　：　2018.04.04 ATTS河村純子　新規作成
///</summary>

namespace CommonWebTemplate.CommonUtil
{
    /// <summary>
    /// ｱﾌﾟﾘｹｰｼｮﾝ制御用定数
    /// </summary>
    public class MessageConstants
    {
        /// <summary>
        /// ｲﾝﾌｫﾒｰｼｮﾝ
        /// </summary>
        public static class Info
        {
            /// <summary>【CW00000.I01】処理が完了しました。</summary>
            public const string I01 = "【CW00000.I01】処理が完了しました。";
            /// <summary>【CW00000.I02】Excel作成処理が完了しました。</summary>
            public const string I02 = "【CW00000.I02】Excel作成処理が完了しました。";
        }

        /// <summary>
        /// ﾜｰﾆﾝｸﾞ
        /// </summary>
        public static class Warning
        {
            /// <summary>【CW00000.W01】入力エラーがあります。</summary>
            public const string W01 = "【CW00000.W01】入力エラーがあります。";
            /// <summary>【CW00000.W02】Excel出力用データが作成されませんでした。</summary>
            public const string W02 = "【CW00000.W02】Excel出力用データが作成されませんでした。";
            /// <summary>【CW00000.W03】ユーザーを入力してください。</summary>
            public const string W03 = "【CW00000.W03】ユーザーを入力してください。";
            /// <summary>【CW00000.W04】パスワードを入力してください。</summary>
            public const string W04 = "【CW00000.W04】パスワードを入力してください。";
            /// <summary>【CW00000.W05】ログイン認証に失敗しました。ユーザー権限がありません。</summary>
            public const string W05 = "【CW00000.W05】ログイン認証に失敗しました。ユーザー権限がありません。";
            /// <summary>【CW00000.W06】ログインしてください。</summary>
            public const string W06 = "【CW00000.W06】ログインしてください。";
            /// <summary>【CW00000.W07】メニューから選択してください。(※ｱｸｾｽ不正)</summary>
            public const string W07 = "【CW00000.W07】メニューから選択してください。";
            /// <summary>【CW00000.W08】機能参照権限がありません。(※ﾒﾆｭｰ権限不正)</summary>
            public const string W08 = "【CW00000.W08】機能参照権限がありません。";
            /// <summary>【CW00000.W09】処理権限がありません。(※ﾎﾞﾀﾝ権限不正)</summary>
            public const string W09 = "【CW00000.W09】処理権限がありません。";
            /// <summary>【CW00000.W10】すでにログインしています。一旦ログアウトしてから再ログインしてください。</summary>
            public const string W10 = "【CW00000.W10】すでにログインしています。一旦ログアウトしてから再ログインしてください。";

        }

        /// <summary>
        /// ｴﾗｰ
        /// </summary>
        public static class Error
        {
            /// <summary>【CW00000.E01】処理に失敗しました。</summary>
            public const string E01 = "【CW00000.E01】処理に失敗しました。";
            /// <summary>WebApiﾘｸｴｽﾄId不正時</summary>
            public const string E02 = "【CW00000.E02】リクエストが不正です。";
            /// <summary>取り込みﾌｧｲﾙ不正時等</summary>
            public const string E03 = "【CW00000.E03】ファイルの取り込みに失敗しました。";
            /// <summary>処理結果不正時等</summary>
            public const string E04 = "【CW00000.E04】処理結果が不正です。";
            /// <summary>遷移元URL不正時等</summary>
            public const string E05 = "【CW00000.E05】SiteMinderログインページが見つかりません。";
            /// <summary>遷移元URLｱｸｾｽ不正</summary>
            public const string E06 = "【CW00000.E06】許可されてないURLからのアクセスです。";
            /// <summary>ｱｸｾｽ不正時（内容が特定できない場合のｴﾗｰ）</summary>
            public const string E07 = "【CW00000.E07】アクセスが不正です。";
            /// <summary>【CW00000.E08】コンボボックスの生成に失敗しました。</summary>
            public const string E08 = "【CW00000.E08】コンボボックスの生成に失敗しました。";
        }

        /// <summary>
        /// ﾎﾟｯﾌﾟｱｯﾌﾟ確認
        /// </summary>
        public static class Confirm
        {
            /// <summary>データ件数が最大件数を超えています。表示しますか？</summary>
            public const string C01 = "データ件数が最大表示件数を超えています。表示しますか？";

        }

        /// <summary>
        /// 表示値
        /// </summary>
        public static class DispVal
        {
            /// <summary>ﾁｪｯｸﾎﾞｯｸｽ - ﾁｪｯｸ状態</summary>
            public const string CheckBoxChecked = "◆";

        }

    }

}
