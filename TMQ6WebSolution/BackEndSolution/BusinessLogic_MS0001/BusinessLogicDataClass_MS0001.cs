using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_MS0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_MS0001
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public string UserId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets構成グループID</summary>
            /// <value>構成グループID</value>
            public string ConductId { get; set; }
            /// <summary>Gets or setsマスタグループ</summary>
            /// <value>マスタグループ</value>
            public string GroupTranslationText { get; set; }
            /// <summary>Gets or setsマスタ</summary>
            /// <value>マスタ</value>
            public string ConductTranslationText { get; set; }
        }

        /// <summary>
        /// CSV出力データ取得条件
        /// </summary>
        public class searchConditionReport
        {
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 権限のある工場ID</summary>
            /// <value>権限のある工場ID</value>
            public List<int> AuthFactoryId { get; set; }
            /// <summary>Gets or sets 工場IDリスト</summary>
            /// <value>工場IDリスト</value>
            public List<int> FactoryIdList { get; set; }
            /// <summary>Gets or sets 共通工場ID</summary>
            /// <value>共通工場ID</value>
            public int CommonFactoryId { get; set; }
            /// <summary>Gets or sets ユーザー本務工場ID</summary>
            /// <value>ユーザー本務工場ID</value>
            public int UserFactoryId { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
            /// <summary>Gets or sets構成グループID</summary>
            /// <value>構成グループID</value>
            public List<int> StructureGroupId { get; set; }
        }

        /// <summary>
        /// CSV出力データ(ヘッダー)
        /// </summary>
        public class searchResultHeaderReport
        {
            /// <summary>Gets or sets 翻訳ID</summary>
            /// <value>翻訳ID</value>
            public long TranslationId { get; set; }
            /// <summary>Gets or sets 翻訳名称</summary>
            /// <value>翻訳名称</value>
            public string HeaderName { get; set; }
        }
        /// <summary>
        /// CSV出力データ
        /// </summary>
        public class searchResultReport
        {
            /// <summary>Gets or sets マスタ種類</summary>
            /// <value>マスタ種類</value>
            public string MasterName { get; set; }
            /// <summary>Gets or sets 工場</summary>
            /// <value>工場</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets 標準</summary>
            /// <value>標準</value>
            public string DefaultItem { get; set; }
            /// <summary>Gets or sets アイテムID</summary>
            /// <value>アイテムID</value>
            public string StructureItemId { get; set; }
            /// <summary>Gets or sets アイテム翻訳</summary>
            /// <value>アイテム翻訳</value>
            public string TranslationText { get; set; }
            /// <summary>Gets or sets 親階層アイテムID</summary>
            /// <value>親階層アイテムID</value>
            public string ParentStructureItemId { get; set; }
            /// <summary>Gets or sets 親階層アイテム翻訳</summary>
            /// <value>親階層アイテム翻訳</value>
            public string ParentTranslationText { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public string DisplayOrder { get; set; }
            /// <summary>Gets or sets 未使用</summary>
            /// <value>未使用</value>
            public string Unused { get; set; }
            /// <summary>Gets or sets 削除</summary>
            /// <value>削除</value>
            public string DeleteFlg { get; set; }
            /// <summary>Gets or sets 拡張項目1</summary>
            /// <value>拡張項目1</value>
            public string ExtensionData1 { get; set; }
            /// <summary>Gets or sets 拡張項目2</summary>
            /// <value>拡張項目2</value>
            public string ExtensionData2 { get; set; }
            /// <summary>Gets or sets 拡張項目3</summary>
            /// <value>拡張項目3</value>
            public string ExtensionData3 { get; set; }
            /// <summary>Gets or sets 拡張項目4</summary>
            /// <value>拡張項目4</value>
            public string ExtensionData4 { get; set; }
            /// <summary>Gets or sets 拡張項目5</summary>
            /// <value>拡張項目5</value>
            public string ExtensionData5 { get; set; }
            /// <summary>Gets or sets 拡張項目6</summary>
            /// <value>拡張項目6</value>
            public string ExtensionData6 { get; set; }
            /// <summary>Gets or sets 拡張項目7</summary>
            /// <value>拡張項目7</value>
            public string ExtensionData7 { get; set; }
            /// <summary>Gets or sets 拡張項目8</summary>
            /// <value>拡張項目8</value>
            public string ExtensionData8 { get; set; }
            /// <summary>Gets or sets 拡張項目9</summary>
            /// <value>拡張項目9</value>
            public string ExtensionData9 { get; set; }
            /// <summary>Gets or sets 拡張項目10</summary>
            /// <value>拡張項目10</value>
            public string ExtensionData10 { get; set; }
        }
    }
}
