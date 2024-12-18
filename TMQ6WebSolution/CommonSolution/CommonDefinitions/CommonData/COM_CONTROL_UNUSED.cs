namespace CommonWebTemplate.Models.Common
{
    using System;
    //using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class COM_CONTROL_UNUSED
    {
        [Key]
        [Column(Order = 0)]
        public int LOCATION_LAYER_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string CONTROL_ID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string CONTROL_TYPE { get; set; }

        public bool DELFLG { get; set; }

        public DateTime INSERT_DATETIME { get; set; }

        [Required]
        public int INSERT_USER_ID { get; set; }

        public DateTime UPDATE_DATETIME { get; set; }

        [Required]
        public int UPDATE_USER_ID { get; set; }
    }
}
