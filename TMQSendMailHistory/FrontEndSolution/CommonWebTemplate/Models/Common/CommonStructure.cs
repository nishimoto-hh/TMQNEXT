///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　構成クラス
/// 説明　　　：　構成の情報を定義します。
///</summary>
using CommonWebTemplate.CommonDefinitions;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CommonWebTemplate.Models.Common
{
    /// <summary>
    /// 構成アイテム
    /// </summary>
    public class CommonStructure : IStructureItemEntity
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonStructure()
        {
            TableName = "v_structure_item";
        }

        /// <summary>Gets テーブル名</summary>
        /// <value>テーブル名</value>
        public string TableName { get; }
        /// <summary>Gets or sets 構成ID</summary>
        /// <value>構成ID</value>
        public int StructureId { get; set; }
        /// <summary>Gets or sets 工場ID</summary>
        /// <value>工場ID</value>
        public int? FactoryId { get; set; }
        /// <summary>Gets or sets 構成グループID</summary>
        /// <value>構成グループID</value>
        public int? StructureGroupId { get; set; }
        /// <summary>Gets or sets 親構成ID</summary>
        /// <value>親構成ID</value>
        public int? ParentStructureId { get; set; }
        /// <summary>Gets or sets 構成階層番号</summary>
        /// <value>構成階層番号</value>
        public int? StructureLayerNo { get; set; }
        /// <summary>Gets or sets 表示順</summary>
        /// <value>表示順</value>
        public int? DisplayOrder { get; set; }
        /// <summary>Gets or sets 翻訳文字列</summary>
        /// <value>翻訳文字列</value>
        public string TranslationText { get; set; }
    }

    /// <summary>
    /// ツリービュー情報
    /// </summary>
    public class CommonTreeViewInfo
    {
        /// <summary>ノードID</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>親ノードID</summary>
        [JsonPropertyName("parent")]
        public string Parent { get; set; }

        /// <summary>ノード文字列</summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        ///// <summary>ノードアイコン</summary>
        //[JsonPropertyName("icon")]
        //public bool Icon { get; set; }

        /// <summary>ノード状態</summary>
        [JsonPropertyName("state")]
        public CommonTreeViewState State { get; set; }

        /// <summary>liタグ属性</summary>
        [JsonPropertyName("li_attr")]
        public CommonTreeViewAttribute LiAttribute { get; set; }

        #region === コンストラクタ ===
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonTreeViewInfo()
        {
            this.Id = "0";
            this.Parent = "#";
            this.Text = string.Empty;
            //this.Icon = false;
            this.State = new CommonTreeViewState();
            this.LiAttribute = new CommonTreeViewAttribute();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="structureInfo">構成マスタ情報</param>
        /// <param name="prefix">接頭語</param>
        public CommonTreeViewInfo(CommonStructure structureInfo)
        {
            this.Id = structureInfo.StructureId.ToString();
            this.Parent = structureInfo.ParentStructureId.ToString();
            this.Text = structureInfo.TranslationText;
            //this.Icon = false;
            this.State = new CommonTreeViewState();
            this.LiAttribute = new CommonTreeViewAttribute(structureInfo);
        }
        #endregion
    }

    /// <summary>
    /// ツリービューliタグ属性
    /// </summary>
    public class CommonTreeViewAttribute
    {
        /// <summary>
        /// 階層番号
        /// </summary>
        [JsonPropertyName("data-structureno")]
        public int? StructureNo { get; set; }
        /// <summary>
        /// 工場ID
        /// </summary>
        [JsonPropertyName("data-factoryid")]
        public int? FactoryId { get; set; }
        /// <summary>
        /// 構成ID
        /// </summary>
        [JsonPropertyName("data-structureid")]
        public int? StructureId { get; set; }

        #region === コンストラクタ ===
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonTreeViewAttribute()
        {
            this.StructureNo = null;
            this.FactoryId = 0;
            this.StructureId = null;
        }
        public CommonTreeViewAttribute(CommonStructure structureInfo)
        {
            this.StructureNo = structureInfo.StructureLayerNo;
            this.FactoryId = structureInfo.FactoryId;
            this.StructureId = structureInfo.StructureId;
        }
        #endregion

    }

    /// <summary>
    /// ツリービューノード状態
    /// </summary>
    public class CommonTreeViewState
    {
        /// <summary>ツリー開閉状態(true:開く/false:閉じる)</summary>
        [JsonPropertyName("opened")]
        public bool Opened { get; set; }

        /// <summary>ツリー有効状態(true:無効/false:有効)</summary>
        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; }

        /// <summary>ツリー選択状態(true:選択/false:未選択)</summary>
        [JsonPropertyName("selected")]
        public bool Selected { get; set; }

        #region === コンストラクタ ===
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonTreeViewState()
        {
            this.Opened = false;
            this.Disabled = false;
            this.Selected = false;
        }
        #endregion
    }

    /// <summary>
    /// 言語情報クラス
    /// </summary>
    public class LanguageInfo
    {
        /// <summary>Gets or sets 言語コード</summary>
        /// <value>言語コード</value>
        public string LanguageCode { get; set; }
        /// <summary>Gets or sets 言語ラベル</summary>
        /// <value>言語ラベル</value>
        public string LanguageLabel { get; set; }
    }

    /// <summary>
    /// テンポラリフォルダパス取得クラス
    /// </summary>
    public class TempFolderPathInfo
    {
        /// <summary>Gets or sets テンポラリフォルダパス</summary>
        /// <value>テンポラリフォルダパス</value>
        public string TempFolderPath { get; set; }
    }
}