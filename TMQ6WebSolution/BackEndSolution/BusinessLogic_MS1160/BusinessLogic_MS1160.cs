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
using Master = CommonTMQUtil.CommonTMQUtil.ComMaster;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_MS1160
{
    /// <summary>
    /// 適用法規マスタ
    /// </summary>
    public class BusinessLogic_MS1160 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 構成グループID
        /// </summary>
        private static int structureGroupId = 1160;

        /// <summary>
        /// 拡張項目件数
        /// </summary>
        private static int itemExCnt = 0;

        /// <summary>
        /// アイテム一覧タイプ
        /// </summary>
        private static int itemListType = (int)TMQUtil.ComMaster.ItemListType.StandardFactory;

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
            public const int ProccesColumnNo = 8;
            /// <summary>
            /// アイテム翻訳列番号
            /// </summary>
            public const int TranslationTextColumnNo = 11;
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_MS1160() : base()
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
                case Master.ConductInfo.FormList.FormNo:   // 一覧
                    if (!initList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case Master.ConductInfo.FormEdit.FormNo:   // 登録・修正
                    if (!initEdit())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case Master.ConductInfo.FormOrder.FormNo:  // 表示順変更
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
                case Master.ConductInfo.FormList.FormNo:     // 一覧
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
                case Master.ConductInfo.FormEdit.FormNo:
                    // 登録・修正画面の登録処理
                    resultRegist = executeRegistEdit();
                    break;
                case Master.ConductInfo.FormOrder.FormNo:
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
            TMQUtil.GetFixedSqlStatement(Master.SqlName.SubDir, Master.SqlName.UpdateMsStructureInfoAddDeleteFlg, out string sql);
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

                    // SQLを取得
                    TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortMasterList, out string masterSql);

                    // 一覧検索実行
                    IList<TMQUtil.CommonExcelPortMasterList> masterResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterList>(masterSql, searchCondition);
                    if (masterResults == null)
                    {
                        return false;
                    }

                    // Dicitionalyに変換
                    dataList = ComUtil.ConvertClassToDictionary<TMQUtil.CommonExcelPortMasterList>(masterResults);
                    break;

                case Master.MaintainanceTargetExInfo.ExData.UnUse: // 標準アイテム未使用設定

                    // SQLを取得
                    TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortItemUnUseList, out string unuseSql);

                    // 一覧検索実行
                    searchCondition.ProcessId = TMQConst.SendProcessId.Update; // 送信時処理を設定
                    IList<TMQUtil.CommonExcelPortMasterItemUnUseList> unuseResults = db.GetListByDataClass<TMQUtil.CommonExcelPortMasterItemUnUseList>(unuseSql, searchCondition);
                    if (unuseResults == null)
                    {
                        return false;
                    }

                    // Dicitionalyに変換
                    dataList = ComUtil.ConvertClassToDictionary<TMQUtil.CommonExcelPortMasterItemUnUseList>(unuseResults);

                    // 出力対象のシート番号を「標準アイテム未使用」用に変更
                    excelPort.DownloadCondition.HideSheetNo = Master.UnuseSheetNo;
                    break;

                case Master.MaintainanceTargetExInfo.ExData.Oerder: // マスタ並び順設定

                    // SQLを取得
                    TMQUtil.GetFixedSqlStatement(Master.SqlName.ExcelPortDir, Master.SqlName.GetExcelPortItemOrderList, out string ordersSql);

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
            if (sheetNo == Master.UnuseSheetNo)
            {
                // 標準アイテム未使用データの取得
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, TargetCtrlId.ExcelPortUpload,
                    out List<TMQUtil.CommonExcelPortMasterItemUnUseList> resultUnuseList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 標準アイテム未使用データ登録処理
                if (!TMQUtil.registUnuseItemExcelPort(structureGroupId, resultUnuseList, DateTime.Now, this.db, this.UserId))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                return ComConsts.RETURN_RESULT.OK;
            }
            else if (sheetNo == Master.OrdeerSheetNo)
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

            // ExcelPortアップロードデータの取得
            if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, TargetCtrlId.ExcelPortUpload,
                out List<TMQUtil.CommonExcelPortMasterList> resultList, out resultMsg, ref fileType, ref fileName, ref ms))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // エラー情報リスト
            List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();

            // 入力チェック・登録処理
            if (!executeCheckAndRegistExcelPort(ref resultList, ref errorInfoList))
            {
                if (errorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                }

                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 入力チェック&登録処理
        /// </summary>
        /// <param name="resultList">入力情報</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeCheckAndRegistExcelPort(ref List<TMQUtil.CommonExcelPortMasterList> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            DateTime now = DateTime.Now;

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            // 入力されたデータの処理
            foreach (TMQUtil.CommonExcelPortMasterList result in resultList)
            {
                // 送信時処理が設定されていない場合は何もしない
                if (result.ProcessId == null)
                {
                    continue;
                }

                // 工場IDがnullの場合は0にする
                result.FactoryId = result.FactoryId == null ? TMQConst.CommonFactoryId : result.FactoryId;
                result.FactoryIdBefore = result.FactoryIdBefore == null ? TMQConst.CommonFactoryId : result.FactoryIdBefore;

                result.StructureGroupId = structureGroupId;

                // 工場の入力チェック
                if (!TMQUtil.commonFactoryCheckExcelPort(result, out string[] errMsg))
                {
                    // エラー時のメッセージを設定する
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortMasterListInfo.ProccesColumnNo, null, GetResMessage(errMsg), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // 新規登録・更新の場合は入力チェック
                if (result.ProcessId == TMQConst.SendProcessId.Regist || result.ProcessId == TMQConst.SendProcessId.Update)
                {
                    // エラーが存在する場合はTrue
                    if (isErrorExcelPort(result, ref errorInfoList))
                    {
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }
                }

                // 登録処理
                if (!executeExcelPortRegist(result, now))
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

            bool isErrorExcelPort(TMQUtil.CommonExcelPortMasterList registInfo, ref List<ComDao.UploadErrorInfo> errorInfoList)
            {
                int cnt = 0;

                // アイテム翻訳が変更されている場合はチェック
                if (registInfo.TranslationText != registInfo.TranslationTextBefore)
                {
                    // 入力チェックに使用する条件を作成
                    TMQUtil.ItemTranslationForMaster condition = new();
                    condition.LocationStructureId = (int)registInfo.FactoryId;      // 工場ID
                    condition.LanguageId = this.LanguageId;                         // 言語ID
                    condition.TranslationText = registInfo.TranslationText;         // アイテム翻訳
                    condition.StructureGroupId = structureGroupId;                  // 構成グループID

                    // 対象構成グループの工場ID「0」または選択された工場IDで同じアイテム翻訳件数を取得する

                    if (!TMQUtil.GetCountDb(condition, Master.SqlName.GetCountTranslation, ref cnt, db))
                    {
                        return true;
                    }

                    // 既に登録されている場合はエラー
                    if (cnt > 0)
                    {
                        // アイテム翻訳は既に登録されています。
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)registInfo.RowNo, ExcelPortMasterListInfo.TranslationTextColumnNo, GetResMessage(new string[] { ComRes.ID.ID111010005 }), GetResMessage(new string[] { ComRes.ID.ID941260001, ComRes.ID.ID111010005 }), TMQUtil.ComReport.LongitudinalDirection, registInfo.ProcessId.ToString()));
                        return true;
                    }
                    return false;
                }

                return false;
            }
        }

        /// <summary>
        /// ExcelPort 登録処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegist(TMQUtil.CommonExcelPortMasterList result, DateTime now)
        {
            // テーブル共通項目を設定
            result.InsertDatetime = now;
            result.InsertUserId = int.Parse(this.UserId);
            result.UpdateDatetime = now;
            result.UpdateUserId = int.Parse(this.UserId);

            // 送信時処理を判定
            switch (result.ProcessId)
            {
                case TMQConst.SendProcessId.Regist: // 新規登録
                case TMQConst.SendProcessId.Update: // 更新
                                                    // 翻訳マスタ登録
                    if (!TMQUtil.registTranslationExcelPort(result, now, this.LanguageId, this.db, this.UserId, out int transId))
                    {
                        return false;
                    }

                    // アイテムマスタ登録
                    if (!TMQUtil.registItemExcelPort(result, transId, now, this.LanguageId, this.db, this.UserId, out int itemId))
                    {
                        return false;
                    }

                    // アイテムマスタ拡張登録
                    if (!TMQUtil.registItemExExcelPort(result, itemId, 0, now, this.db, this.UserId))
                    {
                        return false;
                    }

                    // 構成マスタ登録
                    if (!TMQUtil.registStructureExcelPort(result, itemId, now, this.db, this.UserId))
                    {
                        return false;
                    }
                    break;
                case TMQConst.SendProcessId.Delete: // 削除
                    if (!TMQUtil.deleteExcelPortData(result, this.db))
                    {
                        return false;
                    }
                    break;
                default:
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
        private TMQUtil.SearchConditionForMaster getCondition(TMQUtil.HiddenInfoForMaster hiddenInfo)
        {
            var result = new TMQUtil.SearchConditionForMaster();

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

            // 工場ID
            if (result.FactoryId == null)
            {
                // 検索条件の工場が未選択の場合、工場共通「0」を設定
                result.FactoryId = Const.CommonFactoryId;
            }
            // 構成グループID
            result.StructureGroupId = hiddenInfo.StructureGroupId;
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

            if (hiddenInfo.ItemListType != (int)Master.ItemListType.Factory)
            {
                // 標準アイテム一覧の取得＆設定

                // ページ情報取得
                var pageInfo = GetPageInfo(Master.ConductInfo.FormList.ControlId.StarndardItemId, this.pageInfoList);

                // 標準アイテム一覧取得
                var results = new List<TMQUtil.SearchResultForMaster>();
                if (!TMQUtil.GetItemListForMaster(condition, Master.SqlName.GetStandardItemList, ref results, this.db))
                {
                    return false;
                }

                if (results != null && results.Count > 0)
                {
                    // 検索結果の設定
                    SetSearchResultsByDataClass<TMQUtil.SearchResultForMaster>(pageInfo, results, results.Count);
                }
            }

            if (hiddenInfo.ItemListType != (int)Master.ItemListType.Standard)
            {
                // 工場アイテム一覧の取得＆設定

                // ページ情報取得
                var pageInfo = GetPageInfo(Master.ConductInfo.FormList.ControlId.FactoryItemId, this.pageInfoList);

                // 工場アイテム一覧取得
                var results = new List<TMQUtil.SearchResultForMaster>();
                if (!TMQUtil.GetItemListForMaster(condition, Master.SqlName.GetFactoryItemList, ref results, this.db))
                {
                    return false;
                }

                if (results != null && results.Count > 0)
                {
                    // 検索結果の設定
                    SetSearchResultsByDataClass<TMQUtil.SearchResultForMaster>(pageInfo, results, results.Count);
                }
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
            // ページ情報
            var pageInfo = GetPageInfo(Master.ConductInfo.FormEdit.ControlId.ItemInfoId, this.pageInfoList);

            // 検索実行
            var results = new List<TMQUtil.SearchResultForMaster>();
            if (!TMQUtil.GetItemInfoForMaster(condition, ref results, this.db))
            {
                return false;
            }

            if (results == null)
            {
                return false;
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<TMQUtil.SearchResultForMaster>(pageInfo, results, 1))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            // アイテムIDの設定

            // ページ情報
            pageInfo = GetPageInfo(Master.ConductInfo.FormEdit.ControlId.ItemId, this.pageInfoList);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<TMQUtil.SearchResultForMaster>(pageInfo, results, 1))
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
            var pageInfo = GetPageInfo(Master.ConductInfo.FormOrder.ControlId.ItemOrderId, this.pageInfoList);

            // 検索実行
            var results = new List<TMQUtil.SearchResultForMaster>();
            if (!TMQUtil.GetItemOrderListForMaster(condition, ref results, this.db))
            {
                return false;
            }

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

            // 工場別アイテム表示順マスタ登録
            if (!TMQUtil.RegistItemOrder(hiddenInfo, registInfoList, this.db))
            {
                return false;
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

            // 工場別未使用標準アイテムマスタの排他チェック
            if (hiddenInfo.TargetItemList == (int)Master.TargetItemList.Standard && hiddenInfo.FactoryId != Const.CommonFactoryId)
            {
                // 工場管理者の標準アイテム修正画面の場合

                // 指定されたコントロールIDの結果情報のみ抽出
                list = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId, true);
                // Daoクラスへセット
                TMQUtil.SearchResultForMaster dispRow = new();
                SetDataClassFromDictionary(list.First(), ctrlId, dispRow);

                // 検索条件
                var condition = new TMQUtil.HiddenInfoForMaster();
                condition.StructureId = dispRow.StructureId;
                condition.FactoryId = hiddenInfo.FactoryId;
                condition.StructureGroupId = hiddenInfo.StructureGroupId;
                condition.LanguageId = hiddenInfo.LanguageId;

                // 排他チェック
                if (isErrorExclusiveByUpdateTime(condition, Master.ConductInfo.FormEdit.ControlId.ItemInfoId))
                {
                    return true;
                }
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

            var newList = new List<TMQUtil.SearchResultForMaster>();

            if (ctrlId == Master.ConductInfo.FormOrder.ControlId.ItemOrderId)
            {
                // 表示順変更の場合

                // 検索実行
                newList = new List<TMQUtil.SearchResultForMaster>();
                if (!TMQUtil.GetItemOrderListForMaster(condition, ref newList, this.db))
                {
                    return false;
                }
            }
            else
            {
                // 未使用フラグ更新の場合

                // 検索実行
                if (!TMQUtil.GetItemInfoForMaster(condition, ref newList, this.db))
                {
                    return false;
                }
            }

            // 先頭行のみ取得
            var newRow = newList.First();

            DateTime? dispDateTime;
            DateTime? newDateTime;

            if (ctrlId == Master.ConductInfo.FormOrder.ControlId.ItemOrderId)
            {
                // 表示順変更の場合

                dispDateTime = dispRow.OrderUpdateDatetime;
                newDateTime = newRow.OrderUpdateDatetime;
            }
            else
            {
                // 未使用フラグ更新の場合

                dispDateTime = dispRow.UnusedUpdateDatetime;
                newDateTime = newRow.UnusedUpdateDatetime;
            }

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
                    if (!TMQUtil.CheckDuplicateByItemTran(hiddenInfo, result, ref errFlg, this.db))
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

            // 工場別未使用標準アイテムマスタ登録
            if (!registItemUnused(now, hiddenInfo, itemInfo))
            {
                return false;
            }

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
        #endregion

    }
}