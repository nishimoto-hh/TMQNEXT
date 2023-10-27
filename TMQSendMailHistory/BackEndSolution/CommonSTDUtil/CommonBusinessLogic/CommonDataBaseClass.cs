using System;
using System.Collections.Generic;

using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonSTDUtil
{
    public class CommonDataBaseClass
    {
        /// <summary>
        /// 検索条件格納クラス(共通)
        /// </summary>
        public class SearchCommonClass
        {
            /// <summary>Gets or sets １ページ当たりの件数</summary>
            /// <value>１ページ当たりの件数</value>
            public int? PageSize { get; set; }
            /// <summary>Gets or sets ページ番号</summary>
            /// <value>ページ番号</value>
            public int? PageNumber { get; set; }
            /// <summary>Gets or sets オフセット</summary>
            /// <value>オフセット</value>
            public int? Offset { get; set; }
            /// <summary>Gets or sets a value indicating whether gets or sets 件数</summary>
            /// <value>件数</value>
            public bool IsCount { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
        }

        /// <summary>
        /// テーブル格納クラス(共通)
        /// </summary>
        public class CommonTableItem
        {
            /// <summary>Gets or sets 更新シリアルID</summary>
            /// <value>更新シリアルID</value>
            public int UpdateSerialid { get; set; }
            /// <summary>Gets or sets 登録日時</summary>
            /// <value>登録日時</value>
            public DateTime InsertDatetime { get; set; }
            /// <summary>Gets or sets 登録者ID</summary>
            /// <value>登録者ID</value>
            public int InsertUserId { get; set; }
            /// <summary>Gets or sets 更新日時</summary>
            /// <value>更新日時</value>
            public DateTime UpdateDatetime { get; set; }
            /// <summary>Gets or sets 更新者ID</summary>
            /// <value>更新者ID</value>
            public int UpdateUserId { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public bool DeleteFlg { get; set; }
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
        }

        /// <summary>
        /// ファイル入出力定義マスタ
        /// </summary>
        public class ComInoutFileDefineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ComInoutFileDefineEntity()
            {
                TableName = "com_inout_file_define";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機能ID</summary>
            /// <value>機能ID</value>
            public string Conductid { get; set; }
            /// <summary>Gets or sets ファイル管理№</summary>
            /// <value>ファイル管理№</value>
            public int Fileno { get; set; }
            /// <summary>Gets or sets テンプレートファイル名</summary>
            /// <value>テンプレートファイル名</value>
            public string Templatename { get; set; }
            /// <summary>Gets or sets 共通使用フラグ</summary>
            /// <value>共通使用フラグ</value>
            public bool? Comuseflg { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remarks { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機能ID</summary>
                /// <value>機能ID</value>
                public string Conductid { get; set; }
                /// <summary>Gets or sets ファイル管理№</summary>
                /// <value>ファイル管理№</value>
                public int Fileno { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(string conductid, int fileno)
                {
                    Conductid = conductid;
                    Fileno = fileno;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.Conductid, this.Fileno);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public ComInoutFileDefineEntity GetEntity(string conductid, int fileno, ComDB db)
            {
                ComInoutFileDefineEntity.PrimaryKey condition = new ComInoutFileDefineEntity.PrimaryKey(conductid, fileno);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<ComInoutFileDefineEntity>(getEntitySql);
            }
        }


        /// <summary>
        /// ファイル入力定義の条件クラス
        /// </summary>
        public class InputDefineCondition : CommonTableItem
        {
            /// <summary>Gets or sets 帳票id</summary>
            /// <value>帳票id</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int SheetNo { get; set; }
            /// <summary>Gets or sets コントロールグループid</summary>
            /// <value>コントロールグループid</value>
            public string ControlGroupId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
        }

        /// <summary>
        /// ファイル入力管理クラス
        /// </summary>
        public class InputDefine
        {
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int SheetNo { get; set; }
            /// <summary>Gets or sets 入力方式</summary>
            /// <value>入力方式</value>
            public int DataDirection { get; set; }
            /// <summary>Gets or sets レコード行数</summary>
            /// <value>レコード行数</value>
            public int RecordCount { get; set; }
            /// <summary>Gets or sets コントロールタイプ</summary>
            /// <value>コントロールタイプ</value>
            public string ControlType { get; set; }
            /// <summary>Gets or sets 取込開始列番号</summary>
            /// <value>取込開始列番号</value>
            public int StartColumnNo { get; set; }
            /// <summary>Gets or sets 取込開始行番号</summary>
            /// <value>取込開始行番号</value>
            public int StartRowNo { get; set; }
            /// <summary>Gets or sets 取込時必須</summary>
            /// <value>取込時必須</value>
            public bool? RequiredFlg { get; set; }
            /// <summary>Gets or sets 最小値</summary>
            /// <value>最小値</value>
            public string MinimumValue { get; set; }
            /// <summary>Gets or sets 最大値</summary>
            /// <value>最大値</value>
            public string MaximumValue { get; set; }
            /// <summary>Gets or sets 桁数</summary>
            /// <value>桁数</value>
            public int? MaximumLength { get; set; }
            /// <summary>Gets or sets 項目名</summary>
            /// <value>項目名</value>
            public string ColumnName { get; set; }
            /// <summary>Gets or sets 項目別名</summary>
            /// <value>項目別名</value>
            public string AliasName { get; set; }
            /// <summary>Gets or sets データ種別</summary>
            /// <value>データ種別</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets 項目名翻訳</summary>
            /// <value>項目名翻訳</value>
            public string TranslationText { get; set; }
        }

        /// <summary>
        /// エラー内容
        /// </summary>
        public class UploadErrorInfo
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public List<int> RowNo { get; set; }
            /// <summary>Gets or sets 列番号</summary>
            /// <value>列番号</value>
            public List<int> ColumnNo { get; set; }
            /// <summary>Gets or sets 項目名</summary>
            /// <value>項目名</value>
            public string TranslationText { get; set; }
            /// <summary>Gets or sets エラー内容</summary>
            /// <value>エラー内容</value>
            public string ErrorInfo { get; set; }
            /// <summary>Gets or sets 入力方式</summary>
            /// <value>入力方式</value>
            public int DataDirection { get; set; }
        }

        /// <summary>
        /// 言語情報クラス
        /// </summary>
        public class LanguageInfo
        {
            /// <summary>Gets or sets 言語コード</summary>
            /// <value>言語コード</value>
            public string LanguageCode { get; set; }
            /// <summary>Gets or sets 言語ラベル</summary>
            /// <value>言語ラベル</value>
            public string LanguageLabel { get; set; }
        }

        /// <summary>
        /// テンポラリフォルダパス情報クラス
        /// </summary>
        public class TempFolderPathInfo
        {
            /// <summary>Gets or sets テンポラリフォルダパス</summary>
            /// <value>テンポラリフォルダパス</value>
            public string TempFolderPath { get; set; }
        }

        /// <summary>
        /// 画像表示情報クラス
        /// </summary>
        public class ImageFileInfo
        {
            /// <summary>Gets or sets ファイルの認証用のキー情報</summary>
            /// <value>ファイルの認証用のキー情報</value>
            public string FileInfo { get; set; }
            /// <summary>Gets or sets 表示するファイルのパス</summary>
            /// <value>表示するファイルのパス</value>
            public string FilePath { get; set; }
        }

        /// <summary>
        /// エンティティ取得SQL生成
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <param name="condition">検索条件</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>SQL文</returns>
        public static string getEntity(string tableName, dynamic condition, ComDB db)
        {
            // プライマリーキー取得
            IList<string> keyList = getPrimaryKeyList(tableName, db);
            if (keyList == null)
            {
                return null;
            }

            // SQL生成(SELECT文)
            string sql = "SELECT * FROM " + tableName;
            // SQL生成(WHERE句の条件)
            string whereSql = getSqlWhereByPK(keyList, condition);

            return sql + whereSql;
        }

        /// <summary>
        /// テーブル削除SQL(主キー指定)生成
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <param name="condition">検索条件</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>SQL文</returns>
        public static string getDeleteSql(string tableName, dynamic condition, ComDB db)
        {
            // プライマリーキー取得
            IList<string> keyList = getPrimaryKeyList(tableName, db);
            if (keyList == null)
            {
                return null;
            }

            // SQL生成(SELECT文)
            string sql = "DELETE FROM " + tableName;
            // SQL生成(WHERE句の条件)
            string whereSql = getSqlWhereByPK(keyList, condition);

            return sql + whereSql;
        }

        /// <summary>
        /// テーブルの主キーを取得
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>テーブルの主キー名のリスト</returns>
        private static IList<string> getPrimaryKeyList(string tableName, ComDB db)
        {
            // SQL生成(プライマリーキー取得)
            string sql = string.Empty;
            sql += " SELECT ccu.column_name AS column_name FROM information_schema.table_constraints tb_con ";
            sql += " INNER JOIN information_schema.constraint_column_usage ccu on ";
            sql += "     tb_con.constraint_catalog = ccu.constraint_catalog ";
            sql += " AND tb_con.constraint_schema = ccu.constraint_schema ";
            sql += " AND tb_con.constraint_name = ccu.constraint_name ";
            sql += " INNER JOIN information_schema.key_column_usage kcu on ";
            sql += "     tb_con.constraint_catalog = kcu.constraint_catalog ";
            sql += " AND tb_con.constraint_schema = kcu.constraint_schema ";
            sql += " AND tb_con.constraint_name = kcu.constraint_name ";
            sql += " AND ccu.column_name = kcu.column_name ";
            sql += " WHERE 1 = 1 ";
            sql += " AND tb_con.table_name = @TableName "; // テーブル名
            sql += " AND tb_con.constraint_type = 'PRIMARY KEY' "; // PRIMARY KEY」は大文字
            sql += " ORDER BY tb_con.table_catalog, tb_con.table_name, tb_con.constraint_name, kcu.ordinal_position ";

            // プライマリーキー取得
            IList<string> keyList = db.GetList<string>(sql, new { TableName = tableName });
            return keyList;
        }

        /// <summary>
        /// テーブルの主キーとその条件より、WHERE句を生成
        /// </summary>
        /// <param name="keyList">テーブルの主キー名リスト</param>
        /// <param name="condition">検索条件</param>
        /// <returns>WHERE句のSQL</returns>
        private static string getSqlWhereByPK(IList<string> keyList, dynamic condition)
        {
            string sql = " WHERE 1 = 1 ";
            Type type = condition.GetType();
            for (int i = 0; i < keyList.Count; i++)
            {
                string colName = keyList[i];
                System.Reflection.PropertyInfo property = type.GetProperty(ComUtil.SnakeCaseToPascalCase(colName));
                if (property != null)
                {
                    sql += " AND " + colName + " = '" + property.GetValue(condition) + "'";
                }
                else
                {
                    sql += " AND " + colName + " = ''";
                }
            }
            return sql;
        }

        /// <summary>
        /// 条件絞り込みSQL生成
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <param name="columns">検索条件の列名</param>
        /// <returns>SQL</returns>
        public static string getListConditionSql(string tableName, List<string> columns)
        {
            string sql = "SELECT * FROM " + tableName + getWhereConditionSql(columns);
           
            return sql;
        }

        /// <summary>
        /// 条件指定SQLのWHERE句を生成
        /// </summary>
        /// <param name="columns">検索条件の列名</param>
        /// <returns>WHRE句</returns>
        private static string getWhereConditionSql(List<string> columns)
        {
            string sql = " WHERE 1=1 ";
            foreach (var column in columns)
            {
                string condition = " AND " + column + " = @" + ComUtil.SnakeCaseToPascalCase(column);
                sql += condition;
            }
            return sql;
        }

        /// <summary>
        /// 条件指定削除SQLの生成
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <param name="columns">検索条件の列名</param>
        /// <returns>SQL</returns>
        public static string getDeleteConditionSql(string tableName, List<string> columns)
        {
            string sql = "DELETE FROM " + tableName + getWhereConditionSql(columns);

            return sql;
        }

        /// <summary>
        /// システム年度設定を行うクラスで必要なインタフェース
        /// </summary>
        public interface ISysFiscalYear
        {
            /// <summary>
            /// From-Toを結合してメンバにセットする処理
            /// </summary>
            /// <remarks>無い場合は何もしない処理を実装してください</remarks>
            public void JoinFromTo();
        }
    }
}
