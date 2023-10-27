using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_DM0002
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_DM0002
    {
        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.AttachmentEntity
        {
            /// <summary>Gets or sets 添付元</summary>
            /// <value>添付元</value>
            public string ConductName { get; set; }
            /// <summary>Gets or sets アクション</summary>
            /// <value>アクション</value>
            public string FunctionName { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets ファイル／リンク</summary>
            /// <value>ファイル／リンク</value>
            public string FileLinkName { get; set; }
            /// <summary>Gets or sets リンク</summary>
            /// <value>リンク</value>
            public string Link { get; set; }
            /// <summary>Gets or sets 作成者</summary>
            /// <value>作成者</value>
            public string PersonName { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int JobStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? PlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? SeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? StrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? FacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string FacilityName { get; set; }
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
            /// <summary>Gets or sets 添付種類番号</summary>
            /// <value>添付種類番号</value>
            public int AttachmentTypeNo { get; set; }
            /// <summary>Gets or sets 添付種類拡張項目</summary>
            /// <value>添付種類拡張項目</value>
            public int ExtensionData { get; set; }
        }

        /// <summary>
        /// 詳細画面 件名情報
        /// </summary>
        public class subject : ComDao.AttachmentEntity
        {
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 表示する文書種類コンボボックスの項目番号</summary>
            /// <value>表示する文書種類コンボボックスの項目番号</value>
            public int DocumentTypeValNo { get; set; }
        }

        /// <summary>
        /// アップロード先ディレクトリ構成
        /// </summary>
        public class directory
        {
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int SequenceNo { get; set; }
            /// <summary>Gets or sets 階層</summary>
            /// <value>階層</value>
            public string DirectoryName { get; set; }
        }
    }
}
