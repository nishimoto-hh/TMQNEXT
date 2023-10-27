using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_RM0002
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_RM0002
    {
        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 帳票</summary>
            /// <value>帳票</value>
            public string ReportName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets プログラムID</summary>
            /// <value>プログラムID</value>
            public string ProgramId { get; set; }
            /// <summary>Gets or sets 帳票ID</summary>
            /// <value>帳票ID</value>
            public string ReportId { get; set; }
        }
    }
}
