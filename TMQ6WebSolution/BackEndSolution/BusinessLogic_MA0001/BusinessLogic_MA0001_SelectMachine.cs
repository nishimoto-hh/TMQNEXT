using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MA0001.BusinessLogicDataClass_MA0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

namespace BusinessLogic_MA0001
{
    /// <summary>
    /// 保全活動(機器選択)
    /// </summary>
    public partial class BusinessLogic_MA0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 機器検索、機器選択画面 初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initSelectMachine()
        {
            //場所階層、職種取得元のコントロールID
            string controlId = this.FormNo == ConductInfo.FormSearchMachine.FormNo ? ConductInfo.FormDetail.ControlId.StructureId : ConductInfo.FormRegist.ControlId.StructureId;
            //場所階層設定先のコントロールID
            string conditionStructureId = this.FormNo == ConductInfo.FormSearchMachine.FormNo ? ConductInfo.FormSearchMachine.ControlId.StructureCondition : ConductInfo.FormSelectMachine.ControlId.StructureCondition;
            //職種設定先のコントロールID
            string conditionJobId = this.FormNo == ConductInfo.FormSearchMachine.FormNo ? ConductInfo.FormSearchMachine.ControlId.JobCondition : ConductInfo.FormSelectMachine.ControlId.JobCondition;

            // ページ情報取得
            var pageInfo = GetPageInfo(controlId, this.pageInfoList);
            //編集画面の場所階層、職種のデータを取得
            Dao.SelectMachineStructureCondition condition = new Dao.SelectMachineStructureCondition();
            List<Dictionary<string, object>> dic = ComUtil.GetDictionaryListByCtrlId(this.searchConditionDictionary, controlId);
            SetSearchConditionByDataClass(dic, controlId, condition, pageInfo);

            //最下層の構成IDを取得して場所階層ID、職種機種IDにセットする
            IList<Dao.SelectMachineStructureCondition> conditions = new List<Dao.SelectMachineStructureCondition>();
            conditions.Add(condition);
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.SelectMachineStructureCondition>(ref conditions, new List<StructureType> { StructureType.Location, StructureType.Job });
            // 階層情報をツリー選択ラベル用に設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.SelectMachineStructureCondition>(ref conditions, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

            // 場所階層の設定
            pageInfo = GetPageInfo(conditionStructureId, this.pageInfoList);
            if (SetSearchResultsByDataClass<Dao.SelectMachineStructureCondition>(pageInfo, new List<Dao.SelectMachineStructureCondition> { condition }, 1))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            // 職種の設定
            pageInfo = GetPageInfo(conditionJobId, this.pageInfoList);
            if (SetSearchResultsByDataClass<Dao.SelectMachineStructureCondition>(pageInfo, new List<Dao.SelectMachineStructureCondition> { condition }, 1))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 機器選択画面 検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchSelectMachine()
        {
            // 一覧のページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormSelectMachine.ControlId.List, this.pageInfoList);

            // 検索条件を画面より取得してデータクラスへセット
            Dao.searchMachineSearchCondition condition = GetConditionInfoByGroupNo<Dao.searchMachineSearchCondition>(ConductInfo.FormSelectMachine.GroupNo.SearchCondition);
            // 検索条件の地区・職種は各階層に値が設定されているが、検索には「指定された最下層の値」以下の全ての階層IDを用いるので設定
            setStructureLayerInfo(ref condition);
            if(condition.ManagementStandard == ManagementDivision.NotManagement)
            {
                //管理基準外から選択する場合、保全部位と保全項目は検索条件から除外する
                if(condition.InspectionSiteStructureId != null)
                {
                    condition.InspectionSiteStructureId = null;
                }
                if (condition.InspectionContentStructureId != null)
                {
                    condition.InspectionContentStructureId = null;
                }
            }
            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = ComUtil.GetNotNullNameByClass<Dao.searchMachineSearchCondition>(condition);
            if (condition.ManagementStandard == ManagementDivision.Management)
            {
                //管理基準から選択する場合
                listUnComment.Add(ManagementDivision.UncommentKey);
            }

            // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.SelectMachine.GetSelectMachineList, out string baseSql, listUnComment);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.SelectMachine.GetSelectMachineList, out string withSql, listUnComment);

            // 総件数取得SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, string.Empty, withSql);
            // 総件数を取得
            int cnt = db.GetCount(executeSql, condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                SetSearchResultsByDataClass<Dao.SelectMachineResult>(pageInfo, null, cnt);
                return false;
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY machine_no");

            // 一覧検索実行
            IList<Dao.SelectMachineResult> results = db.GetListByDataClass<Dao.SelectMachineResult>(selectSql.ToString(), condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 地区～設備、職種～機種小分類を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.SelectMachineResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.SelectMachineResult>(pageInfo, results, cnt))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 機器取得処理実行
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        private int GetSelectMachineData()
        {
            // 機器一覧の選択行を取得
            List<Dictionary<string, object>> machineList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormSelectMachine.ControlId.List);
            // 選択行が無ければ終了
            if (machineList == null || machineList.Count == 0)
            {
                return ComConsts.RETURN_RESULT.OK;
            }

            List<Dao.SelectMachineResult> list = new List<Dao.SelectMachineResult>();
            foreach (Dictionary<string, object> dic in machineList)
            {
                //データクラスに変換
                Dao.SelectMachineResult machine = new Dao.SelectMachineResult();
                SetExecuteConditionByDataClass<Dao.SelectMachineResult>(dic, ConductInfo.FormSelectMachine.ControlId.List, machine, DateTime.Now, this.UserId);

                list.Add(machine);
            }
            //追加行をソート
            list = list.OrderBy(x => x.JobName).ThenBy(x => x.LargeClassficationName).ThenBy(x => x.MiddleClassficationName).ThenBy(x => x.SmallClassficationName).ThenBy(x => x.MachineNo).ThenBy(x => x.InspectionSiteStructureId).ThenBy(x => x.InspectionContentStructureId).ToList();

            var pageInfo = GetPageInfo(ConductInfo.FormRegist.ControlId.MachineList, this.pageInfoList);
            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.SelectMachineResult>(pageInfo, list, list.Count))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return ComConsts.RETURN_RESULT.OK;
        }
    }
}
