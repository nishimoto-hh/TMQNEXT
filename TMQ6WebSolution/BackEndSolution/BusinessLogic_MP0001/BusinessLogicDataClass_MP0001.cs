using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_MP0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_MP0001
    {
        /// <summary>
        /// 一覧画面 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 場所階層ID(カンマ区切り)</summary>
            /// <value>場所階層ID(カンマ区切り)</value>
            public string LocationId { get; set; }
            /// <summary>Gets or sets 対象年月度</summary>
            /// <value>対象年月度</value>
            public DateTime TargetYear { get; set; }
            /// <summary>Gets or sets 期の開始月</summary>
            /// <value>期の開始月</value>
            public DateTime StartMonth { get; set; }
            /// <summary>Gets or sets 期首月</summary>
            /// <value>期首月</value>
            public DateTime BeginningMonth { get; set; }
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int StructureId { get; set; }
            /// <summary>Gets or sets 拡張データ</summary>
            /// <value>拡張データ</value>
            public int ExtensionData { get; set; }

        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 項目</summary>
            /// <value>項目</value>
            public string Item { get; set; }

            /// <summary>Gets or sets 要因</summary>
            /// <value>要因</value>
            public string Cause { get; set; }

            /// <summary>Gets or sets 凡例</summary>
            /// <value>凡例</value>
            public string UsageGuide { get; set; }

            /// <summary>Gets or sets 当月/半期/累計</summary>
            /// <value>当月/半期/累計</value>
            public string Period { get; set; }

            /// <summary>Gets or sets 機械</summary>
            /// <value>機械</value>
            public string Machine { get; set; }

            /// <summary>Gets or sets 電気</summary>
            /// <value>電気</value>
            public string Electricity { get; set; }

            /// <summary>Gets or sets 計装</summary>
            /// <value>計装</value>
            public string Instrumentation { get; set; }

            /// <summary>Gets or sets その他</summary>
            /// <value>その他</value>
            public string Other { get; set; }

            /// <summary>Gets or sets 日付</summary>
            /// <value>日付</value>
            public DateTime Date { get; set; }

        }

        /// <summary>
        /// SQL実行結果　階層情報取得
        /// </summary>
        public class StructureGetInfo
        {
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int StructureId { get; set; }
            /// <summary>Gets or sets 親構成ID</summary>
            /// <value>親構成ID</value>
            public int ParentStructureId { get; set; }
            /// <summary>Gets or sets 拡張データ</summary>
            /// <value>拡張データ</value>
            public int? extensionData { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int DisplayOrder { get; set; }
        }

        /// <summary>
        /// SQL実行結果　MQ指標実績表（年俸）の集計
        /// </summary>
        public class StructureGetRepMQActualEvaluationData
        {
            /// <summary>Gets or sets 機械</summary>
            /// <value>機械</value>
            public string Machine { get; set; }

            /// <summary>Gets or sets 電気</summary>
            /// <value>電気</value>
            public string Electricity { get; set; }

            /// <summary>Gets or sets 計装</summary>
            /// <value>計装</value>
            public string Instrumentation { get; set; }

            /// <summary>Gets or sets その他</summary>
            /// <value>その他</value>
            public string Other { get; set; }

            /// <summary>Gets or sets 合計</summary>
            /// <value>合計</value>
            public string Total { get; set; }
            /// <summary>Gets or sets 日付</summary>
            /// <value>日付</value>
            public DateTime TargetDate { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int RowNo { get; set; }
        }

        /// <summary>
        /// MQ月報(旧様式) 用検索条件
        /// </summary>
        public class JobOrder
        {
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int Structureid { get; set; }
            /// <summary>Gets or sets 職種名</summary>
            /// <value>職種名</value>
            public string Translationtext { get; set; }
            /// <summary>Gets or sets ソート順</summary>
            /// <value>ソート順</value>
            public int Sort { get; set; }
        }

        /// <summary>
        /// MQ月報(旧様式) 出力に使用するためのリスト
        /// </summary>
        public class JobListForOutput
        {
            /// <summary>Gets or sets 職種名</summary>
            /// <value>職種名</value>
            public string Translationtext { get; set; }
            /// <summary>Gets or sets 構成IDリスト</summary>
            /// <value>構成IDリスト</value>
            public List<int> StructureIdList { get; set; }
        }
    }
}
