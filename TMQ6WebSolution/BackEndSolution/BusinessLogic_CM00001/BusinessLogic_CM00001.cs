using CommonWebTemplate.Models.Common;
using CommonSTDUtil.CommonBusinessLogic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonExcelUtil;
using System.IO;

using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComRes = CommonSTDUtil.CommonResources;

namespace BusinessLogic_CM00001
{
    public class BusinessLogic_CM00001 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数
        /// <summary>
        /// フォーム種類
        /// </summary>
        private static class FormType
        {
            /// <summary>一覧</summary>
            public const byte List = 0;
        }

        /// <summary>
        /// テーブル名称
        /// </summary>
        private static class TableName
        {
            /// <summary>テーブル名：[テーブル名]</summary>
            public const string Table = "[テーブル名]";
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            // 機能内で不要なSQLは定義は不要です

            /// <summary>SQL名：一覧取得</summary>
            public const string GetList = "[テーブル名]_GetList";
            /// <summary>SQL名：出力一覧取得</summary>
            public const string GetListForReport = "[テーブル名]_GetListForReport";
            /// <summary>SQL名：登録</summary>
            public const string Insert = "[テーブル名]_Insert";
            /// <summary>SQL名：更新</summary>
            public const string Update = "[テーブル名]_Update";
            /// <summary>SQL名：削除/summary>
            public const string Delete = "[テーブル名]_Delete";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = "[サブディレクトリ名]\\[テーブル名]";
        }

        /// <summary>
        /// テンプレート名称
        /// </summary>
        private static class TemplateName
        {
            /// <summary>テンプレート名：Excel出力</summary>
            public const string Report = "template_[機能ID].xlsx";

        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            // 機能内で不要な処理対象は定義は不要です

            /// <summary>
            /// 検索条件の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string SearchCondition = "L_S_Condition";
            /// <summary>
            /// 検索結果の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string SearchResult = "L_S_Result";


            /// <summary>帳票(EXCEL)</summary>
            public const string ReportExcel = "Report1";
            /// <summary>帳票(PDF)</summary>
            public const string ReportPdf = "Report2";
            /// <summary>帳票(CSV)</summary>
            public const string ReportCsv = "Report3";

            /// <summary>取込(EXCEL)</summary>
            public const string UploadExcel = "Upload1";
            /// <summary>取込(CSV)</summary>
            public const string UploadCsv = "Upload2";
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_CM00001() : base()
        {
        }
        #endregion

        #region オーバーライドメソッド
        protected override int InitImpl()
        {
            this.JsonResult = string.Empty;
            var resultList = new List<Dictionary<string, object>>();

            if (this.CtrlId.ToUpper().StartsWith("BACK"))
            {
                // 戻る処理
                if (!searchList()) { return -1; }
            }
            return 0;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns></returns>
        protected override int SearchImpl()
        {
            this.JsonResult = string.Empty;
            var resultList = new List<Dictionary<string, object>>();

            switch (this.FormNo)
            {
                case FormType.List:     // 一覧検索
                    if (!searchList()) { return -1; }
                    break;
                default:
                    return -1;
            }

            return 0;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns></returns>
        protected override int ExecuteImpl()
        {
            if (this.CtrlId.ToUpper().StartsWith("REGIST"))
            {
                // 登録処理実行
                return Regist();
            }
            else if (this.CtrlId.ToUpper().StartsWith("DELETE"))
            {
                // 削除処理実行
                return Delete();
            }
            return -1;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns></returns>
        protected override int RegistImpl()
        {
            int result = 0;
            this.JsonResult = string.Empty;
            DateTime now = DateTime.Now;
            string ctrlId = TargetCtrlId.SearchResult;

            // 入力チェック
            if (!checkInput(ref this.resultInfoDictionary, this.LanguageId))
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return -1;
            }

            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(ctrlId);
            var lockKeyMaps = GetLockKeyMaps(ctrlId);

            // 検索結果の画面項目定義の情報
            var resultInfo = getResultMappingInfo(ctrlId);

            // 指定コントロールIDの結果情報のみ抽出
            var resultDic = this.resultInfoDictionary.Where(x => ctrlId.Equals(x["CTRLID"])).ToList();
            foreach (var condition in resultDic)
            {
                dynamic conditionObj = new ExpandoObject();
                if (TMPTBL_CONSTANTS.ROWSTATUS.New.ToString().Equals(condition["ROWSTATUS"].ToString()))
                {
                    // ROWSTATUS=Newの場合、新規登録
                    SetExecuteCondition(condition, ctrlId, conditionObj, now, this.UserId, this.UserId);

                    // サンプルコード 設定する条件が画面の値より計算した値の場合
                    // 例として、KEY_NAMEにTestDateが設定された列の日付の値+3日のデータをSQLのパラメータTestDate2に渡す場合の処理

                    // 画面項目定義拡張テーブルのKEY_NAMEに"TestDate"が設定されている項目のVALnを取得
                    var valTestDate = resultInfo.getValName("TestDate");
                    // 結果情報より、上記で指定した列の値を取得
                    var testDate = condition[valTestDate];
                    // 条件("TestDate2")に、値(TestDateの値+3日)を設定する　このとき、値の方はDBに合わせ適切に設定すること
                    var testDate2 = DateTime.Parse((string)testDate).AddDays(3);
                    setConditionObj(conditionObj, "TestDate2", testDate2);

                    // 新規登録処理実行
                    result = db.RegistByOutsideSql(SqlName.Insert, SqlName.SubDir, conditionObj);
                }
                else
                {
                    // 排他チェック
                    if (!CheckExclusiveStatus(condition, lockValMaps, lockKeyMaps)) { return -1; }

                    // ROWSTATUS=New以外の場合、更新
                    SetExecuteCondition(condition, ctrlId, conditionObj, now, this.UserId);

                    // 更新処理実行
                    result = db.RegistByOutsideSql(SqlName.Update, SqlName.SubDir, conditionObj);
                }

                if (result < 0)
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「登録処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                    break;
                }
            }

            if (result >= 0)
            {
                // 再検索処理
                this.NeedsTotalCntCheck = false;
                if (!searchList()) { return -1; }
            }
            return result;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns></returns>
        protected override int DeleteImpl()
        {
            int result = 0;
            this.JsonResult = string.Empty;

            foreach (var condition in this.resultInfoDictionary)
            {
                // 画面の結果情報で繰り返し
                if (!TargetCtrlId.SearchResult.Equals(condition["CTRLID"].ToString()))
                {
                    // 検索結果以外ならスキップ
                    continue;
                }

                // TODO:削除対象の存在チェック等を行う

                dynamic conditionObj = new ExpandoObject();

                // 削除条件設定
                var targetParamNames = new List<string> { "TestCd" }; // この項目の値のみを削除条件に設定する
                SetDeleteCondition(condition, TargetCtrlId.SearchResult, conditionObj, targetParamNames);

                // 削除処理実行
                result = db.RegistByOutsideSql(SqlName.Delete, SqlName.SubDir, conditionObj);

                if (result < 0)
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「削除処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911110001 });
                    break;
                }
            }

            if (result >= 0)
            {
                // 再検索処理
                this.NeedsTotalCntCheck = false;
                if (!searchList()) { return -1; }
            }

            return result;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns></returns>
        protected override int ReportImpl()
        {
            int result = 0;
            this.JsonResult = string.Empty;

            // 実装の際は、不要な帳票に対する分岐は削除して構いません

            bool outputExcel = false;
            bool outputPdf = false;

            switch (this.CtrlId)
            {
                case TargetCtrlId.ReportExcel:
                    outputExcel = true;
                    break;
                case TargetCtrlId.ReportPdf:
                    outputExcel = true;
                    outputPdf = true;
                    break;
                case TargetCtrlId.ReportCsv:
                    break;
                default:
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「コントロールIDが不正です。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID911100001 });

                    // エラーログ出力
                    writeErrorLog(this.MsgId);
                    return -1;
            }

            // ファイル名
            string baseFileName = string.Format("{0:yyyyMMddHHmmss}_{1}_{2}", DateTime.Now, this.ConductId, this.CtrlId);

            // データ検索
            var resultList = searchListForReport();
            if (resultList == null || resultList.Count == 0)
            {
                // 警告メッセージで終了
                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「該当データがありません。」
                this.MsgId = GetResMessage(ComRes.ID.ID941060001);
                return result;
            }

            string msg = string.Empty;
            if (outputExcel)
            {
                // Excel出力が必要な場合

                // マッピング情報生成
                // 以下はA列から順番にカラム名リストに一致するデータを行単位でマッピングする
                List<CommonExcelPrtInfo> prtInfoList = CommonExcelUtil.CommonExcelUtil.CreateMappingList(resultList, "Sheet1", 2, "A");

                // コマンド情報生成
                // セルの結合や罫線を引く等のコマンド実行が必要な場合はここでセットする。不要な場合はnullでOK
                List<CommonExcelCmdInfo> cmdInfoList = null;

                // Excel出力実行
                var excelStream = new MemoryStream();
                if (!CommonExcelUtil.CommonExcelUtil.CreateExcelFile(TemplateName.Report, this.UserId, prtInfoList, cmdInfoList, ref excelStream, ref msg))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「Excel出力に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911040001 });

                    // エラーログ出力
                    writeErrorLog(this.MsgId);
                    writeErrorLog(msg);

                    return -1;
                }

                if (outputPdf)
                {
                    // PDF出力の場合

                    // PDF出力実行
                    var pdfStream = new MemoryStream();
                    try
                    {
                        if (!CommonExcelUtil.CommonExcelUtil.CreatePdfFile(excelStream, ref pdfStream, ref msg))
                        {
                            pdfStream.Close();

                            this.Status = CommonProcReturn.ProcStatus.Error;
                            // 「PDF出力に失敗しました。」
                            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911270004 });

                            // エラーログ出力
                            writeErrorLog(this.MsgId);
                            writeErrorLog(msg);

                            return -1;
                        }
                    }
                    finally
                    {
                        // ExcelファイルのStreamは閉じる
                        excelStream.Close();
                    }
                    this.OutputFileType = "3";  // PDF
                    this.OutputFileName = baseFileName + ".pdf";
                    this.OutputStream = pdfStream;
                }
                else
                {
                    // Excel出力の場合
                    this.OutputFileType = "1";  // Excel
                    this.OutputFileName = baseFileName + ".xlsx";
                    this.OutputStream = excelStream;
                }
            }
            else
            {
                // CSV出力の場合

                // CSV出力実行
                Stream csvStream = new MemoryStream();
                if (!CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.ExportCsvFile(
                    resultList, Encoding.GetEncoding("Shift-JIS"), out csvStream, out msg))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「CSV出力に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120007 });

                    // エラーログ出力
                    writeErrorLog(this.MsgId);
                    writeErrorLog(msg);

                    return -1;
                }
                this.OutputFileType = "2";  // CSV
                this.OutputFileName = baseFileName + ".csv";
                this.OutputStream = csvStream;
            }

            // ↓↓↓ 暫定処理 共通FW側が対応したら削除すること！ ↓↓↓
            using (FileStream fs = new FileStream(this.OutputFileName, FileMode.Create))
            {
                this.OutputStream.CopyTo(fs);
                fs.Flush();
            }
            // ↑↑↑ 暫定処理 共通FW側が対応したら削除すること！ ↑↑↑

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;

            return result;

        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns></returns>
        protected override int UploadImpl()
        {
            // 実装の際は、不要な帳票に対する分岐は削除して構いません

            int result = 0;
            string msg = string.Empty;
            this.JsonResult = string.Empty;

            List<string[,]> uploadList = new List<string[,]>();

            List<Stream> excelList = new List<Stream>();
            List<Stream> csvList = new List<Stream>();
            foreach (var file in this.InputStream)
            {
                switch (Path.GetExtension(file.FileName))
                {
                    case ComUtil.FileExtension.Excel:   // Excelファイル
                        excelList.Add(file.OpenReadStream());
                        break;
                    case ComUtil.FileExtension.CSV:    // CSVファイル
                        csvList.Add(file.OpenReadStream());
                        break;
                    default:
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        // 「ファイルの種類が不正です。」
                        this.MsgId = GetResMessage(ComRes.ID.ID941280004);

                        // エラーログ出力
                        writeErrorLog(this.MsgId);
                        return -1;
                }
            }

            if (excelList.Count > 0)
            {
                // Excelファイル読込
                if (!CommonExcelUtil.CommonExcelUtil.ReadExcelFiles(excelList, "", "", ref uploadList, ref msg))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「Excel取込に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911040002 });

                    // エラーログ出力
                    writeErrorLog(this.MsgId);
                    writeErrorLog(msg);

                    return -1;
                }
            }

            if (csvList.Count > 0)
            {
                // CSVファイル読込
                if (!ComUtil.ImportCsvFiles(
                    csvList, true, Encoding.GetEncoding("Shift-JIS"), ref uploadList, ref msg))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「CSV取込に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120008 });

                    // エラーログ出力
                    writeErrorLog(this.MsgId);
                    writeErrorLog(msg);

                    return -1;
                }
            }

            // ↓↓↓ コントロールIDで取込対象を切り分ける場合 ↓↓↓
            //switch (this.CtrlId)
            //{
            //    case TargetCtrlId.UploadExcel:
            //        // Excelファイル読込
            //        if (!CommonExcelUtil.CommonExcelUtil.ReadExcelFiles(excelList, "", "", ref uploadList, ref msg))
            //        {
            //            this.Status = CommonProcReturn.ProcStatus.Error;
            //            // 「Excel取込に失敗しました。」
            //            this.MsgId = GetResMessage(new string[] { "941220002", "911040002" });

            //            // エラーログ出力
            //            logger.Error(this.MsgId);
            //            logger.Error(msg);

            //            return -1;
            //        }
            //        break;
            //    case TargetCtrlId.UploadCsv:
            //        // CSVファイル読込
            //        if (!ComUtil.ImportCsvFiles(
            //            csvList, true, Encoding.GetEncoding("Shift-JIS"), ref uploadList, ref msg))
            //        {
            //            this.Status = CommonProcReturn.ProcStatus.Error;
            //            // 「CSV取込に失敗しました。」
            //            this.MsgId = GetResMessage(new string[] { "941220002", "911120008" });

            //            // エラーログ出力
            //            logger.Error(this.MsgId);
            //            logger.Error(msg);

            //            return -1;
            //        }
            //        break;
            //    default:
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「コントロールIDが不正です。」
            //        this.MsgId = GetResMessage(new string[] { "941060003", "911100001" });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        return -1;
            //}
            // ↑↑↑ コントロールIDで取込対象を切り分ける場合 ↑↑↑

            // ↓↓↓ 表示用データを返却する場合 ↓↓↓
            // 表示用データを返却する場合、コントロールID指定で変換する
            var resultList = ConvertToUploadResultDictionary("[コントロールID]", uploadList);
            // 取込結果の設定
            SetJsonResult(resultList);
            // ↑↑↑ 表示用データを返却する場合 ↑↑↑

            // ↓↓↓ 登録処理を実行する場合 ↓↓↓
            //// 登録処理を実行する場合、コントロール未指定で変換する
            //this.resultInfoDictionary = ConvertToUploadResultDictionary("", uploadList);
            //// トランザクション開始
            //using (var transaction = this.db.Connection.BeginTransaction())
            //{
            //    try
            //    {
            //        // 登録処理実行
            //        result = RegistImpl();

            //        if (result > 0)
            //        {
            //            // コミット
            //            transaction.Commit();
            //        }
            //        else
            //        {
            //            // ロールバック
            //            transaction.Rollback();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        if (transaction != null)
            //        {
            //            // ロールバック
            //            transaction.Rollback();
            //        }
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「取込処理に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { "941220002", "911200004" });
            //        this.LogNo = string.Empty;

            //        logger.Error(this.MsgId, ex);
            //        return -1;
            //    }
            //}
            // ↑↑↑ 登録処理を実行する場合 ↑↑↑

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;

            return result;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="pageInfoList">ページ情報リスト</param>
        /// <returns></returns>
        private bool searchList()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.SearchResult, this.pageInfoList);

            // 検索条件設定
            dynamic conditionObj = new ExpandoObject();
            // 画面の検索条件と、画面項目定義拡張テーブルの検索条件の項目の値より、検索条件を設定
            SetSearchCondition(this.searchConditionDictionary, TargetCtrlId.SearchCondition, conditionObj, pageInfo);

            // 総件数を取得
            conditionObj.IsCount = true;
            // 総件数を取得
            int cnt = db.GetCountByOutsideSql(SqlName.GetList, SqlName.SubDir, conditionObj);
            if (cnt < 0) { return false; }

            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            // 検索実行
            conditionObj.IsCount = false;
            var results = db.GetListByOutsideSql(SqlName.GetList, SqlName.SubDir, conditionObj);
            if (results == null || results.Count == 0) { return false; }

            // 検索結果の設定
            if (SetSearchResultsByDataClass(pageInfo, results, cnt))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 出力用一覧検索処理
        /// </summary>
        /// <returns></returns>
        private List<object[]> searchListForReport()
        {
            var resultList = new List<object[]>();

            dynamic conditionObj = new ExpandoObject();
            SetSearchCondition(this.searchConditionDictionary, TargetCtrlId.SearchCondition, conditionObj, null);

            // 検索実行
            var results = db.GetListByOutsideSql(SqlName.GetListForReport, SqlName.SubDir, conditionObj);
            if (results != null)
            {
                // 帳票出力列は画面項目定義に定義されていないので、以下のように直接指定すること

                // 出力対象のカラム名のリストを生成(検索SQL側カラム名, ヘッダー文字列)
                // ※カラム名とヘッダー文字列が同一の場合はヘッダー文字列指定不要
                // ※ヘッダー文字列に空文字を設定した場合は空文字が設定される
                var colNameList = new List<string[]>()
                {
                    { new string[] { "", "No." } }, // 通し番号列用
                    { new string[] { "[検索結果カラム名1]", GetResMessage("ISxxxx1") } },
                    { new string[] { "[検索結果カラム名2]", "" } },
                    { new string[] { "[検索結果カラム名3]", GetResMessage("ISxxxx3") } },

                    { new string[] { "[検索結果カラム名n]", GetResMessage("ISxxxxn") } }
                };
                resultList = ConvertToReportMappingList(results, colNameList, true, true);
            }
            return resultList;
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="languageId">言語コード</param>
        /// <returns>エラーのある場合、false</returns>
        private bool checkInput(ref List<Dictionary<string, object>> dictionary, string languageId)
        {
            // 結果返却用 エラーのある場合false、無い場合true
            var returnVal = true;
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();
            // エラーメッセージ一時セット用変数
            string errorMessage = null;

            // 明細の各列に対して、入力チェックを行う場合の実装例

            // 明細の項目の情報
            var info = getResultMappingInfo(TargetCtrlId.SearchResult);
            // 明細の各列を取得
            var checkTargetList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.SearchResult);
            // 明細の件数分繰り返し、各列に対して入力チェックを行う
            foreach (var condition in checkTargetList)
            {
                // 入力チェックの対象である、明細列を引数に、エラー情報設定用クラスを宣言
                var errorInfo = new ErrorInfo(condition);
                //もし、エラーチェックの対象が明細でなく、単一の項目である場合は、conditionでなく以下を引数に渡す
                // ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, "コントロールID");

                // 行単位でエラーのある場合、True
                var isError = false;

                //    // 例：コードの入力チェック
                //    // KEY_NAMEにCodeが指定されている列のVALを取得
                //    var val = info.getValName("Code");
                //    // VALより画面の値を取得
                //    var code = condition[val].ToString();

                //    // マスタ存在チェック
                //    var tempStatus = MasterUtil.CheckMaster(Constants.MASTER.CODE, code, ref errorMessage, db);
                //    if (!tempStatus)
                //    {
                //        // 上記入力チェックがエラーの場合、エラー情報をセット
                //        errorInfo.setError(errorMessage, val);
                //        isError = true;
                //    }

                if (isError)
                {
                    // 行でエラーのあった場合、エラー情報を設定する
                    errorInfoDictionary.Add(errorInfo.Result);
                    returnVal = false;
                }
            }

            // エラー情報を画面に反映
            SetJsonResult(errorInfoDictionary);

            return returnVal;
        }
        #endregion
    }
}