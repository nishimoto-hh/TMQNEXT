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
using Dao = BusinessLogic_SP0001.BusinessLogicDataClass_SP0001;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_SP0001
{
    /// <summary>
    /// 予備品検索
    /// </summary>
    public class BusinessLogic_SP0001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"SelectParts";
            /// <summary>
            /// 一覧検索SQL
            /// </summary>
            public const string GetPartsList = "GetPartsList";
        }

        /// <summary>
        /// コントロールID
        /// </summary>
        public static class ControlId
        {
            /// <summary>
            /// 検索条件(予備品名～)
            /// </summary>
            public const string Condition = "CCOND_000_00_LST_0_SP0001";
            /// <summary>
            /// 検索結果一覧
            /// </summary>
            public const string List = "CBODY_020_00_LST_0_SP0001";

        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_SP0001() : base()
        {
        }
        #endregion

        #region オーバーライドメソッド
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int InitImpl()
        {
            //var machineId = GetGlobalData("SP0001_ConditionMachineId");
            //if (machineId != null)
            //{
            //    // ページ情報取得
            //    //var pageInfo = GetPageInfo(ControlId.Condition, this.pageInfoList);
            //    //Dao.searchCondition param = new Dao.searchCondition();
            //    //param.MachineId = long.Parse((string)machineId);

            //    //if (!SetFormByDataClass(ControlId.Condition, new List<Dao.searchCondition> { param }))
            //    //{
            //    //    // エラーの場合
            //    //    this.Status = CommonProcReturn.ProcStatus.Error;
            //    //    return ComConsts.RETURN_RESULT.NG;
            //    //}

            //}
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            // 一覧検索
            if (!searchList())
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ExecuteImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ControlId.List, this.pageInfoList);

            // 検索条件の取得
            Dao.searchCondition condition = getSearchInfo<Dao.searchCondition>();
            var machineId = GetGlobalData("SP0001_ConditionMachineId");
            if (machineId != null)
            {
                condition.MachineId = long.Parse((string)machineId);
            }
            condition.LanguageId = this.LanguageId;

            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = ComUtil.GetNotNullNameByClass<Dao.searchCondition>(condition);

            // SQLを取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetPartsList, out string withSql);
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetPartsList, out string baseSql, listUnComment);
            // 件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, string.Empty, withSql);

            // 総件数を取得
            int cnt = db.GetCount(executeSql.ToString(), condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY parts_name, standard_size, maker");

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results, results.Count))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNo">取得するグループ</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getSearchInfo<T>()
            where T : CommonDataBaseClass.SearchCommonClass, new()
        {
            // 検索条件エリアのコントロールID
            string conditionCtrlId = ControlId.Condition;

            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, conditionCtrlId);
            List<Dictionary<string, object>> conditionList = new() { condition };
            T searchInfo = new();

            // ページ情報取得
            var pageInfo = GetPageInfo(conditionCtrlId, this.pageInfoList);
            // 検索条件データの取得
            if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, conditionCtrlId, searchInfo, pageInfo))
            {
                // エラーの場合終了
                return searchInfo;
            }

            return searchInfo;
        }
        #endregion

    }
}