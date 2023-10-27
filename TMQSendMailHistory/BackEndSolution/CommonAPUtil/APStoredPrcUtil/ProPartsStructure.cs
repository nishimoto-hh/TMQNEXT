using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonSTDUtil.CommonLogger;

using APCheckDigitUtil = CommonAPUtil.APCheckDigitUtil.APCheckDigitUtil;
using ComConst = APConstants.APConstants;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDaoBatch = CommonAPUtil.APStoredPrcUtil.PakCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComID = CommonAPUtil.APCommonUtil.APResources.ID;
using ComPak = CommonAPUtil.APStoredPrcUtil.PakCommon;
using comST = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = CommonAPUtil.APStoredPrcUtil.ProPartsStructureDataClass;
using fncUtil = CommonAPUtil.APStoredFncUtil.APStoredFncUtil;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>
    /// 部品展開処理
    /// </summary>
    public class ProPartsStructure
    {
        /// <summary>
        /// ログ出力用インスタンス
        /// </summary>
        private static CommonLogger logger;

        /// <summary>
        /// 安全在庫確認
        /// </summary>
        private static int treeLevel = 0;

        /// <summary>リードタイム情報リスト </summary>
        private static List<Dao.LeadTimeInfo> leadTimeInfo;

        /// <summary>数値桁数チェックマスタ情報リスト</summary>
        private static List<CommonAPUtil.APCheckDigitUtil.APCheckDigitDataClass.NumberChkDigitDetail> chkDigitInfo;

        /// <summary>カレンダーマスタ：カレンダーコード </summary>
        private static string useCalCd = "ALL";

        #region コンストラクタ

        #endregion

        #region 品目MAXレベル更新処理

        /// <summary>
        /// 品目MAXレベル更新処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="transaction">トランザクション</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="errMessage">エラーリスト</param>
        /// <returns>true:OK、false:NG</returns>
        public static bool ExecMaxLevelSet(ComDB db, System.Data.IDbTransaction transaction,
            ComDaoBatch.ComParamInfo paramInfo, ref string errMessage)
        {
            //ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(paramInfo.PgmId);

            string sql = "";
            int regFlg = 0;
            bool retValue = true;
            string message = "";

            try
            {
                //品目仕様マスタのMAXレベルをセットする
                sql = "";
                sql = sql + "update item_specification ";
                sql = sql + "set ";
                sql = sql + "    update_date = now() ";
                sql = sql + "    , update_user_id = @UserId ";
                sql = sql + "    , low_level_cd = vps.max_level ";
                sql = sql + "from ";
                sql = sql + "    (";
                sql = sql + "        select ";
                sql = sql + "            item ";
                sql = sql + "	         , spec ";
                sql = sql + "	         , max(tree_level)  as max_level ";
                sql = sql + "        from ";
                sql = sql + "            v_parts_structure";
                sql = sql + "	     group by ";
                sql = sql + "            item";
                sql = sql + "	         , spec";
                sql = sql + "    ) vps ";
                sql = sql + "where ";
                sql = sql + "    item_specification.item_cd = vps.item ";
                sql = sql + "and item_specification.specification_cd = vps.spec ";

                regFlg = db.Regist(
                    sql,
                    new
                    {
                        UserId = paramInfo.UserId
                    });
                if (regFlg < 0)
                {
                    // ログ出力(MAXﾚﾍﾞﾙの更新に失敗しました。)
                    message = comST.GetPropertiesMessage(key: ComID.MB10012, languageId: paramInfo.LanguageId);
                    logger.Error(message);
                    errMessage = message;

                    return false;
                }

                retValue = true;
            }
            catch (Exception ex)
            {
                // ログ出力(MAXﾚﾍﾞﾙの更新に失敗しました。)
                message = comST.GetPropertiesMessage(key: ComID.MB10012, languageId: paramInfo.LanguageId);
                logger.Error(message + "【ExecMaxLevelSet】:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage = message + "【ExecMaxLevelSet】:" + ex.Message;
                retValue = false;
            }

            return retValue;

        }

        #endregion

        #region 部品展開処理

        /// <summary>
        /// 部品展開事前処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="transaction">トランザクション</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        /// <remarks>テーブルロック及びテーブル全件削除処理</remarks>
        public static bool ExecPrePartsStructure(ComDB db, System.Data.IDbTransaction transaction,
            ComDaoBatch.ComParamInfo paramInfo, ref List<string> errMessage)
        {

            //ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(paramInfo.PgmId);

            string sql = "";
            int cnt = 0;
            int cnt2 = 0;
            int regFlg = 0;
            string messageVal = "";

            try
            {
                //カレンダーコード取得
                sql = string.Empty;
                sql = sql + "select ";
                sql = sql + "    * ";
                sql = sql + "from ";
                sql = sql + "   names ";
                sql = sql + "where ";
                sql = sql + "    name_division = 'PCCD' ";
                sql = sql + "and name_cd = '1' ";

                ComDao.NamesEntity names = new ComDao.NamesEntity();
                names = db.GetEntityByDataClass<ComDao.NamesEntity>(sql);
                if (names == null)
                {
                    // ログ出力：
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB10026, "[Table:names name_division= 'PCCD',name_cd='1'" },
                        paramInfo.LanguageId);
                    logger.Error(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }
                useCalCd = names.Name01;

                //検索日時：カレンダーマスタ存在チェック
                sql = string.Empty;
                sql = sql + "select ";
                sql = sql + "    count(*)  as cnt ";
                sql = sql + "from ";
                sql = sql + "   cal ";
                sql = sql + "where ";
                sql = sql + "    cal_cd = @CalCd ";
                sql = sql + "and cal_date = current_date + cast('-1 months' as INTERVAL)";
                cnt = db.GetCount(
                    sql,
                    new
                    {
                        CalCd = useCalCd
                    });

                if (cnt == 0)
                {
                    // ログ出力：カレンダーマスタの作成量が十分ではありません。
                    messageVal = comST.GetPropertiesMessage(key: ComID.MB10037, languageId: paramInfo.LanguageId);
                    logger.Error(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }

                sql = string.Empty;
                sql = sql + "with max_cal as ( ";
                sql = sql + "select max(inout_date) as max_date ";
                sql = sql + "from inout_source_fixed) ";
                sql = sql + "select ";
                sql = sql + "    count(*)  as cnt ";
                sql = sql + "from ";
                sql = sql + "   cal ";
                sql = sql + "left join max_cal ";
                sql = sql + "on 1 = 1 ";
                sql = sql + "where ";
                sql = sql + "    cal_cd = @CalCd ";
                sql = sql + "and cal_date = to_date(to_char(max_cal.max_date, 'yyyy/MM/dd'), 'YYYY/MM/DD') + cast('1 months' as INTERVAL)";
                cnt = db.GetCount(
                    sql,
                    new
                    {
                        CalCd = useCalCd
                    });

                sql = string.Empty;
                sql = sql + "with max_cal as ( ";
                sql = sql + "select max(deliver_limit) as max_date ";
                sql = sql + "from production_plan ";
                sql = sql + "where status = 0 ) ";
                sql = sql + "select ";
                sql = sql + "    count(*)  as cnt ";
                sql = sql + "from ";
                sql = sql + "   cal ";
                sql = sql + "left join max_cal ";
                sql = sql + "on 1 = 1 ";
                sql = sql + "where ";
                sql = sql + "    cal_cd = @CalCd ";
                sql = sql + "and cal_date = to_date(to_char(max_cal.max_date, 'yyyy/MM/dd'), 'YYYY/MM/DD') + cast('1 months' as INTERVAL)";
                cnt2 = db.GetCount(
                    sql,
                    new
                    {
                        CalCd = useCalCd
                    });

                if (cnt == 0 && cnt2 == 0)
                {
                    // ログ出力：カレンダーマスタの作成量が十分ではありません。
                    messageVal = comST.GetPropertiesMessage(key: ComID.MB10037, languageId: paramInfo.LanguageId);
                    logger.Error(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }

                try
                {
                    // テーブルロック
                    sql = "";
                    sql = sql + "lock table mrp_result ";
                    sql = sql + "in share row exclusive mode nowait ";

                    // 更新処理実行
                    regFlg = db.Regist(sql);

                    // MRP結果を全削除
                    sql = "";
                    sql = sql + "delete from mrp_result; ";

                    // 更新処理実行
                    regFlg = db.Regist(sql);
                    if (regFlg < 0)
                    {
                        // ログ出力：テーブルロックまたは全削除 失敗
                        messageVal = comST.GetPropertiesMessage(key: ComID.MB10013, languageId: paramInfo.LanguageId);
                        logger.Error(messageVal);
                        errMessage.Add(messageVal);
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    // ログ出力：テーブルロックまたは全削除 失敗
                    messageVal = comST.GetPropertiesMessage(key: ComID.MB10013, languageId: paramInfo.LanguageId);
                    logger.Error(messageVal + "：" + ex.Message);
                    logger.Error(ex.ToString());
                    errMessage.Add(messageVal + "：" + ex.Message);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {

                //ログメッセージ：部品展開処理　異常終了
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10003 }, paramInfo.LanguageId);
                logger.Error(messageVal + "：[ExecPrePartsStructure]" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "：[ExecPrePartsStructure]" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 部品展開処理（メイン）
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="transaction">トランザクション</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <param name="baseMessage">画面表示メッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        public static bool ExecPartsStructure(ComDB db, System.Data.IDbTransaction transaction,
            ComDaoBatch.ComParamInfo paramInfo, ref List<string> errMessage, ref string baseMessage)
        {
            //ログ出力用インスタンス生成
            logger = CommonLogger.GetInstance(paramInfo.PgmId);

            string sql = "";
            int regFlg = 0;
            bool ret = true;
            string messageVal = "";

            try
            {

                //カレンダーコード取得
                sql = string.Empty;
                sql = sql + "select ";
                sql = sql + "    * ";
                sql = sql + "from ";
                sql = sql + "   names ";
                sql = sql + "where ";
                sql = sql + "    name_division = 'PCCD' ";
                sql = sql + "and name_cd = '1' ";

                ComDao.NamesEntity names = db.GetEntityByDataClass<ComDao.NamesEntity>(sql);
                if (names == null)
                {
                    // ログ出力：
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB10026, "[Table:names name_division= 'PCCD',name_cd='1'" },
                        paramInfo.LanguageId);
                    logger.Error(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }
                useCalCd = names.Name01;

                try
                {
                    // テーブルロック
                    sql = "";
                    sql = sql + "lock table mrp_result ";
                    sql = sql + "in share row exclusive mode nowait ";

                    // 更新処理実行
                    regFlg = db.Regist(sql);
                }
                catch (Exception ex)
                {
                    // ログ出力：テーブルロックまたは全削除 失敗
                    messageVal = comST.GetPropertiesMessage(key: ComID.MB10013, languageId: paramInfo.LanguageId);
                    logger.Error(messageVal + "：" + ex.Message);
                    logger.Error(ex.ToString());
                    errMessage.Add(messageVal + "：" + ex.Message);
                    return false;
                }

                //実行パラメータ情報取得
                sql = string.Empty;
                sql = sql + "select ";
                sql = sql + "    extstr1 as DeploymentDateFrom ";
                sql = sql + "    , extstr2 as DeploymentDateTo ";
                sql = sql + "    , extkbn1 as LeadTimeType ";
                sql = sql + "    , extkbn2 as SafteyStockType  ";
                sql = sql + "from ";
                sql = sql + "   com_bat_sch ";
                sql = sql + "where ";
                sql = sql + "    ip_address = @IpAddress ";
                sql = sql + "and conductid = @Conductid ";
                sql = sql + "and pgmid = @Pgmid ";
                sql = sql + "and formno = @Formno ";
                sql = sql + "and exec_date = @ExecDate ";
                sql = sql + "and exec_time = @ExecTime ";
                sql = sql + "and status = " + "4" + " "; // ステータス 4
                sql = sql + "and delflg =  @Delflg ";
                sql = sql + "order by status desc ";
                sql = sql + "limit 1 ";

                Dao.FormCodition condition = new Dao.FormCodition();
                condition = db.GetEntityByDataClass<Dao.FormCodition>(
                    sql,
                    new
                    {
                        IpAddress = paramInfo.TerminalNo,
                        Conductid = paramInfo.ConductId,
                        Pgmid = paramInfo.ConductId,
                        Formno = int.Parse(paramInfo.FormNo),
                        ExecDate = paramInfo.ExecDate,
                        ExecTime = paramInfo.ExecTime,
                        Delflg = ComConst.DEL_FLG.OFF,
                    });
                if (condition == null)
                {
                    // ログ出力：画面条件取得に失敗しました。
                    messageVal = comST.GetPropertiesMessage(key: ComID.MB00018, languageId: paramInfo.LanguageId);
                    logger.Error(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }

                //品目毎リードタイムリスト情報
                leadTimeInfo = new List<Dao.LeadTimeInfo>();

                //数値桁数マスタリスト情報
                chkDigitInfo = new List<CommonAPUtil.APCheckDigitUtil.APCheckDigitDataClass.NumberChkDigitDetail>();

                //品目在庫リスト(品目コード<key1,品目仕様<key2,有効在庫数>>)
                var itemInvList = new List<Dao.TempItemInventory>();

                // バックオーダー取込処理
                ret = backOrderCapture(db, paramInfo, ref itemInvList, ref errMessage, ref baseMessage);

                if (!ret)
                {
                    //ログメッセージ：バックオーダー取込処理 異常終了
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10005}, paramInfo.LanguageId);
                    logger.Error(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }

                //親品目展開処理（生産計画情報よりMRP結果情報生成)
                treeLevel = 0;
                ret = parentDeployment(db, paramInfo, condition, ref itemInvList, ref errMessage, ref baseMessage);

                if (!ret)
                {

                    //ログメッセージ：親部品展開　異常終了
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10006 }, paramInfo.LanguageId);
                    logger.Error(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }

                //子品目展開処理(品目仕様を元にMRP結果情報生成)
                ret = childDeployment(db, paramInfo, condition, ref itemInvList, ref errMessage, ref baseMessage);

                if (!ret)
                {
                    //ログメッセージ：子部品展開　異常終了
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10007 }, paramInfo.LanguageId);
                    logger.Error(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }

                // 発注点処理(発注基準:発注点を対象にMRP結果情報生成)
                //ログメッセージ：発注点発注　開始
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00020, ComID.MB10285 }, paramInfo.LanguageId);
                logger.Info(messageVal);
                errMessage.Add(messageVal);

                ret = orderingPoint(db, paramInfo, ref errMessage);
                if (!ret)
                {
                    //ログメッセージ：発注点発注　異常終了
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10285 }, paramInfo.LanguageId);
                    logger.Error(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }
                else
                {
                    //ログメッセージ：発注点発注　終了
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00021, ComID.MB10285 }, paramInfo.LanguageId);
                    logger.Info(messageVal);
                    errMessage.Add(messageVal);
                }

                return true;
            }
            catch (Exception ex)
            {
                //ログメッセージ：部品展開処理　異常終了
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10003 }, paramInfo.LanguageId);
                logger.Error(messageVal + "：[ExecPartsStructure]" +  ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "：[ExecPartsStructure]" + ex.Message);
                return false;
            }

        }

        /// <summary>
        /// バックオーダ取り込み
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="paramItemInvList">品目在庫リスト</param>
        /// <param name="errMessage">エラーリスト</param>
        /// <param name="baseMessage">画面表示メッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool backOrderCapture(ComDB db, ComDaoBatch.ComParamInfo paramInfo,
            ref List<Dao.TempItemInventory> paramItemInvList, ref List<string> errMessage, ref string baseMessage)
        {

            string sql;
            string message = "";
            string getValue = "";
            bool retVal;

            try
            {

                sql = "";
                sql = sql + "select  ";
                sql = sql + "    inouts.inout_division ";
                sql = sql + "    , inouts.inout_date ";
                sql = sql + "    , inouts.order_division ";
                sql = sql + "    , inouts.order_no ";
                sql = sql + "    , inouts.item_cd ";
                sql = sql + "    , inouts.specification_cd ";
                sql = sql + "    , inouts.location_cd ";
                sql = sql + "    , inouts.inout_qty ";
                sql = sql + "    , inouts.reference_no ";
                sql = sql + "    , inouts.vender_cd ";
                sql = sql + "    , inouts.parent_item_cd ";
                sql = sql + "    , inouts.parent_specification_cd ";
                sql = sql + "    , current_date as sysdate ";
                sql = sql + "from  ";
                sql = sql + "    inout_source_fixed inouts  ";
                sql = sql + "where ";
                sql = sql + "    inout_division = any (array [";
                sql = sql + "     @OrderNomal ";
                sql = sql + "    , @Order ";
                sql = sql + "    , @SaleDeliveryDate ";
                sql = sql + "    , @Shipping ";
                sql = sql + "    , @PurchaseRequest ";
                sql = sql + "    , @OrderRegist ";
                sql = sql + "    , @PurchaseDeliveryDate ";
                sql = sql + "    , @PurchaseAccept ";
                sql = sql + "    , @DirectionApprovalParts ";
                sql = sql + "    , @DirectionApprovalFinish ";
                sql = sql + "    ]";
                sql = sql + "    )";

                // 動的SQL実行
                IList<Dao.TempBackOrder> results = db.GetListByDataClass<Dao.TempBackOrder>(
                    sql,
                    new
                    {
                        OrderNomal = ComConst.INOUT_DIVISION.ORDER_NORMAL,
                        Order = ComConst.INOUT_DIVISION.ORDER,
                        SaleDeliveryDate = ComConst.INOUT_DIVISION.SALE_DELIVERY_DATE,
                        Shipping = ComConst.INOUT_DIVISION.SHIPPING,
                        PurchaseRequest = ComConst.INOUT_DIVISION.PURCHASE_REQUEST,
                        OrderRegist = ComConst.INOUT_DIVISION.ORDER_REGIST,
                        PurchaseDeliveryDate = ComConst.INOUT_DIVISION.PURCHASE_DELIVERY_DATE,
                        PurchaseAccept = ComConst.INOUT_DIVISION.PURCHASE_ACCEPT,
                        DirectionApprovalParts = ComConst.INOUT_DIVISION.DIRECTION_APPROVAL_PARTS,
                        DirectionApprovalFinish = ComConst.INOUT_DIVISION.DIRECTION_APPROVAL_FINISH
                    });

                if (results.Count <= 0)
                {
                    // ログメッセージ：対象情報がありません
                    message = comST.GetPropertiesMessage(new string[] { ComID.MB00017, ComID.MB10010 }, paramInfo.LanguageId);
                    logger.Info(message);
                    errMessage.Add(message);
                    baseMessage = comST.GetPropertiesMessage(ComID.MB10277, paramInfo.LanguageId);
                    return true;
                }

                //MRP結果登録
                ComDao.MrpResultEntity temp = new ComDao.MrpResultEntity();

                foreach (var result in results)
                {
                    temp.PlanNo = string.Empty;
                    temp.ProcedureDivision = 0;
                    temp.ItemCd = result.ItemCd;
                    temp.SpecificationCd = result.SpecificationCd;
                    temp.DeliveryDate = result.InoutDate;
                    temp.PlanQty = 0;
                    temp.ProductionPlanNo = string.Empty;
                    temp.Deliverlimit = result.InoutDate;
                    temp.VenderCd = string.Empty;
                    temp.LocationCd = string.Empty;
                    temp.OrderDate = result.InoutDate;
                    temp.ReferenceNo = result.ReferenceNo;
                    temp.Status = 1;
                    temp.AllocatedQty = 0;
                    temp.MarumeDivision = 0;
                    temp.OrderDivision = 0;
                    temp.OrderNo = string.Empty;
                    temp.OrderRule = 0;
                    temp.BreakdownLevel = 0;
                    temp.DeliveryDateBefore = null;
                    temp.PlanQtyBefore = 0;
                    temp.ParentOrderDivision = 0;
                    temp.ParentOrderNo = string.Empty;
                    temp.ParentItemCd = string.Empty;
                    temp.ParentSpecificationCd = string.Empty;

                    if (result.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_APPROVAL_FINISH)
                    {
                        //--- 製造オーダ ---("MAN0010-02")
                        //計画数量
                        temp.PlanQty = Math.Abs(result.InoutQty);
                        //オーダ発行区分
                        temp.OrderPublishDivision = ComConst.ORDER_PUBLISH_DIVISION.WAREHOUSING;
                        //備考・在庫計算対象区分
                        if (((DateTime)result.InoutDate).Date < result.SystemDate.Date)
                        {
                            // 備考：納期　督促
                            getValue = comST.GetPropertiesMessage(key: ComID.CB10001, languageId: paramInfo.LanguageId);
                            //在庫計算対象区分
                            temp.InventoryCalDivision = 0;
                        }
                        else
                        {
                            // 備考：入庫予定
                            getValue = comST.GetPropertiesMessage(key: ComID.CB10002, languageId: paramInfo.LanguageId);
                            //在庫計算対象区分
                            temp.InventoryCalDivision = 1;
                        }
                        //納入ロケーションコード
                        temp.LocationCd = result.LocationCd;
                        //オーダー区分
                        temp.OrderDivision = result.OrderDivision;

                        //オーダー番号
                        temp.OrderNo = result.OrderNo;

                    }
                    else if (result.InoutDivision == ComConst.INOUT_DIVISION.PURCHASE_REQUEST
                        || result.InoutDivision == ComConst.INOUT_DIVISION.ORDER_REGIST
                        || result.InoutDivision == ComConst.INOUT_DIVISION.PURCHASE_DELIVERY_DATE
                        || result.InoutDivision == ComConst.INOUT_DIVISION.PURCHASE_ACCEPT)
                    {
                        //--- 購入・外注オーダ ---("ORD0010-01","ORD0020-01","ORD0030-01","ACC0010-01")
                        //計画数量
                        temp.PlanQty = Math.Abs(result.InoutQty);
                        //オーダ発行区分
                        temp.OrderPublishDivision = ComConst.ORDER_PUBLISH_DIVISION.WAREHOUSING;
                        //オーダー先コード
                        temp.VenderCd = result.VenderCd;
                        //備考・在庫計算対象区分
                        if (((DateTime)result.InoutDate).Date < result.SystemDate.Date)
                        {
                            // 備考：納期　督促
                            getValue = comST.GetPropertiesMessage(key: ComID.CB10001, languageId: paramInfo.LanguageId);
                            temp.Remark = getValue;
                            //在庫計算対象区分
                            temp.InventoryCalDivision = 0;
                        }
                        else
                        {
                            // 備考：入庫予定
                            getValue = comST.GetPropertiesMessage(key: ComID.CB10002, languageId: paramInfo.LanguageId);
                            temp.Remark = getValue;
                            //在庫計算対象区分
                            temp.InventoryCalDivision = 1;
                        }
                        //納入ロケーションコード
                        temp.LocationCd = result.LocationCd;
                        // オーダ区分
                        temp.OrderDivision = result.OrderDivision;
                        //オーダー番号
                        temp.OrderNo = result.OrderNo;
                    }
                    else if (result.InoutDivision == ComConst.INOUT_DIVISION.DIRECTION_APPROVAL_PARTS)
                    {
                        //--- 出庫予定 ---("MAN0010-01")
                        //オーダ発行区分
                        temp.OrderPublishDivision = ComConst.ORDER_PUBLISH_DIVISION.SHIPPING;
                        // 備考：引当
                        getValue = comST.GetPropertiesMessage(key: ComID.CB10003, languageId: paramInfo.LanguageId);
                        temp.Remark = getValue;
                        //在庫計算対象区分
                        if (((DateTime)result.InoutDate).Date < result.SystemDate.Date)
                        {
                            temp.InventoryCalDivision = 0;
                        }
                        else
                        {
                            temp.InventoryCalDivision = 1;
                        }
                        //引当数量
                        temp.AllocatedQty = Math.Abs(result.InoutQty);
                        //親オーダー区分
                        temp.ParentOrderDivision = result.OrderDivision;
                        //親オーダー番号
                        temp.ParentOrderNo = result.OrderNo;
                        //親品目コード
                        //temp.ParentItemCd = result.SpecificationCd;
                        temp.ParentItemCd = result.ParentItemCd;
                        //親仕様コード
                        temp.ParentSpecificationCd = result.ParentSpecificationCd;
                    }
                    else if (result.InoutDivision == ComConst.INOUT_DIVISION.SALE_DELIVERY_DATE
                        || result.InoutDivision == ComConst.INOUT_DIVISION.ORDER
                        || result.InoutDivision == ComConst.INOUT_DIVISION.ORDER_NORMAL
                        || result.InoutDivision == ComConst.INOUT_DIVISION.SHIPPING)
                    {
                        //--- 受注 ---("ROR0010-01","ROR0010-02","ROR0020-01","SHP0010-01")
                        //オーダ発行区分
                        temp.OrderPublishDivision = ComConst.ORDER_PUBLISH_DIVISION.SHIPPING_NO_DEPLOYMENT;

                        // 備考：受注
                        getValue = comST.GetPropertiesMessage(key: ComID.CB10004, languageId: paramInfo.LanguageId);
                        temp.Remark = getValue;
                        //在庫計算対象区分
                        if (((DateTime)result.InoutDate).Date < result.SystemDate.Date)
                        {
                            temp.InventoryCalDivision = 0;
                        }
                        else
                        {
                            temp.InventoryCalDivision = 1;
                        }
                        //引当数量
                        temp.AllocatedQty = Math.Abs(result.InoutQty);
                        //オーダー区分
                        temp.OrderDivision = result.OrderDivision;
                        //オーダー番号
                        temp.OrderNo = result.OrderNo;
                    }
                    else
                    {
                        //--- 対象外 ---
                        continue;
                    }

                    // データが無かった場合は、INSERT
                    retVal = registMrpReult(db, temp, paramInfo, ref errMessage);
                    if (!retVal)
                    {
                        //MRP結果登録失敗
                        return false;
                    }

                    //==== 品目在庫情報登録 ====
                    int itemCnt = paramItemInvList.Count(s => s.ItemCd == result.ItemCd && s.SpecificationCd == result.SpecificationCd);
                    if (itemCnt == 0)
                    {
                        decimal inventoryQty = 0;
                        retVal = getItemInventory(db, result.ItemCd, result.SpecificationCd, paramInfo, ref paramItemInvList, ref inventoryQty, ref errMessage);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //ログメッセージ：バックオーダ取込処理　異常終了
                message = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10005 }, paramInfo.LanguageId);
                logger.Error(message + "：[BackOrderCapture]" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(message + "：[BackOrderCapture]" + ex.Message);
                return false;
            }
        }

        #region 親品目展開処理
        /// <summary>
        /// 親品目展開処理
        /// （生産計画情報よりMRP結果情報生成)
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="paramCondition">画面情報</param>
        /// <param name="paramItemInvList">品目在庫リスト</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <param name="baseMessage">画面表示メッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool parentDeployment(ComDB db, ComDaoBatch.ComParamInfo paramInfo,
            Dao.FormCodition paramCondition, ref List<Dao.TempItemInventory> paramItemInvList,
            ref List<string> errorMessage, ref string baseMessage)
        {
            string sql;
            bool retVal = true;
            string errMsgId = string.Empty;
            string messageVal = string.Empty;
            int procDivision;
            int leadTime = 0;
            DateTime deliveryLimit;
            string remark;

            try
            {
                //生産計画情報取得
                sql = string.Empty;

                sql = sql + "with info as ( ";

                sql = sql + "select ";
                sql = sql + "   * ";
                sql = sql + "from ";
                sql = sql + "   production_plan ";
                sql = sql + "where ";
                sql = sql + "    status = 0 ";
                if (!string.IsNullOrEmpty(paramCondition.DeploymentDateFrom))
                {
                    sql = sql + "and deliver_limit >= TO_DATE(@DeploymentDateFrom,'YYYY/MM/DD') ";
                }
                if (!string.IsNullOrEmpty(paramCondition.DeploymentDateTo))
                {
                    // 23:59:59
                    sql = sql + "and deliver_limit <= to_timestamp(@DeploymentDateTo ,'YYYY/MM/DD HH24:MI:SS') ";
                }
                sql = sql + "order by ";
                sql = sql + "  item_cd, ";
                sql = sql + "  specification_cd, ";
                sql = sql + "  deliver_limit ";

                sql = sql + ") ";

                //品目情報取得
                sql = sql + "select ";
                sql = sql + "   info.*, ";
                sql = sql + "   spec.active_date, ";
                sql = sql + "   spec.default_location, ";
                sql = sql + "   spec.plan_division, ";
                sql = sql + "   spec.unit_of_operation_management, ";
                sql = sql + "   spec.specification_active_date, ";
                sql = sql + "   spec.stock_division, ";
                sql = sql + "   purc.purchase_lead_time, ";
                sql = sql + "   purc.purchase_safety_lead_time, ";
                sql = sql + "   purc.purchase_trigger, ";
                sql = sql + "   rhead.standard_rate_perday, ";
                sql = sql + "   rhead.production_lead_time, ";
                sql = sql + "   rhead.product_safety_lead_time, ";
                sql = sql + "   rhead.min_qty, ";
                sql = sql + "   rhead.unit_qty, ";
                sql = sql + "   prod.production_plan, ";
                sql = sql + "   current_date as sysdate ";
                sql = sql + "from ";
                sql = sql + "  ((( ";
                sql = sql + "     info ";
                sql = sql + "  left join ( ";
                sql = sql + "      select item.item_cd, max(item.active_date) as active_date ";
                sql = sql + "      from info ";
                sql = sql + "      left join ";
                sql = sql + "           (select ";
                sql = sql + "                item_cd, active_date ";
                sql = sql + "             from ";
                sql = sql + "               item ";
                sql = sql + "             where ";
                sql = sql + "               activate_flg = @ActivateFlg ";
                sql = sql + "               and item.del_flg = @DelFlg ";
                sql = sql + "            ) item ";
                sql = sql + "      on item.active_date <= to_date(to_char(info.deliver_limit, 'yyyy/MM/dd'), 'YYYY/MM/DD') ";
                sql = sql + "      and item.item_cd = info.item_cd ";
                sql = sql + "      group by item.item_cd ";
                sql = sql + "  ) item";
                sql = sql + "  on info.item_cd = item.item_cd ";
                sql = sql + "  left join ( ";
                sql = sql + "      select spec.item_cd, spec.default_location, spec.plan_division, spec.unit_of_operation_management, spec.specification_cd, spec.active_date, spec.specification_active_date, spec.stock_division ";
                sql = sql + "      from item_specification spec ";
                sql = sql + "      inner join info ";
                sql = sql + "      on spec.item_cd = info.item_cd ";
                sql = sql + "      and spec.specification_cd = info.specification_cd ";
                sql = sql + "      and spec.specification_active_date <= to_date(to_char(info.deliver_limit, 'yyyy/MM/dd'), 'YYYY/MM/DD') ";
                sql = sql + "      and spec.activate_flg = @ActivateFlg ";
                sql = sql + "      and spec.del_flg = @DelFlg ";
                sql = sql + "      group by spec.item_cd, spec.default_location, spec.plan_division, spec.unit_of_operation_management, spec.specification_cd, spec.active_date, spec.specification_active_date, spec.stock_division";
                sql = sql + "      ) spec ";
                sql = sql + "  on info.item_cd = spec.item_cd ";
                sql = sql + "  and info.specification_cd = spec.specification_cd ";
                sql = sql + "  and item.active_date = spec.active_date ";
                sql = sql + "  left outer join  ";
                sql = sql + "        item_purchase_attribute purc ";
                sql = sql + "    on ( ";
                sql = sql + "        spec.item_cd = purc.item_cd ";
                sql = sql + "    and spec.specification_cd = purc.specification_cd ";
                sql = sql + "    and spec.active_date = purc.active_date ";
                sql = sql + "       )) ";
                sql = sql + "  left outer join  ";
                sql = sql + "        item_product_attribute prod ";
                sql = sql + "    on ( ";
                sql = sql + "        spec.item_cd = prod.item_cd ";
                sql = sql + "    and spec.specification_cd = prod.specification_cd ";
                sql = sql + "    and spec.active_date = prod.active_date ";
                sql = sql + "       )) ";
                sql = sql + "  left outer join ";
                sql = sql + "        (select ";
                sql = sql + "            recipe_header.item_cd, ";
                sql = sql + "            recipe_header.specification_cd, ";
                sql = sql + "            recipe_header.standard_rate_perday, ";
                sql = sql + "            recipe_header.production_lead_time, ";
                sql = sql + "            recipe_header.product_safety_lead_time, ";
                sql = sql + "            recipe_header.unit_qty, ";
                sql = sql + "            recipe_header.min_qty, ";
                sql = sql + "            row_number() OVER (PARTITION BY recipe_header.item_cd, recipe_header.specification_cd ORDER BY recipe_header.recipe_priority DESC) AS num ";
                sql = sql + "         from ";
                sql = sql + "           recipe_header ";
                sql = sql + "         inner join";
                sql = sql + "           info";
                sql = sql + "           on recipe_header.start_date <= to_date(to_char(info.deliver_limit, 'yyyy/MM/dd'), 'YYYY/MM/DD') ";
                sql = sql + "           and recipe_header.end_date >= to_date(to_char(info.deliver_limit, 'yyyy/MM/dd'), 'YYYY/MM/DD') ";
                sql = sql + "           and recipe_header.item_cd = info.item_cd ";
                sql = sql + "           and recipe_header.specification_cd = info.specification_cd ";
                sql = sql + "         where ";
                sql = sql + "               recipe_header.activate_flg = @ActivateFlg ";
                sql = sql + "               and recipe_header.del_flg = @DelFlg ";
                sql = sql + "         order by recipe_priority desc ";
                sql = sql + "        ) rhead ";
                sql = sql + "    on ( ";
                sql = sql + "        spec.item_cd = rhead.item_cd ";
                // sql = sql + "    and spec.active_date = item.active_date ";
                sql = sql + "        and spec.specification_cd = rhead.specification_cd ";
                sql = sql + "        and rhead.num = 1 ";
                sql = sql + "       )) ";
                sql = sql + " order by info.item_cd, info.specification_cd, info.deliver_limit ";

                //動的sql
                IList<Dao.PartsItemInfo> results = db.GetListByDataClass<Dao.PartsItemInfo>(
                    sql,
                    new
                    {
                        DeploymentDateFrom = paramCondition.DeploymentDateFrom,
                        DeploymentDateTo = paramCondition.DeploymentDateTo + " " + ComConst.COMMON.HourTime,
                        ActivateFlg = ComConst.AVTIVE_FLG.APPROVAL,
                        DelFlg = ComConst.DEL_FLG.OFF,

                    });

                if (results.Count == 0)
                {
                    // ログメッセージ：生産計画　対象情報がありません
                    messageVal = comST.GetPropertiesMessage(new string[] {ComID.MB00017, ComID.MB10008 }, paramInfo.LanguageId);
                    logger.Info(messageVal);
                    errorMessage.Add(messageVal);
                    baseMessage = comST.GetPropertiesMessage(ComID.MB10277, paramInfo.LanguageId);
                    return true;
                }

                //品目仕様マスタ・品目マスタ_購入品扱い属性・BOM&処方マスタ_ヘッダ情報
                //1レベル展開情報
                List<ComDaoBatch.TempPartsStructure> resultList = new List<ComDaoBatch.TempPartsStructure>();
                //MRP結果登録
                ComDao.MrpResultEntity temp = new ComDao.MrpResultEntity();
                string backItemcd = string.Empty;
                string backSpeccd = string.Empty;

                foreach (var result in results)
                {
                    //有効チェック
                    if (result.SpecificationActiveDate == null)
                    {
                        // ログメッセージ：有効な品目仕様が存在しません。
                        messageVal = "品目[" + result.ItemCd + "] 仕様[" + result.SpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10270, paramInfo.LanguageId);
                        logger.Info(messageVal);
                        errorMessage.Add(messageVal);
                        baseMessage = comST.GetPropertiesMessage(ComID.MB10277, paramInfo.LanguageId);
                        continue;
                    }

                    // 研究用BOMチェック
                    if (result.PlanDivision == ComConst.PLAN_DIVISION.PRODUCTION)
                    {
                        if (!checkBomStatus(result.DeliverLimit, result.ItemCd, result.SpecificationCd, db))
                        {
                            // ログメッセージ：製造品に紐付くBomが存在しないか不備があります。
                            messageVal = "品目[" + result.ItemCd + "] 仕様[" + result.SpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10282, paramInfo.LanguageId);
                            logger.Info(messageVal);
                            errorMessage.Add(messageVal);
                            continue;
                        }
                    }

                    //日付チェック
                    deliveryLimit = result.DeliverLimit;
                    if (result.DeliverLimit < result.SysDate)
                    {
                        // ログメッセージ：計画納期が過去日付になっています。
                        messageVal = "品目[" + result.ItemCd + "] 仕様[" + result.SpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10272, paramInfo.LanguageId);
                        logger.Info(messageVal + result.ItemCd + "," + result.SpecificationCd);
                        errorMessage.Add(messageVal + result.ItemCd + "," + result.SpecificationCd);
                        //システム日付を納期とする
                        deliveryLimit = result.SysDate;
                    }

                    //計画発注数チェック
                    if (result.ProductionPlanQty > 1000000000)
                    {
                        // ログメッセージ：計画発注数量が十億を超えました。
                        messageVal = "品目[" + result.ItemCd + "] 仕様[" + result.SpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10274, paramInfo.LanguageId);
                        logger.Info(messageVal + result.ItemCd + "," + result.SpecificationCd);
                        errorMessage.Add(messageVal + result.ItemCd + "," + result.SpecificationCd);
                        // ログメッセージ：当初発注数が十億を超えました。
                        messageVal = "品目[" + result.ItemCd + "] 仕様[" + result.SpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10273, paramInfo.LanguageId);
                        logger.Info(messageVal + result.ItemCd + "," + result.SpecificationCd);
                        errorMessage.Add(messageVal + result.ItemCd + "," + result.SpecificationCd);
                    }

                    //品目在庫情報取得
                    decimal inventoryQty = 0;
                    retVal = getItemInventory(db, result.ItemCd, result.SpecificationCd,
                        paramInfo, ref paramItemInvList, ref inventoryQty, ref errorMessage);
                    if (!retVal)
                    {
                        break;
                    }

                    int safetyLeadTime = 0;

                    //手続き区分/LeadTime
                    if (result.PlanDivision == ComConst.PLAN_DIVISION.PRODUCTION)
                    {
                        procDivision = ComConst.MRP_PROCEDURE_DIVISION.PRODUCTION;
                        //製造品の場合：ＢＯＮヘッダの製造リードタイム＋安全リードタイム
                        leadTime = (result.ProductionLeadTime ?? 0) + (result.ProductSafetyLeadTime ?? 0);
                        safetyLeadTime = result.ProductSafetyLeadTime ?? 0;
                    }
                    else
                    {
                        procDivision = ComConst.MRP_PROCEDURE_DIVISION.PURCHASE;
                        //購買の場合：品目マスタ_購入品扱い属性の販売リードタイム＋安全リードタイム
                        leadTime = (result.PurchaseLeadTime ?? 0) + (result.PurchaseSafetyLeadTime ?? 0);
                        safetyLeadTime = result.PurchaseSafetyLeadTime ?? 0;
                    }

                    //リードタイム取得
                    int workLeadTime = getLeadTime(paramInfo, procDivision, result.ProductionPlanQty,
                        result.StandardRatePerday ?? 0, paramCondition.LeadTimeType, leadTime, safetyLeadTime);

                    //計画発注日取得
                    DateTime planDate = result.DeliverLimit;
                    //retVal = CalcPlanDate(db, result.DeliverLimit, leadTime, pPramInfo.LanguageId, ref planDate);
                    retVal = calcPlanDate(db, result.DeliverLimit, workLeadTime, paramInfo.LanguageId, ref planDate, ref errorMessage);
                    if (!retVal)
                    {
                        break;
                    }

                    //備考編集
                    // 文言："生産計画"
                    string strVal = comST.GetPropertiesMessage(ComID.CB10005, paramInfo.LanguageId);
                    remark = strVal;
                    if (planDate < result.SysDate)
                    {
                        // 文言："緊急手配"
                        planDate = result.SysDate;
                        strVal = comST.GetPropertiesMessage(ComID.CB10009, paramInfo.LanguageId);
                        remark = remark + strVal;
                    }
                    else
                    {
                        // 文言："通常手配"
                        strVal = comST.GetPropertiesMessage(ComID.CB10010, paramInfo.LanguageId);
                        remark = remark + strVal;
                    }

                    //生産量<最小生産量の場合　製造数を最小生産量にする
                    decimal planQty = result.ProductionPlanQty;
                    if (result.ProductionPlanQty < result.MinQty)
                    {
                        planQty = result.MinQty ?? result.ProductionPlanQty;
                    }

                    //生産量を単位生産量 の引き上げ
                    if (result.UnitQty.GetValueOrDefault() != 0 && result.UnitQty != null)
                    {
                        if (result.ProductionPlanQty % result.UnitQty != 0)
                        {
                            planQty = result.UnitQty.GetValueOrDefault() * (Math.Truncate(result.ProductionPlanQty / result.UnitQty.GetValueOrDefault()) + 1);
                        }
                    }

                    //仕入先コード
                    string venderCd = string.Empty;
                    if (result.PlanDivision != ComConst.PLAN_DIVISION.PRODUCTION)
                    {
                        venderCd = result.VenderCd;
                        if (string.IsNullOrWhiteSpace(venderCd))
                        {
                            venderCd = result.VenderCd;
                        }
                    }

                    //MRP結果登録（親品目情報）
                    temp.PlanNo = string.Empty;
                    temp.ProcedureDivision = procDivision;
                    temp.ItemCd = result.ItemCd;
                    temp.SpecificationCd = result.SpecificationCd;
                    temp.DeliveryDate = deliveryLimit;
                    temp.PlanQty = planQty;
                    temp.OrderPublishDivision = ComConst.ORDER_PUBLISH_DIVISION.PRODUCTION; //生産計画
                    temp.ProductionPlanNo = result.ProductionPlanNo;
                    temp.Deliverlimit = result.DeliverLimit;
                    temp.Remark = remark;
                    temp.VenderCd = venderCd;
                    temp.LocationCd = result.DefaultLocation;
                    temp.OrderDate = planDate;
                    temp.ReferenceNo = result.ReferenceNo;
                    temp.Status = 0;
                    temp.AllocatedQty = 0;
                    temp.MarumeDivision = 0;
                    temp.OrderDivision = 0;
                    temp.OrderNo = string.Empty;
                    temp.OrderRule = result.PurchaseTrigger;
                    temp.InventoryCalDivision = 1;
                    temp.BreakdownLevel = 0;
                    temp.DeliveryDateBefore = result.DeliverLimit;
                    temp.PlanQtyBefore = result.ProductionPlanQty;
                    temp.ParentOrderDivision = 0;
                    temp.ParentOrderNo = string.Empty;
                    temp.ParentItemCd = string.Empty;
                    temp.ParentSpecificationCd = string.Empty;

                    //登録
                    retVal = registMrpReult(db, temp, paramInfo, ref errorMessage);
                    if (!retVal)
                    {
                        break;
                    }

                    // 1レベル展開実施有無判定
                    if (result.PlanDivision == ComConst.PLAN_DIVISION.PRODUCTION)
                    {
                        //製品：生産計画区分＝２の場合  展開対象外
                        if (result.ProductionPlan == ComConst.PRODUCTION_PLAN.MANUAL)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        //購入品：発注基準＝個別発注の場合　展開対象外
                        if (result.PurchaseTrigger == ComConst.PURCHASE_TRIGGER.INDIVIDUAL)
                        {
                            continue;
                        }
                    }

                    if ((backItemcd != result.ItemCd) || (backSpeccd != result.SpecificationCd))
                    {
                        //1レベル展開処理
                        resultList.Clear();
                        resultList = new List<ComDaoBatch.TempPartsStructure>();
                        DateTime paramStartDate = DateTime.ParseExact("0001/01/01", "yyyy/MM/dd", null);
                        DateTime paramEndDate = DateTime.ParseExact("9999/12/31", "yyyy/MM/dd", null);
                        int paramLeadTime = 0;
                        bool paramDelFlg = true;

                        retVal = ComPak.Exe1LevelPartsStructure(db, paramInfo,
                            result.ItemCd, result.SpecificationCd,
                            result.DeliverLimit, paramStartDate, paramEndDate, 1,
                            paramLeadTime, paramInfo.UserId, paramInfo.ConductId, result.StockDivision, paramDelFlg, ref resultList, ref errorMessage, result.PlanDivision);
                        if (!retVal)
                        {
                            retVal = true;
                            continue;
                        }

                        //品目情報退避
                        backItemcd = result.ItemCd;
                        backSpeccd = result.SpecificationCd;
                    }

                    decimal childInventoryQty = 0;
                    decimal allocationQty = 0;
                    foreach (var resultRow in resultList)
                    {

                        //品目在庫情報取得
                        childInventoryQty = 0;
                        retVal = getItemInventory(db, resultRow.ChildItemCd, resultRow.ChilSpecificationCd,
                            paramInfo, ref paramItemInvList, ref childInventoryQty, ref errorMessage);
                        if (!retVal)
                        {
                            break;
                        }

                        //引当数計算(引当数＝使用数＊発注数(親))
                        allocationQty = resultRow.UseQty * planQty;

                        //端数処理
                        int smallNumLength = 0;
                        decimal roundDivision = 0;
                        retVal = getNumberChkDigitInfo(db, result.UnitOfOperationManagement, paramInfo.LanguageId,
                            ref smallNumLength, ref roundDivision, ref errorMessage);
                        if (!retVal)
                        {
                            smallNumLength = int.Parse(ComConst.NUMBER_CHKDISIT.INIT_SMALLNUM_LENGTH);
                            roundDivision = decimal.Parse(ComConst.NUMBER_CHKDISIT.INIT_ROUND_DIVISION);
                        }
                        allocationQty = APCheckDigitUtil.APCheckDigitUtil.RoundBatch(allocationQty, smallNumLength, roundDivision);

                        //MRP結果に登録（子品目）
                        retVal = createDataMRPResult(db, paramInfo, result.ItemCd, result.SpecificationCd,
                            result.DeliverLimit, planDate, result.ReferenceNo,
                            resultRow.ChildItemCd, resultRow.ChilSpecificationCd,
                            allocationQty, resultRow.LeadTime, ref errorMessage);
                        if (!retVal)
                        {
                            break;
                        }
                    }

                    //展開処理内でエラーが発生した場合は処理中断！
                    if (!retVal)
                    {
                        break;
                    }
                }

                return retVal;

            }
            catch (Exception ex)
            {
                //ログメッセージ：親品目展開　異常終了
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10006 }, paramInfo.LanguageId);
                logger.Error(messageVal + " [ParentDeployment]:" + ex.Message);
                logger.Error(ex.ToString());
                errorMessage.Add(messageVal + " [ParentDeployment]:" + ex.Message);
                return false;
            }

        }

        #endregion

        #region 子品目展開処理

        /// <summary>
        /// 子品目展開処理
        /// (品目仕様を元にMRP結果情報生成)
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="paramCondition">画面情報</param>
        /// <param name="paramItemInvList">品目在庫リスト</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <param name="baseMessage">画面表示メッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool childDeployment(ComDB db, ComDaoBatch.ComParamInfo paramInfo,
            Dao.FormCodition paramCondition, ref List<Dao.TempItemInventory> paramItemInvList, ref List<string> errMessage, ref string baseMessage)
        {
            bool retVal = true;
            string messageVal = string.Empty;

            try
            {
                //LLC Maxレベル取得
                string sql = "";
                sql = sql + "select ";
                sql = sql + "     max(coalesce(low_level_cd, 0)) as maxLevel ";
                sql = sql + "from  ";
                sql = sql + "    item_specification ";
                sql = sql + "where  ";
                sql = sql + "    activate_flg = @Approval ";
                sql = sql + "and del_flg = @Off ";

                //動的sql
                Dao.MaxLevelInfo　levelInfo = db.GetEntityByDataClass<Dao.MaxLevelInfo>(
                    sql,
                    new
                    {
                        Approval = ComConst.AVTIVE_FLG.APPROVAL,
                        Off = ComConst.DEL_FLG.OFF
                    });
                if (levelInfo == null)
                {
                    //ログメッセージ：MAXレベルの取得に失敗しました
                    messageVal = comST.GetPropertiesMessage(key: ComID.MB10019, languageId: paramInfo.LanguageId);
                    logger.Info(messageVal);
                    errMessage.Add(messageVal);
                    baseMessage = comST.GetPropertiesMessage(ComID.MB10277, paramInfo.LanguageId);
                    return false;
                }

                //0レベルからMAXレベルまで１階層ずつ処理実施
                for (int treeLevel = 0; treeLevel <= levelInfo.MaxLevel; treeLevel++)
                {

                    retVal = getStructureChild(db, paramInfo, paramCondition, treeLevel, ref paramItemInvList, ref errMessage, ref baseMessage);
                    if (!retVal)
                    {
                        break;
                    }

                }
                return retVal;
            }
            catch (Exception ex)
            {
                //ログメッセージ：子品目展開　異常終了
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10007 }, paramInfo.LanguageId);
                logger.Error(messageVal + " [ChildDeployment]:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + " [ChildDeployment]:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 対象LLCに一致する対象品目情報を取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="paramCondition">画面情報</param>
        /// <param name="paramTreeLevel">階層レベル</param>
        /// <param name="paramItemInvList">品目在庫リスト</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <param name="baseMessage">画面表示メッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool getStructureChild(ComDB db, ComDaoBatch.ComParamInfo paramInfo,
            Dao.FormCodition paramCondition,
            int paramTreeLevel, ref List<Dao.TempItemInventory> paramItemInvList,
            ref List<string> errMessage, ref string baseMessage)
        {
            string messageVal = string.Empty;
            string sql;
            bool retVal = true;

            try
            {
                //品目毎の最大LLC＝パラメータの階層レベル情報取得
                sql = string.Empty;
                sql = sql + "select";
                sql = sql + "    item_cd ";
                sql = sql + "    , specification_cd ";
                sql = sql + "    , max(coalesce(low_level_cd, 0)) as maxLLC ";
                sql = sql + "from ";
                sql = sql + "    item_specification ";
                sql = sql + "where ";
                sql = sql + "    activate_flg = @ActiveFlg ";
                sql = sql + "and del_flg = @DelFlg ";
                sql = sql + "and low_level_cd = @LowLevelCd ";
                sql = sql + "group by ";
                sql = sql + "    item_cd ";
                sql = sql + "    , specification_cd ";
                sql = sql + "order by item_cd, specification_cd ";

                //動的sql
                IList<Dao.TargetItem> itemList = db.GetListByDataClass<Dao.TargetItem>(
                    sql,
                    new
                    {
                        ActiveFlg = ComConst.AVTIVE_FLG.APPROVAL,
                        DelFlg = ComConst.DEL_FLG.OFF,
                        LowLevelCd = paramTreeLevel
                    });

                if (itemList == null)
                {
                    // ログメッセージ：生産計画順：N の品目仕様情報が存在しません
                    messageVal = comST.GetPropertiesMessage(new string[] {ComID.MB10033, paramTreeLevel.ToString() }, paramInfo.LanguageId);
                    logger.Info(messageVal);
                    errMessage.Add(messageVal);
                    baseMessage = comST.GetPropertiesMessage(ComID.MB10277, paramInfo.LanguageId);
                    return true;
                }

                foreach (var itemRow in itemList)
                {
                    if (paramTreeLevel < itemRow.MaxLLC)
                    {
                        //指定レベルより大きいレベル情報がある場合は最大レベル情報で子品目展開の為
                        continue;
                    }

                    //品目毎の展開処理
                    retVal = setChildItemInfo(db, paramInfo, paramCondition, itemRow, paramTreeLevel, ref paramItemInvList, ref errMessage, ref baseMessage);
                    if (!retVal)
                    {
                        break;
                    }
                }

                return retVal;
            }
            catch (Exception ex)
            {

                //ログメッセージ：子品目展開　異常終了
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10007 }, paramInfo.LanguageId);
                logger.Error(messageVal + " [GetStructureChild]:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + " [GetStructureChild]:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 対象品目の子品目情報：MRP結果登録
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="paramCondition">画面情報</param>
        /// <param name="paramItemList">対象品目情報</param>
        /// <param name="paramTreeLevel">階層レベル</param>
        /// <param name="paramItemInvList">品目在庫リスト</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <param name="baseMessage">画面表示メッセージ</param>
        /// <remarks>発注数計算し既存情報の更新・対象品目の１レベル展開・子品目のMRP結果登録</remarks>
        /// <returns>true:OK、false:NG</returns>
        private static bool setChildItemInfo(ComDB db, ComDaoBatch.ComParamInfo paramInfo,
            Dao.FormCodition paramCondition, Dao.TargetItem paramItemList, int paramTreeLevel,
            ref List<Dao.TempItemInventory> paramItemInvList, ref List<string> errMessage, ref string baseMessage)
        {
            bool retVal = true;
            string sql;
            string messageVal;

            try
            {
                sql = string.Empty;
                sql = sql + "with target_item as (  ";
                sql = sql + "    select ";
                sql = sql + "        item_spec.item_cd ";
                sql = sql + "        , item_spec.specification_cd ";
                sql = sql + "        , mresult.plan_no ";
                sql = sql + "        , max(item_spec.active_date) as active_date  ";
                sql = sql + "    from ";
                sql = sql + "        item_specification item_spec  ";
                sql = sql + "        inner join (  ";
                sql = sql + "            select ";
                sql = sql + "                plan_no ";
                sql = sql + "                , item_cd ";
                sql = sql + "                , specification_cd ";
                sql = sql + "                , delivery_date  ";
                sql = sql + "            from ";
                sql = sql + "                mrp_result  ";
                sql = sql + "            where ";
                sql = sql + "                item_cd = @ItemCd  ";
                sql = sql + "                and specification_cd = @SpecificationCd  ";
                sql = sql + "        ) mresult  ";
                sql = sql + "            on item_spec.item_cd = mresult.item_cd  ";
                sql = sql + "            and item_spec.specification_cd = mresult.specification_cd  ";
                sql = sql + "            and item_spec.active_date <= mresult.delivery_date  ";
                sql = sql + "    where ";
                sql = sql + "        exists (  ";
                sql = sql + "            select ";
                sql = sql + "                item_cd ";
                sql = sql + "                , active_date  ";
                sql = sql + "            from ";
                sql = sql + "                item  ";
                sql = sql + "            where ";
                sql = sql + "                activate_flg =  @Approval ";
                sql = sql + "                and del_flg = @Off ";
                sql = sql + "                and item.item_cd = item_spec.item_cd  ";
                sql = sql + "                and item.active_date = item_spec.active_date ";
                sql = sql + "        )  ";
                sql = sql + "        and item_spec.activate_flg =  @Approval ";
                sql = sql + "        and item_spec.del_flg =  @Off ";
                sql = sql + "    group by ";
                sql = sql + "        item_spec.item_cd ";
                sql = sql + "        , item_spec.specification_cd ";
                sql = sql + "        , mresult.plan_no ";
                sql = sql + ")  ";
                sql = sql + ", recipe as (  ";
                sql = sql + "    select ";
                sql = sql + "        rh.* ";
                sql = sql + "    from ";
                sql = sql + "        ( ";
                sql = sql + "         select ";
                sql = sql + "             mresult.plan_no, ";
                sql = sql + "             rech.* , ";
                sql = sql + "             row_number() OVER (PARTITION BY mresult.plan_no ORDER BY rech.recipe_priority DESC) AS num ";
                sql = sql + "         from ";
                sql = sql + "             recipe_header  rech ";
                sql = sql + "         inner join (  ";
                sql = sql + "            select ";
                sql = sql + "                plan_no ";
                sql = sql + "                , item_cd ";
                sql = sql + "                , specification_cd ";
                sql = sql + "                , delivery_date  ";
                sql = sql + "            from ";
                sql = sql + "                mrp_result  ";
                sql = sql + "            where ";
                sql = sql + "                item_cd = @ItemCd  ";
                sql = sql + "                and specification_cd = @SpecificationCd  ";
                sql = sql + "         ) mresult  ";
                sql = sql + "            on rech.item_cd = mresult.item_cd  ";
                sql = sql + "            and rech.specification_cd = mresult.specification_cd  ";
                sql = sql + "            and rech.start_date <= mresult.delivery_date ";
                sql = sql + "            and rech.end_date >= mresult.delivery_date  ";
                sql = sql + "         where rech.del_flg = @Off ";
                sql = sql + "         and rech.activate_flg = @Approval ";
                sql = sql + "    ) rh ";
                sql = sql + "    where rh.num = 1 ";
                sql = sql + ")  ";
                sql = sql + "select ";
                sql = sql + "    mrp.plan_no ";
                sql = sql + "    , mrp.item_cd ";
                sql = sql + "    , mrp.specification_cd ";
                sql = sql + "    , mrp.delivery_date ";
                sql = sql + "    , mrp.plan_qty ";
                sql = sql + "    , mrp.order_publish_division ";
                sql = sql + "    , mrp.production_plan_no ";
                sql = sql + "    , mrp.deliverlimit ";
                sql = sql + "    , mrp.remark ";
                sql = sql + "    , mrp.order_date ";
                sql = sql + "    , mrp.reference_no ";
                sql = sql + "    , mrp.allocated_qty ";
                sql = sql + "    , mrp.delivery_date_before ";
                sql = sql + "    , spec.active_date ";
                sql = sql + "    , spec.unit_of_stock_ctrl ";
                sql = sql + "    , spec.unit_of_operation_management ";
                sql = sql + "    , spec.round_off_days ";
                sql = sql + "    , spec.kg_of_fraction_management ";
                sql = sql + "    , spec.default_location ";
                sql = sql + "    , spec.plan_division ";
                sql = sql + "    , spec.stock_division ";
                sql = sql + "    , purc.purchase_trigger ";
                sql = sql + "    , purc.purchase_order_unit_qty ";
                sql = sql + "    , purc.purchase_order_min_qty ";
                sql = sql + "    , purc.purchase_order_max_qty ";
                sql = sql + "    , purc.purchase_lead_time ";
                sql = sql + "    , purc.purchase_safety_lead_time ";
                sql = sql + "    , purc.vender_cd ";
                sql = sql + "    , prod.production_plan ";
                sql = sql + "    , comm.product_order_point ";
                sql = sql + "    , recipe.standard_rate_perday ";
                sql = sql + "    , recipe.production_lead_time ";
                sql = sql + "    , recipe.product_safety_lead_time  ";
                sql = sql + "    , recipe.std_qty "; // 標準生産量
                sql = sql + "    , recipe.min_qty ";
                sql = sql + "    , recipe.max_qty  ";
                sql = sql + "    , current_date as sysdate  ";
                sql = sql + "from ";
                sql = sql + "    (  ";
                sql = sql + "        select ";
                sql = sql + "            mrp_result.* ";
                sql = sql + "            , target_item.active_date  ";
                sql = sql + "        from ";
                sql = sql + "            mrp_result  ";
                sql = sql + "            inner join target_item  ";
                sql = sql + "                on mrp_result.plan_no = target_item.plan_no ";
                sql = sql + "    ) mrp  ";
                sql = sql + "    left outer join item_specification spec  ";
                sql = sql + "        on mrp.item_cd = spec.item_cd  ";
                sql = sql + "        and mrp.specification_cd = spec.specification_cd  ";
                sql = sql + "        and mrp.active_date = spec.active_date  ";
                sql = sql + "    left outer join item_purchase_attribute purc  ";
                sql = sql + "        on mrp.item_cd = purc.item_cd  ";
                sql = sql + "        and mrp.specification_cd = purc.specification_cd  ";
                sql = sql + "        and mrp.active_date = purc.active_date  ";
                sql = sql + "        and spec.plan_division <> 1 "; // 製造以外
                sql = sql + "    left outer join item_product_attribute prod  ";
                sql = sql + "        on mrp.item_cd = prod.item_cd  ";
                sql = sql + "        and mrp.specification_cd = prod.specification_cd  ";
                sql = sql + "        and mrp.active_date = prod.active_date  ";
                sql = sql + "        and spec.plan_division = 1 "; // 製造のみ
                sql = sql + "    left outer join item_common_attribute comm  ";
                sql = sql + "        on mrp.item_cd = comm.item_cd  ";
                sql = sql + "        and mrp.specification_cd = comm.specification_cd  ";
                sql = sql + "        and mrp.active_date = comm.active_date ";
                sql = sql + "    left outer join recipe  ";
                sql = sql + "        on mrp.item_cd = recipe.item_cd  ";
                sql = sql + "        and mrp.specification_cd = recipe.specification_cd ";
                sql = sql + "        and mrp.plan_no = recipe.plan_no  ";
                sql = sql + "where ";
                sql = sql + "    spec.item_cd is not null ";
                //発注点（MRPのみ対象）
                sql = sql + "and coalesce(purc.purchase_trigger,2) = @Mrp ";
                //生産計画区分(自動のみ)
                sql = sql + "and coalesce(prod.production_plan,1) = @Auto ";
                //在庫管理区分
                sql = sql + "and spec.STOCK_DIVISION <> @InventoryExclusion ";
                ////オーダー発行区分（10:出庫予定 ,12:引当）
                //sql = sql + "and mrp.order_publish_division in (" +
                //             "'" + ComConst.ORDER_PUBLISH_DIVISION.SHIPPING + "'," +
                //             "'" + ComConst.ORDER_PUBLISH_DIVISION.RESERVE + "') ";
                sql = sql + "order by ";
                sql = sql + "    mrp.delivery_date , ";
               // sql = sql + "    mrp.delivery_date desc, ";
                sql = sql + "    to_number(mrp.order_publish_division, '99') ";

                IList<Dao.TargetMRPResult> mrpList = db.GetListByDataClass<Dao.TargetMRPResult>(
                    sql,
                    new
                    {
                        ItemCd = paramItemList.ItemCd,
                        SpecificationCd = paramItemList.SpecificationCd,
                        Approval = ComConst.AVTIVE_FLG.APPROVAL,
                        Off = ComConst.DEL_FLG.OFF,
                        Mrp = ComConst.PURCHASE_TRIGGER.MRP,
                        Auto = ComConst.PRODUCTION_PLAN.AUTO,
                        InventoryExclusion = ComConst.STOCK_DIVISION.UPDATE_EXCLUSION
                    });

                if (mrpList == null)
                {
                    // ログメッセージ：対象情報が存在しません。
                    messageVal = comST.GetPropertiesMessage(new string[] {ComID.MB10021, ComID.MB10009, paramItemList.ItemCd, paramItemList.SpecificationCd }, paramInfo.LanguageId);
                    logger.Info(messageVal);
                    errMessage.Add(messageVal);
                    baseMessage = comST.GetPropertiesMessage(ComID.MB10277, paramInfo.LanguageId);
                    return false;
                }

                decimal saftyStock = 0;         //安全在庫
                bool saftyFlg = false;
                bool boolFirst = true;
                decimal calcStockQty = 0;       //計算：在庫数
                decimal calcShortage = 0;       //計算：在庫不足数
                DateTime backPlanDate;
                decimal backOrderQty = 0;
                string backItemCd = "";
                string backSpecCd = "";
                DateTime? backDelivery_date = null;
                DateTime? firstDeliveryDate = null;
                string backPlanNo = string.Empty;
                decimal backCalcStockQty = 0;       //計算：在庫数(日数まるめ)
                string setPlanNo = "";
                decimal maxOrder = 0;
                decimal minOrder = 0;
                decimal orderUnitQty = 0;
                int editRoundOffDay = 0;
                List<ComDaoBatch.TempPartsStructure> levelList = null;

                foreach (var mrpRow in mrpList)
                {
                    // 研究用BOMステータスチェック
                    if (mrpRow.PlanDivision == ComConst.PLAN_DIVISION.PRODUCTION && !checkBomStatus(mrpRow.Deliverlimit, mrpRow.ItemCd, mrpRow.SpecificationCd, db))
                    {
                        // ログメッセージ：製造品に紐付くBomが存在しないか不備があります。
                        messageVal = "品目[" + mrpRow.ItemCd + "] 仕様[" + mrpRow.SpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10282, paramInfo.LanguageId);
                        logger.Info(messageVal);
                        errMessage.Add(messageVal);
                        baseMessage = comST.GetPropertiesMessage(ComID.MB10277, paramInfo.LanguageId);
                        continue;
                    }

                    //安全在庫数設定計算（運用管理単位に換算）
                    //saftyStock = mrpRow.ProductOrderPoint * mrpRow.KgOfFractionManagement;
                    if (paramCondition.SafteyStockType == 1)
                    {
                        saftyStock = 0;
                    }
                    else
                    {
                        //saftyStock = mrpRow.ProductOrderPoint * mrpRow.KgOfFractionManagement;
                        saftyStock = mrpRow.ProductOrderPoint;
                    }
                    //品目在庫：有効在庫数(品目ごとに１回のみ)
                    if (boolFirst)
                    {
                        retVal = getAvailableQty(mrpRow.ItemCd, mrpRow.SpecificationCd,
                                paramInfo.LanguageId, paramItemInvList, ref calcStockQty, ref errMessage);

                        boolFirst = false;
                    }

                    //安全在庫割れ確認
                    if (!saftyFlg)
                    {
                        if ((saftyStock > 0) && (saftyStock < calcStockQty))
                        {
                            saftyFlg = true;
                        }
                    }

                    //有効在庫数 = 有効在庫数 + 発注数 - 引当数
                    calcStockQty = calcStockQty + mrpRow.PlanQty - mrpRow.AllocatedQty;

                    //有効在庫数 >= 安全在庫の場合は発注しない
                    if (calcStockQty >= saftyStock)
                    {
                        continue;
                    }

                    //出庫予定・引当以外は対象外
                    if (!((mrpRow.OrderPublishDivision == ComConst.ORDER_PUBLISH_DIVISION.SHIPPING)
                        || (mrpRow.OrderPublishDivision == ComConst.ORDER_PUBLISH_DIVISION.RESERVE)))
                    {
                        continue;
                    }

                    //有効在庫割れした不足分を求める
                    calcShortage = saftyStock - calcStockQty;

                    //最大発注量・最小発注量設定
                    maxOrder = mrpRow.PurchaseOrderMaxQty;
                    minOrder = mrpRow.PurchaseOrderMinQty;
                    orderUnitQty = mrpRow.PurchaseOrderUnitQty;
                    if (mrpRow.PlanDivision == ComConst.PLAN_DIVISION.PRODUCTION)
                    {
                        //計画区分が製造の場合：BOMヘッダ情報設定
                        maxOrder = mrpRow.MaxQty;
                        minOrder = mrpRow.MinQty;
                        orderUnitQty = mrpRow.StdQty;
                    }

                    //日数まるめ数編集
                    editRoundOffDay = mrpRow.RoundOffDays ?? 0;

                    //日数まるめ判定
                    if (editRoundOffDay == 0)
                    {
                        //日数まるめなし
                        // 発注数計算
                        decimal orderQty = 0;
                        string note = string.Empty;

                        retVal = getOrderQty(db, calcShortage, minOrder, maxOrder, orderUnitQty,
                            mrpRow.UnitOfOperationManagement, paramInfo.LanguageId, ref orderQty, ref note, mrpRow.PlanDivision, ref errMessage);
                        if (!retVal)
                        {
                            break;
                        }
                        //在庫数計算
                        calcStockQty += orderQty;

                        //  MRP結果更新処理
                        backPlanDate = mrpRow.DeliveryDate;
                        backOrderQty = 0;
                        retVal = updateMRPResult(db, paramInfo, paramCondition, mrpRow, mrpRow.PlanNo, orderQty, note,
                            ref saftyFlg, ref backPlanDate, ref backOrderQty, ref errMessage);
                        if (!retVal)
                        {
                            break;
                        }

                        //1レベル展開処理
                        retVal = childItem1LevelDeploy(db, paramInfo, paramTreeLevel, paramCondition,
                            mrpRow, ref paramItemInvList, orderQty, backPlanDate, backItemCd, backSpecCd, ref levelList, ref errMessage);
                        if (!retVal)
                        {
                            break;
                        }

                        //品目情報退避
                        backItemCd = mrpRow.ItemCd;
                        backSpecCd = mrpRow.SpecificationCd;

                    }
                    else
                    {
                        //日数まるめあり
                        if (backDelivery_date == null)
                        {
                            //初回
                            backDelivery_date = DateTime.ParseExact(mrpRow.DeliveryDate.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
                            //初回を退避 2022.03.09
                            firstDeliveryDate = DateTime.ParseExact(mrpRow.DeliveryDate.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
                            if (mrpRow.RoundOffDays > 0)
                            {
                                backDelivery_date = ((DateTime)backDelivery_date).AddDays(editRoundOffDay - 1);
                            }

                            //日数まるめ時点の有効在庫数
                            backCalcStockQty = calcStockQty;

                            //生産計画番号退避
                            backPlanNo = mrpRow.PlanNo;
                        }
                        else
                        {
                            if (backDelivery_date < DateTime.ParseExact(mrpRow.DeliveryDate.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null))
                            {
                                //まるめ日数分の情報を登録

                                //有効在庫割れした不足分を求める
                                calcShortage = saftyStock - backCalcStockQty;

                                //発注数計算
                                decimal orderQty = 0;
                                string note = string.Empty;
                                retVal = getOrderQty(db, calcShortage, minOrder, maxOrder, orderUnitQty,
                                    mrpRow.UnitOfStockCtrl, paramInfo.LanguageId, ref orderQty, ref note, mrpRow.PlanDivision, ref errMessage);
                                if (!retVal)
                                {
                                    break;
                                }

                                //まるめ日数開始時点の有効在庫数 < 安全在庫の場合のみ発注する
                                if (backCalcStockQty < saftyStock)
                                {
                                    //対象生産計画番号設定
                                    setPlanNo = backPlanNo;

                                    //指定計画番号情報取得
                                    var trgetRow = (Dao.TargetMRPResult)mrpList.Where(v => v.PlanNo == setPlanNo).First();

                                    //  MRP結果更新処理
                                    backPlanDate = mrpRow.DeliveryDate;
                                    backOrderQty = 0;
                                    retVal = updateMRPResult(db, paramInfo, paramCondition, trgetRow, setPlanNo, orderQty, note,
                                        ref saftyFlg, ref backPlanDate, ref backOrderQty, ref errMessage);
                                    if (!retVal)
                                    {
                                        break;
                                    }

                                    //1レベル展開処理
                                    retVal = childItem1LevelDeploy(db, paramInfo, paramTreeLevel, paramCondition,
                                        trgetRow, ref paramItemInvList, orderQty, backPlanDate, backItemCd, backSpecCd, ref levelList, ref errMessage);
                                    if (!retVal)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    //発注数・製造依頼数を更新しないので　オーダー数は０とする
                                    orderQty = 0;
                                }

                                //在庫数計算(現在在庫+以前安全在庫を割った際の計画数量)
                                //calcStockQty = backCalcStockQty + orderQty + (mrpRow.PlanQty - mrpRow.AllocatedQty);
                                calcStockQty = calcStockQty + backOrderQty;

                                //判定用終了日付計算
                                backDelivery_date = DateTime.ParseExact(mrpRow.DeliveryDate.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
                                //初回を退避 2022.03.08
                                firstDeliveryDate = DateTime.ParseExact(mrpRow.DeliveryDate.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
                                if (mrpRow.RoundOffDays > 0)
                                {
                                    backDelivery_date = ((DateTime)backDelivery_date).AddDays(editRoundOffDay - 1);

                                }

                                //日数まるめ時点の有効在庫数
                                backCalcStockQty = calcStockQty;

                                //生産計画番号退避
                                backPlanNo = mrpRow.PlanNo;

                                //品目情報退避
                                backItemCd = mrpRow.ItemCd;
                                backSpecCd = mrpRow.SpecificationCd;
                            }
                            else
                            {
                                //退避有効在庫数　加算
                                backCalcStockQty += mrpRow.PlanQty - mrpRow.AllocatedQty;
                            }
                        }
                    }
                }

                //エラーにて処理中断した場合は　以降の処理は実行しない
                if (!retVal)
                {
                    return false;
                }

                //対象情報が存在しない場合は以降の処理不要
                if (mrpList.Count <= 0)
                {
                    return true;
                }

                //最終情報設定(まるめ日数情報　未登録の場合)
                var mrpRowLast = mrpList.Last();

                //出庫予定・引当以外は対象外
                if (!((mrpRowLast.OrderPublishDivision == ComConst.ORDER_PUBLISH_DIVISION.SHIPPING)
                    || (mrpRowLast.OrderPublishDivision == ComConst.ORDER_PUBLISH_DIVISION.RESERVE)))
                {
                    return true;
                }

                if (backDelivery_date != null)
                {
                    //有効在庫割れした不足分を求める
                    calcShortage = saftyStock - backCalcStockQty;

                    //発注数計算
                    decimal orderQty = 0;
                    string note = string.Empty;
                    retVal = getOrderQty(db, calcShortage, minOrder, maxOrder, orderUnitQty,
                        mrpRowLast.UnitOfStockCtrl, paramInfo.LanguageId, ref orderQty, ref note, mrpRowLast.PlanDivision, ref errMessage);
                    if (!retVal)
                    {
                        return false;
                    }

                    //まるめ日数開始時点の有効在庫数 < 安全在庫の場合のみ発注する
                    if (backCalcStockQty < saftyStock)
                    {

                        //  MRP結果更新処理
                        backPlanDate = mrpRowLast.DeliveryDate;
                        // 退避した初回納期を設定
                        mrpRowLast.DeliveryDate = firstDeliveryDate.GetValueOrDefault();
                        backOrderQty = 0;
                        retVal = updateMRPResult(db, paramInfo, paramCondition, mrpRowLast, backPlanNo, orderQty, note,
                            ref saftyFlg, ref backPlanDate, ref backOrderQty, ref errMessage);
                        if (!retVal)
                        {
                            return false;
                        }

                        //1レベル展開処理
                        retVal = childItem1LevelDeploy(db, paramInfo, paramTreeLevel, paramCondition,
                            mrpRowLast, ref paramItemInvList, orderQty, backPlanDate, backItemCd, backSpecCd, ref levelList, ref errMessage);
                        if (!retVal)
                        {
                            return false;
                        }
                    }

                    //在庫数計算
                    calcStockQty = backCalcStockQty + orderQty;
                }

                return true;
            }
            catch (Exception ex)
            {

                //ログメッセージ：子品目展開　異常終了
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10007 }, paramInfo.LanguageId);
                logger.Error(messageVal + "[SetChildItemInfo]:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "[SetChildItemInfo]:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 登録計画数取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramNeedQty">必要数</param>
        /// <param name="paramMinOrderQty">最低発注数</param>
        /// <param name="paramMaxOrderQty">最大発注数</param>
        /// <param name="paramOrdeLotQty">発注単位</param>
        /// <param name="paramUnitOfOperationManagement">運用管理区分</param>
        /// <param name="paramLangageId">言語区分</param>
        /// <param name="paramOrderQty">生産計画数</param>
        /// <param name="paramNote">備考</param>
        /// <param name="paramPlanDivision">計画区分</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool getOrderQty(ComDB db, decimal paramNeedQty, decimal paramMinOrderQty,
            decimal paramMaxOrderQty, decimal paramOrdeLotQty, string paramUnitOfOperationManagement,
            string paramLangageId, ref decimal paramOrderQty, ref string paramNote, int paramPlanDivision, ref List<string> errMessage)
        {

            try
            {
                decimal calcQty = paramNeedQty;
                if ((paramMinOrderQty > 0) && (paramMinOrderQty > paramNeedQty))
                {
                    calcQty = paramMinOrderQty;
                }
                bool retVal = true;

                //数値桁数チェックマスタ情報取得
                int smallNumLength = 0;
                decimal roundDivision = 0;
                retVal = getNumberChkDigitInfo(db, paramUnitOfOperationManagement, paramLangageId,
                    ref smallNumLength, ref roundDivision, ref errMessage);
                if (!retVal)
                {
                    smallNumLength = int.Parse(ComConst.NUMBER_CHKDISIT.INIT_SMALLNUM_LENGTH);
                    roundDivision = decimal.Parse(ComConst.NUMBER_CHKDISIT.INIT_ROUND_DIVISION);
                }

                //ロットまるめ
                if (paramOrdeLotQty > 0)
                {
                    //// 数値桁数チェックマスタからデータを取得
                    //CommonAPUtil.APCheckDigitUtil.APCheckDigitDataClass.NumberChkDigitDetail digitInfo
                    //    = new CommonAPUtil.APCheckDigitUtil.APCheckDigitDataClass.NumberChkDigitDetail();
                    //digitInfo = APCheckDigitUtil.APCheckDigitUtil.GetCheckDigit(pUnitOfStockControl, "", "", db);

                    //小数桁数の１０のべき乗を取得
                    decimal multiplier = (decimal)Math.Pow(10, decimal.ToDouble((decimal)smallNumLength));

                    //ロット数を取得
                    decimal workCnt = decimal.Truncate((calcQty * multiplier) / (paramOrdeLotQty * multiplier));
                    if ((calcQty * multiplier) % (paramOrdeLotQty * multiplier) > 0)
                    {
                        workCnt += 1;
                    }
                    calcQty = workCnt * paramOrdeLotQty;

                }
                else
                {
                    //端数処理
                    calcQty = APCheckDigitUtil.APCheckDigitUtil.RoundBatch(calcQty, smallNumLength, roundDivision);
                }

                //--- 戻り値設定 ---
                //最大発注数チェック
                if ((paramMaxOrderQty != 0) && (paramMaxOrderQty < calcQty))
                {
                    if (paramPlanDivision == ComConst.PLAN_DIVISION.PRODUCTION)
                    {
                        //文言設定：最大生産数オーバー
                        string strVal = comST.GetPropertiesMessage(ComID.CB10011, paramLangageId);
                        paramNote = strVal;
                    }
                    else
                    {
                        //文言設定：最大発注数オーバー
                        string strVal = comST.GetPropertiesMessage(ComID.CB10006, paramLangageId);
                        paramNote = strVal;
                    }
                }

                //計画数
                paramOrderQty = calcQty;

                return true;
            }
            catch (Exception ex)
            {
                //ログメッセージ：子品目展開　異常終了
                string messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10007 }, paramLangageId);
                logger.Error(messageVal + "【GetOrderQty】:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "【GetOrderQty】:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  子品目展開時：１レベル展開処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">共通パラメータクラス</param>
        /// <param name="paramTreeLevel">階層レベル</param>
        /// <param name="paramCondition">画面条件クラス</param>
        /// <param name="paramMrpRow">MRP結果対象行情報</param>
        /// <param name="paramItemInvList">品目在庫リスト</param>
        /// <param name="paramParentOrderQty">親品目発注計画数</param>
        /// <param name="paramParentPlanDate">親品目発注日</param>
        /// <param name="paramBkItemCd">品目コード</param>
        /// <param name="paramBkSpecificationCd">仕様コード</param>
        /// <param name="paramResultList">1レベル展開情報</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool childItem1LevelDeploy(ComDB db, ComDaoBatch.ComParamInfo paramInfo,
            int paramTreeLevel, Dao.FormCodition paramCondition,
            Dao.TargetMRPResult paramMrpRow, ref List<Dao.TempItemInventory> paramItemInvList,
            decimal paramParentOrderQty, DateTime paramParentPlanDate,
            string paramBkItemCd, string paramBkSpecificationCd, ref List<ComDaoBatch.TempPartsStructure> paramResultList,
            ref List<string> errMessage)
        {
            bool retVal = true;
            string messageVal = "";

            try
            {
                //展開実行有無判定
                if (paramMrpRow.PlanDivision == ComConst.PLAN_DIVISION.PRODUCTION)
                {
                    //製造品の場合：生産計画区分が手動は展開対象外
                    if (paramMrpRow.ProductionPlan == ComConst.PRODUCTION_PLAN.MANUAL)
                    {
                        return true;
                    }
                }
                else
                {
                    //購入品の場合：個別発注は展開対象外
                    if (paramMrpRow.PurchaseTrigger == ComConst.PURCHASE_TRIGGER.INDIVIDUAL)
                    {
                        return true;
                    }
                }

                //品目が変わった時のみレベル展開処理を実施
                if (string.IsNullOrEmpty(paramBkItemCd) ||
                    ((paramBkItemCd != paramMrpRow.ItemCd)
                    || (paramBkSpecificationCd != paramMrpRow.SpecificationCd)))
                {
                    //1レベル展開処理
                    DateTime paramStartDate = DateTime.ParseExact("0001/01/01", "yyyy/MM/dd", null);
                    DateTime paramEndDate = DateTime.ParseExact("9999/12/31", "yyyy/MM/dd", null);
                    int paramLeadTime = 0;
                    bool paramDelFlg = true;
                    if (paramResultList != null)
                    {
                        paramResultList.Clear();
                    }

                    paramResultList = new List<ComDaoBatch.TempPartsStructure>();

                    retVal = ComPak.Exe1LevelPartsStructure(db, paramInfo,
                        paramMrpRow.ItemCd, paramMrpRow.SpecificationCd,
                        paramMrpRow.Deliverlimit, paramStartDate, paramEndDate, 1,
                        paramLeadTime, paramInfo.UserId, paramInfo.ConductId, paramMrpRow.StockDivision, paramDelFlg, ref paramResultList, ref errMessage, paramMrpRow.PlanDivision);
                    if (!retVal)
                    {
                        return true;
                    }
                }

                decimal childInventoryQty = 0;
                decimal allocationQty = 0;
                foreach (var resultRow in paramResultList)
                {

                    //品目在庫情報取得
                    childInventoryQty = 0;
                    retVal = getItemInventory(db, resultRow.ChildItemCd, resultRow.ChilSpecificationCd,
                        paramInfo, ref paramItemInvList, ref childInventoryQty, ref errMessage);
                    if (!retVal)
                    {
                        break;
                    }

                    //引当数計算(引当数＝使用数＊発注数(親))
                    allocationQty = resultRow.UseQty * paramParentOrderQty;

                    //端数処理
                    int smallNumLength = 0;
                    decimal roundDivision = 0;
                    retVal = getNumberChkDigitInfo(db, paramMrpRow.UnitOfOperationManagement, paramInfo.LanguageId,
                        ref smallNumLength, ref roundDivision, ref errMessage);
                    if (!retVal)
                    {
                        smallNumLength = int.Parse(ComConst.NUMBER_CHKDISIT.INIT_SMALLNUM_LENGTH);
                        roundDivision = decimal.Parse(ComConst.NUMBER_CHKDISIT.INIT_ROUND_DIVISION);
                    }
                    allocationQty
                        = APCheckDigitUtil.APCheckDigitUtil.RoundBatch(allocationQty, smallNumLength, roundDivision);

                    //MRP結果に登録（子品目）
                    retVal = createDataMRPResult(db, paramInfo, paramMrpRow.ItemCd, paramMrpRow.SpecificationCd,
                        paramMrpRow.Deliverlimit, paramParentPlanDate, paramMrpRow.ReferenceNo,
                        resultRow.ChildItemCd, resultRow.ChilSpecificationCd,
                        allocationQty, resultRow.LeadTime, ref errMessage);

                    if (!retVal)
                    {
                        break;
                    }
                }

                return retVal;
            }
            catch (Exception ex)
            {

                //ログメッセージ：子品目展開　異常終了
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10007 }, paramInfo.LanguageId);
                logger.Error(messageVal + "【ChildItem1LevelDeploy】：" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "【ChildItem1LevelDeploy】：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// MRP結果更新処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="paramCondition">画面条件クラス</param>
        /// <param name="paramMrpRow">MRP結果対象行情報</param>
        /// <param name="setPlanNo">更新対象計画番号</param>
        /// <param name="paramOrderQty">発注計画数</param>
        /// <param name="paramNote">備考</param>
        /// <param name="paramSaftyFlg">安全在庫フラグ</param>
        /// <param name="paramOrderDate">受注日</param>
        /// <param name="paramBcakOrderQty">受注数量</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool updateMRPResult(ComDB db, ComDaoBatch.ComParamInfo paramInfo,
            Dao.FormCodition paramCondition, Dao.TargetMRPResult paramMrpRow,
            string setPlanNo, decimal paramOrderQty, string paramNote, ref bool paramSaftyFlg,
            ref DateTime paramOrderDate, ref decimal paramBcakOrderQty, ref List<string> errMessage)
        {
            bool retVal;
            string strVal = string.Empty;
            string messageVal = string.Empty;
            try
            {

                //安全在庫割れ確認
                //文言："・計画"取得
                strVal = comST.GetPropertiesMessage(ComID.CB10007, paramInfo.LanguageId);
                string note = strVal;
                string note2 = string.Empty;
                if (paramSaftyFlg)
                {
                    //文言："＋安全・計画"
                    strVal = comST.GetPropertiesMessage(ComID.CB10008, paramInfo.LanguageId);
                    note2 = strVal;
                }

                //手続き区分/LeadTime
                int procDivision = ComConst.MRP_PROCEDURE_DIVISION.PURCHASE;
                int leadTime = 0;
                int safetyLeadTime = 0;
                if (paramMrpRow.PlanDivision == ComConst.PLAN_DIVISION.PRODUCTION)
                {
                    procDivision = ComConst.MRP_PROCEDURE_DIVISION.PRODUCTION;
                    //製造品の場合：ＢＯＮヘッダの製造リードタイム＋安全リードタイム
                    leadTime = (paramMrpRow.ProductionLeadTime ?? 0) + (paramMrpRow.ProductSafetyLeadTime ?? 0);
                    safetyLeadTime = paramMrpRow.ProductSafetyLeadTime ?? 0;
                }
                else
                {
                    procDivision = ComConst.MRP_PROCEDURE_DIVISION.PURCHASE;
                    //購買の場合：品目マスタ_購入品扱い属性の販売リードタイム＋安全リードタイム
                    leadTime = (paramMrpRow.PurchaseLeadTime ?? 0) + (paramMrpRow.PurchaseSafetyLeadTime ?? 0);
                    safetyLeadTime = paramMrpRow.PurchaseSafetyLeadTime ?? 0;
                }

                //発注数
                decimal planQty = paramMrpRow.PlanQty + paramOrderQty;
                if (planQty > 1000000000)
                {
                    // ログメッセージ：計画発注数量が十億を超えました。
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB10018, paramMrpRow.ItemCd, paramMrpRow.SpecificationCd }, paramInfo.LanguageId);
                    logger.Info(messageVal);
                    errMessage.Add(messageVal);
                }

                //オーダー発行区分
                string orderDiv = string.Empty;
                if (paramMrpRow.OrderPublishDivision == ComConst.ORDER_PUBLISH_DIVISION.RESERVE)
                {
                    //オーダー発行区分 5:計画･引当
                    orderDiv = ComConst.ORDER_PUBLISH_DIVISION.PLAN_RESERVE;
                }
                else if (paramMrpRow.OrderPublishDivision == ComConst.ORDER_PUBLISH_DIVISION.SHIPPING)
                {
                    //オーダー発行区分 6:計画･出庫予定
                    orderDiv = ComConst.ORDER_PUBLISH_DIVISION.PLAN_SHIPPING;
                }

                //リードタイム取得
                // パラメータ を pMrpRow.PlanQty → planQty に修正
                //int wkLeadTime = GetLeadTime(pPramInfo, procDivision, pMrpRow.PlanQty,
                //    (pMrpRow.StandardRatePerday ?? 0), pCondition.LeadTimeType, leadTime);
                int workLeadTime = getLeadTime(paramInfo, procDivision, planQty,
                        paramMrpRow.StandardRatePerday ?? 0, paramCondition.LeadTimeType, leadTime, safetyLeadTime);
                //計画発注日取得
                DateTime planDate = paramMrpRow.DeliveryDate;
                //retVal = CalcPlanDate(db, pMrpRow.DeliveryDate, leadTime, pPramInfo.LanguageId, ref planDate);
                retVal = calcPlanDate(db, paramMrpRow.DeliveryDate, workLeadTime, paramInfo.LanguageId, ref planDate, ref errMessage);
                if (!retVal)
                {
                    return false;
                }

                //発注日が処理日時点より手前のものを緊急手配とする
                if (planDate < paramMrpRow.SysDate)
                {
                    planDate = paramMrpRow.SysDate;
                    //文言：" 緊急手配 "
                    strVal = comST.GetPropertiesMessage(ComID.CB10009, paramInfo.LanguageId);
                    note2 = note2 + strVal;
                }
                else
                {
                    //文言：" 通常手配 "
                    strVal = comST.GetPropertiesMessage(ComID.CB10010, paramInfo.LanguageId);
                    note2 = note2 + strVal;
                }

                //期間まるめ区分
                int roundOffDays = 0;       //通常

                if (paramMrpRow.RoundOffDays > 0)
                {
                    roundOffDays = 1;       //まるめ対象
                }

                //備考
                if (string.IsNullOrEmpty(paramNote))
                {
                    note = paramMrpRow.Remark + note + note2;
                }
                else
                {
                    note = paramNote;
                }

                //MRP結果更新処理
                string sql = string.Empty;
                sql = sql + "update mrp_result ";
                sql = sql + "set ";
                sql = sql + "    procedure_division = @ProcDivision ";
                sql = sql + "    ,plan_qty = @PlamQty ";
                sql = sql + "    ,order_publish_division = @OrderDiv ";
                sql = sql + "    ,location_cd = @DefaultLocation ";
                sql = sql + "    ,order_date = to_date( @PlanDate ,'YYYY/MM/DD') ";
                sql = sql + "    ,status = 0 ";
                sql = sql + "    ,marume_division = @RoundOffDays ";
                sql = sql + "    ,order_rule = @PurchaseTrigger ";
                sql = sql + "    ,vender_cd = @VenderCd ";
                sql = sql + "    ,remark = @Note ";
                if ((paramMrpRow.DeliveryDateBefore != DateTime.Parse("0001/01/01"))
                    && (paramMrpRow.DeliveryDateBefore != null))
                {
                    sql = sql + "    ,delivery_date_before = to_date('" +
                    paramMrpRow.DeliveryDateBefore.ToString("yyyy/MM/dd") + "','YYYY/MM/DD') ";
                }
                sql = sql + "    ,plan_qty_before = @PlanQty ";
                sql = sql + "    ,update_date = now() ";
                sql = sql + "    ,update_user_id = @UserId ";

                sql = sql + "where ";
                sql = sql + "    plan_no = @PlanNo ";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        ProcDivision = procDivision,
                        PlamQty = planQty,
                        OrderDiv = orderDiv,
                        DefaultLocation = paramMrpRow.DefaultLocation,
                        PlanDate= planDate.ToString("yyyy/MM/dd"),
                        RoundOffDays = roundOffDays,
                        PurchaseTrigger = paramMrpRow.PurchaseTrigger,
                        VenderCd = paramMrpRow.VenderCd,
                        Note = note,
                        PlanQty = paramMrpRow.PlanQty,
                        UserId = paramInfo.UserId,
                        PlanNo = setPlanNo
                    });
                if (regFlg < 0)
                {
                    // 異常終了
                    // ログメッセージ：MRP結果更新失敗
                    messageVal = "品目[" + paramMrpRow.ItemCd + "] 仕様[" + paramMrpRow.SpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10275, paramInfo.LanguageId);
                    logger.Info(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }

                paramOrderDate = planDate;
                paramBcakOrderQty = paramOrderQty;
                paramSaftyFlg = false;

                return true;
            }
            catch (Exception ex)
            {

                //ログメッセージ：子品目展開　異常終了
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10007 }, paramInfo.LanguageId);
                logger.Error(messageVal + "【UpdateMRPResult】:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "【UpdateMRPResult】:" + ex.Message);
                return false;
            }
        }

        #endregion

        #region 発注点発注処理
        /// <summary>
        /// 発注点発注処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>正常終了：true</returns>
        private static bool orderingPoint(ComDB db, ComDaoBatch.ComParamInfo paramInfo, ref List<string> errMessage)
        {
            string sql;
            string errMsgId = string.Empty;
            string messageVal = string.Empty;

            try
            {
                // 発注点取得
                sql = string.Empty;

                sql = sql + " with info as ( ";
                sql = sql + " select item.item_cd ";
                sql = sql + "        , spec.specification_cd ";
                sql = sql + "        , ipa.purchase_order_point ";
                sql = sql + "        , ipa.purchase_order_unit_qty ";
                sql = sql + "        , ipa.vender_cd ";
                sql = sql + "        , spec.default_location ";
                sql = sql + "        , ipa.purchase_lead_time ";
                sql = sql + "        , ipa.purchase_safety_lead_time ";
                sql = sql + "        , spec.round_off_days ";
                sql = sql + "        , ipa.purchase_order_min_qty ";
                sql = sql + "        , ipa.purchase_order_max_qty ";
                sql = sql + " from ";
                sql = sql + "    v_item_regist item ";
                sql = sql + " inner join v_item_specification_regist spec ";
                sql = sql + " on  item.item_cd = spec.item_cd ";
                sql = sql + " and item.active_date = spec.active_date ";
                sql = sql + " and spec.purchase_division = 1 "; // 購入品
                sql = sql + " and spec.plan_division <> 1 "; // 製造品以外
                sql = sql + " inner join item_purchase_attribute ipa ";
                sql = sql + " on  ipa.item_cd = spec.item_cd ";
                sql = sql + " and ipa.specification_cd = spec.specification_cd ";
                sql = sql + " and ipa.active_date = spec.active_date ";
                sql = sql + " and ipa.purchase_trigger = @OrderingPoint ) "; // 発注点

                sql = sql + " , qty as ( ";
                sql = sql + " select info.item_cd ";
                sql = sql + "        , info.specification_cd ";
                sql = sql + "        , info.purchase_order_point ";
                sql = sql + "        , info.purchase_order_unit_qty ";
                sql = sql + "        , info.vender_cd ";
                sql = sql + "        , info.default_location ";
                sql = sql + "        , info.purchase_lead_time ";
                sql = sql + "        , info.purchase_safety_lead_time ";
                sql = sql + "        , info.round_off_days ";
                sql = sql + "        , info.purchase_order_min_qty ";
                sql = sql + "        , info.purchase_order_max_qty ";
                sql = sql + "        , coalesce(sum(vii.available_qty),0) - coalesce(sum(vli.available_qty),0) as available_qty";
                sql = sql + " from ";
                sql = sql + "   info ";
                sql = sql + " left join  v_lot_inventory vli ";
                sql = sql + " on  vli.item_cd = info.item_cd ";
                sql = sql + " and vli.specification_cd = info.specification_cd ";
                sql = sql + " and vli.stock_division <> 1 "; // 帳簿内(自社在庫)以外
                sql = sql + " left join  v_item_inventory vii ";
                sql = sql + " on  vii.item_cd = info.item_cd ";
                sql = sql + " and vii.specification_cd = info.specification_cd ";
                sql = sql + " group by info.item_cd, info.specification_cd, info.purchase_order_point, info.purchase_order_unit_qty ";
                sql = sql + "          , info.vender_cd, info.default_location, info.purchase_lead_time, purchase_safety_lead_time, info.round_off_days, info.purchase_order_min_qty, info.purchase_order_max_qty) ";

                sql = sql + " select * ";
                sql = sql + " from ";
                sql = sql + "    qty ";
                sql = sql + " where ";
                sql = sql + " available_qty < purchase_order_point ";
                sql = sql + " or available_qty is null ";

                //動的sql
                IList<Dao.OrderPoint> results = db.GetListByDataClass<Dao.OrderPoint>(
                    sql,
                    new
                    {
                        OrderingPoint = ComConst.PURCHASE_TRIGGER.ORDERING_POINT
                    });

                if (results == null || results.Count == 0)
                {
                    // ログメッセージ：発注点発注　対象情報がありません
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00017, ComID.MB10285 }, paramInfo.LanguageId);
                    logger.Info(messageVal);
                    errMessage.Add(messageVal);
                    return true;
                }

                foreach (var result in results)
                {
                    if (result.PurchaseOrderUnitQty == null || result.PurchaseOrderUnitQty == 0)
                    {
                        // ログメッセージ：購入品の発注単位に0が設定されているか不備があります。
                        messageVal = "品目[" + result.ItemCd + "] 仕様[" + result.SpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10288, paramInfo.LanguageId);
                        logger.Info(messageVal);
                        errMessage.Add(messageVal);
                        continue;
                    }

                    decimal orderUnitQty = decimal.Parse(result.PurchaseOrderUnitQty.ToString()); // 発注単位
                    decimal orderPoint = decimal.Parse(result.PurchaseOrderPoint.ToString()); // 発注点
                    decimal orderQty = 0; // 発注数量
                    bool overFlg = false; // 最大発注数オーバーフラグ

                    // 発注数量の設定
                    if (orderPoint - result.AvailableQty == 0)
                    {
                        orderQty = orderUnitQty;
                    }
                    else
                    {
                        orderQty = Math.Ceiling((orderPoint - result.AvailableQty) / orderUnitQty) * orderUnitQty;
                    }

                    // 最低発注数設定
                    if (result.PurchaseOrderMinQty != null && orderQty < result.PurchaseOrderMinQty)
                    {
                        orderQty = decimal.Parse(result.PurchaseOrderMinQty.ToString());
                    }

                    // 最大発注数設定
                    if (result.PurchaseOrderMaxQty != null && orderQty > result.PurchaseOrderMaxQty)
                    {
                        overFlg = true;
                    }

                    //手続き区分/LeadTime
                    //リードタイム：品目マスタ_購入品扱い属性の販売リードタイム＋安全リードタイム
                    int workLeadTime = (result.PurchaseLeadTime ?? 0) + (result.PurchaseSafetyLeadTime ?? 0);

                    //計画発注日取得
                    DateTime planDate = DateTime.Now;
                    bool retVal = calcPlanDate(db, DateTime.Now, workLeadTime, paramInfo.LanguageId, ref planDate, ref errMessage, true);
                    if (!retVal)
                    {
                        break;
                    }

                    // MRP結果登録
                    if (!insertMrpOrder(db, paramInfo, ref errMessage, result, orderQty, overFlg, planDate))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                //ログメッセージ：発注点発注　異常終了
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10285 }, paramInfo.LanguageId);
                logger.Error(messageVal + "【UpdateMRPResult】:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "【UpdateMRPResult】:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 発注点MRP結果登録
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <param name="result">品目情報</param>
        /// <param name="orderQty">発注数量</param>
        /// <param name="overFlg">最大発注数オーバーフラグ</param>
        /// <param name="orderDate">納期</param>
        /// <returns>正常終了：true</returns>
        private static bool insertMrpOrder(ComDB db, ComDaoBatch.ComParamInfo paramInfo, ref List<string> errMessage, Dao.OrderPoint result, decimal orderQty, bool overFlg, DateTime orderDate)
        {
            string sql = string.Empty;
            string messageVal = string.Empty;
            try
            {
                //計画番号：新規採番
                string planNo = CommonAPUtil.APStoredPrcUtil.APStoredPrcUtil.ProSeqGetNo(db, ComConst.SEQUENCE_PROC_NAME.MRP_RESULT);

                sql = "";
                sql = sql + " insert ";
                sql = sql + " into mrp_result( ";
                sql = sql + "  plan_no   ";
                sql = sql + "  , procedure_division  ";
                sql = sql + "  , item_cd ";
                sql = sql + "  , specification_cd ";
                sql = sql + "  , delivery_date ";
                sql = sql + "  , plan_qty   ";
                sql = sql + "  , order_publish_division ";
                sql = sql + "  , deliverlimit ";
                sql = sql + "  , remark  ";
                sql = sql + "  , vender_cd  ";
                sql = sql + "  , location_cd   ";
                sql = sql + "  , order_date ";
                sql = sql + "  , status  ";
                sql = sql + "  , marume_division  ";
                sql = sql + "  , delivery_date_before   ";
                sql = sql + "  , plan_qty_before  ";
                sql = sql + "  , input_date ";
                sql = sql + "  , input_user_id ";
                sql = sql + "  , update_date   ";
                sql = sql + "  , update_user_id   ";
                sql = sql + " ) ";
                sql = sql + " values ( ";
                sql = sql + "    @PlanNo ";
                sql = sql + "  , @ProcedureDivision ";
                sql = sql + "  , @ItemCd ";
                sql = sql + "  , @SpecificationCd ";
                sql = sql + "  , to_date(@DeliveryDate , 'YYYY/MM/DD') ";
                sql = sql + "  , @PlanQty ";
                sql = sql + "  , @OrderPublishDivision ";
                sql = sql + "  , to_date(@DeliveryDate , 'YYYY/MM/DD') ";
                sql = sql + "  , @Remark ";
                sql = sql + "  , @VenderCd ";
                sql = sql + "  , @LocationCd ";
                sql = sql + "  , to_date( @OrderDate , 'YYYY/MM/DD') ";
                sql = sql + "  , @Status ";
                sql = sql + "  , @MarumeDivision ";
                sql = sql + "  , to_date(@DeliveryDate , 'YYYY/MM/DD') ";
                sql = sql + "  , @PlanQtyBefore ";
                sql = sql + "  , CURRENT_TIMESTAMP   ";
                sql = sql + "  , @UserId ";
                sql = sql + "  , CURRENT_TIMESTAMP  ";
                sql = sql + "  , @UserId ";
                sql = sql + " )";

                int regFlg;
                string remark = "発注点発注";
                if (overFlg)
                {
                    remark = "発注点発注 最大発注数オーバー";
                }
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        PlanNo = planNo,
                        ProcedureDivision = ComConst.MRP_PROCEDURE_DIVISION.PURCHASE,
                        ItemCd = result.ItemCd,
                        SpecificationCd = result.SpecificationCd,
                        PlanQty = orderQty,
                        OrderPublishDivision = "2",
                        Remark = remark,
                        VenderCd = result.VenderCd,
                        LocationCd = result.DefaultLocation,
                        OrderDate = string.Format("{0:yyyy/MM/dd}", DateTime.Now),
                        Status = 0,
                        MarumeDivision = result.RoundOffDays > 0 ? 1 : 0,
                        DeliveryDate = string.Format("{0:yyyy/MM/dd}", orderDate),
                        PlanQtyBefore = orderQty,
                        UserId = paramInfo.UserId
                    });
                if (regFlg < 0)
                {
                    // ログ出力：MRP結果登録に失敗しました
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB10014, ComID.MB10009, result.ItemCd, result.SpecificationCd }, paramInfo.LanguageId);
                    logger.Info(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // ログ出力：MRP結果登録に失敗しました
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB10014, ComID.MB10009, result.ItemCd, result.SpecificationCd }, paramInfo.LanguageId);
                logger.Error(messageVal + "【RegistMrpReult】:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "【RegistMrpReult】:" + ex.Message);
                return false;
            }
        }

        #endregion

        #region 部品展開内共通処理

        /// <summary>
        /// 品目在庫データ取得
        /// （有効在庫数取得)
        /// </summary>
        /// <param name="paramItemCd">品目コード</param>
        /// <param name="paramSpecificationCd">仕様コード</param>
        /// <param name="paramLangageId">言語区分</param>
        /// <param name="paramItemInvList">品目在庫リスト</param>
        /// <param name="inventoryQty">在庫数</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool getAvailableQty(string paramItemCd, string paramSpecificationCd, string paramLangageId,
             List<Dao.TempItemInventory> paramItemInvList, ref decimal inventoryQty, ref List<string> errMessage)
        {
            //品目在庫取得
            try
            {
                Dao.TempItemInventory result = paramItemInvList.Find(n => n.ItemCd == paramItemCd && n.SpecificationCd == paramSpecificationCd);

                if (result != null)
                {
                    inventoryQty = result.InventoryQty;
                    return true;
                }

                //存在しない場合は　在庫数=0
                inventoryQty = 0;
                return true;

            }
            catch (Exception ex)
            {
                //ログメッセージ：子品目展開　異常終了
                string messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00015, ComID.MB10007 }, paramLangageId);
                logger.Error(messageVal + "【GetAvailableQty】:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "【GetAvailableQty】:" + ex.Message);
                return false;
            }

        }

        /// <summary>
        /// 品目在庫データ取得
        /// （有効在庫数取得)
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramItemCd">品目コード</param>
        /// <param name="paramSpecificationCd">仕様コード</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="paramItemInvList">品目在庫リスト</param>
        /// <param name="inventoryQty">現在在庫数</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool getItemInventory(ComDB db, string paramItemCd, string paramSpecificationCd,
            ComDaoBatch.ComParamInfo paramInfo, ref List<Dao.TempItemInventory> paramItemInvList, ref decimal inventoryQty, ref List<string> errMessage)
        {
            //品目在庫取得
            string message;

            try
            {
                Dao.TempItemInventory result = paramItemInvList.Find(n => n.ItemCd == paramItemCd && n.SpecificationCd == paramSpecificationCd);

                if (result != null)
                {
                    //実在庫を有効在庫数とする
                    inventoryQty = result.InventoryQty;
                    return true;
                }

                //リスト品目在庫が存在しない場合　DBより取得する
                string sql;
                sql = string.Empty;
                sql = sql + "select  * ";
                sql = sql + "  from  item_inventory_fixed ";
                sql = sql + " where  item_cd = @ItemCd ";
                sql = sql + "   and  specification_cd = @Specification ";

                // 動的SQL実行
                IList<ComDao.ItemInventoryFixedEntity> results = db.GetListByDataClass<ComDao.ItemInventoryFixedEntity>(
                    sql,
                    new
                    {
                        ItemCd = paramItemCd,
                        Specification = paramSpecificationCd
                    });

                if (results.Count == 0)
                {
                    //ログメッセージ：品目在庫情報が存在しません。
                    message = "品目[" + paramItemCd + "] 仕様[" + paramSpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10276, paramInfo.LanguageId);
                    logger.Info(message);
                    errMessage.Add(message);

                    //有効在庫数＝０で登録
                    paramItemInvList.Add(new Dao.TempItemInventory(paramItemCd, paramSpecificationCd, 0));
                    inventoryQty = 0;
                }
                else
                {
                    //取得した有効在庫数で取得
                    //pItemInvList.Add(new Dao.TempItemInventory(results[0].ItemCd, results[0].SpecificationCd, (decimal)results[0].AvailableQty));
                    //availableQty = (decimal)results[0].AvailableQty;
                    //実在庫を有効在庫数とする
                    paramItemInvList.Add(new Dao.TempItemInventory(results[0].ItemCd, results[0].SpecificationCd, (decimal)results[0].InventoryQty));
                    inventoryQty = (decimal)results[0].InventoryQty;
                }
                return true;
            }
            catch (Exception ex)
            {
                //ログメッセージ：品目在庫情報　取得に失敗しました
                message = comST.GetPropertiesMessage(new string[] {ComID.MB10026, ComID.MB10028 }, languageId: paramInfo.LanguageId);
                logger.Error(message + " 【GetItemInventory】：" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(message + " 【GetItemInventory】：" + ex.Message);

                return false;
            }

        }

        /// <summary>
        /// 数値桁数マスタ情報取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramUnitDivision">区分(単位）</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramSmallnumLength">小数部桁数</param>
        /// <param name="paramRoundDivision">端数区分</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool getNumberChkDigitInfo(ComDB db, string paramUnitDivision,
             string paramLanguageId, ref int paramSmallnumLength, ref decimal paramRoundDivision, ref List<string> errMessage)
        {
            //数値桁数マスタ情報取得
            string message;

            try
            {

                CommonAPUtil.APCheckDigitUtil.APCheckDigitDataClass.NumberChkDigitDetail
                    result = chkDigitInfo.Find(n => n.UnitDivision == paramUnitDivision);

                if (result != null)
                {
                    paramSmallnumLength = int.Parse((result.SmallnumLength ?? decimal.Parse(ComConst.NUMBER_CHKDISIT.INIT_SMALLNUM_LENGTH)).ToString());
                    paramRoundDivision = result.RoundDivision ?? decimal.Parse(ComConst.NUMBER_CHKDISIT.INIT_ROUND_DIVISION);
                    return true;
                }

                //数値桁数チェックマスタが存在しない場合　DBより取得する
                CommonAPUtil.APCheckDigitUtil.APCheckDigitDataClass.NumberChkDigitDetail digitInfo
                    = new CommonAPUtil.APCheckDigitUtil.APCheckDigitDataClass.NumberChkDigitDetail();
                digitInfo = APCheckDigitUtil.APCheckDigitUtil.GetCheckDigit(paramUnitDivision, "", "", db);

                //リストに追加
                chkDigitInfo.Add(digitInfo);

                paramSmallnumLength = int.Parse((digitInfo.SmallnumLength ?? decimal.Parse(ComConst.NUMBER_CHKDISIT.INIT_SMALLNUM_LENGTH)).ToString());
                paramRoundDivision = digitInfo.RoundDivision ?? decimal.Parse(ComConst.NUMBER_CHKDISIT.INIT_ROUND_DIVISION);

                return true;
            }
            catch (Exception ex)
            {

                //ログメッセージ：数値桁数チェックマスタ情報　取得に失敗しました
                message = comST.GetPropertiesMessage(new string[] { ComID.MB10026, ComID.MB10063 }, languageId: paramLanguageId);
                logger.Error(message + " 【GetItemInventory】：" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(message + " 【GetItemInventory】：" + ex.Message);

                paramSmallnumLength =  int.Parse(ComConst.NUMBER_CHKDISIT.INIT_SMALLNUM_LENGTH);
                paramRoundDivision =  decimal.Parse(ComConst.NUMBER_CHKDISIT.INIT_ROUND_DIVISION);

                return true;
            }

        }

        /// <summary>
        /// 対象品目のリードタイム情報取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramItemCd">品目コード</param>
        /// <param name="paramSpecificationCd">仕様コード</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="paramDeliverLimit">納期</param>
        /// <param name="paramLeadTime">計算後リードタイム</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool getItemLeadTime(ComDB db, string paramItemCd, string paramSpecificationCd,
            ComDaoBatch.ComParamInfo paramInfo, DateTime paramDeliverLimit, ref int paramLeadTime, ref List<string> errMessage)
        {
            //品目在庫取得
            string message;

            try
            {
                IList<Dao.LeadTimeInfo> result = leadTimeInfo
                    .Where(x => x.ItemCd == paramItemCd
                          && x.SpecificationCd == paramSpecificationCd
                          && x.ActiveDate <= paramDeliverLimit)
                    .OrderByDescending(x => x.ActiveDate)
                    .ToList();

                if (result.Count > 0)
                {
                    if (result[0].PlanDivision == ComConst.PLAN_DIVISION.PRODUCTION)
                    {
                        paramLeadTime = result[0].ProductionLeadTime + result[0].ProductSafetyLeadTime;
                    }
                    else
                    {
                        paramLeadTime = result[0].PurchaseLeadTime + result[0].PurchaseSafetyLeadTime;
                    }
                    return true;
                }

                //リストリードタイム情報が存在しない場合　DBより取得する
                string sql;
                sql = string.Empty;
                sql = sql + "with target_item as ( ";
                sql = sql + "    select ";
                sql = sql + "        item_spec.item_cd ";
                sql = sql + "        , item_spec.specification_cd ";
                sql = sql + "        , item_spec.active_date  ";
                sql = sql + "        , item_spec.plan_division";
                sql = sql + "    from ";
                sql = sql + "        item_specification item_spec  ";
                sql = sql + "    where ";
                sql = sql + "        item_cd = @ItemCd  ";
                sql = sql + "        and specification_cd = @SpecificationCd ";
                sql = sql + "        and activate_flg = @Approval ";
                sql = sql + "        and del_flg = @Off ";
                sql = sql + "        and active_date <= to_date( @DeliverLimit ,'YYYY/MM/DD') ";
                sql = sql + "        and exists (  ";
                sql = sql + "            select ";
                sql = sql + "                item_cd ";
                sql = sql + "                , active_date  ";
                sql = sql + "            from ";
                sql = sql + "                item  ";
                sql = sql + "            where ";
                sql = sql + "                activate_flg = @Approval ";
                sql = sql + "                and del_flg =  @Off ";
                sql = sql + "                and item.item_cd = item_spec.item_cd  ";
                sql = sql + "                and item.active_date = item_spec.active_date ";
                sql = sql + "        )  ";
                sql = sql + "    order by ";
                sql = sql + "        item_spec.active_date desc ";
                sql = sql + "    limit 1 ";
                sql = sql + ")";
                sql = sql + ", recipe as ( ";
                sql = sql + "select ";
                sql = sql + "    *  ";
                sql = sql + "from ";
                sql = sql + "    recipe_header  ";
                sql = sql + "where ";
                sql = sql + "    item_cd = @ItemCd  ";
                sql = sql + "    and specification_cd = @SpecificationCd ";
                sql = sql + "    and start_date <= to_date( @DeliverLimit ,'YYYY/MM/DD') ";
                sql = sql + "    and end_date >= to_date( @DeliverLimit ,'YYYY/MM/DD') ";
                sql = sql + "    and activate_flg = @Approval ";
                sql = sql + "    and coalesce(del_flg, 0) = @Off ";
                sql = sql + "order by ";
                sql = sql + "    recipe_priority desc  ";
                sql = sql + "limit 1 ";
                sql = sql + ") ";
                sql = sql + "select ";
                sql = sql + "    spec.item_cd, ";
                sql = sql + "    spec.specification_cd, ";
                sql = sql + "    spec.active_date, ";
                sql = sql + "    spec.plan_division, ";
                sql = sql + "    coalesce(pur.purchase_lead_time,0) as purchase_lead_time, ";
                sql = sql + "    coalesce(pur.purchase_safety_lead_time,0) as purchase_safety_lead_time, ";
                sql = sql + "    coalesce(rec.production_lead_time,0) as production_lead_time, ";
                sql = sql + "    coalesce(rec.product_safety_lead_time,0) as product_safety_lead_time  ";
                sql = sql + "from target_item spec ";
                sql = sql + "  left outer join recipe rec ";
                sql = sql + "    on spec.item_cd = rec.item_cd ";
                sql = sql + "    and spec.specification_cd = rec.specification_cd ";
                sql = sql + "  left outer join item_purchase_attribute pur ";
                sql = sql + "    on spec.item_cd = pur.item_cd ";
                sql = sql + "    and spec.specification_cd = pur.specification_cd ";
                sql = sql + "    and spec.active_date = pur.active_date ";

                // 動的SQL実行
                IList<Dao.LeadTimeInfo> results = db.GetListByDataClass<Dao.LeadTimeInfo>(
                    sql,
                    new
                    {
                        ItemCd = paramItemCd,
                        SpecificationCd = paramSpecificationCd,
                        DeliverLimit = paramDeliverLimit.ToString("yyyy/MM/dd"),
                        Approval = ComConst.AVTIVE_FLG.APPROVAL,
                        Off = ComConst.DEL_FLG.OFF
                    });

                if (results.Count == 0)
                {
                    // ログメッセージ：対象リードタイム情報が存在しません
                    message = comST.GetPropertiesMessage(ComID.MB10027, paramInfo.LanguageId);
                    errMessage.Add(message);
                    logger.Error(message);

                    paramLeadTime = 0;
                }
                else
                {
                    if (results[0].PlanDivision == ComConst.PLAN_DIVISION.PRODUCTION)
                    {
                        paramLeadTime = results[0].ProductionLeadTime + results[0].ProductSafetyLeadTime;
                    }
                    else
                    {
                        paramLeadTime = results[0].PurchaseLeadTime + results[0].PurchaseSafetyLeadTime;
                    }

                    Dao.LeadTimeInfo temp = new Dao.LeadTimeInfo();
                    temp.ItemCd = results[0].ItemCd;
                    temp.SpecificationCd = results[0].SpecificationCd;
                    temp.ActiveDate = results[0].ActiveDate;
                    temp.PlanDivision = results[0].PlanDivision;
                    temp.PurchaseLeadTime = results[0].PurchaseLeadTime;
                    temp.PurchaseSafetyLeadTime = results[0].PurchaseSafetyLeadTime;
                    temp.ProductionLeadTime = results[0].ProductionLeadTime;
                    temp.ProductSafetyLeadTime = results[0].ProductSafetyLeadTime;
                    leadTimeInfo.Add(temp);

                }
                return true;
            }
            catch (Exception ex)
            {
                //ログメッセージ：リードタイム情報　取得に失敗しました。
                message = comST.GetPropertiesMessage(new string[] { ComID.MB10026, ComID.MB10029 }, languageId: paramInfo.LanguageId);
                logger.Error(message + " 【GetLeadTime】：" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(message + " 【GetLeadTime】：" + ex.Message);

                return false;
            }

        }

        /// <summary>
        /// リードタイム計算
        /// </summary>
        /// <param name="pramInfo">共通パタメータ</param>
        /// <param name="paramProcedureDivision">手続き区分</param>
        /// <param name="paramQty">親品目生産数</param>
        /// <param name="paramStandardRatePerday">標準日別生産量</param>
        /// <param name="paramLeadTimeType">指定リードタイム区分</param>
        /// <param name="paramLeadTime">ﾃﾞﾌｫﾙﾄ：リードタイム</param>
        /// <param name="paramSafetyLeadTime">安全リードタイム</param>
        /// <returns>リードタイム</returns>
        private static int getLeadTime(ComDaoBatch.ComParamInfo pramInfo,
            int paramProcedureDivision, decimal paramQty, decimal paramStandardRatePerday, int paramLeadTimeType, int paramLeadTime, int paramSafetyLeadTime)
        {
            int retLeadTime = 0;
            try
            {

                //画面のLeadTime区分＝0 または　手続区分 <> 1(製造以外)
                if ((paramLeadTimeType == 0) || (paramProcedureDivision != 1))
                {
                    return paramLeadTime;
                }

                //画面のLeadTime区分＝2(日当数)
                if (paramLeadTimeType == 2)
                {
                    if (paramStandardRatePerday > 0)
                    {
                        if ((paramQty % paramStandardRatePerday) == 0)
                        {
                            retLeadTime = decimal.ToInt32(paramQty / paramStandardRatePerday) + paramSafetyLeadTime;
                        }
                        else
                        {
                            retLeadTime = decimal.ToInt32(paramQty / paramStandardRatePerday) + 1 + paramSafetyLeadTime;
                        }
                        return retLeadTime;
                    }
                }

                //上記以外
                return paramLeadTime;

            }
            catch (Exception)
            {
                return paramLeadTime;
            }
        }

        /// <summary>
        /// 発注日計算（納期より取得）DB操作クラス
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramDeliverLimit">納期</param>
        /// <param name="paramLeadTime">リードタイム</param>
        /// <param name="paramLanguageId">言語区分</param>
        /// <param name="paramPlanDate">発注日</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <param name="orderPointFlg">発注点発注フラグ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool calcPlanDate(ComDB db, DateTime paramDeliverLimit, int paramLeadTime, string paramLanguageId, ref DateTime paramPlanDate, ref List<string> errMessage, bool orderPointFlg = false)
        {

            try
            {
                string sql;
                sql = string.Empty;
                sql = sql + "select calTemp.cal_date ";
                sql = sql + "  from ( ";
                sql = sql + "        select *  ";
                sql = sql + "          from cal  ";
                sql = sql + "        where cal_cd = @CalCd ";
                if (orderPointFlg)
                {
                    sql = sql + "          and cal_date >= to_date( @DeliverLimit , 'YYYY/MM/DD') ";
                }
                else
                {
                    sql = sql + "          and cal_date <= to_date( @DeliverLimit , 'YYYY/MM/DD') ";
                }
                sql = sql + "          and cal_holiday = 0  ";
                sql = sql + "        order by cal_date ";
                if (!orderPointFlg)
                {
                    sql = sql + " desc ";
                }
                sql = sql + "         limit @LeadTime ";
                sql = sql + "       ) calTemp  ";
                sql = sql + " order by calTemp.cal_date ";
                if (orderPointFlg)
                {
                    sql = sql + " desc ";
                }
                sql = sql + " limit 1 ";

                // 動的SQL実行
                IList<Dao.CalParts> results = db.GetListByDataClass<Dao.CalParts>(
                    sql,
                    new
                    {
                        CalCd = useCalCd,
                        DeliverLimit = paramDeliverLimit.ToString("yyyy/MM/dd"),
                        LeadTime = paramLeadTime
                    });

                if (results.Count == 0)
                {
                    // 対象日付なしの場合は　引数の納期
                    paramPlanDate = paramDeliverLimit;
                }
                else
                {
                    paramPlanDate = results[0].CalDate;
                }

                return true;

            }
            catch (Exception ex)
            {
                // ログメッセージ：発注日計算　失敗しました
                string messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB00016, ComID.MB10030 }, paramLanguageId);
                logger.Error(messageVal + "【CalcPlanDate】:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "【CalcPlanDate】:" + ex.Message);

                return false;
            }
        }

        /// <summary>
        /// MRP結果登録前処理（子品目登録時）
        /// </summary
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="paramParentItemCd">親品目コード</param>
        /// <param name="paramParentSpecificationCD">親仕様コード</param>
        /// <param name="paramParentDeliverLimit">納期</param>
        /// <param name="paramParentPlanDate">計画発注日</param>
        /// <param name="paramReferenceNo">リファレンス番号</param>
        /// <param name="paramChildItemCd">子品目コード</param>
        /// <param name="paramChildSpecificationCd">子仕様コード</param>
        /// <param name="paramUseQty">子品目使用数量</param>
        /// <param name="paramLeadTime">子品目リードタイム</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool createDataMRPResult(ComDB db, ComDaoBatch.ComParamInfo paramInfo,
            string paramParentItemCd, string paramParentSpecificationCD,
            DateTime paramParentDeliverLimit, DateTime paramParentPlanDate, string paramReferenceNo,
            string paramChildItemCd, string paramChildSpecificationCd, decimal paramUseQty, int paramLeadTime,
            ref List<string> errMessage)
        {
            bool retVal;
            DateTime planDate;
            string messageVal;
            int setLeadTime = 0;
            try
            {
                //対象品目のLeadTime取得
                retVal = getItemLeadTime(db, paramChildItemCd, paramChildSpecificationCd, paramInfo,
                    paramParentPlanDate, ref setLeadTime, ref errMessage);
                if (!retVal)
                {
                    return false;
                }

                //納期取得
                //-------デフォルト親の計画発注日（LeadTimが指定されている場合は再取得）
                //planDate = pParentPlanDate;
                //デフォルト納期に修正
                planDate = paramParentDeliverLimit;
                if (setLeadTime > 0)
                {
                    retVal = calcPlanDate(db, paramParentPlanDate, setLeadTime, paramInfo.LanguageId, ref planDate, ref errMessage);
                    if (!retVal)
                    {
                        return false;
                    }
                }

                //"引当"文言取得
                string strVal = comST.GetPropertiesMessage(key: ComID.CB10003, languageId: paramInfo.LanguageId);

                //MRP結果登録
                ComDao.MrpResultEntity temp = new ComDao.MrpResultEntity();
                temp.PlanNo = string.Empty;
                temp.ProcedureDivision = 0;
                temp.ItemCd = paramChildItemCd;
                temp.SpecificationCd = paramChildSpecificationCd;
                temp.DeliveryDate = paramParentPlanDate;
                temp.PlanQty = 0;
                temp.OrderPublishDivision = ComConst.ORDER_PUBLISH_DIVISION.RESERVE; //引当
                temp.ProductionPlanNo = string.Empty;
                temp.Deliverlimit = paramParentPlanDate;
                temp.Remark = strVal;                                                  //引当(文言)
                temp.VenderCd = string.Empty;
                temp.LocationCd = string.Empty;
                temp.OrderDate = planDate;
                temp.ReferenceNo = paramReferenceNo;
                temp.Status = 0;
                temp.AllocatedQty = paramUseQty;
                temp.MarumeDivision = 0;
                temp.OrderDivision = 0;
                temp.OrderNo = string.Empty;
                temp.OrderRule = 0;
                temp.InventoryCalDivision = 1;
                temp.BreakdownLevel = treeLevel + 1;
                temp.DeliveryDateBefore = null;
                temp.PlanQtyBefore = 0;
                temp.ParentOrderDivision = 0;
                temp.ParentOrderNo = string.Empty;
                temp.ParentItemCd = paramParentItemCd;
                temp.ParentSpecificationCd = paramParentSpecificationCD;

                //登録
                retVal = registMrpReult(db, temp, paramInfo, ref errMessage);
                if (!retVal)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // ログ出力：MRP結果登録に失敗しました
                messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB10014, ComID.MB10009, paramChildItemCd, paramChildSpecificationCd }, paramInfo.LanguageId);
                logger.Error(messageVal + "【CreateDataMRPResult】:" + ex.Message);
                logger.Error(ex.ToString());
                return false;
            }

        }

        /// <summary>
        /// MRP結果登録
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramData">登録データ</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <returns>true:OK、false:NG</returns>
        private static bool registMrpReult(ComDB db, ComDao.MrpResultEntity paramData, ComDaoBatch.ComParamInfo paramInfo, ref List<string> errMessage)
        {
            string sql = string.Empty;
            string messageVal = string.Empty;
            try
            {
                //計画番号：新規採番
                paramData.PlanNo = CommonAPUtil.APStoredPrcUtil.APStoredPrcUtil.ProSeqGetNo(db, ComConst.SEQUENCE_PROC_NAME.MRP_RESULT);

                sql = "";
                sql = sql + " insert ";
                sql = sql + " into mrp_result( ";
                sql = sql + "  plan_no   ";
                sql = sql + "  , procedure_division  ";
                sql = sql + "  , item_cd ";
                sql = sql + "  , specification_cd ";
                sql = sql + "  , delivery_date ";
                sql = sql + "  , plan_qty   ";
                sql = sql + "  , order_publish_division ";
                sql = sql + "  , production_plan_no  ";
                sql = sql + "  , deliverlimit  ";
                sql = sql + "  , remark  ";
                sql = sql + "  , vender_cd  ";
                sql = sql + "  , location_cd   ";
                sql = sql + "  , order_date ";
                sql = sql + "  , reference_no ";
                sql = sql + "  , status  ";
                sql = sql + "  , allocated_qty ";
                sql = sql + "  , marume_division  ";
                sql = sql + "  , order_division   ";
                sql = sql + "  , order_no   ";
                sql = sql + "  , order_rule ";
                sql = sql + "  , inventory_cal_division ";
                sql = sql + "  , breakdown_level  ";
                sql = sql + "  , delivery_date_before   ";
                sql = sql + "  , plan_qty_before  ";
                sql = sql + "  , parent_order_division  ";
                sql = sql + "  , parent_order_no  ";
                sql = sql + "  , parent_item_cd   ";
                sql = sql + "  , parent_specification_cd   ";
                sql = sql + "  , input_date ";
                sql = sql + "  , input_user_id ";
                sql = sql + "  , update_date   ";
                sql = sql + "  , update_user_id   ";
                sql = sql + " ) ";
                sql = sql + " values ( ";
                sql = sql + "    @PlanNo ";
                sql = sql + "  , @ProcedureDivision ";
                sql = sql + "  , @ItemCd ";
                sql = sql + "  , @SpecificationCd ";
                sql = sql + "  , to_date( @DeliveryDate , 'YYYY/MM/DD')";
                sql = sql + "  , @PlanQty ";
                sql = sql + "  , @OrderPublishDivision ";
                sql = sql + "  , @ProductionPlanNo ";
                sql = sql + "  , to_date( @Deliverlimit , 'YYYY/MM/DD')";
                sql = sql + "  , @Remark ";
                sql = sql + "  , @VenderCd ";
                sql = sql + "  , @LocationCd ";
                sql = sql + "  , to_date( @OrderDate , 'YYYY/MM/DD')";
                sql = sql + "  , @ReferenceNo ";
                sql = sql + "  , @Status ";
                sql = sql + "  , @AllocatedQty ";
                sql = sql + "  , @MarumeDivision ";
                sql = sql + "  , @OrderDivision ";
                sql = sql + "  , @OrderNo ";
                sql = sql + "  , @OrderRule ";
                sql = sql + "  , @InventoryCalDivision ";
                sql = sql + "  , @BreakdownLevel ";
                sql = sql + "  ,  null ";
                sql = sql + "  , @PlanQtyBefore ";
                sql = sql + "  , @ParentOrderDivision ";
                sql = sql + "  , @ParentOrderNo ";
                sql = sql + "  , @ParentItemCd ";
                sql = sql + "  , @ParentSpecificationCd ";
                sql = sql + "  , CURRENT_TIMESTAMP   ";
                sql = sql + "  , @UserId ";
                sql = sql + "  , CURRENT_TIMESTAMP  ";
                sql = sql + "  , @UserId ";
                sql = sql + " )";

                int regFlg;
                regFlg = db.Regist(
                    sql,
                    new
                    {
                        PlanNo = paramData.PlanNo,
                        ProcedureDivision = paramData.ProcedureDivision,
                        ItemCd = paramData.ItemCd,
                        SpecificationCd = paramData.SpecificationCd,
                        DeliveryDate = string.Format("{0:yyyy/MM/dd}", paramData.DeliveryDate),
                        PlanQty = paramData.PlanQty,
                        OrderPublishDivision = paramData.OrderPublishDivision,
                        ProductionPlanNo = paramData.ProductionPlanNo,
                        Deliverlimit = string.Format("{0:yyyy/MM/dd}", paramData.Deliverlimit),
                        Remark = paramData.Remark,
                        VenderCd = paramData.VenderCd,
                        LocationCd = paramData.LocationCd,
                        OrderDate = string.Format("{0:yyyy/MM/dd}", paramData.OrderDate),
                        ReferenceNo = paramData.ReferenceNo,
                        Status = paramData.Status,
                        AllocatedQty = paramData.AllocatedQty,
                        MarumeDivision = paramData.MarumeDivision,
                        OrderDivision = paramData.OrderDivision,
                        OrderNo = paramData.OrderNo,
                        OrderRule = paramData.OrderRule,
                        InventoryCalDivision  = paramData.InventoryCalDivision,
                        BreakdownLevel = paramData.BreakdownLevel,
                        PlanQtyBefore = paramData.PlanQtyBefore,
                        ParentOrderDivision = paramData.ParentOrderDivision,
                        ParentOrderNo = paramData.ParentOrderNo,
                        ParentItemCd = paramData.ParentItemCd,
                        ParentSpecificationCd = paramData.ParentSpecificationCd,
                        UserId = paramInfo.UserId
                    });
                if (regFlg < 0)
                {
                    // ログ出力：MRP結果登録に失敗しました
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB10014, ComID.MB10009, paramData.ItemCd, paramData.SpecificationCd }, paramInfo.LanguageId);
                    logger.Info(messageVal);
                    errMessage.Add(messageVal);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // ログ出力：MRP結果登録に失敗しました
                messageVal = comST.GetPropertiesMessage(new string[] {ComID.MB10014, ComID.MB10009, paramData.ItemCd, paramData.SpecificationCd }, paramInfo.LanguageId);
                logger.Error(messageVal + "【RegistMrpReult】:" + ex.Message);
                logger.Error(ex.ToString());
                errMessage.Add(messageVal + "【RegistMrpReult】:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// （文字列）SQL登録更新文字列に編集
        /// </summary>
        /// <param name="paramVal">文字列</param>
        /// <returns>'文字列’</returns>
        private static string setSqlValueforString(string paramVal)
        {
            string editVal = string.Empty;

            if (string.IsNullOrEmpty(paramVal))
            {
                editVal = "null";
            }
            else
            {
                editVal = "'" + paramVal + "'";
            }

            return editVal;
        }

        /// <summary>
        /// レシピステータスチェック
        /// </summary>
        /// <param name="deliverLimit">納期</param>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specificationCd">仕様コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>研究用BOMだけの場合false</returns>
        private static bool checkBomStatus(DateTime deliverLimit, string itemCd, string specificationCd, ComDB db)
        {
            string sql = string.Empty;
            sql = sql + "         select ";
            sql = sql + "            recipe_header.item_cd, ";
            sql = sql + "            recipe_header.specification_cd ";
            sql = sql + "         from ";
            sql = sql + "           recipe_header ";
            sql = sql + "         where ";
            sql = sql + "           recipe_header.activate_flg = @ActivateFlg ";
            sql = sql + "           and recipe_header.start_date <= to_date(to_char(@DeliverLimit, 'yyyy/MM/dd'), 'YYYY/MM/DD') ";
            sql = sql + "           and recipe_header.end_date >= to_date(to_char(@DeliverLimit, 'yyyy/MM/dd'), 'YYYY/MM/DD') ";
            sql = sql + "           and recipe_header.item_cd = @ItemCd ";
            sql = sql + "           and recipe_header.specification_cd = @SpecificationCd ";
            sql = sql + "           and recipe_header.recipe_status <>  1 ";
            sql = sql + "                   group by ";
            sql = sql + "               item_cd, specification_cd, standard_rate_perday, recipe_priority, production_lead_time, product_safety_lead_time, std_qty, min_qty";
            sql = sql + "         order by recipe_priority desc ";

            //動的sql
            IList<Dao.PartsItemInfo> results = db.GetListByDataClass<Dao.PartsItemInfo>(
                sql,
                new
                {
                    DeliverLimit = deliverLimit,
                    ItemCd = itemCd,
                    SpecificationCd = specificationCd,
                    ActivateFlg = ComConst.AVTIVE_FLG.APPROVAL,
                });

            if (results == null || results.Count == 0)
            {
                // 研究用BOM以外のBOMが存在しない場合エラー
                return false;
            }

            return true;
        }

        #endregion

        #endregion

    }
}
