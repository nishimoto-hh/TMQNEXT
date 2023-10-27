using CommonWebTemplate.Models.Common;
using CommonSTDUtil.CommonBusinessLogic;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComRes = CommonSTDUtil.CommonResources;
using Dao = BusinessLogic_CM00001.BusinessLogicDataClass_CM00001;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BusinessLogic_CM00001
{
    public class BusinessLogic_CM00001 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数
        /// <summary>
        /// 機能のコントロール情報
        /// </summary>
        private static class ConductInfo
        {
            /// <summary>
            /// トップ画面
            /// </summary>
            public static class FormTop
            {
                /// <summary>申請件数</summary>
                public const string ApplicationCount = "ApplicationHistory";
                /// <summary>申請件数</summary>
                public const string ApprovalCount = "ApprovalHistory";
            }
        }
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = "TOP";

            /// <summary>
            /// 変更管理情報表示用
            /// </summary>
            public static class History
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\HistoryManagement";

                /// <summary>ユーザの所属工場の承認ユーザを取得</summary>
                public const string GetApprovalUserByLogin = "GetApprovalUserByLogin";
                /// <summary>ユーザの申請している変更管理の件数を取得</summary>
                public const string GetApplicationCount = "GetApplicationCount";
                /// <summary>ユーザの承認待ちの変更管理の件数を取得</summary>
                public const string GetApprovalCount = "GetApprovalCount";
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_CM00001() : base()
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
        /// <returns></returns>
        protected override int SearchImpl()
        {
            this.JsonResult = string.Empty;

            if (searchTop())
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
                return ComConsts.RETURN_RESULT.OK;
            }

            // 異常終了
            this.Status = CommonProcReturn.ProcStatus.Error;
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns></returns>
        protected override int ExecuteImpl()
        {
            // 到達不能
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns></returns>
        protected override int RegistImpl()
        {
            // 到達不能
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns></returns>
        protected override int DeleteImpl()
        {
            // 到達不能
            return ComConsts.RETURN_RESULT.NG;
        }

        #endregion

        #region privateメソッド
        /// <summary>
        /// TOP画面の表示
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchTop()
        {
            // 変更管理処理クラス
            TMQUtil.HistoryManagement historyClass = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.None);
            // 検索パラメータ
            ComDao.MsUserEntity userParam = new() { UserId = int.Parse(this.UserId) };

            // ユーザがシステム管理者の場合、終了
            if (isUserAdmin())
            {
                // 申請中、承認待ち件数はいずれも表示しない
                setDisplayCount(false, false);
                return true;
            }

            // ユーザの所属工場に変更管理工場が無い場合、終了
            if (isFactoryNoHistoryManagement(out List<string> approvalUserList))
            {
                // 申請中、承認待ち件数はいずれも表示しない
                setDisplayCount(false, false);
                return true;
            }
            // ユーザの所属工場に変更管理工場がある場合、申請中件数は必ず表示する

            // ユーザが承認者であるか判定
            bool isDisplayApproval = isUserEqualApproval(approvalUserList);

            // 申請中件数と承認中件数の表示
            setDisplayCount(true, isDisplayApproval);

            return true;

            // ユーザがシステム管理者かどうか判定
            bool isUserAdmin()
            {
                // ユーザがシステム管理者かどうか判定
                bool isAdmin = historyClass.isSystemAdministrator();

                if (isAdmin)
                {
                    // システム管理者の場合、以降の処理を行わない
                    return true;
                }
                return false;
            }

            // ユーザの所属工場が変更管理工場かどうか判定し、工場の承認ユーザリストを取得
            bool isFactoryNoHistoryManagement(out List<string> approvalUserList)
            {
                // ユーザが所属する工場の承認ユーザ(拡張項目4)を取得
                approvalUserList = TMQUtil.SqlExecuteClass.SelectList<String>
                    (SqlName.History.GetApprovalUserByLogin, SqlName.History.SubDir, userParam, this.db);
                // 拡張項目のレコードが無い場合
                if (approvalUserList == null || approvalUserList.Count == 0)
                {
                    // 所属の工場に変更管理が無い場合、以降の処理を行わない
                    return true;
                }

                // ユーザが設定されているもののみを取得
                approvalUserList = approvalUserList.Where(x => !string.IsNullOrEmpty(x)).ToList();
                // 拡張項目の値が全て空の場合
                if (approvalUserList == null || approvalUserList.Count == 0)
                {
                    // 所属の工場に変更管理が無い場合、以降の処理を行わない
                    return true;
                }

                return false;
            }

            // 工場の承認ユーザリストにログインユーザが含まれているか判定
            bool isUserEqualApproval(List<string> approvalUserList)
            {
                // 承認ユーザリストにユーザと同じIDがあれば、承認ユーザである
                bool isApprovalUser = approvalUserList.Any(x => x.Equals(this.UserId));
                return isApprovalUser;
            }

            // 申請中件数と承認中件数をそれぞれ表示
            void setDisplayCount(bool isDisplayApplication, bool isDisplayApproval)
            {
                // 申請中件数(初期値は非表示)
                Dao.ApplicationCount application = Dao.ApplicationCount.GetNoDisplay();
                if (isDisplayApplication)
                {
                    application = TMQUtil.SqlExecuteClass.SelectEntity<Dao.ApplicationCount>(SqlName.History.GetApplicationCount, SqlName.History.SubDir, userParam, this.db);
                }
                // 承認中件数(初期値は非表示)
                Dao.AprovalCount approval = Dao.AprovalCount.GetNoDisplay();
                if (isDisplayApproval)
                {
                    approval = TMQUtil.SqlExecuteClass.SelectEntity<Dao.AprovalCount>(SqlName.History.GetApprovalCount, SqlName.History.SubDir, userParam, this.db);
                }
                // 画面に設定
                setForm(ConductInfo.FormTop.ApplicationCount, application);
                setForm(ConductInfo.FormTop.ApprovalCount, approval);

                void setForm<T>(string ctrlId, T result)
                {
                    var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);
                    SetSearchResultsByDataClass(pageInfo, new List<T> { result }, 1);
                }
            }
        }
        #endregion
    }
}