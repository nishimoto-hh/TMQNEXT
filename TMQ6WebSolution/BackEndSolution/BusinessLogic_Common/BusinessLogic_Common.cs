using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonSTDUtil.CommonDBManager;
using CommonWebTemplate.Models.Common;
using System.Collections;

using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using CommonSTDUtil.CommonLogger;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using CommonWebTemplate.CommonDefinitions;

namespace BusinessLogic_Common
{
    public class BusinessLogic_Common
    {

        #region private変数
        /// <summary>DB操作クラス</summary>
        protected CommonDBManager db;
        /// <summary>実行条件(Dictionary)</summary>
        private List<Dictionary<string, object>> searchConditionDictionary;
        /// <summary>実行結果(Dictionary)</summary>
        private List<Dictionary<string, object>> resultInfoDictionary;
        // 排他制御に使用するオブジェクト
        private Object lockObject = new Object();
        /// <summary>ログ出力</summary>
        private static CommonLogger logger = CommonLogger.GetInstance("logger");
        private static CommonLogger loggerLogin = CommonLogger.GetInstance("loggerLogin");
        #endregion

        #region プロパティ
        /// <summary>端末IPアドレス</summary>
        public string TerminalNo { get; set; }
        /// <summary>機能ID</summary>
        public string ConductId { get; set; }
        /// <summary>Form番号</summary>
        public byte FormNo { get; set; }
        /// <summary>登録者ID</summary>
        public string UserId { get; set; }
        /// <summary>ユーザー権限レベルID</summary>
        public int AuthorityLevelId { get; set; }
        /// <summary>ユーザー所属情報</summary>
        public BelongingInfo BelongingInfo { get; set; }
        /// <summary>本務工場ID</summary>
        public string FactoryId { get; set; }
        /// <summary>コントロールID</summary>
        public string CtrlId { get; set; }
        /// <summary>アクション区分</summary>
        public short ActionKbn { get; set; }
        /// <summary>ルート物理パス</summary>
        public string rootPath { get; set; }
        /// <summary>言語ID</summary>
        public string LanguageId { get; set; }
        /// <summary>実行結果(JSON文字列)</summary>
        public string JsonResult { get; private set; }

        /// <summary>ステータス</summary>
        private int Status { get; set; }
        /// <summary>メッセージコード</summary>
        private string MsgId { get; set; }
        /// <summary>ログ問合せ番号</summary>
        private string LogNo { get; set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_Common()
        {
        }
        #endregion

        #region 共通ロジック
        public int ExecuteBusinessLogic(CommonProcParamIn inParam, out CommonProcParamOut outParam)
        {
            this.Status = CommonProcReturn.ProcStatus.Valid;
            this.MsgId = string.Empty;
            this.LogNo = string.Empty;

            int result = 0;
            outParam = new CommonProcParamOut();
            outParam.Status = this.Status;
            outParam.MsgId = this.MsgId;
            outParam.LogNo = this.LogNo;

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

                try
                {
                    // 起動処理名によって処理を分岐する
                    switch (this.CtrlId)
                    {
                        // コンボボックス、オートコンプリートを作成
                        case "CTRLSQL":
                            result = this.CTRLSQL();
                            break;
                        // パスワード変更
                        case "ChangePassword":
                            result = this.ChangePassword();
                            break;
                        // 翻訳取得処理
                        case "GetResourcesValue":
                            result = this.GetResourcesValue();
                            break;
                        // ログイン認証処理
                        case "LoginAuthentication":
                            //result = this.LoginAuthentication(inParam.UserId);
                            result = this.LoginAuthentication(inParam.LoginId, inParam.UserId, inParam.IsNewLogin);
                            break;
                        // ボタン権限チェック処理
                        case "CheckAuthority":
                            result = this.CheckButtonAuthority();
                            break;
                        // 機能権限チェック処理
                        case "CheckConductAuthority":
                            result = this.CheckConductAuthority();
                            break;
                        // 構成リスト取得
                        case "GetStructureList":
                            result = this.GetStructureList(inParam.BelongingInfo, inParam.UserId, this.LanguageId);
                            break;
                        // ログアウト処理
                        case "Logout":
                            result = this.Logout(inParam.UserId, inParam.GUID);
                            break;
                        // ページ当たりの行数コンボ取得処理
                        case "GetComboRowsPerPage":
                            result = this.GetComboRowsPerPage(inParam.UserId);
                            break;
                        // 項目カスタマイズ情報保存処理
                        case "SaveCustomizeListInfo":
                            result = this.SaveCustomizeListInfo();
                            break;
                        // 言語コンボ取得処理
                        case "GetLanguageItemList":
                            result = this.GetLanguageItemList(inParam.UserId);
                            break;
                        // 機能ID取得処理
                        case "GetUserConductAuthorityList":
                            result = this.GetUserConductAuthorityList(inParam.UserId, inParam.AuthorityLevelId);
                            break;
                        // テンポラリフォルダパス取得処理
                        case "GetTemporaryFolderPath":
                            result = this.GetTemporaryFolderPath();
                            break;
                        // 画像表示情報取得
                        case "GetImageFileInfo":
                            result = this.GetImageFileInfo();
                            break;
                        // 復号化データ取得
                        case "GetDecryptedData":
                            result = this.GetDecryptedData();
                            break;
                        // ユーザID取得処理
                        case "GetUserIdByMailAdress":
                            result = this.GetUserIdByMailAdress();
                            break;
                        default:
                            break;
                    }

                    outParam.Status = this.Status;
                    outParam.MsgId = this.MsgId;
                    outParam.LogNo = this.LogNo;
                    outParam.ResultList = this.resultInfoDictionary;

                    return result;
                }
                catch (Exception ex)
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    this.MsgId = ex.Message;
                    this.LogNo = string.Empty;

                    return -3;
                }
            }
            catch (Exception ex)
            {
                outParam.Status = CommonProcReturn.ProcStatus.Error;
                outParam.MsgId = ex.Message;

                return -2;
            }
        }
        #endregion

        #region オートコンプリート、コンボボックス作成
        /// <summary>
        /// オートコンプリート、コンボボックス作成
        /// </summary>
        /// <param name="inParam"></param>
        /// <param name="outParam"></param>
        /// <returns></returns>
        private int CTRLSQL()
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }

                var list = new List<Dictionary<string, object>>();

                if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                {
                    if (!ComUtil.ExecKanriSql(this.db, this.searchConditionDictionary, this.rootPath, ref list, this.FactoryId, this.UserId, this.LanguageId))
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        //コンボ、オートコンプリート処理に失敗しました。
                        this.MsgId = ComUtil.GetPropertiesMessage(CommonSTDUtil.CommonResources.ID.ID941100002, this.LanguageId, null, this.db, new List<int> { Convert.ToInt32(this.FactoryId) });

                        return -1;
                    }

                    if (list != null && list.Count > 0)
                    {
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

        #region パスワード変更
        /// <summary>
        /// パスワード変更
        /// </summary>
        /// <param name="inParam"></param>
        /// <param name="outParam"></param>
        /// <returns></returns>
        private int ChangePassword()
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }

                string message = "";
                // パスワード変更処理
                int result = ComUtil.ChangePassword(this.db, this.searchConditionDictionary, ref message, this.FactoryId, this.UserId, this.LanguageId);
                if (result < 0)
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    this.MsgId = message;

                    return -1;
                }

                this.Status = CommonProcReturn.ProcStatus.Valid;
                this.MsgId = message;

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

        #region ログイン認証
        /// <summary>
        /// ログイン認証
        /// </summary>
        /// <param name="loginId">ログインID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="isNewLogin">true:新規ログイン/false:ログイン済み</param>
        /// <returns></returns>
        //private int LoginAuthentication(string userId)
        private int LoginAuthentication(string loginId, string userId, bool isNewLogin)
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, loggerLogin, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }

                // 機能IDが設定されている場合はユーザー情報の更新時
                bool isUpdateUserInfo = !string.IsNullOrEmpty(this.ConductId);

                var list = new List<Dictionary<string, object>>();
                if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                {
                    int result = ComUtil.GetLoginAuthentication(this.db, loginId, userId, this.searchConditionDictionary, ref list, isNewLogin);
                    if (result < 0)
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        //ログイン認証処理に失敗しました。
                        this.MsgId = ComUtil.GetPropertiesMessage(CommonSTDUtil.CommonResources.ID.ID941430003, this.LanguageId, null, this.db, new List<int> { Convert.ToInt32(this.FactoryId) });

                        return -1;
                    }

                    if (list != null && list.Count > 0)
                    {
                        this.resultInfoDictionary = list;

                        // ユーザID、本務工場IDを取得
                        this.UserId = list[0]["userId"].ToString();
                        var belongingInfo = list[0]["belongingInfo"] as BelongingInfo;
                        var factoryId = string.Empty;
                        if (belongingInfo != null)
                        {
                            factoryId = belongingInfo.DutyFactoryId.ToString();
                        }
                        if (!isUpdateUserInfo)
                        {
                            // ログインログ出力
                            // ※ユーザー情報更新時には出力しない
                            logger.LoginLog(factoryId, this.UserId);
                        }
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

        #region 権限チェック
        /// <summary>
        /// ボタン権限チェック
        /// </summary>
        /// <param name="inParam"></param>
        /// <param name="outParam"></param>
        /// <returns></returns>
        private int CheckButtonAuthority()
        {
            try
            {
                // DB接続
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }

                var list = new List<Dictionary<string, object>>();

                if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                {
                    int result = ComUtil.GetCheckAuthority(this.db, this.BelongingInfo, this.UserId, this.AuthorityLevelId, this.ConductId, this.searchConditionDictionary, ref list);
                    if (result < 0)
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        //権限チェック処理に失敗しました。
                        this.MsgId = ComUtil.GetPropertiesMessage(CommonSTDUtil.CommonResources.ID.ID941090003, this.LanguageId, null, this.db, new List<int> { Convert.ToInt32(this.FactoryId) });

                        return -1;
                    }

                    if (list != null && list.Count > 0)
                    {
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

        /// <summary>
        /// 機能権限チェック
        /// </summary>
        /// <returns></returns>
        private int CheckConductAuthority()
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }

                int result = ComUtil.CheckConductAuthority(this.db, this.BelongingInfo, this.UserId, this.AuthorityLevelId, this.ConductId, out bool hasAuthority);
                if (result < 0)
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    //権限チェック処理に失敗しました。
                    this.MsgId = ComUtil.GetPropertiesMessage(CommonSTDUtil.CommonResources.ID.ID941090003, this.LanguageId, null, this.db, new List<int> { Convert.ToInt32(this.FactoryId) });

                    return -1;
                }

                this.resultInfoDictionary = new List<Dictionary<string, object>>();
                this.resultInfoDictionary.Add(new Dictionary<string, object> { { "HasAuthority", hasAuthority } });
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

        #region 翻訳取得処理
        /// <summary>
        /// 翻訳取得処理
        /// </summary>
        /// <param name="inParam"></param>
        /// <param name="outParam"></param>
        /// <returns></returns>
        private int GetResourcesValue()
        {

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
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }

                var factoryIdList = new List<int>();
                if (!string.IsNullOrEmpty(this.FactoryId))
                {
                    factoryIdList.Add(Convert.ToInt32(this.FactoryId));
                }
                if (!factoryIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
                {
                    factoryIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
                }
                if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                {
                    foreach (var dic in this.searchConditionDictionary)
                    {
                        // メッセージIDを取得する
                        if (dic.ContainsKey("name"))
                        {
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
                        list = ComUtil.GetPropertiesMessages(keys, this.LanguageId, null, this.db, factoryIdList);
                    }
                    else
                    {
                        // パラメータが取得できない場合、全ての翻訳を取得
                        list = ComUtil.GetPropertiesAllMessage(this.LanguageId, this.db, factoryIdList);
                    }

                }
                else
                {
                    // パラメータが未設定の場合、全ての翻訳を取得
                    list = ComUtil.GetPropertiesAllMessage(this.LanguageId, this.db, factoryIdList);
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
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
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

        #region ログアウト処理
        /// <summary>
        /// ログアウト処理
        /// </summary>
        /// <param name="userId">登録者ID</param>
        /// <param name="guid">GUID</param>
        /// <returns></returns>
        private int Logout(string userId, string guid)
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }

                // 一時テーブルデータの削除
                ComUtil.DeleteTmpTableData(this.db, userId, -1, guid);

                // セッション管理テーブルのログアウト日時の更新
                ComUtil.UpdateLogoutDate(this.db, userId, guid, DateTime.Now);

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

        #region 1ページ当たりの行数コンボ取得処理
        /// <summary>
        /// 1ページ当たりの行数コンボ取得処理
        /// </summary>
        /// <param name="userId">ログインユーザID</param>
        /// <returns></returns>
        private int GetComboRowsPerPage(string userId)
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }
                // 処理呼び出し
                var result = ComUtil.GetComboRowsPerPage(userId, this.LanguageId, this.db);

                List<Dictionary<string, object>> resource = new();
                Dictionary<string, object> dicList = new();
                dicList.Add("ComboRows", result);
                resource.Add(dicList);

                this.resultInfoDictionary = resource;
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
        #region 項目カスタマイズ情報保存処理
        /// <summary>
        /// 項目カスタマイズ情報保存処理
        /// </summary>
        /// <returns></returns>
        private int SaveCustomizeListInfo()
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }
                // 処理呼び出し
                if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                {
                    var result = ComUtil.SaveCustomizeListInfo(this.db, this.UserId, this.searchConditionDictionary[0]);
                    if (!result)
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        // 項目カスタマイズ情報の登録に失敗しました。
                        this.MsgId = ComUtil.GetPropertiesMessage(CommonSTDUtil.CommonResources.ID.ID941100003, this.LanguageId, null, this.db, new List<int> { Convert.ToInt32(this.FactoryId) });
                        writeErrorLog(this.MsgId);
                        return -1;
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

        #region 言語取得処理
        /// <summary>
        /// 言語コンボ取得処理
        /// </summary>
        /// <param name="userId">ログインユーザID</param>
        /// <returns></returns>
        private int GetLanguageItemList(string userId)
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }
                // 処理呼び出し
                var result = ComUtil.GetLanguageItemList(userId, this.LanguageId, this.db);

                List<Dictionary<string, object>> resource = new();
                Dictionary<string, object> dicList = new();
                dicList.Add("LanguageItemList", result);
                resource.Add(dicList);

                this.resultInfoDictionary = resource;
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

        #region 機能IDリスト取得処理
        /// <summary>
        /// 機能IDリスト取得処理
        /// </summary>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="authorityLevelId">ユーザー権限レベルID</param>
        /// <returns></returns>
        private int GetUserConductAuthorityList(string userId, int authorityLevelId)
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }
                // 処理呼び出し
                var result = ComUtil.GetUserConductAuthorityList(userId, authorityLevelId, this.LanguageId, this.db);

                List<Dictionary<string, object>> resource = new();
                Dictionary<string, object> dicList = new();
                dicList.Add("ConductIdList", result);
                resource.Add(dicList);

                this.resultInfoDictionary = resource;
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

        #region テンポラリフォルダパス取得処理
        /// <summary>
        /// テンポラリフォルダパス取得処理
        /// </summary>
        /// <returns></returns>
        private int GetTemporaryFolderPath()
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }
                // 処理呼び出し
                var result = ComUtil.GetTemporaryFolderPath(this.db);

                List<Dictionary<string, object>> resource = new();
                Dictionary<string, object> dicList = new();
                dicList.Add("TemporaryFolderPath", result);
                resource.Add(dicList);

                this.resultInfoDictionary = resource;
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

        #region privateメソッド
        /// <summary>
        /// パラメータチェック
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //private bool CheckParameters(dynamic param)
        private bool CheckParameters(CommonProcParamIn param)
        {
            // 設定値チェック
            // ユーザID
            this.UserId = param.UserId;
            // ユーザー権限レベルID
            this.AuthorityLevelId = param.AuthorityLevelId;
            // ユーザー所属情報
            this.BelongingInfo = param.BelongingInfo;
            // 本務工場ID
            this.FactoryId = param.FactoryId;

            // 起動処理名（コントロールID）
            //if (!IsPropertyExists(param, "CtrlId") || string.IsNullOrEmpty(param.CtrlId))
            if (string.IsNullOrEmpty(param.CtrlId))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                this.MsgId = "<パラメータに起動処理名が設定されていません。>";
                this.LogNo = string.Empty;
                return false;
            }

            // ルート物理パス
            //if (!IsPropertyExists(param, "RootPath") || string.IsNullOrEmpty(param.RootPath))
            if (string.IsNullOrEmpty(param.RootPath))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                this.MsgId = "<パラメータにルートパスが設定されていません。>";
                this.LogNo = string.Empty;
                return false;
            }

            // メンバ変数へパラメータを設定
            this.rootPath = param.RootPath; // 物理ルートパス
            this.CtrlId = param.CtrlId; // 起動処理名
            this.ConductId = param.ConductId; // 機能ID

            // 言語ID
            //if (!IsPropertyExists(param, "LanguageId") || string.IsNullOrEmpty(param.LanguageId))
            if (string.IsNullOrEmpty(param.LanguageId))
            {
                // 未設定の場合は日本語を設定
                this.LanguageId = "ja";
            }
            else
            {
                this.LanguageId = param.LanguageId;
            }

            return true;
        }

        /// <summary>
        /// プロパティチェック
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsPropertyExists(dynamic obj, string name)
        {
            if (obj is ExpandoObject)
                return ((IDictionary<string, object>)obj).ContainsKey(name);

            return obj.GetType().GetProperty(name) != null;
        }

        /// <summary>
        /// 検索条件を再設定
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private dynamic setCondition(Dictionary<string, object> condition)
        {
            dynamic searchCondition = new ExpandoObject();
            if (condition != null)
            {
                foreach (var item in condition)
                {
                    ((IDictionary<string, object>)searchCondition).Add(item.Key, item.Value);
                }
            }
            return searchCondition;
        }

        /// <summary>
        /// JSON文字列へのシリアライズ
        /// </summary>
        /// <param name="list"></param>
        /// <returns>JSON文字列</returns>
        private string SerializeToJson(List<Dictionary<string, object>> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    var jsonOptions = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    };
                    return JsonSerializer.Serialize(list, jsonOptions);
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

        private string[] ToStringArray(object[] array, bool includeNulls = false, string nullValue = "")
        {
            IEnumerable<object> enumerable = array;
            if (!includeNulls)
            {
                enumerable = enumerable.Where(e => e != null);
            }
            return enumerable.Select(e => (e ?? nullValue).ToString()).ToArray();
        }
        #endregion

        #region ログ出力
        /// <summary>
        /// エラーログ出力
        /// </summary>
        /// <param name="msg">エラーメッセージ</param>
        /// <param name="ex">例外オブジェクト(省略可)</param>
        protected void writeErrorLog(string msg, Exception ex = null)
        {
            logger.ErrorLog(this.FactoryId, this.UserId, msg, ex);
        }

        /// <summary>
        /// DB接続エラーメッセージ設定
        /// </summary>
        protected void setDBConnectionErrorLogAndMessage()
        {
            // 「DB接続に失敗しました。」
            logger.ErrorLog(this.FactoryId, this.UserId, ComUtil.GetPropertiesMessage(CommonSTDUtil.CommonResources.ID.ID941190004, this.LanguageId));
            // エラーが発生しました。システム管理者に問い合わせてください。
            this.MsgId = ComUtil.GetPropertiesMessage(CommonSTDUtil.CommonResources.ID.ID941040004, this.LanguageId);

        }
        #endregion

        #region 画像表示情報取得
        /// <summary>
        /// 画像表示情報取得処理
        /// </summary>
        /// <returns>正常終了：0、異常終了：-1</returns>
        private int GetImageFileInfo()
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }
                // 処理呼び出し
                if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                {
                    // 画像ファイル情報を取得
                    var result = ComUtil.GetImageFileInfo(this.searchConditionDictionary[0], this.db, out string filePath);
                    if (!result)
                    {
                        // 取得不能の場合、終了
                        return -1;
                    }
                    // 取得した画像ファイルパスを返す
                    List<Dictionary<string, object>> resource = new();
                    Dictionary<string, object> dicList = new();
                    dicList.Add("ImageFilePath", filePath);
                    resource.Add(dicList);
                    this.resultInfoDictionary = resource;
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

        #region 復号化データ取得
        /// <summary>
        /// 復号化データ処理
        /// </summary>
        /// <returns>正常終了：0、異常終了：-1</returns>
        private int GetDecryptedData()
        {
            try
            {
                // 処理呼び出し
                if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                {
                    // 復号化データを取得
                    var decryptedData = ComUtil.GetDecryptedData(this.searchConditionDictionary[0]);
                    if (string.IsNullOrEmpty(decryptedData))
                    {
                        // 変換不能の場合、終了
                        return -1;
                    }
                    // 復号化データを返す
                    List<Dictionary<string, object>> resultList = new();
                    Dictionary<string, object> dicList = new();
                    dicList.Add("DecryptedData", decryptedData);
                    resultList.Add(dicList);
                    this.resultInfoDictionary = resultList;
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

        #region ユーザID取得
        /// <summary>
        /// メールアドレスからユーザID処理
        /// </summary>
        /// <returns>正常終了：0、異常終了：-1</returns>
        private int GetUserIdByMailAdress()
        {
            try
            {
                // DB接続
                this.db = new CommonDBManager(this.rootPath, logger, this.FactoryId, this.UserId);
                if (!this.db.Connect())
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // DB接続エラーログ＆メッセージ設定
                    setDBConnectionErrorLogAndMessage();
                    return -1;
                }
                if (this.searchConditionDictionary != null && this.searchConditionDictionary.Count > 0)
                {
                    // 処理呼び出し
                    var result = ComUtil.GetUserInfoByMailAdress(this.searchConditionDictionary[0], this.db);

                    List<Dictionary<string, object>> resultList = new();
                    Dictionary<string, object> dicList = new();
                    dicList.Add("UserId", result.UserId);
                    resultList.Add(dicList);

                    this.resultInfoDictionary = resultList;
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
            #endregion
        }
    }
}
