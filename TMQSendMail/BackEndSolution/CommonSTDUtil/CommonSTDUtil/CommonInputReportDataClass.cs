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
    public class CommonInputReportDataClass: CommonDataBaseClass
    {
        /// <summary>
        /// ファイル取込コントロールグループ定義マスタ
        /// </summary>
        public class MsInputGroupDefineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsInputGroupDefineEntity()
            {
                TableName = "ms_input_group_define";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 帳票id</summary>
            /// <value>帳票id</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int SheetNo { get; set; }
            /// <summary>Gets or sets コントロールグループid</summary>
            /// <value>コントロールグループid</value>
            public string ControlGroupId { get; set; }
            /// <summary>Gets or sets コントロールグループ名</summary>
            /// <value>コントロールグループ名</value>
            public string ControlGroupName { get; set; }
            /// <summary>Gets or sets 入力方式</summary>
            /// <value>入力方式</value>
            public int DataDirection { get; set; }
            /// <summary>Gets or sets レコード行数</summary>
            /// <value>レコード行数</value>
            public int RecordCount { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
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
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(string pReportId, int pSheetNo, string pControlGroupId)
                {
                    ReportId = pReportId;
                    SheetNo = pSheetNo;
                    ControlGroupId = pControlGroupId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ReportId, this.SheetNo, this.ControlGroupId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsInputGroupDefineEntity GetEntity(string pReportId, int pSheetNo, string pControlGroupId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pReportId, pSheetNo, pControlGroupId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsInputGroupDefineEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(string pReportId, int pSheetNo, string pControlGroupId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pReportId, pSheetNo, pControlGroupId);
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
        /// ファイル取込コントロール定義マスタ
        /// </summary>
        public class MsInputControlDefineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsInputControlDefineEntity()
            {
                TableName = "ms_input_control_define";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 帳票id</summary>
            /// <value>帳票id</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int SheetNo { get; set; }
            /// <summary>Gets or sets コントロールグループid</summary>
            /// <value>コントロールグループid</value>
            public string ControlGroupId { get; set; }
            /// <summary>Gets or sets コントロール番号</summary>
            /// <value>コントロール番号</value>
            public int ControlNo { get; set; }
            /// <summary>Gets or sets コントロールタイプ</summary>
            /// <value>コントロールタイプ</value>
            public string ControlType { get; set; }
            /// <summary>Gets or sets コントロールid</summary>
            /// <value>コントロールid</value>
            public string ControlId { get; set; }
            /// <summary>Gets or sets 取込開始列番号</summary>
            /// <value>取込開始列番号</value>
            public int StartColumnNo { get; set; }
            /// <summary>Gets or sets 取込開始行番号</summary>
            /// <value>取込開始行番号</value>
            public int StartRowNo { get; set; }
            /// <summary>Gets or sets 取込対象フラグ</summary>
            /// <value>取込対象フラグ</value>
            public bool? InputFlg { get; set; }
            /// <summary>Gets or sets 必須項目区分</summary>
            /// <value>必須項目区分</value>
            public bool? RequiredFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
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
                /// <summary>Gets or sets コントロール番号</summary>
                /// <value>コントロール番号</value>
                public int ControlNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(string pReportId, int pSheetNo, string pControlGroupId, int pControlNo)
                {
                    ReportId = pReportId;
                    SheetNo = pSheetNo;
                    ControlGroupId = pControlGroupId;
                    ControlNo = pControlNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ReportId, this.SheetNo, this.ControlGroupId, this.ControlNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsInputControlDefineEntity GetEntity(string pReportId, int pSheetNo, string pControlGroupId, int pControlNo, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pReportId, pSheetNo, pControlGroupId, pControlNo);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsInputControlDefineEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(string pReportId, int pSheetNo, string pControlGroupId, int pControlNo, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pReportId, pSheetNo, pControlGroupId, pControlNo);
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
