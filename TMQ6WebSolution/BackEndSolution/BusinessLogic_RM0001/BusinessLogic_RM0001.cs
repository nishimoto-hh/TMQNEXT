using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_RM0001.BusinessLogicDataClass_RM0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
//using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;

namespace BusinessLogic_RM0001
{
    /// <summary>
    /// 帳票出力
    /// </summary>
    public partial class BusinessLogic_RM0001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Report";

            #region 一覧画面
            /// <summary>
            /// 一覧情報(出力項目)取得SQL
            /// </summary>
            public const string GetReportList = "GetReportList";
            #endregion

            #region 出力パターン登録画面
            /// <summary>
            /// 出力パターン登録SQL
            /// </summary>
            public const string InsertOutputPattern = "InsertOutputPattern";
            /// <summary>
            /// 出力パターン削除SQL
            /// </summary>
            public const string DeleteOutputPattern = "DeleteOutputPattern";
            /// <summary>
            /// 出力項目登録SQL
            /// </summary>
            public const string InsertOutputItem = "InsertOutputItem";
            /// <summary>
            /// 出力項目登録SQL（Sheet2以降用）
            /// </summary>
            public const string InsertOutputItemForSheets = "InsertOutputItemForSheets";
            /// <summary>
            /// 出力項目削除SQL
            /// </summary>
            public const string DeleteOutputItem = "DeleteOutputItem";
            /// <summary>
            /// 最大出力パターンID取得SQL
            /// </summary>
            public const string GetMaxOutputPatternId = "GetMaxOutputPatternId";
            /// <summary>
            /// パターン情報件数取得SQL
            /// </summary>
            public const string GetOutputPatternCountCheck = "GetOutputPatternCountCheck";
            /// <summary>
            /// 帳票定義のカラム名から項目idリスト取得SQL
            /// </summary>
            public const string GetItemIdByColumnName = "GetItemIdByColumnName";
            /// <summary>
            /// 帳票定義のカラム名から項目idリスト取得SQL
            /// </summary>
            public const string GetStartCellInfo = "GetStartCellInfo";
            #endregion

            #region 雛形ファイル登録画面
            /// <summary>
            /// ディレクトリ構成取得SQL
            /// </summary>
            public const string UploadDirectory = "GetUploadDirectory";
            /// <summary>
            /// テンプレート情報登録SQL
            /// </summary>
            public const string InsertOutputTemplate = "InsertOutputTemplate";
            /// <summary>
            /// テンプレート情報登録SQL
            /// </summary>
            public const string UpdateOutputTemplate = "UpdateOutputTemplate";
            /// <summary>
            /// 最大テンプレートID取得SQL
            /// </summary>
            public const string GetMaxTemplateId = "GetMaxTemplateId";
            /// <summary>
            /// テンプレート情報件数取得SQL
            /// </summary>
            public const string GetOutputTemplateCountCheck = "GetOutputTemplateCountCheck";
            /// <summary>
            /// ファイル入出力項目定義情報取得用SQL
            /// </summary>
            public const string GetOutputReportDefineForUpload = "GetOutputReportDefineForUpload";
            /// <summary>
            /// 出力帳票定義情報複写登録SQL
            /// </summary>
            public const string SelectInsertOutputDefineInfo = "SelectInsertOutputDefineInfo";
            #endregion
        }

        /// <summary>
        /// フォーム、グループ、コントロールの定数クラス
        /// </summary>
        private static class ConductInfo
        {
            /// <summary>
            /// 一覧画面
            /// </summary>
            public static class FormList
            {
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 検索条件
                    /// </summary>
                    public const string Condition = "CBODY_000_00_LST_0_RM0001";
                    /// <summary>
                    /// 一覧
                    /// </summary>
                    public const string List = "CBODY_020_00_LST_0_RM0001";
                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonCtrlId
                {
                    /// <summary>
                    /// 検索
                    /// </summary>
                    public const string Search = "RM0001_Search";
                    /// <summary>
                    /// 更新
                    /// </summary>
                    public const string Regist = "RM0001_Regist";
                    /// <summary>
                    /// 削除
                    /// </summary>
                    public const string Delete = "RM0001_Delete";
                    /// <summary>
                    /// 新規
                    /// </summary>
                    public const string New = "RM0001_New";
                    /// <summary>
                    /// アップロード
                    /// </summary>
                    public const string Upload = "RM0001_Upload";
                    /// <summary>
                    /// クリア
                    /// </summary>
                    public const string Clear = "RM0001_Clear";
                    /// <summary>
                    /// ダウンロード
                    /// </summary>
                    public const string Download = "RM0001_Download";
                    /// <summary>
                    /// 出力
                    /// </summary>
                    public const string Output = "RM0001_Output";
                    /// <summary>
                    /// 戻る
                    /// </summary>
                    public const string Back = "RM0001_Back";
                }
            }

            /// <summary>
            /// 出力パターン登録画面
            /// </summary>
            public static class FormOutPattern
            {
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 出力パターン名
                    /// </summary>
                    public const string PatternName = "CBODY_040_00_LST_0_RM0001";
                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonCtrlId
                {
                    /// <summary>
                    /// 更新
                    /// </summary>
                    public const string Regist = "RM0001_RegistOutPattern";
                    /// <summary>
                    /// キャンセル
                    /// </summary>
                    public const string Cancel = "RM0001_CancelOutPattern";
                }
            }

            /// <summary>
            /// 雛形ファイル登録画面
            /// </summary>
            public static class FormUploadTemplate
            {
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 雛形ファイル
                    /// </summary>
                    public const string FileName = "CBODY_060_00_LST_0_RM0001";
                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonCtrlId
                {
                    /// <summary>
                    /// 登録(アップロード)
                    /// </summary>
                    public const string Regist = "RM0001_UploadTmpFile";
                    /// <summary>
                    /// キャンセル
                    /// </summary>
                    public const string Cancel = "RM0001_CancelTmpFile";
                }
            }
        }

        /// <summary>
        /// ファイルアップロード先の桁数
        /// </summary>
        public static class LengthOfFile
        {
            /// <summary>
            /// アップロード先のディレクトリ
            /// </summary>
            public const int Path = 259;
            /// <summary>
            /// ファイル名
            /// </summary>
            public const int FileName = 200;
        }

        /// <summary>
        /// 出力パターンデフォルト値
        /// </summary>
        public static class OutputPatternDefaultValue
        {
            /// <summary>
            /// シートNo
            /// </summary>
            public const int SheetNo = 1;
            /// <summary>
            /// 出力パターンID
            /// </summary>
            public const int OutputPatternId = 1;
            /// <summary>
            /// 出力パターン名
            /// </summary>
            public const string OutputPatternName = "パターン１";
        }

        /// <summary>
        /// ページ状態
        /// </summary>
        public static class PageStatus
        {
            /// <summary>初期状態</summary>
            public const int Init = 0;
            /// <summary>検索後</summary>
            public const int Search = 1;
            /// <summary>明細表示後アクション</summary>
            public const int SearchAf = 2;
            /// <summary>ファイル取込後</summary>
            public const int Upload = 3;
        }

        /// <summary>
        /// 出力項目種別
        /// </summary>
        public static class OutputItemType
        {
            /// <summary>出力項目固定</summary>
            public const int OutputItemFix = 1;
            /// <summary>出力項目可変（出力パターン指定なし）</summary>
            public const int PatternNotExists = 2;
            /// <summary>出力項目可変（出力パターン指定あり）</summary>
            public const int PatternExists = 3;
        }

        /// <summary>
        /// 帳票出力オプション使用帳票ID
        /// </summary>
        public static class UseOptionReportIdList
        {
            /// <summary>長期スケジュール表</summary>
            public const string ReportIdRP0090 = "RP0090";
            /// <summary>年度スケジュール表</summary>
            public const string ReportIdRP0100 = "RP0100";
        }

        /// <summary>
        /// グローバルリストのキー名称
        /// </summary>
        public static class GlobalListKeyRM0001
        {
            /// <summary>呼び元画面の機能ID</summary>
            public const string ParentConductId = "RM0001_ParentConductId";
            /// <summary>呼び元画面のプログラムID</summary>
            public const string ParentPgmId = "RM0001_ParentPgmId";
            /// <summary>呼び元画面の選択帳票ID</summary>
            public const string TargetReportId = "RM0001_TargetReportId";
            /// <summary>呼び元画面の対象のコントロールID</summary>
            public const string TargetCtrlId = "RM0001_TargetCtrlId";
            /// <summary>呼び元画面の対象のコントロールID（長期スケジュール用）</summary>
            public const string TargetOptionCtrlId = "RM0001_TargetOptionCtrlId";
            /// <summary>呼び元画面の対象のコントロールID（長期スケジュール用）の設定値一式</summary>
            public const string OptionDataList = "RM0001_OptionDataList";
            /// <summary>権限レベル</summary>
            public const string AuthorityLevel = "RM0001_AuthorityLevel";
            /// <summary>登録工場ID</summary>
            public const string RegistFactoryId = "RM0001_RegistFactoryId";
            /// <summary>登録パターンID</summary>
            public const string RegistPatternId = "RM0001_RegistPatternId";
            /// <summary>登録テンプレートID</summary>
            public const string RegistTemplateId = "RM0001_RegistTemplateId";
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_RM0001() : base()
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

            changeFormListBtnEnabled(PageStatus.Init);

            // グローバル変数に権限レベルをセットする
            SetGlobalData(GlobalListKeyRM0001.AuthorityLevel, this.AuthorityLevelId);

            // 初期化処理は何もしない
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            // 一覧画面検索処理
            if (!searchList())
            {
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
            // ボタンコントロールIDで判定
            if (this.CtrlId.Contains("Regist"))
            {
                // 登録、更新の場合
                // 登録処理実行
                return Regist();
            }
            else if (this.CtrlId.Contains("Delete"))
            {
                // 削除の場合
                // 削除処理実行
                return Delete();
            }
            // 他の処理がある場合、else if 節に条件を追加する
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

            switch (this.CtrlId)
            {
                case ConductInfo.FormList.ButtonCtrlId.Regist: // 一覧画面_更新
                case ConductInfo.FormOutPattern.ButtonCtrlId.Regist: // 出力パターン_登録画面
                    setReportImplDataByCondition(out int factoryId, out string reportId, out int templateId, 
                                                out int outputPatternId, out string programId, 
                                                out string templateFilePath, out string templateFileName);
                    resultRegist = executeRegistEdit(this.CtrlId, programId);
                    break;
                default:
                    // 処理が想定される場合は、分岐に条件を追加して処理を記載すること
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }

            // 登録処理結果によりエラー処理を行う
            if (!resultRegist)
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 一覧画面再検索処理
            if (!searchList())
            {
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
            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse

            switch (this.CtrlId)
            {
                case ConductInfo.FormList.ButtonCtrlId.Delete: // 一覧画面_削除
                    resultRegist = executeDelete(this.CtrlId);
                    break;
                default:
                    // 処理が想定される場合は、分岐に条件を追加して処理を記載すること
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }

            // 登録処理結果によりエラー処理を行う
            if (!resultRegist)
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 再検索処理
            if (!searchList())
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「削除処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911110001 });

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// ファイルアップロード処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int UploadImpl()
        {
            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse

            // 登録・ファイルアップロード処理実行
            resultRegist = registTemplateFile();

            // 登録処理結果によりエラー処理を行う
            if (!resultRegist)
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 再検索処理
            if (!searchList())
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「登録処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200003 });
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            // ダウンロードの場合
            if (this.CtrlId.Contains("Download"))
            {
                return executeDownload();
            }
            // 出力の場合
            else if (this.CtrlId.Contains("Output"))
            {
                return executeOutputReport();
            }
            else
            {
                // 処理が想定される場合は、分岐に条件を追加して処理を記載すること
                // この部分は到達不能なので、エラーを返す
                return ComConsts.RETURN_RESULT.NG;
            }
        }

        #endregion

        #region privateメソッド

        #region ボタン制御

        /// <summary>
        /// ボタン制御
        /// </summary>
        /// <param name="status">ﾍﾟｰｼﾞ状態　0：初期状態、1：検索後、2：明細表示後ｱｸｼｮﾝ、3：ｱｯﾌﾟﾛｰﾄﾞ後</param>
        private void changeFormListBtnEnabled(int status)
        {
            // 初期表示
            // 検索ボタン以外は非活性
            if (status == PageStatus.Init)
            {
                //// 検索
                //BtnActive(ConductInfo.FormList.ButtonCtrlId.Search);
                // クリア
                BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Clear);
                // 登録
                BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Regist);
                // 削除
                BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Delete);
                // 新規
                BtnDisabled(ConductInfo.FormList.ButtonCtrlId.New);
                // アップロード
                BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Upload);
                // ダウンロード
                BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Download);
                // 出力
                BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Output);
            }
            if (status != PageStatus.Init)
            {
                //// 検索
                //BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Search);

                // クリア
                // 検索後（クリアボタンは活性）
                BtnActive(ConductInfo.FormList.ButtonCtrlId.Clear);
                // アップロード
                // 管理一覧からの遷移でない（各機能からの遷移）場合
                // かつ、アップロード可能な帳票
                if (isTransFromRM0002() == true && isTemplateUpload() == true)
                {
                    BtnActive(ConductInfo.FormList.ButtonCtrlId.Upload);
                }
                else
                {
                    BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Upload);
                }

                // ダウンロード
                // 管理一覧からの遷移でない（各機能からの遷移）場合、かつ
                // Templateが指定されている場合
                if (isTransFromRM0002() == true && isSelectTemplate() == true)
                {
                    BtnActive(ConductInfo.FormList.ButtonCtrlId.Download);
                }
                else
                {
                    BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Download);
                }

                // 新規
                // 対象帳票の出力項目種別が出力パターン指定ありの場合、かつ
                // Templateが指定されている場合
                if (isOutputItemTypePatternExists() == true && isSelectTemplate() == true)
                {
                    BtnActive(ConductInfo.FormList.ButtonCtrlId.New);
                }
                else
                {
                    BtnDisabled(ConductInfo.FormList.ButtonCtrlId.New);
                }

                // 更新
                // 削除
                // 対象帳票の出力項目種別が出力パターン指定ありの場合、かつ
                // 出力パターンが指定されている場合
                if (isOutputItemTypePatternExists() == true && isSelectPattern() == true)
                {
                    BtnActive(ConductInfo.FormList.ButtonCtrlId.Regist);
                    BtnActive(ConductInfo.FormList.ButtonCtrlId.Delete);
                }
                else
                {
                    BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Regist);
                    BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Delete);
                }

                // 出力
                // 管理一覧からの遷移でない（各機能からの遷移）場合、かつ
                // Patternが指定されている場合、もしくはTemplateが指定されており対象帳票の出力項目種別が出力パターン指定あり以外の場合）
                if (isTransFromRM0002() == false &&
                    ((isOutputItemTypePatternExists() == false && isSelectTemplate() == true) || isSelectPattern() == true))
                {
                    BtnActive(ConductInfo.FormList.ButtonCtrlId.Output);
                }
                else
                {
                    BtnDisabled(ConductInfo.FormList.ButtonCtrlId.Output);
                }

            }
        }

        /// <summary>
        /// 対象テンプレートを選択しているかを判定
        /// </summary>
        private bool isSelectTemplate()
        {
            // 抽出条件
            Dao.searchCondition condition = getSearchInfo<Dao.searchCondition>();
            // テンプレートIDが選択されているかどうか
            if (condition.TemplateId != null && condition.TemplateId != 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 選択帳票がテンプレートアップロード可能かを判定
        /// </summary>
        private bool isTemplateUpload()
        {
            // 抽出条件
            Dao.searchCondition condition = getSearchInfo<Dao.searchCondition>();
            // 出力パターンIDが選択されているかどうか
            if (condition != null)
            {
                // 登録帳票定義を取得
                ReportDao.MsOutputReportDefineEntity defineInfo = new ReportDao.MsOutputReportDefineEntity();
                var resultDefine = defineInfo.GetEntity(condition.FactoryId, condition.ProgramId, condition.ReportId, this.db);
                if (resultDefine == null)
                {
                    // エラーの場合
                    return false;
                }
                // テンプレートアップロード可否を返す
                return resultDefine.TemplateUploadFlg;
            }

            return false;
        }

        /// <summary>
        /// 対象パターンを選択しているかを判定
        /// </summary>
        private bool isSelectPattern()
        {
            // 抽出条件
            Dao.searchCondition condition = getSearchInfo<Dao.searchCondition>();
            // 出力パターンIDが選択されているかどうか
            if (condition.OutputPatternId != null && condition.OutputPatternId != 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 管理一覧からの遷移かどうかを判定
        /// </summary>
        private bool isTransFromRM0002()
        {
            // グローバルリストより以下の値を取得する
            object globalObj;
            string globalData = string.Empty;

            // グローバル変数：遷移元プログラムIDより管理一覧からの遷移かどうかを確認
            globalObj = GetGlobalData(GlobalListKeyRM0001.ParentConductId, false);
            globalData = Convert.ToString(globalObj);

            if (globalData.Equals("RM0002"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 対象帳票の出力項目種別が出力パターン指定ありかどうかを判定
        /// </summary>
        private bool isOutputItemTypePatternExists()
        {
            // 対象帳票の出力項目種別が出力パターン指定ありかどうか
            bool isChkFlg = false;
            // 出力項目一覧の隠し項目から出力項目種別を取得する
            var targetDics = ComUtil.GetDictionaryListByCtrlId(this.ResultList, ConductInfo.FormList.ControlId.List);
            foreach (Dictionary<string, object> resultDic in targetDics)
            {
                // 先頭行は飛ばす
                if (resultDic["ROWNO"] != null && int.Parse(resultDic["ROWNO"].ToString()) == 0)
                {
                    continue;
                }
                // 一覧の画面項目定義の情報から出力項目種別のVAL名を取得
                var mappingInfo = getResultMappingInfo(ConductInfo.FormList.ControlId.List);
                string valName = mappingInfo.Value.First(x => x.ParamName.Equals("OutputItemType")).ValName;

                if (resultDic[valName] != null && int.Parse(resultDic[valName].ToString()) == OutputItemType.PatternExists)
                {
                    isChkFlg = true;
                }
                // 最初のデータ行で判断する
                break;
            }
            return isChkFlg;
        }
        #endregion

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNo">取得するグループ</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getSearchInfo<T>()
            where T : CommonDataBaseClass.SearchCommonClass, new()
        {
            // 検索条件エリアのコントロールID
            string conditionCtrlId = ConductInfo.FormList.ControlId.Condition;

            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, conditionCtrlId);
            List<Dictionary<string, object>> conditionList = new() { condition };
            T searchInfo = new();

            // ページ情報取得
            var pageInfo = GetPageInfo(conditionCtrlId, this.pageInfoList);
            // 検索条件データの取得
            if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, conditionCtrlId, searchInfo, pageInfo))
            {
                // エラーの場合終了
                return searchInfo;
            }

            return searchInfo;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="targetCtrlId">対象コントロールID</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(string targetCtrlId, System.DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 相称型を使用することで、色々な型を呼出元で宣言することができる
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfo(targetCtrlId);

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
        /// <returns>登録内容のデータクラスのリスト※未選択のものは取得対象外</returns>
        private List<T> getRegistInfoList<T>(string ctrlId, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {

            // 一覧画面のコントロールID
            string targetCtrlId = ConductInfo.FormList.ControlId.List;

            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(targetCtrlId);
            // コントロールIDにより画面の項目(一覧)を取得
            List<Dictionary<string, object>> resultList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, targetCtrlId);
            // 戻り値となるデータクラスのリスト
            List<T> registInfoList = new();
            // 一覧を繰り返し、データクラスに変換、リストへ追加する
            foreach (var resultRow in resultList)
            {
                // 選択チェックボックスがある場合
                if (resultRow.ContainsKey("SELTAG") == true)
                {
                    // 未選択のデータは処理の対象外とする
                    if (int.Parse(resultRow["SELTAG"].ToString()) == 0)
                    {
                        continue;
                    }
                }
                else
                {
                    // 選択チェックボックスがない場合、スキップ
                    continue;
                }

                T registInfo = new();
                if (!SetExecuteConditionByDataClass<T>(resultRow, targetCtrlId, registInfo, now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return registInfoList;
                }
                registInfoList.Add(registInfo);
            }
            return registInfoList;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <param name="sqlName">SQLファイル名(拡張子は含まない)</param>
        /// <param name="addText">SQLの末尾に追加する内容 省略可能</param>
        /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
        /// <returns>エラーの場合False</returns>
        private bool registDb(dynamic registInfo, string sqlName, string addText = "", List<string> listUnComment = null)
        {
            // SQL実行
            bool result = TMQUtil.SqlExecuteClass.Regist(sqlName, SqlName.SubDir, registInfo, this.db, addText, listUnComment);
            return result;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <param name="sqlName">SQLファイル名</param>
        /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
        /// <returns>returnFlag:エラーの場合False、id:登録データのID</returns>
        private bool registDeleteDb<T>(T registInfo, string sqlName, List<string> listUnComment = null)
        {
            string sql;
            // SQL文の取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out sql, listUnComment))
            {
                return false;
            }

            int result = db.Regist(sql, registInfo);
            return true;
        }

        /// <summary>
        /// グローバル変数を出力処理用の変数に設定
        /// </summary>
        /// <param name="parentPgmId">遷移元のプログラムID</param>
        /// <param name="targetCtrlId">遷移元の対象コントロールID</param>
        /// <param name="keyInfo">一覧選択データのキー情報</param>
        private void setReportImplDataByGlobalData(out string parentPgmId, out string targetCtrlId, out string targetOptionCtrlId, out Dictionary<string, object> dicOptionData)
        {
            // グローバルリストより以下の値を取得する
            object globalObj;
            string globalData = string.Empty;

            globalObj = GetGlobalData(GlobalListKeyRM0001.ParentPgmId, false);
            parentPgmId = Convert.ToString(globalObj);

            globalObj = GetGlobalData(GlobalListKeyRM0001.TargetCtrlId, false);
            targetCtrlId = Convert.ToString(globalObj);

            globalObj = GetGlobalData(GlobalListKeyRM0001.TargetOptionCtrlId, false);
            targetOptionCtrlId = Convert.ToString(globalObj);

            dicOptionData = null;
            globalObj = GetGlobalData(GlobalListKeyRM0001.OptionDataList, false);

            if (globalObj != null)
            {
                List<object> listOptionData = (List<object>)globalObj;
                dicOptionData = (Dictionary<string, object>)listOptionData[0];
            }
        }

        /// <summary>
        /// 検索条件を出力処理用の変数に設定
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="reportId">帳票ID</param>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="outputPatternId">出力パターンID</param>
        /// <param name="programId">プログラムID</param>
        /// <param name="templatefilePath">テンプレートファイルパス</param>
        /// <param name="templatefileName">テンプレートファイル名</param>
        private void setReportImplDataByCondition(out int factoryId, out string reportId, out int templateId, out int outputPatternId,
                                                    out string programId, out string templatefilePath, out string templatefileName)
        {
            // 初期値を設定
            factoryId = 0;
            reportId = string.Empty;
            templateId = 0;
            outputPatternId = 0;
            programId = string.Empty;
            templatefilePath = string.Empty;
            templatefileName = string.Empty;

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.Condition, this.pageInfoList);

            // 検索条件設定
            dynamic conditionObj = new ExpandoObject();
            SetSearchCondition(this.searchConditionDictionary, ConductInfo.FormList.ControlId.Condition, conditionObj, pageInfo);

            // 工場ID
            if (conditionObj.FactoryId != null && string.IsNullOrEmpty(conditionObj.FactoryId) == false)
            {
                factoryId = int.Parse(conditionObj.FactoryId);
            }
            // 帳票ID
            if (conditionObj.ReportId != null && string.IsNullOrEmpty(conditionObj.ReportId) == false)
            {
                reportId = conditionObj.ReportId;
            }
            // テンプレートID
            if (conditionObj.TemplateId != null && string.IsNullOrEmpty(conditionObj.TemplateId) == false)
            {
                templateId = int.Parse(conditionObj.TemplateId);
            }
            // 出力パターンID
            if (conditionObj.OutputPatternId != null && string.IsNullOrEmpty(conditionObj.OutputPatternId) == false)
            {
                outputPatternId = int.Parse(conditionObj.OutputPatternId);
            }
            // 出力パターンID
            if (conditionObj.ProgramId != null && string.IsNullOrEmpty(conditionObj.ProgramId) == false)
            {
                programId = conditionObj.ProgramId;
            }
            // テンプレートファイルパス
            if (conditionObj.TemplateFilePath != null && string.IsNullOrEmpty(conditionObj.TemplateFilePath) == false)
            {
                templatefilePath = conditionObj.TemplateFilePath;
            }
            // テンプレートファイル名
            if (conditionObj.TemplateFileName != null && string.IsNullOrEmpty(conditionObj.TemplateFileName) == false)
            {
                templatefileName = conditionObj.TemplateFileName;
            }

        }
        #endregion
    }
}