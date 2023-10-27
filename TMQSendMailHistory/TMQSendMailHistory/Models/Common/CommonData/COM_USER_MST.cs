namespace CommonWebTemplate.Models.Common
{
    //using System;
    //using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    //using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class COM_USER_MST
    {
        [Key]
        [StringLength(10)]
        public string USERID { get; set; }

        [Required]
        [StringLength(20)]
        public string USERNAME { get; set; }

        [Required]
        [StringLength(40)]
        public string KANA { get; set; }

        [Required]
        [StringLength(128)]
        public string PASSWORD { get; set; }

        public bool DELFLG { get; set; }

        public int INPYMD { get; set; }

        public int INPHMIS { get; set; }

        [Required]
        [StringLength(20)]
        public string INPID { get; set; }

        [Required]
        [StringLength(255)]
        public string INPTERMINAL { get; set; }

        [Required]
        [StringLength(10)]
        public string INPCONDUCTID { get; set; }

        public int UPDYMD { get; set; }

        public int UPDHMIS { get; set; }

        [StringLength(20)]
        public string UPDID { get; set; }

        [StringLength(255)]
        public string UPDTERMINAL { get; set; }

        [Required]
        [StringLength(10)]
        public string UPDCONDUCTID { get; set; }
    }
}
