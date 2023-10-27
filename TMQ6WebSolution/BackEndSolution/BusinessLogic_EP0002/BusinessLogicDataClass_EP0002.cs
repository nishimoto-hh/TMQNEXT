using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_EP0002
{
    /// <summary>
    /// アップロード条件のデータクラス
    /// </summary>
    public class BusinessLogicDataClass_EP0002
    {
        /// <summary>
        /// 検索結果のデータクラス(予備品情報)
        /// </summary>
        public class searchExcelPortUpdCondition
        {
            /// <summary>Gets or sets Excel Portバージョン</summary>
            /// <value>Excel Portバージョン</value>
            public string ExcelPortVer { get; set; }
            /// <summary>Gets or sets アップロードファイル</summary>
            /// <value>アップロードファイル</value>
            public long UploadFile { get; set; }
        }
    }
}
