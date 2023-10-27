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
        private bool searchListShed(Dao.searchCondition condition, List<string> listUnComment)
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.ShedResults, this.pageInfoList);

            //検索SQL、WITH句,総件数取得SQL文の取得
            getSql(SqlName.GetShedList, listUnComment, out string baseSql, out string withSql, out string execSql, pageInfo);

            // 総件数を取得し総件数をチェック
            checkTotalCount(execSql, condition, pageInfo, out int cnt);

            // 検索結果格納リスト
            IList<Dao.searchResultShed> results = new List<Dao.searchResultShed>();

            // データが1件以上あれば処理を行う
            if (cnt > 0)
            {
                // 一覧検索SQLを取得し、ORDER BY句を追加
                string orderBy = " ORDER BY tbl.receiving_datetime DESC ";
                getListSearchSql(execSql, baseSql, withSql, orderBy, out StringBuilder selectSql);

                // 一覧検索実行
                results = db.GetListByDataClass<Dao.searchResultShed>(selectSql.ToString(), condition);
                if (results == null || results.Count == 0)
                {
                    return false;
                }

                // 移庫元棚IDリスト
                List<int> yuanLocationIdList = new List<int>();
                // 移庫先倉庫IDリスト
                List<int> destinationLocationIdList = new List<int>();

                foreach (var result in results)
                {
                    // 移庫元棚ID追加
                    yuanLocationIdList.Add(result.StorageLocationId);
                    // 移庫元棚ID追加
                    destinationLocationIdList.Add(result.ToStorageLocationId);

                    //棚IDと棚枝番結合
                    result.LocationId = CommonTMQUtil.CommonTMQUtil.GetDisplayPartsLocation(result.LocationId, result.PartsLocationDetailNo, result.PartsFactoryId, this.LanguageId, db);
                    result.ToLocationId = CommonTMQUtil.CommonTMQUtil.GetDisplayPartsLocation(result.ToLocationId, result.ToPartsLocationDetailNo, result.PartsFactoryId, this.LanguageId, db);

                    // 数と単位結合
                    result.TransferCount = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.TransferCount, result.UnitDigit, result.UnitRoundDivision), result.UnitName);

                    // 金額と単位結合
                    result.UnitPrice = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.UnitPrice, result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName);
                    result.TransferAmount = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.TransferAmount, result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName);

                    // コードと名称結合
                    result.DepartmentNm = TMQUtil.CombineNumberAndUnit(result.DepartmentCd, result.DepartmentNm, true);
                    result.SubjectNm = TMQUtil.CombineNumberAndUnit(result.SubjectCd, result.SubjectNm, true);
                }

                // 移庫元棚IDリスト
                List<string> yuanLocationList = new List<string>();
                // 移庫元棚IDをループして倉庫を取得する
                foreach (var id in yuanLocationIdList)
                {
                    condition.PartsLocationId = id;
                    var location = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultTransfer>(SqlName.GetWarehouse, SqlName.SubDir, condition, this.db);
                    if (location != null)
                    {
                        yuanLocationList.Add(location.TranslationText);
                    }
                }

                // 移庫先倉庫IDリスト
                List<string> destinationLocationList = new List<string>();
                // 移庫先棚IDをループして倉庫を取得する
                foreach (var id in destinationLocationIdList)
                {
                    condition.PartsLocationId = id;
                    var location = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultTransfer>(SqlName.GetWarehouse, SqlName.SubDir, condition, this.db);
                    if (location != null)
                    {
                        destinationLocationList.Add(location.TranslationText);
                    }
                }

                int i = 0;
                // 取得した移庫元・移庫先予備品倉庫をセット
                foreach (var result in results)
                {
                    result.StorageLocationName = yuanLocationList[i];
                    result.ToStorageLocationName = destinationLocationList[i];
                    i++;
                }
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResultShed>(pageInfo, results, results.Count(), true))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }
    }
}
