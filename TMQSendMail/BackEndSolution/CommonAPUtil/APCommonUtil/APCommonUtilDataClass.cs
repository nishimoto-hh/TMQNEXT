using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;

namespace CommonAPUtil.APCommonUtil
{
    /// <summary>
    /// AP共通Daoクラス
    /// </summary>
    public class APCommonUtilDataClass : APCommonBaseClass
    {
        /// <summary>
        /// 翻訳管理マスタ
        /// </summary>
        public class TransrationEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TransrationEntity()
            {
                TableName = "attr_transration";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 翻訳ID</summary>
            /// <value>翻訳ID</value>
            public string TranslationId { get; set; }
            /// <summary>Gets or sets 翻訳値</summary>
            /// <value>翻訳値</value>
            public string TranslationValue { get; set; }
            /// <summary>Gets or sets 基本言語の名称値</summary>
            /// <value>基本言語の名称値</value>
            public string BaseValue { get; set; }
            /// <summary>Gets or sets コントロールのタイプ</summary>
            /// <value>コントロールのタイプ</value>
            public string AttrType { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets エラーフラグ(I/F処理のエラーカウントに使用)</summary>
            /// <value>エラーフラグ(I/F処理のエラーカウントに使用)</value>
            public int? ErrorFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 言語ID</summary>
                /// <value>言語ID</value>
                public string LanguageId { get; set; }
                /// <summary>Gets or sets 翻訳ID</summary>
                /// <value>翻訳ID</value>
                public string TranslationId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="languageId">言語ID</param>
                /// <param name="translationId">翻訳ID</param>
                public PrimaryKey(string languageId, string translationId)
                {
                    LanguageId = languageId;
                    TranslationId = translationId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.LanguageId, this.TranslationId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="languageId">言語ID</param>
            /// <param name="translationId">翻訳ID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TransrationEntity GetEntity(string languageId, string translationId, ComDB db)
            {
                TransrationEntity.PrimaryKey condition = new TransrationEntity.PrimaryKey(languageId, translationId);
                return GetEntity<TransrationEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// シーケンスマスタ
        /// </summary>
        public class SequencesEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SequencesEntity()
            {
                TableName = "sequences";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets シーケンスＩＤ</summary>
            /// <value>シーケンスＩＤ</value>
            public string Id { get; set; }
            /// <summary>Gets or sets 最大値</summary>
            /// <value>最大値</value>
            public decimal? MaxValue { get; set; }
            /// <summary>Gets or sets 最小値</summary>
            /// <value>最小値</value>
            public decimal? MinValue { get; set; }
            /// <summary>Gets or sets 増分値</summary>
            /// <value>増分値</value>
            public decimal? IncrementValue { get; set; }
            /// <summary>Gets or sets 開始値</summary>
            /// <value>開始値</value>
            public decimal? StartValue { get; set; }
            /// <summary>Gets or sets 現在値</summary>
            /// <value>現在値</value>
            public decimal? SeqValue { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets シーケンスＩＤ</summary>
                /// <value>シーケンスＩＤ</value>
                public string Id { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="id">シーケンスID</param>
                public PrimaryKey(string id)
                {
                    Id = id;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.Id);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="id">シーケンスID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public SequencesEntity GetEntity(string id, ComDB db)
            {
                SequencesEntity.PrimaryKey condition = new SequencesEntity.PrimaryKey(id);
                return GetEntity<SequencesEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 単価・仕様マスタ
        /// </summary>
        public class UnitSpecification : CommonTableItem
        {
            /// <summary>Gets or sets 単価</summary>
            /// <value>単価</value>
            public string Unitprice { get; set; }
            /// <summary>Gets or sets 単価区分</summary>
            /// <value>単価区分</value>
            public string UnitpriceDivision { get; set; }
            /// <summary>Gets or sets 運用管理単位</summary>
            /// <value>運用管理単位</value>
            public string UnitOfOperationManagement { get; set; }
            /// <summary>Gets or sets 換算係数</summary>
            /// <value>換算係数</value>
            public string KgOfFractionManagement { get; set; }
            /// <summary>Gets or sets 重量</summary>
            /// <value>重量</value>
            public string AllUpWeight { get; set; }

        }
    }
}
