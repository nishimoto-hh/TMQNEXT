using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_PT0004
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_PT0004
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.PtStockComfirmEntity
        {
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string UserId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public long FactoryId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public long PartsJobId { get; set; }
            /// <summary>Gets or sets 最終確定年月</summary>
            /// <value>最終確定年月</value>
            public DateTime? LastConfirmedDate { get; set; }
            /// <summary>Gets or sets 最終確定者</summary>
            /// <value>最終確定者</value>
            public string LastConfirmedPerson { get; set; }
            /// <summary>Gets or sets 最終確定日時</summary>
            /// <value>最終確定日時</value>
            public DateTime? LastConfirmedDatetime { get; set; }
            /// <summary>Gets or sets 最大更新日時(在庫確定管理データ)</summary>
            /// <value>最大更新日時(在庫確定管理データ)</value>
            public DateTime? MaxUpdateDatetimeConfirm { get; set; }
            /// <summary>Gets or sets 最大更新日時(確定在庫データ)</summary>
            /// <value>最大更新日時(確定在庫データ)</value>
            public DateTime? MaxUpdateDatetimeFixed { get; set; }
            /// <summary>Gets or sets 対象年月</summary>
            /// <value>対象年月</value>
            public string TargetMonth { get; set; }
            /// <summary>Gets or sets 工場名</summary>
            /// <value>工場名</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets 職種名</summary>
            /// <value>職種名</value>
            public string JobName { get; set; }

        }

        /// <summary>
        /// 確定在庫データ取得用検索条件
        /// </summary>
        public class searchConditionFixedStock : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public long FactoryId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public long PartsJobId { get; set; }
            /// <summary>Gets or sets 受払日時From</summary>
            /// <value>受払日時From</value>
            public DateTime? InoutDatetimeFrom { get; set; }
            /// <summary>Gets or sets 受払日時To</summary>
            /// <value>受払日時To</value>
            public DateTime InoutDatetimeTo { get; set; }
            /// <summary>Gets or sets 最終確定年月</summary>
            /// <value>最終確定年月</value>
            public string LastConfirmedDate { get; set; }
        }

        /// <summary>
        /// フラグ一覧(ボタンの非活性、登録時に使用)
        /// </summary>
        public class flgList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 日付大小フラグ</summary>
            /// <value>工場ID</value>
            public int? FlgJudgeTargetMonth { get; set; }
            /// <summary>Gets or sets 対象年月</summary>
            /// <value>対象年月</value>
            public string TargetMonth { get; set; }
        }
    }
}
