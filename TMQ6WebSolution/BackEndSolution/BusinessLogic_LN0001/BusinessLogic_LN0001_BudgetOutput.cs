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
    /// 件名別長期計画(予算出力)
    /// </summary>
    public partial class BusinessLogic_LN0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 予算出力初期表示
        /// </summary>
        private void initBudgetOutput()
        {
            //******************************************************************
            //*** 一覧画面で選択されたキー情報を予算出力画面の隠し項目に設定 ***
            //******************************************************************
            // 一覧画面で選択された行の内容を取得
            var selectedParams = getSelectedRowsByList(this.searchConditionDictionary, ConductInfo.FormList.ControlId.List);
            // データクラスに変換
            var paramClassList = convertDicListToClassList<ComDao.LnLongPlanEntity>(selectedParams, ConductInfo.FormList.ControlId.List, new List<string> { "LongPlanId" });
            // 重複した長期計画件名IDを除くため、IDでGroupByして最初の要素を取得する(クラスでDistinctできないので代用)
            paramClassList = paramClassList.GroupBy(x => x.LongPlanId).Select(x => x.First()).ToList();
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormBudgetOutput.ControlId.LongPlanId, this.pageInfoList);
            // 画面にセット
            SetSearchResultsByDataClass<ComDao.LnLongPlanEntity>(pageInfo, paramClassList, paramClassList.Count);

            //********************************************************************
            //*** 一覧画面のスケジュール表示条件を予算出力画面の隠し項目に設定 ***
            //********************************************************************
            // 一覧画面のスケジュール表示条件を取得
            var scheduleCond = GetFormDataByCtrlId<Dao.BudgetOutputScheduleCondition>(ConductInfo.FormList.ControlId.ScheduleCondition, false);
            // スケジュール表示期間 = スケジュール表示期間From + "|" + スケジュール表示期間To
            scheduleCond.ScheduleYear = scheduleCond.ScheduleYearFrom.ToString() + ComUtil.FromToDelimiter.ToString() + scheduleCond.ScheduleYearTo.ToString();
            // リストに格納
            IList<Dao.BudgetOutputScheduleCondition> scheduleCondList = new List<Dao.BudgetOutputScheduleCondition>();
            scheduleCondList.Add(scheduleCond);
            // ページ情報取得
            var pageInfoCondition = GetPageInfo(ConductInfo.FormBudgetOutput.ControlId.ScheduleCondition, this.pageInfoList);
            // 画面にセット
            SetFormByDataClass<Dao.BudgetOutputScheduleCondition>(ConductInfo.FormBudgetOutput.ControlId.ScheduleCondition, scheduleCondList);
        }
    }
}
