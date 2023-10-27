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
    /// 棚番移庫タブ
    /// </summary>
    public partial class BusinessLogic_PT0002 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <returns>エラーの場合False</returns>
        private bool searchListCategory(Dao.searchCondition condition, List<string> listUnComment)
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.CategoryResults, this.pageInfoList);

            //検索SQL、WITH句,総件数取得SQL文の取得
            getSql(SqlName.GetCategoryList, listUnComment, out string baseSql, out string withSql, out string execSql, pageInfo);

            // 総件数を取得し総件数をチェック
            int cnt = db.GetCount(execSql, condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                SetSearchResultsByDataClass<Dao.searchResultCategory>(pageInfo, null, cnt, false);
                return false;
            }

            // 検索結果格納リスト
            IList<Dao.searchResultCategory> results = new List<Dao.searchResultCategory>();

            // データが1件以上あれば処理を行う
            if (cnt > 0)
            {
                // 一覧検索SQLを取得し、ORDER BY句を追加
                string orderBy = " ORDER BY tbl.receiving_datetime DESC ";
                getListSearchSql(execSql, baseSql, withSql, orderBy, pageInfo, out StringBuilder selectSql);

                // 一覧検索実行
                results = db.GetListByDataClass<Dao.searchResultCategory>(selectSql.ToString(), condition);
                if (results == null || results.Count == 0)
                {
                    SetSearchResultsByDataClass<Dao.searchResultCategory>(pageInfo, null, 0, false);
                    return false;
                }

                foreach (var result in results)
                {
                    // 数と単位結合
                    result.TransferCount = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.TransferCount, result.UnitDigit, result.UnitRoundDivision), result.UnitName);
                    // 金額と単位結合
                    result.UnitPrice = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.UnitPrice, result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName);
                    result.TransferAmount = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.TransferAmount, result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName);

                    // コードと名称結合
                    result.DepartmentNm = TMQUtil.CombineNumberAndUnit(result.DepartmentCd, result.DepartmentNm, true);
                    result.SubjectNm = TMQUtil.CombineNumberAndUnit(result.SubjectCd, result.SubjectNm, true);
                    result.ToDepartmentNm = TMQUtil.CombineNumberAndUnit(result.ToDepartmentCd, result.ToDepartmentNm, true);
                    result.ToSubjectNm = TMQUtil.CombineNumberAndUnit(result.ToSubjectCd, result.ToSubjectNm, true);
                }
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClassForList<Dao.searchResultCategory>(pageInfo, results, cnt, true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }
    }
}
