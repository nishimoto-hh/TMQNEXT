using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebTemplate
{
    /// <summary>
    /// アプリケーション共通オブジェクトクラス
    /// </summary>
    public class AppCommonObject
    {
        /// <summary>
        /// コンフィグ
        /// </summary>
        public static AppConfigs Config { get; private set; }

        /// <summary>
        /// コンフィグの設定
        /// </summary>
        /// <param name="config"></param>
        public static void SetConfiguration(IConfiguration config)
        {
            var appSettings = new AppSettings();
            config.Bind("AppSettings", appSettings);
            var connectionSettings = new ConnectionStringWrapper();
            config.Bind("ConnectionStringWrapperSettings", connectionSettings);
            var logSettings = new LoggerSettingWrapper();
            config.Bind("LoggerWrapperSettings", logSettings);

            Config = new AppConfigs(appSettings, connectionSettings, logSettings);
        }
    }

    public class AppConfigs
    {
        /// <summary>
        /// アプリケーション設定
        /// </summary>
        public AppSettings AppSettings { get; private set; }

        /// <summary>
        /// DB接続設定
        /// </summary>
        public ConnectionStringWrapper ConnectionSettings { get; set; }

        /// <summary>
        /// ログ出力設定
        /// </summary>
        public LoggerSettingWrapper LogSettings { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app">アプリケーション設定</param>
        /// <param name="con">DB接続設定</param>
        /// <param name="log">ログ出力設定</param>
        public AppConfigs(AppSettings app, ConnectionStringWrapper con, LoggerSettingWrapper log)
        {
            this.AppSettings = app;
            this.ConnectionSettings = con;
            this.LogSettings = log;
        }
    }

    #region アプリケーション設定
    public class AppSettings
    {
        /// <summary>サイト名</summary>
        public string AppTitle { get; set; }

        /// <summary>サイト名略称(タイトルヘッダ表示用)</summary>
        public string AppTitleHeader { get; set; }

        /// <summary>製品バージョン</summary>
        public string AppVersion { get; set; }

        /// <summary>コピーライト</summary>
        public string AppCopyRight { get; set; }

        /// <summary>機能ID：サイトTOP</summary>
        public string AppTopConductId { get; set; }

        /// <summary>メニュー表示位置</summary>
        /// <remarks>「top_menu」上表示、「side_menu」横表示</remarks>
        public string AppMenuPlace { get; set; }

        /// <summary>検索後の画面制御デフォルト値</summary>
        /// <remarks>0:条件エリアロック、1:条件エリアロックしない</remarks>
        public string OpeSearchAfter { get; set; }

        /// <summary>ファイルサイズ：1GB以下</summary>
        public int? UploadFileSize { get; set; }

        /// <summary>SiteMinderからのログインかどうか</summary>
        public bool SiteMinderLogin { get; set; }

        /// <summary>SiteMinderの指定URL</summary>
        /// <remarks>
        /// SiteMinderLoginが"true"の場合に有効
        /// ～設定されている場合、指定URLからのアクセスのみ許可。
        /// ～未設定の場合、すべてのURLをアクセスのみ許可。
        /// </remarks>
        public string SiteMinderURL { get; set; }

        /// <summary>AzureADからのログインかどうか</summary>
        public bool AzureADLogin { get; set; }

        /// <summary>AzureADの指定URL</summary>
        /// <remarks>
        /// AzureADLoginが"true"の場合に有効
        /// ～設定されている場合、指定URLからのアクセスのみ許可。
        /// ～未設定の場合、すべてのURLをアクセスのみ許可。
        /// </remarks>
        public string AzureADEntityId { get; set; }
        public string AzureADSingleSignOnServiceUrl { get; set; }
        public string AzureADSingleLogoutServiceUrl { get; set; }

        /// <summary>ログアウト後に遷移するURL</summary>
        /// <remarks>
        /// SiteMinderLoginが"true"の場合に有効
        /// ～設定されている場合、ログイン時に取得したURLよりも優先される
        /// </remarks>
        public string LogOutURL { get; set; }

        /// <summary>AzureAD連携時にSPとして利用するURL</summary>
        public string TMQEntityId { get; set; }

        public string TMQReturnUrl { get; set; }

        public string TMQPublicOrigin { get; set; }

        /// <summary>公開モード</summary>
        /// <remarks>「test」:メンテナンスモード、「prot」:プロトタイプ作成モード、「debug」：デバッグモード、その他：Releaseモード</remarks>
        public string DeployMode { get; set; }

        /// <summary>DLLフォルダパス（アプリフォルダからの参照パス）</summary>
        public string LogicDllPathRef { get; set; }

        /// <summary>システム標準言語コード</summary>
        public string LanguageIdDefault { get; set; }

        /// <summary>マニュアル仮想URLパス（※未指定の場合、アプリルート）</summary>
        public string ManualRootURL { get; set; }

        /// <summary>マニュアルフォルダパス（アプリフォルダからの参照パス）</summary>
        public string ManualRootPath { get; set; }

        /// <summary>マニュアルファイル接頭語</summary>
        public string ManualFilePrefix { get; set; }

        /// <summary>マニュアルファイル拡張子</summary>
        /// <remarks>ファイル接頭語 + [機能ID] + ファイル拡張子　例)manual_default.pdf</remarks>
        public string ManualFileExt { get; set; }

        /// <summary>画像フォルダパス（※未指定の場合、アプリルート）</summary>
        public string ImageDirURL { get; set; }

        /// <summary>ＩＤ切替ボタン押下時に遷移するURL</summary>
        /// <remarks>未設定の場合、通常のログアウト処理が実行される</remarks>
        public string IdChangeURL { get; set; }

        /// <summary>SQL実行時、タイムアウトまでの秒数</summary>
        public int? DBTimeOutSeconds { get; set; }

        /// <summary>Excelテンプレートファイルフォルダパス（アプリフォルダからの参照パス）</summary>
        public string ExcelTemplateDir { get; set; }

        /// <summary>Excelファイル出力先フォルダパス（アプリフォルダからの参照パス）</summary>
        public string ExcelOutputDir { get; set; }

        /// <summary>一時テーブル削除期限(時間、自ユーザ)</summary>
        /// <remarks>自ユーザの一時テーブルデータを削除対象とする、ログイン開始時間からの時間</remarks>
        public int? TmpTableDeleteHours { get; set; }

        /// <summary>一時テーブル削除期限(時間、他ユーザ)</summary>
        /// <remarks>他ユーザの一時テーブルデータを削除対象とする、ログイン開始時間からの時間</remarks>
        public int? TmpTableDeleteHoursOther { get; set; }

        /// <summary>固定SQL文テキストファイル格納先アセンブリ名</summary>
        public string FixedSqlStatementAssemblyName { get; set; }

        /// <summary>固定SQL文テキストファイル格納先フォルダパス(「.」区切り)</summary>
        public string FixedSqlStatementDir { get; set; }

        /// <summary>オートコンプリート表示行数上限</summary>
        public int? AutoCompleteRowLimit { get; set; }
        /// <summary>マニュアルのページ番号</summary>
        public Dictionary<string, int> ManualPageNo { get; set; }
        /// <summary>ダウンロードファイルフォルダパス（アプリフォルダからの参照パス）</summary>
        public string DownloadFileDir { get; set; }

        /// <summary>偽造防止トークン更新対象キーワード</summary>
        public List<string> TokenUpdateKeywords { get; set; }

        /// <summary>自動ログイン待機時間[ms]</summary>
        public int AutoLoginWaitTime { get; set; }
    }
    #endregion

    #region DB接続設定
    public class ConnectionStringSetting
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

        public ConnectionStringSetting()
        {
        }

        public ConnectionStringSetting(string name, string connectionString)
            : this(name, connectionString, null)
        {
        }

        public ConnectionStringSetting(string name, string connectionString, string providerName)
        {
            this.Name = name;
            this.ConnectionString = connectionString;
            this.ProviderName = providerName;
        }
    }
    #endregion

    #region ログ出力設定
    public class ConnectionStringWrapper
    {
        public string DefaultConnectionStringName { get; set; } = "";
        public List<ConnectionStringSetting> ConnectionStringSettings { get; set; } = new List<ConnectionStringSetting>();
        //public Dictionary<string, ConnectionStringSetting> ConnectionStringSettings { get; set; } = new Dictionary<string, ConnectionStringSetting>();

        public ConnectionStringSetting GetDefaultConnectionStringSetting()
        {
            ConnectionStringSetting returnItem = this.GetConnectionStringSetting(this.DefaultConnectionStringName);
            return returnItem;
        }

        public ConnectionStringSetting GetConnectionStringSetting(string name)
        {
            ConnectionStringSetting returnItem = null;
            if (null != this.ConnectionStringSettings && this.ConnectionStringSettings.Any())
            {
                returnItem = this.ConnectionStringSettings.FirstOrDefault(ce => ce.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            if (null == returnItem)
            {
                throw new ArgumentOutOfRangeException(string.Format("No default ConnectionStringSetting found. (ConnectionStringSettings.Names='{0}', Search.Name='{1}')", this.ConnectionStringSettings == null ? string.Empty : string.Join(",", this.ConnectionStringSettings.Select(ce => ce.Name)), name));
            }
            return returnItem;
        }
    }

    public class LoggerSetting
    {
        /// <summary>
        /// ロガー名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 出力先
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// ログ出力最小レベル
        /// </summary>
        public string MinLevel { get; set; }

        /// <summary>
        /// ログ本文日付フォーマット文字列
        /// </summary>
        public string ContentsDateFormat { get; set; }

        /// <summary>
        /// ログ出力先フォルダパス
        /// </summary>
        public string DirPath { get; set; }

        /// <summary>
        /// ログファイル名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// ログファイル拡張子
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// 世代管理時のログファイルサイズ上限
        /// </summary>
        public string ArchiveAboveSize { get; set; }

        /// <summary>
        /// 日付による世代管理時の日付種類
        /// </summary>
        public string ArchiveDateType { get; set; }

        /// <summary>
        /// ファイル名の日付フォーマット文字列
        /// </summary>
        public string FileNameDateFormat { get; set; }

        /// <summary>
        /// 世代管理ファイル数上限
        /// </summary>
        public string MaxArchiveFiles { get; set; }

        public LoggerSetting()
        {
        }

        public LoggerSetting(string name, string target)
            : this(name, target, null)
        {
        }

        public LoggerSetting(string name, string target, string minLevel)
        {
            this.Name = name;
            this.Target = target;
            this.MinLevel = minLevel;
        }
    }

    public class LoggerSettingWrapper
    {
        public string DefaultLoggerName { get; set; } = "";
        public List<LoggerSetting> LoggerSettings { get; set; } = new List<LoggerSetting>();
        //public Dictionary<string, LoggerSetting> LogSettings { get; set; } = new Dictionary<string, LoggerSetting>();

        public LoggerSetting GetDefaultLoggerSetting()
        {
            LoggerSetting returnItem = this.GetLoggerSetting(this.DefaultLoggerName);
            if (null == returnItem)
            {
                returnItem = new LoggerSetting();
            }
            return returnItem;
        }

        public LoggerSetting GetLoggerSetting(string name)
        {
            LoggerSetting returnItem = null;
            if (null != this.LoggerSettings && this.LoggerSettings.Any())
            {
                returnItem = this.LoggerSettings.FirstOrDefault(ce => ce.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            if (null == returnItem)
            {
                //throw new ArgumentOutOfRangeException(string.Format("No default LoggerSetting found. (LoggerSettings.Names='{0}', Search.Name='{1}')", this.LoggerSettings == null ? string.Empty : string.Join(",", this.LoggerSettings.Select(ce => ce.Name)), name));
                returnItem = GetDefaultLoggerSetting();
            }
            return returnItem;
        }
    }
    #endregion

    #region パスワード暗号化
    /// <summary>
    /// ユーザログインパスワード暗号化管理クラス
    /// </summary>
    public class PasswordEncrypt
    {
        /// <summary>暗号化キー</summary>
        private const string KEY = "G9ifhNTt";
        /// <summary>パスワードソルト(付加文字列)</summary>
        private const string SALT = "_xS5ifag9";
        /// <summary>
        /// 入力されたパスワードにソルトを追加した、データベースの値と比較するためのパスワードを作成
        /// </summary>
        /// <param name="inputPassword"></param>
        /// <returns></returns>
        public static string GetNewPassWord(string inputPassword)
        {
            return inputPassword + SALT;
        }
        /// <summary>
        /// パスワード暗号化のキーを作成
        /// </summary>
        /// <returns></returns>
        public static string GetEncryptKey()
        {
            return KEY;
        }
    }
    #endregion
}
