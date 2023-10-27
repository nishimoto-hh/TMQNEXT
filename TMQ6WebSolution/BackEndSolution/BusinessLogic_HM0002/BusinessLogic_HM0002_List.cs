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
using Dao = BusinessLogic_HM0002.BusinessLogicDataClass_HM0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConst = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_HM0002
{
    /// <summary>
    /// 変更管理 長期計画(一覧)
    /// </summary>
    public partial class BusinessLogic_HM0002 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 自分の申請のみ表示
        /// </summary>
        private const int IsDispOnlyMySubject = 1;
        #endregion

        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="isInit">初期表示の場合はTrue</param>
        /// <returns>エラーの場合False</returns>
        private bool searchList(bool isInit)
        {
            // 初期検索時は「自分の申請のみ表示」をチェック状態にする
            if (!getSearchCondition(isInit, out int dispOnlyMySubject))
            {
                return false;
            }

            // 「自分の申請のみ表示」がチェックされていたらSQLの該当箇所をアンコメント
            List<string> listUnComment = new();
            if (dispOnlyMySubject == IsDispOnlyMySubject)
            {
                listUnComment.Add("DispOnlyMySubject");
            }

            // 一覧のページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);
            // 年度開始月
            int monthStartNendo = getYearStartMonth();
            // システム年度初期化処理
            SetSysFiscalYear<TMQDao.ScheduleList.Condition>(ConductInfo.FormList.ControlId.ScheduleCondition, monthStartNendo);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.List.SubDir, SqlName.List.GetHistoryList, out string baseSql);
            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.List.SubDir, SqlName.List.GetHistoryList, out string withSql, listUnComment);
            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
            {
                return false;
            }
            // 検索条件を設定
            whereParam.LanguageId = this.LanguageId; // 言語ID
            whereParam.UserId = this.UserId;         // ログインユーザーID

            //// SQL、WHERE句、WITH句より件数取得SQLを作成
            //string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);
            //// 総件数を取得
            //int cnt = db.GetCount(executeSql, whereParam);
            //// 総件数のチェック
            //if (!CheckSearchTotalCount(cnt, pageInfo))
            //{
            //    SetSearchResultsByDataClass<Dao.ListSearchResult>(pageInfo, null, cnt, isDetailConditionApplied);
            //    return false;
            //}

            // 一覧検索SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY subject");
            // 一覧検索実行
            IList<Dao.ListSearchResult> results = db.GetListByDataClass<Dao.ListSearchResult>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定(変更管理データ・トランザクションデータ どちらも設定する)
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.ListSearchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job, StructureType.OldLocation, StructureType.OldJob }, this.db, this.LanguageId);

            // 変更があった項目を取得
            TMQUtil.HistoryManagement.setValueChangedItem<Dao.ListSearchResult>(results);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.ListSearchResult>(pageInfo, results, results.Count, isDetailConditionApplied))
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
            TMQUtil.GetFixedSqlStatement(SqlName.List.SubDir, SqlName.List.GetHistoryListSchedule, out string sqlSchedule);
            IList<TMQDao.ScheduleList.Get> scheduleList = db.GetListByDataClass<TMQDao.ScheduleList.Get>(sqlSchedule, cond);
            scheduleList = scheduleList.Where(x => keyIdList.Contains(x.KeyId)).ToList(); // 一覧と紐づくものだけを表示

            // 取得したデータを画面表示用に変換、マークなど取得
            TMQUtil.ScheduleListConverterNoRank listSchedule = new();
            List<TMQDao.ScheduleList.Display> scheduleDisplayList = listSchedule.Execute(scheduleList, cond, monthStartNendo, true, this.db, getScheduleLinkInfo());

            // 画面設定用データに変換
            Dictionary<string, Dictionary<string, string>> setScheduleData = TMQUtil.ScheduleListUtil.ConvertDictionaryAddData(scheduleDisplayList, cond);

            // 画面に設定
            SetScheduleDataToResult(setScheduleData, ConductInfo.FormList.ControlId.List);

            return true;
        }

        /// <summary>
        /// 検索条件を取得(初期検索時は「自分の申請のみ表示」をチェック状態にする)
        /// </summary>
        /// <param name="isInit">初期表示の場合はTrue</param>
        /// <param name="outDispOnlyMySubject">チェック状態</param>
        /// <returns>エラーの場合False</returns>
        private bool getSearchCondition(bool isInit, out int outDispOnlyMySubject)
        {
            outDispOnlyMySubject = new();

            // 検索条件を取得する一覧のコントロールID
            string ctrlId = ConductInfo.FormList.ControlId.MySubjectCondition;

            // 検索条件初期化
            Dao.searchCondition searchCondition = new();

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // 初期検索か判定
            if (isInit)
            {
                // 初期値を設定
                searchCondition.DispOnlyMySubject = IsDispOnlyMySubject; // 自分の申請のみ表示
                outDispOnlyMySubject = IsDispOnlyMySubject;
            }
            else
            {
                // コントロールIDにより画面の項目(一覧)を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);
                var mappingInfo = getResultMappingInfo(ctrlId);

                // 数値に変換(変換できない場合はエラー)
                if (!int.TryParse(condition[getValNoByParam(mappingInfo, "DispOnlyMySubject")].ToString(), out int dispOnlyMySubject))
                {
                    return false;
                }

                // 取得した値を検索条件に設定
                searchCondition.DispOnlyMySubject = dispOnlyMySubject; // 自分の申請のみ表示
                outDispOnlyMySubject = dispOnlyMySubject;
            }

            //承認系ボタン表示制御
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
            searchCondition.IsApprovalUser = historyManagement.IsApprovalUser();

            if (!SetSearchResultsByDataClass<Dao.searchCondition>(pageInfo, new List<Dao.searchCondition> { searchCondition }, 1))
            {
                return false;
            }

            return true;

            string getValNoByParam(MappingInfo info, string keyName)
            {
                // 項目キー名と一致する項目番号を返す
                return info.Value.First(x => x.ParamName.Equals(keyName)).ValName;
            }
        }

        /// <summary>
        /// 一括承認・一括否認
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool registFormList()
        {
            // 選択行取得
            var selectedList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormList.ControlId.List);

            // 排他チェック
            if (!checkExclusiveList(ConductInfo.FormList.ControlId.List, selectedList))
            {
                return false;
            }

            // ボタンコントロールに応じて登録する申請状況(拡張項目)を設定
            TMQConst.MsStructure.StructureId.ApplicationStatus applicationStatus;
            switch (this.CtrlId)
            {
                case ConductInfo.FormList.ButtonId.ApprovalAll: // 一括承認
                    applicationStatus = TMQConst.MsStructure.StructureId.ApplicationStatus.Approved; // 承認済
                    break;
                case ConductInfo.FormList.ButtonId.DenialAll: // 一括否認
                    applicationStatus = TMQConst.MsStructure.StructureId.ApplicationStatus.Return; // 差戻中
                    break;
                default:
                    return false;
            }

            List<ComDao.HmHistoryManagementEntity> conditionList = new();
            foreach (var selectedRow in selectedList)
            {
                // 登録情報を作成
                ComDao.HmHistoryManagementEntity condition = new();
                SetExecuteConditionByDataClass(selectedRow, ConductInfo.FormList.ControlId.List, condition, DateTime.Now, this.UserId, this.UserId, new List<string> { "HistoryManagementId" });
                conditionList.Add(condition);
            }

            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
            // 入力チェック
            if (historyManagement.isErrorBeforeUpdateApplicationStatus(conditionList, this.CtrlId == ConductInfo.FormList.ButtonId.ApprovalAll, out string[] errMsg))
            {
                // エラーメッセージを設定
                this.MsgId = GetResMessage(errMsg);
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            foreach (ComDao.HmHistoryManagementEntity condition in conditionList)
            {
                // 変更管理テーブルの申請状況更新処理
                if (!historyManagement.UpdateApplicationStatus(condition, applicationStatus))
                {
                    return false;
                }

                if (this.CtrlId == ConductInfo.FormList.ButtonId.ApprovalAll)
                {
                    //一括承認の場合、変更管理の内容をトランザクションテーブルへ反映
                    if (!approvalHistory(condition))
                    {
                        return false;
                    }
                }
            }

            // 一覧の再検索
            if (!searchList(false))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 承認時、変更管理の内容をトランザクションテーブルへ反映
        /// </summary>
        /// <param name="condition">選択行の情報</param>
        /// <returns>エラーの場合False</returns>
        private bool approvalHistory(ComDao.HmHistoryManagementEntity condition)
        {
            //変更管理詳細の情報を取得
            List<ComDao.HmMcManagementStandardsContentEntity> list = TMQUtil.SqlExecuteClass.SelectList<ComDao.HmMcManagementStandardsContentEntity>(SqlName.List.GetHistoryManagementDetail, SqlName.List.SubDir, condition, this.db);
            if (list == null || list.Count == 0)
            {
                return false;
            }
            // 登録更新ユーザ
            int userId = int.Parse(this.UserId);
            // システム日時
            DateTime now = DateTime.Now;
            // トランザクションテーブルへ反映
            foreach (ComDao.HmMcManagementStandardsContentEntity detail in list)
            {
                switch (detail.ExecutionDivision)
                {
                    case ExecutionDivision.NewLongPlan: //長期計画の新規登録（複写）
                        //長計件名変更管理の内容を長計件名(トランザクションテーブル)に反映
                        if (!registLongPlan(detail, SqlName.List.InsertLongPlan, SqlName.List.SubDir))
                        {
                            return false;
                        }
                        break;
                    case ExecutionDivision.UpdateLongPlan: //長期計画の修正
                        //長計件名変更管理の内容を長計件名(トランザクションテーブル)に反映
                        if (!registLongPlan(detail, SqlName.Common.UpdateLongPlan, SqlName.SubDirLongPlan))
                        {
                            return false;
                        }

                        break;
                    case ExecutionDivision.DeleteLongPlan: //長期計画の削除
                        //長計件名と紐づく情報の更新
                        if (!deleteLongPlan(detail))
                        {
                            return false;
                        }
                        break;
                    case ExecutionDivision.AddContent: //保全情報一覧の追加
                        // 機器別管理基準内容更新
                        ComDao.McManagementStandardsContentEntity content = new();
                        content.ManagementStandardsContentId = detail.ManagementStandardsContentId;
                        content.LongPlanId = detail.LongPlanId;
                        setExecuteConditionByDataClassCommon<ComDao.McManagementStandardsContentEntity>(ref content, now, userId, userId);
                        bool result = TMQUtil.SqlExecuteClass.Regist(SqlName.SelectStandards.UpdateContent, SqlName.SelectStandards.SubDirLongPlan, content, this.db);
                        if (!result)
                        {
                            return false;
                        }
                        break;
                    case ExecutionDivision.DeleteContent: //保全情報一覧の削除
                        // 機器別管理基準内容更新(長計件名IDをNULLに更新)
                        ComDao.McManagementStandardsContentEntity update = new();
                        update.ManagementStandardsContentId = detail.ManagementStandardsContentId;
                        setExecuteConditionByDataClassCommon<ComDao.McManagementStandardsContentEntity>(ref update, now, userId, userId);
                        if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteRow, SqlName.Detail.SubDirLongPlan, update, this.db))
                        {
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;

            //長計件名変更管理の内容を長計件名(トランザクションテーブル)に反映
            bool registLongPlan(ComDao.HmMcManagementStandardsContentEntity detail, string sqlName, string subDir)
            {
                // 長計件名変更管理を取得
                ComDao.HmLnLongPlanEntity longPlan = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.HmLnLongPlanEntity>(SqlName.List.GetHmLongPlan, SqlName.List.SubDir, detail, this.db);
                setExecuteConditionByDataClassCommon(ref longPlan, now, userId, userId);
                // 長計件名変更管理の内容を長計件名(トランザクションテーブル)に反映
                bool result = TMQUtil.SqlExecuteClass.Regist(sqlName, subDir, longPlan, this.db);
                return result;
            }

            //長計件名と紐づく情報の更新
            bool deleteLongPlan(ComDao.HmMcManagementStandardsContentEntity detail)
            {
                // 長計件名の削除
                if (!new ComDao.LnLongPlanEntity().DeleteByPrimaryKey(detail.LongPlanId ?? -1, this.db))
                {
                    return false;
                }

                // 機器別管理基準内容更新
                // 更新条件をヘッダの隠し項目の長計件名IDより取得
                var headerDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.Hide);
                ComDao.LnLongPlanEntity update = new();
                SetExecuteConditionByDataClass<ComDao.LnLongPlanEntity>(headerDic, ConductInfo.FormDetail.ControlId.Hide, update, now, this.UserId);
                // 更新処理(更新レコードが無い場合も処理継続)
                TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteAll, SqlName.Detail.SubDirLongPlan, update, this.db);

                // 長期計画が紐づいた保全活動件名を更新（長計件名IDをNULLに更新）
                TMQUtil.SqlExecuteClass.Regist(SqlName.List.UpdateSummary, SqlName.List.SubDir, update, this.db);

                // 添付情報削除
                // 削除対象の添付情報のキーIDを取得
                List<ComDao.AttachmentEntity> attachmentList = TMQUtil.SqlExecuteClass.SelectList<ComDao.AttachmentEntity>(SqlName.Detail.GetAttachmentByLongPlanId, SqlName.Detail.SubDirLongPlan, detail, this.db);
                if (attachmentList == null || attachmentList.Count == 0)
                {
                    // 添付情報が存在しない場合は処理を行わない
                    return true;
                }
                // キーIDのみのリストにして重複を排除
                var keyIdList = attachmentList.Select(x => x.KeyId).Distinct().ToList();
                foreach (var keyId in keyIdList)
                {
                    // キーIDで繰り返し削除
                    if (!new ComDao.AttachmentEntity().DeleteByKeyId(TMQConst.Attachment.FunctionTypeId.LongPlan, keyId, this.db))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
