namespace CommonWebTemplate.Models.Common
{
    using System;
    //using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class COM_LISTITEM_USER_CUSTOMIZE
    {
        public int USERID { get; set; }

        public string PGMID { get; set; }
        
        public short FORMNO { get; set; }
        
        public string CTRLID { get; set; }
               
        public short ITEMNO { get; set; }

        public short DATA_DIVISION { get; set; }

        public short DISPLAY_ORDER { get; set; }

        public bool DISPLAY_FLG { get; set; }

        public bool DELETE_FLG { get; set; }
    }
}
