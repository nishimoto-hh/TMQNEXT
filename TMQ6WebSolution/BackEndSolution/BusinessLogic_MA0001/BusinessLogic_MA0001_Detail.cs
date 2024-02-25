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
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MA0001.BusinessLogicDataClass_MA0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using System.Collections;

namespace BusinessLogic_MA0001
{
    /// <summary>
    /// 保全活動（詳細）
    /// </summary>
    public partial class BusinessLogic_MA0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 詳細画面検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchDetailList()
        {
            //画面NO毎に画面定義のコントロールIDを設定
            string fromCtrlId = "";
            List<string> infoCtrlIdList = new List<string>();
            string machineListId = "";
            List<string> failureInfoIdList = new List<string>();
            List<string> requestInfoIdList = new List<string>();
            string planId = "";
            List<string> historyInfoIdList = new List<string>();
            List<string> historyIndividualInfoIdList = new List<string>();
            string failureAnalyzeInfoId = "";
            List<string> failureAnalyzeIndividualInfoIdList = new List<string>();
            Dao.searchCondition conditionObj = new Dao.searchCondition();

            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            switch (this.FormNo)
            {
                case ConductInfo.FormDetail.FormNo:   //詳細
                    //登録画面から詳細画面に遷移した場合、一覧画面から詳細画面に遷移した場合
                    fromCtrlId = compareId.IsRegist() ? ConductInfo.FormRegist.ControlId.DetailInfoIds[0] : ConductInfo.FormList.ControlId.SearchResult;
                    infoCtrlIdList.AddRange(ConductInfo.FormDetail.ControlId.DetailInfoIds.ToList());
                    machineListId = ConductInfo.FormDetail.ControlId.MachineList;
                    failureInfoIdList.AddRange(ConductInfo.FormDetail.ControlId.FailureInfoIds.ToList());
                    requestInfoIdList.AddRange(ConductInfo.FormDetail.ControlId.RequestInfoIds.ToList());
                    planId = ConductInfo.FormDetail.ControlId.PlanId;
                    historyInfoIdList.AddRange(ConductInfo.FormDetail.ControlId.HistoryInfoIds.ToList());
                    historyIndividualInfoIdList.AddRange(ConductInfo.FormDetail.ControlId.HistoryIndividualInfoIds.ToList());
                    failureAnalyzeInfoId = ConductInfo.FormDetail.ControlId.FailureAnalyzeInfoId;
                    failureAnalyzeIndividualInfoIdList.AddRange(ConductInfo.FormDetail.ControlId.FailureAnalyzeIndividualInfoIds.ToList());
                    break;
                case ConductInfo.FormRegist.FormNo:   //修正
                    fromCtrlId = ConductInfo.FormDetail.ControlId.DetailInfoIds[0];
                    infoCtrlIdList.AddRange(ConductInfo.FormRegist.ControlId.DetailInfoIds.ToList());
                    machineListId = ConductInfo.FormRegist.ControlId.MachineList;
                    failureInfoIdList.AddRange(ConductInfo.FormRegist.ControlId.FailureInfoIds.ToList());
                    requestInfoIdList.AddRange(ConductInfo.FormRegist.ControlId.RequestInfoIds.ToList());
                    planId = ConductInfo.FormRegist.ControlId.PlanId;
                    historyInfoIdList.AddRange(ConductInfo.FormRegist.ControlId.HistoryInfoIds.ToList());
                    historyIndividualInfoIdList.AddRange(ConductInfo.FormRegist.ControlId.HistoryIndividualInfoIds.ToList());
                    failureAnalyzeInfoId = ConductInfo.FormRegist.ControlId.FailureAnalyzeId;
                    failureAnalyzeIndividualInfoIdList.AddRange(ConductInfo.FormRegist.ControlId.FailureAnalyzeIndividualInfoIds.ToList());
                    break;
                default:
                    return false;
            }

            // ページ情報取得
            var pageInfo = GetPageInfo(fromCtrlId, this.pageInfoList);
            //選択行のデータを取得
            SetSearchConditionByDataClass(this.searchConditionDictionary, fromCtrlId, conditionObj, pageInfo);
            if(conditionObj.SummaryId == null)
            {
                //条件を取得できなかった場合（登録後、詳細画面で登録画面を開き閉じた場合）再取得
                SetSearchConditionByDataClass(this.searchConditionDictionary, ConductInfo.FormRegist.ControlId.DetailInfoIds[0], conditionObj, pageInfo);
            }
            //件名情報、作業性格情報の取得
            Dao.detailSummaryInfo result = getSummaryInfo(conditionObj, infoCtrlIdList, SqlName.Detail.GetSummaryInfo, new List<StructureType> { StructureType.Location, StructureType.Job });
            if (result == null)
            {
                return false;
            }

            //対象機器、故障情報の取得
            switch (result.ActivityDivision)
            {
                case MaintenanceDivision.Inspection:    //点検
                    //対象機器
                    setMachineList<Dao.detailMachine>(conditionObj, machineListId, SqlName.Detail.GetInspectionMachineList, result.ActivityDivision);
                    break;
                case MaintenanceDivision.Failure:   //故障
                    //対象機器
                    setMachineList<Dao.detailMachine>(conditionObj, machineListId, SqlName.Detail.GetFailureMachineList, result.ActivityDivision);
                    //故障情報
                    conditionObj.FactoryId = result.FactoryId;
                    getDetailInfo<Dao.historyFailure>(conditionObj, failureInfoIdList, SqlName.Detail.GetFailureInfo, new List<StructureType> { StructureType.FailureCause });
                    //故障分析情報
                    getDetailInfo<Dao.FailureAnalyzeInfo>(conditionObj, new List<string> { failureAnalyzeInfoId }, SqlName.Detail.GetFailureAnalyzeInfo, null);
                    //故障分析情報（個別工場）
                    getDetailInfo<Dao.FailureAnalyzeInfo>(conditionObj, failureAnalyzeIndividualInfoIdList, SqlName.Detail.GetFailureAnalyzeIndividualInfo, null);
                    break;
                default:
                    return false;
            }

            //依頼概要、依頼申請情報の取得
            ComDao.MaRequestEntity requestInfo = getRequestInfo(conditionObj, requestInfoIdList, SqlName.Detail.GetRequestInfo, compareId.IsCopy());
            //保全計画情報の取得
            ComDao.MaPlanEntity planInfo = getDetailInfo<ComDao.MaPlanEntity>(conditionObj, new List<string> { planId }, SqlName.Detail.GetPlanInfo, null);
            //保全履歴情報の取得
            conditionObj.FactoryId = result.FactoryId;
            Dao.historyInfo historyInfo = getDetailInfo<Dao.historyInfo>(conditionObj, historyInfoIdList, SqlName.Detail.GetHistoryInfo, null);
            //保全履歴情報（個別工場）の取得
            Dao.historyInfo historyIndividualInfo = getDetailInfo<Dao.historyInfo>(conditionObj, historyIndividualInfoIdList, SqlName.Detail.GetHistoryIndividualInfo, null);

            // 画面定義の翻訳情報取得
            GetContorlDefineTransData(result.FactoryId ?? -1);
            return true;
        }

        /// <summary>
        /// 件名情報、作業性格情報、非表示項目の取得と設定
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="detailInfoIds">一覧のコントロールIDリスト</param>
        /// <param name="sqlFileName">SQLファイル名</param>
        /// <param name="setStructureType">データクラスに設定する階層情報の種類</param>
        /// <returns>エラーの場合null</returns>
        private Dao.detailSummaryInfo getSummaryInfo(Dao.searchCondition condition, List<string> detailInfoIds, string sqlFileName, List<StructureType> setStructureType)
        {
            // ページ情報取得
            Dictionary<string, PageInfo> pageInfos = new Dictionary<string, PageInfo>();
            foreach (string id in detailInfoIds)
            {
                var pageInfo = GetPageInfo(id, this.pageInfoList);
                pageInfos.Add(id, pageInfo);
            }

            // 一覧検索実行
            Dao.detailSummaryInfo result = TMQUtil.SqlExecuteClass.SelectEntity<Dao.detailSummaryInfo>(sqlFileName, SqlName.SubDir, condition, this.db);
            if (result == null)
            {
                return null;
            }

            if (setStructureType != null)
            {
                IList<Dao.detailSummaryInfo> results = new List<Dao.detailSummaryInfo>();
                results.Add(result);
                // 階層情報を設定
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.detailSummaryInfo>(ref results, setStructureType, this.db, this.LanguageId, true);
            }

            //完了日を非表示項目に設定
            result.HideCompletionDate = result.CompletionDate;

            // 突発区分の必須チェックに使用するMQ分類の構成IDを設定
            result.MqNotRequiredStructureId = setMqNotRequiredStructureId();

            // 保全履歴個別表示制御に使用するフラグを設定
            string historyIndividualFlg = getItemExData(HistoryIndividualDivision.Seq, HistoryIndividualDivision.DataType, result.FactoryId);
            result.HistoryIndividualFlg = historyIndividualFlg == null ? IndividualDivision.Hide : historyIndividualFlg;

            // 故障分析個別表示制御に使用するフラグを設定
            string failureIndividualFlg = getItemExData(FailureIndividualDivision.Seq, FailureIndividualDivision.DataType, result.FactoryId);
            result.FailureIndividualFlg = failureIndividualFlg == null ? IndividualDivision.Hide : failureIndividualFlg;

            // 他機能から遷移してきた場合、指定されたタブを表示する
            result.TabNo = condition.TabNo;

            // ユーザ役割の設定
            setUserRole<Dao.detailSummaryInfo>(result);

            foreach (string id in detailInfoIds)
            {
                // 検索結果の設定
                if (SetSearchResultsByDataClass<Dao.detailSummaryInfo>(pageInfos[id], new List<Dao.detailSummaryInfo> { result }, 1))
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                }
            }

            return result;
        }

        /// <summary>
        /// 依頼概要、依頼申請情報の取得と設定
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="detailInfoIds">一覧のコントロールIDリスト</param>
        /// <param name="sqlFileName">SQLファイル名</param>
        /// <param name="isCopy">複写初期表示の場合True</param>
        /// <returns>エラーの場合null</returns>
        private ComDao.MaRequestEntity getRequestInfo(Dao.searchCondition condition, List<string> detailInfoIds, string sqlFileName, bool isCopy)
        {
            // ページ情報取得
            Dictionary<string, PageInfo> pageInfos = new Dictionary<string, PageInfo>();
            foreach (string id in detailInfoIds)
            {
                var pageInfo = GetPageInfo(id, this.pageInfoList);
                pageInfos.Add(id, pageInfo);
            }

            // 一覧検索実行
            ComDao.MaRequestEntity result = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.MaRequestEntity>(sqlFileName, SqlName.SubDir, condition, this.db);
            if (result == null)
            {
                return null;
            }

            if (isCopy)
            {
                // 複写の場合、発行日にシステム日付を設定
                result.IssueDate = DateTime.Now;
            }

            foreach (string id in detailInfoIds)
            {
                // 検索結果の設定
                if (SetSearchResultsByDataClass<ComDao.MaRequestEntity>(pageInfos[id], new List<ComDao.MaRequestEntity> { result }, 1))
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                }
            }

            return result;
        }

        /// <summary>
        /// 故障情報、依頼概要、依頼申請情報、保全計画情報、保全履歴情報、故障分析情報の取得と設定
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="condition">検索条件</param>
        /// <param name="detailInfoIds">一覧のコントロールIDリスト</param>
        /// <param name="sqlFileName">SQLファイル名</param>
        /// <param name="setStructureType">データクラスに設定する階層情報の種類</param>
        /// <returns>エラーの場合null</returns>
        private T getDetailInfo<T>(Dao.searchCondition condition, List<string> detailInfoIds, string sqlFileName, List<StructureType> setStructureType)
        {
            // ページ情報取得
            Dictionary<string, PageInfo> pageInfos = new Dictionary<string, PageInfo>();
            foreach (string id in detailInfoIds)
            {
                var pageInfo = GetPageInfo(id, this.pageInfoList);
                pageInfos.Add(id, pageInfo);
            }

            // 一覧検索実行
            T result = TMQUtil.SqlExecuteClass.SelectEntity<T>(sqlFileName, SqlName.SubDir, condition, this.db);
            if (result == null)
            {
                return default(T);
            }

            if (setStructureType != null)
            {
                IList<T> results = new List<T>();
                results.Add(result);
                // 階層情報を設定
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<T>(ref results, setStructureType, this.db, this.LanguageId, true, condition.FactoryId ?? 0);
            }

            foreach (string id in detailInfoIds)
            {
                // 検索結果の設定
                if (SetSearchResultsByDataClass<T>(pageInfos[id], new List<T> { result }, 1))
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                }
            }

            return result;
        }

        /// <summary>
        /// 対象機器の設定
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="condition">検索条件</param>
        /// <param name="machineListId">一覧のコントロールIDリスト</param>
        /// <param name="sqlFileName">SQLファイル名</param>
        /// <param name="activityDivisionId">活動区分ID</param>
        private void setMachineList<T>(Dao.searchCondition condition, string machineListId, string sqlFileName, int? activityDivisionId)
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(machineListId, this.pageInfoList);

            string sql; // 取得SQL文

            // 一覧検索SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlFileName, out sql))
            {
                return;
            }
            StringBuilder execSql = new StringBuilder();
            execSql.AppendLine(sql);

            // 一覧検索実行
            IList<T> results = db.GetListByDataClass<T>(execSql.ToString(), condition);
            if (results == null)
            {
                return;
            }

            // 職種～機種小分類を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<T>(ref results, new List<StructureType> { StructureType.Job }, this.db, this.LanguageId);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<T>(pageInfo, results, 1))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
        }

        /// <summary>
        /// 詳細情報の削除
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool deleteDetail()
        {
            // =========保全活動件名=========
            //排他ロック用キー＆値が非表示列で設定されているコントロールID
            string ctrlId = ConductInfo.FormDetail.ControlId.DetailInfoIds[0];
            //保全活動件名ID
            string summaryId = getValueByKeyName(ctrlId, "summary_id");
            //保全活動区分
            string maintenanceDivisionId = getValueByKeyName(ctrlId, "activity_division");
            //保全活動件名削除SQL文の取得
            string sql;
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.DeleteSummary, out sql))
            {
                return false;
            }
            //削除処理
            if (!deleteTargetCtrl<ComDao.MaSummaryEntity>(ctrlId, sql))
            {
                return false;
            }

            // =========保全依頼=========
            //排他ロック用キー＆値が非表示列で設定されているコントロールID
            ctrlId = ConductInfo.FormDetail.ControlId.RequestInfoIds[0];
            //削除処理
            if (!deleteTable<ComDao.MaRequestEntity>(ctrlId, "request_id", SqlName.Detail.DeleteRequest))
            {
                return false;
            }

            // =========保全計画=========
            //排他ロック用キー＆値が非表示列で設定されているコントロールID
            ctrlId = ConductInfo.FormDetail.ControlId.PlanId;
            if (!deleteTable<ComDao.MaPlanEntity>(ctrlId, "plan_id", SqlName.Detail.DeletePlan))
            {
                return false;
            }

            // =========保全履歴=========
            //排他ロック用キー＆値が非表示列で設定されているコントロールID
            ctrlId = ConductInfo.FormDetail.ControlId.HistoryInfoIds[0];
            if (!deleteTable<ComDao.MaHistoryEntity>(ctrlId, "history_id", SqlName.Detail.DeleteHistory))
            {
                return false;
            }

            // =========保全履歴機器、保全履歴機器部位、保全履歴点検内容、保全履歴故障情報=========
            if (maintenanceDivisionId == MaintenanceDivision.Inspection.ToString())
            {
                //点検情報の場合
                if (!deleteMachineList())
                {
                    return false;
                }
            }
            else
            {
                //故障情報の場合

                //排他ロック用キー＆値が非表示列で設定されているコントロールID
                ctrlId = ConductInfo.FormDetail.ControlId.FailureInfoIds[0];
                //削除処理
                if (!deleteTable<ComDao.MaHistoryFailureEntity>(ctrlId, "history_failure_id", SqlName.Detail.DeleteHistoryFailure))
                {
                    return false;
                }

            }

            //=========添付情報=========
            Dao.AttachmentInfo attachment = new Dao.AttachmentInfo();
            attachment.FunctionTypeIdList = new List<int>()
            {
                (int)Const.Attachment.FunctionTypeId.Summary, //件名添付
            };
            attachment.KeyId = Convert.ToInt64(summaryId);
            //削除する添付情報を取得
            List<ComDao.AttachmentEntity> result = TMQUtil.SqlExecuteClass.SelectList<ComDao.AttachmentEntity>(SqlName.Detail.GetAttachmentInfo, SqlName.SubDir, attachment, this.db);
            if (result != null)
            {
                //削除対象の件名添付が存在する場合、削除
                if (!new ComDao.AttachmentEntity().DeleteByKeyId(Const.Attachment.FunctionTypeId.Summary, Convert.ToInt64(summaryId), this.db))
                {
                    return false;
                }
            }
            if (maintenanceDivisionId == MaintenanceDivision.Failure.ToString())
            {
                //故障情報の場合

                Dao.AttachmentInfo failureAttachment = new Dao.AttachmentInfo();
                failureAttachment.FunctionTypeIdList = new List<int>()
                {
                    (int)Const.Attachment.FunctionTypeId.HistoryFailureDiagram, //故障分析情報タブ-略図添付
                    (int)Const.Attachment.FunctionTypeId.HistoryFailureAnalyze, //故障分析情報タブ-故障原因分析書添付
                    (int)Const.Attachment.FunctionTypeId.HistoryFailureFactDiagram, //故障分析情報(個別工場)タブ-略図添付
                    (int)Const.Attachment.FunctionTypeId.HistoryFailureFactAnalyze //故障分析情報(個別工場)タブ-故障原因分析書添付
                };
                //保全履歴故障情報IDを取得
                string historyFailureId = getValueByKeyName(ConductInfo.FormDetail.ControlId.FailureInfoIds[0], "history_failure_id");
                failureAttachment.KeyId = Convert.ToInt64(historyFailureId);
                //削除する添付情報を取得
                result = TMQUtil.SqlExecuteClass.SelectList<ComDao.AttachmentEntity>(SqlName.Detail.GetAttachmentInfo, SqlName.SubDir, failureAttachment, this.db);
                if (result != null)
                {
                    //削除対象の略図、故障原因分析書の添付情報が存在する場合、削除
                    bool returnFlg = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteAttachment, SqlName.SubDir, failureAttachment, this.db);
                    if (!returnFlg)
                    {
                        return false;
                    }
                }
            }
            //=========保全スケジュール詳細の更新=========
            ComDao.McMaintainanceScheduleDetailEntity detail = new ComDao.McMaintainanceScheduleDetailEntity();
            detail.SummaryId = Convert.ToInt64(summaryId);
            // 共通の更新日時などを設定
            bool chkUpd = int.TryParse(this.UserId, out int updUserId);
            setExecuteConditionByDataClassCommon<ComDao.McMaintainanceScheduleDetailEntity>(ref detail, DateTime.Now, updUserId, -1);
            //保全活動件名IDをクリアする
            TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateScheduleSummaryId, SqlName.SubDir, detail, this.db);

            return true;

            //SQL文取得＆削除処理
            bool deleteTable<T>(string ctrlId, string key, string sqlName)
                where T : new()
            {
                //キーより値を取得
                string value = getValueByKeyName(ctrlId, key);

                if (value != null && value != "")
                {
                    // 削除SQL文の取得
                    string sql = string.Empty;
                    if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out sql))
                    {
                        return false;
                    }
                    //削除処理
                    if (!deleteTargetCtrl<T>(ctrlId, sql))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// resultInfoDictionaryとキーより値を取得
        /// </summary>
        /// <param name="ctrlId">取得対象のコントロールID</param>
        /// <param name="key">KEY_NAMEの値</param>
        /// <returns>取得した値</returns>
        private string getValueByKeyName(string ctrlId, string key)
        {
            // 値を取得
            Dictionary<string, object> targetDictionary = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
            string value = getDictionaryKeyValue(targetDictionary, key);
            return value;
        }

        /// <summary>
        /// 保全履歴機器、保全履歴機器部位、保全履歴点検の削除
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool deleteMachineList()
        {
            string ctrlId = ConductInfo.FormDetail.ControlId.MachineList;
            // 対象のリストを取得
            List<Dictionary<string, object>> targetDictionary = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);

            // 排他チェック
            if (!checkExclusiveList(ctrlId, targetDictionary))
            {
                // 排他エラー
                return false;
            }

            foreach (var deleteRow in targetDictionary)
            {
                string sql;
                //保全履歴機器
                if (!deleteInspectionTable<ComDao.MaHistoryMachineEntity>(ctrlId, "history_machine_id", SqlName.Detail.DeleteHistoryMachine, deleteRow))
                {
                    return false;
                }
                //保全履歴機器部位
                if (!deleteInspectionTable<ComDao.MaHistoryInspectionSiteEntity>(ctrlId, "history_inspection_site_id", SqlName.Detail.DeleteHistoryInspectionSite, deleteRow))
                {
                    return false;
                }
                //保全履歴点検内容
                if (!deleteInspectionTable<ComDao.MaHistoryInspectionContentEntity>(ctrlId, "history_inspection_content_id", SqlName.Detail.DeleteHistoryInspectionContent, deleteRow))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 指定した削除SQLを実行
        /// </summary>
        /// <typeparam name="T">テーブルクラス</typeparam>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="key">項目キー名</param>
        /// <param name="sqlName">SQLファイル名</param>
        /// <param name="deleteRow">対象行</param>
        /// <returns>エラーの場合False</returns>
        private bool deleteInspectionTable<T>(string ctrlId, string key, string sqlName, Dictionary<string, object> deleteRow)
            where T : new()
        {
            string sql;
            //キーより値を取得
            string value = getValueByKeyName(ctrlId, key);
            if (value != null && value != "")
            {
                //サロゲートキーが設定されている場合、削除実行する

                //SQL取得
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out sql))
                {
                    return false;
                }
                T condition = new();
                SetDeleteConditionByDataClass(deleteRow, ctrlId, condition);
                //SQL実行
                int result = this.db.Regist(sql, condition);
                if (result < 0)
                {
                    // 削除エラー
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// アイテム拡張マスタから拡張データを取得する
        /// </summary>
        /// <param name="seq">連番</param>
        /// <param name="dataType">データタイプ</param>
        /// <param name="factoryId">工場ID</param>
        /// <returns>拡張データ</returns>
        private string getItemExData(short seq, short dataType, int? factoryId = 0)
        {
            string result = null;

            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = (int)Const.MsStructure.GroupId.Location;
            //連番
            param.Seq = seq;
            //構成アイテム、アイテム拡張マスタ情報取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            if (list != null)
            {
                //取得情報から拡張データを取得
                result = list.Where(x => x.FactoryId == factoryId).Select(x => x.ExData).FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// 指示検収表出力処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool outputFiles()
        {
            //長期計画の指示検収票出力処理とほぼ同じ

            //保全活動件名ID
            ComDao.MaSummaryEntity param = GetFormDataByCtrlId<ComDao.MaSummaryEntity>(ConductInfo.FormDetail.ControlId.DetailInfoIds[0], true);
            // 長期計画IDより添付ファイルの情報を取得
            List<string> fileInfos = TMQUtil.SqlExecuteClass.SelectList<string>(SqlName.Detail.GetOutputFileInfo, SqlName.SubDir, param, this.db);

            if (fileInfos == null || fileInfos.Count == 0 || fileInfos.Any(x => !string.IsNullOrEmpty(x)) == false)
            {
                // 取得結果なし or 取得結果が全て空
                // ファイル無しの場合、エラー
                this.MsgId = GetResMessage(ComRes.ID.ID941060001);
                return false;
            }

            //件名情報の取得
            Dao.detailSummaryInfo result = getSummary(param);
            // 工場ID
            var factoryId = result.FactoryId;
            // 件名(ファイル名に使用)
            var subject = result.Subject;

            // テンポラリフォルダのパスを取得
            string tempRootPath = getTempFolderPath();
            tempRootPath = addPath(tempRootPath, factoryId.ToString()); // 工場IDのフォルダを追加
            // 一時的に作成するフォルダ(30桁ランダム)
            string tempNewFolderName = TMQUtil.GetRandomName(30);
            // 一時作成フォルダのパス
            string tempNewFolderPath = addPath(tempRootPath, tempNewFolderName);

            // テンポラリフォルダ作成
            Directory.CreateDirectory(tempNewFolderPath);
            foreach (string fileInfoChar in fileInfos)
            {
                // テンポラリフォルダに添付ファイルをコピーする
                createTempFolder(fileInfoChar, tempNewFolderPath);
            }

            // 作成するZIPファイルのパス
            string zipFilePath = tempNewFolderPath + CommonConstants.REPORT.EXTENSION.ZIP_FILE;
            // ダウンロードするZIPファイルの名前(とりあえず件名.zip)
            string zipFileName = subject + CommonConstants.REPORT.EXTENSION.ZIP_FILE;
            // zipファイル名が有効か判定、無効なら日時に変更
            zipFileName = getNewDownloadFilePath(tempRootPath, zipFileName);

            // ファイルダウンロード
            if (!SetDownloadZip(zipFilePath, tempNewFolderPath, zipFileName))
            {
                return false;
            }

            // ZIPファイルとテンポラリフォルダの削除
            File.Delete(zipFilePath);
            Directory.Delete(tempNewFolderPath, true);

            return true;

            Dao.detailSummaryInfo getSummary(ComDao.MaSummaryEntity param)
            {
                // 検索実行
                Dao.detailSummaryInfo result = TMQUtil.SqlExecuteClass.SelectEntity<Dao.detailSummaryInfo>(SqlName.Detail.GetSummaryInfo, SqlName.SubDir, param, this.db);

                IList<Dao.detailSummaryInfo> results = new List<Dao.detailSummaryInfo>();
                results.Add(result);
                // 階層情報を設定
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.detailSummaryInfo>(ref results, new List<StructureType> { StructureType.Location }, this.db, this.LanguageId, true);

                return result;
            }

            // フォルダパスを作成する処理
            string addPath(string source, string add)
            {
                return Path.Combine(source, add);
            }

            // テンポラリフォルダのパスを取得
            string getTempFolderPath()
            {
                // 条件
                TMQUtil.StructureItemEx.StructureItemExInfo cond = new();
                cond.StructureGroupId = (int)Const.MsStructure.GroupId.TempFolderPath;
                cond.Seq = 1;
                // 取得
                var result = TMQUtil.StructureItemEx.GetStructureItemExData(cond, this.db);
                // 必ず1レコード
                string tempPath = result[0].ExData;

                return tempPath;

            }

            // ファイル情報に存在するファイルをテンポラリフォルダへコピーする処理
            // fileInfoChar ファイル情報の文字列、fileName(A)|filePath(A)||fileName(B)|filePath(B)||…||
            // tempFolder テンポラリフォルダのパス
            void createTempFolder(string fileInfoChar, string tempFolder)
            {
                // ファイルごとの区切り(||)で分割しリストへ
                List<string> fileInfoList = fileInfoChar.Split(Const.FileInfoSplit.File).ToList();
                foreach (string fileInfo in fileInfoList)
                {
                    if (string.IsNullOrEmpty(fileInfo)) { break; } // 空(最後の行)なら終了
                    // ファイルパスとファイル名の区切り(|)で分割
                    List<string> file = fileInfo.Split(Const.FileInfoSplit.Path).ToList();
                    // ファイルパス
                    string filePath = file[1];
                    // 添付ファイルの取得
                    if (File.Exists(filePath))
                    {
                        // 区切りのファイル名は、画面に表示されるファイル名。ファイルパスのファイル名が実際のファイル名となる。同名ファイルがある場合、text.txtとtext(1).txtのように異なる場合がある
                        string fileName = Path.GetFileName(filePath);
                        // ある場合、テンポラリフォルダへコピー
                        File.Copy(filePath, getCopyFilePath(tempFolder, fileName));
                    }
                }
            }

            // テンポラリフォルダにコピーする際のファイルパスを取得する処理
            // 同名ファイルの場合エラーになるため、test(1).txtのように変更
            // copyFolder コピー先フォルダのパス
            // fileName コピー元ファイルの名前
            // return コピーする際に使用するファイルパス
            string getCopyFilePath(string copyFolder, string fileName)
            {
                int i = 1; // 付与する数字
                string targetPath = addPath(copyFolder, fileName); // コピー先ファイルパス

                // ファイルが存在する限り繰り返し→存在しなくなったら終了
                while (File.Exists(targetPath))
                {
                    string newFileName = fileName.Replace(".", $"({i++})."); // text.txt → text(1).txt のように変換
                    targetPath = addPath(copyFolder, newFileName); // 新しいコピー先ファイルパスへ設定
                }
                // このファイルパスは存在しないので、これを使用する
                return targetPath;
            }

            // 件名をファイル名とすると、件名の内容によりエラーとなるので、その場合は日時をファイル名とする
            // ファイルに使用できない文字(\/:?<>|)
            // ファイルパスが256文字超(OSによる)
            // tempRootPath 一時フォルダルートパス　ダウンロード時にここにファイルが作成される
            // zipFileName zipファイル名
            // return 実際に使用するzipファイル名 引数のzipファイル名でエラーとなるときに日時に置換
            string getNewDownloadFilePath(string tempRootPath, string zipFileName)
            {
                string newFilePath = zipFileName;
                // 実際にダウンロードされるファイルのパス
                string downloadFilePath = Path.Combine(tempRootPath, zipFileName);
                try
                {
                    // ファイルを作成し、削除
                    System.IO.File.Create(downloadFilePath).Close();
                    File.Delete(downloadFilePath);
                }
                catch (Exception ex)
                {
                    // 使用できないファイル名なら例外となるので、新たなファイル名を設定
                    newFilePath = string.Format("{0:yyyyMMddHHmmssfff}{1}", DateTime.Now, CommonConstants.REPORT.EXTENSION.ZIP_FILE);
                }

                return newFilePath;
            }
        }
    }
}
