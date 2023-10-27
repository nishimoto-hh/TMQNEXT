using Newtonsoft.Json;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonSTDUtil.CommonDBManager
{
    public class TraceDbProfiler : IDbProfiler
    {
        public bool IsActive => true;

        private Stopwatch stopwatch;
        private string commandText;
        private IDictionary<string, object> parameters;
        CommonLogger.CommonLogger logger;
        private string factoryId;
        private string userId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガーオブジェクト</param>
        /// <param name="factoryId">本務工場ID</param>
        /// <param name="userId">ユーザID</param>
        public TraceDbProfiler(CommonLogger.CommonLogger logger, string factoryId, string userId)
        {
            this.logger = logger;
            this.factoryId = factoryId;
            this.userId = userId;
        }

        /// <summary>
        /// SQLコマンド実行開始イベント
        /// </summary>
        /// <param name="profiledDbCommand"></param>
        /// <param name="executeType"></param>
        public void ExecuteStart(IDbCommand profiledDbCommand, SqlExecuteType executeType)
        {
            stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// SQLコマンド実行終了イベント
        /// </summary>
        /// <param name="profiledDbCommand"></param>
        /// <param name="executeType"></param>
        /// <param name="reader"></param>
        public void ExecuteFinish(IDbCommand profiledDbCommand, SqlExecuteType executeType, DbDataReader reader)
        {
            setCommandTexts(profiledDbCommand);

            // ※Readerコマンドの場合はReaderFinishイベントでログ出力を行う
            if (executeType != SqlExecuteType.Reader)
            {
                stopwatch.Stop();
                // SQLログ出力
                logger.SqlLog(this.factoryId, this.userId, getLogText(executeType));
            }
        }

        /// <summary>
        /// Readerコマンド実行終了イベント
        /// </summary>
        /// <param name="reader"></param>
        public void ReaderFinish(IDataReader reader)
        {
            stopwatch.Stop();
            //SQLログ出力
            logger.SqlLog(this.factoryId, this.userId, getLogText(SqlExecuteType.Reader));
        }

        /// <summary>
        /// エラー発生時イベント
        /// </summary>
        /// <param name="profiledDbCommand"></param>
        /// <param name="executeType"></param>
        /// <param name="exception"></param>
        public void OnError(IDbCommand profiledDbCommand, SqlExecuteType executeType, Exception exception)
        {
            setCommandTexts(profiledDbCommand);
            stopwatch.Stop();
            // エラーログ出力
            logger.ErrorLog(this.factoryId, this.userId, getLogText(executeType, exception));
        }

        /// <summary>
        /// コマンド文字列の設定
        /// </summary>
        /// <param name="profiledDbCommand"></param>
        private void setCommandTexts(IDbCommand profiledDbCommand)
        {
            commandText = profiledDbCommand.CommandText;
            if (profiledDbCommand.Parameters != null && profiledDbCommand.Parameters.Count > 0)
            {
                parameters = new Dictionary<string, object>();
                foreach (DbParameter param in profiledDbCommand.Parameters)
                {
                    parameters.Add(param.ParameterName, param.Value);
                }
            }
        }

        /// <summary>
        /// ログ出力文字列の取得
        /// </summary>
        /// <param name="executeType">SQLステートメントの種類(NonQuery/Scaler/Reader)</param>
        /// <param name="exception">例外オブジェクト</param>
        /// <returns></returns>
        private string getLogText(SqlExecuteType executeType, Exception exception = null)
        {
            return JsonConvert.SerializeObject(
                new
                {
                    Type = executeType.ToString(),
                    ms = stopwatch.ElapsedMilliseconds,
                    Exception = exception?.Message,
                    Command = commandText.MinifySql(),
                    Params = parameters,
                },
                Formatting.None, // JSONを読みやすいように整形しない
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore // nullは出力しない
                }
                );
        }
    }

    /// <summary>
    /// stringの拡張クラス
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 正規表現
        /// </summary>
        public static string RegexReplace(this string text, string regex, string replacement, RegexOptions options = RegexOptions.None)
        {
            return new Regex(regex, options).Replace(text, replacement);
        }

        /// <summary>
        /// SQL Queryの文字列を短縮化 (不要な改行、空白を除去)
        /// </summary>
        public static string MinifySql(this string sql)
        {
            return sql
                .RegexReplace(@"--(.*)", @"/*$1*/") // --コメント を /*コメント*/ に変換
                .RegexReplace(@"\s+", " "); // スペース、タブ、改行の連続を1つのスペースに置換
        }
    }
}
