using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using BaseDao = CommonSTDUtil.CommonDataBaseClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using DBManager = CommonSTDUtil.CommonDBManager;

namespace BusinessLogic_PT0007
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_PT0007
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : BaseDao.SearchCommonClass
        {
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public long WorkNo { get; set; }
            /// <summary>Gets or sets　新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public long OldNewStructureId { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 画面遷移フラグ(0:新規,1:修正)</summary>
            /// <value>画面遷移フラグ(0:新規,1:修正)</value>
            public int TransFlg { get; set; }
            /// <summary>Gets or sets 画面遷移タイプ(2:棚番,3:部門)</summary>
            /// <value>画面遷移タイプ(2:棚番,3:部門)</value>
            public int TransType { get; set; }
            /// <summary>Gets or sets ユーザーの本務工場</summary>
            /// <value>ユーザーの本務工場</value>
            public int UserFactoryId { get; set; }
        }

        /// <summary>
        /// 予備品情報のデータクラス
        /// </summary>
        public class partsInfo : ComDao.PtPartsEntity
        {
            /// <summary>Gets or sets 最新在庫数</summary>
            /// <value>最新在庫数</value>
            public string StockQuantity { get; set; }
            /// <summary>Gets or sets 数量管理単位(名称)</summary>
            /// <value>数量管理単位(名称)</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 小数点以下桁数(数量)</summary>
            /// <value>小数点以下桁数(金額)</value>
            public int UnitDigit { get; set; }
            /// <summary>Gets or sets 丸め処理区分(数量)</summary>
            /// <value>丸め処理区分(数量)</value>
            public int UnitRoundDivision { get; set; }
            /// <summary>Gets or sets 画面遷移フラグ(0:新規,1:修正)</summary>
            /// <value>画面遷移フラグ(0:新規,1:修正)</value>
            public int TransFlg { get; set; }
            /// <summary>Gets or sets 画面遷移タイプ(2:棚番,3:部門)</summary>
            /// <value>画面遷移タイプ(2:棚番,3:部門)</value>
            public int TransType { get; set; }
            /// <summary>Gets or sets ボタン表示(2:棚番,3:部門)</summary>
            /// <value>ボタン表示(0:表示,1:非表示)</value>
            public int BtnVisible { get; set; }
            /// <summary>Gets or sets タブ選択フラグ</summary>
            /// <value>タブ選択フラグ</value>
            public int TabFirstSelect { get; set; }
            /// <summary>Gets or sets 棚別在庫一覧の件数</summary>
            /// <value>棚別在庫一覧の件数</value>
            public int LocationDataCnt { get; set; }
            /// <summary>Gets or sets 部門別在庫一覧の件数</summary>
            /// <value>部門別在庫一覧の件数</value>
            public int DepartmentDataCnt { get; set; }
            /// <summary>
            /// 丸め処理・数量と単位の結合
            /// </summary>
            public void JoinStrAndRound()
            {
                // 在庫数
                this.StockQuantity = TMQUtil.roundDigit(this.StockQuantity, this.UnitDigit, this.UnitRoundDivision);
                this.StockQuantity = TMQUtil.CombineNumberAndUnit(this.StockQuantity, this.UnitName, true);
            }
        }

        /// <summary>
        /// 棚別在庫一覧のデータクラス
        /// </summary>
        public class locationList
        {
            /// <summary>Gets or sets 入庫日</summary>
            /// <value>入庫日</value>
            public DateTime ReceivingDatetime { get; set; }
            /// <summary>Gets or sets ロット№</summary>
            /// <value>ロット№</value>
            public long LotNo { get; set; }
            /// <summary>Gets or sets 棚番</summary>
            /// <value>棚番</value>
            public string LocationNm { get; set; }
            /// <summary>Gets or sets 枝番</summary>
            /// <value>枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int PartsFactoryId { get; set; }
            /// <summary>Gets or sets 新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public long OldNewStructureId { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public string UnitPrice { get; set; }
            /// <summary>Gets or sets 金額単位名称</summary>
            /// <value>金額単位名称</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public string StockQuantity { get; set; }
            /// <summary>Gets or sets 数量単位名称</summary>
            /// <value>数量単位名称</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 部門</summary>
            /// <value>部門</value>
            public string DepartmentNm { get; set; }
            /// <summary>Gets or sets 勘定科目</summary>
            /// <value>勘定科目</value>
            public string SubjectNm { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 管理№</summary>
            /// <value>管理№</value>
            public string ManagementNo { get; set; }
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
            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public int PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚番コード</summary>
            /// <value>棚番コード</value>
            public string PartsLocationCode { get; set; }
            /// <summary>Gets or sets 結合文字列</summary>
            /// <value>結合文字列</value>
            public string JoinString { get; set; }
            /// <summary>Gets or sets 入庫単価(単位なし)</summary>
            /// <value>入庫単価(単位なし)</value>
            public string UnitPriceExeptUnit { get; set; }
            /// <summary>Gets or sets 在庫数(単位なし)</summary>
            /// <value>在庫数(単位なし)</value>
            public string StockQuantityExeptUnit { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentId { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long AccountStructureId { get; set; }
            /// <summary>Gets or sets ロット管理id</summary>
            /// <value>ロット管理id</value>
            public long LotControlId { get; set; }
            /// <summary>Gets or sets 在庫管理id</summary>
            /// <value>在庫管理id</value>
            public long InventoryControlId { get; set; }

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

            /// <summary>
            /// 丸め処理・数量と単位の結合
            /// </summary>
            /// <param name="languageId">言語ID</param>
            /// <param name="db">DBクラス</param>
            public void JoinStrAndRound(string languageId, DBManager.CommonDBManager db)
            {
                this.UnitPrice = TMQUtil.roundDigit(this.UnitPrice, this.CurrencyDigit, this.CurrencyRoundDivision);           // 入庫単価
                this.UnitPriceExeptUnit = TMQUtil.roundDigit(this.UnitPrice, this.CurrencyDigit, this.CurrencyRoundDivision, true);  // 入庫単価(単位なし)
                this.StockQuantity = TMQUtil.roundDigit(this.StockQuantity, this.UnitDigit, this.UnitRoundDivision);           // 在庫数
                this.StockQuantityExeptUnit = TMQUtil.roundDigit(this.StockQuantity, this.UnitDigit, this.UnitRoundDivision, true);  // 在庫数(単位なし)

                this.UnitPrice = TMQUtil.CombineNumberAndUnit(this.UnitPrice, this.CurrencyName, true);     // 入庫単価
                this.StockQuantity = TMQUtil.CombineNumberAndUnit(this.StockQuantity, this.UnitName, true); // 在庫数

                // 結合文字列を取得
                string joinStr = TMQUtil.GetJoinStrOfPartsLocation(this.PartsFactoryId, languageId, db);
                this.JoinString = joinStr;

                // 棚番と枝番を結合
                if (!string.IsNullOrEmpty(this.LocationNm) && !string.IsNullOrEmpty(this.PartsLocationDetailNo))
                {
                    this.LocationNm = TMQUtil.GetDisplayPartsLocation(this.LocationNm, this.PartsLocationDetailNo, joinStr);
                }
            }
        }

        /// <summary>
        /// 移庫先情報(棚別)のデータクラス
        /// </summary>
        public class locationInfoTo : BaseDao.CommonTableItem
        {
            /// <summary>Gets or sets 移庫日</summary>
            /// <value>移庫日</value>
            public DateTime RelocationDate { get; set; }
            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public int PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚のコード(オートコンプリート用)</summary>
            /// <value>棚のコード(オートコンプリート用)</value>
            public string PartsLocationCode { get; set; }
            /// <summary>Gets or sets 枝番</summary>
            /// <value>枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 結合文字列</summary>
            /// <value>結合文字列</value>
            public string JoinString { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int PartsFactoryId { get; set; }
            /// <summary>Gets or sets 移庫数</summary>
            /// <value>移庫数</value>
            public decimal TransferCount { get; set; }
            /// <summary>Gets or sets 数量単位名称</summary>
            /// <value>数量単位名称</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 移庫金額</summary>
            /// <value>移庫金額</value>
            public string TransferAmount { get; set; }
            /// <summary>Gets or sets 金額単位名称</summary>
            /// <value>金額単位名称</value>
            public string CurrencyName { get; set; }
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
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public decimal UnitPrice { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public decimal StockQuantityExeptUnit { get; set; }
            /// <summary>Gets or sets ロット管理id</summary>
            /// <value>ロット管理id</value>
            public long LotControlId { get; set; }
            /// <summary>Gets or sets 在庫管理id</summary>
            /// <value>在庫管理id</value>
            public long InventoryControlId { get; set; }
            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public long WorkNo { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentId { get; set; }
            /// <summary>Gets or sets 棚ID(変更前)</summary>
            /// <value>棚ID(変更前)</value>
            public int PartsLocationIdBefore { get; set; }
            /// <summary>Gets or sets 枝番(変更前)</summary>
            /// <value>枝番(変更前)</value>
            public string DetailNoBefore { get; set; }
            /// <summary>Gets or sets 更新シリアルID</summary>
            /// <value>更新シリアルID</value>
            public int UpdateSerialid { get; set; }
            /// <summary>Gets or sets 新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public long OldNewStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long AccountStructureId { get; set; }
            /// <summary>Gets or sets ユーザーの本務工場</summary>
            /// <value>ユーザーの本務工場</value>
            public int UserFactoryId { get; set; }

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
            /// 丸め処理・数量と単位の結合
            /// </summary>
            public void JoinStrAndRound()
            {
                this.TransferCount = decimal.Parse(TMQUtil.roundDigit(this.TransferCount.ToString(), this.UnitDigit, this.UnitRoundDivision));                   // 移庫数
                this.TransferAmount = TMQUtil.roundDigit(this.TransferAmount, this.CurrencyDigit, this.CurrencyRoundDivision);                                   // 移庫金額
                this.UnitPrice = decimal.Parse(TMQUtil.roundDigit(this.UnitPrice.ToString(), this.CurrencyDigit, this.CurrencyRoundDivision));                   // 入庫単価
                this.StockQuantityExeptUnit = decimal.Parse(TMQUtil.roundDigit(this.StockQuantityExeptUnit.ToString(), this.UnitDigit, this.UnitRoundDivision)); // 在庫数

                this.TransferAmount = TMQUtil.CombineNumberAndUnit(this.TransferAmount, this.CurrencyName, true); // 移庫金額

                this.PartsLocationIdBefore = this.PartsLocationId; // 棚番(変更前)
                this.DetailNoBefore = this.PartsLocationDetailNo;  // 枝番(変更前)
            }
        }

        /// <summary>
        /// 部門別在庫一覧のデータクラス
        /// </summary>
        public class departmentList
        {
            /// <summary>Gets or sets 入庫日</summary>
            /// <value>入庫日</value>
            public DateTime ReceivingDatetime { get; set; }
            /// <summary>Gets or sets ロット№</summary>
            /// <value>ロット№</value>
            public long LotNo { get; set; }
            /// <summary>Gets or sets 新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public long OldNewStructureId { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public string UnitPrice { get; set; }
            /// <summary>Gets or sets 金額単位名称</summary>
            /// <value>金額単位名称</value>
            public string CurrencyName { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public string StockQuantity { get; set; }
            /// <summary>Gets or sets 数量単位名称</summary>
            /// <value>数量単位名称</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 部門</summary>
            /// <value>部門</value>
            public string DepartmentNm { get; set; }
            /// <summary>Gets or sets 勘定科目</summary>
            /// <value>勘定科目</value>
            public string SubjectNm { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 管理№</summary>
            /// <value>管理№</value>
            public string ManagementNo { get; set; }
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
            /// <summary>Gets or sets 入庫単価(単位なし)</summary>
            /// <value>入庫単価(単位なし)</value>
            public string UnitPriceExeptUnit { get; set; }
            /// <summary>Gets or sets 在庫数(単位なし)</summary>
            /// <value>在庫数(単位なし)</value>
            public string StockQuantityExeptUnit { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public string DepartmentId { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCode { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public string SubjectId { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string SubjectCode { get; set; }
            /// <summary>Gets or sets 勘定科目翻訳</summary>
            /// <value>勘定科目翻訳</value>
            public string SubjectTranslation { get; set; }
            /// <summary>Gets or sets 部門No</summary>
            /// <value>部門No</value>
            public int DepartmentFlg { get; set; }
            /// <summary>Gets or sets ロット管理id</summary>
            /// <value>ロット管理id</value>
            public long LotControlId { get; set; }
            /// <summary>Gets or sets 在庫管理id</summary>
            /// <value>在庫管理id</value>
            public string InventoryControlId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int PartsFactoryId { get; set; }

            /// <summary>
            /// 丸め処理・数量と単位の結合
            /// </summary>
            /// <param name="languageId">言語ID</param>
            /// <param name="db">DBクラス</param>
            public void JoinStrAndRound()
            {
                this.UnitPrice = TMQUtil.roundDigit(this.UnitPrice, this.CurrencyDigit, this.CurrencyRoundDivision);           // 入庫単価
                this.UnitPriceExeptUnit = TMQUtil.roundDigit(this.UnitPrice, this.CurrencyDigit, this.CurrencyRoundDivision, true);  // 入庫単価(単位なし)
                this.StockQuantity = TMQUtil.roundDigit(this.StockQuantity, this.UnitDigit, this.UnitRoundDivision);           // 在庫数
                this.StockQuantityExeptUnit = TMQUtil.roundDigit(this.StockQuantity, this.UnitDigit, this.UnitRoundDivision, true);  // 在庫数(単位なし)

                this.UnitPrice = TMQUtil.CombineNumberAndUnit(this.UnitPrice, this.CurrencyName, true);     // 入庫単価
                this.StockQuantity = TMQUtil.CombineNumberAndUnit(this.StockQuantity, this.UnitName, true); // 在庫数
            }
        }

        /// <summary>
        /// 移庫先情報(部門別)のデータクラス
        /// </summary>
        public class departmentInfoTo : BaseDao.CommonTableItem
        {
            /// <summary>Gets or sets 移庫日</summary>
            /// <value>移庫日</value>
            public DateTime RelocationDate { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public decimal UnitPrice { get; set; }
            /// <summary>Gets or sets 移庫数</summary>
            /// <value>移庫数</value>
            public string TransferCount { get; set; }
            /// <summary>Gets or sets 移庫金額</summary>
            /// <value>移庫金額</value>
            public string TransferAmount { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentId { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCode { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long SubjectId { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string SubjectCode { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 管理No.</summary>
            /// <value>管理No.</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 数量単位名称</summary>
            /// <value>数量単位名称</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets 金額単位名称</summary>
            /// <value>金額単位名称</value>
            public string CurrencyName { get; set; }
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
            /// <summary>Gets or sets 部門No</summary>
            /// <value>部門No</value>
            public int DepartmentFlg { get; set; }
            /// <summary>Gets or sets 移庫数(単位なし)</summary>
            /// <value>移庫数(単位なし)</value>
            public string TransferCountExeptUnit { get; set; }
            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public long WorkNo { get; set; }
            /// <summary>Gets or sets ロット管理id</summary>
            /// <value>ロット管理id</value>
            public long LotControlId { get; set; }
            /// <summary>Gets or sets 入庫単価(変更前)</summary>
            /// <value>入庫単価(変更前)</value>
            public decimal UnitPriceBefore { get; set; }
            /// <summary>Gets or sets　新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public long OldNewStructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int PartsFactoryId { get; set; }
            /// <summary>Gets or sets 更新シリアルID</summary>
            /// <value>更新シリアルID</value>
            public int UpdateSerialid { get; set; }
            /// <summary>Gets or sets 部門ID(移庫元)</summary>
            /// <value>部門ID(移庫元)</value>
            public long DepartmentIdBefore { get; set; }
            /// <summary>Gets or sets 勘定科目ID(移庫元)</summary>
            /// <value>勘定科目ID(移庫元)</value>
            public long AccountIdBefore { get; set; }
            /// <summary>Gets or sets ユーザーの本務工場</summary>
            /// <value>ユーザーの本務工場</value>
            public int UserFactoryId { get; set; }

            /// <summary>
            /// 丸め処理・数量と単位の結合
            /// </summary>
            /// <param name="workNo">作業No</param>
            public void JoinStrAndRound(long workNo)
            {
                this.UnitPrice = decimal.Parse(TMQUtil.roundDigit(this.UnitPrice.ToString(), this.CurrencyDigit, this.CurrencyRoundDivision));       // 入庫単価
                this.UnitPriceBefore = decimal.Parse(TMQUtil.roundDigit(this.UnitPrice.ToString(), this.CurrencyDigit, this.CurrencyRoundDivision)); // 入庫単価(変更前)
                this.TransferCount = TMQUtil.roundDigit(this.TransferCount, this.UnitDigit, this.UnitRoundDivision);                                 // 移庫数
                this.TransferCountExeptUnit = TMQUtil.roundDigit(this.TransferCount, this.UnitDigit, this.UnitRoundDivision);                        // 移庫数
                this.TransferAmount = TMQUtil.roundDigit(this.TransferAmount, this.CurrencyDigit, this.CurrencyRoundDivision);                       // 移庫金額

                this.TransferCount = TMQUtil.CombineNumberAndUnit(this.TransferCount.ToString(), this.UnitName, true); // 移庫数
                this.TransferAmount = TMQUtil.CombineNumberAndUnit(this.TransferAmount, this.CurrencyName, true);      // 移庫金額

                this.WorkNo = workNo; // 作業No.
            }
        }

        /// <summary>
        /// 棚卸データを取得するデータクラス
        /// </summary>
        public class historyData : ComDao.PtInoutHistoryEntity
        {
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
        }

        /// <summary>
        /// 棚卸確定日取得条件
        /// </summary>
        public class getInventoryDate : ComDao.PtLotEntity
        {
            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public int PartsLocationId { get; set; }
            /// <summary>Gets or sets 枝番</summary>
            /// <value>枝番</value>
            public string PartsLocationDetailNo { get; set; }
        }
    }
}
