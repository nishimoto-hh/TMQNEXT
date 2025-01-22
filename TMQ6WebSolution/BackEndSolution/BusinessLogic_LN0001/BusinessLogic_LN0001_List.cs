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
using Dao = BusinessLogic_LN0001.BusinessLogicDataClass_LN0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using CommonWebTemplate.CommonDefinitions;

namespace BusinessLogic_LN0001
{
    /// <summary>
    /// 件名別長期計画(一覧)
    /// </summary>
    public partial class BusinessLogic_LN0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // 年度開始月
            int monthStartNendo = getYearStartMonth();
            // システム年度初期化処理
            SetSysFiscalYear<TMQDao.ScheduleList.Condition>(ConductInfo.FormList.ControlId.ScheduleCondition, monthStartNendo);

            // 非表示項目
            // 変更管理ボタンの表示制御用フラグ
            setHistoryManagementFlg(ConductInfo.FormList.ControlId.HiddenInfo);

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            // メニューから選択された場合かつ、有効な詳細検索条件が設定されていない場合は初期検索は行わない
            if (this.CtrlId == "Init" && isNotConfiguredDetailCondition())
            {
                // 検索結果の設定
                if (SetSearchResultsByDataClassForList<Dao.ListSearchResult>(pageInfo, new List<Dao.ListSearchResult>(), 0))
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                }

                // 一覧画面の詳細検索を表示状態(虫眼鏡マークをクリックした後の状態)にするため、グローバル変数にキーを格納
                // キーが格納されているかどうかなので値は空とする
                SetGlobalData("InitDispDetailCondition", string.Empty);

                return true;
            }

            // 項目カスタマイズで選択されている項目のみSELECTする
            List<string> uncommentList = getDisplayCustomizeCol(ConductInfo.FormList.ControlId.List);

            // 一覧データを取得して設定
            if (!setListData(out List<string> keyIdList, uncommentList))
            {
                return false;
            }

            // スケジュール関連
            setSchedule(keyIdList, monthStartNendo);

            return true;

            // 一覧データの取得と設定(スケジュール以外)
            // return bool 後続の処理を行わない場合True
            // out List<string> keyIdList 取得したスケジュール一覧と紐づけるIDのリスト
            bool setListData(out List<string> keyIdList, List<string> uncommentList)
            {
                keyIdList = new();

                // 一覧のページ情報取得
                var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

                // SQLを取得
                uncommentList.Add(UnCommentWordOfGetListNotForExcelPort);
                TMQUtil.GetFixedSqlStatement(SqlName.List.SubDir, SqlName.List.GetList, out string baseSql, uncommentList);

                // 場所分類＆職種機種＆詳細検索条件取得
                if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
                {
                    return false;
                }
                // 一時テーブル設定
                setTempTableForGetList(uncommentList: uncommentList);
                //SQLパラメータに言語ID設定
                whereParam.LanguageId = this.LanguageId;

                // WITH句は別に取得
                TMQUtil.GetFixedSqlStatementWith(SqlName.List.SubDir, SqlName.List.GetList, out string withSql, uncommentList);

                // 総件数を取得
                int cnt = db.GetCount(TMQUtil.GetCountSql(new ComDao.LnLongPlanEntity().TableName, whereParam), whereParam);
                // 総件数のチェック
                if (!CheckSearchTotalCount(cnt, pageInfo))
                {
                    this.Status = CommonProcReturn.ProcStatus.Warning;
                    // 「該当データがありません。」
                    this.MsgId = GetResMessage(CommonResources.ID.ID941060001);
                    SetSearchResultsByDataClass<Dao.ListSearchResult>(pageInfo, null, cnt, isDetailConditionApplied);
                    return false;
                }

                //項目カスタマイズでSELECT項目を絞るので、詳細検索条件は機能側で付与する
                StringBuilder baseSqlSb = new StringBuilder(baseSql);
                baseSqlSb.AppendLine().AppendLine(whereSql);

                // 一覧検索SQL文の取得
                var executeSql = TMQUtil.GetSqlStatementSearch(false, baseSqlSb.ToString(), null, withSql, isDetailConditionApplied, pageInfo.SelectMaxCnt);
                var selectSql = new StringBuilder(executeSql);
                selectSql.AppendLine("ORDER BY subject");
                // 一覧検索実行
                IList<Dao.ListSearchResult> results = db.GetListByDataClass<Dao.ListSearchResult>(selectSql.ToString(), whereParam);
                if (results == null || results.Count == 0)
                {
                    this.Status = CommonProcReturn.ProcStatus.Warning;
                    // 「該当データがありません。」
                    this.MsgId = GetResMessage(CommonResources.ID.ID941060001);
                    SetSearchResultsByDataClass<Dao.ListSearchResult>(pageInfo, null, 0, isDetailConditionApplied);
                    return false;
                }

                // 検索結果の設定
                if (SetSearchResultsByDataClassForList<Dao.ListSearchResult>(pageInfo, results, cnt, isDetailConditionApplied))
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                }
                keyIdList = results.Select(x => x.KeyId).Distinct().ToList(); // スケジュール一覧との紐付用(詳細条件検索により絞り込まれる場合を想定)

                //グローバルリストへ総件数を設定
                SetGlobalData(GlobalKey.LN0001AllListCount, isDetailConditionApplied ? results.Count : cnt);

                // 検索結果リストを解放
                results = null;
                GC.Collect();

                return true;
            }

            // スケジュールを取得して星取表を設定する
            // List<string> keyIdList 取得したスケジュール一覧と紐づけるIDのリスト
            void setSchedule(List<string> keyIdList, int monthStartNendo)
            {
                // 画面の条件を取得
                var scheduleCond = GetFormDataByCtrlId<TMQDao.ScheduleList.Condition>(ConductInfo.FormList.ControlId.ScheduleCondition, false);
                Dao.Schedule.SearchCondition cond = new(scheduleCond, monthStartNendo, this.LanguageId);
                cond.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);

                // 一覧画面の表示条件を個別実装用データへセット
                SetGlobalData(GlobalKey.LN0001ListCondition, cond.Copy());

                // 個別実装用データへスケジュールのレイアウトデータ(scheduleLayout)をセット
                TMQUtil.ScheduleListUtil.SetLayout(ref this.IndividualDictionary, cond, false, getNendoText(), monthStartNendo);

                // 検索条件に画面に表示する長計件名IDを設定(多数だとSQLエラーになるのでカンマ区切り)
                cond.LongPlanIdList = string.Join(",", keyIdList);

                // スケジュール一覧表示用データの取得
                TMQUtil.GetFixedSqlStatement(SqlName.List.SubDir, SqlName.List.GetListSchedule, out string sqlSchedule);
                IList<TMQDao.ScheduleList.Get> scheduleList = db.GetListByDataClass<TMQDao.ScheduleList.Get>(sqlSchedule, cond);

                // 取得したデータを画面表示用に変換、マークなど取得
                TMQUtil.ScheduleListConverterNoRank listSchedule = new();
                List<TMQDao.ScheduleList.Display> scheduleDisplayList = listSchedule.Execute(scheduleList, cond, monthStartNendo, true, this.db, getScheduleLinkInfo());

                // 画面設定用データに変換
                Dictionary<string, Dictionary<string, string>> setScheduleData = TMQUtil.ScheduleListUtil.ConvertDictionaryAddData(scheduleDisplayList, cond);

                // 画面に設定
                SetScheduleDataToResult(setScheduleData, ConductInfo.FormList.ControlId.List);
            }

            // 有効な詳細検索条件が設定されているかどうか判定
            bool isNotConfiguredDetailCondition()
            {
                // 詳細検索条件を取得する
                var list = this.searchConditionDictionary.Where(x => x.ContainsKey("IsDetailCondition") && x.ContainsKey("CTRLID")).ToList();
                var dic = list.Where(x => x["IsDetailCondition"].Equals(true) && x["CTRLID"].Equals(pageInfo.CtrlId)).FirstOrDefault();

                // ディクショナリを絞り込んだ結果がNULLかどうかで詳細検索が設定されているかどうかとする
                // NULL：詳細検索が設定されていない、NULLでない：詳細検索が設定されている
                return dic == null;
            }
        }
    }
}
