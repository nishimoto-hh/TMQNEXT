///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　object操作クラス
/// 説明　　　：　object操作の共通処理を実装します。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

using System;

namespace CommonWebTemplate.CommonUtil
{
    public class ObjectUtil
    {
        #region === 定数定義 ===
        #endregion

        #region === public static処理 ===
        /// <summary>
        /// 指定されたﾌﾟﾛﾊﾟﾃｨ名の値を取得する
        /// </summary>
        /// <returns>ﾌｫﾙﾀﾞﾊﾟｽ</returns>
        public static string GetPropertyValue(object targetObj, string propertyName)
        {
            string retVal = String.Empty;
            try
            {
                var property = targetObj.GetType().GetProperty(propertyName);
                retVal = property.GetValue(targetObj).ToString();
            }
            catch (Exception ex)
            {
                //何もしない
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
            return retVal;
        }

        /// <summary>
        /// ｵﾌﾞｼﾞｪｸﾄの同じﾌﾟﾛﾊﾟﾃｨ名の値をｺﾋﾟｰする
        /// </summary>
        /// <param name="fromSource">ｺﾋﾟｰ元ｵﾌﾞｼﾞｪｸﾄ</param>
        /// <param name="toSource">ｺﾋﾟｰ先ｵﾌﾞｼﾞｪｸﾄ</param>
        /// <param name="isUpper">ｺﾋﾟｰ元ｵﾌﾞｼﾞｪｸﾄのﾌﾟﾛﾊﾟﾃｨ名を大文字にして一致するか比較</param>
        public static void Copy(object fromSource, object toSource, bool isUpper = false)
        {
            try
            {
                foreach (var property in fromSource.GetType().GetProperties())
                {
                    var fromVal = property.GetValue(fromSource);

                    string propertyName = (isUpper ? property.Name.ToUpper() : property.Name);
                    var toProperty = toSource.GetType().GetProperty(propertyName);
                    if (toProperty != null)
                    {
                        toProperty.SetValue(toSource, fromVal);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                //何もしない
            }
        }
        #endregion

        #region === private static処理 ===
        #endregion

    }
}