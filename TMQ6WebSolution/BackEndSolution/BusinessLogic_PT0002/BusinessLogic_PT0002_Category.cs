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
using TMQUtilDao = CommonTMQUtil.CommonTMQUtilDataClass;

namespace BusinessLogic_PT0002
{
    /// <summary>
    /// 部門移庫タブ
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

        /// <summary>
        /// ラベル出力処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool outputLabelCategory()
        {
            // ラベル出力クラス
            TMQUtil.Label label = new(this.db, this.LanguageId);

            // 部門移庫一覧で選択されたレコードを取得
            var selectedList = getSelectedRowsByList(this.resultInfoDictionary, TargetCtrlId.CategoryResults);

            // 出力をするための検索条件リスト
            List<TMQUtilDao.LabelCondition> conditionList = new();

            // 各レコードごとに出力データを作成
            foreach (Dictionary<string, object> selectedRow in selectedList)
            {
                // 検索条件を取得
                TMQUtilDao.LabelCondition condition = new();

                // ディクショナリをデータクラスに変換
                SetDataClassFromDictionary(selectedRow, TargetCtrlId.CategoryResults, condition);

                // 検索条件リストに追加
                conditionList.Add(condition);
            }

            // ラベル出力データ取得処理
            if (!label.GetLabelData(conditionList, out List<object[]> outList))
            {
                return false;
            }

            // CSV出力処理
            if (!CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.ExportCsvFileNotencircleDobleQuotes(outList, Encoding.GetEncoding("Shift-JIS"), out Stream outStream, out string errMsg))
            {
                // エラーログ出力
                logger.ErrorLog(this.FactoryId, this.UserId, errMsg);
                // 「出力処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120006 });
                return false;
            }

            // 画面の出力へ設定
            this.OutputFileType = CommonConstants.REPORT.FILETYPE.CSV;
            this.OutputFileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", label.ReportId, DateTime.Now) + ComConsts.REPORT.EXTENSION.CSV;
            this.OutputStream = outStream;

            return true;
        }
    }
}
