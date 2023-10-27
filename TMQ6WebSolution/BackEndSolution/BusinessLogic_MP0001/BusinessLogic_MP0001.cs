using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonTMQUtil;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId;
using static CommonTMQUtil.CommonTMQConstants.MsStructure.StructureLayerNo;
using static CommonTMQUtil.CommonTMQUtil;
using static CommonTMQUtil.CommonTMQUtil.StructureLayerInfo;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MP0001.BusinessLogicDataClass_MP0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;

/// <summary>
/// 保全実績評価
/// </summary>
namespace BusinessLogic_MP0001
{
    /// <summary>
    /// 保全実績評価
    /// </summary>
    public class BusinessLogic_MP0001 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

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
        /// 一覧行番号
        /// </summary>
        private static class RowNo
        {
            /// <summary>0行目</summary>
            public const int Zero = 0;
            /// <summary>1行目</summary>
            public const int One = 1;
            /// <summary>2行目</summary>
            public const int Two = 2;
            /// <summary>3行目</summary>
            public const int Three = 3;
            /// <summary>4行目</summary>
            public const int Four = 4;
            /// <summary>6行目</summary>
            public const int Six = 6;
            /// <summary>8行目</summary>
            public const int Eight = 8;
            /// <summary>9行目</summary>
            public const int Nine = 9;
            /// <summary>10行目</summary>
            public const int Ten = 10;
            /// <summary>11行目</summary>
            public const int Eleven = 11;
            /// <summary>12行目</summary>
            public const int Twelve = 12;
            /// <summary>15行目</summary>
            public const int Fifteen = 15;
            /// <summary>18行目</summary>
            public const int Eighteen = 18;
            /// <summary>21行目</summary>
            public const int TwentyOne = 21;
            /// <summary>24行目</summary>
            public const int TwentyFour = 24;
        }

        /// <summary>
        /// データの存在チェックをかける一覧数
        /// </summary>
        private static class DataCnt
        {
            /// <summary>5件</summary>
            public const int Cnt = 5;
        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            /// <summary>
            /// 保全実績評価 集計条件(地区～設備)
            /// </summary>
            public const string Placehierarchy = "BODY_000_00_LST_0";
            /// <summary>
            /// 保全実績評価 集計条件(対象年月度)
            /// </summary>
            public const string TargetYearMonth = "BODY_020_00_LST_0";
            /// <summary>
            /// 保全実績評価 概要
            /// </summary>
            public const string OverviewList = "BODY_040_00_LST_0";
            /// <summary>
            /// 保全実績評価 詳細(系の停止)
            /// </summary>
            public const string StopSystemList = "BODY_050_00_LST_0";
            /// <summary>
            /// 保全実績評価 詳細(故障修理件数)
            /// </summary>
            public const string RepairsList = "BODY_060_00_LST_0";
            /// <summary>
            /// 保全実績評価 詳細(作業計画性分類1)
            /// </summary>
            public const string WorkPlanList1 = "BODY_070_00_LST_0";
            /// <summary>
            /// 保全実績評価 詳細(作業計画性分類2)
            /// </summary>
            public const string WorkPlanList2 = "BODY_080_00_LST_0";
            /// <summary>
            /// 保全実績評価 詳細(作業性格分類)
            /// </summary>
            public const string WorkPersonalityList = "BODY_090_00_LST_0";
            /// <summary>
            /// 保全実績評価 詳細(その他)
            /// </summary>
            public const string OthersList = "BODY_100_00_LST_0";

        }

        /// <summary>
        /// ボタンコントロール
        /// </summary>
        public static class Button
        {
            /// <summary>
            /// 分析結果出力ボタン
            /// </summary>
            public const string Output = "Output";
        }
        /// <summary>
        /// 帳票定義
        /// </summary>
        private static class ReportDefine
        {
            /// <summary>帳票ID</summary>
            public const string ReportIdMQActualResults = "RP0320";
            /// <summary>テンプレートID上期</summary>
            public const int TemplateId1 = 1;
            /// <summary>テンプレートID下期</summary>
            public const int TemplateId2 = 2;
            /// <summary>パターンID</summary>
            public const int PatternId = 1;
            /// <summary>カラム名ヘッダー部</summary>
            public const string ColumnNameHeader = "summary_report_item_";
            /// <summary>カラム名対象年月</summary>
            public const string ColumnNameTargetMonth = "target_month";
            /// <summary>カラム名工場名</summary>
            public const string ColumnNameFactoryName = "factory_name";
            /// <summary>カラム名プラント名</summary>
            public const string ColumnNamePlantName = "plant_name";
            /// <summary>カラム名工程名</summary>
            public const string ColumnNameStrokeName = "stroke_name";
            /// <summary>カラム名職種名</summary>
            public const string ColumnNameJobName = "job_name";
            // 見出し（VAL1～VAL3）
            public const string ValTitle1 = "VAL1";
            public const string ValTitle2 = "VAL2";
            public const string ValTitle3 = "VAL3";
            // VAL4：当月（1～）、半期（6～）、年度（11～）
            public const string ValTerm = "VAL4";
            // VAL5～VAL8から集計結果を取得
            // 機械    ：VAL5
            // 電気    ：VAL6
            // 計装    ：VAL7
            // その他  ：VAL8
            // 合計    ：VAL9
            public const string ValMachine = "VAL5";
            public const string ValElectricity = "VAL6";
            public const string ValInstrumentation = "VAL7";
            public const string ValOther = "VAL8";
            public const string ValTotal = "VAL9";
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL名：概要取得</summary>
            public const string GetOverviewDetail = "GetOverviewDetail";
            /// <summary>SQL名：詳細(系の停止)取得</summary>
            public const string GetSystemStopDetail = "GetSystemStopDetail";
            /// <summary>SQL名：詳細(故障修理件数)取得</summary>
            public const string GetNumberOfRepairsDetail = "GetNumberOfRepairsDetail";
            /// <summary>SQL名：詳細(作業計画性分類1)取得</summary>
            public const string GetWorkPlan1Detail = "GetWorkPlan1Detail";
            /// <summary>SQL名：詳細(作業計画性分類2)取得</summary>
            public const string GetWorkPlan2Detail = "GetWorkPlan2Detail";
            /// <summary>SQL名：詳細(作業性格分類)取得</summary>
            public const string GetWorkPersonalityDetail = "GetWorkPersonalityDetail";
            /// <summary>SQL名：詳細(その他)取得 一般工場</summary>
            public const string GetOthersDetailFlg0 = "GetOthersDetailFlg0";
            /// <summary>SQL名：詳細(その他)取得 個別工場</summary>
            public const string GetOthersDetailFlg1 = "GetOthersDetailFlg1";
            /// <summary>SQL名：工場ごとの保全履歴個別工場フラグ取得</summary>
            public const string GetStructureIdByexdata = "GetStructureIdByexdata";
            /// <summary>SQL名：場所階層取得</summary>
            public const string GetStructureId = "GetStructureId";
            /// <summary>SQL名：MQ指標実績表（年俸）の集計結果</summary>
            public const string GetRepMqActualEvaluation = "GetRepMqActualEvaluation";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"MaintenancePerformance";
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_MP0001() : base()
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
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsBack())
            {
                // 戻る
                return InitSearch();
            }
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

            // データが存在するか
            bool isExistFlg = false;

            switch (this.FormNo)
            {
                case FormType.List:     // 一覧検索
                    if (!searchList(out isExistFlg))
                    {
                        if (!isExistFlg)
                        {
                            // 「対象データが存在しません。」
                            this.MsgId = GetResMessage(new string[] { "111160048", "911120009" });
                            // 検索結果がない場合
                            this.Status = CommonProcReturn.ProcStatus.Error;
                        }
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

            switch (this.CtrlId)
            {
                case Button.Output:
                    //分析結果出力

                    //分析結果出力
                    result = 0;

                    // 画面上の対象年月を取得
                    Dao.searchCondition conditionObj = new Dao.searchCondition();
                    // 場所階層IDリスト
                    List<int> locationIdList = new();

                    // 検索条件取得
                    if (!GetConditions(out conditionObj, out locationIdList))
                    {
                        // エラーの場合終了
                        return ComConsts.RETURN_RESULT.NG;
                    }

                    // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                    int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                    reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, ReportDefine.ReportIdMQActualResults, this.db) ? userFactoryId : 0;

                    // 帳票定義取得
                    // 出力帳票シート定義のリストを取得
                    var sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                        TMQUtil.ComReport.GetReportSheetDefine,
                        TMQUtil.ExcelPath,
                        new { FactoryId = reportFactoryId, ReportId = ReportDefine.ReportIdMQActualResults },
                        db);
                    if (sheetDefineList == null)
                    {
                        // 取得できない場合、処理を戻す
                        return ComConsts.RETURN_RESULT.NG;
                    }

                    //集計結果の取得
                    Dictionary<int, IList<dynamic>> dicSummaryDataList = new Dictionary<int, IList<dynamic>>();

                    //出力テンプレートID
                    int templateId = ReportDefine.TemplateId1;

                    // 工場、プラント、工程、職種出力
                    string factoryName = string.Empty;
                    string plantName = string.Empty;
                    string strokeName = string.Empty;
                    string jobName = string.Empty;

                    // シート定義毎にループ
                    foreach (var sheetDefine in sheetDefineList)
                    {
                        // 検索条件フラグの場合はスキップする
                        if (sheetDefine.SearchConditionFlg == true)
                        {
                            continue;
                        }

                        //集計結果の取得
                        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

                        //帳票出力用の対象年月
                        DateTime targetMonth = DateTime.Now;

                        var rowDic = new Dictionary<string, string>();
                        // 先頭シートのみメモリからデータを取得する
                        if (sheetDefine.SheetNo == 1)
                        {
                            // 工場、プラント、工程、職種出力
                            factoryName = string.Empty;
                            plantName = string.Empty;
                            strokeName = string.Empty;
                            jobName = string.Empty;

                            List<string> factoryNameList = new List<string>();
                            List<string> plantNameList = new List<string>();
                            List<string> strokeNameList = new List<string>();
                            List<string> jobNameList = new List<string>();

                            // 地区／工場階層で選択されている工場、プラント、工程、職種名を取得する
                            // IDのリストより全ての階層を検索し、階層情報のリストを取得
                            var param = new { StructureIdList = this.conditionSheetLocationList, LanguageId = this.LanguageId, FactoryId = TMQConsts.CommonFactoryId };
                            List<Dao.StructureGetInfo> structureInfoList = SqlExecuteClass.SelectList<Dao.StructureGetInfo>(SqlName.GetStructureId, SqlName.SubDir, param, db);

                            // 場所階層ツリーが選択されていなければエラー
                            if (structureInfoList == null)
                            {
                                return ComConsts.RETURN_RESULT.NG;
                            }

                            // 親構成IDリスト
                            List<int> parentIdList = structureInfoList.Select(x => x.ParentStructureId).ToList();

                            // 最下層のリスト
                            IList<StructureLocationInfoEx> bottomLayerAll = new List<StructureLocationInfoEx>();

                            foreach (var data in structureInfoList)
                            {
                                // 構成IDが親構成IDに含まれていないレコードを見つける
                                if (!parentIdList.Contains(data.StructureId))
                                {
                                    StructureLocationInfoEx temp = new();
                                    temp.LocationStructureId = data.StructureId;
                                    bottomLayerAll.Add(temp);
                                }
                            }

                            // データクラスに地区及び職種の階層情報を設定する処理
                            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureLocationInfoEx>(ref bottomLayerAll, new List<StructureType> { StructureType.Location }, this.db, this.LanguageId);

                            foreach(var bottomLayer in distinctData(bottomLayerAll).OrderBy(x => x.DistrictId).ThenBy(x => x.FactoryId).ThenBy(x => x.PlantId).ThenBy(x => x.SeriesId)
                                            .ThenBy(x => x.StrokeId).ThenBy(x => x.FacilityId))
                            {
                                if (string.IsNullOrEmpty(bottomLayer.FactoryName) == false && factoryNameList.IndexOf(bottomLayer.FactoryName) < 0)
                                {
                                    factoryNameList.Add(bottomLayer.FactoryName);
                                }
                                if (string.IsNullOrEmpty(bottomLayer.PlantName) == false && plantNameList.IndexOf(bottomLayer.PlantName) < 0)
                                {
                                    plantNameList.Add(bottomLayer.PlantName);
                                }
                                if (string.IsNullOrEmpty(bottomLayer.StrokeName) == false && strokeNameList.IndexOf(bottomLayer.StrokeName) < 0)
                                {
                                    strokeNameList.Add(bottomLayer.StrokeName);
                                }
                            }
                            factoryName = factoryNameList.Count > 0 ? string.Join(",", factoryNameList) : string.Empty;
                            plantName = plantNameList.Count > 0 ? string.Join(",", plantNameList) : string.Empty;
                            strokeName = strokeNameList.Count > 0 ? string.Join(",", strokeNameList) : string.Empty;

                            param = new { StructureIdList = this.conditionSheetJobList, LanguageId = this.LanguageId, FactoryId = TMQConsts.CommonFactoryId };
                            List<Dao.StructureGetInfo> structureJobInfoList = SqlExecuteClass.SelectList<Dao.StructureGetInfo>(SqlName.GetStructureId, SqlName.SubDir, param, db);

                            // 職種階層ツリーが選択されていれば職種名を取得
                            if (structureJobInfoList != null)
                            {
                                // 親構成IDリスト
                                List<int> jobParentIdList = structureJobInfoList.Select(x => x.ParentStructureId).ToList();
                                // 最下層のリスト
                                IList<StructureJobInfoEx> jobBottomLayerAll = new List<StructureJobInfoEx>();
                                foreach (var data in structureJobInfoList)
                                {
                                    // 構成IDが親構成IDに含まれていないレコードを見つける
                                    if (!parentIdList.Contains(data.StructureId))
                                    {
                                        StructureJobInfoEx temp = new();
                                        temp.JobStructureId = data.StructureId;
                                        jobBottomLayerAll.Add(temp);
                                    }
                                }
                                // データクラスに地区及び職種の階層情報を設定する処理
                                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureJobInfoEx>(ref jobBottomLayerAll, new List<StructureType> { StructureType.Job }, this.db, this.LanguageId);
                                foreach (var bottomLayer in jobBottomLayerAll.OrderBy(x => x.JobId))
                                {
                                    if (string.IsNullOrEmpty(bottomLayer.JobName) == false && jobNameList.IndexOf(bottomLayer.JobName) < 0)
                                    {
                                        jobNameList.Add(bottomLayer.JobName);
                                    }
                                }
                            }
                            jobName = jobNameList.Count > 0 ? string.Join(",", jobNameList) : string.Empty;

                            //帳票出力用の対象年月
                            targetMonth = conditionObj.TargetYear;

                            int startIndex = 0;

                            // 表の見出し（レコード区切り判定で使用）
                            string title1 = string.Empty;
                            string title2 = string.Empty;
                            string title3 = string.Empty;

                            // 期間
                            string term1 = GetResMessage(new string[] { ComRes.ID.ID111200010 });              //当月
                            string term2 = GetResMessage(new string[] { ComRes.ID.ID111260023 });              //半期
                            string term3 = GetResMessage(new string[] { ComRes.ID.ID111240001 });              //年度

                            foreach (var resultInfo in this.resultInfoDictionary)
                            {
                                switch (resultInfo["CTRLID"].ToString())
                                {
                                    // 保全実績評価 概要
                                    case TargetCtrlId.OverviewList:

                                        break;
                                    // 保全実績評価 詳細(系の停止)
                                    case TargetCtrlId.StopSystemList:
                                    // 保全実績評価 詳細(故障修理件数)
                                    case TargetCtrlId.RepairsList:
                                    // 保全実績評価 詳細(作業計画性分類1)
                                    case TargetCtrlId.WorkPlanList1:
                                    // 保全実績評価 詳細(作業計画性分類2)
                                    case TargetCtrlId.WorkPlanList2:
                                    // 保全実績評価 詳細(作業性格分類)
                                    case TargetCtrlId.WorkPersonalityList:
                                    // 保全実績評価 詳細(その他)
                                    case TargetCtrlId.OthersList:

                                        // VAL1～VAL3
                                        if ((title1 != resultInfo[ReportDefine.ValTitle1].ToString()
                                            || title2 != resultInfo[ReportDefine.ValTitle2].ToString()
                                            || title3 != resultInfo[ReportDefine.ValTitle3].ToString()) &&
                                            (title1 != string.Empty || title2 != string.Empty || title3 != string.Empty) &&
                                            (resultInfo[ReportDefine.ValTitle1].ToString() != string.Empty ||
                                            resultInfo[ReportDefine.ValTitle2].ToString() != string.Empty ||
                                            resultInfo[ReportDefine.ValTitle3].ToString() != string.Empty))
                                        {
                                            // 見出しが変更されている場合、次の項目に移動していると判定して
                                            // 前のデータまでを登録対象とする
                                            dataList.Add(rowDic);
                                            rowDic = new Dictionary<string, string>();
                                        }

                                        // 開始index
                                        startIndex = 0;
                                        // 期間より開始indexを決める
                                        string term = resultInfo[ReportDefine.ValTerm].ToString();
                                        if (term == term1)
                                        {
                                            startIndex = startIndex;
                                        }
                                        else if (term == term2)
                                        {
                                            startIndex = startIndex + 5;
                                        }
                                        else if (term == term3)
                                        {
                                            startIndex = startIndex + 10;
                                        }

                                        // summary_report_item_1～15で結果値を登録する
                                        rowDic.Add(ReportDefine.ColumnNameHeader + (startIndex + 1).ToString(), resultInfo[ReportDefine.ValMachine].ToString());
                                        rowDic.Add(ReportDefine.ColumnNameHeader + (startIndex + 2).ToString(), resultInfo[ReportDefine.ValElectricity].ToString());
                                        rowDic.Add(ReportDefine.ColumnNameHeader + (startIndex + 3).ToString(), resultInfo[ReportDefine.ValInstrumentation].ToString());
                                        rowDic.Add(ReportDefine.ColumnNameHeader + (startIndex + 4).ToString(), resultInfo[ReportDefine.ValOther].ToString());
                                        rowDic.Add(ReportDefine.ColumnNameHeader + (startIndex + 5).ToString(), resultInfo[ReportDefine.ValTotal].ToString());

                                        if (resultInfo[ReportDefine.ValTitle1].ToString().Equals(string.Empty) == false
                                            || resultInfo[ReportDefine.ValTitle2].ToString().Equals(string.Empty) == false
                                            || resultInfo[ReportDefine.ValTitle3].ToString().Equals(string.Empty) == false)
                                        {
                                            title1 = resultInfo[ReportDefine.ValTitle1].ToString();
                                            title2 = resultInfo[ReportDefine.ValTitle2].ToString();
                                            title3 = resultInfo[ReportDefine.ValTitle3].ToString();
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            }
                            // 最終処理時のデータ登録
                            dataList.Add(rowDic);
                        }
                        else
                        {
                            // 工場、プラント、工程、職種出力
                            factoryName = string.Empty;
                            plantName = string.Empty;
                            strokeName = string.Empty;
                            jobName = string.Empty;

                            //帳票出力用の対象年月
                            targetMonth = conditionObj.BeginningMonth;

                            //出力テンプレートIDを上期、下期で変更する
                            if(conditionObj.TargetYear.Month >= 4 && conditionObj.TargetYear.Month <= 9)
                            {
                                // 上期用テンプレートを使用
                                templateId = ReportDefine.TemplateId1;
                            }
                            else
                            {
                                // 下期用テンプレートを使用
                                templateId = ReportDefine.TemplateId2;
                            }

                            // MQ指標実績表（年俸）の集計結果を取得
                            string structureIdList = string.Empty;
                            structureIdList = string.Join(",", locationIdList.ToArray());
                            var param = new { StartMonth = conditionObj.BeginningMonth, LocationId = conditionObj.LocationId, StructureIdList = structureIdList, LanguageId = this.LanguageId };
                            var selectDataList = SqlExecuteClass.SelectList<Dao.StructureGetRepMQActualEvaluationData>(SqlName.GetRepMqActualEvaluation, SqlName.SubDir, param, db);
                            if (selectDataList == null)
                            {
                                // 取得データが無ければ次のシートに移動
                                continue;
                            }
                            string rowNo = string.Empty;
                            int columnCnt = 0;
                            rowDic = new Dictionary<string, string>();
                            foreach (var selectData in selectDataList)
                            {
                                if (rowNo != selectData.RowNo.ToString() &&
                                    rowNo != string.Empty)
                                {
                                    // 行番号が変わった場合、次の行番号に移動していると判定して
                                    // 前のデータまでを登録対象とする
                                    dataList.Add(rowDic);
                                    rowDic = new Dictionary<string, string>();
                                    columnCnt = 0;
                                }

                                // summary_report_item_1～5で結果値を登録する
                                rowDic.Add(ReportDefine.ColumnNameHeader + ((columnCnt * 5) + 1).ToString(), selectData.Machine != string.Empty ? selectData.Machine : "0");
                                rowDic.Add(ReportDefine.ColumnNameHeader + ((columnCnt * 5) + 2).ToString(), selectData.Electricity != string.Empty ? selectData.Electricity : "0");
                                rowDic.Add(ReportDefine.ColumnNameHeader + ((columnCnt * 5) + 3).ToString(), selectData.Instrumentation != string.Empty ? selectData.Instrumentation : "0");
                                rowDic.Add(ReportDefine.ColumnNameHeader + ((columnCnt * 5) + 4).ToString(), selectData.Other != string.Empty ? selectData.Other : "0");
                                rowDic.Add(ReportDefine.ColumnNameHeader + ((columnCnt * 5) + 5).ToString(), selectData.Total != string.Empty ? selectData.Total : "0");
                                columnCnt += 1;
                                rowNo = selectData.RowNo.ToString();
                            }
                            // 最終処理時のデータ登録
                            dataList.Add(rowDic);
                        }

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
                                sqlItem += "'" + keyValuePair.Value + "'" + " AS " + keyValuePair.Key;
                            }
                            sql += "SELECT " + sqlItem;
                            // 対象年月を埋め込む
                            sql += ",'" + conditionObj.TargetYear + "' AS " + ReportDefine.ColumnNameTargetMonth;
                            // 工場、プラント、工程、職種を埋め込む
                            sql += ",'" + factoryName + "' AS " + ReportDefine.ColumnNameFactoryName;
                            sql += ",'" + plantName + "' AS " + ReportDefine.ColumnNamePlantName;
                            sql += ",'" + strokeName + "' AS " + ReportDefine.ColumnNameStrokeName;
                            sql += ",'" + jobName + "' AS " + ReportDefine.ColumnNameJobName;
                        }
                        var summaryDataList = db.GetListByDataClass<dynamic>(sql.ToString());
                        dicSummaryDataList.Add(sheetDefine.SheetNo, summaryDataList);
                    }

                    // エクセル出力共通処理
                    TMQUtil.CommonOutputExcel(
                        reportFactoryId,                        // 工場ID
                        this.PgmId,                             // プログラムID
                        null,                                   // シートごとのパラメータでの選択キー情報リスト
                        null,                                   // 検索条件
                        ReportDefine.ReportIdMQActualResults,   // 帳票ID
                        templateId,                                      // テンプレートID
                        ReportDefine.PatternId,                 // 出力パターンID
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

                default:
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「コントロールIDが不正です。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID911100001 });

                    // エラーログ出力
                    writeErrorLog(this.MsgId);
                    return ComConsts.RETURN_RESULT.NG;
            }

            return result;

        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 場所階層一覧検索
        /// </summary>
        private int searchCondition()
        {
            // ページ情報取得(条件)
            Dao.searchCondition conditionObj = new Dao.searchCondition();

            // 場所階層ツリーの取得
            List<int> locationIdList = GetLocationTreeValues();

            // IDのリストより全ての階層を検索し、階層情報のリストを取得
            var param = new { StructureIdList = locationIdList, LanguageId = this.LanguageId, FactoryId = TMQConsts.CommonFactoryId };
            List<Dao.StructureGetInfo> structureInfoList = SqlExecuteClass.SelectList<Dao.StructureGetInfo>(SqlName.GetStructureId, SqlName.SubDir, param, db);

            // ページ情報取得
            var hierarchyInfo = GetPageInfo(TargetCtrlId.Placehierarchy, this.pageInfoList);
            hierarchyInfo.CtrlId = TargetCtrlId.Placehierarchy;

            // 場所階層ツリーが選択されていなければエラー
            if (structureInfoList == null)
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 親構成IDリスト
            List<int> parentIdList = structureInfoList.Select(x => x.ParentStructureId).ToList();

            // 最下層のリスト
            IList<StructureLocationInfoEx> bottomLayerAll = new List<StructureLocationInfoEx>();

            foreach (var data in structureInfoList)
            {
                // 構成IDが親構成IDに含まれていないレコードを見つける
                if (!parentIdList.Contains(data.StructureId))
                {
                    StructureLocationInfoEx temp = new();
                    temp.LocationStructureId = data.StructureId;
                    bottomLayerAll.Add(temp);
                }
            }

            // データクラスに地区及び職種の階層情報を設定する処理
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureLocationInfoEx>(ref bottomLayerAll, new List<StructureType> {StructureType.Location }, this.db, this.LanguageId);

            // 画面上で重複しているように見えるため、機能場所階層IDを一括0で上書き
            bottomLayerAll.Where(x => x.LocationStructureId != null).Select(x => x.LocationStructureId = 0).ToList();

            // 地区、工場、プラント、系列、工程、設備の順に並び替え(場所階層ツリーの表示順に合わせる)
            var sortList = bottomLayerAll.OrderBy(x => x.DistrictId).ThenBy(x => x.FactoryId).ThenBy(x => x.PlantId).ThenBy(x => x.SeriesId)
                                            .ThenBy(x => x.StrokeId).ThenBy(x => x.FacilityId).ToList();

            // 検索結果の設定
            if (SetSearchResultsByDataClass<StructureLocationInfoEx>(hierarchyInfo, distinctData(sortList), distinctData(sortList).Count(), true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// リストの重複を削除する
        /// </summary>
        /// <param name="sortList">ソート済みのリスト</param>
        /// <returns>重複のないリスト情報</returns>
        private List<StructureLocationInfoEx> distinctData(IList<StructureLocationInfoEx> sortList)
        {
            // 重複のないリスト
            var distinctList = new List<StructureLocationInfoEx>();

            // 前の行
            bool previousFlg = true;

            // 前行の地区・工場・プラント・系列・工程・設備
            int[] preLacation = new int[] { 0, 0, 0, 0, 0, 0 };

            // 後行の地区・工場・プラント・系列・工程・設備
            int[] backLacation = new int[] { 0, 0, 0, 0, 0, 0 };

            StringBuilder previousSb = new StringBuilder();
            StringBuilder backSb = new StringBuilder();

            // 地区から順に比較する
            foreach (var list in sortList)
            {
                if (previousFlg)
                {
                    // 初期化
                    previousSb = new StringBuilder();

                    preLacation[0] = list.DistrictId ?? 0;  //地区
                    preLacation[1] = list.FactoryId ?? 0;   //工場
                    preLacation[2] = list.PlantId ?? 0;     //プラント
                    preLacation[3] = list.SeriesId ?? 0;    //系列
                    preLacation[4] = list.StrokeId ?? 0;    //工程
                    preLacation[5] = list.FacilityId ?? 0;  //設備

                    // 地区～設備までの値を繋げる
                    foreach (var data in preLacation)
                    {
                        previousSb.Append(data.ToString());
                    }

                    // 同一データでなく、工場までのデータでなければリスト追加
                    if (previousSb.ToString() != backSb.ToString() && previousSb.ToString().Substring(previousSb.ToString().Length - 4) != "0000")
                    {
                        distinctList.Add(list); //リスト追加
                        previousFlg = false;
                    }
                    continue;
                }

                // 初期化
                backSb = new StringBuilder();

                backLacation[0] = list.DistrictId ?? 0;  //地区
                backLacation[1] = list.FactoryId ?? 0;   //工場
                backLacation[2] = list.PlantId ?? 0;     //プラント
                backLacation[3] = list.SeriesId ?? 0;    //系列
                backLacation[4] = list.StrokeId ?? 0;    //工程
                backLacation[5] = list.FacilityId ?? 0;  //設備

                // 地区～設備までの値を繋げる
                foreach (var data in backLacation)
                {
                    backSb.Append(data.ToString());
                }

                // 同一データでなく、工場までのデータでなければリスト追加
                if (previousSb.ToString() != backSb.ToString() && previousSb.ToString().Substring(backSb.ToString().Length - 4) != "0000")
                {
                    distinctList.Add(list); //リスト追加
                    previousFlg = true;
                }
            }
            return distinctList;
        }

        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="isExistFlg">データが存在するかどうか</param>
        /// <returns>検索成功の場合：True</returns>
        private bool searchList(out bool isExistFlg)
        {
            // データが存在するかどうか
            isExistFlg = false;

            // 検索条件検索
            int conditionResult = searchCondition();
            if (conditionResult == ComConsts.RETURN_RESULT.NG)
            {
                // エラーの場合終了
                return false;
            }

            // ページ情報取得(条件)
            Dao.searchCondition conditionObj = new Dao.searchCondition();
            // 場所階層IDリスト
            List<int> locationIdList = new();

            // 検索条件取得
            if (!GetConditions(out conditionObj, out locationIdList))
            {
                // エラーの場合終了
                return false;
            }

            // データの存在チェックをかける一覧数
            int dataCnt = DataCnt.Cnt;

            // ページ情報取得(一覧)
            var pageInfo = this.pageInfoList;
            foreach (var info in pageInfo)
            {
                switch (info.CtrlId)
                {
                    case TargetCtrlId.OverviewList:

                        // 概要 画面表示
                        GetOverviewList(conditionObj, info);
                        break;

                    case TargetCtrlId.StopSystemList:

                        // 詳細(系の停止) 画面表示
                        GetStopSystemList(conditionObj, info, out isExistFlg);
                        reduceCnt(isExistFlg, ref dataCnt);
                        break;

                    case TargetCtrlId.RepairsList:

                        // 詳細(故障修理件数) 画面表示
                        GetRepairsList(conditionObj, info, out isExistFlg);
                        reduceCnt(isExistFlg, ref dataCnt);
                        break;

                    case TargetCtrlId.WorkPlanList1:

                        // 詳細(作業計画性分類) 画面表示
                        GetWorkPlanList1(conditionObj, info, out isExistFlg);
                        reduceCnt(isExistFlg, ref dataCnt);
                        break;

                    case TargetCtrlId.WorkPlanList2:

                        // 詳細(作業計画性分類2) 画面表示
                        GetWorkPlanList2(conditionObj, info);
                        break;

                    case TargetCtrlId.WorkPersonalityList:

                        // 詳細(作業性格分類) 画面表示
                        GetPersonalityList(conditionObj, info, out isExistFlg);
                        reduceCnt(isExistFlg, ref dataCnt);
                        break;

                    case TargetCtrlId.OthersList:

                        // 詳細(その他) 画面表示
                        GetOthersList(conditionObj, info, locationIdList, out isExistFlg);
                        reduceCnt(isExistFlg, ref dataCnt);
                        break;

                    default:
                        break;
                }
            }
            // データが存在しなければエラー
            if (dataCnt == 0)
            {
                isExistFlg = false;
                return false;
            }

            isExistFlg = true;
            return true;
        }

        /// <summary>
        /// 検索条件取得
        /// </summary>
        /// <param name="conditionObj">条件エリアページ情報</param>
        /// <param name="locationIdList">場所階層IDリスト</param>
        /// <returns>正常：True、異常：False</returns>
        private bool GetConditions(out Dao.searchCondition conditionObj, out List<int> locationIdList)
        {
            // 場所階層IDリスト初期化
            locationIdList = new();

            // ページ情報初期化
            conditionObj = new Dao.searchCondition();

            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.TargetYearMonth);
            List<Dictionary<string, object>> conditionList = new() { condition };

            // ページ情報取得
            var targetYearInfo = GetPageInfo(TargetCtrlId.TargetYearMonth, this.pageInfoList);

            // 検索条件データの取得(対象年月度)
            if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, TargetCtrlId.TargetYearMonth, conditionObj, targetYearInfo))
            {
                // エラーの場合終了
                return false;
            }

            // 対象年月度が表示されていない場合は初期値設定された集計条件を設定(デフォルトで1が入る)
            if (conditionObj.TargetYear.Year == 1)
            {
                //昨日の日付を取得(システム日付の前日の月を条件とする)
                conditionObj.TargetYear = DateTime.Now.AddDays(-1);
            }

            // 対象年月度の月末を条件に追加
            var endDate = DateTime.DaysInMonth(year: conditionObj.TargetYear.Year, month: conditionObj.TargetYear.Month);
            conditionObj.TargetYear = new DateTime(conditionObj.TargetYear.Year, conditionObj.TargetYear.Month, endDate);

            // ユーザの本務工場取得
            int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);

            // 期首月、期の期間取得
            TMQUtil.GetBeginningMonth(out int beginningMonth, out int term, this.db, userFactoryId);

            // 対象指定年月度から年度取得
            int year = conditionObj.TargetYear.Year;
            // 月取得
            int month = conditionObj.TargetYear.Month;

            // 期の開始月取得
            int startMonth = GetStartMonth(beginningMonth, term, month);
            if (startMonth == 0)
            {
                return false;
            }
            conditionObj.StartMonth = new DateTime(year, startMonth, 1);         // 半期集計条件設定(1日固定)
            conditionObj.BeginningMonth = new DateTime(year, beginningMonth, 1); // 累計集計条件設定(1日固定)

            // 場所階層ツリーの取得
            locationIdList = GetLocationTreeValues();
            // 場所階層追加
            conditionObj.LocationId = getStructureId(locationIdList);

            string getStructureId(List<int> structureIdList)
            {
                // 選択された構成リストから配下の構成リストを全て取得
                var list = GetLowerStructureIdList(structureIdList);
                // パラメータ用にカンマ区切りで文字列に変換
                return string.Join(",", list);
            }
            return true;
        }

        /// <summary>
        /// 検索処理実行
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="sqlFileName">SQLファイル名</param>
        /// <returns>検索結果一覧</returns>
        private List<Dao.searchResult> getSummaryInfo(Dao.searchCondition condition, string sqlFileName, string whereParam)
        {
            // 一覧検索実行
            var result = TMQUtil.SqlExecuteClass.SelectList<Dao.searchResult>(sqlFileName, SqlName.SubDir, condition, this.db);
            return result;
        }

        /// <summary>
        /// 概要　取得処理
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="info">ページ情報</param>
        /// <returns>正常：True、異常：False</returns>
        private bool GetOverviewList(Dao.searchCondition conditionObj, CommonWebTemplate.CommonDefinitions.PageInfo info)
        {
            // 一覧検索実行
            var overviewResults = getSummaryInfo(conditionObj, SqlName.GetOverviewDetail, string.Empty);
            if (overviewResults == null)
            {
                return false;
            }

            // 項目
            overviewResults[RowNo.Zero].Item = GetResMessage(new string[] { ComRes.ID.ID111200010 });   //当月
            overviewResults[RowNo.One].Item = GetResMessage(new string[] { ComRes.ID.ID111260023 });    //半期
            overviewResults[RowNo.Two].Item = GetResMessage(new string[] { ComRes.ID.ID111240001 });    //年度

            // 保全活動遷移用パラメータ
            DateTime date = new DateTime(conditionObj.TargetYear.Year, conditionObj.TargetYear.Month, 1);  // 対象年月度の月初
            overviewResults[RowNo.Zero].Date = date.AddMonths(1).AddDays(-1);                              // 対象年月度の月末
            overviewResults[RowNo.One].Date = conditionObj.StartMonth;                                     // 期の開始月
            overviewResults[RowNo.Two].Date = conditionObj.BeginningMonth;                                 // 期首月

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(info, overviewResults, overviewResults.Count(), true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 詳細(系の停止)　取得処理
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="info">ページ情報</param>
        /// <param name="isExist">データが存在するか</param>
        /// <returns>正常：True、異常：False</returns>
        private bool GetStopSystemList(Dao.searchCondition conditionObj, CommonWebTemplate.CommonDefinitions.PageInfo info, out bool isExist)
        {
            isExist = false;

            // 一覧検索実行
            var stopDystemResults = getSummaryInfo(conditionObj, SqlName.GetSystemStopDetail, string.Empty);
            if (stopDystemResults == null)
            {
                return false;
            }

            //データが存在するか調べる
            isExist = isExistData(stopDystemResults);

            // 項目
            stopDystemResults[RowNo.Zero].Item = GetResMessage(new string[] { ComRes.ID.ID111090038 });     //系停止回数
            stopDystemResults[RowNo.Nine].Item = GetResMessage(new string[] { ComRes.ID.ID111090046 });     //系の停止時間
            stopDystemResults[RowNo.Zero].Cause = GetResMessage(new string[] { ComRes.ID.ID111300058 });    //保全要因
            stopDystemResults[RowNo.Three].Cause = GetResMessage(new string[] { ComRes.ID.ID111140035 });   //製造要因
            stopDystemResults[RowNo.Six].Cause = GetResMessage(new string[] { ComRes.ID.ID111090047 });     //計

            // 期 行分ループ
            for (int i = 0; i < RowNo.Twelve; i++)
            {
                if (i == 0 || i % 3 == 0)
                {
                    stopDystemResults[i].Period = GetResMessage(new string[] { ComRes.ID.ID111200010 });    //当月
                }
                else if (i == 1 || (i - 1) % 3 == 0)
                {
                    stopDystemResults[i].Period = GetResMessage(new string[] { ComRes.ID.ID111260023 });    //半期
                }
                else
                {
                    stopDystemResults[i].Period = GetResMessage(new string[] { ComRes.ID.ID111240001 });    //年度
                }
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(info, stopDystemResults, stopDystemResults.Count(), true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 詳細(故障修理件数)　取得処理
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="info">ページ情報</param>
        /// <param name="isExist">データが存在するか</param>
        /// <returns>正常：True、異常：False</returns>
        private bool GetRepairsList(Dao.searchCondition conditionObj, CommonWebTemplate.CommonDefinitions.PageInfo info, out bool isExist)
        {
            isExist = false;

            // 一覧検索実行
            var repairsResults = getSummaryInfo(conditionObj, SqlName.GetNumberOfRepairsDetail, string.Empty);
            if (repairsResults == null)
            {
                return false;
            }

            //データが存在するか調べる
            isExist = isExistData(repairsResults);

            // 項目
            repairsResults[RowNo.Zero].Item = GetResMessage(new string[] { ComRes.ID.ID111100050 });               //故障1
            repairsResults[RowNo.Nine].Item = GetResMessage(new string[] { ComRes.ID.ID111100051 });               //故障2
            repairsResults[RowNo.Eighteen].Item = GetResMessage(new string[] { ComRes.ID.ID111380033 });           //予知修理
            repairsResults[RowNo.TwentyOne].Item = GetResMessage(new string[] { ComRes.ID.ID111090047 });          //計
            repairsResults[RowNo.Zero].Cause = GetResMessage(new string[] { ComRes.ID.ID111190023 });              //TBM故障
            repairsResults[RowNo.Three].Cause = GetResMessage(new string[] { ComRes.ID.ID111120099 });             //CBM想定外故障
            repairsResults[RowNo.Six].Cause = GetResMessage(new string[] { ComRes.ID.ID111090047 });               //計
            repairsResults[RowNo.Nine].Cause = GetResMessage(new string[] { ComRes.ID.ID111120099 });              //CBM想定外故障
            repairsResults[RowNo.Twelve].Cause = GetResMessage(new string[] { ComRes.ID.ID111270019 });            //BDM故障
            repairsResults[RowNo.Fifteen].Cause = GetResMessage(new string[] { ComRes.ID.ID111090047 });           //計
            repairsResults[RowNo.TwentyOne].Cause = GetResMessage(new string[] { ComRes.ID.ID111040010 });         //(a+b+c)

            // 凡例
            repairsResults[RowNo.Six].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111040011 });          //a
            repairsResults[RowNo.Fifteen].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111270022 });      //b
            repairsResults[RowNo.Eighteen].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111120111 });     //c
            repairsResults[RowNo.TwentyOne].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111190026 });    //d

            // 期 行分ループ
            for (int i = 0; i < RowNo.TwentyFour; i++)
            {
                if (i == 0 || i % 3 == 0)
                {
                    repairsResults[i].Period = GetResMessage(new string[] { ComRes.ID.ID111200010 });              //当月
                }
                else if (i == 1 || (i - 1) % 3 == 0)
                {
                    repairsResults[i].Period = GetResMessage(new string[] { ComRes.ID.ID111260023 });              //半期
                }
                else
                {
                    repairsResults[i].Period = GetResMessage(new string[] { ComRes.ID.ID111240001 });              //年度
                }
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(info, repairsResults, repairsResults.Count(), true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 詳細(作業計画性分類1)　取得処理
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="info">ページ情報</param>
        /// <param name="isExist">データが存在するか</param>
        /// <returns>正常：True、異常：False</returns>
        private bool GetWorkPlanList1(Dao.searchCondition conditionObj, CommonWebTemplate.CommonDefinitions.PageInfo info, out bool isExist)
        {
            isExist = false;

            // 一覧検索実行
            var workPlanList1Results = getSummaryInfo(conditionObj, SqlName.GetWorkPlan1Detail, string.Empty);
            if (workPlanList1Results == null)
            {
                return false;
            }

            // データが存在するか調べる
            isExist = isExistData(workPlanList1Results);

            // 項目
            workPlanList1Results[RowNo.Zero].Item = GetResMessage(new string[] { ComRes.ID.ID111090050 });           //計画作業件数
            workPlanList1Results[RowNo.Nine].Item = GetResMessage(new string[] { ComRes.ID.ID111090051 });           //計画外作業件数
            workPlanList1Results[RowNo.Eighteen].Item = GetResMessage(new string[] { ComRes.ID.ID111110010 });       //作業総件数
            workPlanList1Results[RowNo.Zero].Cause = GetResMessage(new string[] { ComRes.ID.ID111380036 });          //予防保全作業件数
            workPlanList1Results[RowNo.Three].Cause = GetResMessage(new string[] { ComRes.ID.ID111150005 });         //その他計画作業件数
            workPlanList1Results[RowNo.Six].Cause = GetResMessage(new string[] { ComRes.ID.ID111120101 });           //小計
            workPlanList1Results[RowNo.Nine].Cause = GetResMessage(new string[] { ComRes.ID.ID111200012 });          //突発作業件数
            workPlanList1Results[RowNo.Twelve].Cause = GetResMessage(new string[] { ComRes.ID.ID111150006 });        //その他計画外作業件数
            workPlanList1Results[RowNo.Fifteen].Cause = GetResMessage(new string[] { ComRes.ID.ID111120101 });       //小計
            workPlanList1Results[RowNo.Eighteen].Cause = GetResMessage(new string[] { ComRes.ID.ID111040012 });      //(f+h)

            // 凡例
            workPlanList1Results[RowNo.Zero].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111020037 });     //e
            workPlanList1Results[RowNo.Six].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111040013 });      //f
            workPlanList1Results[RowNo.Nine].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111120112 });     //g
            workPlanList1Results[RowNo.Fifteen].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111040014 });  //h
            workPlanList1Results[RowNo.Eighteen].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111010010 }); //i

            // 期 行分ループ
            for (int i = 0; i < RowNo.TwentyOne; i++)
            {
                if (i == 0 || i % 3 == 0)
                {
                    workPlanList1Results[i].Period = GetResMessage(new string[] { ComRes.ID.ID111200010 });          //当月
                }
                else if (i == 1 || (i - 1) % 3 == 0)
                {
                    workPlanList1Results[i].Period = GetResMessage(new string[] { ComRes.ID.ID111260023 });          //半期
                }
                else
                {
                    workPlanList1Results[i].Period = GetResMessage(new string[] { ComRes.ID.ID111240001 });          //年度
                }
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(info, workPlanList1Results, workPlanList1Results.Count(), true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 詳細(作業計画性分類2)　取得処理
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="info">ページ情報</param>
        /// <returns>正常：True、異常：False</returns>
        private bool GetWorkPlanList2(Dao.searchCondition conditionObj, CommonWebTemplate.CommonDefinitions.PageInfo info)
        {
            // 一覧検索実行
            var workPlanList2Results = getSummaryInfo(conditionObj, SqlName.GetWorkPlan2Detail, string.Empty);
            if (workPlanList2Results == null)
            {
                return false;
            }

            // 項目
            workPlanList2Results[RowNo.Zero].Item = GetResMessage(new string[] { ComRes.ID.ID111090054 });       //計画作業率
            workPlanList2Results[RowNo.Three].Item = GetResMessage(new string[] { ComRes.ID.ID111380037 });      //予防保全作業率
            workPlanList2Results[RowNo.Six].Item = GetResMessage(new string[] { ComRes.ID.ID111200014 });        //突発作業率
            workPlanList2Results[RowNo.Zero].Cause = GetResMessage(new string[] { ComRes.ID.ID111040015 });      //[= f / i × 100]
            workPlanList2Results[RowNo.Three].Cause = "[= e' / I × 100]";                                       //固定文字列
            workPlanList2Results[RowNo.Six].Cause = GetResMessage(new string[] { ComRes.ID.ID111120113 });       //[= g / I × 100]

            //期 行分ループ
            for (int i = 0; i < RowNo.Nine; i++)
            {
                if (i == 0 || i % 3 == 0)
                {
                    workPlanList2Results[i].Period = GetResMessage(new string[] { ComRes.ID.ID111200010 });      //当月
                }
                else if (i == 1 || (i - 1) % 3 == 0)
                {
                    workPlanList2Results[i].Period = GetResMessage(new string[] { ComRes.ID.ID111260023 });      //半期
                }
                else
                {
                    workPlanList2Results[i].Period = GetResMessage(new string[] { ComRes.ID.ID111240001 });      //年度
                }
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(info, workPlanList2Results, workPlanList2Results.Count(), true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 詳細(性格作業分類)　取得処理
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="info">ページ情報</param>
        /// <param name="isExist">データが存在するか</param>
        /// <returns>正常：True、異常：False</returns>
        private bool GetPersonalityList(Dao.searchCondition conditionObj, CommonWebTemplate.CommonDefinitions.PageInfo info, out bool isExist)
        {
            isExist = false;

            // 一覧検索実行
            var personalityListResults = getSummaryInfo(conditionObj, SqlName.GetWorkPersonalityDetail, string.Empty);
            if (personalityListResults == null)
            {
                return false;
            }

            // データが存在するか調べる
            isExist = isExistData(personalityListResults);

            // 項目
            personalityListResults[RowNo.Zero].Item = GetResMessage(new string[] { ComRes.ID.ID111380038 });         //予防保全
            personalityListResults[RowNo.Three].Item = GetResMessage(new string[] { ComRes.ID.ID111120102 });        //事後保全
            personalityListResults[RowNo.Nine].Item = GetResMessage(new string[] { ComRes.ID.ID111140040 });         //製造関連
            personalityListResults[RowNo.Twelve].Item = GetResMessage(new string[] { ComRes.ID.ID111110010 });       //作業総件数
            personalityListResults[RowNo.Zero].Cause = GetResMessage(new string[] { ComRes.ID.ID111380036 });        //予防保全作業件数
            personalityListResults[RowNo.Three].Cause = GetResMessage(new string[] { ComRes.ID.ID111100056 });       //故障修理件数
            personalityListResults[RowNo.Six].Cause = GetResMessage(new string[] { ComRes.ID.ID111150007 });         //その他作業件数
            personalityListResults[RowNo.Nine].Cause = GetResMessage(new string[] { ComRes.ID.ID111140041 });        //製造関連作業件数
            personalityListResults[RowNo.Twelve].Cause = GetResMessage(new string[] { ComRes.ID.ID111040012 });      //(f+h)

            // 凡例
            personalityListResults[RowNo.Zero].UsageGuide = "e'";       //e'
            personalityListResults[RowNo.Three].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111190026 });  //d
            personalityListResults[RowNo.Twelve].UsageGuide = GetResMessage(new string[] { ComRes.ID.ID111120114 }); //j

            // 期 行分ループ
            for (int i = 0; i < RowNo.Fifteen; i++)
            {
                if (i == 0 || i % 3 == 0)
                {
                    personalityListResults[i].Period = GetResMessage(new string[] { ComRes.ID.ID111200010 });        //当月
                }
                else if (i == 1 || (i - 1) % 3 == 0)
                {
                    personalityListResults[i].Period = GetResMessage(new string[] { ComRes.ID.ID111260023 });        //半期
                }
                else
                {
                    personalityListResults[i].Period = GetResMessage(new string[] { ComRes.ID.ID111240001 });        //年度
                }
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(info, personalityListResults, personalityListResults.Count(), true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 詳細(性格作業分類)　取得処理
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="info">ページ情報</param>
        /// <param name="locationIdList">場所階層ID</param>
        /// <param name="isExist">データが存在するか</param>
        /// <returns>正常：True、異常：False</returns>
        private bool GetOthersList(Dao.searchCondition conditionObj, CommonWebTemplate.CommonDefinitions.PageInfo info, List<int> locationIdList, out bool isExist)
        {
            isExist = false;

            // 検索結果格納List定義
            List<Dao.searchResult> resultsList = new();

            // 計算用
            int[] thisTotal = new int[4];
            int[] halfTotal = new int[4];
            int[] totalTotal = new int[4];

            // 選択された構成リストから配下の構成IDを全て取得
            locationIdList = GetLowerStructureIdList(locationIdList);

            // IDのリストより上位の階層を検索し、階層情報のリストを取得
            var param = new { StructureIdList = locationIdList, LanguageId = this.LanguageId };
            var structureInfoList = SqlExecuteClass.SelectList<Dao.StructureGetInfo>(SqlName.GetStructureIdByexdata, SqlName.SubDir, param, db);
            if (structureInfoList == null)
            {
                return false; // 取得出来なければエラー
            }

            // 取得した構成IDと拡張データを条件に追加
            foreach (var data in structureInfoList)
            {
                conditionObj.StructureId = data.StructureId;     // 構成ID
                conditionObj.ExtensionData = data.extensionData; // 拡張データ(保全履歴個別工場フラグ)

                // 0の場合は一般工場
                if (data.extensionData == (int)MaintenanceHistoryFlg.General)
                {
                    resultsList = getSummaryInfo(conditionObj, SqlName.GetOthersDetailFlg0, string.Empty);
                }
                // 1の場合は個別工場
                else
                {
                    resultsList = getSummaryInfo(conditionObj, SqlName.GetOthersDetailFlg1, string.Empty);
                }

                //データが取得できなければエラー
                if (resultsList.Count() == 0)
                {
                    return false;
                }
                else
                {
                    // 期間毎に集計
                    calculationResults(ref thisTotal, ref halfTotal, ref totalTotal, resultsList);
                }
            }
            // 当月
            resultsList[RowNo.Zero].Machine = string.Format("{0:#,0}", thisTotal[0]);         // カンマ区切りの数値に変換
            resultsList[RowNo.Zero].Electricity = string.Format("{0:#,0}", thisTotal[1]);     // カンマ区切りの数値に変換
            resultsList[RowNo.Zero].Instrumentation = string.Format("{0:#,0}", thisTotal[2]); // カンマ区切りの数値に変換
            resultsList[RowNo.Zero].Other = string.Format("{0:#,0}", thisTotal[3]);           // カンマ区切りの数値に変換

            // 半期
            resultsList[RowNo.One].Machine = string.Format("{0:#,0}", halfTotal[0]);          // カンマ区切りの数値に変換
            resultsList[RowNo.One].Electricity = string.Format("{0:#,0}", halfTotal[1]);      // カンマ区切りの数値に変換
            resultsList[RowNo.One].Instrumentation = string.Format("{0:#,0}", halfTotal[2]);  // カンマ区切りの数値に変換
            resultsList[RowNo.One].Other = string.Format("{0:#,0}", halfTotal[3]);            // カンマ区切りの数値に変換

            // 累計
            resultsList[RowNo.Two].Machine = string.Format("{0:#,0}", totalTotal[0]);         // カンマ区切りの数値に変換
            resultsList[RowNo.Two].Electricity = string.Format("{0:#,0}", totalTotal[1]);     // カンマ区切りの数値に変換
            resultsList[RowNo.Two].Instrumentation = string.Format("{0:#,0}", totalTotal[2]); // カンマ区切りの数値に変換
            resultsList[RowNo.Two].Other = string.Format("{0:#,0}", totalTotal[3]);           // カンマ区切りの数値に変換

            // データが存在するか調べる
            isExist = isExistData(resultsList);

            // 呼び出し回数
            resultsList[RowNo.Zero].Item = GetResMessage(new string[] { ComRes.ID.ID111380030 });

            // 期
            resultsList[RowNo.Zero].Period = GetResMessage(new string[] { ComRes.ID.ID111200010 });   //当月
            resultsList[RowNo.One].Period = GetResMessage(new string[] { ComRes.ID.ID111260023 });    //半期
            resultsList[RowNo.Two].Period = GetResMessage(new string[] { ComRes.ID.ID111240001 });    //年度

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(info, resultsList, resultsList.Count(), true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 呼び出し回数を計算するメソッド
        /// </summary>
        /// <param name="thisTotal">職種ごとの当月の累計呼び出し回数</param>
        /// <param name="halfTotal">職種ごとの半期の累計呼び出し回数</param>
        /// <param name="totalTotal">職種ごとの累計の呼び出し回数</param>
        /// <param name="resultsList">詳細(その他)結果リスト</param>
        private void calculationResults(ref int[] thisTotal, ref int[] halfTotal, ref int[] totalTotal, List<Dao.searchResult> resultsList)
        {
            // 期間ごとに集計　当月
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.Zero].Machine))
            {
                thisTotal[0] += int.Parse(resultsList[RowNo.Zero].Machine.Replace(",", ""));                // 当月機械の呼び出し回数を追加
            }
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.Zero].Electricity))
            {
                thisTotal[1] += int.Parse(resultsList[RowNo.Zero].Electricity.Replace(",", ""));            // 当月電気の呼び出し回数を追加
            }
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.Zero].Instrumentation))
            {
                thisTotal[2] += int.Parse(resultsList[RowNo.Zero].Instrumentation.Replace(",", ""));        // 当月計装の呼び出し回数を追加
            }
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.Zero].Other))
            {
                thisTotal[3] += int.Parse(resultsList[RowNo.Zero].Other.Replace(",", ""));                  // 当月その他の呼び出し回数を追加
            }

            // 半期
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.One].Machine))
            {
                halfTotal[0] += int.Parse(resultsList[RowNo.One].Machine.Replace(",", ""));                 // 半期機械の呼び出し回数を追加
            }
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.One].Electricity))
            {
                halfTotal[1] += int.Parse(resultsList[RowNo.One].Electricity.Replace(",", ""));             // 半期電気の呼び出し回数を追加
            }
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.One].Instrumentation))
            {
                halfTotal[2] += int.Parse(resultsList[RowNo.One].Instrumentation.Replace(",", ""));         // 計装電気の呼び出し回数を追加
            }
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.One].Other))
            {
                halfTotal[3] += int.Parse(resultsList[RowNo.One].Other.Replace(",", ""));                   // 半期電気の呼び出し回数を追加
            }

            // 累計
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.Two].Machine))
            {
                totalTotal[0] += int.Parse(resultsList[RowNo.Two].Machine.Replace(",", ""));                 // 累計機械の呼び出し回数を追加
            }
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.Two].Electricity))
            {
                totalTotal[1] += int.Parse(resultsList[RowNo.Two].Electricity.Replace(",", ""));            // 累計電気の呼び出し回数を追加
            }
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.Two].Instrumentation))
            {
                totalTotal[2] += int.Parse(resultsList[RowNo.Two].Instrumentation.Replace(",", ""));        // 累計計装の呼び出し回数を追加
            }
            if (!string.IsNullOrWhiteSpace(resultsList[RowNo.Two].Other))
            {
                totalTotal[3] += int.Parse(resultsList[RowNo.Two].Other.Replace(",", ""));                  // 累計その他の呼び出し回数を追加
            }
        }

        /// <summary>
        /// データが存在するか調べる
        /// </summary>
        /// <param name="resultsList">検索結果リスト</param>
        /// <returns>データが存在する：True　存在しない：False</returns>
        private bool isExistData(List<Dao.searchResult> resultsList)
        {
            foreach (var data in resultsList)
            {
                // 機械、電気、計装、その他の集計値が1行でも0以上であれば存在するとみなす
                if (data.Machine == "" || data.Machine == null)
                {
                    data.Machine = "0"; // 機械
                }
                if (data.Electricity == "" || data.Electricity == null)
                {
                    data.Electricity = "0"; // 電気
                }
                if (data.Instrumentation == "" || data.Instrumentation == null)
                {
                    data.Instrumentation = "0"; // 計装
                }
                if (data.Other == "" || data.Other == null)
                {
                    data.Other = "0"; // その他
                }
                int total = int.Parse(data.Machine.Replace(",", "")) + int.Parse(data.Electricity.Replace(",", "")) + int.Parse(data.Instrumentation.Replace(",", "")) + int.Parse(data.Other.Replace(",", ""));
                if (total != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// データ件数をマイナス1する
        /// </summary>
        /// <param name="isExistFlg">データが存在するか</param>
        /// <param name="dataCnt">データ件数</param>
        /// <returns>残データ件数</returns>
        private int reduceCnt(bool isExistFlg, ref int dataCnt)
        {
            if (!isExistFlg)
            {
                return dataCnt--; //データが無ければマイナス1
            }
            return dataCnt;
        }

        /// <summary>
        /// 期の開始月取得
        /// </summary>
        /// <param name="beginningMonth">期首月</param>
        /// <param name="term">期の期間</param>
        /// <param name="months">対象指定月</param>
        /// <returns>期の開始月</returns>
        private int GetStartMonth(int beginningMonth, int term, int months)
        {
            /*
            * 12か月を期首月を先頭に並び変え、
            * 期の期間毎に分けて対象指定年月が含まれる期間の開始月を返す処理です。
            * 例)期首月：4、期の期間：3、対象指定年月：6の場合
            * {4,5,6} {7,8,9} {10,11,12} {1,2,3}より期の開始月は4。
            */

            int[] monthArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}; //12か月 基本の並び順
            int[] sortMonthArray = new int[12];

            int loopCnt = 0;   // ループ回数
            int reloopCnt = 0; // 12を超えた時のループ回数(先頭に戻る用)

            // 期首月を先頭に並び替え
            while (loopCnt < 12)
            {
                if (beginningMonth + loopCnt <= 12)
                {
                    // 12までは昇順で配列に数字を追加
                    sortMonthArray[loopCnt] = beginningMonth + loopCnt;
                }
                else
                {
                    // 1から順に数字を追加
                    sortMonthArray[loopCnt] = monthArray[reloopCnt];
                    reloopCnt++;
                }
                loopCnt++;
            }
            // カウンター初期化
            loopCnt = 0;

            // ループ回数(切り上げ)
            int ternLoopCnt = (12 + term - 1) / term;

            // 12の約数
            int[] divisor = {1, 2, 3, 4, 6, 12};

            // 配列番号
            int arrayNo = 0;

            // 期の期間で分ける
            while (loopCnt < ternLoopCnt)
            {
                // コピー先
                int[] finedArray = new int[12];
                // 型のサイズを取得する
                int typeSize = System.Runtime.InteropServices.Marshal.SizeOf(sortMonthArray.GetType().GetElementType());

                // 範囲外でコピーしないように制御
                if (divisor.Contains(term) || (loopCnt != ternLoopCnt - 1))
                {
                    // 指定された範囲をコピーする
                    Buffer.BlockCopy(sortMonthArray, arrayNo * typeSize, finedArray, 0, term * typeSize);
                }
                else
                {
                    // 期の期間が12の約数以外かつ最後のループの場合残りをコピー
                    Buffer.BlockCopy(sortMonthArray, arrayNo * typeSize, finedArray, 0, (12 - arrayNo) * typeSize);
                }

                //コピーした配列内に指定対象月があればその配列の先頭の数字を返す
                if (finedArray.Contains(months))
                {
                    return finedArray[0];
                }

                //指定範囲をずらす
                arrayNo = arrayNo + term;

                loopCnt++;
            }

            //期の開始月が取得できなかった場合は0を返す(エラー)
            return 0;
        }
        #endregion
    }
}