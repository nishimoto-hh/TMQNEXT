using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Dao = BusinessLogic_RM0002.BusinessLogicDataClass_RM0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_RM0002
{
    /// <summary>
    /// 一覧画面
    /// </summary>
    public partial class BusinessLogic_RM0002 : CommonBusinessLogicBase
    {

        #region privateメソッド

        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.List, this.pageInfoList);

            List<string> listUnComment = new List<string>();

            dynamic whereParam = new ExpandoObject();

            // 画面の条件欄に入力された内容

            // 検索条件設定
            dynamic conditionObj = new ExpandoObject();
            // 画面の検索条件と、画面項目定義拡張テーブルの検索条件の項目の値より、検索条件を設定
            SetSearchCondition(this.searchConditionDictionary, ConductInfo.FormList.Condition, conditionObj, pageInfo);

            // 検索SQLの取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetList, out string baseSql, listUnComment);
            string whereClause = string.Empty;
            bool isDetailConditionApplied = false;

            // 一覧検索SQL文の取得
            string execSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereClause, string.Empty);
            // 検索SQLにORDER BYを追加

            // 総件数を取得
            int cnt = db.GetCount(execSql, conditionObj);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            execSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereClause, string.Empty);
            // 一覧検索SQL文の取得
            var selectSql = new StringBuilder(execSql);

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), conditionObj);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 固有の処理があれば実行
            // 同一の帳票IDが存在する場合、先頭のレコードを優先する
            IList<Dao.searchResult> results2 = new List<Dao.searchResult>();
            List<string> reportIdList = new List<string>();
            for(int i = 0;  i < results.Count; i++)
            {
                if(reportIdList.IndexOf(results[i].ReportId) < 0)
                {
                    reportIdList.Add(results[i].ReportId);
                    results2.Add(results[i]);
                }
            }
            // 検索結果の設定
            //if (SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results, results.Count, isDetailConditionApplied))
            if (SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results2, results2.Count, isDetailConditionApplied))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        #endregion privateメソッド
    }
}
