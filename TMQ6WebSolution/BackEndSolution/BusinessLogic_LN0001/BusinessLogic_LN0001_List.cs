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
            // 一覧のページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);
            // 年度開始月
            int monthStartNendo = getYearStartMonth();
            // システム年度初期化処理
            SetSysFiscalYear<TMQDao.ScheduleList.Condition>(ConductInfo.FormList.ControlId.ScheduleCondition, monthStartNendo);
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.List.SubDir, SqlName.List.GetList, out string baseSql);
            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.List.SubDir, SqlName.List.GetList, out string withSql);
            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
            {
                return false;
            }
            //SQLパラメータに言語ID設定
            whereParam.LanguageId = this.LanguageId;
            // SQL、WHERE句、WITH句より件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);
            // 総件数を取得
            int cnt = db.GetCount(executeSql, whereParam);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                SetSearchResultsByDataClass<Dao.ListSearchResult>(pageInfo, null, cnt, isDetailConditionApplied);
                return false;
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY subject");
            // 一覧検索実行
            IList<Dao.ListSearchResult> results = db.GetListByDataClass<Dao.ListSearchResult>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.ListSearchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.ListSearchResult>(pageInfo, results, cnt, isDetailConditionApplied))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            List<string> keyIdList = results.Select(x => x.KeyId).Distinct().ToList(); // スケジュール一覧との紐付用(詳細条件検索により絞り込まれる場合を想定)

            // スケジュール関連
            // 画面の条件を取得
            var scheduleCond = GetFormDataByCtrlId<TMQDao.ScheduleList.Condition>(ConductInfo.FormList.ControlId.ScheduleCondition, false);
            Dao.Schedule.SearchCondition cond = new(scheduleCond, monthStartNendo, this.LanguageId);
            cond.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);

            // 個別実装用データへスケジュールのレイアウトデータ(scheduleLayout)をセット
            TMQUtil.ScheduleListUtil.SetLayout(ref this.IndividualDictionary, cond, false, getNendoText(), monthStartNendo);

            // スケジュール一覧表示用データの取得
            TMQUtil.GetFixedSqlStatement(SqlName.List.SubDir, SqlName.List.GetListSchedule, out string sqlSchedule);
            IList<TMQDao.ScheduleList.Get> scheduleList = db.GetListByDataClass<TMQDao.ScheduleList.Get>(sqlSchedule, cond);
            scheduleList = scheduleList.Where(x => keyIdList.Contains(x.KeyId)).ToList(); // 一覧と紐づくものだけを表示

            // 取得したデータを画面表示用に変換、マークなど取得
            TMQUtil.ScheduleListConverterNoRank listSchedule = new();
            List<TMQDao.ScheduleList.Display> scheduleDisplayList = listSchedule.Execute(scheduleList, cond, monthStartNendo, true, this.db, getScheduleLinkInfo());

            // 画面設定用データに変換
            Dictionary<string, Dictionary<string, string>> setScheduleData = TMQUtil.ScheduleListUtil.ConvertDictionaryAddData(scheduleDisplayList, cond);

            // 画面に設定
            SetScheduleDataToResult(setScheduleData, ConductInfo.FormList.ControlId.List);

            // 非表示項目
            // 変更管理ボタンの表示制御用フラグ
            setHistoryManagementFlg(ConductInfo.FormList.ControlId.HiddenInfo);

            return true;
        }
    }
}
