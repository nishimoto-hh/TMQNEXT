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
using Dao = BusinessLogic_SI0001.BusinessLogicDataClass_SI0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_SI0001
{
    /// <summary>
    /// アイテム検索
    /// </summary>
    public class BusinessLogic_SI0001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"SelectItem";
            /// <summary>
            /// 一覧検索SQL
            /// </summary>
            public const string GetItemList = "GetItemList";
        }

        /// <summary>
        /// コントロールID
        /// </summary>
        public static class ControlId
        {
            /// <summary>
            /// 検索条件(構成グループID、工場ID)
            /// </summary>
            public const string Search = "InitCondition";
            /// <summary>
            /// 検索結果一覧
            /// </summary>
            public const string List = "CBODY_010_00_LST_0_SI0001";

        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_SI0001() : base()
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
            // 初期検索実行
            return InitSearch();
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
            Dao.searchCondition condition = getSearchInfo();
            condition.LanguageId = this.LanguageId; // 言語ID

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetItemList, out string baseSql);
            // 件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, string.Empty, string.Empty);

            // 総件数を取得
            int cnt = db.GetCount(executeSql.ToString(), condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, string.Empty);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY");
            selectSql.AppendLine(" IIF(display_order IS NOT NULL, 0, 1)");
            selectSql.AppendLine(" , display_order");
            selectSql.AppendLine(" , structure_id");

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
        /// 検索条件取得
        /// </summary>
        /// <returns>検索条件のデータクラス</returns>
        private Dao.searchCondition getSearchInfo()
        {
            Dao.searchCondition searchInfo = new();

            // 検索条件のコントロールID
            string conditionCtrlId = ControlId.Search;

            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, conditionCtrlId);

            // 構成グループID
            searchInfo.StructureGroupId = Convert.ToInt32(condition["STRUCTUREGROUPID"]);
            // 工場ID
            searchInfo.FactoryId = Convert.ToInt32(condition["FACTORYID"]);

            return searchInfo;
        }
        #endregion

    }
}