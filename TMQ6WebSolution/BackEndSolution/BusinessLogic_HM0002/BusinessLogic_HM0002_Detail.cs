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
using SchedulePlanContent = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.SchedulePlanContent;

namespace BusinessLogic_HM0002
{
    /// <summary>
    /// 変更管理 長期計画(参照画面)
    /// </summary>
    public partial class BusinessLogic_HM0002 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 参照画面の表示種類
        /// </summary>
        private enum DetailDispType
        {
            /// <summary>初期表示</summary>
            Init,
            /// <summary>検索(再表示ボタン)</summary>
            Search,
            /// <summary>再表示(ボタン処理後の再表示)</summary>
            Redisplay,
            /// <summary>新規登録後</summary>
            AfterRegist,
            /// <summary>再表示(ボタン処理後の再表示)・検索条件より非表示項目を取得(画面レイアウトの関係で実装上の区分)</summary>
            RedisplaySearch
        }

        /// <summary>
        /// 参照画面初期化処理
        /// </summary>
        /// <param name="type">参照画面の表示タイプ</param>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <returns>エラーの場合False</returns>
        private bool initDetail(DetailDispType type, long historyManagementId = -1)
        {
            // 表示タイプの再設定
            type = updateType(type);
            // キー情報取得元のコントロールIDを取得
            string ctrlId = getCtrlIdForKeyInfo(type);
            // 長期計画IDを取得
            Dao.detailSearchCondition param = getParam(ctrlId, type == DetailDispType.Redisplay, true, historyManagementId);

            // 初期化処理呼出
            // 一覧画面の一覧より取得した情報で参照画面の項目に値を設定する
            initFormByLongPlanId(param, getResultMappingInfoByGrpNo(ConductInfo.FormDetail.HeaderGroupNo).CtrlIdList, out bool isDisplayMaintainanceKind, out int factoryId, true, ctrlId == ConductInfo.FormList.ControlId.List);
            // スケジュールのシステム年度設定処理
            setScheduleYear(type, factoryId);
            // 一覧の設定
            setDetailList(isDisplayMaintainanceKind, factoryId);

            return true;

            // 画面タイプの再設定
            DetailDispType updateType(DetailDispType type)
            {
                if (type == DetailDispType.AfterRegist)
                {
                    // 修正画面で登録押下時と機器別管理基準選択画面で登録押下時の判定が出来ない
                    // 修正画面で登録押下時は修正画面の非表示エリア(BODY_050_00_LST_2)がSearchConditionDictionaryに設定されているため、それにより判定する
                    var fromEditParam = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormEdit.ControlId.Hide);
                    if (fromEditParam == null || fromEditParam.Count == 0)
                    {
                        // 機器別管理基準選択画面より登録で表示した場合は、戻ると同じ
                        type = DetailDispType.Search;
                    }
                }
                return type;
            }
            // 画面タイプに応じて、キー情報取得元の画面コントロールIDの取得
            string getCtrlIdForKeyInfo(DetailDispType type)
            {
                string ctrlId = string.Empty;
                switch (type)
                {
                    case DetailDispType.Init:
                        // 初期表示の場合、一覧画面の一覧より取得(選択された列)
                        ctrlId = ConductInfo.FormList.ControlId.List;
                        break;
                    case DetailDispType.AfterRegist:
                        // 新規登録後の場合、詳細編集画面の非表示項目より取得(新規採番したキー)
                        ctrlId = ConductInfo.FormEdit.ControlId.Hide;
                        break;
                    default:
                        // それ以外の場合(再表示など)、この画面の非表示項目より取得
                        ctrlId = ConductInfo.FormDetail.ControlId.Hide;
                        break;
                }
                return ctrlId;
            }
            // スケジュールのシステム年度の設定
            void setScheduleYear(DetailDispType type, int factoryId)
            {
                if (type == DetailDispType.Init || type == DetailDispType.AfterRegist)
                {
                    // 年度開始月の取得
                    int monthStartNendo = getYearStartMonth(factoryId);
                    // 初期表示の場合、システム年度初期化処理
                    SetSysFiscalYear<Dao.Detail.Condition>(ConductInfo.FormDetail.ControlId.ScheduleCondition, monthStartNendo);
                }
            }

            // 一覧の取得・設定
            void setDetailList(bool isDisplayMaintainanceKind, int factoryId)
            {
                SchedulePlanContent contentType = getSchedulePlanContentDetail();
                // 画面下部の一覧の値を取得
                var list = getMaintainanceList(param.LongPlanId, factoryId, param.ProcessMode, param.HistoryManagementId, isDisplayMaintainanceKind, contentType);
                // ページ情報取得
                string listCtrlId = getDetailListCtrlId(isDisplayMaintainanceKind);
                var pageInfo = GetPageInfo(listCtrlId, this.pageInfoList);
                // 画面にセット
                SetSearchResultsByDataClass<Dao.Detail.List>(pageInfo, list, list.Count);
                // スケジュール情報のセット
                setSchedule(listCtrlId, true, param.LongPlanId, factoryId, param.ProcessMode, param.HistoryManagementId, contentType);
            }
        }

        /// <summary>
        /// 参照画面　計画内容コンボの値を取得
        /// </summary>
        /// <returns>計画内容コンボの値</returns>
        private SchedulePlanContent getSchedulePlanContentDetail()
        {
            // 表示条件 保全情報一覧 計画内容コンボの値を取得
            var conditionDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.Condition);
            Dao.Detail.Condition condition = new();
            SetDataClassFromDictionary(conditionDic, ConductInfo.FormDetail.ControlId.Condition, condition);
            int value = condition.SchedulePlanContentExtension;
            if (value > 0)
            {
                // 値が取得できる場合
                // intをEnumに変換して返す
                return ComUtil.IntToEnum<SchedulePlanContent>(value);
            }
            // 値が取得できない場合、保全活動を返す
            // 別タブ表示の場合、遷移前処理でセットできないためここでセット
            // 画面側はJavaScriptで設定していて、こちらはSQLの検索条件
            return SchedulePlanContent.Maintainance;
        }

        /// <summary>
        /// 参照画面　ヘッダの内容を取得
        /// </summary>
        /// <param name="isResult">省略時はTrue ResultInfoDictionaryより取得する場合True、SearchConditionDictionaryより取得する場合False</param>
        /// <returns>ヘッダのデータクラス</returns>
        private Dao.ListSearchResult getHeaderDetailForm(bool isResult = true)
        {
            return GetFormDataByCtrlId<Dao.ListSearchResult>(ConductInfo.FormDetail.ControlId.Hide, isResult);
        }

        /// <summary>
        /// 参照画面　ヘッダの隠し項目より、一覧の表示状態を取得する
        /// </summary>
        /// <param name="isResult">省略時はTrue ResultInfoDictionaryより取得する場合True、SearchConditionDictionaryより取得する場合False</param>
        /// <returns>保全情報一覧(点検種別)が表示されている場合True</returns>
        private bool getIsDisplayMaintainanceKindByDetail(bool isResult = true)
        {
            Dao.ListSearchResult headerData = getHeaderDetailForm(isResult);
            // 保全情報一覧(点検種別)の表示フラグを返す
            return headerData.IsDisplayMaintainanceKind;
        }

        /// <summary>
        /// 保全情報一覧の表示フラグより対応するコントロールIDを取得する
        /// </summary>
        /// <param name="isDisplayMaintainanceKind">保全情報一覧(点検種別)の場合True</param>
        /// <returns>対応する一覧のコントロールID</returns>
        private string getDetailListCtrlId(bool isDisplayMaintainanceKind)
        {
            // 保全情報一覧(点検種別)または保全情報一覧のコントロールIDを返す
            return isDisplayMaintainanceKind ? ConductInfo.FormDetail.ControlId.ListCheck : ConductInfo.FormDetail.ControlId.List;
        }

        /// <summary>
        /// 行削除処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool deleteDetailRow()
        {
            // ヘッダの内容
            Dao.ListSearchResult header = getHeaderDetailForm(false);
            if (header.HistoryManagementId <= 0)
            {
                //トランザクションモード
                header.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction;

                //「承認済」以外の変更管理が紐づいている場合、エラー
                if (!checkHistoryManagement(header.LongPlanId))
                {
                    return false;
                }
            }
            else
            {
                //変更管理モード
                header.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.history;

                // 変更管理の排他チェック
                Dictionary<string, object> target = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.Hide);
                if (!checkExclusiveSingleForTable(ConductInfo.FormDetail.ControlId.Hide, new List<string> { new ComDao.HmHistoryManagementEntity().TableName }, target))
                {
                    return false;
                }
            }

            // 表示中の一覧のコントロールID
            bool isDisplayMaintainanceKind = getIsDisplayMaintainanceKindByDetail(false);
            string listCtrlId = getDetailListCtrlId(isDisplayMaintainanceKind);
            //選択行
            var deleteList = getSelectedRowsByList(this.resultInfoDictionary, listCtrlId);
            //各行の排他チェック、入力チェック
            if (isErrorDeleteRow(listCtrlId, deleteList))
            {
                return false;
            }

            // 行削除
            if (!registDeleteRowData(header, deleteList))
            {
                return false;
            }

            // 再検索
            if (!initDetail(DetailDispType.RedisplaySearch, header.HistoryManagementId))
            {
                return false;
            }

            return true;

            // 行削除エラーチェック処理
            bool isErrorDeleteRow(string listCtrlId, List<Dictionary<string, object>> deleteList)
            {
                // 一覧情報
                IList<Dao.Detail.List> list = new List<Dao.Detail.List>();

                // 削除対象の行を繰り返しチェック
                foreach (var deleteRow in deleteList)
                {
                    Dao.Detail.List row = new();
                    SetDataClassFromDictionary(deleteRow, listCtrlId, row);

                    // 削除対象の行をチェック
                    if (row.HmManagementStandardsContentId == null || row.HmManagementStandardsContentId == 0)
                    {
                        // トランザクションのデータを表示している行の排他チェック(変更管理が紐づいている行は、変更管理の排他チェック済)

                        if (list.Count == 0)
                        {
                            // 一覧情報の再取得(1回のみ)
                            Dao.detailSearchCondition longPlanEntity = new();
                            longPlanEntity.LongPlanId = row.LongPlanId;
                            longPlanEntity.ProcessMode = header.ProcessMode;
                            longPlanEntity.HistoryManagementId = header.HistoryManagementId;
                            var longPanInfo = getLongPlanInfo(longPlanEntity);
                            list = getMaintainanceList(longPanInfo.LongPlanId, longPanInfo.FactoryId ?? -1, header.ProcessMode, null, isDisplayMaintainanceKind, SchedulePlanContent.Maintainance);
                        }

                        // 更新シリアルIDで標準の排他チェックを行う項目(機器別管理基準部位、機器別管理基準内容)
                        if (!checkExclusiveSingle(listCtrlId, deleteRow))
                        {
                            // 排他エラー
                            return true;
                        }

                        // 日時で排他チェックを行う(添付情報、スケジュール、スケジュール詳細)
                        var get = list.Where(x => x.ManagementStandardsContentId == row.ManagementStandardsContentId).First();
                        if (isErrorExclusiveUpdtime(true, row, get))
                        {
                            return true;
                        }
                    }

                    // スケジュールに保全活動の紐づきチェック
                    int result = TMQUtil.SqlExecuteClass.SelectEntity<int>(SqlName.Detail.GetSummaryScheduleByContent, SqlName.Detail.SubDirLongPlan, row, this.db);
                    // 保全活動が紐づいている場合、エラー
                    if (result > 0)
                    {
                        //「保全活動が作成されているため、削除できません。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141300004 });
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// 行単位の排他処理(日時項目)
            /// </summary>
            /// <param name="isCheckAttachment">添付情報の排他チェックを行う場合True　行削除は行うがスケジュール確定は行わない</param>
            /// <param name="savedRow">画面より取得した検索時の内容</param>
            /// <param name="getRow">SQLで取得した最新の内容</param>
            /// <returns>排他エラーならTrue</returns>
            bool isErrorExclusiveUpdtime(bool isCheckAttachment, Dao.Detail.List savedRow, Dao.Detail.List getRow)
            {
                // 添付情報
                if (isCheckAttachment)
                {
                    // 行削除時はチェックする
                    if (!CheckExclusiveStatusByUpdateDatetime(savedRow.AttachmentUpdateDatetime, getRow.AttachmentUpdateDatetime))
                    {
                        return true;
                    }
                }

                // スケジュール
                if (!CheckExclusiveStatusByUpdateDatetime(savedRow.ScheduleHeadUpdtime, getRow.ScheduleHeadUpdtime))
                {
                    return true;
                }

                // スケジュール詳細
                if (!CheckExclusiveStatusByUpdateDatetime(savedRow.ScheduleDetailUpdtime, getRow.ScheduleDetailUpdtime))
                {
                    return true;
                }

                return false;
            }

            //削除する行の情報を変更管理に反映
            bool registDeleteRowData(Dao.ListSearchResult header, List<Dictionary<string, object>> deleteList)
            {
                //システム日時
                DateTime now = DateTime.Now;
                // 登録更新ユーザ
                int userId = int.Parse(this.UserId);
                TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);

                if (header.ProcessMode == (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction)
                {
                    // トランザクションモード

                    // 変更管理テーブル登録処理(申請区分は「変更申請」)
                    (bool returnFlag, long historyManagementId) historyManagementResult =
                        historyManagement.InsertHistoryManagement(TMQConst.MsStructure.StructureId.ApplicationDivision.Update, header.LongPlanId, getFactoryId(historyManagement, header.LongPlanId));
                    if (!historyManagementResult.returnFlag)
                    {
                        return false;
                    }
                    header.HistoryManagementId = historyManagementResult.historyManagementId;
                }
                else
                {
                    // 変更管理モード

                    //変更管理テーブル更新処理
                    (bool flg, ComDao.HmHistoryManagementEntity entity) updateResult = updateHistoryManagement(header.HistoryManagementId, userId, now, false);
                    if (!updateResult.flg)
                    {
                        return false;
                    }
                }

                foreach (var deleteRow in deleteList)
                {
                    Dao.Detail.List info = new();
                    SetDataClassFromDictionary(deleteRow, listCtrlId, info);
                    if (info.HmManagementStandardsContentId == null || info.HmManagementStandardsContentId == 0)
                    {
                        //トランザクション行を削除

                        //機器別管理基準内容変更管理テーブルの登録
                        bool contentResult = registHistoryContent(header.HistoryManagementId, info.ManagementStandardsContentId, ExecutionDivision.DeleteContent, now, userId);
                        if (!contentResult)
                        {
                            return false;
                        }
                    }
                    else if (info.ApplicationDivisionCode == ((int)TMQConst.MsStructure.StructureId.ApplicationDivision.New).ToString())
                    {
                        //追加した行を削除

                        // 機器別管理基準内容変更管理テーブル削除
                        bool deleteContent = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteHmContent, SqlName.Detail.SubDir, new { HmManagementStandardsContentId = info.HmManagementStandardsContentId }, this.db, "", new List<string> { "RowDelete" });
                        if (!deleteContent)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 機器別管理基準内容変更管理テーブルの登録
        /// </summary>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <param name="managementStandardsContentId">機器別管理基準内容ID</param>
        /// <param name="now">システム日時</param>
        /// <param name="userId">ユーザID</param>
        /// <returns>エラーの場合False</returns>
        private bool registHistoryContent(long historyManagementId, long managementStandardsContentId, int executionDivision, DateTime now, int userId, long? longPlanId = null)
        {
            //元の機器別管理基準内容を取得
            ComDao.McManagementStandardsContentEntity content = new ComDao.McManagementStandardsContentEntity().GetEntity(managementStandardsContentId, this.db);
            //機器別管理基準内容の値を機器別管理基準内容変更管理へ反映
            ComDao.HmMcManagementStandardsContentEntity entity = new();
            entity.HistoryManagementId = historyManagementId;
            entity.ManagementStandardsContentId = managementStandardsContentId;
            entity.ExecutionDivision = executionDivision;
            entity.ManagementStandardsComponentId = content.ManagementStandardsComponentId;
            entity.InspectionContentStructureId = content.InspectionContentStructureId;
            entity.InspectionSiteImportanceStructureId = content.InspectionSiteImportanceStructureId;
            entity.InspectionSiteConservationStructureId = content.InspectionSiteConservationStructureId;
            entity.MaintainanceDivision = content.MaintainanceDivision;
            entity.MaintainanceKindStructureId = content.MaintainanceKindStructureId;
            entity.BudgetAmount = content.BudgetAmount;
            entity.PreparationPeriod = content.PreparationPeriod;
            entity.LongPlanId = longPlanId ?? content.LongPlanId;
            entity.OrderNo = content.OrderNo;
            entity.ScheduleTypeStructureId = content.ScheduleTypeStructureId;
            // テーブル共通項目を設定
            setExecuteConditionByDataClassCommon<ComDao.HmMcManagementStandardsContentEntity>(ref entity, now, userId, userId);

            return TMQUtil.SqlExecuteClass.Regist(SqlName.HM0001.InsertManagementStandardsContentInfo, SqlName.HM0001.SubDir, entity, this.db, "", new List<string> { "DefaultContent" });
        }

        /// <summary>
        ///  ボタン非表示制御フラグ取得
        /// </summary>
        /// <param name="result">詳細画面の検索結果</param>
        /// <param name="condition">詳細画面の検索条件</param>
        private void GetIsAbleToClickBtn(Dao.ListSearchResult result, Dao.detailSearchCondition condition)
        {
            // ①申請の申請者IDがログインユーザまたはシステム管理者かどうか
            // ②変更管理IDが紐付く機番情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDかどうか
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
            historyManagement.GetFlgHideButton(ref result, condition.HistoryManagementId, (TMQConst.MsStructure.StructureId.ProcessMode)Enum.ToObject(typeof(TMQConst.MsStructure.StructureId.ProcessMode), condition.ProcessMode));
        }

        /// <summary>
        /// 削除申請ボタンの削除処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool deleteRequestDetailAll()
        {
            // ヘッダの内容
            var header = getHeaderDetailForm();

            // 入力チェック
            // 保全活動が紐づいている場合、エラー
            int count = TMQUtil.SqlExecuteClass.SelectEntity<int>(SqlName.Detail.GetCountScheduledContent, SqlName.Detail.SubDirLongPlan, header, this.db);
            if (count > 0)
            {
                // 警告終了
                this.Status = CommonProcReturn.ProcStatus.Warning;
                //「保全活動が作成されているため、削除できません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141300004 });
                return false;
            }

            //「承認済」以外の変更管理が紐づいている場合、エラー
            if (!checkHistoryManagement(header.LongPlanId))
            {
                return false;
            }

            // 排他チェック
            if (!checkExcluseveDetailHeader())
            {
                return false;
            }

            // 表示中の一覧のコントロールID
            bool isDisplayMaintainanceKind = getIsDisplayMaintainanceKindByDetail(false);
            string listCtrlId = getDetailListCtrlId(isDisplayMaintainanceKind);
            // 保全項目一覧
            var list = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, listCtrlId);

            // 変更管理テーブル・変更管理詳細テーブル 登録処理
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);

            // SQL実行(申請区分は「削除申請」)
            (bool returnFlag, long historyManagementId) historyManagementResult =
                historyManagement.InsertHistoryManagement(TMQConst.MsStructure.StructureId.ApplicationDivision.Delete,
                                                                   header.LongPlanId,
                                                                   getFactoryId(historyManagement, header.LongPlanId));
            // 登録に失敗した場合は終了
            if (!historyManagementResult.returnFlag)
            {
                return false;
            }

            DateTime now = DateTime.Now;
            // 登録更新ユーザ
            int userId = int.Parse(this.UserId);

            // 削除する情報を長計件名変更管理テーブルに登録する
            if (!registHmLnLongPlan(historyManagementResult.historyManagementId, header.LongPlanId, now))
            {
                return false;
            }
            foreach (var row in list)
            {
                Dao.Detail.List info = new();
                SetDataClassFromDictionary(row, listCtrlId, info);
                //機器別管理基準内容変更管理テーブルの登録
                bool contentResult = registHistoryContent(historyManagementResult.historyManagementId, info.ManagementStandardsContentId, ExecutionDivision.DeleteContent, now, userId);
                if (!contentResult)
                {
                    return false;
                }
            }
            //再表示
            if (!initDetail(DetailDispType.Redisplay, historyManagementResult.historyManagementId))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 申請内容取消処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool cancelRequest()
        {
            // ヘッダの内容
            Dao.ListSearchResult header = getHeaderDetailForm();

            // 排他チェック
            if (!checkExclusiveSingleForTable(ConductInfo.FormDetail.ControlId.Hide, new List<string> { new ComDao.HmHistoryManagementEntity().TableName }))
            {
                return false;
            }

            // 変更管理テーブル、変更管理詳細テーブル 削除処理
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
            bool result = historyManagement.DeleteHistoryManagement(header.HistoryManagementId);
            if (!result)
            {
                return false;
            }

            //長計件名変更管理の削除（削除対象が無い場合も有）
            TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteHmLnLongPlan, SqlName.Detail.SubDir, new { HistoryManagementId = header.HistoryManagementId }, this.db);
            //機器別管理基準内容変更管理の削除（削除対象が無い場合も有）
            TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteHmContent, SqlName.Detail.SubDir, new { HistoryManagementId = header.HistoryManagementId }, this.db, "", new List<string> { "AllDelete" });
            return true;
        }

        /// <summary>
        /// 参照画面　ヘッダの排他チェック
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool checkExcluseveDetailHeader()
        {
            // 長計件名の排他処理
            if (!checkExclusiveSingleForTable(ConductInfo.FormDetail.ControlId.Hide, new List<string> { new ComDao.LnLongPlanEntity().TableName }))
            {
                return false;
            }
            // 他のテーブルの排他処理
            var header = getHeaderDetailForm();
            Dao.detailSearchCondition param = new();
            param.LongPlanId = header.LongPlanId;
            var newData = getLongPlanInfo(param);
            // 機器別管理基準内容
            if (!CheckExclusiveStatusByUpdateDatetime(header.McManStConUpdateDatetime, newData.McManStConUpdateDatetime))
            {
                return false;
            }
            // スケジュール詳細
            if (!CheckExclusiveStatusByUpdateDatetime(header.ScheDetailUpdateDatetime, newData.ScheDetailUpdateDatetime))
            {
                return false;
            }
            // 添付情報
            if (!CheckExclusiveStatusByUpdateDatetime(header.AttachmentUpdateDatetime, newData.AttachmentUpdateDatetime))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 長期計画件名に対する「承認済み」以外のデータ存在チェック
        /// </summary>
        /// <param name="longPlanId">長計件名ID</param>
        /// <returns>存在しない:True 存在する:False</returns>
        private bool checkHistoryManagement(long longPlanId)
        {
            // 「承認済み」以外のデータ件数取得
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
            if (historyManagement.getApplicationStatusCntByKeyId(longPlanId, TMQConst.MsStructure.StructureId.ApplicationStatus.Approved, false) > 0)
            {
                // 「 対象データに申請が存在するため処理を行えません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160016 });
                return false;
            }

            return true;
        }

        /// <summary>
        /// 長計件名変更管理テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <param name="longPlanId">長計件名ID</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHmLnLongPlan(long historyManagementId, long longPlanId, DateTime now)
        {
            // 長計件名IDより長計件名情報を取得する
            ComDao.LnLongPlanEntity entity = new ComDao.LnLongPlanEntity().GetEntity(longPlanId, this.db);

            // 取得した長計件名の値を長計件名変更管理データクラスに設定する
            ComDao.HmLnLongPlanEntity historyEntity = new();
            historyEntity.HistoryManagementId = historyManagementId;
            historyEntity.LongPlanId = entity.LongPlanId;
            historyEntity.ExecutionDivision = ExecutionDivision.DeleteLongPlan;
            historyEntity.Subject = entity.Subject;
            historyEntity.LocationStructureId = entity.LocationStructureId;
            historyEntity.LocationDistrictStructureId = entity.LocationDistrictStructureId;
            historyEntity.LocationFactoryStructureId = entity.LocationFactoryStructureId;
            historyEntity.LocationPlantStructureId = entity.LocationPlantStructureId;
            historyEntity.LocationSeriesStructureId = entity.LocationSeriesStructureId;
            historyEntity.LocationStrokeStructureId = entity.LocationStrokeStructureId;
            historyEntity.LocationFacilityStructureId = entity.LocationFacilityStructureId;
            historyEntity.JobStructureId = entity.JobStructureId;
            historyEntity.JobKindStructureId = entity.JobKindStructureId;
            historyEntity.JobLargeClassficationStructureId = entity.JobLargeClassficationStructureId;
            historyEntity.JobMiddleClassficationStructureId = entity.JobMiddleClassficationStructureId;
            historyEntity.JobSmallClassficationStructureId = entity.JobSmallClassficationStructureId;
            historyEntity.SubjectNote = entity.SubjectNote;
            historyEntity.PersonId = entity.PersonId;
            historyEntity.PersonName = entity.PersonName;
            historyEntity.WorkItemStructureId = entity.WorkItemStructureId;
            historyEntity.BudgetManagementStructureId = entity.BudgetManagementStructureId;
            historyEntity.BudgetPersonalityStructureId = entity.BudgetPersonalityStructureId;
            historyEntity.MaintenanceSeasonStructureId = entity.MaintenanceSeasonStructureId;
            historyEntity.PurposeStructureId = entity.PurposeStructureId;
            historyEntity.WorkClassStructureId = entity.WorkClassStructureId;
            historyEntity.TreatmentStructureId = entity.TreatmentStructureId;
            historyEntity.FacilityStructureId = entity.FacilityStructureId;

            // 登録更新ユーザ
            int userId = int.Parse(this.UserId);
            // テーブル共通項目を設定
            setExecuteConditionByDataClassCommon<ComDao.HmLnLongPlanEntity>(ref historyEntity, now, userId, userId);

            // SQL実行
            bool result = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertHmLnLongPlan, SqlName.Detail.SubDir, historyEntity, this.db, "", new List<string> { "DeleteRequest" });
            if (!result)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 指定された長期計画件名に「承認済」以外の変更管理が紐づいているかチェック
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private int checkExistsHistory()
        {
            // 長期計画IDを取得
            Dao.detailSearchCondition param = getParam(ConductInfo.FormDetail.ControlId.Hide, true, false);

            //承認済み以外の変更管理が紐づいているか取得
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
            int count = historyManagement.getApplicationStatusCntByKeyId(param.LongPlanId, TMQConst.MsStructure.StructureId.ApplicationStatus.Approved, false);

            // 返り値を設定(実際は画面に設定しない)
            Dao.ListSearchResult result = new() { IsTransitionFlg = count == 0 };
            SetFormByDataClass(ConductInfo.FormDetail.ControlId.Hide, new List<Dao.ListSearchResult>() { result });

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 変更管理テーブルの排他チェック
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private int checkExclusiveHistory()
        {
            // 排他チェック
            bool flg = checkExclusiveSingleForTable(ConductInfo.FormDetail.ControlId.Hide, new List<string> { new ComDao.HmHistoryManagementEntity().TableName });

            // 返り値を設定(実際は画面に設定しない)
            Dao.ListSearchResult result = new() { IsTransitionFlg = flg };
            SetFormByDataClass(ConductInfo.FormDetail.ControlId.Hide, new List<Dao.ListSearchResult>() { result });

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 承認依頼引戻処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executePullBackRequest()
        {
            // ヘッダの内容
            var header = getHeaderDetailForm();

            // 排他チェック
            if (!checkExclusiveSingleForTable(ConductInfo.FormDetail.ControlId.Hide, new List<string> { new ComDao.HmHistoryManagementEntity().TableName }))
            {
                return false;
            }

            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);

            ComDao.HmHistoryManagementEntity condition = new();
            condition.HistoryManagementId = header.HistoryManagementId;
            // 変更管理テーブル更新登録処理
            bool result = historyManagement.UpdateApplicationStatus(condition, TMQConst.MsStructure.StructureId.ApplicationStatus.Return);
            // 登録に失敗した場合は終了
            if (!result)
            {
                return false;
            }

            //再表示
            if (!initDetail(DetailDispType.Redisplay))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 承認処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeApproval()
        {
            // ヘッダの内容
            var header = getHeaderDetailForm();

            // 排他チェック
            if (!checkExclusiveSingleForTable(ConductInfo.FormDetail.ControlId.Hide, new List<string> { new ComDao.HmHistoryManagementEntity().TableName }))
            {
                return false;
            }

            // 登録情報を作成
            ComDao.HmHistoryManagementEntity condition = new();
            condition.HistoryManagementId = header.HistoryManagementId;

            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
            // 入力チェック
            if (historyManagement.isErrorBeforeUpdateApplicationStatus(new List<ComDao.HmHistoryManagementEntity>() { condition }, true, out string[] errMsg))
            {
                // エラーメッセージを設定
                this.MsgId = GetResMessage(errMsg);
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 変更管理テーブルの申請状況更新処理
            if (!historyManagement.UpdateApplicationStatus(condition, TMQConst.MsStructure.StructureId.ApplicationStatus.Approved))
            {
                return false;
            }

            //変更管理の内容をトランザクションテーブルへ反映
            if (!approvalHistory(condition))
            {
                return false;
            }

            return true;
        }
    }
}
