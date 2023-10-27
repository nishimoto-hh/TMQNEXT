using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using static CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_PT0006.BusinessLogicDataClass_PT0006;
using StructureGroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using PT0001Condition = BusinessLogic_PT0001.BusinessLogicDataClass_PT0001.detailSearchCondition;
using InventryCheck = CommonTMQUtil.CommonTMQUtil.PartsInventory.InventryGetInfo.SqlName;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_PT0006
{
    /// <summary>
    /// 出庫入力画面
    /// </summary>
    public class BusinessLogic_PT0006 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数
        /// <summary>
        /// フォーム種類
        /// </summary>
        private static class FormType
        {
            /// <summary>出庫入力画面</summary>
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
        /// 処理対象グループ番号
        /// </summary>
        private static class TargetGrpNo
        {
            /// <summary>在庫一覧</summary>
            public const short InventryList = 1;
        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            /// <summary>
            /// 元の予備品一覧(PT0001)の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string InventryConditionInfo = "BODY_020_00_LST_0";
            /// <summary>
            /// 元の出力一覧(PT0002)(親情報)の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string ConditionInfo = "BODY_050_00_LST_0";
            /// <summary>
            /// <summary>
            /// 元の入出庫履歴一覧(PT0003)の画面項目定義テーブルのコントロールID
            /// </summary>
            public const string InventryHistoryConditionInfo = "BODY_020_00_LST_1";
            /// <summary>
            /// 出庫入力 予備品情報
            /// </summary>
            public const string SparePartsInformation = "CBODY_000_00_LST_6";
            /// <summary>
            /// 出庫入力 部門在庫情報
            /// </summary>
            public const string DepartmentInformation = "CBODY_010_00_LST_6";
            /// <summary>
            /// 出庫入力 出庫情報入力
            /// </summary>
            public const string IssueInformation = "CBODY_020_00_LST_6";
            /// <summary>
            /// 出庫入力 在庫一覧
            /// </summary>
            public const string InventoryInformation = "CBODY_040_00_LST_6";
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL名：出庫一覧詳細(予備品情報)/summary>
            public const string GetIssueSparesList = "GetIssueSparesList";
            /// <summary>SQL名：出庫一覧詳細(部門在庫情報)：新規/summary>
            public const string GetIssueDepartmentList = "GetIssueDepartmentList";
            /// <summary>SQL名：出庫一覧詳細(部門在庫情報)：編集/summary>
            public const string GetIssueDepartmentListEdit = "GetIssueDepartmentListEdit";
            /// <summary>SQL名：受払履歴IDより新旧区分、部門、勘定科目を取得するSQL/summary>
            public const string SelectWhereByInoutHistoryId = "Select_where_by_inout_history_id";
            /// <summary>SQL名：受払履歴IDより同一の作業Noを持つ受払履歴IDを取得するSQL/summary>
            public const string SelectInoutHistoryIdByInoutHistoryId = "Select_inout_history_id_by_inout_history_id";
            /// <summary>SQL名：作業Noより受払履歴IDを取得するSQL/summary>
            public const string SelectInoutHistoryIdByWorkNo = "Select_inout_history_id_by_workno";
            /// <summary>SQL名：出庫一覧詳細(出庫情報入力)：新規/summary>
            public const string GetIssueInput = "GetIssueInput";
            /// <summary>SQL名：出庫一覧詳細(出庫情報入力)：編集/summary>
            public const string GetIssueInputEdit = "GetIssueInputEdit";
            /// <summary>SQL名：出庫一覧詳細(在庫一覧)：新規/summary>
            public const string GetIssueInventoryList = "GetIssueInventoryList";
            /// <summary>SQL名：出庫一覧詳細(在庫一覧)：編集/summary>
            public const string GetIssueInventoryListEdit = "GetIssueInventoryListEdit";
            /// <summary>SQL名：受払履歴IDよりロット管理ID、更新日時、対象年月を取得するSQL/summary>
            public const string SelectInfoByInoutHistoryId = "Select_info_by_inout_history_id";
            /// <summary>SQL名：受払日時以降に受払いが発生しているか見るSQL/summary>
            public const string SelectPreIssueDayCount = "Select_preIssueDayCount";
            /// <summary>SQL名：予備品IDより、棚ID、棚枝番を取得するSQL/summary>
            public const string SelectPartsLocationByIdList = "Select_parts_location_by_parts_id";
            /// <summary>SQL名：新旧区分、部門、勘定科目より棚ID、棚枝番を取得するSQL/summary>
            public const string SelectLocationInfo = "Select_location_info";

            /// <summary>SQL名：予備品情報取得/summary>
            public const string GetDetailPartsInfo = "GetDetailPartsInfo";
            /// <summary>SQL名：予備品情報取得/summary>
            /// <summary>SQL名：一覧取得</summary>
            public const string GetPartsList = "GetPartsList";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDirIssue = @"IssueInput";
            /// <summary>SQL格納先サブディレクトリ名(予備品仕様)</summary>
            public const string SubDirParts = @"Parts";
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

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_PT0006() : base()
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
            // 検索処理実行
            if (!initFormList())
            {
                // エラー終了
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
            this.ResultList = new();

            List<Dao.searchResultInventory> result = new();
            // 在庫一覧検索
            if (!searchInventoryList(FormMode.New, out result))
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                //メッセージが設定されていない場合5
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「出庫引当に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID111120077 });
                }
                return ComConsts.RETURN_RESULT.NG;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
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
                // 削除の場合
                // 削除処理実行
                return Delete();
            }
            else
            {
                // この部分は到達不能なので、エラーを返す
                return ComConsts.RETURN_RESULT.NG;
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
            // 排他チェック
            if (isErrorExclusive())
            {
                // 「他のユーザが変更しています。再度選択してください。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141300007 });
                return ComConsts.RETURN_RESULT.NG;
            }

            // 在庫一覧
            List<Dao.searchResultInventory> result = new();

            // 作業No取得
            var inventryList = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.IssueInformation);

            // 画面情報取得
            DateTime now = DateTime.Now;
            // ここでは登録する内容を取得するので、テーブルのデータクラスを指定
            Dao.searchResultInventory registInfo = getRegistInfo<Dao.searchResultInventory>(TargetGrpNo.InventryList, 0, now);

            // クラスの宣言
            TMQUtil.PartsInventory.Output output = new(this.db, this.UserId, this.LanguageId);

            // 更新登録
            if (!output.Cancel(long.Parse(getDictionaryKeyValue(inventryList, "work_no"))))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region 検索処理
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initFormList()
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
                // 予備品IDの取得
                SetDataClassFromDictionary(targetDic, TargetCtrlId.InventryConditionInfo, conditionObj, new List<string> { "PartsId", "ControlFlag" });

                // 予備品詳細画面からの呼び出しの場合
                if (conditionObj.ControlFlag == FormMode.PT0001Edit)
                {
                    conditionObj.IdList = getDictionaryKeyValue(targetDic, "inout_history_id");
                    // 編集モード
                    formMode = FormMode.PT0001Edit;
                }
            }
            // PT0002より遷移
            else if (transConductId == TransConductId.PT0002)
            {
                // 予備品ID、作業Noの取得
                SetDataClassFromDictionary(targetDic, TargetCtrlId.ConditionInfo, conditionObj, new List<string> { "PartsId", "WorkNo" });

                // 受払履歴IDをカンマ区切りで代入
                conditionObj.IdList = returnIdList(SqlName.SelectInoutHistoryIdByWorkNo, conditionObj);
                // 編集モード
                formMode = FormMode.Edit;
            }
            // PT0003より遷移
            else if (transConductId == TransConductId.PT0003)
            {
                // 予備品ID、入出庫No(作業No)の取得
                SetDataClassFromDictionary(targetDic, TargetCtrlId.InventryHistoryConditionInfo, conditionObj, new List<string> { "PartsId", "InoutNo" });
                conditionObj.WorkNo = conditionObj.InoutNo;

                // 受払履歴IDをカンマ区切りで代入
                conditionObj.IdList = returnIdList(SqlName.SelectInoutHistoryIdByWorkNo, conditionObj);
                // 編集モード
                formMode = FormMode.Edit;
            }
            // 到達不能のためエラーを返す
            else
            {
                return false;
            }

            // 言語ID設定
            conditionObj.LanguageId = this.LanguageId;

            // 予備品情報検索
            if (!searchResultSpares(ref conditionObj))
            {
                return false;
            }
            // 棚別部門別在庫情報検索
            if (!searchResultDepartmentInfo(formMode, ref conditionObj, out bool isEnterExsists, out List<Dao.searchResultDepartmentInfo> departmentInfoList))
            {
                return false;
            }
            // 出庫情報検索
            Dao.registIssue issueInputInfo = new();
            if (!searchResultIssueInput(ref formMode, ref conditionObj, out issueInputInfo, isEnterExsists, departmentInfoList))
            {
                return false;
            }

            // 編集・参照かつ予備品情報に対して入庫がある場合は在庫一覧まで表示
            if (formMode != FormMode.New && isEnterExsists)
            {
                List<Dao.searchResultInventory> result = new();
                // 在庫一覧の検索
                if (!searchInventoryList(formMode, out result, conditionObj, departmentInfoList))
                {
                    return false;
                }
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
            var targetDicPT0002 = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.ConditionInfo);
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
        /// <returns>正常：0、異常：10</returns>
        private bool searchResultSpares(ref Dao.searchCondition conditionObj)
        {
            // 予備品情報取得
            var result = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultSpares>(SqlName.GetIssueSparesList, SqlName.SubDirIssue, conditionObj, this.db);

            // 数量と単位結合
            result.StockQuantity = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.Inventry.ToString(), result.UnitDigit, result.UnitRoundDivision), result.Unit, false);

            // 小数点以下桁数と丸め処理区分を条件に追加
            conditionObj.UnitDigit = result.UnitDigit;
            conditionObj.UnitRoundDivision = result.UnitRoundDivision;

            // ページ情報取得
            var hierarchyInfo = getPageInfoBlank(TargetCtrlId.SparePartsInformation);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResultSpares>(hierarchyInfo, new List<Dao.searchResultSpares> { result }, 1, true))
            {
                // 正常終了
                return true;
            }
            // エラー
            return false;
        }

        /// <summary>
        /// 部門在庫情報検索
        /// </summary>
        /// <param name="formMode">画面タイプ</param>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="isEnterExsists">予備品情報に対して入庫があるかどうか</param>
        /// <returns>正常：0、異常：10</returns>
        private bool searchResultDepartmentInfo(string formMode, ref Dao.searchCondition conditionObj, out bool isEnterExsists, out List<Dao.searchResultDepartmentInfo> departmentInfoList)
        {
            // 画面に表示するための一覧(編集の場合に使用)
            departmentInfoList = new();

            // ページ情報取得
            var sparesInfo = getPageInfoBlank(TargetCtrlId.DepartmentInformation);

            // 初期化
            isEnterExsists = true;

            // 新規の場合
            if (formMode == FormMode.New)
            {
                // 部門在庫情報検索
                if (!searchResultDepartmentInfoNew(conditionObj, sparesInfo))
                {
                    return false;　//エラー
                }
                else if (!string.IsNullOrEmpty(this.MsgId))
                {
                    // 対象予備品に対して1件も入庫がない場合
                    isEnterExsists = false;
                }
                // 正常終了
                return true;
            }
            // 編集の場合
            else
            {
                // 受払履歴IDより出庫を行った新旧区分、部門、勘定科目、棚番、棚枝番を取得
                List<Dao.searchCondition> conditions = TMQUtil.SqlExecuteClass.SelectList<Dao.searchCondition>(SqlName.SelectWhereByInoutHistoryId, SqlName.SubDirIssue, conditionObj, this.db);

                // 条件が取得できない場合はエラー
                if (conditions == null)
                {
                    return false;
                }

                // 工場IDと結合文字列のディクショナリ、同じ工場で重複取得しないようにする
                Dictionary<int, string> factoryJoinDic = new();
                string strJoin = string.Empty;

                // 出庫した明細より取得した条件で棚別部門別在庫情報を検索する
                foreach (Dao.searchCondition condition in conditions)
                {
                    // 検索条件を設定
                    conditionObj.PartsId = condition.PartsId;                             // 予備品ID
                    conditionObj.OldNewStructureId = condition.OldNewStructureId;         // 新旧区分
                    conditionObj.DepartmentStructureId = condition.DepartmentStructureId; // 部門
                    conditionObj.AccountStructureId = condition.AccountStructureId;       // 勘定科目
                    conditionObj.PartsLocationId = condition.PartsLocationId;             // 棚
                    conditionObj.PartsLocationDetailNo = condition.PartsLocationDetailNo; // 棚枝番

                    // 検索処理
                    var result = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultDepartmentInfo>(SqlName.GetIssueDepartmentListEdit, SqlName.SubDirIssue, conditionObj, this.db);

                    // 間に空白を結合
                    result.DepartmentNm = TMQUtil.CombineNumberAndUnit(result.DepartmentCd, result.DepartmentNm, true);
                    result.SubjectNm = TMQUtil.CombineNumberAndUnit(result.SubjectCd, result.SubjectNm, true);

                    // 数量と単位結合
                    result.StockQuantity = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.Inventry.ToString(), result.UnitDigit, result.UnitRoundDivision), result.Unit, false);

                    // 結合文字取得
                    strJoin = TMQUtil.GetJoinStrOfPartsLocationNoDuplicate(result.FactoryId, this.LanguageId, this.db, ref factoryJoinDic);

                    // 棚番と棚枝番を結合
                    result.PartsLocationName = TMQUtil.GetDisplayPartsLocation(result.PartsLocationName, result.PartsLocationDetailNo, strJoin);

                    // 画面に表示する一覧に設定する
                    departmentInfoList.Add(result);
                }

                // 検索結果が1件も存在しない場合
                if (departmentInfoList == null || departmentInfoList.Count == 0)
                {
                    // 1件もない場合はエラーメッセージ(ここに来ることは通常ないと思うが一応)
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941160005, ComRes.ID.ID111280045 });
                    isEnterExsists = false;
                    return true;
                }

                // 検索結果を並び替え
                // 新旧区分、部門、勘定科目、棚番、棚枝番の昇順に並び替え
                departmentInfoList = departmentInfoList.OrderBy(x => x.OldNewStructureId)
                                                       .ThenBy(x => x.ToDepartmentNm)
                                                       .ThenBy(x => x.ToSubjectNm)
                                                       .ThenBy(x => x.PartsLocationId)
                                                       .ThenBy(x => x.PartsLocationDetailNo).ToList();

                // 検索結果の設定
                if (SetSearchResultsByDataClass<Dao.searchResultDepartmentInfo>(sparesInfo, departmentInfoList, departmentInfoList.Count, true))
                {
                    // 正常終了
                    return true;
                }
            }
            // エラー
            return false;
        }

        /// <summary>
        /// 部門在庫情報検索：新規
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="sparesInfo">ページ情報</param>
        /// <returns>正常：true 異常：false</returns>
        private bool searchResultDepartmentInfoNew(Dao.searchCondition conditionObj, CommonWebTemplate.CommonDefinitions.PageInfo sparesInfo)
        {
            // 部門在庫情報取得
            var results = TMQUtil.SqlExecuteClass.SelectList<Dao.searchResultDepartmentInfo>(SqlName.GetIssueDepartmentList, SqlName.SubDirIssue, conditionObj, this.db);
            if (results == null || results.Count == 0)
            {
                // 1件もない場合はエラーメッセージ
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941160005, ComRes.ID.ID111280045 });
                return true;
            }

            // 工場IDと結合文字列のディクショナリ、同じ工場で重複取得しないようにする
            Dictionary<int, string> factoryJoinDic = new();
            string strJoin = string.Empty;

            // 部門在庫情報取得
            foreach (var data in results)
            {
                // 間に空白を結合
                data.DepartmentNm = TMQUtil.CombineNumberAndUnit(data.DepartmentCd, data.DepartmentNm, true);
                data.SubjectNm = TMQUtil.CombineNumberAndUnit(data.SubjectCd, data.SubjectNm, true);

                // 数量と単位結合
                data.StockQuantity = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(data.Inventry.ToString(), data.UnitDigit, data.UnitRoundDivision), data.Unit, false);

                // 結合文字取得
                strJoin = TMQUtil.GetJoinStrOfPartsLocationNoDuplicate(data.FactoryId, this.LanguageId, this.db, ref factoryJoinDic);

                // 棚番と棚枝番を結合
                data.PartsLocationName = TMQUtil.GetDisplayPartsLocation(data.PartsLocationName, data.PartsLocationDetailNo, strJoin);
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResultDepartmentInfo>(sparesInfo, results, results.Count(), true))
            {
                // 正常終了
                return true;
            }
            return true;
        }

        /// <summary>
        /// 出庫情報入力
        /// </summary>
        /// <param name="formMode">画面モード</param>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="isEnterExsists">予備品情報に対して入庫があるかどうか</param>
        /// <returns>正常：0、異常：10</returns>
        private bool searchResultIssueInput(ref string formMode, ref Dao.searchCondition conditionObj, out Dao.registIssue result, bool isEnterExsists, List<Dao.searchResultDepartmentInfo> departmentInfoList = null)
        {
            // ページ情報取得
            var issueInputInfo = getPageInfoBlank(TargetCtrlId.IssueInformation);

            // 対象予備品に対して1件も入庫がない場合
            if (!isEnterExsists)
            {
                // 空データ作成
                result = new Dao.registIssue();
                // 画面制御用(参照モード)
                result.FormType = int.Parse(FormMode.Reference);

                // 検索結果の設定
                if (SetSearchResultsByDataClass<Dao.registIssue>(issueInputInfo, new List<Dao.registIssue> { result }, 1, true))
                {
                    // 正常終了
                    return true;
                }
            }

            // 出庫情報取得
            result = TMQUtil.SqlExecuteClass.SelectEntity<Dao.registIssue>(SqlName.GetIssueInput, SqlName.SubDirIssue, conditionObj, this.db);
            if (result == null)
            {
                return false;
            }

            // 新規の場合
            if (formMode == FormMode.New)
            {
                // 出庫日をセット
                result.InoutDatetime = DateTime.Today;

                // 出庫数の初期値をセット
                result.NumberShipments = 1;

                // 出庫区分の初期値をセット
                result.ShippingDivisionStructureId = getInitialValue((int)StructureGroupId.IssueDivision, 1, "1");

                // 画面タイプをセット(ボタン制御用)
                result.FormType = int.Parse(formMode);
            }
            // 編集の場合
            else
            {
                // 予備品詳細画面より遷移した場合
                if (formMode == FormMode.PT0001Edit)
                {
                    // 受払履歴IDをカンマ区切りで代入
                    conditionObj.IdList = returnIdList(SqlName.SelectInoutHistoryIdByInoutHistoryId, conditionObj);
                }

                // 受払履歴IDより受払数、出庫区分を取得する
                var getIssueInfo = TMQUtil.SqlExecuteClass.SelectEntity<Dao.registIssue>(SqlName.GetIssueInputEdit, SqlName.SubDirIssue, conditionObj, this.db);
                if (getIssueInfo == null)
                {
                    return false;
                }

                // 棚卸中のデータがあるか
                if (isCheckInventryData(conditionObj))
                {
                    // 「棚卸中データのため、修正・削除はできません。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160010 });
                    // 画面タイプをセット(ボタン制御用)
                    result.FormType = int.Parse(FormMode.Reference);
                }

                // 出庫日をセット
                result.InoutDatetime = getIssueInfo.InoutDatetime;
                conditionObj.InoutDatetime = getIssueInfo.InoutDatetime;

                // 出庫数をセット
                result.NumberShipments = getIssueInfo.NumberShipments;

                // 出庫区分をセット
                result.ShippingDivisionStructureId = getIssueInfo.ShippingDivisionStructureId;

                // 作業No(隠し項目)をセット
                result.WorkNo = getIssueInfo.WorkNo;
                conditionObj.WorkNo = getIssueInfo.WorkNo;

                // 在庫確定日取得
                DateTime targetMonth = TMQUtil.PartsGetInfo.GetInventoryConfirmationDate(conditionObj, this.db);

                // 出庫日が棚卸確定日以前のデータが存在するかどうか
                bool isErrorFixedDatetime = false;

                // 棚別部門別在庫一覧(複数のレコードから出庫できるので選択されたレコードごとにチェックを行う)
                foreach (Dao.searchResultDepartmentInfo departmentInfo in departmentInfoList)
                {
                    // 検索条件を設定
                    conditionObj.OldNewStructureId = departmentInfo.OldNewStructureId;             // 新旧区分ID
                    conditionObj.DepartmentStructureId = departmentInfo.ToDepartmentNm.ToString(); // 部門ID
                    conditionObj.AccountStructureId = departmentInfo.ToSubjectNm.ToString();       // 勘定科目ID
                    conditionObj.PartsLocationId = departmentInfo.PartsLocationId;                 // 棚番ID
                    conditionObj.PartsLocationDetailNo = departmentInfo.PartsLocationDetailNo;     // 棚枝番

                    // 棚卸確定日取得
                    DateTime fixedDatetime = TMQUtil.PartsGetInfo.GetTakeInventoryConfirmationDate(conditionObj, this.db);

                    // 出庫日が棚卸確定日以前の場合
                    if (result.InoutDatetime <= fixedDatetime)
                    {
                        // エラーフラグをTrueにして繰り返し処理終了
                        isErrorFixedDatetime = true;
                        break;
                    }
                }

                // 出庫日が在庫確定日以前の場合
                if (result.InoutDatetime <= targetMonth)
                {
                    // 「在庫確定以前の情報のため、修正・取消することはできません。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141110002 });
                    // 画面タイプをセット(ボタン制御用)
                    result.FormType = int.Parse(FormMode.Reference);
                }
                // 出庫日が棚卸確定日以前の場合
                else if (isErrorFixedDatetime)
                {
                    // 「棚卸確定以前の情報のため、修正・取消することはできません。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160005 });
                    // 画面タイプをセット(ボタン制御用)
                    result.FormType = int.Parse(FormMode.Reference);
                }
                // 参照モードチェック
                else if (isReferenceMode(conditionObj))
                {
                    // 画面タイプをセット(ボタン制御用)
                    result.FormType = int.Parse(FormMode.Reference);
                }
                else if (result.FormType != int.Parse(FormMode.Reference))
                {
                    // 画面タイプをセット(ボタン制御用)
                    result.FormType = int.Parse(formMode);
                }
            }

            // 小数点以下桁数・丸め処理区分を設定
            setDigit(ref result);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.registIssue>(issueInputInfo, new List<Dao.registIssue> { result }, 1, true))
            {
                // 正常終了
                return true;
            }
            return false;

            // 小数点以下桁数を検索結果に設定
            void setDigit(ref Dao.registIssue result)
            {
                // 該当予備品データを取得
                ComDao.PtPartsEntity partsInfo = new ComDao.PtPartsEntity().GetEntity(long.Parse(result.PartsId), this.db);
                // 構成マスタより数量管理単位のデータを取得
                ComDao.MsStructureEntity structureInfo = new ComDao.MsStructureEntity().GetEntity((int)partsInfo.UnitStructureId, this.db);
                // アイテムIDより数量管理単位の小数点以下桁数を取得
                ComDao.MsItemExtensionEntity extensionInfo = new ComDao.MsItemExtensionEntity().GetEntity((int)structureInfo.StructureItemId, 2, this.db);
                //小数点以下桁数を設定
                result.Digit = int.Parse(extensionInfo.ExtensionData);

                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDirParts, SqlName.GetDetailPartsInfo, out string executeSql);
                TMQUtil.GetFixedSqlStatementWith(SqlName.SubDirParts, SqlName.GetPartsList, out string withSql);

                // SQL実行
                IList<Dao.RoundDigit> partsResult = db.GetListByDataClass<Dao.RoundDigit>(withSql + executeSql, new
                {
                    @LanguageId = this.LanguageId,
                    @PartsId = long.Parse(result.PartsId),
                    @UserFactoryId = TMQUtil.GetUserFactoryId(this.UserId, db)
                });

                // 取得できない場合は四捨五入
                if (partsResult == null || partsResult.Count == 0)
                {
                    result.RoundDivision = (int)TMQConsts.RoundDivision.Round;
                }
                else
                {
                    // 取得できれば取得した内容を設定
                    result.RoundDivision = partsResult[0].UnitRoundDivision;
                }
            }

        }

        /// <summary>
        /// 棚卸中のデータがあるかどうかを見るメソッド
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <returns>データがあればTrue</returns>
        private bool isCheckInventryData(Dao.searchCondition conditionObj)
        {
            // 受払履歴ID取得
            var historyIdAry = conditionObj.IdList.Split(',');
            foreach (string id in historyIdAry)
            {
                // 受払履歴ID
                conditionObj.InoutHistoryId = long.Parse(id);

                // 受払履歴IDより受払日時と更新日時を取得
                var result = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchCondition>(SqlName.SelectInfoByInoutHistoryId, SqlName.SubDirIssue, conditionObj, this.db);

                // 受払日時
                conditionObj.InoutDatetime = result.InoutDatetime;

                // 更新日時
                conditionObj.UpdateDatetime = result.UpdateDatetime;

                // 棚卸中のデータがあるか
                if (InventryCheck.IsExistsInventryData(conditionObj, this.db))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 在庫一覧検索(出庫引当)
        /// </summary>
        /// <param name="formMode">画面タイプ</param>
        /// <param name="result">検索結果</param>
        /// <param name="conditionObj">検索条件</param>
        /// <returns>エラーの場合False</returns>
        private bool searchInventoryList(string formMode, out List<Dao.searchResultInventory> result, Dao.searchCondition conditionObj = null, List<Dao.searchResultDepartmentInfo> departmentInfoList = null)
        {
            // 初期化
            result = new();
            if (conditionObj == null)
            {
                conditionObj = new Dao.searchCondition();
            }

            // 棚別部門別在庫一覧で選択されている行
            List<Dao.searchResultDepartmentInfo> selectedList = new();

            // 引数にわたってきた一覧が空かどうか判定
            if (departmentInfoList != null && departmentInfoList.Count > 0)
            {
                // 引数に検索結果が渡ってきている場合は代入
                selectedList = departmentInfoList;
            }
            else
            {
                // 引数に検索結果が渡ってきていない場合
                // 棚別部門別在庫一覧で選択されている行を取得
                List<object> departmentList = (List<object>)this.IndividualDictionary["deaprtment"];
                foreach (object row in departmentList)
                {
                    // 行をディクショナリに変換
                    Dictionary<string, object> dicRow = (Dictionary<string, object>)row;

                    // 選択チェックボックスが選択されているか判定
                    if (dicRow["SELTAG"].ToString() == "1")
                    {
                        // ディクショナリをデータクラスに変換
                        Dao.searchResultDepartmentInfo departmentInfo = new();
                        SetDataClassFromDictionary((Dictionary<string, object>)row, TargetCtrlId.DepartmentInformation, departmentInfo);

                        // 一覧に格納
                        selectedList.Add(departmentInfo);
                    }
                }

            }

            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.IssueInformation);

            // ページ情報取得
            var stockInfo = getPageInfoBlank(TargetCtrlId.IssueInformation);

            // 検索条件データの取得
            if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, TargetCtrlId.IssueInformation, conditionObj, stockInfo))
            {
                // エラーの場合終了
                return false;
            }

            // 新規の場合入力チェック
            if (formMode == FormMode.New)
            {
                // エラー情報セット用Dictionary
                var errorInfoDictionary = new List<Dictionary<string, object>>();

                // 出庫日、出庫数、出庫区分入力チェック
                if (isErrorInout(conditionObj, condition, ref errorInfoDictionary, selectedList))
                {
                    // エラー情報を画面に反映
                    SetJsonResult(errorInfoDictionary);
                    return false;
                }

                // 出庫数入力チェック
                if (isErrorNumberShipments(condition, ref errorInfoDictionary, selectedList))
                {
                    // エラー情報を画面に反映
                    SetJsonResult(errorInfoDictionary);
                    return false;
                }
            }

            // 在庫一覧検索
            if (searchResultInventory(formMode, condition, conditionObj, out result, selectedList) != CommonProcReturn.ProcStatus.Valid)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 在庫一覧表示
        /// </summary>
        /// <param name="formMode">画面タイプ</param>
        /// <param name="condition">出庫情報</param>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="result">検索結果</param>
        /// <returns>正常：0、異常：10</returns>
        private int searchResultInventory(string formMode, Dictionary<string, object> condition, Dao.searchCondition conditionObj, out List<Dao.searchResultInventory> totalResult, List<Dao.searchResultDepartmentInfo> selectedList)
        {
            // 受払区分
            long inoutDivision;
            // SQLファイル名
            string sqlName;

            // 新規の場合は受払区分：受入
            if (formMode == FormMode.New)
            {
                // 受入
                inoutDivision = getInitialValue((int)StructureGroupId.InoutDivision, 1, "1");
                // 新規SQL
                sqlName = SqlName.GetIssueInventoryList;
            }
            else
            {
                // 払出
                inoutDivision = getInitialValue((int)StructureGroupId.InoutDivision, 1, "2");
                // 編集用SQL
                sqlName = SqlName.GetIssueInventoryListEdit;
            }

            // 受払区分
            conditionObj.InoutDivisionStructureId = inoutDivision;

            // 検索後の在庫一覧(棚別部門別在庫一覧で選択された行を条件に検索し、各検索結果を格納する)
            totalResult = new();

            // 棚別部門別在庫一覧で選択された行を対象に検索
            foreach (Dao.searchResultDepartmentInfo departmentInfo in selectedList)
            {
                // 検索条件を設定
                conditionObj.OldNewStructureId = departmentInfo.OldNewStructureId;             // 新旧区分ID
                conditionObj.DepartmentStructureId = departmentInfo.ToDepartmentNm.ToString(); // 部門ID
                conditionObj.AccountStructureId = departmentInfo.ToSubjectNm.ToString();       // 勘定科目ID
                conditionObj.PartsLocationId = departmentInfo.PartsLocationId;                 // 棚番ID
                conditionObj.PartsLocationDetailNo = departmentInfo.PartsLocationDetailNo;     // 棚枝番
                conditionObj.UnitStructureId = departmentInfo.UnitStructureId;                 // 数量管理単位

                // 在庫一覧情報取得
                List<Dao.searchResultInventory> results = TMQUtil.SqlExecuteClass.SelectList<Dao.searchResultInventory>(sqlName, SqlName.SubDirIssue, conditionObj, this.db);

                // 取得できなかった場合は次の条件で検索
                if (results == null || results.Count == 0)
                {
                    continue;
                }

                // 画面に表示する一覧に設定する
                foreach (Dao.searchResultInventory result in results)
                {
                    totalResult.Add(result);
                }
            }

            // 丸め処理・数量と単位を結合
            totalResult.ToList().ForEach(x => x.JoinStrAndRound());

            // 工場IDと結合文字列のディクショナリ、同じ工場で重複取得しないようにする
            Dictionary<int, string> factoryJoinDic = new();
            string strJoin = string.Empty;
            foreach (var data in totalResult)
            {
                // 結合文字取得
                strJoin = TMQUtil.GetJoinStrOfPartsLocationNoDuplicate(data.FactoryId, this.LanguageId, this.db, ref factoryJoinDic);
                // 表示用棚番取得
                data.PartsLocationDisplay = TMQUtil.GetDisplayPartsLocation(data.PartsLocationCd, data.PartsLocationDetailNo, strJoin);
            }

            // 入庫日、ロットNo.の昇順に並び替え
            totalResult = totalResult.OrderBy(x => x.ReceivingDatetime).ThenBy(x => x.lotNo).ToList();

            // 新規の場合は入力チェック
            if (formMode == FormMode.New)
            {
                // 出庫日が入庫日以降のデータを取得
                var checkList = totalResult.Where(x => DateTime.Parse(x.ReceivingDatetime) <= DateTime.Parse(getDictionaryKeyValue(condition, "inout_datetime"))).ToList();

                // データが無ければ引当不可とする
                if (checkList.Count() == 0)
                {
                    // 「出庫日が入庫日以前のデータは出庫できません。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141120004 });
                    return CommonProcReturn.ProcStatus.Error;
                }
            }

            // ページ情報取得
            var inventoryInfo = getPageInfoBlank(TargetCtrlId.InventoryInformation);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResultInventory>(inventoryInfo, totalResult, totalResult.Count(), true))
            {
                // 正常終了
                return this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return CommonProcReturn.ProcStatus.Error;
        }

        /// <summary>
        /// GetPageInfoでは十分に情報が取得出来ないため処理をまとめる
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <returns>ページ情報</returns>
        private CommonWebTemplate.CommonDefinitions.PageInfo getPageInfoBlank(string ctrlId)
        {
            var result = GetPageInfo(ctrlId, this.pageInfoList);
            result.CtrlId = ctrlId;                                             // コントロールID追加
            result.CtrlType = FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn3;       // コントロールタイプ追加
            return result;
        }
        #endregion

        #region 登録処理
        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdit()
        {
            // 在庫一覧
            List<Dao.searchResultInventory> result = new();

            // 在庫一覧再検索
            if (!searchInventoryList(FormMode.New, out result))
            {
                return false;
            }

            // 排他チェック
            if (isErrorExclusive())
            {
                // 「出庫予定の情報は他のユーザにより出庫されました。再度、出庫引当処理を実行してください。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID111120143 });
                return false;
            }

            // 入力チェック
            if (isCheckConsistency())
            {
                return false;
            }

            // 出庫情報
            Dictionary<string, object> issueInput;

            // クラスの宣言
            TMQUtil.PartsInventory.Output output = new(this.db, this.UserId, this.LanguageId);

            // 登録件数取得
            int loopCnt = returnLoopCut(result, out issueInput);

            // 対応する引数
            List<TMQDao.PartsInventory.Output> conditions = getRegistInfoList<TMQDao.PartsInventory.Output>(TargetCtrlId.InventoryInformation, DateTime.Now, loopCnt);

            // 登録件数分引数を追加
            for (int i = 0; i < loopCnt; i++)
            {
                // 画面情報取得
                DateTime now = DateTime.Now;
                // ここでは登録する内容を取得するので、テーブルのデータクラスを指定
                Dao.registIssue registInfo = getRegistInfo<Dao.registIssue>(TargetGrpNo.InventryList, i, now);

                conditions[i].InoutDatetime = DateTime.Parse(getDictionaryKeyValue(issueInput, "inout_datetime"));                          // 受払日時(出庫日)
                conditions[i].InoutQuantity = decimal.Parse(Regex.Replace(registInfo.IssueQuantity.ToString(), @"[^-0-9.]", ""));           // 受払数
                conditions[i].PartsLocationId = result[i].PartsLocationId;                                                                  // 棚ID
                conditions[i].PartsLocationDetailNo = result[i].PartsLocationDetailNo;　　　　　　　　　　　　                              // 棚番
                conditions[i].ShippingDivisionStructureId = int.Parse(getDictionaryKeyValue(issueInput, "shipping_division_structure_id")); // 出庫区分
            }

            // 登録
            if (!output.New(conditions, out long workNo))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isCheckConsistency()
        {
            // ページ情報取得(条件エリア)
            Dao.searchCondition conditionObj = new Dao.searchCondition();

            // ページ情報取得
            var stockInfo = getPageInfoBlank(TargetCtrlId.IssueInformation);

            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.IssueInformation);

            // 検索条件データの取得
            if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, TargetCtrlId.IssueInformation, conditionObj, stockInfo))
            {
                // エラーの場合終了
                return true;
            }

            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 引当数合計と入力した出庫数が異なる場合はエラー
            if (isSameNum(getDictionaryKeyValue(condition, "number_shipments"), getDictionaryKeyValue(condition, "reference_quantity"), ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }
            // 正常
            return false;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNo">取得するグループ</param>
        /// <param name="rowNo">行番号</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(short groupNo, int rowNo, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 相称型を使用することで、色々な型を呼出元で宣言することができる
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            // 在庫一覧取得
            List<Dictionary<string, object>> inventryList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

            T resultInfo = new();

            //コントロールIDより画面の項目を取得
            Dictionary<string, object> condition = new();

            if (rowNo == 0)
            {
                // 1回目は先頭を取得
                condition = inventryList.FirstOrDefault();
            }
            else
            {
                // それ以降
                condition = inventryList.Skip(rowNo).FirstOrDefault();
            }

            var mapInfo = grpMapInfo.selectByCtrlId(TargetCtrlId.InventoryInformation);
            // 登録データの設定
            if (!SetExecuteConditionByDataClass(condition, mapInfo.Value, resultInfo, now, this.UserId, this.UserId))
            {
                // エラーの場合終了
                return resultInfo;
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
        private List<T> getRegistInfoList<T>(string ctrlId, DateTime now, int registCount)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(TargetCtrlId.InventoryInformation);
            // コントロールIDにより画面の項目(一覧)を取得
            List<Dictionary<string, object>> resultList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.InventoryInformation);
            // 戻り値となるデータクラスのリスト
            List<T> registInfoList = new();
            // 一覧を繰り返し、データクラスに変換、リストへ追加する
            foreach (var resultRow in resultList)
            {
                if (registCount <= 0)
                {
                    break;
                }
                T registInfo = new();
                if (!SetExecuteConditionByDataClass<T>(resultRow, TargetCtrlId.InventoryInformation, registInfo, now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return registInfoList;
                }
                registInfoList.Add(registInfo);
                registCount--;
            }
            return registInfoList;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 初期値を取得する
        /// </summary>
        /// <param name="structureId">構成グループID</param>
        /// <param name="seq">連番</param>
        /// <param name="extentionData">拡張データ</param>
        /// <returns>初期値の構成ID</returns>
        private int getInitialValue(int structureId, int seq, string extentionData)
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();

            //構成グループID
            param.StructureGroupId = structureId;

            //連番
            param.Seq = seq;

            // 機能レベル取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> partsTypeList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            foreach (var partsType in partsTypeList)
            {
                if (partsType.ExData == extentionData)
                {
                    return partsType.StructureId;
                }
            }
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive()
        {
            // 明細(複数)の場合の排他チェック
            var list = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.InventoryInformation, true);
            if (!checkExclusiveList(TargetCtrlId.InventoryInformation, list))
            {
                // エラーの場合
                return true;
            }

            // 排他チェックOK
            return false;
        }

        /// <summary>
        /// 編集時参照モードかどうかを見るメソッド
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <returns>参照モードの場合True</returns>
        private bool isReferenceMode(Dao.searchCondition conditionObj)
        {
            // 受払フラグ
            bool inoutFlg = false;

            // 受払履歴ID取得
            var historyIdAry = conditionObj.IdList.Split(',');
            foreach (string id in historyIdAry)
            {
                // 受払履歴ID
                conditionObj.InoutHistoryId = long.Parse(id);

                // 受払履歴IDよりロット管理IDを取得
                var info = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultInventory>(SqlName.SelectInfoByInoutHistoryId, SqlName.SubDirIssue, conditionObj, this.db);

                // ロット管理ID
                conditionObj.LotControlId = long.Parse(info.LotControlId);

                // 同一ロット管理IDで対象の受払履歴以降に受払が発生している場合、警告メッセージを表示
                var countList = TMQUtil.SqlExecuteClass.SelectEntity<Dao.searchResultInventory>(SqlName.SelectPreIssueDayCount, SqlName.SubDirIssue, conditionObj, this.db);
                if (countList.Count != 0)
                {
                    inoutFlg = true;
                    break;
                }
            }

            // 最新のデータが含まれていなければ参照モードとする。
            if (inoutFlg)
            {
                // 「新しい受払履歴実績が存在します。修正・取消することはできません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141010001 });
                return true;
            }
            return false;
        }

        /// <summary>
        /// 出庫引当前入力チェック
        /// </summary>
        /// <param name="conditionObj">検索条件</param>
        /// <param name="condition">出庫情報</param>
        /// <param name="errorInfoDictionary">エラー情報リスト</param>
        /// <param name="selectedList">棚別部門別在庫一覧で選択された行</param>
        /// <returns>エラーがあればTrue</returns>
        private bool isErrorInout(Dao.searchCondition conditionObj, Dictionary<string, object> condition, ref List<Dictionary<string, object>> errorInfoDictionary, List<Dao.searchResultDepartmentInfo> selectedList)
        {
            // エラー情報セット用Dictionary
            errorInfoDictionary = new List<Dictionary<string, object>>();

            // エラー情報を画面に設定するためのマッピング情報リスト
            var info = getResultMappingInfo(TargetCtrlId.IssueInformation);
            string val1 = info.getValName("inout_datetime");    // エラーセットVAL値
            string val2 = info.getValName("number_shipments");  // エラーセットVAL値

            // 単一の内容を取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.IssueInformation);

            // エラー情報格納クラス
            var errorInfo = new ErrorInfo(targetDic);
            string errMsg = string.Empty;

            // 予備品IDより、在庫確定日取得
            DateTime targetMonth = TMQUtil.PartsGetInfo.GetInventoryConfirmationDate(conditionObj, this.db);

            // 出庫日取得
            DateTime inoutDatetime = DateTime.Parse(getDictionaryKeyValue(condition, "inout_datetime"));

            // 出庫日が未来日の場合
            if (inoutDatetime > DateTime.Today)
            {
                // 「未来の日付は入力できません。」
                errMsg = GetResMessage(new string[] { ComRes.ID.ID111320002 });
                errorInfo.setError(errMsg, val1);           // エラー情報をセット
                errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                return true;
            }
            // 出庫日が在庫確定日前の場合
            else if (inoutDatetime <= targetMonth)
            {
                // 日付を変換
                string dispTargetMonth = targetMonth.ToString(GetResMessage(new string[] { ComRes.ID.ID150000003 }));

                // 「在庫確定前[{0}]の日付は入力できません。」
                errMsg = GetResMessage(new string[] { ComRes.ID.ID141110001, dispTargetMonth });
                errorInfo.setError(errMsg, val1);           // エラー情報をセット
                errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                return true;
            }

            // 棚・部門の組み合わせのリスト
            List<string> selectCheckList = new();
            string checkKey = string.Empty;

            // 棚別部門別在庫一覧で選択された行を対象にチェック
            foreach (Dao.searchResultDepartmentInfo departmentInfo in selectedList)
            {
                // 検索条件を設定
                conditionObj.OldNewStructureId = departmentInfo.OldNewStructureId;             // 新旧区分ID
                conditionObj.DepartmentStructureId = departmentInfo.ToDepartmentNm.ToString(); // 部門ID
                conditionObj.AccountStructureId = departmentInfo.ToSubjectNm.ToString();       // 勘定科目ID
                conditionObj.PartsLocationId = departmentInfo.PartsLocationId;                 // 棚番ID
                conditionObj.PartsLocationDetailNo = departmentInfo.PartsLocationDetailNo;     // 棚枝番

                // 棚ID、棚枝番、部門IDを文字列としてチェック用リストに追加
                checkKey = departmentInfo.PartsLocationId.ToString() + "_" + departmentInfo.ToDepartmentNm.ToString() + "_" + departmentInfo.PartsLocationDetailNo;
                if (!selectCheckList.Contains(checkKey))
                {
                    selectCheckList.Add(checkKey);
                }

                // 棚卸確定日取得
                DateTime fixedDatetime = TMQUtil.PartsGetInfo.GetTakeInventoryConfirmationDate(conditionObj, this.db);

                // 出庫日が棚卸確定日前の場合
                if (inoutDatetime <= fixedDatetime)
                {
                    // 日付を変換
                    string dispFixedDatetime = fixedDatetime.ToString(GetResMessage(new string[] { ComRes.ID.ID150000003 }));

                    // 「棚卸確定前[{0}]の日付は入力できません。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID141160004, dispFixedDatetime });
                    errorInfo.setError(errMsg, val1);           // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                    return true;
                }
            }

            // 棚・部門の組み合わせが複数選択されている場合はエラー
            if (selectCheckList.Count > 1)
            {
                // 「同一の棚番・部門を選択してください。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141200008 });
                return true;
            }

            // @単位の形で渡ってくるため数値、小数点、符号のみを抽出
            string num = Regex.Replace(getDictionaryKeyValue(condition, "number_shipments"), @"[^-0-9.]", "");
            // 先頭の文字が「.」であれば0を先頭に追加
            if (num.FirstOrDefault() == '.')
            {
                num = "0" + num;
            }

            if (decimal.Parse(num) <= 0)
            {
                // 「出庫数は0以下で登録できません。」
                errMsg = GetResMessage(new string[] { ComRes.ID.ID141060007, ComRes.ID.ID111120069 });
                errorInfo.setError(errMsg, val2);           // エラー情報をセット
                errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
            }
            else if (decimal.Parse(num) < 0 || decimal.Parse(num) > 9999999999.99m)
            {
                // 「出庫数は整数部10桁以下で入力してください。」
                errMsg = GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111120069, ComRes.ID.ID911140003, "10" });
                errorInfo.setError(errMsg, val2);           // エラー情報をセット
                errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
            }

            // エラーがある場合はTrueを返す
            if (errMsg != string.Empty)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在庫数チェック
        /// </summary>
        /// <param name="condition">出庫情報</param>
        /// <param name="errorInfoDictionary">エラー情報リスト</param>
        /// <param name="selectedList">棚別部門別在庫一覧で選択された行</param>
        /// <returns>エラーがあればTrue</returns>
        private bool isErrorNumberShipments(Dictionary<string, object> condition, ref List<Dictionary<string, object>> errorInfoDictionary, List<Dao.searchResultDepartmentInfo> selectedList)
        {
            // エラー情報セット用Dictionary
            errorInfoDictionary = new List<Dictionary<string, object>>();

            // エラー情報を画面に設定するためのマッピング情報リスト
            var info = getResultMappingInfo(TargetCtrlId.IssueInformation);
            string val2 = info.getValName("number_shipments");  // エラーセットVAL値

            // 単一の内容を取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.IssueInformation);

            // エラー情報格納クラス
            var errorInfo = new ErrorInfo(targetDic);
            string errMsg = string.Empty;

            // 棚別部門別在庫一覧で選択されている行の在庫数の合計
            decimal totalStock = 0;

            // 棚別部門別在庫一覧で選択された行を対象にチェック
            foreach (Dao.searchResultDepartmentInfo departmentInfo in selectedList)
            {
                // 選択された行の合計在庫数を取得する
                totalStock += decimal.Parse(Regex.Replace(departmentInfo.StockQuantity, @"[^-0-9.]", ""));
            }

            // 出庫数は@単位の形で渡ってくるため数値、小数点、符号のみを抽出
            string nums = Regex.Replace(getDictionaryKeyValue(condition, "number_shipments"), @"[^-0-9.]", "");

            // 先頭の文字が「.」であれば0を先頭に追加
            if (nums.FirstOrDefault() == '.')
            {
                nums = "0" + nums;
            }

            // 出庫数が在庫数よりも多ければエラーを出す
            if (decimal.Parse(nums) > totalStock)
            {
                // 「在庫数以下の数値で入力してください。」
                errMsg = GetResMessage(new string[] { ComRes.ID.ID111110056 });
                errorInfo.setError(errMsg, val2);           // エラー情報をセット
                errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在庫数整合性チェック
        /// </summary>
        /// <param name="numberShipments">引当時出庫数</param>
        /// <param name="registNumberShipments">登録時出庫数</param>
        /// <param name="errorInfoDictionary">エラー情報リスト</param>
        /// <returns>エラーがあればTrue</returns>
        private bool isSameNum(string numberShipments, string registNumberShipments, ref List<Dictionary<string, object>> errorInfoDictionary)
        {
            // エラー情報セット用Dictionary
            errorInfoDictionary = new List<Dictionary<string, object>>();

            // エラー情報を画面に設定するためのマッピング情報リスト
            var info = getResultMappingInfo(TargetCtrlId.IssueInformation);
            string val2 = info.getValName("number_shipments");  // エラーセットVAL値

            // 単一の内容を取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.IssueInformation);

            // エラー情報格納クラス
            var errorInfo = new ErrorInfo(targetDic);
            string errMsg = string.Empty;

            // 出庫数は@単位の形で渡ってくるため数値、小数点、符号のみを抽出
            string nums = Regex.Replace(numberShipments, @"[^-0-9.]", "");

            // 先頭の文字が「.」であれば0を先頭に追加
            if (nums.FirstOrDefault() == '.')
            {
                nums = "0" + nums;
            }

            // 出庫引当時の出庫数と登録時の出庫数が異なればエラー
            if (decimal.Parse(nums) != decimal.Parse(registNumberShipments))
            {
                // 「出庫数と引当数合計が一致しません。」
                errMsg = GetResMessage(new string[] { ComRes.ID.ID111120142 });
                errorInfo.setError(errMsg, val2);           // エラー情報をセット
                errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                return true;
            }
            // 正常
            return false;
        }

        /// <summary>
        /// 登録件数を返す処理
        /// </summary>
        /// <param name="result">在庫一覧</param>
        /// <param name="issueInput">出庫入力</param>
        /// <returns>登録件数</returns>
        private int returnLoopCut(List<Dao.searchResultInventory> result, out Dictionary<string, object> issueInput)
        {
            // 入庫日が未来日のものでないデータを取得
            var checkList = result.Where(x => DateTime.Parse(x.ReceivingDatetime) <= DateTime.Today).ToList();

            // 出庫情報取得
            issueInput = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, TargetCtrlId.IssueInformation);

            // 入力した出庫数取得
            decimal issueNum = decimal.Parse(Regex.Replace(getDictionaryKeyValue(issueInput, "number_shipments"), @"[^-0-9.]", ""));

            // ループカウント
            int loopCnt = 1;

            decimal totalNum = 0;

            foreach (var data in checkList)
            {
                // 在庫数
                totalNum += decimal.Parse(Regex.Replace(data.Unit, @"[^-0-9.]", ""));

                // 在庫数と入力した出庫数を比べ在庫数の方が多ければ更新
                if (totalNum >= issueNum)
                {
                    break;
                }
                loopCnt++;
            }
            return loopCnt;
        }

        /// <summary>
        /// 作業No、受払履歴IDより同一の作業Noの受払履歴IDをカンマ区切りの文字列で返す処理
        /// </summary>
        /// <param name="sqlName">SQLファイル名</param>
        /// <param name="conditionObj">検索条件</param>
        /// <returns>カンマ区切りの受払履歴リスト</returns>
        private string returnIdList(string sqlName, Dao.searchCondition conditionObj)
        {
            // 受払履歴IDより同一の作業Noの受払履歴IDを取得する
            var inoutHistoryIdList = TMQUtil.SqlExecuteClass.SelectList<Dao.searchCondition>(sqlName, SqlName.SubDirIssue, conditionObj, this.db);
            if (inoutHistoryIdList == null || inoutHistoryIdList.Count == 0)
            {
                return string.Empty;
            }

            // 受払履歴ID格納配列
            string[] array = new string[inoutHistoryIdList.Count];
            int i = 0;

            // ループして配列のID格納
            foreach (var inoutHistoryId in inoutHistoryIdList)
            {
                array[i] = inoutHistoryId.IdList;
                i++;
            }

            // 受払履歴IDをカンマ区切りで代入
            return string.Join(",", array);
        }
        #endregion
    }
}