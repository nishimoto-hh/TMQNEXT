using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonWebTemplate.CommonDefinitions
{
    /// <summary>
    /// 構成アイテム
    /// </summary>
    public interface IStructureItemEntity
    {
        /// <summary>Gets テーブル名</summary>
        /// <value>テーブル名</value>
        string TableName { get; }
        /// <summary>Gets or sets 構成ID</summary>
        /// <value>構成ID</value>
        int StructureId { get; set; }
        /// <summary>Gets or sets 工場ID</summary>
        /// <value>工場ID</value>
        int? FactoryId { get; set; }
        /// <summary>Gets or sets 構成グループID</summary>
        /// <value>構成グループID</value>
        int? StructureGroupId { get; set; }
        /// <summary>Gets or sets 親構成ID</summary>
        /// <value>親構成ID</value>
        int? ParentStructureId { get; set; }
        /// <summary>Gets or sets 構成階層番号</summary>
        /// <value>構成階層番号</value>
        int? StructureLayerNo { get; set; }
        /// <summary>Gets or sets 表示順</summary>
        /// <value>表示順</value>
        int? DisplayOrder { get; set; }
        /// <summary>Gets or sets 翻訳文字列</summary>
        /// <value>翻訳文字列</value>
        string TranslationText { get; set; }
    }
}
