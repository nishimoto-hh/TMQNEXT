///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　ｱﾌﾟﾘｹｰｼｮﾝ制御用定数ｸﾗｽ
/// 説明　　　：　ｱﾌﾟﾘｹｰｼｮﾝ制御に使用する定数を定義します。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

namespace CommonWebTemplate.CommonUtil
{
    /// <summary>
    /// ｱﾌﾟﾘｹｰｼｮﾝ制御用定数
    /// </summary>
    public class AppConstants
    {
        #region 画面ﾚｲｱｳﾄ関連
        /// <summary>
        /// 繰り返し表示項目（N数表示）折り返し項目数
        /// </summary>
        public const int ItemCntVCnt = 5;
        /// <summary>
        /// ボタン幅固定文字数
        /// </summary>
        public const int ButtonWidthFixedStrLen = 5;
        #endregion

        #region ｼｽﾃﾑﾌｫﾙﾀﾞ関連
        /// <summary>
        /// 一時ﾌｧｲﾙﾌｫﾙﾀﾞﾊﾟｽ
        /// </summary>
        public const string TempFileMapPath = @"TempFile";

        /// <summary>Excel共通ﾏｸﾛﾌｧｲﾙﾊﾟｽ</summary>
        public const string ExcelTemplateMapPath = @"Template";
        #endregion

        #region ｼｽﾃﾑ制御
        /// <summary>
        /// ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ制御
        /// </summary>
        /// <remarks>非同期に設定すると、ﾌｧｲﾙ作成を非同期で行います</remarks>
        public enum FileDownloadSet
        {
            /// <summary>ﾌｧｲﾙ作成、およびﾀﾞｳﾝﾛｰﾄﾞ同期処理</summary>
            Douki = 0,
            /// <summary>ﾌｧｲﾙ作成、およびﾀﾞｳﾝﾛｰﾄﾞ非同期処理</summary>
            Hidouki = 1,
        };
        #endregion

    }

}
