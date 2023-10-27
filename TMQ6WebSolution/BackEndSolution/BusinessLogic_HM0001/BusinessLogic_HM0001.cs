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
using Dao = BusinessLogic_HM0001.BusinessLogicDataClass_HM0001;
using FunctionTypeId = CommonTMQUtil.CommonTMQConstants.Attachment.FunctionTypeId;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;
using HistoryManagementDao = CommonTMQUtil.CommonTMQUtilDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_HM0001
{
    /// <summary>
    /// 変更管理 機器台帳
    /// </summary>
    public partial class BusinessLogic_HM0001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 実行処理区分
        /// </summary>
        private static class executionDiv
        {
            /// <summary>機器の新規・複写登録</summary>
            public const int MachineNew = 1;
            /// <summary>機器の修正</summary>
            public const int MachineEdit = 2;
            /// <summary>機器の削除</summary>
            public const int MachineDelete = 3;
            /// <summary>保全項目一覧の追加</summary>
            public const int ComponentNew = 4;
            /// <summary>保全項目一覧の項目編集</summary>
            public const int ComponentEdit = 5;
            /// <summary>保全項目一覧の削除</summary>
            public const int ComponentDelete = 6;
        }
        /// <summary>
        /// 機器レベル区分
        /// </summary>
        private static class MachineLavel
        {
            /// <summary>ループ</summary>
            public const string Loop = "4";
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"HistoryMachine";
            /// <summary>機器台帳(MC0001)のSQL格納先サブディレクトリ名</summary>
            public const string SubDirMachine = @"Machine";

            /// <summary>
            /// 一覧画面SQL
            /// </summary>
            public static class List
            {
                /// <summary>一覧情報取得SQL</summary>
                public const string GetHistoryMachineList = "GetHistoryMachineList";
                /// <summary>変更管理情報取得SQL</summary>
                public const string GetHistoryManagementDetail = "GetHistoryManagementDetail";
                /// <summary>一覧情報件数取得SQL</summary>
                public const string GetCountHistoryMachineList = "GetCountHistoryMachineList";
            }
            /// <summary>
            /// 詳細画面SQL
            /// </summary>
            public static class Detail
            {
                /// <summary>保全項目一覧(トランザクション)取得SQL</summary>
                public const string GetManagementStandardOriginal = "GetManagementStandardOriginal";
                /// <summary>保全項目一覧(変更管理)取得SQL</summary>
                public const string GetHistoryManagementStandardsList = "GetHistoryManagementStandardsList";
                /// <summary>機番IDから見た申請状況の拡張項目を取得する</summary>
                public const string GetApplicationStatusFromMachineId = "GetApplicationStatusFromMachineId";
                /// <summary>変更管理IDより適用法規情報を取得するSQL</summary>
                public const string GetApplicableLaws = "GetApplicableLaws";
                /// <summary>機番IDより適用法規情報を取得するSQL</summary>
                public const string GetApplicableLawsTransaction = "GetApplicableLawsTransaction";
                /// <summary>機番IDより機器別管理基準部位を取得するSQL</summary>
                public const string GetManagementStandardsComponent = "GetManagementStandardsComponent";
                /// <summary>機器別管理基準部位IDより機器別管理基準部位を取得するSQL</summary>
                public const string GetManagementStandardsContent = "GetManagementStandardsContent";
                /// <summary>機器別管理基準内容IDより保全スケジュールを取得するSQL</summary>
                public const string GetMaintainanceSchedule = "GetMaintainanceSchedule";
                /// <summary>機器別管理基準部位変更内容テーブル登録SQL</summary>
                public const string InsertManagementStandardsComponentInfo = "InsertManagementStandardsComponentInfo";
                /// <summary>機器別管理基準内容変更内容テーブル登録SQL</summary>
                public const string InsertManagementStandardsContentInfo = "InsertManagementStandardsContentInfo";
                /// <summary>保全スケジュール変更内容テーブル登録SQL</summary>
                public const string InsertMaintainanceScheduleInfo = "InsertMaintainanceScheduleInfo";
                /// <summary>変更管理IDより機器別管理基準部位を取得するSQL</summary>
                public const string GetManagementStandardsComponentByHistoryManagementId = "GetManagementStandardsComponentByHistoryManagementId";
                /// <summary>変更管理IDより機器別管理基準部位を取得するSQL</summary>
                public const string GetManagementStandardsContentByHistoryManagementId = "GetManagementStandardsContentByHistoryManagementId";
                /// <summary>変更管理IDより保全スケジュールを取得するSQL</summary>
                public const string GetMaintainanceScheduleByHistoryManagementId = "GetMaintainanceScheduleByHistoryManagementId";
                /// <summary>承認時(新規登録申請)、機番情報変更管理テーブルから機番情報テーブルにデータを登録するSQL</summary>
                public const string InsertMachineInfoTransaction = "InsertMachineInfoTransaction";
                /// <summary>承認時(新規登録申請)、機器情報変更管理テーブルから機番情報テーブルにデータを登録するSQL</summary>
                public const string InsertEquipmentInfoTransaction = "InsertEquipmentInfoTransaction";
                /// <summary>承認時(新規登録申請)、適用法規変更管理テーブルから適用法規情報テーブルにデータを登録するSQL</summary>
                public const string InsertApplicableLawsTransaction = "InsertApplicableLawsTransaction";
                /// <summary>承認時(保全項目一覧の登録)、機器別管理基準部位変更管理テーブルから適機器別管理基準部位テーブルにデータを登録するSQL</summary>
                public const string InsertManagementStandardsComponentTransaction = "InsertManagementStandardsComponentTransaction";
                /// <summary>承認時(保全項目一覧の登録)、機器別管理基準内容変更管理テーブルから適機器別管理基準内容テーブルにデータを登録するSQL</summary>
                public const string InsertManagementStandardsContentTransaction = "InsertManagementStandardsContentTransaction";
                /// <summary>承認時(保全項目一覧の登録)、機器別管理基準部位変更管理テーブルから適機器別管理基準部位テーブルにデータを登録するSQL</summary>
                public const string InsertMaintainanceScheduleTransaction = "InsertMaintainanceScheduleTransaction";
                /// <summary>SQL名：機器別管理基準 同一点検種別周期チェック</summary>
                public const string GetMaintainanceKindManageInsertExistCheckHistory = "GetMaintainanceKindManageInsertExistCheckHistory";
                /// <summary>SQL名：機器別管理基準部位変更管理テーブル更新SQL</summary>
                public const string UpdateHmMcManagementStandardsComponent = "UpdateHmMcManagementStandardsComponent";
                /// <summary>SQL名：機器別管理基準内容変更管理テーブル更新SQL</summary>
                public const string UpdateHmMcManagementStandardsContent = "UpdateHmMcManagementStandardsContent";
                /// <summary>SQL名：保全スケジュール変更管理テーブル更新SQL</summary>
                public const string UpdateHmMaintainanceSchedule = "UpdateHmMaintainanceSchedule";
                /// <summary>SQL名：機器別管理基準 同一点検種別周期データ取得</summary>
                public const string GetMaintainanceKindManageDataHistory = "GetMaintainanceKindManageDataHistory";
                /// <summary>SQL名：機器別管理基準 保全項目重複チェック</summary>
                public const string GetManagementStandardCountCheckHistory = "GetManagementStandardCountCheckHistory";
                /// <summary>SQL名：機器別管理基準 同一点検種別点検内容データ取得</summary>
                public const string GetMaintainanceKindManageDataUpdContentHistory = "GetMaintainanceKindManageDataUpdContentHistory";
                /// <summary>SQL名：機器別管理基準内容  スケジューリング一覧更新登録</summary>
                public const string UpdateManagementStandardsContentSchedule = "UpdateManagementStandardsContentSchedule";
                /// <summary>SQL名：機器別管理基準 同一点検種別周期チェック</summary>
                public const string GetMaintainanceKindManageExistCheckHistory = "GetMaintainanceKindManageExistCheckHistory";
                /// <summary>機器別管理基準部位変更管理テーブル登録SQL</summary>
                public const string InsertHmManagementStandardsComponentInfo = "InsertHmManagementStandardsComponentInfo";
                /// <summary>SQL名：長期計画存在チェック(変更管理)</summary>
                public const string GetLongPlanSingleHistory = "GetLongPlanSingleHistory";
                /// <summary>SQL名：機器レベル取得</summary>
                public const string GetMachineLevel = "GetMachineLevel";
                #region 機器台帳(MC0001)のSQLを使用
                /// <summary>保全項目一覧取得(機器台帳：MC0001のSQLを使用)</summary>
                public const string GetManagementStandard = "GetManagementStandard";
                /// <summary>機番・機器情報(トランザクション)取得SQL</summary>
                public const string GetMachineDetail = "MachineDetail";
                /// <summary>SQL名：機器別管理基準_長期計画存在取得</summary>
                public const string GetChkLongPlan = "GetLongPlan";
                /// <summary>SQL名：機器別管理基準_保全活動存在取得</summary>
                public const string GetChkMsSummary = "GetMsSummary";
                /// <summary>SQL名：親子構成存在取得</summary>
                public const string GetChkParentInfo = "GetParentInfo";
                /// <summary>SQL名：ループ構成存在取得</summary>
                public const string GetChkLoopInfo = "GetLoopInfo";
                /// <summary>SQL名：機番情報更新SQL</summary>
                public const string UpdateMachineInfo = "UpdateMachineInfo";
                /// <summary>SQL名：機器情報更新SQL</summary>
                public const string UpdateEquipmentInfo = "UpdateEquipmentInfo";
                /// <summary>SQL名：適用法規情報削除SQL/summary>
                public const string DeleteApplicableLawsInfo = "DeleteApplicableLaws";
                /// <summary>SQL名：機番添付削除/summary>
                public const string DeleteMachineAttachment = "DeleteMachineAttachment";
                /// <summary>SQL名：機器添付削除/summary>
                public const string DeleteEquipmentAttachment = "DeleteEquipmentAttachment";
                /// <summary>SQL名：機器別管理基準添付削除/summary>
                public const string DeleteManagementStandardAttachment = "DeleteManagementStandardAttachment";
                /// <summary>SQL名：MP情報添付削除/summary>
                public const string DeleteMpInfoAttachment = "DeleteMpInfoAttachment";
                /// <summary>SQL名：機器情報削除/summary>
                public const string DeleteEquipmentSpecInfo = "DeleteEquipmentSpecInfo";
                /// <summary>SQL名：保全スケジュール詳細削除/summary>
                public const string DeleteMaintainanceScheduleDetail = "DeleteMaintainanceScheduleDetail";
                /// <summary>SQL名：保全スケジュール削除/summary>
                public const string DeleteMaintainanceSchedule = "DeleteMaintainanceSchedule";
                /// <summary>SQL名：機器別管理基準部位削除/summary>
                public const string DeleteManagementStandardsContent = "DeleteManagementStandardsContent";
                /// <summary>SQL名：機器別管理基準内容削除/summary>
                public const string DeleteManagementStandardsComponent = "DeleteManagementStandardsComponent";
                /// <summary>SQL名：機番使用部品削除/summary>
                public const string DeleteMachineUseParts = "DeleteMachineUseParts";
                /// <summary>SQL名：親子構成削除/summary>
                public const string DeleteParentInfo = "DeleteParentInfo";
                /// <summary>SQL名：ループ構成削除/summary>
                public const string DeleteLoopInfo = "DeleteLoopInfo";
                /// <summary>SQL名：MP情報削除/summary>
                public const string DeleteMpInfo = "DeleteMpInfo";
                /// <summary>SQL名：機器別管理基準 保全項目データ取得</summary>
                public const string GetManagementStandardDetail = "GetManagementStandardDetail";
                /// <summary>SQL名：機器別管理基準 部位設定複数存在チェック</summary>
                public const string GetInspectionMultiExistCheck = "GetInspectionMultiExistCheck";
                /// <summary>SQL名：機器別管理基準 開始日以降保全活動存在チェック</summary>
                public const string GetScheduleMsSummryCountAfterCheck = "GetScheduleMsSummryCountAfterCheck";
                /// <summary>SQL名：機器別管理基準 開始日以降保全活動存在チェック(点検種別単位)</summary>
                public const string GetScheduleMsSummryCountAfterMaintainanceKindManageCheck = "GetScheduleMsSummryCountAfterMaintainanceKindManageCheck";
                /// <summary>SQL名：機器別管理基準 保全履歴存在チェック</summary>
                public const string GetScheduleMsSummryCountCheck = "GetScheduleMsSummryCountCheck";
                /// <summary>SQL名：保全スケジュール詳細  新規登録</summary>
                public const string InsertMaintainanceScheduleDetail = "InsertMaintainanceScheduleDetail";
                /// <summary>SQL名：親子構成登録</summary>
                public const string InsertParentInfo = "InsertParentInfo";
                /// <summary>SQL名：ループ構成登録</summary>
                public const string InsertLoopInfo = "InsertLoopInfo";
                /// <summary>SQL名：長期計画存在チェック</summary>
                public const string GetLongPlanSingle = "GetLongPlanSingle";
                /// <summary>SQL名：保全活動存在チェック</summary>
                public const string GetMsSummarySingle = "GetMsSummarySingle";
                /// <summary>SQL名：保全スケジュール詳細 データ削除 文書の最大更新日時を取得</summary>
                public const string GetMaxDateByKeyId = "GetMaxDateByKeyId";
                /// <summary>SQL名：保全スケジュール詳細 データ削除</summary>
                public const string DeleteMaintainanceScheduleDetailSingle = "DeleteMaintainanceScheduleDetailSingle";
                /// <summary>SQL名：保全スケジュール データ削除</summary>
                public const string DeleteMaintainanceScheduleSingle = "DeleteMaintainanceScheduleSingle";
                /// <summary>SQL名：機器別管理基準内容 データ削除</summary>
                public const string DeleteManagementStandardsContentSingle = "DeleteManagementStandardsContentSingle";
                /// <summary>SQL名：機器別管理基準部位 データ削除</summary>
                public const string DeleteManagementStandardsComponentSingle = "DeleteManagementStandardsComponentSingle";
                /// <summary>SQL名：機能タイプID、キーIDに紐付くレコード件数を取得する</summary>
                public const string GetAttachmentCount = "GetAttachmentCount";
                /// <summary>SQL名：機器別管理基準 同一点検種別点検内容データ取得</summary>
                public const string GetMaintainanceKindManageDataUpdContent = "GetMaintainanceKindManageDataUpdContent";
                /// <summary>SQL名：機器別管理基準 同一点検種別周期データ取得</summary>
                public const string GetMaintainanceKindManageData = "GetMaintainanceKindManageData";
                /// <summary>SQL名：保全スケジュール  未来データ削除</summary>
                public const string DeleteMaintainanceScheduleRemake = "DeleteMaintainanceScheduleRemake";
                /// <summary>SQL名：保全スケジュール  新規登録</summary>
                public const string InsertMaintainanceSchedule = "InsertMaintainanceSchedule";
                /// <summary>SQL名：機器別管理基準部位  更新登録</summary>
                public const string UpdateManagementStandardsComponent = "UpdateManagementStandardsComponent";
                /// <summary>SQL名：機器別管理基準内容  更新登録</summary>
                public const string UpdateManagementStandardsContent = "UpdateManagementStandardsContent";
                /// <summary>SQL名：保全スケジュール詳細  未来データ削除</summary>
                public const string DeleteMaintainanceScheduleDetailRemake = "DeleteMaintainanceScheduleDetailRemake";

                #endregion
            }
            /// <summary>
            /// 編集画面SQL
            /// </summary>
            public static class Edit
            {
                /// <summary>機番情報変更管理テーブル登録SQL</summary>
                public const string InsertMachineInfo = "InsertMachineInfo";
                /// <summary>機器情報変更管理テーブル登録SQL</summary>
                public const string InsertEquipmentInfo = "InsertEquipmentInfo";
                /// <summary>適用法規情報変更管理テーブル登録SQL</summary>
                public const string InsertApplicableLawsInfo = "InsertApplicableLawsInfo";
                /// <summary>機器情報変更管理テーブル更新SQL</summary>
                public const string UpdateHmMcMachine = "UpdateHmMcMachine";
                /// <summary>機器情報変更管理テーブル更新SQL</summary>
                public const string UpdateHmMcEquipment = "UpdateHmMcEquipment";
                /// <summary>機番ID(機番情報テーブル)を新規採番するSQL</summary>
                public const string GetNewMachineId = "GetNewMachineId";
                /// <summary>変更管理IDに紐付く機器の変更申請の件数を取得するSQL</summary>
                public const string GetMachineHistoryManagementCnt = "GetMachineHistoryManagementCnt";
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
                    /// 検索条件(自分の件名のみ表示)
                    /// </summary>
                    public const string Condition = "BODY_020_00_LST_0";
                    /// <summary>
                    /// 一覧
                    /// </summary>
                    public const string List = "BODY_040_00_LST_0";
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
            /// 詳細画面
            /// </summary>
            public static class FormDetail
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 1;
                /// <summary>
                /// グループ番号(変更管理情報)
                /// </summary>
                public const short GroupNoHistory = 201;
                /// <summary>
                /// グループ番号(機番情報)
                /// </summary>
                public const short GroupNoMachine = 202;
                /// <summary>
                /// グループ番号(機器情報)
                /// </summary>
                public const short GroupNoEquipment = 203;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 機番情報(機器番号～保全方式)
                    /// </summary>
                    public const string Machine = "BODY_030_00_LST_1";
                    /// <summary>
                    /// 機番情報(使用区分～点検種別毎管理)
                    /// </summary>
                    public const string Equipment = "BODY_060_00_LST_1";

                    /// <summary>
                    /// 保全項目一覧
                    /// </summary>
                    public const string ManagementStandardsList = "BODY_080_00_LST_1";
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
                    /// 保全項目一覧 単票 登録
                    /// </summary>
                    public const string ManagementStandardRegist = "ManagementStandardRegist";
                }
            }

            /// <summary>
            /// 詳細編集画面
            /// </summary>
            public static class FormEdit
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 2;
                /// <summary>
                /// グループ番号(機番情報)
                /// </summary>
                public const short GroupNoMachine = 301;
                /// <summary>
                /// グループ番号(機器情報)
                /// </summary>
                public const short GroupNoEquipment = 302;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 機番情報(機器番号～保全方式)
                    /// </summary>
                    public const string Machine = "BODY_010_00_LST_2";
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
                    /// <summary>
                    /// 戻る
                    /// </summary>
                    public const string Back = "Back";
                }
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_HM0001() : base()
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
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo: // 一覧検索
                    // 初期表示の場合は引数にTrueを設定
                    if (!searchList(compareId.IsInit()))
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;

                case ConductInfo.FormDetail.FormNo: // 詳細画面
                    if (!searchDetailList(null, null, true))
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;

                case ConductInfo.FormEdit.FormNo: // 詳細編集画面
                    if (!searchEditList())
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
            if (compareId.IsRegist() || compareId.IsStartId(ConductInfo.FormDetail.ButtonId.ManagementStandardRegist))
            {
                // 登録の場合
                // 登録処理実行
                return Regist();
            }
            else if (compareId.IsDelete())
            {
                // 削除の場合
                // 削除処理実行
                return Delete();
            }
            else if (compareId.IsStartId("judgeStatusCntExceptApproved"))
            {
                // 機器に対する「承認済」以外のデータ有無を取得
                return judgeStatusCntExceptApproved();

            }
            else if (compareId.IsStartId("checkHisoryManagemtntExclusiv"))
            {
                // 変更管理の排他チェック
                return checkHisoryManagemtntExclusive();
            }

            // 登録・削除と異なる、機能固有の処理の場合
            string metodName = string.Empty;
            string processName = string.Empty;
            if (this.FormNo == ConductInfo.FormList.FormNo && compareId.IsStartId(ConductInfo.FormList.ButtonId.ApprovalAll))
            {
                // 一覧画面 一括承認ボタン
                metodName = "registFormList";
                processName = ComRes.ID.ID111020051; // 一括承認
            }
            else if (this.FormNo == ConductInfo.FormList.FormNo && compareId.IsStartId(ConductInfo.FormList.ButtonId.DenialAll))
            {
                // 一覧画面 一括否認ボタン
                metodName = "registFormList";
                processName = ComRes.ID.ID111020052; // 一括否認
            }
            else if (this.FormNo == ConductInfo.FormDetail.FormNo && compareId.IsStartId(ConductInfo.FormDetail.ButtonId.DeleteRequest))
            {
                // 詳細画面 削除申請ボタン
                metodName = "deleteRequest";
                processName = ComRes.ID.ID111110068; // 削除申請
            }
            else if (this.FormNo == ConductInfo.FormDetail.FormNo && compareId.IsStartId(ConductInfo.FormDetail.ButtonId.CancelRequest))
            {
                // 詳細画面 申請内容取消ボタン
                metodName = "cancelRequest";
                processName = ComRes.ID.ID111120232; // 申請内容取消
            }
            else if (this.FormNo == ConductInfo.FormDetail.FormNo && compareId.IsStartId(ConductInfo.FormDetail.ButtonId.PullBackRequest))
            {
                // 詳細画面 承認依頼引戻ボタン
                metodName = "pullBackRequest";
                processName = ComRes.ID.ID111120233; // 承認依頼引戻
            }
            else if (this.FormNo == ConductInfo.FormDetail.FormNo && compareId.IsStartId(ConductInfo.FormDetail.ButtonId.ChangeApplicationApproval))
            {
                // 詳細画面 承認ボタン
                metodName = "changeApplicationApproval";
                processName = ComRes.ID.ID111120228; // 承認
            }
            else
            {
                // この部分は到達不能なので、エラーを返す
                return ComConsts.RETURN_RESULT.NG;
            }

            if (!string.IsNullOrEmpty(metodName) && ComExecImpl(metodName, processName) < 0)
            {
                // エラー発生時
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // ○○に失敗しました。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, processName });
                }
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 完了メッセージ表示
            setCompleteExecute(processName);
            return ComConsts.RETURN_RESULT.OK;

            void setCompleteExecute(string paramMsgId)
            {
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 〇〇が完了しました。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, paramMsgId });
                }
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            // 画面番号を判定
            switch (this.FormNo)
            {
                case ConductInfo.FormDetail.FormNo: // 詳細画面 保全項目一覧の単票の登録ボタン
                    if (!registManagementStandards())
                    {
                        setErrMsg();
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;

                case ConductInfo.FormEdit.FormNo: // 詳細編集画面 登録ボタン
                    if (!registFormEdit())
                    {
                        setErrMsg();
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;

                default:
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }

            // 保全項目一覧 単票での登録時に確認メッセージを表示するためのもの
            if (this.Status == CommonProcReturn.ProcStatus.Confirm)
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「登録処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200003 });

            return ComConsts.RETURN_RESULT.OK;

            void setErrMsg()
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 未設定時にエラーメッセージを設定
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「登録処理に処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                }
            }
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            this.ResultList = new();

            // 処理メッセージ
            string processMsg = string.Empty;

            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.DeleteRequest))
            {
                processMsg = ComRes.ID.ID111110068;

                // 詳細画面 削除申請
                if (!deleteRequest())
                {
                    // エラーメッセージ設定

                    setErrMsg(processMsg);
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else
            {
                processMsg = ComRes.ID.ID911110001;

                // 保全項目一覧 行削除処理
                if (!deleteManagementStandards())
                {
                    // エラーメッセージ設定
                    processMsg = ComRes.ID.ID911110001;
                    setErrMsg(processMsg);
                    return ComConsts.RETURN_RESULT.NG;
                }
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「○○に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, processMsg });

            return ComConsts.RETURN_RESULT.OK;

            void setErrMsg(string msg)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;

                // 未設定時にエラーメッセージを設定
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「○○に処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, msg });
                }
            }
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 画面に入力された情報を取得する
        /// </summary>
        /// <param name="now">現在日時</param>
        /// <returns>登録情報</returns>
        private Dao.searchResult getRegistInfoBySearchResult(List<short> groupNoList, DateTime now, bool isSearchConditionDictionary = false)
        {
            // 機番情報・機器情報取得
            Dao.searchResult registInfo = getRegistInfo<Dao.searchResult>(groupNoList, now, isSearchConditionDictionary);

            //最下層の構成IDを取得して機能場所階層ID、職種機種階層IDにセットする
            IList<Dao.searchResult> results = new List<Dao.searchResult>();
            results.Add(registInfo);
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.searchResult>(ref results, new List<TMQUtil.StructureLayerInfo.StructureType> { TMQUtil.StructureLayerInfo.StructureType.Location, TMQUtil.StructureLayerInfo.StructureType.Job });

            // 取得した値を登録情報に設定
            registInfo.ConservationStructureId = registInfo.InspectionSiteConservationStructureId; // 保全方式

            return registInfo;
        }

        /// <summary>
        /// 機器情報(変更管理) 登録処理
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="isDefaultEquipmentId">既存の機器IDを登録する場合はTrue</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registEquipmentInfo(ref Dao.searchResult registInfo, bool isDefaultEquipmentId)
        {
            // 機器情報(変更管理) 登録処理

            // 機器IDを新規採番するか既存の機器IDを使用するか決める
            List<string> listUnComment = new();
            if (isDefaultEquipmentId)
            {
                // 既存の機器IDを登録する
                listUnComment.Add("DefaultEquipmentId");
            }
            else
            {
                // 機器IDを新規採番する
                listUnComment.Add("NewEquipmentId");
            }

            // SQL実行 + 採番した値を返す
            if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long equipmentId, SqlName.Edit.InsertEquipmentInfo, SqlName.SubDir, registInfo, this.db, string.Empty, listUnComment))
            {
                return false;
            }

            // 新規採番した機器IDを設定する(既存の機器IDで登録した場合は既存の機器IDが返される)
            registInfo.EquipmentId = equipmentId;

            return true;
        }

        /// <summary>
        /// 適用法規情報(変更管理) 登録処理
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="now">現在日時</param>
        /// <param name="isDelete">既存のデータを削除する場合True</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registApplicableInfo(Dao.searchResult registInfo, DateTime now, bool isDelete)
        {
            // 機番IDをキーに既存のデータを削除する
            if (isDelete)
            {
                // 適用法規情報(変更管理) 削除処理
                if (!deleteApplicableLawsHistory(registInfo))
                {
                    return false;
                }
            }

            //画面で選択されていない場合は何もしない
            if (string.IsNullOrEmpty(registInfo.ApplicableLawsStructureId))
            {
                return true;
            }

            // 適用法規の構成IDがパイプ「｜」区切りになっているので分割して登録
            foreach (string applicableLawsStructureId in registInfo.ApplicableLawsStructureId.Split("|"))
            {
                ComDao.HmMcApplicableLawsEntity historyApplicableLawsEntity = new();
                historyApplicableLawsEntity.HistoryManagementId = registInfo.HistoryManagementId;             // 変更管理ID
                historyApplicableLawsEntity.ApplicableLawsStructureId = int.Parse(applicableLawsStructureId); // 適用法規アイテムID
                historyApplicableLawsEntity.MachineId = registInfo.MachineId;                                 // 機番ID

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref historyApplicableLawsEntity, now, int.Parse(this.UserId), int.Parse(this.UserId));

                // SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Edit.InsertApplicableLawsInfo, SqlName.SubDir, historyApplicableLawsEntity, this.db, string.Empty, new List<string>() { "NewApplicableLawsId" }))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 適用法規情報(変更管理) 削除
        /// </summary>
        /// <param name="condition">削除条件</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool deleteApplicableLawsHistory(Dao.searchResult condition)
        {
            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetApplicableLaws, out string applicableSql, new List<string>() { "HistoryManagementApplicable" });

            // SQL実行
            IList<ComDao.HmMcApplicableLawsEntity> results = db.GetListByDataClass<ComDao.HmMcApplicableLawsEntity>(applicableSql, condition);
            if (results != null && results.Count > 0)
            {
                // 取得したデータを削除
                foreach (ComDao.HmMcApplicableLawsEntity result in results)
                {
                    ComDao.HmMcApplicableLawsEntity historyApplicableLawsEntity = new();
                    if (!historyApplicableLawsEntity.DeleteByPrimaryKey(result.HmApplicableLawsId, this.db))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// SQL実行処理(削除処理データがない場合はエラーとしない)
        /// </summary>
        /// <param name="condition">削除条件</param>
        /// <param name="subDirName">サブディレクトリ名</param>
        /// <param name="sqlName">SQL名</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool deleteMachineInfoDataExists(dynamic condition, string subDirName, string sqlName)
        {
            // SQL取得
            TMQUtil.GetFixedSqlStatement(subDirName, sqlName, out string sql);

            // SQL実行
            int result = db.Regist(sql, condition);

            // 削除データがない場合があるので返り値が0未満の場合にエラーとする
            if (result < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 変更管理排他チェック
        /// </summary>
        /// <param name="machineInfo">機番情報(画面に表示されている内容)</param>
        /// <param name="isExistsHistory">変更管理が存在するか</param>
        /// <returns>エラーの場合はTrue</returns>
        private bool checkExclusiveHistoryManagement(Dao.searchResult machineInfo, bool isExistsHistory)
        {
            string ctrlId = ConductInfo.FormDetail.ControlId.Machine;

            // 変更管理排他チェック
            // 変更管理が紐付いている場合は排他チェック
            if (isExistsHistory)
            {
                // 排他チェック(変更管理)
                if (!checkExclusiveSingleForTable(ctrlId, new List<string>() { "hm_history_management" }, ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId)))
                {
                    return true;
                }
            }
            else
            {
                // 紐付いていない場合は機器に「承認済」以外の変更管理が紐付いているかチェック
                if (!checkHistoryManagement(machineInfo.MachineId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 機器に対する「承認済み」以外のデータ存在チェック
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>存在しない:True 存在する:False</returns>
        private bool checkHistoryManagement(long machineId)
        {
            // 承認済み」以外のデータ件数取得
            if (getStatusCntExceptApproved(machineId) > 0)
            {
                // 「 対象データに申請が存在するため処理を行えません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160016 });
                return false;
            }

            return true;
        }

        /// <summary>
        /// 機器に対する「承認済み」以外のデータ件数取得
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>承認済み以外の件数件数</returns>
        private int getStatusCntExceptApproved(long machineId)
        {
            // 承認済み」以外のデータ件数取得
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
            return historyManagement.getApplicationStatusCntByKeyId(machineId, TMQConst.MsStructure.StructureId.ApplicationStatus.Approved, false);
        }

        /// <summary>
        /// 機器別管理基準部位変更管理テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <param name="keyId">キーID(機番IDまたは機器別管理基準部位ID)</param>
        /// <param name="now">現在日時</param>
        /// <param name="uncommentItem">SQLでアンコメントする項目</param>
        /// <param name="componentIdList">機器別管理基準部位IDのリスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHmMcManagementStandardsComponent(long historyManagementId, long keyId, DateTime now, string uncommentItem, out List<long> componentIdList)
        {
            componentIdList = new();

            // 検索条件を作成
            ComDao.McManagementStandardsComponentEntity condition = new();
            // どちらにも引数の「KeyId」を設定するが、実際はどちらかしか使用しない
            condition.MachineId = keyId;                      // 機番ID
            condition.ManagementStandardsComponentId = keyId; // 機器別管理基準部位ID

            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetManagementStandardsComponent, out string sql, new List<string>() { uncommentItem });

            // SQL実行
            IList<ComDao.McManagementStandardsComponentEntity> results = db.GetListByDataClass<ComDao.McManagementStandardsComponentEntity>(sql, condition);
            if (results == null || results.Count == 0)
            {
                // データが紐付いていない場合もあるので取得できなくてもtrueを返す
                return true;
            }

            // 取得したデータを機器別管理基準部位変更管理テーブルに登録
            foreach (ComDao.McManagementStandardsComponentEntity result in results)
            {
                // 機器別管理基準部位IDを返り値のリストに格納
                componentIdList.Add(result.ManagementStandardsComponentId);

                ComDao.HmMcManagementStandardsComponentEntity historyComponentEntity = new();
                historyComponentEntity.HistoryManagementId = historyManagementId;                                // 変更管理ID
                historyComponentEntity.ManagementStandardsComponentId = result.ManagementStandardsComponentId;   // 機器別管理基準部位ID
                historyComponentEntity.MachineId = result.MachineId;                                             // 機番ID
                historyComponentEntity.InspectionSiteStructureId = result.InspectionSiteStructureId;             // 部位ID
                historyComponentEntity.IsManagementStandardConponent = result.IsManagementStandardConponent;     // 機器別管理基準フラグ

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref historyComponentEntity, now, int.Parse(this.UserId), int.Parse(this.UserId));

                // SQL実行
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long managementStandardsComponentId, SqlName.Detail.InsertManagementStandardsComponentInfo, SqlName.SubDir, historyComponentEntity, this.db, string.Empty, new List<string>() { "DefaultComponent" }))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 機器別管理基準部内容変更管理テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <param name="now">現在日時</param>
        /// <param name="componentIdList">機器別管理基準部位IDのリスト</param>
        /// <param name="contentIdList">機器別管理基準内容IDのリスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registHmMcManagementStandardsContent(long historyManagementId, DateTime now, List<long> componentIdList, out List<long> contentIdList)
        {
            contentIdList = new();

            // 機器別管理基準内容データを取得するSQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetManagementStandardsContent, out string sql);

            // 機器別管理基準部位IDより機器別管理基準内容のデータを取得
            foreach (long componentId in componentIdList)
            {
                // 検索条件を作成
                ComDao.McManagementStandardsContentEntity condition = new();
                condition.ManagementStandardsComponentId = componentId; // 機器別管理基準部位ID

                // SQL実行
                IList<ComDao.McManagementStandardsContentEntity> results = db.GetListByDataClass<ComDao.McManagementStandardsContentEntity>(sql, condition);
                if (results == null || results.Count == 0)
                {
                    // データが紐付いていない場合もあるのでエラーにはしない
                    break;
                }

                // 取得したデータを機器別管理基準内容変更管理テーブルに登録
                foreach (ComDao.McManagementStandardsContentEntity result in results)
                {
                    // 機器別管理基準内容IDを返り値のリストに格納
                    contentIdList.Add(result.ManagementStandardsContentId);

                    ComDao.HmMcManagementStandardsContentEntity historyContentEntity = new();
                    historyContentEntity.HistoryManagementId = historyManagementId;                                            // 変更管理ID
                    historyContentEntity.ExecutionDivision = executionDiv.ComponentDelete;                                     // 保全項目一覧の削除
                    historyContentEntity.ManagementStandardsContentId = result.ManagementStandardsContentId;                   // 機器別管理基準内容ID
                    historyContentEntity.ManagementStandardsComponentId = componentId;                                         // 機器別管理基準部位ID
                    historyContentEntity.InspectionContentStructureId = result.InspectionContentStructureId;                   // 点検内容ID
                    historyContentEntity.InspectionSiteImportanceStructureId = result.InspectionSiteImportanceStructureId;     // 部位重要度
                    historyContentEntity.InspectionSiteConservationStructureId = result.InspectionSiteConservationStructureId; // 部位保全方式
                    historyContentEntity.MaintainanceDivision = result.MaintainanceDivision;                                   // 保全区分
                    historyContentEntity.MaintainanceKindStructureId = result.MaintainanceKindStructureId;                     // 点検種別
                    historyContentEntity.BudgetAmount = result.BudgetAmount;                                                   // 予算金額
                    historyContentEntity.PreparationPeriod = result.PreparationPeriod;                                         // 準備期間(日)
                    historyContentEntity.LongPlanId = result.LongPlanId;                                                       // 長計件名ID
                    historyContentEntity.OrderNo = result.OrderNo;                                                             // 並び順
                    historyContentEntity.ScheduleTypeStructureId = result.ScheduleTypeStructureId;                             // スケジュール管理基準ID

                    // テーブル共通項目を設定
                    setExecuteConditionByDataClassCommon(ref historyContentEntity, now, int.Parse(this.UserId), int.Parse(this.UserId));

                    // SQL実行
                    if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long managementStandardsContentId, SqlName.Detail.InsertManagementStandardsContentInfo, SqlName.SubDir, historyContentEntity, this.db, string.Empty, new List<string>() { "DefaultContent" }))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 保全スケジュール変更管理テーブル 登録処理
        /// </summary>
        /// <param name="historyManagementId">変更管理ID</param>
        /// <param name="now">現在日時</param>
        /// <param name="contentIdList">機器別管理基準内容IDのリスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registMaintainanceSchedule(long historyManagementId, DateTime now, List<long> contentIdList)
        {
            // 保全スケジュールデータを取得するSQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetMaintainanceSchedule, out string sql);

            // 機器別管理基準内容IDに紐付く保全スケジュールを取得
            foreach (long contentId in contentIdList)
            {
                ComDao.McMaintainanceScheduleEntity condition = new();
                condition.ManagementStandardsContentId = contentId; // 機器別管理基準内容ID

                // SQL実行
                IList<ComDao.McMaintainanceScheduleEntity> results = db.GetListByDataClass<ComDao.McMaintainanceScheduleEntity>(sql, condition);
                if (results == null || results.Count == 0)
                {
                    // データが紐付いていない場合もあるのでエラーにはしない
                    break;
                }

                // 取得したデータを保全スケジュール変更管理テーブルに登録
                foreach (ComDao.McMaintainanceScheduleEntity result in results)
                {
                    ComDao.HmMcMaintainanceScheduleEntity historyScheduleEntity = new();
                    historyScheduleEntity.HistoryManagementId = historyManagementId;                          // 変更管理ID
                    historyScheduleEntity.MaintainanceScheduleId = result.MaintainanceScheduleId;             // 保全スケジュールID
                    historyScheduleEntity.ManagementStandardsContentId = result.ManagementStandardsContentId; // 機器別管理基準内容ID
                    historyScheduleEntity.IsCyclic = result.IsCyclic;                                         // 周期ありフラグ
                    historyScheduleEntity.CycleYear = result.CycleYear;                                       // 周期(年)
                    historyScheduleEntity.CycleMonth = result.CycleMonth;                                     // 周期(月)
                    historyScheduleEntity.CycleDay = result.CycleDay;                                         // 周期(日)
                    historyScheduleEntity.DispCycle = result.DispCycle;                                       // 表示周期
                    historyScheduleEntity.StartDate = result.StartDate;                                       // 開始日

                    // テーブル共通項目を設定
                    setExecuteConditionByDataClassCommon(ref historyScheduleEntity, now, int.Parse(this.UserId), int.Parse(this.UserId));

                    // SQL実行
                    if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long scheduleId, SqlName.Detail.InsertMaintainanceScheduleInfo, SqlName.SubDir, historyScheduleEntity, this.db, string.Empty, new List<string>() { "DefaultSchedule" }))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// データの件数を取得(入力チェックのエラー件数取得に使用)
        /// </summary>
        /// <param name="sqlName">SQL名</param>
        /// <param name="subDir">サブディレクトリ名</param>
        /// <param name="condition">検索条件</param>
        /// <returns>エラー件数がある場合はTrue</returns>
        private bool getErrCnt(string sqlName, string subDir, Dao.managementStandardsResult condition)
        {
            // 検索SQL文の取得
            TMQUtil.GetFixedSqlStatement(subDir, sqlName, out string sql);

            // 総件数を取得
            int cnt = db.GetCount(sql, condition);
            return cnt > 0;
        }

        #region 承認処理
        /// <summary>
        /// 承認処理
        /// </summary>
        /// <param name="condition">変更管理詳細を検索する検索条件</param>
        /// <param name="now">現在日時</param>
        /// <param name="userId">ログインユーザーID</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool approval(ComDao.HmHistoryManagementEntity condition, DateTime now, int userId)
        {
            // 変更管理IDより変更管理詳細情報を取得
            setExecuteConditionByDataClassCommon(ref condition, now, userId, userId);

            // 承認する情報を取得
            List<Dao.historyManagmentDetail> detailList = TMQUtil.SqlExecuteClass.SelectList<Dao.historyManagmentDetail>(SqlName.List.GetHistoryManagementDetail, SqlName.SubDir, condition, this.db);
            if (detailList == null || detailList.Count == 0)
            {
                return false;
            }

            // 翻訳の一時テーブルを作成
            createTranslationTempTbl();

            // 変更管理登録クラス
            TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);

            // 取得した申請データの実行処理区分に応じてトランザクションテーブルに反映する
            foreach (Dao.historyManagmentDetail detail in detailList)
            {
                // 言語IDを設定
                detail.LanguageId = this.LanguageId;

                // 実行処理区分に応じてトランザクションテーブルに反映する
                switch (detail.ExecutionDivision)
                {
                    case executionDiv.MachineNew: // 機器の新規・複写登録
                        if (!registTransactionTableMachine(detail))
                        {
                            return false;
                        }
                        break;
                    case executionDiv.MachineEdit: // 機器の修正
                        if (!updateTransactionTableMachine(detail))
                        {
                            return false;
                        }
                        break;
                    case executionDiv.MachineDelete: // 機器の削除
                        if (!deleteTransactionTableMachine(detail))
                        {
                            return false;
                        }
                        break;
                    case executionDiv.ComponentNew: // 保全項目一覧の追加
                        if (!registTransactionTableComponent(historyManagement, detail, now, userId))
                        {
                            return false;
                        }
                        break;
                    case executionDiv.ComponentEdit: // 保全項目一覧の項目編集
                        if (!updateTransactionTableComponent(historyManagement, detail))
                        {
                            return false;
                        }
                        break;
                    case executionDiv.ComponentDelete: // 保全項目一覧の削除
                        if (!deleteTransactionTableComponent(detail))
                        {
                            return false;
                        }
                        break;

                    default:
                        return false;
                }
            }

            return true;

            // 承認時処理 機器の新規・複写登録
            bool registTransactionTableMachine(Dao.historyManagmentDetail condition)
            {
                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref condition, now, userId, userId);

                // 機番情報 登録処理
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertMachineInfoTransaction, SqlName.SubDir, condition, this.db, string.Empty))
                {
                    return false;
                }

                // 機器情報 登録処理
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertEquipmentInfoTransaction, SqlName.SubDir, condition, this.db, string.Empty))
                {
                    return false;
                }

                // 適用法規が選択されていれば登録
                if (!string.IsNullOrEmpty(condition.ApplicableLawsStructureId))
                {
                    // 適用法規情報 登録処理
                    if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertApplicableLawsTransaction, SqlName.SubDir, condition, this.db, string.Empty))
                    {
                        return false;
                    }
                }

                // 親子構成 ループ構成 登録処理
                if (!registComposition(condition.MachineId))
                {
                    return false;
                }
                return true;
            }

            // 親子構成とループ構成の登録処理
            bool registComposition(long? machineId)
            {
                // 親子構成 登録処理
                ComDao.McMachineParentInfoEntity parentInfo = new();
                parentInfo.MachineId = machineId; // 機番ID
                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref parentInfo, now, userId, userId);
                // SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertParentInfo, SqlName.SubDirMachine, parentInfo, this.db, string.Empty))
                {
                    return false;
                }

                // ループ構成 登録処理
                // 機器レベルがループなら登録とする
                // 一覧検索SQL文の取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetMachineLevel, out string sql);
                dynamic whereParam = null; // WHERE句パラメータ
                whereParam = new { MachineId = machineId, LanguageId = this.LanguageId };
                // 機器レベルの拡張項目取得
                IList<Dao.ExtensionVal> results = db.GetListByDataClass<Dao.ExtensionVal>(sql, whereParam);
                if (results == null || results.Count == 0)
                {
                    return false;
                }
                if (results[0].ExtensionData == MachineLavel.Loop)
                {
                    ComDao.McLoopInfoEntity loopInfo = new();
                    loopInfo.MachineId = machineId;
                    // テーブル共通項目を設定
                    setExecuteConditionByDataClassCommon(ref loopInfo, now, userId, userId);
                    // SQL実行
                    if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertLoopInfo, SqlName.SubDirMachine, loopInfo, this.db, string.Empty))
                    {
                        return false;
                    }
                }
                return true;
            }

            // 承認時処理 機器の修正
            bool updateTransactionTableMachine(Dao.historyManagmentDetail condition)
            {
                // 変更前機器情報を取得
                var oldMachineInfo = new ComDao.McMachineEntity().GetEntity(condition.MachineId, this.db);

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref condition, now, userId, userId);

                // 機番情報 更新処理
                ComDao.HmMcMachineEntity machine = new();
                machine = machine.GetEntity(condition.HmMachineId, this.db);
                setExecuteConditionByDataClassCommon(ref machine, now, userId, userId);
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateMachineInfo, SqlName.SubDirMachine, machine, this.db, string.Empty))
                {
                    return false;
                }

                // 機器情報 更新処理
                ComDao.HmMcEquipmentEntity equipment = new ComDao.HmMcEquipmentEntity().GetEntity(condition.HmEquipmentId, this.db);
                setExecuteConditionByDataClassCommon(ref equipment, now, userId, userId);
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateEquipmentInfo, SqlName.SubDirMachine, equipment, this.db, string.Empty))
                {
                    return false;
                }

                // 適用法規情報 登録処理
                // 機番IDをキーにデータを削除
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteApplicableLawsInfo))
                {
                    return false;
                }

                // 親子構成、ループ機器の登録処理
                if (oldMachineInfo.EquipmentLevelStructureId != machine.EquipmentLevelStructureId)
                {
                    // 元の機器台帳より機器レベルが変更になる場合のみ登録を行う

                    // 既存の親子構成を削除して登録
                    // 親子構成(トランザクション) 削除処理
                    if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteParentInfo))
                    {
                        return false;
                    }
                    // ループ構成(トランザクション) 削除処理
                    if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteLoopInfo))
                    {
                        return false;
                    }
                    // 登録
                    if (!registComposition(condition.MachineId))
                    {
                        return false;
                    }
                }

                // 適用法規が選択されていない場合はここで終了
                if (string.IsNullOrEmpty(condition.ApplicableLawsStructureId))
                {
                    return true;
                }

                // 適用法規情報変更管理テーブルのデータを適用法規情報テーブルに登録する
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertApplicableLawsTransaction, SqlName.SubDir, condition, this.db, string.Empty))
                {
                    return false;
                }
                return true;
            }

            // 承認時処理 機器の削除
            bool deleteTransactionTableMachine(Dao.historyManagmentDetail condition)
            {
                // 添付情報を先に削除

                // 機番が紐付く添付情報 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteMachineAttachment))
                {
                    return false;
                }

                // 機器が紐付く添付情報 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteEquipmentAttachment))
                {
                    return false;
                }

                // 機番が紐付く機器別管理基準が紐付く添付情報 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteManagementStandardAttachment))
                {
                    return false;
                }

                // MP情報に紐付く添付情報 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteMpInfoAttachment))
                {
                    return false;
                }

                // 機番情報(トランザクション) 削除処理
                ComDao.McMachineEntity machineEntity = new();
                if (!machineEntity.DeleteByPrimaryKey(condition.MachineId, this.db))
                {
                    return false;
                }

                // 機器情報(トランザクション) 削除処理
                ComDao.McEquipmentEntity equipmentEntity = new();
                if (!equipmentEntity.DeleteByPrimaryKey(condition.EquipmentId, this.db))
                {
                    return false;
                }

                // 仕様情報(トランザクション) 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteEquipmentSpecInfo))
                {
                    return false;
                }

                // 適用法規(トランザクション) 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteApplicableLawsInfo))
                {
                    return false;
                }

                // 保全スケジュール詳細(トランザクション) 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteMaintainanceScheduleDetail))
                {
                    return false;
                }

                // 保全スケジュール(トランザクション) 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteMaintainanceSchedule))
                {
                    return false;
                }

                // 機器別管理基準内容(トランザクション) 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteManagementStandardsContent))
                {
                    return false;
                }

                // 機器別管理基準部位(トランザクション) 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteManagementStandardsComponent))
                {
                    return false;
                }

                // 使用部品(トランザクション) 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteMachineUseParts))
                {
                    return false;
                }

                // 親子構成(トランザクション) 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteParentInfo))
                {
                    return false;
                }

                // ループ構成(トランザクション) 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteLoopInfo))
                {
                    return false;
                }

                // MP情報(トランザクション) 削除処理
                if (!deleteMachineInfoDataExists(condition, SqlName.SubDirMachine, SqlName.Detail.DeleteMpInfo))
                {
                    return false;
                }

                return true;
            }

            // 承認時処理 保全項目一覧の追加
            bool registTransactionTableComponent(TMQUtil.HistoryManagement historyManagement, Dao.historyManagmentDetail condition, DateTime now, int userId)
            {
                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref condition, now, userId, userId);

                // 機器別管理基準部位(変更管理)のデータを機器別管理基準部位(トランザクション)に登録
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertManagementStandardsComponentTransaction, SqlName.SubDir, condition, this.db, string.Empty))
                {
                    return false;
                }

                // 機器別管理基準内容(変更管理)のデータを機器別管理基準内容(トランザクション)に登録
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertManagementStandardsContentTransaction, SqlName.SubDir, condition, this.db, string.Empty))
                {
                    return false;
                }

                // 保全スケジュール(変更管理)のデータを保全スケジュール(トランザクション)に登録
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertMaintainanceScheduleTransaction, SqlName.SubDir, condition, this.db, string.Empty))
                {
                    return false;
                }

                // 登録を行う最新の保全項目データを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetHistoryManagementStandardsList, out string outSql, new List<string>() { "ComponentId" });
                IList<Dao.managementStandardsResult> managementStandardsResults = db.GetListByDataClass<Dao.managementStandardsResult>(outSql, condition);
                if (managementStandardsResults == null || managementStandardsResults.Count == 0)
                {
                    // 取得できない場合はエラー
                    return false;
                }
                Dao.managementStandardsResult managementStandardsInfo = managementStandardsResults[0];

                // 保全スケジュール詳細
                if (!insertMainteScheduleDetail(historyManagement, condition, managementStandardsInfo, now, userId))
                {
                    return false;
                }

                // 同一点検種別のスケジュール基準更新
                if (!updateManagementContentKindManage(condition, managementStandardsInfo, now, out bool maintainanceKindManage))
                {
                    return false;
                }

                // 点検種別毎管理機器なら同点検種の周期なども変更
                if (!updateMaintainanceKindManage(historyManagement, condition, managementStandardsInfo, maintainanceKindManage, now, userId))
                {
                    return false;
                }

                return true;
            }

            // 承認時処理 保全項目一覧の項目編集
            bool updateTransactionTableComponent(TMQUtil.HistoryManagement historyManagement, Dao.historyManagmentDetail condition)
            {
                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref condition, now, userId, userId);

                // 登録を行う最新の保全項目データを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetHistoryManagementStandardsList, out string outSql, new List<string>() { "ComponentId" });
                IList<Dao.managementStandardsResult> managementStandardsResults = db.GetListByDataClass<Dao.managementStandardsResult>(outSql, condition);
                if (managementStandardsResults == null || managementStandardsResults.Count == 0)
                {
                    // 取得できない場合はエラー
                    return false;
                }
                Dao.managementStandardsResult managementStandardsInfo = managementStandardsResults[0];

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref managementStandardsInfo, now, userId, userId);

                // 変更前データ取得
                IList<Dao.managementStandardsResult> oldResuts = getManagementStandardsData<Dao.managementStandardsResult>(SqlName.SubDirMachine, SqlName.Detail.GetManagementStandardDetail, managementStandardsInfo);

                // 周期変更有チェック
                bool cycleChangeFlg = false;
                // 周期の変更有無判定
                if (managementStandardsInfo.CycleYear != oldResuts[0].CycleYear ||
                    managementStandardsInfo.CycleMonth != oldResuts[0].CycleMonth ||
                    managementStandardsInfo.CycleDay != oldResuts[0].CycleDay ||
                    managementStandardsInfo.StartDate != oldResuts[0].StartDate)
                {
                    cycleChangeFlg = true;
                }

                // 周期ありフラグセット
                if ((managementStandardsInfo.CycleYear == null || managementStandardsInfo.CycleYear == 0) &&
                    (managementStandardsInfo.CycleMonth == null || managementStandardsInfo.CycleMonth == 0) &&
                    (managementStandardsInfo.CycleDay == null || managementStandardsInfo.CycleDay == 0))
                {
                    // 周期有りフラグを設定
                    managementStandardsInfo.IsCyclic = false;
                }
                else
                {
                    // 周期有りフラグを設定
                    managementStandardsInfo.IsCyclic = true;
                }

                // 機器別管理基準部位変更管理情報を取得
                ComDao.HmMcManagementStandardsComponentEntity component = new();
                component = component.GetEntity(condition.HmManagementStandardsComponentId, this.db);

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref component, now, userId, userId);

                // 機器別管理基準部位(変更管理)のデータを機器別管理基準部位(トランザクション)に更新
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateManagementStandardsComponent, SqlName.SubDirMachine, component, this.db, string.Empty))
                {
                    return false;
                }

                // 機器別管理基準内容変更管理情報を取得
                ComDao.HmMcManagementStandardsContentEntity content = new();
                content = content.GetEntity(condition.HmManagementStandardsContentId, this.db);

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref content, now, userId, userId);

                // 機器別管理基準内容(変更管理)のデータを機器別管理基準内容(トランザクション)に更新
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateManagementStandardsContent, SqlName.SubDirMachine, content, this.db, string.Empty))
                {
                    return false;
                }

                // 同一点検種別のスケジュール基準更新
                if (!updateManagementContentKindManage(condition, managementStandardsInfo, now, out bool maintainanceKindManage))
                {
                    return false;
                }

                // 周期・開始日の変更がある場合
                if (cycleChangeFlg)
                {
                    // 「作成済み保全スケジュール詳細データ(開始日以降)」削除
                    if (!deleteMachineInfoDataExists(managementStandardsInfo, SqlName.SubDirMachine, SqlName.Detail.DeleteMaintainanceScheduleDetailRemake))
                    {
                        return false;
                    }

                    // 変更後開始日 < 変更前開始日であれば変更前開始日レコード削除
                    if (managementStandardsInfo.StartDate <= oldResuts[0].StartDate)
                    {
                        // 入力された日付以降の日付で設定されていた保全スケジュールデータは削除
                        if (!deleteMachineInfoDataExists(managementStandardsInfo, SqlName.SubDirMachine, SqlName.Detail.DeleteMaintainanceScheduleRemake))
                        {
                            return false;
                        }
                    }

                    // 保全スケジュールにデータ新規作成
                    (bool returnFlag, long id) maintainanceSchedule = registInsertDbScheduleDetail<Dao.managementStandardsResult>(managementStandardsInfo, SqlName.Detail.InsertMaintainanceSchedule);
                    if (!maintainanceSchedule.returnFlag)
                    {
                        return false;
                    }

                    // 採番した保全スケジュールを条件に設定
                    condition.MaintainanceScheduleId = maintainanceSchedule.id;

                    // 保全スケジュール詳細にデータ新規作成
                    if (!insertMainteScheduleDetail(historyManagement, condition, managementStandardsInfo, now, userId))
                    {
                        return false;
                    }

                }

                // 点検種別毎管理機器なら同点検種の周期なども変更
                if (!updateMaintainanceKindManage(historyManagement, condition, managementStandardsInfo, maintainanceKindManage, now, userId))
                {
                    return false;
                }

                return true;
            }

            // 承認時処理 保全項目一覧の削除
            bool deleteTransactionTableComponent(Dao.historyManagmentDetail condition)
            {
                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref condition, now, userId, userId);

                // 削除SLQリスト
                List<string> sqlIdList = new()
                {
                    SqlName.Detail.DeleteMaintainanceScheduleDetailSingle,  // 保全スケジュール詳細
                    SqlName.Detail.DeleteMaintainanceScheduleSingle,        // 保全スケジュール
                    SqlName.Detail.DeleteManagementStandardsContentSingle,  // 機器別管理基準内容
                    SqlName.Detail.DeleteManagementStandardsComponentSingle // 機器別管理基準部位
                };

                // 各テーブルのレコードを削除する
                foreach (string sqlId in sqlIdList)
                {
                    // SQL取得
                    TMQUtil.GetFixedSqlStatement(SqlName.SubDirMachine, sqlId, out string sql);

                    // 削除SQL実行
                    if (this.db.Regist(sql, condition) < 0)
                    {
                        return false;
                    }
                }

                // 添付情報の件数を取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDirMachine, SqlName.Detail.GetAttachmentCount, out string cntSql);
                if (this.db.GetCount(cntSql, new { FunctionTypeId = (int)TMQConst.Attachment.FunctionTypeId.Content, KeyId = condition.ManagementStandardsContentId }) <= 0)
                {
                    // 添付情報が存在しない場合はここで終了
                    return true;
                }

                // 添付情報が存在する場合は削除
                if (!new ComDao.AttachmentEntity().DeleteByKeyId(TMQConst.Attachment.FunctionTypeId.Content, (int)condition.ManagementStandardsContentId, this.db))
                {
                    return false;
                }

                return true;
            }
        }

        #region 保全項目一覧関連 承認時に実行
        /// <summary>
        /// 保全スケジュール詳細データ新規登録
        /// </summary>
        /// <param name="historyManagement">変更管理登録クラス</param>
        /// <param name="condition">変更管理詳細情報</param>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <param name="now">現在日時</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool insertMainteScheduleDetail(TMQUtil.HistoryManagement historyManagement, Dao.historyManagmentDetail condition, Dao.managementStandardsResult managementStandardsInfo, DateTime now, int userId)
        {
            // 保全スケジュール詳細データ
            ComDao.McMaintainanceScheduleDetailEntity scheduleDetail = new();

            //テーブル共通項目を設定
            setExecuteConditionByDataClassCommon(ref scheduleDetail, now, userId, userId);

            // 登録情報を設定
            scheduleDetail.SummaryId = null;                                          // 保全活動件名ID
            scheduleDetail.Complition = false;                                        // 完了フラグ
            scheduleDetail.MaintainanceScheduleId = condition.MaintainanceScheduleId; // 保全スケジュールID
            scheduleDetail.SequenceCount = 1;                                         // 繰り返し回数
            scheduleDetail.ScheduleDate = managementStandardsInfo.StartDate;          // スケジュール日

            // 周期の有無を判定
            if (managementStandardsInfo.IsCyclic == true)
            {
                // スケジュール作成上限取得
                string strMaxYear = getItemExData(1, 2, (int)TMQConst.MsStructure.GroupId.MakeScheduleYear, 0);

                // 対象機器の工場IDを取得
                int factoryId = 0;
                if (condition.HmMachineId <= 0)
                {
                    // 機器に変更管理が紐付かない場合は機番情報テーブルより取得
                    ComDao.McMachineEntity machine = new();
                    machine = machine.GetEntity(condition.MachineId, this.db);
                    factoryId = historyManagement.getFactoryId((int)machine.LocationStructureId);
                }
                else
                {
                    // 機器に変更管理が存在する場合は機番情報変更管理テーブルより取得
                    ComDao.HmMcMachineEntity hmachine = new();
                    hmachine = hmachine.GetEntity(condition.HmMachineId, this.db);
                    factoryId = historyManagement.getFactoryId((int)hmachine.LocationStructureId);
                }

                // 工場毎年度終了月取得
                string strstartMonth = getItemExData(1, 2, (int)TMQConst.MsStructure.GroupId.BeginningMonth, factoryId);
                if (strstartMonth == null)
                {
                    // 工場毎の設定が取得できなければ工場共通
                    strstartMonth = getItemExData(1, 2, (int)TMQConst.MsStructure.GroupId.BeginningMonth, 0);
                }
                if (int.Parse(strstartMonth) < 10)
                {
                    strstartMonth = "0" + strstartMonth;
                }

                DateTime endDate = DateTime.Parse(strMaxYear + "/" + strstartMonth + "/01");
                endDate = endDate.AddYears(1).AddDays(-1);

                DateTime tmpStardDate = (DateTime)managementStandardsInfo.StartDate;

                (bool returnFlag, long id) maintainanceSchedule = (true, -1);
                // 開始日から終了日までスケジュール詳細作成
                while (tmpStardDate <= endDate.Date)
                {
                    // SQL実行
                    if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertMaintainanceScheduleDetail, SqlName.SubDirMachine, scheduleDetail, this.db, string.Empty))
                    {
                        return false;
                    }

                    // 回数カウントアップ
                    scheduleDetail.SequenceCount = scheduleDetail.SequenceCount + 1;

                    // スケジュール日設定
                    if (managementStandardsInfo.CycleYear != null && managementStandardsInfo.CycleYear > 0)
                    {
                        tmpStardDate = tmpStardDate.AddYears((int)managementStandardsInfo.CycleYear);
                    }
                    if (managementStandardsInfo.CycleMonth != null && managementStandardsInfo.CycleMonth > 0)
                    {
                        tmpStardDate = tmpStardDate.AddMonths((int)managementStandardsInfo.CycleMonth);
                    }
                    if (managementStandardsInfo.CycleDay != null && managementStandardsInfo.CycleDay > 0)
                    {
                        tmpStardDate = tmpStardDate.AddDays((int)managementStandardsInfo.CycleDay);
                    }
                    scheduleDetail.ScheduleDate = tmpStardDate;
                }
            }
            else
            {
                // SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.InsertMaintainanceScheduleDetail, SqlName.SubDirMachine, scheduleDetail, this.db, string.Empty))
                {
                    return false;
                }
            }

            return true;

            /// <summary>
            /// アイテム拡張マスタから拡張データを取得する
            /// </summary>
            /// <param name="seq">連番</param>
            /// <param name="dataType">データタイプ</param>
            /// <param name="factoryId">工場ID</param>
            /// <returns>拡張データ</returns>
            string getItemExData(short seq, short dataType, int? structureGroupId, int? factoryId)
            {
                string val = null;

                //構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                //構成グループID
                param.StructureGroupId = structureGroupId;
                //連番
                param.Seq = seq;
                //データタイプ
                //param.DataType = dataType;
                //構成アイテム、アイテム拡張マスタ情報取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
                if (list != null)
                {
                    //取得情報から拡張データを取得
                    var result = list.Exists(x => x.FactoryId == factoryId);
                    if (result)
                    {
                        val = list.Where(x => x.FactoryId == factoryId).Select(x => x.ExData).First();
                    }
                }
                return val;
            }
        }

        /// <summary>
        /// 同一点検種別のスケジュール基準更新
        /// </summary>
        /// <param name="condition">変更管理詳細情報</param>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool updateManagementContentKindManage(Dao.historyManagmentDetail condition, Dao.managementStandardsResult managementStandardsInfo, DateTime now, out bool maintainanceKindManage)
        {
            // 点検種別毎管理を取得
            maintainanceKindManage = false;
            if (condition.HmEquipmentId <= 0)
            {
                // 機器に変更管理が紐付かない場合は機器情報テーブルより取得
                ComDao.McEquipmentEntity equipment = new();
                equipment = equipment.GetEntity(condition.EquipmentId, this.db);
                maintainanceKindManage = equipment.MaintainanceKindManage;
            }
            else
            {
                // 機器に変更管理が存在する場合は機器情報変更管理テーブルより取得
                ComDao.HmMcEquipmentEntity hequipment = new();
                hequipment = hequipment.GetEntity(condition.HmEquipmentId, this.db);
                maintainanceKindManage = hequipment.MaintainanceKindManage;
            }

            // 点検種別毎管理
            if (maintainanceKindManage)
            {
                // 同一点検種別スケジュール更新処理
                if (!updateManagementContentKindManageOtherSchedule(managementStandardsInfo, now))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 同一点検種別スケジュール更新
        /// </summary>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はTrue</returns>
        private bool updateManagementContentKindManageOtherSchedule(Dao.managementStandardsResult managementStandardsInfo, DateTime now)
        {
            // 同一点検種別データ取得
            IList<Dao.managementStandardsResult> oldResuts = getManagementStandardsData<Dao.managementStandardsResult>(SqlName.SubDirMachine, SqlName.Detail.GetMaintainanceKindManageDataUpdContent, managementStandardsInfo);

            // 取得した同一点検種別データを更新
            foreach (Dao.managementStandardsResult regRes in oldResuts)
            {
                // 登録条件
                Dao.managementStandardsResult condition = regRes;
                condition.ScheduleTypeStructureId = managementStandardsInfo.ScheduleTypeStructureId; // スケジュール管理基準ID

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref condition, now, int.Parse(this.UserId), int.Parse(this.UserId));

                // 更新SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.UpdateManagementStandardsContentSchedule, SqlName.SubDirMachine, condition, this.db, string.Empty))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 同一点検種別の周期等更新
        /// </summary>
        /// <param name="historyManagement">変更管理登録クラス</param>
        /// <param name="condition">変更管理詳細情報</param>
        /// <param name="managementStandardsInfo">保全項目情報</param>
        /// <param name="maintainanceKindManage">点検種別毎管理</param>
        /// <param name="now">現在日時</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool updateMaintainanceKindManage(TMQUtil.HistoryManagement historyManagement, Dao.historyManagmentDetail condition, Dao.managementStandardsResult managementStandardsInfo, bool maintainanceKindManage, DateTime now, int userId)
        {
            // 点検種別毎管理ではない場合は何もしない
            if (!maintainanceKindManage)
            {
                return true;
            }

            // 最新DBデータ取得
            IList<Dao.managementStandardsResult> oldResuts = getManagementStandardsData<Dao.managementStandardsResult>(SqlName.SubDirMachine, SqlName.Detail.GetMaintainanceKindManageData, managementStandardsInfo);

            foreach (Dao.managementStandardsResult result in oldResuts)
            {
                Dao.managementStandardsResult regRes = result;

                var oldDate = regRes.StartDate;
                regRes.StartDate = managementStandardsInfo.StartDate;

                // 「作成済み保全スケジュール詳細データ(開始日以降)」削除
                if (!deleteMachineInfoDataExists(regRes, SqlName.SubDirMachine, SqlName.Detail.DeleteMaintainanceScheduleDetailRemake))
                {
                    return false;
                }

                // 変更後開始日 < 変更前開始日であれば変更前開始日レコード削除
                if (managementStandardsInfo.StartDate <= oldDate)
                {
                    // 入力された日付以降の日付で設定されていた保全スケジュールデータは削除
                    if (!deleteMachineInfoDataExists(regRes, SqlName.SubDirMachine, SqlName.Detail.DeleteMaintainanceScheduleRemake))
                    {
                        return false;
                    }
                }

                // 周期ありフラグセット
                if ((managementStandardsInfo.CycleYear == null || managementStandardsInfo.CycleYear == 0) &&
                    (managementStandardsInfo.CycleMonth == null || managementStandardsInfo.CycleMonth == 0) &&
                    (managementStandardsInfo.CycleDay == null || managementStandardsInfo.CycleDay == 0))
                {
                    // 周期無し
                    regRes.IsCyclic = false;
                }
                else
                {
                    // 周期あり
                    regRes.IsCyclic = true;
                }
                regRes.CycleYear = managementStandardsInfo.CycleYear;   // 周期(年)
                regRes.CycleMonth = managementStandardsInfo.CycleMonth; // 周期(年)
                regRes.CycleDay = managementStandardsInfo.CycleDay;     // 周期(年)
                regRes.DispCycle = managementStandardsInfo.DispCycle;   // 表示周期
                regRes.StartDate = managementStandardsInfo.StartDate;   // 開始日

                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref regRes, now, userId, userId);

                // 保全スケジュールにデータ新規作成
                (bool returnFlag, long id) maintainanceSchedule = registInsertDbScheduleDetail<Dao.managementStandardsResult>(regRes, SqlName.Detail.InsertMaintainanceSchedule);
                if (!maintainanceSchedule.returnFlag)
                {
                    return false;
                }

                // 採番した保全スケジュールを条件に設定
                condition.MaintainanceScheduleId = maintainanceSchedule.id;

                // 保全スケジュール詳細にデータ新規作成
                if (!insertMainteScheduleDetail(historyManagement, condition, managementStandardsInfo, now, userId))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 保全スケジュール詳細の新規登録処理(採番したキーも取得)
        /// </summary>
        /// <typeparam name="T">条件のデータクラス</typeparam>
        /// <param name="registInfo">登録情報</param>
        /// <param name="sqlName">SQL名</param>
        /// <returns>(エラーの場合はFalse、採番したキー)</returns>
        private (bool returnFlag, long id) registInsertDbScheduleDetail<T>(T registInfo, string sqlName)
        {
            string sql;
            // SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDirMachine, sqlName, out sql))
            {
                return (false, -1);
            }

            long returnId = db.RegistAndGetKeyValue<long>(sql, out bool isError, registInfo);
            return (!isError, returnId);
        }

        /// <summary>
        /// 保全項目データ取得
        /// </summary>
        /// <typeparam name="T">取得データクラス</typeparam>
        /// <param name="whereParam">パラメータ</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private IList<T> getManagementStandardsData<T>(string subDir, string sqlFile, dynamic whereParam, List<string> uncommentList = null)
        {
            // SQL取得
            TMQUtil.GetFixedSqlStatement(subDir, sqlFile, out string sql, uncommentList);

            // SQL実行
            IList<T> results = db.GetListByDataClass<T>(sql, whereParam);

            return results;
        }
        #endregion
        #endregion

        /// <summary>
        /// Ajax通信排他チェック(機器に対する「承認済」以外のデータ有無を取得する)
        /// </summary>
        /// <returns>データが存在する場合はFalse</returns>
        private int judgeStatusCntExceptApproved()
        {
            // 検索条件を取得
            Dao.detailSearchCondition condition = getDetailSearchCondition();

            // 返り値を設定(実際は画面に設定しない)
            Dao.searchResult result = new() { StatusCntExceptApproved = checkHistoryManagement(condition.MachineId) };
            SetFormByDataClass(ConductInfo.FormDetail.ControlId.Machine, new List<Dao.searchResult>() { result });

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// Ajax通信排他チェック(変更管理テーブルの排他チェック)
        /// </summary>
        /// <returns>排他エラーの場合はFalse</returns>
        private int checkHisoryManagemtntExclusive()
        {
            // 排他チェック用の一覧のID
            string ctrlId = ConductInfo.FormDetail.ControlId.Machine;

            // 排他チェック(変更管理)
            bool resultFlg = true;
            resultFlg = checkExclusiveSingleForTable(ctrlId, new List<string>() { "hm_history_management" }, ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId));

            // 返り値を設定(実際は画面に設定しない)
            Dao.searchResult result = new() { StatusCntExceptApproved = resultFlg };
            SetFormByDataClass(ConductInfo.FormDetail.ControlId.Machine, new List<Dao.searchResult>() { result });

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索結果を一覧に設定する
        /// </summary>
        /// <param name="groupNo">一覧のグループ番号</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool setSearchResult(short groupNo, IList<Dao.searchResult> results)
        {
            // 画面定義のグループ番号よりコントロールグループIDを取得
            List<string> ctrlIdList = getResultMappingInfoByGrpNo(groupNo).CtrlIdList;

            // グループ番号内の一覧に対して繰り返し値を設定する
            foreach (var ctrlId in ctrlIdList)
            {
                // 画面項目に値を設定
                if (!SetFormByDataClass(ctrlId, results))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNoList">取得するグループ番号のリスト</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(List<short> groupNoList, DateTime now, bool isSearchconditioinDictionary = false)
        where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 相称型を使用することで、色々な型を呼出元で宣言することができる

            T resultInfo = new();

            foreach (short groupNo in groupNoList)
            {
                // 登録対象グループの画面項目定義の情報
                var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

                // 対象グループのコントロールIDの結果情報のみ抽出
                var ctrlIdList = grpMapInfo.CtrlIdList;
                List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(isSearchconditioinDictionary ? this.searchConditionDictionary : this.resultInfoDictionary, ctrlIdList);

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
            }
            return resultInfo;
        }

        /// <summary>
        /// 検索時に必要な一時テーブルを作成
        /// </summary>
        private void createTranslationTempTbl()
        {
            // 翻訳の一時テーブルを作成
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);

            // 添付情報作成
            listPf.GetAttachmentSql(new List<FunctionTypeId> { FunctionTypeId.Machine, FunctionTypeId.Equipment, FunctionTypeId.Content });

            // 翻訳する構成グループのリスト
            var structuregroupList = new List<GroupId>
            {
                GroupId.Location,
                GroupId.Job,
                GroupId.Conservation,
                GroupId.Manufacturer,
                GroupId.ApplicableLaws,
                GroupId.MachineLevel,
                GroupId.Importance,
                GroupId.UseSegment,
                GroupId.ApplicationStatus,
                GroupId.ApplicationDivision,
                GroupId.SiteMaster,
                GroupId.MaintainanceDivision,
                GroupId.InspectionDetails,
                GroupId.MaintainanceKind,
                GroupId.ScheduleType

            };
            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            listPf.RegistTempTable(); // 登録
        }
        #endregion

        #region 見直し予定
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
            //this.ResultList = new();

            //// 実装の際は、不要な帳票に対する分岐は削除して構いません

            //bool outputExcel = false;
            //bool outputPdf = false;

            //switch (this.CtrlId)
            //{
            //    case TargetCtrlId.Button.ReportExcel:
            //        outputExcel = true;
            //        break;
            //    case TargetCtrlId.Button.ReportPdf:
            //        outputExcel = true;
            //        outputPdf = true;
            //        break;
            //    case TargetCtrlId.Button.ReportCsv:
            //        break;
            //    default:
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「コントロールIDが不正です。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID911100001 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        return ComConsts.RETURN_RESULT.NG;
            //}

            //// ファイル名
            //string baseFileName = string.Format("{0:yyyyMMddHHmmss}_{1}_{2}", DateTime.Now, this.ConductId, this.CtrlId);

            //// データ検索
            //var resultList = searchListForReport();
            //if (resultList == null || resultList.Count == 0)
            //{
            //    // 警告メッセージで終了
            //    this.Status = CommonProcReturn.ProcStatus.Warning;
            //    // 「該当データがありません。」
            //    this.MsgId = GetResMessage(ComRes.ID.ID941060001);
            //    return result;
            //}

            //string msg = string.Empty;
            //if (outputExcel)
            //{
            //    // Excel出力が必要な場合

            //    // マッピング情報生成
            //    // 以下はA列から順番にカラム名リストに一致するデータを行単位でマッピングする
            //    List<CommonExcelPrtInfo> prtInfoList = CommonExcelUtil.CommonExcelUtil.CreateMappingList(resultList, "Sheet1", 2, "A");

            //    // コマンド情報生成
            //    // セルの結合や罫線を引く等のコマンド実行が必要な場合はここでセットする。不要な場合はnullでOK
            //    List<CommonExcelCmdInfo> cmdInfoList = null;

            //    // Excel出力実行
            //    var excelStream = new MemoryStream();
            //    if (!CommonExcelUtil.CommonExcelUtil.CreateExcelFile(TemplateName.Report, this.UserId, prtInfoList, cmdInfoList, ref excelStream, ref msg))
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「Excel出力に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911040001 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        logger.Error(msg);

            //        return ComConsts.RETURN_RESULT.NG;
            //    }

            //    if (outputPdf)
            //    {
            //        // PDF出力の場合

            //        // PDF出力実行
            //        var pdfStream = new MemoryStream();
            //        try
            //        {
            //            if (!CommonExcelUtil.CommonExcelUtil.CreatePdfFile(excelStream, ref pdfStream, ref msg))
            //            {
            //                pdfStream.Close();

            //                this.Status = CommonProcReturn.ProcStatus.Error;
            //                // 「PDF出力に失敗しました。」
            //                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911270004 });

            //                // エラーログ出力
            //                logger.Error(this.MsgId);
            //                logger.Error(msg);

            //                return ComConsts.RETURN_RESULT.NG;
            //            }
            //        }
            //        finally
            //        {
            //            // ExcelファイルのStreamは閉じる
            //            excelStream.Close();
            //        }
            //        this.OutputFileType = "3";  // PDF
            //        this.OutputFileName = baseFileName + ".pdf";
            //        this.OutputStream = pdfStream;
            //    }
            //    else
            //    {
            //        // Excel出力の場合
            //        this.OutputFileType = "1";  // Excel
            //        this.OutputFileName = baseFileName + ".xlsx";
            //        this.OutputStream = excelStream;
            //    }
            //}
            //else
            //{
            //    // CSV出力の場合

            //    // CSV出力実行
            //    Stream csvStream = new MemoryStream();
            //    if (!CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.ExportCsvFile(
            //        resultList, Encoding.GetEncoding("Shift-JIS"), out csvStream, out msg))
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「CSV出力に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120007 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        logger.Error(msg);

            //        return ComConsts.RETURN_RESULT.NG;
            //    }
            //    this.OutputFileType = "2";  // CSV
            //    this.OutputFileName = baseFileName + ".csv";
            //    this.OutputStream = csvStream;
            //}

            //// 正常終了
            //this.Status = CommonProcReturn.ProcStatus.Valid;

            return ComConsts.RETURN_RESULT.OK;

        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int UploadImpl()
        {
            // 実装の際は、不要な帳票に対する分岐は削除して構いません

            int result = 0;
            //string msg = string.Empty;
            //this.ResultList = new();

            //List<string[,]> uploadList = new List<string[,]>();

            //List<Stream> excelList = new List<Stream>();
            //List<Stream> csvList = new List<Stream>();
            //foreach (var file in this.InputStream)
            //{
            //    switch (Path.GetExtension(file.FileName))
            //    {
            //        case ComUtil.FileExtension.Excel:   // Excelファイル
            //            excelList.Add(file.OpenReadStream());
            //            break;
            //        case ComUtil.FileExtension.CSV:    // CSVファイル
            //            csvList.Add(file.OpenReadStream());
            //            break;
            //        default:
            //            this.Status = CommonProcReturn.ProcStatus.Error;
            //            // 「ファイルの種類が不正です。」
            //            this.MsgId = GetResMessage(ComRes.ID.ID941280004);

            //            // エラーログ出力
            //            logger.Error(this.MsgId);
            //            return ComConsts.RETURN_RESULT.NG;
            //    }
            //}

            //if (excelList.Count > 0)
            //{
            //    // Excelファイル読込
            //    if (!CommonExcelUtil.CommonExcelUtil.ReadExcelFiles(excelList, "", "", ref uploadList, ref msg))
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「Excel取込に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911040002 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        logger.Error(msg);

            //        return ComConsts.RETURN_RESULT.NG;
            //    }
            //}

            //if (csvList.Count > 0)
            //{
            //    // CSVファイル読込
            //    if (!ComUtil.ImportCsvFiles(
            //        csvList, true, Encoding.GetEncoding(CommonConstants.UPLOAD_INFILE_CHAR_CODE), ref uploadList, ref msg))
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「CSV取込に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120008 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        logger.Error(msg);

            //        return ComConsts.RETURN_RESULT.NG;
            //    }
            //}

            //// ↓↓↓ コントロールIDで取込対象を切り分ける場合 ↓↓↓
            ////switch (this.CtrlId)
            ////{
            ////    case TargetCtrlId.UploadExcel:
            ////        // Excelファイル読込
            ////        if (!CommonExcelUtil.CommonExcelUtil.ReadExcelFiles(excelList, "", "", ref uploadList, ref msg))
            ////        {
            ////            this.Status = CommonProcReturn.ProcStatus.Error;
            ////            // 「Excel取込に失敗しました。」
            ////            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911040002 });

            ////            // エラーログ出力
            ////            logger.Error(this.MsgId);
            ////            logger.Error(msg);

            ////            return -1;
            ////        }
            ////        break;
            ////    case TargetCtrlId.UploadCsv:
            ////        // CSVファイル読込
            ////        if (!ComUtil.ImportCsvFiles(
            ////            csvList, true, Encoding.GetEncoding("Shift-JIS"), ref uploadList, ref msg))
            ////        {
            ////            this.Status = CommonProcReturn.ProcStatus.Error;
            ////            // 「CSV取込に失敗しました。」
            ////            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120008 });

            ////            // エラーログ出力
            ////            logger.Error(this.MsgId);
            ////            logger.Error(msg);

            ////            return -1;
            ////        }
            ////        break;
            ////    default:
            ////        this.Status = CommonProcReturn.ProcStatus.Error;
            ////        // 「コントロールIDが不正です。」
            ////        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID911100001 });

            ////        // エラーログ出力
            ////        logger.Error(this.MsgId);
            ////        return -1;
            ////}
            //// ↑↑↑ コントロールIDで取込対象を切り分ける場合 ↑↑↑

            //// ↓↓↓ 表示用データを返却する場合 ↓↓↓
            //// 表示用データを返却する場合、コントロールID指定で変換する
            //var resultList = ConvertToUploadResultDictionary("[コントロールID]", uploadList);
            //// 取込結果の設定
            //SetJsonResult(resultList);
            //// ↑↑↑ 表示用データを返却する場合 ↑↑↑

            //// ↓↓↓ 登録処理を実行する場合 ↓↓↓
            ////// 登録処理を実行する場合、コントロール未指定で変換する
            ////this.resultInfoDictionary = ConvertToUploadResultDictionary("", uploadList);
            ////// トランザクション開始
            ////using (var transaction = this.db.Connection.BeginTransaction())
            ////{
            ////    try
            ////    {
            ////        // 登録処理実行
            ////        result = RegistImpl();

            ////        if (result > 0)
            ////        {
            ////            // コミット
            ////            transaction.Commit();
            ////        }
            ////        else
            ////        {
            ////            // ロールバック
            ////            transaction.Rollback();
            ////        }
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        if (transaction != null)
            ////        {
            ////            // ロールバック
            ////            transaction.Rollback();
            ////        }
            ////        this.Status = CommonProcReturn.ProcStatus.Error;
            ////        // 「取込処理に失敗しました。」
            ////        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200004 });
            ////        this.LogNo = string.Empty;

            ////        logger.Error(this.MsgId, ex);
            ////        return -1;
            ////    }
            ////}
            //// ↑↑↑ 登録処理を実行する場合 ↑↑↑

            //// 正常終了
            //this.Status = CommonProcReturn.ProcStatus.Valid;

            return result;
        }
        #endregion

    }
}