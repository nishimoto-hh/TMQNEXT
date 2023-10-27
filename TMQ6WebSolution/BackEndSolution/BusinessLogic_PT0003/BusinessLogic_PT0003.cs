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
using Dao = BusinessLogic_PT0003.BusinessLogicDataClass_PT0003;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

namespace BusinessLogic_PT0003
{
    /// <summary>
    /// 予備品管理　棚卸
    /// </summary>
    public partial class BusinessLogic_PT0003 : CommonBusinessLogicBase
    {
        #region private変数

        #region 棚卸準備リスト
        /// <summary>
        /// 追加棚卸IDリスト
        /// </summary>
        private List<long> addInventoryIdList;
        #endregion

        #endregion

        #region 定数
        /// <summary>
        /// 処理対象グループ番号
        /// </summary>
        private static class TargetGrpNo
        {
            /// <summary>入庫単価入力画面</summary>
            public const short EnterInput = 0;
            /// <summary>新規登録画面</summary>
            public const short NewRegist = 2;
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL名：棚卸一覧取得</summary>
            public const string GetInventoryList = "GetInventoryList";
            /// <summary>SQL名：棚卸データの件数取得(入力チェック)</summary>
            public const string GetCountCheckInventory = "GetCountCheckInventory";
            /// <summary>SQL名：棚卸データの件数取得(登録前の存在チェック)</summary>
            public const string GetCountRegistInventory = "GetCountRegistInventory";
            /// <summary>SQL名：棚卸データの登録</summary>
            public const string InsertInventory = "InsertInventory";
            /// <summary>SQL名：棚差調整データの削除/summary>
            public const string DeleteInventoryDifference = "DeleteInventoryDifference";
            /// <summary>SQL名：棚卸IDを条件に棚卸データを取得</summary>
            public const string GetExistInventory = "GetExistInventory";
            /// <summary>SQL名：棚卸データの棚卸取込値を更新</summary>
            public const string UpdateTempInventoryQuantity = "UpdateTempInventoryQuantity";
            /// <summary>SQL名：棚卸データの棚卸数を更新</summary>
            public const string UpdateInventoryQuantity = "UpdateInventoryQuantity";
            /// <summary>SQL名：棚卸データの棚卸調整日時を更新</summary>
            public const string UpdateDifferenceDatetime = "UpdateDifferenceDatetime";
            /// <summary>SQL名：棚卸調整データの登録</summary>
            public const string InsertInventoryDifference = "InsertInventoryDifference";
            /// <summary>SQL名：ロット情報、在庫データの取得</summary>
            public const string GetLotStock = "GetLotStock";
            /// <summary>SQL名：棚卸調整データの取得</summary>
            public const string GetInoutQuantity = "GetInoutQuantity";
            /// <summary>SQL名：予備品仕様マスタの取得</summary>
            public const string GetParts = "GetParts";
            /// <summary>SQL名：予備品IDの在庫確定日の取得</summary>
            public const string GetFixedTargetMonth = "GetFixedTargetMonth";
            /// <summary>SQL名：重複データの件数取得</summary>
            public const string GetCountDuplicationData = "GetCountDuplicationData";
            /// <summary>SQL名：新旧区分の初期表示値を取得するSQL/summary>
            public const string GetInitOldNewStructureId = "GetInitOldNewStructureId";
            /// <summary>SQL名：部門の初期表示値を取得するSQL/summary>
            public const string GetInitDepartmentStructureId = "GetInitDepartmentStructureId";
            /// <summary>SQL名：勘定科目の初期表示値を取得するSQL/summary>
            public const string GetInitAccountStructureId = "GetInitAccountStructureId";

            /// <summary>SQL名：入庫一覧を表示するSQL</summary>
            public const string GetEnterList = "GetEnterList";

            /// <summary>SQL名：入庫単位入力画面を表示するSQL</summary>
            public const string GetEnterInput = "GetEnterInput";
            /// <summary>SQL名：入庫単位を更新登録するSQL</summary>
            public const string UpdateInoutQuantity = "UpdateInoutQuantity";

            /// <summary>SQL名：出庫一覧を表示するSQL</summary>
            public const string GetIssueList = "GetIssueList";
            /// <summary>SQL名：出庫一覧(子要素)を表示するSQL</summary>
            public const string GetIssueChildList = "GetIssueChildList";

            /// <summary>SQL名：棚卸データの件数取得(準備リストが作成されているか)</summary>
            public const string SelectInventryCount = "Select_InventryCount";
            /// <summary>SQL名：入出庫履歴一覧(繰越)を表示する</summary>
            public const string GetInoutHistryForwardList = "GetInoutHistryForwardList";
            /// <summary>SQL名：入出庫履歴一覧を表示する</summary>
            public const string GetInoutHistryList = "GetInoutHistryList";
            /// <summary>SQL名：棚卸準備表（CSV）の列タイトルを取得</summary>
            public const string GetCsvColTitleTransLationText = "GetCsvColTitleTransLationText";
            /// <summary>SQL名：棚卸準備表（CSV）の出力データを取得</summary>
            public const string GetCsvData = "GetCsvData";
            /// <summary>SQL名：棚卸データの在庫数合計値を取得</summary>
            public const string GetSumStockQuantity = "GetSumStockQuantity";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Inventory";
            public const string SubDirInventry = @"InventoryControl";
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
                public const short No = 0;
                public static class Condition
                {
                    /// <summary>
                    /// 検索条件
                    /// </summary>
                    public const string SearchCondition = "COND_010_00_LST_0";
                }
                public static class List
                {
                    /// <summary>
                    /// 棚卸一覧
                    /// </summary>
                    public const string InventoryList = "BODY_040_00_LST_0";
                }
                public static class EnterList
                {
                    /// <summary>
                    /// 入庫一覧
                    /// </summary>
                    public const string List = "BODY_080_00_LST_0";
                }
                public static class IssueList
                {
                    /// <summary>
                    /// 出庫一覧
                    /// </summary>
                    public const string List = "BODY_090_00_LST_0";
                    /// <summary>
                    /// 出庫一覧（子要素）
                    /// </summary>
                    public const string ListChild = "BODY_100_00_LST_0";
                }

                public static class Button
                {
                    /// <summary>
                    /// 棚卸準備リストボタン
                    /// </summary>
                    public const string Output = "Output";
                    /// <summary>
                    /// 棚卸準備リスト(CSV)ボタン
                    /// </summary>
                    public const string OutputCsv = "OutputCsv";
                    /// <summary>
                    /// 棚卸準備取消
                    /// </summary>
                    public const string DeletePreparation = "DeletePreparation";
                    /// <summary>
                    /// 一時登録
                    /// </summary>
                    public const string TemporaryRegist = "TemporaryRegist";
                    /// <summary>
                    /// 棚差調整
                    /// </summary>
                    public const string AdjustDifference = "AdjustDifference";
                    /// <summary>
                    /// 棚卸確定
                    /// </summary>
                    public const string ConfirmInventory = "ConfirmInventory";
                    /// <summary>
                    /// 棚卸確定解除
                    /// </summary>
                    public const string CancelConfirmInventory = "CancelConfirmInventory";
                }
            }

            /// <summary>
            /// 受払履歴
            /// </summary>
            public static class FormDetail
            {
                public const short No = 1;
                public static class Inventry
                {
                    /// <summary>
                    /// 予備品情報一覧
                    /// </summary>
                    public const string InventoryInfoList = "BODY_010_00_LST_1";
                }
                public static class Inout
                {
                    /// <summary>
                    /// 入出庫履歴一覧
                    /// </summary>
                    public const string InoutHistryList = "BODY_020_00_LST_1";
                }
            }
            /// <summary>
            /// 入庫単価入力画面
            /// </summary>
            public static class FormEdit
            {
                public const short No = 2;
                /// <summary>
                /// 入庫単価入力画面
                /// </summary>
                public const string EnterInput = "BODY_000_00_LST_2";
            }

            /// <summary>
            /// 新規取込画面
            /// </summary>
            public static class FormUpload
            {
                public const short No = 3;
                /// <summary>
                /// 取込、エラーメッセージ表示一覧
                /// </summary>
                public const string Info = "BODY_000_00_LST_3";
                /// <summary>
                /// 非表示一覧
                /// </summary>
                public const string HiddenInfo = "BODY_010_00_LST_3";
                public static class Button
                {
                    /// <summary>
                    /// 戻るボタン
                    /// </summary>
                    public const string BackUpload = "BackUpload";
                }
            }

            /// <summary>
            /// 新規登録画面
            /// </summary>
            public static class FormRegist
            {
                public const short No = 4;
                /// <summary>
                /// 棚番一覧
                /// </summary>
                public const string PartsLocation = "BODY_000_00_LST_4";
                /// <summary>
                /// 予備品情報一覧
                /// </summary>
                public const string Info = "BODY_010_00_LST_4";
            }
        }

        /// <summary>
        /// 準備状況
        /// </summary>
        private static class ReadyStatus
        {
            /// <summary>拡張データの連番</summary>
            public const short Seq = 1;
            /// <summary>拡張データ：準備リスト作成済み</summary>
            public const string Created = "1";
            /// <summary>拡張データ：未実施</summary>
            public const string NotYet = "2";

            /// <summary>準備リスト作成済みが選択された場合、SQLをアンコメントするキー</summary>
            public const string CreatedUncommentKey = "Created";
            /// <summary>未実施が選択された場合、SQLをアンコメントするキー</summary>
            public const string NotYetUncommentKey = "NotYet";

        }

        /// <summary>
        /// 棚卸時間
        /// </summary>
        private static class InventoryTime
        {
            /// <summary>拡張データの連番</summary>
            public const short Seq = 1;
        }

        /// <summary>
        /// 新旧区分
        /// </summary>
        private static class OldNewDivition
        {
            /// <summary>拡張データの連番</summary>
            public const short Seq = 1;
        }

        /// <summary>
        /// 受払区分
        /// </summary>
        private static class InoutDivision
        {
            /// <summary>拡張データの連番</summary>
            public const short Seq = 1;
        }

        /// <summary>
        /// 予備品作業区分
        /// </summary>
        private static class WorkDivision
        {
            /// <summary>拡張データの連番</summary>
            public const short Seq = 1;
        }

        /// <summary>
        /// 予備品出庫区分
        /// </summary>
        private static class IssueDivision
        {
            /// <summary>拡張データの連番</summary>
            public const short Seq = 1;
        }

        /// <summary>
        /// 棚卸作成区分
        /// </summary>
        private static class Creation
        {
            /// <summary>拡張データの連番</summary>
            public const short Seq = 1;
        }

        /// <summary>
        /// 丸め処理区分
        /// </summary>
        private static class RoundDivision
        {
            /// <summary>拡張データの連番</summary>
            public const short Seq = 1;
        }

        /// <summary>
        /// 新規取込で取り込むファイル定義
        /// </summary>
        private static class UploadFile
        {
            /// <summary>棚卸準備表(EXCEL)の帳票ID</summary>
            public const string ExcelReportId = "RP0410";
            /// <summary>棚卸準備表(CSV)の帳票ID</summary>
            public const string CsvReportId = "RP0310";
            /// <summary>シート番号</summary>
            public const int SheetNo = 1;
            /// <summary>ヘッダー情報のコントロールグループID</summary>
            public const string HeaderId = "LIST_000_1";
            /// <summary>明細情報のコントロールグループID</summary>
            public const string ListId = "LIST_010_1";
        }

        /// <summary>
        /// 拡張データに持っている取得条件
        /// </summary>
        public static class condPartsType
        {
            /// <summary>
            /// 出庫区分
            /// </summary>
            public static class shippingDivition
            {
                /// <summary>構成グループID</summary>
                public const short StructureGroupId = 1960;
                /// <summary>連番</summary>
                public const short Seq = 1;
                /// <summary>拡張データ</summary>
                public const string ExData = "0";
            }
        }

        /// <summary>
        /// 帳票用
        /// </summary>
        private static class PartsInfoInout
        {
            /// <summary>棚卸ID</summary>
            public const string InventoryId = "InventoryId";
        }

        /// <summary>
        /// ステータス
        /// </summary>
        private static class PartsStatus
        {
            /// <summary>準備リスト未作成</summary>
            public const int NotCreated = 0;
            /// <summary>準備リスト作成</summary>
            public const int Created = 1;
            /// <summary>棚卸確定時</summary>
            public const int Confirm = 2;
        }

        /// <summary>
        /// 項目キー名
        /// </summary>
        private static class KeyName
        {
            /// <summary>棚卸一覧 検索条件 部門</summary>
            public const string ConditionDepartment = "department";
            /// <summary>棚卸一覧 棚卸数</summary>
            public const string InventoryQuantity = "inventory_quantity";
            /// <summary>棚卸一覧 棚卸ID</summary>
            public const string InventoryId = "inventory_id";
            /// <summary>新規登録 棚枝番</summary>
            public const string PartsLocationDetailNo = "parts_location_detail_no";
            /// <summary>新規登録 予備品</summary>
            public const string PartsNo = "parts_no";
            /// <summary>新規登録 棚卸日時</summary>
            public const string InventoryDatetime = "inventory_datetime";
            /// <summary>新規登録 勘定科目</summary>
            public const string Account = "account";
            /// <summary>新規登録 部門</summary>
            public const string Department = "department";
        }

        /// <summary>
        /// 項目キー名（棚卸準備リスト）
        /// </summary>
        private static class KeyNameReport
        {
            /// <summary>棚卸一覧 棚卸数</summary>
            public const string StorageLocationId = "storage_location_id";
            /// <summary>棚卸一覧 棚卸ID</summary>
            public const string DepartmentIdList = "department_id_list";
            /// <summary>工場名</summary>
            public const string FactoryName = "factory_name";
            /// <summary>予備品倉庫名</summary>
            public const string WarehouseName = "warehouse_name";
            /// <summary>棚枝番</summary>
            public const string PartsLocationDetailNo = "PartsLocationDetailNo";
            /// <summary>棚卸一覧 棚卸ID</summary>
            public const string PartsLocationDetailNoIsNull = "PartsLocationDetailNoIsNull";
        }

        /// <summary>
        /// 最終時刻
        /// </summary>
        private static string lastTime = "23:59:59";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_PT0003() : base()
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

            // 画面Noごとに分岐
            switch (this.FormNo)
            {
                case ConductInfo.FormDetail.No:     // 受払履歴検索
                case ConductInfo.FormEdit.No:       // 入庫単価入力画面
                    return InitSearch();
                    break;
                case ConductInfo.FormRegist.No: // 新規登録画面
                    if (!setInitValue())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
            }

            if (compareId.IsBack())
            {
                // 戻る場合
                return InitSearch();
            }

            if (compareId.IsRegist() && this.FormNo == ConductInfo.FormList.No)
            {
                // 新規登録画面から戻ってきた場合
                return InitSearch();
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
                case ConductInfo.FormList.No:     // 一覧検索
                    // 棚卸データ一覧、入庫一覧、出庫一覧
                    if (!formListSearch())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormDetail.No:     // 受払履歴検索
                    if (!searchInoutList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormEdit.No:     // 入庫単価入力画面
                    if (!searchEnterInputList())
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
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsRegist() || compareId.IsStartId(ConductInfo.FormList.Button.TemporaryRegist) || compareId.IsStartId(ConductInfo.FormList.Button.AdjustDifference)
                || compareId.IsStartId(ConductInfo.FormList.Button.ConfirmInventory) || compareId.IsStartId(ConductInfo.FormList.Button.CancelConfirmInventory))
            {
                // 新規登録画面の登録、一時登録、棚差調整、棚卸確定、棚卸確定解除の場合
                // 登録処理実行
                return Regist();
            }
            else if (compareId.IsStartId(ConductInfo.FormList.Button.DeletePreparation))
            {
                // 棚卸準備削除の場合
                // 削除処理実行
                return Delete();
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

            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            switch (this.FormNo)
            {
                case ConductInfo.FormList.No:
                    //一覧画面
                    if (compareId.IsStartId(ConductInfo.FormList.Button.TemporaryRegist))
                    {
                        //一時登録
                        resultRegist = executeRegistInventoryQuantity();
                    }
                    else if (compareId.IsStartId(ConductInfo.FormList.Button.AdjustDifference))
                    {
                        //棚差調整
                        resultRegist = executeRegistInventoryDifference();
                    }
                    else if (compareId.IsStartId(ConductInfo.FormList.Button.ConfirmInventory))
                    {
                        //棚卸確定
                        resultRegist = executeConfirmInventory();
                    }
                    else if (compareId.IsStartId(ConductInfo.FormList.Button.CancelConfirmInventory))
                    {
                        //棚卸確定解除
                        resultRegist = executeCancelConfirmInventory();
                    }
                    break;
                case ConductInfo.FormEdit.No:
                    // 編集画面の場合の登録処理
                    resultRegist = enterInputRegist();
                    break;
                case ConductInfo.FormRegist.No:
                    // 新規登録画面の場合の登録処理
                    resultRegist = executeNewRegist();
                    break;
                default:
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
            // 未設定時にメッセージを設定
            if (string.IsNullOrEmpty(this.MsgId))
            {
                //「登録処理に成功しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200003 });
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            this.ResultList = new();

            //削除処理
            bool resultFlg = deleteInventory();
            if (resultFlg)
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
                //「棚卸準備取消が完了しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, ComRes.ID.ID111160038 });
            }
            else
            {
                this.Status = CommonProcReturn.ProcStatus.Warning;
            }

            return resultFlg ? ComConsts.RETURN_RESULT.OK : ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            int result = 0;
            this.ResultList = new();
            int reportFactoryId = 0;

            switch (this.CtrlId)
            {
                case ConductInfo.FormList.Button.Output:

                    //棚卸準備リスト
                    // private変数初期化
                    addInventoryIdList = new List<long>();

                    if (!registInventory())
                    {
                        // 棚卸データの登録に失敗した場合

                        //「登録処理に失敗しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return ComConsts.RETURN_RESULT.NG;
                        break;
                    }

                    //棚卸準備リストの出力
                    result = 0;

                    // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
                    int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                    reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, UploadFile.ExcelReportId, this.db) ? userFactoryId : 0;

                    // ページ情報取得
                    var pageInfo = GetPageInfo(
                        ConductInfo.FormList.EnterList.List,     // 一覧のコントールID
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
                        new { FactoryId = reportFactoryId, ReportId = UploadFile.ExcelReportId },
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
                            ConductInfo.FormList.List.InventoryList,       　　 // 一覧のコントールID
                            keyInfo,                                            // 設定したキー情報
                            this.resultInfoDictionary);                         // 画面データ

                        // 出力時、棚卸データが追加された場合のIDも出力対象とする
                        foreach (long inventoryId in addInventoryIdList)
                        {
                            SelectKeyData addKeyData = new SelectKeyData();
                            // キー情報にInventoryIdが存在する場合、キー情報を出力条件に追加
                            if (keyInfo.Key1 == PartsInfoInout.InventoryId)
                            {
                                addKeyData.Key1 = inventoryId;
                            }
                            if (keyInfo.Key2 == PartsInfoInout.InventoryId)
                            {
                                addKeyData.Key2 = inventoryId;
                            }
                            if (keyInfo.Key3 == PartsInfoInout.InventoryId)
                            {
                                addKeyData.Key3 = inventoryId;
                            }
                            // キー情報の何れかに値が設定されていれば、キー情報を出力条件に追加
                            if (addKeyData.Key1 != null || addKeyData.Key2 != null || addKeyData.Key3 != null)
                            {
                                selectKeyDataList.Add(addKeyData);
                            }
                        }

                        // シートNoをキーとして帳票用選択キーデータを保存する
                        dicSelectKeyDataList.Add(sheetDefine.SheetNo, selectKeyDataList);
                    }

                    // 固定出力データのセット
                    // 検索条件を画面より取得してデータクラスへセット
                    Dao.searchCondition condition = getCondition(out List<string> listUnComment);
                    condition.PartsLocationId = condition.StorageLocationId;

                    // 工場名、予備品倉庫名を取得
                    IList<Dao.searchCondition> infoList = new List<Dao.searchCondition>();
                    infoList.Add(condition);
                    TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchCondition>(ref infoList, new List<StructureType> { StructureType.SpareLocation }, this.db, this.LanguageId);

                    int storageLocationId = condition.StorageLocationId;
                    string departmentIdList = string.Join("|", condition.DepartmentIdList);
                    Dictionary<string, string> dicFixedValue = new Dictionary<string, string>();
                    dicFixedValue.Add(KeyNameReport.StorageLocationId, storageLocationId.ToString());
                    dicFixedValue.Add(KeyNameReport.DepartmentIdList, departmentIdList);
                    dicFixedValue.Add(KeyNameReport.FactoryName, condition.FactoryName);
                    dicFixedValue.Add(KeyNameReport.WarehouseName, condition.WarehouseName);

                    // エクセル出力共通処理
                    TMQUtil.CommonOutputExcel(
                        reportFactoryId,             // 工場ID
                        this.PgmId,                  // プログラムID
                        dicSelectKeyDataList,        // 選択キー情報リスト
                        searchCondition,             // 検索条件
                        UploadFile.ExcelReportId,         // 帳票ID
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
                        null,
                        null,
                        null,
                        dicFixedValue);

                    // OUTPUTパラメータに設定
                    this.OutputFileType = ComConsts.REPORT.FILETYPE.EXCEL;
                    this.OutputFileName = fileName;
                    this.OutputStream = memStream;

                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                    return ComConsts.RETURN_RESULT.OK;
                    break;

                case ConductInfo.FormList.Button.OutputCsv:
                    //棚卸準備リスト(CSV)
                    // private変数初期化
                    addInventoryIdList = new List<long>();

                    if (!registInventory())
                    {
                        // 棚卸データの登録に失敗した場合

                        //「登録処理に失敗しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return ComConsts.RETURN_RESULT.NG;
                        break;
                    }
                    //棚卸準備リスト(CSV)出力
                    if (!outputCsv())
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                    return ComConsts.RETURN_RESULT.OK;
                    break;
                default:
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「コントロールIDが不正です。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID911100001 });

                    // エラーログ出力
                    writeErrorLog(this.MsgId);
                    return ComConsts.RETURN_RESULT.NG;
            }

            return result;

        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int UploadImpl()
        {
            this.ResultList = new();

            // 取込と入力チェック
            if (isErrorUpload())
            {
                this.Status = CommonProcReturn.ProcStatus.Warning;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
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
        /// 一覧画面　検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool formListSearch()
        {
            // 検索条件を画面より取得してデータクラスへセット
            Dao.searchCondition condition = getCondition(out List<string> listUnComment);
            if (condition == null)
            {
                return false;
            }

            bool flg = true;
            // 棚卸データ一覧
            if (!searchList(condition, listUnComment))
            {
                flg = false;
            }
            // 入庫一覧
            if (!searchEnterList(condition, listUnComment))
            {
                flg = false;
            }
            // 出庫一覧
            if (!searchIssueList(condition, listUnComment))
            {
                flg = false;
            }
            // 出庫一覧(子要素)
            if (!searchIssueChildList(condition, listUnComment))
            {
                flg = false;
            }
            return flg;
        }
        #endregion

    }
}