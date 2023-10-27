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
using Dao = BusinessLogic_MC0001.BusinessLogicDataClass_MC0001;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_MC0001
{
    /// <summary>
    /// 機器台帳(保全活動タブ)
    /// </summary>
    public partial class BusinessLogic_MC0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlNameMaintainanceActivity
        {
            /// <summary>SQL名：長期計画一覧取得</summary>
            public const string GetMaintainanceActivityList = "GetMaintainanceActivityList";
        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlIdMaintainanceActivity
        {
            /// <summary>
            /// 長期計画 長期計画一覧
            /// </summary>
            public const string MaintainanceActivityList220 = "BODY_220_00_LST_1";
        }

        /// <summary>
        /// 保全活動検索
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool searchMaintainanceActivity(long machineId)
        {

            // 対象機器の工場ID取得
            dynamic whereParam = new { MachineId = machineId };
            string sql;
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, typeLst, this.db, this.LanguageId);
            //int factoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
            int factoryId = (int)results[0].FactoryId;
            List<int> factoryIdList = TMQUtil.GetFactoryIdList();
            factoryIdList.Add(factoryId);

            //保全活動一覧初期検索
            setDataList<Dao.maintainanceActivityListResult>(machineId, SqlNameMaintainanceActivity.GetMaintainanceActivityList, TargetCtrlIdMaintainanceActivity.MaintainanceActivityList220, factoryIdList);

            return true;
        }

    }
}
