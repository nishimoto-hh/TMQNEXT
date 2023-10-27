using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_SI0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_SI0001
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets構成グループID</summary>
            /// <value>構成グループID</value>
            public int StructureGroupId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets アイテム翻訳</summary>
            /// <value>アイテム翻訳</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets アイテムID</summary>
            /// <value>アイテムID</value>
            public int ItemId { get; set; }
        }
    }
}
