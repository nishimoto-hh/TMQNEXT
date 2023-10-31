using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MC0001.BusinessLogicDataClass_MC0001;
using DbTransaction = System.Data.IDbTransaction;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using FunctionTypeId = CommonTMQUtil.CommonTMQConstants.Attachment.FunctionTypeId;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;

namespace BusinessLogic_MC0001
{
    /// <summary>
    /// 機器台帳 業務ロジッククラス
    /// </summary>
    public partial class BusinessLogic_MC0001 : CommonBusinessLogicBase
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
            /// <summary>機器選択モーダル画面</summary>
            public const byte SelectMachine = 3;
            /// <summary>機種別仕様編集モーダル画面</summary>
            public const byte SpecEdit = 4;
        }

        /// <summary>
        /// テーブル名称
        /// </summary>
        private static class TableName
        {
            /// <summary>テーブル名：mc_machine</summary>
            public const string TableMcMachine = "mc_machine";
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            // 機能内で不要なSQLは定義は不要です

            /// <summary>SQL名：一覧取得</summary>
            public const string GetList = "GetMachineList";
            /// <summary>SQL名：ExcelPort機器台帳取得</summary>
            public const string GetExcelPortMachineList = "GetExcelPortMachineList";
            /// <summary>SQL名：ExcelPort機器別管理基準取得</summary>
            public const string GetExcelPortManagementStandard = "GetExcelPortManagementStandard";
            /// <summary>SQL名：一覧取得</summary>
            //public const string GetListCount = "Count_MachineList";
            /// <summary>SQL名：出力一覧取得</summary>
            public const string GetListForReport = "[テーブル名]_GetListForReport";
            /// <summary>SQL名：機番情報取得</summary>
            public const string GetMachineDetail = "MachineDetail";
            /// <summary>SQL名：機器別管理基準_長期計画存在取得</summary>
            public const string GetChkLongPlan = "GetLongPlan";
            /// <summary>SQL名：機器別管理基準_保全活動存在取得</summary>
            public const string GetChkMsSummary = "GetMsSummary";
            /// <summary>SQL名：親子構成存在取得</summary>
            public const string GetChkParentInfo = "GetParentInfo";
            /// <summary>SQL名：親子構成存在取得</summary>
            public const string GetChkLoopInfo = "GetLoopInfo";
            /// <summary>SQL名：親子構成存在取得</summary>
            public const string GetChkAccessoryInfo = "GetAccessoryInfo";
            /// <summary>SQL名：機器レベル取得</summary>
            public const string GetMachineLevel = "GetMachineLevel";

            /// <summary>SQL名：機番情報登録</summary>
            public const string InsertMachineInfo = "InsertMachineInfo";
            /// <summary>SQL名：機器情報登録</summary>
            public const string InsertEquipmentInfo = "InsertEquipmentInfo";
            /// <summary>SQL名：適用法規登録</summary>
            public const string InsertApplicableLawsInfo = "InsertApplicableLaws";
            /// <summary>SQL名：親子構成登録</summary>
            public const string InsertParentInfo = "InsertParentInfo";
            /// <summary>SQL名：ループ構成登録</summary>
            public const string InsertLoopInfo = "InsertLoopInfo";
            /// <summary>SQL名：付属品構成登録</summary>
            public const string InsertAccessoryInfo = "InsertAccessoryInfo";
            /// <summary>SQL名：機番情報更新</summary>
            public const string UpdateMachineInfo = "UpdateMachineInfo";
            /// <summary>SQL名：機器情報更新</summary>
            public const string UpdateEquipmentInfo = "UpdateEquipmentInfo";
            /// <summary>SQL名：機番情報削除/summary>
            public const string DeleteMachineInfo = "DeleteMachineInfo";
            /// <summary>SQL名：機器情報削除/summary>
            public const string DeleteEquipmentInfo = "DeleteEquipmentInfo";
            /// <summary>SQL名：機器情報削除/summary>
            public const string DeleteEquipmentSpecInfo = "DeleteEquipmentSpecInfo";
            /// <summary>SQL名：適用法規削除/summary>
            public const string DeleteApplicableLawsInfo = "DeleteApplicableLaws";
            /// <summary>SQL名：保全スケジュール詳細削除/summary>
            public const string DeleteMaintainanceScheduleDetail = "DeleteMaintainanceScheduleDetail";
            /// <summary>SQL名：保全スケジュール削除/summary>
            public const string DeleteMaintainanceSchedule = "DeleteMaintainanceSchedule";
            /// <summary>SQL名：機器別管理基準内容削除/summary>
            public const string DeleteManagementStandardsComponent = "DeleteManagementStandardsComponent";
            /// <summary>SQL名：機器別管理基準部位削除/summary>
            public const string DeleteManagementStandardsContent = "DeleteManagementStandardsContent";
            /// <summary>SQL名：機番使用部品削除/summary>
            public const string DeleteMachineUseParts = "DeleteMachineUseParts";
            /// <summary>SQL名：親子構成削除/summary>
            public const string DeleteParentInfo = "DeleteParentInfo";
            /// <summary>SQL名：親子構成削除/summary>
            public const string DeleteLoopInfo = "DeleteLoopInfo";
            /// <summary>SQL名：親子構成削除/summary>
            public const string DeleteAccessoryInfo = "DeleteAccessoryInfo";
            /// <summary>SQL名：MP情報削除/summary>
            public const string DeleteMpInfo = "DeleteMpInfo";
            /// <summary>SQL名：機番添付削除/summary>
            public const string DeleteMachineAttachment = "DeleteMachineAttachment";
            /// <summary>SQL名：機器添付削除/summary>
            public const string DeleteEquipmentAttachment = "DeleteEquipmentAttachment";
            /// <summary>SQL名：機器別管理基準添付削除/summary>
            public const string DeleteManagementStandardAttachment = "DeleteManagementStandardAttachment";
            /// <summary>SQL名：MP情報添付削除/summary>
            public const string DeleteMpInfoAttachment = "DeleteMpInfoAttachment";

            /// <summary>SQL名：機器情報登録</summary>
            public const string InsertEquipInfo = "InsertEquipInfo";
            /// <summary>SQL名：機器情報更新</summary>
            public const string UpdateEquipInfo = "UpdateEquipInfo";
            /// <summary>SQL名：機器情報削除/summary>
            public const string DeleteEquipInfo = "DeleteEquipInfo";
            /// <summary>SQL名：機種別仕様情報登録</summary>
            public const string InsertSpecInfo = "InsertSpecInfo";
            /// <summary>SQL名：機種別仕様情報更新</summary>
            public const string UpdateSpecInfo = "UpdateSpecInfo";
            /// <summary>SQL名：機種別仕様情報削除/summary>
            public const string DeleteSpecInfo = "DeleteSpecInfo";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Machine";
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
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            // 機能内で不要な処理対象は定義は不要です

            /// <summary>
            /// 検索条件の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string SearchCondition = "SearchCondition";
            /// <summary>
            /// 機器台帳 機器台帳一覧
            /// </summary>
            public const string SearchList = "BODY_020_00_LST_0";
            /// <summary>
            /// 機器台帳 非表示一覧
            /// </summary>
            public const string HiddenInfo = "BODY_030_00_LST_0";
            /// <summary>
            /// 機器台帳詳細 キー情報非表示一覧
            /// </summary>
            public const string Detail00 = "BODY_000_00_LST_1";
            /// <summary>
            /// 機器台帳詳細 非表示一覧
            /// </summary>
            public const string Detail07 = "BODY_007_00_LST_1";
            /// <summary>
            /// 機器台帳詳細 機番情報 場所階層(地区～設備)
            /// </summary>
            public const string Detail10 = "BODY_010_00_LST_1";
            /// <summary>
            /// 機器台帳詳細 機番情報 (機器番号～保全方式)
            /// </summary>
            public const string Detail20 = "BODY_020_00_LST_1";
            /// <summary>
            /// 機器台帳詳細 機番情報 (適用法規～機番メモ)
            /// </summary>
            public const string Detail30 = "BODY_030_00_LST_1";
            /// <summary>
            /// 機器台帳詳細 機器情報 (機種大分類～機種小分類)
            /// </summary>
            public const string Detail40 = "BODY_040_00_LST_1";
            /// <summary>
            /// 機器台帳詳細 機器情報 (使用区分～点検種別毎管理)
            /// </summary>
            public const string Detail50 = "BODY_050_00_LST_1";
            /// <summary>
            /// 機器台帳詳細 機器情報 (機器メモ)
            /// </summary>
            public const string Detail60 = "BODY_060_00_LST_1";
            /// <summary>
            /// 機器台帳詳細 機器仕様
            /// </summary>
            public const string Detail70 = "BODY_070_00_LST_1";
            /// <summary>
            /// 機器台帳詳細編集 機番情報 場所階層(地区～設備)
            /// </summary>
            public const string EditDetail00 = "BODY_000_00_LST_2";
            /// <summary>
            /// 機器台帳詳細編集 機番情報 場所階層(機器番号～保全方式)
            /// </summary>
            public const string EditDetail10 = "BODY_010_00_LST_2";
            /// <summary>
            /// 機器台帳詳細編集 機番情報(適用法規～機番メモ)
            /// </summary>
            public const string EditDetail20 = "BODY_020_00_LST_2";
            /// <summary>
            /// 機器台帳詳細編集 機器情報(機種大分類～機種小分類)
            /// </summary>
            public const string EditDetail30 = "BODY_030_00_LST_2";
            /// <summary>
            /// 機器台帳詳細編集 機器情報(使用区分～点検種別毎管理)
            /// </summary>
            public const string EditDetail40 = "BODY_040_00_LST_2";
            /// <summary>
            /// 機器台帳詳細編集 機器情報(機器メモ)
            /// </summary>
            public const string EditDetail50 = "BODY_050_00_LST_2";
            /// <summary>
            /// 機種別仕様編集 機器仕様
            /// </summary>
            public const string EditSpecDetail00 = "BODY_000_00_LST_4";

            /// <summary>帳票(EXCEL)</summary>
            public const string ReportExcel = "ReportExcel";
            /// <summary>帳票(PDF)</summary>
            public const string ReportPdf = "ReportPdf";
            /// <summary>帳票(CSV)</summary>
            public const string ReportCsv = "ReportCsv";

            /// <summary>取込(EXCEL)</summary>
            public const string UploadExcel = "UploadExcel";
            /// <summary>取込(CSV)</summary>
            public const string UploadCsv = "UploadCsv";

            /// <summary>ExcelPortアップロード</summary>
            public const string ExcelPortUpload = "LIST_000_1";
        }

        /// <summary>
        /// 参照画面処理対象コントロールID
        /// </summary>
        private static ReadOnlyCollection<string> registTargetCtrlIdDetail = new[]
        {
          "BODY_090_00_LST_1", // 保全項目一覧
          "BODY_120_00_LST_1", // 様式１一覧
          "BODY_140_00_LST_1", // スケジューリング条件
          "BODY_160_00_LST_1", // スケジューリング一覧
          "BODY_170_00_LST_1", // 点検種別毎スケジューリング一覧
          "BODY_230_00_LST_1", // 使用部品
          "BODY_260_00_LST_1", // 親子構成
          "BODY_270_00_LST_1", // ループ構成
          "BODY_290_00_LST_1"  // MP情報
        }.ToList().AsReadOnly();

        /// <summary>
        /// ボタン名
        /// </summary>
        private static class buttonName
        {
            /// <summary>機器台帳詳細 削除ボタン</summary>
            public const string BtnDelete = "Delete";
            /// <summary>機器別管理基準 保全項目一覧削除ボタン(非表示)</summary>
            public const string BtnDeleteManagementStandard = "DeleteManagementStandard";
            /// <summary>出力（様式１）</summary>
            public const string BtnOutput = "Output";
            /// <summary>出力（スケジューリング）</summary>
            public const string BtnOutputSchedule = "OutputSchedule";
            /// <summary>出力（スケジューリング）</summary>
            public const string BtnOutputLankSchedule = "OutputLankSchedule";
        }

        /// <summary>
        /// 機器台帳参照画面グループ番号
        /// </summary>
        private static class DetailFormTargetGrpNo
        {
            /// <summary>機番情報</summary>
            public const short MachineInfo = 201;
            /// <summary>機器情報</summary>
            public const short EquipInfo = 202;
        }

        /// <summary>
        /// 機種別仕様参照画面グループ番号
        /// </summary>
        private static class DetailSpecFormTargetGrpNo
        {
            /// <summary>機種別仕様情報</summary>
            public const short SpecInfo = 203;

        }
        /// <summary>
        /// 機器台帳編集画面グループ番号
        /// </summary>
        private static class EditFormTargetGrpNo
        {
            /// <summary>機番情報</summary>
            public const short MachineInfo = 301;
            /// <summary>機器情報</summary>
            public const short EquipInfo = 302;
        }

        /// <summary>
        /// 機種別仕様編集画面グループ番号
        /// </summary>
        private static class EditSpecFormTargetGrpNo
        {
            /// <summary>機種別仕様情報</summary>
            public const short SpecInfo = 303;

        }
        /// <summary>
        /// 編集画面モード
        /// </summary>
        private static class DetailFormType
        {
            /// <summary>参照画面</summary>
            public const byte Detail = 0;
            /// <summary>修正画面</summary>
            public const byte Edit = 1;
            /// <summary>複写画面</summary>
            public const byte Copy = 2;
        }
        /// <summary>
        /// 点検種別毎管理フラグ
        /// </summary>
        private bool flgMaintainanceKindManage = false;

        /// <summary>
        /// スケジュール帳票情報
        /// </summary>
        private static class ReportScheduleInfo
        {
            // 処理対象帳票ID 長期スケジュール表
            public const string ReportIdRP0090 = "RP0090";
            // 処理対象帳票ID 年度スケジュール表
            public const string ReportIdRP0100 = "RP0100";
            // 機能ID 機器別長期計画
            public const string PgmIdLN0002 = "LN0002";
            // VAL1:表示単位
            public const string ScheduleCondType = "1";
            // VAL2:表示年度
            public const string ScheduleCondYear = "2";
            // VAL3:表示期間
            public const string ScheduleCondSpan = "3";
            // VAL4:月度表示ID
            public const string ScheduleCondMondStructureId = "4";
            // スケジュール表示単位 汎用項目SEQ
            public const int SeqNo = 1;
        }

        /// <summary>
        /// ExcelPort機器台帳シート情報
        /// </summary>
        private static class ExcelPortMachineListInfo
        {
            // データ開始行
            public const int StartRowNo = 4;
            // 送信時処理列番号
            public const int ProccesColumnNo = 4;
            // 機器レベル列番号
            public const int EquipmentLevelColumnNo = 28;
        }
        /// <summary>
        /// ExcelPort機器台帳シート情報
        /// </summary>
        private static class ExcelPortManagementStandardListInfo
        {
            // データ開始行
            public const int StartRowNo = 4;
            // 送信時処理列番号
            public const int ProccesColumnNo = 5;
            // 開始日列番号
            public const int StratDateColumnNo = 39;
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_MC0001() : base()
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

            switch (this.FormNo)
            {
                case FormType.List:
                    // 一覧画面
                    this.ResultList = new();
                    //CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
                    //if (compareId.IsBack())
                    //{
                    //    // 戻る場合、検索結果のコントロールタイプに応じて検索処理を切り替える
                    //    //return InitSearch();
                    //}
                    return InitSearch();
                case FormType.Detail:
                    // 詳細画面
                    if (!initDetail())
                    {
                        return -1;
                    }
                    return 0;
                case FormType.Edit:
                    // 詳細編集画面
                    if (!initEdit())
                    {
                        return -1;
                    }
                    return 0;
                case FormType.SelectMachine:
                    // 機器選択画面
                    if (!initSelectMachine())
                    {
                        return -1;
                    }
                    return 0;
                case FormType.SpecEdit:
                    // 機種別仕様編集画面
                    if (!searchSpec())
                    {
                        return -1;
                    }
                    return 0;
                default:
                    // 到達不可
                    return 0;
            }

            // 初期検索不要の場合はこちら↓
            //return 0;
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
                        return -1;
                    }
                    break;
                case FormType.SelectMachine: // 機器選択画面
                    if (!searchSelectMachine())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case FormType.Detail:
                    // 詳細画面
                    if (!initDetail())
                    {
                        return -1;
                    }
                    return 0;
                default:
                    return -1;
            }
            return 0;
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
                // 登録処理実行
                return Regist();
            }
            else if (compareId.IsDelete())
            {
                // 削除処理実行
                return Delete();
            }
            return ComConsts.RETURN_RESULT.NG;
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
                case FormType.Detail:
                    // 登録・更新処理
                    resultRegist = executeRegistDetail();
                    break;
                case FormType.Edit:
                    // 登録・更新処理
                    resultRegist = executeRegistEdit();
                    break;
                case FormType.SelectMachine:
                    resultRegist = executeRegistSelectMachine();
                    break;
                case FormType.SpecEdit:
                    resultRegist = executeRegistSpecEdit();
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
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「登録処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                }
            }
            else
            {
                // 確認ステータスなら-1
                if (this.Status == CommonProcReturn.ProcStatus.Confirm)
                {
                    return ComConsts.RETURN_RESULT.NG;
                }
                else
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                    //「登録処理に成功しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200003 });
                }
            }
            return resultRegist ? ComConsts.RETURN_RESULT.OK : ComConsts.RETURN_RESULT.NG;

        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            this.ResultList = new();

            // 機番IDを取得
            // キー情報取得
            long machineId = getMachineId(false);

            // 機器情報削除処理か各タブ一覧の削除処理か
            if (this.CtrlId == buttonName.BtnDelete)
            {
                // 機器台帳情報削除処理
                if (!deleteMachineDetail())
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    if (string.IsNullOrEmpty(this.MsgId))
                    {
                        // 「削除処理に失敗しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911110001 });
                    }
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else if (this.CtrlId == buttonName.BtnDeleteManagementStandard)
            {
                // 保全項目一覧 削除処理
                if (!deleteManagementStandardSingle())
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    if (string.IsNullOrEmpty(this.MsgId))
                    {
                        // 「削除処理に失敗しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911110001 });
                    }
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else if (this.CtrlId == DeleteUseParts)
            {
                // 使用部品一覧 削除処理
                if (!deleteUseParts())
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    if (string.IsNullOrEmpty(this.MsgId))
                    {
                        // 「削除処理に失敗しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911110001 });
                    }
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else if (this.CtrlId == DeleteParent)
            {
                // 親子構成一覧 削除処理
                if (!updateConstitutionInfo(true))
                {
                    if (string.IsNullOrEmpty(this.MsgId))
                    {
                        // 「削除処理に失敗しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911110001 });
                    }
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else if (this.CtrlId == DeleteLoop)
            {
                // ループ構成一覧 削除処理
                if (!updateConstitutionInfo(false))
                {
                    if (string.IsNullOrEmpty(this.MsgId))
                    {
                        // 「削除処理に失敗しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911110001 });
                    }
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else if (this.CtrlId == DeleteMpInfo)
            {
                // MP情報一覧 削除処理
                if (!deleteMpInfo())
                {
                    if (string.IsNullOrEmpty(this.MsgId))
                    {
                        // 「削除処理に失敗しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911110001 });
                    }
                    return ComConsts.RETURN_RESULT.NG;
                }
            }

            // クリックされたボタンが機番情報の削除では無ければ詳細画面の再検索処理
            if (this.CtrlId != buttonName.BtnDelete)
            {
                if (!initDetail())
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「削除処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911110001 });
            // 正常終了
            return ComConsts.RETURN_RESULT.OK;

        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>0:正常, -1:エラー</returns>
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
            int result = 0;

            int reportFactoryId = 0;
            string reportId = "";
            string pgmId = string.Empty;

            dynamic searchCondition = null;

            Dictionary<int, List<CommonSTDUtil.CommonBusinessLogic.SelectKeyData>> dicSelectKeyDataList = new Dictionary<int, List<SelectKeyData>>();
            string taretListCtrlId = string.Empty;

            // 長期スケジュール用オプションの設定
            TMQUtil.Option option = null;

            switch (this.CtrlId)
            {
                // エクセル出力テスト
                case "Report":
                    taretListCtrlId = TargetCtrlId.SearchList;     // 一覧のコントールID
                    // ページ情報取得
                    var pageInfo = GetPageInfo(
                        TargetCtrlId.SearchList,     // 一覧のコントールID
                        this.pageInfoList);          // ページ情報リスト

                    // 検索条件データ取得
                    getSearchConditionForReport(pageInfo, out searchCondition);

                    reportId = "RP0010";
                    pgmId = this.PgmId;
                    break;

                // 機器別管理基準
                case buttonName.BtnOutput:
                    taretListCtrlId = TargetCtrlId.Detail00;     // 一覧のコントールID

                    reportId = "RP0060";
                    pgmId = this.PgmId;
                    break;
                // スケジューリング
                case buttonName.BtnOutputLankSchedule:
                case buttonName.BtnOutputSchedule:
                    taretListCtrlId = TargetCtrlId.Detail00;     // 一覧のコントールID

                    // 画面の内容
                    var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlIdManagementStandard.DetailScheduleConditionList140);

                    // オプションデータを抽出して条件クラスにセットする
                    TMQDao.ScheduleList.Condition target = new();
                    string[] values = targetDic["VAL" + ReportScheduleInfo.ScheduleCondSpan].ToString().Split(ComUtil.FromToDelimiter);
                    target.ExtensionData = getItemExData((int)TMQConst.MsStructure.GroupId.ScheduleDisp,
                                                        ReportScheduleInfo.SeqNo,
                                                        int.Parse(targetDic["VAL" + ReportScheduleInfo.ScheduleCondType].ToString()));
                    target.ScheduleStartYear = int.Parse(targetDic["VAL" + ReportScheduleInfo.ScheduleCondYear].ToString());
                    target.ScheduleUnit = int.Parse(targetDic["VAL" + ReportScheduleInfo.ScheduleCondType].ToString());
                    target.ScheduleYear = "";
                    target.ScheduleYearFrom = int.Parse(values[0]);
                    target.ScheduleYearTo = int.Parse(values[1]);

                    // 長期スケジュール用オプションの設定
                    option = new TMQUtil.Option();

                    // 年度開始月
                    int monthStartNendo = getYearStartMonth();
                    Dao.Schedule.SearchCondition cond = new(target, monthStartNendo, this.LanguageId);
                    cond.FactoryIdList = TMQUtil.GetFactoryIdList(this.UserId, this.db);

                    // スケジュール表示単位 1:月度、2:年度
                    if (cond.DisplayUnit == CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleDisplayUnit.Month)
                    {
                        // 年度スケジュール表の場合、1:月度
                        option.DisplayUnit = (int)TMQConst.MsStructure.StructureId.ScheduleDisplayUnit.Month;
                        cond.DisplayUnit = TMQConst.MsStructure.StructureId.ScheduleDisplayUnit.Month;
                        reportId = ReportScheduleInfo.ReportIdRP0100;
                    }
                    else
                    {
                        // 長期スケジュール表、2:年度
                        option.DisplayUnit = (int)TMQConst.MsStructure.StructureId.ScheduleDisplayUnit.Year;
                        cond.DisplayUnit = TMQConst.MsStructure.StructureId.ScheduleDisplayUnit.Year;
                        reportId = ReportScheduleInfo.ReportIdRP0090;
                    }
                    // 開始年月日
                    option.StartDate = cond.ScheduleStart;
                    // 終了年月日
                    if (reportId == ReportScheduleInfo.ReportIdRP0100)
                    {
                        // 年度スケジュールの場合
                        option.EndDate = ComUtil.GetNendoLastDay(cond.ScheduleStart, monthStartNendo);
                        cond.ScheduleEnd = ComUtil.GetNendoLastDay(cond.ScheduleStart, monthStartNendo);
                    }
                    else
                    {
                        option.EndDate = cond.ScheduleEnd;
                    }

                    // 出力方式 1:件名別、2:機番別、3:予算別
                    option.OutputMode = TMQUtil.ComReport.OutputMode2;
                    // 年度開始月
                    option.MonthStartNendo = monthStartNendo;
                    // 検索条件クラス
                    option.Condition = cond;
                    // 固定で機器別長期計画のプログラムIDを指定
                    pgmId = ReportScheduleInfo.PgmIdLN0002;

                    break;
            }

            // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
            int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
            reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

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
                    taretListCtrlId,             // 一覧のコントールID
                    keyInfo,                     // 設定したキー情報
                    this.resultInfoDictionary,   // 画面データ
                    false);                      // 未選択データを含める
                // シートNoをキーとして帳票用選択キーデータを保存する
                dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
            }

            // エクセル出力共通処理
            TMQUtil.CommonOutputExcel(
                reportFactoryId,             // 工場ID
                pgmId,                  // プログラムID
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

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return ComConsts.RETURN_RESULT.OK;
            /// <summary>
            /// アイテム拡張マスタから拡張データを取得する
            /// </summary>>
            /// <param name="structureGroupId">構成グループID</param>
            /// <param name="seq">連番</param
            /// <param name="structureId">構成ID</param>
            /// <returns>拡張データ</returns>
            string getItemExData(short structureGroupId, short seq, int structureId)
            {
                string result = null;

                // 構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                // 構成グループID
                param.StructureGroupId = (int)structureGroupId;
                // 連番
                param.Seq = seq;
                // 構成アイテム、アイテム拡張マスタ情報取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
                if (list != null)
                {
                    // 取得情報から拡張データを取得
                    result = list.Where(x => x.StructureId == structureId).Select(x => x.ExData).FirstOrDefault();
                }
                return result;
            }

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

            //TODO: 個別データ検索処理
            IList<Dictionary<string, object>> dataList = null;

            if (excelPort.DownloadCondition.SheetNo == 1)
            {
                // 機器台帳
                // ページ情報取得
                var pageInfo = GetPageInfo(TargetCtrlId.SearchList, this.pageInfoList);

                //// SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetExcelPortMachineList, out string baseSql);

                //// 場所分類＆職種機種＆詳細検索条件取得
                //if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
                //{
                //    // 「ダウンロード処理に失敗しました。」
                //    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                //    return ComConsts.RETURN_RESULT.NG;
                //}
                TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
                listPf.GetAttachmentSql(new List<FunctionTypeId> { FunctionTypeId.Machine, FunctionTypeId.Equipment });
                var structuregroupList = new List<GroupId>
                {
                    GroupId.Location, GroupId.Job, GroupId.MachineLevel, GroupId.Importance, GroupId.Conservation, GroupId.Manufacturer, GroupId.UseSegment, GroupId.ApplicableLaws
                };
                listPf.GetCreateTranslation(); // テーブル作成
                listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
                                                                          //listPf.GetInsertLayerOnly(GroupId.Job, (int)TMQConst.MsStructure.StructureLayerNo.Job.Job);
                                                                          //listPf.AddTempTable(SqlName.SubDir, SqlName.List.CreateTempForGetList, SqlName.List.InsertTempForGetList);
                listPf.RegistTempTable(); // 登録
                //SQLパラメータに言語ID設定
                string whereSql = null;
                dynamic whereParam = new ExpandoObject();
                whereParam.LanguageId = this.LanguageId;

                // 一覧検索SQL文の取得
                string executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, null, false, -1);
                var selectSql = new StringBuilder(executeSql);
                selectSql.AppendLine("ORDER BY");
                selectSql.AppendLine("machine_no ");
                selectSql.AppendLine(",machine_name ");
                // 一覧検索実行
                IList<Dao.excelPortMachineList> results = db.GetListByDataClass<Dao.excelPortMachineList>(selectSql.ToString(), whereParam);
                if (results == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return ComConsts.RETURN_RESULT.NG;
                }

                // Dicitionalyに変換
                dataList = ComUtil.ConvertClassToDictionary<Dao.excelPortMachineList>(results);

            }
            else if (excelPort.DownloadCondition.SheetNo == 2)
            {
                // 機器別管理基準
                // ページ情報取得
                var pageInfo = GetPageInfo(TargetCtrlId.SearchList, this.pageInfoList);

                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetExcelPortManagementStandard, out string baseSql);

                ////// 場所分類＆職種機種＆詳細検索条件取得
                //if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
                //{
                //    // 「ダウンロード処理に失敗しました。」
                //    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                //    return ComConsts.RETURN_RESULT.NG;
                //}
                TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
                //listPf.GetAttachmentSql(new List<FunctionTypeId> { FunctionTypeId.Machine, FunctionTypeId.Equipment });
                var structuregroupList = new List<GroupId>
                {
                    GroupId.Location, GroupId.Job, GroupId.MachineLevel, GroupId.SiteMaster, GroupId.Importance,  GroupId.InspectionDetails, GroupId.Conservation, GroupId.MaintainanceDivision, GroupId.MaintainanceKind, GroupId.ScheduleType
                };
                listPf.GetCreateTranslation(); // テーブル作成
                listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
                                                                          //listPf.GetInsertLayerOnly(GroupId.Job, (int)TMQConst.MsStructure.StructureLayerNo.Job.Job);
                                                                          //listPf.AddTempTable(SqlName.SubDir, SqlName.List.CreateTempForGetList, SqlName.List.InsertTempForGetList);
                listPf.RegistTempTable(); // 登録
                //SQLパラメータに言語ID設定
                string whereSql = null;
                dynamic whereParam = new ExpandoObject();
                whereParam.LanguageId = this.LanguageId;

                // 一覧検索SQL文の取得
                string executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, null, false, -1);
                var selectSql = new StringBuilder(executeSql);
                selectSql.AppendLine("ORDER BY");
                selectSql.AppendLine("machine_no ");
                selectSql.AppendLine(",machine_name ");
                selectSql.AppendLine(",inspection_site_structure_id ");
                selectSql.AppendLine(",inspection_content_structure_id ");

                // 一覧検索実行
                IList<Dao.excelPortManagementStandardResult> results = db.GetListByDataClass<Dao.excelPortManagementStandardResult>(selectSql.ToString(), whereParam);
                if (results == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return ComConsts.RETURN_RESULT.NG;
                }

                // Dicitionalyに変換
                dataList = ComUtil.ConvertClassToDictionary<Dao.excelPortManagementStandardResult>(results);

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
            if (excelPort.UploadCondition.SheetNo == 1)
            {
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, TargetCtrlId.ExcelPortUpload,
                    out List<BusinessLogicDataClass_MC0001.excelPortMachineList> resultList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // エラー情報リスト
                List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();

                // 機器台帳登録チェック処理
                if (!checkExcelPortRegistDetail(ref resultList, ref errorInfoList))
                {
                    if (errorInfoList.Count > 0)
                    {
                        // エラー情報シートへ設定
                        excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    }
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 機器台帳登録処理
                if (!executeExcelPortRegistDetail(resultList))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

            }
            else if (excelPort.UploadCondition.SheetNo == 2)
            {
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, TargetCtrlId.ExcelPortUpload,
                    out List<BusinessLogicDataClass_MC0001.excelPortManagementStandardResult> resultList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // エラー情報リスト
                List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();

                // 機器別管理基準登録処理(チェック処理含む)
                if (!executeExcelPortManagementStandardRegist(resultList, ref errorInfoList))
                {
                    if (errorInfoList.Count > 0)
                    {
                        // エラー情報シートへ設定
                        excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    }
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

            }

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
        private bool searchList()
        {
            // 非表示項目
            // 変更管理ボタンの表示制御用フラグ
            setHistoryManagementFlg(TargetCtrlId.HiddenInfo);

            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.SearchList, this.pageInfoList);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetList, out string baseSql);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
            {
                return false;
            }
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);
            listPf.GetAttachmentSql(new List<FunctionTypeId> { FunctionTypeId.Machine, FunctionTypeId.Equipment });
            var structuregroupList = new List<GroupId>
            {
                GroupId.Location, GroupId.Job, GroupId.MachineLevel, GroupId.Importance, GroupId.Conservation, GroupId.Manufacturer, GroupId.UseSegment, GroupId.ApplicableLaws
            };
            //listPf.GetTranslation(structuregroupList);
            //listPf.GetTranslationLayer(GroupId.Job, (int)Const.MsStructure.StructureLayerNo.Job.Job);
            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            //listPf.GetInsertLayerOnly(GroupId.Job, (int)TMQConst.MsStructure.StructureLayerNo.Job.Job);
            //listPf.AddTempTable(SqlName.SubDir, SqlName.List.CreateTempForGetList, SqlName.List.InsertTempForGetList);
            listPf.RegistTempTable(); // 登録
            //SQLパラメータに言語ID設定
            whereParam.LanguageId = this.LanguageId;
            // 総件数を取得
            int cnt = db.GetCount(TMQUtil.GetCountSql(new ComDao.McMachineEntity().TableName, whereParam), whereParam);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「該当データがありません。」
                this.MsgId = GetResMessage("941060001");
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, null, cnt, isDetailConditionApplied);
                return false;
            }

            // 一覧検索SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, null, isDetailConditionApplied, pageInfo.SelectMaxCnt);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY");
            selectSql.AppendLine("machine_no ");
            selectSql.AppendLine(",machine_name ");
            //log("SQL実行開始", sw);
            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「該当データがありません。」
                this.MsgId = GetResMessage("941060001");
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, null, 0, isDetailConditionApplied);
                return false;
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClassForList<Dao.searchResult>(pageInfo, results, cnt, isDetailConditionApplied))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// 画面初期化(詳細画面)
        /// </summary>
        private bool initDetail()
        {
            this.ResultList = new();
            // キー情報取得
            long machineId = getMachineId(false);
            // 機器ID取得
            long equipmentId = getEquipmentId(false);

            // 機番情報
            searchKiban(machineId, DetailFormType.Detail, out int factoryId, equipmentId);

            // 機種別仕様
            searchSpec();

            // 機器別管理基準
            bool manageScheduleCondError = false;
            searchManagementStandard(machineId, ref manageScheduleCondError);

            // 長期計画
            bool longPlanScheduleCondError = false;
            searchLongPlan(machineId, ref longPlanScheduleCondError);

            // 保全活動
            searchMaintainanceActivity(machineId);

            // 使用部品
            searchUseParts(machineId);

            // 構成機器
            searchConstitution(machineId);

            // MP情報
            searchMPInfo(machineId);

            // ★画面定義の翻訳情報取得★
            GetContorlDefineTransData(factoryId);

            // スケジュール表示条件エラーかどうか
            if (manageScheduleCondError == true || longPlanScheduleCondError == true)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 選択機番ID取得
        /// </summary>
        /// <param name="editFlg">編集画面:True,参照画面:False</param>
        /// <returns>機番ID</returns>
        private long getMachineId(bool editFlg)
        {
            Dictionary<string, object> targetDictionary;
            if (editFlg)
            {
                // 参照画面からの遷移の場合:参照画面で表示していた情報より機番IDを取得(js側にてsearchConditionDictionaryへセット済み)
                targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.Detail20);
            }
            else
            {
                // 一覧画面からの遷移の場合:機器台帳一覧の選択された行より機番IDを取得
                targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.SearchList);
                if (targetDictionary == null)
                {
                    if (this.CtrlId == "Regist")
                    {
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.EditDetail10);
                    }
                    if (targetDictionary == null)
                    {
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.Detail20);
                    }
                }

                // 登録後の初期化処理のパターン
                if (string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "machine_id")))
                {
                    if (this.CtrlId == "Regist")
                    {
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.EditDetail10);
                    }
                    else if (this.CtrlId == "ReSearch")
                    {
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.Detail00);
                    }
                    else if (this.CtrlId == "SearchDispCount")
                    {
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.Detail00);
                    }
                }
            }

            return long.Parse(getDictionaryKeyValue(targetDictionary, "machine_id"));
        }

        /// <summary>
        /// 選択機器ID取得
        /// </summary>
        /// <param name="editFlg">編集画面:True,参照画面:False</param>
        /// <returns>機番ID</returns>
        private long getEquipmentId(bool editFlg)
        {
            Dictionary<string, object> targetDictionary;
            if (editFlg)
            {
                // 参照画面からの遷移の場合:参照画面で表示していた情報より機器IDを取得(js側にてsearchConditionDictionaryへセット済み)
                targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.Detail20);
            }
            else
            {
                // 一覧画面からの遷移の場合:機器台帳一覧の選択された行より機器IDを取得
                targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.SearchList);
                if (targetDictionary == null)
                {
                    if (this.CtrlId == "Regist")
                    {
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.EditDetail10);
                    }
                    if (targetDictionary == null)
                    {
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.Detail20);
                    }
                }

                if (string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "equipment_id")))
                {
                    // 登録後の初期化処理のパターン
                    if (this.CtrlId == "Regist")
                    {
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.EditDetail10);
                    }
                    else if (this.CtrlId == "ReSearch")
                    {
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.Detail00);
                    }
                    else if (this.CtrlId == "SearchDispCount")
                    {
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.Detail00);
                    }
                }

                // 取得できなければ機番IDから取得
                if (string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "equipment_id")))
                {
                    // 検索条件取得
                    dynamic whereParam = null; // WHERE句パラメータ
                    string sql;
                    // 一覧検索SQL文の取得
                    TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
                    whereParam = new { MachineId = getMachineId(false) };

                    // 一覧検索実行
                    IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
                    if (results == null || results.Count == 0)
                    {
                        return -1;
                    }

                    return (long)results[0].EquipmentId;
                }

            }
            return long.Parse(getDictionaryKeyValue(targetDictionary, "equipment_id"));
        }

        /// <summary>
        /// 詳細情報検索(機番情報)
        /// </summary>
        /// <param name="machineId">機器ID</param>
        /// <param name="mode">参照画面:0,修正画面:1,複写画面:2</param>
        /// <param name="factoryId">out 機番情報の工場ID</param>
        /// <param name="equipmentId">省略可能　機番ID</param>
        /// <returns>処理結果：正常ならTrue、異常ならFalse</returns>
        private bool searchKiban(long machineId, int mode, out int factoryId, long equipmentId = -1)
        {
            factoryId = -1;
            flgMaintainanceKindManage = false;
            // 検索条件取得
            dynamic whereParam = null; // WHERE句パラメータ
            string sql;
            // 一覧検索SQL文の取得
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
            whereParam = new { MachineId = machineId };

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            //URL直接起動時、参照データの権限チェック
            if (!CheckAccessUserBelong(results[0].LocationStructureId, results[0].JobStructureId))
            {
                return false;
            }

            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.SearchList, this.pageInfoList);
            Dao.searchResult conditionObj = new Dao.searchResult();

            // コントロールIDより画面の項目を取得
            Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.SearchList);
            // 検索条件データの設定
            if (condition != null)
            {
                if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, TargetCtrlId.SearchList, conditionObj, pageInfo))
                {
                    // エラーの場合終了
                    return false;
                }
                // タブNO(他機能からの遷移の際に初期選択タブを保持)
                results[0].TabNo = conditionObj.TabNo;
            }
            // 点検種別毎管理を退避
            flgMaintainanceKindManage = results[0].MaintainanceKindManage;

            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, typeLst, this.db, this.LanguageId, true);

            // ページ情報取得
            List<CommonWebTemplate.CommonDefinitions.PageInfo> pageInfos = new List<CommonWebTemplate.CommonDefinitions.PageInfo>();
            // 編集画面or参照画面で取得結果セット先を変更
            if (mode == DetailFormType.Detail)
            {
                pageInfos.Add(GetPageInfo(TargetCtrlId.Detail00, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.Detail10, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.Detail20, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.Detail30, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.Detail40, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.Detail50, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.Detail60, this.pageInfoList));
                // 非表示項目
                // 変更管理ボタンの表示制御用フラグ
                setHistoryManagementFlg(TargetCtrlId.Detail07, (int)results[0].FactoryId);

            }
            else
            {
                pageInfos.Add(GetPageInfo(TargetCtrlId.EditDetail00, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.EditDetail10, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.EditDetail20, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.EditDetail30, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.EditDetail40, this.pageInfoList));
                pageInfos.Add(GetPageInfo(TargetCtrlId.EditDetail50, this.pageInfoList));
                // 複写時はキー項目を-1に更新
                if (mode == DetailFormType.Copy)
                {
                    results[0].MachineId = -1;
                    results[0].EquipmentId = -1;
                    results[0].DateOfInstallation = DateTime.Now;
                    results[0].DateOfManufacture = DateTime.Now;
                }
            }

            // 一覧数分
            foreach (CommonWebTemplate.CommonDefinitions.PageInfo info in pageInfos)
            {
                // 検索結果の設定
                if (!SetSearchResultsByDataClass<Dao.searchResult>(info, results, 1))
                {
                    // 異常終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }

            }

            factoryId = results[0].FactoryId.Value;

            return true;
        }

        /// <summary>
        /// 画面初期化(詳細編集画面)
        /// </summary>
        private bool initEdit()
        {
            this.ResultList = new();
            bool result = true;

            // それぞれの処理で情報をセットする
            var resultList = new List<Dictionary<string, object>>();
            // クリックされたボタンを判定
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsNew())
            {
                // 新規登録パターン
                result = setInsertEditData();
            }
            else if (compareId.IsUpdate())
            {
                // 編集パターン
                result = setUpdateEditData();
            }
            else if (compareId.IsCopy())
            {
                // 複写パターン
                result = setCopyEditData();
            }

            return result;
        }

        /// <summary>
        /// 新規登録時初期化処理
        /// </summary>
        /// <returns>実行成否：正常ならTrue、異常ならFlase</returns>
        private bool setInsertEditData()
        {
            //// 初期値セットデータ
            //IList<Dao.searchResult> results = new List<Dao.searchResult>();
            //Dao.searchResult result = new Dao.searchResult();

            //// 初期値セット
            //result.DateOfInstallation = DateTime.Now;
            //result.DateOfManufacture = DateTime.Now;
            //result.MachineId = -1;
            //// 場所分類＆職種機種＆詳細検索条件取得
            //int? loc = getTreeValue(true);
            //if (loc != null)
            //{
            //    result.LocationStructureId = (int)loc;
            //}
            //int? job = getTreeValue(false);
            //if (job != null)
            //{
            //    result.JobStructureId = (int)job;
            //}
            //results.Add(result);

            //string[] listNames = new string[] { TargetCtrlId.EditDetail00, TargetCtrlId.EditDetail10, TargetCtrlId.EditDetail20, TargetCtrlId.EditDetail30, TargetCtrlId.EditDetail40, TargetCtrlId.EditDetail50 };

            //foreach (string listName in listNames)
            //{
            //    // ページ情報取得
            //    var pageInfo = GetPageInfo(listName, this.pageInfoList);
            //    // データセット
            //    if (!SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results, 1))
            //    {
            //        // 異常終了
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        return false;
            //    }
            //}

            List<string> toCtrlIdList = new List<string>();
            toCtrlIdList.Add(TargetCtrlId.EditDetail00);
            toCtrlIdList.Add(TargetCtrlId.EditDetail10);
            toCtrlIdList.Add(TargetCtrlId.EditDetail20);
            toCtrlIdList.Add(TargetCtrlId.EditDetail30);
            toCtrlIdList.Add(TargetCtrlId.EditDetail40);
            toCtrlIdList.Add(TargetCtrlId.EditDetail50);

            // 新規の場合
            Dao.searchResult param = new();
            int? loc = getTreeValue(true);
            if (loc != null)
            {
                param.LocationStructureId = (int)loc;
            }
            int? job = getTreeValue(false);
            if (job != null)
            {
                param.JobStructureId = (int)job;
            }
            param.DateOfInstallation = DateTime.Now;
            param.DateOfManufacture = DateTime.Now;
            param.MachineId = -1;

            // 取得した結果に対して、地区と職種の情報を設定する
            IList<Dao.searchResult> paramList = new List<Dao.searchResult> { param };
            if (loc != null || job != null)
            {
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref paramList, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);
            }
            initFormByParam(param, toCtrlIdList);

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;

            // ツリーの階層IDの値が単一の場合その値を返す処理
            int? getTreeValue(bool isLocation)
            {
                var list = isLocation ? GetLocationTreeValues() : GetJobTreeValues();
                if (list != null && list.Count == 1)
                {
                    // 値が単一でもその下に紐づく階層が複数ある場合は初期表示しないので判定
                    bool result = TMQUtil.GetButtomValueFromTree(list[0], this.db, this.LanguageId, out int buttomId);
                    return result ? buttomId : null;
                }
                return null;
            }
            /// <summary>
            /// 指定された一覧に値を設定する
            /// </summary>
            /// <param name="param">長期計画の内容</param>
            /// <param name="toCtrlIds">値を設定する一覧のコントロールID</param>
            /// <returns>エラーの場合False</returns>
            bool initFormByParam(Dao.searchResult param, List<string> toCtrlIds)
            {
                // 一覧に対して繰り返し値を設定する
                foreach (var ctrlId in toCtrlIds)
                {
                    if (!setFormByDataClass(ctrlId, new List<Dao.searchResult> { param }))
                    {
                        // エラーの場合
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return false;
                    }
                }

                this.Status = CommonProcReturn.ProcStatus.Valid;
                return true;

                /// <summary>
                /// 指定した画面項目に値を設定する処理
                /// </summary>
                /// <typeparam name="T">セットする値の型</typeparam>
                /// <param name="ctrlId">値をセットする一覧のコントロールID</param>
                /// <param name="results">セットする値リスト</param>
                /// <returns>エラーの場合False</returns>
                bool setFormByDataClass<T>(string ctrlId, IList<T> results)
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
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                    return retVal;
                }
            }

        }

        /// <summary>
        /// 修正時初期化処理
        /// </summary>
        /// <returns>実行成否：正常ならTrue、異常ならFlase</returns>
        private bool setUpdateEditData(long paraMachineId = -1)
        {
            // 機番情報取得
            long machineId = -1;
            if (paraMachineId == -1)
            {
                machineId = getMachineId(true);
            }
            else
            {
                machineId = paraMachineId;
            }
            // 機番・機器情報取得
            searchKiban(machineId, DetailFormType.Edit, out int factoryId);
            // ★画面定義の翻訳情報取得★
            GetContorlDefineTransData(factoryId);
            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;
        }

        /// <summary>
        /// 複写時初期化処理
        /// </summary>
        /// <returns>実行成否：正常ならTrue、異常ならFlase</returns>
        private bool setCopyEditData()
        {
            // 機番情報取得
            long machineId = getMachineId(true);
            // 機番・機器情報取得
            searchKiban(machineId, DetailFormType.Copy, out int factoryId);
            // ★画面定義の翻訳情報取得★
            GetContorlDefineTransData(factoryId);
            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;
        }

        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdit()
        {
            // 画面情報取得
            DateTime now = DateTime.Now;
            // 登録内容取得
            List<short> grpNoList = new List<short>() { EditFormTargetGrpNo.MachineInfo, EditFormTargetGrpNo.EquipInfo };
            // 機番情報
            ComDao.McMachineEntity registMachineInfo = getRegistInfo<ComDao.McMachineEntity>(grpNoList, now);
            //最下層の構成IDを取得して機能場所階層ID、職種機種階層IDにセットする
            IList<Dao.searchResult> results = new List<Dao.searchResult>();
            Dao.searchResult result = getRegistInfo<Dao.searchResult>(grpNoList, now);
            results.Add(result);
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.searchResult>(ref results, new List<TMQUtil.StructureLayerInfo.StructureType> { TMQUtil.StructureLayerInfo.StructureType.Location, TMQUtil.StructureLayerInfo.StructureType.Job });
            // 職種と地区の値を登録するデータクラスに設定
            setLayerInfo(ref registMachineInfo, result);
            registMachineInfo.ConservationStructureId = result.InspectionSiteConservationStructureId;

            // 機器情報
            ComDao.McEquipmentEntity registEquipInfo = getRegistInfo<ComDao.McEquipmentEntity>(grpNoList, now);

            // 排他チェック
            // 修正時であればチェック
            if (registMachineInfo.MachineId != -1)
            {
                // 機番情報排他チェック
                if (isErrorExclusive(EditFormTargetGrpNo.MachineInfo))
                {
                    return false;
                }
                // 機器情報排他チェック
                if (isErrorExclusive(EditFormTargetGrpNo.EquipInfo))
                {
                    return false;
                }
                // 機器仕様排他チェック
                //if (isErrorExclusive(EditFormTargetGrpNo.SpecInfo))
                //{
                //    return false;
                //}
            }

            // 入力チェック
            if (isErrorMachineRegist(registMachineInfo))
            {
                return false;
            }

            if (registMachineInfo.MachineId == -1)
            {
                // 新規登録 or 複写
                // 機番情報
                (bool returnFlag, long id) machineResult = registInsertDb<ComDao.McMachineEntity>(registMachineInfo, SqlName.InsertMachineInfo);
                if (!machineResult.returnFlag)
                {
                    return false;
                }
                // 機器情報
                // 登録した機番IDをセット
                registEquipInfo.MachineId = machineResult.id;
                (bool returnFlag, long id) equipResult = registInsertDb<ComDao.McEquipmentEntity>(registEquipInfo, SqlName.InsertEquipmentInfo);
                if (!equipResult.returnFlag)
                {
                    return false;
                }
                // 適用法規
                if (!registApplicable(machineResult.id, now, false, false))
                {
                    return false;
                }
                // 構成
                if (!insertParent(machineResult.id, now, registMachineInfo.EquipmentLevelStructureId))
                {
                    return false;
                }

                // 更新後初期表示処理のためデータセット
                setUpdateEditData(machineResult.id);
            }
            else
            {
                // 更新
                // 更新前情報を取得
                var preUpdInfo = new ComDao.McMachineEntity().GetEntity(registMachineInfo.MachineId, this.db);

                // 機番情報
                if (!registUpdateDb<ComDao.McMachineEntity>(registMachineInfo, SqlName.UpdateMachineInfo))
                {
                    return false;
                }
                // 機器情報
                if (!registUpdateDb<ComDao.McEquipmentEntity>(registEquipInfo, SqlName.UpdateEquipmentInfo))
                {
                    return false;
                }
                // 適用法規
                if (!registApplicable(registMachineInfo.MachineId, now, true, false))
                {
                    return false;
                }
                // 構成
                // 機器レベルに変更があった場合のみ登録
                if (registMachineInfo.EquipmentLevelStructureId != preUpdInfo.EquipmentLevelStructureId)
                {
                    if (!deleteComposition(registMachineInfo.MachineId))
                    {
                        return false;
                    }
                    if (!insertParent(registMachineInfo.MachineId, now, registMachineInfo.EquipmentLevelStructureId))
                    {
                        return false;
                    }
                }

                // 更新後初期表示処理のためデータセット
                setUpdateEditData(registMachineInfo.MachineId);
            }

            return true;

            // 画面のツリーの階層情報を登録用データクラスにセット
            void setLayerInfo(ref ComDao.McMachineEntity target, Dao.searchResult source)
            {
                // 場所階層
                target.LocationStructureId = source.LocationStructureId;
                // 各階層のIDは名称のプロパティに文字列として格納される（ツリーの定義の関係）ため、数値に変換
                target.LocationDistrictStructureId = ComUtil.ConvertStringToInt(source.DistrictName);
                target.LocationFactoryStructureId = ComUtil.ConvertStringToInt(source.FactoryName);
                target.LocationPlantStructureId = ComUtil.ConvertStringToInt(source.PlantName);
                target.LocationSeriesStructureId = ComUtil.ConvertStringToInt(source.SeriesName);
                target.LocationStrokeStructureId = ComUtil.ConvertStringToInt(source.StrokeName);
                target.LocationFacilityStructureId = ComUtil.ConvertStringToInt(source.FacilityName);
                // 職種機種階層
                target.JobStructureId = source.JobStructureId;
                // 各階層のIDは名称のプロパティに文字列として格納される（ツリーの定義の関係）ため、数値に変換
                target.JobKindStructureId = ComUtil.ConvertStringToInt(source.JobName);
                target.JobLargeClassficationStructureId = ComUtil.ConvertStringToInt(source.LargeClassficationName);
                target.JobMiddleClassficationStructureId = ComUtil.ConvertStringToInt(source.MiddleClassficationName);
                target.JobSmallClassficationStructureId = ComUtil.ConvertStringToInt(source.SmallClassficationName);
            }
        }

        /// <summary>
        /// 参照画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistDetail()
        {
            DateTime now = DateTime.Now; // システム日付

            // ResulrInfoDictionaryを登録対象のコントロールIDで絞り込み
            foreach (string ctrlId in registTargetCtrlIdDetail)
            {
                // 画面情報を取得
                Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                if (result == null)
                {
                    continue;
                }

                // コントロールIDに応じて処理を分岐
                switch (ctrlId)
                {
                    case TargetCtrlIdManagementStandard.DetailManagementStandard90: // 保全項目一覧
                        return registManagementStandard(result);
                        break;
                    case TargetCtrlIdManagementStandard.DetailForma1List120: // 様式１一覧
                        return registFormat1List();
                        break;
                    case TargetCtrlIdManagementStandard.DetailScheduleList160: // スケジューリング一覧
                        return registScheduleList();
                        break;
                    case TargetCtrlIdManagementStandard.DetailLankScheduleList170: // 点検種別毎スケジューリング一覧
                        return registLankScheduleList();
                        break;
                    case TargetCtrlIdUseParts.DetailUseParts: // 使用部品 使用部品一覧
                        return registUseParts(result);
                        break;
                    case TargetCtrlIdMpInfo.MpInfoList: // MP情報 MP情報一覧
                        return registMpInfo(result);
                        break;
                    default:
                        break;
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
        private T getRegistInfo<T>(List<short> groupNoList, DateTime now)
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
                List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

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
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <returns>登録内容のデータクラス</returns>
        private string getValue(string ctrlId, string keyName)
        {
            // 指定コントロールIDより特定の値を取得
            Dictionary<string, object> targetDictionary;
            targetDictionary = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
            return getDictionaryKeyValue(targetDictionary, keyName);
        }

        /// <summary>
        /// 登録データの取得
        /// </summary>
        /// <param name="grpNo">グループ番号</param>
        /// <returns>実行成否：正常ならTrue、異常ならFalse</returns>
        //private bool getRegistEntity(short grpNo, DateTime date, ref ComDao.CommonTableItem entity)
        //{
        //    // 登録対象の画面項目定義の情報
        //    var mappingInfo = getResultMappingInfoByGrpNo(grpNo);

        //    // 対象コントロールIDの結果情報のみ抽出
        //    var ctrlIdList = mappingInfo.CtrlIdList;
        //    var conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

        //    if (this.TransActionDiv == LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Edit)
        //    {
        //        // 修正ボタンの場合
        //        // 排他ロック用マッピング情報取得
        //        var lockValMaps = GetLockValMapsByGrpNo(grpNo);
        //        var lockKeyMaps = GetLockKeyMapsByGrpNo(grpNo);
        //        foreach (var condition in conditionList)
        //        {
        //            // 排他チェック
        //            if (!CheckExclusiveStatus(condition, lockValMaps, lockKeyMaps)) { return false; }
        //        }
        //    }

        //    foreach(var ctrlId in ctrlIdList)
        //    {
        //        var conditionListByCtrlId = ComUtil.GetDictionaryListByCtrlId(conditionList, ctrlId);
        //        var mappingList = mappingInfo.Value.Where(x => x.CtrlId == ctrlId).ToList();
        //        // 登録データの設定
        //        foreach (var condition in conditionListByCtrlId)
        //        {
        //            if (!SetExecuteConditionByDataClass(condition, mappingList, entity, date, Convert.ToInt32(this.UserId)))
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        /// <summary>
        /// 縦型一覧用排他チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive(short grpNo)
        {
            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMapsByGrpNo(grpNo);
            var lockKeyMaps = GetLockKeyMapsByGrpNo(grpNo);
            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfoByGrpNo(grpNo);

            // 対象コントロールIDの結果情報のみ抽出
            var ctrlIdList = mappingInfo.CtrlIdList;
            var conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);
            foreach (var condition in conditionList)
            {
                // 排他チェック
                if (!CheckExclusiveStatus(condition, lockValMaps, lockKeyMaps))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 一覧用排他チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusiveList(string ctrlId)
        {
            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(ctrlId);
            var lockKeyMaps = GetLockKeyMaps(ctrlId);
            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfoDirect(ctrlId);

            // 対象コントロールIDの結果情報のみ抽出
            var ctrlIdList = mappingInfo.CtrlIdList;
            var conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);
            foreach (var condition in conditionList)
            {
                // 排他チェック
                if (!CheckExclusiveStatus(condition, lockValMaps, lockKeyMaps))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 機番情報　入力チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorMachineRegist(ComDao.McMachineEntity res)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 明細の場合のイメージ
            //if (isErrorRegistForList(ref errorInfoDictionary))
            //{
            //    // エラー情報を画面に反映
            //    SetJsonResult(errorInfoDictionary);
            //    return true;
            //}

            // 単一項目チェック
            if (isErrorRegistForSingle(ref errorInfoDictionary, res))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            bool isErrorRegistForSingle(ref List<Dictionary<string, object>> errorInfoDictionary, ComDao.McMachineEntity result)
            {
                bool isError = false;   // 処理全体でエラーの有無を保持
                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(TargetCtrlId.EditDetail10);
                // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                // 単一の内容を取得
                Dictionary<string, object> targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlId.EditDetail10);

                // 修正時のみチェック
                if (result.MachineId != -1)
                {
                    // 変更前の機器レベルを取得
                    string whereClause = string.Empty; // WHERE句文字列
                    dynamic whereParam = null; // WHERE句パラメータ
                    string sql;
                    if (!TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql))
                    {
                        return false;
                    }
                    whereParam = new { MachineId = result.MachineId };
                    sql = sql + whereClause;
                    // 一覧検索実行
                    IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
                    if (results == null || results.Count == 0)
                    {
                        return false;
                    }

                    // 機器レベルが変更されているか
                    if (result.EquipmentLevelStructureId != results[0].EquipmentLevelStructureId)
                    {
                        // 機器構成が存在する場合エラー
                        if (!checkComposition(result.MachineId, true))
                        {
                            // エラー情報格納クラス
                            ErrorInfo errorInfo = new ErrorInfo(targetDic);
                            isError = true;
                            // 構成機器が登録されている機器の為、機器レベルを変更できません。
                            string errMsg = GetResMessage("141100001");
                            string val = info.getValName("equipment_level_structure_id"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                            errorInfo.setError(errMsg, val);
                            errorInfoDictionary.Add(errorInfo.Result);
                        }
                    }
                }

                return isError;
            }

        }

        /// <summary>
        /// 新規登録処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <param name="sqlName">SQLファイル名</param>
        /// <returns>returnFlag:エラーの場合False、id:登録データのID</returns>
        private (bool returnFlag, long id) registInsertDb<T>(T registInfo, string sqlName)
        {
            string sql;
            // SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out sql))
            {
                return (false, -1);
            }

            long returnId = db.RegistAndGetKeyValue<long>(sql, out bool isError, registInfo);
            return (!isError, returnId);
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <param name="sqlName">SQLファイル名</param>
        /// <returns>returnFlag:エラーの場合False、id:登録データのID</returns>
        private bool registUpdateDb<T>(T registInfo, string sqlName)
        {
            string sql;
            // SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out sql))
            {
                return false;
            }

            int result = db.Regist(sql, registInfo);
            return result > 0;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <param name="sqlName">SQLファイル名</param>
        /// <returns>returnFlag:エラーの場合False、id:登録データのID</returns>
        private bool registDeleteDb<T>(T registInfo, string sqlName)
        {
            string sql;
            // SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out sql))
            {
                return false;
            }

            int result = db.Regist(sql, registInfo);
            return true;
        }

        /// <summary>
        /// 機器情報削除処理
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteMachineDetail()
        {
            // 機番情報排他チェック
            if (isErrorExclusive(DetailFormTargetGrpNo.MachineInfo))
            {
                return false;
            }
            // 機器情報排他チェック
            if (isErrorExclusive(DetailFormTargetGrpNo.EquipInfo))
            {
                return false;
            }
            //// 機器別管理基準_保全項目一覧排他チェック
            //if (isErrorExclusiveList("ctrlId"))
            //{
            //    return false;
            //}
            //// 使用部品排他チェック
            //if (isErrorExclusiveList())
            //{
            //    return false;
            //}
            //// 親子構成排他チェック
            //if (isErrorExclusiveList())
            //{
            //    return false;
            //}
            //// ループ構成排他チェック
            //if (isErrorExclusiveList())
            //{
            //    return false;
            //}
            //// 付属品構成排他チェック
            //if (isErrorExclusiveList())
            //{
            //    return false;
            //}
            //// MP情報排他チェック
            //if (isErrorExclusiveList())
            //{
            //    return false;
            //}

            // 機番ID・機器IDを取得
            Dictionary<string, object> targetDictionary = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlId.Detail20);
            string strMc = getDictionaryKeyValue(targetDictionary, "machine_id");
            long machineId = long.Parse(strMc);
            targetDictionary = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlId.Detail50);
            string strEq = getDictionaryKeyValue(targetDictionary, "equipment_id");
            long equipId = long.Parse(strEq);

            // 長期計画存在チェック
            if (!checkLongPlan(machineId))
            {
                return false;
            }

            // 保全活動存在チェック
            if (!checkMsSummary(machineId))
            {
                return false;
            }

            // 構成存在チェック
            if (!checkComposition(machineId))
            {
                return false;
            }

            // 削除処理
            // 機番情報削除
            if (!deleteMachineInfo(machineId))
            {
                return false;
            }

            // 機器情報削除
            if (!deleteEquipmentInfo(equipId))
            {
                return false;
            }

            // 仕様情報削除
            if (!deleteEquipmentSpecInfo(equipId))
            {
                return false;
            }

            // 適用法規削除
            if (!deleteApplicableLaws(machineId))
            {
                return false;
            }

            // 機器別管理基準削除
            if (!deleteManagementStandards(machineId))
            {
                return false;
            }

            // 機番使用部品情報削除
            if (!deleteUseParts(machineId))
            {
                return false;
            }

            // 構成機器情報削除
            if (!deleteComposition(machineId))
            {
                return false;
            }

            // MP情報削除
            if (!deleteMpInfo(machineId))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 長期計画存在チェック
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>存在なし:True 存在なし:False</returns>
        private bool checkLongPlan(long machineId)
        {
            dynamic whereParam = null; // WHERE句パラメータ
            string sql = string.Empty;
            // 検索SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetChkLongPlan, out sql))
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            whereParam = new { MachineId = machineId };
            // 総件数を取得
            int cnt = db.GetCount(sql, whereParam);
            if (cnt > 0)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「 長期計画で使用される機器の為、削除できません。」
                this.MsgId = GetResMessage("141170001");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保全活動存在チェック
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>存在なし:True 存在なし:False</returns>
        private bool checkMsSummary(long machineId)
        {
            dynamic whereParam = null; // WHERE句パラメータ

            string sql = string.Empty;
            // 検索SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetChkMsSummary, out sql))
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            whereParam = new { MachineId = machineId };
            // 総件数を取得
            int cnt = db.GetCount(sql, whereParam);
            if (cnt > 0)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「 保全活動で使用される機器の為、削除できません。」
                this.MsgId = GetResMessage("141300003");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 構成存在チェック
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <param name="updCheck">更新時チェックかどうか</param>
        /// <returns>存在なし:True 存在あり:False</returns>
        private bool checkComposition(long machineId, bool updCheck = false)
        {
            // 検索SQL文の取得
            // mc_machine_parent_info
            dynamic whereParam = null; // WHERE句パラメータ
            string sql = string.Empty;
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetChkParentInfo, out sql))
            {
                // エラー終了
                if (!updCheck)
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                }
                return false;
            }
            whereParam = new { MachineId = machineId };
            // 総件数を取得
            int cnt = db.GetCount(sql, whereParam);
            if (cnt > 0)
            {
                // エラー終了
                if (!updCheck)
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「 構成機器が登録されている機器の為、削除できません。」
                    this.MsgId = GetResMessage("141070001");
                }
                return false;
            }

            // mc_loop_info
            whereParam = null; // WHERE句パラメータ
            sql = string.Empty;
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetChkLoopInfo, out sql))
            {
                // エラー終了
                if (!updCheck)
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                }
                return false;
            }
            whereParam = new { MachineId = machineId };
            // 総件数を取得
            cnt = db.GetCount(sql, whereParam);
            if (cnt > 0)
            {
                // エラー終了
                if (!updCheck)
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「 構成機器が登録されている機器の為、削除できません。」
                    this.MsgId = GetResMessage("141070001");
                }
                return false;
            }

            //// mc_accessory_info
            //whereParam = null; // WHERE句パラメータ
            //sql = string.Empty;
            //if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetChkAccessoryInfo, out sql))
            //{
            //    // エラー終了
            //    if (!updCheck)
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //    }
            //    return false;
            //}
            //whereParam = new { MachineId = machineId };
            //// 総件数を取得
            //cnt = db.GetCount(sql, whereParam);
            //if (cnt > 0)
            //{
            //    // エラー終了
            //    if (!updCheck)
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「 構成機器が登録されている機器の為、削除できません。」
            //        this.MsgId = GetResMessage("141070001");
            //    }
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 機番情報削除
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteMachineInfo(long machineId)
        {

            Dao.searchResult result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteMachineInfo))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 添付情報削除
            result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteMachineAttachment))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 機器情報削除
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteEquipmentInfo(long equipmentId)
        {
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.DeleteEquipmentInfo, out string sql))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            // 機器IDを隠しで保持しているコントロール
            string listCtrlId = TargetCtrlId.Detail50;
            if (!deleteTargetCtrl<ComDao.McEquipmentEntity>(listCtrlId, sql))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 機器添付削除
            Dao.searchResult result = new Dao.searchResult();
            result.EquipmentId = equipmentId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteEquipmentAttachment))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 機器情報削除(ExcelPort)
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteExcelPortEquipmentInfo(long equipmentId)
        {
            ComDao.McEquipmentEntity result = new ComDao.McEquipmentEntity();
            result.EquipmentId = equipmentId;
            if (!registDeleteDb<ComDao.McEquipmentEntity>(result, SqlName.DeleteEquipmentInfo))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 機器添付削除
            Dao.searchResult result1 = new Dao.searchResult();
            result.EquipmentId = equipmentId;
            if (!registDeleteDb<Dao.searchResult>(result1, SqlName.DeleteEquipmentAttachment))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 機器仕様情報削除
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteEquipmentSpecInfo(long equipmentId)
        {
            Dao.searchResult result = new Dao.searchResult();
            result.EquipmentId = equipmentId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteEquipmentSpecInfo))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 適用法規削除
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteApplicableLaws(long machineId)
        {
            // 機番IDを隠しで保持しているコントロール
            Dao.searchResult result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteApplicableLawsInfo))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 機器別管理基準削除
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteManagementStandards(long machineId)
        {
            // 保全項目添付情報削除
            Dao.searchResult result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteManagementStandardAttachment))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 保全スケジュール詳細削除
            result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteMaintainanceScheduleDetail))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 保全スケジュール削除
            result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteMaintainanceSchedule))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 機器別管理基準内容削除
            result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteManagementStandardsContent))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 機器別管理基準部位削除
            result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteManagementStandardsComponent))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 機番使用部品情報削除
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteUseParts(long machineId)
        {
            Dao.searchResult result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteMachineUseParts))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 構成削除
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteComposition(long machineId)
        {
            // 機器親子構成削除
            Dao.searchResult result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteParentInfo))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // ループ構成削除
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteLoopInfo))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            //// 付属品構成削除
            //if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteAccessoryInfo))
            //{
            //    // エラーの場合
            //    this.Status = CommonProcReturn.ProcStatus.Error;
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// MP情報削除
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteMpInfo(long machineId)
        {
            // MP情報添付情報削除
            Dao.searchResult result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteMpInfoAttachment))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            result = new Dao.searchResult();
            result.MachineId = machineId;
            if (!registDeleteDb<Dao.searchResult>(result, SqlName.DeleteMpInfo))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 適用法規登録処理
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <param name="dt">登録日時</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool registApplicable(long machineId, DateTime dt, bool updateFlg, bool epFlg, string epValue = null)
        {
            if (updateFlg)
            {
                // 機番IDをキーに一度削除
                ComDao.McApplicableLawsEntity delApplicableInfo = new ComDao.McApplicableLawsEntity();
                delApplicableInfo.MachineId = machineId;
                if (!registDeleteDb<ComDao.McApplicableLawsEntity>(delApplicableInfo, SqlName.DeleteApplicableLawsInfo))
                {
                    return false;
                }
            }

            // 複数選択データを分割し1件ずつINSERT
            string applicableItems = null;
            // ExcelPort用制御
            if (!epFlg)
            {
                applicableItems = getValue(TargetCtrlId.EditDetail20, "applicable_laws_structure_id");
            }
            else
            {
                if (epValue == null || epValue.Length == 0)
                {
                    return true;
                }
                applicableItems = epValue;
            }

            string[] appItem = applicableItems.Split('|');
            foreach (string val in appItem)
            {
                if (val.Length == 0)
                {
                    continue;
                }
                ComDao.McApplicableLawsEntity registApplicableInfo = new ComDao.McApplicableLawsEntity();
                registApplicableInfo.ApplicableLawsStructureId = int.Parse(val);
                registApplicableInfo.MachineId = machineId;
                registApplicableInfo.InsertDatetime = dt;
                registApplicableInfo.UpdateDatetime = dt;
                registApplicableInfo.InsertUserId = int.Parse(this.UserId);
                registApplicableInfo.UpdateUserId = int.Parse(this.UserId);
                (bool returnFlag, long id) appResult = registInsertDb<ComDao.McApplicableLawsEntity>(registApplicableInfo, SqlName.InsertApplicableLawsInfo);
                if (!appResult.returnFlag)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 構成新規登録処理
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <param name="dt">登録日時</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool insertParent(long machineId, DateTime dt, int? levelId)
        {
            // 親子構成
            ComDao.McMachineParentInfoEntity registParentInfo = new ComDao.McMachineParentInfoEntity();
            registParentInfo.MachineId = machineId;
            registParentInfo.InsertDatetime = dt;
            registParentInfo.UpdateDatetime = dt;
            registParentInfo.InsertUserId = int.Parse(this.UserId);
            registParentInfo.UpdateUserId = int.Parse(this.UserId);
            if (!registInsertDb<ComDao.McMachineParentInfoEntity>(registParentInfo, SqlName.InsertParentInfo).returnFlag)
            {
                return false;
            }

            // 機器レベルがループなら登録とする
            // 一覧検索SQL文の取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetMachineLevel, out string sql);
            dynamic whereParam = null; // WHERE句パラメータ
            whereParam = new { MachineLevel = levelId, LanguageId = this.LanguageId };
            // 機器レベルの拡張項目取得
            IList<Dao.ExtensionVal> results = db.GetListByDataClass<Dao.ExtensionVal>(sql, whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }
            // ループ品なら登録
            if (results[0].ExtensionData == "4")
            {
                // ループ構成
                ComDao.McLoopInfoEntity registLoopInfo = new ComDao.McLoopInfoEntity();
                registLoopInfo.MachineId = machineId;
                registLoopInfo.InsertDatetime = dt;
                registLoopInfo.UpdateDatetime = dt;
                registLoopInfo.InsertUserId = int.Parse(this.UserId);
                registLoopInfo.UpdateUserId = int.Parse(this.UserId);
                if (!registInsertDb<ComDao.McLoopInfoEntity>(registLoopInfo, SqlName.InsertLoopInfo).returnFlag)
                {
                    return false;
                }
            }
            //// 付属品構成
            //ComDao.McAccessoryInfoEntity registAcceInfo = new ComDao.McAccessoryInfoEntity();
            //registAcceInfo.MachineId = machineId;
            //registAcceInfo.InsertDatetime = dt;
            //registAcceInfo.UpdateDatetime = dt;
            //registAcceInfo.InsertUserId = int.Parse(this.UserId);
            //registAcceInfo.UpdateUserId = int.Parse(this.UserId);
            //if (!registInsertDb<ComDao.McAccessoryInfoEntity>(registAcceInfo, SqlName.InsertAccessoryInfo).returnFlag)
            //{
            //    return false;
            //}
            return true;
        }

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

        /// <summary>
        /// 変更管理ボタンの表示制御用のフラグを設定
        /// </summary>
        /// <param name="listCtrlId">設定する非表示項目のID</param>
        /// <param name="factoryId">省略可能　変更管理を行うか判定する工場ID</param>
        private void setHistoryManagementFlg(string listCtrlId, int factoryId = -1)
        {
            // 変更管理フラグを取得
            TMQUtil.HistoryManagement history = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
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

        /// <summary>
        /// ExcelPort機器台帳チェック処理
        /// </summary>
        /// <returns>true:正常終了</returns>
        private bool checkExcelPortRegistDetail(ref List<BusinessLogicDataClass_MC0001.excelPortMachineList> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            // 入力チェック
            bool errFlg = false;
            for (int i = 0; i < resultList.Count; i++)
            {
                // 送信時処理IDが設定されているもののみ
                if (resultList[i].ProcessId != null)
                {
                    // 場所階層セット
                    resultList[i].LocationStructureId = (int)resultList[i].DistrictId;
                    resultList[i].LocationDistrictStructureId = (int)resultList[i].DistrictId;
                    resultList[i].LocationStructureId = (int)resultList[i].FactoryId;
                    resultList[i].LocationFactoryStructureId = (int)resultList[i].FactoryId;
                    if (resultList[i].PlantId != null)
                    {
                        resultList[i].LocationStructureId = (int)resultList[i].PlantId;
                        resultList[i].LocationPlantStructureId = (int)resultList[i].PlantId;
                    }
                    if (resultList[i].SeriesId != null)
                    {
                        resultList[i].LocationStructureId = (int)resultList[i].SeriesId;
                        resultList[i].LocationSeriesStructureId = (int)resultList[i].SeriesId;
                    }
                    if (resultList[i].StrokeId != null)
                    {
                        resultList[i].LocationStructureId = (int)resultList[i].StrokeId;
                        resultList[i].LocationStrokeStructureId = (int)resultList[i].StrokeId;
                    }
                    if (resultList[i].FacilityId != null)
                    {
                        resultList[i].LocationStructureId = (int)resultList[i].FacilityId;
                        resultList[i].LocationFacilityStructureId = (int)resultList[i].FacilityId;
                    }

                    // 職種階層セット
                    resultList[i].JobStructureId = (int)resultList[i].JobId;
                    resultList[i].JobKindStructureId = (int)resultList[i].JobId;
                    if (resultList[i].LargeClassficationId != null)
                    {
                        resultList[i].JobStructureId = (int)resultList[i].LargeClassficationId;
                        resultList[i].JobLargeClassficationStructureId = (int)resultList[i].LargeClassficationId;
                    }
                    if (resultList[i].MiddleClassficationId != null)
                    {
                        resultList[i].JobStructureId = (int)resultList[i].MiddleClassficationId;
                        resultList[i].JobMiddleClassficationStructureId = (int)resultList[i].MiddleClassficationId;
                    }
                    if (resultList[i].SmallClassficationId != null)
                    {
                        resultList[i].JobStructureId = (int)resultList[i].SmallClassficationId;
                        resultList[i].JobSmallClassficationStructureId = (int)resultList[i].SmallClassficationId;
                    }

                    // 保全方式
                    resultList[i].ConservationStructureId = resultList[i].InspectionSiteConservationStructureId;

                    // 循環対象
                    if (resultList[i].CirculationTargetFlg == null)
                    {
                        resultList[i].CirculationTargetFlg = 0;
                    }

                    // 点検種別毎管理
                    if (resultList[i].MaintainanceKindManage == null)
                    {
                        resultList[i].MaintainanceKindManage = 0;
                    }
                    //予算管理区分
//                    if (resultList[i].BudgetManagementStructureId != null)
 //                   {
                        resultList[i].BudgetManagementStructureId = null;
                    //                   }
                    //図面保管場所
                    //if (resultList[i].DiagramStorageLocationStructureId != null)
                    //{
                    resultList[i].DiagramStorageLocationStructureId = null;//(int)resultList[i].DiagramStorageLocationStructureId;
                    //}


                    // 更新
                    if (resultList[i].ProcessId == 2)
                    {
                        // 変更前の機器レベルを取得
                        if (!checkEpEquipmentLevel(resultList[i]))
                        {
                            // 構成機器が登録されている機器の為、機器レベルを変更できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortMachineListInfo.EquipmentLevelColumnNo, GetResMessage("111070003"), GetResMessage("141100001"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                            errFlg = true;
                        }

                    }
                    // 削除
                    else if (resultList[i].ProcessId == 9)
                    {
                        // 長計存在チェック
                        if (!checkEpLongPlanExsits(resultList[i]))
                        {
                            // 長期計画で使用される機器の為、削除できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortMachineListInfo.ProccesColumnNo, null, GetResMessage("141170001"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                            errFlg = true;
                        }
                        // 保全活動存在チェック
                        else if (!checkEpMaSummaryExsits(resultList[i]))
                        {
                            // 保全活動で使用される機器の為、削除できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortMachineListInfo.ProccesColumnNo, null, GetResMessage("141300003"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                            errFlg = true;
                        }

                        // 構成存在チェック
                        if (!checkComposition((long)resultList[i].MachineId, true))
                        {
                            // 構成機器が登録されている機器の為、削除できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortMachineListInfo.ProccesColumnNo, null, GetResMessage("141070001"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                            errFlg = true;
                        }
                    }
                }
            }

            // 全件問題無ければ登録処理
            if (errFlg)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ExcelPort機器台帳登録処理
        /// </summary>
        /// <returns>true:正常終了</returns>
        private bool executeExcelPortRegistDetail(List<BusinessLogicDataClass_MC0001.excelPortMachineList> resultList)
        {
            // 登録処理
            for (int i = 0; i < resultList.Count; i++)
            {
                // 送信時処理IDが設定されているもののみ
                if (resultList[i].ProcessId != null)
                {
                    DateTime now = DateTime.Now;
                    // 新規登録
                    if (resultList[i].ProcessId == 1)
                    {
                        resultList[i].InsertDatetime = now;
                        resultList[i].InsertUserId = int.Parse(this.UserId);
                        resultList[i].UpdateDatetime = now;
                        resultList[i].UpdateUserId = int.Parse(this.UserId);

                        // 機番情報
                        (bool returnFlag, long id) machineResult = registInsertDb<BusinessLogicDataClass_MC0001.excelPortMachineList>(resultList[i], SqlName.InsertMachineInfo);
                        if (!machineResult.returnFlag)
                        {
                            return false;
                        }
                        // 機器情報
                        // 登録した機番IDをセット
                        resultList[i].MachineId = machineResult.id;
                        (bool returnFlag, long id) equipResult = registInsertDb<BusinessLogicDataClass_MC0001.excelPortMachineList>(resultList[i], SqlName.InsertEquipmentInfo);
                        if (!equipResult.returnFlag)
                        {
                            return false;
                        }
                        // 適用法規
                        if (!registApplicable(machineResult.id, now, false, true, resultList[i].ApplicableLawsStructureId))
                        {
                            return false;
                        }
                        // 構成
                        if (!insertParent(machineResult.id, now, resultList[i].EquipmentLevelStructureId))
                        {
                            return false;
                        }

                    }
                    // 更新
                    else if (resultList[i].ProcessId == 2)
                    {
                        resultList[i].UpdateDatetime = now;
                        resultList[i].UpdateUserId = int.Parse(this.UserId);

                        // 更新前情報を取得
                        long machineId = resultList[i].MachineId ?? -1;
                        var preUpdInfo = new ComDao.McMachineEntity().GetEntity(machineId, this.db);

                        // 機番情報
                        if (!registUpdateDb<BusinessLogicDataClass_MC0001.excelPortMachineList>(resultList[i], SqlName.UpdateMachineInfo))
                        {
                            return false;
                        }
                        // 機器情報
                        if (!registUpdateDb<BusinessLogicDataClass_MC0001.excelPortMachineList>(resultList[i], SqlName.UpdateEquipmentInfo))
                        {
                            return false;
                        }
                        // 適用法規
                        if (!registApplicable((long)resultList[i].MachineId, now, true, true, resultList[i].ApplicableLawsStructureId))
                        {
                            return false;
                        }
                        // 構成
                        // 機器レベルに変更があった場合のみ登録
                        if (resultList[i].EquipmentLevelStructureId != preUpdInfo.EquipmentLevelStructureId)
                        {
                            if (!deleteComposition(machineId))
                            {
                                return false;
                            }
                            if (!insertParent(machineId, now, resultList[i].EquipmentLevelStructureId))
                            {
                                return false;
                            }
                        }
                    }
                    // 削除
                    else if (resultList[i].ProcessId == 9)
                    {
                        // 削除処理
                        // 機番情報削除
                        if (!deleteMachineInfo((long)resultList[i].MachineId))
                        {
                            return false;
                        }

                        // 機器情報削除
                        if (!deleteExcelPortEquipmentInfo((long)resultList[i].EquipmentId))
                        {
                            return false;
                        }

                        // 仕様情報削除
                        if (!deleteEquipmentSpecInfo((long)resultList[i].EquipmentId))
                        {
                            return false;
                        }

                        // 適用法規削除
                        if (!deleteApplicableLaws((long)resultList[i].MachineId))
                        {
                            return false;
                        }

                        // 機器別管理基準削除
                        if (!deleteManagementStandards((long)resultList[i].MachineId))
                        {
                            return false;
                        }

                        // 機番使用部品情報削除
                        if (!deleteUseParts((long)resultList[i].MachineId))
                        {
                            return false;
                        }

                        // 構成機器情報削除
                        if (!deleteComposition((long)resultList[i].MachineId))
                        {
                            return false;
                        }

                        // MP情報削除
                        if (!deleteMpInfo((long)resultList[i].MachineId))
                        {
                            return false;
                        }

                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Ep用機器レベル変更チェック
        /// </summary>
        /// <param name="result">データ</param>
        /// <returns>true:エラーなし</returns>
        private bool checkEpEquipmentLevel(BusinessLogicDataClass_MC0001.excelPortMachineList result)
        {
            string whereClause = string.Empty; // WHERE句文字列
            dynamic whereParam = null; // WHERE句パラメータ
            string sql;
            if (!TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql))
            {
                return false;
            }
            whereParam = new { MachineId = result.MachineId };
            sql = sql + whereClause;
            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 機器レベルが変更されているか
            if (result.EquipmentLevelStructureId != results[0].EquipmentLevelStructureId)
            {
                // 機器構成が存在する場合エラー
                if (!checkComposition((long)result.MachineId, true))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Ep用長計存在チェック
        /// </summary>
        /// <param name="result">データ</param>
        /// <returns>true:エラーなし</returns>
        private bool checkEpLongPlanExsits(BusinessLogicDataClass_MC0001.excelPortMachineList result)
        {
            dynamic whereParam = null; // WHERE句パラメータ
            string sql = string.Empty;
            // 検索SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetChkLongPlan, out sql))
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            whereParam = new { MachineId = result.MachineId };
            // 総件数を取得
            int cnt = db.GetCount(sql, whereParam);
            if (cnt > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ep用保全活動存在チェック
        /// </summary>
        /// <param name="result">データ</param>
        /// <returns>true:エラーなし</returns>
        private bool checkEpMaSummaryExsits(BusinessLogicDataClass_MC0001.excelPortMachineList result)
        {

            dynamic whereParam = null; // WHERE句パラメータ

            string sql = string.Empty;
            // 検索SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetChkMsSummary, out sql))
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            whereParam = new { MachineId = result.MachineId };
            // 総件数を取得
            int cnt = db.GetCount(sql, whereParam);
            if (cnt > 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}