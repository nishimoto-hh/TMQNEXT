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
using Dao = BusinessLogic_MS1040.BusinessLogicDataClass_MS1040;
using Master = CommonTMQUtil.CommonTMQUtil.ComMaster;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_MS1040
{
    /// <summary>
    /// 予備品ロケーション（予備品倉庫・棚）マスタ
    /// </summary>
    public class BusinessLogic_MS1040 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 構成グループID
        /// </summary>
        private static int structureGroupId = 1040;

        /// <summary>
        /// 拡張項目件数
        /// </summary>
        private static int itemExCnt = 0;

        /// <summary>
        /// アイテム一覧タイプ
        /// </summary>
        private static int itemListType = (int)TMQUtil.ComMaster.ItemListType.Factory;

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            /// <summary>ExcelPortアップロード</summary>
            public const string ExcelPortUpload = "LIST_000_1";
        }

        /// <summary>
        /// ExcelPortシート情報
        /// </summary>
        public static class ExcelPortMasterListInfo
        {
            /// <summary>
            /// /データ開始行
            /// </summary>
            public const int StartRowNo = 4;
            /// <summary>
            /// 送信時処理列番号
            /// </summary>
            public const int ProccesColumnNo = 4;
            /// <summary>
            /// 倉庫ID
            /// </summary>
            public const int WarehouseNo = 24;
            /// <summary>
            /// 倉庫名
            /// </summary>
            public const int WarehouseName = 25;
            /// <summary>
            /// 工場番号
            /// </summary>
            public const int WarehouseParentNumber = 26;
            /// <summary>
            /// 棚ID
            /// </summary>
            public const int RackNo = 36;
            /// <summary>
            /// 棚名
            /// </summary>
            public const int RackName = 37;
            /// <summary>
            /// 倉庫番号
            /// </summary>
            public const int RackParentNumber = 38;
        }

        /// <summary>
        /// ExcelPortアップロード用リスト
        /// </summary>
        public class ExcelPortStructureList
        {
            /// <summary>
            /// 構成ID
            /// </summary>
            public long? StructureId { get; set; }
            /// <summary>
            /// 親構成ID
            /// </summary>
            /// <value>
            public long? ParentId;
        }

        /// <summary>
        /// 画面タイプ
        /// </summary>
        private class FormType
        {
            /// <summary>登録画面</summary>
            public const int Regist = 1;
            /// <summary>修正画面</summary>
            public const int Edit = 2;
        }
        /// <summary>
        /// アイテム一覧タイプ
        /// </summary>
        private class ItemListType
        {
            /// <summary>標準アイテム一覧</summary>
            public const int Standard = 1;
            /// <summary>工場アイテム一覧</summary>
            public const int Factory = 2;
            /// <summary>標準・工場アイテム一覧</summary>
            public const int StandardFactory = 3;
        }

        /// <summary>
        /// 対象アイテム一覧
        /// </summary>
        private class TargetItemList
        {
            /// <summary>標準アイテム一覧</summary>
            public const int Standard = 1;
            /// <summary>工場アイテム一覧</summary>
            public const int Factory = 2;
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private class SqlName
        {
            /// <summary>SQL名：予備品ロケーションアイテム一覧取得(工場アイテム)</summary>
            public const string GetSpareLocationStructureFactoryItemList = "GetSpareLocationStructureFactoryItemList";
            /// <summary>SQL名：予備品ロケーションアイテム情報取得</summary>
            public const string GetSpareLocationStructureItemInfo = "GetSpareLocationStructureItemInfo";
            /// <summary>SQL名：構成ID取得</summary>
            public const string GetStructureId = "GetStructureId";
            ///// <summary>SQL名：予備品ロケーションアイテム表示順一覧取得</summary>
            public const string GetSpareLocationStructureIItemOrderList = "GetSpareLocationStructureItemOrderList";
            /// <summary>SQL名：翻訳マスタ表示順削除</summary>
            public const string DeleteLayersStructureOrder = "DeleteLayersStructureOrder";
            /// <summary>SQL名：翻訳マスタ削除</summary>
            public const string UpdateLayerMsStructureInfoAddDeleteFlg = "UpdateLayerMsStructureInfoAddDeleteFlg";
            /// <summary>SQL名：翻訳マスタ件数取得</summary>
            public const string GetCountLayersTranslation = "GetCountLayersTranslation";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = Master.SqlName.SubDir + @"\SpareLocationStructure";
            public const string ComLayersDir = Master.SqlName.SubDir + @"\ComLayers";
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
                    /// 標準アイテム一覧の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string StarndardItemId = "BODY_020_00_LST_0";
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
                    /// アイテム情報の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string ItemInfoId = "BODY_020_00_LST_1";
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
        public BusinessLogic_MS1040() : base()
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
            string ctrlId = Master.ConductInfo.FormList.ControlId.StarndardItemId;
            var list = getSelectedRowsByList(this.resultInfoDictionary, ctrlId);
            if (list == null || list.Count == 0)
            {
                ctrlId = Master.ConductInfo.FormList.ControlId.FactoryItemId;
                list = getSelectedRowsByList(this.resultInfoDictionary, ctrlId);
                if (list == null || list.Count == 0)
                {
                    // 選択行が無ければエラー
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941160003 });
                    return ComConsts.RETURN_RESULT.NG;
                }
            }

            // 一覧のチェックされた行のレコードを削除する
            // 削除SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.ComLayersDir, SqlName.UpdateLayerMsStructureInfoAddDeleteFlg, out string sql);
            // 削除処理実行
            if (!DeleteSelectedList<TMQUtil.SearchResultForMaster>(ctrlId, sql))
            {
                setError();
                return ComConsts.RETURN_RESULT.NG;
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

        #region ExcelPort
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

            // 検索条件を作成
            TMQUtil.CommonExcelPortMasterCondition searchCondition = getSearchCondition();

            // ページ情報取得
            var pageInfo = GetPageInfo(Master.ConductInfo.FormList.ControlId.HiddenId, this.pageInfoList);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, CommonColumnName.LocationId, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
            {
                // 「ダウンロード処理に失敗しました。」
                resultMsg = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                return ComConsts.RETURN_RESULT.NG;
            }

            // 検索処理
            if (!getDataList(ref excelPort, searchCondition, out IList<Dictionary<string, object>> dataList))
            {
                // 「ダウンロード処理に失敗しました。」
                resultMsg = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                return ComConsts.RETURN_RESULT.NG;
            }

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
        /// 検索条件を作成
        /// </summary>
        /// <returns>検索条件</returns>
        private TMQUtil.CommonExcelPortMasterCondition getSearchCondition()
        {
            // 検索条件初期化
            TMQUtil.CommonExcelPortMasterCondition condition = new();

            // 親画面(EP0001)の定義情報を追加
            AddMappingListOtherPgmId(TMQUtil.ConductIdEP0001);
            string fromCtrlId = Master.ConductInfo.FormExcelPortDownCondition.ControlId.Condition;
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, fromCtrlId);
            // 条件画面で選択された値を取得
            SetDataClassFromDictionary(targetDic, fromCtrlId, condition, new List<string> { "MaintenanceTarget", "FactoryId" });

            // 条件を設定
            condition.LanguageId = this.LanguageId;                                         // 言語ID
            condition.StructureGroupId = structureGroupId;                                  // 構成グループID
            condition.MasterTransLationId = Master.MasterNameTranslation[structureGroupId]; // マスタ名称の翻訳ID
            condition.LayerIdList = new()
            {
                (int)TMQConst.MsStructure.StructureLayerNo.SpareLocation.Warehouse,
                (int)TMQConst.MsStructure.StructureLayerNo.SpareLocation.Rack
            };

            // メンテナンス対象コンボボックスで選択されたアイテムを取得
            TMQUtil.StructureItemEx.StructureItemExInfo param = new()
            {
                StructureGroupId = Master.MaintainanceTargetExInfo.StructureGroupId, //構成グループID
                Seq = Master.MaintainanceTargetExInfo.Seq,                           // 連番
            };
            List<TMQUtil.StructureItemEx.StructureItemExInfo> exdataList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);

            // 拡張項目を設定(1：マスタアイテム、2：標準アイテム未使用設定、3：マスタ並び順設定)
            condition.MaintenanceTargetNo = exdataList.Where(x => x.StructureId == condition.MaintenanceTarget).Select(x => x.ExData).FirstOrDefault();

            return condition;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="excelPort">ExcelPortクラス</param>
        /// <param name="searchCondition">検索条件</param>
        /// <param name="dataList">検索結果</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool getDataList(ref TMQUtil.ComExcelPort excelPort, TMQUtil.CommonExcelPortMasterCondition searchCondition, out IList<Dictionary<string, object>> dataList)
        {
            dataList = null;

            // 一時テーブル作成
            TMQUtil.createTempTblExcelPort(structureGroupId, this.db, this.LanguageId);

            // メンテナンス対象コンボボックスで選択されたアイテムに応じて検索
            switch (searchCondition.MaintenanceTargetNo)
            {
                case Master.MaintainanceTargetExInfo.ExData.MasterItem: // マスタアイテム

                    // 一覧検索実行
                    IList<TMQUtil.CommonExcelPortMasterPartsList> masterReaults = getMasterResults(searchCondition);
                    if (masterReaults == null)
                    {
                        return false;
                    }

                    // Dicitionalyに変換
                    dataList = ComUtil.ConvertClassToDictionary<TMQUtil.CommonExcelPortMasterPartsList>(masterReaults);
                    break;

                case Master.MaintainanceTargetExInfo.ExData.Oerder: // マスタ並び順設定

                    // SQLを取得
                    TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortStructureItemOrderList, out string ordersSql);

                    // 一覧検索実行
                    searchCondition.ProcessId = TMQConst.SendProcessId.Update; // 送信時処理を設定
                    IList<TMQUtil.CommonExcelPortMasterOrderList> orderResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterOrderList>(ordersSql, searchCondition);
                    if (orderResults == null)
                    {
                        return false;
                    }

                    // Dicitionalyに変換
                    dataList = ComUtil.ConvertClassToDictionary<TMQUtil.CommonExcelPortMasterOrderList>(orderResults);

                    // 出力対象のシート番号を「並び順」用に変更
                    excelPort.DownloadCondition.HideSheetNo = Master.OrdeerSheetNo;
                    break;

                default:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// マスタアイテム検索処理
        /// </summary>
        /// <param name="searchCondition">検索条件</param>
        /// <returns>検索結果</returns>
        private List<TMQUtil.CommonExcelPortMasterPartsList> getMasterResults(TMQUtil.CommonExcelPortMasterCondition searchCondition)
        {
            // 地区情報を取得
            if (!getDistrictResults(out IList<TMQUtil.CommonExcelPortMasterStructureList> districtResults, out Dictionary<long?, long?> dicDistrict))
            {
                return new List<TMQUtil.CommonExcelPortMasterPartsList>();
            }

            // 工場情報を取得
            if (!getFactoryResults(dicDistrict, out IList<TMQUtil.CommonExcelPortMasterStructureList> factoryResults, out List<long> factoryIdList, out Dictionary<long?, long?> dicFactory))
            {
                return new List<TMQUtil.CommonExcelPortMasterPartsList>();
            }

            // 工場IDリストを設定
            searchCondition.FactoryIdList = factoryIdList;

            // 予備品倉庫情報を取得
            if (!getWarehouseResults(dicFactory, out IList<TMQUtil.CommonExcelPortMasterPartsList> warehouseResults, out Dictionary<long?, long?> dicWarehouse))
            {
                return new List<TMQUtil.CommonExcelPortMasterPartsList>();
            }

            // 棚情報を取得
            if (!getRackResults(dicWarehouse, out IList<TMQUtil.CommonExcelPortMasterPartsList> rackResults))
            {
                return new List<TMQUtil.CommonExcelPortMasterPartsList>();
            }

            // 取得した「地区」「工場」「予備品倉庫」「棚」のうち、最大のデータ件数を出力データのレコード数とする
            int[] dataCnts = new int[]
            { districtResults.Count,
              factoryResults.Count,
              warehouseResults.Count,
              rackResults.Count
            };
            int recordNum = dataCnts.Max();

            // 取得データを1レコード単位にまとめる
            List<TMQUtil.CommonExcelPortMasterPartsList> results = new();
            for (int i = 0; i < recordNum; i++)
            {
                TMQUtil.CommonExcelPortMasterPartsList record = new();

                // 地区情報
                if (districtResults.Count != 0)
                {
                    record.StructureGroupId = districtResults[0].StructureGroupId; // 構成グループID
                    record.DistrictNumber = districtResults[0].DistrictNumber;     // 地区番号
                    record.DistrictId = districtResults[0].DistrictId;             // 地区ID(構成ID)
                    record.DistrictName = districtResults[0].DistrictName;         // 地区名
                    districtResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 工場情報
                if (factoryResults.Count != 0)
                {
                    record.FactoryNumber = factoryResults[0].FactoryNumber;             // 工場番号
                    record.FactoryId = factoryResults[0].FactoryId;                     // 工場ID(構成ID)
                    record.FactoryName = factoryResults[0].FactoryName;                 // 工場名
                    record.FactoryParentId = factoryResults[0].FactoryParentId;         // 工場の親構成ID
                    record.FactoryParentNumber = factoryResults[0].FactoryParentNumber; // 地区番号
                    factoryResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 予備品倉庫情報
                if (warehouseResults.Count != 0)
                {
                    record.WarehouseItemId = warehouseResults[0].WarehouseItemId;                       // 予備品倉庫アイテムID
                    record.WarehouseId = warehouseResults[0].WarehouseId;                               // 予備品倉庫ID(構成ID)
                    record.WarehouseItemTranslationId = warehouseResults[0].WarehouseItemTranslationId; // 翻訳ID(予備品倉庫)
                    record.WarehouseNumber = warehouseResults[0].WarehouseNumber;                       // 予備品倉庫番号
                    record.WarehouseName = warehouseResults[0].WarehouseName;                           // 予備品倉庫名
                    record.WarehouseNameBefore = warehouseResults[0].WarehouseNameBefore;               // 予備品倉庫名
                    record.WarehouseParentId = warehouseResults[0].WarehouseParentId;                   // 予備品倉庫の親構成ID
                    record.WarehouseParentNumber = warehouseResults[0].WarehouseParentNumber;           // 工場番号
                    record.WarehouseParentNumberBefore = warehouseResults[0].WarehouseParentNumber;     // 工場番号
                    warehouseResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 棚情報
                if (rackResults.Count != 0)
                {
                    record.RackItemId = rackResults[0].RackItemId;                       // 棚アイテムID
                    record.RackId = rackResults[0].RackId;                               // 棚ID(構成ID)
                    record.RackItemTranslationId = rackResults[0].RackItemTranslationId; // 翻訳ID(棚)
                    record.RackNumber = rackResults[0].RackNumber;                       // 棚番号
                    record.RackName = rackResults[0].RackName;                           // 棚名
                    record.RackNameBefore = rackResults[0].RackNameBefore;               // 棚名
                    record.RackParentId = rackResults[0].RackParentId;                   // 棚の親構成ID
                    record.RackParentNumber = rackResults[0].RackParentNumber;           // 予備品倉庫番号
                    record.RackParentNumberBefore = rackResults[0].RackParentNumber;     // 予備品倉庫番号
                    rackResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 出力データ行に追加
                results.Add(record);
            }

            return results;

            // 地区情報を取得
            bool getDistrictResults(out IList<TMQUtil.CommonExcelPortMasterStructureList> districtResults, out Dictionary<long?, long?> dicDistrict)
            {
                districtResults = null;
                dicDistrict = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterDistrictList, out string districtSql);

                // SQL実行
                districtResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterStructureList>(districtSql, searchCondition);
                if (districtResults == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 取得した地区情報より、「地区の構成ID」「地区番号」のディクショナリを作成
                foreach (TMQUtil.CommonExcelPortMasterStructureList districtResult in districtResults)
                {
                    dicDistrict.Add(districtResult.DistrictId, districtResult.DistrictNumber);
                }

                return true;
            }

            // 工場情報を取得
            bool getFactoryResults(Dictionary<long?, long?> dicDistrict, out IList<TMQUtil.CommonExcelPortMasterStructureList> factoryResults, out List<long> factoryIdList, out Dictionary<long?, long?> dicFactory)
            {
                factoryResults = null;
                factoryIdList = new();
                dicFactory = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterFactoryList, out string factorySql);

                // SQL実行
                factoryResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterStructureList>(factorySql, searchCondition);
                if (factoryResults == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 工場の親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterStructureList factoryResult in factoryResults)
                {
                    factoryResult.FactoryParentNumber = (int)dicDistrict[factoryResult.FactoryParentId];
                    dicFactory.Add(factoryResult.FactoryId, factoryResult.FactoryNumber);
                    factoryIdList.Add((long)factoryResult.FactoryId);
                }

                return true;
            }

            // 予備品倉庫情報を取得
            bool getWarehouseResults(Dictionary<long?, long?> dicFactory, out IList<TMQUtil.CommonExcelPortMasterPartsList> warehouseResults, out Dictionary<long?, long?> dicWarehouse)
            {
                warehouseResults = null;
                dicWarehouse = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterWarehouseList, out string warehouseSql);

                // SQL実行
                warehouseResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterPartsList>(warehouseSql, searchCondition);
                if (warehouseResults == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 予備品倉庫の親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterPartsList warehouseResult in warehouseResults)
                {
                    warehouseResult.WarehouseParentNumber = ((int)dicFactory[warehouseResult.WarehouseParentId]).ToString();
                    dicWarehouse.Add(warehouseResult.WarehouseId, long.Parse(warehouseResult.WarehouseNumber));
                }

                return true;
            }

            // 棚情報を取得
            bool getRackResults(Dictionary<long?, long?> dicWarehouse, out IList<TMQUtil.CommonExcelPortMasterPartsList> rackResults)
            {
                rackResults = null;

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterRackList, out string rackSql);

                // SQL実行
                rackResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterPartsList>(rackSql, searchCondition);
                if (rackResults == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 機種大分類の親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterPartsList rackResult in rackResults)
                {
                    rackResult.RackParentNumber = ((int)dicWarehouse[rackResult.RackParentId]).ToString();
                }

                return true;
            }
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

            // シート番号を判定
            int sheetNo = int.Parse(this.IndividualDictionary["TargetSheetNo"].ToString());
            if (sheetNo == Master.OrdeerSheetNo)
            {
                // 並び順データの取得
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, TargetCtrlId.ExcelPortUpload,
                    out List<TMQUtil.CommonExcelPortMasterOrderList> resultOrderList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 並び順データ登録処理
                if (!TMQUtil.registItemOrderExcelPort(structureGroupId, resultOrderList, DateTime.Now, this.db, this.UserId))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                return ComConsts.RETURN_RESULT.OK;
            }

            // ExcelPortアップロードデータの取得(地区)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.District.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterPartsList> resultDistrictList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(工場)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Factory.ControlGroupId,
               out List<TMQUtil.CommonExcelPortMasterPartsList> resultFactoryList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(倉庫)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterPartsList> resultWarehouseList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(棚)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterPartsList> resultRackList, out resultMsg, ref fileType, ref fileName, ref ms);

            // 各階層のリストを作成する
            // 地区
            Dictionary<long?, long?> districtDic = new();
            foreach (TMQUtil.CommonExcelPortMasterPartsList result in resultDistrictList)
            {
                // 地区リスト
                if (result.DistrictNumber != null)
                {
                    districtDic.Add(result.DistrictNumber, result.DistrictId);
                }
            }

            // 工場
            Dictionary<long?, long?> factoryDic = new();
            foreach (TMQUtil.CommonExcelPortMasterPartsList result in resultFactoryList)
            {
                // 工場リスト
                if (result.FactoryNumber != null)
                {
                    factoryDic.Add(result.FactoryNumber, result.FactoryId);
                }
            }

            // エラー情報リスト
            List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            // 倉庫
            Dictionary<long?, ExcelPortStructureList> warehouseDic = new();
            foreach (TMQUtil.CommonExcelPortMasterPartsList result in resultWarehouseList)
            {
                rowErrFlg = false;

                // 倉庫IDが入力されていない場合はエラー
                if (string.IsNullOrEmpty(result.WarehouseNumber) || !long.TryParse(result.WarehouseNumber, out long outWarehouseNumber))
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseNo, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    rowErrFlg = true;
                }
                if (!TMQUtil.rangeChackExcelPort(result.WarehouseNumber))
                {
                    // 範囲外の値の場合はエラー
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseNo, null, GetResMessage(new string[] { ComRes.ID.ID941060015, TMQUtil.numericRangeExcelPort.minValue.ToString(), TMQUtil.numericRangeExcelPort.maxValue.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    rowErrFlg = true;
                }
                if (!TMQUtil.lengthCheckExcelPort(result.WarehouseNumber))
                {
                    // 最大桁数より多い場合はエラー
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseNo, null, GetResMessage(new string[] { ComRes.ID.ID941060018, TMQUtil.numericRangeExcelPort.maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    rowErrFlg = true;
                }
                if (long.TryParse(result.WarehouseNumber, out outWarehouseNumber))
                {
                    // ディクショナリにキーが含まれている場合(重複したIDが入力されている場合)エラー
                    if (warehouseDic.ContainsKey(long.Parse(result.WarehouseNumber)))
                    {
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseNo, null, GetResMessage(new string[] { ComRes.ID.ID141220008 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                        rowErrFlg = true;
                    }
                }

                // 工場IDが入力されていない場合はエラー
                if (string.IsNullOrEmpty(result.WarehouseParentNumber) || !long.TryParse(result.WarehouseParentNumber, out long outWarehouseParentNumber))
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    rowErrFlg = true;
                }
                if (!TMQUtil.rangeChackExcelPort(result.WarehouseParentNumber))
                {
                    // 範囲外の値の場合はエラー
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941060015, TMQUtil.numericRangeExcelPort.minValue.ToString(), TMQUtil.numericRangeExcelPort.maxValue.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    rowErrFlg = true;
                }
                if (!TMQUtil.lengthCheckExcelPort(result.WarehouseParentNumber))
                {
                    // 最大桁数より多い場合はエラー
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941060018, TMQUtil.numericRangeExcelPort.maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    rowErrFlg = true;
                }

                // 該当業でエラーがある場合はここで終了
                if (rowErrFlg)
                {
                    continue;
                }

                // 倉庫リスト
                if (!string.IsNullOrEmpty(result.WarehouseNumber))
                {
                    ExcelPortStructureList list = new();
                    list.StructureId = result.WarehouseId;
                    list.ParentId = long.Parse(result.WarehouseParentNumber);
                    warehouseDic.Add(long.Parse(result.WarehouseNumber), list);
                }
            }

            // 棚
            List<long?> rackDic = new();
            foreach (TMQUtil.CommonExcelPortMasterPartsList result in resultRackList)
            {
                rowErrFlg = false;

                // 棚IDが入力されていない場合はエラー
                if (string.IsNullOrEmpty(result.RackNumber) || !long.TryParse(result.RackNumber, out long outRackNumber))
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackNo, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    rowErrFlg = true;
                }
                if (!TMQUtil.rangeChackExcelPort(result.RackNumber))
                {
                    // 範囲外の値の場合はエラー
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackNo, null, GetResMessage(new string[] { ComRes.ID.ID941060015, TMQUtil.numericRangeExcelPort.minValue.ToString(), TMQUtil.numericRangeExcelPort.maxValue.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    rowErrFlg = true;
                }
                if (!TMQUtil.lengthCheckExcelPort(result.RackNumber))
                {
                    // 最大桁数より多い場合はエラー
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackNo, null, GetResMessage(new string[] { ComRes.ID.ID941060018, TMQUtil.numericRangeExcelPort.maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    rowErrFlg = true;
                }
                if (long.TryParse(result.RackNumber, out outRackNumber))
                {
                    // ディクショナリにキーが含まれている場合(重複したIDが入力されている場合)エラー
                    if (rackDic.Contains(long.Parse(result.RackNumber)))
                    {
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackNo, null, GetResMessage(new string[] { ComRes.ID.ID141220008 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                        rowErrFlg = true;
                    }
                }

                // 倉庫IDが入力されていない場合はエラー
                if (string.IsNullOrEmpty(result.RackParentNumber) || !long.TryParse(result.RackParentNumber, out long outRackParentNumber))
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    rowErrFlg = true;
                }
                if (!TMQUtil.rangeChackExcelPort(result.RackParentNumber))
                {
                    // 範囲外の値の場合はエラー
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941060015, TMQUtil.numericRangeExcelPort.minValue.ToString(), TMQUtil.numericRangeExcelPort.maxValue.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    rowErrFlg = true;
                }
                if (!TMQUtil.lengthCheckExcelPort(result.RackParentNumber))
                {
                    // 最大桁数より多い場合はエラー
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941060018, TMQUtil.numericRangeExcelPort.maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    rowErrFlg = true;
                }

                // 該当業でエラーがある場合はここで終了
                if (rowErrFlg)
                {
                    continue;
                }

                rackDic.Add(long.Parse(result.RackNumber));
            }

            // 各階層のID列が不正の場合はエラー
            if (errorInfoList.Count > 0)
            {
                excelPort.ErrorInfoList = new();

                // エラー情報シートへ設定
                excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 倉庫 入力チェック&登録処理
            if (!executeCheckAndRegistWarehouseExcelPort(ref resultWarehouseList, ref errorInfoList, factoryDic, ref warehouseDic))
            {
                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                }

                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 棚 入力チェック&登録処理
            if (!executeCheckAndRegistRackExcelPort(ref resultRackList, ref errorInfoList, factoryDic, warehouseDic))
            {
                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                }

                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            resultMsg = string.Empty;
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 倉庫 入力チェック&登録処理
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistWarehouseExcelPort(ref List<TMQUtil.CommonExcelPortMasterPartsList> resultWarehouseList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dictionary<long?, long?> factoryDic, ref Dictionary<long?, ExcelPortStructureList> warehouseDic)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            foreach (TMQUtil.CommonExcelPortMasterPartsList result in resultWarehouseList)
            {
                // 倉庫
                if ((!string.IsNullOrEmpty(result.WarehouseName) || !string.IsNullOrEmpty(result.WarehouseParentNumber)) && string.IsNullOrEmpty(result.WarehouseNumber))
                {
                    // 倉庫IDが未入力かつ、倉庫名・工場IDのどちらかが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseNo, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!string.IsNullOrEmpty(result.WarehouseNumber) && string.IsNullOrEmpty(result.WarehouseName))
                {
                    // 倉庫IDが入力されていて倉庫名が未入力の場合
                    // ○○文字以内で入力して下さい。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseName, null, GetResMessage(new string[] { ComRes.ID.ID941060018, TMQUtil.ItemTranslasionMaxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!TMQUtil.commonTextByteCheckExcelPort(result.WarehouseName, out int maxLength))
                {
                    // 文字数チェック
                    // ○○文字以内で入力して下さい。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseName, null, GetResMessage(new string[] { ComRes.ID.ID941060018, maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!string.IsNullOrEmpty(result.WarehouseNumber) && string.IsNullOrEmpty(result.WarehouseParentNumber))
                {
                    // 倉庫IDが入力されていて工場IDが未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!string.IsNullOrEmpty(result.WarehouseParentNumber) && !factoryDic.ContainsKey(long.Parse(result.WarehouseParentNumber)))
                {
                    // 存在しない工場IDが入力されている場合
                    // 入力内容が不正です。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID141220008 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // アイテムの重複チェック
                if (result.WarehouseName != result.WarehouseNameBefore)
                {
                    int cnt = 0;
                    TMQUtil.GetCountDb(new
                    {
                        @LocationStructureId = 0,
                        @LanguageId = this.LanguageId,
                        @TranslationText = result.WarehouseName,
                        @StructureGroupId = structureGroupId,
                        @StructureLayerNo = (int)TMQConst.MsStructure.StructureLayerNo.SpareLocation.Warehouse,
                        @ParentStructureId = factoryDic[long.Parse(result.WarehouseParentNumber)]
                    }, SqlName.GetCountLayersTranslation, ref cnt, db, SqlName.ComLayersDir);

                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.WarehouseName, null, GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Warehouse.ControlGroupId));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                // 登録処理
                if (!executeExcelPortRegistWarehouse(result, factoryDic, ref warehouseDic, now))
                {
                    return false;
                }
            }

            // 全件問題無ければ正常終了
            if (errFlg)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 棚 入力チェック&登録処理
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistRackExcelPort(ref List<TMQUtil.CommonExcelPortMasterPartsList> resultRackList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dictionary<long?, long?> factoryDic, Dictionary<long?, ExcelPortStructureList> warehouseDic)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            foreach (TMQUtil.CommonExcelPortMasterPartsList result in resultRackList)
            {
                // 棚
                if ((!string.IsNullOrEmpty(result.RackName) || !string.IsNullOrEmpty(result.RackParentNumber)) && string.IsNullOrEmpty(result.RackNumber))
                {
                    // 棚IDが未入力かつ、棚名・倉庫IDのどちらかが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackNo, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!string.IsNullOrEmpty(result.RackNumber) && string.IsNullOrEmpty(result.RackName))
                {
                    // 棚IDが入力されていて系列名が未入力の場合
                    // ○○文字以内で入力して下さい。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackName, null, GetResMessage(new string[] { ComRes.ID.ID941060018, TMQUtil.ItemTranslasionMaxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!TMQUtil.commonTextByteCheckExcelPort(result.RackName, out int maxLength))
                {
                    // 文字数チェック
                    // ○○文字以内で入力して下さい。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackName, null, GetResMessage(new string[] { ComRes.ID.ID941060018, maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!string.IsNullOrEmpty(result.RackNumber) && string.IsNullOrEmpty(result.RackParentNumber))
                {
                    // 棚IDが入力されていて倉庫IDが未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!string.IsNullOrEmpty(result.RackParentNumber) && !warehouseDic.ContainsKey(long.Parse(result.RackParentNumber)))
                {
                    // 存在しない倉庫IDが入力されている場合
                    // 入力内容が不正です。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID141220008 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // アイテムの重複チェック
                if (result.RackName != result.RackNameBefore)
                {
                    int cnt = 0;
                    TMQUtil.GetCountDb(new
                    {
                        @LocationStructureId = 0,
                        @LanguageId = this.LanguageId,
                        @TranslationText = result.RackName,
                        @StructureGroupId = structureGroupId,
                        @StructureLayerNo = (int)TMQConst.MsStructure.StructureLayerNo.SpareLocation.Rack,
                        @ParentStructureId = warehouseDic[long.Parse(result.RackParentNumber)].StructureId
                    }, SqlName.GetCountLayersTranslation, ref cnt, db, SqlName.ComLayersDir);

                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.RackName, null, GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1040.Rack.ControlGroupId));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                // 登録処理
                if (!executeExcelPortRegistRack(result, warehouseDic, factoryDic, now))
                {
                    return false;
                }

            }

            // 全件問題無ければ正常終了
            if (errFlg)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 倉庫登録処理
        /// </summary>
        /// <param name="factoryDic">工場IDディクショナリ</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegistWarehouse(TMQUtil.CommonExcelPortMasterPartsList warehouse, Dictionary<long?, long?> factoryDic, ref Dictionary<long?, ExcelPortStructureList> warehouseDic, DateTime now)
        {
            // 所属情報が変更されている場合は削除
            bool isNew = false;
            if (!string.IsNullOrEmpty(warehouse.WarehouseParentNumber) &&
                !string.IsNullOrEmpty(warehouse.WarehouseParentNumberBefore) &&
                warehouse.WarehouseParentNumber != warehouse.WarehouseParentNumberBefore)
            {
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, Master.SqlName.SubDir, new { StructureId = warehouse.WarehouseId, UpdateDatetime = now, UpdateUserId = this.UserId }, db))
                {
                    return false;
                }
                isNew = true;
            }

            // 翻訳マスタ登録処理
            if (!TMQUtil.registTranslationStructureExcelPort(structureGroupId,                                                   // 構成グループID
                                                             now,                                                                // 現在日時
                                                             this.LanguageId,                                                    // 言語ID
                                                             this.db,                                                            // DBクラス
                                                             this.UserId,                                                        // ユーザーID
                                                             warehouse.WarehouseName,                                            // 翻訳名
                                                             warehouse.WarehouseNameBefore,                                      // 変更前翻訳名
                                                             warehouse.WarehouseItemTranslationId,                               // 翻訳ID
                                                             (int)factoryDic[long.Parse(warehouse.WarehouseParentNumber)],                   // 工場ID
                                                             warehouse.WarehouseId,                                              // 構成ID
                                                             (int)TMQConst.MsStructure.StructureLayerNo.SpareLocation.Warehouse, // 階層番号
                                                             (int)factoryDic[long.Parse(warehouse.WarehouseParentNumber)],                   // 親構成ID
                                                             out int transId,                                                    // 翻訳ID
                                                             isNew))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!TMQUtil.registItemStructureExcelPort(warehouse.WarehouseId,                  // 構成ID
                                            structureGroupId,                                 // 構成グループID
                                            (int)factoryDic[long.Parse(warehouse.WarehouseParentNumber)], // 工場ID
                                            warehouse.WarehouseName,                          // 翻訳名
                                            warehouse.WarehouseNameBefore,                    // 変更前翻訳名
                                            warehouse.WarehouseItemTranslationId,             // 翻訳ID
                                            warehouse.WarehouseItemId,                        // アイテムID
                                            transId,                                          // 翻訳ID
                                            now,                                              // 現在日時
                                            this.LanguageId,                                  // 言語ID
                                            this.db,                                          // DBクラス
                                            this.UserId,                                      // ユーザーID
                                            out int itemId,                                   // アイテムID
                                            isNew))
            {
                return false;
            }

            // 構成マスタ登録
            if (!TMQUtil.registStructureExcelPort(structureGroupId,                                                  // 構成グループID
                                                 warehouse.WarehouseId,                                              // 構成ID
                                                 (int)factoryDic[long.Parse(warehouse.WarehouseParentNumber)],                   // 工場ID
                                                 itemId,                                                             // アイテムID
                                                 (int)factoryDic[long.Parse(warehouse.WarehouseParentNumber)],                   // 親構成ID
                                                 (int)TMQConst.MsStructure.StructureLayerNo.SpareLocation.Warehouse, // 階層番号
                                                 now,                                                                // 現在日時
                                                 this.db,                                                            // DBクラス
                                                 this.UserId,                                                        // ユーザーID
                                                 isNew,
                                                 out int structureId))
            {
                return false;
            }

            // 倉庫ディクショナリに登録した情報を追加
            warehouseDic[long.Parse(warehouse.WarehouseNumber)].StructureId = structureId;

            return true;
        }

        /// <summary>
        /// 棚登録処理
        /// </summary>
        /// <param name="warehouseDic">倉庫IDディクショナリ</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegistRack(TMQUtil.CommonExcelPortMasterPartsList rack, Dictionary<long?, ExcelPortStructureList> warehouseDic, Dictionary<long?, long?> factoryDic, DateTime now)
        {
            bool parentChanged = false;

            if (rack.RackId != null)
            {
                // DBより自分自身の親構成IDを取得
                ComDao.MsStructureEntity ms = new ComDao.MsStructureEntity().GetEntity((int)rack.RackId, this.db);
                // 自分の親構成IDを比較(異なる場合は自分の親アイテムの所属が変わった)
                parentChanged = ms.ParentStructureId != warehouseDic[long.Parse(rack.RackParentNumber)].StructureId;
            }

            // 所属情報が変更されている場合は削除(自分の親アイテムの所属が変わった場合も含む)
            bool isNew = false;
            if ((!string.IsNullOrEmpty(rack.RackParentNumber) &&
                 !string.IsNullOrEmpty(rack.RackParentNumberBefore) &&
                rack.RackParentNumber != rack.RackParentNumberBefore) ||
                parentChanged)
            {
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, Master.SqlName.SubDir, new { StructureId = rack.RackId, UpdateDatetime = now, UpdateUserId = this.UserId }, db))
                {
                    return false;
                }
                isNew = true;
            }

            int factoryId = (int)factoryDic[(int)warehouseDic[long.Parse(rack.RackParentNumber)].ParentId];
            // 翻訳マスタ登録処理
            if (!TMQUtil.registTranslationStructureExcelPort(structureGroupId,                                              // 構成グループID
                                                             now,                                                           // 現在日時
                                                             this.LanguageId,                                               // 言語ID
                                                             this.db,                                                       // DBクラス
                                                             this.UserId,                                                   // ユーザーID
                                                             rack.RackName,                                                 // 翻訳名
                                                             rack.RackNameBefore,                                           // 変更前翻訳名
                                                             rack.RackItemTranslationId,                                    // 翻訳ID
                                                             factoryId,                                                     // 工場ID
                                                             rack.RackId,                                                   // 構成ID
                                                             (int)TMQConst.MsStructure.StructureLayerNo.SpareLocation.Rack, // 階層番号
                                                             (int)warehouseDic[long.Parse(rack.RackParentNumber)].StructureId,          // 親構成ID
                                                             out int transId,                                               // 翻訳ID
                                                             isNew))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!TMQUtil.registItemStructureExcelPort(rack.RackId,                // 構成ID
                                            structureGroupId,                         // 構成グループID
                                            factoryId,                                // 工場ID
                                            rack.RackName,                        // 翻訳名
                                            rack.RackNameBefore,                  // 変更前翻訳名
                                            rack.RackItemTranslationId,           // 翻訳ID
                                            rack.RackItemId,                      // アイテムID
                                            transId,                                  // 翻訳ID
                                            now,                                      // 現在日時
                                            this.LanguageId,                          // 言語ID
                                            this.db,                                  // DBクラス
                                            this.UserId,                              // ユーザーID
                                            out int itemId,                           // アイテムID
                                            isNew))
            {
                return false;
            }

            // 構成マスタ登録
            if (!TMQUtil.registStructureExcelPort(structureGroupId,                                             // 構成グループID
                                                 rack.RackId,                                                   // 構成ID
                                                 factoryId,                                                     // 工場ID
                                                 itemId,                                                        // アイテムID
                                                 (int)warehouseDic[long.Parse(rack.RackParentNumber)].StructureId,          // 親構成ID
                                                 (int)TMQConst.MsStructure.StructureLayerNo.SpareLocation.Rack, // 階層番号
                                                 now,                                                           // 現在日時
                                                 this.db,                                                       // DBクラス
                                                 this.UserId,                                                   // ユーザーID
                                                 isNew,
                                                 out int structureId))
            {
                return false;
            }

            return true;
        }

        #endregion
        #endregion

        #region privateメソッド
        /// <summary>
        /// 一覧画面 初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initList()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(Master.ConductInfo.FormList.ControlId.HiddenId, this.pageInfoList);

            // 非表示情報取得
            var hiddenInfo = TMQUtil.GetHiddenInfoForMaster(structureGroupId, itemListType, this.UserId, db);
            // 工場ID
            hiddenInfo.FactoryId = Const.CommonFactoryId;
            // 対象アイテム一覧
            hiddenInfo.TargetItemList = Master.TargetItemList.Factory;

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
            string ctrlId = Master.ConductInfo.FormList.ControlId.HiddenId;

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
            string ctrlId = Master.ConductInfo.FormList.ControlId.SearchId;

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

            //// 工場ID
            //if (result.FactoryId == null)
            //{
            //    // 検索条件の工場が未選択の場合、工場共通「0」を設定
            //    result.FactoryId = Const.CommonFactoryId;
            //}
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
            string ctrlId = (hiddenInfo.TargetItemList == (int)Master.TargetItemList.Standard) ? Master.ConductInfo.FormList.ControlId.StarndardItemId : Master.ConductInfo.FormList.ControlId.FactoryItemId;

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

            // 工場アイテム一覧の取得＆設定
            // 一覧のコントロールID
            var ctrlId = Master.ConductInfo.FormList.ControlId.FactoryItemId;
            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = ComUtil.GetNotNullNameByClass<Dao.SearchCondition>(condition);

            // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetSpareLocationStructureFactoryItemList, out string baseSql, listUnComment);

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
            var pageInfo = GetPageInfo(Master.ConductInfo.FormEdit.ControlId.ItemTranId, this.pageInfoList);

            // アイテム翻訳一覧取得
            var results = new List<TMQUtil.ItemTranslationForMaster>();
            if (!TMQUtil.GetItemTranListForMaster(condition, GetResMessage("111270026"), ref results, this.db))
            {
                return false;
            }

            if (results == null)
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
            //// ページ情報
            //var pageInfo = GetPageInfo(Master.ConductInfo.FormEdit.ControlId.ItemInfoId, this.pageInfoList);

            //// 検索実行
            //var results = new List<TMQUtil.SearchResultForMaster>();
            //if (!TMQUtil.GetItemInfoForMaster(condition, ref results, this.db))
            //{
            //    return false;
            //}

            //if (results == null)
            //{
            //    return false;
            //}

            //// 検索結果の設定
            //if (SetSearchResultsByDataClass<TMQUtil.SearchResultForMaster>(pageInfo, results, 1))
            //{
            //    // 正常終了
            //    this.Status = CommonProcReturn.ProcStatus.Valid;
            //}

            //// アイテムIDの設定

            //// ページ情報
            //pageInfo = GetPageInfo(Master.ConductInfo.FormEdit.ControlId.ItemId, this.pageInfoList);

            //// 検索結果の設定
            //if (SetSearchResultsByDataClass<TMQUtil.SearchResultForMaster>(pageInfo, results, 1))
            //{
            //    // 正常終了
            //    this.Status = CommonProcReturn.ProcStatus.Valid;
            //}
            // 一覧のコントロールID
            var ctrlId = Master.ConductInfo.FormEdit.ControlId.ItemInfoId;

            // ページ情報
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // 検索実行
            var results = TMQUtil.SqlExecuteClass.SelectList<Dao.SearchResult>(SqlName.GetSpareLocationStructureItemInfo, SqlName.SubDir, condition, this.db);
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
            var results = TMQUtil.SqlExecuteClass.SelectList<TMQUtil.SearchResultForMaster>(SqlName.GetSpareLocationStructureIItemOrderList, SqlName.SubDir, condition, this.db);

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

            //// ページ情報
            //var pageInfo = GetPageInfo(Master.ConductInfo.FormOrder.ControlId.ItemOrderId, this.pageInfoList);

            //// 検索実行
            //var results = new List<TMQUtil.SearchResultForMaster>();
            //if (!TMQUtil.GetItemOrderListForMaster(condition, ref results, this.db))
            //{
            //    return false;
            //}

            //// 検索結果の設定
            //if (SetSearchResultsByDataClass<TMQUtil.SearchResultForMaster>(pageInfo, results, results.Count))
            //{
            //    // 正常終了
            //    this.Status = CommonProcReturn.ProcStatus.Valid;
            //}

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
                if (isErrorExclusive())
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
            List<ComDao.MsStructureOrderEntity> registInfoList = getRegistInfoList<ComDao.MsStructureOrderEntity>(Master.ConductInfo.FormOrder.ControlId.ItemOrderId, now);

            // 工場ID、表示順の設定
            int order = 1;
            foreach (ComDao.MsStructureOrderEntity registInfo in registInfoList)
            {
                // 工場IDは検索条件の工場IDを設定(未選択の場合、共通工場「0」を設定)
                registInfo.FactoryId = hiddenInfo.FactoryId;
                // 表示順設定
                registInfo.DisplayOrder = order;
                // 表示順カウントアップ
                order++;
            }

            // 工場別アイテム表示順マスタ削除
            if (!TMQUtil.DeleteDb(hiddenInfo, SqlName.DeleteLayersStructureOrder, this.db, SqlName.ComLayersDir))
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
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive()
        {
            // アイテム翻訳一覧のコントロールID
            string ctrlId = Master.ConductInfo.FormEdit.ControlId.ItemTranId;

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
            ctrlId = Master.ConductInfo.FormEdit.ControlId.ItemInfoId;

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
            var newList = TMQUtil.SqlExecuteClass.SelectList<TMQUtil.SearchResultForMaster>(SqlName.GetSpareLocationStructureIItemOrderList, SqlName.SubDir, condition, this.db);

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
                var ctrlId = Master.ConductInfo.FormEdit.ControlId.ItemTranId;
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
                    if (!MS1040_CheckDuplicateByItemTran(hiddenInfo, result, ref errFlg))
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
                bool MS1040_CheckDuplicateByItemTran(
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
                    if (!TMQUtil.GetCountDb(targetInfo, SqlName.GetCountLayersTranslation, ref cnt, db, SqlName.ComLayersDir))
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
            var itemInfo = getItemInfo();

            //if (hiddenInfo.StructureLayerNo == (int)Master.Structure.StructureLayerNo.Layer2)
            //{
            //    // 子階層翻訳マスタ削除
            //    if (!deleteChildStructure(now, hiddenInfo, itemInfo, (int)Master.Structure.StructureLayerNo.Layer3, itemId, ref structureId))
            //    {
            //        return false;
            //    }
            //}

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
            string ctrlId = Master.ConductInfo.FormEdit.ControlId.ItemTranId;
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
        /// <returns>アイテム情報</returns>
        private TMQUtil.SearchResultForMaster getItemInfo()
        {
            // アイテム情報のコントロールID
            string ctrlId = Master.ConductInfo.FormEdit.ControlId.ItemInfoId;
            // 指定されたコントロールIDの結果情報のみ抽出
            List<Dictionary<string, object>> targetDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);

            // データクラスに変換
            var results = convertDicListToClassList<TMQUtil.SearchResultForMaster>(targetDicList, ctrlId);

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
        private bool registItemEx(DateTime now, TMQUtil.HiddenInfoForMaster hiddenInfo, TMQUtil.SearchResultForMaster itemInfo, int itemId)
        {
            // アイテムマスタ拡張登録
            bool isExclusiveError = false;
            if (!TMQUtil.RegistItemEx(now, hiddenInfo, itemInfo, itemId, this.UserId, itemExCnt, ref isExclusiveError, this.db))
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
        private bool registStructure(DateTime now, TMQUtil.HiddenInfoForMaster hiddenInfo, TMQUtil.SearchResultForMaster itemInfo, int itemId, ref int structureId)
        {
            // 構成マスタ登録
            if (!TMQUtil.RegistStructure(now, hiddenInfo, itemInfo, itemId, this.UserId, ref structureId, this.db))
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
        }

        /// <summary>
        /// 工場別未使用標準アイテムマスタ登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemInfo">アイテム情報</param>
        /// <returns>エラーの場合False</returns>
        private bool registItemUnused(DateTime now, TMQUtil.HiddenInfoForMaster hiddenInfo, TMQUtil.SearchResultForMaster itemInfo)
        {
            // 工場別未使用標準アイテムマスタ登録
            if (!TMQUtil.RegistItemUnused(now, hiddenInfo, itemInfo, this.UserId, this.db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 子階層構成アイテムマスタ削除
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemInfo">アイテム情報</param>
        /// <returns>エラーの場合False</returns>
        private bool deleteChildStructure(DateTime now, TMQUtil.HiddenInfoForMaster hiddenInfo, TMQUtil.SearchResultForMaster itemInfo, int layerNo, int itemId, ref int structureId)
        {
            // 子階層構成アイテムマスタ削除
            if (!TMQUtil.DeleteChildStructure(now, hiddenInfo, itemInfo, layerNo, itemId, this.UserId, ref structureId, this.db))
            {
                return false;
            }

            return true;
        }
        #endregion

    }
}