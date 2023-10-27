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
        private enum EditDispType
        {
            /// <summary>新規申請</summary>
            New,
            /// <summary>複写申請</summary>
            Copy,
            /// <summary>変更申請</summary>
            Change,
            /// <summary>申請内容修正</summary>
            Edit
        }

        /// <summary>
        /// SQLの実行時にアンコメントする項目
        /// </summary>
        private class UnCommentItem
        {
            /// <summary>
            /// 機器の新規登録・複写登録(機番情報)
            /// </summary>
            public const string NewMachineId = "NewMachineId";
            /// <summary>
            /// 機器の修正(機番情報)
            /// </summary>
            public const string DefaultMachineId = "DefaultMachineId";
            /// <summary>
            /// 機器の新規登録・複写登録(機器情報)
            /// </summary>
            public const string NewEquipmentId = "NewEquipmentId";
            /// <summary>
            /// 機器の修正(機器情報)
            /// </summary>
            public const string DefaultEquipmentId = "DefaultEquipmentId";
        }

        /// <summary>
        /// 画面の呼び出し元のコントロールIDを取得
        /// </summary>
        /// <returns>詳細編集画面の表示種類</returns>
        private EditDispType getEditDispType()
        {
            // コントロールIDで比較
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId);
            if (compareId.IsStartId(ConductInfo.FormList.ButtonId.New))
            {
                // 新規申請
                return EditDispType.New;
            }
            else if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.CopyRequest))
            {
                // 複写申請
                return EditDispType.Copy;
            }
            else if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.ChangeRequest))
            {
                // 変更申請
                return EditDispType.Change;
            }
            else if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.EditRequest))
            {
                // 申請内容修正
                return EditDispType.Edit;
            }
            else
            {
                // 到達不能
                throw new Exception();
            }
        }

        #region 検索
        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchEditList()
        {
            // 詳細編集画面の表示種類を判定
            EditDispType dispType = getEditDispType();
            switch (dispType)
            {
                case EditDispType.New: // 新規申請
                    return searchResultNew();
                    break;

                case EditDispType.Copy: // 複写申請
                    return searchResultTransaction(executionDiv.machineNew);
                    break;

                case EditDispType.Change: // 変更申請
                    return searchResultTransaction(executionDiv.machineEdit);
                    break;

                default:
                    return false;
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
            result.DateOfInstallation = DateTime.Now;                             // 設置日
            result.DateOfManufacture = DateTime.Now;                              // 製造日
            result.ExecutionDivision = ((int)executionDiv.machineNew).ToString(); // 実行処理区分(機器の新規・複写登録)

            IList<Dao.searchResult> resultList = new List<Dao.searchResult> { result };

            // 場所階層IDと職種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref resultList, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

            // 検索結果の設定(機番情報)
            if (!initFormByParam(resultList[0], getResultMappingInfoByGrpNo(ConductInfo.FormEdit.GroupNoMachine).CtrlIdList) ||
                !initFormByParam(resultList[0], getResultMappingInfoByGrpNo(ConductInfo.FormEdit.GroupNoEquipment).CtrlIdList))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            this.Status = CommonProcReturn.ProcStatus.Valid;
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
        /// 検索処理(複写申請・変更申請)
        /// </summary>
        /// <param name="executionDivision">実行処理区分</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchResultTransaction(executionDiv executionDivision)
        {
            // 検索条件を取得
            Dao.detailSearchCondition condition = getDetailSearchCondition();

            // SQL取得
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDirMachine, SqlName.Detail.GetMachineDetail, out string sql);

            // SQL実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 実行処理区分(機器の新規・複写登録 または 機器の修正)
            results[0].ExecutionDivision = ((int)executionDivision).ToString();

            // 複写申請の場合は検索結果を別に設定
            if (executionDivision == executionDiv.machineNew)
            {
                results[0].DateOfInstallation = DateTime.Now; // 設置日
                results[0].DateOfManufacture = DateTime.Now;  // 製造日
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

            // 検索結果の設定(機番情報)
            if (!setSearchResult(ConductInfo.FormEdit.GroupNoMachine, results))
            {
                return false;
            }

            // 検索結果の設定(機器情報)
            if (!setSearchResult(ConductInfo.FormEdit.GroupNoEquipment, results))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 指定された一覧に値を設定する
        /// </summary>
        /// <param name="param">長期計画の内容</param>
        /// <param name="toCtrlIds">値を設定する一覧のコントロールID</param>
        /// <returns>エラーの場合False</returns>
        private bool initFormByParam(Dao.searchResult param, List<string> toCtrlIds)
        {
            // 一覧に対して繰り返し値を設定する
            foreach (var ctrlId in toCtrlIds)
            {
                if (!setFormByDataClass(ctrlId, new List<Dao.searchResult> { param }))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }

            this.Status = CommonProcReturn.ProcStatus.Valid;
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
                this.Status = CommonProcReturn.ProcStatus.Valid;
                return retVal;
            }

        }
        #endregion

        #region 登録

        private bool registFormEdit()
        {
            DateTime now = DateTime.Now;

            // 機番情報取得
            Dao.registMachineInfo machineInfo = getRegistMachineInfo(now);


            // 入力チェック


            // 排他チェック


            // 変更管理テーブル・変更管理詳細テーブル 登録処理
            if (!registHistoryManagementInfo(machineInfo.ExecutionDivision, out long historyManagementId, out long historyManagementDetailId))
            {
                return false;
            }

            // 機番情報変更管理テーブル 登録処理
            if (!registMachineInfo(machineInfo, now, historyManagementDetailId, out long machineId))
            {
                return false;
            }

            // 機器情報変更管理テーブル 登録処理
            if (!registEquipmentInfo(now, historyManagementDetailId, machineId, machineInfo.ExecutionDivision, out long equipmentId))
            {
                return false;
            }

            // 再検索処理を実行(詳細画面に戻った際に検索するための情報を設定)
            setKeyId();

            return true;

            void setKeyId()
            {
                var pageInfo = GetPageInfo(ConductInfo.FormEdit.ControlId.Machine, this.pageInfoList);
                Dao.searchResult result = new();
                result.HistoryManagementId = historyManagementId; // 変更管理ID
                result.MachineId = machineId;                     // 機番ID
                result.EquipmentId = equipmentId;                 // 機器ID
                // 項目に設定
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, new List<Dao.searchResult> { result }, 1);
            }
        }

        /// <summary>
        /// 画面に入力された機番情報を取得する
        /// </summary>
        /// <param name="now">現在日時</param>
        /// <param name="execDiv">実行処理区分</param>
        /// <returns>機番情報の登録データ</returns>
        private Dao.registMachineInfo getRegistMachineInfo(DateTime now)
        {
            // 機番情報取得
            Dao.registMachineInfo registMachineInfo = getRegistInfo<Dao.registMachineInfo>(ConductInfo.FormEdit.GroupNoMachine, now);

            //最下層の構成IDを取得して機能場所階層ID、職種機種階層IDにセットする
            IList<Dao.searchResult> results = new List<Dao.searchResult>();
            Dao.searchResult result = getRegistInfo<Dao.searchResult>(new List<short> { ConductInfo.FormEdit.GroupNoMachine, ConductInfo.FormEdit.GroupNoEquipment }, now);
            results.Add(result);
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.searchResult>(ref results, new List<TMQUtil.StructureLayerInfo.StructureType> { TMQUtil.StructureLayerInfo.StructureType.Location, TMQUtil.StructureLayerInfo.StructureType.Job });

            // 取得した値を登録情報に設定
            registMachineInfo.LocationStructureId = result.LocationStructureId;                       // 場所階層ID
            registMachineInfo.JobStructureId = result.JobStructureId;                                 // 職種階層ID
            registMachineInfo.ConservationStructureId = result.InspectionSiteConservationStructureId; // 保全方式

            return registMachineInfo;
        }

        /// <summary>
        /// 変更管理テーブル・変更管理詳細テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <param name="historyManagementDetailId">変更管理詳細ID</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHistoryManagementInfo(int executionDivision, out long historyManagementId, out long historyManagementDetailId)
        {
            historyManagementId = -1;
            historyManagementDetailId = -1;

            // 変更管理登録処理
            ComDao.HmHistoryManagementEntity historyInfo = new();

            // 変更管理テーブル登録処理
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);

            // 機器の新規・複写登録の場合は申請区分は「新規登録申請」
            // 機器の修正の場合は申請区分は「変更申請」
            (bool returnFlag, long historyManagementId, long historyManagementDetailId) historyManagementResult =
                historyManagement.InsertHistoryManagementBaseTable(executionDivision == (int)executionDiv.machineNew ? TMQConst.MsStructure.StructureId.ApplicationDivision.New : TMQConst.MsStructure.StructureId.ApplicationDivision.Update, executionDivision);
            if (!historyManagementResult.returnFlag)
            {
                return false;
            }

            // 新規採番した変更管理ID・変更管理詳細IDを設定
            historyManagementId = historyManagementResult.historyManagementId;
            historyManagementDetailId = historyManagementResult.historyManagementDetailId;

            return true;
        }

        /// <summary>
        /// 機番情報変更管理テーブル 登録処理
        /// </summary>
        /// <param name="now">現在日時</param>
        /// <param name="historyManagementDetailId">変更管理詳細ID</param>
        /// <param name="outMachineId">機番ID</param>
        /// <param name="executionDivision">実行処理区分</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registMachineInfo(Dao.registMachineInfo registMachineInfo, DateTime now, long historyManagementDetailId, out long outMachineId)
        {
            outMachineId = -1;

            // 機番情報登録処理
            // 採番した変更管理詳細IDを設定
            registMachineInfo.HistoryManagementDetailId = historyManagementDetailId;

            // 「機器の新規登録・複写登録」と「機器の修正」の場合で機番IDを新規採番するか既存の機番IDを使用するか決める
            List<string> listUnComment = new();
            if (registMachineInfo.ExecutionDivision == (int)executionDiv.machineNew)
            {
                // 機器の新規登録・複写登録 の場合は機番IDを新規採番する
                listUnComment.Add(UnCommentItem.NewMachineId);
            }
            else
            {
                // 機器の修正 の場合は既存の機番IDを登録する
                listUnComment.Add(UnCommentItem.DefaultMachineId);

                // 既存の機番IDを設定
                outMachineId = registMachineInfo.MachineId;
            }

            // SQL実行
            (bool returnFlag, long machineId) machineResult = registInsertDb<Dao.registMachineInfo>(registMachineInfo, SqlName.Edit.InsertMachineInfo, listUnComment);
            if (!machineResult.returnFlag)
            {
                return false;
            }

            // 機番IDが初期状態の場合は新規採番した機番IDを設定
            if (outMachineId == -1)
            {
                outMachineId = machineResult.machineId;
            }

            return true;
        }

        /// <summary>
        /// 機器情報変更管理テーブル 登録処理
        /// </summary>
        /// <param name="now">現在日時</param>
        /// <param name="historyManagementDetailId">変更管理詳細IS</param>
        /// <param name="machineId">機番ID</param>
        /// <param name="executionDivision">実行処理区分</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registEquipmentInfo(DateTime now, long historyManagementDetailId, long machineId, int executionDivision, out long outEquipmentId)
        {
            outEquipmentId = -1;

            // 機器情報取得
            Dao.registEquipmentInfo registEquipmentInfo = getRegistInfo<Dao.registEquipmentInfo>(ConductInfo.FormEdit.GroupNoEquipment, now);

            // 機器情報登録処理
            // 採番した値を設定
            registEquipmentInfo.HistoryManagementDetailId = historyManagementDetailId; // 変更管理詳細ID
            registEquipmentInfo.MachineId = machineId;                                 // 機番ID

            // 「機器の新規登録・複写登録」と「機器の修正」の場合で機器IDを新規採番するか既存の機器IDを使用するか決める
            List<string> listUnComment = new();
            if (executionDivision == (int)executionDiv.machineNew)
            {
                // 機器の新規登録・複写登録 の場合は機器IDを新規採番する
                listUnComment.Add(UnCommentItem.NewEquipmentId);
            }
            else
            {
                // 機器の修正 の場合は既存の機器IDを登録する
                listUnComment.Add(UnCommentItem.DefaultEquipmentId);

                // 既存の機器IDを設定
                outEquipmentId = registEquipmentInfo.EquipmentId;
            }

            // SQL実行
            (bool returnFlag, long equipmentId) equipmentResult = registInsertDb<Dao.registEquipmentInfo>(registEquipmentInfo, SqlName.Edit.InsertEquipmentInfo, listUnComment);
            if (!equipmentResult.returnFlag)
            {
                return false;
            }

            // 機器IDが初期状態の場合は新規採番した機器IDを設定
            if (outEquipmentId == -1)
            {
                outEquipmentId = equipmentResult.equipmentId;
            }

            return true;
        }

        /// <summary>
        /// 新規登録SQL実行処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <param name="sqlName">SQLファイル名</param>
        /// <param name="listUnComment">SQL内でアンコメントする項目名</param>
        /// <returns>returnFlag:エラーの場合False、id:登録データのID</returns>
        private (bool returnFlag, long id) registInsertDb<T>(T registInfo, string sqlName, List<string> listUnComment)
        {
            string sql;
            // SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out sql, listUnComment))
            {
                return (false, -1);
            }

            // アンコメントする項目に応じてSQL実行メソッドを切替
            if (listUnComment.Contains(UnCommentItem.DefaultMachineId) || listUnComment.Contains(UnCommentItem.DefaultEquipmentId))
            {
                // 機器の修正
                long returnId = (long)db.Regist(sql, registInfo);
                return (returnId > 0, 0);
            }
            else
            {
                // 機器の新規登録・複写登録
                long returnId = db.RegistAndGetKeyValue<long>(sql, out bool isError, registInfo);
                return (!isError, returnId);
            }
        }
        #endregion
    }
}
