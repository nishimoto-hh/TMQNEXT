using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_MS0020
{
    /// <summary>
    /// 機種別仕様マスタで使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_MS0020
    {
        /// <summary>機能共通</summary>
        public class Common
        {
            /// <summary>言語の翻訳の一覧</summary>
            public class Translation
            {
                #region 言語の一覧(常に取得可能)
                /// <summary>Gets or sets 言語ID</summary>
                /// <value>言語ID</value>
                public string LanguageId { get; set; }
                /// <summary>Gets or sets 言語名称</summary>
                /// <value>言語名称</value>
                public string LanguageName { get; set; }

                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                /// <remarks>前の画面より引き継ぎ</remarks>
                public int LocationStructureId { get; set; }
                #endregion

                #region 翻訳マスタの値(新規の場合はNULL)
                /// <summary>Gets or sets 翻訳ID</summary>
                /// <value>翻訳ID</value>
                public int? TranslationId { get; set; }
                /// <summary>Gets or sets 更新シリアルID</summary>
                /// <value>更新シリアルID</value>
                public int? UpdateSerialid { get; set; }

                /// <summary>Gets or sets 翻訳文字列</summary>
                /// <value>翻訳文字列</value>
                public string TranslationText { get; set; }
                /// <summary>Gets or sets 翻訳文字列(変更前)</summary>
                /// <value>翻訳文字列(変更前)</value>
                public string TranslationTextBk { get; set; }
                /// <summary>Gets or sets 翻訳項目説明</summary>
                /// <value>翻訳項目説明</value>
                public string TranslationItemDescription { get; set; }
                #endregion

                #region アイテム翻訳のデータクラス（マスタメンテナンス用）を使用するための項目
                // 入力チェックで同一アイテムの翻訳を検索するのに使用

                /// <summary>Gets or sets 構成グループID</summary>
                /// <value>構成グループID</value>
                public int StructureGroupId { get; set; }
                /// <summary>Gets or sets 構成階層番号</summary>
                /// <value>構成階層番号</value>
                /// <remarks>Null</remarks>
                public int? StructureLayerNo { get; set; }
                /// <summary>Gets or sets 親構成ID</summary>
                /// <value>親構成ID</value>
                /// <remarks>Null</remarks>
                public int? ParentStructureId { get; set; }

                /// <summary>Gets or sets アイテムマスタ拡張の連番</summary>
                /// <value>アイテムマスタ拡張の連番</value>
                /// <remarks>選択肢の場合、仕様項目IDを紐づけるため、1</remarks>
                public int? SequenceNo { get; set; }
                /// <summary>Gets or sets 拡張データ</summary>
                /// <value>拡張データ</value>
                /// <remarks>選択肢の場合、仕様項目ID</remarks>
                public string ExtensionData { get; set; }
                #endregion
            }
            /// <summary>職種</summary>
            /// <remarks>共通処理の関係で、各階層の職種の定義が必要</remarks>
            public class Job
            {
                /// <summary>Gets or sets 職種機種階層ID</summary>
                /// <value>職種機種階層ID</value>
                public int? JobStructureId { get; set; }

                #region 共通　職種設定用
                /// <summary>Gets or sets 職種ID</summary>
                /// <value>職種ID</value>
                public int JobId { get; set; }
                /// <summary>Gets or sets 職種名称</summary>
                /// <value>職種名称</value>
                public string JobName { get; set; }
                /// <summary>Gets or sets 機種大分類ID</summary>
                /// <value>機種大分類ID</value>
                public int? LargeClassficationId { get; set; }
                /// <summary>Gets or sets 機種大分類名称</summary>
                /// <value>機種大分類名称</value>
                public string LargeClassficationName { get; set; }
                /// <summary>Gets or sets 機種中分類ID</summary>
                /// <value>機種中分類ID</value>
                public int? MiddleClassficationId { get; set; }
                /// <summary>Gets or sets 機種中分類名称</summary>
                /// <value>機種中分類名称</value>
                public string MiddleClassficationName { get; set; }
                /// <summary>Gets or sets 機種小分類ID</summary>
                /// <value>機種小分類ID</value>
                public int? SmallClassficationId { get; set; }
                /// <summary>Gets or sets 機種小分類名称</summary>
                /// <value>機種小分類名称</value>
                public string SmallClassficationName { get; set; }
                #endregion
            }
            /// <summary>
            /// SQL検索用パラメータ
            /// </summary>
            public class Param
            {

                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int FactoryId { get; set; }

                /// <summary>Gets or sets 機種別仕様関連付ID</summary>
                /// <value>機種別仕様関連付ID</value>
                public int? MachineSpecRelationId { get; set; }

                /// <summary>Gets or sets 仕様項目ID</summary>
                /// <value>仕様項目ID</value>
                public int? SpecId { get; set; }

                /// <summary>Gets or sets 構成ID</summary>
                /// <value>構成ID</value>
                public int? StructureId { get; set; }
            }
        }

        /// <summary>機種別仕様一覧画面</summary>
        public class SpecList
        {
            /// <summary>一覧</summary>
            public class List : Common.Job
            {
                /// <summary>Gets or sets 機種別仕様関連付ID</summary>
                /// <value>機種別仕様関連付ID</value>
                public int MachineSpecRelationId { get; set; }
                /// <summary>Gets or sets 仕様名称</summary>
                /// <value>仕様名称</value>
                public string SpecificationName { get; set; }
                /// <summary>Gets or sets 表示順</summary>
                /// <value>表示順</value>
                public int? DisplayOrder { get; set; }
                /// <summary>Gets or sets 削除フラグ</summary>
                /// <value>削除フラグ</value>
                public bool DeleteFlg { get; set; }

            }
        }
        /// <summary>機種別仕様登録画面</summary>
        public class SpecRegist
        {
            /// <summary>仕様情報</summary>
            public class SpecInfo : Common.Job
            {
                /// <summary>Gets or sets 機種別仕様関連付ID</summary>
                /// <value>機種別仕様関連付ID</value>
                public int? MachineSpecRelationId { get; set; }
                /// <summary>Gets or sets 入力形式</summary>
                /// <value>入力形式</value>
                public int? SpecTypeId { get; set; }
                /// <summary>Gets or sets 数値書式</summary>
                /// <value>数値書式</value>
                public int? SpecNumDecimalPlaces { get; set; }
                /// <summary>Gets or sets 単位種別</summary>
                /// <value>単位種別</value>
                public int? SpecUnitTypeId { get; set; }
                /// <summary>Gets or sets 単位</summary>
                /// <value>単位</value>
                public int? SpecUnitId { get; set; }

                /// <summary>Gets or sets 表示順</summary>
                /// <value>表示順</value>
                public int? DisplayOrder { get; set; }

                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int? FactoryId { get; set; }

                /// <summary>Gets or sets 削除フラグ</summary>
                /// <value>削除フラグ</value>
                public int DeleteFlg { get; set; }

                /// <summary>Gets or sets 仕様項目ID</summary>
                /// <value>仕様項目ID</value>
                public int SpecId { get; set; }
                /// <summary>Gets or sets 更新シリアルID</summary>
                /// <value>更新シリアルID</value>
                /// <remarks>仕様項目マスタ</remarks>
                public int UpdateSerialid { get; set; }
                /// <summary>Gets or sets 更新シリアルID</summary>
                /// <value>更新シリアルID</value>
                /// <remarks>機種別仕様関連付マスタ</remarks>
                public int UpdateSerialidRelation { get; set; }

                /// <summary>Gets or sets 入力形式の拡張項目の値</summary>
                /// <value>入力形式の拡張項目の値</value>
                public string InputTypeExtension { get; set; }
            }
        }
        /// <summary>仕様項目選択値一覧画面</summary>
        public class SpecItemList
        {
            /// <summary>
            /// ヘッダと一覧の表示用データクラス
            /// </summary>
            public class Result : Common.Param
            {
                // 一覧の項目

                /// <summary>Gets or sets 選択肢名</summary>
                /// <value>選択肢名</value>
                public string ItemName { get; set; }
                /// <summary>Gets or sets 説明</summary>
                /// <value>説明</value>
                public string Description { get; set; }
                /// <summary>Gets or sets 表示順</summary>
                /// <value>表示順</value>
                public int? DisplayOrder { get; set; }
                /// <summary>Gets or sets 削除フラグ</summary>
                /// <value>削除フラグ</value>
                public int? DeleteFlg { get; set; }

                // ヘッダの項目
                /// <summary>Gets or sets 仕様項目名</summary>
                /// <value>仕様項目名</value>
                public string SpecName { get; set; }

                /// <summary>Gets or sets 言語ID</summary>
                /// <value>言語ID</value>
                public string LanguageId { get; set; }
            }
        }
        /// <summary>選択肢登録画面</summary>
        public class SpecItemRegist
        {
            /// <summary>
            /// 項目情報用データクラス
            /// </summary>
            public class Info : Common.Param
            {
                /// <summary>Gets or sets 表示順</summary>
                /// <value>表示順</value>
                public int? DisplayOrder { get; set; }
                /// <summary>Gets or sets 削除フラグ</summary>
                /// <value>削除フラグ</value>
                public int? DeleteFlg { get; set; }
                /// <summary>Gets or sets 構成ID</summary>
                /// <value>構成ID</value>
                public int? StructureId { get; set; }
                /// <summary>Gets or sets 構成マスタの更新シリアルID</summary>
                /// <value>構成マスタの更新シリアルID</value>
                public int? StructureUpdId { get; set; }

                /// <summary>Gets or sets アイテムマスタのID</summary>
                /// <value>アイテムマスタのID</value>
                public int? ItemId { get; set; }
                /// <summary>Gets or sets アイテムマスタの更新シリアルID</summary>
                /// <value>アイテムマスタの更新シリアルID</value>
                public int? ItemUpdId { get; set; }
            }

            /// <summary>
            /// 一覧の表示用データクラス
            /// </summary>
            public class Result : Common.Translation
            {
                // SQLでの取得用に入力項目も定義
                /// <summary>Gets or sets 表示順</summary>
                /// <value>表示順</value>
                public int? DisplayOrder { get; set; }
                /// <summary>Gets or sets 削除フラグ</summary>
                /// <value>削除フラグ</value>
                public int? DeleteFlg { get; set; }

                /// <summary>Gets or sets 構成ID</summary>
                /// <value>構成ID</value>
                public int? StructureId { get; set; }
                /// <summary>Gets or sets 構成マスタの更新シリアルID</summary>
                /// <value>構成マスタの更新シリアルID</value>
                public int? StructureUpdId { get; set; }

                /// <summary>Gets or sets アイテムマスタのID</summary>
                /// <value>アイテムマスタのID</value>
                public int? ItemId { get; set; }
                /// <summary>Gets or sets アイテムマスタの更新シリアルID</summary>
                /// <value>アイテムマスタの更新シリアルID</value>
                public int? ItemUpdId { get; set; }
            }
        }
    }
}
