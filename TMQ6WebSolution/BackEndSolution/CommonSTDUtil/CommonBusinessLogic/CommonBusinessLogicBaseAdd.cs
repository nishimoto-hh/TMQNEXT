using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
namespace CommonSTDUtil.CommonBusinessLogic
{
    public abstract partial class CommonBusinessLogicBase : MarshalByRefObject
    {
        #region 機能で良く使用する処理を呼び出し用にまとめた処理
        /// <summary>
        /// 画面情報の指定された一覧項目をデータクラスのリストに変換する処理
        /// </summary>
        /// <typeparam name="T">変換するデータクラスの型</typeparam>
        /// <param name="dictionaryList">画面項目のディクショナリリスト</param>
        /// <param name="ctrlId">変換する一覧のコントロールID</param>
        /// <param name="columnList">省略可能　変換する項目の列名</param>
        /// <returns>変換したデータクラスのリスト</returns>
        protected List<T> convertDicListToClassList<T>(List<Dictionary<string, object>> dictionaryList, string ctrlId, List<string> columnList = null)
            where T : new()
        {
            // コントロールIDでディクショナリリストを絞り込み
            var targetDicList = ComUtil.GetDictionaryListByCtrlId(dictionaryList, ctrlId);
            // 取得するデータクラスのリスト
            List<T> returnList = new();
            foreach (var targetDic in targetDicList)
            {
                T targetClass = new();
                // 変換してリストに追加
                SetDataClassFromDictionary(targetDic, ctrlId, targetClass, columnList);
                returnList.Add(targetClass);
            }
            return returnList;
        }

        /// <summary>
        /// 一覧を指定し、チェックボックスが選択されている行のディクショナリリストを取得する処理
        /// </summary>
        /// <param name="targetDicList">取得対象の画面情報</param>
        /// <param name="listCtrlId">一覧のコントロールID</param>
        /// <returns>選択されている行のリスト</returns>
        protected List<Dictionary<string, object>> getSelectedRowsByList(List<Dictionary<string, object>> targetDicList, string listCtrlId)
        {
            // 選択行も含めて一覧の情報を取得
            var list = ComUtil.GetDictionaryListByCtrlId(targetDicList, listCtrlId);
            // 選択行のリスト
            List<Dictionary<string, object>> selectedList = new();
            foreach (var row in list)
            {
                // 行が選択されているか判定
                if (!ComUtil.IsSelectedRowDictionary(row))
                {
                    // 選択行で無ければスキップ
                    continue;
                }
                // 選択行の場合、リストへ追加
                selectedList.Add(row);
            }
            // 選択行のリストを返す
            return selectedList;
        }

        /// <summary>
        /// 単票の削除処理
        /// </summary>
        /// <typeparam name="T">削除するテーブルのデータクラス　削除条件に使用</typeparam>
        /// <param name="ctrlId">削除する単票のコントロールID</param>
        /// <param name="deleteSql">DELETEのSQL文</param>
        /// <param name="isErrorInput">削除処理で行う入力チェック　削除する単票のコントロールIDとその内容が引数で、エラーの場合Trueを返す処理</param>
        /// <returns>エラーの場合False</returns>
        protected bool deleteTargetCtrl<T>(string ctrlId, string deleteSql, Func<string, Dictionary<string, object>, bool> isErrorInput = null)
             where T : new()
        {
            // 排他チェック
            if (!checkExclusiveSingle(ctrlId))
            {
                // 排他エラー
                return false;
            }

            // 削除対象の画面項目
            var deleteCtrl = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);

            // 入力チェック
            if (isErrorInput != null && isErrorInput(ctrlId, deleteCtrl))
            {
                // 処理が存在し戻り値がFalse(エラー)の場合、入力チェックエラー
                return false;
            }

            // 削除SQL実行
            T deleteCondition = new();
            SetDeleteConditionByDataClass(deleteCtrl, ctrlId, deleteCondition);
            int result = this.db.Regist(deleteSql, deleteCondition);
            if (result < 0)
            {
                // 削除エラー
                return false;
            }

            return true;
        }

        /// <summary>
        /// 一覧の削除処理
        /// </summary>
        /// <typeparam name="T">削除するテーブルのデータクラス　削除条件に使用</typeparam>
        /// <param name="listCtrlId">削除する一覧のコントロールID</param>
        /// <param name="deleteSql">DELETEのSQL文</param>
        /// <param name="isErrorInput">削除処理で行う入力チェック　削除する一覧のコントロールIDとその内容リストが引数で、エラーの場合Trueを返す処理</param>
        /// <returns>エラーの場合False</returns>
        protected bool DeleteSelectedList<T>(string listCtrlId, string deleteSql, Func<string, List<Dictionary<string, object>>, bool> isErrorInput = null)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            var deleteList = getSelectedRowsByList(this.resultInfoDictionary, listCtrlId);

            // 排他チェック
            if (!checkExclusiveList(listCtrlId, deleteList))
            {
                // 排他エラー
                return false;
            }

            // 入力チェック
            if (isErrorInput != null && isErrorInput(listCtrlId, deleteList))
            {
                // 処理が存在し戻り値がFalse(エラー)の場合、入力チェックエラー
                return false;
            }

            DateTime updateDateTime = DateTime.Now;
            // 行削除
            foreach (var deleteRow in deleteList)
            {
                T deleteCondition = new();
                SetExecuteConditionByDataClass<T>(deleteRow, listCtrlId, deleteCondition, updateDateTime, this.UserId);
                int result = this.db.Regist(deleteSql, deleteCondition);
                if (result < 0)
                {
                    // 削除エラー
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 指定した画面項目に値を設定する処理
        /// </summary>
        /// <typeparam name="T">セットする値の型</typeparam>
        /// <param name="ctrlId">値をセットする一覧のコントロールID</param>
        /// <param name="results">セットする値リスト</param>
        /// <returns>エラーの場合False</returns>
        protected bool SetFormByDataClass<T>(string ctrlId, IList<T> results)
        {
            if (results == null || results.Count == 0)
            {
                // 一覧が0件の場合はエラーとしない
                return true;
            }
            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);
            // 一覧に値をセット
            bool retVal = SetSearchResultsByDataClass<T>(pageInfo, results, results.Count);
            return retVal;
        }


        /// <summary>
        /// フォームの情報を取得
        /// </summary>
        /// <typeparam name="T">戻り値のクラスの型</typeparam>
        /// <param name="ctrlId">取得するフォームのコントロールID</param>
        /// <param name="isResult">省略時はTrue ResultInfoDictionaryより取得する場合True、SearchConditionDictionaryより取得する場合False</param>
        /// <returns>取得した値</returns>
        protected T GetFormDataByCtrlId<T>(string ctrlId, bool isResult = true)
             where T : new()
        {
            // 画面の内容
            var targetDic = ComUtil.GetDictionaryByCtrlId(isResult ? this.resultInfoDictionary : this.searchConditionDictionary, ctrlId);
            // データクラスに変換
            T data = new();
            SetDataClassFromDictionary(targetDic, ctrlId, data);
            return data;
        }

        /// <summary>
        /// 画面の検索条件をグループNoにより取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <returns>検索条件のデータクラス</returns>
        protected T GetConditionInfoByGroupNo<T>(short groupNo)
             where T : Dao.SearchCommonClass, new()
        {
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.searchConditionDictionary, ctrlIdList, true);

            T resultInfo = new();
            // コントロールIDごとに繰り返し
            foreach (var ctrlId in ctrlIdList)
            {
                // コントロールIDより画面の項目を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ctrlId);
                var mapInfo = grpMapInfo.selectByCtrlId(ctrlId);

                // ページ情報取得
                var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);
                // 検索条件データの設定
                if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, ctrlId, resultInfo, pageInfo))
                {
                    // エラーの場合終了
                    return resultInfo;
                }
            }
            return resultInfo;
        }

        /// <summary>
        /// 画面の検索条件をグループNoにより取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNoList">グループNoのリスト</param>
        /// <returns>検索条件のデータクラス</returns>
        protected T GetConditionInfoByGroupNoList<T>(List<int> groupNoList)
             where T : Dao.SearchCommonClass, new()
        {
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfoByGrpNoList(groupNoList);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.searchConditionDictionary, ctrlIdList, true);

            T resultInfo = new();
            // コントロールIDごとに繰り返し
            foreach (var ctrlId in ctrlIdList)
            {
                // コントロールIDより画面の項目を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ctrlId);
                var mapInfo = grpMapInfo.selectByCtrlId(ctrlId);

                // ページ情報取得
                var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);
                // 検索条件データの設定
                if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, ctrlId, resultInfo, pageInfo))
                {
                    // エラーの場合終了
                    return resultInfo;
                }
            }
            return resultInfo;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNo">取得するグループ</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        protected T GetRegistInfoByGroupNo<T>(short groupNo, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList, true);

            T resultInfo = new();
            // コントロールIDごとに繰り返し
            foreach (var ctrlId in ctrlIdList)
            {
                // コントロールIDより画面の項目を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ctrlId);
                var mapInfo = grpMapInfo.selectByCtrlId(ctrlId);

                // 登録データの設定
                if (!SetExecuteConditionByDataClass(condition, mapInfo.Value, resultInfo, now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return resultInfo;
                }
            }
            return resultInfo;
        }
        #endregion

        /// <summary>
        /// 16桁を超える大きな数値を使用する場合、フロント側へのデータ送信で値が変わってしまうので、保持するよう文字列へ変換する処理
        /// </summary>
        /// <param name="ctrlId">大きな値の画面項目を持つ一覧のコントロールID</param>
        /// <param name="colVals">大きな値の画面項目のVAL値のリスト</param>
        /// <remarks>画面へ値をセットした後に使用</remarks>
        protected void ConvertResultBigValueAvaible(string ctrlId, List<string> colVals)
        {
            // 画面の項目を取得
            var targets = ComUtil.GetDictionaryListByCtrlId(this.ResultList, ctrlId);
            // 繰り返し処理
            foreach (var rowDic in targets)
            {
                // VAL値の数分繰り返し
                colVals.ForEach(x => changeBigValue(x, rowDic));
            }
            // 取得した画面の内容の指定された画面項目の値を大きな数値を保持できるようにする
            void changeBigValue(string colVal, Dictionary<string, object> rowDic)
            {
                if (!rowDic.ContainsKey(colVal))
                {
                    // VAL値が無い場合終了
                    return;
                }
                // 値を取得
                object objVal = rowDic[colVal];
                if (objVal == null)
                {
                    // 値が無い場合終了
                    return;
                }
                // 末尾にnを付与する。付与する文字は何でも良さそうだが、nを付与することでJavaScriptの仕様でBigIntとして扱うので影響が少ないと判断。
                string value = objVal.ToString();
                rowDic[colVal] = value + "n";
            }
        }

        /// <summary>
        /// 文字列をNullの場合は空文字にする処理
        /// </summary>
        /// <param name="target"></param>
        protected string ConvertNullToStringEmpty(string target)
        {
            // 対象文字列がNullでない かつ、空文字を除いた長さが0の場合は空文字とする
            if (target == null || target.Trim().Length == 0)
            {
                return string.Empty;
            }

            return target;
        }
    }
}
