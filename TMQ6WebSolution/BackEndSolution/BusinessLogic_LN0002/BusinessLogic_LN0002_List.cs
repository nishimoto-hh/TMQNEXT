using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_LN0002.BusinessLogicDataClass_LN0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using ScheduleStatus = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleStatus;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;

namespace BusinessLogic_LN0002
{
    /// <summary>
    /// 機器別長期計画(一覧画面)
    /// </summary>
    public partial class BusinessLogic_LN0002
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
            SetSysFiscalYear<TMQDao.ScheduleList.Condition>(ConductInfo.FormList.CtrlId.ScheduleCondition, monthStartNendo);

            // 一覧データを取得して設定
            if (!setListData(out List<string> keyIdList, out IList<Dao.FormList.List> resultsWithHead, out List<string> longPlanIdList))
            {
                return false;
            }
            // スケジュール関連
            setSchedule(keyIdList, resultsWithHead, longPlanIdList);
            return true;

            // 一覧データの取得と設定(スケジュール以外)
            // return bool 後続の処理を行わない場合True
            // out List<string> keyIdList 取得したスケジュール一覧と紐づけるIDのリスト
            // out IList<Dao.FormList.List> resultsHead 機器ごとにブランクのヘッダを追加したリスト
            // out List<string> longPlanIdList 取得した一覧の長計件名IDのリスト
            bool setListData(out List<string> keyIdList, out IList<Dao.FormList.List> resultsWithHead, out List<string> longPlanIdList)
            {
                keyIdList = new();
                resultsWithHead = new List<Dao.FormList.List>();
                longPlanIdList = new();

                // 一覧のページ情報取得
                var pageInfo = GetPageInfo(ConductInfo.FormList.CtrlId.List, this.pageInfoList);

                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetList, out string baseSql);
                // WITH句は別に取得
                TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetList, out string withSql);
                // 場所分類＆職種機種＆詳細検索条件取得
                if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
                {
                    return false;
                }
                //SQLパラメータに言語ID設定
                whereParam.LanguageId = this.LanguageId;
                // SQL、WHERE句、WITH句より件数取得SQLを作成
                string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);
                // 一時テーブル設定
                setTempTable();
                // 一覧検索SQL文の取得
                executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);
                var selectSql = new StringBuilder(executeSql);
                selectSql.AppendLine(" ORDER BY machine_no ,machine_name ");
                selectSql.AppendLine(" ,inspection_site_structure_id ,inspection_site_importance_structure_id ");
                selectSql.AppendLine(" ,inspection_site_conservation_structure_id ,inspection_content_structure_id ");
                // 一覧検索実行
                IList<Dao.FormList.List> results = db.GetListByDataClass<Dao.FormList.List>(selectSql.ToString(), whereParam);
                if (results == null || results.Count == 0)
                {
                    this.Status = CommonProcReturn.ProcStatus.Warning;
                    // 「該当データがありません。」
                    this.MsgId = GetResMessage(ComRes.ID.ID941060001);
                    SetSearchResultsByDataClass<Dao.FormList.List>(pageInfo, null, 0, isDetailConditionApplied);
                    return false;
                }
                // ヘッダにスケジュール集計を付与するのでヘッダ行を追加
                resultsWithHead = getAddBlankToList(results);
                // 検索結果の設定
                if (SetSearchResultsByDataClassForList<Dao.FormList.List>(pageInfo, resultsWithHead, results.Count, isDetailConditionApplied))
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                }
                keyIdList = results.Select(x => x.KeyId).Distinct().ToList(); // スケジュール一覧との紐付用(詳細条件検索により絞り込まれる場合を想定)
                longPlanIdList = results.Select(x => x.LongPlanId.ToString()).Distinct().ToList(); // スケジュール一覧の絞り込み用
                return true;

                void setTempTable()
                {
                    TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
                    // 翻訳用一時テーブル登録
                    // 翻訳する構成グループのリスト
                    var structuregroupList = new List<GroupId> { GroupId.SiteMaster, GroupId.Importance, GroupId.Conservation, GroupId.InspectionDetails, GroupId.BudgetPersonality, GroupId.MaintainanceKind };
                    listPf.GetCreateTranslation(); // テーブル作成
                    listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
                    listPf.RegistTempTable(); // 登録
                }

                // 検索結果にグループごとに空行を入れる（スケジュールマークの集計を追加するので、検索結果にも対応した行が必要なため）
                IList<Dao.FormList.List> getAddBlankToList(IList<Dao.FormList.List> getList)
                {
                    // 戻り値の結果リスト、グループごとに空行を追加した結果
                    IList<Dao.FormList.List> rtnList = new List<Dao.FormList.List>();
                    // 処理済みのグループIDを保持するリスト
                    List<long> groupIdList = new();
                    foreach (var row in getList)
                    {
                        // グループIDが処理済みか判定
                        if (groupIdList.IndexOf(row.ListGroupId) < 0)
                        {
                            // 無い場合
                            groupIdList.Add(row.ListGroupId);
                            rtnList.Add(row.MakeGroupHead()); // 現在のデータを空行として追加(表示で変更するのでそのまま追加して問題ない)
                        }
                        // 現在のデータを追加
                        rtnList.Add(row);
                    }
                    return rtnList;
                }
            }

            void setSchedule(List<string> keyIdList, IList<Dao.FormList.List> resultsWithHead, List<string> longPlanIdList)
            {
                // 画面の条件を取得
                var scheduleCond = GetFormDataByCtrlId<TMQDao.ScheduleList.Condition>(ConductInfo.FormList.CtrlId.ScheduleCondition, false);
                Dao.Schedule.SearchCondition cond = new(scheduleCond, monthStartNendo, this.LanguageId);
                cond.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);
                // 検索条件に画面に表示する長計件名IDを設定(多数だとSQLエラーになるのでカンマ区切り)
                cond.KeyIdList = string.Join(",", keyIdList);

                // 個別実装用データへスケジュールのレイアウトデータ(scheduleLayout)をセット
                TMQUtil.ScheduleListUtil.SetLayout(ref this.IndividualDictionary, cond, false, getNendoText(), monthStartNendo);

                // スケジュール一覧表示用データの取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetScheduleList, out string sqlSchedule);
                IList<TMQDao.ScheduleList.Get> scheduleList = db.GetListByDataClass<TMQDao.ScheduleList.Get>(sqlSchedule, cond);

                // 取得したデータを画面表示用に変換、マークなど取得
                TMQUtil.ScheduleListConverterNoRank listSchedule = new();
                List<TMQDao.ScheduleList.Display> scheduleDisplayList = listSchedule.Execute(scheduleList, cond, monthStartNendo, true, this.db, getScheduleLinkInfo());
                var scheduleWithHead = getAddHeadToSchedule(scheduleDisplayList, resultsWithHead);
                // 画面設定用データに変換
                Dictionary<string, Dictionary<string, string>> setScheduleData = TMQUtil.ScheduleListUtil.ConvertDictionaryAddData(scheduleWithHead, cond);

                // 画面に設定
                SetScheduleDataToResult(setScheduleData, ConductInfo.FormList.CtrlId.List);

                return;

                /// <summary>
                /// スケジュールのマークからのリンク先を設定する処理
                /// </summary>
                /// <returns>マークごとのリンク先の情報</returns>
                Dictionary<ScheduleStatus, TMQDao.ScheduleList.Display.LinkTargetInfo> getScheduleLinkInfo()
                {
                    Dictionary<ScheduleStatus, TMQDao.ScheduleList.Display.LinkTargetInfo> result = new();
                    // ●：保全活動参照画面(履歴タブ)
                    result.Add(ScheduleStatus.Complete, new(MA0001.ConductId, MA0001.FormNo.Detail, MA0001.TabNoDetail.History));
                    // ◎：保全活動参照画面(依頼タブ)
                    result.Add(ScheduleStatus.Created, new(MA0001.ConductId, MA0001.FormNo.Detail, MA0001.TabNoDetail.Request));
                    // TODO:この行をコメントアウトすればリンクしなくなる
                    // ○：保全活動新規登録画面
                    result.Add(ScheduleStatus.NoCreate, new(MA0001.ConductId, MA0001.FormNo.New, MA0001.TabNoDetail.New));
                    return result;
                }

                /// <summary>
                /// スケジュールレイアウト作成に必要な「年度」の文言を取得する処理
                /// </summary>
                /// <returns>"年度"に相当する文言</returns>
                string getNendoText()
                {
                    // 「{0}年度」
                    return GetResMessage(ComRes.ID.ID150000013);
                }

                List<TMQDao.ScheduleList.Display> getAddHeadToSchedule(List<TMQDao.ScheduleList.Display> scheduleList, IList<Dao.FormList.List> resultsHead)
                {
                    // 戻り値のスケジュールリスト、グループごとに集計したスケジュールを追加した結果
                    List<TMQDao.ScheduleList.Display> rtnList = new();
                    // 処理済みのグループIDと年月日を保持するリスト
                    Dictionary<long, Dictionary<DateTime, bool>> finishList = new();
                    foreach (var schedule in scheduleList)
                    {
                        long listGroupId = getListGroupId(schedule.KeyId); // 処理対象グループ(行をまとめる単位)
                        DateTime keyDate = schedule.KeyDate; // 処理対象年月日
                        if (!finishList.ContainsKey(listGroupId))
                        {
                            // グループで未処理の場合、処理済みリストへ追加
                            finishList.Add(listGroupId, new());
                        }
                        var groupDic = finishList[listGroupId];
                        // グループで処理がある場合、日付で処理済みか確認
                        if (groupDic.ContainsKey(keyDate))
                        {
                            // グループでも日付でも処理済みの場合、集計を行わずにスキップ
                            rtnList.Add(schedule);
                            continue;
                        }
                        // 日付で未処理の場合、処理済みリストへ処理年月日を追加
                        groupDic.Add(keyDate, true);

                        // グループと日付でスケジュールを集計し、ヘッダ行用として追加する
                        var targets = scheduleList.Where(x => x.KeyDate == keyDate && getListGroupId(x.KeyId) == listGroupId); // 集計対象
                                                                                                                               // 優先順位→点検種別の順に集計
                        var target = targets.OrderBy(x => x.StatusPriority).ThenBy(x => x.MaintainanceKindLevel).First();
                        rtnList.Add(makeGroupHead(target, listGroupId));//キーをヘッダ用にグループIDに変更

                        // 本来の内容を追加
                        rtnList.Add(schedule);
                    }

                    return rtnList;

                    // キーIDからグループのキーを取得
                    long getListGroupId(string keyId)
                    {
                        return resultsHead.First(x => x.KeyId == keyId).ListGroupId;
                    }

                    // グループのヘッダ用のスケジュールを作成
                    TMQDao.ScheduleList.Display makeGroupHead(TMQDao.ScheduleList.Display param, long listGroupId)
                    {
                        // 選択されたスケジュールの情報をコピーする
                        TMQDao.ScheduleList.Display rtn = new();
                        // キーIDはグループID(一覧と同じ)
                        rtn.KeyId = listGroupId.ToString();
                        // リンクは無し
                        rtn.IsLink = false;
                        rtn.KeyDate = param.KeyDate;
                        rtn.MaintainanceKindChar = param.MaintainanceKindChar;
                        rtn.MaintainanceKindLevel = param.MaintainanceKindLevel;
                        rtn.StatusId = param.StatusId;
                        rtn.StatusPriority = param.StatusPriority;
                        return rtn;
                    }
                }
            }

            /// <summary>
            /// 年度開始月を取得する処理
            /// </summary>
            /// <param name="factoryId">工場ID 省略時はユーザの本務工場</param>
            /// <returns>年度開始月</returns>
            int getYearStartMonth(int? factoryId = null)
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
        }
    }
}
