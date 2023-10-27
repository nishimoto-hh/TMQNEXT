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
    /// 変更管理 機器台帳 詳細編集画面
    /// </summary>
    public partial class BusinessLogic_HM0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 詳細編集画面の表示種類
        /// </summary>
        private class EditDispType
        {
            /// <summary>新規申請</summary>
            public const int New = 0;
            /// <summary>複写申請</summary>
            public const int Copy = 1;
            /// <summary>変更申請</summary>
            public const int Change = 2;
            /// <summary>申請内容修正</summary>
            public const int Edit = 3;
        }

        #region 検索
        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchEditList()
        {
            // 遷移元コントロールIDより詳細編集画面の表示種類を判定
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId);
            if (compareId.IsStartId(ConductInfo.FormList.ButtonId.New))
            {
                // 新規申請
                return searchResultNew();
            }
            else if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.CopyRequest))
            {
                // 複写申請
                return searchResultTransaction(EditDispType.Copy);
            }
            else if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.ChangeRequest))
            {
                // 変更申請
                return searchResultTransaction(EditDispType.Change);
            }
            else if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.EditRequest))
            {
                // 申請内容修正
                return searchResultTransaction(EditDispType.Edit);
            }
            else
            {
                // 到達不能
                throw new Exception();
            }

            return true;
        }

        /// <summary>
        /// 検索処理(新規登録申請)
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchResultNew()
        {
            // 新規申請の場合は場所階層ツリー、職種・機種ツリーの内容を反映する
            Dao.searchResult result = new();

            // 場所階層IDを取得
            int? locationStructureId = getTreeValue(true);
            // 取得した場所階層IDがNULLでなければ検索結果に設定する
            if (locationStructureId != null)
            {
                result.LocationStructureId = locationStructureId;
            }

            //職種階層IDを取得
            int? jobSructureId = getTreeValue(false);
            // 取得した職種階層IDがNULLでなければ検索結果に設定する
            if (jobSructureId != null)
            {
                result.JobStructureId = jobSructureId;
            }

            // その他の設定値
            result.DateOfInstallation = DateTime.Now; // 設置日
            result.DateOfManufacture = DateTime.Now;  // 製造日
            result.DispType = (int)EditDispType.New;  // 画面の表示種類(新規申請)
            result.HistoryManagementId = -1;          // 変更管理ID(採番されていないので -1 を設定)

            IList<Dao.searchResult> resultList = new List<Dao.searchResult> { result };

            // 場所階層IDと職種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref resultList, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

            // 検索結果の設定(機番情報・機器情報)
            if (!initFormByParam(resultList[0], getResultMappingInfoByGrpNo(ConductInfo.FormEdit.GroupNoMachine).CtrlIdList) ||
                !initFormByParam(resultList[0], getResultMappingInfoByGrpNo(ConductInfo.FormEdit.GroupNoEquipment).CtrlIdList))
            {
                return false;
            }

            return true;

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
        /// 検索処理(複写申請・変更申請・申請内容修正)
        /// </summary>
        /// <param name="editDispType">画面表示種類</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchResultTransaction(int editDispType)
        {
            // 検索条件を取得
            Dao.detailSearchCondition condition = getDetailSearchCondition();

            string sql = string.Empty;

            // 機器に変更管理が紐付いているか判定
            if (condition.HistoryManagementId <= 0)
            {
                // 変更管理が紐付いていない場合、トランザクションデータを表示
                TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDirMachine, SqlName.Detail.GetMachineDetail, out sql);
            }
            else
            {
                // 変更管理に機器の新規・複写の申請があるか判定
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Edit.GetMachineHistoryManagementCnt, out sql);

                // 件数取得SQLを取得
                string executeSql = TMQUtil.GetSqlStatementSearch(true, sql, string.Empty, string.Empty);

                // 総件数を取得
                int cnt = db.GetCount(executeSql, condition);
                if (cnt > 0)
                {
                    // 変更管理に機器の新規・複写の申請があれば申請内容を表示
                    executeSql = TMQUtil.GetSqlStatementSearch(false, sql, string.Empty, string.Empty);
                }
                else
                {
                    // 変更管理に機器の新規・複写の申請が無ければトランザクションデータを表示
                    TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDirMachine, SqlName.Detail.GetMachineDetail, out sql);
                }
            }

            // SQL実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 変更管理ID・変更管理テーブルの更新シリアルIDを設定
            results[0].HistoryManagementId = condition.HistoryManagementId;
            results[0].UpdateSerialid = new ComDao.HmHistoryManagementEntity().GetEntity(condition.HistoryManagementId, this.db).UpdateSerialid;

            // 画面表示タイプ
            results[0].DispType = editDispType;

            // 複写申請の場合は検索結果を別に設定
            if (editDispType == EditDispType.Copy)
            {
                results[0].DateOfInstallation = DateTime.Now; // 設置日
                results[0].DateOfManufacture = DateTime.Now;  // 製造日
            }

            // 変更管理が紐付いていない場合は変更管理IDを設定
            if (condition.HistoryManagementId <= 0)
            {
                results[0].HistoryManagementId = -1; // 変更管理ID(採番されていないので -1 を設定)
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

            // 検索結果の設定(機番情報・機器情報)
            if (!setSearchResult(ConductInfo.FormEdit.GroupNoMachine, results) ||
                !setSearchResult(ConductInfo.FormEdit.GroupNoEquipment, results))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 指定された一覧に値を設定する
        /// </summary>
        /// <param name="param">検索結果一覧</param>
        /// <param name="toCtrlIds">値を設定する一覧のコントロールIDリスト</param>
        /// <returns>エラーの場合False</returns>
        private bool initFormByParam(Dao.searchResult param, List<string> toCtrlIds)
        {
            // 一覧に対して繰り返し値を設定する
            foreach (var ctrlId in toCtrlIds)
            {
                if (!setFormByDataClass(ctrlId, new List<Dao.searchResult> { param }))
                {
                    // エラーの場合
                    return false;
                }
            }

            return true;

            /// <summary>
            /// 指定した画面項目に値を設定する処理
            /// </summary>
            /// <typeparam name="T">セットする値の型</typeparam>
            /// <param name="ctrlId">値をセットする一覧のコントロールID</param>
            /// <param name="results">セットする値リスト</param>
            /// <returns>エラーの場合False</returns>
            bool setFormByDataClass<T>(string ctrlId, IList<T> results)
            {
                if (results == null || results.Count == 0)
                {
                    // 一覧が0件の場合はエラーとしない
                    return true;
                }
                // ページ情報取得
                var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);
                // 一覧に値をセット
                bool retVal = SetSearchResultsByDataClass<T>(pageInfo, results, results.Count);
                return retVal;
            }

        }
        #endregion

        #region 登録
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool registFormEdit()
        {
            DateTime now = DateTime.Now;

            // 機番情報・機器情報取得
            Dao.searchResult registInfo = getRegistInfoBySearchResult(new List<short>() { ConductInfo.FormEdit.GroupNoMachine, ConductInfo.FormEdit.GroupNoEquipment }, now);

            // 入力チェック
            if (isErrorRegistForSingleFormEdit(registInfo))
            {
                return false;
            }

            // 機器の新規登録・修正の変更管理の有無
            bool isExistsMachineHistoryManagement = registInfo.HmMachineId > 0;

            // 画面に設定されている変更管理IDを判定
            if (registInfo.HistoryManagementId > 0)
            {
                // 変更管理が存在する場合

                // 排他チェック(変更管理)
                if (!checkExclusiveSingleForTable(ConductInfo.FormEdit.ControlId.Machine, new List<string>() { "hm_history_management" }))
                {
                    return false;
                }

                // 変更管理テーブル更新処理
                if (!updateHitoryManagement())
                {
                    return false;
                }
            }
            else
            {
                // 変更管理が存在しない場合

                if (registInfo.DispType == EditDispType.Change ||
                    registInfo.DispType == EditDispType.Edit)
                {
                    // 機器に「承認済」以外の変更管理が紐付いているかチェック
                    if (!checkHistoryManagement(registInfo.MachineId))
                    {
                        return false;
                    }

                    // 排他チェック(機番情報・機器情報)
                    if (!checkExclusiveSingleForTable(ConductInfo.FormEdit.ControlId.Machine, new List<string>() { "mc_machine", "mc_equipment" }))
                    {
                        return false;
                    }
                }

                // 変更管理テーブル 登録処理
                if (!registHistoryManagementInfo(ref registInfo, now))
                {
                    return false;
                }
            }

            // 機器の新規登録・修正の変更管理の有無
            if (isExistsMachineHistoryManagement)
            {
                // 変更管理関連テーブル 更新処理
                if (!updateHistoryInfo(registInfo, now))
                {
                    return false;
                }
            }
            else
            {
                // 画面表示タイプを判定
                if (registInfo.DispType == EditDispType.Change ||
                    registInfo.DispType == EditDispType.Edit)
                {
                    // 処理区分を設定(変更申請・申請内容修正)
                    registInfo.ExecutionDivision = (int)executionDiv.MachineEdit;
                }
                else
                {
                    // 処理区分を設定(新規申請・複写申請)
                    registInfo.ExecutionDivision = (int)executionDiv.MachineNew;
                }

                // 機番情報変更管理テーブル 登録処理
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Edit.InsertMachineInfo, SqlName.SubDir, registInfo, this.db, string.Empty))
                {
                    return false;
                }

                // 機器情報変更管理テーブル 登録処理
                if (!registEquipmentInfo(ref registInfo, registInfo.DispType == EditDispType.Change || registInfo.DispType == EditDispType.Edit))
                {
                    return false;
                }

                // 適用法規情報変更管理テーブル 登録処理
                if (!registApplicableInfo(registInfo, now, false))
                {
                    return false;
                }
            }

            // 再検索処理を実行(詳細画面に戻った際に検索するための情報を設定)
            setKeyId();

            return true;

            void setKeyId()
            {
                var pageInfo = GetPageInfo(ConductInfo.FormEdit.ControlId.Machine, this.pageInfoList);
                // 項目に設定
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, new List<Dao.searchResult> { registInfo }, 1);
            }

            bool updateHitoryManagement()
            {
                TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);

                // 変更管理テーブルデータクラス
                ComDao.HmHistoryManagementEntity historyInfo = new();
                historyInfo.HistoryManagementId = registInfo.HistoryManagementId;                            // 変更管理ID

                // 登録SQLでアンコメントする項目
                List<string> uncommentList = new() { "ApplicationUserId" };

                // 新規登録するデータを更新する場合は工場IDも更新
                bool isUncommentFactoryId = false;
                if (registInfo.DispType == EditDispType.New)
                {
                    historyInfo.FactoryId = historyManagement.getFactoryId((int)registInfo.LocationStructureId); // 工場ID
                    uncommentList.Add("FactoryId");
                }

                historyInfo.ApplicationUserId = int.Parse(this.UserId);                                      // 申請者ID

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref historyInfo, now, int.Parse(this.UserId), int.Parse(this.UserId));

                // 変更管理テーブル更新処理
                if (!historyManagement.UpdateHistoryManagement(historyInfo, uncommentList))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="registInfo">画面に入力された値</param>
        /// <returns>エラーの場合はTrue</returns>
        private bool isErrorRegistForSingleFormEdit(Dao.searchResult registInfo)
        {
            // 「新規登録申請」「複写申請」「機番情報がトランザクションテーブルに存在しない場合」は入力チェックを行わない
            if (registInfo.DispType == EditDispType.New ||
                registInfo.DispType == EditDispType.Copy ||
                new ComDao.McMachineEntity().GetEntity(registInfo.MachineId, this.db) == null)
            {
                return false;
            }

            bool isError = false;   // 処理全体でエラーの有無を保持

            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // エラー情報を画面に設定するためのマッピング情報リスト
            var info = getResultMappingInfo(ConductInfo.FormEdit.ControlId.Machine);

            // 機番IDより機番情報を取得
            ComDao.McMachineEntity oldMachineInfo = new();
            oldMachineInfo = oldMachineInfo.GetEntity(registInfo.MachineId, this.db);

            // 画面に入力された機器レベルとDBの機器レベルを比較
            if (registInfo.EquipmentLevelStructureId != oldMachineInfo.EquipmentLevelStructureId)
            {
                // 構成存在チェック
                if (!checkComposition(oldMachineInfo.MachineId, false))
                {
                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormEdit.ControlId.Machine));
                    isError = true;
                    string errMsg = GetResMessage(new string[] { ComRes.ID.ID141070001 }); // 「 構成機器が登録されている機器の為、削除できません。」
                    string val = info.getValName("equipment_level_structure_id"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val);
                    errorInfoDictionary.Add(errorInfo.Result);
                }
            }

            if (isError)
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
            }

            return isError;
        }

        /// <summary>
        /// 変更管理テーブル 登録処理
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="now">現在日時</param>
        /// <returns>registInfo</returns>
        private bool registHistoryManagementInfo(ref Dao.searchResult registInfo, DateTime now)
        {
            // 新規登録申請・複写申請の場合は機番IDを先に採番する
            if (registInfo.DispType == EditDispType.New ||
                registInfo.DispType == EditDispType.Copy)
            {
                registInfo.MachineId = getNewMachineId();
            }

            // 変更管理登録処理
            ComDao.HmHistoryManagementEntity historyInfo = new();

            // 変更管理テーブル登録処理
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);

            // 登録処理
            // 引数1：画面表示タイプが新規申請 または複写申請の場合は「新規登録申請」 それ以外は「変更申請」
            // 引数2：機番ID
            // 引数3：画面で入力された内容の場所階層ID
            (bool returnFlag, long historyManagementId) historyManagementResult =
                historyManagement.InsertHistoryManagement(registInfo.DispType == EditDispType.New || registInfo.DispType == EditDispType.Copy ? TMQConst.MsStructure.StructureId.ApplicationDivision.New : TMQConst.MsStructure.StructureId.ApplicationDivision.Update,
                                                          registInfo.MachineId,
                                                          historyManagement.getFactoryId((int)registInfo.LocationStructureId));

            // 登録に失敗した場合は終了
            if (!historyManagementResult.returnFlag)
            {
                return false;
            }

            // 新規採番した変更管理IDを設定
            registInfo.HistoryManagementId = historyManagementResult.historyManagementId;

            return true;

            // 新規に登録する機番IDを採番する
            long getNewMachineId()
            {
                // SQL取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Edit.GetNewMachineId, out string getMachineIdSql);
                // SQL実行
                IList<ComDao.McMachineEntity> results = db.GetListByDataClass<ComDao.McMachineEntity>(getMachineIdSql);
                if (results == null || results.Count == 0)
                {
                    return -1;
                }

                // 新規採番した機番ID
                return results[0].MachineId;
            }
        }

        /// <summary>
        /// 変更管理関連テーブル 更新処理
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool updateHistoryInfo(Dao.searchResult registInfo, DateTime now)
        {
            // 機番情報変更管理テーブル
            if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Edit.UpdateHmMcMachine, SqlName.SubDir, registInfo, this.db, string.Empty))
            {
                return false;
            }
            // 機器情報変更管理テーブル
            if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Edit.UpdateHmMcEquipment, SqlName.SubDir, registInfo, this.db, string.Empty))
            {
                return false;
            }

            // 適用法規情報変更管理テーブル
            if (!registApplicableInfo(registInfo, now, true))
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
