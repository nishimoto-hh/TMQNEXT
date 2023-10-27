using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_SU0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_SU0001
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 担当者名</summary>
            /// <value>担当者名</value>
            public string UserName { get; set; }
            /// <summary>Gets or sets メールアドレス</summary>
            /// <value>メールアドレス</value>
            public string MailAddress { get; set; }
            /// <summary>Gets or sets 担当者ID</summary>
            /// <value>担当者ID</value>
            public string UserId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 担当者名</summary>
            /// <value>担当者名</value>
            public string UserName { get; set; }
            /// <summary>Gets or sets 担当者ID</summary>
            /// <value>担当者ID</value>
            public string UserId { get; set; }
        }
    }
}
