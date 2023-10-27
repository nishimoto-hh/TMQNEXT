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
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using SchedulePlanContent = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.SchedulePlanContent;

namespace BusinessLogic_LN0001
{
    /// <summary>
    /// 件名別長期計画(予定作業一括延期)
    /// </summary>
    public partial class BusinessLogic_LN0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 保全活動作成画面初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initPostpone()
        {
            // 長期計画IDを取得
            var param = getParam(ConductInfo.FormDetail.ControlId.Hide, false);
            // 長期計画の情報を取得して画面に設定
            initFormByLongPlanId(param, new List<string> { ConductInfo.FormPostpone.ControlId.Condition }, out bool isMaintKind, out int factoryId);
            // 一覧の値を取得
            var list = getMaintainanceList(param.LongPlanId, factoryId, isMaintKind, SchedulePlanContent.Maintainance, true);
            // ページ情報取得
            string listCtrlId = getPostponeListCtrlId(isMaintKind);
            var pageInfo = GetPageInfo(listCtrlId, this.pageInfoList);
            // 画面にセット
            SetSearchResultsByDataClass<Dao.Detail.List>(pageInfo, list, list.Count);

            // スケジュール情報のセット
            setSchedule(listCtrlId, false, param.LongPlanId, factoryId, SchedulePlanContent.Maintainance, true);

            return true;
        }

        /// <summary>
        /// 保全情報一覧の表示フラグより対応するコントロールIDを取得する
        /// </summary>
        /// <param name="isDisplayMaintainanceKind">保全情報一覧(点検種別)の場合True</param>
        /// <returns>対応する一覧のコントロールID</returns>
        private string getPostponeListCtrlId(bool isDisplayMaintainanceKind)
        {
            // 保全情報一覧(点検種別)または保全情報一覧のコントロールIDを返す
            return isDisplayMaintainanceKind ? ConductInfo.FormPostpone.ControlId.ListCheck : ConductInfo.FormPostpone.ControlId.List;
        }

        /// <summary>
        /// 予定作業一括延期画面 ヘッダの隠し項目より、一覧の表示状態を取得する
        /// </summary>
        /// <returns>保全情報一覧(点検種別)が表示されている場合True</returns>
        private bool getIsDisplayMaintainanceKindByPostpone()
        {
            var info = GetFormDataByCtrlId<Dao.ListSearchResult>(ConductInfo.FormPostpone.ControlId.Condition);
            return info.IsDisplayMaintainanceKind;
        }
        /// <summary>
        /// 予定作業一括延期画面　一覧の選択行を取得
        /// </summary>
        /// <returns>一覧の選択行のディクショナリリスト</returns>
        private List<Dictionary<string, object>> getSelectedListDicPostpone()
        {
            string listCtrlId = getPostponeListCtrlId(getIsDisplayMaintainanceKindByPostpone());
            var list = getSelectedRowsByList(this.resultInfoDictionary, listCtrlId);
            return list;
        }

        /// <summary>
        /// 一括延期処理
        /// </summary>
        /// <returns>エラーの場合-1、正常の場合1</returns>
        public int registPostpone()
        {
            // 選択行を取得
            var list = getSelectedListDicPostpone();
            // 条件を取得
            Dao.Postpone.Condition condPostpone = new();
            var condDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormPostpone.ControlId.Condition);
            SetDataClassFromDictionary(condDic, ConductInfo.FormPostpone.ControlId.Condition, condPostpone);

            // 入力チェック
            if (isErrorInputCheck())
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }
            // 更新処理
            bool result = regist();

            return result ? ComConsts.RETURN_RESULT.OK : ComConsts.RETURN_RESULT.NG;

            // 入力チェック
            bool isErrorInputCheck()
            {
                // 選択行が無ければエラー(JavaScriptでチェックするので到達不能)
                if (list == null || list.Count == 0)
                {
                    // 対象行が選択されていません。
                    this.MsgId = GetResMessage(ComRes.ID.ID941160003);
                    return true;
                }
                // 画面の内容を取得
                Dao.InputCheck.Condition chkCond = getFormDataPostpone<Dao.InputCheck.Condition>(out List<long> managementStandardsContentIdList);

                // エラー情報画面設定用(引数で渡すのが大変なので以下の処理で変更)
                List<Dictionary<string, object>> errorInfoDictionary = new();

                // 延期年月過去チェック
                if (isErrorDateOlderNow(condPostpone.PostponeDate))
                {
                    SetJsonResult(errorInfoDictionary);
                    return true;
                }

                // スケジュール日存在チェック
                if (isErrorSchedule(condPostpone.PostponeDate))
                {
                    return true;
                }

                return false;

                bool isErrorDateOlderNow(DateTime postponeDate)
                {
                    // 指定年月の1日
                    DateTime inputFirstDate = getFirstDate(postponeDate);
                    // システム日付の年月の1日
                    DateTime systemFirstDate = getFirstDate(DateTime.Now);
                    if (inputFirstDate < systemFirstDate)
                    {
                        // 過去の場合、エラー
                        // 過去日付は設定できません。
                        string errMsg = GetResMessage(ComRes.ID.ID141060003);
                        setErrorInfo(ConductInfo.FormPostpone.ControlId.Condition, "PostponeDate", errMsg);

                        return true;
                    }
                    return false;

                    void setErrorInfo(string ctrlId, string keyName, string errMsg)
                    {
                        // エラー情報を画面に設定するためのマッピング情報リスト
                        var info = getResultMappingInfo(ctrlId);
                        // エラー表示対象画面情報
                        var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                        // エラー情報格納クラス
                        ErrorInfo errorInfo = new ErrorInfo(targetDic);
                        string val = info.getValName(keyName); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                        errorInfo.setError(errMsg, val); // エラー情報をセット
                        errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    }
                }

                // スケジュールに該当するデータが存在するかチェック
                bool isErrorSchedule(DateTime postponeDate)
                {
                    // チェックする期間を設定
                    // 条件で指定された年月の最初の日と最後の日
                    chkCond.ScheduleDateFrom = getFirstDate(postponeDate);
                    chkCond.ScheduleDateTo = ComUtil.GetDateMonthLastDay(postponeDate);
                    foreach (var contentId in managementStandardsContentIdList)
                    {
                        // 機器別管理基準内容IDに選択行の値を設定(選択行に対して入力チェックするので、リストだけど1件)
                        chkCond.ManagementStandardsContentIdList = new List<long> { contentId };
                        // チェック用リストを取得
                        var inputCheckList = getListForInputCheck(chkCond);

                        // スケジュール日存在チェック
                        if (isErrorScheduleDateExists(inputCheckList))
                        {
                            return true;
                        }
                    }
                    return false;
                }

                DateTime getFirstDate(DateTime target)
                {
                    // 月初の日を取得
                    return new DateTime(target.Year, target.Month, 1);
                }
            }

            // 画面の情報を取得
            T getFormDataPostpone<T>(out List<long> managementStandardsContentIdList)
                where T : new()
            {
                // ヘッダの情報を取得
                var cond = GetFormDataByCtrlId<T>(ConductInfo.FormPostpone.ControlId.Condition);
                // 機器別管理基準内容IDに選択行の値を設定
                managementStandardsContentIdList = new();
                string listCtrlId = getPostponeListCtrlId(getIsDisplayMaintainanceKindByPostpone());
                foreach (var rowDic in list)
                {
                    Dao.Detail.List row = new();
                    SetDataClassFromDictionary(rowDic, listCtrlId, row);
                    managementStandardsContentIdList.Add(row.ManagementStandardsContentId);
                }
                return cond;
            }

            // 更新処理
            bool regist()
            {
                // 登録用情報を取得
                Dao.Postpone.SqlCondition condUpdate = getFormDataPostpone<Dao.Postpone.SqlCondition>(out List<long> managementStandardsContentIdList);
                condUpdate.ManagementStandardsContentIdList = managementStandardsContentIdList;
                condUpdate.PostponeDate = new DateTime(condUpdate.PostponeDate.Year, condUpdate.PostponeDate.Month, 1);
                // 更新ユーザ
                int userId = int.Parse(this.UserId);
                setExecuteConditionByDataClassCommon(ref condUpdate, DateTime.Now, userId, userId);
                // SQL実行
                bool result = TMQUtil.SqlExecuteClass.Regist(SqlName.Postpone.UpdateScheduleDetail, SqlName.Postpone.SubDir, condUpdate, this.db);

                return result;
            }
        }
    }
}
