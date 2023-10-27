using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_MS1000
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_MS1000
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class SearchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
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
            /// <summary>Gets or sets 工場ID(翻訳用)</summary>
            /// <value>工場ID(翻訳用)</value>
            public int FactoryId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class SearchResult : TMQUtil.CommonResultItemForMaster
        {
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
        }
    }
}
