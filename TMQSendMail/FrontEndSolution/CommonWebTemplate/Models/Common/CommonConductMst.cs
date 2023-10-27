using System.Collections.Generic;

namespace CommonWebTemplate.Models.Common
{
    public class CommonConductMst
    {
        /// <summary>
        /// 機能ﾏｽﾀ情報、もしくは処理ｸﾞﾙｰﾌﾟ情報
        /// </summary>
        public COM_CONDUCT_MST CONDUCTMST { get; set; }

        /// <summary>
        /// 機能ﾏｽﾀに紐づく画面定義ﾘｽﾄ
        /// </summary>
        public IList<CommonFormDefine> FORMDEFINES { get; set; }

        /// <summary>
        /// 処理ｸﾞﾙｰﾌﾟに紐づく機能ﾏｽﾀ一覧
        /// </summary>
        /// <remarks>処理ｸﾞﾙｰﾌﾟ情報の場合のみ設定されるはず</remarks>
        public IList<CommonConductMst> CHILDCONDUCTMST { get; set; }

        /// <summary>
        /// 機能ﾏｽﾀに紐づく共通機能画面機能マスタ情報
        /// </summary>
        public IList<CommonConductMst> CM_CONDUCTMSTS { get; set; }

        /* ボタン権限制御 切替 start ==================================================================== */
        ///// <summary>
        ///// 機能ﾏｽﾀに紐づくﾕｰｻﾞｰ処理権限
        ///// </summary>
        ///// <remarks>KEY：FormNo、VALUE：ﾎﾞﾀﾝ権限情報</remarks>
        ///// <remarks> - ﾎﾞﾀﾝ権限情報＞KEY：ﾎﾞﾀﾝCTRLID、VALUE：0(非表示),1(表示・活性),2(表示・非活性)</remarks>
        //public Dictionary<short, Dictionary<string, int>> UserAuthConductShoris;
        /* ================================================================================================= */
        /// <summary>
        /// 機能ﾏｽﾀに紐づくﾕｰｻﾞｰ処理権限
        /// </summary>
        /// <remarks> - ﾎﾞﾀﾝ権限情報ﾘｽﾄ</remarks>
        /// <remarks> - ﾎﾞﾀﾝ権限情報 KEYS</remarks>
        /// <remarks> - FORMNO  ：画面NO</remarks>
        /// <remarks> - CTRLID  ：ﾎﾞﾀﾝｺﾝﾄﾛｰﾙID</remarks>
        /// <remarks> - DISPKBN ：ﾎﾞﾀﾝ表示区分(USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF)＞Hide(0：非表示),Active(1：活性),Disabled(2：非活性)</remarks>
        /// <remarks> - AUTHKBN ：承認区分＞11(承認依頼),12(承認依頼取消),21(承認),22(承認取消),8(否認),-(承認系ﾎﾞﾀﾝ以外)</remarks>
        public List<Dictionary<string, object>> UserAuthConductShoris;
        /* ボタン権限制御 切替 end ==================================================================== */

        /// <summary>
        /// 翻訳辞書
        /// </summary>
        /// <remarks>KEY：翻訳ID、VALUE：リソース翻訳結果</remarks>
        public Dictionary<string, string> TransDictionary;

        /// <summary>
        /// 場所階層リスト
        /// </summary>
        public List<CommonStructure> StructureList { get; set; }

        /// <summary>
        /// 1ページ当たりの行数リスト(コンボ設定用)
        /// </summary>
        public List<int> PageRowsList  { get; set; }

        /// <summary>
        /// 言語リスト(コンボ設定用)
        /// </summary>
        public List<LanguageInfo> LanguageComboList { get; set; }

        #region === ｺﾝｽﾄﾗｸﾀ ===
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public CommonConductMst()
        {

        }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public CommonConductMst(COM_CONDUCT_MST conductmst)
        {
            CONDUCTMST = conductmst;
        }
        #endregion

    }
}