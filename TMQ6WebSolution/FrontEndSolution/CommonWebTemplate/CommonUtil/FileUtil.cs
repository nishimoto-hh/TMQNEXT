///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　DB操作クラス
/// 説明　　　：　DB操作の共通処理を実装します。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CommonWebTemplate.CommonUtil
{
    public class FileUtil
    {
        #region === 定数定義 ===
        ///<summary>ﾌｧｲﾙ拡張子</summary>
        public enum Extension
        {
            /// <summary>Excelﾌﾞｯｸ(.xlsx)</summary>
            ExcelFile = 0,
            /// <summary>Excelﾏｸﾛ有効ﾌﾞｯｸ(.xlsm)</summary>
            ExcelonMacro,
            /// <summary>Excelﾃﾝﾌﾟﾚｰﾄ(.xltx)</summary>
            ExcelTemplate,
            /// <summary>Excelﾏｸﾛ有効ﾃﾝﾌﾟﾚｰﾄ(.xltm)</summary>
            ExcelTemplateonMacro,
            /// <summary>Excelｱﾄﾞｲﾝ(.xlam)</summary>
            ExcelAddin,
            /// <summary>ﾃｷｽﾄ(.txt)</summary>
            Text,
            /// <summary>CSV(.csv)</summary>
            Csv,
            /// <summary>PDF(.pdf)</summary>
            Pdf,
            /// <summary>Zip(.zip)</summary>
            Zip,
            /// <summary>UnDefined(不明、与えられたファイル名をそのまま出力)</summary>
            UnDefined,
        };

        /// <summary>
        /// ﾌｧｲﾙ拡張子を取得
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetExtensionStr(Extension extension)
        {
            string[] extensionVals = {
                @"xlsx",    //Excelﾌﾞｯｸ
                @"xlsm",    //Excelﾏｸﾛ有効ﾌﾞｯｸ
                @"xltx",    //Excelﾃﾝﾌﾟﾚｰﾄ
                @"xltm",   //Excelﾏｸﾛ有効ﾃﾝﾌﾟﾚｰﾄ
                @"xlam",   //Excelｱﾄﾞｲﾝ
                @"txt",    //ﾃｷｽﾄ
                @"csv",    //CSV
                @"pdf",    //PDF
                @"zip",    //Zip
            };
            return extensionVals[(int)extension];
        }

        ///<summary>FileType(ファイルダウンロード)</summary>
        public static class FileType
        {
            public const string Excel = "1";
            public const string Csv = "2";
            public const string Pdf = "3";
            public const string Zip = "4";
            public const string UnDefined = "5";
        }
        #endregion

        #region === public static処理 ===
        /// <summary>
        /// ﾌｫﾙﾀﾞﾊﾟｽを取得
        /// </summary>
        /// <returns>ﾌｫﾙﾀﾞﾊﾟｽ</returns>
        public static string GetDirectoryName(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }

        /// <summary>
        /// ﾌｧｲﾙ名を取得
        /// </summary>
        /// <returns>ﾌｧｲﾙ名(※拡張子付き)</returns>
        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        /// <summary>
        /// ﾌｧｲﾙ拡張子を取得
        /// </summary>
        /// <returns>ﾌｧｲﾙ拡張子</returns>
        public static string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// 一時ﾌｫﾙﾀﾞﾊﾟｽを取得
        /// </summary>
        /// <returns>ﾌｧｲﾙﾊﾟｽ</returns>
        public static string GetTempFilePath(string rootPath, string fileName)
        {
            // 実行ディレクトリに一時ファイル用パスを付加
            var dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConstants.TempFileMapPath);

            return Path.Combine(
                dirPath,    // 一時ファイル用パス
                fileName);  // ファイル名
        }

        /// <summary>
        /// ﾌｧｲﾙ拡張子を付与
        /// </summary>
        /// <returns>ﾌｧｲﾙ名</returns>
        public static string SetFileExtension(string fileBaseName, Extension extension)
        {
            if (extension == Extension.UnDefined)
            {
                // 拡張子が未定義の場合はファイル名をそのまま返す
                return fileBaseName;
            }
            string ext = @"." + GetExtensionStr(extension);
            return fileBaseName + (fileBaseName.EndsWith(ext) ? "" : ext);
        }

        /// <summary>
        /// ファイル種類よりファイル拡張子を取得
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static Extension GetFileExtFromFileType(string fileType)
        {
            Extension ext = Extension.ExcelFile;
            switch (fileType)
            {
                case FileType.Excel:
                    ext = Extension.ExcelFile;      //.xlsx
                    break;
                case FileType.Csv:
                    ext = Extension.Csv;      //.csv
                    break;
                case FileType.Pdf:
                    ext = Extension.Pdf;      //.pdf
                    break;
                case FileType.Zip:
                    ext = Extension.Zip;      //.zip
                    break;
                case FileType.UnDefined: //未定義
                    ext = Extension.UnDefined;
                    break;
                default:
                    ext = Extension.ExcelFile;      //.xlsx
                    break;
            }
            return ext;
        }

        /// <summary>
        /// ﾌｧｲﾙ名禁止文字列を置き換え
        ///  - 置き換え文字列：『_』
        /// </summary>
        /// <param name="fileName">ﾌｧｲﾙﾊﾟｽ</param>
        /// <returns></returns>
        public static string ConvertInvalidFileNameChars(string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            string converted = string.Concat(
                fileName.Select(c => invalidChars.Contains(c) ? '_' : c));

            return converted;
        }

        /// <summary>
        /// ﾌｧｲﾙ名に付与するｼｽﾃﾑ日付文字列を取得(_yyMMddHHmmssff)
        /// </summary>
        /// <returns>ｼｽﾃﾑ日付文字列</returns>
        /// <remarks>添付ﾌｫﾙﾀﾞ保存時のﾌｧｲﾙ名がﾕﾆｰｸになるように付与</remarks>
        public static string GetTimeString()
        {
            DateTime now = DateTime.Now;
            return String.Format("_{0:yyMMddHHmmssff}", now);
        }

        /// <summary>
        /// ﾀﾞｳﾝﾛｰﾄﾞ対象ﾌｧｲﾙをﾊﾞｲﾄ配列に読込んで取得
        /// </summary>
        /// <param name="filePath">ﾌｧｲﾙﾊﾟｽ</param>
        public static byte[] GetFileToByteData(string filePath)
        {
            //ﾌｧｲﾙをﾊﾞｲﾄ配列に読込む
            using (System.IO.FileStream fs = System.IO.File.Open(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {

                byte[] bs = new byte[fs.Length];
                //ファイルの内容をすべて読み込む
                fs.Read(bs, 0, bs.Length);
                //閉じる
                fs.Close();

                return bs;
            }

        }

        /// <summary>
        /// ﾌｧｲﾙを削除
        /// </summary>
        /// <param name="filePath">削除ﾌｧｲﾙﾊﾟｽ</param>
        public static void DeleteFile(string filePath)
        {
            try
            {
                if (String.IsNullOrEmpty(filePath)) return;

                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                //何もしない
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
        }

        /// <summary>
        /// ﾌｫﾙﾀﾞを作成
        /// </summary>
        /// <param name="filePath">ﾌｧｲﾙﾊﾟｽ</param>
        public static void CreateDirectory(string filePath)
        {
            try
            {
                if (String.IsNullOrEmpty(filePath)) return;

                //ﾌｫﾙﾀﾞﾊﾟｽ
                string dirPath = Path.GetDirectoryName(filePath);
                //ﾌｫﾙﾀﾞ作成
                Directory.CreateDirectory(dirPath);
            }
            catch (Exception ex)
            {
                //何もしない
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
        }

        /// <summary>
        /// 文字ｺｰﾄﾞを判別して返却
        /// </summary>
        /// <param name="bytes">ﾊﾞｲﾄ配列文字列</param>
        public static System.Text.Encoding DecisionEncoding(byte[] bytes)
        {
            try
            {
                System.Text.Encoding enc = null;
                foreach (byte val in bytes)
                {
                    if (val >= 130 && val <= 152)
                    {
                        //Shift_JISに決定
                        enc = System.Text.Encoding.GetEncoding("Shift_JIS");
                        break;
                    }
                    else if (val >= 161 && val <= 193)
                    {
                        //※ﾍﾞﾝﾍﾞﾙｸﾞ版ではｻﾎﾟｰﾄ対象外とする
                        ////EUCに決定
                        //enc = System.Text.Encoding.Unicode;
                        break;
                    }
                    else if (val >= 208 && val <= 239)
                    {
                        //UTF-8に決定
                        enc = System.Text.Encoding.UTF8;
                        break;
                    }
                    //else if (val >= 194 && val <= 207)
                    //{
                    //    ///判別不能のため、次のコードで判定
                    //}
                }

                return enc;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return null;
            }
        }

        /// <summary>
        /// ｃｓｖファイルデータを列項目ごとに分割
        /// </summary>
        /// <param name="line">1行ﾃﾞｰﾀ</param>
        /// <remarks>※""で囲まれたカンマあり項目を考慮</remarks>
        public static string[] SplitCSV(string line)
        {
            const string dQuoteStr = @"""";     //「"」
            const string dQuote2Str = @"""""";  //「""」

            List<string> itemList = new List<string>();

            //先に""で囲まれたカンマあり項目を対象に分割
            string[] separator1 = { @",""" };     //「,"」
            string[] separator2 = { @"""," };     //「",」

            //「,"」で分割
            string[] dQuoteVals = line.Split(separator1, StringSplitOptions.None);
            if (dQuoteVals.Length > 0)
            {
                for (int ii = 0; ii < dQuoteVals.Length; ii++)
                {
                    string dQuoteVal = dQuoteVals[ii];

                    if (ii == 0 && !dQuoteVal.StartsWith(dQuoteStr))
                    {
                        //※先頭項目が「"」始まりでない場合、カンマ区切りで取得
                        itemList.AddRange(dQuoteVal.Split(','));
                    }
                    else
                    {
                        //「",」で分割
                        string[] dQuoteVals2 = dQuoteVal.Split(separator2, StringSplitOptions.None);
                        if (dQuoteVals2.Length <= 0) continue;

                        //先頭項目はそのまま取得
                        string item = dQuoteVals2[0];
                        //※先頭の「"」を除去
                        if (item.StartsWith(dQuoteStr))
                        {
                            item = item.Substring(1);
                        }
                        //※末尾の「"」を除去
                        if (item.EndsWith(dQuoteStr))
                        {
                            item = item.Substring(0, item.Length - 1);
                        }
                        //※文字列中の「""」→「"」に置換
                        itemList.Add(item.Replace(dQuote2Str, dQuoteStr));

                        for (int idx = 1; idx < dQuoteVals2.Length; idx++)
                        {
                            //2番目以降の項目はカンマ区切りで格納
                            itemList.AddRange(dQuoteVals2[idx].Split(','));
                        }

                    }

                }

            }

            return itemList.ToArray();
        }
        #endregion

        #region === private static処理 ===
        #endregion

    }
}