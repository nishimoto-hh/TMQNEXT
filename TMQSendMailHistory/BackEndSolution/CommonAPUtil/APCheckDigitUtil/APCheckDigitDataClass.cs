using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPUtil.APCheckDigitUtil
{
    /// <summary>
    /// 端数処理Daoクラス
    /// </summary>
    public class APCheckDigitDataClass
    {
        /// <summary>
        /// 端数桁数クラス
        /// </summary>
        public class NumberChkDigitDetail
        {
            /// <summary>Gets or sets 区分</summary>
            /// <value>区分</value>
            public string UnitDivision { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 全体桁数</summary>
            /// <value>全体桁数</value>
            public decimal? MaxLength { get; set; }
            /// <summary>Gets or sets 整数部桁数</summary>
            /// <value>整数部桁数</value>
            public decimal? IntegerLength { get; set; }
            /// <summary>Gets or sets 小数点以下桁数</summary>
            /// <value>小数点以下桁数</value>
            public decimal? SmallnumLength { get; set; }
            /// <summary>Gets or sets 端数区分 0:なし、1:切り捨て、2:四捨五入、3:切り上げ</summary>
            /// <value>端数区分 0:なし、1:切り捨て、2:四捨五入、3:切り上げ</value>
            public decimal? RoundDivision { get; set; }
            /// <summary>Gets or sets 下限値</summary>
            /// <value>下限値</value>
            public decimal? LowerLimit { get; set; }
            /// <summary>Gets or sets 上限値</summary>
            /// <value>上限値</value>
            public decimal? UpperLimit { get; set; }
        }
    }
}
