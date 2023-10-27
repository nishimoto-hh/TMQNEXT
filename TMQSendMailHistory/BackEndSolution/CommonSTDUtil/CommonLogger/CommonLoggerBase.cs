using CommonWebTemplate;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonSTDUtil.CommonLogger
{
    /// <summary>
    /// ログ出力基本クラス
    /// </summary>
    public class CommonLoggerBase
    {
        /// <summary>
        /// ログ出力先
        /// </summary>
        public enum LogTarget : int
        {
            /// <summary>ファイル</summary>
            File,
            /// <summary>コンソール</summary>
            Console,
            /// <summary>DB</summary>
            DB,
            /// <summary>イミディエイトウィンドウ</summary>
            Immediate
        }

        private static Dictionary<string, CommonLoggerBase> singletonDictionary = null;

        private readonly object lockObj = new object();

        /// <summary>ログ出力先</summary>
        protected readonly LogTarget target;
        /// <summary>ログファイル出力先ルートパス</summary>
        protected readonly string dirPath;
        /// <summary>最小ログレベル</summary>
        protected readonly LogLevel levelMin;
        /// <summary>ログ内容の日付フォーマット</summary>
        protected readonly string contentsDateFmt;

        /// <summary>ログファイル名</summary>
        protected readonly string logFileName;
        /// <summary>ログファイル拡張子</summary>
        protected readonly string fileExt;
        /// <summary>サイズによる世代管理の有効/無効</summary>
        protected readonly bool archiveByAboveSize;
        /// <summary>サイズによる世代管理の上限値</summary>
        protected readonly long archiveAboveSize;
        /// <summary>日付による世代管理の有効/無効</summary>
        protected readonly bool archiveByDate;
        /// <summary>日付による世代管理の日付種類</summary>
        protected readonly DateType archiveDateType;
        /// <summary>世代管理数</summary>
        protected readonly int maxArchiveFiles;
        /// <summary>ファイル名の日付フォーマット</summary>
        protected readonly string fileNameDateFmt;

        /// <summary>
        /// インスタンス生成
        /// </summary>
        /// <param name="loggerName">ロガー名</param>
        /// <param name="rootPath">ログ出力先ディレクトリルートパス</param>
        /// <returns></returns>
        public static CommonLoggerBase GetInstance(string loggerName)
        {
            if (singletonDictionary == null)
            {
                singletonDictionary = new Dictionary<string, CommonLoggerBase>();
            }

            if (singletonDictionary.ContainsKey(loggerName))
            {
                return singletonDictionary[loggerName];
            }
            else
            {
                var singleton = new CommonLoggerBase(loggerName);
                singletonDictionary.Add(loggerName, singleton);
                return singleton;
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CommonLoggerBase()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CommonLoggerBase(string loggerName)
        {
            // コンフィグ取得
            var configItem = AppCommonObject.Config.LogSettings.GetLoggerSetting(loggerName);

            if (!string.IsNullOrWhiteSpace(configItem.Target))
            {
                if (!Enum.TryParse<LogTarget>(configItem.Target, out this.target))
                {
                    this.target = LogTarget.File;
                }
            }
            else
            {
                this.target = LogTarget.File;
            }

            this.levelMin = !string.IsNullOrWhiteSpace(configItem.MinLevel) ? 
                LogLevel.FromName(configItem.MinLevel) : LogLevel.Info;

            this.contentsDateFmt = !string.IsNullOrWhiteSpace(configItem.ContentsDateFormat) ? 
                configItem.ContentsDateFormat : "yyyy/MM/dd HH:mm:ss";

            // 出力先がファイルの場合の設定を取得
            if (this.target == LogTarget.File)
            {
                this.dirPath = !string.IsNullOrWhiteSpace(configItem.DirPath) ? configItem.DirPath : @".\logs";

                this.logFileName = !string.IsNullOrWhiteSpace(configItem.FileName) ? configItem.FileName : "SendMailHistory";

                this.fileExt = !string.IsNullOrWhiteSpace(configItem.FileExtension) ? configItem.FileExtension : "log";

                long size;
                if (!string.IsNullOrWhiteSpace(configItem.ArchiveAboveSize) && 
                    long.TryParse(configItem.ArchiveAboveSize, out size) && (size > 0))
                {
                    this.archiveByAboveSize = true;
                    this.archiveAboveSize = size;
                }
                else
                {
                    this.archiveByAboveSize = false;
                    this.archiveAboveSize = 0;
                }

                if (!string.IsNullOrWhiteSpace(configItem.ArchiveDateType))
                {
                    DateType type = DateType.FromName(configItem.ArchiveDateType);
                    this.archiveDateType = type;
                    if (!type.Equals(DateType.None))
                    {
                        this.archiveByDate = true;

                        if (!string.IsNullOrWhiteSpace(configItem.FileNameDateFormat))
                        {
                            this.fileNameDateFmt = configItem.FileNameDateFormat;
                        }
                        else
                        {
                            this.fileNameDateFmt = this.archiveDateType.DefaultFormat;
                        }
                    }
                    else
                    {
                        this.archiveByDate = false;
                    }
                }
                else
                {
                    this.archiveByDate = false;
                    this.archiveDateType = DateType.None;
                }

                int files;
                if (!string.IsNullOrWhiteSpace(configItem.MaxArchiveFiles) && int.TryParse(configItem.MaxArchiveFiles, out files))
                {
                    this.maxArchiveFiles = files;
                }
                else
                {
                    this.maxArchiveFiles = 0;
                }
            }

        }

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="msg">メッセージ文字列</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="subFileName">サブファイル名</param>
        /// <param name="param">その他パラメータ</param>
        //public void WriteLog(LogLevel level, string msg, string subDirName = "", string subFileName = "", dynamic param = null)
        public virtual void WriteLog(LogLevel level, LogType type, string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            DateTime now = DateTime.Now;

            try
            {
                if (level < this.levelMin)
                {
                    return;
                }

                switch (this.target)
                {
                    case LogTarget.Console:
                        WriteLogToConsole(level, type, msg, now);
                        break;
                    case LogTarget.DB:
                        WriteLogToDB(level, type, msg, now, param);
                        break;
                    case LogTarget.Immediate:
                        WriteLogToImmediate(level, type, msg, now);
                        break;
                    default:
                        WriteLogToFile(level, type, msg, now, subDirName, prefix, subFileName);
                        break;
                }
            }
            catch
            {
                // 何もしない
            }
        }

        /// <summary>
        /// ファイルへのログ出力
        /// </summary>
        /// <param name="level"></param>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        /// <param name="date"></param>
        /// <param name="subDirName"></param>
        /// <param name="prefix"></param>
        /// <param name="subFileName"></param>
        /// <param name="param">その他パラメータ</param>
        //protected void WriteLogToFile(LogLevel level, string msg, DateTime date, string subDirName, string subFileName)
        protected void WriteLogToFile(LogLevel level, LogType type, string msg, DateTime date, string subDirName, string prefix, string subFileName)
        {
            string filePath = this.dirPath;

            try
            {
                //int tid = System.Threading.Thread.CurrentThread.ManagedThreadId;
                //string msgText = string.Format("[{0:" + this.contentsDateFmt + "}][{1}][{2}] {3}", date, tid, level.Label, msg);
                // ログ種類の指定がない場合はログレベルのラベルを出力する
                string label = type == LogType.None ? level.Label : type.Label;
                string msgText = string.Format("[{0:" + this.contentsDateFmt + "}][{1}] {2}", date, label, msg);

                // 出力先ディレクトリの生成
                if (!string.IsNullOrWhiteSpace(subDirName))
                {
                    // サブディレクトリが指定されている場合、追加
                    filePath = Path.Combine(filePath, subDirName);
                }
                DirectoryInfo di = Directory.CreateDirectory(filePath);

                // 出力ファイル名の生成
                //string baseFileName = this.logFileName;
                string baseFileName = prefix;
                if (!string.IsNullOrEmpty(prefix))
                {
                    baseFileName += "_";
                }
                baseFileName += this.logFileName;
                if (!string.IsNullOrWhiteSpace(subFileName))
                {
                    // サブファイル名が指定されている場合、追加
                    baseFileName += "_" + subFileName;
                }

                if (this.archiveByDate)
                {
                    // 日付による世代管理を実行する場合、日付文字列を追加
                    baseFileName = string.Format("{0}_{1:" + this.fileNameDateFmt + "}", baseFileName, date);
                }

                //　出力ファイル名に拡張子を追加
                string fileName = baseFileName + "." + this.fileExt;

                filePath = Path.Combine(filePath, fileName);
                FileInfo fi = new FileInfo(filePath);
                bool addFile = false;

                lock (this.lockObj)
                {
                    if (fi.Exists)
                    {
                        // 出力ファイルが存在する場合
                        if (this.archiveByAboveSize)
                        {
                            if (fi.Length > this.archiveAboveSize)
                            {
                                // 上限サイズオーバーの場合、同名の過去のログファイルを取得
                                int maxSerialNo = 0;
                                var oldFiles = di.GetFiles(baseFileName + "*." + this.fileExt, SearchOption.TopDirectoryOnly);
                                foreach (var oldFile in oldFiles)
                                {
                                    if (oldFile.Name.Equals(fileName))
                                    {
                                        continue;
                                    }

                                    // 連番の最大値を取得
                                    int serialNo;
                                    if (int.TryParse(oldFile.Name.Replace(baseFileName, "").Replace("." + this.fileExt, "").Replace("_", ""), out serialNo))
                                    {
                                        if (maxSerialNo < serialNo)
                                        {
                                            maxSerialNo = serialNo;
                                        }
                                    }
                                }
                                maxSerialNo++;

                                // 現在のファイルを連番を付けてリネーム
                                fi.MoveTo(Path.Combine(fi.DirectoryName, string.Format("{0}_{1}.{2}", baseFileName, maxSerialNo, this.fileExt)));
                                // ファイル追加フラグON
                                addFile = true;

                            }
                        }
                    }
                    else
                    {
                        // 出力ファイルが存在しない場合、追加フラグON
                        addFile = true;
                    }

                    // ファイルへ出力
                    using (var sw = new StreamWriter(filePath, true, Encoding.UTF8))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine(msgText);
                    }

                    if (addFile && this.maxArchiveFiles > 0)
                    {
                        // 新規ファイル追加、かつ世代管理数が設定されている場合、出力ファイル数をチェック
                        checkOutputFiles(di, fileName, baseFileName, subFileName);
                    }
                }
            }
            catch
            {
                // 何もしない
            }
        }

        /// <summary>
        /// コンソールへのログ出力
        /// </summary>
        /// <param name="level"></param>
        /// <param name="msg"></param>
        /// <param name="date"></param>
        protected void WriteLogToConsole(LogLevel level, LogType type, string msg, DateTime date)
        {
            try
            {
                int tid = System.Threading.Thread.CurrentThread.ManagedThreadId;
                //string msgText = string.Format("[{0:" + this.contentsDateFmt + "}][{1}][{2}] {3}", date, tid, level.Label, msg);
                // ログ種類の指定がない場合はログレベルのラベルを出力する
                string label = type == LogType.None ? level.Label : type.Label;
                string msgText = string.Format("[{0:" + this.contentsDateFmt + "}][{1}] {2}", date, label, msg);

                Console.WriteLine(msgText);
            }
            catch
            {
                // 何もしない
            }
        }

        /// <summary>
        /// イミディエイトウィンドウへのログ出力
        /// </summary>
        /// <param name="level"></param>
        /// <param name="msg"></param>
        /// <param name="date"></param>
        protected void WriteLogToImmediate(LogLevel level, LogType type, string msg, DateTime date)
        {
            try
            {
                int tid = System.Threading.Thread.CurrentThread.ManagedThreadId;
                //string msgText = string.Format("[{0:" + this.contentsDateFmt + "}][{1}][{2}] {3}", date, tid, level.Label, msg);
                // ログ種類の指定がない場合はログレベルのラベルを出力する
                string label = type == LogType.None ? level.Label : type.Label;
                string msgText = string.Format("[{0:" + this.contentsDateFmt + "}][{1}] {2}", date, label, msg);

                System.Diagnostics.Debug.WriteLine(msgText);
            }
            catch
            {
                // 何もしない
            }
        }

        /// <summary>
        /// DBへのログ出力
        /// </summary>
        /// <remarks>DB出力が必要な場合は派生クラスで実装する</remarks>
        /// <param name="level"></param>
        /// <param name="msg"></param>
        /// <param name="date"></param>
        /// <param name="param">その他パラメータ</param>
        protected virtual void WriteLogToDB(LogLevel level, LogType type, string msg, DateTime date, dynamic param)
        {
            try
            {
                // DB出力が必要な場合は派生クラスで実装する
            }
            catch
            {
                // 何もしない
            }
        }

        /// <summary>
        /// トレースログ出力
        /// </summary>
        /// <param name="type">ログ種類</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Trace(LogType type, string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            WriteLog(LogLevel.Trace, type, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// トレースログ出力(例外出力あり)
        /// </summary>
        /// <param name="type">ログ種類</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Trace(LogType type, string msg, Exception ex, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            StringBuilder sbMsg = new StringBuilder();
            if (!string.IsNullOrEmpty(msg))
            {
                sbMsg.AppendLine(msg);
            }
            if (ex != null)
            {
                sbMsg.Append(ex.ToString());
            }
            WriteLog(LogLevel.Trace, type, sbMsg.ToString(), subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// トレースログ出力(ログ種類未指定)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Trace(string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            Trace(LogType.None, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// トレースログ出力(例外出力あり)(ログ種類未指定)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Trace(string msg, Exception ex, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            Trace(LogType.None, msg, ex, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// デバッグログ出力
        /// </summary>
        /// <param name="type">ログ種類</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Debug(LogType type, string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            WriteLog(LogLevel.Debug, type, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// デバッグログ出力(例外出力あり)
        /// </summary>
        /// <param name="type">ログ種類</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Debug(LogType type, string msg, Exception ex, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            StringBuilder sbMsg = new StringBuilder();
            if (!string.IsNullOrEmpty(msg))
            {
                sbMsg.AppendLine(msg);
            }
            if (ex != null)
            {
                sbMsg.Append(ex.ToString());
            }
            WriteLog(LogLevel.Debug, type, sbMsg.ToString(), subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// デバッグログ出力(ログ種類未指定)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Debug(string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            Debug(LogType.None, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// デバッグログ出力(例外出力あり)(ログ種類未指定)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Debug(string msg, Exception ex, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            Debug(LogType.None, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// 情報ログ出力
        /// </summary>
        /// <param name="type">ログ種類</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Info(LogType type, string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            WriteLog(LogLevel.Info, type, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// 情報ログ出力(ログ種類未指定)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Info(string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            Info(LogType.None, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// 警告ログ出力
        /// </summary>
        /// <param name="type">ログ種類</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Warn(LogType type, string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            WriteLog(LogLevel.Warn, type, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// 警告ログ出力(ログ種類未指定)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Warn(string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            Warn(LogType.None, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// エラーログ出力
        /// </summary>
        /// <param name="type">ログ種類</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Error(LogType type, string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            WriteLog(LogLevel.Error, type, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// エラーログ出力(例外出力あり)
        /// </summary>
        /// <param name="type">ログ種類</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Error(LogType type, string msg, Exception ex, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            StringBuilder sbMsg = new StringBuilder();
            if (!string.IsNullOrEmpty(msg))
            {
                sbMsg.AppendLine(msg);
            }
            if (ex != null)
            {
                sbMsg.Append(ex.ToString());
            }
            WriteLog(LogLevel.Error, type, sbMsg.ToString(), subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// エラーログ出力(ログ種類未指定)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Error(string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            Error(LogType.None, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// エラーログ出力(例外出力あり)(ログ種類未指定)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Error(string msg, Exception ex, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            Error(LogType.None, msg, ex, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// 致命的エラーログ出力
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Fatal(LogType type, string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            WriteLog(LogLevel.Fatal, type, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// 致命的ログ出力(例外出力あり)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Fatal(LogType type, string msg, Exception ex, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            StringBuilder sbMsg = new StringBuilder();
            sbMsg.Append(msg);
            if (!string.IsNullOrEmpty(msg))
            {
                sbMsg.AppendLine(msg);
            }
            sbMsg.Append(ex.ToString());
            WriteLog(LogLevel.Fatal, type, sbMsg.ToString(), subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// 致命的エラーログ出力(ログ種類未指定)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Fatal(string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            Fatal(LogType.None, msg, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// 致命的ログ出力(例外出力あり)(ログ種類未指定)
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="prefix">ファイル名接頭語</param>
        /// <param name="subFileName">ファイル名付加文字列</param>
        /// <param name="param">その他パラメータ</param>
        public void Fatal(string msg, Exception ex, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
        {
            Fatal(LogType.None, msg, ex, subDirName, prefix, subFileName, param);
        }

        /// <summary>
        /// 出力ファイル数チェック
        /// </summary>
        /// <param name="di">ディレクトリ情報</param>
        /// <param name="fileName">ログファイル名</param>
        /// <param name="baseFileName">基本ログファイル名</param>
        /// <param name="subFileName">サブファイル名</param>
        private void checkOutputFiles(DirectoryInfo di, string fileName, string baseFileName, string subFileName)
        {
            // 新規ファイル追加時かつ世代管理数が設定されている場合、出力ファイル数をチェック
            var logFiles = di.GetFiles(this.logFileName + "*." + this.fileExt, SearchOption.TopDirectoryOnly);
            var minDate = DateTime.Now;
            var minNo = int.MaxValue;
            FileInfo minFile = null;
            if (logFiles.Length > this.maxArchiveFiles)
            {
                // 一番古いファイルを削除
                foreach (var file in logFiles)
                {
                    if (file.Name.Equals(fileName))
                    {
                        continue;
                    }
                    // ファイル名=[baseFileName]_[subFileName]_[日付]_[連番].[fileExt]
                    string tmpName = file.Name.Replace(baseFileName, "").Replace("." + this.fileExt, "");
                    if (!string.IsNullOrWhiteSpace(subFileName))
                    {
                        tmpName.Replace("_" + subFileName, "");
                    }
                    var archiveInfo = tmpName.Split('_');
                    DateTime tmpDate;
                    int tmpNo;
                    if (archiveInfo.Length > 1)
                    {
                        // 日付、連番あり
                        if (DateTime.TryParseExact(archiveInfo[0], "_" + this.fileNameDateFmt, null, System.Globalization.DateTimeStyles.None, out tmpDate))
                        {
                            if (tmpDate.CompareTo(minDate) <= 0)
                            {
                                if (int.TryParse(archiveInfo[1], out tmpNo))
                                {
                                    if (tmpDate.CompareTo(minDate) < 0 || tmpNo < minNo)
                                    {
                                        minNo = tmpNo;
                                        minFile = file;
                                    }
                                }
                                minDate = tmpDate;
                            }
                        }
                    }
                    else
                    {
                        // 日付あり
                        if (DateTime.TryParseExact(tmpName, "_" + this.fileNameDateFmt, null, System.Globalization.DateTimeStyles.None, out tmpDate))
                        {
                            if (tmpDate.CompareTo(minDate) <= 0)
                            {
                                minDate = tmpDate;
                                minFile = file;
                            }
                        }
                        // 連番あり
                        else if (int.TryParse(tmpName, out tmpNo))
                        {
                            if (tmpNo < minNo)
                            {
                                minNo = tmpNo;
                                minFile = file;
                            }
                        }

                    }
                }
                if (minFile != null)
                {
                    minFile.Delete();
                }
            }
        }

        #region ログレベル
        /// <summary>
        /// ログレベルクラス
        /// </summary>
        public class LogLevel : IComparable<LogLevel>, IComparable, IEquatable<LogLevel>
        {
            /// <summary>トレース</summary>
            public static readonly LogLevel Trace = new LogLevel("Trace", "TRACE", 0);
            /// <summary>デバッグ</summary>
            public static readonly LogLevel Debug = new LogLevel("Debug", "DEBUG", 1);
            /// <summary>情報</summary>
            public static readonly LogLevel Info = new LogLevel("Info", "INFO ", 2);
            /// <summary>警告</summary>
            public static readonly LogLevel Warn = new LogLevel("Warn", "WARN ", 3);
            /// <summary>エラー</summary>
            public static readonly LogLevel Error = new LogLevel("Error", "ERROR", 4);
            /// <summary>致命的エラー</summary>
            public static readonly LogLevel Fatal = new LogLevel("Fatal", "FATAL", 5);
            /// <summary>OFF</summary>
            public static readonly LogLevel Off = new LogLevel("Off", "", 6);

            /// <summary>名称</summary>
            public string Name { get; private set; }
            /// <summary>ラベル</summary>
            public string Label { get; private set; }
            /// <summary>値</summary>
            public int Value { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="name"></param>
            /// <param name="val"></param>
            private LogLevel(string name, string label, int val)
            {
                this.Name = name;
                this.Label = label;
                this.Value = val;
            }

            /// <summary>
            /// ＝＝演算子
            /// </summary>
            /// <param name="level1"></param>
            /// <param name="level2"></param>
            /// <returns></returns>
            public static bool operator ==(LogLevel level1, LogLevel level2)
            {
                if (ReferenceEquals(level1, level2))
                    return true;
                else
                    return (level1 ?? LogLevel.Off).Equals(level2);
            }

            /// <summary>
            /// ！＝演算子
            /// </summary>
            /// <param name="level1"></param>
            /// <param name="level2"></param>
            /// <returns></returns>
            public static bool operator !=(LogLevel level1, LogLevel level2)
            {
                if (ReferenceEquals(level1, level2))
                    return false;
                else
                    return !(level1 ?? LogLevel.Off).Equals(level2);
            }

            /// <summary>
            /// ＞演算子
            /// </summary>
            /// <param name="level1"></param>
            /// <param name="level2"></param>
            /// <returns></returns>
            public static bool operator >(LogLevel level1, LogLevel level2)
            {
                if (ReferenceEquals(level1, level2))
                    return false;
                else
                    return (level1 ?? LogLevel.Off).CompareTo(level2) > 0;
            }

            /// <summary>
            /// ＞＝演算子
            /// </summary>
            /// <param name="level1"></param>
            /// <param name="level2"></param>
            /// <returns></returns>
            public static bool operator >=(LogLevel level1, LogLevel level2)
            {
                if (ReferenceEquals(level1, level2))
                    return true;
                else
                    return (level1 ?? LogLevel.Off).CompareTo(level2) >= 0;
            }

            /// <summary>
            /// ＜演算子
            /// </summary>
            /// <param name="level1"></param>
            /// <param name="level2"></param>
            /// <returns></returns>
            public static bool operator <(LogLevel level1, LogLevel level2)
            {
                if (ReferenceEquals(level1, level2))
                    return false;
                else
                    return (level1 ?? LogLevel.Off).CompareTo(level2) < 0;
            }

            /// <summary>
            /// ＜＝演算子
            /// </summary>
            /// <param name="level1"></param>
            /// <param name="level2"></param>
            /// <returns></returns>
            public static bool operator <=(LogLevel level1, LogLevel level2)
            {
                if (ReferenceEquals(level1, level2))
                    return true;
                else
                    return (level1 ?? LogLevel.Off).CompareTo(level2) <= 0;
            }

            /// <summary>
            /// ログレベル名からログレベルを取得
            /// </summary>
            /// <param name="name">ログレベル名</param>
            /// <returns>ログレベル</returns>
            public static LogLevel FromName(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return LogLevel.Off;
                }

                switch (name.ToUpper())
                {
                    case "TRACE":
                        return LogLevel.Trace;
                    case "DEBUG":
                        return LogLevel.Debug;
                    case "INFO":
                    case "INFOMATION":
                        return LogLevel.Info;
                    case "WARN":
                    case "WARNING":
                        return LogLevel.Warn;
                    case "ERR":
                    case "ERROR":
                        return LogLevel.Error;
                    case "FATAL":
                        return LogLevel.Fatal;
                    default:
                        return LogLevel.Off;
                }
            }

            /// <summary>
            /// ログレベル値からログレベルを取得
            /// </summary>
            /// <param name="val">ログレベル値(int型)</param>
            /// <returns>ログレベル</returns>
            public static LogLevel FromValue(int val)
            {
                switch (val)
                {
                    case 0:
                        return LogLevel.Trace;
                    case 1:
                        return LogLevel.Debug;
                    case 2:
                        return LogLevel.Info;
                    case 3:
                        return LogLevel.Warn;
                    case 4:
                        return LogLevel.Error;
                    case 5:
                        return LogLevel.Fatal;
                    default:
                        return LogLevel.Off;
                }
            }

            /// <summary>
            /// ログレベル値からログレベルを取得
            /// </summary>
            /// <param name="val">ログレベル値(文字列)</param>
            /// <returns>ログレベル</returns>
            public static LogLevel FromValue(string val)
            {
                if (string.IsNullOrWhiteSpace(val))
                {
                    return LogLevel.Off;
                }
                return FromValue(Convert.ToInt32(val));
            }

            /// <summary>
            /// ハッシュコードの取得
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this.Value;
            }

            /// <summary>
            /// Equalsメソッド
            /// </summary>
            /// <param name="obj">比較対象オブジェクト</param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return Equals(obj as LogLevel);
            }

            /// <summary>
            /// Equalsメソッド
            /// </summary>
            /// <param name="other">比較対象LogLevel</param>
            /// <returns></returns>
            public bool Equals(LogLevel other)
            {
                return this.Value == (other ?? LogLevel.Off).Value;
            }

            /// <summary>
            /// CompareToメソッド
            /// </summary>
            /// <param name="obj">比較対象オブジェクト</param>
            /// <returns></returns>
            public int CompareTo(object obj)
            {
                return CompareTo((LogLevel)obj);
            }

            /// <summary>
            /// CompareToメソッド
            /// </summary>
            /// <param name="other">比較対象LogLevel</param>
            /// <returns></returns>
            public int CompareTo(LogLevel other)
            {
                return this.Value - (other ?? LogLevel.Off).Value;
            }

        }
        #endregion

        #region ログ種類
        /// <summary>
        /// ログ種類クラス
        /// </summary>
        public class LogType : IComparable<LogType>, IComparable, IEquatable<LogType>
        {
            /// <summary>なし</summary>
            public static readonly LogType None = new LogType("None", "", 0);
            /// <summary>ログイン</summary>
            public static readonly LogType Login = new LogType("Login", "LOGIN", 1);
            /// <summary>アクション(イベント)</summary>
            public static readonly LogType Event = new LogType("Event", "EVENT", 2);
            /// <summary>発行SQL</summary>
            public static readonly LogType Sql = new LogType("Sql", "SQL  ", 3);
            /// <summary>例外エラー</summary>
            public static readonly LogType Error = new LogType("Error", "ERROR", 4);

            /// <summary>名称</summary>
            public string Name { get; private set; }
            /// <summary>ラベル</summary>
            public string Label { get; private set; }
            /// <summary>値</summary>
            public int Value { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="name"></param>
            /// <param name="val"></param>
            private LogType(string name, string label, int val)
            {
                this.Name = name;
                this.Label = label;
                this.Value = val;
            }

            /// <summary>
            /// ＝＝演算子
            /// </summary>
            /// <param name="type1"></param>
            /// <param name="type2"></param>
            /// <returns></returns>
            public static bool operator ==(LogType type1, LogType type2)
            {
                if (ReferenceEquals(type1, type2))
                    return true;
                else
                    return (type1 ?? LogType.None).Equals(type2);
            }

            /// <summary>
            /// ！＝演算子
            /// </summary>
            /// <param name="type1"></param>
            /// <param name="type2"></param>
            /// <returns></returns>
            public static bool operator !=(LogType type1, LogType type2)
            {
                if (ReferenceEquals(type1, type2))
                    return false;
                else
                    return !(type1 ?? LogType.None).Equals(type2);
            }

            /// <summary>
            /// ＞演算子
            /// </summary>
            /// <param name="type1"></param>
            /// <param name="type2"></param>
            /// <returns></returns>
            public static bool operator >(LogType type1, LogType type2)
            {
                if (ReferenceEquals(type1, type2))
                    return false;
                else
                    return (type1 ?? LogType.None).CompareTo(type2) > 0;
            }

            /// <summary>
            /// ＞＝演算子
            /// </summary>
            /// <param name="type1"></param>
            /// <param name="type2"></param>
            /// <returns></returns>
            public static bool operator >=(LogType type1, LogType type2)
            {
                if (ReferenceEquals(type1, type2))
                    return true;
                else
                    return (type1 ?? LogType.None).CompareTo(type2) >= 0;
            }

            /// <summary>
            /// ＜演算子
            /// </summary>
            /// <param name="type1"></param>
            /// <param name="type2"></param>
            /// <returns></returns>
            public static bool operator <(LogType type1, LogType type2)
            {
                if (ReferenceEquals(type1, type2))
                    return false;
                else
                    return (type1 ?? LogType.None).CompareTo(type2) < 0;
            }

            /// <summary>
            /// ＜＝演算子
            /// </summary>
            /// <param name="type1"></param>
            /// <param name="type2"></param>
            /// <returns></returns>
            public static bool operator <=(LogType type1, LogType type2)
            {
                if (ReferenceEquals(type1, type2))
                    return true;
                else
                    return (type1 ?? LogType.None).CompareTo(type2) <= 0;
            }

            /// <summary>
            /// ログ種類名からログ種類を取得
            /// </summary>
            /// <param name="name">ログ種類名</param>
            /// <returns>ログ種類</returns>
            public static LogType FromName(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return LogType.None;
                }

                switch (name.ToUpper())
                {
                    case "LOGIN":
                        return LogType.Login;
                    case "EVENT":
                        return LogType.Event;
                    case "SQL":
                    case "ERR":
                    case "ERROR":
                        return LogType.Error;
                    default:
                        return LogType.None;
                }
            }

            /// <summary>
            /// ログ種類値からログ種類を取得
            /// </summary>
            /// <param name="val">ログ種類値(int型)</param>
            /// <returns>ログ種類</returns>
            public static LogType FromValue(int val)
            {
                switch (val)
                {
                    case 1:
                        return LogType.Login;
                    case 2:
                        return LogType.Event;
                    case 3:
                        return LogType.Sql;
                    case 4:
                        return LogType.Error;
                    default:
                        return LogType.None;
                }
            }

            /// <summary>
            /// ログレベル値からログレベルを取得
            /// </summary>
            /// <param name="val">ログレベル値(文字列)</param>
            /// <returns>ログレベル</returns>
            public static LogType FromValue(string val)
            {
                if (string.IsNullOrWhiteSpace(val))
                {
                    return LogType.None;
                }
                return FromValue(Convert.ToInt32(val));
            }

            /// <summary>
            /// ハッシュコードの取得
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this.Value;
            }

            /// <summary>
            /// Equalsメソッド
            /// </summary>
            /// <param name="obj">比較対象オブジェクト</param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return Equals(obj as LogType);
            }

            /// <summary>
            /// Equalsメソッド
            /// </summary>
            /// <param name="other">比較対象LogLevel</param>
            /// <returns></returns>
            public bool Equals(LogType other)
            {
                return this.Value == (other ?? LogType.None).Value;
            }

            /// <summary>
            /// CompareToメソッド
            /// </summary>
            /// <param name="obj">比較対象オブジェクト</param>
            /// <returns></returns>
            public int CompareTo(object obj)
            {
                return CompareTo((LogType)obj);
            }

            /// <summary>
            /// CompareToメソッド
            /// </summary>
            /// <param name="other">比較対象LogLevel</param>
            /// <returns></returns>
            public int CompareTo(LogType other)
            {
                return this.Value - (other ?? LogType.None).Value;
            }

        }
        #endregion

        #region 日付種類
        public class DateType
        {
            /// <summary>未設定</summary>
            public static readonly DateType None = new DateType("None", 0, "");
            /// <summary>年</summary>
            public static readonly DateType Year = new DateType("Year", 1, "yyyy");
            /// <summary>月</summary>
            public static readonly DateType Month = new DateType("Month", 2, "yyyyMM");
            /// <summary>日</summary>
            public static readonly DateType Day = new DateType("Day", 3, "yyyyMMdd");
            /// <summary>時間</summary>
            public static readonly DateType Hour = new DateType("Hour", 4, "yyyyMMddHH");
            /// <summary>分</summary>
            public static readonly DateType Minute = new DateType("Minute", 5, "yyyyMMddHHss");

            /// <summary>名称</summary>
            public string Name { get; private set; }
            /// <summary>値</summary>
            public int Value { get; private set; }
            /// <summary>デフォルトフォーマット</summary>
            public string DefaultFormat { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="name"></param>
            /// <param name="val"></param>
            /// <param name="fmt"></param>
            private DateType(string name, int val, string fmt)
            {
                this.Name = name;
                this.Value = val;
                this.DefaultFormat = fmt;
            }

            /// <summary>
            /// 日付種類名から日付種類を取得
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public static DateType FromName(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return DateType.None;
                }

                switch (name.ToUpper())
                {
                    case "YEAR":
                        return DateType.Year;
                    case "MONTH":
                        return DateType.Month;
                    case "DAY":
                        return DateType.Day;
                    case "HOUR":
                        return DateType.Hour;
                    case "MINUTE":
                        return DateType.Minute;
                    default:
                        return DateType.None;
                }
            }

            /// <summary>
            /// 日付種類値から日付種類を取得
            /// </summary>
            /// <param name="val">日付種類値</param>
            /// <returns></returns>
            public static DateType FromValue(int val)
            {
                switch (val)
                {
                    case 1:
                        return DateType.Year;
                    case 2:
                        return DateType.Month;
                    case 3:
                        return DateType.Day;
                    case 4:
                        return DateType.Hour;
                    case 5:
                        return DateType.Minute;
                    default:
                        return DateType.None;
                }
            }

            /// <summary>
            /// 日付種類値から日付種類を取得
            /// </summary>
            /// <param name="val">日付種類値(文字列)</param>
            /// <returns></returns>
            public static DateType FromValue(string val)
            {
                if (string.IsNullOrWhiteSpace(val))
                {
                    return DateType.None;
                }
                return FromValue(Convert.ToInt32(val));
            }
        }
            #endregion

        }
}
