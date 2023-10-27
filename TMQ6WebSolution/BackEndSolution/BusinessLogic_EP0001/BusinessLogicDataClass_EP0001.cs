using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_EP0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_EP0001
    {
        /// <summary>
        /// ダウンロード一覧検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string UserId { get; set; }

            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
        }

        /// <summary>
        /// ダウンロード一覧のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機能分類名</summary>
            /// <value>機能分類名</value>
            public string ConductCategoryName { get; set; }
            /// <summary>Gets or sets 機能名</summary>
            /// <value>機能名</value>
            public string ConductName { get; set; }

            /// <summary>Gets or sets 対象ロジック機能ID</summary>
            /// <value>対象ロジック機能ID</value>
            public string HideConductId { get; set; }

            /// <summary>Gets or sets ExcelPort対象シート番号</summary>
            /// <value>ExcelPort対象シート番号</value>
            public string HideSheetNo { get; set; }

            /// <summary>Gets or sets 追加条件区分</summary>
            /// <value>追加条件区分</value>
            public string AddCondition { get; set; }
        }

        /// <summary>
        /// ダウンロード条件一覧のデータクラス
        /// </summary>
        public class searchListCondition : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 機能名</summary>
            /// <value>機能名</value>
            public string ConductName { get; set; }

            /// <summary>Gets or sets 対象ロジック機能ID</summary>
            /// <value>対象ロジック機能ID</value>
            public string HideConductId { get; set; }

            /// <summary>Gets or sets ExcelPort対象シート番号</summary>
            /// <value>ExcelPort対象シート番号</value>
            public string HideSheetNo { get; set; }

            /// <summary>Gets or sets 追加条件区分</summary>
            /// <value>追加条件区分</value>
            public string AddCondition { get; set; }
        }
    }
}
