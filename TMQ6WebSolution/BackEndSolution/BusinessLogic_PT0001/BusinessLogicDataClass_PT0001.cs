using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

using IListAccessor = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase.AccessorUtil.IListAccessor;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using BusinessLogicBase = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase;

namespace BusinessLogic_PT0001
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_PT0001
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class detailSearchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 機能タイプID(削除処理で使用)</summary>
            /// <value>機能タイプID(削除処理で使用)</value>
            public int? FunctionTypeId { get; set; }
            /// <summary>Gets or sets 年度From(入出庫一覧・画面表示用)</summary>
            /// <value>年度From(入出庫一覧・画面表示用)</value>
            public string YearFrom { get; set; }
            /// <summary>Gets or sets 年度To(入出庫一覧・画面表示用)</summary>
            /// <value>年度To(入出庫一覧・画面表示用)</value>
            public string YearTo { get; set; }
            /// <summary>Gets or sets 年度From(入出庫一覧・画面表示用)</summary>
            /// <value>年度From(入出庫一覧・画面表示用)</value>
            public string DispYearFrom { get; set; }
            /// <summary>Gets or sets 年度To(入出庫一覧・画面表示用)</summary>
            /// <value>年度To(入出庫一覧・画面表示用)</value>
            public string DispYearTo { get; set; }
            /// <summary>Gets or sets ユーザーの本務工場</summary>
            /// <value>ユーザーの本務工場</value>
            public long UserFactoryId { get; set; }
        }

        /// <summary>
        /// 添付情報の最大日時取得条件
        /// </summary>
        public class attachmentMaxDate : ComDao.PtPartsEntity
        {
            /// <summary>Gets or sets 最大更新日時</summary>
            /// <value>最大更新日時</value>
            public DateTime? MaxUpdateDatetime { get; set; }
            /// <summary>Gets or sets 添付ファイル削除用キーID</summary>
            /// <value>添付ファイル削除用キーID</value>
            public int? KeyId { get; set; }
            /// <summary>Gets or sets 機能タイプID</summary>
            /// <value>機能タイプID</value>
            public List<int> FunctionTypeId { get; set; }
        }
        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class searchResult : ComDao.PtPartsEntity, IListAccessor
        {
            /// <summary>Gets or sets 発注点(検索処理は「数量+単位」で表示するのでstring型で定義)</summary>
            /// <value>発注点</value>
            public string LeadTime { get; set; }
            /// <summary>Gets or sets 発注量(検索処理は「数量+単位」で表示するのでstring型で定義)</summary>
            /// <value>発注量</value>
            public string OrderQuantity { get; set; }
            /// <summary>Gets or sets 標準単価(検索処理は「数量+単位」で表示するのでstring型で定義)</summary>
            /// <value>標準単価</value>
            public string UnitPrice { get; set; }
            /// <summary>Gets or sets 標準単価(単位なし)</summary>
            /// <value>標準単価(単位なし)</value>
            public string UnitPriceExceptUnit { get; set; }
            /// <summary>Gets or sets 発注点(単位なし)</summary>
            /// <value>発注点(単位なし)</value>
            public decimal? LeadTimeExceptUnit { get; set; }
            /// <summary>Gets or sets 発注量(単位なし)</summary>
            /// <value>発注量(単位なし)</value>
            public decimal? OrderQuantityExceptUnit { get; set; }
            /// <summary>Gets or sets 画像</summary>
            /// <value>画像</value>
            public string Image { get; set; }
            /// <summary>Gets or sets 最新在庫数</summary>
            /// <value>最新在庫数</value>
            public string StockQuantity { get; set; }
            /// <summary>Gets or sets 最新在庫数(単位なし)</summary>
            /// <value>最新在庫数(単位なし)</value>
            public decimal? StockQuantityExceptUnit { get; set; }
            /// <summary>Gets or sets 発注アラーム</summary>
            /// <value>発注アラーム</value>
            public bool? OrderAlert { get; set; }
            /// <summary>Gets or sets 予備品№(変更前)</summary>
            /// <value>予備品№(変更前)</value>
            public string PartsNoBefore { get; set; }
            /// <summary>Gets or sets 数量管理単位(名称)</summary>
            /// <value>数量管理単位(名称)</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 金額管理単位(名称)</summary>
            /// <value>金額管理単位(名称)</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(数量)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int UnitDigit { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(金額)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int CurrencyDigit { get; set; }
            /// <summary>Gets or sets RFIDタグ件数</summary>
            /// <value>RFIDタグ件数</value>
            public string RfCount { get; set; }
            /// <summary>Gets or sets RFIDタグ件数 単位</summary>
            /// <value>RFIDタグ件数 単位</value>
            public string Matter { get; set; }
            /// <summary>Gets or sets RFIDタグ</summary>
            /// <value>RFIDタグ</value>
            public string RfIdTag { get; set; }
            /// <summary>Gets or sets 丸め処理区分(数量)</summary>
            /// <value>丸め処理区分(数量)</value>
            public int UnitRoundDivision { get; set; }
            /// <summary>Gets or sets 丸め処理区分(金額)</summary>
            /// <value>丸め処理区分(金額)</value>
            public int CurrencyRoundDivision { get; set; }
            /// <summary>Gets or sets 最大更新日時</summary>
            /// <value>最大更新日時</value>
            public DateTime? MaxUpdateDatetime { get; set; }
            /// <summary>Gets or sets 棚番から取得した工場ID</summary>
            /// <value>棚番から取得した工場ID</value>
            public long DefaultFactoryId { get; set; }
            /// <summary>Gets or sets 管理工場ID</summary>
            /// <value>管理工場ID</value>
            public long PartsFactoryId { get; set; }
            /// <summary>Gets or sets ダウンロードリンク(文書)</summary>
            /// <value> ダウンロードリンク(文書)</value>
            public string FileLinkDocument { get; set; }
            /// <summary>Gets or sets  ダウンロードリンク(画像)</summary>
            /// <value> ダウンロードリンク(画像)<value>
            public string FileLinkImage { get; set; }
            /// <summary>Gets or sets メーカー名</summary>
            /// <value>メーカー名</value>
            public string ManufactureName { get; set; }
            /// <summary>Gets or sets 仕入先名</summary>
            /// <value>仕入先名</value>
            public string VenderName { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int PartsParentStructureId { get; set; }
            /// <summary>Gets or sets 管理工場名</summary>
            /// <value>管理工場名</value>
            public string PartsFactoryName { get; set; }
            /// <summary>Gets or sets 工場の丸め処理区分</summary>
            /// <value>工場の丸め処理区分</value>
            public int FactoryRoundDivision { get; set; }
            /// <summary>Gets or sets 標準棚番の階層番号</summary>
            /// <value>標準棚番の階層番号</value>
            public int StructureLayerNo { get; set; }
            /// <summary>Gets or sets 標準部門コード</summary>
            /// <value>標準部門コード</value>
            public string DepartmentCode { get; set; }
            /// <summary>Gets or sets 標準勘定科目コード</summary>
            /// <value>標準勘定科目コード</value>
            public string AccountCode { get; set; }
            /// <summary>Gets or sets 標準部門名</summary>
            /// <value>標準部門名</value>
            public string DepartmentName { get; set; }
            /// <summary>Gets or sets 標準勘定科目名</summary>
            /// <value>標準勘定科目名</value>
            public string AccountName { get; set; }
            /// <summary>Gets or sets 予備品情報に「標準棚番」「標準部門」「標準勘定科目」が登録されているかどうか</summary>
            /// <value>予備品情報に「標準棚番」「標準部門」「標準勘定科目」が登録されているかどうか</value>
            public bool IsRegistedRequiredItemToOutLabel { get; set; }
            /// <summary>Gets or sets 棚ID(ラベル出力用)</summary>
            /// <value>棚ID(ラベル出力用)</value>
            public long PartsLocationIdEnter { get; set; }
            /// <summary>Gets or sets 棚枝番(ラベル出力用)</summary>
            /// <value>棚枝番(ラベル出力用)</value>
            public string PartsLocationDetailNoEnter { get; set; }
            /// <summary>Gets or sets 部門CD(ラベル出力用)</summary>
            /// <value>部門CD(ラベル出力用)</value>
            public string DepartmentCdEnter { get; set; }
            /// <summary>Gets or sets 勘定科目CD(ラベル出力用)</summary>
            /// <value>勘定科目CD(ラベル出力用)</value>
            public string SubjectCdEnter { get; set; }
            /// <summary>Gets or sets 表示年度(From)</summary>
            /// <value>表示年度(From)</value>
            public string DispYearFrom { get; set; }
            /// <summary>Gets or sets 表示年度(To)</summary>
            /// <value>表示年度(To)</value>
            public string DispYearTo { get; set; }

            #region 共通　地区・職種設定用
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            //public int? FactoryId { get; set; }
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
            /// <summary>Gets or sets 倉庫ID</summary>
            /// <value>倉庫ID</value>
            public int? WarehouseId { get; set; }
            /// <summary>Gets or sets 倉庫名称</summary>
            /// <value>倉庫名称</value>
            public string WarehouseName { get; set; }
            /// <summary>Gets or sets 棚番ID</summary>
            /// <value>棚番ID</value>
            public int? RackId { get; set; }
            /// <summary>Gets or sets 棚番名称</summary>
            /// <value>棚番名称</value>
            public string RackName { get; set; }
            /// <summary>Gets or sets 標準棚ID</summary>
            /// <value>標準棚ID</value>
            public string LocationId { get; set; }
            #endregion

            /// <summary>
            /// 数量・金額の単位結合、丸め処理
            /// </summary>
            public void JoinStrAndRound()
            {
                this.LeadTime = TMQUtil.roundDigit(this.LeadTime, this.UnitDigit, this.UnitRoundDivision);           // 発注点
                this.OrderQuantity = TMQUtil.roundDigit(this.OrderQuantity, this.UnitDigit, this.UnitRoundDivision); // 発注量
                this.UnitPrice = TMQUtil.roundDigit(this.UnitPrice, this.CurrencyDigit, this.CurrencyRoundDivision); // 標準単価
                this.StockQuantity = TMQUtil.roundDigit(this.StockQuantity, this.UnitDigit, this.UnitRoundDivision); // 最新在庫数

                this.RfCount = TMQUtil.CombineNumberAndUnit(this.RfCount, this.Matter, true);               // RFタグ件数
                this.LeadTime = TMQUtil.CombineNumberAndUnit(this.LeadTime, this.UnitName, true);           // 発注点
                this.OrderQuantity = TMQUtil.CombineNumberAndUnit(this.OrderQuantity, this.UnitName, true); // 発注量
                this.UnitPrice = TMQUtil.CombineNumberAndUnit(this.UnitPrice, this.CurrencyName, true);     // 標準単価
                this.StockQuantity = TMQUtil.CombineNumberAndUnit(this.StockQuantity, this.UnitName, true); // 最新在庫数
            }

            /// <summary>
            /// 一時テーブルレイアウト作成処理(性能改善対応)
            /// </summary>
            /// <param name="mapDic">マッピング情報のディクショナリ</param>
            /// <returns>一時テーブルレイアウト</returns>
            public dynamic GetTmpTableData(Dictionary<string, ComUtil.DBMappingInfo> mapDic)
            {
                dynamic paramObj;

                paramObj = new ExpandoObject() as IDictionary<string, object>;
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LeadTime, nameof(this.LeadTime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.OrderQuantity, nameof(this.OrderQuantity), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UnitPrice, nameof(this.UnitPrice), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UnitPriceExceptUnit, nameof(this.UnitPriceExceptUnit), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LeadTimeExceptUnit, nameof(this.LeadTimeExceptUnit), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.OrderQuantityExceptUnit, nameof(this.OrderQuantityExceptUnit), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.Image, nameof(this.Image), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StockQuantity, nameof(this.StockQuantity), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StockQuantityExceptUnit, nameof(this.StockQuantityExceptUnit), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.OrderAlert, nameof(this.OrderAlert), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsNoBefore, nameof(this.PartsNoBefore), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UnitName, nameof(this.UnitName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.CurrencyName, nameof(this.CurrencyName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UnitDigit, nameof(this.UnitDigit), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.CurrencyDigit, nameof(this.CurrencyDigit), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.RfCount, nameof(this.RfCount), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.Matter, nameof(this.Matter), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.RfIdTag, nameof(this.RfIdTag), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UnitRoundDivision, nameof(this.UnitRoundDivision), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.CurrencyRoundDivision, nameof(this.CurrencyRoundDivision), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MaxUpdateDatetime, nameof(this.MaxUpdateDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DefaultFactoryId, nameof(this.DefaultFactoryId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsFactoryId, nameof(this.PartsFactoryId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FileLinkDocument, nameof(this.FileLinkDocument), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FileLinkImage, nameof(this.FileLinkImage), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ManufactureName, nameof(this.ManufactureName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.VenderName, nameof(this.VenderName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsParentStructureId, nameof(this.PartsParentStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsFactoryName, nameof(this.PartsFactoryName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FactoryRoundDivision, nameof(this.FactoryRoundDivision), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StructureLayerNo, nameof(this.StructureLayerNo), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationStructureId, nameof(this.LocationStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DistrictId, nameof(this.DistrictId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DistrictName, nameof(this.DistrictName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FactoryName, nameof(this.FactoryName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PlantId, nameof(this.PlantId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PlantName, nameof(this.PlantName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SeriesId, nameof(this.SeriesId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SeriesName, nameof(this.SeriesName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StrokeId, nameof(this.StrokeId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StrokeName, nameof(this.StrokeName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FacilityId, nameof(this.FacilityId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FacilityName, nameof(this.FacilityName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobId, nameof(this.JobId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobName, nameof(this.JobName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LargeClassficationId, nameof(this.LargeClassficationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LargeClassficationName, nameof(this.LargeClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MiddleClassficationId, nameof(this.MiddleClassficationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.MiddleClassficationName, nameof(this.MiddleClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SmallClassficationId, nameof(this.SmallClassficationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.SmallClassficationName, nameof(this.SmallClassficationName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.WarehouseId, nameof(this.WarehouseId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.WarehouseName, nameof(this.WarehouseName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.RackId, nameof(this.RackId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.RackName, nameof(this.RackName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationId, nameof(this.LocationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.TableName, nameof(this.TableName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsId, nameof(this.PartsId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsNo, nameof(this.PartsNo), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsName, nameof(this.PartsName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ManufacturerStructureId, nameof(this.ManufacturerStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.Materials, nameof(this.Materials), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.ModelType, nameof(this.ModelType), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.StandardSize, nameof(this.StandardSize), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsServiceSpace, nameof(this.PartsServiceSpace), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.FactoryId, nameof(this.FactoryId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.JobStructureId, nameof(this.JobStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsLocationId, nameof(this.PartsLocationId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationDistrictStructureId, nameof(this.LocationDistrictStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationFactoryStructureId, nameof(this.LocationFactoryStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationWarehouseStructureId, nameof(this.LocationWarehouseStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LocationRackStructureId, nameof(this.LocationRackStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsLocationDetailNo, nameof(this.PartsLocationDetailNo), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UnitStructureId, nameof(this.UnitStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.VenderStructureId, nameof(this.VenderStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.CurrencyStructureId, nameof(this.CurrencyStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PurchasingNo, nameof(this.PurchasingNo), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.PartsMemo, nameof(this.PartsMemo), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UseSegmentStructureId, nameof(this.UseSegmentStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateSerialid, nameof(this.UpdateSerialid), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.InsertDatetime, nameof(this.InsertDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.InsertUserId, nameof(this.InsertUserId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateDatetime, nameof(this.UpdateDatetime), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.UpdateUserId, nameof(this.UpdateUserId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DeleteFlg, nameof(this.DeleteFlg), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.LanguageId, nameof(this.LanguageId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DepartmentStructureId, nameof(this.DepartmentStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.AccountStructureId, nameof(this.AccountStructureId), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DepartmentCode, nameof(this.DepartmentCode), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.AccountCode, nameof(this.AccountCode), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.DepartmentName, nameof(this.DepartmentName), mapDic);
                IListAccessor.SetParamKeyAndValue(ref paramObj, this.AccountName, nameof(this.AccountName), mapDic);

                return paramObj;
            }

        }

        /// <summary>
        /// 詳細編集画面のデータクラス
        /// </summary>
        public class editResult : ComDao.PtPartsEntity
        {
            /// <summary>Gets or sets 予備品№(変更前)</summary>
            /// <value>予備品№(変更前)</value>
            public string PartsNoBefore { get; set; }
            /// <summary>Gets or sets 発注点(定義のコントロールにあわせて作成)</summary>
            /// <value>発注点</value>
            public decimal? LeadTimeExceptUnit { get; set; }
            /// <summary>Gets or sets 標準単価(定義のコントロールにあわせて作成)</summary>
            /// <value>標準単価</value>
            public decimal UnitPriceExceptUnit { get; set; }
            /// <summary>Gets or sets 発注量(定義のコントロールにあわせて作成)</summary>
            /// <value>発注量</value>
            public decimal? OrderQuantityExceptUnit { get; set; }
            /// <summary>Gets or sets 管理工場ID</summary>
            /// <value>管理工場ID</value>
            public long PartsFactoryId { get; set; }
            /// <summary>Gets or sets メーカー名</summary>
            /// <value>メーカー名</value>
            public string ManufactureName { get; set; }
            /// <summary>Gets or sets 仕入先名</summary>
            /// <value>仕入先名</value>
            public string VenderName { get; set; }
            /// <summary>Gets or sets 標準部門コード</summary>
            /// <value>標準部門コード</value>
            public string DepartmentCode { get; set; }
            /// <summary>Gets or sets 標準勘定科目コード</summary>
            /// <value>標準勘定科目コード</value>
            public string AccountCode { get; set; }
            /// <summary>Gets or sets 表示年度(From)</summary>
            /// <value>表示年度(From)</value>
            public string DispYearFrom { get; set; }
            /// <summary>Gets or sets 表示年度(To)</summary>
            /// <value>表示年度(To)</value>
            public string DispYearTo { get; set; }

            #region 共通　地区・職種設定用
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            //public int? FactoryId { get; set; }
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
            /// <summary>Gets or sets 倉庫ID</summary>
            /// <value>倉庫ID</value>
            public int? WarehouseId { get; set; }
            /// <summary>Gets or sets 倉庫名称</summary>
            /// <value>倉庫名称</value>
            public string WarehouseName { get; set; }
            /// <summary>Gets or sets 棚番ID</summary>
            /// <value>棚番ID</value>
            public int? RackId { get; set; }
            /// <summary>Gets or sets 棚番名称</summary>
            /// <value>棚番名称</value>
            public string RackName { get; set; }
            /// <summary>Gets or sets 標準棚ID</summary>
            /// <value>標準棚ID</value>
            public string LocationId { get; set; }
            #endregion
        }

        /// <summary>
        /// 詳細編集画面 結合文字列
        /// </summary>
        public class editJoinString
        {
            /// <summary>Gets or sets 棚番・枝番の結合文字列</summary>
            /// <value>棚番・枝番の結合文字列</value>
            public string JoinString { get; set; }
        }

        /// <summary>
        /// 詳細画面 棚別在庫一覧
        /// </summary>
        public class inventoryList : ComDao.PtPartsEntity
        {
            /// <summary>Gets or sets 棚番</summary>
            /// <value>棚番</value>
            public string LocationNm { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public string StockQuantity { get; set; }
            /// <summary>Gets or sets 入庫日</summary>
            /// <value>入庫日</value>
            public DateTime? ReceivingDatetime { get; set; }
            /// <summary>Gets or sets 入庫No</summary>
            /// <value>入庫No</value>
            public int? LotNo { get; set; }
            /// <summary>Gets or sets 新旧区分</summary>
            /// <value>新旧区分</value>
            public string OldNewStructureId { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public string UnitPrice { get; set; }
            /// <summary>Gets or sets 在庫金額</summary>
            /// <value>在庫金額</value>
            public string StockAmount { get; set; }
            /// <summary>Gets or sets 部門</summary>
            /// <value>部門</value>
            public string DepartmentNm { get; set; }
            /// <summary>Gets or sets 勘定科目</summary>
            /// <value>勘定科目</value>
            public string SubjectNm { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 管理No</summary>
            /// <value>管理No</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 数量管理単位(名称)</summary>
            /// <value>数量管理単位(名称)</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 金額管理単位(名称)</summary>
            /// <value>金額管理単位(名称)</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 入れ子キー</summary>
            /// <value>入れ子キー</value>
            public string NestKey { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(数量)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int UnitDigit { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(金額)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int CurrencyDigit { get; set; }
            /// <summary>Gets or sets 丸め処理区分(数量)</summary>
            /// <value>丸め処理区分(数量)</value>
            public int UnitRoundDivision { get; set; }
            /// <summary>Gets or sets 丸め処理区分(金額)</summary>
            /// <value>丸め処理区分(金額)</value>
            public int CurrencyRoundDivision { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCdEnter { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string SubjectCdEnter { get; set; }
            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public long PartsLocationIdEnter { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNoEnter { get; set; }

            /// <summary>
            /// 数量・金額の単位結合、丸め処理
            /// </summary>
            /// <param name="results">検索結果</param>
            /// <param name="strJoin">棚番と枝番の結合文字列</param>
            public static void joinStrAndRound(IList<inventoryList> results, string strJoin)
            {
                foreach (inventoryList result in results)
                {
                    // 棚番と棚枝番を結合
                    if (!string.IsNullOrEmpty(result.PartsLocationDetailNo))
                    {
                        result.LocationNm = TMQUtil.GetDisplayPartsLocation(result.LocationNm, result.PartsLocationDetailNo, strJoin);
                    }

                    result.StockQuantity = TMQUtil.roundDigit(result.StockQuantity, result.UnitDigit, result.UnitRoundDivision);     // 在庫数
                    result.UnitPrice = TMQUtil.roundDigit(result.UnitPrice, result.CurrencyDigit, result.CurrencyRoundDivision);     // 入庫単価
                    result.StockAmount = TMQUtil.roundDigit(result.StockAmount, result.CurrencyDigit, result.CurrencyRoundDivision); // 在庫金額

                    result.StockQuantity = TMQUtil.CombineNumberAndUnit(result.StockQuantity, result.UnitName, true); // 在庫数
                    result.UnitPrice = TMQUtil.CombineNumberAndUnit(result.UnitPrice, result.CurrencyName, true);     // 入庫単価
                    result.StockAmount = TMQUtil.CombineNumberAndUnit(result.StockAmount, result.CurrencyName, true); // 在庫金額
                }
            }
        }

        /// <summary>
        /// 詳細画面 部門別在庫一覧
        /// </summary>
        public class categoryList : ComDao.PtPartsEntity
        {
            /// <summary>Gets or sets 新旧区分</summary>
            /// <value>新旧区分</value>
            public string OldNewStructureId { get; set; }
            /// <summary>Gets or sets 部門</summary>
            /// <value>部門</value>
            public string DepartmentNm { get; set; }
            /// <summary>Gets or sets 勘定科目</summary>
            /// <value>勘定科目</value>
            public string SubjectNm { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 管理No</summary>
            /// <value>管理No</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public string StockQuantity { get; set; }
            /// <summary>Gets or sets 在庫金額</summary>
            /// <value>在庫金額</value>
            public string StockAmount { get; set; }
            /// <summary>Gets or sets 入庫No</summary>
            /// <value>入庫No</value>
            public int LotNo { get; set; }
            /// <summary>Gets or sets 入庫日</summary>
            /// <value>入庫日</value>
            public DateTime? ReceivingDatetime { get; set; }
            /// <summary>Gets or sets 棚番</summary>
            /// <value>棚番</value>
            public string LocationNm { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public string UnitPrice { get; set; }
            /// <summary>Gets or sets 数量管理単位(名称)</summary>
            /// <value>数量管理単位(名称)</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 金額管理単位(名称)</summary>
            /// <value>金額管理単位(名称)</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 入れ子キー</summary>
            /// <value>入れ子キー</value>
            public string NestKey { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(数量)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int UnitDigit { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(金額)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int CurrencyDigit { get; set; }
            /// <summary>Gets or sets 丸め処理区分(数量)</summary>
            /// <value>丸め処理区分(数量)</value>
            public int UnitRoundDivision { get; set; }
            /// <summary>Gets or sets 丸め処理区分(金額)</summary>
            /// <value>丸め処理区分(金額)</value>
            public int CurrencyRoundDivision { get; set; }

            /// <summary>
            /// 数量・金額の単位結合、丸め処理
            /// </summary>
            /// <param name="results">検索結果</param>
            /// <param name="strJoin">棚番と枝番の結合文字列</param>
            public static void joinStrAndRound(IList<categoryList> results, string strJoin)
            {
                foreach (categoryList result in results)
                {
                    // 棚番と棚枝番を結合
                    if (!string.IsNullOrEmpty(result.PartsLocationDetailNo))
                    {
                        result.LocationNm = TMQUtil.GetDisplayPartsLocation(result.LocationNm, result.PartsLocationDetailNo, strJoin);
                    }

                    result.StockQuantity = TMQUtil.roundDigit(result.StockQuantity, result.UnitDigit, result.UnitRoundDivision);     // 在庫数
                    result.UnitPrice = TMQUtil.roundDigit(result.UnitPrice, result.CurrencyDigit, result.CurrencyRoundDivision);     // 入庫単価
                    result.StockAmount = TMQUtil.roundDigit(result.StockAmount, result.CurrencyDigit, result.CurrencyRoundDivision); // 在庫金額

                    result.StockQuantity = TMQUtil.CombineNumberAndUnit(result.StockQuantity, result.UnitName, true); // 在庫数
                    result.UnitPrice = TMQUtil.CombineNumberAndUnit(result.UnitPrice, result.CurrencyName, true);     // 入庫単価
                    result.StockAmount = TMQUtil.CombineNumberAndUnit(result.StockAmount, result.CurrencyName, true); // 在庫金額
                }
            }
        }

        /// <summary>
        /// 詳細画面 入出庫履歴一覧
        /// </summary>
        public class inoutHistoryList : ComDao.PtPartsEntity
        {
            /// <summary>Gets or sets 入出庫区分</summary>
            /// <value>入出庫区分</value>
            public int WorkDivisionStructureId { get; set; }
            /// <summary>Gets or sets 日付</summary>
            /// <value>日付</value>
            public DateTime? Date { get; set; }
            /// <summary>Gets or sets 入庫No</summary>
            /// <value>入庫No</value>
            public int LotNo { get; set; }
            /// <summary>Gets or sets 新旧区分</summary>
            /// <value>新旧区分</value>
            public string OldNewStructureId { get; set; }
            /// <summary>Gets or sets 入庫数</summary>
            /// <value>入庫数</value>
            public string InoutQuantity { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public string UnitPrice { get; set; }
            /// <summary>Gets or sets 入庫金額</summary>
            /// <value>入庫金額</value>
            public string StorageAmount { get; set; }
            /// <summary>Gets or sets 出庫数</summary>
            /// <value>出庫数</value>
            public string ShippingQuantity { get; set; }
            /// <summary>Gets or sets 出庫金額</summary>
            /// <value>出庫金額</value>
            public string ShippingAmount { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public string StockQuantity { get; set; }
            /// <summary>Gets or sets 在庫金額</summary>
            /// <value>在庫金額</value>
            public string StockAmount { get; set; }
            /// <summary>Gets or sets 数量管理単位(名称)</summary>
            /// <value>数量管理単位(名称)</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 金額管理単位(名称)</summary>
            /// <value>金額管理単位(名称)</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(数量)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int UnitDigit { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(金額)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int CurrencyDigit { get; set; }
            /// <summary>Gets or sets 入庫数(在庫数計算用)</summary>
            /// <value>入庫数(在庫数計算用)</value>
            public decimal InoutQuantityCalc { get; set; }
            /// <summary>Gets or sets 出庫数(在庫数計算用)</summary>
            /// <value>出庫数(在庫数計算用)</value>
            public decimal ShippingQuantityCalc { get; set; }
            /// <summary>Gets or sets 入庫金額(在庫金額計算用)</summary>
            /// <value>入庫金額(在庫金額計算用)</value>
            public decimal StorageAmountCalc { get; set; }

            /// <summary>Gets or sets 出庫金額(在庫金額計算用)</summary>
            /// <value>出庫金額(在庫金額計算用)</value>
            public decimal ShippingAmountCalc { get; set; }

            /// <summary>Gets or sets 在庫数(在庫数計算用)</summary>
            /// <value>在庫数(在庫数計算用)</value>
            public decimal StockQuantityCalc { get; set; }

            /// <summary>Gets or sets 在庫金額(在庫金額計算用)</summary>
            /// <value>在庫金額(在庫金額計算用)</value>
            public decimal StockAmountCalc { get; set; }
            /// <summary>Gets or sets フラグ(在庫数・金額計算用)</summary>
            /// <value>フラグ(在庫数・金額計算用)</value>
            public string Flg { get; set; }
            /// <summary>Gets or sets 受払履歴ID</summary>
            /// <value>受払履歴ID</value>
            public long InoutHistoryId { get; set; }
            /// <summary>Gets or sets 丸め処理区分(数量)</summary>
            /// <value>丸め処理区分(数量)</value>
            public int UnitRoundDivision { get; set; }
            /// <summary>Gets or sets 丸め処理区分(金額)</summary>
            /// <value>丸め処理区分(金額)</value>
            public int CurrencyRoundDivision { get; set; }
            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public long WorkNo { get; set; }

            /// <summary>
            /// 数量・金額の単位結合、丸め処理
            /// </summary>
            /// <param name="results">検索結果</param>
            public static void joinStrAndRound(IList<inoutHistoryList> results)
            {

                // 在庫数・金額計算用フラグ(繰越のデータの場合は0、それ以外のデータは1
                string carryData = "0";

                // 「繰越」のデータの丸め処理・数量と単位の結合
                // 入庫数
                results[0].InoutQuantity = TMQUtil.roundDigit(results[0].InoutQuantity, results[0].UnitDigit, results[0].UnitRoundDivision);
                results[0].InoutQuantity = TMQUtil.CombineNumberAndUnit(results[0].InoutQuantity, results[0].UnitName, true);
                // 入庫単価
                results[0].UnitPrice = TMQUtil.roundDigit(results[0].UnitPrice, results[0].CurrencyDigit, results[0].CurrencyRoundDivision);
                results[0].UnitPrice = TMQUtil.CombineNumberAndUnit(results[0].UnitPrice, results[0].CurrencyName, true);
                // 入庫金額
                results[0].StorageAmount = TMQUtil.roundDigit(results[0].StorageAmount, results[0].CurrencyDigit, results[0].CurrencyRoundDivision);
                results[0].StorageAmount = TMQUtil.CombineNumberAndUnit(results[0].StorageAmount, results[0].CurrencyName, true);

                bool flgCarryData = results[0].Flg == carryData;

                // 在庫数
                results[0].StockQuantityCalc = results[0].InoutQuantityCalc;
                if (!flgCarryData)
                {
                    results[0].StockQuantity = results[0].StockQuantityCalc.ToString();
                }
                results[0].StockQuantity = TMQUtil.roundDigit(results[0].StockQuantity, results[0].UnitDigit, results[0].UnitRoundDivision);
                results[0].StockQuantity = TMQUtil.CombineNumberAndUnit(results[0].StockQuantity, results[0].UnitName, true);
                // 在庫金額
                results[0].StockAmountCalc = results[0].StorageAmountCalc;
                if (!flgCarryData)
                {
                    results[0].StockAmount = results[0].StockAmountCalc.ToString();
                }
                results[0].StockAmount = TMQUtil.roundDigit(results[0].StockAmount, results[0].CurrencyDigit, results[0].CurrencyRoundDivision);
                results[0].StockAmount = TMQUtil.CombineNumberAndUnit(results[0].StockAmount, results[0].CurrencyName, true);

                // 丸め処理・数量と単位を結合する(値を取得している場合)
                // 繰越のデータは含めないので「i」は1からスタート
                for (int i = 1; i < results.Count; i++)
                {
                    // 入庫数
                    if (!string.IsNullOrEmpty(results[i].InoutQuantity))
                    {
                        results[i].InoutQuantity = TMQUtil.roundDigit(results[i].InoutQuantity, results[i].UnitDigit, results[i].UnitRoundDivision);
                        results[i].InoutQuantity = TMQUtil.CombineNumberAndUnit(results[i].InoutQuantity, results[i].UnitName, true);
                    }
                    // 入庫単価
                    if (!string.IsNullOrEmpty(results[i].UnitPrice))
                    {
                        results[i].UnitPrice = TMQUtil.roundDigit(results[i].UnitPrice, results[i].CurrencyDigit, results[i].CurrencyRoundDivision);
                        results[i].UnitPrice = TMQUtil.CombineNumberAndUnit(results[i].UnitPrice, results[i].CurrencyName, true);
                    }

                    // 入庫金額
                    if (!string.IsNullOrEmpty(results[i].StorageAmount))
                    {
                        results[i].StorageAmount = TMQUtil.roundDigit(results[i].StorageAmount, results[i].CurrencyDigit, results[i].CurrencyRoundDivision);
                        results[i].StorageAmount = TMQUtil.CombineNumberAndUnit(results[i].StorageAmount, results[i].CurrencyName, true);
                    }

                    // 出庫数
                    if (!string.IsNullOrEmpty(results[i].ShippingQuantity))
                    {
                        results[i].ShippingQuantity = TMQUtil.roundDigit(results[i].ShippingQuantity, results[i].UnitDigit, results[i].UnitRoundDivision);
                        results[i].ShippingQuantity = TMQUtil.CombineNumberAndUnit(results[i].ShippingQuantity, results[i].UnitName, true);
                    }

                    // 出庫金額
                    if (!string.IsNullOrEmpty(results[i].ShippingAmount))
                    {
                        results[i].ShippingAmount = TMQUtil.roundDigit(results[i].ShippingAmount, results[i].CurrencyDigit, results[i].CurrencyRoundDivision);
                        results[i].ShippingAmount = TMQUtil.CombineNumberAndUnit(results[i].ShippingAmount, results[i].CurrencyName, true);
                    }

                    // 在庫数
                    results[i].StockQuantityCalc = results[i - 1].StockQuantityCalc + results[i].InoutQuantityCalc - results[i].ShippingQuantityCalc;
                    results[i].StockQuantity = TMQUtil.roundDigit(results[i].StockQuantityCalc.ToString(), results[i].UnitDigit, results[i].UnitRoundDivision);
                    results[i].StockQuantity = TMQUtil.CombineNumberAndUnit(results[i].StockQuantity, results[i].UnitName, true);

                    // 在庫金額
                    results[i].StockAmountCalc = results[i - 1].StockAmountCalc + results[i].StorageAmountCalc - results[i].ShippingAmountCalc;
                    results[i].StockAmount = TMQUtil.roundDigit(results[i].StockAmountCalc.ToString(), results[i].CurrencyDigit, results[i].CurrencyRoundDivision);
                    results[i].StockAmount = TMQUtil.CombineNumberAndUnit(results[i].StockAmount, results[i].CurrencyName, true);

                }
            }

        }

        /// <summary>
        /// ラベル出力画面 勘定科目一覧
        /// </summary>
        public class labelSubjectList : ComDao.PtPartsEntity
        {
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string ItemExtensionData1 { get; set; }
            /// <summary>Gets or sets 勘定科目</summary>
            /// <value>勘定科目</value>
            public string SubjectNm { get; set; }
        }

        /// <summary>
        /// ラベル出力画面 部門一覧
        /// </summary>
        public class labelDepartmentList : ComDao.PtPartsEntity
        {
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string ItemExtensionData1 { get; set; }
            /// <summary>Gets or sets 部門</summary>
            /// <value>部門</value>
            public string DepartmentNm { get; set; }

        }

        /// <summary>
        /// ラベル出力画面 ラベル用データ作成用
        /// </summary>
        public class labelData : ComDao.PtPartsEntity
        {
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string Maker { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCode { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string SubjectCode { get; set; }
            /// <summary>Gets or sets 標準棚番</summary>
            /// <value>標準棚番</value>
            public string ShedName { get; set; }
            /// <summary>Gets or sets QRコード</summary>
            /// <value>QRコード</value>
            public string Qrc { get; set; }
            /// <summary>Gets or sets 管理工場ID</summary>
            /// <value>管理工場ID</value>
            public int PartsFactoryId { get; set; }
            /// <summary>Gets or sets 階層番号</summary>
            /// <value>階層番号</value>
            public int StructureLayerNo { get; set; }
        }

        /// <summary>
        /// ExcelPort 予備品仕様のデータクラス
        /// </summary>
        public class excelPortPartsList : ComDao.PtPartsEntity
        {
            /// <summary>Gets or sets 予備品№</summary>
            /// <value>予備品№</value>
            public string PartsNoBefore { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            /// <summary>Gets or sets 管理工場ID</summary>
            /// <value>管理工場ID</value>
            public long PartsFactoryId { get; set; }
            /// <summary>Gets or sets 標準部門</summary>
            /// <value>標準部門</value>
            public string DepartmentName { get; set; }
            /// <summary>Gets or sets 標準勘定科目</summary>
            /// <value>標準勘定科目</value>
            public string AccountName { get; set; }

            #region 以下は翻訳
            /// <summary>Gets or sets 管理工場</summary>
            /// <value>管理工場</value>
            public string PartsFactoryName { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public string ManufactureName { get; set; }
            /// <summary>Gets or sets 数量管理単位</summary>
            /// <value>数量管理単位</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 金額管理単位</summary>
            /// <value>金額管理単位</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 標準仕入先</summary>
            /// <value>標準仕入先</value>
            public string VenderName { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public string UseSegmentName { get; set; }

            #region 共通　地区・職種設定用
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            //public int? FactoryId { get; set; }
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
            /// <summary>Gets or sets 倉庫ID</summary>
            /// <value>倉庫ID</value>
            public int? WarehouseId { get; set; }
            /// <summary>Gets or sets 倉庫名称</summary>
            /// <value>倉庫名称</value>
            public string WarehouseName { get; set; }
            /// <summary>Gets or sets 棚番ID</summary>
            /// <value>棚番ID</value>
            public int? RackId { get; set; }
            /// <summary>Gets or sets 棚番名称</summary>
            /// <value>棚番名称</value>
            public string RackName { get; set; }
            #endregion
            #endregion
        }

        /// <summary>
        /// 一覧画面 RFタグ出力情報
        /// </summary>
        public class rftagFileInfo
        {
            /// <summary>Gets or sets RFタグ管理ID</summary>
            /// <value>RFタグ管理ID</value>
            public string RftagId { get; set; }
            /// <summary>Gets or sets ISO識別子</summary>
            /// <value>ISO識別子</value>
            public string IsoIdentifier { get; set; }
            /// <summary>Gets or sets 発番機関</summary>
            /// <value>発番機関</value>
            public string IssuingAgency { get; set; }
            /// <summary>Gets or sets 起動コード：登録No</summary>
            /// <value>起動コード：登録No</value>
            public string SymbolicCode { get; set; }
            /// <summary>Gets or sets 工場</summary>
            /// <value>工場</value>
            public string Factory { get; set; }
            /// <summary>Gets or sets シリアルNo</summary>
            /// <value>シリアルNo</value>
            public string SerialNo { get; set; }
            /// <summary>Gets or sets CD</summary>
            /// <value>CD</value>
            public string Cd { get; set; }
            /// <summary>Gets or sets 予備品No</summary>
            /// <value>予備品No</value>
            public string PartsNo { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long? DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCode { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long? AccountStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string AccountCode { get; set; }
            /// <summary>Gets or sets 読取日時(yyyyMMddHHmmssfff)</summary>
            /// <value>読取日時(yyyyMMddHHmmssfff)</value>
            public string ReadDatetimeStr { get; set; }
            /// <summary>Gets or sets 読取日時</summary>
            /// <value>読取日時</value>
            public DateTime ReadDatetime { get; set; }
        }

        /// <summary>
        /// 取込画面のデータクラス
        /// </summary>
        public class uploadInfo : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets エラー内容</summary>
            /// <value>エラー内容</value>
            public string ErrorMessage { get; set; }
            /// <summary>Gets or sets 取込画面を閉じない場合（エラー等ある場合）true</summary>
            /// <value>取込画面を閉じない場合（エラー等ある場合）true</value>
            public bool Flg { get; set; }
        }

        /// <summary>
        /// RFIDタグ一覧
        /// </summary>
        public class rftagList : ComDao.PtRftagPartsLinkEntity
        {
            /// <summary>Gets or sets 部門名(コード+名称)</summary>
            /// <value>部門名(コード+名称)</value>
            public string DepartmentName { get; set; }
            /// <summary>Gets or sets 勘定科目名(コード+名称)</summary>
            /// <value>勘定科目名(コード+名称)</value>
            public string AccountName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCode { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string AccountCode { get; set; }
            /// <summary>Gets or sets RFタグ管理ID(変更前)</summary>
            /// <value>RFタグ管理ID(変更前)</value>
            public string RftagIdBefore { get; set; }
            /// <summary>Gets or sets 更新日時(排他チェック用)</summary>
            /// <value>更新日時(排他チェック用)</value>
            public DateTime SavedUpdateDatetime { get; set; }
            /// <summary>Gets or sets 部門ID(変更前)</summary>
            /// <value>部門ID(変更前)</value>
            public long DepartmentStructureIdBefore { get; set; }
            /// <summary>Gets or sets 勘定科目ID(変更前)</summary>
            /// <value>勘定科目ID(変更前)</value>
            public long AccountStructureIdBefore { get; set; }
        }

        /// <summary>
        /// 予備品の標準情報
        /// </summary>
        public class defaultPartsInfo
        {
            /// <summary>Gets or sets 標準部門コード</summary>
            /// <value>標準部門コード</value>
            public string DefaultDepartmentCode { get; set; }
            /// <summary>Gets or sets 標準勘定科目コード</summary>
            /// <value>標準勘定科目コード</value>
            public string DefaultAccountCode { get; set; }
        }
    }
}
