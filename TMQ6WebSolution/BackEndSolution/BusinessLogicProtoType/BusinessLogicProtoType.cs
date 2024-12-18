using CommonSTDUtil.CommonBusinessLogic;
using CommonSTDUtil.CommonDBManager;
using CommonWebTemplate;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System;
using System.Collections;

using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using CommonSTDUtil.CommonLogger;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using CommonWebTemplate.Models.Common;
using CommonWebTemplate.CommonDefinitions;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLogicProtoType
{
    public class BusinessLogicProtoType
    {
        #region private変数
        /// <summary>ルート物理パス</summary>
        protected string rootPath;
        /// <summary>実行条件(Dictionary)</summary>
        protected List<Dictionary<string, object>> conditionDictionary;
        protected List<Dictionary<string, object>> searchConditionDictionary;
        protected List<Dictionary<string, object>> resultInfoDictionary;
        protected List<Dictionary<string, object>> updateInfoDictionary;
        /// <summary>ページ情報リスト</summary>
        protected List<PageInfo> pageInfoList;

        /// <summary>DB操作クラス</summary>
        private CommonSTDUtil.CommonDBManager.CommonDBManager db;

        /// <summary>共通クラス</summary>
        private CommonSTDUtil.CommonSTDUtil.CommonSTDUtil commonSTDUtil;

        /// <summary>ログ出力</summary>
        private static CommonLogger logger = CommonLogger.GetInstance("logger");
        protected static CommonMemoryData comMemoryData = CommonMemoryData.GetInstance();
        #endregion

        #region コンストラクタ
        public BusinessLogicProtoType()
        {
        }
        #endregion

        #region プロパティ
        /// <summary>端末IPアドレス</summary>
        public string TerminalNo { get; set; }
        /// <summary>工場ID</summary>
        public string FactoryId { get; set; }
        /// <summary>機能ID</summary>
        public string ConductId { get; set; }
        /// <summary>Form番号</summary>
        public short FormNo { get; set; }
        /// <summary>登録者ID</summary>
        public string UserId { get; set; }
        /// <summary>コントロールID</summary>
        public string CtrlId { get; set; }
        /// <summary>アクション区分</summary>
        public short ActionKbn { get; set; }
        /// <summary>言語ID</summary>
        public string LanguageId { get; set; }
        /// <summary>入力ファイル情報(Stream)</summary>
        public Stream[] InputStream { get; protected set; }
        /// <summary>実行結果(JSON文字列)</summary>
        public string JsonResult { get; protected set; }
        /// <summary>GUID</summary>
        public string GUID { get; set; }

        /// <summary>ステータス</summary>
        protected int Status { get; set; }
        /// <summary>メッセージコード</summary>
        protected string MsgId { get; set; }
        /// <summary>ログ問合せ番号</summary>
        protected string LogNo { get; set; }
        /// <summary>出力ファイル情報(Stream)</summary>
        public Stream OutputStream { get; protected set; }
        #endregion

        #region publicメソッド
        /// <summary>
        /// ビジネスロジック実行
        /// </summary>
        /// <param name="inParameter">
        /// INパラメータ
        /// <para>TerminalNo:端末IPアドレス</para>
        /// <para>ConductId：機能ID</para>
        /// <para>FormNo：Form番号</para>
        /// <para>UserId：登録者ID</para>
        /// <para>CtrlId：起動処理名</para>
        /// <para>ActionKbn：アクション区分</para>
        /// <para>RootPath：ルート物理パス</para>
        /// <para>Status：ステータス(0:正常終了　1000999:異常終了)</para>
        /// <para>JsonCondition：実行条件(JSON文字列)</para>
        /// <para>JsonPageInfo：ページ情報(JSON文字列)</para>
        /// <para>JsonResult：登録データ(JSON文字列)</para>
        /// <para>JsonUpdateInfo：更新列情報(JSON文字列)</para>
        /// <para>InputStream：入力ファイル情報(Stream)</para>
        /// </param>
        /// <param name="outParameter">
        /// OUTパラメータ
        /// <para>Status：ステータス(0:正常終了　1000999:異常終了)</para>
        /// <para>MsgId：メッセージコード</para>
        /// <para>LogNo：ログ問合せ番号</para>
        /// <para>JsonResult：実行結果(JSON文字列)</para>
        /// <para>OutputStream：出力ファイル情報(Stream)</para>
        /// </param>
        /// <returns>実行結果</returns>
        //public int ExecuteBusinessLogic(dynamic inParam, out dynamic outParam)
        //{

        //    // アクセスログ出力
        //    WriteAccessLog(inParam);

        //    this.Status = CommonProcReturn.ProcStatus.Valid;
        //    this.MsgId = string.Empty;
        //    this.LogNo = string.Empty;
        //    this.JsonResult = string.Empty;
        //    this.OutputStream = null;

        //    int result = 0;
        //    dynamic inPrm = (ExpandoObject)inParam;
        //    outParam = new ExpandoObject();
        //    outParam.Status = this.Status;
        //    outParam.MsgId = this.MsgId;
        //    outParam.LogNo = this.LogNo;
        //    outParam.JsonResult = this.JsonResult;
        //    outParam.OutputStream = this.OutputStream;

        //    try
        //    {
        //        // パラメータチェック
        //        if (!CheckParameters(inParam))
        //        {
        //            outParam.Status = this.Status;
        //            outParam.MsgId = this.MsgId;
        //            outParam.LogNo = this.LogNo;
        //            return -1;
        //        }

        //        // JSON文字列の実行条件をデシリアライズ

        //        // 検索条件取得
        //        if (IsPropertyExists(inParam, "JsonCondition") && !string.IsNullOrEmpty(inParam.JsonCondition))
        //        {
        //            this.searchConditionDictionary = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(inParam.JsonCondition);
        //        }
        //        else
        //        {
        //            this.searchConditionDictionary = new List<Dictionary<string, object>>();
        //        }

        //        // ページ情報取得
        //        if (IsPropertyExists(inParam, "JsonPageInfo") && !string.IsNullOrEmpty(inParam.JsonPageInfo))
        //        {
        //            this.pageInfoList = PageInfo.GetPageInfoList(JsonSerializer.Deserialize<List<Dictionary<string, object>>>(inParam.JsonPageInfo));
        //        }
        //        else
        //        {
        //            this.pageInfoList = new List<PageInfo>();
        //        }

        //        try
        //        {
        //            // ログイン認証
        //            if (this.CtrlId.Equals("LoginAuthentication"))
        //            {
        //                result = this.GetLoginAuthentication(inParam.UserId);
        //            }

        //            // コンボ生成
        //            if (this.CtrlId.Equals("CTRLSQL"))
        //            {
        //                result = this.ExecKanriSql();
        //            }

        //            // 権限チェック
        //            if (this.CtrlId.Equals("CheckAuthority"))
        //            {
        //                result = this.CheckAuthority();
        //            }

        //            // リソース翻訳
        //            if (this.CtrlId.Equals("GetResourcesValue"))
        //            {
        //                result = this.GetResourcesValue();
        //            }

        //            // 構成リスト取得
        //            if (this.CtrlId.Equals("GetStructureList"))
        //            {
        //                result = this.GetStructureList(inParam.UserId);
        //            }

        //            if (this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.DataGet ||
        //                this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Search)
        //            {
        //                // CSVファイルからデータを取得
        //                this.GetCsvFileData();
        //            }

        //            if (this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComInitForm &&
        //                (this.CtrlId.ToUpper().StartsWith("BACK") ||
        //                 this.FormNo > 0))
        //            {
        //                this.GetCsvFileData();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Error(ex.Message);
        //            outParam.Status = CommonProcReturn.ProcStatus.Error;
        //            outParam.MsgId = "<処理の実行に失敗しました。>";
        //            outParam.LogNo = string.Empty;

        //            return -1;
        //        }

        //        outParam.Status = this.Status;
        //        outParam.MsgId = this.MsgId;
        //        outParam.LogNo = this.LogNo;
        //        outParam.JsonResult = this.JsonResult;

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        outParam.Status = CommonProcReturn.ProcStatus.Error;
        //        outParam.MsgId = "<ビジネスロジックの実行の受け付けに失敗しました。>";

        //        return -2;
        //    }
        //}
        public int ExecuteBusinessLogic(CommonProcParamIn inParam, out CommonProcParamOut outParam)
        {

            // アクセスログ出力
            writeAccessLog(inParam);

            this.Status = CommonProcReturn.ProcStatus.Valid;
            this.MsgId = string.Empty;
            this.LogNo = string.Empty;
            this.JsonResult = string.Empty;
            this.OutputStream = null;

            int result = 0;
            outParam = new CommonProcParamOut();

            try
            {
                // パラメータチェック
                if (!CheckParameters(inParam))
                {
                    outParam.Status = this.Status;
                    outParam.MsgId = this.MsgId;
                    outParam.LogNo = this.LogNo;
                    return -1;
                }

                // JSON文字列の実行条件をデシリアライズ

                // 検索条件取得
                if (inParam.ConditionList != null && inParam.ConditionList.Count > 0)
                {
                    this.searchConditionDictionary =
                        CommonUtil.JsonElementDictionaryListToObjectDictionaryList(inParam.ConditionList);
                }
                else
                {
                    this.searchConditionDictionary = new List<Dictionary<string, object>>();
                }

                // ページ情報取得
                if (inParam.PageInfoList != null)
                {
                    this.pageInfoList = inParam.PageInfoList;
                }
                else
                {
                    this.pageInfoList = new List<PageInfo>();
                }

                try
                {
                    // ログイン認証
                    if (this.CtrlId.Equals("LoginAuthentication"))
                    {
                        result = this.GetLoginAuthentication(inParam.LoginId);
                    }

                    // コンボ生成
                    if (this.CtrlId.Equals("CTRLSQL"))
                    {
                        result = this.ExecKanriSql();
                    }

                    // 権限チェック
                    if (this.CtrlId.Equals("CheckAuthority"))
                    {
                        result = this.CheckAuthority();
                    }

                    // リソース翻訳
                    if (this.CtrlId.Equals("GetResourcesValue"))
                    {
                        result = this.GetResourcesValue();
                    }

                    // 構成リスト取得
                    if (this.CtrlId.Equals("GetStructureList"))
                    {
                        result = this.GetStructureList(inParam.BelongingInfo, inParam.UserId, this.LanguageId);
                    }

                    if (this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.DataGet ||
                        this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Search)
                    {
                        // CSVファイルからデータを取得
                        this.GetCsvFileData();
                    }

                    if (this.ActionKbn == LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComInitForm &&
                        (this.CtrlId.ToUpper().StartsWith("BACK") ||
                         this.FormNo > 0))
                    {
                        this.GetCsvFileData();
                    }
                }
                catch (Exception ex)
                {
                    outParam.Status = CommonProcReturn.ProcStatus.Error;
                    outParam.MsgId = "<処理の実行に失敗しました。>";
                    outParam.LogNo = string.Empty;
                    logger.ErrorLog(inParam.FactoryId, inParam.UserId, outParam.MsgId, ex);

                    return -1;
                }

                outParam.Status = this.Status;
                outParam.MsgId = this.MsgId;
                outParam.LogNo = this.LogNo;
                outParam.ResultList = this.resultInfoDictionary;

                return result;
            }
            catch (Exception ex)
            {
                outParam.Status = CommonProcReturn.ProcStatus.Error;
                outParam.MsgId = "<ビジネスロジックの実行の受け付けに失敗しました。>";
                logger.ErrorLog(inParam.FactoryId, inParam.UserId, outParam.MsgId, ex);

                return -2;
            }
        }
        #endregion

        #region private処理
        /// <summary>
        /// アクセスログ出力
        /// </summary>
        /// <param name="msg"></param>
        private void writeAccessLog(CommonProcParamIn param)
        {
            logger.ActionLog(param.FactoryId, param.UserId, param.ConductId, param.CtrlId);
        }

        /// <summary>
        /// csvファイルデータ読み込み処理
        /// </summary>
        /// <param name="formNo">画面NO</param>
        /// <param name="jsonCondition">検索条件</param>
        /// <returns></returns>
        private int GetCsvFileData()
        {
            var resultList = new List<Dictionary<string, object>>();

            var csvFileName = this.ConductId + ".csv";
            var csvFilePath = string.Format(this.rootPath + "csv\\{0}", csvFileName);

            var list = new List<dynamic>();
            var headerList = new List<string>();

            if (File.Exists(csvFilePath))
            {
                // コントロールIDを取得
                var ctrlIdList = this.GetCtrlIdByFormNo(this.FormNo.ToString(), csvFilePath);

                foreach (var ctrlId in ctrlIdList)
                {
                    // 初期化
                    list = new List<dynamic>();

                    // ページング情報を取得
                    int index = 0; // デフォルトを「0」で設定
                    foreach (var pageInfoDetail in this.pageInfoList)
                    {
                        if (pageInfoDetail.CtrlId == ctrlId)
                        {
                            break;
                        }
                        index++;
                    }
                    if (index == this.pageInfoList.Count) { continue; }

                    // 検索条件を検索条件オブジェクトへ設定
                    var pageInfo = this.pageInfoList.Count > 0 ? this.pageInfoList[index] : null;
                    dynamic conditionObj = new ExpandoObject();
                    SetSearchCondition(conditionObj, pageInfo);

                    // 総件数を取得
                    int cnt = this.GetCsvFileDataCnt(ctrlId, this.FormNo.ToString(), csvFilePath, ref list, ref headerList);
                    //if (cnt > 0)
                    //{
                    //    if (this.Status != CommonProcReturn.ProcStatus.Confirm)
                    //    {
                    //        // ステータスが確認後でない場合はMAX件数のチェックを行う
                    //        if (pageInfo != null && pageInfo.MaxCnt > 0)
                    //        {
                    //            if (cnt > pageInfo.MaxCnt)
                    //            {
                    //                // MAX件数を超えている場合、確認メッセージを設定して処理を終了
                    //                this.Status = CommonProcReturn.ProcStatus.Confirm;
                    //                this.MsgId = "データ件数が最大件数を超えています。表示しますか？";
                    //                this.LogNo = "999";
                    //                return -1;
                    //            }
                    //        }
                    //    }

                    //    // 取得結果を反映
                    //    resultList.AddRange(ConvertToSearchResultDictionary(ctrlId, conditionObj, list, headerList, cnt));
                    //}

                    // 取得結果を反映
                    resultList.AddRange(ConvertToSearchResultDictionary(ctrlId, conditionObj, list, headerList, cnt));
                }

                // 検索結果をJSON文字列へシリアライズ
                //this.JsonResult = SerializeToJson(resultList);
                this.resultInfoDictionary = resultList;
            }

            return 0;
        }
        #endregion

        #region ログイン認証
        public int GetLoginAuthentication(string userId)
        {
            var dicList = new Dictionary<string, object>();
            var list = new List<Dictionary<string, object>>();

            try
            {
                // DB接続
                db = new CommonSTDUtil.CommonDBManager.CommonDBManager(this.rootPath, logger, this.FactoryId, userId);
                if (!db.Connect())
                {
                    return -1;
                }

                // パラメータを分割する
                var userPassword = "";
                var passwordCheckFlg = "";
                var languageId = "";

                foreach (var dic in this.searchConditionDictionary)
                {
                    // パスワード
                    if (dic.ContainsKey("userPassword"))
                    {
                        userPassword = dic["userPassword"].ToString();
                    }

                    // パスワードチェックフラグ
                    if (dic.ContainsKey("passwordCheckFlg"))
                    {
                        passwordCheckFlg = dic["passwordCheckFlg"].ToString();
                    }
                }

                // 担当者マスタから言語IDを取得
                //string sql = "select language_id from login where tanto_cd = '" + userId + "'";
                //dynamic resultLogin = db.GetEntity(sql);
                //languageId = (resultLogin == null) ? "ja" : resultLogin.language_id;
                languageId = "ja";

                dicList.Add("result", "0");            // 認証結果(先頭に領域確保)
                dicList.Add("userName", userId);       // ユーザ名名称
                dicList.Add("languageId", languageId); // 言語ID
                dicList.Add("userId", userId); // ユーザID

                // 機能IDのリストを返却
                string sql = string.Empty;
                //sql = "select distinct conductid from com_conduct_mst where menudisp = 1 order by conductid";
                sql = "select distinct conduct_id as conductid from cm_conduct where menu_division = 1 order by conduct_id";
                dynamic results = db.GetList(sql);
                List<string> conductList = new List<string>();

                foreach (var result in results)
                {
                    conductList.Add(result.conductid);
                }

                // 機能IDのリストを返却
                dicList.Add("conductIdList", conductList);

                //TMQカスタマイズ
                //工場IDのリストを取得
                List<int> factoryIdList = new List<int>();
                sql = "select distinct factory_id as factoryid from ms_structure where factory_id is not null order by factory_id";
                dynamic factorys = db.GetList(sql);
                foreach (var factory in factorys)
                {
                    factoryIdList.Add(factory.factoryid);
                }
                dicList.Add("factoryIdList", factoryIdList); // 所属工場ID

                dicList["result"] = "1"; // 認証結果(正常終了)

                // GUIDを生成
                string guid = Guid.NewGuid().ToString("D");
                dicList.Add("guid", guid);  // GUID
            }
            catch (Exception ex)
            {
                logger.ErrorLog("", userId, "", ex);
                dicList = new Dictionary<string, object>();
                dicList.Add("result", "0");

                return -1;
            }
            finally
            {
                list.Add(dicList);
                //this.JsonResult = this.SerializeToJson(list);
                this.resultInfoDictionary = list;

                if (db != null)
                {
                    db.Close();
                }
            }
            return 0;
        }
        #endregion

        #region オートコンプリート、コンボボックス用SQL実行
        /// <summary>
        /// オートコンプリート、コンボボックス用SQL実行
        /// </summary>
        public int ExecKanriSql()
        {
            // パラメータを取得する
            var ctrlSQLId = "";

            var dicList = new Dictionary<string, object>();
            var list = new List<Dictionary<string, object>>();

            // 検索結果名のペアを生成(Key:検索結果SQL側カラム名、Value:共通FW側カラム名
            var resultNamesPair = new Dictionary<string, string>
            {
                { "ivalues", "VALUE1" },
                { "labels", "VALUE2" }
            };

            try
            {
                // 条件を設定する
                foreach (var dic in this.searchConditionDictionary)
                {
                    // キーコード
                    if (dic.ContainsKey("CTRLSQLID"))
                    {
                        ctrlSQLId = dic["CTRLSQLID"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(ctrlSQLId))
                {
                    // DB接続
                    db = new CommonSTDUtil.CommonDBManager.CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                    if (!db.IsOpened)
                    {
                        if (!db.Connect())
                        {
                            return -1;
                        }
                    }

                    // SQL文生成
                    var sql = "";
                    sql = sql + "select item_value as ivalues, ";
                    sql = sql + "       item_label as labels ";
                    sql = sql + "  from com_trns_mst ";
                    sql = sql + " where transkey = @CtrlSQLId ";
                    if (db.DbType == CommonDBManager.DBType.SQLServer)
                    {
                        sql = sql + "   and delflg = 0 ";
                    }
                    else
                    {
                        sql = sql + "   and delflg = false ";
                    }
                    sql = sql + "order by transorder";
                    // SQL実行
                    dynamic results = db.GetList(sql, new { CtrlSQLId = ctrlSQLId });

                    if (results != null && results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            // 初期化
                            dicList = new Dictionary<string, object>();

                            IDictionary<string, object> dicResult = result as IDictionary<string, object>;
                            foreach (var item in resultNamesPair)
                            {
                                dicList.Add(item.Value, dicResult[item.Key]);
                            }
                            list.Add(dicList);
                        }
                    }

                    //this.JsonResult = this.SerializeToJson(list);
                    this.resultInfoDictionary = list;

                }
                return 0;
            }
            catch (Exception ex)
            {
                logger.ErrorLog(this.FactoryId, this.UserId, "", ex);
                return -1;
            }
            finally
            {
                if (db != null)
                {
                    db.Close();
                }
            }
        }
        #endregion

        #region 権限チェック
        /// <summary>
        /// 権限チェック
        /// </summary>
        /// <param name="inParam"></param>
        /// <param name="outParam"></param>
        /// <returns></returns>
        private int CheckAuthority()
        {

            var list = new List<Dictionary<string, object>>();

            foreach (var dic in this.searchConditionDictionary)
            {
                var dicList = new Dictionary<string, object>();

                // コントロールID
                if (dic.ContainsKey("ctrlId"))
                {
                    // 機能IDのリストを返却
                    dicList.Add("ctrlId", dic["ctrlId"].ToString());
                    dicList.Add("status", "1");     // 1：表示、活性
                    list.Add(dicList);
                }
            }

            //this.JsonResult = this.SerializeToJson(list);
            this.resultInfoDictionary = list;

            return 0;
        }
        #endregion

        #region 翻訳取得処理
        /// <summary>
        /// 翻訳取得処理
        /// </summary>
        /// <param name="inParam"></param>
        /// <param name="outParam"></param>
        /// <returns></returns>
        private int GetResourcesValue()
        {
            commonSTDUtil = new CommonSTDUtil.CommonSTDUtil.CommonSTDUtil();

            var list = new List<Dictionary<string, object>>();
            var dicList = new Dictionary<string, object>();

            // 変数定義
            string[] keys = null;
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return -1;
                }

                if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                {
                    foreach (var dic in this.searchConditionDictionary)
                    {
                        // メッセージIDを取得する
                        if (dic.ContainsKey("name"))
                        {
                            //var keyList = JsonSerializer.Deserialize<List<string>>(dic["name"].ToString());
                            var keyList = dic["name"] as List<string>;

                            keys = new string[keyList.Count];

                            for (int i = 0; i < keyList.Count; i++)
                            {
                                keys[i] = keyList[i].ToString();
                            }
                        }
                    }

                    // 翻訳処理を行う
                    if (keys != null && keys.Count() > 0)
                    {
                        for (int i = 0; i < keys.Count(); i++)
                        {
                            dicList = new Dictionary<string, object>();
                            string message = ComUtil.GetPropertiesMessage(key: keys[i], languageId: this.LanguageId, db: this.db);
                            if (!string.IsNullOrEmpty(message))
                            {
                                dicList.Add(keys[i], ComUtil.GetPropertiesMessage(key: keys[i], languageId: this.LanguageId, db: this.db));
                                list.Add(dicList);
                            }
                        }
                    }
                    else
                    {
                        // パラメータが取得できない場合、全ての翻訳を取得
                        list = ComUtil.GetPropertiesAllMessage(this.LanguageId, this.db);
                    }

                }
                else
                {
                    // パラメータが未設定の場合、全ての翻訳を取得
                    list = ComUtil.GetPropertiesAllMessage(this.LanguageId, this.db);
                }
            }
            finally
            {
                if (this.db != null)
                {
                    this.db.Close();
                }
            }

            // JSON文字列に変換
            if (list != null && list.Count > 0)
            {
                //this.JsonResult = this.SerializeToJson(list);
                this.resultInfoDictionary = list;
            }

            return 0;
        }
        #endregion

        #region 構成リスト取得
        /// <summary>
        /// 構成リスト取得
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="languageId">言語ID</param>
        /// <returns></returns>
        private int GetStructureList(BelongingInfo belongingInfo, string userId, string languageId)
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return -1;
                }

                var list = new List<Dictionary<string, object>>();

                if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                {
                    int result = ComUtil.GetStructureList(this.db, belongingInfo, userId, languageId, this.searchConditionDictionary, ref list);
                    if (result < 0)
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return -1;
                    }

                    if (list != null && list.Count > 0)
                    {
                        //this.JsonResult = this.SerializeToJson(list);
                        this.resultInfoDictionary = list;
                    }
                }
            }
            finally
            {
                if (this.db != null)
                {
                    this.db.Close();
                }
            }

            return 0;
        }
        #endregion

        #region csvファイルデータ読み込み処理
        /// <summary>
        /// 画面NO別コントロールID取得処理
        /// </summary>
        /// <param name="formNo"></param>
        /// <param name="csvFilePath"></param>
        /// <param name="headerList"></param>
        /// <returns></returns>
        protected List<string> GetCtrlIdByFormNo(string formNo, string csvFilePath)
        {
            // 読み込み行番号
            int rowNo = 0;

            // 配列格納位置
            int eCtrlId = -1;
            int eFormNo = -1;
            int eDataType = -1;

            // ヘッダ行退避配列宣言
            var headerList = new List<string>();

            // コントロールID格納配列を宣言
            var ctrlIdList = new List<string>();

            // csvファイルデータ取得
            using (StreamReader sr = new StreamReader(@csvFilePath, System.Text.Encoding.GetEncoding("Shift_JIS")))
            {

                // 末尾まで繰り返す
                while (!sr.EndOfStream)
                {
                    rowNo++; // 行番号を加算

                    // csvファイルを１行読み込む
                    string line = sr.ReadLine();

                    // 読み込んだ１行をカンマ毎に分けて配列に格納する
                    string[] values = line.Split(',');

                    // 配列からリストに格納する
                    var lists = new List<string>();
                    lists.AddRange(values);

                    // ダブルクォーテーションを考慮し、配列を再生成する
                    lists = this.removeDoubleQuotation(lists);

                    if (rowNo == 1)
                    {
                        // ヘッダ行を退避する
                        headerList = lists;

                        // コントロールID、画面NOの格納位置を取得する
                        for (int i = 0; i < headerList.Count; i++)
                        {
                            switch (headerList[i].ToUpper())
                            {
                                case "CTRLID":
                                    eCtrlId = i;
                                    break;
                                case "FORMNO":
                                    eFormNo = i;
                                    break;
                                case "DATATYPE":
                                    eDataType = i;
                                    break;
                                default:
                                    break;
                            }

                            // コントロール、画面NO、データタイプの格納位置を取得できた場合は、処理を行う
                            if (eCtrlId != -1 && eFormNo != -1 && eDataType != -1)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        // コントロール、画面NO、データタイプの格納位置が取得できていない場合は、後続の処理を行わず、0件で処理を抜ける
                        if (rowNo == 2 && (eCtrlId == -1 || eFormNo == -1 || eDataType == -1))
                        {
                            return null;
                        }

                        // 画面NO、データタイプ=1(画面入力値)が一致する場合、カウントする
                        if (lists[eFormNo] == formNo && lists[eDataType].ToString() == TMPTBL_CONSTANTS.DATATYPE.Input.ToString())
                        {
                            // コントロールIDが含んでいない場合、追加を行う
                            if (!ctrlIdList.Contains(lists[eCtrlId]))
                            {
                                ctrlIdList.Add(lists[eCtrlId]);
                            }
                        }
                    }
                }
            }

            return ctrlIdList;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// CSVファイルデータカウント取得
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="formNo">画面NO</param>
        /// <param name="csvFilePath">CSVファイルパス</param>
        /// <param name="resultList">結果データ格納リスト</param>
        /// <param name="headerList">データヘッダ格納リスト</param>
        /// <returns></returns>
        protected int GetCsvFileDataCnt(string ctrlId, string formNo, string csvFilePath, ref List<dynamic> resultList, ref List<string> headerList)
        {
            // 読み込み行番号
            int rowNo = 0;

            // データカウント
            int dataCnt = 0;

            // 配列格納位置
            int eCtrlId = -1;
            int eFormNo = -1;
            int eDataType = -1;

            // csvファイルデータ取得
            using (StreamReader sr = new StreamReader(@csvFilePath, System.Text.Encoding.GetEncoding("Shift_JIS")))
            {

                // 末尾まで繰り返す
                while (!sr.EndOfStream)
                {
                    rowNo++; // 行番号を加算

                    // csvファイルを１行読み込む
                    string line = sr.ReadLine();

                    // 読み込んだ１行をカンマ毎に分けて配列に格納する
                    string[] values = line.Split(',');

                    // 配列からリストに格納する
                    var lists = new List<string>();
                    lists.AddRange(values);

                    // ダブルクォーテーションを考慮し、配列を再生成する
                    lists = this.removeDoubleQuotation(lists);
                    for (int i = 0; i < lists.Count(); i++)
                    {
                        // 最初、末尾の文字がシングルクォーテーションの場合除去を行う
                        if (lists[i].StartsWith("\""))
                        {
                            lists[i] = lists[i].Substring(1, lists[i].Length - 1);
                        }
                        if (lists[i].EndsWith("\""))
                        {
                            lists[i] = lists[i].Substring(0, lists[i].Length - 1);
                        }
                    }

                    if (rowNo == 1)
                    {
                        // ヘッダ行を退避する
                        headerList = lists;

                        // コントロールID、画面NOの格納位置を取得する
                        for (int i = 0; i < headerList.Count; i++)
                        {
                            switch (headerList[i].ToUpper())
                            {
                                case "CTRLID":
                                    eCtrlId = i;
                                    break;
                                case "FORMNO":
                                    eFormNo = i;
                                    break;
                                case "DATATYPE":
                                    eDataType = i;
                                    break;
                                default:
                                    break;
                            }

                            // コントロール、画面NO、データタイプの格納位置を取得できた場合は、処理を行う
                            if (eCtrlId != -1 && eFormNo != -1 && eDataType != -1)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        // コントロール、画面NO、データタイプの格納位置が取得できていない場合は、後続の処理を行わず、0件で処理を抜ける
                        if (rowNo == 2 && (eCtrlId == -1 || eFormNo == -1 || eDataType == -1))
                        {
                            return 0;
                        }

                        // コントロール、画面NO、データタイプ=1(画面入力値)が一致する場合、カウントする
                        if (lists[eCtrlId] == ctrlId && lists[eFormNo] == formNo && lists[eDataType].ToString() == TMPTBL_CONSTANTS.DATATYPE.Input.ToString())
                        {
                            dataCnt++;
                            resultList.Add(lists);
                        }
                    }
                }
            }

            return dataCnt;
        }

        /// <summary>
        /// ダブルクォーテーションを考慮し、配列の再生成を行う
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected List<string> removeDoubleQuotation(List<string> list)
        {

            for (int i = 0; i < list.Count; i++)
            {
                // 先頭のスペースを削除して、ダブルクォーテーションが入っているか判定
                if (list[i] != string.Empty && list[i].TrimStart() != string.Empty && list[i].TrimStart()[0] == '"')
                {
                    // もう１回ダブルクォーテーションが出てくるまで要素を結合
                    while (list[i].TrimEnd()[list[i].TrimEnd().Length - 1] != '"')
                    {
                        list[i] = list[i] + "," + list[i + 1];

                        // 結合したら要素を削除する
                        list.RemoveAt(i + 1);
                    }
                }
            }

            return list;
        }

        //protected bool CheckParameters(dynamic param)
        //{
        //    if (!IsPropertyExists(param, "ActionKbn") || param.ActionKbn < 0)
        //    {
        //        if (param.ActionKbn != LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.DataGet) // ※-1はページングの際の処理のため例外的にエラーにしない
        //        {
        //            this.Status = CommonProcReturn.ProcStatus.Error;
        //            this.MsgId = "<パラメータにアクション区分が設定されていません。>";
        //            this.LogNo = string.Empty;
        //            return false;
        //        }
        //    }

        //    this.ActionKbn = param.ActionKbn;

        //    if (this.ActionKbn != LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.None) // 共通処理などが「9999」で来るため
        //    {
        //        if (!IsPropertyExists(param, "ConductId") || string.IsNullOrEmpty(param.ConductId))
        //        {
        //            this.Status = CommonProcReturn.ProcStatus.Error;
        //            this.MsgId = "<パラメータに機能IDが設定されていません。>";
        //            this.LogNo = string.Empty;
        //            return false;
        //        }
        //    }
        //    if (!IsPropertyExists(param, "FormNo") || param.FormNo < 0)
        //    {
        //        this.Status = CommonProcReturn.ProcStatus.Error;
        //        this.MsgId = "<パラメータにフォーム番号が設定されていません。>";
        //        this.LogNo = string.Empty;
        //        return false;
        //    }
        //    if (!IsPropertyExists(param, "CtrlId") || string.IsNullOrEmpty(param.CtrlId))
        //    {
        //        this.Status = CommonProcReturn.ProcStatus.Error;
        //        this.MsgId = "<パラメータにコントロールIDが設定されていません。>";
        //        this.LogNo = string.Empty;
        //        return false;
        //    }
        //    if (!IsPropertyExists(param, "RootPath") || string.IsNullOrEmpty(param.RootPath))
        //    {
        //        this.Status = CommonProcReturn.ProcStatus.Error;
        //        this.MsgId = "<パラメータにルートパスが設定されていません。>";
        //        this.LogNo = string.Empty;
        //        return false;
        //    }

        //    // メンバ変数へパラメータを設定
        //    this.ConductId = param.ConductId;
        //    this.FormNo = param.FormNo;
        //    this.CtrlId = param.CtrlId;
        //    this.rootPath = param.RootPath;
        //    this.LanguageId = param.LanguageId;
        //    if (IsPropertyExists(param, "GUID"))
        //    {
        //        this.GUID = param.GUID;
        //    }
        //    return true;
        //}

        protected bool CheckParameters(CommonProcParamIn param)
        {
            this.FactoryId = param.FactoryId;

            if (!string.IsNullOrEmpty(param.LanguageId))
            {
                this.LanguageId = param.LanguageId;
            }
            else
            {
                // 言語IDが未指定の場合は日本語
                this.LanguageId = "ja";
            }

            if (param.ActionKbn < 0)
            {
                if (param.ActionKbn != LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.DataGet) // ※-1はページングの際の処理のため例外的にエラーにしない
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    this.MsgId = "<パラメータにアクション区分が設定されていません。>";
                    this.LogNo = string.Empty;
                    return false;
                }
            }

            this.ActionKbn = param.ActionKbn;

            if (this.ActionKbn != LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.None) // 共通処理などが「9999」で来るため
            {
                if (string.IsNullOrEmpty(param.ConductId))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    this.MsgId = "<パラメータに機能IDが設定されていません。>";
                    this.LogNo = string.Empty;
                    return false;
                }
            }
            if (param.FormNo < 0)
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                this.MsgId = "<パラメータにフォーム番号が設定されていません。>";
                this.LogNo = string.Empty;
                return false;
            }
            if (string.IsNullOrEmpty(param.CtrlId))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                this.MsgId = "<パラメータにコントロールIDが設定されていません。>";
                this.LogNo = string.Empty;
                return false;
            }
            if (string.IsNullOrEmpty(param.RootPath))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                this.MsgId = "<パラメータにルートパスが設定されていません。>";
                this.LogNo = string.Empty;
                return false;
            }

            // メンバ変数へパラメータを設定
            this.ConductId = param.ConductId;
            this.FormNo = param.FormNo;
            this.CtrlId = param.CtrlId;
            this.rootPath = param.RootPath;
            this.LanguageId = param.LanguageId;
            if (IsPropertyExists(param, "GUID"))
            {
                this.GUID = param.GUID;
            }
            return true;
        }

        /// <summary>
        /// プロパティの存在チェック
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected bool IsPropertyExists(dynamic obj, string name)
        {
            if (obj is ExpandoObject)
                return ((IDictionary<string, object>)obj).ContainsKey(name);

            return obj.GetType().GetProperty(name) != null;
        }

        /// <summary>
        /// JSON文字列へのシリアライズ
        /// </summary>
        /// <param name="dic">Dictionary</param>
        /// <returns>JSON文字列</returns>
        protected string SerializeToJson(List<Dictionary<string, object>> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    //2024.09 .NET8バージョンアップ対応 start
                    //var jsonOptions = new JsonSerializerOptions
                    //{
                    //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    //};
                    //return JsonSerializer.Serialize(list, jsonOptions);
                    return JsonSerializer.Serialize(list, JsonSerializerOptionsDefine.JsOptionsForEncode);
                    //2024.09 .NET8バージョンアップ対応 start
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 検索条件の設定
        /// </summary>
        /// <param name="namesPair">カラム名の変換辞書</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="pageInfo">ページ情報</param>
        protected bool SetSearchCondition(dynamic condition, PageInfo pageInfo)
        {
            if (pageInfo != null && pageInfo.PageSize > 0 && pageInfo.CtrlType != FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn3)
            {
                // ページサイズ指定がある場合、実行条件に追加
                condition.PageSize = pageInfo.PageSize;
                condition.PageNo = pageInfo.PageNo;
                condition.Offset = pageInfo.Offset;
            }
            return true;
        }

        /// <summary>
        /// dynamic型の検索結果をDictionary型へ変換
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="results">検索結果</param>
        /// <param name="cnt">総件数</param>
        /// <returns></returns>
        /// 
        protected List<Dictionary<string, object>> ConvertToSearchResultDictionary(string ctrlId, dynamic condition, dynamic results, List<string> headerList, int cnt = -1)
        //protected List<Dictionary<string, object>> ConvertToSearchResultDictionary(string ctrlId, dynamic results, List<string> headerList, int cnt = -1)
        {
            var list = new List<Dictionary<string, object>>();
            var dicList = new Dictionary<string, object>();
            var rowNo = 0;
            int offset = 0;
            int pageNo = -1;
            int pageSize = -1;

            var val = "";
            var valIndex = 1;

            if (cnt >= 0)
            {
                // パラメータに総件数が設定されている場合、先頭に総件数行を追加
                dicList = new Dictionary<string, object> {
                    { "CTRLID", ctrlId },
                    { "ROWNO", rowNo++ },
                    { "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit },
                    { "VAL1", cnt.ToString() }
                };
                list.Add(dicList);

                // 検索条件から条件を取得
                if (condition != null)
                {
                    if (((IDictionary<string, object>)condition).ContainsKey("Offset"))
                    {
                        offset = condition.Offset;
                    }

                    if (((IDictionary<string, object>)condition).ContainsKey("PageNo"))
                    {
                        pageNo = condition.PageNo;
                    }

                    if (((IDictionary<string, object>)condition).ContainsKey("PageSize"))
                    {
                        pageSize = condition.PageSize;
                    }
                }
            }
            else
            {
                rowNo = 1;
            }

            // ページ情報を設定
            int index = 0; // データ行
            int minRowNo = -1; // 最小行番号
            int maxRowNo = -1; // 最大行番号

            if (pageNo != -1 && pageSize != -1)
            {
                minRowNo = (pageNo - 1) * pageSize;
                maxRowNo = pageNo * pageSize;
            }
            //グループ化データ退避用
            var groupChildList = new List<Dictionary<string, object>>();
            var groupParentList = new Dictionary<string, object>();
            var GroupParentNo = 0;

            foreach (var result in results)
            {
                var groupNo = 0;
                index++;

                if ((minRowNo == -1 && maxRowNo == -1) || (minRowNo < index && maxRowNo >= index))
                {

                    dicList = new Dictionary<string, object> {
                        { "CTRLID", ctrlId },
                        //{ "DATATYPE", TMPTBL_CONSTANTS.DATATYPE.Input },
                        { "ROWNO", offset + rowNo++ },
                        //{ "ROWSTATUS", TMPTBL_CONSTANTS.ROWSTATUS.Edit }
                    };

                    // 初期化
                    val = "";
                    valIndex = 1;
                    for (int i = 0; i < headerList.Count; i++)
                    {
                        val = "VAL" + valIndex.ToString();
                        var elementIndex = -1;

                        if (headerList[i] == "ROWSTATUS")
                        {
                            elementIndex = i;
                            val = "ROWSTATUS";
                        }
                        //else if (headerList[i] == "UPDTAG")
                        //{
                        //    elementIndex = i;
                        //    val = "UPDTAG";
                        //}
                        else if (headerList[i] == val)
                        {
                            elementIndex = i;
                            valIndex++; // 次のVAL値を設定
                        }
                        else if (headerList[i] == "GROUPNO" && result[i] != "0")
                        {
                            groupNo = Int32.Parse(result[i]);
                        }
                        else
                        {
                            elementIndex = -1;
                        }

                        // VALの格納位置を取得できた場合、処理を行う
                        if (elementIndex != -1)
                        {
                            if (groupNo != 0 && GroupParentNo != 0 && GroupParentNo != groupNo && groupParentList.Count > 0)
                            {
                                //グループ化データの場合
                                groupParentList.Add("_children", groupChildList);
                                list.Add(groupParentList);
                                groupParentList = new Dictionary<string, object>();
                                groupChildList = new List<Dictionary<string, object>>();
                            }
                            dicList.Add(val, result[elementIndex]);
                        }
                    }
                    if (groupNo == 0)
                    {
                        list.Add(dicList);
                    }
                    else
                    {
                        if (GroupParentNo != groupNo)
                        {
                            groupParentList = dicList;
                            GroupParentNo = groupNo;
                        }
                        else
                        {
                            groupChildList.Add(dicList);
                        }
                    }
                }
            }
            if (GroupParentNo != 0)
            {
                groupParentList.Add("_children", groupChildList);
                list.Add(groupParentList);
            }
            return list;
        }
        #endregion
    }
}
