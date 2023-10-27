using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using static CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId;
using static CommonTMQUtil.CommonTMQConstants.MsStructure.StructureLayerNo;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MA0001.BusinessLogicDataClass_MA0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

namespace BusinessLogic_MA0001
{
    /// <summary>
    /// 保全活動（一覧）
    /// </summary>
    public partial class BusinessLogic_MA0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // 件名別長期計画・機器別長期計画の白丸「○」リンクから遷移してきた場合は値をグローバル変数に格納する
            setValFromScheduleLink(out bool isFromScheduleLink);

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.HiddenItemId, this.pageInfoList);
            Dao.userRole role = new Dao.userRole();
            // ユーザ役割の設定（新規登録ボタンの表示制御に使用）
            setUserRole<Dao.userRole>(role);
            // ユーザ役割の設定
            SetSearchResultsByDataClass<Dao.userRole>(pageInfo, new List<Dao.userRole>() { role }, 1);
            if (isFromScheduleLink)
            {
                // 「○」リンクで遷移した場合は新規登録画面を表示するので、一覧の検索は不要。処理終了する
                return true;
            }

            // ページ情報取得
            pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.SearchResult, this.pageInfoList);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.List.GetList, out string baseSql);
            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.List.GetList, out string withSql);

            //保全実績評価から遷移してきた場合、職種を再設定する
            setJobCondition();

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
            {
                return false;
            }
            //SQLパラメータに言語ID設定
            whereParam.LanguageId = this.LanguageId;
            // SQL、WHERE句、WITH句より件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);
            //// 総件数を取得
            //int cnt = db.GetCount(executeSql, whereParam);
            //// 総件数のチェック
            //if (!CheckSearchTotalCount(cnt, pageInfo))
            //{
            //    //ユーザ役割の設定を行うので、検索結果0件の扱いにならないためメッセージはここで設定しておく
            //    this.Status = CommonProcReturn.ProcStatus.Warning;
            //    // 「該当データがありません。」
            //    this.MsgId = GetResMessage("941060001");
            //    SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, null, cnt, isDetailConditionApplied);
            //    return false;
            //}

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY");
            selectSql.AppendLine("occurrence_date desc");
            selectSql.AppendLine(",summary_id desc");
            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                //ユーザ役割の設定を行うので、検索結果0件の扱いにならないためメッセージはここで設定しておく
                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「該当データがありません。」
                this.MsgId = GetResMessage("941060001");
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, null, 0, isDetailConditionApplied);
                return false;
            }

            // 地区～設備、職種～機種小分類、原因性格1、原因性格2を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job, StructureType.FailureCause }, this.db, this.LanguageId);

            //一覧表示は件名単位にするため、集約する（SQLで集約すると詳細検索ができない項目が発生する為、ここで行う）
            IList<Dao.searchResult> displayList = createDisplayList(results);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, displayList, results.Count, isDetailConditionApplied))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;

            //職種を設定
            void setJobCondition()
            {
                //職種ID
                object ids = GetGlobalData(GlobalKey.MA0001JobId, true);
                if (ids != null)
                {
                    List<int> jobIdList = new List<int>();
                    var keyName = STRUCTURE_CONSTANTS.CONDITION_KEY.Job;
                    var dic = this.searchConditionDictionary.Where(x => x.ContainsKey(keyName)).FirstOrDefault();
                    if (dic != null && dic.ContainsKey(keyName))
                    {
                        if (ids.ToString() == jobAll || ids.ToString() == "")
                        {
                            //職種の指定なし
                            this.searchConditionDictionary.Where(x => x.ContainsKey(STRUCTURE_CONSTANTS.CONDITION_KEY.Job)).FirstOrDefault()[STRUCTURE_CONSTANTS.CONDITION_KEY.Job] = new List<int>();
                        }
                        else
                        {
                            List<int> list = ids.ToString().Split("|").ToList().ConvertAll(x => Convert.ToInt32(x));
                            this.searchConditionDictionary.Where(x => x.ContainsKey(STRUCTURE_CONSTANTS.CONDITION_KEY.Job)).FirstOrDefault()[STRUCTURE_CONSTANTS.CONDITION_KEY.Job] = list;
                        }
                    }
                }
            }

            // 保全スケジュール詳細IDをグローバル変数に格納
            void setValFromScheduleLink(out bool isFromScheduleLink)
            {
                // ○でリンクした場合はTrueを設定する
                isFromScheduleLink = false;
                // パラメータに遷移元の情報が存在しない場合は何もしない
                var dic = this.searchConditionDictionary.Where(x => x.ContainsKey("CTRLID")).FirstOrDefault();
                if (dic == null)
                {
                    return;
                }

                // ページ情報取得
                var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.SearchResult, this.pageInfoList);
                //選択行のデータを取得
                Dao.searchCondition conditionObj = new Dao.searchCondition();
                SetSearchConditionByDataClass(this.searchConditionDictionary, ConductInfo.FormList.ControlId.SearchResult, conditionObj, pageInfo);

                // タブ番号が数値に変換できない場合は何もしない
                if (!int.TryParse(conditionObj.TabNo.ToString(), out int tabNo))
                {
                    return;
                }

                // タブ番号が「-1」でない場合は何もしない
                if (tabNo != ConductInfo.FormList.ParamFromLongPlan.TabNo)
                {
                    return;
                }

                isFromScheduleLink = true;
                // グルーバル変数に値を格納(遷移時は「SummaryId」に保全スケジュール詳細IDが入っている)
                SetGlobalData(ConductInfo.FormList.ParamFromLongPlan.GlobalKey, conditionObj.SummaryId);
            }
        }

        /// <summary>
        /// 一覧に表示するデータを件名単位に集約する
        /// </summary>
        /// <param name="results">検索結果</param>
        /// <returns>集約したデータ</returns>
        private IList<Dao.searchResult> createDisplayList(IList<Dao.searchResult> results)
        {
            IList<Dao.searchResult> displayList = new List<Dao.searchResult>();
            List<long> summaryIdList = results.Select(x => x.SummaryId).Distinct().ToList();
            foreach (long summaryId in summaryIdList)
            {
                //集約対象の行を取得
                IList<Dao.searchResult> targetList = results.Where(x => x.SummaryId == summaryId).ToList();
                if (targetList.Count() == 1)
                {
                    if (targetList[0].FollowPlanDate != null)
                    {
                        //フォロー予定年月 表示用項目に値設定
                        DateTime date = targetList[0].FollowPlanDate ?? DateTime.Now;
                        targetList[0].FollowPlanDateDisp = date.ToString(GetResMessage(ComRes.ID.ID150000002));
                    }
                    displayList.Add(targetList[0]);
                    continue;
                }
                //機器番号
                List<string> machineNoList = targetList.Where(x => x.MachineNo != null).Select(x => x.MachineNo).Distinct().ToList();
                string machineNo = string.Join(",", machineNoList);
                //機器名称
                List<string> machineNameList = targetList.Where(x => x.MachineName != null).Select(x => x.MachineName).Distinct().ToList();
                string machineName = string.Join(",", machineNameList);
                //職種
                List<string> jobNameList = targetList.Where(x => x.JobName != null).Select(x => x.JobName).Distinct().ToList();
                string jobName = string.Join(",", jobNameList);
                //機種大分類
                List<string> largeClassficationNameList = targetList.Where(x => x.LargeClassficationName != null).Select(x => x.LargeClassficationName).Distinct().ToList();
                string largeClassficationName = string.Join(",", largeClassficationNameList);
                //機種中分類
                List<string> middleClassficationNameList = targetList.Where(x => x.MiddleClassficationName != null).Select(x => x.MiddleClassficationName).Distinct().ToList();
                string middleClassficationName = string.Join(",", middleClassficationNameList);
                //機種小分類
                List<string> smallClassficationNameList = targetList.Where(x => x.SmallClassficationName != null).Select(x => x.SmallClassficationName).Distinct().ToList();
                string smallClassficationName = string.Join(",", smallClassficationNameList);
                //保全部位
                List<string> maintenanceSiteList = targetList.Where(x => x.MaintenanceSiteName != null).Select(x => x.MaintenanceSiteName).Distinct().ToList();
                string maintenanceSite = string.Join(",", maintenanceSiteList);
                //保全内容
                List<string> maintenanceContentList = targetList.Where(x => x.MaintenanceContentName != null).Select(x => x.MaintenanceContentName).Distinct().ToList();
                string maintenanceContent = string.Join(",", maintenanceContentList);
                //フォロー有無(trueのものが1件以上ある場合、true)
                bool followFlg = targetList.Where(x => x.FollowFlg != null && (x.FollowFlg ?? false)).Select(x => x.FollowFlg).Count() > 0;
                //フォロー予定年月
                List<string> followPlanDateList = targetList.Where(x => x.FollowPlanDate != null).Select(x => (x.FollowPlanDate ?? DateTime.Now).ToString(GetResMessage(ComRes.ID.ID150000002))).Distinct().ToList();
                string followPlanDate = string.Join(",", followPlanDateList);
                //フォロー内容
                List<string> followContentList = targetList.Where(x => x.FollowContent != null).Select(x => x.FollowContent).Distinct().ToList();
                string followContent = string.Join(",", followContentList);

                Dao.searchResult info = targetList[0];
                info.MachineNo = machineNo;
                info.MachineName = machineName;
                info.JobName = jobName;
                info.LargeClassficationName = largeClassficationName;
                info.MiddleClassficationName = middleClassficationName;
                info.SmallClassficationName = smallClassficationName;
                info.MaintenanceSiteName = maintenanceSite;
                info.MaintenanceContentName = maintenanceContent;
                info.FollowFlg = followFlg;
                info.FollowPlanDateDisp = followPlanDate;
                info.FollowContent = followContent;
                displayList.Add(info);
            }
            return displayList;
        }

        /// <summary>
        /// ユーザ役割を取得し、設定
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="result">ユーザ役割を設定するデータ</param>
        private void setUserRole<T>(T result)
        {
            // 設定するデータクラスの情報を取得
            PropertyInfo[] targetProps = typeof(T).GetProperties();
            //製造権限有無
            PropertyInfo propManufacturing = targetProps.FirstOrDefault(x => x.Name.ToUpper().Equals("Manufacturing".ToUpper()));
            //保全権限有無
            PropertyInfo propMaintenance = targetProps.FirstOrDefault(x => x.Name.ToUpper().Equals("Maintenance".ToUpper()));
            if (propManufacturing != null && propMaintenance != null)
            {
                // ユーザ役割の取得
                List<ComDao.MsItemExtensionEntity> results = TMQUtil.SqlExecuteClass.SelectList<ComDao.MsItemExtensionEntity>(SqlName.List.GetUserRole, SqlName.SubDir, new ComDao.MsUserEntity.PrimaryKey(Convert.ToInt32(this.UserId)), this.db);
                if (results == null)
                {
                    ComUtil.SetPropertyValue<T>(propManufacturing, result, false);
                    ComUtil.SetPropertyValue<T>(propMaintenance, result, false);
                    return;
                }

                //製造権限が含まれる場合、true
                ComUtil.SetPropertyValue<T>(propManufacturing, result, results.Exists(x => x.ExtensionData == UserRole.Manufacturing));
                //保全権限が含まれる場合、true
                ComUtil.SetPropertyValue<T>(propMaintenance, result, results.Exists(x => x.ExtensionData == UserRole.Maintenance));
            }

        }

        /// <summary>
        /// 拡張データより構成IDを取得する処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        private int GetStructureIdList()
        {
            //構成グループID
            int groupId = Convert.ToInt32(GetGlobalData(GlobalKey.MA0001GroupId, true));
            //拡張データ 「|」区切り(「|」と「||」区切りはMQ分類のときのみ 10|20||2|3)
            string extensionData = GetGlobalData(GlobalKey.MA0001ExtensionData, true).ToString();
            List<string> extensionDataList = extensionData.Split("||").ToList();
            List<string> condition1 = extensionDataList[0].Split("|").ToList();
            List<string> condition2 = new();
            if (extensionDataList.Count() > 1)
            {
                condition2 = extensionDataList[1].Split("|").ToList();
            }

            // 構成グループ、拡張データより構成IDを取得
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = groupId;
            //連番
            param.Seq = getParamSeq(groupId, condition2.Count == 0);

            //構成ID取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);

            List<int> structureIdList = new();
            if (groupId == (int)Const.MsStructure.GroupId.MqClass && condition2.Count() > 0)
            {
                //MQ分類　「||」区切りで複数拡張データが指定された場合
                //拡張データ2と4のデータが一致する構成IDを取得
                structureIdList = list.Where(x => x.Seq == MqClassBudgetPersonalityDivision.Seq && condition1.IndexOf(x.ExData) >= 0).Select(x => x.StructureId).Distinct().ToList();
                structureIdList = list.Where(x => x.Seq == MqClassDiscriminationDivision.Seq && condition2.IndexOf(x.ExData) >= 0 && structureIdList.IndexOf(x.StructureId) >= 0).Select(x => x.StructureId).Distinct().ToList();
            }
            else if (groupId == (int)Const.MsStructure.GroupId.Job && condition1.IndexOf(((int)JobCode.Machine).ToString()) < 0 && condition1.IndexOf(((int)JobCode.Electricity).ToString()) < 0 && condition1.IndexOf(((int)JobCode.Instrumentation).ToString()) < 0)
            {
                //職種が機械、電気、計装以外(その他)の場合
                structureIdList = list.Where(x => x.StructureLayerNo == (int)Job.Job
                    && (x.ExData == null || (x.ExData != ((int)JobCode.Machine).ToString() && x.ExData != ((int)JobCode.Electricity).ToString() && x.ExData != ((int)JobCode.Instrumentation).ToString()))).Select(x => x.StructureId).Distinct().ToList();
            }
            else
            {
                //拡張データと一致する構成IDを取得
                structureIdList = list.Where(x => condition1.IndexOf(x.ExData) >= 0).Select(x => x.StructureId).Distinct().ToList();
            }
            //構成IDをパイプ区切り文字列にする
            SetGlobalData(GlobalKey.MA0001StructureId, string.Join("|", structureIdList));

            return ComConsts.RETURN_RESULT.OK;

            //構成グループIDから連番を設定
            int? getParamSeq(int groupId, bool flg)
            {
                switch (groupId)
                {
                    case (int)Const.MsStructure.GroupId.Job: //職種
                        //職種が機械、電気、計装以外(その他)の場合、拡張データを設定していない場合を考慮し連番を指定しない
                        return null;
                    case (int)Const.MsStructure.GroupId.StopSystem: //系停止
                        return StopSystemDivision.Seq;
                    case (int)Const.MsStructure.GroupId.Sudden: //突発区分
                        return SuddenDivision.Seq;
                    case (int)Const.MsStructure.GroupId.MqClass: //MQ分類
                        if (flg)
                        {
                            //拡張データ2のみ使用
                            return MqClassBudgetPersonalityDivision.Seq;
                        }
                        else
                        {
                            //拡張データ2,4を使用
                            return null;
                        }
                }
                return null;
            }
        }

    }
}
