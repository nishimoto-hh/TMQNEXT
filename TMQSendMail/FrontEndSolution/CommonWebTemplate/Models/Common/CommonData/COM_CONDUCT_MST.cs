namespace CommonWebTemplate.Models.Common
{
    using System;
    //using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    //using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class COM_CONDUCT_MST
    {
        [Key]
        [StringLength(10)]
        public string CONDUCTID { get; set; }

        [Required]
        [StringLength(10)]
        public string CONDUCTGRP { get; set; }

        [Required]
        [StringLength(255)]
        public string NAME { get; set; }

        [Required]
        [StringLength(255)]
        public string RYAKU { get; set; }

        public short PTN { get; set; }

        public short MENUORDER { get; set; }

        public short MENUDISP { get; set; }

        [StringLength(50)]
        public string PGMID { get; set; }

        [StringLength(20)]
        public string BOOTPARAM { get; set; }

        [StringLength(255)]
        public string CM_CONDUCTID { get; set; }

        [StringLength(20)]
        public string VERSION { get; set; }

        [StringLength(255)]
        public string CHANGELOG { get; set; }

        public bool DELFLG { get; set; }

        public DateTime INSERT_DATETIME { get; set; }

        [Required]
        public int INSERT_USER_ID { get; set; }

        public DateTime UPDATE_DATETIME { get; set; }

        [Required]
        public int UPDATE_USER_ID { get; set; }
    }
}
