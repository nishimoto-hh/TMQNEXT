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
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_HM0001.BusinessLogicDataClass_HM0001;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_HM0001
{
    /// <summary>
    /// 変更管理 機器台帳 詳細画面
    /// </summary>
    public partial class BusinessLogic_HM0001 : CommonBusinessLogicBase
    {
        #region 検索
        /// <summary>
        /// 詳細画面検索処理
        /// </summary>
        /// <param name="historyManagementId">変更管理ID(再検索以外はNULL)</param>
        /// <param name="isInit">初期検索の場合のみTrue</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchDetailList(long? historyManagementId = null, long? machineId = null, bool isInit = false)
        {
            // 検索条件を取得
            Dao.detailSearchCondition condition = getDetailSearchCondition(historyManagementId, machineId, isInit);

            // 翻訳の一時テーブルを作成
            createTranslationTempTbl();

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

            // ★画面定義の翻訳情報取得★
            int factoryId = 0;
            ComDao.HmHistoryManagementEntity historyInfo = new ComDao.HmHistoryManagementEntity().GetEntity(condition.HistoryManagementId, this.db);
            if (historyInfo == null)
            {
                // 機番情報の工場ID
                ComDao.McMachineEntity machineInfo = new ComDao.McMachineEntity().GetEntity(condition.MachineId, this.db);
                factoryId = Convert.ToInt32(machineInfo.LocationFactoryStructureId);
            }
            else
            {
                // 変更管理情報の工場ID
                factoryId = historyInfo.FactoryId;
            }
            GetContorlDefineTransData(factoryId);

            return true;
        }

        /// <summary>
        /// 検索条件を取得
        /// </summary>
        /// <param name="historyManagementId">変更管理ID(再検索以外はNULL)</param>
        /// <param name="isInit">初期検索の場合のみTrue</param>
        /// <returns>検索条件</returns>
        private Dao.detailSearchCondition getDetailSearchCondition(long? historyManagementId = null, long? machineId = null, bool isInit = false)
        {
            // 引数の変更管理IDがNULLでない(再検索処理の)場合
            if (historyManagementId != null)
            {
                Dao.detailSearchCondition researchCondition = new();
                researchCondition.HistoryManagementId = (long)historyManagementId;                         // 変更管理ID
                researchCondition.MachineId = (long)machineId;                                             // 機番ID
                researchCondition.LanguageId = this.LanguageId;                                            // 言語ID
                researchCondition.UserId = int.Parse(this.UserId);                                         // ログインユーザID
                researchCondition.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.history; // 処理モード

                return researchCondition;
            }

            List<Dictionary<string, object>> conditionDictionary = new();
            string fromCtrlId = string.Empty;

            // 遷移元のCtrlIdを判定
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId);
            if (compareId.IsStartId("judgeStatusCntExceptApproved"))
            {
                // 変更申請ボタン押下時のAjax通信
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
                    compareId.IsStartId(ConductInfo.FormDetail.ButtonId.ManagementStandardRegist) ||
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

            // 変更管理IDを引き継がなかった場合、対象の機器で申請状況が「承認済」以外の変更管理IDを取得する
            if (condition.HistoryManagementId <= 0)
            {
                TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
                long? tmpHistoryManagementId = historyManagement.getHistoryManagementIdByKeyId(condition.MachineId);
                if (tmpHistoryManagementId != null && tmpHistoryManagementId > 0)
                {
                    condition.HistoryManagementId = (long)tmpHistoryManagementId;
                }
            }

            // 変更管理IDの有無で処理モードを設定
            if (condition.HistoryManagementId <= 0)
            {
                // 変更管理IDなし の場合、トランザクションモード
                condition.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction;

                // 初期表示の場合はメッセージを表示
                if (isInit)
                {
                    // 表示されている機器に対し行う変更を登録してください。
                    this.MsgId = GetResMessage(ComRes.ID.ID141270003);
                }
            }
            else
            {
                // 変更管理IDあり の場合、変更管理モード
                condition.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.history;

                //保全項目一覧の単票以外で初期検索の場合
                if (!compareId.IsStartId(ConductInfo.FormDetail.ButtonId.ManagementStandardRegist) && isInit)
                {
                    //登録された変更管理に対して処理を行います。
                    this.MsgId = GetResMessage(ComRes.ID.ID141200007);
                }
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
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

            // 申請状況の拡張項目を取得
            results[0].ApplicationStatusCode = getApplicationStatusCode(condition);

            //トランザクションモードの場合、申請状況に「申請なし」を設定
            if (condition.ProcessMode == (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction)
            {
                TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
                results[0].ApplicationStatusId = historyManagement.getApplicationStatus(TMQConst.MsStructure.StructureId.ApplicationStatus.None);
            }

            // ボタン表示/非表示フラグを取得
            GetIsAbleToClickBtn(results[0], condition);

            // 処理モードを検索結果に設定
            results[0].ProcessMode = condition.ProcessMode;

            // 検索結果の設定(変更管理情報・機番情報・機器情報)
            if (!setSearchResult(ConductInfo.FormDetail.GroupNoHistory, results) ||
                !setSearchResult(ConductInfo.FormDetail.GroupNoMachine, results) ||
                !setSearchResult(ConductInfo.FormDetail.GroupNoEquipment, results))
            {
                return false;
            }

            return true;

            // 申請状況の拡張項目を取得
            string getApplicationStatusCode(Dao.detailSearchCondition condition)
            {
                TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
                return historyManagement.getApplicationStatusByHistoryManagementId(new ComDao.HmHistoryManagementEntity() { HistoryManagementId = condition.HistoryManagementId });
            }
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
                    subDir = SqlName.SubDir;
                    sqlName = SqlName.Detail.GetManagementStandardOriginal;
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
        private bool deleteRequest()
        {
            // 機番情報・機器情報取得
            DateTime now = DateTime.Now;
            Dao.searchResult registInfo = getRegistInfoBySearchResult(new List<short>() { ConductInfo.FormDetail.GroupNoMachine, ConductInfo.FormDetail.GroupNoEquipment }, now);

            // 階層系の値が名称に設定されているため、IDに設定
            registInfo.DistrictId = ComUtil.ConvertStringToInt(registInfo.DistrictName);                       // 地区
            registInfo.FactoryId = ComUtil.ConvertStringToInt(registInfo.FactoryName);                         // 工場
            registInfo.PlantId = ComUtil.ConvertStringToInt(registInfo.PlantName);                             // プラント
            registInfo.SeriesId = ComUtil.ConvertStringToInt(registInfo.SeriesName);                           // 系列
            registInfo.StrokeId = ComUtil.ConvertStringToInt(registInfo.StrokeName);                           // 工程
            registInfo.FacilityId = ComUtil.ConvertStringToInt(registInfo.FacilityName);                       // 設備
            registInfo.JobId = int.Parse(registInfo.JobName);                                                  // 職種
            registInfo.LargeClassficationId = ComUtil.ConvertStringToInt(registInfo.LargeClassficationName);   // 機種大分類
            registInfo.MiddleClassficationId = ComUtil.ConvertStringToInt(registInfo.MiddleClassficationName); // 機種中分類
            registInfo.SmallClassficationId = ComUtil.ConvertStringToInt(registInfo.SmallClassficationName);   // 機種小分類

            // 排他チェック
            if (!checkExclusiveSingleForTable(ConductInfo.FormDetail.ControlId.Machine, new List<string>() { "mc_machine", "mc_equipment" }))
            {
                return false;
            }

            // 入力チェック
            if (deleteRequestIsError(registInfo.MachineId))
            {
                return false;
            }

            // 変更管理テーブル 登録処理
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);

            // 登録処理
            // 引数1：申請区分は「削除申請」
            // 引数2：機番ID
            // 引数3：画面で入力された内容の場所階層ID
            (bool returnFlag, long historyManagementId) historyManagementResult =
                historyManagement.InsertHistoryManagement(TMQConst.MsStructure.StructureId.ApplicationDivision.Delete,
                                                          registInfo.MachineId,
                                                          historyManagement.getFactoryId((int)registInfo.LocationStructureId));

            // 登録に失敗した場合は終了
            if (!historyManagementResult.returnFlag)
            {
                return false;
            }

            // 新規採番した変更管理IDを設定
            registInfo.HistoryManagementId = historyManagementResult.historyManagementId;

            // 以下は削除する情報を各変更管理テーブルに登録する

            // 機番情報変更管理テーブル 登録処理
            // 実行処理区分を設定
            registInfo.ExecutionDivision = executionDiv.MachineDelete;

            // 機番情報変更管理 登録処理
            if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Edit.InsertMachineInfo, SqlName.SubDir, registInfo, this.db, string.Empty))
            {
                return false;
            }

            // 機器情報変更管理テーブル 登録処理
            if (!registEquipmentInfo(ref registInfo, true))
            {
                return false;
            }

            // 適用法規情報変更管理テーブル 登録処理
            if (!registApplocableLaws(registInfo.MachineId, registInfo.HistoryManagementId, now))
            {
                return false;
            }

            // 機器別管理基準部位変更管理テーブル 登録処理
            if (!registHmMcManagementStandardsComponent(historyManagementResult.historyManagementId, registInfo.MachineId, now, "MachineId", out List<long> componentIdList))
            {
                return false;
            }

            // 機器別管理基準内容変更管理テーブル 登録処理
            if (!registHmMcManagementStandardsContent(historyManagementResult.historyManagementId, now, componentIdList, out List<long> contentIdList))
            {
                return false;
            }

            // 保全スケジュール変更管理テーブル 登録処理
            if (!registMaintainanceSchedule(historyManagementResult.historyManagementId, now, contentIdList))
            {
                return false;
            }

            // 詳細画面再検索処理
            if (!searchDetailList(historyManagementResult.historyManagementId, registInfo.MachineId))
            {
                return false;
            }

            return true;
        }

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
            if (checkLongPlan(machineId))
            {
                return true;
            }

            //保全活動存在チェック
            if (checkMsSummary(machineId))
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
        /// 長期計画存在チェック
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>存在しない:True 存在する:False</returns>
        private bool checkLongPlan(long machineId)
        {
            // 検索SQL文の取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDirMachine, SqlName.Detail.GetChkLongPlan, out string sql);

            // 総件数を取得
            if (db.GetCount(sql, new { MachineId = machineId }) > 0)
            {
                // 「 長期計画で使用される機器の為、削除できません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141170001 });
                return true;
            }

            return false;
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
            if (db.GetCount(sql, new { MachineId = machineId }) > 0)
            {
                // 「 保全活動で使用される機器の為、削除できません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141300003 });
                return true;
            }

            return false;
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
            string[] sqlIdList = new string[] { SqlName.Detail.GetChkParentInfo, SqlName.Detail.GetChkLoopInfo };

            foreach (string sqlId in sqlIdList)
            {
                // 構成検索SQL文の取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDirMachine, sqlId, out string sql);

                // 総件数を取得
                if (db.GetCount(sql, new { MachineId = machineId }) > 0)
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

        /// <summary>
        /// 適用法規情報管理テーブル 登録処理
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの倍はFalse</returns>
        private bool registApplocableLaws(long machineId, long historyManagementId, DateTime now)
        {
            // 検索条件を作成
            ComDao.McApplicableLawsEntity condition = new();
            condition.MachineId = machineId; // 機番ID

            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetApplicableLawsTransaction, out string sql);

            // SQL実行
            IList<ComDao.McApplicableLawsEntity> results = db.GetListByDataClass<ComDao.McApplicableLawsEntity>(sql, condition);
            if (results == null || results.Count == 0)
            {
                // データが紐付いていない場合もあるので取得できなくてもtrueを返す
                return true;
            }

            // 取得したデータを適用法規情報変更管理テーブルに登録
            foreach (ComDao.McApplicableLawsEntity result in results)
            {
                ComDao.HmMcApplicableLawsEntity applicableLawsEntity = new();
                applicableLawsEntity.HistoryManagementId = historyManagementId;                    // 変更管理ID
                applicableLawsEntity.ApplicableLawsId = result.ApplicableLawsId;                   // 適用法規ID
                applicableLawsEntity.ApplicableLawsStructureId = result.ApplicableLawsStructureId; // 適用法規アイテムID
                applicableLawsEntity.MachineId = result.MachineId;                                 // 機番ID

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref applicableLawsEntity, now, int.Parse(this.UserId), int.Parse(this.UserId));

                // SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Edit.InsertApplicableLawsInfo, SqlName.SubDir, applicableLawsEntity, this.db, string.Empty, new List<string>() { "DefaultApplicableLawsId" }))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 申請内容取消・承認依頼引戻・承認 共通処理
        /// <summary>
        /// 申請内容取消・承認依頼引戻・承認 共通処理
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="applicationStatus">登録する申請状況</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool detailCommonRegist(out Dao.searchResult registInfo, int? applicationStatus = null)
        {
            // 機番情報・機器情報取得
            DateTime now = DateTime.Now;
            registInfo = getRegistInfoBySearchResult(new List<short>() { ConductInfo.FormDetail.GroupNoMachine, ConductInfo.FormDetail.GroupNoEquipment }, now);

            // 排他チェック
            if (!checkExclusiveSingleForTable(ConductInfo.FormDetail.ControlId.Machine, new List<string>() { "hm_history_management" }))
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
            condition.HistoryManagementId = registInfo.HistoryManagementId; // 変更管理ID

            // 登録処理
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
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
            if (!detailCommonRegist(out Dao.searchResult registInfo))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 変更管理テーブル 削除処理
            ComDao.HmHistoryManagementEntity historyManagement = new();
            if (!historyManagement.DeleteByPrimaryKey(registInfo.HistoryManagementId, this.db))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機番情報変更管理テーブル 削除処理
            ComDao.HmMcMachineEntity machineEntity = new();
            if (registInfo.HmMachineId > 0 && !machineEntity.DeleteByPrimaryKey(registInfo.HmMachineId, this.db))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機器情報変更管理テーブル 削除処理
            ComDao.HmMcEquipmentEntity equipmentEntity = new();
            if (registInfo.HmEquipmentId > 0 && !equipmentEntity.DeleteByPrimaryKey(registInfo.HmEquipmentId, this.db))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 適用法規情報変更管理テーブル 削除処理
            if (!deleteApplicableLawsHistory(registInfo))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機器別管理基準部位変更管理テーブル 削除処理
            if (!deleteHmMcManagementStandardsComponent(registInfo))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機器別管理基準内容変更管理テーブル 削除処理
            if (!deleteHmMcManagementStandardsContent(registInfo))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 保全スケジュール変更管理テーブル 削除処理
            if (!deleteMaintainanceSchedule(registInfo))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 機器別管理基準部位変更管理テーブル 削除処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool deleteHmMcManagementStandardsComponent(Dao.searchResult condition)
        {
            // 検索SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetManagementStandardsComponentByHistoryManagementId, out string sql);

            // SQL実行
            IList<ComDao.HmMcManagementStandardsComponentEntity> results = db.GetListByDataClass<ComDao.HmMcManagementStandardsComponentEntity>(sql, condition);
            if (results == null || results.Count == 0)
            {
                // データが紐付いていない場合もあるので取得できなくてもtrueを返す
                return true;
            }

            // 取得したデータを削除
            ComDao.HmMcManagementStandardsComponentEntity component = new();
            foreach (ComDao.HmMcManagementStandardsComponentEntity result in results)
            {
                // 削除処理
                if (!component.DeleteByPrimaryKey(result.HmManagementStandardsComponentId, this.db))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 機器別管理基準内容変更管理テーブル 削除処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool deleteHmMcManagementStandardsContent(Dao.searchResult condition)
        {
            // 検索SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetManagementStandardsContentByHistoryManagementId, out string sql);

            // SQL実行
            IList<ComDao.HmMcManagementStandardsContentEntity> results = db.GetListByDataClass<ComDao.HmMcManagementStandardsContentEntity>(sql, condition);
            if (results == null || results.Count == 0)
            {
                // データが紐付いていない場合もあるので取得できなくてもtrueを返す
                return true;
            }

            // 取得したデータを削除
            ComDao.HmMcManagementStandardsContentEntity content = new();
            foreach (ComDao.HmMcManagementStandardsContentEntity result in results)
            {
                // 削除処理
                if (!content.DeleteByPrimaryKey(result.HmManagementStandardsContentId, this.db))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 保全スケジュール変更管理テーブル 削除処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool deleteMaintainanceSchedule(Dao.searchResult condition)
        {
            // 検索SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetMaintainanceScheduleByHistoryManagementId, out string sql);

            // SQL実行
            IList<ComDao.HmMcMaintainanceScheduleEntity> results = db.GetListByDataClass<ComDao.HmMcMaintainanceScheduleEntity>(sql, condition);
            if (results == null || results.Count == 0)
            {
                // データが紐付いていない場合もあるので取得できなくてもtrueを返す
                return true;
            }

            // 取得したデータを削除
            ComDao.HmMcMaintainanceScheduleEntity schedule = new();
            foreach (ComDao.HmMcMaintainanceScheduleEntity result in results)
            {
                // 削除処理
                if (!schedule.DeleteByPrimaryKey(result.HmMaintainanceScheduleId, this.db))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 承認依頼引戻
        /// <summary>
        /// 承認依頼引戻
        /// </summary>
        /// <returns>エラーの場合-1、正常の場合1</returns>
        public int pullBackRequest()
        {
            // 排他チェック + 申請状況を「差戻中」に更新
            if (!detailCommonRegist(out Dao.searchResult registInfo, (int)TMQConst.MsStructure.StructureId.ApplicationStatus.Return))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 詳細画面再検索処理
            if (!searchDetailList(registInfo.HistoryManagementId, registInfo.MachineId))
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
            if (!detailCommonRegist(out Dao.searchResult registInfo, (int)TMQConst.MsStructure.StructureId.ApplicationStatus.Approved))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 承認処理
            ComDao.HmHistoryManagementEntity histoyManagement = new();
            histoyManagement.HistoryManagementId = registInfo.HistoryManagementId; // 画面に表示されている変更管理IDを設定
            if (!approval(histoyManagement, registInfo.UpdateDatetime, registInfo.UpdateUserId))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region 保全項目一覧 登録・削除

        #region 登録ボタン押下時処理
        /// <summary>
        /// 保全項目一覧 登録処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool registManagementStandards()
        {
            DateTime now = DateTime.Now;

            // 翻訳の一時テーブルを作成
            createTranslationTempTbl();

            // 機番情報取得
            Dao.searchResult machineInfo = getRegistInfoBySearchResult(new List<short>() { ConductInfo.FormDetail.GroupNoMachine, ConductInfo.FormDetail.GroupNoEquipment }, now, true);

            // 保全項目一覧取得
            Dao.managementStandardsResult managementStandardsInfo = getRegistInfo<Dao.managementStandardsResult>(new List<string>() { ConductInfo.FormDetail.ControlId.ManagementStandardsList }, now, true, this.UserId);

            // 登録情報を設定
            managementStandardsInfo.IsManagementStandardConponent = true;                  // 機器別管理基準フラグ
            managementStandardsInfo.HistoryManagementId = machineInfo.HistoryManagementId; // 変更管理D

            // 周期ありフラグ
            if ((managementStandardsInfo.CycleYear == null || managementStandardsInfo.CycleYear == 0) &&
                (managementStandardsInfo.CycleMonth == null || managementStandardsInfo.CycleMonth == 0) &&
                (managementStandardsInfo.CycleDay == null || managementStandardsInfo.CycleDay == 0))
            {
                // 周期無し
                managementStandardsInfo.IsCyclic = false;
            }
            else
            {
                // 周期あり
                managementStandardsInfo.IsCyclic = true;
            }

            //  変更管理が紐付いているかどうか
            bool isExistsHistory = machineInfo.HistoryManagementId > 0;

            // 新規登録か更新かどうか
            bool isUpdate = managementStandardsInfo.ManagementStandardsComponentId > 0;

            // 変更管理登録クラス
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);

            // 排他チェック
            if (isErrorExclusiveManagementStandard(historyManagement, machineInfo, managementStandardsInfo, isExistsHistory, isUpdate, out IList<Dao.managementStandardsResult> dbresult))
            {
                return false;
            }

            // 入力チェック
            if (isErrorRegistManagementStandard(machineInfo, managementStandardsInfo, isUpdate, dbresult))
            {
                return false;
            }

            // スケジュールを更新 が未選択でかつ、次回実施予定日が入力されている場合
            // 確認メッセージを表示する(※メッセージを表示する必要がある時はTrueが返る)
            if (confirmByIsUpdateAndScheduleDate(managementStandardsInfo))
            {
                return true;
            }

            // 新規登録かつ、次回実施予定日が設定されている場合
            // 確認メッセージを表示する(※メッセージを表示する必要がある時はTrueが返る)
            if (confirmByIsNewAndIsUpdate(!isUpdate, managementStandardsInfo))
            {
                return true;
            }

            // 周期または開始日が変更されていて、スケジュールを更新 が未選択の場合
            if (confirmByScheduleInfoChanged(managementStandardsInfo, dbresult))
            {
                return true;
            }

            // 周期または開始日が変更されている
            if (dbresult != null)
            {
                // 更新時確認チェック(周期・開始日変更チェック 点検種別毎時周期・開始日変更チェック)
                if (this.Status < CommonProcReturn.ProcStatus.Confirm)
                {
                    if (isUpdate)
                    {
                        // 確認項目存在チェック
                        if (isConfirmRegistCycle(machineInfo, managementStandardsInfo))
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                // 更新時確認チェック(点検種別毎時周期・開始日変更チェック)
                if (this.Status < CommonProcReturn.ProcStatus.Confirm)
                {
                    // 確認チェック
                    if (isConfirmRegistCycleInsert(machineInfo, managementStandardsInfo))
                    {
                        return true;
                    }
                }
                else
                {
                    // 同一点検種別内に開始日以降で保全活動が紐づいているものがあるかチェック
                    if (isErrorRegistMaintainanceKindManage(managementStandardsInfo, ConductInfo.FormDetail.ControlId.ManagementStandardsList))
                    {
                        return false;
                    }
                }
            }

            // 変更管理テーブル 登録・更新処理
            if (!registHistoryManagement(historyManagement, ref machineInfo, ref managementStandardsInfo, isExistsHistory, now))
            {
                return false;
            }

            // 保全項目関連テーブル  登録・更新処理
            if (!registHistoryManagementDetail(historyManagement, machineInfo, managementStandardsInfo, isUpdate, now))
            {
                return false;
            }

            return true;
        }

        #region 排他チェック
        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <param name="historyManagement">変更管理登録クラス</param>
        /// <param name="machineInfo">機番情報・機器情報</param>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <param name="isExistsHistory">変更管理が存在する場合はTrue</param>
        /// <param name="isUpdate">更新の場合はTrue</param>
        /// <param name="dbresult">変更前の値</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool isErrorExclusiveManagementStandard(TMQUtil.HistoryManagement historyManagement, Dao.searchResult machineInfo, Dao.managementStandardsResult managementStandardsInfo, bool isExistsHistory, bool isUpdate, out IList<Dao.managementStandardsResult> dbresult)
        {
            dbresult = null;

            // 変更管理排他チェック
            if (checkExclusiveHistoryManagement(machineInfo, isExistsHistory))
            {
                return true;
            }

            // 更新の場合、変更前のデータを取得
            if (isUpdate)
            {
                // レコードに変更管理が紐付いているか判定
                if (managementStandardsInfo.ExecutionDivision > 0)
                {
                    // 変更管理が紐付いている場合
                    dbresult = getManagementStandardsData<Dao.managementStandardsResult>(SqlName.SubDir, SqlName.Detail.GetHistoryManagementStandardsList, managementStandardsInfo, new List<string>() { "ComponentId" });

                }
                else
                {
                    // 変更管理が紐付いていない場合
                    dbresult = getManagementStandardsData<Dao.managementStandardsResult>(SqlName.SubDirMachine, SqlName.Detail.GetManagementStandardDetail, managementStandardsInfo);
                    // 排他チェック
                    if (dbresult == null || dbresult.Count == 0 || managementStandardsInfo.UpdateDatetime < dbresult[0].UpdateDatetime)
                    {
                        setExclusiveError();
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion

        #region 入力チェック
        /// <summary>
        /// 保全項目編集画面入力チェック
        /// </summary>
        /// <param name="machineInfo">機番情報・機器情報</param>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <param name="isUpdate">更新の場合はTrue</param>
        /// <param name="dbresult">変更前の値</param>
        /// <returns>エラーの場合はTrue</returns>
        private bool isErrorRegistManagementStandard(Dao.searchResult machineInfo, Dao.managementStandardsResult managementStandardsInfo, bool isUpdate, IList<Dao.managementStandardsResult> dbresult)
        {
            // チェック対象の保全項目一覧のコントロールグループID
            string ctrlId = ConductInfo.FormDetail.ControlId.ManagementStandardsList;
            Dictionary<string, object> targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.ManagementStandardsList);

            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // チェック
            if (isErrorRegistManagementStandardForSingle(ref errorInfoDictionary, managementStandardsInfo, isUpdate, ctrlId, targetDic))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            // メソッド内処理
            bool isErrorRegistManagementStandardForSingle(ref List<Dictionary<string, object>> errorInfoDictionary, Dao.managementStandardsResult result, bool isUpdate, string ctrlId, Dictionary<string, object> targetDic)
            {
                bool isError = false;   // 処理全体でエラーの有無を保持
                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(ctrlId);

                // 同一機器、部位、保全項目重複チェック
                if (getErrCnt(SqlName.Detail.GetManagementStandardCountCheckHistory, SqlName.SubDir, result))
                {
                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                    isError = true;
                    // 「入力された部位、保全項目の組み合わせは既に登録されています。」
                    string errMsg = GetResMessage(ComRes.ID.ID141220002);
                    string val = info.getValName("inspection_site_structure_id"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val);
                    val = info.getValName("inspection_content_structure_i");     // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val);
                    errorInfoDictionary.Add(errorInfo.Result);
                }

                // 更新時なら周期・開始日変更チェック
                if (isUpdate)
                {
                    // 周期または開始日が変更されている
                    if (managementStandardsInfo.CycleYear != dbresult[0].CycleYear ||
                        managementStandardsInfo.CycleMonth != dbresult[0].CycleMonth ||
                        managementStandardsInfo.CycleDay != dbresult[0].CycleDay ||
                        managementStandardsInfo.StartDate != dbresult[0].StartDate)
                    {
                        // 開始日が過去日だった際はエラー
                        if (managementStandardsInfo.StartDate < DateTime.Now.Date)
                        {
                            // エラー情報格納クラス
                            ErrorInfo errorInfo = new ErrorInfo(targetDic);
                            isError = true;
                            // 「過去日付は設定できません。」
                            string errMsg = GetResMessage(ComRes.ID.ID141060003);
                            string val = info.getValName("start_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                            errorInfo.setError(errMsg, val);
                            errorInfoDictionary.Add(errorInfo.Result);
                        }

                        // 開始日以降に保全活動が紐づいているスケジュールが存在する際はエラーとする
                        if (getErrCnt(SqlName.Detail.GetScheduleMsSummryCountAfterCheck, SqlName.SubDirMachine, result))
                        {
                            // エラー情報格納クラス
                            ErrorInfo errorInfo = new ErrorInfo(targetDic);
                            isError = true;
                            // 「開始日には保全活動が登録されたスケジュール以降の日付を設定してください。」
                            string errMsg = GetResMessage(ComRes.ID.ID141060005);
                            string val = info.getValName("start_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                            errorInfo.setError(errMsg, val);
                            errorInfoDictionary.Add(errorInfo.Result);
                        }

                    }
                }

                // 過去日のチェックは行わない
                //// 次回実施予定日に過去日が入力されている場合
                //if (result.ScheduleDate != null && result.ScheduleDateBefore != null && result.ScheduleDate < result.ScheduleDateBefore)
                //{
                //    // エラー情報格納クラス
                //    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                //    isError = true;
                //    // 過去日付は設定できません。
                //    string errMsg = GetResMessage("141060003");
                //    string val = info.getValName("schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                //    errorInfo.setError(errMsg, val);
                //    errorInfoDictionary.Add(errorInfo.Result);
                //}

                // 次回実施予定日が入力されている場合
                if (result.ScheduleDate != null)
                {
                    // 次々回実施予定日を取得
                    List<Dao.managementStandardsResult> nextDate = (List<Dao.managementStandardsResult>)getDbData<Dao.managementStandardsResult>(SqlName.Detail.GetNextScheduleDate, new { ManagementStandardsContentId = result.ManagementStandardsContentId, ScheduleDateBefore = result.ScheduleDateBefore, MaintainanceScheduleId = result.MaintainanceScheduleId });

                    // 取得できた場合は範囲チェックをする
                    if (nextDate != null && nextDate.Count > 0)
                    {
                        bool isDateErr = false;

                        // 入力された次回実施予定日が次々回実施予定日以降の場合
                        if (result.ScheduleDate >= nextDate[0].ScheduleDate)
                        {
                            isDateErr = true;
                        }

                        // 前回実施予定日は存在しない可能性があることを考慮する
                        if (nextDate.Count > 1)
                        {
                            // 入力された次回実施予定日が前回実施予定日の場合以前
                            if (result.ScheduleDate <= nextDate[1].ScheduleDate)
                            {
                                isDateErr = true;
                            }
                        }
                        else
                        {
                            // 前回実施予定日が存在しない場合は開始日と比較する
                            if (result.ScheduleDate < result.StartDate)
                            {
                                isDateErr = true;
                            }
                        }

                        // 日付の範囲エラーの場合はメッセージをセットする
                        if (isDateErr)
                        {
                            // エラー情報格納クラス
                            ErrorInfo errorInfo = new ErrorInfo(targetDic);
                            isError = true;
                            // 次回実施予定日は前回実施予定日～次々回実施予定日以内の日付を指定してください。
                            string errMsg = GetResMessage("141120020");
                            string val = info.getValName("schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                            errorInfo.setError(errMsg, val);
                            errorInfoDictionary.Add(errorInfo.Result);
                        }
                    }
                }

                // 新規登録かつ、スケジュールを更新 が未選択の場合
                if (!isUpdate && result.IsUpdateSchedule.ToString() == ComConsts.CHECK_FLG.OFF)
                {
                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                    isError = true;
                    // 新規登録情報の為　「スケジュールを更新」にチェックを付与してください。
                    string errMsg = GetResMessage("141120017");
                    string val = info.getValName("is_update_schedule"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val);
                    errorInfoDictionary.Add(errorInfo.Result);
                }

                // 保全項目編集画面の登録の場合
                if (dbresult != null)
                {
                    // 開始日または周期(年・月・日)が変更されているかどうか
                    // いずれかが変更されている場合はフラグ = True
                    bool cycleChangeFlg;
                    if (result.CycleYear == dbresult[0].CycleYear && result.CycleMonth == dbresult[0].CycleMonth && result.CycleDay == dbresult[0].CycleDay && result.StartDate == dbresult[0].StartDate)
                    {
                        cycleChangeFlg = false;
                    }
                    else
                    {
                        cycleChangeFlg = true;
                    }

                    // 次回実施予定日が入力されているかつ値が変更されていて、開始日または周期(年・月・日)が変更されている場合
                    if (result.ScheduleDate != null && result.ScheduleDate != result.ScheduleDateBefore && cycleChangeFlg)
                    {
                        // エラー情報格納クラス
                        ErrorInfo errorInfo = new ErrorInfo(targetDic);
                        isError = true;
                        // 周期または開始日が変更されていいます。　同時に次回実施予定日は更新できません。
                        string errMsg = GetResMessage("141120019");
                        string val = info.getValName("schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                        errorInfo.setError(errMsg, val);
                        errorInfoDictionary.Add(errorInfo.Result);
                    }
                }

                return isError;
            }
        }

        /// <summary>
        /// 確認項目存在チェック
        /// </summary>
        /// <param name="machineInfo">機番情報・機器情報</param>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <returns>エラーの場合はTrue</returns>
        private bool isConfirmRegistCycle(Dao.searchResult machineInfo, Dao.managementStandardsResult managementStandardsInfo)
        {
            // 既に保全履歴の登録されているデータが存在する場合、確認メッセージ表示
            if (getErrCnt(SqlName.Detail.GetScheduleMsSummryCountCheck, SqlName.SubDirMachine, managementStandardsInfo))
            {
                // 確認
                this.Status = CommonProcReturn.ProcStatus.Confirm;
                // 「保全履歴が既に登録されてますが周期・開始日が変更されています。スケジュールを再作成しますがよろしいですか？」
                this.MsgId = GetResMessage(ComRes.ID.ID141300005);
                this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
                return true;
            }

            // 点検種別毎管理の機器
            if (!machineInfo.MaintainanceKindManage)
            {
                return false;
            }

            // 同一点検種別存在チェック
            if (getErrCnt(SqlName.Detail.GetMaintainanceKindManageExistCheckHistory, SqlName.SubDir, managementStandardsInfo))
            {
                // 既に同じ点検種別が対象機器の機器別管理基準内に登録されていて、周期と開始日が違う場合、確認メッセージを表示する。
                // ※確認メッセージで「OK」だった際は、入力された周期と開始日で既に登録されている同じ点検種別のデータを更新する。（後勝ち登録)
                // 確認
                this.Status = CommonProcReturn.ProcStatus.Confirm;
                // 同一点検種別で既に異なる周期・開始日が設定されています。入力された周期・内容でスケジュールを再作成しますがよろしいですか？
                this.MsgId = GetResMessage(ComRes.ID.ID141200001);
                this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 新規登録時確認チェック(点検種別毎管理)
        /// </summary>
        /// <param name="machineInfo">機番情報・機器情報</param>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <returns>エラーの場合はTrue</returns>
        private bool isConfirmRegistCycleInsert(Dao.searchResult machineInfo, Dao.managementStandardsResult managementStandardsInfo)
        {
            // 点検種別毎管理
            if (!machineInfo.MaintainanceKindManage)
            {
                return false;
            }

            // 同一点検種別存在チェック
            if (getErrCnt(SqlName.Detail.GetMaintainanceKindManageInsertExistCheckHistory, SqlName.SubDir, managementStandardsInfo))
            {
                // 既に同じ点検種別が対象機器の機器別管理基準内に登録されていて、周期と開始日が違う場合、確認メッセージを表示する。
                // ※確認メッセージで「OK」だった際は、入力された周期と開始日で既に登録されている同じ点検種別のデータを更新する。（後勝ち登録)
                // 確認
                this.Status = CommonProcReturn.ProcStatus.Confirm;
                // 同一点検種別で既に異なる周期・開始日が設定されています。入力された周期・内容でスケジュールを再作成しますがよろしいですか？
                this.MsgId = GetResMessage(ComRes.ID.ID141200001);
                this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 点検種別毎登録時のチェック
        /// </summary>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <param name="ctrlId">エラー情報を設定する一覧のコントロールID</param>
        /// <returns>エラーの場合はTrue</returns>
        private bool isErrorRegistMaintainanceKindManage(Dao.managementStandardsResult managementStandardsInfo, string ctrlId)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // チェック
            // 開始日以降に保全活動が紐づいているスケジュールが存在する際はエラーとする
            if (getErrCnt(SqlName.Detail.GetScheduleMsSummryCountAfterMaintainanceKindManageCheck, SqlName.SubDirMachine, managementStandardsInfo))
            {
                // 単一の内容を取得
                Dictionary<string, object> targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(ctrlId);
                // エラー情報格納クラス
                ErrorInfo errorInfo = new ErrorInfo(targetDic);
                // 「開始日には保全活動が登録されたスケジュール以降の日付を設定してください。」
                string errMsg = GetResMessage(ComRes.ID.ID141060005);
                string val = info.getValName("start_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                errorInfo.setError(errMsg, val);
                errorInfoDictionary.Add(errorInfo.Result);
                SetJsonResult(errorInfoDictionary);
                // エラー情報を画面に反映
                return true;
            }

            return false;
        }

        /// <summary>
        /// 機器別管理基準 保全項目編集画面 入力チェック
        /// </summary>
        /// <param name="registResult">保全項目編集画面で入力された内容</param>
        /// <returns>確認メッセージを表示する場合はTrue</returns>
        private bool confirmByIsUpdateAndScheduleDate(Dao.managementStandardsResult registResult)
        {
            // スケジュールを更新 と 次回実施予定日の入力状態を判定
            if (this.Status < CommonProcReturn.ProcStatus.Confirm &&
                registResult.IsUpdateSchedule.ToString() == ComConsts.CHECK_FLG.OFF &&
                registResult.ScheduleDate != null)
            {
                // スケジュールを更新 が未選択かつ、次回実施予定日が入力されている場合
                // 確認メッセージを表示
                this.Status = CommonProcReturn.ProcStatus.Confirm;
                // 次回実施予定日が設定されている為　直近のスケジュールを１件更新します。よろしいですか？
                this.MsgId = GetResMessage("141120016");
                this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 機器別管理基準 保全項目編集画面 入力チェック
        /// </summary>
        /// <param name="insertFlg">新規登録の場合True</param>
        /// <param name="registResult">保全項目編集画面で入力された内容</param>
        /// <returns>確認メッセージを表示する場合はTrue</returns>
        private bool confirmByIsNewAndIsUpdate(bool insertFlg, Dao.managementStandardsResult registResult)
        {
            // 新規or更新と次回実施予定日の入力状態を判定
            if (this.Status < CommonProcReturn.ProcStatus.Confirm &&
                insertFlg &&
                registResult.ScheduleDate != null)
            {
                // 新規登録かつ、次回実施予定日が入力されている場合
                // 確認メッセージを表示
                this.Status = CommonProcReturn.ProcStatus.Confirm;
                // 新規登録情報の為　次回実施予定日は考慮されません。処理を継続してもよろしいですか？
                this.MsgId = GetResMessage("141120018");
                this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 機器別管理基準 保全項目編集画面 入力チェック
        /// </summary>
        /// <param name="registResult">保全項目編集画面で入力された内容</param>
        /// <param name="dbresult">変更前の情報(DBから取得した値)</param>
        /// <returns>確認メッセージを表示する場合はTrue</returns>
        private bool confirmByScheduleInfoChanged(Dao.managementStandardsResult registResult, IList<Dao.managementStandardsResult> dbresult)
        {
            if (this.Status >= CommonProcReturn.ProcStatus.Confirm)
            {
                return false;
            }

            // DB値がNULLの場合は終了
            if (dbresult == null)
            {
                return false;
            }

            // 開始日、周期(年)、周期(月)、周期(日)がどれも変更されていない場合は終了
            if (registResult.CycleYear == dbresult[0].CycleYear &&
                registResult.CycleMonth == dbresult[0].CycleMonth &&
                registResult.CycleDay == dbresult[0].CycleDay &&
                registResult.StartDate == dbresult[0].StartDate)
            {
                return false;
            }

            // スケジュールを更新 が選択されている場合は終了
            if (registResult.IsUpdateSchedule.ToString() == ComConsts.CHECK_FLG.ON)
            {
                return false;
            }

            // ↓周期または開始日が変更されていて、スケジュールを更新 が選択されていない場合
            // 確認メッセージを表示
            this.Status = CommonProcReturn.ProcStatus.Confirm;
            // 周期または開始日が変更されていますが、スケジュールを更新 がチェックされていないためスケジュールの再作成はされません。よろしいですか？
            this.MsgId = GetResMessage("141120021");
            this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
            return true;
        }
        #endregion

        #region 登録処理
        /// <summary>
        /// 変更管理テーブル 登録・更新処理
        /// </summary>
        /// <param name="historyManagement">変更管理登録クラス</param>
        /// <param name="machineInfo">機番情報・機器情報</param>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <param name="isExistsHistory">変更管理が存在する場合はTrue</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHistoryManagement(TMQUtil.HistoryManagement historyManagement, ref Dao.searchResult machineInfo, ref Dao.managementStandardsResult managementStandardsInfo, bool isExistsHistory, DateTime now)
        {
            // 変更管理が存在するか判定
            if (isExistsHistory)
            {
                // 機器に変更管理が紐付いている場合は更新
                ComDao.HmHistoryManagementEntity historyManagementEntity = new() { HistoryManagementId = machineInfo.HistoryManagementId };
                historyManagementEntity.ApplicationUserId = int.Parse(this.UserId);
                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref historyManagementEntity, now, int.Parse(this.UserId), int.Parse(this.UserId));
                if (!historyManagement.UpdateHistoryManagement(historyManagementEntity, new List<string>() { "ApplicationUserId" }))
                {
                    return false;
                }
            }
            else
            {
                // 変更管理が紐付いていない場合は登録
                // 変更管理 登録処理
                // 引数1：変更申請
                // 引数2：機番ID
                // 引数3：機番IDより求めた場所階層ID
                (bool returnFlag, long historyManagementId) historyManagementResult =
                    historyManagement.InsertHistoryManagement(TMQConst.MsStructure.StructureId.ApplicationDivision.Update,
                                                              machineInfo.MachineId,
                                                              historyManagement.getFactoryId((int)machineInfo.LocationStructureId));

                // 登録に失敗した場合は終了
                if (!historyManagementResult.returnFlag)
                {
                    return false;
                }

                // 採番した変更管理IDを設定
                machineInfo.HistoryManagementId = historyManagementResult.historyManagementId;
                managementStandardsInfo.HistoryManagementId = historyManagementResult.historyManagementId;
            }

            return true;
        }

        /// <summary>
        /// 変更管理詳細テーブル・保全項目関連テーブル 登録・更新処理
        /// </summary>
        /// <param name="historyManagement">変更管理登録クラス</param>
        /// <param name="machineInfo">機番情報・機器情報</param>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <param name="isUpdate">更新の場合はTrue</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHistoryManagementDetail(TMQUtil.HistoryManagement historyManagement, Dao.searchResult machineInfo, Dao.managementStandardsResult managementStandardsInfo, bool isUpdate, DateTime now)
        {
            // レコードに変更管理が紐付いているか判定
            if (managementStandardsInfo.ExecutionDivision > 0)
            {
                // 変更管理が紐付いている場合は更新
                return updateManagementStandards();
            }
            else
            {
                // 変更管理が紐付いていない場合は登録
                return insertManagementStandards();
            }

            return true;

            // 保全項目関連テーブル登録処理
            bool insertManagementStandards()
            {
                // 機器別管理基準部位変更管理 登録処理 + 採番した値を返す
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long managementStandardsComponentId, SqlName.Detail.InsertManagementStandardsComponentInfo, SqlName.SubDir, managementStandardsInfo, this.db, string.Empty, new List<string>() { isUpdate ? "DefaultComponent" : "NewComponent" }))
                {
                    return false;
                }

                // 採番した機器別管理基準部位IDを登録情報に設定
                managementStandardsInfo.ManagementStandardsComponentId = managementStandardsComponentId;

                // 登録情報を設定
                managementStandardsInfo.ExecutionDivision = isUpdate ? executionDiv.ComponentEdit : executionDiv.ComponentNew; // 実行処理区分
                managementStandardsInfo.OrderNo = 0;                                                                           // 並び順

                // 機器別管理基準内容変更管理 登録処理 + 採番した値を返す
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long managementStandardsContentId, SqlName.Detail.InsertManagementStandardsContentInfo, SqlName.SubDir, managementStandardsInfo, this.db, string.Empty, new List<string>() { isUpdate ? "DefaultContent" : "NewContent" }))
                {
                    return false;
                }

                // 採番した機器別管理基準内容IDを登録情報に設定
                managementStandardsInfo.ManagementStandardsContentId = managementStandardsContentId;

                // 保全スケジュール変更管理 登録処理
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertMaintainanceScheduleInfo, SqlName.SubDir, managementStandardsInfo, this.db, string.Empty, new List<string>() { isUpdate ? "DefaultSchedule" : "NewSchedule" }))
                {
                    return false;
                }

                return true;
            }

            // 保全項目関連テーブル更新処理
            bool updateManagementStandards()
            {
                // 機器別管理基準部位変更管理 更新処理
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateHmMcManagementStandardsComponent, SqlName.SubDir, managementStandardsInfo, this.db, string.Empty))
                {
                    return false;
                }

                // 機器別管理基準内容変更管理 更新処理
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateHmMcManagementStandardsContent, SqlName.SubDir, managementStandardsInfo, this.db, string.Empty))
                {
                    return false;
                }

                // 保全スケジュール変更管理 更新処理

                // 保全項目を新規追加するデータの場合は「スケジュールを更新」をチェック状態とする
                if(managementStandardsInfo.ExecutionDivision == executionDiv.ComponentNew)
                {
                    managementStandardsInfo.IsUpdateSchedule = int.Parse(ComConsts.CHECK_FLG.ON);
                }

                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateHmMaintainanceSchedule, SqlName.SubDir, managementStandardsInfo, this.db, string.Empty))
                {
                    return false;
                }

                return true;
            }
        }
        #endregion

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="ctrlIdList">取得するコントロールID</param>
        /// <param name="now">システム日時</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(List<string> ctrlIdList, DateTime now, bool isManagementStandards, string userId = "-1")
            where T : CommonDataBaseClass.CommonTableItem, new()
        {

            T resultInfo = new();
            foreach (string ctrlId in ctrlIdList)
            {
                // コントロールIDにより画面の項目(一覧)を取得
                Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(isManagementStandards ? this.resultInfoDictionary : this.searchConditionDictionary, ctrlId);

                if (!SetExecuteConditionByDataClass<T>(result, ctrlId, resultInfo, now, this.UserId, userId))
                {
                    // エラーの場合終了
                    return resultInfo;
                }
            }

            return resultInfo;
        }
        #endregion

        #region 行削除ボタン押下時処理
        /// <summary>
        /// 保全項目一覧 削除処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool deleteManagementStandards()
        {
            DateTime now = DateTime.Now;
            int userId = int.Parse(this.UserId);

            // 機番情報取得
            Dao.searchResult machineInfo = getRegistInfoBySearchResult(new List<short>() { ConductInfo.FormDetail.GroupNoMachine }, now, true);

            // 変更管理排他チェック
            if (checkExclusiveHistoryManagement(machineInfo, machineInfo.HistoryManagementId > 0))
            {
                return false;
            }

            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
            // 変更管理が紐付いていない場合は登録
            if (machineInfo.HistoryManagementId <= 0)
            {
                // SQL実行(申請区分は「変更申請」)
                (bool returnFlag, long historyManagementId) historyManagementResult =
                    historyManagement.InsertHistoryManagement(TMQConst.MsStructure.StructureId.ApplicationDivision.Update,
                                                              machineInfo.MachineId,
                                                              historyManagement.getFactoryId((int)machineInfo.LocationStructureId));

                // 採番した変更管理IDを設定
                machineInfo.HistoryManagementId = historyManagementResult.historyManagementId;

            }
            else
            {
                // 機器に変更管理が紐付いている場合は更新
                ComDao.HmHistoryManagementEntity historyManagementEntity = new() { HistoryManagementId = machineInfo.HistoryManagementId };
                historyManagementEntity.ApplicationUserId = userId;
                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref historyManagementEntity, now, userId, userId);
                if (!historyManagement.UpdateHistoryManagement(historyManagementEntity, new List<string>() { "ApplicationUserId" }))
                {
                    return false;
                }
            }

            // 選択されたレコード取得
            var deleteList = getSelectedRowsByList(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.ManagementStandardsList);

            foreach (var deleteRow in deleteList)
            {
                // 選択されたレコードをデータクラスに変換
                Dao.managementStandardsResult deleteCondition = new();

                // 変換できない場合はエラー
                if (!ComUtil.SetConditionByDataClass(ConductInfo.FormDetail.ControlId.ManagementStandardsList, this.mapInfoList, deleteCondition, deleteRow, ComUtil.ConvertType.Execute, null))
                {

                    return false;
                }

                // 既に行削除されている場合は何もしない
                if (deleteCondition.ExecutionDivision == executionDiv.ComponentDelete)
                {
                    continue;
                }

                // 入力チェック
                if (inputCheckStandardsManagement(deleteCondition))
                {
                    return false;
                }

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref deleteCondition, now, userId, userId);

                // 「保全項目一覧の追加」「保全項目一覧の項目編集」の場合、申請しているレコードを削除
                if (deleteCondition.ExecutionDivision == executionDiv.ComponentNew ||
                    deleteCondition.ExecutionDivision == executionDiv.ComponentEdit)
                {
                    // 機器別管理基準部位変更管理
                    ComDao.HmMcManagementStandardsComponentEntity component = new();
                    if (!component.DeleteByPrimaryKey(deleteCondition.HmManagementStandardsComponentId, this.db))
                    {
                        return false;
                    }

                    // 機器別管理基準内容変更管理
                    ComDao.HmMcManagementStandardsContentEntity content = new();
                    if (!content.DeleteByPrimaryKey(deleteCondition.HmManagementStandardsContentId, this.db))
                    {
                        return false;
                    }

                    // 保全スケジュール変更管理
                    ComDao.HmMcMaintainanceScheduleEntity schedule = new();
                    if (!schedule.DeleteByPrimaryKey(deleteCondition.HmMaintainanceScheduleId, this.db))
                    {
                        return false;
                    }
                }

                // 「保全項目一覧の項目編集」「トランザクションデータの削除」の場合レコードを削除申請として登録する
                if (deleteCondition.ExecutionDivision == executionDiv.ComponentEdit ||
                    deleteCondition.ExecutionDivision <= 0)
                {
                    // 機器別管理基準部位変更管理テーブル 登録処理
                    if (!registHmMcManagementStandardsComponent(machineInfo.HistoryManagementId, deleteCondition.ManagementStandardsComponentId, now, "ComponentId", out List<long> componentList))
                    {
                        return false;
                    }

                    // 機器別管理基準内容変更管理テーブル 登録処理
                    if (!registHmMcManagementStandardsContent(machineInfo.HistoryManagementId, now, componentList, out List<long> contentList))
                    {
                        return false;
                    }

                    // 保全スケジュール変更管理テーブル 登録処理
                    if (!registMaintainanceSchedule(machineInfo.HistoryManagementId, now, contentList))
                    {
                        return false;
                    }
                }
            }

            // 詳細画面再検索処理
            if (!searchDetailList(machineInfo.HistoryManagementId, machineInfo.MachineId))
            {
                return false;
            }

            return true;
        }

        #region 入力チェック
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="deleteCondition">削除条件</param>
        /// <returns>エラーの場合はTrue</returns>
        private bool inputCheckStandardsManagement(Dao.managementStandardsResult deleteCondition)
        {
            //選択されたレコードに変更管理が紐付いていない場合
            if (deleteCondition.ExecutionDivision <= 0)
            {
                // 変更前のデータ取得
                IList<Dao.managementStandardsResult> dbresult = getManagementStandardsData<Dao.managementStandardsResult>(SqlName.SubDirMachine, SqlName.Detail.GetManagementStandardDetail, deleteCondition);

                // 排他チェック
                if (dbresult == null || dbresult.Count == 0 || deleteCondition.UpdateDatetime < dbresult[0].UpdateDatetime)
                {
                    setExclusiveError();
                    return true;
                }
            }

            // 長期計画存在チェック
            if (deleteCondition.ExecutionDivision <= 0)
            {
                // レコードに変更管理が紐付いていない場合はトランザクションテーブル
                if (getErrCnt(SqlName.Detail.GetLongPlanSingle, SqlName.SubDirMachine, deleteCondition))
                {
                    // 「 長期計画で使用されている為、削除できません。」
                    this.MsgId = this.MsgId = GetResMessage(ComRes.ID.ID141170002);
                    return true;
                }
            }
            else
            {
                // レコードに変更管理が紐付いている場合は機器別管理基準内容変更管理テーブル
                if (getErrCnt(SqlName.Detail.GetLongPlanSingleHistory, SqlName.SubDir, deleteCondition))
                {
                    // 「 長期計画で使用されている為、削除できません。」
                    this.MsgId = this.MsgId = GetResMessage(ComRes.ID.ID141170002);
                    return true;
                }
            }

            // 保全活動存在チェック
            if (getErrCnt(SqlName.Detail.GetMsSummarySingle, SqlName.SubDirMachine, deleteCondition))
            {
                // 「 保全活動が作成されている為、削除できません。」
                this.MsgId = this.MsgId = GetResMessage(ComRes.ID.ID141300004);
                return true;
            }

            // 添付ファイル排他チェック
            if (exclusiveAttachment(deleteCondition))
            {
                return true;
            }

            return false;

            // 添付ファイル 排他チェック
            bool exclusiveAttachment(Dao.managementStandardsResult deleteCondition)
            {
                // 添付ファイルの最大更新日時を取得
                // SQL取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDirMachine, SqlName.Detail.GetMaxDateByKeyId, out string sql);

                // SQL実行
                var maxDateResult = db.GetEntity(sql, new { KeyId = deleteCondition.ManagementStandardsContentId, FunctionTypeId = (int)TMQConst.Attachment.FunctionTypeId.Content });

                // 次第更新日時で比較
                DateTime? maxDateOfList = deleteCondition.MaxUpdateDatetime != null ? deleteCondition.MaxUpdateDatetime : null;
                if (!CheckExclusiveStatusByUpdateDatetime(maxDateOfList, maxDateResult.max_update_datetime))
                {
                    return true;
                }

                return false;
            }
        }
        #endregion

        #endregion
        #endregion
    }
}
