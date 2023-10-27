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
    /// 機器台帳(構成タブ)
    /// </summary>
    public partial class BusinessLogic_MC0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlNameConstitution
        {
            /// <summary>SQL名：表示している機器の最新の機器レベル取得</summary>
            public const string GetEquipmentLevel = "GetEquipmentLevel";
            /// <summary>SQL名：構成機器 親子構成一覧取得(自分自身が「親機」の場合)</summary>
            public const string GetEquipmentLevelParent = "GetEquipmentLevelParent";
            /// <summary>SQL名：構成機器 親子構成一覧取得(自分自身が「機器」の場合)</summary>
            public const string GetEquipmentLevelMachine = "GetEquipmentLevelMachine";
            /// <summary>SQL名：構成機器 親子構成一覧取得(自分自身が「付属品」の場合)</summary>
            public const string GetEquipmentLevelAccessory = "GetEquipmentLevelAccessory";
            /// <summary>SQL名：構成機器 ループ構成一覧取得(自分自身が「ループ」の場合)</summary>
            public const string GetEquipmentLevelLoop = "GetEquipmentLevelLoop";
            /// <summary>SQL名：構成機器 ループ構成一覧取得(自分自身が「機器」の場合)</summary>
            public const string GetEquipmentLevelLoopMachine = "GetEquipmentLevelLoopMachine";
            /// <summary>SQL名：構成機器 ループ構成一覧取得(自分自身が「付属品」の場合)</summary>
            public const string GetEquipmentLevelLoopAccessoery = "GetEquipmentLevelLoopAccessoery";
            /// <summary>SQL名：構成機器 親子構成 更新</summary>
            public const string UpdateMachineParent = "UpdateMachineParent";
            /// <summary>SQL名：構成機器 ループ構成 削除</summary>
            public const string DeleteLoopInfoByLoopId = "DeleteLoopInfoByLoopId";
            /// <summary>SQL名：構成機器 親子構成一覧取得(自分自身が「機器」の場合、親機が無い場合)</summary>
            public const string GetEquipmentLevelMachineFromMyData = "GetEquipmentLevelMachineFromMyData";
            /// <summary>SQL名：構成機器 親子構成一覧取得 (自分自身が「付属品」の場合、親機が無い場合)</summary>
            public const string GetEquipmentLevelAccessoryFromMyData = "GetEquipmentLevelAccessoryFromMyData";
        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlIdConstitution
        {
            /// <summary>
            /// 構成機器 非表示一覧
            /// </summary>
            public const string HideCondition = "BODY_250_00_LST_1";
            /// <summary>
            /// 構成機器 親子構成一覧
            /// </summary>
            public const string ParentList = "BODY_260_00_LST_1";

            /// <summary>
            /// 構成機器 ループ構成一覧
            /// </summary>
            public const string LoopList = "BODY_270_00_LST_1";

        }

        /// <summary>
        /// 機器レベル
        /// </summary>
        private static class EquipmentLevel
        {
            /// <summary>
            /// 親機
            /// </summary>
            public const int Parent = 1;
            /// <summary>
            /// 機器
            /// </summary>
            public const int Machine = 2;
            /// <summary>
            /// 付属品
            /// </summary>
            public const int Accessory = 3;
            /// <summary>
            /// ループ
            /// </summary>
            public const int Loop = 4;

        }

        /// <summary>
        /// 機器レベル取得条件
        /// </summary>
        private static class condEquipmentLevel
        {
            /// <summary>構成グループID</summary>
            public const short StructureGroupId = 1170;
            /// <summary>連番</summary>
            public const short Seq = 1;
            /// <summary>データタイプ</summary>
            public const short DataType = 2;
        }

        /// <summary>
        /// SearchConditionDictionaryから「機器レベル」の値を取得する際の列名
        /// </summary>
        private const string ColNameEquipmentLevel = "equipment_level_structure_id";

        /// <summary>
        /// 親子構成一覧で行削除ボタン(-アイコン)クリック時に連携される削除ボタンの名称
        /// </summary>
        private const string DeleteParent = "DeleteParent";

        /// <summary>
        /// ループ構成一覧で行削除ボタン(-アイコン)クリック時に連携される削除ボタンの名称
        /// </summary>
        private const string DeleteLoop = "DeleteLoop";

        /// <summary>
        /// 構成検索
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool searchConstitution(long machineId)
        {
            // SQL名
            string sqlNameParent = string.Empty; // 親子構成一覧
            string sqlNameLoop = string.Empty;   // ループ構成一覧
            string getMyData = string.Empty;     // 自分自身のみ取得

            // 表示している機器の最新の機器レベルを取得
            if(!getEquipmentLevel(machineId, out long equipmentLevel))
            {
                return false;
            }

            // 機器レベルを判定
            int equipmentLevelNo = getEquipmentLevelNo(equipmentLevel);
            switch (equipmentLevelNo)
            {
                case EquipmentLevel.Parent:    // 親機
                    sqlNameParent = SqlNameConstitution.GetEquipmentLevelParent;
                    break;
                case EquipmentLevel.Machine:   // 機器
                    sqlNameParent = SqlNameConstitution.GetEquipmentLevelMachine;
                    sqlNameLoop = SqlNameConstitution.GetEquipmentLevelLoopMachine;
                    getMyData = SqlNameConstitution.GetEquipmentLevelMachineFromMyData;
                    break;
                case EquipmentLevel.Accessory: // 付属品
                    sqlNameParent = SqlNameConstitution.GetEquipmentLevelAccessory;
                    getMyData = SqlNameConstitution.GetEquipmentLevelAccessoryFromMyData;
                    break;
                case EquipmentLevel.Loop:      // ループ
                    sqlNameLoop = SqlNameConstitution.GetEquipmentLevelLoop;
                    break;
                default:
                    return true; // 上記に該当しない場合
            }

            // 機器レベル(非表示一覧)
            if(!setEquipmentLevelNo())
            {
                // 異常終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 親子構成一覧初期検索
            if (!string.IsNullOrEmpty(sqlNameParent))
            {
                setDataListConstitution<Dao.constitutionListResult>(machineId, sqlNameParent, TargetCtrlIdConstitution.ParentList, getMyData);
            }

            // ループ構成一覧初期検索(機器レベルが「ループ」「機器」の場合)
            if (!string.IsNullOrEmpty(sqlNameLoop))
            {
                setDataListConstitution<Dao.constitutionListResult>(machineId, sqlNameLoop, TargetCtrlIdConstitution.LoopList, getMyData);
            }

            return true;

            int getEquipmentLevelNo(long structure_id)
            {
                //構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                //構成グループID
                param.StructureGroupId = condEquipmentLevel.StructureGroupId;
                //連番
                param.Seq = condEquipmentLevel.Seq;
                //データタイプ
                param.DataType = condEquipmentLevel.DataType;

                // 機能レベル取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> equipmentLevelList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
                foreach (var equipmentLevel in equipmentLevelList)
                {
                    if (Convert.ToInt32(structure_id) == equipmentLevel.StructureId)
                    {
                        return int.Parse(equipmentLevel.ExData);
                    }
                }
                return ComConsts.RETURN_RESULT.NG;
            }

            bool setEquipmentLevelNo()
            {
                // 非表示一覧のデータクラス
                Dao.constitutionHideList resultEquipmentLevel = new Dao.constitutionHideList();
                // データクラスに機器レベルを設定
                resultEquipmentLevel.EquipmentLevel = equipmentLevelNo; // 機器レベル番号
                resultEquipmentLevel.MachineId = machineId;             // 機番ID
                IList<Dao.constitutionHideList> result = new List<Dao.constitutionHideList> { resultEquipmentLevel };

                // ページ情報取得
                var pageInfo = GetPageInfo(TargetCtrlIdConstitution.HideCondition, this.pageInfoList);

                //　非表示一覧に機器レベルを設定
                if (!SetSearchResultsByDataClass<Dao.constitutionHideList>(pageInfo, result, result.Count))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 検索結果一覧セット
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <param name="sqlName">SQL名称</param>
        /// <param name="ctrlId">検索結果設定先一覧のコントロールID</param>
        /// <param name="getFromMyData">自分自身以下のみ取得する場合true</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool setDataListConstitution<T>(long machineId, string sqlName, string ctrlId, string getFromMyData)
        {
            // 一覧検索SQL文の取得
            string sql;
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out sql);
            dynamic whereParam = null; // WHERE句パラメータ
            whereParam = new { MachineId = machineId, LanguageId = this.LanguageId };

            // 一覧検索実行
            IList<T> results = db.GetListByDataClass<T>(sql, whereParam);
            if (results == null || results.Count == 0)
            {
                if(!string.IsNullOrEmpty(getFromMyData))
                {
                    TMQUtil.GetFixedSqlStatement(SqlName.SubDir, getFromMyData, out sql);
                    results = db.GetListByDataClass<T>(sql, whereParam);
                }

                if (results == null || results.Count == 0)
                {
                    // 正常終了
                    return true;
                }
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<T>(ref results, new List<StructureType> { StructureType.Job }, this.db, this.LanguageId);

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<T>(pageInfo, results, results.Count))
            {
                // 異常終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 正常終了
            return true;
        }

        /// <summary>
        /// 表示している機器の最新の機器レベルを取得
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <param name="equipment_level_structure_id">機器レベル</param>
        /// <returns>取得できない場合False</returns>
        private bool getEquipmentLevel(long machineId, out long equipment_level_structure_id)
        {
            equipment_level_structure_id = 0;

            // 表示している機器の最新の機器レベルを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameConstitution.GetEquipmentLevel, out string outSql);

            // 検索条件を設定
            dynamic param = new ExpandoObject();
            param.MachineId = machineId; // 機番ID

            // SQL実行
            dynamic equipmtntLevel = db.GetEntity(outSql, param);

            // 取得できなかった場合
            if (equipmtntLevel == null || equipmtntLevel.equipment_level_structure_id == null)
            {
                return false;
            }

            // 取得した機器レベルを設定
            equipment_level_structure_id = equipmtntLevel.equipment_level_structure_id;

            return true;
        }

        /// <summary>
        /// 構成削除処理
        /// </summary>
        /// <param name="isParent">親子構成の場合：true、ループ構成の場合：false</param>
        /// <returns>エラーの場合False</returns>
        private bool updateConstitutionInfo(bool isParent)
        {
            string ctrlId = string.Empty;  // 一覧のコントロールID
            string sqlName = string.Empty; // SQL名

            // 更新する構成の判定
            if (isParent)
            {
                // 親子構成の場合
                ctrlId = TargetCtrlIdConstitution.ParentList;
                sqlName = SqlNameConstitution.UpdateMachineParent;
            }
            else
            {
                // ループ構成の場合
                ctrlId = TargetCtrlIdConstitution.LoopList;
                sqlName = SqlNameConstitution.DeleteLoopInfoByLoopId;
            }

            // 一覧の選択チェックボックスがチェックされている行を取得
            List<Dictionary<string, object>> resultList = getSelectedRowsByList(this.resultInfoDictionary, ctrlId);

            // 排他チェック
            if (!checkExclusiveList(ctrlId, resultList))
            {
                // 排他エラー
                return false;
            }

            // SQL取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out string sql))
            {
                return false;
            }

            foreach (var resultRow in resultList)
            {
                // 更新条件取得
                Dao.constitutionListResult condition = new();
                SetExecuteConditionByDataClass(resultRow, ctrlId, condition, DateTime.Now, this.UserId);

                // SQL実行
                int result = this.db.Regist(sql, condition);
                if (result < 0)
                {
                    // 更新エラー
                    return false;
                }
            }
            return true;
        }
    }
}
