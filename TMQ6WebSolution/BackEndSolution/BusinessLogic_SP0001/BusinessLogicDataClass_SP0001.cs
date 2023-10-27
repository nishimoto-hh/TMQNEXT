using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_SP0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_SP0001
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 予備品名</summary>
            /// <value>予備品名</value>
            public string PartsName { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 予備品名</summary>
            /// <value>予備品名</value>
            public string PartsName { get; set; }
            /// <summary>Gets or sets 部品名</summary>
            /// <value>部品名</value>
            public int PartsId { get; set; }
            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string StandardSize { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets メーカー名称</summary>
            /// <value>メーカー名称</value>
            public string Maker { get; set; }
        }
    }
}
