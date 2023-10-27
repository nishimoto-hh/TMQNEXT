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
using Dao = BusinessLogic_HM0003.BusinessLogicDataClass_HM0003;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_HM0003
{
    /// <summary>
    /// 変更管理 申請状況変更
    /// </summary>
    public class BusinessLogic_HM0003 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            /// <summary>
            /// 理由入力エリアのコントロールID
            /// </summary>
            public const string CtrlId = "CBODY_000_00_LST_0";

            public static class Button
            {
                /// <summary>
                /// 承認依頼
                /// </summary>
                public const string Request = "Request";
                /// <summary>
                /// 否認
                /// </summary>
                public const string Denial = "Denial";
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_HM0003() : base()
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

            //遷移元から受け取ったパラメータを取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.CtrlId);
            Dao.searchCondition param = new Dao.searchCondition();
            SetDataClassFromDictionary(targetDic, TargetCtrlId.CtrlId, param);

            //変更管理の情報を取得
            ComDao.HmHistoryManagementEntity entity = new ComDao.HmHistoryManagementEntity().GetEntity(param.HistoryManagementId, this.db);
            param.ApplicationReason = entity.ApplicationReason;
            param.RejectionReason = entity.RejectionReason;
            param.UpdateSerialid = entity.UpdateSerialid;

            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.CtrlId, this.pageInfoList);
            // 値設定
            if (!SetSearchResultsByDataClass<Dao.searchCondition>(pageInfo, new List<Dao.searchCondition> { param }, 1))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ExecuteImpl()
        {
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsStartId(TargetCtrlId.Button.Request) || compareId.IsStartId(TargetCtrlId.Button.Denial))
            {
                return Regist();
            }
            // この部分は到達不能なので、エラーを返す
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            // 登録処理戻り値、エラーならFalse
            bool resultRegist = updateHistoryManagement();

            // 登録処理結果によりエラー処理を行う
            if (!resultRegist)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 未設定時にエラーメッセージを設定
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「登録処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                }
                return ComConsts.RETURN_RESULT.NG;
            }
            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「登録処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200003 });

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
        /// 変更管理更新処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool updateHistoryManagement()
        {
            // 排他チェック
            if (!checkExclusiveSingle(TargetCtrlId.CtrlId))
            {
                return false;
            }

            //システム日時
            DateTime now = DateTime.Now;

            // 画面情報取得
            Dao.searchCondition result = GetFormDataByCtrlId<Dao.searchCondition>(TargetCtrlId.CtrlId);

            ComDao.HmHistoryManagementEntity condition = new();
            condition.HistoryManagementId = result.HistoryManagementId;
            if (result.RequestFlg)
            {
                //承認依頼の場合
                condition.ApplicationReason = result.ApplicationReason;
            }
            else
            {
                //否認の場合
                condition.RejectionReason = result.RejectionReason;
            }
            //変更管理の更新
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.None);
            return historyManagement.UpdateApplicationStatus(condition, result.RequestFlg ? TMQConst.MsStructure.StructureId.ApplicationStatus.Request : TMQConst.MsStructure.StructureId.ApplicationStatus.Return);
        }

        #endregion
    }
}