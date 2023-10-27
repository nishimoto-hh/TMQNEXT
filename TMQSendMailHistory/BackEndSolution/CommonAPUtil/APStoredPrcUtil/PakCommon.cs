using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonSTDUtil.CommonLogger;

using APCheckDigitUtil = CommonAPUtil.APCheckDigitUtil.APCheckDigitUtil;
using ComConst = APConstants.APConstants;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComID = CommonAPUtil.APCommonUtil.APResources.ID;
using comST = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = CommonAPUtil.APStoredPrcUtil.PakCommonDataClass;

namespace CommonAPUtil.APStoredPrcUtil
{
    #region MyRegion
    /// <summary>
    /// 1レベル展開処理
    /// </summary>
    public class PakCommon
    {

        /// <summary>
        /// 列挙体
        /// </summary>
        public enum BatchStatus : int
        {
            /// <summary>要求</summary>
            request,
            /// <summary>受付</summary>
            reception,
            /// <summary>開始</summary>
            start,
            /// <summary>実行</summary>
            end = 9
        }

        #region コンストラクタ
        //public PakCommon()
        //{
        //}
        #endregion

        #region 1レベル展開処理

        /// <summary>
        /// １レベル部品展開処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramInfo">バッチ共通：パラメータ情報</param>
        /// <param name="paramItemCd">親品目コード</param>
        /// <param name="paramSpecificationCd">親仕様コード</param>
        /// <param name="paramDeliveryDate">納期（納入予定日？）</param>
        /// <param name="paramStartDate">有効開始日</param>
        /// <param name="paramEndDate">有効終了日</param>
        /// <param name="paramQty">親品目使用数</param>
        /// <param name="paramLeadTime">親品目リードタイム</param>
        /// <param name="paramUserCd">ユーザID（テーブル登録がないので不要かも？）</param>
        /// <param name="paramModuleCd">起動元機能ID</param>
        /// <param name="stockDivision">在庫管理区分</param>
        /// <param name="paramDelFlg">削除フラグ（不要かも？）</param>
        /// <param name="resultList">取得結果リスト</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <param name="planDivision">計画区分</param>
        /// <returns>true:正常　false:エラー</returns>
        public static bool Exe1LevelPartsStructure(
            ComDB db,
            Dao.ComParamInfo paramInfo,
            string paramItemCd,
            string paramSpecificationCd,
            DateTime paramDeliveryDate,
            DateTime paramStartDate,
            DateTime paramEndDate,
            decimal paramQty,
            int paramLeadTime,
            string paramUserCd,
            string paramModuleCd,
            int? stockDivision,
            bool paramDelFlg,
            ref List<Dao.TempPartsStructure> resultList,
            ref List<string> errMessage,
            int planDivision)
        {
            //ログ出力用インスタンス生成
            CommonLogger logger = CommonLogger.GetInstance(paramInfo.PgmId);

            string messageVal = string.Empty;

            try
            {
                //１レベル情報取得
                string sql = "";
                sql = "";
                //サブクエリ：BOM情報
                sql = sql + "with recipe as ( ";
                sql = sql + " select ";
                sql = sql + "    head.recipe_id ";
                sql = sql + "    , head.item_cd as parent_item_cd ";
                sql = sql + "    , head.specification_cd as parent_specification_cd ";
                sql = sql + "    , head.start_date ";
                sql = sql + "    , head.end_date ";
                sql = sql + "    , head.std_qty ";
                sql = sql + "    , formula.step_no ";
                sql = sql + "    , formula.result_division ";
                sql = sql + "    , formula.seq_no ";
                sql = sql + "    , formula.item_cd as child_item_cd ";
                sql = sql + "    , formula.specification_cd as child_specification_cd ";
                sql = sql + "    , formula.qty ";
                sql = sql + " from ";
                sql = sql + "    ( ";
                sql = sql + "     select ";
                sql = sql + "         rhead.recipe_id ";
                sql = sql + "         , rhead.item_cd ";
                sql = sql + "         , rhead.specification_cd ";
                sql = sql + "         , rhead.start_date ";
                sql = sql + "         , rhead.end_date ";
                sql = sql + "         , rhead.std_qty ";
                sql = sql + "         , rhead.recipe_priority ";
                sql = sql + "     from ";
                sql = sql + "        recipe_header rhead ";
                sql = sql + "     where ";
                sql = sql + "         coalesce(rhead.del_flg, 0) = @Off ";
                sql = sql + "     and rhead.activate_flg =  @Approval  ";
                sql = sql + "     and rhead.start_date <= to_date(@ParamDeliveryDate,'YYYY/MM/DD') ";
                sql = sql + "     and rhead.end_date >= to_date(@ParamDeliveryDate ,'YYYY/MM/DD') ";
                sql = sql + "     and rhead.item_cd =  @ParamItemId  ";
                sql = sql + "     and rhead.specification_cd  = @ParamSpecificationCd ";
                sql = sql + "     order by ";
                sql = sql + "         recipe_priority desc ";
                sql = sql + "     limit 1 ";
                sql = sql + "    ) head ";
                sql = sql + "    inner join ";
                sql = sql + "         recipe_formula formula ";
                sql = sql + "     on head.recipe_id = formula.recipe_id ";
                sql = sql + " where ";
                sql = sql + "    formula.result_division = 0 ";
                sql = sql + ")  ";
                //サブクエリ：品目情報
                sql = sql + ", item_info as ( ";
                sql = sql + " select ";
                sql = sql + "      spec.item_cd ";
                sql = sql + "      , spec.specification_cd ";
                sql = sql + "      , spec.active_date ";
                sql = sql + "      , spec.kg_of_fraction_management ";                // 換算係数(在庫)
                sql = sql + "      , spec.unit_of_stock_ctrl ";                       // 在庫管理単位
                sql = sql + "      , spec.unit_of_operation_management ";             // 運用管理単位
                sql = sql + "      , spec.default_location ";                         // 基準保管場所
                sql = sql + "      , spec.plan_division ";                            // 計画区分（旧：原価区分）
                sql = sql + "      , spec.product_division ";
                sql = sql + "      , spec.purchase_division ";
                sql = sql + "      , spec.stock_division ";
                sql = sql + " from ";
                sql = sql + "     item_specification spec ";
                sql = sql + " inner join ";
                sql = sql + " ( ";
                sql = sql + "   select";
                sql = sql + "        spe.item_cd";
                sql = sql + "      , spe.specification_cd";
                sql = sql + "      , max(spe.active_date) as active_date";
                sql = sql + "   from ";
                sql = sql + "       item_specification spe ";
                sql = sql + "   where ";
                sql = sql + "       spe.activate_flg = @Approval ";
                sql = sql + "   and spe.del_flg = @Off ";
                sql = sql + "   and spe.active_date <= to_date(@ParamDeliveryDate ,'YYYY/MM/DD') ";
                sql = sql + "   and spe.specification_active_date <= to_date(@ParamDeliveryDate ,'YYYY/MM/DD') ";
                sql = sql + "   and exists ( ";
                sql = sql + "           select ";
                sql = sql + "               item_cd ";
                sql = sql + "               , active_date ";
                sql = sql + "           from ";
                sql = sql + "               item ";
                sql = sql + "           where ";
                sql = sql + "               activate_flg = @Approval ";
                sql = sql + "           and del_flg = @Off ";
                sql = sql + "           and item.item_cd = spe.item_cd ";
                sql = sql + "           and item.active_date = spe.active_date ";
                sql = sql + "           ) ";
                sql = sql + "   group by spe.item_cd,spe.specification_cd  ";
                sql = sql + " ) target ";
                sql = sql + " on spec.item_cd = target.item_cd ";
                sql = sql + " and spec.specification_cd = target.specification_cd ";
                sql = sql + " and spec.active_date = target.active_date ";
                sql = sql + ")  ";
                //メイン
                sql = sql + "select ";
                sql = sql + "    recipe.recipe_id ";
                sql = sql + "    , recipe.parent_item_cd ";
                sql = sql + "    , recipe.parent_specification_cd ";
                sql = sql + "    , recipe.start_date ";
                sql = sql + "    , recipe.end_date ";
                sql = sql + "    , recipe.std_qty ";
                sql = sql + "    , recipe.step_no ";
                sql = sql + "    , recipe.result_division ";
                sql = sql + "    , recipe.seq_no ";
                sql = sql + "    , recipe.child_item_cd ";
                sql = sql + "    , recipe.child_specification_cd ";
                sql = sql + "    , recipe.qty ";
                sql = sql + "    , item_info.item_cd ";
                sql = sql + "    , item_info.specification_cd ";
                sql = sql + "    , coalesce(item_info.kg_of_fraction_management, 1) as kg_of_fraction_management ";     // 換算係数(在庫)
                sql = sql + "    , item_info.unit_of_stock_ctrl ";                                                      // 在庫管理単位
                sql = sql + "    , item_info.unit_of_operation_management ";                                            // 運用管理単位
                sql = sql + "    , item_info.default_location ";                                                        // 基準保管場所
                sql = sql + "    , item_info.plan_division ";                                                           // 計画区分（旧：原価区分）
                sql = sql + "    , item_info.stock_division ";                                                          // 在庫管理単位
                //sql = sql + "    , inventory.item_cd as inventory_item_cd ";                                            // 品目在庫：品目コード
                //sql = sql + "    , inventory.specification_cd as inventory_specification_cd ";                          // 品目在庫：仕様コード
                sql = sql + "from ";
                sql = sql + "    recipe ";
                sql = sql + "left outer join ";
                sql = sql + "    item_info ";
                sql = sql + "   on ( ";
                sql = sql + "        recipe.child_item_cd = item_info.item_cd ";
                sql = sql + "    and recipe.child_specification_cd = item_info.specification_cd ";
                sql = sql + "    )  ";
                //sql = sql + "left outer join ";
                //sql = sql + "    item_inventory_fixed inventory ";
                //sql = sql + "    on ( ";
                //sql = sql + "        recipe.child_item_cd = inventory.item_cd ";
                //sql = sql + "    and recipe.child_specification_cd = inventory.specification_cd ";
                //sql = sql + "       ) ";
                sql = sql + "where ";
                sql = sql + "    item_info.item_cd is not null ";
                //sql = sql + "and inventory.item_cd is not null ";
                sql = sql + "and item_info.stock_division <> @StockDivision ";
                sql = sql + "order by ";
                sql = sql + "    recipe.seq_no, recipe.child_item_cd, child_specification_cd ";

                // 動的SQL実行
                IList<Dao.PartsStructureOrder> results = db.GetListByDataClass<Dao.PartsStructureOrder>(
                    sql,
                    new
                    {
                        Off = ComConst.DEL_FLG.OFF,
                        Approval = ComConst.AVTIVE_FLG.APPROVAL,
                        ParamDeliveryDate = string.Format("{0:yyyy/MM/dd}", paramDeliveryDate),
                        ParamItemId = paramItemCd,
                        ParamSpecificationCd = paramSpecificationCd,
                        StockDivision = ComConst.STOCK_DIVISION.UPDATE_EXCLUSION
                    });

                if (results.Count == 0)
                {
                    if (planDivision == ComConst.PLAN_DIVISION.PRODUCTION)
                    {
                        // 製造品の展開の場合エラ-
                        if (stockDivision == ComConst.STOCK_DIVISION.UPDATE_EXCLUSION)
                        {
                            // 親品目が更新除外品目の場合
                            // ログメッセージ：更新除外品目なので展開対象外です。
                            messageVal = "品目[" + paramItemCd + "] 仕様[" + paramSpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10279, paramInfo.LanguageId);
                            logger.Info(messageVal);
                            errMessage.Add(messageVal);
                        }
                        else
                        {
                            // ログメッセージ：製造品に紐付くBomが存在しないか不備があります。
                            messageVal = "品目[" + paramItemCd + "] 仕様[" + paramSpecificationCd + "]：" + comST.GetPropertiesMessage(ComID.MB10282, paramInfo.LanguageId);
                            logger.Info(messageVal);
                            errMessage.Add(messageVal);
                        }
                    }
                    return true;
                }

                foreach (var result in results)
                {
                    DateTime workStartDate;
                    DateTime workEndDate;
                    decimal workUseQty;

                    //有効開始日設定
                    if (result.StartDate < paramStartDate)
                    {
                        workStartDate = paramStartDate;
                    }
                    else
                    {
                        workStartDate = result.StartDate;
                    }

                    //有効終了日設定
                    if ((result.EndDate == null) || (result.EndDate < paramEndDate))
                    {
                        workEndDate = paramEndDate;
                    }
                    else
                    {
                        workEndDate = (DateTime)result.EndDate;
                    }

                    //使用量計算（子品目数量×比率　まるめ処理はマスタに準じる）
                    //(比率：製造数量÷標準生産量）
                    workUseQty = result.Qty * (paramQty / result.StdQty);

                    //==== 取得結果登録 ====
                    Dao.TempPartsStructure temp = new Dao.TempPartsStructure();
                    temp.ChildItemCd = result.ChildItemCd;
                    temp.ChilSpecificationCd = result.ChildSpecificationCd;
                    temp.StartDate = workStartDate;
                    temp.EndDate = workEndDate;
                    temp.UseQty = workUseQty;
                    temp.StockDivision = result.UnitOfStockCtrl;
                    temp.DefaultLocation = result.DefaultLocation;
                    temp.LeadTime = paramLeadTime;

                    resultList.Add(temp);

                }

                return true;
            }
            catch (Exception ex)
            {
                //  ログメッセージ：１レベル部品展開に失敗しました。
                messageVal = comST.GetPropertiesMessage(ComID.MB10022, paramInfo.LanguageId);
                logger.Error(messageVal + "[Exe1LevelPartsStructure]: " + ex.Message);
                errMessage.Add(messageVal);
                return false;
            }
        }

        /// <summary>
        /// 生産計画ログ登録
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramLogDivision">ログ区分</param>
        /// <param name="paramStatus">ステータス</param>
        /// <param name="paramInfo">共通パラメータ</param>
        /// <returns>true:正常　false:エラー</returns>
        public static  bool RegisPlanLog(ComDB db, int paramLogDivision, int paramStatus,
            Dao.ComParamInfo paramInfo)
        {
            string sql;
            string messageVal = string.Empty;

            //ログ出力用インスタンス生成
            CommonLogger logger = CommonLogger.GetInstance(paramInfo.PgmId);

            try
            {
                sql = "";
                sql = sql + "insert into plan_log ";
                sql = sql + "( log_division, ";
                sql = sql + "  ctrl_date, ";
                sql = sql + "  ctrl_status, ";
                sql = sql + "  input_date, ";
                sql = sql + "  input_user_id, ";
                sql = sql + "  update_date, ";
                sql = sql + "  update_user_id ";
                sql = sql + ") VALUES ( ";
                sql = sql + " @ParamLogDivision, ";
                sql = sql + " CURRENT_TIMESTAMP, ";
                sql = sql + " @ParamStatus, ";
                sql = sql + " CURRENT_TIMESTAMP, ";
                sql = sql + " @UserId, ";
                sql = sql + " CURRENT_TIMESTAMP, ";
                sql = sql + " @UserId ";
                sql = sql + ")  ";

                int regFlg = db.Regist(
                    sql,
                    new
                    {
                        ParamLogDivision = paramLogDivision,
                        ParamStatus = paramStatus,
                        UserId = paramInfo.UserId,
                    });
                if (regFlg < 0)
                {
                    // ログメッセージ：　生産計画ログ　登録に失敗しました。
                    messageVal = comST.GetPropertiesMessage(new string[] { ComID.MB10034, ComID.MB10035 }, paramInfo.LanguageId);
                    logger.Error(messageVal);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // ログメッセージ：　生産計画ログ　登録に失敗しました。
                messageVal = comST.GetPropertiesMessage(new string[] {ComID.MB10034, ComID.MB10035 }, paramInfo.LanguageId);
                logger.Error(messageVal + "[RegisPlanLog]: " + ex.Message);
                return false;
            }
        }

        #endregion

    }
#endregion

}
