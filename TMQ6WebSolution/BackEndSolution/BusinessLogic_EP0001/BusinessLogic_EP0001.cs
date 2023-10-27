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
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using Dao = BusinessLogic_EP0001.BusinessLogicDataClass_EP0001;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace BusinessLogic_EP0001
{
    /// <summary>
    /// ExcelPortダウンロード
    /// </summary>
    public class BusinessLogic_EP0001 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"ExcelPort";

            /// <summary>SQL名：一覧取得()</summary>
            public const string GetExcelPortDownLoadList = "GetExcelPortDownLoadList";
        }

        /// <summary>
        /// フォーム、グループ、コントロールの親子関係を表現した場合の定数クラス
        /// </summary>
        private static class ConductInfo
        {
            /// <summary>
            /// 一覧画面
            /// </summary>
            public static class FormList
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 0;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// ExcelPortダウンロード一覧
                    /// </summary>
                    public const string List = "BODY_000_00_LST_0";

                }
            }
            /// <summary>
            /// ダウンロード条件画面
            /// </summary>
            public static class FormCondition
            {
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// ExcelPortダウンロード条件一覧
                    /// </summary>
                    public const string ConditonList = "BODY_000_00_LST_1";
                }

            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_EP0001() : base()
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
            // 初期表示
            if (this.FormNo == 0)
            {
            	// 一覧画面
                searchList();
            }
            else if (this.FormNo == 1)
            {
            	// 条件画面
                searchListCondition();
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            if (this.FormNo == 0)
            {
            	// 一覧画面
                searchList();
            }
            else if (this.FormNo == 1)
            {
            	// 条件画面
                searchListCondition();
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
        /// <param name="transaction">トランザクション</param>
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

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int UploadImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// ExcelPortダウンロード一覧初期検索
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetExcelPortDownLoadList, out string baseSql);
            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetExcelPortDownLoadList, out string withSql);

            //SQLパラメータに条件設定
            Dao.searchCondition searchParam = new();
            searchParam.LanguageId = this.LanguageId;　// 言語ID
            searchParam.UserId = this.UserId;          // ユーザーID

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(withSql + baseSql, searchParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results, results.Count))
            {
                return false;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;

            return true;
        }

        /// <summary>
        /// ExcelPortダウンロード条件初期検索
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchListCondition()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormCondition.ControlId.ConditonList, this.pageInfoList);

            Dao.searchResult registInfoParent = getRegistInfo<Dao.searchResult>(ConductInfo.FormList.ControlId.List, DateTime.Now);

            // データクラス定義
            Dao.searchListCondition registInfo = new();

            // searchConditionDictionaryに入っている選択された行のデータを入れる
            registInfo.ConductName = registInfoParent.ConductName;
            registInfo.HideConductId = registInfoParent.HideConductId;
            registInfo.HideSheetNo = registInfoParent.HideSheetNo;
            registInfo.AddCondition = registInfoParent.AddCondition;

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.searchListCondition>(pageInfo, new List<Dao.searchListCondition>() { registInfo }, 1))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="ctrlId">取得するコントロールID</param>
        /// <param name="now">システム日時</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(string ctrlId, DateTime now, string userId = "-1")
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // コントロールIDにより画面の項目(一覧)を取得
            Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);

            // TODO:ユーザIDを数値に変換するのは共通化
            T resultInfo = new();
            if (!SetExecuteConditionByDataClass<T>(result, ctrlId, resultInfo, now, this.UserId, userId))
            {
                // エラーの場合終了
                return resultInfo;
            }
            return resultInfo;
        }
    }
    #endregion

    #region publicメソッド
    #endregion
}