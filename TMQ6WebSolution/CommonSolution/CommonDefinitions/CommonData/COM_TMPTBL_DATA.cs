namespace CommonWebTemplate.Models.Common
{
    //using System;
    //using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class COM_TMPTBL_DATA
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string TERMINALNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string USERID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string CONDUCTID { get; set; }

        [Key]
        [Column(Order = 3)]
        public short FORMNO { get; set; }

        [Key]
        [Column(Order = 4)]
        public short REPORT_NO { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string CTRLID { get; set; }

        [Key]
        [Column(Order = 6)]
        public short DATATYPE { get; set; }

        [Key]
        [Column(Order = 7)]
        public long ROWNO { get; set; }

        public byte ROWSTATUS { get; set; }

        [StringLength(20)]
        public string UPDTAG { get; set; }

        [StringLength(255)]
        public string VAL1 { get; set; }

        [StringLength(255)]
        public string VAL2 { get; set; }

        [StringLength(255)]
        public string VAL3 { get; set; }

        [StringLength(255)]
        public string VAL4 { get; set; }

        [StringLength(255)]
        public string VAL5 { get; set; }

        [StringLength(255)]
        public string VAL6 { get; set; }

        [StringLength(255)]
        public string VAL7 { get; set; }

        [StringLength(255)]
        public string VAL8 { get; set; }

        [StringLength(255)]
        public string VAL9 { get; set; }

        [StringLength(255)]
        public string VAL10 { get; set; }

        [StringLength(255)]
        public string VAL11 { get; set; }

        [StringLength(255)]
        public string VAL12 { get; set; }

        [StringLength(255)]
        public string VAL13 { get; set; }

        [StringLength(255)]
        public string VAL14 { get; set; }

        [StringLength(255)]
        public string VAL15 { get; set; }

        [StringLength(255)]
        public string VAL16 { get; set; }

        [StringLength(255)]
        public string VAL17 { get; set; }

        [StringLength(255)]
        public string VAL18 { get; set; }

        [StringLength(255)]
        public string VAL19 { get; set; }

        [StringLength(255)]
        public string VAL20 { get; set; }

        [StringLength(255)]
        public string VAL21 { get; set; }

        [StringLength(255)]
        public string VAL22 { get; set; }

        [StringLength(255)]
        public string VAL23 { get; set; }

        [StringLength(255)]
        public string VAL24 { get; set; }

        [StringLength(255)]
        public string VAL25 { get; set; }

        [StringLength(255)]
        public string VAL26 { get; set; }

        [StringLength(255)]
        public string VAL27 { get; set; }

        [StringLength(255)]
        public string VAL28 { get; set; }

        [StringLength(255)]
        public string VAL29 { get; set; }

        [StringLength(400)]
        public string VAL30 { get; set; }

        [StringLength(400)]
        public string VAL31 { get; set; }

        [StringLength(400)]
        public string VAL32 { get; set; }

        [StringLength(400)]
        public string VAL33 { get; set; }

        [StringLength(400)]
        public string VAL34 { get; set; }

        [StringLength(400)]
        public string VAL35 { get; set; }

        [StringLength(400)]
        public string VAL36 { get; set; }

        [StringLength(400)]
        public string VAL37 { get; set; }

        [StringLength(400)]
        public string VAL38 { get; set; }

        [StringLength(400)]
        public string VAL39 { get; set; }

        [StringLength(400)]
        public string VAL40 { get; set; }

        [StringLength(400)]
        public string VAL41 { get; set; }

        [StringLength(400)]
        public string VAL42 { get; set; }

        [StringLength(400)]
        public string VAL43 { get; set; }

        [StringLength(400)]
        public string VAL44 { get; set; }

        [StringLength(400)]
        public string VAL45 { get; set; }

        [StringLength(400)]
        public string VAL46 { get; set; }

        [StringLength(400)]
        public string VAL47 { get; set; }

        [StringLength(400)]
        public string VAL48 { get; set; }

        [StringLength(400)]
        public string VAL49 { get; set; }

        [StringLength(400)]
        public string VAL50 { get; set; }

        [StringLength(400)]
        public string VAL51 { get; set; }

        [StringLength(400)]
        public string VAL52 { get; set; }

        [StringLength(400)]
        public string VAL53 { get; set; }

        [StringLength(400)]
        public string VAL54 { get; set; }

        [StringLength(400)]
        public string VAL55 { get; set; }

        [StringLength(400)]
        public string VAL56 { get; set; }

        [StringLength(400)]
        public string VAL57 { get; set; }

        [StringLength(400)]
        public string VAL58 { get; set; }

        [StringLength(400)]
        public string VAL59 { get; set; }

        [StringLength(400)]
        public string VAL60 { get; set; }

        [StringLength(400)]
        public string VAL61 { get; set; }

        [StringLength(400)]
        public string VAL62 { get; set; }

        [StringLength(400)]
        public string VAL63 { get; set; }

        [StringLength(400)]
        public string VAL64 { get; set; }

        [StringLength(400)]
        public string VAL65 { get; set; }

        [StringLength(400)]
        public string VAL66 { get; set; }

        [StringLength(400)]
        public string VAL67 { get; set; }

        [StringLength(400)]
        public string VAL68 { get; set; }

        [StringLength(400)]
        public string VAL69 { get; set; }

        [StringLength(400)]
        public string VAL70 { get; set; }

        [StringLength(400)]
        public string VAL71 { get; set; }

        [StringLength(400)]
        public string VAL72 { get; set; }

        [StringLength(400)]
        public string VAL73 { get; set; }

        [StringLength(400)]
        public string VAL74 { get; set; }

        [StringLength(400)]
        public string VAL75 { get; set; }

        [StringLength(400)]
        public string VAL76 { get; set; }

        [StringLength(400)]
        public string VAL77 { get; set; }

        [StringLength(400)]
        public string VAL78 { get; set; }

        [StringLength(400)]
        public string VAL79 { get; set; }

        [StringLength(400)]
        public string VAL80 { get; set; }

        [StringLength(400)]
        public string VAL81 { get; set; }

        [StringLength(400)]
        public string VAL82 { get; set; }

        [StringLength(400)]
        public string VAL83 { get; set; }

        [StringLength(400)]
        public string VAL84 { get; set; }

        [StringLength(400)]
        public string VAL85 { get; set; }

        [StringLength(400)]
        public string VAL86 { get; set; }

        [StringLength(400)]
        public string VAL87 { get; set; }

        [StringLength(400)]
        public string VAL88 { get; set; }

        [StringLength(400)]
        public string VAL89 { get; set; }

        [StringLength(400)]
        public string VAL90 { get; set; }

        [StringLength(400)]
        public string VAL91 { get; set; }

        [StringLength(400)]
        public string VAL92 { get; set; }
        
        [StringLength(400)]
        public string VAL93 { get; set; }

        [StringLength(400)]
        public string VAL94 { get; set; }

        [StringLength(400)]
        public string VAL95 { get; set; }

        [StringLength(400)]
        public string VAL96 { get; set; }

        [StringLength(400)]
        public string VAL97 { get; set; }

        [StringLength(400)]
        public string VAL98 { get; set; }

        [StringLength(400)]
        public string VAL99 { get; set; }

        [StringLength(400)]
        public string VAL100 { get; set; }

        [StringLength(400)]
        public string VAL101 { get; set; }

        [StringLength(400)]
        public string VAL102 { get; set; }

        [StringLength(400)]
        public string VAL103 { get; set; }

        [StringLength(400)]
        public string VAL104 { get; set; }

        [StringLength(400)]
        public string VAL105 { get; set; }

        [StringLength(400)]
        public string VAL106 { get; set; }

        [StringLength(400)]
        public string VAL107 { get; set; }

        [StringLength(400)]
        public string VAL108 { get; set; }

        [StringLength(400)]
        public string VAL109 { get; set; }

        [StringLength(400)]
        public string VAL110 { get; set; }

        [StringLength(400)]
        public string VAL111 { get; set; }

        [StringLength(400)]
        public string VAL112 { get; set; }

        [StringLength(400)]
        public string VAL113 { get; set; }

        [StringLength(400)]
        public string VAL114 { get; set; }

        [StringLength(400)]
        public string VAL115 { get; set; }

        [StringLength(400)]
        public string VAL116 { get; set; }

        [StringLength(400)]
        public string VAL117 { get; set; }

        [StringLength(400)]
        public string VAL118 { get; set; }

        [StringLength(400)]
        public string VAL119 { get; set; }

        [StringLength(400)]
        public string VAL120 { get; set; }

        [StringLength(400)]
        public string VAL121 { get; set; }

        [StringLength(400)]
        public string VAL122 { get; set; }

        [StringLength(400)]
        public string VAL123 { get; set; }

        [StringLength(400)]
        public string VAL124 { get; set; }

        [StringLength(400)]
        public string VAL125 { get; set; }

        [StringLength(400)]
        public string VAL126 { get; set; }

        [StringLength(400)]
        public string VAL127 { get; set; }

        [StringLength(400)]
        public string VAL128 { get; set; }

        [StringLength(400)]
        public string VAL129 { get; set; }

        [StringLength(400)]
        public string VAL130 { get; set; }

        [StringLength(400)]
        public string VAL131 { get; set; }

        [StringLength(400)]
        public string VAL132 { get; set; }

        [StringLength(400)]
        public string VAL133 { get; set; }

        [StringLength(400)]
        public string VAL134 { get; set; }

        [StringLength(400)]
        public string VAL135 { get; set; }

        [StringLength(400)]
        public string VAL136 { get; set; }

        [StringLength(400)]
        public string VAL137 { get; set; }

        [StringLength(400)]
        public string VAL138 { get; set; }

        [StringLength(400)]
        public string VAL139 { get; set; }

        [StringLength(400)]
        public string VAL140 { get; set; }

        [StringLength(400)]
        public string VAL141 { get; set; }

        [StringLength(400)]
        public string VAL142 { get; set; }

        [StringLength(400)]
        public string VAL143 { get; set; }

        [StringLength(400)]
        public string VAL144 { get; set; }

        [StringLength(400)]
        public string VAL145 { get; set; }

        [StringLength(400)]
        public string VAL146 { get; set; }

        [StringLength(400)]
        public string VAL147 { get; set; }

        [StringLength(400)]
        public string VAL148 { get; set; }

        [StringLength(400)]
        public string VAL149 { get; set; }

        [StringLength(400)]
        public string VAL150 { get; set; }

        [StringLength(400)]
        public string VAL151 { get; set; }

        [StringLength(400)]
        public string VAL152 { get; set; }

        [StringLength(400)]
        public string VAL153 { get; set; }

        [StringLength(400)]
        public string VAL154 { get; set; }

        [StringLength(400)]
        public string VAL155 { get; set; }

        [StringLength(400)]
        public string VAL156 { get; set; }

        [StringLength(400)]
        public string VAL157 { get; set; }

        [StringLength(400)]
        public string VAL158 { get; set; }

        [StringLength(400)]
        public string VAL159 { get; set; }

        [StringLength(400)]
        public string VAL160 { get; set; }

        [StringLength(400)]
        public string VAL161 { get; set; }

        [StringLength(400)]
        public string VAL162 { get; set; }

        [StringLength(400)]
        public string VAL163 { get; set; }

        [StringLength(400)]
        public string VAL164 { get; set; }

        [StringLength(400)]
        public string VAL165 { get; set; }

        [StringLength(400)]
        public string VAL166 { get; set; }

        [StringLength(400)]
        public string VAL167 { get; set; }

        [StringLength(400)]
        public string VAL168 { get; set; }

        [StringLength(400)]
        public string VAL169 { get; set; }

        [StringLength(400)]
        public string VAL170 { get; set; }

        [StringLength(400)]
        public string VAL171 { get; set; }

        [StringLength(400)]
        public string VAL172 { get; set; }

        [StringLength(400)]
        public string VAL173 { get; set; }

        [StringLength(400)]
        public string VAL174 { get; set; }

        [StringLength(400)]
        public string VAL175 { get; set; }

        [StringLength(400)]
        public string VAL176 { get; set; }

        [StringLength(400)]
        public string VAL177 { get; set; }

        [StringLength(400)]
        public string VAL178 { get; set; }

        [StringLength(400)]
        public string VAL179 { get; set; }

        [StringLength(400)]
        public string VAL180 { get; set; }

        [StringLength(400)]
        public string VAL181 { get; set; }

        [StringLength(400)]
        public string VAL182 { get; set; }

        [StringLength(400)]
        public string VAL183 { get; set; }

        [StringLength(400)]
        public string VAL184 { get; set; }

        [StringLength(400)]
        public string VAL185 { get; set; }

        [StringLength(400)]
        public string VAL186 { get; set; }

        [StringLength(400)]
        public string VAL187 { get; set; }

        [StringLength(400)]
        public string VAL188 { get; set; }

        [StringLength(400)]
        public string VAL189 { get; set; }

        [StringLength(400)]
        public string VAL190 { get; set; }

        [StringLength(400)]
        public string VAL191 { get; set; }

        [StringLength(400)]
        public string VAL192 { get; set; }

        [StringLength(400)]
        public string VAL193 { get; set; }

        [StringLength(400)]
        public string VAL194 { get; set; }

        [StringLength(400)]
        public string VAL195 { get; set; }

        [StringLength(400)]
        public string VAL196 { get; set; }

        [StringLength(400)]
        public string VAL197 { get; set; }

        [StringLength(400)]
        public string VAL198 { get; set; }

        [StringLength(400)]
        public string VAL199 { get; set; }

        [StringLength(400)]
        public string VAL200 { get; set; }

        public int INPYMD { get; set; }

        public int INPHMIS { get; set; }

        public int UPDYMD { get; set; }

        public int UPDHMIS { get; set; }
    }
}
