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
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_PT0001.BusinessLogicDataClass_PT0001;
using DbTransaction = System.Data.IDbTransaction;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_PT0001
{
    /// <summary>
    /// ラベル出力画面
    /// </summary>
    public partial class BusinessLogic_PT0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// CSV出力時の値を囲む文字列
        /// </summary>
        private string encircleValue = "\"";
        /// <summary>
        /// 帳票ID(出力したファイルの先頭に設定)
        /// </summary>
        private string reportId = "RP0280";

        /// <summary>
        /// ラベル出力画面 検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchLabelList()
        {
            // 勘定科目選択一覧検索
            if (!selectLabelList<Dao.labelSubjectList>(ConductInfo.FormLabel.ControlId.SubjectList, SqlName.Label.GetLabelSubjectList, out int subjectCnt))
            {
                return false;
            }

            // 部門選択一覧検索
            if (!selectLabelList<Dao.labelDepartmentList>(ConductInfo.FormLabel.ControlId.DepartmentList, SqlName.Label.GetLabelDepartmentList, out int categoryCnt))
            {
                return false;
            }

            // どちらの一覧も検索結果が無い場合はエラーとする
            if (subjectCnt + categoryCnt == 0)
            {
                return false;
            }

            // 予備品一覧検索(非表示)
            if (!setPartsIdList())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 勘定科目選択一覧、部門選択一覧 検索処理
        /// </summary>
        /// <param name="ctrlId">一覧のコントロールID</param>
        /// <param name="sqlName">SQL名</param>
        /// <param name="resultCnt">out データ件数</param>
        /// <returns>エラーの場合False</returns>
        private bool selectLabelList<T>(string ctrlId, string sqlName, out int resultCnt)
        {
            resultCnt = 0;

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out string baseSql);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
            {
                return false;
            }

            // 検索条件追加
            whereParam.LanguageId = this.LanguageId; // 言語ID
            whereParam.userId = this.UserId;         // ユーザーID

            // 検索条件を再生成(勘定科目一覧検索の場合)
            if(sqlName == SqlName.Label.GetLabelSubjectList)
            {
                // 「AND」でSQLを分割
                whereSql = whereSql.Replace("WHERE", "AND").Replace("location_structure_id", "item2.location_structure_id");
                string[] condList = whereSql.Split(")");
                condList[0] = condList[0] + "OR item2.location_structure_id = " + TMQConst.CommonFactoryId.ToString();
                whereSql = condList[0] + "\r\n" + ")";
                // SQLを再生成した条件で置換
                baseSql = baseSql.Replace("ReplaceLocationIdList", whereSql);
            }

            // 一覧検索実行
            IList<T> results = db.GetListByDataClass<T>(baseSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                return true;
            }

            resultCnt = results.Count;
            // 検索結果の設定
            if (!SetSearchResultsByDataClass<T>(pageInfo, results, results.Count, false))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 一覧画面で選択されたレコードの予備品IDを設定する
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool setPartsIdList()
        {
            string ctrlId = ConductInfo.FormList.ControlId.List;
            // 選択されたレコード取得
            var partsIdList = getSelectedRowsByList(this.searchConditionDictionary, ctrlId);

            List<ComDao.PtPartsEntity> results = new();
            foreach (var partsId in partsIdList)
            {
                ComDao.PtPartsEntity condition = new();
                SetDataClassFromDictionary(partsId, ctrlId, condition, new List<string> { "PartsId" });
                // リストに格納
                results.Add(condition);
            }

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormLabel.ControlId.PartsIdList, this.pageInfoList);

            // 予備品IDの設定
            if (!SetSearchResultsByDataClass<ComDao.PtPartsEntity>(pageInfo, results, results.Count, false))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ラベル用データを出力する
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool outputLabelData()
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Label.GetLabelData, out string outSql);
            // 検索条件作成
            dynamic whereParam = new ExpandoObject();
            // 予備品IDリスト
            whereParam.PartsIdList = getPartsIdList();
            // 勘定科目コードリスト
            whereParam.SubjectCodeList = getSubjectCodeIdList();
            // 部門コードリスト
            whereParam.DepartmentCodeList = getDepartmentCodeIdList();
            // 言語ID
            whereParam.LanguageId = this.LanguageId;

            // 一覧検索実行
            IList<Dao.labelData> results = db.GetListByDataClass<Dao.labelData>(outSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 検索結果を配列に追加
            List<object[]> list = new();
            foreach (var result in results)
            {
                // 倉庫の場合は空にする
                if (result.StructureLayerNo == 2)
                {
                    result.ShedName = string.Empty;
                }

                // 棚名称・枝番が空でないかつ、標準棚番の構成IDの階層番号が「3」の場合
                if (!string.IsNullOrEmpty(result.ShedName) && !string.IsNullOrEmpty(result.PartsLocationDetailNo) && result.StructureLayerNo == 3)
                {
                    // 標準棚番 + 枝番
                    result.ShedName = TMQUtil.GetDisplayPartsLocation(result.ShedName, result.PartsLocationDetailNo, result.PartsFactoryId, this.LanguageId, this.db);
                }
                list.Add(new object[]
                {
                  encircleValue + result.PartsNo + encircleValue,                                                      // 予備品No.
                  encircleValue + result.PartsName + encircleValue,                                                    // 予備品名
                  encircleValue + result.Maker + encircleValue,                                                        // メーカー
                  encircleValue + result.ModelType + encircleValue,                                                    // 型式
                  encircleValue + result.StandardSize + encircleValue,                                                 // 規格・寸法
                  encircleValue + result.DepartmentCode + encircleValue,                                               // 部門コード
                  encircleValue + result.SubjectCode + encircleValue,                                                  // 勘定科目コード
                  encircleValue + result.ShedName + encircleValue,                                                     // 標準棚番 + 枝番
                  encircleValue + result.LeadTime + encircleValue,                                                     // 発注点
                  encircleValue + result.OrderQuantity + encircleValue,                                                // 発注量
                  encircleValue + result.Qrc + encircleValue                                                           // QRコード
                });
            }

            // CSV出力処理
            if (!CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.ExportCsvFileNotencircleDobleQuotes(list, Encoding.GetEncoding("Shift-JIS"), out Stream outStream, out string errMsg))
            {
                this.MsgId = errMsg;
                return false;
            }

            // 画面の出力へ設定
            this.OutputFileType = CommonConstants.REPORT.FILETYPE.CSV;
            this.OutputFileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", reportId, DateTime.Now) + ComConsts.REPORT.EXTENSION.CSV;
            this.OutputStream = outStream;

            return true;
        }

        /// <summary>
        /// 予備品ID一覧より値を取得
        /// </summary>
        /// <returns>予備品ID</returns>
        private string getPartsIdList()
        {
            var tmpList = convertDicListToClassList<ComDao.PtPartsEntity>(this.resultInfoDictionary, ConductInfo.FormLabel.ControlId.PartsIdList);
            string param = string.Empty;
            bool firstTime = true;

            foreach (var data in tmpList)
            {
                // 初回は値の追加のみ
                if (firstTime)
                {
                    param += data.PartsId.ToString();
                    firstTime = false;
                    continue;
                }

                // 2つ目以降の値はカンマ区切り
                param += ",";
                param += data.PartsId.ToString();
            }

            return param;
        }

        /// <summary>
        /// 勘定科目選択一覧より値を取得
        /// </summary>
        /// <returns>勘定科目コード</returns>
        private string getSubjectCodeIdList()
        {
            var tmpList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormLabel.ControlId.SubjectList);
            string param = string.Empty;
            bool firstTime = true;

            foreach (var data in tmpList)
            {
                Dao.labelSubjectList result = new();
                SetDataClassFromDictionary(data, ConductInfo.FormLabel.ControlId.DepartmentList, result);
                // 初回は値の追加のみ
                if (firstTime)
                {
                    param += result.ItemExtensionData1;
                    firstTime = false;
                    continue;
                }

                // 2つ目以降の値はカンマ区切り
                param += ",";
                param += result.ItemExtensionData1.ToString();
            }

            return param;
        }

        /// <summary>
        /// 部門選択一覧より値を取得
        /// </summary>
        /// <returns>部門コード</returns>
        private string getDepartmentCodeIdList()
        {
            var tmpList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormLabel.ControlId.DepartmentList);
            string param = string.Empty;
            bool firstTime = true;

            foreach (var data in tmpList)
            {
                Dao.labelDepartmentList result = new();
                SetDataClassFromDictionary(data, ConductInfo.FormLabel.ControlId.DepartmentList, result);
                // 初回は値の追加のみ
                if (firstTime)
                {
                    param += result.ItemExtensionData1;
                    firstTime = false;
                    continue;
                }

                // 2つ目以降の値はカンマ区切り
                param += ",";
                param += result.ItemExtensionData1.ToString();
            }

            return param;
        }

        /// <summary>
        /// 結合文字列+枝番結合処理
        /// </summary>
        /// <param name="joinStr">結合文字列</param>
        /// <param name="detaiNo">枝番</param>
        /// <returns>結合文字列+枝番</returns>
        private string addJoinStr(string joinStr, string detaiNo)
        {
            // 枝番が空の場合は空文字を返す
            if (string.IsNullOrEmpty(detaiNo))
            {
                return string.Empty;
            }
            else
            {
                return joinStr + detaiNo;
            }
        }
    }
}
