using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonWebTemplate.CommonDefinitions
{
    public class CommonUtil
    {
        #region 入力チェック
        /// <summary>
        /// nullまたは空かどうかのチェック
        /// </summary>
        /// <param name="val">対象オブジェクト</param>
        /// <returns>チェック結果(true:nullまたは空/false:空でない)</returns>
        public static bool IsNullOrEmpty(object val)
        {
            return val != null ? string.IsNullOrEmpty(val.ToString()) : true;
        }

        /// <summary>
        /// 文字列が英数字かどうか判定
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>true:文字列が英数字、false:左記以外</returns>
        /// <remarks>大文字・小文字の区別なし</remarks>
        public static bool IsAlphaNumeric(string str)
        {
            return new Regex("^[0-9a-zA-Z]+$").IsMatch(str);
        }

        /// <summary>
        /// 文字列が日付に変更できるか判定
        /// </summary>
        /// <param name="date">対象文字列</param>
        /// <returns>true:正常：false:以上</returns>
        public static bool IsDate(string date)
        {
            DateTime dt;
            if (DateTime.TryParse(date, out dt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 文字列(yyyymmdd)が日付に変更できるか判定
        /// </summary>
        /// <param name="date">判定する文字列(yyyymmdd)</param>
        /// <returns>判定できる場合True</returns>
        public static bool IsDateYyyymmdd(string date)
        {
            DateTime dt;

            if (DateTime.TryParseExact(date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 文字列がDecimal型に変更できるか判定
        /// </summary>
        /// <param name="str">判定する文字列</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool IsDecimal(string str)
        {
            decimal num;
            if (decimal.TryParse(str, out num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 文字列が半角カナかどうか判定
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>true:半角カナ、false:左記以外</returns>
        public static bool IsHalfKatakana(string str)
        {
            return new Regex("^[\uFF65-\uFF9F]+$").IsMatch(str);
        }

        /// <summary>
        /// 文字列が半角カナかどうか判定
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>true:半角カナを含む、false:半角カナを含まない</returns>
        public static bool ExistHalfKatakana(string str)
        {
            return new Regex("[\uFF65-\uFF9F]").IsMatch(str);
        }

        /// <summary>
        /// 日付比較(※VAL値が異なる場合に使用)
        /// </summary>
        /// <param name="sDate">(開始)日付１</param>
        /// <param name="eDate">(終了)日付２</param>
        /// <returns>1:sDateの方が未来,0:同日,-1:sDateの方が過去</returns>
        public static int CompareDate(DateTime sDate, DateTime eDate)
        {
            return sDate.CompareTo(eDate.Date);
        }

        /// <summary>
        /// 日付比較(※VAL値が異なる場合に使用)
        /// </summary>
        /// <param name="start">(開始)日付１</param>
        /// <param name="end">(終了)日付２</param>
        /// <returns>1:sDateの方が未来,0:同日,-1:sDateの方が過去,-9:エラー(変換失敗など)</returns>
        public static int CompareDate(string start, string end)
        {
            // 比較結果(初期値)
            int result = -9;

            DateTime sDate, eDate;

            // 日付型に型変更
            if (!(DateTime.TryParse(start, out sDate) && DateTime.TryParse(end, out eDate)))
            {
                return result; // 変換失敗
            }

            return CompareDate(sDate, eDate);
        }
        #endregion

        #region 変換処理
        /// <summary>
        /// int値からEnum値へ変換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T IntToEnum<T>(int val)
        {
            return (T)Enum.ToObject(typeof(T), val);
        }

        /// <summary>
        /// 文字列からEnum値へ変換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T StringToEnum<T>(string val) where T : struct
        {
            T result;
            if (Enum.TryParse<T>(val, out result))
            {
                return result;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// JsonElement構造体のDictionary型のListをobject型のDictionary型のListへ変換
        /// </summary>
        /// <param name="list">JsonElement構造体のDictionary型のList</param>
        /// <returns>object型のDictionary型のList</returns>
        public static List<Dictionary<string, object>> JsonElementDictionaryListToObjectDictionaryList(List<Dictionary<string, JsonElement>> list)
        {
            return list.Select(d => JsonElementDictionaryToObjectDictionary(d)).ToList();
        }

        /// <summary>
        /// JsonElement構造体のDictionary型のListをobject型のDictionary型のListへ変換
        /// </summary>
        /// <param name="list">JsonElement構造体のDictionary型のList</param>
        /// <returns>object型のDictionary型のList</returns>
        public static List<Dictionary<string, object>> JsonElementDictionaryListToObjectDictionaryList(List<Dictionary<string, object>> list)
        {
            return list.Select(d => JsonElementDictionaryToObjectDictionary(d)).ToList();
        }

        /// <summary>
        /// JSON文字列をDictionary型のListへ変換
        /// </summary>
        /// <param name="json">JSON文字列</param>
        /// <returns>Dictionary型のList</returns>
        public static List<Dictionary<string, object>> JsonToDictionaryList(string json)
        {
            // JSON文字列をList<Dictionary<string, JsonElement>>型に変換
            var list = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(json);

            return list.Select(d => JsonElementDictionaryToObjectDictionary(d)).ToList();
        }

        /// <summary>
        /// JSON文字列をDictionary型へ変換
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDictionary(string json)
        {
            // JSON文字列をDictionary<string, JsonElement>に変換
            var dic = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            // Dictionary<string, JsonElement>からDictionary<string, object>に変換して返す
            return JsonElementDictionaryToObjectDictionary(dic);
        }

        /// <summary>
        /// JsonElement構造体のDictionaryをobject型のDictionary型へ変換
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonElementDictionaryToObjectDictionary(Dictionary<string, JsonElement> dic)
        {
            // JsonElementから値を取り出してDictionary<string, object>型で返す
            return dic.Select(d => new { key = d.Key, value = ParseJsonElement(d.Value) })
                .ToDictionary(a => a.key, a => a.value);
        }

        /// <summary>
        /// JsonElement構造体のobjectのDictionaryをobject型のDictionary型へ変換
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonElementDictionaryToObjectDictionary(Dictionary<string, object> dic)
        {
            // JsonElemen型のobjectから値を取り出してDictionary<string, object>型で返す
            return dic.Select(d => new { key = d.Key, value = ParseJsonElement(d.Value) })
                .ToDictionary(a => a.key, a => a.value);
        }

        /// <summary>
        /// JsonElementを各型に変換
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static object ParseJsonElement(object obj)
        {
            if(obj is JsonElement)
            {
                return ParseJsonElement((JsonElement)obj);
            }
            else
            {
                return obj;
            }
        }

        /// <summary>
        /// JsonElementを各型に変換
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static object ParseJsonElement(JsonElement elem)
        {
            // データの種類によって値を取得する処理を変える
            return elem.ValueKind switch
            {
                JsonValueKind.String => elem.GetString(),
                JsonValueKind.Number => elem.GetDecimal(),
                JsonValueKind.False => false,
                JsonValueKind.True => true,
                JsonValueKind.Array => elem.EnumerateArray().Select(e => ParseJsonElement(e)).ToList(),
                JsonValueKind.Null => null,
                JsonValueKind.Object => JsonToDictionary(elem.GetRawText()),
                _ => throw new NotSupportedException(),
            };
        }

        /// <summary>
        /// マッチパターンに応じた検索条件文字列の取得
        /// </summary>
        /// <param name="text">対象文字列</param>
        /// <param name="pattern">マッチパターン</param>
        /// <returns></returns>
        public static string GetMatchPatternText(string text, MatchPattern pattern)
        {
            string result = text;
            switch (pattern)
            {
                case MatchPattern.ForwardMatch: // 前方一致
                    result = result + "%";
                    break;
                case MatchPattern.BackwardMatch:    // 後方一致
                    result = "%" + result;
                    break;
                case MatchPattern.PartialMatch: // 部分一致
                    result = "%" + result + "%";
                    break;
                default:    // 完全一致
                    break;
            }
            return result;
        }

        #endregion
    }

    /// <summary>
    /// 検索マッチパターン
    /// </summary>
    public enum MatchPattern : int
    {
        /// <summary>完全一致</summary>
        ExactMatch,
        /// <summary>前方一致</summary>
        ForwardMatch,
        /// <summary>後方一致</summary>
        BackwardMatch,
        /// <summary>部分一致</summary>
        PartialMatch
    }
}
