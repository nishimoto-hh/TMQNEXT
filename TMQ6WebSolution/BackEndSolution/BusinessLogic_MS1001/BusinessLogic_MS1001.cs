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
using Dao = BusinessLogic_MS1001.BusinessLogicDataClass_MS1001;
using Master = CommonTMQUtil.CommonTMQUtil.ComMaster;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_MS1001
{
    /// <summary>
    /// 場所階層マスタ
    /// </summary>
    public class BusinessLogic_MS1001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 構成グループID
        /// </summary>
        private static int structureGroupId = 1000;

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
            /// プラントID
            /// </summary>
            public const int PlantNo = 24;
            /// <summary>
            /// プラント名
            /// </summary>
            public const int PlantName = 25;
            /// <summary>
            /// 工場番号
            /// </summary>
            public const int PlantParentNumber = 26;
            /// <summary>
            /// 系列ID
            /// </summary>
            public const int SeriesNo = 36;
            /// <summary>
            /// 系列名
            /// </summary>
            public const int SeriesName = 37;
            /// <summary>
            /// プラント番号
            /// </summary>
            public const int SeriesParentNumber = 38;
            /// <summary>
            /// 工程ID
            /// </summary>
            public const int StrokeNo = 48;
            /// <summary>
            /// 工程名
            /// </summary>
            public const int StrokeName = 49;
            /// <summary>
            /// 系列番号
            /// </summary>
            public const int StrokeParentNumber = 50;
            /// <summary>
            /// 設備ID
            /// </summary>
            public const int FacilityNo = 60;
            /// <summary>
            /// 設備名
            /// </summary>
            public const int FacilityName = 61;
            /// <summary>
            /// 工程番号
            /// </summary>
            public const int FacilityParentNumber = 62;
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
            public int? ParentId;
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private class SqlName
        {
            /// <summary>SQL名：場所階層アイテム一覧取得</summary>
            public const string GetLocationStructureItemList = "GetLocationStructureItemList";
            /// <summary>SQL名：場所階層アイテム情報取得</summary>
            public const string GetLocationStructureItemInfo = "GetLocationStructureItemInfo";

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
                    /// 標準アイテム一覧の画面項目定義テーブルのコントロールID
                    /// </summary>
                    public const string StandardItemId = "BODY_020_00_LST_0";
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
        public BusinessLogic_MS1001() : base()
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

            // ユーザ情報の更新が必要
            this.UpdateUserInfo = true;

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
            string ctrlId = ConductInfo.FormList.ControlId.FactoryItemId;
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

            // ユーザ情報の更新が必要
            this.UpdateUserInfo = true;

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
                (int)TMQConst.MsStructure.StructureLayerNo.Location.Plant,
                (int)TMQConst.MsStructure.StructureLayerNo.Location.Series,
                (int)TMQConst.MsStructure.StructureLayerNo.Location.Stroke,
                (int)TMQConst.MsStructure.StructureLayerNo.Location.Facility
            }; // 階層番号

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
                    IList<TMQUtil.CommonExcelPortMasterStructureList> masterReaults = getMasterResults(searchCondition);
                    if (masterReaults == null)
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
                return new List<TMQUtil.CommonExcelPortMasterStructureList>();
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
                    record.PlantItemId = plantResults[0].PlantItemId;                       // プラントアイテムID
                    record.PlantId = plantResults[0].PlantId;                               // プラントID(構成ID)
                    record.PlantItemTranslationId = plantResults[0].PlantItemTranslationId; // 翻訳ID(プラント)
                    record.PlantNumber = plantResults[0].PlantNumber;                       // プラント番号
                    record.PlantName = plantResults[0].PlantName;                           // プラント名
                    record.PlantNameBefore = plantResults[0].PlantNameBefore;               // プラント名(変更前)
                    record.PlantParentId = plantResults[0].PlantParentId;                   // プラントの親構成ID
                    record.PlantParentNumber = plantResults[0].PlantParentNumber;           // 工場番号
                    record.PlantParentNumberBefore = plantResults[0].PlantParentNumber;     // 工場番号
                    plantResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 系列情報
                if (seriesResults.Count != 0)
                {
                    record.SeriesItemId = seriesResults[0].SeriesItemId;                       // 系列アイテムID
                    record.SeriesId = seriesResults[0].SeriesId;                               // 系列ID(構成ID)
                    record.SeriesItemTranslationId = seriesResults[0].SeriesItemTranslationId; // 翻訳ID(系列)
                    record.SeriesNumber = seriesResults[0].SeriesNumber;                       // 系列番号
                    record.SeriesName = seriesResults[0].SeriesName;                           // 系列名
                    record.SeriesNameBefore = seriesResults[0].SeriesNameBefore;               // 系列名(変更前)
                    record.SeriesParentId = seriesResults[0].SeriesParentId;                   // 系列の親構成ID
                    record.SeriesParentNumber = seriesResults[0].SeriesParentNumber;           // プラント番号
                    record.SeriesParentNumberBefore = seriesResults[0].SeriesParentNumber;     // プラント番号
                    seriesResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 工程情報
                if (strokeResults.Count != 0)
                {
                    record.StrokeItemId = strokeResults[0].StrokeItemId;                       // 工程アイテムID
                    record.StrokeId = strokeResults[0].StrokeId;                               // 工程ID(構成ID)
                    record.StrokeItemTranslationId = strokeResults[0].StrokeItemTranslationId; // 翻訳ID(工程)
                    record.StrokeNumber = strokeResults[0].StrokeNumber;                       // 工程番号
                    record.StrokeName = strokeResults[0].StrokeName;                           // 工程名
                    record.StrokeNameBefore = strokeResults[0].StrokeNameBefore;               // 工程名(変更前)
                    record.StrokeParentId = strokeResults[0].StrokeParentId;                   // 工程の親構成ID
                    record.StrokeParentNumber = strokeResults[0].StrokeParentNumber;           // プラント番号
                    record.StrokeParentNumberBefore = strokeResults[0].StrokeParentNumber;     // プラント番号
                    strokeResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 設備情報
                if (facilityResults.Count != 0)
                {
                    record.FacilityItemId = facilityResults[0].FacilityItemId;                       // 設備アイテムID
                    record.FacilityId = facilityResults[0].FacilityId;                               // 設備ID(構成ID)
                    record.FacilityItemTranslationId = facilityResults[0].FacilityItemTranslationId; // 翻訳ID(設備)
                    record.FacilityNumber = facilityResults[0].FacilityNumber;                       // 設備番号
                    record.FacilityName = facilityResults[0].FacilityName;                           // 設備名
                    record.FacilityNameBefore = facilityResults[0].FacilityNameBefore;               // 設備名(変更前)
                    record.FacilityParentId = facilityResults[0].FacilityParentId;                   // 設備の親構成ID
                    record.FacilityParentNumber = facilityResults[0].FacilityParentNumber;           // 工程番号
                    record.FacilityParentNumberBefore = facilityResults[0].FacilityParentNumber;     // 工程番号
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
            bool getFactoryResults(Dictionary<long?, int?> dicDistrict, out IList<TMQUtil.CommonExcelPortMasterStructureList> factoryResults, out List<long> factoryIdList, out Dictionary<long?, int?> dicFactory)
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

            // プラント情報を取得
            bool getPlantResults(Dictionary<long?, int?> dicFactory, out IList<TMQUtil.CommonExcelPortMasterStructureList> plantResults, out Dictionary<long?, int?> dicPlant)
            {
                plantResults = null;
                dicPlant = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterPlantList, out string plantSql);

                // SQL実行
                plantResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterStructureList>(plantSql, searchCondition);
                if (plantResults == null)
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
                if (seriesResults == null)
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
                if (strokeResults == null)
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
                if (facilityResults == null)
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
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.District.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterStructureList> resultDistrictList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(工場)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Factory.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterStructureList> resultFactoryList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(プラント)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Plant.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterStructureList> resultPlantList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(系列)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Series.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterStructureList> resultSeriesList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(工程)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Stroke.ControlGroupId,
               out List<TMQUtil.CommonExcelPortMasterStructureList> resultStrokeList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(設備)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Facility.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterStructureList> resultFacilityList, out resultMsg, ref fileType, ref fileName, ref ms);

            // 各階層のリストを作成する
            // 地区
            Dictionary<int?, long?> districtDic = new();
            foreach (TMQUtil.CommonExcelPortMasterStructureList result in resultDistrictList)
            {
                // 地区リスト
                if (result.DistrictNumber != null)
                {
                    districtDic.Add(result.DistrictNumber, result.DistrictId);
                }
            }

            // 工場
            Dictionary<int?, long?> factoryDic = new();
            foreach (TMQUtil.CommonExcelPortMasterStructureList result in resultFactoryList)
            {
                // 工場リスト
                if (result.FactoryNumber != null)
                {
                    factoryDic.Add(result.FactoryNumber, result.FactoryId);
                }
            }

            // エラー情報リスト
            List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();

            // プラント
            Dictionary<int?, ExcelPortStructureList> plantDic = new();
            foreach (TMQUtil.CommonExcelPortMasterStructureList result in resultPlantList)
            {
                // プラントIDが入力されていない場合はエラー
                if (result.PlantNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.PlantNo, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Plant.ControlGroupId));
                    continue;
                }

                // 工場IDが入力されていない場合はエラー
                if (result.PlantParentNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.PlantParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Plant.ControlGroupId));
                    continue;
                }

                // プラントリスト
                if (result.PlantNumber != null)
                {
                    ExcelPortStructureList list = new();
                    list.StructureId = result.PlantId;
                    list.ParentId = result.PlantParentNumber;
                    plantDic.Add(result.PlantNumber, list);
                }
            }

            // 系列
            Dictionary<int?, ExcelPortStructureList> seriesDic = new();
            foreach (TMQUtil.CommonExcelPortMasterStructureList result in resultSeriesList)
            {
                // 系列IDが入力されていない場合はエラー
                if (result.SeriesNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SeriesNo, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Series.ControlGroupId));
                    continue;
                }

                // プラントIDが入力されていない場合はエラー
                if (result.SeriesParentNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SeriesParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Series.ControlGroupId));
                    continue;
                }

                // 系列リスト
                if (result.SeriesNumber != null)
                {
                    ExcelPortStructureList list = new();
                    list.StructureId = result.SeriesId;
                    list.ParentId = result.SeriesParentNumber;
                    seriesDic.Add(result.SeriesNumber, list);
                }
            }

            // 工程
            Dictionary<int?, ExcelPortStructureList> strokeDic = new();
            foreach (TMQUtil.CommonExcelPortMasterStructureList result in resultStrokeList)
            {
                // 工程IDが入力されていない場合はエラー
                if (result.StrokeNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.StrokeNo, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Stroke.ControlGroupId));
                    continue;
                }

                // 系列IDが入力されていない場合はエラー
                if (result.StrokeParentNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.StrokeParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Stroke.ControlGroupId));
                    continue;
                }

                // 工程リスト
                if (result.StrokeNumber != null)
                {
                    ExcelPortStructureList list = new();
                    list.StructureId = result.StrokeId;
                    list.ParentId = result.StrokeParentNumber;
                    strokeDic.Add(result.StrokeNumber, list);
                }
            }

            // 設備
            foreach (TMQUtil.CommonExcelPortMasterStructureList result in resultFacilityList)
            {
                // 設備IDが入力されていない場合はエラー
                if (result.FacilityNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.FacilityNo, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Facility.ControlGroupId));
                    continue;
                }

                // 工程IDが入力されていない場合はエラー
                if (result.FacilityParentNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.FacilityParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Facility.ControlGroupId));
                    continue;
                }
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

            // プラント 入力チェック&登録処理
            if (!executeCheckAndRegistPlantExcelPort(ref resultPlantList, ref errorInfoList, factoryDic, ref plantDic))
            {
                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                }

                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 系列 入力チェック&登録処理
            if (!executeCheckAndRegistSeriesExcelPort(ref resultSeriesList, ref errorInfoList, factoryDic, plantDic, ref seriesDic))
            {
                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                }

                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 工程 入力チェック&登録処理
            if (!executeCheckAndRegistStrokeExcelPort(ref resultStrokeList, ref errorInfoList, factoryDic, plantDic, seriesDic, ref strokeDic))
            {
                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                }

                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 設備入力チェック& 登録処理
            if (!executeCheckAndRegistFacilityExcelPort(ref resultFacilityList, ref errorInfoList, factoryDic, plantDic, seriesDic, strokeDic))
            {
                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                }

                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // ユーザ情報の更新が必要
            this.UpdateUserInfo = true;

            resultMsg = string.Empty;
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// プラント 入力チェック&登録処理
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistPlantExcelPort(ref List<TMQUtil.CommonExcelPortMasterStructureList> resultPlantList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dictionary<int?, long?> factoryDic, ref Dictionary<int?, ExcelPortStructureList> plantDic)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            foreach (TMQUtil.CommonExcelPortMasterStructureList result in resultPlantList)
            {
                // プラント
                if ((!string.IsNullOrEmpty(result.PlantName) || result.PlantParentNumber != null) && result.PlantNumber == null)
                {
                    // プラントIDが未入力かつ、プラント名・工場IDのどちらかが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.PlantNo, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Plant.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.PlantNumber != null && string.IsNullOrEmpty(result.PlantName))
                {
                    // プラントIDが入力されていてプラント名が未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.PlantName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, TMQUtil.ItemTranslasionMaxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Plant.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.PlantNumber != null && result.PlantParentNumber == null)
                {
                    // プラントIDが入力されていて工場IDが未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.PlantParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Plant.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.PlantParentNumber != null && !factoryDic.ContainsKey(result.PlantParentNumber))
                {
                    // 存在しない工場IDが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.PlantParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Plant.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!TMQUtil.commonTextByteCheckExcelPort(result.PlantName, out int maxLength))
                {
                    // 文字数チェック
                    // アイテム翻訳は○○桁以下で入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.PlantName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Plant.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // アイテムの重複チェック
                if (result.PlantName != result.PlantNameBefore)
                {
                    long factoryId = (long)factoryDic[result.PlantParentNumber];
                    int cnt = 0;
                    TMQUtil.GetCountDb(new
                    {
                        @LocationStructureId = factoryId,
                        @LanguageId = this.LanguageId,
                        @TranslationText = result.PlantName,
                        @StructureGroupId = structureGroupId,
                        @FactoryId = factoryId,
                        @StructureLayerNo = (int)TMQConst.MsStructure.StructureLayerNo.Location.Plant,
                        @ParentStructureId = factoryId
                    }, Master.SqlName.GetCountLayersTranslationByFactory, ref cnt, db, Master.SqlName.ComLayersDir);

                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.PlantName, null, GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Plant.ControlGroupId));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                // 登録処理
                if (!executeExcelPortRegistPlant(result, factoryDic, ref plantDic, now))
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
        /// 系列 入力チェック&登録処理
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistSeriesExcelPort(ref List<TMQUtil.CommonExcelPortMasterStructureList> resultSeriesList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dictionary<int?, long?> factoryDic, Dictionary<int?, ExcelPortStructureList> plantDic, ref Dictionary<int?, ExcelPortStructureList> seriesDic)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            foreach (TMQUtil.CommonExcelPortMasterStructureList result in resultSeriesList)
            {
                // 系列
                if ((!string.IsNullOrEmpty(result.SeriesName) || result.SeriesParentNumber != null) && result.SeriesNumber == null)
                {
                    // 系列IDが未入力かつ、系列名・プラントIDのどちらかが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SeriesNo, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Series.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.SeriesNumber != null && string.IsNullOrEmpty(result.SeriesName))
                {
                    // 系列IDが入力されていて系列名が未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SeriesName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, TMQUtil.ItemTranslasionMaxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Series.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.SeriesNumber != null && result.SeriesParentNumber == null)
                {
                    // 系列IDが入力されていてプラントIDが未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SeriesParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Series.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.SeriesParentNumber != null && !plantDic.ContainsKey(result.SeriesParentNumber))
                {
                    // 存在しないプラントIDが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SeriesParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Series.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!TMQUtil.commonTextByteCheckExcelPort(result.SeriesName, out int maxLength))
                {
                    // 文字数チェック
                    // アイテム翻訳は○○桁以下で入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SeriesName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Series.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // アイテムの重複チェック
                if (result.SeriesName != result.SeriesNameBefore)
                {
                    int plantId = (int)plantDic[result.SeriesParentNumber].ParentId;
                    long factoryId = (long)factoryDic[plantId];
                    int cnt = 0;
                    TMQUtil.GetCountDb(new
                    {
                        @LocationStructureId = factoryId,
                        @LanguageId = this.LanguageId,
                        @TranslationText = result.SeriesName,
                        @StructureGroupId = structureGroupId,
                        @FactoryId = factoryId,
                        @StructureLayerNo = (int)TMQConst.MsStructure.StructureLayerNo.Location.Series,
                        @ParentStructureId = plantDic[result.SeriesParentNumber].StructureId
                    }, Master.SqlName.GetCountLayersTranslationByFactory, ref cnt, db, Master.SqlName.ComLayersDir);

                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SeriesName, null, GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Series.ControlGroupId));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                // 登録処理
                if (!executeExcelPortRegistSeries(result, plantDic, factoryDic, ref seriesDic, now))
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
        /// 工程 入力チェック&登録処理
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistStrokeExcelPort(ref List<TMQUtil.CommonExcelPortMasterStructureList> resultStrokeList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dictionary<int?, long?> factoryDic, Dictionary<int?, ExcelPortStructureList> plantDic, Dictionary<int?, ExcelPortStructureList> seriesDic, ref Dictionary<int?, ExcelPortStructureList> strokeDic)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            foreach (TMQUtil.CommonExcelPortMasterStructureList result in resultStrokeList)
            {

                // 工程
                if ((!string.IsNullOrEmpty(result.StrokeName) || result.StrokeParentNumber != null) && result.StrokeNumber == null)
                {
                    // 工程IDが未入力かつ、工程名・系列IDのどちらかが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.StrokeNo, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Stroke.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.StrokeNumber != null && string.IsNullOrEmpty(result.StrokeName))
                {
                    // 工程IDが入力されていて工程名が未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.StrokeName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, TMQUtil.ItemTranslasionMaxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Stroke.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.StrokeNumber != null && result.StrokeParentNumber == null)
                {
                    // 工程IDが入力されていて系列IDが未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.StrokeParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Stroke.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.StrokeParentNumber != null && !seriesDic.ContainsKey(result.StrokeParentNumber))
                {
                    // 存在しない系列IDが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.StrokeParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Stroke.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!TMQUtil.commonTextByteCheckExcelPort(result.StrokeName, out int maxLength))
                {
                    // 文字数チェック
                    // アイテム翻訳は○○桁以下で入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.StrokeName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Stroke.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // アイテムの重複チェック
                if (result.StrokeName != result.StrokeNameBefore)
                {
                    int seriesId = (int)seriesDic[result.StrokeParentNumber].ParentId;
                    int plantId = (int)plantDic[seriesId].ParentId;
                    long factoryId = (long)factoryDic[plantId];
                    int cnt = 0;
                    TMQUtil.GetCountDb(new
                    {
                        @LocationStructureId = factoryId,
                        @LanguageId = this.LanguageId,
                        @TranslationText = result.StrokeName,
                        @StructureGroupId = structureGroupId,
                        @FactoryId = factoryId,
                        @StructureLayerNo = (int)TMQConst.MsStructure.StructureLayerNo.Location.Stroke,
                        @ParentStructureId = seriesDic[result.StrokeParentNumber].StructureId
                    }, Master.SqlName.GetCountLayersTranslationByFactory, ref cnt, db, Master.SqlName.ComLayersDir);

                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.StrokeName, null, GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Stroke.ControlGroupId));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                // 登録処理
                if (!executeExcelPortRegistStroke(result, seriesDic, factoryDic, plantDic, ref strokeDic, now))
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
        /// 設備 入力チェック&登録処理
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistFacilityExcelPort(ref List<TMQUtil.CommonExcelPortMasterStructureList> resultFacilityList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dictionary<int?, long?> factoryDic, Dictionary<int?, ExcelPortStructureList> plantDic, Dictionary<int?, ExcelPortStructureList> seriesDic, Dictionary<int?, ExcelPortStructureList> strokeDic)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            foreach (TMQUtil.CommonExcelPortMasterStructureList result in resultFacilityList)
            {
                // 設備
                if ((!string.IsNullOrEmpty(result.FacilityName) || result.FacilityParentNumber != null) && result.FacilityNumber == null)
                {
                    // 設備IDが未入力かつ、設備名・工程IDのどちらかが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.FacilityNo, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Facility.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.FacilityNumber != null && string.IsNullOrEmpty(result.FacilityName))
                {
                    // 設備IDが入力されていて設備名が未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.FacilityName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, TMQUtil.ItemTranslasionMaxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Facility.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.FacilityNumber != null && result.FacilityParentNumber == null)
                {
                    // 設備IDが入力されていて工程IDが未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.FacilityParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Facility.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.FacilityParentNumber != null && !strokeDic.ContainsKey(result.FacilityParentNumber))
                {
                    // 存在しない工程IDが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.FacilityParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Facility.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!TMQUtil.commonTextByteCheckExcelPort(result.FacilityName, out int maxLength))
                {
                    // 文字数チェック
                    // アイテム翻訳は○○桁以下で入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.FacilityName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Facility.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // アイテムの重複チェック
                if (result.FacilityName != result.FacilityNameBefore)
                {
                    int strokeId = (int)strokeDic[result.FacilityParentNumber].ParentId;
                    int seriesId = (int)seriesDic[strokeId].ParentId;
                    int plantId = (int)plantDic[seriesId].ParentId;
                    long factoryId = (long)factoryDic[plantId];
                    int cnt = 0;
                    TMQUtil.GetCountDb(new
                    {
                        @LocationStructureId = factoryId,
                        @LanguageId = this.LanguageId,
                        @TranslationText = result.FacilityName,
                        @StructureGroupId = structureGroupId,
                        @FactoryId = factoryId,
                        @StructureLayerNo = (int)TMQConst.MsStructure.StructureLayerNo.Location.Facility,
                        @ParentStructureId = strokeDic[result.FacilityParentNumber].StructureId
                    }, Master.SqlName.GetCountLayersTranslationByFactory, ref cnt, db, Master.SqlName.ComLayersDir);

                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.FacilityName, null, GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1000.Facility.ControlGroupId));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                // 登録処理
                if (!executeExcelPortRegistFacility(result, seriesDic, factoryDic, plantDic, strokeDic, now))
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
        /// プラント登録処理
        /// </summary>
        /// <param name="factoryDic">工場IDディクショナリ</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegistPlant(TMQUtil.CommonExcelPortMasterStructureList plant, Dictionary<int?, long?> factoryDic, ref Dictionary<int?, ExcelPortStructureList> plantDic, DateTime now)
        {

            // プラント登録処理

            // 所属情報が変更されている場合は削除
            bool isNew = false;
            if (plant.PlantParentNumber != null &&
                plant.PlantParentNumberBefore != null &&
                plant.PlantParentNumber != plant.PlantParentNumberBefore)
            {
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, Master.SqlName.SubDir, new { StructureId = plant.PlantId, UpdateDatetime = now, UpdateUserId = this.UserId }, db))
                {
                    return false;
                }
                isNew = true;
            }

            // 翻訳マスタ登録処理
            if (!TMQUtil.registTranslationStructureExcelPort(structureGroupId,                                          // 構成グループID
                                                             now,                                                       // 現在日時
                                                             this.LanguageId,                                           // 言語ID
                                                             this.db,                                                   // DBクラス
                                                             this.UserId,                                               // ユーザーID
                                                             plant.PlantName,                                           // 翻訳名
                                                             plant.PlantNameBefore,                                     // 変更前翻訳名
                                                             plant.PlantItemTranslationId,                              // 翻訳ID
                                                             (int)factoryDic[plant.PlantParentNumber],                  // 工場ID
                                                             plant.PlantId,                                             // 構成ID
                                                             (int)TMQConst.MsStructure.StructureLayerNo.Location.Plant, // 階層番号
                                                             (int)factoryDic[plant.PlantParentNumber],                  // 親構成ID
                                                             out int transId,                                           // 翻訳ID
                                                             isNew))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!TMQUtil.registItemStructureExcelPort(plant.PlantId,                  // 構成ID
                                            structureGroupId,                         // 構成グループID
                                            (int)factoryDic[plant.PlantParentNumber], // 工場ID
                                            plant.PlantName,                          // 翻訳名
                                            plant.PlantNameBefore,                    // 変更前翻訳名
                                            plant.PlantItemTranslationId,             // 翻訳ID
                                            plant.PlantItemId,                        // アイテムID
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
            if (!TMQUtil.registStructureExcelPort(structureGroupId,                                         // 構成グループID
                                                 plant.PlantId,                                             // 構成ID
                                                 (int)factoryDic[plant.PlantParentNumber],                  // 工場ID
                                                 itemId,                                                    // アイテムID
                                                 (int)factoryDic[plant.PlantParentNumber],                  // 親構成ID
                                                 (int)TMQConst.MsStructure.StructureLayerNo.Location.Plant, // 階層番号
                                                 now,                                                       // 現在日時
                                                 this.db,                                                   // DBクラス
                                                 this.UserId,                                               // ユーザーID
                                                 isNew,
                                                 out int structureId))
            {
                return false;
            }

            // プラントディクショナリに登録した情報を追加
            plantDic[plant.PlantNumber].StructureId = structureId;

            return true;
        }

        /// <summary>
        /// 系列登録処理
        /// </summary>
        /// <param name="plantDic">プラントIDディクショナリ</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegistSeries(TMQUtil.CommonExcelPortMasterStructureList series, Dictionary<int?, ExcelPortStructureList> plantDic, Dictionary<int?, long?> factoryDic, ref Dictionary<int?, ExcelPortStructureList> seriesDic, DateTime now)
        {
            bool parentChanged = false;

            if (series.SeriesId != null)
            {
                // DBより自分自身の親構成IDを取得
                ComDao.MsStructureEntity ms = new ComDao.MsStructureEntity().GetEntity((int)series.SeriesId, this.db);
                // 自分の親構成IDを比較(異なる場合は自分の親アイテムの所属が変わった)
                parentChanged = ms.ParentStructureId != plantDic[series.SeriesParentNumber].StructureId;
            }

            // 所属情報が変更されている場合は削除
            bool isNew = false;
            if ((series.SeriesParentNumber != null &&
                series.SeriesParentNumberBefore != null &&
                series.SeriesParentNumber != series.SeriesParentNumberBefore) ||
                parentChanged)
            {
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, Master.SqlName.SubDir, new { StructureId = series.SeriesId, UpdateDatetime = now, UpdateUserId = this.UserId }, db))
                {
                    return false;
                }
                isNew = true;
            }

            int factoryId = (int)factoryDic[(int)plantDic[series.SeriesParentNumber].ParentId];
            // 翻訳マスタ登録処理
            if (!TMQUtil.registTranslationStructureExcelPort(structureGroupId,                                           // 構成グループID
                                                             now,                                                        // 現在日時
                                                             this.LanguageId,                                            // 言語ID
                                                             this.db,                                                    // DBクラス
                                                             this.UserId,                                                // ユーザーID
                                                             series.SeriesName,                                          // 翻訳名
                                                             series.SeriesNameBefore,                                    // 変更前翻訳名
                                                             series.SeriesItemTranslationId,                             // 翻訳ID
                                                             factoryId,                                                  // 工場ID
                                                             series.SeriesId,                                            // 構成ID
                                                             (int)TMQConst.MsStructure.StructureLayerNo.Location.Series, // 階層番号
                                                             (int)plantDic[series.SeriesParentNumber].StructureId,       // 親構成ID
                                                             out int transId,                                            // 翻訳ID
                                                             isNew))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!TMQUtil.registItemStructureExcelPort(series.SeriesId,                // 構成ID
                                            structureGroupId,                         // 構成グループID
                                            factoryId,                                // 工場ID
                                            series.SeriesName,                        // 翻訳名
                                            series.SeriesNameBefore,                  // 変更前翻訳名
                                            series.SeriesItemTranslationId,           // 翻訳ID
                                            series.SeriesItemId,                      // アイテムID
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
            if (!TMQUtil.registStructureExcelPort(structureGroupId,                                          // 構成グループID
                                                 series.SeriesId,                                            // 構成ID
                                                 factoryId,                                                  // 工場ID
                                                 itemId,                                                     // アイテムID
                                                 (int)plantDic[series.SeriesParentNumber].StructureId,       // 親構成ID
                                                 (int)TMQConst.MsStructure.StructureLayerNo.Location.Series, // 階層番号
                                                 now,                                                        // 現在日時
                                                 this.db,                                                    // DBクラス
                                                 this.UserId,                                                // ユーザーID
                                                 isNew,
                                                 out int structureId))
            {
                return false;
            }

            // 系列ディクショナリに登録した情報を追加
            seriesDic[series.SeriesNumber].StructureId = structureId;

            return true;
        }

        /// <summary>
        /// 工程登録処理
        /// </summary>
        /// <param name="seriesDic">系列IDディクショナリ</param>
        /// <param name="factoryDic">工場IDディクショナリ</param>
        /// <param name="plantDic">プラントIDディクショナリ</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegistStroke(TMQUtil.CommonExcelPortMasterStructureList stroke, Dictionary<int?, ExcelPortStructureList> seriesDic, Dictionary<int?, long?> factoryDic, Dictionary<int?, ExcelPortStructureList> plantDic, ref Dictionary<int?, ExcelPortStructureList> strokeDic, DateTime now)
        {
            bool parentChanged = false;

            if (stroke.StrokeId != null)
            {
                // DBより自分自身の親構成IDを取得
                ComDao.MsStructureEntity ms = new ComDao.MsStructureEntity().GetEntity((int)stroke.StrokeId, this.db);
                // 自分の親構成IDを比較(異なる場合は自分の親アイテムの所属が変わった)
                parentChanged = ms.ParentStructureId != seriesDic[stroke.StrokeParentNumber].StructureId;
            }

            // 所属情報が変更されている場合は削除
            bool isNew = false;
            if ((stroke.StrokeParentNumber != null &&
                stroke.StrokeParentNumberBefore != null &&
                stroke.StrokeParentNumber != stroke.StrokeParentNumberBefore) ||
                parentChanged)
            {
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, Master.SqlName.SubDir, new { StructureId = stroke.StrokeId, UpdateDatetime = now, UpdateUserId = this.UserId }, db))
                {
                    return false;
                }
                isNew = true;
            }

            int seriesId = (int)seriesDic[stroke.StrokeParentNumber].ParentId;
            int plantId = (int)plantDic[seriesId].ParentId;
            int factoryId = (int)factoryDic[plantId];
            // 翻訳マスタ登録処理
            if (!TMQUtil.registTranslationStructureExcelPort(structureGroupId,                                          // 構成グループID
                                                             now,                                                       // 現在日時
                                                             this.LanguageId,                                           // 言語ID
                                                             this.db,                                                   // DBクラス
                                                             this.UserId,                                               // ユーザーID
                                                             stroke.StrokeName,                                         // 翻訳名
                                                             stroke.StrokeNameBefore,                                   // 変更前翻訳名
                                                             stroke.StrokeItemTranslationId,                            // 翻訳ID
                                                             factoryId,                                                 // 工場ID
                                                             stroke.StrokeId,                                           // 構成ID
                                                             (int)TMQConst.MsStructure.StructureLayerNo.Location.Stroke, // 階層番号
                                                             (int)seriesDic[stroke.StrokeParentNumber].StructureId,        // 親構成ID
                                                             out int transId,                       // 翻訳ID
                                                             isNew))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!TMQUtil.registItemStructureExcelPort(stroke.StrokeId,                // 構成ID
                                            structureGroupId,                         // 構成グループID
                                            factoryId,                                // 工場ID
                                            stroke.StrokeName,                        // 翻訳名
                                            stroke.StrokeNameBefore,                  // 変更前翻訳名
                                            stroke.StrokeItemTranslationId,           // 翻訳ID
                                            stroke.StrokeItemId,                      // アイテムID
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
            if (!TMQUtil.registStructureExcelPort(structureGroupId,                                          // 構成グループID
                                                 stroke.StrokeId,                                            // 構成ID
                                                 factoryId,                                                  // 工場ID
                                                 itemId,                                                     // アイテムID
                                                 (int)seriesDic[stroke.StrokeParentNumber].StructureId,         // 親構成ID
                                                 (int)TMQConst.MsStructure.StructureLayerNo.Location.Stroke, // 階層番号
                                                 now,                                                        // 現在日時
                                                 this.db,                                                    // DBクラス
                                                 this.UserId,                                                // ユーザーID
                                                 isNew,
                                                 out int structureId))
            {
                return false;
            }

            // 工程ディクショナリに登録した情報を追加
            strokeDic[stroke.StrokeNumber].StructureId = structureId;

            return true;
        }

        /// <summary>
        /// 設備登録処理
        /// </summary>
        /// <param name="seriesDic">系列IDディクショナリ</param>
        /// <param name="factoryDic">工場IDディクショナリ</param>
        /// <param name="plantDic">プラントIDディクショナリ</param>
        /// <param name="strokeDic">工程IDディクショナリ</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegistFacility(TMQUtil.CommonExcelPortMasterStructureList facility, Dictionary<int?, ExcelPortStructureList> seriesDic, Dictionary<int?, long?> factoryDic, Dictionary<int?, ExcelPortStructureList> plantDic, Dictionary<int?, ExcelPortStructureList> strokeDic, DateTime now)
        {
            bool parentChanged = false;

            if (facility.FacilityId != null)
            {
                // DBより自分自身の親構成IDを取得
                ComDao.MsStructureEntity ms = new ComDao.MsStructureEntity().GetEntity((int)facility.FacilityId, this.db);
                // 自分の親構成IDを比較(異なる場合は自分の親アイテムの所属が変わった)
                parentChanged = ms.ParentStructureId != strokeDic[facility.FacilityParentNumber].StructureId;
            }

            // 所属情報が変更されている場合は削除
            bool isNew = false;
            if ((facility.FacilityParentNumber != null &&
                facility.FacilityParentNumberBefore != null &&
                facility.FacilityParentNumber != facility.FacilityParentNumberBefore) ||
                parentChanged)
            {
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, Master.SqlName.SubDir, new { StructureId = facility.FacilityId, UpdateDatetime = now, UpdateUserId = this.UserId }, db))
                {
                    return false;
                }
                isNew = true;
            }

            int strokeId = (int)strokeDic[facility.FacilityParentNumber].ParentId;
            int seriesId = (int)seriesDic[strokeId].ParentId;
            int plantId = (int)plantDic[seriesId].ParentId;
            int factoryId = (int)factoryDic[plantId];
            // 翻訳マスタ登録処理
            if (!TMQUtil.registTranslationStructureExcelPort(structureGroupId,                                             // 構成グループID
                                                             now,                                                          // 現在日時
                                                             this.LanguageId,                                              // 言語ID
                                                             this.db,                                                      // DBクラス
                                                             this.UserId,                                                  // ユーザーID
                                                             facility.FacilityName,                                        // 翻訳名
                                                             facility.FacilityNameBefore,                                  // 変更前翻訳名
                                                             facility.FacilityItemTranslationId,                           // 翻訳ID
                                                             factoryId,                                                    // 工場ID
                                                             facility.FacilityId,                                          // 構成ID
                                                             (int)TMQConst.MsStructure.StructureLayerNo.Location.Facility, // 階層番号
                                                             (int)strokeDic[facility.FacilityParentNumber].StructureId,    // 親構成ID
                                                             out int transId,                          // 翻訳ID
                                                             isNew))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!TMQUtil.registItemStructureExcelPort(facility.FacilityId,            // 構成ID
                                            structureGroupId,                         // 構成グループID
                                            factoryId,                                // 工場ID
                                            facility.FacilityName,                    // 翻訳名
                                            facility.FacilityNameBefore,              // 変更前翻訳名
                                            facility.FacilityItemTranslationId,       // 翻訳ID
                                            facility.FacilityItemId,                  // アイテムID
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
            if (!TMQUtil.registStructureExcelPort(structureGroupId,                                            // 構成グループID
                                                 facility.FacilityId,                                          // 構成ID
                                                 factoryId,                                                    // 工場ID
                                                 itemId,                                                       // アイテムID
                                                 (int)strokeDic[facility.FacilityParentNumber].StructureId,    // 親構成ID
                                                 (int)TMQConst.MsStructure.StructureLayerNo.Location.Facility, // 階層番号
                                                 now,                                                          // 現在日時
                                                 this.db,                                                      // DBクラス
                                                 this.UserId,                                                  // ユーザーID
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
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.HiddenId, this.pageInfoList);

            // 非表示情報取得
            var hiddenInfo = TMQUtil.GetHiddenInfoForMaster(structureGroupId, itemListType, this.UserId, db);
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
            string ctrlId = ConductInfo.FormList.ControlId.FactoryItemId;

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

            // アイテム一覧の取得＆設定

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.FactoryItemId, this.pageInfoList);

            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = ComUtil.GetNotNullNameByClass<Dao.SearchCondition>(condition);

            // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetLocationStructureItemList, out string baseSql, listUnComment);

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
            // ページ情報
            var pageInfo = GetPageInfo(ConductInfo.FormEdit.ControlId.ItemInfoId, this.pageInfoList);

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
            var results = TMQUtil.SqlExecuteClass.SelectList<TMQUtil.SearchResultForMaster>(Master.SqlName.GetLayersItemOrderList, Master.SqlName.ComLayersDir, condition, this.db);

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
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive()
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
            ctrlId = ConductInfo.FormEdit.ControlId.ItemInfoId;

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
            var newList = TMQUtil.SqlExecuteClass.SelectList<TMQUtil.SearchResultForMaster>(Master.SqlName.GetLayersItemOrderList, Master.SqlName.ComLayersDir, condition, this.db);

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

                    // 工場ID
                    result.FactoryId = hiddenInfo.FactoryId;
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
                    if (!MS1001_CheckDuplicateByItemTran(hiddenInfo, result, ref errFlg))
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
                bool MS1001_CheckDuplicateByItemTran(
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

                    // 構成グループの選択工場の対象階層番号に同じ翻訳が存在するか検索

                    // 件数取得
                    int cnt = 0;
                    if (!TMQUtil.GetCountDb(targetInfo, Master.SqlName.GetCountLayersTranslationByFactory, ref cnt, db, Master.SqlName.ComLayersDir))
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
            string ctrlId = ConductInfo.FormEdit.ControlId.ItemInfoId;
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