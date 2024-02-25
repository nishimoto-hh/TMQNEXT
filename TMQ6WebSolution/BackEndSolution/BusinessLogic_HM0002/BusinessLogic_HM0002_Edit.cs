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
    /// 変更管理 長期計画(編集画面)
    /// </summary>
    public partial class BusinessLogic_HM0002 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 参照画面の表示種類
        /// </summary>
        private enum EditDispType
        {
            /// <summary>新規</summary>
            New,
            /// <summary>修正</summary>
            Update,
            /// <summary>複写</summary>
            Copy,
            /// <summary>登録後再表示</summary>
            Redisplay
        }

        /// <summary>
        /// 詳細編集画面初期化処理
        /// </summary>
        /// <param name="type">詳細編集画面の起動種類</param>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <returns>エラーの場合False</returns>
        private bool initEdit(EditDispType type, long? historyManagementId = null)
        {
            // グループより対象のコントロールIDを取得
            List<string> toCtrlIdList = getResultMappingInfoByGrpNo(ConductInfo.FormEdit.HeaderGroupNo).CtrlIdList;

            if (type == EditDispType.New)
            {
                // 新規の場合
                Dao.ListSearchResult param = new();
                param.LocationStructureId = getTreeValue(true);
                param.JobStructureId = getTreeValue(false);
                // 取得した結果に対して、地区と職種の情報を設定する
                IList<Dao.ListSearchResult> paramList = new List<Dao.ListSearchResult> { param };
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.ListSearchResult>(ref paramList, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

                initFormByParam(param, toCtrlIdList);
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
            else
            {
                // 更新の場合
                // 再表示かどうか
                bool isReSearch = type == EditDispType.Redisplay;
                // キー情報取得元のコントロールID、再表示でない場合、参照画面の非表示項目。再表示の場合はこの画面の非表示項目
                string ctrlId = !isReSearch ? ConductInfo.FormDetail.ControlId.Hide : ConductInfo.FormEdit.ControlId.Hide;
                // キー情報取得
                var param = getParam(ctrlId, isReSearch, false, historyManagementId ?? -1);
                // 初期化処理呼出
                // 参照画面の非表示項目より取得した情報で参照画面の項目に値を設定する
                // ツリー表示
                initFormByLongPlanId(param, toCtrlIdList, out bool isMaintainanceKindFactory, out int factoryId, true);
                // ★画面定義の翻訳情報取得★
                GetContorlDefineTransData(factoryId);
            }

            return true;
        }
        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdit()
        {
            EditDispType editType = getEditType();
            bool isInsert = isInsertEdit(editType);
            // キー情報取得
            Dao.detailSearchCondition info = new();
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormEdit.ControlId.Hide);
            SetDataClassFromDictionary(targetDic, ConductInfo.FormEdit.ControlId.Hide, info, new List<string> { HistoryManagementId, LongPlanId });
            bool isTransactionMode = info.HistoryManagementId == null || info.HistoryManagementId <= 0;
            if (!isInsert && isTransactionMode)
            {
                //「承認済」以外の変更管理が紐づいている場合、エラー
                if (!checkHistoryManagement(info.LongPlanId))
                {
                    return false;
                }
            }

            // 排他チェック対象テーブル
            string exclusiveTable = isTransactionMode ? new ComDao.LnLongPlanEntity().TableName : new ComDao.HmHistoryManagementEntity().TableName;
            // 排他チェック(更新のみ)
            if (!isInsert && !checkExclusiveSingleForTable(ConductInfo.FormEdit.ControlId.Hide, new List<string> { exclusiveTable }))
            {
                return false;
            }

            // 画面情報取得
            DateTime now = DateTime.Now;
            Dao.ListSearchResult registInfo = GetRegistInfoByGroupNo<Dao.ListSearchResult>(ConductInfo.FormEdit.HeaderGroupNo, now);
            // 職種の階層情報を取得するために、階層を持つクラスに画面の内容を反映し、取得する
            IList<Dao.ListSearchResult> registStructureInfo = new List<Dao.ListSearchResult> { GetRegistInfoByGroupNo<Dao.ListSearchResult>(ConductInfo.FormEdit.HeaderGroupNo, now) };
            // 階層情報を設定
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.ListSearchResult>(ref registStructureInfo, new List<StructureType> { StructureType.Location, StructureType.Job });
            // 職種と地区の値を登録するデータクラスに設定
            setLayerInfo(ref registInfo, registStructureInfo[0]);
            // 登録
            if (!registDb(registInfo, out long newLongPlanId, out long historyManagementId))
            {
                return false;
            }

            // 再検索
            if (!isInsert)
            {
                // 更新時、再検索処理
                return initEdit(EditDispType.Redisplay, historyManagementId);
            }
            // INSERTの場合の再検索処理
            var param = new Dao.detailSearchCondition();
            param.LongPlanId = newLongPlanId;
            param.HistoryManagementId = historyManagementId;
            param.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.history;
            List<string> toCtrlIdList = getResultMappingInfoByGrpNo(ConductInfo.FormEdit.HeaderGroupNo).CtrlIdList;
            initFormByLongPlanId(param, toCtrlIdList, out bool isMaintainanceKindFactory, out int factoryId, true);
            return true;

            // 画面のツリーの階層情報を登録用データクラスにセット
            void setLayerInfo(ref Dao.ListSearchResult target, Dao.ListSearchResult source)
            {
                // 場所階層
                target.LocationStructureId = source.LocationStructureId;
                // 各階層のIDは名称のプロパティに文字列として格納される（ツリーの定義の関係）ため、数値に変換
                target.LocationDistrictStructureId = ComUtil.ConvertStringToInt(source.DistrictName);
                target.LocationFactoryStructureId = ComUtil.ConvertStringToInt(source.FactoryName);
                target.LocationPlantStructureId = ComUtil.ConvertStringToInt(source.PlantName);
                target.LocationSeriesStructureId = ComUtil.ConvertStringToInt(source.SeriesName);
                target.LocationStrokeStructureId = ComUtil.ConvertStringToInt(source.StrokeName);
                target.LocationFacilityStructureId = ComUtil.ConvertStringToInt(source.FacilityName);
                // 職種機種階層
                target.JobStructureId = source.JobStructureId;
                // 各階層のIDは名称のプロパティに文字列として格納される（ツリーの定義の関係）ため、数値に変換
                target.JobKindStructureId = ComUtil.ConvertStringToInt(source.JobName);
                target.JobLargeClassficationStructureId = ComUtil.ConvertStringToInt(source.LargeClassficationName);
                target.JobMiddleClassficationStructureId = ComUtil.ConvertStringToInt(source.MiddleClassficationName);
                target.JobSmallClassficationStructureId = ComUtil.ConvertStringToInt(source.SmallClassficationName);
            }
        }

        /// <summary>
        /// 画面の起動種類を呼出元ボタンの画面遷移アクション区分より判定
        /// </summary>
        /// <returns>画面の起動種類、新規or修正or複写</returns>
        private EditDispType getEditType()
        {
            switch (this.TransActionDiv)
            {
                case LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.New:
                    // 新規
                    return EditDispType.New;
                    break;
                case LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Edit:
                    // 修正
                    return EditDispType.Update;
                    break;
                case LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Copy:
                    // 複写
                    return EditDispType.Copy;
                    break;
                default:
                    // 到達不能
                    throw new Exception();
            }
        }

        /// <summary>
        /// INSERTかUPDATEかを取得
        /// </summary>
        /// <param name="type">この画面の起動種類</param>
        /// <returns>INSERTならTRUE</returns>
        private bool isInsertEdit(EditDispType type)
        {
            return type == EditDispType.New || type == EditDispType.Copy;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <param name="newLongPlanId">out INSERTの場合、採番した長期計画ID</param>
        /// <returns>エラーの場合False</returns>
        private bool registDb(Dao.ListSearchResult registInfo, out long newLongPlanId, out long historyManagementId)
        {
            newLongPlanId = -1;
            historyManagementId = -1;

            //システム日時
            DateTime now = DateTime.Now;
            // 登録更新ユーザ
            int userId = int.Parse(this.UserId);

            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);

            if (isInsertEdit(getEditType()))
            {
                // 新規登録申請・複写申請
                return registNew(out newLongPlanId, out historyManagementId);
            }
            else
            {
                if (registInfo.HistoryManagementId <= 0)
                {
                    // 変更申請の場合
                    return changeRequest(out newLongPlanId, out historyManagementId);
                }
                else
                {
                    //申請内容修正の場合
                    return editRequest(out newLongPlanId, out historyManagementId);
                }
            }

            // 新規登録申請・複写申請
            bool registNew(out long newLongPlanId, out long historyManagementId)
            {
                newLongPlanId = -1;
                historyManagementId = -1;

                // 新規登録申請・複写申請の場合は長期計画件名IDを先に採番する
                newLongPlanId = getNewLongPlanId();
                registInfo.LongPlanId = newLongPlanId;
                // 変更管理テーブル登録処理
                (bool returnFlag, long historyManagementId) historyManagementResult =
                    historyManagement.InsertHistoryManagement(TMQConst.MsStructure.StructureId.ApplicationDivision.New, registInfo.LongPlanId, historyManagement.getFactoryId(registInfo.LocationStructureId ?? -1));
                // 登録に失敗した場合は終了
                if (!historyManagementResult.returnFlag)
                {
                    return false;
                }
                //変更管理ID
                historyManagementId = historyManagementResult.historyManagementId;
                registInfo.HistoryManagementId = historyManagementResult.historyManagementId;
                //実行処理区分
                registInfo.ExecutionDivision = ExecutionDivision.NewLongPlan;
                ComDao.HmLnLongPlanEntity entity = setHmLnLongPlanEntity(registInfo);
                //長計件名変更管理の登録
                return TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertHmLnLongPlan, SqlName.Detail.SubDir, entity, this.db, "", new List<string> { "New" });
            }

            // 変更申請の場合
            bool changeRequest(out long newLongPlanId, out long historyManagementId)
            {
                newLongPlanId = -1;
                historyManagementId = -1;

                // 元の長期計画件名を取得
                ComDao.LnLongPlanEntity longPlan = new ComDao.LnLongPlanEntity().GetEntity(registInfo.LongPlanId, this.db);
                // 変更管理テーブル登録処理
                (bool returnFlag, long historyManagementId) historyManagementResult =
                    historyManagement.InsertHistoryManagement(TMQConst.MsStructure.StructureId.ApplicationDivision.Update, registInfo.LongPlanId, historyManagement.getFactoryId(longPlan.LocationStructureId ?? -1));
                // 登録に失敗した場合は終了
                if (!historyManagementResult.returnFlag)
                {
                    return false;
                }
                //長期計画ID
                newLongPlanId = registInfo.LongPlanId;
                //変更管理ID
                historyManagementId = historyManagementResult.historyManagementId;
                registInfo.HistoryManagementId = historyManagementResult.historyManagementId;
                //実行処理区分
                registInfo.ExecutionDivision = ExecutionDivision.UpdateLongPlan;
                ComDao.HmLnLongPlanEntity entity = setHmLnLongPlanEntity(registInfo);
                //長計件名変更管理の登録
                return TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertHmLnLongPlan, SqlName.Detail.SubDir, entity, this.db, "", new List<string> { "New" });
            }

            //申請内容修正の場合
            bool editRequest(out long newLongPlanId, out long historyManagementId)
            {
                newLongPlanId = -1;
                historyManagementId = -1;

                //変更管理の情報を取得
                ComDao.HmHistoryManagementEntity history = new ComDao.HmHistoryManagementEntity().GetEntity(registInfo.HistoryManagementId, this.db);
                //申請区分が「新規登録申請」か判定
                int structureId = historyManagement.getApplicationDivision(TMQConst.MsStructure.StructureId.ApplicationDivision.New);
                bool isNew = history.ApplicationDivisionId == structureId;
                //変更管理テーブル更新処理
                (bool flg, ComDao.HmHistoryManagementEntity entity) result = updateHistoryManagement(registInfo.HistoryManagementId, userId, now, isNew, registInfo.LocationStructureId ?? -1);
                if (!result.flg)
                {
                    return false;
                }

                //変更管理詳細の情報を取得
                List<ComDao.HmMcManagementStandardsContentEntity> list = TMQUtil.SqlExecuteClass.SelectList<ComDao.HmMcManagementStandardsContentEntity>(SqlName.List.GetHistoryManagementDetail, SqlName.List.SubDir, result.entity, this.db);
                if (list == null || list.Count == 0)
                {
                    return false;
                }
                //長期計画の新規登録または更新の変更管理が存在するか
                historyManagementId = list.Where(x => x.ExecutionDivision == ExecutionDivision.NewLongPlan || x.ExecutionDivision == ExecutionDivision.UpdateLongPlan).Select(x => x.HistoryManagementId).FirstOrDefault();
                if (historyManagementId != null && historyManagementId > 0)
                {
                    //新規登録（複写）申請した長期計画または変更申請された長期計画を修正

                    ComDao.HmLnLongPlanEntity entity = setHmLnLongPlanEntity(registInfo);
                    //長計件名変更管理の更新
                    return TMQUtil.SqlExecuteClass.Regist(SqlName.Edit.UpdateHmLongPlan, SqlName.Edit.SubDir, entity, this.db);
                }
                else
                {
                    //保全項目一覧を変更（追加、削除）した長期計画を修正

                    //実行処理区分
                    registInfo.ExecutionDivision = ExecutionDivision.UpdateLongPlan;
                    ComDao.HmLnLongPlanEntity entity = setHmLnLongPlanEntity(registInfo);
                    //長計件名変更管理の登録
                    return TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertHmLnLongPlan, SqlName.Detail.SubDir, entity, this.db, "", new List<string> { "New" });
                }
            }

            // 新規に登録する長期計画件名IDを採番する
            long getNewLongPlanId()
            {
                // SQL実行
                ComDao.HmLnLongPlanEntity result = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.HmLnLongPlanEntity>(SqlName.Edit.GetNewLongPlanId, SqlName.Edit.SubDir, null, this.db);
                if (result == null)
                {
                    return -1;
                }

                // 新規採番した機番ID
                return result.LongPlanId;
            }

            //長計件名変更管理テーブル 登録処理
            ComDao.HmLnLongPlanEntity setHmLnLongPlanEntity(Dao.ListSearchResult registInfo)
            {
                // 取得した長計件名の値を長計件名変更管理データクラスに設定する
                ComDao.HmLnLongPlanEntity historyEntity = new();
                historyEntity.HistoryManagementId = registInfo.HistoryManagementId;
                historyEntity.LongPlanId = registInfo.LongPlanId;
                historyEntity.ExecutionDivision = registInfo.ExecutionDivision;
                historyEntity.Subject = registInfo.Subject;
                historyEntity.LocationStructureId = registInfo.LocationStructureId;
                historyEntity.LocationDistrictStructureId = registInfo.LocationDistrictStructureId;
                historyEntity.LocationFactoryStructureId = registInfo.LocationFactoryStructureId;
                historyEntity.LocationPlantStructureId = registInfo.LocationPlantStructureId;
                historyEntity.LocationSeriesStructureId = registInfo.LocationSeriesStructureId;
                historyEntity.LocationStrokeStructureId = registInfo.LocationStrokeStructureId;
                historyEntity.LocationFacilityStructureId = registInfo.LocationFacilityStructureId;
                historyEntity.JobStructureId = registInfo.JobStructureId;
                historyEntity.JobKindStructureId = registInfo.JobKindStructureId;
                historyEntity.JobLargeClassficationStructureId = registInfo.JobLargeClassficationStructureId;
                historyEntity.JobMiddleClassficationStructureId = registInfo.JobMiddleClassficationStructureId;
                historyEntity.JobSmallClassficationStructureId = registInfo.JobSmallClassficationStructureId;
                historyEntity.SubjectNote = registInfo.SubjectNote;
                historyEntity.PersonId = registInfo.PersonId;
                historyEntity.PersonName = registInfo.PersonName;
                historyEntity.WorkItemStructureId = registInfo.WorkItemStructureId;
                historyEntity.BudgetManagementStructureId = registInfo.BudgetManagementStructureId;
                historyEntity.BudgetPersonalityStructureId = registInfo.BudgetPersonalityStructureId;
                historyEntity.MaintenanceSeasonStructureId = registInfo.MaintenanceSeasonStructureId;
                historyEntity.PurposeStructureId = registInfo.PurposeStructureId;
                historyEntity.WorkClassStructureId = registInfo.WorkClassStructureId;
                historyEntity.TreatmentStructureId = registInfo.TreatmentStructureId;
                historyEntity.FacilityStructureId = registInfo.FacilityStructureId;
                historyEntity.LongPlanDivisionStructureId = registInfo.LongPlanDivisionStructureId;
                historyEntity.LongPlanGroupStructureId = registInfo.LongPlanGroupStructureId;

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon<ComDao.HmLnLongPlanEntity>(ref historyEntity, now, userId, userId);

                return historyEntity;
            }

        }
    }
}
