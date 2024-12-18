using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
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
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MS0020.BusinessLogicDataClass_MS0020;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using Master = CommonTMQUtil.CommonTMQUtil.ComMaster;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
using SpecType = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.SpecType;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;

/// <summary>
/// 機種別仕様マスタメンテ
/// </summary>
namespace BusinessLogic_MS0020
{
    /// <summary>
    /// 機種別仕様マスタメンテ
    /// </summary>
    public partial class BusinessLogic_MS0020 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class Sql
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Master\MachineSpec";
            /// <summary>機種別仕様一覧画面</summary>
            public static class SpecList
            {
                /// <summary>機種別仕様一覧の検索SQL</summary>
                public const string GetList = "GetMachineSpecList";
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = Sql.SubDir + @"\SpecList";
            }
            /// <summary>機種別仕様登録画面</summary>
            public static class SpecRegist
            {
                /// <summary>仕様の言語の一覧と登録された翻訳を取得するSQL</summary>
                public const string GetSpecTranslationList = "GetSpecTranslationList";
                /// <summary>仕様の情報を取得するSQL</summary>
                public const string GetSpecInfos = "GetSpecInfo";

                // 翻訳の更新はマスタ共通を使用

                /// <summary>INSERT：仕様項目マスタ</summary>
                public const string InsertMsSpec = "InsertMsSpec";
                /// <summary>UPDATE：仕様項目マスタ</summary>
                public const string UpdateMsSpec = "UpdateMsSpec";
                /// <summary>INSERT：機種別仕様関連付けマスタ</summary>
                public const string InsertMsMachineSpecRelation = "InsertMsMachineSpecRelation";
                /// <summary>DELETE：機種別仕様関連付けマスタ</summary>
                public const string DeleteMsMachineSpecRelation = "DeleteMsMachineSpecRelation";
                /// <summary>入力チェック：仕様項目マスタで重複した登録が無いかを判定するSQL</summary>
                public const string GetCountDuplicateTranslation = "GetCountDuplicateTranslation";
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = Sql.SubDir + @"\SpecRegist";
            }
            /// <summary>仕様項目選択値一覧画面</summary>
            public static class SpecItemList
            {
                /// <summary>画面情報取得SQL</summary>
                public const string GetData = "GetSpecSelectItemList";

                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = Sql.SubDir + @"\SpecItemList";
            }
            /// <summary>選択肢登録画面</summary>
            public static class SpecItemRegist
            {
                /// <summary>画面情報取得SQL</summary>
                public const string GetData = "GetSpecItemTranslationList";

                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = Sql.SubDir + @"\SpecItemRegist";
            }
            /// <summary>ExcelPort</summary>
            public static class Excelport
            {
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = @"Master\MachineSpec\ExcelPort";
                /// <summary>仕様項目取得SQL</summary>
                public const string GetSpecList = "GetSpecList";
                /// <summary>機種別仕様関連付取得SQL</summary>
                public const string GetSpecRelationList = "GetSpecRelationList";
                /// <summary>仕様項目選択肢取得SQL</summary>
                public const string GetSpecItemList = "GetSpecItemList";
                /// <summary>仕様項目IDより、機種別仕様関連付けデータを取得するSQL</summary>
                public const string GetSpecRelationBySpecId = "GetSpecRelationBySpecId";
                /// <summary>仕様項目IDより、機種別仕様関連付けデータを削除するSQL</summary>
                public const string DeleteSpecRelationBySpecId = "DeleteSpecRelationBySpecId";
                /// <summary>選択されている最下層のアイテムの親配下にあるアイテムを構成マスタより取得するSQL</summary>
                public const string GetRegistedJobDataBySelectedId = "GetRegistedJobDataBySelectedId";
                /// <summary>削除するための入力チェックを行うSQL</summary>
                public const string GetSpecRelationForDeleteCheck = "GetSpecRelationForDeleteCheck";
            }
        }

        /// <summary>
        /// 画面定義の定数クラス
        /// </summary>
        private static class FormInfo
        {
            /// <summary>機種別仕様一覧画面</summary>
            public static class SpecList
            {
                /// <summary>フォームNo</summary>
                public const short No = 1;

                /// <summary>検索条件</summary>
                public const string Condition = "BODY_000_00_LST_0";
                /// <summary>検索結果一覧</summary>
                public const string List = "BODY_020_00_LST_0";
                /// <summary>非表示項目</summary>
                public const string Hide = "BODY_050_00_LST_0";
            }
            /// <summary>機種別仕様登録画面</summary>
            public static class SpecRegist
            {
                /// <summary>フォームNo</summary>
                public const short No = 2;

                /// <summary>ヘッダ</summary>
                public const string Header = "BODY_000_00_LST_1";
                /// <summary>翻訳リスト</summary>
                public const string TranslateList = "BODY_010_00_LST_1";
                /// <summary>仕様内容</summary>
                public const string SpecInfo = "BODY_020_00_LST_1";
                /// <summary>機種別仕様関連付けマスタ(排他チェック用)</summary>
                public const string RelationInfos = "BODY_040_00_LST_1";
            }
            /// <summary>仕様項目選択値一覧画面</summary>
            public static class SpecItemList
            {
                /// <summary>フォームNo</summary>
                public const short No = 3;

                /// <summary>ヘッダ</summary>
                public const string Header = "BODY_000_00_LST_2";
                /// <summary>一覧</summary>
                public const string List = "BODY_010_00_LST_2";
            }
            /// <summary>選択肢登録画面</summary>
            public static class SpecItemRegist
            {
                /// <summary>フォームNo</summary>
                public const short No = 4;

                /// <summary>登録情報</summary>
                public const string Info = "BODY_010_00_LST_3";
                /// <summary>一覧</summary>
                public const string List = "BODY_000_00_LST_3";
            }
        }

        /// <summary>
        /// ExcelPort 一覧画面のコントロールID
        /// </summary>
        private string excelPortFromCtrlId = "BODY_000_00_LST_0";

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            /// <summary>ExcelPortアップロード</summary>
            public const string ExcelPortUpload = "LIST_000_1";
        }

        /// <summary>
        /// ExcelPort仕様項目シート情報
        /// </summary>
        private static class ExcelPortSpecInfo
        {
            // データ開始行
            public const int StartRowNo = 4;

            /// <summary>
            /// 仕様項目シートの定義情報
            /// </summary>
            public class Spec
            {
                // 送信時処理列番号
                public const int ProccesColumnNo = 6;
                // 工場名列番号
                public const int FactoryColumnNo = 8;
                // 仕様項目名列番号
                public const int SpecNameColumnNo = 9;
                // 数値書式列番号
                public const int SpecNumDecimalPlacesColumnNo = 13;
                // 単位種別列番号
                public const int SpecUnitTypeColumnNo = 15;
                // 単位列番号
                public const int SpecUnitColumnNo = 17;
            }

            /// <summary>
            /// 機種別仕様関連付シートの定義情報
            /// </summary>
            public class SpecRelation
            {
                // 送信時処理列番号
                public const int ProccesColumnNo = 4;
            }

            /// <summary>
            /// 仕様項目選択肢シートの定義情報
            /// </summary>
            public class SpecItem
            {
                // 送信時処理列番号
                public const int ProccesColumnNo = 6;
                // 仕様項目名列番号
                public const int SpecItemNameColumnNo = 11;
                /// <summary>構成グループID</summary>
                public const int StructureGroupId = 1340;
            }
        }
        /// <summary>
        /// ExcelPort SQLコメントアウト解除用文字列
        /// </summary>
        private string sqlUnComment = "DeleteFlg";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_MS0020() : base()
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

            switch (this.FormNo)
            {
                case FormInfo.SpecList.No:   // 機種別仕様一覧画面
                    if (compareId.IsBack() || compareId.IsRegist())
                    {
                        // 戻る場合、再検索を行う
                        return InitSearch();
                    }
                    else
                    {
                        if (!initSpecList())
                        {
                            return ComConsts.RETURN_RESULT.NG;
                        }
                        break;
                    }
                    break;
                case FormInfo.SpecRegist.No:   // 機種別仕様登録画面
                    if (!initSpecRegist())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case FormInfo.SpecItemList.No: // 仕様項目選択肢一覧画面
                    if (!initSpecItemList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case FormInfo.SpecItemRegist.No: // 仕様項目選択肢一覧画面
                    if (!initSpecItemRegist())
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
                case FormInfo.SpecList.No:     // 一覧検索
                    if (!searchSpecList())
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
        /// <returns>実行成否：正常なら1以上、異常なら-1</returns>
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
                case FormInfo.SpecRegist.No:
                    // 機種別仕様登録画面の場合の登録処理
                    resultRegist = registSpecRegist();
                    break;
                case FormInfo.SpecItemRegist.No:
                    // 選択肢登録画面の場合の登録処理
                    resultRegist = registSpecItemRegist();
                    break;
                default:
                    // 到達不能
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
            bool resultDelete = false;  // 登録処理戻り値、エラーならFalse

            switch (this.FormNo)
            {
                case FormInfo.SpecItemRegist.No:
                    // 機種別仕様登録
                    resultDelete = true;
                    // 削除処理
                    break;
                case FormInfo.SpecItemList.No:
                    // 仕様項目選択肢一覧
                    resultDelete = true;
                    // 削除処理
                    break;
                default:
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

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
            if (!getDataList(ref excelPort, out IList<Dictionary<string, object>> dataList))
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
        /// 検索処理
        /// </summary>
        /// <param name="excelPort">ExcelPortクラス</param>
        /// <param name="dataList">検索結果</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool getDataList(ref TMQUtil.ComExcelPort excelPort, out IList<Dictionary<string, object>> dataList)
        {
            dataList = null;

            Dao.excelPortSearchCondition condition = new();
            // 親画面(EP0001)の定義情報を追加
            AddMappingListOtherPgmId(TMQUtil.ConductIdEP0001);
            string fromCtrlId = excelPortFromCtrlId;
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, fromCtrlId);
            // 条件画面で選択された値を取得
            SetDataClassFromDictionary(targetDic, fromCtrlId, condition);

            // 翻訳の一時テーブルを作成
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);

            // 翻訳する構成グループのリスト
            var structuregroupList = new List<GroupId>
            {
                GroupId.Location,
                GroupId.SpecSelectItem,
                GroupId.SpecUnitType,
                GroupId.SpecUnit,
                GroupId.SpecType,
                GroupId.SpecNumDecimalPlaces
            };

            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            listPf.RegistTempTable(); // 登録

            // 検索対象シート番号を判定
            int sheetNo = int.Parse(condition.HideSheetNo);
            if (sheetNo == TMQUtil.ComExcelPort.SheetNo.SpecSheetNo)
            {
                // 仕様項目
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(Sql.Excelport.SubDir, Sql.Excelport.GetSpecList, out string sql, new List<string>() { sqlUnComment });

                // 一覧検索実行
                IList<Dao.excelPortSpecList> results = db.GetListByDataClass<Dao.excelPortSpecList>(sql, new { @LanguageId = this.LanguageId });
                if (results == null)
                {
                    return false;
                }

                // Dicitionalyに変換
                dataList = ComUtil.ConvertClassToDictionary<Dao.excelPortSpecList>(results);

                // 出力対象のシート番号を「仕様項目」用に変更
                excelPort.DownloadCondition.HideSheetNo = TMQUtil.ComExcelPort.SheetNo.SpecSheetNo;

            }
            else if (sheetNo == TMQUtil.ComExcelPort.SheetNo.SpecItemSheetNo)
            {
                // 仕様項目選択肢
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(Sql.Excelport.SubDir, Sql.Excelport.GetSpecItemList, out string sql, new List<string>() { sqlUnComment });

                // 一覧検索実行
                IList<Dao.excelPortSpecItemList> results = db.GetListByDataClass<Dao.excelPortSpecItemList>(sql, new { @LanguageId = this.LanguageId });
                if (results == null)
                {
                    return false;
                }

                // Dicitionalyに変換
                dataList = ComUtil.ConvertClassToDictionary<Dao.excelPortSpecItemList>(results);

                // 出力対象のシート番号を「仕様項目」用に変更
                excelPort.DownloadCondition.HideSheetNo = TMQUtil.ComExcelPort.SheetNo.SpecItemSheetNo;
            }
            else if (sheetNo == TMQUtil.ComExcelPort.SheetNo.SpecRelarionSheetNo)
            {
                // 機種別仕様関連付
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(Sql.Excelport.SubDir, Sql.Excelport.GetSpecRelationList, out string sql);

                // 一覧検索実行
                IList<Dao.excelPortSpecRelationList> results = db.GetListByDataClass<Dao.excelPortSpecRelationList>(sql, new { @LanguageId = this.LanguageId });
                if (results == null)
                {
                    return false;
                }

                // 職種機種階層IDから上位の階層を設定
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.excelPortSpecRelationList>(ref results, new List<StructureType> { StructureType.Job }, this.db, this.LanguageId);

                // Dicitionalyに変換
                dataList = ComUtil.ConvertClassToDictionary<Dao.excelPortSpecRelationList>(results);

                // 出力対象のシート番号を「機種別仕様関連付」用に変更
                excelPort.DownloadCondition.HideSheetNo = TMQUtil.ComExcelPort.SheetNo.SpecRelarionSheetNo;
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

            // エラー情報リスト
            List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();

            DateTime now = DateTime.Now;

            // シート番号を判定
            int sheetNo = int.Parse(this.IndividualDictionary["TargetSheetNo"].ToString());
            if (sheetNo == TMQUtil.ComExcelPort.SheetNo.SpecSheetNo)
            {
                // 仕様項目アップロードデータの取得
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, TargetCtrlId.ExcelPortUpload,
                    out List<Dao.excelPortSpecList> resultSpecList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 仕様項目 登録処理
                if (!registAndCheckSpec(resultSpecList, ref errorInfoList, now))
                {
                    if (errorInfoList.Count > 0)
                    {
                        // エラー情報シートへ設定
                        excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    }
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else if (sheetNo == TMQUtil.ComExcelPort.SheetNo.SpecItemSheetNo)
            {
                // 仕様項目選択肢アップロードデータの取得
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, TargetCtrlId.ExcelPortUpload,
                    out List<Dao.excelPortSpecItemList> resultSpecItemList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 仕様項目選択肢 登録処理
                if (!registAndCheckSpecItem(resultSpecItemList, ref errorInfoList, now))
                {
                    if (errorInfoList.Count > 0)
                    {
                        // エラー情報シートへ設定
                        excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    }
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }
                //★インメモリ化対応 start
                // 更新対象構成グループIDを設定
                SetGlobalData("TargetGrpId", ExcelPortSpecInfo.SpecItem.StructureGroupId);
                //★インメモリ化対応 end
            }
            else if (sheetNo == TMQUtil.ComExcelPort.SheetNo.SpecRelarionSheetNo)
            {
                // 機種別仕様関連付けアップロードデータの取得
                if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, TargetCtrlId.ExcelPortUpload,
                    out List<Dao.excelPortSpecRelationList> resultSpecRelationList, out resultMsg, ref fileType, ref fileName, ref ms))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }

                // 機種別仕様関連付け 登録処理
                if (!registSpecRelation(resultSpecRelationList, ref errorInfoList, now))
                {
                    if (errorInfoList.Count > 0)
                    {
                        // エラー情報シートへ設定
                        excelPort.SetErrorInfoSheet(file, errorInfoList, ref fileType, ref fileName, ref ms);
                    }
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return ComConsts.RETURN_RESULT.NG;
                }
            }
            else
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        #region 仕様項目
        /// <summary>
        /// 仕様項目 入力チェック&登録処理
        /// </summary>
        /// <param name="resultSpecList">仕様項目情報リスト</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registAndCheckSpec(List<Dao.excelPortSpecList> resultSpecList, ref List<ComDao.UploadErrorInfo> errorInfoList, DateTime now)
        {
            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            // 翻訳の一時テーブルを作成
            registTempTable();
            // 変更前のデータを取得(削除アイテム含む)
            TMQUtil.GetFixedSqlStatement(Sql.Excelport.SubDir, Sql.Excelport.GetSpecList, out string sql);
            IList<Dao.excelPortSpecList> beforeResultList = db.GetListByDataClass<Dao.excelPortSpecList>(sql, new { @LanguageId = this.LanguageId });

            foreach (Dao.excelPortSpecList result in resultSpecList)
            {
                // 送信時処理が設定されていない場合は何もしない
                if (result.ProcessId == null)
                {
                    continue;
                }
                rowErrFlg = false;

                // 非表示列の変更前の情報を再取得
                setSpecBeforeData(result, beforeResultList);

                // 工場IDがnullの場合は0にする
                result.FactoryId = result.FactoryId == null ? TMQConsts.CommonFactoryId : result.FactoryId;
                result.FactoryIdBefore = result.FactoryIdBefore == null ? TMQConsts.CommonFactoryId : result.FactoryIdBefore;

                // アイテム翻訳重複チェック
                if (!checkTranslationTextExcelPort(true, result.SpecName, result.SpecNameBefore, (long)result.FactoryId, result.SpecId))
                {
                    // 「アイテム翻訳は既に登録されています。」
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.Spec.SpecNameColumnNo, GetResMessage(new string[] { ComRes.ID.ID111100039 }), GetResMessage(new string[] { ComRes.ID.ID941260001, ComRes.ID.ID111010005 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // 工場アイテムが別の工場アイテムに変更されている場合
                if (result.ProcessId == TMQConsts.SendProcessId.Update && result.FactoryIdBefore != result.FactoryId)
                {
                    // 別工場のアイテムに変更することはできません。
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.Spec.ProccesColumnNo, null, GetResMessage(new string[] { ComRes.ID.ID141290008 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // 入力形式が「数値」または「数値(範囲)」の場合、数値書式・単位種別が選択されていない場合はエラー
                // 拡張情報を取得
                var structure = new ComDao.MsStructureEntity().GetEntity(result.SpecTypeId ?? -1, this.db);
                int itemId = structure.StructureItemId ?? -1;
                var itemEx = new ComDao.MsItemExtensionEntity().GetEntity(itemId, 1, this.db);

                // 取得した拡張項目を判定
                if (itemEx.ExtensionData == ((int)SpecType.Number).ToString() ||
                   itemEx.ExtensionData == ((int)SpecType.NumberRange).ToString())
                {
                    // 数値書式が未入力の場合はエラー
                    if (result.SpecNumDecimalPlacesId == null)
                    {
                        // 「必須項目です。入力してください。」
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.Spec.SpecNumDecimalPlacesColumnNo, GetResMessage(new string[] { ComRes.ID.ID111130015 }), GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }

                    // 単位種別が未入力の場合はエラー
                    if (result.SpecUnitTypeId == null)
                    {
                        // 「必須項目です。入力してください。」
                        errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.Spec.SpecUnitTypeColumnNo, GetResMessage(new string[] { ComRes.ID.ID111130015 }), GetResMessage(new string[] { ComRes.ID.ID941270001 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                        errFlg = true;
                        rowErrFlg = true;
                        continue;
                    }

                    // 単位が選択されている場合
                    if (result.SpecUnitId != null)
                    {
                        // 単位種別情報を取得
                        var specUnitType = new ComDao.MsStructureEntity().GetEntity((int)result.SpecUnitTypeId, this.db);
                        // 単位情報を取得
                        var specUnit = new ComDao.MsStructureEntity().GetEntity((int)result.SpecUnitId, this.db);
                        var specUnitEx = new ComDao.MsItemExtensionEntity().GetEntity((int)specUnit.StructureItemId, 1, this.db);

                        // 選択された単位が、単位種別に紐付くものでない場合はエラー
                        if ((int)specUnitType.StructureItemId != int.Parse(specUnitEx.ExtensionData))
                        {
                            // 「単位種別に紐づく単位を選択してください。」
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.Spec.SpecUnitColumnNo, GetResMessage(new string[] { ComRes.ID.ID111160029 }), GetResMessage(new string[] { ComRes.ID.ID141160020 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = true;
                            rowErrFlg = true;
                            continue;
                        }
                    }
                }

                // 翻訳マスタ登録
                if (!registTranslationExcelPort(result.ProcessId == TMQConsts.SendProcessId.Regist, result.SpecName, result.SpecNameBefore, result.TranslationId, (int)result.FactoryId, now, out int newTranslationId))
                {
                    return false;
                }

                // 仕様項目登録
                if (!registSpec(result, newTranslationId, now))
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
        /// 仕様項目登録処理
        /// </summary>
        /// <param name="specInfo">仕様項目情報</param>
        /// <param name="translationId">翻訳ID</param>
        /// <param name="now">現在日時</param>
        private bool registSpec(Dao.excelPortSpecList specInfo, int translationId, DateTime now)
        {
            // 仕様項目テーブルの登録情報を作成
            ComDao.MsSpecEntity condition = new();
            condition.SpecId = specInfo.SpecId;         // 仕様項目ID
            condition.TranslationId = translationId;    // 翻訳ID
            condition.SpecTypeId = specInfo.SpecTypeId; // 仕様項目入力型式

            // 入力形式が数値項目か判定
            if (isNotNumber())
            {
                // 数値でないとき、以下はNULLで登録
                condition.SpecUnitTypeId = null;       // 単位種別
                condition.SpecUnitId = null;           // 単位
                condition.SpecNumDecimalPlaces = null; // 小数点以下桁数
            }
            else
            {
                condition.SpecUnitTypeId = specInfo.SpecUnitTypeId; // 単位種別
                condition.SpecUnitId = specInfo.SpecUnitId;         // 単位
                // 数値の場合、数値書式コンボの値から　拡張項目の値を取得して、DB登録内容に設定
                var structure = new ComDao.MsStructureEntity().GetEntity(Convert.ToInt32(specInfo.SpecNumDecimalPlacesId), this.db);
                int itemId = structure.StructureItemId ?? -1;
                var itemEx = new ComDao.MsItemExtensionEntity().GetEntity(itemId, 1, this.db);
                condition.SpecNumDecimalPlaces = int.Parse(itemEx.ExtensionData);
            }

            // テーブル共通項目を設定
            setExecuteConditionByDataClassCommon(ref condition, now, int.Parse(this.UserId), int.Parse(this.UserId));

            // 新規・更新・削除の判定
            if (specInfo.ProcessId == TMQConsts.SendProcessId.Regist)
            {
                // 新規の場合、採番したキーの値を取得
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out int specId, Sql.SpecRegist.InsertMsSpec, Sql.SpecRegist.SubDir, condition, this.db))
                {
                    return false;
                }

                // 機種別仕様関連付けマスタに空のデータを登録する(ms_machine_specrelation)
                ComDao.MsMachineSpecRelationEntity emptyData = new();
                emptyData.LocationStructureId = specInfo.FactoryId; // 工場ID
                emptyData.SpecId = specId;                          // 仕様項目ID
                // テーブル共通項目を設定
                setExecuteConditionByDataClassCommon(ref emptyData, now, int.Parse(this.UserId), int.Parse(this.UserId));
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out int relationId, Sql.SpecRegist.InsertMsMachineSpecRelation, Sql.SpecRegist.SubDir, emptyData, this.db))
                {
                    return false;
                }
            }
            else if (specInfo.ProcessId == TMQConsts.SendProcessId.Update ||
                specInfo.ProcessId == TMQConsts.SendProcessId.Delete)
            {
                // 削除の場合は削除フラグ = True
                if (specInfo.ProcessId == TMQConsts.SendProcessId.Delete)
                {
                    condition.DeleteFlg = true;
                }

                // 仕様項目テーブルの更新
                if (!TMQUtil.SqlExecuteClass.Regist(Sql.SpecRegist.UpdateMsSpec, Sql.SpecRegist.SubDir, condition, this.db))
                {
                    return false;
                }
            }

            return true;

            // 仕様項目の値が数値項目かどうか判定
            bool isNotNumber()
            {
                // 仕様項目の拡張情報を取得
                var structure = new ComDao.MsStructureEntity().GetEntity(specInfo.SpecTypeId ?? -1, this.db);
                int itemId = structure.StructureItemId ?? -1;
                var itemEx = new ComDao.MsItemExtensionEntity().GetEntity(itemId, 1, this.db);

                // 取得した拡張項目を判定
                if (itemEx.ExtensionData == ((int)SpecType.Number).ToString() ||
                   itemEx.ExtensionData == ((int)SpecType.NumberRange).ToString())
                {
                    // 「数値」または「数値(範囲)」の場合
                    return false;
                }
                else
                {
                    // 「テキスト」または「選択」の場合
                    return true;
                }
            }
        }

        /// <summary>
        /// ExcelPort 非表示列に保持している変更前の値をDBから取得
        /// </summary>
        /// <param name="result">Excelから取得したデータ</param>
        /// <param name="beforeResultList">ダウンロード検索SQLの実行結果</param>
        private void setSpecBeforeData(Dao.excelPortSpecList result, IList<Dao.excelPortSpecList> beforeResultList)
        {
            if (beforeResultList == null || beforeResultList.Count == 0)
            {
                result.SpecNameBefore = null;
                result.FactoryIdBefore = null;
                return;
            }
            // 対象データの仕様項目ID
            int specId = result.SpecId;
            // 変更前の情報
            Dao.excelPortSpecList beforeResult = beforeResultList.Where(x => x.SpecId == specId).FirstOrDefault();
            if (beforeResult != null)
            {
                // 非表示列に保持している変更前の値は、DBから取得した値を正とする
                result.SpecNameBefore = beforeResult.SpecNameBefore;
                result.FactoryIdBefore = beforeResult.FactoryIdBefore;
                result.TranslationId = beforeResult.TranslationId;
            }
            else
            {
                result.SpecNameBefore = null;
                result.FactoryIdBefore = null;
            }
        }
        #endregion

        #region 機種別仕様関連付け
        /// <summary>
        /// 機種別仕様関連付け登録処理
        /// </summary>
        /// <param name="resultSpecRelationList">機種別仕様関連付け情報リスト</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse/</returns>
        private bool registSpecRelation(List<Dao.excelPortSpecRelationList> resultSpecRelationList, ref List<ComDao.UploadErrorInfo> errorInfoList, DateTime now)
        {
            foreach (Dao.excelPortSpecRelationList result in resultSpecRelationList)
            {
                // 送信時処理が設定されていない場合は何もしない
                if (result.ProcessId == null)
                {
                    continue;
                }

                // 変更前の仕様項目IDを取得
                ComDao.MsMachineSpecRelationEntity beforeSpecRelation = new ComDao.MsMachineSpecRelationEntity().GetEntity(result.MachineSpecRelationId, this.db);
                if (beforeSpecRelation != null && beforeSpecRelation.SpecId != null)
                {
                    result.SpecIdBefore = beforeSpecRelation.SpecId ?? 0;
                }

                // 仕様項目に紐付く機種別仕様関連付データを取得
                IList<Dao.excelPortSpecRelationList> relationData = getRelationData(result.SpecId);

                // 送信時処理を判定
                switch (result.ProcessId)
                {
                    case TMQConsts.SendProcessId.Regist: // 登録
                    case TMQConsts.SendProcessId.Update: // 更新
                        if (!registRelation(ref relationData, result, ref errorInfoList, now))
                        {
                            return false;
                        }
                        break;
                    case TMQConsts.SendProcessId.Delete: // 削除
                        if (!deleteRelation(result, ref errorInfoList))
                        {
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// 機種別仕様関連付け 登録・更新処理
        /// </summary>
        /// <param name="relationData">仕様項目に紐付く機種別仕様関連付けデータ</param>
        /// <param name="result">機種別仕様関連付け情報</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registRelation(ref IList<Dao.excelPortSpecRelationList> relationData, Dao.excelPortSpecRelationList result, ref List<ComDao.UploadErrorInfo> errorInfoList, DateTime now)
        {
            // 選択されている職種階層のうち、最下層の構成IDを取得
            int lowestId = getJobStructureId(result, out int layerNo);

            // 更新の場合は対象のレコードを更新ではなく登録扱いとするため、登録対象リストから削除
            if (result.ProcessId == TMQConsts.SendProcessId.Update)
            {
                relationData = relationData.Where(x => x.MachineSpecRelationId != result.MachineSpecRelationId).ToList();
            }

            // 入力チェック
            if (!checkIsMustRegist(ref relationData, lowestId, layerNo, result, ref errorInfoList))
            {
                return false;
            }

            // 既に登録されているデータより職種IDを取得しリストとする
            var tmpRelationData = relationData.Select(x => x.JobStructureId).ToList();

            // 選択されているアイテムの親配下のアイテムを取得
            IList<ComDao.MsStructureEntity> registedItemOfParentData = getRegistedData();

            // 親配下のアイテムのうち、既に機種別仕様関連付けマスタに登録されている(今回選択されたExcelのレコード含む)レコード数を取得
            var registedIdList = registedItemOfParentData.Where(x => x.StructureId == lowestId || tmpRelationData.Contains(x.StructureId))
                                          .Select(x => x.StructureId)
                                          .ToList();

            // 親配下のアイテムが全て登録されている場合、これから登録するデータの職種階層IDは
            // 選択されているアイテムの構成IDではなく親構成IDにする
            if (registedIdList.Count == registedItemOfParentData.Count)
            {
                // 選択されているアイテムの親配下のアイテムを登録対象から除外する
                relationData = relationData.Where(x => !registedIdList.Contains((int)x.JobStructureId)).ToList();

                // 今回登録するデータの階層の値をNULLにする
                setNullData(layerNo);
            }

            // 今回登録するレコードを登録対象に追加
            Dao.excelPortSpecRelationList newData = new();
            newData.LocationStructureId = result.FactoryId;                      // 工場ID
            newData.JobStructureId = getJobStructureId(result, out int unuseNo); // 職種階層ID
            newData.SpecId = result.SpecId;                                      // 仕様項目ID
            relationData.Add(newData);

            // 仕様項目に紐付く機種別仕様関連付データを削除
            TMQUtil.SqlExecuteClass.Regist(Sql.Excelport.DeleteSpecRelationBySpecId, Sql.Excelport.SubDir, new { @SpecId = result.SpecId }, this.db);

            // 職種レコード分登録を行う
            foreach (Dao.excelPortSpecRelationList data in relationData)
            {
                ComDao.MsMachineSpecRelationEntity registInfo = new();
                registInfo.LocationStructureId = data.LocationStructureId; // 工場ID
                registInfo.JobStructureId = data.JobStructureId;           // 職種階層ID
                registInfo.SpecId = data.SpecId;                           // 仕様項目ID
                registInfo.DisplayOrder = result.DisplayOrder;             // 並び順

                // テーブル共通項目を設定
                registInfo.InsertDatetime = now;                           // 登録日時
                registInfo.InsertUserId = int.Parse(this.UserId);          // 登録ユーザー
                registInfo.UpdateDatetime = now;                           // 更新日時
                registInfo.UpdateUserId = int.Parse(this.UserId);          // 更新ユーザー

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out int relationId, Sql.SpecRegist.InsertMsMachineSpecRelation, Sql.SpecRegist.SubDir, registInfo, this.db))
                {
                    return false;
                }
            }

            return true;

            // 指定された階層の値をNUllにする
            void setNullData(int targetLayerId)
            {
                if (targetLayerId == (int)TMQConsts.MsStructure.StructureLayerNo.Job.LargeClassfication)
                {
                    // 機種大分類
                    result.LargeClassficationId = null;
                }
                else if (targetLayerId == (int)TMQConsts.MsStructure.StructureLayerNo.Job.MiddleClassfication)
                {
                    // 機種中分類
                    result.MiddleClassficationId = null;
                }
                else if (targetLayerId == (int)TMQConsts.MsStructure.StructureLayerNo.Job.SmallClassfication)
                {
                    // 機種小分類
                    result.SmallClassficationId = null;
                }
            }

            // 選択されているアイテムの親配下のアイテムを取得
            IList<ComDao.MsStructureEntity> getRegistedData()
            {
                // SQL取得
                TMQUtil.GetFixedSqlStatement(Sql.Excelport.SubDir, Sql.Excelport.GetRegistedJobDataBySelectedId, out string sql);

                // SQL実行
                IList<ComDao.MsStructureEntity> registedData = db.GetListByDataClass<ComDao.MsStructureEntity>(sql, new { @StructureId = lowestId });

                return registedData;
            }
        }

        /// <summary>
        /// 機種別仕様関連付け 削除処理
        /// </summary>
        /// <param name="result">機種別仕様関連付け情報</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool deleteRelation(Dao.excelPortSpecRelationList result, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(Sql.Excelport.SubDir, Sql.Excelport.GetSpecRelationForDeleteCheck, out string cntSql);

            // 仕様項目に紐付く機種別仕様関連付けデータのうち、自分自身以外のデータ件数を取得する
            int cnt = db.GetCount(cntSql, new { @SpecId = result.SpecIdBefore, @MachineSpecRelationId = result.MachineSpecRelationId });

            if (cnt <= 0)
            {
                // 「該当の仕様項目の関連付けを全て削除することはできません。」
                errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.SpecRelation.ProccesColumnNo, null, GetResMessage(new string[] { ComRes.ID.ID141060009 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                return false;
            }

            // 機種別仕様関連付けデータの削除
            if (!new ComDao.MsMachineSpecRelationEntity().DeleteByPrimaryKey(result.MachineSpecRelationId, this.db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 仕様項目に紐付く機種別仕様関連付データを取得
        /// </summary>
        /// <param name="specId">仕様項目ID</param>
        /// <returns>仕様項目に紐付く機種別仕様関連付けデータ</returns>
        private IList<Dao.excelPortSpecRelationList> getRelationData(int specId)
        {
            // SQL取得
            TMQUtil.GetFixedSqlStatement(Sql.Excelport.SubDir, Sql.Excelport.GetSpecRelationBySpecId, out string sql);

            // SQL実行
            IList<Dao.excelPortSpecRelationList> relationData = db.GetListByDataClass<Dao.excelPortSpecRelationList>(sql, new { @SpecId = specId });

            // // 職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.excelPortSpecRelationList>(ref relationData, new List<StructureType> { StructureType.Job }, this.db, this.LanguageId);

            return relationData;
        }

        /// <summary>
        /// 選択されている職種階層のうち、最下層の構成IDを取得する
        /// </summary>
        /// <param name="result">機種別仕様関連付け情報</param>
        /// <returns>職種階層の構成ID</returns>
        private int getJobStructureId(Dao.excelPortSpecRelationList result, out int layerNo)
        {
            layerNo = -1;

            // 機種小分類が選択されている場合
            if (result.SmallClassficationId != null)
            {
                layerNo = (int)TMQConsts.MsStructure.StructureLayerNo.Job.SmallClassfication;

                // 機種小分類の構成IDを返す
                return (int)result.SmallClassficationId;
            }
            // 機種中分類が選択されている場合
            else if (result.MiddleClassficationId != null)
            {
                layerNo = (int)TMQConsts.MsStructure.StructureLayerNo.Job.MiddleClassfication;

                // 機種中分類の構成IDを返す
                return (int)result.MiddleClassficationId;
            }
            // 機種大分類が選択されている場合
            else if (result.LargeClassficationId != null)
            {
                layerNo = (int)TMQConsts.MsStructure.StructureLayerNo.Job.LargeClassfication;

                // 機種大分類の構成IDを返す
                return (int)result.LargeClassficationId;
            }
            // 職種が選択されている場合
            else if (result.JobId != null)
            {
                layerNo = (int)TMQConsts.MsStructure.StructureLayerNo.Job.Job;

                // 職種の構成IDを返す
                return (int)result.JobId;
            }

            // 職種階層は選択されているのであり得ない
            return -1;
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="relationData">仕様項目に紐付く機種別仕様関連付けデータ</param>
        /// <param name="lowestId">今回登録する最下層の値</param>
        /// <param name="layerNo">今回登録する最下層の階層番号</param>
        /// <param name="result">今回登録するExcelで入力されたレコード</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool checkIsMustRegist(ref IList<Dao.excelPortSpecRelationList> relationData, int lowestId, int layerNo, Dao.excelPortSpecRelationList result, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            // 更新の場合かつ、仕様項目が変更されている場合は削除チェックを行う
            if (result.ProcessId == TMQConsts.SendProcessId.Update && result.SpecId != result.SpecIdBefore)
            {
                if (!deleteRelation(result, ref errorInfoList))
                {
                    return false;
                }
            }

            // 件数代入用
            int cnt = 0;

            // 既に登録されているデータの中に、取得した最下層の値が存在するか判定
            if (relationData.Where(x => x.JobStructureId == lowestId).ToList().Count > 0)
            {
                // 「既に登録されている職種の組み合わせです。」
                errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.SpecRelation.ProccesColumnNo, null, GetResMessage(new string[] { ComRes.ID.ID141130003 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                return false;
            }

            // 職種
            if (layerNo == (int)TMQConsts.MsStructure.StructureLayerNo.Job.Job)
            {
                // 自分の下位階層(職種が同じで、機種大分類の値が設定されている)のデータ取得
                var registedIdList = relationData.Where(x => x.JobId == lowestId && x.LargeClassficationId != null).Select(x => x.MachineSpecRelationId).ToList();
                if (registedIdList.Count > 0)
                {
                    // 該当の機種別仕様関連付IDのデータを登録対象から除く
                    relationData = relationData.Where(x => !registedIdList.Contains(x.MachineSpecRelationId)).ToList();
                }
            }

            // 機種大分類
            if (layerNo == (int)TMQConsts.MsStructure.StructureLayerNo.Job.LargeClassfication)
            {
                // 自分の上位階層(職種)の値が既に登録されている場合
                cnt = relationData.Where(x => x.JobStructureId == result.JobId).ToList().Count;
                if (cnt > 0)
                {
                    // 「既に登録されている職種の組み合わせです。」
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.SpecRelation.ProccesColumnNo, null, GetResMessage(new string[] { ComRes.ID.ID141130003 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                    return false;
                }

                // 自分の下位階層(機種大分類が同じで、機種中分類の値が設定されている)のデータ取得
                var registedIdList = relationData.Where(x => x.LargeClassficationId == lowestId && x.MiddleClassficationId != null).Select(x => x.MachineSpecRelationId).ToList();
                if (registedIdList.Count > 0)
                {
                    // 該当の機種別仕様関連付IDのデータを登録対象から除く
                    relationData = relationData.Where(x => !registedIdList.Contains(x.MachineSpecRelationId)).ToList();
                }
            }

            // 機種中分類
            if (layerNo == (int)TMQConsts.MsStructure.StructureLayerNo.Job.MiddleClassfication)
            {
                // 自分の上位階層(職種・機種大分類)の値が既に登録されている場合
                cnt = relationData.Where(x => x.JobStructureId == result.JobId || x.JobStructureId == result.LargeClassficationId).ToList().Count;
                if (cnt > 0)
                {
                    // 「既に登録されている職種の組み合わせです。」
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.SpecRelation.ProccesColumnNo, null, GetResMessage(new string[] { ComRes.ID.ID141130003 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                    return false;
                }

                // 自分の下位階層(機種中分類が同じで、機種小分類の値が設定されている)のデータ取得
                var registedIdList = relationData.Where(x => x.MiddleClassficationId == lowestId && x.SmallClassficationId != null).Select(x => x.MachineSpecRelationId).ToList();
                if (registedIdList.Count > 0)
                {
                    // 該当の機種別仕様関連付IDのデータを登録対象から除く
                    relationData = relationData.Where(x => !registedIdList.Contains(x.MachineSpecRelationId)).ToList();
                }
            }

            // 機種小分類
            if (layerNo == (int)TMQConsts.MsStructure.StructureLayerNo.Job.SmallClassfication)
            {
                // 自分の上位階層(職種・機種大分類・機種中分類)の値が既に登録されている場合
                cnt = relationData.Where(x => x.JobStructureId == result.JobId || x.JobStructureId == result.LargeClassficationId || x.JobStructureId == result.MiddleClassficationId).ToList().Count;
                if (cnt > 0)
                {
                    // 「既に登録されている職種の組み合わせです。」
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.SpecRelation.ProccesColumnNo, null, GetResMessage(new string[] { ComRes.ID.ID141130003 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                    return false;
                }
            }

            return true;

            // 機種別仕様関連付けデータを削除
            bool deleteRelationData(IList<int> idlist)
            {
                foreach (int machineSpecRelationId in idlist)
                {
                    if (!new ComDao.MsMachineSpecRelationEntity().DeleteByPrimaryKey(machineSpecRelationId, this.db))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        #endregion

        #region 仕様項目選択肢
        /// <summary>
        /// 仕様項目選択肢 入力チェック&登録処理
        /// </summary>
        /// <param name="resultSpecList">仕様項目選択肢情報リスト</param>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registAndCheckSpecItem(List<Dao.excelPortSpecItemList> resultSpecList, ref List<ComDao.UploadErrorInfo> errorInfoList, DateTime now)
        {
            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            // 翻訳の一時テーブルを作成
            registTempTable();
            // 変更前のデータを取得(削除アイテム含む)
            TMQUtil.GetFixedSqlStatement(Sql.Excelport.SubDir, Sql.Excelport.GetSpecItemList, out string sql);
            IList<Dao.excelPortSpecItemList> beforeResultList = db.GetListByDataClass<Dao.excelPortSpecItemList>(sql, new { @LanguageId = this.LanguageId });

            foreach (Dao.excelPortSpecItemList result in resultSpecList)
            {
                // 送信時処理が設定されていない場合は何もしない
                if (result.ProcessId == null)
                {
                    continue;
                }
                rowErrFlg = false;

                // 非表示列の変更前の情報を再取得
                setSpecItemBeforeData(result, beforeResultList);

                // アイテム翻訳重複チェック
                if (!checkTranslationTextExcelPort(false, result.TranslationText, result.TranslationTextBefore, (long)result.FactoryId, result.SpecId))
                {
                    // 「アイテム翻訳は既に登録されています。」
                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ExcelPortSpecInfo.SpecItem.SpecItemNameColumnNo, GetResMessage(new string[] { ComRes.ID.ID111010005 }), GetResMessage(new string[] { ComRes.ID.ID941260001, ComRes.ID.ID111010005 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                    errFlg = true;
                    rowErrFlg = true;
                    continue;
                }

                // 翻訳マスタ登録
                if (!registTranslationExcelPort(result.ProcessId == TMQConsts.SendProcessId.Regist, result.TranslationText, result.TranslationTextBefore, result.TranslationId, (int)result.FactoryId, now, out int newTranslationId))
                {
                    return false;
                }

                // アイテムマスタ登録
                if (!registItemExcelPort(result, newTranslationId, now, out int itemId))
                {
                    return false;
                }

                // アイテムマスタ拡張登録
                if (!registItemExtensionExcelPort(result, itemId, now))
                {
                    return false;
                }

                // 構成マスタ登録
                if (!registStructureExcelPort(result, itemId, now, out int structureId))
                {
                    return false;
                }

                // 工場別アイテム表示順マスタ登録
                if (!registOrderExcelPort(structureId, result, now))
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
        /// アイテムマスタ登録処理
        /// </summary>
        /// <param name="result">仕様項目選択肢情報</param>
        /// <param name="translationId">翻訳ID</param>
        /// <param name="now">現在日時</param>
        /// <param name="itemId">アイテムID</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registItemExcelPort(Dao.excelPortSpecItemList result, int translationId, DateTime now, out int itemId)
        {
            itemId = -1;
            // 登録情報を作成
            ComDao.MsItemEntity registInfo = new();
            // テーブル共通項目を設定
            setExecuteConditionByDataClassCommon(ref registInfo, now, int.Parse(this.UserId), int.Parse(this.UserId));

            registInfo.ItemTranslationId = translationId;                                    // 翻訳ID
            registInfo.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.SpecSelectItem; // 構成グループiD

            // 新規か更新・削除か判定
            if (result.ProcessId == TMQConsts.SendProcessId.Regist)
            {
                // 新規の場合、採番したキーの値を取得
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out itemId, Master.SqlName.InsertMsItemInfo, Master.SqlName.SubDir, registInfo, this.db))
                {
                    return false;
                }
            }
            else
            {
                // 更新の場合、前画面より引き継いだ構成IDよりアイテムマスタIDを取得
                ComDao.MsStructureEntity structure = new();
                structure = structure.GetEntity(result.StructureId, this.db);
                registInfo.ItemId = structure.StructureItemId ?? -1;
                if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsItemInfo, Master.SqlName.SubDir, registInfo, this.db))
                {
                    return false;
                }
                itemId = registInfo.ItemId;
            }

            return true;
        }

        /// <summary>
        /// アイテムマスタ拡張登録処理
        /// </summary>
        /// <param name="result">仕様項目選択肢情報</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registItemExtensionExcelPort(Dao.excelPortSpecItemList result, int itemId, DateTime now)
        {
            // 新規登録以外の場合は何もしない
            if (result.ProcessId != TMQConsts.SendProcessId.Regist)
            {
                return true;
            }

            // 登録情報を作成
            ComDao.MsItemExtensionEntity registInfo = new();

            // テーブル共通項目を設定
            setExecuteConditionByDataClassCommon(ref registInfo, now, int.Parse(this.UserId), int.Parse(this.UserId));

            registInfo.ItemId = itemId;                          // アイテムID
            registInfo.SequenceNo = 1;                           // 連番
            registInfo.ExtensionData = result.SpecId.ToString(); // 仕様項目ID

            // SQL実行
            if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.InsertMsItemExtensionInfo, Master.SqlName.SubDir, registInfo, this.db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 構成マスタ登録
        /// </summary>
        /// <param name="result">仕様項目選択肢情報</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="now">現在日時</param>
        /// <param name="structureId">構成ID</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registStructureExcelPort(Dao.excelPortSpecItemList result, int itemId, DateTime now, out int structureId)
        {
            structureId = -1;

            // 登録情報の作成
            ComDao.MsStructureEntity registInfo = new();
            registInfo.FactoryId = result.FactoryId;                                         // 工場ID
            registInfo.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.SpecSelectItem; // 構成グループID
            registInfo.ParentStructureId = null;                                             // 親構成ID
            registInfo.StructureLayerNo = null;                                              // 階層番号
            registInfo.StructureItemId = itemId;                                             // アイテムID

            // テーブル共通項目を設定
            setExecuteConditionByDataClassCommon(ref registInfo, now, int.Parse(this.UserId), int.Parse(this.UserId));

            // 新規・更新・削除の判定
            if (result.ProcessId == TMQConsts.SendProcessId.Regist)
            {
                // 新規の場合、採番したキーの値を取得
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out structureId, Master.SqlName.InsertMsStructureInfo, Master.SqlName.SubDir, registInfo, this.db))
                {
                    return false;
                }
            }
            else if (result.ProcessId == TMQConsts.SendProcessId.Update ||
                result.ProcessId == TMQConsts.SendProcessId.Delete)
            {
                // 削除の場合は削除フラグ = True
                if (result.ProcessId == TMQConsts.SendProcessId.Delete)
                {
                    registInfo.DeleteFlg = true;
                }

                // 更新の場合、前画面より引き継いだ構成IDを設定
                registInfo.StructureId = result.StructureId;
                TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfo, Master.SqlName.SubDir, registInfo, this.db);
                structureId = registInfo.StructureId;
            }

            return true;
        }

        /// <summary>
        /// 工場別アイテム表示順マスタ登録
        /// </summary>
        /// <param name="structureId">構成ID</param>
        /// <param name="result">仕様項目選択肢情報</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合あhFalse</returns>
        private bool registOrderExcelPort(int structureId, Dao.excelPortSpecItemList result, DateTime now)
        {
            // 登録情報の作成
            ComDao.MsStructureOrderEntity registInfo = new();
            registInfo.StructureId = structureId;                                            // 構成ID
            registInfo.FactoryId = (int)result.FactoryId;                                    // 工場ID
            registInfo.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.SpecSelectItem; // 構成グループID
            registInfo.DisplayOrder = result.DisplayOrder;                                   // 並び順

            // テーブル共通項目を設定
            setExecuteConditionByDataClassCommon(ref registInfo, now, int.Parse(this.UserId), int.Parse(this.UserId));

            // 新規登録以外は一度削除
            if (result.ProcessId != TMQConsts.SendProcessId.Regist)
            {
                new ComDao.MsStructureOrderEntity().DeleteByPrimaryKey(registInfo.StructureId, registInfo.FactoryId, this.db);
            }

            // 登録SQL実行
            if (!TMQUtil.SqlExecuteClass.Regist(Master.SqlName.InsertMsStructureOrder, Master.SqlName.SubDir, registInfo, this.db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ExcelPort 非表示列に保持している変更前の値をDBから取得
        /// </summary>
        /// <param name="result">Excelから取得したデータ</param>
        /// <param name="beforeResultList">ダウンロード検索SQLの実行結果</param>
        private void setSpecItemBeforeData(Dao.excelPortSpecItemList result, IList<Dao.excelPortSpecItemList> beforeResultList)
        {
            if (beforeResultList == null || beforeResultList.Count == 0)
            {
                result.TranslationTextBefore = null;
                return;
            }
            // 対象データの構成ID
            int structureId = result.StructureId;
            // 変更前の情報
            Dao.excelPortSpecItemList beforeResult = beforeResultList.Where(x => x.StructureId == structureId).FirstOrDefault();
            if (beforeResult != null)
            {
                // 非表示列に保持している変更前の値は、DBから取得した値を正とする
                result.TranslationTextBefore = beforeResult.TranslationTextBefore;
                result.ItemId = beforeResult.ItemId;
                result.TranslationId = beforeResult.TranslationId;
            }
            else
            {
                result.TranslationTextBefore = null;
            }
        }
        #endregion

        #region 共通処理
        /// <summary>
        /// 翻訳重複チェック
        /// </summary>
        /// <param name="isSpec">機種別仕様を登録する場合True、選択肢を登録する場合False</param>
        /// <param name="translationText">翻訳</param>
        /// <param name="translationTextBk">翻訳(変更前)</param>
        /// <param name="locationStructureId">工場ID</param>
        /// <param name="specId">仕様項目ID</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool checkTranslationTextExcelPort(bool isSpec, string translationText, string translationTextBk, long locationStructureId, int specId)
        {
            // 条件を設定
            Dao.excelPortChecTranslation condition = new();
            condition.TranslationText = translationText;                                    // 翻訳
            condition.TranslationTextBk = translationTextBk;                                // 翻訳(変更前)
            condition.LocationStructureId = locationStructureId;                            // 工場ID
            condition.LanguageId = this.LanguageId;                                         // 言語ID
            condition.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.SpecSelectItem; // 構成グループID
            condition.SequenceNo = 1;                                                       // 連番
            condition.ExtensionData = specId.ToString();                                    // 仕様項目ID

            // アイテムの翻訳重複チェックのSQL
            string sqlIdTransDuplicate = isSpec ? Sql.SpecRegist.GetCountDuplicateTranslation : Master.SqlName.GetCountTranslation;

            // アイテムの翻訳重複チェックのSQLのフォルダ
            string sqlDirTransDuplicate = isSpec ? Sql.SpecRegist.SubDir : Master.SqlName.SubDir;

            // アイテムの翻訳重複チェックのSQLのアンコメント文字列
            List<string> listUnCommentDuplicate = isSpec ? null : new List<string> { "Extension" };

            if (string.IsNullOrEmpty(condition.TranslationText))
            {
                // アイテム翻訳が未入力の場合、チェック対象外
                return true;
            }

            if (condition.TranslationText == condition.TranslationTextBk)
            {
                // アイテム翻訳に変更がない場合、チェック対象外
                return true;
            }

            // 同じ構成グループで同じ工場のアイテムで、同じ翻訳があるかを取得
            int cnt = 0;

            if (!TMQUtil.GetCountDb(condition, sqlIdTransDuplicate, ref cnt, db, sqlDirTransDuplicate, listUnCommentDuplicate))
            {
                return false;
            }

            if (cnt > 0)
            {
                // 翻訳がある場合、エラー
                return false;
            }

            return true;
        }

        /// <summary>
        /// 翻訳マスタ登録処理
        /// </summary>
        /// <param name="isNew">新規登録の場合True</param>
        /// <param name="translationText">翻訳</param>
        /// <param name="translationTextBk">翻訳(変更前)</param>
        /// <param name="translationIdBk">「翻訳(変更前)」の翻訳ID</param>
        /// <param name="factoryId">工場ID</param>
        /// <param name="now">現在日時</param>
        /// <param name="translationId">新しい翻訳ID</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registTranslationExcelPort(bool isNew,
                                                string translationText,
                                                string translationTextBk,
                                                int translationIdBk,
                                                int factoryId,
                                                DateTime now,
                                                out int translationId)
        {
            translationId = -1; // 採番した翻訳マスタのキー値

            // 翻訳の変更内容より、INSERTorUPDATEフラグと、UPDATEの場合は更新するキー値を取得
            bool isInsert = isInsertTranslation(out int updateTransId, out TMQUtil.ItemTranslationForMaster userTrans);
            // 翻訳を登録し、新規の場合は追加したキーを取得
            if (!executeRegistTranslation(isInsert, updateTransId, userTrans, out int insertedTransId))
            {
                return false;
            }

            translationId = isInsert ? insertedTransId : updateTransId;
            return true;

            /// <summary>
            /// 翻訳マスタにINSERTするかUPDATEするか判定する
            /// </summary>
            /// <param name="updTranslationId">UPDATEするばあい、対象の翻訳ID</param>
            /// <returns>INSERTの場合TRUE</returns>
            bool isInsertTranslation(out int updTranslationId, out TMQUtil.ItemTranslationForMaster userTrans)
            {
                updTranslationId = -1; // 使用する翻訳ID
                bool isInsert = false; // INSERTの場合TRUE

                // 条件を作成
                userTrans = new();
                userTrans.LocationStructureId = factoryId;       // 工場ID
                userTrans.LanguageId = this.LanguageId;          // 言語ID
                userTrans.TranslationText = translationText;     // 翻訳
                userTrans.TranslationTextBk = translationTextBk; // 翻訳(変更前)
                userTrans.TranslationId = translationIdBk;       //「翻訳(変更前)」の翻訳ID

                if (isNew)
                {
                    // 新規登録の場合
                    // 同じ工場で日本語で同じ翻訳があるか確認
                    isInsert = isNewTranslation(userTrans, out updTranslationId);
                    return isInsert;
                }
                else
                {
                    // 修正登録の場合

                    // ユーザ言語の翻訳の更新有無を確認
                    bool isUpdate = userTrans.TranslationText != userTrans.TranslationTextBk;
                    if (!isUpdate)
                    {
                        // 翻訳が変更されていない場合
                        // 翻訳ID取得
                        updTranslationId = userTrans.TranslationId ?? -1;
                        // 修正登録
                        return false;
                    }
                    // 翻訳が変更されている場合
                    // 入力された翻訳があるかどうか確認
                    isInsert = isNewTranslation(userTrans, out updTranslationId);
                    if (isInsert)
                    {
                        // ない場合でも、現在の翻訳IDをUPDATEする
                        updTranslationId = userTrans.TranslationId ?? -1;
                    }
                    // 修正登録の場合は必ずUPDATE
                    return false;
                }

                bool isNewTranslation(TMQUtil.ItemTranslationForMaster userTrans, out int updTranslationId)
                {
                    updTranslationId = -1;
                    // 同じ翻訳文字列の翻訳IDを取得
                    var registTranIds = TMQUtil.SqlExecuteClass.SelectList<int?>(Master.SqlName.GetMsTranslationInfo, Master.SqlName.SubDir, userTrans, this.db);
                    if (registTranIds == null || registTranIds.Count == 0)
                    {
                        // 同じ翻訳が存在しない場合、新規登録
                        return true;
                    }

                    updTranslationId = registTranIds.First().Value;
                    return false;

                }
            }

            /// <summary>
            /// 翻訳マスタの登録SQL実行
            /// </summary>
            /// <param name="isInsert">INSERTの場合TRUE</param>
            /// <param name="updateTransId">UPDATEの場合更新対象の翻訳ID</param>
            /// <param name="insertedTransId">out INSERTの場合採番した翻訳ID</param>
            /// <returns></returns>
            bool executeRegistTranslation(bool isInsert, int updateTransId, TMQUtil.ItemTranslationForMaster userTrans, out int insertedTransId)
            {
                insertedTransId = -1; // 採番した翻訳マスタのキー値

                // INSERTの場合、先頭行はINSERTしたキー値を取得し、以降は取得したキーでINSERTを行う
                bool isFirstInsert = true;
                // UPDATEの場合のSQLとコメントアウト文字列
                string registSql = Master.SqlName.UpdateMsTranslationInfo; // UPDATE
                List<string> listUnComment = new List<string> { "TranslationItemDescription" };
                if (isInsert)
                {
                    // INSERTの場合のSQL
                    registSql = Master.SqlName.InsertMsTranslationInfoGetTranslationId; //INSERT
                    listUnComment = new(); // コメントアウトは無し
                }

                // ユーザ言語以外の言語情報
                var transList = TMQUtil.SqlExecuteClass.SelectList<TMQUtil.ItemTranslationForMaster>(TMQUtil.ComMaster.SqlName.GetLanguageList, TMQUtil.ComMaster.SqlName.SubDir, new { LanguageId = this.LanguageId }, db);

                foreach (var rowDic in transList)
                {
                    // 繰り返し実行し、1行ずつ登録
                    if (!isInsert && string.IsNullOrEmpty(userTrans.TranslationText))
                    {
                        // 更新で入力内容が空の場合、登録しない
                        continue;
                    }

                    // 翻訳マスタ登録情報作成
                    ComDao.MsTranslationEntity condition = new();
                    condition.LocationStructureId = userTrans.LocationStructureId;                                       // 工場ID
                    condition.LanguageId = rowDic.LanguageId;                                                            // 言語ID
                    condition.TranslationText = rowDic.LanguageId == this.LanguageId ? userTrans.TranslationText : null; // 翻訳
                    condition.InsertUserId = int.Parse(this.UserId);                                                     // ユーザーID
                    condition.InsertDatetime = now;                                                                      // 現在日時
                    condition.UpdateUserId = int.Parse(this.UserId);                                                     // ユーザーID
                    condition.UpdateDatetime = now;                                                                      // 現在日時

                    if (isInsert)
                    {
                        if (isFirstInsert)
                        {
                            TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out insertedTransId, registSql, Master.SqlName.SubDir, condition, this.db, listUnComment: listUnComment);
                            isFirstInsert = false; // 以降はキーの採番を行わない
                            registSql = Master.SqlName.InsertMsTranslationInfo; //INSERT
                            continue;
                        }
                        else
                        {
                            condition.TranslationId = insertedTransId;
                        }
                    }
                    else
                    {
                        condition.TranslationId = updateTransId;
                    }

                    // ユーザ言語以外で翻訳の更新は行わない
                    if (rowDic.LanguageId != this.LanguageId && registSql == Master.SqlName.UpdateMsTranslationInfo)
                    {
                        continue;
                    }

                    // SQL実行
                    TMQUtil.SqlExecuteClass.Regist(registSql, Master.SqlName.SubDir, condition, this.db, listUnComment: listUnComment);
                }

                return true;
            }
        }

        #endregion
        #endregion

        #region privateメソッド
        /// <summary>
        /// 翻訳マスタ登録処理
        /// </summary>
        /// <param name="ctrlId">翻訳一覧のコントロールID</param>
        /// <param name="isNew">新規登録の場合TRUE</param>
        /// <param name="factoryId">工場ID</param>
        /// <param name="now">システム日時</param>
        /// <param name="translationId">out INSERT/UPDATEした翻訳ID</param>
        /// <returns>エラーの場合FALSE</returns>
        private bool registTranslation(string ctrlId, bool isNew, int factoryId, DateTime now, out int translationId)
        {
            translationId = -1; // 採番した翻訳マスタのキー値

            // 翻訳の変更内容より、INSERTorUPDATEフラグと、UPDATEの場合は更新するキー値を取得
            bool isInsert = isInsertTranslation(out int updateTransId);
            // 翻訳を登録し、新規の場合は追加したキーを取得
            executeRegistTranslation(isInsert, updateTransId, out int insertedTransId);

            translationId = isInsert ? insertedTransId : updateTransId;
            return true;

            /// <summary>
            /// 翻訳マスタにINSERTするかUPDATEするか判定する
            /// </summary>
            /// <param name="updTranslationId">UPDATEするばあい、対象の翻訳ID</param>
            /// <returns>INSERTの場合TRUE</returns>
            bool isInsertTranslation(out int updTranslationId)
            {
                updTranslationId = -1; // 使用する翻訳ID
                bool isInsert = false; // INSERTの場合TRUE

                // 翻訳一覧のデータクラスを取得
                var transList = convertDicListToClassList<Dao.Common.Translation>(this.resultInfoDictionary, ctrlId);
                // 自分の言語IDを取得
                string userLanguageId = getUserLanguageId();
                // 自分の言語を取得
                var userTrans = transList.Where(x => x.LanguageId == userLanguageId).FirstOrDefault();

                if (isNew)
                {
                    // 新規登録の場合
                    // 同じ工場で日本語で同じ翻訳があるか確認
                    isInsert = isNewTranslation(out updTranslationId);
                    return isInsert;
                }
                else
                {
                    // 修正登録の場合

                    // ユーザ言語の翻訳の更新有無を確認
                    bool isUpdate = userTrans.TranslationText != userTrans.TranslationTextBk;
                    if (!isUpdate)
                    {
                        // 翻訳が変更されていない場合
                        // 翻訳ID取得
                        updTranslationId = userTrans.TranslationId ?? -1;
                        // 修正登録
                        return false;
                    }
                    // 翻訳が変更されている場合
                    // 入力された翻訳があるかどうか確認
                    isInsert = isNewTranslation(out updTranslationId);
                    if (isInsert)
                    {
                        // ない場合でも、現在の翻訳IDをUPDATEする
                        updTranslationId = userTrans.TranslationId ?? -1;
                    }
                    // 修正登録の場合は必ずUPDATE
                    return false;
                }

                bool isNewTranslation(out int updTranslationId)
                {
                    updTranslationId = -1;
                    // 同じ翻訳文字列の翻訳IDを取得
                    var registTranIds = TMQUtil.SqlExecuteClass.SelectList<int?>(Master.SqlName.GetMsTranslationInfo, Master.SqlName.SubDir, userTrans, this.db);
                    if (registTranIds == null || registTranIds.Count == 0)
                    {
                        // 同じ翻訳が存在しない場合、新規登録
                        return true;
                    }

                    updTranslationId = registTranIds.First().Value;
                    return false;

                }
            }

            /// <summary>
            /// 翻訳マスタの登録SQL実行
            /// </summary>
            /// <param name="isInsert">INSERTの場合TRUE</param>
            /// <param name="updateTransId">UPDATEの場合更新対象の翻訳ID</param>
            /// <param name="insertedTransId">out INSERTの場合採番した翻訳ID</param>
            /// <returns></returns>
            bool executeRegistTranslation(bool isInsert, int updateTransId, out int insertedTransId)
            {
                insertedTransId = -1; // 採番した翻訳マスタのキー値

                // INSERTの場合、先頭行はINSERTしたキー値を取得し、以降は取得したキーでINSERTを行う
                bool isFirstInsert = true;
                // UPDATEの場合のSQLとコメントアウト文字列
                string registSql = Master.SqlName.UpdateMsTranslationInfo; // UPDATE
                List<string> listUnComment = new List<string> { "TranslationItemDescription" };
                if (isInsert)
                {
                    // INSERTの場合のSQL
                    registSql = Master.SqlName.InsertMsTranslationInfoGetTranslationId; //INSERT
                    listUnComment = new(); // コメントアウトは無し
                }

                // 一覧の内容を取得
                var dicTranslations = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);
                foreach (var rowDic in dicTranslations)
                {
                    // 繰り返し実行し、1行ずつ登録
                    ComDao.MsTranslationEntity regist = new();
                    SetExecuteConditionByDataClass(rowDic, ctrlId, regist, now, this.UserId, this.UserId);
                    regist.LocationStructureId = factoryId; // 退避した前画面から引き継いだ工場ID
                    if (!isInsert && string.IsNullOrEmpty(regist.TranslationText))
                    {
                        // 更新で入力内容が空の場合、登録しない
                        continue;
                    }
                    if (isInsert)
                    {
                        if (isFirstInsert)
                        {
                            TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out insertedTransId, registSql, Master.SqlName.SubDir, regist, this.db, listUnComment: listUnComment);
                            isFirstInsert = false; // 以降はキーの採番を行わない
                            registSql = Master.SqlName.InsertMsTranslationInfo; //INSERT
                            continue;
                        }
                        else
                        {
                            regist.TranslationId = insertedTransId;
                        }
                    }
                    else
                    {
                        regist.TranslationId = updateTransId;
                    }
                    // SQL実行
                    TMQUtil.SqlExecuteClass.Regist(registSql, Master.SqlName.SubDir, regist, this.db, listUnComment: listUnComment);
                }

                return true;
            }
        }

        /// <summary>
        /// ユーザの言語を取得
        /// </summary>
        /// <returns>言語ID</returns>
        private string getUserLanguageId()
        {
            int userId = int.Parse(this.UserId);
            var userInfo = new ComDao.MsUserEntity().GetEntity(userId, this.db);
            return userInfo.LanguageId;
        }

        /// <summary>
        /// 翻訳一覧入力チェック
        /// </summary>
        /// <param name="isSpec">機種別仕様を登録する場合True、選択肢を登録する場合False</param>
        /// <param name="keyInfo">登録用キー情報</param>
        /// <param name="errorInfoDictionary">out エラー情報</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorTranslationList(bool isSpec, Dao.Common.Param keyInfo, out List<Dictionary<string, object>> errorInfoDictionary)
        {
            // 一覧のID
            string ctrlId = isSpec ? FormInfo.SpecRegist.TranslateList : FormInfo.SpecItemRegist.List;
            // アイテムの翻訳重複チェックのSQL
            string sqlIdTransDuplicate = isSpec ? Sql.SpecRegist.GetCountDuplicateTranslation : Master.SqlName.GetCountTranslation;
            // アイテムの翻訳重複チェックのSQLのフォルダ
            string sqlDirTransDuplicate = isSpec ? Sql.SpecRegist.SubDir : Master.SqlName.SubDir;
            // アイテムの翻訳重複チェックのSQLのアンコメント文字列
            List<string> listUnCommentDuplicate = isSpec ? null : new List<string> { "Extension" };

            errorInfoDictionary = new();
            // 戻り値、エラーの場合True
            bool isError = false;

            // 翻訳一覧のマッピング情報リスト
            var infoTransList = getResultMappingInfo(ctrlId);

            // 翻訳一覧の画面の内容
            var dicTranslationList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);

            // ユーザ言語を取得
            var userLanguageId = getUserLanguageId();

            // 翻訳一覧を繰り返し処理
            foreach (var dicRow in dicTranslationList)
            {
                // 行の内容をデータクラスに変換
                Dao.Common.Translation transRow = new();
                SetDataClassFromDictionary(dicRow, ctrlId, transRow);

                // 表示するエラー情報を設定
                ErrorInfo errorInfo = new ErrorInfo(dicRow);
                // 行単位のエラー有無
                bool isErrorRow = false;

                // ユーザ言語の必須チェック
                if (isErrorTransNameNull(transRow, userLanguageId))
                {
                    isErrorRow = true;
                    // ツールチップ表示項目のVAL値
                    string val = infoTransList.getValName("item_tran_name");
                    // 「入力してください。」
                    string msg = GetResMessage(ComRes.ID.ID941220009);
                    errorInfo.setError(msg, val);
                }
                // アイテム翻訳重複チェック
                if (isErrorDuplicateItemTraslation(transRow))
                {
                    isErrorRow = true;
                    // ツールチップ表示項目のVAL値
                    string val = infoTransList.getValName("item_tran_name");
                    // 「アイテム翻訳は既に登録されています。」
                    string errMsg = GetResMessage(new string[] { ComRes.ID.ID941260001, ComRes.ID.ID111010005 });
                    errorInfo.setError(errMsg, val);
                }

                if (isErrorRow)
                {
                    isError = true;
                    // エラー情報を設定
                    errorInfoDictionary.Add(errorInfo.Result);
                }
            }

            return isError;

            // ユーザ言語の翻訳必須チェック
            bool isErrorTransNameNull(Dao.Common.Translation transRow, string userLanguageId)
            {
                // 行の言語がログインユーザの言語の場合
                if (transRow.LanguageId == userLanguageId)
                {
                    // 行の名称がNullのない場合、エラー
                    bool isError = string.IsNullOrEmpty(transRow.TranslationText);
                    return isError;
                }
                // ログインユーザの言語でない場合はエラーにならない
                return false;
            }

            // アイテム翻訳重複チェック
            bool isErrorDuplicateItemTraslation(Dao.Common.Translation transRow)
            {
                if (string.IsNullOrEmpty(transRow.TranslationText))
                {
                    // アイテム翻訳が未入力の場合、チェック対象外
                    return false;
                }
                if (transRow.TranslationText == transRow.TranslationTextBk)
                {
                    // アイテム翻訳に変更がない場合、チェック対象外
                    return false;
                }
                // 同じ構成グループで同じ工場のアイテムで、同じ翻訳があるかを取得
                int cnt = 0;

                if (!isSpec)
                {
                    // 選択肢の場合、検索SQLが異なるため追加で条件に設定
                    transRow.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.SpecSelectItem;
                    transRow.SequenceNo = 1;
                    transRow.ExtensionData = keyInfo.SpecId.ToString();
                }
                if (!TMQUtil.GetCountDb(transRow, sqlIdTransDuplicate, ref cnt, db, sqlDirTransDuplicate, listUnCommentDuplicate))
                {
                    return true;
                }

                if (cnt > 0)
                {
                    // 翻訳がある場合、エラー
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 一覧に対する排他チェック
        /// </summary>
        /// <param name="ctrlId">一覧のID</param>
        /// <param name="isTranslation">翻訳の場合、IDが無ければチェック対象から外すためTRUE</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusiveList(string ctrlId, bool isTranslation = false)
        {
            var dicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);
            if (isTranslation)
            {
                // 翻訳の排他チェックの場合、特定の言語は登録されていない場合があり、その場合に必ず排他チェックエラーとなるのを避けるため、チェック対象より除外する
                var mapInfo = getResultMappingInfo(ctrlId);
                string valNo = mapInfo.getValName("translation_id");
                dicList = dicList.Where(x => !string.IsNullOrEmpty(x[valNo].ToString())).ToList();
            }
            bool result = !checkExclusiveList(ctrlId, dicList);
            return result;
        }

        /// <summary>
        /// 翻訳の一時テーブルを作成
        /// </summary>
        private void registTempTable()
        {
            // 翻訳の一時テーブルを作成
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);

            // 翻訳する構成グループのリスト
            var structuregroupList = new List<GroupId>
            {
                GroupId.Location,
                GroupId.SpecSelectItem,
                GroupId.SpecUnitType,
                GroupId.SpecUnit,
                GroupId.SpecType,
                GroupId.SpecNumDecimalPlaces
            };

            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            listPf.RegistTempTable(); // 登録
        }
        #endregion
    }
}