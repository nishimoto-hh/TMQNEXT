using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonSTDUtil.CommonLogger;
using System.Globalization;

using APComUtil = APCommonUtil.APCommonUtil.APCommonUtil;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComID = CommonAPUtil.APCommonUtil.APResources.ID;
using ComPack = CommonAPUtil.APStoredPrcUtil.PakCommon;
using ComST = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Constants = APConstants.APConstants;
using Dao = CommonAPUtil.APStoredPrcUtil.ProFixedDataClass;
using fncUtil = CommonAPUtil.APStoredFncUtil.APStoredFncUtil;
using InvConst = CommonAPUtil.APInventoryUtil.InventoryConst;
using InvDao = CommonAPUtil.APInventoryUtil.InventoryDataClass;
using InvManufactureOrder = CommonAPUtil.APInventoryUtil.ManufactureOrderInventory;
using InvPurchaseRequest = CommonAPUtil.APInventoryUtil.PurchaseRequestInventory;
using PakDao = CommonAPUtil.APStoredPrcUtil.PakCommonDataClass;
using PlanPackage = CommonAPUtil.APStoredPrcUtil.ProPlanPackage;
using PrcUtil = CommonAPUtil.APStoredPrcUtil.APStoredPrcUtil;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>
    /// 確定処理
    /// </summary>
    public class ProFixed
    {
        #region private変数
        /// <summary>製造指図ヘッダテーブルのシーケンスの管理テーブルのキー値</summary>
        private const string SeqDirectionNo = "DIRECTION_NO";

        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>ログメッセージ</summary>
        private static string logMessage = "";
        /// <summary>製造オーダ番号または購入依頼番号</summary>
        //protected static string wkodrNo = "";個別に設定するよう修正
        /// <summary>納期</summary>
        //protected static DateTime? wkPDATE = DateTime.MinValue;//個別に設定するよう修正
        #endregion

        #region コンストラクタ
        /// <summary>
        /// インスタンス
        /// </summary>
        public ProFixed()
        {
            // コンストラクタは不要
            //ログ出力用インスタンス生成
            //logger = CommonLogger.GetInstance("test");
        }
        #endregion

        #region 定数
        /// <summary>
        /// 確定処理定数定義クラス
        /// </summary>
        private static class MyConst
        {
            /// <summary>発生元区分 1:計画</summary>
            public const int OriginDivisionPlan = 1;
            /// <summary>モジュールコード</summary>
            //public const string ModuleCd = "0508";
            /// <summary>クライアント</summary>
            //public const string Client = "確定処理";
            /// <summary>確定条件</summary>
            public const string NamesFXCD = "FXCD";
            /// <summary>購入依頼件名</summary>
            public const string NamesPSBJ = "PSBJ";
            /// <summary>"1"</summary>
            public const string One = "1";
            /// <summary>製造オーダー番号</summary>
            //public const string NOKEY_MASASI = "NOKEY_MASASI";
            /// <summary>発注オーダー番号</summary>
            //public const string NOKEY_HACHU = "NOKEY_HACHU";
            /// <summary>消費税区分マスタ：用途</summary>
            public const string STOCKING = "STOCKING";
            /// <summary>品目タイプ：2:B2100:仕掛品 x半製品</summary>
            public const int ItemType_2_B2100 = 2;
        }
        #endregion

        #region 戻り値クラス
        /// <summary>
        /// 戻り値クラス
        /// </summary>
        public class ResultInfo
        {
            /// <summary>Gets or sets a value indicating whether gets or sets エラー有無</summary>
            /// <value>エラー有無</value>
            public bool IsError { get; set; }
            /// <summary>Gets or sets メッセージ</summary>
            /// <value>メッセージ</value>
            public string Message { get; set; }
            /// <summary>
            /// コンストラクタ(エラーなし)
            /// </summary>
            public ResultInfo()
            {
                this.IsError = false;
                this.Message = string.Empty;
            }
            /// <summary>
            /// コンストラクタ(値指定)
            /// </summary>
            /// <param name="isError">エラーフラグ</param>
            /// <param name="message">メッセージ</param>
            public ResultInfo(bool isError, string message)
            {
                this.IsError = isError;
                this.Message = message;
            }
        }
        #endregion

        #region レベル１
        #region ExecProFixedは不要 2021.09.01
        /// <summary>
        /// 確定処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="transaction">トランザクション</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool ExecProFixed(ComDB db, System.Data.IDbTransaction transaction)
        {
            string sql;         // 動的SQL
            int cnt = 0;        // 件数
            int regFlg = 0;     // 更新フラグ
            //int ret = 0;        // 戻り値
            int execStart = 0;  // 実行開始フラグ

            try
            {
                // プロシージャ待ち件数を取得
                sql = "";
                sql = sql + "select ";
                sql = sql + "	* ";
                sql = sql + "from ";
                sql = sql + "	proc_param ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and process_cd = 'PRO_FIXED' ";
                sql = sql + "and check_flg = '1' "; // 1:登録中
                cnt = db.GetCount(sql);

                if (cnt > 0)
                {
                    // 実行パラメータ情報取得
                    sql = "";
                    sql = sql + "select ";
                    sql = sql + "	* ";
                    sql = sql + "from ";
                    sql = sql + "	proc_param ";
                    sql = sql + "where ";
                    sql = sql + "    1 = 1 ";
                    sql = sql + "and process_cd = 'PRO_FIXED' ";
                    sql = sql + "and check_flg = '1' "; // 1:登録中

                    ComDao.ProcParamEntity procParamEntity = new ComDao.ProcParamEntity();
                    procParamEntity = db.GetEntityByDataClass<ComDao.ProcParamEntity>(sql);

                    // プロシージャ実行中に変更
                    sql = "";
                    sql = sql + "update proc_param ";
                    sql = sql + "set ";
                    sql = sql + "    check_flg = '2' "; // 2:実行中
                    sql = sql + "    ,update_date = Now() ";
                    sql = sql + "    ,update_user_id = @InputUserId ";
                    sql = sql + "where ";
                    sql = sql + "    1 = 1 ";
                    sql = sql + "and process_cd = 'PRO_FIXED' ";
                    // 更新処理実行
                    regFlg = db.Regist(
                        sql,
                        new
                        {
                            InputUserId = procParamEntity.InputUserId
                        });
                    if (regFlg < 0)
                    {
                        // 異常終了
                        return false;
                    }
                    // コミット
                    transaction.Commit();

                    // テーブルロック情報判定：在庫更新、バッチ処理でのロック情報判定
                    ProTableLockInfo(db, procParamEntity.InputUserId);

                    // パラメータ初期化
                    execStart = 1;

                    // 確定処理実行
                    //ProFixedExecute(db,);

                    // バッチテーブル削除
                    sql = "";
                    sql = sql + "delete from proc_param ";
                    sql = sql + "where ";
                    sql = sql + "    1 = 1 ";
                    sql = sql + "and process_cd = 'PRO_FIXED' ";
                    // 更新処理実行
                    regFlg = db.Regist(sql);
                    if (regFlg < 0)
                    {
                        // 異常終了
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.ToString());
                //throw ex;
                if (execStart == 1)
                {
                    // バッチテーブル削除
                    sql = "";
                    sql = sql + "delete from proc_param ";
                    sql = sql + "where ";
                    sql = sql + "    1 = 1 ";
                    sql = sql + "and process_cd = 'PRO_FIXED' ";
                    // 更新処理実行
                    regFlg = db.Regist(sql);
                    // コミット
                    transaction.Commit();
                }
                // 異常終了
                return false;
            }
        }
        #endregion
        #endregion

        #region レベル２
        #region ProTableLockInfoは廃止 2021.09.01
        /// <summary>
        /// テーブルロック情報判定：在庫更新、バッチ処理でのロック情報判定
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inputUserId">ユーザID</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool ProTableLockInfo(ComDB db, string inputUserId)
        {
            try
            {
                string sql;         // 動的SQL
                int regFlg = 0;     // 更新フラグ

                // 実行パラメータ情報取得
                sql = "";
                sql = sql + "select ";
                sql = sql + "	* ";
                sql = sql + "from ";
                sql = sql + "	proc_param ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and process_cd = 'PROC_EXCLUSIVE_01' ";
                sql = sql + "for update "; // FOR UPDATE句によりロック

                ComDao.ProcParamEntity procParamEntity = new ComDao.ProcParamEntity();
                procParamEntity = db.GetEntityByDataClass<ComDao.ProcParamEntity>(sql);

                sql = "";
                sql = sql + "update proc_param ";
                sql = sql + "set ";
                sql = sql + "    ,update_date = Now() ";
                sql = sql + "    ,update_user_id = @InputUserId ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and process_cd = 'PROC_EXCLUSIVE_01' ";

                // 更新処理実行
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        InputUserId = inputUserId
                    });
                if (regFlg < 0)
                {
                    // 異常終了
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.ToString());
                //throw ex;
                // 異常終了
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 確定処理実行
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="transaction">トランザクション</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <param name="batUserId">バッチ実行ユーザーID</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        public static ResultInfo ProFixedExecute(
            ComDB db, System.Data.IDbTransaction transaction, PakDao.ComParamInfo paramInfo, ref List<string> errMessage, string batUserId)
        {
            //ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(paramInfo.PgmId);
            string sql;         // 動的SQL
            int cnt = 0;        // 件数
            //int regFlg = 0;     // 更新フラグ
            bool ret = true;
            bool mstErrorFlg = false; // マスタチェックエラーフラグ

            // 実行条件
            // 製造
            string productProductDivision = "";
            string productProductDtFrom = "";
            string productProductDtTo = "";
            //string productFetchDivision = "";
            //string productFetchDtFrom = "";
            //string productFetchDtTo = "";
            string productFixCondition = "";
            string productDeliveryLimit = "";
            // 購買
            string purchaseProductDivision = "";
            string purchaseProductDtFrom = "";
            string purchaseProductDtTo = "";
            //string purchaseFetchDivision = "";
            //string purchaseFetchDtFrom = "";
            //string purchaseFetchDtTo = "";
            string purchaseFixCondition = "";
            string purchaseDeliveryLimit = "";
            // 外注
            string orderProductDivision = "";
            string orderProductDtFrom = "";
            string orderProductDtTo = "";
            //string orderFetchDivision = "";
            //string orderFetchDtFrom = "";
            //string orderFetchDtTo = "";
            string orderFixCondition = "";
            string orderDeliveryLimit = "";

            try
            {
                try
                {
                    // 実行条件を取得
                    sql = "";
                    sql = sql + "select ";
                    sql = sql + "    condition ";
                    sql = sql + "from ";
                    sql = sql + "   com_bat_sch ";
                    sql = sql + "where ";
                    sql = sql + "    ip_address = @TerminalNo ";
                    sql = sql + "and conductid = @ConductId ";
                    sql = sql + "and pgmid = @ConductId ";
                    sql = sql + "and formno = @FormNo ";
                    sql = sql + "and exec_date = @ExecDate ";
                    sql = sql + "and exec_time = @ExecTime ";
                    sql = sql + "and status = " + "2" + " "; // ステータス 2:起動
                    sql = sql + "and delflg = " + "0" + " ";
                    string condition = db.GetEntity<string>(
                        sql,
                        new
                        {
                            TerminalNo = paramInfo.TerminalNo,
                            ConductId = paramInfo.ConductId,
                            FormNo = int.Parse(paramInfo.FormNo),
                            ExecDate = paramInfo.ExecDate,
                            ExecTime = paramInfo.ExecTime
                        });

                    // 実行条件を分割
                    var cond = condition.Split('|');
                    // 実行条件を設定
                    // 製造
                    productProductDivision = cond[0];
                    productProductDtFrom = cond[1];
                    productProductDtTo = cond[2];
                    //productFetchDivision = cond[3];
                    //productFetchDtFrom = cond[4];
                    //productFetchDtTo = cond[5];
                    productFixCondition = cond[3];
                    productDeliveryLimit = cond[4];
                    // 購買
                    purchaseProductDivision = cond[5];
                    purchaseProductDtFrom = cond[6];
                    purchaseProductDtTo = cond[7];
                    //purchaseFetchDivision = cond[11];
                    //purchaseFetchDtFrom = cond[12];
                    //purchaseFetchDtTo = cond[13];
                    purchaseFixCondition = cond[8];
                    purchaseDeliveryLimit = cond[9];
                    // 外注
                    orderProductDivision = cond[10];
                    orderProductDtFrom = cond[11];
                    orderProductDtTo = cond[12];
                    //orderFetchDivision = cond[19];
                    //orderFetchDtFrom = cond[20];
                    //orderFetchDtTo = cond[21];
                    orderFixCondition = cond[13];
                    orderDeliveryLimit = cond[14];
                }
                catch (Exception ex)
                {
                    // ログ出力：画面条件取得に失敗しました。
                    logMessage = ComST.GetPropertiesMessage(key: ComID.MB00018, languageId: paramInfo.LanguageId);
                    logger.Error(logMessage);
                    logger.Error(ex.Message);
                    logger.Error(ex.ToString());
                    errMessage.Add(logMessage);
                    errMessage.Add(ex.Message);
                    errMessage.Add(ex.ToString());
                    return new ResultInfo(true, logMessage);
                }
                try
                {
                    // カレンダーマスタが十分で無いとき、メッセージを出す
                    // システム日付の１か月前
                    sql = "";
                    sql = sql + "select ";
                    sql = sql + "    count(*) as cnt ";
                    sql = sql + "from ";
                    sql = sql + "    cal ";
                    sql = sql + "where ";
                    sql = sql + "    1 = 1 ";
                    //sql = sql + "and cal_cd = (select name01 from names where name_division = 'PCCD' and name_cd = '1') ";
                    sql = sql + "and cal_cd = (select cal_cd from company where company_cd = (select utility_no from utility where utility_division = 'CMPN' and utility_cd = '1')) ";
                    sql = sql + "and to_char(cal_date, 'YYYY/MM/DD') = to_char(Now() + cast('-1 months' as INTERVAL), 'YYYY/MM/DD')  ";
                    cnt = db.GetCount(sql);
                    if (cnt <= 0)
                    {
                        // ログ出力：カレンダーマスタの作成量が十分ではありません。
                        logMessage = ComST.GetPropertiesMessage(key: ComID.MB10037, languageId: paramInfo.LanguageId);
                        logger.Error(logMessage);
                        errMessage.Add(logMessage);
                        return new ResultInfo(true, logMessage);
                    }
                    // システム日付の１か月後
                    sql = "";
                    sql = sql + "with max_cal as ( ";
                    sql = sql + "select max(delivery_date) as max_date ";
                    sql = sql + "from mrp_result) ";
                    sql = sql + "select ";
                    sql = sql + "    count(*) as cnt ";
                    sql = sql + "from ";
                    sql = sql + "    cal ";
                    sql = sql + "left join max_cal ";
                    sql = sql + "on 1 = 1 ";
                    sql = sql + "where ";
                    sql = sql + "    1 = 1 ";
                    //sql = sql + "and cal_cd = (select name01 from names where name_division = 'PCCD' and name_cd = '1') ";
                    sql = sql + "and cal_cd = (select cal_cd from company where company_cd = (select utility_no from utility where utility_division = 'CMPN' and utility_cd = '1')) ";
                    sql = sql + "and cal_date = to_date(to_char(max_cal.max_date, 'yyyy/MM/dd'), 'YYYY/MM/DD') + cast('1 months' as INTERVAL)  ";
                    cnt = db.GetCount(sql);
                    if (cnt <= 0)
                    {
                        // ログ出力：カレンダーマスタの作成量が十分ではありません。
                        logMessage = ComST.GetPropertiesMessage(key: ComID.MB10037, languageId: paramInfo.LanguageId);
                        logger.Error(logMessage);
                        errMessage.Add(logMessage);
                        return new ResultInfo(true, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    // ログ出力：カレンダーマスタの作成量が十分ではありません。
                    logMessage = ComST.GetPropertiesMessage(key: ComID.MB10037, languageId: paramInfo.LanguageId);
                    logger.Error(logMessage);
                    logger.Error(ex.Message);
                    logger.Error(ex.ToString());
                    errMessage.Add(logMessage);
                    errMessage.Add(ex.Message);
                    errMessage.Add(ex.ToString());
                    return new ResultInfo(true, logMessage);
                }
                /*
                // 確定ファイルの削除
                try
                {
                    // 画面.内示作成＝するの時、内示オーダーをすべて削除
                    // → 内示作成は不要

                    // テーブルロック処理
                    ret = PakFixedLockTable(
                        db
                    );
                    if (ret == false)
                    {
                        // エラー処理
                        numRet = -1;
                        // ログ出力：テーブルロック処理 異常終了
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10053 }, languageId: pInfo.LanguageId);
                        logger.Error(logMessage);
                        return false;
                    }
                    // 確定ファイルの削除
                    //try
                    //{
                    //    // バッチテーブル削除
                    //    sql = "";
                    //    sql = sql + "delete from fixed_plan ";
                    //    // 更新処理実行
                    //    regFlg = db.Regist(sql);

                    //}
                    //catch (Exception ex)
                    //{
                    //    // エラー処理
                    //    numRet = -1;
                    //    // TODO:ログ出力
                    //    return false;
                    //}
                }
                catch (Exception ex)
                {
                    // エラー処理
                    numRet = -1;
                    // TODO:ログ出力:確定ファイルの削除エラー
                    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10003 }, languageId: pInfo.LanguageId);
                    logger.Error(logMessage);
                    logger.Error(ex.Message);
                    // ロールバック
                    transaction.Rollback();
                    return false;
                }
                */
                // 確定処理
                try
                {
                    // ログ出力:確定処理 開始
                    //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10004, ComID.MB10059, "", "", "" }, languageId: pInfo.LanguageId);
                    //logger.Info(logMessage);
                    /*
                    // テーブルロック処理
                    ret = PakFixedLockTable(
                        db
                    );
                    if (ret == false)
                    {
                        // エラー処理
                        //numRet = -1;
                        // ログ出力：テーブルロック処理 異常終了
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10053 }, languageId: pInfo.LanguageId);
                        logger.Error(logMessage);
                        return false;
                    }
                    */
                    // 製造品目：確定処理
                    if (productProductDivision == "1")
                    {
                        //// ログ出力:製造品目 製造オーダー 確定処理 開始
                        //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10048, ComID.MB10051, ComID.MB10004, ComID.MB10059, "" }, languageId: pInfo.LanguageId);
                        //logger.Info(logMessage);
                        // ログ出力:製造品目 製造オーダー 確定処理 製造オーダー日（From）
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10048, ComID.MB10051, ComID.MB10004, ComID.MB10054, productProductDtFrom ?? "" }, languageId: paramInfo.LanguageId);
                        logger.Info(logMessage);
                        errMessage.Add(logMessage);
                        // ログ出力:製造品目 製造オーダー 確定処理 製造オーダー日（To）
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10048, ComID.MB10051, ComID.MB10004, ComID.MB10055, productProductDtTo ?? "" }, languageId: paramInfo.LanguageId);
                        logger.Info(logMessage);
                        errMessage.Add(logMessage);
                        // 製造品目：確定条件
                        string releaseCondition = PakCommonGetNames(db, MyConst.NamesFXCD, productFixCondition, paramInfo, ref errMessage);

                        // ログ出力:製造品目 製造オーダー 確定処理 確定条件
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10048, ComID.MB10051, ComID.MB10004, ComID.MB10057, releaseCondition ?? "" }, languageId: paramInfo.LanguageId);
                        logger.Info(logMessage);
                        errMessage.Add(logMessage);

                        DateTime dummyDate = DateTime.MinValue;
                        DateTime? tmp_date1 = null;
                        DateTime? tmp_date2 = null;
                        DateTime? tmp_date3 = null;
                        if (DateTime.TryParse(productProductDtFrom, out dummyDate))
                        {
                            tmp_date1 = dummyDate;
                        }
                        if (DateTime.TryParse(productProductDtTo, out dummyDate))
                        {
                            tmp_date2 = dummyDate;
                        }
                        if (DateTime.TryParse(productDeliveryLimit, out dummyDate))
                        {
                            tmp_date3 = dummyDate;
                        }

                        // ログ出力:製造品目 製造オーダー 確定処理 実行
                        //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10048, ComID.MB10051, ComID.MB10004, ComID.MB10061, "" }, languageId: pInfo.LanguageId);
                        //logger.Info(logMessage);

                        // 製造オーダー確定処理
                        ret = PakFixedFixProductionOrder(
                            db,
                            0,
                            productFixCondition,
                            tmp_date1,
                            tmp_date2,
                            tmp_date3,
                            paramInfo,
                            ref errMessage,
                            ref mstErrorFlg,
                            batUserId);
                        if (ret == false)
                        {
                            // ログ出力:製造品目 製造オーダー 確定処理 エラー
                            logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10048, ComID.MB10051, ComID.MB10004, ComID.MB00022, logMessage }, languageId: paramInfo.LanguageId);
                            logger.Error(logMessage);
                            errMessage.Add(logMessage);
                            return new ResultInfo(true, logMessage);
                        }
                        // ログ出力:製造品目 製造オーダー 確定処理 終了
                        //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10048, ComID.MB10051, ComID.MB10004, ComID.MB10060, "" }, languageId: pInfo.LanguageId);
                        //logger.Info(logMessage);

                    } // 製造品目：確定処理 productProductDivision == "1"

                    // 購買品目：確定処理
                    if (purchaseProductDivision == "1")
                    {
                        //// ログ出力:購買品目 購買オーダー 確定処理 開始
                        //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10049, ComID.MB10192, ComID.MB10004, ComID.MB10059, "" }, languageId: pInfo.LanguageId);
                        //logger.Info(logMessage);
                        // ログ出力:購買品目 購買オーダー 確定処理 製造オーダー日（From）
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10049, ComID.MB10192, ComID.MB10004, ComID.MB10054, purchaseProductDtFrom ?? "" }, languageId: paramInfo.LanguageId);
                        logger.Info(logMessage);
                        errMessage.Add(logMessage);
                        // ログ出力:購買品目 購買オーダー 確定処理 製造オーダー日（To）
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10049, ComID.MB10192, ComID.MB10004, ComID.MB10055, purchaseProductDtTo ?? "" }, languageId: paramInfo.LanguageId);
                        logger.Info(logMessage);
                        errMessage.Add(logMessage);

                        // 購買品目：確定条件
                        string releaseCondition = PakCommonGetNames(db, MyConst.NamesFXCD, purchaseFixCondition, paramInfo, ref errMessage);

                        // ログ出力:購買品目 購買オーダー 確定処理 確定条件
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10049, ComID.MB10192, ComID.MB10004, ComID.MB10057, releaseCondition ?? "" }, languageId: paramInfo.LanguageId);
                        logger.Info(logMessage);
                        errMessage.Add(logMessage);

                        DateTime dummyDate = DateTime.MinValue;
                        DateTime? tmp_date1 = null;
                        DateTime? tmp_date2 = null;
                        DateTime? tmp_date3 = null;
                        if (DateTime.TryParse(purchaseProductDtFrom, out dummyDate))
                        {
                            tmp_date1 = dummyDate;
                        }
                        if (DateTime.TryParse(purchaseProductDtTo, out dummyDate))
                        {
                            tmp_date2 = dummyDate;
                        }
                        if (DateTime.TryParse(purchaseDeliveryLimit, out dummyDate))
                        {
                            tmp_date3 = dummyDate;
                        }

                        // 購買オーダー確定処理
                        ret = PakFixedFixProductionOrder(
                            db,
                            1,
                            purchaseFixCondition,
                            tmp_date1,
                            tmp_date2,
                            tmp_date3,
                            paramInfo,
                            ref errMessage,
                            ref mstErrorFlg,
                            batUserId);
                        if (ret == false)
                        {
                            // ログ出力:購買品目 購買オーダー 確定処理 エラー
                            logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10049, ComID.MB10192, ComID.MB10004, ComID.MB00022, logMessage }, languageId: paramInfo.LanguageId);
                            logger.Error(logMessage);
                            errMessage.Add(logMessage);
                            return new ResultInfo(true, logMessage);
                        }
                        //// ログ出力:購買品目 購買オーダー 確定処理 正常終了
                        //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10049, ComID.MB10192, ComID.MB10004, ComID.MB10060, "" }, languageId: pInfo.LanguageId);
                        //logger.Info(logMessage);

                    } // 購買品目：確定処理 purchaseProductDivision == "1"

                    // 外注品目：確定処理
                    if (orderProductDivision == "1")
                    {
                        //// ログ出力:外注品目 購買オーダー 確定処理 開始
                        //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10050, ComID.MB10192, ComID.MB10004, ComID.MB10059, "" }, languageId: pInfo.LanguageId);
                        //logger.Info(logMessage);
                        // ログ出力:外注品目 購買オーダー 確定処理 製造オーダー日（From）
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10050, ComID.MB10192, ComID.MB10004, ComID.MB10054, orderProductDtFrom ?? "" }, languageId: paramInfo.LanguageId);
                        logger.Info(logMessage);
                        errMessage.Add(logMessage);
                        // ログ出力:外注品目 購買オーダー 確定処理 製造オーダー日（To）
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10050, ComID.MB10192, ComID.MB10004, ComID.MB10055, orderProductDtTo ?? "" }, languageId: paramInfo.LanguageId);
                        logger.Info(logMessage);
                        errMessage.Add(logMessage);
                        // 外注品目：確定条件
                        string releaseCondition = PakCommonGetNames(db, MyConst.NamesFXCD, orderFixCondition, paramInfo, ref errMessage);
                        // ログ出力:外注品目 購買オーダー 確定処理 確定条件
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10050, ComID.MB10192, ComID.MB10004, ComID.MB10057, releaseCondition ?? "" }, languageId: paramInfo.LanguageId);
                        logger.Info(logMessage);
                        errMessage.Add(logMessage);

                        DateTime dummyDate = DateTime.MinValue;
                        DateTime? tmp_date1 = null;
                        DateTime? tmp_date2 = null;
                        DateTime? tmp_date3 = null;
                        if (DateTime.TryParse(orderProductDtFrom, out dummyDate))
                        {
                            tmp_date1 = dummyDate;
                        }
                        if (DateTime.TryParse(orderProductDtTo, out dummyDate))
                        {
                            tmp_date2 = dummyDate;
                        }
                        if (DateTime.TryParse(orderDeliveryLimit, out dummyDate))
                        {
                            tmp_date3 = dummyDate;
                        }

                        // 外注オーダー確定処理
                        ret = PakFixedFixProductionOrder(
                            db,
                            2,
                            orderFixCondition,
                            tmp_date1,
                            tmp_date2,
                            tmp_date3,
                            paramInfo,
                            ref errMessage,
                            ref mstErrorFlg,
                            batUserId);
                        if (ret == false)
                        {
                            // ログ出力:外注品目 購買オーダー 確定処理 エラー
                            logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10050, ComID.MB10192, ComID.MB10004, ComID.MB00022, logMessage }, languageId: paramInfo.LanguageId);
                            logger.Error(logMessage);
                            errMessage.Add(logMessage);
                            return new ResultInfo(true, logMessage);
                        }
                    } // 外注品目：確定処理 orderProductDivision == "1"

                    // ログ出力:外注品目 購買オーダー 確定処理 正常終了
                    //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10050, ComID.MB10192, ComID.MB10004, ComID.MB10060, "" }, languageId: pInfo.LanguageId);
                    //logger.Info(logMessage);
                }
                catch (Exception ex)
                {
                    // ログ出力:確定処理 異常終了
                    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10004, ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                    logger.Info(logMessage);
                    errMessage.Add(logMessage);
                    logMessage = "";
                    logger.Error(ex.Message);
                    logger.Error(ex.ToString());
                    errMessage.Add(ex.Message);
                    errMessage.Add(ex.ToString());
                    return new ResultInfo(true, logMessage);
                } // 製造オーダー作成処理(確定処理)

                // 取込オーダー作成処理
                //try
                //{
                //// ログ出力:取込オーダー 確定処理 開始
                //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10052, ComID.MB10004, ComID.MB10059, "", "" }, languageId: pInfo.LanguageId);
                //logger.Info(logMessage);

                //// テーブルロック処理
                //ret = PakFixedLockTable(db, pInfo);
                //if (ret == false)
                //{
                //    // ログ出力：テーブルロック処理 異常終了
                //    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10053 }, languageId: pInfo.LanguageId);
                //    logger.Error(logMessage);
                //    return false;
                //}

                // 製造品目：確定処理
                //if (productFetchDivision == "1")
                //{
                //    DateTime? tmp_date1 = null;
                //    DateTime? tmp_date2 = null;
                //    DateTime? tmp_date3 = null;
                //    DateTime try_date;
                //    int flg_date;
                //    // 確定取込品目の日付を並べ替える
                //    tmp_date1 = null;
                //    tmp_date2 = null;
                //    tmp_date3 = null;
                //    if (productDeliveryLimit.CompareTo(productFetchDtFrom) <= 0)
                //    {
                //        // 画面納期 <= 開始納期  < 終了納期
                //        if (DateTime.TryParseExact(productDeliveryLimit, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date1 = try_date;
                //        }
                //        if (DateTime.TryParseExact(productFetchDtFrom, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date2 = try_date;
                //        }
                //        if (DateTime.TryParseExact(productFetchDtTo, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date3 = try_date;
                //        }
                //        flg_date = 0;
                //    }
                //    else if (productFetchDtTo.CompareTo(productDeliveryLimit) < 0)
                //    {
                //        // 開始納期  < 終了納期 < 画面納期
                //        if (DateTime.TryParseExact(productFetchDtFrom, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date1 = try_date;
                //        }
                //        if (DateTime.TryParseExact(productFetchDtTo, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date2 = try_date;
                //        }
                //        if (DateTime.TryParseExact(productDeliveryLimit, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date3 = try_date;
                //        }
                //        flg_date = 1;
                //    }
                //    else
                //    {
                //        // 開始納期 < 画面納期  < 終了納期
                //        if (DateTime.TryParseExact(productFetchDtFrom, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date1 = try_date;
                //        }
                //        if (DateTime.TryParseExact(productDeliveryLimit, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            // 日付をマイナス１
                //            tmp_date2 = try_date.AddDays(-1);
                //        }
                //        if (DateTime.TryParseExact(productFetchDtTo, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date3 = try_date;
                //        }
                //        flg_date = 2;
                //    }
                //    if (string.IsNullOrEmpty(productDeliveryLimit) && string.IsNullOrEmpty(productFetchDtFrom) && string.IsNullOrEmpty(productFetchDtTo))
                //    {
                //        tmp_date1 = null;
                //        tmp_date2 = null;
                //        tmp_date3 = null;
                //        flg_date = 3;
                //    }

                //    // ログ出力:製造品目 取込オーダー 確定処理 実行
                //    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10048, ComID.MB10052, ComID.MB10004, ComID.MB10061, "" }, languageId: pInfo.LanguageId);
                //    logger.Info(logMessage);

                //    // 取込オーダー確定処理
                //    ret = PakFixedFixFetchOrder(
                //        db
                //        , 0
                //        , productFixCondition
                //        , tmp_date1
                //        , tmp_date2
                //        , tmp_date3
                //        , DateTime.Parse(productDeliveryLimit)
                //        , flg_date
                //        , pInfo
                //    );
                //    if (ret == false)
                //    {
                //        // ログ出力:製造品目 取込オーダー 確定処理 異常終了
                //        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10048, ComID.MB10052, ComID.MB10004, ComID.MB10062, "" }, languageId: pInfo.LanguageId);
                //        logger.Error(logMessage);
                //        return false;
                //    }
                //    // ログ出力:製造品目 取込オーダー 確定処理 正常終了
                //    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10048, ComID.MB10052, ComID.MB10004, ComID.MB10060, "" }, languageId: pInfo.LanguageId);
                //    logger.Info(logMessage);

                //} // 製造品目：確定処理

                // 購買品目：確定処理
                //if (purchaseFetchDivision == "1")
                //{
                //    // ログ出力:購買品目 取込オーダー 確定処理 開始
                //    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10049, ComID.MB10052, ComID.MB10004, ComID.MB10059, "" }, languageId: pInfo.LanguageId);
                //    logger.Info(logMessage);

                //    DateTime? tmp_date1 = null;
                //    DateTime? tmp_date2 = null;
                //    DateTime? tmp_date3 = null;
                //    DateTime try_date;
                //    int flg_date;
                //    // 確定取込品目の日付を並べ替える
                //    tmp_date1 = null;
                //    tmp_date2 = null;
                //    tmp_date3 = null;
                //    if (purchaseDeliveryLimit.CompareTo(purchaseFetchDtFrom) <= 0)
                //    {
                //        // 画面納期 <= 開始納期  < 終了納期
                //        if (DateTime.TryParseExact(purchaseDeliveryLimit, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date1 = try_date;
                //        }
                //        if (DateTime.TryParseExact(purchaseFetchDtFrom, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date2 = try_date;
                //        }
                //        if (DateTime.TryParseExact(purchaseFetchDtTo, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date3 = try_date;
                //        }
                //        flg_date = 0;
                //    }
                //    else if (purchaseFetchDtTo.CompareTo(purchaseDeliveryLimit) < 0)
                //    {
                //        // 開始納期  < 終了納期 < 画面納期
                //        if (DateTime.TryParseExact(purchaseFetchDtFrom, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date1 = try_date;
                //        }
                //        if (DateTime.TryParseExact(purchaseFetchDtTo, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date2 = try_date;
                //        }
                //        if (DateTime.TryParseExact(purchaseDeliveryLimit, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date3 = try_date;
                //        }
                //        flg_date = 1;
                //    }
                //    else
                //    {
                //        // 開始納期 < 画面納期  < 終了納期
                //        if (DateTime.TryParseExact(purchaseFetchDtFrom, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date1 = try_date;
                //        }
                //        if (DateTime.TryParseExact(purchaseDeliveryLimit, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            // 日付をマイナス１
                //            tmp_date2 = try_date.AddDays(-1);
                //        }
                //        if (DateTime.TryParseExact(purchaseFetchDtTo, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date3 = try_date;
                //        }
                //        flg_date = 2;
                //    }
                //    if (string.IsNullOrEmpty(purchaseDeliveryLimit) && string.IsNullOrEmpty(purchaseFetchDtFrom) && string.IsNullOrEmpty(purchaseFetchDtTo))
                //    {
                //        tmp_date1 = null;
                //        tmp_date2 = null;
                //        tmp_date3 = null;
                //        flg_date = 3;
                //    }

                //    // ログ出力:購買品目 取込オーダー 確定処理 実行
                //    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10049, ComID.MB10052, ComID.MB10004, ComID.MB10061, "" }, languageId: pInfo.LanguageId);
                //    logger.Info(logMessage);

                //    // 取込オーダー確定処理
                //    ret = PakFixedFixFetchOrder(
                //        db
                //        , 1
                //        , purchaseFixCondition
                //        , tmp_date1
                //        , tmp_date2
                //        , tmp_date3
                //        , DateTime.Parse(purchaseDeliveryLimit)
                //        , flg_date
                //        , pInfo
                //    );
                //    if (ret == false)
                //    {
                //        // ログ出力:購買品目 取込オーダー 確定処理 異常終了
                //        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10049, ComID.MB10052, ComID.MB10004, ComID.MB10062, "" }, languageId: pInfo.LanguageId);
                //        logger.Error(logMessage);
                //        return false;
                //    }
                //    // ログ出力:購買品目 取込オーダー 確定処理 正常終了
                //    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10049, ComID.MB10052, ComID.MB10004, ComID.MB10060, "" }, languageId: pInfo.LanguageId);
                //    logger.Info(logMessage);

                //} // 購買品目：確定処理

                // 外注品目：確定処理
                //if (orderFetchDivision == "1")
                //{
                //    DateTime? tmp_date1 = null;
                //    DateTime? tmp_date2 = null;
                //    DateTime? tmp_date3 = null;
                //    DateTime try_date;
                //    int flg_date;
                //    // 確定取込品目の日付を並べ替える
                //    tmp_date1 = null;
                //    tmp_date2 = null;
                //    tmp_date3 = null;
                //    if (orderDeliveryLimit.CompareTo(orderFetchDtFrom) <= 0)
                //    {
                //        // 画面納期 <= 開始納期  < 終了納期
                //        if (DateTime.TryParseExact(orderDeliveryLimit, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date1 = try_date;
                //        }
                //        if (DateTime.TryParseExact(orderFetchDtFrom, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date2 = try_date;
                //        }
                //        if (DateTime.TryParseExact(orderFetchDtTo, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date3 = try_date;
                //        }
                //        flg_date = 0;
                //    }
                //    else if (orderFetchDtTo.CompareTo(orderDeliveryLimit) < 0)
                //    {
                //        // 開始納期  < 終了納期 < 画面納期
                //        if (DateTime.TryParseExact(orderFetchDtFrom, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date1 = try_date;
                //        }
                //        if (DateTime.TryParseExact(orderFetchDtTo, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date2 = try_date;
                //        }
                //        if (DateTime.TryParseExact(orderDeliveryLimit, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date3 = try_date;
                //        }
                //        flg_date = 1;
                //    }
                //    else
                //    {
                //        // 開始納期 < 画面納期  < 終了納期
                //        if (DateTime.TryParseExact(orderFetchDtFrom, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date1 = try_date;
                //        }
                //        if (DateTime.TryParseExact(orderDeliveryLimit, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            // 日付をマイナス１
                //            tmp_date2 = try_date.AddDays(-1);
                //        }
                //        if (DateTime.TryParseExact(orderFetchDtTo, "yyyy/MM/dd", null, DateTimeStyles.AssumeLocal, out try_date))
                //        {
                //            tmp_date3 = try_date;
                //        }
                //        flg_date = 2;
                //    }
                //    if (string.IsNullOrEmpty(orderDeliveryLimit) && string.IsNullOrEmpty(orderFetchDtFrom) && string.IsNullOrEmpty(orderFetchDtTo))
                //    {
                //        tmp_date1 = null;
                //        tmp_date2 = null;
                //        tmp_date3 = null;
                //        flg_date = 3;
                //    }

                //    // ログ出力:外注品目 取込オーダー 確定処理 実行
                //    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10050, ComID.MB10052, ComID.MB10004, ComID.MB10061, "" }, languageId: pInfo.LanguageId);
                //    logger.Info(logMessage);

                //    // 取込オーダー確定処理
                //    ret = PakFixedFixFetchOrder(
                //        db
                //        , 2
                //        , orderFixCondition
                //        , tmp_date1
                //        , tmp_date2
                //        , tmp_date3
                //        , DateTime.Parse(orderDeliveryLimit)
                //        , flg_date
                //        , pInfo
                //    );
                //    if (ret == false)
                //    {
                //        // ログ出力:外注品目 取込オーダー 確定処理 異常終了
                //        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10050, ComID.MB10052, ComID.MB10004, ComID.MB10062, "" }, languageId: pInfo.LanguageId);
                //        logger.Error(logMessage);
                //        return false;
                //    }
                //} // 外注品目：確定処理

                //// ログ出力:外注品目 取込オーダー 確定処理 正常終了
                //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10050, ComID.MB10052, ComID.MB10004, ComID.MB10060, "" }, languageId: pInfo.LanguageId);
                //logger.Info(logMessage);

                //} // 取込オーダー作成処理
                //catch (Exception ex)
                //{
                //    // ログ出力:取込オーダー 確定処理 異常終了
                //    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10052, ComID.MB10004, ComID.MB10062, "", "" }, languageId: pInfo.LanguageId);
                //    logger.Error(logMessage);
                //    logger.Error(ex.Message);
                //    return false;
                //}

                // ログ出力:取込オーダー 確定処理 正常終了
                //logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10052, ComID.MB10004, ComID.MB10060, "", "" }, languageId: pInfo.LanguageId);
                //logger.Info(logMessage);

                // 承認ワークフロー削除処理
                ret = PakFixedWorkFlowDelete(db, productProductDivision, purchaseProductDivision, orderProductDivision);
                if (ret == false)
                {
                    // ログ出力:承認ワークフロー削除処理 エラー
                    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10180, ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                    logger.Error(logMessage);
                    errMessage.Add(logMessage);
                    return new ResultInfo(true, logMessage);
                }

                if (mstErrorFlg)
                {
                    // 正常終了(作成できない確定処理データ有り)
                    return new ResultInfo(false, ComST.GetPropertiesMessage(ComID.MB10277, paramInfo.LanguageId));
                }
                // 正常終了
                return new ResultInfo();
            }
            catch (Exception ex)
            {
                // ログ出力:確定処理 エラー
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10004, ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                errMessage.Add(logMessage);
                logMessage = "";
                logger.Error(ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(ex.Message);
                errMessage.Add(ex.ToString());
                return new ResultInfo(true, logMessage);
            }
        }
        #endregion

        #region レベル３
        /// <summary>
        /// 確定処理：テーブルロック
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        public static bool PakFixedLockTable(
             ComDB db, PakDao.ComParamInfo paramInfo)
        {
            string sql = "";         // 動的SQL
            try
            {

                // FIXED_PLAN は不要なのでロック処理を省略

                // PURCHASE_PLAN → mrp_result にテーブル名を変更
                sql = "";
                sql = sql + "select ";
                sql = sql + "    item_cd ";
                sql = sql + "from ";
                sql = sql + "    mrp_result ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and status = 0 ";
//                sql = sql + "and order_rule in (3, 6) ";
                sql = sql + "for update nowait ";

                IList<string> resultList = null;
                string results = null;
                resultList = db.GetList<string>(sql);
                if (resultList != null)
                {
                    for (int i = 0; i < resultList.Count; i++)
                    {
                        results = resultList[i];
                    }
                }
                // 正常終了
                return true;

            }
            catch (Exception ex)
            {
                // ログ出力:テーブルロック 異常終了
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10053 }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                logger.Error(ex.Message);
                logger.Error(ex.ToString());
                return false;

            }
        }

        /// <summary>
        /// 製造品目：製造オーダー確定処理（発注日指定）
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="mode">モード 0:製造 1:購買 2:外注</param>
        /// <param name="condition">条件</param>
        /// <param name="dateFrom">開始日付</param>
        /// <param name="dateTo">終了日付</param>
        /// <param name="deliverLimit">発注期限</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <param name="mstErrorFlg">マスタチェックエラーフラグ</param>
        /// <param name="batUserId">バッチ実行ユーザ</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        public static bool PakFixedFixProductionOrder(
            ComDB db,
            int mode,
            string condition,
            DateTime? dateFrom,
            DateTime? dateTo,
            DateTime? deliverLimit,
            PakDao.ComParamInfo paramInfo,
            ref List<string> errMessage,
            ref bool mstErrorFlg,
            string batUserId)
        {
            string sql;         // 動的SQL
            bool ret = false;
            int ret_cd = 0;
            int sub_cost = 0;
            int cost_mang = 0;
            int cnt = 0;
            int unissuedCnt = 0;
            DateTime tmpDateFrom = DateTime.Now;
            DateTime tmpDateTo = DateTime.Now;

            try
            {
                sql = "";
                sql = sql + "select distinct ";
                sql = sql + "     pp.item_cd ";                 // 品目コード
                sql = sql + "    ,pp.specification_cd ";        // 仕様コード
                sql = sql + "    ,pp.delivery_date ";           // 納期
                sql = sql + "    ,pp.plan_no ";                 // 計画番号
                sql = sql + "    ,pp.procedure_division ";      // 手続区分
                sql = sql + "    ,pp.order_publish_division ";  // オーダー発行区分
                sql = sql + "    ,pp.order_date ";              // 購入依頼日
                sql = sql + "    ,pp.order_rule ";              // 発注基準
                sql = sql + "    ,pp.deliverlimit ";            // 計画納期
                sql = sql + "    ,pp.plan_qty ";                // 計画数量
                sql = sql + "    ,pp.production_plan_no ";      // 生産計画番号
                sql = sql + "    ,pp.reference_no ";            // リファレンス番号
                //sql = sql + "    ,pp.location_division_cd ";
                sql = sql + "    ,pp.location_cd ";             // 納入ロケーションコード
                sql = sql + "    ,pp.vender_cd ";               // オーダー先コード
                sql = sql + "    ,pp.remark ";                  // 備考
                sql = sql + "    ,im.item_cd         im_code "; // 品目コード
                sql = sql + "    ,im.item_name       im_hinnm "; // 品目名称
                sql = sql + "    ,null               imaccd ";  // 科目コード
                sql = sql + "    ,null               imhojo ";  // 補助科目コード
                sql = sql + "    ,null               imeflg4 "; // 生産中止区分|0:生産,1～9:中止(主に"3"で中止)
                sql = sql + "    ,null               imgenka "; // 原価区分|0～6
                sql = sql + "    ,null               imtest ";  // 基準検査方法|0:無検査,1:抜取検査,2:全品検査(未使用)
                sql = sql + "    ,null               imzaiko "; // 在庫管理区分|0:通常,1:在庫表除外,2:更新除外

                sql = sql + "    ,im.active_date ";                                 // 有効日付
                sql = sql + "    ,sp.specification_name ";                          // 仕様名称
                sql = sql + "    ,sp.default_location ";                            // 基準保管場所
                sql = sql + "    ,sp.item_type ";                                   // 品目タイプ
                sql = sql + "    ,lc.location_name default_location_name ";         // 基準保管場所名称

                sql = sql + "    ,iz.item_cd         iz_code ";                     // 品目コード
                sql = sql + "    , @UserId plan_tanto_cd  ";          // 計画担当者コード
                if (mode == 0)
                {
                    sql = sql + "    ,null wccode ";  // 作業区コード
                    sql = sql + "    ,null wcbumon "; // 部門コード
                    sql = sql + "    ,null ts_code ";
                    sql = sql + "    ,null tsnyobi1 ";
                    sql = sql + "    ,null tsnyobi2 ";
                    sql = sql + "    ,null ts_tanto ";
                    sql = sql + "    ,null ts_name ";
                    sql = sql + "    ,null ta_code ";
                    sql = sql + "    ,null ta_bumon ";
                }
                else
                {
                    sql = sql + "    ,null wccode ";
                    sql = sql + "    ,null wcbumon ";
                    sql = sql + "    ,ts.vender_cd    ts_code ";        // 取引先コード
                    sql = sql + "    ,null tsnyobi1 ";   // 納入曜日１(未使用)|0:日 1:月 2:火 3:水 4:木 5:金 6:土
                    sql = sql + "    ,null tsnyobi2 ";   // 納入曜日２(未使用)|0:日 1:月 2:火 3:水 4:木 5:金 6:土
//                    sql = sql + "    ,ts.tanto_cd      ts_tanto ";
                    sql = sql + "    ,ts.user_id      ts_tanto ";       // 担当者コード
                    sql = sql + "    ,ts.vender_name1 vender_name ";    // 取引先名称
//                    sql = sql + "    ,ta.tanto_cd     ta_code ";
                    sql = sql + "    ,ta.user_id     ta_code ";         // 担当者コード
                    sql = sql + "    ,ta.organization_cd   ta_bumon ";  // 部署コード
                }
                sql = sql + "from ";
                sql = sql + "    mrp_result pp "; // purchase_plan → mrp_result にテーブル名を変更
                sql = sql + "    left outer join ";
                //                sql = sql + "        item im ";
                //                sql = sql + "        v_item im ";
                // active_date 対応 品目マスタの有効年月日がMRP結果の計画納期以内で最新のもの
                sql = sql + "        ( ";
                sql = sql + "            select ";
                sql = sql + "                temp.* ";
                sql = sql + "            from ";
                sql = sql + "            ( ";
                sql = sql + "                select ";
                sql = sql + "                    itm.* ";
                sql = sql + "                    ,row_number() over (partition by itm.item_cd order by itm.active_date desc) as num ";
                sql = sql + "                from ";
                sql = sql + "                    (select * from v_item order by item_cd, active_date) as itm ";
                sql = sql + "                    , mrp_result mrp ";
                sql = sql + "                where ";
                sql = sql + "                    itm.item_cd = mrp.item_cd ";
                sql = sql + "                    and itm.active_date <= mrp.deliverlimit ";
                sql = sql + "                    and itm.language_id = 'ja' ";
                sql = sql + "                    and itm.activate_flg = 1 ";
                sql = sql + "                    and itm.del_flg = 0 ";
                sql = sql + "            ) as temp ";
                sql = sql + "            where ";
                sql = sql + "                1 = 1 ";
                sql = sql + "            and temp.num = 1 ";
                sql = sql + "        ) im ";
                sql = sql + "    on pp.item_cd = im.item_cd ";
                sql = sql + "    left outer join ";
                //                sql = sql + "        v_item_specification sp ";
                // active_date 対応 品目仕様マスタの有効年月日がMRP結果の計画納期以内で最新のもの
                sql = sql + "        ( ";
                sql = sql + "            select ";
                sql = sql + "                temp.* ";
                sql = sql + "            from ";
                sql = sql + "            ( ";
                sql = sql + "                select ";
                sql = sql + "                    spec.* ";
                sql = sql + "                    ,row_number() over (partition by spec.item_cd, spec.specification_cd order by spec.active_date desc) as num ";
                sql = sql + "                from ";
                sql = sql + "                    (select * from v_item_specification order by item_cd, specification_cd, active_date)  as spec ";
                sql = sql + "                    , mrp_result mrp ";
                sql = sql + "                where ";
                sql = sql + "                    spec.item_cd = mrp.item_cd ";
                sql = sql + "                    and spec.specification_cd = mrp.specification_cd ";
                sql = sql + "                    and spec.active_date <= mrp.deliverlimit ";
                sql = sql + "                    and spec.language_id = 'ja' ";
                sql = sql + "                    and spec.activate_flg = 1 ";
                sql = sql + "                    and spec.del_flg = 0 ";
                sql = sql + "            ) as temp ";
                sql = sql + "            where ";
                sql = sql + "                1 = 1 ";
                sql = sql + "            and temp.num = 1 ";
                sql = sql + "        ) sp ";

                sql = sql + "    on pp.item_cd = sp.item_cd and pp.specification_cd = sp.specification_cd ";
                sql = sql + "    left outer join ";
                sql = sql + "        v_item_inventory iz "; // ビューに置き換え
                sql = sql + "    on pp.item_cd = iz.item_cd ";
                sql = sql + "    left outer join ";
                sql = sql + "        location lc ";
                sql = sql + "    on sp.default_location = lc.location_cd "; // 基準保管場所
                if (mode == 0)
                {
                    // work_center は廃止
                    //sql = sql + "    left outer join ";
                    //sql = sql + "        work_center wc ";
                    //sql = sql + "    on pp.vender_cd = wc.work_center_cd ";
                }
                else
                {
                    sql = sql + "    left outer join ";
                    sql = sql + "        vender ts ";                   // 取引先マスタ
                    sql = sql + "    on pp.vender_cd = ts.vender_cd and ts.vender_division = 'SI' ";
                    sql = sql + "    left outer join ";
                    sql = sql + "        belong  ta ";                  // 所属マスタ
                    sql = sql + "    on ts.user_id = ta.user_id ";
                }
                sql = sql + " where ";
//                sql = sql + "     pp.item_cd = pp.item_cd ";
                sql = sql + "     1 = 1 ";

                // 発注日指定
                if (condition == "0")
                {
                    if (dateFrom != null)
                    {
                        tmpDateFrom = (DateTime)dateFrom;
                        sql = sql + "and to_char(pp.order_date,'yyyymmdd') >= @TmpDateFrom ";
                    }
                    else
                    {
                        // 指定がない場合、システム日付
                        sql = sql + "and to_char(pp.order_date,'yyyymmdd') >= to_char(Now(), 'yyyymmdd') ";

                    }
                    if (dateTo != null)
                    {
                        tmpDateTo = (DateTime)dateTo;
                        sql = sql + "and to_char(pp.order_date,'yyyymmdd') <= @TmpDateTo ";
                    }
                }
                else
                {
                    // 納入日指定
                    if (dateFrom != null)
                    {
                        tmpDateFrom = (DateTime)dateFrom;
                        sql = sql + "and to_char(pp.delivery_date,'yyyymmdd') >= @TmpDateFrom ";
                    }
                    else
                    {
                        // 指定がない場合、システム日付
                        sql = sql + "and to_char(pp.delivery_date,'yyyymmdd') >= to_char(Now(), 'yyyymmdd') ";

                    }
                    if (dateTo != null)
                    {
                        tmpDateTo = (DateTime)dateTo;
                        sql = sql + "and to_char(pp.delivery_date,'yyyymmdd') <= @TmpDateTo ";
                    }
                }
                if (mode == 0)
                {
                    // 製造
                    sql = sql + " and pp.procedure_division = 1 ";
                    sql = sql + " and sp.plan_division = 1 ";       // 品目仕様.計画区分 = 1:製造
                    sql = sql + " and pp.status = 0 ";
                    sql = sql + " order by  ";
                    sql = sql + "     pp.order_date ";
                }
                else if (mode == 1)
                {
                    // 購買
                    sql = sql + " and pp.procedure_division = 2 ";
                    //sql = sql + " and im.cost_division in (4,5,6) ";
                    sql = sql + " and sp.plan_division = 2 ";       // 品目仕様.計画区分 = 2:購買
                    sql = sql + " and pp.status = 0 ";
                    sql = sql + " order by  ";
                    sql = sql + "     pp.delivery_date ";
                }
                else
                {
                    // 外注
                    sql = sql + " and pp.procedure_division = 2 ";
                    //sql = sql + " and im.cost_division in (2,3) ";
                    sql = sql + " and sp.plan_division = 3 ";       // 品目仕様.計画区分 = 3:外注
                    sql = sql + " and pp.status = 0 ";
                    sql = sql + " order by  ";
                    sql = sql + "     pp.delivery_date ";
                }
                //sql = sql + " and pp.status = 0 ";                  // ステータス（FXKB）0:未確定
                //sql = sql + " and pp.order_rule in (3,6) ";         // 発注基準 3:定期、6:個別←シャチハタ固有みたいなのでコメント
                //sql = sql + " order by  ";
                //sql = sql + "     pp.order_date ";

                // 自社マスタの検索 不要 2021.09.01
                //try
                //{
                //    string sql_company;         // 動的SQL
                //    sql_company = "";
                //    sql_company = sql_company + "select ";
                //    sql_company = sql_company + "    * ";
                //    //sql_company = sql_company + "    manufacturing_subcontract_cost ";// 製造外注原価項目区分|２～５のみ入力可 (2)
                //    //sql_company = sql_company + "    ,cost_update_management ";       // 発生原価更新管理区分|0:する 1:しない(1)
                //    sql_company = sql_company + "from ";
                //    sql_company = sql_company + "    company ";
                //    ComDao.CompanyEntity company = db.GetEntityByDataClass<ComDao.CompanyEntity>(sql_company);
                //    if (company == null)
                //    {
                //        // TODO:ログ出力
                //        // TODO:wErrNum = 1
                //        return false;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    // TODO:ログ出力
                //    // TODO:wErrNum = 1
                //    return false;
                //}

                // 動的SQL実行
                IList<Dao.FixProductionOrder> cur_fixed = db.GetListByDataClass<Dao.FixProductionOrder>(
                    sql,
                    new
                    {
                        UserId = paramInfo.UserId,
                        TmpDateFrom = tmpDateFrom.ToString("yyyyMMdd"),
                        TmpDateTo = tmpDateTo.ToString("yyyyMMdd")
                    });

                // 購入依頼番号配列
                Dictionary<string, string> slipDictionary = new Dictionary<string, string>();

                foreach (var rec_fixed in cur_fixed)
                {
                    string buf;
                    buf = rec_fixed.ItemCd;

                    // 発注数<=０のオーダーは確定処理を行わない
                    if (rec_fixed.PlanQty <= 0)
                    {
                        logger.Error("【未発行】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]：発注数 <= 0 ");
                        errMessage.Add("【未発行】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]：発注数 <= 0 ");

                        mstErrorFlg = true;
                        unissuedCnt += 1;
                        continue;
                    }

                    // マスタのチェック
                    ret = PakFixedCheckMaster(
                        mode,
                        0,
                        rec_fixed,
                        paramInfo,
                        batUserId,
                        db);
                    if (ret == false)
                    {
                        errMessage.Add(logMessage);
                        mstErrorFlg = true;
                        unissuedCnt += 1;
                        continue;
                    }

                    if (mode == 0)
                    {
                        // 製造
                        // 製造のみBOMチェックするよう修正
                        // BOMのチェック
                        ret = PakFixedCheckBom(
                            db,
                            rec_fixed.PlanNo,
                            rec_fixed.ImCode,
                            //, rec_fixed.DeliveryDate.GetValueOrDefault().ToString("yyyy/MM/dd")
                            rec_fixed.OrderDate.GetValueOrDefault().ToString("yyyy/MM/dd"), // 計画発注日に変更
                            paramInfo,
                            ref errMessage);
                        if (ret == false)
                        {
                            logMessage = "【未発行】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]：" + ComST.GetPropertiesMessage(new string[] { ComID.M00306, rec_fixed.ItemCd, ComID.I10092 }, paramInfo.LanguageId);
                            logger.Error(logMessage);
                            errMessage.Add(logMessage);

                            mstErrorFlg = true;
                            unissuedCnt += 1;
                            continue;
                        }
                    }
                    if (mode == 0)
                    {
                        // 製造
                        // PakFixedFixProductionSub()のなかで行う
                        // 製造オーダー番号
                        //wkodrNo = APComUtil.getNumber(db, Constants.SEQUENCE_PROC_NAME.DIRECTION_NO);
                        //if (ret_cd != 0)
                        //{
                        //    // TODO:ログ出力
                        //    // TODO:wErrNum := 1;
                        //    return false;
                        //}
                    }
                    else
                    {
                        // 購買、外注
                        // 発注オーダー番号
                        //wkodrNo = APComUtil.getNumber(db, Constants.SEQUENCE_PROC_NAME.PURCHASE_REQUEST);
                        //if (ret_cd != 0)
                        //{
                        //    // TODO:ログ出力
                        //    // wErrNum := 1;
                        //    return false;
                        //}
                    }
                    //wkPDATE = rec_fixed.DeliveryDate;
                    // オーダーが生産計画そのものである時、生産計画ファイル更新

                    if (rec_fixed.ProductionPlanNo != "0")
                    {
                        // 発注予測と生産計画とで分岐
                        if (rec_fixed.OrderPublishDivision == "13")
                        {
                            // 新APはないのでコメントアウト
                            // 対象の発注予測データを[1:確定済]に更新
                            //sql = "";
                            //sql = sql + "update purchase_forecast ";
                            //sql = sql + "set    status      = 1 ";
                            //sql = sql + "      ,screen_id   = " + screenId.ToString();
                            //sql = sql + "      ,update_date = Now() ";
                            //sql = sql + "      ,updator_cd  = '" + updatorCd + "' ";
                            //sql = sql + "where  ";
                            //sql = sql + "    purchase_forecast_no = '" + rec_fixed.ProductionPlanNo + "' ";
                        }
                        else
                        {
                            // 対象データを[1:確定済]に更新
                            sql = "";
                            sql = sql + "update production_plan ";
                            sql = sql + "set    status      = 1 ";
                            //sql = sql + "      ,screen_id   = " + screenId.ToString();
                            sql = sql + "      ,update_date = Now() ";
                            sql = sql + "      ,update_user_id  = @UserId ";
                            sql = sql + "where  ";
                            sql = sql + "    production_plan_no = @ProductionPlanNo ";

                        }
                        // 更新登録
                        ret_cd = db.Regist(
                            sql,
                            new
                            {
                                UserId = paramInfo.UserId,
                                ProductionPlanNo = rec_fixed.ProductionPlanNo
                            });
                    } // if (result.ProductionPlanNo != "0")

                    // オーダーの作成
                    // 製造
                    if (mode == 0)
                    {
                        // 製造確定処理
                        ret = PakFixedFixProductionSub(
                            db,
                            rec_fixed,
                            false,
                            sub_cost,
                            cost_mang,
                            0,
                            rec_fixed.PlanQty.GetValueOrDefault(),
                            deliverLimit,
                            paramInfo,
                            ref slipDictionary);
                        if (ret == false)
                        {
                            // 製造確定処理内でのエラーメッセージ
                            errMessage.Add(logMessage);
                            // ログ出力:製造確定処理 エラー
                            logMessage = "【エラー】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]：" + ComST.GetPropertiesMessage(new string[] { ComID.MB10058, "製造確定処理", ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                            logger.Error(logMessage);
                            errMessage.Add(logMessage);
                            return false;
                        }
                    }
                    else
                    {
                        // 購買、外注
                        // 発注確定処理
                        ret = PakFixedFixOrderSub(
                            db,
                            rec_fixed,
                            rec_fixed.PlanQty.GetValueOrDefault(),
                            rec_fixed.DeliveryDate.GetValueOrDefault(),
                            false,
                            0,
                            paramInfo,
                            ref slipDictionary);
                        if (ret == false)
                        {
                            // 発注確定処理内でのエラーメッセージ
                            errMessage.Add(logMessage);
                            // ログ出力:発注確定処理 エラー
                            logMessage = "【エラー】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10058, "発注確定処理", ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                            logger.Error(logMessage);
                            errMessage.Add(logMessage);
                            return false;
                        }
                    }
                    // if (mode == 0)

                    // 確定ファイル作成(2:製造, 3:購買外注)
                    // MakeFixedPlan() は廃止
                    //if (ret == false)
                    //{
                    //    // TODO:ログ出力
                    //    return false;
                    //}

                    // 発注計画ファイル更新 ステータス=1:確定
                    // PURCHASE_PLAN → mrp_result にテーブル名変更
                    sql = "";
                    sql = sql + "update mrp_result ";
                    sql = sql + "set    status      = 1 ";
                    //sql = sql + "      ,screen_id   = " + screenId.ToString();
                    sql = sql + "      ,update_date = Now() ";
                    sql = sql + "      ,update_user_id  = '" + paramInfo.UserId + "' ";
                    sql = sql + "where  ";
                    sql = sql + "    plan_no = '" + rec_fixed.PlanNo + "' ";
                    // 更新登録
                    ret_cd = db.Regist(
                        sql,
                        new
                        {
                            UserId = paramInfo.UserId,
                            PlanNo = rec_fixed.PlanNo
                        });

                    cnt = cnt + 1;

                } // foreach(var result in results)

                // ログ出力：製造オーダー確定処理 未発行
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10004, "未発行", unissuedCnt.ToString(), "件", "" }, languageId: paramInfo.LanguageId);
                logger.Info(logMessage);
                errMessage.Add(logMessage);

                string workflowUtDiv = "PPL03";
                string workflowDiv = "MAN01";
                string conductId = "MAN0010";
                if (mode == 1)
                {
                    workflowUtDiv = "PPL04";
                    workflowDiv = "ORD01";
                    conductId = "ORD0010";
                }
                else if (mode == 2)
                {
                    // 外注の場合
                    workflowUtDiv = "PPL05";
                    workflowDiv = "ORD01";
                    conductId = "ORD0010";
                }

                // 一括承認
                if (!APComUtil.repeatApprovalRequest(workflowUtDiv, workflowDiv, slipDictionary, db, paramInfo.LanguageId, paramInfo.UserId, conductId, "1", null, null, null))
                {
                    // ログ出力：承認WF作成処理 エラー
                    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.IS00038, ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                    logger.Info(logMessage);
                    errMessage.Add(logMessage);
                    return false;
                }

                // ログ出力：製造オーダー確定処理 正常終了
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10004, ComID.MB10060, cnt.ToString(), "件", "" }, languageId: paramInfo.LanguageId);
                logger.Info(logMessage);
                errMessage.Add(logMessage);
                return true;

            }
            catch (Exception ex)
            {
                // ログ出力：製造オーダー確定処理 エラー
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10051, ComID.MB10004, ComID.MB00022, "", "" }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                errMessage.Add(logMessage);
                logMessage = "";
                logger.Error(ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(ex.Message);
                errMessage.Add(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 取込オーダー確定処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="mode">モード</param>
        /// <param name="condition">条件</param>
        /// <param name="date1">日付1</param>
        /// <param name="date2">日付2</param>
        /// <param name="date3">日付3</param>
        /// <param name="deliverLimit">発注期限</param>
        /// <param name="flag">先頭シーケンス</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool PakFixedFixFetchOrder(
            ComDB db,
            int mode,
            string condition,
            DateTime? date1,
            DateTime? date2,
            DateTime? date3,
            DateTime? deliverLimit,
            int flag,
            PakDao.ComParamInfo paramInfo)
        {

            bool ret = false;

            // ログ出力:取込オーダー 確定処理 開始
            logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10052, ComID.MB10004, ComID.MB10059, "", "" }, languageId: paramInfo.LanguageId);
            logger.Info(logMessage);

            // 指定日付以前のオーダーを対象
            // 画面指定納期が開始納期より小さいときはデータなし
            if (flag == 1 || flag == 2 || flag == 3)
            {
                ret = PakFixedFixFetchOrderMain(
                    db,
                    mode,
                    condition,
                    date1.GetValueOrDefault(),
                    date2.GetValueOrDefault(),
                    date3.GetValueOrDefault(),
                    deliverLimit.GetValueOrDefault(),
                    flag,
                    0, // 丸め
                    0, // 内示確定
                    paramInfo);
                if (ret == false)
                {
                    return false;
                }
            }

            // まるめをしない確定取込オーダー作成

            if (mode == 0 || mode == 1)
            {
                // 製造、購買
                // 画面指定納期が終了納期より大きいときはデータなし
                if (flag == 0 || flag == 2 || flag == 3)
                {
                    ret = PakFixedFixFetchOrderMain(
                        db,
                        mode,
                        condition,
                        date1.GetValueOrDefault(),
                        date2.GetValueOrDefault(),
                        date3.GetValueOrDefault(),
                        deliverLimit.GetValueOrDefault(),
                        flag,
                        1,
                        0,
                        paramInfo);
                    if (ret == false)
                    {
                        return false;
                    }
                }
            } // if (mode == 0 || mode == 1)
            else
            {
                // 外注
                // 画面指定納期が開始納期より小さいときはデータなし
                if (flag == 1 || flag == 2 || flag == 3)
                {
                    ret = PakFixedFixFetchOrderMain(
                        db,
                        mode,
                        condition,
                        date1.GetValueOrDefault(),
                        date2.GetValueOrDefault(),
                        date3.GetValueOrDefault(),
                        deliverLimit.GetValueOrDefault(),
                        flag,
                        1,
                        0,
                        paramInfo);
                    if (ret == false)
                    {
                        return false;
                    }
                }
            }

            // まるめをする確定取込オーダー作成

            // 画面指定納期が終了納期より大きいときはデータなし
            if (flag == 1 || flag == 3)
            {
                ret = PakFixedFixFetchOrderMainMarume(
                    db,
                    mode,
                    condition,
                    date1.GetValueOrDefault(),
                    date2.GetValueOrDefault(),
                    date3.GetValueOrDefault(),
                    deliverLimit.GetValueOrDefault(),
                    flag,
                    paramInfo);
                if (ret == false)
                {
                    return false;
                }
            }

            // ログ出力:取込オーダー 確定処理 正常終了
            logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10052, ComID.MB10004, ComID.MB10060, "", "" }, languageId: paramInfo.LanguageId);
            logger.Info(logMessage);
            return true;

        }
        #endregion

        #region レベル４
        /// <summary>
        /// マスタのチェック
        /// </summary>
        /// <param name="mode">モード</param>
        /// <param name="naikaku">確定フラグ</param>
        /// <param name="rec_fixed">処理データ</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <param name="batUserId">バッチ実行ユーザ</param>
        /// <param name="db">データベース情報</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        public static bool PakFixedCheckMaster(
            int mode,
            int naikaku,
            Dao.FixProductionOrder rec_fixed,
            PakDao.ComParamInfo paramInfo,
            string batUserId,
            ComDB db)
        {

            // 品目マスタチェック
            if (string.IsNullOrEmpty(rec_fixed.ImCode))
            {
                // ログ出力(品目マスタ未登録 品目コード:)
                logMessage = "【未発行】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]：" + ComST.GetPropertiesMessage(new string[] { ComID.MS00084, ComID.I10087, ComID.I00776, rec_fixed.ItemCd }, paramInfo.LanguageId);
                logger.Error(logMessage);
                return false;
            }
            // 品目在庫チェック
            if (string.IsNullOrEmpty(rec_fixed.IzCode))
            {
                // ログ出力(品目在庫未登録 品目コード:)
                logMessage = "【未発行】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]：" + ComST.GetPropertiesMessage(new string[] { ComID.MS00084, ComID.I10088, ComID.I00776, rec_fixed.ItemCd }, paramInfo.LanguageId);
                logger.Error(logMessage);
                return false;
            }
            if (mode == 0)
            {
                if (naikaku == 0)
                {
                    // 作業区マスタチェック 廃止
                    //if (string.IsNullOrEmpty(rec_fixed.WcCode))
                }
            }
            else
            {
                // 基準仕入先コードが未設定の場合、担当者を購入依頼担当者に設定
                if (string.IsNullOrEmpty(rec_fixed.TsCode))
                {
                    rec_fixed.TsTanto = batUserId;
                }
                // 管理部署コードが未設定の場合、担当者の所属部署を設定
                if (string.IsNullOrEmpty(rec_fixed.TaBumon))
                {
                    // バッチ実行ユーザの所属部署を取得
                    string sql = "";
                    sql = sql + "select organization_cd ta_bumon ";
                    sql = sql + "from belong ";
                    sql = sql + "where ";
                    sql = sql + "    user_id = @UserId ";
                    sql = sql + "order by organization_cd ";
                    sql = sql + "limit 1 ";

                    // 動的SQL実行
                    IList<Dao.FixProductionOrder> cur_fixed = db.GetListByDataClass<Dao.FixProductionOrder>(
                        sql,
                        new
                        {
                            UserId = batUserId
                        });
                    if (cur_fixed.Count != 0 && cur_fixed != null)
                    {
                        rec_fixed.TaBumon = cur_fixed[0].TaBumon;
                    }
                }

                // 取引先マスタチェック
                //if (string.IsNullOrEmpty(rec_fixed.TsCode))
                //{
                //    // 取引先マスタ未登録 仕入先コード:
                //    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MS00084, ComID.I10089, ComID.I10091, rec_fixed.VenderCd }, pInfo.LanguageId);
                //    logger.Error(logMessage);
                //    return false;
                //}
                // 担当者マスタチェック(取引先マスタが未設定の場合スキップ)
                //if (string.IsNullOrEmpty(rec_fixed.TaCode) && !string.IsNullOrEmpty(rec_fixed.TsCode))
                //{
                //    // 担当者マスタ未登録  担当者コード:
                //    logMessage = "【未発行】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]：" + ComST.GetPropertiesMessage(new string[] { ComID.MS00084, ComID.I10090, ComID.I10001, rec_fixed.TaCode }, paramInfo.LanguageId);
                //    logger.Error(logMessage);
                //    return false;
                //}
            }
            return true;
        }

        /// <summary>
        /// BOM有無確認
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="planNo">計画番号</param>
        /// <param name="itemCd">品目コード</param>
        /// <param name="deliveryDate">納期</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <param name="errMessage">エラーメッセージリスト</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        public static bool PakFixedCheckBom(
            ComDB db,
            string planNo,
            string itemCd,
            string deliveryDate,
            PakDao.ComParamInfo paramInfo,
            ref List<string> errMessage)
        {
            int costDivision = 0;
            string sql = "";
            try
            {
                // 部品構成マスタのチェック
                // 部品構成マスタはレシピヘッダとレシピフォーミュラに移行
                sql = "";
                sql = sql + "select ";
                sql = sql + "    count(*) cnt ";
                sql = sql + "from ";
                sql = sql + "    recipe_header rh ";                    // レシピヘッダ
                sql = sql + "    inner join ";
                sql = sql + "        recipe_formula rf ";               // レシピフォーミュラ
                sql = sql + "    on rh.recipe_id = rf.recipe_id ";      // レシピID
                sql = sql + "    left outer join ";
                sql = sql + "        item im ";                         // 品目マスタ
                sql = sql + "    on rf.item_cd = im.item_cd ";          // 子品目(フォーミュラ)
                sql = sql + "    left outer join ";
                sql = sql + "        v_item_inventory  ii ";            // 在庫ファイル
                sql = sql + "    on rf.item_cd = ii.item_cd ";          // 子品目(フォーミュラ)
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and rh.item_cd = @ItemCd ";       // 親品目
                sql = sql + "and rf.result_division = 0 ";              // 実績区分 = 0:部品（子品目）
                sql = sql + "and rh.recipe_status <> 1 ";                // レシピステータス <> 1:研究用
                sql = sql + "and rh.activate_flg = 1 ";
                sql = sql + "and rh.del_flg = 0 ";

                if (string.IsNullOrEmpty(deliveryDate))
                {
                    sql = sql + "and rh.start_date <= @DeliveryDate";       // 有効開始日
                    sql = sql + "and (rh.start_date is null or ";
                    sql = sql + "     rh.start_date >= @DeliveryDate ";
                    sql = sql + "     ) ";
                }

                int cnt = db.GetEntity<int>(
                    sql,
                    new
                    {
                        ItemCd = itemCd,
                        DeliveryDate = deliveryDate
                    });

                // ITEM.COST_DIVISION 項目は現在存在しない
                if (costDivision == 0 || costDivision == 1 || costDivision == 2 || costDivision == 3)
                {
                    if (cnt > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // ログ出力:BOM チェック処理  異常終了
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.I10092, ComID.MB10064, ComID.MB00022, "", "" }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                errMessage.Add(logMessage);
                logger.Error(ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(ex.Message);
                errMessage.Add(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 製造確定処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="rec_fixed">処理データ</param>
        /// <param name="marumeFlg">まるめフラグ</param>
        /// <param name="manufacturingCost">原価項目</param>
        /// <param name="costUpdateManage">原価更新管理</param>
        /// <param name="naiKaku">確定フラグ</param>
        /// <param name="qty">計画数</param>
        /// <param name="deliverLimit">納期</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <param name="slipDictionary">伝票番号記憶用配列</param>
        /// <returns>(戻値)true:OKまたはfalse:NG</returns>
        public static bool PakFixedFixProductionSub(
            ComDB db,
            Dao.FixProductionOrder rec_fixed,
            bool marumeFlg,
            int manufacturingCost,
            int costUpdateManage,
            int naiKaku,
            decimal qty,
            DateTime? deliverLimit,
            PakDao.ComParamInfo paramInfo,
            ref Dictionary<string, string> slipDictionary)
        {
            string bk_hcd = null;
            bool ret = false;
            //int wkhkFlg = 0;
            string sql = "";
            //int intRet = 0;
            //decimal? wkQty = 0;
            //string wkEndSeirialNo = "";

            string directionNo = "";

            List<PakDao.TempPartsStructure> resultList = new List<PakDao.TempPartsStructure>();

            try
            {
                // 製造指図番号を採番
                directionNo = APComUtil.GetNumber(db, SeqDirectionNo);

                // 構成部品の引当用ファイル作成(確定オーダーのみ)
                if (naiKaku == 0)
                {
                    // 品目コードが変わったら１レベル展開を行う
                    if (bk_hcd == null || bk_hcd != rec_fixed.ItemCd)
                    {
                        //// １レベル部品展開
                        //ret = PakCommon.Exe1LevelPartsStructure(
                        //    db
                        //    , pInfo
                        //    , rec_fixed.ItemCd
                        //    , rec_fixed.SpecificationCd
                        //    , rec_fixed.DeliveryDate.GetValueOrDefault()
                        //    //, rec_fixed.OrderDate
                        //    , DateTime.MinValue
                        //    , DateTime.MaxValue
                        //    , 1
                        //    , 0
                        //    , pInfo.UserId
                        //    , pInfo.PgmId
                        //    //, client
                        //    //, 0
                        //    , true
                        //    , ref resultList
                        //);
                        //if (ret == false)
                        //{
                        //    return false;
                        //}

                        //wkhkFlg = 0; // 引当明細フラグ
                    } // if (bk_hcd == null || bk_hcd != rec_fixed.ItemCd)

                    // 親部品情報セット
                    Dao.TrnOwn workTrnOwn = new Dao.TrnOwn();
                    workTrnOwn.ProcedureDivision = rec_fixed.ProcedureDivision;
                    workTrnOwn.ItemCd = rec_fixed.ItemCd;
                    workTrnOwn.DeliverLimit = rec_fixed.Deliverlimit;
                    workTrnOwn.OrderDate = rec_fixed.OrderDate;
                    workTrnOwn.PlanQty = qty;
                    workTrnOwn.ReferenceNo = rec_fixed.ReferenceNo;
                    workTrnOwn.OrderNo = directionNo;
                    workTrnOwn.VenderCd = rec_fixed.VenderCd;
                    workTrnOwn.MarumeFlg = marumeFlg;

                } // if (naiKaku == 0)

                // 作業手順マスタ検索→製造オーダー明細の作成→工程内外注が存在するときはDMODを作成
                /*
                ret = PakFixedMakeDirectionProcedure(
                    db
                    , rec_fixed
                    , qty
                    , deliverLimit.GetValueOrDefault()
                    , manufacturingCost
                    , costUpdateManage
                    , naiKaku
                );
                if (ret == false)
                {
                    return false;
                }
                */

                // 発注残数の更新(確定オーダーのみ更新)
                // → 品目在庫は不要の為、廃止

                DateTime? plan_dt;
                // 確定取込のときは画面指定納期
                if (rec_fixed.OrderRule == 4)
                {
                    plan_dt = deliverLimit;
                }
                else
                {
                    // wkPDATEを廃止
                    //plan_dt = wkPDATE;
                    if (marumeFlg == false)
                    {
                        // marume == 0
                        plan_dt = rec_fixed.DeliveryDate.GetValueOrDefault();
                    }
                    else
                    {
                        plan_dt = deliverLimit;
                    }
                }
                int cnt = 0;
                //int publishflg = 0;

                // 品目マスタ検索
                sql = "";
                sql = sql + "select ";
                sql = sql + "    count(*) cnt ";
                sql = sql + "from ";
                sql = sql + "    item ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and item_cd = @ItemCd ";
                cnt = db.GetEntity<int>(
                    sql,
                    new
                    {
                        ItemCd = rec_fixed.ItemCd
                    });

                if (cnt > 0)
                {
                    // 製造指図書発行フラグ|0:発行する,1:しない
                    //sql = "";
                    //sql = sql + "select ";
                    //sql = sql + "    direction_publish_flg ";
                    //sql = sql + "from ";
                    //sql = sql + "    item ";
                    //sql = sql + "where ";
                    //sql = sql + "    1 = 1 ";
                    //sql = sql + "and item_cd = '" + rec_fixed.ItemCd + "' ";
                    //publishflg = db.GetEntity<int>(sql);
                    //publishflg = 0;

                }

                // 製造オーダーファイル作成
                // 製造指図ヘッダ登録
                // → 共通メソッドにて登録
                /*
                sql = "";
                sql = sql + "insert into direction_header ( ";
                sql = sql + "    direction_division ";
                sql = sql + "    ,direction_no ";
                sql = sql + "    ,direction_date ";
                //sql = sql + "    ,section_cd ";
                //sql = sql + "    ,work_center_cd ";
                sql = sql + "    ,direction_status ";
                sql = sql + "    ,item_cd ";
                sql = sql + "    ,item_name ";
                sql = sql + "    ,item_active_date ";
                sql = sql + "    ,specification_cd ";
                sql = sql + "    ,specification_name ";
                sql = sql + "    ,reference_no ";
                sql = sql + "    ,planed_qty ";
                sql = sql + "    ,planed_end_date ";
                //sql = sql + "    ,inspect_method ";
                //sql = sql + "    ,mortgage_detail_flg ";
                sql = sql + "    ,remark ";

                //sql = sql + "    ,last_in_day ";
                sql = sql + "    ,result_qty ";
                //sql = sql + "    ,inspreceipt_wait_quantity ";
                //sql = sql + "    ,location_division_cd ";
                //sql = sql + "    ,location_cd ";
                //sql = sql + "    ,incurred_material_cost1 ";
                //sql = sql + "    ,incurred_material_cost2 ";
                //sql = sql + "    ,incurred_outsourcing_cost1 ";
                //sql = sql + "    ,incurred_outsourcing_cost2 ";
                //sql = sql + "    ,incurred_labor_cost1 ";
                //sql = sql + "    ,incurred_labor_cost2 ";
                //sql = sql + "    ,incurred_expense ";
                //sql = sql + "    ,cost_move_flg ";
                sql = sql + "    ,planed_start_date ";
                //sql = sql + "    ,inspection_bad_qty ";
                //sql = sql + "    ,adv_pur_notice_decide_division ";
                //sql = sql + "    ,production_plan_no ";
                sql = sql + "    ,vender_cd ";
                sql = sql + "    ,input_date ";
                sql = sql + "    ,input_user_id ";
                sql = sql + "    ,update_date ";
                sql = sql + "    ,update_user_id ";
                sql = sql + "    ,stamp_flg";//direction_publish_flg  → stamp_flg
                //sql = sql + "    ,screen_id ";
                sql = sql + ") values ( ";
                sql = sql + "    , 1 ";                                      // 指図区分
                sql = sql + "    , '" + wkodrNo + "' ";                      // 製造オ－ダ番号
                sql = sql + "    ,  " + rec_fixed.OrderDate + " ";           // 指図日
                //sql = sql + "    , '" + rec_fixed.WcBumon + "' ";          // 部門コ－ド
                //sql = sql + "    , '" + rec_fixed.VenderCd + "' ";           // 作業区コ－ド
                sql = sql + "    ,0  ";                                      // ステ－タス
                sql = sql + "    , '" + rec_fixed.ItemCd + "' ";             // 品目コ－ド
                sql = sql + "    , '" + rec_fixed.ImHinnm + "' ";            // 品目名称
                sql = sql + "    , to_date('" + rec_fixed.ActiveDate.GetValueOrDefault().ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') ";            // 品目開始有効日
                sql = sql + "    , '" + rec_fixed.SpecificationCd + "' ";    // 仕様コ－ド
                sql = sql + "    , '" + rec_fixed.SpecificationName + "' ";  // 仕様名称
                sql = sql + "    , '" + rec_fixed.ReferenceNo + "' ";        // リファレンス
                sql = sql + "    ,  " + qty + " ";                           // 予定生産量
                sql = sql + "    , to_date('" + plan_dt.GetValueOrDefault().ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') ";            // 終了予定日時
                //sql = sql + "    , " + rec_fixed.ImTest + " ";             // 検査方法
                //sql = sql + "    , " + wkhkFlg + " ";                        // 引当明細ＦＬＧ
                sql = sql + "    , '' ";                                     // 適用

                //sql = sql + "    ,NULL  ";                                   // 最終入庫日
                sql = sql + "    ,0  ";                                      // 入庫累計数
                //sql = sql + "    ,0  ";                                      // 検査待ち数
                //sql = sql + "    , '" + rec_fixed.LocationDivisionCd + "' "; // 納入ロケ－ション区分
                //sql = sql + "    , '" + rec_fixed.LocationCd + "' ";         // 納入ロケ－ション
                //sql = sql + "    ,0 ";                                       // 発生原価（材料費１）
                //sql = sql + "    ,0 ";                                       // 発生原価（材料費２）
                //sql = sql + "    ,0 ";                                       // 発生原価（外注費１）
                //sql = sql + "    ,0 ";                                       // 発生原価（外注費２）
                //sql = sql + "    ,0 ";                                       // 発生原価（労務費１）
                //sql = sql + "    ,0 ";                                       // 発生原価（労務費２）
                //sql = sql + "    ,0 ";                                       // 発生原価（経費）
                //sql = sql + "    ,0 ";                                       // 原価移動ＦＬＧ
                sql = sql + "    , " + rec_fixed.OrderDate + " ";            // 開始日
                //sql = sql + "    ,0 ";                                       // 検収不良数
                //sql = sql + "    ,1 ";                                       // 内示・確定区分＝確定
                //sql = sql + "    , '" + rec_fixed.ProductionPlanNo + "' ";   // 生産計画No
                sql = sql + "    , '" + rec_fixed.VenderCd + "' ";           // 仕入先コード
                sql = sql + "    , Now() ";                                  // システム日付
                sql = sql + "    , '" + updatorCd + "' ";
                sql = sql + "    , Now() ";                                  // システム日付
                sql = sql + "    , '" + updatorCd + "' ";
                sql = sql + "    , " + publishflg + " ";                     // 製造指図書発行フラグ
                //sql = sql + "    , 189 ";                                    // 画面ID
                sql = sql + ") ";

                intRet = db.Regist(sql);
                */

                //外注加工品かつBOMがあれば製造指図作成
                //return 1;

                DateTime now = DateTime.Now;

                PlanPackage.ParamRegistDirection direction = new PlanPackage.ParamRegistDirection();
                direction.Common = new PlanPackage.CommonDirection();
                // 製造指図区分
                int directionDivision = 0;
                // 仕様マスタの品目タイプ（item_type）が2:B2100:仕掛品 x半製品の場合
                if (rec_fixed.ItemType.GetValueOrDefault() == MyConst.ItemType_2_B2100)
                {
                    directionDivision = Constants.DIRECTION.COMMON.DIRECTION_DIVISION.BATCH;    // 1:仕掛品
                }
                else
                {
                    directionDivision = Constants.DIRECTION.COMMON.DIRECTION_DIVISION.FILLING;  // 2:製品
                }
                direction.Common.DirectionDivision = directionDivision;                                     // 製造指図区分
                direction.Common.DirectionNo = directionNo;                                                 // 指図番号
                direction.Common.InputUserId = paramInfo.UserId;                                            // 登録者ID
                direction.Common.LanguageId = paramInfo.LanguageId;                                         // 言語ID
                direction.Common.ProductionStartDate = rec_fixed.OrderDate.GetValueOrDefault();             // 開始予定日時
                // 終了日時は 当日の23:59:00とする
                DateTime planDate = plan_dt.GetValueOrDefault();
                planDate = planDate.AddDays(1);     // 日付を追加
                planDate = planDate.Date;           // 時分秒を切捨て
                //planDate = planDate.AddSeconds(-1); // -1秒
                planDate = planDate.AddMinutes(-1); // -1分
                direction.Common.ProductionEndDate = planDate;                                              // 終了予定日時

                //BOM取得
                Dao.SearchBomCondition cndBom = new Dao.SearchBomCondition();
                cndBom.ItemCd = rec_fixed.ItemCd;                                                           // 品目コード
                cndBom.SpecificationCd = rec_fixed.SpecificationCd;                                         // 仕様コード
                // 計画発注日に変更
                //cndBom.ValidDate = rec_fixed.ActiveDate.GetValueOrDefault();                                // 開始有効日
                cndBom.ValidDate = rec_fixed.OrderDate.GetValueOrDefault();                                // 開始有効日 = 計画発注日

                sql = "";
                sql = sql + "select ";
                sql = sql + "    recipe_cd ";
                sql = sql + "    ,production_line ";
                sql = sql + "from ";
                sql = sql + "    recipe_header ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and item_cd = @ItemCd ";
                sql = sql + "and specification_cd = @SpecificationCd ";
                sql = sql + "and start_date <= @ValidDate ";
                sql = sql + "and end_date >= @ValidDate ";
                sql = sql + "and recipe_status <> 1 ";
                //and @ValidDate between start_date and end_date
                //sql = sql + "and start_date >= @ValidDate ";
                sql = sql + "and activate_flg = 1 ";
                sql = sql + "and del_flg = 0 ";
                sql = sql + "order by ";
                sql = sql + "    recipe_priority desc nulls last ";
                sql = sql + "limit 1 ";

                Dao.SearchBom resultBom = db.GetEntityByDataClass<Dao.SearchBom>(sql, new { ItemCd = cndBom.ItemCd, SpecificationCd = cndBom.SpecificationCd, ValidDate = cndBom.ValidDate });

                // 製造ヘッダのデータを設定
                direction.Header = new PlanPackage.HeaderDirection();
                direction.Header.OrderNo = null;                            //受注番号
                direction.Header.OrderRowNo = null;                         //受注行
                direction.Header.ProductionLine = resultBom.ProductionLine; //生産ライン(検索)
                direction.Header.RecipeCd = resultBom.RecipeCd;             //レシピコード(BOM,検索)
                direction.Header.ApprovalStatus = 20;                       //承認ステータス
                // 処方テーブルを検索して取得
                ComDao.RecipeHeaderEntity recipe = APComUtil.GetRecipeHeaderInfoByRecipeCd(resultBom.RecipeCd, now, db);
                direction.Header.RecipeId = recipe.RecipeId;
                direction.Header.RecipeVersion = recipe.RecipeVersion;
                direction.Header.PlanedQty = rec_fixed.PlanQty ?? 0;                                //計画数量
                direction.Header.Remark = null;
                direction.Header.Notes = null;
                direction.Header.DirectionStatus = Constants.DIRECTION.HEADER.DIRECTION_STATUS.FIX; //指図確定

                // ロケーションを取得
                //sql = "";
                //sql = sql + "select ";
                //sql = sql + "    default_location ";
                //sql = sql + "from ";
                //sql = sql + "    v_item ";
                //sql = sql + "where ";
                //sql = sql + "    1 = 1 ";
                //sql = sql + "and item_cd = @ItemCd ";
                //sql = sql + "and specification_cd = @SpecificationCd ";
                //sql = sql + "order by ";
                //sql = sql + "    item_cd ";
                //sql = sql + "    , specification_cd ";
                //sql = sql + "    , active_date desc ";
                //sql = sql + "limit 1 ";

                // デフォルトロケーションを取得
                //string defaultLocation = db.GetEntityByDataClass<string>(sql, cndBom);
                //string defaultLocation = db.GetEntityByDataClass<string>(sql, new { ItemCd = cndBom.ItemCd, SpecificationCd = cndBom.SpecificationCd } );

                string defaultLocation = rec_fixed.DefaultLocation;

                // 外注は無し
                direction.Header.VenderCd = rec_fixed.VenderCd;                         // 仕入先
                direction.Header.ReferenceNo = rec_fixed.ReferenceNo;                   // リファレンス番号
                direction.Header.ItemCd = rec_fixed.ItemCd;                             // 品目
                direction.Header.SpecificationCd = rec_fixed.SpecificationCd;           // 仕様

                direction.LocationCd = defaultLocation;                                 //デフォルトロケーション
                direction.LotNo = null;

                // 製造指図を作成
                PlanPackage.ResultInfo resultRegist = PlanPackage.RegistDirection(direction, db);
                if (resultRegist.IsError)
                {
                    // ログ出力:製造指図作成 エラー
                    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10065, ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                    logger.Error(logMessage);
                    logMessage = "【エラー】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]：" + resultRegist.Message;
                    logger.Error(logMessage);
                    // 製造指図エラーのログを出力
                    return false;
                }

                // 在庫更新（製造指図）
                ret = inventoryRegistManufactureOrder(db, direction, paramInfo);
                if (ret == false)
                {
                    // ログ出力:在庫更新処理に失敗しました。
                    logMessage = "【エラー】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]：" + ComST.GetPropertiesMessage(new string[] { ComID.MB10174 }, languageId: paramInfo.LanguageId);
                    logger.Error(logMessage);
                    return false;
                }

                /*
                // 製造シリアルNoトランザクション登録(確定オーダーのみ更新)
                if (naiKaku == 0)
                {
                    try
                    {
                        // 終了シリアルNOの編集
                        wkQty = qty;
                        // 小数点以下を繰り上げ
                        if (qty - Decimal.Truncate(qty) > 0)
                        {
                            wkQty = wkQty + 1;
                        }
                        wkEndSeirialNo = wkQty.ToString().PadLeft(10, '0');
                        sql = "";
                        sql = sql + "insert into serial_no ( ";
                        sql = sql + "   direction_no ";
                        sql = sql + "   ,item_cd ";
                        sql = sql + "   ,reference_no ";
                        sql = sql + "   ,housing_no ";
                        sql = sql + "   ,start_seirial_no ";
                        sql = sql + "   ,end_seirial_no ";
                        sql = sql + "   ,planed_qty ";
                        sql = sql + "   ,result_qty ";
                        sql = sql + "   ,inspection_qty ";
                        sql = sql + "   ,defect_qty ";
                        sql = sql + "   ,delivery_qty ";
                        sql = sql + "   ,status ";
                        sql = sql + "   ,input_date ";
                        sql = sql + "   ,inputor_cd ";
                        sql = sql + "   ,update_date ";
                        sql = sql + "   ,updator_cd ";
                        sql = sql + ") values ( ";
                        sql = sql + "   '" + wkodrNo + "' ";                         // 製造オ－ダ番号
                        sql = sql + "   , '" + rec_fixed.ItemCd + "' ";              // 品目コ－ド
                        sql = sql + "   , '" + rec_fixed.ReferenceNo + "' ";         // リファレンス
                        sql = sql + "   ,'0000000000' ";                             // 製造入庫番号
                        sql = sql + "   ,'0000000001' ";                             // 開始製造シリアルＮＯ
                        sql = sql + "   , '" + wkEndSeirialNo + "' ";                // 終了製造シリアルＮＯ
                        sql = sql + "   , " + qty + " ";                             // 製造指示数
                        sql = sql + "   ,0 ";                                        // 製造入庫数
                        sql = sql + "   ,0 ";                                        // 検収数
                        sql = sql + "   ,0 ";                                        // 不良数
                        sql = sql + "   ,0 ";                                        // 出庫数
                        sql = sql + "   ,0 ";                                        // ステ－タス
                        sql = sql + "    , Now() ";                                  // システム日付
                        sql = sql + "    , '" + updatorCd + "' ";
                        sql = sql + "    , Now() ";                                  // システム日付
                        sql = sql + "    , '" + updatorCd + "' ";
                        sql = sql + ") ";

                        intRet = db.Regist(sql);
                        if (intRet < 0)
                        {
                            // TODO:ログ出力

                            return false;

                        }

                    }
                    catch (Exception ex)
                    {
                        // TODO:ログ出力
                        // wErrNum := 1;
                        logger.Error(ex.Message);
                        //throw ex;
                        return false;
                    }
                }
                */

                slipDictionary.Add(directionNo, directionNo);
                return true;
            }
            catch (Exception ex)
            {
                // ログ出力:製造確定処理 エラー
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10066, ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                logMessage = "";
                logger.Error(ex.Message);
                logger.Error(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 発注確定処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="rec_fixed">処理データ</param>
        /// <param name="qty">計画数</param>
        /// <param name="deliveryDate">納期</param>
        /// <param name="marumeFlg">まるめフラグ</param>
        /// <param name="naiKaku">確定フラグ</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <param name="slipDictionary">購入依頼番号配列</param>
        /// <returns>(戻値)true:OKまたはfalse:NG</returns>
        public static bool PakFixedFixOrderSub(
            ComDB db,
            Dao.FixProductionOrder rec_fixed,
            decimal qty,
            DateTime? deliveryDate,
            bool marumeFlg,
            int naiKaku,
            PakDao.ComParamInfo paramInfo,
            ref Dictionary<string, string> slipDictionary)
        {
            bool ret = false;
            int intRet = 0;

            string sql = "";

            string bk_hcd = "";

            //PakDao.ComParamInfo pInfo = new PakDao.ComParamInfo();
            List<PakDao.TempPartsStructure> resultList = new List<PakDao.TempPartsStructure>();

            decimal? gettan = 0;
            //int tmp_flg = 0;
            decimal king = 0;
            //decimal szkin = 0;
            //decimal tskin = 0;
            //decimal? zritu = 0;
            //int shasu = 0;
            //int shasutn = 0;
            //int flg2 = 0;
            //string value1 = "";
            int w_Naikaku = 0;
            //int w_cnt = 0;
            //int w_publishflg = 0;
            DateTime order_due_date = DateTime.MinValue;

            // 購入依頼番号
            string purchase_request_no = "";
            purchase_request_no = APComUtil.GetNumber(db, Constants.SEQUENCE_PROC_NAME.PURCHASE_REQUEST);

            // 原価項目=2,3の場合、BOMチェックする
            //if (rec_fixed.ImGenka == 2 || rec_fixed.ImGenka == 3)
            //{
            // 確定情報の場合
            if (naiKaku == 0)
            {
                string date = null;
                if (rec_fixed.DeliveryDate != null)
                {
                    date = rec_fixed.DeliveryDate.GetValueOrDefault().ToString("YYYY/MM/DD");
                }
                // BOMチェックを外す
                //ret = PakFixedCheckBom(db, rec_fixed.PlanNo, rec_fixed.ImCode, date, pInfo);
                //if (ret == false)
                //{
                //    return false;
                //}
            }
            //}
            //

            // 構成子部品の引当ファイル作成(確定オーダーのみ)
            if (naiKaku == 0)
            {
                // 品目コードが変わったら１レベル展開を行う
                if (string.IsNullOrEmpty(bk_hcd) || bk_hcd != rec_fixed.ItemCd)
                {

                    //// １レベル部品展開
                    //ComPack.Exe1LevelPartsStructure(
                    //    db
                    //    , pInfo
                    //    , rec_fixed.ItemCd
                    //    , rec_fixed.SpecificationCd
                    //    , rec_fixed.DeliveryDate.GetValueOrDefault()
                    //    , DateTime.MinValue
                    //    , DateTime.MaxValue
                    //    , 1
                    //    , 0
                    //    , pInfo.UserId
                    //    , pInfo.PgmId
                    //    , true
                    //    , ref resultList
                    //);
                    //if (ret == false)
                    //{
                    //    return false;
                    //}
                } // 品目コードが変わったら１レベル展開を行う
                // 親部品情報セット
                Dao.TrnOwn workTrnOwn = new Dao.TrnOwn();
                workTrnOwn.ProcedureDivision = rec_fixed.ProcedureDivision;
                workTrnOwn.ItemCd = rec_fixed.ItemCd;
                workTrnOwn.SpecificationCd = rec_fixed.SpecificationCd;
                workTrnOwn.DeliverLimit = rec_fixed.Deliverlimit;
                workTrnOwn.OrderDate = rec_fixed.OrderDate;
                workTrnOwn.PlanQty = rec_fixed.PlanQty;
                workTrnOwn.ReferenceNo = rec_fixed.ReferenceNo;
                workTrnOwn.OrderNo = purchase_request_no;
                workTrnOwn.VenderCd = rec_fixed.VenderCd;
                workTrnOwn.MarumeFlg = marumeFlg;

                // 引当明細ファイル登録処理(購入依頼)
                //ret = PakFixedMakeInoutSource(db, wTrnOwn, resultList, pInfo, MyConst.InventoryRegistDivision.PurchaseRequest);
                //if (ret == false)
                //{
                //    return false;
                //}

            } // 構成子部品の引当ファイル作成(確定オーダーのみ)

            // 発注残数の更新(確定オーダーのみ更新)
            //if (naiKaku == 0)
            //{
            // TODO:if (rec_fixed.Imzaiko != 2)
            //{
            // UPDATE ITEM_INVENTORY // ITEM_INVENTORY テーブルは廃止
            //}
            //}

            // 決定単価取得
            PakCommonGetTanka(db, "SI", rec_fixed.VenderCd, rec_fixed.ItemCd, rec_fixed.SpecificationCd, DateTime.Now, qty, ref gettan, paramInfo);
            if (gettan == 0)
            {
                //tmp_flg = 1;
            }
            else
            {
                //tmp_flg = 0;
            }
            // 消費税計算
            king = qty * gettan.GetValueOrDefault();
            //ret = PakCommonCalcTaxAmt(db, "SI", rec_fixed.VenderCd, rec_fixed.ItemCd, ref king, ref szkin, ref tskin, ref zritu, ref shasu, ref shasutn, pInfo);
            //if (ret == true)
            //{
            //    king = PakCommonRounding(king, shasu, shasutn, pInfo);
            //}
            //else
            //{
            //    zritu = 0;
            //}

            // 拡張項目情報設定
            //flg2 = 0;
            //value1 = "";
            //if (rec_fixed.ImGenka == 2 || rec_fixed.ImGenka == 3)
            //{
            // flg2 = 1;
            // value1 = wkordNo;
            //}

            // 内示・確定区分編集
            if (naiKaku == 0)
            {
                w_Naikaku = 1;
            }
            else
            {
                w_Naikaku = 0;
            }
            //w_cnt = 0;
            //w_publishflg = 0;

            // ITEM.DIRECTION_PUBLISH_FLG 指図発行フラグは廃止

            // 納期から注文書表示納期[ORDER_DUE_DATE]を計算
            order_due_date = PakFixedCalcOrderDueDate(db, deliveryDate, 0);

            // 購入依頼件名作成
            string purchase_subject = "";
            List<string> errMessage = new List<string>();
            // 名称マスタの値 + 購入依頼番号
            purchase_subject = PakCommonGetNames(db, MyConst.NamesPSBJ, MyConst.One, paramInfo, ref errMessage) + " " + purchase_request_no;

            // TODO:購買外注オーダーファイルの作成
            // TODO:PURCHASE_SUBCONTRACT → purchase_request_head オーダー区分 0:通常

            // 購入依頼ヘッダ作成
            sql = "";
            sql = sql + "insert into purchase_request_head ( ";
            sql = sql + "   purchase_request_no ";
            sql = sql + "   ,purchase_status ";
            sql = sql + "   ,purchase_request_date ";
            sql = sql + "   ,origin_division ";
            sql = sql + "   ,purchase_subject ";
            sql = sql + "   ,purchase_request_department_cd ";
            sql = sql + "   ,purchase_request_charge_cd ";
            sql = sql + "   ,buy_subcontract_order_remark ";
            sql = sql + "   ,input_date ";
            sql = sql + "   ,input_user_id ";
            sql = sql + "   ,update_date ";
            sql = sql + "   ,update_user_id ";
            sql = sql + ") values ( ";
            sql = sql + "   @PurchaseRequestNo ";                                                        // 購入依頼番号
            sql = sql + "   , @PurchaseRequestApproval ";  // 購買ステータス：購入依頼承認済
            sql = sql + "   ,to_date( @OrderDate , 'YYYY/MM/DD') "; // 購入依頼日
            sql = sql + "   , @OriginDivisionPlan ";    // 発生元区分 1:計画
            sql = sql + "   , @PurchaseSubject ";                                       // 購入依頼件名
            sql = sql + "   , @TaBumon ";               // 管理部署コード
            sql = sql + "   , @TsTanto ";               // 購入依頼担当者ID
            sql = sql + "   , @Remark ";                // 購入依頼備考
            sql = sql + "    , Now() ";                                  // システム日付
            sql = sql + "    , @UserId ";
            sql = sql + "    , Now() ";                                  // システム日付
            sql = sql + "    , @UserId ";
            sql = sql + ") ";

            intRet = db.Regist(
                sql,
                new
                {
                    PurchaseRequestNo = purchase_request_no,
                    PurchaseRequestApproval = Constants.PURCHASE_STATUS.PURCHASE_REQUEST_APPROVAL,
                    OrderDate = rec_fixed.OrderDate.GetValueOrDefault().ToString("yyyy/MM/dd"),
                    OriginDivisionPlan = MyConst.OriginDivisionPlan,
                    PurchaseSubject = purchase_subject,
                    TaBumon = rec_fixed.TaBumon,
                    TsTanto = rec_fixed.TsTanto,
                    Remark = rec_fixed.Remark,
                    UserId = paramInfo.UserId
                });
            if (intRet < 0)
            {
                // ログ出力:発注確定処理 異常終了
                logMessage = "【エラー】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10067, ComID.MB10068, ComID.MB00022, "", "" }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                return false;

            }
            //発注区分取得
            int? orderDivision = getPlanDivison(db, rec_fixed.ItemCd, rec_fixed.SpecificationCd);
            string strOrderDivision = "";
            if (orderDivision == null)
            {
                strOrderDivision = "null";
            }
            else
            {
                strOrderDivision = orderDivision.ToString();
            }
            // 購入依頼明細作成
            sql = "";
            sql = sql + "insert into purchase_request_detail ( ";
            sql = sql + "   purchase_request_no ";
            sql = sql + "   ,purchase_request_row_no ";
            sql = sql + "   ,disp_order ";
            sql = sql + "   ,purchase_status ";
            sql = sql + "   ,order_division ";
            sql = sql + "   ,item_cd ";
            sql = sql + "   ,item_active_date ";
            sql = sql + "   ,item_name ";
            sql = sql + "   ,specification_cd ";
            sql = sql + "   ,specification_name ";
            sql = sql + "   ,request_vender_cd ";
            sql = sql + "   ,request_vender_name ";
            sql = sql + "   ,delivery_cd ";
            sql = sql + "   ,delivery_name ";
            sql = sql + "   ,purchase_request_quantity ";
            sql = sql + "   ,purchase_request_weight ";
            sql = sql + "   ,request_unitprice ";
            sql = sql + "   ,request_amount ";
            sql = sql + "   ,request_delivery_date ";
            sql = sql + "   ,detail_remark ";
            sql = sql + "   ,buy_subcontract_transaction_id ";
            sql = sql + "   ,buy_subcontract_approval_date ";
            sql = sql + "   ,input_date ";
            sql = sql + "   ,input_user_id ";
            sql = sql + "   ,update_date ";
            sql = sql + "   ,update_user_id ";
            sql = sql + ") values ( ";
            sql = sql + "   @PurchaseRequestNo ";                         // 購入依頼番号
            sql = sql + "    ,1 ";                                        // 明細行No
            sql = sql + "    ,1 ";                                        // 表示順
            sql = sql + "    , @PurchaseRequestApproval ";  // 購買ステータス：購入依頼承認済
            sql = sql + "    , @StrOrderDivision ";                 // 発注区分 1:原材料, 2:外注製品（直送）, 3:外注製品（非直送）, 4:スポット品, 5:仕入直送品, 6:仕入在庫品
            sql = sql + "    , @ItemCd ";              // 品目コ－ド
            sql = sql + "    , to_date( @ActiveDate , 'YYYY/MM/DD HH24:MI:SS') ";          // 品目有効開始日
            sql = sql + "    , @ImHinnm ";             // 品目名称
            sql = sql + "    , @SpecificationCd ";     // 仕様コ－ド
            sql = sql + "    , @SpecificationName ";   // 仕様名称
            sql = sql + "    , @VenderCd ";            // 仕入先コード
            sql = sql + "    , @VenderName ";          // 仕入先名称
            sql = sql + "    , @DefaultLocation ";     // 納入先コード（基準保管場所）
            sql = sql + "    , @DefaultLocationName "; // 納入先名称（基準保管場所）
            sql = sql + "   , @Qty ";                    // 購入依頼数量
            sql = sql + "   ,null ";                                      // 購入依頼重量
            sql = sql + "   , @Gettan ";                 // 依頼単価
            sql = sql + "   , @King ";                   // 依頼金額
            sql = sql + "   ,to_date( @DeliveryDate , 'YYYY/MM/DD')"; // 希望納期
            sql = sql + "   , @Remark ";               // 明細備考
            sql = sql + "   ,null ";                                      // 購入依頼承認トランザクションID
            sql = sql + "   ,null ";                                      // 購入依頼承認日
            sql = sql + "    , Now() ";                                   // 登録日時
            sql = sql + "    , @UserId ";              // 登録者ID
            sql = sql + "    , Now() ";                                   // 更新日時
            sql = sql + "    , @UserId ";              // 更新者ID
            sql = sql + ") ";

            intRet = db.Regist(
                sql,
                new
                {
                    PurchaseRequestNo = purchase_request_no,
                    PurchaseRequestApproval = Constants.PURCHASE_STATUS.PURCHASE_REQUEST_APPROVAL,
                    StrOrderDivision = int.Parse(strOrderDivision),
                    ItemCd = rec_fixed.ItemCd,
                    ActiveDate = rec_fixed.ActiveDate.ToString(),
                    ImHinnm = rec_fixed.ImHinnm,
                    SpecificationCd = rec_fixed.SpecificationCd,
                    SpecificationName = rec_fixed.SpecificationName,
                    VenderCd = rec_fixed.VenderCd,
                    VenderName = rec_fixed.VenderName,
                    DefaultLocation = rec_fixed.DefaultLocation,
                    DefaultLocationName = rec_fixed.DefaultLocationName,
                    Qty = qty,
                    Gettan = gettan,
                    King = king,
                    DeliveryDate = deliveryDate.GetValueOrDefault().ToString("yyyy/MM/dd"),
                    Remark = rec_fixed.Remark,
                    UserId = paramInfo.UserId
                });
            if (intRet < 0)
            {
                // ログ出力:発注確定処理 異常終了
                logMessage = "【エラー】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10067, ComID.MB10069, ComID.MB00022, "", "" }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                return false;
            }

            // 在庫更新（購入依頼）
            ret = inventoryRegistPurchaseRequest(db, purchase_request_no, rec_fixed, paramInfo);
            if (ret == false)
            {
                // ログ出力:在庫更新処理に失敗しました。
                logMessage = "【エラー】計画番号 [" + rec_fixed.PlanNo + "] 品目 [" + rec_fixed.ItemCd + "] 仕様 [" + rec_fixed.SpecificationCd + "]" + ComST.GetPropertiesMessage(new string[] { ComID.MB10174 }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                return false;
            }

            // 原価項目=2,3の場合　製造指図ヘッダーを更新する
            // TODO:if (rec_fixed.ImGenka == 2 || rec_fixed.ImGenka == 3)
            //{
            // 確定情報の場合
            if (w_Naikaku == 1)
            {
                // 製造指図ヘッダ登録
                /*
                sql = "";
                sql = sql + "insert into direction_header ( ";
                sql = sql + "    direction_division ";
                sql = sql + "    ,direction_no ";
                sql = sql + "    ,direction_date ";
                sql = sql + "    ,direction_status ";
                sql = sql + "    ,item_cd ";
                sql = sql + "    ,item_name ";
                sql = sql + "    ,item_active_date ";
                sql = sql + "    ,specification_cd ";
                sql = sql + "    ,specification_name ";
                sql = sql + "    ,reference_no ";
                sql = sql + "    ,planed_qty ";
                sql = sql + "    ,planed_end_date ";
                sql = sql + "    ,remark ";
                sql = sql + "    ,result_qty ";
                sql = sql + "    ,planed_start_date ";
                sql = sql + "    ,vender_cd ";
                sql = sql + "    ,input_date ";
                sql = sql + "    ,input_user_id ";
                sql = sql + "    ,update_date ";
                sql = sql + "    ,update_user_id ";
                sql = sql + ") values ( ";
                sql = sql + "    ,1 ";                                       // TODO:指図区分
                sql = sql + "    , '" + wkodrNo + "' ";                      // 指図番号
                sql = sql + "    ,  " + rec_fixed.OrderDate + " ";           // 指図日時
                sql = sql + "    ,0  ";                                      // ステ－タス
                sql = sql + "    , '" + rec_fixed.ItemCd + "' ";             // 品目コ－ド
                sql = sql + "    , '" + rec_fixed.ImHinnm + "' ";            // 品目名称
                sql = sql + "    , to_date('" + rec_fixed.ActiveDate.GetValueOrDefault().ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') "; // 品目開始有効日
                sql = sql + "    , '" + rec_fixed.SpecificationCd + "' ";    // 仕様コ－ド
                sql = sql + "    , '" + rec_fixed.SpecificationName + "' ";  // 仕様名称
                sql = sql + "    , '" + rec_fixed.ReferenceNo + "' ";        // リファレンス
                sql = sql + "    ,  " + qty.ToString() + " ";                           // 予定生産量
                sql = sql + "    , to_date('" + deliveryDate.GetValueOrDefault().ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') "; // 終了予定日時
                sql = sql + "    , '" + rec_fixed.Remark + "' ";             // 適用
                sql = sql + "    ,0  ";                                      // 入庫累計数
                sql = sql + "    , " + rec_fixed.OrderDate + " ";            // 開始日
                sql = sql + "    , '" + rec_fixed.VenderCd + "' ";           // 仕入先コード
                sql = sql + "    , Now() ";                                  // システム日付
                sql = sql + "    , '" + updatorCd + "' ";
                sql = sql + "    , Now() ";                                  // システム日付
                sql = sql + "    , '" + updatorCd + "' ";
                sql = sql + "    , " + w_publishflg.ToString() + " ";                     // 製造指図書発行フラグ
                sql = sql + ") ";

                intRet = db.Regist(sql);
                if (intRet < 0)
                {
                    // TODO:ログ出力

                    return false;

                }
                */

            }
            //}
            slipDictionary.Add(purchase_request_no, purchase_request_no);
            return true;
        }

        /// <summary>
        /// 確定取込デ－タ作成メイン処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="mode">モード</param>
        /// <param name="condition">条件</param>
        /// <param name="date1">日付1</param>
        /// <param name="date2">日付2</param>
        /// <param name="date3">日付3</param>
        /// <param name="deliverLimit">納期</param>
        /// <param name="flag">フラグ</param>
        /// <param name="marume">まるめ</param>
        /// <param name="naiKaku">内示確定</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <returns>(戻値)true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool PakFixedFixFetchOrderMain(
            ComDB db,
            int mode,
            string condition,
            DateTime date1,
            DateTime date2,
            DateTime date3,
            DateTime deliverLimit,
            int flag,
            int marume,
            int naiKaku,
            PakDao.ComParamInfo paramInfo)
        {
            bool ret = false;
            int intRet = 0;
            string sql = "";
            //string wkodrNo = "";
            //DateTime wkPDATE = DateTime.MinValue;

            int sub_cost = 0;
            int cost_mang = 0;
            int cnt = 0;

            // 自社マスタ検索
            //sql = "";
            //sql = sql + "select ";
            //sql = sql + "    manufacturing_subcontract_cost ";
            //sql = sql + "    ,cost_update_management ";
            //sql = sql + "from ";
            //sql = sql + "    company ";
            //ComDao.CompanyEntity company = db.GetEntity<ComDao.CompanyEntity>(sql);

            // sub_cost = company  manufacturing_subcontract_costがない
            // cost_mang = company cost_update_managementがない

            // SQL生成
            sql = "";
            sql = sql + "select ";
            sql = sql + "     pp.item_cd ";                 // 品目コード
            sql = sql + "    ,pp.specification_cd ";        // 仕様コード
            sql = sql + "    ,pp.delivery_date ";           // 納期
            sql = sql + "    ,pp.plan_no ";                 // 計画番号
            sql = sql + "    ,pp.procedure_division ";      // 手続区分
            sql = sql + "    ,pp.order_publish_division ";  // オーダー発行区分
            sql = sql + "    ,pp.order_date ";              // 購入依頼日
            sql = sql + "    ,pp.order_rule ";              // 発注基準
            sql = sql + "    ,pp.deliverlimit ";            // 計画納期
            sql = sql + "    ,pp.plan_qty ";                // 計画数量
            sql = sql + "    ,pp.production_plan_no ";      // 生産計画番号
            sql = sql + "    ,pp.reference_no ";            // リファレンス番号
            //sql = sql + "    ,pp.location_division_cd ";
            sql = sql + "    ,pp.location_cd ";             // 納入ロケーションコード
            sql = sql + "    ,pp.vender_cd ";               // オーダー先コード
            sql = sql + "    ,pp.remark ";                  // 備考
            sql = sql + "    ,im.item_cd         imcode ";  // 品目コード
            sql = sql + "    ,im.item_name       imhinnm "; // 品目名称
            sql = sql + "    ,null               imaccd ";  // 科目コード
            sql = sql + "    ,null               imhojo ";  // 補助科目コード
            sql = sql + "    ,null               imeflg4 "; // 生産中止区分|0:生産,1～9:中止(主に"3"で中止)
            sql = sql + "    ,null               imgenka "; // 原価区分|0～6
            sql = sql + "    ,null               imtest ";  // 基準検査方法|0:無検査,1:抜取検査,2:全品検査(未使用)
            sql = sql + "    ,null               imzaiko "; // 在庫管理区分|0:通常,1:在庫表除外,2:更新除外

            sql = sql + "    ,im.active_date ";             // 有効日付
            sql = sql + "    ,sp.specification_name ";      // 仕様名称

            sql = sql + "    ,iz.item_cd         izcode ";  // 品目コード
            sql = sql + "    , @UserId plan_tanto_cd  "; // 計画担当者コード
            if (mode == 0)
            {
                sql = sql + "    ,null wccode ";  // 作業区コード
                sql = sql + "    ,null wcbumon "; // 部門コード
                sql = sql + "    ,null tscode ";
                sql = sql + "    ,null tsnyobi1 ";
                sql = sql + "    ,null tsnyobi2 ";
                sql = sql + "    ,null tstanto ";
                sql = sql + "    ,null tacode ";
                sql = sql + "    ,null tabumon ";
            }
            else
            {
                sql = sql + "    ,null wccode ";
                sql = sql + "    ,null wcbumon ";
                sql = sql + "    ,ts.vender_cd    tscode ";
                sql = sql + "    ,null tsnyobi1 ";   // 納入曜日１(未使用)|0:日 1:月 2:火 3:水 4:木 5:金 6:土
                sql = sql + "    ,null tsnyobi2 ";   // 納入曜日２(未使用)|0:日 1:月 2:火 3:水 4:木 5:金 6:土
//                sql = sql + "    ,ts.tanto_cd     ts_tanto ";     // 担当者コード
                sql = sql + "    ,ts.user_id     ts_tanto ";        // 担当者コード
                sql = sql + "    ,ts.vender_name1 vender_name ";    // 取引先名称
//                sql = sql + "    ,ta.tanto_cd     ta_code ";      // 担当者コード
                sql = sql + "    ,ta.user_id     ta_code ";         // 担当者コード
                sql = sql + "    ,ta.organization_cd   tabumon ";   // 部署コード
            }
            sql = sql + "from ";
            sql = sql + "    mrp_result pp "; // purchase_plan → mrp_resultに 変更
            sql = sql + "    left outer join ";
            //                sql = sql + "        item im ";
            sql = sql + "        v_item_active im ";
            sql = sql + "    on pp.item_cd = im.item_cd ";
            sql = sql + "    left outer join ";
            sql = sql + "        v_item_specification_active sp ";
            sql = sql + "    on pp.item_cd = sp.item_cd and pp.specification_cd = sp.specification_cd ";
            sql = sql + "    left outer join ";
            sql = sql + "        v_item_inventory iz "; // ビューに置き換え
            sql = sql + "    on pp.item_cd = iz.item_cd ";

            if (mode == 0)
            {
                // work_center は廃止
                //sql = sql + "    left outer join ";
                //sql = sql + "        work_center wc ";
                //sql = sql + "    on pp.vender_cd = wc.work_center_cd ";
            }
            else
            {
                sql = sql + "    left outer join ";
                sql = sql + "        vender ts ";                   // 取引先マスタ
                sql = sql + "    on pp.vender_cd = ts.vender_cd and ts.vender_division = 'SI' ";
                sql = sql + "    left outer join ";
                sql = sql + "        belong  ta ";                  // 所属マスタ
                sql = sql + "    on ts.user_id = ta.user_id ";
            }
            sql = sql + "where ";
            //                sql = sql + "     pp.item_cd = pp.item_cd ";
            sql = sql + "     1 = 1 ";

            if (marume == 0)
            {
                if (date1 != null)
                {
                    sql = sql + "and to_char(pp.delivery_date, 'yyyymmdd') >= @Date1 ";
                }
                if (date2 != null)
                {
                    sql = sql + "and to_char(pp.delivery_date, 'yyyymmdd') <= @Date2 ";
                }
                if (mode == 0)
                {
                    // 製造
                    sql = sql + "and pp.procedure_division = 1 ";
                    sql = sql + " and sp.plan_division = 1 ";       // 品目仕様.計画区分 = 1:製造
                }
                else if (mode == 1)
                {
                    // 購買
                    sql = sql + "and pp.procedure_division = 2 ";
                    //sql = sql + " and im.cost_division in (4,5,6) ";
                    sql = sql + " and sp.plan_division = 2 ";       // 品目仕様.計画区分 = 2:購買
                }
                else
                {
                    // 外注
                    sql = sql + "and pp.procedure_division = 2 ";
                    //sql = sql + " and im.cost_division in (2,3) ";
                    sql = sql + " and sp.plan_division = 3 ";       // 品目仕様.計画区分 = 3:外注
                }
            }
            else
            {
                if (mode == 0)
                {
                    // 外注
                    if (date2 != null)
                    {
                        sql = sql + "and to_char(pp.delivery_date, 'yyyymmdd') >= @Date2 ";
                    }
                    if (date3 != null)
                    {
                        sql = sql + "and to_char(pp.delivery_date, 'yyyymmdd') <= @Date3 ";
                    }
                    sql = sql + "and pp.procedure_division = 1 ";
                    sql = sql + " and sp.plan_division = 1 ";       // 品目仕様.計画区分 = 1:製造
                }
                else if (mode == 1)
                {
                    if (date2 != null)
                    {
                        sql = sql + "and to_char(pp.delivery_date, 'yyyymmdd') >= @Date2 ";
                    }
                    if (date3 != null)
                    {
                        sql = sql + "and to_char(pp.delivery_date, 'yyyymmdd') <= @Date3 ";
                    }
                    sql = sql + "and pp.procedure_division = 2 ";
                    //sql = sql + " and im.cost_division in (4,5,6) ";
                    sql = sql + " and sp.plan_division = 2 ";       // 品目仕様.計画区分 = 2:購買
                }
                else
                {
                    if (date1 != null)
                    {
                        sql = sql + "and to_char(pp.delivery_date, 'yyyymmdd') >= @Date1 ";
                    }
                    if (date2 != null)
                    {
                        sql = sql + "and to_char(pp.delivery_date, 'yyyymmdd') <= @Date2 ";
                    }
                    sql = sql + "and pp.procedure_division = 2 ";
                    //sql = sql + " and im.cost_division in (2,3) ";
                    sql = sql + " and sp.plan_division = 3 ";       // 品目仕様.計画区分 = 3:外注
                }
                sql = sql + "and pp.marume_division = 0 ";
            }
            sql = sql + "and pp.status = 0 ";
            //sql = sql + "and pp.order_rule = 4 ";                   // 発注基準 4:確定引取←シャチハタ固有みたいなのでコメント
            //if (mode == 1)
            //{
            //    sql = sql + "and ts.vender_division = 'SI' ";
            //}
            sql = sql + "order by ";
            sql = sql + "    pp.vender_cd ";
            sql = sql + "    , pp.item_cd ";
            sql = sql + "    , pp.delivery_date ";

            IList<Dao.FixProductionOrder> cur_fixed = db.GetListByDataClass<Dao.FixProductionOrder>(
                sql,
                new
                {
                    UserId = paramInfo.UserId,
                    Date1 = date1.ToString("yyyyMMdd"),
                    Date2 = date2.ToString("yyyyMMdd"),
                    Date3 = date3.ToString("yyyyMMdd")
                });
            foreach (var rec_fixed in cur_fixed)
            {
                // 発注数<=０のオーダーは確定処理を行わない
                if (rec_fixed.PlanQty <= 0)
                {
                    continue;
                }
                // マスタのチェック
                ret = PakFixedCheckMaster(
                    mode,
                    0,
                    rec_fixed,
                    paramInfo,
                    "",
                    db);
                if (ret == false)
                {
                    continue;
                }
                // BOMのチェック
                //ret = PakFixedCheckBom(
                //    db,
                //    rec_fixed.PlanNo,
                //    rec_fixed.ImCode,
                //    rec_fixed.DeliveryDate.GetValueOrDefault().ToString("yyyy/MM/dd"),
                //    paramInfo);
                //if (ret == false)
                //{
                //    continue;
                //}
                //if (mode == 0)
                //{
                //    // 製造オーダー番号
                //    wkodrNo = APComUtil.getNumber(db, Constants.SEQUENCE_PROC_NAME.DIRECTION_NO);
                //}
                //else
                //{
                //    // 発注オーダー番号
                //    wkodrNo = APComUtil.getNumber(db, Constants.SEQUENCE_PROC_NAME.PURCHASE_REQUEST);
                //}
                //if (marume == 0)
                //{
                //    wkPDATE = rec_fixed.DeliveryDate.GetValueOrDefault();
                //}
                //else
                //{
                //    wkPDATE = deliverLimit;
                //}
                // オーダーが生産計画そのものである時、生産計画ファイル更新
                /*
                if (rec_fixed.ProductionPlanNo != "0")
                {
                    // 発注予測と生産計画とで分岐
                    if (rec_fixed.OrderPublishDivision == "13")
                    {
                        // 対象の発注予測データを[1:確定済]に更新
                        sql = "";
                        sql = sql + "update ";
                        sql = sql + "    purchase_forecast ";
                        sql = sql + "set ";
                        sql = sql + "    status = 1 ";
                        sql = sql + "    ,screen_id = " + numScreenId.ToString() + " ";
                        sql = sql + "    ,update_date = Now() ";
                        sql = sql + "    ,updator_cd = '" + updatorCd + "' ";
                        sql = sql + "where ";
                        sql = sql + "    1 = 1 ";
                        sql = sql + "and purchase_forecast_no = '" + rec_fixed.ProductionPlanNo + "' ";

                    }
                    else
                    {
                        // 生産計画
                        sql = "";
                        sql = sql + "update ";
                        sql = sql + "    production_plan ";
                        sql = sql + "set ";
                        sql = sql + "    status = 1 ";
                        sql = sql + "    ,screen_id = " + numScreenId.ToString() + " ";
                        sql = sql + "    ,update_date = Now() ";
                        sql = sql + "    ,updator_cd = '" + updatorCd + "' ";
                        sql = sql + "where ";
                        sql = sql + "    1 = 1 ";
                        sql = sql + "    production_plan_no = '" + rec_fixed.ProductionPlanNo + "' ";
                    }
                    intRet = db.Regist(sql);

                }// if (rec_fixed.ProductionPlanNo != "0")
                */
                Dictionary<string, string> list = new Dictionary<string, string>();

                // オーダーの作成
                if (mode == 0)
                {
                    // 製造
                    // 製造確定処理
                    ret = PakFixedFixProductionSub(
                        db,
                        rec_fixed,
                        false,
                        sub_cost,
                        cost_mang,
                        0,
                        rec_fixed.PlanQty.GetValueOrDefault(),
                        deliverLimit,
                        paramInfo,
                        ref list);
                    if (ret == false)
                    {
                        return false;
                    }
                }
                else
                {
                    // 購買、外注
                    // 発注確定取込処理
                    ret = PakFixedFixFetchOrderSub(
                        db,
                        rec_fixed,
                        rec_fixed.PlanQty.GetValueOrDefault(),
                        false,
                        0,
                        deliverLimit,
                        paramInfo);
                    if (ret == false)
                    {
                        return false;
                    }
                }
                // 確定ファイル作成(2:製造, 3:購買外注)
                //ret = MAKE_FIXED_PLAN() 廃止
                //if (ret == false)
                //{
                //    return false;
                //}

                // MRP結果（発注計画ファイル）更新 ステータス=1:確定
                // purchase_plan → mrp_result にテーブル名変更
                sql = "";
                sql = sql + "update ";
                sql = sql + "    mrp_result ";
                sql = sql + "set ";
                sql = sql + "    status = 1 ";
                //sql = sql + "    ,screen_id = " + numScreenId.ToString() + " ";
                sql = sql + "    ,update_date = Now() ";
                sql = sql + "    ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and plan_no = @PlanNo ";

                intRet = db.Regist(
                    sql,
                    new
                    {
                        UserId = paramInfo.UserId,
                        PlanNo = rec_fixed.PlanNo
                    });

                cnt = cnt + 1;

            } // foreach(var rec_fixed in cur_fixed)

            return true;
        }

        /// <summary>
        /// 確定取込デ－タ作成メイン処理 (まるめ処理をするオーダーを対象)
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="mode">モード</param>
        /// <param name="condition">条件</param>
        /// <param name="date1">日付1</param>
        /// <param name="date2">日付2</param>
        /// <param name="date3">日付3</param>
        /// <param name="deliverLimit">納期</param>
        /// <param name="flag">フラグ</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <returns>(戻値)true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool PakFixedFixFetchOrderMainMarume(
            ComDB db,
            int mode,
            string condition,
            DateTime date1,
            DateTime date2,
            DateTime date3,
            DateTime deliverLimit,
            int flag,
            PakDao.ComParamInfo paramInfo)
        {
            bool ret = false;

            string sql = "";
            //string wkodrNo = ""; // オーダー番号（グローバル変数の代わりに追加）
            //DateTime wkPDATE = DateTime.MinValue; // オーダー番号（グローバル変数の代わりに追加）

            int cnt = 0;
            Dao.FixProductionOrder bk_Point = new Dao.FixProductionOrder();

            string ts_wcd = "";
            string itm_cd = "";
            decimal? gndsu = 0;
            //int cost_mang = 0;

            // SQL生成
            sql = "";
            sql = sql + "select ";
            sql = sql + "     pp.item_cd ";
            sql = sql + "    ,pp.specification_cd "; // 仕様コード
            sql = sql + "    ,pp.delivery_date ";
            sql = sql + "    ,pp.plan_no ";
            sql = sql + "    ,pp.procedure_division ";
            sql = sql + "    ,pp.order_publish_division ";
            sql = sql + "    ,pp.order_date ";
            sql = sql + "    ,pp.order_rule ";
            sql = sql + "    ,pp.deliverlimit ";
            sql = sql + "    ,pp.plan_qty ";
            sql = sql + "    ,pp.production_plan_no ";
            sql = sql + "    ,pp.reference_no ";
            //sql = sql + "    ,pp.location_division_cd ";
            sql = sql + "    ,pp.location_cd ";
            sql = sql + "    ,pp.vender_cd ";
            sql = sql + "    ,im.item_cd         imcode ";
            sql = sql + "    ,im.item_name       imhinnm ";
            sql = sql + "    ,null               imaccd ";  // 科目コード
            sql = sql + "    ,null               imhojo ";  // 補助科目コード
            sql = sql + "    ,null               imeflg4 "; // 生産中止区分|0:生産,1～9:中止(主に"3"で中止)
            sql = sql + "    ,null               imgenka "; // 原価区分|0～6
            sql = sql + "    ,null               imtest ";  // 基準検査方法|0:無検査,1:抜取検査,2:全品検査(未使用)
            sql = sql + "    ,null               imzaiko "; // 在庫管理区分|0:通常,1:在庫表除外,2:更新除外

            sql = sql + "    ,im.active_date ";
            sql = sql + "    ,sp.specification_name ";

            sql = sql + "    ,iz.item_cd         izcode ";
            sql = sql + "    , @UserId plan_tanto_cd  ";          // 計画担当者コード
            if (mode == 0)
            {
                sql = sql + "    ,null wccode ";  // 作業区コード
                sql = sql + "    ,null wcbumon "; // 部門コード
                sql = sql + "    ,null tscode ";
                sql = sql + "    ,null tsnyobi1 ";
                sql = sql + "    ,null tsnyobi2 ";
                sql = sql + "    ,null tstanto ";
                sql = sql + "    ,null tacode ";
                sql = sql + "    ,null tabumon ";
            }
            else
            {
                sql = sql + "    ,null wccode ";
                sql = sql + "    ,null wcbumon ";
                sql = sql + "    ,ts.vender_cd    tscode ";
                sql = sql + "    ,null tsnyobi1 ";   // 納入曜日１(未使用)|0:日 1:月 2:火 3:水 4:木 5:金 6:土
                sql = sql + "    ,null tsnyobi2 ";   // 納入曜日２(未使用)|0:日 1:月 2:火 3:水 4:木 5:金 6:土
                sql = sql + "    ,ts.tanto_cd     ts_tanto ";
                sql = sql + "    ,ts.vender_name1 vender_name ";
                sql = sql + "    ,ta.tanto_cd     ta_code ";
                sql = sql + "    ,ta.organization_cd   tabumon "; // 部署コード
            }
            sql = sql + "from ";
            sql = sql + "    mrp_result pp "; // purchase_plan → mrp_resultに 変更
            sql = sql + "    left outer join ";
            sql = sql + "        item im ";
            sql = sql + "    on pp.item_cd = im.item_cd ";
            sql = sql + "    left outer join ";
            sql = sql + "        v_item_specification_active sp ";
            sql = sql + "    on pp.item_cd = sp.item_cd and pp.specification_cd = sp.specification_cd ";
            sql = sql + "    left outer join ";
            sql = sql + "        v_item_inventory iz "; // ビューに置き換え
            sql = sql + "    on pp.item_cd = iz.item_cd ";

            if (mode == 0)
            {
                // work_center は廃止
                //sql = sql + "    left outer join ";
                //sql = sql + "        work_center wc ";
                //sql = sql + "    on pp.vender_cd = wc.work_center_cd ";
            }
            else
            {
                sql = sql + "    left outer join ";
                sql = sql + "        vender ts ";
                sql = sql + "    on pp.vender_cd = ts.vender_cd and ts.vender_division = 'SI'";
                sql = sql + "    left outer join ";
                sql = sql + "        belong  ta ";
                sql = sql + "    on ts.tanto_cd = ta.tanto_cd ";
            }
            sql = sql + "where ";
            sql = sql + "    pp.item_cd = pp.item_cd ";
            if (date2 != null)
            {
                sql = sql + "and to_char(pp.delivery_date, 'yyyymmdd') >= @Date2 ";
            }
            if (date3 != null)
            {
                sql = sql + "and to_char(pp.delivery_date, 'yyyymmdd') <= @Date3 ";
            }
            if (mode == 0)
            {
                sql = sql + "and pp.procedure_division = 1 ";
                sql = sql + " and sp.plan_division = 1 ";       // 品目仕様.計画区分 = 1:製造
            }
            else if (mode == 1)
            {
                sql = sql + "and pp.procedure_division = 2 ";
                //sql = sql + " and im.cost_division in (4,5,6) ";
                sql = sql + " and sp.plan_division = 2 ";       // 品目仕様.計画区分 = 2:購買
            }
            else
            {
                sql = sql + "and pp.procedure_division = 2 ";
                //sql = sql + " and im.cost_division in (2,3) ";
                sql = sql + " and sp.plan_division = 3 ";       // 品目仕様.計画区分 = 3:外注
            }
            sql = sql + "and pp.status = 0 ";
            //sql = sql + "and pp.order_rule = 4 "; // order_rule = 4は廃止
            //if (mode == 1)
            //{
            //    sql = sql + "and ts.vender_division = 'SI' ";
            //}
            sql = sql + "order by ";
            sql = sql + "    pp.vender_cd ";
            sql = sql + "    , pp.item_cd ";
            sql = sql + "    , pp.delivery_date ";

            ts_wcd = "0";

            IList<Dao.FixProductionOrder> cur_fixed = db.GetListByDataClass<Dao.FixProductionOrder>(
                sql,
                new
                {
                    UserId = paramInfo.UserId,
                    Date2 = date2.ToString("yyyyMMdd"),
                    Date3 = date3.ToString("yyyyMMdd"),
                });
            foreach (var rec_fixed in cur_fixed)
            {
                // 発注数<=０のオーダーは確定処理を行わない
                if (rec_fixed.PlanQty <= 0)
                {
                    continue;
                }
                // (仕入先,品目) が変わったらオーダーを作成
                if (rec_fixed.VenderCd != ts_wcd || rec_fixed.ItemCd != itm_cd)
                {
                    if (ts_wcd != "0" || !string.IsNullOrEmpty(itm_cd))
                    {
                        //wkPDATE = deliverLimit;

                        // マスタのチェック
                        ret = PakFixedCheckMaster(
                            mode,
                            0,
                            bk_Point,
                            paramInfo,
                            "",
                            db);
                        if (ret == true)
                        {
                            // BOMのチェック
                            //ret = PakFixedCheckBom(
                            //    db,
                            //    rec_fixed.PlanNo,
                            //    rec_fixed.ImCode,
                            //    rec_fixed.DeliveryDate.GetValueOrDefault().ToString("yyyy/MM/dd"),
                            //    paramInfo);
                            //if (ret == true)
                            //{
                            //    if (mode == 0)
                            //    {
                            //        // 製造確定処理
                            //        ret = PakFixedFixProductionSub(
                            //            db,
                            //            bk_Point,
                            //            false,
                            //            0, //bk_Point.ImGenka
                            //            cost_mang,
                            //            0,
                            //            gndsu.GetValueOrDefault(),
                            //            deliverLimit,
                            //            paramInfo);
                            //        if (ret == false)
                            //        {
                            //            return false;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        // 発注確定取込処理
                            //        ret = PakFixedFixFetchOrderSub(
                            //            db,
                            //            bk_Point,
                            //            gndsu.GetValueOrDefault(),
                            //            false,
                            //            0,
                            //            deliverLimit,
                            //            paramInfo);
                            //        if (ret == false)
                            //        {
                            //            return false;
                            //        }
                            //    }
                            //    cnt = cnt + 1;
                            //    // TODO:commit;

                            //}
                        }
                        else
                        {
                            // TODO:rollback;
                        }
                    } // if (ts_wcd != "0" || !string.IsNullOrEmpty(itm_cd))
                    bk_Point.ItemCd = bk_Point.ItemCd;
                    bk_Point.SpecificationCd = bk_Point.SpecificationCd;
                    bk_Point.DeliveryDate = rec_fixed.DeliveryDate;
                    bk_Point.PlanNo = rec_fixed.PlanNo;
                    bk_Point.ProcedureDivision = rec_fixed.ProcedureDivision;
                    bk_Point.OrderPublishDivision = rec_fixed.OrderPublishDivision;
                    bk_Point.OrderDate = rec_fixed.OrderDate;
                    bk_Point.OrderRule = rec_fixed.OrderRule;
                    bk_Point.Deliverlimit = rec_fixed.Deliverlimit;
                    bk_Point.PlanQty = rec_fixed.PlanQty;
                    bk_Point.ProductionPlanNo = rec_fixed.ProductionPlanNo;
                    bk_Point.ReferenceNo = rec_fixed.ReferenceNo;
                    bk_Point.LocationDivisionCd = rec_fixed.LocationDivisionCd;
                    bk_Point.LocationCd = rec_fixed.LocationCd;
                    bk_Point.VenderCd = rec_fixed.VenderCd;
                    bk_Point.ImCode = rec_fixed.ImCode;
                    bk_Point.ImHinnm = rec_fixed.ImHinnm;
                    //bk_Point.IMACCD = rec_fixed.IMACCD;
                    //bk_Point.IMHOJO = rec_fixed.IMHOJO;
                    //bk_Point.IMEFLG4 = rec_fixed.IMEFLG4;
                    //bk_Point.IMGENKA = NVL(rec_fixed.IMGENKA, 0);
                    //bk_Point.IMTEST = rec_fixed.IMTEST;
                    //bk_Point.IMZAIKO = rec_fixed.IMZAIKO;
                    bk_Point.IzCode = rec_fixed.IzCode;
                    //bk_Point.WCCODE = rec_fixed.WCCODE;
                    //bk_Point.WCBUMON = rec_fixed.WCBUMON;
                    bk_Point.TsCode = rec_fixed.TsCode;
                    //bk_Point.TSNYOBI1 = rec_fixed.TSNYOBI1;
                    //bk_Point.TSNYOBI2 = rec_fixed.TSNYOBI2;
                    bk_Point.TsTanto = rec_fixed.TsTanto;
                    bk_Point.TaCode = rec_fixed.TaCode;
                    bk_Point.TaBumon = rec_fixed.TaBumon;
                    bk_Point.ActiveDate = rec_fixed.ActiveDate;
                    bk_Point.SpecificationName = rec_fixed.SpecificationName;
                    bk_Point.Remark = rec_fixed.Remark;
                    bk_Point.VenderName = rec_fixed.VenderName;

                    ts_wcd = rec_fixed.VenderCd;
                    itm_cd = rec_fixed.ItemCd;
                    gndsu = rec_fixed.PlanQty;
                    //if (mode == 0)
                    //{
                    //    // 製造オーダー番号
                    //    wkodrNo = APComUtil.getNumber(db, Constants.SEQUENCE_PROC_NAME.DIRECTION_NO);
                    //}
                    //else
                    //{
                    //    // 発注オーダー番号
                    //    wkodrNo = APComUtil.getNumber(db, Constants.SEQUENCE_PROC_NAME.PURCHASE_REQUEST);
                    //}
                } // if (rec_fixed.VenderCd != ts_wcd || rec_fixed.ItemCd != itm_cd)
                else
                {
                    gndsu = gndsu + rec_fixed.PlanQty;
                }

                // 確定ファイル作成(2:製造, 3:購買外注)
                //ret = MAKE_FIXED_PLAN() 不要となる予定
                //if (ret == false)
                //{
                //    return false;
                //}

                // MRP結果（発注計画ファイル）更新 ステータス=1:確定
                // purchase_plan → mrp_result にテーブル名変更
                sql = "";
                sql = sql + "update ";
                sql = sql + "    mrp_result ";
                sql = sql + "set ";
                sql = sql + "    status = 1 ";
                sql = sql + "    ,update_date = Now() ";
                sql = sql + "    ,update_user_id = @UserId ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "    plan_no = @PlanNo ";

                int intRet = db.Regist(
                    sql,
                    new
                    {
                        UserId = paramInfo.UserId,
                        PlanNo = rec_fixed.PlanNo
                    });

                cnt = cnt + 1;

            } // foreach(var rec_fixed in cur_fixed)

            if (!string.IsNullOrEmpty(ts_wcd) || !string.IsNullOrEmpty(itm_cd))
            {
                //wkPDATE = deliverLimit;
                // マスタのチェック
                ret = PakFixedCheckMaster(
                    mode,
                    0,
                    bk_Point,
                    paramInfo,
                    "",
                    db);
                if (ret == true)
                {
                    // BOMのチェック
                    //ret = PakFixedCheckBom(
                    //    db,
                    //    bk_Point.PlanNo,
                    //    bk_Point.ImCode,
                    //    bk_Point.DeliveryDate.GetValueOrDefault().ToString("yyyy/MM/dd"),
                    //    paramInfo);
                    //if (ret == true)
                    //{
                    //    // 最後のオーダーを作成する
                    //    if (mode == 0)
                    //    {
                    //        // 製造確定処理
                    //        ret = PakFixedFixProductionSub(
                    //            db,
                    //            bk_Point,
                    //            false,
                    //            0, //bk_Point.ImGenka
                    //            cost_mang,
                    //            0,
                    //            gndsu.GetValueOrDefault(),
                    //            deliverLimit,
                    //            paramInfo);
                    //        if (ret == false)
                    //        {
                    //            return false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        // 発注確定取込処理
                    //        ret = PakFixedFixFetchOrderSub(
                    //            db,
                    //            bk_Point,
                    //            gndsu.GetValueOrDefault(),
                    //            false,
                    //            0,
                    //            deliverLimit,
                    //            paramInfo);
                    //        if (ret == false)
                    //        {
                    //            return false;
                    //        }
                    //    }
                    //    cnt = cnt + 1;
                    //    // TODO:commit;
                    //}
                }
                else
                {
                    // TODO:rollback;
                }

            }
            return true;
        }
        #endregion

        #region レベル５～８
        #region 製造オ－ダ明細の作成は廃止2021.11.13
        /// <summary>
        /// 製造オ－ダ明細の作成
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="rec_fixed">処理データ</param>
        /// <param name="qty">数量</param>
        /// <param name="deliverLimit">納期</param>
        /// <param name="manufacturingCost">コスト区分</param>
        /// <param name="costUpdateManage">原価更新管理</param>
        /// <param name="naiKaku">確定フラグ</param>
        /// <returns>(戻値)true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool PakFixedMakeDirectionProcedure(
            ComDB db,
            Dao.FixProductionOrder rec_fixed,
            decimal qty,
            DateTime deliverLimit,
            int manufacturingCost,
            decimal costUpdateManage,
            int naiKaku)
        {
            /*
            string sql = "";
            DateTime ss_edpd = DateTime.MinValue;
            DateTime? ss_date = rec_fixed.OrderDate;
            DateTime? ee_date = DateTime.MinValue;
            //decimal? gettan = 0;
            //int tmp_flg = 0;
            //int w_NaiKaku = 0;
            int intRet = 0;
            //decimal szkin = 0;
            //decimal tskin = 0;
            //decimal? zritu = 0;
            //int shasu = 0;
            //int shasutn = 0;

            // 確定取込のときは画面指定納期
            if (rec_fixed.OrderRule == 4)
            {
                ss_edpd = deliverLimit;
            }
            else
            {
                ss_edpd = wkPDATE.GetValueOrDefault();
            }

            // シャチハタの作業手順マスタ → BOMから取得
            //sql = "";
            //sql = sql + "select ";
            //sql = sql + "    * ";
            //sql = sql + "from ";
            //sql = sql + "    operation_procedure ";
            //sql = sql + "where ";
            //sql = sql + "    1 = 1 ";
            //sql = sql + "and item_cd = '" + rec_fixed.ItemCd + "' ";
            //sql = sql + "order by ";
            //sql = sql + "    operation_no ";
            sql = "";
            sql = sql + "select ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    recipe_procedure ";
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and recipe_id = ( ";
            sql = sql + "    select ";
            sql = sql + "        recipe_id ";
            sql = sql + "    from ";
            sql = sql + "        recipe_header ";
            sql = sql + "    where ";
            sql = sql + "        1 = 1 ";
            sql = sql + "    and item_cd = '" + rec_fixed.ItemCd + "' ";
            sql = sql + "    and specification_cd = '" + rec_fixed.SpecificationCd + "' ";
            sql = sql + "    and start_date <= '" + rec_fixed.ActiveDate + "' ";
            sql = sql + "    and end_date >= '" + rec_fixed.ActiveDate + "' ";
            sql = sql + ") ";
            sql = sql + "order by ";
            sql = sql + "    step_no ";


            decimal sum_sgtm = 0;
            decimal sgtm_dt = 0;
            decimal sum_ldtm = 0;
            decimal cur_ldtm = 0;
            int row_cnt = 0;
            int ss_cnt = 0;

            // データクラス化
            //IList<ComDao.RecipeProcedureEntity> cur_ope = db.GetListByDataClass<ComDao.RecipeProcedureEntity>(sql);
            dynamic cur_ope = db.GetListByDataClass<ComDao.RecipeProcedureEntity>(sql);
            row_cnt = db.GetCount(sql);
            foreach (var rec_ope in cur_ope)
            {
                // リードタイム算出処理 作業区分 0:外作 1:社内作業
                if (rec_ope.OperationDivision == 0)
                {
                    if (sum_sgtm > 0)
                    {
                        if (sum_sgtm % sgtm_dt == 0)
                        {
                            sum_ldtm = sum_ldtm + sum_sgtm / sgtm_dt;
                        }
                        else
                        {
                            sum_ldtm = sum_ldtm + Math.Floor(sum_sgtm / sgtm_dt) + 1;
                        }
                        sum_sgtm = 0;
                    }
                    sum_ldtm = sum_ldtm + rec_ope.LeadTime;
                } // if (rec_ope.OperationDivision == 0)
                else if (rec_ope.OperationDivision == 1)
                {
                    sum_sgtm = sum_sgtm + rec_ope.OperationTime;
                }
                else if (rec_ope.OperationDivision == 2)
                {
                    if (rec_ope.PlanedQty != 0)
                    {
                        sum_sgtm = sum_sgtm + qty * (rec_ope.OperationTime / rec_ope.PlanedQty);
                    }
                }
                // 作業区分が 0:外注
                if (rec_ope.OperationDivision == 0)
                {
                    cur_ldtm = sum_ldtm;
                }
                else
                {
                    if (ss_cnt == row_cnt)
                    {
                        if (sum_sgtm % sgtm_dt == 0)
                        {
                            cur_ldtm = sum_ldtm + Math.Floor(sum_sgtm / sgtm_dt);
                        }
                        else
                        {
                            cur_ldtm = sum_ldtm + Math.Floor(sum_sgtm / sgtm_dt) + 1;
                        }
                    }
                } // (rec_ope.OperationDivision == 0)
                if (ss_cnt == row_cnt)
                {
                    ee_date = ss_edpd; // 製造オーダーの完成予定日
                }
                else
                {
                    // 納期算出
                    sql = "";
                    sql = sql + "select ";
                    sql = sql + "    max(cal_date) ";
                    sql = sql + "from ";
                    sql = sql + "    cal ";
                    sql = sql + "where ";
                    sql = sql + "    1 = 1 ";
                    sql = sql + "and cal_date <= '" + rec_fixed.DeliveryDate.GetValueOrDefault().ToString("YYYY/MM/DD") + "' ";
                    sql = sql + "and cal_holiday = 0 ";
                    sql = sql + "and cal_cd = (select name01 from names where name_division = 'PCCD' and name_cd = '1') ";
                    sql = sql + "limit 1";

                    ee_date = db.GetEntity<DateTime>(sql);

                    if (ee_date == null)
                    {
                        // TODO:ログ出力
                        return false;
                    }
                    if (ee_date > ss_edpd)
                    {
                        ee_date = ss_edpd;
                    }
                } // if (ss_cnt == row_cnt)

                // 作業区分が 0:外注 なら購買外注オーダー作成
                // 2:工程内外注 の作成は廃止
                //if (rec_ope.OperationDivision == 0)
                //{
                //    // 伝票番号採番(NOKEY_HACHU)
                //    // TODO:PRO_GET_SLIP_NO → ProSeqGetNoに置き換え
                //    PrcUtil.ProSeqGetNo(db, "NOKEY_HACHU", null, null, null, null, null, null);
                //    // 取引先マスタの検索（仕入先）
                //    sql = "";
                //    sql = sql + "select ";
                //    sql = sql + "    tanto_cd ";
                //    sql = sql + "    , deliver_day1 ";
                //    sql = sql + "    , deliver_day2 ";
                //    sql = sql + "from ";
                //    sql = sql + "    vender ";
                //    sql = sql + "where ";
                //    sql = sql + "    1 = 1 ";
                //    sql = sql + "and vender_division = 'SI' ";
                //    sql = sql + "and vender_cd = '" + rec_ope.WorkCenterCd + "' ";
                //    sql = sql + "limit 1 ";

                //    ComDao.VenderEntity vender = new ComDao.VenderEntity();
                //    vender = db.GetEntity<ComDao.VenderEntity>(sql);
                //    if (vender == null)
                //    {
                //        // TOOD:エラーログ出力
                //        return false;
                //    }
                //    // 決定単価取得

                //    PakCommonGetTanka(db, "SI", rec_ope.WorkCenterCd, rec_fixed.ItemCd, rec_fixed.SpecificationCd, DateTime.Now, qty, ref gettan);
                //    // 仮単価フラグ＝通常(発注単価が０の時は仮単価)
                //    if (gettan == 0)
                //    {
                //        tmp_flg = 1;
                //        // TODO:ログ出力
                //    }
                //    else
                //    {
                //        tmp_flg = 0;
                //    }
                //    // 内示・確定区分編集
                //    if (naiKaku == 0)
                //    {
                //        w_NaiKaku = 1;
                //    }
                //    else
                //    {
                //        w_NaiKaku = 0;
                //    }
                //    // 消費税計算
                //    decimal king;
                //    king = qty * gettan.GetValueOrDefault();
                //    bool ret = false;
                //    ret = PakCommonCalcTaxAmt(db, "SI", rec_ope.WorkCenterCd, rec_fixed.ItemCd, ref king, ref szkin, ref tskin, ref zritu, ref shasu, ref shasutn);
                //    if (ret == true)
                //    {
                //        king = PakCommonRounding(king, shasu, shasutn);
                //    }
                //    else
                //    {
                //        zritu = 0;
                //    }
                //    // 納期から注文書表示納期[ORDER_DUE_DATE]を計算
                //    DateTime order_due_date = DateTime.MinValue;
                //    order_due_date = PakFixedCalcOrderDueDate(db, ee_date.GetValueOrDefault(), manufacturingCost);

                // TODO:INSERT INTO PURCHASE_SUBCONTRACT オーダー区分 2:工程内外注 は 不要

                //} // if (rec_ope.OperationDivision == 0) 作業区分が 0:外注 なら購買外注オーダー作成

            // 製造オ－ダ明細の作成
            if (naiKaku == 0)
                {
                    sql = "";
                    sql = sql + "insert into direction_procedure ( ";
                    sql = sql + "   direction_division ";
                    sql = sql + "   ,direction_no ";
                    //sql = sql + "   ,step_no ";
                    sql = sql + "   ,seq ";
                    sql = sql + "   ,operation_cd ";
                    sql = sql + "   ,condition ";
                    sql = sql + "   ,remark ";
                    sql = sql + "   ,notes ";
                    sql = sql + "   ,start_date ";
                    sql = sql + "   ,end_date ";
                    sql = sql + "   ,result_start_date ";
                    sql = sql + "   ,result_end_date ";
                    sql = sql + "   ,input_date ";
                    sql = sql + "   ,input_user_id ";
                    sql = sql + "   ,update_date ";
                    sql = sql + "   ,update_user_id ";
                    sql = sql + ") values ( ";
                    sql = sql + "   1 ";                                            // 指図区分|各種名称マスタ
                    sql = sql + "   ,'" + wkodrNo + "' ";                           // 指図番号
                    sql = sql + "   ,1 ";                                           // 工程番号
                    sql = sql + "   ,1 ";                                           // 表示順
                    sql = sql + "   ,null ";                                        // 工程コード
                    sql = sql + "   ,null ";                                        // 条件
                    sql = sql + "   ,null ";                                        // 備考
                    sql = sql + "   ,notes ";                                       // 注釈
                    sql = sql + "   , todate('" + ss_date + "', 'YYYY/MM/DD') ";    // 有効開始日時
                    sql = sql + "   , todate('" + ee_date + "', 'YYYY/MM/DD') ";    // 有効終了日時
                    sql = sql + "   ,null ";                                        // 開始実績日時
                    sql = sql + "   ,null ";                                        // 終了実績日時
                    sql = sql + "    , Now() ";                                     // 登録日時
                    sql = sql + "    , '" + updatorCd + "' ";                       // 登録者ID
                    sql = sql + "    , Now() ";                                     // 更新日時
                    sql = sql + "    , '" + updatorCd + "' ";                       // 更新者ID
                    sql = sql + ") ";

                    intRet = db.Regist(sql);
                    if (intRet < 0)
                    {
                        // TODO:ログ出力

                        return false;

                    }
                }
                // WK着手予定日にWK完成予定日をセット
                ss_date = ee_date;
                ss_cnt = ss_cnt + 1;
            } // foreach (var rec_ope in cur_ope)


            // 作業手順が無ければ空明細出力
            if (naiKaku == 0)
            {
                if (ss_cnt == 0)
                {
                    sql = "";
                    sql = sql + "insert into direction_procedure ( ";
                    sql = sql + "   direction_division ";
                    sql = sql + "   ,direction_no ";
                    //sql = sql + "   ,step_no ";
                    sql = sql + "   ,seq ";
                    sql = sql + "   ,operation_cd ";
                    sql = sql + "   ,condition ";
                    sql = sql + "   ,remark ";
                    sql = sql + "   ,notes ";
                    sql = sql + "   ,start_date ";
                    sql = sql + "   ,end_date ";
                    sql = sql + "   ,result_start_date ";
                    sql = sql + "   ,result_end_date ";
                    sql = sql + "   ,input_date ";
                    sql = sql + "   ,input_user_id ";
                    sql = sql + "   ,update_date ";
                    sql = sql + "   ,update_user_id ";
                    sql = sql + ") values ( ";
                    sql = sql + "   1 ";                                            // 指図区分|各種名称マスタ
                    sql = sql + "   ,'" + wkodrNo + "' ";                           // 指図番号
                    sql = sql + "   ,1 ";                                           // 工程番号
                    sql = sql + "   ,1 ";                                           // 表示順
                    sql = sql + "   ,null ";                                        // 工程コード
                    sql = sql + "   ,null ";                                        // 条件
                    sql = sql + "   ,null ";                                        // 備考
                    sql = sql + "   ,notes ";                                       // 注釈
                    sql = sql + "   , todate('" + ss_date + "', 'YYYY/MM/DD') ";    // 有効開始日時
                    sql = sql + "   , todate('" + ee_date + "', 'YYYY/MM/DD') ";    // 有効終了日時
                    sql = sql + "   ,null ";                                        // 開始実績日時
                    sql = sql + "   ,null ";                                        // 終了実績日時
                    sql = sql + "    , Now() ";                                     // 登録日時
                    sql = sql + "    , '" + updatorCd + "' ";                       // 登録者ID
                    sql = sql + "    , Now() ";                                     // 更新日時
                    sql = sql + "    , '" + updatorCd + "' ";                       // 更新者ID
                    sql = sql + ") ";

                }
            }
            */
            return true;
        }
        #endregion

        #region 引当明細ファイル登録処理は受払ソース登録処理は在庫更新プロシージャにて置換え
        ///// <summary>
        ///// 引当明細ファイル登録処理
        ///// </summary>
        ///// <param name="db">DB操作クラス</param>
        ///// <param name="wTrnOwn">親部品情報</param>
        ///// <param name="resultList">部品展開結果</param>
        ///// <param name="pInfo">パラメータ情報</param>
        ///// <param name="inventoryRegistDivision">在庫更新区分</param>
        ///// <returns>true:OKまたはfalse:NG</returns>
        //public static bool PakFixedMakeInoutSource(
        //    ComDB db
        //    , Dao.TrnOwn wTrnOwn
        //    , List<PakDao.TempPartsStructure> resultList
        //    , PakDao.ComParamInfo pInfo
        //    , string inventoryRegistDivision
        //)
        //{
        //    return false;
        //}
        #endregion
        #endregion

        /// <summary>
        /// 発注確定取込処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="rec_fixed">処理データ</param>
        /// <param name="qty">数量</param>
        /// <param name="marumeFlg">まるめフラグ</param>
        /// <param name="naiKaku">確定フラグ</param>
        /// <param name="deliverLimit">納期</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <returns>(戻値)true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool PakFixedFixFetchOrderSub(
            ComDB db,
            Dao.FixProductionOrder rec_fixed,
            decimal qty,
            bool marumeFlg,
            int naiKaku,
            DateTime? deliverLimit,
            PakDao.ComParamInfo paramInfo)
        {
            bool ret = false;
            DateTime? nd_day = DateTime.MinValue;

            // 発注納期を元に仕入先の納入曜日を考慮して、納入日を決定する
            if (rec_fixed.OrderRule == 4)
            {
                //ret = PakFixedGetDeliverDate(
                //    db
                //    , rec_fixed.VenderCd
                //    , rec_fixed.DeliveryDate.GetValueOrDefault()
                //    , ref nd_day
                //    , pInfo
                //    );
                //if (ret == false)
                //{
                //    return false;
                //}
                // if (marume == 0)
                if (marumeFlg == false)
                {
                    nd_day = rec_fixed.DeliveryDate.GetValueOrDefault();
                }
                else
                {
                    nd_day = deliverLimit;
                }
            }
            else
            {
                //nd_day = wkPDATE.GetValueOrDefault();
                // if (marume == 0)
                if (marumeFlg == false)
                {
                    nd_day = rec_fixed.DeliveryDate.GetValueOrDefault();
                }
                else
                {
                    nd_day = deliverLimit;
                }
            }
            Dictionary<string, string> empty = new Dictionary<string, string>();
            // 発注確定取込処理
            ret = PakFixedFixOrderSub(
                db,
                rec_fixed,
                qty,
                nd_day,
                marumeFlg,
                naiKaku,
                paramInfo,
                ref empty);
            if (ret == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 納入日決定処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="venderCd">仕入先コード</param>
        /// <param name="planDate">計画日</param>
        /// <param name="deliverDate">納期</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <returns>(戻値)true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool PakFixedGetDeliverDate(
            ComDB db,
            string venderCd,
            DateTime planDate,
            ref DateTime? deliverDate,
            PakDao.ComParamInfo paramInfo)
        {
            string sql = "";

            int delv_day1 = 0;
            int delv_day2 = 0;
            int? cal_week = 0;

            // 取引先マスタ取り込み
            // 納入曜日１、納入曜日２がない
            sql = "";
            sql = sql + "select ";
            //sql = sql + "    deliver_day1 ";
            //sql = sql + "    ,deliver_day2 ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    vender ";
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and vender_division = 'SI' ";
            sql = sql + "and vender_cd = @VenderCd ";
            sql = sql + "limit 1 ";

            ComDao.VenderEntity vender = new ComDao.VenderEntity();
            vender = db.GetEntityByDataClass<ComDao.VenderEntity>(
                sql,
                new
                {
                    VenderCd = venderCd
                });
            if (vender == null)
            {
                // ログ出力:取引先マスタ
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.M00306, venderCd, ComID.I10089 }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                return false;
            }
            // カレンダーマスタ検索(納期から曜日を求める)
            sql = "";
            sql = sql + "select ";
            sql = sql + "    cal_week ";
            sql = sql + "from ";
            sql = sql + "    cal ";
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and to_char(cal_date, 'YYYY/MM/DD') = @PlanDate ";
            //sql = sql + "and cal_cd = (select name01 from names where name_division = 'PCCD' and name_cd = '1') ";
            sql = sql + "and cal_cd = (select cal_cd from company where company_cd = (select utility_no from utility where utility_division = 'CMPN' and utility_cd = '1')) ";
            sql = sql + "limit 1 ";

            cal_week = db.GetEntity<int?>(
                sql,
                new
                {
                    PlanDate = planDate.ToString("yyyy/MM/dd"),
                });
            if (cal_week == null)
            {
                // ログ出力:曜日取得に失敗しました。
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10026, ComID.MB10076 }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                return false;
            }
            // 取引先マスタの納入曜日の設定により納入日を決定→決定した納入日を納期とする
            if (delv_day1 == 0 || delv_day1 == 7)
            {
                // 納入曜日1=0:未設定　OR　7:毎日　の時は直前の稼働日を納入日とする
                sql = "";
                sql = sql + "select ";
                sql = sql + "    max(cal_date) ";
                sql = sql + "from ";
                sql = sql + "    cal ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and to_char(cal_date, 'YYYY/MM/DD') <= '" + planDate.ToString("yyyy/MM/dd") + "' ";
                //sql = sql + "and cal_cd = (select name01 from names where name_division = 'PCCD' and name_cd = '1') ";
                sql = sql + "and cal_cd = (select cal_cd from company where company_cd = (select utility_no from utility where utility_division = 'CMPN' and utility_cd = '1')) ";
                sql = sql + "and cal_holiday = 0 ";
                sql = sql + "limit 1 ";

                deliverDate = db.GetEntity<DateTime?>(sql);
                if (deliverDate == null)
                {
                    // ログ出力:納期取得に失敗しました。
                    logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10026, ComID.I10036 }, languageId: paramInfo.LanguageId);
                    logger.Error(logMessage);
                    return false;
                }
            }
            else
            {
                if (delv_day2 == 0 || delv_day2 == 7)
                {
                    // 納入曜日2=0:未設定　or　7:毎日　の時､(すなわち週１回納入の時),納期から逆算して、納入曜日１の日を算出
                    sql = "";
                    sql = sql + "select ";
                    sql = sql + "    max(cal_date) ";
                    sql = sql + "from ";
                    sql = sql + "    cal ";
                    sql = sql + "where ";
                    sql = sql + "    1 = 1 ";
                    sql = sql + "and to_char(cal_date, 'YYYY/MM/DD') <= @PlanDate ";
                    //sql = sql + "and cal_cd = (select name01 from names where name_division = 'PCCD' and name_cd = '1') ";
                    sql = sql + "and cal_cd = (select cal_cd from company where company_cd = (select utility_no from utility where utility_division = 'CMPN' and utility_cd = '1')) ";
                    sql = sql + "and cal_week = @DelvDay1 ";
                    sql = sql + "and cal_holiday = 0 ";
                    sql = sql + "limit 1 ";

                    deliverDate = db.GetEntity<DateTime?>(
                        sql,
                        new
                        {
                            PlanDate = planDate.ToString("yyyy/MM/dd"),
                            DelvDay1 = delv_day1
                        });
                    if (deliverDate == null)
                    {
                        // ログ出力:納期取得に失敗しました。
                        logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10026, ComID.I10036 }, languageId: paramInfo.LanguageId);
                        logger.Error(logMessage);
                        return false;
                    }
                } // if (delv_day2 == 0 || delv_day2 == 7)
                else
                {
                    // 納入曜日が週２便の時の処理
                    if (delv_day1 <= cal_week && cal_week < delv_day2)
                    {
                        sql = "";
                        sql = sql + "select ";
                        sql = sql + "    max(cal_date) ";
                        sql = sql + "from ";
                        sql = sql + "    cal ";
                        sql = sql + "where ";
                        sql = sql + "    1 = 1 ";
                        sql = sql + "and to_char(cal_date, 'YYYY/MM/DD') <= @PlanDate ";
                        //sql = sql + "and cal_cd = (select name01 from names where name_division = 'PCCD' and name_cd = '1') ";
                        sql = sql + "and cal_cd = (select cal_cd from company where company_cd = (select utility_no from utility where utility_division = 'CMPN' and utility_cd = '1')) ";
                        sql = sql + "and cal_week = @DelvDay1 ";
                        sql = sql + "and cal_holiday = 0 ";
                        sql = sql + "limit 1 ";

                        deliverDate = db.GetEntity<DateTime?>(
                            sql,
                            new
                            {
                                PlanDate = planDate.ToString("yyyy/MM/dd"),
                                DelvDay1 = delv_day1
                            });
                        if (deliverDate == null)
                        {
                            // ログ出力:納期取得に失敗しました。
                            logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10026, ComID.I10036 }, languageId: paramInfo.LanguageId);
                            logger.Error(logMessage);
                            return false;
                        }

                    } // if (delv_day1 <= cal_week && cal_week < delv_day2)
                    else
                    {
                        sql = "";
                        sql = sql + "select ";
                        sql = sql + "    max(cal_date) ";
                        sql = sql + "from ";
                        sql = sql + "    cal ";
                        sql = sql + "where ";
                        sql = sql + "    1 = 1 ";
                        sql = sql + "and to_char(cal_date, 'YYYY/MM/DD') <= @PlanDate ";
                        //sql = sql + "and cal_cd = (select name01 from names where name_division = 'PCCD' and name_cd = '1') ";
                        sql = sql + "and cal_cd = (select cal_cd from company where company_cd = (select utility_no from utility where utility_division = 'CMPN' and utility_cd = '1')) ";
                        sql = sql + "and cal_week = @DelvDay2 ";
                        sql = sql + "and cal_holiday = 0 ";
                        sql = sql + "limit 1 ";

                        deliverDate = db.GetEntity<DateTime?>(
                            sql,
                            new
                            {
                                PlanDate = planDate.ToString("yyyy/MM/dd"),
                                DelvDay2 = delv_day2
                            });
                        if (deliverDate == null)
                        {
                            // ログ出力:納期取得に失敗しました。
                            logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10026, ComID.I10036 }, languageId: paramInfo.LanguageId);
                            logger.Error(logMessage);
                            return false;
                        }
                    }

                }
            }
            // 指定納期 < システム日付の場合　システム日付を納期とする。
            if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) > int.Parse(deliverDate.GetValueOrDefault().ToString("yyyyMMdd")))
            {
                deliverDate = DateTime.Today;
            }

            return true;
        }

        /// <summary>
        /// 納期から注文書表示納期[ORDER_DUE_DATE]を計算
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="deliveryDate">納期</param>
        /// <param name="costDivision">コスト区分</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        public static DateTime PakFixedCalcOrderDueDate(
            ComDB db,
            DateTime? deliveryDate,
            int costDivision)
        {
            DateTime ret = DateTime.MinValue;
            DateTime orderDueDate = DateTime.MinValue;
            int chosei = 0;
            int keyren = 0;
            int tmp_num = 0;
            int changeDays = 0;
            string sql = "";

            // 名称マスタから原価項目別の注文書表示前倒日数を取得
            // 現行もすべて０ name_division = 'CTDV' nmqty01
            changeDays = 0;

            // 前倒し日数を加算する
            keyren = changeDays + chosei;

            sql = "";
            sql = sql + "select ";
            sql = sql + "    cal_date ";
            sql = sql + "from ";
            sql = sql + "    cal ";
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and cal_date <= to_date( @DeliveryDate , 'YYYY/MM/DD') ";
            sql = sql + "order by ";
            sql = sql + "    cal_date desc ";

            IList<DateTime> recCal = db.GetList<DateTime>(
                sql,
                new
                {
                    DeliveryDate = deliveryDate.GetValueOrDefault().ToString("yyyy/MM/dd")
                });

            if (recCal.Count == 0)
            {
                return ret;
            }
            foreach (DateTime curCal in recCal)
            {
                orderDueDate = curCal;
                if (keyren == tmp_num)
                {
                    break;
                }
                tmp_num = tmp_num + 1;
            }
            return orderDueDate;

        }

        /// <summary>
        /// 消費税の計算
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="venderDivision">取引先区分|TS:得意先 SI:仕入先</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="itemCd">品目コード</param>
        /// <param name="amount">元金額</param>
        /// <param name="tax">消費税</param>
        /// <param name="taxAmount">課税対象額</param>
        /// <param name="taxRate">消費税率</param>
        /// <param name="round">仕入仕入金額端数処理</param>
        /// <param name="roundUnit">仕入仕入金額端数単位</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool PakCommonCalcTaxAmt(
            ComDB db,
            string venderDivision,
            string venderCd,
            string itemCd,
            ref decimal  amount,
            ref decimal  tax,
            ref decimal  taxAmount,
            ref decimal? taxRate,
            ref int      round,
            ref int      roundUnit,
            PakDao.ComParamInfo paramInfo)
        {
            bool ret = false;

            decimal taxRateBuf = 0;

            int? calcDivision = 0;
            int? taxDivision = 0;
            int? taxRoundDivision = 0;
            int? taxRoundUnit = 0;
            int? saleRoundDivision = 0;
            int? saleRoundUnit = 0;
            int? purchaseRoundDivision = 0;
            int? purchaseRoundUnit = 0;

            ret = PakCommonGetTaxParam(
                db,
                venderDivision,
                venderCd,
                itemCd,
                ref calcDivision,
                ref taxDivision,
                ref taxRate,
                ref taxRoundDivision,
                ref taxRoundUnit,
                ref saleRoundDivision,
                ref saleRoundUnit,
                ref purchaseRoundDivision,
                ref purchaseRoundUnit);
            if (ret == false)
            {
                return false;
            }
            if (venderDivision == "TS")
            {
                amount = PakCommonRounding(amount, saleRoundDivision.GetValueOrDefault(), saleRoundUnit.GetValueOrDefault(), paramInfo);
            }
            else if (venderDivision == "SI")
            {
                amount = PakCommonRounding(amount, purchaseRoundDivision.GetValueOrDefault(), purchaseRoundUnit.GetValueOrDefault(), paramInfo);
            }
            if (taxDivision != 0 && taxDivision != 1)
            {
                tax = 0;
                taxAmount = amount;
            }
            taxRateBuf = taxRate.GetValueOrDefault() / 100;
            if (taxDivision == 0)
            {
                // 外税
                tax = PakCommonRounding(amount * taxRate.GetValueOrDefault(), taxRoundDivision.GetValueOrDefault(), taxRoundUnit.GetValueOrDefault(), paramInfo);
            }
            else if (taxDivision == 1)
            {
                // 内税
                tax = PakCommonRounding(amount * taxRate.GetValueOrDefault() / (1 + taxRate.GetValueOrDefault()), taxRoundDivision.GetValueOrDefault(), taxRoundUnit.GetValueOrDefault(), paramInfo);
            }
            if (taxDivision == 0)
            {
                taxAmount = amount;
            }
            else
            {
                taxAmount = amount - tax;
            }

            return true;

        }

        /// <summary>
        /// 消費税パラメータ取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="venderDivision">取引先区分|TS:得意先 SI:仕入先</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="itemCd">品目コード</param>
        /// <param name="calcDivision">算出区分     [0:明細 1:伝票 2:請求時 3:自社マスタ]</param>
        /// <param name="taxDivision">消費税課税区分</param>
        /// <param name="taxRate">消費税率</param>
        /// <param name="taxRoundDivision">消費税端数処理区分</param>
        /// <param name="taxRoundUnit">消費税端数処理単位</param>
        /// <param name="saleRoundDivision">売上仕入金額端数処理</param>
        /// <param name="saleRoundUnit">売上仕入金額端数単位</param>
        /// <param name="purchaseRoundDivision">仕入仕入金額端数処理</param>
        /// <param name="purchaseRoundUnit">仕入仕入金額端数単位</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool PakCommonGetTaxParam(
            ComDB db,
            string venderDivision,
            string venderCd,
            string itemCd,
            ref int? calcDivision,
            ref int? taxDivision,
            ref decimal? taxRate,
            ref int? taxRoundDivision,
            ref int? taxRoundUnit,
            ref int? saleRoundDivision,
            ref int? saleRoundUnit,
            ref int? purchaseRoundDivision,
            ref int? purchaseRoundUnit)
        {
            //bool ret = false;
            string sql = "";

            // 自社マスタの消費税処理パラメータ取得
            sql = "";
            sql = sql + "select ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    company ";

            ComDao.CompanyEntity company = new ComDao.CompanyEntity();
            company = db.GetEntityByDataClass<ComDao.CompanyEntity>(sql);
            if (company == null)
            {
                return false;
            }
            calcDivision = company.CalcDivision;                // 消費税算出区分
            taxDivision = company.TaxDivision;                  // 消費税課税区分
            //taxRate = company.TaxRatio;                         // 消費税率
            taxRoundDivision = company.TaxRoundup;              // 消費税端数処理区分
            taxRoundUnit = company.TaxRoundupUnit;              // 消費税端数処理単位
            saleRoundDivision = company.SalesRoundup;           // 売上金額端数処理
            saleRoundUnit = company.SalesRoundupUnit;           // 売上金額端数処理単位
            purchaseRoundDivision = company.PurchaseRoundup;    // 仕入金額端数処理
            purchaseRoundUnit = company.PurchaseRoundupUnit;    // 仕入金額端数処理単位

            // 取引先マスタ検索
            sql = "";
            sql = sql + "select ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    vender ";
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and vender_division = @VenderDivision ";
            sql = sql + "and vender_cd = @VenderCd ";
            sql = sql + "order by ";
            sql = sql + "    active_date desc ";
            sql = sql + "limit 1 ";

            ComDao.VenderEntity vender = new ComDao.VenderEntity();
            vender = db.GetEntityByDataClass<ComDao.VenderEntity>(
                sql,
                new
                {
                    VenderDivision = venderDivision,
                    VenderCd = venderCd
                });
            if (vender == null)
            {
                return false;
            }
            int? venderTaxDivision = vender.TaxDivision;                             // 消費税区分
            // 消費税率がない
            int? venderTaxRoundup = vender.TaxRoundup;                               // 消費税端数処理区分
            int? venderTaxRoundupUnit = vender.TaxRoundupUnit;                       // 消費税端数処理単位
            int? venderSalesPurchaseRoundup = vender.SalesPurchaseRoundup;           // 売上仕入金額端数処理
            int? venderSalesPurchaseRoundupUnit = vender.SalesPurchaseRoundupUnit;   // 売上仕入金額端数単位
            int? venderCalcDivision = vender.CalcDivision;                           // 算出区分

            // 消費税区分 3:自社マスタ優先
            //if (vTaxDivision != 3)
            if (venderTaxDivision != 4)
            {
                // CTAX 4:自社マスタ
                taxDivision = venderTaxDivision;
                //taxRate = vTaxRate;
            }
            // 消費税端数処理区分 3:自社マスタ優先
            //if (vTaxRoundup != 3)
            if (venderTaxRoundup != 4)
            {
                // CRUP 4:自社マスタ
                taxRoundDivision = venderTaxRoundup;
                taxRoundUnit = venderTaxRoundupUnit;
            }
            // 売上仕入金額端数処理 3:自社マスタ優先
            //if (vSalesPurchaseRoundup != 3)
            if (venderSalesPurchaseRoundup != 4)
            {
                // CRUP 4:自社マスタ
                saleRoundDivision = venderSalesPurchaseRoundup;
                saleRoundUnit = venderSalesPurchaseRoundupUnit;
                purchaseRoundDivision = venderSalesPurchaseRoundup;
                purchaseRoundUnit = venderSalesPurchaseRoundupUnit;
            }
            // 算出区分:3自社マスタ優先以外
            //if (vCalcDivision != 3)
            if (venderCalcDivision != 4)
            {
                // CCAL 4:自社マスタ
                calcDivision = venderCalcDivision;
            }
            // 品目コードの設定がないか、消費税区分 2:非課税の場合は品目マスタを参照しない
            if (!string.IsNullOrEmpty(itemCd) && taxDivision != 2)
            {
                // 品目マスタ検索
                // 品目マスタ、品目仕様マスタに税区分と税率はないので消費税区分マスタから取得する。
                //select
                //    tax_division
                //    , tax_ratio
                //from
                //    item
                //where
                //    1=1
                //and item_cd = itemCd
                //    order by  active_date desc
                //    limit 1

            }
            // 消費税区分マスタ
            sql = "";
            sql = sql + "select ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    tax_master ";
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and category = @Stocking ";
            sql = sql + "and tax_category = 1 "; // 軽減税率|1:通常 2:軽減
            sql = sql + "and tax_division1 = 1 "; // 課税区分|1:通常 2:不課税 3:非課税 4:免税
            sql = sql + "and valid_date <= to_date( @Now , 'YYYY/MM/DD') ";
            sql = sql + "order by ";
            sql = sql + "    valid_date desc ";
            sql = sql + "limit 1 ";

            ComDao.TaxMasterEntity tax_master = new ComDao.TaxMasterEntity();
            tax_master = db.GetEntityByDataClass<ComDao.TaxMasterEntity>(
                sql,
                new
                {
                    Stocking = MyConst.STOCKING,
                    Now = DateTime.Now.ToString("yyyy/MM/dd")
                });
            if (tax_master == null)
            {
                return false;
            }
            // 消費税率
            taxRate = tax_master.TaxRatio;

            return true;

        }

        /// <summary>
        /// 単価取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="division">単価区分|TS：標準販売単価 SI：標準仕入単価 SH：標準支給単価</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specificationCd">仕様コード</param>
        /// <param name="validDate">有効日</param>
        /// <param name="qty">数量</param>
        /// <param name="unitPrice">決定単価（返り値）</param>
        /// <param name="paramInfo">パラメータ情報</param>
        public static void PakCommonGetTanka(
            ComDB db,
            string division,
            string venderCd,
            string itemCd,
            string specificationCd,
            DateTime validDate,
            decimal qty,
            ref decimal? unitPrice,
            PakDao.ComParamInfo paramInfo)
        {
            unitPrice = 0;

            // 取引先別単価マスタ検索
            try
            {
                string sql;         // 動的SQL
                sql = "";
                sql = sql + "select ";
                sql = sql + "    a.unitprice ";
                sql = sql + "from ";
                sql = sql + "    ( ";
                sql = sql + "        select ";
                sql = sql + "            unitprice ";
                sql = sql + "        from ";
                sql = sql + "            unitprice ";
                sql = sql + "        where ";
                sql = sql + "            1 = 1 ";
                sql = sql + "        and vender_division = @Division ";
                sql = sql + "        and balance_cd = @VenderCd ";
                sql = sql + "        and item_cd = @ItemCd ";
                sql = sql + "        and specification_cd = @ItemCd ";
                sql = sql + "        and valid_date <= to_date( @ValidDate ,'YYYY/MM/DD HH24:MI:SS') ";
                sql = sql + "        and quantity_from <= @Qty ";
                sql = sql + "        order by ";
                sql = sql + "            valid_date desc, ";
                sql = sql + "            quantity_from desc ";
                sql = sql + "    ) a ";
                sql = sql + "limit 1 ";
                unitPrice = db.GetEntity<decimal?>(
                    sql,
                    new
                    {
                        Division = division,
                        VenderCd = venderCd,
                        ItemCd = itemCd,
                        SpecificationCd = specificationCd,
                        ValidDate = validDate.ToString(),
                        Qty = qty
                    });

                if (unitPrice == null)
                {
                    // 取引先別単価マスタ再検索(VENDER_CD = '0000000000')
                    sql = "";
                    sql = sql + "select ";
                    sql = sql + "    a.unitprice ";
                    sql = sql + "from ";
                    sql = sql + "    ( ";
                    sql = sql + "        select ";
                    sql = sql + "            unitprice ";
                    sql = sql + "        from ";
                    sql = sql + "            unitprice ";
                    sql = sql + "        where ";
                    sql = sql + "            1 = 1 ";
                    sql = sql + "        and vender_division = @Division ";
                    sql = sql + "        and balance_cd = '0000000000' ";
                    sql = sql + "        and item_cd = @ItemCd ";
                    sql = sql + "        and specification_cd = @SpecificationCd ";
                    sql = sql + "        and valid_date <= to_date( @ValidDate ,'YYYY/MM/DD HH24:MI:SS') ";
                    sql = sql + "        and quantity_from <= @Qty ";
                    sql = sql + "        order by ";
                    sql = sql + "            valid_date desc, ";
                    sql = sql + "            quantity_from desc ";
                    sql = sql + "    ) a ";
                    sql = sql + "limit 1 ";
                    unitPrice = db.GetEntity<decimal?>(
                        sql,
                        new
                        {
                            Division = division,
                            ItemCd = itemCd,
                            SpecificationCd = specificationCd,
                            ValidDate = validDate.ToString(),
                            Qty = qty
                        });
                    if (unitPrice == null)
                    {
                        unitPrice = 0;
                    }
                }
                return;

            }
            catch (Exception ex)
            {
                // ログ出力:単価取得 異常終了
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10072, ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                logMessage = "";
                logger.Error(ex.Message);
                logger.Error(ex.ToString());
                unitPrice = 0;
                return;
            }

        }

        #region 在庫更新
        /// <summary>
        /// 在庫更新（製造指図）
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="directionInfo">製造指図情報</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        private static bool inventoryRegistManufactureOrder(ComDB db, PlanPackage.ParamRegistDirection directionInfo, PakDao.ComParamInfo paramInfo)
        {
            string sql = "";

            // 製造指図ヘッダを取得
            ComDao.DirectionHeaderEntity headerBean = new ComDao.DirectionHeaderEntity();
            headerBean = headerBean.GetEntity(directionInfo.Common.DirectionDivision, directionInfo.Common.DirectionNo, db);

            // 製造指図フォーミュラを取得
            sql = "";
            sql = sql + "select ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    direction_formula ";
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and direction_division = @DirectionDivision ";
            sql = sql + "and direction_no = @DirectionNo ";
            sql = sql + "order by ";
            sql = sql + "    result_division ";
            sql = sql + "    , seq_no ";
            IList<ComDao.DirectionFormulaEntity> formulaList = db.GetListByDataClass<ComDao.DirectionFormulaEntity>(
                                                                    sql,
                                                                    new
                                                                    {
                                                                        DirectionDivision = directionInfo.Common.DirectionDivision,
                                                                        DirectionNo = directionInfo.Common.DirectionNo,
                                                                    });
            if (formulaList == null)
            {
                return false;
            }
            foreach (var formula in formulaList)
            {
                string inoutDivision = "";
                // 部品と仕上でパラメータが異なる
                if (formula.ResultDivision == 0)
                {
                    // 部品の受払区分「MAN0010 - 01」
                    inoutDivision = Constants.INOUT_DIVISION.DIRECTION_APPROVAL_PARTS;
                }
                else
                {
                    // 仕上の受払区分「MAN0010 - 02」
                    inoutDivision = Constants.INOUT_DIVISION.DIRECTION_APPROVAL_FINISH;
                }
                List<InvDao.InventoryParameter> paramList = new List<InvDao.InventoryParameter>();
                InvDao.InventoryParameter param = new InvDao.InventoryParameter();
                param.ProcessDivision = InvConst.INVENTORY_PROCESS_DIVISION.ADD;                  // 処理区分        = 0:登録
                param.InoutDivision = inoutDivision;                                              // 受払区分
                param.InoutDate = headerBean.PlanedEndDate.GetValueOrDefault();                   // 受払予定日      = 製造終了日
                param.OrderDivision = headerBean.DirectionDivision;                               // オーダー区分    = 製造指図区分
                param.OrderNo = headerBean.DirectionNo;                                           // オーダー番号    = 製造指図番号
                param.OrderLineNo1 = formula.ResultDivision;                                      // オーダー行番号1 = 実績区分 0:部品, 1:仕上
                //param.OrderLineNo2 = formula.Seq.GetValueOrDefault();                             // オーダー行番号2 = 表示順
                param.OrderLineNo2 = formula.SeqNo;                                               // オーダー行番号2 = シーケンスNO
                param.ResultOrderDivision = headerBean.DirectionDivision;                         // 伝票区分        = 製造指図区分
                param.ResultOrderNo = headerBean.DirectionNo;                                     // 伝票番号        = 製造指図番号
                param.ResultOrderLineNo1 = formula.ResultDivision;                                // 伝票行番号1     = 実績区分 0:部品, 1:仕上
                //param.ResultOrderLineNo2 = formula.Seq.GetValueOrDefault();                       // 伝票行番号2     = 表示順
                param.ResultOrderLineNo2 = formula.SeqNo;                                         // 伝票行番号2     = シーケンスNO
                param.ItemCd = formula.ItemCd;                                                    // 品目コード
                param.SpecificationCd = formula.SpecificationCd;                                  // 仕様コード
                param.LocationCd = formula.LocationCd;                                            // ロケーションコード
                param.LotNo = formula.LotNo;                                                      // ロット番号
                param.SubLotNo1 = formula.SubLotNo1;                                              // サブロット番号1
                param.SubLotNo2 = formula.SubLotNo2;                                              // サブロット番号2
                param.ReferenceNo = headerBean.ReferenceNo;                                       // リファレンス番号
                param.InoutQty = formula.Qty;                                                     // 受払数          = 数量
                param.InputMenuId = paramInfo.PgmId;                                              // メニュー番号    = プログラムID
                param.InputDisplayId = paramInfo.FormNo;                                          // タブ番号        = コンロールID
                param.InputControlId = int.Parse(MyConst.One);                                    // 操作番号        = 項目番号:1
                param.UserId = paramInfo.UserId;                                                  // ユーザID

                // パラメータリストに追加
                paramList.Add(param);

                // 在庫更新（製造指図）
                InvDao.InventoryReturnInfo result = InvManufactureOrder.InventoryRegist(db, paramList, paramInfo.LanguageId);

                if (result.ResultValue == false)
                {
                    logMessage = result.Message;
                    logger.Error(logMessage);
                    return false;
                }

            }
            return true;
        }

        /// <summary>
        /// 在庫更新（購入依頼）
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="purchaseRequestNo">購入依頼番号</param>
        /// <param name="rec_fixed">処理データ</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        private static bool inventoryRegistPurchaseRequest(ComDB db, string purchaseRequestNo, Dao.FixProductionOrder rec_fixed, PakDao.ComParamInfo paramInfo)
        {
            string sql = "";

            // 購入依頼ヘッダを取得
            ComDao.PurchaseRequestHeadEntity header = new ComDao.PurchaseRequestHeadEntity();
            header = header.GetEntity(purchaseRequestNo, db);

            // 購入依頼詳細を取得
            sql = "";
            sql = sql + "select ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    purchase_request_detail ";
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and purchase_request_no = @PurchaseRequestNo ";
            sql = sql + "order by ";
            sql = sql + "    purchase_request_row_no ";
            IList<ComDao.PurchaseRequestDetailEntity> detailList = db.GetListByDataClass<ComDao.PurchaseRequestDetailEntity>(
                                                                    sql,
                                                                    new
                                                                    {
                                                                        PurchaseRequestNo = purchaseRequestNo,
                                                                    });
            if (detailList == null)
            {
                return false;
            }
            foreach (var detail in detailList)
            {
                // パラメータを作成
                List<InvDao.InventoryParameter> paramList = new List<InvDao.InventoryParameter>();
                InvDao.InventoryParameter param = new InvDao.InventoryParameter();
                param.ProcessDivision = InvConst.INVENTORY_PROCESS_DIVISION.ADD;    // 処理区分           = 0:登録
                param.InoutDivision = Constants.INOUT_DIVISION.PURCHASE_REQUEST;    // 受払区分
                param.InoutDate = detail.RequestDeliveryDate;                       // 受払予定日         = 希望納期
                param.OrderDivision = detail.OrderDivision;                         // オーダー区分       = 発注区分 1:原材料, 3:外注製品（非直送）
                param.OrderNo = detail.PurchaseRequestNo;                           // オーダー番号       = 購入依頼番号
                param.OrderLineNo1 = detail.PurchaseRequestRowNo;                   // オーダー行番号1    = 購入依頼行番号
                param.ResultOrderDivision = detail.OrderDivision;                   // 伝票区分           = 発注区分 1:原材料, 3:外注製品（非直送）
                param.ResultOrderNo = detail.PurchaseRequestNo;                     // 伝票番号           = 購入依頼番号
                param.ResultOrderLineNo1 = detail.PurchaseRequestRowNo;             // 伝票行番号1        = 購入依頼行番号
                param.ItemCd = detail.ItemCd;                                       // 品目コード
                param.SpecificationCd = detail.SpecificationCd;                     // 仕様コード
                param.LocationCd = detail.DeliveryCd;                               // ロケーションコード = 納入先コード
                param.InoutQty = detail.PurchaseRequestQuantity;                    // 受払数             = 購入依頼数量
                param.ReferenceNo = rec_fixed.ReferenceNo;                          // リファレンス番号
                param.InputMenuId = paramInfo.PgmId;                                // メニュー番号       = プログラムID
                param.InputDisplayId = paramInfo.FormNo;                            // タブ番号           = コンロールID
                param.InputControlId = int.Parse(MyConst.One);                      // 操作番号           = 項目番号:1
                param.UserId = paramInfo.UserId;                                    // ユーザID

                // パラメータリストに追加
                paramList.Add(param);

                // 在庫更新（購入依頼）
                InvDao.InventoryReturnInfo result = InvPurchaseRequest.InventoryRegist(db, paramList, paramInfo.LanguageId);

                if (result.ResultValue == false)
                {
                    logger.Error(result.Message);
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 引当明細ファイル作成処理は廃止 2021.11.13
        /// <summary>
        /// 引当明細ファイル作成処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="trnOwn">親部品情報</param>
        /// <param name="trnKo">子部品情報</param>
        /// <param name="qty">数量</param>
        /// <param name="changeFlg">変更フラグ</param>
        /// <param name="zaikoKbn">在庫区分</param>
        /// <param name="resultList">部品展開結果</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        [Obsolete("現在未使用")]
        public static bool PakFixedCreateInoutSourceData(
            ComDB db,
            Dao.TrnOwn trnOwn,
            Dao.TrnKo trnKo,
            decimal qty,
            bool changeFlg,
            int zaikoKbn,
            List<PakDao.TempPartsStructure> resultList)
        {
            /*
            //string bk_hcd = null;
            //bool ret = false;
            //int wkhkFlg = 0;

            //int intRet = 0;
            //decimal? wkQty = 0;
            //string wkEndSeirialNo = "";

            //decimal? AllocationQty = 0;// 在庫引当数
            string sql = "";

            string ko_hcd;
            int? ko_supdiv;
            decimal? ko_useqty;
            string ko_kbid;
            string ko_locd;
            int? ko_lodiv;
            int? ko_zkdiv;
            string ko_outid;
            int? ko_outdiv;
            int unit_ctl;
            int inout_cnt;
            decimal hk_qty;
            int ret_cd;
            //string err_msg;
            string io_no;
            int io_div = 0;
            int gettan = 0;
            //int getrate;

            try
            {
                // 品目切替なし
                if (changeFlg == false)
                {
                    ko_hcd = trnKo.ChildItemCd;                    // 子品目コード
                    ko_supdiv = trnKo.StockDivision;               // 支給区分
                    ko_useqty = trnKo.UseQty2;                     // 使用数
                    ko_kbid = trnKo.PartsCd;                       // 基板ＩＤ
                    ko_locd = trnKo.DefaultLocation;               // ロケーションコード
                    ko_lodiv = trnKo.DefaultLocationDivision;      // ロケーション区分
                    ko_zkdiv = trnKo.StockDivision;                // 在庫管理区分
                    ko_outid = trnKo.OutId;                        // 出庫ＩＤ
                    ko_outdiv = trnKo.OutPrepatationDivision;      // 出庫区分
                }
                else
                {
                    ko_hcd = trnKo.AfterChildItemCd;               // 子品目コード
                    ko_supdiv = trnKo.AfterSupplyDivision;         // 支給区分
                    ko_useqty = trnKo.AfterUseQty2;                // 使用数
                    ko_kbid = trnKo.AfterPartsCd;                  // 基板ＩＤ
                    ko_locd = trnKo.AfterLocationcd;               // ロケーションコード
                    ko_lodiv = trnKo.AfterLocationDivision;        // ロケーション区分
                    ko_zkdiv = trnKo.AfterStockDivision;           // 在庫管理区分
                    ko_outid = trnKo.AfterOutId;                   // 出庫ＩＤ
                    ko_outdiv = trnKo.AfterOutPrepatationDivision; // 出庫区分
                }

                // 端数処理
                sql = "";
                sql = sql + "select ";
                sql = sql + "    unit_of_stock_control ";
                sql = sql + "from ";
                sql = sql + "    item ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and item_cd = '" + ko_hcd + "' ";
                sql = sql + "limit 1 ";
                unit_ctl = db.GetEntity<int>(sql);

                // 引当数
                hk_qty = qty;

                // 手続区分
                if (trnOwn.ProcedureDivision == 1)
                {
                    // オ－ダ区分=(製造)
                    io_div = 0;
                }
                else if (trnOwn.ProcedureDivision == 2)
                {
                    // オ－ダ区分=(購買)
                    io_div = 1;
                }

                // 引当明細ファイル存在チェック
                sql = "";
                sql = sql + "select ";
                sql = sql + "    * ";
                sql = sql + "from ";
                sql = sql + "    inout_source ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and inout_division = " + io_div.ToString() + " ";
                sql = sql + "and oder_no = '" + trnOwn.OrderNo + "' ";
                sql = sql + "and item_cd = '" + ko_hcd + "' ";
                sql = sql + "and parts_cd = '" + ko_kbid + "' ";
                inout_cnt = db.GetCount(sql);

                if (inout_cnt == 0)
                {
                    // 受払ソース番号(NOKEY_UKES)
                    //ProGetSlipNo() → ProSeqGetNo() にマージ
                    io_no = PrcUtil.ProSeqGetNo(db, "NOKEY_UKES");

                    // 支給単価
                    if (ko_supdiv == 1)
                    {
                        // 支給区分 = 有償
                        // TODO:PakComonGetTanka()
                    }
                    else
                    {
                        gettan = 0;
                    }
                    // データがない場合は insert
                    sql = "";
                    sql = sql + "insert into inout_source ( ";
                    sql = sql + "   inout_source_no ";
                    sql = sql + "   ,inout_division ";
                    sql = sql + "   ,oder_no ";
                    sql = sql + "   ,item_cd ";
                    sql = sql + "   ,parts_cd ";
                    sql = sql + "   ,reference_no ";
                    sql = sql + "   ,use_division ";
                    sql = sql + "   ,use_qty ";
                    sql = sql + "   ,allocated_qty ";
                    sql = sql + "   ,inout_date ";
                    sql = sql + "   ,inout_qty ";
                    sql = sql + "   ,last_out_date ";
                    sql = sql + "   ,supplied_goods_division ";
                    sql = sql + "   ,supplied_goods_qty ";
                    sql = sql + "   ,supplied_goods_cost ";
                    sql = sql + "   ,location_division_cd ";
                    sql = sql + "   ,location_cd ";
                    sql = sql + "   ,ship_division ";
                    sql = sql + "   ,transfer_qty ";
                    sql = sql + "   ,bad_quantity ";
                    sql = sql + "   ,out_prepatation_division ";
                    sql = sql + "   ,out_id ";
                    sql = sql + "   ,inout_status ";
                    sql = sql + "   ,parent_item_cd ";
                    sql = sql + "   ,change_spec_flg ";
                    sql = sql + "   ,input_date ";
                    sql = sql + "   ,inputor_cd ";
                    sql = sql + "   ,update_date ";
                    sql = sql + "   ,updator_cd ";
                    sql = sql + "   ,screen_id ";
                    sql = sql + ") values ( ";
                    sql = sql + "   '" + io_no + "' ";
                    sql = sql + "   ," + io_div.ToString() + " ";
                    sql = sql + "   ,'" + trnOwn.OrderNo + "' ";
                    sql = sql + "   ,'" + ko_hcd + "' ";
                    sql = sql + "   ,'" + ko_kbid + "' ";
                    sql = sql + "   ,'" + trnOwn.ReferenceNo + "' ";
                    sql = sql + "   ,0 ";
                    sql = sql + "   ," + ko_useqty.ToString() + " ";
                    sql = sql + "   ," + hk_qty.ToString() + " ";
                    sql = sql + "   ," + trnOwn.OrderDate.ToString() + " ";
                    sql = sql + "   ,0 ";
                    sql = sql + "   ,null ";
                    sql = sql + "   ," + ko_supdiv.ToString() + " ";
                    sql = sql + "   ,0 ";
                    sql = sql + "   ," + gettan.ToString() + " ";
                    sql = sql + "   ," + ko_lodiv.ToString() + " ";
                    sql = sql + "   ,'" + ko_locd + "' ";
                    sql = sql + "   ,'" + trnOwn.VenderCd + "' ";
                    sql = sql + "   ,0 ";
                    sql = sql + "   ,0 ";
                    sql = sql + "   ," + ko_outdiv.ToString() + " ";
                    sql = sql + "   ,'" + ko_outid + "' ";
                    sql = sql + "   ,0 ";
                    sql = sql + "   ,'" + trnOwn.ItemCd + "' ";
                    sql = sql + "   ," + zaikoKbn.ToString() + " ";
                    sql = sql + "   ,Now() ";
                    sql = sql + "   ,'" + updatorCd + "' ";
                    sql = sql + "   ,Now() ";
                    sql = sql + "   ,'" + updatorCd + "' ";
                    sql = sql + "   ,189 ";
                    sql = sql + ") ";

                }
                else // if (inout_cnt == 0)
                {
                    // データが存在している場合はUPDATE
                    sql = "";
                    sql = sql + "update ";
                    sql = sql + "    inout_source ";
                    sql = sql + "set ";
                    sql = sql + "    allocated_qty = allocated_qty + " + hk_qty.ToString() + " ";
                    // 丸めフラグの場合 use_qty を更新
                    if (trnOwn.MarumeFlg == false)
                    {
                        sql = sql + "    ,use_qty = use_qty + " + ko_useqty.ToString() + " ";
                    }
                    sql = sql + "    ,update_date = Now() ";
                    sql = sql + "    ,updator_cd = '" + updatorCd + " ";
                    sql = sql + "    ,screen_id = 189 ";
                    sql = sql + "where ";
                    sql = sql + "    1 = 1 ";
                    sql = sql + "and inout_division = " + io_div.ToString() + " ";
                    sql = sql + "and oder_no = '" + trnOwn.OrderNo + "' ";
                    sql = sql + "and item_cd = '" + ko_hcd + "' ";
                    sql = sql + "and parts_cd = '" + ko_kbid + "' ";

                }

                // SQL実行
                ret_cd = db.Regist(sql);

                // 在庫マスタ（引当残の更新）
                // item_inventory は不要
                //if (ko_zkdiv != 2)
                //{
                //    sql = "";
                //    sql = sql + "update ";
                //    sql = sql + "    item_inventory ";
                //    sql = sql + "set ";
                //    sql = sql + "    assign_qty = assign_qty + " + hk_qty.ToString() + " ";
                //    sql = sql + "    ,update_date = Now() ";
                //    sql = sql + "    ,updator_cd = '" + updatorCd + " ";
                //    sql = sql + "where ";
                //    sql = sql + "    1 = 1 ";
                //    sql = sql + "and item_cd = '" + ko_hcd + "' ";
                //    ret_cd = db.Regist(sql);
                //}

                return true;
            }
            catch (Exception ex)
            {
                // TODO:ログ出力
                logger.Error(ex.Message);
                //throw ex;
                return false;
            }
            */
            return true;

        }
        #endregion

        /// <summary>
        /// テーブルロック情報判定：在庫更新、バッチ処理でのロック情報判定
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="sequenceTop">先頭シーケンス</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        public static bool PakCommonInitSequenceNo(ComDB db, decimal? sequenceTop)
        {
            string sql = "";
            if (sequenceTop == null)
            {
                sql = "";
                sql = "select nextval('seq_nokey_1level');";
                sequenceTop = db.GetEntity<decimal>(sql);
            }
            return true;
        }

        /// <summary>
        /// ワークフローデータ削除
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="productProductDivision">製造品目区分</param>
        /// <param name="purchaseProductDivision">購買品目区分</param>
        /// <param name="orderProductDivision">外注品目区分</param>
        /// <returns>true:OKまたはfalse:NG</returns>
        public static bool PakFixedWorkFlowDelete(ComDB db, string productProductDivision, string purchaseProductDivision, string orderProductDivision)
        {
            int ret = 0;
            string sql = "";
            string workFlowDivision = "";
            if (productProductDivision == "1")
            {
                if (!string.IsNullOrEmpty(workFlowDivision))
                {
                    workFlowDivision = workFlowDivision + ", ";
                }
                workFlowDivision = workFlowDivision + "'PPL03'";
            }
            if (purchaseProductDivision == "1")
            {
                if (!string.IsNullOrEmpty(workFlowDivision))
                {
                    workFlowDivision = workFlowDivision + ", ";
                }
                workFlowDivision = workFlowDivision + "'PPL04'";
            }
            if (orderProductDivision == "1")
            {
                if (!string.IsNullOrEmpty(workFlowDivision))
                {
                    workFlowDivision = workFlowDivision + ", ";
                }
                workFlowDivision = workFlowDivision + "'PPL05'";
            }

            // ワークフロー詳細削除
            sql = "";
            sql = sql + "delete from workflow_detail ";                             // ワークフロー詳細
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and wf_no in ( ";
            sql = sql + "    select distinct ";
            sql = sql + "        wf_no ";                                           // ワークフローNo
            sql = sql + "    from ";
            sql = sql + "        workflow_header ";                                 // ワークフローヘッダ
            sql = sql + "    where ";
            sql = sql + "        1 = 1 ";
            sql = sql + "    and wf_division in ( @WorkflowDivision ) ";       // 呼出元区分
            sql = sql + ") ";
            // 更新処理実行
            ret = db.Regist(
                sql,
                new
                {
                    WorkflowDivision = workFlowDivision
                });
            if (ret < 0)
            {
                // 異常終了

                return false;
            }
            // ワークフローヘッダ削除
            sql = "";
            sql = sql + "delete from workflow_header ";                             // ワークフローヘッダ
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and wf_division in ( @WorkfloDivision ) ";           // 呼出元区分
            // 更新処理実行
            ret = db.Regist(
                sql,
                new
                {
                    WorkfloDivision = workFlowDivision
                });
            if (ret < 0)
            {
                // 異常終了
                return false;
            }
            return true;
        }

        /// <summary>
        /// 名称マスタ取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="nameDivision">名称区分</param>
        /// <param name="nameCd">名称コード</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>取得した名称</returns>
        public static string PakCommonGetNames(
            ComDB db,
            string nameDivision,
            string nameCd,
            PakDao.ComParamInfo paramInfo,
            ref List<string> errMessage)
        {
            string ret = "";

            // 名称マスタの検索
            try
            {
                string sql;         // 動的SQL
                sql = "";
                sql = sql + "select ";
                sql = sql + "    * ";
                sql = sql + "from ";
                sql = sql + "    names ";
                sql = sql + "where ";
                sql = sql + "    1 = 1 ";
                sql = sql + "and name_division = @NameDivision ";
                sql = sql + "and name_cd = @NameCd ";

                ComDao.NamesEntity result = db.GetEntityByDataClass<ComDao.NamesEntity>(
                    sql,
                    new
                    {
                        NameDivision = nameDivision,
                        NameCd = nameCd
                    });
                if (result == null)
                {
                    return ret;
                }
                ret = result.Name01;
                return ret;
            }
            catch (Exception ex)
            {
                // ログ出力:名称マスタ取得 エラー
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10073, ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                errMessage.Add(logMessage);
                logMessage = "";
                logger.Error(ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(ex.Message);
                errMessage.Add(ex.ToString());

                return ret;
            }
        }

        /// <summary>
        /// 発注区分取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="itemcd">品目コード</param>
        /// <param name="specfifcationcd">仕様コード</param>
        /// <returns>計画区分</returns>
        private static int? getPlanDivison(ComDB db, string itemcd, string specfifcationcd)
        {
            int? retValue = null;
            string sql = "";

            // 品目仕様から計画区分を取得
            sql = "";
            sql = sql + "select ";
            sql = sql + "    plan_division ";
            sql = sql + "from ";
            sql = sql + "    v_item_specification_active ";
            sql = sql + "where ";
            sql = sql + "    1 = 1 ";
            sql = sql + "and item_cd = @ItemCd ";
            sql = sql + "and specification_cd = @SpecificationCd ";
            sql = sql + "limit 1 ";

            int? plan_division = db.GetEntityByDataClass<int?>(
                sql,
                new
                {
                    ItemCd = itemcd,
                    SpecificationCd = specfifcationcd
                });

            if (plan_division != null)
            {
                if (plan_division == 3)
                {
                    retValue = 3; // 3:外注製品（非直送）
                }
                else
                {
                    retValue = 1; // 1:原材料
                }
            }
            return retValue;
        }

        /// <summary>
        /// 端数処理
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="mode">モード</param>
        /// <param name="unit">単位</param>
        /// <param name="paramInfo">パラメータ情報</param>
        /// <returns>処理後の値</returns>
        public static decimal PakCommonRounding(
            decimal data,
            int mode,
            int unit,
            PakDao.ComParamInfo paramInfo)
        {
            decimal ret;
            decimal ope;
            // 小数点位置チェック
            int digit = 0; // 桁数
            try
            {
                // CRUU;端数単位
                switch (unit)
                {
                    case 1:
                        digit = 0;
                        break;
                    case 2:
                        digit = 1;
                        break;
                    case 3:
                        digit = 2;
                        break;
                    case 4:
                        digit = 3;
                        break;
                    case 5:
                        digit = 4;
                        break;
                    case 6:
                        digit = 5;
                        break;
                    case 7:
                        digit = 6;
                        break;
                    case 8:
                        digit = 7;
                        break;
                    case 9:
                        digit = 0;
                        break;
                    default:
                        digit = 0;
                        break;
                }

                // CRUP:端数区分
                // 処理区分で処理を分岐
                //if (mode == 0)
                if (mode == 1)
                {
                    // 切捨
                    ope = decimal.Parse(Math.Pow(10, digit).ToString());
                    ret = Math.Floor(data * ope) / ope;
                }
                //else if (mode == 1)
                else if (mode == 2)
                {
                    // 四捨五入の場合
                    ret = Math.Round(data, digit, MidpointRounding.AwayFromZero);
                }
                else
                {
                    // 切上
                    ope = decimal.Parse(Math.Pow(10, digit).ToString());
                    ret = Math.Ceiling(data * ope) / ope;
                }
                return ret;
            }
            catch (Exception ex)
            {
                // ログ出力:端数処理 エラー
                logMessage = ComST.GetPropertiesMessage(new string[] { ComID.MB10058, ComID.MB10074, ComID.MB00022, "", "", "" }, languageId: paramInfo.LanguageId);
                logger.Error(logMessage);
                logMessage = "";
                logger.Error(ex.Message);
                logger.Error(ex.ToString());
                throw;
            }

        }

    }
}
