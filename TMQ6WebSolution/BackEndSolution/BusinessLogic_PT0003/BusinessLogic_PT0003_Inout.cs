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
    /// 予備品管理　棚卸(受払履歴)
    /// </summary>
    public partial class BusinessLogic_PT0003 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchInoutList()
        {
            // ディクショナリを取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormList.List.InventoryList);

            // 予備品情報格納変数
            Dao.inoutHistryList info = new Dao.inoutHistryList();

            // targetDicより表示項目を取得
            SetDataClassFromDictionary(targetDic, ConductInfo.FormList.List.InventoryList, info, new List<string> { });

            // 年度取得
            var dateDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormList.Condition.SearchCondition);
            info.Year = getDictionaryKeyValue(dateDic, "target_year_month").Substring(0, 4); // 年のみ取得
            string month = getDictionaryKeyValue(dateDic, "target_year_month").Substring(5, 2);  //月のみ取得

            // 年、月より最終日取得
            int endDate = DateTime.DaysInMonth(year: int.Parse(info.Year), month: int.Parse(month));
            // 対象年月の月末の最終日を取得
            info.MonthYear = new DateTime(int.Parse(info.Year), month: int.Parse(month), endDate, 23, 59, 59);

            // 棚卸データより準備日時、確定日時を取得
            int status = 0;
            var result = TMQUtil.SqlExecuteClass.SelectEntity<Dao.inoutHistryList>(SqlName.SelectInventryCount, SqlName.SubDir, info, this.db);

            // 準備リスト未作成
            if (result == null)
            {
                status = PartsStatus.NotCreated;
            }
            // 作成済み
            else if (result.FixedDatetime == null)
            {
                status = PartsStatus.Created;
            }
            // 棚卸確定
            else
            {
                status = PartsStatus.Confirm;
            }

            // 言語ID設定
            info.LanguageId = this.LanguageId;

             // 予備品情報一覧検索
            if (!InventryInfoList(status, info))
            {
                return false; // 検索失敗
            }

            // 入出庫履歴取得
            if (!InoutHistryList(status, info))
            {
                return false; // 検索失敗
            }
            return true;
        }

        /// <summary>
        /// 予備品情報検索
        /// </summary>
        /// <param name="status">ステータス</param>
        /// <param name="info">遷移元の情報</param>
        /// <returns>正常時:True</returns>
        private bool InventryInfoList(int status, Dao.inoutHistryList info)
        {
            // 検索結果表示リスト
            List<Dao.inoutHistryList> result = new();

            // 取得した値を追加
            result.Add(info);

            // ページ情報取得
            var hierarchyInfo = GetPageInfo(ConductInfo.FormDetail.Inventry.InventoryInfoList, this.pageInfoList);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.inoutHistryList>(hierarchyInfo, result, result.Count(), true))
            {
                // 正常終了
                return true;
            }
            return false;
        }

        /// <summary>
        /// 入出庫履歴取得
        /// </summary>
        /// <param name="status">ステータス</param>
        /// <param name="info">遷移元の情報</param>
        /// <returns>正常時:True</returns>
        private bool InoutHistryList(int status, Dao.inoutHistryList info)
        {
            // ステータス
            info.Status = status;

            // 入出庫履歴一覧(繰越行)取得
            var forwardList = TMQUtil.SqlExecuteClass.SelectEntity<Dao.inoutHistryList>(SqlName.GetInoutHistryForwardList, SqlName.SubDir, info, this.db);
            if (forwardList != null)
            {
                // 繰越がある場合、先頭行は棚卸固定
                forwardList.workDivisionStructureId = getInitialValue(condPartsType.shippingDivition.StructureGroupId, condPartsType.shippingDivition.Seq, condPartsType.shippingDivition.ExData);
                forwardList.workDivisionName = getInitialValueName(condPartsType.shippingDivition.StructureGroupId, condPartsType.shippingDivition.Seq, condPartsType.shippingDivition.ExData);

                // 値と単位結合
                forwardList = joinBlankForwardList(forwardList, info);

                // 在庫確定データの対象年月の末日
                info.CleateDate = forwardList.TargetMonth.AddMonths(1).AddDays(-1);
            }
            else
            {
                // 在庫確定データの対象年月がない場合は固定で日付を追加
                info.CleateDate = new DateTime(1900, 1, 1);
            }

            // 入出庫履歴一覧検索SQLの取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetInoutHistryList, out string baseSql))
            {
                return false;
            }
            // WITH句の取得
            if (!TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetInoutHistryList, out string withSql))
            {
                return false;
            }

            // 総件数取得SQL文の取得
            string execSql = TMQUtil.GetSqlStatementSearch(true, baseSql, string.Empty, withSql);

            // ページ情報取得
            var inoutInfo = GetPageInfo(ConductInfo.FormDetail.Inout.InoutHistryList, this.pageInfoList);

            // 総件数を取得
            int cnt = db.GetCount(execSql, info);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, inoutInfo))
            {
                SetSearchResultsByDataClass<Dao.inoutHistryList>(inoutInfo, null, cnt, false);
                return false;
            }

            // 一覧検索SQL文の取得
            execSql = TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql);
            // 検索SQLにORDER BYを追加
            var selectSql = new StringBuilder(execSql);
            selectSql.AppendLine(" ORDER BY tbl.date ");

            // 一覧検索実行
            var inoutList = db.GetListByDataClass<Dao.inoutHistryList>(selectSql.ToString(), info);
            if (inoutList == null || inoutList.Count == 0)
            {
                return false;
            }

            // 値と単位結合
            for (int i = 0; i < inoutList.Count(); i++)
            {
                inoutList = joinBlankConclusion<Dao.inoutHistryList>(inoutList, info, i);
            }

            // 繰越があれば先頭行にデータに挿入
            if (forwardList != null)
            {
                inoutList.Insert(0, forwardList);
                // 制御用フラグ(js側で使用)
                inoutList[0].ControlFlag = "0"; //繰越
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.inoutHistryList>(inoutInfo, inoutList, inoutList.Count(), true))
            {
                // 正常終了
                return true;
            }
            return false;
        }

        /// <summary>
        /// 値と単位を結合(繰越データ用)
        /// </summary>
        /// <param name="result">一覧</param>
        /// <param name="info">遷移元の情報</param>
        private Dao.inoutHistryList joinBlankForwardList(Dao.inoutHistryList result, Dao.inoutHistryList info)
        {
            // 入庫数がある場合
            if (result.InventoryQuantity != null)
            {
                // 入庫数
                result.InventoryQuantityDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.InventoryQuantity.ToString(), result.UnitDigit, result.UnitRoundDivision), result.UnitName, false);
                // 入庫単価
                result.UnitPriceDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.UnitPrice.ToString(), result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName, false);
                // 入庫金額
                result.InventoryAmountDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.InventoryAmount.ToString(), result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName, false);
            }

            // 在庫数がある場合
            if (result.StockQuantity != null)
            {
                // 在庫数
                result.StockQuantityDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.StockQuantity.ToString(), result.UnitDigit, result.UnitRoundDivision), result.UnitName, false);
                // 在庫金額
                result.StockAmountDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.StockAmount.ToString(), result.CurrencyDigit, result.CurrencyRoundDivision), result.CurrencyName, false);
            }

            // 予備品ID表示(隠し項目)
            result.PartsId = info.PartsId;

            return result;
        }

        /// <summary>
        /// 値と単位を結合
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="results">一覧</param>
        /// <param name="info">遷移元の情報</param>
        /// <param name="rowNo">行番号</param>
        private IList<Dao.inoutHistryList> joinBlankConclusion<T>(IList<Dao.inoutHistryList> results, Dao.inoutHistryList info, int rowNo)
        {
            // 入庫数がある場合
            if (results[rowNo].InventoryQuantity != null)
            {
                // 入庫数
                results[rowNo].InventoryQuantityDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(results[rowNo].InventoryQuantity.ToString(), results[rowNo].UnitDigit, results[rowNo].UnitRoundDivision), results[rowNo].UnitName, false);
                // 入庫単価
                results[rowNo].UnitPriceDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(results[rowNo].UnitPrice.ToString(), results[rowNo].CurrencyDigit, results[rowNo].CurrencyRoundDivision), results[rowNo].CurrencyName, false);
                // 入庫金額
                results[rowNo].InventoryAmountDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(results[rowNo].InventoryAmount.ToString(), results[rowNo].CurrencyDigit, results[rowNo].CurrencyRoundDivision), results[rowNo].CurrencyName, false);
            }

            // 出庫数がある場合
            if (results[rowNo].IssueQuantity != null)
            {
                // 出庫数
                results[rowNo].IssueQuantityDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(results[rowNo].IssueQuantity.ToString(), results[rowNo].UnitDigit, results[rowNo].UnitRoundDivision), results[rowNo].UnitName, false);
                // 出庫金額
                results[rowNo].IssueAmountDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(results[rowNo].IssueAmount.ToString(), results[rowNo].CurrencyDigit, results[rowNo].CurrencyRoundDivision), results[rowNo].CurrencyName, false);
            }

            // 在庫数がある場合
            if (results[rowNo].StockQuantity != null)
            {
                // 在庫数
                results[rowNo].StockQuantityDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(results[rowNo].StockQuantity.ToString(), results[rowNo].UnitDigit, results[rowNo].UnitRoundDivision), results[rowNo].UnitName, false);
                // 在庫金額
                results[rowNo].StockAmountDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(results[rowNo].StockAmount.ToString(), results[rowNo].CurrencyDigit, results[rowNo].CurrencyRoundDivision), results[rowNo].CurrencyName, false);
            }

            // 予備品ID表示(隠し項目)
            results[rowNo].PartsId = info.PartsId;

            return results;
        }

        /// <summary>
        /// 初期値を取得する(構成IDを返す)
        /// </summary>
        /// <param name="structureId">構成グループID</param>
        /// <param name="seq">連番</param>
        /// <param name="extentionData">拡張データ</param>
        /// <returns>初期値の構成ID</returns>
        private int getInitialValue(int structureId, int seq, string extentionData)
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();

            //構成グループID
            param.StructureGroupId = structureId;

            //連番
            param.Seq = seq;

            // 機能レベル取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> partsTypeList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            foreach (var partsType in partsTypeList)
            {
                if (partsType.ExData == extentionData)
                {
                    return partsType.StructureId;
                }
            }
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 初期値を取得する(翻訳を返す)
        /// </summary>
        /// <param name="structureId">構成グループID</param>
        /// <param name="seq">連番</param>
        /// <param name="extentionData">拡張データ</param>
        /// <returns>初期値の構成ID</returns>
        private string getInitialValueName(int structureId, int seq, string extentionData)
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();

            //構成グループID
            param.StructureGroupId = structureId;

            //連番
            param.Seq = seq;

            // 機能レベル取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> partsTypeList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            foreach (var partsType in partsTypeList)
            {
                if (partsType.ExData == extentionData)
                {
                    return partsType.TranslationText;
                }
            }
            return string.Empty;
        }
    }
}
