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
using Microsoft.AspNetCore.Http;

using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_LN0001.BusinessLogicDataClass_LN0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using SchedulePlanContent = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.SchedulePlanContent;
using ScheduleStatus = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleStatus;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using FunctionTypeId = CommonTMQUtil.CommonTMQConstants.Attachment.FunctionTypeId;
using TMQExcelPort = CommonTMQUtil.CommonTMQUtil.ComExcelPort;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;
using AutoMapper;

namespace BusinessLogic_LN0001
{
    /// <summary>
    /// 件名別長期計画
    /// </summary>
    public partial class BusinessLogic_LN0001 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数

        /// <summary>GetDetailWithのアンコメント用文字列</summary>
        private const string UnCommentWordOfGetDetailWith = "UnComp";
        /// <summary>GetListのアンコメント用文字列</summary>
        private const string UnCommentWordOfGetList = "UnExcelPort";

        /// <summary>スケジュール日時条件用フォーマット</summary>
        private const string ScheduleDateConditionFormat = "yyyyMM";

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"LongPlan";

            /// <summary>
            /// 一覧画面で使用するSQL
            /// </summary>
            public static class List
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\List";
                /// <summary>一覧情報取得SQL</summary>
                public const string GetList = "GetLongPlanList";
                /// <summary>一覧スケジュール情報取得SQL</summary>
                public const string GetListSchedule = "GetLongPlanSchedule";
                /// <summary>一覧情報取得の一時テーブル作成SQL</summary>
                public const string CreateTempTable = "CreateTableTempGetLongPlanList";
                /// <summary>一覧情報取得の一時テーブル登録SQL</summary>
                public const string InsertTempTable = "InsertTempGetLongPlanList";

            }
            /// <summary>
            /// 共通
            /// </summary>
            public static class Common
            {
                /// <summary>長期計画登録</summary>
                public const string InsertLongPlan = "InsertLongPlan";
                /// <summary>長期計画更新</summary>
                public const string UpdateLongPlan = "UpdateLongPlan";
                /// <summary>長期計画削除</summary>
                public const string DeleteLongPlan = "DeleteLongPlan";
            }

            /// <summary>
            /// 入力チェック
            /// </summary>
            public static class InputCheck
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\InputCheck";
                /// <summary>入力チェック用長計件名情報取得</summary>
                public const string GetLongPlanInfo = "GetLongPlanInfo";
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
                /// <summary>機器別管理基準内容IDに紐づくスケジュール詳細情報を取得</summary>
                public const string GetScheduleDetailInfo = "GetScheduleDetailInfoFromContentId";
            }

            /// <summary>
            /// 計画一括作成画面
            /// </summary>
            public static class MakePlan
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\MakePlan";
                /// <summary>保全活動件名登録</summary>
                public const string InsertSummary = "InsertSummary";
                /// <summary>保全依頼登録</summary>
                public const string InsertRequest = "InsertRequest";
                /// <summary>保全計画登録</summary>
                public const string InsertPlan = "InsertPlan";
                /// <summary>保全履歴機器登録</summary>
                public const string InsertHistoryMachine = "InsertHistoryMachine";
                /// <summary>保全履歴機器部位登録</summary>
                public const string InsertHistoryInspectionSite = "InsertHistoryInspectionSite";
                /// <summary>保全履歴点検内容登録</summary>
                public const string InsertHistoryInspectionContent = "InsertHistoryInspectionContent";
                /// <summary>保全履歴登録</summary>
                public const string InsertHistory = "InsertHistory";
                /// <summary>保全スケジュール詳細更新</summary>
                public const string UpdateScheduleDetail = "UpdateScheduleDetail";
            }
            /// <summary>
            /// 参照画面
            /// </summary>
            public static class Detail
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\Detail";
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
                /// <summary>保全情報一覧取得(ExcelPort)</summary>
                public const string GetDetailExcelPort = GetDetail + "_ExcelPort";

                /// <summary>保全情報のスケジュール一覧取得SQLの名前共通部分</summary>
                private const string GetSchedule = "GetScheduleList";
                /// <summary>保全情報のスケジュール一覧取得(機器)</summary>
                public const string GetScheduleEquipment = GetSchedule + "_Equipment";
                /// <summary>保全情報のスケジュール一覧取得(部位)</summary>
                public const string GetScheduleInspectionSite = GetSchedule + "_InspectionSite";
                /// <summary>保全情報のスケジュール一覧取得(保全項目)</summary>
                public const string GetScheduleMaintainance = GetSchedule + "_Maintainance";

                /// <summary>スケジュール確定ボタン押下時の更新SQL</summary>
                public const string UpdateSchedule = "UpdateScheduleDetail";

                /// <summary>指示検収票ボタン押下時の出力ファイル情報の取得SQL</summary>
                public const string GetOutputFileInfo = "GetOutputFileInfo";

                /// <summary>行削除入力チェック：機器別管理基準内容に紐づく保全活動作成済みスケジュールの件数を取得</summary>
                public const string GetSummaryScheduleByContent = "GetCountSummaryScheduleByContent";

                /// <summary>長期計画IDより紐づく機器の点検種別管理を取得するSQL</summary>
                public const string GetMaintKindManageByLongPlanId = "GetMaintainanceKindManageByLongPlanId";

            }

            /// <summary>
            /// 保全活動作成画面
            /// </summary>
            public static class MakeMaint
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\MakeMaintainance";
                /// <summary>保全活動件名登録</summary>
                public const string InsertSummary = "InsertSummary";
                /// <summary>保全依頼登録</summary>
                public const string InsertRequest = "InsertRequest";
                /// <summary>保全計画登録</summary>
                public const string InsertPlan = "InsertPlan";
                /// <summary>保全履歴機器登録</summary>
                public const string InsertHistoryMachine = "InsertHistoryMachine";
                /// <summary>保全履歴機器部位登録</summary>
                public const string InsertHistoryInspectionSite = "InsertHistoryInspectionSite";
                /// <summary>保全履歴点検内容登録</summary>
                public const string InsertHistoryInspectionContent = "InsertHistoryInspectionContent";
                /// <summary>保全履歴登録</summary>
                public const string InsertHistory = "InsertHistory";
                /// <summary>保全スケジュール詳細更新</summary>
                public const string UpdateScheduleDetail = "UpdateScheduleDetail";
            }

            /// <summary>
            /// 機器別管理基準選択画面
            /// </summary>
            public static class SelectStandards
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\SelectStandards";
                /// <summary>一覧取得</summary>
                public const string GetList = "GetManagementStandardsContentList";
                /// <summary>登録処理　機器別管理基準内容更新</summary>
                public const string UpdateContent = "UpdateManagementStandardsContent";
            }

            /// <summary>
            /// 予定作業一括延期画面
            /// </summary>
            public static class Postpone
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\Postpone";
                /// <summary>スケジュール詳細更新</summary>
                public const string UpdateScheduleDetail = "UpdateScheduleDetailPostpone";
            }

            /// <summary>
            /// 機器情報
            /// </summary>
            public static class Machine
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = @"Machine";
                /// <summary>保全スケジュール詳細更新</summary>
                public const string InsertScheduleDetail = "InsertMaintainanceScheduleDetail";
            }

            /// <summary>
            /// ExelPort
            /// </summary>
            public static class ExcelPort
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\ExcelPort";
                /// <summary>保全情報一覧取得SQL</summary>
                public const string GetDetail = "GetDetailList";
                /// <summary>保全スケジュール情報取得SQL</summary>
                public const string GetScheduleInfo = "GetScheduleInfo";
                /// <summary>保全スケジュール詳細情報取得SQL(ダウンロード用)</summary>
                public const string GetScheduleForDownload = "GetScheduleListForDownload";
                /// <summary>保全スケジュール詳細情報取得SQL(アップロード用)</summary>
                public const string GetScheduleForUpload = "GetScheduleListForUpload";
                /// <summary>保全スケジュール詳細情報取得SQL(履歴完了データ)</summary>
                public const string GetScheduleComplete = "GetScheduleListComplete";
                /// <summary>長計件名IDに紐づく添付情報の削除SQL</summary>
                public const string DeleteAttachment = "DeleteAttachment";
                /// <summary>保全スケジュール詳細更新</summary>
                public const string InsertScheduleDetail = "InsertMaintainanceScheduleDetail";
                /// <summary>機器別管理基準情報の削除SQL</summary>
                public const string DeleteContent = "DeleteContent";
                /// <summary>機器別管理基準内容に紐づく保全スケジュール詳細情報の削除SQL</summary>
                public const string DeleteScheduleDetail = "DeleteScheduleDetail";
                /// <summary>保全スケジュール詳細の繰り返し回数の最大値取得SQL</summary>
                public const string GetSequenceCountMax = "GetScheduleDetailSequenceCountMax";
                /// <summary>保全スケジュール詳細情報の繰り返し回数更新SQL</summary>
                public const string UpdateSequenceCount = "UpdateScheduleDetailSequenceCount";
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
                    /// 非表示項目
                    /// </summary>
                    public const string HiddenInfo = "BODY_050_00_LST_0";

                    /// <summary>ExcelPortアップロード</summary>
                    public const string ExcelPortUpload = "LIST_000_1";

                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {
                    /// <summary>新規ボタン</summary>
                    public const string New = "btnInsert";
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

                    /// <summary>ExcelPortアップロード</summary>
                    public const string ExcelPortUpload = "LIST_000_1";

                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {
                    /// <summary>編集ボタン</summary>
                    public const string Edit = "btnUpdate";
                    /// <summary>削除ボタン</summary>
                    public const string Delete = "btnDelete";
                    /// <summary>再表示ボタン</summary>
                    public const string Redisplay = "btnRedisplay";
                    /// <summary>複写ボタン</summary>
                    public const string Copy = "btnCopy";
                    /// <summary>スケジュール確定</summary>
                    public const string Schedule = "btnUpdateSchedule";
                    /// <summary>指示検収票</summary>
                    public const string OutputFiles = "btnOutputFiles";
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
            }

            /// <summary>
            /// 計画一括作成
            /// </summary>
            public class FormMakePlan
            {
                public const short FormNo = 3;
                public static class ControlId
                {
                    /// <summary>作成条件</summary>
                    public const string Condition = "BODY_010_00_LST_3";
                    /// <summary>処理対象長計件名ID</summary>
                    public const string LongPlanId = "BODY_030_00_LST_3";
                }
                public static class ButtonId
                {
                    /// <summary>作成ボタン</summary>
                    public const string MakePlan = "btnMakePlan";
                }
            }
            /// <summary>
            /// 保全活動作成
            /// </summary>
            public class FormMakeMaintainance
            {
                /// <summary>フォーム番号</summary>
                public const short FormNo = 4;
                /// <summary>コントロールID</summary>
                public static class ControlId
                {
                    /// <summary>作成条件</summary>
                    public const string Condition = "BODY_000_00_LST_4";
                    /// <summary>保全情報一覧</summary>
                    public const string List = "BODY_010_00_LST_4";
                    /// <summary>点検種別毎保全情報一覧</summary>
                    public const string ListCheck = "BODY_020_00_LST_4";
                }
                /// <summary>ボタンID</summary>
                public static class ButtonId
                {
                    /// <summary>保全活動作成</summary>
                    public const string Make = "MakeMaintainance";
                }
            }
            /// <summary>
            /// 機器別管理基準選択
            /// </summary>
            public class FormSelectStandards
            {
                /// <summary>フォーム番号</summary>
                public const short FormNo = 5;

                /// <summary>検索条件のグループ番号</summary>
                public const short SearchConditionGroupNo = 501;

                /// <summary>コントロールID</summary>
                public static class ControlId
                {
                    /// <summary>機器選択一覧</summary>
                    public const string List = "BODY_040_00_LST_5";
                    /// <summary>非表示項目</summary>
                    public const string Hide = "BODY_060_00_LST_5";
                    /// <summary>検索条件：場所</summary>
                    public const string Location = "COND_000_00_LST_5";
                    /// <summary>検索条件：職種機種</summary>
                    public const string Job = "COND_010_00_LST_5";

                }
            }
            /// <summary>
            /// 予算出力
            /// </summary>
            public class FormBudgetOutput
            {
                public const short FormNo = 6;
                public static class ControlId
                {
                    /// <summary>出力条件</summary>
                    public const string OutputCondition = "BODY_000_00_LST_6";
                    /// <summary>処理対象長計件名ID</summary>
                    public const string LongPlanId = "BODY_020_00_LST_6";
                    /// <summary>スケジュール表示条件</summary>
                    public const string ScheduleCondition = "BODY_030_00_LST_6";
                }
                public static class ButtonId
                {
                    /// <summary>出力ボタン</summary>
                    public const string BudgetOutput = "btnOutput";
                }
            }
            /// <summary>
            /// 予定作業一括延期
            /// </summary>
            public class FormPostpone
            {
                /// <summary>フォーム番号</summary>
                public const short FormNo = 7;
                /// <summary>コントロールID</summary>
                public static class ControlId
                {
                    /// <summary>作成条件</summary>
                    public const string Condition = "BODY_000_00_LST_7";
                    /// <summary>保全情報一覧</summary>
                    public const string List = "BODY_010_00_LST_7";
                    /// <summary>点検種別毎保全情報一覧</summary>
                    public const string ListCheck = "BODY_020_00_LST_7";
                }
                /// <summary>ボタンID</summary>
                public static class ButtonId
                {
                    /// <summary>一括延期</summary>
                    public const string Postpone = "Postpone";
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

        private static class ExcelPort
        {
            public static class LongPlan
            {
                public static class ColumnNo
                {
                    /// <summary>送信時処理</summary>
                    public const int SendProcess = 2;

                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_LN0001() : base()
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
                    if (compareId.IsStartId(ConductInfo.FormMakeMaintainance.ButtonId.Make))
                    {
                        // 保全活動画面の保全活動作成ボタンの場合
                        detailType = DetailDispType.Search;
                    }
                    if (compareId.IsStartId(ConductInfo.FormPostpone.ButtonId.Postpone))
                    {
                        // 予定作業一括延期画面の予定作業一括延期画面ボタンの場合
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
                    if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.Edit))
                    {
                        // 修正(参照画面より起動)
                        editType = EditDispType.Update;
                    }
                    if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.Copy))
                    {
                        // 複写(参照画面より起動)
                        editType = EditDispType.Copy;
                    }
                    // 初期化処理
                    if (!initEdit(editType))
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormMakePlan.FormNo:
                    // 計画一括作成画面
                    initMakePlan();
                    return ComConsts.RETURN_RESULT.OK;
                case ConductInfo.FormMakeMaintainance.FormNo:
                    // 保全活動作成画面
                    initMakeMaintainance();
                    return ComConsts.RETURN_RESULT.OK;
                case ConductInfo.FormSelectStandards.FormNo:
                    // 機器別管理基準選択画面
                    initSelectStandards();
                    return ComConsts.RETURN_RESULT.OK;
                case ConductInfo.FormBudgetOutput.FormNo:
                    // 予算出力画面
                    initBudgetOutput();
                    return ComConsts.RETURN_RESULT.OK;
                case ConductInfo.FormPostpone.FormNo:
                    // 予定作業一括延期画面
                    initPostpone();
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

            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo:
                    // 一覧画面
                    if (!searchList())
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
            if (compareId.IsRegist())
            {
                // 登録の場合
                // 登録処理実行
                return Regist();
            }
            else if (compareId.IsDelete() || compareId.IsStartId(ConductInfo.FormDetail.ButtonId.Delete))
            {
                // 削除の場合
                // 削除処理実行
                return Delete();
            }

            // 登録・削除と異なる、機能固有の処理の場合
            string metodName = string.Empty;
            string processName = string.Empty;
            if (this.FormNo == ConductInfo.FormDetail.FormNo && compareId.IsStartId(ConductInfo.FormDetail.ButtonId.Schedule))
            {
                // 参照画面・スケジュール確定ボタン
                metodName = "updateSchedule";
                processName = ComRes.ID.ID111130006; // スケジュール確定
            }
            else if (this.FormNo == ConductInfo.FormMakePlan.FormNo && compareId.IsStartId(ConductInfo.FormMakePlan.ButtonId.MakePlan))
            {
                // 計画一括作成画面・計画一括作成ボタン
                metodName = "registMakePlan";
                processName = ComRes.ID.ID111090001; // 計画一括作成
            }
            else if (this.FormNo == ConductInfo.FormMakeMaintainance.FormNo && compareId.IsStartId(ConductInfo.FormMakeMaintainance.ButtonId.Make))
            {
                // 保全活動作成画面・保全活動作成ボタン
                metodName = "registMakeMaintainance";
                processName = ComRes.ID.ID111300031; // 保全活動作成
            }
            else if (this.FormNo == ConductInfo.FormPostpone.FormNo && compareId.IsStartId(ConductInfo.FormPostpone.ButtonId.Postpone))
            {
                // 予定作業一括延期画面・一括延期ボタン
                metodName = "registPostpone";
                processName = ComRes.ID.ID111020032; // 一括延期
            }
            else
            {
                // 他の処理がある場合、else if 節に条件を追加する
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
                // 計画一括作成と保全活動作成は独自に完了メッセージを表示する
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
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse

            // 登録ボタンが複数の画面に無い場合、分岐は不要
            // 処理を実行する画面Noの値により処理を分岐する
            switch (this.FormNo)
            {
                case ConductInfo.FormEdit.FormNo:
                    // 編集画面の場合の登録処理
                    resultRegist = executeRegistEdit();
                    break;
                case ConductInfo.FormSelectStandards.FormNo:
                    // 機器別管理基準選択画面の場合の登録処理
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
            this.ResultList = new();
            // ボタンIDに応じて処理を分岐
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId);
            // 参照画面
            if (this.FormNo == ConductInfo.FormDetail.FormNo)
            {
                if (compareId.IsDelete())
                {
                    // 行削除
                    if (!deleteDetailRow())
                    {
                        setError();
                        return ComConsts.RETURN_RESULT.NG;
                    }
                }
                else if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.Delete))
                {
                    // 削除ボタン
                    if (!deleteDetailAll())
                    {
                        setError();
                        return ComConsts.RETURN_RESULT.NG;
                    }
                }
            }

            void setError()
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 未設定時にエラーメッセージを設定
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「削除処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911110001 });
                }
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「削除処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911110001 });

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            int reportFactoryId = 0;
            string reportId = string.Empty;

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

            string processName;
            if (this.FormNo == ConductInfo.FormDetail.FormNo && compareId.IsStartId(ConductInfo.FormDetail.ButtonId.OutputFiles))
            {
                // 参照画面・指示検収票ボタン
                if (!outputFiles())
                {
                    // エラーの場合
                    return ComConsts.RETURN_RESULT.NG;
                }
            }

            // 件名情報出力テスト
            if (this.CtrlId == "SubjectInformation")
            {
                reportId = "RP0070";
                // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                // ユーザの本務工場取得
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

                // ページ情報取得
                var pageInfo = GetPageInfo(
                    ConductInfo.FormList.ControlId.List,     // 一覧のコントールID
                    this.pageInfoList);          // ページ情報リスト

                // シートごとの帳票用選択キーデータ設定変数
                Dictionary<int, List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData>> dicSelectKeyDataList = new Dictionary<int, List<SelectKeyData>>();

                // 帳票定義取得
                // 出力帳票シート定義のリストを取得
                var sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                    TMQUtil.ComReport.GetReportSheetDefine,
                    TMQUtil.ExcelPath,
                    new { FactoryId = reportFactoryId, ReportId = reportId },
                    db);
                if (sheetDefineList == null)
                {
                    // 取得できない場合、処理を戻す
                    return ComConsts.RETURN_RESULT.NG;
                }
                // シート定義毎にループ
                foreach (var sheetDefine in sheetDefineList)
                {
                    // 検索条件フラグの場合はスキップする
                    if (sheetDefine.SearchConditionFlg == true)
                    {
                        continue;
                    }
                    Key keyInfo = getKeyInfoByTargetSqlParams(sheetDefine.TargetSqlParams);

                    // 帳票用選択キーデータ取得
                    // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                    List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport(
                        ConductInfo.FormList.ControlId.List,                // 一覧のコントールID
                        keyInfo,                                            // 設定したキー情報
                        this.resultInfoDictionary);                         // 画面データ

                    // シートNoをキーとして帳票用選択キーデータを保存する
                    dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                }

                //// 検索条件データ取得
                getSearchConditionForReport(pageInfo, out dynamic searchCondition);
                TMQUtil.CommonOutputExcel(
                    reportFactoryId,             // 工場ID
                    this.PgmId,                  // プログラムID
                    dicSelectKeyDataList,        // シートごとのパラメータでの選択キー情報リスト
                    searchCondition,             // 検索条件
                    reportId,                    // 帳票ID
                    1,                           // テンプレートID
                    1,                           // 出力パターンID
                    this.UserId,                 // ユーザID
                    this.LanguageId,             // 言語ID
                    this.conditionSheetLocationList,    // 場所階層構成IDリスト
                    this.conditionSheetJobList,         // 職種機種構成IDリスト
                    this.conditionSheetNameList,        // 検索条件項目名リスト
                    this.conditionSheetValueList,       // 検索条件設定値リスト
                    out string fileType,         // ファイルタイプ
                    out string fileName,         // ファイル名
                    out MemoryStream memStream,  // メモリストリーム
                    out string message,          // メッセージ
                    db);

                // OUTPUTパラメータに設定
                this.OutputFileType = fileType;
                this.OutputFileName = fileName;
                this.OutputStream = memStream;

            }

            // 長期スケジュール表出力
            if (this.CtrlId == "Report")
            {

                reportId = "RP0090";
                // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

                // ページ情報取得
                var pageInfo = GetPageInfo(
                    ConductInfo.FormList.ControlId.List,     // 一覧のコントールID
                    this.pageInfoList);          // ページ情報リスト

                // 検索条件データ取得
                getSearchConditionForReport(pageInfo, out dynamic searchCondition);

                // シートごとの帳票用選択キーデータ設定変数
                Dictionary<int, List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData>> dicSelectKeyDataList = new Dictionary<int, List<SelectKeyData>>();

                // 帳票定義取得
                // 出力帳票シート定義のリストを取得
                var sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                    TMQUtil.ComReport.GetReportSheetDefine,
                    TMQUtil.ExcelPath,
                    new { FactoryId = reportFactoryId, ReportId = reportId },
                    db);
                if (sheetDefineList == null)
                {
                    // 取得できない場合、処理を戻す
                    return ComConsts.RETURN_RESULT.NG;
                }
                // シート定義毎にループ
                foreach (var sheetDefine in sheetDefineList)
                {
                    // 検索条件フラグの場合はスキップする
                    if (sheetDefine.SearchConditionFlg == true)
                    {
                        continue;
                    }
                    Key keyInfo = getKeyInfoByTargetSqlParams(sheetDefine.TargetSqlParams);

                    // 帳票用選択キーデータ取得
                    // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                    List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport(
                        ConductInfo.FormList.ControlId.List,                // 一覧のコントールID
                        keyInfo,                                            // 設定したキー情報
                        this.resultInfoDictionary,                          // 画面データ
                        true);                                              // 選択フラグ

                    // シートNoをキーとして帳票用選択キーデータを保存する
                    dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                }

                // スケジュール関連
                // 年度開始月
                int monthStartNendo = getYearStartMonth();
                // 画面の条件を取得
                var scheduleCond = GetFormDataByCtrlId<TMQDao.ScheduleList.Condition>(ConductInfo.FormList.ControlId.ScheduleCondition, false);
                Dao.Schedule.SearchCondition cond = new(scheduleCond, monthStartNendo, this.LanguageId);
                cond.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);

                // 長期スケジュール用オプション
                TMQUtil.Option option = new TMQUtil.Option();
                // スケジュール表示単位 1:月度、2:年度
                option.DisplayUnit = (int)cond.DisplayUnit;
                // 開始年月日
                option.StartDate = cond.ScheduleStart;
                // 終了年月日
                option.EndDate = cond.ScheduleEnd;
                // 出力方式 1:件名別、2:機番別、3:予算別
                option.OutputMode = TMQUtil.ComReport.OutputMode3;
                // 年度開始月
                option.MonthStartNendo = monthStartNendo;
                // 検索条件クラス
                option.Condition = cond;

                // スケジュール表示単位が 1:月度の場合
                if (option.DisplayUnit == (int)TMQConst.MsStructure.StructureId.ScheduleDisplayUnit.Month)
                {
                    reportId = "RP0100";
                }
                TMQUtil.CommonOutputExcel(
                    reportFactoryId,             // 工場ID
                    this.PgmId,                  // プログラムID
                    dicSelectKeyDataList,        // シートごとのパラメータでの選択キー情報リスト
                    searchCondition,             // 検索条件
                    reportId,                    // 帳票ID
                    1,                           // テンプレートID
                    1,                           // 出力パターンID
                    this.UserId,                 // ユーザID
                    this.LanguageId,             // 言語ID
                    this.conditionSheetLocationList,    // 場所階層構成IDリスト
                    this.conditionSheetJobList,         // 職種機種構成IDリスト
                    this.conditionSheetNameList,        // 検索条件項目名リスト
                    this.conditionSheetValueList,       // 検索条件設定値リスト
                    out string fileType,         // ファイルタイプ
                    out string fileName,         // ファイル名
                    out MemoryStream memStream,  // メモリストリーム
                    out string message,          // メッセージ
                    db,
                    option);

                // OUTPUTパラメータに設定
                this.OutputFileType = fileType;
                this.OutputFileName = fileName;
                this.OutputStream = memStream;

            }

            // 予算出力画面から出力ボタン押下の場合
            if (this.FormNo == ConductInfo.FormBudgetOutput.FormNo && this.CtrlId == "btnOutPut")
            {
                // 出力期間入力チェック
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormBudgetOutput.ControlId.OutputCondition);
                string[] outputPeriod = targetDic["VAL1"].ToString().Split("|");
                if (string.IsNullOrEmpty(outputPeriod[0]) || string.IsNullOrEmpty(outputPeriod[1]))
                {
                    // 出力期間を入力してください。
                    this.MsgId = GetResMessage(new string[] { "941450003", "111120043" });
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                reportId = "RP0090";
                // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

                // ページ情報取得
                var pageInfo = GetPageInfo(
                    ConductInfo.FormList.ControlId.List,     // 一覧のコントールID
                    this.pageInfoList);          // ページ情報リスト
                // 検索条件データ取得
                getSearchConditionForReport(pageInfo, out dynamic searchCondition);

                // シートごとの帳票用選択キーデータ設定変数
                Dictionary<int, List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData>> dicSelectKeyDataList = new Dictionary<int, List<SelectKeyData>>();

                // 帳票定義取得
                // 出力帳票シート定義のリストを取得
                var sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                    TMQUtil.ComReport.GetReportSheetDefine,
                    TMQUtil.ExcelPath,
                    new { FactoryId = reportFactoryId, ReportId = reportId },
                    db);
                if (sheetDefineList == null)
                {
                    // 取得できない場合、処理を戻す
                    return ComConsts.RETURN_RESULT.NG;
                }
                // シート定義毎にループ
                foreach (var sheetDefine in sheetDefineList)
                {
                    // 検索条件フラグの場合はスキップする
                    if (sheetDefine.SearchConditionFlg == true)
                    {
                        continue;
                    }
                    Key keyInfo = getKeyInfoByTargetSqlParams(sheetDefine.TargetSqlParams);

                    // 帳票用選択キーデータ取得
                    // 一覧のコントールIDに持つキー項目の値を画面データと紐づけを行い取得する
                    List<SelectKeyData> selectKeyDataList = getSelectKeyDataForReport(
                        ConductInfo.FormBudgetOutput.ControlId.LongPlanId,  // 一覧のコントールID
                        keyInfo,                                            // 設定したキー情報
                        this.resultInfoDictionary,                          // 画面データ
                        false);                                             // 選択フラグ

                    // シートNoをキーとして帳票用選択キーデータを保存する
                    dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                }

                // スケジュール関連
                // 年度開始月
                int monthStartNendo = getYearStartMonth();

                // スケジュール表示条件を取得
                var scheduleCond = GetFormDataByCtrlId<TMQDao.ScheduleList.Condition>(ConductInfo.FormBudgetOutput.ControlId.ScheduleCondition, true);
                Dao.Schedule.SearchCondition cond = new(scheduleCond, monthStartNendo, this.LanguageId);
                cond.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);

                // 長期スケジュール用オプション
                TMQUtil.Option option = new TMQUtil.Option();
                // スケジュール表示単位 1:月度、2:年度
                option.DisplayUnit = (int)cond.DisplayUnit;
                // 開始年月日
                //option.StartDate = cond.ScheduleStart;
                // 終了年月日
                //option.EndDate = cond.ScheduleEnd;
                // 出力方式 1:件名別、2:機番別、3:予算別
                // 3:予算別
                option.OutputMode = TMQUtil.ComReport.OutputMode3;
                // 年度開始月
                option.MonthStartNendo = monthStartNendo;
                // 検索条件クラス
                option.Condition = cond;

                // 予算出力画面の出力条件を取得
                var outputCond = GetFormDataByCtrlId<Dao.BudgetOutputOutputCondition>(ConductInfo.FormBudgetOutput.ControlId.OutputCondition, true);
                // 開始年月日と終了年月日は予算出力画面からに置き換え
                // 年度の開始日に変換
                DateTime startDate = ComUtil.GetNendoStartDay(outputCond.OutputPeriodFrom.GetValueOrDefault(), monthStartNendo);
                // 年度の終了日に変換
                DateTime endDate = ComUtil.GetNendoLastDay(outputCond.OutputPeriodTo.GetValueOrDefault(), monthStartNendo);
                // スケジュール表示単位 1:月度の場合
                if (option.DisplayUnit == (int)TMQConst.MsStructure.StructureId.ScheduleDisplayUnit.Month)
                {
                    // 予算出力画面の開始を年度の終了日に変換
                    endDate = ComUtil.GetNendoLastDay(outputCond.OutputPeriodFrom.GetValueOrDefault(), monthStartNendo);
                }

                // 予算開始年月日
                option.BudgetStartDate = startDate;
                // 予算終了年月日
                option.BudgetEndDate = endDate;
                // 開始年月日
                option.StartDate = startDate;
                // 終了年月日
                option.EndDate = endDate;
                cond.ScheduleStart = startDate;
                cond.ScheduleEnd = endDate;

                // スケジュール表示単位が 1:月度の場合
                if (option.DisplayUnit == (int)TMQConst.MsStructure.StructureId.ScheduleDisplayUnit.Month)
                {
                    reportId = "RP0100";
                }
                TMQUtil.CommonOutputExcel(
                    reportFactoryId,             // 工場ID
                    this.PgmId,                  // プログラムID
                    dicSelectKeyDataList,        // シートごとのパラメータでの選択キー情報リスト
                    searchCondition,             // 検索条件
                    reportId,                    // 帳票ID
                    1,                           // テンプレートID
                    1,                           // 出力パターンID
                    this.UserId,                 // ユーザID
                    this.LanguageId,             // 言語ID
                    this.conditionSheetLocationList,    // 場所階層構成IDリスト
                    this.conditionSheetJobList,         // 職種機種構成IDリスト
                    this.conditionSheetNameList,        // 検索条件項目名リスト
                    this.conditionSheetValueList,       // 検索条件設定値リスト
                    out string fileType,         // ファイルタイプ
                    out string fileName,         // ファイル名
                    out MemoryStream memStream,  // メモリストリーム
                    out string message,          // メッセージ
                    db,
                    option);

                // OUTPUTパラメータに設定
                this.OutputFileType = fileType;
                this.OutputFileName = fileName;
                this.OutputStream = memStream;
            }
            // 出力処理が完了しました。
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, ComRes.ID.ID911120006 });
            this.Status = CommonProcReturn.ProcStatus.Valid;

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// ExcelPortダウンロード処理
        /// </summary>
        /// <param name="fileType">ファイル種類</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="ms">メモリストリーム</param>
        /// <param name="resultMsg">結果メッセージ</param>
        /// <param name="detailMsg">詳細メッセージ</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ExcelPortDownloadImpl(ref string fileType, ref string fileName, ref MemoryStream ms, ref string resultMsg, ref string detailMsg)
        {
            // ExcelPortクラスの生成
            var excelPort = new TMQUtil.ComExcelPort(
                this.db, this.UserId, this.BelongingInfo, this.LanguageId, this.FormNo, this.searchConditionDictionary, this.messageResources);

            // ExcelPortテンプレートファイル情報初期化
            this.Status = CommonProcReturn.ProcStatus.Valid;
            if (!excelPort.InitializeExcelPortTemplateFile(out resultMsg, out detailMsg))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }
            else if (!string.IsNullOrEmpty(resultMsg))
            {
                // 正常終了時、詳細メッセージがセットされている場合、警告メッセージ
                this.Status = CommonProcReturn.ProcStatus.Warning;
            }

            // 個別データ検索処理
            IList<Dictionary<string, object>> dataList = null;

            // SQLパラメータに言語ID設定
            dynamic whereParam = new ExpandoObject();
            whereParam.LanguageId = this.LanguageId;

            // 現在日付を取得
            DateTime now = DateTime.Now;

            if (excelPort.DownloadCondition.SheetNo == TMQExcelPort.SheetNo.LongPlan)
            {
                // 長期計画

                // 一時テーブル設定
                setTempTableForGetList(null, true);

                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.List.SubDir, SqlName.List.GetList, out string baseSql);
                // WITH句は別に取得
                TMQUtil.GetFixedSqlStatementWith(SqlName.List.SubDir, SqlName.List.GetList, out string withSql);

                // WHERE句に場所階層と職種機種を設定
                var whereSql = new StringBuilder();
                whereSql.AppendLine("WHERE");
                whereSql.Append("EXISTS(SELECT * FROM #temp_structure_selected temp WHERE ");
                whereSql.Append(CommonColumnName.LocationId).AppendLine("= temp.structure_id)");
                whereSql.AppendLine("AND");
                whereSql.Append("EXISTS(SELECT * FROM #temp_structure_selected temp WHERE ");
                whereSql.Append(CommonColumnName.JobId).AppendLine("= temp.structure_id)");

                // 一覧検索SQL文の取得
                string executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql.ToString(), withSql);
                var selectSql = new StringBuilder(executeSql);
                selectSql.AppendLine("ORDER BY");
                selectSql.AppendLine("subject");

                // 一覧検索実行
                IList<Dao.ExcelPort.List> results = db.GetListByDataClass<Dao.ExcelPort.List>(selectSql.ToString(), whereParam);
                if (results == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // Dicitionalyに変換
                dataList = ComUtil.ConvertClassToDictionary<Dao.ExcelPort.List>(results);

            }
            else if (excelPort.DownloadCondition.SheetNo == TMQExcelPort.SheetNo.ManagementStandards)
            {
                // 機器別管理基準

                // 一時テーブル設定
                setTempTableForGetList(-1, true);

                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.ExcelPort.SubDir, SqlName.ExcelPort.GetDetail, out string selectSql);

                // 一覧検索実行
                IList<Dao.ExcelPort.DeitalList> results = db.GetListByDataClass<Dao.ExcelPort.DeitalList>(selectSql, whereParam);
                if (results == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 機器別管理基準内容IDを抽出してパラメータへ設定
                if (results.Count > 0)
                {
                    whereParam.ManagementStandardsContentIdList = string.Join(',', results.Select(x => x.ManagementStandardsContentId));
                }

                // Dicitionalyに変換
                dataList = ComUtil.ConvertClassToDictionary<Dao.ExcelPort.DeitalList>(results);

            }
            //if (dataList == null || dataList.Count == 0)
            //{
            //    this.Status = CommonProcReturn.ProcStatus.Warning;
            //    // 「該当データがありません。」
            //    resultMsg = GetResMessage(ComRes.ID.ID941060001);
            //    return ComConsts.RETURN_RESULT.NG;
            //}

            // 出力最大データ数チェック
            if (!excelPort.CheckDownloadMaxCnt(dataList.Count))
            {
                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「出力可能上限データ数を超えているため、ダウンロードできません。」
                resultMsg = GetResMessage(ComRes.ID.ID141120013);
                return ComConsts.RETURN_RESULT.NG;
            }

            if (excelPort.DownloadCondition.SheetNo == TMQExcelPort.SheetNo.ManagementStandards && dataList.Count > 0)
            {
                // 機器別管理基準の場合、データ件数チェック後にスケジュールデータを取得する
                // スケジュール一覧取得
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.ExcelPort.SubDir, SqlName.ExcelPort.GetScheduleForDownload, out string selectSql);
                // スケジュール一覧検索実行
                IList<Dao.Detail.ScheduleList> schedules = db.GetListByDataClass<Dao.Detail.ScheduleList>(selectSql, whereParam);
                if (schedules == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                var conIdName = nameof(Dao.Detail.ScheduleList.ManagementStandardsContentId);
                var scIdName = nameof(Dao.Detail.ScheduleList.ScheduleId);
                var scTextName = nameof(Dao.Detail.ScheduleList.Schedule);
                var scDateName = nameof(Dao.Detail.ScheduleList.ScheduleDate);
                for (int i = 0; i < TMQUtil.ComExcelPort.ScheduleMonths; i++)
                {
                    DateTime month = now.AddMonths(i);
                    var tmpSchedules = schedules.Where(x => (x.ScheduleDate.Year == month.Year && x.ScheduleDate.Month == month.Month)).ToList();
                    if (tmpSchedules != null && tmpSchedules.Count > 0)
                    {
                        // スケジュールのセット
                        foreach (var schedule in tmpSchedules)
                        {
                            var data = dataList.Where(x => schedule.ManagementStandardsContentId.Equals(x[conIdName])).FirstOrDefault();
                            if (data != null)
                            {
                                var keyId = scIdName + (i + 1);
                                var keyText = scTextName + (i + 1);
                                var keyDate = scDateName + (i + 1);
                                if (data.ContainsKey(keyId))
                                {
                                    if ((DateTime)data[keyDate] <= schedule.ScheduleDate)
                                    {
                                        data[keyId] = schedule.ScheduleId;
                                        data[keyText] = schedule.Schedule;
                                        data[keyDate] = schedule.ScheduleDate;
                                    }
                                }
                                else
                                {
                                    data.Add(keyId, schedule.ScheduleId);
                                    data.Add(keyText, schedule.Schedule);
                                    data.Add(keyDate, schedule.ScheduleDate);
                                }
                            }
                        }
                    }
                }
            }

            // 個別シート出力処理
            if (!excelPort.OutputExcelPortTemplateFile(dataList, out fileType, out fileName, out ms, out detailMsg, ref resultMsg))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// ExcelPortアップロード個別処理
        /// </summary>
        /// <param name="file">アップロード対象ファイル</param>
        /// <param name="fileType">ファイル種類</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="ms">メモリストリーム</param>
        /// <param name="resultMsg">結果メッセージ</param>
        /// <param name="detailMsg">詳細メッセージ</param>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected override int ExcelPortUploadImpl(IFormFile file, ref string fileType, ref string fileName, ref MemoryStream ms, ref string resultMsg, ref string detailMsg)
        {
            // ExcelPortクラスの生成
            var excelPort = new TMQUtil.ComExcelPort(
                this.db, this.UserId, this.BelongingInfo, this.LanguageId, this.FormNo, this.searchConditionDictionary, this.messageResources);

            // ExcelPortテンプレートファイル情報初期化
            this.Status = CommonProcReturn.ProcStatus.Valid;
            if (!excelPort.InitializeExcelPortTemplateFile(out resultMsg, out detailMsg, true, this.IndividualDictionary))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // ExcelPortアップロードデータの取得
            if (excelPort.UploadCondition.SheetNo == TMQExcelPort.SheetNo.LongPlan)
            {
                // 長期計画
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, ConductInfo.FormList.ControlId.ExcelPortUpload,
                    out List<Dao.ExcelPort.List> resultList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                //IListに変換する
                IList<Dao.ExcelPort.List> list = resultList as IList<Dao.ExcelPort.List>;
                //最下層の構成IDを取得して機能場所階層ID、職種機種階層IDにセットする
                TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.ExcelPort.List>(ref list, new List<StructureType> { StructureType.Location, StructureType.Job }, true);
                resultList = list.ToList();

                // エラー情報リスト
                List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();

                // 長期計画登録チェック処理
                if (isErrorListForExcelPort(excelPort.UploadCondition, ref resultList, ref errorInfoList))
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 長期計画件名登録処理
                if (!executeRegistListForExcelPort(resultList))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

            }
            else if (excelPort.UploadCondition.SheetNo == TMQExcelPort.SheetNo.ManagementStandards)
            {
                // 機器別管理基準
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, ConductInfo.FormDetail.ControlId.ExcelPortUpload,
                    out List<Dao.ExcelPort.DeitalList> resultList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // エラー情報リスト
                List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();

                // 機器別管理基準登録チェック処理
                if(isErrorDetailListForExcelPort(excelPort.UploadCondition, ref resultList, ref errorInfoList))
                {
                    if (errorInfoList.Count > 0)
                    {
                        // エラー情報シートへ設定
                        excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    }
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 機器別管理基準登録処理
                if (!executeRegistDetailListForExcelPort(excelPort.UploadCondition, resultList))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド

        #region 機能内汎用の処理
        /// <summary>
        /// 遷移元の画面の一覧の内容よりキー項目の内容を取得する
        /// </summary>
        /// <param name="fromCtrlId">遷移元の一覧のコントロールID</param>
        /// <param name="isReSearch">再表示の場合True</param>
        /// <returns>取得したキー項目の内容、取得できない場合(新規)はNull</returns>
        private ComDao.LnLongPlanEntity getParam(string fromCtrlId, bool isReSearch)
        {
            // 遷移元の一覧と検索条件の内容より、キー項目の値を取得する
            ComDao.LnLongPlanEntity param = new();
            // 画面情報取得元、再表示の場合はSearchConditionDictionaryでなくResultInfoDictionary
            var targetDicList = !isReSearch ? this.searchConditionDictionary : this.resultInfoDictionary;
            var targetDic = ComUtil.GetDictionaryByCtrlId(targetDicList, fromCtrlId);
            SetDataClassFromDictionary(targetDic, fromCtrlId, param, new List<string> { "LongPlanId" });
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
        #endregion

        #region 他機能に転用できそうな汎用の処理
        // 使ってブラッシュアップして良さそうなら共通に持っていく

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

        #endregion

        #region 一覧・参照画面関連
        /// <summary>
        /// 一覧取得SQLに必要な、一時テーブル登録処理
        /// </summary>
        private void setTempTableForGetList(long? longPlanId = null, bool isExcelPort = false)
        {
            // 一覧の場合、長計件名IDがNull。詳細の場合、Nullでない。
            bool isList = longPlanId == null;

            // 一時テーブル関連処理
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
            listPf.GetAttachmentSql(new List<FunctionTypeId> { FunctionTypeId.Machine, FunctionTypeId.Equipment, FunctionTypeId.LongPlan });
            // 使用する構成グループ(点検種別はスケジュールで使用)
            var structuregroupList = new List<GroupId>
                    {
                        GroupId.Location, GroupId.Job, GroupId.Season, GroupId.BudgetPersonality, GroupId.Purpose,
                        GroupId.WorkClass, GroupId.Treatment, GroupId.Facility, GroupId.MaintainanceKind,
                        GroupId.WorkItem, GroupId.BudgetManagement
                    };
            if (!isList)
            {
                // 詳細の場合、翻訳を追加
                structuregroupList.AddRange(new List<GroupId> { GroupId.Importance, GroupId.SiteMaster, GroupId.InspectionDetails, GroupId.ScheduleType });
            }
            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ

            // 機能個別
            if (!isExcelPort)
            {
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
            }

            listPf.RegistTempTable(); // 登録
        }

        /// <summary>
        /// 指定された長期計画の一覧の情報を取得する
        /// </summary>
        /// <param name="param">取得する長期計画の条件</param>
        /// <param name="isTree">省略可能 ツリービュー表示用の場合、True</param>
        /// <returns>取得した長期計画の情報(一覧画面の1レコード)</returns>
        private Dao.ListSearchResult getLongPlanInfo(ComDao.LnLongPlanEntity param, bool isTree = false)
        {
            // 一時テーブル関連処理
            setTempTableForGetList(param.LongPlanId);

            // 一覧検索のSQLをキー指定で実行し、画面に表示する内容を取得する
            TMQUtil.GetFixedSqlStatementWith(SqlName.List.SubDir, SqlName.List.GetList, out string withSql, new List<string> { UnCommentWordOfGetList });
            TMQUtil.GetFixedSqlStatement(SqlName.List.SubDir, SqlName.List.GetList, out string outSql, new List<string> { UnCommentWordOfGetList });
            StringBuilder execSql = new(withSql);
            execSql.AppendLine(addSqlWhereLongPlanId(outSql));

            // キー指定なので結果は必ず1件
            IList<Dao.ListSearchResult> results = new List<Dao.ListSearchResult> { db.GetEntityByDataClass<Dao.ListSearchResult>(execSql.ToString(), param) };

            // 取得した結果に対して、地区と職種の情報を設定する
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.ListSearchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, isTree);

            return results[0];
        }

        /// <summary>
        /// 変更管理ボタンの表示制御用のフラグを設定
        /// </summary>
        /// <param name="listCtrlId">設定する非表示項目のID</param>
        /// <param name="factoryId">省略可能　変更管理を行うか判定する工場ID</param>
        private void setHistoryManagementFlg(string listCtrlId, int factoryId = -1)
        {
            // 変更管理フラグを取得
            TMQUtil.HistoryManagement history = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);
            // 変更管理対象外の工場の権限有無(工場ID未指定の、ユーザに対する権限を取得する場合に使用)
            bool isExistsNoHistory = false;
            // 工場ID省略時は引数に含めずに、変更管理フラグを取得
            bool isHitoryManagementFlg = factoryId == -1 ? history.IsHistoryManagementFactoryUserBelong(out isExistsNoHistory) : history.IsHistoryManagementFactory(factoryId);
            // 画面に設定する内容
            Dao.HiddenInfo hideInfo = new();
            // 変更管理ボタンの表示フラグ
            // 0:変更管理対象外、表示しない
            // 1:変更管理対象、表示する
            // 2:混在する(一覧画面のみ)、変更管理する場合としない場合両方を表示
            hideInfo.IsHistoryManagementFlg = isHitoryManagementFlg ? (isExistsNoHistory ? 2 : 1) : 0;
            // 画面に設定
            var pageInfo = GetPageInfo(listCtrlId, this.pageInfoList);
            SetSearchResultsByDataClass<Dao.HiddenInfo>(pageInfo, new List<Dao.HiddenInfo> { hideInfo }, 1);
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
        private bool initFormByLongPlanId(ComDao.LnLongPlanEntity param, List<string> toCtrlIds, out bool isMaintainanceKindFactory, out int factoryId, bool isTree = false, bool isInitDetail = false)
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
        /// <returns>SQLの内容</returns>
        private string getSqlGetDetailWith(bool isUnComp)
        {
            List<string> listUnComment = new();
            if (isUnComp)
            {
                listUnComment.Add(UnCommentWordOfGetDetailWith);
            }
            TMQUtil.GetFixedSqlStatement(SqlName.Detail.SubDir, SqlName.Detail.GetDetailWith, out string withSql, listUnComment);
            return withSql;
        }

        /// <summary>
        /// 保全情報一覧取得
        /// </summary>
        /// <param name="longPlanId">長計件名ID</param>
        /// <param name="factoryId">この長計件名IDの工場ID</param>
        /// <param name="isDisplayMaintainanceKind">保全情報一覧(点検種別毎)を表示する場合True</param>
        /// <param name="schedulePlanContent">取得する一覧の種類</param>
        /// <param name="isUnComp">未完了を含むもののみ表示する場合はTrue、省略時はFalse</param>
        /// <returns>取得した一覧のデータクラスのリスト</returns>
        private IList<Dao.Detail.List> getMaintainanceList(long longPlanId, int factoryId, bool isDisplayMaintainanceKind, SchedulePlanContent schedulePlanContent, bool isUnComp = false)
        {
            // 値に応じてSQLを取得
            string fileNameSelect = string.Empty;
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
                    break;
                default:
                    // 到達不能
                    return null;
            }
            // SQL取得
            string withSql = getSqlGetDetailWith(isUnComp); // WITH句
            TMQUtil.GetFixedSqlStatement(SqlName.Detail.SubDir, fileNameSelect, out string selectSql); // SELECT句
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
            var param = getScheduleCondition(longPlanId, factoryId, getYearStartMonth(factoryId));
            // 検索
            var list = db.GetListByDataClass<Dao.Detail.List>(execSql.ToString(), param);
            return list;
        }

        #endregion

        #region 保全活動作成関連
        /// <summary>
        /// 保全活動を一括で作成する機能の完了メッセージ表示
        /// </summary>
        /// <param name="summaryCounts">保全活動件名の作成件数</param>
        private void setCompleteMessageForMakeSummary(int summaryCounts)
        {
            // N件の保全活動件名を作成しました。
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141090003, summaryCounts.ToString(), ComRes.ID.ID111300033 });
        }
        #endregion

        #region 入力チェック関連
        /// <summary>
        /// 入力チェックに使用する値を取得
        /// </summary>
        /// <param name="condition">画面の条件(長期計画ID、スケジュール日の開始と終了)</param>
        /// <returns>入力チェック値リスト</returns>
        private List<Dao.InputCheck.Result> getListForInputCheck(Dao.InputCheck.Condition condition)
        {
            List<string> listUnComment = new();
            // 機器別管理基準内容IDが検索条件に含まれている場合、SQLの検索条件を有効化
            if (condition.ManagementStandardsContentIdList != null && condition.ManagementStandardsContentIdList.Count > 0)
            {
                listUnComment.Add("Content");
            }
            var list = TMQUtil.SqlExecuteClass.SelectList<Dao.InputCheck.Result>(SqlName.InputCheck.GetLongPlanInfo, SqlName.InputCheck.SubDir, condition, this.db,
                listUnComment: listUnComment);
            return list;
        }

        /// <summary>
        /// スケジュール日存在チェック(入力チェック用SQL結果を使用)
        /// </summary>
        /// <param name="longPlanList">入力チェック用SQL結果</param>
        /// <returns>エラーの場合TRUE</returns>
        private bool isErrorScheduleDateExists(List<Dao.InputCheck.Result> longPlanList)
        {
            // スケジュール日存在チェック
            if (longPlanList == null || longPlanList.Count == 0)
            {
                // 指定された条件期間に機器別管理基準データのスケジュール日が存在しません。
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141120001 });
                return true;
            }
            return false;
        }

        /// <summary>
        /// 保全活動作成済みチェック(入力チェック用SQL結果を使用)
        /// </summary>
        /// <param name="longPlanList">入力チェック用SQL結果</param>
        /// <returns>エラーの場合TRUE</returns>
        private bool isErrorMaintainanceSummaryCreated(List<Dao.InputCheck.Result> longPlanList)
        {
            // 作成済みチェック
            if (longPlanList != null && longPlanList.Count > 0 && longPlanList.Count(x => x.SummaryId != null) > 0)
            {
                // 指定された条件は既に保全活動が作成されています。
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141120002 });
                return true;
            }
            return false;
        }
        #endregion

        #region スケジュール関連

        /// <summary>
        /// 画面のスケジュールよりスケジュールのSQL検索条件を取得する処理
        /// </summary>
        /// <param name="longPlanId">長期計画ID</param>
        /// <param name="factoryId">長期計画の工場ID</param>
        /// <param name="monthStartNendo">年度開始月</param>
        /// <returns>スケジュールのSQL検索条件</returns>
        private Dao.Schedule.SearchConditionLongPlanId getScheduleCondition(long longPlanId, int factoryId, int monthStartNendo)
        {
            // 画面の条件を取得
            var scheduleCond = GetFormDataByCtrlId<TMQDao.ScheduleList.Condition>(ConductInfo.FormDetail.ControlId.ScheduleCondition, false);
            Dao.Schedule.SearchConditionLongPlanId cond = new(scheduleCond, longPlanId, monthStartNendo, this.LanguageId);
            cond.FactoryIdList = TMQUtil.GetFactoryIdList();
            cond.FactoryIdList.Add(factoryId);

            return cond;
        }

        /// <summary>
        /// 保全情報一覧に紐づけるスケジュールリストを取得する処理
        /// </summary>
        /// <param name="listCtrlId">一覧のコントロールID</param>
        /// <param name="isDetail">参照画面の場合True(仕様が異なる、移動可、リンク有)</param>
        /// <param name="longPlanId">長期計画ID</param>
        /// <param name="factoryId">長期計画IDの工場ID</param>
        /// <param name="schedulePlanContent">スケジュールの表示単位</param>
        /// <param name="isUnComp">未完了のデータを表示する場合True</param>
        private void setSchedule(string listCtrlId, bool isDetail, long longPlanId, int factoryId, SchedulePlanContent schedulePlanContent, bool isUnComp = false)
        {
            // 実行するSQL文を取得
            string fileNameSelect = string.Empty;
            switch (schedulePlanContent)
            {
                case SchedulePlanContent.Equipment:
                    fileNameSelect = SqlName.Detail.GetScheduleEquipment;
                    break;
                case SchedulePlanContent.InspectionSite:
                    fileNameSelect = SqlName.Detail.GetScheduleInspectionSite;
                    break;
                case SchedulePlanContent.Maintainance:
                    fileNameSelect = SqlName.Detail.GetScheduleMaintainance;
                    break;
                default:
                    // 到達不能
                    return;
            }
            string withSql = getSqlGetDetailWith(isUnComp); // WITH句
            TMQUtil.GetFixedSqlStatement(SqlName.Detail.SubDir, fileNameSelect, out string selectSql); // SELECT句
            // 結合
            StringBuilder execSql = new(withSql);
            execSql.AppendLine(selectSql);

            // 年度開始月の取得
            int monthStartNendo = getYearStartMonth(factoryId);

            // 画面の条件を取得
            Dao.Schedule.SearchConditionLongPlanId cond = getScheduleCondition(longPlanId, factoryId, monthStartNendo);

            // 個別実装用データへスケジュールのレイアウトデータ(scheduleLayout)をセット
            bool isMovable = false;
            if (isDetail)
            {
                // 参照画面の場合、計画内容が保全項目なら移動可能
                isMovable = schedulePlanContent == SchedulePlanContent.Maintainance;
            }
            TMQUtil.ScheduleListUtil.SetLayout(ref this.IndividualDictionary, cond, isMovable, getNendoText(), monthStartNendo);

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
            // TODO:この行をコメントアウトすればリンクしなくなる
            // ○：保全活動新規登録画面
            result.Add(ScheduleStatus.NoCreate, new(MA0001.ConductId, MA0001.FormNo.New, MA0001.TabNoDetail.New));
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
        #endregion

        #region 帳票出力関連
        /// <summary>
        /// 出力帳票シート定義の対象sqlパラメータよりキー情報を取得する
        /// </summary>
        /// <param name="targetSqlParams">対象sqlパラメータ</param>
        /// <returns>キー情報</returns>
        private Key getKeyInfoByTargetSqlParams(string targetSqlParams)
        {
            string keyParam1 = string.Empty;
            string keyParam2 = string.Empty;
            string keyParam3 = string.Empty;
            if (targetSqlParams != null && string.IsNullOrEmpty(targetSqlParams) == false)
            {
                string[] sqlParams = targetSqlParams.Split("|");
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    if (i == 0)
                    {
                        keyParam1 = sqlParams[i];
                    }
                    else if (i == 1)
                    {
                        keyParam2 = sqlParams[i];
                    }
                    if (i == 2)
                    {
                        keyParam3 = sqlParams[i];
                    }
                }
            }
            return new Key(keyParam1, keyParam2, keyParam3);
        }
        #endregion

        #region ExcelPort関連
        /// <summary>
        /// ExcelPort入力チェック：長期計画
        /// </summary>
        /// <param name="resultList">対象データリスト</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>チェック結果</returns>
        private bool isErrorListForExcelPort(TMQUtil.ExcelPortUploadCondition uploadCondition, ref List<Dao.ExcelPort.List> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            bool isError = false;

            // 送信時処理が削除のデータを抽出
            var deleteList = resultList.Where(x => x.ProcessId == TMQConst.SendProcessId.Delete);
            if(deleteList == null || deleteList.Count() == 0)
            {
                return isError;
            }

            // 長計件名IDを抽出
            var longPlanIdList = deleteList.Select(x => x.LongPlanId.ToString()).ToList();

            // 長計件名IDに紐づく保全スケジュール詳細情報を取得
            var scheduleList = getScheduleDetailListForExcelPort(
                new Dao.ExcelPort.Condition()
                {
                    LongPlanIdList = string.Join(',', longPlanIdList),
                    ScheduleDateFrom = uploadCondition.ScheduleDateFrom.ToString(ScheduleDateConditionFormat),
                    ScheduleDateTo = uploadCondition.ScheduleDateTo.ToString(ScheduleDateConditionFormat)
                });

            foreach (var result in deleteList)
            {
                if (scheduleList != null && scheduleList.Count(x => x.LongPlanId == result.LongPlanId && x.SummaryId != null) > 0)
                {
                    // 保全活動が紐づけられているスケジュールが存在する場合
                    // 「保全活動が作成されている為、削除できません。」
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo(result.RowNo.Value, ExcelPort.LongPlan.ColumnNo.SendProcess, null, GetResMessage(ComRes.ID.ID141300004), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                    isError = true;
                }
            }
            return isError;
        }

        /// <summary>
        /// 保全スケジュール詳細情報を取得
        /// </summary>
        /// <param name="condition">条件(機器別管理基準内容ID、対象期間From/To)</param>
        /// <returns>入力チェック値リスト</returns>
        private List<Dao.ExcelPort.Result> getScheduleDetailListForExcelPort(Dao.ExcelPort.Condition condition, bool isComplete = false)
        {
            List<Dao.ExcelPort.Result> list;
            if (!isComplete)
            {
                var unComment = !string.IsNullOrEmpty(condition.LongPlanIdList) ? "LongPlan" : "Content";
                list = TMQUtil.SqlExecuteClass.SelectList<Dao.ExcelPort.Result>(
                    SqlName.ExcelPort.GetScheduleForUpload, SqlName.ExcelPort.SubDir, condition, this.db, listUnComment:new List<string> { unComment });
            }
            else
            {
                list = TMQUtil.SqlExecuteClass.SelectList<Dao.ExcelPort.Result>(
                    SqlName.ExcelPort.GetScheduleComplete, SqlName.ExcelPort.SubDir, condition, this.db);
            }
            return list;
        }

        /// <summary>
        /// ExcelPort入力チェック：長期計画
        /// </summary>
        /// <param name="resultList">対象データリスト</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>チェック結果</returns>
        private bool isErrorDetailListForExcelPort(TMQUtil.ExcelPortUploadCondition uploadCondition, ref List<Dao.ExcelPort.DeitalList> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            bool isError = false;

            // 機器別管理基準内容IDを抽出
            var managementStandardsContentIdList = resultList.Select(x => x.ManagementStandardsContentId.ToString());

            // 機器別管理基準内容IDに紐づく保全スケジュール詳細データを取得
            var scheduleListAll = getScheduleDetailListForExcelPort(
                new Dao.ExcelPort.Condition()
                {
                    ManagementStandardsContentIdList = string.Join(',', managementStandardsContentIdList),
                    ScheduleDateFrom = uploadCondition.ScheduleDateFrom.ToString(ScheduleDateConditionFormat),
                    ScheduleDateTo = uploadCondition.ScheduleDateTo.ToString(ScheduleDateConditionFormat),
                    LanguageId = this.LanguageId
                });
            if(scheduleListAll == null || scheduleListAll.Count == 0)
            {
                return isError;
            }

            foreach (var result in resultList)
            {
                var scheduleList = scheduleListAll.Where(x => x.ManagementStandardsContentId == result.ManagementStandardsContentId).ToList();
                if (result.LongPlanId != null)
                {
                    // 長期計画件名が設定されている場合、機器別管理基準に紐づけ
                    if (scheduleList != null && scheduleList.Count > 0)
                    {
                        foreach (var schedule in result.ScheduleList)
                        {
                            // スケジュール済(○)の場合
                            if (schedule.ScheduleId == (int)TMQConst.MsStructure.StructureId.ScheduleStatus.NoCreate)
                            {
                                if (scheduleList.Count(x => schedule.ScheduleDate.ToString(ScheduleDateConditionFormat).Equals(x.ScheduleDate.ToString(ScheduleDateConditionFormat)) &&
                                   (x.Complition.HasValue && x.Complition.Value)) > 0)
                                {
                                    // 該当年月に履歴(●)が存在する場合
                                    // 「履歴が存在する年月には新規にスケジュールを追加できません。」
                                    errorInfoList.Add(TMQUtil.setTmpErrorInfo(result.RowNo.Value, schedule.ColumnNo, null, GetResMessage(ComRes.ID.ID141400001), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                                    isError = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // 長期計画件名が未設定の場合、長期計画件名の紐づけを解除
                    if (scheduleList.Count(x => x.SummaryId != null) > 0)
                    {
                        // 保全活動が紐づけられているスケジュールが存在する場合
                        // 「保全活動が作成されている為、削除できません。」
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo(result.RowNo.Value, ExcelPort.LongPlan.ColumnNo.SendProcess, null, GetResMessage(ComRes.ID.ID141300004), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                        isError = true;
                    }
                }
            }
            return isError;
        }

        /// <summary>
        /// ExcelPort長期計画件名情報登録処理実行
        /// </summary>
        /// <param name="resultList">対象データリスト</param>
        private bool executeRegistListForExcelPort(List<Dao.ExcelPort.List> resultList)
        {
            var now = DateTime.Now;
            bool chkUpd = int.TryParse(this.UserId, out int updUserId);
            for (int i = 0; i < resultList.Count; i++)
            {
                var result = resultList[i];
                if (result.ProcessId == TMQConst.SendProcessId.Regist)
                {
                    // 登録の場合

                    // ツリーの階層情報を登録用プロパティに設定
                    setLayerInfo(result);
                    // 共通の更新日時などを設定
                    setExecuteConditionByDataClassCommon<Dao.ExcelPort.List>(ref result, now, updUserId, updUserId);
                    // 長期計画件名情報を登録
                    if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Common.InsertLongPlan, SqlName.SubDir, result, this.db))
                    {
                        return false;
                    }
                }
                else if (result.ProcessId == TMQConst.SendProcessId.Update)
                {
                    // 内容更新の場合

                    // ツリーの階層情報を登録用プロパティに設定
                    setLayerInfo(result);
                    // 共通の更新日時などを設定
                    setExecuteConditionByDataClassCommon<Dao.ExcelPort.List>(ref result, now, updUserId, -1);
                    // 長期計画件名情報を更新
                    if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Common.UpdateLongPlan, SqlName.SubDir, result, this.db))
                    {
                        return false;
                    }
                }
                else if (result.ProcessId == TMQConst.SendProcessId.Delete)
                {
                    // 削除の場合

                    // 長期計画件名を削除
                    if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Common.DeleteLongPlan, SqlName.SubDir, result, this.db))
                    {
                        return false;
                    }

                    // 機器別管理基準の長計件名IDをNullに更新
                    // 共通の更新日時などを設定
                    setExecuteConditionByDataClassCommon<Dao.ExcelPort.List>(ref result, now, updUserId, -1);
                    // ※機器別管理基準が紐づけられていない場合もあるため、戻り値は見ない
                    TMQUtil.SqlExecuteClass.Regist(SqlName.Detail.DeleteAll, SqlName.Detail.SubDir, result, this.db);

                    // 長期計画件名に紐づく添付情報を削除
                    // ※添付情報が紐づけられていない場合もあるため、戻り値は見ない
                    TMQUtil.SqlExecuteClass.Regist(SqlName.ExcelPort.DeleteAttachment, SqlName.ExcelPort.SubDir, result, this.db);
                }
            }
            return true;

            // ツリーの階層情報を登録用プロパティに設定
            void setLayerInfo(Dao.ExcelPort.List target)
            {
                // 場所階層
                target.LocationDistrictStructureId = target.DistrictId;
                target.LocationFactoryStructureId = target.FactoryId;
                target.LocationPlantStructureId = target.PlantId;
                target.LocationSeriesStructureId = target.SeriesId;
                target.LocationStrokeStructureId = target.StrokeId;
                target.LocationFacilityStructureId = target.FacilityId;
                // 職種機種
                target.JobKindStructureId = target.JobId;
                target.JobLargeClassficationStructureId = target.LargeClassficationId;
                target.JobMiddleClassficationStructureId = target.MiddleClassficationId;
                target.JobSmallClassficationStructureId = target.SmallClassficationId;
            }
        }

        /// <summary>
        /// ExcelPort機器別管理基準情報登録処理実行
        /// </summary>
        /// <param name="resultList">対象データリスト</param>
        private bool executeRegistDetailListForExcelPort(TMQUtil.ExcelPortUploadCondition uploadCondition, List<Dao.ExcelPort.DeitalList> resultList)
        {
            var now = DateTime.Now;

            var dateFrom = now.ToString(ScheduleDateConditionFormat);
            var dateTo = now.AddMonths(TMQUtil.ComExcelPort.ScheduleMonths - 1).ToString(ScheduleDateConditionFormat);
            bool chkUpd = int.TryParse(this.UserId, out int updUserId);

            // 機器別管理基準内容IDを抽出
            var managementStandardsContentIdList = resultList.Select(x => x.ManagementStandardsContentId.ToString()).ToList();

            var condition = new Dao.ExcelPort.Condition()
            {
                ManagementStandardsContentIdList = string.Join(',', managementStandardsContentIdList),
                ScheduleDateFrom = uploadCondition.ScheduleDateFrom.ToString(ScheduleDateConditionFormat),
                ScheduleDateTo = uploadCondition.ScheduleDateTo.ToString(ScheduleDateConditionFormat)
            };

            // 機器別管理基準内容IDに紐づく保全スケジュールデータを取得
            var scheduleInfoList = TMQUtil.SqlExecuteClass.SelectList<Dao.ExcelPort.DeitalList>(
                SqlName.ExcelPort.GetScheduleInfo, SqlName.ExcelPort.SubDir, condition, this.db);

            // 機器別管理基準内容IDに紐づく履歴完了(●)の保全スケジュール詳細データを取得
            var scheduleListAll = getScheduleDetailListForExcelPort(condition, true);

            // 送信時処理が更新のデータを抽出
            var updateList = resultList.Where(x => x.ProcessId == TMQConst.SendProcessId.Update).ToList();
            for (int i = 0; i < updateList.Count; i++)
            {
                var result = updateList[i];

                // 機器別管理基準情報の長期計画件名IDを更新
                // 共通の更新日時などを設定
                setExecuteConditionByDataClassCommon<Dao.ExcelPort.DeitalList>(ref result, now, updUserId, -1);
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.SelectStandards.UpdateContent, SqlName.SelectStandards.SubDir, result, this.db))
                {
                    return false;
                }

                // 対象期間の保全スケジュール詳細情報を削除(保全履歴完了データ●は除く)
                condition.ManagementStandardsContentId = result.ManagementStandardsContentId;
                // ※削除対象のスケジュールデータが存在しない場合があるため、戻り値は見ない
                TMQUtil.SqlExecuteClass.Regist(SqlName.ExcelPort.DeleteScheduleDetail, SqlName.ExcelPort.SubDir, condition, this.db);

                if (result.ScheduleList.Count == 0)
                {
                    continue;
                }

                // 登録対象データからExcel上の履歴完了データ(●)を除く
                result.ScheduleList = result.ScheduleList.Where(x => x.ScheduleId == (int)TMQConst.MsStructure.StructureId.ScheduleStatus.NoCreate).ToList();

                if (scheduleListAll != null && scheduleListAll.Count > 0)
                {
                    // 保全スケジュール履歴完了データ(●)を取得
                    var completeList = scheduleListAll.Where(x => x.ManagementStandardsContentId == result.ManagementStandardsContentId).ToList();
                    if (completeList.Count > 0)
                    {
                        // スケジュール情報クラスへ詰め替え
                        var mapConfig = new MapperConfiguration(x => x.CreateMap<Dao.ExcelPort.Result, CommonTMQUtil.ScheduleInfo>());
                        var mapper = mapConfig.CreateMapper();
                        var scheduleList = completeList.Select(x => mapper.Map<CommonTMQUtil.ScheduleInfo>(x)).ToList();
                        // 入力データに追加して、スケジュール年月で並び替え
                        result.ScheduleList.AddRange(scheduleList);
                        result.ScheduleList = result.ScheduleList.OrderBy(x => x.ScheduleDate).ToList();
                    }
                }

                // 前月までの繰り返し回数の最大値を取得
                var sequenceCount = TMQUtil.SqlExecuteClass.SelectIntValue(SqlName.ExcelPort.GetSequenceCountMax, SqlName.ExcelPort.SubDir,
                    new { MaintainanceScheduleId = result.MaintainanceScheduleId, ScheduleDate = dateFrom }, this.db);

                foreach (var schedule in result.ScheduleList)
                {
                    // 保全スケジュール詳細データ
                    ComDao.McMaintainanceScheduleDetailEntity detail = new();
                    detail.SequenceCount = ++sequenceCount;
                    if (schedule.ScheduleId == (int)TMQConst.MsStructure.StructureId.ScheduleStatus.NoCreate)
                    {
                        // スケジュール済(○)の場合、入力された保全スケジュール詳細情報を登録
                        // 機器別管理基準内容IDに紐づく開始日がスケジュール日以前で最大の保全スケジュール情報を取得
                        var scheduleInfo = scheduleInfoList.Where(x => x.ManagementStandardsContentId == condition.ManagementStandardsContentId &&
                            x.StartDate.CompareTo(schedule.ScheduleDate) <= 0)
                            .OrderByDescending(x => x.StartDate).FirstOrDefault();
                        if (scheduleInfo == null)
                        {
                            // 存在しない場合は最小の開始日のスケジュール情報を取得
                            scheduleInfo = scheduleInfoList.Where(x => x.ManagementStandardsContentId == condition.ManagementStandardsContentId)
                            .OrderBy(x => x.StartDate).FirstOrDefault();
                        }

                        detail.SummaryId = null;
                        detail.Complition = false;
                        detail.MaintainanceScheduleId = scheduleInfo.MaintainanceScheduleId;
                        detail.ScheduleDate = schedule.ScheduleDate;
                        // 共通の更新日時などを設定
                        setExecuteConditionByDataClassCommon<ComDao.McMaintainanceScheduleDetailEntity>(ref detail, now, updUserId, updUserId);
                        // 新規登録
                        if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Machine.InsertScheduleDetail, SqlName.Machine.SubDir, detail, this.db))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // 保全履歴完了(●)の場合、保全スケジュール詳細の繰り返し回数を更新
                        // 共通の更新日時などを設定
                        setExecuteConditionByDataClassCommon<ComDao.McMaintainanceScheduleDetailEntity>(ref detail, now, updUserId, -1);
                        detail.MaintainanceScheduleDetailId = schedule.MaintainanceScheduleDetailId;
                        // 更新
                        if (!TMQUtil.SqlExecuteClass.Regist(SqlName.ExcelPort.UpdateSequenceCount, SqlName.ExcelPort.SubDir, detail, this.db))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #endregion
    }
}