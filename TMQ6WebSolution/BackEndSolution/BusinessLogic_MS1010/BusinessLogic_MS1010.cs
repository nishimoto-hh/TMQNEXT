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
using Dao = BusinessLogic_MS1010.BusinessLogicDataClass_MS1010;
using Master = CommonTMQUtil.CommonTMQUtil.ComMaster;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_MS1010
{
    /// <summary>
    /// 職種・機種マスタ
    /// </summary>
    public class BusinessLogic_MS1010 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 構成グループID
        /// </summary>
        private static int structureGroupId = 1010;

        /// <summary>
        /// 拡張項目件数
        /// </summary>
        private static int itemExCnt = 1;

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
            /// 職種ID
            /// </summary>
            public const int JobNo = 24;
            /// <summary>
            /// 職種名
            /// </summary>
            public const int JobName = 25;
            /// <summary>
            /// 工場番号
            /// </summary>
            public const int JobParentNumber = 26;
            /// <summary>
            /// 保全実績集計コード
            /// </summary>
            public const int JobCode = 28;
            /// <summary>
            /// 機種大分類ID
            /// </summary>
            public const int LargeClassNo = 38;
            /// <summary>
            /// 機種大分類名
            /// </summary>
            public const int LargeClassName = 39;
            /// <summary>
            /// 職種番号
            /// </summary>
            public const int LargeClassParentNumber = 40;
            /// <summary>
            /// 機種中分類ID
            /// </summary>
            public const int MiddleClassNo = 50;
            /// <summary>
            /// 機種中分類名
            /// </summary>
            public const int MiddleClassName = 51;
            /// <summary>
            /// 機種大分類番号
            /// </summary>
            public const int MiddleClassParentNumber = 52;
            /// <summary>
            /// 機種小分類ID
            /// </summary>
            public const int SmallClassNo = 62;
            /// <summary>
            /// 機種小分類名
            /// </summary>
            public const int SmallClassName = 63;
            /// <summary>
            /// 機種中分類番号
            /// </summary>
            public const int SmallClassParentNumber = 64;
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
            /// <summary>SQL名：職種・機種アイテム一覧取得</summary>
            public const string GetJobClassficationItemList = "GetJobClassficationItemList";
            /// <summary>SQL名：職種・機種アイテム情報取得</summary>
            public const string GetJobClassficationItemInfo = "GetJobClassficationItemInfo";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = Master.SqlName.SubDir + @"\JobClassfication";
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
        public BusinessLogic_MS1010() : base()
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
                (int)TMQConst.MsStructure.StructureLayerNo.Job.Job,
                (int)TMQConst.MsStructure.StructureLayerNo.Job.LargeClassfication,
                (int)TMQConst.MsStructure.StructureLayerNo.Job.MiddleClassfication,
                (int)TMQConst.MsStructure.StructureLayerNo.Job.SmallClassfication
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
                    IList<TMQUtil.CommonExcelPortMasterJobList> masterReaults = getMasterResults(searchCondition);
                    if (masterReaults == null)
                    {
                        return false;
                    }

                    // Dicitionalyに変換
                    dataList = ComUtil.ConvertClassToDictionary<TMQUtil.CommonExcelPortMasterJobList>(masterReaults);
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
        private List<TMQUtil.CommonExcelPortMasterJobList> getMasterResults(TMQUtil.CommonExcelPortMasterCondition searchCondition)
        {
            // 地区情報を取得
            if (!getDistrictResults(out IList<TMQUtil.CommonExcelPortMasterStructureList> districtResults, out Dictionary<long?, int?> dicDistrict))
            {
                return new List<TMQUtil.CommonExcelPortMasterJobList>();
            }

            // 工場情報を取得
            if (!getFactoryResults(dicDistrict, out IList<TMQUtil.CommonExcelPortMasterStructureList> factoryResults, out List<long> factoryIdList, out Dictionary<long?, int?> dicFactory))
            {
                return new List<TMQUtil.CommonExcelPortMasterJobList>();
            }

            // 工場IDリストを設定
            searchCondition.FactoryIdList = factoryIdList;

            // 職種情報を取得
            if (!getJobResults(dicFactory, out IList<TMQUtil.CommonExcelPortMasterJobList> jobResults, out Dictionary<long?, int?> dicJob))
            {
                return new List<TMQUtil.CommonExcelPortMasterJobList>();
            }

            // 機種大分類情報を取得
            if (!getLargeClassResults(dicJob, out IList<TMQUtil.CommonExcelPortMasterJobList> largeClassResults, out Dictionary<long?, int?> dicLargeClass))
            {
                return new List<TMQUtil.CommonExcelPortMasterJobList>();
            }

            // 機種中分類情報を取得
            if (!getMiddleClassResults(dicLargeClass, out IList<TMQUtil.CommonExcelPortMasterJobList> middleClassResults, out Dictionary<long?, int?> dicMiddleClass))
            {
                return new List<TMQUtil.CommonExcelPortMasterJobList>();
            }

            // 機種小分類情報取得
            if (!getSmallClassResults(dicMiddleClass, out IList<TMQUtil.CommonExcelPortMasterJobList> smallClassResults))
            {
                return new List<TMQUtil.CommonExcelPortMasterJobList>();
            }

            // 取得した「地区」「工場」「職種」「機種大分類」「機種中分類」「機種小分類」のうち、最大のデータ件数を出力データのレコード数とする
            int[] dataCnts = new int[]
            { districtResults.Count,
              factoryResults.Count,
              jobResults.Count,
              largeClassResults.Count,
              middleClassResults.Count,
              smallClassResults.Count
            };
            int recordNum = dataCnts.Max();

            // 取得データを1レコード単位にまとめる
            List<TMQUtil.CommonExcelPortMasterJobList> results = new();
            for (int i = 0; i < recordNum; i++)
            {
                TMQUtil.CommonExcelPortMasterJobList record = new();

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

                // 職種情報
                if (jobResults.Count != 0)
                {
                    record.JobItemId = jobResults[0].JobItemId;                       // 職種アイテムID
                    record.JobId = jobResults[0].JobId;                               // 職種ID(構成ID)
                    record.JobItemTranslationId = jobResults[0].JobItemTranslationId; // 翻訳ID(職種)
                    record.JobNumber = jobResults[0].JobNumber;                       // 職種番号
                    record.JobName = jobResults[0].JobName;                           // 職種名
                    record.JobNameBefore = jobResults[0].JobNameBefore;               // 職種名
                    record.JobParentId = jobResults[0].JobParentId;                   // 職種の親構成ID
                    record.JobParentNumber = jobResults[0].JobParentNumber;           // 工場番号
                    record.JobParentNumberBefore = jobResults[0].JobParentNumber;     // 工場番号
                    record.JobCodeVal = jobResults[0].JobCodeVal;                     // 保全実績集計職種コード
                    record.JobCode = jobResults[0].JobCode;                           // 保全実績集計職種コード
                    jobResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 機種大分類情報
                if (largeClassResults.Count != 0)
                {
                    record.LargeClassItemId = largeClassResults[0].LargeClassItemId;                       // 機種大分類アイテムID
                    record.LargeClassId = largeClassResults[0].LargeClassId;                               // 機種大分類ID(構成ID)
                    record.LargeClassItemTranslationId = largeClassResults[0].LargeClassItemTranslationId; // 翻訳ID(機種大分類)
                    record.LargeClassNumber = largeClassResults[0].LargeClassNumber;                       // 機種大分類番号
                    record.LargeClassName = largeClassResults[0].LargeClassName;                           // 機種大分類名
                    record.LargeClassNameBefore = largeClassResults[0].LargeClassNameBefore;               // 機種大分類名
                    record.LargeClassParentId = largeClassResults[0].LargeClassParentId;                   // 機種大分類の親構成ID
                    record.LargeClassParentNumber = largeClassResults[0].LargeClassParentNumber;           // 職種番号
                    record.LargeClassParentNumberBefore = largeClassResults[0].LargeClassParentNumber;     // 職種番号
                    largeClassResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 機種中分類情報
                if (middleClassResults.Count != 0)
                {
                    record.MiddleClassItemId = middleClassResults[0].MiddleClassItemId;                        // 機種中分類アイテムID
                    record.MiddleClassId = middleClassResults[0].MiddleClassId;                               // 機種中分類ID(構成ID)
                    record.MiddleClassItemTranslationId = middleClassResults[0].MiddleClassItemTranslationId; // 翻訳ID(機種中分類)
                    record.MiddleClassNumber = middleClassResults[0].MiddleClassNumber;                       // 機種中分類番号
                    record.MiddleClassName = middleClassResults[0].MiddleClassName;                           // 機種中分類名
                    record.MiddleClassNameBefore = middleClassResults[0].MiddleClassNameBefore;               // 機種中分類名
                    record.MiddleClassParentId = middleClassResults[0].MiddleClassParentId;                   // 機種中分類の親構成ID
                    record.MiddleClassParentNumber = middleClassResults[0].MiddleClassParentNumber;           // 機種大分類番号
                    record.MiddleClassParentNumberBefore = middleClassResults[0].MiddleClassParentNumber;     // 機種大分類番号
                    middleClassResults.RemoveAt(0); // 先頭のデータを削除
                }

                // 機種小分類情報
                if (smallClassResults.Count != 0)
                {
                    record.SmallClassItemId = smallClassResults[0].SmallClassItemId;                       // 機種小分類アイテムID
                    record.SmallClassId = smallClassResults[0].SmallClassId;                               // 機種小分類ID(構成ID)
                    record.SmallClassItemTranslationId = smallClassResults[0].SmallClassItemTranslationId; // 翻訳ID(機種小分類)
                    record.SmallClassNumber = smallClassResults[0].SmallClassNumber;                       // 機種小分類番号
                    record.SmallClassName = smallClassResults[0].SmallClassName;                           // 機種小分類名
                    record.SmallClassNameBefore = smallClassResults[0].SmallClassNameBefore;               // 機種小分類名
                    record.SmallClassParentId = smallClassResults[0].SmallClassParentId;                   // 機種小分類の親構成ID
                    record.SmallClassParentNumber = smallClassResults[0].SmallClassParentNumber;           // 機種中分類番号
                    record.SmallClassParentNumberBefore = smallClassResults[0].SmallClassParentNumber;     // 機種中分類番号
                    smallClassResults.RemoveAt(0); // 先頭のデータを削除
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

            // 職種情報を取得
            bool getJobResults(Dictionary<long?, int?> dicFactory, out IList<TMQUtil.CommonExcelPortMasterJobList> jobResults, out Dictionary<long?, int?> dicJob)
            {
                jobResults = null;
                dicJob = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterJobList, out string jobSql);

                // SQL実行
                jobResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterJobList>(jobSql, searchCondition);
                if (jobResults == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 職種の親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterJobList jobResult in jobResults)
                {
                    if (dicFactory.ContainsKey(jobResult.JobParentId))
                    {
                        jobResult.JobParentNumber = (int)dicFactory[jobResult.JobParentId];
                        dicJob.Add(jobResult.JobId, jobResult.JobNumber);
                    }
                }

                return true;
            }

            // 機種大分類情報を取得
            bool getLargeClassResults(Dictionary<long?, int?> dicJob, out IList<TMQUtil.CommonExcelPortMasterJobList> largeClassResults, out Dictionary<long?, int?> dicLargeClass)
            {
                largeClassResults = null;
                dicLargeClass = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterLargeClassList, out string largeClassSql);

                // SQL実行
                largeClassResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterJobList>(largeClassSql, searchCondition);
                if (largeClassResults == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 機種大分類の親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterJobList largeClassResult in largeClassResults)
                {
                    if (dicJob.ContainsKey(largeClassResult.LargeClassParentId))
                    {
                        largeClassResult.LargeClassParentNumber = (int)dicJob[largeClassResult.LargeClassParentId];
                        dicLargeClass.Add(largeClassResult.LargeClassId, largeClassResult.LargeClassNumber);
                    }
                }

                return true;
            }

            // 機種中分類情報を取得
            bool getMiddleClassResults(Dictionary<long?, int?> dicLargeClass, out IList<TMQUtil.CommonExcelPortMasterJobList> middleClassResults, out Dictionary<long?, int?> dicMiddleClass)
            {
                middleClassResults = null;
                dicMiddleClass = new();

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterMiddleClassList, out string middleClassSql);

                // SQL実行
                middleClassResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterJobList>(middleClassSql, searchCondition);
                if (middleClassResults == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 機種中分類の親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterJobList middleClassResult in middleClassResults)
                {
                    if (dicLargeClass.ContainsKey(middleClassResult.MiddleClassParentId))
                    {
                        middleClassResult.MiddleClassParentNumber = (int)dicLargeClass[middleClassResult.MiddleClassParentId];
                        dicMiddleClass.Add(middleClassResult.MiddleClassId, middleClassResult.MiddleClassNumber);
                    }
                }

                return true;
            }

            // 機種小分類情報を取得
            bool getSmallClassResults(Dictionary<long?, int?> dicMiddleClass, out IList<TMQUtil.CommonExcelPortMasterJobList> smallClassResults)
            {
                smallClassResults = null;

                // SQL取得
                TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterSmallClassList, out string smallClassSql);

                // SQL実行
                smallClassResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterJobList>(smallClassSql, searchCondition);
                if (smallClassResults == null)
                {
                    // 「ダウンロード処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                    return false;
                }

                // 機種小分類の親IDを設定
                foreach (TMQUtil.CommonExcelPortMasterJobList smallClassResult in smallClassResults)
                {
                    if (dicMiddleClass.ContainsKey(smallClassResult.SmallClassParentId))
                    {
                        smallClassResult.SmallClassParentNumber = (int)dicMiddleClass[smallClassResult.SmallClassParentId];
                    }

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
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.District.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterJobList> resultDistrictList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(工場)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Factory.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterJobList> resultFactoryList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(職種)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Job.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterJobList> resultJobList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(機種大分類)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Large.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterJobList> resultLargeList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(機種中分類)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Middle.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterJobList> resultMiddleList, out resultMsg, ref fileType, ref fileName, ref ms);

            // ExcelPortアップロードデータの取得(機種小分類)
            excelPort.GetUploadDataList(file, this.IndividualDictionary, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Small.ControlGroupId,
                out List<TMQUtil.CommonExcelPortMasterJobList> resultSmallList, out resultMsg, ref fileType, ref fileName, ref ms);

            // 各階層のリストを作成する
            // 地区
            Dictionary<int?, long?> districtDic = new();
            foreach (TMQUtil.CommonExcelPortMasterJobList result in resultDistrictList)
            {
                // 地区リスト
                if (result.DistrictNumber != null)
                {
                    districtDic.Add(result.DistrictNumber, result.DistrictId);
                }
            }

            // 工場
            Dictionary<int?, long?> factoryDic = new();
            foreach (TMQUtil.CommonExcelPortMasterJobList result in resultFactoryList)
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

            // 保全実績集計職種コードのリストを作成
            List<string> jobCodeList = new();
            foreach (TMQConst.MsStructure.StructureId.JobCode jobCode in Enum.GetValues(typeof(TMQConst.MsStructure.StructureId.JobCode)))
            {
                jobCodeList.Add(((int)jobCode).ToString());
            }

            // 職種
            Dictionary<int?, ExcelPortStructureList> jobDic = new();
            foreach (TMQUtil.CommonExcelPortMasterJobList result in resultJobList)
            {
                rowErrFlg = false;

                // 職種IDが入力されていない場合はエラー
                if (result.JobNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.JobNo, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Job.ControlGroupId));
                    rowErrFlg = true;
                }

                // 工場IDが入力されていない場合はエラー
                if (result.JobParentNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.JobParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Job.ControlGroupId));
                    rowErrFlg = true;
                }

                // 保全実績集計職種コード(コンボ)が入力されていて、選択が不正(指定されたコード以外)の場合はエラー
                if (!string.IsNullOrEmpty(result.JobCode) && !jobCodeList.Contains(result.JobCode))
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.JobCode, null, GetResMessage(new string[] { ComRes.ID.ID141140004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Job.ControlGroupId));
                    rowErrFlg = true;
                }

                // 該当行でエラーがある場合はここで終了
                if (rowErrFlg)
                {
                    continue;
                }

                // コンボに入力されている内容を正とする
                result.JobCodeVal = result.JobCode; // 保全実績集計職種コード

                // 職種リスト
                if (result.JobNumber != null)
                {
                    ExcelPortStructureList list = new();
                    list.StructureId = result.JobId;
                    list.ParentId = result.JobParentNumber;
                    jobDic.Add(result.JobNumber, list);
                }
            }

            // 機種大分類
            Dictionary<int?, ExcelPortStructureList> largeDic = new();
            foreach (TMQUtil.CommonExcelPortMasterJobList result in resultLargeList)
            {
                rowErrFlg = false;

                // 機種大分類IDが入力されていない場合はエラー
                if (result.LargeClassNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.LargeClassNo, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Large.ControlGroupId));
                    rowErrFlg = true;
                }

                // 職種IDが入力されていない場合はエラー
                if (result.LargeClassParentNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.LargeClassParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Large.ControlGroupId));
                    rowErrFlg = true;
                }

                // 該当行でエラーがある場合はここで終了
                if (rowErrFlg)
                {
                    continue;
                }

                // 機種大分類リスト
                if (result.LargeClassNumber != null)
                {
                    ExcelPortStructureList list = new();
                    list.StructureId = result.LargeClassId;
                    list.ParentId = result.LargeClassParentNumber;
                    largeDic.Add(result.LargeClassNumber, list);
                }
            }

            // 機種中分類
            Dictionary<int?, ExcelPortStructureList> middleDic = new();
            foreach (TMQUtil.CommonExcelPortMasterJobList result in resultMiddleList)
            {
                rowErrFlg = false;

                // 機種中分類IDが入力されていない場合はエラー
                if (result.MiddleClassNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.MiddleClassNo, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Middle.ControlGroupId));
                    rowErrFlg = true;
                }

                // 機種大分類IDが入力されていない場合はエラー
                if (result.MiddleClassParentNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.MiddleClassParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Middle.ControlGroupId));
                    rowErrFlg = true;
                }

                // 該当行でエラーがある場合はここで終了
                if (rowErrFlg)
                {
                    continue;
                }

                // 機種中分類リスト
                if (result.MiddleClassNumber != null)
                {
                    ExcelPortStructureList list = new();
                    list.StructureId = result.MiddleClassId;
                    list.ParentId = result.MiddleClassParentNumber;
                    middleDic.Add(result.MiddleClassNumber, list);
                }
            }

            // 機種小分類
            Dictionary<int?, ExcelPortStructureList> smallDic = new();
            foreach (TMQUtil.CommonExcelPortMasterJobList result in resultSmallList)
            {
                rowErrFlg = false;

                // 機種中分類IDが入力されていない場合はエラー
                if (result.SmallClassNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SmallClassNo, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Small.ControlGroupId));
                    rowErrFlg = true;
                }

                // 機種大分類IDが入力されていない場合はエラー
                if (result.SmallClassParentNumber == null)
                {
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SmallClassParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941130004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Small.ControlGroupId));
                    rowErrFlg = true;
                }

                // 該当行でエラーがある場合はここで終了
                if (rowErrFlg)
                {
                    continue;
                }

                // 機種小分類リスト
                if (result.SmallClassNumber != null)
                {
                    ExcelPortStructureList list = new();
                    list.StructureId = result.SmallClassId;
                    list.ParentId = result.SmallClassParentNumber;
                    smallDic.Add(result.SmallClassNumber, list);
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

            // 職種 入力チェック&登録処理
            if (!executeCheckAndRegistJobExcelPort(ref resultJobList, ref errorInfoList, factoryDic, ref jobDic))
            {
                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                }

                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機種大分類 入力チェック&登録処理
            if (!executeCheckAndRegistLargeExcelPort(ref resultLargeList, ref errorInfoList, factoryDic, jobDic, ref largeDic))
            {
                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                }

                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機種中分類 入力チェック&登録処理
            if (!executeCheckAndRegistMiddleExcelPort(ref resultMiddleList, ref errorInfoList, factoryDic, jobDic, largeDic, ref middleDic))
            {
                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                }

                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機種小分類入力チェック&登録処理
            if (!executeCheckAndRegistSmallExcelPort(ref resultSmallList, ref errorInfoList, factoryDic, jobDic, largeDic, middleDic))
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
        /// 職種 入力チェック&登録処理
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistJobExcelPort(ref List<TMQUtil.CommonExcelPortMasterJobList> resultJobList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dictionary<int?, long?> factoryDic, ref Dictionary<int?, ExcelPortStructureList> jobDic)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            foreach (TMQUtil.CommonExcelPortMasterJobList result in resultJobList)
            {
                // 職種
                if ((!string.IsNullOrEmpty(result.JobName) || result.JobParentNumber != null) && result.JobNumber == null)
                {
                    // 職種IDが未入力かつ、職種名・工場IDのどちらかが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.JobNo, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Job.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.JobNumber != null && string.IsNullOrEmpty(result.JobName))
                {
                    // 職種IDが入力されていて職種名が未入力の場合
                    // アイテム翻訳は○○桁以下で入力して下さい。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.JobName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, TMQUtil.ItemTranslasionMaxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Job.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!TMQUtil.commonTextByteCheckExcelPort(result.JobName, out int maxLength))
                {
                    // 文字数チェック
                    // アイテム翻訳は○○桁以下で入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.JobName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Job.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.JobNumber != null && result.JobParentNumber == null)
                {
                    // 職種IDが入力されていて工場IDが未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.JobParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Job.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.JobParentNumber != null && !factoryDic.ContainsKey(result.JobParentNumber))
                {
                    // 存在しない工場IDが入力されている場合
                    // 入力内容が不正です。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.JobParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID141220008 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Job.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // アイテムの重複チェック
                if (result.JobName != result.JobNameBefore)
                {
                    long factoryId = (long)factoryDic[result.JobParentNumber];
                    int cnt = 0;
                    TMQUtil.GetCountDb(new
                    {
                        @LocationStructureId = factoryId,
                        @LanguageId = this.LanguageId,
                        @TranslationText = result.JobName,
                        @StructureGroupId = structureGroupId,
                        @FactoryId = factoryId,
                        @StructureLayerNo = (int)TMQConst.MsStructure.StructureLayerNo.Job.Job,
                        @ParentStructureId = 0
                    }, Master.SqlName.GetCountLayersTranslationByFactory, ref cnt, db, Master.SqlName.ComLayersDir);

                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.JobName, null, GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Job.ControlGroupId));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                // 登録処理
                if (!executeExcelPortRegistJob(result, factoryDic, ref jobDic, now))
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
        /// 機種大分類 入力チェック&登録処理
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistLargeExcelPort(ref List<TMQUtil.CommonExcelPortMasterJobList> resultLargeList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dictionary<int?, long?> factoryDic, Dictionary<int?, ExcelPortStructureList> jobDic, ref Dictionary<int?, ExcelPortStructureList> largeDic)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            foreach (TMQUtil.CommonExcelPortMasterJobList result in resultLargeList)
            {
                // 機種大分類
                if ((!string.IsNullOrEmpty(result.LargeClassName) || result.LargeClassParentNumber != null) && result.LargeClassNumber == null)
                {
                    // 機種大分類IDが未入力かつ、機種大分類名・職種IDのどちらかが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.LargeClassNo, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Large.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.LargeClassNumber != null && string.IsNullOrEmpty(result.LargeClassName))
                {
                    // 機種大分類IDが入力されていて機種大分類名が未入力の場合
                    // アイテム翻訳は○○桁以下で入力して下さい。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.LargeClassName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, TMQUtil.ItemTranslasionMaxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Large.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!TMQUtil.commonTextByteCheckExcelPort(result.LargeClassName, out int maxLength))
                {
                    // 文字数チェック
                    // アイテム翻訳は○○桁以下で入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.LargeClassName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Large.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.LargeClassNumber != null && result.LargeClassParentNumber == null)
                {
                    // 機種大分類IDが入力されていて工場IDが未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.LargeClassParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Large.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.LargeClassParentNumber != null && !jobDic.ContainsKey(result.LargeClassParentNumber))
                {
                    // 存在しない職種IDが入力されている場合
                    // 入力内容が不正です。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.LargeClassParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID141220008 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Large.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // アイテムの重複チェック
                if (result.LargeClassName != result.LargeClassNameBefore)
                {
                    int jobId = (int)jobDic[result.LargeClassParentNumber].ParentId;
                    long factoryId = (long)factoryDic[jobId];
                    int cnt = 0;
                    TMQUtil.GetCountDb(new
                    {
                        @LocationStructureId = factoryId,
                        @LanguageId = this.LanguageId,
                        @TranslationText = result.LargeClassName,
                        @StructureGroupId = structureGroupId,
                        @FactoryId = factoryId,
                        @StructureLayerNo = (int)TMQConst.MsStructure.StructureLayerNo.Job.LargeClassfication,
                        @ParentStructureId = jobDic[result.LargeClassParentNumber].StructureId
                    }, Master.SqlName.GetCountLayersTranslationByFactory, ref cnt, db, Master.SqlName.ComLayersDir);

                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.LargeClassName, null, GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Large.ControlGroupId));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                // 登録処理
                if (!executeExcelPortRegistLarge(result, jobDic, factoryDic, ref largeDic, now))
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
        /// 機種中分類 入力チェック&登録処理
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistMiddleExcelPort(ref List<TMQUtil.CommonExcelPortMasterJobList> resultMiddleList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dictionary<int?, long?> factoryDic, Dictionary<int?, ExcelPortStructureList> jobDic, Dictionary<int?, ExcelPortStructureList> largeDic, ref Dictionary<int?, ExcelPortStructureList> middleDic)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            foreach (TMQUtil.CommonExcelPortMasterJobList result in resultMiddleList)
            {
                // 機種中分類
                if ((!string.IsNullOrEmpty(result.MiddleClassName) || result.MiddleClassParentNumber != null) && result.MiddleClassNumber == null)
                {
                    // 機種中分類IDが未入力かつ、機種中分類名・機種大分類IDのどちらかが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.MiddleClassNo, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Middle.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.MiddleClassNumber != null && string.IsNullOrEmpty(result.MiddleClassName))
                {
                    // 機種中分類IDが入力されていて機種中分類名が未入力の場合
                    // アイテム翻訳は○○桁以下で入力して下さい。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.MiddleClassName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, TMQUtil.ItemTranslasionMaxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Middle.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!TMQUtil.commonTextByteCheckExcelPort(result.MiddleClassName, out int maxLength))
                {
                    // 文字数チェック
                    // アイテム翻訳は○○桁以下で入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.MiddleClassName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Middle.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.MiddleClassNumber != null && result.MiddleClassParentNumber == null)
                {
                    // 機種中分類IDが入力されていて機種大分類IDが未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.MiddleClassParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Middle.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.MiddleClassParentNumber != null && !largeDic.ContainsKey(result.MiddleClassParentNumber))
                {
                    // 存在しない機種大分類IDが入力されている場合
                    // 入力内容が不正です。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.MiddleClassParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID141220008 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Middle.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // アイテムの重複チェック
                if (result.MiddleClassName != result.MiddleClassNameBefore)
                {
                    int largeId = (int)largeDic[result.MiddleClassParentNumber].ParentId;
                    int jobId = (int)jobDic[largeId].ParentId;
                    long factoryId = (long)factoryDic[jobId];
                    int cnt = 0;
                    TMQUtil.GetCountDb(new
                    {
                        @LocationStructureId = factoryId,
                        @LanguageId = this.LanguageId,
                        @TranslationText = result.MiddleClassName,
                        @StructureGroupId = structureGroupId,
                        @FactoryId = factoryId,
                        @StructureLayerNo = (int)TMQConst.MsStructure.StructureLayerNo.Job.MiddleClassfication,
                        @ParentStructureId = largeDic[result.MiddleClassParentNumber].StructureId
                    }, Master.SqlName.GetCountLayersTranslationByFactory, ref cnt, db, Master.SqlName.ComLayersDir);

                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.MiddleClassName, null, GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Middle.ControlGroupId));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                //　登録処理
                if (!executeExcelPortRegistMiddle(result, jobDic, factoryDic, largeDic, ref middleDic, now))
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
        /// 機種小分類 入力チェック&登録処理
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistSmallExcelPort(ref List<TMQUtil.CommonExcelPortMasterJobList> resultSmallList, ref List<ComDao.UploadErrorInfo> errorInfoList, Dictionary<int?, long?> factoryDic, Dictionary<int?, ExcelPortStructureList> jobDic, Dictionary<int?, ExcelPortStructureList> largeDic, Dictionary<int?, ExcelPortStructureList> middleDic)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            foreach (TMQUtil.CommonExcelPortMasterJobList result in resultSmallList)
            {
                // 機種小分類
                if ((!string.IsNullOrEmpty(result.SmallClassName) || result.SmallClassParentNumber != null) && result.SmallClassNumber == null)
                {
                    // 機種小分類IDが未入力かつ、機種小分類名・機種中分類IDのどちらかが入力されている場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SmallClassNo, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Small.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.SmallClassNumber != null && string.IsNullOrEmpty(result.SmallClassName))
                {
                    // 機種小分類IDが入力されていて機種小分類名が未入力の場合
                    // アイテム翻訳は○○桁以下で入力して下さい。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SmallClassName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, TMQUtil.ItemTranslasionMaxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Small.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (!TMQUtil.commonTextByteCheckExcelPort(result.SmallClassName, out int maxLength))
                {
                    // 文字数チェック
                    // アイテム翻訳は○○桁以下で入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SmallClassName, null, GetResMessage(new string[] { ComRes.ID.ID941260004, ComRes.ID.ID111010005, maxLength.ToString() }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Small.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.SmallClassNumber != null && result.SmallClassParentNumber == null)
                {
                    // 機種小分類IDが入力されていて工場IDが未入力の場合
                    // 必須項目です。入力してください。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SmallClassParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Small.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }
                if (result.SmallClassParentNumber != null && !middleDic.ContainsKey(result.SmallClassParentNumber))
                {
                    // 存在しない機種中分類IDが入力されている場合
                    // 入力内容が不正です。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SmallClassParentNumber, null, GetResMessage(new string[] { ComRes.ID.ID141220008 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Small.ControlGroupId));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // アイテムの重複チェック
                if (result.SmallClassName != result.SmallClassNameBefore)
                {
                    int middleId = (int)middleDic[result.SmallClassParentNumber].ParentId;
                    int largeId = (int)largeDic[middleId].ParentId;
                    int jobId = (int)jobDic[largeId].ParentId;
                    long factoryId = (long)factoryDic[jobId];
                    int cnt = 0;
                    TMQUtil.GetCountDb(new
                    {
                        @LocationStructureId = factoryId,
                        @LanguageId = this.LanguageId,
                        @TranslationText = result.SmallClassName,
                        @StructureGroupId = structureGroupId,
                        @FactoryId = factoryId,
                        @StructureLayerNo = (int)TMQConst.MsStructure.StructureLayerNo.Job.SmallClassfication,
                        @ParentStructureId = middleDic[result.SmallClassParentNumber].StructureId
                    }, Master.SqlName.GetCountLayersTranslationByFactory, ref cnt, db, Master.SqlName.ComLayersDir);

                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.SmallClassName, null, GetResMessage(new string[] { ComRes.ID.ID941260001, "111010005" }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString(), string.Empty, TMQUtil.ComExcelPort.MasterColumnInfo.StructureGroup1010.Small.ControlGroupId));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                // 登録処理
                if (!executeExcelPortRegistSmall(result, jobDic, factoryDic, largeDic, middleDic, now))
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
        /// 職種登録処理
        /// </summary>
        /// <param name="factoryDic">工場IDディクショナリ</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegistJob(TMQUtil.CommonExcelPortMasterJobList job, Dictionary<int?, long?> factoryDic, ref Dictionary<int?, ExcelPortStructureList> jobDic, DateTime now)
        {
            // 所属情報が変更されている場合は削除
            bool isNew = false;
            if (job.JobParentNumber != null &&
                job.JobParentNumberBefore != null &&
                job.JobParentNumber != job.JobParentNumberBefore)
            {
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, Master.SqlName.SubDir, new { StructureId = job.JobId, UpdateDatetime = now, UpdateUserId = this.UserId }, db))
                {
                    return false;
                }
                isNew = true;
            }

            // 翻訳マスタ登録処理
            if (!TMQUtil.registTranslationStructureExcelPort(structureGroupId,                                      // 構成グループID
                                                             now,                                                   // 現在日時
                                                             this.LanguageId,                                       // 言語ID
                                                             this.db,                                               // DBクラス
                                                             this.UserId,                                           // ユーザーID
                                                             job.JobName,                                           // 翻訳名
                                                             job.JobNameBefore,                                     // 変更前翻訳名
                                                             job.JobItemTranslationId,                              // 翻訳ID
                                                             (int)factoryDic[job.JobParentNumber],                  // 工場ID
                                                             job.JobId,                                             // 構成ID
                                                             (int)TMQConst.MsStructure.StructureLayerNo.Job.Job,    // 階層番号
                                                             0,                                                     // 親構成ID
                                                             out int transId,                                       // 翻訳ID
                                                             isNew))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!TMQUtil.registItemStructureExcelPort(job.JobId,                  // 構成ID
                                            structureGroupId,                     // 構成グループID
                                            (int)factoryDic[job.JobParentNumber], // 工場ID
                                            job.JobName,                          // 翻訳名
                                            job.JobNameBefore,                    // 変更前翻訳名
                                            job.JobItemTranslationId,             // 翻訳ID
                                            job.JobItemId,                        // アイテムID
                                            transId,                              // 翻訳ID
                                            now,                                  // 現在日時
                                            this.LanguageId,                      // 言語ID
                                            this.db,                              // DBクラス
                                            this.UserId,                          // ユーザーID
                                            out int itemId,                       // アイテムID
                                            isNew))
            {
                return false;
            }

            // アイテムマスタ拡張登録
            if (!TMQUtil.registStructureItemExExcelPort(job.JobCode, 1, itemId, now, this.db, this.UserId))
            {
                return false;
            }

            // 構成マスタ登録
            if (!TMQUtil.registStructureExcelPort(structureGroupId,                                     // 構成グループID
                                                 job.JobId,                                             // 構成ID
                                                 (int)factoryDic[job.JobParentNumber],                  // 工場ID
                                                 itemId,                                                // アイテムID
                                                 0,                                                     // 親構成ID
                                                 (int)TMQConst.MsStructure.StructureLayerNo.Job.Job,    // 階層番号
                                                 now,                                                   // 現在日時
                                                 this.db,                                               // DBクラス
                                                 this.UserId,                                           // ユーザーID
                                                 isNew,
                                                 out int structureId))
            {
                return false;
            }

            // 職種ディクショナリに登録した情報を追加
            jobDic[job.JobNumber].StructureId = structureId;

            return true;
        }

        /// <summary>
        /// 機種大分類登録処理
        /// </summary>
        /// <param name="jobDic">職種IDディクショナリ</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegistLarge(TMQUtil.CommonExcelPortMasterJobList large, Dictionary<int?, ExcelPortStructureList> jobDic, Dictionary<int?, long?> factoryDic, ref Dictionary<int?, ExcelPortStructureList> largeDic, DateTime now)
        {
            bool parentChanged = false;

            if (large.LargeClassId != null)
            {
                // DBより自分自身の親構成IDを取得
                ComDao.MsStructureEntity ms = new ComDao.MsStructureEntity().GetEntity((int)large.LargeClassId, this.db);
                // 自分の親構成IDを比較(異なる場合は自分の親アイテムの所属が変わった)
                parentChanged = ms.ParentStructureId != jobDic[large.LargeClassParentNumber].StructureId;
            }

            // 所属情報が変更されている場合は削除(自分の親アイテムの所属が変わった場合も含む)
            bool isNew = false;
            if ((large.LargeClassParentNumber != null &&
                large.LargeClassParentNumberBefore != null &&
                large.LargeClassParentNumber != large.LargeClassParentNumberBefore) ||
                parentChanged)
            {
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, Master.SqlName.SubDir, new { StructureId = large.LargeClassId, UpdateDatetime = now, UpdateUserId = this.UserId }, db))
                {
                    return false;
                }
                isNew = true;
            }

            int factoryId = (int)factoryDic[(int)jobDic[large.LargeClassParentNumber].ParentId];
            // 翻訳マスタ登録処理
            if (!TMQUtil.registTranslationStructureExcelPort(structureGroupId,                                                  // 構成グループID
                                                             now,                                                               // 現在日時
                                                             this.LanguageId,                                                   // 言語ID
                                                             this.db,                                                           // DBクラス
                                                             this.UserId,                                                       // ユーザーID
                                                             large.LargeClassName,                                              // 翻訳名
                                                             large.LargeClassNameBefore,                                        // 変更前翻訳名
                                                             large.LargeClassItemTranslationId,                                 // 翻訳ID
                                                             factoryId,                                                         // 工場ID
                                                             large.LargeClassId,                                                // 構成ID
                                                             (int)TMQConst.MsStructure.StructureLayerNo.Job.LargeClassfication, // 階層番号
                                                             (int)jobDic[large.LargeClassParentNumber].StructureId,             // 親構成ID
                                                             out int transId,                                                   // 翻訳ID
                                                             isNew))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!TMQUtil.registItemStructureExcelPort(large.LargeClassId,             // 構成ID
                                            structureGroupId,                         // 構成グループID
                                            factoryId,                                // 工場ID
                                            large.LargeClassName,                     // 翻訳名
                                            large.LargeClassNameBefore,               // 変更前翻訳名
                                            large.LargeClassItemTranslationId,        // 翻訳ID
                                            large.LargeClassItemId,                   // アイテムID
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
                                                 large.LargeClassId,                                         // 構成ID
                                                 factoryId,                                                  // 工場ID
                                                 itemId,                                                     // アイテムID
                                                 (int)jobDic[large.LargeClassParentNumber].StructureId,      // 親構成ID
                                                 (int)TMQConst.MsStructure.StructureLayerNo.Job.LargeClassfication, // 階層番号
                                                 now,                                                        // 現在日時
                                                 this.db,                                                    // DBクラス
                                                 this.UserId,                                                // ユーザーID
                                                 isNew,
                                                 out int structureId))
            {
                return false;
            }

            // 機種大分類ディクショナリに登録した情報を追加
            largeDic[large.LargeClassNumber].StructureId = structureId;

            return true;
        }

        /// <summary>
        /// 機種中分類登録処理
        /// </summary>
        /// <param name="jobDic">職種IDディクショナリ</param>
        /// <param name="factoryDic">工場IDディクショナリ</param>
        /// <param name="largeDic">機種大分類IDディクショナリ</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegistMiddle(TMQUtil.CommonExcelPortMasterJobList middle, Dictionary<int?, ExcelPortStructureList> jobDic, Dictionary<int?, long?> factoryDic, Dictionary<int?, ExcelPortStructureList> largeDic, ref Dictionary<int?, ExcelPortStructureList> middleDic, DateTime now)
        {
            bool parentChanged = false;

            if (middle.MiddleClassId != null)
            {
                // DBより自分自身の親構成IDを取得
                ComDao.MsStructureEntity ms = new ComDao.MsStructureEntity().GetEntity((int)middle.MiddleClassId, this.db);
                // 自分の親構成IDを比較(異なる場合は自分の親アイテムの所属が変わった)
                parentChanged = ms.ParentStructureId != largeDic[middle.MiddleClassParentNumber].StructureId;
            }

            // 所属情報が変更されている場合は削除(自分の親アイテムの所属が変わった場合も含む)
            bool isNew = false;
            if ((middle.MiddleClassParentNumber != null &&
                middle.MiddleClassParentNumberBefore != null &&
                middle.MiddleClassParentNumber != middle.MiddleClassParentNumberBefore) ||
                parentChanged)
            {
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, Master.SqlName.SubDir, new { StructureId = middle.MiddleClassId, UpdateDatetime = now, UpdateUserId = this.UserId }, db))
                {
                    return false;
                }
                isNew = true;
            }

            int largeId = (int)largeDic[middle.MiddleClassParentNumber].ParentId;
            int jodId = (int)jobDic[largeId].ParentId;
            int factoryId = (int)factoryDic[jodId];
            // 翻訳マスタ登録処理
            if (!TMQUtil.registTranslationStructureExcelPort(structureGroupId,                                                    // 構成グループID
                                                             now,                                                                 // 現在日時
                                                             this.LanguageId,                                                     // 言語ID
                                                             this.db,                                                             // DBクラス
                                                             this.UserId,                                                         // ユーザーID
                                                             middle.MiddleClassName,                                              // 翻訳名
                                                             middle.MiddleClassNameBefore,                                        // 変更前翻訳名
                                                             middle.MiddleClassItemTranslationId,                                 // 翻訳ID
                                                             factoryId,                                                           // 工場ID
                                                             middle.MiddleClassId,                                                // 構成ID
                                                             (int)TMQConst.MsStructure.StructureLayerNo.Job.MiddleClassfication,  // 階層番号
                                                             (int)largeDic[middle.MiddleClassParentNumber].StructureId,           // 親構成ID
                                                             out int transId,                                 // 翻訳ID
                                                             isNew))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!TMQUtil.registItemStructureExcelPort(middle.MiddleClassId,           // 構成ID
                                            structureGroupId,                         // 構成グループID
                                            factoryId,                                // 工場ID
                                            middle.MiddleClassName,                   // 翻訳名
                                            middle.MiddleClassNameBefore,             // 変更前翻訳名
                                            middle.MiddleClassItemTranslationId,      // 翻訳ID
                                            middle.MiddleClassItemId,                 // アイテムID
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
            if (!TMQUtil.registStructureExcelPort(structureGroupId,                                                  // 構成グループID
                                                 middle.MiddleClassId,                                               // 構成ID
                                                 factoryId,                                                          // 工場ID
                                                 itemId,                                                             // アイテムID
                                                 (int)largeDic[middle.MiddleClassParentNumber].StructureId,          // 親構成ID
                                                 (int)TMQConst.MsStructure.StructureLayerNo.Job.MiddleClassfication, // 階層番号
                                                 now,                                                                // 現在日時
                                                 this.db,                                                            // DBクラス
                                                 this.UserId,                                                        // ユーザーID
                                                 isNew,
                                                 out int structureId))
            {
                return false;
            }

            // 機種分中類ディクショナリに登録した情報を追加
            middleDic[middle.MiddleClassNumber].StructureId = structureId;

            return true;
        }

        /// <summary>
        /// 機種小分類登録処理
        /// </summary>
        /// <param name="jobDic">系列IDディクショナリ</param>
        /// <param name="factoryDic">工場IDディクショナリ</param>
        /// <param name="largeDic">機種大分類IDディクショナリ</param>
        /// <param name="middleDic">機種中分類IDディクショナリ</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegistSmall(TMQUtil.CommonExcelPortMasterJobList small, Dictionary<int?, ExcelPortStructureList> jobDic, Dictionary<int?, long?> factoryDic, Dictionary<int?, ExcelPortStructureList> largeDic, Dictionary<int?, ExcelPortStructureList> middleDic, DateTime now)
        {
            bool parentChanged = false;

            if (small.SmallClassId != null)
            {
                // DBより自分自身の親構成IDを取得
                ComDao.MsStructureEntity ms = new ComDao.MsStructureEntity().GetEntity((int)small.SmallClassId, this.db);
                // 自分の親構成IDを比較(異なる場合は自分の親アイテムの所属が変わった)
                parentChanged = ms.ParentStructureId != middleDic[small.SmallClassParentNumber].StructureId;
            }

            // 所属情報が変更されている場合は削除
            bool isNew = false;
            if ((small.SmallClassParentNumber != null &&
                small.SmallClassParentNumberBefore != null &&
                small.SmallClassParentNumber != small.SmallClassParentNumberBefore) ||
                parentChanged)
            {
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, Master.SqlName.SubDir, new { StructureId = small.SmallClassId, UpdateDatetime = now, UpdateUserId = this.UserId }, db))
                {
                    return false;
                }
                isNew = true;
            }

            int middleId = (int)middleDic[small.SmallClassParentNumber].ParentId;
            int largeId = (int)largeDic[middleId].ParentId;
            int jodId = (int)jobDic[largeId].ParentId;
            int factoryId = (int)factoryDic[jodId];
            // 翻訳マスタ登録処理
            if (!TMQUtil.registTranslationStructureExcelPort(structureGroupId,                                                   // 構成グループID
                                                             now,                                                                // 現在日時
                                                             this.LanguageId,                                                    // 言語ID
                                                             this.db,                                                            // DBクラス
                                                             this.UserId,                                                        // ユーザーID
                                                             small.SmallClassName,                                               // 翻訳名
                                                             small.SmallClassNameBefore,                                         // 変更前翻訳名
                                                             small.SmallClassItemTranslationId,                                  // 翻訳ID
                                                             factoryId,                                                          // 工場ID
                                                             small.SmallClassId,                                                 // 構成ID
                                                             (int)TMQConst.MsStructure.StructureLayerNo.Job.SmallClassfication,  // 階層番号
                                                             (int)middleDic[small.SmallClassParentNumber].StructureId,           // 親構成ID
                                                             out int transId,                                // 翻訳ID
                                                             isNew))
            {
                return false;
            }

            // アイテムマスタ登録
            if (!TMQUtil.registItemStructureExcelPort(small.SmallClassId,            // 構成ID
                                            structureGroupId,                        // 構成グループID
                                            factoryId,                               // 工場ID
                                            small.SmallClassName,                    // 翻訳名
                                            small.SmallClassNameBefore,              // 変更前翻訳名
                                            small.SmallClassItemTranslationId,       // 翻訳ID
                                            small.SmallClassItemId,                  // アイテムID
                                            transId,                                 // 翻訳ID
                                            now,                                     // 現在日時
                                            this.LanguageId,                         // 言語ID
                                            this.db,                                 // DBクラス
                                            this.UserId,                             // ユーザーID
                                            out int itemId,                          // アイテムID
                                            isNew))
            {
                return false;
            }

            // 構成マスタ登録
            if (!TMQUtil.registStructureExcelPort(structureGroupId,                                                 // 構成グループID
                                                 small.SmallClassId,                                                // 構成ID
                                                 factoryId,                                                         // 工場ID
                                                 itemId,                                                            // アイテムID
                                                 (int)middleDic[small.SmallClassParentNumber].StructureId,          // 親構成ID
                                                 (int)TMQConst.MsStructure.StructureLayerNo.Job.SmallClassfication, // 階層番号
                                                 now,                                                               // 現在日時
                                                 this.db,                                                           // DBクラス
                                                 this.UserId,                                                       // ユーザーID
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
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetJobClassficationItemList, out string baseSql, listUnComment);

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
            var results = TMQUtil.SqlExecuteClass.SelectList<Dao.SearchResult>(SqlName.GetJobClassficationItemInfo, SqlName.SubDir, condition, this.db);
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
                    if (!MS1010_CheckDuplicateByItemTran(hiddenInfo, result, ref errFlg))
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
                bool MS1010_CheckDuplicateByItemTran(
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

            // 職種アイテムの場合
            if (hiddenInfo.StructureLayerNo == Master.Structure.StructureLayerNo.Layer0)
            {
                // アイテムマスタ拡張登録
                if (!registItemEx(now, hiddenInfo, itemInfo, itemId))
                {
                    return false;
                }
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