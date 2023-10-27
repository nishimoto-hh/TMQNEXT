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
    /// 機器台帳 機器選択画面
    /// </summary>
    public partial class BusinessLogic_MC0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlNameSelectMachine
        {
            /// <summary>
            /// SQL名：機器選択 一覧検索(親子構成)
            /// </summary>
            public const string GetSelectMachineParentList = "GetSelectMachineParentList";
            /// <summary>
            /// SQL名：機器選択 一覧検索(ループ構成)
            /// </summary>
            public const string GetSelectMachineLoopList = "GetSelectMachineLoopList";
            /// <summary>
            /// SQL名：機器選択 場所階層、職種機種取得(初期化時)
            /// </summary>
            public const string GetSelectMachineListStructure = "GetSelectMachineListStructure";
            /// <summary>
            /// SQL名：機器選択 更新(親子構成)
            /// </summary>
            public const string UpdateSelectMachineParent = "UpdateSelectMachineParent";
            /// <summary>
            /// SQL名：機器選択 登録(ループ構成)
            /// </summary>
            public const string InsertSelectMachineLoop = "InsertSelectMachineLoop";

        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlIdSelectMachine
        {
            /// <summary>
            /// 機器選択 検索条件 非表示一覧
            /// </summary>
            public const string Condition00 = "COND_000_00_LST_3";
            /// <summary>
            /// 機器選択 検索条件(地区～設備)
            /// </summary>
            public const string Condition10 = "COND_010_00_LST_3";
            /// <summary>
            /// 機器選択 検索条件(職種～機器名称)
            /// </summary>
            public const string Condition20 = "COND_020_00_LST_3";
            /// <summary>
            /// 機器選択 検索条件(使用区分～製造年月)
            /// </summary>
            public const string Condition30 = "COND_030_00_LST_3";
            /// <summary>
            /// 機器選択 検索結果一覧
            /// </summary>
            public const string SearchList = "BODY_050_00_LST_3";

            /// <summary>
            /// 検索条件のグループ番号
            /// </summary>
            public const short SearchConditionGroupNo = 401;
        }

        // 構成機器タブで行追加ボタン(+アイコン)がクリックされた一覧
        private static class ListFlg
        {
            /// <summary>
            /// 構成機器一覧
            /// </summary>
            public const int Parent = 1;
            /// <summary>
            /// ループ構成一覧
            /// </summary>
            public const int Loop = 2;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initSelectMachine()
        {
            // コントロールIDにより構成機器タブの項目(非表示一覧)を取得
            Dao.constitutionHideList condition = getSearchCondition(TargetCtrlIdConstitution.HideCondition);

            // SQL文の取得
            string selectSql;
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameSelectMachine.GetSelectMachineListStructure, out selectSql);

            dynamic whereParam = new ExpandoObject(); // WHERE句パラメータ
            whereParam.MachineId = condition.MachineId;    // 機番ID

            // 場所階層、職種機種の初期値を設定する一覧のコントトールIDリスト
            string[] ctrlIdList = {TargetCtrlIdSelectMachine.Condition10, TargetCtrlIdSelectMachine.Condition20 };
            foreach (string ctrlId in ctrlIdList)
            {
                // ページ情報取得
                var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

                // 検索実行
                IList<Dao.selectMachineList> results = db.GetListByDataClass<Dao.selectMachineList>(selectSql.ToString(), whereParam);
                if (results == null || results.Count == 0)
                {
                    return false;
                }

                if (ctrlId == TargetCtrlIdSelectMachine.Condition20)
                {
                    // 機器レベルの構成ID取得
                    int equipmentLevel = getEquipmentLevel();
                    if(equipmentLevel != 0)
                    {
                        results[0].EquipmentLevelStructureId = equipmentLevel;
                    }
                }

                // 機能場所階層IDと職種機種階層IDから上位の階層を設定
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.selectMachineList>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

                // 検索結果の設定
                if (!SetSearchResultsByDataClass<Dao.selectMachineList>(pageInfo, results, results.Count))
                {
                    return false;
                }
            }

            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;

            int getEquipmentLevel()
            {
                //構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                //構成グループID
                param.StructureGroupId = condEquipmentLevel.StructureGroupId;
                //連番
                param.Seq = condEquipmentLevel.Seq;

                // 機能レベルリスト取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> equipmentLevelList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);

                // コントロールIDにより画面の項目(一覧)を取得
                Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlIdConstitution.HideCondition);

                // 画面項目定義の情報
                var mappingInfo = getResultMappingInfo(TargetCtrlIdConstitution.HideCondition);
                string equipmentLevelVal = getValNoByParam(mappingInfo, "EquipmentLevel");
                int equipmentLevelNo = int.Parse(result[equipmentLevelVal].ToString()); // 機器レベル番号

                // 自分自身の子のデータの機器レベルに変更する
                if(equipmentLevelNo == EquipmentLevel.Parent)
                {
                    // 自分自身が「親機」の場合
                    // 検索対象を「機器」に変更
                    equipmentLevelNo = EquipmentLevel.Machine;
                }
                else if(equipmentLevelNo == EquipmentLevel.Machine)
                {
                    // 自分自身が「機器」の場合
                    // 検索対象を「付属品」に変更
                    equipmentLevelNo = EquipmentLevel.Accessory;
                }
                else if(equipmentLevelNo == EquipmentLevel.Loop)
                {
                    // 自分自身が「ループ」の場合
                    // 検索対象を「機器」に変更
                    equipmentLevelNo = EquipmentLevel.Machine;
                }

                // 機器レベルの構成IDを取得
                foreach (var equipmentLevel in equipmentLevelList)
                {
                    if (equipmentLevelNo.ToString() == equipmentLevel.ExData)
                    {
                        return equipmentLevel.StructureId;
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchSelectMachine()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlIdSelectMachine.SearchList, this.pageInfoList);

            // 検索条件の取得
            Dao.selectMachineSearchCondition condition = getConditionInfoGroup<Dao.selectMachineSearchCondition>(TargetCtrlIdSelectMachine.SearchConditionGroupNo);

            // 検索条件の地区・職種は各階層に値が設定されているが、検索には「指定された最下層の値」以下の全ての階層IDを用いるので設定
            setStructureLayerInfo(ref condition);

            // 検索するSQLを取得
            string sqlName = string.Empty;
            sqlName = getSqlName(out long? machineId);

            // 機番IDを設定
            condition.MachineId = machineId;

            // 元の機器が変更管理工場か取得
            if (getHistManageFlg(machineId))
            {
                condition.HistoryManage = 1;
            }
            else
            {
                condition.HistoryManage = null;
            }

            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = ComUtil.GetNotNullNameByClass<Dao.selectMachineSearchCondition>(condition);

            // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out string baseSql, listUnComment);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, sqlName, out string withSql, listUnComment);

            // 総件数取得SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, string.Empty, withSql);
            // 総件数を取得
            int cnt = db.GetCount(executeSql, condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            // 一覧検索SQL文の取得
            StringBuilder sql = new StringBuilder(TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql));
            sql.Append("ORDER BY machine_no");

            // 一覧検索実行
            IList<Dao.selectMachineList> results = db.GetListByDataClass<Dao.selectMachineList>(sql.ToString(), condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.selectMachineList>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.selectMachineList>(pageInfo, results, results.Count))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;

            string getSqlName(out long? machineId)
            {
                // コントロールIDにより画面の項目(非表示一覧)を取得
                Dao.constitutionHideList condition = getSearchCondition(TargetCtrlIdSelectMachine.Condition00);
                // 機番IDを取得
                machineId = condition.MachineId;

                //　行追加ボタン(+アイコン)がクリックされた一覧を判定
                if (condition.ListFlg == ListFlg.Parent)
                {
                    // 親子構成一覧
                    return SqlNameSelectMachine.GetSelectMachineParentList;
                }
                else
                {
                    // ループ構成一覧
                    return SqlNameSelectMachine.GetSelectMachineLoopList;
                }
            }
        }

        /// <summary>
        /// 変更管理する工場の機器かどうかを判定
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>結果</returns>
        private bool getHistManageFlg(long? machineId)
        {
            // SQL文の取得
            string selectSql;
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameSelectMachine.GetSelectMachineListStructure, out selectSql);

            dynamic whereParam = new ExpandoObject(); // WHERE句パラメータ
            whereParam.MachineId = machineId;    // 機番ID
                                                           // 検索実行
            IList<Dao.selectMachineList> results = db.GetListByDataClass<Dao.selectMachineList>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }
            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.selectMachineList>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

            // 変更管理フラグを取得
            TMQUtil.HistoryManagement history = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConsts.MsStructure.StructureId.ApplicationConduct.HM0001);
            // 変更管理対象外の工場の権限有無(工場ID未指定の、ユーザに対する権限を取得する場合に使用)
            bool isExistsNoHistory = false;
            // 工場ID省略時は引数に含めずに、変更管理フラグを取得
            bool isHitoryManagementFlg = results[0].FactoryId == -1 ? history.IsHistoryManagementFactoryUserBelong(out isExistsNoHistory) : history.IsHistoryManagementFactory((int)results[0].FactoryId);
            // 画面に設定する内容
            Dao.HiddenInfo hideInfo = new();
            // 変更管理ボタンの表示フラグ
            // 0:変更管理対象外、表示しない
            // 1:変更管理対象、表示する
            // 2:混在する(一覧画面のみ)、変更管理する場合としない場合両方を表示
            hideInfo.IsHistoryManagementFlg = isHitoryManagementFlg ? (isExistsNoHistory ? 2 : 1) : 0;

            if (hideInfo.IsHistoryManagementFlg == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistSelectMachine()
        {
            // 一覧の選択チェックボックスがチェックされている行を取得
            List<Dictionary<string, object>> resultList = getSelectedRowsByList(this.resultInfoDictionary, TargetCtrlIdSelectMachine.SearchList);

            // コントロールIDにより画面の項目(非表示一覧)を取得
            Dao.constitutionHideList condition = getSearchCondition(TargetCtrlIdSelectMachine.Condition00);

            // 更新処理を行うSQL
            string sqlName = string.Empty;

            // //　行追加ボタン(+アイコン)がクリックされた一覧を判定
            if (condition.ListFlg == ListFlg.Parent)
            {
                // 親子構成一覧
                // SQL名
                sqlName = SqlNameSelectMachine.UpdateSelectMachineParent;

                // 排他チェック(親子構成の場合のみ)
                if (!checkExclusiveList(TargetCtrlIdSelectMachine.SearchList, resultList))
                {
                    // 排他エラー
                    return false;
                }
            }
            else
            {
                // ループ構成一覧
                sqlName = SqlNameSelectMachine.InsertSelectMachineLoop;
            }

            // SQL取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out string sql))
            {
                return false;
            }

            // 登録・更新処理実行
            foreach (var resultRow in resultList)
            {
                // 更新条件取得
                Dao.selectMachineList registCondition = new();
                SetExecuteConditionByDataClass(resultRow, TargetCtrlIdSelectMachine.SearchList, registCondition, DateTime.Now, this.UserId, this.UserId);
                // ループ元の機番ID
                registCondition.LoopMachineId = condition.MachineId;

                if (condition.ListFlg == ListFlg.Parent)
                {
                    // 親の機番ID
                    registCondition.MachineId = condition.MachineId;
                }

                // SQL実行
                int result = this.db.Regist(sql, registCondition);
                if (result < 0)
                {
                    // 更新エラー
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 画面の内容をグループNoにより取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <returns>登録内容のデータクラス</returns>
        private T getConditionInfoGroup<T>(short groupNo)
             where T : new()
        {
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.searchConditionDictionary, ctrlIdList, true);

            T resultInfo = new();
            // コントロールIDごとに繰り返し
            foreach (var ctrlId in ctrlIdList)
            {
                // コントロールIDより画面の項目を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ctrlId);
                var mapInfo = grpMapInfo.selectByCtrlId(ctrlId);

                // ページ情報取得
                var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);
                // 検索条件データの設定
                if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, ctrlId, resultInfo, pageInfo))
                {
                    // エラーの場合終了
                    return resultInfo;
                }
            }
            return resultInfo;
        }

        /// <summary>
        /// 画面より取得した検索条件は地区と職種が各階層の値なので、階層IDを取得し設定する
        /// </summary>
        /// <param name="condition">ref 画面より取得した検索条件</param>
        /// <remarks>指定された階層ID配下の階層IDリストを検索条件に設定</remarks>
        private void setStructureLayerInfo(ref Dao.selectMachineSearchCondition condition)
        {
            // 共通処理で階層IDを設定するが、画面の検索条件は画面定義の関係でプロパティ名にIDでなく名称を使用しているので置き換える
            Dao.StructureLayerCondition structureInfo = new();
            // 地区
            structureInfo.DistrictId = condition.DistrictName;
            structureInfo.FactoryId = condition.FactoryName;
            structureInfo.PlantId = condition.PlantName;
            structureInfo.SeriesId = condition.SeriesName;
            structureInfo.StrokeId = condition.StrokeName;
            structureInfo.FacilityId = condition.FacilityName;
            // 職種
            structureInfo.JobId = condition.JobName;
            structureInfo.LargeClassficationId = condition.LargeClassficationName;
            structureInfo.MiddleClassficationId = condition.MiddleClassficationName;
            structureInfo.SmallClassficationId = condition.SmallClassficationName;
            // 地区と職種の階層IDを取得
            IList<Dao.StructureLayerCondition> tempList = new List<Dao.StructureLayerCondition> { structureInfo };
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.StructureLayerCondition>(ref tempList, new List<StructureType> { StructureType.Location, StructureType.Job });

            // 取得した階層ID配下の階層IDリストを検索条件に設定
            if (tempList[0].LocationStructureId != null)
            {
                // 機能場所階層ID
                condition.LocationStructureIdList = GetLowerStructureIdList(new List<int> { tempList[0].LocationStructureId ?? -1 });
            }
            if (tempList[0].JobStructureId != null)
            {
                // 職種機種階層ID
                condition.JobStructureIdList = GetLowerStructureIdList(new List<int> { tempList[0].JobStructureId ?? -1 });
            }
        }

        /// <summary>
        /// パラメータ名と一致する項目番号を返す
        /// </summary>
        /// <param name="info">一覧情報</param>
        /// <param name="keyName">項目キー名</param>
        /// <returns>項目番号</returns>
        private　string getValNoByParam(MappingInfo info, string keyName)
        {
            // パラメータ名と一致する項目番号を返す
            return info.Value.First(x => x.ParamName.Equals(keyName)).ValName;
        }

        /// <summary>
        /// searchConditionDictionaryから指定した一覧の値を取得
        /// </summary>
        /// <param name="ctrlId">取得対象一覧のコントロールID</param>
        /// <returns>取得したデータ</returns>
        private Dao.constitutionHideList getSearchCondition(string ctrlId)
        {
            // 非表示の一覧より値を取得
            Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>> { result };

            Dao.constitutionHideList condition = new();

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            if (!SetSearchConditionByDataClass(resultList, ctrlId, condition, pageInfo))
            {
                return condition;
            }
            return condition;
        }
    }
}
