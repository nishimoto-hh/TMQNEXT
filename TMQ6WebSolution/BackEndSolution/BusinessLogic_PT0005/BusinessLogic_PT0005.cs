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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Consts = CommonTMQUtil.CommonTMQConstants;
using Dao = BusinessLogic_PT0005.BusinessLogicDataClass_PT0005;
using InventryCheck = CommonTMQUtil.CommonTMQUtil.PartsInventory.InventryGetInfo.SqlName;
using STDDao = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_PT0005
{
    /// <summary>
    /// 入庫入力画面
    /// </summary>
    public class BusinessLogic_PT0005 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数
        /// <summary>
        /// フォーム種類
        /// </summary>
        private static class FormType
        {
            /// <summary>入庫入力画面</summary>
            public const byte List = 0;
        }

        /// <summary>
        /// 起動モード
        /// </summary>
        private static class FormMode
        {
            /// <summary>新規</summary>
            public const string New = "0";
            /// <summary>編集</summary>
            public const string PT0001Edit = "1";
            /// <summary>編集</summary>
            public const string Edit = "2";
            /// <summary>参照</summary>
            public const string Reference = "3";
        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            /// <summary>
            /// 呼出元の予備品一覧の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string InventryConditionInfo = "BODY_020_00_LST_0";
            /// <summary>
            /// 呼出元の入出力一覧の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string InoutConditionInfo = "BODY_030_00_LST_0";
            /// <summary>
            /// 呼出元の棚卸入出庫履歴一覧の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string InventryHistoryConditionInfo = "BODY_020_00_LST_1";
            /// <summary>
            /// 入庫入力 予備品情報
            /// </summary>
            public const string SparePartsInfo = "CBODY_000_00_LST_5";
            /// <summary>
            /// 入庫入力 入庫情報 グループ番号
            /// </summary>
            public const short GroupNo = 501;
            /// <summary>
            /// 入庫入力 入庫情報(入庫日・予備品倉庫)
            /// </summary>
            public const string StorageInfo1 = "CBODY_010_00_LST_5";
            /// <summary>
            /// 入庫入力 入庫情報(棚番)
            /// </summary>
            public const string StorageInfo2 = "CBODY_030_00_LST_5";
            /// <summary>
            /// 入庫入力 入庫情報(新旧区分～入庫金額)
            /// </summary>
            public const string StorageInfo3 = "CBODY_040_00_LST_5";
            /// <summary>
            /// 入庫入力 入庫情報(棚枝番)
            /// </summary>
            public const string StorageInfo4 = "CBODY_050_00_LST_5";
            /// <summary>
            /// 入庫入力 入庫情報(結合文字列)
            /// </summary>
            public const string StorageInfo5 = "CBODY_060_00_LST_5";
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL名：入庫入力(予備品情報)</summary>
            public const string GetStorageSpareInfo = "GetStorageSpareInfo";
            /// <summary>SQL名：入庫入力(入庫情報)：新規</summary>
            public const string GetStorageInfoNew = "GetStorageInfoNew";
            /// <summary>SQL名：入庫入力(入庫情報)：編集</summary>
            public const string GetStorageInfoEdit = "GetStorageInfoEdit";
            /// <summary>SQL名：新旧区分の初期表示値を取得するSQL/summary>
            public const string GetInitOldNewStructureId = "GetInitOldNewStructureId";
            /// <summary>SQL名：部門の初期表示値を取得するSQL/summary>
            public const string GetInitDepartmentStructureId = "GetInitDepartmentStructureId";
            /// <summary>SQL名：勘定科目の初期表示値を取得するSQL/summary>
            public const string GetInitAccountStructureId = "GetInitAccountStructureId";
            /// <summary>SQL名：受払履歴IDよりロット情報を取得するSQL/summary>
            public const string GetLotInfoByInoutHistoryId = "GetLotInfoByInoutHistoryId";
            /// <summary>SQL名：受払日時以降の受払件数を取得するSQL/summary>
            public const string SelectPreIssueDayCount = "Select_preIssueDayCount";
            /// <summary>SQL名：倉庫IDより、工場IDを取得するSQL/summary>
            public const string GetFactoryIdByWarehouseId = "GetFactoryIdByWarehouseId";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDirIssue = @"IssueInput";
            public const string SubDirInventry = @"InventoryControl";
        }

        /// <summary>
        /// 遷移元機能ID
        /// </summary>
        private static class TransConductId
        {
            /// <summary>PT0001</summary>
            public const string PT0001 = "PT0001";
            /// <summary>PT0002</summary>
            public const string PT0002 = "PT0002";
            /// <summary>PT0003</summary>
            public const string PT0003 = "PT0003";
        }

        /// <summary>
        /// 拡張データに持っている取得条件
        /// </summary>
        public static class condPartsType
        {
            /// <summary>
            /// 新旧区分
            /// </summary>
            public static class oldNewDivition
            {
                /// <summary>構成グループID</summary>
                public const short StructureGroupId = 1940;
                /// <summary>連番</summary>
                public const short Seq = 1;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_PT0005() : base()
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
            if (compareId.IsBack())
            {
                // 戻る場合
                return InitSearch();
            }

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

            switch (this.FormNo)
            {
                case FormType.List:     // 一覧検索
                    if (!searchList())
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
            else if (compareId.IsStartId("CANCEL"))
            {
                // 取消の場合
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

            // 登録ボタンが複数の画面に無い場合、分岐は不要
            // 処理を実行する画面Noの値により処理を分岐する
            switch (this.FormNo)
            {
                case FormType.List:
                    // 編集画面の場合の登録処理
                    resultRegist = executeRegistEdit();
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
            // 排他チェック
            if (isErrorExclusive())
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 取消処理結果によりエラー処理を行う
            if (!registDb(false))
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 未設定時にエラーメッセージを設定
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「取消処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200010 });
                }
                return ComConsts.RETURN_RESULT.NG;
            }
            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「取消処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200010 });

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // 画面モード
            string formMode = FormMode.New;

            // 遷移元のマッピング情報追加
            AddMappingListOtherPgmId(TransConductId.PT0001);
            AddMappingListOtherPgmId(TransConductId.PT0002);
            AddMappingListOtherPgmId(TransConductId.PT0003);

            // 検索条件
            Dao.searchCondition conditionObj = new Dao.searchCondition();

            // 遷移元機能ID取得
            var transConductId = transitionSource(out Dictionary<string, object> targetDic);

            // PT0001より遷移
            if (transConductId == TransConductId.PT0001)
            {
                // 予備品ID、制御用フラグの取得
                SetDataClassFromDictionary(targetDic, TargetCtrlId.InventryConditionInfo, conditionObj, new List<string> { "PartsId", "ControlFlag" });

                // 予備品詳細画面からの呼び出しの場合
                if (conditionObj.ControlFlag == FormMode.PT0001Edit)
                {
                    // 受払履歴ID
                    conditionObj.InoutHistoryId = long.Parse(getDictionaryKeyValue(targetDic, "inout_history_id"));
                    // 編集モード
                    formMode = FormMode.PT0001Edit;
                }
            }
            // PT0002より遷移
            else if (transConductId == TransConductId.PT0002)
            {
                // 予備品ID取得
                conditionObj.PartsId = long.Parse(getDictionaryKeyValue(targetDic, "parts_id"));
                // 受払履歴ID
                conditionObj.InoutHistoryId = long.Parse(getDictionaryKeyValue(targetDic, "inout_history_id"));
                // 編集モード
                formMode = FormMode.PT0001Edit;
            }
            // PT0003より遷移
            else if (transConductId == TransConductId.PT0003)
            {
                // 予備品ID取得
                conditionObj.PartsId = long.Parse(getDictionaryKeyValue(targetDic, "parts_id"));
                // 受払履歴ID取得
                conditionObj.InoutHistoryId = long.Parse(getDictionaryKeyValue(targetDic, "inout_history_id"));
                // 編集モード
                formMode = FormMode.PT0001Edit;
            }
            // 到達不能のためエラーを返す
            else
            {
                return false;
            }

            // 言語ID設定
            conditionObj.LanguageId = this.LanguageId;

            // 予備品情報検索
            if (!searchResultSpareInfo(ref conditionObj))
            {
                return false;
            }

            // 入庫情報検索
            Dao.searchResultStorageInfo storageInfo = new();
            if (!searchResultStorageInfo(formMode, ref conditionObj, out storageInfo))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// どの画面から遷移するかを返すメソッド
        /// </summary>
        /// <returns>遷移元機能ID</returns>
        private string transitionSource(out Dictionary<string, object> targetDic)
        {
            // 初期化
            targetDic = null;

            // PT0001のディクショナリを取得
            var targetDicPT0001 = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.InventryConditionInfo);
            if (targetDicPT0001 != null)
            {
                targetDic = targetDicPT0001;
                return TransConductId.PT0001;
            }
            // PT0002のディクショナリを取得
            var targetDicPT0002 = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.InoutConditionInfo);
            if (targetDicPT0002 != null)
            {
                targetDic = targetDicPT0002;
                return TransConductId.PT0002;
            }
            // PT0003のディクショナリを取得
            var targetDicPT0003 = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.InventryHistoryConditionInfo);
            if (targetDicPT0003 != null)
            {
                targetDic = targetDicPT0003;
                return TransConductId.PT0003;
            }
            // 到達不能なので空
            return string.Empty;
        }

        /// <summary>
        /// 予備品情報検索
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <returns>正常：true、異常：false</returns>
        private bool searchResultSpareInfo(ref Dao.searchCondition conditionObj)
        {
            var result = new Dao.searchResultSpareInfo();

            // 予備品情報取得
            result = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultSpareInfo>(SqlName.GetStorageSpareInfo, SqlName.SubDirInventry, conditionObj, this.db);
            if (result == null)
            {
                return false;
            }

            // 標準棚(表示用)
            result.PartsLocationDisp = TMQUtil.GetDisplayPartsLocation(result.PartsLocationId, result.PartsLocationDetailNo, result.FactoryId, this.LanguageId, this.db);
            // 在庫数(表示用)
            result.StockQuantityDisp = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.StockQuantity.ToString(), result.UnitDigit, result.RoundDivision), result.UnitName, true);

            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.SparePartsInfo, this.pageInfoList);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResultSpareInfo>(pageInfo, new List<Dao.searchResultSpareInfo> { result }, 1))
            {
                // 正常終了
                return true;
            }

            return false;
        }

        /// <summary>
        /// 入庫情報検索
        /// </summary>
        /// <param name="formMode">画面モード</param>
        /// <param name="conditionObj">検索条件</param>
        /// <returns>正常：true、異常：false</returns>
        private bool searchResultStorageInfo(string formMode, ref Dao.searchCondition conditionObj, out Dao.searchResultStorageInfo result)
        {
            // 新規の場合
            if (formMode == FormMode.New)
            {
                // 入庫情報取得
                result = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultStorageInfo>(SqlName.GetStorageInfoNew, SqlName.SubDirInventry, conditionObj, this.db);
                if (result == null)
                {
                    return false;
                }

                // 入庫日
                result.InoutDatetime = DateTime.Today;
                // 入庫単価(表示用)
                result.UnitPriceDisp = TMQUtil.roundDigit(result.UnitPrice.ToString(), result.CurrencyDigit, result.RoundDivision);
                // 画面タイプをセット(ボタン制御用)
                result.FormType = int.Parse(formMode);

                // 条件追加
                conditionObj.FactoryId = Convert.ToInt32(result.FactoryId);  // 工場ID
                conditionObj.LanguageId = this.LanguageId;                   // 言語ID
                conditionObj.FactoryIdList = Consts.CommonFactoryId.ToString() + ',' + result.FactoryId.ToString();     // 工場IDリスト

                // 新旧区分の初期表示値取得
                var initOldNewDivisionInfo = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultStorageInfo>(SqlName.GetInitOldNewStructureId, SqlName.SubDirInventry, conditionObj, this.db);
                if (initOldNewDivisionInfo != null)
                {
                    // 新旧区分(0:新品)
                    result.OldNewStructureId = initOldNewDivisionInfo.OldNewStructureId;
                }

                // 部門の初期表示値取得
                List<string> conditionListForDepartment = new() { getFactoryIdByWarehouseId(result.PartsStorageLocationId) + "," + Consts.CommonFactoryId.ToString() };
                // 条件を取得(倉庫IDから取得した工場ID、共通工場ID「0」)
                string getFactoryIdByWarehouseId(long warehouseId)
                {
                    // SQLを取得
                    TMQUtil.GetFixedSqlStatement(SqlName.SubDirInventry, SqlName.GetFactoryIdByWarehouseId, out string sql);
                    // SQL実行
                    IList<ComDao.MsStructureEntity> results = this.db.GetListByDataClass<ComDao.MsStructureEntity>(sql, new { WarehouseId = warehouseId });
                    if (results == null || results.Count == 0)
                    {
                        // 取得できない場合は共通工場ID「0」
                        return Consts.CommonFactoryId.ToString();
                    }

                    // 取得した工場ID
                    return results[0].FactoryId.ToString();
                }
                var initDepartmentInfo = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultStorageInfo>(SqlName.GetInitDepartmentStructureId,
                                                                                                           SqlName.SubDirInventry,
                                                                                                           new { FactoryIdList = conditionListForDepartment, LanguageId = this.LanguageId },
                                                                                                           this.db);
                if (initDepartmentInfo != null)
                {
                    // 部門(工場表示順の最上位)
                    result.DepartmentStructureId = initDepartmentInfo.DepartmentStructureId;
                    result.DepartmentCd = initDepartmentInfo.DepartmentCd;
                }

                // 勘定科目の初期表示値取得
                var initAccoutInfo = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultStorageInfo>(SqlName.GetInitAccountStructureId, SqlName.SubDirInventry, conditionObj, this.db);
                if (initAccoutInfo != null)
                {
                    // 勘定科目(B4140:設備貯蔵品)
                    result.AccountStructureId = initAccoutInfo.AccountStructureId;
                    result.AccountCd = initAccoutInfo.AccountCd;
                    result.AccountOldNewDivision = initAccoutInfo.AccountOldNewDivision;
                }

                // 入庫する予備品データを取得
                ComDao.PtPartsEntity partsInfo = new ComDao.PtPartsEntity().GetEntity(result.PartsId, this.db);
                // 取得した予備品の棚番がNULLの場合は検索結果に「-1」を設定する
                if (partsInfo.LocationRackStructureId == null)
                {
                    result.PartsLocationId = -1;
                }
            }
            // 編集の場合
            else
            {
                // 入庫情報取得
                result = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultStorageInfo>(SqlName.GetStorageInfoEdit, SqlName.SubDirInventry, conditionObj, this.db);
                if (result == null)
                {
                    return false;
                }

                // 入庫数(表示用)
                result.StorageQuantityDisp = TMQUtil.roundDigit(result.StorageQuantity.ToString(), result.UnitDigit, result.RoundDivision);
                // 入庫単価(表示用)
                result.UnitPriceDisp = TMQUtil.roundDigit(result.UnitPrice.ToString(), result.CurrencyDigit, result.RoundDivision);
                // 画面タイプをセット(ボタン制御用)
                result.FormType = int.Parse(FormMode.Edit);

                // 受払履歴IDよりロット情報取得
                var lotInfo = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchCondition>(SqlName.GetLotInfoByInoutHistoryId, SqlName.SubDirInventry, conditionObj, this.db);
                if (lotInfo == null)
                {
                    return false;
                }

                // 条件追加
                conditionObj.LotControlId = lotInfo.LotControlId;                    // ロット番号
                conditionObj.OldNewStructureId = lotInfo.OldNewStructureId;          // 新旧区分
                conditionObj.DepartmentStructureId = lotInfo.DepartmentStructureId;　// 部門
                conditionObj.AccountStructureId = lotInfo.AccountStructureId;　　　　// 勘定科目
                conditionObj.LanguageId = this.LanguageId;                           // 言語ID
                conditionObj.PartsLocationId = result.PartsLocationId;               // 棚ID
                conditionObj.PartsLocationDetailNo = result.PartsLocationDetailNo;   // 棚枝番

                // エラーチェック
                bool isError = false;

                // 在庫確定日取得
                DateTime stockDatetime = TMQUtil.PartsGetInfo.GetInventoryConfirmationDate(conditionObj, this.db);
                // 棚卸確定日取得
                DateTime fixedDatetime = TMQUtil.PartsGetInfo.GetTakeInventoryConfirmationDate(conditionObj, this.db);

                // 入庫日が在庫確定日以前の場合
                if (result.InoutDatetime <= stockDatetime)
                {
                    // 「在庫確定以前の情報のため、修正・取消することはできません。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141110002 });
                    isError = true;
                }

                // 入庫日が棚卸確定日以前の場合
                if (!isError && result.InoutDatetime <= fixedDatetime)
                {
                    // 「棚卸確定以前の情報のため、修正・取消することはできません。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160005 });
                    isError = true;
                }

                // 受払実績の存在チェック
                if (!isError && isCheckInoutData(conditionObj))
                {
                    // 「新しい受払履歴実績が存在します。修正・取消することはできません。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141010001 });
                    isError = true;
                }

                // 棚卸中データ存在チェック
                if (!isError && InventryCheck.IsExistsInventryData(conditionObj, this.db))
                {
                    // 「棚卸中データのため、修正・削除はできません。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160010 });
                    isError = true;
                }

                // エラーがある場合
                if (isError)
                {
                    //// 警告表示
                    //this.Status = CommonProcReturn.ProcStatus.Warning;
                    // 画面タイプをセット(ボタン制御用)
                    result.FormType = int.Parse(FormMode.Reference);
                }
            }

            // 棚番結合文字列取得
            result.JoinString = TMQUtil.GetJoinStrOfPartsLocation(TMQUtil.GetUserFactoryId(this.UserId, this.db), this.LanguageId, this.db);

            // 入庫情報に検索結果を設定する
            List<string> ctrlIdList = getResultMappingInfoByGrpNo(TargetCtrlId.GroupNo).CtrlIdList;
            foreach (var ctrlId in ctrlIdList)
            {
                // ページ情報取得
                var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);
                // 検索結果の設定
                if (!SetSearchResultsByDataClass<Dao.searchResultStorageInfo>(pageInfo, new List<Dao.searchResultStorageInfo> { result }, 1))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }

            return true;

            /// <summary>
            /// 受払実績の存在チェック
            /// </summary>
            /// <param name="conditionObj">検索条件</param>
            /// <returns>true:エラーあり、false:エラーなし</returns>
            bool isCheckInoutData(Dao.searchCondition conditionObj)
            {
                // 受払履歴情報取得
                var inoutHistoryEntity = new ComDao.PtInoutHistoryEntity().GetEntity(conditionObj.InoutHistoryId, this.db);

                // 受払日時
                conditionObj.InoutDatetime = inoutHistoryEntity.InoutDatetime;
                // 更新日時
                conditionObj.UpdateDatetime = inoutHistoryEntity.UpdateDatetime;

                // 受払日時以降の受払件数を取得する

                // 件数取得
                int cnt = TMQUtil.SqlExecuteClass.SelectEntity<int>(SqlName.SelectPreIssueDayCount, SqlName.SubDirIssue, conditionObj, this.db);
                if (cnt > 0)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// アイテム拡張マスタから拡張データを取得する
        /// </summary>>
        /// <param name="structureGroupId">構成グループID</param>
        /// <param name="seq">連番</param
        /// <param name="structureId">構成ID</param>
        /// <returns>拡張データ</returns>
        private string getItemExData(short structureGroupId, short seq, int structureId)
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

            // 登録
            if (!registDb(true))
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
            var ctrlId = TargetCtrlId.StorageInfo3;
            // エラーチェックを行う画面の項目を取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
            // 画面タイプ取得
            var formType = getDictionaryKeyValue(targetDic, "form_type");

            // 修正登録の場合、排他チェックを行う
            if (formType == FormMode.Edit)
            {
                // 排他ロック用マッピング情報取得
                var lockValMaps = GetLockValMaps(ctrlId);
                var lockKeyMaps = GetLockKeyMaps(ctrlId);

                // 単一の場合の排他チェック
                if (!checkExclusiveSingle(ctrlId))
                {
                    // エラーの場合
                    return true;
                }
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
            var mappingInfo = getResultMappingInfo(ctrlId);
            // コントロールIDにより画面の項目(一覧)を取得
            List<Dictionary<string, object>> resultList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);
            // 戻り値となるデータクラスのリスト
            List<T> registInfoList = new();
            // 一覧を繰り返し、データクラスに変換、リストへ追加する
            foreach (var resultRow in resultList)
            {
                T registInfo = new();
                if (!SetExecuteConditionByDataClass<T>(resultRow, ctrlId, registInfo, now, this.UserId, this.UserId))
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

            // 単一の項目の場合
            if (isErrorRegistForSingle(ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            bool isErrorRegistForSingle(ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                bool isError = false;   // 処理全体でエラーの有無を保持

                // 入庫情報取得
                Dao.searchResultStorageInfo storageInfo = getRegistInfo<Dao.searchResultStorageInfo>(TargetCtrlId.GroupNo, DateTime.Now);

                // 入庫情報に対して入力チェックを行う
                List<string> ctrlIdList = getResultMappingInfoByGrpNo(TargetCtrlId.GroupNo).CtrlIdList;
                foreach (var ctrlId in ctrlIdList)
                {
                    // 入力チェック
                    checkStorageInfoInput(ctrlId, storageInfo, ref errorInfoDictionary);
                }

                return isError;

                void checkStorageInfoInput(string targetCtrlId, Dao.searchResultStorageInfo storageInfo, ref List<Dictionary<string, object>> errorInfoDictionary)
                {
                    // エラー情報を画面に設定するためのマッピング情報リスト
                    var info = getResultMappingInfo(targetCtrlId);

                    // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                    // 単一の内容を取得
                    var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, targetCtrlId);

                    // Dictionaryをデータクラスに変換
                    Dao.searchResultStorageInfo result = new();
                    SetDataClassFromDictionary(targetDic, targetCtrlId, result);

                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                    string errMsg;
                    string val;

                    if (targetCtrlId == TargetCtrlId.StorageInfo1)
                    {
                        // 入庫情報(入庫日・予備品倉庫)

                        // 在庫確定日取得
                        DateTime stockDatetime = TMQUtil.PartsGetInfo.GetInventoryConfirmationDate(storageInfo, this.db);
                        // 棚卸確定日取得
                        DateTime fixedDatetime = TMQUtil.PartsGetInfo.GetTakeInventoryConfirmationDate(storageInfo, this.db);

                        // 入庫日が未来日の場合
                        if (result.InoutDatetime > DateTime.Today)
                        {
                            // 「未来の日付は入力できません。」
                            errMsg = GetResMessage(new string[] { ComRes.ID.ID111320002 });
                            val = info.getValName("inout_datetime");
                            errorInfo.setError(errMsg, val);                // エラー情報をセット
                            errorInfoDictionary.Add(errorInfo.Result);      // エラー情報を追加
                            isError = true;
                        }

                        // 入庫日が在庫確定日前の場合
                        if (result.InoutDatetime <= stockDatetime)
                        {
                            // 日付を変換
                            string strStockDatetime = stockDatetime.ToString(GetResMessage(new string[] { ComRes.ID.ID150000003 }));

                            // 「在庫確定前[{0}]の日付は入力できません。」
                            errMsg = GetResMessage(new string[] { ComRes.ID.ID141110001, strStockDatetime });
                            val = info.getValName("inout_datetime");
                            errorInfo.setError(errMsg, val);           // エラー情報をセット
                            errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                            isError = true;
                        }

                        // 入庫日が棚卸確定日前の場合
                        if (result.InoutDatetime <= fixedDatetime)
                        {
                            // 日付を変換
                            string strFixedDatetime = fixedDatetime.ToString(GetResMessage(new string[] { ComRes.ID.ID150000003 }));

                            // 「棚卸確定前[{0}]の日付は入力できません。」
                            errMsg = GetResMessage(new string[] { ComRes.ID.ID141160004, strFixedDatetime });
                            val = info.getValName("inout_datetime");
                            errorInfo.setError(errMsg, val);            // エラー情報をセット
                            errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                            isError = true;
                        }
                    }
                    else if (targetCtrlId == TargetCtrlId.StorageInfo2)
                    {
                        // 入庫情報(棚)

                        // 倉庫と棚の関連チェック
                        if (checkPartsLocation(result))
                        {
                            // 「予備品倉庫に紐付く棚を選択してください。」
                            errMsg = GetResMessage(new string[] { ComRes.ID.ID141380001 });
                            val = info.getValName("parts_location");
                            errorInfo.setError(errMsg, val);            // エラー情報をセット
                            errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                            isError = true;
                        }

                        bool checkPartsLocation(Dao.searchResultStorageInfo condition)
                        {
                            // 棚IDの構成マスタ情報取得
                            var structureInfo = new ComDao.MsStructureEntity().GetEntity(Convert.ToInt32(condition.PartsLocationId), this.db);
                            if (structureInfo == null)
                            {
                                return true;
                            }

                            // 倉庫と棚に関連がない場合、エラー
                            if (structureInfo.ParentStructureId != condition.PartsStorageLocationId)
                            {
                                return true;
                            }

                            return false;
                        }
                    }
                    else if (targetCtrlId == TargetCtrlId.StorageInfo4)
                    {
                        // 入庫情報(棚枝番)

                        // 棚枝番の半角記号チェック
                        if (!string.IsNullOrEmpty(result.PartsLocationDetailNo))
                        {
                            var enc = Encoding.GetEncoding("Shift_JIS");
                            if (enc.GetByteCount(result.PartsLocationDetailNo) != result.PartsLocationDetailNo.Length || !ComUtil.IsAlphaNumeric(result.PartsLocationDetailNo))
                            {
                                // 「半角英数字で入力してください。」
                                errMsg = GetResMessage(new string[] { ComRes.ID.ID141260002 });
                                val = info.getValName("parts_location_detail_no");
                                errorInfo.setError(errMsg, val);            // エラー情報をセット
                                errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                                isError = true;
                            }
                        }
                    }
                    else if (targetCtrlId == TargetCtrlId.StorageInfo3)
                    {
                        // 入庫情報(新旧区分～入庫金額)

                        // 入庫数
                        var storageQuantity = ConvertDecimal(result.StorageQuantityDisp);
                        // 入庫数の上下限チェック
                        if (storageQuantity <= 0)
                        {
                            // 「入庫数は0以下で登録できません。」
                            errMsg = GetResMessage(new string[] { ComRes.ID.ID141060007, ComRes.ID.ID111220007 });
                            val = info.getValName("storage_quantity");
                            errorInfo.setError(errMsg, val);            // エラー情報をセット
                            errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                            isError = true;
                        }
                        else if (storageQuantity > 9999999999.99m)
                        {
                            // 「入庫数は整数部10桁以下で入力してください。」
                            errMsg = GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111220007, ComRes.ID.ID911140003, "10" });
                            val = info.getValName("storage_quantity");
                            errorInfo.setError(errMsg, val);            // エラー情報をセット
                            errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                            isError = true;
                        }

                        // 入庫単価
                        var unitPrice = ConvertDecimal(result.UnitPriceDisp);
                        // 入庫単価の上下限チェック
                        if (unitPrice <= 0)
                        {
                            // 「入庫単価は0以下で登録できません。」
                            errMsg = GetResMessage(new string[] { ComRes.ID.ID141060007, ComRes.ID.ID111220008 });
                            val = info.getValName("unit_price");
                            errorInfo.setError(errMsg, val);            // エラー情報をセット
                            errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                            isError = true;
                        }
                        else if (unitPrice > 9999999999.99m)
                        {
                            // 「入庫単価は整数部10桁以下で入力してください。」
                            errMsg = GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111220008, ComRes.ID.ID911140003, "10" });
                            val = info.getValName("unit_price");
                            errorInfo.setError(errMsg, val);            // エラー情報をセット
                            errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                            isError = true;
                        }

                        // 新旧区分と勘定項目の関連チェック
                        if (checkAccountStructure(result))
                        {
                            // 「新旧区分と勘定科目の正しい組み合わせを入力してください。」
                            errMsg = GetResMessage(new string[] { ComRes.ID.ID141120005 });
                            val = info.getValName("account_structure");
                            errorInfo.setError(errMsg, val);            // エラー情報をセット
                            errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                            isError = true;
                        }

                        bool checkAccountStructure(Dao.searchResultStorageInfo condition)
                        {
                            // 新旧区分IDよりコード値を取得
                            var oldNewStructureCd = getItemExData(condPartsType.oldNewDivition.StructureGroupId, condPartsType.oldNewDivition.Seq, Convert.ToInt32(result.OldNewStructureId));

                            // 新旧区分と勘定科目の組み合わせが不正の場合、エラー
                            if (oldNewStructureCd != condition.AccountOldNewDivision)
                            {
                                return true;
                            }

                            return false;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 対象項目をdecimal型に変換する
        /// </summary>
        /// <param name="item">対象項目</param>
        /// <returns>エラーの場合False</returns>
        private decimal ConvertDecimal(string item)
        {
            // @単位の形で渡ってくるため数値、小数点、符号のみを抽出
            string result = Regex.Replace(item, @"[^-0-9.]", "");
            // 先頭の文字が「.」であれば0を先頭に追加
            if (result.FirstOrDefault() == '.')
            {
                result = "0" + result;
            }

            return decimal.Parse(result);
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="isRegist">true:登録処理、false:取消処理</param>
        /// <returns>エラーの場合False</returns>
        private bool registDb(bool isRegist)
        {
            // 入庫情報取得
            Dao.searchResultStorageInfo storageInfo = getRegistInfo<Dao.searchResultStorageInfo>(TargetCtrlId.GroupNo, DateTime.Now);

            // クラスの宣言
            TMQUtil.PartsInventory.Input inventory = new(this.db, this.UserId, this.LanguageId);

            // 対応する引数
            TMQDao.PartsInventory.Input condInp = getRegistInfo<TMQDao.PartsInventory.Input>(TargetCtrlId.GroupNo, DateTime.Now);
            setRegistInfo(storageInfo, ref condInp);

            // 棚枝番に入力がない場合はnullではなく空文字にする
            condInp.PartsLocationDetailNo = ConvertNullToStringEmpty(condInp.PartsLocationDetailNo);

            // DB登録
            if (isRegist)
            {
                // 登録処理

                if (storageInfo.FormType == int.Parse(FormMode.New))
                {
                    // 新規登録
                    if (!inventory.New(condInp, out long workNo))
                    {
                        return false;
                    }

                }
                else if (storageInfo.FormType == int.Parse(FormMode.Edit))
                {
                    // 修正登録
                    if (!inventory.Update(condInp, (long)storageInfo.WorkNo, out long newWorkNo))
                    {
                        return false;
                    }
                }
            }
            else
            {
                // 取消処理
                if (!inventory.Cancel((long)storageInfo.WorkNo))
                {
                    return false;
                }
            }

            return true;

            void setRegistInfo(Dao.searchResultStorageInfo result, ref TMQDao.PartsInventory.Input condInp)
            {
                // 登録できなかった項目をセット
                condInp.ReceivingDatetime = result.InoutDatetime;
                condInp.InoutQuantity = ConvertDecimal(result.StorageQuantityDisp);
                condInp.UnitPrice = ConvertDecimal(result.UnitPriceDisp);
            }
        }

        #endregion
    }
}