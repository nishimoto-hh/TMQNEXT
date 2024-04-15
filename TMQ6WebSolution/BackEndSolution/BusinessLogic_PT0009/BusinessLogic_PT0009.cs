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
using Dao = BusinessLogic_PT0009.BusinessLogicDataClass_PT0009;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;
using CommonTMQUtil;
using static CommonTMQUtil.CommonTMQUtil;

/// <summary>
/// 会計帳票出力
/// </summary>
namespace BusinessLogic_PT0009
{
    /// <summary>
    /// 会計帳票出力
    /// </summary>
    public class BusinessLogic_PT0009 : CommonBusinessLogicBase
    {
        #region private変数

        /// <summary>
        /// 拡張データに持っている取得条件
        /// </summary>
        private static class condPartsType
        {
            /// <summary>
            /// 金額管理単位
            /// </summary>
            public static class currencyId
            {
                /// <summary>構成グループID</summary>
                public const short StructureGroupId = 1740;
                /// <summary>連番_小数点以下桁数</summary>
                public const short SeqDigit = 1;
                /// <summary>連番_丸め処理区分</summary>
                public const short SeqRoundDivision = 2;
            }
        }
        #endregion

        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Parts";
        }

        /// <summary>
        /// SQLファイル名称(仮確定在庫用会計提出表用)
        /// </summary>
        private static class SqlNameForTemp
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Excel\RP0460";
            /// <summary>
            /// 一時テーブル作成SQL
            /// </summary>
            public const string CreateTableTempReport = "CreateTableTempReport";
            /// <summary>
            /// 一時テーブル作成SQL
            /// </summary>
            public const string InsertTempReport = "InsertTempReport";
        }

        /// <summary>
        /// フォーム、グループ、コントロールの親子関係を表現した場合の定数クラス
        /// </summary>
        private static class ConductInfo
        {
            public static class FormList
            {
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 出力条件一覧
                    /// </summary>
                    public const string ConditionList = "BODY_000_00_LST_0";
                    /// <summary>
                    /// ボタンエリア
                    /// </summary>
                    public const string List = "BODY_010_00_BTN_0";
                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonCtrlId
                {
                    /// <summary>
                    /// 出力
                    /// </summary>
                    public const string Search = "Output";
                }
            }
        }

        /// <summary>
        /// テンプレートデフォルト値
        /// </summary>
        public static class TemplateDefaultValue
        {
            /// <summary>
            /// テンプレートID
            /// </summary>
            public const int TemplateId = 1;
        }

        /// <summary>
        /// 出力パターンデフォルト値
        /// </summary>
        public static class OutputPatternDefaultValue
        {
            /// <summary>
            /// 出力パターンID
            /// </summary>
            public const int OutputPatternId = 1;
        }

        /// <summary>
        /// 仮確定在庫用会計提出表の帳票ID
        /// </summary>
        private const string TempReportId = "RP0460";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_PT0009() : base()
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
            //return InitSearch();

            // 初期化処理で処理を行わない場合は以下のように定数を返す
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

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
            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「登録処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200003 });

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
            try
            {
                int result = 0;
                int reportFactoryId = 0;
                string reportId = string.Empty;
                string pgmId = string.Empty;
                int templateId = 0;
                int outputPatternId = 0;

                this.ResultList = new();

                // 画面情報取得
                DateTime now = DateTime.Now;
                Dao.conditionList condition = getRegistInfo<Dao.conditionList>(ConductInfo.FormList.ControlId.ConditionList, now);

                // 帳票ごとの情報を取得
                reportId = condition.ReportId;
                // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;
                pgmId = this.PgmId; // PT0009

                templateId = TemplateDefaultValue.TemplateId;
                outputPatternId = OutputPatternDefaultValue.OutputPatternId;

                // 画面から検索条件データ取得
                // 一覧のコントロールID指定で取得
                getSearchConditionByTargetCtrlIdForReport(ConductInfo.FormList.ControlId.ConditionList, out dynamic searchCondition);

                // 出力用に会計帳票出力条件の設定
                setCondAccountReport(condition, out TMQUtil.CondAccountReport condAccountReport);

                // 件数チェック
                bool existsFlg = false;
                // シートごとにデータチェックを行い、出力該当データなしの場合、処理を終了する
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

                // シート定義毎にループ
                foreach (var sheetDefine in sheetDefineList)
                {
                    // 検索条件フラグの場合はスキップする
                    if (sheetDefine.SearchConditionFlg == true)
                    {
                        continue;
                    }

                    // 翻訳の一時テーブルを作成
                    TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
                    listPf.GetCreateTranslation(); // テーブル作成
                    listPf.GetInsertTranslationAll(new List<GroupId>(), true); // 各グループ
                    listPf.RegistTempTable(); // 登録

                    // 仮確定在庫用会計提出表(RP0460)の場合、データを集計するため一時テーブルに出力データを格納する
                    if (condition.ReportId == TempReportId)
                    {
                        registTempReportData(condAccountReport);
                    }

                    IList<dynamic> dataList = TMQUtil.GetAccountReportData(sheetDefine.TargetSql, db, condAccountReport);
                    if (dataList.Count > 0)
                    {
                        existsFlg = true;
                    }
                }
                // 出力データが無い場合、終了
                if (existsFlg == false)
                {
                    // 以上終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    this.MsgId = GetResMessage(new string[] { "941160005", condition.ReportName });
                    return ComConsts.RETURN_RESULT.NG;
                }

                // エクセル出力共通処理
                TMQUtil.CommonOutputExcel(
                    reportFactoryId,            // 工場ID
                    pgmId,                       // プログラムID
                    null,                        // シートごとのパラメータでの選択キー情報リスト
                    searchCondition,             // 検索条件
                    reportId,                    // 帳票ID
                    templateId,                  // テンプレートID
                    outputPatternId,             // 出力パターンID
                    this.UserId,                 // ユーザID
                    this.LanguageId,             // 言語ID
                    this.conditionSheetLocationList,    // 場所階層構成IDリスト
                    this.conditionSheetJobList,         // 職種機種構成IDリスト
                    this.conditionSheetNameList,        // 検索条件項目名リスト
                    this.conditionSheetValueList,       // 検索条件設定値リスト
                    out string fileType,         // ファイルタイプ
                    out string fileName,         // ファイル名
                    out MemoryStream memStream,  // メモリストリーム
                    out string message,          // メッセージ
                    db,
                    null,
                    condAccountReport);

                // OUTPUTパラメータに設定
                this.OutputFileType = fileType;
                this.OutputFileName = fileName;
                this.OutputStream = memStream;

                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
                return ComConsts.RETURN_RESULT.OK;
            }
            catch (Exception ex)
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「ダウンロード処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "111160052" });
                this.LogNo = string.Empty;

                writeErrorLog(this.MsgId, ex);
                return -1;
            }
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int UploadImpl()
        {
            // 実装の際は、不要な帳票に対する分岐は削除して構いません

            int result = 0;

            return result;
        }

        #endregion

        #region privateメソッド
        private T getRegistInfo<T>(string targetCtrlId, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 相称型を使用することで、色々な型を呼出元で宣言することができる
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfo(targetCtrlId);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

            T resultInfo = new();
            // コントロールIDごとに繰り返し
            foreach (var ctrlId in ctrlIdList)
            {
                // コントロールIDより画面の項目を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ctrlId);
                var mapInfo = grpMapInfo.selectByCtrlId(ctrlId);
                // 登録データの設定
                if (!SetExecuteConditionByDataClass(condition, mapInfo.Value, resultInfo, now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return resultInfo;
                }
            }
            return resultInfo;
        }
        /// <summary>
        /// 会計帳票出力条件画面条件設定
        /// </summary>
        /// <param name="condition">会計帳票出力条件画面</param>
        /// <param name="condAccountReport">会計帳票出力条件クラス</param>
        ///
        private void setCondAccountReport(Dao.conditionList condition, out TMQUtil.CondAccountReport condAccountReport)
        {
            // 会計帳票出力条件
            condAccountReport = new TMQUtil.CondAccountReport();
            List<int> departmentIdList = new List<int>();
            // SQLの中でコメントアウトを解除したい箇所のリスト
            List<string> listUnComment = new List<string>();

            // 対象年月
            if (condition.TargetYearMonth != null && string.IsNullOrEmpty(condition.TargetYearMonth) == false)
            {
                // 会計提出表(未確定)の場合は対象年月の値を 年月 → 年月末日に変更
                if (condition.ReportId == TempReportId)
                {
                    condAccountReport.TargetMaxDate = DateTime.Parse(condition.TargetYearMonth).AddMonths(1);
                    listUnComment.Add(nameof(condition.TargetMaxDate));
                }

                // 会計提出表の場合は入力された値をそのまま使用
                condAccountReport.TargetYearMonth = condition.TargetYearMonth;
                listUnComment.Add(nameof(condition.TargetYearMonth));

                // 会計提出表(未確定)で滞留日数を算出するための日付を設定
                condAccountReport.TargetYearMonthForLongStay = condition.TargetYearMonth;
            }
            else
            {
                // 会計提出表(未確定)で滞留日数を算出するための日付を設定
                // 抽出条件の 対象年月が未入力の場合はシステム日付の年月を使用
                condAccountReport.TargetYearMonthForLongStay = DateTime.Now.ToString("yyyy/MM");
            }

            // 工場
            if (condition.FactoryIdList != null && condition.FactoryIdList.Count > 0)
            {
                condAccountReport.FactoryIdList = condition.FactoryIdList;

                // 工場の絞り込み対象に標準工場「0」を追加
                condAccountReport.FactoryIdList.Add(0);

                listUnComment.Add(nameof(condition.FactoryIdList));
            }
            // 職種
            if (condition.JobId != null && condition.JobId >= 0)
            {
                condAccountReport.JobId = condition.JobId;
                listUnComment.Add(nameof(condition.JobId));
            }
            // 予備品No
            if (condition.PartsNo != null && string.IsNullOrEmpty(condition.PartsNo) == false)
            {
                condAccountReport.PartsNo = condition.PartsNo;
                listUnComment.Add(nameof(condition.PartsNo));
            }
            // 予備品名
            if (condition.PartsName != null && string.IsNullOrEmpty(condition.PartsName) == false)
            {
                condAccountReport.PartsName = condition.PartsName;
                listUnComment.Add(nameof(condition.PartsName));
            }
            // 型式
            if (condition.ModelType != null && string.IsNullOrEmpty(condition.ModelType) == false)
            {
                condAccountReport.ModelType = condition.ModelType;
                listUnComment.Add(nameof(condition.ModelType));
            }
            // 規格・寸法
            if (condition.StandardSize != null && string.IsNullOrEmpty(condition.StandardSize) == false)
            {
                condAccountReport.StandardSize = condition.StandardSize;
                listUnComment.Add(nameof(condition.StandardSize));
            }
            // メーカー
            if (condition.ManufacturerStructureId != null && condition.ManufacturerStructureId >= 0)
            {
                condAccountReport.ManufacturerStructureId = condition.ManufacturerStructureId;
                listUnComment.Add(nameof(condition.ManufacturerStructureId));
            }
            // 使用場所
            if (condition.PartsServiceSpace != null && string.IsNullOrEmpty(condition.PartsServiceSpace) == false)
            {
                condAccountReport.PartsServiceSpace = condition.PartsServiceSpace;
                listUnComment.Add(nameof(condition.PartsServiceSpace));
            }
            // 仕入先
            if (condition.VenderStructureId != null && condition.VenderStructureId >= 0)
            {
                condAccountReport.VenderStructureId = condition.VenderStructureId;
                listUnComment.Add(nameof(condition.VenderStructureId));
            }
            // 予備品倉庫
            if (condition.StorageLocationId != null && condition.StorageLocationId >= 0)
            {
                condAccountReport.StorageLocationId = condition.StorageLocationId;
                listUnComment.Add(nameof(condition.StorageLocationId));
            }
            // 棚番
            if (condition.PartsLocationId != null && condition.PartsLocationId >= 0)
            {
                condAccountReport.PartsLocationId = condition.PartsLocationId;
                listUnComment.Add(nameof(condition.PartsLocationId));
            }
            // 入出庫日From
            if (condition.WorkDivisionDateFrom != null)
            {
                DateTime? workDivisionDateFrom = DateTime.TryParse(condition.WorkDivisionDateFrom.ToString(), out DateTime outDate) ? outDate : null;
                if (workDivisionDateFrom != null)
                {
                    condAccountReport.WorkDivisionDateFrom = condition.WorkDivisionDateFrom;
                    listUnComment.Add(nameof(condition.WorkDivisionDateFrom));
                }
            }
            // 入出庫日To
            if (condition.WorkDivisionDateTo != null)
            {
                DateTime? workDivisionDateTo = DateTime.TryParse(condition.WorkDivisionDateTo.ToString(), out DateTime outDate) ? outDate : null;
                if (workDivisionDateTo != null)
                {
                    condAccountReport.WorkDivisionDateTo = condition.WorkDivisionDateTo;
                    listUnComment.Add(nameof(condition.WorkDivisionDateTo));
                }
            }
            // 入出庫No
            if (condition.WorkDivisionNo != null && condition.WorkDivisionNo >= 0)
            {
                condAccountReport.WorkDivisionNo = condition.WorkDivisionNo;
                listUnComment.Add(nameof(condition.WorkDivisionNo));
            }
            // 新旧区分
            if (condition.OldNewStructureId != null && condition.OldNewStructureId >= 0)
            {
                condAccountReport.OldNewStructureId = condition.OldNewStructureId;
                listUnComment.Add(nameof(condition.OldNewStructureId));
            }
            // 勘定科目
            if (condition.AccountStructureId != null && condition.AccountStructureId >= 0)
            {
                condAccountReport.AccountStructureId = condition.AccountStructureId;
                listUnComment.Add(nameof(condition.AccountStructureId));
            }
            // 部門
            if (condition.DepartmentIdList != null && condition.DepartmentIdList.Count > 0)
            {
                condAccountReport.DepartmentIdList = condition.DepartmentIdList;
                listUnComment.Add(nameof(condition.DepartmentIdList));
            }
            // 管理区分
            if (condition.ManagementDivision != null && string.IsNullOrEmpty(condition.ManagementDivision) == false)
            {
                condAccountReport.ManagementDivision = condition.ManagementDivision;
                listUnComment.Add(nameof(condition.ManagementDivision));
            }
            // 管理No
            if (condition.ManagementNo != null && string.IsNullOrEmpty(condition.ManagementNo) == false)
            {
                condAccountReport.ManagementNo = condition.ManagementNo;
                listUnComment.Add(nameof(condition.ManagementNo));
            }
            // 帳票名
            if (condition.ReportId != null && string.IsNullOrEmpty(condition.ReportId) == false)
            {
                condAccountReport.ReportId = condition.ReportId;
                listUnComment.Add(nameof(condition.ReportId));
            }
            // SQLの中でコメントアウトを解除したい箇所のリスト
            if (listUnComment.Count > 0)
            {
                condAccountReport.ListUnComment = listUnComment;
            }

            // 金額管理単位の取得
            setComCondAccountReport(out int currencyDigit, out int currencyRoundDivision);
            // 以下、会計帳票出力条件で共通の設定
            // 言語ID
            condAccountReport.LanguageId = this.LanguageId;
            // 小数点以下桁数(金額)
            condAccountReport.CurrencyDigit = currencyDigit;
            // 丸め処理区分(金額)
            condAccountReport.CurrencyRoundDivision = currencyRoundDivision;
        }

        /// <summary>
        /// 金額管理単位の取得
        /// </summary>
        /// <param name="currencyDigit">小数点以下桁数(金額)</param>
        /// <param name="currencyRoundDivision">丸め処理区分(金額)</param>
        ///
        private void setComCondAccountReport(out int currencyDigit, out int currencyRoundDivision)
        {
            // 小数点以下桁数(金額)
            currencyDigit = 0;
            // 丸め処理区分(金額)
            currencyRoundDivision = 0;

            currencyDigit = getCurrencyItemExData(condPartsType.currencyId.StructureGroupId, condPartsType.currencyId.SeqDigit);
            currencyRoundDivision = getCurrencyItemExData(condPartsType.currencyId.StructureGroupId, condPartsType.currencyId.SeqRoundDivision);
        }

        /// <summary>
        /// 金額管理単位の拡張データを取得する
        /// </summary>
        /// <returns>拡張データ</returns>
        private int getCurrencyItemExData(int structureGroupId, int seq)
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();

            //構成グループID
            param.StructureGroupId = structureGroupId;

            //連番
            param.Seq = seq;

            //言語ID
            param.LanguageId = this.LanguageId;

            // 機能レベル取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> partsTypeList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            foreach (var partsType in partsTypeList)
            {
                // 取得データの先頭データを正として扱う
                if (string.IsNullOrEmpty(partsType.ExData))
                {
                    int temp;
                    if (!int.TryParse(partsType.ExData, out temp))
                    {
                        return temp;
                    }
                }
            }
            // 取得できない場合、0を返す
            return 0;
        }

        /// <summary>
        /// 仮確定在庫用会計提出表を出力するための一時テーブルを作成する
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool registTempReportData(TMQUtil.CondAccountReport condAccountReport)
        {
            // 一時テーブルを作成SQLを取得し、実行する
            string createSql = SqlExecuteClass.GetExecuteSql(SqlNameForTemp.CreateTableTempReport, SqlNameForTemp.SubDir, string.Empty);
            this.db.Regist(createSql);

            // 一時テーブルに集計データを登録するSQLを取得し、実行する
            string insertSql = SqlExecuteClass.GetExecuteSql(SqlNameForTemp.InsertTempReport, SqlNameForTemp.SubDir, string.Empty, condAccountReport.ListUnComment);
            this.db.Regist(insertSql, condAccountReport);

            return true;
        }
        #endregion

        #region publicメソッド
        #endregion

    }
}