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

namespace BusinessLogic_LN0001
{
    /// <summary>
    /// 件名別長期計画(計画一括作成)
    /// </summary>
    public partial class BusinessLogic_LN0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 計画一括作成初期表示
        /// </summary>
        private void initMakePlan()
        {
            // 一覧画面で選択された行の内容を取得
            var selectedParams = getSelectedRowsByList(this.searchConditionDictionary, ConductInfo.FormList.ControlId.List);
            // データクラスに変換
            var paramClassList = convertDicListToClassList<ComDao.LnLongPlanEntity>(selectedParams, ConductInfo.FormList.ControlId.List, new List<string> { "LongPlanId" });
            // 重複した長期計画件名IDを除くため、IDでGroupByして最初の要素を取得する(クラスでDistinctできないので代用)
            paramClassList = paramClassList.GroupBy(x => x.LongPlanId).Select(x => x.First()).ToList();
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormMakePlan.ControlId.LongPlanId, this.pageInfoList);
            // 画面にセット
            SetSearchResultsByDataClass<ComDao.LnLongPlanEntity>(pageInfo, paramClassList, paramClassList.Count);
        }

        /// <summary>
        /// 計画一括作成処理
        /// </summary>
        /// <returns>エラーの場合-1、正常の場合1</returns>
        public int registMakePlan()
        {
            // 入力チェック
            if (isErrorInputMakePlan(out List<long> targetIdList))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }
            // 登録
            bool result = regist(targetIdList);

            return result ? ComConsts.RETURN_RESULT.OK : ComConsts.RETURN_RESULT.NG;

            // 入力チェック
            bool isErrorInputMakePlan(out List<long> targetIdList)
            {
                // 処理実施対象長期計画IDリスト
                targetIdList = new();

                // 一覧画面で選択された長期計画IDのリスト
                var targets = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormMakePlan.ControlId.LongPlanId);
                // 画面の条件欄に入力された内容
                var input = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormMakePlan.ControlId.Condition);

                // SQLの条件
                Dao.MakePlan.Condition condition = new(this.LanguageId);
                // 画面の条件欄、一覧画面で選択された長期計画IDを登録条件に設定
                SetDataClassFromDictionary(input, ConductInfo.FormMakePlan.ControlId.Condition, condition);

                // ユーザのマスタ存在チェック
                var user = new ComDao.MsUserEntity().GetEntity(condition.ConstructionPersonnelId ?? -1, this.db);
                if (user == null || user.DeleteFlg)
                {
                    // ユーザーがマスタに存在しません。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060004, ComRes.ID.ID911370002 });
                    return true;
                }

                // 長期計画IDで繰り返し
                foreach (var target in targets)
                {
                    Dao.InputCheck.Condition param = new();
                    // 画面の条件欄、一覧画面で選択された長期計画IDを登録条件に設定
                    SetDataClassFromDictionary(input, ConductInfo.FormMakePlan.ControlId.Condition, param); //開始日付と終了日付
                    SetDataClassFromDictionary(target, ConductInfo.FormMakePlan.ControlId.LongPlanId, param);   // 長期計画ID
                    // 長期計画IDで登録対象のデータを取得
                    var longplanList = getListForInputCheck(param);
                    // スケジュール日存在チェック
                    if (isErrorScheduleDateExists(longplanList))
                    {
                        continue;
                    }
                    // 保全活動作成済みチェック
                    if (isErrorMaintainanceSummaryCreated(longplanList))
                    {
                        continue;
                    }
                    // チェックOKの場合、処理対象に追加
                    targetIdList.Add(param.LongPlanId);
                }
                // 処理対象の件数が0件の場合、エラー
                bool result = targetIdList.Count == 0;
                return result;
            }

            // 登録処理
            bool regist(List<long> targetIdList)
            {
                // 一覧画面で選択された長期計画IDのリスト
                var targets = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormMakePlan.ControlId.LongPlanId);
                DateTime now = DateTime.Now;
                // 実行するSQLの一覧を取得
                Dictionary<string, string> sqlDic = getSqlDictionary();
                // 画面の条件欄に入力された内容
                var input = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormMakePlan.ControlId.Condition);

                // メッセージ表示用 登録保全活動件数
                int registCounts = 0;

                // 長期計画IDで繰り返し
                foreach (var target in targets)
                {
                    bool isError = false;   // エラーが発生した場合True
                    // SQLの登録条件
                    Dao.MakePlan.Condition condition = new(this.LanguageId);
                    // 画面の条件欄、一覧画面で選択された長期計画IDを登録条件に設定
                    SetDataClassFromDictionary(input, ConductInfo.FormMakePlan.ControlId.Condition, condition);
                    SetExecuteConditionByDataClass<Dao.MakePlan.Condition>(target, ConductInfo.FormMakePlan.ControlId.LongPlanId, condition, now, this.UserId, this.UserId);

                    // 処理対象の長期計画IDでなければ処理を行わない
                    if (targetIdList.IndexOf(condition.LongPlanId) <= -1)
                    {
                        continue;
                    }

                    // 工場IDの設定
                    condition.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);

                    // 長期計画IDで登録対象のデータを取得
                    Dao.InputCheck.Condition cond = new();
                    cond.LongPlanId = condition.LongPlanId;
                    cond.ScheduleDateFrom = condition.ScheduleDateFrom;
                    cond.ScheduleDateTo = condition.ScheduleDateTo;
                    var longplanList = getListForInputCheck(cond);

                    // スケジュール日付でグルーピング
                    var dateList = longplanList.GroupBy(x => x.ScheduleDate).Select(x => x.Key).ToList();
                    // スケジュール日付で繰り返し
                    foreach (var date in dateList)
                    {
                        // 保全活動件名を登録
                        condition.SummaryId = db.RegistAndGetKeyValue<long>(sqlDic[SqlName.MakePlan.InsertSummary], out isError, condition);
                        registCounts++;
                        if (isError)
                        {
                            return false;
                        }
                        // 保全依頼を登録
                        if (db.Regist(sqlDic[SqlName.MakePlan.InsertRequest], condition) < 0)
                        {
                            return false;
                        }
                        // 保全計画を登録
                        if (db.Regist(sqlDic[SqlName.MakePlan.InsertPlan], condition) < 0)
                        {
                            return false;
                        }
                        // 保全履歴を登録
                        condition.HistoryId = db.RegistAndGetKeyValue<long>(sqlDic[SqlName.MakePlan.InsertHistory], out isError, condition);
                        if (isError)
                        {
                            return false;
                        }
                        // スケジュール日付に合致する登録対象データを取得
                        var contentList = longplanList.Where(x => x.ScheduleDate == date).ToList();
                        foreach (var content in contentList)
                        {
                            condition.ManagementStandardsContentId = content.ManagementStandardsContentId;
                            // 保全履歴機器を登録
                            condition.HistoryMachineId = db.RegistAndGetKeyValue<long>(sqlDic[SqlName.MakePlan.InsertHistoryMachine], out isError, condition);
                            if (isError)
                            {
                                return false;
                            }
                            // 保全履歴機器部位を登録
                            condition.HistoryInspectionSiteId = db.RegistAndGetKeyValue<long>(sqlDic[SqlName.MakePlan.InsertHistoryInspectionSite], out isError, condition);
                            if (isError)
                            {
                                return false;
                            }
                            // 保全履歴点検内容を登録
                            if (db.Regist(sqlDic[SqlName.MakePlan.InsertHistoryInspectionContent], condition) < 0)
                            {
                                return false;
                            }
                            // 保全スケジュール詳細を更新
                            condition.MaintainanceScheduleDetailId = content.MaintainanceScheduleDetailId;
                            if (db.Regist(sqlDic[SqlName.MakePlan.UpdateScheduleDetail], condition) < 0)
                            {
                                return false;
                            }
                        }
                    }
                }

                // メッセージ表示
                setCompleteMessageForMakeSummary(registCounts);

                return true;

                // 登録処理で使用するSQLを取得してディクショナリへ格納
                // キーはSQLのファイル名、値がSQLの内容
                Dictionary<string, string> getSqlDictionary()
                {
                    Dictionary<string, string> sqlDic = new();
                    // SQLのファイル名リスト
                    List<string> sqlNameList = new List<string>
                    {
                        SqlName.MakePlan.InsertSummary, SqlName.MakePlan.InsertHistory, SqlName.MakePlan.InsertHistoryInspectionContent,
                        SqlName.MakePlan.InsertHistoryInspectionSite, SqlName.MakePlan.InsertPlan, SqlName.MakePlan.InsertRequest, SqlName.MakePlan.InsertHistoryMachine,
                        SqlName.MakePlan.UpdateScheduleDetail
                    };
                    // ファイル名リストを繰り返しSQLを取得しディクショナリへ追加
                    sqlNameList.ForEach(x => sqlDic.Add(x, getSql(x)));

                    return sqlDic;

                    // SQLファイルの内容を取得する処理
                    string getSql(string sqlName)
                    {
                        string subDir = SqlName.MakePlan.SubDir;
                        TMQUtil.GetFixedSqlStatement(subDir, sqlName, out string sql);
                        return sql;
                    }
                }
            }
        }
    }
}
