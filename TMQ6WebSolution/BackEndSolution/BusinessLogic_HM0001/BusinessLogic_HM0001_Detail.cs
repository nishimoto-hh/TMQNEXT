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
        private bool searchDetailList()
        {
            // 検索条件を取得
            Dao.detailSearchCondition condition = getDetailSearchCondition();

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
        /// <returns>検索条件</returns>
        private Dao.detailSearchCondition getDetailSearchCondition()
        {
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
            else if(compareId.IsStartId(ConductInfo.FormEdit.ButtonId.Regist))
            {
                // 詳細編集画面の登録ボタンクリック
                conditionDictionary = this.searchConditionDictionary;
                fromCtrlId = ConductInfo.FormEdit.ControlId.Machine;
            }
            else if(compareId.IsStartId(ConductInfo.FormDetail.ButtonId.CopyRequest) ||
                    compareId.IsStartId(ConductInfo.FormDetail.ButtonId.ChangeRequest) ||
                    compareId.IsStartId(ConductInfo.FormEdit.ButtonId.Back))
            {
                // 詳細画面の複写申請ボタンクリック
                // 詳細画面の変更申請ボタンクリック
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

        #region 登録
        private bool registFormDetail()
        {
            // コントロールIDで比較
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId);

            if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.DeleteRequest))
            {
                // 削除申請
                return deleteRequest();
            }
            else
            {
                // 到達不能
                throw new Exception();
            }
        }
        #endregion

        #region 削除申請
        private bool deleteRequest()
        {
            // 削除条件を取得
            Dao.detailSearchCondition condition = getDetailSearchCondition();




            return true;
        }



        #endregion

        #region 登録
        #endregion
    }
}
