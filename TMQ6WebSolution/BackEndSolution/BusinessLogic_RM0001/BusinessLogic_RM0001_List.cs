using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using ComConsts = CommonSTDUtil.CommonConstants;
using Dao = BusinessLogic_RM0001.BusinessLogicDataClass_RM0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Const = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_RM0001
{
    /// <summary>
    /// 一覧画面
    /// </summary>
    public partial class BusinessLogic_RM0001 : CommonBusinessLogicBase
    {

        #region privateメソッド

        #region 検索処理

        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="patternId">パターンID(パターン新規登録後に格納されている)</param>
        /// <returns>エラーの場合False</returns>
        private bool searchList(int? patternId = null)
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            List<string> listUnComment = new List<string>();

            dynamic whereParam = new ExpandoObject();

            // 検索条件設定
            dynamic conditionObj = new ExpandoObject();
            // 画面の検索条件と、画面項目定義拡張テーブルの検索条件の項目の値より、検索条件を設定
            SetSearchCondition(this.searchConditionDictionary, ConductInfo.FormList.ControlId.Condition, conditionObj, pageInfo);

            // 検索条件にテンプレートが選択されていればコメントを外す
            if (conditionObj.TemplateId != null && string.IsNullOrEmpty(conditionObj.TemplateId) == false)
            {
                listUnComment.Add("TemplateId");
            }
            // 検索条件にテンプレートが選択されていなければ、未指定用のコメントを外す
            else
            {
                listUnComment.Add("NoConditionTemplateId");
            }
            // 検索条件に出力パターンが選択されていればコメントを外す
            if (conditionObj.OutputPatternId != null && string.IsNullOrEmpty(conditionObj.OutputPatternId) == false)
            {
                listUnComment.Add("OutputPatternId");
            }
            // 検索条件に出力パターンが選択されていなければ、未指定用のコメントを外す
            else
            {
                listUnComment.Add("NoConditionOutputPatternId");
            }

            // パターンの登録後の再検索の場合
            if (patternId != null)
            {
                // 検索条件の値を登録後のパターンIDで上書きする
                conditionObj.OutputPatternId = patternId;

                // 登録後のパターンIDを使用するのでアンコメントリストも変更する
                // 検索条件のパターンが選択されている状態にする
                if (!listUnComment.Contains("OutputPatternId"))
                {
                    listUnComment.Add("OutputPatternId");
                }
                if (listUnComment.Contains("NoConditionOutputPatternId"))
                {
                    listUnComment.Remove("NoConditionOutputPatternId");
                }
            }

            // 検索SQLの取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetReportList, out string baseSql, listUnComment);

            string whereClause = string.Empty;
            bool isDetailConditionApplied = false;

            // 一覧検索SQL文の取得
            string execSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereClause, string.Empty);

            // 総件数を取得
            int cnt = db.GetCount(execSql, conditionObj);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            // 一覧検索SQL文の取得
            execSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereClause, string.Empty);
            var selectSql = new StringBuilder(execSql);

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), conditionObj);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results, results.Count, isDetailConditionApplied))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            changeFormListBtnEnabled(PageStatus.Search, patternId);

            return true;
        }

        #endregion 検索処理

        #region テンプレートダウンロード
        /// <summary>
        /// 編集画面　テンプレートダウンロード処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private int executeDownload()
        {
            int result = 0;
            try
            {
                // 検索条件を出力処理用の変数に設定
                setReportImplDataByCondition(out int factoryId, out string reportId, out int templateId, out int outputPatternId,
                                            out string programId, out string templateFilePath, out string templateFileName);

                // テンプレートファイルダウンロード
                if (!DownloadTemplate(templateFilePath, templateFileName))
                {
                    return -1;
                }
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

            return result;

        }

        /// <summary>
        /// 指定したパスからテンプレートファイルをダウンロード可能にする
        /// </summary>
        /// <param name="templateFilePath">フォルダ(ファイル)のパス</param>
        /// <param name="templateFileName">ダウンロードするファイルの名称</param>
        /// <returns>エラーの場合False</returns>
        private bool DownloadTemplate(string templateFilePath, string templateFileName)
        {
            try
            {
                // ファイルをダウンロードさせる
                var fileStream = new MemoryStream();

                // 実行パス取得
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                string templateFolder = Path.Combine(appPath, CommonWebTemplate.AppCommonObject.Config.AppSettings.ExcelTemplateDir);
                templateFolder = Path.Combine(templateFolder, templateFilePath);

                var filePath = string.Format(templateFolder + "\\{0}", templateFileName);

                using (FileStream file = new(filePath, FileMode.Open, FileAccess.Read))
                {
                    file.CopyTo(fileStream);
                }
                // 画面の出力へ設定
                this.OutputFileType = CommonConstants.REPORT.FILETYPE.EXCEL;
                this.OutputFileName = templateFileName;
                this.OutputStream = fileStream;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        #endregion

        #region 出力処理

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private int executeOutputReport()
        {
            try
            {
                this.ResultList = new();

                // グローバル変数を出力処理用の変数に設定
                setReportImplDataByGlobalData(out string parentPgmId, out string targetCtrlId, out string targetOptionCtrlId, out Dictionary<string, object> dicOptionData);

                // 検索条件を出力処理用の変数に設定
                setReportImplDataByCondition(out int factoryId, out string reportId, out int templateId, out int outputPatternId,
                                            out string programId, out string templateFilePath, out string templateFileName);

                // 遷移元画面のマッピング情報を取得する
                AddMappingListOtherPgmId(parentPgmId);

                // シートごとの帳票用選択キーデータ設定変数
                Dictionary<int, List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData>> dicSelectKeyDataList = new Dictionary<int, List<SelectKeyData>>();
                // 帳票定義取得
                // 出力帳票シート定義のリストを取得
                var sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                    TMQUtil.ComReport.GetReportSheetDefine,
                    TMQUtil.ExcelPath,
                    new { FactoryId = factoryId, ReportId = reportId },
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
                        targetCtrlId,                // 一覧のコントールID
                        keyInfo,                     // 設定したキー情報
                        this.resultInfoDictionary);  // 画面データ

                    // シートNoをキーとして帳票用選択キーデータを保存する
                    dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                }

                // 帳票出力キー情報を取得する。

                // 長期スケジュール用オプションの設定
                TMQUtil.Option option = null;
                // 画面側に対象エリアが存在、且つオプション使用可能帳票の場合
                if (string.IsNullOrEmpty(targetOptionCtrlId) == false && isUseOptionReport(reportId) == true && dicOptionData != null)
                {
                    // オプションデータを抽出して条件クラスにセットする
                    TMQDao.ScheduleList.Condition target = new();
                    SetDataClassFromDictionary(dicOptionData, targetOptionCtrlId, target);

                    option = new TMQUtil.Option();
                    // 年度開始月
                    int monthStartNendo = getYearStartMonth();
                    Dao.SearchCondition cond = new(target, monthStartNendo, this.LanguageId);
                    cond.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);

                    // スケジュール表示単位 1:月度、2:年度
                    //option.DisplayUnit = (int)cond.DisplayUnit;
                    if (reportId == "RP0100")
                    {
                        // 年度スケジュール表の場合、1:月度
                        option.DisplayUnit = (int)Const.MsStructure.StructureId.ScheduleDisplayUnit.Month;
                        cond.DisplayUnit = Const.MsStructure.StructureId.ScheduleDisplayUnit.Month;
                    }
                    else
                    {
                        // 長期スケジュール表、2:年度
                        option.DisplayUnit = (int)Const.MsStructure.StructureId.ScheduleDisplayUnit.Year;
                        cond.DisplayUnit = Const.MsStructure.StructureId.ScheduleDisplayUnit.Year;
                    }
                    // 開始年月日
                    option.StartDate = cond.ScheduleStart;
                    // 終了年月日
                    if (reportId == "RP0100")
                    {
                        // 年度スケジュールの場合
                        option.EndDate = ComUtil.GetNendoLastDay(cond.ScheduleStart, monthStartNendo);
                        cond.ScheduleEnd = ComUtil.GetNendoLastDay(cond.ScheduleStart, monthStartNendo);
                    }
                    else
                    {
                        option.EndDate = cond.ScheduleEnd;
                    }

                    if (parentPgmId == "LN0002")
                    {
                        // 出力方式 1:件名別、2:機番別、3:予算別
                        option.OutputMode = TMQUtil.ComReport.OutputMode2;
                    }
                    else
                    {
                        // 出力方式 1:件名別、2:機番別、3:予算別
                        option.OutputMode = TMQUtil.ComReport.OutputMode1;
                    }
                    // 年度開始月
                    option.MonthStartNendo = monthStartNendo;
                    // 検索条件クラス
                    option.Condition = cond;
                }

                // 検索条件データ取得
                // 一覧のコントロールID指定で取得
                getSearchConditionByTargetCtrlIdForReport(targetCtrlId, out dynamic searchCondition);

                // エクセル出力共通処理
                TMQUtil.CommonOutputExcel(
                    factoryId,                   // 工場ID
                    parentPgmId,                 // プログラムID
                    dicSelectKeyDataList,        // シートごとのパラメータでの選択キー情報リスト
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
                    option);

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
        /// 対象帳票がオプション使用帳票かどうかを判定する
        /// </summary>
        /// <param name="reportId">帳票ID</param>
        /// <returns>true：対象、false：対象外</returns>
        private bool isUseOptionReport(string reportId)
        {
            var typeReportId = typeof(UseOptionReportIdList);
            foreach (var field in typeReportId.GetFields())
            {
                if (reportId.Equals(field.GetValue(typeReportId)))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion 出力処理

        #endregion privateメソッド
    }
}
