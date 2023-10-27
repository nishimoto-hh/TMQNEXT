namespace CommonWebTemplate.Models.Common
{
    using System;
    //using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class COM_FORM_DEFINE
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string PGMID { get; set; }

        [Key]
        [Column(Order = 1)]
        public short FORMNO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string CTRLID { get; set; }

        [Required]
        [StringLength(50)]
        public string CTRLNAME { get; set; }

        public short CTRLTYPE { get; set; }

        public short AREAKBN { get; set; }

        public short DISPORDER { get; set; }

        [StringLength(30)]
        public string DAT_FORMTITLE { get; set; }
        
        public short CTRLGRPNO { get; set; }

        [StringLength(255)]
        public string CTRLGRPNAME { get; set; }

        public short TABNO { get; set; }

        [StringLength(255)]
        public string TABNAME { get; set; }

        public short DAT_TRANSPTN { get; set; }

        public short DAT_TRANSDISPPTN { get; set; }

        [StringLength(50)]
        public string DAT_TRANSTARGET { get; set; }

        [StringLength(255)]
        public string DAT_TRANSPARAM { get; set; }

        public short DAT_EDITPTN { get; set; }

        public short DAT_DIRECTION { get; set; }

        public short DAT_HEADERDISPKBN { get; set; }

        public Nullable<int> DAT_PAGEROWS { get; set; }

        public Nullable<int> DAT_MAXROWS { get; set; }

        [StringLength(255)]
        public string DAT_TITLE { get; set; }

        public short DAT_SWITCHKBN { get; set; }

        public short DAT_ROWADDKBN { get; set; }

        public short DAT_ROWDELKBN { get; set; }

        public short DAT_ROWSELKBN { get; set; }

        public short DAT_COLSELKBN { get; set; }

        public short DAT_ROWSORTKBN { get; set; }

        public short DAT_TRANSICONKBN { get; set; }

        public short DAT_HEIGHT { get; set; }

        [Required]
        [StringLength(50)]
        public string CTR_RELATIONCTRLID { get; set; }

        public short CTR_POSITIONKBN { get; set; }

        [StringLength(50)]
        public string CSSNAME { get; set; }

        [StringLength(255)]
        public string TOOLTIP { get; set; }

        public Nullable<bool> REFERENCE_MODE { get; set; }

        public Nullable<short> COMM_FORMNO { get; set; }

        public bool DELFLG { get; set; }

        public DateTime INSERT_DATETIME { get; set; }

        [Required]
        public int INSERT_USER_ID { get; set; }

        public DateTime UPDATE_DATETIME { get; set; }

        [Required]
        public int UPDATE_USER_ID { get; set; }
    }
}
