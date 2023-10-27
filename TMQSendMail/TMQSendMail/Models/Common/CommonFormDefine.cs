using System.Collections.Generic;

namespace CommonWebTemplate.Models.Common
{
    public class CommonFormDefine
    {
        /// <summary>
        /// 画面定義情報
        /// </summary>
        public COM_FORM_DEFINE FORMDEFINE { get; set; }

        /// <summary>
        /// 画面定義(一覧)に紐づく一覧項目定義情報
        /// </summary>
        public IList<COM_LISTITEM_DEFINE> LISTITEMDEFINES { get; set; }

        /// <summary>
        /// 画面定義(一覧)に紐づくコントロールグループ画面定義情報
        /// </summary>
        public IList<CommonFormDefine> CTR_FORMDEFINES { get; set; }

        /// <summary>
        /// 画面定義(一覧)に紐づく一覧項目ユーザ情報
        /// </summary>
        public IList<COM_LISTITEM_USER> LISTITEMUSERS { get; set; }

        /// <summary>
        /// 画面定義(一覧)に紐づく条件ﾃﾞｰﾀ
        /// </summary>
        /// <remarks>ﾍﾞﾀ表出力制御用</remarks>
        public Dictionary<string, object> ConditionData;
        ///// <summary>
        ///// 画面定義(一覧)に紐づく明細ﾃﾞｰﾀ
        ///// </summary>
        ///// <remarks>ﾍﾞﾀ表出力制御用</remarks>
        //public IList<COM_TMPTBL_DATA> DetailData;

        #region === ｺﾝｽﾄﾗｸﾀ ===
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public CommonFormDefine(COM_FORM_DEFINE formdefine)
        {
            FORMDEFINE = formdefine;
        }
        #endregion
    }
}