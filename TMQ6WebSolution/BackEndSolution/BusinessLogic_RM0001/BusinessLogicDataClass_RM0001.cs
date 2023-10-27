using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;

namespace BusinessLogic_RM0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_RM0001
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            // 検索条件に使用する場合は、検索条件格納共通クラスを継承してください。

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
            /// <summary>Gets or sets プログラムID</summary>
            /// <value>プログラムID</value>
            public string ProgramId { get; set; }
            /// <summary>Gets or sets テンプレートファイルパス</summary>
            /// <value>テンプレートファイルパス</value>
            public string TemplateFilePath { get; set; }
            /// <summary>Gets or sets テンプレートファイル名</summary>
            /// <value>テンプレートファイル名</value>
            public string TemplateFileName { get; set; }
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int SheetNo { get; set; }
            /// <summary>Gets or sets 権限レベル</summary>
            /// <value>権限レベル</value>
            public int AuthorityLevel { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 項目名</summary>
            /// <value>項目名</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 帳票ID</summary>
            /// <value>帳票ID</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets テンプレートID</summary>
            /// <value>テンプレートID</value>
            public int TemplateId { get; set; }
            /// <summary>Gets or sets テンプレートファイルパス</summary>
            /// <value>テンプレートファイルパス</value>
            public string TemplateFilePath { get; set; }
            /// <summary>Gets or sets テンプレートファイル名</summary>
            /// <value>テンプレートファイル名</value>
            public string TemplateFileName { get; set; }
            /// <summary>Gets or sets 出力パターンID</summary>
            /// <value>出力パターンID</value>
            public int OutputPatternId { get; set; }
            /// <summary>Gets or sets シート番号</summary>
            /// <value>シート番号</value>
            public int SheetNo { get; set; }
            /// <summary>Gets or sets 項目ID</summary>
            /// <value>項目ID</value>
            public int ItemId { get; set; }
            /// <summary>Gets or sets 出力項目ID</summary>
            /// <value>出力項目ID</value>
            public int OutputItemId { get; set; }
            /// <summary>Gets or sets 出力項目定義</summary>
            /// <value>出力項目定義</value>
            public int OutputItemType { get; set; }
        }

        /// <summary>
        /// スケジュールの検索条件(工場IDのみ)
        /// </summary>
        public class SearchCondition : TMQDao.ScheduleList.GetCondition
        {
            /// <summary>Gets or sets 構成マスタ検索対象の工場ID</summary>
            /// <value>構成マスタ検索対象の工場ID</value>
            public List<int> FactoryIdList { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="condition">画面の検索条件</param>
            /// <param name="monthStartNendo">年度開始月</param>
            /// <param name="languageId">言語ID</param>
            public SearchCondition(TMQDao.ScheduleList.Condition condition, int monthStartNendo, string languageId) : base(condition, monthStartNendo)
            {
                this.LanguageId = languageId;
            }
        }
    }
}
