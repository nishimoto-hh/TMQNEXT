///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　管理SQL制御用データクラス
/// 説明　　　：　SQLIDで管理されたSQL文を発行して取得されるデータの型を定義します。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

using System.Collections.Generic;

namespace CommonWebTemplate.Models.Common
{
    public class CommonListItemInfo
    {

        /// <summary>
        /// 一覧CTRLID
        /// </summary>
        public string CTRLID { get; set; }
        /// <summary>
        /// 列番号(※VAL1～)
        /// </summary>
        public string COLNO { get; set; }
        

        public List<CommonSqlKanriData> ITEMLIST { get; set; }

        ///// <summary>
        ///// 選択ﾘｽﾄ:値
        ///// </summary>
        //public string CODE { get; set; }
        ///// <summary>
        ///// 選択ﾘｽﾄ:表示値
        ///// </summary>
        //public string VALUE { get; set; }
    }
}