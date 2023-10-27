namespace CommonWebTemplate.Models.Common
{
    using System;
    //using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class COM_LISTITEM_DEFINE
    {
        public string PGMID { get; set; }
        
        public short FORMNO { get; set; }
        
        public string CTRLID { get; set; }
        
        public short DEFINETYPE { get; set; }
        
        public short ITEMNO { get; set; }
        
        public string ITEMNAME { get; set; }

        public short DISPKBN { get; set; }

        public short ROWNO { get; set; }

        public short COLNO { get; set; }

        public short ROWSPAN { get; set; }

        public short COLSPAN { get; set; }

        public Nullable<short> HEADER_ROWSPAN { get; set; }

        public Nullable<short> HEADER_COLSPAN { get; set; }

        public string POSITION { get; set; }
        //public short HEADER_ALIGN { get; set; }

        /// <summary>
        /// 表示位置(数字)から文字列の表示位置を取得する処理
        /// </summary>
        /// <param name="isHeader">ヘッダの場合True</param>
        /// <returns>left,center,right</returns>
        public string GetTextAlign(bool isHeader)
        {
            // |区切りの場合分割し、ヘッダとデータで取得
            if (this.POSITION.IndexOf("|") >= 0)
            {
                // |区切りでヘッダとデータがそれぞれ指定されている場合
                int idx = isHeader ? 0 : 1;
                string target = this.POSITION.Split('|')[idx];
                return setAlign(target);
            }
            // 区切られていない場合はそのまま取得
            return setAlign(this.POSITION);

            string setAlign(string dataPosition)
            {
                var align = "";
                //データの表示位置を設定する
                switch (dataPosition)
                {
                    case LISTITEM_DEFINE_CONSTANTS.HEADER_ALIGN.left:
                        align = "left";
                        break;
                    case LISTITEM_DEFINE_CONSTANTS.HEADER_ALIGN.center:
                        align = "center";
                        break;
                    case LISTITEM_DEFINE_CONSTANTS.HEADER_ALIGN.right:
                        align = "right";
                        break;
                    default:
                        align = "left";
                        break;
                }
                return align;
            }
        }

        public string CELLTYPE { get; set; }

        public string COLWIDTH { get; set; }

        public short FROMTOKBN { get; set; }

        public short ITEM_CNT { get; set; }
        
        public string INITVAL { get; set; }

        public short NULLCHKKBN { get; set; }
        
        public string MINVAL { get; set; }
        
        public string MAXVAL { get; set; }
        
        public string FORMAT { get; set; }

        public short MAXLENGTH { get; set; }

        public short TXT_AUTOCOMPKBN { get; set; }
        
        public string BTN_CTRLID { get; set; }

        public short BTN_ACTIONKBN { get; set; }

        public short BTN_AUTHCONTROLKBN { get; set; }

        public short BTN_AFTEREXECKBN { get; set; }
        
        public string BTN_MESSAGE { get; set; }

        public short DAT_TRANSITION_PATTERN { get; set; }

        public short DAT_TRANSITION_ACTION_DIVISION { get; set; }

        public string RELATIONID { get; set; }
        
        public string RELATIONPARAM { get; set; }
        
        public string OPTIONINFO { get; set; }

        public short UNCHANGEABLEKBN { get; set; }

        public short COLFIXKBN { get; set; }

        public short FILTERUSEKBN { get; set; }

        public short SORT_DIVISION { get; set; }

        //public Nullable<bool> DETAILED_SEARCH_FLG { get; set; }
        public Nullable<short> DETAILED_SEARCH_DIVISION { get; set; }

        public string DETAILED_SEARCH_CELLTYPE { get; set; }

        public Nullable<bool> ITEM_CUSTOMIZE_FLG { get; set; }

        public string TXT_PLACEHOLDER { get; set; }
        
        public string CSSNAME { get; set; }
        
        public string TOOLTIP { get; set; }
        
        public string EXP_KEY_NAME { get; set; }
        
        public string EXP_TABLE_NAME { get; set; }
        
        public string EXP_COL_NAME { get; set; }
        
        public string EXP_PARAM_NAME { get; set; }
        
        public string EXP_ALIAS_NAME { get; set; }

        public short EXP_LIKE_PATTERN { get; set; }

        public short EXP_IN_CLAUSE_KBN { get; set; }

        public short EXP_LOCK_TYPE { get; set; }

        public bool DELFLG { get; set; }

        //public int UPDYMD { get; set; }

        //public int UPDHMIS { get; set; }
        
        //public string UPDID { get; set; }

        public int LOCATION_LAYER_ID { get; set; }
        
        public string ITEMID { get; set; }

        //public string COLUMN_NAME { get; set; }
        
        //public string COLUMN_TYPE { get; set; }

        public short DISPLAY_ORDER { get; set; }

        public bool DISPLAY_FLG { get; set; }
    }
}
