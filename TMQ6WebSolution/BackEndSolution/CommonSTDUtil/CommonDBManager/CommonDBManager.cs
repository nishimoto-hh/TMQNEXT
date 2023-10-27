using Dapper;
using Jiifureit.Dapper.OutsideSql;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CommonSTDUtil.CommonDBManager
{
    public class CommonDBManager
    {
        #region　定数
        /// <summary>
        /// DB種類
        /// </summary>
        public enum DBType : int
        {
            /// <summary>不明</summary>
            UnKnown = -1,
            /// <summary>SQLServer</summary>
            SQLServer = 0,
            /// <summary>PostgreSQL</summary>
            PostgreSQL = 1,
            /// <summary>Oracle</summary>
            Oracle = 2
        }
        #endregion

        private string rootPath;
        private CommonLogger.CommonLogger logger;
        private string factoryId;
        private string userId;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonDBManager()
        {
            this.Connection = null;
            this.ConnectionSetting = CommonWebTemplate.AppCommonObject.Config.ConnectionSettings.GetDefaultConnectionStringSetting();
            this.TimeOutSeconds = getTimeOutSeconds();
            this.rootPath = string.Empty;
            this.transaction = null;
            this.IsExistsTransaction = false;
            this.logger = null;
            this.factoryId = string.Empty;
            this.userId = string.Empty;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="userId"></param>
        public CommonDBManager(CommonLogger.CommonLogger logger, string factoryId, string userId) : this()
        {
            this.logger = logger;
            this.factoryId = factoryId;
            this.userId = userId;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">ルートディレクトリ物理パス</param>
        public CommonDBManager(string path, CommonLogger.CommonLogger logger, string factoryId, string userId) : this(logger, factoryId, userId)
        {
            this.rootPath = path;
        }
        #endregion

        #region プロパティ
        /// <summary>DB接続</summary>
        public IDbConnection Connection { get; private set; }
        /// <summary>DB接続文字列</summary>
        public CommonWebTemplate.ConnectionStringSetting ConnectionSetting { get; private set; }
        /// <summary>DB種類</summary>
        public DBType DbType
        {
            get
            {
                if (this.ConnectionSetting != null)
                {
                    string name = this.ConnectionSetting.ProviderName.ToLower();
                    if (name.Contains("sqlclient"))
                    {
                        return DBType.SQLServer;
                    }
                    else if (name.Contains("npgsql"))
                    {
                        return DBType.PostgreSQL;
                    }
                    else if (name.Contains("oracle"))
                    {
                        return DBType.Oracle;
                    }
                    else
                    {
                        return DBType.UnKnown;
                    }
                }
                else
                {
                    return DBType.UnKnown;
                }
            }
        }

        /// <summary>DBオープン状態(true:オープン済/false:クローズ)</summary>
        public bool IsOpened
        {
            get
            {
                return this.Connection != null && this.Connection.State == ConnectionState.Open;
            }
        }

        /// <summary>外部SQLファイル格納先フォルダルートパス</summary>
        public string SqlRootPath
        {
            get
            {
                if (!string.IsNullOrEmpty(this.rootPath))
                {
                    return Path.Combine(this.rootPath, "sql");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// タイムアウト秒
        /// </summary>
        public int? TimeOutSeconds { get; private set; }

        /// <summary>トランザクション宣言</summary>
        private System.Data.IDbTransaction transaction { get; set; }
        /// <summary>トランザクション宣言の有無(true:宣言済/false:未宣言、クローズ済)</summary>
        public bool IsExistsTransaction { get; private set; }
        #endregion

        #region priveteメソッド
        /// <summary>
        /// DB接続の取得
        /// </summary>
        /// <returns></returns>
        private bool getConnection()
        {
            try
            {
                if (this.ConnectionSetting == null)
                {
                    return false;
                }

                try
                {
                    // 接続文字列のプロバイダ名から情報を取得
                    var factory = DbProviderFactories.GetFactory(this.ConnectionSetting.ProviderName);
                    if (factory == null)
                    {
                        return false;
                    }

                    // DB接続を生成
                    this.Connection = new ProfiledDbConnection(factory.CreateConnection(), new TraceDbProfiler(this.logger, this.factoryId, this.userId));
                }
                catch
                {
                    switch (this.DbType)
                    {
                        case DBType.SQLServer:
                            this.Connection = new ProfiledDbConnection(new System.Data.SqlClient.SqlConnection(), new TraceDbProfiler(this.logger, this.factoryId, this.userId));
                            break;
                        case DBType.PostgreSQL:
                            this.Connection = new ProfiledDbConnection(new Npgsql.NpgsqlConnection(), new TraceDbProfiler(this.logger, this.factoryId, this.userId));
                            break;
                        case DBType.Oracle:
                            this.Connection = new ProfiledDbConnection(new Oracle.ManagedDataAccess.Client.OracleConnection(), new TraceDbProfiler(this.logger, this.factoryId, this.userId));
                            break;
                        default:
                            this.Connection = null;
                            return false;
                    }
                }
                this.Connection.ConnectionString = this.ConnectionSetting.ConnectionString;

                return true;
            }
            catch
            {
                Close();
                throw;
            }
        }

        /// <summary>
        /// 外部SQLファイルパス取得処理
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <returns>SQLファイルパス</returns>
        private string getSqlPath(string sqlName, string subDirName = "")
        {
            string fileName = sqlName + ".sql";
            string dirPath = this.SqlRootPath;

            // ルートの物理パスからファイルパスを取得する
            if (!string.IsNullOrEmpty(subDirName))
            {
                dirPath = Path.Combine(dirPath, subDirName);
            }
            return Path.Combine(dirPath, fileName);
        }

        /// <summary>
        /// タイムアウト時間を取得
        /// </summary>
        /// <returns></returns>
        private int? getTimeOutSeconds()
        {
            // プロパティの値
            var value = CommonWebTemplate.AppCommonObject.Config.AppSettings.DBTimeOutSeconds;
            // 数値に変換
            if (!value.HasValue)
            {
                // 失敗した場合、Null(タイムアウトはデフォルト)
                return null;
            }
            // 成功した場合、プロパティの値
            return value;
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// DB接続オープン
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                // DB接続の取得
                if (this.Connection == null)
                {
                    if (!getConnection())
                    {
                        return false;
                    }
                }

                if (!this.IsOpened)
                {
                    // DB接続オープン
                    this.Connection.Open();
                }
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        /// <summary>
        /// DB接続クローズ
        /// </summary>
        public void Close()
        {
            if (this.IsOpened)
            {
                this.Connection.Close();
                this.Connection.Dispose();
            }
            this.Connection = null;
        }

        #region SQL文字列指定
        /// <summary>
        /// 検索処理(結果格納クラス指定＆SQL文字列指定)
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public T GetEntity<T>(string sql, object param = null)
        {
            T result = default(T);

            if (!this.IsOpened)
            {
                if (!Connect()) { return default(T); }
            }

            result = this.Connection.QueryFirstOrDefault<T>(sql, param, this.transaction, commandTimeout: this.TimeOutSeconds);

            return result;
        }

        /// <summary>
        /// 検索処理(結果格納クラス指定＆SQL文字列指定)
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public T GetEntityByDataClass<T>(string sql, object param = null)
        {
            T result = default(T);

            if (!this.IsOpened)
            {
                if (!Connect()) { return default(T); }
            }

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            result = this.Connection.QueryFirstOrDefault<T>(sql, param, this.transaction, commandTimeout: this.TimeOutSeconds);

            return result;
        }

        /// <summary>
        /// 検索処理(結果格納クラス未指定＆SQL文字列指定)
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public dynamic GetEntity(string sql, object param = null)
        {
            dynamic result = null;

            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            result = this.Connection.QueryFirstOrDefault(sql, param, this.transaction, commandTimeout: this.TimeOutSeconds);

            return result;
        }

        /// <summary>
        /// 検索結果件数取得処理(SQL文字列指定)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public int GetCount(string sql, object param = null)
        {
            int cnt = -1;
            if (!this.IsOpened)
            {
                if (!Connect()) { return -1; }
            }

            cnt = Convert.ToInt32(this.Connection.ExecuteScalar(sql, param, transaction: this.transaction, commandTimeout: this.TimeOutSeconds));

            return cnt;
        }

        /// <summary>
        /// 一覧検索処理(結果格納クラス指定＆SQL文字列指定)
        /// </summary>
        /// <typeparam name="T">結果格納クラス</typeparam>
        /// <param name="sql">SQL文字列</param>
        /// <param name="param">パラメータ</param>
        /// <returns></returns>
        public IList<T> GetList<T>(string sql, object param = null)
        {
            IList<T> result = null;

            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            result = this.Connection.Query<T>(sql, param, this.transaction, commandTimeout: this.TimeOutSeconds).ToList<T>();

            return result;
        }

        /// <summary>
        /// 一覧検索処理(結果格納クラス未指定＆SQL文字列指定)
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public IList<dynamic> GetList(string sql, object param = null)
        {
            IList<dynamic> result = null;

            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            result = this.Connection.Query(sql, param, this.transaction, commandTimeout: this.TimeOutSeconds).ToList();

            return result;
        }

        /// <summary>
        /// 一覧検索処理(結果格納クラス指定＆SQL文字列指定)
        /// </summary>
        /// <typeparam name="T">結果格納クラス</typeparam>
        /// <param name="sql">SQL文字列</param>
        /// <param name="param">パラメータ</param>
        /// <returns></returns>
        public IList<T> GetListByDataClass<T>(string sql, object param = null)
        {
            IList<T> result = null;

            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            result = this.Connection.Query<T>(sql, param, this.transaction, commandTimeout: this.TimeOutSeconds).ToList<T>();

            return result;
        }

        /// <summary>
        /// 登録/更新処理(SQL文字列指定)
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="param">登録パラメータ</param>
        /// <returns></returns>
        public int Regist(string sql, object param = null)
        {
            if (!this.IsOpened)
            {
                if (!Connect()) { return -1; }
            }

            return this.Connection.Execute(sql, param, transaction: this.transaction, commandTimeout: this.TimeOutSeconds);
        }

        /// <summary>
        /// 登録/更新処理(SQL文字列指定＆キー取得)
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="isError">out エラー発生時True</param>
        /// <param name="param">登録パラメータ</param>
        /// <returns>登録/更新時のキー値</returns>
        public T RegistAndGetKeyValue<T>(string sql, out bool isError, object param = null)
        {
            isError = false;
            if (!this.IsOpened)
            {
                if (!Connect())
                {
                    isError = true;
                    return default(T);
                }
            }

            return (T)this.Connection.ExecuteScalar(sql, param, this.transaction, commandTimeout: this.TimeOutSeconds);
        }

        /// <summary>
        /// 登録/更新処理(SQL文字列指定＆キー取得)
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="param">登録パラメータ</param>
        /// <returns>登録/更新時のキー値(DateTable)</returns>
        public DataTable RegistAndGetKeyTable(string sql, object param = null)
        {
            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            var reader = this.Connection.ExecuteReader(sql, param, this.transaction, commandTimeout: this.TimeOutSeconds);
            DataTable table = new DataTable();
            table.Load(reader);

            return table;
        }

        /// <summary>
        /// SQL実行結果(IDataReader)取得処理(SQL文字列指定)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, object param = null)
        {
            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            return this.Connection.ExecuteReader(sql, param, this.transaction, commandTimeout: this.TimeOutSeconds);
        }
        #endregion

        #region 外部SQL使用
        /// <summary>
        /// 検索処理(結果格納クラス指定＆外部SQL名指定)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public T GetEntityByOutsideSql<T>(string sqlName, string subDirName = "", object param = null)
        {
            T result = default(T);

            if (!this.IsOpened)
            {
                if (!Connect()) { return default(T); }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                result = this.Connection.QueryFirstOrDefaultOutsideSql<T>(path, param, this.transaction, commandTimeout: this.TimeOutSeconds);
            }

            return result;
        }

        /// <summary>
        /// 検索処理(結果格納クラス指定＆外部SQL名指定)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public dynamic GetEntityByOutsideSql(string sqlName, string subDirName = "", object param = null)
        {
            dynamic result = null;

            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                result = this.Connection.QueryFirstOrDefaultOutsideSql(path, param, this.transaction, commandTimeout: this.TimeOutSeconds);
            }

            return result;
        }

        /// <summary>
        /// 検索処理(結果格納クラス指定＆外部SQL名指定)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public T GetEntityByOutsideSqlByDataClass<T>(string sqlName, string subDirName = "", object param = null)
        {
            T result = default(T);

            if (!this.IsOpened)
            {
                if (!Connect()) { return default(T); }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                result = this.Connection.QueryFirstOrDefaultOutsideSql<T>(path, param, this.transaction, commandTimeout: this.TimeOutSeconds);
            }

            return result;
        }

        /// <summary>
        /// 検索結果件数取得処理(外部SQL名指定)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public int GetCountByOutsideSql(string sqlName, string subDirName = "", object param = null)
        {
            int cnt = -1;
            if (!this.IsOpened)
            {
                if (!Connect()) { return -1; }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                cnt = Convert.ToInt32(this.Connection.ExecuteScalarOutsideSql(path, param, this.transaction, commandTimeout: this.TimeOutSeconds));
            }
            return cnt;
        }

        /// <summary>
        /// 一覧検索処理(結果格納クラス指定＆外部SQL名指定)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public IList<T> GetListByOutsideSql<T>(string sqlName, string subDirName = "", object param = null)
        {
            IList<T> result = null;

            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                result = this.Connection.QueryOutsideSql<T>(path, param, this.transaction, commandTimeout: this.TimeOutSeconds).ToList();
            }
            return result;
        }

        /// <summary>
        /// 一覧検索処理(結果格納クラス指定＆外部SQL名指定)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public IList<T> GetListByOutsideSqlByDataClass<T>(string sqlName, string subDirName = "", object param = null)
        {
            IList<T> result = null;

            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                result = this.Connection.QueryOutsideSql<T>(path, param, this.transaction, commandTimeout: this.TimeOutSeconds).ToList();
            }
            return result;
        }

        /// <summary>
        /// 一覧検索処理(結果格納クラス未指定＆外部SQL名指定)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">検索パラメータ</param>
        /// <returns></returns>
        public IList<dynamic> GetListByOutsideSql(string sqlName, string subDirName = "", object param = null)
        {
            IList<dynamic> result = null;

            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                result = this.Connection.QueryOutsideSql(path, param, this.transaction, commandTimeout: this.TimeOutSeconds).ToList();
            }
            return result;
        }

        /// <summary>
        /// 登録/更新処理(外部SQL名指定)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">登録パラメータ</param>
        /// <returns></returns>
        public int RegistByOutsideSql(string sqlName, string subDirName = "", object param = null)
        {
            if (!this.IsOpened)
            {
                if (!Connect()) { return -1; }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return -1;
            }
            return this.Connection.ExecuteOutsideSql(path, param, this.transaction, commandTimeout: this.TimeOutSeconds);
        }

        /// <summary>
        /// 登録/更新処理(外部SQL名指定＆キー取得)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">登録パラメータ</param>
        /// <returns>登録/更新時のキー値</returns>
        public object RegistAndGetKeyValueByOutsideSql(string sqlName, string subDirName = "", object param = null)
        {
            if (!this.IsOpened)
            {
                if (!Connect()) { return -1; }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return -1;
            }
            return this.Connection.ExecuteScalarOutsideSql(path, param, this.transaction, commandTimeout: this.TimeOutSeconds);
        }

        /// <summary>
        /// 登録/更新処理(外部SQL名指定＆キー取得)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">登録パラメータ</param>
        /// <returns>登録/更新時のキー値(DateTable)</returns>
        public DataTable RegistAndGetKeyTableByOutsideSql(string sqlName, string subDirName = "", object param = null)
        {
            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return null;
            }

            var reader = this.Connection.ExecuteReaderOutsideSql(path, param, this.transaction, commandTimeout: this.TimeOutSeconds);
            DataTable table = new DataTable();
            table.Load(reader);

            return table;
        }

        /// <summary>
        /// SQL実行結果(IDataReader)取得処理(外部SQL名指定)
        /// </summary>
        /// <param name="sqlName">外部SQL名</param>
        /// <param name="subDirName">外部SQL格納先サブディレクトリ名</param>
        /// <param name="param">実行パラメータ</param>
        /// <returns>SQL実行結果(IDataReader)</returns>
        public IDataReader ExecuteReaderByOutsideSql(string sqlName, string subDirName = "", object param = null)
        {
            if (!this.IsOpened)
            {
                if (!Connect()) { return null; }
            }

            string path = getSqlPath(sqlName, subDirName);
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return null;
            }

            return this.Connection.ExecuteReaderOutsideSql(path, param, this.transaction, commandTimeout: this.TimeOutSeconds);
        }
        #endregion

        #region トランザクション関連
        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void BeginTransaction()
        {
            if (this.IsExistsTransaction)
            {
                return;
            }
            this.transaction = this.Connection.BeginTransaction();
            this.IsExistsTransaction = true;
        }
        /// <summary>
        /// トランザクション終了
        /// </summary>
        /// <remarks>ロールバックしてから終了</remarks>
        public void EndTransaction()
        {
            if (!this.IsExistsTransaction)
            {
                return;
            }
            this.transaction.Dispose();
            this.IsExistsTransaction = false;
        }
        /// <summary>
        /// トランザクションをコミット
        /// </summary>
        /// <remarks>終了はしないので別途終了してください</remarks>
        public void Commit()
        {
            if (!this.IsExistsTransaction)
            {
                return;
            }
            this.transaction.Commit();
        }
        /// <summary>
        /// トランザクションをロールバック
        /// </summary>
        /// <remarks>終了はしないので別途終了してください</remarks>
        public void RollBack()
        {
            if (!this.IsExistsTransaction)
            {
                return;
            }
            this.transaction.Rollback();
        }
        #endregion
        #endregion
    }
}
