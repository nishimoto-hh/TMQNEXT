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
using Dao = BusinessLogic_LN0002.BusinessLogicDataClass_LN0002;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

/// <summary>
/// 機器別長期計画
/// </summary>
namespace BusinessLogic_LN0002
{
    /// <summary>
    /// 機器別長期計画
    /// </summary>
    public partial class BusinessLogic_LN0002 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL名：一覧取得</summary>
            public const string GetList = "GetList";
            /// <summary>SQL名：スケジュール一覧取得</summary>
            public const string GetScheduleList = "GetScheduleList";
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"LongPlanMachine";
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubExcelDir = @"Excel";
            /// <summary>SQL名：LongPlanId取得用SQL</summary>
            public const string GetLongPlanIdList = "GetLongPlanIdList";
            /// <summary>SQL名：○リンククリック時に同一年月の○リンク件数を取得</summary>
            public const string GetLinkCntForTrans = "GetLinkCntForTrans";
            /// <summary>SQL名：○リンククリック時に保全スケジュール詳細IDから機器情報を取得</summary>
            public const string GetEquipmentInfoByScheduleDetailId = "GetEquipmentInfoByScheduleDetailId";

            /// <summary>SQL名：一時テーブル作成：一覧取得用</summary>
            public const string CreateTempForGetList = "CreateTableTempGetList";
            /// <summary>SQL名：一時テーブル登録：一覧取得用</summary>
            public const string InsertTempForGetList = "InsertTempGetList";
        }

        /// <summary>
        /// 画面定義
        /// </summary>
        private static class ConductInfo
        {
            /// <summary>
            /// 一覧画面
            /// </summary>
            public static class FormList
            {
                /// <summary>画面No </summary>
                public const short Id = 0;
                /// <summary>
                /// コントロールグループID
                /// </summary>
                public static class CtrlId
                {
                    /// <summary>一覧</summary>
                    public const string List = "BODY_040_00_LST_0";
                    /// <summary>スケジュール表示条件</summary>
                    public const string ScheduleCondition = "BODY_020_00_LST_0";
                }
            }
            /// <summary>
            /// 予算出力
            /// </summary>
            public class FormBudgetOutput
            {
                public const short FormNo = 1;
                public static class ControlId
                {
                    /// <summary>出力条件</summary>
                    public const string OutputCondition = "BODY_000_00_LST_1";
                    /// <summary>処理対象機番ID</summary>
                    public const string MachineId = "BODY_020_00_LST_1";
                    /// <summary>スケジュール表示条件</summary>
                    public const string ScheduleCondition = "BODY_030_00_LST_1";
                }
                public static class ButtonId
                {
                    /// <summary>出力ボタン</summary>
                    public const string BudgetOutput = "btnOutput";
                }
            }
        }

        /// <summary>
        /// スケジュールより遷移する保全活動画面の情報
        /// </summary>
        private static class MA0001
        {
            /// <summary>機能ID</summary>
            public const string ConductId = "MA0001";
            /// <summary>フォームNo</summary>
            public static class FormNo
            {
                /// <summary>参照画面</summary>
                public const int Detail = 1;
                /// <summary>新規登録</summary>
                public const int New = 0;
            }
            /// <summary>タブなしの場合のタブNo</summary>
            public const int TabNoNone = 0;

            /// <summary>タブNo(参照画面)</summary>
            public static class TabNoDetail
            {
                /// <summary>履歴タブ</summary>
                public const int History = 3;
                /// <summary>依頼タブ</summary>
                public const int Request = 1;
                /// <summary>参照画面でなく、新規登録</summary>
                public const int New = -1;
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_LN0002() : base()
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
                // 戻る場合、検索結果のコントロールタイプに応じて検索処理を切り替える
                return InitSearch();
            }
            switch (this.FormNo)
            {
                case ConductInfo.FormBudgetOutput.FormNo:
                    // 予算出力画面
                    initBudgetOutput();
                    return ComConsts.RETURN_RESULT.OK;
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

            if (this.FormNo == ConductInfo.FormList.Id)
            {
                // 一覧画面検索処理
                if (!searchList())
                {
                    // エラーの場合
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else
            {
                // 到達不能
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
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            if (compareId.IsStartId("checkExistsOtherScheduleLink"))
            {
                // クリックされた○と同一年月の○リンクの件数を取得する
                return checkExistsOtherScheduleLink();
            }

            // 処理なし
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            // 処理なし
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            // 処理なし
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            int reportFactoryId = 0;
            string reportId = string.Empty;

            // 長期スケジュール表
            if (this.CtrlId == "Report")
            {
                reportId = "RP0090";
                // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

                //// キー情報を設定
                //Key keyInfo = new Key("MachineId"); // 長期計画件名ID

                //List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport<Dao.FormList.List>(
                //    ConductInfo.FormList.CtrlId.List, // 一覧のコントールID
                //    keyInfo,                               // 設定したキー情報
                //    this.resultInfoDictionary,             // 画面データ
                //    true);                                 // 選択フラグ

                // ページ情報取得
                var pageInfo = GetPageInfo(
                    ConductInfo.FormList.CtrlId.List,     // 一覧のコントールID
                    this.pageInfoList);          // ページ情報リスト

                // シートごとの帳票用選択キーデータ設定変数
                Dictionary<int, List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData>> dicSelectKeyDataList = new Dictionary<int, List<SelectKeyData>>();

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
                    Key keyInfo = getKeyInfoByTargetSqlParams(sheetDefine.TargetSqlParams);

                    // 帳票用選択キーデータ取得
                    // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                    List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport(
                        ConductInfo.FormList.CtrlId.List,                   // 一覧のコントールID
                        keyInfo,                                            // 設定したキー情報
                        this.resultInfoDictionary);                         // 画面データ

                    // シートNoをキーとして帳票用選択キーデータを保存する
                    dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                }

                // スケジュール関連
                // 年度開始月
                int monthStartNendo = getYearStartMonth();
                // 画面の条件を取得
                var scheduleCond = GetFormDataByCtrlId<TMQDao.ScheduleList.Condition>(ConductInfo.FormList.CtrlId.ScheduleCondition, false);
                Dao.Schedule.SearchCondition cond = new(scheduleCond, monthStartNendo, this.LanguageId);
                cond.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);

                // 長期スケジュール用オプション
                TMQUtil.Option option = new TMQUtil.Option();
                // スケジュール表示単位 1:月度、2:年度
                option.DisplayUnit = (int)cond.DisplayUnit;
                // 開始年月日
                option.StartDate = cond.ScheduleStart;
                // 終了年月日
                option.EndDate = cond.ScheduleEnd;
                // 出力方式 1:件名別、2:機番別、3:予算別
                option.OutputMode = TMQUtil.ComReport.OutputMode2;
                // 年度開始月
                option.MonthStartNendo = monthStartNendo;
                // 検索条件クラス
                option.Condition = cond;
                // スケジュール表示単位が 1:月度の場合
                if (option.DisplayUnit == (int)TMQConst.MsStructure.StructureId.ScheduleDisplayUnit.Month)
                {
                    reportId = "RP0100";
                }
                //// 検索条件データ取得
                getSearchConditionForReport(pageInfo, out dynamic searchCondition);
                TMQUtil.CommonOutputExcel(
                    reportFactoryId,             // 工場ID
                    "LN0001",                    // プログラムID(マスタ取得のため件名別長期計画とする)
                    dicSelectKeyDataList,        // シートごとのパラメータでの選択キー情報リスト
                    searchCondition,             // 検索条件
                    reportId,                    // 帳票ID
                    1,                           // テンプレートID
                    1,                           // 出力パターンID
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
                    option);

                // OUTPUTパラメータに設定
                this.OutputFileType = fileType;
                this.OutputFileName = fileName;
                this.OutputStream = memStream;

            }

            // 予算出力画面から出力ボタン押下の場合
            if (this.FormNo == ConductInfo.FormBudgetOutput.FormNo && this.CtrlId == "btnOutPut")
            {
                // 出力期間入力チェック
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormBudgetOutput.ControlId.OutputCondition);
                string[] outputPeriod = targetDic["VAL1"].ToString().Split("|");
                if (string.IsNullOrEmpty(outputPeriod[0]) || string.IsNullOrEmpty(outputPeriod[1]))
                {
                    // 出力期間を入力してください。
                    this.MsgId = GetResMessage(new string[] { "941450003", "111120043" });
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                reportId = "RP0090";
                // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

                //// キー情報を設定
                //Key keyInfo = new Key("LongPlanId"); // 長期計画件名ID

                //List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport<Dao.Detail.List>(
                //    ConductInfo.FormBudgetOutput.ControlId.LongPlanId, // 一覧のコントールID
                //    keyInfo,                               // 設定したキー情報
                //    this.resultInfoDictionary,             // 画面データ
                //    false);                                 // 選択フラグ

                // ページ情報取得
                var pageInfo = GetPageInfo(
                    ConductInfo.FormList.CtrlId.List,     // 一覧のコントールID
                    this.pageInfoList);          // ページ情報リスト
                // 検索条件データ取得
                getSearchConditionForReport(pageInfo, out dynamic searchCondition);

                // シートごとの帳票用選択キーデータ設定変数
                Dictionary<int, List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData>> dicSelectKeyDataList = new Dictionary<int, List<SelectKeyData>>();

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
                    Key keyInfo;
                    if (sheetDefine.SheetName == "予算別")
                    {
                        // 機器の予算別の場合、"MachineId"に紐付く "LongPlanId" に置き換えて帳票を出力する
                        keyInfo = new Key("MachineId");
                        // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                        List<SelectKeyData> selectKeyDataListMachineId = getSelectKeyDataForReport(
                            ConductInfo.FormBudgetOutput.ControlId.MachineId,  // 一覧のコントールID
                            keyInfo,                                            // 設定したキー情報
                            this.resultInfoDictionary,                          // 画面データ
                            false);                                             // 選択フラグ

                        List<long> machineIdList = new List<long>();
                        foreach (SelectKeyData selectKeyData in selectKeyDataListMachineId)
                        {
                            machineIdList.Add(long.Parse(selectKeyData.Key1.ToString()));
                        }

                        // SQL取得
                        TMQUtil.GetFixedSqlStatement(SqlName.SubExcelDir, SqlName.GetLongPlanIdList, out string baseSql);
                        // 長期計画IDリストを取得
                        IList<Dao.LongPlanIdInfo> longPlanIdList = db.GetListByDataClass<Dao.LongPlanIdInfo>(
                            baseSql,
                            new { MachineIdList = machineIdList });
                        // 長期計画IDをキーリストとして作成する
                        List<SelectKeyData> selectKeyDataList = new List<SelectKeyData>();
                        foreach(var longPlanId in longPlanIdList)
                        {
                            SelectKeyData selectKeyData = new SelectKeyData();
                            selectKeyData.Key1 = longPlanId.LongPlanId;
                            // 選択キーデータリストに追加
                            selectKeyDataList.Add(selectKeyData);
                        }
                        // シートNoをキーとして帳票用選択キーデータを保存する
                        dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                    }
                    else
                    {
                        keyInfo = getKeyInfoByTargetSqlParams(sheetDefine.TargetSqlParams);
                        // 帳票用選択キーデータ取得
                        // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                        List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport(
                            ConductInfo.FormBudgetOutput.ControlId.MachineId,  // 一覧のコントールID
                            keyInfo,                                            // 設定したキー情報
                            this.resultInfoDictionary,                          // 画面データ
                            false);                                             // 選択フラグ
                        // シートNoをキーとして帳票用選択キーデータを保存する
                        dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                    }
                }

                // スケジュール関連
                // 年度開始月
                int monthStartNendo = getYearStartMonth();

                // スケジュール表示条件を取得
                var scheduleCond = GetFormDataByCtrlId<TMQDao.ScheduleList.Condition>(ConductInfo.FormBudgetOutput.ControlId.ScheduleCondition, true);
                Dao.Schedule.SearchCondition cond = new(scheduleCond, monthStartNendo, this.LanguageId);
                cond.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);

                // 長期スケジュール用オプション
                TMQUtil.Option option = new TMQUtil.Option();
                // スケジュール表示単位 1:月度、2:年度
                option.DisplayUnit = (int)cond.DisplayUnit;
                // 開始年月日
                //option.StartDate = cond.ScheduleStart;
                // 終了年月日
                //option.EndDate = cond.ScheduleEnd;
                // 出力方式 1:件名別、2:機番別、3:予算別
                // 3:予算別
                option.OutputMode = TMQUtil.ComReport.OutputMode3;
                // 年度開始月
                option.MonthStartNendo = monthStartNendo;
                // 検索条件クラス
                option.Condition = cond;

                // 予算出力画面の出力条件を取得
                var outputCond = GetFormDataByCtrlId<Dao.BudgetOutputOutputCondition>(ConductInfo.FormBudgetOutput.ControlId.OutputCondition, true);
                // 開始年月日と終了年月日は予算出力画面からに置き換え
                // 年度の開始日に変換
                DateTime startDate = ComUtil.GetNendoStartDay(outputCond.OutputPeriodFrom.GetValueOrDefault(), monthStartNendo);
                // 年度の終了日に変換
                DateTime endDate = ComUtil.GetNendoLastDay(outputCond.OutputPeriodTo.GetValueOrDefault(), monthStartNendo);
                // スケジュール表示単位 1:月度の場合
                if (option.DisplayUnit == (int)TMQConst.MsStructure.StructureId.ScheduleDisplayUnit.Month)
                {
                    // 予算出力画面の開始を年度の終了日に変換
                    endDate = ComUtil.GetNendoLastDay(outputCond.OutputPeriodFrom.GetValueOrDefault(), monthStartNendo);
                }
                // 予算開始年月日
                option.BudgetStartDate = startDate;
                // 予算終了年月日
                option.BudgetEndDate = endDate;
                // 開始年月日
                option.StartDate = startDate;
                // 終了年月日
                option.EndDate = endDate;
                cond.ScheduleStart = startDate;
                cond.ScheduleEnd = endDate;

                // スケジュール表示単位が 1:月度の場合
                if (option.DisplayUnit == (int)TMQConst.MsStructure.StructureId.ScheduleDisplayUnit.Month)
                {
                    reportId = "RP0100";
                }
                TMQUtil.CommonOutputExcel(
                    reportFactoryId,             // 工場ID
                    "LN0001",                    // プログラムID(マスタ取得のため件名別長期計画とする)
                    dicSelectKeyDataList,        // シートごとのパラメータでの選択キー情報リスト
                    searchCondition,             // 検索条件
                    reportId,                    // 帳票ID
                    1,                           // テンプレートID
                    1,                           // 出力パターンID
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
                    option);

                // OUTPUTパラメータに設定
                this.OutputFileType = fileType;
                this.OutputFileName = fileName;
                this.OutputStream = memStream;
            }

            // 出力処理が完了しました。
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, ComRes.ID.ID911120006 });
            this.Status = CommonProcReturn.ProcStatus.Valid;

            return ComConsts.RETURN_RESULT.OK;
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

        /// <summary>
        /// 年度開始月を取得する処理
        /// </summary>
        /// <param name="factoryId">工場ID 省略時はユーザの本務工場</param>
        /// <returns>年度開始月</returns>
        private int getYearStartMonth(int? factoryId = null)
        {
            int startMonth;
            if (factoryId == null)
            {
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                startMonth = TMQUtil.GetYearStartMonth(this.db, userFactoryId);
            }
            else
            {
                startMonth = TMQUtil.GetYearStartMonth(this.db, factoryId ?? -1);
            }
            return startMonth;
        }

        /// <summary>
        /// 詳細画面の保全項目一覧・点検種別毎保全項目一覧で○リンクがクリックされた際のチェック
        /// クリックされた○と同一年月に○リンクが存在するかチェックする
        /// </summary>
        /// <returns>存在する場合はTrue</returns>
        private int checkExistsOtherScheduleLink()
        {
            // クリックされた○リンクの保全スケジュール詳細ID
            string detailId = this.IndividualDictionary["MaintainanceScheduleDetailId"].ToString();

            // 保全スケジュール詳細IDから遡って機器情報を取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetEquipmentInfoByScheduleDetailId, out string sql);
            ComDao.McEquipmentEntity equipment = db.GetEntity<ComDao.McEquipmentEntity>(sql, new { MaintainanceScheduleDetailId = detailId });

            // クリックされた○リンクの機器の工場が点検種別毎管理の対象工場かどうか取得
            ComDao.McMachineEntity machine = new ComDao.McMachineEntity().GetEntity((long)equipment.MachineId, this.db);
            var list = new ComDao.MsStructureEntity().GetGroupList(TMQConst.MsStructure.GroupId.MaintainanceKindManageFactory, this.db);
            bool maintainanceKindManageFactory = list.Count(x => !x.DeleteFlg && x.FactoryId == machine.LocationFactoryStructureId) > 0;

            // 機器が点検種別毎管理の場合は機器ごとの○リンクの件数を取得する
            // →機器が点検種別毎管理かつ、機器の工場が点検種別毎管理の場合はスケジュールマークは1つにまとめられるため
            // →同一機器内で点検種別が異なる場合は上位のスケジュールマークのみリンク表示となるため
            List<string> listUnComment = new();

            // 機器が点検種別毎管理かどうかでSQLの集約方法が少し異なる
            if (equipment.MaintainanceKindManage && maintainanceKindManageFactory)
            {
                // 機器が点検種別毎管理のかつ、機器の工場が点検種別毎管理の場合
                listUnComment.Add("MaintainanceKindManage");
            }
            else
            {
                // 機器が点検種別毎管理のかつ、機器の工場が点検種別毎管理　ではない場合
                listUnComment.Add("NotMaintainanceKindManage");
            }

            // 件数を取得するためのSQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetLinkCntForTrans, out sql, listUnComment);

            // 件数を取得
            int cnt = db.GetCount(sql, new { MaintainanceScheduleDetailId = detailId });

            // 取得した件数をグローバルリストに格納(削除はjavascript側で行う)
            this.IndividualDictionary.Add("LinkCountForMA0001", cnt);

            // javascript側から渡ってきた検索条件(保全スケジュール詳細ID)をグローバルリストから削除
            this.IndividualDictionary.Remove("MaintainanceScheduleDetailId");

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion
    }
}