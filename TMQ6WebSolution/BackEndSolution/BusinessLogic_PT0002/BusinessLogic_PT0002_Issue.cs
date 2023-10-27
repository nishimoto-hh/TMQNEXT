using System;
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
using Dao = BusinessLogic_PT0002.BusinessLogicDataClass_PT0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

namespace BusinessLogic_PT0002
{
    /// <summary>
    /// 出庫タブ
    /// </summary>
    public partial class BusinessLogic_PT0002 : CommonBusinessLogicBase
    {

        /// <summary>
        /// 出庫一覧　親要素検索
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <param name="workNoList">出庫Noリスト</param>
        /// <returns>エラーであればfalseを返す</returns>
        private bool searchListIssue(Dao.searchCondition condition, List<string> listUnComment, out List<int> workNoList)
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.IssueResults, this.pageInfoList);

            // 初期化
            workNoList = new();

            //検索SQL、WITH句,総件数取得SQL文の取得
            getSql(SqlName.GetIssueList, listUnComment, out string baseSql, out string withSql, out string execSql, pageInfo);

            // 総件数を取得し総件数をチェック
            int cnt = db.GetCount(execSql, condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                SetSearchResultsByDataClass<Dao.searchResultIssueParents>(pageInfo, null, cnt, false);
                return false;
            }

            // 検索結果格納リスト
            IList<Dao.searchResultIssueParents> results = new List<Dao.searchResultIssueParents>();

            // データが1件以上あれば処理を行う
            if (cnt > 0)
            {
                // 一覧検索SQLを取得し、ORDER BY句を追加
                string orderBy = " ORDER BY tbl.inout_datetime DESC ";
                getListSearchSql(execSql, baseSql, withSql, orderBy, pageInfo, out StringBuilder selectSql);

                // 一覧検索実行
                results = db.GetListByDataClass<Dao.searchResultIssueParents>(selectSql.ToString(), condition);
                if (results == null || results.Count == 0)
                {
                    SetSearchResultsByDataClass<Dao.searchResultIssueParents>(pageInfo, null, 0, false);
                    return false;
                }

                foreach (var result in results)
                {
                    // 子画面の条件となる出庫Noを取得
                    workNoList.Add(result.WorkNo);

                    // 金額と単位結合
                    result.IssueQuantity = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.InoutQuantity, result.UnitDigit, result.UnitRoundDivision), result.IssueQuantity);
                    result.IssueMonney = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.IssueMonney, result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName);
                }
            }
            // 検索結果の設定
            if (SetSearchResultsByDataClassForList<Dao.searchResultIssueParents>(pageInfo, results, cnt, true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 出庫一覧　子要素検索
        /// </summary>
        /// <param name="workNoList">親画面の出庫Noリスト</param>
        /// <returns>エラーであればfalseを返す</returns>
        private bool searchListIssueChild(List<int> workNoList)
        {
            // ページ情報取得(条件エリア)
            Dao.searchCondition conditionObj = new Dao.searchCondition();
            conditionObj.LanguageId = this.LanguageId; //言語ID設定
            conditionObj.WorkNoList = workNoList;      //出庫No設定

            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.IssueResultsChild, this.pageInfoList);

            // 一覧検索実行
            var results = TMQUtil.SqlExecuteClass.SelectList<Dao.searchResultIssueParents>(SqlName.GetIssueListChild, SqlName.SubDir, conditionObj, this.db);
            if (results == null || results.Count == 0)
            {
                SetSearchResultsByDataClass<Dao.searchResultIssueParents>(pageInfo, null, 0, false);
                return false;
            }

            // 工場IDと結合文字列のディクショナリ、同じ工場で重複取得しないようにする
            Dictionary<int, string> factoryJoinDic = new();
            string strJoin = string.Empty;
            foreach (var result in results)
            {
                // 金額と単位結合
                result.UnitPrice = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.UnitPrice, result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName);
                result.IssueQuantity = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.InoutQuantity, result.UnitDigit, result.UnitRoundDivision), result.UnitName);
                result.IssueMonney = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.AmountMoney, result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName);

                // 結合文字取得
                strJoin = TMQUtil.GetJoinStrOfPartsLocationNoDuplicate(result.PartsFactoryId, this.LanguageId, this.db, ref factoryJoinDic);

                // コードと名称結合
                result.PartsLocationName = TMQUtil.GetDisplayPartsLocation(result.PartsLocationName, result.PartsLocationDetailNo, strJoin);
                result.DepartmentNm = TMQUtil.CombineNumberAndUnit(result.DepartmentCd, result.DepartmentNm, true);
                result.SubjectNm = TMQUtil.CombineNumberAndUnit(result.SubjectCd, result.SubjectNm, true);
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClassForList<Dao.searchResultIssueParents>(pageInfo, results, results.Count(), true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }
    }
}
