using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonExcelUtil
{
    /// <summary>
    /// マッピング情報クラス
    /// </summary>
    public class MappingInfo
    {
        /// <summary>
        /// X座標
        /// </summary>
        private int x = -1;
        /// <summary>
        /// Gets or sets X座標
        /// </summary>
        /// <value>
        /// X座標
        /// </value>
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        /// <summary>
        /// Y座標
        /// </summary>
        private int y = -1;
        /// <summary>
        /// Gets or sets Y座標
        /// </summary>
        /// <value>
        /// Y座標
        /// </value>
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        /// <summary>
        /// 値
        /// </summary>
        private object val = string.Empty;
        /// <summary>
        /// Gets or sets 値
        /// </summary>
        /// <value>
        /// 値
        /// </value>
        public object Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
            }
        }

        /// <summary>
        /// フォーマット
        /// </summary>
        private string format = string.Empty;
        /// <summary>
        /// Gets or sets フォーマット
        /// </summary>
        /// <value>
        /// フォーマット
        /// </value>
        public string Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
            }
        }
    }
}
