using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
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
            public const int Factory = 5;
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
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                return ComConsts.RETURN_RESULT.NG;
            }

            // 検索処理
            if (!getDataList(ref excelPort, searchCondition, out IList<Dictionary<string, object>> dataList))
            {
                // 「ダウンロード処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                return ComConsts.RETURN_RESULT.NG;
            }

            if (dataList == null || dataList.Count == 0)
            {
                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「該当データがありません。」
                resultMsg = GetResMessage(ComRes.ID.ID941060001);
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
            if (!excelPort.OutputExcelPortTemplateFile(dataList, out fileType, out fileName, out ms, out detailMsg))
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
            condition.LayerIdList = new List<long>() { 2, 3, 4, 5 };                        // 階層番号

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

            // メンテナンス対象コンボボックスで選択されたアイテムに応じて検索
            switch (searchCondition.MaintenanceTargetNo)
            {
                case Master.MaintainanceTargetExInfo.ExData.MasterItem: // マスタアイテム

                    // 一覧検索実行
                    IList<TMQUtil.CommonExcelPortMasterStructureList> masterReaults = getMasterResults(searchCondition);
                    if (masterReaults == null || masterReaults.Count == 0)
                    {
                        return false;
                    }

                    // Dicitionalyに変換
                    dataList = ComUtil.ConvertClassToDictionary<TMQUtil.CommonExcelPortMasterStructureList>(masterReaults);
                    break;

                case Master.MaintainanceTargetExInfo.ExData.Oerder: // マスタ並び順設定

                    // SQLを取得
                    TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortStructureItemOrderList, out string ordersSql);

                    // 一覧検索実行
                    IList<TMQUtil.CommonExcelPortMasterOrderList> orderResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterOrderList>(ordersSql, searchCondition);
                    if (orderResults == null || orderResults.Count == 0)
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
        private List<TMQUtil.CommonExcelPortMasterStructureList> getMasterResults(TMQUtil.CommonExcelPortMasterCondition searchCondition)
        {
            // 地区情報を取得
            if (!getDistrictResults(out IList<TMQUtil.CommonExcelPortMasterStructureList> districtResults, out Dictionary<long?, int?> dicDistrict))
            {
                return new List<TMQUtil.CommonExcelPortMasterStructureList>();
            }

            // 工場情報を取得
            if (!getFactoryResults(dicDistrict, out IList<TMQUtil.CommonExcelPortMasterStructureList> factoryResults, out List<long> factoryIdList, out Dictionary<long?, int?> dicFactory))
            {
                return new List<TMQUtil.CommonExcelPortMasterStructureList>();
            }

            // 工場IDリストを設定
            searchCondition.FactoryIdList = factoryIdList;

            // プラント情報を取得
            if (!getPlantResults(dicFactory, out IList<TMQUtil.CommonExcelPortMasterStructureList> plantResults, out Dictionary<long?, int?> dicPlant))
            {
                return new List<TMQUtil.CommonExcelPortMasterStructureList>();
            }

            // 系列情報を取得
            if (!getSeriesResults(dicPlant, out IList<TMQUtil.CommonExcelPortMasterStructureList> seriesResults, out Dictionary<long?, int?> dicSeries))
            {
                return new List<TMQUtil.CommonExcelPortMasterStructureList>(); ;
            }

            // 工程情報を取得
            if (!getStrokeResults(dicSeries, out IList<TMQUtil.CommonExcelPortMasterStructureList> strokeResults, out Dictionary<long?, int?> dicStroke))
            {
                return new List<TMQUtil.CommonExcelPortMasterStructureList>();
            }

            // 設備情報取得
            if (!getFacilityResults(dicStroke, out IList<TMQUtil.CommonExcelPortMasterStructureList> facilityResults))
            {
                return new List<TMQUtil.CommonExcelPortMasterStructureList>();
            }

            // 取得した「地区」「工場」「プラント」「系列」「工程」「設備」のうち、最大のデータ件数を出力データのレコード数とする
            int[] dataCnts = new int[]
            { districtResults.Count,
              factoryResults.Count,
              plantResults.Count,
              seriesResults.Count,
              strokeResults.Count,
              facilityResults.Count
            };
            int recordNum = dataCnts.Max();

            // 取得データを1レコード単位にまとめる
            List<TMQUtil.CommonExcelPortMasterStructureList> results = new();
            for (int i = 0; i < recordNum; i++)
            {
                TMQUtil.CommonExcelPortMasterStructureList record = new();

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

                // プラント情報
                if (plantResults.Count != 0)
                {
                    record.PlantId = plantResults[0].PlantId;                               // プラントID(構成ID)
                    record.PlantItemTranslationId = plantResults[0].PlantItemTranslationId; // 翻訳ID(プラント)
                    record.PlantNumber = plantResults[0].PlantNumber;                       // プラント番号
                    record.PlantName = plantResults[0].PlantName;                           // プラント名
                    record.PlantParentId = plantResults[0].PlantParentId;                   // プラントの親構成ID
                    record.PlantParentNumber = plantResults[0].PlantParentNumber;           // 工場番号
                    plantResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 系列情報
                if (seriesResults.Count != 0)
                {
                    record.SeriesId = seriesResults[0].SeriesId;                               // 系列ID(構成ID)
                    record.SeriesItemTranslationId = seriesResults[0].SeriesItemTranslationId; // 翻訳ID(系列)
                    record.SeriesNumber = seriesResults[0].SeriesNumber;                       // 系列番号
                    record.SeriesName = seriesResults[0].SeriesName;                           // 系列名
                    record.SeriesParentId = seriesResults[0].SeriesParentId;                   // 系列の親構成ID
                    record.SeriesParentNumber = seriesResults[0].SeriesParentNumber;           // プラント番号
                    seriesResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 工程情報
                if (strokeResults.Count != 0)
                {
                    record.StrokeId = strokeResults[0].StrokeId;                               // 工程ID(構成ID)
                    record.StrokeItemTranslationId = strokeResults[0].StrokeItemTranslationId; // 翻訳ID(工程)
                    record.StrokeNumber = strokeResults[0].StrokeNumber;                       // 工程番号
                    record.StrokeName = strokeResults[0].StrokeName;                           // 工程名
                    record.StrokeParentId = strokeResults[0].StrokeParentId;                   // 工程の親構成ID
                    record.StrokeParentNumber = strokeResults[0].StrokeParentNumber;           // プラント番号
                    strokeResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 設備情報
                if (facilityResults.Count != 0)
                {
                    record.FacilityId = facilityResults[0].FacilityId;                               // 設備ID(構成ID)
                    record.FacilityItemTranslationId = facilityResults[0].FacilityItemTranslationId; // 翻訳ID(設備)
                    record.FacilityNumber = facilityResults[0].FacilityNumber;                       // 設備番号
                    record.FacilityName = facilityResults[0].FacilityName;                           // 設備名
                    record.FacilityParentId = facilityResults[0].FacilityParentId;                   // 設備の親構成ID
                    record.FacilityParentNumber = facilityResults[0].FacilityParentNumber;           // 工程番号
                    facilityResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 出力データ行に追加
                results.Add(record);
            }

            return results;

            // 地区情報を取得
            bool getDistrictResults(out IList<TMQUtil.CommonExcelPortMasterStructureList> districtResults, out Dictionary<long?, int?> dicDistrict)
            {
                districtResults = null;
                dicDistrict = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterDistrictList, out string districtSql);

                // SQL実行
                districtResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterStructureList>(districtSql, searchCondition);
                if (districtResults == null || districtResults.Count == 0)
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
            bool getFactoryResults(Dictionary<long?, int?> dicDistrict, out IList<TMQUtil.CommonExcelPortMasterStructureList> factoryResults, out List<long> factoryIdList, out Dictionary<long?, int?> dicFactory)
            {
                factoryResults = null;
                factoryIdList = new();
                dicFactory = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterFactoryList, out string factorySql);

                // SQL実行
                factoryResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterStructureList>(factorySql, searchCondition);
                if (factoryResults == null || factoryResults.Count == 0)
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

            // プラント情報を取得
            bool getPlantResults(Dictionary<long?, int?> dicFactory, out IList<TMQUtil.CommonExcelPortMasterStructureList> plantResults, out Dictionary<long?, int?> dicPlant)
            {
                plantResults = null;
                dicPlant = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterPlantList, out string plantSql);

                // SQL実行
                plantResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterStructureList>(plantSql, searchCondition);
                if (plantResults == null || plantResults.Count == 0)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // プラントの親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterStructureList plantResult in plantResults)
                {
                    plantResult.PlantParentNumber = (int)dicFactory[plantResult.PlantParentId];
                    dicPlant.Add(plantResult.PlantId, plantResult.PlantNumber);
                }

                return true;
            }

            // 系列情報を取得
            bool getSeriesResults(Dictionary<long?, int?> dicPlant, out IList<TMQUtil.CommonExcelPortMasterStructureList> seriesResults, out Dictionary<long?, int?> dicSeries)
            {
                seriesResults = null;
                dicSeries = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterSeriesList, out string seriesSql);

                // SQL実行
                seriesResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterStructureList>(seriesSql, searchCondition);
                if (seriesResults == null || seriesResults.Count == 0)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 系列の親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterStructureList seriesResult in seriesResults)
                {
                    seriesResult.SeriesParentNumber = (int)dicPlant[seriesResult.SeriesParentId];
                    dicSeries.Add(seriesResult.SeriesId, seriesResult.SeriesNumber);
                }

                return true;
            }

            // 工程情報を取得
            bool getStrokeResults(Dictionary<long?, int?> dicSeries, out IList<TMQUtil.CommonExcelPortMasterStructureList> strokeResults, out Dictionary<long?, int?> dicStroke)
            {
                strokeResults = null;
                dicStroke = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterStrokeList, out string strokeSql);

                // SQL実行
                strokeResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterStructureList>(strokeSql, searchCondition);
                if (strokeResults == null || strokeResults.Count == 0)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 工程の親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterStructureList strokeResult in strokeResults)
                {
                    strokeResult.StrokeParentNumber = (int)dicSeries[strokeResult.StrokeParentId];
                    dicStroke.Add(strokeResult.StrokeId, strokeResult.StrokeNumber);
                }

                return true;
            }

            // 設備情報を取得
            bool getFacilityResults(Dictionary<long?, int?> dicStroke, out IList<TMQUtil.CommonExcelPortMasterStructureList> facilityResults)
            {
                facilityResults = null;

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterFacilityList, out string facilitySql);

                // SQL実行
                facilityResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterStructureList>(facilitySql, searchCondition);
                if (facilityResults == null || facilityResults.Count == 0)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 設備の親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterStructureList facilityResult in facilityResults)
                {
                    facilityResult.FacilityParentNumber = (int)dicStroke[facilityResult.FacilityParentId];
                }

                return true;
            }
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
            if (hiddenInfo.StructureLayerNo == Master.Structure.StructureLayerNo.Layer1)
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
        #endregion

    }
}