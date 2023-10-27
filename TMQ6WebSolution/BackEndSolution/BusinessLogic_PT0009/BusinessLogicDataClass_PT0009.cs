using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_PT0009
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_PT0009
    {
        /// <summary>
        /// 出力条件リストのデータクラス
        /// </summary>
        public class conditionList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 対象年月</summary>
            /// <value>対象年月</value>
            public string TargetYearMonth { get; set; }
            /// <summary>Gets or sets 工場</summary>
            /// <value>工場</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 職種</summary>
            /// <value>職種</value>
            public int? JobId { get; set; }
            /// <summary>Gets or sets 予備品No</summary>
            /// <value>予備品No</value>
            public string PartsNo { get; set; }
            /// <summary>Gets or sets 予備品名</summary>
            /// <value>予備品名</value>
            public string PartsName { get; set; }
            /// <summary>Gets or sets 型式</summary>
            /// <value>型式</value>
            public string ModelType { get; set; }
            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string StandardSize { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public int? ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets 使用場所</summary>
            /// <value>使用場所</value>
            public string PartsServiceSpace { get; set; }
            /// <summary>Gets or sets 仕入先</summary>
            /// <value>仕入先</value>
            public int? VenderStructureId { get; set; }
            /// <summary>Gets or sets 予備品倉庫</summary>
            /// <value>予備品倉庫</value>
            public int? StorageLocationId { get; set; }
            /// <summary>Gets or sets 棚番</summary>
            /// <value>棚番</value>
            public int? PartsLocationId { get; set; }
            /// <summary>Gets or sets 入出庫日From</summary>
            /// <value>入出庫日From</value>
            public DateTime? WorkDivisionDateFrom { get; set; }
            /// <summary>Gets or sets 入出庫日To</summary>
            /// <value>入出庫日To</value>
            public DateTime? WorkDivisionDateTo { get; set; }
            /// <summary>Gets or sets 入出庫No</summary>
            /// <value>入出庫No</value>
            public int? WorkDivisionNo { get; set; }
            /// <summary>Gets or sets 新旧区分</summary>
            /// <value>新旧区分</value>
            public int? OldNewStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目</summary>
            /// <value>勘定科目</value>
            public int? AccountStructureId { get; set; }
            /// <summary>Gets or sets 部門</summary>
            /// <value>部門</value>
            public List<int> DepartmentIdList { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 管理No</summary>
            /// <value>管理No</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 帳票ID</summary>
            /// <value>帳票ID</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets 帳票名</summary>
            /// <value>帳票名</value>
            public string ReportName { get; set; }
        }
    }
}
