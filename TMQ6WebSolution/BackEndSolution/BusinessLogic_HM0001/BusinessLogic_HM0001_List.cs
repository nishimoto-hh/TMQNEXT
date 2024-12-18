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
using static CommonWebTemplate.Models.Common.COM_CTRL_CONSTANTS;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_HM0001.BusinessLogicDataClass_HM0001;
using FunctionTypeId = CommonTMQUtil.CommonTMQConstants.Attachment.FunctionTypeId;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_HM0001
{
    /// <summary>
    /// 変更管理 機器台帳 一覧画面
    /// </summary>
    public partial class BusinessLogic_HM0001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 自分の件名のみ表示
        /// </summary>
        private const int IsDispOnlyMySubject = 1;
        #endregion

        #region 検索
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="isInit">初期表示の場合はTrue</param>
        /// <param name="isReSearch">一括承認/否認後の再検索の場合はTrue</param>
        /// <returns>エラーの場合False</returns>
        private bool searchList(bool isInit, bool isReSearch = false)
        {
            // 初期検索時は「自分の件名のみ表示」をチェック状態にする
            if (!getSearchCondition(isInit, out int dispOnlyMySubject))
            {
                return false;
            }

            // 警告コメントを設定する
            setWarningComment();

            // 一覧のページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            // メニューから選択された際の初期検索は行わない
            if (isInit)
            {
                // 検索結果の設定
                if (SetSearchResultsByDataClassForList<Dao.searchResult>(pageInfo, new List<Dao.searchResult>(), 0))
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                }
                return true;
            }

            // 「自分の件名のみ表示」がチェックされていたらSQLの該当箇所をアンコメント
            List<string> listUnComment = new();
            if (dispOnlyMySubject == IsDispOnlyMySubject)
            {
                listUnComment.Add("DispOnlyMySubject");
            }

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.List.GetHistoryMachineList, out string baseSql);

            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.List.GetHistoryMachineList, out string withSql, listUnComment);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
            {
                return false;
            }

            // 翻訳の一時テーブルを作成
            createTranslationTempTbl();

            // 検索条件を設定
            whereParam.LanguageId = this.LanguageId; // 言語ID
            whereParam.UserId = this.UserId;         // ログインユーザーID

            // SQL、WHERE句、WITH句より件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);

            // 総件数を取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.List.GetCountHistoryMachineList, out string cntSql, listUnComment);
            cntSql += whereParam.CountSqlWhere;
            int cnt = db.GetCount(cntSql, whereParam);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                // 一括承認/否認後の再検索の場合はデータが0件でもエラーとしない
                if (isReSearch)
                {
                    return true;
                }

                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「該当データがありません。」
                this.MsgId = GetResMessage(CommonResources.ID.ID941060001);
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, null, cnt, isDetailConditionApplied);
                return false;
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql, isDetailConditionApplied, pageInfo.SelectMaxCnt);
            var selectSql = new StringBuilder(executeSql);

            // 機器番号の昇順
            selectSql.AppendLine("ORDER BY machine_no");

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                // 一括承認/否認後の再検索の場合はデータが0件でもエラーとしない
                if (isReSearch)
                {
                    return true;
                }

                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「該当データがありません。」
                this.MsgId = GetResMessage(CommonResources.ID.ID941060001);
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, null, 0, isDetailConditionApplied);
                return false;
            }

            // 検索結果の設定
            if (!SetSearchResultsByDataClassForList<Dao.searchResult>(pageInfo, results, cnt, isDetailConditionApplied))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;

            // 警告コメントを設定する
            void setWarningComment()
            {
                // 一覧のページ情報取得
                var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.WarningComment, this.pageInfoList);

                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.List.GetWarningComment, out string commentSql);

                // 一覧検索実行
                IList<Dao.WarningComment> comment = db.GetListByDataClass<Dao.WarningComment>(commentSql.ToString(), new { LanguageId = this.LanguageId });

                // 検索結果の設定
                SetFormByDataClass(ConductInfo.FormList.ControlId.WarningComment, comment);
            }
        }

        /// <summary>
        /// 検索条件を取得(初期検索時は「自分の件名のみ表示」をチェック状態にする)
        /// </summary>
        /// <param name="isInit">初期表示の場合はTrue</param>
        /// <param name="outDispOnlyMySubject">チェック状態</param>
        /// <returns>エラーの場合False</returns>
        private bool getSearchCondition(bool isInit, out int outDispOnlyMySubject)
        {
            outDispOnlyMySubject = new();

            // 検索条件を取得する一覧のコントロールID
            string ctrlId = ConductInfo.FormList.ControlId.Condition;

            // 検索条件初期化
            Dao.searchCondition searchCondition = new();

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // 初期検索か判定
            if (isInit)
            {
                // トップ画面から遷移した場合、遷移元のリンクに応じた「自分の件名のみ表示」のチェック状態を使用する
                TMQUtil.HistoryManagement.IsDispOnlyMySubjectFromTop(ref this.searchConditionDictionary, out bool isTop, out bool isDispOnlyMySubject);
                // チェック状態を設定
                // トップ画面からの場合、戻り値に応じて設定。そうでない場合、チェック状態を設定
                outDispOnlyMySubject = isTop ? (isDispOnlyMySubject ? IsDispOnlyMySubject : 0) : IsDispOnlyMySubject;
                searchCondition.DispOnlyMySubject = outDispOnlyMySubject;
            }
            else
            {
                // コントロールIDにより画面の項目(一覧)を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);
                var mappingInfo = getResultMappingInfo(ctrlId);

                // 数値に変換(変換できない場合はエラー)
                if (!int.TryParse(condition[mappingInfo.getValName("DispOnlyMySubject")].ToString(), out int dispOnlyMySubject))
                {
                    return false;
                }

                // 取得した値を検索条件に設定
                outDispOnlyMySubject = dispOnlyMySubject; // 自分の件名のみ表示
                searchCondition.DispOnlyMySubject = dispOnlyMySubject;
            }

            // 承認系ボタン表示制御
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
            searchCondition.IsApprovalUser = historyManagement.IsApprovalUser();

            // 画面項目に設定
            if (!SetSearchResultsByDataClass<Dao.searchCondition>(pageInfo, new List<Dao.searchCondition> { searchCondition }, 1))
            {
                return false;
            }

            return true;

        }
        #endregion

        #region 登録
        /// <summary>
        /// 一括承認・一括否認
        /// </summary>
        /// <returns>エラーの場合False</returns>
        public int registFormList()
        {
            // 選択行取得
            var selectedList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormList.ControlId.List);

            // 排他チェック
            if (!checkExclusiveList(ConductInfo.FormList.ControlId.List, selectedList))
            {
                return ComConsts.RETURN_RESULT.NG;
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
                    return ComConsts.RETURN_RESULT.NG;
            }

            DateTime now = DateTime.Now;
            List<ComDao.HmHistoryManagementEntity> conditionList = new();
            foreach (var selectedRow in selectedList)
            {
                // 登録情報を作成
                ComDao.HmHistoryManagementEntity condition = new();
                SetExecuteConditionByDataClass(selectedRow, ConductInfo.FormList.ControlId.List, condition, now, this.UserId, this.UserId, new List<string> { "HistoryManagementId" });
                conditionList.Add(condition);
            }

            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
            // 入力チェック
            if (historyManagement.isErrorBeforeUpdateApplicationStatus(conditionList, this.CtrlId == ConductInfo.FormList.ButtonId.ApprovalAll, out string[] errMsg))
            {
                // エラーメッセージを設定
                this.MsgId = GetResMessage(errMsg);
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;

            }
            foreach (var condition in conditionList)
            {
                // 申請状況更新処理
                if (!historyManagement.UpdateApplicationStatus(condition, applicationStatus))
                {
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 一括承認の場合はトランザクションテーブルに反映する
                if (this.CtrlId == ConductInfo.FormList.ButtonId.ApprovalAll)
                {
                    // 承認処理
                    if (!approval(condition, now, int.Parse(this.UserId)))
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                }
            }

            // 一覧の再検索
            searchList(false, true);

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion
    }
}
