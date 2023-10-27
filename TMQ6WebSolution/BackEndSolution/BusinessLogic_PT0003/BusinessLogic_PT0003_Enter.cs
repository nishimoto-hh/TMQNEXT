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
    /// 予備品管理　棚卸(入庫一覧)
    /// </summary>
    public partial class BusinessLogic_PT0003 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <returns>エラーの場合False</returns>
        private bool searchEnterList(Dao.searchCondition condition, List<string> listUnComment)
        {
            // 検索SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetEnterList, out string baseSql, listUnComment);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetEnterList, out string withSql, listUnComment);

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.EnterList.List, this.pageInfoList);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied, true))
            {
                return false;
            }

            // 総件数取得SQL文の取得
            string execSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);

            // 総件数を取得
            int cnt = db.GetCount(execSql, condition);
            // 総件数のチェック
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

            //棚IDより翻訳をまとめて取得しておく
            List<long> partsLocationIdList = results.Select(x => x.PartsLocationId).Distinct().ToList();
            List<STDData.VStructureItemEntity> partsLocationList = TMQUtil.GetpartsLocationList(partsLocationIdList, this.LanguageId, db);

            //工場ID棚番結合文字列を保持するDictionary
            Dictionary<int, string> factoryJoinDic = new();

            //棚番、単位結合
            foreach (var result in results)
            {
                // コードと名称結合
                result.PartsLocationDisp = TMQUtil.GetDisplayPartsLocation(result.PartsLocationId, result.PartsLocationDetailNo, result.FactoryId, this.LanguageId, this.db, ref factoryJoinDic, partsLocationList); // 棚番
                result.DepartmentNm = TMQUtil.CombineNumberAndUnit(result.DepartmentCd, result.DepartmentNm, true);                                                                                              // 部門

                result.InoutQuantityDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.InoutQuantity.ToString(), result.UnitDigit, result.UnitRoundDivision), result.UnitName, false);             // 入庫数
                result.UnitPriceDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.UnitPrice.ToString(), result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName, false);         // 入庫単価
                result.InventoryAmountDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.AmountMoney.ToString(), result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName, false); // 入庫金額
            }

            // 受払履歴情報のデータを前に棚ID、棚枝番、予備品NO、新旧区分、部門ID、勘定科目単位にソート
            var sortList = results.OrderBy(x => x.ControlFlag).ThenBy(x => x.PartsLocationId).ThenBy(x => x.PartsLocationDetailNo).ThenBy(x => x.PartsNo)
                                            .ThenBy(x => x.OldNewStructureId).ThenBy(x => x.DepartmentStructureId).ThenBy(x => x.AccountStructureId).ToList();

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.inoutList>(pageInfo, sortList, cnt, false))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }
    }
}
