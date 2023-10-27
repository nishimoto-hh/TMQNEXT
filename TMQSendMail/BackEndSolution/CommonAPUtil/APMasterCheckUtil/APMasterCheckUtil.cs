using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonSTDUtil.CommonDBManager;
using CommonSTDUtil.Properties;
using CommonSTDUtil.CommonLogger;

using APConsts = APConstants.APConstants;
using APResources = CommonAPUtil.APCommonUtil.APResources;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using EntityDao = CommonAPUtil.APCommonUtil.APCommonDataClass;

namespace CommonAPUtil.APMasterCheckUtil
{
    /// <summary>
    /// APマスタチェッククラス
    /// </summary>
    public class APMasterCheckUtil
    {
        #region クラス内変数
        /// <summary>ログ出力</summary>
        private static CommonLogger logger = CommonLogger.GetInstance("logger");
        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public APMasterCheckUtil()
        {
        }
        #endregion

        /// <summary>
        /// マスタチェック 汎用共通処理
        /// </summary>
        /// <param name="inSqlString">実行SQL</param>
        /// <param name="inColumnName">チェックする項目名(メッセージ生成用)</param>
        /// <param name="outResultEntity">取得した</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="condition">条件</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool GetEntityCom(string inSqlString, string inColumnName, ref dynamic outResultEntity, ref string outErrMsg, object condition, ComDB db)
        {
            try
            {
                // DB接続
                if (!db.Connect())
                {
                    outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00317", db: db), inColumnName);
                    return false;
                }

                // データ取得
                outResultEntity = db.GetEntity<string>(inSqlString, condition);

                if (outResultEntity == null)
                {
                    outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00308", db: db), inColumnName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00317", db: db), inColumnName);
                return false;
            }

            return true;
        }

        /// <summary>
        /// エンティティ取得 汎用
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="inTableName">テーブル名</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="inKey">プライマリーキーのリスト</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>dynamic 取得したレコード１件</returns>
        public static T GetEntity<T>(string inTableName, ref string outErrMsg, string[] inKey, ComDB db)
        {
            try
            {
                // DB接続
                if (!db.Connect())
                {
                    outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00317", db: db), inTableName);
                    return default(T);
                }

                // SQL生成
                string inSqlKey = " select ccu.column_name as column_name from information_schema.table_constraints tb_con ";
                inSqlKey = inSqlKey + " inner join information_schema.constraint_column_usage ccu on ";
                inSqlKey = inSqlKey + "     tb_con.constraint_catalog = ccu.constraint_catalog ";
                inSqlKey = inSqlKey + " and tb_con.constraint_schema = ccu.constraint_schema ";
                inSqlKey = inSqlKey + " and tb_con.constraint_name = ccu.constraint_name ";
                inSqlKey = inSqlKey + " inner join information_schema.key_column_usage kcu on ";
                inSqlKey = inSqlKey + "     tb_con.constraint_catalog = kcu.constraint_catalog ";
                inSqlKey = inSqlKey + " and tb_con.constraint_schema = kcu.constraint_schema ";
                inSqlKey = inSqlKey + " and tb_con.constraint_name = kcu.constraint_name ";
                inSqlKey = inSqlKey + " and ccu.column_name = kcu.column_name ";
                inSqlKey = inSqlKey + " where 1 = 1 ";         // DB名
                inSqlKey = inSqlKey + " and tb_con.table_name = @TableName ";              // テーブル名
                inSqlKey = inSqlKey + " and tb_con.constraint_type = 'PRIMARY KEY' "; // 「PRIMARY KEY」は大文字
                inSqlKey = inSqlKey + " order by tb_con.table_catalog, tb_con.table_name, tb_con.constraint_name, kcu.ordinal_position ";
                //inSqlKey = string.Format(inSqlKey, inTableName);

                // プライマリーキー取得
                IList<string> keyList = db.GetList<string>(inSqlKey, new { TableName = inTableName });

                if ((keyList == null) || (keyList.Count != inKey.Count()))
                {
                    outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00317", db: db), inTableName);
                    return default(T);
                }

                // 引数の条件と、共通データを統合し、条件を再作成する
                dynamic param = new System.Dynamic.ExpandoObject();
                // 検索結果クラスのプロパティを列挙
                var properties = typeof(T).GetProperties();

                // SQL生成
                string inSqlEntity = " select * from " + inTableName + " where 1 = 1 ";
                for (int i = 0; keyList.Count > i; i++)
                {
                    //inSqlEntity = inSqlEntity + " and " + keyList[i] + " = '" + inKey[i] + "' ";
                    //((IDictionary<string, object>)param).Add(ComUtil.SnakeCaseToPascalCase(keyList[i]), inKey[i]);
                    inSqlEntity = inSqlEntity + " and " + keyList[i] + " = @" + ComUtil.SnakeCaseToPascalCase(keyList[i]) + " ";
                    setValueToClassCon(properties, keyList[i], inKey[i], ref param);
                }

                // エンティティ取得
                return db.GetEntity<T>(inSqlEntity, param);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00317", db: db), inTableName);
                return default(T);
            }
        }

        /// <summary>
        /// データクラスに値を設定する
        /// </summary>
        /// <param name="properties">プロパティ</param>
        /// <param name="key">キー情報</param>
        /// <param name="val">設定値</param>
        /// <param name="condition">条件データ</param>
        private static void setValueToClassCon(System.Reflection.PropertyInfo[] properties, string key, object val, ref object condition)
        {
            if (properties == null || properties.Count() == 0)
            {
                return;
            }

            foreach (var property in properties)
            {
                // 該当のキー情報の場合、処理を実施
                if (property.Name.Equals(ComUtil.SnakeCaseToPascalCase(key)))
                {
                    setPropertyValue(property, ComUtil.SnakeCaseToPascalCase(key), condition, val);
                    break;
                }
            }
            return;
        }

        /// <summary>
        /// 指定クラスのプロパティの型に値を変換して設定する
        /// </summary>
        /// <param name="prop">プロパティ情報</param>
        /// <param name="key">対象キー</param>
        /// <param name="param">条件</param>
        /// <param name="val">設定値</param>
        private static void setPropertyValue(System.Reflection.PropertyInfo prop, string key, object param, object val)
        {
            if (ComUtil.IsNullOrEmpty(val))
            {
                ((IDictionary<string, object>)param).Add(key, null);
                return;
            }

            if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
            {
                decimal tmpVal = Math.Round(decimal.Parse(val.ToString()));
                ((IDictionary<string, object>)param).Add(key, tmpVal);
            }
            else if (prop.PropertyType == typeof(short) || prop.PropertyType == typeof(short?))
            {
                decimal tmpVal = Math.Round(decimal.Parse(val.ToString()));
                ((IDictionary<string, object>)param).Add(key, tmpVal);
            }
            else if (prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(byte?))
            {
                decimal tmpVal = Math.Round(decimal.Parse(val.ToString()));
                ((IDictionary<string, object>)param).Add(key, tmpVal);
            }
            else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
            {
                decimal tmpVal = Math.Round(decimal.Parse(val.ToString()));
                ((IDictionary<string, object>)param).Add(key, tmpVal);
            }
            else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
            {
                ((IDictionary<string, object>)param).Add(key, Convert.ToDecimal(val));
            }
            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
            {
                ((IDictionary<string, object>)param).Add(key, Convert.ToDateTime(val));
            }
            else if (prop.PropertyType == typeof(string))
            {
                ((IDictionary<string, object>)param).Add(key, val.ToString());
            }
            else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            {
                ((IDictionary<string, object>)param).Add(key, Convert.ToBoolean(val));
            }
            else
            {
                ((IDictionary<string, object>)param).Add(key, val);
            }
        }

        /// <summary>
        /// テーブル主キー項目の一覧抽出 ★
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="inTableName">テーブル名</param>
        /// <param name="whereList">条件リスト</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>dynamic 取得した主キーレコードn件</returns>
        public static IList<T> GetKeyList<T>(string inTableName, Dictionary<string, List<string>> whereList, ref string outErrMsg, ComDB db)
        {
            IList<T> rtnList = null;
            try
            {
                // DB接続
                if (!db.Connect())
                {
                    outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00317", db: db), inTableName);
                    return rtnList;
                }

                // SQL生成
                string inSqlKey = " select ccu.column_name as column_name from information_schema.table_constraints tb_con ";
                inSqlKey = inSqlKey + " inner join information_schema.constraint_column_usage ccu on ";
                inSqlKey = inSqlKey + "     tb_con.constraint_catalog = ccu.constraint_catalog ";
                inSqlKey = inSqlKey + " and tb_con.constraint_schema = ccu.constraint_schema ";
                inSqlKey = inSqlKey + " and tb_con.constraint_name = ccu.constraint_name ";
                inSqlKey = inSqlKey + " inner join information_schema.key_column_usage kcu on ";
                inSqlKey = inSqlKey + "     tb_con.constraint_catalog = kcu.constraint_catalog ";
                inSqlKey = inSqlKey + " and tb_con.constraint_schema = kcu.constraint_schema ";
                inSqlKey = inSqlKey + " and tb_con.constraint_name = kcu.constraint_name ";
                inSqlKey = inSqlKey + " and ccu.column_name = kcu.column_name ";
                inSqlKey = inSqlKey + " where 1 = 1 ";         // DB名
                inSqlKey = inSqlKey + " and tb_con.table_name = @TableName ";              // テーブル名
                inSqlKey = inSqlKey + " and tb_con.constraint_type = 'PRIMARY KEY' "; // 「PRIMARY KEY」は大文字
                inSqlKey = inSqlKey + " order by tb_con.table_catalog, tb_con.table_name, tb_con.constraint_name, kcu.ordinal_position ";
                //inSqlKey = string.Format(inSqlKey, inTableName.ToLower());

                // プライマリーキー取得
                IList<string> keyList = db.GetList<string>(inSqlKey, new { TableName = inTableName });

                if (keyList == null)
                {
                    outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00317", db: db), inTableName);
                    return rtnList;
                }

                // SQL生成
                string inSqlEntity = "select ";
                for (int i = 0; keyList.Count > i; i++)
                {
                    if (i != 0)
                    {
                        inSqlEntity += ",";
                    }
                    inSqlEntity = inSqlEntity + keyList[i];
                }
                inSqlEntity += " from " + inTableName;

                // 引数項目を共通データへ置換するインデックス。
                int z = 1;

                // 引数の条件と、共通データを統合し、条件を再作成する
                dynamic param = new System.Dynamic.ExpandoObject();
                // 検索結果クラスのプロパティを列挙
                var properties = typeof(T).GetProperties();

                //条件リストが設定されていれば条件式を設定する。
                if (whereList.Count != 0)
                {
                    // 条件表作成
                    Dictionary<string, string> likeList = new Dictionary<string, string>();
                    foreach (var list in whereList)
                    {
                        foreach (var val in list.Value)
                        {
                            if (likeList.ContainsKey(list.Key))
                            {
                                string values = likeList[list.Key];
                                likeList.Remove(list.Key);
                                likeList.Add(list.Key, values + ", @" + z);
                                // 1からの連番を付与するため既存の設定方法から拡張する。
                                foreach (var property in properties)
                                {
                                    // 該当のキー情報の場合、処理を実施
                                    if (property.Name.Equals(ComUtil.SnakeCaseToPascalCase(list.Key)))
                                    {
                                        setPropertyValue(property, ComUtil.SnakeCaseToPascalCase(z.ToString()), param, val);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                likeList.Add(list.Key, "@" + z);
                                // 1からの連番を付与するため既存の設定方法から拡張する。
                                foreach (var property in properties)
                                {
                                    // 該当のキー情報の場合、処理を実施
                                    if (property.Name.Equals(ComUtil.SnakeCaseToPascalCase(list.Key)))
                                    {
                                        setPropertyValue(property, ComUtil.SnakeCaseToPascalCase(z.ToString()), param, val);
                                        break;
                                    }
                                }
                            }
                            z++;
                        }
                    }
                    // 条件表を条件に置換する。
                    int x = 0;
                    foreach (var kvp in likeList)
                    {
                        if (x == 0)
                        {
                            if (kvp.Value.Split(',').Count() > 1)
                            {
                                inSqlEntity += " where " + kvp.Key + " in (" + kvp.Value + ") ";
                            }
                            else
                            {
                                inSqlEntity += " where " + kvp.Key + " = " + kvp.Value;
                            }
                            x++;
                        }
                        else
                        {
                            if (kvp.Value.Split(',').Count() > 1)
                            {
                                inSqlEntity += " and " + kvp.Key + " in (" + kvp.Value + ") ";
                            }
                            else
                            {
                                inSqlEntity += " and " + kvp.Key + " = " + kvp.Value;
                            }
                        }

                    }
                }
                // エンティティ取得
                return db.GetList<T>(inSqlEntity, param);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00317", db: db), inTableName);
                return rtnList;
            }
        }

        // ※テーブル定義書に該当のテーブルが存在しないため、コメントアウト
        ///// <summary>
        ///// マスタ存在チェック. 設備グループマスタ
        ///// </summary>
        ///// <param name="iResouceGroupCd">設備コード</param>
        ///// <param name="oErrMsg">エラーメッセージ</param>
        ///// <returns>bool true:OK, false:NG</returns>
        //public static bool CheckRecipeResouceGroup(string iResouceGroupCd, ref string oErrMsg, ComDB db)
        //{
        //    dynamic result = GetEntity("recipe_resouce_group", ref oErrMsg, new string[] { iResouceGroupCd }, db);

        //    if (result == null)
        //    {
        //        oErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00988" }, db: db);
        //        return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// マスタ存在チェック. ロケーションマスタ
        /// </summary>
        /// <param name="inLocationCd">ロケーションコード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="assetDivision">資産管理区分(APConstants.ASSET_DIVISION)、省略時は「帳簿内(自社在庫)」</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckLocation(string inLocationCd, ref string outErrMsg, ComDB db, string assetDivision = null)
        {
            if (string.IsNullOrEmpty(assetDivision))
            {
                // デフォルト値に定数を設定できないため、こちらでセット
                assetDivision = APConsts.ASSET_DIVISION.INSIDE_ACCOUNT_COMPANY;
            }

            EntityDao.LocationEntity result = GetEntity<EntityDao.LocationEntity>("location", ref outErrMsg, new string[] { inLocationCd }, db);
            if (result == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I10106" }, db: db);
                return false;
            }
            if (result.DelFlg == 1)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I10106" }, db: db);
                return false;
            }
            if (result.AssetDivision != assetDivision)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M10217" }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. ロケーションマスタ（在庫可能）
        /// </summary>
        /// <param name="inLocationCd">ロケーションコード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="assetDivision">資産管理区分(APConstants.ASSET_DIVISION)、省略時は「帳簿内(自社在庫)」</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckAvailableLocation(string inLocationCd, ref string outErrMsg, ComDB db, string assetDivision = null)
        {
            if (string.IsNullOrEmpty(assetDivision))
            {
                // デフォルト値に定数を設定できないため、こちらでセット
                assetDivision = APConsts.ASSET_DIVISION.INSIDE_ACCOUNT_COMPANY;
            }

            // 項目名設定
            string columnName = ComUtil.GetPropertiesMessage(key: "I10106", db: db);
            dynamic resultEntity = null;

            // SQL文生成
            string inSqlString = " select * from location where location_cd = @LocationCd and coalesce(available_flg, 1) = 2 and asset_division = @AssetDivision and del_flg != 1";

            // DB接続
            if (!db.Connect())
            {
                outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00317", db: db), columnName);
                return false;
            }

            // データ取得
            resultEntity = db.GetEntity<string>(inSqlString, new { LocationCd = inLocationCd, AssetDivision = assetDivision });

            if (resultEntity == null)
            {
                outErrMsg = string.Format(ComUtil.GetPropertiesMessage(key: "M00308", db: db), columnName);
                return false;
            }

            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 各種名称マスタ
        /// </summary>
        /// <param name="inNameCd">名称コード</param>
        /// <param name="inNameDivision">名称区分</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="messageKey">エラーメッセージに表示する項目名</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckNames(string inNameCd, string inNameDivision, ref string outErrMsg, string messageKey, ComDB db)
        {
            EntityDao.NamesEntity result = GetEntity<EntityDao.NamesEntity>("names", ref outErrMsg, new string[] { inNameDivision, inNameCd }, db);

            if (result == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", messageKey }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 取引先マスタ
        /// </summary>
        /// <param name="inVenderDivision">取引先区分(TS:得意先,SI:仕入先)</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckVender(string inVenderDivision, string inVenderCd, ref string outErrMsg, ComDB db)
        {
            // SQL文生成
            string sql = "";
            sql = sql + " select ";
            sql = sql + "     * ";
            sql = sql + " from ";
            sql = sql + "     v_vender_regist ";
            sql = sql + " where ";
            sql = sql + "     vender_division = @iVenderDivision ";
            sql = sql + " and vender_cd = @iVenderCd ";
            sql = sql + " and del_flg != 1 ";

            object param = null;
            param = new { iVenderDivision = inVenderDivision, iVenderCd = inVenderCd };

            // 検索実行
            EntityDao.VenderEntity results = db.GetEntity<EntityDao.VenderEntity>(sql, param);

            if (results == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00611" }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 外注先マスタ
        /// </summary>
        /// <param name="inVenderDivision">取引先区分(TS:得意先,SI:仕入先)</param>
        /// <param name="inGiDivision">外注先区分(1:外注先対象、2：外注先対象外)</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckOutsource(string inVenderDivision, string inGiDivision, string inVenderCd, ref string outErrMsg, ComDB db)
        {
            // 項目名設定
            string columnName = ComUtil.GetPropertiesMessage(key: "I02061", db: db);
            dynamic resultEntity = null;

            // SQL文生成
            string inSqlString = " select * from vender where vender_cd is not null and vender_division = @VenderDivision and vender_cd = @VenderCd and gi_division = @GiDivision ";
            //inSqlString = string.Format(inSqlString, inVenderDivision, inVenderCd, inGiDivision);

            return GetEntityCom(inSqlString, columnName, ref resultEntity, ref outErrMsg, new { VenderDivision = inVenderDivision, VenderCd = inVenderCd, GiDivision = inGiDivision }, db);
        }

        /// <summary>
        /// マスタ存在チェック. 納入先マスタ
        /// </summary>
        /// <param name="inDeliveryCd">納入先コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckDelivery(string inDeliveryCd, ref string outErrMsg, ComDB db)
        {
            EntityDao.DeliveryEntity result = GetEntity<EntityDao.DeliveryEntity>("delivery", ref outErrMsg, new string[] { inDeliveryCd }, db);
            if (result == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00737" }, db: db);
                return false;
            }
            if (result.DelFlg == 1)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00737" }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 部署マスタ
        /// </summary>
        /// <param name="inOrganizationCd">部署コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckOrganizationMaster(string inOrganizationCd, ref string outErrMsg, ComDB db)
        {
            EntityDao.OrganizationMasterEntity result = GetEntity<EntityDao.OrganizationMasterEntity>("organization_master", ref outErrMsg, new string[] { inOrganizationCd }, db);
            if (result == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00782" }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 担当者マスタ
        /// </summary>
        /// <param name="inLoginCd">担当者コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckLogin(string inLoginCd, ref string outErrMsg, ComDB db)
        {
            // 項目名設定
            string columnName = ComUtil.GetPropertiesMessage(key: "I00703", db: db);
            dynamic resultEntity = null;

            // SQL文生成
            //string iSqlString = " select * from login where tanto_cd = '{0}' and active_flg = '1' and delete_flg = '0' ";
            string inSqlString = " select * from login where user_id = @UserId and active_flg = '1' and del_flg = '0' ";
            //inSqlString = string.Format(inSqlString, inLoginCd);

            return GetEntityCom(inSqlString, columnName, ref resultEntity, ref outErrMsg, new { UserId = inLoginCd }, db);
        }

        /// <summary>
        /// マスタ存在チェック. 品目マスタ
        /// </summary>
        /// <param name="inItemCd">品目コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckItem(string inItemCd, ref string outErrMsg, ComDB db)
        {
            // SQL文生成
            string sql = "";
            sql = sql + " select ";
            sql = sql + "     * ";
            sql = sql + " from ";
            sql = sql + "     v_item_regist ";
            sql = sql + " where ";
            sql = sql + "     item_cd = @iItemCd ";

            object param = null;
            param = new { iItemCd = inItemCd };

            // 検索実行
            EntityDao.ItemEntity results = db.GetEntity<EntityDao.ItemEntity>(sql, param);

            if (results == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00776" }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 品目マスタ
        /// </summary>
        /// <param name="inItemCd">品目コード</param>
        /// <param name="inLanguageId">言語ID</param>
        /// <param name="db">DB接続</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckItemMaxVersion(string inItemCd, string inLanguageId, ComDB db)
        {
            // SQL文生成
            string sql = "";
            sql = sql + "select item.item_cd ";
            sql = sql + "  from v_item_regist item ";
            sql = sql + " where item.item_cd = @ItemCd ";
            sql = sql + "   and item.language_id = @LanguageId";

            object param = null;
            param = new { ItemCd = inItemCd, LanguageId = inLanguageId };

            // 検索実行
            EntityDao.ItemEntity results = db.GetEntity<EntityDao.ItemEntity>(sql, param);

            if (results != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// マスタ存在チェック. 品目マスタ
        /// </summary>
        /// <param name="inItemCd">品目コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="date">日付</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckItem(string inItemCd, ref string outErrMsg, DateTime date, ComDB db)
        {
            // SQL文生成
            string sql = "";
            sql = sql + " select ";
            sql = sql + "     * ";
            sql = sql + " from ";
            sql = sql + "     v_item_regist ";
            sql = sql + " where ";
            sql = sql + "     item_cd = @iItemCd ";

            object param = null;
            param = new { iItemCd = inItemCd };

            // 検索実行
            EntityDao.ItemEntity results = db.GetEntity<EntityDao.ItemEntity>(sql, param);

            if (results == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00776" }, db: db);
                return false;
            }
            if (results.ActiveDate > date)
            {
                // 仕入日付が有効開始日より過去の場合エラー
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 仕様マスタ
        /// </summary>
        /// <param name="inItemCd">品目コード</param>
        /// <param name="inItemSpecCd">仕様コード</param>
        /// <param name="inLanguageId">言語ID</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckItemSpec(string inItemCd, string inItemSpecCd, string inLanguageId, ref string outErrMsg, ComDB db)
        {
            // SQL文生成
            string sql = "";
            sql = sql + " select ";
            sql = sql + "     * ";
            sql = sql + " from ";
            sql = sql + "     v_item_regist item ";
            sql = sql + "  inner join ";
            sql = sql + "     v_item_specification_regist item_spec ";
            sql = sql + "   on ";
            sql = sql + "     ( item.item_cd = item_spec.item_cd ";
            sql = sql + "   and item.active_date = item_spec.active_date ";
            sql = sql + "   and item.language_id = item_spec.language_id )";
            sql = sql + " where ";
            sql = sql + "     item.item_cd = @iItemCd ";
            sql = sql + " and item_spec.specification_cd = @iSpecificationCd ";
            sql = sql + " and item_spec.status = '3' ";
            sql = sql + " and item.language_id = @iLanguageId  ";

            object param = null;
            param = new { iItemCd = inItemCd, iSpecificationCd = inItemSpecCd, iLanguageId = inLanguageId };

            // 検索実行
            EntityDao.ItemEntity results = db.GetEntity<EntityDao.ItemEntity>(sql, param);

            if (results == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00594" }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 仕様マスタ
        /// </summary>
        /// <param name="inItemCd">品目コード</param>
        /// <param name="inItemSpecCd">仕様コード</param>
        /// <param name="inLanguageId">言語ID</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="date">日付</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckItemSpec(string inItemCd, string inItemSpecCd, string inLanguageId, ref string outErrMsg, DateTime date, ComDB db)
        {
            // SQL文生成
            string sql = "";
            sql = sql + " select ";
            sql = sql + "     * ";
            sql = sql + " from ";
            sql = sql + "     v_item_regist item ";
            sql = sql + "  inner join ";
            sql = sql + "     v_item_specification_regist item_spec ";
            sql = sql + "   on ";
            sql = sql + "     ( item.item_cd = item_spec.item_cd ";
            sql = sql + "   and item.active_date = item_spec.active_date ";
            sql = sql + "   and item.language_id = item_spec.language_id )";
            sql = sql + " where ";
            sql = sql + "     item.item_cd = @iItemCd ";
            sql = sql + " and item_spec.specification_cd = @iSpecificationCd ";
            sql = sql + " and item_spec.status = '3' ";
            sql = sql + " and item.language_id = @iLanguageId  ";

            object param = null;
            param = new { iItemCd = inItemCd, iSpecificationCd = inItemSpecCd, iLanguageId = inLanguageId };

            // 検索実行
            EntityDao.ItemSpecificationEntity results = db.GetEntity<EntityDao.ItemSpecificationEntity>(sql, param);

            if (results == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00594" }, db: db);
                return false;
            }
            if (results.SpecificationActiveDate > date)
            {
                // 仕入日付が有効開始日より過去の場合エラー
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 科目マスタ
        /// </summary>
        /// <param name="inAccountsDivision">勘定コード区分</param>
        /// <param name="inAccountsCd">科目コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inDelflg">削除フラグを確認する</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckAccounts(string inAccountsDivision, string inAccountsCd, ref string outErrMsg, ComDB db, bool inDelflg = false)
        {
            EntityDao.AccountsEntity result = GetEntity<EntityDao.AccountsEntity>("accounts", ref outErrMsg, new string[] { inAccountsDivision, inAccountsCd }, db);
            bool delflg = false;
            if (inDelflg)
            {
                if (result != null && result.DelFlg.Equals(1))
                {
                    delflg = true; // 削除
                }
            }
            if (result == null || delflg)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00539" }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 銀行マスタ
        /// </summary>
        /// <param name="inBankMasterCd">銀行マスタコード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckBank(string inBankMasterCd, ref string outErrMsg, ComDB db)
        {
            // 項目名設定
            string columnName = ComUtil.GetPropertiesMessage(key: "I00119", db: db);
            dynamic resultEntity = null;

            // SQL文生成
            string inSqlString = " select * from bank where bank_master_cd = @BankMasterCd ";
            //inSqlString = string.Format(inSqlString, inBankMasterCd);

            return GetEntityCom(inSqlString, columnName, ref resultEntity, ref outErrMsg, new { BankMasterCd = inBankMasterCd }, db);
        }

        /// <summary>
        /// マスタ存在チェック. 帳合マスタ
        /// </summary>
        /// <param name="inBalanceCd">帳合コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckBalance(string inBalanceCd, ref string outErrMsg, ComDB db)
        {
            EntityDao.BalanceEntity result = GetEntity<EntityDao.BalanceEntity>("balance", ref outErrMsg, new string[] { inBalanceCd }, db);
            if (result == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I10002" }, db: db);
                return false;
            }
            return true;
        }

        // ※テーブル定義書に該当のテーブルが存在しないため、コメントアウト
        ///// <summary>
        ///// マスタ存在チェック. 翻訳マスタ
        ///// </summary>
        ///// <param name="iLanguageId">言語ID</param>
        ///// <param name="iTransrationId">翻訳ID</param>
        ///// <param name="oErrMsg">エラーメッセージ</param>
        ///// <returns>bool true:OK, false:NG</returns>
        //public static bool CheckTransration(string iLanguageId, string iTransrationId, ref string oErrMsg, ComDB db)
        //{
        //    dynamic result = GetEntity("attr_transration", ref oErrMsg, new string[] { iLanguageId, iTransrationId }, db);
        //    if (result == null)
        //    {
        //        oErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00477" }, db: db);
        //        return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// マスタ存在チェック. 工程マスタ
        /// </summary>
        /// <param name="inOperationCd">工程コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckOperation(string inOperationCd, ref string outErrMsg, ComDB db)
        {
            EntityDao.OperationEntity result = GetEntity<EntityDao.OperationEntity>("operation", ref outErrMsg, new string[] { inOperationCd }, db);
            if (result == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00573" }, db: db);
                return false;
            }
            return true;
        }

        // ※テーブル定義書に該当のテーブルが存在しないため、コメントアウト
        ///// <summary>
        ///// マスタ存在チェック. 工程グループマスタ
        ///// </summary>
        ///// <param name="iOperationGroupCd">工程グループコード</param>
        ///// <param name="oErrMsg">エラーメッセージ</param>
        ///// <returns>bool true:OK, false:NG</returns>
        //public static bool CheckOperationGroup(string iOperationGroupCd, ref string oErrMsg, ComDB db)
        //{
        //    dynamic result = GetEntity("operation_group", ref oErrMsg, new string[] { iOperationGroupCd }, db);
        //    if (result == null)
        //    {
        //        oErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00141" }, db: db);
        //        return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// マスタ存在チェック. 生産ラインマスタ
        /// </summary>
        /// <param name="inProductionLine">生産ラインコード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckLine(string inProductionLine, ref string outErrMsg, ComDB db)
        {
            EntityDao.LineEntity result = GetEntity<EntityDao.LineEntity>("line", ref outErrMsg, new string[] { inProductionLine }, db);
            if (result == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00312" }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 会計部門マスタ
        /// </summary>
        /// <param name="inSectionCd">部門コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inDelflg">削除フラグを確認する</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckBumon(string inSectionCd, ref string outErrMsg, ComDB db, bool inDelflg = false)
        {
            EntityDao.BumonEntity result = GetEntity<EntityDao.BumonEntity>("bumon", ref outErrMsg, new string[] { inSectionCd }, db);
            bool delflg = false;
            if (inDelflg)
            {
                if (result != null && result.DelFlg.Equals(1))
                {
                    delflg = true; // 削除
                }
            }
            if (result == null || delflg)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00469" }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. ロケーションマスタ(地区指定)
        /// </summary>
        /// <param name="inLocationCd">ロケーションコード</param>
        /// <param name="inAreaCd">地区コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckAreaLocation(string inLocationCd, string inAreaCd, ref string outErrMsg, ComDB db)
        {
            // 項目名設定
            string columnName = ComUtil.GetPropertiesMessage(key: "I10106", db: db);
            dynamic resultEntity = null;

            // SQL文生成
            string inSqlString = " select * from location where location_cd = @LocationCd and area_cd = @AreaCd ";
            //inSqlString = string.Format(inSqlString, inLocationCd, inAreaCd);

            return GetEntityCom(inSqlString, columnName, ref resultEntity, ref outErrMsg, new { LocationCd = inLocationCd, AreaCd = inAreaCd }, db);
        }

        /// <summary>
        /// マスタ存在チェック. ロケーションマスタ（「地区コード：null」対応）
        /// </summary>
        /// <param name="inLocationCd">ロケーションコード</param>
        /// <param name="inAreaCd">地区コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckAreaLocationForAreaNull(string inLocationCd, string inAreaCd, ref string outErrMsg, ComDB db)
        {
            if (!string.IsNullOrEmpty(inAreaCd))
            {
                return CheckAreaLocation(inLocationCd, inAreaCd, ref outErrMsg, db);    // 地区コード指定有り
            }
            else
            {
                return CheckLocation(inLocationCd, ref outErrMsg, db);                 // 地区コード指定なし
            }
        }

        /// <summary>
        /// マスタ存在チェック. 理由マスタ
        /// </summary>
        /// <param name="inRyCd">理由コード</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckReason(string inRyCd, ref string outErrMsg, ComDB db)
        {
            EntityDao.ReasonEntity result = GetEntity<EntityDao.ReasonEntity>("reason", ref outErrMsg, new string[] { inRyCd }, db);
            if (result == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "M00303" }, db: db);
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 取引先別単価マスタ(販売系)
        /// </summary>
        /// <param name="inBalanceCd">帳合コード</param>
        /// <param name="inItemCd">品目コード</param>
        /// <param name="inSpecificationCd">仕様コード</param>
        /// <param name="inValidDate">日付</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckUnitpriceForSales(string inBalanceCd, string inItemCd, string inSpecificationCd, string inValidDate, ref string outErrMsg, ComDB db)
        {
            // 自社マスタ 単価絞込み区分取得
            EntityDao.CompanyEntity resultCompany = GetEntity<EntityDao.CompanyEntity>("company", ref outErrMsg, new string[] { "000001" }, db);
            if (resultCompany == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00219" }, db: db);
                return false;
            }

            if (resultCompany.SaleUnitpriceDivision != null && resultCompany.SaleUnitpriceDivision == 1)
            {
                // SQL生成
                string inSqlList = " select * from unitprice ";
                inSqlList = inSqlList + " where balance_cd = @BalanceCd and item_cd = @ItemCd and specification_cd = @SpecificationCd and vender_division = 'TS' ";
                inSqlList = inSqlList + " and valid_date <= @ValidDate and approval_status = '3'  -- 承認済 ";
                //inSqlList = string.Format(inSqlList, inBalanceCd, inItemCd, inSpecificationCd, inValidDate);

                // 単価マスタ取得
                IList<EntityDao.UnitpriceEntity> priceList = db.GetList<EntityDao.UnitpriceEntity>(inSqlList,
                    new { BalanceCd = inBalanceCd, ItemCd = inItemCd, SpecificationCd = inSpecificationCd, ValidDate = ComUtil.ConvertDateTime(inValidDate) ?? DateTime.Now });

                if (priceList == null || priceList.Count() <= 0)
                {
                    outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00306", "I02247", "I00592" }, db: db);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// マスタ存在チェック. 取引先別単価マスタ(購買系)
        /// </summary>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="inItemCd">品目コード</param>
        /// <param name="inSpecificationCd">仕様コード</param>
        /// <param name="inValidDate">日付</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckUnitpriceForPurchase(string inVenderCd, string inItemCd, string inSpecificationCd, string inValidDate, ref string outErrMsg, ComDB db)
        {
            // 自社マスタ 単価絞込み区分取得
            EntityDao.CompanyEntity resultCompany = GetEntity<EntityDao.CompanyEntity>("company", ref outErrMsg, new string[] { "000001" }, db);
            if (resultCompany == null)
            {
                outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00308", "I00219" }, db: db);
                return false;
            }

            if (resultCompany.PurchaseUnitpriceDivision != null && resultCompany.PurchaseUnitpriceDivision == 1)
            {
                // SQL生成
                string inSqlList = " select * from unitprice ";
                inSqlList = inSqlList + " where balance_cd = @BalanceCd and item_cd = @ItemCd and specification_cd = @SpecificationCd and vender_division = 'SI' ";
                inSqlList = inSqlList + " and valid_date <= @ValidDate and approval_status = '3'  -- 承認済 ";
                //inSqlList = string.Format(inSqlList, inVenderCd, inItemCd, inSpecificationCd, inValidDate);

                // 単価マスタ取得
                IList<EntityDao.UnitpriceEntity> priceList = db.GetList<EntityDao.UnitpriceEntity>(inSqlList,
                    new { BalanceCd = inVenderCd, ItemCd = inItemCd, SpecificationCd = inSpecificationCd, ValidDate = ComUtil.ConvertDateTime(inValidDate) ?? DateTime.Now });

                if (priceList == null || priceList.Count() <= 0)
                {
                    outErrMsg = ComUtil.GetPropertiesMessage(keys: new string[] { "M00306", "I02247", "I00592" }, db: db);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// マスタ存在チェック. カレンダーマスタ
        /// </summary>
        /// <param name="inCalCd">カレンダーコード</param>
        /// <param name="inLanguageId">言語ID</param>
        /// <param name="outErrMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool CheckCal(string inCalCd, string inLanguageId, ref string outErrMsg, ComDB db)
        {
            // 項目名設定
            string columnName = ComUtil.GetPropertiesMessage(key: "I00034", db: db);
            dynamic resultEntity = null;

            // SQL文生成
            string inSqlString = " select cal_cd from (select * from v_cal where language_id = @LanguageId) cal where cal_cd = @CalCd group by cal_cd ";
            //inSqlString = string.Format(inSqlString, inCalCd, inLanguageId);
            object condition = new { LanguageId = inLanguageId, CalCd = inCalCd };

            return GetEntityCom(inSqlString, columnName, ref resultEntity, ref outErrMsg, condition, db);
        }
    }
}
