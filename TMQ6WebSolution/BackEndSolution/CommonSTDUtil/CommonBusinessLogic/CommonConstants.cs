///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　共通データ定数定義クラス
/// 説明　　　：　共通データテーブル内で使用する定数を定義します。
/// 履歴　　　：　
///</summary>

namespace CommonSTDUtil
{
    /// <summary>
    /// 共通データ定数定義クラス
    /// </summary>
    public class CommonConstants
    {
        /// <summary>
        /// 明示的な戻り値
        /// </summary>
        /// <remarks>1:正常、-1:異常</remarks>
        public static class RETURN_RESULT
        {
            /// <summary>正常</summary>
            public const int OK = 1;
            /// <summary>異常</summary>
            public const int NG = -1;
        }

        /// <summary>
        /// チェックフラグ
        /// </summary>
        public static class CHECK_FLG
        {
            /// <summary>チェックあり</summary>
            public static string ON = "1";
            /// <summary>チェックなし</summary>
            public static string OFF = "0";
        }

        /// <summary>
        /// ファイル取込で使用するファイルの文字コード
        /// </summary>
        public const string UPLOAD_INFILE_CHAR_CODE = "UTF-8";

        /// <summary>
        /// 削除フラグ
        /// </summary>
        public static class DEL_FLG
        {
            /// <summary>未削除</summary>
            public static int OFF = 0;
            /// <summary>削除</summary>
            public static int ON = 1;
        }

        /// <summary>
        /// 明示的な戻り値
        /// </summary>
        /// <remarks>1:正常、-1:異常</remarks>
        public static class LOG_NO
        {
            /// <summary>確認ステータス時ログ番号</summary>
            public const string CONFIRM_LOG_NO = "999";
        }
        /// <summary>
        /// ワークフロー関連定数
        /// </summary>
        public static class WORKFLOW
        {
            /// <summary>
            /// ワークフローヘッダステータス
            /// </summary>
            public static class HEADERSTATUS
            {
                /// <summary>依頼中</summary>
                public static int REQUESTING = 0;
                /// <summary>承認中</summary>
                public static int APPROVING = 10;
                /// <summary>承認完了</summary>
                public static int APPROVAL = 20;
                /// <summary>否認</summary>
                public static int DENIAL = 90;
                /// <summary>引戻</summary>
                public static int CANCEL = 99;
            }

            /// <summary>
            /// ワークフロー詳細ステータス
            /// </summary>
            public static class DETAILSTATUS
            {
                /// <summary>未承認</summary>
                public static int? UNAPPROVED = null;
                /// <summary>承認</summary>
                public static int APPROVAL = 10;
                /// <summary>否認</summary>
                public static int DENIAL = 90;
            }

            /// <summary>
            /// 操作履歴
            /// </summary>
            public static class LOGSTATUS
            {
                /// <summary>承認依頼</summary>
                public static int APPROVAL_REQUEST = 0;
                /// <summary>承認</summary>
                public static int APPROVAL = 10;
                /// <summary>承認取消</summary>
                public static int APPROVAL_CANCEL = 19;
                /// <summary>否認</summary>
                public static int DENIAL = 90;
                /// <summary>引戻</summary>
                public static int CANCEL = 99;
            }

            /// <summary>
            /// 内部パラメータ
            /// </summary>
            public static class REQUESTPARAM
            {
                /// <summary>依頼依頼</summary>
                public static int APPROVAL_REQUEST = 0;
                /// <summary>承認依頼取消</summary>
                public static int APPROVAL_REQUEST_CANCEL = 9;
                /// <summary>承認依頼</summary>
                public static int APPROVAL = 10;
                /// <summary>承認</summary>
                public static int APPROVAL_CANCEL = 19;
                /// <summary>否認</summary>
                public static int DENIAL = 90;
            }
        }

        /// <summary>
        /// エクセル出力関連定数
        /// </summary>
        public static class REPORT
        {
            /// <summary>
            /// 出力ファイル種類
            /// </summary>
            public static class FILETYPE
            {
                /// <summary>EXCEL</summary>
                public static string EXCEL = "1";
                /// <summary>CSV</summary>
                public static string CSV = "2";
                /// <summary>PDF</summary>
                public static string PDF = "3";
                /// <summary>Zip</summary>
                public static string ZIP = "4";
                /// <summary>UnDefined</summary>
                public static string UNDEFINED = "5";
            }

            /// <summary>
            /// ファイル拡張子
            /// </summary>
            public static class EXTENSION
            {
                /// <summary>.xlsx</summary>
                public static string EXCEL_BOOK = ".xlsx";
                /// <summary>.zip</summary>
                public static string ZIP_FILE = ".zip";
                /// <summary>.csv</summary>
                public static string CSV = ".csv";
            }
        }

        /// <summary>From-Toの区切り用文字</summary>
        public const string SeparatorFromTo = "|";

        /// <summary>システム年度の初期値設定用文字列</summary>
        /// <remarks>SysYearを含むとシステム年の処理に含まれてしまうので注意</remarks>
        public const string SystemFiscalYearText = "SysFiscalYear";

        /// <summary>
        /// 言語ID
        /// </summary>
        public static class LanguageId
        {
            /// <summary>日本語</summary>
            public const string Japanese = "ja";
        }
    }
}
