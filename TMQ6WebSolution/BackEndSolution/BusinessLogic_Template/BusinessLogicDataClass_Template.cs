using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;
namespace BusinessLogic_Template
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_Template
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            // 検索条件に使用する場合は、検索条件格納共通クラスを継承してください。

            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            // SQLの検索結果の列を定義してください。
            // 品目マスタから多くの列を取得する場合は、品目マスタのデータクラスを継承することで、それらの定義を省くことができます。

            /// <summary>Gets or sets 品目名称(テスト)</summary>
            /// <value>品目名称(テスト)</value>
            public string ItemNameTest { get; set; }
        }
    }
}
