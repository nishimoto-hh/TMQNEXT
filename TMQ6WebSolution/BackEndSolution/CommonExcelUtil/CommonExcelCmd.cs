using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ClosedXML.Excel;

namespace CommonExcelUtil
{
    /// <summary>
    /// EXCELコマンド処理クラス
    /// </summary>
    public class CommonExcelCmd : IDisposable
    {
        #region 定数定義
        /// <summary>
        /// 範囲指定形式
        /// </summary>
        public enum RangeType : int
        {
            /// <summary>該当なし</summary>
            Invalid,
            /// <summary>"AA99"セル指定形式</summary>
            Cell,
            /// <summary>"ZZ"列指定形式</summary>
            Col,
            /// <summary>"99"行指定形式</summary>
            Row,
            /// <summary>"ZZ99:ZZ99"セル範囲指定形式</summary>
            CellRange,
            /// <summary>"ZZ:ZZ"列範囲指定形式</summary>
            ColRange,
            /// <summary>"99:99"行範囲指定形式</summary>
            RowRange
        }

        /// <summary>Gets 最大行番号</summary>
        /// <value>最大行番号</value>
        public static int MaxRowNumber
        {
            get
            {
                return 1048576;
            }
        }
        /// <summary>Gets 最大列名</summary>
        /// <value>最大列名</value>
        public static string MaxColLetter
        {
            get
            {
                return "XFD";
            }
        }
        #endregion

        #region private変数
        /// <summary>操作対象ワークブック</summary>
        private XLWorkbook workBook;
        /// <summary>操作対象ワークシート1</summary>
        private IXLWorksheet workSheet;
        /// <summary>操作対象ワークシート2</summary>
        private IXLWorksheet workSheet2;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonExcelCmd()
        {
            // ワークブックを新規作成
            workBook = new ClosedXML.Excel.XLWorkbook();
            workSheet = workBook.Worksheet(1);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileFullName">ファイル名</param>
        public CommonExcelCmd(string fileFullName)
        {
            // 指定パスのワークブックを生成(読込)
            workBook = new ClosedXML.Excel.XLWorkbook(fileFullName);
            workSheet = workBook.Worksheet(1);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileStream">Stream</param>
        public CommonExcelCmd(Stream fileStream)
        {
            // Streamからワークブックを生成
            workBook = new ClosedXML.Excel.XLWorkbook(fileStream);
            workSheet = workBook.Worksheet(1);
        }

        ///// <summary>
        ///// コンストラクタ
        ///// </summary>
        ///// <param name="fileStream">Stream</param>
        ///// <param name="delimiter">区切り文字</param>
        //public CommonExcelCmd(Stream fileStream, string delimiter)
        //{
        //    if (string.IsNullOrEmpty(delimiter))
        //    {
        //        // Streamからワークブックを生成
        //        workBook = new ClosedXML.Excel.XLWorkbook(fileStream);
        //    }
        //    else
        //    {
        //        // 新規ワークブックの作成
        //        workBook = new ClosedXML.Excel.XLWorkbook();

        //        // TODO:CSVファイル読み込み

        //    }
        //    workSheet = workBook.Worksheet(1);
        //}
        #endregion

        #region publicメソッド
        /// <summary>
        /// エクセル読込
        /// </summary>
        /// <param name="readSheet">対象シート</param>
        /// <param name="readRange">対象範囲</param>
        /// <param name="result">読み込み結果(2次元配列)</param>
        /// <param name="msg">メッセージ</param>
        /// <returns>処理結果(true:成功/false失敗)</returns>
        public bool ReadExcel(string readSheet, string readRange,
             ref string[,] result, ref string msg)
        {
            msg = string.Empty;
            try
            {
                // シート名　デフォルトは先頭シート
                workSheet = workBook.Worksheet(1);
                if (!string.IsNullOrEmpty(readSheet))
                {
                    workSheet = GetWorkSheet(readSheet, 1);
                    if (workSheet == null)
                    {
                        // 対象シート無し
                        msg = string.Format("Worksheet [{0}] is not exists.", readSheet);
                        return false;
                    }
                }

                // 対象範囲の値を二次元配列へ変換
                result = getCellValuesArray(readRange);
                if (result == null)
                {
                    msg = string.Format("Cell range [{0}] is invalid.", readRange);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// エクセル読込
        /// </summary>
        /// <param name="readSheetNo">対象シート番号</param>
        /// <param name="readRange">対象範囲</param>
        /// <param name="result">読み込み結果(2次元配列)</param>
        /// <param name="msg">メッセージ</param>
        /// <returns>処理結果(true:成功/false失敗)</returns>
        public bool ReadExcelBySheetNo(int readSheetNo, string readRange,
             ref string[,] result, ref string msg)
        {
            msg = string.Empty;
            try
            {
                // シート番号指定で読み込み
                if (readSheetNo < 1 || readSheetNo > workBook.Worksheets.Count)
                {
                    msg = string.Format("Worksheet number [{0}] is invalid.", readSheetNo);
                    return false;
                }
                this.workSheet = workBook.Worksheet(readSheetNo);

                // 対象範囲の値を二次元配列へ変換
                result = getCellValuesArray(readRange);
                if (result == null)
                {
                    msg = string.Format("Cell range [{0}] is invalid.", readRange);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return false;
            }
        }

        ///// <summary>
        ///// PDFファイルの生成
        ///// </summary>
        ///// <param name="fileStream">ExcelファイルのStream</param>
        ///// <param name="msg">メッセージ</param>
        ///// <returns>処理結果(true:成功/false失敗)</returns>
        //public bool MakePdf(ref MemoryStream fileStream, ref string msg)
        //{
        //    msg = string.Empty;
        //    try
        //    {
        //        // PDF保存
        //        workBook.Save(fileStream, SaveFileFormat.Pdf);
        //        fileStream.Seek(0, SeekOrigin.Begin);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.ToString();
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// PDFファイルの生成
        ///// </summary>
        ///// <param name="fileName">Excelファイル</param>
        ///// <param name="msg">メッセージ</param>
        ///// <returns>処理結果(true:成功/false失敗)</returns>
        //public bool MakePdf(string fileName, ref string msg)
        //{
        //    msg = string.Empty;
        //    try
        //    {
        //        // PDF保存
        //        workBook.Save(fileName, SaveFileFormat.Pdf);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.ToString();
        //        return false;
        //    }
        //}

        /// <summary>
        /// 指定されたExcelの範囲指定アドレスが正しい表現かをチェックしてチェック結果を戻す
        /// </summary>
        /// <param name="rangeAddress">範囲指定形式</param>
        /// <returns>
        /// 範囲指定形式
        /// 1:正常（"AA99"セル指定形式）
        /// 2:正常（"ZZ"列指定形式）
        /// 3:正常（"99"行指定形式）
        /// 4:正常（"ZZ99:ZZ99"セル範囲指定形式）
        /// 5:正常（"ZZ:ZZ"列範囲指定形式）
        /// 6:正常（"99:99"行範囲指定形式）
        /// 0:エラー（該当なし）
        /// </returns>
        public RangeType CheckRangeAddressMatch(string rangeAddress)
        {
            // 1:正常（"AA99"セル指定形式）
            if (Regex.IsMatch(rangeAddress, @"^[A-Z]{1,}[1-9]{1}[0-9]*$"))
            {
                return RangeType.Cell;
            }
            // 2:正常（"ZZ"列指定形式）
            else if (Regex.IsMatch(rangeAddress, @"^[A-Z]{1,}$"))
            {
                return RangeType.Col;
            }
            // 3:正常（"99"行指定形式）
            else if (Regex.IsMatch(rangeAddress, @"^[1-9]{1}[0-9]*$"))
            {
                return RangeType.Row;
            }
            // 4:正常（"ZZ99:ZZ99"セル範囲指定形式）
            else if (Regex.IsMatch(rangeAddress, @"^[A-Z]{1,}[1-9]{1}[0-9]*:[A-Z]{1,}[1-9]{1}[0-9]*$"))
            {
                return RangeType.CellRange;
            }
            // 5:正常（"ZZ:ZZ"列範囲指定形式）
            else if (Regex.IsMatch(rangeAddress, @"^[A-Z]{1,}:[A-Z]{1,}$"))
            {
                return RangeType.ColRange;
            }
            // 6:正常（"99:99"行範囲指定形式）
            else if (Regex.IsMatch(rangeAddress, @"^[1-9]{1}[0-9]*:[1-9]{1}[0-9]*$"))
            {
                return RangeType.RowRange;
            }
            else
            {
                return RangeType.Invalid;
            }
        }

        /// <summary>
        /// 範囲指定文字列への変換
        /// </summary>
        /// <param name="rangeAddress">範囲</param>
        /// <returns>変換後の文字列</returns>
        public string ConvertToRangeAddress(string rangeAddress)
        {
            // 範囲指定形式の取得
            var rangeType = CheckRangeAddressMatch(rangeAddress);

            // 範囲指定でない場合、範囲指定に変換
            return ConvertToRangeAddress(rangeType, rangeAddress);
        }

        /// <summary>
        /// 範囲指定文字列への変換
        /// </summary>
        /// <param name="rangeType">範囲タイプ</param>
        /// <param name="rangeAddress">範囲指定形式</param>
        /// <returns>変換後の文字列</returns>
        public string ConvertToRangeAddress(RangeType rangeType, string rangeAddress)
        {
            string range = rangeAddress;
            // 範囲指定でない場合、範囲指定に変換
            if (rangeType == RangeType.Cell
                || rangeType == RangeType.Col
                || rangeType == RangeType.Row)
            {
                range += ":" + range;
            }
            return range;
        }

        /// <summary>
        /// copy：出力対象シート内の行、列、指定範囲セルをコピー
        /// （コピー元、コピー先のシートは同一でも別シートでも可）
        /// </summary>
        /// <param name="param">
        ///  [0]：コピー元行（範囲）
        ///  [1]：コピー先行（範囲）
        ///  [2]：コピー元シート名またはシート番号（未設定時は先頭シート）
        ///  [3]：コピー先シート名またはシート番号（未設定時は先頭シート）
        /// </param>
        public void Copy(string[] param)
        {
            if (param.Length < 2 ||
                (param.Length >= 1 && string.IsNullOrEmpty(param[0])) ||
                (param.Length >= 2 && string.IsNullOrEmpty(param[1])))
            {
                // パラメータ不足
                return;
            }
            string copyFrom = param[0];
            string copyTo = param[1];
            var rangeTypeFrom = CheckRangeAddressMatch(copyFrom);
            if (rangeTypeFrom == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            var rangeTypeTo = CheckRangeAddressMatch(copyTo);
            if (rangeTypeTo == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }

            // 範囲指定でない場合、範囲指定に変換
            copyFrom = ConvertToRangeAddress(rangeTypeFrom, copyFrom);
            copyTo = ConvertToRangeAddress(rangeTypeTo, copyTo);

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }
            workSheet2 = workBook.Worksheet(1);
            if (param.Length >= 4 && !string.IsNullOrEmpty(param[3]))
            {
                workSheet2 = GetWorkSheet(param[3], 1);
                if (workSheet2 == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            workSheet.Range(copyFrom).CopyTo(workSheet2.Range(copyTo));
        }

        /// <summary>
        /// Delete：行削除、行削除（範囲）
        /// </summary>
        /// <param name="param">
        /// [0]：削除対象行、削除対象行範囲
        ///  ※１：MAX指定時
        ///  行の場合は 指定位置～1048576行まで削除する
        ///  列の場合は 指定位置～XFD列まで削除する
        /// [1]：Up・Left(削除方向)（未設定時はUp）
        /// [2]：シート名またはシート番号（未設定時は先頭シート）
        /// </param>
        public void Delete(string[] param)
        {
            if (param.Length < 1 || string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            // 指定位置を取得
            string range = param[0];
            RangeType rangeType;
            if (range.Contains(":MAX"))
            {
                rangeType = CheckRangeAddressMatch(range.Replace(":MAX", ""));
                if (rangeType == RangeType.Col)
                {
                    rangeType = RangeType.ColRange;
                }
                else if (rangeType == RangeType.Row)
                {
                    rangeType = RangeType.RowRange;
                }
            }
            else
            {
                rangeType = CheckRangeAddressMatch(range);
            }
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }

            if (range.Contains("MAX"))
            {
                // MAX指定の場合
                // "99:99"行範囲指定形式の場合
                if (rangeType == RangeType.RowRange)
                {
                    range = range.Replace("MAX", MaxRowNumber.ToString());
                }
                // "ZZ:ZZ"列範囲指定形式の場合
                else if (rangeType == RangeType.ColRange)
                {
                    range = range.Replace("MAX", MaxColLetter);
                }
                else
                {
                    // 指定位置不正
                    return;
                }
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, range);

            // セル指定の削除の場合
            if (rangeType == RangeType.Cell || rangeType == RangeType.CellRange)
            {
                // 削除方向を決定する（デフォルトはUp）
                XLShiftDeletedCells deleteShiftDirection = XLShiftDeletedCells.ShiftCellsUp;
                if (param.Length >= 2 && "LEFT".Equals(param[1].ToUpper()))
                {
                    // Left指定の場合
                    deleteShiftDirection = XLShiftDeletedCells.ShiftCellsLeft;
                }
                // 範囲削除
                workSheet.Range(range).Delete(deleteShiftDirection);
            }
            // 列指定の削除の場合
            else if (rangeType == RangeType.Col || rangeType == RangeType.ColRange)
            {
                // 列範囲削除
                workSheet.Columns(range).Delete();
            }
            // 行指定の削除の場合
            else if (rangeType == RangeType.Row || rangeType == RangeType.RowRange)
            {
                // 行範囲削除
                workSheet.Rows(range).Delete();
            }
        }

        /// <summary>
        /// hidden：出力対象シート内の行、列を非表示
        /// </summary>
        /// <param name="param">
        /// [0]：非表示行・列（範囲）
        /// [1]：シート名またはシート番号（未設定時は先頭シート）
        /// </param>
        public void Hidden(string[] param)
        {
            if (param.Length < 1 || string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }

            // 指定位置を取得
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Cell || rangeType == RangeType.CellRange || rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workSheet = workBook.Worksheet(1);
            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
            {
                workSheet = GetWorkSheet(param[1], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            var range = workSheet.Range(ConvertToRangeAddress(rangeType, param[0]));
            if (rangeType == RangeType.Col || rangeType == RangeType.ColRange)
            {
                // 対象列を非表示
                workSheet.Columns(range.FirstColumn().ColumnNumber(), range.LastColumn().ColumnNumber()).Hide();
            }
            else if (rangeType == RangeType.Row || rangeType == RangeType.RowRange)
            {
                // 対象行を非表示
                workSheet.Rows(range.FirstRow().RowNumber(), range.LastRow().RowNumber()).Hide();
            }
        }

        /// <summary>
        /// copySheet：出力対象Book内のシートコピー
        /// </summary>
        /// <param name="param">
        /// [0]：コピー元シート名またはシート番号　デフォルトは先頭シート
        /// [1]：コピー位置（シート名、シート番号：デフォルトは一番後ろ）
        /// [2]：Before・After　デフォルトはAfter
        /// [3]：シート名またはシート番号（未設定時は標準仕様）
        /// </param>
        public void CopySheet(string[] param)
        {
            // コピー元シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 1)
            {
                workSheet = GetWorkSheet(param[0], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }
            // コピー位置（シート名、シート番号：デフォルトは一番後ろ）
            workSheet2 = workBook.Worksheet(workBook.Worksheets.Count);
            if (param.Length >= 2)
            {
                workSheet2 = GetWorkSheet(param[1], workBook.Worksheets.Count);
                if (workSheet2 == null)
                {
                    // 指定シート無し
                    return;
                }
            }
            // コピー位置のシートのPositionを取得
            int position = workSheet2.Position;

            // Before・After デフォルトはAfter
            if (param.Length >= 3)
            {
                if (string.IsNullOrEmpty(param[2])
                    || !"BEFORE".Equals(param[2].ToUpper()))
                {
                    // Beforeが指定されていない場合はAfter
                    position++;
                }
            }
            else
            {
                // 未指定の場合はAfter
                position++;
            }

            // シート名（未設定時は標準仕様"Sheet"+連番）
            string sheetName = GetDefaultSheetName(workBook);
            if (param.Length >= 4)
            {
                if (!string.IsNullOrEmpty(param[3]))
                {
                    sheetName = param[3];
                    if (workBook.Worksheets.Contains(sheetName))
                    {
                        // 既に指定シートが存在する場合は連番を付加
                        sheetName = GetDefaultSheetName(workBook, sheetName);
                    }
                }
            }

            workSheet.CopyTo(sheetName, position);

            // シートの表示倍率をコピー
            workBook.Worksheet(sheetName).SheetView.ZoomScale = workSheet.SheetView.ZoomScale;
            // シートの枠線表示設定をコピー
            workBook.Worksheet(sheetName).ShowGridLines = workSheet.ShowGridLines;
        }

        /// <summary>
        /// deleteSheet：対象シート削除
        /// </summary>
        /// <param name="param">
        /// [0]：削除対象シート名またはシート番号
        /// </param>
        public void DeleteSheet(string[] param)
        {
            if (param.Length < 1 || string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }

            workSheet = GetWorkSheet(param[0], 1);
            if (workSheet == null)
            {
                // 指定シート無し
                return;
            }
            // 対象シートを削除
            workBook.Worksheet(param[0]).Delete();
        }

        /// <summary>
        /// hiddenSheet：対象シート非表示
        /// </summary>
        /// <param name="param">
        ///  [0]：非表示対象シート名またはシート番号
        ///  [1]：再表示不可フラグ
        /// </param>
        public void HiddenSheet(string[] param)
        {
            if (param.Length < 1 || string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }

            workSheet = GetWorkSheet(param[0], 1);
            if (workSheet == null)
            {
                // 指定シート無し
                return;
            }

            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
            {
                if (param[1].Equals("1"))
                {
                    // 再表示不可フラグが"1"の場合
                    // 対象シートを再表示されない非表示
                    workSheet.Visibility = XLWorksheetVisibility.VeryHidden;
                }
            }
            else
            {
                // 対象シートを非表示
                workSheet.Visibility = XLWorksheetVisibility.Hidden;
            }
        }

        /// <summary>
        /// ShowSheet：対象シート表示
        /// </summary>
        /// <param name="param">
        /// [0]：対象シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void ShowSheet(string[] param)
        {
            // デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 1 && !string.IsNullOrEmpty(param[0]))
            {
                workSheet = GetWorkSheet(param[0], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            // 対象シートを表示
            workSheet.Visibility = XLWorksheetVisibility.Visible;
        }


        /// <summary>
        /// ActivateSheet：シートをアクティブ化
        /// </summary>
        /// <param name="param">
        /// [0]：対象シート名またはシート番号
        /// </param>
        public void ActivateSheet(string[] param)
        {
            if (param.Length < 1 || string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }

            workSheet = GetWorkSheet(param[0], 1);
            if (workSheet == null)
            {
                // 指定シート無し
                return;
            }
            workSheet.SetTabActive();

        }

        /// <summary>
        /// SelectSheet：シートを選択
        /// </summary>
        /// <param name="param">
        /// [0]：対象シート名またはシート番号 デフォルトは先頭シート
        /// </param>
        public void SelectSheet(string[] param)
        {
            // デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 1 && !string.IsNullOrEmpty(param[0]))
            {
                workSheet = GetWorkSheet(param[0], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            // シートを選択
            workSheet.SetTabSelected(true);
        }

        /// <summary>
        /// UnSelectSheet：シートの選択を解除
        /// </summary>
        /// <param name="param">
        /// [0]：対象シート名またはシート番号 デフォルトは先頭シート
        /// </param>
        public void UnSelectSheet(string[] param)
        {
            // デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 1 && !string.IsNullOrEmpty(param[0]))
            {
                workSheet = GetWorkSheet(param[0], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            // シートの選択を解除
            workSheet.SetTabSelected(false);
        }

        /// <summary>
        /// MoveSheet：出力対象Book内のシート移動
        /// </summary>
        /// <param name="param">
        /// [0]：移動元シート（シート名、シート番号：デフォルトは先頭シート）
        /// [1]：移動先シート（シート名、シート番号：デフォルトは先頭シート）
        /// [2]：Before・After・Last　デフォルトはBefore
        /// </param>
        /// <remarks>第1引数のみ指定時は移動元シートを先頭へ移動</remarks>
        public void MoveSheet(string[] param)
        {
            // 移動元シート　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 1)
            {
                workSheet = GetWorkSheet(param[0], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }
            // 移動先シート　デフォルトは先頭シート
            workSheet2 = workBook.Worksheet(1);
            if (param.Length >= 2)
            {
                workSheet2 = GetWorkSheet(param[1], workBook.Worksheets.Count);
                if (workSheet2 == null)
                {
                    // 指定シート無し
                    return;
                }
            }
            // 移動先のシートのPositionを取得
            int position = workSheet2.Position;

            // Before・After・Last デフォルトはBefore
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                switch (param[2].ToUpper())
                {
                    case "AFTER":   // 移動先シートの1つ後ろへ移動
                        position++;
                        break;
                    case "LAST":    // 末尾へ移動
                        position = workBook.Worksheets.Count;
                        break;
                    default:        // 移動先シートの1つ前へ移動
                        position--;
                        break;
                }
            }
            else
            {
                // 未指定の場合はBefore
                position--;
            }
            if(position < 1)
            {
                position = 1;   // 先頭
            }
            else if(position > workBook.Worksheets.Count)
            {
                position = workBook.Worksheets.Count;   // 末尾
            }

            if(workSheet.Position == position)
            {
                // 移動先のシート番号が同じ場合は移動しない
                return;
            }

            // シートを移動
            workSheet.Position = position;

        }

        /// <summary>
        /// AutoFit：自動調整
        /// </summary>
        /// <param name="param">
        ///  [0]：自動調整対象行、列、セル範囲（未指定の場合、全セル範囲）
        ///  [1]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void AutoFit(string[] param)
        {
            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
            {
                workSheet = GetWorkSheet(param[1], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            if (param.Length >= 1 && !string.IsNullOrEmpty(param[0]))
            {
                // 自動調整対象行、列が指定されている場合
                // 対象行、列、セル範囲の取得
                var rangeType = CheckRangeAddressMatch(param[0]);
                if (rangeType == RangeType.Invalid)
                {
                    // 指定位置不正
                    return;
                }
                var range = workSheet.Range(ConvertToRangeAddress(rangeType, param[0]));

                if (rangeType != RangeType.Col && rangeType != RangeType.ColRange)
                {
                    // 対象が列以外の場合
                    // 指定行自動調整
                    workSheet.Rows(range.FirstRow().RowNumber(), range.LastRow().RowNumber()).AdjustToContents();
                }
                if (rangeType != RangeType.Row && rangeType != RangeType.RowRange)
                {
                    // 対象が行以外の場合
                    // 指定列自動調整
                    workSheet.Columns(range.FirstColumn().ColumnNumber(), range.LastColumn().ColumnNumber()).AdjustToContents();
                }
            }
            else
            {
                // 対象範囲が未指定の場合、全セル範囲
                // 全行自動調整
                workSheet.Rows().AdjustToContents();
                // 全列自動調整
                workSheet.Columns().AdjustToContents();
            }

        }

        /// <summary>
        /// Clear：指定シートの数式・文字・書式など全削除
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲
        /// [1]：全てクリア（"1"） or 値のみクリア（"2"）　デフォルトは全てクリア
        /// [2]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void Clear(string[] param)
        {
            if (param.Length < 1 || string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3)
            {
                if (!string.IsNullOrEmpty(param[2]))
                {
                    workSheet = GetWorkSheet(param[2], 1);
                    if (workSheet == null)
                    {
                        // 指定シート無し
                        return;
                    }
                }
            }

            string range = string.Empty;
            // 対象行、列、セル範囲の取得
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            // 全てクリア（"1"） or 値のみクリア（"2"）　デフォルトは全てクリア
            bool allClear = true;
            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
            {
                allClear = !"2".Equals(param[1]);
            }

            if (allClear)
            {
                // 式、値、及び全ての書式設定をクリア
                workSheet.Range(range).Clear(XLClearOptions.All);
            }
            else
            {
                // 式と値をクリア
                workSheet.Range(range).Clear(XLClearOptions.AllContents);
            }
        }

        /// <summary>
        /// Merge：指定シートの数式・文字・書式などを結合、結合解除
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲
        /// [1]：結合（"true"） or 解除（"false"）　デフォルトは結合
        /// [2]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void Merge(string[] param)
        {
            if (param.Length < 1 || string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            string range = string.Empty;
            // 対象行、列、セル範囲の取得
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            // 結合（"true"） or 解除（"false"）　デフォルトは結合
            bool merge = true;
            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
            {
                merge = !"FALSE".Equals(param[1].ToUpper());
            }

            if (merge)
            {
                // 結合
                workSheet.Range(range).Merge(false);
            }
            else
            {
                // 解除
                workSheet.Range(range).Unmerge();
            }
        }

        // ClosedXMLではオートシェイプは操作不能
        ///// <summary>
        ///// deleteShape：名称を指定したオートシェイプを削除する。
        ///// </summary>
        ///// <param name="param">
        /////  [0]：オートシェイプ名称
        /////  [1]：シート名　デフォルトは先頭シート
        ///// </param>
        //public void DeleteShape(string[] param)
        //{
        //    // シート名　デフォルトは先頭シート
        //    workSheet = workBook.Worksheets[0];
        //    if (param.Length >= 2)
        //    {
        //        if (!string.IsNullOrEmpty(param[1]))
        //        {
        //            workSheet = workBook.Worksheets[param[1]];
        //        }
        //    }

        //    // オートシェイプ名称
        //    if (param.Length >= 1)
        //    {
        //        if (!string.IsNullOrEmpty(param[0]))
        //        {
        //            // 指定のシート名がブック内にあれば削除
        //            foreach (IShape shape in workSheet.Shapes)
        //            {
        //                if (shape.Name.Equals(param[0]))
        //                {
        //                    workSheet.Shapes[param[0]].Delete();
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// PrintArea：名称を指定したシートの印刷範囲を指定する。
        /// </summary>
        /// <param name="param">
        ///  [0]：印刷範囲
        ///  [1]：シート名またはシート番号　デフォルトは先頭シート
        ///  [2]：印刷タイトル行
        ///  [3]：印刷タイトル列
        /// </param>
        public void PrintArea(string[] param)
        {
            if (param.Length < 1 && string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }
            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
            {
                workSheet = GetWorkSheet(param[1], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            // 印刷範囲
            string range = string.Empty;
            // 対象行、列、セル範囲の取得
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            // ワークシートに印刷範囲を設定
            workSheet.PageSetup.PrintAreas.Add(range);

            // 印刷タイトル行
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                // 印刷タイトル行の指定位置を取得
                range = param[2];
                rangeType = CheckRangeAddressMatch(range);
                if (rangeType == RangeType.Invalid)
                {
                    // 指定位置不正
                    return;
                }

                // 範囲指定でない場合、範囲指定に変換
                range = ConvertToRangeAddress(rangeType, range);

                // 行指定の場合
                if (rangeType == RangeType.Row || rangeType == RangeType.RowRange)
                {
                    // ワークシートに印刷タイトル行を設定
                    workSheet.PageSetup.SetRowsToRepeatAtTop(range);
                }
            }

            // 印刷タイトル列
            if (param.Length >= 4 && !string.IsNullOrEmpty(param[3]))
            {
                // 印刷タイトル列の指定位置を取得
                range = param[3];
                rangeType = CheckRangeAddressMatch(range);
                if (rangeType == RangeType.Invalid)
                {
                    // 指定位置不正
                    return;
                }

                // 範囲指定でない場合、範囲指定に変換
                range = ConvertToRangeAddress(rangeType, range);

                // 列指定の場合
                if (rangeType == RangeType.Col || rangeType == RangeType.ColRange)
                {
                    // ワークシートに印刷タイトル列を設定
                    workSheet.PageSetup.SetColumnsToRepeatAtLeft(range);
                }
            }

        }

        /// <summary>
        /// PageSetup：名称を指定したシートの印刷範囲を指定する。
        /// </summary>
        /// <param name="param">
        /// [0]：設定値
        /// [1]：ヘッダー・フッターの指定　デフォルトはHeader
        /// [2]：出力位置　デフォルトはLeft
        /// [3]：シート名　デフォルトは先頭シート
        /// </param>
        public void PageSetup(string[] param)
        {
            if (param.Length < 1 || string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 4 && !string.IsNullOrEmpty(param[3]))
            {
                workSheet = GetWorkSheet(param[3], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            // 設定値
            string setValue = param[0];

            // ヘッダー・フッターの指定　デフォルトはHeader
            IXLHeaderFooter hf = workSheet.PageSetup.Header;
            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]) && "FOOTER".Equals(param[1].ToUpper()))
            {
                hf = workSheet.PageSetup.Footer;
            }

            // 出力位置　デフォルトはLeft
            string verticalAlign = "LEFT";
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                verticalAlign = param[2].ToUpper();
            }

            switch (verticalAlign)
            {
                case "LEFT":
                    hf.Left.AddText(setValue);
                    break;
                case "CENTER":
                    hf.Center.AddText(setValue);
                    break;
                case "RIGHT":
                    hf.Right.AddText(setValue);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// FormatLocal：指定範囲に表示形式の設定を行う
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲
        /// [1]：設定フォーマット
        /// [2]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void FormatLocal(string[] param)
        {
            if (param.Length < 2)
            {
                // パラメータ不足
                return;
            }
            if (string.IsNullOrEmpty(param[0]))
            {
                // 対象行、列、セル範囲が指定されている場合のみ実行
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            string range = string.Empty;
            // 対象行、列、セル範囲が指定されている場合
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            // 数値フォーマットを設定
            workSheet.Range(range).Style.NumberFormat.Format = param[1];
        }

        /// <summary>
        /// FontChange：指定したセル範囲のフォントサイズを変更
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲
        /// [1]：変更サイズ
        /// [2]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void FontChange(string[] param)
        {
            if (param.Length < 2)
            {
                // パラメータ不足
                return;
            }
            if (string.IsNullOrEmpty(param[0]) || string.IsNullOrEmpty(param[1]))
            {
                // 対象行、列、セル範囲、変更サイズの両方が指定されている場合のみ実行
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            string range = string.Empty;
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            // 引数が数値の場合のみ変更
            double size;
            if (!double.TryParse(param[1], out size))
            {
                // サイズ指定不正
                return;
            }

            // フォントサイズ設定
            workSheet.Range(range).Style.Font.FontSize = size;
        }

        /// <summary>
        /// Alignment：指定箇所の文字位置を指定
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲
        /// [1]：横の位置　デフォルトは未指定
        /// [2]：縦の位置　デフォルトは未指定
        /// [3]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void Alignment(string[] param)
        {
            if (param.Length < 1 || string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }

            // 指定位置を取得
            string range = string.Empty;
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 4)
            {
                if (!string.IsNullOrEmpty(param[3]))
                {
                    workSheet = GetWorkSheet(param[3], 1);
                    if (workSheet == null)
                    {
                        // 指定シート無し
                        return;
                    }
                }
            }

            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            // 横の位置　デフォルトは未指定
            string horizonalStrAlign = string.Empty;
            XLAlignmentHorizontalValues horizonalAlign = XLAlignmentHorizontalValues.General;
            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
            {
                horizonalStrAlign = param[1];
                switch (horizonalStrAlign)
                {
                    // 中央揃え
                    case "C":
                        horizonalAlign = XLAlignmentHorizontalValues.Center;
                        break;
                    // 左詰
                    case "L":
                        horizonalAlign = XLAlignmentHorizontalValues.Left;
                        break;
                    //  右詰
                    case "R":
                        horizonalAlign = XLAlignmentHorizontalValues.Right;
                        break;
                    default:
                        horizonalStrAlign = string.Empty;
                        break;
                }
            }

            // 縦の位置　デフォルトは未指定
            string verticalStrAlign = string.Empty;
            XLAlignmentVerticalValues verticalAlign = XLAlignmentVerticalValues.Top;
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                verticalStrAlign = param[2];
                switch (verticalStrAlign)
                {
                    // 中央揃え
                    case "C":
                        verticalAlign = XLAlignmentVerticalValues.Center;
                        break;
                    // 下詰
                    case "B":
                        verticalAlign = XLAlignmentVerticalValues.Bottom;
                        break;
                    //  上揃え
                    case "T":
                        verticalAlign = XLAlignmentVerticalValues.Top;
                        break;
                    default:
                        verticalStrAlign = string.Empty;
                        break;
                }
            }

            // 指定がある場合のみ設定
            if (!string.IsNullOrEmpty(horizonalStrAlign))
            {
                workSheet.Range(range).Style.Alignment.Horizontal = horizonalAlign;
            }
            if (!string.IsNullOrEmpty(verticalStrAlign))
            {
                workSheet.Range(range).Style.Alignment.Vertical = verticalAlign;
            }
        }

        /// <summary>
        /// PageBreaks：指定位置に改ページを挿入する
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列（範囲指定の場合は先頭行、列に対して挿入する）
        /// [1]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void PageBreaks(string[] param)
        {
            if (param.Length < 1 || string.IsNullOrEmpty(param[0]))
            {
                // パラメータ不足
                return;
            }

            // 指定位置を取得
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid || rangeType == RangeType.Cell || rangeType == RangeType.CellRange)
            {
                // 指定位置不正
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
            {
                workSheet = GetWorkSheet(param[1], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            var range = workSheet.Range(ConvertToRangeAddress(rangeType, param[0]));

            // 改ページの追加
            if (rangeType == RangeType.Col || rangeType == RangeType.ColRange)
            {
                // 列指定の場合
                // 指定列の後に垂直方向の改ページを追加
                workSheet.PageSetup.AddVerticalPageBreak(range.FirstColumn().ColumnNumber());
            }
            else
            {
                // 行指定の場合
                // 指定行の後に水平方向の改ページを追加
                workSheet.PageSetup.AddHorizontalPageBreak(range.FirstRow().RowNumber());
            }
        }

        /// <summary>
        /// lineBox：指定箇所に罫線を作成、または削除
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲
        /// [1]：罫線の作成位置
        /// [2]：罫線の太さ　デフォルトは細線
        /// [3]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void LineBox(string[] param)
        {
            if (param.Length < 2 ||
                (param.Length >= 1 && string.IsNullOrEmpty(param[0])) ||
                (param.Length >= 2 && string.IsNullOrEmpty(param[1])))
            {
                // パラメータ不足
                return;
            }

            // 指定位置を取得
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 4 && !string.IsNullOrEmpty(param[3]))
            {
                workSheet = GetWorkSheet(param[3], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return;
                }
            }

            // 範囲指定でない場合、範囲指定に変換
            string range = ConvertToRangeAddress(rangeType, param[0]);

            // 罫線のスタイル デフォルトは細線
            string borderParam = string.Empty;
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                borderParam = param[2];
            }
            var borderStyle = getBorderStyle(borderParam);

            // 罫線の作成
            switch (param[1].ToUpper())
            {
                // 上部に作成
                case "T":
                    workSheet.Range(range).Style.Border.TopBorder = borderStyle;
                    break;
                // 下部に作成
                case "B":
                    workSheet.Range(range).Style.Border.BottomBorder = borderStyle;
                    break;
                // 右部に作成
                case "R":
                    workSheet.Range(range).Style.Border.RightBorder = borderStyle;
                    break;
                // 左部に作成
                case "L":
                    workSheet.Range(range).Style.Border.LeftBorder = borderStyle;
                    break;
                // 内側
                case "I":
                    workSheet.Range(range).Style.Border.InsideBorder = borderStyle;
                    break;
                // 外枠
                case "O":
                    workSheet.Range(range).Style.Border.OutsideBorder = borderStyle;
                    break;
                // 格子
                case "IO":
                    // 上下左右の組み合わせの方が内側＆外枠の組み合わせより速度が速くなる
                    workSheet.Range(range).Style.Border.TopBorder = borderStyle;
                    workSheet.Range(range).Style.Border.BottomBorder = borderStyle;
                    workSheet.Range(range).Style.Border.RightBorder = borderStyle;
                    workSheet.Range(range).Style.Border.LeftBorder = borderStyle;
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 罫線のスタイル取得
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <returns>取得したスタイル</returns>
        private XLBorderStyleValues getBorderStyle(string param)
        {
            XLBorderStyleValues style = XLBorderStyleValues.Thin;
            if (!string.IsNullOrEmpty(param))
            {
                switch (param.ToUpper())
                {
                    case "B":
                        style = XLBorderStyleValues.Thick;
                        break;
                    case "M":
                        style = XLBorderStyleValues.Medium;
                        break;
                    case "H":
                        style = XLBorderStyleValues.Hair;
                        break;
                    case "DS":
                        style = XLBorderStyleValues.Dashed;
                        break;
                    case "DSDT":
                        style = XLBorderStyleValues.DashDot;
                        break;
                    case "DSDTDT":
                        style = XLBorderStyleValues.DashDotDot;
                        break;
                    case "DT":
                        style = XLBorderStyleValues.Dotted;
                        break;
                    case "MDS":
                        style = XLBorderStyleValues.MediumDashed;
                        break;
                    case "MDSDT":
                        style = XLBorderStyleValues.MediumDashDot;
                        break;
                    case "MDSDTDT":
                        style = XLBorderStyleValues.MediumDashDotDot;
                        break;
                    case "DB":
                        style = XLBorderStyleValues.Double;
                        break;
                    case "SDSDT":
                        style = XLBorderStyleValues.SlantDashDot;
                        break;
                    case "N":
                        style = XLBorderStyleValues.None;
                        break;
                    default:
                        break;

                }
            }
            return style;
        }

        /// <summary>
        /// CopyRows：出力対象シート内の行をコピーし貼り付け
        /// （コピー元、挿入先のシートは同一でも別シートでも可）
        /// </summary>
        /// <param name="param">
        /// [0]：コピー元行（範囲）
        /// [1]：コピー先行（範囲）
        /// [2]：N数 
        /// [3]：コピー元シート名（未設定時は先頭シート）
        /// [4]：コピー先シート名（未設定時は先頭シート）
        /// </param>
        public void CopyRows(string[] param)
        {
            int copyRow = 0;
            if (!checkCopyInsParam(param, ref copyRow))
            {
                // パラメータ不正
                return;
            }

            // 範囲指定でない場合、範囲指定に変換
            string copyFrom = ConvertToRangeAddress(param[0]);
            string copyTo = ConvertToRangeAddress(param[1]);

            var rangeFrom = workSheet.Range(copyFrom);
            int rowcountFrom = rangeFrom.RowCount();

            var rangeTo = workSheet2.Range(copyTo);

            // N数分だけコピー処理を実施する
            int rowFrom = rangeFrom.FirstRow().RowNumber();
            int rowTo = rangeTo.FirstRow().RowNumber();
            for (int i = 0; i < copyRow; i++)
            {
                rangeFrom.CopyTo(workSheet2.Row(rowTo));
                // コピー元の行の高さをコピー先の行へ設定
                for (int j = 0; j < rowcountFrom; j++)
                {
                    workSheet2.Row(rowTo + j).Height = workSheet.Row(rowFrom + j).Height;

                    // 非表示行の設定
                    if (workSheet.Row(rowFrom + j).IsHidden)
                    {
                        workSheet2.Row(rowTo + j).Hide();
                    }
                }
                rowTo += rowcountFrom;
            }
        }

        /// <summary>
        /// CopyInsRow：出力対象シート内の行をコピーし挿入
        /// （コピー元、挿入先のシートは同一でも別シートでも可）
        /// </summary>
        /// <param name="param">
        /// [0]：コピー元行（範囲）
        /// [1]：挿入先行（範囲）
        /// [2]：N数 
        /// [3]：コピー元シート名（未設定時は先頭シート）
        /// [4]：コピー先シート名（未設定時は先頭シート）
        /// </param>
        public void CopyInsRow(string[] param)
        {
            int copyRow = 0;
            if (!checkCopyInsParam(param, ref copyRow))
            {
                // パラメータ不正
                return;
            }

            // 範囲指定でない場合、範囲指定に変換
            string copyFrom = ConvertToRangeAddress(param[0]);
            string copyTo = ConvertToRangeAddress(param[1]);

            var rangeFrom = workSheet.Range(copyFrom);
            int rowcountFrom = rangeFrom.RowCount();

            var rangeTo = workSheet2.Range(copyTo);

            // コピー元範囲行数*N数分だけ行挿入する [(10:12)の範囲を3件(3行*3件=9行)挿入する]
            rangeTo.InsertRowsBelow(rowcountFrom * copyRow);

            // N数分だけコピー処理を実施する
            int rowFrom = rangeFrom.FirstRow().RowNumber();
            int rowTo = rangeTo.FirstRow().RowNumber();
            for (int i = 0; i < copyRow; i++)
            {
                rangeFrom.CopyTo(workSheet2.Row(rowTo));
                // コピー元の行の高さをコピー先の行へ設定
                for (int j = 0; j < rowcountFrom; j++)
                {
                    workSheet2.Row(rowTo + j).Height = workSheet.Row(rowFrom + j).Height;
                }
                rowTo += rowcountFrom;
            }
        }

        /// <summary>
        /// copyInsCol：出力対象シート内の列をコピーし挿入
        /// （コピー元、挿入先のシートは同一でも別シートでも可）
        /// </summary>
        /// <param name="param">
        /// [0]：コピー元列（範囲） 
        /// [1]：挿入先列（範囲）
        /// [2]：N数
        /// [3]：コピー元シート名（未設定時は先頭シート）
        /// [4]：コピー先シート名（未設定時は先頭シート）
        /// </param>
        public void CopyInsCol(string[] param)
        {
            int copyColCnt = 0;
            if (!checkCopyInsParam(param, ref copyColCnt))
            {
                // パラメータ不正
                return;
            }

            // 範囲指定でない場合、範囲指定に変換
            string copyFrom = ConvertToRangeAddress(param[0]);
            string copyTo = ConvertToRangeAddress(param[1]);

            var rangeFrom = workSheet.Range(copyFrom);
            int colcountFrom = rangeFrom.ColumnCount();

            var rangeTo = workSheet2.Range(copyTo);

            // コピー元範囲列数*N数分だけ行挿入する [(A:C)の範囲を3件(3列*3件=9列)挿入する]
            rangeTo.InsertColumnsAfter(colcountFrom * copyColCnt);

            // N数分だけコピー処理を実施する
            int i = rangeTo.FirstColumn().ColumnNumber();
            for (int j = 0; j < copyColCnt; j++)
            {
                copyTo = ToAlphabet(i) + ":" + ToAlphabet(i);
                rangeFrom.CopyTo(workSheet2.Range(copyTo));
                i += colcountFrom;
            }
        }

        /// <summary>
        /// copyInsRange：出力対象シート内のセル範囲をコピーし挿入
        /// （コピー元、挿入先のシートは同一でも別シートでも可）
        /// </summary>
        /// <param name="param">
        /// [0]：コピー元セル（範囲）
        /// [1]：挿入先セル（範囲）
        /// [2]：挿入後のセルのシフト方向（未設定時は右方向）
        /// [3]：コピー元シート名（未設定時は先頭シート）
        /// [4]：コピー先シート名（未設定時は先頭シート）
        /// </param>
        public void CopyInsRange(string[] param)
        {
            int dummyNCnt = 0;  // ダミーのパラメータ
            if (!checkCopyInsParam(param, ref dummyNCnt, false))
            {
                // パラメータ不正
                return;
            }

            // 範囲指定でない場合、範囲指定に変換
            string copyFrom = ConvertToRangeAddress(param[0]);
            string copyTo = ConvertToRangeAddress(param[1]);

            var rangeFrom = workSheet.Range(copyFrom);
            int rowCntFrom = rangeFrom.RowCount();
            int colCntFrom = rangeFrom.ColumnCount();

            var rangeTo = workSheet2.Range(copyTo);

            // コピー先のセル範囲の左上のセルの位置を取得
            var topRowNo = rangeTo.FirstRow().RowNumber();
            var bottomRowNo = workSheet2.LastRowUsed().RowNumber();
            var leftColNo = rangeTo.FirstColumn().ColumnNumber();
            var rightColNo = workSheet2.LastColumnUsed().ColumnNumber();

            string shiftType = string.Empty;
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                shiftType = param[2];
            }

            if ("DOWN".Equals(shiftType.ToUpper()))
            {
                // 下方向にシフトの場合
                for (int col = 0; col < colCntFrom; col++)
                {
                    // コピー先セル範囲の左上のセル位置からコピー元のセル範囲の行数×列数分セルを挿入
                    workSheet2.Cell(topRowNo, leftColNo + col).InsertCellsAbove(rowCntFrom);
                }

            }
            else
            {
                // 右方向にシフトの場合
                for (int row = 0; row < rowCntFrom; row++)
                {
                    // コピー先セル範囲の左上のセル位置からコピー元のセル範囲の列数×行数分セルを挿入
                    workSheet2.Cell(topRowNo + row, leftColNo).InsertCellsBefore(colCntFrom);
                }
            }

            // コピー処理
            rangeFrom.CopyTo(workSheet2.Range(copyTo));
        }

        /// <summary>
        /// シート名の変更
        /// </summary>
        /// <param name="param">
        /// [0]：変更対象シート名またはシート番号
        /// [1]：変更後シート名
        /// </param>
        public void ChangeSheetName(string[] param)
        {
            if (param.Length < 2 ||
               (param.Length >= 2 && string.IsNullOrEmpty(param[1])))
            {
                // パラメータ不足
                return;
            }

            // 変更対象シート　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (!string.IsNullOrEmpty(param[0]))
            {
                workSheet = GetWorkSheet(param[0], 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return;
                }
            }

            string sheetNameFrom = param[0];
            string sheetNameTo = param[1];
            if (workBook.Worksheets.Contains(sheetNameTo) &&
                workBook.Worksheet(sheetNameFrom).Position != workBook.Worksheet(sheetNameTo).Position)
            {
                // 指定シート名がワークブックに既に存在する場合は連番を付加
                sheetNameTo = GetDefaultSheetName(workBook, sheetNameTo);
            }

            // シート名を設定
            workBook.Worksheet(sheetNameFrom).Name = sheetNameTo;
        }

        /// <summary>
        /// ConditionalFormat：条件付き書式
        /// </summary>
        /// <param name="param">
        ///  [0]：対象行、列、セル範囲
        ///  [1]：条件付き書式区分
        ///  [2]：条件
        ///  [3]：書式
        ///  [4]：シート名またはシート番号 デフォルトは先頭シート
        /// </param>
        public void ConditionalFormat(string[] param)
        {
            if (param.Length < 5 ||
                (param.Length >= 1 && string.IsNullOrEmpty(param[0])) ||
                (param.Length >= 2 && string.IsNullOrEmpty(param[1])) ||
                (param.Length >= 3 && string.IsNullOrEmpty(param[2])) ||
                (param.Length >= 4 && string.IsNullOrEmpty(param[3])))
            {
                // パラメータ不足
                return;
            }

            // 対象行、列、セル範囲の取得
            string range = string.Empty;
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 5 && !string.IsNullOrEmpty(param[4]))
            {
                workSheet = GetWorkSheet(param[4], 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return;
                }
            }

            // 条件付き書式を指定する
            switch (param[1])
            {
                case "NotContainsNumFormat":
                    // 条件：特定の文字列を含まない
                    // 書式：設定フォーマット
                    workSheet.Range(range).AddConditionalFormat()
                    .WhenNotContains(param[2])            // 条件
                    .NumberFormat.Format = param[3];      // 書式
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// BackgroundColor：指定範囲の背景色の塗りつぶしを行う
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲
        /// [1]：背景色
        /// [2]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void BackgroundColor(string[] param)
        {
            if (param.Length < 2)
            {
                // パラメータ不足
                return;
            }
            if (string.IsNullOrEmpty(param[0]))
            {
                // 対象行、列、セル範囲が指定されている場合のみ実行
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return;
                }
            }

            string range = string.Empty;
            // 対象行、列、セル範囲が指定されている場合
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            if (string.IsNullOrEmpty(param[1]))
            {
                // 背景色が指定されている場合のみ実行
                return;
            }
            // 背景色を設定
            if (!"NOCOLOR".Equals(param[1].ToUpper()))
            {
                // 色指定
                workSheet.Range(range).Style.Fill.BackgroundColor = XLColor.FromHtml(param[1]);
            }
            else
            {
                // 塗りつぶしなし
                workSheet.Range(range).Style.Fill.BackgroundColor = XLColor.NoColor;
            }
        }

        /// <summary>
        /// FontColor：指定範囲の文字色の設定を行う
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲
        /// [1]：文字色
        /// [2]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void FontColor(string[] param)
        {
            if (param.Length < 2)
            {
                // パラメータ不足
                return;
            }
            if (string.IsNullOrEmpty(param[0]))
            {
                // 対象行、列、セル範囲が指定されている場合のみ実行
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return;
                }
            }

            string range = string.Empty;
            // 対象行、列、セル範囲が指定されている場合
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            if (string.IsNullOrEmpty(param[1]))
            {
                // 文字色が指定されている場合のみ実行
                return;
            }
            // 文字色を設定
            workSheet.Range(range).Style.Font.FontColor = XLColor.FromHtml(param[1]);
        }

        /// <summary>
        /// formulaA1：計算式の設定
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲
        /// [1]：計算式
        /// [2]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void FormulaA1(string[] param)
        {
            if (param.Length < 2)
            {
                // パラメータ不足
                return;
            }
            if (string.IsNullOrEmpty(param[0]))
            {
                // 対象行、列、セル範囲が指定されている場合のみ実行
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return;
                }
            }

            string range = string.Empty;
            // 対象行、列、セル範囲が指定されている場合
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            if (string.IsNullOrEmpty(param[1]))
            {
                // 計算式が指定されている場合のみ実行
                return;
            }
            // 計算式を設定
            workSheet.Range(range).FormulaA1 = param[1];
        }

        /// <summary>
        /// SetCellComment：セルコメントの設定
        /// </summary>
        /// <param name="param">
        /// [0]：対象セル
        /// [1]：コメント
        /// [2]：シート名またはシート番号　デフォルトは先頭シート
        /// [3]：追加フラグ　"1"：追加、省略時または左記以外の値は更新
        /// [4]：表示フラグ　"1"：表示、省略時または左記以外の値は非表示
        /// [5]：幅　省略時の場合は自動調整
        /// [6]：高さ　省略時の場合は自動調整
        /// </param>
        public void SetCellComment(string [] param)
        {
            if (param.Length < 2)
            {
                // パラメータ不足
                return;
            }
            if (string.IsNullOrEmpty(param[0]))
            {
                // 対象行、列、セル範囲が指定されている場合のみ実行
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return;
                }
            }

            // 対象セルが指定されている場合
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType != RangeType.Cell)
            {
                // セル指定でない場合、指定位置不正
                return;
            }

            string commentText = param[1];
            if (string.IsNullOrEmpty(commentText))
            {
                // コメントが指定されている場合のみ実行
                return;
            }

            var cell = workSheet.Cell(param[0]);
            IXLComment comment;
            if (cell.HasComment)
            {
                // コメントが存在する場合
                comment = cell.GetComment();
                if (param.Length >= 4 && "1".Equals(param[3]))
                {
                    // 追加フラグがONの場合は改行を追加
                    comment.AddNewLine();
                }
                else
                {
                    // 追加フラグが上記以外の場合、一旦削除
                    comment = cell.CreateComment();
                }
            }
            else
            {
                comment = cell.CreateComment();
            }
            // コメントを追加
            comment.AddText(commentText);
            

            if (param.Length >= 5 && "1".Equals(param[4]))
            {
                // 表示フラグがONの場合、コメントを表示
                comment.SetVisible();
            }

            // コメントサイズの自動調整をON
            comment.Style.Size.AutomaticSize = true;

            if (param.Length >= 6 && !string.IsNullOrEmpty (param[5]))
            {
                // 幅が指定されている場合、コメントの幅を設定
                comment.Style.Size.SetWidth(Convert.ToInt64(param[5]));
            }

            if (param.Length >= 7 && !string.IsNullOrEmpty(param[6]))
            {
                // 高さが指定されている場合、コメントの高さを設定
                comment.Style.Size.SetHeight(Convert.ToInt64(param[6]));
            }
        }

        /// <summary>
        /// ClearCellComment：セルコメントのクリア
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲、シート全体の場合は「ALL」指定
        /// [1]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void ClearCellComment(string[] param)
        {
            if (param.Length < 1)
            {
                // パラメータ不足
                return;
            }
            if (string.IsNullOrEmpty(param[0]))
            {
                // 対象行、列、セル範囲が指定されている場合のみ実行
                return;
            }

            // シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
            {
                workSheet = GetWorkSheet(param[1], 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return;
                }
            }

            if (!"ALL".Equals(param[0].ToUpper()))
            {
                string range = string.Empty;
                // 対象行、列、セル範囲が指定されている場合
                var rangeType = CheckRangeAddressMatch(param[0]);
                if (rangeType == RangeType.Invalid)
                {
                    // 指定位置不正
                    return;
                }
                // 範囲指定でない場合、範囲指定に変換
                range = ConvertToRangeAddress(rangeType, param[0]);

                // コメントを削除
                workSheet.Range(range).DeleteComments();
            }
            else
            {
                // シート全体が対象の場合
                workSheet.DeleteComments();
            }
        }

        /// <summary>
        /// SetHyperLink：ハイパーリンクの設定
        /// </summary>
        /// <param name="param">
        /// [0]：対象セル
        /// [1]：リンク先セル
        /// [2]：対象シート名またはシート番号　デフォルトは先頭シート
        /// [3]：リンク先シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void SetHyperLink(string[] param)
        {
            if (param.Length < 2)
            {
                // パラメータ不足
                return;
            }
            if (string.IsNullOrEmpty(param[0]))
            {
                // 対象行、列、セル範囲が指定されている場合のみ実行
                return;
            }

            // 対象シート名　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return;
                }
            }

            // 対象シート名　デフォルトは先頭シート
            workSheet2 = workBook.Worksheet(1);
            if (param.Length >= 4 && !string.IsNullOrEmpty(param[3]))
            {
                workSheet2 = GetWorkSheet(param[3], 1);
                if (workSheet2 == null)
                {
                    // 対象シート無し
                    return;
                }
            }

            // 対象セルが指定されている場合
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType != RangeType.Cell)
            {
                // セル指定でない場合、指定位置不正
                return;
            }

            if (string.IsNullOrEmpty(param[1]))
            {
                // リンク先が指定されている場合のみ実行
                return;
            }
            rangeType = CheckRangeAddressMatch(param[1]);
            if (rangeType != RangeType.Cell)
            {
                // セル指定でない場合、指定位置不正
                return;
            }

            // ハイパーリンクを設定
            workSheet.Cell(param[0]).SetHyperlink(new XLHyperlink("'" + workSheet2.Name + "'!" + param[1]));
        }

        /// <summary>
        /// SetValue：セル値の設定
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲
        /// [1]：設定値
        /// [2]：シート名またはシート番号　デフォルトは先頭シート
        /// </param>
        public void SetCellValue(string[] param)
        {
            if (param.Length < 2)
            {
                // パラメータ不足
                return;
            }
            if (string.IsNullOrEmpty(param[0]))
            {
                // 対象行、列、セル範囲が指定されている場合のみ実行
                return;
            }

            // シート名またはシート番号　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                workSheet = GetWorkSheet(param[2], 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return;
                }
            }

            string range = string.Empty;
            // 対象行、列、セル範囲が指定されている場合
            var rangeType = CheckRangeAddressMatch(param[0]);
            if (rangeType == RangeType.Invalid)
            {
                // 指定位置不正
                return;
            }
            // 範囲指定でない場合、範囲指定に変換
            range = ConvertToRangeAddress(rangeType, param[0]);

            if (string.IsNullOrEmpty(param[1]))
            {
                // 計算式が指定されている場合のみ実行
                return;
            }
            // 値を設定
            workSheet.Range(range).Value = param[1];
        }

        /// <summary>
        /// マッピング実施
        /// </summary>
        /// <param name="values">マッピング情報リスト</param>
        /// <param name="sheetName">対象シート名（シート名が未指定の場合はシート番号で指定）</param>
        /// <param name="sheetNo">対象シート番号（シート番号が未指定の場合は先頭シート）</param>
        public void SetValue(List<MappingInfo> values, string sheetName = "", int sheetNo = 0)
        {
            if (!string.IsNullOrEmpty(sheetName))
            {
                if (!workBook.TryGetWorksheet(sheetName, out workSheet))
                {
                    // 対象シート無し
                    return;
                }
                workSheet = workBook.Worksheet(sheetName);
            }
            else
            {
                if (sheetNo > workBook.Worksheets.Count)
                {
                    // 対象シート無し
                    return;
                }
                if (sheetNo < 1)
                {
                    // シート番号が未指定の場合は先頭シート
                    workSheet = workBook.Worksheet(1);
                }
                else
                {
                    workSheet = workBook.Worksheet(sheetNo);
                }
            }
            foreach (MappingInfo val in values)
            {
                if (val == null || string.IsNullOrEmpty(val.ToString()))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(val.Format))
                {
                    workSheet.Cell(val.Y + 1, val.X + 1).Style.NumberFormat.Format = val.Format;
                }
                workSheet.Cell(val.Y + 1, val.X + 1).Value = val.Value;
            }
        }

        /// <summary>
        /// シート名の初期値を取得
        /// </summary>
        /// <param name="book">対象ブック</param>
        /// <param name="sheetNameFixed">固定名(未指定時は"Sheet")</param>
        /// <returns>シート名：[固定名]＋[連番]</returns>
        public string GetDefaultSheetName(XLWorkbook book, string sheetNameFixed = "Sheet")
        {
            int maxNo = 0;
            foreach (var workSheet in book.Worksheets)
            {
                var match = Regex.Match(workSheet.Name, sheetNameFixed + @"(?<no>\d+)");
                if (match.Success && match.Groups["no"] != null)
                {
                    int tmpNo = int.Parse(match.Groups["no"].Value);
                    if (tmpNo > maxNo)
                    {
                        maxNo = tmpNo;
                    }
                }
            }
            return string.Format(sheetNameFixed + "{0}", maxNo + 1);
        }

        /// <summary>
        /// シートの取得
        /// </summary>
        /// <param name="param">シート名またはシート番号</param>
        /// <param name="defNo">デフォルトシート番号</param>
        /// <returns>シート</returns>
        public IXLWorksheet GetWorkSheet(string param, int defNo)
        {
            IXLWorksheet workSheet = null;
            if (!string.IsNullOrEmpty(param))
            {
                // 引数が数値かどうかで判断
                int no;
                if (int.TryParse(param, out no))
                {
                    if (no < 1 || no > workBook.Worksheets.Count)
                    {
                        // シート番号不正
                        return null;
                    }
                    // シート番号でシートを指定
                    workSheet = workBook.Worksheet(no);
                }
                else
                {
                    // シート名でシートを指定
                    if (!workBook.TryGetWorksheet(param, out workSheet))
                    {
                        // 指定シート無し
                        return null;
                    }
                }
            }
            if (workSheet == null)
            {
                workSheet = workBook.Worksheet(defNo);
            }
            return workSheet;
        }

        /// <summary>
        /// GetSheetName：シート名取得
        /// </summary>
        /// <param name="param">シート名またはシート番号</param>
        public string GetSheetName(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                // パラメータ不足
                return string.Empty;
            }

            workSheet = GetWorkSheet(param, 1);
            if (workSheet == null)
            {
                // 指定シート無し
                return string.Empty;
            }
            return workSheet.Name;
        }

        /// <summary>
        /// ブック保存
        /// </summary>
        public void Save()
        {
            // ファイル書き込み
            workBook.Save();
        }

        /// <summary>
        /// ブック保存
        /// </summary>
        /// <param name="ms">ストリーム</param>
        public void Save(ref MemoryStream ms)
        {
            // ファイル書き込みではなくmemorystreamに書き込み
            workBook.SaveAs(ms);
            ms.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// ブック保存
        /// </summary>
        /// <param name="fileFullName">ファイルフルネーム</param>
        public void Save(string fileFullName)
        {
            // ファイル書き込み
            workBook.SaveAs(fileFullName);
        }

        /// <summary>
        /// 列数値をアルファベット変換
        /// </summary>
        /// <param name="columnNo">列番号</param>
        /// <returns>アルファベット</returns>
        public string ToAlphabet(int columnNo)
        {
            string alphabet = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
            string columnStr = string.Empty;
            int m = 0;

            do
            {
                m = columnNo % 26;
                columnStr = alphabet[m] + columnStr;
                columnNo = columnNo / 26;
            }
            while (columnNo > 0 && m != 0);

            return columnStr;
        }
        /// <summary>
        /// アルファベットから数値列変換
        /// </summary>
        /// <param name="colStr">列のアルファベット</param>
        /// <returns>列番号</returns>
        public int ToColNumber(string colStr)
        {
            return convertString(0, new Queue<char>(colStr.ToCharArray()));
        }
        /// <summary>
        /// LockSheet：シート保護
        /// </summary>
        /// <param name="param">
        /// [0]：シート名　デフォルトは先頭シート
        /// [1]：パスワード　デフォルトは未設定
        /// </param>
        public void LockSheet(string[] param)
        {
            if (param.Length < 1)
            {
                // パラメータ不足
                return;
            }

            if (!string.IsNullOrEmpty(param[0]))
            {
                if (!workBook.TryGetWorksheet(param[0], out workSheet))
                {
                    // 対象シート無し
                    return;
                }
                workSheet = workBook.Worksheet(param[0]);
            }
            else
            {
                workSheet = workBook.Worksheet(1);
            }

            string password = string.Empty;
            if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
            {
                password = param[1];
            }

            // シートに対してロックをかける
            if (password == string.Empty)
            {
                workSheet.Protect();
            }
            else
            {
                workSheet.Protect(password);
            }
        }
        /// <summary>
        /// LockCell：セル保護
        /// </summary>
        /// <param name="param">
        /// [0]：対象行、列、セル範囲　デフォルトはシート全体
        /// [1]：シート名　デフォルトは先頭シート
        /// [2]：パスワード　デフォルトは未設定
        /// </param>
        public void LockCell(string[] param)
        {
            if (param.Length < 2)
            {
                // パラメータ不足
                return;
            }

            if (!string.IsNullOrEmpty(param[1]))
            {
                if (!workBook.TryGetWorksheet(param[1], out workSheet))
                {
                    // 対象シート無し
                    return;
                }
                workSheet = workBook.Worksheet(param[1]);
            }
            else
            {
                workSheet = workBook.Worksheet(1);
            }

            string range = string.Empty;
            if (param.Length >= 1 && !string.IsNullOrEmpty(param[0]))
            {
                // 対象行、列、セル範囲が指定されている場合
                var rangeType = CheckRangeAddressMatch(param[0]);
                if (rangeType == RangeType.Invalid)
                {
                    // 指定位置不正
                    return;
                }
                // 範囲指定でない場合、範囲指定に変換
                range = ConvertToRangeAddress(rangeType, param[0]);
            }

            string password = string.Empty;
            if (param.Length >= 3 && !string.IsNullOrEmpty(param[2]))
            {
                password = param[2];
            }

            // 解除したい範囲で解除設定
            // ロックがかかっていれば解除
            if (range != string.Empty)
            {
                workSheet.Range(range).Style.Protection.SetLocked(false);
            }

            // シートに対してロックをかける
            if (password == string.Empty)
            {
                workSheet.Protect();
            }
            else
            {
                workSheet.Protect(password);
            }
        }

        /// <summary>
        /// GetLastRowNo：データ最終行の取得
        /// </summary>
        /// <param name="param">シート名またはシート番号　デフォルトは先頭シート</param>
        /// <returns></returns>
        public int GetLastRowNo(string param = "")
        {
            // シート名またはシート番号　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (!string.IsNullOrEmpty(param))
            {
                workSheet = GetWorkSheet(param, 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return -1;
                }
            }
            return workSheet.LastRowUsed().RowNumber();
        }

        /// <summary>
        /// GetLastColumnLetter：データ最終列の取得
        /// </summary>
        /// <param name="param">シート名またはシート番号　デフォルトは先頭シート</param>
        /// <returns></returns>
        public string GetLastColumnLetter(string param)
        {
            // シート名またはシート番号　デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (!string.IsNullOrEmpty(param))
            {
                workSheet = GetWorkSheet(param, 1);
                if (workSheet == null)
                {
                    // 対象シート無し
                    return string.Empty;
                }
            }
            return workSheet.LastColumnUsed().ColumnLetter();
        }

        /// <summary>
        /// 列単位に値をマッピング
        /// </summary>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="address">開始セル</param>
        /// <param name="data">設定データ</param>
        /// <param name="formatList">フォーマット</param>
        /// <param name="numFlg">数値フラグ(数値の場合右寄せ)</param>
        public void InsertData(int sheetNo, string address, object[] data, List<MappingInfo> formatList, bool numFlg)
        {
            //対象シート
            workSheet = workBook.Worksheet(sheetNo);
            //列の値設定
            var range = workSheet.Cell(address).InsertData(data);
            //フォーマット設定
            if (range != null && formatList.Any(x => !string.IsNullOrEmpty(x.Format)))
            {
                range.Style.NumberFormat.Format = formatList.Select(x => x.Format).FirstOrDefault();
            }
            if (range != null && numFlg)
            {
                //数値列の場合、右寄せ
                range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            }
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 指定セル範囲の値を取得
        /// </summary>
        /// <param name="readRange">対象行、列、セル範囲</param>
        /// <returns>セル範囲の値</returns>
        private string[,] getCellValuesArray(string readRange)
        {
            string[,] result = null;

            var rangeType = CheckRangeAddressMatch(readRange);
            if (rangeType == RangeType.Invalid)
            {
                // 指定範囲不正
                return null;
            }

            // 範囲指定でない場合、範囲指定に変換
            readRange = ConvertToRangeAddress(rangeType, readRange);
            var range = workSheet.Range(readRange);

            // 2次元配列に変換
            var cells = workSheet.Range(readRange).Cells();
            int minColNo = range.FirstColumn().ColumnNumber();
            int minRowNo = range.FirstRow().RowNumber();

            result = new string[range.RowCount(), range.ColumnCount()];
            foreach (var cell in cells)
            {
                int row = cell.Address.RowNumber - minRowNo;
                int col = cell.Address.ColumnNumber - minColNo;
                // 2023/02/10 フォーマットされた値を取得するよう変更
                //result[row, col] = cell.Value.ToString();
                result[row, col] = cell.GetFormattedString();
            }
            return result;
        }

        /// <summary>
        /// checkCopyInsParam：コピー＆挿入用パラメータのチェック
        /// </summary>
        /// <param name="param">
        /// [0]：コピー元行（範囲）
        /// [1]：挿入先行（範囲）
        /// [2]：N数
        /// [3]：コピー元シート名（未設定時は先頭シート）
        /// [4]：コピー先シート名（未設定時は先頭シート）
        /// </param>
        /// <param name="cnt">行数</param>
        /// <param name="checkNCnt">N数のチェックを行うかどうか</param>
        /// <returns>true:正常　false:エラー</returns>
        private bool checkCopyInsParam(string[] param, ref int cnt, bool checkNCnt = true)
        {
            if (param.Length < 2 ||
               (param.Length >= 1 && string.IsNullOrEmpty(param[0])) ||
               (param.Length >= 2 && string.IsNullOrEmpty(param[1])) ||
               (checkNCnt && (param.Length < 3 || string.IsNullOrEmpty(param[2]))))
            {
                // パラメータ不足
                return false;
            }

            // コピー元指定位置を取得
            var rangeTypeFrom = CheckRangeAddressMatch(param[0]);
            if (rangeTypeFrom == RangeType.Invalid)
            {
                // 指定位置不正
                return false;
            }

            // コピー先指定位置を取得
            string copyTo = param[1];
            var rangeTypeTo = CheckRangeAddressMatch(param[1]);
            if (rangeTypeTo == RangeType.Invalid)
            {
                // 指定位置不正
                return false;
            }

            if (checkNCnt && !int.TryParse(param[2], out cnt))
            {
                // N数不正
                return false;
            }

            // コピー元シート名 デフォルトは先頭シート
            workSheet = workBook.Worksheet(1);
            if (param.Length >= 4)
            {
                workSheet = GetWorkSheet(param[3], 1);
                if (workSheet == null)
                {
                    // 指定シート無し
                    return false;
                }
            }
            // コピー先シート名 デフォルトは先頭シート
            workSheet2 = workBook.Worksheet(1);
            if (param.Length >= 5)
            {
                workSheet2 = GetWorkSheet(param[4], 1);
                if (workSheet2 == null)
                {
                    // 指定シート無し
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 文字列へ変換
        /// </summary>
        /// <param name="ret">戻り値</param>
        /// <param name="chars">文字列</param>
        /// <returns>処理結果</returns>
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

        /// <summary>
        /// 計算
        /// </summary>
        /// <param name="c">文字</param>
        /// <param name="times">乗数</param>
        /// <returns>計算結果</returns>
        private int calcDecimal(char c, int times)
        {
            return (int)Math.Pow(AzLength, times) * ((int)c - OffsetNum);
        }

        /// <summary>
        ///  'Z' = 90
        /// </summary>
        private static readonly int AzLength = (int)'Z' - (int)'A' + 1;

        /// <summary>
        /// 'A' = 25
        /// </summary>
        private static readonly int OffsetNum = (int)'A' - 1; // 64
        #endregion

        #region IDisposable Support

        /// <summary>
        /// 重複する呼び出しを検出するには
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.workBook != null)
                    {
                        this.workBook.Dispose();
                    }
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~CommonExcelCmd() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        /// <summary>
        /// このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        /// </summary>
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
