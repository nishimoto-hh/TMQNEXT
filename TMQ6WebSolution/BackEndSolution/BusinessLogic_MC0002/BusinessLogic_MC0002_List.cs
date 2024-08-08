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
using Dao = BusinessLogic_MC0002.BusinessLogicDataClass_MC0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_MC0002
{
    /// <summary>
    /// 機器別管理基準標準 一覧画面
    /// </summary>
    public partial class BusinessLogic_MC0002 : CommonBusinessLogicBase
    {
        #region 検索処理
        /// <summary>
        /// 一覧画面 検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // 一覧のページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            // 項目カスタマイズで選択されている項目のみSELECTする
            List<string> uncommentList = getDisplayCustomizeCol(ConductInfo.FormList.ControlId.List);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.Common.SubDirCommon, SqlName.Common.GetManagementstandardsList, out string sql, uncommentList);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, sql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied, isJobNullAble: true))
            {
                return false;
            }

            // 場所階層を検索するための一時テーブル(#temp_location)に地区のIDを登録する（工場が属する地区も検索対象とする）
            // 標準は工場がNULLの可能性もあるため
            if (!TMQUtil.SqlExecuteClass.Regist(SqlName.List.InsertDistrictIdForTempLoc, SqlName.List.SubDirList, null, this.db))
            {
                return false;
            }

            // 検索条件追加
            whereParam.LanguageId = this.LanguageId; // 言語ID

            // 翻訳の一時テーブルを作成
            createTranslationTempTbl(true, uncommentList);

            //項目カスタマイズでSELECT項目を絞るので、詳細検索条件は機能側で付与する
            StringBuilder baseSqlSb = new StringBuilder(sql);
            baseSqlSb.AppendLine(whereSql);

            // SQL、WHERE句、WITH句より件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSqlSb.ToString(), null);

            // 総件数を取得
            // 件数を取得するSQLを取得
            int cnt = db.GetCount(executeSql, whereParam);

            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                SetSearchResultsByDataClass<Dao.searchResultList>(pageInfo, null, cnt, isDetailConditionApplied);
                return false;
            }
            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSqlSb.ToString(), null, string.Empty, isDetailConditionApplied, pageInfo.SelectMaxCnt);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY ");    // 並び順を指定
            selectSql.AppendLine("management_standards_name ASC"); // 標準名称 昇順
            selectSql.AppendLine(",management_standards_id ASC");  // 機器別管理基準標準ID  昇順

            // 一覧検索実行
            IList<Dao.searchResultList> results = db.GetListByDataClass<Dao.searchResultList>(selectSql.ToString(), whereParam);

            // 検索結果の設定
            if (!SetSearchResultsByDataClassForList<Dao.searchResultList>(pageInfo, results, cnt, isDetailConditionApplied))
            {
                return false;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;

            //グローバルリストへ総件数を設定
            SetGlobalData(GlobalKey.MC0002AllListCount, isDetailConditionApplied ? results.Count : cnt);

            // 検索結果リストを解放
            results = null;
            GC.Collect();
            return true;
        }
        #endregion
    }
}
