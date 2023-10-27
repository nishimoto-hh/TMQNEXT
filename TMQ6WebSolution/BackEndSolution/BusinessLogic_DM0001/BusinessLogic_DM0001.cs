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
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using CommonDM = BusinessLogic_DM0002.BusinessLogic_DM0002;
using CommonDaoDM = BusinessLogic_DM0002.BusinessLogicDataClass_DM0002;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_DM0001
{
    /// <summary>
    /// 文書管理
    /// </summary>
    public partial class BusinessLogic_DM0001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>
            /// 一覧画面SQL
            /// </summary>
            public static class List
            {
                /// <summary>
                /// 一覧検索SQL
                /// </summary>
                public const string AttachmentList = "AttachmentList";
                /// <summary>
                /// 一時テーブルに構成ID(地区)を追加するSQL
                /// </summary>
                public const string InsertLocationStructureIdToTemp = "InsertLocationStructureIdToTemp";
                /// <summary>
                /// 一時テーブルに構成ID(職種)を追加するSQL
                /// </summary>
                public const string InsertJobStructureIdToTemp = "InsertJobStructureIdToTemp";
            }
        }

        /// <summary>
        /// 機能のコントロール情報
        /// </summary>
        /// <remarks>画面の規模が大きくなるとどのコントロールがどのフォームか分からない、特に改修時</remarks>
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
                    /// 一覧
                    /// </summary>
                    public const string List = "BODY_020_00_LST_0";
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_DM0001() : base()
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
        /// 登録/更新処理
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
            var deleteList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormList.ControlId.List);

            // 排他チェック
            if (!checkExclusiveList(ConductInfo.FormList.ControlId.List, deleteList))
            {
                // 排他エラー
                return ComConsts.RETURN_RESULT.NG;
            }

            // 削除SQL取得
            if (!TMQUtil.GetFixedSqlStatement(CommonDM.SqlName.SubDir, CommonDM.SqlName.DeleteAttachmentInfo, out string sql))
            {
                setError();
                return ComConsts.RETURN_RESULT.NG;
            }

            // 行削除
            foreach (var deleteRow in deleteList)
            {
                CommonDaoDM.searchResult delCondition = new();
                SetDeleteConditionByDataClass(deleteRow, ConductInfo.FormList.ControlId.List, delCondition);

                int result = this.db.Regist(sql, delCondition);
                if (result < 0)
                {
                    // 削除エラー
                    setError();
                    return ComConsts.RETURN_RESULT.NG;
                }
            }

            // 一覧画面再検索処理
            searchList(true);

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

            // 登録・ファイルアップロード処理実行
            resultRegist = executeRegistList();

            // 登録処理結果によりエラー処理を行う
            if (!resultRegist)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // メッセージが設定されていない場合
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「更新処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911100002 });
                }
                return ComConsts.RETURN_RESULT.NG;
            }
            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「更新処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911100002 });
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
        /// 一覧画面検索処理
        /// </summary>
        /// <param name="isReSearch">再検索フラグ</param>
        /// <returns>エラーの場合False</returns>
        private bool searchList(bool isReSearch = false)
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(CommonDM.SqlName.SubDir, SqlName.List.AttachmentList, out string baseSql);
            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(CommonDM.SqlName.SubDir, SqlName.List.AttachmentList, out string withSql);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
            {
                return false;
            }

            // 地区IDを取得して一時テーブルに登録する
            addStructureIdToTemp();

            //SQLパラメータに言語ID設定
            whereParam.LanguageId = this.LanguageId;
            whereParam.CommomFactoryId = TMQConsts.CommonFactoryId;
            // SQL、WHERE句、WITH句より件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);

            if (!isReSearch)
            {
                // 総件数を取得
                int cnt = db.GetCount(executeSql.ToString(), whereParam);
                // 総件数のチェック
                if (!CheckSearchTotalCount(cnt, pageInfo))
                {
                    SetSearchResultsByDataClass<CommonDaoDM.searchResult>(pageInfo, null, cnt, isDetailConditionApplied);
                    return false;
                }
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY attachment_date desc, conduct_name, document_type_structure_id, document_no");
            // 一覧検索実行
            IList<CommonDaoDM.searchResult> results = db.GetListByDataClass<CommonDaoDM.searchResult>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 固有の処理があれば実行
            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<CommonDaoDM.searchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<CommonDaoDM.searchResult>(pageInfo, results, results.Count, isDetailConditionApplied))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        private void addStructureIdToTemp()
        {
            // 場所階層：場所階層リストから地区IDを取得して一時テーブルに登録する
            // 職種：「0」を登録(予備品地図の職種用)

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(CommonDM.SqlName.SubDir, SqlName.List.InsertLocationStructureIdToTemp, out string insertLocarionSql); // 場所階層
            TMQUtil.GetFixedSqlStatement(CommonDM.SqlName.SubDir, SqlName.List.InsertJobStructureIdToTemp, out string insertJobSql);           // 職種

            this.db.Regist(insertLocarionSql);
            this.db.Regist(insertJobSql);
        }

        /// <summary>
        /// 一覧画面　更新処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistList()
        {
            // 排他チェック
            if (isErrorExclusive(ConductInfo.FormList.ControlId.List))
            {
                return false;
            }

            DateTime now = DateTime.Now;
            // 入力された内容を取得
            CommonDaoDM.searchResult registInfo = getRegistInfo<CommonDaoDM.searchResult>(ConductInfo.FormList.ControlId.List, now);

            // 登録情報を設定
            registInfo.AttachmentUserId = this.UserId;                    // 作成者ID
            registInfo.AttachmentUserName = CommonDM.getUserName(this.UserId, this.db);     // 作成者
            registInfo.AttachmentTypeNo = getAttachmentTypeNo(registInfo.AttachmentTypeStructureId);

            // 添付種類が「リンク」の場合
            if (registInfo.AttachmentTypeNo == CommonDM.AttachmentType.Link)
            {
                registInfo.FileName = registInfo.Link;
            }

            // 入力チェック
            if (isErrorRegist(ref registInfo, ConductInfo.FormList.ControlId.List))
            {
                return false;
            }

            // アップロード処理
            CommonDM.uploadFile(ref registInfo, this.InputStream);

            // 登録
            if (!CommonDM.registDb(registInfo, true, this.InputStream, this.db))
            {
                return false;
            }

            return true;
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
        /// 入力チェック
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="ctrlId">対象一覧のコントロールID</param>
        /// <returns>エラーの場合true</returns>
        private bool isErrorRegist(ref CommonDaoDM.searchResult registInfo, string ctrlId)
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

            bool checkErrorRegist(ref List<Dictionary<string, object>> errorInfoDictionary, ref CommonDaoDM.searchResult registInfo, string ctrlId)
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
                if (registInfo.AttachmentTypeNo == CommonDM.AttachmentType.Link)
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
                if (!CommonDM.createUploadPath(ref registInfo, this.LanguageId, this.db))
                {
                    // 「アップロード先の取得に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID941010001 });
                    return true;
                }

                // ファイル名を設定
                registInfo.FileName = CommonDM.getFileName(registInfo.FilePath, this.InputStream);

                // ファイル名が指定された文字数より多い場合はエラー
                if (registInfo.FileName.Length > CommonDM.LengthOfFile.FileName)
                {
                    // 「ファイル名が設定桁数を超えています。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941060014, ComRes.ID.ID111280020 });
                    errorInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    return true;
                }

                // アップロード先のパス(絶対パス)が指定された文字数より多い場合はエラー
                if (Path.GetFullPath(registInfo.FilePath + "\\" + registInfo.FileName).Length > CommonDM.LengthOfFile.Path)
                {
                    // 「アップロード先のパスが設定桁数を超えています。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060014, ComRes.ID.ID941010002 });
                    return true;
                }

                return false;
            }

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
                result[attachmentIdVal] = CommonDM.IdIsNone;
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
        /// 拡張テーブルの添付種類番号を取得する
        /// </summary>
        /// <param name="attachmentTyprStructureId">添付種類の構成ID</param>
        /// <returns>添付種類番号</returns>
        private int getAttachmentTypeNo(int attachmentTyprStructureId)
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = CommonDM.condAttachmentType.StructureGroupId;
            //連番
            param.Seq = CommonDM.condAttachmentType.Seq;

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