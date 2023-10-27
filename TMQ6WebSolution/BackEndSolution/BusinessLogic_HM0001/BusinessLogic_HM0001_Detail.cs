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
using Dao = BusinessLogic_HM0001.BusinessLogicDataClass_HM0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConst = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_HM0001
{
    /// <summary>
    /// 変更管理 機器台帳 詳細画面
    /// </summary>
    public partial class BusinessLogic_HM0001 : CommonBusinessLogicBase
    {
        #region 定数
        #endregion

        #region 検索
        /// <summary>
        /// 詳細画面検索処理
        /// </summary>
        /// <param name="historyManagementId">変更管理ID(再検索以外はNULL)</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchDetailList(long? historyManagementId = null)
        {
            // 検索条件を取得
            Dao.detailSearchCondition condition = getDetailSearchCondition(historyManagementId);

            // 機番情報・機器情報
            if (!searchMachineAndEquipmentInfo(condition))
            {
                return false;
            }

            //　保全項目一覧
            if (!searchManagementStandardsList(condition))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 検索条件を取得
        /// </summary>
        /// <param name="historyManagementId">変更管理ID(再検索以外はNULL)</param>
        /// <returns>検索条件</returns>
        private Dao.detailSearchCondition getDetailSearchCondition(long? historyManagementId = null)
        {
            // 引数の変更管理IDがNULLでない(再検索処理の)場合
            if (historyManagementId != null)
            {
                Dao.detailSearchCondition researchCondition = new();
                researchCondition.HistoryManagementId = (long)historyManagementId;                         // 変更管理ID
                researchCondition.LanguageId = this.LanguageId;                                            // 言語ID
                researchCondition.UserId = int.Parse(this.UserId);                                         // ログインユーザID
                researchCondition.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.history; // 処理モード

                return researchCondition;
            }

            List<Dictionary<string, object>> conditionDictionary = new();
            string fromCtrlId = string.Empty;

            // 遷移元のCtrlIdを判定
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId);
            if (compareId.IsStartId("checkIsInProgress"))
            {
                // 変更申請・承認依頼・申請内容修正ボタン押下時のAjax通信
                conditionDictionary = this.resultInfoDictionary;
                fromCtrlId = ConductInfo.FormDetail.ControlId.Machine;
            }
            else if (compareId.IsStartId(ConductInfo.FormEdit.ButtonId.Regist))
            {
                // 詳細編集画面の登録ボタンクリック
                conditionDictionary = this.searchConditionDictionary;
                fromCtrlId = ConductInfo.FormEdit.ControlId.Machine;
            }
            else if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.CopyRequest) ||
                    compareId.IsStartId(ConductInfo.FormDetail.ButtonId.ChangeRequest) ||
                    compareId.IsStartId(ConductInfo.FormDetail.ButtonId.EditRequest) ||
                    compareId.IsStartId(ConductInfo.FormEdit.ButtonId.Back))
            {
                // 詳細画面の複写申請ボタンクリック
                // 詳細画面の変更申請ボタンクリック
                // 詳細画面の申請内容修正ボタンクリック
                // 詳細編集画面の戻るボタンクリック
                conditionDictionary = this.searchConditionDictionary;
                fromCtrlId = ConductInfo.FormDetail.ControlId.Machine;
            }
            else
            {
                // 一覧画面の詳細リンククリック
                // 機器台帳(MC0001)詳細画面からの遷移
                conditionDictionary = this.searchConditionDictionary;
                fromCtrlId = ConductInfo.FormList.ControlId.List;
            }

            // 検索条件を作成
            Dao.detailSearchCondition condition = new();
            var targetDic = ComUtil.GetDictionaryByCtrlId(conditionDictionary, fromCtrlId);

            // 変更管理ID、機番IDを取得
            SetDataClassFromDictionary(targetDic, fromCtrlId, condition, new List<string> { "HistoryManagementId", "MachineId" });
            condition.LanguageId = this.LanguageId;    // 言語ID
            condition.UserId = int.Parse(this.UserId); // ログインユーザID

            // 変更管理IDの有無で処理モードを設定
            if (condition.HistoryManagementId <= 0)
            {
                // 変更管理IDなし の場合、トランザクションモード
                condition.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction;
            }
            else
            {
                // 変更管理IDあり の場合、変更管理モード
                condition.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.history;
            }

            return condition;
        }

        /// <summary>
        /// 機番情報・機器情報 検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchMachineAndEquipmentInfo(Dao.detailSearchCondition condition)
        {
            // 表示するデータタイプに応じたSQLを取得;
            string withSql = string.Empty;
            string sql = string.Empty;

            // 処理モードを判定
            switch (condition.ProcessMode)
            {
                case (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction: // トランザクションモード

                    // SQL取得
                    TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDirMachine, SqlName.Detail.GetMachineDetail, out string traSql);
                    sql = traSql;
                    break;

                case (int)TMQConst.MsStructure.StructureId.ProcessMode.history: // 変更管理モード

                    // SQL取得
                    TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.List.GetHistoryMachineList, out string hisSql);
                    TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.List.GetHistoryMachineList, out string outWithSql, new List<string>() { "IsDetail" });
                    sql = hisSql;
                    withSql = outWithSql;
                    break;
                default:

                    // 該当しない場合はエラー
                    return false;
            }

            // SQL実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(withSql + sql, condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定(変更管理データ・トランザクションデータ どちらも設定する)
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job, StructureType.OldLocation, StructureType.OldJob }, this.db, this.LanguageId, true);

            // 申請状況の拡張項目を取得
            results[0].ApplicationStatusCode = getApplicationStatusCode(condition, condition.ProcessMode == (int)TMQConst.MsStructure.StructureId.ProcessMode.history);

            // 変更があった項目を取得(変更管理モードの場合)
            if (condition.ProcessMode == (int)TMQConst.MsStructure.StructureId.ProcessMode.history)
            {
                // 変更があった項目を取得
                TMQUtil.HistoryManagement.setValueChangedItem<Dao.searchResult>(results);
            }

            // ボタン表示/非表示フラグを取得
            GetIsAbleToClickBtn(results[0], condition);

            // 処理モードを検索結果に設定
            results[0].ProcessMode = condition.ProcessMode;

            // 検索結果の設定(機番情報)
            if (!setSearchResult(ConductInfo.FormDetail.GroupNoMachine, results))
            {
                return false;
            }

            // 検索結果の設定(機器情報)
            if (!setSearchResult(ConductInfo.FormDetail.GroupNoEquipment, results))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保全項目一覧 検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchManagementStandardsList(Dao.detailSearchCondition condition)
        {
            // 表示するデータタイプに応じたSQLを取得
            string sqlName = string.Empty; // SQLファイル名
            string subDir = string.Empty;  // サブディレクトリ名

            // 処理モードを判定
            switch (condition.ProcessMode)
            {
                case (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction: // トランザクションモード
                    subDir = SqlName.SubDirMachine;
                    sqlName = SqlName.Detail.GetManagementStandard;
                    break;

                case (int)TMQConst.MsStructure.StructureId.ProcessMode.history: // 変更管理モード
                    subDir = SqlName.SubDir;
                    sqlName = SqlName.Detail.GetHistoryManagementStandardsList;
                    break;

                default:
                    // 該当しない場合はエラー
                    return false;
            }

            // SQL取得
            TMQUtil.GetFixedSqlStatement(subDir, sqlName, out string sql);

            // SQL実行
            IList<Dao.managementStandardsResult> results = db.GetListByDataClass<Dao.managementStandardsResult>(sql, condition);
            if (results == null || results.Count == 0)
            {
                return true;
            }

            // 検索結果を一覧にを設定
            if (!SetFormByDataClass(ConductInfo.FormDetail.ControlId.ManagementStandardsList, results))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///  ボタン非表示制御フラグ取得
        /// </summary>
        /// <param name="result">詳細画面の検索結果</param>
        /// <param name="condition">詳細画面の検索条件</param>
        private void GetIsAbleToClickBtn(Dao.searchResult result, Dao.detailSearchCondition condition)
        {
            // ①申請の申請者IDがログインユーザまたはシステム管理者かどうか
            // ②// 変更管理IDが紐付く機番情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDかどうか
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
            historyManagement.GetFlgHideButton(ref result, condition.HistoryManagementId, (TMQConst.MsStructure.StructureId.ProcessMode)Enum.ToObject(typeof(TMQConst.MsStructure.StructureId.ProcessMode), condition.ProcessMode));
        }
        #endregion

        #region 削除申請
        /// <summary>
        /// 削除申請
        /// </summary>
        /// <returns>エラーの場合-1、正常の場合1</returns>
        public int deleteRequest()
        {
            // 一覧の表示情報取得
            Dao.searchResult searchResult = getRegistInfo<Dao.searchResult>(ConductInfo.FormDetail.GroupNoMachine, DateTime.Now);

            // 排他チェック
            if (!deleteRequestCheckExclusiveSingle(ConductInfo.FormDetail.ControlId.Machine, new List<string>() { "mc_machine", "mc_equipment" }))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 入力チェック
            if (deleteRequestIsError(searchResult.MachineId))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 変更管理テーブル・変更管理詳細テーブル 登録処理
            ComDao.HmHistoryManagementEntity historyInfo = new();
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);

            // SQL実行(申請区分は「削除申請」)
            (bool returnFlag, long historyManagementId, long historyManagementDetailId) historyManagementResult =
                historyManagement.InsertHistoryManagementBaseTable(TMQConst.MsStructure.StructureId.ApplicationDivision.Delete,
                                                                   (int)executionDiv.machineDelete,
                                                                   searchResult.MachineId,
                                                                   getFactoryId(historyManagement, searchResult.MachineId));
            // 登録に失敗した場合は終了
            if (!historyManagementResult.returnFlag)
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 以下は削除する情報を各変更管理テーブルに登録する
            DateTime now = DateTime.Now;

            // 機番情報変更管理テーブル 登録処理
            if (!registHmMcMachine(historyManagementResult.historyManagementDetailId, searchResult.MachineId, now))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機器情報変更管理テーブル 登録処理
            if (!registHmMcEquipment(historyManagementResult.historyManagementDetailId, searchResult.EquipmentId, now))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 適用法規情報変更管理テーブル 登録処理
            if (!registHmMcApplicableLaws(historyManagementResult.historyManagementDetailId, searchResult.MachineId, now))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機器別管理基準部位変更管理テーブル 登録処理
            if (!registHmMcManagementStandardsComponent(historyManagementResult.historyManagementDetailId, searchResult.MachineId, now, out List<long> componentIdList))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機器別管理基準内容変更管理テーブル 登録処理
            if (!registHmMcManagementStandardsContent(historyManagementResult.historyManagementDetailId, now, componentIdList, out List<long> contentIdList))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 保全スケジュール変更管理テーブル 登録処理
            if (!registMaintainanceSchedule(historyManagementResult.historyManagementDetailId, now, contentIdList))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 詳細画面再検索処理
            if (!searchDetailList(historyManagementResult.historyManagementId))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        #region 排他チェック

        #endregion

        #region 入力チェック
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>エラーの場合はTrue</returns>
        private bool deleteRequestIsError(long machineId)
        {
            // 機器に対する「承認済み」以外のデータ存在チェック
            if (!checkHistoryManagement(machineId))
            {
                return true;
            }

            // 長期計画存在チェック
            if (!checkLongPlan(machineId))
            {
                return true;
            }

            //保全活動存在チェック
            if (!checkMsSummary(machineId))
            {
                return true;
            }

            // 構成存在チェック
            if (!checkComposition(machineId, true))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 機器に対する「承認済み」以外のデータ存在チェック
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>存在しない:True 存在する:False</returns>
        private bool checkHistoryManagement(long machineId)
        {
            // 承認済み」以外のデータ件数取得
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
            if (historyManagement.getApplicationStatusCntByKeyId(machineId, TMQConst.MsStructure.StructureId.ApplicationStatus.Approved, false) > 0)
            {
                // 「 対象データに申請が存在するため処理を行えません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160016 });
                return false;
            }

            return true;
        }

        /// <summary>
        /// 長期計画存在チェック
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>存在しない:True 存在する:False</returns>
        private bool checkLongPlan(long machineId)
        {
            // 検索SQL文の取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDirMachine, SqlName.Detail.GetChkLongPlan, out string sql);

            // 総件数を取得
            int cnt = db.GetCount(sql, new { MachineId = machineId });
            if (cnt > 0)
            {
                // 「 長期計画で使用される機器の為、削除できません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141170001 });
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保全活動存在チェック
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>存在しない:True 存在する:False</returns>
        private bool checkMsSummary(long machineId)
        {
            // 検索SQL文の取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDirMachine, SqlName.Detail.GetChkMsSummary, out string sql);

            // 総件数を取得
            int cnt = db.GetCount(sql, new { MachineId = machineId });
            if (cnt > 0)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「 保全活動で使用される機器の為、削除できません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141300003 });
                return false;
            }

            return true;
        }

        /// <summary>
        /// 構成存在チェック
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <param name="isSetFormMsgId">画面のメッセージに設定する場合はTrue</param>
        /// <returns>存在しない:True 存在する:False</returns>
        private bool checkComposition(long machineId, bool isSetFormMsgId)
        {
            // 親子構成、ループ構成、付属品構成 を検索するSQL
            string[] sqlIdList = new string[] { SqlName.Detail.GetChkParentInfo, SqlName.Detail.GetChkLoopInfo, SqlName.Detail.GetChkAccessoryInfo };

            foreach (string sqlId in sqlIdList)
            {
                // 構成検索SQL文の取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDirMachine, SqlName.Detail.GetChkParentInfo, out string sql);

                // 総件数を取得
                int cnt = db.GetCount(sql, new { MachineId = machineId });

                // 構成が存在する場合
                if (cnt > 0)
                {
                    // 引数のフラグに応じてエラーメッセージを画面のメッセージに設定する
                    if (isSetFormMsgId)
                    {
                        // 「 構成機器が登録されている機器の為、削除できません。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141070001 });
                    }
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 変更管理テーブル登録処理
        /// <summary>
        /// 機番情報変更管理テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementDetailId">変更管理詳細ID</param>
        /// <param name="machineId">機番ID</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHmMcMachine(long historyManagementDetailId, long machineId, DateTime now)
        {
            // 機番IDより機番情報を取得する
            ComDao.McMachineEntity machineEntity = new();
            machineEntity = machineEntity.GetEntity(machineId, this.db);

            // 取得した機番情報の値を機番情報変更管理データクラスに設定する
            ComDao.HmMcMachineEntity historyMachineEntity = new();
            historyMachineEntity.HistoryManagementDetailId = historyManagementDetailId;               // 変更管理詳細ID
            historyMachineEntity.MachineId = machineEntity.MachineId;                                 // 機番ID
            historyMachineEntity.LocationStructureId = machineEntity.LocationStructureId;             // 機能場所階層ID
            historyMachineEntity.JobStructureId = machineEntity.JobStructureId;                       // 職種機種階層ID
            historyMachineEntity.MachineNo = machineEntity.MachineNo;                                 // 機器番号
            historyMachineEntity.MachineName = machineEntity.MachineName;                             // 機器毎証
            historyMachineEntity.InstallationLocation = machineEntity.InstallationLocation;           // 設置場所
            historyMachineEntity.NumberOfInstallation = machineEntity.NumberOfInstallation;           // 設置台数
            historyMachineEntity.EquipmentLevelStructureId = machineEntity.EquipmentLevelStructureId; // 機器レベル
            historyMachineEntity.DateOfInstallation = machineEntity.DateOfInstallation;               // 設置日
            historyMachineEntity.ImportanceStructureId = machineEntity.ImportanceStructureId;         // 重要度
            historyMachineEntity.ConservationStructureId = machineEntity.ConservationStructureId;     // 保全方式
            historyMachineEntity.MachineNote = machineEntity.MachineNote;                             // 機番メモ

            // テーブル共通項目を設定
            setCommonItem(ref historyMachineEntity, now);

            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Edit.InsertMachineInfo, out string sql, new List<string>() { UnCommentItem.DefaultMachineId });

            // SQL実行
            if (db.Regist(sql, historyMachineEntity) <= 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 機器情報変更管理テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementDetailId">変更管理詳細ID</param>
        /// <param name="equipmentId">機器ID</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHmMcEquipment(long historyManagementDetailId, long equipmentId, DateTime now)
        {
            // 機器IDより機器情報を取得する
            ComDao.McEquipmentEntity equipmentEntity = new();
            equipmentEntity = equipmentEntity.GetEntity(equipmentId, this.db);

            // 取得した機器情報の値を機器情報変更管理データクラスに設定する
            ComDao.HmMcEquipmentEntity historyEquipmentEntity = new();
            historyEquipmentEntity.HistoryManagementDetailId = historyManagementDetailId;             // 変更管理詳細ID
            historyEquipmentEntity.EquipmentId = equipmentEntity.EquipmentId;                         // 機器ID
            historyEquipmentEntity.MachineId = equipmentEntity.MachineId;                             // 機番ID
            historyEquipmentEntity.CirculationTargetFlg = equipmentEntity.CirculationTargetFlg;       // 循環対象
            historyEquipmentEntity.ManufacturerStructureId = equipmentEntity.ManufacturerStructureId; // メーカー
            historyEquipmentEntity.ManufacturerType = equipmentEntity.ManufacturerType;               // メーカー型式
            historyEquipmentEntity.ModelNo = equipmentEntity.ModelNo;                                 // 型式コード
            historyEquipmentEntity.SerialNo = equipmentEntity.SerialNo;                               // シリアル番号
            historyEquipmentEntity.DateOfManufacture = equipmentEntity.DateOfManufacture;             // 製造日
            historyEquipmentEntity.DeliveryDate = equipmentEntity.DeliveryDate;                       // 納期
            historyEquipmentEntity.EquipmentNote = equipmentEntity.EquipmentNote;                     // 機器メモ
            historyEquipmentEntity.UseSegmentStructureId = equipmentEntity.UseSegmentStructureId;     // 使用区分
            historyEquipmentEntity.FixedAssetNo = equipmentEntity.FixedAssetNo;                       // 固定資産番号
            historyEquipmentEntity.MaintainanceKindManage = equipmentEntity.MaintainanceKindManage;   // 点検種別毎管理

            // テーブル共通項目を設定
            setCommonItem(ref historyEquipmentEntity, now);

            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Edit.InsertEquipmentInfo, out string sql, new List<string>() { UnCommentItem.DefaultEquipmentId });

            // SQL実行
            if (db.Regist(sql, historyEquipmentEntity) <= 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 適用法規情報変更管理テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementDetailId">変更管理詳細ID</param>
        /// <param name="machineId">機番ID</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHmMcApplicableLaws(long historyManagementDetailId, long machineId, DateTime now)
        {
            // 検索条件を作成
            ComDao.McApplicableLawsEntity condition = new();
            condition.MachineId = machineId; // 機番ID

            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetApplicableLaws, out string sql);

            // SQL実行
            IList<ComDao.McApplicableLawsEntity> results = db.GetListByDataClass<ComDao.McApplicableLawsEntity>(sql, condition);
            if (results == null || results.Count == 0)
            {
                // データが紐付いていない場合もあるので取得できなくてもtrueを返す
                return true;
            }

            // 登録用SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.InsertApplicableLawsInfo, out string insertSql);
            // 取得したデータを適用法規情報変更管理テーブルに登録
            foreach (ComDao.McApplicableLawsEntity result in results)
            {
                ComDao.HmMcApplicableLawsEntity historyApplicableLawsEntity = new();
                historyApplicableLawsEntity.HistoryManagementDetailId = historyManagementDetailId;        // 変更管理詳細ID
                historyApplicableLawsEntity.ApplicableLawsId = result.ApplicableLawsId;                   // 適用法規ID
                historyApplicableLawsEntity.ApplicableLawsStructureId = result.ApplicableLawsStructureId; // 適用法規アイテムID
                historyApplicableLawsEntity.MachineId = result.MachineId;                                 // 機番ID

                // テーブル共通項目を設定
                setCommonItem(ref historyApplicableLawsEntity, now);

                // SQL実行
                if (db.Regist(insertSql, historyApplicableLawsEntity) <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 機器別管理基準部位変更管理テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementDetailId">変更管理詳細ID</param>
        /// <param name="machineId">機番ID</param>
        /// <param name="now">現在日時</param>
        /// <param name="componentIdList">機器別管理基準部位IDリスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHmMcManagementStandardsComponent(long historyManagementDetailId, long machineId, DateTime now, out List<long> componentIdList)
        {
            componentIdList = new();

            // 検索条件を作成
            ComDao.McManagementStandardsComponentEntity condition = new();
            condition.MachineId = machineId; // 機番ID

            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetManagementStandardsComponent, out string sql);

            // SQL実行
            IList<ComDao.McManagementStandardsComponentEntity> results = db.GetListByDataClass<ComDao.McManagementStandardsComponentEntity>(sql, condition);
            if (results == null || results.Count == 0)
            {
                // データが紐付いていない場合もあるので取得できなくてもtrueを返す
                return true;
            }

            // 機器別管理基準部位変更管理テーブル登録用SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.InsertManagementStandardsComponentInfo, out string insertSql);
            // 取得したデータを機器別管理基準部位変更管理テーブルに登録
            foreach (ComDao.McManagementStandardsComponentEntity result in results)
            {
                // 機器別管理基準部位IDを返り値のリストに格納
                componentIdList.Add(result.ManagementStandardsComponentId);

                ComDao.HmMcManagementStandardsComponentEntity historyComponentEntity = new();
                historyComponentEntity.HistoryManagementDetailId = historyManagementDetailId;                    // 変更管理詳細ID
                historyComponentEntity.ManagementStandardsComponentId = result.ManagementStandardsComponentId;   // 機器別管理基準部位ID
                historyComponentEntity.MachineId = result.MachineId;                                             // 機番ID
                historyComponentEntity.InspectionSiteStructureId = result.InspectionSiteStructureId;             // 部位ID
                historyComponentEntity.IsManagementStandardConponent = result.IsManagementStandardConponent;     // 機器別管理基準フラグ

                // テーブル共通項目を設定
                setCommonItem(ref historyComponentEntity, now);

                // SQL実行
                if (db.Regist(insertSql, historyComponentEntity) <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 機器別管理基準部内容変更管理テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementDetailId">変更管理詳細ID</param>
        /// <param name="now">現在日時</param>
        /// <param name="componentIdList">機器別管理基準部位IDリスト</param>
        /// <param name="contentIdList">機器別管理基準内容IDリスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHmMcManagementStandardsContent(long historyManagementDetailId, DateTime now, List<long> componentIdList, out List<long> contentIdList)
        {
            contentIdList = new();

            // 機器別管理基準内容データを取得するSQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetManagementStandardsContent, out string sql);
            // 機器別管理基準内容変更管理テーブル登録用SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.InsertManagementStandardsContentInfo, out string insertSql);

            // 機器別管理基準部位IDより機器別管理基準内容のデータを取得
            foreach (long componentId in componentIdList)
            {
                // 検索条件を作成
                ComDao.McManagementStandardsContentEntity condition = new();
                condition.ManagementStandardsComponentId = componentId; // 機器別管理基準部位ID

                // SQL実行
                IList<ComDao.McManagementStandardsContentEntity> results = db.GetListByDataClass<ComDao.McManagementStandardsContentEntity>(sql, condition);
                if (results == null || results.Count == 0)
                {
                    // データが紐付いていない場合もあるのでエラーにはしない
                    break;
                }

                // 取得したデータを機器別管理基準内容変更管理テーブルに登録
                foreach (ComDao.McManagementStandardsContentEntity result in results)
                {
                    // 機器別管理基準部位IDを返り値のリストに格納
                    contentIdList.Add(result.ManagementStandardsContentId);

                    ComDao.HmMcManagementStandardsContentEntity historyContentEntity = new();
                    historyContentEntity.HistoryManagementDetailId = historyManagementDetailId;                                // 変更管理詳細ID
                    historyContentEntity.ManagementStandardsContentId = result.ManagementStandardsContentId;                   // 機器別管理基準内容ID
                    historyContentEntity.ManagementStandardsComponentId = componentId;                                         // 機器別管理基準部位ID
                    historyContentEntity.InspectionContentStructureId = result.InspectionContentStructureId;                   // 点検内容ID
                    historyContentEntity.InspectionSiteImportanceStructureId = result.InspectionSiteImportanceStructureId;     // 部位重要度
                    historyContentEntity.InspectionSiteConservationStructureId = result.InspectionSiteConservationStructureId; // 部位保全方式
                    historyContentEntity.MaintainanceDivision = result.MaintainanceDivision;                                   // 保全区分
                    historyContentEntity.MaintainanceKindStructureId = result.MaintainanceKindStructureId;                     // 点検種別
                    historyContentEntity.BudgetAmount = result.BudgetAmount;                                                   // 予算金額
                    historyContentEntity.PreparationPeriod = result.PreparationPeriod;                                         // 準備期間(日)
                    historyContentEntity.LongPlanId = result.LongPlanId;                                                       // 長計件名ID
                    historyContentEntity.OrderNo = result.OrderNo;                                                             // 並び順
                    historyContentEntity.ScheduleTypeStructureId = result.ScheduleTypeStructureId;                             // スケジュール管理基準ID

                    // テーブル共通項目を設定
                    setCommonItem(ref historyContentEntity, now);

                    // SQL実行
                    if (db.Regist(insertSql, historyContentEntity) <= 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 保全スケジュール変更管理テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementDetailId">変更管理詳細ID</param>
        /// <param name="now">現在日時</param>
        /// <param name="contentIdList">機器別管理基準内容IDリスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registMaintainanceSchedule(long historyManagementDetailId, DateTime now, List<long> contentIdList)
        {

            // 保全スケジュールデータを取得するSQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetMaintainanceSchedule, out string sql);
            // 保全スケジュール変更管理テーブル登録用SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.InsertMaintainanceScheduleInfo, out string insertSql);

            // 機器別管理基準内容IDに紐付く保全スケジュールを取得
            foreach (long contentId in contentIdList)
            {
                ComDao.McMaintainanceScheduleEntity condition = new();
                condition.ManagementStandardsContentId = contentId;

                // SQL実行
                IList<ComDao.McMaintainanceScheduleEntity> results = db.GetListByDataClass<ComDao.McMaintainanceScheduleEntity>(sql, condition);
                if (results == null || results.Count == 0)
                {
                    // データが紐付いていない場合もあるのでエラーにはしない
                    break;
                }

                // 取得したデータを保全スケジュール変更管理テーブルに登録
                foreach (ComDao.McMaintainanceScheduleEntity result in results)
                {
                    ComDao.HmMcMaintainanceScheduleEntity historyScheduleEntity = new();
                    historyScheduleEntity.HistoryManagementDetailId = historyManagementDetailId;              // 変更管理詳細ID
                    historyScheduleEntity.MaintainanceScheduleId = result.MaintainanceScheduleId;             // 保全スケジュールID
                    historyScheduleEntity.ManagementStandardsContentId = result.ManagementStandardsContentId; // 機器別管理基準内容ID
                    historyScheduleEntity.IsCyclic = result.IsCyclic;                                         // 周期ありフラグ
                    historyScheduleEntity.CycleYear = result.CycleYear;                                       // 周期(年)
                    historyScheduleEntity.CycleMonth = result.CycleMonth;                                     // 周期(月)
                    historyScheduleEntity.CycleDay = result.CycleDay;                                         // 周期(日)
                    historyScheduleEntity.DispCycle = result.DispCycle;                                       // 表示周期
                    historyScheduleEntity.StartDate = result.StartDate;                                       // 開始日

                    // テーブル共通項目を設定
                    setCommonItem(ref historyScheduleEntity, now);

                    // SQL実行
                    if (db.Regist(insertSql, historyScheduleEntity) <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// テーブル共通項目を設定する
        /// </summary>
        /// <typeparam name="T">テーブル共通クラス</typeparam>
        /// <param name="result">登録データクラス</param>
        /// <param name="now">現在日時</param>
        private void setCommonItem<T>(ref T result, DateTime now)
            where T : ComDao.CommonTableItem, new()
        {
            result.InsertUserId = int.Parse(this.UserId); // 登録者ID
            result.InsertDatetime = now;                  // 登録日時
            result.UpdateUserId = int.Parse(this.UserId); // 更新者ID
            result.UpdateDatetime = now;                  // 更新日時
        }
        #endregion
        #endregion

        #region 申請内容取消・承認依頼引戻・承認 共通処理
        /// <summary>
        /// 申請内容取消・承認依頼引戻・承認 共通処理
        /// </summary>
        /// <param name="historyManagementId">画面に表示されている変更管理ID</param>
        /// <param name="applicationStatus">登録する申請状況</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool detailCommonRegist(out long historyManagementId, int? applicationStatus = null)
        {
            // 画面の表示内容取得
            Dao.searchResult searchResult = getRegistInfo<Dao.searchResult>(ConductInfo.FormDetail.GroupNoMachine, DateTime.Now);

            // 画面に表示している変更管理IDを返り値に設定
            historyManagementId = searchResult.HistoryManagementId;

            // 排他チェック
            if (!checkExclusiveSingle(ConductInfo.FormDetail.ControlId.Machine))
            {
                return false;
            }

            // 登録する申請状況がNUll(申請内容取消)の場合はここで処理終了
            if (applicationStatus == null)
            {
                return true;
            }

            // 登録情報を作成
            ComDao.HmHistoryManagementEntity condition = new();
            condition.HistoryManagementId = searchResult.HistoryManagementId; // 変更管理ID

            // 登録処理
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
            if (!historyManagement.UpdateApplicationStatus(condition, (TMQConst.MsStructure.StructureId.ApplicationStatus)Enum.ToObject(typeof(TMQConst.MsStructure.StructureId.ApplicationStatus), applicationStatus)))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 申請内容取消
        /// <summary>
        /// 申請内容取消
        /// </summary>
        /// <returns>エラーの場合-1、正常の場合1</returns>
        public int cancelRequest()
        {
            // 排他チェック
            if (!detailCommonRegist(out long historoyManagementId))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 変更管理テーブル、変更管理詳細テーブル 削除処理
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
            if (!historyManagement.DeleteHistoryManagement(historoyManagementId))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region 承認依頼引戻
        /// <summary>
        /// 承認依頼引戻
        /// </summary>
        /// <returns>エラーの場合-1、正常の場合1</returns>
        public int pullBackRequest()
        {
            // 申請状況を「差戻中」に更新
            if (!detailCommonRegist(out long historoyManagementId, (int)TMQConst.MsStructure.StructureId.ApplicationStatus.Return))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 詳細画面再検索処理
            if (!searchDetailList(historoyManagementId))
            {
                return ComConsts.RETURN_RESULT.NG;
            }
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region 承認
        /// <summary>
        /// 承認
        /// </summary>
        /// <returns>エラーの場合-1、正常の場合1</returns>
        public int changeApplicationApproval()
        {
            // 申請状況を「承認済」に更新
            if (!detailCommonRegist(out long historoyManagementId, (int)TMQConst.MsStructure.StructureId.ApplicationStatus.Approved))
            {
                return ComConsts.RETURN_RESULT.NG;
            }











            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion
    }
}
