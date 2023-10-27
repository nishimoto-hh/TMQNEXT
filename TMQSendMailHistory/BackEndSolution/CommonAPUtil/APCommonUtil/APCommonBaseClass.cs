using System;
using System.Collections.Generic;
using System.Dynamic;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonAPUtil.APCommonUtil
{
    /// <summary>
    /// AP共通ベースDaoクラス
    /// </summary>
    public class APCommonBaseClass
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
            /// <summary>Gets or sets 登録日時</summary>
            /// <value>登録日時</value>
            public DateTime? InputDate { get; set; }
            /// <summary>Gets or sets 登録者ID</summary>
            /// <value>登録者ID</value>
            public string InputUserId { get; set; }
            /// <summary>Gets or sets 更新日時</summary>
            /// <value>更新日時</value>
            public DateTime? UpdateDate { get; set; }
            /// <summary>Gets or sets 更新者ID</summary>
            /// <value>更新者ID</value>
            public string UpdateUserId { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
        }

        /// <summary>
        /// プルーフテーブル格納クラス(共通)
        /// </summary>
        public class CommonProofItem
        {
            /// <summary>Gets or sets システム日付</summary>
            /// <value>システム日付</value>
            public DateTime SysDate { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string TantoCd { get; set; }
            /// <summary>Gets or sets プルーフステータス</summary>
            /// <value>プルーフステータス</value>
            public decimal ProofStatus { get; set; }
        }

        /// <summary>
        /// 通貨管理マスタ
        /// </summary>
        public class CurrencyCtrlSearch
        {
            /// <summary>Gets or sets レート換算結果</summary>
            /// <value>レート換算結果</value>
            public decimal? CalculatedValue { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 通貨シンボル</summary>
            /// <value>通貨シンボル</value>
            public string CurrencySymbol { get; set; }
            /// <summary>Gets or sets 国</summary>
            /// <value>国</value>
            public string CountryCd { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime ValidDate { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal ExRate { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 最大レート適用開始日</summary>
            /// <value>最大レート適用開始日</value>
            public string Maxdate { get; set; }
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
                /// <param name="conductid">機能ID</param>
                /// <param name="fileno">ファイル管理No</param>
                public PrimaryKey(string conductid, int fileno)
                {
                    Conductid = conductid;
                    Fileno = fileno;
                }
            }

            /// <summary>
            /// プライマリーキー
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
            /// <param name="conductid">機能ID</param>
            /// <param name="fileno">ファイル番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ComInoutFileDefineEntity GetEntity(string conductid, int fileno, ComDB db)
            {
                ComInoutFileDefineEntity.PrimaryKey condition = new ComInoutFileDefineEntity.PrimaryKey(conductid, fileno);
                return GetEntity<ComInoutFileDefineEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ファイル入出力グループ定義マスタ
        /// </summary>
        public class ComInoutGroupDefineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ComInoutGroupDefineEntity()
            {
                TableName = "com_inout_group_define";
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
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int Sheetno { get; set; }
            /// <summary>Gets or sets 項目ID</summary>
            /// <value>項目ID</value>
            public string Itemid { get; set; }
            /// <summary>Gets or sets 項目名</summary>
            /// <value>項目名</value>
            public string Itemname { get; set; }
            /// <summary>Gets or sets 入出力方式</summary>
            /// <value>入出力方式</value>
            public int? Datadirection { get; set; }
            /// <summary>Gets or sets レコード行数</summary>
            /// <value>レコード行数</value>
            public int Recordcount { get; set; }

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
                /// <summary>Gets or sets シート番号</summary>
                /// <value>シート番号</value>
                public int Sheetno { get; set; }
                /// <summary>Gets or sets 項目ID</summary>
                /// <value>項目ID</value>
                public string Itemid { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="conductid">機能ID</param>
                /// <param name="fileno">ファイル管理No</param>
                /// <param name="sheetno">シート番号</param>
                /// <param name="itemid">項目ID</param>
                public PrimaryKey(string conductid, int fileno, int sheetno, string itemid)
                {
                    Conductid = conductid;
                    Fileno = fileno;
                    Sheetno = sheetno;
                    Itemid = itemid;
                }
            }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.Conductid, this.Fileno, this.Sheetno, this.Itemid);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            /// <param name="conductid">機能ID</param>
            /// <param name="fileno">ファイル管理No</param>
            /// <param name="sheetno">シート番号</param>
            /// <param name="itemid">項目ID</param>
            /// <param name="db">DB操作クラス</param>
            public ComInoutGroupDefineEntity GetEntity(string conductid, int fileno, int sheetno, string itemid, ComDB db)
            {
                ComInoutGroupDefineEntity.PrimaryKey condition = new ComInoutGroupDefineEntity.PrimaryKey(conductid, fileno, sheetno, itemid);
                return GetEntity<ComInoutGroupDefineEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ファイル入出力管理クラス
        /// </summary>
        public class InoutDefine
        {
            /// <summary>Gets or sets 項目名</summary>
            /// <value>項目名</value>
            public string Itemname { get; set; }
            /// <summary>Gets or sets 項目表示名</summary>
            /// <value>項目表示名</value>
            public string Itemdisplayname { get; set; }
            /// <summary>Gets or sets 入出力開始列番号</summary>
            /// <value>入出力開始列番号</value>
            public int? Startcolno { get; set; }
            /// <summary>Gets or sets 入出力開始行番号</summary>
            /// <value>入出力開始行番号</value>
            public int? Startrowno { get; set; }
            /// <summary>Gets or sets 取込対象フラグ</summary>
            /// <value>取込対象フラグ</value>
            public bool? Inputflg { get; set; }
            /// <summary>Gets or sets セルタイプ</summary>
            /// <value>セルタイプ</value>
            public int? Celltype { get; set; }
            /// <summary>Gets or sets 桁数</summary>
            /// <value>桁数</value>
            public int? Maxlength { get; set; }
            /// <summary>Gets or sets 取込時必須</summary>
            /// <value>取込時必須</value>
            public bool? Nullcheckflg { get; set; }
            /// <summary>Gets or sets 行数</summary>
            /// <value>行数</value>
            public int Recordcount { get; set; }
            /// <summary>Gets or sets 入出力方式</summary>
            /// <value>入出力方式</value>
            public int? Datadirection { get; set; }
        }

        /// <summary>
        /// エラー内容
        /// </summary>
        public class UploadErrorInfo
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int RowNo { get; set; }
            /// <summary>Gets or sets 列番号/項目名</summary>
            /// <value>列番号/項目名</value>
            public int ItemName { get; set; }
            /// <summary>Gets or sets エラー内容</summary>
            /// <value>エラー内容</value>
            public string ErrorInfo { get; set; }
        }

        /// <summary>
        /// エンティティ取得SQL生成
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <param name="condition">検索条件</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>作成したSQL</returns>
        public static string GetEntity(string tableName, dynamic condition, ComDB db)
        {
            // SQL生成(プライマリーキー取得)
            string sql = string.Empty;
            sql += " select ccu.column_name as column_name from information_schema.table_constraints tb_con ";
            sql += " inner join information_schema.constraint_column_usage ccu on ";
            sql += "     tb_con.constraint_catalog = ccu.constraint_catalog ";
            sql += " and tb_con.constraint_schema = ccu.constraint_schema ";
            sql += " and tb_con.constraint_name = ccu.constraint_name ";
            sql += " inner join information_schema.key_column_usage kcu on ";
            sql += "     tb_con.constraint_catalog = kcu.constraint_catalog ";
            sql += " and tb_con.constraint_schema = kcu.constraint_schema ";
            sql += " and tb_con.constraint_name = kcu.constraint_name ";
            sql += " and ccu.column_name = kcu.column_name ";
            sql += " where 1 = 1 ";
            sql += " and tb_con.table_name = @TableName "; // テーブル名
            sql += " and tb_con.constraint_type = 'PRIMARY KEY' "; // PRIMARY KEY」は大文字
            sql += " order by tb_con.table_catalog, tb_con.table_name, tb_con.constraint_name, kcu.ordinal_position ";

            // プライマリーキー取得
            IList<string> keyList = db.GetList<string>(sql, new { TableName = tableName });
            if (keyList == null)
            {
                return null;
            }

            // SQL生成(SELECT文)
            sql = string.Empty;
            sql = "select * from " + tableName + " where 1 = 1";

            Type type = condition.GetType();
            for (int i = 0; i < keyList.Count; i++)
            {
                string colName = keyList[i];
                System.Reflection.PropertyInfo property = type.GetProperty(ComUtil.SnakeCaseToPascalCase(colName));
                if (property != null)
                {
                    sql += " and " + colName + " = '" + property.GetValue(condition) + "'";
                }
                else
                {
                    sql += " and " + colName + " = ''";
                }
            }

            return sql;
        }

        /// <summary>
        /// エンティティ取得SQL生成
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="tableName">テーブル名</param>
        /// <param name="condition">検索条件</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>作成したSQL</returns>
        public static T GetEntity<T>(string tableName, dynamic condition, ComDB db)
        {
            // 引数の条件と、共通データを統合し、条件を再作成する
            dynamic param = new ExpandoObject();

            // SQL生成(プライマリーキー取得)
            string sql = string.Empty;
            sql += " select ccu.column_name as column_name from information_schema.table_constraints tb_con ";
            sql += " inner join information_schema.constraint_column_usage ccu on ";
            sql += "     tb_con.constraint_catalog = ccu.constraint_catalog ";
            sql += " and tb_con.constraint_schema = ccu.constraint_schema ";
            sql += " and tb_con.constraint_name = ccu.constraint_name ";
            sql += " inner join information_schema.key_column_usage kcu on ";
            sql += "     tb_con.constraint_catalog = kcu.constraint_catalog ";
            sql += " and tb_con.constraint_schema = kcu.constraint_schema ";
            sql += " and tb_con.constraint_name = kcu.constraint_name ";
            sql += " and ccu.column_name = kcu.column_name ";
            sql += " where 1 = 1 ";
            sql += " and tb_con.table_name = @TableName "; // テーブル名
            sql += " and tb_con.constraint_type = 'PRIMARY KEY' "; // PRIMARY KEY」は大文字
            sql += " order by tb_con.table_catalog, tb_con.table_name, tb_con.constraint_name, kcu.ordinal_position ";

            // プライマリーキー取得
            IList<string> keyList = db.GetList<string>(sql, new { TableName = tableName });
            if (keyList == null)
            {
                return default(T);
            }

            // SQL生成(SELECT文)
            sql = string.Empty;
            sql = "select * from " + tableName + " where 1 = 1";

            Type type = condition.GetType();
            for (int i = 0; i < keyList.Count; i++)
            {
                string colName = keyList[i];
                System.Reflection.PropertyInfo property = type.GetProperty(ComUtil.SnakeCaseToPascalCase(colName));

                if (property != null)
                {
                    sql += " and " + colName + " = @" + ComUtil.SnakeCaseToPascalCase(colName);
                    ((IDictionary<string, object>)param).Add(ComUtil.SnakeCaseToPascalCase(colName), property.GetValue(condition));
                }
                else
                {
                    sql += " and " + colName + " = ''";
                }
            }

            return db.GetEntityByDataClass<T>(sql, param);
        }
    }
}
