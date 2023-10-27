using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using IListAccessor = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase.AccessorUtil.IListAccessor;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using BusinessLogicBase = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase;

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
        public class searchResult : ComDao.CommonTableItem, IListAccessor
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

            /// <summary>
            /// 一時テーブルレイアウト作成処理(性能改善対応)
            /// </summary>
            /// <param name="mapDic">マッピング情報のディクショナリ</param>
            /// <returns>一時テーブルレイアウト</returns>
            public dynamic GetTmpTableData(Dictionary<string, ComUtil.DBMappingInfo> mapDic)
            {
                dynamic paramObj;

                paramObj = new ExpandoObject() as IDictionary<string, object>;
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FactoryId, nameof(this.FactoryId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsJobId, nameof(this.PartsJobId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LastConfirmedDate, nameof(this.LastConfirmedDate), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LastConfirmedPerson, nameof(this.LastConfirmedPerson), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LastConfirmedDatetime, nameof(this.LastConfirmedDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MaxUpdateDatetimeConfirm, nameof(this.MaxUpdateDatetimeConfirm), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MaxUpdateDatetimeFixed, nameof(this.MaxUpdateDatetimeFixed), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TargetMonth, nameof(this.TargetMonth), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FactoryName, nameof(this.FactoryName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobName, nameof(this.JobName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateSerialid, nameof(this.UpdateSerialid), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.InsertDatetime, nameof(this.InsertDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.InsertUserId, nameof(this.InsertUserId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateDatetime, nameof(this.UpdateDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateUserId, nameof(this.UpdateUserId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DeleteFlg, nameof(this.DeleteFlg), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LanguageId, nameof(this.LanguageId), mapDic);

                return paramObj;
            }

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
