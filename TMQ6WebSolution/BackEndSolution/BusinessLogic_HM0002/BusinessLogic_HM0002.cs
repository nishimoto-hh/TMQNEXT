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
using Dao = BusinessLogic_HM0002.BusinessLogicDataClass_HM0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ScheduleStatus = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleStatus;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using SchedulePlanContent = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.SchedulePlanContent;
using FunctionTypeId = CommonTMQUtil.CommonTMQConstants.Attachment.FunctionTypeId;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;

namespace BusinessLogic_HM0002
{
    /// <summary>
    /// 変更管理 長期計画
    /// </summary>
    public partial class BusinessLogic_HM0002 : CommonBusinessLogicBase
    {

        #region 定数
        /// <summary>GetDetailWithのアンコメント用文字列</summary>
        private const string UnCommentWordOfGetDetailWith = "UnComp";
        /// <summary>GetListのアンコメント用文字列</summary>
        private const string UnCommentWordOfGetList = "UnExcelPort";
        /// <summary>変更管理ID キー文字列</summary>
        private const string HistoryManagementId = "HistoryManagementId";
        /// <summary>長期計画ID キー文字列</summary>
        private const string LongPlanId = "LongPlanId";

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"HistoryLongPlan";
            /// <summary>LN0001件名別長期計画のSQL格納先サブディレクトリ名</summary>
            public const string SubDirLongPlan = @"LongPlan";

            /// <summary>
            /// 共通
            /// </summary>
            public static class Common
            {
                /// <summary>長期計画更新</summary>
                public const string UpdateLongPlan = "UpdateLongPlan";
            }

            /// <summary>
            /// HM0001機器台帳変更管理
            /// </summary>
            public static class HM0001
            {
                /// <summary>HM0001機器台帳変更管理のSQL格納先サブディレクトリ名</summary>
                public const string SubDir = @"HistoryMachine";
                /// <summary>機器別管理基準内容変更管理の登録</summary>
                public const string InsertManagementStandardsContentInfo = "InsertManagementStandardsContentInfo";
            }

            /// <summary>
            /// 一覧画面で使用するSQL
            /// </summary>
            public static class List
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\List";
                /// <summary>LN0001件名別長期計画のSQL格納先サブディレクトリ名</summary>
                public const string SubDirLongPlan = SqlName.SubDirLongPlan + @"\List";

                /// <summary>件数取得SQL</summary>
                public const string GetCountHistoryManagementList = "GetCountHistoryManagementList";
                /// <summary>一覧情報取得SQL</summary>
                public const string GetHistoryList = "GetHistoryLongPlanList";
                /// <summary>一覧スケジュール情報取得SQL</summary>
                public const string GetHistoryListSchedule = "GetHistoryLongPlanSchedule";
                /// <summary>変更管理詳細情報取得SQL</summary>
                public const string GetHistoryManagementDetail = "GetHistoryManagementDetail";
                /// <summary>長計件名登録SQL（件名IDは採番しない）</summary>
                public const string InsertLongPlan = "InsertLongPlan";
                /// <summary>長計件名変更管理取得SQL</summary>
                public const string GetHmLongPlan = "GetHmLongPlan";
                /// <summary>保全活動件名更新SQL</summary>
                public const string UpdateSummary = "UpdateSummary";

                /// <summary>一覧情報取得SQL(LN0001件名別長期計画)</summary>
                public const string GetList = "GetLongPlanList";

                /// <summary>一覧情報取得の一時テーブル作成SQL</summary>
                public const string CreateTempTable = "CreateTableTempGetLongPlanList";
                /// <summary>一覧情報取得の一時テーブル登録SQL</summary>
                public const string InsertTempTable = "InsertTempGetLongPlanList";
            }

            /// <summary>
            /// 参照画面
            /// </summary>
            public static class Detail
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\Detail";
                /// <summary>LN0001件名別長期計画のSQL格納先サブディレクトリ名</summary>
                public const string SubDirLongPlan = SqlName.SubDirLongPlan + @"\Detail";

                /// <summary>行削除(紐づいた長計件名IDをクリア)</summary>
                public const string DeleteRow = "UpdateContentForDeleteRow";
                /// <summary>長計件名IDから紐づく添付情報を取得</summary>
                public const string GetAttachmentByLongPlanId = "GetAttachmentByLongPlanId";
                /// <summary>長計件名IDが紐づく機器別管理基準に保全活動が作成されているか確認</summary>
                public const string GetCountScheduledContent = "GetCountScheduledContent";
                /// <summary>削除ボタン(紐づいた長計件名IDをクリア)</summary>
                public const string DeleteAll = "UpdataContentForDeleteAll";

                /// <summary>保全情報一覧取得SQLの名前共通部分</summary>
                private const string GetDetail = "GetDetailList";
                /// <summary>保全情報一覧取得(With句)</summary>
                public const string GetDetailWith = GetDetail + "_With";
                /// <summary>保全情報一覧取得(機器)</summary>
                public const string GetDetailEquipment = GetDetail + "_Equipment";
                /// <summary>保全情報一覧取得(部位)</summary>
                public const string GetDetailInspectionSite = GetDetail + "_InspectionSite";
                /// <summary>保全情報一覧取得(保全項目)</summary>
                public const string GetDetailMaintainance = GetDetail + "_Maintainance";

                /// <summary>保全情報のスケジュール一覧取得SQLの名前共通部分</summary>
                private const string GetSchedule = "GetScheduleList";
                /// <summary>保全情報のスケジュール一覧取得(機器)</summary>
                public const string GetScheduleEquipment = GetSchedule + "_Equipment";
                /// <summary>保全情報のスケジュール一覧取得(部位)</summary>
                public const string GetScheduleInspectionSite = GetSchedule + "_InspectionSite";
                /// <summary>保全情報のスケジュール一覧取得(保全項目)</summary>
                public const string GetScheduleMaintainance = GetSchedule + "_Maintainance";

                /// <summary>行削除入力チェック：機器別管理基準内容に紐づく保全活動作成済みスケジュールの件数を取得</summary>
                public const string GetSummaryScheduleByContent = "GetCountSummaryScheduleByContent";

                /// <summary>長期計画IDより紐づく機器の点検種別管理を取得するSQL</summary>
                public const string GetMaintKindManageByLongPlanId = "GetMaintainanceKindManageByLongPlanId";

                /// <summary>長計件名変更管理を登録するSQL</summary>
                public const string InsertHmLnLongPlan = "InsertHmLnLongPlan";
                /// <summary>長計件名変更管理を削除するSQL</summary>
                public const string DeleteHmLnLongPlan = "DeleteHmLnLongPlan";
                /// <summary>機器別管理基準内容変更管理を削除するSQL</summary>
                public const string DeleteHmContent = "DeleteHmContent";
            }

            /// <summary>
            /// 編集画面
            /// </summary>
            public static class Edit
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\Edit";

                /// <summary>長期計画件名ID取得SQL</summary>
                public const string GetNewLongPlanId = "GetNewLongPlanId";
                /// <summary>長計件名変更管理を更新するSQL</summary>
                public const string UpdateHmLongPlan = "UpdateHmLongPlan";
            }

            /// <summary>
            /// 入力チェック
            /// </summary>
            public static class InputCheck
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\InputCheck";
                /// <summary>LN0001件名別長期計画のSQL格納先サブディレクトリ名</summary>
                public const string SubDirLongPlan = SqlName.SubDirLongPlan + @"\InputCheck";
                // 点検種別毎管理の統一チェック
                /// <summary>長期計画IDに紐づく機番ID取得</summary>
                public const string GetMachineId = "GetMachineIdFromLongPlanId";
                /// <summary>機番IDの点検種別毎管理の件数を取得</summary>
                public const string GetMaintKindManageCount = "GetMaintainanceKindManageCount";
                // 点検種別ごとの周期の統一チェック
                /// <summary>長期計画IDに紐づく件名別長期計画の情報を取得</summary>
                public const string GetContentInfo = "GetContentInfoFromLongPlanId";
                /// <summary>機器別管理基準内容IDに紐づくスケジュール情報を取得</summary>
                public const string GetScheduleInfo = "GetScheduleInfoFromContentId";
            }

            /// <summary>
            /// 機器別管理基準選択画面
            /// </summary>
            public static class SelectStandards
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\SelectStandards";
                /// <summary>LN0001件名別長期計画のSQL格納先サブディレクトリ名</summary>
                public const string SubDirLongPlan = SqlName.SubDirLongPlan + @"\SelectStandards";
                /// <summary>一覧取得</summary>
                public const string GetList = "GetManagementStandardsContentList";
                /// <summary>登録処理　機器別管理基準内容更新</summary>
                public const string UpdateContent = "UpdateManagementStandardsContent";
            }
        }

        /// <summary>
        /// 機能のコントロール情報
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
                    /// 一覧
                    /// </summary>
                    public const string List = "BODY_040_00_LST_0";
                    /// <summary>
                    /// スケジュール表示条件
                    /// </summary>
                    public const string ScheduleCondition = "BODY_020_00_LST_0";
                    /// <summary>
                    /// 検索条件(自分の申請のみ表示)
                    /// </summary>
                    public const string MySubjectCondition = "BODY_050_00_LST_0";
                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {
                    /// <summary>
                    /// 新規申請
                    /// </summary>
                    public const string New = "New";
                    /// <summary>
                    /// 一括承認
                    /// </summary>
                    public const string ApprovalAll = "ApprovalAll";
                    /// <summary>
                    /// 一括否認
                    /// </summary>
                    public const string DenialAll = "DenialAll";
                }
            }
            /// <summary>
            /// 参照画面
            /// </summary>
            public class FormDetail
            {
                /// <summary>フォーム番号</summary>
                public const short FormNo = 1;
                /// <summary>ヘッダのグループ番号</summary>
                public const short HeaderGroupNo = 201;
                /// <summary>コントロールID</summary>
                public static class ControlId
                {
                    /// <summary>非表示</summary>
                    public const string Hide = "BODY_100_00_LST_1";

                    /// <summary>保全情報一覧</summary>
                    public const string List = "BODY_070_00_LST_1";
                    /// <summary>点検種別毎保全情報一覧</summary>
                    public const string ListCheck = "BODY_080_00_LST_1";

                    /// <summary>保全情報一覧</summary>
                    public const string Condition = "BODY_050_00_LST_1";

                    /// <summary>
                    /// スケジュール表示条件
                    /// </summary>
                    public const string ScheduleCondition = "BODY_050_00_LST_1";
                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {
                    /// <summary>
                    /// 複写申請
                    /// </summary>
                    public const string CopyRequest = "CopyRequest";
                    /// <summary>
                    /// 変更申請
                    /// </summary>
                    public const string ChangeRequest = "ChangeRequest";
                    /// <summary>
                    /// 削除申請
                    /// </summary>
                    public const string DeleteRequest = "DeleteRequest";
                    /// <summary>
                    /// 承認依頼
                    /// </summary>
                    public const string ChangeApplicationRequest = "ChangeApplicationRequest";
                    /// <summary>
                    /// 申請内容修正
                    /// </summary>
                    public const string EditRequest = "EditRequest";
                    /// <summary>
                    /// 申請内容取消
                    /// </summary>
                    public const string CancelRequest = "CancelRequest";
                    /// <summary>
                    /// 承認依頼引戻
                    /// </summary>
                    public const string PullBackRequest = "PullBackRequest";
                    /// <summary>
                    /// 承認
                    /// </summary>
                    public const string ChangeApplicationApproval = "ChangeApplicationApproval";
                    /// <summary>
                    /// 否認
                    /// </summary>
                    public const string ChangeApplicationDenial = "ChangeApplicationDenial";
                    /// <summary>
                    /// 行削除
                    /// </summary>
                    public const string Delete = "Delete";
                }
            }
            /// <summary>
            /// 詳細編集画面
            /// </summary>
            public class FormEdit
            {
                /// <summary>フォーム番号</summary>
                public const short FormNo = 2;
                /// <summary>ヘッダのグループ番号</summary>
                public const short HeaderGroupNo = 301;
                /// <summary>コントロールID</summary>
                public static class ControlId
                {
                    /// <summary>非表示</summary>
                    public const string Hide = "BODY_050_00_LST_2";
                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {
                    /// <summary>
                    /// 登録
                    /// </summary>
                    public const string Regist = "Regist";
                }
            }

            /// <summary>
            /// 機器別管理基準選択
            /// </summary>
            public class FormSelectStandards
            {
                /// <summary>フォーム番号</summary>
                public const short FormNo = 3;

                /// <summary>検索条件のグループ番号</summary>
                public const short SearchConditionGroupNo = 501;

                /// <summary>コントロールID</summary>
                public static class ControlId
                {
                    /// <summary>機器選択一覧</summary>
                    public const string List = "BODY_040_00_LST_3";
                    /// <summary>非表示項目</summary>
                    public const string Hide = "BODY_060_00_LST_3";
                    /// <summary>検索条件：場所</summary>
                    public const string Location = "COND_000_00_LST_3";
                    /// <summary>検索条件：職種機種</summary>
                    public const string Job = "COND_010_00_LST_3";

                }
            }
        }

        /// <summary>
        /// スケジュールより遷移する保全活動画面の情報
        /// </summary>
        private static class MA0001
        {
            /// <summary>機能ID</summary>
            public const string ConductId = "MA0001";
            /// <summary>フォームNo</summary>
            public static class FormNo
            {
                /// <summary>参照画面</summary>
                public const int Detail = 1;
                /// <summary>新規登録</summary>
                public const int New = 0;
            }
            /// <summary>タブなしの場合のタブNo</summary>
            public const int TabNoNone = 0;

            /// <summary>タブNo(参照画面)</summary>
            public static class TabNoDetail
            {
                /// <summary>履歴タブ</summary>
                public const int History = 3;
                /// <summary>依頼タブ</summary>
                public const int Request = 1;
                /// <summary>参照画面でなく、新規登録</summary>
                public const int New = -1;
            }
        }

        /// <summary>
        /// 実行処理区分
        /// </summary>
        private static class ExecutionDivision
        {
            /// <summary>長期計画の新規登録（複写）</summary>
            public const int NewLongPlan = 1;
            /// <summary>長期計画の修正</summary>
            public const int UpdateLongPlan = 2;
            /// <summary>長期計画の削除</summary>
            public const int DeleteLongPlan = 3;
            /// <summary>保全情報一覧の追加</summary>
            public const int AddContent = 4;
            /// <summary>保全情報一覧の削除</summary>
            public const int DeleteContent = 5;
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_HM0002() : base()
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
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo:
                    // 一覧画面
                    return InitSearch();
                case ConductInfo.FormDetail.FormNo:
                    // 参照画面
                    // 詳細編集画面の新規登録後と初期表示
                    DetailDispType detailType = compareId.IsRegist() ? DetailDispType.AfterRegist : DetailDispType.Init;
                    if (compareId.IsBack())
                    {
                        // 戻るの場合
                        detailType = DetailDispType.Search;
                    }
                    if (!initDetail(detailType))
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormEdit.FormNo:
                    // 詳細編集画面
                    // どのボタンで起動したかを判定
                    // 新規(一覧画面より起動)
                    EditDispType editType = EditDispType.New;
                    if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.ChangeRequest) || compareId.IsStartId(ConductInfo.FormDetail.ButtonId.EditRequest))
                    {
                        // 変更申請、申請内容修正(参照画面より起動)
                        editType = EditDispType.Update;
                    }
                    if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.CopyRequest))
                    {
                        // 複写申請(参照画面より起動)
                        editType = EditDispType.Copy;
                    }
                    // 初期化処理
                    if (!initEdit(editType))
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormSelectStandards.FormNo:
                    // 機器別管理基準選択画面
                    initSelectStandards();
                    return ComConsts.RETURN_RESULT.OK;
                default:
                    return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo:
                    // 一覧画面
                    if (!searchList(compareId.IsInit()))
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormDetail.FormNo:
                    // 参照画面
                    if (!initDetail(DetailDispType.Search))
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormSelectStandards.FormNo:
                    // 機器別管理基準選択画面
                    if (!searchSelectStandards())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                default:
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
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            switch (this.CtrlId)
            {
                case ConductInfo.FormList.ButtonId.ApprovalAll: //一括承認
                case ConductInfo.FormList.ButtonId.DenialAll: //一括否認
                case ConductInfo.FormDetail.ButtonId.PullBackRequest: //承認依頼引戻
                case ConductInfo.FormDetail.ButtonId.ChangeApplicationApproval: //承認
                case ConductInfo.FormEdit.ButtonId.Regist: //登録(編集画面、機器別管理基準選択画面)
                    return Regist();
                case ConductInfo.FormDetail.ButtonId.DeleteRequest: //削除申請
                case ConductInfo.FormDetail.ButtonId.CancelRequest: //申請内容取消
                case ConductInfo.FormDetail.ButtonId.Delete: //行削除
                    return Delete();
                case "checkExistsHistory": //画面遷移前のチェック（「承認済」以外の変更管理が紐づいているかチェック）
                    return checkExistsHistory();
                case "checkExclusiveHistory": //画面遷移前のチェック（変更管理の排他チェック）
                    return checkExclusiveHistory();
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
            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse

            string processName = setProcessName(); // 処理名称

            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            // 処理を実行する画面Noの値により処理を分岐する
            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo: // 一覧画面
                    //一括承認、一括否認
                    resultRegist = registFormList();
                    break;
                case ConductInfo.FormDetail.FormNo: // 詳細画面
                    if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.PullBackRequest))
                    {
                        //承認依頼引戻
                        resultRegist = executePullBackRequest();
                    }
                    else if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.ChangeApplicationApproval))
                    {
                        //承認
                        resultRegist = executeApproval();
                    }
                    break;
                case ConductInfo.FormEdit.FormNo: // 編集画面
                    //登録
                    resultRegist = executeRegistEdit();
                    break;
                case ConductInfo.FormSelectStandards.FormNo: // 機器別管理基準選択画面
                    // 登録
                    resultRegist = executeRegistSelectStandards();
                    break;
                default:
                    // 処理が想定される場合は、分岐に条件を追加して処理を記載すること
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }

            // 登録処理結果によりエラー処理を行う
            if (!resultRegist)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 未設定時にエラーメッセージを設定
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「○○処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, processName });
                }
                return ComConsts.RETURN_RESULT.NG;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「○○処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, processName });

            return ComConsts.RETURN_RESULT.OK;

            string setProcessName()
            {
                if (this.CtrlId == ConductInfo.FormList.ButtonId.ApprovalAll)
                {
                    return ComRes.ID.ID111020051; // 一括承認
                }
                else if (this.CtrlId == ConductInfo.FormList.ButtonId.DenialAll)
                {
                    return ComRes.ID.ID111020052; // 一括否認
                }
                else if (this.CtrlId == ConductInfo.FormDetail.ButtonId.PullBackRequest)
                {
                    return ComRes.ID.ID111120233; // 承認依頼引戻
                }
                else if (this.CtrlId == ConductInfo.FormDetail.ButtonId.ChangeApplicationApproval)
                {
                    return ComRes.ID.ID111120228; // 承認
                }

                return ComRes.ID.ID911200003;
            }
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            this.ResultList = new();

            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse

            string processName = setProcessName(); // 処理名称

            // ボタンIDに応じて処理を分岐
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId);
            // 参照画面
            if (this.FormNo == ConductInfo.FormDetail.FormNo)
            {
                if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.DeleteRequest))
                {
                    // 削除申請ボタン
                    if (!deleteRequestDetailAll())
                    {
                        setError();
                        return ComConsts.RETURN_RESULT.NG;
                    }
                }
                else if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.CancelRequest))
                {
                    // 申請内容取消
                    if (!cancelRequest())
                    {
                        setError();
                        return ComConsts.RETURN_RESULT.NG;
                    }
                }
                else if (compareId.IsDelete())
                {
                    // 行削除
                    if (!deleteDetailRow())
                    {
                        setError();
                        return ComConsts.RETURN_RESULT.NG;
                    }
                }
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「○○処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, processName });

            return ComConsts.RETURN_RESULT.OK;

            void setError()
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 未設定時にエラーメッセージを設定
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「○○処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, processName });
                }
            }

            string setProcessName()
            {
                if (this.CtrlId == ConductInfo.FormDetail.ButtonId.DeleteRequest)
                {
                    return ComRes.ID.ID111110068; // 削除申請
                }
                else if (this.CtrlId == ConductInfo.FormDetail.ButtonId.CancelRequest)
                {
                    return ComRes.ID.ID111120232; // 申請内容取消
                }

                return ComRes.ID.ID911110001;
            }
        }
        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            // ファイルダウンロードの場合
            if (compareId.IsDownload())
            {
                // ダウンロード情報取得
                var info = TMQUtil.GetFileDownloadInfo(this.searchConditionDictionary, this.db, out bool isError);
                if (isError)
                {
                    // エラーの場合は終了
                    OutputFileDownloadError();
                    return ComConsts.RETURN_RESULT.NG;
                }
                // ファイルをダウンロード
                if (!OutputDownloadFile(info.FileName, info.FilePath))
                {
                    // エラーの場合は終了
                    return ComConsts.RETURN_RESULT.NG;
                }
                return ComConsts.RETURN_RESULT.OK;
            }
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
        #region スケジュール関連
        /// <summary>
        /// 画面のスケジュールよりスケジュールのSQL検索条件を取得する処理
        /// </summary>
        /// <param name="longPlanId">長期計画ID</param>
        /// <param name="factoryId">長期計画の工場ID</param>
        /// <param name="monthStartNendo">年度開始月</param>
        /// <returns>スケジュールのSQL検索条件</returns>
        private Dao.Schedule.SearchConditionLongPlanId getScheduleCondition(long longPlanId, int factoryId, int monthStartNendo, long? historyManagementId)
        {
            // 画面の条件を取得
            var scheduleCond = GetFormDataByCtrlId<TMQDao.ScheduleList.Condition>(ConductInfo.FormDetail.ControlId.ScheduleCondition, false);
            Dao.Schedule.SearchConditionLongPlanId cond = new(scheduleCond, longPlanId, monthStartNendo, this.LanguageId);
            cond.FactoryIdList = TMQUtil.GetFactoryIdList();
            cond.FactoryIdList.Add(factoryId);
            cond.HistoryManagementId = historyManagementId ?? -1;

            return cond;
        }

        /// <summary>
        /// 年度開始月を取得する処理
        /// </summary>
        /// <param name="factoryId">工場ID 省略時はユーザの本務工場</param>
        /// <returns>年度開始月</returns>
        private int getYearStartMonth(int? factoryId = null)
        {
            int startMonth;
            if (factoryId == null)
            {
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                startMonth = TMQUtil.GetYearStartMonth(this.db, userFactoryId);
            }
            else
            {
                startMonth = TMQUtil.GetYearStartMonth(this.db, factoryId ?? -1);
            }
            return startMonth;
        }

        /// <summary>
        /// 保全情報一覧に紐づけるスケジュールリストを取得する処理
        /// </summary>
        /// <param name="listCtrlId">一覧のコントロールID</param>
        /// <param name="isDetail">参照画面の場合True(仕様が異なる、リンク有)</param>
        /// <param name="longPlanId">長期計画ID</param>
        /// <param name="factoryId">長期計画IDの工場ID</param>
        /// <param name="processMode">処理モード</param>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <param name="schedulePlanContent">スケジュールの表示単位</param>
        /// <param name="isUnComp">未完了のデータを表示する場合True</param>
        private void setSchedule(string listCtrlId, bool isDetail, long longPlanId, int factoryId, int processMode, long? historyManagementId, SchedulePlanContent schedulePlanContent, bool isUnComp = false)
        {
            // 実行するSQL文を取得
            string fileNameSelect = string.Empty;
            //実行するSQL格納先ディレクトリ
            string subDir = SqlName.Detail.SubDirLongPlan;
            switch (schedulePlanContent)
            {
                case SchedulePlanContent.Equipment:
                    fileNameSelect = SqlName.Detail.GetScheduleEquipment;
                    //実行するSQL格納先ディレクトリ
                    if (processMode == (int)TMQConst.MsStructure.StructureId.ProcessMode.history)
                    {
                        subDir = SqlName.Detail.SubDir;
                    }
                    break;
                case SchedulePlanContent.InspectionSite:
                    fileNameSelect = SqlName.Detail.GetScheduleInspectionSite;
                    //実行するSQL格納先ディレクトリ
                    if (processMode == (int)TMQConst.MsStructure.StructureId.ProcessMode.history)
                    {
                        subDir = SqlName.Detail.SubDir;
                    }
                    break;
                case SchedulePlanContent.Maintainance:
                    fileNameSelect = SqlName.Detail.GetScheduleMaintainance;
                    break;
                default:
                    // 到達不能
                    return;
            }
            string withSql = getSqlGetDetailWith(isUnComp, processMode); // WITH句
            TMQUtil.GetFixedSqlStatement(subDir, fileNameSelect, out string selectSql); // SELECT句
            // 結合
            StringBuilder execSql = new(withSql);
            execSql.AppendLine(selectSql);

            // 年度開始月の取得
            int monthStartNendo = getYearStartMonth(factoryId);

            // 画面の条件を取得
            Dao.Schedule.SearchConditionLongPlanId cond = getScheduleCondition(longPlanId, factoryId, monthStartNendo, historyManagementId);

            // 個別実装用データへスケジュールのレイアウトデータ(scheduleLayout)をセット
            TMQUtil.ScheduleListUtil.SetLayout(ref this.IndividualDictionary, cond, false, getNendoText(), monthStartNendo);

            // 一時テーブル設定
            setTempTable();

            // 画面表示データの取得
            List<TMQDao.ScheduleList.Display> scheduleDisplayList;
            if (schedulePlanContent == SchedulePlanContent.Maintainance)
            {
                // 保全項目単位の場合、上位ランクのステータスを取得
                scheduleDisplayList = getScheduleDisplayList<TMQUtil.ScheduleListConverter>(execSql, cond);
            }
            else
            {
                // 保全項目単位以外(機器、部位)の場合、上位ランクのステータスは取得しない
                scheduleDisplayList = getScheduleDisplayList<TMQUtil.ScheduleListConverterNoRank>(execSql, cond);
            }

            // 画面設定用データに変換
            Dictionary<string, Dictionary<string, string>> setScheduleData = TMQUtil.ScheduleListUtil.ConvertDictionaryAddData(scheduleDisplayList, cond);

            // 画面に設定
            SetScheduleDataToResult(setScheduleData, listCtrlId);

            // 画面表示データの取得 SQLを実行し、実行結果を変換 変換するクラスが上位ランクの有無が異なるので分岐
            List<TMQDao.ScheduleList.Display> getScheduleDisplayList<T>(StringBuilder execSql, Dao.Schedule.SearchConditionLongPlanId scheduleCondition)
                where T : TMQUtil.ScheduleListConverter, new()
            {
                IList<TMQDao.ScheduleList.Get> scheduleList = db.GetListByDataClass<TMQDao.ScheduleList.Get>(execSql.ToString(), cond);
                // 画面表示用に変換
                T listSchedule = new();
                // リンク有無
                bool isLink = isDetail; // リンクは詳細画面のみ
                if (cond.IsYear())
                {
                    // リンクは月単位のみなので、年単位の場合リンクしない
                    isLink = false;
                }
                var scheduleDisplayList = listSchedule.Execute(scheduleList, cond, monthStartNendo, isLink, this.db, getScheduleLinkInfo());
                return scheduleDisplayList;
            }

            void setTempTable()
            {
                TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
                listPf.GetCreateTranslation();
                listPf.GetInsertTranslationAll(new List<GroupId> { GroupId.MaintainanceKind }, true);
                listPf.RegistTempTable();
            }
        }

        /// <summary>
        /// スケジュールのマークからのリンク先を設定する処理
        /// </summary>
        /// <returns>マークごとのリンク先の情報</returns>
        private Dictionary<ScheduleStatus, TMQDao.ScheduleList.Display.LinkTargetInfo> getScheduleLinkInfo()
        {
            Dictionary<ScheduleStatus, TMQDao.ScheduleList.Display.LinkTargetInfo> result = new();
            // ●：保全活動参照画面(履歴タブ)
            result.Add(ScheduleStatus.Complete, new(MA0001.ConductId, MA0001.FormNo.Detail, MA0001.TabNoDetail.History));
            // ◎：保全活動参照画面(依頼タブ)
            result.Add(ScheduleStatus.Created, new(MA0001.ConductId, MA0001.FormNo.Detail, MA0001.TabNoDetail.Request));
            // リンクなし
            // ○：保全活動新規登録画面
            //result.Add(ScheduleStatus.NoCreate, new(MA0001.ConductId, MA0001.FormNo.New, MA0001.TabNoDetail.New));
            // ▲：保全活動参照画面(履歴タブ)
            result.Add(ScheduleStatus.UpperComplete, new(MA0001.ConductId, MA0001.FormNo.Detail, MA0001.TabNoDetail.History));
            return result;
        }

        /// <summary>
        /// スケジュールレイアウト作成に必要な「年度」の文言を取得する処理
        /// </summary>
        /// <returns>"年度"に相当する文言</returns>
        private string getNendoText()
        {
            // 「{0}年度」
            return GetResMessage(ComRes.ID.ID150000013);
        }

        #endregion
        /// <summary>
        /// 遷移元の画面の一覧の内容よりキー項目の内容を取得する
        /// </summary>
        /// <param name="fromCtrlId">遷移元の一覧のコントロールID</param>
        /// <param name="isReSearch">再表示の場合True</param>
        /// <param name="setMessage">初期表示時のメッセージを設定する場合True</param>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <returns>取得したキー項目の内容、取得できない場合(新規)はNull</returns>
        private Dao.detailSearchCondition getParam(string fromCtrlId, bool isReSearch, bool setMessage, long historyManagementId = -1)
        {
            // 遷移元の一覧と検索条件の内容より、キー項目の値を取得する
            Dao.detailSearchCondition param = new();
            // 画面情報取得元、再表示の場合はSearchConditionDictionaryでなくResultInfoDictionary
            var targetDicList = !isReSearch ? this.searchConditionDictionary : this.resultInfoDictionary;
            var targetDic = ComUtil.GetDictionaryByCtrlId(targetDicList, fromCtrlId);
            SetDataClassFromDictionary(targetDic, fromCtrlId, param, new List<string> { HistoryManagementId, LongPlanId });
            param.LanguageId = this.LanguageId;    // 言語ID
            param.UserId = int.Parse(this.UserId); // ログインユーザID

            //LN0001件名別長期計画から遷移してきた場合、紐づく変更管理がある場合は変更管理の内容を表示する
            if (historyManagementId <= 0 && (param.HistoryManagementId == null || param.HistoryManagementId <= 0))
            {
                TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
                param.HistoryManagementId = historyManagement.getHistoryManagementIdByKeyId(param.LongPlanId);
            }

            // 変更管理IDの有無で処理モードを設定
            if (historyManagementId <= 0 && (param.HistoryManagementId == null || param.HistoryManagementId <= 0))
            {
                // 変更管理IDなし の場合、トランザクションモード
                param.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction;
                if (setMessage)
                {
                    //表示されている件名に対し行う変更を登録してください。
                    this.MsgId = GetResMessage(ComRes.ID.ID141270002);
                }
            }
            else
            {
                // 変更管理IDあり の場合、変更管理モード
                if (param.HistoryManagementId == null || param.HistoryManagementId <= 0)
                {
                    param.HistoryManagementId = historyManagementId;
                }
                param.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.history;
                if (setMessage)
                {
                    //登録された変更管理に対して処理を行います。
                    this.MsgId = GetResMessage(ComRes.ID.ID141200007);
                }
            }
            return param;
        }

        /// <summary>
        /// SQLの条件に長期計画IDを追加する
        /// </summary>
        /// <param name="sql">条件を追加するSQL</param>
        /// <returns>条件を追加したSQL</returns>
        private string addSqlWhereLongPlanId(string sql)
        {
            StringBuilder execSql = new StringBuilder(sql);
            execSql.AppendLine(" where target.long_plan_id = @LongPlanId");
            return execSql.ToString();
        }

        /// <summary>
        /// 指定された一覧に値を設定する
        /// </summary>
        /// <param name="param">長期計画の内容</param>
        /// <param name="toCtrlIds">値を設定する一覧のコントロールID</param>
        /// <returns>エラーの場合False</returns>
        private bool initFormByParam(Dao.ListSearchResult param, List<string> toCtrlIds)
        {
            // 一覧に対して繰り返し値を設定する
            foreach (var ctrlId in toCtrlIds)
            {
                if (!SetFormByDataClass(ctrlId, new List<Dao.ListSearchResult> { param }))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }

            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;
        }
        #region 一覧・参照画面関連
        /// <summary>
        /// 一覧取得SQLに必要な、一時テーブル登録処理
        /// </summary>
        private void setTempTableForGetList(long? longPlanId = null)
        {
            // 一覧の場合、長計件名IDがNull。詳細の場合、Nullでない。
            bool isList = longPlanId == null;

            // 一時テーブル関連処理
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
            listPf.GetAttachmentSql(new List<FunctionTypeId> { FunctionTypeId.Machine, FunctionTypeId.Equipment, FunctionTypeId.LongPlan });
            // 使用する構成グループ(点検種別はスケジュールで使用)
            var structuregroupList = new List<GroupId>
                    {
                        GroupId.Location, GroupId.Job, GroupId.Season, GroupId.BudgetPersonality, GroupId.Purpose, GroupId.WorkItem, GroupId.BudgetManagement,
                        GroupId.WorkClass, GroupId.Treatment, GroupId.Facility, GroupId.MaintainanceKind, GroupId.ApplicationStatus, GroupId.ApplicationDivision
                    };
            if (!isList)
            {
                // 詳細の場合、翻訳を追加
                structuregroupList.AddRange(new List<GroupId> { GroupId.Importance, GroupId.SiteMaster, GroupId.InspectionDetails, GroupId.ScheduleType });
            }
            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ

            // 機能個別
            // Create文
            string createSql = TMQUtil.SqlExecuteClass.GetExecuteSql(SqlName.List.CreateTempTable, SqlName.List.SubDir, string.Empty);
            // Insert文は一覧と詳細で異なる
            List<string> listUnComment = new();
            listUnComment.Add(isList ? "ForList" : "ForDetail");
            // Insert文
            string insertSql = TMQUtil.SqlExecuteClass.GetExecuteSql(SqlName.List.InsertTempTable, SqlName.List.SubDir, string.Empty, listUnComment);
            if (!isList)
            {
                // 詳細の場合、長計件名IDをSQLに設定
                insertSql = insertSql.Replace("@LongPlanId", longPlanId.ToString());
            }
            // 設定
            listPf.AddTempTableBySql(createSql, insertSql);

            listPf.RegistTempTable(); // 登録
        }

        /// <summary>
        /// 指定された長期計画の一覧の情報を取得する
        /// </summary>
        /// <param name="param">取得する長期計画の条件</param>
        /// <param name="isTree">省略可能 ツリービュー表示用の場合、True</param>
        /// <returns>取得した長期計画の情報(一覧画面の1レコード)</returns>
        private Dao.ListSearchResult getLongPlanInfo(Dao.detailSearchCondition param, bool isTree = false)
        {
            // 一時テーブル関連処理
            setTempTableForGetList(param.LongPlanId);

            // 表示するデータタイプに応じたSQLを取得
            StringBuilder execSql = new();
            // 処理モードを判定
            switch (param.ProcessMode)
            {
                case (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction: // トランザクションモード

                    // SQL取得
                    TMQUtil.GetFixedSqlStatementWith(SqlName.List.SubDirLongPlan, SqlName.List.GetList, out string withTraSql, new List<string> { UnCommentWordOfGetList });
                    TMQUtil.GetFixedSqlStatement(SqlName.List.SubDirLongPlan, SqlName.List.GetList, out string traSql, new List<string> { UnCommentWordOfGetList });
                    execSql.AppendLine(withTraSql);
                    execSql.AppendLine(addSqlWhereLongPlanId(traSql));
                    break;

                case (int)TMQConst.MsStructure.StructureId.ProcessMode.history: // 変更管理モード

                    // SQL取得
                    TMQUtil.GetFixedSqlStatementWith(SqlName.List.SubDir, SqlName.List.GetHistoryList, out string withHisSql, new List<string>() { "IsDetail" });
                    TMQUtil.GetFixedSqlStatement(SqlName.List.SubDir, SqlName.List.GetHistoryList, out string hisSql);
                    execSql.AppendLine(withHisSql);
                    execSql.AppendLine(hisSql);
                    break;
                default:

                    // 該当しない場合はエラー
                    return null;
            }

            // キー指定なので結果は必ず1件
            IList<Dao.ListSearchResult> results = new List<Dao.ListSearchResult> { db.GetEntityByDataClass<Dao.ListSearchResult>(execSql.ToString(), param) };

            // 取得した結果に対して、地区と職種の情報を設定する(変更管理データ・トランザクションデータ どちらも設定する)
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.ListSearchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job, StructureType.OldLocation, StructureType.OldJob }, this.db, this.LanguageId, isTree);

            if (param.ProcessMode == (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction)
            {
                //トランザクションモードの場合、申請状況に「申請なし」を設定
                TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
                results[0].ApplicationStatusId = historyManagement.getApplicationStatus(TMQConst.MsStructure.StructureId.ApplicationStatus.None);
            }

            // ボタン表示/非表示フラグを取得
            GetIsAbleToClickBtn(results[0], param);

            // 処理モードを検索結果に設定
            results[0].ProcessMode = param.ProcessMode;

            return results[0];
        }

        /// <summary>
        /// 長計件名IDより、該当データの場所階層IDを取得する
        /// </summary>
        /// <param name="historyManagement">変更管理クラス</param>
        /// <param name="longPlanId">長計件名ID</param>
        /// <returns>工場ID</returns>
        private int getFactoryId(TMQUtil.HistoryManagement historyManagement, long longPlanId)
        {
            // 長計件名IDより長計件名情報を取得
            ComDao.LnLongPlanEntity entity = new();
            entity = entity.GetEntity(longPlanId, this.db);

            // 場所階層IDより工場IDを取得する
            return historyManagement.getFactoryId((int)entity.LocationStructureId);
        }

        #endregion
        #region 保全情報一覧関連
        /// <summary>
        /// 指定された保全情報一覧に長期計画IDで取得した内容を設定する
        /// </summary>
        /// <param name="param">取得するキー項目の内容</param>
        /// <param name="toCtrlIds">値を設定する一覧のコントロールID</param>
        /// <param name="isMaintainanceKindFactory">out 長期計画の工場IDが点検種別ごと一覧表示工場の場合True</param>
        /// <param name="factoryId">out 長期計画の工場ID</param>
        /// <param name="isTree">省略可能 ツリービュー表示用の場合、True</param>
        /// <param name="isInitDetail">詳細画面の初期化表示時(一覧画面→詳細画面)の場合のみTrue</param>
        /// <returns>エラーの場合False</returns>
        private bool initFormByLongPlanId(Dao.detailSearchCondition param, List<string> toCtrlIds, out bool isMaintainanceKindFactory, out int factoryId, bool isTree = false, bool isInitDetail = false)
        {
            // SQLで値を取得
            var infoLongPlan = getLongPlanInfo(param, isTree);

            // 工場IDを設定
            factoryId = infoLongPlan.FactoryId ?? -1;
            // 保全情報一覧(点検種別)を表示するかを取得して設定
            isMaintainanceKindFactory = getIsMaintainanceKindFactoryByDb(infoLongPlan.FactoryId);
            infoLongPlan.IsDisplayMaintainanceKind = isMaintainanceKindFactory;

            // 詳細画面の初期化時(一覧画面→詳細画面)
            if (isInitDetail)
            {
                //URL直接起動時、参照データの権限チェック
                if (!CheckAccessUserBelong((int)infoLongPlan.LocationStructureId, (int)infoLongPlan.JobStructureId))
                {
                    return false;
                }
            }

            // 画面に反映
            bool resut = initFormByParam(infoLongPlan, toCtrlIds);
            return resut;

            /// <summary>
            /// 保全情報一覧(点検種別)を表示するかをDBより取得する処理
            /// </summary>
            /// <param name="factoryId">表示対象かどうか調べる工場ID</param>
            /// <returns>長期計画の工場IDが点検種別ごと一覧表示工場の場合True</returns>
            bool getIsMaintainanceKindFactoryByDb(int? factoryId)
            {
                // 点検種別毎一覧表示工場
                var list = new ComDao.MsStructureEntity().GetGroupList(TMQConst.MsStructure.GroupId.MaintainanceKindManageFactory, this.db);
                // この結果の工場IDと一致するかを判定
                var isMaintainanceKindFactory = list.Count(x => x.FactoryId == factoryId) > 0;

                if (isMaintainanceKindFactory)
                {
                    // 点検種別毎表示の工場でも、長期計画に紐づく機器が点検種別毎管理でなければ通常の一覧を表示する
                    bool? result = TMQUtil.SqlExecuteClass.SelectEntity<bool?>(SqlName.Detail.GetMaintKindManageByLongPlanId, SqlName.Detail.SubDir, param, this.db);
                    // 紐づく機器が無い場合は終了
                    if (result == null) { return isMaintainanceKindFactory; }
                    if (!(result ?? false))
                    {
                        // 点検種別毎管理で無い場合、通常の一覧なのでFalse
                        isMaintainanceKindFactory = false;
                    }
                }

                return isMaintainanceKindFactory;
            }
        }

        /// <summary>
        /// 保全情報一覧検索のSQLのWITH句を取得する処理
        /// </summary>
        /// <param name="isUnComp">未完了のものだけを取得する場合True</param>
        /// <param name="processMode">処理モード</param>
        /// <returns>SQLの内容</returns>
        private string getSqlGetDetailWith(bool isUnComp, int processMode)
        {
            List<string> listUnComment = new();
            if (isUnComp)
            {
                listUnComment.Add(UnCommentWordOfGetDetailWith);
            }
            //実行するSQL格納先ディレクトリ
            string subDir = processMode == (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction ? SqlName.Detail.SubDirLongPlan : SqlName.Detail.SubDir;
            TMQUtil.GetFixedSqlStatement(subDir, SqlName.Detail.GetDetailWith, out string withSql, listUnComment);
            return withSql;
        }

        /// <summary>
        /// 保全情報一覧取得
        /// </summary>
        /// <param name="longPlanId">長計件名ID</param>
        /// <param name="factoryId">この長計件名IDの工場ID</param>
        /// <param name="processMode">処理モード</param>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <param name="isDisplayMaintainanceKind">保全情報一覧(点検種別毎)を表示する場合True</param>
        /// <param name="schedulePlanContent">取得する一覧の種類</param>
        /// <param name="isUnComp">未完了を含むもののみ表示する場合はTrue、省略時はFalse</param>
        /// <returns>取得した一覧のデータクラスのリスト</returns>
        private IList<Dao.Detail.List> getMaintainanceList(long longPlanId, int factoryId, int processMode, long? historyManagementId, bool isDisplayMaintainanceKind, SchedulePlanContent schedulePlanContent, bool isUnComp = false)
        {
            // 値に応じてSQLを取得
            string fileNameSelect = string.Empty;
            //実行するSQL格納先ディレクトリ
            string subDir = SqlName.Detail.SubDirLongPlan;
            // ソートキー、部位と保全項目の場合は追加
            StringBuilder orderBy = new();
            switch (schedulePlanContent)
            {
                case SchedulePlanContent.Equipment:
                    // 機器
                    fileNameSelect = SqlName.Detail.GetDetailEquipment;
                    break;
                case SchedulePlanContent.InspectionSite:
                    // 部位
                    fileNameSelect = SqlName.Detail.GetDetailInspectionSite;
                    // 機器に加えて部位でソート
                    orderBy.AppendLine(",inspection_site_structure_id");
                    orderBy.AppendLine(",management_standards_component_id");
                    break;
                case SchedulePlanContent.Maintainance:
                    // 保全項目
                    fileNameSelect = SqlName.Detail.GetDetailMaintainance;
                    // 機器に加えて部位、項目でソート
                    orderBy.AppendLine(",inspection_site_structure_id");
                    orderBy.AppendLine(",management_standards_component_id");
                    orderBy.AppendLine(",inspection_content_structure_id");
                    orderBy.AppendLine(",management_standards_content_id");
                    //実行するSQL格納先ディレクトリ
                    if (processMode == (int)TMQConst.MsStructure.StructureId.ProcessMode.history)
                    {
                        subDir = SqlName.Detail.SubDir;
                    }
                    break;
                default:
                    // 到達不能
                    return null;
            }
            // SQL取得
            string withSql = getSqlGetDetailWith(isUnComp, processMode); // WITH句
            TMQUtil.GetFixedSqlStatement(subDir, fileNameSelect, out string selectSql); // SELECT句
            // 結合
            StringBuilder execSql = new(withSql);
            execSql.AppendLine(selectSql);
            // 機器
            execSql.AppendLine("machine_name");
            execSql.AppendLine(",machine_id");
            if (isDisplayMaintainanceKind)
            {
                // 保全情報一覧(点検種別毎)を表示する場合
                if (schedulePlanContent == SchedulePlanContent.Maintainance)
                {
                    // 点検種別を表示する場合はソートキーに追加
                    execSql.AppendLine(",kind_order");
                }
            }
            // 部位、保全項目のキーを(列を表示する場合は)追加
            execSql.AppendLine(orderBy.ToString());

            // 検索条件 前画面より引き継いだ長期計画件名ID
            var param = getScheduleCondition(longPlanId, factoryId, getYearStartMonth(factoryId), historyManagementId);
            // 検索
            var list = db.GetListByDataClass<Dao.Detail.List>(execSql.ToString(), param);
            return list;
        }

        #endregion

        /// <summary>
        /// 変更管理更新
        /// </summary>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="now">システム日時</param>
        /// <param name="updateFactoryFlg">工場IDを更新する場合true</param>
        /// <param name="locationStructureId">場所階層ID</param>
        /// <returns>エラーの場合False</returns>
        private (bool result, ComDao.HmHistoryManagementEntity entity) updateHistoryManagement(long historyManagementId, int userId, DateTime now, bool updateFactoryFlg, int locationStructureId = -1)
        {
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
            List<string> listUnComment = new List<string>() { "ApplicationUserId" };
            ComDao.HmHistoryManagementEntity entity = new();
            entity.HistoryManagementId = historyManagementId;
            entity.ApplicationUserId = userId;
            if (updateFactoryFlg)
            {
                //申請区分が「新規登録申請」の場合、工場IDを設定
                entity.FactoryId = historyManagement.getFactoryId(locationStructureId);
                listUnComment.Add("FactoryId");
            }
            // テーブル共通項目を設定
            setExecuteConditionByDataClassCommon<ComDao.HmHistoryManagementEntity>(ref entity, now, userId, userId);
            //変更管理テーブル更新処理
            bool result = historyManagement.UpdateHistoryManagement(entity, listUnComment);
            return (result, entity);
        }
        #endregion
    }
}