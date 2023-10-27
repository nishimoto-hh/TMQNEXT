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
    /// 機器台帳(機種別仕様タブ)
    /// </summary>
    public partial class BusinessLogic_MC0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlNameSpec
        {
            /// <summary>SQL名：場所階層・職種取得</summary>
            public const string GetEquipLocationJob = "GetEquipLocationJob";
            /// <summary>SQL名：機種別仕様取得</summary>
            public const string GetEquipSpec = "GetEquipSpec";
            /// <summary>SQL名：機種別仕様レイアウト取得</summary>
            public const string GetEquipSpecLayout = "GetEquipSpecLayout";
            /// <summary>SQL名：機種別仕様単位取得</summary>
            public const string GetEquipSpecLayoutUnit = "GetEquipSpecLayoutUnit";
            /// <summary>SQL名：機種別仕様登録</summary>
            public const string InsertEquipmentSpec = "InsertEquipmentSpec";
            /// <summary>SQL名：機種別仕様更新</summary>
            public const string UpdateEquipmentSpec = "UpdateEquipmentSpec";

        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlIdSpec
        {
            /// <summary>
            /// 機種別仕様一覧
            /// </summary>
            public const string SpecDetail70 = "BODY_070_00_LST_1";
            /// <summary>
            /// 機種別仕様一覧
            /// </summary>
            public const string SpecEdit00 = "BODY_000_00_LST_4";
            /// <summary>
            /// 機種別仕様隠し一覧(詳細画面)
            /// </summary>
            public const string SpecDetail75 = "BODY_075_00_LST_1";
            /// <summary>
            /// 機種別仕様隠し一覧(編集画面)
            /// </summary>
            public const string SpecEdit20 = "BODY_020_00_LST_4";
        }

        /// <summary>
        /// 入力形式区分
        /// </summary>
        private static class SpecType
        {
            /// <summary>テキスト</summary>
            public const int Text = 1;
            /// <summary>数値</summary>
            public const int Num = 2;
            /// <summary>数値(範囲)</summary>
            public const int NumMinMax = 3;
            /// <summary>選択</summary>
            public const int Select = 4;
        }

        /// <summary>
        /// 機種別仕様検索処理
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool searchSpec()
        {
            // 機器ID取得
            long equipid = getEquipmentId(false);
            // 機器IDに紐づく工場職種を取得
            string sql;
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameSpec.GetEquipLocationJob, out sql);
            // SQL実行
            dynamic list = db.GetList(sql, new { EquipmentId = equipid });
            long? factoryId = list[0].factory_id;
            string job_structure_id_all = list[0].job_structure_id_all;

            // 画面レイアウト情報取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameSpec.GetEquipSpecLayout, out sql);
            sql = sql + " AND(msr.job_structure_id IN(" + job_structure_id_all + ") ";
            sql = sql + "     OR msr.job_structure_id IS NULL) ";
            sql = sql + " ORDER BY msr.job_structure_id,msr.display_order ";
            // SQL実行
            IList<Dao.EquipmentSpecLayout> results = db.GetListByDataClass<Dao.EquipmentSpecLayout>(sql, new { FactoryId = factoryId, LanguageId = this.LanguageId });
            for (int i = 0; i < results.Count; i++)
            {
                // 仕様項目が数値なら単位取得
                if (results[i].ExtensionData == "2" || results[i].ExtensionData == "3")
                {
                    TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameSpec.GetEquipSpecLayoutUnit, out sql);
                    list = db.GetList(sql, new { SpecUnitId = results[i].SpecUnitId, LanguageId = this.LanguageId, FactoryId = factoryId });
                    if (list.Count > 0)
                    {
                        results[i].UnitTranslationText = list[0].translation_text;
                    }
                }
            }
            //機器仕様取得
            // 一覧検索SQL文の取得
            sql = null;
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameSpec.GetEquipSpec, out sql);
            dynamic whereParam = null; // WHERE句パラメータ
            whereParam = new { EquipmentId = equipid, LanguageId = this.LanguageId };

            // 一覧検索実行
            IList<Dao.EquipmentSpecLayout> resultsData = db.GetListByDataClass<Dao.EquipmentSpecLayout>(sql, whereParam);
            // 実際の登録値データが空白のままだとレイアウトされないため、空白の登録値データを作成
            //if (resultsData == null || resultsData.Count == 0)
            //{
            //    // 正常終了
            //    return true;
            //}

            if (resultsData.Count == results.Count)
            {
                foreach (Dao.EquipmentSpecLayout data in resultsData)
                {
                    foreach (Dao.EquipmentSpecLayout layoutData in results)
                    {
                        if (data.SpecId == layoutData.SpecId)
                        {
                            layoutData.SpecValue = data.SpecValue;
                            layoutData.SpecStructureId = data.SpecStructureId;
                            layoutData.SpecNum = data.SpecNum;
                            layoutData.SpecNumMin = data.SpecNumMin;
                            layoutData.SpecNumMax = data.SpecNumMax;
                            layoutData.EquipmentSpecId = data.EquipmentSpecId;
                            layoutData.UpdateSerialid = data.UpdateSerialid;
                        }
                    }
                }
            }
            else
            {
                bool extFlg = false;
                foreach (Dao.EquipmentSpecLayout layoutData in results)
                {
                    extFlg = false;
                    foreach (Dao.EquipmentSpecLayout data in resultsData)
                    {
                        if (data.SpecId == layoutData.SpecId)
                        {
                            extFlg = true;
                            layoutData.SpecValue = data.SpecValue;
                            layoutData.SpecStructureId = data.SpecStructureId;
                            layoutData.SpecNum = data.SpecNum;
                            layoutData.SpecNumMin = data.SpecNumMin;
                            layoutData.SpecNumMax = data.SpecNumMax;
                            layoutData.EquipmentSpecId = data.EquipmentSpecId;
                            layoutData.UpdateSerialid = data.UpdateSerialid;
                        }
                    }
                    if (!extFlg)
                    {
                        layoutData.SpecValue = null;
                        layoutData.SpecStructureId = null;
                        layoutData.SpecNum = null;
                        layoutData.SpecNumMin = null;
                        layoutData.SpecNumMax = null;
                        layoutData.EquipmentSpecId = null;
                        layoutData.UpdateSerialid = 0;
                    }
                }
            }

            // ページ情報取得
            var pageInfo = GetPageInfo(this.FormNo == FormType.Detail ? TargetCtrlIdSpec.SpecDetail75 : TargetCtrlIdSpec.SpecEdit20, this.pageInfoList);
            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.EquipmentSpecLayout>(pageInfo, results, results.Count))
            {
                // 異常終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 桁数の大きい値を保持できるよう変換
            string ctrlId = this.FormNo == FormType.Detail ? TargetCtrlIdSpec.SpecDetail75 : TargetCtrlIdSpec.SpecEdit20;
            var info = getResultMappingInfo(ctrlId);
            List<string> numColumns = new List<string>();
            numColumns.Add(info.getValName("numColumn1"));
            numColumns.Add(info.getValName("numColumn2"));
            numColumns.Add(info.getValName("numColumn3"));
            ConvertResultBigValueAvaible(ctrlId, numColumns);

            //// 2023.09 選択項目表示不具合対応 strat
            //// 隠し一覧に対して工場IDをセットしコンボ用SQL(C0032)にパラメータとして渡す
            // ページ情報取得
            pageInfo = GetPageInfo(this.FormNo == FormType.Detail ? TargetCtrlIdSpec.SpecDetail70 : TargetCtrlIdSpec.SpecEdit00, this.pageInfoList);
            Dao.searchResult r = new Dao.searchResult();
            r.FactoryId = (int)factoryId;
            IList<Dao.searchResult> rs = new List<Dao.searchResult>();
            rs.Add(r);
            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, rs, rs.Count))
            {
                // 異常終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            //// 2023.09 選択項目表示不具合対応 end

            return true;
        }

        ///// <summary>
        ///// 選択機器ID取得
        ///// </summary>
        ///// <param name="editFlg">編集画面:True,参照画面:False</param>
        ///// <returns>機器ID</returns>
        //private long getEquipmentId(bool reg = false)
        //{
        //    Dictionary<string, object> targetDictionary = null;
        //    if (reg)
        //    {
        //        // 参照画面からの遷移の場合:参照画面で表示していた情報より機番IDを取得(js側にてsearchConditionDictionaryへセット済み)
        //        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.SearchList);
        //        if (targetDictionary == null)
        //        {
        //            targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.Detail20);
        //        }
        //    }
        //    else
        //    {

        //        if (this.CtrlId == "Regist")
        //        {
        //            targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.EditDetail10);
        //        }
        //    }
        //    return long.Parse(getDictionaryKeyValue(targetDictionary, "equipment_id"));
        //}

        /// <summary>
        /// 機種別仕様登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistSpecEdit()
        {
            DateTime now = DateTime.Now;
            string ctrlId = TargetCtrlIdSpec.SpecEdit20;
            // 機器ID取得
            long equipmentId = getEquipmentId(true);

            List<Dao.EquipmentSpecLayout> list = new List<Dao.EquipmentSpecLayout>();
            //機器別仕様隠し一覧の情報取得
            var mappingInfo = getResultMappingInfo(ctrlId);
            List<Dictionary<string, object>> dicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);
            foreach (Dictionary<string, object> dic in dicList)
            {
                //データクラスに変換
                Dao.EquipmentSpecLayout spec = new Dao.EquipmentSpecLayout();
                if (!SetExecuteConditionByDataClass<Dao.EquipmentSpecLayout>(dic, ctrlId, spec, now, this.UserId, this.UserId))
                {
                    return false;
                }

                if (spec.EquipmentSpecId != null)
                {
                    //排他チェック
                    if (!checkExclusiveSingle(ctrlId, dic))
                    {
                        return false;
                    }
                }
                // 機器ID
                spec.EquipmentId = equipmentId;
                list.Add(spec);
            }

            //登録・更新
            foreach(Dao.EquipmentSpecLayout data in list)
            {
                string sqlFile = data.EquipmentSpecId == null ? SqlNameSpec.InsertEquipmentSpec : SqlNameSpec.UpdateEquipmentSpec;
                if (!registUpdateDb<Dao.EquipmentSpecLayout>(data, sqlFile))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
