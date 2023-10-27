using CommonExcelUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonSTDUtil.CommonDBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using System.Dynamic;
using static CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;

using ComConsts = CommonSTDUtil.CommonConstants;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComRes = CommonSTDUtil.CommonResources;
using ExcelUtil = CommonExcelUtil.CommonExcelUtil;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
using Dao = CommonTMQUtil.CommonTMQUtilDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using System.IO;

namespace CommonTMQUtil
{
    /// <summary>
    /// TMQ用共通ユーティリティクラス
    /// </summary>
    public static partial class CommonTMQUtil
    {
        #region ExcelPort共通処理
        /// <summary>
        /// ダウンロード条件クラス
        /// </summary>
        public class ExcelPortDownloadCondition : ComDao.SearchCommonClass
        {
            /// <summary>機能ID(隠し項目)</summary>
            public string HideConductId { get; set; }
            /// <summary>シート番号(隠し項目)</summary>
            public int HideSheetNo { get; set; }
            /// <summary>機能ID</summary>
            public string ConductId { get { return this.HideConductId; } }
            /// <summary>シート番号</summary>
            public int SheetNo { get { return this.HideSheetNo; } }
            /// <summary>追加条件区分</summary>
            public int? AddCondition { get; set; }
            /// <summary>メンテナンス対象</summary>
            public string MaintenanceTarget { get; set; }
            /// <summary>工場ID</summary>
            public int? FactoryId { get; set; }
            /// <summary>工場単一選択必須区分</summary>
            public int? FactorySingleSelectionDivision { get; set; }
            /// <summary>発生日(From)</summary>
            public DateTime OccurrenceDateFrom { get; set; }
            /// <summary>発生日(To)</summary>
            public DateTime OccurrenceDateTo { get; set; }
            /// <summary>着工予定日(From)</summary>
            public DateTime ExpectedConstructionDateFrom { get; set; }
            /// <summary>着工予定日(To)</summary>
            public DateTime ExpectedConstructionDateTo { get; set; }
            /// <summary>完了日(From)</summary>
            public DateTime CompletionDateFrom { get; set; }
            /// <summary>完了日(To)</summary>
            public DateTime CompletionDateTo { get; set; }
            /// <summary>完了区分</summary>
            public int CompletionDivision { get; set; }
        }

        /// <summary>
        /// ExcelPort共通
        /// </summary>
        public class ComExcelPort
        {
            #region 定数
            /// <summary>条件コントロールグループIDフォーマット</summary>
            public const string ConditionContorlGroupIdFmt = "BODY_000_00_LST_{0}";

            /// <summary>
            /// プログラムID
            /// </summary>
            public static class ProgramId
            {
                /// <summary>プログラムID：ExcelPortダウンロード</summary>
                public const string Download = "EP0001";
                /// <summary>プログラムID：ExcelPortアップロード</summary>
                public const string Upload = "EP0002";
            }

            /// <summary>
            /// ExcelPort用テンプレートファイル
            /// </summary>
            public static class Template
            {
                /// <summary>工場ID：ExcelPort用テンプレートファイル</summary>
                public const int FactoryId = 0;
                /// <summary>帳票ID：ExcelPort用テンプレートファイル</summary>
                public const string ReportId = "RP1000";
                /// <summary>テンプレートID：ExcelPort用テンプレートファイル</summary>
                public const int TemplateId = 1;
                /// <summary>出力パターンID：ExcelPort用テンプレートファイル</summary>
                public const int PatternId = 1;
                /// <summary>出力時シート番号：ExcelPort用テンプレートファイル</summary>
                public const int OutputSheetNo = 1;
            }

            /// <summary>
            /// シート番号
            /// </summary>
            public static class SheetNo
            {
                /// <summary>シート番号：エラー情報シート</summary>
                public const int ErrorInfo = 9;
                /// <summary>シート番号：レイアウト定義情報シート</summary>
                public const int DefineInfo = 10;
                /// <summary>シート番号：アイテム情報シート</summary>
                public const int ItemInfo = 11;
                /// <summary>シート番号：翻訳情報シート</summary>
                public const int TranslationInfo = 12;
            }

            /// <summary>
            /// シート名
            /// </summary>
            public static class SheetName
            {
                /// <summary>シート番号：エラー情報シート</summary>
                public const string ErrorInfo = "Sheet_Error";
                /// <summary>シート番号：レイアウト定義情報シート</summary>
                public const string DefineInfo = "Sheet_Define";
                /// <summary>シート番号：アイテム情報シート</summary>
                public const string ItemInfo = "Sheet_Item";
                /// <summary>シート番号：翻訳情報シート</summary>
                public const string TranslationInfo = "Sheet_Message";
            }

            /// <summary>
            /// セル種類
            /// </summary>
            public static class CellType
            {
                /// <summary>セル種類：文字列</summary>
                public const int Text = 1;
                /// <summary>セル種類：数値</summary>
                public const int Numeric = 2;
                /// <summary>セル種類：日付</summary>
                public const int Date = 3;
                /// <summary>セル種類：時刻</summary>
                public const int Time = 4;
                /// <summary>セル種類：コンボボックス</summary>
                public const int ComboBox = 5;
                /// <summary>セル種類：複数選択リストボックス</summary>
                public const int MultiListBox = 6;
                /// <summary>セル種類：チェックボックス</summary>
                public const int CheckBox = 7;
                /// <summary>セル種類：画面選択</summary>
                public const int FormSelect = 8;
            }

            /// <summary>
            /// SQL名
            /// </summary>
            public static class SqlName
            {
                /// <summary>ExcelPort用SQL格納先サブディレクトリ名</summary>
                public const string SubDirName = @"ExcelPort";
                /// <summary>SQL名：対象構成ID上下全階層登録用一時テーブル生成</summary>
                public const string CreateTempStructureAll = "Create_TempStructureAll";
                /// <summary>SQL名：対象構成ID上下全階層登録</summary>
                public const string InsertStructureList = "Insert_StructureList";

                /// <summary>コンボボックスデータ取得用SQL格納先サブディレクトリ名</summary>
                public const string SubDirNameForCombo = @"Common\ExcelPort";

                /// <summary>マッピング情報取得用SQL格納先サブディレクトリ名</summary>
                public const string SubDirMapping = "Common";
                /// <summary>SQL名：マッピング情報一覧取得</summary>
                public const string GetMappingInfoList = "MappingInfo_GetList";
            }

            /// <summary>
            /// DBカラム名
            /// </summary>
            public static class ColName
            {
                /// <summary>カラム名：列種類</summary>
                public const string ColumnType = "column_type";
                /// <summary>カラム名：選択列グループID</summary>
                public const string GrpId = "ep_select_group_id";
                /// <summary>カラム名：選択列関連情報ID</summary>
                public const string RelationId = "ep_relation_id";
                /// <summary>カラム名：選択列関連情報パラメータ</summary>
                public const string RelationParam = "ep_relation_parameters";
            }

            /// <summary>
            /// 条件データ項目名
            /// </summary>
            public static class ConditionValName
            {
                /// <summary>項目名：場所階層IDリスト</summary>
                public const string LocationIdList = "locationIdList";
                /// <summary>項目名：職種IDリスト</summary>
                public const string JobIdList = "jobIdList";
            }
            #endregion

            #region コンストラクタ
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="db"DB接続></param>
            /// <param name="userId">ユーザID</param>
            /// <param name="belongingInfo">所属情報</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="condition">実行条件</param>
            /// <param name="msgRes">メッセージリソース</param>
            public ComExcelPort(
                CommonDBManager db,
                string userId,
                BelongingInfo belongingInfo,
                string languageId,
                int formNo,
                List<Dictionary<string, object>> condition,
                ComUtil.MessageResources msgRes)
            {
                this.db = db;
                this.userId = userId;
                this.belongingInfo = belongingInfo;
                this.languageId = languageId;
                this.formNo = formNo;
                this.condition = condition;
                this.msgResources = msgRes;

                this.TargetLocationInfoList = new List<StructureInfo>();
                this.TargetJobInfoList = new List<StructureInfo>();
                this.TargetFactoryIdList = new List<int>();
            }
            #endregion

            #region privateメンバ変数
            /// <summary>DB接続</summary>
            CommonDBManager db;
            /// <summary>場所階層条件リスト</summary>
            private List<int> locationIdList;
            /// <summary>職種条件条件リスト</summary>
            private List<int> jobIdList;
            /// <summary>シート定義情報リスト</summary>
            private List<ReportDao.MsOutputReportSheetDefineEntity> sheetDefineList;
            /// <summary>マッピング情報リスト</summary>
            private List<CommonExcelPrtInfo> mappingInfoList;
            /// <summary>コマンド情報リスト</summary>
            private List<CommonExcelCmdInfo> cmdInfoList;
            /// <summary>テンプレート情報</summary>
            private ReportDao.MsOutputTemplateEntity templateInfo;
            /// <summary>ユーザID</summary>
            private string userId;
            /// <summary>所属情報</summary>
            private BelongingInfo belongingInfo;
            /// <summary>言語情報</summary>
            private string languageId;
            /// <summary>画面番号</summary>
            private int formNo;
            /// <summary>条件辞書</summary>
            private List<Dictionary<string, object>> condition;
            /// <summary>メッセージリソース</summary>
            private ComUtil.MessageResources msgResources;
            #endregion

            #region プロパティ
            /// <summary>ダウンロード条件コントロールグループID</summary>
            public string ConditionControlGroupId { get { return string.Format(ConditionContorlGroupIdFmt, formNo); } }
            /// <summary>ダウンロード条件</summary>
            public ExcelPortDownloadCondition DownloadCondition { get; set; }
            /// <summary>対象場所階層情報リスト</summary>
            public List<StructureInfo> TargetLocationInfoList { get; set; }
            /// <summary>対象職種情報リスト</summary>
            public List<StructureInfo> TargetJobInfoList { get; set; }
            /// <summary>対象工場IDリスト</summary>
            public List<int> TargetFactoryIdList { get; set; }
            #endregion

            #region publicメソッド
            /// <summary>
            /// DBマッピング情報リストの取得
            /// </summary>
            /// <param name="ctrlId">コントロールID</param>
            /// <returns></returns>
            public List<ComUtil.DBMappingInfo> GetDBMappingList()
            {
                var mappingList = this.db.GetListByOutsideSql<ComUtil.DBMappingInfo>(
                    SqlName.GetMappingInfoList, SqlName.SubDirMapping, new { PgmId = ProgramId.Download, LanguageId = this.languageId }).ToList();

                return mappingList;
            }

            /// <summary>
            /// ExcelPort用テンプレートファイル情報初期化
            /// </summary>
            /// <param name="msg">詳細メッセージ</param>
            /// <returns></returns>
            public bool InitializeExcelPortTemplateFile(out string resultMsg, out string detailMsg)
            {
                //==========
                // 初期化
                //==========
                resultMsg = string.Empty;
                detailMsg = string.Empty;

                // ExcelPort用マッピング情報の取得
                var mapInfoList = GetDBMappingList();

                // ダウンロード条件の取得
                string ctrlGrpId = this.ConditionControlGroupId;
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.condition, ctrlGrpId);
                ExcelPortDownloadCondition dlCondition = new();
                ComUtil.SetConditionByDataClass(ctrlGrpId, mapInfoList, dlCondition, targetDic, ComUtil.ConvertType.Result);
                this.DownloadCondition = dlCondition;

                this.locationIdList = getConditionList<int>(condition, ConditionValName.LocationIdList);
                this.jobIdList = getConditionList<int>(condition, ConditionValName.JobIdList);

                // 出力帳票シート定義情報
                this.sheetDefineList = new List<ReportDao.MsOutputReportSheetDefineEntity>();
                // マッピング情報
                this.mappingInfoList = new List<CommonExcelPrtInfo>();
                // コマンド情報
                // セルの結合や罫線を引く等のコマンド実行が必要な場合はここでセットする。不要な場合はnullでOK
                this.cmdInfoList = new List<CommonExcelCmdInfo>();

                // 場所階層条件から対象工場、対象場所階層情報を取得
                HistoryManagement history = new HistoryManagement(
                    this.db, this.userId, this.languageId, DateTime.Now, CommonTMQConstants.MsStructure.StructureId.ApplicationConduct.None);
                if (this.locationIdList.Count == 0)
                {
                    // 場所階層条件が未指定の場合、所属情報から取得
                    this.TargetFactoryIdList.AddRange(this.belongingInfo.BelongingFactoryIdList);
                    this.TargetLocationInfoList.AddRange(this.belongingInfo.LocationInfoList);
                }
                else
                {
                    foreach (var id in this.locationIdList)
                    {
                        var tmpId = history.getFactoryId(id);
                        if (!this.TargetFactoryIdList.Contains(tmpId))
                        {
                            this.TargetFactoryIdList.Add(tmpId);
                        }
                        this.TargetLocationInfoList.Add(new StructureInfo() { FactoryId = tmpId, StructureId = id });
                    }
                }
                // 職種条件から対象職種情報を取得
                if (this.jobIdList.Count == 0)
                {
                    // 職種条件が未指定の場合、所属情報から取得
                    this.TargetJobInfoList.AddRange(this.belongingInfo.JobInfoList);
                }
                else
                {
                    foreach (var id in this.jobIdList)
                    {
                        var tmpId = history.getFactoryId(id);
                        this.TargetJobInfoList.Add(new StructureInfo() { FactoryId = tmpId, StructureId = id });
                    }
                }

                // ExcelPort対象工場の取得
                var excelPortFactoryList = getExcelPortTargetFactoryIdList();
                var excludedFactoryIdList = new List<int>();
                if (excelPortFactoryList.Count == 0)
                {
                    // ExcelPort利用可能工場が存在しない場合
                    // 「ExcelPort利用可能工場以外の工場データは出力できません。」
                    resultMsg = GetResMessage(ComRes.ID.ID141040003, this.languageId, this.msgResources);
                    return false;
                }
                else
                {
                    var contains = false;
                    for (int i = this.TargetFactoryIdList.Count - 1; i >= 0; i--)
                    {
                        var id = this.TargetFactoryIdList[i];
                        if (!excelPortFactoryList.Contains(id))
                        {
                            // ExcelPort利用可能工場以外の場合、対象外
                            excludedFactoryIdList.Add(id);  // 除外工場
                            TargetFactoryIdList.RemoveAt(i);
                            contains = true;
                        }
                    }
                    if (TargetFactoryIdList.Count == 0)
                    {
                        // 対象工場にExcelPort利用可能工場が存在しない場合
                        // 「ExcelPort利用可能工場以外の工場データは出力できません。」
                        resultMsg = GetResMessage(ComRes.ID.ID141040003, this.languageId, this.msgResources);
                        return false;
                    }
                    if (contains)
                    {
                        // ExcelPort利用可能工場以外の工場が含まれる場合
                        // 「ダウンロードされたExcelファイルにExcelPort利用可能工場以外の工場データは出力されていません。」
                        resultMsg = GetResMessage(ComRes.ID.ID141160018, this.languageId, this.msgResources);
                    }
                }

                // 変更管理対象工場の取得
                var approvalList = history.GetUserApprovalFactoryList();
                var excludedApprovalCnt = 0;
                if (approvalList.Count > 0)
                {
                    var contains = false;
                    for (int i = this.TargetFactoryIdList.Count - 1; i >= 0; i--)
                    {
                        if (approvalList.Contains(this.TargetFactoryIdList[i]))
                        {
                            // 変更管理対象工場の場合、対象外
                            this.TargetFactoryIdList.RemoveAt(i);
                            contains = true;
                        }
                    }
                    foreach (var id in excludedFactoryIdList)
                    {
                        if (approvalList.Contains(id))
                        {
                            // ExcelPort利用可能工場以外の工場に変更管理対象工場が含まれる場合
                            excludedApprovalCnt++;
                            contains = true;
                        }
                    }
                    if (this.TargetFactoryIdList.Count == 0)
                    {
                        // 対象工場が変更管理対象工場のみの場合
                        // 「変更履歴管理対象の工場データは出力できません。」
                        resultMsg = GetResMessage(ComRes.ID.ID141290001, this.languageId, this.msgResources);
                        return false;
                    }
                    else
                    {
                        if (contains)
                        {
                            if (excludedApprovalCnt == excludedFactoryIdList.Count)
                            {
                                // 対象外の工場が変更管理対象工場のみの場合
                                // 「ダウンロードされたExcelファイルに変更履歴管理対象の工場データは出力されていません。」
                                resultMsg = GetResMessage(ComRes.ID.ID141160017, this.languageId, this.msgResources);
                            }
                            else
                            {
                                // 対象工場にExcelPort利用工場以外の工場と変更管理対象工場の両方が含まれる場合
                                // 「ダウンロードされたExcelファイルにExcelPort利用可能工場以外の工場データおよび変更履歴管理対象の工場データは出力されていません。」
                                resultMsg = GetResMessage(ComRes.ID.ID141160019, this.languageId, this.msgResources);
                            }
                        }
                    }
                }
                // 対象工場で対象場所階層情報と対象職種情報を絞り込む
                this.TargetLocationInfoList = this.TargetLocationInfoList.Where(x => this.TargetFactoryIdList.Contains(x.FactoryId)).ToList();
                this.TargetJobInfoList = this.TargetJobInfoList.Where(x => this.TargetFactoryIdList.Contains(x.FactoryId)).ToList();

               // 場所階層、職種機種絞り込み用一時テーブルを生成
                if (!createTempTableForTargetStructureInfo())
                {
                    detailMsg = "Failed to create the temporary table.";
                    return false;
                }
                return true;
            }

            /// <summary>
            /// ExcelPortテンプレートファイル出力処理
            /// </summary>
            /// <param name="db">DB接続</param>
            /// <param name="sheetNo">対象シート番号</param>
            /// <param name="dataList">出力データ</param>
            /// <param name="fileType">ファイル種類</param>
            /// <param name="fileName">ファイル名</param>
            /// <param name="memoryStream">出力ストリーム</param>
            /// <param name="detailMsg">詳細メッセージ</param>
            /// <returns></returns>
            public bool OutputExcelPortTemplateFile(
                IList<Dictionary<string, object>> dataList,
                out string fileType,
                out string fileName,
                out MemoryStream memoryStream,
                out string detailMsg)
            {
                //==========
                // 初期化
                //==========
                int factoryId = Template.FactoryId;
                string reportId = Template.ReportId;
                int templateId = Template.TemplateId;
                int patternId = Template.PatternId;
                int sheetNo = this.DownloadCondition.SheetNo;

                // ファイルタイプ
                fileType = ComConsts.REPORT.FILETYPE.EXCEL_MACRO;
                // ダウンロードファイル名
                fileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", reportId, DateTime.Now) + ComConsts.REPORT.EXTENSION.EXCEL_MACRO_BOOK;
                // メモリストリーム
                memoryStream = new MemoryStream();
                // メッセージ
                detailMsg = "";
                // シートデータ件数
                int sheetDataCount = 0;

                // 対象機能IDとシート番号をセット
                Option option = new Option();
                option.TargetConductId = this.DownloadCondition.ConductId;
                option.TargetSheetNo = this.DownloadCondition.SheetNo;

                // ExcelPortバージョンを取得
                var versionEntity = TMQUtil.SqlExecuteClass.SelectEntity<AutoCompleteEntity>(ComReport.GetExcelPortVersion, ExcelPath, null, db);
                if (versionEntity != null)
                {
                    option.Version = Convert.ToDecimal(versionEntity.Exparam1);
                }

                // テンプレート情報を取得
                this.templateInfo = new ReportDao.MsOutputTemplateEntity().GetEntity(factoryId, reportId, templateId, db);
                if (this.templateInfo == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Template information does not exist. [ReportId:{0}]", reportId);
                    return false;
                }

                // 出力帳票シート定義のリストを取得
                this.sheetDefineList = TMQUtil.SqlExecuteClass.SelectList<ReportDao.MsOutputReportSheetDefineEntity>(
                    ComReport.GetReportSheetDefine,
                    ExcelPath,
                    new { FactoryId = factoryId, ReportId = reportId },
                    db);
                if (sheetDefineList == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Sheet definition information does not exist. [ReportId:{0}]", reportId);
                    return false;
                }

                // レイアウト定義情報のシート定義を取得
                var sheetDefine = this.sheetDefineList.Where(x => x.SheetNo == SheetNo.DefineInfo).FirstOrDefault();
                if (sheetDefine == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Layout definition sheet information does not exist. [SheetNo:{0}]", SheetNo.DefineInfo);
                    return false;
                }

                // 対象SQLファイルにてSQLを実行し、該当シート出力用データを取得する
                string targetSql = sheetDefine.TargetSql;
                var keyList = new List<SelectKeyData>();
                keyList.Add(new SelectKeyData() { Key1 = this.DownloadCondition.SheetNo, Key2 = Template.OutputSheetNo });
                var tmpDataList = GetReportData(keyList, targetSql, db, userId, languageId, null);
                // マッピングデータ作成
                List<CommonExcelPrtInfo> mappingDataList = CreateMappingList(
                                                            factoryId,
                                                            ProgramId.Download,
                                                            reportId,
                                                            sheetDefine.SheetNo,
                                                            templateId,
                                                            patternId,
                                                            tmpDataList,
                                                            templateInfo.TemplateFileName,
                                                            templateInfo.TemplateFilePath,
                                                            languageId,
                                                            db,
                                                            out int optionRowCount,
                                                            out int optionColomnCount,
                                                            option);
                // マッピング情報リストに追加
                this.mappingInfoList.AddRange(mappingDataList);
                // 非表示シートのため、罫線は不要。必要な場合は以下のコメントを外す
                //// 一覧罫線用にデータ件数を退避
                //sheetDataCount = dataList.Count;
                //// 一覧フラグの場合
                //if (sheetDefine.ListFlg == true)
                //{
                //    // 罫線設定セル範囲を取得
                //    string range = GetTargetCellRange(factoryId, reportId, templateId, patternId, sheetDefine.SheetNo, sheetDataCount, sheetDefine.RecordCount, db, optionRowCount, optionColomnCount);
                //    if (range != null)
                //    {
                //        //範囲が取得できた場合、罫線を引く
                //        var sheetName = getDefaultSheetName(sheetDefine.SheetNo);
                //        cmdInfoList.AddRange(CommandLineBox(range, sheetDefine.SheetName));
                //    }
                //}

                // アイテム定義情報のシート定義を取得
                sheetDefine = sheetDefineList.Where(x => x.SheetNo == SheetNo.ItemInfo).FirstOrDefault();
                if (sheetDefine == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Item definition sheet information does not exist. [SheetNo:{0}]", SheetNo.ItemInfo);
                    return false;
                }

                // レイアウト定義情報からアイテム定義取得用データを抽出する
                var selectItemList = tmpDataList.Where(x =>
                    (int)((IDictionary<string, object>)x)[ColName.ColumnType] == CellType.ComboBox ||
                    (int)((IDictionary<string, object>)x)[ColName.ColumnType] == CellType.MultiListBox ||
                    (int)((IDictionary<string, object>)x)[ColName.ColumnType] == CellType.FormSelect).ToList();
                var itemDataList = new List<Dictionary<string, object>>();
                if (selectItemList.Count() > 0)
                {
                    var factoryIdList = new List<int>();
                    factoryIdList.AddRange(this.TargetFactoryIdList);
                    //システム共通の階層も併せて取得する
                    if (!factoryIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
                    {
                        factoryIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
                    }
                    foreach (var selectItem in selectItemList)
                    {
                        var dic = (IDictionary<string, object>)selectItem;
                        string grpId = dic[ColName.GrpId].ToString();
                        string relationId = dic[ColName.RelationId].ToString();
                        string relationParam = dic[ColName.RelationParam].ToString();
                        // コンボ選択アイテムデータの取得
                        var resultList = getComboBoxData(grpId, relationId, relationParam, factoryIdList);
                        if (resultList.Count > 0)
                        {
                            itemDataList.AddRange(resultList);
                        }
                    }
                    // マッピングデータ作成
                    mappingDataList = CreateMappingList(
                                        factoryId,
                                        ProgramId.Download,
                                        reportId,
                                        sheetDefine.SheetNo,
                                        templateId,
                                        patternId,
                                        itemDataList,
                                        templateInfo.TemplateFileName,
                                        templateInfo.TemplateFilePath,
                                        languageId,
                                        db,
                                        out optionRowCount,
                                        out optionColomnCount);
                    // マッピング情報リストに追加
                    mappingInfoList.AddRange(mappingDataList);

                    // 非表示シートのため、罫線は不要。必要な場合は以下のコメントを外す
                    //// 一覧罫線用にデータ件数を退避
                    //sheetDataCount = itemDataList.Count;
                    //// 一覧フラグの場合
                    //if (sheetDefine.ListFlg == true)
                    //{
                    //    // 罫線設定セル範囲を取得
                    //    string range = GetTargetCellRange(factoryId, reportId, templateId, patternId, sheetDefine.SheetNo, sheetDataCount, sheetDefine.RecordCount, db, optionRowCount, optionColomnCount);
                    //    if (range != null)
                    //    {
                    //        //範囲が取得できた場合、罫線を引く
                    //        var sheetName = getDefaultSheetName(sheetDefine.SheetNo);
                    //        cmdInfoList.AddRange(CommandLineBox(range, sheetDefine.SheetName));
                    //    }
                    //}
                }

                // 翻訳定義情報のシート定義を取得
                sheetDefine = sheetDefineList.Where(x => x.SheetNo == SheetNo.TranslationInfo).FirstOrDefault();
                if (sheetDefine == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Translation definition sheet information does not exist. [SheetNo:{0}]", SheetNo.TranslationInfo);
                    return false;
                }

                // 対象SQLファイルにてSQLを実行し、該当シート出力用データを取得する
                targetSql = sheetDefine.TargetSql;
                tmpDataList = GetReportData(new List<SelectKeyData>(), targetSql, db, userId, languageId, null);
                mappingDataList = CreateMappingList(
                                    factoryId,
                                    ProgramId.Download,
                                    reportId,
                                    sheetDefine.SheetNo,
                                    templateId,
                                    patternId,
                                    tmpDataList,
                                    templateInfo.TemplateFileName,
                                    templateInfo.TemplateFilePath,
                                    languageId,
                                    db,
                                    out optionRowCount,
                                    out optionColomnCount);
                // マッピング情報リストに追加
                mappingInfoList.AddRange(mappingDataList);

                // 非表示シートのため、罫線は不要。必要な場合は以下のコメントを外す
                //// 一覧罫線用にデータ件数を退避
                //sheetDataCount = dataList.Count;
                //// 一覧フラグの場合
                //if (sheetDefine.ListFlg == true)
                //{
                //    // 罫線設定セル範囲を取得
                //    string range = GetTargetCellRange(factoryId, reportId, templateId, patternId, sheetDefine.SheetNo, sheetDataCount, sheetDefine.RecordCount, db, optionRowCount, optionColomnCount);
                //    if (range != null)
                //    {
                //        //範囲が取得できた場合、罫線を引く
                //        var sheetName = getDefaultSheetName(sheetDefine.SheetNo);
                //        cmdInfoList.AddRange(CommandLineBox(range, sheetDefine.SheetName));
                //    }
                //}

                // 機能個別シートのレイアウト定義情報のシート定義を取得
                sheetDefine = this.sheetDefineList.Where(x => x.SheetNo == sheetNo).FirstOrDefault();
                if (sheetDefine == null)
                {
                    // 取得できない場合、処理を戻す
                    detailMsg = string.Format("Sheet definition information does not exist. [SheetNo:{0}]", sheetNo);
                    return false;
                }

                if (dataList == null && !string.IsNullOrEmpty(sheetDefine.TargetSql))
                {
                    // 指定データがnullで対象SQLが設定されている場合、
                    // 対象SQLファイルにてSQLを実行し、該当シート出力用データを取得する
                    targetSql = sheetDefine.TargetSql;
                    dataList = (IList<Dictionary<string, object>>)GetReportData(new List<SelectKeyData>(), targetSql, db, userId, languageId, null);
                }

                // 場所階層と職種情報を設定する
                if (dataList != null)
                {
                    // 階層情報の取得
                    setStructureLayerInfo(dataList);
                }
                else
                {
                    dataList = new List<Dictionary<string, object>>();
                }
                // マッピングデータ作成
                mappingDataList = CreateMappingList(
                                                            factoryId,
                                                            ProgramId.Download,
                                                            reportId,
                                                            sheetDefine.SheetNo,
                                                            templateId,
                                                            patternId,
                                                            dataList,
                                                            templateInfo.TemplateFileName,
                                                            templateInfo.TemplateFilePath,
                                                            languageId,
                                                            db,
                                                            out optionRowCount,
                                                            out optionColomnCount);
                // マッピング情報リストに追加
                this.mappingInfoList.AddRange(mappingDataList);

                // 一覧罫線用にデータ件数を退避
                sheetDataCount = dataList.Count;
                var sheetName = getDefaultSheetName(sheetNo);
                // 一覧フラグの場合
                if (sheetDefine.ListFlg == true)
                {
                    // 罫線設定セル範囲を取得
                    string range = GetTargetCellRange(factoryId, reportId, templateId, patternId, sheetDefine.SheetNo, sheetDataCount, sheetDefine.RecordCount, db, optionRowCount, optionColomnCount);
                    if (range != null)
                    {
                        //範囲が取得できた場合、罫線を引く
                        cmdInfoList.AddRange(CommandLineBox(range, sheetName));
                    }
                }

                // シート名変更コマンドを追加
                CommonExcelCmdInfo cmdInfo = new CommonExcelCmdInfo();
                var newSheetName = GetSheetName(sheetDefine, factoryId, this.languageId, db);
                string[] param = new string[] { sheetName, newSheetName };  // シート名、変更後シート名
                cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdChangeSheetName, param);
                this.cmdInfoList.Add(cmdInfo);

                // 不要シートの削除＆作業用シートの非表示
                deleteOrHideUnnecessarySheets(sheetNo, factoryId);

                // エクセルファイル作成
                ExcelUtil.CreateExcelFile(templateInfo.TemplateFileName, templateInfo.TemplateFilePath, userId, mappingInfoList, cmdInfoList, ref memoryStream, ref detailMsg);
                return true;
            }
            #endregion

            #region privateメソッド
            /// <summary>
            /// 条件文字列の取得
            /// </summary>
            /// <param name="condition">出力条件</param>
            /// <param name="valName">条件名</param>
            /// <returns></returns>
            private string getConditionString(List<Dictionary<string, object>> condition, string valName)
            {
                var val = condition.Where(x => x.ContainsKey(valName)).Select(y => y[valName]).FirstOrDefault();
                return val.ToString();
            }

            /// <summary>
            /// 条件文字列の取得
            /// </summary>
            /// <param name="condition">出力条件</param>
            /// <param name="valName">条件名</param>
            /// <returns></returns>
            private List<T> getConditionList<T>(List<Dictionary<string, object>> condition, string valName)
            {
                var val = condition.Where(x => x.ContainsKey(valName)).Select(y => y[valName]).FirstOrDefault();
                return val as List<T>;
            }

            /// <summary>
            /// ExcelPort対象工場を取得
            /// </summary>
            /// <returns>ExcelPort対象工場のリスト</returns>
            private List<int> getExcelPortTargetFactoryIdList()
            {
                var factoryIdList = new List<int>();
                // 拡張項目5の値がセットされた工場のリストを取得
                // 検索条件
                StructureItemEx.StructureItemExInfo param = new();
                param.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.Location; // 場所階層
                param.Seq = 5; // 拡張5が"1"のデータ
                param.ExData = "1";
                param.LanguageId = this.languageId;
                List<StructureItemEx.StructureItemExInfo> locationList = StructureItemEx.GetStructureItemExData(param, this.db);
                if (locationList == null || locationList.Count == 0)
                {
                    // 無い場合終了
                    return factoryIdList;
                }

                // 有る場合は階層番号が工場のものを絞り込み
                factoryIdList = locationList.Where(x => x.StructureLayerNo == (int)TMQConsts.MsStructure.StructureLayerNo.Location.Factory).Select(x => x.StructureId).ToList();

                return factoryIdList;
            }

            /// <summary>
            /// 対象場所階層/職種情報(上下階層)を一時テーブルへ登録
            /// </summary>
            /// <returns></returns>
            private bool createTempTableForTargetStructureInfo()
            {
                // 一時テーブル生成用のSQL文字列を取得
                if (!ComUtil.GetFixedSqlStatement(SqlName.SubDirName, SqlName.CreateTempStructureAll, out string createSql))
                {
                    return false;
                }

                // 一時テーブル登録用のSQL文字列を取得
                if (!ComUtil.GetFixedSqlStatement(SqlName.SubDirName, SqlName.InsertStructureList, out string insertSql))
                {
                    return false;
                }

                // 対象の構成IDをカンマ区切りの文字列にする
                string structureIdList = string.Join(',', this.TargetLocationInfoList.Select(x=>x.StructureId));
                structureIdList += ',' + string.Join(',', this.TargetJobInfoList.Select(x => x.StructureId));

                // 一時テーブルを生成
                this.db.Regist(createSql);
                // 一時テーブルへ構成IDを登録
                this.db.Regist(insertSql, new { StructureIdList = structureIdList, LanguageId = this.languageId });
                return true;
            }

            /// <summary>
            /// コンボボックス用SQL実行
            /// </summary>
            /// <param name="conditionDictionary"></param>
            /// <param name="rootPath"></param>
            /// <param name="list"></param>
            /// <param name="languageId"></param>
            /// <returns></returns>
            private List<Dictionary<string, object>> getComboBoxData(string grpId, string sqlId, string sqlParam, List<int> factoryIdList)
            {
                var resultList = new List<Dictionary<string, object>>();
                var condition = new ExpandoObject();
                int index = 1;
                string param = "";

                // パラメータを設定する
                if (!string.IsNullOrEmpty(sqlParam))
                {
                    string[] paramList = sqlParam.Split(','); // カンマ区切りで配列に挿入

                    for (int i = 0; i < paramList.Count(); i++)
                    {
                        param = "param" + index.ToString();
                        ((IDictionary<string, object>)condition).Add(param, paramList[i]);
                        index++;
                    }
                }

                // SQL格納フォルダ
                string sqlDir = SqlName.SubDirNameForCombo;

                // 工場IDリストの設定
                ((IDictionary<string, object>)condition).Add("factoryIdList", factoryIdList);

                // 言語コードを設定
                ((IDictionary<string, object>)condition).Add("languageId", this.languageId);

                // 検索実行
                var results = this.db.GetListByOutsideSql(sqlId, sqlDir, condition);
                if (results == null || results.Count == 0)
                {
                    return resultList;
                }
                // 検索結果を結果リストへ登録する
                foreach (var result in results)
                {
                    var item = (IDictionary<string, object>)result;
                    var dic = new Dictionary<string, object>();
                    dic.Add(ColName.GrpId, grpId);
                    foreach (var key in item.Keys)
                    {
                        object value = item[key];
                        if (CommonUtil.IsNullOrEmpty(value))
                        {
                            // 値が空の場合
                            if (key.ToUpper().StartsWith("EXPARAM"))
                            {
                                // 拡張項目の場合は結果リストに格納しない
                                continue;
                            }
                            else
                            {
                                // 拡張項目以外の場合は空文字をセットして返す
                                value = "";
                            }
                        }
                        dic.Add(key, value);
                    }
                    resultList.Add(dic);
                }
                return resultList;
            }

            /// <summary>
            /// シート名の初期値を取得
            /// </summary>
            /// <param name="sheetNo">シート番号</param>
            /// <returns></returns>
            private string getDefaultSheetName(int sheetNo)
            {
                string sheetName = string.Empty;

                switch (sheetNo)
                {
                    case SheetNo.ErrorInfo:
                        sheetName = SheetName.ErrorInfo;
                        break;
                    case SheetNo.DefineInfo:
                        sheetName = SheetName.DefineInfo;
                        break;
                    case SheetNo.ItemInfo:
                        sheetName = SheetName.ItemInfo;
                        break;
                    case SheetNo.TranslationInfo:
                        sheetName = SheetName.TranslationInfo;
                        break;
                    default:
                        sheetName = string.Format("Sheet{0}", sheetNo);
                        break;
                }
                return sheetName;
            }

            /// <summary>
            /// 階層情報の設定
            /// </summary>
            /// <param name="dataList">出力データリスト</param>
            private void setStructureLayerInfo(IList<Dictionary<string, object>> dataList)
            {
                foreach (var data in dataList)
                {
                    // 機能場所階層IDと職種機種階層IDから上位の階層を設定

                    List<StructureLayerInfo.StructureType> typeLst = new List<StructureLayerInfo.StructureType>();
                    typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
                    IList<StructureLocationInfoForReport> locationInfoList = new List<StructureLocationInfoForReport>();
                    StructureLocationInfoForReport locationInfo = new StructureLocationInfoForReport();

                    var rowDic = (IDictionary<string, object>)data;
                    if (rowDic.ContainsKey(ColumnName.LocationStructureId))
                    {
                        var val = rowDic[ColumnName.LocationStructureId];
                        if (!ComUtil.IsNullOrEmpty(val))
                        {
                            locationInfo.LocationStructureId = Convert.ToInt32(val);
                            locationInfoList.Add(locationInfo);
                            StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureLocationInfoForReport>(ref locationInfoList, typeLst, db, languageId);
                            // 関連情報の設定
                            if (locationInfoList != null)
                            {
                                // 地区
                                rowDic[ColumnName.DistrictId] = locationInfoList[0].DistrictId;
                                rowDic[ColumnName.DistrictName] = locationInfoList[0].DistrictName;
                                // 工場
                                rowDic[ColumnName.FactoryId] = locationInfoList[0].FactoryId;
                                rowDic[ColumnName.FactoryName] = locationInfoList[0].FactoryName;
                                // プラント
                                rowDic[ColumnName.PlantId] = locationInfoList[0].PlantId;
                                rowDic[ColumnName.PlantName] = locationInfoList[0].PlantName;
                                // 系列
                                rowDic[ColumnName.SeriesId] = locationInfoList[0].SeriesId;
                                rowDic[ColumnName.SeriesName] = locationInfoList[0].SeriesName;
                                // 工程
                                rowDic[ColumnName.StrokeId] = locationInfoList[0].StrokeId;
                                rowDic[ColumnName.StrokeName] = locationInfoList[0].StrokeName;
                                // 設備
                                rowDic[ColumnName.FacilityId] = locationInfoList[0].FacilityId;
                                rowDic[ColumnName.FacilityName] = locationInfoList[0].FacilityName;
                            }
                        }
                    }

                    // 職種機種情報の設定
                    typeLst = new List<StructureLayerInfo.StructureType>();
                    typeLst.Add(StructureLayerInfo.StructureType.Job);

                    IList<StructureJobInfoForReport> jobInfoList = new List<StructureJobInfoForReport>();
                    StructureJobInfoForReport jobInfo = new StructureJobInfoForReport();

                    if (rowDic.ContainsKey(ColumnName.JobStructureId))
                    {
                        var val = rowDic[ColumnName.JobStructureId];
                        if (!ComUtil.IsNullOrEmpty(val))
                        {
                            jobInfo.JobStructureId = Convert.ToInt32(val);
                            jobInfoList.Add(jobInfo);
                            StructureLayerInfo.SetStructureLayerInfoToDataClass<StructureJobInfoForReport>(ref jobInfoList, typeLst, db, languageId);
                            // 関連情報の設定
                            if (jobInfoList != null)
                            {
                                // 職種
                                rowDic[ColumnName.JobId] = jobInfoList[0].JobId;
                                rowDic[ColumnName.JobName] = jobInfoList[0].JobName;
                                // 機種大分類
                                rowDic[ColumnName.LargeClassficationId] = jobInfoList[0].LargeClassficationId;
                                rowDic[ColumnName.LargeClassficationName] = jobInfoList[0].LargeClassficationName;
                                // 機種中分類
                                rowDic[ColumnName.MiddleClassficationId] = jobInfoList[0].MiddleClassficationId;
                                rowDic[ColumnName.MiddleClassficationName] = jobInfoList[0].MiddleClassficationName;
                                // 機種小分類
                                rowDic[ColumnName.SmallClassficationId] = jobInfoList[0].SmallClassficationId;
                                rowDic[ColumnName.SmallClassficationName] = jobInfoList[0].SmallClassficationName;

                                // 機種名称設定(RP0060)
                                if (!string.IsNullOrEmpty(jobInfoList[0].SmallClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].SmallClassficationName;
                                }
                                else if (!string.IsNullOrEmpty(jobInfoList[0].MiddleClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].MiddleClassficationName;
                                }
                                else if (!string.IsNullOrEmpty(jobInfoList[0].LargeClassficationName))
                                {
                                    rowDic[ColumnName.ModelName] = jobInfoList[0].LargeClassficationName;
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// 不要シートの削除＆作業用シートの非表示
            /// </summary>
            /// <param name="sheetNo">シート番号</param>
            /// <param name="factoryId">工場ID</param>
            private void deleteOrHideUnnecessarySheets(int sheetNo, int factoryId)
            {
                // 不要シートの削除＆作業用シートの非表示
                // シート定義情報リストから最大シート番号を取得
                var maxSheetNo = this.sheetDefineList.Max(x => x.SheetNo);
                for (int i = 1; i <= maxSheetNo; i++)
                {
                    if (i == sheetNo)
                    {
                        // 出力対象シートの場合スキップ
                        continue;
                    }

                    var sheetDefine = this.sheetDefineList.Where(x => x.SheetNo == i).FirstOrDefault();
                    var cmdInfo = new CommonExcelCmdInfo();
                    string sheetName;
                    string[] param;
                    if (sheetDefine == null)
                    {
                        // シート定義情報がない場合
                        // シート削除コマンドを追加
                        cmdInfo = new CommonExcelCmdInfo();
                        sheetName = string.Format("Sheet{0}", i);
                        param = new string[] { sheetName }; // シート名
                        cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdDeleteSheet, param);
                        this.cmdInfoList.Add(cmdInfo);
                        continue;
                    }

                    sheetName = getDefaultSheetName(sheetDefine.SheetNo);
                    var newSheetName = string.Empty;
                    cmdInfo = new CommonExcelCmdInfo();
                    switch (i)
                    {
                        case SheetNo.ErrorInfo:
                            // シート非表示コマンドを追加
                            param = new string[] { sheetName }; // シート名
                            cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdHiddenSheet, param);
                            this.cmdInfoList.Add(cmdInfo);
                            // シート名変更コマンドを追加
                            cmdInfo = new CommonExcelCmdInfo();
                            newSheetName = GetSheetName(sheetDefine, factoryId, this.languageId, db);
                            param = new string[] { sheetName, newSheetName };  // シート名、変更後シート名
                            cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdChangeSheetName, param);
                            this.cmdInfoList.Add(cmdInfo);
                            break;
                        case SheetNo.DefineInfo:
                        case SheetNo.ItemInfo:
                        case SheetNo.TranslationInfo:
                            // シート非表示コマンドを追加
                            param = new string[] { sheetName };    // シート名
                            cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdHiddenSheet, param);
                            this.cmdInfoList.Add(cmdInfo);
                            break;
                        default:
                            // シート削除コマンドを追加
                            sheetName = string.Format("Sheet{0}", i);
                            param = new string[] { sheetName }; // シート名
                            cmdInfo.SetExlCmdInfo(CommonExcelCmdInfo.CExecTmgAfter, CommonExcelCmdInfo.CExecCmdDeleteSheet, param);
                            this.cmdInfoList.Add(cmdInfo);
                            break;
                    }
                }
            }
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// ExcelPort用ファイル入出力管理クラス
    /// </summary>
    public class InoutDefineForExcelPort : Dao.InoutDefine
    {
        /// <summary>必須項目区分(ExcelPort用)</summary>
        public bool EpRequiredFlg { get; set; }

        /// <summary>関連情報ID(ExcelPort選択項目生成用)</summary>
        public string EpRelationId { get; set; }

        /// <summary>関連情報パラメータ(ExcelPort選択項目生成用)</summary>
        public string EpRelationParameters { get; set; }

        /// <summary>選択項目グループID(ExcelPort用)</summary>
        public string EpSelectGroupId { get; set; }

        /// <summary>選択項目ID格納先列番号(ExcelPort用)</summary>
        public int EpSelectIdColumnNo { get; set; }

        /// <summary>選択項目連動元列番号(ExcelPort用)</summary>
        public int EpSelectLinkColumnNo { get; set; }
    }
}
