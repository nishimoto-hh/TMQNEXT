using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

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
            public int UserFactoryId { get; set; }
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
        public class searchResult : ComDao.PtPartsEntity
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
            public int? LeadTimeExceptUnit { get; set; }
            /// <summary>Gets or sets 発注量(単位なし)</summary>
            /// <value>発注量(単位なし)</value>
            public int? OrderQuantityExceptUnit { get; set; }
            /// <summary>Gets or sets 画像</summary>
            /// <value>画像</value>
            public string Image { get; set; }
            /// <summary>Gets or sets 最新在庫数</summary>
            /// <value>最新在庫数</value>
            public string StockQuantity { get; set; }
            /// <summary>Gets or sets 最新在庫数(単位なし)</summary>
            /// <value>最新在庫数(単位なし)</value>
            public int? StockQuantityExceptUnit { get; set; }
            /// <summary>Gets or sets 発注アラーム</summary>
            /// <value>発注アラーム</value>
            public string OrderAlert { get; set; }
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
            public decimal LeadTimeExceptUnit { get; set; }
            /// <summary>Gets or sets 標準単価(定義のコントロールにあわせて作成)</summary>
            /// <value>標準単価</value>
            public decimal UnitPriceExceptUnit { get; set; }
            /// <summary>Gets or sets 発注量(定義のコントロールにあわせて作成)</summary>
            /// <value>発注量</value>
            public decimal OrderQuantityExceptUnit { get; set; }
            /// <summary>Gets or sets 管理工場ID</summary>
            /// <value>管理工場ID</value>
            public long PartsFactoryId { get; set; }
            /// <summary>Gets or sets メーカー名</summary>
            /// <value>メーカー名</value>
            public string ManufactureName { get; set; }
            /// <summary>Gets or sets 仕入先名</summary>
            /// <value>仕入先名</value>
            public string VenderName { get; set; }

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
    }
}
