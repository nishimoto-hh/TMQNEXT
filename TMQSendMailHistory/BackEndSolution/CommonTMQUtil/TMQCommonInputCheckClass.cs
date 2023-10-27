using System;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComMsg = CommonSTDUtil.CommonResources;

namespace CommonTMQUtil.TMQCommonUtil
{
    /// <summary>
    /// 入力チェック用クラス
    /// </summary>
    public class TMQCommonInputCheckClass : MarshalByRefObject
    {
        /// <summary>
        /// 入力チェックの共通クラス
        /// </summary>
        /// <remarks>この機能を継承して、日付や数値など独自のチェックを実装する。必須チェックなど共通はこちらに実装する。</remarks>
        public class InputCheckBaseClass
        {
            /// <summary>言語ID、メッセージ取得に必要</summary>
            private string languageId;
            /// <summary>メッセージリソース、メッセージ取得に必要</summary>
            private ComUtil.MessageResources messageResources;
            /// <summary>エラーメッセージ</summary>
            public string errorMessage { get; set; }
            /// <summary>エラーコード、一部のチェックで返す場合がある</summary>
            public int errorCode { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="languageId">言語ID(基本的に自身(this)の値を渡す)</param>
            /// <param name="messageResources">メッセージリソース(基本的に自身(this)の値を渡す)</param>
            public InputCheckBaseClass(string languageId, ComUtil.MessageResources messageResources)
            {
                this.languageId = languageId;
                this.messageResources = messageResources;
                this.errorCode = 0;
                this.errorMessage = string.Empty;
            }

            /// <summary>
            /// リソースメッセージ取得
            /// </summary>
            /// <param name="key">メッセージID</param>
            /// <returns>メッセージ</returns>
            protected string GetResMessage(string key)
            {
                return ComUtil.GetPropertiesMessage(key, this.languageId, this.messageResources);
            }

            /// <summary>
            /// リソースメッセージ取得
            /// </summary>
            /// <param name="keys">メッセージIDの配列</param>
            /// <returns>メッセージ</returns>
            protected string GetResMessage(string[] keys)
            {
                return ComUtil.GetPropertiesMessage(keys, this.languageId, this.messageResources);
            }

            /// <summary>
            /// 必須チェック
            /// </summary>
            /// <param name="value">チェック対象の値</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>エラーの場合True</returns>
            public bool checkExists(string value, string name)
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260007, name });
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 桁数チェック
            /// </summary>
            /// <param name="value">チェック対象の値</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <param name="digit">桁数</param>
            /// <returns>エラーの場合True</returns>
            public bool checkDigit(string value, string name, int digit)
            {
                int checkLength = 0;
                if (!string.IsNullOrEmpty(value))
                {
                    checkLength = value.Length;
                }
                if (checkLength > digit)
                {
                    errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260004, name, digit.ToString() });
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 文字型入力チェック
        /// </summary>
        public class InputCheckCharacter : InputCheckBaseClass
        {
            /// <summary>
            /// コンストラクタ　共通と同一
            /// </summary>
            /// <param name="languageId">言語ID(基本的に自身(this)の値を渡す)</param>
            /// <param name="messageResources">メッセージリソース(基本的に自身(this)の値を渡す)</param>
            public InputCheckCharacter(string languageId, ComUtil.MessageResources messageResources) : base(languageId, messageResources)
            {
            }

            /// <summary>
            /// 半角英数チェック
            /// </summary>
            /// <param name="value">値</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>エラーの場合True</returns>
            public bool checkAlphaNumeric(string value, string name)
            {
                if (!ComUtil.IsAlphaNumeric(value))
                {
                    errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260009, name, ComMsg.ID.ID911260004 });
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 半角英数記号チェック
            /// </summary>
            /// <param name="value">値</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>エラーの場合True</returns>
            public bool checkAlphaNumericSymbol(string value, string name)
            {
                if (!ComUtil.IsAlphaNumericSymbol(value))
                {
                    // TODO メッセージ追加
                    errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260009, name, "半角英数記号" });
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 文字列の大小チェック
            /// </summary>
            /// <param name="strFrom">小さいはずの文字列</param>
            /// <param name="strTo">大きいはずの文字列</param>
            /// <param name="fromName">小さいはずの文字列の名称</param>
            /// <param name="toName">大きいはずの文字列の名称</param>
            /// <returns>エラーの場合True</returns>
            public bool checkStrCompare(string strFrom, string strTo, string fromName, string toName)
            {
                if (strFrom.CompareTo(strTo) > 0)
                {
                    errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941200002, fromName, toName });
                    return true;
                }
                return false;
            }

        }

        /// <summary>
        /// 数値型入力チェック
        /// </summary>
        public class InputCheckNumber : InputCheckBaseClass
        {
            /// <summary>
            /// コンストラクタ　共通と同一
            /// </summary>
            /// <param name="languageId">言語ID(基本的に自身(this)の値を渡す)</param>
            /// <param name="messageResources">メッセージリソース(基本的に自身(this)の値を渡す)</param>
            public InputCheckNumber(string languageId, ComUtil.MessageResources messageResources) : base(languageId, messageResources)
            {
            }

            /// <summary>
            /// 数値チェック
            /// </summary>
            /// <param name="value">値</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>エラーの場合True</returns>
            public bool checkNumber(string value, string name)
            {
                decimal val;
                if (!decimal.TryParse(value, out val))
                {
                    errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260009, name, ComMsg.ID.ID911130002 });
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 整数部桁数チェック
            /// </summary>
            /// <param name="value">値</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <param name="digit">桁数</param>
            /// <returns>エラーの場合True</returns>
            public bool checkInteger(string value, string name, int digit)
            {
                // TODO 与えられた文字列が数値なら整数部の桁数を取得して比較する処理を作成してください
                if (false)
                {
                    errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260008, name, ComMsg.ID.ID911140003, digit.ToString() });
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 小数部桁数チェック
            /// </summary>
            /// <param name="value">値</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <param name="digit">桁数</param>
            /// <returns>エラーの場合True</returns>
            public bool checkDecimal(string value, string name, int digit)
            {
                // TODO 与えられた文字列が数値なら小数部の桁数を取得して比較する処理を作成してください
                if (false)
                {
                    errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260008, name, ComMsg.ID.ID911120019, digit.ToString() });
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 数値大小チェック(大きい)
            /// </summary>
            /// <param name="bigValue">大きい値</param>
            /// <param name="smallValue">小さい値</param>
            /// <param name="bigName">大きい値の名称</param>
            /// <param name="smallName">小さい値の名称</param>
            /// <param name="isOkEqual">省略可能 同じ場合はエラーにするなら、False(デフォルト値)、同じ場合はエラーにしないなら、True</param>
            /// <returns>大きい値>(≧)小さい値ならば、True。そうでなければ、False(エラー)</returns>
            public bool isGreaterValue(string bigValue, string smallValue, string bigName, string smallName, bool isOkEqual = false)
            {
                // 引数の値を数値に変換、変換できないなら最小値
                var big = ComUtil.ConvertDecimal(bigValue) ?? decimal.MinValue;
                var small = ComUtil.ConvertDecimal(smallValue) ?? decimal.MinValue;

                // エラーメッセージ 以上と超で分岐
                // TODO メッセージ追加
                string errorMsg = isOkEqual ? "{0}は{1}以上の値を入力してください。" : ComMsg.ID.ID941260012;
                if (isOkEqual)
                {
                    // 同じ値はエラーとしない場合、≧
                    if (big >= small)
                    {
                        return true;
                    }
                }
                else
                {
                    // 同じ場合はエラーとする場合、>
                    if (big > small)
                    {
                        return true;
                    }
                }

                errorMessage = GetResMessage(new string[] { errorMsg, bigName, smallName });
                return false;
            }

            /// <summary>
            /// 数値大小チェック(小さい)
            /// </summary>
            /// <param name="smallValue">小さい値</param>
            /// <param name="bigValue">大きい値</param>
            /// <param name="smallName">小さい値の名称</param>
            /// <param name="bigName">大きい値の名称</param>
            /// <param name="isOkEqual">省略可能 同じ場合はエラーにするなら、False(デフォルト値)、同じ場合はエラーにしないなら、True</param>
            /// <returns>大きい値<(≦)小さい値ならば、True。そうでなければ、False(エラー)</returns>
            public bool isLessValue(string smallValue, string bigValue, string smallName, string bigName, bool isOkEqual = false)
            {
                // 引数の値を数値に変換、変換できないなら最小値
                var big = ComUtil.ConvertDecimal(bigValue) ?? decimal.MinValue;
                var small = ComUtil.ConvertDecimal(smallValue) ?? decimal.MinValue;
                // エラーメッセージ 以下と未満で分岐
                // TODO メッセージ追加
                string errorMsg = isOkEqual ? "{0}は{1}以下の値を入力してください。" : ComMsg.ID.ID941260013;
                if (isOkEqual)
                {
                    // 同じ値はエラーとしない場合、≧
                    if (big >= small)
                    {
                        return true;
                    }
                }
                else
                {
                    // 同じ場合はエラーとする場合、>
                    if (big > small)
                    {
                        return true;
                    }
                }
                errorMessage = GetResMessage(new string[] { errorMsg, smallName, bigName });
                return false;
            }
        }


        /// <summary>
        /// 日付型入力チェック
        /// </summary>
        public class InputCheckDate : InputCheckBaseClass
        {
            /// <summary>
            /// コンストラクタ　共通と同一
            /// </summary>
            /// <param name="languageId">言語ID(基本的に自身(this)の値を渡す)</param>
            /// <param name="messageResources">メッセージリソース(基本的に自身(this)の値を渡す)</param>
            public InputCheckDate(string languageId, ComUtil.MessageResources messageResources) : base(languageId, messageResources)
            {
            }

            /// <summary>
            /// 日付チェック
            /// </summary>
            /// <param name="value">値(yyyyMMdd)</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>エラーの場合True</returns>
            public bool checkDate(string value, string name)
            {
                if (!ComUtil.IsDateYyyymmdd(value))
                {
                    errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260009, name, ComMsg.ID.ID911270007 });
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 日付チェック
            /// </summary>
            /// <param name="value">値</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>エラーの場合True</returns>
            public bool checkDateTime(string value, string name)
            {
                if (!ComUtil.IsDateTime(value))
                {
                    errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260009, name, ComMsg.ID.ID911270007 });
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 日付大小チェック(大きい)
            /// </summary>
            /// <param name="after">大きいはずの日時</param>
            /// <param name="before">小さいはずの日時</param>
            /// <param name="afterName">大きいはずの日時の名称</param>
            /// <param name="beforeName">小さいはずの日時の名称</param>
            /// <returns>大きいはずの日時が小さいはずより大きい場合、True.同じか小さい場合、False(エラー)</returns>
            public bool isAfterDateTime(DateTime after, DateTime before, string afterName, string beforeName)
            {
                // 大小比較
                int result = ComUtil.CompareDate(after, before);
                if (result > 0)
                {
                    // 大きい場合
                    return true;
                }
                // それ以外の場合
                errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260012, afterName, beforeName });
                return false;
            }

            /// <summary>
            /// 日付大小チェック(小さい)
            /// </summary>
            /// <param name="before">小さいはずの日時</param>
            /// <param name="after">大きいはずの日時</param>
            /// <param name="beforeName">小さいはずの日時の名称</param>
            /// <param name="afterName">大きいはずの日時の名称</param>
            /// <returns>小さいはずの日時が大きいはずより小さい場合、True.同じか大きい場合、False(エラー)</returns>
            public bool isBeforeDateTime(DateTime before, DateTime after, string beforeName, string afterName)
            {
                // 大小比較
                int result = ComUtil.CompareDate(before, after);
                if (result > 0)
                {
                    // 小さい場合
                    return true;
                }
                // それ以外の場合
                errorMessage = GetResMessage(new string[] { ComMsg.ID.ID941260013, beforeName, afterName });
                return false;
            }

            /// <summary>
            /// システム日付入力チェック(大きい)
            /// </summary>
            /// <param name="value">システム日付と比較する日時</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>引数の値がシステム日付より大きい場合、True.同じか小さい場合、False(エラー)</returns>
            public bool isAfterSysDate(DateTime value, string name)
            {
                DateTime today = DateTime.Today;
                // TODO メッセージ追加
                return isAfterDateTime(value, today, name, "システム日付");
            }

            /// <summary>
            /// システム日付入力チェック(小さい)
            /// </summary>
            /// <param name="value">システム日付と比較する日時</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>引数の値がシステム日付より小さい場合、True.同じか大きい場合、False(エラー)</returns>
            public bool isBeforeSysDate(DateTime value, string name)
            {
                DateTime today = DateTime.Today;
                // TODO メッセージ追加
                return isBeforeDateTime(value, today, name, "システム日付");
            }

            /// <summary>
            /// システム日時入力チェック(以上)
            /// </summary>
            /// <param name="value">システム日付と比較する日時</param>
            /// <returns>引数の値がシステム日付と同じか大きい場合True.小さい場合、False(エラー)</returns>
            public bool isOnAfterSysDate(DateTime value)
            {
                DateTime today = DateTime.Today;
                // 大小比較
                int result = ComUtil.CompareDate(value, today);
                if (result >= 0)
                {
                    // 大きい、または同じ場合
                    return true;
                }
                // それ以外の場合
                // TODO メッセージ追加
                errorMessage = GetResMessage(new string[] { "{0}以降の日付を入力して下さい。", "システム日付" });
                return false;
            }
        }
    }
}
