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
            if(!searchManagementStandardsList(condition))
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
            // 検索条件を作成
            Dao.detailSearchCondition condition = new();
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormList.ControlId.List);

            // 変更管理ID、機番ID、データタイプ(トランザクションのデータか変更管理のデータか)を取得
            SetDataClassFromDictionary(targetDic, ConductInfo.FormList.ControlId.List, condition, new List<string> { "HistoryManagementId", "MachineId", "DataType" });
            condition.LanguageId = this.LanguageId; // 言語ID

            return condition;
        }

        /// <summary>
        /// 機番情報・機器情報 検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchMachineAndEquipmentInfo(Dao.detailSearchCondition condition)
        {
            // 表示するデータタイプに応じたSQLを取得
            string sqlName = string.Empty;
            switch (condition.DataType)
            {
                case (int)dataType.transaction: // トランザクションデータ
                    sqlName = SqlName.Detail.GetTransactionMachineInfo;
                    break;

                case (int)dataType.history: // 変更管理データ
                    sqlName = SqlName.Detail.GetHistoryMachineInfo;
                    break;
                default:
                    // 該当しない場合はエラー
                    return false;
            }

            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out string sql);

            // SQL実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);       // 変更管理データ
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, new List<StructureType> { StructureType.OldLocation, StructureType.OldJob }, this.db, this.LanguageId, true); // トランザクションデータ

            // 変更があった項目を取得
            getValueChangedItem(results);

            // 検索結果の設定(機番情報)
            if (!setSearchResult(ConductInfo.FormDetail.GroupNoMachine))
            {
                return false;
            }

            // 検索結果の設定(機器情報)
            if(!setSearchResult(ConductInfo.FormDetail.GroupNoEquipment))
            {
                return false;
            }

            return true;

            // 検索結果を一覧に設定する
            bool setSearchResult(short groupNo)
            {
                // 画面定義のグループ番号よりコントロールグループIDを取得
                List<string> ctrlIdList = getResultMappingInfoByGrpNo(groupNo).CtrlIdList;

                // グループ番号内の一覧に対して繰り返し値を設定する
                foreach (var ctrlId in ctrlIdList)
                {
                    // 画面項目に値を設定
                    if (!SetFormByDataClass(ctrlId, results))
                    {
                        // エラーの場合
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return false;
                    }
                }

                return true;
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
            string sqlName = string.Empty;
            switch (condition.DataType)
            {
                case (int)dataType.transaction: // トランザクションデータ
                    sqlName = SqlName.Detail.GetTransactionManagementStandardsList;
                    break;

                case (int)dataType.history: // 変更管理データ
                    sqlName = SqlName.Detail.GetHistoryManagementStandardsList;
                    break;
                default:
                    // 該当しない場合はエラー
                    return false;
            }

            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out string sql);

            // SQL実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }









            return true;
        }
        #endregion

        #region 登録
        #endregion
    }
}
