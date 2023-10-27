using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_MS1040
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_MS1040
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class SearchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 原因性格1ID</summary>
            /// <value>予備品倉庫ID</value>
            public int? SpareStrageId { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DeleteFlg { get; set; }
            /// <summary>Gets or sets 構成階層番号</summary>
            /// <value>構成階層番号</value>
            public int StructureLayerNo { get; set; }
            /// <summary>Gets or sets 親構成ID</summary>
            /// <value>親構成ID</value>
            public int ParentStructureId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int StructureGroupId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class SearchResult : TMQUtil.CommonResultItemForMaster
        {
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string factoryName { get; set; }
            /// <summary>Gets or sets 予備品倉庫名称</summary>
            /// <value>予備品倉庫名称</value>
            public string spareStrageName { get; set; }
        }
    }
}
