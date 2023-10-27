using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComUtilDb = CommonSTDUtil.CommonDBManager;
using Dao = BusinessLogic_DM0002.BusinessLogicDataClass_DM0002;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_DM0002
{
    /// <summary>
    /// 文書管理
    /// </summary>
    public partial class BusinessLogic_DM0002 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        public static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Attachment";
            /// <summary>
            /// 添付情報更新SQL
            /// </summary>
            public const string UpdateAttachmentInfo = "UpdateAttachmentInfo";
            /// <summary>
            /// 添付情報更新SQL(ファイル情報以外)
            /// </summary>
            public const string UpdateAttachmentInfoExceptFile = "UpdateAttachmentInfoExceptFile";
            /// <summary>
            /// 添付情報削除SQL
            /// </summary>
            public const string DeleteAttachmentInfo = "DeleteAttachmentInfo";
            /// <summary>
            /// ユーザー名取得SQL
            /// </summary>
            public const string GetUserName = "GetUserName";

            /// <summary>
            /// 詳細画面SQL
            /// </summary>
            public static class Detail
            {
                /// <summary>
                /// 添付情報一覧検索SQL
                /// </summary>
                public const string GetAttachmentInfoList = "GetAttachmentInfoList";
                /// <summary>
                /// 件名(機器台帳 機番添付)取得SQL
                /// </summary>
                public const string MachineSubject = "GetMachineSubject";
                /// <summary>
                /// 件名(機器台帳 機器添付)取得SQL
                /// </summary>
                public const string EquipmentSubject = "GetEquipmentSubject";
                /// <summary>
                /// 件名(機器台帳 保全項目一覧 ファイル添付)取得SQL
                /// </summary>
                public const string ContentSubject = "GetContentSubject";
                /// <summary>
                /// 件名(機器台帳 MP情報タブ ファイル添付)取得SQL
                /// </summary>
                public const string MpInfoSubject = "GetMpInfoSubject";
                /// <summary>
                /// 件名(件名別長期計画 件名添付)取得SQL
                /// </summary>
                public const string LongPlanSubject = "GetLongPlanSubject";
                /// <summary>
                /// 件名(保全活動 件名添付)取得SQL
                /// </summary>
                public const string SummarySubject = "GetSummarySubject";
                /// <summary>
                /// 件名(保全活動 略図添付)取得SQL
                /// </summary>
                public const string HistoryFailureSubject = "GetHistoryFailureSubject";
                /// <summary>
                /// 件名(保全活動 故障原因分析書添付)取得SQL
                /// </summary>
                public const string HistoryFailureAnalyzeSubject = "GetHistoryFailureAnalyzeSubject";
                /// <summary>
                /// 件名(予備品管理 画像添付)取得SQL
                /// </summary>
                public const string SpareSubjectImage = "GetSpareSubjectImage";
                /// <summary>
                /// 件名(予備品管理 文書添付)取得SQL
                /// </summary>
                public const string SpareSubjectDocument = "GetSpareSubjectDocument";
                /// <summary>
                /// 件名(予備品管理 予備品地図)取得SQL
                /// </summary>
                public const string SpareSubjectMap = "GetSpareSubjectMap";
                /// <summary>
                /// 添付情報登録SQL
                /// </summary>
                public const string InsertAttachmentInfo = "InsertAttachmentInfo";
            }
        }

        /// <summary>
        /// 機能のコントロール情報
        /// </summary>
        /// <remarks>画面の規模が大きくなるとどのコントロールがどのフォームか分からない、特に改修時</remarks>
        public static class ConductInfo
        {
            /// <summary>
            /// 詳細画面
            /// </summary>
            public static class FormDetail
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
                    /// 件名情報
                    /// </summary>
                    public const string Subject = "CBODY_000_00_LST_0_DM0002";
                    /// <summary>
                    /// 一覧
                    /// </summary>
                    public const string List = "CBODY_010_00_LST_0_DM0002";
                    /// <summary>
                    /// 親画面一覧(一覧画面)
                    /// </summary>
                    public const string ParentList = "BODY_020_00_LST_0";
                }
            }
        }

        /// <summary>
        /// 添付種類の番号
        /// </summary>
        public static class AttachmentType
        {
            /// <summary>
            /// ファイル
            /// </summary>
            public const int File = 1;
            /// <summary>
            /// リンク
            /// </summary>
            public const int Link = 2;

        }

        /// <summary>
        /// 拡張データに持っている添付種類番号の取得条件
        /// </summary>
        public static class condAttachmentType
        {
            /// <summary>構成グループID</summary>
            public const short StructureGroupId = 1710;
            /// <summary>連番</summary>
            public const short Seq = 1;
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
        /// 割り振られていない仮添付ID
        /// </summary>
        public const int IdIsNone = -1;

        /// <summary>
        /// 一覧画面の機能ID
        /// </summary>
        public const string ParentConductId = "DM0001";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_DM0002() : base()
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
            // 初期検索実行
            return InitSearch();
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            // 単票でボタン(登録、戻る)がクリックされた場合
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (!searchDetail(compareId.IsBack() || compareId.IsUpload()))
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
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsDelete())
            {

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
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            this.ResultList = new();

            // 削除リスト取得
            var deleteList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.List);

            // 排他チェック
            if (!checkExclusiveList(ConductInfo.FormDetail.ControlId.List, deleteList))
            {
                // 排他エラー
                return ComConsts.RETURN_RESULT.NG;
            }

            // 削除SQL取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.DeleteAttachmentInfo, out string sql))
            {
                setError();
                return ComConsts.RETURN_RESULT.NG;
            }

            // 行削除
            foreach (var deleteRow in deleteList)
            {
                Dao.searchResult delCondition = new();
                SetDeleteConditionByDataClass(deleteRow, ConductInfo.FormDetail.ControlId.List, delCondition);

                int result = this.db.Regist(sql, delCondition);
                if (result < 0)
                {
                    // 削除エラー
                    setError();
                    return ComConsts.RETURN_RESULT.NG;
                }
            }

            // 再検索処理
            searchDetail(true);

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「削除処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911110001 });
            // 正常終了
            return ComConsts.RETURN_RESULT.OK;

            void setError()
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「削除処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911110001 });
                }
            }
        }

        /// <summary>
        /// 登録・ファイルアップロード処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int UploadImpl()
        {
            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse
            // メッセージ「更新処理」
            string actionMsg = ComRes.ID.ID911100002;

            // 登録・ファイルアップロード処理実行
            resultRegist = executeRegistDetail(ref actionMsg);

            // 登録処理結果によりエラー処理を行う
            if (!resultRegist)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // メッセージが設定されていない場合
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「登録処理/更新処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, actionMsg });
                }
                return ComConsts.RETURN_RESULT.NG;
            }
            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「登録処理/更新処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, actionMsg });
            return ComConsts.RETURN_RESULT.OK;
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
            // 到達不能
            return ComConsts.RETURN_RESULT.NG;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchDetail(bool isReSearch)
        {

            Dao.searchResult param = new();

            string ctrlId = isReSearch ? ConductInfo.FormDetail.ControlId.Subject : ConductInfo.FormDetail.ControlId.ParentList; // 値を取得する一覧
            Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);
            if (!isReSearch)
            {
                // 初期表示時はDM0001の一覧の画面定義を用いて引き継いだ値を取得するが、その定義がDM0002にないため追加する
                AddMappingListOtherPgmId(ParentConductId);
            }
            SetDataClassFromDictionary(result, ctrlId, param);

            // 言語IDを設定
            param.LanguageId = this.LanguageId;

            //件名取得
            string sqlName = string.Empty;
            switch (param.FunctionTypeId)
            {
                case (int)TMQConsts.Attachment.FunctionTypeId.Machine:                   //機器台帳 機番添付
                    sqlName = SqlName.Detail.MachineSubject;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.Equipment:                 //機器台帳 機器添付
                    sqlName = SqlName.Detail.EquipmentSubject;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.Content:                   //機器台帳 保全項目一覧 ファイル添付
                    sqlName = SqlName.Detail.ContentSubject;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.MpInfo:                    //機器台帳 MP情報タブ ファイル添付
                    sqlName = SqlName.Detail.MpInfoSubject;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.LongPlan:                  //件名別長期計画 件名添付
                    sqlName = SqlName.Detail.LongPlanSubject;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.Summary:                   //保全活動 件名添付
                    sqlName = SqlName.Detail.SummarySubject;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.HistoryFailureDiagram:     //保全活動 故障分析情報タブ 略図添付
                    sqlName = SqlName.Detail.HistoryFailureSubject;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.HistoryFailureAnalyze:     //保全活動 故障分析情報タブ 故障原因分析書添付
                    sqlName = SqlName.Detail.HistoryFailureAnalyzeSubject;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.HistoryFailureFactDiagram: //保全活動 故障分析情報(個別工場)タブ 略図添付
                    sqlName = SqlName.Detail.HistoryFailureSubject;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.HistoryFailureFactAnalyze: //保全活動 故障分析情報(個別工場)タブ 故障原因分析書添付
                    sqlName = SqlName.Detail.HistoryFailureAnalyzeSubject;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.SpareImage:                //予備品管理 画像添付
                    sqlName = SqlName.Detail.SpareSubjectImage;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.SpareDocument:             //予備品管理 文書添付
                    sqlName = SqlName.Detail.SpareSubjectDocument;
                    break;
                case (int)TMQConsts.Attachment.FunctionTypeId.SpareMap:                  //予備品管理 予備品地図
                    sqlName = SqlName.Detail.SpareSubjectMap;
                    break;
                default:
                    return false;
            }

            // 文書種類を取得するコンボボックスの項目番号を取得
            var mappingInfo = getResultMappingInfo(ConductInfo.FormDetail.ControlId.List);
            int documentTypeValNo = int.Parse(getValNoByKey(mappingInfo, param.FunctionTypeId.ToString()).Replace("VAL", ""));

            // 件名取得
            if (!getSubject(sqlName, param.FunctionTypeId, param.KeyId, documentTypeValNo))
            {
                return false;
            }

            // 添付情報取得
            getAttachmentInfoList(param);

            return true;

            string getValNoByKey(MappingInfo info, string keyName)
            {
                // 項目名と一致する項目番号を返す
                return info.Value.First(x => x.KeyName.Equals(keyName)).ValName;
            }
        }

        /// <summary>
        /// 件名 取得処理
        /// </summary>
        /// <param name="sqlName">SQL名称</param>
        /// <param name="functionTypeId">機能タイプID</param>
        /// <param name="keyId">キーID</param>
        /// <param name="documentTypeValNo">表示している文書種類コンボボックスの項目番号(VAL)</param>
        /// <returns>エラーの場合False</returns>
        private bool getSubject(string sqlName, int functionTypeId, long keyId, int documentTypeValNo)
        {
            // 件名取得SQL文の取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out string outSql);

            // 検索実行
            dynamic whereParam = new ExpandoObject();         // WHERE句パラメータ
            whereParam.FunctionTypeId = functionTypeId;       // 機能タイプID
            whereParam.KeyId = keyId;                         // キーID
            whereParam.DocumentTypeValNo = documentTypeValNo; // 文書種類コンボボックスの項目番号
            whereParam.LanguageId = this.LanguageId;          // 言語ID
            whereParam.CommomFactoryId = TMQConsts.CommonFactoryId;
            IList<Dao.subject> results = db.GetListByDataClass<Dao.subject>(outSql, whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormDetail.ControlId.Subject, this.pageInfoList);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.subject>(pageInfo, results, results.Count))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// 添付情報一覧検索処理
        /// </summary>
        /// <param name="param">検索条件</param>
        /// <returns>エラーの場合False</returns>
        private bool getAttachmentInfoList(Dao.searchResult param)
        {
            // 一覧検索SQL文の取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetAttachmentInfoList, out string outSql);

            dynamic whereParam = new ExpandoObject(); // WHERE句パラメータ
            whereParam = param;

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(outSql, whereParam);
            if (results == null || results.Count == 0)
            {
                // 添付されているデータが無い場合はメッセージを表示する
                // ＋ボタンを押下して新規登録してください。
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141280002 });
                return true;
            }

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormDetail.ControlId.List, this.pageInfoList);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results, results.Count))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// 詳細画面 登録処理
        /// </summary>
        /// <param name="msgId">メッセージID</param>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistDetail(ref string msgId)
        {
            // 排他チェック
            if (isErrorExclusive(ConductInfo.FormDetail.ControlId.List))
            {
                return false;
            }

            DateTime now = DateTime.Now;
            // 入力された内容を取得
            Dao.searchResult registInfo = getRegistInfo<Dao.searchResult>(ConductInfo.FormDetail.ControlId.List, now, this.UserId);
            // 件名情報一覧に隠しで登録されている値(登録条件)取得
            Dao.subject subjectInfo = getUploadCondition<Dao.subject>(ConductInfo.FormDetail.ControlId.Subject, now, this.UserId);

            // 登録情報を設定
            registInfo.LocationStructureId = subjectInfo.LocationStructureId;           // 場所階層ID
            registInfo.FunctionTypeId = subjectInfo.FunctionTypeId;                     // 機能タイプID
            registInfo.KeyId = subjectInfo.KeyId;                                       // キーID
            registInfo.AttachmentUserId = this.UserId;                                  // 作成者ID
            registInfo.AttachmentUserName = getUserName(this.UserId, this.db);          // 作成者
            registInfo.AttachmentTypeNo = getAttachmentTypeNo(registInfo.AttachmentTypeStructureId);
            // 文書種類を取得しなおす
            Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.List);
            registInfo.DocumentTypeStructureId = int.Parse(result["VAL" + subjectInfo.DocumentTypeValNo.ToString()].ToString());

            // 添付種類が「リンク」の場合
            if (registInfo.AttachmentTypeNo == AttachmentType.Link)
            {
                registInfo.FileName = registInfo.Link;
            }

            // 入力チェック
            if (isErrorRegist(ref registInfo, ConductInfo.FormDetail.ControlId.List))
            {
                return false;
            }

            bool updateFlg = true;
            // 新規登録の場合
            if (registInfo.AttachmentId == IdIsNone)
            {
                // メッセージ「登録処理」
                msgId = ComRes.ID.ID911200003;
                updateFlg = false;
            }

            // アップロード処理
            uploadFile(ref registInfo, this.InputStream);

            // 登録・更新処理
            if (!registDb(registInfo, updateFlg, this.InputStream, this.db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="ctrlId">取得するコントロールID</param>
        /// <param name="now">システム日時</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getUploadCondition<T>(string ctrlId, DateTime now, string userId = "-1")
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(ctrlId);

            // コントロールIDにより画面の項目(一覧)を取得
            Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);

            // TODO:ユーザIDを数値に変換するのは共通化
            T resultInfo = new();
            if (!SetExecuteConditionByDataClass<T>(result, ctrlId, resultInfo, now, this.UserId, userId))
            {
                // エラーの場合終了
                return resultInfo;
            }

            return resultInfo;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <param name="ctrlId">対象一覧のコントロールID</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive(string ctrlId)
        {
            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(ctrlId);
            var lockKeyMaps = GetLockKeyMaps(ctrlId);

            // 入力された内容を取得
            var condition = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);

            // 排他チェック
            if (!CheckExclusiveStatus(condition, lockValMaps, lockKeyMaps))
            {
                // エラーの場合
                return true;
            }

            return false;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="ctrlId">取得するコントロールID</param>
        /// <param name="now">システム日時</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(string ctrlId, DateTime now, string userId = "-1")
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // コントロールIDにより画面の項目(一覧)を取得
            Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);

            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(ctrlId);
            string attachmentIdVal = getValNoByParam(mappingInfo, "AttachmentId"); // 添付IDの項目番号

            // 添付IDが変換できるかどうか(変換できない = 新規登録)
            if (!int.TryParse(result[attachmentIdVal].ToString(), out int resulst))
            {
                result[attachmentIdVal] = IdIsNone;
            }

            // TODO:ユーザIDを数値に変換するのは共通化
            T resultInfo = new();
            if (!SetExecuteConditionByDataClass<T>(result, ctrlId, resultInfo, now, this.UserId, userId))
            {
                // エラーの場合終了
                return resultInfo;
            }

            return resultInfo;

            string getValNoByParam(MappingInfo info, string keyName)
            {
                // パラメータ名と一致する項目番号を返す
                return info.Value.First(x => x.ParamName.Equals(keyName)).ValName;
            }
        }

        /// <summary>
        /// ユーザー名取得処理
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="db">DBクラス</param>
        /// <returns>取得できない場合は空文字</returns>
        public static string getUserName(string userId, ComUtilDb.CommonDBManager db)
        {
            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetUserName, out string outSql);
            // WHERE句パラメータ
            dynamic whereParam = new ExpandoObject();
            whereParam.UserId = userId;
            // SQL実行
            dynamic userName = db.GetEntity(outSql, whereParam);

            // ユーザー名が取得できなかった場合
            if (userName == null || userName.user_name == null)
            {
                return string.Empty;
            }

            return userName.user_name;
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="ctrlId">対象一覧のコントロールID</param>
        /// <returns>エラーの場合true</returns>
        private bool isErrorRegist(ref Dao.searchResult registInfo, string ctrlId)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 単一の項目の場合のイメージ
            if (checkErrorRegist(ref errorInfoDictionary, ref registInfo, ctrlId))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            bool checkErrorRegist(ref List<Dictionary<string, object>> errorInfoDictionary, ref Dao.searchResult registInfo, string ctrlId)
            {
                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(ctrlId);
                // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                // 単一の内容を取得
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                // エラー情報格納クラス
                var errorInfo = new ErrorInfo(targetDic);
                string errMsg = string.Empty;
                string val = info.getValName("File"); // エラーをセットするファイル選択コントロールの項目番号
                string linkVal = info.getValName("Link"); // エラーをセットするリンクの項目番号

                // 添付種類が「リンク」の場合
                if (registInfo.AttachmentTypeNo == AttachmentType.Link)
                {
                    // 先頭文字が「HTTP://」「HTTPS://」以外の場合はエラーとする
                    if (!registInfo.Link.ToUpper().StartsWith("HTTP://") && !registInfo.Link.ToUpper().StartsWith("HTTPS://"))
                    {
                        // 「入力内容が不正です。」
                        errMsg = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID141220005 });
                        errorInfo.setError(errMsg, linkVal); // エラー情報をセット
                        errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                        return true;
                    }
                }

                // ファイル選択されていない場合は何もしない
                if (this.InputStream == null)
                {
                    return false;
                }

                // アップロード先を取得
                if (!createUploadPath(ref registInfo, this.LanguageId, this.db))
                {
                    // 「アップロード先の取得に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID941010001 });
                    return true;
                }

                // ファイル名を設定
                registInfo.FileName = getFileName(registInfo.FilePath, this.InputStream);

                // ファイル名が指定された文字数より多い場合はエラー
                if (registInfo.FileName.Length > LengthOfFile.FileName)
                {
                    // 「ファイル名が設定桁数を超えています。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941060014, ComRes.ID.ID111280020 });
                    errorInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    return true;
                }

                // アップロード先のパス(絶対パス)が指定された文字数より多い場合はエラー
                if (Path.GetFullPath(registInfo.FilePath + "\\" + registInfo.FileName).Length > LengthOfFile.Path)
                {
                    // 「アップロード先のパスが設定桁数を超えています。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060014, ComRes.ID.ID941010002 });
                    return true;
                }

                return false;
            }

        }

        /// <summary>
        /// 登録・更新処理
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="isUpdate">更新の場合はtrue</param>
        /// <param name="inputStream">選択されたファイル情報</param>
        /// <param name="db">DBクラス</param>
        /// <returns>エラーの場合はfalse</returns>
        public static bool registDb(Dao.searchResult registInfo, bool isUpdate, IFormFile[] inputStream, ComUtilDb.CommonDBManager db)
        {
            // SQL文
            string sqlName;

            if (isUpdate)
            {
                // 更新の場合
                if (registInfo.AttachmentTypeNo == AttachmentType.File && inputStream == null)
                {
                    // ファイル情報以外更新
                    sqlName = SqlName.UpdateAttachmentInfoExceptFile;
                }
                else
                {
                    // ファイル情報も含めすべて更新
                    sqlName = sqlName = SqlName.UpdateAttachmentInfo;
                }
            }
            else
            {
                // 新規登録の場合
                sqlName = SqlName.Detail.InsertAttachmentInfo;
            }

            // 登録SQL実行
            bool result = TMQUtil.SqlExecuteClass.Regist(sqlName, SqlName.SubDir, registInfo, db);
            return result;
        }

        /// <summary>
        /// 拡張テーブルの添付種類番号を取得する
        /// </summary>
        /// <param name="attachmentTyprStructureId">添付種類の構成ID</param>
        /// <returns>添付種類番号</returns>
        private int getAttachmentTypeNo(int attachmentTyprStructureId)
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = condAttachmentType.StructureGroupId;
            //連番
            param.Seq = condAttachmentType.Seq;

            // 機能レベル取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> attachmentTypeList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            foreach (var attachmentType in attachmentTypeList)
            {
                if (attachmentTyprStructureId == attachmentType.StructureId)
                {
                    return int.Parse(attachmentType.ExData);
                }
            }
            return ComConsts.RETURN_RESULT.NG;
        }
        #endregion
    }
}