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
using static CommonSTDUtil.CommonConstants;
using static CommonTMQUtil.CommonTMQConstants.Attachment;
using static CommonTMQUtil.CommonTMQConstants.MsStructure;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MC0002.BusinessLogicDataClass_MC0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_MC0002
{
    /// <summary>
    /// 機器別管理基準標準
    /// </summary>
    public partial class BusinessLogic_MC0002 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"ManagementStandards";

            /// <summary>
            /// 標準一覧画面
            /// </summary>
            public static class List
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDirList = SqlName.SubDir + @"\" + @"List";
                /// <summary>
                /// 標準一覧検索時に使用する一時テーブル(#temp_location)に地区IDを格納するSQL
                /// </summary>
                public const string InsertDistrictIdForTempLoc = "InsertDistrictIdForTempLoc";
            }

            /// <summary>
            /// 詳細画面
            /// </summary>
            public static class Detail
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDirDetail = SqlName.SubDir + @"\" + @"Detail";
                /// <summary>
                /// 保全項目一覧取得SQL
                /// </summary>
                public const string GetManagementstandardsDetailList = "GetManagementstandardsDetailList";
                /// <summary>
                /// 機器別管理基準標準に紐付く機器別管理基準標準詳細を削除するSQL
                /// </summary>
                public const string DeleteManagementStandardsDetail = "DeleteManagementStandardsDetail";
            }

            /// <summary>
            /// 詳細編集画面
            /// </summary>
            public static class Edit
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDirEdit = SqlName.SubDir + @"\" + @"Edit";
                /// <summary>
                /// 機器別管理基準標準 新規登録
                /// </summary>
                public const string InsertManagementStandardsInfo = "InsertManagementStandardsInfo";
                /// <summary>
                /// 機器別管理基準標準 更新
                /// </summary>
                public const string UpdateManagementStandardsInfo = "UpdateManagementStandardsInfo";
            }

            /// <summary>
            /// 保全項目編集画面
            /// </summary>
            public static class ManagementStandardsDetail
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDirMsd = SqlName.SubDir + @"\" + @"ManagementStandardsDetail";
                /// <summary>
                /// 部位・項目重複チェック用SQL
                /// </summary>
                public const string GetCountManagementStandardDetail = "GetCountManagementStandardDetail";
                /// <summary>
                /// 機器別管理基準標準詳細登録SQL
                /// </summary>
                public const string InsertManagementStandardsDetail = "InsertManagementStandardsDetail";
                /// <summary>
                /// 機器別管理基準標準詳細更新SQL
                /// </summary>
                public const string UpdateManagementStandardsDetail = "UpdateManagementStandardsDetail";
            }

            /// <summary>
            /// 標準割当画面
            /// </summary>
            public static class Quota
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDirQuota = SqlName.SubDir + @"\" + @"Quota";
                /// <summary>
                /// 機器一覧取得SQL
                /// </summary>
                public const string GetMachineList = "GetMachineList";
                /// <summary>
                /// 割当を行う機器別管理基準詳細を取得
                /// </summary>
                public const string GetManagementStandardsDetailForQuota = "GetManagementStandardsDetailForQuota";
                /// <summary>
                /// 割当時、機器別管理基準部位テーブル登録
                /// </summary>
                public const string InsertManagementStandardsComponent = "InsertManagementStandardsComponent";
                /// <summary>
                /// 割当時、機器別管理基準内容テーブル登録
                /// </summary>
                public const string InsertManagementStandardsContent = "InsertManagementStandardsContent";
                /// <summary>
                /// 割当時、保全スケジュールテーブル登録
                /// </summary>
                public const string InsertMaintainanceSchedule = "InsertMaintainanceSchedule";
                /// <summary>
                /// 割当時、保全スケジュール詳細テーブル登録
                /// </summary>
                public const string InsertMaintainanceScheduleDetail = "InsertMaintainanceScheduleDetail";
                /// <summary>
                /// 入力されたテキストから構成IDを取得
                /// </summary>
                public const string GetStructureIdByTransText = "GetStructureIdByTransText";
                /// <summary>
                /// 構成IDより翻訳テキストを取得(工場アイテム)
                /// </summary>
                public const string GetTransTextByStructureId = "GetTransTextByStructureId";
                /// <summary>
                /// 構成マスタ更新(削除フラグをFalseに更新)
                /// </summary>
                public const string UpdateMsStructure = "UpdateMsStructure";
                /// <summary>
                /// 機器に紐付く保全部位・保全項目の組み合わせを取得
                /// </summary>
                public const string GetSiteAndContentByMachineId = "GetSiteAndContentByMachineId";
                /// <summary>
                /// 機器に紐付く保全部位・保全項目の組み合わせを取得(機番IDリストで複数取得)
                /// </summary>
                public const string GetSiteAndContentByMachineIdList = "GetSiteAndContentByMachineIdList";
                /// <summary>
                /// 同一点検種別のスケジュール管理基準(開始日基準or完了日基準)を更新
                /// </summary>
                public const string UpdateScheduleType = "UpdateScheduleType";
                /// <summary>
                /// 同一点検種別のスケジュール詳細を更新するためのデータを取得
                /// </summary>
                public const string GetManagementStandardsDifferenceCycle = "GetManagementStandardsDifferenceCycle";
                /// <summary>
                /// 保全スケジュール詳細データを削除
                /// </summary>
                public const string DeleteScheduleDetail = "DeleteScheduleDetail";

                /// <summary>
                /// 機器別管理基準部位テーブルのシーケンスを取得
                /// </summary>
                public const string GetSeqComponent = "GetSeqComponent";
                /// <summary>
                /// 機器別管理基準内容テーブルのシーケンスを取得
                /// </summary>
                public const string GetSeqContent = "GetSeqContent";
                /// <summary>
                /// 保全スケジュールテーブルのシーケンスを取得
                /// </summary>
                public const string GetSeqSchedule = "GetSeqSchedule";
                /// <summary>
                /// 機器別管理基準部位テーブルのシーケンスを更新
                /// </summary>
                public const string UpdateSeqComponent = "UpdateSeqComponent";
                /// <summary>
                /// 機器別管理基準内容テーブルのシーケンスを更新
                /// </summary>
                public const string UpdateSeqContent = "UpdateSeqContent";
                /// <summary>
                /// 保全スケジュールテーブルのシーケンスを更新
                /// </summary>
                public const string UpdateSeqSchedule = "UpdateSeqSchedule";
                /// <summary>
                /// 機器別管理基準部位テーブル登録(バルクインサート用)
                /// </summary>
                public const string InsertManagementStandardsComponentTable = "InsertManagementStandardsComponentTable";
                /// <summary>
                /// 機器別管理基準内容テーブル登録(バルクインサート用)
                /// </summary>
                public const string InsertManagementStandardsContentTable = "InsertManagementStandardsContentTable";
                /// <summary>
                /// 保全スケジュールテーブル登録(バルクインサート用)
                /// </summary>
                public const string InsertMaintainanceScheduleTable = "InsertMaintainanceScheduleTable";
                /// <summary>
                /// 保全スケジュール詳細テーブル登録(バルクインサート用)
                /// </summary>
                public const string InsertMaintainanceScheduleDetailTable = "InsertMaintainanceScheduleDetailTable";
                /// <summary>
                /// 機番IDと開始日を格納するための一時テーブルを作成
                /// </summary>
                public const string CreateIdDateForProc = "CreateIdDateForProc";
                /// <summary>
                /// 機番IDと開始日を一時テーブルに格納
                /// </summary>
                public const string InsertIdDateForProc = "InsertIdDateForProc";
                /// <summary>
                /// 割当処理のプロシージャを実行するSQL
                /// </summary>
                public const string InsertManagementStandardsByProc = "InsertManagementStandardsByProc";
            }

            /// <summary>
            /// 画面間で共通的に使用するSQL
            /// </summary>
            public static class Common
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDirCommon = SqlName.SubDir + @"\" + @"Common";
                /// <summary>
                /// 標準取得SQL
                /// </summary>
                public const string GetManagementstandardsList = "GetManagementstandardsList";
            }

        }

        /// <summary>
        /// 機能のコントロール情報
        /// </summary>
        private static class ConductInfo
        {
            /// <summary>
            /// 標準一覧画面
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
                    /// 標準一覧
                    /// </summary>
                    public const string List = "BODY_020_00_LST_0";
                }

                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {

                }
            }

            /// <summary>
            /// 標準詳細画面
            /// </summary>
            public static class FormDetail
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 1;

                /// <summary>
                /// 標準件名情報のグループ番号
                /// </summary>
                public const short GroupNo = 201;

                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 標準件名情報(標準名称～メモ)
                    /// </summary>
                    public const string ManagementStandardsInfo = "BODY_030_00_LST_1";
                    /// <summary>
                    /// 保全項目一覧
                    /// </summary>
                    public const string ManagementStandardsDetailList = "BODY_040_00_LST_1";
                }

                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {
                    /// <summary>
                    /// 画面上部の削除ボタン
                    /// </summary>
                    public const string Delete = "Delete";
                    /// <summary>
                    /// 画面上部の割当ボタン
                    /// </summary>
                    public const string Quota = "Quota";
                    /// <summary>
                    /// 保全項目一覧の行削除ボタン
                    /// </summary>
                    public const string DeleteManagementStandardsDetail = "DeleteManagementStandardsDetail";
                }
            }

            /// <summary>
            /// 標準詳細編集画面
            /// </summary>
            public static class FormEdit
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 2;

                /// <summary>
                /// 標準件名情報のグループ番号
                /// </summary>
                public const short GroupNo = 301;

                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 標準件名情報(標準名称～メモ)
                    /// </summary>
                    public const string ManagementStandardsInfo = "BODY_020_00_LST_2";
                }

                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {

                }
            }

            /// <summary>
            /// 保全項目編集画面
            /// </summary>
            public static class FormEditManagemantStandardsDetail
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 3;

                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 保全項目編集画面
                    /// </summary>
                    public const string ManagementStandardsDetail = "BODY_000_00_LST_3";
                }

                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {

                }
            }

            /// <summary>
            /// 標準割当画面
            /// </summary>
            public static class FormQuota
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 4;

                /// <summary>
                /// 標準件名情報のグループ番号
                /// </summary>
                public const short GroupNo = 401;

                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 検索条件(循環対象～機器別管理基準)
                    /// </summary>
                    public const string ToIsManagementStandards = "COND_020_00_LST_4";

                    /// <summary>
                    /// 機器一覧
                    /// </summary>
                    public const string MachineList = "BODY_050_00_LST_4";
                }

                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {
                    /// <summary>
                    /// 確定ボタン
                    /// </summary>
                    public const string Decision = "Decision";
                }
            }
        }

        /// <summary>
        /// グローバル変数キー
        /// </summary>
        private static class GlobalKey
        {
            // グローバル変数のキー、一覧画面の表示データを更新する用
            public const string MC0002UpdateListData = "MC0002_UpdateListData";
            // グローバル変数のキー、一覧画面用の総件数
            public const string MC0002AllListCount = "MC0002_AllListCount";
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_MC0002() : base()
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
            // 画面番号を判定
            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo:                          // 標準一覧画面
                    // メニューから選択された際の初期検索は行わない
                    return ComConsts.RETURN_RESULT.OK;
                case ConductInfo.FormDetail.FormNo:                        // 標準詳細画面
                case ConductInfo.FormEdit.FormNo:                          // 標準詳細編集画面
                case ConductInfo.FormEditManagemantStandardsDetail.FormNo: // 保全項目編集画面
                    // 初期検索実行
                    return InitSearch();

                case ConductInfo.FormQuota.FormNo: // 割当画面
                    if (!initQuota())
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
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            // 画面番号を判定
            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo: // 標準一覧画面

                    // 一覧画面検索処理
                    if (!searchList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormDetail.FormNo: // 標準詳細画面

                    // 詳細画面検索処理
                    if (!searchDetail(ConductInfo.FormList.ControlId.List, this.searchConditionDictionary))
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormEdit.FormNo: // 標準詳細編集画面

                    // 詳細編集画面検索処理
                    if (!searchEdit())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormEditManagemantStandardsDetail.FormNo: // 保全項目編集画面

                    // 保全項目編集画面検索処理
                    if (!searchManagementStandardsDetail())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormQuota.FormNo: // 割当画面

                    // 割当画面検索処理(検索ボタンクリック後)
                    if (!searchMachineList())
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
            else if (compareId.IsDelete())
            {
                // 削除の場合
                // 削除処理実行
                return Delete();
            }

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

            // 画面番号を判定
            switch (this.FormNo)
            {
                case ConductInfo.FormEdit.FormNo: // 詳細編集画面
                    if (!registEdit())
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
                    break;
                case ConductInfo.FormEditManagemantStandardsDetail.FormNo: // 保全項目編集画面
                    if (!registManagementStandardsDetail())
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
                    break;
                case ConductInfo.FormQuota.FormNo:
                    // 標準割当画面 確定ボタン
                    if (!quotaMachine())
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
                    break;
                default:
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

            // ボタンコントロールIDを判定
            switch (this.CtrlId)
            {
                case ConductInfo.FormDetail.ButtonId.Delete: // 詳細画面 画面上部の削除ボタン
                    if (!deleteManagementStandardsInfo())
                    {
                        setError();
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormDetail.ButtonId.DeleteManagementStandardsDetail: // 詳細画面 保全項目一覧の行削除ボタン
                    if (!deleteManagementStandardsDetailInfo())
                    {
                        setError();
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                default:
                    return ComConsts.RETURN_RESULT.NG;

            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「削除処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911110001 });

            return ComConsts.RETURN_RESULT.OK;

            // 削除失敗時、エラーメッセージを設定
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
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            // 処理なし
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 検索時に必要な一時テーブルを作成
        /// </summary>
        /// <param name="isList">一覧画面の場合true</param>
        /// <param name="uncommentList">アンコメントリスト</param>
        private void createTranslationTempTbl(bool isList = false, List<string> uncommentList = null)
        {
            // 翻訳する構成グループのリスト
            List<GroupId> structuregroupList = new();
            if (isList)
            {
                if (uncommentList == null)
                {
                    return;
                }
                //一覧画面の場合、表示項目のみの構成グループIDを指定する
                //場所階層、職種機種はいずれかの項目が１つでも存在する場合、構成グループIDを追加する
                List<string> locationList = new List<string>() { nameof(Dao.searchResultList.DistrictName), nameof(Dao.searchResultList.FactoryName) };
                List<string> jobList = new List<string>() { nameof(Dao.searchResultList.JobName), nameof(Dao.searchResultList.LargeClassficationName), nameof(Dao.searchResultList.MiddleClassficationName), nameof(Dao.searchResultList.SmallClassficationName) };
                int locationCount = uncommentList.FindAll(locationList.Contains).Count;
                int jobCount = uncommentList.FindAll(jobList.Contains).Count;

                if (locationCount > 0)
                {
                    structuregroupList.Add(GroupId.Location);
                }
                if (jobCount > 0)
                {
                    structuregroupList.Add(GroupId.Job);
                }
            }
            else
            {
                // 翻訳する構成グループのリスト
                structuregroupList.AddRange(new List<GroupId>
                {
                    GroupId.Location,             // 場所階層
                    GroupId.Job,                  // 職種機種
                    GroupId.Importance,           // 部位重要度
                    GroupId.Conservation,         // 保全方式
                    GroupId.MaintainanceDivision, // 保全区分
                    GroupId.MaintainanceKind,     // 点検種別
                    GroupId.ScheduleType          // スケジュール管理
                });
            }

            // 翻訳の一時テーブルを作成
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);

            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            listPf.RegistTempTable(); // 登録
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

        #endregion
    }
}