using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPUtil.APInventoryUtil
{
    /// <summary>
    /// 在庫更新にて使用するデータクラス
    /// </summary>
    public class InventoryDataClass
    {

        /// <summary>
        /// 結果情報（在庫更新戻り値)
        /// </summary>
        public class InventoryReturnInfo
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InventoryReturnInfo()
            {
                ResultValue = true;
                Message = string.Empty;
            }

            /// <summary>
            /// コンストラクタ(ﾊﾟﾗﾒｰﾀあり)
            /// </summary>
            /// <param name="paramResult">結果</param>
            /// <param name="paramMessage">エラーメッセージ</param>
            public InventoryReturnInfo(bool paramResult, string paramMessage)
            {
                ResultValue = paramResult;
                Message = paramMessage;
            }

            /// <summary>Gets or sets a value indicating whether 結果</summary>
            /// <value>True:正常 false:異常</value>
            public bool ResultValue { get; set; }
            /// <summary>Gets or sets エラーメッセージ</summary>
            /// <value>正常時：空白　異常時：エラーメッセージ</value>
            public string Message { get; set; }
        }

        /// <summary>
        /// 在庫更新用パラメータデータクラス
        /// </summary>
        public class InventoryParameter
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InventoryParameter()
            {
                //初期値設定
                OrderDivision = 0;
                OrderNo = "0";
                OrderLineNo1 = 0;
                OrderLineNo2 = 0;
                ResultOrderDivision = 0;
                ResultOrderNo = "0";
                ResultOrderLineNo1 = 0;
                ResultOrderLineNo2 = 0;
                LotNo = "*";
                SubLotNo1 = "*";
                SubLotNo2 = "*";
                InoutQty = 0;
                AssignFlg = 2;
                OverFlg = 0;
                CompleteFlg = 0;
                PlanInoutQty = 0;
                TransferInoutQty = 0;
            }

            /// <summary>
            /// コンストラクタ(コピー機能)
            /// </summary>
            /// <param name="paramData">在庫更新要パラメータ</param>
            public InventoryParameter(InventoryParameter paramData)
            {
                //初期値設定
                ProcessDivision = paramData.ProcessDivision;
                InoutDivision = paramData.InoutDivision;
                AccountYears = paramData.AccountYears;
                InoutDate = paramData.InoutDate;
                OrderDivision = paramData.OrderDivision;
                OrderNo = paramData.OrderNo;
                OrderLineNo1 = paramData.OrderLineNo1;
                OrderLineNo2 = paramData.OrderLineNo2;
                ResultOrderDivision = paramData.ResultOrderDivision;
                ResultOrderNo = paramData.ResultOrderNo;
                ResultOrderLineNo1 = paramData.ResultOrderLineNo1;
                ResultOrderLineNo2 = paramData.ResultOrderLineNo2;
                ItemCd = paramData.ItemCd;
                SpecificationCd = paramData.SpecificationCd;
                LocationCd = paramData.LocationCd;
                LotNo = paramData.LotNo;
                SubLotNo1 = paramData.SubLotNo1;
                SubLotNo2 = paramData.SubLotNo2;
                InoutQty = paramData.InoutQty;
                ReferenceNo = paramData.ReferenceNo;
                ReferenceLineNo = paramData.ReferenceLineNo;
                AssignFlg = 1;
                OverFlg = 0;
                InputMenuId = paramData.InputMenuId;
                InputDisplayId = paramData.InputDisplayId;
                InputControlId = paramData.InputControlId;
                CompleteFlg = paramData.CompleteFlg;
                Remark = paramData.Remark;
                RyCd = paramData.RyCd;
                Reason = paramData.Reason;
                PlanInoutQty = paramData.PlanInoutQty;
                BeforeLocationCd = paramData.BeforeLocationCd;
                TransferItemCd = paramData.TransferItemCd;
                TransferSpecificationCd = paramData.TransferSpecificationCd;
                TransferLotNo = paramData.TransferLotNo;
                TransferSubLotNo1 = paramData.TransferSubLotNo1;
                TransferSubLotNo2 = paramData.TransferSubLotNo2;
                TransferInoutQty = paramData.TransferInoutQty;
                UserId = paramData.UserId;
                InoutSourceNo = paramData.InoutSourceNo;
                StockDivision = paramData.StockDivision;
                ExtendedString1 = paramData.ExtendedString1;
                ExtendedString2 = paramData.ExtendedString2;
                ExtendedString3 = paramData.ExtendedString3;
                ExtendedString4 = paramData.ExtendedString4;
                ExtendedString5 = paramData.ExtendedString5;
                ExtendedNumber1 = paramData.ExtendedNumber1;
                ExtendedNumber2 = paramData.ExtendedNumber2;
                ExtendedNumber3 = paramData.ExtendedNumber3;
                ExtendedNumber4 = paramData.ExtendedNumber4;
                ExtendedNumber5 = paramData.ExtendedNumber5;
                ExtendedDateTime1 = paramData.ExtendedDateTime1;
                ExtendedDateTime2 = paramData.ExtendedDateTime2;
                ExtendedDateTime3 = paramData.ExtendedDateTime3;
                ExtendedDateTime4 = paramData.ExtendedDateTime4;
                ExtendedDateTime5 = paramData.ExtendedDateTime5;
            }

            /// <summary>Gets or sets  処理区分</summary>
            /// <value>処理区分</value>
            public int ProcessDivision { get; set; }
            /// <summary>Gets or sets 受払区分|各種名称マスタ</summary>
            /// <value>受払区分|各種名称マスタ</value>
            public string InoutDivision { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 受払予定日</summary>
            /// <value>受払予定日</value>
            public DateTime? InoutDate { get; set; }
            /// <summary>Gets or sets オーダー区分</summary>
            /// <value>オーダー区分</value>
            public int? OrderDivision { get; set; }
            /// <summary>Gets or sets オーダー番号</summary>
            /// <value>オーダー番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets オーダー行番号1</summary>
            /// <value>オーダー行番号1</value>
            public int OrderLineNo1 { get; set; }
            /// <summary>Gets or sets オーダー行番号2</summary>
            /// <value>オーダー行番号2</value>
            public int? OrderLineNo2 { get; set; }
            /// <summary>Gets or sets 伝票区分</summary>
            /// <value>伝票区分</value>
            public int? ResultOrderDivision { get; set; }
            /// <summary>Gets or sets 伝票番号</summary>
            /// <value>伝票番号</value>
            public string ResultOrderNo { get; set; }
            /// <summary>Gets or sets 伝票行番号1</summary>
            /// <value>伝票行番号1</value>
            public int ResultOrderLineNo1 { get; set; }
            /// <summary>Gets or sets 伝票行番号2</summary>
            /// <value>伝票行番号2</value>
            public int? ResultOrderLineNo2 { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>サブロット番号1</value>
            public string SubLotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>サブロット番号2</value>
            public string SubLotNo2 { get; set; }
            /// <summary>Gets or sets 受払数</summary>
            /// <value>受払数</value>
            public decimal? InoutQty { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }
            /// <summary>Gets or sets リファレンス行番号</summary>
            /// <value>リファレンス行番号</value>
            public int? ReferenceLineNo { get; set; }
            /// <summary>Gets or sets 引当済フラグ|1</summary>
            /// <value>引当済フラグ</value>
            public int? AssignFlg { get; set; }
            /// <summary>Gets or sets 過剰フラグ|引当数量以上に処理した場合</summary>
            /// <value>過剰フラグ|引当数量以上に処理した場合</value>
            public int? OverFlg { get; set; }
            /// <summary>Gets or sets プログラムID</summary>
            /// <value>プログラムID</value>
            public string InputMenuId { get; set; }
            /// <summary>Gets or sets コントロールID</summary>
            /// <value>コントロールID</value>
            public string InputDisplayId { get; set; }
            /// <summary>Gets or sets 項目番号</summary>
            /// <value>項目番号</value>
            public int? InputControlId { get; set; }
            /// <summary>Gets or sets 完納フラグ</summary>
            /// <value>完納フラグ</value>
            public int CompleteFlg { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 理由コード</summary>
            /// <value>理由コード</value>
            public string RyCd { get; set; }
            /// <summary>Gets or sets 理由</summary>
            /// <value>理由</value>
            public string Reason { get; set; }
            /// <summary>Gets or sets オーダー番号の受払予定数</summary>
            /// <value>オーダー番号の受払予定数</value>
            public decimal? PlanInoutQty { get; set; }
            /// <summary>Gets or sets 変更前ロケーションコード/移動元ロケーションコード</summary>
            /// <value>変更前ロケーションコード/移動元ロケーションコード</value>
            public string BeforeLocationCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>振替品目コード</value>
            public string TransferItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>振替仕様コード</value>
            public string TransferSpecificationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>振替ロット番号</value>
            public string TransferLotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>振替サブロット番号1</value>
            public string TransferSubLotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>振替サブロット番号2</value>
            public string TransferSubLotNo2 { get; set; }
            /// <summary>Gets or sets 振替受払数</summary>
            /// <value>振替受払数</value>
            public decimal? TransferInoutQty { get; set; }
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 受払ソース番号</summary>
            /// <value>受払ソース番号</value>
            public string InoutSourceNo { get; set; }
            /// <summary>Gets or sets 在庫管理区分</summary>
            /// <value>在庫管理区分</value>
            public int? StockDivision { get; set; }
            /// <summary>Gets or sets 拡張文字列１</summary>
            /// <value>拡張文字列１</value>
            public string ExtendedString1 { get; set; }
            /// <summary>Gets or sets 拡張文字列２</summary>
            /// <value>拡張文字列２</value>
            public string ExtendedString2 { get; set; }
            /// <summary>Gets or sets 拡張文字列３</summary>
            /// <value>拡張文字列３</value>
            public string ExtendedString3 { get; set; }
            /// <summary>Gets or sets 拡張文字列４</summary>
            /// <value>拡張文字列４</value>
            public string ExtendedString4 { get; set; }
            /// <summary>Gets or sets 拡張文字列５</summary>
            /// <value>拡張文字列５</value>
            public string ExtendedString5 { get; set; }
            /// <summary>Gets or sets 拡張数値１</summary>
            /// <value>拡張数値１</value>
            public decimal ExtendedNumber1 { get; set; }
            /// <summary>Gets or sets 拡張数値２</summary>
            /// <value>拡張数値２</value>
            public decimal ExtendedNumber2 { get; set; }
            /// <summary>Gets or sets 拡張数値３</summary>
            /// <value>拡張数値３</value>
            public decimal ExtendedNumber3 { get; set; }
            /// <summary>Gets or sets 拡張数値４</summary>
            /// <value>拡張数値４</value>
            public decimal ExtendedNumber4 { get; set; }
            /// <summary>Gets or sets 拡張数値５</summary>
            /// <value>拡張数値５</value>
            public decimal ExtendedNumber5 { get; set; }
            /// <summary>Gets or sets 拡張日付</summary>
            /// <value>拡張日付</value>
            public DateTime? ExtendedDateTime1 { get; set; }
            /// <summary>Gets or sets 拡張日付</summary>
            /// <value>拡張日付</value>
            public DateTime? ExtendedDateTime2 { get; set; }
            /// <summary>Gets or sets 拡張日付</summary>
            /// <value>拡張日付</value>
            public DateTime? ExtendedDateTime3 { get; set; }
            /// <summary>Gets or sets 拡張日付</summary>
            /// <value>拡張日付</value>
            public DateTime? ExtendedDateTime4 { get; set; }
            /// <summary>Gets or sets 拡張日付</summary>
            /// <value>拡張日付</value>
            public DateTime? ExtendedDateTime5 { get; set; }
        }

    }
}
