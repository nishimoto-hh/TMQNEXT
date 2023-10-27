using CommonExcelUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_RM0001.BusinessLogicDataClass_RM0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComTMQDao = CommonTMQUtil.TMQCommonDataClass;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using ComDao = CommonTMQUtil.CommonTMQUtilDataClass;
using System;
using System.Collections.Generic;
using System.IO;

namespace BusinessLogic_RM0001
{
    /// <summary>
    /// 雛形ファイル登録画面
    /// </summary>
    public partial class BusinessLogic_RM0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// アップロード・登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool registTemplateFile()
        {
            // 出力テンプレートの登録フラグ
            bool isInsertTemplate = false;

            // 画面より工場ID、帳票ID、テンプレートIDを取得する。
            // 検索条件を出力処理用の変数に設定
            setReportImplDataByCondition(out int factoryId, out string reportId, out int templateId, out int outputPatternId,
                                            out string programId, out string templateFilePath, out string templateFileName);

            // TODO
            // アップロード時の工場について選択できるようにする
            factoryId = int.Parse(this.FactoryId);

            // テンプレートが未選択の場合
            if (templateId <= 0)
            {
                isInsertTemplate = true;
                // 新規登録の為、テンプレートIDを採番
                templateId = GetMaxTemplateId(factoryId, reportId) + 1;
            }

            // ファイルをアップロードするパスを作成
            if (!createUploadPath(factoryId, reportId, templateId, out string uploadPath))
            {
                return false;
            }

            // ファイルアップロード処理
            uploadFile(uploadPath, out string filePath, out string fileName);

            // 帳票定義の複写登録（テンプレート新規登録時、且つ定義情報未登録時のみ）
            if(isInsertTemplate == true) {
                if(!registReportDefineInfo(factoryId, programId, reportId))
                {
                    return false;
                }
            }

            // テンプレート情報登録処理
            if (!registTemplateInfno(factoryId, programId, reportId, templateId, fileName, isInsertTemplate))
            {
                return false;
            }

            // テンプレートExcelファイルより出力項目登録処理
            if (!registTemplateItemByExcel(factoryId, programId, reportId, templateId, fileName, isInsertTemplate))
            {
                return false;
            }

            // グローバル変数に登録テンプレートIDをセットする
            SetGlobalData(GlobalListKeyRM0001.RegistFactoryId, factoryId);
            SetGlobalData(GlobalListKeyRM0001.RegistTemplateId, templateId);
            SetGlobalData(GlobalListKeyRM0001.RegistPatternId, OutputPatternDefaultValue.OutputPatternId);

            return true;
        }

        #region 登録処理関係
        /// <summary>
        /// 定義情報複写登録処理
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="programId">プログラムID</param>
        /// <param name="reportId">帳票ID</param>
        /// <returns>エラーの場合False</returns>
        private bool registReportDefineInfo(int factoryId, string programId, string reportId)
        {
            // 登録情報設定
            ReportDao.MsOutputReportDefineEntity registInfo = new ReportDao.MsOutputReportDefineEntity();
            var result = registInfo.GetEntity(factoryId, programId, reportId, this.db);
            if (result != null)
            {
                // データが存在するため、後続処理にすすむ
                return true;
            }

            // 出力帳票定義、出力帳票シート定義、出力帳票項目定義の複写登録
            if (!registDb(registInfo, SqlName.SelectInsertOutputDefineInfo))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            return true;
        }

        /// <summary>
        /// テンプレート情報登録処理
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="programId">プログラムID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="isInsertTemplate">テンプレート新規登録フラグ</param>
        /// <returns>エラーの場合False</returns>
        private bool registTemplateInfno(int factoryId, string programId, string reportId, int templateId, string fileName, bool isInsertTemplate)
        {
            // 登録帳票定義を取得
            ReportDao.MsOutputReportDefineEntity defineInfo = new ReportDao.MsOutputReportDefineEntity();
            var result = defineInfo.GetEntity(factoryId, programId, reportId, this.db);
            if (result == null)
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 登録情報設定
            ComTMQDao.MsOutputTemplateEntity registInfo = new ComTMQDao.MsOutputTemplateEntity();
            // 画面情報から取得（雛形ファイル）
            DateTime now = DateTime.Now;
            registInfo = getRegistInfo<ComTMQDao.MsOutputTemplateEntity>(ConductInfo.FormUploadTemplate.ControlId.FileName, now);

            registInfo.FactoryId = factoryId;               // 工場ID
            registInfo.ReportId = reportId;                 // 帳票ID
            registInfo.TemplateId = templateId;             // テンプレートID
            registInfo.TemplateFileName = fileName;         // テンプレートファイル名
            if (result.ManagementType == 2)
            {
                // 登録帳票の管理種別が 2：ユーザ単位の場合のみ、使用ユーザIDを登録する
                registInfo.UseUserId = int.Parse(this.UserId);  // 使用ユーザID
            }

            // テンプレートファイルパス
            // ディレクトリ構成を作成
            // 構成：工場ID\帳票ID\テンプレートID
            registInfo.TemplateFilePath = factoryId + "\\" + reportId + "\\" + templateId;

            // 入力チェック
            if (isErrorRegist(registInfo, fileName, isInsertTemplate))
            {
                return false;
            }

            // 出力テンプレート登録・更新、出力パターン登録
            // 新規登録の場合
            if (isInsertTemplate == true)
            {
                // 出力テンプレート登録
                if (!registDb(registInfo, SqlName.InsertOutputTemplate))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }

                // 出力パターン登録準備
                ReportDao.MsOutputPatternEntity registOutputPattern = new ReportDao.MsOutputPatternEntity();
                setExecuteConditionByDataClassCommon(ref registOutputPattern, DateTime.Now, int.Parse(this.UserId), int.Parse(this.UserId));

                registOutputPattern.FactoryId = factoryId;                                                          // 工場ID
                registOutputPattern.ReportId = reportId;                                                            // 帳票ID
                registOutputPattern.TemplateId = templateId;                                                        // テンプレートID
                registOutputPattern.OutputPatternId = OutputPatternDefaultValue.OutputPatternId;                    // 出力パターンID（「1」固定）
                registOutputPattern.OutputPatternName = OutputPatternDefaultValue.OutputPatternName;                // 出力パターン名（「パターン1」固定）

                // 登録帳票の管理種別が 2：ユーザ単位の場合のみ、使用ユーザIDを登録する
                if (result.ManagementType == 2)
                {
                    registOutputPattern.UseUserId = int.Parse(this.UserId);                                             // 使用ユーザID
                }

                // 出力パターン登録
                if (!registDb(registOutputPattern, SqlName.InsertOutputPattern))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }
            // 更新の場合
            else
            {
                // 出力テンプレート
                // 更新SQL実行
                if (!registDb(registInfo, SqlName.UpdateOutputTemplate))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Excelファイルより出力項目登録
        /// <param name="factoryId">工場ID</param>
        /// <param name="programId">プログラムID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="isInsertTemplate">テンプレート新規登録フラグ</param>
        /// <returns>エラーの場合False</returns>
        /// </summary>
        private bool registTemplateItemByExcel(int factoryId, string programId, string reportId, int templateId, string fileName, bool isInsertTemplate)
        {
            ReportDao.MsOutputItemEntity registOutputItem = new ReportDao.MsOutputItemEntity();
            // 更新の場合
            if (isInsertTemplate == false)
            {
                // 既存の出力項目を削除する
                registOutputItem = new ReportDao.MsOutputItemEntity();
                setExecuteConditionByDataClassCommon(ref registOutputItem, DateTime.Now, int.Parse(this.UserId), int.Parse(this.UserId));
                registOutputItem.FactoryId = factoryId;                                                          // 工場ID
                registOutputItem.ReportId = reportId;                                                            // 帳票ID
                registOutputItem.TemplateId = templateId;                                                        // テンプレートID
                registOutputItem.OutputPatternId = OutputPatternDefaultValue.OutputPatternId;                    // 出力パターンID（「1」固定）

                // 登録済の出力項目の削除
                if (!registDeleteDb<ReportDao.MsOutputItemEntity>(registOutputItem, SqlName.DeleteOutputItem))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }

            // テンプレートファイル名を設定
            string templateFileName = fileName;
            // テンプレートファイルパスを設定
            string templateFilePath = factoryId + "\\" + reportId + "\\" + templateId;

            // マッピング情報生成
            List<CommonExcelPrtInfo> mappingInfoList = new List<CommonExcelPrtInfo>();
            // 帳票定義を取得
            var reportDefine = new ReportDao.MsOutputReportDefineEntity().GetEntity(factoryId, programId, reportId, db);
            // 出力帳票シート定義のリストを取得
            var sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                TMQUtil.ComReport.GetReportSheetDefine,
                TMQUtil.ExcelPath,
                new { FactoryId = factoryId, ReportId = reportId },
                db);

            if (sheetDefineList == null)
            {
                // 取得できない場合、処理を戻す
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // シート定義毎にループ
            foreach (var sheetDefine in sheetDefineList)
            {
                // 検索条件フラグ設定済のシートの場合、スキップ
                if (sheetDefine.SearchConditionFlg == true)
                {
                    continue;
                }

                // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetOutputReportDefineForUpload, out string baseSql);

                // 項目定義を取得
                IList<ComDao.InoutDefine> reportInfoList = db.GetListByDataClass<ComDao.InoutDefine>(
                    baseSql,
                    new { FactoryId = factoryId, ReportId = reportId, SheetNo = sheetDefine.SheetNo, LanguageId = this.LanguageId });
                if (reportInfoList == null || reportInfoList.Count == 0)
                {
                    // 取得できない場合、処理を戻す
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }

                // 対象テンプレートファイルシートのデータを読込み、開始行番号と開始列番号を設定
                TMQUtil.SetStartInfoByExcel(ref reportInfoList, templateFileName, templateFilePath, sheetDefine.SheetNo, sheetDefine.SheetDefineMaxRow,
                                                sheetDefine.SheetDefineMaxColumn, reportDefine.OutputItemType);

                int displayOrder = 1;  // 表示順（1～連番）
                foreach (var reportInfo in reportInfoList)
                {
                    // 開始行番号、開始列番号が設定されている項目に対して出力項目を登録する
                    if (reportInfo.StartRowNo != null && reportInfo.StartColNo != null)
                    {
                        // 出力項目登録
                        registOutputItem = new ReportDao.MsOutputItemEntity();
                        setExecuteConditionByDataClassCommon(ref registOutputItem, DateTime.Now, int.Parse(this.UserId), int.Parse(this.UserId));

                        registOutputItem.FactoryId = factoryId;                                                          // 工場ID
                        registOutputItem.ReportId = reportId;                                                            // 帳票ID
                        registOutputItem.TemplateId = templateId;                                                        // テンプレートID
                        registOutputItem.OutputPatternId = OutputPatternDefaultValue.OutputPatternId;                    // 出力パターンID（「1」固定）
                        registOutputItem.SheetNo = sheetDefine.SheetNo;                                                  // シート番号
                        registOutputItem.ItemId = reportInfo.ItemId;                                                     // 項目ID
                        registOutputItem.DisplayOrder = displayOrder;                                                    // 表示順
                        registOutputItem.DefaultCellRowNo = reportInfo.StartRowNo;                                       // デフォルトセル行No
                        registOutputItem.DefaultCellColumnNo = reportInfo.StartColNo;                                    // デフォルトセル列No

                        if (!registDb(registOutputItem, SqlName.InsertOutputItem))
                        {
                            // エラーの場合
                            this.Status = CommonProcReturn.ProcStatus.Error;
                            return false;
                        }
                        displayOrder += 1;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="fileName">アップロードファイル名</param>
        /// <param name="isInsertTemplate">テンプレート新規登録フラグ</param>
        /// <returns>エラーの場合true</returns>
        private bool isErrorRegist(ComTMQDao.MsOutputTemplateEntity registInfo, string fileName, bool isInsertTemplate)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 単一の項目の場合のイメージ
            if (checkErrorRegist(ref errorInfoDictionary, registInfo, fileName, isInsertTemplate))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            bool checkErrorRegist(ref List<Dictionary<string, object>> errorInfoDictionary, ComTMQDao.MsOutputTemplateEntity registInfo, string fileName, bool isInsertTemplate)
            {
                bool isError = false;   // 処理全体でエラーの有無を保持

                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(ConductInfo.FormUploadTemplate.ControlId.FileName);
                // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                // 単一の内容を取得
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormUploadTemplate.ControlId.FileName);
                // エラー情報格納クラス
                var errorInfo = new ErrorInfo(targetDic);
                string errMsg = string.Empty;
                string val = info.getValName("File"); // エラーをセットするファイル選択コントロールの項目番号

                // ファイル名が指定された文字数より多い場合はエラー
                if (fileName.Length > LengthOfFile.FileName)
                {
                    // 「ファイル名が設定桁数を超えています。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941060014, ComRes.ID.ID111280020 });
                    errorInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    isError = true;
                }

                // アップロード先のパス(絶対パス)が指定された文字数より多い場合はエラー
                if (Path.GetFullPath(registInfo.TemplateFileName).Length > LengthOfFile.Path)
                {
                    // 「アップロード先のパスが設定桁数を超えています。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941060014, ComRes.ID.ID941010002 });
                    errorInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    isError = true;
                }

                // 同一テンプレート名重複チェック
                if (!checkContentDuplicate(registInfo, isInsertTemplate))
                {
                    val = info.getValName("TemplateName"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    // エラーの場合
                    // 「指定されたテンプレート名がすでに登録されています。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID141120003, ComRes.ID.ID111190018 });
                    errorInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    isError = true;
                }

                return isError;
            }

            /// <summary>
            /// 同一テンプレート名重複チェック
            /// </summary>
            /// <param name="outputTemplate">登録情報</param>
            /// <param name="isInsertTemplate">テンプレート新規登録フラグ</param>
            /// <returns>エラーの場合false</returns>
            bool checkContentDuplicate(ComTMQDao.MsOutputTemplateEntity outputTemplate, bool isInsertTemplate)
            {
                // 検索SQL文の取得
                dynamic whereParam = null; // WHERE句パラメータ
                string sql = string.Empty;

                List<string> listUnComment = new List<string>();
                // 更新時のみ
                if (isInsertTemplate == false)
                {
                    listUnComment.Add("TemplateId");
                }

                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetOutputTemplateCountCheck, out sql, listUnComment))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
                whereParam = new { FactoryId = outputTemplate.FactoryId, ReportId = outputTemplate.ReportId, TemplateId = outputTemplate.TemplateId, TemplateName = outputTemplate.TemplateName };
                // 総件数を取得
                int cnt = db.GetCount(sql, whereParam);
                if (cnt > 0)
                {
                    return false;
                }

                return true;
            }
        }
        #endregion

        #region アップロード関係
        /// <summary>
        /// ファイルアップロード処理
        /// </summary>
        /// <param name="uploadPath">アップロードパス</param>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="fileName">ファイル名</param>
        private void uploadFile(string uploadPath, out string filePath, out string fileName)
        {
            filePath = string.Empty;
            fileName = string.Empty;

            // アップロード処理
            using (Stream inStream = this.InputStream[0].OpenReadStream())
            using (MemoryStream storeStream = new MemoryStream())
            {
                // 選択されたファイルをメモリに格納
                storeStream.SetLength(inStream.Length);
                inStream.Read(storeStream.GetBuffer(), 0, (int)inStream.Length);
                storeStream.Flush();
                inStream.Close();

                // アップロードパス + ファイル名
                fileName = getFileName(uploadPath);
                filePath = uploadPath + "\\" + fileName;
                // メモリの情報を出力ファイルに反映
                saveMemoryStream(storeStream, filePath);

                storeStream.Close();
            }

            void saveMemoryStream(MemoryStream ms, string fileName)
            {
                // 出力ファイルにストリームの情報を書き込み
                using (FileStream outStream = File.OpenWrite(fileName))
                {
                    ms.WriteTo(outStream);
                    outStream.Flush();
                    outStream.Close();
                }
            }
        }

        /// <summary>
        /// アップロードパス作成
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="uploadPath">アップロードパス</param>
        /// <returns>エラーの場合False</returns>
        private bool createUploadPath(int factoryId, string reportId, int templateId, out string uploadPath)
        {
            // アップロードパス作成
            uploadPath = string.Empty;

            // ファイルアップロード先フォルダ取得
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string uploadFolder = Path.Combine(appPath, CommonWebTemplate.AppCommonObject.Config.AppSettings.ExcelTemplateDir);
            if (string.IsNullOrEmpty(uploadFolder))
            {
                // 取得できない場合はエラー
                // 「アップロード先の取得に失敗しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID941010001 });
                return false;
            }

            // 取得したフォルダをアップロードパスに設定
            uploadPath += uploadFolder;

            // ディレクトリ構成を作成
            // 構成：工場ID\帳票ID\テンプレートID
            uploadPath += "\\" + factoryId;
            uploadPath += "\\" + reportId;
            uploadPath += "\\" + templateId;

            return true;
        }

        /// <summary>
        /// アップロードファイル名の取得
        /// </summary>
        /// <param name="fileDirectory">ディレクトリ</param>
        /// <returns>アップロードファイル名</returns>
        private string getFileName(string fileDirectory)
        {
            // アップロード先が存在しない場合
            if (!Directory.Exists(fileDirectory))
            {
                // アップロード先作成
                Directory.CreateDirectory(fileDirectory);
            }

            string baseName = Path.GetFileNameWithoutExtension(this.InputStream[0].FileName); // ファイル名
            string tmpName = Path.GetFileNameWithoutExtension(this.InputStream[0].FileName);  //一時ファイル名
            string extension = Path.GetExtension(this.InputStream[0].FileName);               // 拡張子

            // 既に存在する場合は上書きする。
            // 変更後のファイル名 + 拡張子
            return tmpName + extension;
        }

        /// <summary>
        /// 現在のms_output_templateのテンプレートIDMAX値（工場ID、帳票ID）を取得
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="reportId">帳票ID</param>
        private int GetMaxTemplateId(int factoryId, string reportId)
        {
            // 最大更新日時取得SQL
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetMaxTemplateId, out string outSql);
            ReportDao.MsOutputPatternEntity getMaxOutputPatternIdParam = new ReportDao.MsOutputPatternEntity();

            getMaxOutputPatternIdParam.FactoryId = factoryId;
            getMaxOutputPatternIdParam.ReportId = reportId;
            // SQL実行
            var maxTemplateId = db.GetEntity(outSql, getMaxOutputPatternIdParam);

            return maxTemplateId.max_template_id;
        }
        #endregion
    }
}
