using CommonSTDUtil.CommonLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonSTDUtil.CommonLogger
{
    public class CommonLogger : CommonLoggerBase
    {
        private static Dictionary<string, CommonLogger> singletonDictionary = null;

        private readonly object lockObj = new object();

        /// <summary>
        /// インスタンス生成
        /// </summary>
        /// <param name="loggerName">ロガー名</param>
        /// <param name="rootPath">ログ出力先ディレクトリルートパス</param>
        /// <returns></returns>
        public static new CommonLogger GetInstance(string loggerName)
        {
            if (singletonDictionary == null)
            {
                singletonDictionary = new Dictionary<string, CommonLogger>();
            }

            if (singletonDictionary.ContainsKey(loggerName))
            {
                return singletonDictionary[loggerName];
            }
            else
            {
                var singleton = new CommonLogger(loggerName);
                singletonDictionary.Add(loggerName, singleton);
                return singleton;
            }
        }

        private CommonLogger()
        {

        }

        private CommonLogger(string loggerName) : base(loggerName)
        {
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="msg">メッセージ文字列</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="subFileName">サブファイル名</param>
        /// <param name="param">その他パラメータ</param>
        public override void WriteLog(LogLevel level, LogType type, string msg, string subDirName = "", string prefix = "", string subFileName = "", dynamic param = null)
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
                        // サブディレクトリ名に年月日フォルダ名を設定
                        var dateDirName = Path.Combine(subDirName, now.ToString("yyyyMMdd"));
                        WriteLogToFile(level, type, msg, now, dateDirName, prefix, subFileName);
                        break;
                }
            }
            catch
            {
                // 何もしない
            }
        }

        /// <summary>
        /// ログインログ出力
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="userId">ログインユーザID</param>
        public void LoginLog(string factoryId, string userId)
        {
            //「年月日時分秒」+ "[LOGIN]" +「ユーザID」					
            string message = string.Format("UserID:{0}", userId);
            Info(LogType.Login, message, "", factoryId);
        }

        /// <summary>
        /// アクション(イベント)ログ出力
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="conductId">機能ID</param>
        /// <param name="ctrlId">コントロールID</param>
        public void ActionLog(string factoryId, string userId, string conductId, string ctrlId)
        {
            //「年月日時分秒」+ "[EVENT]" +「ユーザID」+「機能ID」+「コントロールID」		
            string message = string.Format("UserID:{0} ConductID:{1} ControlID:{2}", userId, conductId, ctrlId);
            Info(LogType.Event, message, "", factoryId);
        }

        /// <summary>
        /// SQLログ出力
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="sql">SQL文字列</param>
        public void SqlLog(string factoryId, string userId, string sql)
        {
            //「年月日時分秒」+ "[SQL  ]" +「ユーザID」+「SQL文字列」	
            string message = string.Format("UserID:{0} {1}", userId, sql);
            Debug(LogType.Sql, message, "", factoryId);
        }

        /// <summary>
        /// エラーログ出力
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="msg">メッセージ</param>
        public void ErrorLog(string factoryId, string userId, string msg)
        {
            //「年月日時分秒」+ "[ERROR]" +「ユーザID」+「エラーメッセージ」	
            string message = string.Format("UserID:{0} {1}", userId, msg);
            Error(LogType.Error, message, "", factoryId);
        }

        /// <summary>
        /// 例外エラーログ出力
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        public void ErrorLog(string factoryId, string userId, string msg, Exception ex)
        {
            //「年月日時分秒」+ "[ERROR]" +「ユーザID」+「エラーメッセージ」+「例外メッセージ」	
            string message = string.Format("UserID:{0} {1}", userId, msg);
            Error(LogType.Error, message, ex, "", factoryId);
        }

        /// <summary>
        /// デバッグログ出力
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        public void DebugLog(string factoryId, string userId, string msg)
        {
            //「年月日時分秒」+ "[DEBUG]" +「ユーザID」+「メッセージ」
            string message = string.Format("UserID:{0} {1}", userId, msg);
            Debug(LogType.None, message, "", factoryId);
        }

    }
}
