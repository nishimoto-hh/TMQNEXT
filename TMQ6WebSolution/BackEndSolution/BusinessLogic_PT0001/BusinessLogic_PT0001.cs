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
using Dao = BusinessLogic_PT0001.BusinessLogicDataClass_PT0001;
using DbTransaction = System.Data.IDbTransaction;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;

namespace BusinessLogic_PT0001
{
    /// <summary>
    /// 予備品一覧
    /// </summary>
    public partial class BusinessLogic_PT0001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Parts";
            /// <summary>SQL名：共通のWITH句</summary>
            public const string Common = "Common";

            /// <summary>
            /// 一覧画面
            /// </summary>
            public static class List
            {
                /// <summary>SQL名：一覧取得</summary>
                public const string GetPartsList = "GetPartsList";
                /// <summary>SQL名：一覧画面検索時、データの件数を取得するSQL</summary>
                public const string GetCountPartsList = "GetCountPartsList";
            }

            /// <summary>
            /// 詳細画面
            /// </summary>
            public static class Detail
            {
                /// <summary>SQL名：予備品情報取得</summary>
                public const string GetDetailPartsInfo = "GetDetailPartsInfo";
                /// <summary>SQL名：棚別在庫情報取得(親一覧)</summary>
                public const string GetInventoryParentList = "GetInventoryParentList ";
                /// <summary>SQL名：棚別在庫情報取得(子一覧)</summary>
                public const string GetInventoryChildList = "GetInventoryChildList";
                /// <summary>SQL名：部門別在庫情報取得(親一覧)</summary>
                public const string GetCategoryParentList = "GetCategoryParentList";
                /// <summary>SQL名：部門別在庫情報取得(子一覧)</summary>
                public const string GetCategoryChildList = "GetCategoryChildList";
                /// <summary>SQL名：工場IDを取得</summary>
                public const string GetFactoryIdByPartsId = "GetFactoryIdByPartsId";
                /// <summary>SQL名：受払履歴より予備品IDの件数を取得(削除処理用)</summary>
                public const string GetHistoryCountByPartsId = "GetHistoryCountByPartsId";
                /// <summary>SQL名：入出庫履歴一覧</summary>
                public const string GetInoutHistoryList = "GetInoutHistoryList";
                /// <summary>SQL名：添付情報取得</summary>
                public const string GetAttachmentInfo = "GetAttachmentInfo";
                /// <summary>SQL名：工場の期首月取得</summary>
                public const string GetStartMonthByFactoryId = "GetStartMonthByFactoryId";
                /// <summary>SQL名：予備品に紐付く添付情報の最大更新日時を取得</summary>
                public const string GetMaxDateByPartsId = "GetMaxDateByPartsId";
            }

            /// <summary>
            /// 詳細編集画面
            /// </summary>
            public static class Edit
            {
                /// <summary>SQL名：予備品情報登録</summary>
                public const string InsertPartsInfo = "InsertPartsInfo";
                /// <summary>SQL名：予備品No重複チェック用SQL</summary>
                public const string GetPartsNoCount = "GetPartsNoCount";
                /// <summary>SQL名：棚IDより倉庫IDを取得</summary>
                public const string GetParentIdByLocationId = "GetParentIdByLocationId";
                /// <summary>SQL名：予備品情報更新</summary>
                public const string UpdatePartsInfo = "UpdatePartsInfo";
                /// <summary>SQL名：数量管理単位小数点以下桁数取得</summary>
                public const string GetUnitDigit = "GetUnitDigit";
                /// <summary>SQL名：数量管理単位丸め処理区分取得</summary>
                public const string GetUnitRoundDivision = "GetUnitRoundDivision";
                /// <summary>SQL名：金額管理単位小数点以下桁数・丸め処理区分取得</summary>
                public const string GetCurrencyDigitAndDivision = "GetCurrencyDigitAndDivision";
            }

            /// <summary>
            /// ラベル出力画面
            /// </summary>
            public static class Label
            {
                /// <summary>SQL名：勘定科目一覧取得</summary>
                public const string GetLabelSubjectList = "GetLabelSubjectList";
                /// <summary>SQL名：部門一覧取得</summary>
                public const string GetLabelDepartmentList = "GetLabelDepartmentList";
                /// <summary>SQL名：ラベル用データ取得</summary>
                public const string GetLabelData = "GetLabelData";
                /// <summary>SQL名：一時テーブルに構成ID(共通工場ID)を追加するSQL/summary>
                public const string InsertCommonFactoryIdToTemp = "InsertCommonFactoryIdToTemp";
            }

            public static class ExcelPort
            {
                /// <summary>SQL名：ExcelPort予備品仕様取得</summary>
                public const string GetExcelPortPartsList = "GetExcelPortPartsList";
                /// <summary>SQL名：工場の丸め処理区分を取得</summary>
                public const string GetRoundDivision = "GetRoundDivision";
            }
        }

        /// <summary>
        /// フォーム、グループ、コントロールの親子関係を表現した場合の定数クラス
        /// </summary>
        private static class ConductInfo
        {
            /// <summary>
            /// 一覧画面
            /// </summary>
            public static class FormList
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 0;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// フィルター
                    /// </summary>
                    public const string Filter = "BODY_010_00_LST_0";
                    /// <summary>
                    /// 予備品一覧
                    /// </summary>
                    public const string List = "BODY_020_00_LST_0";
                }
            }

            /// <summary>
            /// 詳細画面
            /// </summary>
            public static class FormDetail
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 1;
                /// <summary>
                /// グループ番号
                /// </summary>
                public const short GroupNo = 201;
                /// <summary>
                /// 再表示ボタンのボタンコントロールID
                /// </summary>
                public const string BtnReDisp = "ReDisp";
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 予備品情報
                    /// </summary>
                    public const string PartsInfo = "BODY_010_00_LST_1";
                    /// <summary>
                    /// 標準保管場所情報
                    /// </summary>
                    public const string DefaultSafeKeep = "BODY_020_00_LST_1";
                    /// <summary>
                    /// 画像
                    /// </summary>
                    public const string Image = "BODY_030_00_LST_1";
                    /// <summary>
                    /// 購買管理情報
                    /// </summary>
                    public const string PurchaseInfo = "BODY_040_00_LST_1";
                    /// <summary>
                    /// メモ
                    /// </summary>
                    public const string Memo = "BODY_050_00_LST_1";
                    /// <summary>
                    /// 棚別在庫情報(親一覧)
                    /// </summary>
                    public const string InventoryParentList = "BODY_080_00_LST_1";
                    /// <summary>
                    /// 棚別在庫情報(子一覧)
                    /// </summary>
                    public const string InventoryChildList = "BODY_090_00_LST_1";
                    /// <summary>
                    /// 部門別在庫情報(親一覧)
                    /// </summary>
                    public const string CategoryParentList = "BODY_100_00_LST_1";
                    /// <summary>
                    /// 部門別在庫情報(子一覧)
                    /// </summary>
                    public const string CategoryChildList = "BODY_110_00_LST_1";
                    /// <summary>
                    /// 表示年度
                    /// </summary>
                    public const string DispYear = "BODY_120_00_LST_1";
                    /// <summary>
                    /// 入出庫履歴一覧
                    /// </summary>
                    public const string InOutHistoryList = "BODY_140_00_LST_1";
                }
            }

            /// <summary>
            /// 詳細編集画面
            /// </summary>
            public static class FormEdit
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 2;

                /// <summary>
                /// グループ番号
                /// </summary>
                public const short GroupNo = 301;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 予備品情報
                    /// </summary>
                    public const string PartsInfo = "BODY_010_00_LST_2";
                    /// <summary>
                    /// 使用場所
                    /// </summary>
                    public const string UsedPlace = "BODY_020_00_LST_2";
                    /// <summary>
                    /// 標準保管場所情報(地区～倉庫)
                    /// </summary>
                    public const string DefaultSafeKeep = "BODY_030_00_LST_2";
                    /// <summary>
                    /// 標準保管場所情報(棚)
                    /// </summary>
                    public const string DefaultSafeKeepLocation = "BODY_040_00_LST_2";
                    /// <summary>
                    /// 棚枝番
                    /// </summary>
                    public const string DetailNo = "BODY_070_00_LST_2";
                    /// <summary>
                    /// 結合文字列
                    /// </summary>
                    public const string JoinString = "BODY_080_00_LST_2";
                    /// <summary>
                    /// 購買管理情報
                    /// </summary>
                    public const string PurchaseInfo = "BODY_050_00_LST_2";
                    /// <summary>
                    /// メモ
                    /// </summary>
                    public const string Memo = "BODY_050_00_LST_2";
                }
            }

            /// <summary>
            /// ラベル出力画面
            /// </summary>
            public static class FormLabel
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
                    /// 勘定科目選択一覧
                    /// </summary>
                    public const string SubjectList = "BODY_000_00_LST_3";
                    /// <summary>
                    /// 部門選択一覧
                    /// </summary>
                    public const string DepartmentList = "BODY_010_00_LST_3";
                    /// <summary>
                    /// 予備品ID一覧
                    /// </summary>
                    public const string PartsIdList = "BODY_020_00_LST_3";
                }
            }

            /// <summary>ExcelPortアップロード</summary>
            public const string ExcelPortUpload = "LIST_000_1";

            /// <summary>
            /// ExcelPort予備品仕様シート情報
            /// </summary>
            public static class ExcelPortPartsListInfo
            {
                // データ開始行
                public const int StartRowNo = 4;
                // 送信時処理列番号
                public const int ProccesColumnNo = 4;
                // 予備品No列番号
                public const int PartsNoColumnNo = 5;
                // 棚枝番列番号
                public const int PartsLocationDetailNoColumnNo = 27;
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_PT0001() : base()
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
            //画面番号を判定
            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo:               // 一覧画面
                case ConductInfo.FormLabel.FormNo:              // ラベル出力画面
                    // SearchImplを呼出
                    return InitSearch();
                    break;
                case ConductInfo.FormDetail.FormNo:             // 詳細画面
                    // 詳細編集画面の新規登録後か初期表示(一覧画面のNo.リンク)を判定
                    DetailDispType detailType = compareId.IsRegist() ? DetailDispType.AfterRegist : DetailDispType.Init;
                    if (compareId.IsBack())
                    {
                        // 戻るの場合
                        detailType = DetailDispType.Search;
                    }
                    // 初期検索実行
                    if (!searchDetailList(detailType))
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormEdit.FormNo:               // 詳細編集画面(新規・修正・複写)
                    // 初期検索実行
                    if (!searchEdit())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                default:
                    return ComConsts.RETURN_RESULT.OK;
            }

            // 初期化処理で処理を行わない場合は以下のように定数を返す
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            //画面番号を判定
            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo:               // 一覧画面
                    if (!searchList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case ConductInfo.FormLabel.FormNo:        // ラベル出力画面
                    if (!searchLabelList())
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
            else if (this.CtrlId == ConductInfo.FormDetail.BtnReDisp)
            {
                // 詳細画面(再表示ボタン押下時)
                if (!searchDetailList(DetailDispType.Redisplay))
                {
                    return ComConsts.RETURN_RESULT.NG;
                }
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
            // 詳細編集画面の登録処理結果によりエラー処理を行う
            if (!executeRegistEdit())
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
            this.ResultList = new();

            // 詳細画面 削除ボタン
            if (!deletePartsInfo())
            {
                setError();
                return ComConsts.RETURN_RESULT.NG;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「削除処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911110001 });

            return ComConsts.RETURN_RESULT.OK;

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
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            // ファイルダウンロードの場合
            if (compareId.IsDownload())
            {
                // ダウンロード情報取得
                var info = TMQUtil.GetFileDownloadInfo(this.searchConditionDictionary, this.db, out bool isError);
                if (isError)
                {
                    // エラーの場合は終了
                    OutputFileDownloadError();
                    return ComConsts.RETURN_RESULT.NG;
                }
                // ファイルをダウンロード
                if (!OutputDownloadFile(info.FileName, info.FilePath))
                {
                    // エラーの場合は終了
                    return ComConsts.RETURN_RESULT.NG;
                }
                return ComConsts.RETURN_RESULT.OK;
            }
            // ラベル用データ出力
            if (!outputLabelData())
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

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

            //TODO: 個別データ検索処理
            IList<Dictionary<string, object>> dataList = null;

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.ExcelPort.GetExcelPortPartsList, out string baseSql);
            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.ExcelPort.GetExcelPortPartsList, out string withSql);

            // 翻訳の一時テーブルを作成
            TMQUtil.ListPerformanceUtil listPf = new(this.db, this.LanguageId);

            // 翻訳する構成グループのリスト
            var structuregroupList = new List<GroupId>
            {
                GroupId.Location,
                GroupId.Job,
                GroupId.SpareLocation,
                GroupId.Manufacturer,
                GroupId.Vender,
                GroupId.Unit,
                GroupId.Currency,
                GroupId.PartsUseSegment
            };
            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            listPf.RegistTempTable(); // 登録

            // 一覧検索SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, null, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY");    // 並び順を指定
            selectSql.AppendLine("parts_no");    // 予備品No. 昇順
            selectSql.AppendLine(",parts_name"); // 予備品名  昇順

            // 一覧検索実行
            IList<Dao.excelPortPartsList> results = db.GetListByDataClass<Dao.excelPortPartsList>(selectSql.ToString(), new { LanguageId = this.LanguageId });
            if (results == null)
            {
                // 「ダウンロード処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                return ComConsts.RETURN_RESULT.NG;
            }

            // Dicitionalyに変換
            dataList = ComUtil.ConvertClassToDictionary<Dao.excelPortPartsList>(results);

            // 個別シート出力処理
            if (!excelPort.OutputExcelPortTemplateFile(dataList, out fileType, out fileName, out ms, out detailMsg, ref resultMsg))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
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

            // ExcelPortアップロードデータの取得
            if (!excelPort.GetUploadDataList(file, this.IndividualDictionary, ConductInfo.ExcelPortUpload,
                out List<BusinessLogicDataClass_PT0001.excelPortPartsList> resultList, out resultMsg, ref fileType, ref fileName, ref ms))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // エラー情報リスト
            List<ComDao.UploadErrorInfo> errorInfoList = new List<CommonDataBaseClass.UploadErrorInfo>();

            // 予備品仕様入力チェック＆登録処理
            if (!executeExcelPortRegist(resultList, ref errorInfoList))
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
        #endregion

        #region privateメソッド
        /// <summary>
        /// ExcelPort予備品仕様 登録処理
        /// </summary>
        /// <param name="resultList">登録情報リスト</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool executeExcelPortRegist(List<BusinessLogicDataClass_PT0001.excelPortPartsList> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            DateTime now = DateTime.Now;

            // 工場毎の丸め処理区分用のDictionary
            Dictionary<long, int> roundDivisionDic = new();

            // 数量管理単位の小数点以下桁数用のDictionary
            Dictionary<long, int> unitDigitDic = new();

            // 金額管理単位の小数点以下桁数用のDictionary
            Dictionary<long, int> currencyDigitDic = new();

            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            // 登録処理
            foreach (BusinessLogicDataClass_PT0001.excelPortPartsList result in resultList)
            {
                // 送信時処理が設定されていない場合は何もしない
                if (result.ProcessId == null)
                {
                    continue;
                }
                rowErrFlg = false;

                // 工場の丸め処理区分を取得
                getRoundDivision(result.PartsFactoryId);

                //数量管理単位・金額管理単位の小数点以下桁数を取得
                getDigit((long)result.UnitStructureId, (long)result.CurrencyStructureId);

                // 新規登録または更新の場合
                if (result.ProcessId == TMQConst.SendProcessId.Regist ||
                    result.ProcessId == TMQConst.SendProcessId.Update)
                {
                    // 棚枝番の半角英数字チェック
                    if (!string.IsNullOrEmpty(result.PartsLocationDetailNo))
                    {
                        var enc = Encoding.GetEncoding("Shift_JIS");
                        if (enc.GetByteCount(result.PartsLocationDetailNo) != result.PartsLocationDetailNo.Length || !ComUtil.IsAlphaNumeric(result.PartsLocationDetailNo))
                        {
                            // 半角英数字で入力してください。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPortPartsListInfo.PartsLocationDetailNoColumnNo, GetResMessage(new string[] { ComRes.ID.ID111270032 }), GetResMessage(new string[] { ComRes.ID.ID141260002 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = true;
                            rowErrFlg = true;
                        }
                    }

                    // エラー有りなら次へ
                    if (rowErrFlg)
                    {
                        continue;
                    }

                    // 予備品No重複チェック
                    if (result.PartsNo != result.PartsNoBefore)
                    {
                        TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Edit.GetPartsNoCount, out string outSql);
                        if (db.GetEntityByDataClass<int>(outSql, new { PartsNo = result.PartsNo }) > 0)
                        {
                            // 指定された予備品Noがすでに登録されています。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPortPartsListInfo.PartsNoColumnNo, GetResMessage(new string[] { ComRes.ID.ID111380022 }), GetResMessage(new string[] { ComRes.ID.ID141120003, ComRes.ID.ID111380022 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = true;
                            rowErrFlg = true;
                        }
                    }

                    // エラー有りなら次へ
                    if (rowErrFlg)
                    {
                        continue;
                    }
                }

                // 登録用データクラスに格納
                Dao.editResult registInfo = setRegistInfo(result);

                // テーブル共通項目を設定
                registInfo.InsertDatetime = now;
                registInfo.InsertUserId = int.Parse(this.UserId);
                registInfo.UpdateDatetime = now;
                registInfo.UpdateUserId = int.Parse(this.UserId);

                // 送信時処理を判定
                switch (result.ProcessId)
                {
                    case TMQConst.SendProcessId.Regist: // 新規登録
                        if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long newPartsId, SqlName.Edit.InsertPartsInfo, SqlName.SubDir, registInfo, db))
                        {
                            return false;
                        }
                        break;
                    case TMQConst.SendProcessId.Update: // 更新
                        if (!TMQUtil.SqlExecuteClass.Regist(SqlName.Edit.UpdatePartsInfo, SqlName.SubDir, registInfo, db))
                        {
                            return false;
                        }
                        break;

                    case TMQConst.SendProcessId.Delete: // 削除

                        // 受払履歴存在チェック
                        TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetHistoryCountByPartsId, out string outSql);
                        if (db.GetEntityByDataClass<int>(outSql, new { PartsId = result.PartsId }) > 0)
                        {
                            // 入出庫データが存在するため、削除できません。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)result.RowNo, ConductInfo.ExcelPortPartsListInfo.ProccesColumnNo, null, GetResMessage(new string[] { ComRes.ID.ID141220004 }), TMQUtil.ComReport.LongitudinalDirection, result.ProcessId.ToString()));
                            errFlg = true;
                            rowErrFlg = true;
                        }

                        // エラー有りなら次へ
                        if (rowErrFlg)
                        {
                            continue;
                        }

                        // 予備品情報削除
                        if (!new ComDao.PtPartsEntity().DeleteByPrimaryKey(result.PartsId, this.db))
                        {
                            return false;
                        }

                        Dao.detailSearchCondition condition = new();
                        condition.PartsId = result.PartsId;

                        // 添付情報削除(画像)
                        if (!deleteAttachmentInfo(condition, TMQConst.Attachment.FunctionTypeId.SpareImage))
                        {
                            return false;
                        }

                        // 添付情報削除(文書)
                        if (!deleteAttachmentInfo(condition, TMQConst.Attachment.FunctionTypeId.SpareDocument))
                        {
                            return false;
                        }

                        break;
                    default:
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

            // Excelで入力された内容を登録用のデータクラスに設定する
            Dao.editResult setRegistInfo(BusinessLogicDataClass_PT0001.excelPortPartsList result)
            {
                Dao.editResult registInfo = new();
                registInfo.PartsId = result.PartsId;                                 // 予備品ID
                registInfo.PartsNo = result.PartsNo;                                 // 予備品No
                registInfo.PartsName = result.PartsName;                             // 予備品名
                registInfo.ManufacturerStructureId = result.ManufacturerStructureId; // メーカー
                registInfo.Materials = result.Materials;                             // 材質
                registInfo.ModelType = result.ModelType;                             // 型式
                registInfo.StandardSize = result.StandardSize;                       // 規格・寸法
                registInfo.PartsServiceSpace = result.PartsServiceSpace;             // 使用場所
                registInfo.PartsFactoryId = result.PartsFactoryId;                   // 管理工場
                registInfo.JobStructureId = result.JobId;                            // 職種
                registInfo.UseSegmentStructureId = result.UseSegmentStructureId;     // 使用区分
                registInfo.PartsLocationDetailNo = string.IsNullOrEmpty(result.PartsLocationDetailNo) ? string.Empty : result.PartsLocationDetailNo; // 標準棚枝番(入力されていない場合は空文字)
                registInfo.LeadTimeExceptUnit = result.LeadTime == null ? 0 : result.LeadTime; // 発注点
                registInfo.OrderQuantityExceptUnit = result.OrderQuantity == null ? 0 : result.OrderQuantity; // 発注量
                registInfo.PartsLocationId = result.RackId == null ? (long)result.WarehouseId : (long)result.RackId; // 標準棚番ID(標準棚番まで入力されている場合は予備品倉庫ではなく標準棚番を登録する)
                registInfo.UnitStructureId = result.UnitStructureId;                 // 数量管理単位
                registInfo.VenderStructureId = result.VenderStructureId;             // 標準仕入先
                registInfo.CurrencyStructureId = result.CurrencyStructureId;         // 金額管理単位
                registInfo.UnitPriceExceptUnit = result.UnitPrice == null ? 0 : (decimal)result.UnitPrice; // 標準単価
                registInfo.PurchasingNo = result.PurchasingNo;                       // 購買システムコード
                registInfo.PartsMemo = result.PartsMemo;                             // メモ
                registInfo.LocationDistrictStructureId = result.DistrictId;          // 地区ID
                registInfo.LocationFactoryStructureId = result.FactoryId;            // 工場ID
                registInfo.LocationWarehouseStructureId = result.WarehouseId;        // 倉庫ID
                registInfo.LocationRackStructureId = result.RackId;                  // 棚ID

                // 丸め処理
                // 発注点
                registInfo.LeadTimeExceptUnit = decimal.Parse(TMQUtil.roundDigit(registInfo.LeadTimeExceptUnit.ToString(), unitDigitDic[(long)registInfo.UnitStructureId], roundDivisionDic[registInfo.PartsFactoryId], true));
                // 発注量
                registInfo.OrderQuantityExceptUnit = decimal.Parse(TMQUtil.roundDigit(registInfo.OrderQuantityExceptUnit.ToString(), unitDigitDic[(long)registInfo.UnitStructureId], roundDivisionDic[registInfo.PartsFactoryId], true));
                // 標準単価
                registInfo.UnitPriceExceptUnit = decimal.Parse(TMQUtil.roundDigit(registInfo.UnitPriceExceptUnit.ToString(), currencyDigitDic[(long)registInfo.CurrencyStructureId], roundDivisionDic[registInfo.PartsFactoryId], true));

                return registInfo;
            }

            // 工場の丸め処理区分を取得
            void getRoundDivision(long factoryId)
            {
                // 工場の丸め処理区分がディクショナリに含まれているか判定
                if (roundDivisionDic.ContainsKey(factoryId))
                {
                    // 既に含まれている場合は何もしない
                    return;
                }

                // SQL取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.ExcelPort.GetRoundDivision, out string outSql);
                // SQLを実行し、工場の丸め処理区分を取得
                int roundDivision = db.GetEntityByDataClass<int>(outSql, new { FactoryId = factoryId });
                // 取得結果をディクショナリに格納
                roundDivisionDic.Add(factoryId, roundDivision);
            }

            // 数量管理単位・金額管理単位の小数点以下桁数を取得
            void getDigit(long unitStructureId, long currencyStructureId)
            {
                // 数量管理単位
                if (!unitDigitDic.ContainsKey(unitStructureId))
                {
                    // 数量管理単位の小数点以下桁数を取得
                    ComDao.MsStructureEntity structureInfo = new ComDao.MsStructureEntity().GetEntity((int)unitStructureId, this.db);
                    ComDao.MsItemExtensionEntity extensionInfo = new ComDao.MsItemExtensionEntity().GetEntity((int)structureInfo.StructureItemId, 2, this.db);
                    // 小数点以下桁数をディクショナリに格納
                    unitDigitDic.Add(unitStructureId, int.Parse(extensionInfo.ExtensionData));
                }

                // 金額管理単位
                if (!currencyDigitDic.ContainsKey(currencyStructureId))
                {
                    // 金額管理単位の小数点以下桁数を取得
                    ComDao.MsStructureEntity structureInfo = new ComDao.MsStructureEntity().GetEntity((int)currencyStructureId, this.db);
                    ComDao.MsItemExtensionEntity extensionInfo = new ComDao.MsItemExtensionEntity().GetEntity((int)structureInfo.StructureItemId, 2, this.db);
                    // 小数点以下桁数をディクショナリに格納
                    currencyDigitDic.Add(currencyStructureId, int.Parse(extensionInfo.ExtensionData));
                }
            }
        }
        #endregion
    }
}