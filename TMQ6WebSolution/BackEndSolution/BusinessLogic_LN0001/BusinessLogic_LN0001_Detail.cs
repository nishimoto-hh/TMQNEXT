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
using System.Text.RegularExpressions;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_LN0001.BusinessLogicDataClass_LN0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using SchedulePlanContent = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.SchedulePlanContent;

using ScheduleStatus = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleStatus;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;
using DocumentFormat.OpenXml.Math;

namespace BusinessLogic_LN0001
{
    /// <summary>
    /// 件名別長期計画(参照画面)
    /// </summary>
    public partial class BusinessLogic_LN0001 : CommonBusinessLogicBase
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
        /// <returns>エラーの場合False</returns>
        private bool initDetail(DetailDispType type)
        {
            // 表示タイプの再設定
            type = updateType(type);
            // キー情報取得元のコントロールIDを取得
            string ctrlId = getCtrlIdForKeyInfo(type);
            // 長期計画IDを取得
            var param = getParam(ctrlId, type == DetailDispType.Redisplay);
            // 初期化処理呼出
            // 一覧画面の一覧より取得した情報で参照画面の項目に値を設定する
            initFormByLongPlanId(param, getResultMappingInfoByGrpNo(ConductInfo.FormDetail.HeaderGroupNo).CtrlIdList, out bool isDisplayMaintainanceKind, out int factoryId, true, ctrlId == ConductInfo.FormList.ControlId.List);
            // スケジュールのシステム年度設定処理
            setScheduleYear(type, factoryId);
            // 一覧の設定
            setDetailList(isDisplayMaintainanceKind, factoryId);
            // 非表示項目
            // 変更管理ボタンの表示制御用フラグ
            setHistoryManagementFlg(ConductInfo.FormDetail.ControlId.Hide, factoryId);

            // ★画面定義の翻訳情報取得★
            GetContorlDefineTransData(factoryId);

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
                var list = getMaintainanceList(param.LongPlanId, factoryId, isDisplayMaintainanceKind, contentType);
                // ページ情報取得
                string listCtrlId = getDetailListCtrlId(isDisplayMaintainanceKind);
                var pageInfo = GetPageInfo(listCtrlId, this.pageInfoList);
                // 画面にセット
                SetSearchResultsByDataClass<Dao.Detail.List>(pageInfo, list, list.Count);
                // スケジュール情報のセット
                // 保全活動の場合移動可能
                setSchedule(isDisplayMaintainanceKind, listCtrlId, true, param.LongPlanId, factoryId, contentType);
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
            // 表示中の一覧のコントロールID
            bool isDisplayMaintainanceKind = getIsDisplayMaintainanceKindByDetail(false);
            string listCtrlId = getDetailListCtrlId(isDisplayMaintainanceKind);

            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.Detail.SubDir, SqlName.Detail.DeleteRow, out string sql);
            // 削除
            if (!DeleteSelectedList<ComDao.McManagementStandardsContentEntity>(listCtrlId, sql, isErrorDeleteRow))
            {
                return false;
            }
            // 再検索
            if (!initDetail(DetailDispType.RedisplaySearch))
            {
                return false;
            }

            return true;

            // 行削除エラーチェック処理
            bool isErrorDeleteRow(string listCtrlId, List<Dictionary<string, object>> deleteList)
            {
                // 排他チェック
                if (isErrorExclusiveDetailRow(true, listCtrlId, deleteList, isDisplayMaintainanceKind))
                {
                    return true;
                }

                // 削除対象の行を繰り返しチェック
                foreach (var deleteRow in deleteList)
                {
                    Dao.Detail.List row = new();
                    SetDataClassFromDictionary(deleteRow, listCtrlId, row);

                    // スケジュールに保全活動の紐づきチェック
                    int result = TMQUtil.SqlExecuteClass.SelectEntity<int>(SqlName.Detail.GetSummaryScheduleByContent, SqlName.Detail.SubDir, row, this.db);
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
        }

        /// <summary>
        /// 参照画面　一覧の行単位で排他チェックを行う処理
        /// </summary>
        /// <param name="isCheckAttachment">添付情報の排他チェックを行う場合True　行削除は行うがスケジュール確定は行わない</param>
        /// <param name="listCtrlId">一覧のID</param>
        /// <param name="targetList">一覧の更新対象の行の画面情報</param>
        /// <param name="isDisplayMaintainanceKind">保全情報一覧(点検種別)が表示されている場合True</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusiveDetailRow(bool isCheckAttachment, string listCtrlId, List<Dictionary<string, object>> targetList, bool isDisplayMaintainanceKind)
        {
            // 更新シリアルIDで標準の排他チェックを行う項目(機器別管理基準部位、機器別管理基準内容)
            if (!checkExclusiveList(listCtrlId, targetList))
            {
                // 排他エラー
                return true;
            }

            // 日時で排他チェックを行う項目(添付情報、スケジュール、スケジュール詳細)

            // 一覧情報(削除対象行に長期計画IDを持つため、最初は空)
            IList<Dao.Detail.List> list = new List<Dao.Detail.List>();

            // 削除対象の行を繰り返しチェック
            foreach (var targetRow in targetList)
            {
                Dao.Detail.List row = new();
                SetDataClassFromDictionary(targetRow, listCtrlId, row);

                if (list.Count == 0)
                {
                    // 一覧情報の再取得(削除対象の先頭行のみ)
                    ComDao.LnLongPlanEntity longPlanEntity = new();
                    longPlanEntity.LongPlanId = row.LongPlanId;
                    var longPanInfo = getLongPlanInfo(longPlanEntity);
                    list = getMaintainanceList(longPanInfo.LongPlanId, longPanInfo.FactoryId ?? -1, isDisplayMaintainanceKind, SchedulePlanContent.Maintainance);
                }

                // 排他チェック(添付情報、スケジュール、スケジュール詳細)
                var get = list.Where(x => x.ManagementStandardsContentId == row.ManagementStandardsContentId).First();
                if (isErrorExclusiveUpdtime(row, get))
                {
                    return true;
                }
            }

            return false;

            /// <summary>
            /// 行単位の排他処理(日時項目)
            /// </summary>
            /// <param name="savedRow">画面より取得した検索時の内容</param>
            /// <param name="getRow">SQLで取得した最新の内容</param>
            /// <returns>排他エラーならTrue</returns>
            bool isErrorExclusiveUpdtime(Dao.Detail.List savedRow, Dao.Detail.List getRow)
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
        }

        /// <summary>
        /// 削除ボタンの削除処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool deleteDetailAll()
        {
            // ヘッダの内容
            var header = getHeaderDetailForm();

            // 入力チェック
            // 保全活動が紐づいている場合、エラー
            int count = TMQUtil.SqlExecuteClass.SelectEntity<int>(SqlName.Detail.GetCountScheduledContent, SqlName.Detail.SubDir, header, this.db);
            if (count > 0)
            {
                // 警告終了
                this.Status = CommonProcReturn.ProcStatus.Warning;
                //「保全活動が作成されているため、削除できません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141300004 });
                return false;
            }

            // 排他チェック
            if (!checkExcluseveDetailHeader())
            {
                return false;
            }

            // 削除
            // 長計件名の削除
            if (!new ComDao.LnLongPlanEntity().DeleteByPrimaryKey(header.LongPlanId, this.db))
            {
                return false;
            }

            // 機器別管理基準内容更新
            // 更新条件をヘッダの隠し項目の長計件名IDより取得
            var headerDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.Hide);
            ComDao.LnLongPlanEntity update = new();
            SetExecuteConditionByDataClass<ComDao.LnLongPlanEntity>(headerDic, ConductInfo.FormDetail.ControlId.Hide, update, DateTime.Now, this.UserId);
            // 更新処理(更新レコードが無い場合も処理継続)
            TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteAll, SqlName.Detail.SubDir, update, this.db);

            // 添付情報削除
            // キーIDで削除
            new ComDao.AttachmentEntity().DeleteByKeyId(TMQConst.Attachment.FunctionTypeId.LongPlan, update.LongPlanId, this.db);

            return true;
        }

        /// <summary>
        /// 参照画面　ヘッダの排他チェック
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool checkExcluseveDetailHeader()
        {
            // 長計件名の排他処理
            if (!checkExclusiveSingle(ConductInfo.FormDetail.ControlId.Hide))
            {
                return false;
            }
            // 他のテーブルの排他処理
            var header = getHeaderDetailForm();
            ComDao.LnLongPlanEntity param = new();
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
        /// スケジュール確定処理
        /// </summary>
        /// <returns>エラーの場合-1、正常の場合1</returns>
        public int updateSchedule()
        {
            // 更新対象の一覧のID取得
            bool isDisplayMaintainanceKindByDetail = getIsDisplayMaintainanceKindByDetail(false);
            string listCtrlId = getDetailListCtrlId(isDisplayMaintainanceKindByDetail);
            // 更新対象の一覧の内容を取得
            var rowDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, listCtrlId);
            // 一覧のキー値のVAL値を取得(スケジュールとの紐付に使用)
            var valKeyId = GetValKeyIdForSchedule(listCtrlId);
            // 同一マークのキー値のVAL値を取得(スケジュールとの紐付に使用)
            var valSameMarkKey = GetValSameMarkKeyForSchedule(listCtrlId);

            // 長期計画の工場IDを取得
            int factoryId = getFactoryId();
            // 年度開始月
            int nendoStartMonth = getYearStartMonth(factoryId);

            // 一覧より更新されたスケジュールの情報を取得
            List<TMQDao.ScheduleList.Display> updatedScheduleList = TMQUtil.ScheduleListUtil.GetScheduleUpdatedData(rowDicList, valKeyId, nendoStartMonth);

            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();
            var info = getResultMappingInfo(listCtrlId);

            // 更新対象のキーIDの一覧を取得(画面のディクショナリとの紐づけに使用)
            var updateKeyIdList = updatedScheduleList.Select(x => x.KeyId).ToList();

            // エラー情報を表示するVAL値(エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得)
            string val = info.getValName("next_schedule_date");

            // 画面の内容より、検索時の次回実施予定日とスケジュール確定時の実施予定日が異なるデータを取得
            // ※次回実施予定日が変更されているデータのみを入力チェックとするため
            List<Dao.Detail.List> scheduleChangedList = new();
            foreach (var target in rowDicList)
            {
                // 更新対象行の情報をデータクラスに変換
                Dao.Detail.List updRowClass = new();
                SetDataClassFromDictionary(target, listCtrlId, updRowClass);

                // 次回実施予定日が未入力の場合または、次回実施予定日が変更されていない場合は何もしない
                if (updRowClass.ScheduleDate == null || updRowClass.ScheduleDate == updRowClass.ScheduleDateBefore)
                {
                    continue;
                }

                // 過去日のチェックは行わない
                //// 次回実施予定日に過去日が指定されている場合
                //if (updRowClass.ScheduleDate < DateTime.Now.Date)
                //{
                //    // エラー情報格納クラス
                //    ErrorInfo errorInfo = new ErrorInfo(target);
                //    // 過去日付は設定できません。
                //    errorInfo.setError(GetResMessage("141060003"), val);
                //    errorInfoDictionary.Add(errorInfo.Result);
                //    continue;
                //}

                // 次々回実施予定日を取得
                TMQUtil.GetFixedSqlStatement(SqlName.InputCheck.SubDir, SqlName.InputCheck.GetNextScheduleDate, out string getNextDaySql);
                IList<Dao.Detail.List> nextDate = db.GetListByDataClass<Dao.Detail.List>(getNextDaySql, new { ManagementStandardsContentId = updRowClass.ManagementStandardsContentId, ScheduleDateBefore = updRowClass.ScheduleDateBefore, MaintainanceScheduleId = updRowClass.MaintainanceScheduleId });

                // 取得できた場合は範囲チェックをする
                if (nextDate != null && nextDate.Count > 0)
                {
                    bool isDateErr = false;

                    // 入力された次回実施予定日が次々回実施予定日以降の場合
                    if (updRowClass.ScheduleDate >= nextDate[0].ScheduleDate)
                    {
                        isDateErr = true;
                    }

                    // 前回実施予定日は存在しない可能性があることを考慮する
                    if (nextDate.Count > 1)
                    {
                        // 入力された次回実施予定日が前回実施予定日の場合以前
                        if (updRowClass.ScheduleDate <= nextDate[1].ScheduleDate)
                        {
                            isDateErr = true;
                        }
                    }
                    else
                    {
                        // 前回実施予定日が存在しない場合は開始日と比較する
                        if (updRowClass.ScheduleDate < updRowClass.StartDate)
                        {
                            isDateErr = true;
                        }
                    }

                    // 日付の範囲エラーの場合はメッセージをセットする
                    if (isDateErr)
                    {
                        // エラー情報格納クラス
                        ErrorInfo errorInfo = new ErrorInfo(target);
                        // 次回実施予定日は前回実施予定日～次々回実施予定日以内の日付を指定してください。
                        string errMsg = GetResMessage("141120020");
                        errorInfo.setError(errMsg, val);
                        errorInfoDictionary.Add(errorInfo.Result);
                        continue;
                    }
                }

                // 次回実施予定日が変更されていて、スケジュールも変更されている場合
                // 同時には変更できないのでエラーとする
                if (updRowClass.ScheduleDate != updRowClass.ScheduleDateBefore && updateKeyIdList.Contains(updRowClass.KeyId))
                {
                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(target);
                    // 星取表と次回実施予定日を同時に変更することはできません。
                    errorInfo.setError(GetResMessage("141300010"), val);
                    errorInfoDictionary.Add(errorInfo.Result);
                    continue;
                }

                // 入力エラーが無い場合は次回実施予定日変更リストに格納
                scheduleChangedList.Add(updRowClass);
            }

            // 入力エラーがある場合はここで終了
            if (errorInfoDictionary.Count > 0)
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return ComConsts.RETURN_RESULT.NG;
            }

            // スケジュールが変更された件数も次回実施予定日が変更された件数も0件の場合はここで終了
            if (updatedScheduleList.Count == 0 && scheduleChangedList.Count == 0)
            {
                // 更新データなし
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「該当データがありません。」
                this.MsgId = GetResMessage(CommonResources.ID.ID941060001);
                return ComConsts.RETURN_RESULT.NG;
            }

            // 排他チェック
            // 更新対象行
            // 画面より取得した一覧の内容を使って排他チェックをするので、更新対象のキーIDの行だけを抽出
            var updateTargetDics = rowDicList.Where(x => updateKeyIdList.IndexOf(getKeyIdByDic(x)) > -1).ToList();

            // 排他チェック対象リスト
            // 更新対象の列に加え、更新対象の列と同じ同一スケジュールマークキーの列も取得
            var checkTargetDics = getExclusiveTargets(rowDicList, updateTargetDics);

            // 排他チェック(スケジュールのまとめ表示により更新対象行が指定できないため全行)
            if (isErrorExclusiveDetailRow(false, listCtrlId, checkTargetDics, isDisplayMaintainanceKindByDetail))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 次回実施日が変更されたデータの登録処理
            if (!updateNextScheduleDate(scheduleChangedList))
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // スケジュールが変更されたデータの登録処理
            if (!updateScheduleDetail(updatedScheduleList, updateTargetDics))
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 再検索(非表示項目の情報がSearchConditionDictionaryに入っているので処理が特殊)
            if (!initDetail(DetailDispType.RedisplaySearch))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;

            // 行のディクショナリからキーIDを取得して返す
            string getKeyIdByDic(Dictionary<string, object> rowDic)
            {
                var keyId = rowDic[valKeyId].ToString();
                return keyId;
            }

            // 一覧のすべての行と更新対象行から、排他チェック対象行を取得する
            // List<Dictionary<string, object>> rowDicList 一覧のすべての行
            // List<Dictionary<string, object>> updateTargetDics 上記を絞り込んだ、更新対象行
            List<Dictionary<string, object>> getExclusiveTargets(List<Dictionary<string, object>> rowDicList, List<Dictionary<string, object>> updateTargetDics)
            {
                // 更新対象行と同じ値の同一スケジュールキーを持つ一覧の行を取得する
                List<Dictionary<string, object>> targets = new();
                foreach (var updRow in updateTargetDics)
                {
                    // 同一スケジュールキー
                    string markKey = getSameMarkKeyByDic(updRow);
                    // 一覧の行の内、同じもの
                    var addRows = rowDicList.Where(x => markKey == getSameMarkKeyByDic(x)).ToList();
                    targets.AddRange(addRows);
                }
                // キーIDで重複を排除
                var rtnTargets = targets.GroupBy(x => getKeyIdByDic(x)).Select(x => x.First()).ToList();
                return rtnTargets;

                // 行のディクショナリから同一スケジュールキーを取得して返す
                string getSameMarkKeyByDic(Dictionary<string, object> rowDic)
                {
                    var keyId = rowDic[valSameMarkKey].ToString();
                    return keyId;
                }
            }

            // 更新処理
            // List<TMQDao.ScheduleList.Display> updatedScheduleList 更新対象のスケジュール情報
            // List<Dictionary<string, object>> updateDicList 更新対象の一覧の情報
            // return エラーの場合False
            bool updateScheduleDetail(List<TMQDao.ScheduleList.Display> updatedScheduleList, List<Dictionary<string, object>> updateDicList)
            {
                // 更新対象行共通　更新日時、ユーザID
                var now = DateTime.Now;
                int userId = int.Parse(this.UserId);
                // 工場IDリスト(共通、長期計画)
                var factoryIdList = TMQUtil.GetFactoryIdList();
                factoryIdList.Add(factoryId);

                // 更新対象行で繰り返し
                foreach (var updRowDic in updateDicList)
                {
                    // 更新対象行の情報をデータクラスに変換
                    Dao.Detail.List updRowClass = new();
                    SetDataClassFromDictionary(updRowDic, listCtrlId, updRowClass);
                    // 更新対象のスケジュールの情報をキーIDにより取得
                    var updInfos = updatedScheduleList.Where(x => x.KeyId == updRowClass.KeyId);
                    // それぞれのスケジュールに対して処理
                    foreach (var updInfo in updInfos)
                    {
                        var updCond = getUpdCond(updInfo.KeyDate, updInfo.OriginDate, updRowClass.ManagementStandardsContentId, updInfo.IsUpdateYear, factoryIdList);
                        bool result = TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateSchedule, SqlName.Detail.SubDir, updCond, this.db);
                        if (!result)
                        {
                            return false;
                        }
                    }
                }
                return true;

                // スケジュール更新SQLの条件を作成する処理
                // DateTime updateDate 更新対象のスケジュールより取得した移動先の日付
                // DateTime originDate 更新対象のスケジュールより取得した移動元の日付
                // long managementStandardsContentId 更新対象の画面の行より取得した機器別管理基準内容ID
                // bool isUpdateYear 更新対象が年単位ならTrue、年月単位ならFalse
                Dao.Schedule.UpdateCondition getUpdCond(DateTime updateDate, DateTime originDate, long managementStandardsContentId, bool isUpdateYear, List<int> factoryIdList)
                {
                    Dao.Schedule.UpdateCondition cond = new();
                    cond.AddMonth = ComUtil.GetMonthDiff(updateDate, originDate); // 加算月数は移動先-移動元で算出
                    cond.ManagementStandardsContentId = managementStandardsContentId;
                    cond.MonthStartDate = new DateTime(originDate.Year, originDate.Month, 1); // 更新範囲開始年月日は移動元の1日
                    // 更新範囲終了年月日
                    // 年の場合→12月31日
                    // 年月の場合→移動先の月の最終日
                    cond.MonthEndDate = isUpdateYear ? ComUtil.GetNendoLastDay(originDate, nendoStartMonth) : ComUtil.GetDateMonthLastDay(originDate);
                    cond.UpdateUserId = userId;
                    cond.UpdateDatetime = now;
                    // 工場ID
                    cond.FactoryIdList = factoryIdList;
                    return cond;
                }
            }

            // 長期計画の工場IDを取得
            int getFactoryId()
            {
                // 長期計画IDを取得(searchConditionDictionaryより取得)
                var param = getParam(ConductInfo.FormDetail.ControlId.Hide, false);
                var longPlanInfo = getLongPlanInfo(param, false);
                int factoryId = longPlanInfo.FactoryId ?? -1;
                return factoryId;
            }

            // 次回実施予定日の更新処理
            // List<Dao.Detail.List> scheduleChangedList 次回実施予定日を変更するレコードのデータ
            bool updateNextScheduleDate(List<Dao.Detail.List> scheduleChangedList)
            {
                // 更新SQL文の取得
                if (!TMQUtil.GetFixedSqlStatement(SqlName.Detail.SubDir, SqlName.Detail.UpdateScheduleDateByDetailId, out string updateSql))
                {
                    return false;
                }

                // 機器が点検種別毎管理が取得するためのSQL文を取得
                if (!TMQUtil.GetFixedSqlStatement(SqlName.Detail.SubDir, SqlName.Detail.GetEquipInfoByMachineId, out string equipInfoSql))
                {
                    return false;
                }

                // 自身のレコードと同じ長期計画件名ID、機番ID、点検種別のデータを取得するためのSQL文を取得
                string withSql = getSqlGetDetailWith(false); // WITH句
                if (!TMQUtil.GetFixedSqlStatement(SqlName.Detail.SubDir, SqlName.Detail.GetDetailMaintainance, out string getContentIdSql))
                {
                    return false;
                }
                StringBuilder execSql = new(withSql);
                execSql.AppendLine(getContentIdSql);
                execSql.AppendLine(" kind_order");

                // 自身のレコードの点検種別と同一の点検種別のデータの次回実施予定日を更新するSQL文を取得
                if (!TMQUtil.GetFixedSqlStatement(SqlName.Detail.SubDir, SqlName.Detail.UpdateScheduleDateByDetailIdAndSameKind, out string updateSameKind))
                {
                    return false;
                }

                // 次回実施予定日変更リストの内容で繰り返し更新する
                DateTime now = DateTime.Now;
                foreach (Dao.Detail.List condition in scheduleChangedList)
                {
                    // 更新SQL実行
                    if (db.Regist(updateSql, new { ScheduleDate = condition.ScheduleDate, ScheduleDateBefore = condition.ScheduleDateBefore, Updatedatetime = now, Updateuserid = this.UserId, ManagementStandardsContentId = condition.ManagementStandardsContentId, MaintainanceScheduleId  = condition.MaintainanceScheduleId}) < 0)
                    {
                        return false;
                    }

                    // 更新対象機器が点検種別毎管理か取得
                    ComDao.McEquipmentEntity equipInfo = db.GetEntityByDataClass<ComDao.McEquipmentEntity>(equipInfoSql, condition);
                    if (equipInfo == null)
                    {
                        // 機器情報が取得できない場合はここで終了
                        continue;
                    }

                    // 点検種別毎管理か判定
                    if (!equipInfo.MaintainanceKindManage)
                    {
                        // 点検種別毎管理では無い場合はここで終了
                        continue;
                    }

                    // 同一点検種別のデータを取得(自分のデータ以外)
                    var param = getScheduleCondition(condition.LongPlanId, factoryId, getYearStartMonth(factoryId));
                    IList<Dao.Detail.List> scheduleInfoList = db.GetListByDataClass<Dao.Detail.List>(execSql.ToString(), param);
                    scheduleInfoList = scheduleInfoList.Where(x => x.ManagementStandardsContentId != condition.ManagementStandardsContentId && x.MaintainanceKindStructureId == condition.MaintainanceKindStructureId).ToList();
                    foreach (Dao.Detail.List scheduleInfo in scheduleInfoList)
                    {
                        // 次回実施予定日を更新する
                        // 次回実施予定日をもう一度取得しなおして、取得した日付のデータを更新する
                        // ※自身のレコードの次回実施予定日と他のデータの次回実施予定日が同一とは限らないため
                        if (db.Regist(updateSameKind, new { MachineId = equipInfo.MachineId, ScheduleDate = condition.ScheduleDate, Updatedatetime = now, Updateuserid = this.UserId, ManagementStandardsContentId = scheduleInfo.ManagementStandardsContentId, MaintainanceScheduleId = scheduleInfo.MaintainanceScheduleId }) < 0)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 指示検収表出力処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool outputFiles()
        {
            // ヘッダの内容より長期計画IDを取得
            var header = getHeaderDetailForm();
            // 長期計画IDより添付ファイルの情報を取得
            List<string> fileInfos = TMQUtil.SqlExecuteClass.SelectList<string>(SqlName.Detail.GetOutputFileInfo, SqlName.Detail.SubDir, header, this.db);

            if (fileInfos == null || fileInfos.Count == 0 || fileInfos.Any(x => !string.IsNullOrEmpty(x)) == false)
            {
                // 取得結果なし or 取得結果が全て空
                // ファイル無しの場合、エラー
                this.MsgId = GetResMessage(ComRes.ID.ID941060001);
                return false;
            }

            // 長期計画の情報
            var longPlanInfo = getLongPlanInfo(header, false);
            // 工場ID
            var factoryId = longPlanInfo.FactoryId;
            // 件名(ファイル名に使用)
            var subject = longPlanInfo.Subject;

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
                cond.StructureGroupId = (int)TMQConst.MsStructure.GroupId.TempFolderPath;
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
                List<string> fileInfoList = fileInfoChar.Split(TMQConst.FileInfoSplit.File).ToList();
                foreach (string fileInfo in fileInfoList)
                {
                    if (string.IsNullOrEmpty(fileInfo)) { break; } // 空(最後の行)なら終了
                    // ファイルパスとファイル名の区切り(|)で分割
                    List<string> file = fileInfo.Split(TMQConst.FileInfoSplit.Path).ToList();
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
