using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Http;
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
using Const = CommonTMQUtil.CommonTMQConstants;
using Dao = BusinessLogic_MS1000.BusinessLogicDataClass_MS1000;
using Master = CommonTMQUtil.CommonTMQUtil.ComMaster;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_MS1000
{
    /// <summary>
    /// 地区/工場マスタ
    /// </summary>
    public class BusinessLogic_MS1000 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 構成グループID
        /// </summary>
        private static int structureGroupId = 1000;

        /// <summary>
        /// 拡張項目件数
        /// </summary>
        private static class ItemExCnt
        {
            /// <summary>地区</summary>
            public const int District = 0;
            /// <summary>工場</summary>
            public const int Factory = 6;
        }

        /// <summary>
        /// アイテム一覧タイプ
        /// </summary>
        private static int itemListType = (int)TMQUtil.ComMaster.ItemListType.Standard;

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private class SqlName
        {
            /// <summary>SQL名：場所階層アイテム一覧取得</summary>
            public const string GetDistrictFactoryItemList = "GetDistrictFactoryItemList";
            /// <summary>SQL名：場所階層アイテム情報取得</summary>
            public const string GetLocationStructureItemInfo = "GetLocationStructureItemInfo";
            /// <summary>SQL名：構成ID取得</summary>
            public const string GetStructureId = "GetStructureId";
            /// <summary>SQL名：場所階層アイテム表示順一覧取得</summary>
            public const string GetLocationStructureIItemOrderList = "GetLocationStructureIItemOrderList";
            /// <summary>SQL名：構成マスタ登録</summary>
            public const string InsertLocationStructureInfo = "InsertLocationStructureInfo";
            /// <summary>SQL名：場所階層翻訳マスタ削除</summary>
            public const string DeleteLocationStructureOrder = "DeleteLocationStructureOrder";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = Master.SqlName.SubDir + @"\LocationStructure";

            /// <summary>
            /// ユーザの権限チェック用SQL
            /// </summary>
            public static class AuthCheck
            {
                /// <summary>SQLファイル格納フォルダ</summary>
                public const string SubDir = @"Common\HistoryManagement";
                /// <summary>ユーザの権限の拡張項目を取得するSQL</summary>
                public const string GetAuthLevel = "GetAuthLevel";
            }
        }

        /// <summary>
        /// フォーム、グループ、コントロールの親子関係を表現した場合の定数クラス
        /// </summary>
        private class ConductInfo
        {
            /// <summary>
            /// 一覧画面
            /// </summary>
            public static class FormList
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 1;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 検索条件の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string SearchId = "BODY_000_00_LST_0";
                    /// <summary>
                    /// 地区アイテム一覧の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string DistrictItemId = "BODY_020_00_LST_0";
                    /// <summary>
                    /// 工場アイテム一覧の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string FactoryItemId = "BODY_030_00_LST_0";
                    /// <summary>
                    /// 非表示情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string HiddenId = "BODY_050_00_LST_0";

                }
            }

            /// <summary>
            /// 登録・修正画面
            /// </summary>
            public static class FormEdit
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 2;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// アイテムIDの画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string ItemId = "BODY_000_00_LST_1";
                    /// <summary>
                    /// アイテム翻訳の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string ItemTranId = "BODY_010_00_LST_1";
                    /// <summary>
                    /// 地区アイテム情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string DistrictItemInfoId = "BODY_020_00_LST_1";
                    /// <summary>
                    /// 工場アイテム情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string FactoryItemInfoId = "BODY_030_00_LST_1";
                }
            }

            /// <summary>
            /// 表示順変更画面
            /// </summary>
            public static class FormOrder
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
                    /// アイテム一覧の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string ItemOrderId = "BODY_000_00_LST_2";
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_MS1000() : base()
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
            if (compareId.IsBack() || compareId.IsRegist())
            {
                // 戻るボタン、登録ボタン押下時
                return InitSearch();
            }

            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo:   // 一覧
                    if (!initList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormEdit.FormNo:   // 登録・修正
                    if (!initEdit())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormOrder.FormNo:  // 表示順変更
                    if (!initOrder())
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

            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo:     // 一覧
                    // 一覧検索実行
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
            else if (compareId.IsDelete())
            {
                // 削除の場合
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

            switch (this.FormNo)
            {
                case ConductInfo.FormEdit.FormNo:
                    // 登録・修正画面の登録処理
                    resultRegist = executeRegistEdit();
                    break;
                case ConductInfo.FormOrder.FormNo:
                    // 表示順変更画面の登録処理
                    resultRegist = executeRegistOrder();
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
            // 非表示情報取得
            var condition = getHiddenInfo();
            if (condition == null)
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 対象コントロールID取得
            string ctrlId = getCtrlId(condition, ConductInfo.FormList.FormNo);
            var list = getSelectedRowsByList(this.resultInfoDictionary, ctrlId);
            if (list == null || list.Count == 0)
            {
                // 選択行が無ければエラー
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941160003 });
                return ComConsts.RETURN_RESULT.NG;
            }

            // 一覧のチェックされた行のレコードを削除する
            // 削除SQL取得
            TMQUtil.GetFixedSqlStatement(Master.SqlName.SubDir, Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, out string sql);
            // 削除処理実行
            if (!DeleteSelectedList<Dao.SearchResult>(ctrlId, sql))
            {
                setError();
                return ComConsts.RETURN_RESULT.NG;
            }

            // 行削除
            var now = DateTime.Now;
            foreach (var deleteRow in list)
            {
                Dao.SearchResult deleteCondition = new();
                SetExecuteConditionByDataClass<Dao.SearchResult>(deleteRow, ctrlId, deleteCondition, now, this.UserId);
                // 子のアイテムを削除する
                if (!TMQUtil.UpdateChildLayers(now, condition, deleteCondition.StructureId, this.UserId, this.db))
                {
                    setError();
                    return ComConsts.RETURN_RESULT.NG;
                }
            }

            // 再検索処理
            if (!searchList())
            {
                return ComConsts.RETURN_RESULT.NG;
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

        #endregion

        #region privateメソッド
        /// <summary>
        /// 一覧画面 初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initList()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.HiddenId, this.pageInfoList);

            // 非表示情報取得
            var hiddenInfo = TMQUtil.GetHiddenInfoForMaster(structureGroupId, itemListType, this.UserId, db);
            // 工場ID
            hiddenInfo.FactoryId = Const.CommonFactoryId;
            // 対象アイテム一覧
            hiddenInfo.TargetItemList = Master.TargetItemList.Standard;

            // 非表示情報の設定
            if (!SetSearchResultsByDataClass<TMQUtil.HiddenInfoForMaster>(pageInfo, new List<TMQUtil.HiddenInfoForMaster>() { hiddenInfo }, 1))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 登録・修正画面 初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initEdit()
        {
            // 非表示情報取得
            var condition = getHiddenInfo();
            if (condition == null)
            {
                return false;
            }

            // 選択行データ取得
            condition.StructureId = getSelectData(condition);

            // アイテム翻訳検索
            if (!searchItemTran(condition))
            {
                return false;
            }

            if (condition.FormType == (int)Master.FormType.Regist)
            {
                // 登録画面の場合、処理終了
                return true;
            }

            // アイテム情報検索
            if (!searchItemInfo(condition))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 表示順変更画面 初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initOrder()
        {
            // 非表示情報取得
            var condition = getHiddenInfo();
            if (condition == null)
            {
                return false;
            }

            // アイテム表示順一覧検索
            if (!searchItemOrderList(condition))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 非表示情報取得
        /// </summary>
        /// <returns>非表示情報</returns>
        private TMQUtil.HiddenInfoForMaster getHiddenInfo()
        {
            var result = new TMQUtil.HiddenInfoForMaster();

            // 非表示情報のコントロールID
            string ctrlId = ConductInfo.FormList.ControlId.HiddenId;

            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> dicCondition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);
            List<Dictionary<string, object>> dicConditionList = new() { dicCondition };

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // 非表示情報取得
            if (!SetSearchConditionByDataClass(dicConditionList, ctrlId, result, pageInfo))
            {
                return result;
            }

            // 言語IDはユーザ情報の言語IDを設定
            result.LanguageId = getDictionaryKeyValue(dicCondition, "language_id");

            return result;
        }

        /// <summary>
        /// 検索条件取得
        /// </summary>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <returns>検索条件</returns>
        private Dao.SearchCondition getCondition(TMQUtil.HiddenInfoForMaster hiddenInfo)
        {
            var result = new Dao.SearchCondition();

            // 検索条件のコントロールID
            string ctrlId = ConductInfo.FormList.ControlId.SearchId;

            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> dicCondition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);
            List<Dictionary<string, object>> dicConditionList = new() { dicCondition };

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // 検索条件取得
            if (!SetSearchConditionByDataClass(dicConditionList, ctrlId, result, pageInfo))
            {
                return result;
            }

            // 構成グループID
            result.StructureGroupId = hiddenInfo.StructureGroupId;
            // 工場ID
            result.FactoryId = hiddenInfo.FactoryId;
            // 構成階層番号
            result.StructureLayerNo = (int)hiddenInfo.StructureLayerNo;
            // 親構成ID
            result.ParentStructureId = (int)hiddenInfo.ParentStructureId;
            // 言語IDはユーザ情報の言語IDを設定
            result.LanguageId = hiddenInfo.LanguageId;

            return result;
        }

        /// <summary>
        /// 選択行データ取得
        /// </summary>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <returns>選択行の構成ID</returns>
        private int? getSelectData(TMQUtil.HiddenInfoForMaster hiddenInfo)
        {
            if (hiddenInfo.FormType == (int)Master.FormType.Regist)
            {
                // 登録画面の場合
                return null;
            }

            var result = new TMQUtil.SearchResultForMaster();

            // 一覧のコントロールID
            string ctrlId = getCtrlId(hiddenInfo, ConductInfo.FormList.FormNo);

            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> dicCondition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);
            List<Dictionary<string, object>> dicConditionList = new() { dicCondition };

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // 選択行のデータを取得
            if (!SetSearchConditionByDataClass(dicConditionList, ctrlId, result, pageInfo))
            {
                return null;
            }

            return result.StructureId;
        }

        /// <summary>
        /// コントロールID取得
        /// </summary>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="formNo">画面番号</param>
        /// <returns>コントロールID</returns>
        private string getCtrlId(TMQUtil.HiddenInfoForMaster hiddenInfo, int formNo)
        {
            string result = null;

            switch (hiddenInfo.StructureLayerNo)
            {
                case Master.Structure.StructureLayerNo.Layer0:
                    // 地区
                    result = (formNo == ConductInfo.FormList.FormNo) ? ConductInfo.FormList.ControlId.DistrictItemId : ConductInfo.FormEdit.ControlId.DistrictItemInfoId;
                    break;
                case Master.Structure.StructureLayerNo.Layer1:
                    // 工場
                    result = (formNo == ConductInfo.FormList.FormNo) ? ConductInfo.FormList.ControlId.FactoryItemId : ConductInfo.FormEdit.ControlId.FactoryItemInfoId;
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // 非表示情報取得
            var hiddenInfo = getHiddenInfo();
            if (hiddenInfo == null)
            {
                return false;
            }
            // 検索条件取得
            var condition = getCondition(hiddenInfo);
            if (condition == null)
            {
                return false;
            }

            // 地区/工場アイテム一覧の取得＆設定

            // 一覧のコントロールID
            var ctrlId = getCtrlId(hiddenInfo, ConductInfo.FormList.FormNo);

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = ComUtil.GetNotNullNameByClass<Dao.SearchCondition>(condition);

            // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetDistrictFactoryItemList, out string baseSql, listUnComment);

            // 一覧検索実行
            IList<Dao.SearchResult> results = db.GetListByDataClass<Dao.SearchResult>(baseSql, condition);

            if (results != null && results.Count > 0)
            {
                // 検索結果の設定
                SetSearchResultsByDataClass<Dao.SearchResult>(pageInfo, results, results.Count);
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;

            return true;
        }

        /// <summary>
        /// アイテム翻訳検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合False</returns>
        private bool searchItemTran(TMQUtil.HiddenInfoForMaster condition)
        {
            // ページ情報
            var pageInfo = GetPageInfo(ConductInfo.FormEdit.ControlId.ItemTranId, this.pageInfoList);

            // アイテム翻訳一覧取得
            var results = new List<TMQUtil.ItemTranslationForMaster>();
            if (!TMQUtil.GetItemTranListForMaster(condition, GetResMessage("111270026"), ref results, this.db))
            {
                return false;
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<TMQUtil.ItemTranslationForMaster>(pageInfo, results, results.Count))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// アイテム情報検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合False</returns>
        private bool searchItemInfo(TMQUtil.HiddenInfoForMaster condition)
        {
            // 一覧のコントロールID
            var ctrlId = getCtrlId(condition, ConductInfo.FormEdit.FormNo);

            // ページ情報
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // 検索実行
            var results = TMQUtil.SqlExecuteClass.SelectList<Dao.SearchResult>(SqlName.GetLocationStructureItemInfo, SqlName.SubDir, condition, this.db);
            if (results == null)
            {
                return false;
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.SearchResult>(pageInfo, results, 1))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            // アイテムIDの設定

            // ページ情報
            pageInfo = GetPageInfo(ConductInfo.FormEdit.ControlId.ItemId, this.pageInfoList);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.SearchResult>(pageInfo, results, 1))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// アイテム表示順一覧検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合False</returns>
        private bool searchItemOrderList(TMQUtil.HiddenInfoForMaster condition)
        {
            // ページ情報
            var pageInfo = GetPageInfo(ConductInfo.FormOrder.ControlId.ItemOrderId, this.pageInfoList);

            // 検索実行
            var results = TMQUtil.SqlExecuteClass.SelectList<TMQUtil.SearchResultForMaster>(SqlName.GetLocationStructureIItemOrderList, SqlName.SubDir, condition, this.db);

            // 総件数のチェック
            if (results == null || results.Count == 0)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 「該当データがありません。」
                this.MsgId = GetResMessage(ComRes.ID.ID941060001);
                return false;
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<TMQUtil.SearchResultForMaster>(pageInfo, results, results.Count))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdit()
        {
            // 非表示情報取得
            var hiddenInfo = getHiddenInfo();
            if (hiddenInfo == null)
            {
                return false;
            }

            if (hiddenInfo.FormType == (int)Master.FormType.Edit)
            {
                // 修正画面

                // 排他チェック
                if (isErrorExclusive(hiddenInfo))
                {
                    return false;
                }
            }

            // 入力チェック
            if (isErrorRegist(hiddenInfo))
            {
                return false;
            }

            // 登録処理
            if (!registDb(hiddenInfo))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 表示順変更画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistOrder()
        {
            // 非表示情報取得
            var hiddenInfo = getHiddenInfo();
            if (hiddenInfo == null)
            {
                return false;
            }

            // 排他チェック
            if (isErrorExclusiveByUpdateTime(hiddenInfo, Master.ConductInfo.FormOrder.ControlId.ItemOrderId))
            {
                return false;
            }

            // 登録するデータクラスを取得
            DateTime now = DateTime.Now;
            List<ComDao.MsStructureOrderEntity> registInfoList = getRegistInfoList<ComDao.MsStructureOrderEntity>(ConductInfo.FormOrder.ControlId.ItemOrderId, now);

            // 工場ID、表示順の設定
            int order = 1;
            foreach (ComDao.MsStructureOrderEntity registInfo in registInfoList)
            {
                // 工場ID
                registInfo.FactoryId = hiddenInfo.FactoryId;
                // 表示順設定
                registInfo.DisplayOrder = order;
                // 表示順カウントアップ
                order++;
            }

            // 工場別アイテム表示順マスタ削除
            if (!TMQUtil.DeleteDb(hiddenInfo, Master.SqlName.DeleteLayersStructureOrder, this.db, Master.SqlName.ComLayersDir))
            {
                return false;
            }

            // 工場別アイテム表示順マスタ登録
            foreach (ComDao.MsStructureOrderEntity registInfo in registInfoList)
            {
                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.InsertMsStructureOrder, Master.SqlName.SubDir, registInfo, db))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive(TMQUtil.HiddenInfoForMaster hiddenInfo)
        {
            // アイテム翻訳一覧のコントロールID
            string ctrlId = ConductInfo.FormEdit.ControlId.ItemTranId;

            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(ctrlId);
            var lockKeyMaps = GetLockKeyMaps(ctrlId);

            // 翻訳マスタの排他チェック
            var list = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId, true);
            var targetList = new List<Dictionary<string, object>>();
            foreach (Dictionary<string, object> dic in list)
            {
                // 翻訳マスタ未登録データは除外
                var value = getDictionaryKeyValue(dic, "translation_text_bk");
                if (value != null && value.Length > 0)
                {
                    targetList.Add(dic);
                }
            }
            if (!checkExclusiveList(ctrlId, targetList))
            {
                // エラーの場合
                return true;
            }

            // アイテム情報のコントロールID
            ctrlId = getCtrlId(hiddenInfo, ConductInfo.FormEdit.FormNo);

            // 排他ロック用マッピング情報取得
            lockValMaps = GetLockValMaps(ctrlId);
            lockKeyMaps = GetLockKeyMaps(ctrlId);

            // 構成マスタの排他チェック
            if (!checkExclusiveSingle(ctrlId))
            {
                // エラーの場合
                return true;
            }

            // 排他チェックOK
            return false;
        }

        /// <summary>
        /// 排他チェック(更新日時比較)
        /// </summary>
        /// <param name="condition">非表示情報</param>
        /// <param name="ctrlId">コントロールID</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusiveByUpdateTime(TMQUtil.HiddenInfoForMaster condition, string ctrlId)
        {
            // 指定されたコントロールIDの結果情報のみ抽出
            List<Dictionary<string, object>> dicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);
            // Daoクラスへセット
            TMQUtil.SearchResultForMaster dispRow = new();
            SetDataClassFromDictionary(dicList.First(), ctrlId, dispRow);

            // 最新の更新日時のリストを取得

            // 検索実行
            var newList = TMQUtil.SqlExecuteClass.SelectList<TMQUtil.SearchResultForMaster>(SqlName.GetLocationStructureIItemOrderList, SqlName.SubDir, condition, this.db);

            // 先頭行のみ取得
            var newRow = newList.First();

            DateTime? dispDateTime = dispRow.OrderUpdateDatetime;
            DateTime? newDateTime = newRow.OrderUpdateDatetime;

            // 更新日時で排他チェック
            if (!CheckExclusiveStatusByUpdateDatetime(dispDateTime, newDateTime))
            {
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
        /// <param name="hiddenInfo">非表示情報</param>
        /// <returns>入力チェックエラーがある場合True</returns>
        private bool isErrorRegist(TMQUtil.HiddenInfoForMaster hiddenInfo)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // アイテム翻訳入力チェック
            if (isErrorRegistForItemTranList(hiddenInfo, ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            // 変更管理承認者とExcelPort使用権限の関連チェック
            if (isErrorRelationHistoryAndExcelPort(hiddenInfo, ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            // 工場を登録する際、変更管理の承認者が設定されている場合、ユーザの権限を判定
            // ゲストかシステム管理者の場合エラー
            if (isErrorApprovalUserError(hiddenInfo, ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            /// <summary>
            /// アイテム翻訳 入力チェック
            /// </summary>
            /// <param name="hiddenInfo">非表示情報</param>
            /// <param name="errorInfoDictionary">エラー情報</param>
            /// <returns>入力チェックエラーがある場合True</returns>
            bool isErrorRegistForItemTranList(TMQUtil.HiddenInfoForMaster hiddenInfo, ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                bool isError = false;   // 処理全体でエラーの有無を保持

                // 対象コントロールID
                var ctrlId = ConductInfo.FormEdit.ControlId.ItemTranId;
                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(ctrlId);

                // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                // 画面に表示されている(=削除されていない)項目を取得
                var targetDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);

                // アイテム翻訳一覧取得
                var itemTranList = getItemTranList();

                // 一覧の件数分絞り込み
                foreach (var rowDic in targetDicList)
                {
                    // Dictionaryをデータクラスに変換
                    TMQUtil.ItemTranslationForMaster result = new();
                    SetDataClassFromDictionary(rowDic, ctrlId, result);
                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(rowDic);
                    bool isErrorRow = false; // 行単位でエラーの有無を保持

                    // 標準アイテムはチェック対象外
                    if (result.Num == 0)
                    {
                        continue;
                    }

                    // 構成階層番号
                    result.StructureLayerNo = hiddenInfo.StructureLayerNo;
                    // 親構成ID
                    result.ParentStructureId = hiddenInfo.ParentStructureId;

                    // 必須チェック
                    if (TMQUtil.CheckRequiredByItemTran(hiddenInfo, result, itemTranList))
                    {
                        // 未入力の場合、エラー
                        isErrorRow = true;
                        string errMsg = GetResMessage("941220009");     // 入力して下さい。
                        string val = info.getValName("item_tran_name");
                        errorInfo.setError(errMsg, val);
                    }

                    // 重複チェック
                    var errFlg = false;
                    if (!MS1000_CheckDuplicateByItemTran(hiddenInfo, result, ref errFlg))
                    {
                        return false;
                    }

                    if (errFlg)
                    {
                        // アイテム翻訳が既に登録済みの場合、エラー
                        isErrorRow = true;
                        string errMsg = GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" });     // アイテム翻訳は既に登録されています。
                        string val = info.getValName("item_tran_name");
                        errorInfo.setError(errMsg, val);
                    }

                    if (isErrorRow)
                    {
                        // 行でエラーのあった場合、エラー情報を設定する
                        errorInfoDictionary.Add(errorInfo.Result);
                        // 「入力エラーがあります。」
                        this.MsgId = GetResMessage(ComRes.ID.ID941220005);
                        isError = true;
                    }
                }
                return isError;

                /// <summary>
                /// 登録・修正画面 アイテム翻訳重複チェック
                /// </summary>
                /// <param name="hiddenInfo">非表示情報</param>
                /// <param name="targetInfo">アイテム翻訳情報</param>
                /// <param name="errFlg">エラー有無(エラーの場合true)</param>
                /// <param name="db">DB操作クラス</param>
                /// <returns>エラーの場合False</returns>
                bool MS1000_CheckDuplicateByItemTran(
                    TMQUtil.HiddenInfoForMaster hiddenInfo,
                    TMQUtil.ItemTranslationForMaster targetInfo,
                    ref bool errFlg)
                {
                    if (string.IsNullOrEmpty(targetInfo.TranslationText))
                    {
                        // アイテム翻訳が未入力の場合、チェック対象外
                        return true;
                    }
                    if (targetInfo.TranslationText == targetInfo.TranslationTextBk)
                    {
                        // アイテム翻訳に変更がない場合、チェック対象外
                        return true;
                    }

                    // 構成グループに同じ翻訳が存在するか検索

                    // 件数取得
                    int cnt = 0;
                    if (!TMQUtil.GetCountDb(targetInfo, Master.SqlName.GetCountLayersTranslation, ref cnt, db, Master.SqlName.ComLayersDir))
                    {
                        return false;
                    }

                    if (cnt > 0)
                    {
                        // アイテム翻訳が既に登録済みの場合、エラー
                        errFlg = true;
                    }

                    return true;
                }
            }

            /// <summary>
            /// 承認者の入力チェック
            /// </summary>
            /// <param name="hiddenInfo">非表示情報</param>
            /// <param name="errorInfoDictionary">エラー情報</param>
            /// <returns>入力チェックエラーがある場合True</returns>
            bool isErrorApprovalUserError(TMQUtil.HiddenInfoForMaster hiddenInfo, ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                if (!isFactory(hiddenInfo))
                {
                    // 工場の登録でない場合、終了
                    return false;
                }

                // 以下、工場の登録の場合

                // 承認者を取得
                var ctrlId = ConductInfo.FormEdit.ControlId.FactoryItemInfoId; // 対象コントロールID
                // 承認者のユーザIDを取得
                Dao.SearchResult inputData = GetFormDataByCtrlId<Dao.SearchResult>(ctrlId);
                string approvalUserId = inputData.ExData4;
                if (string.IsNullOrEmpty(approvalUserId))
                {
                    // 承認者が未指定の場合、終了
                    return false;
                }
                // 承認者が指定されている場合
                // 指定されたユーザの権限を確認
                var authInfo = TMQUtil.SqlExecuteClass.SelectEntity<TMQUtil.StructureItemEx.StructureItemExInfo>(
                    SqlName.AuthCheck.GetAuthLevel, SqlName.AuthCheck.SubDir, new { ApprovalUserId = approvalUserId }, this.db);
                if (authInfo == null || authInfo.ExData == null)
                {
                    // 取得できない場合はエラー(ユーザIDでない場合？運用では到達しないはず)
                    setErrorInfo(ctrlId, ref errorInfoDictionary);
                    return true;
                }
                // 権限を判定
                string guest = convertAuthLevetString(TMQConst.MsStructure.StructureId.AuthLevel.Guest);
                string admin = convertAuthLevetString(TMQConst.MsStructure.StructureId.AuthLevel.SystemAdministrator);
                if (authInfo.ExData == guest || authInfo.ExData == admin)
                {
                    // ゲストかシステム管理者の場合にエラー
                    setErrorInfo(ctrlId, ref errorInfoDictionary);
                    return true;
                }

                return false;

                // エラー情報をセット
                void setErrorInfo(string ctrlId, ref List<Dictionary<string, object>> errorInfoDictionary)
                {
                    var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                    var info = getResultMappingInfo(ctrlId);  // エラー情報を画面に設定するためのマッピング情報リスト
                    string errMsg = GetResMessage(ComRes.ID.ID141290009);     // 変更管理の承認者は一般ユーザか特権ユーザを指定してください。
                    string val = info.getValName("approval_user");
                    errorInfo.setError(errMsg, val);
                    errorInfoDictionary.Add(errorInfo.Result);
                }

                // 権限を文字列に変換
                string convertAuthLevetString(TMQConst.MsStructure.StructureId.AuthLevel authLevel)
                {
                    int authLevelValue = (int)authLevel;
                    return authLevelValue.ToString();
                }
            }

            /// <summary>
            /// 変更管理承認者とExcelPort使用権限の関連チェック
            /// </summary>
            /// <param name="hiddenInfo">非表示情報</param>
            /// <param name="errorInfoDictionary">エラー情報</param>
            /// <returns>入力チェックエラーがある場合True</returns>
            bool isErrorRelationHistoryAndExcelPort(TMQUtil.HiddenInfoForMaster hiddenInfo, ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                if (!isFactory(hiddenInfo))
                {
                    // 工場の登録でない場合、終了
                    return false;
                }

                // 入力内容を取得
                var ctrlId = ConductInfo.FormEdit.ControlId.FactoryItemInfoId; // 対象コントロールID
                Dao.SearchResult inputData = GetFormDataByCtrlId<Dao.SearchResult>(ctrlId);

                // 変更管理承認者とExcelPort使用権限がどちらも入力されている場合はエラーとする
                if (!string.IsNullOrEmpty(inputData.ExData4) &&
                    !string.IsNullOrEmpty(inputData.ExData5))
                {
                    setErrorInfo(ctrlId, ref errorInfoDictionary);
                    return true;
                }

                return false;

                // エラー情報をセット
                void setErrorInfo(string ctrlId, ref List<Dictionary<string, object>> errorInfoDictionary)
                {
                    var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                    var info = getResultMappingInfo(ctrlId);  // エラー情報を画面に設定するためのマッピング情報リスト
                    string errMsg = GetResMessage(ComRes.ID.ID141290010);     // 変更管理承認者とExcelPort使用権限は同時に設定できません。
                    // 変更管理承認者
                    string val = info.getValName("approval_user");
                    errorInfo.setError(errMsg, val);
                    errorInfoDictionary.Add(errorInfo.Result);
                    // ExcelPort使用権限
                    val = info.getValName("excelport_auth");
                    errorInfo.setError(errMsg, val);
                    errorInfoDictionary.Add(errorInfo.Result);
                }
            }
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <returns>エラーの場合False</returns>
        private bool registDb(TMQUtil.HiddenInfoForMaster hiddenInfo)
        {
            DateTime now = DateTime.Now;
            int structureId = -1;
            int transId = -1;
            int itemId = -1;

            // アイテム翻訳一覧取得
            var itemTranList = getItemTranList();
            // アイテム情報取得
            var itemInfo = getItemInfo(hiddenInfo);

            // 翻訳マスタ登録
            if (!registTranslation(now, hiddenInfo, itemTranList, ref transId))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!registItem(now, hiddenInfo, itemTranList, transId, ref itemId))
            {
                return false;
            }

            // アイテムマスタ拡張登録
            if (!registItemEx(now, hiddenInfo, itemInfo, itemId))
            {
                return false;
            }

            // 構成マスタ登録
            if (!registStructure(now, hiddenInfo, itemInfo, itemId, ref structureId))
            {
                return false;
            }

            //// 工場別未使用標準アイテムマスタ登録
            //if (!registItemUnused(now, hiddenInfo, itemInfo))
            //{
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// アイテム翻訳一覧取得
        /// </summary>
        /// <returns>アイテム翻訳一覧</returns>
        private List<TMQUtil.ItemTranslationForMaster> getItemTranList()
        {
            // アイテム翻訳一覧のコントロールID
            string ctrlId = ConductInfo.FormEdit.ControlId.ItemTranId;
            // 指定されたコントロールIDの結果情報のみ抽出
            List<Dictionary<string, object>> targetDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);

            // データクラスに変換
            var results = convertDicListToClassList<TMQUtil.ItemTranslationForMaster>(targetDicList, ctrlId);
            // 標準アイテムを除く
            var targetResults = results.Where(x => x.Num > 0).ToList();

            return targetResults;
        }

        /// <summary>
        /// アイテム情報取得
        /// </summary>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <returns>アイテム情報</returns>
        private Dao.SearchResult getItemInfo(TMQUtil.HiddenInfoForMaster hiddenInfo)
        {
            // アイテム情報のコントロールID
            string ctrlId = getCtrlId(hiddenInfo, ConductInfo.FormEdit.FormNo);
            // 指定されたコントロールIDの結果情報のみ抽出
            List<Dictionary<string, object>> targetDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);

            // データクラスに変換
            var results = convertDicListToClassList<Dao.SearchResult>(targetDicList, ctrlId);

            return results.FirstOrDefault();
        }

        /// <summary>
        /// 翻訳マスタ登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemTranList">アイテム翻訳一覧</param>
        /// <param name="tranId">翻訳ID</param>
        /// <returns>エラーの場合False</returns>
        private bool registTranslation(DateTime now, TMQUtil.HiddenInfoForMaster hiddenInfo, List<TMQUtil.ItemTranslationForMaster> itemTranList, ref int tranId)
        {
            // 翻訳マスタ登録
            if (!TMQUtil.RegistTranslation(now, hiddenInfo, itemTranList, this.UserId, ref tranId, this.db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// アイテムマスタ登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemTranList">アイテム翻訳一覧</param>
        /// <param name="tranId">翻訳ID</param>
        /// <param name="itemId">アイテムID</param>
        /// <returns>エラーの場合False</returns>
        private bool registItem(DateTime now, TMQUtil.HiddenInfoForMaster hiddenInfo, List<TMQUtil.ItemTranslationForMaster> itemTranList, int tranId, ref int itemId)
        {
            // アイテムマスタ登録
            bool isExclusiveError = false;
            if (!TMQUtil.RegistItem(now, hiddenInfo, itemTranList, tranId, this.UserId, ref itemId, ref isExclusiveError, this.db))
            {
                if (isExclusiveError)
                {
                    // 排他エラー
                    setExclusiveError();
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// アイテムマスタ拡張登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemInfo">アイテム情報</param>
        /// <param name="itemId">アイテムID</param>
        /// <returns>エラーの場合False</returns>
        private bool registItemEx(DateTime now, TMQUtil.HiddenInfoForMaster hiddenInfo, Dao.SearchResult itemInfo, int itemId)
        {
            // 拡張項目数
            var itemExCnt = (hiddenInfo.StructureLayerNo == Master.Structure.StructureLayerNo.Layer0) ? ItemExCnt.District : ItemExCnt.Factory;

            // アイテムマスタ拡張登録
            bool isExclusiveError = false;
            if (!TMQUtil.RegistItemEx<Dao.SearchResult>(now, hiddenInfo, itemInfo, itemId, this.UserId, itemExCnt, ref isExclusiveError, this.db))
            {
                if (isExclusiveError)
                {
                    // 排他エラー
                    setExclusiveError();
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// 構成マスタ登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemInfo">アイテム情報</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="structureId">構成ID</param>
        /// <returns>エラーの場合False</returns>
        private bool registStructure(DateTime now, TMQUtil.HiddenInfoForMaster hiddenInfo, Dao.SearchResult itemInfo, int itemId, ref int structureId)
        {
            if (isFactory(hiddenInfo))
            {
                // 工場アイテムの場合

                if (hiddenInfo.FormType == Master.FormType.Regist)
                {
                    // 登録画面

                    // 工場アイテム構成マスタ登録
                    if (!RegistStructureByFactoryItem(ref structureId))
                    {
                        return false;
                    }

                    return true;
                }
            }

            // 構成マスタ登録
            if (!TMQUtil.RegistStructure<Dao.SearchResult>(now, hiddenInfo, itemInfo, itemId, this.UserId, ref structureId, this.db))
            {
                return false;
            }

            // 削除する場合、子のアイテムも削除する
            if (hiddenInfo.FormType == Master.FormType.Edit && itemInfo.DeleteFlg)
            {
                if (!TMQUtil.UpdateChildLayers(now, hiddenInfo, itemInfo.StructureId, this.UserId, this.db))
                {
                    return false;
                }
            }

            return true;

            /// <summary>
            /// 工場アイテム構成マスタ登録
            /// </summary>
            /// <param name="structureId">構成ID</param>
            /// <returns>エラーの場合False</returns>
            bool RegistStructureByFactoryItem(
                ref int structureId)
            {
                // 登録するデータクラスを作成
                ComDao.MsStructureEntity registInfo = new();
                registInfo.StructureGroupId = hiddenInfo.StructureGroupId;
                registInfo.ParentStructureId = hiddenInfo.ParentStructureId;
                registInfo.StructureLayerNo = hiddenInfo.StructureLayerNo;
                registInfo.StructureItemId = itemId;
                registInfo.DeleteFlg = false;
                TMQUtil.SetCommonDataBaseClass(now, ref registInfo, this.UserId, this.UserId);

                // 構成マスタ新規登録

                // 構成ID取得
                structureId = TMQUtil.SqlExecuteClass.SelectEntity<int>(SqlName.GetStructureId, SqlName.SubDir, null, this.db);

                // 構成ID
                registInfo.StructureId = structureId;
                // 工場ID
                registInfo.FactoryId = structureId;

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.InsertLocationStructureInfo, SqlName.SubDir, registInfo, this.db))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 工場別未使用標準アイテムマスタ登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemInfo">アイテム情報</param>
        /// <returns>エラーの場合False</returns>
        private bool registItemUnused(DateTime now, TMQUtil.HiddenInfoForMaster hiddenInfo, Dao.SearchResult itemInfo)
        {
            // 工場別未使用標準アイテムマスタ登録
            if (!TMQUtil.RegistItemUnused<Dao.SearchResult>(now, hiddenInfo, itemInfo, this.UserId, this.db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 非表示情報より階層番号を取得し、工場の登録かどうかを判定
        /// </summary>
        /// <param name="hiddenInfo">非表示項目の内容</param>
        /// <returns>工場の場合True</returns>
        private bool isFactory(TMQUtil.HiddenInfoForMaster hiddenInfo)
        {
            return hiddenInfo.StructureLayerNo == Master.Structure.StructureLayerNo.Layer1;
        }
        #endregion

    }
}