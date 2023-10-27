using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using CommonSTDUtil.CommonLogger;

using APResources = CommonAPUtil.APCommonUtil.APResources;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComNCD = CommonSTDUtil.CommonSTDUtil.CommonSTDUtilDataClass;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Constants = APConstants.APConstants;
using Dao = CommonAPUtil.APCheckDigitUtil.APCheckDigitDataClass;

namespace CommonAPUtil.APCheckDigitUtil
{
    /// <summary>
    /// 端数処理クラス
    /// </summary>
    public class APCheckDigitUtil
    {
        #region クラス内変数
        /// <summary>
        /// 基本書式(#,0.)
        /// </summary>
        private static string decimalPointFormatBase = "#,0.";
        /// <summary>ログ出力</summary>
        private static CommonLogger logger = CommonLogger.GetInstance("logger");
        #endregion

        #region エラーコードクラス
        /// <summary>
        /// エラーコード
        /// </summary>
        private static class ErrCode
        {
            /// <summary>
            /// チェックOK(エラー無し)
            /// </summary>
            public const int ErrNone = 0;
            /// <summary>
            /// 数値文字列エラー
            /// </summary>
            public const int ErrToInt = 1;
            /// <summary>
            /// 最大文字列長エラー
            /// </summary>
            public const int ErrMaxLength = 2;
            /// <summary>
            /// 整数部桁数エラー
            /// </summary>
            public const int ErrIntegerPart = 3;
            /// <summary>
            /// 小数部桁数エラー
            /// </summary>
            public const int ErrDecimalPart = 4;
            /// <summary>
            /// レンジエラー
            /// </summary>
            public const int ErrRange = 5;
        }

        /// <summary>
        /// 丸め処理
        /// </summary>
        public static class RoundDivision
        {
            /// <summary>なし</summary>
            public const decimal None = decimal.Zero;
            /// <summary>切り捨て</summary>
            public const decimal Truncate = 1;
            /// <summary>四捨五入</summary>
            public const decimal Rounding = 2;
            /// <summary>切り上げ</summary>
            public const decimal RoundUp = 3;
            /// <summary>その他</summary>
            public const decimal Other = 4;
        }
        #endregion

        #region エラーメッセージクラス
        /// <summary>
        /// エラーメッセージ(仮？)
        /// </summary>
        private static class ErrMessage
        {
            /// <summary>{0}は数値を入力して下さい。</summary>
            public const string M00320 = "{0}は数値を入力して下さい。";
            /// <summary>{1}行目{0}は数値を入力して下さい。</summary>
            public const string M00337 = "{1}行目{0}は数値を入力して下さい。";
            /// <summary>{0}は{1}桁以下で入力して下さい。</summary>
            public const string M00043 = "{0}は{1}桁以下で入力して下さい。";
            /// <summary>{2}行目{0}は{1}桁以下で入力して下さい。</summary>
            public const string M00340 = "{2}行目{0}は{1}桁以下で入力して下さい。";
            /// <summary>{0}の整数部は{1}桁までで入力して下さい。</summary>
            public const string M00535 = "{0}の整数部は{1}桁までで入力して下さい。";
            /// <summary>{2}行目{0}の整数部は{1}桁までで入力して下さい。</summary>
            public const string M00558 = "{2}行目{0}の整数部は{1}桁までで入力して下さい。";
            /// <summary>{0}の小数部は{1}桁までで入力して下さい。</summary>
            public const string M00536 = "{0}の小数部は{1}桁までで入力して下さい。";
            /// <summary>{2}行目{0}の小数部は{1}桁までで入力して下さい。</summary>
            public const string M00559 = "{2}行目{0}の小数部は{1}桁までで入力して下さい。";
            /// <summary>{0}は{1}から{2}で入力してください。</summary>
            public const string M00537 = "{0}は{1}から{2}で入力してください。";
            /// <summary>{3}行目{0}は{1}から{2}で入力してください。</summary>
            public const string M00341 = "{3}行目{0}は{1}から{2}で入力してください。";
        }
        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public APCheckDigitUtil()
        {

        }
        #endregion

        #region 数値チェックメソッド
        /// <summary>
        /// 数値桁数チェックマスタのは数区分、小数点以下桁数に基づき、数値をフォーマットする
        /// </summary>
        /// <param name="inUnitDivision">単位区分</param>
        /// <param name="inValue">フォーマット対象の数値</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>フォーマットされた値</returns>
        public static string Format(string inUnitDivision, decimal inValue, ComDB db)
        {
            // 指定されて書式にフォーマットする
            return Format(inUnitDivision, null, null, inValue, db);
        }

        /// <summary>
        /// 数値桁数チェックマスタの端数区分、小数点以下桁数に基づき、数値をフォーマットする
        /// </summary>
        /// <param name="inUnitDivision">単位区分</param>
        /// <param name="inVenderDivision">取引先区分</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="inValue">フォーマット対象の数値</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inActiveDate">有効開始日</param>
        /// <returns>フォーマットされた値</returns>
        public static string Format(string inUnitDivision, string inVenderDivision, string inVenderCd, decimal inValue, ComDB db, DateTime? inActiveDate = null)
        {
            // 数値桁数チェックマスタからデータを取得
            Dao.NumberChkDigitDetail results = new Dao.NumberChkDigitDetail();
            results = GetCheckDigit(inUnitDivision, inVenderDivision, inVenderCd, db, inActiveDate);

            // 小数点以下桁数
            int smallNum = int.Parse(results.SmallnumLength.ToString());
            // 端数区分
            decimal? roundDivision = results.RoundDivision;
            // フォーマットした処理を返す
            return Format(inValue, smallNum, roundDivision);
        }

        /// <summary>
        /// フォーマット処理
        /// </summary>
        /// <param name="inValue">フォーマット対象の数値</param>
        /// <param name="smallNum">小数点以下桁数</param>
        /// <param name="roundDivision">端数区分</param>
        /// <returns>フォーマットされた値</returns>
        public static string Format(decimal inValue, int smallNum, decimal? roundDivision)
        {
            // 丸め処理後の数値
            decimal val;
            // 丸め処理に使用
            int roundVal;
            string temp = "1";
            // 丸め処理計算数値生成
            for (int i = 0; i < smallNum; i++)
            {
                temp += "0";
            }
            roundVal = int.Parse(temp);

            // 端数区分に応じて丸め処理
            switch (roundDivision.ToString())
            {
                case "0":
                    // なし
                    val = inValue;
                    break;
                case "1":
                    // 切り捨て
                    val = Math.Floor(inValue * roundVal) / roundVal;
                    break;
                case "2":
                    // 四捨五入
                    val = Math.Round(inValue, smallNum, MidpointRounding.AwayFromZero);
                    break;
                case "3":
                    // 切り上げ
                    val = Math.Ceiling(inValue * roundVal) / roundVal;
                    break;
                default:
                    // 端数区分を取得できなかった場合は、端数区分:0、小数点以下桁数:2として処理する
                    val = inValue;
                    smallNum = 2;
                    break;
            }

            // 丸め処理済みの数値を3桁カンマ区切りの文字列にして返す
            return val.ToString(GetPattern(smallNum));
        }

        /// <summary>
        /// 数値桁数チェックマスタから主キーで検索する
        /// </summary>
        /// <param name="inUnitDivision">単位区分</param>
        /// <param name="inVenderDivision">取引先区分</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inActiveDate">有効開始日</param>
        /// <returns>検索結果</returns>
        public static IList<Dao.NumberChkDigitDetail> FindByPrimaryKey(string inUnitDivision, string inVenderDivision, string inVenderCd, ComDB db, DateTime? inActiveDate = null)
        {
            // SQL文生成
            var sql = "";
            sql = sql + "select number_chkdisit.unit_division,";
            sql = sql + " number_chkdisit.vender_division,";
            sql = sql + " number_chkdisit.vender_Cd,";
            sql = sql + " number_chkdisit.max_length,";
            sql = sql + " number_chkdisit.integer_length,";
            sql = sql + " number_chkdisit.smallnum_length,";
            sql = sql + " number_chkdisit.round_division,";
            sql = sql + " number_chkdisit.lower_limit,";
            sql = sql + " number_chkdisit.upper_limit,";
            sql = sql + " number_chkdisit.active_date,";
            sql = sql + " case when number_chkdisit.vender_division <> ' ' then 1  else 2 end order_division";
            sql = sql + "   from number_chkdisit";
            sql = sql + "   where number_chkdisit.unit_division = @UnitDivision";
            sql = sql + "   and number_chkdisit.vender_division = @VenderDivision";
            sql = sql + "   and number_chkdisit.vender_cd = @VenderCd";
            if (inActiveDate != null)
            {
                // 開始有効日が範囲内でない場合の対策
                sql += "    and number_chkdisit.active_date <= @VenderActiveDate";
                //sql += "    and number_chkdisit.active_date = @VenderActiveDate";
            }
            sql = sql + "   or ( number_chkdisit.unit_division = @UnitDivision";
            sql = sql + "   and number_chkdisit.vender_division = ' '";
            sql = sql + "   and number_chkdisit.vender_cd = ' ' )";
            sql = sql + "   order by";
            sql = sql + "   number_chkdisit.unit_division,";
            //sql = sql + "   number_chkdisit.vender_division desc,";
            sql = sql + "   order_division,";
            // 開始有効日が複数あった場合の対策
            sql = sql + "   number_chkdisit.active_date desc,";
            sql = sql + "   number_chkdisit.vender_cd";

            try
            {
                // SQL実行
                IList<Dao.NumberChkDigitDetail> list = new List<Dao.NumberChkDigitDetail>();
                if (inActiveDate != null)
                {
                    list = db.GetListByDataClass<Dao.NumberChkDigitDetail>(
                    sql,
                    new
                    {
                        UnitDivision = inUnitDivision,
                        VenderDivision = inVenderDivision,
                        VenderCd = inVenderCd,
                        VenderActiveDate = inActiveDate
                    });
                }
                else
                {
                    list = db.GetListByDataClass<Dao.NumberChkDigitDetail>(
                    sql,
                    new
                    {
                        UnitDivision = inUnitDivision,
                        VenderDivision = inVenderDivision,
                        VenderCd = inVenderCd
                    });
                }

                if (list != null && list.Count > 0)
                {
                    return list;
                }
                else
                {
                    // データがない場合はnull
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 数値桁数チェックマスタメンテから設定を取得する
        /// </summary>
        /// <param name="inUnitDivision">単位区分</param>
        /// <param name="inVenderDivision">取引先区分</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inActiveDate">有効開始日</param>
        /// <returns>数値桁数チェックマスタメンテの検索結果</returns>
        public static Dao.NumberChkDigitDetail GetCheckDigitDetailDao(string inUnitDivision, string inVenderDivision, string inVenderCd, ComDB db, DateTime? inActiveDate = null)
        {
            // 引数を使用してレコードを取得
            IList<Dao.NumberChkDigitDetail> results = FindByPrimaryKey(inUnitDivision, inVenderDivision, inVenderCd, db, inActiveDate);
            if (results == null)
            {
                // データの取得ができなかった場合は空のレコードにデフォルト値(整数部桁数12、小数部桁数2、全体行桁15)のみをセットして返却する。
                Dao.NumberChkDigitDetail result = new Dao.NumberChkDigitDetail();
                result.UnitDivision = null;
                result.VenderDivision = null;
                result.VenderCd = null;
                result.ActiveDate = DateTime.Now;
                result.MaxLength = decimal.Parse(Constants.NUMBER_CHKDISIT.INIT_MAX_LENGTH);
                result.IntegerLength = decimal.Parse(Constants.NUMBER_CHKDISIT.INIT_INTEGER_LENGTH);
                result.SmallnumLength = decimal.Parse(Constants.NUMBER_CHKDISIT.INIT_SMALLNUM_LENGTH);
                result.RoundDivision = decimal.Parse(Constants.NUMBER_CHKDISIT.INIT_ROUND_DIVISION);
                result.LowerLimit = decimal.Parse(Constants.NUMBER_CHKDISIT.INIT_LOWER_LIMIT);
                result.UpperLimit = decimal.Parse(Constants.NUMBER_CHKDISIT.INIT_UPPER_LIMIT);
                return result;
            }
            else
            {
                // レコードの先頭行を採用
                Dao.NumberChkDigitDetail result = new Dao.NumberChkDigitDetail();
                result.UnitDivision = results[0].UnitDivision;
                result.VenderDivision = results[0].VenderDivision;
                result.VenderCd = results[0].VenderCd;
                result.ActiveDate = results[0].ActiveDate;
                result.MaxLength = results[0].MaxLength;
                result.IntegerLength = results[0].IntegerLength;
                result.SmallnumLength = results[0].SmallnumLength;
                result.RoundDivision = results[0].RoundDivision;
                result.LowerLimit = results[0].LowerLimit;
                result.UpperLimit = results[0].UpperLimit;

                // レコードの値を再編集
                string lowerLimit = result.LowerLimit.ToString();
                // 下限値 小数点以下の末尾に0がある場合は除去
                int index = lowerLimit.IndexOf(".");
                if (index != -1)
                {
                    for (int i = lowerLimit.Length; i > index; i--)
                    {
                        if (lowerLimit.Substring(lowerLimit.Length - 1, 1) == "0" ||
                            lowerLimit.Substring(lowerLimit.Length - 1, 1) == ".")
                        {
                            lowerLimit = lowerLimit.Remove(lowerLimit.Length - 1, 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    result.LowerLimit = decimal.Parse(lowerLimit);
                }

                // 上限値 整数部桁数と小数部桁数から上限値を再作成
                string upperLimit = "";
                // 整数部作成
                for (int i = 0; i < int.Parse(result.IntegerLength.ToString()); i++)
                {
                    upperLimit += "9";
                }

                int smallLength = int.Parse(result.SmallnumLength.ToString());
                // 小数部作成
                if (smallLength > 0)
                {
                    upperLimit += ".";
                    for (int i = 0; i < smallLength; i++)
                    {
                        upperLimit += "9";
                    }
                }
                result.UpperLimit = decimal.Parse(upperLimit);
                return result;
            }
        }

        /// <summary>
        /// 値がnullの場合は代表取得用コード(半角スペース)を返す
        /// </summary>
        /// <param name="inStr">対象文字列</param>
        /// <returns>null:半角スペース / null以外:そのまま返す</returns>
        public static string GetAllCode(string inStr)
        {
            // 対象文字列がnullか判定
            if (string.IsNullOrEmpty(inStr))
            {
                // 半角スペースを返す
                return " ";
            }

            // 対象文字列をそのまま返す
            return inStr;
        }

        /// <summary>
        /// 数値桁数チェックマスタメンテから設定を取得する
        /// </summary>
        /// <param name="inUnitDivision">単位区分</param>
        /// <param name="inVenderDivision">取引先区分</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inActiveDate">有効開始日</param>
        /// <returns>数値桁数チェックマスタメンテの検索結果</returns>
        public static Dao.NumberChkDigitDetail GetCheckDigit(string inUnitDivision, string inVenderDivision, string inVenderCd, ComDB db, DateTime? inActiveDate = null)
        {
            // 取引先区分がnullの場合は半角スペースにする
            string venDivision = GetAllCode(inVenderDivision);
            // 取引先コードがnullの場合は半角スペースにする
            string venCd = GetAllCode(inVenderCd);
            // 単位区分、取引先区分、取引先コードを引数に設定取得
            return GetCheckDigitDetailDao(inUnitDivision, venDivision, venCd, db, inActiveDate);
        }

        /// <summary>
        /// 数値から3桁カンマ区切り有りの文字列に変換するための書式を生成する
        /// </summary>
        /// <param name="inDecimalPoint">小数点桁数</param>
        /// <returns>生成した書式</returns>
        public static string GetPattern(int inDecimalPoint)
        {
            StringBuilder buf = new StringBuilder(decimalPointFormatBase);

            // 指定された桁数分、基本書式に"#"を追加する
            for (int i = 0; i < inDecimalPoint; i++)
            {
                buf.Append("0");
            }

            return buf.ToString();
        }

        /// <summary>
        /// 数値桁数チェックマスタの設定に基づき、数値文字列の入力チェックを行う
        /// </summary>
        /// <param name="inUnitDivision">単位区分</param>
        /// <param name="inVenderDivision">取引先区分</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="inValue">チェック対象の数値文字列(3桁カンマ区切り有りを許容)</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inActiveDate">有効開始日</param>
        /// <returns>true:チェックOK false:エラー</returns>
        public static bool IsCheckDigit(string inUnitDivision, string inVenderDivision, string inVenderCd, string inValue, ComDB db, DateTime? inActiveDate = null)
        {
            int oouCode = 0;
            return CheckDigit(inUnitDivision, inVenderDivision, inVenderCd, inValue, ref oouCode, db, inActiveDate);
        }

        /// <summary>
        /// チェック対象の文字列が指定された全体桁数を超過していないかチェックする。
        /// </summary>
        /// <param name="inValue">チェック対象の数値文字列</param>
        /// <param name="inMaxLength">全体桁数</param>
        /// <param name="outCode">エラーコード(2:最大文字列長エラー)</param>
        /// <returns>true:チェックOK false:エラー</returns>
        public static bool CheckMaxLength(string inValue, int? inMaxLength, ref int outCode)
        {
            // 全体桁数がnullの場合はチェック不要とし、trueを返す
            if (inMaxLength == null)
            {
                return true;
            }

            // チェック対象の数値文字列の文字数が指定桁数を超過していればfalseを返す
            if (inValue.Length > inMaxLength)
            {
                outCode = ErrCode.ErrMaxLength;
                return false;
            }

            // 指定桁数以内であればtrueを返す
            return true;
        }

        /// <summary>
        /// チェック対象の文字列にて、整数部が指定された桁数を超過していないかチェックする
        /// </summary>
        /// <param name="inValue">チェック対象の数値文字列</param>
        /// <param name="inLength">整数部桁数</param>
        /// <param name="outCode">エラーコード(3:整数部桁数エラー)</param>
        /// <returns>true:チェックOK false:エラー</returns>
        public static bool CheckIntegerPart(string inValue, int? inLength, ref int outCode)
        {
            int index;

            // 整数部桁数がnullの場合はチェック不要とし、trueを返す
            if (inLength == null)
            {
                return true;
            }

            // 小数点があるかをチェック
            index = inValue.IndexOf(".");
            string val;
            if (index == -1)
            {
                val = inValue;
            }
            else
            {
                // 整数部だけを取得
                val = inValue.Substring(0, index);
            }

            // チェック対象の数値文字列の整数部の文字数が指定桁数を超過していればfalseを返す
            if (val.Length > inLength)
            {
                outCode = ErrCode.ErrIntegerPart;
                return false;
            }

            // 指定桁数以内であればtrueを返す
            return true;
        }

        /// <summary>
        /// チェック対象の文字列にて、小数部が指定された桁数を超過していないかチェックする
        /// </summary>
        /// <param name="inValue">チェック対象の数値文字列</param>
        /// <param name="inLength">小数部桁数</param>
        /// <param name="outCode">エラーコード(4:小数部桁数エラー)</param>
        /// <returns>true:チェックOK false:エラー</returns>
        public static bool CheckDecimalPart(string inValue, int? inLength, ref int outCode)
        {
            int index;

            // 小数部桁数がnullの場合は"2"として処理を継続する
            if (inLength == null)
            {
                inLength = 2;
            }

            // 小数点のインデックスを取得
            index = inValue.IndexOf(".");
            // 小数点を含まない場合はチェック不要とし、trueを返す
            if (index == -1)
            {
                return true;
            }

            //小数部の末尾にゼロがある場合は削除
            for (int i = inValue.Length; i > index; i--)
            {
                if (inValue.Substring(inValue.Length - 1, 1) == "0" ||
                    inValue.Substring(inValue.Length - 1, 1) == ".")
                {
                    inValue = inValue.Remove(inValue.Length - 1, 1);
                }
                else
                {
                    break;
                }
            }

            // 小数点を含まない場合はチェック不要とし、trueを返す
            if (inValue.IndexOf(".") == -1)
            {
                return true;
            }

            // 小数部だけを取得
            string val = inValue.Substring(index + 1, inValue.Length - index - 1);

            // // チェック対象の数値文字列の整数部の文字数が指定桁数を超過していればfalseを返す
            if (val.Length > inLength)
            {
                outCode = ErrCode.ErrDecimalPart;
                return false;
            }

            // 指定桁数以内であればtrueを返す
            return true;
        }

        /// <summary>
        /// チェック対象の数値が、指定された上限値/下限値を超過していないかチェックする
        /// </summary>
        /// <param name="inValue">チェック対象の数値</param>
        /// <param name="inLowerLimit">下限値</param>
        /// <param name="inUpperLimit">上限値</param>
        /// <param name="outCode">エラーコード(5:レンジエラー)</param>
        /// <returns>true:チェックOK false:エラー</returns>
        public static bool CheckRange(decimal inValue, decimal inLowerLimit, decimal inUpperLimit, ref int outCode)
        {
            // チェック対象の数値が上下限の範囲外ならfalseを返す
            if (inValue < inLowerLimit || inValue > inUpperLimit)
            {
                outCode = ErrCode.ErrRange;
                return false;
            }

            // 範囲以内ならtrueを返す
            return true;
        }

        /// <summary>
        /// チェック対象の数値が、指定された上限値/下限値を超過していないかチェックする
        /// </summary>
        /// <param name="inValue">チェック対象の数値</param>
        /// <param name="inLowerLimit">下限値</param>
        /// <param name="inUpperLimit">上限値</param>
        /// <param name="zeroFlg">ゼロ許可フラグ true:許可、false:禁止</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <returns>true:チェックOK false:エラー</returns>
        public static bool CheckRange(decimal inValue, decimal? inLowerLimit, decimal? inUpperLimit, bool zeroFlg, ref string errorMsg, string languageId, ComUtil.MessageResources msgResources)
        {
            // 下限値、上限値ともに未設定の場合、チェックを実施しない
            if (inLowerLimit == null && inUpperLimit == null)
            {
                return true;
            }
            else if (inLowerLimit == null && inUpperLimit != null)
            {
                // 上限値のみ設定されている場合
                if (inValue > inUpperLimit)
                {
                    // {0}以下の値を入力して下さい。
                    errorMsg = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00120, inUpperLimit.ToString() }, languageId, msgResources);
                    return false;
                }
            }
            else if (inLowerLimit != null && inUpperLimit == null)
            {
                // 下限値のみ設定されている場合
                // ０許可の場合
                if (zeroFlg)
                {
                    if (inValue < inLowerLimit)
                    {
                        // {0}以上の値を入力して下さい。
                        errorMsg = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00119, inLowerLimit.ToString() }, languageId, msgResources);
                        return false;
                    }
                }
                else
                {
                    // ０禁止の場合
                    if (inValue <= inLowerLimit)
                    {
                        // {0}より大きい値を入力して下さい。
                        errorMsg = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00131, inLowerLimit.ToString() }, languageId, msgResources);
                        return false;
                    }
                }
            }
            else
            {
                // チェック対象の数値が上下限の範囲外ならfalseを返す
                // ０許可の場合
                if (zeroFlg)
                {
                    if (inValue < inLowerLimit || inValue > inUpperLimit)
                    {
                        // {0}から{1}の範囲で入力して下さい。
                        errorMsg = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00118, inLowerLimit.ToString(), inUpperLimit.ToString() }, languageId, msgResources);
                        return false;
                    }
                }
                else
                {
                    // ０禁止の場合
                    if (inValue <= inLowerLimit || inValue >= inUpperLimit)
                    {
                        // {0}より大きい値から{1}の範囲で入力して下さい。
                        errorMsg = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00132, inLowerLimit.ToString(), inUpperLimit.ToString() }, languageId, msgResources);
                        return false;
                    }
                }
            }

            // 範囲以内ならtrueを返す
            return true;
        }

        /// <summary>
        /// 数値桁数チェックマスタの設定に基づき、数値文字列の入力チェックを行う
        /// </summary>
        /// <param name="inUnitDivision">単位区分</param>
        /// <param name="inValue">チェック対象の数値文字列(3桁カンマ区切り有りを許容)</param>
        /// <param name="outCode">エラーコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inActiveDate">有効開始日</param>
        /// <returns>true:チェックOK false:エラー</returns>
        public static bool CheckDigit(string inUnitDivision, string inValue, ref int outCode, ComDB db, DateTime? inActiveDate = null)
        {
            return CheckDigit(inUnitDivision, null, null, inValue, ref outCode, db, inActiveDate);
        }

        /// <summary>
        /// 数値チェックマスタの設定に基づき、数値文字列の入力チェックを行う
        /// </summary>
        /// <param name="inUnitDivision">単位区分</param>
        /// <param name="inVenderDivision">取引先区分</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="inValue">チェック対象の数値文字列(3桁カンマ区切り有りを許容)</param>
        /// <param name="outCode">エラーコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inActiveDate">有効開始日</param>
        /// <returns>0:チェックOK</returns>
        /// <returns>1:数値文字列エラー</returns>
        /// <returns>2:最大文字列エラー</returns>
        /// <returns>3:整数部桁数エラー</returns>
        /// <returns>4:小数部エラー</returns>
        /// <returns>5:レンジエラー</returns>
        public static bool CheckDigit(string inUnitDivision, string inVenderDivision, string inVenderCd, string inValue, ref int outCode, ComDB db, DateTime? inActiveDate = null)
        {
            // 数値に変換後のチェック対象文字
            decimal decVal;
            int output;

            // チェック対象文字列がnull,空文字の場合は、0(チェックOK)を返す
            if (string.IsNullOrEmpty(inValue))
            {
                outCode = ErrCode.ErrNone;
                return true;
            }

            // チェック対象文字列から3桁区切りのカンマを除去
            inValue = inValue.Replace(",", "");
            // 数値に変換できなければ、1(数値文字列エラー)を返す
            if (!decimal.TryParse(inValue, out decVal))
            {
                outCode = ErrCode.ErrToInt;
                return false;
            }

            // 数値桁数チェックマスタから設定を取得
            Dao.NumberChkDigitDetail list = GetCheckDigit(inUnitDivision, inVenderDivision, inVenderCd, db, inActiveDate);

            // 全体桁数チェック(falseの場合oCodeにエラーコード2が入る)
            int? maxLength = int.Parse(list.MaxLength.ToString());
            if (!CheckMaxLength(inValue, maxLength, ref outCode))
            {
                return false;
            }

            // 整数部桁数取得
            int? length = null;
            if (int.TryParse(list.IntegerLength.ToString(), out output))
            {
                length = output;
            }
            // 整数部桁数チェック(falseの場合にoCodeにエラーコード3が入る)
            if (!CheckIntegerPart(inValue, length, ref outCode))
            {
                return false;
            }

            // 小数部桁数取得
            length = null;
            if (int.TryParse(list.SmallnumLength.ToString(), out output))
            {
                length = output;
            }
            // 小数部桁数チェック(falseの場合にoCodeにエラーコード4が入る)
            if (!CheckDecimalPart(inValue, length, ref outCode))
            {
                return false;
            }

            // 上限値、下限値を取得
            decimal upperLimit = decimal.Parse(list.UpperLimit.ToString());
            decimal lowerLimit = decimal.Parse(list.LowerLimit.ToString());
            // レンジチェック(falseの場合にoCodeにエラーコード5が入る)
            if (!CheckRange(decVal, lowerLimit, upperLimit, ref outCode))
            {
                return false;
            }

            // 正常な値の場合は、0(チェックOK)を返す
            outCode = ErrCode.ErrNone;
            return true;
        }

        /// <summary>
        /// 数値桁数チェックマスタの設定に基づき、数値文字列の入力チェックを実行
        /// </summary>
        /// <param name="inUnitDivision">単位区分</param>
        /// <param name="inVenderDivision">取引先区分</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="inValue">チェック対象の数値文字列</param>
        /// <param name="inItemName">チェック対象の項目名</param>
        /// <param name="outMessage">エラーメッセージ</param>
        /// <param name="inRow">チェック対象の行番号</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="inActiveDate">有効開始日</param>
        /// <returns>true:チェックOK false:エラー</returns>
        public static bool CheckDigitMessage(string inUnitDivision, string inVenderDivision, string inVenderCd, string inValue, string inItemName,
            ref string outMessage, int? inRow, ComDB db, DateTime? inActiveDate = null)
        {
            // エラーコード
            int errCode = 0;

            // エラーが無ければtrueを返して終了
            if (CheckDigit(inUnitDivision, inVenderDivision, inVenderCd, inValue, ref errCode, db, inActiveDate))
            {
                return true;
            }

            // 数値桁数チェックマスタからデータ取得
            Dao.NumberChkDigitDetail list = GetCheckDigit(inUnitDivision, inVenderDivision, inVenderCd, db, inActiveDate);
            string maxLength = list.MaxLength.ToString();
            string integerLength = list.IntegerLength.ToString();
            string smallnumLength = list.SmallnumLength.ToString();
            string lowerLimit = list.LowerLimit.ToString();
            string upperLimit = list.UpperLimit.ToString();

            // エラーコードからエラーメッセージ生成(行番号が設定されているかも判定)
            switch (errCode)
            {
                case ErrCode.ErrToInt:
                    // 1:数値文字列エラー
                    if (inRow == null)
                    {
                        outMessage = string.Format(ErrMessage.M00320, inItemName);
                    }
                    else
                    {
                        outMessage = string.Format(ErrMessage.M00337, inItemName, inRow.ToString());
                    }
                    break;
                case ErrCode.ErrMaxLength:
                    // 2:最大文字列長エラー
                    if (inRow == null)
                    {
                        outMessage = string.Format(ErrMessage.M00043, inItemName, maxLength);
                    }
                    else
                    {
                        outMessage = string.Format(ErrMessage.M00340, inItemName, maxLength, inRow.ToString());
                    }

                    break;
                case ErrCode.ErrIntegerPart:
                    // 3:整数部桁数エラー
                    if (inRow == null)
                    {
                        outMessage = string.Format(ErrMessage.M00535, inItemName, integerLength);
                    }
                    else
                    {
                        outMessage = string.Format(ErrMessage.M00558, inItemName, integerLength, inRow.ToString());
                    }
                    break;
                case ErrCode.ErrDecimalPart:
                    // 4:小数部桁数エラー
                    if (inRow == null)
                    {
                        outMessage = string.Format(ErrMessage.M00536, inItemName, smallnumLength);
                    }
                    else
                    {
                        outMessage = string.Format(ErrMessage.M00559, inItemName, smallnumLength, inRow.ToString());
                    }
                    break;
                case ErrCode.ErrRange:
                    // レンジエラー
                    if (inRow == null)
                    {
                        outMessage = string.Format(ErrMessage.M00537, inItemName, lowerLimit, upperLimit);
                    }
                    else
                    {
                        outMessage = string.Format(ErrMessage.M00341, inItemName, lowerLimit, upperLimit, inRow.ToString());
                    }
                    break;
            }
            // エラーの場合はfalseを返す
            return false;
        }

        /// <summary>
        /// 数値桁数チェックマスタの端数区分設定に基づき、数値を丸める
        /// </summary>
        /// <param name="inUnitDivision">単位区分</param>
        /// <param name="inVenderDivision">取引先区分</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="inValue">丸め処理対象の数値</param>
        /// <param name="inRoundDivision">強制端数区分</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>丸め処理後の値</returns>
        /// <param name="inActiveDate">有効開始日</param>
        public static decimal Round(string inUnitDivision, string inVenderDivision, string inVenderCd, decimal inValue, int? inRoundDivision, ComDB db, DateTime? inActiveDate = null)
        {
            // 単位区分がnullの場合は無変換で数値を返す
            if (string.IsNullOrEmpty(inUnitDivision))
            {
                return inValue;
            }

            // 数値桁数チェックマスタからデータ取得
            Dao.NumberChkDigitDetail list = GetCheckDigit(inUnitDivision, inVenderDivision, inVenderCd, db, inActiveDate);

            // 丸め処理後の数値
            decimal val = 0;
            // 丸め処理に使用
            int roundVal;
            string temp = "1";
            // 端数区分
            string roundDivision;
            // 小数点以下桁数
            int smallNum = int.Parse(list.SmallnumLength.ToString());
            // 丸め処理計算数値生成
            for (int i = 0; i < smallNum; i++)
            {
                temp += "0";
            }
            roundVal = int.Parse(temp);

            // 端数区分を取得(引数に設定されていた場合はそのまま使用)
            if (inRoundDivision != null)
            {
                roundDivision = inRoundDivision.ToString();
            }
            else
            {
                roundDivision = list.RoundDivision.ToString();
            }

            // 端数区分に応じて丸め処理
            switch (roundDivision)
            {
                case "0":
                    // なし
                    val = inValue;
                    break;
                case "1":
                    // 切り捨て
                    val = Math.Floor(inValue * roundVal) / roundVal;
                    break;
                case "2":
                    // 四捨五入
                    val = Math.Round(inValue, smallNum, MidpointRounding.AwayFromZero);
                    break;
                case "3":
                    // 切り上げ
                    val = Math.Ceiling(inValue * roundVal) / roundVal;
                    break;
            }
            // 丸め処理後の値を返す
            return val;
        }

        /// <summary>
        /// 数値桁数チェックマスタの端数区分設定に基づき、数値を丸める
        ///  マスタ情報はバッチでは内部で保持する（I/O回数を減らす為）
        /// </summary>
        /// <param name="inValue">丸め処理対象の数値</param>
        /// <param name="inSmallNum">小数点以下桁数</param>
        /// <param name="inRoundDivision">端数区分</param>
        /// <returns>丸め処理後の値</returns>
        public static decimal RoundBatch(decimal inValue, int inSmallNum, decimal inRoundDivision)
        {
            // 丸め処理後の数値
            decimal val = 0;
            // 丸め処理に使用
            int roundVal;
            string temp = "1";
            // 丸め処理計算数値生成
            for (int i = 0; i < inSmallNum; i++)
            {
                temp += "0";
            }
            roundVal = int.Parse(temp);

            // 端数区分に応じて丸め処理
            switch (inRoundDivision.ToString())
            {
                case "0":
                    // なし
                    val = inValue;
                    break;
                case "1":
                    // 切り捨て
                    val = Math.Floor(inValue * roundVal) / roundVal;
                    break;
                case "2":
                    // 四捨五入
                    val = Math.Round(inValue, inSmallNum, MidpointRounding.AwayFromZero);
                    break;
                case "3":
                    // 切り上げ
                    val = Math.Ceiling(inValue * roundVal) / roundVal;
                    break;
            }
            // 丸め処理後の値を返す
            return val;
        }

        /// <summary>
        /// 端数処理クラス
        /// </summary>
        public class CheckDigitInfo
        {
            /// <summary>Gets 端数情報格納リスト</summary>
            /// <value>端数情報格納リスト</value>
            public Dictionary<string, Dao.NumberChkDigitDetail> CheckDigit { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CheckDigitInfo()
            {
                // 端数情報格納リストを初期化
                this.CheckDigit = new Dictionary<string, APCheckDigitDataClass.NumberChkDigitDetail>();
            }

            /// <summary>
            /// 数値桁数チェックマスタのは数区分、小数点以下桁数に基づき、数値をフォーマットする
            /// </summary>
            /// <param name="inUnitDivision">単位区分</param>
            /// <param name="inValue">フォーマット対象の数値</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>フォーマット後の値</returns>
            public string Format(string inUnitDivision, decimal inValue, ComDB db)
            {
                // 指定されて書式にフォーマットする
                return Format(inUnitDivision, null, null, inValue, db);
            }

            /// <summary>
            /// 数値桁数チェックマスタの端数区分、小数点以下桁数に基づき、数値をフォーマットする
            /// </summary>
            /// <param name="inUnitDivision">単位区分</param>
            /// <param name="inVenderDivision">取引先区分</param>
            /// <param name="inVenderCd">取引先コード</param>
            /// <param name="inValue">フォーマット対象の数値</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="inActiveDate">有効開始日</param>
            /// <returns>フォーマット後の値</returns>
            public string Format(string inUnitDivision, string inVenderDivision, string inVenderCd, decimal inValue, ComDB db, DateTime? inActiveDate = null)
            {
                // 数値桁数チェックマスタからデータを取得
                Dao.NumberChkDigitDetail results = new Dao.NumberChkDigitDetail();

                // 既に検索済みの場合、退避した端数データを用いて処理を行う
                var keyInfo = "";
                keyInfo += inUnitDivision != null ? inUnitDivision : "";
                keyInfo += "|";
                keyInfo += inVenderDivision != null ? inVenderDivision : "";
                keyInfo += "|";
                keyInfo += inVenderCd != null ? inVenderCd : "";
                keyInfo += "|";
                keyInfo += inActiveDate != null ? ((DateTime)inActiveDate).ToString("yyyy/MM/dd HH:mm:ss") : ""; // キー情報を生成
                if (CheckDigit.ContainsKey(keyInfo))
                {
                    results = CheckDigit[keyInfo];
                }
                else
                {
                    // 検索を行い、ディクショナリーに退避
                    results = GetCheckDigit(inUnitDivision, inVenderDivision, inVenderCd, db, inActiveDate);
                    CheckDigit.Add(keyInfo, results);
                }

                // 小数点以下桁数
                int smallNum = int.Parse(results.SmallnumLength.ToString());
                // 端数区分
                decimal? roundDivision = results.RoundDivision;
                // フォーマットした処理を返す
                return Format(inValue, smallNum, roundDivision);
            }

            /// <summary>
            /// 数値桁数チェックマスタメンテから設定を取得する
            /// </summary>
            /// <param name="inUnitDivision">単位区分</param>
            /// <param name="inVenderDivision">取引先区分</param>
            /// <param name="inVenderCd">取引先コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="inActiveDate">有効開始日</param>
            /// <returns>フォーマット後の値</returns>
            public Dao.NumberChkDigitDetail GetCheckDigitInfo(string inUnitDivision, string inVenderDivision, string inVenderCd, ComDB db, DateTime? inActiveDate = null)
            {
                // 数値桁数チェックマスタからデータを取得
                Dao.NumberChkDigitDetail results = new Dao.NumberChkDigitDetail();

                // 既に検索済みの場合、退避した端数データを用いて処理を行う
                var keyInfo = "";
                keyInfo += inUnitDivision != null ? inUnitDivision : "";
                keyInfo += "|";
                keyInfo += inVenderDivision != null ? inVenderDivision : "";
                keyInfo += "|";
                keyInfo += inVenderCd != null ? inVenderCd : "";
                keyInfo += "|";
                keyInfo += inActiveDate != null ? ((DateTime)inActiveDate).ToString("yyyy/MM/dd HH:mm:ss") : ""; // キー情報を生成
                if (CheckDigit.ContainsKey(keyInfo))
                {
                    results = CheckDigit[keyInfo];
                }
                else
                {
                    // 検索を行い、ディクショナリーに退避
                    // 取引先区分がnullの場合は半角スペースにする
                    string venDivision = GetAllCode(inVenderDivision);
                    // 取引先コードがnullの場合は半角スペースにする
                    string venCd = GetAllCode(inVenderCd);
                    // 単位区分、取引先区分、取引先コードを引数に設定取得
                    results = GetCheckDigitDetailDao(inUnitDivision, venDivision, venCd, db, inActiveDate);

                    CheckDigit.Add(keyInfo, results);
                }
                return results;
            }

            /// <summary>
            /// フォーマット処理
            /// </summary>
            /// <param name="inValue">フォーマット対象の数値</param>
            /// <param name="smallNum">小数点以下桁数</param>
            /// <param name="roundDivision">端数区分</param>
            /// <returns>丸め処理済みの数値</returns>
            public string Format(decimal inValue, int smallNum, decimal? roundDivision)
            {
                // 丸め処理後の数値
                decimal val;
                // 丸め処理に使用
                int roundVal;
                string temp = "1";
                // 丸め処理計算数値生成
                for (int i = 0; i < smallNum; i++)
                {
                    temp += "0";
                }
                roundVal = int.Parse(temp);

                // 端数区分に応じて丸め処理
                switch (roundDivision.ToString())
                {
                    case "0":
                        // なし
                        val = inValue;
                        break;
                    case "1":
                        // 切り捨て
                        val = Math.Floor(inValue * roundVal) / roundVal;
                        break;
                    case "2":
                        // 四捨五入
                        val = Math.Round(inValue, smallNum, MidpointRounding.AwayFromZero);
                        break;
                    case "3":
                        // 切り上げ
                        val = Math.Ceiling(inValue * roundVal) / roundVal;
                        break;
                    default:
                        // 端数区分を取得できなかった場合は、端数区分:0、小数点以下桁数:2として処理する
                        val = inValue;
                        smallNum = 2;
                        break;
                }

                // 丸め処理済みの数値を3桁カンマ区切りの文字列にして返す
                return val.ToString(GetPattern(smallNum));
            }
        }

        /// <summary>
        /// 数値チェックマスタの設定に基づき、数値文字列の入力チェックを行う
        /// </summary>
        /// <param name="numberChkDigit">数値桁数チェックマスタ</param>
        /// <param name="inUnitDivision">区分</param>
        /// <param name="inValue">数量</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="minusFlg">マイナス許可フラグ ※true:許可、false:禁止 エラーメッセージで最小値を0で設定するため使用</param>
        /// <param name="zeroFlg">ゼロ許可フラグ ※true:許可、false:禁止</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool CheckDigit(IList<ComNCD.NumberChkdisitEntity> numberChkDigit, string inUnitDivision,
            string inValue, ref string errorMsg, ComDB db, string languageId, ComUtil.MessageResources msgResources, bool minusFlg = false, bool zeroFlg = true)
        {
            return CheckDigit(numberChkDigit, inUnitDivision, null, null, inValue, ref errorMsg, db, languageId, msgResources, null, minusFlg, zeroFlg);
        }

        /// <summary>
        /// 数値チェックマスタの設定に基づき、数値文字列の入力チェックを行う
        /// </summary>
        /// <param name="numberChkDigit">数値桁数チェックマスタ</param>
        /// <param name="inUnitDivision">区分</param>
        /// <param name="inVenderDivision">取引先区分</param>
        /// <param name="inVenderCd">取引先コード</param>
        /// <param name="inValue">数量</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="inActiveDate">有効開始日</param>
        /// <param name="minusFlg">マイナス許可フラグ ※true:許可、false:禁止 エラーメッセージで最小値を0で設定するため使用</param>
        /// <param name="zeroFlg">ゼロ許可フラグ ※true:許可、false:禁止</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool CheckDigit(IList<ComNCD.NumberChkdisitEntity> numberChkDigit, string inUnitDivision, string inVenderDivision,
            string inVenderCd, string inValue, ref string errorMsg, ComDB db, string languageId, ComUtil.MessageResources msgResources, DateTime? inActiveDate = null, bool minusFlg = false, bool zeroFlg = true)
        {
            // チェック対象が未設定の場合、処理を実施しない
            if (string.IsNullOrEmpty(inValue))
            {
                return true;
            }

            // 数値に変換できなければ、1(数値文字列エラー)を返す
            // 数値に変換後のチェック対象文字
            decimal decVal;
            if (!decimal.TryParse(inValue, out decVal))
            {
                // 数値で入力して下さい。
                errorMsg = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00109 }, languageId, msgResources);
                return false;
            }

            // 取引先区分がnullの場合は半角スペースにする
            string venderDivision = GetAllCode(inVenderDivision);
            // 取引先コードがnullの場合は半角スペースにする
            string venderCd = GetAllCode(inVenderCd);

            // 桁数チェックマスタから該当のレコードを取得
            ComNCD.NumberChkdisitEntity chkDigit = new ComNCD.NumberChkdisitEntity();
            if (inActiveDate == null)
            {
                chkDigit = numberChkDigit.Where(x => x.UnitDivision.Equals(inUnitDivision) &&
                    x.VenderDivision.Equals(venderDivision) && x.VenderCd.Equals(venderCd)).FirstOrDefault();
            }
            else
            {
                // 開始有効日が範囲内でない場合の対策
                // chkDigit = numberChkDigit.Where(x => x.UnitDivision.Equals(inUnitDivision) &&
                //    x.VenderDivision.Equals(venderDivision) && x.VenderCd.Equals(venderCd) && x.ActiveDate.Equals(inActiveDate)).FirstOrDefault();
                chkDigit = numberChkDigit.Where(x => x.UnitDivision.Equals(inUnitDivision) &&
                    x.VenderDivision.Equals(venderDivision) && x.VenderCd.Equals(venderCd) && x.ActiveDate <= inActiveDate).OrderByDescending(x => x.ActiveDate).FirstOrDefault();
            }

            if (chkDigit == null)
            {
                // 存在しない場合、取引先区分、取引先コードを半角スペースにして検索
                chkDigit = numberChkDigit.Where(x => x.UnitDivision.Equals(inUnitDivision) && x.VenderDivision.Equals(" ") && x.VenderCd.Equals(" ")).FirstOrDefault();
            }

            if (chkDigit == null)
            {
                // 存在しない場合、チェックを実施しない
                return true;
            }

            // チェック対象文字列から3桁区切りのカンマを除去
            inValue = inValue.Replace(",", "");

            // 全体桁数チェック
            int outCode = 0;
            if (!CheckMaxLength(inValue, chkDigit.MaxLength, ref outCode))
            {
                // {0}桁以下で入力して下さい。;
                errorMsg = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00128, chkDigit.MaxLength.ToString() }, languageId, msgResources);
                return false;
            }

            // 整数部桁数チェック
            if (!CheckIntegerPart(inValue, chkDigit.IntegerLength, ref outCode))
            {
                // 整数部は{0}桁までで入力して下さい。;
                errorMsg = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00129, chkDigit.IntegerLength.ToString() }, languageId, msgResources);
                return false;
            }

            // 小数部桁数チェック
            if (!CheckDecimalPart(inValue, chkDigit.SmallnumLength, ref outCode))
            {
                // 小数部は{0}桁までで入力して下さい。;
                errorMsg = ComUtil.GetPropertiesMessage(new string[] { APResources.ID.MS00130, chkDigit.SmallnumLength.ToString() }, languageId, msgResources);
                return false;
            }

            // 範囲チェック
            decimal? lowerLimit = chkDigit.LowerLimit;
            if (!minusFlg)
            {
                if (lowerLimit == null || (lowerLimit != null && lowerLimit < decimal.Zero) || lowerLimit == 0)
                {
                    lowerLimit = decimal.Zero;
                }
            }
            if (!CheckRange(decVal, lowerLimit, chkDigit.UpperLimit, zeroFlg, ref errorMsg, languageId, msgResources))
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}