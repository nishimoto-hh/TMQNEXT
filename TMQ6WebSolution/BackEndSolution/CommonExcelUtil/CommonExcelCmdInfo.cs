using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonExcelUtil
{
    /// <summary>
    /// EXCELコマンド情報クラス
    /// </summary>
    public class CommonExcelCmdInfo
    {
        private const int CMaxCnt = 10;

        public const string CExecTmgBefore = "1";
        public const string CExecTmgAfter = "2";

        /// <summary>コピー</summary>
        public const string CExecCmdCopy = "Copy";
        /// <summary>削除</summary>
        public const string CExecCmdDelete = "Delete";
        /// <summary>非表示</summary>
        public const string CExecCmdHidden = "Hidden";
        /// <summary>シートコピー</summary>
        public const string CExecCmdCopySheet = "CopySheet";
        /// <summary>シート削除</summary>
        public const string CExecCmdDeleteSheet = "DeleteSheet";
        /// <summary>シート非表示</summary>
        public const string CExecCmdHiddenSheet = "HiddenSheet";
        /// <summary>シート表示</summary>
        public const string CExecCmdShowSheet = "ShowSheet";
        /// <summary>シート移動</summary>
        public const string CExecCmdMoveSheet = "MoveSheet";
        /// <summary>シートアクティブ化</summary>
        public const string CExecCmdActivateSheet = "ActivateSheet";
        /// <summary>行高自動調整</summary>
        public const string CExecCmdAutoFit = "AutoFit";
        /// <summary>クリア</summary>
        public const string CExecCmdClear = "Clear";
        /// <summary>セル結合</summary>
        public const string CExecCmdMerge = "Merge";
        /// <summary>オートシェイプ削除</summary>
        public const string CExecCmdDeleteShape = "DeleteShape";
        /// <summary>印刷範囲指定</summary>
        public const string CExecCmdPrintArea = "PrintArea";
        /// <summary>ヘッダ・フッタの設定</summary>
        public const string CExecCmdPageSetup = "PageSetup";
        /// <summary>表示形式設定</summary>
        public const string CExecCmdFormatLocal = "FormatLocal";
        /// <summary>フォントサイズ変更</summary>
        public const string CExecCmdFontChange = "FontChange";
        /// <summary>文字位置指定</summary>
        public const string CExecCmdAlignment = "Alignment";
        /// <summary>改ページ挿入</summary>
        public const string CExecCmdPageBreaks = "PageBreaks";
        /// <summary>罫線の作成、削除</summary>
        public const string CExecCmdLineBox = "LineBox";
        /// <summary>行コピーしたセルの貼り付け</summary>
        public const string CExecCmdCopyRows = "CopyRows";
        /// <summary>行コピーしたセルの挿入</summary>
        public const string CExecCmdCopyInsRow = "CopyInsRow";
        /// <summary>列コピーしたセルの挿入</summary>
        public const string CExecCmdCopyInsCol = "CopyInsCol";
        /// <summary>行コピーしたセルの挿入</summary>
        public const string CExecCmdCopyInsRange = "CopyInsRange";
        /// <summary>電子印作成</summary>
        public const string CExecCmdMakeStamp = "MakeStamp";
        /// <summary>シート名の変更</summary>
        public const string CExecCmdChangeSheetName = "ChangeSheetName";
        /// <summary>条件付き書式</summary>
        public const string CExecCmdContionFormat = "ConditionFormat";
        /// <summary>背景色の塗りつぶし</summary>
        public const string CExecCmdBackgroundColor = "BackgroundColor";
        /// <summary>計算式</summary>
        public const string CExecCmdFormulaA1 = "FormulaA1";
        /// <summary>シートの一部ロック</summary>
        public const string CExecCmdLockSheet = "LockSheet";
        /// <summary>シートの一部ロック</summary>
        public const string CExecCmdLockCell = "LockCell";

        /// <summary>条件付き書式タイプ</summary>
        /// <summary>
        /// 条件：特定の文字列を含まない
        /// 書式：設定フォーマット
        /// </summary>
        public const string cConFormatNotContainsNumFormat = "NotContainsNumFormat";

        private string execTmgCode; // 1:データ出力前、2:データ出力後
        private string execCmd;
        private string[] execPram;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonExcelCmdInfo()
        {
            this.execCmd = string.Empty;
            this.execPram = new string[CMaxCnt];
            this.execTmgCode = CExecTmgBefore;
        }

        /// <summary>
        /// コマンド情報セット
        /// </summary>
        /// <param name="execTmgCode">execTmgCode</param>
        /// <param name="execCmd">execCmd</param>
        /// <param name="execPram">execPram</param>
        public void SetExlCmdInfo(string execTmgCode, string execCmd, string[] execPram)
        {
            this.execTmgCode = execTmgCode;
            this.execCmd = execCmd;
            this.execPram = execPram;
        }

        /// <summary>
        /// コマンドパラメータ取得
        /// </summary>
        /// <returns>コマンドパラメータ</returns>
        public string[] GetExecPram()
        {
            return this.execPram;
        }

        /// <summary>
        /// コマンド取得
        /// </summary>
        /// <returns>コマンド</returns>
        public string GetExecCmd()
        {
            return this.execCmd;
        }

        /// <summary>
        /// コマンドコード(実行タイミング)取得
        /// </summary>
        /// <returns>コマンドコード</returns>
        public string GetExecTmgCode()
        {
            return this.execTmgCode;
        }
    }

}
