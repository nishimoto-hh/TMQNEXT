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
using Dao = BusinessLogic_MC0001.BusinessLogicDataClass_MC0001;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
using ScheduleStatus = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleStatus;

namespace BusinessLogic_MC0001
{
    /// <summary>
    /// 機器台帳(長期計画タブ)
    /// </summary>
    public partial class BusinessLogic_MC0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlNameLongPlan
        {
            /// <summary>SQL名：長期計画一覧取得</summary>
            public const string GetLongPlanList = "GetLongPlanList";
            /// <summary>SQL名：長期計画一覧スケジュール取得</summary>
            public const string GetLongPlanScheduleDetail = "GetLongPlanScheduleDetail";
        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlIdLongPlan
        {
            /// <summary>
            /// 長期計画 スケジューリング表示条件
            /// </summary>
            public const string LongPlanScheduleConditionList190 = "BODY_190_00_LST_1";
            /// <summary>
            /// 長期計画 長期計画一覧
            /// </summary>
            public const string LongPlanList210 = "BODY_210_00_LST_1";
            /// <summary>
            /// 長期計画 スケジューリング表示条件 非表示
            /// </summary>
            public const string DetailScheduleConditionList006 = "BODY_006_00_LST_1";
        }

        /// <summary>
        /// 長期計画検索
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool searchLongPlan(long machineId, ref bool scheduleCondError)
        {

            //長期計画一覧初期検索
            setDataList<Dao.longPlanListResult>(machineId, SqlNameLongPlan.GetLongPlanList, TargetCtrlIdLongPlan.LongPlanList210);

            // スケジュール表示条件セット
            //setScheduleCondition(TargetCtrlIdLongPlan.LongPlanScheduleConditionList190);

            // スケジュール情報のセット
            setLongPlanSchedule(TargetCtrlIdLongPlan.LongPlanList210, machineId, ref scheduleCondError);
            return true;
        }

        /// <summary>
        /// 保全情報一覧に紐づけるスケジュールリストを取得する処理
        /// </summary>
        private void setLongPlanSchedule(string listCtrlId, long machineId, ref bool scheduleCondError)
        {
            // 対象機器の工場ID取得
            dynamic whereParam = new { MachineId = machineId };
            string sql;
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
            //if (results == null || results.Count == 0)
            //{
            //    return false;
            //}
            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, typeLst, this.db, this.LanguageId);

            //int factoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
            int factoryId = (int)results[0].FactoryId;

            // 年度開始月の取得
            int monthStartNendo = getYearStartMonth(factoryId);

            // 画面の条件を取得
            // スケジュール表示条件セット
            Dao.ScheduleCondInfo condInfo = setScheduleCondition(TargetCtrlIdLongPlan.LongPlanScheduleConditionList190, TargetCtrlIdLongPlan.DetailScheduleConditionList006, ref scheduleCondError, monthStartNendo);
            if (scheduleCondError)
            {
                return;
            }

            //// 非表示一覧にセット
            //setScheduleCondition(TargetCtrlIdLongPlan.DetailScheduleConditionList006);
            var scheduleCond = new TMQDao.ScheduleList.Condition();
            if (condInfo.ScheduleCondType == condInfo.ScheduleCondMonthStructureId)
            {
                // 月度
                scheduleCond.ScheduleStartYear = (int)condInfo.ScheduleCondYear;
                scheduleCond.ScheduleUnit = (int)TMQConsts.MsStructure.StructureId.ScheduleDisplayUnit.Month;
                scheduleCond.ExtensionData = TMQConsts.MsStructure.StructureId.ScheduleDisplayUnit.Month.ToString();
            }
            else
            {
                string[] span = condInfo.scheduleCondSpan.Split('|');
                scheduleCond.ScheduleYearFrom = int.Parse(span[0]);
                scheduleCond.ScheduleYearTo = int.Parse(span[1]);
                scheduleCond.ScheduleStartYear = int.Parse(span[0]);
                scheduleCond.ScheduleUnit = (int)TMQConsts.MsStructure.StructureId.ScheduleDisplayUnit.Year;
                scheduleCond.ExtensionData = TMQConsts.MsStructure.StructureId.ScheduleDisplayUnit.Year.ToString();
            }
            TMQDao.ScheduleList.Condition co = new();
            Dao.Schedule.SearchConditionMachineId cond = new(scheduleCond, machineId, monthStartNendo, this.LanguageId);
            cond.FactoryIdList = TMQUtil.GetFactoryIdList();
            cond.FactoryIdList.Add(factoryId);
            // 個別実装用データへスケジュールのレイアウトデータ(scheduleLayout)をセット
            bool isMovable = false;
            TMQUtil.ScheduleListUtil.SetLayout(ref this.IndividualDictionary, cond, isMovable, getNendoText(), monthStartNendo, 3);

            // 画面表示データの取得
            List<TMQDao.ScheduleList.Display> scheduleDisplayList = getScheduleDisplayList<TMQUtil.ScheduleListConverterNoRank>(cond);

            // 画面設定用データに変換
            Dictionary<string, Dictionary<string, string>> setScheduleData = TMQUtil.ScheduleListUtil.ConvertDictionaryAddData(scheduleDisplayList, cond);

            // 画面に設定
            SetScheduleDataToResult(setScheduleData, listCtrlId);

            // 画面表示データの取得 SQLを実行し、実行結果を変換 変換するクラスが上位ランクの有無が異なるので分岐
            List<TMQDao.ScheduleList.Display> getScheduleDisplayList<T>(Dao.Schedule.SearchConditionMachineId scheduleCondition)
                where T : TMQUtil.ScheduleListConverter, new()
            {
                string sql = string.Empty;
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameLongPlan.GetLongPlanScheduleDetail, out sql);
                IList<TMQDao.ScheduleList.Get> scheduleList = db.GetListByDataClass<TMQDao.ScheduleList.Get>(sql, scheduleCondition);
                // 画面表示用に変換
                T listSchedule = new();
                // リンク有無
                bool isLink = true; // リンクは詳細画面のみ
                if (cond.IsYear())
                {
                    // リンクは月単位のみなので、年単位の場合リンクしない
                    isLink = false;
                }
                var scheduleDisplayList = listSchedule.Execute(scheduleList, cond, monthStartNendo, isLink, this.db, getScheduleLinkInfo());
                return scheduleDisplayList;
            }
        }

    }
}
