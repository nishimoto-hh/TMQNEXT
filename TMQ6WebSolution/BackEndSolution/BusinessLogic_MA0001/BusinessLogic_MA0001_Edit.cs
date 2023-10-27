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
    /// 保全活動（登録・編集）
    /// </summary>
    public partial class BusinessLogic_MA0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 新規登録画面　初期表示
        /// </summary>
        /// <param name="ctrlId">コントロールID比較用クラス</param>
        /// <returns>正常なら0以上、異常なら-1</returns>
        private int initNew(CompareCtrlIdClass ctrlId)
        {
            Dao.detailSummaryInfo result = new Dao.detailSummaryInfo();
            //新規登録時、保全活動件名IDは-1
            result.SummaryId = newSummaryId;
            //活動区分ID（点検or故障）
            if (ctrlId.IsStartId(ConductInfo.FormList.Button.NewFailure))
            {
                //故障情報登録
                result.ActivityDivision = MaintenanceDivision.Failure;
            }
            else if (ctrlId.IsStartId(ConductInfo.FormList.Button.NewInspection) || ctrlId.IsNew())
            {
                //点検情報登録、新規登録
                result.ActivityDivision = MaintenanceDivision.Inspection;
            }

            if (ctrlId.IsStartId(ConductInfo.FormDetail.Button.Follow))
            {
                //フォロー計画の場合、計画元の保全活動件名IDを設定
                getFollowCondition(result);
            }

            // 長期計画の白丸「○」リンクから遷移してきた場合はTrue、通常起動はFalse
            bool isFromLongPlan = this.IndividualDictionary.ContainsKey(ConductInfo.FormList.ParamFromLongPlan.GlobalKey);
            Dao.searchResultFromLongPlan resultFromLongPlan = new();
            if (isFromLongPlan)
            {
                // 初期表示項目取得
                if (!getDataFromLongPlan(out Dao.searchResultFromLongPlan outResultFromLongPlan))
                {
                    return ComConsts.RETURN_RESULT.NG;
                }
                resultFromLongPlan = outResultFromLongPlan;
            }

            //MQ分類：設備工事、撤去工事の構成IDをカンマ区切りで設定
            result.MqNotRequiredStructureId = setMqNotRequiredStructureId();

            //場所階層、職種の取得
            Dao.detailSummaryInfo structureLayer = new();
            structureLayer.LocationStructureId = getTreeValue(true);
            structureLayer.JobStructureId = getTreeValue(false);
            // 取得した結果に対して、場所階層、職種の情報を設定する
            IList<Dao.detailSummaryInfo> structureLayerList = new List<Dao.detailSummaryInfo> { structureLayer };
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.detailSummaryInfo>(ref structureLayerList, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormRegist.ControlId.StructureId, this.pageInfoList);

            // 場所階層、職種の設定(長期計画の白丸「○」リンクから遷移してこなかった場合)
            if (!isFromLongPlan)
            {
                SetSearchResultsByDataClass<Dao.detailSummaryInfo>(pageInfo, structureLayerList, 1);
            }

            // 保全履歴個別表示制御に使用するフラグを設定
            string historyIndividualFlg = getItemExData(HistoryIndividualDivision.Seq, HistoryIndividualDivision.DataType, structureLayer.FactoryId);
            result.HistoryIndividualFlg = historyIndividualFlg == null ? IndividualDivision.Hide : historyIndividualFlg;

            // 故障分析情報個別表示制御に使用するフラグを設定
            string failureIndividualFlg = getItemExData(FailureIndividualDivision.Seq, FailureIndividualDivision.DataType, structureLayer.FactoryId);
            result.FailureIndividualFlg = failureIndividualFlg == null ? IndividualDivision.Hide : failureIndividualFlg;

            // ユーザ役割の設定
            setUserRole<Dao.detailSummaryInfo>(result);

            // ページ情報取得
            pageInfo = GetPageInfo(ConductInfo.FormRegist.ControlId.DetailInfoIds[0], this.pageInfoList);

            // 長期計画の白丸「○」リンクから遷移してきた際の初期値を設定
            if (isFromLongPlan)
            {
                // 保全スケジュール詳細ID
                result.MaintainanceScheduleDetailId = resultFromLongPlan.MaintainanceScheduleDetailId;
            }

            // 保全活動区分、MQ分類(非表示)、ユーザ役割の設定
            SetSearchResultsByDataClass<Dao.detailSummaryInfo>(pageInfo, new List<Dao.detailSummaryInfo> { result }, 1);

            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = (int)Const.MsStructure.GroupId.StopSystem;
            //連番
            param.Seq = StopSystemDivision.Seq;
            //データタイプ
            param.DataType = StopSystemDivision.DataType;
            //拡張データ
            param.ExData = StopSystemDivision.ExData;

            //系停止「なし」の構成ID取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> stopSystemList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            Dao.StopSystemInfo stopSystemInfo = new Dao.StopSystemInfo();
            //系停止
            stopSystemInfo.StopSystemStructureId = stopSystemList[0].StructureId;
            //活動区分ID（点検or故障 MQ分類コンボのアイテム絞り込みに使用）
            stopSystemInfo.ActivityDivision = result.ActivityDivision;
            // ページ情報取得
            pageInfo = GetPageInfo(ConductInfo.FormRegist.ControlId.WorkInfoId, this.pageInfoList);

            // 長期計画の白丸「○」リンクから遷移してきた際の初期値を設定
            if (isFromLongPlan)
            {
                stopSystemInfo.BudgetPersonalityStructureId = resultFromLongPlan.BudgetPersonalityStructureId; // 予算性格区分
                stopSystemInfo.BudgetManagementStructureId = resultFromLongPlan.BudgetManagementStructureId;   // 予算性格区分
            }

            // 系停止の設定
            SetSearchResultsByDataClass<Dao.StopSystemInfo>(pageInfo, new List<Dao.StopSystemInfo> { stopSystemInfo }, 1);

            // 長期計画の白丸「○」リンクから遷移してきた際の初期値を設定
            if (!setDataFromLongPlan(isFromLongPlan, resultFromLongPlan))
            {
                return ComConsts.RETURN_RESULT.NG;
                this.Status = CommonProcReturn.ProcStatus.Error;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;

            return ComConsts.RETURN_RESULT.OK;

            //詳細画面に表示している件名の保全活動件名IDを新規画面のフォロー計画キーIDに設定する
            void getFollowCondition(Dao.detailSummaryInfo result)
            {
                Dao.searchCondition condition = new();
                // ページ情報取得
                var pageInfo = GetPageInfo(ConductInfo.FormDetail.ControlId.DetailInfoIds[0], this.pageInfoList);
                //詳細画面のデータを取得
                SetSearchConditionByDataClass(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.DetailInfoIds[0], condition, pageInfo);

                //フォロー計画キーIDに保全活動件名IDに設定
                result.FollowPlanKeyId = condition.SummaryId;
                //活動区分ID（フォロー計画元の活動区分IDを設定）
                result.ActivityDivision = condition.ActivityDivision;
            }

            // ツリーの階層IDの値が単一の場合その値を返す処理
            int? getTreeValue(bool isLocation)
            {
                var list = isLocation ? GetLocationTreeValues() : GetJobTreeValues();
                if (list != null && list.Count == 1)
                {
                    // 値が単一でもその下に紐づく階層が複数ある場合は初期表示しないので判定
                    bool result = TMQUtil.GetButtomValueFromTree(list[0], this.db, this.LanguageId, out int buttomId);
                    return result ? buttomId : null;
                }
                return null;
            }
        }

        /// <summary>
        /// 件名別長期計画・機器別長期計画の白丸「○」リンクから遷移してきた際の初期値を取得する
        /// </summary>
        /// <param name="result">検索結果</param>
        /// <returns>エラーの場合False</returns>
        private bool getDataFromLongPlan(out Dao.searchResultFromLongPlan result)
        {
            result = new();

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetScheduleFromLongPlan, out string sql);

            // 検索条件を設定
            Dao.searchResultFromLongPlan condition = new();
            // 保全スケジュール詳細ID
            condition.MaintainanceScheduleDetailId = long.Parse(GetGlobalData(ConductInfo.FormList.ParamFromLongPlan.GlobalKey).ToString());

            // SQL実行
            IList<Dao.searchResultFromLongPlan> results = db.GetListByDataClass<Dao.searchResultFromLongPlan>(sql, condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            results[0].MaintainanceScheduleDetailId = condition.MaintainanceScheduleDetailId; // 保全スケジュール詳細ID
            results[0].IssueDate = DateTime.Now;                                              // 発行日

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResultFromLongPlan>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

            result = results[0];
            return true;
        }

        /// <summary>
        /// 件名別長期計画・機器別長期計画の白丸「○」リンクから遷移してきた際の初期値を設定
        /// </summary>
        /// <param name="isFromLongPlan">長期計画の白丸「○」リンクから遷移してきた場合はTrue</param>
        /// <param name="result">検索結果</param>
        /// <returns>エラーの場合False</returns>
        private bool setDataFromLongPlan(bool isFromLongPlan, Dao.searchResultFromLongPlan result)
        {
            if(!isFromLongPlan)
            {
                return true;
            }

            // 取得している値を一覧に設定
            foreach (string ctrlId in ConductInfo.FormRegist.ControlId.MakeMaintenanceFromLongPlan)
            {
                if (!SetFormByDataClass(ctrlId, new List<Dao.searchResultFromLongPlan> { result }))
                {
                    return false;
                }
            }

            // 対象機器一覧検索
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetMachineListFromLongPlan, out string sql);

            // SQL実行
            IList<Dao.detailMachine> resultMachineList = db.GetListByDataClass<Dao.detailMachine>(sql, result);
            if(resultMachineList == null || resultMachineList.Count == 0)
            {
                return false;
            }

            // 職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.detailMachine>(ref resultMachineList, new List<StructureType> { StructureType.Job }, this.db, this.LanguageId);

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormRegist.ControlId.MachineList, this.pageInfoList);

            // 検索結果の設定
            if(!SetSearchResultsByDataClass<Dao.detailMachine>(pageInfo, resultMachineList, resultMachineList.Count))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// MQ分類：設備工事、撤去工事の構成IDをカンマ区切り文字列にする
        /// </summary>
        /// <returns>カンマ区切り文字列</returns>
        private string setMqNotRequiredStructureId()
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = (int)Const.MsStructure.GroupId.MqClass;
            //連番
            param.Seq = MqClassDivision.Seq;
            //データタイプ
            param.DataType = MqClassDivision.DataType;
            //拡張データ
            param.ExData = MqClassDivision.ExData;
            //MQ分類：設備工事、撤去工事の構成ID
            List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            List<int> structureList = list.Select(x => x.StructureId).ToList();
            //カンマ区切りで連結
            return string.Join(",", structureList);
        }

        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdit()
        {
            //ユーザ役割取得
            //製造権限（true：権限あり）
            bool manufacturingFlg = Convert.ToBoolean(getValueByKeyName(ConductInfo.FormRegist.ControlId.DetailInfoIds[0], "manufacturing"));
            //保全権限（true：権限あり）
            bool maintenanceFlg = Convert.ToBoolean(getValueByKeyName(ConductInfo.FormRegist.ControlId.DetailInfoIds[0], "maintenance"));
            //保全履歴個別工場表示フラグ
            string historyIndividualFlg = getValueByKeyName(ConductInfo.FormRegist.ControlId.DetailInfoIds[0], "history_individual_flg");

            // 排他チェック
            if (isErrorExclusive(manufacturingFlg, maintenanceFlg, historyIndividualFlg))
            {
                return false;
            }

            // 入力チェック
            if (isErrorRegist())
            {
                return false;
            }

            // システム日時
            DateTime now = DateTime.Now;
            // 登録する内容を取得
            //保全活動件名
            List<short> grpNoList = new List<short>() { ConductInfo.FormRegist.GroupNo.SummaryInfo, ConductInfo.FormRegist.GroupNo.WorkPersonalityInfo };
            //表示している履歴タブの情報をセットする
            if (historyIndividualFlg != IndividualDivision.Show)
            {
                //保全履歴タブ
                grpNoList.Add(ConductInfo.FormRegist.GroupNo.HistoryInfo);
            }
            else
            {
                //保全履歴(個別工場)タブ
                grpNoList.Add(ConductInfo.FormRegist.GroupNo.HistoryIndividualInfo);
            }
            Dao.detailSummaryInfo registSummaryInfo = getRegistInfo<Dao.detailSummaryInfo>(grpNoList, now);

            //最下層の構成IDを取得して機能場所階層ID、職種機種階層IDにセットする
            IList<Dao.detailSummaryInfo> results = new List<Dao.detailSummaryInfo>();
            results.Add(registSummaryInfo);
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.detailSummaryInfo>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job });

            //新規登録or更新
            bool isRegist = registSummaryInfo.SummaryId == newSummaryId;

            //保全依頼
            ComDao.MaRequestEntity registRequestInfo = new ComDao.MaRequestEntity();
            if (manufacturingFlg || isRegist)
            {
                //製造権限がない場合は依頼情報の更新は行わない（新規登録の場合はレコードは作成しておく）
                grpNoList = new List<short>() { ConductInfo.FormRegist.GroupNo.RequestInfo, ConductInfo.FormRegist.GroupNo.RequestApplicationInfo };
                registRequestInfo = getRegistInfo<ComDao.MaRequestEntity>(grpNoList, now);
                if (isRegist)
                {
                    //依頼番号を取得
                    string requestNo = getRequestNo(registSummaryInfo, now);
                    if (requestNo == null)
                    {
                        return false;
                    }
                    registRequestInfo.RequestNo = requestNo;
                }
            }

            //トランザクションを分けるため、依頼番号取得後に画面の内容を登録する
            // 保全活動件名登録
            long val = newSummaryId;
            bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out val, isRegist ? SqlName.Regist.InsertSummary : SqlName.Regist.UpdateSummary, SqlName.SubDir, registSummaryInfo, this.db);
            if (!returnFlag)
            {
                return false;
            }
            //保全活動件名ID
            long summaryId = val;

            //点検・故障によって登録先を変更
            switch (registSummaryInfo.ActivityDivision)
            {
                case MaintenanceDivision.Inspection:
                    //保全履歴、保全履歴機器、保全履歴機器部位、保全履歴点検内容
                    if (!registInspectionData(summaryId, isRegist, maintenanceFlg, registSummaryInfo, now))
                    {
                        return false;
                    }
                    break;
                case MaintenanceDivision.Failure:
                    //保全履歴、保全履歴故障情報
                    if (!registFailureData(summaryId, isRegist, maintenanceFlg, registSummaryInfo, now))
                    {
                        return false;
                    }
                    break;
            }

            // 保全依頼登録
            if (manufacturingFlg || isRegist)
            {
                //製造権限がない場合は依頼情報の更新は行わない（新規登録の場合はレコードは作成しておく）
                if (!registRequest())
                {
                    return false;
                }
            }

            if (!maintenanceFlg && !isRegist)
            {
                //保全権限がない場合は依頼情報以外のタブの更新は行わない（新規登録の場合はレコードは作成しておく）
                return true;
            }

            //保全計画
            if (!registPlan())
            {
                return false;
            }

            //保全スケジュール詳細更新
            if (!updateSchedule())
            {
                return false;
            }

            //登録した保全活動件名IDを設定（詳細画面を表示するための情報設定）
            var pageInfo = GetPageInfo(ConductInfo.FormRegist.ControlId.DetailInfoIds[0], this.pageInfoList);
            ComDao.MaSummaryEntity info = new ComDao.MaSummaryEntity();
            info.SummaryId = summaryId;
            SetSearchResultsByDataClass<ComDao.MaSummaryEntity>(pageInfo, new List<ComDao.MaSummaryEntity> { info }, 1);

            return true;

            //依頼情報の登録
            bool registRequest()
            {
                registRequestInfo.SummaryId = summaryId;
                bool resultRequest = TMQUtil.SqlExecuteClass.Regist(isRegist ? SqlName.Regist.InsertRequest : SqlName.Regist.UpdateRequest, SqlName.SubDir, registRequestInfo, this.db);
                if (!resultRequest)
                {
                    return false;
                }
                return true;
            }

            //計画情報の登録
            bool registPlan()
            {
                List<short> grpNoList = new List<short>() { ConductInfo.FormRegist.GroupNo.PlanInfo };
                ComDao.MaPlanEntity registPlanInfo = getRegistInfo<ComDao.MaPlanEntity>(grpNoList, now);
                registPlanInfo.SummaryId = summaryId;
                bool resultPlan = TMQUtil.SqlExecuteClass.Regist(isRegist ? SqlName.Regist.InsertPlan : SqlName.Regist.UpdatePlan, SqlName.SubDir, registPlanInfo, this.db);
                if (!resultPlan)
                {
                    return false;
                }
                return true;
            }

            //保全スケジュール詳細更新
            bool updateSchedule()
            {
                if (isRegist || registSummaryInfo.CompletionDate == null)
                {
                    //完了日が設定されていない場合、更新なし
                    return true;
                }
                //条件、更新値設定
                ComDao.McMaintainanceScheduleDetailEntity detail = new();
                detail.ScheduleDate = registSummaryInfo.CompletionDate;
                detail.Complition = true;
                detail.SummaryId = summaryId;

                //SQL文の取得
                string sql;
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetCountMaintainanceScheduleDetail, out sql))
                {
                    return false;
                }
                //更新対象の存在チェック
                int cnt = db.GetCount(sql, detail);
                if (cnt <= 0)
                {
                    //対象レコード無し
                    return true;
                }

                // 共通の更新日時などを設定
                bool chkUpd = int.TryParse(this.UserId, out int updatorCdNum);
                setExecuteConditionByDataClassCommon<ComDao.McMaintainanceScheduleDetailEntity>(ref detail, now, updatorCdNum, -1);
                //完了日の登録
                bool result = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateComplition, SqlName.SubDir, detail, this.db);
                if (!result)
                {
                    return false;
                }

                // SQLを取得
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetCountTargetSchedule, out sql))
                {
                    return false;
                }
                //更新対象の存在チェック
                cnt = db.GetCount(sql, detail);
                if (cnt <= 0)
                {
                    //対象レコード無し
                    return true;
                }

                //保全スケジュール詳細のスケジュール日の更新
                result = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateScheduleDate, SqlName.SubDir, detail, this.db);
                if (!result)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <param name="manufacturingFlg">製造権限がある場合true</param>
        /// <param name="maintenanceFlg">保全権限がある場合true</param>
        /// <param name="historyIndividualFlg">保全履歴個別工場表示フラグ</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive(bool manufacturingFlg, bool maintenanceFlg, string historyIndividualFlg)
        {
            string ctrlId = ConductInfo.FormRegist.ControlId.DetailInfoIds[0];
            //保全活動件名ID
            string summaryId = getValueByKeyName(ctrlId, "summary_id");
            //保全活動区分
            string maintenanceDivisionId = getValueByKeyName(ctrlId, "activity_division");
            if (summaryId == newSummaryId.ToString())
            {
                //新規登録の場合、排他チェック不要のため終了
                return false;
            }

            //保全活動件名の排他チェック
            if (!checkExclusiveSingle(ctrlId))
            {
                return true;
            }

            if (maintenanceDivisionId == MaintenanceDivision.Inspection.ToString())
            {
                //点検情報の場合

                //保全履歴機器、保全履歴機器部位、保全履歴点検内容の排他チェック
                ctrlId = ConductInfo.FormRegist.ControlId.MachineList;
                // 対象のリストを取得
                List<Dictionary<string, object>> targetList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId, true);
                foreach (Dictionary<string, object> target in targetList)
                {
                    //データクラスに変換
                    Dao.detailMachine machine = new Dao.detailMachine();
                    if (!SetExecuteConditionByDataClass<Dao.detailMachine>(target, ConductInfo.FormRegist.ControlId.MachineList, machine, DateTime.Now, this.UserId, this.UserId))
                    {
                        return true;
                    }
                    //追加行の排他チェックは行わない
                    if (machine.HistoryMachineId != null)
                    {
                        //保全履歴機器、保全履歴機器部位、保全履歴点検内容の排他チェック
                        if (!checkExclusiveSingle(ctrlId, target))
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                //故障情報の場合

                ctrlId = ConductInfo.FormRegist.ControlId.FailureInfoIds[0];
                //保全履歴故障情報の排他チェック
                if (!checkExclusiveSingle(ctrlId))
                {
                    return true;
                }
            }

            //保全依頼の排他チェック
            if (manufacturingFlg)
            {
                //製造権限がある場合
                ctrlId = ConductInfo.FormRegist.ControlId.RequestInfoIds[0];
                if (!checkExclusiveSingle(ctrlId))
                {
                    return true;
                }
            }

            if (!maintenanceFlg)
            {
                //保全権限がない場合、以降の非表示タブの排他チェックは行わない
                return false;
            }

            //保全計画の排他チェック
            ctrlId = ConductInfo.FormRegist.ControlId.PlanId;
            if (!checkExclusiveSingle(ctrlId))
            {
                return true;
            }

            //保全履歴の排他チェック
            //表示している履歴タブの情報を取得
            ctrlId = historyIndividualFlg != IndividualDivision.Show ? ConductInfo.FormRegist.ControlId.HistoryInfoIds[0] : ConductInfo.FormRegist.ControlId.HistoryIndividualInfoIds[0];
            if (!checkExclusiveSingle(ctrlId))
            {
                return true;
            }

            // 排他チェックOK
            return false;
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorRegist()
        {
            //対象機器のデータ取得（削除行は含まない）
            List<Dictionary<string, object>> machineDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormRegist.ControlId.MachineList);
            if (machineDicList == null || machineDicList.Count == 0)
            {
                //データがない場合、終了
                return false;
            }

            bool error = false;
            //エラー行保持用リスト
            List<int> lengthChkContent = new List<int>();
            List<int> dateChkPlan = new List<int>();
            List<int> dateChkCompletion = new List<int>();
            int rowNo = 0;
            //横方向一覧の直接入力は共通側でチェック不可のため業務側で行う
            foreach (var row in machineDicList)
            {
                //行数
                rowNo = rowNo + 1;

                // 更新フラグがtrueの行をチェックする
                if (!isUpdatedTableRow(row))
                {
                    continue;
                }

                //フォロー予定年月
                string followPlanDate = getDictionaryKeyValue(row, "follow_plan_date");
                //フォロー内容
                string followContent = getDictionaryKeyValue(row, "follow_content");
                //フォロー完了日
                string followCompletionDate = getDictionaryKeyValue(row, "follow_completion_date");

                // フォロー予定年月形式チェック
                if (!string.IsNullOrEmpty(followPlanDate) && !DateTime.TryParse(followPlanDate, out DateTime planDate))
                {
                    dateChkPlan.Add(rowNo);
                }
                //フォロー内容文字数チェック
                if (followContent.Length > followContentLength)
                {
                    lengthChkContent.Add(rowNo);
                }
                // フォロー完了日形式チェック
                if (!string.IsNullOrEmpty(followCompletionDate) && !DateTime.TryParse(followCompletionDate, out DateTime completionDate))
                {
                    dateChkCompletion.Add(rowNo);
                }
            }
            // フォロー予定年月のエラーメッセージを設定
            if (dateChkPlan.Count > 0)
            {
                string errRow = string.Join(",", dateChkPlan);
                // フォロー予定年月の値が不正です(1,2行目)
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage(ComRes.ID.ID941250001), GetResMessage(ComRes.ID.ID111280028)) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage(ComRes.ID.ID941250001), GetResMessage(ComRes.ID.ID111280028)) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                }
                error = true;
            }
            // フォロー内容のエラーメッセージを設定
            if (lengthChkContent.Count > 0)
            {
                string errRow = string.Join(",", lengthChkContent);
                // フォロー内容は○文字以内で入力してください。(1,2行目)
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage(ComRes.ID.ID941260014), GetResMessage(ComRes.ID.ID111280009), followContentLength) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage(ComRes.ID.ID941260014), GetResMessage(ComRes.ID.ID111280009), followContentLength) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                }
                error = true;
            }
            // フォロー完了日のエラーメッセージを設定
            if (dateChkCompletion.Count > 0)
            {
                string errRow = string.Join(",", dateChkCompletion);
                // フォロー完了日の値が不正です(1,2行目)
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage(ComRes.ID.ID941250001), GetResMessage(ComRes.ID.ID111280029)) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage(ComRes.ID.ID941250001), GetResMessage(ComRes.ID.ID111280029)) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                }
                error = true;
            }

            if (error)
            {
                return true;
            }

            List<Dao.detailMachine> machineList = new List<Dao.detailMachine>();
            foreach (Dictionary<string, object> machineDic in machineDicList)
            {
                //データクラスに変換
                Dao.detailMachine machine = new Dao.detailMachine();
                SetExecuteConditionByDataClass<Dao.detailMachine>(machineDic, ConductInfo.FormRegist.ControlId.MachineList, machine, DateTime.Now, this.UserId);
                machineList.Add(machine);
            }

            //機番ID、機器ID、部位ID、点検内容IDでグルーピングし、重複している件数を取得
            var count = machineList.GroupBy(x => new { x.MachineId, x.EquipmentId, x.InspectionSiteStructureId, x.InspectionContentStructureId }).Select(x => new { Count = x.Count() }).Where(x => x.Count > 1).Count();
            if (count > 0)
            {
                // 対象機器が重複しています。
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160001 });
                return true;
            }
            return false;
        }

        /// <summary>
        /// 登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNoList">取得するグループ番号のリスト</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(List<short> groupNoList, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            T resultInfo = new();

            foreach (short groupNo in groupNoList)
            {
                // 登録対象グループの画面項目定義の情報
                var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

                // 対象グループのコントロールIDの結果情報のみ抽出
                var ctrlIdList = grpMapInfo.CtrlIdList;
                List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

                // コントロールIDごとに繰り返し
                foreach (var ctrlId in ctrlIdList)
                {
                    // コントロールIDより画面の項目を取得
                    Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ctrlId);
                    var mapInfo = grpMapInfo.selectByCtrlId(ctrlId);
                    // 登録データの設定
                    if (!SetExecuteConditionByDataClass(condition, mapInfo.Value, resultInfo, now, this.UserId, this.UserId))
                    {
                        // エラーの場合終了
                        return resultInfo;
                    }
                }
            }
            return resultInfo;
        }

        /// <summary>
        /// 故障情報の場合、登録する内容(対象機器)をデータクラスで取得
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="ctrlId">取得するコントロールID</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getMachineInfo<T>(string ctrlId, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(ctrlId);
            // コントロールIDにより対象機器を取得
            List<Dictionary<string, object>> resultList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);
            T registInfo = new();
            if (resultList == null || resultList.Count <= 0)
            {
                return registInfo;
            }
            // データクラスに変換する(削除行は除くため、故障情報の対象機器は1件)
            SetExecuteConditionByDataClass<T>(resultList[0], ctrlId, registInfo, now, this.UserId, this.UserId);

            return registInfo;
        }

        /// <summary>
        /// 依頼番号を取得
        /// </summary>
        /// <param name="registSummaryInfo">保全活動件名の登録内容</param>
        /// <param name="now">システム日時</param>
        /// <returns>依頼番号</returns>
        private string getRequestNo(Dao.detailSummaryInfo registSummaryInfo, DateTime now)
        {
            //工場ID取得（ツリー選択ラベルの為、FactoryNameにIDが入ってくる想定）
            int factoryId = Convert.ToInt32(registSummaryInfo.FactoryName);
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = (int)Const.MsStructure.GroupId.RequestNumberingPattern;
            //連番
            param.Seq = RequestNumberingPattern.Seq;
            //依頼番号採番パターンの情報取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> patternList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);

            //工場IDで絞り込む
            List<TMQUtil.StructureItemEx.StructureItemExInfo> factoryPatternList = patternList.Where(x => x.FactoryId == factoryId).OrderBy(x => x.StructureId).ToList();
            if (factoryPatternList == null || factoryPatternList.Count == 0)
            {
                //工場固有のパターンが設定されていない場合、共通の定義を使用
                factoryPatternList = patternList.Where(x => x.FactoryId == 0).OrderBy(x => x.StructureId).ToList();
            }
            //採番パターン（パターンが複数取得できた場合、構成IDが最小のものを使用する）
            string pattern = factoryPatternList[0].ExData;

            // 更新者ID、登録者IDの変換
            bool chkUpd = int.TryParse(this.UserId, out int updatorCdNum);
            bool chkInp = int.TryParse(this.UserId, out int inputorCdNum);
            // いずれかが変換エラーの場合、エラーを返す
            if (!chkUpd || !chkInp)
            {
                return null;
            }

            //依頼番号に使用する連番
            int seqNo = 0;

            //検索条件
            ComDao.MaRequestNumberingEntity condition = new ComDao.MaRequestNumberingEntity();
            condition.NumberingPattern = Convert.ToInt32(pattern);
            condition.Year = Convert.ToInt32(now.ToString("yyyy"));
            //パターン1の場合は画面で選択されている場所階層ID、左記以外は0
            condition.LocationStructureId = pattern == RequestNumberingPattern.Pattern1 ? (registSummaryInfo.LocationStructureId ?? 0) : 0;

            //採番テーブルを検索
            ComDao.MaRequestNumberingEntity result = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.MaRequestNumberingEntity>(SqlName.Regist.GetRequestNumbering, SqlName.SubDir, condition, this.db);
            if (result == null)
            {
                //レコードが存在しない場合、登録する

                condition.SeqNo = 1;
                // 共通の更新日時などを設定
                setExecuteConditionByDataClassCommon<ComDao.MaRequestNumberingEntity>(ref condition, now, updatorCdNum, inputorCdNum);

                //登録
                int val = -1;
                bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out val, SqlName.Regist.InsertRequestNumbering, SqlName.SubDir, condition, this.db);
                if (!returnFlag)
                {
                    return null;
                }

                //登録した連番を設定
                seqNo = val;
            }
            else
            {
                // 共通の更新日時などを設定
                setExecuteConditionByDataClassCommon<ComDao.MaRequestNumberingEntity>(ref result, now, updatorCdNum, -1);
                //更新(連番を+1する)
                int val = -1;
                bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out val, SqlName.Regist.UpdateRequestNumbering, SqlName.SubDir, result, this.db);
                if (!returnFlag)
                {
                    return null;
                }

                //更新した連番を設定
                seqNo = val;

            }
            //コミット
            db.Commit();
            //トランザクション終了
            db.EndTransaction();
            //トランザクション開始
            db.BeginTransaction();

            //採番パターンより依頼番号を取得
            string requestNo = getNumberingPattern(pattern, seqNo, registSummaryInfo.LocationStructureId ?? 0, now);

            return requestNo;
        }

        /// <summary>
        /// 採番パターンより依頼番号を取得
        /// </summary>
        /// <param name="pattern">採番パターン</param>
        /// <param name="seqNo">採番テーブルから取得した連番</param>
        /// <param name="locationStructureId">場所階層ID</param>
        /// <param name="now">システム日時</param>
        /// <returns>依頼番号の文字列</returns>
        private string getNumberingPattern(string pattern, int seqNo, int locationStructureId, DateTime now)
        {
            string requestNo = "";
            switch (pattern)
            {
                case RequestNumberingPattern.Pattern1:
                    //1+場所階層ID+yyyy+連番3桁
                    requestNo = "1" + locationStructureId + now.ToString("yyyy") + seqNo.ToString("D3");
                    break;
                case RequestNumberingPattern.Pattern2:
                    //1+yyyy+連番5桁
                    requestNo = "1" + now.ToString("yyyy") + seqNo.ToString("D5");
                    break;
                case RequestNumberingPattern.Pattern3:
                    //yy+連番4桁
                    requestNo = now.ToString("yy") + seqNo.ToString("D4");
                    break;
                case RequestNumberingPattern.Pattern4:
                    //R+連番7桁
                    requestNo = "R" + seqNo.ToString("D7");
                    break;
            }
            return requestNo;
        }

        /// <summary>
        /// 点検情報の場合の登録更新
        /// </summary>
        /// <param name="summaryId">保全活動件名ID</param>
        /// <param name="isRegist">新規登録の場合True</param>
        /// <param name="maintenanceFlg">保全権限がある場合True</param>
        /// <param name="registSummaryInfo">保全活動件名情報</param>
        /// <param name="now">システム日時</param>
        /// <returns>エラーの場合False</returns>
        private bool registInspectionData(long summaryId, bool isRegist, bool maintenanceFlg, Dao.detailSummaryInfo registSummaryInfo, DateTime now)
        {
            // 保全履歴登録
            (bool returnFlag, long val) resultHistory = registHistory(summaryId, isRegist, maintenanceFlg, registSummaryInfo.HistoryIndividualFlg, now);
            if (!resultHistory.returnFlag)
            {
                return false;
            }

            //対象機器の情報取得
            var mappingInfo = getResultMappingInfo(ConductInfo.FormRegist.ControlId.MachineList);
            //削除行も併せて取得
            List<Dictionary<string, object>> machineDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormRegist.ControlId.MachineList, true);
            foreach (Dictionary<string, object> machineDic in machineDicList)
            {
                //データクラスに変換
                Dao.detailMachine machine = new Dao.detailMachine();
                if (!SetExecuteConditionByDataClass<Dao.detailMachine>(machineDic, ConductInfo.FormRegist.ControlId.MachineList, machine, now, this.UserId, this.UserId))
                {
                    return false;
                }

                if (ComUtil.IsEqualRowStatus(machineDic, TMPTBL_CONSTANTS.ROWSTATUS.New))
                {
                    //追加行の登録
                    if (!registAddRow(machine))
                    {
                        return false;
                    }

                    continue;
                }
                if (ComUtil.IsEqualRowStatus(machineDic, TMPTBL_CONSTANTS.ROWSTATUS.None))
                {
                    //削除行の削除
                    if (!deleteRow(machine, machineDic))
                    {
                        return false;
                    }
                    continue;
                }
                if (isUpdatedTableRow(machineDic))
                {
                    //修正行の更新
                    if (!updateRow(machine))
                    {
                        return false;
                    }
                    continue;
                }
            }

            return true;

            //追加行の登録
            bool registAddRow(Dao.detailMachine machine)
            {
                //保全履歴機器の検索条件設定
                ComDao.MaHistoryMachineEntity historyMachine = new ComDao.MaHistoryMachineEntity();
                historyMachine.HistoryId = resultHistory.val;
                historyMachine.MachineId = machine.MachineId;
                historyMachine.EquipmentId = machine.EquipmentId;
                // 共通の更新日時などを設定
                setExecuteConditionByDataClassCommon<ComDao.MaHistoryMachineEntity>(ref historyMachine, now, machine.UpdateUserId, machine.InsertUserId);

                // 保全履歴機器が登録済みかチェック
                ComDao.MaHistoryMachineEntity resultHistoryMachine = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.MaHistoryMachineEntity>(SqlName.Regist.GetHistoryMachine, SqlName.SubDir, historyMachine, this.db);
                if (resultHistoryMachine == null)
                {
                    //保全履歴機器が未登録の場合、保全履歴機器、保全履歴機器部位、保全履歴点検内容を登録

                    //保全履歴機器
                    //機器使用期間の設定
                    int days = setUsedDaysMachine(registSummaryInfo, machine);
                    if (days >= 0)
                    {
                        historyMachine.UsedDaysMachine = days;
                    }

                    //登録
                    long val = -1;
                    bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out val, SqlName.Regist.InsertHistoryMachine, SqlName.SubDir, historyMachine, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }

                    //保全履歴機器部位
                    machine.HistoryMachineId = val;
                    //登録
                    val = -1;
                    returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out val, SqlName.Regist.InsertHistoryInspectionSite, SqlName.SubDir, machine, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }

                    //保全履歴点検内容
                    machine.HistoryInspectionSiteId = val;
                    //登録
                    bool registHistoryInspectionContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.InsertHistoryInspectionContent, SqlName.SubDir, machine, this.db);
                    if (!registHistoryInspectionContent)
                    {
                        return false;
                    }
                }
                else
                {
                    //保全履歴機器は登録済み

                    //保全履歴機器
                    //機器使用期間の設定
                    int days = setUsedDaysMachine(registSummaryInfo, machine);
                    if (days >= 0)
                    {
                        historyMachine.UsedDaysMachine = days;
                        historyMachine.HistoryMachineId = resultHistoryMachine.HistoryMachineId;
                        //機器使用期間の更新
                        bool registHistoryMachine = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryMachine, SqlName.SubDir, historyMachine, this.db);
                        if (!registHistoryMachine)
                        {
                            return false;
                        }
                    }

                    //保全履歴機器部位が登録済みかチェック
                    machine.HistoryMachineId = resultHistoryMachine.HistoryMachineId;
                    Dao.detailMachine resultInspectionSite = TMQUtil.SqlExecuteClass.SelectEntity<Dao.detailMachine>(SqlName.Regist.GetHistoryInspectionSite, SqlName.SubDir, machine, this.db);
                    if (resultInspectionSite == null)
                    {
                        //保全履歴機器部位が未登録の場合、保全履歴機器部位、保全履歴点検内容を登録

                        //保全履歴機器部位
                        machine.HistoryMachineId = resultHistoryMachine.HistoryMachineId;
                        //登録
                        long val = -1;
                        bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out val, SqlName.Regist.InsertHistoryInspectionSite, SqlName.SubDir, machine, this.db);
                        if (!returnFlag)
                        {
                            return false;
                        }

                        //保全履歴点検内容
                        machine.HistoryInspectionSiteId = val;
                        //登録
                        bool registHistoryInspectionContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.InsertHistoryInspectionContent, SqlName.SubDir, machine, this.db);
                        if (!registHistoryInspectionContent)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //保全履歴機器部位は登録済み

                        //保全履歴点検内容
                        machine.HistoryInspectionSiteId = resultInspectionSite.HistoryInspectionSiteId;
                        //登録
                        bool registHistoryInspectionContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.InsertHistoryInspectionContent, SqlName.SubDir, machine, this.db);
                        if (!registHistoryInspectionContent)
                        {
                            return false;
                        }
                    }

                }
                return true;
            }

            //削除行の削除
            bool deleteRow(Dao.detailMachine machine, Dictionary<string, object> machineDic)
            {
                if (machine.HistoryMachineId == null)
                {
                    //追加後、削除した行のため、処理なし
                    return true;
                }

                if (machine.HistoryInspectionContentId != null)
                {
                    //保全履歴点検内容の削除
                    if (!deleteInspectionTable<ComDao.MaHistoryInspectionContentEntity>(ConductInfo.FormRegist.ControlId.MachineList, "history_inspection_content_id", SqlName.Detail.DeleteHistoryInspectionContent, machineDic))
                    {
                        return false;
                    }

                }

                if (machine.HistoryInspectionSiteId != null)
                {
                    //保全履歴機器部位に紐づく保全履歴点検内容の存在チェック
                    //SQL文の取得
                    string sql;
                    if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetCountHistoryInspectionContent, out sql))
                    {
                        return false;
                    }
                    StringBuilder execSql = new StringBuilder();
                    execSql.AppendLine(sql);

                    // 件数を取得
                    int cnt = db.GetCount(execSql.ToString(), machine);
                    if (cnt > 0)
                    {
                        //保全履歴機器部位に紐づく保全履歴点検内容が存在するため、保全履歴機器部位は削除しない
                        return true;
                    }

                    //保全履歴機器部位の削除
                    if (!deleteInspectionTable<ComDao.MaHistoryInspectionSiteEntity>(ConductInfo.FormRegist.ControlId.MachineList, "history_inspection_site_id", SqlName.Detail.DeleteHistoryInspectionSite, machineDic))
                    {
                        return false;
                    }
                }

                if (machine.HistoryMachineId != null)
                {
                    //保全履歴機器に紐づく保全履歴機器部位の存在チェック
                    //SQL文の取得
                    string sql;
                    if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Regist.GetCountHistoryInspectionSite, out sql))
                    {
                        return false;
                    }
                    StringBuilder execSql = new StringBuilder();
                    execSql.AppendLine(sql);

                    // 件数を取得
                    int cnt = db.GetCount(execSql.ToString(), machine);
                    if (cnt > 0)
                    {
                        //保全履歴機器に紐づく保全履歴機器部位が存在するため、保全履歴機器は削除しない
                        return true;
                    }

                    //保全履歴機器の削除
                    if (!deleteInspectionTable<ComDao.MaHistoryMachineEntity>(ConductInfo.FormRegist.ControlId.MachineList, "history_machine_id", SqlName.Detail.DeleteHistoryMachine, machineDic))
                    {
                        return false;
                    }

                }

                return true;
            }

            //修正行の更新
            bool updateRow(Dao.detailMachine machine)
            {
                //保全履歴機器
                if (machine.HistoryMachineId != null)
                {
                    //機器使用期間の取得
                    int days = setUsedDaysMachine(registSummaryInfo, machine);
                    if (days >= 0)
                    {
                        //保全履歴機器の設定
                        ComDao.MaHistoryMachineEntity historyMachine = new ComDao.MaHistoryMachineEntity();
                        historyMachine.UsedDaysMachine = days;
                        historyMachine.HistoryMachineId = machine.HistoryMachineId ?? -1;
                        // 共通の更新日時などを設定
                        setExecuteConditionByDataClassCommon<ComDao.MaHistoryMachineEntity>(ref historyMachine, now, machine.UpdateUserId, -1);
                        //機器使用期間の更新
                        bool registHistoryMachine = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryMachine, SqlName.SubDir, historyMachine, this.db);
                        if (!registHistoryMachine)
                        {
                            return false;
                        }
                    }
                }

                //保全部位はラベルのため保全履歴機器部位の更新なし

                //保全履歴点検内容
                if (machine.HistoryInspectionContentId != null)
                {
                    //登録
                    bool registHistoryInspectionContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryInspectionContent, SqlName.SubDir, machine, this.db);
                    if (!registHistoryInspectionContent)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 故障情報の場合の登録更新
        /// </summary>
        /// <param name="summaryId">保全活動件名ID</param>
        /// <param name="isRegist">新規登録の場合True</param>
        /// <param name="maintenanceFlg">保全権限がある場合True</param>
        /// <param name="now">システム日時</param>
        /// <returns>エラーの場合False</returns>
        private bool registFailureData(long summaryId, bool isRegist, bool maintenanceFlg, Dao.detailSummaryInfo registSummaryInfo, DateTime now)
        {
            // 保全履歴登録
            (bool returnFlag, long val) resultHistory = registHistory(summaryId, isRegist, maintenanceFlg, registSummaryInfo.HistoryIndividualFlg, now);
            if (!resultHistory.returnFlag)
            {
                return false;
            }

            //故障情報取得
            List<short> grpNoList = new List<short>() { ConductInfo.FormRegist.GroupNo.FailureInfo };
            //故障分析個別工場表示フラグ取得
            string failureIndividualFlg = registSummaryInfo.FailureIndividualFlg;
            if (failureIndividualFlg == IndividualDivision.Show)
            {
                //故障分析情報(個別工場)タブのグループ番号を追加
                grpNoList.Add(ConductInfo.FormRegist.GroupNo.FailureStatusInfo);
                grpNoList.Add(ConductInfo.FormRegist.GroupNo.FailureCauseInfo);
                grpNoList.Add(ConductInfo.FormRegist.GroupNo.RecoveryActionInfo);
                grpNoList.Add(ConductInfo.FormRegist.GroupNo.ImprovementMeasureInfo);
            }
            else
            {
                //故障分析情報タブのグループ番号を追加
                grpNoList.Add(ConductInfo.FormRegist.GroupNo.FailureAnalyzeInfo);
            }
            Dao.historyFailure registFailureInfo = getRegistInfo<Dao.historyFailure>(grpNoList, now);
            //最下層の構成IDを取得して原因性格階層IDにセットする
            IList<Dao.historyFailure> results = new List<Dao.historyFailure>();
            results.Add(registFailureInfo);
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.historyFailure>(ref results, new List<StructureType> { StructureType.FailureCause });

            //対象機器の情報取得(削除行は除くため、対象機器は1件)
            Dao.detailMachine registMachine = getMachineInfo<Dao.detailMachine>(ConductInfo.FormRegist.ControlId.MachineList, now);

            //保全履歴故障情報
            registFailureInfo.HistoryId = resultHistory.val;
            registFailureInfo.MachineId = registMachine.MachineId;
            registFailureInfo.EquipmentId = registMachine.EquipmentId;

            //機器使用期間の設定
            int days = setUsedDaysMachine(registSummaryInfo, registMachine);
            if (days >= 0)
            {
                registFailureInfo.UsedDaysMachine = days;
            }

            //実行SQLファイル名
            string sqlFile = SqlName.Regist.InsertHistoryFailure;

            //保全履歴故障情報が登録済みかチェック
            Dao.historyFailure historyFailure = TMQUtil.SqlExecuteClass.SelectEntity<Dao.historyFailure>(SqlName.Regist.GetFailureInfoForHistoryId, SqlName.SubDir, registFailureInfo, this.db);
            if (historyFailure != null)
            {
                //更新SQL
                sqlFile = SqlName.Regist.UpdateHistoryFailure;
            }
            //登録・更新
            bool resultFailure = TMQUtil.SqlExecuteClass.Regist(sqlFile, SqlName.SubDir, registFailureInfo, this.db);
            if (!resultFailure)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 機器使用期間の設定
        /// </summary>
        /// <param name="summaryInfo">保全活動件名情報</param>
        /// <param name="machine">対象機器</param>
        /// <returns>機器使用期間、取得できない場合-1</returns>
        private int setUsedDaysMachine(Dao.detailSummaryInfo summaryInfo, Dao.detailMachine machine)
        {
            int val = -1;
            ComDao.McMachineEntity machineEntity = machine.GetEntity(machine.MachineId, this.db);
            if (summaryInfo.CompletionDate != null && machineEntity != null && machineEntity.DateOfInstallation != null)
            {
                //機器使用日数 = 完了日 - 設置日
                TimeSpan days = (TimeSpan)(summaryInfo.CompletionDate - machineEntity.DateOfInstallation);
                val = days.Days;
            }
            return val;
        }

        /// <summary>
        /// 保全履歴の登録・更新
        /// </summary>
        /// <param name="summaryId">保全活動件名ID</param>
        /// <param name="isRegist">新規登録の場合True</param>
        /// <param name="maintenanceFlg">保全権限がある場合True</param>
        /// <param name="historyIndividualFlg">保全履歴個別工場表示フラグ</param>
        /// <param name="now">システム日時</param>
        /// <returns>returnFlag:エラーの場合False、val:登録データの取得値</returns>
        private (bool returnFlag, long val) registHistory(long summaryId, bool isRegist, bool maintenanceFlg, string historyIndividualFlg, DateTime now)
        {
            List<short> grpNoList = new List<short>();
            //表示しているタブの情報を登録する
            if (historyIndividualFlg != IndividualDivision.Show)
            {
                //保全履歴タブ
                grpNoList = new List<short>() { ConductInfo.FormRegist.GroupNo.HistoryInfo, ConductInfo.FormRegist.GroupNo.WorkTimeInfo, ConductInfo.FormRegist.GroupNo.CostInfo };
            }
            else
            {
                //保全履歴（個別工場）タブ
                grpNoList = new List<short>() { ConductInfo.FormRegist.GroupNo.HistoryIndividualInfo };
            }
            Dao.historyInfo registHistoryInfo = getRegistInfo<Dao.historyInfo>(grpNoList, now);
            registHistoryInfo.SummaryId = summaryId;

            if (historyIndividualFlg == IndividualDivision.Hide && registHistoryInfo.CallCount != null)
            {
                //個別工場タブ非表示の場合、呼出に登録する値（構成IDから拡張データ）を取得する
                //個別工場タブ表示の場合、入力された数値をそのまま登録する

                //構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                //構成グループID
                param.StructureGroupId = (int)Const.MsStructure.GroupId.Call;
                //連番
                param.Seq = CallDivision.Seq;
                //データタイプ
                param.DataType = CallDivision.DataType;
                //呼出の構成IDから拡張データを取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
                string callCountStr = list.Where(x => x.StructureId == (registHistoryInfo.CallCount ?? 0)).Select(x => x.ExData).FirstOrDefault();
                registHistoryInfo.CallCount = callCountStr != null ? Convert.ToInt32(callCountStr) : null;
            }

            // 保全履歴登録
            if (maintenanceFlg || isRegist)
            {
                //保全権限がない場合は依頼情報以外のタブの更新は行わない（新規登録の場合はレコードは作成しておく）
                long val = -1;
                bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out val, isRegist ? SqlName.Regist.InsertHistory : SqlName.Regist.UpdateHistory, SqlName.SubDir, registHistoryInfo, this.db);
                return (returnFlag, val);
            }
            return (true, registHistoryInfo.HistoryId);
        }

        /// <summary>
        /// 個別工場表示フラグ取得処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        private int GetIndividualFlg()
        {
            //選択された工場IDを取得
            object id = GetGlobalData("factoryId", false);
            int factoryId = Convert.ToInt32(id);

            // 保全履歴個別表示制御に使用するフラグを設定
            string historyIndividualFlg = getItemExData(HistoryIndividualDivision.Seq, HistoryIndividualDivision.DataType, factoryId);
            SetGlobalData("historyIndividualFlg", historyIndividualFlg == null ? IndividualDivision.Hide : historyIndividualFlg);

            // 故障分析個別表示制御に使用するフラグを設定
            string failureIndividualFlg = getItemExData(FailureIndividualDivision.Seq, FailureIndividualDivision.DataType, factoryId);
            SetGlobalData("failureIndividualFlg", failureIndividualFlg == null ? IndividualDivision.Hide : failureIndividualFlg);

            return ComConsts.RETURN_RESULT.OK;
        }
    }
}
