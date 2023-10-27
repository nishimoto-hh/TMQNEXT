using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CommonExcelUtil
{
    /// <summary>
    /// EXCEL印刷情報クラス
    /// </summary>
    public class CommonExcelPrtInfo
    {
        private static readonly int AzLength = (int)'Z' - (int)'A' + 1; // 26
        private static readonly int OffsetNum = (int)'A' - 1; // 64
        private string sheetName;
        private int sheetNo;
        private List<MappingInfo> exlSetValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonExcelPrtInfo()
        {
            this.sheetName = string.Empty;
            this.sheetNo = 0;
            this.exlSetValue = new List<MappingInfo>();
        }

        /// <summary>
        /// マッピング情報セット
        /// </summary>
        /// <param name="address">セル</param>
        /// <param name="value">設定値</param>
        /// <param name="format">フォーマット</param>
        public void SetExlSetValueByAddress(string address, object value, string format = null)
        {
            // "$"があれば取り除く
            string tmpAddress = address.Replace("$", "");

            // アドレス部の文字チェック
            // XX99以外の書式はエラー
            if (!Regex.IsMatch(tmpAddress, @"^[A-Z]{1,}[1-9]{1}[0-9]*$"))
            {
                // エラーメッセージ

                return;
            }

            // アドレス部を1文字ずつ切り出す
            char[] charAddress = tmpAddress.ToCharArray();
            string addressX = string.Empty;
            string addressY = string.Empty;

            for (int i = 0; i < charAddress.Length; i++)
            {
                // 数値の場合、Y軸
                if (char.IsNumber(charAddress[i]) == true)
                {
                    addressY += charAddress[i].ToString();
                    continue;
                }
                // 数値以外の場合、X軸
                else
                {
                    addressX += charAddress[i].ToString();
                }
            }

            // X軸のインデックスを求める
            int intX = toColNumber(addressX) - 1;
            // Y軸のインデックスを求める
            int intY = int.Parse(addressY) - 1;
            // リスト追加
            MappingInfo mappingInfo = new MappingInfo();
            mappingInfo.X = intX;
            mappingInfo.Y = intY;
            mappingInfo.Value = value;

            if (!string.IsNullOrEmpty(format))
            {
                // 0落ち対応
                mappingInfo.Format = format;
            }

            exlSetValue.Add(mappingInfo);
        }

        /// <summary>
        /// マッピング情報セット(行単位)
        /// </summary>
        /// <param name="address">セル</param>
        /// <param name="value">設定値</param>
        public void SetExlSetRowValueByAddress(string address, object[] value)
        {
            // "$"があれば取り除く
            string tmpAddress = address.Replace("$", "");

            // アドレス部の文字チェック
            // XX99以外の書式はエラー
            if (!Regex.IsMatch(tmpAddress, @"^[A-Z]{1,}[1-9]{1}[0-9]*$"))
            {
                // エラーメッセージ

                return;
            }

            // アドレス部を1文字ずつ切り出す
            char[] charAddress = tmpAddress.ToCharArray();
            string addressX = string.Empty;
            string addressY = string.Empty;

            for (int i = 0; i < charAddress.Length; i++)
            {
                // 数値の場合、Y軸
                if (char.IsNumber(charAddress[i]) == true)
                {
                    addressY += charAddress[i].ToString();
                    continue;
                }
                // 数値以外の場合、X軸
                else
                {
                    addressX += charAddress[i].ToString();
                }
            }

            // X軸のインデックスを求める
            int intX = toColNumber(addressX) - 1;
            // Y軸のインデックスを求める
            int intY = int.Parse(addressY) - 1;

            MappingInfo mmappingInfo = new MappingInfo();
            foreach (var val in value)
            {
                if (val != null && !string.IsNullOrEmpty(val.ToString()))
                {
                    mmappingInfo = new MappingInfo();
                    mmappingInfo.X = intX;
                    mmappingInfo.Y = intY;
                    mmappingInfo.Value = val;
                    exlSetValue.Add(mmappingInfo);
                }
                intX = intX + 1;
            }
        }

        /// <summary>
        /// 対象シート名セット
        /// </summary>
        /// <param name="sheetName">シート名</param>
        public void SetSheetName(string sheetName)
        {
            this.sheetName = sheetName;
        }

        /// <summary>
        /// 対象シート番号セット
        /// </summary>
        /// <param name="sheetNo">シート番号</param>
        public void SetSheetNo(int sheetNo)
        {
            this.sheetNo = sheetNo;
        }

        /// <summary>
        /// マッピング情報リスト取得
        /// </summary>
        /// <returns>マッピング情報リスト</returns>
        public List<MappingInfo> GetExlOutputData()
        {
            return this.exlSetValue;
        }

        /// <summary>
        /// シート名取得
        /// </summary>
        /// <returns>シート名</returns>
        public string GetSheetName()
        {
            return this.sheetName;
        }

        /// <summary>
        /// シート番号取得
        /// </summary>
        /// <returns>シート番号</returns>
        public int GetSheetNo()
        {
            return this.sheetNo;
        }

        /// <summary>
        /// 列(文字)から数値変換
        /// </summary>
        /// <param name="colStr">列（文字）</param>
        /// <returns>列（数値）</returns>
        private int toColNumber(string colStr)
        {
            return convertString(0, new Queue<char>(colStr.ToCharArray()));
        }

        /// <summary>
        /// convertString
        /// </summary>
        /// <param name="ret">ret</param>
        /// <param name="chars">chars</param>
        /// <returns>return</returns>
        private int convertString(int ret, Queue<char> chars)
        {
            if (chars.Count == 0)
            {
                return ret;
            }
            else
            {
                char c = chars.Dequeue();
                return convertString(calcDecimal(c, chars.Count) + ret, chars); // 再帰
            }
        }

        private int calcDecimal(char c, int times)
        {
            return (int)Math.Pow(AzLength, times) * ((int)c - OffsetNum);
        }

    }
}
