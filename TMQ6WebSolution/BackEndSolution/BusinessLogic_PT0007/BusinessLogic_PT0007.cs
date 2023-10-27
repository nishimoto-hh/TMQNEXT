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
using Dao = BusinessLogic_PT0007.BusinessLogicDataClass_PT0007;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_PT0007
{
    /// <summary>
    /// 移庫入力画面
    /// </summary>
    public partial class BusinessLogic_PT0007 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 遷移元機能ID
        /// </summary>
        private static class TransConductId
        {
            /// <summary>PT0001(予備品一覧)</summary>
            public const string PT0001 = "PT0001";
            /// <summary>PT0002(入出庫一覧)</summary>
            public const string PT0002 = "PT0002";
        }

        /// <summary>
        /// 画面遷移フラグ
        /// </summary>
        private static class TransFlg
        {
            /// <summary>
            /// 新規
            /// </summary>
            public const int FlgNew = 0;
            /// <summary>
            /// 修正
            /// </summary>
            public const int FlgEdit = 1;
        }

        /// <summary>
        /// 画面遷移タイプ
        /// </summary>
        private static class TransType
        {
            /// <summary>
            /// 棚番移庫
            /// </summary>
            public const int Subject = 2;
            /// <summary>
            /// 部門移庫
            /// </summary>
            public const int Department = 3;
        }

        /// <summary>
        /// ボタン活性/非活性
        /// </summary>
        private static class BtnVisible
        {
            /// <summary>
            /// 活性
            /// </summary>
            public const int Visible = 0;
            /// <summary>
            /// 非非活性
            /// </summary>
            public const int UnVisible = 1;
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Transfer";
            /// <summary>SQL名：共通のWITH句</summary>
            public const string CommonWith = "Common";

            /// <summary>
            /// 移庫画面の共通部分
            /// </summary>
            public static class Common
            {
                /// <summary>SQL名：予備品情報取得</summary>
                public const string GetPartsInfno = "GetPartsInfno";
                /// <summary>SQL名：移庫日取得</summary>
                public const string GetInoutDate = "GetInoutDate";
                /// <summary>SQL名：棚卸確定日を取得するための検索条件を取得</summary>
                public const string GetInventoryDateCondition = "GetInventoryDateCondition";
                /// <summary>SQL名：棚卸確定日を取得するための検索条件を取得(新規登録用)</summary>
                public const string GetInventoryDateConditionNew = "GetInventoryDateConditionNew";
                /// <summary>SQL名：移庫日以降の受払データを取得</summary>
                public const string GetNewDataOverInoutDate = "GetNewDataOverInoutDate";
                /// <summary>SQL名：棚卸データを取得</summary>
                public const string GetInventoryData = "GetInventoryData";
                /// <summary>SQL名：移庫先に対する受払の件数を取得</summary>
                public const string GetInoutCount = "GetInoutCount";
            }

            /// <summary>
            /// 棚番移庫タブ
            /// </summary>
            public static class Location
            {
                /// <summary>SQL名：棚別在庫一覧取得(新規以外)</summary>
                public const string GetLocationListEdit = "GetLocationListEdit";
                /// <summary>SQL名：棚別在庫一覧取得(新規)</summary>
                public const string GetLocationListNew = "GetLocationListNew";
                /// <summary>SQL名：移庫先情報取得</summary>
                public const string GetLocationInfoTo = "GetLocationInfoTo";
                /// <summary>SQL名：棚IDより倉庫IDを取得</summary>
                public const string GetParentIdByLocationId = "GetParentIdByLocationId";
                /// <summary>SQL名：最大更新日時取得</summary>
                public const string GetMaxUpdateDateLocation = "GetMaxUpdateDateLocation";
            }

            /// <summary>
            /// 部門移庫タブ
            /// </summary>
            public static class Department
            {
                /// <summary>SQL名：部門別在庫一覧取得(新規以外)</summary>
                public const string GetDepartmentListEdit = "GetDepartmentListEdit";
                /// <summary>SQL名：部門別在庫一覧取得(新規)</summary>
                public const string GetDepartmentListNew = "GetDepartmentListNew";
                /// <summary>SQL名：移庫先情報取得</summary>
                public const string GetDepartmentInfoTo = "GetDepartmentInfoTo";
                /// <summary>SQL名：最大更新日時取得</summary>
                public const string GetMaxUpdateDateDepartment = "GetMaxUpdateDateDepartment";
                /// <summary>SQL名：棚卸確定日を取得するための条件を取得</summary>
                public const string GetConditionToGetInventoryDate = "GetConditionToGetInventoryDate";
            }
        }

        /// <summary>
        /// フォーム、グループ、コントロールの親子関係を表現した場合の定数クラス
        /// </summary>
        private static class ConductInfo
        {
            /// <summary>
            /// フォーム番号
            /// </summary>
            public const short FormNo = 0;

            /// <summary>
            /// 画面の共通部分のコントロールID
            /// </summary>
            public static class CommonControlId
            {
                /// <summary>
                /// 予備品情報
                /// </summary>
                public const string PartsInfo = "CBODY_000_00_LST_7";
            }

            /// <summary>
            /// 棚番移庫タブのコントロールID
            /// </summary>
            public static class LocationControlId
            {
                /// <summary>
                /// 棚別在庫一覧
                /// </summary>
                public const string LocationInventoryList = "CBODY_010_00_LST_7";
                /// <summary>
                /// 移庫先情報(移庫日～予備品倉庫)
                /// </summary>
                public const string LocationTransferInfoToWarehouse = "CBODY_020_00_LST_7";
                /// <summary>
                /// 移庫先情報(棚番)
                /// </summary>
                public const string LocationTransferInfoToWarehouseId = "CBODY_070_00_LST_7";
                /// <summary>
                /// 移庫先情報(枝番)
                /// </summary>
                public const string LocationTransferInfoToJoinStr = "CBODY_080_00_LST_7";
                /// <summary>
                /// 移庫先情報(移庫数～移庫金額)
                /// </summary>
                public const string LocationTransferInfoToMoney = "CBODY_090_00_LST_7";
                /// <summary>
                /// グループ番号
                /// </summary>
                public const short GroupNo = 101;
            }

            /// <summary>
            /// 部門移庫タブのコントロールID
            /// </summary>
            public static class DepartmentControlId
            {
                /// <summary>
                /// 部門別在庫一覧
                /// </summary>
                public const string DepartmentInventoryList = "CBODY_040_00_LST_7";
                /// <summary>
                /// 移庫先情報
                /// </summary>
                public const string DepartmentTransferInfo = "CBODY_050_00_LST_7";
            }

            /// <summary>
            /// ボタンコントロールID
            /// </summary>
            public static class ButtonCtrlId
            {
                /// <summary>
                /// 棚番移庫タブ 登録ボタン
                /// </summary>
                public const string RegistLocation = "RegistLocation";
                /// <summary>
                /// 棚番移庫タブ 取消ボタン
                /// </summary>
                public const string CancelLocation = "CancelLocation";

                /// <summary>
                /// 部門移庫タブ 登録ボタン
                /// </summary>
                public const string RegistDepartment = "RegistDepartment";
                /// <summary>
                /// 部門移庫タブ 取消ボタン
                /// </summary>
                public const string CancelDepartment = "CancelDepartment";
            }

            /// <summary>
            /// 遷移元の機能
            /// </summary>
            public static class OtherConduct
            {
                /// <summary>
                /// 予備品一覧の画面項目定義テーブルのコントロールID
                /// </summary>
                public const string PT0001List = "BODY_020_00_LST_0";
                /// <summary>
                /// 入出庫一覧(棚番タブ)の画面項目定義テーブルのコントロールID
                /// </summary>
                public const string PT0002Location = "BODY_070_00_LST_0";
                /// <summary>
                /// 入出庫一覧(部門タブ)の画面項目定義テーブルのコントロールID
                /// </summary>
                public const string PT0002Department = "BODY_090_00_LST_0";
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_PT0007() : base()
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

            // 遷移元のマッピング情報追加
            AddMappingListOtherPgmId(TransConductId.PT0001);
            AddMappingListOtherPgmId(TransConductId.PT0002);

            // 検索条件を取得
            Dao.searchCondition searchCondition = getSearchCondition();

            // 初期化時チェック(修正の場合)
            initCheck(searchCondition, out int btnVisible);

            // 棚番移庫タブ初期化
            if (!initLocation(searchCondition, out int locationCnt))
            {
                // エラー
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 部門移庫タブ初期化
            if (!initDepartment(searchCondition, out int departmentCnt))
            {
                // エラー
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            //予備品情報初期化
            if (!initPartsInfo(searchCondition, btnVisible, locationCnt, departmentCnt))
            {
                // エラー
                this.Status = CommonProcReturn.ProcStatus.Error;
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
            // 処理なし
            this.ResultList = new();
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ExecuteImpl()
        {
            return Regist();
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse

            // ボタンコントロールIDで判定
            switch (this.CtrlId)
            {
                case ConductInfo.ButtonCtrlId.RegistLocation: // 棚番移庫タブ 登録ボタン
                case ConductInfo.ButtonCtrlId.CancelLocation: // 棚番移庫タブ 取消ボタン
                    resultRegist = executeLocation();
                    break;
                case ConductInfo.ButtonCtrlId.RegistDepartment: // 部門移庫タブ 登録ボタン
                case ConductInfo.ButtonCtrlId.CancelDepartment: // 部門移庫タブ 取消ボタン
                    resultRegist = executeDepartment();
                    break;
                default:
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
            // 処理なし
            this.ResultList = new();
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド

        /// <summary>
        /// 予備品情報検索
        /// </summary>
        /// <param name="btnVisible">登録・取消ボタンの活性/非活性</param>
        /// <param name="locationDataCnt">棚別在庫一覧の件数</param>
        /// <param name="departmentDataCnt">部門別在庫一覧の件数</param>
        /// <returns>エラーの場合False</returns>
        private bool initPartsInfo(Dao.searchCondition searchCondition, int btnVisible, int locationDataCnt, int departmentDataCnt)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Common.GetPartsInfno, out string outSql);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.CommonWith, out string outWithSql);
            // SQL実行
            IList<Dao.partsInfo> results = db.GetListByDataClass<Dao.partsInfo>(outWithSql + outSql, searchCondition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 丸め処理・数量と単位の結合
            results.ToList().ForEach(x => x.JoinStrAndRound());

            results[0].TransFlg = searchCondition.TransFlg;   // 画面遷移フラグ
            results[0].TransType = searchCondition.TransType; // 画面遷移タイプ
            results[0].BtnVisible = btnVisible;               // 登録・取消ボタンの活性/非活性
            results[0].TabFirstSelect = 0;                    // タブの選択回数(初期値「0」)
            results[0].LocationDataCnt = locationDataCnt;     // 棚別在庫一覧の件数
            results[0].DepartmentDataCnt = departmentDataCnt; // 部門別在庫一覧の件数

            // 新規の場合
            if (searchCondition.TransFlg == TransFlg.FlgNew)
            {
                // 一覧の件数を判定
                if (locationDataCnt == 0 && departmentDataCnt == 0)
                {
                    // 棚別在庫一覧・部門別在庫一覧どちらも0件の場合
                    // 棚別在庫一覧・部門別在庫一覧 対象情報がありません。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941160005, ComRes.ID.ID141160015 });
                }
                else if (locationDataCnt == 0 && departmentDataCnt != 0)
                {
                    // 棚別在庫一覧が0件の場合
                    // 棚別在庫一覧 対象情報がありません。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941160005, ComRes.ID.ID111160037 });
                }
                else if (locationDataCnt != 0 && departmentDataCnt == 0)
                {
                    // 部門別在庫一覧が0件の場合
                    // 部門別在庫一覧 対象情報がありません。
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941160005, ComRes.ID.ID111280044 });
                }
            }

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.CommonControlId.PartsInfo, this.pageInfoList);
            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.partsInfo>(pageInfo, results, results.Count))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 検索条件を取得
        /// </summary>
        /// <returns>検索条件のデータクラス</returns>
        private Dao.searchCondition getSearchCondition()
        {
            // 初期化
            Dao.searchCondition searchCondition = new();
            searchCondition.LanguageId = this.LanguageId;                              // 言語ID
            searchCondition.UserFactoryId = TMQUtil.GetUserFactoryId(this.UserId, db); // 本務工場

            // PT0001(予備品一覧)のディクショナリを取得
            var targetDicPT0001List = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.OtherConduct.PT0001List);
            if (targetDicPT0001List != null)
            {
                SetDataClassFromDictionary(targetDicPT0001List, ConductInfo.OtherConduct.PT0001List, searchCondition);
                searchCondition.TransFlg = TransFlg.FlgNew; // 画面遷移フラグ(新規)
                return searchCondition;
            }
            // PT0002(棚番)のディクショナリを取得
            var targetDicPT0002Location = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.OtherConduct.PT0002Location);
            if (targetDicPT0002Location != null)
            {
                SetDataClassFromDictionary(targetDicPT0002Location, ConductInfo.OtherConduct.PT0002Location, searchCondition);
                searchCondition.TransFlg = TransFlg.FlgEdit;   // 画面遷移フラグ(修正)
                searchCondition.TransType = TransType.Subject; // 画面遷移タイプ(棚番)
                return searchCondition;
            }
            // PT0002(部門)のディクショナリを取得
            var targetDicPT0002Department = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.OtherConduct.PT0002Department);
            if (targetDicPT0002Department != null)
            {
                SetDataClassFromDictionary(targetDicPT0002Department, ConductInfo.OtherConduct.PT0002Department, searchCondition);
                searchCondition.TransFlg = TransFlg.FlgEdit;      // 画面遷移フラグ(修正)
                searchCondition.TransType = TransType.Department; // 画面遷移タイプ(部門)
                return searchCondition;
            }

            return searchCondition;
        }

        /// <summary>
        /// 初期化時チェック
        /// </summary>
        /// <param name="searchCondition">検索条件</param>
        /// <param name="btnVisible">登録・取消ボタンの活性/非活性</param>
        private void initCheck(Dao.searchCondition searchCondition, out int btnVisible)
        {
            btnVisible = BtnVisible.Visible;
            string errMsg = string.Empty;

            // 修正ではない場合は何もしない
            if (searchCondition.TransFlg != TransFlg.FlgEdit)
            {
                return;
            }

            // 移庫日を取得
            DateTime inoutDate = getInoutDate();

            // 在庫確定日以前の移庫情報かチェック
            checkTargetMonth();

            // 棚卸確定日以前の移庫情報かチェック
            checkFixedDate();

            // 移庫日以降に受払が発生しているかチェック
            checkNewDataOverInoutDate();

            // 棚卸中のデータかチェック
            checkInventoryData();

            //　メッセ―ジをセット
            this.MsgId = errMsg;

            // 登録・取消ボタンを非表示にするフラグ
            if (!string.IsNullOrEmpty(errMsg))
            {
                btnVisible = BtnVisible.UnVisible;
            }

            return;

            /// <summary>
            /// 移庫日を取得
            /// </summary>
            /// <returns>移庫日</returns>
            DateTime getInoutDate()
            {
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Common.GetInoutDate, out string outSql);
                TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.CommonWith, out string outWithSql);

                // 作業Noより移庫日を取得
                DateTime inoutDate = this.db.GetEntity<DateTime>(outWithSql + outSql, searchCondition);
                return this.db.GetEntity<DateTime>(outWithSql + outSql, searchCondition);
            }

            /// <summary>
            /// 在庫確定日以前の移庫情報かチェック
            /// </summary>
            void checkTargetMonth()
            {
                // エラーメッセージがある場合は何もしない
                if (!string.IsNullOrEmpty(errMsg))
                {
                    return;
                }

                // 在庫確定日取得
                DateTime targetMonth = TMQUtil.PartsGetInfo.GetInventoryConfirmationDate(searchCondition, this.db);

                // 移庫日が在庫確定日以前の場合
                if (inoutDate <= targetMonth)
                {
                    // 「在庫確定以前の情報のため、修正・取消することはできません。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID141110002 });
                }
            }

            /// <summary>
            /// 棚卸確定日以前の移庫情報かチェック
            /// </summary>
            void checkFixedDate()
            {
                // エラーメッセージがある場合は何もしない
                if (!string.IsNullOrEmpty(errMsg))
                {
                    return;
                }

                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Common.GetInventoryDateCondition, out string outSql);
                TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.CommonWith, out string outWithSql);

                // 作業Noより取得
                IList<Dao.getInventoryDate> lotInfo = this.db.GetListByDataClass<Dao.getInventoryDate>(outWithSql + outSql, searchCondition);

                foreach (ComDao.PtLotEntity lot in lotInfo)
                {
                    // 棚卸確定日取得
                    DateTime fixedDatetime = TMQUtil.PartsGetInfo.GetTakeInventoryConfirmationDate(lot, this.db);

                    // 移庫日が棚卸確定日以前の場合
                    if (inoutDate <= fixedDatetime)
                    {
                        // 「棚卸確定以前の情報のため、修正・取消することはできません。」
                        errMsg = GetResMessage(new string[] { ComRes.ID.ID141160005 });
                        break;
                    }
                }
            }

            /// <summary>
            /// 移庫日以降に受払が発生しているかチェック
            /// </summary>
            void checkNewDataOverInoutDate()
            {
                // エラーメッセージがある場合は何もしない
                if (!string.IsNullOrEmpty(errMsg))
                {
                    return;
                }

                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Common.GetNewDataOverInoutDate, out string outSql);
                TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.CommonWith, out string outWithSql);

                // 件数を取得
                int cnt = db.GetEntityByDataClass<int>(outWithSql + outSql, searchCondition);
                if (cnt > 0)
                {
                    // 「移庫後に移庫先の受払情報が既に登録されている為、修正・取消はできません。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID141020001 });
                }
            }

            /// <summary>
            /// 棚卸中のデータかチェック
            /// </summary>
            void checkInventoryData()
            {
                // エラーメッセージがある場合は何もしない
                if (!string.IsNullOrEmpty(errMsg))
                {
                    return;
                }

                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Common.GetInventoryData, out string outSql);
                TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.CommonWith, out string outWithSql);

                // 件数を取得
                int cnt = db.GetEntityByDataClass<int>(outWithSql + outSql, searchCondition);
                if (cnt > 0)
                {
                    // 「棚卸中データのため、修正・削除はできません。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID141160010 });
                }
            }
        }

        /// <summary>
        /// 予備品情報の取得
        /// </summary>
        /// <returns>登録内容のデータクラス</returns>
        private Dao.partsInfo getPartsInfo()
        {
            // コントロールIDにより画面の項目(一覧)を取得
            Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.CommonControlId.PartsInfo);

            // TODO:ユーザIDを数値に変換するのは共通化
            Dao.partsInfo partsInfo = new();
            if (!SetExecuteConditionByDataClass<Dao.partsInfo>(result, ConductInfo.CommonControlId.PartsInfo, partsInfo, DateTime.Now, this.UserId, this.UserId))
            {
                // エラーの場合終了
                return partsInfo;
            }

            return partsInfo;
        }

        /// <summary>
        /// 登録時の共通入力チェック
        /// </summary>
        /// <param name="partsInfo">予備品情報</param>
        /// <param name="ctrlId">取得する一覧のコントロールID</param>
        /// <param name="relocationDate">移庫日</param>
        /// <param name="inventoryConditionList">棚卸確定日取得の条件</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorRegistCommon(Dao.partsInfo partsInfo, string ctrlId, DateTime relocationDate, List<Dao.getInventoryDate> inventoryConditionList, long lotCntrlId)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 単一の項目の場合のイメージ
            if (isErrorRegistForSingle(ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            bool isErrorRegistForSingle(ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                // エラー情報格納クラス
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                ErrorInfo errorInfo = new ErrorInfo(targetDic);
                bool isError = false;   // 処理全体でエラーの有無を保持
                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(ctrlId);

                // 移庫日に未来日が入力されている場合はエラー
                if (relocationDate > DateTime.Now)
                {
                    string errMsg = GetResMessage(new string[] { ComRes.ID.ID141320001 }); // 「未来の日付は入力できません。」
                    string val = info.getValName("RelocationDate");
                    errorInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    return true;
                }

                // 在庫確定日取得
                DateTime targetMonth = TMQUtil.PartsGetInfo.GetInventoryConfirmationDate(partsInfo, this.db);

                // 移庫日が在庫確定日以前の場合
                if (relocationDate <= targetMonth)
                {
                    string errMsg = GetResMessage(new string[] { ComRes.ID.ID141110001, relocationDate.ToString() }); // 「在庫確定前[{0}]の日付は入力できません。」
                    string val = info.getValName("RelocationDate");
                    errorInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    return true;
                }

                // 棚卸確定日チェック
                foreach (Dao.getInventoryDate condition in inventoryConditionList)
                {
                    // 棚卸確定日取得
                    DateTime fixedDatetime = TMQUtil.PartsGetInfo.GetTakeInventoryConfirmationDate(condition, this.db);

                    // 移庫日が棚卸確定日以前の場合
                    if (relocationDate <= fixedDatetime)
                    {
                        string errMsg = GetResMessage(new string[] { ComRes.ID.ID141160004, relocationDate.ToString() }); // 「棚卸確定前[{0}]の日付は入力できません。」
                        string val = info.getValName("RelocationDate");
                        errorInfo.setError(errMsg, val); // エラー情報をセット
                        errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                        return true;
                    }
                }

                // 移庫する情報を取得
                ComDao.PtLotEntity lotInfo = new ComDao.PtLotEntity().GetEntity(lotCntrlId, this.db);
                // 移庫日 < 入庫日の場合はエラーとする
                if (relocationDate < lotInfo.ReceivingDatetime)
                {
                    string errMsg = GetResMessage(new string[] { ComRes.ID.ID141020005 }); // 「移庫日が入庫日以前のデータは移庫できません。」
                    string val = info.getValName("RelocationDate");
                    errorInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 移庫先に対する受払が発生しているかチェック
        /// </summary>
        /// <param name="condition">取得条件</param>
        /// <returns>エラーの場合True</returns>
        private bool isAppearInoutData(object condition)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Common.GetNewDataOverInoutDate, out string outSql);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.CommonWith, out string outWithSql);

            // 件数を取得
            int cnt = db.GetEntityByDataClass<int>(outWithSql + outSql, condition);
            if (cnt > 0)
            {
                // 「移庫後に移庫先の受払情報が既に登録されている為、修正・取消はできません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141020001 });
                return true;
            }

            return false;
        }
        #endregion

    }
}