using CommonWebTemplate.CommonDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonSTDUtil.CommonSTDUtil
{
    public class CommonOutputReportDataClass: CommonDataBaseClass
    {
        /// <summary>
        /// 出力帳票定義
        /// </summary>
        public class MsOutputReportDefineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsOutputReportDefineEntity()
            {
                TableName = "ms_output_report_define";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工場id</summary>
            /// <value>工場id</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets プログラムid</summary>
            /// <value>プログラムid</value>
            public string ProgramId { get; set; }
            /// <summary>Gets or sets 帳票id</summary>
            /// <value>帳票id</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets テンプレートアップロード可否</summary>
            /// <value>テンプレートアップロード可否</value>
            public bool TemplateUploadFlg { get; set; }
            /// <summary>Gets or sets 帳票名翻訳id</summary>
            /// <value>帳票名翻訳id</value>
            public int ReportNameTranslationId { get; set; }
            /// <summary>Gets or sets 帳票名（開発者向け）</summary>
            /// <value>帳票名（開発者向け）</value>
            public string ReportName { get; set; }
            /// <summary>Gets or sets 管理種別</summary>
            /// <value>管理種別</value>
            public int ManagementType { get; set; }
            /// <summary>Gets or sets 出力項目種別</summary>
            /// <value>出力項目種別</value>
            public int OutputItemType { get; set; }
            /// <summary>Gets or sets 出力順</summary>
            /// <value>出力順</value>
            public int? DisplayOrder { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 工場id</summary>
                /// <value>工場id</value>
                public int FactoryId { get; set; }
                /// <summary>Gets or sets プログラムid</summary>
                /// <value>プログラムid</value>
                public string ProgramId { get; set; }
                /// <summary>Gets or sets 帳票id</summary>
                /// <value>帳票id</value>
                public string ReportId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pFactoryId, string pProgramId, string pReportId)
                {
                    FactoryId = pFactoryId;
                    ProgramId = pProgramId;
                    ReportId = pReportId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.FactoryId, this.ProgramId, this.ReportId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsOutputReportDefineEntity GetEntity(int pFactoryId, string pProgramId, string pReportId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pProgramId, pReportId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsOutputReportDefineEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pFactoryId, string pProgramId, string pReportId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pProgramId, pReportId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 出力帳票シート定義
        /// </summary>
        public class MsOutputReportSheetDefineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsOutputReportSheetDefineEntity()
            {
                TableName = "ms_output_report_sheet_define";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工場id</summary>
            /// <value>工場id</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 帳票id</summary>
            /// <value>帳票id</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int SheetNo { get; set; }
            /// <summary>Gets or sets シート名翻訳id</summary>
            /// <value>シート名翻訳id</value>
            public int SheetNameTranslationId { get; set; }
            /// <summary>Gets or sets シート名（開発者向け）</summary>
            /// <value>シート名（開発者向け）</value>
            public string SheetName { get; set; }
            /// <summary>Gets or sets シート定義最大行</summary>
            /// <value>シート定義最大行</value>
            public int SheetDefineMaxRow { get; set; }
            /// <summary>Gets or sets シート定義最大列</summary>
            /// <value>シート定義最大列</value>
            public int SheetDefineMaxColumn { get; set; }
            /// <summary>Gets or sets 検索条件フラグ</summary>
            /// <value>検索条件フラグ</value>
            public bool SearchConditionFlg { get; set; }
            /// <summary>Gets or sets 一覧フラグ</summary>
            /// <value>一覧フラグ</value>
            public bool ListFlg { get; set; }
            /// <summary>Gets or sets レコード行数</summary>
            /// <value>レコード行数</value>
            public int? RecordCount { get; set; }
            /// <summary>Gets or sets 対象sql</summary>
            /// <value>対象sql</value>
            public string TargetSql { get; set; }
            /// <summary>Gets or sets 対象sqlパラメータ</summary>
            /// <value>対象sqlパラメータ</value>
            public string TargetSqlParams { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 工場id</summary>
                /// <value>工場id</value>
                public int FactoryId { get; set; }
                /// <summary>Gets or sets 帳票id</summary>
                /// <value>帳票id</value>
                public string ReportId { get; set; }
                /// <summary>Gets or sets シート番号</summary>
                /// <value>シート番号</value>
                public int SheetNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pFactoryId, string pReportId, int pSheetNo)
                {
                    FactoryId = pFactoryId;
                    ReportId = pReportId;
                    SheetNo = pSheetNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.FactoryId, this.ReportId, this.SheetNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsOutputReportSheetDefineEntity GetEntity(int pFactoryId, string pReportId, int pSheetNo, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pSheetNo);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsOutputReportSheetDefineEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pFactoryId, string pReportId, int pSheetNo, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pSheetNo);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 出力帳票項目定義
        /// </summary>
        public class MsOutputReportItemDefineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsOutputReportItemDefineEntity()
            {
                TableName = "ms_output_report_item_define";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工場id</summary>
            /// <value>工場id</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 帳票id</summary>
            /// <value>帳票id</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int SheetNo { get; set; }
            /// <summary>Gets or sets 項目id</summary>
            /// <value>項目id</value>
            public int ItemId { get; set; }
            /// <summary>Gets or sets 項目名（開発者向け）</summary>
            /// <value>項目名（開発者向け）</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets コントロールID</summary>
            /// <value>コントロールID</value>
            public string ControlId { get; set; }
            /// <summary>Gets or sets コントロールタイプ</summary>
            /// <value>コントロールタイプ</value>
            public string ControlType { get; set; }
            /// <summary>Gets or sets カラム名</summary>
            /// <value>カラム名</value>
            public string ColumnName { get; set; }
            /// <summary>Gets or sets 出力方式</summary>
            /// <value>出力方式</value>
            public int OutputMethod { get; set; }
            /// <summary>Gets or sets 連続出力間隔</summary>
            /// <value>連続出力間隔</value>
            public int? ContinuousOutputInterval { get; set; }
            /// <summary>Gets or sets デフォルトセル行no</summary>
            /// <value>デフォルトセル行no</value>
            public int? DefaultCellRowNo { get; set; }
            /// <summary>Gets or sets デフォルトセル列no</summary>
            /// <value>デフォルトセル列no</value>
            public int? DefaultCellColumnNo { get; set; }
            /// <summary>Gets or sets デフォルト行結合数</summary>
            /// <value>デフォルト行結合数</value>
            public int? DefaultRowJoinCount { get; set; }
            /// <summary>Gets or sets デフォルト列結合数</summary>
            /// <value>デフォルト列結合数</value>
            public int? DefaultColumnJoinCount { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 工場id</summary>
                /// <value>工場id</value>
                public int FactoryId { get; set; }
                /// <summary>Gets or sets 帳票id</summary>
                /// <value>帳票id</value>
                public string ReportId { get; set; }
                /// <summary>Gets or sets シート番号</summary>
                /// <value>シート番号</value>
                public int SheetNo { get; set; }
                /// <summary>Gets or sets 項目id</summary>
                /// <value>項目id</value>
                public int ItemId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pFactoryId, string pReportId, int pSheetNo, int pItemId)
                {
                    FactoryId = pFactoryId;
                    ReportId = pReportId;
                    SheetNo = pSheetNo;
                    ItemId = pItemId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.FactoryId, this.ReportId, this.SheetNo, this.ItemId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsOutputReportItemDefineEntity GetEntity(int pFactoryId, string pReportId, int pSheetNo, int pItemId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pSheetNo, pItemId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsOutputReportItemDefineEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pFactoryId, string pReportId, int pSheetNo, int pItemId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pSheetNo, pItemId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 出力テンプレート
        /// </summary>
        public class MsOutputTemplateEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsOutputTemplateEntity()
            {
                TableName = "ms_output_template";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工場id</summary>
            /// <value>工場id</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 帳票id</summary>
            /// <value>帳票id</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets テンプレートid</summary>
            /// <value>テンプレートid</value>
            public int TemplateId { get; set; }
            /// <summary>Gets or sets テンプレート名</summary>
            /// <value>テンプレート名</value>
            public string TemplateName { get; set; }
            /// <summary>Gets or sets テンプレートファイルパス</summary>
            /// <value>テンプレートファイルパス</value>
            public string TemplateFilePath { get; set; }
            /// <summary>Gets or sets テンプレートファイル名</summary>
            /// <value>テンプレートファイル名</value>
            public string TemplateFileName { get; set; }
            /// <summary>Gets or sets 使用ユーザid</summary>
            /// <value>使用ユーザid</value>
            public int? UseUserId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 工場id</summary>
                /// <value>工場id</value>
                public int FactoryId { get; set; }
                /// <summary>Gets or sets 帳票id</summary>
                /// <value>帳票id</value>
                public string ReportId { get; set; }
                /// <summary>Gets or sets テンプレートid</summary>
                /// <value>テンプレートid</value>
                public int TemplateId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pFactoryId, string pReportId, int pTemplateId)
                {
                    FactoryId = pFactoryId;
                    ReportId = pReportId;
                    TemplateId = pTemplateId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.FactoryId, this.ReportId, this.TemplateId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsOutputTemplateEntity GetEntity(int pFactoryId, string pReportId, int pTemplateId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pTemplateId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsOutputTemplateEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pFactoryId, string pReportId, int pTemplateId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pTemplateId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 出力パターン
        /// </summary>
        public class MsOutputPatternEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsOutputPatternEntity()
            {
                TableName = "ms_output_pattern";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 帳票ID</summary>
            /// <value>帳票ID</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets テンプレートID</summary>
            /// <value>テンプレートID</value>
            public int TemplateId { get; set; }
            /// <summary>Gets or sets 出力パターンID</summary>
            /// <value>出力パターンID</value>
            public int OutputPatternId { get; set; }
            /// <summary>Gets or sets 出力パターン名</summary>
            /// <value>出力パターン名</value>
            public string OutputPatternName { get; set; }
            /// <summary>Gets or sets 使用ユーザID</summary>
            /// <value>使用ユーザID</value>
            public int? UseUserId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int FactoryId { get; set; }
                /// <summary>Gets or sets 帳票ID</summary>
                /// <value>帳票ID</value>
                public string ReportId { get; set; }
                /// <summary>Gets or sets テンプレートID</summary>
                /// <value>テンプレートID</value>
                public int TemplateId { get; set; }
                /// <summary>Gets or sets 出力パターンID</summary>
                /// <value>出力パターンID</value>
                public int OutputPatternId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pFactoryId, string pReportId, int pTemplateId, int pOutputPatternId)
                {
                    FactoryId = pFactoryId;
                    ReportId = pReportId;
                    TemplateId = pTemplateId;
                    OutputPatternId = pOutputPatternId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.FactoryId, this.ReportId, this.TemplateId, this.OutputPatternId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsOutputPatternEntity GetEntity(int pFactoryId, string pReportId, int pTemplateId, int pOutputPatternId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pTemplateId, pOutputPatternId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsOutputPatternEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pFactoryId, string pReportId, int pTemplateId, int pOutputPatternId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pTemplateId, pOutputPatternId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }
        /// <summary>
        /// 出力シート
        /// </summary>
        public class MsOutputSheetEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsOutputSheetEntity()
            {
                TableName = "ms_output_sheet";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 帳票ID</summary>
            /// <value>帳票ID</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets テンプレートID</summary>
            /// <value>テンプレートID</value>
            public string TemplateId { get; set; }
            /// <summary>Gets or sets 出力パターンID</summary>
            /// <value>出力パターンID</value>
            public int OutputPatternId { get; set; }
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int SheetNo { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int FactoryId { get; set; }
                /// <summary>Gets or sets 帳票ID</summary>
                /// <value>帳票ID</value>
                public string ReportId { get; set; }
                /// <summary>Gets or sets テンプレートID</summary>
                /// <value>テンプレートID</value>
                public string TemplateId { get; set; }
                /// <summary>Gets or sets 出力パターンID</summary>
                /// <value>出力パターンID</value>
                public int OutputPatternId { get; set; }
                /// <summary>Gets or sets シート番号</summary>
                /// <value>シート番号</value>
                public int SheetNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pFactoryId, string pReportId, string pTemplateId, int pOutputPatternId, int pSheetNo)
                {
                    FactoryId = pFactoryId;
                    ReportId = pReportId;
                    TemplateId = pTemplateId;
                    OutputPatternId = pOutputPatternId;
                    SheetNo = pSheetNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.FactoryId, this.ReportId, this.TemplateId, this.OutputPatternId, this.SheetNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsOutputSheetEntity GetEntity(int pFactoryId, string pReportId, string pTemplateId, int pOutputPatternId, int pSheetNo, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pTemplateId, pOutputPatternId, pSheetNo);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsOutputSheetEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pFactoryId, string pReportId, string pTemplateId, int pOutputPatternId, int pSheetNo, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pTemplateId, pOutputPatternId, pSheetNo);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }
        /// <summary>
        /// 出力項目
        /// </summary>
        public class MsOutputItemEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsOutputItemEntity()
            {
                TableName = "ms_output_item";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 帳票ID</summary>
            /// <value>帳票ID</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets テンプレートID</summary>
            /// <value>テンプレートID</value>
            public int TemplateId { get; set; }
            /// <summary>Gets or sets 出力パターンID</summary>
            /// <value>出力パターンID</value>
            public int OutputPatternId { get; set; }
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int SheetNo { get; set; }
            /// <summary>Gets or sets 項目ID</summary>
            /// <value>項目ID</value>
            public int ItemId { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DisplayOrder { get; set; }
            /// <summary>Gets or sets デフォルトセル行No</summary>
            /// <value>デフォルトセル行No</value>
            public int? DefaultCellRowNo { get; set; }
            /// <summary>Gets or sets デフォルトセル列No</summary>
            /// <value>デフォルトセル列No</value>
            public int? DefaultCellColumnNo { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int FactoryId { get; set; }
                /// <summary>Gets or sets 帳票ID</summary>
                /// <value>帳票ID</value>
                public string ReportId { get; set; }
                /// <summary>Gets or sets テンプレートID</summary>
                /// <value>テンプレートID</value>
                public int TemplateId { get; set; }
                /// <summary>Gets or sets 出力パターンID</summary>
                /// <value>出力パターンID</value>
                public int OutputPatternId { get; set; }
                /// <summary>Gets or sets シート番号</summary>
                /// <value>シート番号</value>
                public int SheetNo { get; set; }
                /// <summary>Gets or sets 項目ID</summary>
                /// <value>項目ID</value>
                public int ItemId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pFactoryId, string pReportId, int pTemplateId, int pOutputPatternId, int pSheetNo, int pItemId)
                {
                    FactoryId = pFactoryId;
                    ReportId = pReportId;
                    TemplateId = pTemplateId;
                    OutputPatternId = pOutputPatternId;
                    SheetNo = pSheetNo;
                    ItemId = pItemId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.FactoryId, this.ReportId, this.TemplateId, this.OutputPatternId, this.SheetNo, this.ItemId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsOutputItemEntity GetEntity(int pFactoryId, string pReportId, int pTemplateId, int pOutputPatternId, int pSheetNo, int pItemId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pTemplateId, pOutputPatternId, pSheetNo, pItemId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsOutputItemEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pFactoryId, string pReportId, int pTemplateId, int pOutputPatternId, int pSheetNo, int pItemId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pTemplateId, pOutputPatternId, pSheetNo, pItemId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

    }
}
