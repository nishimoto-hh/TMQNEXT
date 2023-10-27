using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonExcelUtil;
using Microsoft.AspNetCore.Http;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ExcelUtil = CommonExcelUtil.CommonExcelUtil;
using TMQDataClass = CommonTMQUtil.TMQCommonDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using Dao = CommonTMQUtil.CommonTMQUtilDataClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using STDDao = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using System.Dynamic;
using ComBase = CommonSTDUtil.CommonDataBaseClass;
using InputDao = CommonSTDUtil.CommonSTDUtil.CommonInputReportDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using System.Text.RegularExpressions;

// 一つのファイルに書くと長くなって対象の処理を探すのが大変になりそうなので分割テスト(partial)
// 将来的には適当な処理単位で分割したい。その際はファイル名も相応しい内容に変更
namespace CommonTMQUtil
{
    /// <summary>
    /// TMQ用共通ユーティリティクラス
    /// </summary>
    public static partial class CommonTMQUtil
    {
        #region 定数
        /// <summary>SQL格納先サブディレクトリ名</summary>
        public const string ExcelPath = "Excel";
        #endregion

        #region エクセル入出力共通処理
        /// <summary>
        /// エクセル入出力
        /// </summary>
        public class ComReport
        {
            /// <summary>入出力方式：単一セル</summary>
            public const int SingleCell = 1;
            /// <summary>入出力方式：縦方向連続</summary>
            public const int LongitudinalDirection = 2;
            /// <summary>入出力方式：横方向連続</summary>
            public const int LateralDirection = 3;
            /// <summary>入出力方式：縦横方向連続</summary>
            public const int LongitudinalLateralDirection = 4;

            /// <summary>出力位置定義：場所階層構成リスト 出力列</summary>
            public const int ConditionSheetLocationRow = 2;
            /// <summary>出力位置定義：職種機種構成リスト 出力列</summary>
            public const int ConditionSheetJobRow = 3;
            /// <summary>出力位置定義：検索条件 出力開始列</summary>
            public const int ConditionSheetStartRow = 6;

            ///// <summary>セルタイプ：文字列</summary>
            //public const int InputTypeStr = 1;
            ///// <summary>セルタイプ：数値</summary>
            //public const int InputTypeNum = 2;
            ///// <summary>セルタイプ：日付</summary>
            //public const int InputTypeDat = 3;
            // コントロールマスタのデータタイプ
            /// <summary>データタイプ：文字列</summary>
            public const int DataTypeStr = 0;
            /// <summary>データタイプ：整数</summary>
            public const int DataTypeInt = 1;
            /// <summary>データタイプ：実数</summary>
            public const int DataTypeNum = 2;
            /// <summary>データタイプ：日付</summary>
            public const int DataTypeDat = 3;

            /// <summary>出力項目種別：出力項目固定</summary>
            public const int OutputItemType1 = 1;
            /// <summary>出力項目種別：出力項目固定（出力パターン指定なし）</summary>
            public const int OutputItemType2 = 2;
            /// <summary>出力項目種別：出力項目固定（出力パターン指定あり※ベタ票）</summary>
            public const int OutputItemType3 = 3;

            /// <summary>０落ち対応 フォーマット</summary>
            public const string StrFormat = "@";
            /// <summary>SQL名：出力帳票シート定義取得用SQL</summary>
            public const string GetReportSheetDefine = "GetReportSheetDefine";
            /// <summary>SQL名：ファイル入出力項目定義情報取得用SQL</summary>
            public const string GetComReportInfo = "ComInoutItemDefine_GetInfo";
            /// <summary>SQL名：テンポラリテーブル作成用SQL</summary>
            public const string CreateTableTemp = "CreateTableTemp";
            /// <summary>SQL名：テンポラリテーブル登録用SQL</summary>
            public const string InsertTemp = "InsertTemp";
            /// <summary>SQL名：最大行取得用SQL</summary>
            public const string GetMaxRowNo = "GetMaxRowNo";
            /// <summary>SQL名：最小列取得用SQL</summary>
            public const string GetMinColumnNo = "GetMinColumnNo";
            /// <summary>SQL名：最大列取得用SQL</summary>
            public const string GetMaxColumnNo = "GetMaxColumnNo";

            /// <summary>SQL名：オプション用SQL</summary>
            public const string Option = "_Option";

            /// <summary>出力方式：件名別</summary>
            public const int OutputMode1 = 1;
            /// <summary>出力方式：機番別</summary>
            public const int OutputMode2 = 2;
            /// <summary>出力方式：予算別</summary>
            public const int OutputMode3 = 3;

            /// <summary>階層ネスト最大数</summary>
            public const int MaxLayerNest = 3;

            #region 取込処理用
            /// <summary>SQL名：ファイル取込項目定義情報取得用SQL</summary>
            public const string GetInputControlDefine = "GetInputControlDefine";
            #endregion

       }

        /// <summary>
        /// カラム名
        /// </summary>
        public class ColumnName
        {
            /// <summary>職種機種ID</summary>
            public const string JobStructureId = "job_structure_id";
            /// <summary>職種ID</summary>
            public const string JobId = "job_id";
            /// <summary>職種名称</summary>
            public const string JobName = "job_name";
            /// <summary>機種大分類ID</summary>
            public const string LargeClassficationId = "large_classfication_id";
            /// <summary>機種大分類名称</summary>
            public const string LargeClassficationName = "large_classfication_name";
            /// <summary>機種中分類ID</summary>
            public const string MiddleClassficationId = "middle_classfication_id";
            /// <summary>機種中分類名称</summary>
            public const string MiddleClassficationName = "middle_classfication_name";
            /// <summary>機種小分類ID</summary>
            public const string SmallClassficationId = "small_classfication_id";
            /// <summary>機種小分類名称</summary>
            public const string SmallClassficationName = "small_classfication_name";

            /// <summary>機種名称</summary>
            public const string ModelName = "model_name";

            /// <summary>場所階層ID</summary>
            public const string LocationStructureId = "location_structure_id";
            /// <summary>地区ID</summary>
            public const string DistrictId = "district_id";
            /// <summary>地区名称</summary>
            public const string DistrictName = "district_name";
            /// <summary>工場ID</summary>
            public const string FactoryId = "factory_id";
            /// <summary>工場名称</summary>
            public const string FactoryName = "factory_name";
            /// <summary>プラントID</summary>
            public const string PlantId = "plant_id";
            /// <summary>プラント名称</summary>
            public const string PlantName = "plant_name";
            /// <summary>系列ID</summary>
            public const string SeriesId = "series_id";
            /// <summary>系列名称</summary>
            public const string SeriesName = "series_name";
            /// <summary>工程ID</summary>
            public const string StrokeId = "stroke_id";
            /// <summary>工程名称</summary>
            public const string StrokeName = "stroke_name";
            /// <summary>設備ID</summary>
            public const string FacilityId = "facility_id";
            /// <summary>設備名称</summary>
            public const string FacilityName = "facility_name";


            /// <summary>共通用タイトル</summary>
            public const string ComTitle = "com_title";
            /// <summary>共通用日付</summary>
            public const string ComDate = "com_date";
            /// <summary>共通用時刻</summary>
            public const string ComTime = "com_time";

            /// <summary>共通用シートタイトル</summary>
            public const string ComSheetTitle = "com_sheet_title";
            /// <summary>共通用バージョン</summary>
            public const string ComVersion = "com_version";
            /// <summary>共通用日時</summary>
            public const string ComDateTime = "com_datetime";
            /// <summary>共通用機能ID</summary>
            public const string ComConductId = "com_conduct_id";
            /// <summary>共通用シート番号</summary>
            public const string ComSheetNo = "com_sheet_no";

            /// <summary>スケジュール</summary>
            public const string Schedule = "schedule";
            /// <summary>予算実績</summary>
            public const string BudgetResult = "budget_result";

            /// <summary>キー項</summary>
            public const string KeyId = "key_id";

            /// <summary>
            /// 共通用カラムかどうか
            /// </summary>
            /// <param name="colName">カラム名</param>
            /// <returns>true：共通用カラム/false：その他のカラム</returns>
            public static bool IsCommonColumn(string colName)
            {
                string[] comNames = new string[] { 
                    ComTitle, ComDate, ComTitle, ComSheetTitle, ComVersion, ComDateTime, ComConductId, ComSheetNo 
                };
                return comNames.Contains(colName);
            }
        }

        /// <summary>
        /// 項目名
        /// </summary>
        public class ItemName
        {
            /// <summary>場所階層構成IDリスト</summary>
            public const string LocationIdList = "LocationIdList";
            /// <summary>職種機種構成IDリスト</summary>
            public const string JobIdList = "JobIdList";
            /// <summary>詳細検索条件</summary>
            public const string detailSearchList = "detailSearchList";
        }
        /// <summary>
        /// 職種階層の情報を保持するデータクラス（帳票用）
        /// </summary>
        /// <remarks>このクラスを継承するか、同名のメンバを持つクラスが利用可能</remarks>
        public class StructureJobInfoForReport
        {
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int JobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? LargeClassficationId { get; set; }
            /// <summary>Gets or sets 機種大分類名称</summary>
            /// <value>機種大分類名称</value>
            public string LargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? MiddleClassficationId { get; set; }
            /// <summary>Gets or sets 機種中分類名称</summary>
            /// <value>機種中分類名称</value>
            public string MiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? SmallClassficationId { get; set; }
            /// <summary>Gets or sets 機種小分類名称</summary>
            /// <value>機種小分類名称</value>
            public string SmallClassficationName { get; set; }
        }

        /// <summary>
        /// 場所階層の情報を保持するデータクラス（帳票用）
        /// </summary>
        /// <remarks>このクラスを継承するか、同名のメンバを持つクラスが利用可能</remarks>
        public class StructureLocationInfoForReport
        {
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string FacilityName { get; set; }
        }

        /// <summary>
        /// オプション
        /// </summary>
        public class Option
        {
            /// <summary>表示単位</summary>
            public int DisplayUnit { get; set; }
            /// <summary>開始日</summary>
            public DateTime StartDate { get; set; }
            /// <summary>終了日</summary>
            public DateTime EndDate { get; set; }
            /// <summary>出力方式</summary>
            public int OutputMode { get; set; }
            /// <summary>年度開始月</summary>
            public int MonthStartNendo { get; set; }
            /// <summary>検索条件クラス</summary>
            public TMQDao.ScheduleList.GetCondition Condition { get; set; }
            /// <summary>予算開始日</summary>
            public DateTime? BudgetStartDate { get; set; }
            /// <summary>予算終了日</summary>
            public DateTime? BudgetEndDate { get; set; }
            /// <summary>バージョン</summary>
            public decimal Version { get; set; }
            /// <summary>対象機能ID</summary>
            public string TargetConductId { get; set; }
            /// <summary>対象シート番号</summary>
            public int TargetSheetNo { get; set; }
        }

        /// <summary>
        /// 会計帳票出力条件のデータクラス
        /// </summary>
        public class CondAccountReport
        {
            /// <summary>Gets or sets 対象年月</summary>
            /// <value>対象年月</value>
            public string TargetYearMonth { get; set; }
            /// <summary>Gets or sets 工場</summary>
            /// <value>工場</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 職種</summary>
            /// <value>職種</value>
            public int? JobId { get; set; }
            /// <summary>Gets or sets 予備品No</summary>
            /// <value>予備品No</value>
            public string PartsNo { get; set; }
            /// <summary>Gets or sets 予備品名</summary>
            /// <value>予備品名</value>
            public string PartsName { get; set; }
            /// <summary>Gets or sets 型式</summary>
            /// <value>型式</value>
            public string ModelType { get; set; }
            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string StandardSize { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public int? ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets 使用場所</summary>
            /// <value>使用場所</value>
            public string PartsServiceSpace { get; set; }
            /// <summary>Gets or sets 仕入先</summary>
            /// <value>仕入先</value>
            public int? VenderStructureId { get; set; }
            /// <summary>Gets or sets 予備品倉庫</summary>
            /// <value>予備品倉庫</value>
            public int? StorageLocationId { get; set; }
            /// <summary>Gets or sets 棚番</summary>
            /// <value>棚番</value>
            public int? PartsLocationId { get; set; }
            /// <summary>Gets or sets 入出庫日From</summary>
            /// <value>入出庫日From</value>
            public DateTime? WorkDivisionDateFrom { get; set; }
            /// <summary>Gets or sets 入出庫日To</summary>
            /// <value>入出庫日To</value>
            public DateTime? WorkDivisionDateTo { get; set; }
            /// <summary>Gets or sets 入出庫No</summary>
            /// <value>入出庫No</value>
            public int? WorkDivisionNo { get; set; }
            /// <summary>Gets or sets 新旧区分</summary>
            /// <value>新旧区分</value>
            public int? OldNewStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目</summary>
            /// <value>勘定科目</value>
            public int? AccountStructureId { get; set; }
            /// <summary>Gets or sets 部門</summary>
            /// <value>部門</value>
            public List<int> DepartmentIdList { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 管理No</summary>
            /// <value>管理No</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 帳票名</summary>
            /// <value>帳票名</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets SQLの中でコメントアウトを解除したい箇所のリスト</summary>
            /// <value>SQLの中でコメントアウトを解除したい箇所のリスト</value>
            public List<string> ListUnComment { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(金額)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int CurrencyDigit { get; set; }
            /// <summary>Gets or sets 丸め処理区分(金額)</summary>
            /// <value>丸め処理区分(金額)</value>
            public int CurrencyRoundDivision { get; set; }
            /// <summary>Gets or sets 現在のページ</summary>
            /// <value>現在のページ</value>
            public int CurrentPage { get; set; }
            /// <summary>Gets or sets 最終ページ</summary>
            /// <value>最終ページ</value>
            public int LastPage { get; set; }
            /// <summary>Gets or sets 合計</summary>
            /// <value>合計</value>
            public decimal Total { get; set; }
            /// <summary>Gets or sets 一覧明細部の最大件数</summary>
            /// <value>一覧明細部の最大件数</value>
            public int ListMaxCnt { get; set; }
        }

        #region RP0090長期スケジュール表用クラス
        /// <summary>
        /// RP0090長期スケジュール表用クラス
        /// </summary>
        public class ReportRP0090
        {
            /// <summary>帳票ID</summary>
            public const string ReportId = "RP0090";
        }
        #endregion

        #region RP0100年度スケジュール表用クラス
        /// <summary>
        /// RP0100年度スケジュール表用クラス
        /// </summary>
        public class ReportRP0100
        {
            /// <summary>帳票ID</summary>
            public const string ReportId = "RP0100";
        }
        #endregion

        #region RP0270会計提出用クラス
        /// <summary>
        /// RP0270会計提出用クラス
        /// </summary>
        public class ReportRP0270
        {
            /// <summary>帳票ID</summary>
            public const string ReportId = "RP0270";
            /// <summary>帳票シート定義基本シートNo</summary>
            public const int BaseSheetNo = 1;
            /// <summary>テンプレートシート名_開始</summary>
            public const string BaseTemplateSheetNameS = "会計提出表_S";
            /// <summary>テンプレートシート名_中間</summary>
            public const string BaseTemplateSheetNameM = "会計提出表_M";
            /// <summary>明細最大件数</summary>
            public const int ListMaxCnt = 50;
            /// <summary>シート名最大文字数</summary>
            public const int MaxSheetNameLength = 31;
            /// <summary>SQLファイルコメントデータ件数用</summary>
            public const string SqlUnCommentForDataCnt = "GetCntEveryAccountAndDepartment";
            /// <summary>SQLファイルコメントデータ用</summary>
            public const string SqlUnCommentForDataSelect = "GetEveryAccountAndDepartment";
        }
        #endregion エクセル入出力共通処理

        #region RP0410棚卸準備リスト用クラス
        /// <summary>
        /// RP0410棚卸準備リスト用クラス
        /// </summary>
        public class ReportRP0410
        {
            /// <summary>帳票ID</summary>
            public const string ReportId = "RP0410";
            /// <summary>帳票ID</summary>
            public const string password = "";
        }
        #endregion

        #region エクセル入出力共通処理

        /// <summary>
        /// エクセル出力共通処理
        /// </summary>
        /// <typeparam name="T">データクラス</typeparam>
        /// <param name="factoryId">工場ID</param>
        /// <param name="programId">プログラムID</param>
        /// <param name="selectKeyDataList">選択データ</param>
        /// <param name="searchCondition">検索条件</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="outputPatternId">出力パターンID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="fileType">ファイルタイプ</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="memoryStream">メモリストリーム</param>
        /// <param name="message">メッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="option">オプションクラス</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool CommonOutputExcel(
            int factoryId,
            string programId,
            List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData> selectKeyDataList,
            dynamic searchCondition,
            string reportId,
            int templateId,
            int outputPatternId,
            string userId,
            string languageId,
            out string fileType,
            out string fileName,
            out MemoryStream memoryStream,
            out string message,
            ComDB db,
            Option option = null)
        {

            //==========
            // 初期化
            //==========
            // ファイルタイプ
            fileType = ComConsts.REPORT.FILETYPE.EXCEL;
            // ダウンロードファイル名
            fileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", reportId, DateTime.Now) + ComConsts.REPORT.EXTENSION.EXCEL_BOOK;
            // メモリストリーム
            memoryStream = new MemoryStream();
            // メッセージ
            message = "";
            // シートデータ件数
            int sheetDataCount = 0;

            // マッピング情報生成
            List<CommonExcelPrtInfo> mappingInfoList = new List<CommonExcelPrtInfo>();

            // コマンド情報生成
            // セルの結合や罫線を引く等のコマンド実行が必要な場合はここでセットする。不要な場合はnullでOK
            List<CommonExcelCmdInfo> cmdInfoList = new List<CommonExcelCmdInfo>();

            // オプションデータ用
            List<TMQDao.ScheduleList.Display> scheduleDisplayList = null;
            int optionRowCount = 0;
            int optionColomnCount = 0;

            // テンプレート情報を取得
            var template = new ReportDao.MsOutputTemplateEntity().GetEntity(factoryId, reportId, templateId, db);
            if (template == null)
            {
                // 取得できない場合、処理を戻す
                return false;
            }
            // テンプレートファイル名を設定
            string templateFileName = template.TemplateFileName;
            // テンプレートファイルパスを設定
            string templateFilePath = template.TemplateFilePath;

            // 出力帳票シート定義のリストを取得
            var sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                ComReport.GetReportSheetDefine,
                ExcelPath,
                new { FactoryId = factoryId, ReportId = reportId },
                db);
            if (sheetDefineList == null)
            {
                // 取得できない場合、処理を戻す
                return false;
            }

            // シート定義毎にループ
            foreach (var sheetDefine in sheetDefineList)
            {
                // 検索条件フラグの場合
                if (sheetDefine.SearchConditionFlg == true)
                {
                    // 検索条件からデータを取得し、エクセルを作成する
                    var dataList = GetReportDatabySearchCondition(searchCondition, db);
                    // マッピングデータ作成
                    List<CommonExcelPrtInfo> mappingDataList = TMQUtil.CreateMappingList(
                                                                            factoryId,
                                                                            programId,
                                                                            reportId,
                                                                            sheetDefine.SheetNo,
                                                                            templateId,
                                                                            outputPatternId,
                                                                            dataList,
                                                                            templateFileName,
                                                                            templateFilePath,
                                                                            languageId,
                                                                            db,
                                                                            out optionRowCount,
                                                                            out optionColomnCount);
                    // マッピング情報リストに追加
                    mappingInfoList.AddRange(mappingDataList);
                }
                else
                {

                    // オプション指定のある場合
                    if (option != null)
                    {
                        // 出力方式とシート番号が異なる場合はデータ作成しない
                        if (option.OutputMode != sheetDefine.SheetNo)
                        {
                            continue;
                        }
                    }
                    // 対象SQLファイルにてSQLを実行し、エクセルを作成する
                    string targetSql = sheetDefine.TargetSql;
                    var dataList = GetReportData(selectKeyDataList, targetSql, db, userId, languageId, option);

                    // 最大ネスト数分、ループ
                    for (int idx = 0; idx < ComReport.MaxLayerNest; idx++)
                    {
                        string addNum;

                        // ２周目以降は項目名末尾に数値を付与する
                        if (idx == 0)
                        {
                            addNum = "";
                        }
                        else
                        {
                            addNum = (idx + 1).ToString();
                        }

                        // 職種情報を設定する
                        foreach (var data in dataList)
                        {
                            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
                            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
                            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);

                            IList<StructureJobInfoForReport> jobInfoList = new List<StructureJobInfoForReport>();
                            StructureJobInfoForReport jobInfo = new StructureJobInfoForReport();

                            var rowDic = (IDictionary<string, object>)data;
                            var val = rowDic[ColumnName.JobStructureId + addNum];
                            if (val == null)
                            {
                                continue;
                            }
                            jobInfo.JobStructureId = Convert.ToInt32(val);
                            jobInfoList.Add(jobInfo);
                            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureJobInfoForReport>(ref jobInfoList, typeLst, db, languageId);
                            // 関連情報の設定
                            if (jobInfoList == null)
                            {
                                continue;
                            }
                            // 職種名称
                            rowDic[ColumnName.JobName + addNum] = jobInfoList[0].JobName;
                            // 機種大分類名称
                            rowDic[ColumnName.LargeClassficationName + addNum] = jobInfoList[0].LargeClassficationName;
                            // 機種中分類名称
                            rowDic[ColumnName.MiddleClassficationName + addNum] = jobInfoList[0].MiddleClassficationName;
                            // 機種小分類名称
                            rowDic[ColumnName.SmallClassficationName + addNum] = jobInfoList[0].SmallClassficationName;

                            if (idx == 0)
                            {
                                // 機種名称設定(RP0060)
                                if (!string.IsNullOrEmpty(jobInfoList[0].LargeClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].LargeClassficationName;
                                }
                                if (!string.IsNullOrEmpty(jobInfoList[0].MiddleClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].MiddleClassficationName;
                                }
                                if (!string.IsNullOrEmpty(jobInfoList[0].SmallClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].SmallClassficationName;
                                }
                            }

                        }
                        // 機能場所階層情報を設定する
                        foreach (var data in dataList)
                        {
                            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
                            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
                            IList<StructureLocationInfoForReport> locationInfoList = new List<StructureLocationInfoForReport>();
                            StructureLocationInfoForReport locationInfo = new StructureLocationInfoForReport();

                            var rowDic = (IDictionary<string, object>)data;
                            var val = rowDic["location_structure_id" + addNum];
                            if (val == null)
                            {
                                continue;
                            }
                            locationInfo.LocationStructureId = Convert.ToInt32(val);
                            locationInfoList.Add(locationInfo);
                            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureLocationInfoForReport>(ref locationInfoList, typeLst, db, languageId);
                            // 関連情報の設定
                            if (locationInfoList == null)
                            {
                                continue;
                            }

                            // 地区
                            rowDic[ColumnName.DistrictName + addNum] = locationInfoList[0].DistrictName;
                            // 工場
                            rowDic[ColumnName.FactoryName + addNum] = locationInfoList[0].FactoryName;
                            // プラント
                            rowDic[ColumnName.PlantName + addNum] = locationInfoList[0].PlantName;
                            // 系列
                            rowDic[ColumnName.SeriesName + addNum] = locationInfoList[0].SeriesName;
                            // 工程
                            rowDic[ColumnName.StrokeName + addNum] = locationInfoList[0].StrokeName;
                            // 設備
                            rowDic[ColumnName.FacilityName + addNum] = locationInfoList[0].FacilityName;

                        }
                    }

                    //                    typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);

                    // オプション指定のある場合
                    if (option != null)
                    {
                        string targetSqlSchedule = sheetDefine.TargetSql + ComReport.Option;
                        TMQUtil.GetFixedSqlStatement(ExcelPath, targetSqlSchedule, out string selectSql);

                        scheduleDisplayList = getScheduleDisplayList<TMQUtil.ScheduleListConverter>(selectSql, option.Condition);

                        // 画面表示データの取得 SQLを実行し、実行結果を変換 変換するクラスが上位ランクの有無が異なるので分岐
                        List<TMQDao.ScheduleList.Display> getScheduleDisplayList<T>(
                                string execSql,
                                Dao.ScheduleList.GetCondition cond)
                            where T : TMQUtil.ScheduleListConverter, new()
                        {
                            IList<TMQDao.ScheduleList.Get> scheduleList = db.GetListByDataClass<TMQDao.ScheduleList.Get>(execSql.ToString(), cond);
                            // 画面表示用に変換
                            T listSchedule = new();
                            // リンク有無
                            bool isLink = false; // リンクは詳細画面のみ
                            var scheduleDisplayList = listSchedule.Execute(scheduleList, cond, option.MonthStartNendo, isLink, db, null);
                            return scheduleDisplayList;
                        }
                    }
                    // マッピングデータ作成
                    List<CommonExcelPrtInfo> mappingDataList = TMQUtil.CreateMappingList(
                                                                            factoryId,
                                                                            programId,
                                                                            reportId,
                                                                            sheetDefine.SheetNo,
                                                                            templateId,
                                                                            outputPatternId,
                                                                            dataList,
                                                                            templateFileName,
                                                                            templateFilePath,
                                                                            languageId,
                                                                            db,
                                                                            out optionRowCount,
                                                                            out optionColomnCount,
                                                                            option,
                                                                            scheduleDisplayList);
                    // マッピング情報リストに追加
                    mappingInfoList.AddRange(mappingDataList);
                    // 一覧罫線用にデータ件数を退避
                    sheetDataCount = dataList.Count;

                }
                // 一覧フラグの場合
                if (sheetDefine.ListFlg == true)
                {
                    // 罫線設定セル範囲を取得
                    string range = GetTargetCellRange(factoryId, reportId, templateId, outputPatternId, sheetDefine.SheetNo, sheetDataCount, sheetDefine.RecordCount, db, optionRowCount, optionColomnCount);
                    if (range != null)
                    {
                        string sheetName = GetSheetName(factoryId, reportId, sheetDefine.SheetNo, languageId, db);
                        if (!string.IsNullOrEmpty(sheetName))
                        {
                            //範囲が取得できた場合、罫線を引く
                            cmdInfoList.AddRange(CommandLineBox(range, GetSheetName(factoryId, reportId, sheetDefine.SheetNo, languageId, db)));
                        }

                    }
                }
                // オプション指定のある場合
                if (option != null)
                {
                    // 不要なシートを削除
                    int targetSheetNo = sheetDefine.SheetNo;
                    int deleteSheetNo1 = 0;
                    int deleteSheetNo2 = 0;
                    string deleteSheetNameNo1 = "";
                    string deleteSheetNameNo2 = "";
                    switch (sheetDefine.SheetNo)
                    {
                        case 1:
                            deleteSheetNo1 = 2;
                            deleteSheetNo2 = 3;
                            break;
                        case 2:
                            deleteSheetNo1 = 1;
                            deleteSheetNo2 = 3;
                            break;
                        case 3:
                            deleteSheetNo1 = 1;
                            deleteSheetNo2 = 2;
                            break;
                    }
                    // 削除用シート名を取得
                    deleteSheetNameNo1 = GetSheetName(factoryId, reportId, deleteSheetNo1, languageId, db);
                    deleteSheetNameNo2 = GetSheetName(factoryId, reportId, deleteSheetNo2, languageId, db);

                    // シート削除コマンドを追加
                    // コマンド情報
                    CommonExcelCmdInfo cmdInfo1, cmdInfo2;
                    // コマンドパラメータ
                    string[] param1, param2;
                    cmdInfo1 = new CommonExcelCmdInfo();
                    cmdInfo2 = new CommonExcelCmdInfo();
                    param1 = new string[1];
                    param2 = new string[1];
                    param1[0] = deleteSheetNameNo1;
                    param2[0] = deleteSheetNameNo2;
                    cmdInfo1.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdDeleteSheet, param1);
                    cmdInfo2.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdDeleteSheet, param2);
                    cmdInfoList.Add(cmdInfo1);
                    cmdInfoList.Add(cmdInfo2);
                }

            }
            // エクセルファイル作成
            ExcelUtil.CreateExcelFile(templateFileName, templateFilePath, userId, mappingInfoList, cmdInfoList, ref memoryStream, ref message);

            return true;
        }

        /// <summary>
        /// エクセル出力共通処理
        /// </summary>
        /// <typeparam name="T">データクラス</typeparam>
        /// <param name="factoryId">工場ID</param>
        /// <param name="programId">プログラムID</param>
        /// <param name="selectKeyDataList">選択データ</param>
        /// <param name="searchCondition">検索条件</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="outputPatternId">出力パターンID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="languageId">言語ID</param>
        /// /// <param name="conditionSheetLocatoinList">場所階層構成IDリスト</param>
        /// <param name="conditionSheetJobList">職種機種構成IDリスト</param>
        /// <param name="conditionSheetNameList">検索条件項目名リスト</param>
        /// <param name="conditionSheetValueList">検索条件設定値リスト</param>
        /// <param name="fileType">ファイルタイプ</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="memoryStream">メモリストリーム</param>
        /// <param name="message">メッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="option">オプションクラス</param>
        /// <param name="condAccountReport">会計帳票出力条件クラス</param>
        /// <param name="summaryDataList">集計帳票集計データ</param>
        /// <param name="dicFixedValueForOutput">固定値出力用データ</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool CommonOutputExcel(
        int factoryId,
        string programId,
        Dictionary<int, List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData>> dicSelectKeyDataList,
        dynamic searchCondition,
        string reportId,
        int templateId,
        int outputPatternId,
        string userId,
        string languageId,
        List<int> conditionSheetLocatoinList,
        List<int> conditionSheetJobList,
        List<string> conditionSheetNameList,
        List<string> conditionSheetValueList,
        out string fileType,
        out string fileName,
        out MemoryStream memoryStream,
        out string message,
        ComDB db,
        Option option = null,
        CondAccountReport condAccountReport = null,
        Dictionary<int, IList<dynamic>> dicSummaryDataList = null,
        Dictionary<string, string> dicFixedValueForOutput = null)
        {

            //==========
            // 初期化
            //==========
            // ファイルタイプ
            fileType = ComConsts.REPORT.FILETYPE.EXCEL;
            // ダウンロードファイル名
            fileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", reportId, DateTime.Now) + ComConsts.REPORT.EXTENSION.EXCEL_BOOK;
            // メモリストリーム
            memoryStream = new MemoryStream();
            // メッセージ
            message = "";
            // シートデータ件数
            int sheetDataCount = 0;

            // マッピング情報生成
            List<CommonExcelPrtInfo> mappingInfoList = new List<CommonExcelPrtInfo>();

            // コマンド情報生成
            // セルの結合や罫線を引く等のコマンド実行が必要な場合はここでセットする。不要な場合はnullでOK
            List<CommonExcelCmdInfo> cmdInfoList = new List<CommonExcelCmdInfo>();

            // オプションデータ用
            List<TMQDao.ScheduleList.Display> scheduleDisplayList = null;

            int optionRowCount = 0;
            int optionColomnCount = 0;

            // テンプレート情報を取得
            var template = new ReportDao.MsOutputTemplateEntity().GetEntity(factoryId, reportId, templateId, db);
            if (template == null)
            {
                // 取得できない場合、処理を戻す
                return false;
            }
            // テンプレートファイル名を設定
            string templateFileName = template.TemplateFileName;
            // テンプレートファイルパスを設定
            string templateFilePath = template.TemplateFilePath;

            // 出力帳票シート定義のリストを取得
            var sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                ComReport.GetReportSheetDefine,
                ExcelPath,
                new { FactoryId = factoryId, ReportId = reportId },
                db);

            // 個別処理（RP0270：会計提出表）
            if (reportId.Equals(ReportRP0270.ReportId))
            {
                // RP0270：会計提出表シート定義作成、コマンド設定
                SetSheetDefineForRP0270(out sheetDefineList, out cmdInfoList);
            }

            if (sheetDefineList == null)
            {
                // 取得できない場合、処理を戻す
                return false;
            }

            // シート定義毎にループ
            foreach (var sheetDefine in sheetDefineList)
            {
                // 検索条件フラグの場合
                if (sheetDefine.SearchConditionFlg == true)
                {
                    // 検索条件からデータを取得し、エクセルを作成する
                    var dataList = GetReportDatabySearchCondition(searchCondition, db);
                    // マッピングデータ作成
                    List<CommonExcelPrtInfo> mappingDataList = TMQUtil.CreateMappingListForCondition(
                                                                            factoryId,
                                                                            programId,
                                                                            reportId,
                                                                            sheetDefine.SheetNo,
                                                                            templateId,
                                                                            outputPatternId,
                                                                            dataList,
                                                                            conditionSheetLocatoinList,
                                                                            conditionSheetJobList,
                                                                            conditionSheetNameList,
                                                                            conditionSheetValueList,
                                                                            templateFileName,
                                                                            templateFilePath,
                                                                            languageId,
                                                                            db,
                                                                            out optionRowCount,
                                                                            out optionColomnCount);
                    // マッピング情報リストに追加
                    mappingInfoList.AddRange(mappingDataList);
                }
                else
                {

                    // オプション指定のある場合
                    if (option != null)
                    {
                        // 出力方式とシート番号が異なる場合はデータ作成しない
                        if (option.OutputMode != sheetDefine.SheetNo)
                        {
                            continue;
                        }
                    }
                    // 対象SQLファイルにてSQLを実行し、エクセルを作成する
                    string targetSql = sheetDefine.TargetSql;
                    IList<dynamic> dataList = null;
                    // 会計帳票出力条件が設定されている場合、会計帳票用のデータ取得処理を実行する
                    if (condAccountReport != null)
                    {
                        // 会計帳票データを取得
                        if (condAccountReport.ReportId == ReportRP0270.ReportId)
                        {
                            dataList = GetAccountReportDataEveryAccountAndDepartment(targetSql, db, condAccountReport, sheetDefine.TargetSqlParams);
                        }
                        else
                        {
                            dataList = GetAccountReportData(targetSql, db, condAccountReport);
                        }
                        // 会計帳票データの個別変換処理
                        AccountReportJoinStrAndRound(dataList, sheetDefine.SheetNo, languageId, db, dicFixedValueForOutput);
                    }
                    // 集計帳票の場合、遷移元で集計した結果をセットする
                    else if (dicSummaryDataList != null)
                    {
                        dataList = new List<dynamic>(dicSummaryDataList[sheetDefine.SheetNo]);
                    }
                    else
                    {
                        // 帳票データを取得
                        // var dataList = GetReportData(dicSelectKeyDataList[sheetDefine.SheetNo], targetSql, db, userId, languageId, option);
                        dataList = GetReportData(dicSelectKeyDataList[sheetDefine.SheetNo], targetSql, db, userId, languageId, option);

                        // 帳票IDごとの個別処理
                        switch (reportId)
                        {
                            case "RP0190": // 予備品管理-予備品情報
                                RP0190JoinStrAndRound(dataList, sheetDefine.SheetNo, languageId, db);
                                break;
                            case ReportRP0410.ReportId: // 棚卸-棚卸準備表
                                RP0410JoinStrAndRound(dataList, sheetDefine.SheetNo, languageId, db, dicFixedValueForOutput);
                                break;
                            default:
                                break;

                        }
                    }

                    // 最大ネスト数分、ループ
                    for (int idx = 0; idx < ComReport.MaxLayerNest; idx++)
                    {
                        string addNum;

                        // ２周目以降は項目名末尾に数値を付与する
                        if (idx == 0)
                        {
                            addNum = "";
                        }
                        else
                        {
                            addNum = (idx + 1).ToString();
                        }

                        // 職種情報を設定する
                        foreach (var data in dataList)
                        {
                            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
                            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
                            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);

                            IList<StructureJobInfoForReport> jobInfoList = new List<StructureJobInfoForReport>();
                            StructureJobInfoForReport jobInfo = new StructureJobInfoForReport();

                            var rowDic = (IDictionary<string, object>)data;
                            var val = rowDic[ColumnName.JobStructureId + addNum];
                            if (val == null || string.IsNullOrEmpty(val.ToString()) == true)
                            {
                                continue;
                            }
                            jobInfo.JobStructureId = Convert.ToInt32(val);
                            jobInfoList.Add(jobInfo);
                            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureJobInfoForReport>(ref jobInfoList, typeLst, db, languageId);
                            // 関連情報の設定
                            if (jobInfoList == null)
                            {
                                continue;
                            }
                            // 職種名称
                            rowDic[ColumnName.JobName + addNum] = jobInfoList[0].JobName;
                            // 機種大分類名称
                            rowDic[ColumnName.LargeClassficationName + addNum] = jobInfoList[0].LargeClassficationName;
                            // 機種中分類名称
                            rowDic[ColumnName.MiddleClassficationName + addNum] = jobInfoList[0].MiddleClassficationName;
                            // 機種小分類名称
                            rowDic[ColumnName.SmallClassficationName + addNum] = jobInfoList[0].SmallClassficationName;

                            if (idx == 0)
                            {
                                // 機種名称設定(RP0060)
                                if (!string.IsNullOrEmpty(jobInfoList[0].LargeClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].LargeClassficationName;
                                }
                                if (!string.IsNullOrEmpty(jobInfoList[0].MiddleClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].MiddleClassficationName;
                                }
                                if (!string.IsNullOrEmpty(jobInfoList[0].SmallClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].SmallClassficationName;
                                }
                            }

                        }
                        // 機能場所階層情報を設定する
                        foreach (var data in dataList)
                        {
                            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
                            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
                            IList<StructureLocationInfoForReport> locationInfoList = new List<StructureLocationInfoForReport>();
                            StructureLocationInfoForReport locationInfo = new StructureLocationInfoForReport();

                            var rowDic = (IDictionary<string, object>)data;
                            var val = rowDic["location_structure_id" + addNum];
                            if (val == null || string.IsNullOrEmpty(val.ToString()) == true)
                            {
                                continue;
                            }
                            locationInfo.LocationStructureId = Convert.ToInt32(val);
                            locationInfoList.Add(locationInfo);
                            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureLocationInfoForReport>(ref locationInfoList, typeLst, db, languageId);
                            // 関連情報の設定
                            if (locationInfoList == null)
                            {
                                continue;
                            }

                            // 地区
                            rowDic[ColumnName.DistrictName + addNum] = locationInfoList[0].DistrictName;
                            // 工場
                            rowDic[ColumnName.FactoryName + addNum] = locationInfoList[0].FactoryName;
                            // プラント
                            rowDic[ColumnName.PlantName + addNum] = locationInfoList[0].PlantName;
                            // 系列
                            rowDic[ColumnName.SeriesName + addNum] = locationInfoList[0].SeriesName;
                            // 工程
                            rowDic[ColumnName.StrokeName + addNum] = locationInfoList[0].StrokeName;
                            // 設備
                            rowDic[ColumnName.FacilityName + addNum] = locationInfoList[0].FacilityName;

                        }
                    }

                    //                    typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);

                    // オプション指定のある場合
                    if (option != null)
                    {
                        string targetSqlSchedule = sheetDefine.TargetSql + ComReport.Option;
                        TMQUtil.GetFixedSqlStatement(ExcelPath, targetSqlSchedule, out string selectSql);
                        // 機器別の場合、上位ランクの場合の処理を行わないスケジュール変換クラス
                        if (sheetDefine.SheetNo == 2)
                        {
                            scheduleDisplayList = getScheduleDisplayList<TMQUtil.ScheduleListConverterNoRank>(selectSql, option.Condition);
                        }
                        // 機器別以外の場合、通常のスケジュールの変換
                        else
                        {
                            scheduleDisplayList = getScheduleDisplayList<TMQUtil.ScheduleListConverter>(selectSql, option.Condition);
                        }
                        // 画面表示データの取得 SQLを実行し、実行結果を変換 変換するクラスが上位ランクの有無が異なるので分岐
                        List<TMQDao.ScheduleList.Display> getScheduleDisplayList<T>(
                                string execSql,
                                Dao.ScheduleList.GetCondition cond)
                            where T : TMQUtil.ScheduleListConverter, new()
                        {

                            IList<TMQDao.ScheduleList.Get> scheduleList = db.GetListByDataClass<TMQDao.ScheduleList.Get>(execSql.ToString(), cond);
                            // 画面表示用に変換
                            T listSchedule = new();
                            // リンク有無
                            bool isLink = false; // リンクは詳細画面のみ
                            bool halfPeriodFlag = false;

                            // 出力方式が 3:予算別の場合
                            if (option.OutputMode == TMQUtil.ComReport.OutputMode3)
                            {
                                // 表示単位が 2:年度の場合
                                if (option.DisplayUnit == (int)Const.MsStructure.StructureId.ScheduleDisplayUnit.Year)
                                {
                                    // 半期ごとの表示単位
                                    halfPeriodFlag = true;
                                }
                                // 予算帳票用データ取得
                                var scheduleDisplayList = listSchedule.ExecuteForBudgetReport(scheduleList, cond, option.MonthStartNendo, isLink, db, null, halfPeriodFlag);
                                return scheduleDisplayList;
                            }
                            else
                            {
                                // 予算以外（件名別、機器別）帳票データ取得
                                var scheduleDisplayList = listSchedule.Execute(scheduleList, cond, option.MonthStartNendo, isLink, db, null);
                                return scheduleDisplayList;
                            }


                        }
                    }
                    // マッピングデータ作成
                    List<CommonExcelPrtInfo> mappingDataList = TMQUtil.CreateMappingList(
                                                                            factoryId,
                                                                            programId,
                                                                            reportId,
                                                                            sheetDefine.SheetNo,
                                                                            templateId,
                                                                            outputPatternId,
                                                                            dataList,
                                                                            templateFileName,
                                                                            templateFilePath,
                                                                            languageId,
                                                                            db,
                                                                            out optionRowCount,
                                                                            out optionColomnCount,
                                                                            option,
                                                                            scheduleDisplayList);
                    // マッピング情報リストに追加
                    mappingInfoList.AddRange(mappingDataList);
                    // 一覧罫線用にデータ件数を退避
                    sheetDataCount = dataList.Count;

                }
                // 一覧フラグの場合
                if (sheetDefine.ListFlg == true)
                {
                    // 罫線設定セル範囲を取得
                    string range = GetTargetCellRange(
                        factoryId,
                        reportId,
                        templateId,
                        outputPatternId,
                        sheetDefine.SheetNo,
                        sheetDataCount,
                        sheetDefine.RecordCount,
                        db,
                        optionRowCount,
                        optionColomnCount);
                    if (range != null)
                    {
                        string sheetName = GetSheetName(factoryId, reportId, sheetDefine.SheetNo, languageId, db);
                        if (!string.IsNullOrEmpty(sheetName))
                        {
                            //範囲が取得できた場合、罫線を引く
                            cmdInfoList.AddRange(CommandLineBox(range, GetSheetName(factoryId, reportId, sheetDefine.SheetNo, languageId, db)));
                        }

                    }
                }
                // オプション指定のある場合
                if (option != null)
                {
                    // 不要なシートを削除
                    int targetSheetNo = sheetDefine.SheetNo;
                    int deleteSheetNo1 = 0;
                    int deleteSheetNo2 = 0;
                    string deleteSheetNameNo1 = "";
                    string deleteSheetNameNo2 = "";
                    switch (sheetDefine.SheetNo)
                    {
                        case 1:
                            deleteSheetNo1 = 2;
                            deleteSheetNo2 = 3;
                            break;
                        case 2:
                            deleteSheetNo1 = 1;
                            deleteSheetNo2 = 3;
                            break;
                        case 3:
                            deleteSheetNo1 = 1;
                            deleteSheetNo2 = 2;
                            break;
                    }
                    // 削除用シート名を取得
                    deleteSheetNameNo1 = GetSheetName(factoryId, reportId, deleteSheetNo1, languageId, db);
                    deleteSheetNameNo2 = GetSheetName(factoryId, reportId, deleteSheetNo2, languageId, db);

                    // シート削除コマンドを追加
                    // コマンド情報
                    CommonExcelCmdInfo cmdInfo1, cmdInfo2;
                    // コマンドパラメータ
                    string[] param1, param2;
                    cmdInfo1 = new CommonExcelCmdInfo();
                    cmdInfo2 = new CommonExcelCmdInfo();
                    param1 = new string[1];
                    param2 = new string[1];
                    param1[0] = deleteSheetNameNo1;
                    param2[0] = deleteSheetNameNo2;
                    cmdInfo1.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdDeleteSheet, param1);
                    cmdInfo2.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdDeleteSheet, param2);
                    cmdInfoList.Add(cmdInfo1);
                    cmdInfoList.Add(cmdInfo2);

                    // 印刷範囲設定
                    // コマンド情報生成（印刷範囲設定）
                    // 罫線設定セル範囲を取得
                    string range = GetTargetCellRange(
                        factoryId,
                        reportId,
                        templateId,
                        outputPatternId,
                        sheetDefine.SheetNo,
                        sheetDataCount,
                        sheetDefine.RecordCount,
                        db,
                        optionRowCount,
                        optionColomnCount,
                        true);
                    CommonExcelCmdInfo cmdPrintArea;
                    cmdPrintArea = new CommonExcelCmdInfo();
                    string[] param;
                    param = new string[4];
                    param[0] = range;                                                               // [0]：印刷範囲
                    param[1] = GetSheetName(factoryId, reportId, targetSheetNo, languageId, db);    // [1]：シート名　デフォルトは先頭シート
                    param[2] = null;                                                                // [2]：印刷タイトル行
                    param[3] = null;                                                                // [3]：印刷タイトル列
                    cmdPrintArea.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdPrintArea, param);
                    cmdInfoList.Add(cmdPrintArea);

                }

                // 個別処理（RP0410：棚卸準備リスト）
                if (reportId.Equals(ReportRP0410.ReportId))
                {
                    CommonExcelCmdInfo cmdInfo1;
                    // コマンドパラメータ
                    string[] param1;
                    cmdInfo1 = new CommonExcelCmdInfo();
                    param1 = new string[2];
                    param1[0] = string.Empty;
                    param1[1] = ReportRP0410.password;
                    cmdInfo1.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdLockSheet, param1);
                    cmdInfoList.Add(cmdInfo1);
                }

            }
            // エクセルファイル作成
            ExcelUtil.CreateExcelFile(templateFileName, templateFilePath, userId, mappingInfoList, cmdInfoList, ref memoryStream, ref message);

            return true;


            /// <summary>
            /// RP0270：会計提出表シート定義作成、コマンド設定
            /// </summary>
            /// <param name="sheetDefineList">シート定義リスト</param>
            /// <param name="cmdInfoList">Excelコマンドリスト</param>
            void SetSheetDefineForRP0270(out List<ReportDao.MsOutputReportSheetDefineEntity> sheetDefineList, out List<CommonExcelCmdInfo> cmdInfoList)
            {
                sheetDefineList = new List<ReportDao.MsOutputReportSheetDefineEntity>();
                cmdInfoList = new List<CommonExcelCmdInfo>();

                // 変数
                int totalSheetCnt = 0;
                // コマンドパラメータ
                CommonExcelCmdInfo cmdInfo;
                string addSheetName = string.Empty;
                string addSheetNamePage = string.Empty;

                // 出力データに合わせて出力帳票シート定義の作成を行う

                // 取得済の出力帳票シート定義のリストをクリアする
                sheetDefineList.Clear();
                // 主キーを指定して出力帳票シート定義のベースとなる定義を取得する（sheetNo：1）
                ReportDao.MsOutputReportSheetDefineEntity sheetDefineBase = new ReportDao.MsOutputReportSheetDefineEntity().GetEntity(factoryId, reportId, ReportRP0270.BaseSheetNo, db);

                // 出力対象となるデータから以下を求める
                // 　勘定科目コード、部門コード、トータル件数
                IList<dynamic> dataList = GetAccountReportDataCnt(sheetDefineBase.TargetSql, db, condAccountReport);

                foreach (var data in dataList)
                {
                    int totalPageCnt = 1;
                    int currentPage = 1;

                    var rowDic = (IDictionary<string, object>)data;
                    var accountStructureId = rowDic["account_structure_id"];
                    var departmentStructureId = rowDic["department_structure_id"];
                    var accountId = rowDic["account_id"];
                    var departmentId = rowDic["department_id"];
                    var total = rowDic["total"];
                    var dataCnt = rowDic["data_cnt"];

                    // 出力帳票シート定義の作成
                    SheetDefineCopy(sheetDefineBase, out ReportDao.MsOutputReportSheetDefineEntity sheetDefine);

                    // 追加するシートNoを割り当てる
                    totalSheetCnt += 1;
                    sheetDefine.SheetNo = totalSheetCnt;

                    // totalが1ページの明細最大件数を超えている場合
                    if (int.Parse(dataCnt.ToString()) > ReportRP0270.ListMaxCnt)
                    {
                        totalPageCnt = Convert.ToInt32(Math.Ceiling((double)int.Parse(dataCnt.ToString()) / ReportRP0270.ListMaxCnt));
                    }
                    else
                    {
                        totalPageCnt = 1;
                    }

                    // シート定義のSQLパラメータに次の値をセットする
                    // @CurrentPage、@LastPage、@Total、@ListMaxCnt、@AccountStructureId、@DepartmentIdList
                    sheetDefine.TargetSqlParams = string.Join(",", new[] {
                        currentPage.ToString(),
                        totalPageCnt.ToString(),
                        total.ToString(),
                        ReportRP0270.ListMaxCnt.ToString() ,
                        accountStructureId.ToString(),
                        departmentStructureId.ToString() });

                    // シート定義を追加する（1ページ目）
                    sheetDefineList.Add(sheetDefine);

                    // シートをコピーするコマンドを準備
                    // 追加シート名の作成
                    // 会計提出表_S
                    // 　⇒会計提出表+ "_" + 勘定科目コード + "_" + 部門コード + "_" + ページ番号
                    addSheetName = ReportRP0270.BaseTemplateSheetNameS.Replace("_S", "_" + accountId.ToString() + "_" + departmentId.ToString());
                    addSheetNamePage = "_" + currentPage.ToString();
                    // ３１文字を超える場合、ページ番号以外を削る
                    if (addSheetName.Length + addSheetNamePage.Length > ReportRP0270.MaxSheetNameLength)
                    {
                        addSheetName = addSheetName.Substring(0, ReportRP0270.MaxSheetNameLength - addSheetNamePage.Length);
                    }
                    GetSheetCopyCmd(ReportRP0270.BaseTemplateSheetNameS, string.Empty, string.Empty, addSheetName + addSheetNamePage, CommonExcelCmdInfo.CExecTmgBefore, out cmdInfo);
                    // コマンドを追加
                    cmdInfoList.Add(cmdInfo);

                    // ２ページ目移行のシート定義、シート追加コマンドを追加する
                    for (int iCnt = 2; iCnt <= totalPageCnt; iCnt++)
                    {
                        // 出力帳票シート定義の作成
                        SheetDefineCopy(sheetDefineBase, out sheetDefine);

                        // ページ番号のカウントアップ
                        currentPage += 1;

                        // 追加するシートNoを割り当てる
                        totalSheetCnt += 1;
                        sheetDefine.SheetNo = totalSheetCnt;

                        // シート定義のSQLパラメータに次の値をセットする
                        // @CurrentPage、@LastPage、@Total、@ListMaxCnt、@AccountStructureId、@DepartmentIdList
                        sheetDefine.TargetSqlParams = string.Join(",", new[] {
                            currentPage.ToString(),
                            totalPageCnt.ToString(),
                            total.ToString(),
                            ReportRP0270.ListMaxCnt.ToString(),
                            accountStructureId.ToString(),
                            departmentStructureId.ToString() });

                        // シート定義を追加する（1ページ目）
                        sheetDefineList.Add(sheetDefine);

                        // シートをコピーするコマンドを準備
                        // 追加シート名の作成
                        // 会計提出表_M
                        // 　⇒会計提出表+ "_" + 勘定科目コード + "_" + 部門コード + "_" + ページ番号
                        addSheetName = ReportRP0270.BaseTemplateSheetNameM.Replace("_M", "_" + accountId.ToString() + "_" + departmentId.ToString());
                        addSheetNamePage = "_" + currentPage.ToString();
                        // ３１文字を超える場合、ページ番号以外を削る
                        if (addSheetName.Length + addSheetNamePage.Length > ReportRP0270.MaxSheetNameLength)
                        {
                            addSheetName = addSheetName.Substring(0, ReportRP0270.MaxSheetNameLength - addSheetNamePage.Length);
                        }
                        GetSheetCopyCmd(ReportRP0270.BaseTemplateSheetNameM, string.Empty, string.Empty, addSheetName + addSheetNamePage, CommonExcelCmdInfo.CExecTmgBefore, out cmdInfo);
                        // コマンドを追加
                        cmdInfoList.Add(cmdInfo);
                    }
                }

                // templatefileの不要シートの削除
                GetSheetDeleteCmd(ReportRP0270.BaseTemplateSheetNameS, CommonExcelCmdInfo.CExecTmgBefore, out cmdInfo);
                cmdInfoList.Add(cmdInfo);
                GetSheetDeleteCmd(ReportRP0270.BaseTemplateSheetNameM, CommonExcelCmdInfo.CExecTmgBefore, out cmdInfo);
                cmdInfoList.Add(cmdInfo);
            }

            /// <summary>
            /// シート定義クラスコピー
            /// </summary>
            /// <param name="sheetDefineBase">シート定義クラス（コピー元）</param>
            /// <param name="sheetDefine">シート定義クラス（コピー先）</param>
            void SheetDefineCopy(ReportDao.MsOutputReportSheetDefineEntity sheetDefineBase, out ReportDao.MsOutputReportSheetDefineEntity sheetDefine)
            {
                sheetDefine = new ReportDao.MsOutputReportSheetDefineEntity();

                sheetDefine.FactoryId = sheetDefineBase.FactoryId;
                sheetDefine.LanguageId = sheetDefineBase.LanguageId;
                sheetDefine.ListFlg = sheetDefineBase.ListFlg;
                sheetDefine.RecordCount = sheetDefineBase.RecordCount;
                sheetDefine.ReportId = sheetDefineBase.ReportId;
                sheetDefine.SearchConditionFlg = sheetDefineBase.SearchConditionFlg;
                sheetDefine.SheetDefineMaxColumn = sheetDefineBase.SheetDefineMaxColumn;
                sheetDefine.SheetDefineMaxRow = sheetDefineBase.SheetDefineMaxRow;
                sheetDefine.SheetName = sheetDefineBase.SheetName;
                sheetDefine.SheetNameTranslationId = sheetDefineBase.SheetNameTranslationId;
                sheetDefine.TargetSql = sheetDefineBase.TargetSql;

            }

            /// <summary>
            /// シートコピーコマンド作成
            /// </summary>
            /// <param name="baseSheetName">コピー元シート名　デフォルトは先頭シート</param>
            /// <param name="copyPosition">コピー位置（シート名、シート番号：デフォルトは一番後ろ）</param>
            /// <param name="copyBeforeOrAfter">Before・After　デフォルトはAfter</param>
            /// <param name="newSheetName">シート名（未設定時は標準仕様）</param>
            /// <param name="execTmg">実行タイミング</param>
            /// <param name="cmdInfo">コマンド</param>
            void GetSheetCopyCmd(string baseSheetName, string copyPosition, string copyBeforeOrAfter, string newSheetName, string execTmg, out CommonExcelCmdInfo cmdInfo)
            {
                cmdInfo = new CommonExcelCmdInfo();
                string[] param = new string[4];
                param[0] = baseSheetName;
                param[1] = copyPosition;
                param[2] = copyBeforeOrAfter;
                param[3] = newSheetName;
                cmdInfo.SetExlCmdInfo(execTmg, CommonExcelCmdInfo.CExecCmdCopySheet, param);
            }

            /// <summary>
            /// シート削除コマンド作成
            /// </summary>
            /// <param name="sheetName">シート名</param>
            /// <param name="execTmg">実行タイミング</param>
            /// <param name="cmdInfo">コマンド</param>
            void GetSheetDeleteCmd(string sheetName, string execTmg, out CommonExcelCmdInfo cmdInfo)
            {
                cmdInfo = new CommonExcelCmdInfo();
                string[] param = new string[1];
                param[0] = sheetName;
                cmdInfo.SetExlCmdInfo(execTmg, CommonExcelCmdInfo.CExecCmdDeleteSheet, param);
            }

        }

        /// <summary>
        /// 帳票データ取得(検索条件から)
        /// </summary>
        /// <param name="selectKeyDataList">選択データ</param>
        /// <param name="targetSql">対象SQL</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:正常　false:エラー</returns>
        public static IList<dynamic> GetReportDatabySearchCondition(
            dynamic searchCondition,
            ComDB db)
        {
            IList<dynamic> dataList = new List<dynamic>();
            IDictionary<string, object> dicResult = new Dictionary<string, object>();

            // 場所階層構成IDリストを取得
            if (((IDictionary<string, object>)searchCondition).ContainsKey(ItemName.LocationIdList))
            {
                var locationIdList = searchCondition.LocationIdList;
                foreach (int locationId in locationIdList)
                {
                    // 構成マスタビューからデータ取得
                    var structureItem = new STDDao.VStructureItemEntity().GetEntity(locationId, db);
                    string keyName = "";
                    // 階層の各レイヤーに設定する。複数ある場合、カンマで追加する。
                    switch (structureItem.StructureLayerNo.GetValueOrDefault())
                    {
                        // 工場
                        case (int)Const.MsStructure.StructureLayerNo.Location.Factory:
                            keyName = ColumnName.FactoryName;
                            break;
                        // プラント
                        case (int)Const.MsStructure.StructureLayerNo.Location.Plant:
                            keyName = ColumnName.PlantName;
                            break;
                        // 系列
                        case (int)Const.MsStructure.StructureLayerNo.Location.Series:
                            keyName = ColumnName.SeriesName;
                            break;
                        // 工程
                        case (int)Const.MsStructure.StructureLayerNo.Location.Stroke:
                            keyName = ColumnName.StrokeName;
                            break;
                        // 設備
                        case (int)Const.MsStructure.StructureLayerNo.Location.Facility:
                            keyName = ColumnName.FacilityName;
                            break;
                        default:
                            keyName = "";
                            break;

                    }
                    if (!string.IsNullOrEmpty(keyName))
                    {
                        if (dicResult.ContainsKey(keyName))
                        {
                            dicResult[keyName] = dicResult[keyName] + "," + structureItem.TranslationText;
                        }
                        else
                        {
                            dicResult.Add(keyName, structureItem.TranslationText);
                        }
                    }

                }
            }
            // 職種機種構成IDリストを取得
            if (((IDictionary<string, object>)searchCondition).ContainsKey(ItemName.JobIdList))
            {
                var jobIdList = searchCondition.JobIdList;
                foreach (int jobId in jobIdList)
                {
                    // 構成マスタビューからデータ取得
                    var structureItem = new STDDao.VStructureItemEntity().GetEntity(jobId, db);
                    string keyName = ColumnName.JobName;
                    if (dicResult.ContainsKey(keyName))
                    {
                        dicResult[keyName] = dicResult[keyName] + "," + structureItem.TranslationText;
                    }
                    else
                    {
                        dicResult.Add(keyName, structureItem.TranslationText);
                    }
                }
            }
            // 詳細検索条件を取得
            if (((IDictionary<string, object>)searchCondition).ContainsKey(ItemName.detailSearchList))
            {
                var detailSearchList = searchCondition.detailSearchList;
                foreach (Dictionary<string, object> detailSearch in detailSearchList)
                {
                    foreach (var dic in detailSearch)
                    {
                        dicResult.Add(dic);
                    }
                }
            }

            dataList.Add(dicResult);
            return dataList;

        }

        /// <summary>
        /// 帳票データ取得
        /// </summary>
        /// <param name="selectKeyDataList">選択データ</param>
        /// <param name="targetSql">対象SQL</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="option">オプションクラス</param>
        /// <returns>true:正常　false:エラー</returns>
        public static IList<dynamic> GetReportData(
            List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData> selectKeyDataList,
            string targetSql,
            ComDB db,
            string userId,
            string languageId,
            Option option = null)
        {
            // テンポラリテーブル作成
            TMQUtil.SqlExecuteClass.Regist(ComReport.CreateTableTemp, ExcelPath, null, db);

            // ユーザの本務工場を取得
            List<int> FactoryIdList = TMQUtil.GetFactoryIdList(userId, db);
            FactoryIdList.Sort();
            int factoryId = FactoryIdList[FactoryIdList.Count - 1];

            // 選択データ分ループ
            if (selectKeyDataList.Count > 0)
            {
                foreach (var selectKey in selectKeyDataList)
                {
                    // テンポラリデータ作成
                    TMQUtil.SqlExecuteClass.Regist(ComReport.InsertTemp,
                                                   ExcelPath,
                                                   new { Key1 = selectKey.Key1, Key2 = selectKey.Key2, Key3 = selectKey.Key3, LanguageId = languageId, FactoryId = factoryId },
                                                   db);
                }
            }
            else
            {
                // テンポラリデータ作成
                TMQUtil.SqlExecuteClass.Regist(ComReport.InsertTemp,
                                               ExcelPath,
                                               new { Key1 = -1, Key2 = -1, Key3 = -1, LanguageId = languageId, FactoryId = factoryId },
                                               db);
            }
            // 対象SQLファイルからSQL文を取得
            if (!TMQUtil.GetFixedSqlStatement(ExcelPath, targetSql, out string selectSql))
            {
                return null;
            }
            // オプション指定のある場合
            if (option != null)
            {
                // パラメータ指定して帳票データ取得
                var OptionDataList = db.GetList(selectSql, new { StartDate = option.StartDate, EndDate = option.EndDate });
                return OptionDataList;
            }
            // 帳票データ取得
            var dataList = db.GetList(selectSql);
            return dataList;
        }
        #endregion

        #region 会計帳票データ関連処理
        /// <summary>
        /// 会計帳票データ取得
        /// </summary>
        /// <param name="targetSql">対象SQL</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="condAccountReport">会計帳票出力条件のデータクラス</param>
        /// <returns>true:正常　false:エラー</returns>
        public static IList<dynamic> GetAccountReportData(
            string targetSql,
            ComDB db,
            CondAccountReport condAccountReport)
        {
            string selectSql = string.Empty;

            // SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(ExcelPath, targetSql, out selectSql, condAccountReport.ListUnComment))
            {
                return null;
            }

            var results = db.GetListByDataClass<dynamic>(selectSql.ToString(), condAccountReport);

            return results;
        }

        /// <summary>
        /// 会計帳票データ取得
        /// </summary>
        /// <param name="targetSql">対象SQL</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="condAccountReport">会計帳票出力条件のデータクラス</param>
        /// <returns>true:正常　false:エラー</returns>
        public static IList<dynamic> GetAccountReportDataEveryAccountAndDepartment(
            string targetSql,
            ComDB db,
            CondAccountReport condAccountReport,
            string targetSqlParams)
        {

            string selectSql = string.Empty;
            // 画面で指定した条件に加えてデータ取得用のコメントを追加
            List<string> listUnCommentForData = new List<string>(condAccountReport.ListUnComment);
            listUnCommentForData.Add(ReportRP0270.SqlUnCommentForDataSelect);

            // シート定義作成時に追加した以下の値をパラメータとして設定する
            // @CurrentPage、@LastPage、@Total、@ListMaxCnt、\ountStructureId、@DepartmentIdList
            string[] targetSqlParam = targetSqlParams.Split(',');
            condAccountReport.CurrentPage = int.Parse(targetSqlParam[0]);
            condAccountReport.LastPage = int.Parse(targetSqlParam[1]);
            condAccountReport.Total = Decimal.Parse(targetSqlParam[2]);
            condAccountReport.ListMaxCnt = int.Parse(targetSqlParam[3]);

            // 勘定科目ID　※画面で未設定の場合のみ
            if (condAccountReport.AccountStructureId == null || condAccountReport.AccountStructureId <= 0)
            {
                condAccountReport.AccountStructureId = int.Parse(targetSqlParam[4]);
            }
            listUnCommentForData.Add(nameof(condAccountReport.AccountStructureId));

            // 部門IDリスト　※画面で設定済であれば一旦クリアする
            if (condAccountReport.DepartmentIdList != null && condAccountReport.DepartmentIdList.Count > 0)
            {
                condAccountReport.DepartmentIdList.Clear();
            }
            else
            {
                condAccountReport.DepartmentIdList = new List<int>();
            }
            condAccountReport.DepartmentIdList.Add(int.Parse(targetSqlParam[5]));
            listUnCommentForData.Add(nameof(condAccountReport.DepartmentIdList));

            // SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(ExcelPath, targetSql, out selectSql, listUnCommentForData))
            {
                return null;
            }

            var results = db.GetListByDataClass<dynamic>(selectSql.ToString(), condAccountReport);

            return results;
        }

        /// <summary>
        /// 会計帳票データ件数取得（会計提出表用）
        /// </summary>
        /// <param name="targetSql">対象SQL</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="condAccountReport">会計帳票出力条件のデータクラス</param>
        /// <returns>true:正常　false:エラー</returns>
        public static IList<dynamic> GetAccountReportDataCnt(
            string targetSql,
            ComDB db,
            CondAccountReport condAccountReport)
        {
            string selectSql = string.Empty;

            // 画面で指定した条件に加えて件数取得用のコメントを追加
            List<string> listUnCommentForCnt = new List<string>(condAccountReport.ListUnComment);
            // 勘定科目、部門ごとの件数取得
            listUnCommentForCnt.Add(ReportRP0270.SqlUnCommentForDataCnt);

            // SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(ExcelPath, targetSql, out selectSql, listUnCommentForCnt))
            {
                return null;
            }

            var results = db.GetListByDataClass<dynamic>(selectSql.ToString(), condAccountReport);

            return results;
        }

        /// <summary>
        /// 会計帳票の出力データ取得後の個別処理
        /// </summary>
        /// <param name="results">取得結果</param>
        /// <param name="sheetNo">シートNo</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB情報</param>
        /// <param name="dicFixedValueForOutput">固定出力データ</param>
        public static void AccountReportJoinStrAndRound(IList<dynamic> results, int sheetNo, string languageId, ComDB db, Dictionary<string, string> dicFixedValueForOutput = null)
        {
            if (results == null)
            {
                return;
            }

            //棚IDより翻訳をまとめて取得しておく
            List<dynamic> partsLocationIdList = results.Select(x => x.parts_location_id.ToString()).Distinct().ToList();
            List<STDDao.VStructureItemEntity> partsLocationList = TMQUtil.GetpartsLocationList(partsLocationIdList, languageId, db);

            //工場ID棚番結合文字列を保持するDictionary
            Dictionary<int, string> factoryJoinDic = new();

            string joinStr = string.Empty;
            foreach (dynamic result in results)
            {
                //棚番の設定
                if (result.parts_location_id != null && result.parts_location_detail_no != null && result.factory_id != null && result.parts_location_name != null)
                {
                    //棚番
                    long partsLocationId = long.Parse(result.parts_location_id.ToString());
                    string partsLocationDetailNo = (result.parts_location_detail_no).ToString();
                    int factoryId = int.Parse(result.factory_id.ToString());
                    result.parts_location_name = TMQUtil.GetDisplayPartsLocation(partsLocationId, partsLocationDetailNo, factoryId, languageId, db, ref factoryJoinDic, partsLocationList);
                }
                else
                {
                    if (result.parts_location_id != null && result.factory_id != null && result.parts_location_name != null)
                    {
                        //棚番
                        long partsLocationId = long.Parse(result.parts_location_id.ToString());
                        string partsLocationDetailNo = String.Empty;
                        int factoryId = int.Parse(result.factory_id.ToString());
                        result.parts_location_name = TMQUtil.GetDisplayPartsLocation(partsLocationId, partsLocationDetailNo, factoryId, languageId, db, ref factoryJoinDic, partsLocationList);
                    }
                }

                //// 文字列に変換
                //string strStockAmount = (result.stock_amount).ToString();       // 在庫金額
                //string strStockQuantity = (result.stock_quantity).ToString(); 　// 在庫数

                //result.stock_Amount = TMQUtil.roundDigit(strStockAmount, result.unit_digit, result.unit_round_division);        // 在庫金額
                //result.stock_quantity = TMQUtil.roundDigit(strStockQuantity, result.unit_digit, result.unit_round_division);    // 在庫数
                //                                                                                                                //棚番
                //result.parts_location_name = TMQUtil.GetDisplayPartsLocation(result.parts_location_id, result.parts_location_detail_no, result.FactoryId, languageId, db);

                //// 固定出力データの埋め込み
                //if (dicFixedValueForOutput != null && dicFixedValueForOutput.Count > 0)
                //{
                //    foreach (KeyValuePair<string, string> fixedValue in dicFixedValueForOutput)
                //    {
                //        // 項目名が一致した場合、固定値を結果に埋め込む
                //        if (result.storage_location_id != null && nameof(result.storage_location_id) == fixedValue.Key)
                //        {
                //            result.storage_location_id = fixedValue.Value;
                //        }
                //        if (result.department_id_list != null && nameof(result.department_id_list) == fixedValue.Key)
                //        {
                //            result.department_id_list = fixedValue.Value;
                //        }
                //    }
                //}
            }
        }
        #endregion

        #region　予備品関連処理
        /// <summary>
        /// 予備品管理－予備品情報(RP0190)の出力データ取得後の個別処理
        /// </summary>
        /// <param name="results">取得結果</param>
        /// <param name="sheetNo">シートNo</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB情報</param>
        public static void RP0190JoinStrAndRound(IList<dynamic> results, int sheetNo, string languageId, ComDB db)
        {
            // 予備品一覧のデータを表示するシート
            if (sheetNo == 1)
            {
                // 工場IDと結合文字列のディクショナリ、同じ工場で重複取得しないようにする
                Dictionary<int, string> factoryJoinDic = new();
                string joinStr = string.Empty;
                foreach (dynamic result in results)
                {
                    // 棚番と枝番を結合する
                    if (!string.IsNullOrEmpty((result.rack_name).ToString()) && !string.IsNullOrEmpty((result.parts_location_detail_no).ToString()))
                    {
                        // 結合文字列を取得
                        joinStr = TMQUtil.GetJoinStrOfPartsLocationNoDuplicate((int)result.factory_id, languageId, db, ref factoryJoinDic);
                        // 棚番 + 枝番
                        result.rack_name = TMQUtil.GetDisplayPartsLocation(result.rack_name, result.parts_location_detail_no, joinStr);
                    }

                    // 文字列に変換
                    string strLeadTime = (result.lead_time).ToString();           // 発注点
                    string strOrderQuantity = (result.order_quantity).ToString(); // 発注量
                    string strUnitPrice = (result.unit_price).ToString();         // 標準単価
                    string strStockQuantity = (result.stock_quantity).ToString(); // 最新在庫数

                    result.lead_time = TMQUtil.roundDigit(strLeadTime, result.unit_digit, result.unit_round_division);           // 発注点    
                    result.order_quantity = TMQUtil.roundDigit(strOrderQuantity, result.unit_digit, result.unit_round_division); // 発注量
                    result.unit_price = TMQUtil.roundDigit(strUnitPrice, result.unit_digit, result.unit_round_division);         // 標準単価
                    result.stock_quantity = TMQUtil.roundDigit(strStockQuantity, result.unit_digit, result.unit_round_division); // 最新在庫数

                    result.lead_time = TMQUtil.CombineNumberAndUnit(result.lead_time, result.unit_name, true);           // 発注点
                    result.order_quantity = TMQUtil.CombineNumberAndUnit(result.order_quantity, result.unit_name, true); // 発注量
                    result.unit_price = TMQUtil.CombineNumberAndUnit(result.unit_price, result.currency_name, true);     // 標準単価
                    result.stock_quantity = TMQUtil.CombineNumberAndUnit(result.stock_quantity, result.unit_name, true); // 最新在庫数

                }
            }
        }

        /// <summary>
        /// 棚卸－棚卸準備表(RP0410)の出力データ取得後の個別処理
        /// </summary>
        /// <param name="results">取得結果</param>
        /// <param name="sheetNo">シートNo</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB情報</param>
        /// <param name="dicFixedValueForOutput">固定出力データ</param>
        public static void RP0410JoinStrAndRound(IList<dynamic> results, int sheetNo, string languageId, ComDB db, Dictionary<string, string> dicFixedValueForOutput = null)
        {
            // 棚卸準備表のデータを表示するシート
            if (sheetNo == 1)
            {
                if (results == null)
                {
                    return;
                }
                //棚IDより翻訳をまとめて取得しておく
                List<dynamic> partsLocationIdList = results.Select(x => x.parts_location_id.ToString()).Distinct().ToList();
                List<STDDao.VStructureItemEntity> partsLocationList = TMQUtil.GetpartsLocationList(partsLocationIdList, languageId, db);

                //工場ID棚番結合文字列を保持するDictionary
                Dictionary<int, string> factoryJoinDic = new();

                string joinStr = string.Empty;
                foreach (dynamic result in results)
                {
                    // 文字列に変換
                    string strStockAmount = (result.stock_amount).ToString();       // 在庫金額
                    string strStockQuantity = (result.stock_quantity).ToString(); 　// 在庫数

                    result.stock_Amount = TMQUtil.roundDigit(strStockAmount, result.unit_digit, result.unit_round_division);        // 在庫金額
                    result.stock_quantity = TMQUtil.roundDigit(strStockQuantity, result.unit_digit, result.unit_round_division);    // 在庫数

                    //棚番
                    long partsLocationId = long.Parse(result.parts_location_id.ToString());
                    string partsLocationDetailNo = string.Empty;
                    if (result.parts_location_detail_no != null)
                    {
                        partsLocationDetailNo = (result.parts_location_detail_no).ToString();
                    }
                    int factoryId = int.Parse(result.factory_id.ToString());

                    result.parts_location_name = TMQUtil.GetDisplayPartsLocation(partsLocationId, partsLocationDetailNo, factoryId, languageId, db, ref factoryJoinDic, partsLocationList);

                    // 固定出力データの埋め込み
                    if (dicFixedValueForOutput != null && dicFixedValueForOutput.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> fixedValue in dicFixedValueForOutput)
                        {
                            // 項目名が一致した場合、固定値を結果に埋め込む
                            if (result.storage_location_id != null && nameof(result.storage_location_id) == fixedValue.Key)
                            {
                                result.storage_location_id = fixedValue.Value;
                            }
                            if (result.department_id_list != null && nameof(result.department_id_list) == fixedValue.Key)
                            {
                                result.department_id_list = fixedValue.Value;
                            }
                            if (result.factory_name != null && nameof(result.factory_name) == fixedValue.Key)
                            {
                                result.factory_name = fixedValue.Value;
                            }
                            if (result.warehouse_name != null && nameof(result.warehouse_name) == fixedValue.Key)
                            {
                                result.warehouse_name = fixedValue.Value;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 個別工場ID設定の帳票定義が存在するかを確認
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="proram_id">プログラムID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="db">DB情報</param>
        public static Boolean IsExistsFactoryReportDefine(int factoryId, string programId, string reportId, ComDB db)
        {
            // 帳票定義を取得
            var reportDefine = new ReportDao.MsOutputReportDefineEntity().GetEntity(factoryId, programId, reportId, db);
            // 成功すればtrue、未取得であればfalse
            if (reportDefine != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 帳票データ取得
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="programId">プログラムID</param>
        /// <param name="selectKeyDataList">選択データ</param>
        /// <param name="searchCondition">検索条件</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="outputPattenId">出力パターンID</param>
        /// <param name="dataList">データリスト</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:正常　false:エラー</returns>
        [Obsolete("古いメソッドの為削除予定")]
        public static bool GetReportData<T>(
            int factoryId,
            string programId,
            List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData> selectKeyDataList,
            string searchCondition,
            string reportId,
            int templateId,
            int outputPattenId,
            ref List<T> dataList,
            ComDB db)
        {
            // テンポラリテーブル作成
            TMQUtil.SqlExecuteClass.Regist(ComReport.CreateTableTemp, ExcelPath, null, db);

            // 選択データ分ループ
            foreach (var selectKey in selectKeyDataList)
            {
                // テンポラリデータ作成
                TMQUtil.SqlExecuteClass.Regist(ComReport.InsertTemp, ExcelPath, new { Key1 = selectKey.Key1, Key2 = selectKey.Key2, Key3 = selectKey.Key3 }, db);
            }

            // 帳票データ取得
            dataList = TMQUtil.SqlExecuteClass.SelectList<T>(reportId, ExcelPath, null, db);


            return true;
        }

        /// <summary>
        /// ファイル マッピング情報
        /// </summary>
        /// <typeparam name="T">データクラス</typeparam>
        /// <param name="factoryId">工場ID</param>
        /// <param name="programId">プログラムID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="outputPattenId">出力パターンID</param>
        /// <param name="list">出力結果</param>
        /// <param name="templateFileName">テンプレートファイル名</param>
        /// <param name="templateFilePath">テンプレートファイルパス</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="optionRowCount">オプション行数</param>
        /// <param name="optionColumnCount">オプション行数</param>
        /// <param name="option">オプションクラス</param>
        /// <param name="optionDataList">オプションクラス</param>
        /// <returns>ファイル マッピング情報リスト</returns>
        public static List<CommonExcelPrtInfo> CreateMappingList(
            int factoryId,
            string programId,
            string reportId,
            int sheetNo,
            int templateId,
            int outputPattenId,
            dynamic list,
            string templateFileName,
            string templateFilePath,
            string languageId,
            ComDB db,
            out int optionRowCount,
            out int optionColumnCount,
            Option option = null,
            List<TMQDao.ScheduleList.Display> optionDataList = null)
        {
            // 初期化
            var mappingList = new List<CommonExcelPrtInfo>();

            optionRowCount = 0;
            optionColumnCount = 0;

            // 帳票定義を取得
            var reportDefine = new ReportDao.MsOutputReportDefineEntity().GetEntity(factoryId, programId, reportId, db);

            // シート定義を取得
            var sheetDefine = new ReportDao.MsOutputReportSheetDefineEntity().GetEntity(factoryId, reportId, sheetNo, db);
            if (reportId == ReportRP0270.ReportId)
            {
                // 会計提出表の場合、帳票定義１つの為、シートNoは固定で取得
                sheetDefine = new ReportDao.MsOutputReportSheetDefineEntity().GetEntity(factoryId, reportId, ReportRP0270.BaseSheetNo, db);
            }

            if (sheetDefine == null)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(ExcelPath, ComReport.GetComReportInfo, out string baseSql);

            // 項目定義を取得
            IList<Dao.InoutDefine> reportInfoList = db.GetListByDataClass<Dao.InoutDefine>(
                baseSql,
                new { FactoryId = factoryId, LanguageId = languageId,  ReportId = reportId, SheetNo = sheetNo, TemplateId = templateId, OutputPatternId = outputPattenId });
            if (reportId == ReportRP0270.ReportId)
            {
                // 会計提出表の場合、帳票定義１つの為、シートNoは固定で取得
                reportInfoList = db.GetListByDataClass<Dao.InoutDefine>(
                    baseSql,
                    new { FactoryId = factoryId, ReportId = reportId, SheetNo = ReportRP0270.BaseSheetNo, TemplateId = templateId, OutputPatternId = outputPattenId });
            }
            if (reportInfoList == null || reportInfoList.Count == 0)
            {
                // 取得できない場合、処理を戻す
                return null;
            }
            // 検索結果のプロパティを列挙
            //var properites = typeof(T).GetProperties();

            // 対象テンプレートファイルシートのデータを読込み、開始行番号と開始列番号を設定
            if (reportId == ReportRP0270.ReportId)
            {
                // 会計提出表の場合、帳票定義１つの為、シートNoは固定で取得
                SetStartInfo(ref reportInfoList, templateFileName, templateFilePath, ReportRP0270.BaseSheetNo, sheetDefine.SheetDefineMaxRow, sheetDefine.SheetDefineMaxColumn, reportDefine.OutputItemType);
            }
            else
            {
                SetStartInfo(ref reportInfoList, templateFileName, templateFilePath, sheetNo, sheetDefine.SheetDefineMaxRow, sheetDefine.SheetDefineMaxColumn, reportDefine.OutputItemType);
            }

            // 共通用タイトル、共通用日付、共通用時刻をマッピング
            // 項目定義
            foreach (var reportInfo in reportInfoList)
            {
                // 開始行番号、開始列番号が未指定の場合、スキップ
                if (reportInfo.StartRowNo == null || reportInfo.StartColNo == null)
                {
                    continue;
                }
                // カラム名が共通用タイトル、共通用日付、共通用時刻以外の場合、スキップ
                //if (reportInfo.ColumnName != ColumnName.ComTitle && reportInfo.ColumnName != ColumnName.ComDate && reportInfo.ColumnName != ColumnName.ComTime)
                if (!ColumnName.IsCommonColumn(reportInfo.ColumnName))
                {
                    continue;
                }

                // 初期化
                var info = new CommonExcelPrtInfo();
                info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
                info.SetSheetNo(sheetNo); // シート番号に対象のシート番号を設定
                bool setTitle = false;
                // マッピングセルを設定
                string address = ToAlphabet((int)reportInfo.StartColNo) + reportInfo.StartRowNo;
                // フォーマット設定
                string format = ComReport.StrFormat;
                object val;
                // カラム名ごとに値を設定
                switch (reportInfo.ColumnName)
                {
                    // 共通タイトルの場合
                    case (ColumnName.ComTitle):
                        // 帳票名を設定
                        val = GetTranslationText(reportDefine.ReportNameTranslationId, languageId, db);
                        break;
                    // 共通日付の場合
                    case (ColumnName.ComDate):
                        // フォーマット：曜日の完全名, 月の省略名 月の日にち 年４桁 ※カルチャをen-US(英語/アメリカ合衆国)に指定
                        val = DateTime.Now.ToString("dddd, MMM d yyyy", new System.Globalization.CultureInfo("en-US"));
                        break;
                    // 共通時刻の場合
                    case (ColumnName.ComTime):
                        val = DateTime.Now.ToString("HH:mm:ss");
                        break;

                    // 共通シートタイトルの場合
                    case (ColumnName.ComSheetTitle):
                        // シート名を設定
                        val = GetTranslationText(sheetDefine.SheetNameTranslationId, languageId, db);
                        break;
                    // 共通バージョンの場合
                    case ColumnName.ComVersion:
                        val = option.Version.ToString(reportInfo.FormatText);
                        setTitle = true;
                        break;
                    // 共通日時の場合
                    case ColumnName.ComDateTime:
                        val = DateTime.Now.ToString(reportInfo.FormatText);
                        setTitle = true;
                        break;
                    // 共通対象機能IDの場合
                    case ColumnName.ComConductId:
                        val = option.TargetConductId;
                        setTitle = true;
                        break;
                    // 共通対象シート番号の場合
                    case ColumnName.ComSheetNo:
                        val = option.TargetSheetNo;
                        setTitle = true;
                        break;
                    default:
                        continue;
                }

                // マッピング情報設定
                info.SetExlSetValueByAddress(address, val, format);
                // マッピングリストに追加
                mappingList.Add(info);
                if (setTitle)
                {
                    // タイトル列に項目名をセット
                    address = ToAlphabet((int)reportInfo.StartColNo - 1) + reportInfo.StartRowNo;
                    val = reportInfo.ItemName;

                    // マッピング情報設定
                    info = new CommonExcelPrtInfo();
                    info.SetSheetName(null); 
                    info.SetSheetNo(sheetNo);
                    info.SetExlSetValueByAddress(address, val, format);
                    // マッピングリストに追加
                    mappingList.Add(info);
                }
            }

            // 出力項目種別が3:出力項目固定（出力パターン指定あり※ベタ票）の場合、ヘッダをマッピング
            if (reportDefine.OutputItemType == ComReport.OutputItemType3)
            {
                // 項目定義
                foreach (var reportInfo in reportInfoList)
                {
                    // 開始行番号、開始列番号が未指定の場合、スキップ
                    if (reportInfo.StartRowNo == null || reportInfo.StartColNo == null)
                    {
                        continue;
                    }
                    // 初期化
                    var info = new CommonExcelPrtInfo();
                    info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
                    info.SetSheetNo(sheetNo); // シート番号に対象のシート番号を設定
                    // 出力方式
                    int startColNo = 0;
                    int startRowNo = 0;
                    switch (reportInfo.OutputMethod.GetValueOrDefault())
                    {
                        // 1:単一セルの場合
                        case (ComReport.SingleCell):
                            continue; // 何もしない
                        // 2:縦方向連続の場合
                        case (ComReport.LongitudinalDirection):
                            startColNo = (int)reportInfo.StartColNo;
                            startRowNo = reportInfo.StartRowNo.GetValueOrDefault() - 1; // 行がマイナス１
                            break;
                        // 3:横方向連続の場合
                        case (ComReport.LateralDirection):
                            startColNo = (int)reportInfo.StartColNo - 1;
                            startRowNo = reportInfo.StartRowNo.GetValueOrDefault(); // 行がマイナス１
                            break;
                        default:
                            continue; // 何もしない
                    }

                    // マッピングセルを設定
                    string address = ToAlphabet(startColNo) + startRowNo;
                    // フォーマット設定
                    string format = null;
                    object val;
                    // 項目名がヘッダタイトル
                    val = reportInfo.ItemName;
                    // マッピング情報設定
                    info.SetExlSetValueByAddress(address, val, format);
                    // マッピングリストに追加
                    mappingList.Add(info);
                }

            }

            // オプション指定のある場合
            if (option != null)
            {
                // スケジュールヘッダを作成
                // 項目定義
                foreach (var reportInfo in reportInfoList)
                {
                    // 開始行番号、開始列番号が未指定の場合、スキップ
                    if (reportInfo.StartRowNo == null || reportInfo.StartColNo == null)
                    {
                        continue;
                    }
                    // 入出力方式が縦横方向連続以外の場合、スキップ
                    if (reportInfo.OutputMethod != ComReport.LongitudinalLateralDirection)
                    {
                        continue;
                    }
                    // 初期化
                    var info = new CommonExcelPrtInfo();
                    info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
                    info.SetSheetNo(sheetNo); // シート番号に対象のシート番号を設定

                    int startColNo = (int)reportInfo.StartColNo;
                    // 指定セルの行をマイナス１
                    int startRowNo = reportInfo.StartRowNo.GetValueOrDefault() - 1;
                    // マッピングセルを設定
                    string address = ToAlphabet(startColNo) + startRowNo;
                    // フォーマット設定
                    string format = ComReport.StrFormat;
                    object val;

                    // 表示単位が 1:月度 の場合
                    if (option.DisplayUnit == (int)Const.MsStructure.StructureId.ScheduleDisplayUnit.Month)
                    {
                        // 出力方式が 3:予算別の場合
                        if (option.OutputMode == TMQUtil.ComReport.OutputMode3)
                        {
                            // 予算実績枠分を追加
                            val = "";
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, val, format);
                            // 列をプラス１
                            startColNo = startColNo + 1;
                            // 罫線用の変数も列をプラス１
                            optionColumnCount = optionColumnCount + 1;
                            // アドレスを変更
                            address = ToAlphabet(startColNo) + startRowNo;
                        }
                        // 年度開始月～12か月分
                        for (int i = 0; i < 12; i++)
                        {
                            if (option.MonthStartNendo + i <= 12)
                            {
                                val = option.MonthStartNendo + i;
                            }
                            else
                            {
                                val = option.MonthStartNendo + i - 12;
                            }
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, val, format);
                            // マッピングリストに追加
                            mappingList.Add(info);
                            // 列をプラス１
                            startColNo = startColNo + 1;
                            // 罫線用の変数も列をプラス１
                            optionColumnCount = optionColumnCount + 1;
                            // アドレスを変更
                            address = ToAlphabet(startColNo) + startRowNo;
                        }
                        // 出力方式が 3:予算別の場合
                        if (option.OutputMode == TMQUtil.ComReport.OutputMode3)
                        {
                            // 予算/実績をマッピング
                            // 開始列は帳票定義
                            startColNo = (int)reportInfo.StartColNo;
                            // 開始行も帳票定義
                            startRowNo = reportInfo.StartRowNo.GetValueOrDefault();
                            // 取得データ分ループ
                            foreach (var data in list)
                            {
                                // 予算を設定
                                val = "予算";
                                // アドレスを変更
                                address = ToAlphabet(startColNo) + startRowNo;
                                // マッピング情報設定
                                info.SetExlSetValueByAddress(address, val, format);
                                // 実績を設定
                                val = "実績";
                                // 行をプラス１
                                startRowNo = startRowNo + 1;
                                // アドレスを変更
                                address = ToAlphabet(startColNo) + startRowNo;
                                // マッピング情報設定
                                info.SetExlSetValueByAddress(address, val, format);
                                // 行をプラス１
                                startRowNo = startRowNo + 1;
                                // 罫線用の変数も行をプラス１
                                optionRowCount = optionRowCount + 1;
                            }
                        }

                    }
                    // 表示単位が 2:年度の場合
                    else if (option.DisplayUnit == (int)Const.MsStructure.StructureId.ScheduleDisplayUnit.Year)
                    {
                        // 出力方式が 3:予算別の場合
                        if (option.OutputMode == TMQUtil.ComReport.OutputMode3)
                        {
                            // 予算実績枠分を追加
                            val = "";
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, val, format);
                            // 列をプラス１
                            startColNo = startColNo + 1;
                            // 罫線用の変数も列をプラス１
                            optionColumnCount = optionColumnCount + 1;
                            // アドレスを変更
                            address = ToAlphabet(startColNo) + startRowNo;
                        }
                        // 開始年月日の年を取得
                        int startYear = option.StartDate.Year;
                        // 終了年月日の年を取得
                        int endYear = option.EndDate.Year;
                        for (int i = startYear; i < endYear; i++)
                        {
                            // 年を設定
                            val = i;
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, val, format);
                            // マッピングリストに追加
                            mappingList.Add(info);
                            // 列をプラス１
                            startColNo = startColNo + 1;
                            // 罫線用の変数も列をプラス１
                            optionColumnCount = optionColumnCount + 1;
                            // アドレスを変更
                            address = ToAlphabet(startColNo) + startRowNo;

                            // 出力方式が 3:予算別の場合
                            if (option.OutputMode == TMQUtil.ComReport.OutputMode3)
                            {
                                // 下期分の列を追加
                                val = "";
                                // マッピング情報設定
                                info.SetExlSetValueByAddress(address, val, format);
                                // マッピングリストに追加
                                mappingList.Add(info);
                                // 列をプラス１
                                startColNo = startColNo + 1;
                                // 罫線用の変数も列をプラス１
                                optionColumnCount = optionColumnCount + 1;
                                // アドレスを変更
                                address = ToAlphabet(startColNo) + startRowNo;
                            }
                        }
                        // 出力方式が 3:予算別の場合
                        if (option.OutputMode == TMQUtil.ComReport.OutputMode3)
                        {
                            // 上期/下期をマッピング
                            // 開始列は帳票定義プラス１
                            startColNo = (int)reportInfo.StartColNo + 1;
                            // 開始行は帳票定義
                            startRowNo = reportInfo.StartRowNo.GetValueOrDefault();
                            for (int i = startYear; i < endYear; i++)
                            {
                                // 上期を設定
                                val = "上期";
                                // アドレスを変更
                                address = ToAlphabet(startColNo) + startRowNo;
                                // マッピング情報設定
                                info.SetExlSetValueByAddress(address, val, format);
                                // 列をプラス１
                                startColNo = startColNo + 1;
                                // 下期を設定
                                val = "下期";
                                // アドレスを変更
                                address = ToAlphabet(startColNo) + startRowNo;
                                // マッピング情報設定
                                info.SetExlSetValueByAddress(address, val, format);
                                // 列をプラス１
                                startColNo = startColNo + 1;
                            }

                            // 予算/実績をマッピング
                            // 開始列は帳票定義
                            startColNo = (int)reportInfo.StartColNo;
                            // 開始行も帳票定義
                            startRowNo = reportInfo.StartRowNo.GetValueOrDefault();
                            // １行目は空白を設定
                            val = "";
                            // アドレスを変更
                            address = ToAlphabet(startColNo) + startRowNo;
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, val, format);
                            // 行をプラス１
                            startRowNo = startRowNo + 1;
                            // 取得データ分ループ
                            foreach (var data in list)
                            {
                                // 予算を設定
                                val = "予算";
                                // アドレスを変更
                                address = ToAlphabet(startColNo) + startRowNo;
                                // マッピング情報設定
                                info.SetExlSetValueByAddress(address, val, format);
                                // 実績を設定
                                val = "実績";
                                // 行をプラス１
                                startRowNo = startRowNo + 1;
                                // アドレスを変更
                                address = ToAlphabet(startColNo) + startRowNo;
                                // マッピング情報設定
                                info.SetExlSetValueByAddress(address, val, format);
                                // 行をプラス１
                                startRowNo = startRowNo + 1;
                                // 罫線用の変数も行をプラス１
                                optionRowCount = optionRowCount + 1;
                            }
                            // 上期/下期の１行分変数をプラス１
                            optionRowCount = optionRowCount + 1;
                        }
                    }
                }
            }

            // 項目定義
            foreach (var reportInfo in reportInfoList)
            {
                // 開始行番号、開始列番号が未指定の場合、スキップ
                if (reportInfo.StartRowNo == null || reportInfo.StartColNo == null)
                {
                    continue;
                }
                // カラム名が共通用タイトル、共通用日付、共通用時刻の場合、スキップ
                //if (reportInfo.ColumnName == ColumnName.ComTitle || reportInfo.ColumnName == ColumnName.ComDate || reportInfo.ColumnName == ColumnName.ComTime)
                if (ColumnName.IsCommonColumn(reportInfo.ColumnName))
                {
                    continue;
                }
                // カラム名が予算実績で出力方式が4:縦横方向連続の場合、スキップ
                if (reportInfo.ColumnName == ColumnName.BudgetResult && reportInfo.OutputMethod == ComReport.LongitudinalLateralDirection)
                {
                    continue;
                }
                int index = 0;

                // 出力結果分ループ
                foreach (var data in list)
                {
                    // 2行目以降、出力方式によって、マッピング位置を変更
                    if (index > 0)
                    {
                        switch (reportInfo.OutputMethod)
                        {
                            case ComReport.SingleCell:
                                // 単一セルの場合
                                continue;
                            case ComReport.LongitudinalDirection:
                                // 縦方向連続の場合、行番号を加算する
                                reportInfo.StartRowNo += reportInfo.ContinuousOutputInterval.GetValueOrDefault(1);
                                // 出力方式が 3:予算別の場合
                                if (option != null && option.OutputMode == TMQUtil.ComReport.OutputMode3)
                                {
                                    // 1行開ける
                                    reportInfo.StartRowNo += 1;
                                }
                                break;
                            case ComReport.LateralDirection:
                                // 横方向連続の場合、列番号を加算する
                                reportInfo.StartColNo += reportInfo.ContinuousOutputInterval.GetValueOrDefault(1);
                                break;
                            default:
                                // 入出力方式が未設定の場合、スキップ
                                break;
                        }
                    }

                    // 初期化
                    var info = new CommonExcelPrtInfo();
                    info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
                    info.SetSheetNo(sheetNo); // シート番号に対象のシート番号を設定

                    // Dictionaryにキャストする
                    var rowDic = (IDictionary<string, object>)data;
                    object val;
                    // 取得データに対象項目が存在するか
                    if (rowDic.ContainsKey(reportInfo.ColumnName))
                    {
                        val = rowDic[reportInfo.ColumnName];
                    }
                    else
                    {
                        // パスカルケースも
                        if (rowDic.ContainsKey(ComUtil.SnakeCaseToPascalCase(reportInfo.ColumnName)))
                        {
                            val = rowDic[ComUtil.SnakeCaseToPascalCase(reportInfo.ColumnName)];
                        }
                        else
                        {
                            // 取得データに対象項目名がない場合、空文字
                            val = "";
                        }
                    }
                    // マッピングセルを設定
                    string address = ToAlphabet((int)reportInfo.StartColNo) + reportInfo.StartRowNo;
                    // フォーマット設定
                    string format = null;

                    if (reportInfo.DataType != null && reportInfo.DataType == ComReport.DataTypeStr)
                    {
                        format = ComReport.StrFormat;
                    }
                    // 1行目の場合
                    if (index == 0)
                    {
                        // 出力方式が 3:予算別で表示単位が 2:年度の場合
                        if (option != null &&
                            option.OutputMode == TMQUtil.ComReport.OutputMode3 &&
                            option.DisplayUnit == (int)Const.MsStructure.StructureId.ScheduleDisplayUnit.Year)
                        {
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, "", format);
                            // マッピングリストに追加
                            mappingList.Add(info);
                            // 1行開ける
                            reportInfo.StartRowNo += 1;
                            // アドレス再設定
                            address = ToAlphabet((int)reportInfo.StartColNo) + reportInfo.StartRowNo;
                        }
                    }
                    // マッピング情報設定
                    info.SetExlSetValueByAddress(address, val, format);
                    // マッピングリストに追加
                    mappingList.Add(info);
                    // 行数を加算
                    index++;
                }
            }

            // オプション指定のある場合
            if (option != null && optionDataList != null)
            {
                // スケジュールを作成
                // 項目定義
                foreach (var reportInfo in reportInfoList)
                {
                    // 開始行番号、開始列番号が未指定の場合、スキップ
                    if (reportInfo.StartRowNo == null || reportInfo.StartColNo == null)
                    {
                        continue;
                    }
                    // 入出力方式が縦横方向連続以外の場合、スキップ
                    if (reportInfo.OutputMethod != ComReport.LongitudinalLateralDirection)
                    {
                        continue;
                    }
                    // 初期化
                    var info = new CommonExcelPrtInfo();
                    info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
                    info.SetSheetNo(sheetNo); // シート番号に対象のシート番号を設定

                    // 開始セルを設定
                    int startColNo = (int)reportInfo.StartColNo;
                    int startRowNo = reportInfo.StartRowNo.GetValueOrDefault();

                    // オプションデータ分ループ
                    foreach (var optionData in optionDataList)
                    {
                        int offsetCol = 0;
                        int offsetRow = 0;
                        int index = 0;
                        // 取得データ分ループ
                        foreach (var data in list)
                        {
                            // Dictionaryにキャストする
                            var rowDic = (IDictionary<string, object>)data;
                            // 取得データに対象項目が存在するか
                            if (rowDic.ContainsKey(ColumnName.KeyId))
                            {
                                // オプションデータのキーと取得データのキーが同じ場合
                                if (optionData.KeyId == rowDic[ColumnName.KeyId].ToString())
                                {
                                    // 列のオフセットを取得

                                    // 表示単位が 1:月度 の場合
                                    if (option.DisplayUnit == (int)Const.MsStructure.StructureId.ScheduleDisplayUnit.Month)
                                    {
                                        offsetCol = optionData.KeyDate.Month - option.StartDate.Month;
                                        if (offsetCol < 0)
                                        {
                                            offsetCol = offsetCol + 12;
                                        }
                                        // 出力方式が 3:予算別の場合
                                        if (option.OutputMode == TMQUtil.ComReport.OutputMode3)
                                        {
                                            // 予算/実績列があるのでオフセット＋１
                                            offsetCol = offsetCol + 1;
                                        }
                                    }
                                    // 表示単位が 2:年度の場合
                                    if (option.DisplayUnit == (int)Const.MsStructure.StructureId.ScheduleDisplayUnit.Year)
                                    {
                                        offsetCol = optionData.KeyDate.Year - option.StartDate.Year;
                                        // 出力方式が 3:予算別の場合
                                        if (option.OutputMode == TMQUtil.ComReport.OutputMode3)
                                        {
                                            // 予算は上期と下期があり、さらに予算/実績列があるのでオフセット２倍＋１
                                            offsetCol = (offsetCol * 2) + 1;
                                            // 対象の月 から 年度開始月 を引いて上期か下期を判定
                                            int monthDiff = optionData.KeyDate.Month - option.MonthStartNendo;
                                            if (monthDiff < 0)
                                            {
                                                monthDiff = monthDiff + 12;
                                            }
                                            // 下期であればプラス１
                                            if (monthDiff >= 6)
                                            {
                                                offsetCol = offsetCol + 1;
                                            }
                                        }
                                    }

                                    // 行のオフセットを取得

                                    // 出力方式が 3:予算別の場合
                                    if (option.OutputMode == TMQUtil.ComReport.OutputMode3)
                                    {
                                        // 表示単位が 1:月度の場合
                                        if (option.DisplayUnit == (int)Const.MsStructure.StructureId.ScheduleDisplayUnit.Month)
                                        {
                                            // 行数×２を加算
                                            offsetRow = startRowNo + (index * 2);
                                        }
                                        else
                                        {
                                            // 表示単位が 2:年度の場合行数×２＋１を加算
                                            offsetRow = startRowNo + (index * 2) + 1;
                                        }
                                    }
                                    else
                                    {
                                        // 予算でない場合は行数を加算
                                        offsetRow = startRowNo + index;
                                    }

                                    // マッピングセルを設定
                                    string address = ToAlphabet(startColNo + offsetCol) + offsetRow;
                                    // フォーマット設定
                                    string format = ComReport.StrFormat;
                                    object val;

                                    // 出力方式が 3:予算別の場合、記号ではなく、予算と実績を設定する。
                                    if (option.OutputMode == TMQUtil.ComReport.OutputMode3)
                                    {
                                        // 予算を設定する
                                        if (optionData.BudgetAmount == null)
                                        {
                                            val = null;
                                        }
                                        else
                                        {
                                            val = (optionData.BudgetAmount.GetValueOrDefault()).ToString("#,##0.###");
                                        }
                                        // マッピング情報設定
                                        info.SetExlSetValueByAddress(address, val, format);
                                        // マッピングリストに追加
                                        mappingList.Add(info);
                                        // 実績を設定する
                                        if (optionData.Expenditure == null)
                                        {
                                            val = null;
                                        }
                                        else
                                        {
                                            val = (optionData.Expenditure.GetValueOrDefault()).ToString("#,##0.###");
                                        }
                                        // 実績は１行下に設定
                                        address = ToAlphabet(startColNo + offsetCol) + (offsetRow + 1);
                                        // マッピング情報設定
                                        info.SetExlSetValueByAddress(address, val, format);
                                        // マッピングリストに追加
                                        mappingList.Add(info);
                                    }
                                    else
                                    {
                                        // スケジュールのマーク
                                        switch (optionData.StatusId)
                                        {
                                            // 保全履歴完了、●
                                            case Const.MsStructure.StructureId.ScheduleStatus.Complete:
                                                val = Const.MsStructure.StructureId.ScheduleStatusText.Complete;
                                                break;
                                            // 上位ランクが履歴完了済み、▲
                                            case Const.MsStructure.StructureId.ScheduleStatus.UpperComplete:
                                                val = Const.MsStructure.StructureId.ScheduleStatusText.UpperComplete;
                                                break;
                                            // 計画作成済み、◎
                                            case Const.MsStructure.StructureId.ScheduleStatus.Created:
                                                val = Const.MsStructure.StructureId.ScheduleStatusText.Created;
                                                break;
                                            // スケジュール済み、○
                                            case Const.MsStructure.StructureId.ScheduleStatus.NoCreate:
                                                val = Const.MsStructure.StructureId.ScheduleStatusText.NoCreate;
                                                break;
                                            // 上位ランクがスケジュール済み、△
                                            case Const.MsStructure.StructureId.ScheduleStatus.UpperScheduled:
                                                val = Const.MsStructure.StructureId.ScheduleStatusText.UpperScheduled;
                                                break;
                                            // なし、非表示
                                            case Const.MsStructure.StructureId.ScheduleStatus.NoSchedule:
                                                val = Const.MsStructure.StructureId.ScheduleStatusText.NoSchedule;
                                                break;
                                            default:
                                                val = Const.MsStructure.StructureId.ScheduleStatusText.NoSchedule;
                                                break;
                                        }
                                        // マッピング情報設定
                                        info.SetExlSetValueByAddress(address, val, format);
                                        // マッピングリストに追加
                                        mappingList.Add(info);
                                    }

                                }
                            }
                            // 行数を加算
                            index++;
                        }
                    }
                }
            }

            // マッピング情報リストを返却
            return mappingList;
        }

        /// <summary>
        /// ファイル マッピング情報
        /// </summary>
        /// <typeparam name="T">データクラス</typeparam>
        /// <param name="factoryId">工場ID</param>
        /// <param name="programId">プログラムID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="outputPattenId">出力パターンID</param>
        /// <param name="list">出力結果</param>
        /// <param name="conditionSheetLocatoinList">場所階層構成IDリスト</param>
        /// <param name="conditionSheetJobList">職種機種構成IDリスト</param>
        /// <param name="conditionSheetNameList">検索条件項目名リスト</param>
        /// <param name="conditionSheetValueList">検索条件設定値リスト</param>
        /// <param name="templateFileName">テンプレートファイル名</param>
        /// <param name="templateFilePath">テンプレートファイルパス</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="optionRowCount">オプション行数</param>
        /// <param name="optionColumnCount">オプション行数</param>
        /// <param name="option">オプションクラス</param>
        /// <param name="optionDataList">オプションクラス</param>
        /// <returns>ファイル マッピング情報リスト</returns>
        public static List<CommonExcelPrtInfo> CreateMappingListForCondition(
            int factoryId,
            string programId,
            string reportId,
            int sheetNo,
            int templateId,
            int outputPattenId,
            dynamic list,
            List<int> conditionSheetLocationList,
            List<int> conditionSheetJobList,
            List<string> conditionSheetNameList,
            List<string> conditionSheetValueList,
             string templateFileName,
            string templateFilePath,
            string languageId,
            ComDB db,
            out int optionRowCount,
            out int optionColumnCount,
            Option option = null,
            List<TMQDao.ScheduleList.Display> optionDataList = null)
        {
            // 初期化
            var mappingList = new List<CommonExcelPrtInfo>();
            var info = new CommonExcelPrtInfo();

            string address;
            optionRowCount = 0;
            optionColumnCount = 0;

            // 帳票定義を取得
            var reportDefine = new ReportDao.MsOutputReportDefineEntity().GetEntity(factoryId, programId, reportId, db);

            // シート定義を取得
            var sheetDefine = new ReportDao.MsOutputReportSheetDefineEntity().GetEntity(factoryId, reportId, sheetNo, db);
            if (sheetDefine == null)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // 出力行の制御
            long outputRowCount = 1;

            // 固定見出しをマッピング情報に追加
            info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
            info.SetSheetNo(sheetNo); // シート番号に対象のシート番号を設定

            // 検索条件
            info.SetExlSetValueByAddress("A" + outputRowCount.ToString(), GetTranslationText(111090032, languageId, db));
            mappingList.Add(info);
            outputRowCount += 1;

            // 場所階層
            info.SetExlSetValueByAddress("A" + outputRowCount.ToString(), GetTranslationText(111260021, languageId, db));
            mappingList.Add(info);
            // 地区
            info.SetExlSetValueByAddress("B" + outputRowCount.ToString(), GetTranslationText(111170008, languageId, db));
            mappingList.Add(info);
            // 工場
            info.SetExlSetValueByAddress("C" + outputRowCount.ToString(), GetTranslationText(111100012, languageId, db));
            mappingList.Add(info);
            // プラント
            info.SetExlSetValueByAddress("D" + outputRowCount.ToString(), GetTranslationText(111280011, languageId, db));
            mappingList.Add(info);
            // 系列
            info.SetExlSetValueByAddress("E" + outputRowCount.ToString(), GetTranslationText(111090018, languageId, db));
            mappingList.Add(info);
            // 工程
            info.SetExlSetValueByAddress("F" + outputRowCount.ToString(), GetTranslationText(111100013, languageId, db));
            mappingList.Add(info);
            // 設備
            info.SetExlSetValueByAddress("G" + outputRowCount.ToString(), GetTranslationText(111140018, languageId, db));
            mappingList.Add(info);

            // 場所階層
            if (conditionSheetLocationList.Count > 0)
            {
                // SQL取得
                GetFixedSqlStatement(SqlName.SubDir, SqlName.GetUpperStructureList, out string sqlText);
                // IDのリストより上位の階層を検索し、階層情報のリストを取得
                // 場所階層はカンマ区切りの文字列で渡す
                var param = new { LanguageId = languageId, StructureIdList = string.Join(",", conditionSheetLocationList) };

                IList<TMQUtil.StructureLayerInfo.StructureGetInfo> results = db.GetListByDataClass<TMQUtil.StructureLayerInfo.StructureGetInfo>(sqlText, param);
                if (results != null)
                {
                    var structureInfoList = results.ToList();
                    if (structureInfoList != null)
                    {
                        // 最上位の階層から最下層IDを取る
                        IList<TMQUtil.StructureLayerInfo.StructureGetInfo> structureLayerInfoList = structureInfoList.Where(x => x.StructureLayerNo == 0).ToArray();

                        // 最下層のリスト
                        IList<TMQUtil.StructureLayerInfo.StructureLocationInfoEx> bottomLayerAll = new List<TMQUtil.StructureLayerInfo.StructureLocationInfoEx>();

                        // 最上位の階層ごとに処理を繰り返す
                        foreach (TMQUtil.StructureLayerInfo.StructureGetInfo structureGetInfo in structureLayerInfoList)
                        {
                            TMQUtil.StructureLayerInfo.StructureLocationInfoEx temp = new();
                            temp.LocationStructureId = structureGetInfo.OrgStructureId;
                            bottomLayerAll.Add(temp);
                        }

                        // データクラスに地区及び職種の階層情報を設定する処理
                        TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<TMQUtil.StructureLayerInfo.StructureLocationInfoEx>(ref bottomLayerAll, new List<TMQUtil.StructureLayerInfo.StructureType> { TMQUtil.StructureLayerInfo.StructureType.Location }, db, languageId);
                        // 地区、工場、プラント、系列、工程、設備の順に並び替え(場所階層ツリーの表示順に合わせる)
                        var sortList = bottomLayerAll.OrderBy(x => x.DistrictId).ThenBy(x => x.FactoryId).ThenBy(x => x.PlantId).ThenBy(x => x.SeriesId)
                                                        .ThenBy(x => x.StrokeId).ThenBy(x => x.FacilityId).ToList();

                        for (int i = 0; i < sortList.Count; i++)
                        {
                            // 地区
                            // マッピングセルを設定
                            address = "B" + (outputRowCount + 1).ToString();
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, sortList[i].DistrictName);
                            // マッピングリストに追加
                            mappingList.Add(info);

                            // 工場
                            // マッピングセルを設定
                            address = "C" + (outputRowCount + 1).ToString();
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, sortList[i].FactoryName);
                            // マッピングリストに追加
                            mappingList.Add(info);

                            // プラント
                            // マッピングセルを設定
                            address = "D" + (outputRowCount + 1).ToString();
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, sortList[i].PlantName);
                            // マッピングリストに追加
                            mappingList.Add(info);

                            // 系列
                            // マッピングセルを設定
                            address = "E" + (outputRowCount + 1).ToString();
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, sortList[i].SeriesName);
                            // マッピングリストに追加
                            mappingList.Add(info);

                            // 工程
                            // マッピングセルを設定
                            address = "F" + (outputRowCount + 1).ToString();
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, sortList[i].StrokeName);
                            // マッピングリストに追加
                            mappingList.Add(info);

                            // 設備
                            // マッピングセルを設定
                            address = "G" + (outputRowCount + 1).ToString();
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, sortList[i].FacilityName);
                            // マッピングリストに追加
                            mappingList.Add(info);

                            outputRowCount += 1;
                        }
                    }
                }
            }
            // 職種・機種
            // 行をあける
            outputRowCount += 2;
            // 職種・機種
            info.SetExlSetValueByAddress("A" + outputRowCount.ToString(), GetTranslationText(111120081, languageId, db));
            mappingList.Add(info);
            // 職種
            info.SetExlSetValueByAddress("B" + outputRowCount.ToString(), GetTranslationText(111120002, languageId, db));
            mappingList.Add(info);
            // 機種大分類
            info.SetExlSetValueByAddress("C" + outputRowCount.ToString(), GetTranslationText(111070005, languageId, db));
            mappingList.Add(info);
            // 機種中分類
            info.SetExlSetValueByAddress("D" + outputRowCount.ToString(), GetTranslationText(111070006, languageId, db));
            mappingList.Add(info);
            // 機種小分類
            info.SetExlSetValueByAddress("E" + outputRowCount.ToString(), GetTranslationText(111070007, languageId, db));
            mappingList.Add(info);

            // 職種・機種
            if (conditionSheetJobList.Count > 0)
            {
                // SQL取得
                GetFixedSqlStatement(SqlName.SubDir, SqlName.GetUpperStructureList, out string sqlText2);
                // IDのリストより上位の階層を検索し、階層情報のリストを取得
                // 職種はカンマ区切りの文字列で渡す
                var param = new { LanguageId = languageId, StructureIdList = string.Join(",", conditionSheetJobList) };
                IList<TMQUtil.StructureLayerInfo.StructureGetInfo> results = db.GetListByDataClass<TMQUtil.StructureLayerInfo.StructureGetInfo>(sqlText2, param);
                if (results != null)
                {
                    var structureInfoList2 = results.ToList();

                    if (structureInfoList2 != null)
                    {
                        // 最上位の階層から最下層IDを取る
                        IList<TMQUtil.StructureLayerInfo.StructureGetInfo> structureLayerInfoList2 = structureInfoList2.Where(x => x.StructureLayerNo == 0).ToArray();

                        // 最下層のリスト
                        IList<TMQUtil.StructureLayerInfo.StructureJobInfoEx> bottomLayerAll2 = new List<TMQUtil.StructureLayerInfo.StructureJobInfoEx>();

                        // 最上位の階層ごとに処理を繰り返す
                        foreach (TMQUtil.StructureLayerInfo.StructureGetInfo structureGetInfo2 in structureLayerInfoList2)
                        {
                            TMQUtil.StructureLayerInfo.StructureJobInfoEx temp = new();
                            temp.JobStructureId = structureGetInfo2.OrgStructureId;
                            bottomLayerAll2.Add(temp);
                        }

                        // データクラスに地区及び職種の階層情報を設定する処理
                        TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<TMQUtil.StructureLayerInfo.StructureJobInfoEx>(ref bottomLayerAll2, new List<TMQUtil.StructureLayerInfo.StructureType> { TMQUtil.StructureLayerInfo.StructureType.Job }, db, languageId);
                        // 職種、機種大分類、機種中分類、機種小分類の順に並び替え(職種階層ツリーの表示順に合わせる)
                        var sortList2 = bottomLayerAll2.OrderBy(x => x.JobId).OrderBy(x => x.LargeClassficationId).ThenBy(x => x.MiddleClassficationId).ThenBy(x => x.SmallClassficationId).ToList();

                        for (int i = 0; i < sortList2.Count; i++)
                        {
                            // 職種
                            // マッピングセルを設定
                            address = "B" + (outputRowCount + 1).ToString();
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, sortList2[i].JobName);
                            // マッピングリストに追加
                            mappingList.Add(info);

                            // 機種大分類
                            // マッピングセルを設定
                            address = "C" + (outputRowCount + 1).ToString();
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, sortList2[i].LargeClassficationName);
                            // マッピングリストに追加
                            mappingList.Add(info);

                            // 機種中分類
                            // マッピングセルを設定
                            address = "D" + (outputRowCount + 1).ToString();
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, sortList2[i].MiddleClassficationName);
                            // マッピングリストに追加
                            mappingList.Add(info);

                            // 機種小分類
                            // マッピングセルを設定
                            address = "E" + (outputRowCount + 1).ToString();
                            // マッピング情報設定
                            info.SetExlSetValueByAddress(address, sortList2[i].SmallClassficationName);
                            // マッピングリストに追加
                            mappingList.Add(info);

                            outputRowCount += 1;
                        }
                    }
                }
            }
            // 行をあける
            outputRowCount += 2;

            // 検索条件(項目名、設定値)
            if ((conditionSheetNameList != null) && (conditionSheetNameList.Count >= 1))
            {
                // 固定見出しをマッピング情報に追加 
                info.SetExlSetValueByAddress("A" + outputRowCount.ToString(), GetTranslationText(111100030, languageId, db));
                mappingList.Add(info);
                info.SetExlSetValueByAddress("B" + outputRowCount.ToString(), GetTranslationText(111120218, languageId, db));
                mappingList.Add(info);

                outputRowCount += 1;

                // 項目定義
                for (int i = 0; i < conditionSheetNameList.Count; i++)
                {
                    info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
                    info.SetSheetNo(sheetNo); // シート番号に対象のシート番号を設定
                    // マッピングセルを設定
                    address = "A" + (i + outputRowCount).ToString();
                    // マッピング情報設定
                    info.SetExlSetValueByAddress(address, conditionSheetNameList[i]);
                    // マッピングリストに追加
                    mappingList.Add(info);

                    // マッピングセルを設定
                    address = "B" + (i + outputRowCount).ToString();
                    // マッピング情報設定
                    info.SetExlSetValueByAddress(address, conditionSheetValueList[i]);
                    // マッピングリストに追加
                    mappingList.Add(info);
                }
            }

            // マッピング情報リストを返却
            return mappingList;

        }

        /// <summary>
        /// 開始情報設定
        /// </summary>
        /// <param name="reportInfoList">シート番号</param>
        /// <param name="templateFileName">テンプレートファイル名</param>
        /// <param name="templateFilePath">テンプレートファイルパス</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="sheetDefineMaxRow">シート定義最大行</param>
        /// <param name="sheetDefineMaxColumn">シート定義最大列</param>
        /// <param name="outputItemType">出力項目種別</param>
        public static void SetStartInfo(
            ref IList<Dao.InoutDefine> reportInfoList,
            string templateFileName,
            string tempFilePath,
            int sheetNo,
            int sheetDefineMaxRow,
            int sheetDefineMaxColumn,
            int outputItemType)
        {
            // 実行パス取得
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            // テンプレートフォルダ取得
            string templateFolder = Path.Combine(appPath, CommonWebTemplate.AppCommonObject.Config.AppSettings.ExcelTemplateDir);
            templateFolder = Path.Combine(templateFolder, tempFilePath);
            // コンストラクタ
            CommonExcelCmd cmd = new CommonExcelCmd(Path.Combine(templateFolder, templateFileName));

            string msg = string.Empty;
            string[,] vals = null;
            // マッピングセルを設定
            string address = "A1:" + ToAlphabet(sheetDefineMaxColumn) + sheetDefineMaxRow;
            // エクセル対象シートを読込
            cmd.ReadExcelBySheetNo(sheetNo, address, ref vals, ref msg);

            if (vals == null || vals.Length == 0)
            {
                return;
            }

            foreach (var reportInfo in reportInfoList)
            {
                bool itemFlag = false;
                // 出力マスタに開始情報が設定されている場合
                if (reportInfo.OutputDefaultCellRowNo != null && reportInfo.OutputDefaultCellColumnNo != null)
                {
                    // 対象セルに「@」+ 項目名が設定されている、または
                    // 出力項目種別が3:出力項目固定（出力パターン指定あり※ベタ票）の場合
                    if (vals[reportInfo.OutputDefaultCellRowNo.GetValueOrDefault() - 1, reportInfo.OutputDefaultCellColumnNo.GetValueOrDefault() - 1] == "@" + reportInfo.ItemName ||
                        outputItemType == ComReport.OutputItemType3)
                    {
                        // 開始情報を設定する
                        reportInfo.StartRowNo = reportInfo.OutputDefaultCellRowNo;
                        reportInfo.StartColNo = reportInfo.OutputDefaultCellColumnNo;
                        continue;
                    }

                }
                // 定義マスタに開始情報が設定されている場合
                if (reportInfo.DefaultCellRowNo != null && reportInfo.DefaultCellColumnNo != null)
                {
                    // 対象セルに「@」+ 項目名が設定されている、
                    // または出力項目種別が3:出力項目固定（出力パターン指定あり※ベタ票）の場合
                    if (vals[reportInfo.DefaultCellRowNo.GetValueOrDefault() - 1, reportInfo.DefaultCellColumnNo.GetValueOrDefault() - 1] == "@" + reportInfo.ItemName ||
                        outputItemType == ComReport.OutputItemType3)
                    {
                        // 開始情報を設定する
                        reportInfo.StartRowNo = reportInfo.DefaultCellRowNo;
                        reportInfo.StartColNo = reportInfo.DefaultCellColumnNo;
                        continue;
                    }
                }
                // エクセルシート分ループ
                // 行
                for (int i = 0; i < vals.GetLongLength(0); i++)
                {
                    if (itemFlag == true)
                    {
                        continue;
                    }
                    // 列
                    for (int j = 0; j < vals.GetLongLength(1); j++)
                    {
                        if (itemFlag == true)
                        {
                            continue;
                        }
                        // "@" + 項目IDと同じ値があった場合、開始情報を設定
                        if (vals[i, j] == "@" + reportInfo.ItemName)
                        {
                            reportInfo.StartRowNo = i + 1;
                            reportInfo.StartColNo = j + 1;
                            itemFlag = true;
                            continue;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 開始情報設定（Excelファイルのみからの設定）
        /// </summary>
        /// <param name="reportInfoList">シート番号</param>
        /// <param name="templateFileName">テンプレートファイル名</param>
        /// <param name="templateFilePath">テンプレートファイルパス</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="sheetDefineMaxRow">シート定義最大行</param>
        /// <param name="sheetDefineMaxColumn">シート定義最大列</param>
        /// <param name="outputItemType">出力項目種別</param>
        public static void SetStartInfoByExcel(
            ref IList<Dao.InoutDefine> reportInfoList,
            string templateFileName,
            string tempFilePath,
            int sheetNo,
            int sheetDefineMaxRow,
            int sheetDefineMaxColumn,
            int outputItemType)
        {
            // 実行パス取得
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            // テンプレートフォルダ取得
            string templateFolder = Path.Combine(appPath, CommonWebTemplate.AppCommonObject.Config.AppSettings.ExcelTemplateDir);
            templateFolder = Path.Combine(templateFolder, tempFilePath);
            // コンストラクタ
            CommonExcelCmd cmd = new CommonExcelCmd(Path.Combine(templateFolder, templateFileName));

            string msg = string.Empty;
            string[,] vals = null;
            // マッピングセルを設定
            string address = "A1:" + ToAlphabet(sheetDefineMaxColumn) + sheetDefineMaxRow;
            // エクセル対象シートを読込
            cmd.ReadExcelBySheetNo(sheetNo, address, ref vals, ref msg);

            if (vals == null || vals.Length == 0)
            {
                return;
            }

            foreach (var reportInfo in reportInfoList)
            {
                bool itemFlag = false;
                // エクセルシート分ループ
                // 行
                for (int i = 0; i < vals.GetLongLength(0); i++)
                {
                    if (itemFlag == true)
                    {
                        continue;
                    }
                    // 列
                    for (int j = 0; j < vals.GetLongLength(1); j++)
                    {
                        if (itemFlag == true)
                        {
                            continue;
                        }
                        // "@" + 項目IDと同じ値があった場合、開始情報を設定
                        if (vals[i, j] == "@" + reportInfo.ItemName)
                        {
                            reportInfo.StartRowNo = i + 1;
                            reportInfo.StartColNo = j + 1;
                            itemFlag = true;
                            continue;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// テンプレートファイルアップロード
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="templateFileName">テンプレートファイル名</param>
        /// <param name="inputStream">入力ファイル情報</param>
        /// <param name="useUserId">使用ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        public static bool UploadTemplateFile(
            int factoryId, string reportId, string templateFileName, IFormFile[] inputStream, int useUserId, ComDB db)
        {
            // 実行パス取得
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            // テンプレートフォルダ取得
            string templateFolder = Path.Combine(appPath, CommonWebTemplate.AppCommonObject.Config.AppSettings.ExcelTemplateDir);
            // フォルダー存在チェック
            if (!Directory.Exists(templateFolder))
            {
                // フォルダーがなければ作成
                Directory.CreateDirectory(templateFolder);
            }
            // ファイル存在チェック
            if (File.Exists(Path.Combine(templateFolder, templateFileName)))
            {
                // ファイルが存在したらエラー
                return false;
            }
            var uploadFile = inputStream[0];
            // ファイルアップロード
            var stream = System.IO.File.Create(Path.Combine(templateFolder, templateFileName));
            uploadFile.CopyTo(stream);

            // 出力テンプレートマスタに登録
            //ReportDao.MsOutputTemplateEntity regist = new ReportDao.MsOutputTemplateEntity
            //{
            //    FactoryId = factoryId, // 工場ID
            //    ReportId = reportId, // 帳票ID
            //    TemplateId = templateId, // テンプレートID
            //    TemplateFileName = templateFileName, // テンプレートファイル名
            //    UseUserId = useUserId
            //};

            // 登録


            return true;
        }

        /// <summary>
        /// テンプレートファイル名取得処理
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>テンプレートファイル名</returns>
        public static string GetTmplateFileName(int factoryId, string reportId, int templateId, ComDB db)
        {
            // 機能ID、ファイル管理NOから該当のテンプレートファイル名を取得
            ReportDao.MsOutputTemplateEntity fileInfo = new ReportDao.MsOutputTemplateEntity();
            fileInfo = fileInfo.GetEntity(factoryId, reportId, templateId, db);

            // 該当のレコードが存在しない場合、nullを戻す
            if (fileInfo == null)
            {
                return null;
            }

            return fileInfo.TemplateFileName;
        }

        /// <summary>
        /// 翻訳文字列取得処理
        /// </summary>
        /// <param name="translationId">翻訳ID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>翻訳文字列</returns>
        public static string GetTranslationText(int translationId, string languageId, ComDB db, int factoryId = -1)
        {
            // 翻訳ID から翻訳文字列を取得
            string[] messageIdList = new string[1];
            messageIdList[0] = translationId.ToString();
            List<int> factoryIdList = null;
            if(factoryId >= 0)
            {
                factoryIdList = new List<int>() { factoryId };
            }
            ComUtil.MessageResources message = ComUtil.GetMessageResourceFromDb(db, languageId, messageIdList, factoryIdList);

            // 該当のレコードが存在しない場合、nullを戻す
            if (message == null)
            {
                return null;
            }

            return message.GetMessage(translationId.ToString(), languageId);

        }

        /// <summary>
        /// シート名取得処理
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>シート名</returns>
        public static string GetSheetName(int factoryId, string reportId, int sheetNo, string languageId, ComDB db)
        {
            // 出力帳票シート定義を取得
            ReportDao.MsOutputReportSheetDefineEntity sheetDefine = new ReportDao.MsOutputReportSheetDefineEntity();
            sheetDefine = sheetDefine.GetEntity(factoryId, reportId, sheetNo, db);

            return GetSheetName(sheetDefine, factoryId, languageId, db);
        }

        /// <summary>
        /// シート名取得処理
        /// </summary>
        /// <param name="reportId">出力帳票シート定義</param>
        /// <param name="factoryId">工場ID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>シート名</returns>
        public static string GetSheetName(ReportDao.MsOutputReportSheetDefineEntity sheetDefine, int factoryId, string languageId, ComDB db)
        {
            // シート定義のシート名翻訳ID からシート名を取得
            return GetTranslationText(sheetDefine.SheetNameTranslationId, languageId, db, factoryId);
        }

        /// <summary>
        /// 対象セル範囲を取得（縦方向連続方向の帳票のみ対応）
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="outputPatternId">出力パターンID</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="cnt">件数</param>
        /// <param name="recordCount">レコード行数</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="optionRowCount">オプション行数</param>
        /// <param name="optionColumnCount">オプション列数</param>
        /// <param name="printAreaFlag">印刷範囲フラグ</param>
        /// <returns>対象セル範囲</returns>
        public static string GetTargetCellRange(
            int factoryId,
            string reportId,
            int templateId,
            int outputPatternId,
            int sheetNo,
            int cnt,
            int? recordCount,
            ComDB db,
            int optionRowCount = 0,
            int optionColumnCount = 0,
            bool printAreaFlag = false)
        {
            int? startCol = null;
            int? endCol = null;
            int? startRow = null;

            // 1レコード分の行数を取得する
            if (recordCount == null)
            {
                return null;
            }
            int addRow = recordCount.GetValueOrDefault();

            // 開始列を設定
            startCol = TMQUtil.SqlExecuteClass.SelectEntity<int?>(
                ComReport.GetMinColumnNo,
                ExcelPath,
                new { FactoryId = factoryId, ReportId = reportId, TemplateId = templateId, OutputPatternId = outputPatternId, SheetNo = sheetNo },
                db);
            if (startCol == null)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // 終了列を設定
            endCol = TMQUtil.SqlExecuteClass.SelectEntity<int?>(
                ComReport.GetMaxColumnNo,
                ExcelPath,
                new { FactoryId = factoryId, ReportId = reportId, TemplateId = templateId, OutputPatternId = outputPatternId, SheetNo = sheetNo },
                db);
            if (endCol == null)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // 開始行を設定
            startRow = TMQUtil.SqlExecuteClass.SelectEntity<int?>(
                ComReport.GetMaxRowNo,
                ExcelPath,
                new { FactoryId = factoryId, ReportId = reportId, TemplateId = templateId, OutputPatternId = outputPatternId, SheetNo = sheetNo },
                db);
            if (startRow == null)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // 開始行列、終了列を取得できなかった場合、nullを戻す
            if (startCol == null && endCol == null && startRow == null)
            {
                return null;
            }

            // 縦方向一覧の場合は開始行をマイナス１（ヘッダも罫線を引くため）
            startRow = startRow - 1;

            if (printAreaFlag == false)
            {
                // 範囲を返す
                return ToAlphabet((int)startCol) + startRow + ":" + ToAlphabet((int)endCol + optionColumnCount) + (startRow + (addRow * cnt) + optionRowCount);

            }
            else
            {
                // 印刷範囲（A1～）を返す
                return "A1:" + ToAlphabet((int)endCol + optionColumnCount) + (startRow + (addRow * cnt) + optionRowCount);
            }
        }

        /// <summary>
        /// コマンド情報取得（縦方向連続方向の帳票のみ対応）
        /// </summary>
        /// <param name="factoryId">機能ID</param>
        /// <param name="reportId">帳票番号</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="cnt">件数</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>コマンド情報</returns>
        public static List<CommonExcelCmdInfo> CreateCmdInfoList(int factoryId, string reportId, int sheetNo, int cnt, string languageId, ComDB db)
        {
            string sheetName = null;
            int? startCol = null;
            int? endCol = null;
            int? startRow = null;
            int recordCount = 1;

            // 初期化
            var cmdInfoList = new List<CommonExcelCmdInfo>();

            // シート名取得
            sheetName = GetSheetName(factoryId, reportId, sheetNo, languageId, db);

            // ファイル入出力項目定義情報を取得
            IList<Dao.InoutDefine> reportInfoList = db.GetListByOutsideSqlByDataClass<Dao.InoutDefine>(ComReport.GetComReportInfo, ExcelPath,
                new { FactoryId = factoryId, ReportId = reportId, Inputflg = false });
            if (reportInfoList == null)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // 開始列を設定
            IList<Dao.InoutDefine> workList = reportInfoList.Where(x => x.DataDirection != null && !x.DataDirection.Equals(ComReport.SingleCell)).OrderBy(x => x.StartColNo).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.StartColNo))
                {
                    continue;
                }
                startCol = (int)work.StartColNo;
                break; // 取得できた場合、処理を抜ける
            }

            // 終了列を設定
            workList = reportInfoList.Where(x => x.DataDirection != null && !x.DataDirection.Equals(ComReport.SingleCell)).OrderByDescending(x => x.StartColNo).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.StartColNo))
                {
                    continue;
                }
                endCol = (int)work.StartColNo;
                break; // 取得できた場合、処理を抜ける
            }

            // 開始行を設定
            workList = reportInfoList.Where(x => x.DataDirection != null && !x.DataDirection.Equals(ComReport.SingleCell)).OrderBy(x => x.StartRowNo).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.StartRowNo))
                {
                    continue;
                }
                startRow = (int)work.StartRowNo;
                break; // 取得できた場合、処理を抜ける
            }

            // レコード行数を設定
            workList = reportInfoList.Where(x => x.DataDirection != null && !x.DataDirection.Equals(ComReport.SingleCell)).OrderBy(x => x.StartRowNo).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.RecordCount))
                {
                    continue;
                }
                recordCount = (int)work.RecordCount;
                break; // 取得できた場合、処理を抜ける
            }

            // シート名、開始行、終了列を取得できなかった場合、nullを戻す
            if (sheetName == null || endCol == null || startRow == null)
            {
                return null;
            }

            // ベタ表コマンド情報作成
            cmdInfoList.AddRange(ExcelUtil.CreateCmdInfoListForSimpleList(sheetName, cnt, (int)startRow, ToAlphabet((int)startCol), ToAlphabet((int)endCol), recordCount));

            return cmdInfoList;
        }

        /// <summary>
        /// 数字→文字列
        /// </summary>
        /// <param name="columnNo">列番号(数字)</param>
        /// <returns>アルファベット</returns>
        public static string ToAlphabet(int columnNo)
        {
            //// A ～ YZまでを想定
            //string alphabet = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
            //string columnStr = string.Empty;
            //int m, n = 0;

            //m = columnNo % 26; // 一の位
            //n = columnNo / 26; // 十の位

            //// 一の位がZだった場合
            //if (m == 0)
            //{
            //    n = n - 1;
            //}

            //columnStr = alphabet[m].ToString();

            //// 指定列がAA以降の場合
            //if (n != 0 && columnNo != 0)
            //{
            //    columnStr = alphabet[n] + columnStr;
            //}

            //return columnStr;

            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string columnStr = "";
            for (; columnNo > 0; columnNo = (columnNo - 1) / 26)
            {
                int n = (columnNo - 1) % 26;
                columnStr = alpha.Substring(n, 1) + columnStr;
            }
            return columnStr;

        }

        /// <summary>
        /// リソースメッセージ取得
        /// </summary>
        /// <param name="key">キー情報</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <returns>メッセージ</returns>
        private static string GetResMessage(string key, string languageId, ComUtil.MessageResources msgResources)
        {
            return ComUtil.GetPropertiesMessage(key, languageId, msgResources);
        }

        /// <summary>
        /// リソースメッセージ取得
        /// </summary>
        /// <param name="keys">キー情報</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <returns>メッセージ</returns>
        private static string GetResMessage(string[] keys, string languageId, ComUtil.MessageResources msgResources)
        {
            return ComUtil.GetPropertiesMessage(keys, languageId, msgResources);
        }

        /// <summary>
        /// コマンド情報生成：自動調整
        /// </summary>
        /// <param name="cellRange">セル範囲</param>
        /// <param name="sheetName">シート名</param>
        /// <returns>コマンド情報</returns>
        private static List<CommonExcelCmdInfo> CommandLineBox(string cellRange, string sheetName)
        {
            List<CommonExcelCmdInfo> cmdInfoList = new List<CommonExcelCmdInfo>();

            for (int i = 0; i < 7; i++)
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
                    //case 5:
                    //    // 内側水平罫線
                    //    borderIndex = "IH";
                    //    break;
                    //default:
                    //    // 内側垂直罫線
                    //    borderIndex = "IV";
                    //    break;
                    case 5:
                        // 内側に作成
                        borderIndex = "I";
                        break;
                    case 6:
                        // 外枠
                        borderIndex = "O";
                        break;
                    default:
                        // 格子
                        borderIndex = "IO";
                        break;
                }

                param[0] = cellRange; // [0]：セル範囲
                //param[1] = ""; // [1]：罫線の太さ、デフォルトは細線
                //param[2] = borderIndex; // [2]：罫線の作成位置
                param[1] = borderIndex; // [2]：罫線の作成位置
                param[2] = ""; // [1]：罫線の太さ、デフォルトは細線
                param[3] = sheetName; // [3]：シート名　デフォルトは先頭シート
                cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdLineBox, param);
                cmdInfoList.Add(cmdInfo);
            }

            return cmdInfoList;
        }

        /// <summary>
        /// 取込ファイル読込処理
        /// </summary>
        /// <param name="fileStream">読込対象エクセルStream</param>
        /// <param name="delimiter">デリミタ文字</param>
        /// <returns>ファイル情報</returns>
        public static CommonExcelCmd FileOpen(Stream fileStream, string delimiter = null)
        {
            CommonExcelCmd cmd = new CommonExcelCmd(fileStream);

            return cmd;
        }

        /// <summary>
        /// 取込（EXCEL）共通チェック処理
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="cmd">エクセルコマンド処理クラス</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="controlGroupId">コントロールグループID</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="resultList">結果格納クラス</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="checkFlg">省略可能　チェックフラグ　省略時はチェック有り</param>
        /// <returns>エラー情報</returns>
        public static List<ComBase.UploadErrorInfo> ComUploadErrorCheck<T>(
            CommonExcelCmd cmd, string reportId, int sheetNo, string controlGroupId, ref List<T> resultList,
            string languageId, ComUtil.MessageResources msgResources, ComDB db, bool checkFlg = true)
        {
            // エラー内容格納クラス
            List<ComBase.UploadErrorInfo> errorInfo = new List<ComBase.UploadErrorInfo>();

            // ファイル入力項目定義情報を取得
            ComBase.InputDefineCondition param = new ComBase.InputDefineCondition();
            param.ReportId = reportId;
            param.SheetNo = sheetNo;
            param.ControlGroupId = controlGroupId;
            param.LanguageId = languageId;
            param.FactoryId = Const.CommonFactoryId;
            IList<ComBase.InputDefine> reportInfoList = TMQUtil.SqlExecuteClass.SelectList<ComBase.InputDefine>(ComReport.GetInputControlDefine, ExcelPath, param, db);
            if (reportInfoList == null || reportInfoList.Count == 0)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // 検索結果クラスのプロパティを列挙
            var properites = typeof(T).GetProperties();
            // 1レコード分の行数、1レコード分の行数を取得する
            int addRow = reportInfoList[0].RecordCount;
            // 入力方式を取得
            int dataDirection = reportInfoList[0].DataDirection;

            int index = 0;
            while (true)
            {
                // エラー内容一時格納クラス
                List<ComBase.UploadErrorInfo> tmpErrorInfo = new List<ComBase.UploadErrorInfo>();

                bool flg = false; // データ存在チェックフラグ
                object tmpResult = Activator.CreateInstance<T>();

                // 取得できた項目定義分処理を行う
                foreach (ComBase.InputDefine reportInfo in reportInfoList)
                {
                    // 2行目以降、入出力方式によって、表示位置をずらす
                    if (index > 0)
                    {
                        switch (dataDirection)
                        {
                            case ComReport.SingleCell:
                                // 基本、到達しない
                                continue;
                            case ComReport.LongitudinalDirection:
                                // 縦方向連続の場合、行番号を加算する
                                reportInfo.StartRowNo += addRow;
                                break;
                            case ComReport.LateralDirection:
                                // 横方向連続の場合、列番号を加算する
                                reportInfo.StartColumnNo += addRow;
                                break;
                            default:
                                // 入出力方式が未設定の場合、スキップ
                                break;
                        }
                    }

                    string error = string.Empty;
                    string[,] vals = null;
                    string msg = string.Empty;
                    // マッピングセルを設定
                    string address = ToAlphabet(reportInfo.StartColumnNo) + reportInfo.StartRowNo;
                    // セル単位でデータを取得する
                    if (!cmd.ReadExcelBySheetNo(sheetNo, address, ref vals, ref msg))
                    {
                        // 読込失敗した場合、エラー内容を設定
                        // 「該当セルの読込に失敗しました。」
                        error = GetResMessage(ComRes.ID.ID941060006, languageId, msgResources);
                        tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                        setErrorInfo(ref errorInfo, tmpErrorInfo);
                        // レイアウトがおかしい場合、読み込みが無限ループの恐れがあるため、終了
                        return errorInfo;
                    }

                    // 設定値を取得
                    string val = vals[0, 0]; // セル単位で取得しているので先頭を対象データとみなす。

                    if (checkFlg)
                    {
                        // 値が取得できない場合、スキップ
                        if (string.IsNullOrEmpty(val))
                        {
                            if (reportInfo.RequiredFlg != null && (bool)reportInfo.RequiredFlg)
                            {
                                // 必須入力項目の場合、エラー内容を設定
                                // 「必須項目です。入力してください。」
                                error = GetResMessage(ComRes.ID.ID941270001, languageId, msgResources);
                                tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                            }
                            continue;
                        }
                    }

                    // 入力項目が存在する場合、フラグをたてる
                    flg = true;

                    if (checkFlg)
                    {
                        //// 入力チェック
                        //// 数値の場合、指数表記の可能性があるので、変換を実施
                        //if (reportInfo.DataType == ComReport.DataTypeInt || reportInfo.DataType == ComReport.DataTypeNum)
                        //{
                        //    if (val != null && ComUtil.ConvertDecimal(val) != null)
                        //    {
                        //        val = ComUtil.ConvertDecimal(val).ToString();
                        //    }
                        //}
                        //if (!checkCellType(reportInfo.DataType, val, languageId, msgResources, ref error))
                        //{
                        //    // 型が異なる場合、エラーを設定し、スキップ
                        //    tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                        //    continue;
                        //}

                        //// 桁数チェック
                        //if (reportInfo.MaximumLength != null && reportInfo.MaximumLength > 0)
                        //{
                        //    // セルタイプが指定され、日付以外の場合、チェックを行う
                        //    if (reportInfo.DataType != ComReport.DataTypeDat)
                        //    {
                        //        // 桁数を超えている場合、エラーを設定を設定し、スキップ
                        //        if (val.Length > reportInfo.MaximumLength)
                        //        {
                        //            // 「入力値が設定桁数を超えています。」
                        //            error = GetResMessage(new string[] { ComRes.ID.ID941220008 }, languageId, msgResources);
                        //            tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                        //            continue;
                        //        }
                        //    }
                        //}

                        //// 数値の場合、上下限チェック
                        //// 入力チェック
                        //if (reportInfo.DataType == ComReport.DataTypeInt || reportInfo.DataType == ComReport.DataTypeNum)
                        //{
                        //    if (!checkRange(reportInfo, val, db, languageId, msgResources, ref error))
                        //    {
                        //        // 範囲エラーの場合、エラーを設定し、スキップ
                        //        tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                        //        continue;
                        //    }
                        //}
                        // アップロード共通チェック実行
                        if(ExecuteCommonUploadCheck(reportInfo, val, dataDirection, languageId, msgResources, db, tmpErrorInfo))
                        {
                            continue;
                        }
                    }

                    // 値をデータクラスに設定
                    string pascalItemName = ComUtil.SnakeCaseToPascalCase(reportInfo.AliasName != null ? reportInfo.AliasName : reportInfo.ColumnName).ToUpper();
                    var prop = properites.FirstOrDefault(x => x.Name.ToUpper().Equals(pascalItemName));
                    if (prop == null)
                    {
                        // 該当する項目が存在しない場合、スキップ
                        continue;
                    }
                    ComUtil.SetPropertyValue<T>(prop, (T)tmpResult, val);
                }

                // データが1件も取得できなかった場合、処理を抜ける
                if (!flg)
                {
                    break;
                }

                //エラーがある場合、エラーフラグを立てる
                var errProp = properites.FirstOrDefault(x => x.Name.ToUpper().Equals("ErrorFlg"));
                if (errProp != null && tmpErrorInfo.Count > 0)
                {
                    ComUtil.SetPropertyValue<T>(errProp, (T)tmpResult, true);
                }

                // データが存在する場合、リストに追加する
                setErrorInfo(ref errorInfo, tmpErrorInfo);
                resultList.Add((T)tmpResult);
                index++;

                // 入力方式が単一セルの場合、処理を抜ける
                if ((int)dataDirection == ComReport.SingleCell)
                {
                    break;
                }
            }

            return errorInfo;
        }

        /// <summary>
        /// アップロード時共通チェック
        /// </summary>
        /// <param name="reportInfo">ファイル入力項目定義情報</param>
        /// <param name="val">セル値</param>
        /// <param name="dataDirection">入力方式</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="tmpErrorInfo">エラー情報リスト</param>
        /// <returns></returns>
        public static bool ExecuteCommonUploadCheck(
            ComBase.InputDefine reportInfo, 
            string val, 
            int dataDirection, 
            string languageId, 
            ComUtil.MessageResources msgResources, 
            ComDB db,
            List<ComBase.UploadErrorInfo> tmpErrorInfo)
        {
            string error = string.Empty;
            // 入力チェック
            // 数値の場合、指数表記の可能性があるので、変換を実施
            if (reportInfo.DataType == ComReport.DataTypeInt || reportInfo.DataType == ComReport.DataTypeNum)
            {
                if (val != null && ComUtil.ConvertDecimal(val) != null)
                {
                    val = ComUtil.ConvertDecimal(val).ToString();
                }
            }
            if (!checkCellType(reportInfo.DataType, val, languageId, msgResources, ref error))
            {
                // 型が異なる場合、エラーを設定し、スキップ
                tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                return false;
            }

            // 桁数チェック
            if (reportInfo.MaximumLength != null && reportInfo.MaximumLength > 0)
            {
                // セルタイプが指定され、日付以外の場合、チェックを行う
                if (reportInfo.DataType != ComReport.DataTypeDat)
                {
                    // 桁数を超えている場合、エラーを設定を設定し、スキップ
                    if (val.Length > reportInfo.MaximumLength)
                    {
                        // 「入力値が設定桁数を超えています。」
                        error = GetResMessage(new string[] { ComRes.ID.ID941220008 }, languageId, msgResources);
                        tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                        return false;
                    }
                }
            }

            // 数値の場合、上下限チェック
            // 入力チェック
            if (reportInfo.DataType == ComReport.DataTypeInt || reportInfo.DataType == ComReport.DataTypeNum)
            {
                if (!checkRange(reportInfo, val, db, languageId, msgResources, ref error))
                {
                    // 範囲エラーの場合、エラーを設定し、スキップ
                    tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// エラー情報設定
        /// </summary>
        /// <param name="rowNo">行番号</param>
        /// <param name="columnNo">列番号</param>
        /// <param name="columnName">項目名</param>
        /// <param name="error">エラー内容</param>
        /// <param name="dataDirection">入力方式</param>
        /// <returns>エラー情報</returns>
        private static ComBase.UploadErrorInfo setTmpErrorInfo(int rowNo, int columnNo, string columnName, string error, int dataDirection)
        {
            // エラー情報を初期化
            ComBase.UploadErrorInfo errorInfo = new ComBase.UploadErrorInfo();

            // エラー情報を設定
            errorInfo.RowNo = new List<int>() { rowNo };
            errorInfo.ColumnNo = new List<int>() { columnNo };
            errorInfo.TranslationText = columnName;
            errorInfo.ErrorInfo = error;
            errorInfo.DataDirection = dataDirection;

            return errorInfo;
        }

        /// <summary>
        /// エラー情報をマージして設定
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <param name="rowNo">行番号</param>
        /// <param name="columnNo">列番号</param>
        /// <param name="columnName">項目名</param>
        /// <param name="error">エラー内容</param>
        /// <param name="dataDirection">入力方式</param>
        private static void setErrorInfo(ref List<ComBase.UploadErrorInfo> errorInfoList, List<ComBase.UploadErrorInfo> tmpErrorInfo)
        {
            foreach (ComBase.UploadErrorInfo tmp in tmpErrorInfo)
            {
                //項目、エラー内容が同じものが設定されているかチェック(メッセージはエラーの種類ごとにまとめて表示する為)
                int count = errorInfoList.Where(x => x.TranslationText == tmp.TranslationText && x.ErrorInfo == tmp.ErrorInfo).Count();
                if (count > 0)
                {
                    //対象のインデックス番号を取得
                    int index = errorInfoList.Select((e, index) => (e, index)).Where(x => x.e.TranslationText == tmp.TranslationText && x.e.ErrorInfo == tmp.ErrorInfo).Select(x => x.index).FirstOrDefault();
                    //行番号または列番号を追加
                    switch (tmp.DataDirection)
                    {
                        case ComReport.LongitudinalDirection:
                            // 縦方向連続の場合、行番号を追加
                            errorInfoList[index].RowNo.Add(tmp.RowNo[0]);
                            break;
                        case ComReport.LateralDirection:
                            // 横方向連続の場合、列番号を追加
                            errorInfoList[index].ColumnNo.Add(tmp.ColumnNo[0]);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    errorInfoList.Add(tmp);
                }
            }
        }
        /// <summary>
        /// 取込処理 型チェック
        /// </summary>
        /// <param name="dataType">データタイプ</param>
        /// <param name="val">文字列</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="error">エラー情報</param>
        /// <returns>true:正常、false:異常</returns>
        private static bool checkCellType(int dataType, string val, string languageId, ComUtil.MessageResources msgResources, ref string error)
        {
            // データタイプによって処理を分岐
            switch (dataType)
            {
                case ComReport.DataTypeStr:
                    // 文字列の場合、正常
                    return true;
                case ComReport.DataTypeInt:
                case ComReport.DataTypeNum:
                    // 数値の場合
                    if (ComUtil.IsDecimal(val))
                    {
                        return true;
                    }
                    // 「数値で入力してください。」
                    error = GetResMessage(new string[] { ComRes.ID.ID941190002, ComRes.ID.ID911130002 }, languageId, msgResources);
                    return false;
                case ComReport.DataTypeDat:
                    // 日付の場合
                    if (ComUtil.IsDate(val) || ComUtil.IsDateYyyymmdd(val))
                    {
                        return true;
                    }
                    // 「日付で入力してください。」
                    error = GetResMessage(new string[] { ComRes.ID.ID941190002, ComRes.ID.ID911270007 }, languageId, msgResources);
                    return false;
                default:
                    // 基本、到達しない
                    return true;
            }
        }

        /// <summary>
        /// 取込処理 上下限チェック
        /// </summary>
        /// <param name="reportInfoList">ファイル入力項目定義情報</param>
        /// <param name="val">文字列</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="error">エラー情報</param>
        /// <returns>true:正常、false:異常</returns>
        private static bool checkRange(ComBase.InputDefine reportInfo, string val, ComDB db,
            string languageId, ComUtil.MessageResources msgResources, ref string error)
        {
            // 文字列⇒数値に変換
            decimal? workVal = ComUtil.ConvertDecimal(val);
            if (workVal == null)
            {
                // 変換できない場合、正常で処理を戻す ※型チェックなどのエラーは他で判定しているため
                return true;
            }
            //最小値
            decimal? minVal = ComUtil.ConvertDecimal(reportInfo.MinimumValue);
            //最大値
            decimal? maxVal = ComUtil.ConvertDecimal(reportInfo.MaximumValue);

            if (minVal != null && maxVal != null)
            {
                // 上下限値ともに設定されている場合
                if (decimal.Compare((decimal)minVal, (decimal)workVal) > 0 || decimal.Compare((decimal)maxVal, (decimal)workVal) < 0)
                {
                    // 「{0}から{1}の範囲で入力して下さい。」
                    error = GetResMessage(
                        new string[] { ComRes.ID.ID941060015, minVal.ToString(), maxVal.ToString() }, languageId, msgResources);
                    return false;
                }
            }
            else if (minVal != null && maxVal == null)
            {
                // 下限値のみ設定されている場合
                if (decimal.Compare((decimal)minVal, (decimal)workVal) > 0)
                {
                    // 「{0}以下の値を入力して下さい。」
                    error = GetResMessage(new string[] { ComRes.ID.ID941060016, ((double)minVal).ToString() }, languageId, msgResources);
                    return false;
                }
            }
            else if (minVal == null && maxVal != null)
            {
                // 上限値のみ設定されている場合
                if (decimal.Compare((decimal)maxVal, (decimal)workVal) < 0)
                {
                    // 「{0}以上の値を入力して下さい。」
                    error = GetResMessage(new string[] { ComRes.ID.ID941060017, ((double)maxVal).ToString() }, languageId, msgResources);
                    return false;
                }
            }
            else
            {
                // 上下限値ともに未設定の場合
                return true;
            }

            return true;
        }

        /// <summary>
        /// 入力チェックで検出されたエラー内容をメッセージ出力する
        /// </summary>
        /// <param name="isHeader">取込ファイルのヘッダの場合True、一覧の場合False</param>
        /// <param name="errorList">共通メソッドの戻り値であるエラー情報</param>
        /// <param name="outputMessages">out 出力メッセージリスト</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <returns>エラー内容が存在する場合、True</returns>
        public static bool SetErrorUploadCheckCommon(List<ComDao.UploadErrorInfo> errorList, ref List<string> outputMessages, string languageId, ComUtil.MessageResources msgResources)
        {
            bool isError = false;


            foreach (ComDao.UploadErrorInfo error in errorList)
            {
                // エラーリストが存在する場合、エラーあり
                isError = true;
                // 項目名：エラーメッセージ (例：「計画数量:数値を入力してください。」)
                string msg = GetResMessage(new string[] { ComRes.ID.ID941100001, error.TranslationText, error.ErrorInfo }, languageId, msgResources);

                //行番号または列番号を追加
                switch (error.DataDirection)
                {
                    case ComReport.SingleCell:
                        // 単一セルの場合
                        outputMessages.Add(msg);
                        break;
                    case ComReport.LongitudinalDirection:
                        // 縦方向連続の場合
                        string errRow = string.Join(",", error.RowNo);
                        // エラーメッセージ（例：「計画数量：数値を入力してください。(1,2行目)」）
                        outputMessages.Add(msg + "(" + errRow + GetResMessage(ComRes.ID.ID141070004, languageId, msgResources) + ")");
                        break;
                    case ComReport.LateralDirection:
                        // 横方向連続の場合
                        string errCol = string.Join(",", error.ColumnNo);
                        // エラーメッセージ（例：「計画数量：数値を入力してください。(1,2列目)」）
                        outputMessages.Add(msg + "(" + errCol + GetResMessage(ComRes.ID.ID141420001, languageId, msgResources) + ")");
                        break;
                    default:
                        break;
                }
            }

            return isError;
        }

        /// <summary>テキストファイル取込クラス </summary>
        public class UploadText
        {
            /// <summary>テキストファイルの内容</summary>
            private List<List<string>> textData;
            /// <summary>メッセージ出力用・言語ID</summary>
            private string languageId;
            /// <summary>メッセージ出力用・メッセージリソース</summary>
            private ComUtil.MessageResources msgResources;
            /// <summary>DB接続</summary>
            private ComDB db;
            /// <summary>
            /// コンストラクタ(UTF-8、CSV)
            /// </summary>
            /// <param name="uploadFile">InputStream</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="msgResources">メッセージリソース</param>
            /// <param name="db">DB接続</param>
            public UploadText(Stream uploadFile, string languageId, ComUtil.MessageResources msgResources, ComDB db)
            {
                setTextData(uploadFile, Encoding.UTF8, ComUtil.CharacterConsts.Comma);
                setMemberValues(languageId, msgResources, db);
            }

            /// <summary>
            /// コンストラクタ(エンコーディング、区切り文字指定)
            /// </summary>
            /// <param name="uploadFile">InputStream</param>
            /// <param name="encoding">ファイルのエンコーディング</param>
            /// <param name="delimiter">ファイルの区切り文字</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="msgResources">メッセージリソース</param>
            /// <param name="db">DB接続</param>
            public UploadText(Stream uploadFile, Encoding encoding, char delimiter, string languageId, ComUtil.MessageResources msgResources, ComDB db)
            {
                setTextData(uploadFile, encoding, delimiter);
                setMemberValues(languageId, msgResources, db);
            }

            /// <summary>
            /// テキストデータ以外のメンバ変数を設定する処理
            /// </summary>
            /// <param name="inLanguageId">言語ID</param>
            /// <param name="inMsgResources">メッセージリソース</param>
            /// <param name="inDb">DB接続</param>
            private void setMemberValues(string inLanguageId, ComUtil.MessageResources inMsgResources, ComDB inDb)
            {
                this.languageId = inLanguageId;
                this.msgResources = inMsgResources;
                this.db = inDb;
            }
            /// <summary>
            /// 取込ファイルの内容を文字列のリストに設定する処理
            /// </summary>
            /// <param name="inUploadFile">InputStream</param>
            /// <param name="inEncoding">ファイルのエンコーディング</param>
            /// <param name="inDelimiter">ファイルの区切り文字</param>
            /// <remarks>コンストラクタで引数省略をしたいけど、出来ないからオーバーライド用のベースクラス</remarks>
            private void setTextData(Stream inUploadFile, Encoding inEncoding, char inDelimiter)
            {
                // 初期化
                this.textData = new List<List<string>>();
                // 読み込み
                string fileTexts = getFileText(inUploadFile, inEncoding);
                // 改行コードで分割
                string[] textData = fileTexts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                // 読み込んでリストに変換
                // 行で繰り返し
                foreach (string row in textData)
                {
                    if (string.IsNullOrEmpty(row))
                    {
                        // 空行はスキップ
                        continue;
                    }
                    // 1行の内容を格納するリスト
                    List<string> rowData = new List<string>();
                    // 区切り文字で1行の内容を分割
                    string[] vals = ComUtil.SplitCSV(row, inDelimiter);
                    // 列で繰り返し
                    foreach (string val in vals)
                    {
                        // 行に列の内容を追加
                        rowData.Add(val);
                    }
                    // 行の内容を追加
                    this.textData.Add(rowData);
                }
            }

            /// <summary>
            /// ファイルを読み込んで、文字列に変換する処理
            /// </summary>
            /// <param name="inUploadFile">InputStream</param>
            /// <param name="inEncoding">ファイルのエンコーディング</param>
            /// <returns>読み込んだファイルの内容(文字列)</returns>
            /// <remarks>CommonSTDUtil.ImportFileWithDelimiterよりコピー</remarks>
            private string getFileText(Stream inUploadFile, Encoding inEncoding)
            {
                string fileTexts = string.Empty;
                try
                {
                    using (var memStream = new MemoryStream())
                    {
                        // 取込ファイルのバイト配列をコピー
                        inUploadFile.CopyTo(memStream);
                        byte[] bytes = memStream.ToArray();
                        if (bytes == null || bytes.Length <= 0)
                        {
                            // 取込ファイルにデータなし
                            // 空の配列を返却する
                            // エラーかどうかは呼び元で判断する
                            return string.Empty;
                        }

                        // 全行取り出し
                        fileTexts = inEncoding.GetString(bytes);
                    }
                }
                finally
                {
                    inUploadFile.Close();
                    inUploadFile = null;
                }
                return fileTexts;
            }
            /// <summary>
            /// 取込（テキスト）共通チェック処理
            /// </summary>
            /// <typeparam name="T">型</typeparam>
            /// <param name="conductId">機能ID</param>
            /// <param name="fileNo">ファイル管理NO</param>
            /// <param name="sheetNo">シート番号(0オリジン)</param>
            /// <param name="itemid">項目ID</param>
            /// <param name="resultList">ref 取込結果格納クラス</param>
            /// <param name="checkFlg">省略可能　チェックフラグ　省略時はチェック有り</param>
            /// <returns>エラー情報</returns>
            /// <remarks>ComUploadErrorCheckよりコピー</remarks>
            public List<ComBase.UploadErrorInfo> ComUploadErrorCheck<T>(string reportId, int sheetNo, string controlGroupId, ref List<T> resultList, bool checkFlg = true)
            {
                // エラー内容格納クラス
                List<ComBase.UploadErrorInfo> errorInfo = new List<ComBase.UploadErrorInfo>();

                // ファイル入力項目定義情報を取得
                ComBase.InputDefineCondition param = new ComBase.InputDefineCondition();
                param.ReportId = reportId;
                param.SheetNo = sheetNo;
                param.ControlGroupId = controlGroupId;
                param.LanguageId = languageId;
                param.FactoryId = Const.CommonFactoryId;
                IList<ComBase.InputDefine> reportInfoList = TMQUtil.SqlExecuteClass.SelectList<ComBase.InputDefine>(ComReport.GetInputControlDefine, ExcelPath, param, db);
                if (reportInfoList == null || reportInfoList.Count == 0)
                {
                    // 取得できない場合、処理を戻す
                    return null;
                }

                // 検索結果クラスのプロパティを列挙
                var properites = typeof(T).GetProperties();
                // 1レコード分の行数、1レコード分の行数を取得する
                int addRow = reportInfoList[0].RecordCount;
                // 入力方式を取得
                int dataDirection = reportInfoList[0].DataDirection;

                int index = 0;
                while (true)
                {
                    // エラー内容一時格納クラス
                    List<ComBase.UploadErrorInfo> tmpErrorInfo = new List<ComBase.UploadErrorInfo>();

                    bool flg = false; // データ存在チェックフラグ
                    object tmpResult = Activator.CreateInstance<T>();

                    // 取得できた項目定義分処理を行う
                    foreach (ComBase.InputDefine reportInfo in reportInfoList)
                    {
                        // 2行目以降、入出力方式によって、表示位置をずらす
                        if (index > 0)
                        {
                            switch (dataDirection)
                            {
                                case ComReport.SingleCell:
                                    // 基本、到達しない
                                    continue;
                                case ComReport.LongitudinalDirection:
                                    // 縦方向連続の場合、行番号を加算する
                                    reportInfo.StartRowNo += addRow;
                                    break;
                                case ComReport.LateralDirection:
                                    // 横方向連続の場合、列番号を加算する
                                    reportInfo.StartColumnNo += addRow;
                                    break;
                                default:
                                    // 入出力方式が未設定の場合、スキップ
                                    break;
                            }
                        }

                        string error = string.Empty;
                        // 行と列の番号を取得(Nullは無いので適当)
                        int rowIndex = reportInfo.StartRowNo - 1;
                        int colIndex = reportInfo.StartColumnNo - 1;
                        if (rowIndex >= this.textData.Count)
                        {
                            break;
                        }
                        if (colIndex >= this.textData[rowIndex].Count)
                        {
                            // レイアウト定義では読み込むのに、取込ファイルに含まれない場合エラー
                            // 「ファイルレイアウトが有効ではありません。」
                            error = GetResMessage(ComRes.ID.ID941280008, this.languageId, this.msgResources);
                            tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                            setErrorInfo(ref errorInfo, tmpErrorInfo);
                            // レイアウトがおかしい場合、読み込みが無限ループの恐れがあるため、終了
                            return errorInfo;
                        }
                        string val = this.textData[rowIndex][colIndex];

                        if (checkFlg)
                        {
                            // 値が取得できない場合、スキップ
                            if (string.IsNullOrEmpty(val))
                            {
                                if (reportInfo.RequiredFlg != null && (bool)reportInfo.RequiredFlg)
                                {
                                    // 必須入力項目の場合、エラー内容を設定
                                    // 「必須項目です。入力してください。」
                                    error = GetResMessage(ComRes.ID.ID941270001, languageId, msgResources);
                                    tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                                }
                                continue;
                            }
                        }


                        // 入力項目が存在する場合、フラグをたてる
                        flg = true;

                        if (checkFlg)
                        {
                            // 入力チェック
                            // 数値の場合、指数表記の可能性があるので、変換を実施
                            if (reportInfo.DataType == ComReport.DataTypeInt || reportInfo.DataType == ComReport.DataTypeNum)
                            {
                                if (val != null && ComUtil.ConvertDecimal(val) != null)
                                {
                                    val = ComUtil.ConvertDecimal(val).ToString();
                                }
                            }
                            if (!checkCellType(reportInfo.DataType, val, languageId, msgResources, ref error))
                            {
                                // 型が異なる場合、エラーを設定し、スキップ
                                tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                                continue;
                            }

                            // 桁数チェック
                            if (reportInfo.MaximumLength != null && reportInfo.MaximumLength > 0)
                            {
                                // セルタイプが指定され、日付以外の場合、チェックを行う
                                if (reportInfo.DataType != ComReport.DataTypeDat)
                                {
                                    // 桁数を超えている場合、エラーを設定を設定し、スキップ
                                    if (val.Length > reportInfo.MaximumLength)
                                    {
                                        // 「入力値が設定桁数を超えています。」
                                        error = GetResMessage(new string[] { ComRes.ID.ID941220008 }, languageId, msgResources);
                                        tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                                        continue;
                                    }
                                }
                            }

                            // 数値の場合、上下限チェック
                            // 入力チェック
                            if (reportInfo.DataType == ComReport.DataTypeInt || reportInfo.DataType == ComReport.DataTypeNum)
                            {
                                if (!checkRange(reportInfo, val, db, languageId, msgResources, ref error))
                                {
                                    // 範囲エラーの場合、エラーを設定し、スキップ
                                    tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, error, dataDirection));
                                    continue;
                                }
                            }
                        }

                        // 値をデータクラスに設定
                        string pascalItemName = ComUtil.SnakeCaseToPascalCase(reportInfo.AliasName != null ? reportInfo.AliasName : reportInfo.ColumnName).ToUpper();
                        var prop = properites.FirstOrDefault(x => x.Name.ToUpper().Equals(pascalItemName));
                        if (prop == null)
                        {
                            // 該当する項目が存在しない場合、スキップ
                            continue;
                        }
                        ComUtil.SetPropertyValue<T>(prop, (T)tmpResult, val);
                    }

                    // データが1件も取得できなかった場合、処理を抜ける
                    if (!flg)
                    {
                        break;
                    }

                    //エラーがある場合、エラーフラグを立てる
                    var errProp = properites.FirstOrDefault(x => x.Name.ToUpper().Equals("ErrorFlg"));
                    if (errProp != null && tmpErrorInfo.Count > 0)
                    {
                        ComUtil.SetPropertyValue<T>(errProp, (T)tmpResult, true);
                    }

                    // データが存在する場合、リストに追加する
                    setErrorInfo(ref errorInfo, tmpErrorInfo);
                    resultList.Add((T)tmpResult);
                    index++;

                    // 入力方式が単一セルの場合、処理を抜ける
                    if ((int)dataDirection == ComReport.SingleCell)
                    {
                        break;
                    }
                }
                return errorInfo;
            }

        }
        #endregion
    }
}
