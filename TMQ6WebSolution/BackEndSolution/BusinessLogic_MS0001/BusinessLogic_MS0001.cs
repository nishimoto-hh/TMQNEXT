using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MS0001.BusinessLogicDataClass_MS0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_MS0001
{
    /// <summary>
    /// マスタメンテナンスメニュー
    /// </summary>
    public class BusinessLogic_MS0001 : CommonBusinessLogicBase
    {
        #region private変数
        /// <summary>
        /// 出力項目を囲む文字列
        /// </summary>
        private string encircleValue = "\"";
        /// <summary>
        /// 出力ファイルID
        /// </summary>
        private string reportId = "RP0370";
        #endregion

        #region 定数
        /// <summary>
        /// フォーム種類
        /// </summary>
        private static class FormType
        {
            /// <summary>一覧画面</summary>
            public const byte List = 0;
            /// <summary>詳細子画面</summary>
            public const byte Detail = 1;
            /// <summary>編集モーダル画面</summary>
            public const byte Edit = 2;
        }

        /// <summary>
        /// 処理対象グループ番号
        /// </summary>
        private static class TargetGrpNo
        {
            /// <summary>テスト用</summary>
            public const short GroupTest = 4;
            /// <summary>サンプル用</summary>
            public const short GroupSample = 5;
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
            public const string SearchCondition = "SearchCondition";
            /// <summary>
            /// 検索結果の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string SearchResult1 = "SearchResult_1";
            /// <summary>
            /// 検索結果の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string SearchResult2 = "SearchResult_2";

            public static class Button
            {
                /// <summary>帳票(EXCEL)</summary>
                public const string ReportExcel = "ReportExcel";
                /// <summary>帳票(PDF)</summary>
                public const string ReportPdf = "ReportPdf";
                /// <summary>帳票(CSV)</summary>
                public const string ReportCsv = "ReportCsv";

                /// <summary>取込(EXCEL)</summary>
                public const string UploadExcel = "UploadExcel";
                /// <summary>取込(CSV)</summary>
                public const string UploadCsv = "UploadCsv";
            }
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Master";
            /// <summary>
            /// 一覧検索SQL
            /// </summary>
            public const string GetMasterMenuList = "GetMasterMenuList";
            /// <summary>
            /// CSV出力データ検索SQL
            /// </summary>
            public const string GetOutputMasterMenuData = "GetOutputMasterMenuData";
            /// <summary>
            /// CSV出力ヘッダー検索SQL
            /// </summary>
            public const string GetOutputHeaderData = "GetOutputHeaderData";
            /// <summary>
            /// ユーザーの権限レベル取得SQL
            /// </summary>
            public const string GetUserAuthLevel = "GetUserAuthLevel";
            /// <summary>
            /// ユーザーに権限のある工場ID取得SQL
            /// </summary>
            public const string GetUserFactory = "GetUserFactory";
            /// <summary>
            /// 全ての工場ID取得SQL
            /// </summary>
            public const string GetUserFactoryAll = "GetUserFactoryAll";
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
        /// コントロールID
        /// </summary>
        public static class ControlId
        {
            /// <summary>
            /// 検索条件
            /// </summary>
            public const string Condition = "BODY_000_00_BTN_0";
            /// <summary>
            /// 検索結果一覧
            /// </summary>
            public const string List = "BODY_010_00_LST_0";

        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_MS0001() : base()
        {
        }
        #endregion

        #region オーバーライドメソッド
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int InitImpl()
        {
            this.ResultList = new();

            // 初期検索実行
            return InitSearch();
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            // 一覧検索
            if (!searchList())
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ExecuteImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            // マスタアイテム出力
            if (!outPutMasterItem())
            {
                return ComConsts.RETURN_RESULT.NG;
            }
            return ComConsts.RETURN_RESULT.OK;

        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ControlId.List, this.pageInfoList);

            // 検索条件取得
            var condition = new Dao.searchCondition();
            condition.UserId = this.UserId;
            condition.LanguageId = this.LanguageId;

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetMasterMenuList, out string baseSql);
            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetMasterMenuList, out string withSql);

            // SQL、WITH句より件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, string.Empty, withSql);

            // 総件数を取得
            int cnt = db.GetCount(executeSql.ToString(), condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY display_order");

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results, results.Count))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool outPutMasterItem()
        {
            // 検索条件取得
            Dao.searchConditionReport searchCondition = getSearchCondition();

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetOutputMasterMenuData, out string outSql);

            // 検索実行
            IList<Dao.searchResultReport> results = db.GetListByDataClass<Dao.searchResultReport>(outSql.ToString(), searchCondition);
            if (results == null)
            {
                return false;
            }

            // 検索結果を配列に格納
            List<object[]> list = new();

            // ヘッダーを追加
            List<object[]> headerRow = getHeader();
            foreach (object[] row in headerRow)
            {
                list.Add(row);
            }

            foreach (var result in results)
            {
                list.Add(new object[]
                {
                    encircle(result.MasterName),            // マスタ種類
                    encircle(result.FactoryName),           // 工場
                    encircle(result.DefaultItem),           // 標準
                    encircle(result.StructureItemId),       // アイテムID
                    encircle(result.TranslationText),       // アイテム翻訳
                    encircle(result.ParentStructureItemId), // 親階層アイテムID
                    encircle(result.ParentTranslationText), // 親階層アイテム翻訳
                    encircle(result.DisplayOrder),          // 表示順
                    encircle(result.Unused),                // 未使用
                    encircle(result.DeleteFlg),             // 削除
                    encircle(result.ExtensionData1),        // 拡張項目1
                    encircle(result.ExtensionData2),        // 拡張項目2
                    encircle(result.ExtensionData3),        // 拡張項目3
                    encircle(result.ExtensionData4),        // 拡張項目4
                    encircle(result.ExtensionData5),        // 拡張項目5
                    encircle(result.ExtensionData6),        // 拡張項目6
                    encircle(result.ExtensionData7),        // 拡張項目7
                    encircle(result.ExtensionData8),        // 拡張項目8
                    encircle(result.ExtensionData9),        // 拡張項目9
                    encircle(result.ExtensionData10),       // 拡張項目10

                });
            }

            // CSV出力処理
            if (!CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.ExportCsvFileNotencircleDobleQuotes(list, Encoding.GetEncoding("Shift-JIS"), out Stream outStream, out string errMsg))
            {
                // エラーログ出力
                logger.ErrorLog(this.FactoryId, this.UserId, errMsg);
                // 「出力処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911120006" });
                //this.MsgId = errMsg;
                return false;
            }

            // 画面の出力へ設定
            this.OutputFileType = CommonConstants.REPORT.FILETYPE.CSV;
            this.OutputFileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", reportId, DateTime.Now) + ComConsts.REPORT.EXTENSION.CSV;
            this.OutputStream = outStream;

            return true;

            Dao.searchConditionReport getSearchCondition()
            {
                // ユーザーの本務工場取得
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);

                // 検索条件の作成
                Dao.searchConditionReport condition = new();
                condition.FactoryIdList = new();
                condition.StructureGroupId = new();
                condition.UserId = this.UserId;                         // ユーザーID
                condition.FactoryIdList.Add(TMQConsts.CommonFactoryId); // 工場IDリスト(共通工場)
                condition.FactoryIdList.Add(userFactoryId);             // 工場IDリスト(本務工場)
                condition.CommonFactoryId = TMQConsts.CommonFactoryId;  // 共通工場
                condition.UserFactoryId = userFactoryId;                // 本務工場
                condition.LanguageId = this.LanguageId;                 // 言語ID
                condition.AuthFactoryId = getAuthFactoryId();           // 権限のある工場ID

                // 一覧で選択されたレコードの構成グループIDを取得
                var selectedList = getSelectedRowsByList(this.resultInfoDictionary, ControlId.List);
                foreach (var id in selectedList)
                {
                    Dao.searchResult result = new();
                    SetDataClassFromDictionary(id, ControlId.List, result);

                    // MS0010、MS0020は追加しない
                    if (result.ConductId == "MS0010" || result.ConductId == "MS0020")
                    {
                        continue;
                    }

                    condition.StructureGroupId.Add(int.Parse(result.ConductId.Replace("MS", "")));
                }

                return condition;
            }

            List<object[]> getHeader()
            {
                // ヘッダーの翻訳を取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetOutputHeaderData, out string outSql);
                IList<Dao.searchResultHeaderReport> headerList = db.GetListByDataClass<Dao.searchResultHeaderReport>(outSql.ToString(), searchCondition);
                Dictionary<long, string> headerName = new();
                foreach (Dao.searchResultHeaderReport header in headerList)
                {
                    headerName.Add(header.TranslationId, header.HeaderName);
                }

                // 空のレコード
                object[] emptyRow = new object[]
                {
                    string.Empty
                };

                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
                string date = DateTime.Now.ToString("dddd,MMM d yyyy HH:mm:ss", ci);

                // タイトル行
                object[] titleRow = new object[]
                {
                    encircle(headerName[111310007]), // マスタアイテム一覧
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    date
                };

                // ヘッダー行
                object[] headerRow = new object[]
                {
                    encircle(headerName[111310006]), // マスタ種類
                    encircle(headerName[111100012]), // 工場
                    encircle(headerName[111270024]), // 標準
                    encircle(headerName[111010004]), // アイテムID
                    encircle(headerName[111010005]), // アイテム翻訳
                    encircle(headerName[111050007]), // 親階層アイテムID
                    encircle(headerName[111050008]), // 親階層アイテム翻訳
                    encircle(headerName[111270017]), // 表示順
                    encircle(headerName[111320001]), // 未使用
                    encircle(headerName[111110001]), // 削除
                    encircle(headerName[111060041]), // 拡張項目1
                    encircle(headerName[111060042]), // 拡張項目2
                    encircle(headerName[111060043]), // 拡張項目3
                    encircle(headerName[111060079]), // 拡張項目4
                    encircle(headerName[111060080]), // 拡張項目5
                    encircle(headerName[111060081]), // 拡張項目6
                    encircle(headerName[111060082]), // 拡張項目7
                    encircle(headerName[111060083]), // 拡張項目8
                    encircle(headerName[111060084]), // 拡張項目9
                    encircle(headerName[111060085]), // 拡張項目10
                };

                return new List<object[]> { emptyRow, titleRow, emptyRow, headerRow };

            }

            string encircle(string value)
            {
                // 指定された文字列で囲む
                return encircleValue + value + encircleValue;
            }
        }

        /// <summary>
        /// ユーザーに権限のある工場IDを取得
        /// </summary>
        /// <returns>工場IDリスト</returns>
        private List<int> getAuthFactoryId()
        {
            Dao.searchConditionReport condition = new();
            condition.UserId = this.UserId; // ユーザーID

            // ユーザーの権限レベルを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetUserAuthLevel, out string outSql);
            int authLevel = db.GetEntity<int>(outSql, condition);

            List<int> factoryIdList = new();
            if (authLevel == 99)
            {
                // システム管理者の場合、全ての工場IDを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetUserFactoryAll, out string outSqlFacAll);
                factoryIdList = (List<int>)db.GetList<int>(outSqlFacAll, condition);
            }
            else
            {
                // システム管理者以外の場合、ユーザーに権限のある工場IDを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetUserFactory, out string outSqlFac);
                factoryIdList = (List<int>)db.GetList<int>(outSqlFac, condition);
            }

            return factoryIdList;
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// Testボタン押下時の処理
        /// </summary>
        /// <returns>エラーの場合-1、正常の場合1</returns>
        /// <remarks>トランザクション制御のため、publicで戻り値はint、引数はなし</remarks>
        public int executeTest()
        {
            bool result = regist();

            return result ? ComConsts.RETURN_RESULT.OK : ComConsts.RETURN_RESULT.NG;

            bool regist()
            {
                return true;
            }

        }
        #endregion

        #region 見直し予定
        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int UploadImpl()
        {
            // 実装の際は、不要な帳票に対する分岐は削除して構いません

            int result = 0;
            //string msg = string.Empty;
            //this.ResultList = new();

            //List<string[,]> uploadList = new List<string[,]>();

            //List<Stream> excelList = new List<Stream>();
            //List<Stream> csvList = new List<Stream>();
            //foreach (var file in this.InputStream)
            //{
            //    switch (Path.GetExtension(file.FileName))
            //    {
            //        case ComUtil.FileExtension.Excel:   // Excelファイル
            //            excelList.Add(file.OpenReadStream());
            //            break;
            //        case ComUtil.FileExtension.CSV:    // CSVファイル
            //            csvList.Add(file.OpenReadStream());
            //            break;
            //        default:
            //            this.Status = CommonProcReturn.ProcStatus.Error;
            //            // 「ファイルの種類が不正です。」
            //            this.MsgId = GetResMessage(ComRes.ID.ID941280004);

            //            // エラーログ出力
            //            logger.Error(this.MsgId);
            //            return ComConsts.RETURN_RESULT.NG;
            //    }
            //}

            //if (excelList.Count > 0)
            //{
            //    // Excelファイル読込
            //    if (!CommonExcelUtil.CommonExcelUtil.ReadExcelFiles(excelList, "", "", ref uploadList, ref msg))
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「Excel取込に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911040002 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        logger.Error(msg);

            //        return ComConsts.RETURN_RESULT.NG;
            //    }
            //}

            //if (csvList.Count > 0)
            //{
            //    // CSVファイル読込
            //    if (!ComUtil.ImportCsvFiles(
            //        csvList, true, Encoding.GetEncoding(CommonConstants.UPLOAD_INFILE_CHAR_CODE), ref uploadList, ref msg))
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「CSV取込に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120008 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        logger.Error(msg);

            //        return ComConsts.RETURN_RESULT.NG;
            //    }
            //}

            //// ↓↓↓ コントロールIDで取込対象を切り分ける場合 ↓↓↓
            ////switch (this.CtrlId)
            ////{
            ////    case TargetCtrlId.UploadExcel:
            ////        // Excelファイル読込
            ////        if (!CommonExcelUtil.CommonExcelUtil.ReadExcelFiles(excelList, "", "", ref uploadList, ref msg))
            ////        {
            ////            this.Status = CommonProcReturn.ProcStatus.Error;
            ////            // 「Excel取込に失敗しました。」
            ////            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911040002 });

            ////            // エラーログ出力
            ////            logger.Error(this.MsgId);
            ////            logger.Error(msg);

            ////            return -1;
            ////        }
            ////        break;
            ////    case TargetCtrlId.UploadCsv:
            ////        // CSVファイル読込
            ////        if (!ComUtil.ImportCsvFiles(
            ////            csvList, true, Encoding.GetEncoding("Shift-JIS"), ref uploadList, ref msg))
            ////        {
            ////            this.Status = CommonProcReturn.ProcStatus.Error;
            ////            // 「CSV取込に失敗しました。」
            ////            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120008 });

            ////            // エラーログ出力
            ////            logger.Error(this.MsgId);
            ////            logger.Error(msg);

            ////            return -1;
            ////        }
            ////        break;
            ////    default:
            ////        this.Status = CommonProcReturn.ProcStatus.Error;
            ////        // 「コントロールIDが不正です。」
            ////        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID911100001 });

            ////        // エラーログ出力
            ////        logger.Error(this.MsgId);
            ////        return -1;
            ////}
            //// ↑↑↑ コントロールIDで取込対象を切り分ける場合 ↑↑↑

            //// ↓↓↓ 表示用データを返却する場合 ↓↓↓
            //// 表示用データを返却する場合、コントロールID指定で変換する
            //var resultList = ConvertToUploadResultDictionary("[コントロールID]", uploadList);
            //// 取込結果の設定
            //SetJsonResult(resultList);
            //// ↑↑↑ 表示用データを返却する場合 ↑↑↑

            //// ↓↓↓ 登録処理を実行する場合 ↓↓↓
            ////// 登録処理を実行する場合、コントロール未指定で変換する
            ////this.resultInfoDictionary = ConvertToUploadResultDictionary("", uploadList);
            ////// トランザクション開始
            ////using (var transaction = this.db.Connection.BeginTransaction())
            ////{
            ////    try
            ////    {
            ////        // 登録処理実行
            ////        result = RegistImpl();

            ////        if (result > 0)
            ////        {
            ////            // コミット
            ////            transaction.Commit();
            ////        }
            ////        else
            ////        {
            ////            // ロールバック
            ////            transaction.Rollback();
            ////        }
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        if (transaction != null)
            ////        {
            ////            // ロールバック
            ////            transaction.Rollback();
            ////        }
            ////        this.Status = CommonProcReturn.ProcStatus.Error;
            ////        // 「取込処理に失敗しました。」
            ////        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200004 });
            ////        this.LogNo = string.Empty;

            ////        logger.Error(this.MsgId, ex);
            ////        return -1;
            ////    }
            ////}
            //// ↑↑↑ 登録処理を実行する場合 ↑↑↑

            //// 正常終了
            //this.Status = CommonProcReturn.ProcStatus.Valid;

            return result;
        }
        #endregion

    }
}