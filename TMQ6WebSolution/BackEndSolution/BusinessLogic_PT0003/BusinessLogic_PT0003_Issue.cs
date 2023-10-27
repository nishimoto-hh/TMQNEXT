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
using STDData = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using Dao = BusinessLogic_PT0003.BusinessLogicDataClass_PT0003;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_PT0003
{
    /// <summary>
    /// 予備品管理　棚卸(出庫一覧)
    /// </summary>
    public partial class BusinessLogic_PT0003 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 出庫一覧検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <returns>エラーの場合False</returns>
        private bool searchIssueList(Dao.searchCondition condition, List<string> listUnComment)
        {
            // 検索SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetIssueList, out string baseSql, listUnComment);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetIssueList, out string withSql, listUnComment);

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.IssueList.List, this.pageInfoList);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied, true, isJobKindOnly: true, isJobNullAble: true))
            {
                return false;
            }

            // 総件数取得SQL文の取得
            string execSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);

            // 総件数を取得
            int cnt = db.GetCount(execSql, condition);
            // 総件数のチェック
            if (cnt == 0)
            {
                //0件の場合は問題なし（対象年月の棚卸が未実施の場合）
                return true;
            }
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            // 一覧検索SQL文の取得
            execSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);

            // 一覧検索実行
            var results = db.GetListByDataClass<Dao.inoutList>(execSql, condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            foreach (var result in results)
            {
                // 金額・単位結合
                result.IssueQuantityDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.IssueQuantity.ToString(), result.UnitDigit, result.UnitRoundDivision), result.UnitName, false);             // 出庫数
                result.IssueAmountDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.IssueAmount.ToString(), result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName, false);     // 出庫金額
            }

            // 受払履歴情報のデータを前に予備品NO、新旧区分、部門ID、勘定科目単位にソート
            var sortList = results.OrderBy(x => x.ControlFlag).ThenBy(x => x.PartsNo).ThenBy(x => x.OldNewStructureId)
                                  .ThenBy(x => x.DepartmentStructureId).ThenBy(x => x.AccountStructureId).ToList();

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.inoutList>(pageInfo, sortList, cnt, false))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// 出庫一覧(子要素)検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <returns>エラーの場合False</returns>
        private bool searchIssueChildList(Dao.searchCondition condition, List<string> listUnComment)
        {
            // 検索SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetIssueChildList, out string baseSql, listUnComment);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetIssueChildList, out string withSql, listUnComment);

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.IssueList.ListChild, this.pageInfoList);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied, true, isJobKindOnly: true, isJobNullAble: true))
            {
                return false;
            }

            // 一覧検索SQL文の取得
            string execSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);
            // 一覧検索実行
            var results = db.GetListByDataClass<Dao.inoutList>(execSql.ToString(), condition);
            if (results == null)
            {
                return false;
            }
            if (results.Count == 0)
            {
                //0件の場合は問題なし（対象年月の棚卸が未実施の場合）
                return true;
            }

            //棚IDより翻訳をまとめて取得しておく
            List<long> partsLocationIdList = results.Select(x => x.PartsLocationId).Distinct().ToList();
            List<STDData.VStructureItemEntity> partsLocationList = TMQUtil.GetpartsLocationList(partsLocationIdList, this.LanguageId, db);

            //工場ID棚番結合文字列を保持するDictionary
            Dictionary<int, string> factoryJoinDic = new();

            foreach (var result in results)
            {
                // 棚＋棚番
                result.PartsLocationDisp = TMQUtil.GetDisplayPartsLocation(result.PartsLocationId, result.PartsLocationDetailNo, result.FactoryId, this.LanguageId, this.db, ref factoryJoinDic, partsLocationList);

                // 金額・単位結合
                result.UnitPriceDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.UnitPrice.ToString(), result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName, false);          // 入庫単価
                result.InoutQuantityDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.InoutQuantity.ToString(), result.UnitDigit, result.UnitRoundDivision), result.UnitName, false);              // 出庫数
                result.IssueAmountDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.IssueAmount.ToString(), result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName, false);      // 出庫金額

                // コード＋名称
                result.DepartmentNm = TMQUtil.CombineNumberAndUnit(result.DepartmentCd, result.DepartmentNm, true);
                result.SubjectNm = TMQUtil.CombineNumberAndUnit(result.SubjectCd, result.SubjectNm, true);
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.inoutList>(pageInfo, results, results.Count(), false))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }
    }
}
