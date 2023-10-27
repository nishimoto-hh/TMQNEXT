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
using Dao = BusinessLogic_RM0002.BusinessLogicDataClass_RM0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

/// <summary>
/// 機能のテンプレート、実装サンプル
/// </summary>
namespace BusinessLogic_RM0002
{
    /// <summary>
    /// 機能のテンプレート、実装サンプル
    /// </summary>
    public partial class BusinessLogic_RM0002 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数
        /// <summary>
        /// フォーム種類
        /// </summary>
        private static class FormType
        {
            /// <summary>一覧画面</summary>
            public const byte List = 0;
            /// <summary>詳細子画面</summary>
            public const byte Detail = 1;
            /// <summary>編集モーダル画面</summary>
            public const byte Edit = 2;
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            // 機能内で不要なSQLは定義は不要です

            /// <summary>SQL名：一覧取得(FROMもSELECTも無し)</summary>
            public const string GetList = "GetReportManageList";
            /// <summary>SQL名：出力一覧取得</summary>
            public const string GetListForReport = "Select_ReportManageListForReport";
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Report";
        }

        /// <summary>
        /// テンプレート名称
        /// </summary>
        private static class TemplateName
        {
            /// <summary>テンプレート名：Excel出力</summary>
            public const string Report = "template_[機能ID].xlsx";

        }

        /// <summary>
        /// フォーム、グループ、コントロールの親子関係を表現した場合の定数クラス(サンプル)
        /// </summary>
        /// <remarks>画面の規模が大きくなるとどのコントロールがどのフォームか分からない、特に改修時。どのように定義するのが分かりやすいか、良いアイデア募集中です。</remarks>
        private static class ConductInfo
        {
            public static class FormList
            {
                /// <summary>
                /// 検索条件
                /// </summary>
                public const string Condition = "BODY_000_00_LST_0";
                /// <summary>
                /// 一覧
                /// </summary>
                public const string List = "BODY_020_00_LST_0";
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_RM0002() : base()
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
            this.ResultList = new();

            // 初期化処理は何もしない
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            switch (this.FormNo)
            {
                case FormType.List:     // 一覧検索
                    if (!searchList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                default:
                    // 処理が想定される場合は、分岐に条件を追加して処理を記載すること
                    // この部分は到達不能なので、エラーを返す
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
            return ComConsts.RETURN_RESULT.NG;
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
            int result = 0;
            return result;

        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int UploadImpl()
        {
            int result = 0;
            return result;
        }

        #endregion

        #region privateメソッド
        #endregion

        #region publicメソッド
        #endregion

    }
}