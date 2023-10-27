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
using Dao = BusinessLogic_PT0002.BusinessLogicDataClass_PT0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using STDData = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;

namespace BusinessLogic_PT0002
{
    /// <summary>
    /// 入出庫一覧
    /// </summary>
    public partial class BusinessLogic_PT0002 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// フォーム種類
        /// </summary>
        private static class FormType
        {
            /// <summary>一覧画面</summary>
            public const byte List = 0;
        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            /// <summary>
            /// 検索条件の画面項目定義テーブルのコントロールID(作業日)
            /// </summary>
            public const string WorkingDay = "COND_000_00_LST_0";

            /// <summary>
            /// 検索結果の画面項目定義テーブルのコントロールID(入庫)
            /// </summary>
            public const string EnterResults = "BODY_030_00_LST_0";

            /// <summary>
            /// 検索結果の画面項目定義テーブルのコントロールID(出庫：親要素)
            /// </summary>
            public const string IssueResults = "BODY_050_00_LST_0";

            /// <summary>
            /// 検索結果の画面項目定義テーブルのコントロールID(出庫：子要素)
            /// </summary>
            public const string IssueResultsChild = "BODY_100_00_LST_0";

            /// <summary>
            /// 検索結果の画面項目定義テーブルのコントロールID(棚番移庫)
            /// </summary>
            public const string ShedResults = "BODY_070_00_LST_0";

            /// <summary>
            /// 検索結果の画面項目定義テーブルのコントロールID(部門移庫)
            /// </summary>
            public const string CategoryResults = "BODY_090_00_LST_0";
            public static class Button
            {
                /// <summary>
                /// 出力ボタン（入庫一覧）
                /// </summary>
                public const string OutputEnter = "OutputEnter";
                /// <summary>
                /// 出力ボタン（出庫一覧）
                /// </summary>
                public const string OutputIssue = "OutputIssue";
                /// <summary>
                /// 出力ボタン（棚番移庫一覧）
                /// </summary>
                public const string OutputShed = "OutputShed";
                /// <summary>
                /// 出力ボタン（部門移庫一覧）
                /// </summary>
                public const string OutputCategory = "OutputCategory";
                /// <summary>
                /// 購入明細書ボタン
                /// </summary>
                public const string OutputPurchaseDetails = "OutputPurchaseDetails";
            }
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL名：入出庫一覧(入庫)</summary>
            public const string GetEnterList = "GetEnterList";
            /// <summary>SQL名：入出庫一覧(出庫)</summary>
            public const string GetIssueList = "GetIssueList";
            /// <summary>SQL名：入出庫一覧(出庫(入れ子))</summary>
            public const string GetIssueListChild = "GetIssueListChild";
            /// <summary>SQL名：入出庫一覧(棚番移庫)</summary>
            public const string GetShedList = "GetShedList";
            /// <summary>SQL名：入出庫一覧(部門移庫)</summary>
            public const string GetCategoryList = "GetCategoryList";
            /// <summary>SQL名：棚IDより倉庫を取得する</summary>
            public const string GetWarehouse = "GetWarehouse";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"InventoryControl";
        }
        /// <summary>
        /// 出力ファイル定義
        /// </summary>
        private static class OutputReportDefine
        {
            /// <summary>テンプレートID</summary>
            public const int TemplateId1 = 1;
            /// <summary>出力パターンID</summary>
            public const int PatternId1 = 1;
            ///// <summary>カラム名（新旧区分）</summary>
            //public const string ColumnNameNewOldName = "old_new_name";
            ///// <summary>カラム名（新旧区分id）</summary>
            //public const string ColumnNameNewOldStructureId = "old_new_structure_id";
            ///// <summary>カラム名（新旧区分）</summary>
            //public const string ColumnNameShippingDivisionName = "shipping_division_name";
            ///// <summary>カラム名（新旧区分id）</summary>
            //public const string ColumnNameShippingDivisionStructureId = "shipping_division_structure_id";
            ///// <summary>カラム名（新旧区分）</summary>
            //public const string ColumnNameStorageLocationName = "storage_location_name";
            ///// <summary>カラム名（新旧区分id）</summary>
            //public const string ColumnNameStorageLocationId = "storage_location_id";
            ///// <summary>カラム名（新旧区分）</summary>
            //public const string ColumnNameToStorageLocationName = "to_storage_location_name";
            ///// <summary>カラム名（新旧区分id）</summary>
            //public const string ColumnNameToStorageLocationId = "to_storage_location_id";
            ///  <summary>カラム名（子レコードとの紐づけ用）</summary>
            public const string LinkColumnNameForChildRecord = "work_no";
            ///  <summary>カラム名追加接続詞（子レコード認識用）</summary>
            public const string AddColumnNameForChildRecord = "_child";
            public static class ReportId
            {
                /// <summary>
                /// 出力ボタン（入庫一覧）
                /// </summary>
                public const string OutputEnter = "RP0370";
                /// <summary>
                /// 出力ボタン（入庫一覧）
                /// </summary>
                public const string OutputIssue = "RP0380";
                /// <summary>
                /// 出力ボタン（入庫一覧）
                /// </summary>
                public const string OutputShed = "RP0390";
                /// <summary>
                /// 出力ボタン（入庫一覧）
                /// </summary>
                public const string OutputCategory = "RP0400";
                /// <summary>
                /// 購入明細書ボタン
                /// </summary>
                public const string OutputPurchaseDetails = "RP0290";
            }
        }
        /// <summary>
        /// グローバルリストのキー名称
        /// </summary>
        public static class GlobalListKeyPT0002
        {
            /// <summary>作業日（タイトル）</summary>
            public const string ConditionTitle1 = "PT0002_ConditionTitle1";
            /// <summary>作業日（値）</summary>
            public const string ConditionValue1 = "PT0002_ConditionValue1";
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_PT0002() : base()
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

            switch (this.FormNo)
            {
                case FormType.List:     // 一覧検索
                    if (!searchList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                default:
                    // この部分は到達不能なので、エラーを返す
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
            // 到達不能
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            // 到達不能
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            // 到達不能
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            int result = 0;
            this.ResultList = new();
            int reportFactoryId = 0;
            Dictionary<int, List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData>> dicSelectKeyDataList = new Dictionary<int, List<SelectKeyData>>();

            switch (this.CtrlId)
            {
                //入庫一覧出力
                case TargetCtrlId.Button.OutputEnter:
                //出庫一覧出力
                case TargetCtrlId.Button.OutputIssue:
                //棚番移庫一覧出力
                case TargetCtrlId.Button.OutputShed:
                //部門移庫一覧出力
                case TargetCtrlId.Button.OutputCategory:

                    result = 0;

                    // 帳票ごとに対象の一覧コントロールIDと帳票IDをセットする
                    string targetCtrlId = string.Empty;
                    string reportId = string.Empty;
                    switch (this.CtrlId)
                    {
                        case TargetCtrlId.Button.OutputEnter:
                            targetCtrlId = TargetCtrlId.EnterResults;
                            reportId = OutputReportDefine.ReportId.OutputEnter;
                            break;
                        case TargetCtrlId.Button.OutputIssue:
                            targetCtrlId = TargetCtrlId.IssueResults;
                            reportId = OutputReportDefine.ReportId.OutputIssue;
                            break;
                        case TargetCtrlId.Button.OutputShed:
                            targetCtrlId = TargetCtrlId.ShedResults;
                            reportId = OutputReportDefine.ReportId.OutputShed;
                            break;
                        case TargetCtrlId.Button.OutputCategory:
                            targetCtrlId = TargetCtrlId.CategoryResults;
                            reportId = OutputReportDefine.ReportId.OutputCategory;
                            break;
                        case TargetCtrlId.Button.OutputPurchaseDetails:
                            targetCtrlId = TargetCtrlId.IssueResults;
                            reportId = OutputReportDefine.ReportId.OutputPurchaseDetails;
                            break;
                    }

                    // ページ情報取得
                    var pageInfo = GetPageInfo(
                    targetCtrlId,                   // 一覧のコントールID
                    this.pageInfoList);             // ページ情報リスト

                    // 検索条件データ取得
                    getSearchConditionByTargetCtrlIdForReport(TargetCtrlId.WorkingDay, out dynamic searchCondition);

                    // 画面の検索条件をグローバル変数より取得
                    // 見出しが必要の為、js側で設定
                    object globalObj;
                    string globalData = string.Empty;
                    globalObj = GetGlobalData(GlobalListKeyPT0002.ConditionTitle1, false);
                    globalData = Convert.ToString(globalObj);
                    this.conditionSheetNameList.Add(globalData);
                    globalObj = GetGlobalData(GlobalListKeyPT0002.ConditionValue1, false);
                    globalData = Convert.ToString(globalObj);
                    this.conditionSheetValueList.Add(globalData);

                    // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                    int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                    reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

                    // 帳票定義取得
                    // 出力帳票シート定義のリストを取得
                    var sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                        TMQUtil.ComReport.GetReportSheetDefine,
                        TMQUtil.ExcelPath,
                        new { FactoryId = reportFactoryId, ReportId = reportId },
                        db);
                    if (sheetDefineList == null)
                    {
                        // 取得できない場合、処理を戻す
                        return ComConsts.RETURN_RESULT.NG;
                    }

                    //集計結果の取得
                    Dictionary<int, IList<dynamic>> dicSummaryDataList = new Dictionary<int, IList<dynamic>>();

                    // SQLの出力項目でシングルクォーテーション「'」を付けない項目
                    List<string> keyNameList = new();
                    switch (this.CtrlId)
                    {
                        case TargetCtrlId.Button.OutputEnter: // 入庫
                            keyNameList = new() { "lot_no" };
                            break;
                        case TargetCtrlId.Button.OutputIssue: // 出庫
                            keyNameList = new() { "work_no", "lot_no_child" };
                            break;
                        case TargetCtrlId.Button.OutputShed:     // 棚番移庫
                        case TargetCtrlId.Button.OutputCategory: // 部門移庫
                            keyNameList = new() { "lot_no", "transfer_no" };
                            break;
                    }

                    // シート定義毎にループ
                    foreach (var sheetDefine in sheetDefineList)
                    {
                        // 検索条件フラグの場合はスキップする
                        if (sheetDefine.SearchConditionFlg == true)
                        {
                            continue;
                        }

                        // 先頭シートのみメモリからデータを取得する
                        if (sheetDefine.SheetNo == 1)
                        {
                            //集計結果の取得
                            List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
                            //集計結果の取得（子データ分）
                            List<Dictionary<string, string>> dataList2 = new List<Dictionary<string, string>>();

                            // 対象リストからデータを取得
                            dataList = getDataListByResultInfoDictionary(targetCtrlId);

                            // 出荷一覧のみ子レコードの結果をマージする
                            if (targetCtrlId == TargetCtrlId.IssueResults)
                            {
                                // 子レコードのデータを取得
                                dataList2 = getDataListByResultInfoDictionary(TargetCtrlId.IssueResultsChild);

                                // 集計結果のマージ
                                List<Dictionary<string, string>> mergeDataList = new List<Dictionary<string, string>>();
                                // 親レコードに対して、紐づけ用の項目の値で紐づく子レコードをマージする
                                // 子レコードの項目名には子レコード認識用の追加接続詞があるため、キー重複は無い前提
                                foreach (Dictionary<string, string> dicData in dataList)
                                {
                                    foreach (Dictionary<string, string> dicData2 in dataList2)
                                    {
                                        if (dicData2.ContainsKey(OutputReportDefine.LinkColumnNameForChildRecord
                                                + OutputReportDefine.AddColumnNameForChildRecord) == true
                                            && dicData2[OutputReportDefine.LinkColumnNameForChildRecord
                                                + OutputReportDefine.AddColumnNameForChildRecord] == dicData[OutputReportDefine.LinkColumnNameForChildRecord])
                                        {
                                            Dictionary<string, string> mergeDic = dicData.Concat(dicData2)
                                                  .GroupBy(
                                                    pair => pair.Key,
                                                    (_, pairs) => pairs.First()).ToDictionary(pair => pair.Key, pair => pair.Value);

                                            mergeDataList.Add(mergeDic);
                                        }
                                    }
                                }
                                dataList.Clear();
                                dataList = new List<Dictionary<string, string>>(mergeDataList);
                            }

                            // ユーザの本務工場を取得
                            List<int> factoryIdList = TMQUtil.GetFactoryIdList(this.UserId, db);
                            factoryIdList.Sort();
                            int factoryId = factoryIdList[factoryIdList.Count - 1];

                            string sql = string.Empty;
                            string sqlItem = string.Empty;
                            foreach (Dictionary<string, string> dic in dataList)
                            {
                                if (sql.Equals(string.Empty) == false)
                                {
                                    sql += " UNION ALL ";
                                }
                                sqlItem = string.Empty;
                                foreach (KeyValuePair<string, string> keyValuePair in dic)
                                {
                                    if (sqlItem.Equals(string.Empty) == false)
                                    {
                                        sqlItem = sqlItem + ",";
                                    }

                                    // 数値項目(リストに格納されている項目)はシングルクォーテーションを付けないようにする
                                    // リストにキー名称が存在するか判定
                                    if (keyNameList.Contains(keyValuePair.Key))
                                    {
                                        // シングルクォーテーションなし
                                        sqlItem += " " + keyValuePair.Value + " AS " + keyValuePair.Key;
                                    }
                                    else
                                    {
                                        // シングルクォーテーションあり
                                        sqlItem += "'" + keyValuePair.Value + "'" + " AS " + keyValuePair.Key;
                                    }

                                    /*
                                                                        // 個別処理：新旧区分の埋め込み
                                                                        if(keyValuePair.Key == OutputReportDefine.ColumnNameNewOldStructureId)
                                                                        {
                                                                            sqlItem += "," + "[dbo].[get_v_structure_item](" + keyValuePair.Value + "," + factoryId + ",'" + this.LanguageId + "') AS " + OutputReportDefine.ColumnNameNewOldName;
                                                                        }

                                                                        // 個別処理：出庫区分の埋め込み
                                                                        if (keyValuePair.Key == OutputReportDefine.ColumnNameShippingDivisionStructureId)
                                                                        {
                                                                            sqlItem += "," + "[dbo].[get_v_structure_item](" + keyValuePair.Value + "," + factoryId + ",'" + this.LanguageId + "') AS " + OutputReportDefine.ColumnNameShippingDivisionName;
                                                                        }

                                                                        // 個別処理：予備品倉庫の埋め込み
                                                                        if (keyValuePair.Key == OutputReportDefine.ColumnNameStorageLocationId)
                                                                        {
                                                                            sqlItem += "," + "[dbo].[get_v_structure_item](" + keyValuePair.Value + "," + factoryId + ",'" + this.LanguageId + "') AS " + OutputReportDefine.ColumnNameStorageLocationName;
                                                                        }

                                                                        // 個別処理：予備品倉庫（移庫先）の埋め込み
                                                                        if (keyValuePair.Key == OutputReportDefine.ColumnNameToStorageLocationId)
                                                                        {
                                                                            sqlItem += "," + "[dbo].[get_v_structure_item](" + keyValuePair.Value + "," + factoryId + ",'" + this.LanguageId + "') AS " + OutputReportDefine.ColumnNameToStorageLocationName;
                                                                        }
                                    */
                                }
                                sql += "SELECT " + sqlItem;
                                sql += ", '1' AS output_report_location_name_got_flg "; // 機能場所名称情報取得済フラグ（帳票用）
                                sql += ", '1' AS output_report_job_name_got_flg "; // 職種・機種名称情報取得済フラグ（帳票用）
                            }
                            var summaryDataList = db.GetListByDataClass<dynamic>(sql.ToString());
                            dicSummaryDataList.Add(sheetDefine.SheetNo, summaryDataList);
                        }
                    }

                    // エクセル出力共通処理
                    TMQUtil.CommonOutputExcel(
                        reportFactoryId,                        // 工場ID
                        this.PgmId,                             // プログラムID
                        null,                                   // シートごとのパラメータでの選択キー情報リスト
                        searchCondition,                        // 検索条件
                        reportId,                               // 帳票ID
                        OutputReportDefine.TemplateId1,         // テンプレートID
                        OutputReportDefine.PatternId1,          // 出力パターンID
                        this.UserId,                            // ユーザID
                        this.LanguageId,                        // 言語ID
                        this.conditionSheetLocationList,        // 場所階層構成IDリスト
                        this.conditionSheetJobList,             // 職種機種構成IDリスト
                        this.conditionSheetNameList,            // 検索条件項目名リスト
                        this.conditionSheetValueList,           // 検索条件設定値リスト
                        out string fileType,                    // ファイルタイプ
                        out string fileName,                    // ファイル名
                        out MemoryStream memStream,             // メモリストリーム
                        out string message,                     // メッセージ
                        db,
                        null,
                        null,
                        dicSummaryDataList);

                    // OUTPUTパラメータに設定
                    this.OutputFileType = ComConsts.REPORT.FILETYPE.EXCEL;
                    this.OutputFileName = fileName;
                    this.OutputStream = memStream;

                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                    return ComConsts.RETURN_RESULT.OK;
                    break;
                //購入明細書出力
                case TargetCtrlId.Button.OutputPurchaseDetails:
                    //購入明細書出力
                    reportId = OutputReportDefine.ReportId.OutputPurchaseDetails;
                    // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                    userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                    reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

                    // 帳票定義取得
                    // 出力帳票シート定義のリストを取得
                    sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                        TMQUtil.ComReport.GetReportSheetDefine,
                        TMQUtil.ExcelPath,
                        new { FactoryId = reportFactoryId, ReportId = reportId },
                        db);
                    if (sheetDefineList == null)
                    {
                        // 取得できない場合、処理を戻す
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    // シート定義毎にループ
                    foreach (var sheetDefine in sheetDefineList)
                    {
                        // 検索条件フラグの場合はスキップする
                        if (sheetDefine.SearchConditionFlg == true)
                        {
                            continue;
                        }
                        Key keyInfo = getKeyInfoByTargetSqlParams(sheetDefine.TargetSqlParams);

                        // 帳票用選択キーデータ取得
                        // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                        List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport(
                            TargetCtrlId.IssueResults,     // 依頼情報のコントールID
                            keyInfo,                     // 設定したキー情報
                            this.resultInfoDictionary);  // 画面データ

                        // シートNoをキーとして帳票用選択キーデータを保存する
                        dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                    }

                    //// 検索条件データ取得
                    //getSearchConditionForReport(pageInfo, out dynamic searchCondition);
                    TMQUtil.CommonOutputExcel(
                        reportFactoryId,             // 工場ID
                        this.PgmId,                  // プログラムID
                        dicSelectKeyDataList,        // シートごとのパラメータでの選択キー情報リスト
                        null,                        // 検索条件
                        reportId,                    // 帳票ID
                        1,                           // テンプレートID
                        1,                           // 出力パターンID
                        this.UserId,                 // ユーザID
                        this.LanguageId,             // 言語ID
                        this.conditionSheetLocationList,    // 場所階層構成IDリスト
                        this.conditionSheetJobList,         // 職種機種構成IDリスト
                        this.conditionSheetNameList,        // 検索条件項目名リスト
                        this.conditionSheetValueList,       // 検索条件設定値リスト
                        out fileType,         // ファイルタイプ
                        out fileName,         // ファイル名
                        out memStream,  // メモリストリーム
                        out message,          // メッセージ
                        db);

                    // OUTPUTパラメータに設定
                    this.OutputFileType = fileType;
                    this.OutputFileName = fileName;
                    this.OutputStream = memStream;

                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                    return ComConsts.RETURN_RESULT.OK;

                    break;
                default:
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「コントロールIDが不正です。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID911100001 });

                    // エラーログ出力
                    writeErrorLog(this.MsgId);
                    return ComConsts.RETURN_RESULT.NG;
            }

            return result;

            /// <summary>
            /// 対象リストからデータを取得する処理
            /// </summary>
            /// <param name="targetCtrlId">対象リストコントロールID</param>
            /// <returns>対象データ辞書（スネークケース、値）</returns>
            List<Dictionary<string, string>> getDataListByResultInfoDictionary(string targetCtrlId)
            {
                List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
                var rowDic = new Dictionary<string, string>();

                foreach (var resultInfo in this.resultInfoDictionary)
                {
                    // 対象の一覧の場合のみ
                    if (resultInfo["CTRLID"].ToString() == targetCtrlId)
                    {
                        rowDic = new Dictionary<string, string>();
                        // 子データのみ全レコードを取得対象とする
                        if (targetCtrlId == TargetCtrlId.IssueResultsChild)
                        {
                            // 画面から項目名（スネークケース）、項目値を取得
                            Dao.searchResultIssueParents formData = new();
                            SetDataClassFromDictionary(resultInfo, targetCtrlId, formData);
                            // 取得結果の辞書（スネークケース、値）用データに変換
                            rowDic = getDicItemNameAndValue(formData, true);
                        }
                        else
                        {
                            // 選択データのみ対象とする
                            if (ComUtil.IsSelectedRowDictionary(resultInfo))
                            {
                                if (targetCtrlId == TargetCtrlId.EnterResults)
                                {
                                    // 画面から項目名（スネークケース）、項目値を取得
                                    Dao.searchResultEnter formData = new();
                                    SetDataClassFromDictionary(resultInfo, resultInfo["CTRLID"].ToString(), formData);
                                    // 取得結果の辞書（スネークケース、値）用データに変換
                                    rowDic = getDicItemNameAndValue(formData);
                                }
                                else if (targetCtrlId == TargetCtrlId.IssueResults)
                                {
                                    // 画面から項目名（スネークケース）、項目値を取得
                                    Dao.searchResultIssueParents formData = new();
                                    SetDataClassFromDictionary(resultInfo, targetCtrlId, formData);
                                    // 取得結果の辞書（スネークケース、値）用データに変換
                                    rowDic = getDicItemNameAndValue(formData);
                                }
                                else if (targetCtrlId == TargetCtrlId.ShedResults)
                                {
                                    // 画面から項目名（スネークケース）、項目値を取得
                                    Dao.searchResultShed formData = new();
                                    SetDataClassFromDictionary(resultInfo, targetCtrlId, formData);
                                    // 取得結果の辞書（スネークケース、値）用データに変換
                                    rowDic = getDicItemNameAndValue(formData);
                                }
                                else if (targetCtrlId == TargetCtrlId.CategoryResults)
                                {
                                    // 画面から項目名（スネークケース）、項目値を取得
                                    Dao.searchResultCategory formData = new();
                                    SetDataClassFromDictionary(resultInfo, targetCtrlId, formData);
                                    // 取得結果の辞書（スネークケース、値）用データに変換
                                    rowDic = getDicItemNameAndValue(formData);
                                }
                                else if (targetCtrlId == TargetCtrlId.IssueResultsChild)
                                {
                                    // 画面から項目名（スネークケース）、項目値を取得
                                    Dao.searchResultEnter formData = new();
                                    SetDataClassFromDictionary(resultInfo, targetCtrlId, formData);
                                    // 取得結果の辞書（スネークケース、値）用データに変換
                                    rowDic = getDicItemNameAndValue(formData);
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        dataList.Add(rowDic);
                    }
                }
                return dataList;
            }
            /// <summary>
            /// 項目名（スネークケース）、項目値を取得
            /// </summary>
            /// <param name="formData">対象データ</param>
            /// <param name="childFlg">子レコードフラグ（デフォルト：false）</param>
            /// <returns>対象データ辞書（スネークケース、値）</returns>
            Dictionary<string, string> getDicItemNameAndValue(object formData, bool childFlg = false)
            {
                Dictionary<string, string> rowDic = new Dictionary<string, string>();

                foreach (var property in formData.GetType().GetProperties())
                {
                    string formVal = string.Empty;
                    // 項目名（スネークケース）の取得
                    string propertyName = ComUtil.GetSnakeCase(property.Name);
                    // 子レコードの場合、子レコードの値と認識できるように固定文字列を付加する
                    if (childFlg == true)
                    {
                        propertyName += "_child";
                    }
                    if (property.GetValue(formData) != null)
                    {
                        formVal = property.GetValue(formData).ToString();
                    }
                    rowDic.Add(propertyName, formVal);
                }

                return rowDic;
            }
        }

        #endregion

        #region privateメソッド
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.WorkingDay);

            // 検索条件　職種・場所階層情報取得
            Dao.searchCondition conditionObj = getCondition(out List<string> listUnComment);

            // ページ情報取得(作業日)
            var workingDayInfo = GetPageInfo(TargetCtrlId.WorkingDay, this.pageInfoList);

            // 検索条件データの取得(作業日)
            if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, TargetCtrlId.WorkingDay, conditionObj, workingDayInfo))
            {
                // エラーの場合終了
                return false;
            }

            // 作業日From
            conditionObj.WorkingDay = DateTime.Parse(conditionObj.WorkingDayFrom);

            // 作業日Toの翌日の日付を取得
            conditionObj.WorkingDayNext = DateTime.Parse(conditionObj.WorkingDayTo).AddDays(1);

            // 対象年月の指定範囲が1年を超えて入力された場合はエラー
            if (DateTime.Parse(conditionObj.WorkingDayTo) > conditionObj.WorkingDay.AddYears(1))
            {
                // 「作業日は1年以内で入力して下さい。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141110005 });
                return false;
            }

            // 翻訳の一時テーブルを作成
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);

            // 翻訳する構成グループのリスト
            var structuregroupList = new List<GroupId>
            {
                GroupId.SpareLocation,
                GroupId.Unit,
                GroupId.Currency,
                GroupId.Department,
                GroupId.Account,
                GroupId.OldNewDivition,
                GroupId.IssueDivision
            };

            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            listPf.RegistTempTable(); // 登録

            // 入庫タブ検索
            searchListEnter(conditionObj, listUnComment);

            // 親画面で取得された出庫Noリスト
            List<int> workNoList = new();

            // 出庫タブ親要素検索
            searchListIssue(conditionObj, listUnComment, out workNoList);
            // 出庫タブ子要素検索
            searchListIssueChild(workNoList);

            // 棚番移庫タブ検索
            searchListShed(conditionObj, listUnComment);

            // 部門移庫タブ検索
            searchListCategory(conditionObj, listUnComment);

            return true;
        }

        /// <summary>
        /// 検索条件を取得
        /// </summary>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <returns>条件</returns>
        private Dao.searchCondition getCondition(out List<string> listUnComment)
        {
            listUnComment = new List<string>();

            // 検索条件
            Dao.searchCondition conditionObj = new();

            //場所階層の条件を取得
            setStructureIdList(true);

            //職種の条件を取得s
            setStructureIdList(false);

            // SQLのアンコメントする条件を設定
            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            listUnComment = ComUtil.GetNotNullNameByClass<Dao.searchCondition>(conditionObj);

            return conditionObj;

            //場所階層、職種の条件を取得
            void setStructureIdList(bool isLocation)
            {
                //isLocation=trueの場合、場所階層
                var keyName = isLocation ? STRUCTURE_CONSTANTS.CONDITION_KEY.Location : STRUCTURE_CONSTANTS.CONDITION_KEY.Job;
                var dic = this.searchConditionDictionary.Where(x => x.ContainsKey(keyName)).FirstOrDefault();
                if (dic != null && dic.ContainsKey(keyName))
                {
                    // 選択された構成IDリストから配下の構成IDをすべて取得
                    List<int> locationIdList = dic[keyName] as List<int>;
                    if (locationIdList != null && locationIdList.Count > 0)
                    {
                        //紐づく全ての階層を取得
                        List<STDData.VStructureItemEntity> locationList = TMQUtil.GetStructureItemList(locationIdList, this.db, this.LanguageId);

                        if (isLocation)
                        {
                            //工場IDを抽出
                            conditionObj.FactoryIdList = locationList.Where(x => x.StructureLayerNo == (int)Const.MsStructure.StructureLayerNo.Location.Factory).Select(x => x.StructureId).ToList();
                        }
                        else
                        {
                            //職種IDを抽出
                            conditionObj.JobIdList = locationList.Where(x => x.StructureLayerNo == (int)Const.MsStructure.StructureLayerNo.Job.Job).Select(x => x.StructureId).ToList();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 検索SQL、WITH句,総件数取得SQL文を取得するメソッド
        /// </summary>
        /// <param name="fileName">SQLテキストファイル名</param>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <param name="baseSql">out 取得したSQL文</param>
        /// <param name="withSql">out 取得したWITH句</param>
        /// <param name="execSql">out 総件数取得SQL文</param>
        /// <param name="pageInfo">ページ情報</param>
        /// <returns>取得結果(true:取得OK/false:取得NG )</returns>
        private bool getSql(string fileName, List<string> listUnComment, out string baseSql, out string withSql, out string execSql, CommonWebTemplate.CommonDefinitions.PageInfo pageInfo)
        {
            // 初期化
            baseSql = string.Empty;
            withSql = string.Empty;
            execSql = string.Empty;

            // 検索SQLの取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, fileName, out baseSql, listUnComment))
            {
                return false;
            }

            // WITH句の取得
            if (!TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, fileName, out withSql, listUnComment))
            {
                return false;
            }

            // 総件数取得SQL文の取得
            execSql = TMQUtil.GetSqlStatementSearch(true, baseSql, string.Empty, withSql);

            return true;
        }

        /// <summary>
        /// 総件数を取得し総件数をチェックするメソッド
        /// </summary>
        /// <param name="execSql">総件数取得SQL文</param>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="pageInfo">out ページ情報</param>
        /// <param name="cnt">out 総件数</param>
        /// <returns>取得結果(true:取得、チェックOK/false:チェックNG )</returns>
        private bool checkTotalCount(string execSql, Dao.searchCondition conditionObj, CommonWebTemplate.CommonDefinitions.PageInfo pageInfo, out int cnt)
        {
            // 総件数を取得
            cnt = db.GetCount(execSql, conditionObj);
            // 総件数のチェック
            if (cnt > 0 && !CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 一覧検索SQLを取得し、ORDER BY句を追加するメソッド
        /// </summary>
        /// <param name="execSql">総件数取得SQL文</param>
        /// <param name="baseSql">SQL文</param>
        /// <param name="withSql">WITH句</param>
        /// <param name="selectSql">一覧検索SQL</param>
        /// <returns>取得結果(true:取得OK/false:取得NG )</returns>
        private bool getListSearchSql(string execSql, string baseSql, string withSql, string orderBy, CommonWebTemplate.CommonDefinitions.PageInfo pageInfo, out StringBuilder selectSql)
        {
            // 一覧検索SQL文の取得
            execSql = TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql, false, pageInfo.SelectMaxCnt);
            // 検索SQLにORDER BYを追加
            selectSql = new StringBuilder(execSql);
            selectSql.AppendLine(orderBy);

            return true;
        }
        /// <summary>
        /// 出力帳票シート定義の対象sqlパラメータよりキー情報を取得する
        /// </summary>
        /// <param name="targetSqlParams">対象sqlパラメータ</param>
        /// <returns>キー情報</returns>
        private Key getKeyInfoByTargetSqlParams(string targetSqlParams)
        {
            string keyParam1 = string.Empty;
            string keyParam2 = string.Empty;
            string keyParam3 = string.Empty;
            if (targetSqlParams != null && string.IsNullOrEmpty(targetSqlParams) == false)
            {
                string[] sqlParams = targetSqlParams.Split("|");
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    if (i == 0)
                    {
                        keyParam1 = sqlParams[i];
                    }
                    else if (i == 1)
                    {
                        keyParam2 = sqlParams[i];
                    }
                    if (i == 2)
                    {
                        keyParam3 = sqlParams[i];
                    }
                }
            }
            return new Key(keyParam1, keyParam2, keyParam3);
        }
        #endregion
    }
}