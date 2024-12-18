namespace CommonWebTemplate.Models.Common
{
    //using System;
    //using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class COM_LISTITEM_USER
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string USERID { get; set; }
        
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
        public short ITEMNO { get; set; }

        public int UPDYMD { get; set; }

        public int UPDHMIS { get; set; }

        [StringLength(255)]
        public string UPDID { get; set; }
    }
}
