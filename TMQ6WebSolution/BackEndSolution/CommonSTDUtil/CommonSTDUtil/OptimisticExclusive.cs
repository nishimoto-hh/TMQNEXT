using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using static CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonSTDUtil.CommonSTDUtil
{
    /// <summary>
    /// 楽観排他クラス
    /// </summary>
    public class OptimisticExclusive
    {
        ///// <summary>DB操作クラス</summary>
        //protected CommonDBManager.CommonDBManager db;

        ///// <summary>
        ///// 楽観ロックデータチェック
        ///// </summary>
        ///// <param name="jsonLockData">json形式ロックデータ</param>
        ///// <returns>true:OK false:NG</returns>
        //public bool CheckLockData(string jsonLockData)
        //{
        //    // テーブル名
        //    string tableName = string.Empty;
        //    // ロックキー項目
        //    string lockKeyItem = string.Empty;
        //    // ロックキー値
        //    string lockKeyValue = string.Empty;
        //    // プライマリキー1～10
        //    string primaryKeyItem1 = string.Empty;
        //    string primaryKeyItem2 = string.Empty;
        //    string primaryKeyItem3 = string.Empty;
        //    string primaryKeyItem4 = string.Empty;
        //    string primaryKeyItem5 = string.Empty;
        //    string primaryKeyItem6 = string.Empty;
        //    string primaryKeyItem7 = string.Empty;
        //    string primaryKeyItem8 = string.Empty;
        //    string primaryKeyItem9 = string.Empty;
        //    string primaryKeyItem10 = string.Empty;
        //    // プライマリキー値1～10
        //    string primaryKeyValue1 = string.Empty;
        //    string primaryKeyValue2 = string.Empty;
        //    string primaryKeyValue3 = string.Empty;
        //    string primaryKeyValue4 = string.Empty;
        //    string primaryKeyValue5 = string.Empty;
        //    string primaryKeyValue6 = string.Empty;
        //    string primaryKeyValue7 = string.Empty;
        //    string primaryKeyValue8 = string.Empty;
        //    string primaryKeyValue9 = string.Empty;
        //    string primaryKeyValue10 = string.Empty;
        //    // ディクショナリ
        //    List<Dictionary<string, string>> dic = new List<Dictionary<string, string>>();

        //    if (!string.IsNullOrEmpty(jsonLockData))
        //    {
        //        // JSON文字列の実行条件をデシリアライズ
        //        var serializer = new JavaScriptSerializer();
        //        dic = serializer.Deserialize<List<Dictionary<string, string>>>(jsonLockData);
        //    }

        //    this.db = new CommonDBManager.CommonDBManager();
        //    if (!this.db.Connect())
        //    {
        //        // エラーログを出力する。
        //        //TODO 後で
        //        // 戻り値 = false を返却する。
        //        return false;
        //    }
        //    for (int i = 0; i < dic.Count; i++)
        //    {
        //        /////////////
        //        // 初期化
        //        /////////////

        //        // テーブル名
        //        tableName = string.Empty;
        //        // ロックキー項目
        //        lockKeyItem = string.Empty;
        //        // ロックキー値
        //        lockKeyValue = string.Empty;
        //        // プライマリキー1～10
        //        primaryKeyItem1 = string.Empty;
        //        primaryKeyItem2 = string.Empty;
        //        primaryKeyItem3 = string.Empty;
        //        primaryKeyItem4 = string.Empty;
        //        primaryKeyItem5 = string.Empty;
        //        primaryKeyItem6 = string.Empty;
        //        primaryKeyItem7 = string.Empty;
        //        primaryKeyItem8 = string.Empty;
        //        primaryKeyItem9 = string.Empty;
        //        primaryKeyItem10 = string.Empty;
        //        // プライマリキー値1～10
        //        primaryKeyValue1 = string.Empty;
        //        primaryKeyValue2 = string.Empty;
        //        primaryKeyValue3 = string.Empty;
        //        primaryKeyValue4 = string.Empty;
        //        primaryKeyValue5 = string.Empty;
        //        primaryKeyValue6 = string.Empty;
        //        primaryKeyValue7 = string.Empty;
        //        primaryKeyValue8 = string.Empty;
        //        primaryKeyValue9 = string.Empty;
        //        primaryKeyValue10 = string.Empty;

        //        /////////////
        //        // 値を取得
        //        /////////////

        //        // テーブル名
        //        tableName = dic[i]["tableName"];
        //        // ロックキー項目
        //        lockKeyItem = dic[i]["lockKeyItem"];
        //        // プライマリキー1～10
        //        primaryKeyItem1 = dic[i]["primaryKeyItem1"];
        //        if (dic[i].ContainsKey("primaryKeyItem2"))
        //        {
        //            primaryKeyItem2 = dic[i]["primaryKeyItem2"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem3"))
        //        {
        //            primaryKeyItem3 = dic[i]["primaryKeyItem3"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem4"))
        //        {
        //            primaryKeyItem4 = dic[i]["primaryKeyItem4"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem5"))
        //        {
        //            primaryKeyItem5 = dic[i]["primaryKeyItem5"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem6"))
        //        {
        //            primaryKeyItem6 = dic[i]["primaryKeyItem6"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem7"))
        //        {
        //            primaryKeyItem7 = dic[i]["primaryKeyItem7"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem8"))
        //        {
        //            primaryKeyItem8 = dic[i]["primaryKeyItem8"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem9"))
        //        {
        //            primaryKeyItem9 = dic[i]["primaryKeyItem9"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem10"))
        //        {
        //            primaryKeyItem10 = dic[i]["primaryKeyItem10"];
        //        }

        //        // プライマリキー値1～10
        //        primaryKeyValue1 = dic[i]["primaryKeyValue1"];
        //        if (dic[i].ContainsKey("primaryKeyValue2"))
        //        {
        //            primaryKeyValue2 = dic[i]["primaryKeyValue2"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue3"))
        //        {
        //            primaryKeyValue3 = dic[i]["primaryKeyValue3"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue4"))
        //        {
        //            primaryKeyValue4 = dic[i]["primaryKeyValue4"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue5"))
        //        {
        //            primaryKeyValue5 = dic[i]["primaryKeyValue5"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue6"))
        //        {
        //            primaryKeyValue6 = dic[i]["primaryKeyValue6"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue7"))
        //        {
        //            primaryKeyValue7 = dic[i]["primaryKeyValue7"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue8"))
        //        {
        //            primaryKeyValue8 = dic[i]["primaryKeyValue8"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue9"))
        //        {
        //            primaryKeyValue9 = dic[i]["primaryKeyValue9"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue10"))
        //        {
        //            primaryKeyValue10 = dic[i]["primaryKeyValue10"];
        //        }

        //        /////////////
        //        // SQLを作成
        //        /////////////
        //        string sql = string.Empty;
        //        sql = sql + "SELECT ";
        //        sql = sql + "    " + lockKeyItem + " as lock_key_value ";
        //        sql = sql + "FROM ";
        //        sql = sql + "    " + tableName + " ";
        //        sql = sql + "WHERE 1 = 1 ";
        //        sql = sql + "AND " + primaryKeyItem1 + " = '" + primaryKeyValue1 + "' ";
        //        if (dic[i].ContainsKey("primaryKeyItem2"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem2 + " = '" + primaryKeyValue2 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem3"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem3 + " = '" + primaryKeyValue3 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem4"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem4 + " = '" + primaryKeyValue4 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem5"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem5 + " = '" + primaryKeyValue5 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem6"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem6 + " = '" + primaryKeyValue6 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem7"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem7 + " = '" + primaryKeyValue7 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem8"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem8 + " = '" + primaryKeyValue8 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem9"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem9 + " = '" + primaryKeyValue9 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem10"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem10 + " = '" + primaryKeyValue10 + "' ";
        //        }
        //        sql = sql + ";";

        //        /////////////
        //        // SQLを実行
        //        /////////////
        //        dynamic result = db.GetEntity(sql);

        //        /////////////
        //        // 実行結果を設定
        //        /////////////
        //        if (result == null)
        //        {
        //            return false;
        //        }
        //        DateTime buf = result.lock_key_value;
        //        lockKeyValue = buf.ToString();
        //        /////////////
        //        // 取得したロックキー値をディクショナリと比較する
        //        /////////////
        //        if (dic[i].ContainsKey("lockKeyValue"))
        //        {
        //            if (lockKeyValue.CompareTo(dic[i]["lockKeyValue"]) != 0)
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 楽観ロックデータ取得
        ///// </summary>
        ///// <param name="jsonLockData">json形式ロックデータ</param>
        ///// <returns>json形式ロックデータ</returns>
        //public string GetLockData(string jsonLockData)
        //{
        //    // テーブル名
        //    string tableName = string.Empty;
        //    // ロックキー項目
        //    string lockKeyItem = string.Empty;
        //    // ロックキー値
        //    string lockKeyValue = string.Empty;
        //    // プライマリキー1～10
        //    string primaryKeyItem1 = string.Empty;
        //    string primaryKeyItem2 = string.Empty;
        //    string primaryKeyItem3 = string.Empty;
        //    string primaryKeyItem4 = string.Empty;
        //    string primaryKeyItem5 = string.Empty;
        //    string primaryKeyItem6 = string.Empty;
        //    string primaryKeyItem7 = string.Empty;
        //    string primaryKeyItem8 = string.Empty;
        //    string primaryKeyItem9 = string.Empty;
        //    string primaryKeyItem10 = string.Empty;
        //    // プライマリキー値1～10
        //    string primaryKeyValue1 = string.Empty;
        //    string primaryKeyValue2 = string.Empty;
        //    string primaryKeyValue3 = string.Empty;
        //    string primaryKeyValue4 = string.Empty;
        //    string primaryKeyValue5 = string.Empty;
        //    string primaryKeyValue6 = string.Empty;
        //    string primaryKeyValue7 = string.Empty;
        //    string primaryKeyValue8 = string.Empty;
        //    string primaryKeyValue9 = string.Empty;
        //    string primaryKeyValue10 = string.Empty;
        //    // ディクショナリ
        //    List<Dictionary<string, string>> dic = new List<Dictionary<string, string>>();

        //    if (!string.IsNullOrEmpty(jsonLockData))
        //    {
        //        // JSON文字列の実行条件をデシリアライズ
        //        var serializer = new JavaScriptSerializer();
        //        dic = serializer.Deserialize<List<Dictionary<string, string>>>(jsonLockData);
        //    }

        //    this.db = new CommonDBManager.CommonDBManager();
        //    if (!this.db.Connect())
        //    {
        //        // エラーログを出力する。
        //        //TODO 後で
        //        // 戻り値 = false を返却する。
        //        return string.Empty;
        //    }

        //    for (int i = 0; i < dic.Count; i++)
        //    {
        //        /////////////
        //        // 初期化
        //        /////////////

        //        // テーブル名
        //        tableName = string.Empty;
        //        // ロックキー項目
        //        lockKeyItem = string.Empty;
        //        // ロックキー値
        //        lockKeyValue = string.Empty;
        //        // プライマリキー1～10
        //        primaryKeyItem1 = string.Empty;
        //        primaryKeyItem2 = string.Empty;
        //        primaryKeyItem3 = string.Empty;
        //        primaryKeyItem4 = string.Empty;
        //        primaryKeyItem5 = string.Empty;
        //        primaryKeyItem6 = string.Empty;
        //        primaryKeyItem7 = string.Empty;
        //        primaryKeyItem8 = string.Empty;
        //        primaryKeyItem9 = string.Empty;
        //        primaryKeyItem10 = string.Empty;
        //        // プライマリキー値1～10
        //        primaryKeyValue1 = string.Empty;
        //        primaryKeyValue2 = string.Empty;
        //        primaryKeyValue3 = string.Empty;
        //        primaryKeyValue4 = string.Empty;
        //        primaryKeyValue5 = string.Empty;
        //        primaryKeyValue6 = string.Empty;
        //        primaryKeyValue7 = string.Empty;
        //        primaryKeyValue8 = string.Empty;
        //        primaryKeyValue9 = string.Empty;
        //        primaryKeyValue10 = string.Empty;

        //        /////////////
        //        // 値を取得
        //        /////////////

        //        // テーブル名
        //        tableName = dic[i]["tableName"];
        //        // ロックキー項目
        //        lockKeyItem = dic[i]["lockKeyItem"];
        //        // プライマリキー1～10
        //        primaryKeyItem1 = dic[i]["primaryKeyItem1"];
        //        if (dic[i].ContainsKey("primaryKeyItem2"))
        //        {
        //            primaryKeyItem2 = dic[i]["primaryKeyItem2"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem3"))
        //        {
        //            primaryKeyItem3 = dic[i]["primaryKeyItem3"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem4"))
        //        {
        //            primaryKeyItem4 = dic[i]["primaryKeyItem4"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem5"))
        //        {
        //            primaryKeyItem5 = dic[i]["primaryKeyItem5"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem6"))
        //        {
        //            primaryKeyItem6 = dic[i]["primaryKeyItem6"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem7"))
        //        {
        //            primaryKeyItem7 = dic[i]["primaryKeyItem7"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem8"))
        //        {
        //            primaryKeyItem8 = dic[i]["primaryKeyItem8"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem9"))
        //        {
        //            primaryKeyItem9 = dic[i]["primaryKeyItem9"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem10"))
        //        {
        //            primaryKeyItem10 = dic[i]["primaryKeyItem10"];
        //        }

        //        // プライマリキー値1～10
        //        primaryKeyValue1 = dic[i]["primaryKeyValue1"];
        //        if (dic[i].ContainsKey("primaryKeyValue2"))
        //        {
        //            primaryKeyValue2 = dic[i]["primaryKeyValue2"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue3"))
        //        {
        //            primaryKeyValue3 = dic[i]["primaryKeyValue3"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue4"))
        //        {
        //            primaryKeyValue4 = dic[i]["primaryKeyValue4"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue5"))
        //        {
        //            primaryKeyValue5 = dic[i]["primaryKeyValue5"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue6"))
        //        {
        //            primaryKeyValue6 = dic[i]["primaryKeyValue6"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue7"))
        //        {
        //            primaryKeyValue7 = dic[i]["primaryKeyValue7"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue8"))
        //        {
        //            primaryKeyValue8 = dic[i]["primaryKeyValue8"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue9"))
        //        {
        //            primaryKeyValue9 = dic[i]["primaryKeyValue9"];
        //        }
        //        if (dic[i].ContainsKey("primaryKeyValue10"))
        //        {
        //            primaryKeyValue10 = dic[i]["primaryKeyValue10"];
        //        }


        //        /////////////
        //        // SQLを作成
        //        /////////////
        //        string sql = string.Empty;
        //        sql = sql + "SELECT ";
        //        sql = sql + "    " + lockKeyItem + " as lock_key_value ";
        //        sql = sql + "FROM ";
        //        sql = sql + "    " + tableName + " ";
        //        sql = sql + "WHERE 1 = 1 ";
        //        sql = sql + "AND " + primaryKeyItem1 + " = '" + primaryKeyValue1 + "' ";
        //        if (dic[i].ContainsKey("primaryKeyItem2"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem2 + " = '" + primaryKeyValue2 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem3"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem3 + " = '" + primaryKeyValue3 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem4"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem4 + " = '" + primaryKeyValue4 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem5"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem5 + " = '" + primaryKeyValue5 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem6"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem6 + " = '" + primaryKeyValue6 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem7"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem7 + " = '" + primaryKeyValue7 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem8"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem8 + " = '" + primaryKeyValue8 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem9"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem9 + " = '" + primaryKeyValue9 + "' ";
        //        }
        //        if (dic[i].ContainsKey("primaryKeyItem10"))
        //        {
        //            sql = sql + "AND " + primaryKeyItem10 + " = '" + primaryKeyValue10 + "' ";
        //        }
        //        sql = sql + ";";

        //        /////////////
        //        // SQLを実行
        //        /////////////
        //        dynamic result = db.GetEntity(sql);

        //        /////////////
        //        // 実行結果を設定
        //        /////////////
        //        DateTime buf = result.lock_key_value;
        //        lockKeyValue = buf.ToString();
        //        /////////////
        //        // 取得したロックキー値をディクショナリに追加する
        //        /////////////
        //        if (dic[i].ContainsKey("lockKeyValue"))
        //        {
        //            dic[i]["lockKeyValue"] = lockKeyValue;
        //        }
        //        else
        //        {
        //            dic[i].Add("lockKeyValue", lockKeyValue);
        //        }

        //    }

        //    /////////////
        //    // ディクショナリからstringに変換
        //    /////////////
        //    try
        //    {
        //        if (dic != null && dic.Count > 0)
        //        {
        //            var serializer = new JavaScriptSerializer();
        //            string jsonLockDataResult = serializer.Serialize(dic);
        //            return jsonLockDataResult;
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        /// <summary>
        /// 辞書リストにロック情報を設定
        /// </summary>
        /// <param name="list">結果辞書リスト</param>
        /// <param name="mappingList">マッピング情報リスト</param>
        public static void SetLockDataString<T>(List<T> list, IList<DBMappingInfo> mappingList, string ctrlId)
        {
            var lockValMaps = mappingList.Where(x => x.CtrlId.Equals(ctrlId) && x.LockTypeEnum == LockType.Value);
            var lockKeyMaps = mappingList.Where(x => x.CtrlId.Equals(ctrlId) && x.LockTypeEnum == LockType.Key);

            foreach (var data in list)
            {
                var lockDic = new Dictionary<string, object>();
                var lockObject = new LockDataInfo();
                var dic = data as IDictionary<string, object>;

                // ロック値情報を設定
                foreach (var val in lockValMaps)
                {
                    if (dic.ContainsKey(val.ValName))
                    {
                        lockObject.AddLockValue(val.ItemNo);
                    }
                }

                // ロックキー情報を設定
                foreach (var key in lockKeyMaps)
                {
                    if (dic.ContainsKey(key.ValName))
                    {
                        lockObject.AddLockKey(key.ItemNo);
                    }
                }

                // ロック情報をJSON文字列へ変換
                var jsonText = lockObject.GetJsonText();

                if (!string.IsNullOrEmpty(jsonText))
                {
                    dic.Add(LockDataKeyName, jsonText);
                }
            }
        }

        ///// <summary>
        ///// 排他ロックデータのチェック
        ///// </summary>
        ///// <param name="db">DB操作クラス</param>
        ///// <param name="dic">チェック対象Dictionary</param>
        ///// <param name="lockValMaps">ロック値用マッピング情報リスト</param>
        ///// <param name="lockKeyMaps">ロックキー用マッピング情報リスト</param>
        ///// <returns></returns>
        //public static bool CheckLockData(CommonDBManager.CommonDBManager db, Dictionary<string, object> dic, List<string> tblNames, List<DBMappingInfo> lockValMaps, List<DBMappingInfo> lockKeyMaps, Dictionary<string, List<DBColumnInfo>> colInfoDic)
        //{
        //    // ロックデータ無しの場合、チェック対象外
        //    if (!dic.ContainsKey(LockDataKeyName) || CommonUtil.IsNullOrEmpty(dic[LockDataKeyName])) { return true; }

        //    // ロックデータのJSON文字列をロック情報へ変換
        //    string jsonText = dic[LockDataKeyName].ToString();
        //    var lockInfo = new LockDataInfo();
        //    lockInfo.SetInfoFromJsonText(jsonText);

        //    //// チェック対象のテーブル名を抽出
        //    //var tblNames = lockValMaps.Select(x => x.TblName).ToList();

        //    StringBuilder sbSql;

        //    // テーブル単位にSQL文を生成
        //    foreach (var tblName in tblNames)
        //    {
        //        sbSql = new StringBuilder();
        //        sbSql.AppendLine("select");

        //        var valMaps = lockValMaps.Where(x => x.LockTblName.Equals(tblName));
        //        var keyMaps = lockKeyMaps.Where(x => x.LockTblName.Equals(tblName));
        //        bool isFirst = true;

        //        // ロック値のカラム名をSELECT対象へ追加
        //        foreach (var valMap in valMaps)
        //        {
        //            if (!isFirst) { sbSql.AppendLine(","); }
        //            isFirst = false;
        //            if (lockInfo.LockValues.Contains(valMap.ItemNo))
        //            {
        //                sbSql.Append(valMap.ColOrgName);
        //                sbSql.Append(" as ");
        //                sbSql.Append(valMap.ColName);
        //            }
        //        }
        //        // ロック値情報が存在しない場合はスキップ
        //        if (isFirst) { continue; }

        //        sbSql.AppendLine("").AppendLine("from").AppendLine(tblName).Append("where ");
        //        dynamic param = new ExpandoObject();
        //        isFirst = true;

        //        // ロックキーのカラム名と値をWHERE句へ追加
        //        foreach (var keyMap in keyMaps)
        //        {
        //            if (lockInfo.LockKeys.Contains(keyMap.ItemNo))
        //            {
        //                if (!isFirst) { sbSql.Append("and "); }
        //                isFirst = false;
        //                sbSql.Append(keyMap.ColOrgName).AppendLine(string.Format(" = @{0}", keyMap.ParamName));
        //                ((IDictionary<string, object>)param).Add(keyMap.ParamName, dic[keyMap.ValName]);
        //            }
        //        }
        //        // ロックキー情報が存在しない場合はスキップ
        //        if (isFirst) { continue; }

        //        // 型変換
        //        CommonSTDUtil.ConvertColumnType(keyMaps.ToList(), colInfoDic[tblName], param, ConvertType.Execute);

        //        if (!db.Connect()) { return false; }
        //        var entity = db.GetEntity(sbSql.ToString(), param);
        //        if (entity == null) { return false; }

        //        // 型変換
        //        CommonSTDUtil.ConvertColumnType(valMaps.ToList(), colInfoDic[tblName], entity, ConvertType.Result);

        //        foreach (var lockVal in (IDictionary<string, object>)entity)
        //        {
        //            var valMap = valMaps.FirstOrDefault(x => x.ColName.Equals(lockVal.Key));
        //            if (valMap == null) { return false; }

        //            // 取得したロック値と検索時のロック値を比較
        //            if (!lockVal.Value.ToString().Equals(dic[valMap.ValName]))
        //            {
        //                // 一致しない場合はチェックNG
        //                return false;
        //            }
        //        }
        //    }

        //    return true;
        //}

        /// <summary>
        /// 排他ロックデータのチェック
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="dic">チェック対象Dictionary</param>
        /// <param name="lockValMaps">ロック値用マッピング情報リスト</param>
        /// <param name="lockKeyMaps">ロックキー用マッピング情報リスト</param>
        /// <returns></returns>
        public static bool CheckLockData(CommonDBManager.CommonDBManager db, Dictionary<string, object> dic, List<string> tblNames, List<DBMappingInfo> lockValMaps, List<DBMappingInfo> lockKeyMaps)
        {
            // ロックデータ無しの場合、チェック対象外
            if (!dic.ContainsKey(LockDataKeyName) || CommonUtil.IsNullOrEmpty(dic[LockDataKeyName])) { return true; }

            // ロックデータのJSON文字列をロック情報へ変換
            string jsonText = dic[LockDataKeyName].ToString();
            var lockInfo = new LockDataInfo();
            lockInfo.SetInfoFromJsonText(jsonText);

            //// チェック対象のテーブル名を抽出
            //var tblNames = lockValMaps.Select(x => x.TblName).ToList();

            StringBuilder sbSql;

            // テーブル単位にSQL文を生成
            foreach (var tblName in tblNames)
            {
                sbSql = new StringBuilder();
                sbSql.AppendLine("select");

                var valMaps = lockValMaps.Where(x => x.LockTblName.Equals(tblName));
                var keyMaps = lockKeyMaps.Where(x => x.LockTblName.Equals(tblName));
                bool isFirst = true;

                // ロック値のカラム名をSELECT対象へ追加
                foreach (var valMap in valMaps)
                {
                    if (!isFirst) { sbSql.AppendLine(","); }
                    isFirst = false;
                    if (lockInfo.LockValues.Contains(valMap.ItemNo))
                    {
                        sbSql.Append(valMap.ColOrgName);
                        sbSql.Append(" as ");
                        sbSql.Append(valMap.ColName);
                    }
                }
                // ロック値情報が存在しない場合はスキップ
                if (isFirst) { continue; }

                sbSql.AppendLine("").AppendLine("from").AppendLine(tblName).Append("where ");
                dynamic param = new ExpandoObject();
                isFirst = true;

                // ロックキーのカラム名と値をWHERE句へ追加
                foreach (var keyMap in keyMaps)
                {
                    if (lockInfo.LockKeys.Contains(keyMap.ItemNo))
                    {
                        if (!isFirst) { sbSql.Append("and "); }
                        isFirst = false;
                        sbSql.Append(keyMap.ColOrgName).AppendLine(string.Format(" = @{0}", keyMap.ParamName));
                        ((IDictionary<string, object>)param).Add(keyMap.ParamName, dic[keyMap.ValName]);
                    }
                }
                // ロックキー情報が存在しない場合はスキップ
                if (isFirst) { continue; }

                // 型変換
                CommonSTDUtil.ConvertColumnType(keyMaps.ToList(), param, ConvertType.Execute);

                if (!db.Connect()) { return false; }
                var entity = db.GetEntity(sbSql.ToString(), param);
                if (entity == null) { return false; }

                // 型変換
                CommonSTDUtil.ConvertColumnType(valMaps.ToList(), entity, ConvertType.Result);

                foreach (var lockVal in (IDictionary<string, object>)entity)
                {
                    var valMap = valMaps.FirstOrDefault(x => x.ColName.Equals(lockVal.Key));
                    if (valMap == null) { return false; }

                    // 取得したロック値と検索時のロック値を比較
                    if (!lockVal.Value.ToString().Equals(dic[valMap.ValName].ToString()))
                    {
                        // 一致しない場合はチェックNG
                        return false;
                    }
                }
            }

            return true;
        }
    }

    /// <summary>
    /// 排他ロック種類
    /// </summary>
    public enum LockType : int
    {
        /// <summary>対象外</summary>
        None,
        /// <summary>ロック値</summary>
        Value,
        /// <summary>ロックキー</summary>
        Key,
    }

    /// <summary>
    /// 排他ロックデータクラス
    /// </summary>
    public class LockData
    {
        /// <summary>項目番号</summary>
        public int ItemNo { get; set; }
        /// <summary>ロックデータ値</summary>
        public object Value { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LockData()
        {
            this.ItemNo = 0;
            this.Value = null;
        }
    }

    /// <summary>
    /// 排他ロック情報クラス
    /// </summary>
    public class LockDataInfo
    {
        /// <summary>ロック値名</summary>
        private const string LockValueName = "lockValues";
        /// <summary>ロックキー名</summary>
        private const string LockKeyName = "lockKeys";

        /// <summary>ロック値項目番号リスト</summary>
        public List<int> LockValues { get; private set; }
        /// <summary>ロックキー項目番号リスト</summary>
        public List<int> LockKeys { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LockDataInfo()
        {
            this.LockValues = new List<int>();
            this.LockKeys = new List<int>();
        }

        /// <summary>
        /// ロック値データの追加
        /// </summary>
        /// <param name="itemNo">項目番号</param>
        /// <param name="value">値</param>
        public void AddLockValue(int itemNo)
        {
            this.LockValues.Add(itemNo);
        }

        /// <summary>
        /// ロックキーデータの追加
        /// </summary>
        /// <param name="itemNo">項目番号</param>
        /// <param name="value">値</param>
        public void AddLockKey(int itemNo)
        {
            this.LockKeys.Add(itemNo);
        }

        /// <summary>
        /// ロック情報のJSON文字列を取得
        /// </summary>
        /// <returns></returns>
        public string GetJsonText()
        {
            string jsonText = string.Empty;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (this.LockValues.Count > 0)
            {
                dic.Add(LockValueName, this.LockValues);
            }
            if (this.LockKeys.Count > 0)
            {
                dic.Add(LockKeyName, this.LockKeys);
            }
            if (dic.Count > 0)
            {
                //2024.09 .NET8バージョンアップ対応 start
                //var jsonOptions = new JsonSerializerOptions
                //{
                //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                //};
                //jsonText = JsonSerializer.Serialize(dic, jsonOptions);
                jsonText = JsonSerializer.Serialize(dic, JsonSerializerOptionsDefine.JsOptionsForEncode);
                //2024.09 .NET8バージョンアップ対応 end
            }
            return jsonText;
        }

        /// <summary>
        /// JSON文字列から変換したロック情報の設定
        /// </summary>
        /// <param name="jsonText">JSON文字列</param>
        public void SetInfoFromJsonText(string jsonText)
        {
            if (string.IsNullOrWhiteSpace(jsonText)) { return; }

            //2024.09 .NET8バージョンアップ対応 start
            //var jsonOptions = new JsonSerializerOptions
            //{
            //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            //};
            //var results = JsonSerializer.Deserialize<Dictionary<string, List<int>>>(jsonText, jsonOptions);
            var results = JsonSerializer.Deserialize<Dictionary<string, List<int>>>(jsonText, JsonSerializerOptionsDefine.JsOptionsForEncode);
            //2024.09 .NET8バージョンアップ対応 end

            this.LockValues.AddRange(results[LockValueName]);
            this.LockKeys.AddRange(results[LockKeyName]);
        }
    }
}
