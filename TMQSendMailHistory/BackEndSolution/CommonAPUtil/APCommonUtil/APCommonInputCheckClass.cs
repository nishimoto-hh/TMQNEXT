using System;
using APChecker = CommonAPUtil.APCheckDigitUtil.APCheckDigitUtil;
using APDigitUtil = CommonAPUtil.APCheckDigitUtil.APCheckDigitUtil;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonAPUtil.APCommonUtil
{
    /// <summary>
    /// 入力チェック用クラス
    /// </summary>
    public class APCommonInputCheckClass : MarshalByRefObject
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
            /// <summary>Gets or sets エラーメッセージ</summary>
            /// <value>エラーメッセージ</value>
            public string ErrorMessage { get; set; }
            /// <summary>Gets or sets エラーコード、一部のチェックで返す場合がある</summary>
            /// <value>エラーコード、一部のチェックで返す場合がある</value>
            public int ErrorCode { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="languageId">言語ID(基本的に自身(this)の値を渡す)</param>
            /// <param name="messageResources">メッセージリソース(基本的に自身(this)の値を渡す)</param>
            public InputCheckBaseClass(string languageId, ComUtil.MessageResources messageResources)
            {
                this.languageId = languageId;
                this.messageResources = messageResources;
                this.ErrorCode = 0;
                this.ErrorMessage = string.Empty;
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
            public bool CheckExists(string value, string name)
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00041, name });
                    return true;
                }
                return false;
            }

            /// <summary>
            /// NULLチェック
            /// </summary>
            /// <param name="value">チェック対象の値</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>エラーの場合true</returns>
            public bool CheckNull(string value, string name)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00010, name });
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
            public bool CheckDigit(string value, string name, int digit)
            {
                int errCode = 0;
                if (!APDigitUtil.CheckMaxLength(value, digit, ref errCode))
                {
                    ErrorCode = errCode;
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00023, name, digit.ToString() });
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
            public bool CheckAlphaNumeric(string value, string name)
            {
                if (!ComUtil.IsAlphaNumeric(value))
                {
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00043, name, APResources.ID.MS00045 });
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
            public bool CheckAlphaNumericSymbol(string value, string name)
            {
                if (!ComUtil.IsAlphaNumericSymbol(value))
                {
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00043, name, APResources.ID.MS00112 });
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
            public bool CheckStrCompare(string strFrom, string strTo, string fromName, string toName)
            {
                if (strFrom.CompareTo(strTo) > 0)
                {
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00022, fromName, toName });
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
            public bool CheckNumber(string value, string name)
            {
                decimal val;
                if (!decimal.TryParse(value, out val))
                {
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00043, name, APResources.ID.MS00046 });
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
            public bool CheckInteger(string value, string name, int digit)
            {
                int errCode = 0;
                if (!APDigitUtil.CheckIntegerPart(value, digit, ref errCode))
                {
                    ErrorCode = errCode;
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00042, name, APResources.ID.MS00048, digit.ToString() });
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
            public bool CheckDecimal(string value, string name, int digit)
            {
                int errCode = 0;
                if (!APDigitUtil.CheckDecimalPart(value, digit, ref errCode))
                {
                    ErrorCode = errCode;
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00042, name, APResources.ID.MS00049, digit.ToString() });
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 数値範囲チェック
            /// </summary>
            /// <param name="value">値</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <param name="unitDivision">単位区分(数値桁数チェックマスタ)</param>
            /// <param name="db">DB接続</param>
            /// <returns>エラーの場合True</returns>
            public bool IsErrorRange(string value, string name, string unitDivision, ComDB db)
            {
                if (string.IsNullOrEmpty(unitDivision))
                {
                    // 単位区分がNullの場合、チェックなし
                    return false;
                }
                // 範囲チェックを行う
                int errCode = 0;
                if (!APChecker.CheckDigit(unitDivision, value, ref errCode, db))
                {
                    // エラーの場合
                    ErrorCode = errCode;
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00098, name });
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
            public bool IsGreaterValue(string bigValue, string smallValue, string bigName, string smallName, bool isOkEqual = false)
            {
                // 引数の値を数値に変換、変換できないなら最小値
                var big = ComUtil.ConvertDecimal(bigValue) ?? decimal.MinValue;
                var small = ComUtil.ConvertDecimal(smallValue) ?? decimal.MinValue;

                // エラーメッセージ 以上と超で分岐
                string errorMsg = isOkEqual ? APResources.ID.MS00107 : APResources.ID.MS00079;
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

                ErrorMessage = GetResMessage(new string[] { errorMsg, bigName, smallName });
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
            public bool IsLessValue(string smallValue, string bigValue, string smallName, string bigName, bool isOkEqual = false)
            {
                // 引数の値を数値に変換、変換できないなら最小値
                var big = ComUtil.ConvertDecimal(bigValue) ?? decimal.MinValue;
                var small = ComUtil.ConvertDecimal(smallValue) ?? decimal.MinValue;
                // エラーメッセージ 以下と未満で分岐
                string errorMsg = isOkEqual ? APResources.ID.MS00108 : APResources.ID.MS00080;
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
                ErrorMessage = GetResMessage(new string[] { errorMsg, smallName, bigName });
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
            public bool CheckDate(string value, string name)
            {
                if (!ComUtil.IsDateYyyymmdd(value))
                {
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00043, name, APResources.ID.MS00047 });
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
            public bool CheckDateTime(string value, string name)
            {
                if (!ComUtil.IsDateTime(value))
                {
                    ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00043, name, APResources.ID.MS00047 });
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
            public bool IsAfterDateTime(DateTime after, DateTime before, string afterName, string beforeName)
            {
                // 大小比較
                int result = ComUtil.CompareDate(after, before);
                if (result > 0)
                {
                    // 大きい場合
                    return true;
                }
                // それ以外の場合
                ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00079, afterName, beforeName });
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
            public bool IsBeforeDateTime(DateTime before, DateTime after, string beforeName, string afterName)
            {
                // 大小比較
                int result = ComUtil.CompareDate(before, after);
                if (result > 0)
                {
                    // 小さい場合
                    return true;
                }
                // それ以外の場合
                ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00080, beforeName, afterName });
                return false;
            }

            /// <summary>
            /// システム日付入力チェック(大きい)
            /// </summary>
            /// <param name="value">システム日付と比較する日時</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>引数の値がシステム日付より大きい場合、True.同じか小さい場合、False(エラー)</returns>
            public bool IsAfterSysDate(DateTime value, string name)
            {
                DateTime today = DateTime.Today;
                return IsAfterDateTime(value, today, name, APResources.ID.MS00090);
            }

            /// <summary>
            /// システム日付入力チェック(小さい)
            /// </summary>
            /// <param name="value">システム日付と比較する日時</param>
            /// <param name="name">エラーメッセージに出力する項目名</param>
            /// <returns>引数の値がシステム日付より小さい場合、True.同じか大きい場合、False(エラー)</returns>
            public bool IsBeforeSysDate(DateTime value, string name)
            {
                DateTime today = DateTime.Today;
                return IsBeforeDateTime(value, today, name, APResources.ID.MS00090);
            }

            /// <summary>
            /// システム日時入力チェック(以上)
            /// </summary>
            /// <param name="value">システム日付と比較する日時</param>
            /// <returns>引数の値がシステム日付と同じか大きい場合True.小さい場合、False(エラー)</returns>
            public bool IsOnAfterSysDate(DateTime value)
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
                ErrorMessage = GetResMessage(new string[] { APResources.ID.MS00092, APResources.ID.MS00090 });
                return false;
            }
        }
    }
}
