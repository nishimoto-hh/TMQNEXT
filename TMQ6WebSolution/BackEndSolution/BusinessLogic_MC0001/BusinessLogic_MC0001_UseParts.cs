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
using Dao = BusinessLogic_MC0001.BusinessLogicDataClass_MC0001;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_MC0001
{
    /// <summary>
    /// 機器台帳(使用部品タブ)
    /// </summary>
    public partial class BusinessLogic_MC0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlNameUseParts
        {
            /// <summary>SQL名：使用部品 使用部品一覧取得</summary>
            public const string GetUseParts = "GetUseParts";
            /// <summary>SQL名：使用部品 使用部品重複チェック</summary>
            public const string GetUsePartsCheck = "GetUsePartsCheck";
            /// <summary>SQL名：使用部品 使用部品登録</summary>
            public const string InsertUseParts = "InsertUseParts";
            /// <summary>SQL名：使用部品 使用部品更新</summary>
            public const string UpdateUseParts = "UpdateUseParts";
            /// <summary>SQL名：使用部品 使用部品削除</summary>
            public const string DeleteUseParts = "DeleteUseParts";

        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlIdUseParts
        {
            /// <summary>
            /// 使用部品 使用部品一覧
            /// </summary>
            public const string DetailUseParts = "BODY_230_00_LST_1";
        }

        /// <summary>
        /// 使用部品一覧で行削除ボタン(-アイコン)クリック時に連携される削除ボタンの名称
        /// </summary>
        private const string DeleteUseParts = "DeleteUseParts";

        /// <summary>
        /// 使用部品検索
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool searchUseParts(long machineId)
        {
            // 対象機器の工場ID取得
            dynamic whereParam = new { MachineId = machineId };
            string sql;
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }
            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, typeLst, this.db, this.LanguageId, true);

            // 使用部品一覧初期検索
            sql = null;
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameUseParts.GetUseParts, out sql);
            whereParam = new { MachineId = machineId, LanguageId = this.LanguageId, FactoryId = results[0].LocationStructureId};

            // 一覧検索実行
            IList<Dao.usePartsListResult> results1 = db.GetListByDataClass<Dao.usePartsListResult>(sql, whereParam);
            if (results1 == null || results1.Count == 0)
            {
                // 正常終了
                return true;
            }
            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlIdUseParts.DetailUseParts, this.pageInfoList);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.usePartsListResult>(pageInfo, results1, results.Count))
            {
                // 異常終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="result">入力データ</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool registUseParts(Dictionary<string, object> result)
        {
            // 入力された内容を取得
            Dao.usePartsListResult registResult = getRegist<Dao.usePartsListResult>(result, TargetCtrlIdUseParts.DetailUseParts);
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();
            // エラー情報を画面に設定するためのマッピング情報リスト
            var info = getResultMappingInfo(TargetCtrlIdUseParts.DetailUseParts);
            // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
            // 単一の内容を取得
            Dictionary<string, object> targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlIdUseParts.DetailUseParts);

            if(registResult.PartsId == null)
            {
                //// エラー情報格納クラス
                //ErrorInfo errorInfo = new ErrorInfo(targetDic);
                ////string errMsg = GetResMessage("141220003");
                //string errMsg = ComRes.ID.ID941270001;
                //string val = info.getValName("parts_id"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                //errorInfo.setError(errMsg, val);
                //errorInfoDictionary.Add(errorInfo.Result);
                //// エラー情報を画面に反映
                //SetJsonResult(errorInfoDictionary);
                // 部品名は必須項目です
                this.MsgId = string.Format(GetResMessage(ComRes.ID.ID141060006), GetResMessage("111280004"));
                return false;
            }

            if (registResult.UseQuantity == null)
            {
                // 使用個数は必須項目です
                this.MsgId = string.Format(GetResMessage(ComRes.ID.ID141060006), GetResMessage("111120009"));
                return false;
            }

            if (registResult.MachineUsePartsId == -1)
            {
                // 選択された予備品が別で登録されていないかチェック
                if (!checkUsePartsDuplicate(registResult))
                {
                    //// エラー情報格納クラス
                    //ErrorInfo errorInfo = new ErrorInfo(targetDic);
                    //// 入力された部品は既に登録されています。
                    //string errMsg = GetResMessage("141220003");
                    //string val = info.getValName("parts_id"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    //errorInfo.setError(errMsg, val);
                    //errorInfoDictionary.Add(errorInfo.Result);
                    //// エラー情報を画面に反映
                    //SetJsonResult(errorInfoDictionary);
                    this.MsgId = GetResMessage("141220003");
                    return false;
                }

                // 新規登録
                // 機番使用部品
                (bool returnFlag, long id) useParts = registInsertDb<Dao.usePartsListResult>(registResult, SqlNameUseParts.InsertUseParts);
                if (!useParts.returnFlag)
                {
                    return false;
                }
            }
            else
            {
                // 排他チェック
                if (isErrorExclusiveList(TargetCtrlIdUseParts.DetailUseParts))
                {
                    return false;
                }

                // 選択された予備品が別で登録されていないかチェック
                if (!checkUsePartsDuplicate(registResult))
                {
                    //// エラー情報格納クラス
                    //ErrorInfo errorInfo = new ErrorInfo(targetDic);
                    //// 入力された部品は既に登録されています。
                    //string errMsg = GetResMessage("141220003");
                    //string val = info.getValName("parts_id"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    //errorInfo.setError(errMsg, val);
                    //errorInfoDictionary.Add(errorInfo.Result);
                    //// エラー情報を画面に反映
                    //SetJsonResult(errorInfoDictionary);
                    this.MsgId = GetResMessage("141220003");
                    return false;
                }

                // 更新
                // 機番使用部品
                if (!registUpdateDb<Dao.usePartsListResult>(registResult, SqlNameUseParts.UpdateUseParts))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 使用部品一覧 削除処理
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteUseParts()
        {
            // 削除リスト取得
            var deleteList = getSelectedRowsByList(this.resultInfoDictionary, TargetCtrlIdUseParts.DetailUseParts);

            // 排他チェック
            if (isErrorExclusiveList(TargetCtrlIdUseParts.DetailUseParts))
            {
                return false;
            }

            // チェック分削除処理実施
            foreach (var deleteRow in deleteList)
            {
                // 削除SQL取得
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameUseParts.DeleteUseParts, out string sql1))
                {
                    return false;
                }
                Dao.usePartsListResult delCondition = new();
                SetDeleteConditionByDataClass(deleteRow, TargetCtrlIdUseParts.DetailUseParts, delCondition);

                // 使用部品一覧
                int result = this.db.Regist(sql1, delCondition);
                if (result < 0)
                {
                    // 削除エラー
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegist<T>(Dictionary<string, object> result, string ctrlId)
        where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // データクラスに変換
            T data = new();

            if (!SetExecuteConditionByDataClass<T>(result, ctrlId, data, DateTime.Now, this.UserId, this.UserId))
            {
                // エラーの場合終了
                return data;
            }

            return data;
        }

        /// <summary>
        /// 予備品重複チェック
        /// </summary>
        /// <param name="result">入力データ</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool checkUsePartsDuplicate(Dao.usePartsListResult result)
        {
            // 検索SQL文の取得
            dynamic whereParam = null; // WHERE句パラメータ
            string sql = string.Empty;
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameUseParts.GetUsePartsCheck, out sql))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            whereParam = new { @PartsId = result.PartsId, @MachineUsePartsId = result.MachineUsePartsId, MachineId = result.MachineId };
            // 総件数を取得
            int cnt = db.GetCount(sql, whereParam);
            if (cnt > 0)
            {
                return false;
            }

            return true;
        }

    }
}
