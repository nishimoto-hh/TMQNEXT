using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommonExcelUtil;
using CommonWebTemplate.Models.Common;
using CommonSTDUtil.CommonLogger;

using APResources = CommonAPUtil.APCommonUtil.APResources;
using apStoredFncUtil = CommonAPUtil.APStoredFncUtil.APStoredFncUtil;
using apStoredPrcUtil = CommonAPUtil.APStoredPrcUtil.APStoredPrcUtil;
using BaseClass = CommonAPUtil.APCommonUtil.APCommonBaseClass;
using CheckDigit = CommonAPUtil.APCheckDigitUtil.APCheckDigitUtil;
using ComBusBase = CommonSTDUtil.CommonBusinessLogic.CommonBusinessLogicBase;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Constants = APConstants.APConstants;
using EntityDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ExcelUtil = CommonExcelUtil.CommonExcelUtil;
using MailUtil = CommonAPUtil.APSendMailUtil.APSendMailUtil;
using masterCheckUtil = CommonAPUtil.APMasterCheckUtil.APMasterCheckUtil;
using proCheckClass = CommonAPUtil.APStoredPrcUtil.ProCheckClass;

namespace APCommonUtil.APCommonUtil
{
    /// <summary>
    /// AP共通メソッド
    /// </summary>
    public class APCommonUtil : MarshalByRefObject
    {
        #region 定数
        /// <summary>自社コード</summary>
        private const string Company_Cd = "000001";
        /// <summary>日本の国際番号「81」</summary>
        private const string Integer_No_Japan = "81";
        /// <summary>国際番号表示「+」</summary>
        private const string Tel_Hader_Plus = "+";
        /// <summary>ハイフン</summary>
        private const string Hyphen = "-";
        /// <summary>SQL格納先サブディレクトリ名</summary>
        public const string DirPath = "Common";
        /// <summary>プルーフSQL格納先サブディレクトリ名</summary>
        public const string DirPathProof = "Common\\Proof";

        /// <summary>
        /// ロット付帯情報クラス
        /// </summary>
        public class LotExtInfo
        {
            /// <summary>
            /// SQL定義
            /// </summary>
            public static class SqlName
            {
                /// <summary>SQL名：ロット付帯情報ヘッダ登録用SQL</summary>
                public const string InsLotExtInfoHead = "InsLotExtInfoHead";
                /// <summary>SQL名：ロット付帯情報詳細登録用SQL</summary>
                public const string InsLotExtInfoDetail = "InsLotExtInfoDetail";
                /// <summary>SQL名：ロット付帯情報ヘッダ更新用SQL</summary>
                public const string UpdLotExtInfoHead = "UpdLotExtInfoHead";
                /// <summary>SQL名：ロット付帯情報詳細更新用SQL</summary>
                public const string UpdLotExtInfoDetail = "UpdLotExtInfoDetail";
                /// <summary>SQL名：ロット付帯情報ヘッダ削除用SQL</summary>
                public const string DelLotExtInfoHead = "DelLotExtInfoHead";
                /// <summary>SQL名：ロット付帯情報詳細削除用SQL</summary>
                public const string DelLotExtInfoDetail = "DelLotExtInfoDetail";

                /// <summary>SQL名：ロット付帯情報ヘッダプルーフSQL</summary>
                public const string HeadProof = "Lot_Ext_Info_Head_Proof";
                /// <summary>SQL名：ロット付帯情報詳細プルーフSQL</summary>
                public const string DetailProof = "Lot_Ext_Info_Detail_Proof";
            }
        }

        /// <summary>
        /// 製造指図クラス
        /// </summary>
        public class Direction
        {
            /// <summary>
            /// SQL定義
            /// </summary>
            public static class SqlName
            {
                /// <summary>SQL名：製造指図フォーミュラ登録用SQL</summary>
                public const string InsDirectionFormula = "DirectionFormula_Insert";
                /// <summary>SQL名：製造指図フォーミュラ更新用SQL</summary>
                public const string UpdDirectionFormula = "DirectionFormula_Update";
                /// <summary>SQL名：製造指図フォーミュラ削除用SQL</summary>
                public const string DelDirectionFormula = "DirectionFormula_Delete";

                /// <summary>SQL名：製造指図フォーミュラプルーフSQL</summary>
                public const string FormulaProof = "Direction_Formula_Proof";
            }
        }

        /// <summary>
        /// ワークフロークラス
        /// </summary>
        public class WorkFlow
        {
            /// <summary>SQL名：ワークフローNo取得</summary>
            public const string WorkFlow_GetWorkflowNo = "WorkFlow_GetWfNo";
            /// <summary>SQL名：ワークフロー操作ログ 更新処理</summary>
            public const string WorkFlowLog_Insert = "WorkFlowLog_Insert";
            /// <summary>SQL名：ワークフローヘッダ 更新処理</summary>
            public const string WorkFlowHeader_Update = "WorkFlowHeader_Update";
            /// <summary>SQL名：ワークフロー詳細 更新処理</summary>
            public const string WorkFlowDetail_Update = "WorkFlowDetail_Update";
            /// <summary>SQL名：ワークフロー 通知対象ユーザ取得</summary>
            public const string WorkFlow_GetNoticeUserList = "WorkFlow_GetNoticeUserList";
            /// <summary>SQL名：ワークフロー 通知対象未承認ユーザ取得</summary>
            public const string WorkFlow_GetUnApprovedUserList = "WorkFlow_GetUnApprovedUserList";
            /// <summary>SQL名：ワークフロー 通知メール文章取得</summary>
            public const string Workflow_GetMailText = "Workflow_GetMailText";
            /// <summary>SQL名：ワークフロー 通知：順序：承認：直近の更新階層情報取得</summary>
            public const string WorkFlow_GetUpdatedSeqInfo = "WorkFlow_GetUpdatedSeqInfo";
            /// <summary>SQL名：ワークフロー 通知メール送信者取得</summary>
            public const string WorkFlow_GetMailFrom = "WorkFlow_GetMailFrom";
            /// <summary>SQL名：ワークフロー 通知メール宛先取得</summary>
            public const string WorkFlow_GetMailAddressList = "WorkFlow_GetMailAddressList";
            /// <summary>SQL名：ワークフロー ヘッダ登録処理</summary>
            public const string WorkFlowHeader_Insert = "WorkFlowHeader_Insert";
            /// <summary>SQL名：ワークフロー 詳細登録処理</summary>
            public const string WorkFlowDetail_Insert = "WorkFlowDetail_Insert";
            /// <summary>SQL名：ワークフロー 詳細削除処理</summary>
            public const string WorkFlowDetail_DeleteByWfNo = "WorkFlowDetail_DeleteByWfNo";
            /// <summary>SQL名：通知情報 登録処理</summary>
            public const string NoticeInfo_Insert = "NoticeInfo_Insert";
            /// <summary>SQL名：通知情報 削除処理</summary>
            public const string NoticeInfo_Delete = "NoticeInfo_Delete";
            /// <summary>SQL名：通知情報 削除処理 キー番号単位</summary>
            public const string NoticeInfo_DeleteByKeyNo = "NoticeInfo_DeleteByKeyNo";

            ///// <summary>ワークフロー関連SQL格納フォルダ</summary>
            //public const string SubDir = "WorkFlow";
            /// <summary>ワークフローNoのシーケンスのキー</summary>
            public const string Key_WorkflowNo_Seq = "WORKFLOW_NO";
            /// <summary>通知番号のシーケンスキー</summary>
            public const string Key_NoticeNo_Seq = "NOTICE_NO";

            /// <summary>グローバルリストのキー：承認依頼の戻り値 </summary>
            public const string GlobalDataKeyRequest = "Key_WfRequestValue";
            /// <summary>グローバルリストのキー：承認系の戻り値 </summary>
            public const string GlobalDataKeyApproval = "Key_WfApprovalValue";

            /// <summary>通知内容 付加共通文字取得用コード</summary>
            public const string NoticeContentsUtilityCd = "NCDM";
            /// <summary>通知内容 付加共通デフォルト文字</summary>
            public const string NoticeContentsDef = "伝票番号";
        }

        /// <summary>
        /// エクセル入出力
        /// </summary>
        public class ComReport
        {
            /// <summary>入出力方式：単一セル</summary>
            public const int SingleCell = 1;
            /// <summary>入出力方式：縦方向連続</summary>
            public const int LongitudinalDirection = 2;
            /// <summary>入出力方式：横方向連続</summary>
            public const int LateralDirection = 3;

            /// <summary>セルタイプ：文字列</summary>
            public const int InputTypeStr = 1;
            /// <summary>セルタイプ：数値</summary>
            public const int InputTypeNum = 2;
            /// <summary>セルタイプ：日付</summary>
            public const int InputTypeDat = 3;

            /// <summary>０落ち対応 フォーマット</summary>
            public const string StrFormat = "@";

            /// <summary>SQL名：ファイル入出力項目定義情報取得用SQL</summary>
            public const string GetComReportInfo = "ComInoutItemDefine_GetInfo";
        }

        /// <summary>
        /// ビュー指定 ※検索系 or 登録系
        /// </summary>
        public enum TargetView : int
        {
            /// <summary>検索系</summary>
            Search,
            /// <summary>登録系</summary>
            Regist
        }
        #endregion

        #region private 変数
        /// <summary>ログ出力</summary>
        private static CommonLogger logger = CommonLogger.GetInstance("logger");
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public APCommonUtil()
        {
        }
        #endregion

        /// <summary>
        /// 名称マスタ取得
        /// </summary>
        /// <param name="nameDivision">名称区分</param>
        /// <param name="nameCd">名称コード</param>
        /// <param name="index">インデックス(1:名称01、2:名称02、3:名称03)</param>
        /// <param name="languageId">言語コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得した名称</returns>
        public static string GetName(string nameDivision, string nameCd, int index, string languageId,
            ComDB db)
        {
            // SQL文生成
            var sql = "";
            sql = sql + "select * ";
            sql = sql + "  from v_names "; // ※本来はビューを参照するため、作成できた変更すること
            sql = sql + " where 1 = 1 ";
            if (!string.IsNullOrEmpty(nameCd))
            {
                sql = sql + " and name_cd = @NameCd ";
            }
            if (!string.IsNullOrEmpty(nameDivision))
            {
                sql = sql + " and name_division = @NameDivision ";
            }
            if (!string.IsNullOrEmpty(languageId))
            {
                sql = sql + " and language_id = @LanguageId ";
            }

            try
            {
                // SQL実行
                EntityDao.NamesEntity results = db.GetEntity<EntityDao.NamesEntity>(
                    sql,
                    new
                    {
                        NameCd = nameCd,
                        NameDivision = nameDivision,
                        LanguageId = languageId
                    });

                if (results != null)
                {
                    if (index == 1)
                    {
                        return results.Name01;
                    }
                    else if (index == 2)
                    {
                        return results.Name02;
                    }
                    else if (index == 3)
                    {
                        return results.Name03;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }

            return "";
        }

        /// <summary>
        /// 名称マスタのディクショナリを取得
        /// </summary>
        /// <param name="nameDivisions">名称区分のリスト(検索条件)</param>
        /// <param name="delFlg">削除フラグ(検索条件)</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>名称マスタのディクショナリ</returns>
        [Obsolete("削除予定のため、こちらのメソッドは使用しないでください。")]
        public static Dictionary<string, Dictionary<string, string>> GetNamesDictionary(List<string> nameDivisions, int delFlg, ComDB db)
        {
            // 検索条件の初期化
            dynamic conditionObj = new ExpandoObject();
            conditionObj.NameDivision = "";
            conditionObj.NameCd = "";
            conditionObj.DelFlg = delFlg;

            var namesDictionary = new Dictionary<string, Dictionary<string, string>>();

            for (int i = 0; i < nameDivisions.Count; i++)
            {
                // 検索条件の切替
                conditionObj.NameDivision = nameDivisions[i];

                // 検索実行
                dynamic results = db.GetListByOutsideSqlByDataClass<EntityDao.NamesEntity>("Names_GetKeyList", DirPath, conditionObj);

                // 検索結果を一時Dictionaryに追加
                var tempDictionary = new Dictionary<string, string>();
                foreach (var rowData in results)
                {
                    if (!tempDictionary.ContainsKey(rowData.NameCd))
                    {
                        // 一時Dictionaryに名称コードを取得
                        tempDictionary.Add(rowData.NameCd, "");
                    }
                }

                // 一時Dictionaryを名称区分をキーとして親Dictionaryに追加
                if (!namesDictionary.ContainsKey(nameDivisions[i]))
                {
                    namesDictionary.Add(nameDivisions[i], tempDictionary);
                }
            }

            // 親Dictionaryを返す
            return namesDictionary;
        }

        /*
        /// <summary>
        /// トランザクション使用判定（マスタ）
        /// </summary>
        /// <param name="conductId">機能ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="code">コード</param>
        /// <param name="subCode">サブコード</param>
        /// <param name="flg">フラグ※品目マスタで使用する予定（0:品目、1:品目仕様）</param>
        /// <returns>true:使用中、false:未使用</returns>
        public static bool IsExistsByMaster(string conductId,
            ComDB db, string code = null, string subCode = null, int flg = 0)
        {

            // 機能IDによって処理を分岐
            if (conductId == Constants.CONDUCTID.Item)
            {
                if (flg == 0)
                {
                    // 品目マスタ
                    return proCheckClass.ProCheckItemCd(code, db);
                }
                else if (flg == 1)
                {
                    // 品目仕様マスタ
                    return proCheckClass.ProCheckItemSubCd(code, subCode, db);
                }
            }
            else if (conductId == Constants.CONDUCTID.ItemCategory)
            {
                // 品目分類マスタ
                return proCheckClass.ProCheckItemCategory(code, db);
            }
            else if (conductId == Constants.CONDUCTID.Operation)
            {
                // 工程マスタ
                return proCheckClass.ProCheckOperationCd(code, db);
            }
            else if (conductId == Constants.CONDUCTID.OperationGroup)
            {
                // 工程グループマスタ
                return proCheckClass.ProCheckOperationGroupCd(code, db);
            }
            else if (conductId == Constants.CONDUCTID.OperationPattern)
            {
                // 工程パターンマスタ
                // ※工程パターンはどのテーブルからも使用されていないため、固定でfalseを返す
                return false;
            }
            else if (conductId == Constants.CONDUCTID.RecipeResouce)
            {
                // 設備マスタ
                return proCheckClass.ProCheckResouceCd(code, db);
            }
            else if (conductId == Constants.CONDUCTID.RecipeResouceGroup)
            {
                // 設備グループマスタ
                return proCheckClass.ProCheckResouceGroupCd(code, db);
            }
            else if (conductId == Constants.CONDUCTID.Line)
            {
                // 生産ラインマスタ
                return proCheckClass.ProCheckProductionLine(code, db);
            }
            else if (conductId == Constants.CONDUCTID.Location)
            {
                // ロケーションマスタ
                //return proCheckLocationCd.Exec(code);
                return proCheckClass.ProCheckLocationCd(code, db);
            }
            else if (conductId == Constants.CONDUCTID.Bank)
            {
                // 銀行マスタ
                return proCheckClass.ProCheckBankMasterCd(code, db);
            }
            //else if (conductId == PlanaAPConstants.CONDUCTID.Accounts)
            //{
            //    // 科目マスタ
            //}
            else if (conductId == Constants.CONDUCTID.Balance)
            {
                // 帳合マスタ
                return proCheckClass.ProCheckBalanceCd(code, db);
            }
            else if (conductId == Constants.CONDUCTID.Area)
            {
                // 地区マスタ
                return proCheckClass.ProCheckAreaCd(code, db);
            }
            else if (conductId == Constants.CONDUCTID.Delivery)
            {
                // 納入先マスタ
                return proCheckClass.ProCheckDeliveryCd(code, db);
            }
            //else if (conductId == PlanaAPConstants.CONDUCTID.Carry)
            //{
            //    // 運送会社マスタ
            //}
            else if (conductId == Constants.CONDUCTID.Names)
            {
                // 名称マスタ
                return proCheckClass.ProCheckNameCd(code, subCode, db);
            }
            else if (conductId == Constants.CONDUCTID.Reason)
            {
                // 理由マスタ
                return proCheckClass.ProCheckRyCd(code, db);
            }
            else if (conductId == Constants.CONDUCTID.Login)
            {
                // 担当者マスタ
                return proCheckClass.ProCheckTantoCd(code, db);
            }
            else if (conductId == Constants.CONDUCTID.Organization)
            {
                // 部署マスタ
                return proCheckClass.ProCheckOrganizationCd(code, db);
            }
            else if (conductId == Constants.CONDUCTID.Post)
            {
                // 役職マスタ
                return proCheckClass.ProCheckPostId(code, db);
            }
            else if (conductId == Constants.CONDUCTID.Role)
            {
                // ロールマスタ
                return proCheckClass.ProCheckRoleId(code, db);
            }
            else if (conductId == Constants.CONDUCTID.CurrencyCtlCtl)
            {
                // 多通貨マスタ
                return proCheckClass.ProCheckCurrencyCd(code, subCode, db);
            }
            else if (conductId == Constants.CONDUCTID.vender)
            {
                // 取引先マスタ
                return proCheckClass.ProCheckVenderCd(code, subCode, db);
            }
            else
            {
                return true;
            }

            return true;
        }

        /// <summary>
        /// トランザクション使用判定（BOM）
        /// </summary>
        /// <param name="bomCd">BOMコード</param>
        /// <param name="bomVersion">BOMバージョン</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:使用中、false:未使用</returns>
        public static bool IsExistsByBom(string bomCd, decimal bomVersion,
            ComDB db)
        {
            return proCheckClass.ProCheckBomCd(bomCd, bomVersion, db);
        }

        /// <summary>
        /// トランザクション使用判定（取引先データ）
        /// </summary>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="venderDivision">取引先区分</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>-1:例外エラー、1:取引先データなし、2:請求先データなし、3:請求先の消費税情報なし、0:その他</returns>
        public static int IsExistsByVender(string venderCd, string venderDivision,
            ComDB db)
        {
            return proCheckClass.ProCheckVenderTax(venderCd, venderDivision, db);
        }
        */
        /// <summary>
        /// スポット区分を取得
        /// </summary>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specCd">品目仕様コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得したスポット区分</returns>
        public static string GetSpotDivision(string itemCd, string specCd,
            ComDB db)
        {
            return GetStringData("1", itemCd, specCd, null, null, null, null, null, null, null, null, db);
        }

        /// <summary>
        /// 入金の締め処理フラグを取得
        /// </summary>
        /// <param name="creditNo">入金番号</param>
        /// <param name="creditDate">入金日</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常 -1:月次・売掛共に処理済み -2:月次のみ締め処理済み -3:売掛のみ締め処理済み -9:その他エラー</returns>
        public static string GetCloseResultCredit(string creditNo, string creditDate,
            ComDB db)
        {
            return GetStringData("19", creditNo, creditDate, null, null, null, null, null, null, null, null, db);
        }

        /// <summary>
        /// 売上・仕入の区分から消費税コードを取得する
        /// </summary>
        /// <param name="category">用途(SALES:売上 STOCKING:仕入 CREDIT:入金)</param>
        /// <param name="taxDate">受入日/売上日</param>
        /// <param name="taxCategory">消費税率</param>
        /// <param name="taxDivision">課税区分</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>消費税コード</returns>
        public static string GetTaxCdFromDate(string category, string taxDate, string taxCategory, string taxDivision,
            ComDB db)
        {
            return GetStringData("20", category, taxDate, taxCategory, taxDivision, null, null, null, null, null, null, db);
        }

        /// <summary>
        /// 取引先からレート適用開始日を取得する
        /// </summary>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="venderDivision">取引先区分</param>
        /// <param name="procDate">レート適用開始日</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得したレート適用開始日</returns>
        public static string GetExValidDateFromVender(string venderCd, string venderDivision, string procDate,
            ComDB db)
        {
            return GetStringData("300", venderCd, venderDivision, procDate, null, null, null, null, null, null, null, db);
        }

        /// <summary>
        /// 取引先から通貨レートを取得する
        /// </summary>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="venderDivision">取引先区分</param>
        /// <param name="procDate">レート適用開始日</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得した通貨レート</returns>
        public static string GetExRateFromVender(string venderCd, string venderDivision, string procDate,
            ComDB db)
        {
            return GetStringData("400", venderCd, venderDivision, procDate, null, null, null, null, null, null, null, db);
        }

        /// <summary>
        /// 入金番号がキャンセル済みかチェックする
        /// </summary>
        /// <param name="creditNo">入金番号</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取消チェック結果(0:未取消 -1:取消済)</returns>
        public static int CheckCancelCredit(string creditNo,
            ComDB db)
        {
            return GetNumericData("6", creditNo, null, null, null, null, null, null, null, null, null, db);
        }

        /// <summary>
        /// 自己承認チェック
        /// </summary>
        /// <param name="loginCd">ログイン担当者コード</param>
        /// <param name="repuestCd">承認依頼者</param>
        /// <param name="approvalCd">承認者</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:承認OK false:自己承認できない為エラー</returns>
        public static string CheckSelfApproval(string loginCd, string repuestCd, string approvalCd, ComDB db)
        {
            string messages = "";
            string str = "";

            try
            {
                // ログイン担当者を検索
                EntityDao.LoginEntity bean = masterCheckUtil.GetEntity<EntityDao.LoginEntity>("login", ref str, new string[] { loginCd }, db);
                if (bean != null)
                {
                    // テストユーザの場合、自己承認可能
                    if (bean.TestUserFlg != null &&
                        bean.TestUserFlg == "1")
                    {
                        return messages;
                    }

                    // 承認者と承認依頼者が異なる場合はOK
                    if (repuestCd == approvalCd)
                    {
                        //messages = string.Format(Resources.M70001);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }

            return messages;
        }

        // 20211105 ロット付帯情報ヘッダのレイアウト変更に伴い、ロット在庫より使用列が削除、ビルドエラーとなるのでコメントアウト
        /// <summary>
        /// 在庫の品質保証期限をチェックする
        /// </summary>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specCd">品目仕様コード</param>
        /// <param name="lotNo">ロット番号</param>
        /// <param name="subLot1">サブロット番号1</param>
        /// <param name="subLot2">サブロット番号2</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>-1:ロット在庫なし -2:出荷止めステータスである -3;品質保証期限切れ</returns>
        public static int CheckInventoryQuality(string itemCd, string specCd, string lotNo, string subLot1, string subLot2, ComDB db)
        {
            // NGロケーションコード
            //var ngLocationCd = locationCd;
            var str = "";

            // ロケーション検索
            try
            {
                //EntityDao.LocationEntity beanLocation = masterCheckUtil.GetEntity<EntityDao.LocationEntity>("location", ref str, new string[] { ngLocationCd }, db);
                //if (beanLocation == null)
                //{
                //    // NGロケーションコードが未登録の場合は元のロケーションコードを使用する
                //    ngLocationCd = locationCd;
                //}

                // ロット別在庫を検索
                //EntityDao.LotInventoryEntity lot = masterCheckUtil.GetEntity<EntityDao.LotInventoryEntity>("lot_inventory", ref str, new string[] { itemCd, specCd, ngLocationCd, lotNo }, db);
                EntityDao.LotExtInfoHeadEntity lot = masterCheckUtil.GetEntity<EntityDao.LotExtInfoHeadEntity>("lot_ext_info_head", ref str, new string[] { itemCd, specCd, lotNo, subLot1, subLot2 }, db);
                if (lot == null)
                {
                    // 在庫なし
                    return -1;
                }
                else
                {
                    if (lot.StatusShipping != null &&
                        lot.StatusShipping.ToString() == "9")
                    {
                        return -2;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 在庫情報取得処理
        /// </summary>
        public static class GetInventoryInfo
        {
            /// <summary>検索条件：品目仕様在庫</summary>
            /// <remarks>ロケーション、ロットはこれを継承し、キーを増やす</remarks>
            public class KeyItemSpecification
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }

                /// <summary>内部処理用：自身の値を検索条件として出力する</summary>
                /// <returns>品目コード、仕様コードをシングルクォーテーションで囲って、カンマ区切りで返す</returns>
                public virtual string GetKeyValues()
                {
                    return AddQuotes(this.ItemCd) + AddCommaQuotes(this.SpecificationCd);
                }

                /// <summary>内部処理用：自身の値を検索条件として出力する</summary>
                /// <param name="index">インデックス</param>
                /// <param name="param">条件</param>
                /// <returns>品目コード、仕様コードをシングルクォーテーションで囲って、カンマ区切りで返す</returns>
                public virtual string GetKeyValues(int index, ref object param)
                {
                    ((IDictionary<string, object>)param).Add("ItemCd" + index, this.ItemCd);
                    ((IDictionary<string, object>)param).Add("SpecicationCd" + index, this.SpecificationCd);
                    return "@ItemCd" + index + ", @SpecicationCd" + index;
                }

                /// <summary>内部処理用：ダブルクォーテーションで囲う</summary>
                /// <param name="value">囲う値</param>
                /// <returns>ダブルクォーテーションで囲った値</returns>
                protected string AddQuotes(string value)
                {
                    return "'" + value + "'";
                }

                /// <summary>内部処理用：ダブルクォーテーションで囲ってカンマを先頭に付ける</summary>
                /// <param name="value">囲う値</param>
                /// <returns>ダブルクォーテーションで囲ってカンマを付けた値</returns>
                protected string AddCommaQuotes(string value)
                {
                    return "," + AddQuotes(value);
                }
            }
            /// <summary>検索条件：ロケーション在庫</summary>
            public class KeyLocation : KeyItemSpecification
            {
                /// <summary>Gets or sets ロケーションコード</summary>
                /// <value>ロケーションコード</value>
                public string LocationCd { get; set; }

                /// <summary>内部処理用：自身の値を検索条件として出力する</summary>
                /// <returns>品目仕様にロケーションを加える</returns>
                public override string GetKeyValues()
                {
                    return base.GetKeyValues() + AddCommaQuotes(this.LocationCd);
                }

                /// <summary>内部処理用：自身の値を検索条件として出力する</summary>
                /// <param name="index">インデックス</param>
                /// <param name="param">条件</param>
                /// <returns>品目コード、仕様コードをシングルクォーテーションで囲って、カンマ区切りで返す</returns>
                public override string GetKeyValues(int index, ref object param)
                {
                    ((IDictionary<string, object>)param).Add("LocationCd" + index, this.LocationCd);
                    return base.GetKeyValues(index, ref param) + ", @LocationCd" + index;
                }
            }
            /// <summary>検索条件：ロット在庫</summary>
            public class KeyLot : KeyLocation
            {
                /// <summary>Gets or sets ロット番号</summary>
                /// <value>ロット番号</value>
                public string LotNo { get; set; }
                /// <summary>Gets or sets サブロット番号1</summary>
                /// <value>サブロット番号1</value>
                public string SubLotNo1 { get; set; }
                /// <summary>Gets or sets サブロット番号2</summary>
                /// <value>サブロット番号2</value>
                public string SubLotNo2 { get; set; }

                /// <summary>内部処理用：自身の値を検索条件として出力する</summary>
                /// <returns>ロケーションにロットサブロットを加える</returns>
                public override string GetKeyValues()
                {
                    return base.GetKeyValues() + AddCommaQuotes(this.LotNo) + AddCommaQuotes(this.SubLotNo1) + AddCommaQuotes(this.SubLotNo2);
                }

                /// <summary>内部処理用：自身の値を検索条件として出力する</summary>
                /// <param name="index">インデックス</param>
                /// <param name="param">条件</param>
                /// <returns>品目コード、仕様コードをシングルクォーテーションで囲って、カンマ区切りで返す</returns>
                public override string GetKeyValues(int index, ref object param)
                {
                    ((IDictionary<string, object>)param).Add("LotNo" + index, this.LotNo);
                    ((IDictionary<string, object>)param).Add("SubLotNo1" + index, this.SubLotNo1);
                    ((IDictionary<string, object>)param).Add("SubLotNo2" + index, this.SubLotNo2);
                    return base.GetKeyValues(index, ref param) + ", @LotNo" + index + ", @SubLotNo1" + index + ", @SubLotNo2" + index;
                }
            }
            /// <summary>戻り値：品目仕様在庫</summary>
            public class ItemSpecificationInfo : KeyItemSpecification
            {
                /// <summary>Gets or sets 有効在庫数</summary>
                /// <value>有効在庫数</value>
                public decimal AvailableQty { get; set; }
                /// <summary>Gets or sets 現在在庫数</summary>
                /// <value>現在在庫数</value>
                public decimal InventoryQty { get; set; }
                // 取得する項目を増やしたければ、このクラスに追加する
            }
            /// <summary>戻り値：ロケーション在庫</summary>
            public class LocationInfo : ItemSpecificationInfo
            {
                /// <summary>Gets or sets ロケーションコード</summary>
                /// <value>ロケーションコード</value>
                public string LocationCd { get; set; }
            }
            /// <summary>戻り値：ロット在庫</summary>
            public class LotInfo : LocationInfo
            {
                /// <summary>Gets or sets ロット番号</summary>
                /// <value>ロット番号</value>
                public string LotNo { get; set; }
                /// <summary>Gets or sets サブロット番号1</summary>
                /// <value>サブロット番号1</value>
                public string SubLotNo1 { get; set; }
                /// <summary>Gets or sets サブロット番号2</summary>
                /// <value>サブロット番号2</value>
                public string SubLotNo2 { get; set; }
            }

            /// <summary>
            /// 在庫ビューの種類
            /// </summary>
            /// <remarks>品目仕様＜ロケーション＜ロットとキーが増えるため、値の大小関係を利用する</remarks>
            private enum InventoryType
            {
                /// <summary>品目仕様</summary>
                ItemSpecification = 0,
                /// <summary>ロケーション</summary>
                Location = 1,
                /// <summary>ロット</summary>
                Lot = 2
            }

            /// <summary>
            /// 品目仕様在庫の情報を取得
            /// </summary>
            /// <param name="paramList">キーとなる品目仕様のリスト</param>
            /// <param name="db">DB接続</param>
            /// <returns>品目仕様在庫の情報</returns>
            public static IList<ItemSpecificationInfo> GetItemInventory(List<KeyItemSpecification> paramList, ComDB db)
            {
                return getInventoryInfoList<ItemSpecificationInfo, KeyItemSpecification>(paramList, InventoryType.ItemSpecification, db);
            }

            /// <summary>
            /// ロケーション在庫の情報を取得
            /// </summary>
            /// <param name="paramList">キーとなる品目仕様ロケーションのリスト</param>
            /// <param name="db">DB接続</param>
            /// <returns>ロケーション在庫の情報</returns>
            public static IList<LocationInfo> GetLocationInventory(List<KeyLocation> paramList, ComDB db)
            {
                return getInventoryInfoList<LocationInfo, KeyLocation>(paramList, InventoryType.Location, db);
            }
            /// <summary>
            /// ロット在庫の情報を取得
            /// </summary>
            /// <param name="paramList">キーとなる品目仕様ロケーションロットのリスト</param>
            /// <param name="db">DB接続</param>
            /// <returns>ロット在庫の情報</returns>
            public static IList<LotInfo> GetLotInventory(List<KeyLot> paramList, ComDB db)
            {
                return getInventoryInfoList<LotInfo, KeyLot>(paramList, InventoryType.Lot, db);
            }

            /// <summary>
            /// 在庫ビューのキーの列を取得
            /// </summary>
            /// <param name="type">在庫ビューの種類</param>
            /// <returns>SQLで使用するキーの列の文字列</returns>
            private static string getSqlInventoryKey(InventoryType type)
            {
                string key = "item_cd , specification_cd";
                if (type > InventoryType.ItemSpecification)
                {
                    key += " , location_cd";
                }
                if (type > InventoryType.Location)
                {
                    key += " , lot_no , sub_lot_no_1 , sub_lot_no_2";
                }
                return key;
            }

            /// <summary>
            /// 在庫ビューより情報を取得するSQLを作成し、実行する処理
            /// </summary>
            /// <typeparam name="T">在庫情報の取得結果のクラス</typeparam>
            /// <typeparam name="TS">在庫情報のキーのクラス</typeparam>
            /// <param name="paramList">在庫情報のキーのリスト</param>
            /// <param name="type">在庫ビューの種類</param>
            /// <param name="db">DB接続</param>
            /// <returns>在庫情報の取得結果</returns>
            private static IList<T> getInventoryInfoList<T, TS>(List<TS> paramList, InventoryType type, ComDB db)
                where T : ItemSpecificationInfo // 結果：品目仕様に基づいた型
                where TS : KeyItemSpecification  // キー：品目仕様に基づいた型
            {
                string sql = "";
                sql += " select ";
                // キーを追加(検索結果)
                sql += getSqlInventoryKey(type);
                sql += " , available_qty ";
                sql += " , inventory_qty ";
                sql += " from ";
                // ビューの種類に応じて取得ビューを変更
                switch (type)
                {
                    case InventoryType.ItemSpecification:
                        sql += " v_item_inventory ";
                        break;
                    case InventoryType.Location:
                        sql += " v_location_inventory ";
                        break;
                    case InventoryType.Lot:
                        sql += " v_lot_inventory ";
                        break;
                    default:
                        return null;
                }
                // 引数リストが空でない場合、WHERE句を設定
                dynamic param = new ExpandoObject();
                if (paramList != null && paramList.Count > 0)
                {
                    sql += " where ( ";
                    // キーを追加(検索条件)
                    sql += getSqlInventoryKey(type);
                    sql += " ) in ( ";
                    // 検索キーの値を繰り返し設定
                    for (int i = 0; i < paramList.Count; i++)
                    {
                        if (i > 0)
                        {
                            sql += ",";
                        }
                        // カンマ区切りで値を取得
                        sql += "(" + paramList[i].GetKeyValues(i, ref param) + ")";
                    }
                    sql += " ) ";
                }
                sql += " order by ";
                // キーを追加(表示順)
                sql += getSqlInventoryKey(type);

                // SQL実行
                //IList<T> resultList = db.GetListByDataClass<T>(sql);
                IList<T> resultList = db.GetListByDataClass<T>(sql, param);
                return resultList;
            }
        }

        /// <summary>
        /// 月次受払更新を行っているか判定する処理
        /// </summary>
        /// <param name="procDate">勘定年月</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:未処理 1:処理済</returns>
        public static string CheckMonthlyInoutPorc(string procDate, ComDB db)
        {
            return GetStringData("9", "M_INOUT", null, procDate, null, null, null, null, null, null, null, db);
        }

        /// <summary>
        /// 国内番号を国際番号に変換する
        /// </summary>
        /// <param name="telNo">電話番号またはFAX番号</param>
        /// <param name="isHyphenDelete">true:文字列中のハイフンを消去</param>
        /// <param name="isHeaderPlus">true:先頭に「+」を挿入</param>
        /// <returns>国際番号</returns>
        public static string ConvertInterNo(string telNo, bool isHyphenDelete, bool isHeaderPlus)
        {
            string convertTelNo = telNo;
            if (convertTelNo != null && !convertTelNo.Equals(string.Empty))
            {
                // 先頭が0であれば0を消去する
                if (convertTelNo.Substring(0, 1).CompareTo("0") == 0)
                {
                    convertTelNo = convertTelNo.Substring(1);
                }
                // 先頭に日本の国際番号「81」を追加
                convertTelNo = Integer_No_Japan + convertTelNo;
                // 文字列中のハイフンを消去
                if (isHyphenDelete)
                {
                    convertTelNo = convertTelNo.Replace(Hyphen, "");
                }
                // 先頭に「+」を挿入
                if (isHeaderPlus)
                {
                    convertTelNo = Tel_Hader_Plus + convertTelNo;
                }
            }
            return convertTelNo;
        }

        /// <summary>
        /// 取引先コードから部署コードを取得する
        /// </summary>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="venderDivision">取引先区分</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得した部署コード</returns>
        public static string GetOrganizationCdFromVender(string venderCd, string venderDivision, ComDB db)
        {
            return GetStringData("500", venderCd, venderDivision, null, null, null, null, null, null, null, null, db);
        }

        /// <summary>
        /// デフォルトロット番号取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <returns>デフォルトロット番号</returns>
        public static string GetDefaultLotNo(ComDB db)
        {
            try
            {
                EntityDao.CompanyEntity bean = new EntityDao.CompanyEntity();
                bean = bean.GetEntity(Company_Cd, db);

                if (bean == null)
                {
                    return "";
                }
                else
                {
                    return bean.DefaultLotNo;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 桁数制御情報を取得します
        /// </summary>
        /// <param name="request">リクエスト</param>
        public static void GetNumberChkdisit(HttpSessionStateBase request)
        {

        }

        /// <summary>
        /// 文字列項目取得用 ※ここは引数の区分毎に取得する値を変えています。新規に追加する場合は区分を重複せずまた区分を変えないようにお願いします
        /// </summary>
        /// <param name="division">処理区分</param>
        /// <param name="param01">文字型拡張パラメータ1</param>
        /// <param name="param02">文字型拡張パラメータ2</param>
        /// <param name="param03">文字型拡張パラメータ3</param>
        /// <param name="param04">文字型拡張パラメータ4</param>
        /// <param name="param05">文字型拡張パラメータ5</param>
        /// <param name="param06">文字型拡張パラメータ6</param>
        /// <param name="param07">文字型拡張パラメータ7</param>
        /// <param name="param08">文字型拡張パラメータ8</param>
        /// <param name="param09">文字型拡張パラメータ9</param>
        /// <param name="param10">文字型拡張パラメータ10</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得したデータ</returns>
        public static string GetStringData(string division, string param01, string param02, string param03, string param04,
            string param05, string param06, string param07, string param08, string param09, string param10,
            ComDB db)
        {
            // 戻り値宣言
            var stringData = "";

            // SQL変数宣言
            var sql = "";

            switch (division)
            {
                case "1":
                    // スポット区分を取得する
                    int? data = apStoredFncUtil.FncGetSpotDivision(param01, param02, db);
                    if (data != null)
                    {
                        stringData = data.ToString();
                    }
                    else
                    {
                        stringData = null;
                    }
                    break;
                case "9":
                    // 締め処理済みフラグを取得する
                    DateTime procDate = DateTime.Parse(param03);
                    DateTime execDate;
                    DateTime closeDateFrom;
                    DateTime closeDateTo;

                    DateTime.TryParse(param03, out procDate);
                    DateTime.TryParse(param04, out execDate);
                    DateTime.TryParse(param05, out closeDateFrom);
                    DateTime.TryParse(param06, out closeDateTo);

                    int? chkRtn = apStoredFncUtil.FncGetCloseRsult(param01, param02, procDate, execDate, closeDateFrom, closeDateTo, param07, db);

                    if (chkRtn != null)
                    {
                        stringData = chkRtn.ToString();
                    }
                    else
                    {
                        stringData = null;
                    }
                    break;
                case "19":
                    // 入金から締め処理フラグが未処理であるかチェックする
                    DateTime procCreditDate = DateTime.Parse(param02);

                    DateTime.TryParse(param02, out procCreditDate);

                    int? chkCreditRtn = apStoredFncUtil.FncGetCloseResultCredit(param01, procCreditDate, db);
                    if (chkCreditRtn != null)
                    {
                        stringData = chkCreditRtn.ToString();
                    }
                    else
                    {
                        stringData = null;
                    }
                    break;
                case "20":
                    // 受入日/売上日から消費税コードを取得する
                    string taxCdRtn = apStoredFncUtil.FncGetTaxCdFromDate(param01, param02, param03, null, null, null, null, null, param04, db);
                    if (taxCdRtn != null)
                    {
                        stringData = taxCdRtn.ToString();
                    }
                    else
                    {
                        stringData = null;
                    }
                    break;
                case "300":
                    // 仕入先と仕入先区分からレート適用開始日を取得する
                    // SQL文生成
                    sql = sql + "select to_char(max(cur2.ex_valid_date), 'yyyy/mm/dd') as ex_valid_date ";
                    sql = sql + "  from (select vender.currency_code ";
                    sql = sql + "          from vender_queue vender ";
                    sql = sql + "         where vender.vender_division = @Param2 ";
                    sql = sql + "           and vender.vender_cd = @Param1 ";
                    sql = sql + "       ) ven ";
                    sql = sql + "       left outer join currency_ctl_ctl cur2 on ven.currency_code = cur2.currency_code ";
                    sql = sql + " where to_char(cur2.ex_valid_date, 'yyyy/mm/dd') <= @Param3 ";
                    sql = sql + " group by cur2.currency_code";
                    break;
                case "400":
                    // 仕入先と仕入先区分から通貨レートを取得する
                    // SQL文生成
                    sql = sql + "select cur2.ex_rate ";
                    sql = sql + "  from (select vender.currency_code ";
                    sql = sql + "          from vender_queue vender ";
                    sql = sql + "         where vender.vender_division = @Param2 ";
                    sql = sql + "           and vender.vender_cd = @Param1 ";
                    sql = sql + "       ) ven ";
                    sql = sql + "       left outer join currency_ctl_ctl cur2 on ven.currency_code = cur2.currency_code ";
                    sql = sql + " where to_char(cur2.ex_valid_date, 'yyyy/mm/dd') <= @Param3 ";
                    sql = sql + " group by cur2.ex_rate";
                    break;
                case "500":
                    // 仕入先と仕入先区分から担当部署を取得する
                    // SQL文生成
                    sql = sql + "select vender.organization_cd ";
                    sql = sql + "  from vender_queue vender ";
                    sql = sql + " where vender.vender_division = @Param2 ";
                    sql = sql + "   and vender.vender_cd = @Param1 ";
                    break;
                default:
                    break;
            }

            try
            {
                if (sql != "")
                {
                    // SQL実行
                    EntityDao.DepositHeaderEntity results = db.GetEntity<EntityDao.DepositHeaderEntity>(
                        sql,
                        new
                        {
                            Param1 = param01,
                            Param2 = param02,
                            Param3 = param03
                        });

                    if (results != null)
                    {
                        if (division == "300")
                        {
                            stringData = results.ExValidDate.ToString();
                        }
                        else if (division == "400")
                        {
                            stringData = results.ExRate.ToString();
                        }
                        else if (division == "500")
                        {
                            stringData = results.OrganizationCd;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }

            return stringData;
        }

        /// <summary>
        /// 数値項目取得用 ※ここは引数の区分毎に取得する値を変えています。新規に追加する場合は区分を重複せずまた区分を変えないようにお願いします
        /// </summary>
        /// <param name="division">処理区分</param>
        /// <param name="param01">文字型拡張パラメータ1</param>
        /// <param name="param02">文字型拡張パラメータ2</param>
        /// <param name="param03">文字型拡張パラメータ3</param>
        /// <param name="param04">文字型拡張パラメータ4</param>
        /// <param name="param05">文字型拡張パラメータ5</param>
        /// <param name="param06">文字型拡張パラメータ6</param>
        /// <param name="param07">文字型拡張パラメータ7</param>
        /// <param name="param08">文字型拡張パラメータ8</param>
        /// <param name="param09">文字型拡張パラメータ9</param>
        /// <param name="param10">文字型拡張パラメータ10</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得したデータ</returns>
        public static int GetNumericData(string division, string param01, string param02, string param03, string param04,
            string param05, string param06, string param07, string param08, string param09, string param10,
            ComDB db)
        {
            // 戻り値宣言
            var intData = -1;

            switch (division)
            {
                case "6":
                    // 入金番号が取消済かチェックする  0:未取消 -1:取消済
                    intData = apStoredFncUtil.FncCheckCanselCredit(param01, db);
                    break;
                default:
                    break;
            }
            return intData;
        }

        /// <summary>
        /// プルーフ登録処理
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="db">DB操作クラス</param>
        /// <param name="condition">パラメータ情報</param>
        /// <param name="sqlName">SQLファイル名</param>
        /// <param name="tantoCd">担当者コード</param>
        /// <param name="status">プルーフステータス</param>
        /// <returns>true:正常終了、false:異常終了</returns>
        public static bool CreateProof<T>(ComDB db, T condition, string sqlName, string tantoCd, decimal status)
        {
            // 共通更新データを設定
            EntityDao.CommonProofItem proof = new BaseClass.CommonProofItem();
            proof.SysDate = DateTime.Now;
            proof.TantoCd = tantoCd;
            proof.ProofStatus = status;

            // 引数の条件と、共通データを統合し、条件を再作成する
            dynamic tmpCon = new ExpandoObject();

            // 条件クラスのプロパティを列挙
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                // テーブル名の場合、スキップ
                if (property.Name.Equals("TableName"))
                {
                    continue;
                }
                ((IDictionary<string, object>)tmpCon).Add(property.Name, property.GetValue(condition));
            }

            // 共通データを統合
            var comProperties = proof.GetType().GetProperties();
            foreach (var comProperty in comProperties)
            {
                ((IDictionary<string, object>)tmpCon).Add(comProperty.Name, comProperty.GetValue(proof));
            }

            // 登録処理を行う
            int result = db.RegistByOutsideSql(sqlName, DirPathProof, tmpCon);

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// シーケンスにて連番を発番
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="seqName">連番処理名称</param>
        /// <returns>シーケンス番号(※取得できない場合は空文字を返す)</returns>
        public static string GetNumber(ComDB db, string seqName)
        {
            return apStoredPrcUtil.ProSeqGetNo(db, seqName);
        }

        /// <summary>
        /// 基準日から稼働日数を±した日付を取得する
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="baseDay">基準日</param>
        /// <param name="addDay">増減日</param>
        /// <param name="calCd">カレンダーコード</param>
        /// <returns>値が取得できない場合、そのまま返す</returns>
        /// <remarks>増減日が0の場合、１営業日後を返す</remarks>
        public static DateTime GetWorkingDay(ComDB db, DateTime baseDay, decimal addDay, string calCd)
        {
            // SQL文を生成
            string sql = string.Empty;
            DateTime returnDate = baseDay;

            if (addDay >= 0)
            {
                if (addDay == 0)
                {
                    addDay = 1;
                }
                sql = sql + "select max(cal.cal_date) as cal_date ";
                sql = sql + "  from ( ";
                sql = sql + "     select cal_date, ";
                sql = sql + "            row_number() over (order by cal_date) as rownum ";
                sql = sql + "       from cal ";
                sql = sql + "      where cal_cd = @CalCd ";
                if (addDay > 0)
                {
                    sql = sql + "        and cal_date > @CalDate ";
                }
                else
                {
                    sql = sql + "        and cal_date >= @CalDate ";
                }
                sql = sql + "        and cal_holiday = 0 "; // 0:平日
                sql = sql + "       ) cal ";
                sql = sql + " where cal.rownum <= @AddDay";
            }
            else
            {
                addDay = Math.Abs(addDay);
                sql = sql + "select min(cal.cal_date) as cal_date ";
                sql = sql + "  from ( ";
                sql = sql + "     select cal_date, ";
                sql = sql + "            row_number() over (order by cal_date desc) as rownum ";
                sql = sql + "       from cal ";
                sql = sql + "      where cal_cd = @CalCd ";
                sql = sql + "        and cal_date < @CalDate ";
                sql = sql + "        and cal_holiday = 0 "; // 0:平日
                sql = sql + "       ) cal ";
                sql = sql + " where cal.rownum <= @AddDay";
            }

            // SQL実行
            EntityDao.CalEntity result = db.GetEntity<EntityDao.CalEntity>(sql, new { CalCd = calCd, CalDate = baseDay, AddDay = addDay });
            if (result != null)
            {
                returnDate = result.CalDate != null ? result.CalDate : baseDay;
            }
            return returnDate;
        }

        /// <summary>
        /// 指定日が稼働日かどうか判定する
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="workDate">指定日</param>
        /// <param name="calCd">カレンダーコード</param>
        /// <returns>true:稼働日、false;休日</returns>
        /// <remarks>データが取得できた場合、trueを返却</remarks>
        public static bool GetWorkingCheck(ComDB db, DateTime workDate, string calCd)
        {
            // SQL文を生成
            string sql = string.Empty;

            // 指定日
            if (workDate == null)
            {
                return false;
            }

            sql += "select ";
            sql += "       count(*) as count "; // 件数を取得
            sql += "  from ";
            sql += "       cal "; // カレンダーマスタ
            sql += " where ";
            sql += "       cal_cd = @CalCd "; // カレンダーコード
            sql += "   and ";
            sql += "       cal_holiday = 0 "; // 0:平日(稼働日)
            sql += "   and ";
            sql += "       cal_date = @CalDate "; // 指定日

            // SQL実行
            int result = db.GetEntity<int>(sql, new { CalCd = calCd, CalDate = workDate });

            return result > 0 ? true : false;
        }

        /// <summary>
        /// 対象日付のレートでの単価変換値を返却する(DB格納単価をレートで除算した値を返却)
        /// </summary>
        /// <param name="change">通貨換算前データ</param>
        /// <param name="targetDate">対象日付(yyyy/MM/dd型)</param>
        /// <param name="currencyCd">通貨コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>通貨情報</returns>
        public static EntityDao.CurrencyCtrlSearch GetChangeVal(decimal change, string targetDate, string currencyCd, ComDB db)
        {
            // 通貨マスタ情報取得
            var sql = "";
            sql += "select max_currency.currency_cd as currency_cd, ";
            sql += "       max_currency.currency_symbol, ";
            sql += "       max_currency.country_cd as country_cd, ";
            sql += "       max_currency.ex_valid_date as valid_date, ";
            sql += "       max_currency.ex_rate, ";
            sql += "       max_currency.remark, ";
            sql += "       max_currency.maxdate ";
            sql += "  from (select currency_ctl_ctl.currency_cd, ";
            sql += "               currency_ctl_ctl.currency_symbol, ";
            sql += "               currency_ctl_ctl.country_cd, ";
            sql += "               currency_ctl_ctl.ex_valid_date, ";
            sql += "               to_char(currency_ctl_ctl.ex_valid_date, 'yyyyMMdd') as str_valid_date, ";
            sql += "               currency_ctl_ctl.ex_rate, ";
            sql += "               currency_ctl_ctl.remark, ";
            sql += "               max(to_char(currency_ctl_ctl.ex_valid_date, 'yyyyMMdd')) over(partition by currency_ctl_ctl.currency_cd) as maxdate ";
            sql += "          from currency_ctl_ctl ";
            sql += "         where to_char(currency_ctl_ctl.ex_valid_date, 'yyyy/MM/dd') <= @ValidDate ";
            sql += "       ) max_currency ";
            sql += " where max_currency.str_valid_date = max_currency.maxdate ";
            sql += "   and max_currency.currency_cd = @CurrencyCd ";

            EntityDao.CurrencyCtrlSearch result = new EntityDao.CurrencyCtrlSearch();
            result = db.GetEntityByDataClass<EntityDao.CurrencyCtrlSearch>(sql, new { ValidDate = targetDate, CurrencyCd = currencyCd });

            if (result == null)
            {
                return null;
            }

            decimal chg = decimal.Zero;
            decimal rt = decimal.Zero;

            try
            {
                chg = change;
                rt = result.ExRate;
                chg = decimal.Multiply(chg, rt);
            }
            catch
            {
                chg = change;
            }

            result.CalculatedValue = chg;

            return result;
        }

        /// <summary>
        /// 指定した開始日以前の最新のレート適用開始日を取得
        /// </summary>
        /// <param name="currencyCd">通貨コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="targetDate">対象日付(yyyy/MM/dd) ※省略可（省略の場合、システム日付以前）</param>
        /// <returns>レート適用開始日 ※取得できない場合、nullを戻す</returns>
        public static DateTime? GetExValidDate(string currencyCd, ComDB db, string targetDate = null)
        {
            // 対象日付を設定
            if (ComUtil.ConvertDateTime(targetDate) == null)
            {
                targetDate = ComUtil.ConvertDatetimeToFmtString(DateTime.Now, "yyyy/MM/dd");
            }

            // SQL分を生成
            string sql = string.Empty;
            sql += "select ex_valid_date ";
            sql += "  from currency_ctl_ctl";
            sql += " where currency_cd = @CurrencyCd ";
            sql += "   and to_char(ex_valid_date, 'yyyy/MM/dd') <= @ExValidDate ";
            sql += "   and del_flg = 0 ";
            sql += "order by ex_valid_date desc limit 1";

            // SQL実行
            DateTime? result = db.GetEntityByDataClass<DateTime?>(sql, new { ExValidDate = targetDate, CurrencyCd = currencyCd });

            return result;
        }

        /// <summary>
        /// 検索条件を再設定
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>再設定後の検索条件</returns>
        protected static dynamic SetCondition(Dictionary<string, string> condition)
        {
            dynamic searchCondition = new ExpandoObject();
            if (condition != null)
            {
                foreach (var item in condition)
                {
                    ((IDictionary<string, object>)searchCondition).Add(item.Key, item.Value);
                }
            }
            return searchCondition;
        }

        /// <summary>
        /// Dictionaryリスト用ユーティリティクラス
        /// </summary>
        /// <remarks>resultInfoDictionaryやsearchConditionDictionaryの型List<Dictionary<string,object>>がややこしいので簡略化したい</remarks>
        public class DictionaryList
        {
            /// <summary>Gets ディクショナリのリスト</summary>
            /// <value>ディクショナリのリスト</value>
            public List<Dictionary<string, object>> Value { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="dictionaryList">利用するディクショナリリストの値</param>
            public DictionaryList(List<Dictionary<string, object>> dictionaryList)
            {
                // 複製でなくそのまま参照するので、このクラスでの変更は反映される
                this.Value = dictionaryList;
            }

            /// <summary>
            /// ctrlIdを指定して要素を取得
            /// </summary>
            /// <param name="ctrlId">項目ID</param>
            /// <returns>Listから取得した要素</returns>
            public Dictionary<string, object> GetMember(string ctrlId)
            {
                return GetDictionaryByCtrlId(this.Value, ctrlId);
            }
        }

        /// <summary>
        /// 画面のコントロールIDで対象のディクショナリを取得
        /// </summary>
        /// <param name="dictionaryList">ディクショナリのリスト、searchConditionDictionaryやresultInfoDictionary</param>
        /// <param name="ctrlId">取得対象の画面のコントロールID</param>
        /// <returns>対象のディクショナリ</returns>
        public static Dictionary<string, object> GetDictionaryByCtrlId(List<Dictionary<string, object>> dictionaryList, string ctrlId)
        {
            // 先頭
            return dictionaryList.FirstOrDefault(x => ctrlId.Equals(x["CTRLID"].ToString()));
        }

        /// <summary>
        /// 画面のコントロールIDで対象のディクショナリのリストを取得(明細用)
        /// </summary>
        /// <param name="dictionaryList">ディクショナリのリスト、searchConditionDictionaryやresultInfoDictionary</param>
        /// <param name="ctrlId">取得対象の画面のコントロールID</param>
        /// <param name="needNone">削除行を含む場合、True(デフォルトはfalse)</param>
        /// <returns>対象のディクショナリのリスト</returns>
        public static List<Dictionary<string, object>> GetDictionaryListByCtrlId(List<Dictionary<string, object>> dictionaryList, string ctrlId, bool needNone = false)
        {
            if (needNone)
            {
                // 削除行を含む場合、コントロールIDが一致するリスト
                return dictionaryList.Where(x => ctrlId.Equals(x["CTRLID"].ToString())).ToList();
            }
            // コントロールIDが一致するリストを返却
            return dictionaryList.Where(x => ctrlId.Equals(x["CTRLID"].ToString())).Where(x => !TMPTBL_CONSTANTS.ROWSTATUS.None.ToString().Equals(x["ROWSTATUS"].ToString())).ToList();
        }

        /// <summary>
        /// 画面のコントロールIDで対象のディクショナリの最小の行番号の要素を取得(明細用)
        /// </summary>
        /// <param name="dictionaryList">ディクショナリのリスト、searchConditionDictionaryやresultInfoDictionary</param>
        /// <param name="ctrlId">取得対象の画面のコントロールID</param>
        /// <param name="needNone">削除行を含む場合、True(デフォルトはfalse)</param>
        /// <param name="rowNo">行番号で絞り込む場合、指定(デフォルトは最小)</param>
        /// <returns>行番号が最小のディクショナリ</returns>
        public static Dictionary<string, object> GetDictionaryMinByCtrlId(List<Dictionary<string, object>> dictionaryList, string ctrlId, bool needNone = false, string rowNo = null)
        {
            var temp = GetDictionaryListByCtrlId(dictionaryList, ctrlId, needNone);
            var minRowNo = rowNo;
            if (string.IsNullOrEmpty(minRowNo))
            {
                minRowNo = temp.Min(x => x["ROWNO"]).ToString();
            }
            return temp.FirstOrDefault(x => x["ROWNO"].ToString() == minRowNo);
        }

        /// <summary>
        /// 選択チェックボックスがチェックされているかを取得する
        /// </summary>
        /// <param name="dictionary">選択チェックボックスが存在する行のディクショナリ</param>
        /// <returns>チェックされている場合True</returns>
        public static bool IsSelectedRowDictionary(Dictionary<string, object> dictionary)
        {
            // resultInfoDIctionary/searchConditionDictionaryのキー名「SELTAG」にチェック状態が格納されている
            var seltag = dictionary["SELTAG"] != null ? dictionary["SELTAG"].ToString() : null;
            // チェック有なら1
            return seltag == "1";
        }

        /// <summary>
        /// 行の状態を引数の状態と比較する
        /// </summary>
        /// <param name="dictionary">行のディクショナリ</param>
        /// <param name="rowStatus">比較するRowStatus(CommonSTDUtil.TMPTBL_CONSTANTS.ROWSTATUS)</param>
        /// <returns>一致するならTrue</returns>
        public static bool IsEqualRowStatus(Dictionary<string, object> dictionary, short rowStatus)
        {
            string dicValue = dictionary["ROWSTATUS"].ToString();
            return rowStatus.ToString() == dicValue;
        }

        /// <summary>
        /// 品目と仕様より、運用管理単位を取得
        /// </summary>
        /// <param name="inItemCd">品目コード</param>
        /// <param name="inSpecificationCd">仕様コード</param>
        /// <param name="inLanguageId">言語</param>
        /// <param name="db">DB接続情報</param>
        /// <returns>取得した運用管理単位</returns>
        public static string GetUnitOfOperationManagement(string inItemCd, string inSpecificationCd, string inLanguageId, ComDB db)
        {
            // SQL文生成
            string sql = " select * from v_item_specification_regist where item_cd = @ItemCd and specification_cd = @SpecificationCd and language_id = @LanguageId";
            object param = new { ItemCd = inItemCd, SpecificationCd = inSpecificationCd, LanguageId = inLanguageId };
            // 検索実行
            EntityDao.ItemSpecificationEntity result = db.GetEntity<EntityDao.ItemSpecificationEntity>(sql, param);

            if (result == null)
            {
                return null;
            }

            return result.UnitOfOperationManagement;
        }

        /// <summary>
        /// 品目と仕様と開始有効日より、任意のテーブルから情報を取得
        /// </summary>
        /// <typeparam name="T">検索するテーブルのクラス</typeparam>
        /// <param name="table">検索するテーブル名</param>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specificationCd">仕様コード</param>
        /// <param name="activeDate">有効日</param>
        /// <param name="db">DB接続</param>
        /// <returns>取得した情報</returns>
        public static T GetItemInfoByActiveDateFromTable<T>(string table, string itemCd, string specificationCd, DateTime activeDate, ComDB db)
        {
            string sql = "select * from " + table + " where item_cd = @ItemCd and specification_cd = @SpecificationCd and @ActiveDate >= active_date order by active_date desc limit 1";
            object param = new { ItemCd = itemCd, SpecificationCd = specificationCd, ActiveDate = activeDate };

            // 検索実行
            return db.GetEntityByDataClass<T>(sql, param);
        }

        /// <summary>
        /// 品目ビュー(全期間)より指定日時での有効な情報を取得
        /// </summary>
        /// <typeparam name="T">取得したい項目名を持つクラス</typeparam>
        /// <param name="itemCd">品目コード</param>
        /// <param name="activeDate">開始有効日</param>
        /// <param name="languageId">言語コード</param>
        /// <param name="db">DB接続</param>
        /// <param name="activateFlg">省略可能　有効フラグ　省略時は有効</param>
        /// <param name="delFlg">省略可能　削除フラグ　省略時は未削除</param>
        /// <returns>取得した品目情報</returns>
        public static T GetItemInfoByActiveDate<T>(string itemCd, DateTime activeDate, string languageId, ComDB db, int activateFlg = Constants.AVTIVE_FLG.APPROVAL, int delFlg = Constants.DEL_FLG.OFF)
        {
            return db.GetEntityByOutsideSqlByDataClass<T>("ItemAllTime_GetInfo", DirPath,
                new { ItemCd = itemCd, ActiveDate = activeDate, LanguageId = languageId, ActivateFlg = activateFlg, DelFlg = delFlg });
        }

        /// <summary>
        /// 品目ビューから品目コードのみで最新のレコードを取得
        /// </summary>
        /// <param name="itemCd">品目コード</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="targetView">参照先ビュー ※検索系 or 登録系</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>品目情報</returns>
        public static EntityDao.ItemEntity GetItemInfoMaxActiveDate(string itemCd, string languageId, TargetView targetView, ComDB db)
        {
            string sql = string.Empty;
            sql += "select * from ";
            if (targetView == TargetView.Search)
            {
                sql += "v_item_search ";
            }
            else
            {
                sql += "v_item_regist ";
            }
            sql += "where item_cd = @ItemCd and language_id = @LanguageId order by active_date desc limit 1";

            return db.GetEntityByDataClass<EntityDao.ItemEntity>(sql, new { ItemCd = itemCd, LanguageId = languageId });
        }

        /// <summary>
        /// 品目仕様ビュー(全期間)より指定日時での有効な情報を取得
        /// </summary>
        /// <typeparam name="T">取得したい項目名を持つクラス</typeparam>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specificationCd">仕様コード</param>
        /// <param name="activeDate">開始有効日</param>
        /// <param name="languageId">言語コード</param>
        /// <param name="db">DB接続</param>
        /// <param name="activateFlg">省略可能　有効フラグ　省略時は有効</param>
        /// <param name="delFlg">省略可能　削除フラグ　省略時は未削除</param>
        /// <returns>取得した品目情報</returns>
        public static T GetItemSpecificationInfoByActiveDate<T>(string itemCd, string specificationCd, DateTime activeDate, string languageId, ComDB db, int activateFlg = Constants.AVTIVE_FLG.APPROVAL, int delFlg = Constants.DEL_FLG.OFF)
        {
            return db.GetEntityByOutsideSqlByDataClass<T>("ItemSpecificationAllTime_GetInfo", DirPath,
                new { ItemCd = itemCd, SpecificationCd = specificationCd, ActiveDate = activeDate, LanguageId = languageId, ActivateFlg = activateFlg, DelFlg = delFlg });
        }

        /// <summary>
        /// 品目仕様ビューから品目、仕様コードのみで最新のレコードを取得
        /// </summary>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specificationCd">仕様コード</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="targetView">参照先ビュー ※検索系 or 登録系</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>品目情報</returns>
        public static EntityDao.ItemSpecificationEntity GetItemSpecificationInfoMaxActiveDate(string itemCd, string specificationCd, string languageId, TargetView targetView, ComDB db)
        {
            string sql = string.Empty;
            sql += "select * from ";
            if (targetView == TargetView.Search)
            {
                sql += "v_item_specification_search ";
            }
            else
            {
                sql += "v_item_specification_regist ";
            }
            // limit 1だが、PKでの指定なので1件のみが取得できる想定
            sql += "where item_cd = @ItemCd and specification_cd = @SpecificationCd and language_id = @LanguageId order by active_date desc limit 1";

            return db.GetEntityByDataClass<EntityDao.ItemSpecificationEntity>(sql, new { ItemCd = itemCd, SpecificationCd = specificationCd, LanguageId = languageId });
        }

        /// <summary>
        /// 取引先ビューから取引先区分、取引先コードのみで最新のレコードを取得
        /// </summary>
        /// <param name="venderDivision">取引先区分</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="targetView">参照先ビュー ※検索系 or 登録系</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取引先情報</returns>
        public static EntityDao.VenderEntity GetVenderInfoMaxActiveDate(string venderDivision, string venderCd, string languageId, TargetView targetView, ComDB db)
        {
            string sql = string.Empty;
            sql += "select * from ";
            if (targetView == TargetView.Search)
            {
                sql += "v_vender_search ";
            }
            else
            {
                sql += "v_vender_regist ";
            }
            // limit 1だが、PKでの指定なので1件のみが取得できる想定
            sql += "where vender_division = @VenderDivision and vender_cd = @VenderCd and language_id = @LanguageId order by active_date desc limit 1";

            return db.GetEntityByDataClass<EntityDao.VenderEntity>(sql, new { VenderDivision = venderDivision, VenderCd = venderCd, LanguageId = languageId });
        }

        /// <summary>
        /// 取引先ビュー(全期間)より指定日時での有効な情報を取得
        /// </summary>
        /// <typeparam name="T">取得したい項目名を持つクラス</typeparam>
        /// <param name="venderDivision">取引先区分</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="activeDate">開始有効日</param>
        /// <param name="languageId">言語コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="activateFlg">省略可能　有効フラグ　省略時は有効</param>
        /// <param name="delFlg">省略可能　削除フラグ　省略時は未削除</param>
        /// <returns>取得した取引先情報</returns>
        public static T GetVenderInfoByActiveDate<T>(string venderDivision, string venderCd, DateTime activeDate, string languageId, ComDB db, int activateFlg = Constants.AVTIVE_FLG.APPROVAL, int delFlg = Constants.DEL_FLG.OFF)
        {
            return db.GetEntityByOutsideSqlByDataClass<T>("VenderAllTime_GetInfo", DirPath,
                new { VenderDivision = venderDivision, VenderCd = venderCd, ActiveDate = activeDate, LanguageId = languageId, ActivateFlg = activateFlg, DelFlg = delFlg });
        }

        /// <summary>
        /// 納入先ビューより情報を取得
        /// </summary>
        /// <typeparam name="T">取得したい項目名を持つクラス</typeparam>
        /// <param name="deliveryCd">納入先コード</param>
        /// <param name="languageId">言語コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得した納入先情報</returns>
        public static T GetDeliveryInfo<T>(string deliveryCd, string languageId, ComDB db)
        {
            return db.GetEntityByOutsideSqlByDataClass<T>("VDelivery_GetInfo", DirPath, new { DeliveryCd = deliveryCd, LanguageId = languageId });
        }

        /// <summary>
        /// ロケーションビューより情報を取得
        /// </summary>
        /// <typeparam name="T">取得したい項目名を持つクラス</typeparam>
        /// <param name="locationCd">ロケーションコード</param>
        /// <param name="languageId">言語コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得したロケーション情報</returns>
        public static T GetLocationInfo<T>(string locationCd, string languageId, ComDB db)
        {
            return db.GetEntityByOutsideSqlByDataClass<T>("VLocation_GetInfo", DirPath, new { LocationCd = locationCd, LanguageId = languageId });
        }

        /// <summary>
        /// 所属マスタよりユーザの所属部署コードを取得
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>所属部署の先頭</returns>
        public static string GetBelongOrganizationCdByUserId(string userId, ComDB db)
        {
            return db.GetEntityByOutsideSql<string>("BelongInfo_GetOrganization", DirPath, new { UserId = userId });
        }

        /// <summary>
        /// 処方ヘッダよりレシピコードで情報を取得
        /// </summary>
        /// <param name="recipeCd">レシピコード</param>
        /// <param name="activeDate">有効開始日</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="recipeType">レシピタイプ(省略可能)</param>
        /// <param name="activateFlg">有効フラグ</param>
        /// <param name="delFlg">削除フラグ</param>
        /// <returns>取得した処方ヘッダの情報</returns>
        public static EntityDao.RecipeHeaderEntity GetRecipeHeaderInfoByRecipeCd(string recipeCd, DateTime activeDate, ComDB db, int? recipeType = null, int activateFlg = Constants.AVTIVE_FLG.APPROVAL, int delFlg = Constants.DEL_FLG.OFF)
        {
            return db.GetEntityByOutsideSqlByDataClass<EntityDao.RecipeHeaderEntity>("RecipeHeader_GetInfo", DirPath, new { RecipeCd = recipeCd, RecipeType = recipeType, ActiveDate = activeDate, ActivateFlg = activateFlg, DelFlg = delFlg });
        }

        /// <summary>
        /// 消費税の計算を行う処理
        /// </summary>
        /// <param name="amount">計算を行う金額</param>
        /// <param name="taxKey">消費税区分マスタのキー</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>消費税の金額(カンマ区切り文字列)</returns>
        public static string GetTaxAmount(decimal amount, EntityDao.TaxMasterEntity.PrimaryKey taxKey, ComDB db)
        {
            // 消費税区分マスタの値を取得
            EntityDao.TaxMasterEntity taxEntity = new EntityDao.TaxMasterEntity();
            taxEntity = taxEntity.GetEntity(taxKey.TaxCd, taxKey.Category, db);
            if (taxEntity == null)
            {
                return "0";
            }
            // 消費税を計算(金額 * 税率(%の値) / 100 )
            decimal tax = amount * taxEntity.TaxRatio / 100;

            // 端数処理
            string newTax = CheckDigit.Format(Constants.NUMBER_CHKDISIT.TAX_AMOUNT, tax, db);
            return newTax;
        }

        /// <summary>
        /// 消費税の計算を行う処理
        /// </summary>
        /// <param name="amount">計算を行う金額</param>
        /// <param name="taxKey">消費税区分マスタのキー</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>消費税の金額(decimal)</returns>
        public static decimal GetTaxAmountDecimal(decimal amount, EntityDao.TaxMasterEntity.PrimaryKey taxKey, ComDB db)
        {
            string tax = GetTaxAmount(amount, taxKey, db);
            return decimal.Parse(tax);
        }

        /// <summary>
        /// 自社マスタの検索処理
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <returns>検索結果</returns>
        public static EntityDao.CompanyEntity GetCompanyData(ComDB db)
        {
            return new EntityDao.CompanyEntity().GetEntity(Company_Cd, db);
        }

        /// <summary>
        /// ロット付帯情報ヘッダ登録処理
        /// </summary>
        /// <param name="lotExtInfoHead">登録条件クラス</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常 -1:エラー </returns>
        public static int InsertLotExtInfoHead(EntityDao.LotExtInfoHeadEntity lotExtInfoHead, string userId, ComDB db)
        {
            // ロット付帯情報ヘッダ登録処理
            int result = db.RegistByOutsideSql(LotExtInfo.SqlName.InsLotExtInfoHead, DirPath, lotExtInfoHead);
            if (result < 0)
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }

            // 正常終了
            return APConstants.APConstants.SQL_RESULT.COMMIT;
        }

        /// <summary>
        /// ロット付帯情報ヘッダ更新処理
        /// </summary>
        /// <param name="lotExtInfoHead">更新条件クラス</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常 -1:エラー </returns>
        public static int UpdateLotExtInfoHead(EntityDao.LotExtInfoHeadEntity lotExtInfoHead, string userId, ComDB db)
        {
            // ロット付帯情報ヘッダ更新処理
            int result = db.RegistByOutsideSql(LotExtInfo.SqlName.UpdLotExtInfoHead, DirPath, lotExtInfoHead);
            if (result <= 0)
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }

            return APConstants.APConstants.SQL_RESULT.COMMIT;
        }

        /// <summary>
        /// ロット付帯情報（ヘッダ・詳細）削除処理
        /// </summary>
        /// <param name="lotExtInfoHead">削除条件クラス</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常 -1:エラー </returns>
        public static int DeleteLotExtInfoHead(EntityDao.LotExtInfoHeadEntity lotExtInfoHead, string userId, ComDB db)
        {
            // ロット付帯情報ヘッダ削除処理
            int result = db.RegistByOutsideSql(LotExtInfo.SqlName.DelLotExtInfoHead, DirPath, lotExtInfoHead);
            //if (result <= 0)
            if (result < 0)
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }

            // ロット付帯情報詳細 削除処理
            result = db.RegistByOutsideSql(LotExtInfo.SqlName.DelLotExtInfoDetail, DirPath, lotExtInfoHead);
            //if (result <= 0)
            if (result < 0)
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }

            return APConstants.APConstants.SQL_RESULT.COMMIT;
        }

        /// <summary>
        /// ロット付帯情報詳細登録処理
        /// </summary>
        /// <param name="lotExtInfoDetail">登録条件クラス</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常 -1:エラー </returns>
        public static int InsertLotExtInfoDetail(EntityDao.LotExtInfoDetailEntity lotExtInfoDetail, string userId, ComDB db)
        {
            // ロット付帯情報詳細 登録処理
            int result = db.RegistByOutsideSql(LotExtInfo.SqlName.InsLotExtInfoDetail, DirPath, lotExtInfoDetail);
            if (result < 0)
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }

            // 正常終了
            return APConstants.APConstants.SQL_RESULT.COMMIT;
        }

        /// <summary>
        /// ロット付帯情報詳細更新処理
        /// </summary>
        /// <param name="lotExtInfoDetail">登録条件クラス</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常 -1:エラー </returns>
        public static int UpdateLotExtInfoDetail(EntityDao.LotExtInfoDetailEntity lotExtInfoDetail, string userId, ComDB db)
        {
            // ロット付帯情報ヘッダ 更新処理
            int result = db.RegistByOutsideSql(LotExtInfo.SqlName.UpdLotExtInfoDetail, DirPath, lotExtInfoDetail);
            if (result < 0)
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }

            return APConstants.APConstants.SQL_RESULT.COMMIT;
        }

        /// <summary>
        /// 製造指図フォーミュラ登録処理
        /// </summary>
        /// <param name="directionFormula">登録条件クラス</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常 -1:エラー </returns>
        public static int InsertDirectionFormula(EntityDao.DirectionFormulaEntity directionFormula, string userId, ComDB db)
        {
            // 製造指図フォーミュラ登録処理
            int result = db.RegistByOutsideSql(Direction.SqlName.InsDirectionFormula, DirPath, directionFormula);
            if (result < 0)
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }
            // プルーフ登録（新規追加後）
            if (!CreateProof<EntityDao.DirectionFormulaEntity>(db, directionFormula, Direction.SqlName.FormulaProof, userId, APConstants.APConstants.PROOF_STATUS.NEW))
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }
            // 正常終了
            return APConstants.APConstants.SQL_RESULT.COMMIT;
        }

        /// <summary>
        /// 製造指図フォーミュラ更新処理
        /// </summary>
        /// <param name="directionFormula">更新条件クラス</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常 -1:エラー </returns>
        public static int UpdateDirectionFormula(EntityDao.DirectionFormulaEntity directionFormula, string userId, ComDB db)
        {
            // プルーフ登録（更新前）
            if (!CreateProof<EntityDao.DirectionFormulaEntity>(db, directionFormula, Direction.SqlName.FormulaProof, userId, APConstants.APConstants.PROOF_STATUS.PRE_UPDATE))
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }
            // 製造指図フォーミュラ更新処理
            int result = db.RegistByOutsideSql(Direction.SqlName.UpdDirectionFormula, DirPath, directionFormula);
            if (result < 0)
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }
            // プルーフ登録（更新後）
            if (!CreateProof<EntityDao.DirectionFormulaEntity>(db, directionFormula, Direction.SqlName.FormulaProof, userId, APConstants.APConstants.PROOF_STATUS.POST_UPDATE))
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }
            return APConstants.APConstants.SQL_RESULT.COMMIT;
        }

        /// <summary>
        /// 製造指図フォーミュラ削除処理
        /// </summary>
        /// <param name="directionFormula">削除条件クラス</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>0:正常 -1:エラー </returns>
        public static int DeleteDirectionFormula(EntityDao.DirectionFormulaEntity directionFormula, string userId, ComDB db)
        {
            // プルーフ登録（削除前）
            if (!CreateProof<EntityDao.DirectionFormulaEntity>(db, directionFormula, Direction.SqlName.FormulaProof, userId, APConstants.APConstants.PROOF_STATUS.DELETE))
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }
            // 製造指図フォーミュラ削除処理
            int result = db.RegistByOutsideSql(Direction.SqlName.DelDirectionFormula, DirPath, directionFormula);
            if (result < 0)
            {
                // エラー終了
                return APConstants.APConstants.SQL_RESULT.ROLLBACK;
            }
            return APConstants.APConstants.SQL_RESULT.COMMIT;
        }

        #region ワークフロー
        /// <summary>
        /// ワークフロー　承認依頼処理実行（機能共通）
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="requestMailParamList">承認依頼メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonRequest(string slipNo, List<string> requestMailParamList, ComBusBase form, ComDB db, string slipBranchNo1 = "", string slipBranchNo2 = "")
        {
            // 承認依頼共通画面より戻り値を取得(JSON形式)
            string paramWfJson = form.GetGlobalData(WorkFlow.GlobalDataKeyRequest).ToString();
            // JSON形式をデシリアライズ
            WorkflowApprovalRequestParam paramWfReq = form.DeserializeFromJson<WorkflowApprovalRequestParam>(paramWfJson)[0];

            ComBusBase.PushInfo info;       // プッシュ通知内容格納用
            string errorMsg = string.Empty; // メール送信エラー設定用
            // 承認依頼処理実行
            if (CreateApprovalRequestForWorkFlow(paramWfReq, slipNo, slipBranchNo1, slipBranchNo2, requestMailParamList, form.LanguageId, form.UserId, db, out info, ref errorMsg))
            {
                // 成功時、プッシュ通知を格納
                form.SetPushTarget(info);
                // メール送信失敗している場合、エラーを設定
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    form.SetErrorMsg(errorMsg);
                }
                return true;
            }
            // 失敗時、Falseを返す
            return false;
        }

        /// <summary>
        /// ワークフロー　承認依頼処理実行（機能共通）
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="requestMailParamList">承認依頼メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonRequestNotMail(string slipNo, List<string> requestMailParamList, ComBusBase form, ComDB db, string slipBranchNo1 = "", string slipBranchNo2 = "")
        {
            // 承認依頼共通画面より戻り値を取得(JSON形式)
            string paramWfJson = form.GetGlobalData(WorkFlow.GlobalDataKeyRequest).ToString();
            // JSON形式をデシリアライズ
            WorkflowApprovalRequestParam paramWfReq = form.DeserializeFromJson<WorkflowApprovalRequestParam>(paramWfJson)[0];

            ComBusBase.PushInfo info;       // プッシュ通知内容格納用
            string errorMsg = string.Empty; // メール送信エラー設定用
            // 承認依頼処理実行
            if (CreateApprovalRequestForWorkFlowNotMail(paramWfReq, slipNo, slipBranchNo1, slipBranchNo2, requestMailParamList, form.LanguageId, form.UserId, db, out info, ref errorMsg))
            {
                // 成功時、プッシュ通知を格納
                form.SetPushTarget(info);
                // メール送信失敗している場合、エラーを設定
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    form.SetErrorMsg(errorMsg);
                }
                return true;
            }
            // 失敗時、Falseを返す
            return false;
        }

        /// <summary>
        /// ワークフロー　承認処理実行（機能共通）
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="requestMailParamList">承認依頼メールの引数リスト</param>
        /// <param name="completeMailParamList">承認完了メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonApproval(string slipNo, List<string> approvalMailParamList, List<string> requestMailParamList, List<string> completeMailParamList, ComBusBase form, ComDB db,
            string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "")
        {
            // 承認で共通処理呼び出し
            return WorkflowCommonApprovalBase(Constants.WORKFLOW.REQUESTPARAM.APPROVAL, slipNo, approvalMailParamList, form, db, slipBranchNo1, slipBranchNo2, requestMailParamList, completeMailParamList, addSearchKey);
        }

        /// <summary>
        /// WF承認処理（承認完了時メール送信なし）
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="requestMailParamList">承認依頼メールの引数リスト</param>
        /// <param name="completeMailParamList">承認完了メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonApprovalNotMail(string slipNo, List<string> approvalMailParamList, List<string> requestMailParamList, List<string> completeMailParamList, ComBusBase form, ComDB db,
            string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "")
        {
            // 承認で共通処理呼び出し
            return WorkflowCommonApprovalBaseNotMail(Constants.WORKFLOW.REQUESTPARAM.APPROVAL, slipNo, approvalMailParamList, form, db, slipBranchNo1, slipBranchNo2, requestMailParamList, completeMailParamList, addSearchKey);
        }

        /// <summary>
        /// ワークフロー　承認取消処理実行（機能共通）
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonApprovalCancelNotMail(string slipNo, List<string> approvalMailParamList, ComBusBase form, ComDB db, string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "")
        {
            // 承認取消で共通処理呼び出し
            return WorkflowCommonApprovalBaseAllNotMail(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL, slipNo, approvalMailParamList, form, db, slipBranchNo1, slipBranchNo2, addSearchKey: addSearchKey);
        }

        /// <summary>
        /// ワークフロー　否認処理実行（機能共通）
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonDisapproval(string slipNo, List<string> approvalMailParamList, ComBusBase form, ComDB db, string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "")
        {
            // 否認で共通処理呼び出し
            return WorkflowCommonApprovalBase(Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL, slipNo, approvalMailParamList, form, db, slipBranchNo1, slipBranchNo2, addSearchKey: addSearchKey);
        }

        /// <summary>
        /// ワークフロー　否認処理実行（メール送信無）
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonDisapprovalNotMail(string slipNo, List<string> approvalMailParamList, ComBusBase form, ComDB db, string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "")
        {
            // 否認で共通処理呼び出し
            return WorkflowCommonApprovalBaseAllNotMail(Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL, slipNo, approvalMailParamList, form, db, slipBranchNo1, slipBranchNo2, addSearchKey: addSearchKey);
        }

        /// <summary>
        /// ワークフロー　承認依頼取消処理実行（機能共通）
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonApprovalRequestCancel(string slipNo, List<string> approvalMailParamList, ComBusBase form, ComDB db, string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "")
        {
            // 承認依頼取消で共通処理呼び出し
            return WorkflowCommonApprovalBase(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL, slipNo, approvalMailParamList, form, db, slipBranchNo1, slipBranchNo2, addSearchKey: addSearchKey);
        }

        /// <summary>
        /// ワークフロー　承認依頼取消処理実行（メール送信無）
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonApprovalRequestCancelNotMail(string slipNo, List<string> approvalMailParamList, ComBusBase form, ComDB db, string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "")
        {
            // 承認依頼取消で共通処理呼び出し
            return WorkflowCommonApprovalBaseAllNotMail(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL, slipNo, approvalMailParamList, form, db, slipBranchNo1, slipBranchNo2, addSearchKey: addSearchKey);
        }

        /// <summary>
        /// ワークフロー　承認取消処理実行（機能共通）
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonApprovalCancel(string slipNo, List<string> approvalMailParamList, ComBusBase form, ComDB db, string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "")
        {
            // 承認取消で共通処理呼び出し
            return WorkflowCommonApprovalBase(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL, slipNo, approvalMailParamList, form, db, slipBranchNo1, slipBranchNo2, addSearchKey: addSearchKey);
        }

        /// <summary>
        /// ワークフロー　承認処理実行（本体）
        /// </summary>
        /// <param name="requestParam">処理区分 11:承認依頼、12:承認依頼取消、21:承認、22:承認取消、8:否認 APConstsnts参照</param>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="requestMailParamList">承認依頼メールの引数リスト、承認以外は省略、省略時はNull</param>
        /// <param name="completeMailParamList">承認完了メールの引数リスト、承認以外は省略、省略時はNull</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonApprovalBase(int requestParam, string slipNo, List<string> approvalMailParamList, ComBusBase form, ComDB db,
            string slipBranchNo1 = "", string slipBranchNo2 = "", List<string> requestMailParamList = null, List<string> completeMailParamList = null, string addSearchKey = "")
        {
            // 承認画面で入力されたコメント
            string commnet = form.GetGlobalData(WorkFlow.GlobalDataKeyApproval).ToString();

            ComBusBase.PushInfo info;  // プッシュ通知内容格納用
            string errorMsg = string.Empty; // メール送信エラー設定用
            if (SetApprovalStatusForWorkFlow(requestParam, form.ConductId, slipNo, slipBranchNo1, slipBranchNo2, commnet, approvalMailParamList,
                form.LanguageId, form.UserId, db, out info, ref errorMsg, requestMailParamList, completeMailParamList, addSearchKey))
            {
                form.SetPushTarget(info);
                // メール送信失敗している場合、エラーを設定
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    form.SetErrorMsg(errorMsg);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// WF承認処理実行（承認完了時メール送信なし）
        /// </summary>
        /// <param name="requestParam">処理区分 11:承認依頼、12:承認依頼取消、21:承認、22:承認取消、8:否認 APConstsnts参照</param>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="requestMailParamList">承認依頼メールの引数リスト、承認以外は省略、省略時はNull</param>
        /// <param name="completeMailParamList">承認完了メールの引数リスト、承認以外は省略、省略時はNull</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonApprovalBaseNotMail(int requestParam, string slipNo, List<string> approvalMailParamList, ComBusBase form, ComDB db,
            string slipBranchNo1 = "", string slipBranchNo2 = "", List<string> requestMailParamList = null, List<string> completeMailParamList = null, string addSearchKey = "")
        {
            // 承認画面で入力されたコメント
            string commnet = form.GetGlobalData(WorkFlow.GlobalDataKeyApproval).ToString();

            ComBusBase.PushInfo info;  // プッシュ通知内容格納用
            string errorMsg = string.Empty; // メール送信エラー設定用
            if (SetApprovalStatusForWorkFlowNotMail(requestParam, form.ConductId, slipNo, slipBranchNo1, slipBranchNo2, commnet, approvalMailParamList,
                form.LanguageId, form.UserId, db, out info, ref errorMsg, requestMailParamList, completeMailParamList, addSearchKey))
            {
                form.SetPushTarget(info);
                // メール送信失敗している場合、エラーを設定
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    form.SetErrorMsg(errorMsg);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// WF承認処理実行（全てメール送信なし）
        /// </summary>
        /// <param name="requestParam">処理区分 11:承認依頼、12:承認依頼取消、21:承認、22:承認取消、8:否認 APConstsnts参照</param>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="approvalMailParamList">承認メールの引数リスト</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="requestMailParamList">承認依頼メールの引数リスト、承認以外は省略、省略時はNull</param>
        /// <param name="completeMailParamList">承認完了メールの引数リスト、承認以外は省略、省略時はNull</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>エラー発生時、False</returns>
        public static bool WorkflowCommonApprovalBaseAllNotMail(int requestParam, string slipNo, List<string> approvalMailParamList, ComBusBase form, ComDB db,
            string slipBranchNo1 = "", string slipBranchNo2 = "", List<string> requestMailParamList = null, List<string> completeMailParamList = null, string addSearchKey = "")
        {
            // 承認画面で入力されたコメント
            string commnet = form.GetGlobalData(WorkFlow.GlobalDataKeyApproval).ToString();

            ComBusBase.PushInfo info;  // プッシュ通知内容格納用
            string errorMsg = string.Empty; // メール送信エラー設定用
            if (SetApprovalStatusForWorkFlowAllNotMail(requestParam, form.ConductId, slipNo, slipBranchNo1, slipBranchNo2, commnet, approvalMailParamList,
                form.LanguageId, form.UserId, db, out info, ref errorMsg, requestMailParamList, completeMailParamList, addSearchKey))
            {
                form.SetPushTarget(info);
                // メール送信失敗している場合、エラーを設定
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    form.SetErrorMsg(errorMsg);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 承認ワークフローヘッダ 承認状態取得
        /// </summary>
        /// <param name="workflowDivision">呼出元区分</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <param name="button">押下した承認ボタンのCTRLID</param>
        /// <returns>-1:異常、それ以外、正常</returns>
        public static int GetApprovalWorkFlowStatus(string workflowDivision, string slipNo, string slipBranchNo1, string slipBranchNo2, ComDB db, string addSearchKey, string button)
        {
            bool approvalDivision = false;
            if (button == Constants.WORKFLOW.ButtonCtrlId.ApprovalRequest || button == Constants.WORKFLOW.ButtonCtrlId.ApprovalRequestCancel)
            {
                // 承認依頼、承認取消の場合true
                approvalDivision = true;
            }

            // 呼出元区分を取得
            EntityDao.NamesEntity namesBean = getWfDivision(workflowDivision, approvalDivision, db, addSearchKey);
            if (namesBean == null)
            {
                // 存在しない場合、エラーに
                return -1;
            }

            // 呼出元区分、登録元伝票番号、登録元伝票番号枝番1、登録元伝票番号枝番2をもとにワークフローNoを取得
            EntityDao.WorkflowHeaderEntity bean = new EntityDao.WorkflowHeaderEntity();
            bean.WfDivision = namesBean.NameCd; // 呼出元区分
            bean.SlipNo = slipNo;               // 登録元伝票番号
            bean.SlipBranchNo1 = slipBranchNo1; // 登録元伝票番号枝番1
            bean.SlipBranchNo2 = slipBranchNo2; // 登録元伝票番号枝番2

            // レコードを取得
            bean = db.GetEntityByOutsideSqlByDataClass<EntityDao.WorkflowHeaderEntity>(WorkFlow.WorkFlow_GetWorkflowNo, WorkFlow.SubDir, bean);

            if (bean == null || bean.DelFlg == Constants.DEL_FLG.ON)
            {
                // 存在しない場合か削除済みの場合エラーに
                return -1;
            }
            else
            {
                return bean.Status != null ? (int)bean.Status : -1;
            }
        }

        /// <summary>
        /// ワークフロー承認依頼処理
        /// </summary>
        /// <param name="workflowAppReqParam">承認依頼画面から取得した、パラメータクラス</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">枝番1</param>
        /// <param name="slipBranchNo2">枝番2</param>
        /// <param name="mailParamList">承認依頼メールの引数リスト</param>
        /// <param name="languageId">メールの言語ID</param>
        /// <param name="tantoCd">承認依頼を行ったユーザID</param>
        /// <param name="db">DB接続</param>
        /// <param name="pushInfo">out プッシュ通知の情報</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool CreateApprovalRequestForWorkFlow(WorkflowApprovalRequestParam workflowAppReqParam, string slipNo, string slipBranchNo1, string slipBranchNo2,
             List<string> mailParamList, string languageId, string tantoCd, ComDB db, out ComBusBase.PushInfo pushInfo, ref string errorMsg)
        {
            int result = 0;
            DateTime now = DateTime.Now;
            pushInfo = new ComBusBase.PushInfo();

            // 呼出元区分を取得
            string workflowDivision = workflowAppReqParam.WorkflowDivision;

            // ヘッダ登録
            bool updateFlg = true; // 更新処理フラグを定義 ※true:Insert、false:Update
            EntityDao.WorkflowHeaderEntity registHeader = getWfRegistHeader(workflowAppReqParam.Header, workflowDivision, slipNo, slipBranchNo1, slipBranchNo2, tantoCd, workflowAppReqParam.Comment, now, db, ref updateFlg);
            if (updateFlg)
            {
                result = db.RegistByOutsideSql(WorkFlow.WorkFlowHeader_Insert, WorkFlow.SubDir, registHeader);
            }
            else
            {
                result = db.RegistByOutsideSql(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, registHeader);
            }

            if (result < 0)
            {
                // 登録エラー
                return false;
            }

            // ワークフローNOをKeyにしてDeleteを実施
            result = db.RegistByOutsideSql(WorkFlow.WorkFlowDetail_DeleteByWfNo, WorkFlow.SubDir, new { WfNo = registHeader.WfNo });
            if (result < 0)
            {
                // 削除エラー
                return false;
            }

            // 詳細登録
            foreach (var detailInfo in workflowAppReqParam.DetailList)
            {
                EntityDao.WorkflowDetailEntity registDetail = getWfRegistDetail(registHeader.WfNo, detailInfo, string.Empty, now, tantoCd);
                result = db.RegistByOutsideSql(WorkFlow.WorkFlowDetail_Insert, WorkFlow.SubDir, registDetail);
                if (result < 0)
                {
                    // 登録エラー
                    return false;
                }
            }

            // 通知処理
            // レコードを取得
            EntityDao.WorkflowHeaderEntity headerBean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, workflowDivision, db);
            if (headerBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            // ログに登録
            result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.APPROVAL_REQUEST, workflowAppReqParam.Comment, now, db);
            // 更新失敗した場合、エラーに
            if (result < 0)
            {
                return false;
            }

            // ワークフロー通知クラスを作成
            WorlflowNotice notice = new WorlflowNotice(workflowDivision, headerBean, db);

            // 通知
            pushInfo = notice.Notice(registHeader, Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST, mailParamList, languageId, tantoCd, null, db, ref errorMsg);

            return true;
        }

        /// <summary>
        /// ワークフロー承認依頼処理(メール送信無)
        /// </summary>
        /// <param name="workflowAppReqParam">承認依頼画面から取得した、パラメータクラス</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">枝番1</param>
        /// <param name="slipBranchNo2">枝番2</param>
        /// <param name="mailParamList">承認依頼メールの引数リスト</param>
        /// <param name="languageId">メールの言語ID</param>
        /// <param name="tantoCd">承認依頼を行ったユーザID</param>
        /// <param name="db">DB接続</param>
        /// <param name="pushInfo">out プッシュ通知の情報</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool CreateApprovalRequestForWorkFlowNotMail(WorkflowApprovalRequestParam workflowAppReqParam, string slipNo, string slipBranchNo1, string slipBranchNo2,
             List<string> mailParamList, string languageId, string tantoCd, ComDB db, out ComBusBase.PushInfo pushInfo, ref string errorMsg)
        {
            int result = 0;
            DateTime now = DateTime.Now;
            pushInfo = new ComBusBase.PushInfo();

            // 呼出元区分を取得
            string workflowDivision = workflowAppReqParam.WorkflowDivision;

            // ヘッダ登録
            bool updateFlg = true; // 更新処理フラグを定義 ※true:Insert、false:Update
            EntityDao.WorkflowHeaderEntity registHeader = getWfRegistHeader(workflowAppReqParam.Header, workflowDivision, slipNo, slipBranchNo1, slipBranchNo2, tantoCd, workflowAppReqParam.Comment, now, db, ref updateFlg);
            if (updateFlg)
            {
                result = db.RegistByOutsideSql(WorkFlow.WorkFlowHeader_Insert, WorkFlow.SubDir, registHeader);
            }
            else
            {
                result = db.RegistByOutsideSql(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, registHeader);
            }

            if (result < 0)
            {
                // 登録エラー
                return false;
            }

            // ワークフローNOをKeyにしてDeleteを実施
            result = db.RegistByOutsideSql(WorkFlow.WorkFlowDetail_DeleteByWfNo, WorkFlow.SubDir, new { WfNo = registHeader.WfNo });
            if (result < 0)
            {
                // 削除エラー
                return false;
            }

            // 詳細登録
            foreach (var detailInfo in workflowAppReqParam.DetailList)
            {
                EntityDao.WorkflowDetailEntity registDetail = getWfRegistDetail(registHeader.WfNo, detailInfo, string.Empty, now, tantoCd);
                result = db.RegistByOutsideSql(WorkFlow.WorkFlowDetail_Insert, WorkFlow.SubDir, registDetail);
                if (result < 0)
                {
                    // 登録エラー
                    return false;
                }
            }

            // 通知処理
            // レコードを取得
            EntityDao.WorkflowHeaderEntity headerBean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, workflowDivision, db);
            if (headerBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            // ログに登録
            result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.APPROVAL_REQUEST, workflowAppReqParam.Comment, now, db);
            // 更新失敗した場合、エラーに
            if (result < 0)
            {
                return false;
            }

            // ワークフロー通知クラスを作成
            WorlflowNotice notice = new WorlflowNotice(workflowDivision, headerBean, db);

            // 通知
            pushInfo = notice.NoticeNotMail(registHeader, Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST, mailParamList, languageId, tantoCd, null, db, ref errorMsg);

            return true;
        }

        /// <summary>
        /// 承認依頼登録クラス
        /// </summary>
        public class WorkflowApprovalRequestParam
        {
            /// <summary>Gets or sets ヘッダ情報 </summary>
            /// <value>ヘッダ情報</value>
            public EntityDao.WorkflowHeaderEntity Header { get; set; }
            /// <summary>Gets or sets 詳細情報リスト </summary>
            /// <value>詳細情報リスト</value>
            public List<EntityDao.WorkflowDetailEntity> DetailList { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Comment { get; set; }
            /// <summary>Gets or sets 呼出元区分</summary>
            /// <value>呼出元区分</value>
            public string WorkflowDivision { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public WorkflowApprovalRequestParam()
            {
                Header = new EntityDao.WorkflowHeaderEntity();
                DetailList = new List<EntityDao.WorkflowDetailEntity>();
            }
        }

        /// <summary>
        /// ワークフローヘッダ登録用データクラス作成
        /// </summary>
        /// <param name="headerInfo">承認依頼画面より取得した、登録する内容のクラス</param>
        /// <param name="workflowDivision">呼出元区分</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">枝番1</param>
        /// <param name="slipBranchNo2">枝番2</param>
        /// <param name="tantoCd">承認依頼を行ったユーザ</param>
        /// <param name="comments">承認依頼時のコメント</param>
        /// <param name="now">システム日時</param>
        /// <param name="db">DB接続</param>
        /// <param name="updateFlg">true:Insert、false:Update</param>
        /// <returns>ワークフローヘッダ登録用データクラス</returns>
        private static EntityDao.WorkflowHeaderEntity getWfRegistHeader(EntityDao.WorkflowHeaderEntity headerInfo, string workflowDivision, string slipNo,
            string slipBranchNo1, string slipBranchNo2, string tantoCd, string comments, DateTime now, ComDB db, ref bool updateFlg)
        {
            updateFlg = false; // 更新処理

            // ワークフローNoを使いまわすように修正
            // 既存レコードが存在するかチェック
            EntityDao.WorkflowHeaderEntity registHeader = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, workflowDivision, db);
            if (registHeader == null)
            {
                registHeader = new EntityDao.WorkflowHeaderEntity();
                // 存在しない場合、ワークフローNOを再付番
                registHeader.WfNo = GetNumber(db, WorkFlow.Key_WorkflowNo_Seq); // シーケンスより取得
                updateFlg = true; // インサート処理を実施
            }

            // 戻り値　登録する内容
            // 承認依頼画面より取得した情報を設定
            registHeader.WfTemplateNo = headerInfo.WfTemplateNo;
            registHeader.ActiveDate = headerInfo.ActiveDate;
            registHeader.WfName = headerInfo.WfName;
            // 機能IDより名称マスタWFDVで取得した呼出元区分を設定
            registHeader.WfDivision = workflowDivision;
            // 呼出元画面より与えられた伝票番号と枝番を設定
            registHeader.SlipNo = slipNo;
            registHeader.SlipBranchNo1 = slipBranchNo1;
            registHeader.SlipBranchNo2 = slipBranchNo2;
            // 操作ユーザ
            registHeader.RequestUserId = tantoCd;
            // 承認依頼画面より取得した情報を設定
            registHeader.NoticeDivision = headerInfo.NoticeDivision;
            registHeader.RequestComments = comments;
            // 承認依頼なので、依頼中
            registHeader.Status = Constants.WORKFLOW.HEADERSTATUS.REQUESTING;
            registHeader.InputDate = now;
            registHeader.InputUserId = tantoCd;
            registHeader.UpdateDate = now;
            registHeader.UpdateUserId = tantoCd;
            registHeader.DelFlg = Constants.DEL_FLG.OFF;
            return registHeader;
        }

        /// <summary>
        /// ワークフロー詳細登録用データクラス作成
        /// </summary>
        /// <param name="workflowNo">ワークフローNo</param>
        /// <param name="detailInfo">承認依頼画面より取得した、登録する内容のクラス</param>
        /// <param name="comments">承認依頼時のコメント</param>
        /// <param name="now">システム日時</param>
        /// <param name="tantoCd">承認依頼を行ったユーザID</param>
        /// <returns>ワークフロー詳細登録用データクラス</returns>
        private static EntityDao.WorkflowDetailEntity getWfRegistDetail(string workflowNo, EntityDao.WorkflowDetailEntity detailInfo, string comments, DateTime now, string tantoCd)
        {
            // 戻り値　 詳細に登録する内容
            var registDetail = new EntityDao.WorkflowDetailEntity();
            // 承認依頼画面より取得した情報を設定
            registDetail.WfNo = workflowNo;
            registDetail.Seq = detailInfo.Seq;
            registDetail.ApprovalUserId = detailInfo.ApprovalUserId;
            registDetail.AllApprovalFlg = detailInfo.AllApprovalFlg;
            // 未承認のため、Null
            registDetail.Status = null;
            registDetail.ApprovalDate = null;
            // 承認依頼画面より取得した情報を設定
            registDetail.Comments = comments;
            registDetail.InputDate = now;
            registDetail.InputUserId = tantoCd;
            registDetail.UpdateDate = now;
            registDetail.UpdateUserId = tantoCd;

            return registDetail;
        }

        /// <summary>
        /// 承認状態登録前共通処理
        /// </summary>
        /// <param name="workflowDivision">呼出元区分、機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <param name="button">押下ボタン</param>
        /// <param name="tantoCd">承認ユーザーID</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool CheckApprovalStatusForWorkFlow(string workflowDivision, string slipNo, ComDB db, ComBusBase form, ComUtil.MessageResources msgResources,
            ref string errorMsg, string button, string tantoCd, string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "")
        {
            int status = GetApprovalWorkFlowStatus(workflowDivision, slipNo, slipBranchNo1, slipBranchNo2, db, addSearchKey, button);

            switch (status)
            {
                case Constants.WORKFLOW.HEADERSTATUS.REQUESTING:
                    if (button == Constants.WORKFLOW.ButtonCtrlId.ApprovalRequest)
                    {
                        // 「別の担当者にて承認依頼されました。」
                        errorMsg = getResMessage(APResources.ID.M10176, form.LanguageId, msgResources);
                        return false;
                    }
                    return true;
                case Constants.WORKFLOW.HEADERSTATUS.APPROVING:
                    if (button == Constants.WORKFLOW.ButtonCtrlId.ApprovalRequest)
                    {
                        // 「別の担当者にて承認依頼されました。」
                        errorMsg = getResMessage(APResources.ID.M10176, form.LanguageId, msgResources);
                        return false;
                    }
                    else if (button == Constants.WORKFLOW.ButtonCtrlId.Approval ||
                             button == Constants.WORKFLOW.ButtonCtrlId.Disapproval)
                    {
                        return CheckApprovalStatusForWorkFlowDetail(workflowDivision, button, db, form, slipNo, tantoCd, msgResources, ref errorMsg, slipBranchNo1, slipBranchNo2, addSearchKey);
                    }
                    return true;
                case Constants.WORKFLOW.HEADERSTATUS.DENIAL:
                    if (button == Constants.WORKFLOW.ButtonCtrlId.ApprovalRequestCancel ||
                        button == Constants.WORKFLOW.ButtonCtrlId.Approval ||
                        button == Constants.WORKFLOW.ButtonCtrlId.Disapproval ||
                        button == Constants.WORKFLOW.ButtonCtrlId.ApprovalCancel)
                    {
                        // 「別の担当者にて否認されました。」
                        errorMsg = getResMessage(APResources.ID.M10171, form.LanguageId, msgResources);
                        return false;
                    }
                    return true;
                case Constants.WORKFLOW.HEADERSTATUS.APPROVAL:
                    if (button == Constants.WORKFLOW.ButtonCtrlId.ApprovalRequestCancel ||
                        button == Constants.WORKFLOW.ButtonCtrlId.Approval ||
                        button == Constants.WORKFLOW.ButtonCtrlId.Disapproval)
                    {
                        // 「別の担当者にて承認されました。」
                        errorMsg = getResMessage(APResources.ID.M10170, form.LanguageId, msgResources);
                        return false;
                    }
                    else if (button == Constants.WORKFLOW.ButtonCtrlId.ApprovalRequest)
                    {
                        // 「別の担当者にて承認依頼されました。」
                        errorMsg = getResMessage(APResources.ID.M10176, form.LanguageId, msgResources);
                        return false;
                    }
                    return true;
                case Constants.WORKFLOW.HEADERSTATUS.CANCEL:
                    if (button == Constants.WORKFLOW.ButtonCtrlId.Approval ||
                        button == Constants.WORKFLOW.ButtonCtrlId.Disapproval ||
                        button == Constants.WORKFLOW.ButtonCtrlId.ApprovalCancel)
                    {
                        // 「別の担当者にて承認依頼が取消されました。」
                        errorMsg = getResMessage(APResources.ID.M10172, form.LanguageId, msgResources);
                        return false;
                    }
                    return true;
                default:
                    // 依頼無し
                    return true;
            }
        }

        /// <summary>
        /// 承認状態登録前共通処理(workflow_detail)
        /// </summary>
        /// <param name="workflowDivision">呼出元区分、機能ID</param>
        /// <param name="button">押下ボタン</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="tantoCd">承認ユーザーID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool CheckApprovalStatusForWorkFlowDetail(string workflowDivision, string button, ComDB db, ComBusBase form, string slipNo, string tantoCd,
            ComUtil.MessageResources msgResources, ref string errorMsg, string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "")
        {
            // 呼出元区分を取得
            // 承認依頼取消は、True。それ以外は、False
            EntityDao.NamesEntity namesBean = getWfDivision(workflowDivision, button == Constants.WORKFLOW.ButtonCtrlId.ApprovalRequestCancel, db, addSearchKey);
            // レコードを取得
            EntityDao.WorkflowHeaderEntity headerBean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);
            // 更新対象レコードを取得
            int? seq = getUpdateWorkflowDetailRecord(headerBean.WfNo, tantoCd, db);
            if (seq == null)
            {
                // 取得できなかった場合、既に同じ順序のユーザーによって承認されている
                // 「別の担当者にて承認されました。」
                errorMsg = getResMessage(APResources.ID.M10170, form.LanguageId, msgResources);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 承認状態登録共通処理
        /// </summary>
        /// <param name="requestParam">内部パラメータ 11:承認依頼、12:承認依頼取消、21:承認、22:承認取消、8:否認 APConstsnts参照</param>
        /// <param name="workflowDivision">呼出元区分、機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="comments">詳細のコメントに設定する内容</param>
        /// <param name="mailParamList">メールの引数リスト</param>
        /// <param name="languageId">メールの言語ID</param>
        /// <param name="tantoCd">ユーザコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="pushInfo">out プッシュ通知の情報</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <param name="mailParamListRequest">承認時、順序の場合に次階層の承認者に承認依頼メールを送信する場合があり、そのメールの引数リスト</param>
        /// <param name="completeMailParamList">承認時、承認完了の場合に承認完了メールを送信する、そのメールの引数リスト</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool SetApprovalStatusForWorkFlow(int requestParam, string workflowDivision, string slipNo, string slipBranchNo1, string slipBranchNo2,
             string comments, List<string> mailParamList, string languageId, string tantoCd, ComDB db, out ComBusBase.PushInfo pushInfo, ref string errorMsg,
             List<string> mailParamListRequest = null, List<string> completeMailParamList = null, string addSearchKey = "")
        {
            int result = 0;
            DateTime now = DateTime.Now;
            pushInfo = new ComBusBase.PushInfo();
            int? sequence = null;

            // 呼出元区分を取得
            // 承認依頼取消は、True。それ以外は、False
            EntityDao.NamesEntity namesBean = getWfDivision(workflowDivision, requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL), db, addSearchKey);
            if (namesBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            // レコードを取得
            EntityDao.WorkflowHeaderEntity headerBean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);
            if (headerBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            // 更新処理によりレコードが変更されるので、処理前にワークフロー通知クラスを作成
            WorlflowNotice notice = new WorlflowNotice(namesBean.NameCd, headerBean, db);

            // 内部パラメータによって処理を分岐
            if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST))
            {
                // 承認依頼
                // 承認依頼の場合はこのメソッドは呼び出されないので、到達不能
                return true;
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL))
            {
                // 承認依頼取消
                headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.CANCEL;
                headerBean.DelFlg = Constants.DEL_FLG.ON; // 削除フラグをたてる
                headerBean.UpdateDate = now;       // 更新日時
                headerBean.UpdateUserId = tantoCd; // 更新者ID

                result = db.RegistByOutsideSql(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.CANCEL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL))
            {
                // 承認
                // 更新対象レコードを取得
                int? seq = getUpdateWorkflowDetailRecord(headerBean.WfNo, tantoCd, db);
                if (seq == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }
                sequence = seq;

                // 対象のワークフロー詳細レコードを取得
                EntityDao.WorkflowDetailEntity detailBean = new EntityDao.WorkflowDetailEntity();
                detailBean = detailBean.GetEntity(headerBean.PK().WfNo, (int)seq, tantoCd, db);
                if (detailBean == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                detailBean.Status = Constants.WORKFLOW.DETAILSTATUS.APPROVAL; // ステータス
                detailBean.ApprovalDate = now;     // 承認日時
                detailBean.Comments = comments;    // コメント
                detailBean.UpdateDate = now;       // 更新日時
                detailBean.UpdateUserId = tantoCd; // 更新者ID

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowDetail_Update, WorkFlow.SubDir, detailBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // 承認残があるかどうかチェックを行う
                if (!checkWorkflowCompApproval(headerBean.PK().WfNo, detailBean, db))
                {
                    // 承認残がない場合、ヘッダのステータスを"承認完了"として更新を行う
                    headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.APPROVAL;
                }
                else
                {
                    // 承認残がある場合、ヘッダのステータスを"承認中"として更新を行う
                    headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.APPROVING;
                }
                headerBean.UpdateDate = now;
                headerBean.UpdateUserId = tantoCd;

                // WFヘッダ更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.APPROVAL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL))
            {
                // 承認取消
                // 更新対象レコードを取得
                int? seq = getUpdateWorkflowDetailRecord(headerBean.WfNo, tantoCd, db, false);
                if (seq == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                // 詳細を更新
                EntityDao.WorkflowDetailEntity detailBean = new EntityDao.WorkflowDetailEntity();
                detailBean = detailBean.GetEntity(headerBean.PK().WfNo, (int)seq, tantoCd, db);
                if (detailBean == null)
                {
                    return false;
                }

                detailBean.Status = Constants.WORKFLOW.DETAILSTATUS.APPROVAL_CANCEL; // ステータス
                detailBean.ApprovalDate = null;    // 承認日時
                detailBean.Comments = comments;    // コメント
                detailBean.UpdateDate = now;       // 更新日時
                detailBean.UpdateUserId = tantoCd; // 更新者ID

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowDetail_Update, WorkFlow.SubDir, detailBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ワークフローヘッダ情報を取得
                headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.DENIAL;
                headerBean.UpdateDate = now;
                headerBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.APPROVAL_CANCEL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }
            else
            {
                // 否認
                // 更新対象レコードを取得
                int? seq = getUpdateWorkflowDetailRecord(headerBean.WfNo, tantoCd, db);
                if (seq == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                // 詳細を更新
                EntityDao.WorkflowDetailEntity detailBean = new EntityDao.WorkflowDetailEntity();
                detailBean = detailBean.GetEntity(headerBean.PK().WfNo, (int)seq, tantoCd, db);
                if (detailBean == null)
                {
                    return false;
                }

                detailBean.Status = Constants.WORKFLOW.DETAILSTATUS.DENIAL;
                detailBean.Comments = comments;
                detailBean.UpdateDate = now;
                detailBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowDetail_Update, WorkFlow.SubDir, detailBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ワークフローヘッダ情報を取得
                headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.DENIAL;
                headerBean.UpdateDate = now;
                headerBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.DENIAL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }

            // ワークフローヘッダのレコードを再取得 ※更新が走る想定なので再取得
            EntityDao.WorkflowHeaderEntity bean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);

            // 通知処理を行い、戻り値となるプッシュ通知の情報を取得
            pushInfo = notice.Notice(bean, requestParam, mailParamList, languageId, tantoCd, sequence, db, ref errorMsg, mailParamListRequest, completeMailParamList);

            return true;
        }

        /// <summary>
        /// 承認状態登録処理（承認完了の場合メール送信なし）
        /// </summary>
        /// <param name="requestParam">内部パラメータ 11:承認依頼、12:承認依頼取消、21:承認、22:承認取消、8:否認 APConstsnts参照</param>
        /// <param name="workflowDivision">呼出元区分、機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="comments">詳細のコメントに設定する内容</param>
        /// <param name="mailParamList">メールの引数リスト</param>
        /// <param name="languageId">メールの言語ID</param>
        /// <param name="tantoCd">ユーザコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="pushInfo">out プッシュ通知の情報</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <param name="mailParamListRequest">承認時、順序の場合に次階層の承認者に承認依頼メールを送信する場合があり、そのメールの引数リスト</param>
        /// <param name="completeMailParamList">承認時、承認完了の場合に承認完了メールを送信する、そのメールの引数リスト</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool SetApprovalStatusForWorkFlowNotMail(int requestParam, string workflowDivision, string slipNo, string slipBranchNo1, string slipBranchNo2,
            string comments, List<string> mailParamList, string languageId, string tantoCd, ComDB db, out ComBusBase.PushInfo pushInfo, ref string errorMsg,
            List<string> mailParamListRequest = null, List<string> completeMailParamList = null, string addSearchKey = "")
        {
            int result = 0;
            DateTime now = DateTime.Now;
            pushInfo = new ComBusBase.PushInfo();
            int? sequence = null;

            // 呼出元区分を取得
            // 承認依頼取消は、True。それ以外は、False
            EntityDao.NamesEntity namesBean = getWfDivision(workflowDivision, requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL), db, addSearchKey);
            if (namesBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            // レコードを取得
            EntityDao.WorkflowHeaderEntity headerBean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);
            if (headerBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            // 更新処理によりレコードが変更されるので、処理前にワークフロー通知クラスを作成
            WorlflowNotice notice = new WorlflowNotice(namesBean.NameCd, headerBean, db);

            // 内部パラメータによって処理を分岐
            if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST))
            {
                // 承認依頼
                // 承認依頼の場合はこのメソッドは呼び出されないので、到達不能
                return true;
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL))
            {
                // 承認依頼取消
                headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.CANCEL;
                headerBean.DelFlg = Constants.DEL_FLG.ON; // 削除フラグをたてる
                headerBean.UpdateDate = now;       // 更新日時
                headerBean.UpdateUserId = tantoCd; // 更新者ID

                result = db.RegistByOutsideSql(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.CANCEL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL))
            {
                // 承認
                // 更新対象レコードを取得
                int? seq = getUpdateWorkflowDetailRecord(headerBean.WfNo, tantoCd, db);
                if (seq == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                sequence = seq;

                // 対象のワークフロー詳細レコードを取得
                EntityDao.WorkflowDetailEntity detailBean = new EntityDao.WorkflowDetailEntity();
                detailBean = detailBean.GetEntity(headerBean.PK().WfNo, (int)seq, tantoCd, db);
                if (detailBean == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                detailBean.Status = Constants.WORKFLOW.DETAILSTATUS.APPROVAL; // ステータス
                detailBean.ApprovalDate = now;     // 承認日時
                detailBean.Comments = comments;    // コメント
                detailBean.UpdateDate = now;       // 更新日時
                detailBean.UpdateUserId = tantoCd; // 更新者ID

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowDetail_Update, WorkFlow.SubDir, detailBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // 承認残があるかどうかチェックを行う
                if (!checkWorkflowCompApproval(headerBean.PK().WfNo, detailBean, db))
                {
                    // 承認残がない場合、ヘッダのステータスを"承認完了"として更新を行う
                    headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.APPROVAL;
                }
                else
                {
                    // 承認残がある場合、ヘッダのステータスを"承認中"として更新を行う
                    headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.APPROVING;
                }
                headerBean.UpdateDate = now;
                headerBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.APPROVAL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL))
            {
                // 承認取消
                // 更新対象レコードを取得
                int? seq = getUpdateWorkflowDetailRecord(headerBean.WfNo, tantoCd, db, false);
                if (seq == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                // 詳細を更新
                EntityDao.WorkflowDetailEntity detailBean = new EntityDao.WorkflowDetailEntity();
                detailBean = detailBean.GetEntity(headerBean.PK().WfNo, (int)seq, tantoCd, db);
                if (detailBean == null)
                {
                    return false;
                }

                detailBean.Status = Constants.WORKFLOW.DETAILSTATUS.APPROVAL_CANCEL; // ステータス
                detailBean.ApprovalDate = null;    // 承認日時
                detailBean.Comments = comments;    // コメント
                detailBean.UpdateDate = now;       // 更新日時
                detailBean.UpdateUserId = tantoCd; // 更新者ID

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowDetail_Update, WorkFlow.SubDir, detailBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ワークフローヘッダ情報を取得
                headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.DENIAL;
                headerBean.UpdateDate = now;
                headerBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.APPROVAL_CANCEL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }
            else
            {
                // 否認
                // 更新対象レコードを取得
                int? seq = getUpdateWorkflowDetailRecord(headerBean.WfNo, tantoCd, db);
                if (seq == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                // 詳細を更新
                EntityDao.WorkflowDetailEntity detailBean = new EntityDao.WorkflowDetailEntity();
                detailBean = detailBean.GetEntity(headerBean.PK().WfNo, (int)seq, tantoCd, db);
                if (detailBean == null)
                {
                    return false;
                }

                detailBean.Status = Constants.WORKFLOW.DETAILSTATUS.DENIAL;
                detailBean.Comments = comments;
                detailBean.UpdateDate = now;
                detailBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowDetail_Update, WorkFlow.SubDir, detailBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ワークフローヘッダ情報を取得
                headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.DENIAL;
                headerBean.UpdateDate = now;
                headerBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.DENIAL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }

            // ワークフローヘッダのレコードを再取得 ※更新が走る想定なので再取得
            EntityDao.WorkflowHeaderEntity bean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);

            // 通知処理を行い、戻り値となるプッシュ通知の情報を取得
            // 承認完了の時はメールを送信しない
            if (bean.Status != Constants.WORKFLOW.REQUESTPARAM.APPROVAL_COMPLETE)
            {
                pushInfo = notice.NoticeMail(bean, requestParam, mailParamList, languageId, tantoCd, db, ref errorMsg, sequence, mailParamListRequest, completeMailParamList);
            }
            else
            {
                // プッシュ通知は行う
                notice.createNoticeInfo(bean, requestParam, tantoCd, sequence, languageId, db);
            }
            return true;
        }

        /// <summary>
        /// 承認状態登録処理（全てメール送信なし）
        /// </summary>
        /// <param name="requestParam">内部パラメータ 11:承認依頼、12:承認依頼取消、21:承認、22:承認取消、8:否認 APConstsnts参照</param>
        /// <param name="workflowDivision">呼出元区分、機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="comments">詳細のコメントに設定する内容</param>
        /// <param name="mailParamList">メールの引数リスト</param>
        /// <param name="languageId">メールの言語ID</param>
        /// <param name="tantoCd">ユーザコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="pushInfo">out プッシュ通知の情報</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <param name="mailParamListRequest">承認時、順序の場合に次階層の承認者に承認依頼メールを送信する場合があり、そのメールの引数リスト</param>
        /// <param name="completeMailParamList">承認時、承認完了の場合に承認完了メールを送信する、そのメールの引数リスト</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool SetApprovalStatusForWorkFlowAllNotMail(int requestParam, string workflowDivision, string slipNo, string slipBranchNo1, string slipBranchNo2,
            string comments, List<string> mailParamList, string languageId, string tantoCd, ComDB db, out ComBusBase.PushInfo pushInfo, ref string errorMsg,
            List<string> mailParamListRequest = null, List<string> completeMailParamList = null, string addSearchKey = "")
        {
            int result = 0;
            DateTime now = DateTime.Now;
            pushInfo = new ComBusBase.PushInfo();
            int? sequence = null;

            // 呼出元区分を取得
            // 承認依頼取消は、True。それ以外は、False
            EntityDao.NamesEntity namesBean = getWfDivision(workflowDivision, requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL), db, addSearchKey);
            if (namesBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            // レコードを取得
            EntityDao.WorkflowHeaderEntity headerBean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);
            if (headerBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            // 更新処理によりレコードが変更されるので、処理前にワークフロー通知クラスを作成
            WorlflowNotice notice = new WorlflowNotice(namesBean.NameCd, headerBean, db);

            // 内部パラメータによって処理を分岐
            if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST))
            {
                // 承認依頼
                // 承認依頼の場合はこのメソッドは呼び出されないので、到達不能
                return true;
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL))
            {
                // 承認依頼取消
                headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.CANCEL;
                headerBean.DelFlg = Constants.DEL_FLG.ON; // 削除フラグをたてる
                headerBean.UpdateDate = now;       // 更新日時
                headerBean.UpdateUserId = tantoCd; // 更新者ID

                result = db.RegistByOutsideSql(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.CANCEL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL))
            {
                // 承認
                // 更新対象レコードを取得
                int? seq = getUpdateWorkflowDetailRecord(headerBean.WfNo, tantoCd, db);
                if (seq == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                sequence = seq;

                // 対象のワークフロー詳細レコードを取得
                EntityDao.WorkflowDetailEntity detailBean = new EntityDao.WorkflowDetailEntity();
                detailBean = detailBean.GetEntity(headerBean.PK().WfNo, (int)seq, tantoCd, db);
                if (detailBean == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                detailBean.Status = Constants.WORKFLOW.DETAILSTATUS.APPROVAL; // ステータス
                detailBean.ApprovalDate = now;     // 承認日時
                detailBean.Comments = comments;    // コメント
                detailBean.UpdateDate = now;       // 更新日時
                detailBean.UpdateUserId = tantoCd; // 更新者ID

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowDetail_Update, WorkFlow.SubDir, detailBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // 承認残があるかどうかチェックを行う
                if (!checkWorkflowCompApproval(headerBean.PK().WfNo, detailBean, db))
                {
                    // 承認残がない場合、ヘッダのステータスを"承認完了"として更新を行う
                    headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.APPROVAL;
                }
                else
                {
                    // 承認残がある場合、ヘッダのステータスを"承認中"として更新を行う
                    headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.APPROVING;
                }
                headerBean.UpdateDate = now;
                headerBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.APPROVAL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL))
            {
                // 承認取消
                // 更新対象レコードを取得
                int? seq = getUpdateWorkflowDetailRecord(headerBean.WfNo, tantoCd, db, false);
                if (seq == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                // 詳細を更新
                EntityDao.WorkflowDetailEntity detailBean = new EntityDao.WorkflowDetailEntity();
                detailBean = detailBean.GetEntity(headerBean.PK().WfNo, (int)seq, tantoCd, db);
                if (detailBean == null)
                {
                    return false;
                }

                detailBean.Status = Constants.WORKFLOW.DETAILSTATUS.APPROVAL_CANCEL; // ステータス
                detailBean.ApprovalDate = null;    // 承認日時
                detailBean.Comments = comments;    // コメント
                detailBean.UpdateDate = now;       // 更新日時
                detailBean.UpdateUserId = tantoCd; // 更新者ID

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowDetail_Update, WorkFlow.SubDir, detailBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ワークフローヘッダ情報を取得
                headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.DENIAL;
                headerBean.UpdateDate = now;
                headerBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.APPROVAL_CANCEL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }
            else
            {
                // 否認
                // 更新対象レコードを取得
                int? seq = getUpdateWorkflowDetailRecord(headerBean.WfNo, tantoCd, db);
                if (seq == null)
                {
                    // 存在しない場合、エラーに
                    return false;
                }

                // 詳細を更新
                EntityDao.WorkflowDetailEntity detailBean = new EntityDao.WorkflowDetailEntity();
                detailBean = detailBean.GetEntity(headerBean.PK().WfNo, (int)seq, tantoCd, db);
                if (detailBean == null)
                {
                    return false;
                }

                detailBean.Status = Constants.WORKFLOW.DETAILSTATUS.DENIAL;
                detailBean.Comments = comments;
                detailBean.UpdateDate = now;
                detailBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowDetail_Update, WorkFlow.SubDir, detailBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ワークフローヘッダ情報を取得
                headerBean.Status = Constants.WORKFLOW.HEADERSTATUS.DENIAL;
                headerBean.UpdateDate = now;
                headerBean.UpdateUserId = tantoCd;

                // 更新処理
                if (!registWorkFlow(WorkFlow.WorkFlowHeader_Update, WorkFlow.SubDir, headerBean, db))
                {
                    // 失敗と判定する
                    return false;
                }

                // ログに登録
                result = updateWorkflowLog(headerBean.WfNo, tantoCd, Constants.WORKFLOW.LOGSTATUS.DENIAL, comments, now, db);
                // 更新失敗した場合、エラーに
                if (result < 0)
                {
                    return false;
                }
            }

            // ワークフローヘッダのレコードを再取得 ※更新が走る想定なので再取得
            EntityDao.WorkflowHeaderEntity bean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);

            // 通知処理を行い、戻り値となるプッシュ通知の情報を取得
            // メールを送信しない
            // プッシュ通知は行う
            notice.createNoticeInfo(bean, requestParam, tantoCd, sequence, languageId, db);
            return true;
        }

        /// <summary>
        /// ワークフロー操作ログ 更新処理
        /// </summary>
        /// <param name="workflowNo">ワークフローNO</param>
        /// <param name="tantoCd">ユーザコード</param>
        /// <param name="status">ステータス</param>
        /// <param name="comment">コメント</param>
        /// <param name="now">システム日付</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>登録件数</returns>
        /// <remarks>最大連番を取得するため、メソッド自体を排他ロックをかけている</remarks>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static int updateWorkflowLog(string workflowNo, string tantoCd, int status, string comment, DateTime now, ComDB db)
        {
            // ログ連番を付与
            // ワークフロー操作ログNoをキーに最大ログ連番を取得
            string sql = "select max(log_seq) from workflow_log where wf_no = @WfNo group by wf_no";
            int log_seq = db.GetEntity<int>(sql, new { WfNo = workflowNo });

            // Max連番+1を設定値とする
            log_seq += 1;

            // ワークフロー操作ログのデータクラスを定義
            EntityDao.WorkflowLogEntity bean = new EntityDao.WorkflowLogEntity();
            bean.WfNo = workflowNo;
            bean.LogSeq = log_seq;
            bean.UserId = tantoCd;
            bean.Operation = status;
            bean.OperationDate = now;
            bean.Comments = comment;
            bean.InputDate = now;
            bean.InputUserId = tantoCd;
            bean.UpdateDate = now;
            bean.UpdateUserId = tantoCd;

            // 登録処理を実行
            return db.RegistByOutsideSql(WorkFlow.WorkFlowLog_Insert, WorkFlow.SubDir, bean);
        }

        /// <summary>
        /// 呼出元区分の名称レコードを取得
        /// </summary>
        /// <param name="workflowDivision">呼出元区分</param>
        /// <param name="isRequest">承認依頼、承認依頼取消はTrue。それ以外はFalse</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="addSearchKey">呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>取得した名称レコード</returns>
        private static EntityDao.NamesEntity getWfDivision(string workflowDivision, bool isRequest, ComDB db, string addSearchKey)
        {
            string sql = "select * ";
            sql += "        from names ";
            sql += "       where name_division = 'WFDV' ";
            // 依頼時、extend_info1、承認時、extend_info2
            // 機能ID|FormNoなので、前半部分を指定する
            sql += "         and (split_part(" + (isRequest ? "extend_info1" : "extend_info2") + ",'|',1) = @WfDivision)";
            if (string.IsNullOrEmpty(addSearchKey))
            {
                return db.GetEntityByDataClass<EntityDao.NamesEntity>(sql, new { WfDivision = workflowDivision });
            }
            // 機能ID|FormNo|識別用キー の場合
            sql += "         and (split_part(" + (isRequest ? "extend_info1" : "extend_info2") + ",'|',3) = @AddSearchKey)";

            return db.GetEntityByDataClass<EntityDao.NamesEntity>(sql, new { WfDivision = workflowDivision, AddSearchKey = addSearchKey });
        }

        /// <summary>
        /// 呼出元区分、登録元伝票番号、登録元伝票番号枝番1、登録元伝票番号枝番2をもとに
        /// ワークフローヘッダを取得する処理
        /// </summary>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2をもとに</param>
        /// <param name="nameCd">呼出元区分</param>
        /// <param name="db">DB接続</param>
        /// <returns>ワークフローヘッダ</returns>
        private static EntityDao.WorkflowHeaderEntity getWorkflowHeader(string slipNo, string slipBranchNo1, string slipBranchNo2, string nameCd, ComDB db)
        {
            // 呼出元区分、登録元伝票番号、登録元伝票番号枝番1、登録元伝票番号枝番2をもとにワークフローヘッダを取得
            EntityDao.WorkflowHeaderEntity headerBean = new EntityDao.WorkflowHeaderEntity();
            headerBean.WfDivision = nameCd; // 呼出元区分
            headerBean.SlipNo = slipNo;               // 登録元伝票番号
            headerBean.SlipBranchNo1 = slipBranchNo1; // 登録元伝票番号枝番1
            headerBean.SlipBranchNo2 = slipBranchNo2; // 登録元伝票番号枝番2

            // レコードを取得
            return db.GetEntityByOutsideSqlByDataClass<EntityDao.WorkflowHeaderEntity>(WorkFlow.WorkFlow_GetWorkflowNo, WorkFlow.SubDir, headerBean);
        }

        /// <summary>
        /// ワークフロー詳細更新対象レコード取得
        /// </summary>
        /// <param name="workflowNo">ワークフローNo</param>
        /// <param name="tantoCd">承認ユーザコード</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="flg">true:seq最小、false:seq最大</param>
        /// <returns>ワークフローテンプレート詳細レコード</returns>
        private static int? getUpdateWorkflowDetailRecord(string workflowNo, string tantoCd, ComDB db, bool flg = true)
        {
            // テンプレートNo、有効開始日、承認ユーザコードに該当するレコードを取得し、戻す
            string sql = "select seq ";
            sql += "        from workflow_detail ";
            sql += "       where wf_no = @WfNo ";

            if (flg)
            {
                sql += "     and seq in ( select seq ";
                sql += "                  from workflow_detail ";
                sql += "                  where wf_no = @WfNo ";
                sql += "                  and approval_user_id = @ApprovalUserId) ";
                sql += "   group by seq ";
                sql += "   having min(status) is null ";
                sql += "   order by seq ";
            }
            else
            {
                sql += "     and approval_user_id = @ApprovalUserId ";
                sql += "     and status = @Status ";
                sql += "   order by seq desc ";
            }

            if (flg)
            {
                return db.GetEntityByDataClass<int?>(sql, new { WfNo = workflowNo, ApprovalUserId = tantoCd });
            }
            else
            {
                return db.GetEntityByDataClass<int?>(sql, new { WfNo = workflowNo, ApprovalUserId = tantoCd, Status = Constants.WORKFLOW.DETAILSTATUS.APPROVAL });
            }
        }

        /// <summary>
        /// 承認ワークフロー時、承認残があるかどうかチェック
        /// </summary>
        /// <param name="workflowNo">ワークフローNo</param>
        /// <param name="bean">承認ユーザ ワークフロー詳細データ</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="flg">0:承認、1:承認取消</param>
        /// <returns>true:承認残あり、false:承認残なし</returns>
        private static bool checkWorkflowCompApproval(string workflowNo, EntityDao.WorkflowDetailEntity bean, ComDB db, int flg = 0)
        {
            // 承認者が全承認権限がある場合、承認残有無関係なく、ヘッダを更新する
            // ※承認取消の場合、条件として扱わない
            if (flg == 0 && bean.AllApprovalFlg != null && bean.AllApprovalFlg == 1)
            {
                return false;
            }

            // ワークフローNoで承認残があるかどうかチェック
            //string sql = "select max(status) ";
            string sql = "select min(status) as status ";
            sql += "        from workflow_detail ";
            sql += "       where wf_no = @WfNo ";
            sql += "      group by wf_no, seq";
            IList<int?> results = db.GetList<int?>(sql, new { WfNo = workflowNo });

            for (int i = 0; i < results.Count; i++)
            {
                if (flg == 0)
                {
                    // 承認処理の場合、nullのデータが存在する場合
                    if (results[i] == null)
                    {
                        // 承認残がある場合、処理を戻す
                        return true;
                    }
                }
                else
                {
                    // 承認取消の場合、null以外のデータが存在する場合
                    if (results[i] != null)
                    {
                        // 承認がある場合、処理を戻す
                        return true;
                    }
                }
            }

            // 承認残なしとして、処理を戻す
            return false;
        }

        /// <summary>
        /// 承認ワークフローヘッダのステータス状態を確認
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="workflowDivision">呼出元区分、機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:承認残なし、false:承認残あり</returns>
        public static bool CheckApprovalRemaining(ComDB db, string workflowDivision, string slipNo, string slipBranchNo1 = null, string slipBranchNo2 = null, string addSearchKey = "")
        {
           　// 呼出元区分を取得
            EntityDao.NamesEntity namesBean = getWfDivision(workflowDivision, false, db, addSearchKey);
            if (namesBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            // レコードを取得
            EntityDao.WorkflowHeaderEntity headerBean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);
            if (headerBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            if (headerBean.Status != null && headerBean.Status == Constants.WORKFLOW.HEADERSTATUS.APPROVAL)
            {
                // 承認済みの場合、"true"を戻す
                return true;
            }
            else
            {
                // 上記以外、"false"を戻す
                return false;
            }
        }

        /// <summary>
        /// 承認依頼ワークフロー権限チェック ※ボタン制御用
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="userId">ユーザコード</param>
        /// <param name="conductId">機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:権限あり、false:権限なし</returns>
        public static bool CheckWFStatusApprovalRequest(ComDB db, string userId, string conductId, string slipNo, string slipBranchNo1 = null, string slipBranchNo2 = null, string addSearchKey = "")
        {
            return CheckWorkFlowStatusForBtnControl(db, Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST, userId, conductId, slipNo, slipBranchNo1, slipBranchNo2, addSearchKey);
        }

        /// <summary>
        /// 承認依頼取消ワークフロー権限チェック ※ボタン制御用
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="userId">ユーザコード</param>
        /// <param name="conductId">機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:権限あり、false:権限なし</returns>
        public static bool CheckWFStatusApprovalRequestCancel(ComDB db, string userId, string conductId, string slipNo, string slipBranchNo1 = null, string slipBranchNo2 = null, string addSearchKey = "")
        {
            return CheckWorkFlowStatusForBtnControl(db, Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL, userId, conductId, slipNo, slipBranchNo1, slipBranchNo2, addSearchKey);
        }

        /// <summary>
        /// 承認ワークフロー権限チェック ※ボタン制御用
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="userId">ユーザコード</param>
        /// <param name="conductId">呼出元区分、機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:権限あり、false:権限なし</returns>
        public static bool CheckWFStatusApproval(ComDB db, string userId, string conductId, string slipNo, string slipBranchNo1 = null, string slipBranchNo2 = null, string addSearchKey = "")
        {
            return CheckWorkFlowStatusForBtnControl(db, Constants.WORKFLOW.REQUESTPARAM.APPROVAL, userId, conductId, slipNo, slipBranchNo1, slipBranchNo2, addSearchKey);
        }

        /// <summary>
        /// 承認取消ワークフロー権限チェック ※ボタン制御用
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="userId">ユーザコード</param>
        /// <param name="conductId">呼出元区分、機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:権限あり、false:権限なし</returns>
        public static bool CheckWFStatusApprovalCancel(ComDB db, string userId, string conductId, string slipNo, string slipBranchNo1 = null, string slipBranchNo2 = null, string addSearchKey = "")
        {
            return CheckWorkFlowStatusForBtnControl(db, Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL, userId, conductId, slipNo, slipBranchNo1, slipBranchNo2, addSearchKey);
        }

        /// <summary>
        /// 否認ワークフロー権限チェック ※ボタン制御用
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="userId">ユーザコード</param>
        /// <param name="conductId">呼出元区分、機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:権限あり、false:権限なし</returns>
        public static bool CheckWFStatusDisApproval(ComDB db, string userId, string conductId, string slipNo, string slipBranchNo1 = null, string slipBranchNo2 = null, string addSearchKey = "")
        {
            return CheckWorkFlowStatusForBtnControl(db, Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL, userId, conductId, slipNo, slipBranchNo1, slipBranchNo2, addSearchKey);
        }

        /// <summary>
        /// ワークフロー権限チェック ※ボタン制御用
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="requestParam">内部パラメータ 11:承認依頼、12:承認依頼取消、21:承認、22:承認取消、8:否認</param>
        /// <param name="userId">ユーザコード</param>
        /// <param name="workflowDivision">呼出元区分、機能ID</param>
        /// <param name="slipNo">登録元伝票番号</param>
        /// <param name="slipBranchNo1">登録元伝票番号枝番1</param>
        /// <param name="slipBranchNo2">登録元伝票番号枝番2</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <returns>true:権限あり、false:権限なし</returns>
        public static bool CheckWorkFlowStatusForBtnControl(ComDB db, int requestParam, string userId, string workflowDivision,
            string slipNo, string slipBranchNo1 = null, string slipBranchNo2 = null, string addSearchKey = "")
        {
            // 呼出元区分を取得
            EntityDao.NamesEntity namesBean = getWfDivision(workflowDivision, requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL), db, addSearchKey);
            if (namesBean == null)
            {
                // 存在しない場合、エラーに
                return false;
            }

            // 承認依頼の場合、このメソッドでは制御を行わず、呼出元のステータスの制御に任せる
            if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST))
            {
                // 押下可で処理を戻す
                return true;
            }

            // レコードを取得
            EntityDao.WorkflowHeaderEntity headerBean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);
            if (headerBean == null)
            {
                // 承認依頼の場合、
                // 存在しない場合、エラーに
                return false;
            }

            // 内部パラメータによって処理を分岐
            if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL))
            {
                // 承認依頼取消
                // 承認依頼者であり、ワークフローステータスが"依頼中"、"承認中"の場合、押下可
                if (headerBean.RequestUserId != null && headerBean.RequestUserId.Equals(userId) &&
                   (headerBean.Status != null && (headerBean.Status == Constants.WORKFLOW.HEADERSTATUS.REQUESTING || headerBean.Status == Constants.WORKFLOW.HEADERSTATUS.APPROVING)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL))
            {
                // 承認処理
                if (!checkApprovalAuthUser(headerBean, userId, db))
                {
                    // 承認権限がない場合、処理を戻す
                    return false;
                }
                // ワークフローのステータスによって分岐
                if (headerBean.Status != null && headerBean.Status == Constants.WORKFLOW.HEADERSTATUS.REQUESTING)
                {
                    // 依頼中の場合、権限があれば押下可
                    return true;
                }
                else if (headerBean.Status != null && headerBean.Status == Constants.WORKFLOW.HEADERSTATUS.APPROVING)
                {
                    // 承認中の場合、ユーザが未承認の場合かつ、同順序のユーザが未承認の場合、押下可
                    if (!checkApprovedUser(headerBean, userId, db, requestParam))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // 上記以外、押下不可
                    return false;
                }
            }
            else if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL))
            {
                // 承認取消
                if (!checkApprovalAuthUser(headerBean, userId, db))
                {
                    // 承認権限がない場合、処理を戻す
                    return false;
                }
                // ワークフローのステータスによって分岐
                if (headerBean.Status != null && (
                    headerBean.Status == Constants.WORKFLOW.HEADERSTATUS.APPROVING ||
                    headerBean.Status == Constants.WORKFLOW.HEADERSTATUS.APPROVAL))
                {
                    // 承認中、承認完了の場合、ユーザが承認済の場合、押下可
                    if (checkApprovedUser(headerBean, userId, db, requestParam))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // 上記以外、押下不可
                    return false;
                }
            }
            else
            {
                // 否認
                if (!checkApprovalAuthUser(headerBean, userId, db))
                {
                    // 承認権限がない場合、処理を戻す
                    return false;
                }
                // ワークフローのステータスによって分岐
                if (headerBean.Status != null && headerBean.Status == Constants.WORKFLOW.HEADERSTATUS.REQUESTING)
                {
                    // 依頼中の場合、権限があれば押下可
                    return true;
                }
                else if (headerBean.Status != null && headerBean.Status == Constants.WORKFLOW.HEADERSTATUS.APPROVING)
                {
                    // 承認中の場合、ユーザが未承認の場合、押下可
                    if (!checkApprovedUser(headerBean, userId, db, requestParam))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // 上記以外、押下不可
                    return false;
                }
            }
        }

        /// <summary>
        /// 承認権限有無チェック
        /// </summary>
        /// <param name="bean">承認ワークフローヘッダ</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:権限あり、false:権限なし</returns>
        private static bool checkApprovalAuthUser(EntityDao.WorkflowHeaderEntity bean, string userId, ComDB db)
        {
            string sql = string.Empty;
            sql += "select * ";
            sql += "  from workflow_detail ";
            sql += " where wf_no = @WfNo ";
            sql += "   and approval_user_id = @ApprovalUserId ";

            IList<EntityDao.WorkflowDetailEntity> list = db.GetListByDataClass<EntityDao.WorkflowDetailEntity>(sql,
                new { WfNo = bean.WfNo, ApprovalUserId = userId });
            if (list != null && list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ユーザが承認済みかどうかをチェック
        /// </summary>
        /// <param name="bean">承認ワークフローヘッダ</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="requestParam">内部パラメータ 21:承認、22:承認取消、8:否認</param>
        /// <returns>true:承認済み、false:未承認含む</returns>
        /// <remarks>未承認レコードが1件でも存在すると未承認とみなす</remarks>
        private static bool checkApprovedUser(EntityDao.WorkflowHeaderEntity bean, string userId, ComDB db, int requestParam)
        {
            string sql = string.Empty;
            sql += "select min(status) as status ";
            sql += "  from workflow_detail ";
            sql += " where wf_no = @WfNo ";
            sql += " and seq in ( select seq ";
            sql += "              from workflow_detail ";
            sql += "              where wf_no = @WfNo ";
            sql += "              and approval_user_id = @ApprovalUserId) ";
            sql += " group by seq ";
            sql += " having min(status) is null ";

            IList<EntityDao.WorkflowDetailEntity> list = db.GetListByDataClass<EntityDao.WorkflowDetailEntity>(sql,
                new { WfNo = bean.WfNo, ApprovalUserId = userId });
            if (list != null && list.Count > 0)
            {
                return false;
            }
            else
            {
                // 未承認レコードがない場合
                if (requestParam.Equals(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL))
                {
                    // 承認取消の場合、自分で承認したデータが存在しない場合false
                    sql = string.Empty;
                    sql += "select * ";
                    sql += "  from workflow_detail ";
                    sql += " where wf_no = @WfNo ";
                    sql += "   and approval_user_id = @ApprovalUserId ";
                    sql += "   and status = 10 ";
                    IList<EntityDao.WorkflowDetailEntity> list2 = db.GetListByDataClass<EntityDao.WorkflowDetailEntity>(sql,
                    new { WfNo = bean.WfNo, ApprovalUserId = userId });
                    if (list2 == null || list2.Count <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// ワークフロー関連、更新処理を行う
        /// </summary>
        /// <param name="sqlName">SQL名</param>
        /// <param name="sqlPath">SQLファイルパス</param>
        /// <param name="param">更新条件</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:正常、false:異常</returns>
        private static bool registWorkFlow(string sqlName, string sqlPath, object param, ComDB db)
        {
            // 更新処理を行う
            int result = db.RegistByOutsideSql(sqlName, sqlPath, param);
            if (result < 1)
            {
                // 更新レコードが１件未満の場合、異常と判定する
                return false;
            }
            return true;
        }

        /// <summary>
        /// ワークフロー通知クラス
        /// </summary>
        private class WorlflowNotice
        {
            /// <summary>名称マスタのワークフローの情報から取得した呼出元区分</summary>
            private string workflowDivision;
            /// <summary>ワークフローヘッダの情報</summary>
            private EntityDao.WorkflowHeaderEntity workflowHeader;
            /// <summary>ワークフローテンプレートの情報</summary>
            private EntityDao.WorkflowTemplateHeaderEntity workflowTempHeader;
            /// <summary>DB接続</summary>
            private ComDB db;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="workflowDivision">名称マスタのワークフローの情報から取得した呼出元区分</param>
            /// <param name="workflowHeader">ワークフローヘッダの情報</param>
            /// <param name="db">DB接続</param>
            public WorlflowNotice(string workflowDivision, EntityDao.WorkflowHeaderEntity workflowHeader, ComDB db)
            {
                this.db = db;
                this.workflowDivision = workflowDivision;
                this.workflowHeader = workflowHeader;
                // ワークフローテンプレートを取得
                workflowTempHeader = new EntityDao.WorkflowTemplateHeaderEntity().GetEntity(int.Parse(workflowHeader.WfTemplateNo), workflowHeader.ActiveDate ?? DateTime.Now, db);
                if (workflowTempHeader == null)
                {
                    return;
                }
            }

            /// <summary>
            /// 通知処理
            /// </summary>
            /// <param name="bean">ワークフローヘッダ情報</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="mailParamList">メールの引数リスト</param>
            /// <param name="languageId">メールの言語ID</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="seq">処理を行った順序</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="errorMsg">エラーメッセージ</param>
            /// <param name="mailParamListRequest">承認時、順序の場合に次階層の承認者に承認依頼メールを送信する場合があり、そのメールの引数リスト</param>
            /// <param name="mailParamListAppComp">承認時、承認完了の場合に操作ユーザ以外に完了メールを送信する場合があり、そのメールの引数リスト</param>
            /// <returns>プッシュ通知の情報</returns>
            public ComBusBase.PushInfo Notice(EntityDao.WorkflowHeaderEntity bean, int requestParam, List<string> mailParamList, string languageId, string tantoCd,
                int? seq, ComDB db, ref string errorMsg, List<string> mailParamListRequest = null, List<string> mailParamListAppComp = null)
            {
                if (this.isError())
                {
                    // 必要な情報が取得できなかった場合、終了
                    return null;
                }

                // メソッドの戻り値　プッシュ通知情報
                var pushInfo = new ComBusBase.PushInfo();

                if (mailParamList is null)
                {
                    // メール送信設定がされていない場合は取込操作と判断して
                    // 非通知、非メールの状態で終了 20220525 S.Fujimaki
                    return pushInfo;
                }

                //// プッシュ通知を行うかどうか判定
                //if (this.isNoticePush(requestParam))
                //{
                //    // 通知情報テーブルにレコードを生成
                //    if (!createNoticeInfo(bean, requestParam, tantoCd, seq, languageId, db))
                //    {
                //        return pushInfo;
                //    }

                //    // テンプレートでプッシュ通知する設定の場合、プッシュ通知情報取得
                //    pushInfo = this.getPush(requestParam, tantoCd, db, seq);
                //}
                //else
                //{
                //    // 承認完了以外のプッシュ通知が設定されている場合、自身にもプッシュ通知を設定
                //    if (this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL))
                //    {
                //        // ユーザIDを取得
                //        List<string> useridList = new List<string>();
                //        useridList.Add(tantoCd);

                //        pushInfo = new ComBusBase.PushInfo(useridList, db);
                //    }
                //}

                // 通知情報テーブルにレコードを生成
                if (!createNoticeInfo(bean, requestParam, tantoCd, seq, languageId, db))
                {
                    return pushInfo;
                }

                // テンプレートでプッシュ通知する設定の場合、プッシュ通知情報取得
                pushInfo = this.getPush(requestParam, tantoCd, db, seq);

                // メール送信を行うかどうか判定
                if (this.isSendMail(requestParam))
                {
                    // テンプレートでメール送信する設定の場合、メール送信
                    if (this.sendMail(requestParam, tantoCd, mailParamList, languageId, ref errorMsg, mailParamListRequest, mailParamListAppComp, seq))
                    {
                        return pushInfo;
                    }
                }

                return pushInfo;
            }

            /// <summary>
            /// 通知処理(メール送信無)
            /// </summary>
            /// <param name="bean">ワークフローヘッダ情報</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="mailParamList">メールの引数リスト</param>
            /// <param name="languageId">メールの言語ID</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="seq">処理を行った順序</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="errorMsg">エラーメッセージ</param>
            /// <param name="mailParamListRequest">承認時、順序の場合に次階層の承認者に承認依頼メールを送信する場合があり、そのメールの引数リスト</param>
            /// <param name="mailParamListAppComp">承認時、承認完了の場合に操作ユーザ以外に完了メールを送信する場合があり、そのメールの引数リスト</param>
            /// <returns>プッシュ通知の情報</returns>
            public ComBusBase.PushInfo NoticeNotMail(EntityDao.WorkflowHeaderEntity bean, int requestParam, List<string> mailParamList, string languageId, string tantoCd,
                int? seq, ComDB db, ref string errorMsg, List<string> mailParamListRequest = null, List<string> mailParamListAppComp = null)
            {
                if (this.isError())
                {
                    // 必要な情報が取得できなかった場合、終了
                    return null;
                }

                // メソッドの戻り値　プッシュ通知情報
                var pushInfo = new ComBusBase.PushInfo();

                if (mailParamList is null)
                {
                    // メール送信設定がされていない場合は取込操作と判断して
                    // 非通知、非メールの状態で終了 20220525 S.Fujimaki
                    return pushInfo;
                }

                //// プッシュ通知を行うかどうか判定
                //if (this.isNoticePush(requestParam))
                //{
                //    // 通知情報テーブルにレコードを生成
                //    if (!createNoticeInfo(bean, requestParam, tantoCd, seq, languageId, db))
                //    {
                //        return pushInfo;
                //    }

                //    // テンプレートでプッシュ通知する設定の場合、プッシュ通知情報取得
                //    pushInfo = this.getPush(requestParam, tantoCd, db, seq);
                //}
                //else
                //{
                //    // 承認完了以外のプッシュ通知が設定されている場合、自身にもプッシュ通知を設定
                //    if (this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL))
                //    {
                //        // ユーザIDを取得
                //        List<string> useridList = new List<string>();
                //        useridList.Add(tantoCd);

                //        pushInfo = new ComBusBase.PushInfo(useridList, db);
                //    }
                //}

                // 通知情報テーブルにレコードを生成
                if (!createNoticeInfo(bean, requestParam, tantoCd, seq, languageId, db))
                {
                    return pushInfo;
                }

                // テンプレートでプッシュ通知する設定の場合、プッシュ通知情報取得
                pushInfo = this.getPush(requestParam, tantoCd, db, seq);

                return pushInfo;
            }

            /// <summary>
            /// 通知処理
            /// </summary>
            /// <param name="bean">ワークフローヘッダ情報</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="mailParamList">メールの引数リスト</param>
            /// <param name="languageId">メールの言語ID</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="errorMsg">エラーメッセージ</param>
            /// <param name="seq">処理を行った順序</param>
            /// <param name="mailParamListRequest">承認時、順序の場合に次階層の承認者に承認依頼メールを送信する場合があり、そのメールの引数リスト</param>
            /// <param name="mailParamListAppComp">承認時、承認完了の場合に操作ユーザ以外に完了メールを送信する場合があり、そのメールの引数リスト</param>
            /// <param name="mailFlg">メールフラグ</param>
            /// <returns>プッシュ通知の情報</returns>
            public ComBusBase.PushInfo NoticeMail(EntityDao.WorkflowHeaderEntity bean, int requestParam, List<string> mailParamList, string languageId, string tantoCd,
                ComDB db, ref string errorMsg, int? seq, List<string> mailParamListRequest = null, List<string> mailParamListAppComp = null, bool mailFlg = false)
            {
                if (this.isError())
                {
                    // 必要な情報が取得できなかった場合、終了
                    return null;
                }

                // メソッドの戻り値　プッシュ通知情報
                var pushInfo = new ComBusBase.PushInfo();
                // プッシュ通知を行うかどうか判定
                //if (this.isNoticePush(requestParam))
                //{
                //    // 通知情報テーブルにレコードを生成
                //    if (!createNoticeInfo(bean, requestParam, tantoCd, seq, languageId, db))
                //    {
                //        return pushInfo;
                //    }

                //    // テンプレートでプッシュ通知する設定の場合、プッシュ通知情報取得
                //    pushInfo = this.getPush(requestParam, tantoCd, db, seq);
                //}
                //else
                //{
                //    // 承認完了以外のプッシュ通知が設定されている場合、自身にもプッシュ通知を設定
                //    if (this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL) ||
                //        this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL))
                //    {
                //        // ユーザIDを取得
                //        List<string> useridList = new List<string>();
                //        useridList.Add(tantoCd);

                //        pushInfo = new ComBusBase.PushInfo(useridList, db);
                //    }
                //}

                // 通知情報テーブルにレコードを生成
                if (!createNoticeInfo(bean, requestParam, tantoCd, seq, languageId, db))
                {
                    return pushInfo;
                }

                // テンプレートでプッシュ通知する設定の場合、プッシュ通知情報取得
                pushInfo = this.getPush(requestParam, tantoCd, db, seq);

                // メール送信を行うかどうか判定
                if (this.isSendMail(requestParam))
                {
                    // テンプレートでメール送信する設定の場合、メール送信
                    if (this.sendMail(requestParam, tantoCd, mailParamList, languageId, ref errorMsg, mailParamListRequest, mailParamListAppComp))
                    {
                        return pushInfo;
                    }
                }

                return pushInfo;
            }

            /// <summary>
            /// 通知クラスで処理が行えるかどうかを判定する
            /// </summary>
            /// <returns>処理が行えない場合、True</returns>
            private bool isError()
            {
                if (workflowDivision == null || workflowHeader == null || workflowTempHeader == null || db == null)
                {
                    // いずれかがNullならエラー
                    return true;
                }
                return false;
            }

            /// <summary>
            /// プッシュ通知を行うかどうか取得
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <returns>行う場合True</returns>
            private bool isNoticePush(int requestParam)
            {
                return isNotice(requestParam, true);
            }
            /// <summary>
            /// メール送信を行うかどうか取得
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <returns>行う場合True</returns>
            private bool isSendMail(int requestParam)
            {
                return isNotice(requestParam, false);
            }

            /// <summary>
            /// 通知を行うかどうかをワークフローテンプレートより取得する
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="isPush">プッシュ通知の場合True、メールの場合False</param>
            /// <returns>通知する場合True</returns>
            private bool isNotice(int requestParam, bool isPush)
            {
                // 内部パラメータごとに分岐
                switch (requestParam)
                {
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST:
                        // 承認依頼
                        return isPush ? checkIntFlag(workflowTempHeader.RequestPushFlg) : checkIntFlag(workflowTempHeader.RequestMailFlg);
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL:
                        // 承認依頼取消
                        return isPush ? checkIntFlag(workflowTempHeader.RequestCancelPushFlg) : checkIntFlag(workflowTempHeader.RequestCancelMailFlg);
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL:
                        if (getIsApprovalComplete())
                        {
                            // 承認完了
                            return isPush ? checkIntFlag(workflowTempHeader.CompletePushFlg) : checkIntFlag(workflowTempHeader.CompleteMailFlg);
                        }
                        else
                        {
                            // 承認
                            return isPush ? checkIntFlag(workflowTempHeader.ApprovalPushFlg) : checkIntFlag(workflowTempHeader.ApprovalMailFlg);
                        }
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL:
                        // 承認取消
                        return isPush ? checkIntFlag(workflowTempHeader.ApprovalCancelPushFlg) : checkIntFlag(workflowTempHeader.ApprovalCancelMailFlg);
                    case Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL:
                        // 否認
                        // 全員
                        return isPush ? checkIntFlag(workflowTempHeader.RejectPushFlg) : checkIntFlag(workflowTempHeader.RejectMailFlg);
                    default:
                        // 到達不能
                        return false;
                }
            }

            /// <summary>
            /// 共通通知情報作成メイン処理
            /// </summary>
            /// <param name="bean">ワークフローヘッダ情報</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="seq">処理を行った順序</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>true:正常、false;異常</returns>
            public bool createNoticeInfo(EntityDao.WorkflowHeaderEntity bean, int requestParam, string tantoCd, int? seq, string languageId, ComDB db)
            {
                // 通知区分が未設定の場合、処理を実施しない
                if (bean == null || bean.NoticeDivision == null)
                {
                    return true;
                }

                // 通知区分によって生成方法を分岐
                if (bean.NoticeDivision == (int)Constants.WORKFLOW.NOTICEDIVISION.SEQUENTIAL)
                {
                    // 順序
                    return sequential(bean, requestParam, tantoCd, seq, languageId, db);
                }
                else if (bean.NoticeDivision == (int)Constants.WORKFLOW.NOTICEDIVISION.PARALLEL)
                {
                    // 平行
                    return parallel(bean, requestParam, tantoCd, seq, languageId, db);
                }
                else
                {
                    // 上記以外は処理をしない。到達する想定なし。
                    return true;
                }
            }

            /// <summary>
            /// 通知情報生成メイン処理 ※順序
            /// </summary>
            /// <param name="bean">ワークフローヘッダ情報</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="seq">処理を行った順序</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>true:正常、false;異常</returns>
            private bool sequential(EntityDao.WorkflowHeaderEntity bean, int requestParam, string tantoCd, int? seq, string languageId, ComDB db)
            {
                // ワークフロー詳細データを取得
                IList<EntityDao.WorkflowDetailEntity> lists = getWorkflowDetailInfo(bean.WfNo, db);
                if (lists == null || lists.Count == 0)
                {
                    // レコードが取得できない場合、処理を中断
                    return true;
                }

                // 通知内容を設定
                // 処理メッセージ
                string noticeContents = setNoticeContents(bean, requestParam, languageId, db);
                // 承認依頼メッセージ ※承認時、承認完了でない場合、次のユーザに承認依頼を設定するため
                string approvalRequestMsg = setNoticeContents(bean, Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST, languageId, db);

                // 内部パラメータによって処理を分岐
                switch (requestParam)
                {
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST:
                        // 承認依頼の場合
                        // プッシュ通知を行う場合、直近の未承認者に対して通知情報を生成
                        if (this.isNoticePush(requestParam) && !pushToNextApprovalRequestUser(bean, noticeContents, tantoCd, requestParam, db))
                        {
                            return false;
                        }

                        // 自身の通知情報を削除
                        return deleteNoticeInfoByKeyNo(bean.WfNo, tantoCd, db);
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL:
                        // 承認依頼取消の場合
                        // プッシュ通知を行う場合、承認済の承認者に対して通知情報を生成
                        if (this.isNoticePush(requestParam) && !pushToAllApprovalUserByCompApproval(bean, noticeContents, tantoCd, requestParam, db))
                        {
                            return false;
                        }

                        // 全員の通知情報を削除
                        return deleteNoticeInfoByKeyNo(bean.WfNo, null, db);
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL:
                        if (getIsApprovalComplete())
                        {
                            // 承認完了の場合
                            // 全員の通知情報を削除
                            if (!deleteNoticeInfoByKeyNo(bean.WfNo, null, db))
                            {
                                return false;
                            }

                            // プッシュ通知を行う場合、承認完了の内容を設定する
                            if (this.isNoticePush(requestParam))
                            {
                                string tmpNoticeContents = setNoticeContents(bean, Constants.WORKFLOW.REQUESTPARAM.APPROVAL_COMPLETE, languageId, db);

                                // 承認依頼者に通知情報を生成
                                if (!pushToApprovalRequestUser(bean, tmpNoticeContents, tantoCd, requestParam, db, false))
                                {
                                    return false;
                                }

                                //承認者以外の承認担当者用メッセージを取得
                                string tmpNoticeContents2 = "";
                                if (requestParam == Constants.WORKFLOW.REQUESTPARAM.APPROVAL)
                                {
                                    // 処理メッセージ
                                    tmpNoticeContents2 = setNoticeContents(bean, Constants.WORKFLOW.REQUESTPARAM.APPROVAL_COMPLETE_ANOTHER, languageId, db);
                                }

                                // 操作ユーザ以外の全員に通知情報を設定
                                return pushToAllApprovalUser(bean, tmpNoticeContents2, tantoCd, requestParam, db, false, true);
                            }

                            return true;
                        }
                        else
                        {
                            // 承認完了していない場合
                            // プッシュ通知を行う場合、承認依頼者に通知情報を生成
                            if (this.isNoticePush(requestParam) && !pushToApprovalRequestUser(bean, noticeContents, tantoCd, requestParam, db, false))
                            {
                                return false;
                            }

                            // 自身の通知情報を削除
                            if (!deleteNoticeInfoByKeyNo(bean.WfNo, tantoCd, db))
                            {
                                return false;
                            }

                            // 同じ順序の通知情報を削除&生成
                            if (!deleteNoticeInfoBySeq(bean.WfNo, tantoCd, seq, db, bean, noticeContents, requestParam, false))
                            {
                                return false;
                            }

                            // プッシュ通知を行う場合、更新した順序以下の順序で一人も承認していない順序が存在しない場合直近の承認者に対して通知情報を生成
                            if (this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST) && !checkApprovalUserBySequential(bean.WfNo, tantoCd, db))
                            {
                                // 承認依頼の内容を設定する
                                return pushToNextApprovalRequestUser(bean, approvalRequestMsg, tantoCd, Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST, db);
                            }

                            return true;
                        }
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL:
                    case Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL:
                        // 承認取消、否認の場合
                        // 全員の通知情報を削除
                        if (!deleteNoticeInfoByKeyNo(bean.WfNo, null, db))
                        {
                            return false;
                        }

                        // プッシュ通知を行う場合、否認の場合否認者以外の承認担当者用メッセージを取得
                        if (this.isNoticePush(requestParam))
                        {
                            string noticeContents2 = "";
                            if (requestParam == Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL)
                            {
                                // 処理メッセージ
                                noticeContents2 = setNoticeContents(bean, Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL_ANOTHER, languageId, db);
                            }

                            // 操作ユーザ以外の全員に通知情報を設定
                            if (!pushToAllApprovalUser(bean, noticeContents2, tantoCd, requestParam, db, false, true))
                            {
                                return false;
                            }

                            // 承認依頼者に通知情報を生成
                            return pushToApprovalRequestUser(bean, noticeContents, tantoCd, requestParam, db);
                        }

                        return true;
                    default:
                        // 到達不能
                        return true;
                }
            }

            /// <summary>
            /// 通知情報生成メイン処理 ※並列
            /// </summary>
            /// <param name="bean">ワークフローヘッダ情報</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="seq">処理を行った順序</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>true:正常、false;異常</returns>
            private bool parallel(EntityDao.WorkflowHeaderEntity bean, int requestParam, string tantoCd, int? seq, string languageId, ComDB db)
            {
                // ワークフロー詳細データを取得
                IList<EntityDao.WorkflowDetailEntity> lists = getWorkflowDetailInfo(bean.WfNo, db);
                if (lists == null || lists.Count == 0)
                {
                    // レコードが取得できない場合、処理を中断
                    return true;
                }

                // 通知内容を設定
                // 処理メッセージ
                string noticeContents = setNoticeContents(bean, requestParam, languageId, db);

                // 内部パラメータによって処理を分岐
                switch (requestParam)
                {
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST:
                        // 承認依頼の場合
                        // プッシュ通知を行う場合、承認担当者全員に通知情報を生成
                        if (this.isNoticePush(requestParam) && !pushToAllApprovalUser(bean, noticeContents, tantoCd, requestParam, db))
                        {
                            return false;
                        }

                        // 自身の通知情報を削除
                        return deleteNoticeInfoByKeyNo(bean.WfNo, tantoCd, db);
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL:
                        // 承認依頼取消の場合
                        // プッシュ通知を行う場合、承認担当者全員に通知情報を生成
                        if (this.isNoticePush(requestParam) && !pushToAllApprovalUser(bean, noticeContents, tantoCd, requestParam, db, true, true))
                        {
                            return false;
                        }

                        // 全員の通知情報を削除
                        return deleteNoticeInfoByKeyNo(bean.WfNo, null, db);
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL:
                        if (getIsApprovalComplete())
                        {
                            // 承認完了の場合
                            // 全員の通知情報を削除
                            if (!deleteNoticeInfoByKeyNo(bean.WfNo, null, db))
                            {
                                return false;
                            }

                            // 承認完了の内容を設定する
                            string tmpNoticeContents = setNoticeContents(bean, Constants.WORKFLOW.REQUESTPARAM.APPROVAL_COMPLETE, languageId, db);

                            // プッシュ通知を行う場合、承認依頼者に通知情報を生成
                            if (this.isNoticePush(requestParam) && !pushToApprovalRequestUser(bean, tmpNoticeContents, tantoCd, requestParam, db, false))
                            {
                                return false;
                            }

                            // プッシュ通知を行う場合、操作ユーザ以外の全員に通知情報を設定
                            if (this.isNoticePush(requestParam))
                            {
                                return pushToAllApprovalUser(bean, tmpNoticeContents, tantoCd, requestParam, db, false, true);
                            }

                            return true;
                        }
                        else
                        {
                            // 承認の場合
                            // プッシュ通知を行う場合、承認依頼者に通知情報を生成
                            if (this.isNoticePush(requestParam) && !pushToApprovalRequestUser(bean, noticeContents, tantoCd, requestParam, db, false))
                            {
                                return false;
                            }

                            // 同じ順序の通知情報を削除&生成
                            if (!deleteNoticeInfoBySeq(bean.WfNo, tantoCd, seq, db, bean, noticeContents, requestParam, false))
                            {
                                return false;
                            }

                            // 自身の通知情報を削除
                            return deleteNoticeInfoByKeyNo(bean.WfNo, tantoCd, db);
                        }
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL:
                    case Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL:
                        // 承認取消、否認の場合
                        // 全員の通知情報を削除
                        if (!deleteNoticeInfoByKeyNo(bean.WfNo, null, db))
                        {
                            return false;
                        }

                        // プッシュ通知を行う場合、操作ユーザ以外の全員に通知情報を設定
                        if (this.isNoticePush(requestParam) && !pushToAllApprovalUser(bean, noticeContents, tantoCd, requestParam, db, false, true))
                        {
                            return false;
                        }

                        // プッシュ通知を行う場合、承認依頼者に通知情報を生成
                        if (this.isNoticePush(requestParam))
                        {
                            return pushToApprovalRequestUser(bean, noticeContents, tantoCd, requestParam, db);
                        }

                        return true;
                    default:
                        // 到達不能
                        return true;
                }
            }

            /// <summary>
            /// 通知内容付加文字取得
            /// </summary>
            /// <param name="db">DB操作クラス</param>
            /// <returns>汎用マスタ一覧</returns>
            private IList<EntityDao.UtilityEntity> getAddNoticeContents(ComDB db)
            {
                // SQL文生成
                string sql = "select * from utility where utility_division = @UtilityDivision";

                return db.GetListByDataClass<EntityDao.UtilityEntity>(sql, new { UtilityDivision = WorkFlow.NoticeContentsUtilityCd });
            }

            /// <summary>
            /// 通知内容設定
            /// </summary>
            /// <param name="bean">ワークフローヘッダデータ</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="list">汎用マスタデータ一覧 ※省略可</param>
            /// <returns>通知内容 ※取得できない場合、null</returns>
            private string setNoticeContents(EntityDao.WorkflowHeaderEntity bean, int requestParam, string languageId, ComDB db, IList<EntityDao.UtilityEntity> list = null)
            {
                // 付加文字を設定 ※初期値としてデフォルト値を設定
                string addStr = WorkFlow.NoticeContentsDef;
                if (list == null)
                {
                    // 汎用マスタデータが渡ってきていない場合、ここで取得
                    list = getAddNoticeContents(db);
                }

                // 汎用マスタの定義分、判定を実施
                foreach (var data in list)
                {
                    // ワークフローヘッダ.呼出元区分と一致する付加文字を設定
                    if (data.UtilityCd.Equals(bean.WfDivision))
                    {
                        // 値が設定されている場合のみマスタ値を設定 ※未指定の場合、デフォルト値を設定
                        if (!string.IsNullOrEmpty(data.Name01))
                        {
                            addStr = data.Name01;
                        }
                        // 処理を抜ける
                        break;
                    }
                }

                // メール情報を生成
                EntityDao.MailTemplateEntity mailInfo = getMailTextInfo(requestParam, languageId);
                if (mailInfo == null)
                {
                    return null;
                }

                // 取得できた場合、件名を設定 （例）【AP21】【受注】承認依頼が届いています (伝票番号：XXXXXXXXXXXX)
                return mailInfo.TextTitle + " (" + addStr + "：" + bean.SlipNo + ")";
            }

            /// <summary>
            /// 直近の未承認者に対して通知情報を生成
            /// </summary>
            /// <param name="bean">ワークフローデータ</param>
            /// <param name="noticeContents">メール本文</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>true:正常、false:異常</returns>
            private bool pushToNextApprovalRequestUser(EntityDao.WorkflowHeaderEntity bean, string noticeContents, string tantoCd, int requestParam, ComDB db)
            {
                // ワークフロー詳細に設定されているユーザを取得
                IList<EntityDao.WorkflowDetailEntity> list = getNextUserWorkflowDetailInfo(bean.WfNo, db);
                foreach (var data in list)
                {
                    // 取得できた件数分、通知情報を生成
                    EntityDao.NoticeInfoEntity insertBean = new EntityDao.NoticeInfoEntity();

                    insertBean.UserId = data.ApprovalUserId;                     // ユーザーID
                    insertBean.NoticeDivision = setNoticeDivision(requestParam); // 通知区分
                    insertBean.NoticeClass = setNoticeClass(requestParam);       // 通知分類
                    insertBean.NoticeContents = noticeContents;                  // 通知内容
                    insertBean.KeyNo = bean.WfNo;                                // キー番号 ※通知元情報紐づけるためのキー

                    // 更新処理
                    if (!insertNoticeInfo(insertBean, tantoCd, db))
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// 承認依頼者に対して通知情報を生成
            /// </summary>
            /// <param name="bean">ワークフローデータ</param>
            /// <param name="noticeContents">メール本文</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="db">DB操作クラス</param>
            /// <param param name="noticeFlg">true:通知、false:お知らせ</param>
            /// <returns>true:正常、false:異常</returns>
            private bool pushToApprovalRequestUser(EntityDao.WorkflowHeaderEntity bean, string noticeContents, string tantoCd,
                int requestParam, ComDB db, bool noticeFlg = true)
            {
                // 通知情報を生成
                EntityDao.NoticeInfoEntity insertBean = new EntityDao.NoticeInfoEntity();

                int noticeDivision = noticeFlg ? Constants.NOTICE_INFO.NOTICE_DIVISION.NOTICE : Constants.NOTICE_INFO.NOTICE_DIVISION.NEWS;

                insertBean.UserId = bean.RequestUserId;                // ユーザーID
                insertBean.NoticeDivision = noticeDivision;            // 通知区分
                insertBean.NoticeClass = setNoticeClass(requestParam); // 通知分類
                insertBean.NoticeContents = noticeContents;            // 通知内容
                insertBean.KeyNo = bean.WfNo;                          // キー番号 ※通知元情報紐づけるためのキー

                // 更新処理
                if (!insertNoticeInfo(insertBean, tantoCd, db))
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 承認担当者全員に対して通知情報を生成 ※承認済みのユーザのみを対象
            /// </summary>
            /// <param name="bean">ワークフローデータ</param>
            /// <param name="noticeContents">メール本文</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>true:正常、false:異常</returns>
            private bool pushToAllApprovalUserByCompApproval(EntityDao.WorkflowHeaderEntity bean, string noticeContents, string tantoCd,
                int requestParam, ComDB db)
            {
                // ワークフロー詳細に設定されているユーザを取得
                IList<EntityDao.WorkflowDetailEntity> list = getWorkflowDetailInfo(bean.WfNo, db);
                foreach (var data in list)
                {
                    // 承認済以外のユーザは対象外
                    if (!data.Status.Equals(Constants.WORKFLOW.DETAILSTATUS.APPROVAL))
                    {
                        continue;
                    }

                    // 取得できた件数分、通知情報を生成
                    EntityDao.NoticeInfoEntity insertBean = new EntityDao.NoticeInfoEntity();

                    insertBean.UserId = data.ApprovalUserId;                     // ユーザーID
                    insertBean.NoticeDivision = setNoticeDivision(requestParam); // 通知区分
                    insertBean.NoticeClass = setNoticeClass(requestParam);       // 通知分類
                    insertBean.NoticeContents = noticeContents;                  // 通知内容
                    insertBean.KeyNo = bean.WfNo;                                // キー番号 ※通知元情報紐づけるためのキー

                    // 更新処理
                    if (!insertNoticeInfo(insertBean, tantoCd, db))
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// 承認担当者全員に対して通知情報を生成
            /// </summary>
            /// <param name="bean">ワークフローデータ</param>
            /// <param name="noticeContents">メール本文</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="insertFlg">true:対象者全員生成、false:操作ユーザは除外</param>
            /// <param name="skipFlg">true:生成済みユーザの場合、処理をスキップ、false:処理をスキップしない</param>
            /// <returns>true:正常、false:異常</returns>
            private bool pushToAllApprovalUser(EntityDao.WorkflowHeaderEntity bean, string noticeContents, string tantoCd,
                int requestParam, ComDB db, bool insertFlg = true, bool skipFlg = false)
            {
                // 生成済ユーザをスキップするために、変数を定義
                List<string> skipUserList = new List<string>();

                // ワークフロー詳細に設定されているユーザを取得
                IList<EntityDao.WorkflowDetailEntity> list = getWorkflowDetailInfo(bean.WfNo, db);
                foreach (var data in list)
                {
                    // 操作ユーザはデータ生成除外
                    if (!insertFlg && data.ApprovalUserId.Equals(tantoCd))
                    {
                        continue;
                    }

                    // 既に設定済みのユーザの場合、処理をスキップ
                    if (skipFlg && skipUserList.Contains(data.ApprovalUserId))
                    {
                        continue;
                    }
                    if (!skipUserList.Contains(data.ApprovalUserId))
                    {
                        skipUserList.Add(data.ApprovalUserId);
                    }

                    // 取得できた件数分、通知情報を生成
                    EntityDao.NoticeInfoEntity insertBean = new EntityDao.NoticeInfoEntity();

                    insertBean.UserId = data.ApprovalUserId;                     // ユーザーID
                    insertBean.NoticeDivision = setNoticeDivision(requestParam); // 通知区分
                    insertBean.NoticeClass = setNoticeClass(requestParam);       // 通知分類
                    insertBean.NoticeContents = noticeContents;                  // 通知内容
                    insertBean.KeyNo = bean.WfNo;                                // キー番号 ※通知元情報紐づけるためのキー

                    // 更新処理
                    if (!insertNoticeInfo(insertBean, tantoCd, db))
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// ワークフロー詳細データを取得 ※ワークフローNo単位
            /// </summary>
            /// <param name="workflowNo">ワークフローNo</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>ワークフロー詳細データ</returns>
            private IList<EntityDao.WorkflowDetailEntity> getWorkflowDetailInfo(string workflowNo, ComDB db)
            {
                // SQL文生成
                string sql = string.Empty;

                sql += "select * ";
                sql += "  from workflow_detail ";
                sql += " where wf_no = @WfNo";

                return db.GetListByDataClass<EntityDao.WorkflowDetailEntity>(sql, new { WfNo = workflowNo });
            }

            /// <summary>
            /// ワークフロー詳細データを取得 ※直近の承認者
            /// </summary>
            /// <param name="workflowNo">ワークフローNo</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>ワークフロー詳細データ</returns>
            private IList<EntityDao.WorkflowDetailEntity> getNextUserWorkflowDetailInfo(string workflowNo, ComDB db)
            {
                // SQL文生成
                string sql = string.Empty;

                sql = "select * ";
                sql += "        from workflow_detail ";
                sql += "       where wf_no = @WfNo ";
                sql += "     and seq = ( select seq ";
                sql += "                  from workflow_detail ";
                sql += "                  where wf_no = @WfNo ";
                sql += "                  group by seq ";
                sql += "                  having min(status) is null ";
                sql += "                  order by seq asc limit 1 )";

                return db.GetListByDataClass<EntityDao.WorkflowDetailEntity>(sql, new { WfNo = workflowNo });
            }

            /// <summary>
            /// 未承認ユーザが存在するかどうかチェックを実施
            /// </summary>
            /// <param name="workflowNo">ワークフローNo</param>
            /// <param name="approvalUserId">処理ユーザID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>true:未承認者あり、false:未承認者なし</returns>
            private bool checkApprovalUserBySequential(string workflowNo, string approvalUserId, ComDB db)
            {
                // SQL文生成
                string sql = string.Empty;

                // この処理で更新したワークフロー詳細を取得
                sql += "with updated as ( ";
                sql += "    select * ";
                sql += "      from workflow_detail ";
                sql += "     where wf_no = @WfNo ";
                sql += "       and approval_user_id = @ApprovalUserId ";
                sql += "    order by update_date desc ";
                sql += "    limit 1 ";
                sql += ") ";

                // 承認者のいないseqを取得する
                sql += "select min(wfd.status) ";
                sql += "  from workflow_detail wfd ";
                sql += "       inner join updated on ";
                sql += "           wfd.wf_no = updated.wf_no ";
                sql += "       and wfd.seq <= updated.seq ";
                sql += "  group by wfd.seq ";
                sql += "  having min(wfd.status) is null "; // 未承認者のみのseq

                IList<EntityDao.WorkflowDetailEntity> result = db.GetListByDataClass<EntityDao.WorkflowDetailEntity>(sql, new { WfNo = workflowNo, ApprovalUserId = approvalUserId });
                if (result.Count > 0)
                {
                    // 1レコードでもデータが存在する場合
                    return true;
                }

                return false;
            }

            /// <summary>
            /// 通知区分を設定
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <returns>通知区分</returns>
            private int setNoticeDivision(int requestParam)
            {
                // 内部パラメータによって処理を分岐
                switch (requestParam)
                {
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST:
                        // 承認依頼の場合、「1:通知」を設定
                        return Constants.NOTICE_INFO.NOTICE_DIVISION.NOTICE;
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL:
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL:
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL:
                    case Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL:
                        // 上記以外の場合、「2:お知らせ」を設定
                        return Constants.NOTICE_INFO.NOTICE_DIVISION.NEWS;
                    default:
                        // 到達不能
                        return 0;
                }
            }

            /// <summary>
            /// 通知分類を設定
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <returns>通知区分</returns>
            private string setNoticeClass(int requestParam)
            {
                // 内部パラメータによって処理を分岐
                switch (requestParam)
                {
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST:
                        // 承認依頼の場合、"WF01"を設定
                        return Constants.NOTICE_INFO.NOTICE_CLASS.APPROVAL_REQUEST;
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL:
                        // 承認依頼取消の場合、"WF99"を設定
                        return Constants.NOTICE_INFO.NOTICE_CLASS.APPROVAL_REQUEST_CANCEL;
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL:
                        if (getIsApprovalComplete())
                        {
                            // 承認完了の場合、"WF20"を設定
                            return Constants.NOTICE_INFO.NOTICE_CLASS.APPROVAL_COMPLETE;
                        }
                        else
                        {
                            // 承認の場合、"WF10"を設定
                            return Constants.NOTICE_INFO.NOTICE_CLASS.APPROVAL;
                        }
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL:
                        // 承認取消の場合、"WF19"を設定
                        return Constants.NOTICE_INFO.NOTICE_CLASS.APPROVAL_CANCEL;
                    case Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL:
                        // 否認の場合、"WF90"を設定
                        return Constants.NOTICE_INFO.NOTICE_CLASS.DISAPPROVAL;
                    default:
                        // 到達不能
                        return null;
                }
            }

            /// <summary>
            /// 通知情報を追加
            /// </summary>
            /// <param name="insertBean">生成データ</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>true:正常、false:異常</returns>
            /// <remarks>最大連番を取得するため、メソッド自体を排他ロックをかけている</remarks>
            private bool insertNoticeInfo(EntityDao.NoticeInfoEntity insertBean, string tantoCd, ComDB db)
            {
                // 通知番号をシーケンスより取得
                insertBean.NoticeNo = GetNumber(db, WorkFlow.Key_NoticeNo_Seq); // 通知番号
                insertBean.InputDate = DateTime.Now;       // 登録日時
                insertBean.InputUserId = tantoCd;          // 登録者ID
                insertBean.UpdateDate = DateTime.Now;      // 更新日時
                insertBean.UpdateUserId = tantoCd;         // 更新者ID
                insertBean.DelFlg = Constants.DEL_FLG.OFF; // 削除フラグ

                // 登録処理
                if (db.RegistByOutsideSql(WorkFlow.NoticeInfo_Insert, WorkFlow.SubDir, insertBean) < 0)
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 通知情報 削除処理 ※キー番号
            /// </summary>
            /// <param name="keyNo">キー番号</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>true:正常、false:異常</returns>
            private bool deleteNoticeInfoByKeyNo(string keyNo, string tantoCd, ComDB db)
            {
                // 削除データを生成
                EntityDao.NoticeInfoEntity deleteBean = new EntityDao.NoticeInfoEntity();
                deleteBean.UserId = tantoCd; // ユーザーID
                deleteBean.KeyNo = keyNo;    // キー番号

                // 削除処理
                if (db.RegistByOutsideSql(WorkFlow.NoticeInfo_DeleteByKeyNo, WorkFlow.SubDir, deleteBean) < 0)
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 通知情報 削除処理 ※キー番号 + 順序
            /// </summary>
            /// <param name="keyNo">キー番号</param>
            /// <param name="tantoCd">処理を行ったユーザID</param>
            /// <param name="seq">処理を行った順序</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="bean">ワークフローヘッダ</param>
            /// <param name="noticeContents">通知内容</param>
            /// <param name="requestParam">通知分類</param>
            /// <param name="noticeFlg">通知区分</param>
            /// <returns>true:正常、false:異常</returns>
            private bool deleteNoticeInfoBySeq(string keyNo, string tantoCd, int? seq, ComDB db, EntityDao.WorkflowHeaderEntity bean, string noticeContents,
                int requestParam, bool noticeFlg = true)
            {
                // SQL文生成
                string sql = string.Empty;

                sql += "select approval_user_id ";
                sql += "  from workflow_detail ";
                sql += " where wf_no = @WfNo ";
                sql += "   and seq = @Seq ";
                sql += "   and approval_user_id <> @ApprovalUserId ";
                sql += "   and status is null "; // 未承認者のみ
                // 削除対象のユーザーを検索
                IList<EntityDao.WorkflowDetailEntity> list = db.GetListByDataClass<EntityDao.WorkflowDetailEntity>(sql, new { WfNo = keyNo, Seq = seq, ApprovalUserId = tantoCd });
                if (list == null || list.Count <= 0)
                {
                    return true;
                }

                // 通知情報を生成
                EntityDao.NoticeInfoEntity insertBean = new EntityDao.NoticeInfoEntity();

                int noticeDivision = noticeFlg ? Constants.NOTICE_INFO.NOTICE_DIVISION.NOTICE : Constants.NOTICE_INFO.NOTICE_DIVISION.NEWS;
                insertBean.NoticeDivision = noticeDivision;            // 通知区分
                insertBean.NoticeClass = setNoticeClass(requestParam); // 通知分類
                insertBean.NoticeContents = noticeContents;            // 通知内容
                insertBean.KeyNo = keyNo;                              // キー番号 ※通知元情報紐づけるためのキー

                foreach (var user in list)
                {
                    // 削除データを生成
                    EntityDao.NoticeInfoEntity deleteBean = new EntityDao.NoticeInfoEntity();
                    deleteBean.UserId = user.ApprovalUserId; // ユーザーID
                    deleteBean.KeyNo = keyNo;    // キー番号

                    // 削除処理
                    if (db.RegistByOutsideSql(WorkFlow.NoticeInfo_DeleteByKeyNo, WorkFlow.SubDir, deleteBean) < 0)
                    {
                        return false;
                    }

                    insertBean.UserId = user.ApprovalUserId;                // ユーザーID

                    // 通知情報登録
                    if (!insertNoticeInfo(insertBean, tantoCd, db))
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// プッシュ通知情報取得
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="tantoCd">ワークフロー更新を行ったユーザ</param>
            /// <param name="db">DB操作クラス</param>
            /// <param name="seq">ワークフロー更新を行った順序(承認で使用)</param>
            /// <returns>プッシュ通知情報</returns>
            private ComBusBase.PushInfo getPush(int requestParam, string tantoCd, ComDB db, int? seq)
            {
                // 通知ユーザリストを取得
                List<WorkflowGetNoticeUserListResult> noticeUserList = getTargetNoticeUserList(requestParam, tantoCd, seq);
                // ユーザIDを取得
                List<string> useridList = noticeUserList.Select(x => x.UserId).Distinct().ToList();
                // 承認完了以外のプッシュ通知が設定されている場合、自身にもプッシュ通知を設定
                if (this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST) ||
                    this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL) ||
                    this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL) ||
                    this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL) ||
                    this.isNoticePush(Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL))
                {
                    // 既に含まれている場合、設定を行わない
                    if (!useridList.Contains(tantoCd))
                    {
                        useridList.Add(tantoCd);
                    }
                }
                // ユーザIDを重複排除して取得
                return new ComBusBase.PushInfo(useridList, db);
            }

            /// <summary>
            /// メール送信処理
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="tantoCd">ワークフロー更新を行ったユーザ</param>
            /// <param name="mailParamList">メールのパラメータ</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="errorMsg">エラーメッセージ</param>
            /// <param name="mailParamListApprovalRequest">承認を行った際、一緒に送信する承認依頼メールのパラメータ、省略した場合Null</param>
            /// <param name="mailParamListAppComp">承認を行った際、承認完了メールのパラメータ、省略した場合Null</param>
            /// <param name="seq">承認を行った順序</param>
            /// <returns>成功した場合、True</returns>
            private bool sendMail(int requestParam, string tantoCd, List<string> mailParamList, string languageId, ref string errorMsg,
                List<string> mailParamListApprovalRequest = null, List<string> mailParamListAppComp = null, int? seq = null)
            {
                // 通知ユーザリストを取得
                // 承認の場合は同順序未承認ユーザも取得
                List<WorkflowGetNoticeUserListResult> noticeUserList = getTargetNoticeUserList(requestParam, tantoCd, seq);

                // 送信するメールの内容を取得
                EntityDao.MailTemplateEntity mailText = getMailTextInfo(requestParam, languageId, mailParamList);

                if (mailText == null)
                {
                    // 取得できない場合、終了
                    return false;
                }

                // メール送信者情報取得　検索
                EntityDao.UtilityEntity mailFrom = db.GetEntityByOutsideSqlByDataClass<EntityDao.UtilityEntity>(WorkFlow.WorkFlow_GetMailFrom, WorkFlow.SubDir);
                if (mailFrom == null)
                {
                    return false;
                }

                // 通知区分：順序で、承認の際に承認依頼のメールも送信するかどうかのフラグ
                bool isApprovedNextSeq = this.isApprovedNextSeq(requestParam, tantoCd);
                if (isApprovedNextSeq)
                {
                    // 承認メール(依頼者) + 承認メール(同順序未承認者) + 承認依頼メール(次順序承認者)
                    // 承認依頼のメールも送信する場合は、通知ユーザのうち、まずは依頼者のみにメールを送信する
                    callSendMail(mailFrom, getRequestNoticeUserList(noticeUserList), mailText, languageId, ref errorMsg);

                    if (seq == null)
                    {
                        // 取得できていない場合終了
                        return false;
                    }

                    // 同順序の未承認ユーザに依頼者と同じメールを送信
                    int sequence = int.Parse(seq.ToString());
                    callSendMail(mailFrom, getApprovalNoticeUserSeqList(noticeUserList, sequence), mailText, languageId, ref errorMsg);

                    // 承認依頼のメール送信
                    // 送信するメールの内容を取得
                    EntityDao.MailTemplateEntity mailTextAppReq = getMailTextInfo(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST, languageId, mailParamListApprovalRequest);
                    if (mailTextAppReq == null)
                    {
                        // 取得できない場合、終了
                        return false;
                    }
                    // 次の順序の承認者に承認依頼を送信
                    callSendMail(mailFrom, getApprovalNoticeUserUnSeqList(noticeUserList, sequence), mailTextAppReq, languageId, ref errorMsg);
                }
                else
                {
                    // 上記以外の場合
                    // (承認メール or 承認完了メール)のみ

                    // 承認を行って、承認完了の場合、全通知ユーザにメールを送信
                    if (requestParam == Constants.WORKFLOW.REQUESTPARAM.APPROVAL && getIsApprovalComplete())
                    {
                        // 送信するメールの内容を取得
                        EntityDao.MailTemplateEntity mailTextAppComp = getMailTextInfo(Constants.WORKFLOW.REQUESTPARAM.APPROVAL_COMPLETE, languageId, mailParamList);
                        if (mailTextAppComp == null)
                        {
                            // 取得できない場合、終了
                            return false;
                        }
                        // 全通知ユーザに送信
                        callSendMail(mailFrom, noticeUserList, mailTextAppComp, languageId, ref errorMsg);
                    }
                    else
                    {
                        // 通常の処理
                        callSendMail(mailFrom, noticeUserList, mailText, languageId, ref errorMsg);
                    }
                }

                return true;
            }

            /// <summary>
            /// メール送信メソッドを呼び出すために必要な情報を取得して送信する処理
            /// </summary>
            /// <param name="mailFrom">メール送信元の情報</param>
            /// <param name="noticeUserList">通知対象ユーザリスト(メール送信先の情報)</param>
            /// <param name="mailText">メール本文の情報</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="errorMsg">エラーメッセージ</param>
            /// <returns>成功した場合、True</returns>
            private bool callSendMail(EntityDao.UtilityEntity mailFrom, List<WorkflowGetNoticeUserListResult> noticeUserList, EntityDao.MailTemplateEntity mailText,
                string languageId, ref string errorMsg)
            {
                // 通知対象ユーザのリストをユーザIDのリストに変換
                List<string> userIdList = noticeUserList.Select(x => x.UserId).Distinct().ToList();
                if (userIdList == null || userIdList.Count == 0)
                {
                    return false;
                }
                // ユーザIDのリストで、ログインテーブルのリストを取得
                IList<string> mailAddressList = getMailAddressList(userIdList);
                if (mailAddressList == null || mailAddressList.Count == 0)
                {
                    // 取得できなかった場合エラー
                    return false;
                }
                // メールアドレスのリストをカンマ区切りの文字列に変換する
                string toAddressCsv = string.Join(",", mailAddressList.Distinct().ToList());
                // 改行を置換
                mailText.TextBody = mailText.TextBody.Replace("\\n", "\n");

                // メール送信サーバ情報取得
                MailUtil.MailSendServerInfo server = MailUtil.GetMailSendServerInfo(db);
                string mailSendServer = server.Url; // サーバURL
                int mailSendPort = server.Port;  // ポート
                string mailAuthUser = server.AuthUser;   // 承認ユーザ
                string mailAuthPasswd = server.AuthPassword;   // 承認パスワード

                // 送信
                // Name02:アドレス、Name01:名前
                if (!MailUtil.SendMailImpl(mailFrom.Name02, mailFrom.Name01, toAddressCsv, null, null, mailText.TextTitle, mailText.TextBody, null, mailSendServer, mailSendPort, mailAuthUser, mailAuthPasswd))
                {
                    errorMsg = ComUtil.GetPropertiesMessage(APResources.ID.MS00114, languageId, null, db);
                }
                return true;
            }

            /// <summary>
            /// ユーザの一覧より、メールアドレスを取得
            /// </summary>
            /// <param name="userIdList">ユーザIDの一覧</param>
            /// <returns>メールアドレスの一覧</returns>
            /// <remarks>テストのメールアドレスを取得出来た場合は、そちらを使用する。</remarks>
            private IList<string> getMailAddressList(List<string> userIdList)
            {
                // 戻り値のメールアドレスリスト
                IList<string> mailAddressList = new List<string>();
                // 汎用マスタに区分=WFMT、コード=呼出元区分、番号=0(固定)で、テスト宛先が定義されている
                var testMail = new EntityDao.UtilityEntity().GetEntity("WFMT", this.workflowDivision, "0", this.db);
                if (testMail != null && testMail.DelFlg == Constants.DEL_FLG.OFF)
                {
                    // 取得でき、削除フラグが0の場合、名称1のアドレスを使用する
                    // 送信するユーザの件数分、アドレスを追加
                    userIdList.ForEach(x => mailAddressList.Add(testMail.Name01));
                }
                else
                {
                    // 取得できなかった場合、または削除フラグが1の場合、通常通りユーザマスタよりアドレスを取得する
                    mailAddressList = db.GetListByOutsideSql<string>(WorkFlow.WorkFlow_GetMailAddressList, WorkFlow.SubDir, new { UserIdList = userIdList });
                }
                return mailAddressList;
            }

            /// <summary>
            /// 送信するメールのタイトルと文章を取得 ※本文未指定可
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="languageId">言語ID</param>
            /// <returns>メールテンプレートマスタのメール本文を引数リストで置き換えて返却</returns>
            private EntityDao.MailTemplateEntity getMailTextInfo(int requestParam, string languageId)
            {
                // メール情報取得　検索条件
                EntityDao.UtilityEntity condition = new EntityDao.UtilityEntity();
                condition.UtilityCd = this.workflowDivision;   // 名称区分はパターン区分
                condition.UtilityNo = requestParam.ToString(); // 名称コードは内部パラメータ
                condition.LanguageId = languageId; // 言語ID(メールテーブル)

                // 検索
                EntityDao.MailTemplateEntity mailText = db.GetEntityByOutsideSqlByDataClass<EntityDao.MailTemplateEntity>(WorkFlow.Workflow_GetMailText, WorkFlow.SubDir, condition);
                if (mailText == null)
                {
                    // 取得できない場合、終了
                    return null;
                }

                return mailText;
            }

            /// <summary>
            /// 送信するメールのタイトルと文章を取得
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="mailParamList">メール本文の引数リスト</param>
            /// <returns>メールテンプレートマスタのメール本文を引数リストで置き換えて返却</returns>
            private EntityDao.MailTemplateEntity getMailTextInfo(int requestParam, string languageId, List<string> mailParamList)
            {
                // メール情報取得　検索条件
                EntityDao.UtilityEntity condition = new EntityDao.UtilityEntity();
                condition.UtilityCd = this.workflowDivision;   // 名称区分はパターン区分
                condition.UtilityNo = requestParam.ToString(); // 名称コードは内部パラメータ
                condition.LanguageId = languageId; // 言語ID(メールテーブル)

                // 検索
                EntityDao.MailTemplateEntity mailText = db.GetEntityByOutsideSqlByDataClass<EntityDao.MailTemplateEntity>(WorkFlow.Workflow_GetMailText, WorkFlow.SubDir, condition);
                if (mailText == null)
                {
                    // 取得できない場合、終了
                    return null;
                }
                // 本文を引数リストで置換
                mailText.TextBody = string.Format(mailText.TextBody, mailParamList.ToArray());
                return mailText;
            }

            /// <summary>
            /// 通知ユーザリストを取得
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="tantoCd">ワークフロー更新を行ったユーザ</param>
            /// <param name="seq">ワークフロー更新を行った順序(承認の場合のみ必要)</param>
            /// <returns>ユーザリスト</returns>
            private List<WorkflowGetNoticeUserListResult> getTargetNoticeUserList(int requestParam, string tantoCd, int? seq = null)
            {
                List<WorkflowGetNoticeUserListResult> noticeUserList;
                // ワークフローが順序か並行かを判定
                if (isSequential())
                {
                    // 順序の場合
                    noticeUserList = getSearialNoticeUserList(requestParam, tantoCd, seq);
                }
                else
                {
                    // 並行の場合
                    noticeUserList = getParallelNoticeUserList(requestParam, tantoCd, seq);
                }
                return noticeUserList;
            }

            /// <summary>
            /// 対象通知ユーザ取得
            /// </summary>
            /// <param name="noticeUserList">通知対象ユーザリスト</param>
            /// <param name="seq">順序</param>
            /// <returns>対象通知ユーザリスト</returns>
            private List<WorkflowGetNoticeUserListResult> getTargetNoticeUserSeq(List<WorkflowGetNoticeUserListResult> noticeUserList, int? seq)
            {
                return noticeUserList.Where(x => x.Seq != seq || x.IsRequest == true).ToList();
            }

            /// <summary>
            /// ワークフローが順序か並行かを判定
            /// </summary>
            /// <returns>通知区分</returns>
            private bool isSequential()
            {
                return workflowHeader.NoticeDivision == (int)Constants.WORKFLOW.NOTICEDIVISION.SEQUENTIAL;
            }

            /// <summary>
            /// Null許容整数のフラグチェック(1がTrue)
            /// </summary>
            /// <param name="flag">Null許容整数型のフラグ</param>
            /// <returns>True(1)、False(それ以外、Null)</returns>
            private bool checkIntFlag(int? flag)
            {
                if (flag == null)
                {
                    return false;
                }
                int newFlag = flag ?? 0;
                return newFlag == 1;
            }

            /// <summary>
            /// ワークフローが承認完了か判定
            /// </summary>
            /// <returns>ステータス</returns>
            private bool getIsApprovalComplete()
            {
                return this.workflowHeader.Status == Constants.WORKFLOW.HEADERSTATUS.APPROVAL;
            }

            ///// <summary>
            ///// 承認時、承認順序に他ユーザが存在するか判定する
            ///// </summary>
            ///// <returns>他ユーザがいる場合true</returns>
            //private bool getIsApprovalSeqUser()
            //{
            //    return false;
            //}

            /// <summary>
            /// 全ての通知対象ユーザを取得する
            /// </summary>
            /// <param name="workflowNo">ワークフロー番号（パターン番号）</param>
            /// <returns>取得したユーザーのリスト</returns>
            private IList<WorkflowGetNoticeUserListResult> getAllNoticeUserList(string workflowNo)
            {
                return db.GetListByOutsideSqlByDataClass<WorkflowGetNoticeUserListResult>(WorkFlow.WorkFlow_GetNoticeUserList, WorkFlow.SubDir, new EntityDao.WorkflowHeaderEntity.PrimaryKey(workflowNo));
            }

            /// <summary>
            /// 通知対象ユーザより未承認ユーザのみを取得
            /// </summary>
            /// <param name="workflowNo">ワークフローNo</param>
            /// <returns>未承認の場合True、承認者でなければFalse</returns>
            private IList<WorkflowGetNoticeUserListResult> getUnApprived(string workflowNo)
            {
                IList<WorkflowGetNoticeUserListResult> userList = db.GetListByOutsideSqlByDataClass<WorkflowGetNoticeUserListResult>(WorkFlow.WorkFlow_GetUnApprovedUserList, WorkFlow.SubDir, new EntityDao.WorkflowHeaderEntity.PrimaryKey(workflowNo));
                return userList.Where(x => x.IsUnApproved()).ToList();
            }

            /// <summary>
            /// 通知対象ユーザより、依頼者のみを取得
            /// </summary>
            /// <param name="userList">通知対象ユーザのリスト</param>
            /// <returns>依頼者に絞り込んだ通知対象ユーザのリスト</returns>
            private List<WorkflowGetNoticeUserListResult> getRequestNoticeUserList(IList<WorkflowGetNoticeUserListResult> userList)
            {
                return userList.Where(x => x.IsRequest == true).ToList();
            }

            /// <summary>
            /// 通知対象ユーザより、承認者のみを取得
            /// </summary>
            /// <param name="userList">通知対象ユーザのリスト</param>
            /// <returns>承認者に絞り込んだ通知対象ユーザのリスト</returns>
            private List<WorkflowGetNoticeUserListResult> getApprovalNoticeUserList(IList<WorkflowGetNoticeUserListResult> userList)
            {
                return userList.Where(x => x.IsRequest == false).ToList();
            }

            /// <summary>
            /// 通知対象ユーザより、同順序でない承認者のみを取得
            /// </summary>
            /// <param name="userList">通知対象ユーザのリスト</param>
            /// <param name="seq">処理を行った順序</param>
            /// <returns>承認者に絞り込んだ通知対象ユーザのリスト</returns>
            private List<WorkflowGetNoticeUserListResult> getApprovalNoticeUserUnSeqList(IList<WorkflowGetNoticeUserListResult> userList, int seq)
            {
                return userList.Where(x => x.IsRequest == false).Where(x => x.Seq != seq).ToList();
            }

            /// <summary>
            /// 通知対象ユーザより、同順序の承認者のみを取得
            /// </summary>
            /// <param name="userList">通知対象ユーザのリスト</param>
            /// <param name="seq">処理を行った順序</param>
            /// <returns>承認者に絞り込んだ通知対象ユーザのリスト</returns>
            private List<WorkflowGetNoticeUserListResult> getApprovalNoticeUserSeqList(IList<WorkflowGetNoticeUserListResult> userList, int seq)
            {
                userList = userList.Where(x => x.IsRequest == false).ToList();
                return userList.Where(x => x.Seq == seq).ToList();
            }

            /// <summary>
            /// 通知対象ユーザより、操作ユーザを除外したリストを取得
            /// </summary>
            /// <param name="userList">通知対象ユーザのリスト</param>
            /// <param name="tantoCd">除外する操作ユーザ</param>
            /// <returns>操作ユーザを除外した通知対象ユーザのリスト</returns>
            private List<WorkflowGetNoticeUserListResult> getExceptTantoNoticeUserList(IList<WorkflowGetNoticeUserListResult> userList, string tantoCd)
            {
                return userList.Where(x => x.UserId != tantoCd).ToList();
            }

            /// <summary>
            /// 順序の場合の通知対象ユーザを内部パラメータに応じて絞り込み
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="tantoCd">処理を行ったユーザ(承認の場合使用)</param>
            /// <param name="seq">処理を行った順序(承認の場合使用)</param>
            /// <returns>絞り込んだ通知対象ユーザ</returns>
            private List<WorkflowGetNoticeUserListResult> getSearialNoticeUserList(int requestParam, string tantoCd, int? seq)
            {
                // 全対象ユーザ取得
                IList<WorkflowGetNoticeUserListResult> userList = getAllNoticeUserList(this.workflowHeader.WfNo);

                // 内部パラメータごとに分岐
                switch (requestParam)
                {
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST:
                        // 承認依頼
                        // 承認依頼は承認の場合と処理が重複するので、分岐の外で実施
                        break;
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL:
                        // 承認依頼取消
                        // 承認済、否認済の承認者全員
                        return userList.Where(x => x.IsApproval() || x.IsDenial()).ToList();
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL:
                        if (getIsApprovalComplete())
                        {
                            // 承認完了
                            // 全員
                            return getExceptTantoNoticeUserList(userList, tantoCd).ToList();
                        }
                        // 承認の場合は、複雑なので分岐の外で実施
                        break;
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL:
                        // 承認取消
                        // 全員
                        return getExceptTantoNoticeUserList(userList, tantoCd).ToList();
                    case Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL:
                        // 否認
                        // 全員
                        return getExceptTantoNoticeUserList(userList, tantoCd).ToList();
                    default:
                        // 到達不能
                        return null;
                }

                // 承認依頼のリストを取得する(直近の未承認者)
                // 未承認者のリスト
                // ↓他の承認者の情報がいるのでこの形式で判定できない

                //IList<WorkflowGetNoticeUserListResult> isUnApprovedUserList = userList.Where(x => x.IsUnApproved()).ToList();
                // 一人も承認者がいない階層の未承認者を取得
                IList<WorkflowGetNoticeUserListResult> isUnApprovedUserList = getUnApprived(this.workflowHeader.WfNo);
                // 未承認者の最小の順序が、直近の未承認者の順序になる
                int minSeq = isUnApprovedUserList.Min(x => x.Seq);

                // 承認依頼のリスト(未承認かつ順序が最小)
                List<WorkflowGetNoticeUserListResult> approvalRequestList = isUnApprovedUserList.Where(x => x.Seq == minSeq).ToList();
                if (requestParam == Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST)
                {
                    // 承認依頼の場合、承認依頼を送る承認者のリストを返して終了
                    return approvalRequestList; // 承認依頼メールを送る対象のリスト
                }

                // 承認の場合
                // 依頼者と、「直前の承認者が /*（すべて）*/ 承認した場合」に直近の未承認者(承認依頼と同じ)
                // 承認の場合の戻り値に、依頼者のリストを設定
                var retApprovedList = getRequestNoticeUserList(userList);

                // 承認した階層の承認状態は必ず「承認」
                // 承認依頼のリスト + 依頼者のリスト
                retApprovedList.AddRange(approvalRequestList);

                // 処理を行った順序でまだ承認していないユーザを取得
                List<WorkflowGetNoticeUserListResult> seqApprovalList = userList.Where(x => x.IsUnApproved()).Where(x => x.Seq == seq).ToList();
                // 承認依頼のリスト + 依頼者のリスト + 同階層の未承認者のリスト
                retApprovedList.AddRange(seqApprovalList);

                return retApprovedList;
            }

            /// <summary>
            /// 通知区分が順序で、承認の場合、直近の未承認者へ通知するかどうかの判定
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="tantoCd">ワークフロー更新を行ったユーザ</param>
            /// <returns>通知区分：順序、内部パラメータ：承認、ヘッダのステータス：承認でない、詳細の更新した階層のステータス：全て承認　ならTrue</returns>
            private bool isApprovedNextSeq(int requestParam, string tantoCd)
            {
                // 順序か並行かを判定
                if (isSequential() == false)
                {
                    // 並行の場合は、False
                    return false;
                }
                // 順序の場合
                // 内部パラメータ承認で、ヘッダが承認完了でない場合
                if (requestParam == Constants.WORKFLOW.REQUESTPARAM.APPROVAL && getIsApprovalComplete() == false)
                {
                    // 直前の承認された階層の情報を取得
                    EntityDao.WorkflowDetailEntity condition = new EntityDao.WorkflowDetailEntity();
                    condition.WfNo = this.workflowHeader.WfNo;
                    condition.ApprovalUserId = tantoCd;
                    IList<EntityDao.WorkflowDetailEntity> approvedSeqInfoList = db.GetListByOutsideSqlByDataClass<EntityDao.WorkflowDetailEntity>(WorkFlow.WorkFlow_GetUpdatedSeqInfo, WorkFlow.SubDir, condition);
                    // この階層の承認者が１人でも承認した場合は、次の階層へ行く
                    // ステータスが承認の件数が1件以上の場合、承認されている
                    return approvedSeqInfoList.Where(x => x.Status == Constants.WORKFLOW.DETAILSTATUS.APPROVAL).Count() > 0;
                }
                else
                {
                    // それ以外の場合は、False
                    return false;
                }
            }

            /// <summary>
            /// 並行の場合の通知対象ユーザを内部パラメータに応じて絞り込み
            /// </summary>
            /// <param name="requestParam">内部パラメータ</param>
            /// <param name="tantoCd">ワークフロー更新を行ったユーザ</param>
            /// <param name="seq">ワークフロー更新を行った順序(承認で使用)</param>
            /// <returns>絞り込んだ通知対象ユーザ</returns>
            private List<WorkflowGetNoticeUserListResult> getParallelNoticeUserList(int requestParam, string tantoCd, int? seq)
            {
                // 全対象ユーザ取得
                IList<WorkflowGetNoticeUserListResult> userList = getAllNoticeUserList(this.workflowHeader.WfNo);

                // 内部パラメータごとに分岐
                switch (requestParam)
                {
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST:
                        // 承認依頼
                        // 承認担当者全員
                        return getApprovalNoticeUserList(userList);
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_REQUEST_CANCEL:
                        // 承認依頼取消
                        // 承認担当者全員
                        return getApprovalNoticeUserList(userList);
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL:
                        // 承認
                        if (getIsApprovalComplete())
                        {
                            // 承認完了の場合
                            // 全員
                            return getExceptTantoNoticeUserList(userList, tantoCd).ToList();
                        }
                        else
                        {
                            // 承認の場合
                            // 依頼者
                            var retApprovedList = getRequestNoticeUserList(userList);
                            // 処理を行った順序でまだ承認していないユーザを取得
                            List<WorkflowGetNoticeUserListResult> seqApprovalList = userList.Where(x => x.IsUnApproved()).Where(x => x.Seq == seq).ToList();
                            // 依頼者のリスト + 同階層の未承認者のリスト
                            retApprovedList.AddRange(seqApprovalList);
                            return retApprovedList;
                        }
                    case Constants.WORKFLOW.REQUESTPARAM.APPROVAL_CANCEL:
                        // 承認取消
                        // 全員
                        return getExceptTantoNoticeUserList(userList, tantoCd).ToList();
                    case Constants.WORKFLOW.REQUESTPARAM.DISAPPROVAL:
                        // 否認
                        // 全員
                        return getExceptTantoNoticeUserList(userList, tantoCd).ToList();
                    default:
                        // 到達不能
                        return null;
                }
            }

            /// <summary>
            /// ワークフロー 通知対象ユーザ取得の検索結果クラス
            /// </summary>
            private class WorkflowGetNoticeUserListResult
            {
                /// <summary>Gets or sets a value indicating whether 依頼区分</summary>
                /// <value>依頼区分</value>
                public bool IsRequest { get; set; }
                /// <summary>Gets or sets 順序</summary>
                /// <value>順序</value>
                public int Seq { get; set; }
                /// <summary>Gets or sets ユーザID</summary>
                /// <value>ユーザID</value>
                public string UserId { get; set; }
                /// <summary>Gets or sets ステータス</summary>
                /// <value>ステータス(詳細のみ)</value>
                public int? Status { get; set; }

                /// <summary>
                /// 通知対象ユーザが承認しているか判定
                /// </summary>
                /// <returns>承認の場合True、承認者でなければFalse</returns>
                public bool IsApproval()
                {
                    // 承認の場合True
                    return checkStatus(Constants.WORKFLOW.DETAILSTATUS.APPROVAL);
                }
                /// <summary>
                /// 通知対象ユーザが否認しているか判定
                /// </summary>
                /// <returns>否認の場合True、承認者でなければFalse</returns>
                public bool IsDenial()
                {
                    // 否認の場合True
                    return checkStatus(Constants.WORKFLOW.DETAILSTATUS.DENIAL);
                }
                /// <summary>
                /// 通知対象ユーザが未承認か判定
                /// </summary>
                /// <returns>未承認の場合True、承認者でなければFalse</returns>
                public bool IsUnApproved()
                {
                    // 未承認の場合True
                    return checkStatus(Constants.WORKFLOW.DETAILSTATUS.UNAPPROVED);
                }

                /// <summary>
                /// 与えられた承認ステータスかどうかを判定
                /// </summary>
                /// <param name="status">チェックする承認ステータス</param>
                /// <returns>承認者でない場合は、False</returns>
                private bool checkStatus(int? status)
                {
                    if (this.IsRequest == true)
                    {
                        // ユーザが依頼者の場合、False
                        return false;
                    }
                    // 与えられたステータスと同じ場合の場合True
                    return this.Status == status;
                }
            }
        }

        /// <summary>
        /// ワークフロー　承認依頼ユーザ取得
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB接続、this.db</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <param name="getName">true:ユーザ名を返却、false:ユーザIDを返却</param>
        /// <returns>承認依頼ユーザ名 ※取得できない場合、nullを返す</returns>
        public static string GetApprovalRequestUser(string slipNo, ComBusBase form, ComDB db,
            string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "", bool getName = true)
        {
            // 呼出元区分を取得
            // 承認依頼取消は、True。それ以外は、False
            EntityDao.NamesEntity namesBean = getWfDivision(form.ConductId, true, db, addSearchKey);
            if (namesBean == null)
            {
                // 存在しない場合、エラーに
                return null;
            }

            // レコードを取得
            EntityDao.WorkflowHeaderEntity headerBean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);
            if (headerBean == null)
            {
                // 存在しない場合、エラーに
                return null;
            }

            // 依頼ユーザIDを返却する場合
            if (!getName)
            {
                return headerBean.RequestUserId;
            }

            // 依頼ユーザ名称
            EntityDao.LoginEntity loginInfo = new EntityDao.LoginEntity();
            loginInfo = loginInfo.GetEntity(headerBean.RequestUserId, db);

            return loginInfo.UserName;
        }

        /// <summary>
        /// 通知情報削除
        /// </summary>
        /// <param name="slipNo">処理対象の伝票番号</param>
        /// <param name="form">機能自身、this</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="slipBranchNo1">伝票番号枝番1、省略時はブランク</param>
        /// <param name="slipBranchNo2">伝票番号枝番2、省略時はブランク</param>
        /// <param name="addSearchKey">省略可能　呼出元区分で名称マスタの承認が一意にならない場合の識別用キー</param>
        /// <param name="otherConductId">別機能IDを使用する場合指定 ※省略時は自分機能IDを使用</param>
        public static void DeleteNoticeInfo(string slipNo, ComBusBase form, ComDB db, string slipBranchNo1 = "", string slipBranchNo2 = "", string addSearchKey = "", string otherConductId = "")
        {
            // キー情報(※ワークフローNo)を取得
            string conductId = form.ConductId;
            if (!string.IsNullOrEmpty(otherConductId))
            {
                // 別機能IDが指定されている場合、そちらを優先する
                conductId = otherConductId;
            }

            EntityDao.NamesEntity namesBean = getWfDivision(conductId, false, db, addSearchKey);
            if (namesBean == null)
            {
                // 存在しない場合、処理を中断
                return;
            }

            // レコードを取得
            EntityDao.WorkflowHeaderEntity headerBean = getWorkflowHeader(slipNo, slipBranchNo1, slipBranchNo2, namesBean.NameCd, db);
            if (headerBean == null)
            {
                // 存在しない場合、処理を中断
                return;
            }

            // キー情報が取得できた場合、削除処理を実施
            string sql = string.Empty;

            // ワークフローヘッダの削除フラグを立てる
            sql += " update workflow_header set del_flg = @DelFlg , ";
            sql += "                           update_date = @Now , ";
            sql += "                           updator_cd = @UserId ";
            sql += " where wf_no = @WfNo";
            db.Regist(sql, new { WfNo = headerBean.WfNo, DelFlg = Constants.DEL_FLG.ON, Now = DateTime.Now, UserId = form.UserId });

            sql = string.Empty; // 初期化
            // 通知処理をだれに行うか削除対象ユーザを取得
            sql += "select * from notice_info where notice_division = 1 and key_no = @KeyNo";
            IList<EntityDao.NoticeInfoEntity> noticeInfoList = db.GetList<EntityDao.NoticeInfoEntity>(sql, new { KeyNo = headerBean.WfNo });
            if (noticeInfoList == null || noticeInfoList.Count == 0)
            {
                // 対象データが存在しない場合、処理を中断する
                return;
            }

            sql = string.Empty; // 初期化
            sql += "delete from notice_info where notice_division = 1 and key_no = @KeyNo"; // 念のため、通知情報をすべてクリアしておく
            db.Regist(sql, new { KeyNo = headerBean.WfNo }); // 削除処理を実施

            // ユーザIDを取得
            List<string> useridList = noticeInfoList.Select(x => x.UserId).Distinct().ToList();
            // プッシュ通知を実施
            ComBusBase.PushInfo pushInfo = new ComBusBase.PushInfo(useridList, db);
            form.SetPushTarget(pushInfo);
        }

        /// <summary>
        /// 最新のワークフローヘッダから同じ承認依頼を作成し承認
        /// </summary>
        /// <param name="paramUtilityWorkflowDivision">utilityから取得する際の呼出元区分</param>
        /// <param name="paramWorkflowDivision">承認WFに使用する呼出元区分</param>
        /// <param name="paramSlipNo">承認対象伝票番号一覧</param>
        /// <param name="db">データベース情報</param>
        /// <param name="paramLanguageId">言語ID</param>
        /// <param name="paramUserId">ユーザーID</param>
        /// <param name="paramConductId">機能ID</param>
        /// <param name="paramSlipBranchNo1">伝票番号枝番1</param>
        /// <param name="paramSlipBranchNo2">伝票番号枝番2</param>
        /// <param name="paramComment">備考</param>
        /// <param name="paramMailparamList">メールパラメータリスト</param>
        /// <returns>正常：true　エラー：false</returns>
        public static bool repeatApprovalRequest(string paramUtilityWorkflowDivision, string paramWorkflowDivision, Dictionary<string, string> paramSlipNo, ComDB db, string paramLanguageId, string paramUserId, string paramConductId,
            string paramSlipBranchNo1, string paramSlipBranchNo2, string paramComment, List<string> paramMailparamList)
        {
            // 承認依頼情報
            var workflowInfo = new WorkflowApprovalRequestParam();

            // utilityテーブルより使用パターン取得
            EntityDao.UtilityEntity pattern = new EntityDao.UtilityEntity();
            pattern = pattern.GetEntity("PWFD", paramUtilityWorkflowDivision, "1", db);

            // テンプレート情報取得
            string sql = string.Empty;
            sql += " select * ";
            sql += " from workflow_template_header ";
            sql += " where wf_template_no = @TempNo ";
            sql += " and active_date = @ActiveDate ";
            IList<EntityDao.WorkflowTemplateHeaderEntity> head = db.GetListByDataClass<EntityDao.WorkflowTemplateHeaderEntity>(sql, new { TempNo = int.Parse(pattern.Name01), ActiveDate = DateTime.Parse(pattern.Name02) });

            workflowInfo.Header.WfTemplateNo = head[0].WfTemplateNo.ToString(); // テンプレートNo
            workflowInfo.Header.ActiveDate = head[0].ActiveDate;                // 有効開始日
            workflowInfo.Header.WfName = head[0].WfName;                        // ワークフロー名
            workflowInfo.Header.NoticeDivision = (int?)head[0].NoticeDivision;  // 通知区分

            // 承認者を取得
            sql = string.Empty;
            sql += " with head as ( ";
            sql += " select wf_no  ";
            sql += " from workflow_header ";
            sql += " where wf_division = @WfDivision ";
            sql += " order by input_date desc ";
            sql += " limit 1 ) ";

            sql += " select * ";
            sql += " from workflow_detail detail ";
            sql += " inner join head ";
            sql += " on head.wf_no = detail.wf_no ";
            sql += " order by seq ";
            IList<EntityDao.WorkflowDetailEntity> detailList = db.GetListByDataClass<EntityDao.WorkflowDetailEntity>(sql, new { WfDivision = paramUtilityWorkflowDivision });

            foreach (var user in detailList)
            {
                var detail = new EntityDao.WorkflowDetailEntity();
                detail.Seq = user.Seq;
                detail.ApprovalUserId = user.ApprovalUserId;
                detail.AllApprovalFlg = user.AllApprovalFlg;

                workflowInfo.DetailList.Add(detail);
            }

            workflowInfo.WorkflowDivision = head[0].WfDivision;

            // 全ての伝票番号に対して承認処理を行う
            foreach (var param in paramSlipNo)
            {

                ComBusBase.PushInfo info;       // プッシュ通知内容格納用
                string errorMsg = string.Empty; // メール送信エラー設定用

                // 承認依頼
                if (!CreateApprovalRequestForWorkFlow(workflowInfo, param.Value, paramSlipBranchNo1, paramSlipBranchNo2, null, paramLanguageId, paramUserId, db, out info, ref errorMsg))
                {
                    return false;
                }

                // 登録したWFヘッダのwf_noを取得
                sql = string.Empty;
                sql += " select wf_no ";
                sql += " from workflow_header ";
                sql += " where slip_no = @SlipNo ";
                sql += " order by input_date desc ";
                sql += " limit 1 ";
                IList<EntityDao.WorkflowHeaderEntity> headerInfo = db.GetListByDataClass<EntityDao.WorkflowHeaderEntity>(sql, new { SlipNo = param.Value });

                // 全ての承認担当者を承認状態にする
                //foreach (var user in detailList)
                //{
                // 承認(共通処理では表示順が重複している場合エラーになるので全件updateで対応)
                sql = string.Empty;
                sql += " update workflow_detail ";
                sql += " set status = 10 "; // 承認
                sql += "     , approval_date = @Now ";
                if (paramComment != null)
                {
                    sql += " , comments = @Comment ";
                }
                sql += "     , update_date = @Now ";
                sql += "     , updator_cd = @UserId ";
                sql += " where wf_no = @WfNo ";
                int result = db.Regist(sql, new { Now = DateTime.Now, Comment = paramComment, UserId = paramUserId, WfNo = headerInfo[0].WfNo });

                //if (!SetApprovalStatusForWorkFlow(Constants.WORKFLOW.DETAILSTATUS.APPROVAL, paramConductId, param.Value, paramSlipBranchNo1, paramSlipBranchNo2, paramComment, paramMailparamList,
                //    paramLanguageId, user.ApprovalUserId, db, out ComBusBase.PushInfo pushInfo, ref errorMsg, null, null, null))
                //{
                //    return false;
                //}
                //}

                // ワークフローヘッダのステータスを「承認完了」にする
                sql = string.Empty;
                sql += " update workflow_header ";
                sql += " set status = 20 "; // 承認完了
                sql += "     , update_date = @Now ";
                sql += "     , updator_cd = @UserId ";
                sql += " where wf_no = @WfNo ";
                result = db.Regist(sql, new { Now = DateTime.Now, UserId = paramUserId, WfNo = headerInfo[0].WfNo });
            }

            return true;
        }
        #endregion

        /// <summary>テキストファイル取込クラス </summary>
        public class UploadText
        {
            /// <summary>TSVファイルの内容(テキスト)</summary>
            private List<List<string>> textData;
            /// <summary>メッセージ出力用・言語ID</summary>
            private string languageId;
            /// <summary>メッセージ出力用・メッセージリソース</summary>
            private ComUtil.MessageResources msgResources;
            /// <summary>DB接続</summary>
            private ComDB db;
            /// <summary>
            /// コンストラクタ(UTF-8、TSV)
            /// </summary>
            /// <param name="uploadFile">InputStream</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="msgResources">メッセージリソース</param>
            /// <param name="db">DB接続</param>
            public UploadText(Stream uploadFile, string languageId, ComUtil.MessageResources msgResources, ComDB db)
            {
                setTextData(uploadFile, Encoding.UTF8, ComUtil.CharacterConsts.Tab);
                setMemberValues(languageId, msgResources, db);
            }
            /// <summary>
            /// コンストラクタ(エンコーディング、区切り文字指定)
            /// </summary>
            /// <param name="uploadFile">InputStream</param>
            /// <param name="encoding">ファイルのエンコーディング</param>
            /// <param name="delimiter">ファイルの区切り文字</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="msgResources">メッセージリソース</param>
            /// <param name="db">DB接続</param>
            public UploadText(Stream uploadFile, Encoding encoding, char delimiter, string languageId, ComUtil.MessageResources msgResources, ComDB db)
            {
                setTextData(uploadFile, encoding, delimiter);
                setMemberValues(languageId, msgResources, db);
            }

            /// <summary>
            /// テキストデータ以外のメンバ変数を設定する処理
            /// </summary>
            /// <param name="inLanguageId">言語ID</param>
            /// <param name="inMsgResources">メッセージリソース</param>
            /// <param name="inDb">DB接続</param>
            private void setMemberValues(string inLanguageId, ComUtil.MessageResources inMsgResources, ComDB inDb)
            {
                this.languageId = inLanguageId;
                this.msgResources = inMsgResources;
                this.db = inDb;
            }
            /// <summary>
            /// 取込ファイルの内容を文字列のリストに設定する処理
            /// </summary>
            /// <param name="inUploadFile">InputStream</param>
            /// <param name="inEncoding">ファイルのエンコーディング</param>
            /// <param name="inDelimiter">ファイルの区切り文字</param>
            /// <remarks>コンストラクタで引数省略をしたいけど、出来ないからオーバーライド用のベースクラス</remarks>
            private void setTextData(Stream inUploadFile, Encoding inEncoding, char inDelimiter)
            {
                // 初期化
                this.textData = new List<List<string>>();
                // 読み込み
                string fileTexts = getFileText(inUploadFile, inEncoding);
                // 改行コードで分割
                string[] textData = fileTexts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                // 読み込んでリストに変換
                // 行で繰り返し
                foreach (string row in textData)
                {
                    if (string.IsNullOrEmpty(row))
                    {
                        // 空行はスキップ
                        continue;
                    }
                    // 1行の内容を格納するリスト
                    List<string> rowData = new List<string>();
                    // 区切り文字で1行の内容を分割
                    string[] vals = ComUtil.SplitCSV(row, inDelimiter);
                    // 列で繰り返し
                    foreach (string val in vals)
                    {
                        // 行に列の内容を追加
                        rowData.Add(val);
                    }
                    // 行の内容を追加
                    this.textData.Add(rowData);
                }
            }

            /// <summary>
            /// ファイルを読み込んで、文字列に変換する処理
            /// </summary>
            /// <param name="inUploadFile">InputStream</param>
            /// <param name="inEncoding">ファイルのエンコーディング</param>
            /// <returns>読み込んだファイルの内容(文字列)</returns>
            /// <remarks>CommonSTDUtil.ImportFileWithDelimiterよりコピー</remarks>
            private string getFileText(Stream inUploadFile, Encoding inEncoding)
            {
                string fileTexts = string.Empty;
                try
                {
                    using (var memStream = new MemoryStream())
                    {
                        // 取込ファイルのバイト配列をコピー
                        inUploadFile.CopyTo(memStream);
                        byte[] bytes = memStream.ToArray();
                        if (bytes == null || bytes.Length <= 0)
                        {
                            // 取込ファイルにデータなし
                            // 空の配列を返却する
                            // エラーかどうかは呼び元で判断する
                            return string.Empty;
                        }

                        // 全行取り出し
                        fileTexts = inEncoding.GetString(bytes);
                    }
                }
                finally
                {
                    inUploadFile.Close();
                    inUploadFile = null;
                }
                return fileTexts;
            }

            /// <summary>
            /// 取込共通チェック処理
            /// </summary>
            /// <typeparam name="T">型</typeparam>
            /// <param name="conductId">機能ID</param>
            /// <param name="fileNo">ファイル管理NO</param>
            /// <param name="sheetNo">シート番号(0オリジン)</param>
            /// <param name="itemid">項目ID</param>
            /// <param name="resultList">ref 取込結果格納クラス</param>
            /// <param name="checkFlg">省略可能　チェックフラグ　省略時はチェック有り</param>
            /// <returns>エラー情報</returns>
            /// <remarks>ComUploadErrorCheckよりコピー</remarks>
            public List<BaseClass.UploadErrorInfo> ComUploadErrorCheck<T>(string conductId, int fileNo, int sheetNo, string itemid, ref List<T> resultList, bool checkFlg = true)
            {
                // エラー内容格納クラス
                List<BaseClass.UploadErrorInfo> errorInfo = new List<BaseClass.UploadErrorInfo>();

                // ファイル入出力項目定義情報を取得
                IList<BaseClass.InoutDefine> reportInfoList = this.db.GetListByOutsideSqlByDataClass<BaseClass.InoutDefine>(ComReport.GetComReportInfo, DirPath,
                    new { Conductid = conductId, Fileno = fileNo, Sheetno = sheetNo, Inputflg = true, Itemid = itemid });
                if (reportInfoList == null || reportInfoList.Count == 0)
                {
                    // 取得できない場合、処理を戻す
                    return null;
                }

                // 検索結果クラスのプロパティを列挙
                var properites = typeof(T).GetProperties();
                // 1レコード分の行数、1レコード分の行数を取得する
                int addRow = reportInfoList[0].Recordcount;
                // 入出力方式を取得
                if (reportInfoList[0].Datadirection == null)
                {
                    // 取得できない場合、処理を戻す
                    return null;
                }
                int datadirection = (int)reportInfoList[0].Datadirection;

                int index = 0;

                while (true)
                {
                    // エラー内容一時格納クラス
                    List<BaseClass.UploadErrorInfo> tmpErrorInfo = new List<BaseClass.UploadErrorInfo>();

                    bool flg = false; // データ存在チェックフラグ
                    object tmpResult = Activator.CreateInstance<T>();

                    // 取得できた項目定義分処理を行う
                    foreach (var reportInfo in reportInfoList)
                    {
                        // 開始行番号、開始列番号が未指定の場合、スキップ
                        if (reportInfo.Startrowno == null || reportInfo.Startcolno == null)
                        {
                            continue;
                        }
                        // 2行目以降、入出力方式によって、表示位置をずらす
                        if (index > 0)
                        {
                            switch (datadirection)
                            {
                                case ComReport.SingleCell:
                                    // 基本、到達しない
                                    continue;
                                case ComReport.LongitudinalDirection:
                                    // 縦方向連続の場合、行番号を加算する
                                    reportInfo.Startrowno += addRow;
                                    break;
                                case ComReport.LateralDirection:
                                    // 横方向連続の場合、列番号を加算する
                                    reportInfo.Startcolno += addRow;
                                    break;
                                default:
                                    // 入出力方式が未設定の場合、スキップ
                                    break;
                            }
                        }

                        // 行と列の番号を取得(Nullは無いので適当)
                        int rowIndex = (reportInfo.Startrowno ?? 0) - 1;
                        int colIndex = (reportInfo.Startcolno ?? 0) - 1;
                        if (rowIndex >= this.textData.Count)
                        {
                            break;
                        }
                        if (colIndex >= this.textData[rowIndex].Count)
                        {
                            // レイアウト定義では読み込むのに、取込ファイルに含まれない場合エラー
                            // 「ファイルレイアウトが有効ではありません。」
                            string error = getResMessage(APResources.ID.MS00096, this.languageId, this.msgResources);
                            tmpErrorInfo.Add(setErrorInfo((int)reportInfo.Startrowno, (int)reportInfo.Startcolno, error));
                            errorInfo.AddRange(tmpErrorInfo);
                            // レイアウトがおかしい場合、読み込みが無限ループの恐れがあるため、終了
                            return errorInfo;
                        }
                        string val = this.textData[rowIndex][colIndex];

                        if (checkFlg)
                        {
                            // 値が取得できない場合、スキップ
                            if (checkNull(val, reportInfo, ref tmpErrorInfo))
                            {
                                continue;
                            }
                            // Nullの場合、エラーでなくても処理を抜ける
                            if (string.IsNullOrEmpty(val))
                            {
                                continue;
                            }
                        }

                        // 入力項目が存在する場合、フラグをたてる
                        flg = true;

                        if (checkFlg)
                        {
                            // 入力チェック
                            if (checkInput(val, reportInfo, ref tmpErrorInfo))
                            {
                                continue;
                            }

                            // 桁数チェック
                            if (checkLength(val, reportInfo, ref tmpErrorInfo))
                            {
                                continue;
                            }
                        }

                        // 値をデータクラスに設定
                        string pascalItemName = ComUtil.SnakeCaseToPascalCase(reportInfo.Itemname).ToUpper();
                        var prop = properites.FirstOrDefault(x => x.Name.ToUpper().Equals(pascalItemName));
                        if (prop == null)
                        {
                            // 該当する項目が存在しない場合、スキップ
                            continue;
                        }
                        ComUtil.SetPropertyValue<T>(prop, (T)tmpResult, val);
                    }
                    // データが1件も取得できなかった場合、処理を抜ける
                    if (!flg)
                    {
                        break;
                    }

                    // データが存在する場合、リストに追加する
                    errorInfo.AddRange(tmpErrorInfo);
                    resultList.Add((T)tmpResult);
                    index++;

                    // 入力方式が単一セルの場合、処理を抜ける
                    if ((int)datadirection == ComReport.SingleCell)
                    {
                        break;
                    }
                }
                return errorInfo;
            }

            /// <summary>
            /// 必須チェック
            /// </summary>
            /// <param name="val">チェックする値</param>
            /// <param name="reportInfo">ファイル入出力項目定義情報</param>
            /// <param name="tmpErrorInfo">エラー内容一時格納クラス</param>
            /// <returns>エラー有の場合、True</returns>
            private bool checkNull(string val, BaseClass.InoutDefine reportInfo, ref List<BaseClass.UploadErrorInfo> tmpErrorInfo)
            {
                if (string.IsNullOrEmpty(val))
                {
                    if (reportInfo.Nullcheckflg != null && (bool)reportInfo.Nullcheckflg)
                    {
                        // 必須入力項目の場合、エラー内容を設定
                        // 「必須項目です。入力してください。」
                        string error = getResMessage(APResources.ID.MS00075, this.languageId, this.msgResources);
                        tmpErrorInfo.Add(setErrorInfo((int)reportInfo.Startrowno, (int)reportInfo.Startcolno, error));
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// 入力チェック
            /// </summary>
            /// <param name="val">チェックする値</param>
            /// <param name="reportInfo">ファイル入出力項目定義情報</param>
            /// <param name="tmpErrorInfo">エラー内容一時格納クラス</param>
            /// <returns>エラー有の場合、True</returns>
            private bool checkInput(string val, BaseClass.InoutDefine reportInfo, ref List<BaseClass.UploadErrorInfo> tmpErrorInfo)
            {
                if (reportInfo.Celltype != null)
                {
                    string error = string.Empty;
                    if (!checkCellType((int)reportInfo.Celltype, val, this.languageId, this.msgResources, ref error))
                    {
                        // 型が異なる場合、エラーを設定し、スキップ
                        tmpErrorInfo.Add(setErrorInfo((int)reportInfo.Startrowno, (int)reportInfo.Startcolno, error));
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// 桁数チェック
            /// </summary>
            /// <param name="val">チェックする値</param>
            /// <param name="reportInfo">ファイル入出力項目定義情報</param>
            /// <param name="tmpErrorInfo">エラー内容一時格納クラス</param>
            /// <returns>エラー有の場合、True</returns>
            private bool checkLength(string val, BaseClass.InoutDefine reportInfo, ref List<BaseClass.UploadErrorInfo> tmpErrorInfo)
            {
                if (reportInfo.Maxlength != null && (int)reportInfo.Maxlength > 0)
                {
                    // セルタイプが指定され、日付以外の場合、チェックを行う
                    if (reportInfo.Celltype != null && (int)reportInfo.Celltype != ComReport.InputTypeDat)
                    {
                        // 桁数を超えている場合、エラーを設定を設定し、スキップ
                        if (val.Length > (int)reportInfo.Maxlength)
                        {
                            string error = string.Empty;
                            // 「入力値が設定桁数を超えています。」
                            error = getResMessage(new string[] { APResources.ID.MS00076, reportInfo.Maxlength.ToString() }, this.languageId, this.msgResources);
                            tmpErrorInfo.Add(setErrorInfo((int)reportInfo.Startrowno, (int)reportInfo.Startcolno, error));
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        #region エクセル入出力共通処理
        /// <summary>
        /// ファイル マッピング情報
        /// </summary>
        /// <typeparam name="T">データクラス</typeparam>
        /// <param name="conductId">機能ID</param>
        /// <param name="fileNo">ファイル管理NO</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="itemId">項目ID</param>
        /// <param name="list">出力結果</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>ファイル マッピング情報リスト</returns>
        public static List<CommonExcelPrtInfo> CreateMappingList<T>(
            string conductId, int fileNo, int sheetNo, string itemId, IList<T> list, ComDB db)
        {
            // 初期化
            var mappingList = new List<CommonExcelPrtInfo>();

            // ファイル入出力項目定義情報を取得
            IList<BaseClass.InoutDefine> reportInfoList = db.GetListByOutsideSqlByDataClass<BaseClass.InoutDefine>(ComReport.GetComReportInfo, DirPath,
                new { Conductid = conductId, Fileno = fileNo, Sheetno = sheetNo, Inputflg = false, Itemid = itemId });
            if (reportInfoList == null || reportInfoList.Count == 0)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // 検索結果クラスのプロパティを列挙
            var properites = typeof(T).GetProperties();
            // 1レコード分の行数、1レコード分の行数を取得する
            int addRow = reportInfoList[0].Recordcount;
            // 入出力方式を取得
            if (reportInfoList[0].Datadirection == null)
            {
                // 取得できない場合、処理を戻す
                return null;
            }
            int datadirection = (int)reportInfoList[0].Datadirection;

            int index = 0;
            foreach (var data in list)
            {
                foreach (var reportInfo in reportInfoList)
                {
                    // 開始行番号、開始列番号が未指定の場合、スキップ
                    if (reportInfo.Startrowno == null || reportInfo.Startcolno == null)
                    {
                        continue;
                    }

                    // 2行目以降、入出力方式によって、表示位置をずらす
                    if (index > 0)
                    {
                        switch (datadirection)
                        {
                            case ComReport.SingleCell:
                                continue;
                            case ComReport.LongitudinalDirection:
                                // 縦方向連続の場合、行番号を加算する
                                reportInfo.Startrowno += addRow;
                                break;
                            case ComReport.LateralDirection:
                                // 横方向連続の場合、列番号を加算する
                                reportInfo.Startcolno += addRow;
                                break;
                            default:
                                // 入出力方式が未設定の場合、スキップ
                                break;
                        }
                    }

                    // 初期化
                    var info = new CommonExcelPrtInfo();
                    info.SetSheetName(null);  // シート名にnullを設定(シート番号でマッピングを行うため)
                    info.SetSheetNo(sheetNo + 1); // シート番号に対象のシート番号を設定

                    string pascalItemName = ComUtil.SnakeCaseToPascalCase(reportInfo.Itemname).ToUpper();
                    var prop = properites.FirstOrDefault(x => x.Name.ToUpper().Equals(pascalItemName));
                    if (prop == null)
                    {
                        // 該当する項目が存在しない場合、スキップ
                        continue;
                    }
                    if (ComUtil.IsNullOrEmpty(prop.GetValue(data)))
                    {
                        // 対象データが存在しない(nullの)場合、処理をスキップ
                        continue;
                    }

                    // マッピングセルを設定
                    string address = ToAlphabet((int)reportInfo.Startcolno) + reportInfo.Startrowno;
                    // フォーマット設定
                    string format = null;
                    var test = reportInfo.Itemname;
                    if (reportInfo.Celltype != null && reportInfo.Celltype == ComReport.InputTypeStr)
                    {
                        format = ComReport.StrFormat;
                    }
                    info.SetExlSetValueByAddress(address, prop.GetValue(data), format);

                    mappingList.Add(info);
                }
                // 行数を加算
                index++;
            }

            return mappingList;
        }

        /// <summary>
        /// テンプレートファイル名取得処理
        /// </summary>
        /// <param name="conductId">機能ID</param>
        /// <param name="fileNo">ファイル管理NO</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>テンプレートファイル名</returns>
        public static string GetTmplateFileName(string conductId, int fileNo, ComDB db)
        {
            // 機能ID、ファイル管理NOから該当のテンプレートファイル名を取得
            EntityDao.ComInoutFileDefineEntity fileInfo = new EntityDao.ComInoutFileDefineEntity();
            fileInfo = fileInfo.GetEntity(conductId, fileNo, db);

            // 該当のレコードが存在しない場合、nullを戻す
            if (fileInfo == null)
            {
                return null;
            }

            return fileInfo.Templatename;
        }

        /// <summary>
        /// シート名取得処理
        /// </summary>
        /// <param name="conductId">機能ID</param>
        /// <param name="fileNo">ファイル管理NO</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="itemId">項目ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>シート名</returns>
        public static string GetSheetName(string conductId, int fileNo, int sheetNo, string itemId, ComDB db)
        {
            // 機能ID、ファイル管理NO、シート番号、項目IDから該当のシート名を取得
            EntityDao.ComInoutGroupDefineEntity groupInfo = new EntityDao.ComInoutGroupDefineEntity();
            groupInfo = groupInfo.GetEntity(conductId, fileNo, sheetNo, itemId, db);

            // 該当のレコードが存在しない場合、nullを戻す
            if (groupInfo == null)
            {
                return null;
            }

            return groupInfo.Itemname;
        }

        /// <summary>
        /// 対象セル範囲を取得（縦方向連続方向の帳票のみ対応）
        /// </summary>
        /// <param name="conductId">機能ID</param>
        /// <param name="fileNo">ファイル管理NO</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="itemid">項目ID</param>
        /// <param name="cnt">件数</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>対象セル範囲</returns>
        public static string GetTargetCellRange(string conductId, int fileNo, int sheetNo, string itemid, int cnt, ComDB db)
        {
            int? startCol = null;
            int? endCol = null;
            int? startRow = null;

            // ファイル入出力項目定義情報を取得
            IList<BaseClass.InoutDefine> reportInfoList = db.GetListByOutsideSqlByDataClass<BaseClass.InoutDefine>(ComReport.GetComReportInfo, DirPath,
                new { Conductid = conductId, Fileno = fileNo, Sheetno = sheetNo, Inputflg = false, Itemid = itemid });
            if (reportInfoList == null)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // 1レコード分の行数を取得する
            int addRow = reportInfoList[0].Recordcount;
            // 開始列を設定
            IList<BaseClass.InoutDefine> workList = reportInfoList.Where(x => x.Datadirection != null && !x.Datadirection.Equals(ComReport.SingleCell)).OrderBy(x => x.Startcolno).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.Startcolno))
                {
                    continue;
                }
                startCol = (int)work.Startcolno;
                break; // 取得できた場合、処理を抜ける
            }

            // 終了列を設定
            workList = reportInfoList.Where(x => x.Datadirection != null && !x.Datadirection.Equals(ComReport.SingleCell)).OrderByDescending(x => x.Startcolno).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.Startcolno))
                {
                    continue;
                }
                endCol = (int)work.Startcolno;
                break; // 取得できた場合、処理を抜ける
            }

            // 開始行を設定
            workList = reportInfoList.Where(x => x.Datadirection != null && !x.Datadirection.Equals(ComReport.SingleCell)).OrderBy(x => x.Startrowno).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.Startrowno))
                {
                    continue;
                }
                startRow = (int)work.Startrowno;
                break; // 取得できた場合、処理を抜ける
            }

            // 開始行列、終了列を取得できなかった場合、nullを戻す
            if (startCol == null && endCol == null && startRow == null)
            {
                return null;
            }

            return ToAlphabet((int)startCol) + startRow + ":" + ToAlphabet((int)endCol) + (startRow + (addRow * cnt) - 1);
        }

        /// <summary>
        /// コマンド情報取得（縦方向連続方向の帳票のみ対応）
        /// </summary>
        /// <param name="conductId">機能ID</param>
        /// <param name="fileNo">ファイル管理NO</param>
        /// <param name="sheetNo">シート番号</param>
        /// <param name="itemId">項目ID</param>
        /// <param name="cnt">件数</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>コマンド情報</returns>
        public static List<CommonExcelCmdInfo> CreateCmdInfoList(string conductId, int fileNo, int sheetNo, string itemId, int cnt, ComDB db)
        {
            string sheetName = null;
            int? startCol = null;
            int? endCol = null;
            int? startRow = null;
            int recordCount = 1;

            // 初期化
            var cmdInfoList = new List<CommonExcelCmdInfo>();

            // シート名取得
            sheetName = GetSheetName(conductId, fileNo, sheetNo, itemId, db);

            // ファイル入出力項目定義情報を取得
            IList<BaseClass.InoutDefine> reportInfoList = db.GetListByOutsideSqlByDataClass<BaseClass.InoutDefine>(ComReport.GetComReportInfo, DirPath,
                new { Conductid = conductId, Fileno = fileNo, Sheetno = sheetNo, Inputflg = false, Itemid = itemId });
            if (reportInfoList == null)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // 開始列を設定
            IList<BaseClass.InoutDefine> workList = reportInfoList.Where(x => x.Datadirection != null && !x.Datadirection.Equals(ComReport.SingleCell)).OrderBy(x => x.Startcolno).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.Startcolno))
                {
                    continue;
                }
                startCol = (int)work.Startcolno;
                break; // 取得できた場合、処理を抜ける
            }

            // 終了列を設定
            workList = reportInfoList.Where(x => x.Datadirection != null && !x.Datadirection.Equals(ComReport.SingleCell)).OrderByDescending(x => x.Startcolno).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.Startcolno))
                {
                    continue;
                }
                endCol = (int)work.Startcolno;
                break; // 取得できた場合、処理を抜ける
            }

            // 開始行を設定
            workList = reportInfoList.Where(x => x.Datadirection != null && !x.Datadirection.Equals(ComReport.SingleCell)).OrderBy(x => x.Startrowno).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.Startrowno))
                {
                    continue;
                }
                startRow = (int)work.Startrowno;
                break; // 取得できた場合、処理を抜ける
            }

            // レコード行数を設定
            workList = reportInfoList.Where(x => x.Datadirection != null && !x.Datadirection.Equals(ComReport.SingleCell)).OrderBy(x => x.Startrowno).ToArray();
            foreach (var work in workList)
            {
                // 未設定の場合、スキップ
                if (ComUtil.IsNullOrEmpty(work.Recordcount))
                {
                    continue;
                }
                recordCount = (int)work.Recordcount;
                break; // 取得できた場合、処理を抜ける
            }

            // シート名、開始行、終了列を取得できなかった場合、nullを戻す
            if (sheetName == null || endCol == null || startRow == null)
            {
                return null;
            }

            // ベタ表コマンド情報作成
            cmdInfoList.AddRange(ExcelUtil.CreateCmdInfoListForSimpleList(sheetName, cnt, (int)startRow, ToAlphabet((int)startCol), ToAlphabet((int)endCol), recordCount));

            return cmdInfoList;
        }

        /// <summary>
        /// 取込ファイル読込処理
        /// </summary>
        /// <param name="fileStream">読込対象エクセルStream</param>
        /// <param name="delimiter">デリミタ文字</param>
        /// <returns>ファイル情報</returns>
        public static CommonExcelCmd FileOpen(Stream fileStream, string delimiter = null)
        {
            CommonExcelCmd cmd = null;
            // エクセルコマンドクラス
            if (!string.IsNullOrEmpty(delimiter))
            {
                //TODO:UploadTextクラスのテキスト読み込み処理を使用してください
                //cmd = new CommonExcelCmd(fileStream, delimiter);
            }
            else
            {
                cmd = new CommonExcelCmd(fileStream);
            }

            return cmd;
        }

        /// <summary>
        /// 取込共通チェック処理
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="cmd">エクセルコマンド処理クラス</param>
        /// <param name="conductId">機能ID</param>
        /// <param name="fileNo">ファイル管理NO</param>
        /// <param name="sheetNo">シート番号(0オリジン)</param>
        /// <param name="itemid">項目ID</param>
        /// <param name="resultList">結果格納クラス</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="checkFlg">省略可能　チェックフラグ　省略時はチェック有り</param>
        /// <returns>エラー情報</returns>
        public static List<BaseClass.UploadErrorInfo> ComUploadErrorCheck<T>(
            CommonExcelCmd cmd, string conductId, int fileNo, int sheetNo, string itemid, ref List<T> resultList,
            string languageId, ComUtil.MessageResources msgResources, ComDB db, bool checkFlg = true)
        {
            // エラー内容格納クラス
            List<BaseClass.UploadErrorInfo> errorInfo = new List<BaseClass.UploadErrorInfo>();

            // ファイル入出力項目定義情報を取得
            IList<BaseClass.InoutDefine> reportInfoList = db.GetListByOutsideSqlByDataClass<BaseClass.InoutDefine>(ComReport.GetComReportInfo, DirPath,
                new { Conductid = conductId, Fileno = fileNo, Sheetno = sheetNo, Inputflg = true, Itemid = itemid });
            if (reportInfoList == null || reportInfoList.Count == 0)
            {
                // 取得できない場合、処理を戻す
                return null;
            }

            // 検索結果クラスのプロパティを列挙
            var properites = typeof(T).GetProperties();
            // 1レコード分の行数、1レコード分の行数を取得する
            int addRow = reportInfoList[0].Recordcount;
            // 入出力方式を取得
            if (reportInfoList[0].Datadirection == null)
            {
                // 取得できない場合、処理を戻す
                return null;
            }
            int datadirection = (int)reportInfoList[0].Datadirection;

            // 汎用マスタから上下限チェック用データを取得
            string sql = string.Empty;
            sql += "select * from utility where utility_division = @UtilityDivision and utility_cd = @UtilityCd and utility_no = @UtilityNo";

            EntityDao.UtilityEntity bean = db.GetEntityByDataClass<EntityDao.UtilityEntity>(sql,
                new { UtilityDivision = "ULRC", UtilityCd = conductId, UtilityNo = "0" });

            int index = 0;
            while (true)
            {
                // エラー内容一時格納クラス
                List<BaseClass.UploadErrorInfo> tmpErrorInfo = new List<BaseClass.UploadErrorInfo>();

                bool flg = false; // データ存在チェックフラグ
                object tmpResult = Activator.CreateInstance<T>();

                // 取得できた項目定義分処理を行う
                foreach (var reportInfo in reportInfoList)
                {
                    // 開始行番号、開始列番号が未指定の場合、スキップ
                    if (reportInfo.Startrowno == null || reportInfo.Startcolno == null)
                    {
                        continue;
                    }

                    // 2行目以降、入出力方式によって、表示位置をずらす
                    if (index > 0)
                    {
                        switch (datadirection)
                        {
                            case ComReport.SingleCell:
                                // 基本、到達しない
                                continue;
                            case ComReport.LongitudinalDirection:
                                // 縦方向連続の場合、行番号を加算する
                                reportInfo.Startrowno += addRow;
                                break;
                            case ComReport.LateralDirection:
                                // 横方向連続の場合、列番号を加算する
                                reportInfo.Startcolno += addRow;
                                break;
                            default:
                                // 入出力方式が未設定の場合、スキップ
                                break;
                        }
                    }

                    string error = string.Empty;
                    string[,] vals = null;
                    string msg = string.Empty;
                    // マッピングセルを設定
                    string address = ToAlphabet((int)reportInfo.Startcolno) + reportInfo.Startrowno;
                    // セル単位でデータを取得する
                    if (!cmd.ReadExcelBySheetNo(sheetNo + 1, address, ref vals, ref msg))
                    {
                        // 読込失敗した場合、エラー内容を設定
                        // 「該当セルの読込に失敗しました。」
                        error = getResMessage(APResources.ID.MS00074, languageId, msgResources);
                        tmpErrorInfo.Add(setErrorInfo((int)reportInfo.Startrowno, (int)reportInfo.Startcolno, error));
                        errorInfo.AddRange(tmpErrorInfo);
                        // レイアウトがおかしい場合、読み込みが無限ループの恐れがあるため、終了
                        return errorInfo;
                    }

                    // 設定値を取得
                    string val = vals[0, 0]; // セル単位で取得しているので先頭を対象データとみなす。

                    if (checkFlg)
                    {
                        // 値が取得できない場合、スキップ
                        if (string.IsNullOrEmpty(val))
                        {
                            if (reportInfo.Nullcheckflg != null && (bool)reportInfo.Nullcheckflg)
                            {
                                // 必須入力項目の場合、エラー内容を設定
                                // 「必須項目です。入力してください。」
                                error = getResMessage(APResources.ID.MS00075, languageId, msgResources);
                                tmpErrorInfo.Add(setErrorInfo((int)reportInfo.Startrowno, (int)reportInfo.Startcolno, error));
                            }
                            continue;
                        }
                    }

                    // 入力項目が存在する場合、フラグをたてる
                    flg = true;

                    if (checkFlg)
                    {
                        // 入力チェック
                        if (reportInfo.Celltype != null)
                        {
                            // 数値の場合、指数表記の可能性があるので、変換を実施
                            if ((int)reportInfo.Celltype == ComReport.InputTypeNum)
                            {
                                if (val != null && ComUtil.ConvertDecimalIndexNumber(val.ToString()) != null)
                                {
                                    val = ComUtil.ConvertDecimalIndexNumber(val.ToString()).ToString();
                                }
                            }
                            if (!checkCellType((int)reportInfo.Celltype, val, languageId, msgResources, ref error))
                            {
                                // 型が異なる場合、エラーを設定し、スキップ
                                tmpErrorInfo.Add(setErrorInfo((int)reportInfo.Startrowno, (int)reportInfo.Startcolno, error));
                                continue;
                            }
                        }

                        // 桁数チェック
                        if (reportInfo.Maxlength != null && (int)reportInfo.Maxlength > 0)
                        {
                            // セルタイプが指定され、日付以外の場合、チェックを行う
                            if (reportInfo.Celltype != null && (int)reportInfo.Celltype != ComReport.InputTypeDat)
                            {
                                // 桁数を超えている場合、エラーを設定を設定し、スキップ
                                if (val.Length > (int)reportInfo.Maxlength)
                                {
                                    // 「入力値が設定桁数を超えています。」
                                    error = getResMessage(new string[] { APResources.ID.MS00076, reportInfo.Maxlength.ToString() }, languageId, msgResources);
                                    tmpErrorInfo.Add(setErrorInfo((int)reportInfo.Startrowno, (int)reportInfo.Startcolno, error));
                                    continue;
                                }
                            }
                        }

                        // 数値の場合、上下限チェック
                        // 入力チェック
                        if (reportInfo.Celltype != null && (int)reportInfo.Celltype == ComReport.InputTypeNum)
                        {
                            if (!checkRange(bean, val, db, languageId, msgResources, ref error))
                            {
                                // 範囲エラーの場合、エラーを設定し、スキップ
                                tmpErrorInfo.Add(setErrorInfo((int)reportInfo.Startrowno, (int)reportInfo.Startcolno, error));
                                continue;
                            }
                        }
                    }

                    // 値をデータクラスに設定
                    string pascalItemName = ComUtil.SnakeCaseToPascalCase(reportInfo.Itemname).ToUpper();
                    var prop = properites.FirstOrDefault(x => x.Name.ToUpper().Equals(pascalItemName));
                    if (prop == null)
                    {
                        // 該当する項目が存在しない場合、スキップ
                        continue;
                    }
                    ComUtil.SetPropertyValue<T>(prop, (T)tmpResult, val);
                }

                // データが1件も取得できなかった場合、処理を抜ける
                if (!flg)
                {
                    break;
                }

                // データが存在する場合、リストに追加する
                errorInfo.AddRange(tmpErrorInfo);
                resultList.Add((T)tmpResult);
                index++;

                // 入力方式が単一セルの場合、処理を抜ける
                if ((int)datadirection == ComReport.SingleCell)
                {
                    break;
                }
            }

            return errorInfo;
        }

        /// <summary>
        /// 数字→文字列
        /// </summary>
        /// <param name="columnNo">列番号(数字)</param>
        /// <returns>アルファベット</returns>
        public static string ToAlphabet(int columnNo)
        {
            // A ～ YZまでを想定
            string alphabet = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
            string columnStr = string.Empty;
            int m, n = 0;

            m = columnNo % 26; // 一の位
            n = columnNo / 26; // 十の位

            // 一の位がZだった場合
            if (m == 0)
            {
                n = n - 1;
            }

            columnStr = alphabet[m].ToString();

            // 指定列がAA以降の場合
            if (n != 0 && columnNo != 0)
            {
                columnStr = alphabet[n] + columnStr;
            }

            return columnStr;
        }

        /// <summary>
        /// リソースメッセージ取得
        /// </summary>
        /// <param name="key">キー情報</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <returns>メッセージ</returns>
        private static string getResMessage(string key, string languageId, ComUtil.MessageResources msgResources)
        {
            return ComUtil.GetPropertiesMessage(key, languageId, msgResources);
        }

        /// <summary>
        /// リソースメッセージ取得
        /// </summary>
        /// <param name="keys">キー情報</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <returns>メッセージ</returns>
        private static string getResMessage(string[] keys, string languageId, ComUtil.MessageResources msgResources)
        {
            return ComUtil.GetPropertiesMessage(keys, languageId, msgResources);
        }

        /// <summary>
        /// エラー情報設定
        /// </summary>
        /// <param name="rowno">行番号</param>
        /// <param name="itemName">列番号/項目名</param>
        /// <param name="error">エラー内容</param>
        /// <returns>エラー情報</returns>
        private static BaseClass.UploadErrorInfo setErrorInfo(int rowno, int itemName, string error)
        {
            // エラー情報を初期化
            BaseClass.UploadErrorInfo errorInfo = new BaseClass.UploadErrorInfo();

            // エラー情報を設定
            errorInfo.RowNo = rowno;
            errorInfo.ItemName = itemName;
            errorInfo.ErrorInfo = error;

            return errorInfo;
        }

        /// <summary>
        /// 取込処理 型チェック
        /// </summary>
        /// <param name="cellType">セルタイプ</param>
        /// <param name="val">文字列</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="error">エラー情報</param>
        /// <returns>true:正常、false:異常</returns>
        private static bool checkCellType(int cellType, string val, string languageId, ComUtil.MessageResources msgResources, ref string error)
        {
            // セルタイプによって処理を分岐
            switch (cellType)
            {
                case ComReport.InputTypeStr:
                    // 文字列の場合、正常
                    return true;
                case ComReport.InputTypeNum:
                    // 数値の場合
                    if (ComUtil.IsDecimal(val))
                    {
                        return true;
                    }
                    // 「数値で入力してください。」
                    error = getResMessage(new string[] { APResources.ID.MS00078, APResources.ID.MS00046 }, languageId, msgResources);
                    return false;
                case ComReport.InputTypeDat:
                    // 日付の場合
                    if (ComUtil.IsDate(val) || ComUtil.IsDateYyyymmdd(val))
                    {
                        return true;
                    }
                    // 「日付で入力してください。」
                    error = getResMessage(new string[] { APResources.ID.MS00078, APResources.ID.MS00047 }, languageId, msgResources);
                    return false;
                default:
                    // 基本、到達しない
                    return true;
            }
        }

        /// <summary>
        /// 取込処理 上下限チェック
        /// </summary>
        /// <param name="bean">汎用マスタ</param>
        /// <param name="val">文字列</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="msgResources">メッセージリソース</param>
        /// <param name="error">エラー情報</param>
        /// <returns>true:正常、false:異常</returns>
        private static bool checkRange(EntityDao.UtilityEntity bean, string val, ComDB db,
            string languageId, ComUtil.MessageResources msgResources, ref string error)
        {
            // 汎用マスタの定義が未設定の場合、処理を実施しない
            if (bean == null)
            {
                return true;
            }

            // 文字列⇒数値に変換
            decimal? workVal = ComUtil.ConvertDecimalIndexNumber(val);
            if (workVal == null)
            {
                // 変換できない場合、正常で処理を戻す ※型チェックなどのエラーは他で判定しているため
                return true;
            }

            if (bean.Num01 != null && bean.Num02 != null)
            {
                // 上下限値ともに設定されている場合
                if (decimal.Compare((decimal)bean.Num01, (decimal)workVal) > 0 || decimal.Compare((decimal)bean.Num02, (decimal)workVal) < 0)
                {
                    // 「{0}から{1}の範囲で入力して下さい。」
                    error = getResMessage(
                        new string[] { APResources.ID.MS00118, ((double)bean.Num01).ToString(), ((double)bean.Num02).ToString() }, languageId, msgResources);
                    return false;
                }
            }
            else if (bean.Num01 != null && bean.Num02 == null)
            {
                // 下限値のみ設定されている場合
                if (decimal.Compare((decimal)bean.Num01, (decimal)workVal) > 0)
                {
                    // 「{0}以下の値を入力して下さい。」
                    error = getResMessage(new string[] { APResources.ID.MS00120, ((double)bean.Num01).ToString() }, languageId, msgResources);
                    return false;
                }
            }
            else if (bean.Num01 == null && bean.Num02 != null)
            {
                // 上限値のみ設定されている場合
                if (decimal.Compare((decimal)bean.Num02, (decimal)workVal) < 0)
                {
                    // 「{0}以上の値を入力して下さい。」
                    error = getResMessage(new string[] { APResources.ID.MS00119, ((double)bean.Num02).ToString() }, languageId, msgResources);
                    return false;
                }
            }
            else
            {
                // 上下限値ともに未設定の場合
                return true;
            }

            return true;
        }
        #endregion

        /// <summary>
        /// ロット番号やサブロット番号に対して、デフォルト値の変換を行う処理
        /// </summary>
        /// <param name="targetNo">対象のロット番号</param>
        /// <param name="isDisplay">True:画面表示値を取得、False:DB登録値を取得</param>
        /// <param name="defaultLotNo">デフォルトロット番号,GetDefaultLotNoで取得</param>
        /// <returns>変換後のロット番号</returns>
        public static string ConvertLotNo(string targetNo, bool isDisplay, string defaultLotNo)
        {
            string returnVal = string.Empty;
            if (isDisplay)
            {
                // DBの内容を表示する場合
                // デフォルト文字列なら空を設定
                returnVal = defaultLotNo.Equals(targetNo) ? string.Empty : targetNo;
            }
            else
            {
                // 画面の内容をDBに登録する場合
                // 空ならデフォルト文字列を設定
                returnVal = string.IsNullOrEmpty(targetNo) ? defaultLotNo : targetNo;
            }
            return returnVal;
        }

        /// <summary>
        /// マイナス在庫許可チェック
        /// </summary>
        /// <param name="itemCd">品目コード</param>
        /// <param name="specificationCd">仕様コード</param>
        /// <param name="itemActiveDate">品目有効開始日</param>
        /// <param name="locationCd">ロケーションコード</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:マイナス在庫許可、false:マイナス在庫禁止</returns>
        public static bool CheckMinusInventory(string itemCd, string specificationCd, string itemActiveDate, string locationCd, string languageId, ComDB db)
        {
            // 品目仕様情報
            EntityDao.ItemSpecificationEntity itemBean = new EntityDao.ItemSpecificationEntity();
            // ロケーション情報
            EntityDao.LocationEntity locationBean = new EntityDao.LocationEntity();

            // 品目情報にて、マイナス在庫許可フラグを参照し、NGの場合、falseを返す ※取得できない場合はtrueで返す
            if (!string.IsNullOrEmpty(itemCd) && !string.IsNullOrEmpty(specificationCd))
            {
                // 品目有効開始日が設定されている場合（日付型として変換できた場合）
                if (!string.IsNullOrEmpty(itemActiveDate) && ComUtil.ConvertDateTime(itemActiveDate) != null)
                {
                    itemBean = new EntityDao.ItemSpecificationEntity().GetEntity(itemCd, specificationCd, (DateTime)ComUtil.ConvertDateTime(itemActiveDate), db);
                }
                else if (string.IsNullOrEmpty(itemActiveDate) || ComUtil.ConvertDateTime(itemActiveDate) == null)
                {
                    // 日付型として変換できない場合、registビューにて検証を実施
                    itemBean = GetItemSpecificationInfoMaxActiveDate(itemCd, specificationCd, languageId, TargetView.Regist, db);
                }
            }

            // ロケーションコード
            if (!string.IsNullOrEmpty(locationCd))
            {
                locationBean = new EntityDao.LocationEntity().GetEntity(locationCd, db);
            }

            // 品目仕様情報が在庫除外の場合、チェックをしない
            if (itemBean != null &&
                itemBean.StockDivision != null &&
                itemBean.StockDivision == Constants.STOCK_DIVISION.UPDATE_EXCLUSION)
            {
                return true;
            }

            // ロケーション情報が在庫除外の場合、チェックをしない
            if (locationBean != null &&
                locationBean.StockDivision != null &&
                locationBean.StockDivision == Constants.STOCK_DIVISION.UPDATE_EXCLUSION)
            {
                return true;
            }

            // 品目仕様情報
            if (itemBean != null &&
                itemBean.NegativeInventoryPermitFlg != null)
            {
                // マイナス在庫NGの場合
                if (itemBean.NegativeInventoryPermitFlg.Equals(Constants.NEGATIVE_INVENTORY_PERMIT_FLG.NG))
                {
                    return false;
                }
                // 自社マスタの場合
                else if (itemBean.NegativeInventoryPermitFlg.Equals(Constants.NEGATIVE_INVENTORY_PERMIT_FLG.Company))
                {
                    // 自社マスタを参照し、マイナス在庫NGの場合
                    EntityDao.CompanyEntity companyBean = new EntityDao.CompanyEntity().GetEntity(Constants.COMPANY.COMPANY_CD, db);
                    if (companyBean != null &&
                        companyBean.NegativeInventoryPermitFlg != null &&
                        companyBean.NegativeInventoryPermitFlg.Equals(Constants.NEGATIVE_INVENTORY_PERMIT_FLG.NG))
                    {
                        return false;
                    }
                }
            }

            // ロケーション情報
            if (locationBean != null &&
                locationBean.NegativeInventoryPermitFlg != null)
            {
                // マイナス在庫NGの場合
                if (locationBean.NegativeInventoryPermitFlg.Equals(Constants.NEGATIVE_INVENTORY_PERMIT_FLG.NG))
                {
                    return false;
                }
                // 自社マスタの場合
                else if (locationBean.NegativeInventoryPermitFlg.Equals(Constants.NEGATIVE_INVENTORY_PERMIT_FLG.Company))
                {
                    // 自社マスタを参照し、マイナス在庫NGの場合
                    EntityDao.CompanyEntity companyBean = new EntityDao.CompanyEntity().GetEntity(Constants.COMPANY.COMPANY_CD, db);
                    if (companyBean != null &&
                        companyBean.NegativeInventoryPermitFlg != null &&
                        companyBean.NegativeInventoryPermitFlg.Equals(Constants.NEGATIVE_INVENTORY_PERMIT_FLG.NG))
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        #region 月次処理
        /// <summary>
        /// 月次処理管理ステータスを取得
        /// </summary>
        /// <param name="accountYear">勘定年月</param>
        /// <param name="closeDivision">締処理区分</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>取得したステータス</returns>
        public static int? CheckMonthManagementStatus(string accountYear, int closeDivision, ComDB db)
        {
            // SQL文生成
            string sql = string.Empty;
            sql += "select status ";
            sql += "  from month_management ";
            sql += " where account_years = @AccountYears ";
            sql += "   and close_division = @CloseDivision ";
            return db.GetEntity<int?>(sql, new { AccountYears = accountYear, CloseDivision = closeDivision });
        }

        /// <summary>
        /// 月次処理管理テーブル ※未実行
        /// </summary>
        /// <param name="accountYear">勘定年月</param>
        /// <param name="closeDivision">締処理区分</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:正常、false:終了</returns>
        public static bool NotExecMonthlyManagement(string accountYear, int closeDivision, string userId, ComDB db)
        {
            return ComUpdateMonthlyManagement(accountYear, closeDivision, Constants.MONTH_MANAGEMENT.STATUS.NOT_EXECUTED, userId, db, true, true);
        }

        /// <summary>
        /// 月次処理管理テーブル ※実行中
        /// </summary>
        /// <param name="accountYear">勘定年月</param>
        /// <param name="closeDivision">締処理区分</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:正常、false:終了</returns>
        public static bool UpdateMonthlyManagement(string accountYear, int closeDivision, string userId, ComDB db)
        {
            return ComUpdateMonthlyManagement(accountYear, closeDivision, Constants.MONTH_MANAGEMENT.STATUS.DURING_EXECUTION, userId, db, true);
        }

        /// <summary>
        /// 月次処理管理テーブル ※クローズ処理
        /// </summary>
        /// <param name="accountYear">勘定年月</param>
        /// <param name="closeDivision">締処理区分</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:正常、false:終了</returns>
        public static bool ClosedMonthlyManagement(string accountYear, int closeDivision, string userId, ComDB db)
        {
            return ComUpdateMonthlyManagement(accountYear, closeDivision, Constants.MONTH_MANAGEMENT.STATUS.CLOSED, userId, db, false);
        }

        /// <summary>
        /// 月次処理管理テーブル ※失敗処理
        /// </summary>
        /// <param name="accountYear">勘定年月</param>
        /// <param name="closeDivision">締処理区分</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>true:正常、false:終了</returns>
        public static bool FailedMonthlyManagement(string accountYear, int closeDivision, string userId, ComDB db)
        {
            return ComUpdateMonthlyManagement(accountYear, closeDivision, Constants.MONTH_MANAGEMENT.STATUS.FAILED, userId, db, false);
        }

        /// <summary>
        /// 月次処理管理テーブル共通処理
        /// </summary>
        /// <param name="accountYear">勘定年月</param>
        /// <param name="closeDivision">締処理区分</param>
        /// <param name="status">ステータス</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="updateFlg">true:実行開始日時、false:実行完了日時</param>
        /// <param name="clearFlg">true:実行開始、完了、担当者をクリア、false:クリアしない</param>
        /// <returns>true:正常、false:終了</returns>
        public static bool ComUpdateMonthlyManagement(string accountYear, int closeDivision, int status, string userId, ComDB db, bool updateFlg, bool clearFlg = false)
        {
            string exec_date = updateFlg ? "exec_start_date" : "exec_end_date";

            // トランザクション開始
            using (var transaction = db.Connection.BeginTransaction())
            {
                try
                {
                    // SQL文生成
                    string sql = string.Empty;
                    // 月次処理管理テーブルにレコードが存在するか確認
                    sql += "select count(*) ";
                    sql += "  from month_management ";
                    sql += " where account_years = @AccountYears ";
                    sql += "   and close_division = @CloseDivision ";

                    // SQL実行
                    int cnt = db.GetCount(sql, new { AccountYears = accountYear, CloseDivision = closeDivision });

                    // レコードが存在する場合、更新処理
                    sql = string.Empty; // 初期化
                    if (cnt > 0)
                    {
                        sql += "update month_management ";
                        sql += "   set ";
                        if (clearFlg)
                        {
                            sql += "   exec_start_date = null, ";
                            sql += "   exec_end_date = null, ";
                            sql += "   exec_user_id = null, ";
                        }
                        else
                        {
                            sql += "        " + exec_date + " = now(), ";
                            sql += "       exec_user_id = @UserId, ";
                        }
                        sql += "       status = @Status, ";
                        sql += "       update_date = now(), ";
                        sql += "       update_user_id = @UserId ";
                        sql += " where account_years = @AccountYears ";
                        sql += "   and close_division = @CloseDivision ";
                    }
                    else
                    {
                        sql += "insert into month_management ";
                        sql += "( ";
                        sql += "     account_years, ";
                        sql += "     close_division, ";
                        if (!clearFlg)
                        {
                            sql += " " + exec_date + ", ";
                            sql += " exec_user_id, ";
                        }
                        sql += "     status, ";
                        sql += "     input_date, ";
                        sql += "     input_user_id, ";
                        sql += "     update_date, ";
                        sql += "     update_user_id ";
                        sql += ") ";
                        sql += "values ";
                        sql += "( ";
                        sql += "     @AccountYears, ";
                        sql += "     @CloseDivision, ";
                        if (!clearFlg)
                        {
                            sql += " now(), ";
                            sql += " @UserId, ";
                        }
                        sql += "     @Status, ";
                        sql += "     now(), ";
                        sql += "     @UserId, ";
                        sql += "     now(), ";
                        sql += "     @UserId ";
                        sql += ") ";
                    }

                    // 更新処理
                    int result = db.Regist(sql, new { UserId = userId, Status = status, AccountYears = accountYear, CloseDivision = closeDivision });
                    if (result < 0)
                    {
                        // ロールバック
                        transaction.Rollback();

                        // 更新処理に失敗した場合、エラーで返す
                        return false;
                    }

                    // コミット
                    transaction.Commit();

                    // 更新処理成功
                    return true;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        // ロールバック
                        transaction.Rollback();
                    }

                    logger.Error(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 締め処理最大勘定年月
        /// </summary>
        /// <param name="closeDiv">締処理区分 ※APConstantsのMONTH_MANAGEMENT.CLOSE_DIVISIONに定義されている</param>
        /// <param name="db">データベース操作クラス</param>
        /// <returns>最大勘定年月の月末日を返す(yyyy/MM/dd) ※取得できない場合、nullを返す</returns>
        public static string GetMaxMonthlyAccountYears(int closeDiv, ComDB db)
        {
            // SQL文生成
            string sql = string.Empty;
            // 月次処理管理テーブルの締処理区分によって、クローズされている最大勘定年月を取得する
            sql += "select account_years ";
            sql += "  from month_management ";
            sql += " where close_division = @CloseDivision ";
            sql += "   and status = 2 "; // 「2:クローズ」固定
            sql += "order by account_years desc limit 1";

            // 最大勘定年月を取得
            string accountYears = db.GetEntity<string>(sql, new { CloseDivision = closeDiv });
            if (string.IsNullOrEmpty(accountYears))
            {
                // 取得できない場合、nullを返す
                return null;
            }

            // 日付変換できない場合、nullで返す
            DateTime? date = ComUtil.ConvertDateTimeFromYyyymmddString(accountYears + "01");
            if (date == null)
            {
                return null;
            }

            // 取得できた場合、対象の月末を返す
            int days = DateTime.DaysInMonth(((DateTime)date).Year, ((DateTime)date).Month);
            DateTime newDate = new DateTime(((DateTime)date).Year, ((DateTime)date).Month, days);

            return ComUtil.ConvertDatetimeToFmtString(newDate, "yyyy/MM/dd");
        }

        /// <summary>
        /// 月次締処理チェック
        /// </summary>
        /// <param name="list">画面情報 ※this.resultInfoDictionaryをそのまま設定する想定</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="form">機能自身</param>
        /// <param name="msgResources">リソースファイル</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool CheckMonthlyClose(List<Dictionary<string, object>> list, ComDB db, ComBusBase form, ComUtil.MessageResources msgResources)
        {
            // 月次締処理制御を実施する
            // 月次締処理制御用データを取得
            // SQLを生成
            string sql = string.Empty;
            sql += "select * ";
            sql += "  from monthly_close_control ";
            sql += " where conductid = @Conductid ";
            sql += "   and formno = @Formno ";

            // SQLを実行
            IList<EntityDao.MonthlyCloseControlEntity> mccList = db.GetListByDataClass<EntityDao.MonthlyCloseControlEntity>(sql,
                new { Conductid = form.ConductId, Formno = form.FormNo });

            // 締処理制御データが存在しない場合、処理を戻す
            if (mccList == null || mccList.Count == 0)
            {
                return true;
            }

            string accountYear = string.Empty;
            var errorInfoDictionary = new List<Dictionary<string, object>>(); // エラー情報
            Dictionary<int, string> monthlyCloseDate = new Dictionary<int, string>();  // 締処理区分別最大勘定年月退避用
            Dictionary<string, bool> processedCtrlid = new Dictionary<string, bool>(); // 処理済コンロトールID退避用
            foreach (var mcc in mccList)
            {
                // 対象のコントロールIdが処理済の場合、処理をスキップ
                if (processedCtrlid.ContainsKey(mcc.Ctrlid))
                {
                    continue;
                }
                processedCtrlid.Add(mcc.Ctrlid, true); // 次回以降、該当のコントロールIdはスキップを行う

                // 画面情報から対象のコントロールIdのデータを絞り込む
                var dataInfos = GetDictionaryListByCtrlId(list, mcc.Ctrlid);

                // コントロールId別リストを取得
                var tmpDataInfos = mccList.Where(x => x.Ctrlid.Equals(mcc.Ctrlid)).ToList();

                // データ分処理を実施する
                foreach (var dataInfo in dataInfos)
                {
                    // エラー情報
                    var error = new ErrorInfo(dataInfo);
                    var errorFlg = false;

                    // コントロールId分処理を実施する
                    foreach (var tmpDataInfo in tmpDataInfos)
                    {
                        // 締処理区分が未設定の場合、処理をスキップ
                        if (tmpDataInfo.CloseDivision == null)
                        {
                            continue;
                        }

                        // 締処理区分が設定されている場合、チェックを行う
                        accountYear = string.Empty;
                        if (monthlyCloseDate.ContainsKey((int)tmpDataInfo.CloseDivision))
                        {
                            // 既に取得済みの締処理区分の場合、退避データから取得を行う
                            accountYear = monthlyCloseDate[(int)tmpDataInfo.CloseDivision];
                        }
                        else
                        {
                            // 未取得の場合、共通メソッドから締め処理最大勘定年月を取得し、退避しておく
                            accountYear = GetMaxMonthlyAccountYears((int)tmpDataInfo.CloseDivision, db);
                            if (!string.IsNullOrEmpty(accountYear))
                            {
                                // "yyyy/MM/dd 23:59:59"にしておく
                                accountYear += " " + Constants.COMMON.HourTime;
                            }
                            monthlyCloseDate.Add((int)tmpDataInfo.CloseDivision, accountYear);
                        }

                        // 締め処理最大勘定年月が取得できない場合、処理をスキップ
                        if (string.IsNullOrEmpty(accountYear))
                        {
                            continue;
                        }

                        // 比較対象セルを設定
                        var value = "VAL" + tmpDataInfo.Itemno.ToString();

                        // 比較対象セルが存在しない場合、処理をスキップ
                        if (!dataInfo.ContainsKey(value))
                        {
                            continue;
                        }

                        var targetDate = dataInfo[value].ToString();
                        // 比較対象値が未設定の場合、処理をスキップ
                        if (string.IsNullOrEmpty(targetDate))
                        {
                            continue;
                        }

                        // 比較を実施
                        if (ComUtil.CompareDateTime(targetDate, accountYear) != 1)
                        {
                            var closeInfo = "";
                            switch (tmpDataInfo.CloseDivision)
                            {
                                case Constants.MONTH_MANAGEMENT.CLOSE_DIVISION.SALES:
                                    // 販売
                                    closeInfo = APResources.ID.I10271;
                                    break;
                                case Constants.MONTH_MANAGEMENT.CLOSE_DIVISION.PURCHASE:
                                    // 購買
                                    closeInfo = APResources.ID.I10272;
                                    break;
                                case Constants.MONTH_MANAGEMENT.CLOSE_DIVISION.PRODUCTION:
                                    // 製造
                                    closeInfo = APResources.ID.I10269;
                                    break;
                                case Constants.MONTH_MANAGEMENT.CLOSE_DIVISION.INVENTORY_ADJUSTMENT:
                                    // 在庫調整
                                    closeInfo = APResources.ID.I10273;
                                    break;
                                case Constants.MONTH_MANAGEMENT.CLOSE_DIVISION.MONTH_FINAL_CLOSING:
                                    // 月次本締
                                    closeInfo = APResources.ID.I10276;
                                    break;
                                default:
                                    // 到達しない想定
                                    break;
                            }

                            // ○○締処理されているためyyyy/mm/dd以前の日付は入力できません。
                            var errorMsg = getResMessage(new string[] { APResources.ID.M10183, closeInfo, accountYear.Replace(" " + Constants.COMMON.HourTime, "") }, form.LanguageId, msgResources);
                            error.SetError(errorMsg, value);
                            errorFlg = true;
                        }
                    }

                    // エラーの場合、設定を行う
                    if (errorFlg)
                    {
                        errorInfoDictionary.Add(error.Result);
                    }
                }
            }

            // エラーが存在する場合、画面に反映
            if (errorInfoDictionary != null && errorInfoDictionary.Count > 0)
            {
                form.SetJsonResultOpen(errorInfoDictionary);
                form.SetErrorMsg(getResMessage(new string[] { APResources.ID.M10183, APResources.ID.I10276, accountYear.Replace(" " + Constants.COMMON.HourTime, "") }, form.LanguageId, msgResources));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 仕訳送信チェック
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="div">仕訳処理区分</param>
        /// <param name="slipNo">伝票番号</param>
        /// <param name="rowNo">行番号</param>
        /// <returns>true:送信済、false:未送</returns>
        public static bool IsSiwekeLink(ComDB db, int div, string slipNo, int rowNo = 0)
        {
            // SQL文生成
            string sql = string.Empty;

            if (APConstants.APConstants.SIWAKE_MANAGEMENT.DIVISION.SALES.Equals(div))
            {
                // 売上
                sql += "select link_flg from external_if.if_shiwake_sales where sales_no = @SlipNo order by seq desc limit 1";
            }
            else if (APConstants.APConstants.SIWAKE_MANAGEMENT.DIVISION.CREDIT.Equals(div))
            {
                // 入金
                sql += "select link_flg from external_if.if_shiwake_credit where credit_no = @SlipNo and credit_row_no = @RowNo order by seq desc limit 1";
            }
            else if (APConstants.APConstants.SIWAKE_MANAGEMENT.DIVISION.STOCKING.Equals(div))
            {
                // 仕入
                sql += "select link_flg from external_if.if_shiwake_stocking where stocking_no = @SlipNo and stocking_row_no = @RowNo order by seq desc limit 1";
            }
            else if (APConstants.APConstants.SIWAKE_MANAGEMENT.DIVISION.PAYMENT.Equals(div))
            {
                // 支払
                sql += "select link_flg from external_if.if_shiwake_payment where payment_slip_no = @SlipNo and payment_slip_row_no = @RowNo order by seq desc limit 1";
            }
            else if (APConstants.APConstants.SIWAKE_MANAGEMENT.DIVISION.OFFSET.Equals(div))
            {
                // 相殺
                sql += "select link_flg from external_if.if_shiwake_offset_group_data where offset_no = @SlipNo order by seq desc limit 1";
            }
            else if (APConstants.APConstants.SIWAKE_MANAGEMENT.DIVISION.ERASER_CREDIT.Equals(div))
            {
                // 入金消込
                sql += "select link_flg from external_if.if_shiwake_eraser_detail_credit where eraser_no = @SlipNo order by seq desc limit 1";
            }
            else if (APConstants.APConstants.SIWAKE_MANAGEMENT.DIVISION.ERASER_PAYMENT.Equals(div))
            {
                // 支払消込
                sql += "select link_flg from external_if.if_shiwake_eraser_detail_payment where eraser_no = @SlipNo order by seq desc limit 1";
            }

            if (ComUtil.IsNullOrEmpty(sql))
            {
                return false;
            }

            try
            {
                int link_flg = db.GetEntity<int?>(sql, new { SlipNo = slipNo, RowNo = rowNo }) ?? 0;
                return 1.Equals(link_flg); // 1:送信済
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// エラー情報管理クラス
        /// </summary>
        private class ErrorInfo
        {
            /// <summary>
            /// Gets resultInfoDictionaryにセットする、単一の画面項目に対するエラー情報
            /// </summary>
            /// <value>
            /// ResultInfoDictionaryにセットする、単一の画面項目に対するエラー情報
            /// </value>
            public Dictionary<string, object> Result { get; }

            /// <summary>
            /// Gets or Sets 項目でなく、画面に表示するメッセージ
            /// </summary>
            /// <value>
            /// 項目でなく、画面に表示するメッセージ
            /// </value>
            public string FormMessage { get; set; }

            /// <summary>
            /// コンストラクタ(対象の画面項目)
            /// </summary>
            /// <param name="resultInfoDictionary">対象の画面項目</param>
            public ErrorInfo(Dictionary<string, object> resultInfoDictionary)
            {
                // 情報をコピー
                this.Result = new Dictionary<string, object>(resultInfoDictionary);
                // エラー情報を設定
                this.Result["DATATYPE"] = TMPTBL_CONSTANTS.DATATYPE.ErrorDetail;

                // VALnはエラー情報を設定するので不要、削除する
                List<string> keyList = this.Result.Keys.Where(x => x.StartsWith("VAL")).ToList();
                keyList.ForEach(x => this.Result.Remove(x));
            }

            /// <summary>
            /// エラー情報セット
            /// </summary>
            /// <param name="errorMessage">エラーメッセージ</param>
            /// <param name="valId">エラーメッセージをセットする項目ID、resultInfoDictionaryのVALn</param>
            public void SetError(string errorMessage, string valId)
            {
                if (!this.Result.ContainsKey(valId))
                {
                    // VALnが存在しない場合、新規追加
                    this.Result.Add(valId, errorMessage);
                    return;
                }
                // 存在する場合、既存のメッセージに追加
                this.Result[valId] += "\n" + errorMessage;
            }

            /// <summary>
            /// エラー情報セット
            /// </summary>
            /// <param name="errorMessage">エラーメッセージ</param>
            /// <param name="valId">エラーメッセージをセットする項目ID、resultInfoDictionaryのVALn</param>
            /// <param name="flg">0:From、1:To</param>
            public void SetErrorByFromTo(string errorMessage, string valId, int flg)
            {
                // FromTo設定
                string key = flg < 1 ? "From" : "To";
                var dicInfo = new Dictionary<string, object>();

                // 未設定の場合
                if (!this.Result.ContainsKey(valId))
                {
                    dicInfo.Add(key, errorMessage);
                    this.Result.Add(valId, dicInfo);
                }
                else
                {
                    dicInfo = (Dictionary<string, object>)this.Result[valId];
                    if (!dicInfo.ContainsKey(key))
                    {
                        dicInfo.Add(key, errorMessage);
                    }
                    else
                    {
                        dicInfo[key] = dicInfo[key].ToString() + "\n" + errorMessage;
                    }
                    this.Result[valId] = dicInfo;
                }
            }

            /// <summary>
            /// フォーム用エラーメッセージセット
            /// </summary>
            /// <param name="errorMessage">エラーメッセージ</param>
            public void SetFormError(string errorMessage)
            {
                if (ComUtil.IsNullOrEmpty(this.FormMessage))
                {
                    // 新規の場合
                    this.FormMessage = errorMessage;
                    return;
                }
                // TODO 複数行のエラーメッセージ出力は可能？
                // 既にメッセージのある場合、改行して追加
                this.FormMessage += "\n" + errorMessage;
            }
        }
        #endregion

        /// <summary>
        /// PDF出力情報を取得
        /// </summary>
        /// <param name="conductId">呼び出し元の機能ID</param>
        /// <param name="db">db</param>
        /// <returns>汎用マスタから取得したPDF出力情報（実行ファイルパス・実行コマンド・実行要否）</returns>
        public static Dictionary<string, object> GetPdfOutputInfo(string conductId, ComDB db)
        {
            //戻り値のディクショナリ
            Dictionary<string, object> pdfInfo = new Dictionary<string, object> { { "isOutputPdf", false }, { "command", string.Empty }, { "exePath", string.Empty } };

            //PDF出力の要否を取得
            EntityDao.UtilityEntity pdfFlg = new EntityDao.UtilityEntity();
            pdfFlg = pdfFlg.GetEntity("PDF", conductId, "FLG", db);

            //ubuntuに渡すコマンドを取得
            EntityDao.UtilityEntity command = new EntityDao.UtilityEntity();
            command = command.GetEntity("PDF", "PDF", "COMMAND", db);

            //実行ファイルのパスを取得
            EntityDao.UtilityEntity exePath = new EntityDao.UtilityEntity();
            exePath = exePath.GetEntity("PDF", "PDF", "EXEPATH", db);

            //エンティティがnullの場合
            if (pdfFlg == null || command == null || exePath == null)
            {
                return pdfInfo;
            }

            //フラグが1でない・実行コマンド・実行ファイルパスが設定されていない場合
            if (pdfFlg.Name01 != "1" || string.IsNullOrWhiteSpace(command.Name01) || string.IsNullOrWhiteSpace(exePath.Name01))
            {
                return pdfInfo;
            }

            //戻り値に取得したコマンドを設定する・フラグを立てる
            pdfInfo["isOutputPdf"] = true;
            pdfInfo["command"] = command.Name01;
            pdfInfo["exePath"] = exePath.Name01;

            return pdfInfo;
        }

        /// <summary>
        /// 機能名称を取得
        /// </summary>
        /// <param name="conductId">機能ID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>機能名称</returns>
        public static string GetConductName(string conductId, string languageId, ComDB db)
        {
            string conductName = string.Empty;

            // 機能IDをもとに機能ﾏｽﾀから名称を取得
            string sql = string.Empty;
            sql += "select ryaku from com_conduct_mst where conductid = @ConductId";

            conductName = db.GetEntity<string>(sql, new { ConductId = conductId });
            if (string.IsNullOrEmpty(conductName))
            {
                // 機能名が取得できない場合、機能IDを返す
                return conductId;
            }

            // ★多言語対応
            sql = string.Empty; // 初期化
            sql += "select translation_value from com_translation where translation_code = @TranslationCode and language_code = @LanguageCode";

            string conductNameEx = db.GetEntity<string>(sql, new { TranslationCode = conductName, LanguageCode = languageId });
            if (string.IsNullOrEmpty(conductNameEx))
            {
                // 翻訳名称が取得できない場合、機能ﾏｽﾀの名称を戻す
                return conductName;
            }

            return conductNameEx;
        }
    }
}
