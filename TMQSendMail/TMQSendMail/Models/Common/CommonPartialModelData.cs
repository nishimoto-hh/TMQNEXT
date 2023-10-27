using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebTemplate.Models.Common
{
    public class CommonFormAreaData
    {
        public short FormNo { get; set; }
        public COM_CONDUCT_MST ConductMst { get; set; }
        public IList<CommonFormDefine> FormDefines { get; set; }
        public IList<CommonFormDefine> InputLists { get; set; }
        public bool IsComForm { get; set; } = false;
    }

    public class CommonTabControlGroupData
    {
        public IList<CommonFormDefine> TabCtrlDefines { get; set; }
        public int LastCtrlGrpNo { get; set; }
        public string GrpIdStr { get; set; }
    }

    public class CommonVerticalTableDivData
    {
        public COM_FORM_DEFINE FORMDEFINE { get; set; }
        public IList<COM_LISTITEM_USER> LISTITEMUSERS { get; set; }
        public IList<COM_LISTITEM_DEFINE> ListItems { get; set; }
        public int ItemCount { get; set; }
        public string Id { get; set; }
    }

    public class CommonDataCellData
    {
        public COM_FORM_DEFINE FORMDEFINE { get; set; }
        public IList<COM_LISTITEM_USER> LISTITEMUSERS { get; set; }
        public COM_LISTITEM_DEFINE ListItem { get; set; }
        public bool IsLayoutRow { get; set; }
        public int Index { get; set; }
        public bool IsHorizontal { get; set; } = false;
        //public bool IsDispNoEdit { get; set; } = false;
        //public bool IsDispNoIndex { get; set; } = false;
        public bool IsDispNoLink { get; set; } = false;
        public bool IsListReadOnly { get; set; } = false;
        public int ColFixIdx { get; set; } = -1;
    }
}
