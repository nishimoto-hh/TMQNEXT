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

namespace BusinessLogic_HM0002
{
    /// <summary>
    /// 変更管理 長期計画
    /// </summary>
    public partial class BusinessLogic_HM0002 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数
        /// <summary>
        /// 処理対象グループ番号
        /// </summary>
        private static class TargetGrpNo
        {
            /// <summary>テスト用</summary>
            public const short GroupTest = 4;
            /// <summary>サンプル用</summary>
            public const short GroupSample = 5;
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
            /// 検索結果の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string SearchResult1 = "SearchResult_1";
            /// <summary>
            /// 検索結果の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string SearchResult2 = "SearchResult_2";

            public static class Button
            {
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
            }
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"HistoryLongPlan";

            /// <summary>
            /// 一覧画面で使用するSQL
            /// </summary>
            public static class List
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = SqlName.SubDir + @"\List";
                /// <summary>一覧情報取得SQL</summary>
                public const string GetHistoryList = "GetHistoryLongPlanList";
                /// <summary>一覧スケジュール情報取得SQL</summary>
                public const string GetHistoryListSchedule = "GetHistoryLongPlanSchedule";
            }

            /// <summary>SQL名：一覧取得(FROMもSELECTも無し)</summary>
            public const string GetList = "[対象データ名]List";
            /// <summary>SQL名：出力一覧取得</summary>
            public const string GetListForReport = "Select_[対象データ名]ListForReport";
            /// <summary>SQL名：登録</summary>
            public const string Insert = "Insert_[対象データ名]";
            /// <summary>SQL名：更新</summary>
            public const string Update = "Update_[対象データ名]";
            /// <summary>SQL名：削除/summary>
            public const string Delete = "Delete_[対象データ名]";

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
                    /// 検索条件(自分の件名のみ表示)
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
                //case ConductInfo.FormDetail.FormNo:
                //    // 参照画面
                //    // 詳細編集画面の新規登録後と初期表示
                //    DetailDispType detailType = compareId.IsRegist() ? DetailDispType.AfterRegist : DetailDispType.Init;
                //    if (compareId.IsBack())
                //    {
                //        // 戻るの場合
                //        detailType = DetailDispType.Search;
                //    }
                //    if (compareId.IsStartId(ConductInfo.FormMakeMaintainance.ButtonId.Make))
                //    {
                //        // 保全活動画面の保全活動作成ボタンの場合
                //        detailType = DetailDispType.Search;
                //    }
                //    if (compareId.IsStartId(ConductInfo.FormPostpone.ButtonId.Postpone))
                //    {
                //        // 予定作業一括延期画面の予定作業一括延期画面ボタンの場合
                //        detailType = DetailDispType.Search;
                //    }
                //    if (!initDetail(detailType))
                //    {
                //        return ComConsts.RETURN_RESULT.NG;
                //    }
                //    break;
                //case ConductInfo.FormEdit.FormNo:
                //    // 詳細編集画面
                //    // どのボタンで起動したかを判定
                //    // 新規(一覧画面より起動)
                //    EditDispType editType = EditDispType.New;
                //    if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.Edit))
                //    {
                //        // 修正(参照画面より起動)
                //        editType = EditDispType.Update;
                //    }
                //    if (compareId.IsStartId(ConductInfo.FormDetail.ButtonId.Copy))
                //    {
                //        // 複写(参照画面より起動)
                //        editType = EditDispType.Copy;
                //    }
                //    // 初期化処理
                //    if (!initEdit(editType))
                //    {
                //        return ComConsts.RETURN_RESULT.NG;
                //    }
                //    break;
                //case ConductInfo.FormMakePlan.FormNo:
                //    // 計画一括作成画面
                //    initMakePlan();
                //    return ComConsts.RETURN_RESULT.OK;
                //case ConductInfo.FormMakeMaintainance.FormNo:
                //    // 保全活動作成画面
                //    initMakeMaintainance();
                //    return ComConsts.RETURN_RESULT.OK;
                //case ConductInfo.FormSelectStandards.FormNo:
                //    // 機器別管理基準選択画面
                //    initSelectStandards();
                //    return ComConsts.RETURN_RESULT.OK;
                //case ConductInfo.FormBudgetOutput.FormNo:
                //    // 予算出力画面
                //    initBudgetOutput();
                //    return ComConsts.RETURN_RESULT.OK;
                //case ConductInfo.FormPostpone.FormNo:
                //    // 予定作業一括延期画面
                //    initPostpone();
                //    return ComConsts.RETURN_RESULT.OK;
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
                //case ConductInfo.FormDetail.FormNo:
                //    // 参照画面
                //    if (!initDetail(DetailDispType.Search))
                //    {
                //        return ComConsts.RETURN_RESULT.NG;
                //    }
                //    break;
                //case ConductInfo.FormSelectStandards.FormNo:
                //    if (!searchSelectStandards())
                //    {
                //        return ComConsts.RETURN_RESULT.NG;
                //    }
                //    break;
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
                    return Regist();
                    break;
            }
            //if (compareId.IsRegist())
            //{
            //    // 登録の場合
            //    // 登録処理実行
            //    return Regist();
            //}
            //else if (compareId.IsDelete())
            //{
            //    // 削除の場合
            //    // 削除処理実行
            //    return Delete();
            //}
            //else if (compareId.IsStartId("Test"))
            //{
            //    // Testボタンの場合(画面で独自の処理)
            //}
            // 他の処理がある場合、else if 節に条件を追加する
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

            // 登録ボタンが複数の画面に無い場合、分岐は不要
            // 処理を実行する画面Noの値により処理を分岐する
            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo: // 一覧画面
                    //一括承認、一括否認
                    resultRegist = registFormList();
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

            // 一覧のチェックされた行のレコードを削除するイメージ
            // 削除SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Delete, out string sql);
            // 削除処理実行
            // 第三引数は機能独自の入力チェック
            DeleteSelectedList<Dao.searchResult>(TargetCtrlId.SearchResult1, sql, isErrorDeleteRow);

            // 行削除エラーチェック処理、削除処理の引数として渡す
            // エラーの時True
            bool isErrorDeleteRow(string listCtrlId, List<Dictionary<string, object>> deleteList)
            {
                // 削除対象の行を繰り返しチェック
                foreach (var deleteRow in deleteList)
                {
                    Dao.searchResult row = new();
                    SetDataClassFromDictionary(deleteRow, listCtrlId, row);

                    if (row.ItemNameTest != null)
                    {
                        // エラーメッセージを設定してTrueを返す
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141300004 });
                        return true;
                    }
                }
                // エラーが無い場合Falseを返す
                return false;
            }

            // 再検索処理
            if (!searchList(false))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
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

        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdit()
        {
            // 排他チェック
            if (isErrorExclusive())
            {
                return false;
            }

            // 入力チェック
            if (isErrorRegist())
            {
                return false;
            }

            // 画面情報取得
            DateTime now = DateTime.Now;
            // ここでは登録する内容を取得するので、テーブルのデータクラスを指定
            Dao.searchResult registInfo = getRegistInfo<Dao.searchResult>(TargetGrpNo.GroupTest, now);

            // 登録
            if (!registDb(registInfo))
            {
                return false;
            }

            // 再検索
            if (!searchList(false))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive()
        {
            // 排他チェックに必要な項目が複数のコントロールにまたがって定義されていることは無いと思われるので、コントロールIDで指定

            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(TargetCtrlId.SearchResult1);
            var lockKeyMaps = GetLockKeyMaps(TargetCtrlId.SearchResult1);

            // 単一の場合の排他チェック
            if (!checkExclusiveSingle(TargetCtrlId.SearchResult1))
            {
                // エラーの場合
                return true;
            }

            // 明細(複数)の場合の排他チェック
            var list = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.SearchResult1, true);
            if (!checkExclusiveList(TargetCtrlId.SearchResult1, list))
            {
                // エラーの場合
                return true;
            }

            // 排他チェックOK
            return false;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNo">取得するグループ</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(short groupNo, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 相称型を使用することで、色々な型を呼出元で宣言することができる
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

            T resultInfo = new();
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
            return resultInfo;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(リスト)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="ctrlId">取得するコントロールID</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラスのリスト</returns>
        private List<T> getRegistInfoList<T>(string ctrlId, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(TargetCtrlId.SearchResult1);
            // コントロールIDにより画面の項目(一覧)を取得
            List<Dictionary<string, object>> resultList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.SearchResult1);
            // 戻り値となるデータクラスのリスト
            List<T> registInfoList = new();
            // 一覧を繰り返し、データクラスに変換、リストへ追加する
            foreach (var resultRow in resultList)
            {
                T registInfo = new();
                if (!SetExecuteConditionByDataClass<T>(resultRow, TargetCtrlId.SearchResult1, registInfo, now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return registInfoList;
                }
                registInfoList.Add(registInfo);
            }
            return registInfoList;
        }

        /// <summary>
        /// 登録処理　入力チェック
        /// </summary>
        /// <returns>入力チェックエラーがある場合True</returns>
        private bool isErrorRegist()
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 明細の場合のイメージ
            if (isErrorRegistForList(ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            // 単一の項目の場合のイメージ
            if (isErrorRegistForSingle(ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            // このメソッドの中でしか使用しない処理は、以下のようにローカル関数として記載できます
            // 他のメソッドから不要に参照できなくなります
            bool isErrorRegistForList(ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                // 一覧の場合の入力チェックのサンプルです。
                // グループの中に一つの一覧のみが配置されている想定で処理を行っています。

                bool isError = false;   // 処理全体でエラーの有無を保持

                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(TargetCtrlId.SearchResult1);

                // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                // 画面に表示されている(=削除されていない)項目を取得
                var targetDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.SearchResult1);

                // 一覧の件数分絞り込み
                foreach (var rowDic in targetDicList)
                {
                    // Dictionaryをデータクラスに変換
                    Dao.searchResult result = new();
                    SetDataClassFromDictionary(rowDic, TargetCtrlId.SearchResult1, result);
                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(rowDic);
                    bool isErrorRow = false; // 行単位でエラーの有無を保持
                    // 実際の入力チェック(内容はサンプル)
                    if (string.IsNullOrEmpty(result.ItemNameTest))
                    {
                        isErrorRow = true;
                        // エラーの場合
                        // エラーメッセージとエラーを設定する画面項目を取得してセット
                        string errMsg = GetResMessage(new string[] { ComRes.ID.ID941260007, ComRes.ID.ID111090032 }); // エラーメッセージはリソースに定義されたもののみが利用可能です
                        string val = info.getValName("ItemNameTest"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                        errorInfo.setError(errMsg, val); // エラー情報をセット
                    }

                    if (isErrorRow)
                    {
                        // 行でエラーのあった場合、エラー情報を設定する
                        errorInfoDictionary.Add(errorInfo.Result);
                        isError = true;
                    }
                }
                return isError;
            }

            bool isErrorRegistForSingle(ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                // 一覧ではない、単一の項目の場合の入力チェックのサンプルです。
                // 一つのグループの中に複数のコントロールIDの項目が配置されていることを想定しています。
                // ですが、入力チェックはおそらくコントロールIDごとに行うと思われます。

                bool isError = false;   // 処理全体でエラーの有無を保持

                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(TargetCtrlId.SearchResult1);

                // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                // 単一の内容を取得
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlId.SearchResult1);

                // Dictionaryをデータクラスに変換
                Dao.searchResult result = new();
                SetDataClassFromDictionary(targetDic, TargetCtrlId.SearchResult1, result);

                // エラー情報格納クラス
                ErrorInfo errorInfo = new ErrorInfo(targetDic);

                // 実際の入力チェック(内容はサンプル)
                if (string.IsNullOrEmpty(result.ItemNameTest))
                {
                    isError = true;
                    // エラーの場合
                    // エラーメッセージとエラーを設定する画面項目を取得してセット
                    string errMsg = GetResMessage(new string[] { ComRes.ID.ID941260007, ComRes.ID.ID111090032 }); // エラーメッセージはリソースに定義されたもののみが利用可能です
                    string val = info.getValName("ItemNameTest"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                }

                return isError;
            }

        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <returns>エラーの場合False</returns>
        private bool registDb(Dao.searchResult registInfo)
        {
            // TODO:registInfoは登録するテーブルの型、データクラス作成後に変更
            string sqlName;

            // 画面遷移アクション区分に応じてINSERT/UPDATEを分岐していますが、ボタンによって処理が明らかならば必要ありません。
            // 同じボタンでINSERT/UPDATEを切り替える場合は、画面にキー値を保持しているかで判定してください。
            if (this.TransActionDiv == LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Edit)
            {
                // 修正ボタンの場合
                // 更新SQL文の取得
                sqlName = SqlName.Update;
            }
            else
            {
                // 新規・複写ボタンの場合
                // TODO:シーケンスを採番する処理

                // 新規登録SQL文の取得
                sqlName = SqlName.Insert;
            }
            // 登録SQL実行
            bool result = TMQUtil.SqlExecuteClass.Regist(sqlName, SqlName.SubDir, registInfo, this.db);
            return result;
        }

        #endregion

        #region 見直し予定
        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            int result = 0;
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

            return result;

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