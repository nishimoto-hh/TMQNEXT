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
using FunctionTypeId = CommonTMQUtil.CommonTMQConstants.Attachment.FunctionTypeId;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_PT0001
{
    /// <summary>
    /// 一覧画面
    /// </summary>
    public partial class BusinessLogic_PT0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.List.GetPartsList, out string baseSql);
            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.List.GetPartsList, out string withSql);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied, true, isJobKindOnly: true, isJobNullAble: true))
            {
                return false;
            }

            //SQLパラメータに条件設定
            whereParam.LanguageId = this.LanguageId;　// 言語ID

            // SQL、WHERE句、WITH句より件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);

            // 翻訳の一時テーブルを作成
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);

            // 添付情報作成
            listPf.GetAttachmentSql(new List<FunctionTypeId> { FunctionTypeId.SpareImage, FunctionTypeId.SpareDocument });

            // 翻訳する構成グループのリスト
            var structuregroupList = new List<GroupId>
            {
                GroupId.Location,
                GroupId.Manufacturer,
                GroupId.Vender,
                GroupId.Unit,
                GroupId.Currency,
                GroupId.Job,
                GroupId.SpareLocation,
                GroupId.Department,
                GroupId.Account
            };
            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            listPf.RegistTempTable(); // 登録

            // 総件数を取得
            // 件数を取得するSQLを取得
            int cnt = db.GetCount(executeSql, whereParam);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, null, cnt, isDetailConditionApplied);
                return false;
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql, isDetailConditionApplied);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY");    // 並び順を指定
            selectSql.AppendLine("parts_no");    // 予備品No. 昇順
            selectSql.AppendLine(",parts_name"); // 予備品名  昇順

            // グローバルデータにRFIDタグ件数の単位が存在しない場合は取得して保持
            if (GetGlobalData(GlobalKey.PT0001Matter) == null)
            {
                SetGlobalData(GlobalKey.PT0001Matter, GetResMessage(ResourceKey.Matter));
            }

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, null, 0, isDetailConditionApplied);
                return false;
            }

            // 丸め処理・数量と単位を結合
            results.ToList().ForEach(x => x.JoinStrAndRound());

            // 棚番と枝番を結合
            JoinLocationAndDetailNo(results);

            // 検索結果の設定
            if (!SetSearchResultsByDataClassForList<Dao.searchResult>(pageInfo, results, cnt, isDetailConditionApplied))
            {
                return false;
            }

            //RFタグ取込画面の取込後の再表示であれば、メッセージを設定
            object message = GetGlobalData(ConductInfo.FormRFUpload.GlobalKey, true);
            if (message != null)
            {
                this.MsgId = message.ToString();
            }

            // グローバルデータに発注アラーム判定用フラグを保持
            if (GetGlobalData(GlobalKey.PT0001OrderAlertJudgeFlg) == null)
            {
                SetGlobalData(GlobalKey.PT0001OrderAlertJudgeFlg, results.First().OrderAlertJudgeFlg);
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;

            /// <summary>
            /// 棚番と枝番を結合する
            /// </summary>
            /// <param name="results">検索結果</param>
            void JoinLocationAndDetailNo(IList<Dao.searchResult> results)
            {
                // 棚番と枝番がある場合は結合
                string joinStr = string.Empty;
                // 工場IDと結合文字列のディクショナリ、同じ工場で重複取得しないようにする
                Dictionary<int, string> factoryJoinDic = new();
                foreach (Dao.searchResult result in results)
                {
                    if (!string.IsNullOrEmpty(result.RackName) && !string.IsNullOrEmpty(result.PartsLocationDetailNo))
                    {
                        // 結合文字列を取得
                        joinStr = TMQUtil.GetJoinStrOfPartsLocationNoDuplicate((int)result.DefaultFactoryId, this.LanguageId, db, ref factoryJoinDic);
                        // 棚番 + 枝番
                        result.RackName = TMQUtil.GetDisplayPartsLocation(result.RackName, result.PartsLocationDetailNo, joinStr);
                    }
                }
            }
        }

        /// <summary>
        /// RFタグ情報出力
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool outputRftagInfo()
        {
            // 出力データを取得する
            List<Dao.rftagFileInfo> dataList = TMQUtil.SqlExecuteClass.SelectList<Dao.rftagFileInfo>(SqlName.List.GetRftagOutputData, SqlName.SubDir, new { UserId = this.UserId }, this.db);

            // 出力対象データ
            List<object[]> list = getDataRow();

            // CSV出力処理
            if (!ComUtil.ExportCsvFileNotencircleDobleQuotes(list, Encoding.GetEncoding("Shift-JIS"), out Stream outStream, out string errMsg))
            {
                // エラーログ出力
                logger.ErrorLog(this.FactoryId, this.UserId, errMsg);
                // 「出力処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { "941220002", "911120006" });
                return false;
            }

            // 出力情報設定
            this.OutputFileType = CommonConstants.REPORT.FILETYPE.CSV;
            this.OutputFileName = ConductInfo.FormList.RftagFileName + ComConsts.REPORT.EXTENSION.CSV;
            this.OutputStream = outStream;

            return true;

            // 出力するデータ
            List<object[]> getDataRow()
            {
                // データ行を作成
                List<object[]> dataRow = new();

                if (dataList == null || dataList.Count == 0)
                {
                    return dataRow;
                }

                foreach (Dao.rftagFileInfo data in dataList)
                {
                    dataRow.Add(new object[]
                    {
                        data.IsoIdentifier, // ISO識別子
                        data.IssuingAgency, // 発番機関
                        data.SymbolicCode, // 記号コード：登録No
                        data.Factory, // 工場
                        data.SerialNo, // シリアルNo
                        data.Cd, // CD
                        data.PartsNo, // 予備品No.
                        "", // ブランク
                        data.DepartmentCode, // 部門コード
                        "", // ブランク
                        data.AccountCode // 勘定科目コード
                    });
                }

                return dataRow;
            }
        }

    }
}
