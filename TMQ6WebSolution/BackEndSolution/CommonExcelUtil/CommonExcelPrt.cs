using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;

namespace CommonExcelUtil
{
    /// <summary>
    /// EXCEL印刷クラス
    /// </summary>
    public class CommonExcelPrt
    {
        // テンプレートファイルの保存先
        private string templateFolder;
        // 作成ファイルの保存先
        private string saveFolder;
        // テンプレートファイル名
        private string templateFile;
        // 作成ファイル名
        private string saveFile;

        private List<CommonExcelPrtInfo> lstPrtInfo;
        private List<CommonExcelCmdInfo> lstCmdInfo;

        private CommonExcelCmd exlCmdMain;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tempFileName">ファイル名</param>
        /// <param name="tempFilePath">ファイルパス</param>
        /// <param name="userId">ユーザーID</param>
        public CommonExcelPrt(string tempFileName, string tempFilePath, string userId)
        {

            // 実行パス取得
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            // テンプレートフォルダ取得
            this.templateFolder = Path.Combine(appPath, CommonWebTemplate.AppCommonObject.Config.AppSettings.ExcelTemplateDir);
            this.templateFolder = Path.Combine(this.templateFolder, tempFilePath);
            // 出力先フォルダ取得
            this.saveFolder = Path.Combine(appPath, CommonWebTemplate.AppCommonObject.Config.AppSettings.ExcelOutputDir);

            this.templateFile = tempFileName;
            this.saveFile = userId + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + tempFileName;

            lstPrtInfo = new List<CommonExcelPrtInfo>();
            lstCmdInfo = new List<CommonExcelCmdInfo>();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tempFileName">ファイル名</param>
        /// <param name="userId">ユーザーID</param>
        public CommonExcelPrt(string tempFileName, string userId)
        {

            // 実行パス取得
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            // テンプレートフォルダ取得
            this.templateFolder = Path.Combine(appPath, CommonWebTemplate.AppCommonObject.Config.AppSettings.ExcelTemplateDir);

            // 出力先フォルダ取得
            this.saveFolder = Path.Combine(appPath, CommonWebTemplate.AppCommonObject.Config.AppSettings.ExcelOutputDir);

            this.templateFile = tempFileName;
            this.saveFile = userId + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + tempFileName;

            lstPrtInfo = new List<CommonExcelPrtInfo>();
            lstCmdInfo = new List<CommonExcelCmdInfo>();
        }
        /// <summary>Gets メッセージ</summary>
        /// <value>メッセージ</value>
        public string Message { get; private set; }

        /// <summary>
        /// EXCEL生成実行
        /// </summary>
        /// <param name="ms">メモリストリーム</param>
        /// <param name="msg">メッセージ</param>
        /// <returns>true:正常　false:エラー</returns>
        public bool Print(ref MemoryStream ms, ref string msg)
        {
            msg = string.Empty;

            try
            {
                // テンプレートファイル取得
                if (!isExistTemplateFile(templateFile))
                {
                    msg = "<テンプレートファイルが見つかりません。>";
                    return false;
                }

                //// 出力ファイルの作成（テンプレートファイルからのコピー）
                //bRtn = canMakeOutputFile(this.templateFile, this.saveFile);
                //if (bRtn == false) return bRtn;

                exlCmdMain = new CommonExcelCmd(Path.Combine(templateFolder, templateFile));

                // 命令データの実行（データ出力前）
                if (lstCmdInfo != null && lstCmdInfo.Count > 0)
                {
                    if (!canExecCmd(lstCmdInfo, CommonExcelCmdInfo.CExecTmgBefore))
                    {
                        msg = "<命令データの実行に失敗しました。>";
                        return false;
                    }
                }

                // データ出力
                if (lstPrtInfo != null && lstPrtInfo.Count > 0)
                {
                    if (!canOutputData(lstPrtInfo))
                    {
                        msg = "<データの出力に失敗しました。>";
                        return false;
                    }
                }

                // 命令データの実行（データ出力後）
                if (lstCmdInfo != null && lstCmdInfo.Count > 0)
                {
                    if (!canExecCmd(lstCmdInfo, CommonExcelCmdInfo.CExecTmgAfter))
                    {
                        msg = "<命令データの実行に失敗しました。>";
                        return false;
                    }
                }

                exlCmdMain.Save(ref ms);

                return true;
            }
            catch (Exception ex)
            {
                // 例外メッセージを設定
                msg = ex.ToString();
                return false;
            }
            finally
            {
                // クラス破棄
                exlCmdMain = null;
            }
        }

        /// <summary>
        /// EXCEL生成実行
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>true:正常　false:エラー</returns>
        public bool Print(string fileName, ref string msg, ref string filePath)
        {
            msg = string.Empty;

            try
            {
                // テンプレートファイル取得
                if (!isExistTemplateFile(templateFile))
                {
                    msg = "<テンプレートファイルが見つかりません。>";
                    return false;
                }

                // 出力ファイルの作成（テンプレートファイルからのコピー）
                //bRtn = canMakeOutputFile(this.templateFile, this.saveFile);
                //if (bRtn == false) return bRtn;

                exlCmdMain = new CommonExcelCmd(Path.Combine(templateFolder, templateFile));

                // 命令データの実行（データ出力前）
                if (lstCmdInfo != null && lstCmdInfo.Count > 0)
                {
                    if (!canExecCmd(lstCmdInfo, CommonExcelCmdInfo.CExecTmgBefore))
                    {
                        msg = "<命令データの実行に失敗しました。>";
                        return false;
                    }
                }

                // データ出力
                if (lstPrtInfo != null && lstPrtInfo.Count > 0)
                {
                    if (!canOutputData(lstPrtInfo))
                    {
                        msg = "<データの出力に失敗しました。>";
                        return false;
                    }
                }

                // 命令データの実行（データ出力後）
                if (lstCmdInfo != null && lstCmdInfo.Count > 0)
                {
                    if (!canExecCmd(lstCmdInfo, CommonExcelCmdInfo.CExecTmgAfter))
                    {
                        msg = "<命令データの実行に失敗しました。>";
                        return false;
                    }
                }

                // ファイルパス
                filePath = Path.Combine(saveFolder, fileName);

                // 出力先ディレクトリの生成
                string folderName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folderName))
                {
                    DirectoryInfo di = Directory.CreateDirectory(folderName);
                }

                // ブック保存
                exlCmdMain.Save(filePath);

                return true;
            }
            catch (Exception ex)
            {
                // 例外メッセージを設定
                msg = ex.ToString();
                return false;
            }
            finally
            {
                // クラス破棄
                exlCmdMain = null;
            }
        }

        /// <summary>
        /// マッピング実行
        /// </summary>
        /// <param name="lstPrtInfo">lstPrtInfo</param>
        /// <returns>true:正常　false:エラー</returns>
        private bool canOutputData(List<CommonExcelPrtInfo> lstPrtInfo)
        {
            try
            {
                foreach (CommonExcelPrtInfo prtInfo in lstPrtInfo)
                {
                    if (prtInfo.GetColFlg())
                    {
                        //列単位にマッピング
                        exlCmdMain.InsertData(prtInfo.GetSheetNo(), prtInfo.GetAdress(), prtInfo.GetColData(), prtInfo.GetExlOutputData(), prtInfo.GetNumFlg());
                    }
                    else
                    {
                        //セル単位にマッピング
                        exlCmdMain.SetValue(prtInfo.GetExlOutputData(), prtInfo.GetSheetName(), prtInfo.GetSheetNo());
                    }
                }
            }
            catch (Exception ex)
            {
                this.Message = ex.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="lstCmdInfo">lstCmdInfo</param>
        /// <param name="execTmgCode">execTmgCode</param>
        /// <returns>true:正常　false:エラー</returns>
        private bool canExecCmd(List<CommonExcelCmdInfo> lstCmdInfo, string execTmgCode)
        {
            try
            {
                foreach (CommonExcelCmdInfo cmdInfo in lstCmdInfo)
                {
                    if (cmdInfo.GetExecTmgCode().Equals(execTmgCode) == false)
                    {
                        continue;
                    }

                    switch (cmdInfo.GetExecCmd())
                    {
                        case CommonExcelCmdInfo.CExecCmdCopy:
                            exlCmdMain.Copy(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdDelete:
                            exlCmdMain.Delete(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdHidden:
                            exlCmdMain.Hidden(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdCopySheet:
                            exlCmdMain.CopySheet(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdDeleteSheet:
                            exlCmdMain.DeleteSheet(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdHiddenSheet:
                            exlCmdMain.HiddenSheet(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdShowSheet:
                            exlCmdMain.ShowSheet(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdMoveSheet:
                            exlCmdMain.MoveSheet(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdActivateSheet:
                            exlCmdMain.ActivateSheet(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdAutoFit:
                            exlCmdMain.AutoFit(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdClear:
                            exlCmdMain.Clear(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdMerge:
                            exlCmdMain.Merge(cmdInfo.GetExecPram());
                            break;
                        //case CommonExcelCmdInfo.cExecCmdDeleteShape:
                        //    exlCmdMain.DeleteShape(cmdInfo.GetExecPram());
                        //    break;
                        case CommonExcelCmdInfo.CExecCmdPrintArea:
                            exlCmdMain.PrintArea(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdPageSetup:
                            exlCmdMain.PageSetup(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdFormatLocal:
                            exlCmdMain.FormatLocal(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdFontChange:
                            exlCmdMain.FontChange(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdFontColorPart:
                            exlCmdMain.FontColorPart(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdAlignment:
                            exlCmdMain.Alignment(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdPageBreaks:
                            exlCmdMain.PageBreaks(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdLineBox:
                            exlCmdMain.LineBox(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdCopyRows:
                            exlCmdMain.CopyRows(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdCopyInsRow:
                            exlCmdMain.CopyInsRow(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdCopyInsCol:
                            exlCmdMain.CopyInsCol(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdCopyInsRange:
                            exlCmdMain.CopyInsRange(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdSetFunctionToCell:
                            exlCmdMain.SetFunctionToCell(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdInsertColumn:
                            exlCmdMain.InsertColumn(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdChangeSheetName:
                            exlCmdMain.ChangeSheetName(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdContionFormat:
                            exlCmdMain.ConditionalFormat(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdBackgroundColor:
                            exlCmdMain.BackgroundColor(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdFormulaA1:
                            exlCmdMain.FormulaA1(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdLockSheet:
                            exlCmdMain.LockSheet(cmdInfo.GetExecPram());
                            break;
                        case CommonExcelCmdInfo.CExecCmdLockCell:
                            exlCmdMain.LockCell(cmdInfo.GetExecPram());
                            break;
                        default:
                            // エラーメッセージ
                            this.Message = string.Format("存在しないコマンドです。[{0}]", cmdInfo.GetExecCmd());
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Message = ex.ToString();
                return false;
            }
            return true;
        }

        ///// <summary>
        ///// テンプレートファイルからの出力ファイル作成
        ///// </summary>
        ///// <param name="templateFile"></param>
        ///// <param name="saveFile"></param>
        ///// <returns></returns>
        //private bool canMakeOutputFile(string templateFile, string saveFile)
        //{
        //    File.Copy(templateFolder + templateFile, saveFolder + saveFile, true);

        //    return true;
        //}

        /// <summary>
        /// テンプレートファイル存在確認
        /// </summary>
        /// <param name="templateFile">テンプレートファイル</param>
        /// <returns>true:正常　false:エラー</returns>
        private bool isExistTemplateFile(string templateFile)
        {
            if (!File.Exists(Path.Combine(templateFolder, templateFile)))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// SetPrtInfo
        /// </summary>
        /// <param name="lstPrtInfo">lstPrtInfo</param>
        public void SetPrtInfo(List<CommonExcelPrtInfo> lstPrtInfo)
        {
            this.lstPrtInfo = lstPrtInfo;
        }

        /// <summary>
        /// SetCmdInfo
        /// </summary>
        /// <param name="lstCmdInfo">lstCmdInfo</param>
        public void SetCmdInfo(List<CommonExcelCmdInfo> lstCmdInfo)
        {
            this.lstCmdInfo = lstCmdInfo;
        }

        ///// <summary>
        ///// 作成したEXCELファイル情報取得
        ///// </summary>
        ///// <returns></returns>
        //public FileInfo GetSaveFile()
        //{
        //    return new System.IO.FileInfo(saveFolder + saveFile);
        //}

        ///// <summary>
        ///// 作成したEXCELファイルパス取得
        ///// </summary>
        ///// <returns></returns>
        //public string GetSaveFilePath()
        //{
        //    return saveFolder + saveFile;
        //}

    }

}
