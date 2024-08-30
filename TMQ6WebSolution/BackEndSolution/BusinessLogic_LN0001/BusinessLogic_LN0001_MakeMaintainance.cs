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
using SchedulePlanContent = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.SchedulePlanContent;

namespace BusinessLogic_LN0001
{
    /// <summary>
    /// 件名別長期計画(保全活動作成)
    /// </summary>
    public partial class BusinessLogic_LN0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 保全活動作成画面初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initMakeMaintainance()
        {
            // 長期計画IDを取得
            var param = getParam(ConductInfo.FormDetail.ControlId.Hide, false);
            // 長期計画の情報を取得して画面に設定
            initFormByLongPlanId(param, new List<string> { ConductInfo.FormMakeMaintainance.ControlId.Condition }, out bool isMaintKind, out int factoryId);
            // 一覧の値を取得
            var list = getMaintainanceList(param.LongPlanId, factoryId, isMaintKind, SchedulePlanContent.Maintainance, true);
            // ページ情報取得
            string listCtrlId = getMakeMaintListCtrlId(isMaintKind);
            var pageInfo = GetPageInfo(listCtrlId, this.pageInfoList);
            // 画面にセット
            SetSearchResultsByDataClass<Dao.Detail.List>(pageInfo, list, list.Count);

            // スケジュール情報のセット
            setSchedule(isMaintKind, listCtrlId, false, param.LongPlanId, factoryId, SchedulePlanContent.Maintainance, true);

            return true;
        }

        /// <summary>
        /// 保全情報一覧の表示フラグより対応するコントロールIDを取得する
        /// </summary>
        /// <param name="isDisplayMaintainanceKind">保全情報一覧(点検種別)の場合True</param>
        /// <returns>対応する一覧のコントロールID</returns>
        private string getMakeMaintListCtrlId(bool isDisplayMaintainanceKind)
        {
            // 保全情報一覧(点検種別)または保全情報一覧のコントロールIDを返す
            return isDisplayMaintainanceKind ? ConductInfo.FormMakeMaintainance.ControlId.ListCheck : ConductInfo.FormMakeMaintainance.ControlId.List;
        }

        /// <summary>
        /// 保全情報一覧画面 ヘッダの隠し項目より、一覧の表示状態を取得する
        /// </summary>
        /// <returns>保全情報一覧(点検種別)が表示されている場合True</returns>
        private bool getIsDisplayMaintainanceKindByMaint()
        {
            var info = GetFormDataByCtrlId<Dao.ListSearchResult>(ConductInfo.FormMakeMaintainance.ControlId.Condition);
            return info.IsDisplayMaintainanceKind;
        }

        /// <summary>
        /// 保全活動作成処理
        /// </summary>
        /// <returns>エラーの場合-1、正常の場合1</returns>
        public int registMakeMaintainance()
        {
            // 入力チェック
            if (isErrorInputCheckMakeMaintainance(out List<long> targetIdList))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }
            // 登録
            bool result = regist(targetIdList);

            return result ? ComConsts.RETURN_RESULT.OK : ComConsts.RETURN_RESULT.NG;

            // 入力チェック
            bool isErrorInputCheckMakeMaintainance(out List<long> targetIdList)
            {
                // 処理実施対象機器別管理基準内容IDリスト
                targetIdList = new();

                // 選択行を取得
                string listCtrlId = getMakeMaintListCtrlId(getIsDisplayMaintainanceKindByMaint());
                var list = getSelectedRowsByList(this.resultInfoDictionary, listCtrlId);
                // 選択行が無ければエラー(JavaScriptでチェックするので到達不能)
                if (list == null || list.Count == 0)
                {
                    // 対象行が選択されていません。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941160003 });
                    return true;
                }

                // ヘッダの情報を取得
                var condition = GetFormDataByCtrlId<Dao.InputCheck.Condition>(ConductInfo.FormMakeMaintainance.ControlId.Condition);
                foreach (var rowDic in list)
                {
                    Dao.Detail.List row = new();
                    SetDataClassFromDictionary(rowDic, listCtrlId, row);
                    // 機器別管理基準内容IDに選択行の値を設定(選択行に対して入力チェックするので、リストだけど1件)
                    condition.ManagementStandardsContentIdList = new List<long> { row.ManagementStandardsContentId };

                    // 入力チェック用情報を取得
                    var inputCheckList = getListForInputCheck(condition);

                    // スケジュール日存在チェック
                    if (isErrorScheduleDateExists(inputCheckList))
                    {
                        continue;
                    }
                    // 保全活動作成済みチェック
                    if (isErrorMaintainanceSummaryCreated(inputCheckList))
                    {
                        continue;
                    }
                    // チェックOKの場合、処理対象に追加
                    targetIdList.Add(row.ManagementStandardsContentId);
                }
                // 処理対象の件数が0件の場合、エラー
                bool result = targetIdList.Count == 0;
                return result;
            }

            // 登録
            bool regist(List<long> targetIdList)
            {
                bool isError = false;   // エラーが発生した場合True
                // 実行するSQLの一覧を取得
                Dictionary<string, string> sqlDic = getSqlDictionary();
                // SQLの登録条件
                Dao.MakeMaintainance.Condition condition = new(this.LanguageId);
                var dic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormMakeMaintainance.ControlId.Condition);
                SetExecuteConditionByDataClass<Dao.MakeMaintainance.Condition>(dic, ConductInfo.FormMakeMaintainance.ControlId.Condition, condition, DateTime.Now, this.UserId, this.UserId);
                // 登録キー情報
                var longplanList = getInputCheckList(targetIdList);
                // スケジュール日付でグルーピング
                var dateList = longplanList.GroupBy(x => x.ScheduleYm).Select(x => x.Key).ToList();
                // 工場IDの設定
                condition.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);

                // メッセージ表示用 登録保全活動件数
                int registCounts = 0;

                // 選択行で繰り返し
                foreach (var yearMonth in dateList)
                {
                    // 保全活動件名を登録
                    condition.SummaryId = db.RegistAndGetKeyValue<long>(sqlDic[SqlName.MakeMaint.InsertSummary], out isError, condition);
                    registCounts++;
                    if (isError)
                    {
                        return false;
                    }
                    // 保全依頼を登録
                    if (db.Regist(sqlDic[SqlName.MakeMaint.InsertRequest], condition) < 0)
                    {
                        return false;
                    }
                    // 保全計画を登録
                    if (db.Regist(sqlDic[SqlName.MakeMaint.InsertPlan], condition) < 0)
                    {
                        return false;
                    }
                    // 保全履歴を登録
                    condition.HistoryId = db.RegistAndGetKeyValue<long>(sqlDic[SqlName.MakeMaint.InsertHistory], out isError, condition);
                    if (isError)
                    {
                        return false;
                    }
                    // スケジュール日付に合致する登録対象データを取得
                    var contentList = longplanList.Where(x => x.ScheduleYm == yearMonth).ToList();
                    foreach (var content in contentList)
                    {
                        condition.ManagementStandardsContentId = content.ManagementStandardsContentId;
                        // 保全履歴機器を登録
                        condition.HistoryMachineId = db.RegistAndGetKeyValue<long>(sqlDic[SqlName.MakeMaint.InsertHistoryMachine], out isError, condition);
                        if (isError)
                        {
                            return false;
                        }
                        // 保全履歴機器部位を登録
                        condition.HistoryInspectionSiteId = db.RegistAndGetKeyValue<long>(sqlDic[SqlName.MakeMaint.InsertHistoryInspectionSite], out isError, condition);
                        if (isError)
                        {
                            return false;
                        }
                        // 保全履歴点検内容を登録
                        if (db.Regist(sqlDic[SqlName.MakeMaint.InsertHistoryInspectionContent], condition) < 0)
                        {
                            return false;
                        }
                        // 保全スケジュール詳細を更新
                        condition.MaintainanceScheduleDetailId = content.MaintainanceScheduleDetailId;
                        if (db.Regist(sqlDic[SqlName.MakeMaint.UpdateScheduleDetail], condition) < 0)
                        {
                            return false;
                        }
                    }
                }

                // メッセージ表示
                setCompleteMessageForMakeSummary(registCounts);

                // 一覧画面の選択行の長計件名IDを保持
                this.selectedLongPlanIdList.Add(condition.LongPlanId);

                return true;

                // 登録処理で使用するSQLを取得してディクショナリへ格納
                // キーはSQLのファイル名、値がSQLの内容
                Dictionary<string, string> getSqlDictionary()
                {
                    Dictionary<string, string> sqlDic = new();
                    // SQLのファイル名リスト
                    List<string> sqlNameList = new List<string>
                    {
                        SqlName.MakeMaint.InsertSummary, SqlName.MakeMaint.InsertHistory, SqlName.MakeMaint.InsertHistoryInspectionContent,
                        SqlName.MakeMaint.InsertHistoryInspectionSite, SqlName.MakeMaint.InsertPlan, SqlName.MakeMaint.InsertRequest, SqlName.MakeMaint.InsertHistoryMachine,
                        SqlName.MakeMaint.UpdateScheduleDetail
                    };
                    // ファイル名リストを繰り返しSQLを取得しディクショナリへ追加
                    sqlNameList.ForEach(x => sqlDic.Add(x, getSql(x)));

                    return sqlDic;

                    // SQLファイルの内容を取得する処理
                    string getSql(string sqlName)
                    {
                        string subDir = SqlName.MakeMaint.SubDir;
                        TMQUtil.GetFixedSqlStatement(subDir, sqlName, out string sql);
                        return sql;
                    }
                }

                // 登録用長期計画情報を取得
                List<Dao.InputCheck.Result> getInputCheckList(List<long> targetIdList)
                {
                    // ヘッダの情報を取得
                    var condition = GetFormDataByCtrlId<Dao.InputCheck.Condition>(ConductInfo.FormMakeMaintainance.ControlId.Condition);
                    // 機器別管理基準内容IDに入力チェックOKの値を設定
                    condition.ManagementStandardsContentIdList = targetIdList;
                    // 取得
                    var inputCheckList = getListForInputCheck(condition);
                    return inputCheckList;
                }
            }
        }
    }
}
