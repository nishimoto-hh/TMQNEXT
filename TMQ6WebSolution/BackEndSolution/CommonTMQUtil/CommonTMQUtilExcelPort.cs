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
using ComBase = CommonSTDUtil.CommonDataBaseClass;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using ExcelUtil = CommonExcelUtil.CommonExcelUtil;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
using Dao = CommonTMQUtil.CommonTMQUtilDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using System.IO;
using Microsoft.AspNetCore.Http;

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

        public class ExcelPortUploadCondition
        {
            /// <summary>ExcelPortバージョン</summary>
            public decimal ExcelPortVersion { get; set; }
            /// <summary>機能ID</summary>
            public string ConductId { get; set; }
            /// <summary>シート番号</summary>
            public int SheetNo { get; set; }
        }

        /// <summary>
        /// ExcelPort共通
        /// </summary>
        public class ComExcelPort
        {
            #region 定数
            public static class ControlGroupId
            {
                /// <summary>コントロールグループID：条件フォーマット</summary>
                public const string ConditionFmt = "BODY_000_00_LST_{0}";
                /// <summary>コントロールグループID：アップロード</summary>
                public const string Upload = "LIST_000_1";
            }

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
                /// <summary>SQL名：ファイル取込項目定義情報取得用SQL</summary>
                public const string GetInputControlDefineForExcelPort = "GetInputControlDefineForExcelPort";
                /// <summary>SQL名：ExcelPortバージョン取得用SQL</summary>
                public const string GetExcelPortVersion = "GetExcelPortVersion";
                /// <summary>SQL名：ExcelPort対象情報取得用SQL</summary>
                public const string GetExcelPortTargetInfo = "GetExcelPortTargetInfo";

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

                /// <summary>項目名：対象機能ID</summary>
                public const string TargetConductId = "TargetConductId";
                /// <summary>項目名：対象シート番号</summary>
                public const string TargetSheetNo= "TargetSheetNo";
            }

            /// <summary>
            /// レイアウト定義シート情報
            /// </summary>
            public static class DefineSheetInfo
            {
                /// <summary>列番号：ヘッダー値</summary>
                public const int ColNoHeaderVal = 15;
                /// <summary>行番号：ExcelPortバージョン番号</summary>
                public const int RowNoVersion = 1;
                /// <summary>行番号：対象機能ID</summary>
                public const int RowNoConductId = 3;
                /// <summary>行番号：対象シート番号</summary>
                public const int RowNoSheetNo = 4;

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
            public string ConditionControlGroupId { get { return string.Format(ControlGroupId.ConditionFmt, formNo); } }
            /// <summary>ダウンロード条件</summary>
            public ExcelPortDownloadCondition DownloadCondition { get; set; }
            /// <summary>アップロード条件</summary>
            public ExcelPortUploadCondition UploadCondition { get; set; }
            /// <summary>対象場所階層情報リスト</summary>
            public List<StructureInfo> TargetLocationInfoList { get; set; }
            /// <summary>対象職種情報リスト</summary>
            public List<StructureInfo> TargetJobInfoList { get; set; }
            /// <summary>対象工場IDリスト</summary>
            public List<int> TargetFactoryIdList { get; set; }
            /// <summary>ExcelPort利用可能工場IDリスト</summary>
            public List<int> ExcelPortFactoryIdList { get; set; }
            /// <summary>変更履歴管理対象工場IDリスト</summary>
            public List<int> ApprovalFactoryIdList { get; set; }
            /// <summary>Excelコマンド処理クラス</summary>
            public CommonExcelCmd ExcelCmd { get; set; }
            /// <summary>エラー情報リスト</summary>
            public List<ComBase.UploadErrorInfo> ErrorInfoList { get; set; }
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
            /// <param name="resultMsg">結果メッセージ</param>
            /// <param name="detailMsg">詳細メッセージ</param>
            /// <param name="isUpload"></param>
            /// <returns></returns>
            public bool InitializeExcelPortTemplateFile(out string resultMsg, out string detailMsg, bool isUpload = false)
            {
                //==========
                // 初期化
                //==========
                resultMsg = string.Empty;
                detailMsg = string.Empty;

                // ExcelPort用マッピング情報の取得
                var mapInfoList = GetDBMappingList();

                string ctrlGrpId = this.ConditionControlGroupId;
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.condition, ctrlGrpId);
                if (isUpload) {
                    // アップロード条件の取得
                    ExcelPortUploadCondition ulCondition = new();
                    ComUtil.SetConditionByDataClass(ctrlGrpId, mapInfoList, ulCondition, targetDic, ComUtil.ConvertType.Execute);
                    this.UploadCondition = ulCondition;
                }
                else
                {
                    // ダウンロード条件の取得
                    ExcelPortDownloadCondition dlCondition = new();
                    ComUtil.SetConditionByDataClass(ctrlGrpId, mapInfoList, dlCondition, targetDic, ComUtil.ConvertType.Execute);
                    this.DownloadCondition = dlCondition;
                }

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
                if (this.locationIdList.Count == 0 || isUpload)
                {
                    // 場所階層条件が未指定またはアップロードの場合、所属情報から取得
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
                if (this.jobIdList.Count == 0 || isUpload)
                {
                    // 職種条件が未指定またはアップロードの場合、所属情報から取得
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
                this.ExcelPortFactoryIdList = getExcelPortTargetFactoryIdList();
                var excludedFactoryIdList = new List<int>();
                if (this.ExcelPortFactoryIdList.Count == 0)
                {
                    // ExcelPort利用可能工場が存在しない場合
                    if (isUpload)
                    {
                        // 「ExcelPort利用可能工場以外の工場データは登録できません。」
                        resultMsg = GetResMessage(ComRes.ID.ID141040004, this.languageId, this.msgResources);
                    }
                    else
                    {
                        // 「ExcelPort利用可能工場以外の工場データは出力できません。」
                        resultMsg = GetResMessage(ComRes.ID.ID141040003, this.languageId, this.msgResources);
                    }
                    return false;
                }
                else
                {
                    var contains = false;
                    for (int i = this.TargetFactoryIdList.Count - 1; i >= 0; i--)
                    {
                        var id = this.TargetFactoryIdList[i];
                        if (!this.ExcelPortFactoryIdList.Contains(id))
                        {
                            // ExcelPort利用可能工場以外の場合、対象外
                            excludedFactoryIdList.Add(id);  // 除外工場
                            TargetFactoryIdList.RemoveAt(i);
                            contains = true;
                        }
                    }
                    if (!isUpload)
                    {
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
                }

                // 変更管理対象工場の取得
                this.ApprovalFactoryIdList = history.GetUserApprovalFactoryList();
                var excludedApprovalCnt = 0;
                if (this.ApprovalFactoryIdList.Count > 0)
                {
                    var contains = false;
                    for (int i = this.TargetFactoryIdList.Count - 1; i >= 0; i--)
                    {
                        if (this.ApprovalFactoryIdList.Contains(this.TargetFactoryIdList[i]))
                        {
                            // 変更管理対象工場の場合、対象外
                            this.TargetFactoryIdList.RemoveAt(i);
                            contains = true;
                        }
                    }
                    if (!isUpload)
                    {
                        foreach (var id in excludedFactoryIdList)
                        {
                            if (this.ApprovalFactoryIdList.Contains(id))
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

                // 構成マスタからExcelPortバージョンを取得
                option.Version = getExcelPortVersion();

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

            /// <summary>
            /// ExcelPortアップロード条件のチェック
            /// </summary>
            /// <param name="file">Excelファイル</param>
            /// <param name="msg">メッセージ</param>
            /// <returns></returns>
            public bool CheckUploadCondition(IFormFile file, out string msg, out string conductId, out int sheetNo)
            {
                conductId = string.Empty;
                sheetNo = -1;

                // Excel読み込み
                this.ExcelCmd = TMQUtil.FileOpen(file.OpenReadStream());

                // Excelファイルから各条件を取得

                // ExcelPortバージョン番号を取得
                var versionNo = getCellValue(SheetName.DefineInfo, DefineSheetInfo.ColNoHeaderVal, DefineSheetInfo.RowNoVersion);
                if (string.IsNullOrEmpty(versionNo))
                {
                    // 「指定されたEXCELのフォーマットが不正です。」
                    msg = GetResMessage(ComRes.ID.ID141120010, this.languageId, this.msgResources);
                    return false;
                }

                // 対象機能IDを取得
                conductId = getCellValue(SheetName.DefineInfo, DefineSheetInfo.ColNoHeaderVal, DefineSheetInfo.RowNoConductId);
                if (string.IsNullOrEmpty(conductId))
                {
                    // 「指定されたEXCELのフォーマットが不正です。」
                    msg = GetResMessage(ComRes.ID.ID141120010, this.languageId, this.msgResources);
                    return false;
                }

                // 対象シート番号を取得
                var sheetNoStr = getCellValue(SheetName.DefineInfo, DefineSheetInfo.ColNoHeaderVal, DefineSheetInfo.RowNoSheetNo);
                if (string.IsNullOrEmpty(sheetNoStr))
                {
                    // 「指定されたEXCELのフォーマットが不正です。」
                    msg = GetResMessage(ComRes.ID.ID141120010, this.languageId, this.msgResources);
                    return false;
                }
                sheetNo = Convert.ToInt32(sheetNoStr);

                // 構成マスタからExcelPort番号を取得
                decimal versionNo2 = getExcelPortVersion();
                if(Convert.ToDecimal(versionNo) != versionNo2)
                {
                    // バージョン番号が一致しない場合
                    // 「指定されたEXCELはバージョンが最新ではありません。最新バージョンをダウンロードしてください。」
                    msg = GetResMessage(ComRes.ID.ID141120011, this.languageId, this.msgResources);
                    return false;
                }

                // 構成マスタから対象機能名を取得(拡張項目1=対象機能ID、拡張項目2=対象シート番号のデータ)
                string targetName = getTargetConductName(conductId, sheetNo);
                if (string.IsNullOrEmpty(targetName))
                {
                    // 「指定されたEXCELから更新対象機能が特定できません。」
                    msg = GetResMessage(ComRes.ID.ID141120012, this.languageId, this.msgResources);
                    return false;
                }

                // 「{0}データを登録します。よろしいですか？」
                msg = GetResMessage(new string[] { ComRes.ID.ID141120010, targetName }, this.languageId, this.msgResources);

                return true;
            }

            /// <summary>
            /// ExcelPortアップロード用データの取得
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="file"></param>
            /// <param name="condition"></param>
            /// <param name="resultList"></param>
            /// <param name="errorMsg"></param>
            /// <returns></returns>
            public bool GetUploadDataList<T>(IFormFile file, Dictionary<string, object> condition, out List<T> resultList, out string errorMsg, 
                ref string fileType, ref string fileName, ref MemoryStream ms)
            {
                resultList = new List<T>();
                errorMsg = string.Empty;

                // Excel読み込み
                this.ExcelCmd = TMQUtil.FileOpen(file.OpenReadStream());

                // アップロード条件を取得
                if(!condition.ContainsKey(ConditionValName.TargetConductId) || !condition.ContainsKey(ConditionValName.TargetSheetNo))
                {
                    // 「指定されたEXCELから更新対象機能が特定できません。」
                    errorMsg = GetResMessage(ComRes.ID.ID141120012, this.languageId, this.msgResources);
                    return false;
                }
                this.UploadCondition.ConductId = condition[ConditionValName.TargetConductId].ToString();
                this.UploadCondition.SheetNo = (int)(condition[ConditionValName.TargetSheetNo]);

                // 入力チェック＆変換
                this.ErrorInfoList = checkUploadCondition<T>(ref resultList, ref errorMsg);
                if(!string.IsNullOrEmpty(errorMsg))
                {
                    return false;
                }
                else if(this.ErrorInfoList.Count > 0)
                {
                    // エラー情報シートへ設定
                    setErrorInfoSheet(file, ref fileType, ref fileName, ref ms);
                    return false;
                }
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
            /// 構成マスタからExcelPortバージョンを取得
            /// </summary>
            /// <returns>ExelPortバージョン番号</returns>
            private decimal getExcelPortVersion()
            {
                // 構成マスタからExcelPortバージョンを取得
                var entity = TMQUtil.SqlExecuteClass.SelectEntity<AutoCompleteEntity>(SqlName.GetExcelPortVersion, SqlName.SubDirName, null, db);
                if (entity != null)
                {
                    return Convert.ToDecimal(entity.Exparam1);
                }
                else
                {
                    return decimal.MinValue;
                }
            }

            /// <summary>
            /// 構成マスタからExcelPort対象機能名を取得
            /// </summary>
            /// <returns>ExcelPort対象機能名</returns>
            private string getTargetConductName(string conductId, int sheetNo)
            {
                // 構成マスタからExcelPort対象機能名を取得
                var entity = TMQUtil.SqlExecuteClass.SelectEntity<AutoCompleteEntity>(
                    SqlName.GetExcelPortTargetInfo, SqlName.SubDirName, new { ConductId = conductId, SheetNo = sheetNo, LanguageId = this.languageId }, db);
                if (entity != null)
                {
                    return entity.Labels;
                }
                else
                {
                    return string.Empty;
                }
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

                // ログインユーザIDを設定
                ((IDictionary<string, object>)condition).Add("userId", this.userId);

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

            /// <summary>
            /// アップロード条件のチェック
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="resultList"></param>
            /// <param name="errorMsg"></param>
            /// <param name="checkFlg"></param>
            /// <returns></returns>
            private List<ComBase.UploadErrorInfo> checkUploadCondition<T>(ref List<T> resultList, ref string errorMsg, bool checkFlg = true)
            {
                // エラー内容格納クラス
                List<ComBase.UploadErrorInfo> errorInfo = new List<ComBase.UploadErrorInfo>();

                // ファイル入力項目定義情報を取得
                int sheetNo = this.UploadCondition.SheetNo;
                ComBase.InputDefineCondition param = new ComBase.InputDefineCondition()
                {
                    ReportId = Template.ReportId,
                    SheetNo = sheetNo,
                    ControlGroupId = ControlGroupId.Upload,
                    LanguageId = this.languageId,
                    FactoryId = TMQConsts.CommonFactoryId
                };
                var reportInfoList = TMQUtil.SqlExecuteClass.SelectList<InputDefineForExcelPort>(SqlName.GetInputControlDefineForExcelPort, SqlName.SubDirName, param, db);
                if (reportInfoList == null || reportInfoList.Count == 0)
                {
                    // 取得できない場合、処理を戻す
                    // 「指定されたEXCELのフォーマットが不正です。」
                    errorMsg = GetResMessage(ComRes.ID.ID141120010, this.languageId, this.msgResources);
                    return errorInfo;
                }

                // 検索結果クラスのプロパティを列挙
                var properites = typeof(T).GetProperties();
                // 1レコード分の行数、1レコード分の行数を取得する
                int addRow = reportInfoList[0].RecordCount;
                // 入力方式を取得
                int dataDirection = reportInfoList[0].DataDirection;

                int index = 0;
                while (true)
                {
                    // エラー内容一時格納クラス
                    List<ComBase.UploadErrorInfo> tmpErrorInfo = new List<ComBase.UploadErrorInfo>();

                    bool flg = false; // データ存在チェックフラグ
                    object tmpResult = Activator.CreateInstance<T>();

                    // 取得できた項目定義分処理を行う
                    foreach (ComBase.InputDefine reportInfo in reportInfoList)
                    {
                        // 2行目以降、入出力方式によって、表示位置をずらす
                        if (index > 0)
                        {
                            switch (dataDirection)
                            {
                                case ComReport.SingleCell:
                                    // 基本、到達しない
                                    continue;
                                case ComReport.LongitudinalDirection:
                                    // 縦方向連続の場合、行番号を加算する
                                    reportInfo.StartRowNo += addRow;
                                    break;
                                case ComReport.LateralDirection:
                                    // 横方向連続の場合、列番号を加算する
                                    reportInfo.StartColumnNo += addRow;
                                    break;
                                default:
                                    // 入出力方式が未設定の場合、スキップ
                                    break;
                            }
                        }

                        // 設定値を取得
                        string val = getCellValueBySheetNo(sheetNo, reportInfo.StartColumnNo, reportInfo.StartRowNo);

                        if (checkFlg)
                        {
                            // 値が取得できない場合、スキップ
                            if (string.IsNullOrEmpty(val))
                            {
                                if (reportInfo.RequiredFlg != null && (bool)reportInfo.RequiredFlg)
                                {
                                    // 必須入力項目の場合、エラー内容を設定
                                    // 「必須項目です。入力してください。」
                                    errorMsg = GetResMessage(ComRes.ID.ID941270001, languageId, msgResources);
                                    tmpErrorInfo.Add(setTmpErrorInfo(reportInfo.StartRowNo, reportInfo.StartColumnNo, reportInfo.TranslationText, errorMsg, dataDirection));
                                }
                                continue;
                            }
                        }

                        // 入力項目が存在する場合、フラグをたてる
                        flg = true;

                        if (checkFlg)
                        {
                            // アップロード共通チェック実行
                            if (ExecuteCommonUploadCheck(reportInfo, val, dataDirection, languageId, msgResources, this.db, tmpErrorInfo))
                            {
                                continue;
                            }
                        }

                        // 値をデータクラスに設定
                        string pascalItemName = ComUtil.SnakeCaseToPascalCase(reportInfo.AliasName != null ? reportInfo.AliasName : reportInfo.ColumnName).ToUpper();
                        var prop = properites.FirstOrDefault(x => x.Name.ToUpper().Equals(pascalItemName));
                        if (prop == null)
                        {
                            // 該当する項目が存在しない場合、スキップ
                            continue;
                        }
                        ComUtil.SetPropertyValue<T>(prop, (T)tmpResult, val);
                    }

                    // データが1件も取得できなかった場合、処理を抜ける
                    if (!flg)
                    {
                        break;
                    }

                    //エラーがある場合、エラーフラグを立てる
                    var errProp = properites.FirstOrDefault(x => x.Name.ToUpper().Equals("ErrorFlg"));
                    if (errProp != null && tmpErrorInfo.Count > 0)
                    {
                        ComUtil.SetPropertyValue<T>(errProp, (T)tmpResult, true);
                    }

                    // データが存在する場合、リストに追加する
                    setErrorInfo(ref errorInfo, tmpErrorInfo);
                    resultList.Add((T)tmpResult);
                    index++;

                    // 入力方式が単一セルの場合、処理を抜ける
                    if ((int)dataDirection == ComReport.SingleCell)
                    {
                        break;
                    }
                }

                return errorInfo;
            }

            /// <summary>
            /// Excelセルの値を取得(シート名指定)
            /// </summary>
            /// <param name="sheetName">シート名</param>
            /// <param name="colNo">列番号</param>
            /// <param name="rowNo">行番号</param>
            /// <returns></returns>
            private string getCellValue(string sheetName, int colNo, int rowNo)
            {
                string error = string.Empty;
                string msg = string.Empty;
                string[,] vals = null;

                // マッピングセルを設定
                string address = ToAlphabet(colNo) + rowNo;
                // セル単位でデータを取得する
                if (!this.ExcelCmd.ReadExcel(sheetName, address, ref vals, ref msg))
                {
                    // 読込失敗した場合、nullを返す
                    return null;
                }
                return vals[0, 0]; // セル単位で取得しているので先頭を対象データとみなす。
            }

            /// <summary>
            /// Excelセルの値を取得(シート番号指定)
            /// </summary>
            /// <param name="sheetNo"></param>
            /// <param name="colNo"></param>
            /// <param name="rowNo"></param>
            /// <returns></returns>
            private string getCellValueBySheetNo(int sheetNo, int colNo, int rowNo)
            {
                string error = string.Empty;
                string msg = string.Empty;
                string[,] vals = null;

                // マッピングセルを設定
                string address = ToAlphabet(colNo) + rowNo;
                // セル単位でデータを取得する
                if (!this.ExcelCmd.ReadExcelBySheetNo(sheetNo, address, ref vals, ref msg))
                {
                    // 読込失敗した場合、nullを返す
                    return null;
                }
                return vals[0, 0]; // セル単位で取得しているので先頭を対象データとみなす。
            }

            private void setErrorInfoSheet(IFormFile file, ref string fileType, ref string fileName, ref MemoryStream ms)
            {
                // ファイルタイプ
                fileType = ComConsts.REPORT.FILETYPE.EXCEL_MACRO;
                // ダウンロードファイル名
                fileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", file.FileName, DateTime.Now) + ComConsts.REPORT.EXTENSION.EXCEL_MACRO_BOOK;
                // メモリストリーム
                ms = new MemoryStream();

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

    /// <summary>
    /// ExcelPort用ファイル入力管理クラス
    /// </summary>
    public class InputDefineForExcelPort : ComBase.InputDefine
    {
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
