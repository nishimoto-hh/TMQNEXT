namespace CommonWebTemplate.Models.Common
{
    using System;
    //using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class COM_CONTROL_DEFINE
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public string CONTROL_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string CONTROL_TYPE { get; set; }

        [StringLength(100)]
        public string MINVAL { get; set; }

        [StringLength(100)]
        public string MAXVAL { get; set; }

        public Nullable<int> FORMAT_TRANSLATION_ID { get; set; }

        public Nullable<short> MAXLENGTH { get; set; }

        public Nullable<int> TEXT_PLACEHOLDER_TRANSLATION_ID { get; set; }

        public Nullable<int> TOOLTIP_TRANSLATION_ID { get; set; }

        public bool DELFLG { get; set; }

        public DateTime INSERT_DATETIME { get; set; }

        [Required]
        public int INSERT_USER_ID { get; set; }

        public DateTime UPDATE_DATETIME { get; set; }

        [Required]
        public int UPDATE_USER_ID { get; set; }
    }
}
