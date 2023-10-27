using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace CommonExcelUtil
{
    /// <summary>
    /// EXCEL出力クラス
    /// </summary>
    public class CommonExcelUtil
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private CommonExcelUtil()
        {
        }


        /// <summary>
        /// EXCEL生成
        /// </summary>
        /// <param name="excelTemplate">エクセルテンプレート名</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="exlPrtInfoList">マッピングリスト</param>
        /// <param name="exlCmdInfoList">コマンドリスト</param>
        /// <param name="ms">出力エクセルファイル</param>
        /// <param name="msg">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool CreateExcelFile(
            string excelTemplate,
            string excelTemplatePath,
            string userId,
            List<CommonExcelPrtInfo> exlPrtInfoList,
            List<CommonExcelCmdInfo> exlCmdInfoList,
            ref MemoryStream ms,
            ref string msg)
        {
            // ファイルではなくメモリストリームで返す
            // EXCEL生成クラス
            CommonExcelPrt exlPrt = new CommonExcelPrt(excelTemplate, excelTemplatePath, userId);
            // マッピング情報リストセット
            exlPrt.SetPrtInfo(exlPrtInfoList);
            // コマンド情報リストセット
            exlPrt.SetCmdInfo(exlCmdInfoList);
            // EXCEL作成
            return exlPrt.Print(ref ms, ref msg);
        }

        /// <summary>
        /// EXCEL生成
        /// </summary>
        /// <param name="excelTemplate">エクセルテンプレート名</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="exlPrtInfoList">マッピングリスト</param>
        /// <param name="exlCmdInfoList">コマンドリスト</param>
        /// <param name="ms">出力エクセルファイル</param>
        /// <param name="msg">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool CreateExcelFile(
            string excelTemplate,
            string userId,
            List<CommonExcelPrtInfo> exlPrtInfoList,
            List<CommonExcelCmdInfo> exlCmdInfoList,
            ref MemoryStream ms,
            ref string msg)
        {
            // ファイルではなくメモリストリームで返す
            // EXCEL生成クラス
            CommonExcelPrt exlPrt = new CommonExcelPrt(excelTemplate, userId);
            // マッピング情報リストセット
            exlPrt.SetPrtInfo(exlPrtInfoList);
            // コマンド情報リストセット
            exlPrt.SetCmdInfo(exlCmdInfoList);
            // EXCEL作成
            return exlPrt.Print(ref ms, ref msg);
        }

        /// <summary>
        /// EXCEL生成
        /// </summary>
        /// <param name="excelTemplate">エクセルテンプレート名</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="exlPrtInfoList">マッピングリスト</param>
        /// <param name="exlCmdInfoList">コマンドリスト</param>
        /// <param name="excelFile">エクセルファイル名</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="excelFilePath">エクセルファイルパス</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool CreateExcelFile(
            string excelTemplate,
            string userId,
            List<CommonExcelPrtInfo> exlPrtInfoList,
            List<CommonExcelCmdInfo> exlCmdInfoList,
            string excelFile,
            ref string msg,
            ref string excelFilePath)
        {
            // ファイルで返す
            // EXCEL生成クラス
            CommonExcelPrt exlPrt = new CommonExcelPrt(excelTemplate, userId);
            // マッピング情報リストセット
            exlPrt.SetPrtInfo(exlPrtInfoList);
            // コマンド情報リストセット
            exlPrt.SetCmdInfo(exlCmdInfoList);
            // EXCEL作成
            return exlPrt.Print(excelFile, ref msg, ref excelFilePath);
        }



        /// <summary>
        /// EXCEL読込
        /// </summary>
        /// <param name="readFilePath">読込み対象エクセルパス</param>
        /// <param name="readSheet">読込み対象シート</param>
        /// <param name="readRange">読込範囲(A1:AB200など)</param>
        /// <param name="result">読込み結果(2次元配列)</param>
        /// <param name="msg">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool ReadExcelFile(string readFilePath, string readSheet, string readRange,
            ref string[,] result, ref string msg)
        {
            // エクセルコマンドクラス
            CommonExcelCmd cmd = new CommonExcelCmd(readFilePath);

            // 読込実行
            return cmd.ReadExcel(readSheet, readRange, ref result, ref msg);
        }

        /// <summary>
        /// EXCEL読込
        /// </summary>
        /// <param name="fileStream">読込み対象エクセルStream</param>
        /// <param name="readSheet">読込み対象シート</param>
        /// <param name="readRange">読込範囲(A1:AB200など)</param>
        /// <param name="result">読込み結果(2次元配列)</param>
        /// <param name="msg">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool ReadExcelFile(Stream fileStream, string readSheet, string readRange,
            ref string[,] result, ref string msg)
        {
            // エクセルコマンドクラス
            CommonExcelCmd cmd = new CommonExcelCmd(fileStream);

            // 読込実行
            return cmd.ReadExcel(readSheet, readRange, ref result, ref msg);
        }

        /// <summary>
        /// EXCEL読込
        /// </summary>
        /// <param name="fileStreams">読込み対象エクセルStream</param>
        /// <param name="readSheet">読込み対象シート</param>
        /// <param name="readRange">読込範囲(A1:AB200など)</param>
        /// <param name="resultList">読込み結果(2次元配列)</param>
        /// <param name="msg">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool ReadExcelFiles(List<Stream> fileStreams, string readSheet, string readRange,
            ref List<string[,]> resultList, ref string msg)
        {
            resultList = new List<string[,]>();

            foreach (var fs in fileStreams)
            {
                string[,] result = null;
                if (!ReadExcelFile(fs, readSheet, readRange, ref result, ref msg))
                {
                    return false;
                }
                resultList.Add(result);
            }
            return true;
        }

        ///// <summary>
        ///// EXCEL読込
        ///// </summary>
        ///// <param name="fileStreams">読込み対象エクセルStream</param>
        ///// <param name="readSheet">読込み対象シート</param>
        ///// <param name="readRange">読込範囲(A1:AB200など)</param>
        ///// <param name="result">読込み結果(2次元配列)</param>
        ///// <returns></returns>
        //public static bool ReadExcelFile(Stream fileStream, string readSheet, string readRange,
        //    ref string[,] result, ref string msg)
        //{
        //        // エクセルコマンドクラス
        //        CommonExcelCmd cmd = new CommonExcelCmd(fileStream);

        //        // 読込実行
        //        result = null;
        //        if (!cmd.ReadExcel(readSheet, readRange, ref result, ref msg))
        //        {
        //            return false;
        //        }
        //    return true;
        //}

        /// <summary>
        /// PDF生成
        /// </summary>
        /// <param name="excelFilePath">変換元エクセルファイルパス</param>
        /// <param name="pdfFileStream">メモリストリーム</param>
        /// <param name="msg">出力PDFファイル</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool CreatePdfFile(string excelFilePath, ref MemoryStream pdfFileStream, ref string msg)
        {
            // エクセルコマンドクラス
            CommonExcelCmd cmd = new CommonExcelCmd(excelFilePath);

            // PDFファイル生成⇒未実装
            //return cmd.MakePdf(ref pdfFileStream, ref msg);
            throw new Exception("PDF file generation is not implemented.");
        }

        /// <summary>
        /// PDF生成
        /// </summary>
        /// <param name="excelFileStream">変換元エクセルファイルストリーム</param>
        /// <param name="pdfFileStream">変換先PDFファイルストリーム</param>
        /// <param name="msg">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool CreatePdfFile(MemoryStream excelFileStream, ref MemoryStream pdfFileStream, ref string msg)
        {
            // エクセルコマンドクラス
            CommonExcelCmd cmd = new CommonExcelCmd(excelFileStream);

            // PDFファイル生成⇒未実装
            //return cmd.MakePdf(ref pdfFileStream, ref msg);
            throw new Exception("PDF file generation is not implemented.");
        }

        /// <summary>
        /// PDF生成
        /// </summary>
        /// <param name="excelFilePath">変換元エクセルファイルパス</param>
        /// <param name="msg">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool CreatePdfFile(string excelFilePath, ref string msg)
        {
            // エクセルコマンドクラス
            CommonExcelCmd cmd = new CommonExcelCmd(excelFilePath);

            // PDFファイル名
            string fileName = excelFilePath.Replace(".xlsx", ".pdf");

            // PDFファイル生成⇒未実装
            //return cmd.MakePdf(fileName, ref msg);
            throw new Exception("PDF file generation is not implemented.");
        }

        /// <summary>
        /// エクセルファイルをPDFに変換
        /// </summary>
        /// <param name="inputPath">変換対象のエクセルファイルパス</param>
        /// <param name="command">ubuntuに渡す命令</param>
        /// <param name="exePath">エクセルファイルパス</param>
        /// <param name="outputPath">出力先のフォルダパス</param>
        /// <returns>実行成否</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool convertToPdfFile(string inputPath, string command, string exePath, string outputPath)
        {
            //パスを保存
            string originalPath = inputPath;

            //エクセルファイルの存在チェック
            if (string.IsNullOrEmpty(inputPath) || !File.Exists(inputPath))
            {
                return false;
            }

            //pdf出力先のフォルダの存在チェック
            if (string.IsNullOrWhiteSpace(outputPath) || !Directory.Exists(outputPath))
            {
                return false;
            }

            //パス先頭のドライブ名を取得
            string driveName = inputPath.FirstOrDefault().ToString();

            //パスのドライブ名を仮想ドライブに変更　（例）C:inetpub/wwwroot → /mnt/c/User
            inputPath = inputPath.Replace(driveName.ToUpper() + ":", "/mnt/" + driveName.ToLower());
            outputPath = outputPath.Replace(driveName.ToUpper() + ":", " /mnt/" + driveName.ToLower());

            //パスの"\"を"/"に変更
            inputPath = inputPath.Replace(@"\", "/");
            outputPath = outputPath.Replace(@"\", "/");

            // コマンドから外部アプリケーションを実行
            Process p = new Process();

            // 出力場所にディレクトリを移動
            string changeDir = "cd " + outputPath + " ; pwd ;";

            // バッチファイルに渡す引数（変換対象のファイル名のパス）を設定
            p.StartInfo.Arguments = "run " + "\"" + changeDir + " " + command + " " + inputPath + "\"";

            // 汎用マスタから取得した命令を設定
            //p.StartInfo.FileName = "C:\\inetpub\\wwwroot\\akap2_ut\\Batch\\ubuntu.exe";
            //p.StartInfo.FileName = "C:\\Program Files\\WindowsApps\\CanonicalGroupLimited.UbuntuonWindows_2004.2020.424.0_x64__79rhkp1fndgsc\\ubuntu.exe";
            p.StartInfo.FileName = exePath;

            ////標準出力、標準エラー出力を取得できるようにする
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;

            //実行
            p.Start();

            //コンソール終了まで待機
            p.WaitForExit();

            //標準出力を取得
            string stext = p.StandardOutput.ReadToEnd();

            //標準エラー出力を取得
            string etext = p.StandardError.ReadToEnd();

            //プロセスの状態を取得　終了していればtrue：続いていればfalse
            bool status = p.HasExited;

            //拡張子.xlsc を.pdfに変換
            originalPath = Path.ChangeExtension(originalPath, ".pdf");

            //指定先にpdfファイルがあればtrue：無ければfalse
            if (File.Exists(originalPath))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// マッピング情報(行単位)の生成
        /// </summary>
        /// <param name="list">マッピング値リスト</param>
        /// <param name="sheetName">シート名</param>
        /// <param name="startRowNo">開始行番号</param>
        /// <param name="startColName">開始列名</param>
        /// <returns>マッピング情報</returns>
        public static List<CommonExcelPrtInfo> CreateMappingList(
            List<object[]> list, string sheetName, int startRowNo, string startColName)
        {
            var mappingList = new List<CommonExcelPrtInfo>();
            int rowNo = startRowNo;

            foreach (var data in list)
            {
                string address = startColName + rowNo++;
                var info = new CommonExcelPrtInfo();
                info.SetSheetName(sheetName);
                info.SetExlSetRowValueByAddress(address, data);
                mappingList.Add(info);
            }

            return mappingList;
        }

        /// <summary>
        /// マッピング情報(セル単位)の生成
        /// </summary>
        /// <param name="list">マッピング値リスト</param>
        /// <param name="sheetName">シート名</param>
        /// <returns>マッピング情報</returns>
        public static List<CommonExcelPrtInfo> CreateMappingList(List<object[]> list, string sheetName)
        {
            var mappingList = new List<CommonExcelPrtInfo>();

            foreach (var data in list)
            {
                string address = data[0].ToString();
                var info = new CommonExcelPrtInfo();
                info.SetSheetName(sheetName);
                info.SetExlSetValueByAddress(address, data[1]);
                mappingList.Add(info);
            }

            return mappingList;
        }

        #region ベタ表コマンド情報作成

        /// <summary>
        /// ベタ表用のコマンド情報を作成する処理
        /// </summary>
        /// <param name="sheetName">シート名</param>
        /// <param name="rowCount">データ行数</param>
        /// <param name="startRow">開始行</param>
        /// <param name="startCol">開始列</param>
        /// <param name="endCol">最終列</param
        /// <param name="recordCount">レコード行数</param>
        /// <returns>コマンド情報リスト</returns>
        public static List<CommonExcelCmdInfo> CreateCmdInfoListForSimpleList(string sheetName, int rowCount, int startRow, string startCol, string endCol, int recordCount)
        {
            List<CommonExcelCmdInfo> cmdInfoList = new List<CommonExcelCmdInfo>();
            // コマンド情報生成（出力対象シート内の行をコピーし貼り付け）
            cmdInfoList.AddRange(createCmdCopyInsRow(sheetName, rowCount, startRow, recordCount));
            // コマンド情報生成（行高自動調整）
            cmdInfoList.AddRange(createCmdAutoFit(sheetName));
            // コマンド情報生成（印刷範囲設定）
            cmdInfoList.AddRange(createCmdPrintArea(sheetName, "A1:" + endCol + (rowCount + startRow - 1).ToString(), "1:" + (startRow - 1).ToString()));
            // コマンド情報生成（罫線設定）
            cmdInfoList.AddRange(commandLineBox(startCol + startRow + ":" + endCol + (startRow + (recordCount * rowCount) - 1), sheetName));

            return cmdInfoList;
        }

        /// <summary>
        /// コマンド情報生成：出力対象シート内の行をコピーし貼り付け
        /// </summary>
        /// <param name="sheetName">シート名</param>
        /// <param name="resultCount">検索結果件数</param>
        /// <param name="copySourceRow">コピー元行（範囲）</param>
        /// <param name="recordCount">レコード行数</param>
        /// <returns>コマンド情報</returns>
        private static List<CommonExcelCmdInfo> createCmdCopyInsRow(string sheetName, int resultCount, int copySourceRow, int recordCount)
        {
            // コマンド情報リスト
            List<CommonExcelCmdInfo> cmdInfoList = new List<CommonExcelCmdInfo>();
            // コマンド情報
            CommonExcelCmdInfo cmdInfo;
            // コマンドパラメータ
            string[] param;

            if (resultCount > 1)
            {
                // コピー元行(範囲)
                string copyFromRange = copySourceRow.ToString() + ":" + (copySourceRow + recordCount - 1).ToString();
                // コピー先行(範囲)
                string copyToRange = (copySourceRow + recordCount).ToString();

                // コマンド情報生成
                cmdInfo = new CommonExcelCmdInfo();
                param = new string[5];
                param[0] = copyFromRange;                       // [0]：コピー元行（範囲）
                param[1] = copyToRange;                         // [1]：コピー先行（範囲）
                param[2] = Convert.ToString(resultCount - 1);   // [2]：N数
                param[3] = sheetName;                           // [3]：コピー元シート名（未設定時は先頭シート）
                param[4] = sheetName;                           // [4]：コピー先シート名（未設定時は先頭シート）
                cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgBefore, CommonExcelCmdInfo.CExecCmdCopyRows, param);
                cmdInfoList.Add(cmdInfo);
            }

            return cmdInfoList;
        }

        /// <summary>
        /// コマンド情報生成：行高自動調整
        /// </summary>
        /// <param name="sheetName">シート名</param>
        /// <returns>コマンド情報</returns>
        private static List<CommonExcelCmdInfo> createCmdAutoFit(string sheetName)
        {
            // コマンド情報リスト
            List<CommonExcelCmdInfo> cmdInfoList = new List<CommonExcelCmdInfo>();
            // コマンド情報
            CommonExcelCmdInfo cmdInfo;
            // コマンドパラメータ
            string[] param;

            cmdInfo = new CommonExcelCmdInfo();
            param = new string[2];
            param[0] = null;                                    // [0]：自動調整対象行、列、セル範囲（未指定の場合、全セル範囲）
            param[1] = sheetName;                               // [1]：シート名　デフォルトは先頭シート
            cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdAutoFit, param);
            cmdInfoList.Add(cmdInfo);

            return cmdInfoList;
        }

        /// <summary>
        /// コマンド情報生成：印刷範囲設定
        /// </summary>
        /// <param name="sheetName">シート名</param>
        /// <param name="range">印刷範囲</param>
        /// <param name="titleRow">タイトル行(範囲)</param>
        /// <param name="titleCol">タイトル列(範囲)</param>
        /// <returns>コマンド情報</returns>
        private static List<CommonExcelCmdInfo> createCmdPrintArea(string sheetName, string range, string titleRow = null, string titleCol = null)
        {
            // コマンド情報リスト
            List<CommonExcelCmdInfo> cmdInfoList = new List<CommonExcelCmdInfo>();
            // コマンド情報
            CommonExcelCmdInfo cmdInfo;
            // コマンドパラメータ
            string[] param;

            cmdInfo = new CommonExcelCmdInfo();
            param = new string[4];
            param[0] = range;                                   // [0]：印刷範囲
            param[1] = sheetName;                               // [1]：シート名　デフォルトは先頭シート
            param[2] = titleRow;                                // [2]：印刷タイトル行
            param[3] = titleCol;                                // [3]：印刷タイトル列
            cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdPrintArea, param);
            cmdInfoList.Add(cmdInfo);

            return cmdInfoList;
        }

        /// <summary>
        /// コマンド情報生成：罫線設定
        /// </summary>
        /// <param name="cellRange">セル範囲</param>
        /// <param name="sheetName">シート名</param>
        /// <returns>コマンド情報</returns>
        private static List<CommonExcelCmdInfo> commandLineBox(string cellRange, string sheetName)
        {
            List<CommonExcelCmdInfo> cmdInfoList = new List<CommonExcelCmdInfo>();

            for (int i = 0; i < 6; i++)
            {
                CommonExcelCmdInfo cmdInfo = new CommonExcelCmdInfo();
                string[] param = new string[4];
                string borderIndex = string.Empty;

                switch (i)
                {
                    case 1:
                        // 上部に作成
                        borderIndex = "T";
                        break;
                    case 2:
                        // 下部に作成
                        borderIndex = "B";
                        break;
                    case 3:
                        // 右部に作成
                        borderIndex = "R";
                        break;
                    case 4:
                        // 左部に作成
                        borderIndex = "L";
                        break;
                    case 5:
                        // 内側水平罫線
                        borderIndex = "IH";
                        break;
                    default:
                        // 内側垂直罫線
                        borderIndex = "IV";
                        break;
                }

                param[0] = cellRange;   // [0]：セル範囲
                param[1] = borderIndex; // [1]：罫線の作成位置
                param[2] = "";          // [2]：罫線の太さ、デフォルトは細線
                param[3] = sheetName;   // [3]：シート名　デフォルトは先頭シート
                cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdLineBox, param);
                cmdInfoList.Add(cmdInfo);
            }

            return cmdInfoList;
        }
        #endregion

    }
}