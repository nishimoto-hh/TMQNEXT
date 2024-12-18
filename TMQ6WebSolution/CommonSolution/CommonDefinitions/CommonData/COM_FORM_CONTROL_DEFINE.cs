namespace CommonWebTemplate.Models.Common
{
    using System;
    //using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class COM_FORM_CONTROL_DEFINE
    {
        [Key]
        [Column(Order = 0)]
        public int LOCATION_LAYER_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string PGMID { get; set; }

        [Key]
        [Column(Order = 2)]
        public short FORMNO { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string CTRLID { get; set; }

        [Key]
        [Column(Order = 4)]
        public short DEFINETYPE { get; set; }

        [Key]
        [Column(Order = 5)]
        public short CONTROL_NO { get; set; }

        [Required]
        [StringLength(4)]
        public string CONTROL_TYPE { get; set; }

        [StringLength(100)]
        public string CONTROL_ID { get; set; }

        public short DISPKBN { get; set; }

        public short ROWNO { get; set; }

        public short COLNO { get; set; }

        public short ROWSPAN { get; set; }

        public short COLSPAN { get; set; }

        public Nullable<short> HEADER_ROWSPAN { get; set; }

        public Nullable<short> HEADER_COLSPAN { get; set; }

        public string POSITION { get; set; }

        [Required]
        [StringLength(10)]
        public string COLWIDTH { get; set; }

        public short FROMTOKBN { get; set; }

        public short ITEM_CNT { get; set; }

        public string INITVAL { get; set; }

        public short NULLCHKKBN { get; set; }

        public short TXT_AUTOCOMPKBN { get; set; }

        [Required]
        [StringLength(50)]
        public string BTN_CTRLID { get; set; }

        public short BTN_ACTIONKBN { get; set; }

        public short BTN_AUTHCONTROLKBN { get; set; }

        public short BTN_AFTEREXECKBN { get; set; }

        [StringLength(50)]
        public string BTN_MESSAGE { get; set; }

        public short DAT_TRANSITION_PATTERN { get; set; }

        public short DAT_TRANSITION_ACTION_DIVISION { get; set; }

        [Required]
        [StringLength(50)]
        public string RELATIONID { get; set; }

        [StringLength(255)]
        public string RELATIONPARAM { get; set; }

        [StringLength(50)]
        public string OPTIONINFO { get; set; }

        public short UNCHANGEABLEKBN { get; set; }

        public short COLFIXKBN { get; set; }

        public short FILTERUSEKBN { get; set; }

        public Nullable<short> SORT_DIVISION { get; set; }

        //public Nullable<bool> DETAILED_SEARCH_FLG { get; set; }
        public Nullable<short> DETAILED_SEARCH_DIVISION { get; set; }

        [StringLength(4)]
        public string DETAILED_SEARCH_CONTROL_TYPE { get; set; }

        public Nullable<bool> ITEM_CUSTOMIZE_FLG { get; set; }

        [StringLength(50)]
        public string CSSNAME { get; set; }

        [StringLength(30)]
        public string EXP_KEY_NAME { get; set; }

        [StringLength(30)]
        public string EXP_TABLE_NAME { get; set; }

        [StringLength(30)]
        public string EXP_COL_NAME { get; set; }

        [StringLength(30)]
        public string EXP_PARAM_NAME { get; set; }

        [StringLength(30)]
        public string EXP_ALIAS_NAME { get; set; }

        public short EXP_LIKE_PATTERN { get; set; }

        public short EXP_IN_CLAUSE_KBN { get; set; }

        public short EXP_LOCK_TYPE { get; set; }

        public bool DELFLG { get; set; }

        public DateTime INSERT_DATETIME { get; set; }

        [Required]
        public int INSERT_USER_ID { get; set; }

        public DateTime UPDATE_DATETIME { get; set; }

        [Required]
        public int UPDATE_USER_ID { get; set; }
    }
}
