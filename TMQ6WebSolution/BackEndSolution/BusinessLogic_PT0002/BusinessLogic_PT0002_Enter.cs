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
    /// 入庫タブ
    /// </summary>
    public partial class BusinessLogic_PT0002 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <returns>エラーの場合False</returns>
        private bool searchListEnter(Dao.searchCondition condition, List<string> listUnComment)
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.EnterResults, this.pageInfoList);

            //検索SQL、WITH句,総件数取得SQL文の取得
            getSql(SqlName.GetEnterList, listUnComment, out string baseSql, out string withSql, out string execSql, pageInfo);

            // 総件数を取得し総件数をチェック
            int cnt = db.GetCount(execSql, condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                SetSearchResultsByDataClass<Dao.searchResultEnter>(pageInfo, null, cnt, false);
                return false;
            }

            // 検索結果格納リスト
            IList<Dao.searchResultEnter> results = new List<Dao.searchResultEnter>();

            // データが1件以上あれば処理を行う
            if (cnt > 0)
            {
                // 一覧検索SQLを取得し、ORDER BY句を追加
                string orderBy = " ORDER BY tbl.receiving_datetime DESC ";
                getListSearchSql(execSql, baseSql, withSql, orderBy, pageInfo, out StringBuilder selectSql);

                // 一覧検索実行
                results = db.GetListByDataClass<Dao.searchResultEnter>(selectSql.ToString(), condition);
                if (results == null || results.Count == 0)
                {
                    SetSearchResultsByDataClass<Dao.searchResultEnter>(pageInfo, null, 0, false);
                    return false;
                }

                // 工場IDと結合文字列のディクショナリ、同じ工場で重複取得しないようにする
                Dictionary<int, string> factoryJoinDic = new();
                string strJoin = string.Empty;
                foreach (var result in results)
                {
                    // 金額と単位結合
                    result.InoutQuantity = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.InoutQuantity, result.UnitDigit, result.UnitRoundDivision), result.UnitName);
                    result.UnitPrice = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.UnitPrice, result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName);
                    result.AmountMoney = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.AmountMoney, result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName);

                    // 結合文字取得
                    strJoin = TMQUtil.GetJoinStrOfPartsLocationNoDuplicate(result.PartsFactoryId, this.LanguageId, this.db, ref factoryJoinDic);

                    // コードと名称結合
                    result.PartsLocationName = TMQUtil.GetDisplayPartsLocation(result.PartsLocationName, result.PartsLocationDetailNo, strJoin);
                    result.DepartmentNm = TMQUtil.CombineNumberAndUnit(result.DepartmentCd, result.DepartmentNm, true);
                    result.SubjectNm = TMQUtil.CombineNumberAndUnit(result.SubjectCd, result.SubjectNm, true);
                }
            }
            // 検索結果の設定
            if (SetSearchResultsByDataClassForList<Dao.searchResultEnter>(pageInfo, results, cnt, true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }
    }
}
