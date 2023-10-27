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
using Dao = BusinessLogic_PT0001.BusinessLogicDataClass_PT0001;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConst = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_PT0001
{
    /// <summary>
    /// 一覧画面
    /// </summary>
    public partial class BusinessLogic_PT0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.List.GetPartsList, out string baseSql);
            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.List.GetPartsList, out string withSql);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied, true))
            {
                return false;
            }

            //SQLパラメータに条件設定
            whereParam.LanguageId = this.LanguageId;　// 言語ID

            // SQL、WHERE句、WITH句より件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);

            // 総件数を取得
            int cnt = db.GetCount(executeSql.ToString(), whereParam);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, null, cnt, isDetailConditionApplied);
                return false;
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY");    // 並び順を指定
            selectSql.AppendLine("parts_no");    // 予備品No. 昇順
            selectSql.AppendLine(",parts_name"); // 予備品名  昇順

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 丸め処理・数量と単位を結合
            results.ToList().ForEach(x => x.JoinStrAndRound());

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, new List<StructureType> { StructureType.Job, StructureType.SpareLocation }, this.db, this.LanguageId); // 職種、予備品場所階層

            // 棚番と枝番を結合
            JoinLocationAndDetailNo(results);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results, results.Count, isDetailConditionApplied))
            {
                return false;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;

            /// <summary>
            /// 棚番と枝番を結合する
            /// </summary>
            /// <param name="results">検索結果</param>
            void JoinLocationAndDetailNo(IList<Dao.searchResult> results)
            {
                // 棚番と枝番がある場合は結合
                string joinStr = string.Empty;
                // 工場IDと結合文字列のディクショナリ、同じ工場で重複取得しないようにする
                Dictionary<int, string> factoryJoinDic = new();
                foreach (Dao.searchResult result in results)
                {
                    if (!string.IsNullOrEmpty(result.RackName) && !string.IsNullOrEmpty(result.PartsLocationDetailNo))
                    {
                        // 結合文字列を取得
                        joinStr = TMQUtil.GetJoinStrOfPartsLocationNoDuplicate((int)result.DefaultFactoryId, this.LanguageId, db, ref factoryJoinDic);
                        // 棚番 + 枝番
                        result.RackName = TMQUtil.GetDisplayPartsLocation(result.RackName, result.PartsLocationDetailNo, joinStr);
                    }
                }
            }
        }
    }
}
