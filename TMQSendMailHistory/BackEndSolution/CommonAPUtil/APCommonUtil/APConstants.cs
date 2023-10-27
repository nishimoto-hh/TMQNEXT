/// <summary>
/// 機能名　　：　【共通】
/// タイトル　：　AP共通データ定数定義クラス
/// 説明　　　：　AP共通データテーブル内で使用する定数を定義します。
/// 履歴　　　：
/// </summary>

namespace APConstants
{
    /// <summary>
    /// 定数宣言クラス
    /// </summary>
    public class APConstants
    {
        /// <summary>
        /// 共通
        /// </summary>
        public static class COMMON
        {
            /// <summary>時分</summary>
            public const string HourTime = "23:59:00";
        }

        /// <summary>
        /// 機能ID
        /// </summary>
        public static class CONDUCTID
        {
            /// <summary>品目マスタ</summary>
            public const string Item = "BB00060";
            /// <summary>品目分類マスタ</summary>
            public const string ItemCategory = "BB00061";
            /// <summary>工程マスタ</summary>
            public const string Operation = "BB00062";
            /// <summary>工程グループマスタ</summary>
            public const string OperationGroup = "BB00063";
            /// <summary>工程パターンマスタ</summary>
            public const string OperationPattern = "BB00064";
            /// <summary>設備マスタ</summary>
            public const string RecipeResouce = "BB00065";
            /// <summary>設備グループマスタ</summary>
            public const string RecipeResouceGroup = "BB00066";
            /// <summary>生産ラインマスタ</summary>
            public const string Line = "BB00067";
            /// <summary>ロケーションマスタ</summary>
            public const string Location = "BB00068";
            /// <summary>銀行マスタ</summary>
            public const string Bank = "BB00071";
            /// <summary>科目マスタ</summary>
            public const string Accounts = "BB00072";
            /// <summary>取引先マスタ</summary>
            public const string Vender = "BB00074";
            /// <summary>帳合マスタ</summary>
            public const string Balance = "BB00076";
            /// <summary>地区マスタ</summary>
            public const string Area = "BB00078";
            /// <summary>納入先マスタ</summary>
            public const string Delivery = "BB00079";
            /// <summary>運送会社マスタ</summary>
            public const string Carry = "BB00080";
            /// <summary>名称マスタ</summary>
            public const string Names = "BB00083";
            /// <summary>理由マスタ</summary>
            public const string Reason = "BB00084";
            /// <summary>担当者マスタ</summary>
            public const string Login = "BB00090";
            /// <summary>部署マスタ</summary>
            public const string Organization = "BB00091";
            /// <summary>役職マスタ</summary>
            public const string Post = "BB00095";
            /// <summary>ロールマスタ</summary>
            public const string Role = "BB00096";
            /// <summary>多通貨マスタ</summary>
            public const string CurrencyCtlCtl = "BB00106";
        }

        /// <summary>
        /// プルーフステータス
        /// </summary>
        public static class PROOF_STATUS
        {
            /// <summary>Gets 新規登録</summary>
            /// <value>新規登録</value>
            public static decimal NEW
            {
                get
                {
                    return decimal.Zero;
                }
            }
            /// <summary>Gets 更新前</summary>
            /// <value>更新前</value>
            public static decimal PRE_UPDATE
            {
                get
                {
                    return decimal.One;
                }
            }
            /// <summary>Gets 更新後</summary>
            /// <value>更新後</value>
            public static decimal POST_UPDATE
            {
                get
                {
                    return decimal.Parse("2");
                }
            }
            /// <summary>Gets or 削除</summary>
            /// <value>削除</value>
            public static decimal DELETE
            {
                get
                {
                    return decimal.Parse("3");
                }
            }
        }

        /// <summary>
        /// SQL実行結果の明示的な戻り値
        /// </summary>
        /// <remarks>1以上でコミット、-1でロールバック</remarks>
        public static class SQL_RESULT
        {
            /// <summary>
            /// 異常(ロールバック)
            /// </summary>
            public const int ROLLBACK = -1;
            /// <summary>
            /// 正常(コミット)
            /// </summary>
            public const int COMMIT = 1;
        }

        /// <summary>
        /// 明示的な戻り値
        /// </summary>
        /// <remarks>1:正常、-1:異常</remarks>
        public static class RETURN_RESULT
        {
            /// <summary>正常</summary>
            public const int OK = 1;
            /// <summary>異常</summary>
            public const int NG = -1;
        }

        /// <summary>複数選択チェックボックスでの「すべて」の値</summary>
        public const int ALL_VALUE_MULTI_SELECT = 0;

        /// <summary>
        /// 連番取得処理制御マスタの連番処理名称(make_sequence_ctl.proc_name)
        /// </summary>
        public static class SEQUENCE_PROC_NAME
        {
            /// <summary>
            /// 生産計画テーブルの生産計画番号
            /// </summary>
            public const string PRODUCTION_PLAN = "PLAN_NO";
            /// <summary>受注ヘッダテーブルの受注番号</summary>
            public const string ORDER_HEAD = "ORDER_NO";
            /// <summary>
            /// 購入依頼テーブルの購入依頼番号
            /// </summary>
            public const string PURCHASE_REQUEST = "PURCHASE_REQUEST_NO";
            /// <summary>
            /// MRP結果テーブルの計画番号
            /// </summary>
            public const string MRP_RESULT = "MRP_RESULT_PLAN_NO";
            /// <summary>
            /// 発注テーブルの購入依頼番号
            /// </summary>
            public const string PURCHASE = "PURCHASE_NO";
            /// <summary>
            /// 受入テーブルの受入番号
            /// </summary>
            public const string ACCEPTING = "ACCEPTING_NO";
            /// <summary>
            /// 仕入テーブルの仕入番号
            /// </summary>
            public const string STOCKING = "SLIP_NO";
            /// <summary>
            /// 入金テーブルの入金番号
            /// </summary>
            public const string DEPOSIT = "DEPOSIT_NO";
            /// <summary>
            /// ロット番号の連番取得用
            /// </summary>
            public const string LOT_SERIES = "LOT_SERIES_NO";
            /// <summary>
            /// 製造指図番号の連番取得用
            /// </summary>
            public const string DIRECTION_NO = "DIRECTION_NO";
            /// <summary>
            /// 受払ソースの連番取得用
            /// </summary>
            public const string INOUT_SOURCE_NO = "INOUT_SOURCE_NO";
            /// <summary>
            /// 受払履歴の連番取得用
            /// </summary>
            public const string INOUT_RECORD_NO = "INOUT_RECORD_NO";
            /// <summary>
            /// 在庫指図番号の連番取得用
            /// </summary>
            public const string INV_IO_DIRECTION_NO = "INV_IO_DIRECTION_NO";
            /// <summary>
            /// 売上ヘッダ／明細テーブルの売上番号
            /// </summary>
            public const string SALES = "SALES_NO";
            /// <summary>
            /// タスクメモテーブルのタスク番号
            /// </summary>
            public const string TASK_NO = "TASK_NO";
        }

        /// <summary>
        /// チェックフラグ
        /// </summary>
        public static class CHECK_FLG
        {
            /// <summary>チェックあり</summary>
            public const string ON = "1";
            /// <summary>チェックなし</summary>
            public const string OFF = "0";
        }

        /// <summary>
        /// 数値フォーマット関連
        /// </summary>
        public static class NUMBER_CHKDISIT
        {
            /// <summary>チェック項目初期値 整数部桁数</summary>
            public const string INIT_INTEGER_LENGTH = "12";
            /// <summary>チェック項目初期値 小数部桁数</summary>
            public const string INIT_SMALLNUM_LENGTH = "2";
            /// <summary>チェック項目初期値 端数区分</summary>
            public const string INIT_ROUND_DIVISION = "0";
            /// <summary>チェック項目初期値 全体桁数</summary>
            public const string INIT_MAX_LENGTH = "22";
            /// <summary>チェック項目初期値 上限値</summary>
            public const string INIT_UPPER_LIMIT = "99999999999.9999990";
            /// <summary>チェック項目初期値 下限値</summary>
            public const string INIT_LOWER_LIMIT = "0.0000000";
            /// <summary>原価</summary>
            public const string GENKA = "GENKA";
            /// <summary>単価</summary>
            public const string TANKA = "TANKA";
            /// <summary>売上単価</summary>
            public const string URTANKA = "URTANKA";
            /// <summary>仕入単価</summary>
            public const string SITANKA = "SITANKA";
            /// <summary>金額</summary>
            public const string KINGAKU = "KINGAKU";
            /// <summary>売上金額</summary>
            public const string URKINGAKU = "URKINGAKU";
            /// <summary>仕入金額</summary>
            public const string SIKINGAKU = "SIKINGAKU";
            /// <summary>消費税</summary>
            public const string TAX_AMOUNT = "TAX_AMOUNT";
            /// <summary>係数</summary>
            public const string KEISU = "KEISU";
            /// <summary>日数3桁</summary>
            public const string NISU3 = "NISU3";
            /// <summary>日数4桁</summary>
            public const string NISU4 = "NISU4";
            /// <summary>数量</summary>
            public const string SURYO = "SURYO";
            /// <summary>リードタイム</summary>
            public const string LEADTIME = "LEADTIME";
            /// <summary>共通3桁</summary>
            public const string SONOTA = "SONOTA";
        }

        /// <summary>
        /// 共通画面用定数
        /// </summary>
        public static class COMMON_FORM
        {
            /// <summary>
            /// 在庫検索
            /// </summary>
            public static class COM0050
            {
                /// <summary>呼出元画面の在庫検索画面からの戻りボタン</summary>
                public const string BackButton = "COM0050_BACK";
                /// <summary>グローバルリストのキー、在庫検索画面で登録を行ったかを呼出元画面に渡す</summary>
                public const string GlobalKeyIsRegist = "COM0050_IsRegist";
            }
        }

        /// <summary>
        /// 取引先区分
        /// </summary>
        public static class VENDER_DIVISION
        {
            /// <summary>仕入</summary>
            public const string SI = "SI";
            /// <summary>販売</summary>
            public const string TS = "TS";
        }

        /// <summary>
        /// 受注区分
        /// </summary>
        public static class ORDER_DIVISION
        {
            /// <summary>通常受注</summary>
            public const string STANDARD = "1";
            /// <summary>有償サンプル</summary>
            public const string PAID_SAMPLE = "2";
            /// <summary>無償サンプル</summary>
            public const string FREE_SAMPLE = "3";
            /// <summary>受注生産</summary>
            public const string ORDER_DIV = "4";
            /// <summary>通常受注（生産要求）</summary>
            public const string PRODUCT_REQUEST = "5";
            /// <summary>受注生産（生産要求）</summary>
            public const string ORDER_PRODUCT_REQUEST = "6";
        }

        /// <summary>
        /// 自社マスタ
        /// </summary>
        public static class COMPANY
        {
            /// <summary>Gets 自社コード</summary>
            /// <value>自社コード</value>
            public static string COMPANY_CD
            {
                get
                {
                    return "000001";
                }
            }
        }

        /// <summary>
        /// 受注ステータス
        /// </summary>
        public static class ORDER_STATUS
        {
            /// <summary>未登録</summary>
            public const int NOT_REGIST = 0;
            /// <summary>受注登録</summary>
            public const int ORDER_REGIST = 1;
            /// <summary>納期回答完了</summary>
            public const int REPLY_COMPLETE = 2;
            /// <summary>承認依頼中</summary>
            public const int APPROVAL_REQUEST = 3;
            /// <summary>承認</summary>
            public const int APPROVAL = 4;
            /// <summary>出荷指図入力中</summary>
            public const int ORDER_SHIPPING = 5;
            /// <summary>出荷実績入力中</summary>
            public const int ORDER_SHIPPING_RESULT = 6;
            /// <summary>受注クローズ</summary>
            public const int ORDER_CLOSE = 7;
            /// <summary>受注取消</summary>
            public const int ORDER_CANCEL = 9;
        }

        /// <summary>
        /// 購買ステータス
        /// </summary>
        public static class PURCHASE_STATUS
        {
            /// <summary>購入依頼登録</summary>
            public const int PURCHASE_REQUEST_REGIST = 1;
            /// <summary>購入依頼承認依頼中</summary>
            public const int PURCHASE_REQUEST_APPROVAL_REQUEST = 2;
            /// <summary>購入依頼承認済</summary>
            public const int PURCHASE_REQUEST_APPROVAL = 3;
            /// <summary>購買受付</summary>
            public const int PURCHASE_ACCEPT = 4;
            /// <summary>発注登録</summary>
            public const int ORDER_REGIST = 5;
            /// <summary>発注承認依頼中</summary>
            public const int ORDER_APPROVAL_REQUEST = 6;
            /// <summary>発注承認済</summary>
            public const int ORDER_APPROVAL = 7;
            /// <summary>納期回答済</summary>
            public const int DELIVERY_DATE_ANSWERD = 8;
            /// <summary>注文書発行済</summary>
            public const int PURCHASE_ORDER_ISSUED = 9;
            /// <summary>受入検収済</summary>
            public const int ACCEPTED = 10;
            /// <summary>クローズ</summary>
            public const int CLOSE = 99;

        }

        /// <summary>
        /// 生産計画テーブルのステータスの値(production_plan.status)
        /// </summary>
        public static class PRODUCTIONPLAN_STATUS
        {
            /// <summary>
            /// 未確定
            /// </summary>
            public const int UNSETTLED = 0;
            /// <summary>
            /// 確定済
            /// </summary>
            public const int CONFIRMED = 1;
        }

        /// <summary>
        /// 単位コード
        /// </summary>
        public static class UNIT_CODE
        {
            /// <summary>Gets Kg</summary>
            /// <value>Kg</value>
            public static string KG
            {
                get
                {
                    return "2";
                }
            }
        }

        /// <summary>
        /// ファイル取込で使用するファイルの文字コード
        /// </summary>
        public const string UPLOAD_INFILE_CHAR_CODE = "UTF-8";

        /// <summary>
        /// 削除フラグ
        /// </summary>
        public static class DEL_FLG
        {
            /// <summary>未削除</summary>
            public const int OFF = 0;
            /// <summary>削除</summary>
            public const int ON = 1;
        }

        /// <summary>
        /// ワークフロー関連定数
        /// </summary>
        public static class WORKFLOW
        {
            /// <summary>
            /// 各承認のボタンのコントロールID
            /// </summary>
            public static class ButtonCtrlId
            {
                /// <summary>承認依頼</summary>
                public const string ApprovalRequest = "APPROVALREQUEST";
                /// <summary>承認依頼取消</summary>
                public const string ApprovalRequestCancel = "APPROVALREQUESTCANCEL";
                /// <summary>承認</summary>
                public const string Approval = "APPROVAL";
                /// <summary>否認</summary>
                public const string Disapproval = "DISAPPROVAL";
                /// <summary>承認取消</summary>
                public const string ApprovalCancel = "APPROVALCANCEL";

                /// <summary>上記処理を呼び出す隠しボタン</summary>
                public const string ComAWFExec = "COMAWFEXEC";
            }

            /// <summary>
            /// ワークフローヘッダステータス
            /// </summary>
            public static class HEADERSTATUS
            {
                /// <summary>依頼中</summary>
                public const int REQUESTING = 1;
                /// <summary>承認中</summary>
                public const int APPROVING = 10;
                /// <summary>承認完了</summary>
                public const int APPROVAL = 20;
                /// <summary>否認</summary>
                public const int DENIAL = 90;
                /// <summary>引戻</summary>
                public const int CANCEL = 99;
            }

            /// <summary>
            /// ワークフロー詳細ステータス
            /// </summary>
            public static class DETAILSTATUS
            {
                /// <summary>Gets 未承認</summary>
                /// <value>未承認</value>
                public static int? UNAPPROVED
                {
                    get
                    {
                        return null;
                    }
                }
                /// <summary>Gets 承認</summary>
                /// <value>承認</value>
                public static int APPROVAL
                {
                    get
                    {
                        return 10;
                    }
                }
                /// <summary>Gets 承認取消</summary>
                /// <value>承認取消</value>
                public static int APPROVAL_CANCEL
                {
                    get
                    {
                        return 19;
                    }
                }
                /// <summary>Gets 否認</summary>
                /// <value>否認</value>
                public static int DENIAL
                {
                    get
                    {
                        return 90;
                    }
                }
            }

            /// <summary>
            /// 操作履歴
            /// </summary>
            public static class LOGSTATUS
            {
                /// <summary>承認依頼</summary>
                public const int APPROVAL_REQUEST = 1;
                /// <summary>承認</summary>
                public const int APPROVAL = 10;
                /// <summary>承認取消</summary>
                public const int APPROVAL_CANCEL = 19;
                /// <summary>否認</summary>
                public const int DENIAL = 90;
                /// <summary>引戻(承認依頼取消)</summary>
                public const int CANCEL = 99;
            }

            /// <summary>
            /// 内部パラメータ
            /// </summary>
            public static class REQUESTPARAM
            {
                /// <summary>承認依頼</summary>
                public const int APPROVAL_REQUEST = 1;
                /// <summary>承認依頼取消</summary>
                public const int APPROVAL_REQUEST_CANCEL = 99;
                /// <summary>承認</summary>
                public const int APPROVAL = 10;
                /// <summary>承認取消</summary>
                public const int APPROVAL_CANCEL = 19;
                /// <summary>否認</summary>
                public const int DISAPPROVAL = 90;
                /// <summary>否認(否認者以外)</summary>
                public const int DISAPPROVAL_ANOTHER = 95;
                /// <summary>承認完了(指定不能、承認処理で設定される)</summary>
                public const int APPROVAL_COMPLETE = 20;
                /// <summary>承認完了(承認者以外)</summary>
                public const int APPROVAL_COMPLETE_ANOTHER = 25;
            }

            /// <summary>
            /// 通知区分
            /// </summary>
            public enum NOTICEDIVISION
            {
                /// <summary>
                /// 順序
                /// </summary>
                SEQUENTIAL = 1,
                /// <summary>
                /// 並行
                /// </summary>
                PARALLEL = 0
            }

            /// <summary>
            /// ワークフロー区分
            /// </summary>
            public static class WorkflowDivision
            {
                /// <summary>製造</summary>
                public const string MAN01 = "MAN01";
                /// <summary>購入依頼</summary>
                public const string ORD01 = "ORD01";
                /// <summary>発注入力</summary>
                public const string ORD02 = "ORD02";
                /// <summary>製造計画</summary>
                public const string PPL03 = "PPL03";
                /// <summary>購入依頼計画</summary>
                public const string PPL04 = "PPL04";
                /// <summary>購入依頼計画（外注）</summary>
                public const string PPL05 = "PPL05";
                /// <summary>入金入力</summary>
                public const string REC01 = "REC01";
                /// <summary>受注</summary>
                public const string ROR01 = "ROR01";
                /// <summary>在庫入庫</summary>
                public const string INV01 = "INV01";
                /// <summary>在庫出庫</summary>
                public const string INV02 = "INV02";
                /// <summary>在庫移動</summary>
                public const string INV03 = "INV03";
                /// <summary>品目振替</summary>
                public const string INV04 = "INV04";
            }

            /// <summary>
            /// 名称マスタ区分
            /// </summary>
            public static class NamesDivision
            {
                /// <summary>承認ワークフローヘッダステータス</summary>
                public const string WFHS = "WFHS";

                /// <summary>承認ワークフロー区分</summary>
                public const string WFDV = "WFDV";
            }
        }

        /// <summary>
        /// エクセル出力関連定数
        /// </summary>
        public static class REPORT
        {
            /// <summary>
            /// 出力ファイル種類
            /// </summary>
            public static class FILETYPE
            {
                /// <summary>Gets EXCEL</summary>
                /// <value>EXCEL</value>
                public static string EXCEL
                {
                    get
                    {
                        return "1";
                    }
                }
                /// <summary>Gets CSV</summary>
                /// <value>CSV</value>
                public static string CSV
                {
                    get
                    {
                        return "2";
                    }
                }
                /// <summary>Gets PDF</summary>
                /// <value>PDF</value>
                public static string PDF
                {
                    get
                    {
                        return "3";
                    }
                }
                /// <summary>Gets ZIP</summary>
                /// <value>ZIP</value>
                public static string ZIP
                {
                    get
                    {
                        return "4";
                    }
                }
                /// <summary>Gets EXCEL_MACRO</summary>
                /// <value>EXCEL_MACRO</value>
                public static string MACRO
                {
                    get
                    {
                        return "5";
                    }
                }
            }

            /// <summary>
            /// ファイル拡張子
            /// </summary>
            public static class EXTENSION
            {
                /// <summary>Gets .xlsx</summary>
                /// <value>.xlsx</value>
                public static string EXCEL_BOOK
                {
                    get
                    {
                        return ".xlsx";
                    }
                }
                /// <summary>Gets .xlsm</summary>
                /// <value>.xlsm</value>
                public static string EXCEL_MACRO
                {
                    get
                    {
                        return ".xlsm";
                    }
                }
                /// <summary>Gets .pdf</summary>
                /// <value>.pdf</value>
                public static string PDF_FILE
                {
                    get
                    {
                        return ".pdf";
                    }
                }
                /// <summary>Gets .zip</summary>
                /// <value>.zip</value>
                public static string ZIP_FILE
                {
                    get
                    {
                        return ".zip";
                    }
                }
            }
        }

        /// <summary>
        /// PPL0130生産計画確定処理で承認依頼を行う際の伝票番号
        /// </summary>
        public static class SlipNumber
        {
            /// <summary>製造計画照会</summary>
            public const string PPL0030 = "PPL0030";
            /// <summary>購入依頼計画照会</summary>
            public const string PPL0040 = "PPL0040";
        }

        /// <summary>
        /// 製造指図関連テーブル定数
        /// </summary>
        public static class DIRECTION
        {
            /// <summary>
            /// 共通
            /// </summary>
            public static class COMMON
            {
                /// <summary>製造指図区分</summary>
                public static class DIRECTION_DIVISION
                {
                    /// <summary>仕掛品</summary>
                    public const int BATCH = 1;
                    /// <summary>製品</summary>
                    public const int FILLING = 2;
                    /// <summary>品目振替</summary>
                    public const int REFILL = 3;
                    /// <summary>外注品</summary>
                    public const int OUT = 4;
                }
            }

            /// <summary>
            /// 製造指図ヘッダ
            /// </summary>
            public static class HEADER
            {
                /// <summary>指図ステータス</summary>
                public static class DIRECTION_STATUS
                {
                    /// <summary>指図登録</summary>
                    public const int NEW = 1;
                    /// <summary>指図確定</summary>
                    public const int FIX = 2;
                    /// <summary>実績登録中</summary>
                    public const int RESULT_INPUT = 3;
                    /// <summary>指図完了</summary>
                    public const int MANUFACT_COMPLETE = 4;
                }

                /// <summary>検査有無</summary>
                public static class INSPECTION_EXISTENCE
                {
                    /// <summary>検査必要</summary>
                    public const int NEED = 1;
                    /// <summary>検査不要</summary>
                    public const int NOTNEED = 0;
                }
            }

            /// <summary>
            /// 製造指図フォーミュラ
            /// </summary>
            public static class FORMULA
            {
                /// <summary>実績区分</summary>
                public static class RESULT_DIVISION
                {
                    /// <summary>Gets 部品</summary>
                    /// <value>部品</value>
                    public static int PARTS
                    {
                        get
                        {
                            return 0;
                        }
                    }
                    /// <summary>Gets 仕上</summary>
                    /// <value>仕上</value>
                    public static int FINISH
                    {
                        get
                        {
                            return 1;
                        }
                    }
                }

                /// <summary>仕上完了フラグ</summary>
                public static class COMP_FLG
                {
                    /// <summary>Gets 未完了</summary>
                    /// <value>未完了</value>
                    public static int NOTCOMPLETE
                    {
                        get
                        {
                            return 0;
                        }
                    }
                    /// <summary>Gets 完了</summary>
                    /// <value>完了</value>
                    public static int COMPLETE
                    {
                        get
                        {
                            return 1;
                        }
                    }
                }

                /// <summary>部品入力区分</summary>
                public static class PART_INPUT_DIVISION
                {
                    /// <summary>Gets 部品タブ入力</summary>
                    /// <value>部品タブ入力</value>
                    public static int PARTS
                    {
                        get
                        {
                            return 0;
                        }
                    }
                    /// <summary>Gets 仕上タブ入力</summary>
                    /// <value>仕上タブ入力</value>
                    public static int FINISH
                    {
                        get
                        {
                            return 1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 受払区分
        /// </summary>
        public static class INOUT_DIVISION
        {
            /// <summary>Gets 販売売上：通常受注</summary>
            /// <value>販売売上：通常受注</value>
            public static string ORDER_NORMAL
            {
                get
                {
                    return "ROR0010-01";
                }
            }
            /// <summary>Gets 販売売上：受注</summary>
            /// <value>販売売上：受注</value>
            public static string ORDER
            {
                get
                {
                    return "ROR0010-02";
                }
            }
            /// <summary>Gets 販売売上：受注クローズ</summary>
            /// <value>販売売上：受注クローズ</value>
            public static string ORDER_CLOSE
            {
                get
                {
                    return string.Empty;
                }
            }
            /// <summary>Gets 販売売上：納期回答入力</summary>
            /// <value>販売売上：納期回答入力</value>
            public static string SALE_DELIVERY_DATE
            {
                get
                {
                    return "ROR0020-01";
                }
            }
            /// <summary>Gets 販売売上：出荷指図入力</summary>
            /// <value>販売売上：出荷指図入力</value>
            public static string SHIPPING
            {
                get
                {
                    return "SHP0010-01";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（出荷基準）登録</summary>
            /// <value>販売売上：出荷実績（出荷基準）登録</value>
            public static string SHIPPING_STANDARD_REGIST
            {
                get
                {
                    return "SHP0040-01";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（出荷基準）取消</summary>
            /// <value>販売売上：出荷実績（出荷基準）取消</value>
            public static string SHIPPING_STANDARD_CANCEL
            {
                get
                {
                    return "SHP0040-91";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（検収基準）登録</summary>
            /// <value>販売売上：出荷実績（検収基準）登録</value>
            public static string ACCEPTANCE_CRITERIA_REGIST
            {
                get
                {
                    return "SHP0040-02";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（検収基準）取消</summary>
            /// <value>販売売上：出荷実績（検収基準）取消</value>
            public static string ACCEPTANCE_CRITERIA_CANCEL
            {
                get
                {
                    return "SHP0040-92";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（預り在庫）登録</summary>
            /// <value>販売売上：出荷実績（預り在庫）登録</value>
            public static string INVENTORY_CUSTODY_REGIST
            {
                get
                {
                    return "SHP0040-03";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（預り在庫）取消</summary>
            /// <value>販売売上：出荷実績（預り在庫）取消</value>
            public static string INVENTORY_CUSTODY_CANCEL
            {
                get
                {
                    return "SHP0040-93";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（出荷基準）完了</summary>
            /// <value>販売売上：出荷実績（出荷基準）完了</value>
            public static string SHIPPING_STANDARD_COMPLETED
            {
                get
                {
                    return "SHP0040-04";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（出荷基準）完了時取消データ</summary>
            /// <value>販売売上：出荷実績（出荷基準）完了時取消データ</value>
            public static string SHIPPING_STANDARD_BEFORE_COMPLETE_CANCEL
            {
                get
                {
                    return "SHP0040-94";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（検収基準）完了</summary>
            /// <value>販売売上：出荷実績（検収基準）完了</value>
            public static string ACCEPTANCE_CRITERIA_COMPLETED
            {
                get
                {
                    return "SHP0040-05";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（検収基準）完了時取消データ</summary>
            /// <value>販売売上：出荷実績（検収基準）完了時取消データ</value>
            public static string ACCEPTANCE_CRITERIA_BEFORE_COMPLETE_CANCEL
            {
                get
                {
                    return "SHP0040-95";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（預り在庫）完了</summary>
            /// <value>販売売上：出荷実績（預り在庫）完了</value>
            public static string INVENTORY_CUSTODY_COMPLETED
            {
                get
                {
                    return "SHP0040-06";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（預り在庫）完了時取消データ</summary>
            /// <value>販売売上：出荷実績（預り在庫）完了時取消データ</value>
            public static string INVENTORY_CUSTODY_BEFORE_COMPLETE_CANCEL
            {
                get
                {
                    return "SHP0040-96";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（出荷基準）完了取消</summary>
            /// <value>販売売上：出荷実績（出荷基準）完了取消</value>
            public static string SHIPPING_STANDARD_COMPLETE_CANCEL
            {
                get
                {
                    return "SHP0040-97";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（検収基準）完了取消</summary>
            /// <value>販売売上：出荷実績（検収基準）完了取消</value>
            public static string ACCEPTANCE_CRITERIA_COMPLETE_CANCEL
            {
                get
                {
                    return "SHP0040-98";
                }
            }
            /// <summary>Gets 販売売上：出荷実績（預り在庫）完了取消</summary>
            /// <value>販売売上：出荷実績（預り在庫）完了取消</value>
            public static string INVENTORY_CUSTODY_COMPLETE_CANCEL
            {
                get
                {
                    return "SHP0040-99";
                }
            }
            /// <summary>Gets 販売売上：出荷売上（検収基準）登録・承認</summary>
            /// <value>販売売上：出荷売上（検収基準）登録・承認</value>
            public static string SALES_ACCEPTANCE_CRITERIA_REGIST
            {
                get
                {
                    return "SAL0010-01";
                }
            }
            /// <summary>Gets 販売売上：出荷売上（検収基準）取消・承認取消</summary>
            /// <value>販売売上：出荷売上（検収基準）取消・承認取消</value>
            public static string SALES_ACCEPTANCE_CRITERIA_CANCEL
            {
                get
                {
                    return "SAL0010-91";
                }
            }
            /// <summary>Gets 販売売上：通常売上（返品）登録・承認</summary>
            /// <value>販売売上：通常売上（返品）登録・承認</value>
            public static string SALES_RETURN_REGIST
            {
                get
                {
                    return "SAL0010-02";
                }
            }
            /// <summary>Gets 販売売上：通常売上（返品）取消・承認取消</summary>
            /// <value>販売売上：通常売上（返品）取消・承認取消</value>
            public static string SALES_RETURN_CANCEL
            {
                get
                {
                    return "SAL0010-92";
                }
            }
            /// <summary>Gets 購買仕入：購入依頼</summary>
            /// <value>購買仕入：購入依頼</value>
            public static string PURCHASE_REQUEST
            {
                get
                {
                    return "ORD0010-01";
                }
            }
            /// <summary>Gets 購買仕入：発注</summary>
            /// <value>購買仕入：発注</value>
            public static string ORDER_REGIST
            {
                get
                {
                    return "ORD0020-01";
                }
            }
            /// <summary>Gets 購買仕入：発注クローズ</summary>
            /// <value>購買仕入：発注クローズ</value>
            public static string PURCHASE_ORDER_CLOSE
            {
                get
                {
                    return string.Empty;
                }
            }
            /// <summary>Gets 購買仕入：納期回答</summary>
            /// <value>購買仕入：納期回答</value>
            public static string PURCHASE_DELIVERY_DATE
            {
                get
                {
                    return "ORD0030-01";
                }
            }
            /// <summary>Gets 購買仕入：受入登録</summary>
            /// <value>購買仕入：受入登録</value>
            public static string PURCHASE_ACCEPT
            {
                get
                {
                    return "ACC0010-01";
                }
            }
            /// <summary>Gets 購買仕入：受入取消</summary>
            /// <value>購買仕入：受入取消</value>
            public static string PURCHASE_ACCEPT_CANCEL
            {
                get
                {
                    return "ACC0010-91";
                }
            }
            /// <summary>Gets 購買仕入：受入検収登録</summary>
            /// <value>購買仕入：受入検収登録</value>
            public static string PURCHASE_ACCEPT_CHECK
            {
                get
                {
                    return "ACC0010-02";
                }
            }
            /// <summary>Gets 購買仕入：受入検収取消</summary>
            /// <value>購買仕入：受入検収取消</value>
            public static string PURCHASE_ACCEPT_CHECK_CANCEL
            {
                get
                {
                    return "ACC0010-92";
                }
            }
            /// <summary>Gets 購買仕入：仕入（返品）登録・承認</summary>
            /// <value>購買仕入：仕入（返品）登録・承認</value>
            public static string STOCKING_RETURN_REGIST
            {
                get
                {
                    return "PUR0010-01";
                }
            }
            /// <summary>Gets 購買仕入：仕入（返品）取消・承認取消</summary>
            /// <value>購買仕入：仕入（返品）取消・承認取消</value>
            public static string STOCKING_RETURN_CANCEL
            {
                get
                {
                    return "PUR0010-91";
                }
            }
            /// <summary>Gets 計画製造：製造指図_確定承認(部品)</summary>
            /// <value>計画製造：製造指図_確定承認(部品)</value>
            public static string DIRECTION_APPROVAL_PARTS
            {
                get
                {
                    return "MAN0010-01";
                }
            }
            /// <summary>Gets 計画製造：製造指図_確定承認(仕上)</summary>
            /// <value>計画製造：製造指図_確定承認(仕上)</value>
            public static string DIRECTION_APPROVAL_FINISH
            {
                get
                {
                    return "MAN0010-02";
                }
            }
            /// <summary>Gets 計画製造：製造実績_確定承認(部品)</summary>
            /// <value>計画製造：製造実績_確定承認(部品)</value>
            public static string DIRECTION_RESULT_PARTS_REGIST
            {
                get
                {
                    return "MAN0030-01";
                }
            }
            /// <summary>Gets 計画製造：製造実績_確定承認取消(部品)</summary>
            /// <value>計画製造：製造実績_確定承認取消(部品)</value>
            public static string DIRECTION_RESULT_PARTS_CANCEL
            {
                get
                {
                    return "MAN0030-91";
                }
            }
            /// <summary>Gets 計画製造：製造実績_確定承認(仕上)</summary>
            /// <value>計画製造：製造実績_確定承認(仕上)</value>
            public static string DIRECTION_RESULT_FINISH_REGIST
            {
                get
                {
                    return "MAN0030-02";
                }
            }
            /// <summary>Gets 計画製造：製造実績_確定承認取消(仕上)</summary>
            /// <value>計画製造：製造実績_確定承認取消(仕上)</value>
            public static string DIRECTION_RESULT_FINISH_CANCEL
            {
                get
                {
                    return "MAN0030-92";
                }
            }
            /// <summary>Gets 計画製造：製造実績取込_確定承認(部品)</summary>
            /// <value>計画製造：製造実績取込_確定承認(部品)</value>
            public static string DIRECTION_IMPORT_PARTS_REGIST
            {
                get
                {
                    return "MAN0040-01";
                }
            }
            /// <summary>Gets 計画製造：製造実績取込_確定承認取消(部品)</summary>
            /// <value>計画製造：製造実績取込_確定承認取消(部品)</value>
            public static string DIRECTION_IMPORT_PARTS_CANCEL
            {
                get
                {
                    return "MAN0040-91";
                }
            }
            /// <summary>Gets 計画製造：製造実績取込_確定承認(仕上)</summary>
            /// <value>計画製造：製造実績取込_確定承認(仕上)</value>
            public static string DIRECTION_IMPORT_FINISH_REGIST
            {
                get
                {
                    return "MAN0040-02";
                }
            }
            /// <summary>Gets 計画製造：製造実績取込_確定承認取消(仕上)</summary>
            /// <value>計画製造：製造実績取込_確定承認取消(仕上)</value>
            public static string DIRECTION_IMPORT_FINISH_CANCEL
            {
                get
                {
                    return "MAN0040-92";
                }
            }
            /// <summary>Gets 在庫調整：在庫入庫入力</summary>
            /// <value>在庫調整：在庫入庫入力</value>
            public static string INVENTORY_RECEIPT_REGIST
            {
                get
                {
                    return "INV0020-01";
                }
            }
            /// <summary>Gets 在庫調整：在庫入庫取消</summary>
            /// <value>在庫調整：在庫入庫取消</value>
            public static string INVENTORY_RECEIPT_CANCEL
            {
                get
                {
                    return "INV0020-91";
                }
            }
            /// <summary>Gets 在庫調整：在庫出庫入力</summary>
            /// <value>在庫調整：在庫出庫入力</value>
            public static string INVENTORY_DELIVERY_REGIST
            {
                get
                {
                    return "INV0030-01";
                }
            }
            /// <summary>Gets 在庫調整：在庫出庫取消</summary>
            /// <value>在庫調整：在庫出庫取消</value>
            public static string INVENTORY_DELIVERY_CANCEL
            {
                get
                {
                    return "INV0030-91";
                }
            }
            /// <summary>Gets 在庫調整：在庫移動入力</summary>
            /// <value>在庫調整：在庫移動入力</value>
            public static string INVENTORY_MOVEMENT_REGIST
            {
                get
                {
                    return "INV0040-01";
                }
            }
            /// <summary>Gets 在庫調整：在庫移動取消</summary>
            /// <value>在庫調整：在庫移動取消</value>
            public static string INVENTORY_MOVEMENT_CANCEL
            {
                get
                {
                    return "INV0040-91";
                }
            }
            /// <summary>Gets 在庫調整：品目振替入力</summary>
            /// <value>在庫調整：品目振替入力</value>
            public static string INVENTORY_ITEM_TRANSFER_REGIST
            {
                get
                {
                    return "INV0050-01";
                }
            }
            /// <summary>Gets 在庫調整：品目振替取消</summary>
            /// <value>在庫調整：品目振替取消</value>
            public static string INVENTORY_ITEM_TRANSFER_CANCEL
            {
                get
                {
                    return "INV0050-91";
                }
            }
            /// <summary>Gets 在庫調整：棚卸</summary>
            /// <value>在庫調整：棚卸</value>
            public static string INVENTORY
            {
                get
                {
                    return "STT0010-01";
                }
            }
            /// <summary>Gets 在庫調整：棚卸(取消)</summary>
            /// <value>在庫調整：棚卸(取消)</value>
            public static string INVENTORYROLLBACK
            {
                get
                {
                    return "STT0010-02";
                }
            }
        }

        /// <summary>
        /// 有効フラグ
        /// </summary>
        public static class AVTIVE_FLG
        {
            /// <summary>有効(承認)</summary>
            public const int APPROVAL = 1;
            /// <summary>無効(未承認)</summary>
            public const int UNAPPROVAL = 0;
        }

        /// <summary>
        /// 手続き区分（MRP結果）
        /// </summary>
        public static class MRP_PROCEDURE_DIVISION
        {
            /// <summary>初期値</summary>
            public const int FIRST = 0;
            /// <summary>製造</summary>
            public const int PRODUCTION = 1;
            /// <summary>購買</summary>
            public const int PURCHASE = 2;
        }

        /// <summary>
        /// 製造区分
        /// </summary>
        public static class PRODUCTION_DIVISION
        {
            /// <summary>該当</summary>
            public const int ON = 1;
            /// <summary>非該当</summary>
            public const int OFF = 0;
        }

        /// <summary>
        /// 販売区分
        /// </summary>
        public static class ARTICLE_DIVISION
        {
            /// <summary>該当</summary>
            public const int ON = 1;
            /// <summary>非該当</summary>
            public const int OFF = 0;
        }

        /// <summary>
        /// 購入品区分
        /// </summary>
        public static class PURCHASE_DIVISION
        {
            /// <summary>該当</summary>
            public const int ON = 1;
            /// <summary>非該当</summary>
            public const int OFF = 0;
        }

        /// <summary>
        /// 計画区分
        /// </summary>
        public static class PLAN_DIVISION
        {
            /// <summary>製造</summary>
            public const int PRODUCTION = 1;
            /// <summary>購買</summary>
            public const int PURCHASE = 2;
            /// <summary>外注</summary>
            public const int OUTSOURCING = 3;
        }

        /// <summary>
        /// オーダー発行区分
        /// </summary>
        public static class ORDER_PUBLISH_DIVISION
        {
            /// <summary>入庫予定</summary>
            public const string WAREHOUSING = "1";
            /// <summary>生産計画</summary>
            public const string PRODUCTION = "3";
            /// <summary>独立需要</summary>
            public const string INDEPENDENT = "4";
            /// <summary>計画・引当</summary>
            public const string PLAN_RESERVE = "5";
            /// <summary>計画・出庫予定</summary>
            public const string PLAN_SHIPPING = "6";
            /// <summary>出庫予定</summary>
            public const string SHIPPING = "10";
            /// <summary>出庫予定(展開なし)</summary>
            public const string SHIPPING_NO_DEPLOYMENT = "11";
            /// <summary>引当</summary>
            public const string RESERVE = "12";

        }

        /// <summary>
        /// スポット区分
        /// </summary>
        public static class SPOT_DIVISION
        {
            /// <summary>通常</summary>
            public const int NotSpot = 1;
            /// <summary>スポット</summary>
            public const int Spot = 2;
        }

        /// <summary>
        /// 在庫管理区分
        /// </summary>
        public static class STOCK_DIVISION
        {
            /// <summary>通常</summary>
            public const int USUALLY = 1;
            /// <summary>在庫表除外</summary>
            public const int INVENTORY_EXCLUSION = 2;
            /// <summary>更新除外</summary>
            public const int UPDATE_EXCLUSION = 3;

        }

        /// <summary>
        /// 発注基準
        /// </summary>
        public static class PURCHASE_TRIGGER
        {
            /// <summary>発注点</summary>
            public const int ORDERING_POINT = 1;
            /// <summary>MRP</summary>
            public const int MRP = 2;
            /// <summary>個別</summary>
            public const int INDIVIDUAL = 3;

        }

        /// <summary>
        /// 複数社発注区分
        /// </summary>
        public static class MULTI_VENDER_DIVISION
        {
            /// <summary>する</summary>
            public const int YES = 1;
            /// <summary>しない</summary>
            public const int NO = 2;

        }

        /// <summary>
        /// 標準検査方法
        /// </summary>
        public static class INSPECTION_TYPE
        {
            /// <summary>無検査</summary>
            public const int NOT = 1;
            /// <summary>抜取検査</summary>
            public const int POINT = 2;
            /// <summary>全品検査</summary>
            public const int ALL = 2;

        }

        /// <summary>
        /// 製造指図フォーミュラ 品目タイプ
        /// </summary>
        public static class DIR_FORMULA_LINE_TYPE
        {
            /// <summary>原材料</summary>
            public const int MATERIAL = -1;
            /// <summary>製品</summary>
            public const int MAIN = 1;
            /// <summary>副製品</summary>
            public const int SUB = 2;
            /// <summary>仕掛品・半製品</summary>
            public const int MIDDLE = 3;
            /// <summary>回収品・廃棄物</summary>
            public const int WASTE = 4;
        }

        /// <summary>
        /// 生産計画ログ：ログ区分
        /// </summary>
        public static class PLAN_LOG_DIVISION
        {
            /// <summary>生産計画準備処理</summary>
            public const int PLAN_PREPARATION = 1;
            /// <summary>部品展開処理</summary>
            public const int PARTS_DEVELOPMENT = 2;
            /// <summary>生産計画確定処理</summary>
            public const int PRODUCTION_PLAN_FIXED = 3;

        }

        /// <summary>
        /// 生産計画ログ：ステータス
        /// </summary>
        public static class PLAN_LOG_STATUS
        {
            /// <summary>完了</summary>
            public const int COMPLETION = 0;
            /// <summary>エラー</summary>
            public const int ERROR = 9;

        }

        /// <summary>
        /// 品目マスタ_製造品扱い属性：生産計画区分
        /// </summary>
        public static class PRODUCTION_PLAN
        {
            /// <summary>自動</summary>
            public const int AUTO = 1;
            /// <summary>手動</summary>
            public const int MANUAL = 2;

        }

        /// <summary>
        /// 品目マスタ_販売品扱い属性：預り品区分
        /// </summary>
        public static class KEEP_DIVISION
        {
            /// <summary>通常</summary>
            public const int NORMAL = 1;
            /// <summary>預り品</summary>
            public const int KEEP = 2;

        }

        /// <summary>
        /// 品目仕様マスタ
        /// </summary>
        public static class ITEM_SPECIFICATION
        {
            /// <summary>検査フラグ</summary>
            public static class INSPECTION_FLG
            {
                /// <summary>要</summary>
                public const int NEED = 1;
                /// <summary>不要</summary>
                public const int NOTNEED = 0;
            }
        }

        /// <summary>
        /// 消費税区分マスタ
        /// </summary>
        public static class TAX_MASTER
        {
            /// <summary>
            /// 用途
            /// </summary>
            public static class CATEGORY
            {
                /// <summary>売上</summary>
                public const string SALES = "SALES";
                /// <summary>仕入</summary>
                public const string STOCKING = "STOCKING";
                /// <summary>入金</summary>
                public const string CREDUT = "CREDIT";
            }
        }

        /// <summary>出荷テーブル</summary>
        public static class SHIPPING
        {
            /// <summary>ヘッダ</summary>
            public static class HEAD
            {
                /// <summary>出荷指図区分</summary>
                public static class SHIPPING_DIVISION
                {
                    /// <summary>受注出荷</summary>
                    public const string ORDER = "1";
                    /// <summary>預かり在庫出荷</summary>
                    public const string KEEP = "2";
                }

                /// <summary>ステータス</summary>
                public static class SHIPPING_STATUS
                {
                    /// <summary>未出荷</summary>
                    public const int UNSHIPPING = 1;
                    /// <summary>未出荷(配車済)</summary>
                    public const int UNSHIPPING_DISPATCH = 2;
                    /// <summary>出荷中</summary>
                    public const int SHIPPING = 3;
                    /// <summary>出荷済</summary>
                    public const int SHIPPING_REGISTED = 4;
                    /// <summary>出荷完了</summary>
                    public const int SHIPPING_COMPLERTE = 5;
                }

                /// <summary>完納区分</summary>
                public static class DELIVERY_COMP
                {
                    /// <summary>未完納</summary>
                    public const int UNCOMPLETE = 0;
                    /// <summary>完納</summary>
                    public const int COMPLETE = 1;
                    /// <summary>更新無し(必須のため、内部処理用)</summary>
                    public const int NO_UPDATE = -1;
                }

                /// <summary>在庫預かり</summary>
                public static class KEEP_INVENTORY
                {
                    /// <summary>なし</summary>
                    public const int NO = 0;
                    /// <summary>あり</summary>
                    public const int YES = 1;
                }
            }

            /// <summary>詳細</summary>
            public static class DETAIL
            {
                /// <summary>出荷引当フラグ</summary>
                public static class SELECT_LOC_LOT_FLG
                {
                    /// <summary>未引当</summary>
                    public const int UNCOMPLETE = 0;
                    /// <summary>引当済</summary>
                    public const int COMPLETE = 1;
                }

                /// <summary>ロット指図有無</summary>
                public static class LOT_DIRECTION_FLG
                {
                    /// <summary>出荷指図にてロット指定なし</summary>
                    public const int NO = 0;
                    /// <summary>出荷指図にてロット指定あり</summary>
                    public const int YES = 1;
                }
            }
        }

        /// <summary>ロット付帯テーブル</summary>
        public static class LOT_EX_INFO
        {
            /// <summary>ヘッダ</summary>
            public static class HEAD
            {
                /// <summary>検査ステータス</summary>
                public static class INSPECTION_STATUS
                {
                    /// <summary>検査完了</summary>
                    public const int INSPECTED = 2;
                    /// <summary>未検査</summary>
                    public const int NOTINSPECTED = 1;
                }

                /// <summary>検査グレード</summary>
                public static class QUALITY_GRAFE
                {
                    /// <summary>1級品</summary>
                    public const int FIRST_GRADE_PRODUCT = 1;
                    /// <summary>2級品</summary>
                    public const int SECOND_GRADE_PRODUCT = 2;
                    /// <summary>不適合品(再利用可能)</summary>
                    public const int REUSABLE = 3;
                    /// <summary>不適合品(廃棄)</summary>
                    public const int WASTE = 4;
                }

                /// <summary>出荷用在庫ステータス</summary>
                public static class SHIPPING_STATUS
                {
                    /// <summary>正常</summary>
                    public const int NORMAL = 1;
                    /// <summary>異常</summary>
                    public const int ABNORMAL = 9;
                }

                /// <summary>製造用在庫ステータス</summary>
                public static class DIRECTION_STATUS
                {
                    /// <summary>正常</summary>
                    public const int NORMAL = 1;
                    /// <summary>異常</summary>
                    public const int ABNORMAL = 9;
                }
            }
        }

        /// <summary>ロケーション．資産管理区分</summary>
        public static class ASSET_DIVISION
        {
            /// <summary>Gets 帳簿内(自社在庫)</summary>
            /// <value>帳簿内(自社在庫)</value>
            public static string INSIDE_ACCOUNT_COMPANY
            {
                get
                {
                    return "01";
                }
            }
            /// <summary>Gets 帳簿内(預け在庫)</summary>
            /// <value>帳簿内(預け在庫)</value>
            public static string INSIDE_ACCOUNT_INSPECTION
            {
                get
                {
                    return "02";
                }
            }
            /// <summary>Gets 帳簿外(預り在庫)</summary>
            /// <value>帳簿外(預り在庫)</value>
            public static string INSIDE_ACCOUNT_KEEP
            {
                get
                {
                    return "03";
                }
            }
        }

        /// <summary>受注ヘッダ</summary>
        public static class ORDER_HEAD
        {
            /// <summary>売上計上区分</summary>
            public static class RECORD_SALES_BASIS
            {
                /// <summary>出荷基準</summary>
                public const int SHIPPING = 1;
                /// <summary>検収基準</summary>
                public const int INSPECTION = 2;
            }

            /// <summary>受注明細書発行区分</summary>
            public static class ORDER_LIST_OUTPUT_DIVISION
            {
                /// <summary>未発行</summary>
                public const int UNISSUED = 1;
                /// <summary>発行済</summary>
                public const int ISSUED = 2;
            }

            /// <summary>納品書発行区分</summary>
            public static class DELIVERY_NOTE_OUTPUT_DIVISION
            {
                /// <summary>未発行</summary>
                public const int UNISSUED = 1;
                /// <summary>発行済</summary>
                public const int ISSUED = 2;
            }
        }

        /// <summary>売上テーブル</summary>
        public static class SALES
        {
            /// <summary>ヘッダ</summary>
            public static class HEAD
            {
                /// <summary>ステータス</summary>
                public static class STATUS
                {
                    /// <summary>登録済</summary>
                    public const int REGISTED = 1;
                    /// <summary>承認依頼中</summary>
                    public const int APPROVAL_REQUEST = 2;
                    /// <summary>承認</summary>
                    public const int APPROVAL = 3;
                    /// <summary>仕訳送信済</summary>
                    public const int JOURNAL_SEND = 4;
                    /// <summary>売掛更新済</summary>
                    public const int ACCOUNTS_RECIEVABLE_UPDATED = 5;
                }

                /// <summary>伝票発行済区分</summary>
                public static class SLIP_PUBLISH_COMP
                {
                    /// <summary>未発行</summary>
                    public const int UN_ISSUE = 1;
                    /// <summary>発行済</summary>
                    public const int ISSUED = 2;
                    /// <summary>不要</summary>
                    public const int NOT_NEED = 9;
                }

                /// <summary>売上種別</summary>
                public static class SALES_KIND
                {
                    /// <summary>通常売上</summary>
                    public const int NORMAL = 1;
                    /// <summary>出荷売上</summary>
                    public const int SHIPPING = 2;
                    /// <summary>預り売上</summary>
                    public const int KEEP = 3;
                }
            }
            /// <summary>詳細</summary>
            public static class DETAIL
            {

            }
        }

        /// <summary>仕入テーブル</summary>
        public static class STOCKING
        {
            /// <summary>ステータス</summary>
            public static class STATUS
            {
                /// <summary>登録済</summary>
                public const int REGISTED = 1;
                /// <summary>承認依頼中</summary>
                public const int APPROVAL_REQUEST = 2;
                /// <summary>承認</summary>
                public const int APPROVAL = 3;
                /// <summary>仕訳送信済</summary>
                public const int JOURNAL_SEND = 4;
                /// <summary>買掛更新済</summary>
                public const int ACCOUNTS_RECIEVABLE_UPDATED = 5;
            }
        }

        /// <summary>
        /// 対象
        /// </summary>
        public static class TARGET_DIVISION
        {
            /// <summary>未処理</summary>
            public const int UNPRPOCESSED = 0;
            /// <summary>処理済</summary>
            public const int PRPOCESSED = 1;
            /// <summary>対象外</summary>
            public const int NA = 9;
        }

        /// <summary>
        /// 更新フラグ
        /// </summary>
        public static class UPDATE_DIVISION
        {
            /// <summary>未処理</summary>
            public const int UNPRPOCESSED = 0;
            /// <summary>処理済</summary>
            public const int PRPOCESSED = 1;
        }

        /// <summary>取引先</summary>
        public static class VENDER
        {
            /// <summary>消費税課税区分</summary>
            public static class TAX_DIVISION
            {
                /// <summary>外税</summary>
                public const int EXCLUDED = 1;
                /// <summary>内税</summary>
                public const int INCLUDED = 2;
                /// <summary>非課税</summary>
                public const int FREE = 3;
                /// <summary>自社マスタ</summary>
                public const int COMPANY = 4;
                /// <summary>不課税</summary>
                public const int NONE = 5;
                /// <summary>免税</summary>
                public const int EXEMPT = 6;
            }
            /// <summary>消費税算出区分</summary>
            public static class CALC_DIVISION
            {
                /// <summary>明細</summary>
                public const int DETAIL = 1;
                /// <summary>伝票</summary>
                public const int VOUCHER = 2;
                /// <summary>自社マスタ</summary>
                public const int COMPANY = 4;
            }

            /// <summary>消費税端数区分</summary>
            public static class TAX_ROUNDUP
            {
                /// <summary>切り捨て</summary>
                public const int ROUND_UP = 1;
                /// <summary>四捨五入</summary>
                public const int ROUND_OFF = 2;
                /// <summary>切り上げ</summary>
                public const int ROUND_DOWN = 3;
                /// <summary>自社マスタ</summary>
                public const int COMPANY = 4;
            }

            /// <summary>消費税端数単位</summary>
            public static class TAX_ROUNDUP_INIT
            {
                /// <summary>自社マスタ</summary>
                public const int COMPANY = 9;
            }
        }

        /// <summary>
        /// 品目のステータス
        /// </summary>
        public static class ITEM_STATUS
        {
            /// <summary>登録中</summary>
            public const int REGIST = 1;
            /// <summary>承認依頼中</summary>
            public const int APPROVAL_REQUEST = 2;
            /// <summary>承認済</summary>
            public const int APPROVAL = 3;
            /// <summary>否認済</summary>
            public const int DENIAL = 4;
            /// <summary>無効</summary>
            public const int INVALID = 9;
        }

        /// <summary>
        /// 品目仕様のステータス
        /// </summary>
        public static class SPEC_STATUS
        {
            /// <summary>登録中</summary>
            public const int REGIST = 1;
            /// <summary>承認依頼中</summary>
            public const int APPROVAL_REQUEST = 2;
            /// <summary>承認済</summary>
            public const int APPROVAL = 3;
            /// <summary>否認済</summary>
            public const int DENIAL = 4;
            /// <summary>未登録</summary>
            public const int NOREGIST = 5;
        }

        /// <summary>
        /// 名称マスタ区分
        /// </summary>
        public static class NamesDivision
        {
            /// <summary>売上科目区分</summary>
            public const string CARC = "CARC";
            /// <summary>仕入科目区分</summary>
            public const string CPUC = "CPUC";
            /// <summary>勘定コード表区分</summary>
            public const string ZACD = "ZACD";

            // 品目マスタ
            /// <summary>品目タイプ</summary>
            public const string CITP = "CITP";
            /// <summary>製造品区分</summary>
            public const string CPRD = "CPRD";
            /// <summary>販売品区分</summary>
            public const string CARD = "CARD";
            /// <summary>購入品区分</summary>
            public const string CPUD = "CPUD";
            /// <summary>受入検査区分</summary>
            public const string CIND = "CIND";
            /// <summary>単位</summary>
            public const string UNIT = "UNIT";
            /// <summary>在庫管理区分</summary>
            public const string CSTD = "CSTD";
            /// <summary>マイナス在庫許可</summary>
            public const string ZNIP = "ZNIP";
            /// <summary>ロット管理区分</summary>
            public const string CRUN = "CRUN";
            /// <summary>スポット区分</summary>
            public const string CSPD = "CSPD";
            /// <summary>検査フラグ</summary>
            public const string ZCIF = "ZCIF";
            /// <summary>計画区分</summary>
            public const string PLDV = "PLDV";

            // 製造品属性
            /// <summary>生産計画区分</summary>
            public const string CPLD = "CPLD";
            /// <summary>指図発行時初期状態</summary>
            public const string ZDDF = "ZDDF";

            // 販売品属性
            /// <summary>預り品区分</summary>
            public const string CKPD = "CKPD";
            /// <summary>受注登録時指図発行フラグ</summary>
            public const string ZADF = "ZADF";

            // 購入品属性
            /// <summary>発注基準</summary>
            public const string CPTD = "CPTD";
            /// <summary>標準検査方法</summary>
            public const string CINS = "CINS";

            // 原処方
            /// <summary>レシピタイプ</summary>
            public const string CRTP = "CRTP";
            /// <summary>用途</summary>
            public const string CRUE = "CRUE";
            /// <summary>レシピステータス</summary>
            public const string CRST = "CRST";
            /// <summary>品目タイプ</summary>
            public const string CLTP = "CLTP";
            /// <summary>紐づき単位</summary>
            public const string ZPID = "ZPID";

            // 取引先区分
            /// <summary>取引先区分</summary>
            public const string CVND = "CVND";

        }

        /// <summary>
        /// 仕訳処理管理
        /// </summary>
        public static class SIWAKE_MANAGEMENT
        {
            /// <summary>
            /// 仕訳処理区分
            /// </summary>
            public static class DIVISION
            {
                /// <summary>販売</summary>
                public const int SALES = 1;
                /// <summary>入金</summary>
                public const int CREDIT = 2;
                /// <summary>仕入</summary>
                public const int STOCKING = 3;
                /// <summary>支払</summary>
                public const int PAYMENT = 4;
                /// <summary>相殺</summary>
                public const int OFFSET = 5;
                /// <summary>入金消込</summary>
                public const int ERASER_CREDIT = 6;
                /// <summary>支払消込</summary>
                public const int ERASER_PAYMENT = 7;
                /// <summary>相殺消込</summary>
                public const int ERASER_OFFSET = 8;
            }
        }

        /// <summary>
        /// 月次処理管理
        /// </summary>
        public static class MONTH_MANAGEMENT
        {
            /// <summary>
            /// 締処理区分
            /// </summary>
            public static class CLOSE_DIVISION
            {
                /// <summary>販売</summary>
                public const int SALES = 1;
                /// <summary>購買</summary>
                public const int PURCHASE = 2;
                /// <summary>製造</summary>
                public const int PRODUCTION = 3;
                /// <summary>在庫調整</summary>
                public const int INVENTORY_ADJUSTMENT = 4;
                /// <summary>月次受払更新</summary>
                public const int MONTHLY_RECEIPT = 10;
                /// <summary>月次本締</summary>
                public const int MONTH_FINAL_CLOSING = 11;
            }

            /// <summary>
            /// ステータス
            /// </summary>
            public static class STATUS
            {
                /// <summary>未実行</summary>
                public const int NOT_EXECUTED = 0;
                /// <summary>実行中</summary>
                public const int DURING_EXECUTION = 1;
                /// <summary>クローズ</summary>
                public const int CLOSED = 2;
                /// <summary>異常終了</summary>
                public const int FAILED = 10;
            }
        }

        /// <summary>
        /// 承認ステータス区分
        /// </summary>
        public static class APDV
        {
            /// <summary>登録中</summary>
            public const int REGIST = 1;
            /// <summary>承認依頼中</summary>
            public const int APPROVAL_REQUEST = 2;
            /// <summary>承認済</summary>
            public const int APPROVAL = 3;
            ///// <summary>否認済</summary>
            //public const int DENIAL = 4;
            ///// <summary>未登録</summary>
            //public const int NOREGIST = 5;
        }

        /// <summary>
        /// 通知情報
        /// </summary>
        public static class NOTICE_INFO
        {
            /// <summary>通知区分</summary>
            public static class NOTICE_DIVISION
            {
                /// <summary>通知</summary>
                public const int NOTICE = 1;
                /// <summary>お知らせ</summary>
                public const int NEWS = 2;
            }

            /// <summary>
            /// 通知分類
            /// </summary>
            public static class NOTICE_CLASS
            {
                /// <summary>承認依頼</summary>
                public const string APPROVAL_REQUEST = "WF01";
                /// <summary>承認依頼取消</summary>
                public const string APPROVAL_REQUEST_CANCEL = "WF99";
                /// <summary>承認</summary>
                public const string APPROVAL = "WF10";
                /// <summary>承認取消</summary>
                public const string APPROVAL_CANCEL = "WF19";
                /// <summary>否認</summary>
                public const string DISAPPROVAL = "WF90";
                /// <summary>承認完了(指定不能、承認処理で設定される)</summary>
                public const string APPROVAL_COMPLETE = "WF20";
            }
        }

        /// <summary>
        /// 品目仕様マスタ:マイナス在庫許可フラグ
        /// </summary>
        public static class NEGATIVE_INVENTORY_PERMIT_FLG
        {
            /// <summary>マイナス在庫NG</summary>
            public const int NG = 0;
            /// <summary>マイナス在庫OK</summary>
            public const int OK = 1;
            /// <summary>自社マスタ</summary>
            public const int Company = 2;
        }

        /// <summary>
        /// ロケーションマスタ：在庫可能フラグ
        /// </summary>
        public static class AVAILABLE_FLG
        {
            /// <summary>マイナス在庫NG</summary>
            public const int NG = 1;
            /// <summary>マイナス在庫OK</summary>
            public const int OK = 2;
        }

        /// <summary>
        /// 原処方の用途
        /// </summary>
        public static class RECIPE_USE_DIVISION
        {
            /// <summary>原価</summary>
            public const string COST = "COST";
            /// <summary>製造</summary>
            public const string PRODUCT = "PRODUCT";
            /// <summary>包装</summary>
            public const string FILL  = "FILL";
        }

        /// <summary>
        /// 原処方フォーミュラの部品入力区分
        /// </summary>
        public static class PART_INPUT_DIVISION
        {
            /// <summary>配合</summary>
            public const int FORMULA = 0;
            /// <summary>仕上</summary>
            public const int FINISH = 1;
        }

        /// <summary>
        /// 原処方フォーミュラの実績区分
        /// </summary>
        public static class RESULT_DIVISION
        {
            /// <summary>配合</summary>
            public const int FORMULA = 0;
            /// <summary>仕上</summary>
            public const int FINISH = 1;
        }

        /// <summary>
        /// 原処方配合の品目タイプ
        /// </summary>
        public static class LINE_TYPE
        {
            /// <summary>原材料</summary>
            public const int MATERIAL = -1;
        }

        /// <summary>
        /// 完納区分
        /// </summary>
        public static class COMPLETE_DIVISION
        {
            /// <summary>未完納</summary>
            public const int INCOMPLETE = 0;
            /// <summary>完納</summary>
            public const int COMPLETE = 1;
        }
    }
}
