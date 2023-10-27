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
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_PT0001.BusinessLogicDataClass_PT0001;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

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
                /// <summary>SQL名：一覧取得()</summary>
                public const string GetPartsList = "GetPartsList";
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
        #endregion

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

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied, true))
            {
                // 「ダウンロード処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                return ComConsts.RETURN_RESULT.NG;
            }

            // SQLパラメータに言語ID設定
            whereParam.LanguageId = this.LanguageId;
            // 一覧検索SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY");    // 並び順を指定
            selectSql.AppendLine("parts_no");    // 予備品No. 昇順
            selectSql.AppendLine(",parts_name"); // 予備品名  昇順

            // 一覧検索実行
            IList<Dao.excelPortPartsList> results = db.GetListByDataClass<Dao.excelPortPartsList>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                // 「ダウンロード処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911160003 });
                return ComConsts.RETURN_RESULT.NG;
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.excelPortPartsList>(ref results, new List<StructureType> { StructureType.Job, StructureType.SpareLocation }, this.db, this.LanguageId);

            // Dicitionalyに変換
            dataList = ComUtil.ConvertClassToDictionary<Dao.excelPortPartsList>(results);

            if (dataList == null || dataList.Count == 0)
            {
                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「該当データがありません。」
                resultMsg = GetResMessage(ComRes.ID.ID941060001);
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
    }
}