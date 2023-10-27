using System;
using System.Collections.Generic;

using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonAPUtil.APCommonUtil
{
    /// <summary>
    /// テーブル定義書より作成した、各テーブルのデータクラス
    /// </summary>
    /// <remarks>原則として手による変更禁止、既定の手順で生成すること</remarks>
    public class APCommonDataClass : APCommonBaseClass
    {
        /// <summary>
        /// 勘定科目マスタ
        /// </summary>
        public class AccountsEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public AccountsEntity()
            {
                TableName = "accounts";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 勘定コード表区分</summary>
            /// <value>勘定コード表区分</value>
            public string AccountsDivision { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string AccountsCd { get; set; }
            /// <summary>Gets or sets 勘定科目名称</summary>
            /// <value>勘定科目名称</value>
            public string AccountsName { get; set; }
            /// <summary>Gets or sets 課税区分</summary>
            /// <value>課税区分</value>
            public string TaxationDivision { get; set; }
            /// <summary>Gets or sets 仕入科目区分|各種名称マスタ</summary>
            /// <value>仕入科目区分|各種名称マスタ</value>
            public int? PurchaseAccounts { get; set; }
            /// <summary>Gets or sets 売上科目区分|各種名称マスタ</summary>
            /// <value>売上科目区分|各種名称マスタ</value>
            public int? ArticleAccounts { get; set; }
            /// <summary>Gets or sets 税カテゴリ</summary>
            /// <value>税カテゴリ</value>
            public string TaxCategory { get; set; }
            /// <summary>Gets or sets 総勘定元帳勘定グループ</summary>
            /// <value>総勘定元帳勘定グループ</value>
            public string AccountsGroup { get; set; }
            /// <summary>Gets or sets 貸借対照表勘定フラグ</summary>
            /// <value>貸借対照表勘定フラグ</value>
            public int? BalanceAccountsFlg { get; set; }
            /// <summary>Gets or sets 取引先会社ID</summary>
            /// <value>取引先会社ID</value>
            public string VenderId { get; set; }
            /// <summary>Gets or sets グループ勘定コード</summary>
            /// <value>グループ勘定コード</value>
            public string GroupAccountsCd { get; set; }
            /// <summary>Gets or sets 勘定コード長</summary>
            /// <value>勘定コード長</value>
            public string AccountsLength { get; set; }
            /// <summary>Gets or sets 損益計算書勘定タイプ</summary>
            /// <value>損益計算書勘定タイプ</value>
            public string PlAccountsType { get; set; }
            /// <summary>Gets or sets 勘定削除フラグ</summary>
            /// <value>勘定削除フラグ</value>
            public string AccountsDelFlg { get; set; }
            /// <summary>Gets or sets 登録ブロックフラグ</summary>
            /// <value>登録ブロックフラグ</value>
            public string RegistBlockFlg { get; set; }
            /// <summary>Gets or sets 転記ブロックフラグ</summary>
            /// <value>転記ブロックフラグ</value>
            public string PostingBlockFlg { get; set; }
            /// <summary>Gets or sets 計画ブロックフラグ</summary>
            /// <value>計画ブロックフラグ</value>
            public string PlanBlockFlg { get; set; }
            /// <summary>Gets or sets 機能領域</summary>
            /// <value>機能領域</value>
            public string FuncArea { get; set; }
            /// <summary>Gets or sets 取引先必須フラグ</summary>
            /// <value>取引先必須フラグ</value>
            public string VenderRequiredFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 勘定コード表区分</summary>
                /// <value>勘定コード表区分</value>
                public string AccountsDivision { get; set; }
                /// <summary>Gets or sets 勘定科目コード</summary>
                /// <value>勘定科目コード</value>
                public string AccountsCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="accountsDivision">勘定コード表区分</param>
                /// <param name="accountsCd">勘定科目コード</param>
                public PrimaryKey(string accountsDivision, string accountsCd)
                {
                    AccountsDivision = accountsDivision;
                    AccountsCd = accountsCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.AccountsDivision, this.AccountsCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            /// <param name="accountsDivision">勘定コード表区分</param>
            /// <param name="accountsCd">勘定科目コード</param>
            /// <param name="db">DB操作クラス</param>
            public AccountsEntity GetEntity(string accountsDivision, string accountsCd, ComDB db)
            {
                AccountsEntity.PrimaryKey condition = new AccountsEntity.PrimaryKey(accountsDivision, accountsCd);
                return GetEntity<AccountsEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 勘定科目税率マスタ
        /// </summary>
        public class AccountsTaxEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public AccountsTaxEntity()
            {
                TableName = "accounts_tax";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string AccountsCd { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime ValidDate { get; set; }
            /// <summary>Gets or sets 用途</summary>
            /// <value>用途</value>
            public string Category { get; set; }
            /// <summary>Gets or sets 税コード</summary>
            /// <value>税コード</value>
            public string TaxCd { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 勘定科目コード</summary>
                /// <value>勘定科目コード</value>
                public string AccountsCd { get; set; }
                /// <summary>Gets or sets 有効開始日</summary>
                /// <value>有効開始日</value>
                public DateTime ValidDate { get; set; }
                /// <summary>Gets or sets 用途</summary>
                /// <value>用途</value>
                public string Category { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="accountsCd">勘定科目コード</param>
                /// <param name="validDate">有効開始日</param>
                /// <param name="category">用途</param>
                public PrimaryKey(string accountsCd, DateTime validDate, string category)
                {
                    AccountsCd = accountsCd;
                    ValidDate = validDate;
                    Category = category;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.AccountsCd, this.ValidDate, this.Category);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="accountsCd">勘定科目コード</param>
            /// <param name="validDate">有効開始日</param>
            /// <param name="category">用途</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public AccountsTaxEntity GetEntity(string accountsCd, DateTime validDate, string category, ComDB db)
            {
                AccountsTaxEntity.PrimaryKey condition = new AccountsTaxEntity.PrimaryKey(accountsCd, validDate, category);
                return GetEntity<AccountsTaxEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 帳合マスタ
        /// </summary>
        public class BalanceEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public BalanceEntity()
            {
                TableName = "balance";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 帳合コード</summary>
            /// <value>帳合コード</value>
            public string BalanceCd { get; set; }
            /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
            /// <value>取引先区分|各種名称マスタ</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 上位帳合コード</summary>
            /// <value>上位帳合コード</value>
            public string UpperBalanceCd { get; set; }
            /// <summary>Gets or sets 次店</summary>
            /// <value>次店</value>
            public int? ShopLevel { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 帳合コード</summary>
                /// <value>帳合コード</value>
                public string BalanceCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="balanceCd">帳合コード</param>
                public PrimaryKey(string balanceCd)
                {
                    BalanceCd = balanceCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.BalanceCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="balanceCd">帳合コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public BalanceEntity GetEntity(string balanceCd, ComDB db)
            {
                BalanceEntity.PrimaryKey condition = new BalanceEntity.PrimaryKey(balanceCd);
                return GetEntity<BalanceEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 銀行マスタ
        /// </summary>
        public class BankEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public BankEntity()
            {
                TableName = "bank";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 銀行カナ名称</summary>
            /// <value>銀行カナ名称</value>
            public string BankKanaName { get; set; }
            /// <summary>Gets or sets 銀行名称</summary>
            /// <value>銀行名称</value>
            public string BankName { get; set; }
            /// <summary>Gets or sets 支店コード</summary>
            /// <value>支店コード</value>
            public string BranchCd { get; set; }
            /// <summary>Gets or sets 支店カナ名称</summary>
            /// <value>支店カナ名称</value>
            public string BranchKanaName { get; set; }
            /// <summary>Gets or sets 支店名称</summary>
            /// <value>支店名称</value>
            public string BranchName { get; set; }
            /// <summary>Gets or sets 預金種別</summary>
            /// <value>預金種別</value>
            public string DepositType { get; set; }
            /// <summary>Gets or sets 預金勘定</summary>
            /// <value>預金勘定</value>
            public string DepositAccount { get; set; }
            /// <summary>Gets or sets MOコード</summary>
            /// <value>MOコード</value>
            public string MoCd { get; set; }
            /// <summary>Gets or sets カレンダーコード</summary>
            /// <value>カレンダーコード</value>
            public string CalCd { get; set; }
            /// <summary>Gets or sets 振込手数料</summary>
            /// <value>振込手数料</value>
            public decimal? Fee { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 銀行コード</summary>
                /// <value>銀行コード</value>
                public string BankCd { get; set; }
                /// <summary>Gets or sets 支店コード</summary>
                /// <value>支店コード</value>
                public string BranchCd { get; set; }
                /// <summary>Gets or sets 預金種別</summary>
                /// <value>預金種別</value>
                public string DepositType { get; set; }
                /// <summary>Gets or sets 預金勘定</summary>
                /// <value>預金勘定</value>
                public string DepositAccount { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="bankCd">銀行コード</param>
                /// <param name="branchCd">支店コード</param>
                /// <param name="depositType">預金種別</param>
                /// <param name="depositAccount">預金勘定</param>
                public PrimaryKey(string bankCd, string branchCd, string depositType, string depositAccount)
                {
                    BankCd = bankCd;
                    BranchCd = branchCd;
                    DepositType = depositType;
                    DepositAccount = depositAccount;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.BankCd, this.BranchCd, this.DepositType, this.DepositAccount);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="bankCd">銀行コード</param>
            /// <param name="branchCd">支店コード</param>
            /// <param name="depositType">預金種別</param>
            /// <param name="depositAccount">預金勘定</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public BankEntity GetEntity(string bankCd, string branchCd, string depositType, string depositAccount, ComDB db)
            {
                BankEntity.PrimaryKey condition = new BankEntity.PrimaryKey(bankCd, branchCd, depositType, depositAccount);
                return GetEntity<BankEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 所属マスタ
        /// </summary>
        public class BelongEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public BelongEntity()
            {
                TableName = "belong";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 役職コード</summary>
            /// <value>役職コード</value>
            public int PostId { get; set; }
            /// <summary>Gets or sets 所属区分|各種名称マスタ</summary>
            /// <value>所属区分|各種名称マスタ</value>
            public string BelongDivision { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 部署コード</summary>
                /// <value>部署コード</value>
                public string OrganizationCd { get; set; }
                /// <summary>Gets or sets 担当者コード</summary>
                /// <value>担当者コード</value>
                public string UserId { get; set; }
                /// <summary>Gets or sets 役職コード</summary>
                /// <value>役職コード</value>
                public int PostId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="organizationCd">部署コード</param>
                /// <param name="userId">担当者コード</param>
                /// <param name="postId">役職コード</param>
                public PrimaryKey(string organizationCd, string userId, int postId)
                {
                    OrganizationCd = organizationCd;
                    UserId = userId;
                    PostId = postId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OrganizationCd, this.UserId, this.PostId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="organizationCd">部署コード</param>
            /// <param name="userId">担当者コード</param>
            /// <param name="postId">役職コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public BelongEntity GetEntity(string organizationCd, string userId, int postId, ComDB db)
            {
                BelongEntity.PrimaryKey condition = new BelongEntity.PrimaryKey(organizationCd, userId, postId);
                return GetEntity<BelongEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 所属・ロール組合せマスタ
        /// </summary>
        public class BelongRoleEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public BelongRoleEntity()
            {
                TableName = "belong_role";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 役職コード</summary>
            /// <value>役職コード</value>
            public int PostId { get; set; }
            /// <summary>Gets or sets ロールID</summary>
            /// <value>ロールID</value>
            public int RoleId { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 部署コード</summary>
                /// <value>部署コード</value>
                public string OrganizationCd { get; set; }
                /// <summary>Gets or sets 役職コード</summary>
                /// <value>役職コード</value>
                public int PostId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="organizationCd">部署コード</param>
                /// <param name="postId">役職コード</param>
                public PrimaryKey(string organizationCd, int postId)
                {
                    OrganizationCd = organizationCd;
                    PostId = postId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OrganizationCd, this.PostId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="organizationCd">部署コード</param>
            /// <param name="postId">役職コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public BelongRoleEntity GetEntity(string organizationCd, int postId, ComDB db)
            {
                BelongRoleEntity.PrimaryKey condition = new BelongRoleEntity.PrimaryKey(organizationCd, postId);
                return GetEntity<BelongRoleEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 会計部門マスタ
        /// </summary>
        public class BumonEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public BumonEntity()
            {
                TableName = "bumon";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 会計部門コード</summary>
            /// <value>会計部門コード</value>
            public string SectionCd { get; set; }
            /// <summary>Gets or sets 会計部門名称</summary>
            /// <value>会計部門名称</value>
            public string SectionName { get; set; }
            /// <summary>Gets or sets 会計部門略称</summary>
            /// <value>会計部門略称</value>
            public string SectionShortedName { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 会計部門コード</summary>
                /// <value>会計部門コード</value>
                public string SectionCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="sectionCd">会計部門コード</param>
                public PrimaryKey(string sectionCd)
                {
                    SectionCd = sectionCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SectionCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="sectionCd">会計部門コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public BumonEntity GetEntity(string sectionCd, ComDB db)
            {
                BumonEntity.PrimaryKey condition = new BumonEntity.PrimaryKey(sectionCd);
                return GetEntity<BumonEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// カレンダーマスタ
        /// </summary>
        public class CalEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CalEntity()
            {
                TableName = "cal";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets カレンダーコード</summary>
            /// <value>カレンダーコード</value>
            public string CalCd { get; set; }
            /// <summary>Gets or sets カレンダー用途名</summary>
            /// <value>カレンダー用途名</value>
            public string CalName { get; set; }
            /// <summary>Gets or sets 会計年度</summary>
            /// <value>会計年度</value>
            public int CalYear { get; set; }
            /// <summary>Gets or sets 年月日</summary>
            /// <value>年月日</value>
            public DateTime CalDate { get; set; }
            /// <summary>Gets or sets 曜日|0</summary>
            /// <value>曜日|0</value>
            public int? CalWeek { get; set; }
            /// <summary>Gets or sets 休日区分|0</summary>
            /// <value>休日区分|0</value>
            public int? CalHoliday { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets カレンダーコード</summary>
                /// <value>カレンダーコード</value>
                public string CalCd { get; set; }
                /// <summary>Gets or sets 年月日</summary>
                /// <value>年月日</value>
                public DateTime CalDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="calCd">カレンダーコード</param>
                /// <param name="calDate">年月日</param>
                public PrimaryKey(string calCd, DateTime calDate)
                {
                    CalCd = calCd;
                    CalDate = calDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.CalCd, this.CalDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="calCd">カレンダーコード</param>
            /// <param name="calDate">年月日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public CalEntity GetEntity(string calCd, DateTime calDate, ComDB db)
            {
                CalEntity.PrimaryKey condition = new CalEntity.PrimaryKey(calCd, calDate);
                return GetEntity<CalEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 運送会社マスタ
        /// </summary>
        public class CarryEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CarryEntity()
            {
                TableName = "carry";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 運送会社コード</summary>
            /// <value>運送会社コード</value>
            public string CarryCd { get; set; }
            /// <summary>Gets or sets 運送会社名称1</summary>
            /// <value>運送会社名称1</value>
            public string CarryName1 { get; set; }
            /// <summary>Gets or sets 運送会社名称2</summary>
            /// <value>運送会社名称2</value>
            public string CarryName2 { get; set; }
            /// <summary>Gets or sets 運送会社略称</summary>
            /// <value>運送会社略称</value>
            public string Abbreviation { get; set; }
            /// <summary>Gets or sets 郵便番号</summary>
            /// <value>郵便番号</value>
            public string Zipcode { get; set; }
            /// <summary>Gets or sets 運送会社担当者名</summary>
            /// <value>運送会社担当者名</value>
            public string UserName { get; set; }
            /// <summary>Gets or sets 電話番号</summary>
            /// <value>電話番号</value>
            public string TelNo { get; set; }
            /// <summary>Gets or sets FAX番号</summary>
            /// <value>FAX番号</value>
            public string FaxNo { get; set; }
            /// <summary>Gets or sets 住所1</summary>
            /// <value>住所1</value>
            public string Address1 { get; set; }
            /// <summary>Gets or sets 住所2</summary>
            /// <value>住所2</value>
            public string Address2 { get; set; }
            /// <summary>Gets or sets 住所3</summary>
            /// <value>住所3</value>
            public string Address3 { get; set; }
            /// <summary>Gets or sets mailアドレス</summary>
            /// <value>mailアドレス</value>
            public string Mail { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 運送会社コード</summary>
                /// <value>運送会社コード</value>
                public string CarryCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="carryCd">運送会社コード</param>
                public PrimaryKey(string carryCd)
                {
                    CarryCd = carryCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.CarryCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="carryCd">運送会社コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public CarryEntity GetEntity(string carryCd, ComDB db)
            {
                CarryEntity.PrimaryKey condition = new CarryEntity.PrimaryKey(carryCd);
                return GetEntity<CarryEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 更新履歴_メニュー別(品目仕様マスタで使用)
        /// </summary>
        public class ChangeHistoryEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ChangeHistoryEntity()
            {
                TableName = "change_history";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets メニューID</summary>
            /// <value>メニューID</value>
            public long MenuId { get; set; }
            /// <summary>Gets or sets 主コード(品目)</summary>
            /// <value>主コード(品目)</value>
            public string MainCd { get; set; }
            /// <summary>Gets or sets 副コード(仕様)</summary>
            /// <value>副コード(仕様)</value>
            public string SubCd { get; set; }
            /// <summary>Gets or sets 理由</summary>
            /// <value>理由</value>
            public string Reason { get; set; }
            /// <summary>Gets or sets バージョン(未使用)</summary>
            /// <value>バージョン(未使用)</value>
            public long? Version { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets メニューID</summary>
                /// <value>メニューID</value>
                public long MenuId { get; set; }
                /// <summary>Gets or sets 主コード(品目)</summary>
                /// <value>主コード(品目)</value>
                public string MainCd { get; set; }
                /// <summary>Gets or sets 副コード(仕様)</summary>
                /// <value>副コード(仕様)</value>
                public string SubCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="menuId">メニューID</param>
                /// <param name="mainCd">主コード(品目)</param>
                /// <param name="subCd">副コード(仕様)</param>
                public PrimaryKey(long menuId, string mainCd, string subCd)
                {
                    MenuId = menuId;
                    MainCd = mainCd;
                    SubCd = subCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MenuId, this.MainCd, this.SubCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="menuId">メニューID</param>
            /// <param name="mainCd">主コード(品目)</param>
            /// <param name="subCd">副コード(仕様)</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ChangeHistoryEntity GetEntity(long menuId, string mainCd, string subCd, ComDB db)
            {
                ChangeHistoryEntity.PrimaryKey condition = new ChangeHistoryEntity.PrimaryKey(menuId, mainCd, subCd);
                return GetEntity<ChangeHistoryEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 請求ヘッダ
        /// </summary>
        public class ClaimHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClaimHeaderEntity()
            {
                TableName = "claim_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime ClaimDate { get; set; }
            /// <summary>Gets or sets 入金予定日</summary>
            /// <value>入金予定日</value>
            public DateTime CreditScheduledDate { get; set; }
            /// <summary>Gets or sets 決済方法</summary>
            /// <value>決済方法</value>
            public string PaymentMethod { get; set; }
            /// <summary>Gets or sets 手形サイト</summary>
            /// <value>手形サイト</value>
            public int? NoteSight { get; set; }
            /// <summary>Gets or sets 休日指定フラグ</summary>
            /// <value>休日指定フラグ</value>
            public int? HolidayFlg { get; set; }
            /// <summary>Gets or sets 前月請求残高</summary>
            /// <value>前月請求残高</value>
            public decimal? ClaimAmountForward { get; set; }
            /// <summary>Gets or sets 入金額</summary>
            /// <value>入金額</value>
            public decimal? CreditAmountForward { get; set; }
            /// <summary>Gets or sets その他入金額</summary>
            /// <value>その他入金額</value>
            public decimal? OtherCreditAmountForward { get; set; }
            /// <summary>Gets or sets 売上金額</summary>
            /// <value>売上金額</value>
            public decimal? SalesAmount { get; set; }
            /// <summary>Gets or sets 返品金額</summary>
            /// <value>返品金額</value>
            public decimal? SalesReturnedAmount { get; set; }
            /// <summary>Gets or sets 値引金額</summary>
            /// <value>値引金額</value>
            public decimal? SalesDiscountAmount { get; set; }
            /// <summary>Gets or sets その他売上金額</summary>
            /// <value>その他売上金額</value>
            public decimal? OtherSalesAmount { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 当月請求残高</summary>
            /// <value>当月請求残高</value>
            public decimal? ClaimAmount { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal? EraserBalanceAmount { get; set; }
            /// <summary>Gets or sets 請求書発行済フラグ</summary>
            /// <value>請求書発行済フラグ</value>
            public int BillDivision { get; set; }
            /// <summary>Gets or sets 発行日時</summary>
            /// <value>発行日時</value>
            public DateTime? IssueDate { get; set; }
            /// <summary>Gets or sets 発行者ID</summary>
            /// <value>発行者ID</value>
            public string IssuerCd { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 請求番号</summary>
                /// <value>請求番号</value>
                public string ClaimNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="claimNo">請求番号</param>
                public PrimaryKey(string claimNo)
                {
                    ClaimNo = claimNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ClaimNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="claimNo">請求番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ClaimHeaderEntity GetEntity(string claimNo, ComDB db)
            {
                ClaimHeaderEntity.PrimaryKey condition = new ClaimHeaderEntity.PrimaryKey(claimNo);
                return GetEntity<ClaimHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 請求ヘッダ発行履歴
        /// </summary>
        public class ClaimHeaderRecordEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClaimHeaderRecordEntity()
            {
                TableName = "claim_header_record";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime ClaimDate { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public DateTime? ClaimNo { get; set; }
            /// <summary>Gets or sets 発行日時</summary>
            /// <value>発行日時</value>
            public DateTime RecordDate { get; set; }
            /// <summary>Gets or sets 発行者ID</summary>
            /// <value>発行者ID</value>
            public string RecordUserId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 取引先区分</summary>
                /// <value>取引先区分</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 請求先コード</summary>
                /// <value>請求先コード</value>
                public string InvoiceCd { get; set; }
                /// <summary>Gets or sets 取引先開始有効日</summary>
                /// <value>取引先開始有効日</value>
                public DateTime VenderActiveDate { get; set; }
                /// <summary>Gets or sets 請求締め日</summary>
                /// <value>請求締め日</value>
                public DateTime ClaimDate { get; set; }
                /// <summary>Gets or sets 発行日時</summary>
                /// <value>発行日時</value>
                public DateTime RecordDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="invoiceCd">請求先コード</param>
                /// <param name="venderActiveDate">取引先開始有効日</param>
                /// <param name="claimDate">請求締め日</param>
                /// <param name="recordDate">発行日時</param>
                public PrimaryKey(string venderDivision, string invoiceCd, DateTime venderActiveDate, DateTime claimDate, DateTime recordDate)
                {
                    VenderDivision = venderDivision;
                    InvoiceCd = invoiceCd;
                    VenderActiveDate = venderActiveDate;
                    ClaimDate = claimDate;
                    RecordDate = recordDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.VenderDivision, this.InvoiceCd, this.VenderActiveDate, this.ClaimDate, this.RecordDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="invoiceCd">請求先コード</param>
            /// <param name="venderActiveDate">取引先開始有効日</param>
            /// <param name="claimDate">請求締め日</param>
            /// <param name="recordDate">発行日時</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ClaimHeaderRecordEntity GetEntity(string venderDivision, string invoiceCd, DateTime venderActiveDate, DateTime claimDate, DateTime recordDate, ComDB db)
            {
                ClaimHeaderRecordEntity.PrimaryKey condition = new ClaimHeaderRecordEntity.PrimaryKey(venderDivision, invoiceCd, venderActiveDate, claimDate, recordDate);
                return GetEntity<ClaimHeaderRecordEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 会計用設定マスタ
        /// </summary>
        public class ClassificationEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassificationEntity()
            {
                TableName = "classification";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets データ種別|1</summary>
            /// <value>データ種別|1</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets 分類コード（分類マスタ参照）</summary>
            /// <value>分類コード（分類マスタ参照）</value>
            public int CategoryDivision { get; set; }
            /// <summary>Gets or sets データ集計区分（分類マスタ参照）</summary>
            /// <value>データ集計区分（分類マスタ参照）</value>
            public int DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類名称</summary>
            /// <value>分類名称</value>
            public string CategoryName { get; set; }
            /// <summary>Gets or sets 対外分類名称</summary>
            /// <value>対外分類名称</value>
            public string ExternalCategoryName { get; set; }
            /// <summary>Gets or sets 借方勘定科目コード</summary>
            /// <value>借方勘定科目コード</value>
            public string DebitAccountsCd { get; set; }
            /// <summary>Gets or sets 借方補助科目コード</summary>
            /// <value>借方補助科目コード</value>
            public string DebitAccountsSubCd { get; set; }
            /// <summary>Gets or sets 貸方勘定科目コード</summary>
            /// <value>貸方勘定科目コード</value>
            public string CreditAccountsCd { get; set; }
            /// <summary>Gets or sets 貸方補助科目コード</summary>
            /// <value>貸方補助科目コード</value>
            public string CreditAccountsSubCd { get; set; }
            /// <summary>Gets or sets 売掛対象区分|0</summary>
            /// <value>売掛対象区分|0</value>
            public int? ArDivision { get; set; }
            /// <summary>Gets or sets 請求対象区分|0</summary>
            /// <value>請求対象区分|0</value>
            public int? ClaimDivision { get; set; }
            /// <summary>Gets or sets 買掛対象区分|0</summary>
            /// <value>買掛対象区分|0</value>
            public int? CreditDivision { get; set; }
            /// <summary>Gets or sets 支払対象区分|0</summary>
            /// <value>支払対象区分|0</value>
            public int? PaymentDivision { get; set; }
            /// <summary>Gets or sets 銀行必須区分|0</summary>
            /// <value>銀行必須区分|0</value>
            public int? BankDivision { get; set; }
            /// <summary>Gets or sets 手形必須区分|0</summary>
            /// <value>手形必須区分|0</value>
            public int? NoteDivision { get; set; }
            /// <summary>Gets or sets +-区分 -1</summary>
            /// <value>+-区分 -1</value>
            public int? PlusMinusDivision { get; set; }
            /// <summary>Gets or sets フリー区分1 別途追加する場合、使用する。実際使用する場合は管理者に連絡し使用する。</summary>
            /// <value>フリー区分1 別途追加する場合、使用する。実際使用する場合は管理者に連絡し使用する。</value>
            public string FreeDivision1 { get; set; }
            /// <summary>Gets or sets フリー区分2 別途追加する場合、使用する。実際使用する場合は管理者に連絡し使用する。</summary>
            /// <value>フリー区分2 別途追加する場合、使用する。実際使用する場合は管理者に連絡し使用する。</value>
            public string FreeDivision2 { get; set; }
            /// <summary>Gets or sets フリー区分3 別途追加する場合、使用する。実際使用する場合は管理者に連絡し使用する。</summary>
            /// <value>フリー区分3 別途追加する場合、使用する。実際使用する場合は管理者に連絡し使用する。</value>
            public string FreeDivision3 { get; set; }
            /// <summary>Gets or sets フリー区分4 別途追加する場合、使用する。実際使用する場合は管理者に連絡し使用する。</summary>
            /// <value>フリー区分4 別途追加する場合、使用する。実際使用する場合は管理者に連絡し使用する。</value>
            public string FreeDivision4 { get; set; }
            /// <summary>Gets or sets フリー区分5 別途追加する場合、使用する。実際使用する場合は管理者に連絡し使用する。</summary>
            /// <value>フリー区分5 別途追加する場合、使用する。実際使用する場合は管理者に連絡し使用する。</value>
            public string FreeDivision5 { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }
        }

        /// <summary>
        /// 締処理設定
        /// </summary>
        public class CloseConfigEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CloseConfigEntity()
            {
                TableName = "close_config";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 実行部署>棚卸区分</summary>
            /// <value>実行部署>棚卸区分</value>
            public string FactoryDivision { get; set; }
            /// <summary>Gets or sets 循環棚卸区分</summary>
            /// <value>循環棚卸区分</value>
            public string CountDivision { get; set; }
            /// <summary>Gets or sets 勘定年月 </summary>
            /// <value>勘定年月 </value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 締処理区分</summary>
            /// <value>締処理区分</value>
            public int CloseDivision { get; set; }
            /// <summary>Gets or sets 締処理予定日時</summary>
            /// <value>締処理予定日時</value>
            public DateTime ExecDate { get; set; }
            /// <summary>Gets or sets 締処理対象日付(開始) </summary>
            /// <value>締処理対象日付(開始) </value>
            public int? CloseDateFrom { get; set; }
            /// <summary>Gets or sets 締処理対象日付(終了) </summary>
            /// <value>締処理対象日付(終了) </value>
            public int? CloseDateTo { get; set; }
            /// <summary>Gets or sets 締処理済みフラグ</summary>
            /// <value>締処理済みフラグ</value>
            public int CloseFlg { get; set; }
            /// <summary>Gets or sets 実行開始予定日時</summary>
            /// <value>実行開始予定日時</value>
            public DateTime? ExecPlanDate { get; set; }
            /// <summary>Gets or sets 実行開始日時</summary>
            /// <value>実行開始日時</value>
            public DateTime? ExecStartDate { get; set; }
            /// <summary>Gets or sets 実行完了日時</summary>
            /// <value>実行完了日時</value>
            public DateTime? ExecEndDate { get; set; }
            /// <summary>Gets or sets 実行担当者</summary>
            /// <value>実行担当者</value>
            public string ExecUserId { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            //public ? Status { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 実行部署>棚卸区分</summary>
                /// <value>実行部署>棚卸区分</value>
                public string FactoryDivision { get; set; }
                /// <summary>Gets or sets 循環棚卸区分</summary>
                /// <value>循環棚卸区分</value>
                public string CountDivision { get; set; }
                /// <summary>Gets or sets 勘定年月 </summary>
                /// <value>勘定年月 </value>
                public string AccountYears { get; set; }
                /// <summary>Gets or sets 締処理区分</summary>
                /// <value>締処理区分</value>
                public int CloseDivision { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="factoryDivision">実行部署>棚卸区分</param>
                /// <param name="countDivision">循環棚卸区分</param>
                /// <param name="accountYears">勘定年月</param>
                /// <param name="closeDivision">締処理区分</param>
                public PrimaryKey(string factoryDivision, string countDivision, string accountYears, int closeDivision)
                {
                    FactoryDivision = factoryDivision;
                    CountDivision = countDivision;
                    AccountYears = accountYears;
                    CloseDivision = closeDivision;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.FactoryDivision, this.CountDivision, this.AccountYears, this.CloseDivision);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="factoryDivision">実行部署>棚卸区分</param>
            /// <param name="countDivision">循環棚卸区分</param>
            /// <param name="accountYears">勘定年月</param>
            /// <param name="closeDivision">締処理区分</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public CloseConfigEntity GetEntity(string factoryDivision, string countDivision, string accountYears, int closeDivision, ComDB db)
            {
                CloseConfigEntity.PrimaryKey condition = new CloseConfigEntity.PrimaryKey(factoryDivision, countDivision, accountYears, closeDivision);
                return GetEntity<CloseConfigEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 締め処理状態保存用テーブル
        /// </summary>
        public class CloseResultEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CloseResultEntity()
            {
                TableName = "close_result";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 締処理区分</summary>
            /// <value>締処理区分</value>
            public string Division { get; set; }
            /// <summary>Gets or sets 締年月</summary>
            /// <value>締年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 締処理日付</summary>
            /// <value>締処理日付</value>
            public DateTime? ExecDate { get; set; }
            /// <summary>Gets or sets 締処理開始日時</summary>
            /// <value>締処理開始日時</value>
            public DateTime? CloseDateFrom { get; set; }
            /// <summary>Gets or sets 締処理終了日時</summary>
            /// <value>締処理終了日時</value>
            public DateTime? CloseDateTo { get; set; }
            /// <summary>Gets or sets 締処理済みフラグ|0</summary>
            /// <value>締処理済みフラグ|0</value>
            public int? CloseFlg { get; set; }
            /// <summary>Gets or sets 拡張文字列01</summary>
            /// <value>拡張文字列01</value>
            public string ExString01 { get; set; }
            /// <summary>Gets or sets 拡張文字列02</summary>
            /// <value>拡張文字列02</value>
            public string ExString02 { get; set; }
            /// <summary>Gets or sets 拡張文字列03</summary>
            /// <value>拡張文字列03</value>
            public string ExString03 { get; set; }
            /// <summary>Gets or sets 拡張数値01</summary>
            /// <value>拡張数値01</value>
            public decimal? ExNumeric01 { get; set; }
            /// <summary>Gets or sets 拡張数値02</summary>
            /// <value>拡張数値02</value>
            public decimal? ExNumeric02 { get; set; }
            /// <summary>Gets or sets 拡張数値03</summary>
            /// <value>拡張数値03</value>
            public decimal? ExNumeric03 { get; set; }
            /// <summary>Gets or sets 拡張日付01</summary>
            /// <value>拡張日付01</value>
            public DateTime? ExDate01 { get; set; }
            /// <summary>Gets or sets 拡張日付02</summary>
            /// <value>拡張日付02</value>
            public DateTime? ExDate02 { get; set; }
            /// <summary>Gets or sets 拡張日付03</summary>
            /// <value>拡張日付03</value>
            public DateTime? ExDate03 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 締処理区分</summary>
                /// <value>締処理区分</value>
                public string Division { get; set; }
                /// <summary>Gets or sets 締年月</summary>
                /// <value>締年月</value>
                public string AccountYears { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="division">締処理区分</param>
                /// <param name="accountYears">締年月</param>
                public PrimaryKey(string division, string accountYears)
                {
                    Division = division;
                    AccountYears = accountYears;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.Division, this.AccountYears);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="division">締処理区分</param>
            /// <param name="accountYears">締年月</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public CloseResultEntity GetEntity(string division, string accountYears, ComDB db)
            {
                CloseResultEntity.PrimaryKey condition = new CloseResultEntity.PrimaryKey(division, accountYears);
                return GetEntity<CloseResultEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 月次処理管理
        /// </summary>
        public class MonthManagementEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MonthManagementEntity()
            {
                TableName = "month_management";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 締処理区分</summary>
            /// <value>締処理区分</value>
            public int CloseDivision { get; set; }
            /// <summary>Gets or sets 実行開始予定日時</summary>
            /// <value>実行開始予定日時</value>
            public DateTime? ExecPlanDate { get; set; }
            /// <summary>Gets or sets 実行開始日時</summary>
            /// <value>実行開始日時</value>
            public DateTime? ExecStartDate { get; set; }
            /// <summary>Gets or sets 実行完了日時</summary>
            /// <value>実行完了日時</value>
            public DateTime? ExecEndDate { get; set; }
            /// <summary>Gets or sets 実行担当者</summary>
            /// <value>実行担当者</value>
            public string ExecUserId { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            //public ? Status { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 勘定年月</summary>
                /// <value>勘定年月</value>
                public string AccountYears { get; set; }
                /// <summary>Gets or sets 締処理区分</summary>
                /// <value>締処理区分</value>
                public int CloseDivision { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="accountYears">勘定年月</param>
                /// <param name="closeDivision">締処理区分</param>
                public PrimaryKey(string accountYears, int closeDivision)
                {
                    AccountYears = accountYears;
                    CloseDivision = closeDivision;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.AccountYears, this.CloseDivision);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="accountYears">勘定年月</param>
            /// <param name="closeDivision">締処理区分</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MonthManagementEntity GetEntity(string accountYears, int closeDivision, ComDB db)
            {
                MonthManagementEntity.PrimaryKey condition = new MonthManagementEntity.PrimaryKey(accountYears, closeDivision);
                return GetEntity<MonthManagementEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// テンプレート保存先フォルダマスタ
        /// </summary>
        public class CommonEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CommonEntity()
            {
                TableName = "common";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets テンプレート区分</summary>
            /// <value>テンプレート区分</value>
            public string CommonCd { get; set; }
            /// <summary>Gets or sets テンプレート種類</summary>
            /// <value>テンプレート種類</value>
            public string CommonName { get; set; }
            /// <summary>Gets or sets テンプレート保存先パス</summary>
            /// <value>テンプレート保存先パス</value>
            public string CommonValue { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets テンプレート区分</summary>
                /// <value>テンプレート区分</value>
                public string CommonCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="commonCd">テンプレート区分</param>
                public PrimaryKey(string commonCd)
                {
                    CommonCd = commonCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.CommonCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="commonCd">テンプレート区分</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public CommonEntity GetEntity(string commonCd, ComDB db)
            {
                CommonEntity.PrimaryKey condition = new CommonEntity.PrimaryKey(commonCd);
                return GetEntity<CommonEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 帳票の発行ステータス情報等を保持するテーブル
        /// </summary>
        public class CommonProcInfoEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CommonProcInfoEntity()
            {
                TableName = "common_proc_info";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 処理名称</summary>
            /// <value>処理名称</value>
            public string ProcName { get; set; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 指図行番号</summary>
            /// <value>指図行番号</value>
            public int RowNo { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 指示日</summary>
            /// <value>指示日</value>
            public DateTime? PlanDate { get; set; }
            /// <summary>Gets or sets 指示担当者コード</summary>
            /// <value>指示担当者コード</value>
            public string PlanUserId { get; set; }
            /// <summary>Gets or sets 実績日</summary>
            /// <value>実績日</value>
            public DateTime? ResultDate { get; set; }
            /// <summary>Gets or sets 実績担当者コード</summary>
            /// <value>実績担当者コード</value>
            public string ResultUserId { get; set; }
            /// <summary>Gets or sets ex_string_01</summary>
            /// <value>ex_string_01</value>
            public string ExString01 { get; set; }
            /// <summary>Gets or sets ex_string_02</summary>
            /// <value>ex_string_02</value>
            public string ExString02 { get; set; }
            /// <summary>Gets or sets ex_string_03</summary>
            /// <value>ex_string_03</value>
            public string ExString03 { get; set; }
            /// <summary>Gets or sets ex_numeric_01</summary>
            /// <value>ex_numeric_01</value>
            public decimal? ExNumeric01 { get; set; }
            /// <summary>Gets or sets ex_numeric_02</summary>
            /// <value>ex_numeric_02</value>
            public decimal? ExNumeric02 { get; set; }
            /// <summary>Gets or sets ex_numeric_03</summary>
            /// <value>ex_numeric_03</value>
            public decimal? ExNumeric03 { get; set; }
            /// <summary>Gets or sets ex_date_01</summary>
            /// <value>ex_date_01</value>
            public DateTime? ExDate01 { get; set; }
            /// <summary>Gets or sets ex_date_02</summary>
            /// <value>ex_date_02</value>
            public DateTime? ExDate02 { get; set; }
            /// <summary>Gets or sets ex_date_03</summary>
            /// <value>ex_date_03</value>
            public DateTime? ExDate03 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 処理名称</summary>
                /// <value>処理名称</value>
                public string ProcName { get; set; }
                /// <summary>Gets or sets 指図番号</summary>
                /// <value>指図番号</value>
                public string OrderNo { get; set; }
                /// <summary>Gets or sets 指図行番号</summary>
                /// <value>指図行番号</value>
                public int RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="procName">処理名称</param>
                /// <param name="orderNo">指図番号</param>
                /// <param name="rowNo">指図行番号</param>
                public PrimaryKey(string procName, string orderNo, int rowNo)
                {
                    ProcName = procName;
                    OrderNo = orderNo;
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ProcName, this.OrderNo, this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="procName">処理名称</param>
            /// <param name="orderNo">指図番号</param>
            /// <param name="rowNo">指図行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public CommonProcInfoEntity GetEntity(string procName, string orderNo, int rowNo, ComDB db)
            {
                CommonProcInfoEntity.PrimaryKey condition = new CommonProcInfoEntity.PrimaryKey(procName, orderNo, rowNo);
                return GetEntity<CommonProcInfoEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 自社マスタ
        /// </summary>
        public class CompanyEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CompanyEntity()
            {
                TableName = "company";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 会社コード</summary>
            /// <value>会社コード</value>
            public string CompanyCd { get; set; }
            /// <summary>Gets or sets 自社名称</summary>
            /// <value>自社名称</value>
            public string HomeName { get; set; }
            /// <summary>Gets or sets 郵便番号</summary>
            /// <value>郵便番号</value>
            public string Zipcode { get; set; }
            /// <summary>Gets or sets 住所1</summary>
            /// <value>住所1</value>
            public string Address1 { get; set; }
            /// <summary>Gets or sets 住所2</summary>
            /// <value>住所2</value>
            public string Address2 { get; set; }
            /// <summary>Gets or sets 住所3</summary>
            /// <value>住所3</value>
            public string Address3 { get; set; }
            /// <summary>Gets or sets 電話番号</summary>
            /// <value>電話番号</value>
            public string TelNo { get; set; }
            /// <summary>Gets or sets FAX番号</summary>
            /// <value>FAX番号</value>
            public string FaxNo { get; set; }
            /// <summary>Gets or sets 代表者役職</summary>
            /// <value>代表者役職</value>
            public string RepresentRole { get; set; }
            /// <summary>Gets or sets 代表者名</summary>
            /// <value>代表者名</value>
            public string RepresentPerson { get; set; }
            /// <summary>Gets or sets 決算月|1から12</summary>
            /// <value>決算月|1から12</value>
            public int ClosingMonth { get; set; }
            /// <summary>Gets or sets 締日|1から31又は99</summary>
            /// <value>締日|1から31又は99</value>
            public int ClosingDay { get; set; }
            /// <summary>Gets or sets 支払更新区分|各種名称マスタ</summary>
            /// <value>支払更新区分|各種名称マスタ</value>
            public int PaymentUpdate { get; set; }
            /// <summary>Gets or sets 消費税課税区分|各種名称マスタ</summary>
            /// <value>消費税課税区分|各種名称マスタ</value>
            public int? TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税算出区分|各種名称マスタ</summary>
            /// <value>消費税算出区分|各種名称マスタ</value>
            public int? CalcDivision { get; set; }
            /// <summary>Gets or sets 消費税端数区分|各種名称マスタ</summary>
            /// <value>消費税端数区分|各種名称マスタ</value>
            public int? TaxRoundup { get; set; }
            /// <summary>Gets or sets 消費税端数単位|各種名称マスタ</summary>
            /// <value>消費税端数単位|各種名称マスタ</value>
            public int? TaxRoundupUnit { get; set; }
            /// <summary>Gets or sets 端数処理区分|各種名称マスタ</summary>
            /// <value>端数処理区分|各種名称マスタ</value>
            public int? Roundup { get; set; }
            /// <summary>Gets or sets 端数処理単位|各種名称マスタ</summary>
            /// <value>端数処理単位|各種名称マスタ</value>
            public int? RoundupUnit { get; set; }
            /// <summary>Gets or sets 売上金額端数区分|各種名称マスタ</summary>
            /// <value>売上金額端数区分|各種名称マスタ</value>
            public int SalesRoundup { get; set; }
            /// <summary>Gets or sets 売上金額端数単位|各種名称マスタ</summary>
            /// <value>売上金額端数単位|各種名称マスタ</value>
            public int SalesRoundupUnit { get; set; }
            /// <summary>Gets or sets 仕入金額端数区分|各種名称マスタ</summary>
            /// <value>仕入金額端数区分|各種名称マスタ</value>
            public int PurchaseRoundup { get; set; }
            /// <summary>Gets or sets 仕入金額端数単位|各種名称マスタ</summary>
            /// <value>仕入金額端数単位|各種名称マスタ</value>
            public int PurchaseRoundupUnit { get; set; }
            /// <summary>Gets or sets 単価端数処理|各種名称マスタ</summary>
            /// <value>単価端数処理|各種名称マスタ</value>
            public int? UnitpriceRoundup { get; set; }
            /// <summary>Gets or sets 単価端数単位|各種名称マスタ</summary>
            /// <value>単価端数単位|各種名称マスタ</value>
            public int? UnitpriceRoundupUnit { get; set; }
            /// <summary>Gets or sets 処方BOM切替区分|各種名称マスタ</summary>
            /// <value>処方BOM切替区分|各種名称マスタ</value>
            public int RecipeBomDivision { get; set; }
            /// <summary>Gets or sets 配合量端数区分|各種名称マスタ</summary>
            /// <value>配合量端数区分|各種名称マスタ</value>
            public int BlendQtyRoundup { get; set; }
            /// <summary>Gets or sets 配合量端数単位|各種名称マスタ</summary>
            /// <value>配合量端数単位|各種名称マスタ</value>
            public int BlendQtyRoundupUnit { get; set; }
            /// <summary>Gets or sets 配合率端数区分|各種名称マスタ</summary>
            /// <value>配合率端数区分|各種名称マスタ</value>
            public int MixRateRoundup { get; set; }
            /// <summary>Gets or sets 配合率端数単位|各種名称マスタ</summary>
            /// <value>配合率端数単位|各種名称マスタ</value>
            public int MixRateRoundupUnit { get; set; }
            /// <summary>Gets or sets 配合調整端数区分|各種名称マスタ</summary>
            /// <value>配合調整端数区分|各種名称マスタ</value>
            public int AdjRoundup { get; set; }
            /// <summary>Gets or sets 配合調整端数単位|各種名称マスタ</summary>
            /// <value>配合調整端数単位|各種名称マスタ</value>
            public int AdjRoundupUnit { get; set; }
            /// <summary>Gets or sets カレンダーコード</summary>
            /// <value>カレンダーコード</value>
            public string CalCd { get; set; }
            /// <summary>Gets or sets 販売品目単価マスタ絞込区分|各種名称マスタ</summary>
            /// <value>販売品目単価マスタ絞込区分|各種名称マスタ</value>
            public int? SaleUnitpriceDivision { get; set; }
            /// <summary>Gets or sets 購買品目単価マスタ絞込区分|各種名称マスタ</summary>
            /// <value>購買品目単価マスタ絞込区分|各種名称マスタ</value>
            public int? PurchaseUnitpriceDivision { get; set; }
            /// <summary>Gets or sets 請求書外部コード使用区分</summary>
            /// <value>請求書外部コード使用区分</value>
            public int? BillOutsideCdPublish { get; set; }
            /// <summary>Gets or sets 入金伝票発行区分|各種名称マスタ</summary>
            /// <value>入金伝票発行区分|各種名称マスタ</value>
            public int? CreditIssuedDivision { get; set; }
            /// <summary>Gets or sets 支払伝票発行区分|各種名称マスタ</summary>
            /// <value>支払伝票発行区分|各種名称マスタ</value>
            public int? PaymentIssuedDivision { get; set; }
            /// <summary>Gets or sets 入金銀行コード1</summary>
            /// <value>入金銀行コード1</value>
            public string BankCd1 { get; set; }
            /// <summary>Gets or sets 支店コード1</summary>
            /// <value>支店コード1</value>
            public string BranchCd1 { get; set; }
            /// <summary>Gets or sets 預金種別1</summary>
            /// <value>預金種別1</value>
            public string DepositType1 { get; set; }
            /// <summary>Gets or sets 預金勘定1</summary>
            /// <value>預金勘定1</value>
            public string DepositAccount1 { get; set; }
            /// <summary>Gets or sets 入金銀行マスタコード1</summary>
            /// <value>入金銀行マスタコード1</value>
            public string BankMasterCd1 { get; set; }
            /// <summary>Gets or sets 口座略称1</summary>
            /// <value>口座略称1</value>
            public string AccountAbbreviation1 { get; set; }
            /// <summary>Gets or sets 口座区分1|各種名称マスタ</summary>
            /// <value>口座区分1|各種名称マスタ</value>
            public int AccountDivision1 { get; set; }
            /// <summary>Gets or sets 口座番号1</summary>
            /// <value>口座番号1</value>
            public string AccountNo1 { get; set; }
            /// <summary>Gets or sets 口座名義人1</summary>
            /// <value>口座名義人1</value>
            public string AccountName1 { get; set; }
            /// <summary>Gets or sets 入金銀行コード2</summary>
            /// <value>入金銀行コード2</value>
            public string BankCd2 { get; set; }
            /// <summary>Gets or sets 支店コード2</summary>
            /// <value>支店コード2</value>
            public string BranchCd2 { get; set; }
            /// <summary>Gets or sets 預金種別2</summary>
            /// <value>預金種別2</value>
            public string DepositType2 { get; set; }
            /// <summary>Gets or sets 預金勘定2</summary>
            /// <value>預金勘定2</value>
            public string DepositAccount2 { get; set; }
            /// <summary>Gets or sets 入金銀行マスタコード2</summary>
            /// <value>入金銀行マスタコード2</value>
            public string BankMasterCd2 { get; set; }
            /// <summary>Gets or sets 口座略称2</summary>
            /// <value>口座略称2</value>
            public string AccountAbbreviation2 { get; set; }
            /// <summary>Gets or sets 口座区分2|各種名称マスタ</summary>
            /// <value>口座区分2|各種名称マスタ</value>
            public int? AccountDivision2 { get; set; }
            /// <summary>Gets or sets 口座番号2</summary>
            /// <value>口座番号2</value>
            public string AccountNo2 { get; set; }
            /// <summary>Gets or sets 口座名義人2</summary>
            /// <value>口座名義人2</value>
            public string AccountName2 { get; set; }
            /// <summary>Gets or sets 入金銀行コード3</summary>
            /// <value>入金銀行コード3</value>
            public string BankCd3 { get; set; }
            /// <summary>Gets or sets 支店コード3</summary>
            /// <value>支店コード3</value>
            public string BranchCd3 { get; set; }
            /// <summary>Gets or sets 預金種別3</summary>
            /// <value>預金種別3</value>
            public string DepositType3 { get; set; }
            /// <summary>Gets or sets 預金勘定3</summary>
            /// <value>預金勘定3</value>
            public string DepositAccount3 { get; set; }
            /// <summary>Gets or sets 入金銀行マスタコード3</summary>
            /// <value>入金銀行マスタコード3</value>
            public string BankMasterCd3 { get; set; }
            /// <summary>Gets or sets 口座略称3</summary>
            /// <value>口座略称3</value>
            public string AccountAbbreviation3 { get; set; }
            /// <summary>Gets or sets 口座区分3|各種名称マスタ</summary>
            /// <value>口座区分3|各種名称マスタ</value>
            public int? AccountDivision3 { get; set; }
            /// <summary>Gets or sets 口座番号3</summary>
            /// <value>口座番号3</value>
            public string AccountNo3 { get; set; }
            /// <summary>Gets or sets 口座名義人3</summary>
            /// <value>口座名義人3</value>
            public string AccountName3 { get; set; }
            /// <summary>Gets or sets 入金銀行コード4</summary>
            /// <value>入金銀行コード4</value>
            public string BankCd4 { get; set; }
            /// <summary>Gets or sets 支店コード4</summary>
            /// <value>支店コード4</value>
            public string BranchCd4 { get; set; }
            /// <summary>Gets or sets 預金種別4</summary>
            /// <value>預金種別4</value>
            public string DepositType4 { get; set; }
            /// <summary>Gets or sets 預金勘定4</summary>
            /// <value>預金勘定4</value>
            public string DepositAccount4 { get; set; }
            /// <summary>Gets or sets 入金銀行マスタコード4</summary>
            /// <value>入金銀行マスタコード4</value>
            public string BankMasterCd4 { get; set; }
            /// <summary>Gets or sets 口座略称4</summary>
            /// <value>口座略称4</value>
            public string AccountAbbreviation4 { get; set; }
            /// <summary>Gets or sets 口座区分4|各種名称マスタ</summary>
            /// <value>口座区分4|各種名称マスタ</value>
            public int? AccountDivision4 { get; set; }
            /// <summary>Gets or sets 口座番号4</summary>
            /// <value>口座番号4</value>
            public string AccountNo4 { get; set; }
            /// <summary>Gets or sets 口座名義人4</summary>
            /// <value>口座名義人4</value>
            public string AccountName4 { get; set; }
            /// <summary>Gets or sets 支払銀行コード</summary>
            /// <value>支払銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 支払支店コード</summary>
            /// <value>支払支店コード</value>
            public string BranchCd { get; set; }
            /// <summary>Gets or sets 支払預金種別</summary>
            /// <value>支払預金種別</value>
            public string DepositType { get; set; }
            /// <summary>Gets or sets 支払預金勘定</summary>
            /// <value>支払預金勘定</value>
            public string DepositAccount { get; set; }
            /// <summary>Gets or sets 支払銀行マスタコード</summary>
            /// <value>支払銀行マスタコード</value>
            public string BankMasterCd { get; set; }
            /// <summary>Gets or sets 口座区分|各種名称マスタ</summary>
            /// <value>口座区分|各種名称マスタ</value>
            public int? AccountDivision { get; set; }
            /// <summary>Gets or sets 口座番号</summary>
            /// <value>口座番号</value>
            public string AccountNo { get; set; }
            /// <summary>Gets or sets 振込依頼人コード</summary>
            /// <value>振込依頼人コード</value>
            public string TransferClientCd { get; set; }
            /// <summary>Gets or sets 振込依頼人名</summary>
            /// <value>振込依頼人名</value>
            public string TransferClientName { get; set; }
            /// <summary>Gets or sets デフォルトロット番号</summary>
            /// <value>デフォルトロット番号</value>
            public string DefaultLotNo { get; set; }
            /// <summary>Gets or sets パスワード有効期限</summary>
            /// <value>パスワード有効期限</value>
            public long? PasswordValidTerm { get; set; }
            /// <summary>Gets or sets パスワード桁数下限</summary>
            /// <value>パスワード桁数下限</value>
            public long? PasswordDigitLowerLimit { get; set; }
            /// <summary>Gets or sets パスワード桁数上限</summary>
            /// <value>パスワード桁数上限</value>
            public long? PasswordDigitUpperLimit { get; set; }
            /// <summary>Gets or sets 取引先別単価マスタ|0</summary>
            /// <value>取引先別単価マスタ|0</value>
            public int? UnitpriceApproval { get; set; }
            /// <summary>Gets or sets 受注承認|0</summary>
            /// <value>受注承認|0</value>
            public int? OrderApproval { get; set; }
            /// <summary>Gets or sets 購買承認|0</summary>
            /// <value>購買承認|0</value>
            public int? PurchaseApproval { get; set; }
            /// <summary>Gets or sets 売上承認|0</summary>
            /// <value>売上承認|0</value>
            public int? SalesApproval { get; set; }
            /// <summary>Gets or sets 移庫の移動中ロケーション</summary>
            /// <value>移庫の移動中ロケーション</value>
            public string CarryingLocation { get; set; }
            /// <summary>Gets or sets メール送信サーバ</summary>
            /// <value>メール送信サーバ</value>
            public string MailSendServer { get; set; }
            /// <summary>Gets or sets メール送信サーバポート</summary>
            /// <value>メール送信サーバポート</value>
            public int? MailSendServerPort { get; set; }
            /// <summary>Gets or sets メール送信サーバユーザー</summary>
            /// <value>メール送信サーバユーザー</value>
            public string MailSendServerUser { get; set; }
            /// <summary>Gets or sets メール送信サーバパスワード</summary>
            /// <value>メール送信サーバパスワード</value>
            public string MailSendServerPassword { get; set; }
            /// <summary>Gets or sets メール送信元アドレス</summary>
            /// <value>メール送信元アドレス</value>
            public string MailFromAddress { get; set; }
            /// <summary>Gets or sets メール送信全体制御|0</summary>
            /// <value>メール送信全体制御|0</value>
            public int? MailSendFlg { get; set; }
            /// <summary>Gets or sets 全社共通マスタ会社レコード</summary>
            /// <value>全社共通マスタ会社レコード</value>
            public string AirBukrs { get; set; }
            /// <summary>Gets or sets 自動FAXSENDER</summary>
            /// <value>自動FAXSENDER</value>
            public string AutoFaxSender { get; set; }
            /// <summary>Gets or sets 自動FAXパスワード</summary>
            /// <value>自動FAXパスワード</value>
            public string AutoFaxPassword { get; set; }
            /// <summary>Gets or sets 自動FAXアカウントコード</summary>
            /// <value>自動FAXアカウントコード</value>
            public string AutoFaxAccount { get; set; }
            /// <summary>Gets or sets 自動FAX送信解像度|自動的にFINEモード</summary>
            /// <value>自動FAX送信解像度|自動的にFINEモード</value>
            public string AutoFaxMode { get; set; }
            /// <summary>Gets or sets 自動FAX返信通知設定|failure</summary>
            /// <value>自動FAX返信通知設定|failure</value>
            public string AutoFaxDel { get; set; }
            /// <summary>Gets or sets 自動FAX共通ドメイン</summary>
            /// <value>自動FAX共通ドメイン</value>
            public string AutoFaxCommonDomain { get; set; }
            /// <summary>Gets or sets 自動FAX標準本文(カバー)</summary>
            /// <value>自動FAX標準本文(カバー)</value>
            public string AutoFaxTextBody { get; set; }
            /// <summary>Gets or sets 消費税率(未使用)</summary>
            /// <value>消費税率(未使用)</value>
            public decimal? TaxRatio { get; set; }
            /// <summary>Gets or sets シングルサインオンフラグ</summary>
            /// <value>シングルサインオンフラグ</value>
            public int? SsoFlg { get; set; }
            /// <summary>Gets or sets マイナス在庫許可</summary>
            /// <value>マイナス在庫許可</value>
            public int? NegativeInventoryPermitFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 会社コード</summary>
                /// <value>会社コード</value>
                public string CompanyCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="companyCd">会社コード</param>
                public PrimaryKey(string companyCd)
                {
                    CompanyCd = companyCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.CompanyCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="companyCd">会社コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public CompanyEntity GetEntity(string companyCd, ComDB db)
            {
                CompanyEntity.PrimaryKey condition = new CompanyEntity.PrimaryKey(companyCd);
                return GetEntity<CompanyEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 自社マスタ拡張設定(内部パラメータ用サブマスタ)
        /// </summary>
        public class CompanySettingEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CompanySettingEntity()
            {
                TableName = "company_setting";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 自社コード</summary>
            /// <value>自社コード</value>
            public string CompanyCd { get; set; }
            /// <summary>Gets or sets 数量チェック 受注数に対する出荷指図累計数の超過|0</summary>
            /// <value>数量チェック 受注数に対する出荷指図累計数の超過|0</value>
            public string QtyCheckShippingOrderOver { get; set; }
            /// <summary>Gets or sets 数量チェック 受注数に対する出荷指図累計数の不足|0</summary>
            /// <value>数量チェック 受注数に対する出荷指図累計数の不足|0</value>
            public string QtyCheckShippingOrderShort { get; set; }
            /// <summary>Gets or sets 数量チェック 出荷指図数に対する出荷実績数の超過|0</summary>
            /// <value>数量チェック 出荷指図数に対する出荷実績数の超過|0</value>
            public string QtyCheckShippingInstructOver { get; set; }
            /// <summary>Gets or sets 数量チェック 受注数に対する出荷指図累計数の不足|0</summary>
            /// <value>数量チェック 受注数に対する出荷指図累計数の不足|0</value>
            public string QtyCheckShippingInstructShort { get; set; }
            /// <summary>Gets or sets 数量チェック 受注数に対する出荷実績累計数の超過|0</summary>
            /// <value>数量チェック 受注数に対する出荷実績累計数の超過|0</value>
            public string QtyCheckShippingResultOver { get; set; }
            /// <summary>Gets or sets 数量チェック 受注数に対する出荷指図累計数の不足|0</summary>
            /// <value>数量チェック 受注数に対する出荷指図累計数の不足|0</value>
            public string QtyCheckShippingResultShort { get; set; }
            /// <summary>Gets or sets 数量チェック 発注数に対する受入予定累計数の超過|0</summary>
            /// <value>数量チェック 発注数に対する受入予定累計数の超過|0</value>
            public string QtyCheckPurchaseOrderOver { get; set; }
            /// <summary>Gets or sets 数量チェック 受入予定数に対する受入実績数の超過|0</summary>
            /// <value>数量チェック 受入予定数に対する受入実績数の超過|0</value>
            public string QtyCheckPurchaseInstructOver { get; set; }
            /// <summary>Gets or sets 数量チェック 発注数に対する受入累計数の超過|0</summary>
            /// <value>数量チェック 発注数に対する受入累計数の超過|0</value>
            public string QtyCheckPurchaseResultOver { get; set; }
            /// <summary>Gets or sets 仕訳送信用代表部門コード</summary>
            /// <value>仕訳送信用代表部門コード</value>
            public string SectionCd { get; set; }
            /// <summary>Gets or sets 仕訳送信用勘定年月(YYYYMM*当月をセット)</summary>
            /// <value>仕訳送信用勘定年月(YYYYMM*当月をセット)</value>
            public string AccountYears { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 自社コード</summary>
                /// <value>自社コード</value>
                public string CompanyCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="companyCd">自社コード</param>
                public PrimaryKey(string companyCd)
                {
                    CompanyCd = companyCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.CompanyCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="companyCd">自社コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public CompanySettingEntity GetEntity(string companyCd, ComDB db)
            {
                CompanySettingEntity.PrimaryKey condition = new CompanySettingEntity.PrimaryKey(companyCd);
                return GetEntity<CompanySettingEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 操作マスタ
        /// </summary>
        public class ControlEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ControlEntity()
            {
                TableName = "control";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets メニューID</summary>
            /// <value>メニューID</value>
            public long MenuId { get; set; }
            /// <summary>Gets or sets タブID</summary>
            /// <value>タブID</value>
            public long TabId { get; set; }
            /// <summary>Gets or sets 操作ID</summary>
            /// <value>操作ID</value>
            public long CtrlId { get; set; }
            /// <summary>Gets or sets 操作名称</summary>
            /// <value>操作名称</value>
            public string CtrlName { get; set; }
            /// <summary>Gets or sets 在庫更新処理番号</summary>
            /// <value>在庫更新処理番号</value>
            public int? ProcessNo { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets メニューID</summary>
                /// <value>メニューID</value>
                public long MenuId { get; set; }
                /// <summary>Gets or sets タブID</summary>
                /// <value>タブID</value>
                public long TabId { get; set; }
                /// <summary>Gets or sets 操作ID</summary>
                /// <value>操作ID</value>
                public long CtrlId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="menuId">メニューID</param>
                /// <param name="tabId">タブID</param>
                /// <param name="ctrlId">操作ID</param>
                public PrimaryKey(long menuId, long tabId, long ctrlId)
                {
                    MenuId = menuId;
                    TabId = tabId;
                    CtrlId = ctrlId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MenuId, this.TabId, this.CtrlId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="menuId">メニューID</param>
            /// <param name="tabId">タブID</param>
            /// <param name="ctrlId">操作ID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ControlEntity GetEntity(long menuId, long tabId, long ctrlId, ComDB db)
            {
                ControlEntity.PrimaryKey condition = new ControlEntity.PrimaryKey(menuId, tabId, ctrlId);
                return GetEntity<ControlEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 権限マスタ_操作権限
        /// </summary>
        public class ControlAuthorityEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ControlAuthorityEntity()
            {
                TableName = "control_authority";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機能ID</summary>
            /// <value>機能ID</value>
            public string Conductid { get; set; }
            /// <summary>Gets or sets 画面NO</summary>
            /// <value>画面NO</value>
            public int Formno { get; set; }
            /// <summary>Gets or sets コントロールID</summary>
            /// <value>コントロールID</value>
            public string Ctrlid { get; set; }
            /// <summary>Gets or sets 定義種類</summary>
            /// <value>定義種類</value>
            public int Definetype { get; set; }
            /// <summary>Gets or sets 項目番号</summary>
            /// <value>項目番号</value>
            public int Itemno { get; set; }
            /// <summary>Gets or sets ロールID</summary>
            /// <value>ロールID</value>
            public int RoleId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機能ID</summary>
                /// <value>機能ID</value>
                public string Conductid { get; set; }
                /// <summary>Gets or sets 画面NO</summary>
                /// <value>画面NO</value>
                public int Formno { get; set; }
                /// <summary>Gets or sets コントロールID</summary>
                /// <value>コントロールID</value>
                public string Ctrlid { get; set; }
                /// <summary>Gets or sets 定義種類</summary>
                /// <value>定義種類</value>
                public int Definetype { get; set; }
                /// <summary>Gets or sets 項目番号</summary>
                /// <value>項目番号</value>
                public int Itemno { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="conductid">機能ID</param>
                /// <param name="formno">画面NO</param>
                /// <param name="ctrlid">コントロールID</param>
                /// <param name="definetype">定義種類</param>
                /// <param name="itemno">項目番号</param>
                public PrimaryKey(string conductid, int formno, string ctrlid, int definetype, int itemno)
                {
                    Conductid = conductid;
                    Formno = formno;
                    Ctrlid = ctrlid;
                    Definetype = definetype;
                    Itemno = itemno;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.Conductid, this.Formno, this.Ctrlid, this.Definetype, this.Itemno);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="conductid">機能ID</param>
            /// <param name="formno">画面NO</param>
            /// <param name="ctrlid">コントロールID</param>
            /// <param name="definetype">定義種類</param>
            /// <param name="itemno">項目番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ControlAuthorityEntity GetEntity(string conductid, int formno, string ctrlid, int definetype, int itemno, ComDB db)
            {
                ControlAuthorityEntity.PrimaryKey condition = new ControlAuthorityEntity.PrimaryKey(conductid, formno, ctrlid, definetype, itemno);
                return GetEntity<ControlAuthorityEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// コピープロシジャ実行管理
        /// </summary>
        public class CopyLastUpdateEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CopyLastUpdateEntity()
            {
                TableName = "copy_last_update";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 実行PROC区分</summary>
            /// <value>実行PROC区分</value>
            public int? CopyDivision { get; set; }
            /// <summary>Gets or sets 最終実行日</summary>
            /// <value>最終実行日</value>
            public DateTime? CopyUpdateDate { get; set; }
        }

        /// <summary>
        /// 入金トランザクション
        /// </summary>
        public class CreditEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CreditEntity()
            {
                TableName = "credit";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 入金番号</summary>
            /// <value>入金番号</value>
            public string CreditNo { get; set; }
            /// <summary>Gets or sets 入金行番号</summary>
            /// <value>入金行番号</value>
            public int CreditRowNo { get; set; }
            /// <summary>Gets or sets 入金元番号</summary>
            /// <value>入金元番号</value>
            public string OriginalCreditNo { get; set; }
            /// <summary>Gets or sets 入金ステータス|各種名称マスタ</summary>
            /// <value>入金ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 入金日付</summary>
            /// <value>入金日付</value>
            public DateTime CreditDate { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets データ種別|(分類マスタ参照)</summary>
            /// <value>データ種別|(分類マスタ参照)</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 入金金額</summary>
            /// <value>入金金額</value>
            public decimal? CreditAmount { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 支店コード</summary>
            /// <value>支店コード</value>
            public string BranchCd { get; set; }
            /// <summary>Gets or sets 預金種別</summary>
            /// <value>預金種別</value>
            public string DepositType { get; set; }
            /// <summary>Gets or sets 預金勘定</summary>
            /// <value>預金勘定</value>
            public string DepositAccount { get; set; }
            /// <summary>Gets or sets 口座区分</summary>
            /// <value>口座区分</value>
            public int? AccountDivision { get; set; }
            /// <summary>Gets or sets 口座番号</summary>
            /// <value>口座番号</value>
            public string AccountNo { get; set; }
            /// <summary>Gets or sets 手形種別</summary>
            /// <value>手形種別</value>
            public int? NoteDivision { get; set; }
            /// <summary>Gets or sets 手形番号</summary>
            /// <value>手形番号</value>
            public string NoteNo { get; set; }
            /// <summary>Gets or sets 振出日</summary>
            /// <value>振出日</value>
            public DateTime? DrawalDate { get; set; }
            /// <summary>Gets or sets 満期日</summary>
            /// <value>満期日</value>
            public DateTime? NoteDate { get; set; }
            /// <summary>Gets or sets 摘要名</summary>
            /// <value>摘要名</value>
            public string Summary { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ</summary>
            /// <value>消込完了フラグ</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 売掛対象</summary>
            /// <value>売掛対象</value>
            public int? DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 請求対象</summary>
            /// <value>請求対象</value>
            public int? ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ</summary>
            /// <value>売掛更新フラグ</value>
            public int? DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 請求更新フラグ</summary>
            /// <value>請求更新フラグ</value>
            public int? ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }
            /// <summary>Gets or sets 身代</summary>
            /// <value>身代</value>
            public decimal? CreditAmountEx { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal? TaxRatio { get; set; }
            /// <summary>Gets or sets 消費税コード</summary>
            /// <value>消費税コード</value>
            public string TaxCd { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 入金番号</summary>
                /// <value>入金番号</value>
                public string CreditNo { get; set; }
                /// <summary>Gets or sets 入金行番号</summary>
                /// <value>入金行番号</value>
                public int CreditRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="creditNo">入金番号</param>
                /// <param name="creditRowNo">入金行番号</param>
                public PrimaryKey(string creditNo, int creditRowNo)
                {
                    CreditNo = creditNo;
                    CreditRowNo = creditRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.CreditNo, this.CreditRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="creditNo">入金番号</param>
            /// <param name="creditRowNo">入金行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public CreditEntity GetEntity(string creditNo, int creditRowNo, ComDB db)
            {
                CreditEntity.PrimaryKey condition = new CreditEntity.PrimaryKey(creditNo, creditRowNo);
                return GetEntity<CreditEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 通貨マスタ
        /// </summary>
        public class CurrencyCtlCtlEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CurrencyCtlCtlEntity()
            {
                TableName = "currency_ctl_ctl";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime ExValidDate { get; set; }
            /// <summary>Gets or sets 通貨シンボル</summary>
            /// <value>通貨シンボル</value>
            public string CurrencySymbol { get; set; }
            /// <summary>Gets or sets 国</summary>
            /// <value>国</value>
            public string CountryCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal ExRate { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal? TaxRatio { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 通貨コード</summary>
                /// <value>通貨コード</value>
                public string CurrencyCd { get; set; }
                /// <summary>Gets or sets レート適用開始日</summary>
                /// <value>レート適用開始日</value>
                public DateTime ExValidDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="currencyCd">通貨コード</param>
                /// <param name="validDate">レート適用開始日</param>
                public PrimaryKey(string currencyCd, DateTime validDate)
                {
                    CurrencyCd = currencyCd;
                    ExValidDate = validDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.CurrencyCd, this.ExValidDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="currencyCd">通貨コード</param>
            /// <param name="validDate">レート適用開始日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public CurrencyCtlCtlEntity GetEntity(string currencyCd, DateTime validDate, ComDB db)
            {
                CurrencyCtlCtlEntity.PrimaryKey condition = new CurrencyCtlCtlEntity.PrimaryKey(currencyCd, validDate);
                return GetEntity<CurrencyCtlCtlEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// プロシージャグローバル値格納用
        /// </summary>
        public class DbStorageEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DbStorageEntity()
            {
                TableName = "db_storage";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets spid</summary>
            /// <value>spid</value>
            public int Spid { get; set; }
            /// <summary>Gets or sets login_time</summary>
            /// <value>login_time</value>
            public DateTime LoginTime { get; set; }
            /// <summary>Gets or sets name</summary>
            /// <value>name</value>
            public string Name { get; set; }
            /// <summary>Gets or sets v_value</summary>
            /// <value>v_value</value>
            public string VValue { get; set; }
            /// <summary>Gets or sets c_value</summary>
            /// <value>c_value</value>
            public string CValue { get; set; }
            /// <summary>Gets or sets nc_value</summary>
            /// <value>nc_value</value>
            public string NcValue { get; set; }
            /// <summary>Gets or sets b_value</summary>
            /// <value>b_value</value>
            //public ? BValue { get; set; }
        }

        /// <summary>
        /// 納入先マスタ
        /// </summary>
        public class DeliveryEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DeliveryEntity()
            {
                TableName = "delivery";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 納入先名称1</summary>
            /// <value>納入先名称1</value>
            public string DeliveryName1 { get; set; }
            /// <summary>Gets or sets 納入先名称2</summary>
            /// <value>納入先名称2</value>
            public string DeliveryName2 { get; set; }
            /// <summary>Gets or sets 納入先略称</summary>
            /// <value>納入先略称</value>
            public string DeliveryShortName { get; set; }
            /// <summary>Gets or sets 納入先名称かな</summary>
            /// <value>納入先名称かな</value>
            public string DeliveryNameKana { get; set; }
            /// <summary>Gets or sets 郵便番号</summary>
            /// <value>郵便番号</value>
            public string Zipcode { get; set; }
            /// <summary>Gets or sets 住所1</summary>
            /// <value>住所1</value>
            public string Address1 { get; set; }
            /// <summary>Gets or sets 住所2</summary>
            /// <value>住所2</value>
            public string Address2 { get; set; }
            /// <summary>Gets or sets 住所3</summary>
            /// <value>住所3</value>
            public string Address3 { get; set; }
            /// <summary>Gets or sets 電話番号</summary>
            /// <value>電話番号</value>
            public string TelNo { get; set; }
            /// <summary>Gets or sets FAX番号</summary>
            /// <value>FAX番号</value>
            public string FaxNo { get; set; }
            /// <summary>Gets or sets 運賃請求有無|各種名称マスタ</summary>
            /// <value>運賃請求有無|各種名称マスタ</value>
            public int? FareClaimExistence { get; set; }
            /// <summary>Gets or sets 自社営業担当ID</summary>
            /// <value>自社営業担当ID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 納入先担当名</summary>
            /// <value>納入先担当名</value>
            public string DeliveryUserName { get; set; }
            /// <summary>Gets or sets mailアドレス</summary>
            /// <value>mailアドレス</value>
            public string Mail { get; set; }
            /// <summary>Gets or sets 運送会社</summary>
            /// <value>運送会社</value>
            public string CarryCd { get; set; }
            /// <summary>Gets or sets 納入条件</summary>
            /// <value>納入条件</value>
            public string DeliveryCondition { get; set; }
            /// <summary>Gets or sets 納入備考</summary>
            /// <value>納入備考</value>
            public string DeliveryRemark { get; set; }
            /// <summary>Gets or sets 取引先区分(未使用)|各種名称マスタ</summary>
            /// <value>取引先区分(未使用)|各種名称マスタ</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード(未使用)</summary>
            /// <value>取引先コード(未使用)</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 基準払出地区(未使用)</summary>
            /// <value>基準払出地区(未使用)</value>
            public string AreaCd { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int? DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 納入先コード</summary>
                /// <value>納入先コード</value>
                public string DeliveryCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="deliveryCd">納入先コード</param>
                public PrimaryKey(string deliveryCd)
                {
                    DeliveryCd = deliveryCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.DeliveryCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="deliveryCd">納入先コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public DeliveryEntity GetEntity(string deliveryCd, ComDB db)
            {
                DeliveryEntity.PrimaryKey condition = new DeliveryEntity.PrimaryKey(deliveryCd);
                return GetEntity<DeliveryEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 売掛トランザクション_ヘッダ
        /// </summary>
        public class DepositHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DepositHeaderEntity()
            {
                TableName = "deposit_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string InvoiceDivision { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime? InvoiceActiveDate { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime CreditDate { get; set; }
            /// <summary>Gets or sets 差引繰越額</summary>
            /// <value>差引繰越額</value>
            public decimal? BalanceForward { get; set; }
            /// <summary>Gets or sets 売上金額</summary>
            /// <value>売上金額</value>
            public decimal? SalesAmount { get; set; }
            /// <summary>Gets or sets 入金金額</summary>
            /// <value>入金金額</value>
            public decimal? DepositAmount { get; set; }
            /// <summary>Gets or sets その他入金金額</summary>
            /// <value>その他入金金額</value>
            public decimal? OtherDepositAmount { get; set; }
            /// <summary>Gets or sets 返品金額</summary>
            /// <value>返品金額</value>
            public decimal? ReturnedAmount { get; set; }
            /// <summary>Gets or sets 値引金額</summary>
            /// <value>値引金額</value>
            public decimal? DiscountAmount { get; set; }
            /// <summary>Gets or sets その他売上金額</summary>
            /// <value>その他売上金額</value>
            public decimal? OtherSalesAmount { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 売掛残</summary>
            /// <value>売掛残</value>
            public decimal? CreditAmount { get; set; }
            /// <summary>Gets or sets 売掛金(内訳)</summary>
            /// <value>売掛金(内訳)</value>
            public decimal? CreditSalesBreakdown { get; set; }
            /// <summary>Gets or sets 未収金(内訳)</summary>
            /// <value>未収金(内訳)</value>
            public decimal? AccruedDebitBreakdown { get; set; }
            /// <summary>Gets or sets 以外(内訳)</summary>
            /// <value>以外(内訳)</value>
            public decimal? ExceptBreakdown { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 売掛番号</summary>
                /// <value>売掛番号</value>
                public string DepositNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="depositNo">売掛番号</param>
                public PrimaryKey(string depositNo)
                {
                    DepositNo = depositNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.DepositNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="depositNo">売掛番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public DepositHeaderEntity GetEntity(string depositNo, ComDB db)
            {
                DepositHeaderEntity.PrimaryKey condition = new DepositHeaderEntity.PrimaryKey(depositNo);
                return GetEntity<DepositHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 製造指図ヘッダー
        /// </summary>
        public class DirectionHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DirectionHeaderEntity()
            {
                TableName = "direction_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 指図区分</summary>
            /// <value>指図区分</value>
            public int DirectionDivision { get; set; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string DirectionNo { get; set; }
            /// <summary>Gets or sets 指図日時</summary>
            /// <value>指図日時</value>
            public DateTime? DirectionDate { get; set; }
            /// <summary>Gets or sets 指図ステータス</summary>
            /// <value>指図ステータス</value>
            public int? DirectionStatus { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 品目開始有効日</summary>
            /// <value>品目開始有効日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }
            /// <summary>Gets or sets 予定生産量</summary>
            /// <value>予定生産量</value>
            public decimal? PlanedQty { get; set; }
            /// <summary>Gets or sets 実績生産量</summary>
            /// <value>実績生産量</value>
            public decimal? ResultQty { get; set; }
            /// <summary>Gets or sets 開始予定日時</summary>
            /// <value>開始予定日時</value>
            public DateTime? PlanedStartDate { get; set; }
            /// <summary>Gets or sets 終了予定日時</summary>
            /// <value>終了予定日時</value>
            public DateTime? PlanedEndDate { get; set; }
            /// <summary>Gets or sets 開始実績日時</summary>
            /// <value>開始実績日時</value>
            public DateTime? ResultStartDate { get; set; }
            /// <summary>Gets or sets 終了実績日時</summary>
            /// <value>終了実績日時</value>
            public DateTime? ResultEndDate { get; set; }
            /// <summary>Gets or sets 指図書発行フラグ</summary>
            /// <value>指図書発行フラグ</value>
            public int? StampFlg { get; set; }
            /// <summary>Gets or sets 指図書発行日時</summary>
            /// <value>指図書発行日時</value>
            public DateTime? StampDate { get; set; }
            /// <summary>Gets or sets 指図書発行者ID</summary>
            /// <value>指図書発行者ID</value>
            public string StampUserId { get; set; }
            /// <summary>Gets or sets 検査有無</summary>
            /// <value>検査有無</value>
            public int? InspectionExistence { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets 承認ステータス</summary>
            /// <value>承認ステータス</value>
            public int? ApprovalStatus { get; set; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long? RecipeId { get; set; }
            /// <summary>Gets or sets レシピコード</summary>
            /// <value>レシピコード</value>
            public string RecipeCd { get; set; }
            /// <summary>Gets or sets レシピバージョン</summary>
            /// <value>レシピバージョン</value>
            public long? RecipeVersion { get; set; }
            /// <summary>Gets or sets 生産ライン</summary>
            /// <value>生産ライン</value>
            public string ProductionLine { get; set; }
            /// <summary>Gets or sets 仕入先コード</summary>
            /// <value>仕入先コード</value>
            public string VenderCd { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 指図区分</summary>
                /// <value>指図区分</value>
                public int DirectionDivision { get; set; }
                /// <summary>Gets or sets 指図番号</summary>
                /// <value>指図番号</value>
                public string DirectionNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="directionDivision">指図区分</param>
                /// <param name="directionNo">指図番号</param>
                public PrimaryKey(int directionDivision, string directionNo)
                {
                    DirectionDivision = directionDivision;
                    DirectionNo = directionNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.DirectionDivision, this.DirectionNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="directionDivision">指図区分</param>
            /// <param name="directionNo">指図番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public DirectionHeaderEntity GetEntity(int directionDivision, string directionNo, ComDB db)
            {
                DirectionHeaderEntity.PrimaryKey condition = new DirectionHeaderEntity.PrimaryKey(directionDivision, directionNo);
                return GetEntity<DirectionHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 製造指図プロシージャ
        /// </summary>
        public class DirectionProcedureEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DirectionProcedureEntity()
            {
                TableName = "direction_procedure";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 指図区分|各種名称マスタ</summary>
            /// <value>指図区分|各種名称マスタ</value>
            public int DirectionDivision { get; set; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string DirectionNo { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? Seq { get; set; }
            /// <summary>Gets or sets 工程コード</summary>
            /// <value>工程コード</value>
            public string OperationCd { get; set; }
            /// <summary>Gets or sets 条件</summary>
            /// <value>条件</value>
            public string Condition { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets 有効開始日時</summary>
            /// <value>有効開始日時</value>
            public DateTime? StartDate { get; set; }
            /// <summary>Gets or sets 有効終了日時</summary>
            /// <value>有効終了日時</value>
            public DateTime? EndDate { get; set; }
            /// <summary>Gets or sets 開始実績日時</summary>
            /// <value>開始実績日時</value>
            public DateTime? ResultStartDate { get; set; }
            /// <summary>Gets or sets 終了実績日時</summary>
            /// <value>終了実績日時</value>
            public DateTime? ResultEndDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 指図区分|各種名称マスタ</summary>
                /// <value>指図区分|各種名称マスタ</value>
                public int DirectionDivision { get; set; }
                /// <summary>Gets or sets 指図番号</summary>
                /// <value>指図番号</value>
                public string DirectionNo { get; set; }
                /// <summary>Gets or sets 工程番号</summary>
                /// <value>工程番号</value>
                public int StepNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="directionDivision">指図区分</param>
                /// <param name="directionNo">指図番号</param>
                /// <param name="stepNo">工程番号</param>
                public PrimaryKey(int directionDivision, string directionNo, int stepNo)
                {
                    DirectionDivision = directionDivision;
                    DirectionNo = directionNo;
                    StepNo = stepNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.DirectionDivision, this.DirectionNo, this.StepNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="directionDivision">指図区分</param>
            /// <param name="directionNo">指図番号</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public DirectionProcedureEntity GetEntity(int directionDivision, string directionNo, int stepNo, ComDB db)
            {
                DirectionProcedureEntity.PrimaryKey condition = new DirectionProcedureEntity.PrimaryKey(directionDivision, directionNo, stepNo);
                return GetEntity<DirectionProcedureEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 製造指図フォーミュラ
        /// </summary>
        public class DirectionFormulaEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DirectionFormulaEntity()
            {
                TableName = "direction_formula";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 指図区分</summary>
            /// <value>指図区分</value>
            public int DirectionDivision { get; set; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string DirectionNo { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets 実績区分</summary>
            /// <value>実績区分</value>
            public int ResultDivision { get; set; }
            /// <summary>Gets or sets シーケンスNO</summary>
            /// <value>シーケンスNO</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? Seq { get; set; }
            /// <summary>Gets or sets 品目タイプ</summary>
            /// <value>品目タイプ</value>
            public int? LineType { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目開始有効日</summary>
            /// <value>品目開始有効日</value>
            public DateTime? ItemActiveDate { get; set; }
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
            /// <summary>Gets or sets 仕上NO</summary>
            /// <value>仕上NO</value>
            public int? FinishNo { get; set; }
            /// <summary>Gets or sets 仕上完了フラグ</summary>
            /// <value>仕上完了フラグ</value>
            public int? CompFlg { get; set; }
            /// <summary>Gets or sets 計上予定日</summary>
            /// <value>計上予定日</value>
            public DateTime? ScheduledAccountingDate { get; set; }
            /// <summary>Gets or sets 実績日時</summary>
            /// <value>実績日時</value>
            public DateTime? AccountingDate { get; set; }
            /// <summary>Gets or sets 数量</summary>
            /// <value>数量</value>
            public decimal? Qty { get; set; }
            /// <summary>Gets or sets 実績量</summary>
            /// <value>実績量</value>
            public decimal? ResultQty { get; set; }
            /// <summary>Gets or sets 部品入力区分</summary>
            /// <value>部品入力区分</value>
            public int? PartInputDivision { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 指図区分</summary>
                /// <value>指図区分</value>
                public int DirectionDivision { get; set; }
                /// <summary>Gets or sets 指図番号</summary>
                /// <value>指図番号</value>
                public string DirectionNo { get; set; }
                /// <summary>Gets or sets 工程番号</summary>
                /// <value>工程番号</value>
                public int StepNo { get; set; }
                /// <summary>Gets or sets 実績区分</summary>
                /// <value>実績区分</value>
                public int ResultDivision { get; set; }
                /// <summary>Gets or sets シーケンスNO</summary>
                /// <value>シーケンスNO</value>
                public int SeqNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="directionDivision">指図区分</param>
                /// <param name="directionNo">指図番号</param>
                /// <param name="stepNo">工程番号</param>
                /// <param name="resultDivision">実績区分</param>
                /// <param name="seqNo">シーケンスNO</param>
                public PrimaryKey(int directionDivision, string directionNo, int stepNo, int resultDivision, int seqNo)
                {
                    DirectionDivision = directionDivision;
                    DirectionNo = directionNo;
                    StepNo = stepNo;
                    ResultDivision = resultDivision;
                    SeqNo = seqNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.DirectionDivision, this.DirectionNo, this.StepNo, this.ResultDivision, this.SeqNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="directionDivision">指図区分</param>
            /// <param name="directionNo">指図番号</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="resultDivision">実績区分</param>
            /// <param name="seqNo">シーケンスNO</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public DirectionFormulaEntity GetEntity(int directionDivision, string directionNo, int stepNo, int resultDivision, int seqNo, ComDB db)
            {
                DirectionFormulaEntity.PrimaryKey condition = new DirectionFormulaEntity.PrimaryKey(directionDivision, directionNo, stepNo, resultDivision, seqNo);
                return GetEntity<DirectionFormulaEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 製造指図作業
        /// </summary>
        public class DirectionTaskEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DirectionTaskEntity()
            {
                TableName = "direction_task";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 指図区分|各種名称マスタ</summary>
            /// <value>指図区分|各種名称マスタ</value>
            public int DirectionDivision { get; set; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string DirectionNo { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets シーケンスNO</summary>
            /// <value>シーケンスNO</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? Seq { get; set; }
            /// <summary>Gets or sets 作業コード</summary>
            /// <value>作業コード</value>
            public string TaskCd { get; set; }
            /// <summary>Gets or sets 作業者ID</summary>
            /// <value>作業者ID</value>
            public string WorkerId { get; set; }
            /// <summary>Gets or sets 時間内作業時間(分)</summary>
            /// <value>時間内作業時間(分)</value>
            public long? IntimeJobtime { get; set; }
            /// <summary>Gets or sets 時間外作業時間(分)</summary>
            /// <value>時間外作業時間(分)</value>
            public long? OuttimeJobtime { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 指図区分|各種名称マスタ</summary>
                /// <value>指図区分|各種名称マスタ</value>
                public int DirectionDivision { get; set; }
                /// <summary>Gets or sets 指図番号</summary>
                /// <value>指図番号</value>
                public string DirectionNo { get; set; }
                /// <summary>Gets or sets 工程番号</summary>
                /// <value>工程番号</value>
                public int StepNo { get; set; }
                /// <summary>Gets or sets シーケンスNO</summary>
                /// <value>シーケンスNO</value>
                public int SeqNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="directionDivision">指図区分</param>
                /// <param name="directionNo">指図番号</param>
                /// <param name="stepNo">工程番号</param>
                /// <param name="seqNo">シーケンスNO</param>
                public PrimaryKey(int directionDivision, string directionNo, int stepNo, int seqNo)
                {
                    DirectionDivision = directionDivision;
                    DirectionNo = directionNo;
                    StepNo = stepNo;
                    SeqNo = seqNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.DirectionDivision, this.DirectionNo, this.StepNo, this.SeqNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="directionDivision">指図区分</param>
            /// <param name="directionNo">指図番号</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="seqNo">シーケンスNO</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public DirectionTaskEntity GetEntity(int directionDivision, string directionNo, int stepNo, int seqNo, ComDB db)
            {
                DirectionTaskEntity.PrimaryKey condition = new DirectionTaskEntity.PrimaryKey(directionDivision, directionNo, stepNo, seqNo);
                return GetEntity<DirectionTaskEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 製造指図ロス
        /// </summary>
        public class DirectionLossEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DirectionLossEntity()
            {
                TableName = "direction_loss";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 指図区分|各種名称マスタ</summary>
            /// <value>指図区分|各種名称マスタ</value>
            public int DirectionDivision { get; set; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string DirectionNo { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets 実績区分</summary>
            /// <value>実績区分</value>
            public int ResultDivision { get; set; }
            /// <summary>Gets or sets シーケンスNO</summary>
            /// <value>シーケンスNO</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? Seq { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目開始有効日</summary>
            /// <value>品目開始有効日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets ロス数量</summary>
            /// <value>ロス数量</value>
            public decimal? LossQty { get; set; }
            /// <summary>Gets or sets ロス要因</summary>
            /// <value>ロス要因</value>
            public string LossRyCd { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 指図区分|各種名称マスタ</summary>
                /// <value>指図区分|各種名称マスタ</value>
                public int DirectionDivision { get; set; }
                /// <summary>Gets or sets 指図番号</summary>
                /// <value>指図番号</value>
                public string DirectionNo { get; set; }
                /// <summary>Gets or sets 工程番号</summary>
                /// <value>工程番号</value>
                public int StepNo { get; set; }
                /// <summary>Gets or sets 実績区分</summary>
                /// <value>実績区分</value>
                public int ResultDivision { get; set; }
                /// <summary>Gets or sets シーケンスNO</summary>
                /// <value>シーケンスNO</value>
                public int SeqNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="directionDivision">指図区分</param>
                /// <param name="directionNo">指図番号</param>
                /// <param name="stepNo">工程番号</param>
                /// <param name="resultDivision">実績区分</param>
                /// <param name="seqNo">シーケンスNO</param>
                public PrimaryKey(int directionDivision, string directionNo, int stepNo, int resultDivision, int seqNo)
                {
                    DirectionDivision = directionDivision;
                    DirectionNo = directionNo;
                    StepNo = stepNo;
                    ResultDivision = resultDivision;
                    SeqNo = seqNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.DirectionDivision, this.DirectionNo, this.StepNo, this.ResultDivision, this.SeqNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="directionDivision">指図区分</param>
            /// <param name="directionNo">指図番号</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="resultDivision">実績区分</param>
            /// <param name="seqNo">シーケンスNO</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public DirectionLossEntity GetEntity(int directionDivision, string directionNo, int stepNo, int resultDivision, int seqNo, ComDB db)
            {
                DirectionLossEntity.PrimaryKey condition = new DirectionLossEntity.PrimaryKey(directionDivision, directionNo, stepNo, resultDivision, seqNo);
                return GetEntity<DirectionLossEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ロット付帯情報ヘッダ
        /// </summary>
        public class LotExtInfoHeadEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LotExtInfoHeadEntity()
            {
                TableName = "lot_ext_info_head";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets ロットNO</summary>
            /// <value>ロットNO</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>サブロット番号1</value>
            public string SubLotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>サブロット番号2</value>
            public string SubLotNo2 { get; set; }
            /// <summary>Gets or sets ロット発生日時</summary>
            /// <value>ロット発生日時</value>
            public DateTime? IssueDate { get; set; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }
            /// <summary>Gets or sets 検査ステータス</summary>
            /// <value>検査ステータス</value>
            public int? InspectionStatus { get; set; }
            /// <summary>Gets or sets 検査日</summary>
            /// <value>検査日</value>
            public DateTime? InspectionDate { get; set; }
            /// <summary>Gets or sets ロット品質期限(検査日基準)</summary>
            /// <value>ロット品質期限(検査日基準)</value>
            public DateTime? QualityDeadline { get; set; }
            /// <summary>Gets or sets ロット有効期限(仕上日基準)</summary>
            /// <value>ロット有効期限(仕上日基準)</value>
            public DateTime? ExpirationDeadline { get; set; }
            /// <summary>Gets or sets 品質グレード</summary>
            /// <value>品質グレード</value>
            public int? QualityGrade { get; set; }
            /// <summary>Gets or sets 出荷用在庫ステータス</summary>
            /// <value>出荷用在庫ステータス</value>
            public int? StatusShipping { get; set; }
            /// <summary>Gets or sets 製造用在庫ステータス</summary>
            /// <value>製造用在庫ステータス</value>
            public int? StatusDirection { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets ロットNO</summary>
                /// <value>ロットNO</value>
                public string LotNo { get; set; }
                /// <summary>Gets or sets サブロット番号1</summary>
                /// <value>サブロット番号1</value>
                public string SubLotNo1 { get; set; }
                /// <summary>Gets or sets サブロット番号2</summary>
                /// <value>サブロット番号2</value>
                public string SubLotNo2 { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="lotNo">ロットNO</param>
                /// <param name="subLotNo1">サブロット番号1</param>
                /// <param name="subLotNo2">サブロット番号2</param>
                public PrimaryKey(string itemCd, string specificationCd, string lotNo, string subLotNo1, string subLotNo2)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    LotNo = lotNo;
                    SubLotNo1 = subLotNo1;
                    SubLotNo2 = subLotNo2;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd, this.LotNo, this.SubLotNo1, this.SubLotNo2);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="lotNo">ロットNO</param>
            /// <param name="subLotNo1">サブロット番号1</param>
            /// <param name="subLotNo2">サブロット番号2</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public LotExtInfoHeadEntity GetEntity(string itemCd, string specificationCd, string lotNo, string subLotNo1, string subLotNo2, ComDB db)
            {
                LotExtInfoHeadEntity.PrimaryKey condition = new LotExtInfoHeadEntity.PrimaryKey(itemCd, specificationCd, lotNo, subLotNo1, subLotNo2);
                return GetEntity<LotExtInfoHeadEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ロット付帯情報詳細
        /// </summary>
        public class LotExtInfoDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LotExtInfoDetailEntity()
            {
                TableName = "lot_ext_info_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets ロットNO</summary>
            /// <value>ロットNO</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>サブロット番号1</value>
            public string SubLotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>サブロット番号2</value>
            public string SubLotNo2 { get; set; }
            /// <summary>Gets or sets 項目ID</summary>
            /// <value>項目ID</value>
            public string InfoId { get; set; }
            /// <summary>Gets or sets シーケンスNO</summary>
            /// <value>シーケンスNO</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets 検査日</summary>
            /// <value>検査日</value>
            public DateTime? InspectionDate { get; set; }
            /// <summary>Gets or sets 実績値</summary>
            /// <value>実績値</value>
            public string ResultValue { get; set; }
            /// <summary>Gets or sets 条件</summary>
            /// <value>条件</value>
            public string Condition { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets ロットNO</summary>
                /// <value>ロットNO</value>
                public string LotNo { get; set; }
                /// <summary>Gets or sets サブロット番号1</summary>
                /// <value>サブロット番号1</value>
                public string SubLotNo1 { get; set; }
                /// <summary>Gets or sets サブロット番号2</summary>
                /// <value>サブロット番号2</value>
                public string SubLotNo2 { get; set; }
                /// <summary>Gets or sets 項目ID</summary>
                /// <value>項目ID</value>
                public string InfoId { get; set; }
                /// <summary>Gets or sets シーケンスNO</summary>
                /// <value>シーケンスNO</value>
                public int SeqNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="lotNo">ロットNO</param>
                /// <param name="subLotNo1">サブロット番号1</param>
                /// <param name="subLotNo2">サブロット番号2</param>
                /// <param name="infoId">項目ID</param>
                /// <param name="seqNo">シーケンスNO</param>
                public PrimaryKey(string itemCd, string specificationCd, string lotNo, string subLotNo1, string subLotNo2, string infoId, int seqNo)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    LotNo = lotNo;
                    SubLotNo1 = subLotNo1;
                    SubLotNo2 = subLotNo2;
                    InfoId = infoId;
                    SeqNo = seqNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd, this.LotNo, this.SubLotNo1, this.SubLotNo2, this.InfoId, this.SeqNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="lotNo">ロットNO</param>
            /// <param name="subLotNo1">サブロット番号1</param>
            /// <param name="subLotNo2">サブロット番号2</param>
            /// <param name="infoId">項目ID</param>
            /// <param name="seqNo">シーケンスNO</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public LotExtInfoDetailEntity GetEntity(string itemCd, string specificationCd, string lotNo, string subLotNo1, string subLotNo2, string infoId, int seqNo, ComDB db)
            {
                LotExtInfoDetailEntity.PrimaryKey condition = new LotExtInfoDetailEntity.PrimaryKey(itemCd, specificationCd, lotNo, subLotNo1, subLotNo2, infoId, seqNo);
                return GetEntity<LotExtInfoDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 消込トランザクション
        /// </summary>
        public class EraserCsmEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public EraserCsmEntity()
            {
                TableName = "eraser_csm";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets データ種別|1</summary>
            /// <value>データ種別|1</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets 伝票番号</summary>
            /// <value>伝票番号</value>
            public string SlipNo { get; set; }
            /// <summary>Gets or sets 処理日付</summary>
            /// <value>処理日付</value>
            public DateTime ProcessingDate { get; set; }
            /// <summary>Gets or sets 消込対象額</summary>
            /// <value>消込対象額</value>
            public decimal EraserObjectAmount { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal EraserBalanceAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 承認ステータス|1</summary>
            /// <value>承認ステータス|1</value>
            public int? ApprovalStatus { get; set; }
            /// <summary>Gets or sets 承認者</summary>
            /// <value>承認者</value>
            public string ApprovedBy { get; set; }
            /// <summary>Gets or sets 承認日</summary>
            /// <value>承認日</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets 消込日付</summary>
            /// <value>消込日付</value>
            public DateTime? EraserDate { get; set; }
            /// <summary>Gets or sets 消込更新日時</summary>
            /// <value>消込更新日時</value>
            public DateTime? EraserUpdateDate { get; set; }
            /// <summary>Gets or sets 消込担当者ID</summary>
            /// <value>消込担当者ID</value>
            public string EraseUserIdCd { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 請求先コード</summary>
                /// <value>請求先コード</value>
                public string InvoiceCd { get; set; }
                /// <summary>Gets or sets データ種別|1</summary>
                /// <value>データ種別|1</value>
                public int DataType { get; set; }
                /// <summary>Gets or sets 伝票番号</summary>
                /// <value>伝票番号</value>
                public string SlipNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="invoiceCd">請求先コード</param>
                /// <param name="dataType">データ種別</param>
                /// <param name="slipNo">伝票番号</param>
                public PrimaryKey(string invoiceCd, int dataType, string slipNo)
                {
                    InvoiceCd = invoiceCd;
                    DataType = dataType;
                    SlipNo = slipNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InvoiceCd, this.DataType, this.SlipNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="invoiceCd">請求先コード</param>
            /// <param name="dataType">データ種別</param>
            /// <param name="slipNo">伝票番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public EraserCsmEntity GetEntity(string invoiceCd, int dataType, string slipNo, ComDB db)
            {
                EraserCsmEntity.PrimaryKey condition = new EraserCsmEntity.PrimaryKey(invoiceCd, dataType, slipNo);
                return GetEntity<EraserCsmEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 入金消込トランザクション
        /// </summary>
        public class EraserCsmCreditEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public EraserCsmCreditEntity()
            {
                TableName = "eraser_csm_credit";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime ClaimDate { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets データ種別</summary>
            /// <value>データ種別</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 消込ステータス|各種名称マスタ</summary>
            /// <value>消込ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 売上日付</summary>
            /// <value>売上日付</value>
            public DateTime SalesDate { get; set; }
            /// <summary>Gets or sets 消込対象額</summary>
            /// <value>消込対象額</value>
            public decimal EraserObjectAmount { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal EraserAmount { get; set; }
            /// <summary>Gets or sets 相殺消込額</summary>
            /// <value>相殺消込額</value>
            public decimal EraserOffsetAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal EraserBalanceAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ</summary>
            /// <value>消込完了フラグ</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 消込日付</summary>
            /// <value>消込日付</value>
            public DateTime? EraserDate { get; set; }
            /// <summary>Gets or sets 消込更新日時</summary>
            /// <value>消込更新日時</value>
            public DateTime? EraserUpdateDate { get; set; }
            /// <summary>Gets or sets 消込担当者ID</summary>
            /// <value>消込担当者ID</value>
            public string EraseUserId { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 請求先コード</summary>
                /// <value>請求先コード</value>
                public string InvoiceCd { get; set; }
                /// <summary>Gets or sets データ種別</summary>
                /// <value>データ種別</value>
                public int DataType { get; set; }
                /// <summary>Gets or sets 売上番号</summary>
                /// <value>売上番号</value>
                public string SalesNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="invoiceCd">請求先コード</param>
                /// <param name="dataType">データ種別</param>
                /// <param name="salesNo">売上番号</param>
                public PrimaryKey(string invoiceCd, int dataType, string salesNo)
                {
                    InvoiceCd = invoiceCd;
                    DataType = dataType;
                    SalesNo = salesNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InvoiceCd, this.DataType, this.SalesNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="invoiceCd">請求先コード</param>
            /// <param name="dataType">データ種別</param>
            /// <param name="salesNo">売上番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public EraserCsmCreditEntity GetEntity(string invoiceCd, int dataType, string salesNo, ComDB db)
            {
                EraserCsmCreditEntity.PrimaryKey condition = new EraserCsmCreditEntity.PrimaryKey(invoiceCd, dataType, salesNo);
                return GetEntity<EraserCsmCreditEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 支払消込トランザクション
        /// </summary>
        public class EraserCsmPaymentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public EraserCsmPaymentEntity()
            {
                TableName = "eraser_csm_payment";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime PayableDate { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets データ種別</summary>
            /// <value>データ種別</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets 仕入番号</summary>
            /// <value>仕入番号</value>
            public string StockingNo { get; set; }
            /// <summary>Gets or sets 消込ステータス|各種名称マスタ</summary>
            /// <value>消込ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 仕入日付</summary>
            /// <value>仕入日付</value>
            public DateTime StockingDate { get; set; }
            /// <summary>Gets or sets 消込対象額</summary>
            /// <value>消込対象額</value>
            public decimal EraserObjectAmount { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal EraserAmount { get; set; }
            /// <summary>Gets or sets 相殺消込額</summary>
            /// <value>相殺消込額</value>
            public decimal EraserOffsetAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal EraserBalanceAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ</summary>
            /// <value>消込完了フラグ</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 消込日付</summary>
            /// <value>消込日付</value>
            public DateTime? EraserDate { get; set; }
            /// <summary>Gets or sets 消込更新日時</summary>
            /// <value>消込更新日時</value>
            public DateTime? EraserUpdateDate { get; set; }
            /// <summary>Gets or sets 消込担当者ID</summary>
            /// <value>消込担当者ID</value>
            public string EraseUserId { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }
            /// <summary>Gets or sets 支払日</summary>
            /// <value>支払日</value>
            public DateTime? PaymentDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 支払先コード</summary>
                /// <value>支払先コード</value>
                public string SupplierCd { get; set; }
                /// <summary>Gets or sets データ種別</summary>
                /// <value>データ種別</value>
                public int DataType { get; set; }
                /// <summary>Gets or sets 仕入番号</summary>
                /// <value>仕入番号</value>
                public string StockingNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="supplierCd">支払先コード</param>
                /// <param name="dataType">データ種別</param>
                /// <param name="stockingNo">仕入番号</param>
                public PrimaryKey(string supplierCd, int dataType, string stockingNo)
                {
                    SupplierCd = supplierCd;
                    DataType = dataType;
                    StockingNo = stockingNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SupplierCd, this.DataType, this.StockingNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="supplierCd">支払先コード</param>
            /// <param name="dataType">データ種別</param>
            /// <param name="stockingNo">仕入番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public EraserCsmPaymentEntity GetEntity(string supplierCd, int dataType, string stockingNo, ComDB db)
            {
                EraserCsmPaymentEntity.PrimaryKey condition = new EraserCsmPaymentEntity.PrimaryKey(supplierCd, dataType, stockingNo);
                return GetEntity<EraserCsmPaymentEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 消込ログ
        /// </summary>
        public class EraserHeaderDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public EraserHeaderDetailEntity()
            {
                TableName = "eraser_header_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 消込番号</summary>
            /// <value>消込番号</value>
            public string EraserNo { get; set; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 入金番号</summary>
            /// <value>入金番号</value>
            public string CreditNo { get; set; }
            /// <summary>Gets or sets 入金行番号</summary>
            /// <value>入金行番号</value>
            public int CreditRowNo { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 売上消込完了フラグ|0</summary>
            /// <value>売上消込完了フラグ|0</value>
            public int? SalesEraserDivision { get; set; }
            /// <summary>Gets or sets 入金消込完了フラグ|0</summary>
            /// <value>入金消込完了フラグ|0</value>
            public int? CreditEraserDivision { get; set; }
            /// <summary>Gets or sets 消込日付</summary>
            /// <value>消込日付</value>
            public DateTime? EraserDate { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 消込番号</summary>
                /// <value>消込番号</value>
                public string EraserNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="eraserNo">消込番号</param>
                public PrimaryKey(string eraserNo)
                {
                    EraserNo = eraserNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.EraserNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="eraserNo">消込番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public EraserHeaderDetailEntity GetEntity(string eraserNo, ComDB db)
            {
                EraserHeaderDetailEntity.PrimaryKey condition = new EraserHeaderDetailEntity.PrimaryKey(eraserNo);
                return GetEntity<EraserHeaderDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 入金消込ログ
        /// </summary>
        public class EraserDetailCreditEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public EraserDetailCreditEntity()
            {
                TableName = "eraser_detail_credit";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 消込番号</summary>
            /// <value>消込番号</value>
            public string EraserNo { get; set; }
            /// <summary>Gets or sets 消込ステータス|各種名称マスタ</summary>
            /// <value>消込ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 入金番号</summary>
            /// <value>入金番号</value>
            public string CreditNo { get; set; }
            /// <summary>Gets or sets 入金行番号</summary>
            /// <value>入金行番号</value>
            public int? CreditRowNo { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込日付</summary>
            /// <value>消込日付</value>
            public DateTime? EraserDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 消込番号</summary>
                /// <value>消込番号</value>
                public string EraserNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="eraserNo">消込番号</param>
                public PrimaryKey(string eraserNo)
                {
                    EraserNo = eraserNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.EraserNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="eraserNo">消込番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public EraserDetailCreditEntity GetEntity(string eraserNo, ComDB db)
            {
                EraserDetailCreditEntity.PrimaryKey condition = new EraserDetailCreditEntity.PrimaryKey(eraserNo);
                return GetEntity<EraserDetailCreditEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 相殺消込ログ
        /// </summary>
        public class EraserDetailOffsetEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public EraserDetailOffsetEntity()
            {
                TableName = "eraser_detail_offset";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 消込番号</summary>
            /// <value>消込番号</value>
            public string EraserNo { get; set; }
            /// <summary>Gets or sets 消込ステータス|各種名称マスタ</summary>
            /// <value>消込ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 仕入番号</summary>
            /// <value>仕入番号</value>
            public string StockingNo { get; set; }
            /// <summary>Gets or sets 相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込日付</summary>
            /// <value>消込日付</value>
            public DateTime? EraserDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 消込番号</summary>
                /// <value>消込番号</value>
                public string EraserNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="eraserNo">消込番号</param>
                public PrimaryKey(string eraserNo)
                {
                    EraserNo = eraserNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.EraserNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="eraserNo">消込番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public EraserDetailOffsetEntity GetEntity(string eraserNo, ComDB db)
            {
                EraserDetailOffsetEntity.PrimaryKey condition = new EraserDetailOffsetEntity.PrimaryKey(eraserNo);
                return GetEntity<EraserDetailOffsetEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 支払消込ログ
        /// </summary>
        public class EraserDetailPaymentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public EraserDetailPaymentEntity()
            {
                TableName = "eraser_detail_payment";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 消込番号</summary>
            /// <value>消込番号</value>
            public string EraserNo { get; set; }
            /// <summary>Gets or sets 消込ステータス|各種名称マスタ</summary>
            /// <value>消込ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 仕入番号</summary>
            /// <value>仕入番号</value>
            public string StockingNo { get; set; }
            /// <summary>Gets or sets 支払伝票番号</summary>
            /// <value>支払伝票番号</value>
            public string PaymentSlipNo { get; set; }
            /// <summary>Gets or sets 支払伝票行番号</summary>
            /// <value>支払伝票行番号</value>
            public int? PaymentSlipRowNo { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込日付</summary>
            /// <value>消込日付</value>
            public DateTime? EraserDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 消込番号</summary>
                /// <value>消込番号</value>
                public string EraserNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="eraserNo">消込番号</param>
                public PrimaryKey(string eraserNo)
                {
                    EraserNo = eraserNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.EraserNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="eraserNo">消込番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public EraserDetailPaymentEntity GetEntity(string eraserNo, ComDB db)
            {
                EraserDetailPaymentEntity.PrimaryKey condition = new EraserDetailPaymentEntity.PrimaryKey(eraserNo);
                return GetEntity<EraserDetailPaymentEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ガジェットマスタ
        /// </summary>
        public class GadgetEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public GadgetEntity()
            {
                TableName = "gadget";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ガジェットID</summary>
            /// <value>ガジェットID</value>
            public string GadgetId { get; set; }
            /// <summary>Gets or sets メニューID</summary>
            /// <value>メニューID</value>
            public long MenuId { get; set; }
            /// <summary>Gets or sets タブID</summary>
            /// <value>タブID</value>
            public long TabId { get; set; }
            /// <summary>Gets or sets 操作ID</summary>
            /// <value>操作ID</value>
            public long CtrlId { get; set; }
            /// <summary>Gets or sets タイトル</summary>
            /// <value>タイトル</value>
            public string Title { get; set; }
            /// <summary>Gets or sets タイトルURL</summary>
            /// <value>タイトルURL</value>
            public string TitleUrl { get; set; }
            /// <summary>Gets or sets アクションURL</summary>
            /// <value>アクションURL</value>
            public string ActionUrl { get; set; }
            /// <summary>Gets or sets 検索実行SQL</summary>
            /// <value>検索実行SQL</value>
            public string SqlCd { get; set; }
            /// <summary>Gets or sets ガジェットに表示する項目の最大値</summary>
            /// <value>ガジェットに表示する項目の最大値</value>
            public int? MaxLine { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets メニューID</summary>
                /// <value>メニューID</value>
                public long MenuId { get; set; }
                /// <summary>Gets or sets タブID</summary>
                /// <value>タブID</value>
                public long TabId { get; set; }
                /// <summary>Gets or sets 操作ID</summary>
                /// <value>操作ID</value>
                public long CtrlId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="menuId">メニューID</param>
                /// <param name="ctrlId">タブID</param>
                /// <param name="tabId">操作ID</param>
                public PrimaryKey(long menuId, long tabId, long ctrlId)
                {
                    MenuId = menuId;
                    TabId = tabId;
                    CtrlId = ctrlId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MenuId, this.TabId, this.CtrlId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="menuId">メニューID</param>
            /// <param name="tabId">操作ID</param>
            /// <param name="ctrlId">タブID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public GadgetEntity GetEntity(long menuId, long tabId, long ctrlId, ComDB db)
            {
                GadgetEntity.PrimaryKey condition = new GadgetEntity.PrimaryKey(menuId, tabId, ctrlId);
                return GetEntity<GadgetEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ガジェットマスタ_担当者別設定
        /// </summary>
        public class GadgetConfigEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public GadgetConfigEntity()
            {
                TableName = "gadget_config";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets メニューID</summary>
            /// <value>メニューID</value>
            public long MenuId { get; set; }
            /// <summary>Gets or sets タブID</summary>
            /// <value>タブID</value>
            public long TabId { get; set; }
            /// <summary>Gets or sets ガジェットID</summary>
            /// <value>ガジェットID</value>
            public string GadgetId { get; set; }
            /// <summary>Gets or sets レーン番号</summary>
            /// <value>レーン番号</value>
            public int LaneNo { get; set; }
            /// <summary>Gets or sets 順番</summary>
            /// <value>順番</value>
            public int VerticalOrder { get; set; }
            /// <summary>Gets or sets 表示状態|0</summary>
            /// <value>表示状態|0</value>
            public int? SlideStatus { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 担当者コード</summary>
                /// <value>担当者コード</value>
                public string UserId { get; set; }
                /// <summary>Gets or sets メニューID</summary>
                /// <value>メニューID</value>
                public long MenuId { get; set; }
                /// <summary>Gets or sets タブID</summary>
                /// <value>タブID</value>
                public long TabId { get; set; }
                /// <summary>Gets or sets ガジェットID</summary>
                /// <value>ガジェットID</value>
                public string GadgetId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="userId">担当者コード</param>
                /// <param name="menuId">メニューID</param>
                /// <param name="tabId">タブID</param>
                /// <param name="gadgetId">ガジェットID</param>
                public PrimaryKey(string userId, long menuId, long tabId, string gadgetId)
                {
                    UserId = userId;
                    MenuId = menuId;
                    TabId = tabId;
                    GadgetId = gadgetId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.UserId, this.MenuId, this.TabId, this.GadgetId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="userId">担当者コード</param>
            /// <param name="menuId">メニューID</param>
            /// <param name="tabId">タブID</param>
            /// <param name="gadgetId">ガジェットID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public GadgetConfigEntity GetEntity(string userId, long menuId, long tabId, string gadgetId, ComDB db)
            {
                GadgetConfigEntity.PrimaryKey condition = new GadgetConfigEntity.PrimaryKey(userId, menuId, tabId, gadgetId);
                return GetEntity<GadgetConfigEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ガジェット・メニューロール組み合わせマスタ
        /// </summary>
        public class GadgetMenuRoleEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public GadgetMenuRoleEntity()
            {
                TableName = "gadget_menu_role";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets メニューロールID</summary>
            /// <value>メニューロールID</value>
            public int MenuRoleId { get; set; }
            /// <summary>Gets or sets ガジェットID</summary>
            /// <value>ガジェットID</value>
            public string GadgetId { get; set; }
            /// <summary>Gets or sets AUTHORITY区分</summary>
            /// <value>AUTHORITY区分</value>
            public string AuthorityDivision { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets メニューロールID</summary>
                /// <value>メニューロールID</value>
                public int MenuRoleId { get; set; }
                /// <summary>Gets or sets ガジェットID</summary>
                /// <value>ガジェットID</value>
                public string GadgetId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="menuRoleId">メニューロールID</param>
                /// <param name="gadgetId">ガジェットID</param>
                public PrimaryKey(int menuRoleId, string gadgetId)
                {
                    MenuRoleId = menuRoleId;
                    GadgetId = gadgetId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MenuRoleId, this.GadgetId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="menuRoleId">メニューロールID</param>
            /// <param name="gadgetId">ガジェットID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public GadgetMenuRoleEntity GetEntity(int menuRoleId, string gadgetId, ComDB db)
            {
                GadgetMenuRoleEntity.PrimaryKey condition = new GadgetMenuRoleEntity.PrimaryKey(menuRoleId, gadgetId);
                return GetEntity<GadgetMenuRoleEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// IF仕訳データ
        /// </summary>
        public class IfShiwakeEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public IfShiwakeEntity()
            {
                TableName = "if_shiwake";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int? Seqno { get; set; }
            /// <summary>Gets or sets ファイル区分</summary>
            /// <value>ファイル区分</value>
            public string Flk { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string Kjn { get; set; }
            /// <summary>Gets or sets 連結コード</summary>
            /// <value>連結コード</value>
            public string Rkc { get; set; }
            /// <summary>Gets or sets 転記日付</summary>
            /// <value>転記日付</value>
            public string Ymd { get; set; }
            /// <summary>Gets or sets 支払基準日</summary>
            /// <value>支払基準日</value>
            public string Skb { get; set; }
            /// <summary>Gets or sets 借方部門科目</summary>
            /// <value>借方部門科目</value>
            public string Dbmk { get; set; }
            /// <summary>Gets or sets 借方勘定科目</summary>
            /// <value>借方勘定科目</value>
            public string Dkkm { get; set; }
            /// <summary>Gets or sets 借方取引先</summary>
            /// <value>借方取引先</value>
            public string Dths { get; set; }
            /// <summary>Gets or sets 貸方部門科目</summary>
            /// <value>貸方部門科目</value>
            public string Cbmk { get; set; }
            /// <summary>Gets or sets 貸方勘定科目</summary>
            /// <value>貸方勘定科目</value>
            public string Ckkm { get; set; }
            /// <summary>Gets or sets 貸方取引先</summary>
            /// <value>貸方取引先</value>
            public string Cths { get; set; }
            /// <summary>Gets or sets 銀行</summary>
            /// <value>銀行</value>
            public string Bnk { get; set; }
            /// <summary>Gets or sets 税計算対象区分</summary>
            /// <value>税計算対象区分</value>
            public string Zeik { get; set; }
            /// <summary>Gets or sets 金額</summary>
            /// <value>金額</value>
            public string Kin3 { get; set; }
            /// <summary>Gets or sets 借方消費税コード(税コード)</summary>
            /// <value>借方消費税コード(税コード)</value>
            public string Dmwskz { get; set; }
            /// <summary>Gets or sets 貸方消費税コード(税コード)</summary>
            /// <value>貸方消費税コード(税コード)</value>
            public string Cmwskz { get; set; }
            /// <summary>Gets or sets 送信済フラグ</summary>
            /// <value>送信済フラグ</value>
            public string SendFlg { get; set; }
            /// <summary>Gets or sets 指図(製造)番号</summary>
            /// <value>指図(製造)番号</value>
            public string Sashizuno { get; set; }
            /// <summary>Gets or sets 指図完了フラグ</summary>
            /// <value>指図完了フラグ</value>
            public string Sashizukanryoflg { get; set; }
            /// <summary>Gets or sets 仕訳パタンコード</summary>
            /// <value>仕訳パタンコード</value>
            public string Cd { get; set; }
            /// <summary>Gets or sets 仕訳パタン仕訳区分</summary>
            /// <value>仕訳パタン仕訳区分</value>
            public string Division { get; set; }
            /// <summary>Gets or sets 入金用MOコード</summary>
            /// <value>入金用MOコード</value>
            public string Mocd { get; set; }
            /// <summary>Gets or sets サイト</summary>
            /// <value>サイト</value>
            public string Site { get; set; }
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public string Userid { get; set; }
            /// <summary>Gets or sets 登録日付</summary>
            /// <value>登録日付</value>
            public DateTime? Insymd { get; set; }
            /// <summary>Gets or sets 更新日付</summary>
            /// <value>更新日付</value>
            public DateTime? Updymd { get; set; }
        }

        /// <summary>
        /// 原価仕訳連携データ
        /// </summary>
        public class IfShiwakeGenkaEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public IfShiwakeGenkaEntity()
            {
                TableName = "if_shiwake_genka";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 処理区分</summary>
            /// <value>処理区分</value>
            public string XShoridivision { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string Kjn { get; set; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public long Seqno { get; set; }
            /// <summary>Gets or sets ファイル区分</summary>
            /// <value>ファイル区分</value>
            public string Flk { get; set; }
            /// <summary>Gets or sets 連結コード</summary>
            /// <value>連結コード</value>
            public string Rck { get; set; }
            /// <summary>Gets or sets 転記日付</summary>
            /// <value>転記日付</value>
            public string Ymd { get; set; }
            /// <summary>Gets or sets 支払基準日</summary>
            /// <value>支払基準日</value>
            public string Skb { get; set; }
            /// <summary>Gets or sets 借方部門科目</summary>
            /// <value>借方部門科目</value>
            public string Dbmk { get; set; }
            /// <summary>Gets or sets 借方勘定科目</summary>
            /// <value>借方勘定科目</value>
            public string Dkkm { get; set; }
            /// <summary>Gets or sets 借方取引先</summary>
            /// <value>借方取引先</value>
            public string Dths { get; set; }
            /// <summary>Gets or sets 貸方部門科目</summary>
            /// <value>貸方部門科目</value>
            public string Cbmk { get; set; }
            /// <summary>Gets or sets 貸方勘定科目</summary>
            /// <value>貸方勘定科目</value>
            public string Ckkm { get; set; }
            /// <summary>Gets or sets 貸方取引先</summary>
            /// <value>貸方取引先</value>
            public string Cths { get; set; }
            /// <summary>Gets or sets 税計算対象区分</summary>
            /// <value>税計算対象区分</value>
            public string Zeik { get; set; }
            /// <summary>Gets or sets 銀行</summary>
            /// <value>銀行</value>
            public string Bnk { get; set; }
            /// <summary>Gets or sets 金額</summary>
            /// <value>金額</value>
            public string Kin3 { get; set; }
            /// <summary>Gets or sets 指図(製造)番号</summary>
            /// <value>指図(製造)番号</value>
            public string Sashizuno { get; set; }
            /// <summary>Gets or sets 指図完了フラグ|0</summary>
            /// <value>指図完了フラグ|0</value>
            public string Sashizukanryoflg { get; set; }
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public string Userid { get; set; }
            /// <summary>Gets or sets 登録日付</summary>
            /// <value>登録日付</value>
            public DateTime? Insymd { get; set; }
            /// <summary>Gets or sets 更新日付</summary>
            /// <value>更新日付</value>
            public DateTime? Updymd { get; set; }
            /// <summary>Gets or sets 伝票番号</summary>
            /// <value>伝票番号</value>
            public string Denno { get; set; }
            /// <summary>Gets or sets acdivision</summary>
            /// <value>acdivision</value>
            public int? Acdivision { get; set; }
            /// <summary>Gets or sets iodivision</summary>
            /// <value>iodivision</value>
            public int? Iodivision { get; set; }
            /// <summary>Gets or sets kjnb</summary>
            /// <value>kjnb</value>
            public string Kjnb { get; set; }
            /// <summary>Gets or sets seqnob</summary>
            /// <value>seqnob</value>
            public long? Seqnob { get; set; }
            /// <summary>Gets or sets 借方消費税コード（税コード）</summary>
            /// <value>借方消費税コード（税コード）</value>
            public string Dmwskz { get; set; }
            /// <summary>Gets or sets 貸方消費税コード（税コード）</summary>
            /// <value>貸方消費税コード（税コード）</value>
            public string Cmwskz { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 処理区分</summary>
                /// <value>処理区分</value>
                public string XShoridivision { get; set; }
                /// <summary>Gets or sets 勘定年月</summary>
                /// <value>勘定年月</value>
                public string Kjn { get; set; }
                /// <summary>Gets or sets 連番</summary>
                /// <value>連番</value>
                public long Seqno { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="shoridivision">処理区分</param>
                /// <param name="kjn">勘定年月</param>
                /// <param name="seqno">連番</param>
                public PrimaryKey(string shoridivision, string kjn, long seqno)
                {
                    XShoridivision = shoridivision;
                    Kjn = kjn;
                    Seqno = seqno;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.XShoridivision, this.Kjn, this.Seqno);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="shoridivision">処理区分</param>
            /// <param name="kjn">勘定年月</param>
            /// <param name="seqno">連番</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public IfShiwakeGenkaEntity GetEntity(string shoridivision, string kjn, long seqno, ComDB db)
            {
                IfShiwakeGenkaEntity.PrimaryKey condition = new IfShiwakeGenkaEntity.PrimaryKey(shoridivision, kjn, seqno);
                return GetEntity<IfShiwakeGenkaEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仕訳送信履歴
        /// </summary>
        public class IfShiwakeHistEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public IfShiwakeHistEntity()
            {
                TableName = "if_shiwake_hist";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 履歴番号</summary>
            /// <value>履歴番号</value>
            public int RevNo { get; set; }
            /// <summary>Gets or sets 仕訳区分|0</summary>
            /// <value>仕訳区分|0</value>
            public int? DivisionNo { get; set; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets ファイル区分</summary>
            /// <value>ファイル区分</value>
            public string Flk { get; set; }
            /// <summary>Gets or sets 勘定年月|YYMM</summary>
            /// <value>勘定年月|YYMM</value>
            public string Kjn { get; set; }
            /// <summary>Gets or sets 連結コード|UA1~6</summary>
            /// <value>連結コード|UA1~6</value>
            public string Rkc { get; set; }
            /// <summary>Gets or sets dhb</summary>
            /// <value>dhb</value>
            public string Dhb { get; set; }
            /// <summary>Gets or sets msn</summary>
            /// <value>msn</value>
            public string Msn { get; set; }
            /// <summary>Gets or sets 転記日付</summary>
            /// <value>転記日付</value>
            public string Ymd { get; set; }
            /// <summary>Gets or sets 支払基準日</summary>
            /// <value>支払基準日</value>
            public string Skb { get; set; }
            /// <summary>Gets or sets 借方部門科目</summary>
            /// <value>借方部門科目</value>
            public string Dbmk { get; set; }
            /// <summary>Gets or sets 借方勘定科目</summary>
            /// <value>借方勘定科目</value>
            public string Dkkm { get; set; }
            /// <summary>Gets or sets 借方取引先</summary>
            /// <value>借方取引先</value>
            public string Dths { get; set; }
            /// <summary>Gets or sets dysk</summary>
            /// <value>dysk</value>
            public string Dysk { get; set; }
            /// <summary>Gets or sets dkrk</summary>
            /// <value>dkrk</value>
            public string Dkrk { get; set; }
            /// <summary>Gets or sets dkrn</summary>
            /// <value>dkrn</value>
            public string Dkrn { get; set; }
            /// <summary>Gets or sets dkre</summary>
            /// <value>dkre</value>
            public string Dkre { get; set; }
            /// <summary>Gets or sets dnsk</summary>
            /// <value>dnsk</value>
            public string Dnsk { get; set; }
            /// <summary>Gets or sets dtht</summary>
            /// <value>dtht</value>
            public string Dtht { get; set; }
            /// <summary>Gets or sets yobi1</summary>
            /// <value>yobi1</value>
            public string Yobi1 { get; set; }
            /// <summary>Gets or sets 貸方部門科目</summary>
            /// <value>貸方部門科目</value>
            public string Cbmk { get; set; }
            /// <summary>Gets or sets 貸方勘定科目</summary>
            /// <value>貸方勘定科目</value>
            public string Ckkm { get; set; }
            /// <summary>Gets or sets 貸方取引先</summary>
            /// <value>貸方取引先</value>
            public string Cths { get; set; }
            /// <summary>Gets or sets cysk</summary>
            /// <value>cysk</value>
            public string Cysk { get; set; }
            /// <summary>Gets or sets ckrk</summary>
            /// <value>ckrk</value>
            public string Ckrk { get; set; }
            /// <summary>Gets or sets ckrn</summary>
            /// <value>ckrn</value>
            public string Ckrn { get; set; }
            /// <summary>Gets or sets ckre</summary>
            /// <value>ckre</value>
            public string Ckre { get; set; }
            /// <summary>Gets or sets cnsk</summary>
            /// <value>cnsk</value>
            public string Cnsk { get; set; }
            /// <summary>Gets or sets ctht</summary>
            /// <value>ctht</value>
            public string Ctht { get; set; }
            /// <summary>Gets or sets yobi2</summary>
            /// <value>yobi2</value>
            public string Yobi2 { get; set; }
            /// <summary>Gets or sets 税計算対象区分</summary>
            /// <value>税計算対象区分</value>
            public string Zeik { get; set; }
            /// <summary>Gets or sets 銀行</summary>
            /// <value>銀行</value>
            public string Bnk { get; set; }
            /// <summary>Gets or sets renno</summary>
            /// <value>renno</value>
            public string Renno { get; set; }
            /// <summary>Gets or sets smc</summary>
            /// <value>smc</value>
            public string Smc { get; set; }
            /// <summary>Gets or sets skey</summary>
            /// <value>skey</value>
            public string Skey { get; set; }
            /// <summary>Gets or sets nshh</summary>
            /// <value>nshh</value>
            public string Nshh { get; set; }
            /// <summary>Gets or sets ksk</summary>
            /// <value>ksk</value>
            public string Ksk { get; set; }
            /// <summary>Gets or sets sit</summary>
            /// <value>sit</value>
            public string Sit { get; set; }
            /// <summary>Gets or sets fdb</summary>
            /// <value>fdb</value>
            public string Fdb { get; set; }
            /// <summary>Gets or sets mkb</summary>
            /// <value>mkb</value>
            public string Mkb { get; set; }
            /// <summary>Gets or sets tno</summary>
            /// <value>tno</value>
            public string Tno { get; set; }
            /// <summary>Gets or sets sgai</summary>
            /// <value>sgai</value>
            public string Sgai { get; set; }
            /// <summary>Gets or sets krt</summary>
            /// <value>krt</value>
            public string Krt { get; set; }
            /// <summary>Gets or sets kymd</summary>
            /// <value>kymd</value>
            public string Kymd { get; set; }
            /// <summary>Gets or sets 金額</summary>
            /// <value>金額</value>
            public string Kin3 { get; set; }
            /// <summary>Gets or sets gai3</summary>
            /// <value>gai3</value>
            public string Gai3 { get; set; }
            /// <summary>Gets or sets rvr</summary>
            /// <value>rvr</value>
            public string Rvr { get; set; }
            /// <summary>Gets or sets sres</summary>
            /// <value>sres</value>
            public string Sres { get; set; }
            /// <summary>Gets or sets taxcds</summary>
            /// <value>taxcds</value>
            public string Taxcds { get; set; }
            /// <summary>Gets or sets taxcdh</summary>
            /// <value>taxcdh</value>
            public string Taxcdh { get; set; }
            /// <summary>Gets or sets taxrs</summary>
            /// <value>taxrs</value>
            public string Taxrs { get; set; }
            /// <summary>Gets or sets taxrh</summary>
            /// <value>taxrh</value>
            public string Taxrh { get; set; }
            /// <summary>Gets or sets din</summary>
            /// <value>din</value>
            public string Din { get; set; }
        }

        /// <summary>
        /// 勘定明細ＢＳ
        /// </summary>
        public class IfShiwakeRcvBsEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public IfShiwakeRcvBsEntity()
            {
                TableName = "if_shiwake_rcv_bs";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets カンパニーコード</summary>
            /// <value>カンパニーコード</value>
            public string CompanyCd { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string KanjoYm { get; set; }
            /// <summary>Gets or sets 会社コード</summary>
            /// <value>会社コード</value>
            public string KaisyaCd { get; set; }
            /// <summary>Gets or sets 事業領域</summary>
            /// <value>事業領域</value>
            public string JigyouArea { get; set; }
            /// <summary>Gets or sets 管理領域</summary>
            /// <value>管理領域</value>
            public string KanriArea { get; set; }
            /// <summary>Gets or sets 伝票の転記日付</summary>
            /// <value>伝票の転記日付</value>
            public DateTime? DenpyoTenkiDate { get; set; }
            /// <summary>Gets or sets 原価／利益センタ</summary>
            /// <value>原価／利益センタ</value>
            public string CostProfCenter { get; set; }
            /// <summary>Gets or sets 会計単位</summary>
            /// <value>会計単位</value>
            public string KaikeiTani { get; set; }
            /// <summary>Gets or sets 取引先</summary>
            /// <value>取引先</value>
            public string Torihikisaki6 { get; set; }
            /// <summary>Gets or sets 得意先／仕入先コード</summary>
            /// <value>得意先／仕入先コード</value>
            public string CustomerVenderCd { get; set; }
            /// <summary>Gets or sets 代表取引先</summary>
            /// <value>代表取引先</value>
            public string DaihyoTorihikisaki { get; set; }
            /// <summary>Gets or sets 勘定コード表</summary>
            /// <value>勘定コード表</value>
            public string AccountsCdTable { get; set; }
            /// <summary>Gets or sets 勘定コード</summary>
            /// <value>勘定コード</value>
            public string AccountsCd { get; set; }
            /// <summary>Gets or sets 大科目</summary>
            /// <value>大科目</value>
            public string Daikamoku { get; set; }
            /// <summary>Gets or sets 小科目</summary>
            /// <value>小科目</value>
            public string Syoukamoku { get; set; }
            /// <summary>Gets or sets 連結コード</summary>
            /// <value>連結コード</value>
            public string LinkingCd { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string KanriDivision { get; set; }
            /// <summary>Gets or sets 管理NO</summary>
            /// <value>管理NO</value>
            public string KanriNo { get; set; }
            /// <summary>Gets or sets 入手元区分</summary>
            /// <value>入手元区分</value>
            public string NyuusyumotoDivision { get; set; }
            /// <summary>Gets or sets 当月金額</summary>
            /// <value>当月金額</value>
            public long? TougetsuKingaku { get; set; }
            /// <summary>Gets or sets 残高</summary>
            /// <value>残高</value>
            public long? Zandaka { get; set; }
            /// <summary>Gets or sets 更新ＩＤ</summary>
            /// <value>更新ＩＤ</value>
            public string RenewalId { get; set; }
            /// <summary>Gets or sets 更新年月日</summary>
            /// <value>更新年月日</value>
            public DateTime? RenewalDate { get; set; }
        }

        /// <summary>
        /// 一件別仕訳明細
        /// </summary>
        public class IfShiwakeRcvDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public IfShiwakeRcvDetailEntity()
            {
                TableName = "if_shiwake_rcv_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets クライアント</summary>
            /// <value>クライアント</value>
            public string Client { get; set; }
            /// <summary>Gets or sets 会社コード</summary>
            /// <value>会社コード</value>
            public string KaisyaCd { get; set; }
            /// <summary>Gets or sets 会計年度</summary>
            /// <value>会計年度</value>
            public int KaikeiY { get; set; }
            /// <summary>Gets or sets 会計伝票番号</summary>
            /// <value>会計伝票番号</value>
            public string KaidenBan { get; set; }
            /// <summary>Gets or sets 会計伝票番内の明細番号</summary>
            /// <value>会計伝票番内の明細番号</value>
            public int KaidenMeisaiBan { get; set; }
            /// <summary>Gets or sets 会計伝票登録日付時刻</summary>
            /// <value>会計伝票登録日付時刻</value>
            public string KaidenTourokuDate { get; set; }
            /// <summary>Gets or sets 登録区分</summary>
            /// <value>登録区分</value>
            public string TourokuDivision { get; set; }
            /// <summary>Gets or sets 伝票タイプ</summary>
            /// <value>伝票タイプ</value>
            public string DenpyoType { get; set; }
            /// <summary>Gets or sets 伝票日付</summary>
            /// <value>伝票日付</value>
            public string DenpyoDate { get; set; }
            /// <summary>Gets or sets 伝票の転記日付</summary>
            /// <value>伝票の転記日付</value>
            public string DenpyoTenkiDate { get; set; }
            /// <summary>Gets or sets 伝票ＮＯ</summary>
            /// <value>伝票ＮＯ</value>
            public string DenpyoNo { get; set; }
            /// <summary>Gets or sets 勘定コード表</summary>
            /// <value>勘定コード表</value>
            public string AccountsCdTable { get; set; }
            /// <summary>Gets or sets 勘定コード</summary>
            /// <value>勘定コード</value>
            public string AccountsCd { get; set; }
            /// <summary>Gets or sets 大科目</summary>
            /// <value>大科目</value>
            public string Daikamoku { get; set; }
            /// <summary>Gets or sets 小科目</summary>
            /// <value>小科目</value>
            public string Syoukamoku { get; set; }
            /// <summary>Gets or sets フラグ</summary>
            /// <value>フラグ</value>
            public string BsAccountsFlg { get; set; }
            /// <summary>Gets or sets 損益計算書勘定タイプ</summary>
            /// <value>損益計算書勘定タイプ</value>
            public string PlKanjoType { get; set; }
            /// <summary>Gets or sets 借方/貸方フラグ</summary>
            /// <value>借方/貸方フラグ</value>
            public string KariKashiFlg { get; set; }
            /// <summary>Gets or sets 取引タイプ</summary>
            /// <value>取引タイプ</value>
            public string TorihikiType { get; set; }
            /// <summary>Gets or sets 転記キー</summary>
            /// <value>転記キー</value>
            public string TenkiKey { get; set; }
            /// <summary>Gets or sets 社員NO</summary>
            /// <value>社員NO</value>
            public string SyainNo { get; set; }
            /// <summary>Gets or sets 伝票発行社員NO</summary>
            /// <value>伝票発行社員NO</value>
            public string DenpyoHakkouSyainNo { get; set; }
            /// <summary>Gets or sets 伝票承認社員NO</summary>
            /// <value>伝票承認社員NO</value>
            public string DenpyoSyouninSyainNo { get; set; }
            /// <summary>Gets or sets 承認NO</summary>
            /// <value>承認NO</value>
            public string SyouninNo { get; set; }
            /// <summary>Gets or sets 事業領域</summary>
            /// <value>事業領域</value>
            public string JigyouArea { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string KanriDivision { get; set; }
            /// <summary>Gets or sets 管理NO</summary>
            /// <value>管理NO</value>
            public string KanriNo { get; set; }
            /// <summary>Gets or sets 管理NO枝番</summary>
            /// <value>管理NO枝番</value>
            public string KanriNoEda { get; set; }
            /// <summary>Gets or sets 管理区分集約ｺｰﾄﾞ</summary>
            /// <value>管理区分集約ｺｰﾄﾞ</value>
            public string ManagementDivisionAggregationCd { get; set; }
            /// <summary>Gets or sets 発行部門科目NO</summary>
            /// <value>発行部門科目NO</value>
            public string HakkouBumonNo { get; set; }
            /// <summary>Gets or sets 流通チャネル</summary>
            /// <value>流通チャネル</value>
            public string RyuutsuuChannel { get; set; }
            /// <summary>Gets or sets 管理領域</summary>
            /// <value>管理領域</value>
            public string KanriArea { get; set; }
            /// <summary>Gets or sets 会計単位</summary>
            /// <value>会計単位</value>
            public string KaikeiTani { get; set; }
            /// <summary>Gets or sets 原価センタタイプ</summary>
            /// <value>原価センタタイプ</value>
            public string CostCenterType { get; set; }
            /// <summary>Gets or sets 原価センタ</summary>
            /// <value>原価センタ</value>
            public string CostCenter { get; set; }
            /// <summary>Gets or sets 利益センタ</summary>
            /// <value>利益センタ</value>
            public string ProfCenter { get; set; }
            /// <summary>Gets or sets 仕入先</summary>
            /// <value>仕入先</value>
            public string Shiiresaki { get; set; }
            /// <summary>Gets or sets 得意先</summary>
            /// <value>得意先</value>
            public string Tokuisaki { get; set; }
            /// <summary>Gets or sets 取引先（6桁）</summary>
            /// <value>取引先（6桁）</value>
            public string Torihikisaki6 { get; set; }
            /// <summary>Gets or sets 取引先</summary>
            /// <value>取引先</value>
            public string Torihikisaki { get; set; }
            /// <summary>Gets or sets 代表取引先NO.</summary>
            /// <value>代表取引先NO.</value>
            public string DaihyoTorihikisakiNo { get; set; }
            /// <summary>Gets or sets 相殺対象フラグ</summary>
            /// <value>相殺対象フラグ</value>
            public string OffsetTargetFlg { get; set; }
            /// <summary>Gets or sets 論理システム</summary>
            /// <value>論理システム</value>
            public string RonriSystem { get; set; }
            /// <summary>Gets or sets 税コード</summary>
            /// <value>税コード</value>
            public string TaxCd { get; set; }
            /// <summary>Gets or sets 明細テキスト</summary>
            /// <value>明細テキスト</value>
            public string MeisaiText { get; set; }
            /// <summary>Gets or sets 年月日</summary>
            /// <value>年月日</value>
            public string Nengappi { get; set; }
            /// <summary>Gets or sets 連結コード</summary>
            /// <value>連結コード</value>
            public string LinkingCd { get; set; }
            /// <summary>Gets or sets 配賦規則</summary>
            /// <value>配賦規則</value>
            public string HaifuRule { get; set; }
            /// <summary>Gets or sets 配賦規則パターン</summary>
            /// <value>配賦規則パターン</value>
            public int? HaifuRulePattern { get; set; }
            /// <summary>Gets or sets 機能区分</summary>
            /// <value>機能区分</value>
            public string KinouDivision { get; set; }
            /// <summary>Gets or sets 計画レベル</summary>
            /// <value>計画レベル</value>
            public string KeikakuLevel { get; set; }
            /// <summary>Gets or sets 計画グループ</summary>
            /// <value>計画グループ</value>
            public string KeikakuGroup { get; set; }
            /// <summary>Gets or sets 事業領域(取引先)</summary>
            /// <value>事業領域(取引先)</value>
            public string JigyouAreaTorihikisaki { get; set; }
            /// <summary>Gets or sets 仕向国</summary>
            /// <value>仕向国</value>
            public string Shimukekoku { get; set; }
            /// <summary>Gets or sets 受払区分</summary>
            /// <value>受払区分</value>
            public string UkeharaiDivision { get; set; }
            /// <summary>Gets or sets 原価/利益センタ（相手）</summary>
            /// <value>原価/利益センタ（相手）</value>
            public string CostProfCenterAite { get; set; }
            /// <summary>Gets or sets 伝票種類</summary>
            /// <value>伝票種類</value>
            public string DenpyoSyurui { get; set; }
            /// <summary>Gets or sets 期日計算の支払基準日</summary>
            /// <value>期日計算の支払基準日</value>
            public string ShiharaiKijunDate { get; set; }
            /// <summary>Gets or sets 換算レート</summary>
            /// <value>換算レート</value>
            public decimal? KanzanRate { get; set; }
            /// <summary>Gets or sets 伝票通貨コード</summary>
            /// <value>伝票通貨コード</value>
            public string SlipMoneyCd { get; set; }
            /// <summary>Gets or sets 国内通貨コード</summary>
            /// <value>国内通貨コード</value>
            public string DomesticMoneyCd { get; set; }
            /// <summary>Gets or sets 伝票通貨額</summary>
            /// <value>伝票通貨額</value>
            public decimal? DenpyoKingaku { get; set; }
            /// <summary>Gets or sets 国内通貨額</summary>
            /// <value>国内通貨額</value>
            public long? KokunaiKingaku { get; set; }
            /// <summary>Gets or sets 計上元伝票タイプ</summary>
            /// <value>計上元伝票タイプ</value>
            public string KeijyomotoDenpyoType { get; set; }
            /// <summary>Gets or sets 取引先会社コード</summary>
            /// <value>取引先会社コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 依頼元利益センタ</summary>
            /// <value>依頼元利益センタ</value>
            public string IraimotoProfCenter { get; set; }
            /// <summary>Gets or sets 依頼元会社</summary>
            /// <value>依頼元会社</value>
            public string IraimotoKaisya { get; set; }
            /// <summary>Gets or sets カンパニー伝票番号</summary>
            /// <value>カンパニー伝票番号</value>
            public string CompanyDenpyoBan { get; set; }
            /// <summary>Gets or sets 参照伝票番号</summary>
            /// <value>参照伝票番号</value>
            public string SansyouDenpyoBan { get; set; }
            /// <summary>Gets or sets 会計年度バリアント</summary>
            /// <value>会計年度バリアント</value>
            public string KaikeiYValiant { get; set; }
            /// <summary>Gets or sets 会計年度/期間</summary>
            /// <value>会計年度/期間</value>
            public int? KaikeiYKikan { get; set; }
            /// <summary>Gets or sets 会計期間</summary>
            /// <value>会計期間</value>
            public int? KaikeiKikan { get; set; }
            /// <summary>Gets or sets カレンダ年度</summary>
            /// <value>カレンダ年度</value>
            public int? CalenderY { get; set; }
            /// <summary>Gets or sets カレンダ年月</summary>
            /// <value>カレンダ年月</value>
            public int? CalenderYm { get; set; }
            /// <summary>Gets or sets カレンダ月</summary>
            /// <value>カレンダ月</value>
            public int? CalenderM { get; set; }
            /// <summary>Gets or sets カレンダ年/四半期</summary>
            /// <value>カレンダ年/四半期</value>
            public int? CalenderYQuarter { get; set; }
            /// <summary>Gets or sets 四半期</summary>
            /// <value>四半期</value>
            public string Quarter { get; set; }
            /// <summary>Gets or sets Halfyear</summary>
            /// <value>Halfyear</value>
            public string Halfyear { get; set; }
            /// <summary>Gets or sets 登録日付時刻</summary>
            /// <value>登録日付時刻</value>
            public string TourokuDate { get; set; }
            /// <summary>Gets or sets 登録者</summary>
            /// <value>登録者</value>
            public string TourokuName { get; set; }
            /// <summary>Gets or sets 入手元区分</summary>
            /// <value>入手元区分</value>
            public string NyuusyumotoDivision { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string KanjoYm { get; set; }
            /// <summary>Gets or sets カンパニーコード</summary>
            /// <value>カンパニーコード</value>
            public string CompanyCd { get; set; }
            /// <summary>Gets or sets 相手カンパニーコード</summary>
            /// <value>相手カンパニーコード</value>
            public string AiteCompanyCd { get; set; }
            /// <summary>Gets or sets 相手会社コード</summary>
            /// <value>相手会社コード</value>
            public string AiteKaisyaCd { get; set; }
            /// <summary>Gets or sets 相手事業領域</summary>
            /// <value>相手事業領域</value>
            public string AiteJigyouArea { get; set; }
            /// <summary>Gets or sets 相手原価／利益センタ</summary>
            /// <value>相手原価／利益センタ</value>
            public string AiteCostProfCenter { get; set; }
            /// <summary>Gets or sets 相手勘定コード</summary>
            /// <value>相手勘定コード</value>
            public string AiteAccountsCd { get; set; }
            /// <summary>Gets or sets 相手仕入先コード</summary>
            /// <value>相手仕入先コード</value>
            public string AiteVenderCd { get; set; }
            /// <summary>Gets or sets 相手管理区分</summary>
            /// <value>相手管理区分</value>
            public string AiteKanriDivision { get; set; }
            /// <summary>Gets or sets 相手管理NO</summary>
            /// <value>相手管理NO</value>
            public string AiteKanriNo { get; set; }
            /// <summary>Gets or sets 更新ＩＤ</summary>
            /// <value>更新ＩＤ</value>
            public string RenewalId { get; set; }
            /// <summary>Gets or sets 更新年月日</summary>
            /// <value>更新年月日</value>
            public string RenewalDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets クライアント</summary>
                /// <value>クライアント</value>
                public string Client { get; set; }
                /// <summary>Gets or sets 会社コード</summary>
                /// <value>会社コード</value>
                public string KaisyaCd { get; set; }
                /// <summary>Gets or sets 会計年度</summary>
                /// <value>会計年度</value>
                public int KaikeiY { get; set; }
                /// <summary>Gets or sets 会計伝票番号</summary>
                /// <value>会計伝票番号</value>
                public string KaidenBan { get; set; }
                /// <summary>Gets or sets 会計伝票番内の明細番号</summary>
                /// <value>会計伝票番内の明細番号</value>
                public int KaidenMeisaiBan { get; set; }
                /// <summary>Gets or sets 登録区分</summary>
                /// <value>登録区分</value>
                public string TourokuDivision { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="client">クライアント</param>
                /// <param name="kaisyaCd">会社コード</param>
                /// <param name="kaikeiY">会計年度</param>
                /// <param name="kaidenBan">会社伝票番号</param>
                /// <param name="kaidenMeisaiBan">会計伝票番内の明細番号</param>
                /// <param name="tourokuDivision">登録区分</param>
                public PrimaryKey(string client, string kaisyaCd, int kaikeiY, string kaidenBan, int kaidenMeisaiBan, string tourokuDivision)
                {
                    Client = client;
                    KaisyaCd = kaisyaCd;
                    KaikeiY = kaikeiY;
                    KaidenBan = kaidenBan;
                    KaidenMeisaiBan = kaidenMeisaiBan;
                    TourokuDivision = tourokuDivision;
                }
            }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.Client, this.KaisyaCd, this.KaikeiY, this.KaidenBan, this.KaidenMeisaiBan, this.TourokuDivision);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="client">クライアント</param>
            /// <param name="kaisyaCd">会社コード</param>
            /// <param name="kaikeiY">会計年度</param>
            /// <param name="kaidenBan">会社伝票番号</param>
            /// <param name="kaidenMeisaiBan">会計伝票番内の明細番号</param>
            /// <param name="tourokuDivision">登録区分</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public IfShiwakeRcvDetailEntity GetEntity(string client, string kaisyaCd, int kaikeiY, string kaidenBan, int kaidenMeisaiBan, string tourokuDivision, ComDB db)
            {
                IfShiwakeRcvDetailEntity.PrimaryKey condition = new IfShiwakeRcvDetailEntity.PrimaryKey(client, kaisyaCd, kaikeiY, kaidenBan, kaidenMeisaiBan, tourokuDivision);
                return GetEntity<IfShiwakeRcvDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 勘定明細ＰＬ
        /// </summary>
        public class IfShiwakeRcvPlEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public IfShiwakeRcvPlEntity()
            {
                TableName = "if_shiwake_rcv_pl";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets カンパニーコード</summary>
            /// <value>カンパニーコード</value>
            public string CompanyCd { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string KanjoYm { get; set; }
            /// <summary>Gets or sets 会社コード</summary>
            /// <value>会社コード</value>
            public string KaisyaCd { get; set; }
            /// <summary>Gets or sets 事業領域</summary>
            /// <value>事業領域</value>
            public string JigyouArea { get; set; }
            /// <summary>Gets or sets 管理領域</summary>
            /// <value>管理領域</value>
            public string KanriArea { get; set; }
            /// <summary>Gets or sets 伝票の転記日付</summary>
            /// <value>伝票の転記日付</value>
            public DateTime? DenpyoTenkiDate { get; set; }
            /// <summary>Gets or sets 原価／利益センタ</summary>
            /// <value>原価／利益センタ</value>
            public string CostProfCenter { get; set; }
            /// <summary>Gets or sets 会計単位</summary>
            /// <value>会計単位</value>
            public string KaikeiTani { get; set; }
            /// <summary>Gets or sets 取引先</summary>
            /// <value>取引先</value>
            public string Torihikisaki6 { get; set; }
            /// <summary>Gets or sets 得意先／仕入先コード</summary>
            /// <value>得意先／仕入先コード</value>
            public string CustomerVenderCd { get; set; }
            /// <summary>Gets or sets 代表取引先</summary>
            /// <value>代表取引先</value>
            public string DaihyoTorihikisaki { get; set; }
            /// <summary>Gets or sets 勘定コード表</summary>
            /// <value>勘定コード表</value>
            public string AccountsCdTable { get; set; }
            /// <summary>Gets or sets 勘定コード</summary>
            /// <value>勘定コード</value>
            public string AccountsCd { get; set; }
            /// <summary>Gets or sets 大科目</summary>
            /// <value>大科目</value>
            public string Daikamoku { get; set; }
            /// <summary>Gets or sets 小科目</summary>
            /// <value>小科目</value>
            public string Syoukamoku { get; set; }
            /// <summary>Gets or sets 連結コード</summary>
            /// <value>連結コード</value>
            public string LinkingCd { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string KanriDivision { get; set; }
            /// <summary>Gets or sets 管理NO</summary>
            /// <value>管理NO</value>
            public string KanriNo { get; set; }
            /// <summary>Gets or sets 入手元区分</summary>
            /// <value>入手元区分</value>
            public string NyuusyumotoDivision { get; set; }
            /// <summary>Gets or sets 当月金額</summary>
            /// <value>当月金額</value>
            public long? TougetsuKingaku { get; set; }
            /// <summary>Gets or sets 累計金額</summary>
            /// <value>累計金額</value>
            public long? RuikeiKingaku { get; set; }
            /// <summary>Gets or sets 更新ＩＤ</summary>
            /// <value>更新ＩＤ</value>
            public string RenewalId { get; set; }
            /// <summary>Gets or sets 更新年月日</summary>
            /// <value>更新年月日</value>
            public DateTime? RenewalDate { get; set; }
        }

        /// <summary>
        /// 新連結仕訳戻しファイル
        /// </summary>
        public class IfShiwakeReturnEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public IfShiwakeReturnEntity()
            {
                TableName = "if_shiwake_return";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets バージョン</summary>
            /// <value>バージョン</value>
            public string Versi { get; set; }
            /// <summary>Gets or sets 送信件数</summary>
            /// <value>送信件数</value>
            public int? Tsent { get; set; }
            /// <summary>Gets or sets 日時</summary>
            /// <value>日時</value>
            public string StartDate { get; set; }
            /// <summary>Gets or sets 伝票キー番号</summary>
            /// <value>伝票キー番号</value>
            public string Keyno { get; set; }
            /// <summary>Gets or sets 会計伝票番号1</summary>
            /// <value>会計伝票番号1</value>
            public int? Belnr { get; set; }
            /// <summary>Gets or sets 会計年度</summary>
            /// <value>会計年度</value>
            public int? Gjahr { get; set; }
            /// <summary>Gets or sets 会計期間</summary>
            /// <value>会計期間</value>
            public int? Monat { get; set; }
            /// <summary>Gets or sets 伝票タイプ</summary>
            /// <value>伝票タイプ</value>
            public string Blart { get; set; }
            /// <summary>Gets or sets 伝票日付</summary>
            /// <value>伝票日付</value>
            public int? Bldat { get; set; }
            /// <summary>Gets or sets 伝票の転記日付</summary>
            /// <value>伝票の転記日付</value>
            public int? Budat { get; set; }
            /// <summary>Gets or sets 参照伝票番号2</summary>
            /// <value>参照伝票番号2</value>
            public string Xblnr { get; set; }
            /// <summary>Gets or sets 換算日付</summary>
            /// <value>換算日付</value>
            public int? Wwert { get; set; }
            /// <summary>Gets or sets 伝票ヘッダテキスト</summary>
            /// <value>伝票ヘッダテキスト</value>
            public string Bktxt { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string Waers { get; set; }
            /// <summary>Gets or sets 換算レート</summary>
            /// <value>換算レート</value>
            public decimal? Kursf { get; set; }
            /// <summary>Gets or sets 参照伝票番号</summary>
            /// <value>参照伝票番号</value>
            public string Awref { get; set; }
            /// <summary>Gets or sets 参照組織単位</summary>
            /// <value>参照組織単位</value>
            public string Aworg { get; set; }
            /// <summary>Gets or sets 論理システム</summary>
            /// <value>論理システム</value>
            public string Awsys { get; set; }
            /// <summary>Gets or sets 会計伝票内の明細番号</summary>
            /// <value>会計伝票内の明細番号</value>
            public int? Buzei { get; set; }
            /// <summary>Gets or sets グローバル会社コード</summary>
            /// <value>グローバル会社コード</value>
            public string Bukrs { get; set; }
            /// <summary>Gets or sets 転記キー</summary>
            /// <value>転記キー</value>
            public string Bschl { get; set; }
            /// <summary>Gets or sets 借方／貸方フラグ</summary>
            /// <value>借方／貸方フラグ</value>
            public string Shkzg { get; set; }
            /// <summary>Gets or sets 事業領域</summary>
            /// <value>事業領域</value>
            public string Gsber { get; set; }
            /// <summary>Gets or sets 税コード</summary>
            /// <value>税コード</value>
            public string Mwskz { get; set; }
            /// <summary>Gets or sets 国内通貨額</summary>
            /// <value>国内通貨額</value>
            public long? Dmbtr { get; set; }
            /// <summary>Gets or sets 伝票通貨額</summary>
            /// <value>伝票通貨額</value>
            public string Wrbtr { get; set; }
            /// <summary>Gets or sets 起算日</summary>
            /// <value>起算日</value>
            public int? Valut { get; set; }
            /// <summary>Gets or sets ソートキー</summary>
            /// <value>ソートキー</value>
            public string Zuonr { get; set; }
            /// <summary>Gets or sets 明細テキスト</summary>
            /// <value>明細テキスト</value>
            public string Sgtxt { get; set; }
            /// <summary>Gets or sets 取引タイプ</summary>
            /// <value>取引タイプ</value>
            public string Bewar { get; set; }
            /// <summary>Gets or sets 原価センタ</summary>
            /// <value>原価センタ</value>
            public string Kostl { get; set; }
            /// <summary>Gets or sets 総勘定元帳勘定</summary>
            /// <value>総勘定元帳勘定</value>
            public string Hkont { get; set; }
            /// <summary>Gets or sets 利益センタ</summary>
            /// <value>利益センタ</value>
            public string Prctr { get; set; }
            /// <summary>Gets or sets 取引先参照キー１</summary>
            /// <value>取引先参照キー１</value>
            public string Xref1 { get; set; }
            /// <summary>Gets or sets 取引先参照キー２</summary>
            /// <value>取引先参照キー２</value>
            public string Xref2 { get; set; }
            /// <summary>Gets or sets 明細の参照キー３</summary>
            /// <value>明細の参照キー３</value>
            public string Xref3 { get; set; }
            /// <summary>Gets or sets 特殊仕訳コード</summary>
            /// <value>特殊仕訳コード</value>
            public string Umskz { get; set; }
            /// <summary>Gets or sets 得意先コード</summary>
            /// <value>得意先コード</value>
            public string Kunnr { get; set; }
            /// <summary>Gets or sets 期日計算の支払基準日</summary>
            /// <value>期日計算の支払基準日</value>
            public int? Zfbdt { get; set; }
            /// <summary>Gets or sets 支払条件キー</summary>
            /// <value>支払条件キー</value>
            public string Zterm { get; set; }
            /// <summary>Gets or sets 支払方法</summary>
            /// <value>支払方法</value>
            public string Zlsch { get; set; }
            /// <summary>Gets or sets 支払保留キー</summary>
            /// <value>支払保留キー</value>
            public string Zlspr { get; set; }
            /// <summary>Gets or sets 仕入先または債権者の勘定コード</summary>
            /// <value>仕入先または債権者の勘定コード</value>
            public string Lifnr { get; set; }
            /// <summary>Gets or sets 特別勘定ソート</summary>
            /// <value>特別勘定ソート</value>
            public string Hzuon { get; set; }
            /// <summary>Gets or sets 現金割引期間 1 (日数)</summary>
            /// <value>現金割引期間 1 (日数)</value>
            public int? Zbd1T { get; set; }
            /// <summary>Gets or sets 数量</summary>
            /// <value>数量</value>
            public decimal? Menge { get; set; }
            /// <summary>Gets or sets 基本数量単位</summary>
            /// <value>基本数量単位</value>
            public string Meins { get; set; }
            /// <summary>Gets or sets 支払参照</summary>
            /// <value>支払参照</value>
            public string Kidno { get; set; }
            /// <summary>Gets or sets 予備エリア</summary>
            /// <value>予備エリア</value>
            public string Filler { get; set; }
            /// <summary>Gets or sets カード別伝票別取引区分</summary>
            /// <value>カード別伝票別取引区分</value>
            public string CardSlipType { get; set; }
            /// <summary>Gets or sets 流通チャネル</summary>
            /// <value>流通チャネル</value>
            public string DistributionChannel { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string MngType { get; set; }
            /// <summary>Gets or sets 管理ＮＯ</summary>
            /// <value>管理ＮＯ</value>
            public string MngNo { get; set; }
            /// <summary>Gets or sets 管理ＮＯ枝番</summary>
            /// <value>管理ＮＯ枝番</value>
            public string MngBranchNo { get; set; }
            /// <summary>Gets or sets 取引先</summary>
            /// <value>取引先</value>
            public string BusinessConnections { get; set; }
            /// <summary>Gets or sets 品名</summary>
            /// <value>品名</value>
            public string GoodsName { get; set; }
            /// <summary>Gets or sets 金利負担区分</summary>
            /// <value>金利負担区分</value>
            public string ShareType { get; set; }
            /// <summary>Gets or sets サイト</summary>
            /// <value>サイト</value>
            public string Site { get; set; }
            /// <summary>Gets or sets 金利負担日数</summary>
            /// <value>金利負担日数</value>
            public int? ShareDays { get; set; }
            /// <summary>Gets or sets 振出日</summary>
            /// <value>振出日</value>
            public string StartYmd2 { get; set; }
            /// <summary>Gets or sets 満期日</summary>
            /// <value>満期日</value>
            public string EndYmd { get; set; }
            /// <summary>Gets or sets 手形ＮＯ</summary>
            /// <value>手形ＮＯ</value>
            public string DraftNo { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 入金区分</summary>
            /// <value>入金区分</value>
            public string MoneyReceivedType { get; set; }
            /// <summary>Gets or sets 社員番号</summary>
            /// <value>社員番号</value>
            public string EmployeeNo1 { get; set; }
            /// <summary>Gets or sets 個人立替金連番</summary>
            /// <value>個人立替金連番</value>
            public string AdvanceNo { get; set; }
            /// <summary>Gets or sets 伝票メニュＮＯ</summary>
            /// <value>伝票メニュＮＯ</value>
            public string SlipMenuNo { get; set; }
            /// <summary>Gets or sets 税計算対象外区分</summary>
            /// <value>税計算対象外区分</value>
            public string TaxType { get; set; }
            /// <summary>Gets or sets 種別コード</summary>
            /// <value>種別コード</value>
            public string TypeCd { get; set; }
            /// <summary>Gets or sets 取組日</summary>
            /// <value>取組日</value>
            public string TransferYmd { get; set; }
            /// <summary>Gets or sets 被仕向銀行番号</summary>
            /// <value>被仕向銀行番号</value>
            public string HandlingBankCd { get; set; }
            /// <summary>Gets or sets 被仕向銀行名</summary>
            /// <value>被仕向銀行名</value>
            public string HandlingBankKanji { get; set; }
            /// <summary>Gets or sets 被仕向支店番号</summary>
            /// <value>被仕向支店番号</value>
            public string HandlingBranchCd { get; set; }
            /// <summary>Gets or sets 被仕向支店名</summary>
            /// <value>被仕向支店名</value>
            public string HandlingBranchKanji { get; set; }
            /// <summary>Gets or sets 振込先預金種目</summary>
            /// <value>振込先預金種目</value>
            public int? HandlingAccountType { get; set; }
            /// <summary>Gets or sets 振込先口座番号</summary>
            /// <value>振込先口座番号</value>
            public string HandlingAccountNo { get; set; }
            /// <summary>Gets or sets 受取人名</summary>
            /// <value>受取人名</value>
            public string HandlingAccountName { get; set; }
            /// <summary>Gets or sets 振込金額</summary>
            /// <value>振込金額</value>
            public string PaymentMoney { get; set; }
            /// <summary>Gets or sets 社員ＩＤ</summary>
            /// <value>社員ＩＤ</value>
            public string EmployeeNo2 { get; set; }
            /// <summary>Gets or sets 資金部門コード</summary>
            /// <value>資金部門コード</value>
            public string DepartmentCd { get; set; }
            /// <summary>Gets or sets 支払資金科目</summary>
            /// <value>支払資金科目</value>
            public string PaybaseName { get; set; }
            /// <summary>Gets or sets 承認日付</summary>
            /// <value>承認日付</value>
            public string ApproveYmd { get; set; }
            /// <summary>Gets or sets 承認時刻</summary>
            /// <value>承認時刻</value>
            public string ApproveHm { get; set; }
            /// <summary>Gets or sets 承認者社員ＮＯ</summary>
            /// <value>承認者社員ＮＯ</value>
            public string ApproveEmpNo { get; set; }
            /// <summary>Gets or sets 承認社員所属部門コード</summary>
            /// <value>承認社員所属部門コード</value>
            public string ApprovalDepartmentCd { get; set; }
            /// <summary>Gets or sets EOF</summary>
            /// <value>EOF</value>
            public string Temp { get; set; }
        }

        /// <summary>
        /// 仕訳送信履歴~1
        /// </summary>
        public class IfShiwakeSendEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public IfShiwakeSendEntity()
            {
                TableName = "if_shiwake_send";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 履歴番号</summary>
            /// <value>履歴番号</value>
            public int RevNo { get; set; }
            /// <summary>Gets or sets 仕訳区分|0</summary>
            /// <value>仕訳区分|0</value>
            public int? DivisionNo { get; set; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets ファイル区分</summary>
            /// <value>ファイル区分</value>
            public string Flk { get; set; }
            /// <summary>Gets or sets 勘定年月|YYMM</summary>
            /// <value>勘定年月|YYMM</value>
            public string Kjn { get; set; }
            /// <summary>Gets or sets 連結コード|UA1~6</summary>
            /// <value>連結コード|UA1~6</value>
            public string Rkc { get; set; }
            /// <summary>Gets or sets dhb</summary>
            /// <value>dhb</value>
            public string Dhb { get; set; }
            /// <summary>Gets or sets msn</summary>
            /// <value>msn</value>
            public string Msn { get; set; }
            /// <summary>Gets or sets 転記日付</summary>
            /// <value>転記日付</value>
            public string Ymd { get; set; }
            /// <summary>Gets or sets 支払基準日</summary>
            /// <value>支払基準日</value>
            public string Skb { get; set; }
            /// <summary>Gets or sets 借方部門科目</summary>
            /// <value>借方部門科目</value>
            public string Dbmk { get; set; }
            /// <summary>Gets or sets 借方勘定科目</summary>
            /// <value>借方勘定科目</value>
            public string Dkkm { get; set; }
            /// <summary>Gets or sets 借方取引先</summary>
            /// <value>借方取引先</value>
            public string Dths { get; set; }
            /// <summary>Gets or sets dysk</summary>
            /// <value>dysk</value>
            public string Dysk { get; set; }
            /// <summary>Gets or sets dkrk</summary>
            /// <value>dkrk</value>
            public string Dkrk { get; set; }
            /// <summary>Gets or sets dkrn</summary>
            /// <value>dkrn</value>
            public string Dkrn { get; set; }
            /// <summary>Gets or sets dkre</summary>
            /// <value>dkre</value>
            public string Dkre { get; set; }
            /// <summary>Gets or sets dnsk</summary>
            /// <value>dnsk</value>
            public string Dnsk { get; set; }
            /// <summary>Gets or sets dtht</summary>
            /// <value>dtht</value>
            public string Dtht { get; set; }
            /// <summary>Gets or sets yobi1</summary>
            /// <value>yobi1</value>
            public string Yobi1 { get; set; }
            /// <summary>Gets or sets 貸方部門科目</summary>
            /// <value>貸方部門科目</value>
            public string Cbmk { get; set; }
            /// <summary>Gets or sets 貸方勘定科目</summary>
            /// <value>貸方勘定科目</value>
            public string Ckkm { get; set; }
            /// <summary>Gets or sets 貸方取引先</summary>
            /// <value>貸方取引先</value>
            public string Cths { get; set; }
            /// <summary>Gets or sets cysk</summary>
            /// <value>cysk</value>
            public string Cysk { get; set; }
            /// <summary>Gets or sets ckrk</summary>
            /// <value>ckrk</value>
            public string Ckrk { get; set; }
            /// <summary>Gets or sets ckrn</summary>
            /// <value>ckrn</value>
            public string Ckrn { get; set; }
            /// <summary>Gets or sets ckre</summary>
            /// <value>ckre</value>
            public string Ckre { get; set; }
            /// <summary>Gets or sets cnsk</summary>
            /// <value>cnsk</value>
            public string Cnsk { get; set; }
            /// <summary>Gets or sets ctht</summary>
            /// <value>ctht</value>
            public string Ctht { get; set; }
            /// <summary>Gets or sets yobi2</summary>
            /// <value>yobi2</value>
            public string Yobi2 { get; set; }
            /// <summary>Gets or sets 税計算対象区分</summary>
            /// <value>税計算対象区分</value>
            public string Zeik { get; set; }
            /// <summary>Gets or sets 銀行</summary>
            /// <value>銀行</value>
            public string Bnk { get; set; }
            /// <summary>Gets or sets renno</summary>
            /// <value>renno</value>
            public string Renno { get; set; }
            /// <summary>Gets or sets smc</summary>
            /// <value>smc</value>
            public string Smc { get; set; }
            /// <summary>Gets or sets skey</summary>
            /// <value>skey</value>
            public string Skey { get; set; }
            /// <summary>Gets or sets nshh</summary>
            /// <value>nshh</value>
            public string Nshh { get; set; }
            /// <summary>Gets or sets ksk</summary>
            /// <value>ksk</value>
            public string Ksk { get; set; }
            /// <summary>Gets or sets sit</summary>
            /// <value>sit</value>
            public string Sit { get; set; }
            /// <summary>Gets or sets fdb</summary>
            /// <value>fdb</value>
            public string Fdb { get; set; }
            /// <summary>Gets or sets mkb</summary>
            /// <value>mkb</value>
            public string Mkb { get; set; }
            /// <summary>Gets or sets tno</summary>
            /// <value>tno</value>
            public string Tno { get; set; }
            /// <summary>Gets or sets sgai</summary>
            /// <value>sgai</value>
            public string Sgai { get; set; }
            /// <summary>Gets or sets krt</summary>
            /// <value>krt</value>
            public string Krt { get; set; }
            /// <summary>Gets or sets kymd</summary>
            /// <value>kymd</value>
            public string Kymd { get; set; }
            /// <summary>Gets or sets 金額</summary>
            /// <value>金額</value>
            public string Kin3 { get; set; }
            /// <summary>Gets or sets gai3</summary>
            /// <value>gai3</value>
            public string Gai3 { get; set; }
            /// <summary>Gets or sets rvr</summary>
            /// <value>rvr</value>
            public string Rvr { get; set; }
            /// <summary>Gets or sets sres</summary>
            /// <value>sres</value>
            public string Sres { get; set; }
            /// <summary>Gets or sets taxcds</summary>
            /// <value>taxcds</value>
            public string Taxcds { get; set; }
            /// <summary>Gets or sets taxcdh</summary>
            /// <value>taxcdh</value>
            public string Taxcdh { get; set; }
            /// <summary>Gets or sets taxrs</summary>
            /// <value>taxrs</value>
            public string Taxrs { get; set; }
            /// <summary>Gets or sets taxrh</summary>
            /// <value>taxrh</value>
            public string Taxrh { get; set; }
            /// <summary>Gets or sets din</summary>
            /// <value>din</value>
            public string Din { get; set; }
        }

        /// <summary>
        /// 在庫更新実行ログ
        /// </summary>
        public class InoutLogEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InoutLogEntity()
            {
                TableName = "inout_log";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ログ連番</summary>
            /// <value>ログ連番</value>
            public int LogSeq { get; set; }
            /// <summary>Gets or sets 処理日時</summary>
            /// <value>処理日時</value>
            public DateTime? CtrlDate { get; set; }
            /// <summary>Gets or sets メニューID</summary>
            /// <value>メニューID</value>
            public long? MenuId { get; set; }
            /// <summary>Gets or sets タブID</summary>
            /// <value>タブID</value>
            public long? TabId { get; set; }
            /// <summary>Gets or sets 操作ID</summary>
            /// <value>操作ID</value>
            public long? CtrlId { get; set; }
            /// <summary>Gets or sets 処理番号</summary>
            /// <value>処理番号</value>
            public int? ProcessNo { get; set; }
            /// <summary>Gets or sets 受払履歴|各種名称マスタ</summary>
            /// <value>受払履歴|各種名称マスタ</value>
            public int? InoutDivision { get; set; }
            /// <summary>Gets or sets ログインID</summary>
            /// <value>ログインID</value>
            public string CtrlUser { get; set; }
            /// <summary>Gets or sets 処理区分</summary>
            /// <value>処理区分</value>
            public int? ProcessDivision { get; set; }
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
            /// <summary>Gets or sets メッセージ</summary>
            /// <value>メッセージ</value>
            public string Message { get; set; }
        }

        /// <summary>
        /// 在庫更新プロパティ
        /// </summary>
        public class InoutPropertyEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InoutPropertyEntity()
            {
                TableName = "inout_property";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 在庫更新処理番号</summary>
            /// <value>在庫更新処理番号</value>
            public int ProcessNo { get; set; }
            /// <summary>Gets or sets 受払区分</summary>
            /// <value>受払区分</value>
            public int InoutDivision { get; set; }
            /// <summary>Gets or sets 受払ソース作成フラグ|0</summary>
            /// <value>受払ソース作成フラグ|0</value>
            public int InoutSourceFlg { get; set; }
            /// <summary>Gets or sets 受払履歴作成フラグ|0</summary>
            /// <value>受払履歴作成フラグ|0</value>
            public int InoutRecodeFlg { get; set; }
            /// <summary>Gets or sets ロット在庫作成フラグ|0</summary>
            /// <value>ロット在庫作成フラグ|0</value>
            public int LotInventoryFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 在庫更新処理番号</summary>
                /// <value>在庫更新処理番号</value>
                public int ProcessNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="processNo">在庫更新処理番号</param>
                public PrimaryKey(int processNo)
                {
                    ProcessNo = processNo;
                }
            }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ProcessNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="processNo">在庫更新処理番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InoutPropertyEntity GetEntity(int processNo, ComDB db)
            {
                InoutPropertyEntity.PrimaryKey condition = new InoutPropertyEntity.PrimaryKey(processNo);
                return GetEntity<InoutPropertyEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 受払履歴
        /// </summary>
        public class InoutRecordEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InoutRecordEntity()
            {
                TableName = "inout_record";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 受払番号</summary>
            /// <value>受払番号</value>
            public string InoutNo { get; set; }
            /// <summary>Gets or sets 受払区分|各種名称マスタ</summary>
            /// <value>受払区分|各種名称マスタ</value>
            public string InoutDivision { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 受払日</summary>
            /// <value>受払日</value>
            public DateTime InoutDate { get; set; }
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
            public decimal InoutQty { get; set; }
            /// <summary>Gets or sets 受払単価</summary>
            /// <value>受払単価</value>
            public decimal InoutPrice { get; set; }
            /// <summary>Gets or sets 受払金額</summary>
            /// <value>受払金額</value>
            public decimal InoutCost { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 理由コード</summary>
            /// <value>理由コード</value>
            public string RyCd { get; set; }
            /// <summary>Gets or sets 理由</summary>
            /// <value>理由</value>
            public string Reason { get; set; }
            /// <summary>Gets or sets 受払更新日時</summary>
            /// <value>受払更新日時</value>
            public DateTime? InoutUpdateDate { get; set; }
            /// <summary>Gets or sets 在庫更新日時</summary>
            /// <value>在庫更新日時</value>
            public DateTime? InventoryUpdateDate { get; set; }
            /// <summary>Gets or sets 登録時メニュー番号</summary>
            /// <value>登録時メニュー番号</value>
            public string InputMenuId { get; set; }
            /// <summary>Gets or sets 登録時タブ番号</summary>
            /// <value>登録時タブ番号</value>
            public string InputDisplayId { get; set; }
            /// <summary>Gets or sets 登録時操作番号</summary>
            /// <value>登録時操作番号</value>
            public int InputControlId { get; set; }
            /// <summary>Gets or sets 更新時メニュー番号</summary>
            /// <value>更新時メニュー番号</value>
            public string UpdateMenuId { get; set; }
            /// <summary>Gets or sets 更新時タブ番号</summary>
            /// <value>更新時タブ番号</value>
            public string UpdateDisplayId { get; set; }
            /// <summary>Gets or sets 更新時操作番号</summary>
            /// <value>更新時操作番号</value>
            public int UpdateControlId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 受払番号</summary>
                /// <value>受払番号</value>
                public string InoutNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="inoutNo">受払番号</param>
                public PrimaryKey(string inoutNo)
                {
                    InoutNo = inoutNo;
                }
            }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InoutNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="inoutNo">受払番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InoutRecordEntity GetEntity(string inoutNo, ComDB db)
            {
                InoutRecordEntity.PrimaryKey condition = new InoutRecordEntity.PrimaryKey(inoutNo);
                return GetEntity<InoutRecordEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 受払ソース
        /// </summary>
        public class InoutSourceEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InoutSourceEntity()
            {
                TableName = "inout_source";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 受払ソース番号</summary>
            /// <value>受払ソース番号</value>
            public string InoutSourceNo { get; set; }
            /// <summary>Gets or sets 受払区分|各種名称マスタ</summary>
            /// <value>受払区分|各種名称マスタ</value>
            public string InoutDivision { get; set; }
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
            /// <value>引当済フラグ|1</value>
            public int? AssignFlg { get; set; }
            /// <summary>Gets or sets 過剰フラグ|引当数量以上に処理した場合</summary>
            /// <value>過剰フラグ|引当数量以上に処理した場合</value>
            public int? OverFlg { get; set; }
            /// <summary>Gets or sets 登録時メニュー番号</summary>
            /// <value>登録時メニュー番号</value>
            public string InputMenuId { get; set; }
            /// <summary>Gets or sets 登録時タブ番号</summary>
            /// <value>登録時タブ番号</value>
            public string InputDisplayId { get; set; }
            /// <summary>Gets or sets 登録時操作番号</summary>
            /// <value>登録時操作番号</value>
            public int? InputControlId { get; set; }
            /// <summary>Gets or sets 更新時メニュー番号</summary>
            /// <value>更新時メニュー番号</value>
            public string UpdateMenuId { get; set; }
            /// <summary>Gets or sets 更新時タブ番号</summary>
            /// <value>更新時タブ番号</value>
            public string UpdateDisplayId { get; set; }
            /// <summary>Gets or sets 更新時操作番号</summary>
            /// <value>更新時操作番号</value>
            public int? UpdateControlId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 受払ソース番号</summary>
                /// <value>受払ソース番号</value>
                public string InoutSourceNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="inoutSourceNo">受払ソース番号</param>
                public PrimaryKey(string inoutSourceNo)
                {
                    InoutSourceNo = inoutSourceNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InoutSourceNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="inoutSourceNo">受払ソース番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InoutSourceEntity GetEntity(string inoutSourceNo, ComDB db)
            {
                InoutSourceEntity.PrimaryKey condition = new InoutSourceEntity.PrimaryKey(inoutSourceNo);
                return GetEntity<InoutSourceEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 受払ソース~1
        /// </summary>
        public class InoutSourceFinalEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InoutSourceFinalEntity()
            {
                TableName = "inout_source_final";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 受払ソース番号</summary>
            /// <value>受払ソース番号</value>
            public string InoutSourceNo { get; set; }
            /// <summary>Gets or sets 受払区分|各種名称マスタ</summary>
            /// <value>受払区分|各種名称マスタ</value>
            public string InoutDivision { get; set; }
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
            /// <value>引当済フラグ|1</value>
            public int? AssignFlg { get; set; }
            /// <summary>Gets or sets 過剰フラグ|引当数量以上に処理した場合</summary>
            /// <value>過剰フラグ|引当数量以上に処理した場合</value>
            public int? OverFlg { get; set; }
            /// <summary>Gets or sets 登録時メニュー番号</summary>
            /// <value>登録時メニュー番号</value>
            public string InputMenuId { get; set; }
            /// <summary>Gets or sets 登録時タブ番号</summary>
            /// <value>登録時タブ番号</value>
            public string InputDisplayId { get; set; }
            /// <summary>Gets or sets 登録時操作番号</summary>
            /// <value>登録時操作番号</value>
            public int? InputControlId { get; set; }
            /// <summary>Gets or sets 更新時メニュー番号</summary>
            /// <value>更新時メニュー番号</value>
            public string UpdateMenuId { get; set; }
            /// <summary>Gets or sets 更新時タブ番号</summary>
            /// <value>更新時タブ番号</value>
            public string UpdateDisplayId { get; set; }
            /// <summary>Gets or sets 更新時操作番号</summary>
            /// <value>更新時操作番号</value>
            public int? UpdateControlId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 受払ソース番号</summary>
                /// <value>受払ソース番号</value>
                public string InoutSourceNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="inoutSourceNo">受払ソース番号</param>
                public PrimaryKey(string inoutSourceNo)
                {
                    InoutSourceNo = inoutSourceNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InoutSourceNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="inoutSourceNo">受払ソース番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InoutSourceFinalEntity GetEntity(string inoutSourceNo, ComDB db)
            {
                InoutSourceFinalEntity.PrimaryKey condition = new InoutSourceFinalEntity.PrimaryKey(inoutSourceNo);
                return GetEntity<InoutSourceFinalEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 在庫不整合チェックログ
        /// </summary>
        public class InventoryCheckLogEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InventoryCheckLogEntity()
            {
                TableName = "inventory_check_log";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets チェック種別|1</summary>
            /// <value>チェック種別|1</value>
            public int Division { get; set; }
            /// <summary>Gets or sets 区分名称</summary>
            /// <value>区分名称</value>
            public string DivisionName { get; set; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets 比較元在庫数量</summary>
            /// <value>比較元在庫数量</value>
            public decimal? InventoryFrom { get; set; }
            /// <summary>Gets or sets 比較先在庫数量</summary>
            /// <value>比較先在庫数量</value>
            public decimal? InventoryTo { get; set; }
            /// <summary>Gets or sets チェック日時</summary>
            /// <value>チェック日時</value>
            public DateTime? CheckDate { get; set; }
        }

        /// <summary>
        /// 棚卸
        /// </summary>
        public class InventoryCountEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InventoryCountEntity()
            {
                TableName = "inventory_count";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 棚卸データ作成日</summary>
            /// <value>棚卸データ作成日</value>
            public DateTime CountDate { get; set; }
            /// <summary>Gets or sets 循環棚卸区分</summary>
            /// <value>循環棚卸区分</value>
            public string CountDivision { get; set; }
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
            /// <summary>Gets or sets ロット発生日</summary>
            /// <value>ロット発生日</value>
            public DateTime IssueDate { get; set; }
            /// <summary>Gets or sets 発生元区分</summary>
            /// <value>発生元区分</value>
            public int? OriginDivision { get; set; }
            /// <summary>Gets or sets 在庫数量</summary>
            /// <value>在庫数量</value>
            public decimal? CountQty { get; set; }
            /// <summary>Gets or sets 端数在庫数量</summary>
            /// <value>端数在庫数量</value>
            public decimal? FractionQty { get; set; }
            /// <summary>Gets or sets 棚卸入力数量</summary>
            /// <value>棚卸入力数量</value>
            public decimal? InputQty { get; set; }
            /// <summary>Gets or sets 端数棚卸入力数量</summary>
            /// <value>端数棚卸入力数量</value>
            public decimal? InputFraction { get; set; }
            /// <summary>Gets or sets 計上日</summary>
            /// <value>計上日</value>
            public DateTime? CountAccrualDate { get; set; }
            /// <summary>Gets or sets 理由コード</summary>
            /// <value>理由コード</value>
            public string ReasonCd { get; set; }
            /// <summary>Gets or sets 棚卸更新処理日</summary>
            /// <value>棚卸更新処理日</value>
            public DateTime? CountUpdateDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 勘定年月</summary>
                /// <value>勘定年月</value>
                public string AccountYears { get; set; }
                /// <summary>Gets or sets 棚卸データ作成日</summary>
                /// <value>棚卸データ作成日</value>
                public DateTime CountDate { get; set; }
                /// <summary>Gets or sets 循環棚卸区分</summary>
                /// <value>循環棚卸区分</value>
                public string CountDivision { get; set; }
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
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="accountYears">勘定年月</param>
                /// <param name="countDate">棚卸データ作成日</param>
                /// <param name="countDivision">循環棚卸区分</param>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="locationCd">ロケーションコード</param>
                /// <param name="lotNo">ロット番号</param>
                /// <param name="subLotNo1">サブロット番号1</param>
                /// <param name="subLotNo2">サブロット番号2</param>
                public PrimaryKey(string accountYears, DateTime countDate, string countDivision, string itemCd, string specificationCd, string locationCd, string lotNo, string subLotNo1, string subLotNo2)
                {
                    AccountYears = accountYears;
                    CountDate = countDate;
                    CountDivision = countDivision;
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    LocationCd = locationCd;
                    LotNo = lotNo;
                    SubLotNo1 = subLotNo1;
                    SubLotNo2 = subLotNo2;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.AccountYears, this.CountDate, this.CountDivision, this.ItemCd, this.SpecificationCd, this.LocationCd, this.LotNo, this.SubLotNo1, this.SubLotNo2);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="accountYears">勘定年月</param>
            /// <param name="countDate">棚卸データ作成日</param>
            /// <param name="countDivision">循環棚卸区分</param>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="locationCd">ロケーションコード</param>
            /// <param name="lotNo">ロット番号</param>
            /// <param name="subLotNo1">サブロット番号1</param>
            /// <param name="subLotNo2">サブロット番号2</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InventoryCountEntity GetEntity(string accountYears, DateTime countDate, string countDivision, string itemCd, string specificationCd, string locationCd, string lotNo, string subLotNo1, string subLotNo2, ComDB db)
            {
                InventoryCountEntity.PrimaryKey condition = new InventoryCountEntity.PrimaryKey(accountYears, countDate, countDivision, itemCd, specificationCd, locationCd, lotNo, subLotNo1, subLotNo2);
                return GetEntity<InventoryCountEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 在庫入出庫指図
        /// </summary>
        public class InventoryInoutDirectionEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InventoryInoutDirectionEntity()
            {
                TableName = "inventory_inout_direction";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string IoDirectionNo { get; set; }
            /// <summary>Gets or sets 入出庫区分|各種名称マスタ|IODV</summary>
            /// <value>入出庫区分|各種名称マスタ|IODV</value>
            public int InoutDivision { get; set; }
            /// <summary>Gets or sets 指図日</summary>
            /// <value>指図日</value>
            public DateTime? IoDirectionDate { get; set; }
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
            /// <summary>Gets or sets 金額</summary>
            /// <value>金額</value>
            public decimal? Cost { get; set; }
            /// <summary>Gets or sets 入出庫数量</summary>
            /// <value>入出庫数量</value>
            public decimal? IoQty { get; set; }
            /// <summary>Gets or sets 理由コード</summary>
            /// <value>理由コード</value>
            public string RyCd { get; set; }
            /// <summary>Gets or sets 理由</summary>
            /// <value>理由</value>
            public string Reason { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 承認ステータス|各種名称マスタ</summary>
            /// <value>承認ステータス|各種名称マスタ</value>
            public int? ApprovalStatus { get; set; }
            /// <summary>Gets or sets 承認依頼者</summary>
            /// <value>承認依頼者</value>
            public string ApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 承認依頼日付</summary>
            /// <value>承認依頼日付</value>
            public DateTime? ApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 承認者</summary>
            /// <value>承認者</value>
            public string ApprovalUserId { get; set; }
            /// <summary>Gets or sets 承認日付</summary>
            /// <value>承認日付</value>
            public DateTime? ApprovalDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 指図番号</summary>
                /// <value>指図番号</value>
                public string IoDirectionNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="directionNo">指図番号</param>
                public PrimaryKey(string directionNo)
                {
                    IoDirectionNo = directionNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.IoDirectionNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="directionNo">指図番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InventoryInoutDirectionEntity GetEntity(string directionNo, ComDB db)
            {
                InventoryInoutDirectionEntity.PrimaryKey condition = new InventoryInoutDirectionEntity.PrimaryKey(directionNo);
                return GetEntity<InventoryInoutDirectionEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 品目マスタ
        /// </summary>
        public class ItemEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ItemEntity()
            {
                TableName = "item";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 品目名称(多言語)</summary>
            /// <value>品目名称(多言語)</value>
            public string ItemNameCommon { get; set; }
            /// <summary>Gets or sets 品目サブ名称</summary>
            /// <value>品目サブ名称</value>
            public string ItemSubName { get; set; }
            /// <summary>Gets or sets 品目サブ名称(多言語)</summary>
            /// <value>品目サブ名称(多言語)</value>
            public string ItemSubNameCommon { get; set; }
            /// <summary>Gets or sets 品目名称かな</summary>
            /// <value>品目名称かな</value>
            public string ItemNameKana { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets ステータス|各種名称マスタ</summary>
            /// <value>ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 有効フラグ</summary>
            /// <value>有効フラグ</value>
            public int ActivateFlg { get; set; }
            /// <summary>Gets or sets 承認依頼先担当者ID</summary>
            /// <value>承認依頼先担当者ID</value>
            public string ReceiveUserId { get; set; }
            /// <summary>Gets or sets 承認依頼者ID</summary>
            /// <value>承認依頼者ID</value>
            public string ApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 承認依頼日時</summary>
            /// <value>承認依頼日時</value>
            public DateTime? ApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 承認者ID</summary>
            /// <value>承認者ID</value>
            public string ApprovalUserId { get; set; }
            /// <summary>Gets or sets 承認日時</summary>
            /// <value>承認日時</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 開始有効日</summary>
                /// <value>開始有効日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="activeDate">開始有効日</param>
                public PrimaryKey(string itemCd, DateTime activeDate)
                {
                    ItemCd = itemCd;
                    ActiveDate = activeDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.ActiveDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="activeDate">開始有効日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ItemEntity GetEntity(string itemCd, DateTime activeDate, ComDB db)
            {
                ItemEntity.PrimaryKey condition = new ItemEntity.PrimaryKey(itemCd, activeDate);
                return GetEntity<ItemEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 品目マスタ_販売品扱い属性
        /// </summary>
        public class ItemArticleAttributeEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ItemArticleAttributeEntity()
            {
                TableName = "item_article_attribute";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 安全リードタイム</summary>
            /// <value>安全リードタイム</value>
            public int? ArticleSafetyLeadTime { get; set; }
            /// <summary>Gets or sets 預り品区分|各種名称マスタ</summary>
            /// <value>預り品区分|各種名称マスタ</value>
            public int? KeepDivision { get; set; }
            /// <summary>Gets or sets 売上部門コード</summary>
            /// <value>売上部門コード</value>
            public string ArticleSectionCd { get; set; }
            /// <summary>Gets or sets 売上科目コード</summary>
            /// <value>売上科目コード</value>
            public string ArticleAccountsCd { get; set; }
            /// <summary>Gets or sets 受注登録時指図発行フラグ</summary>
            /// <value>受注登録時指図発行フラグ</value>
            public int? AutoDirectionFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets 開始有効日</summary>
                /// <value>開始有効日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="activeDate">開始有効日</param>
                public PrimaryKey(string itemCd, string specificationCd, DateTime activeDate)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    ActiveDate = activeDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd, this.ActiveDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="activeDate">開始有効日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ItemArticleAttributeEntity GetEntity(string itemCd, string specificationCd, DateTime activeDate, ComDB db)
            {
                ItemArticleAttributeEntity.PrimaryKey condition = new ItemArticleAttributeEntity.PrimaryKey(itemCd, specificationCd, activeDate);
                return GetEntity<ItemArticleAttributeEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 品目マスタ_在庫単価共通情報
        /// </summary>
        public class ItemCommonAttributeEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ItemCommonAttributeEntity()
            {
                TableName = "item_common_attribute";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 安全在庫</summary>
            /// <value>安全在庫</value>
            public decimal? ProductOrderPoint { get; set; }
            /// <summary>Gets or sets 発注情報</summary>
            /// <value>発注情報</value>
            public string OrderInfo { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets 開始有効日</summary>
                /// <value>開始有効日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="activeDate">開始有効日</param>
                public PrimaryKey(string itemCd, string specificationCd, DateTime activeDate)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    ActiveDate = activeDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd, this.ActiveDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="activeDate">開始有効日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ItemCommonAttributeEntity GetEntity(string itemCd, string specificationCd, DateTime activeDate, ComDB db)
            {
                ItemCommonAttributeEntity.PrimaryKey condition = new ItemCommonAttributeEntity.PrimaryKey(itemCd, specificationCd, activeDate);
                return GetEntity<ItemCommonAttributeEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 品目マスタ_統合ERP関連項目
        /// </summary>
        public class ItemErpAttributeEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ItemErpAttributeEntity()
            {
                TableName = "item_erp_attribute";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets item_cd</summary>
            /// <value>item_cd</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets specification_cd</summary>
            /// <value>specification_cd</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets ERP_品目コード</summary>
            /// <value>ERP_品目コード</value>
            public string ErpItemCd { get; set; }
            /// <summary>Gets or sets ERP_サイズ寸法</summary>
            /// <value>ERP_サイズ寸法</value>
            public string ErpSize { get; set; }
            /// <summary>Gets or sets ERP_プラント</summary>
            /// <value>ERP_プラント</value>
            public string ErpPlant { get; set; }
            /// <summary>Gets or sets ERP_保管場所</summary>
            /// <value>ERP_保管場所</value>
            public string ErpLocation { get; set; }
            /// <summary>Gets or sets ERP_文書番号</summary>
            /// <value>ERP_文書番号</value>
            public string ErpDocument { get; set; }
            /// <summary>Gets or sets ERP_基本数量単位</summary>
            /// <value>ERP_基本数量単位</value>
            public string ErpUnit { get; set; }
            /// <summary>Gets or sets ERP_品目テキスト</summary>
            /// <value>ERP_品目テキスト</value>
            public string ErpItemText { get; set; }
            /// <summary>Gets or sets ERP_品目タイプ</summary>
            /// <value>ERP_品目タイプ</value>
            public string ErpItemDivision { get; set; }
            /// <summary>Gets or sets ERP_品目グループ</summary>
            /// <value>ERP_品目グループ</value>
            public string ErpItemGroup { get; set; }
            /// <summary>Gets or sets ERP_外部品目グループ</summary>
            /// <value>ERP_外部品目グループ</value>
            public string ErpExternalItemGroup { get; set; }
            /// <summary>Gets or sets ERP_製造計画担当</summary>
            /// <value>ERP_製造計画担当</value>
            public string ErpPlanner { get; set; }
            /// <summary>Gets or sets del_flg</summary>
            /// <value>del_flg</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets item_cd</summary>
                /// <value>item_cd</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets specification_cd</summary>
                /// <value>specification_cd</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets 開始有効日</summary>
                /// <value>開始有効日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="activeDate">開始有効日</param>
                public PrimaryKey(string itemCd, string specificationCd, DateTime activeDate)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    ActiveDate = activeDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd, this.ActiveDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="activeDate">開始有効日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ItemErpAttributeEntity GetEntity(string itemCd, string specificationCd, DateTime activeDate, ComDB db)
            {
                ItemErpAttributeEntity.PrimaryKey condition = new ItemErpAttributeEntity.PrimaryKey(itemCd, specificationCd, activeDate);
                return GetEntity<ItemErpAttributeEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 品目マスタ_製造品扱い属性
        /// </summary>
        public class ItemProductAttributeEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ItemProductAttributeEntity()
            {
                TableName = "item_product_attribute";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 生産計画区分|各種名称マスタ</summary>
            /// <value>生産計画区分|各種名称マスタ</value>
            public int? ProductionPlan { get; set; }
            /// <summary>Gets or sets 生産分類コード</summary>
            /// <value>生産分類コード</value>
            public string ProductionCategory { get; set; }
            /// <summary>Gets or sets 標準発注数量</summary>
            /// <value>標準発注数量</value>
            public decimal? StdQty { get; set; }
            /// <summary>Gets or sets 指図発行時初期状態</summary>
            /// <value>指図発行時初期状態</value>
            public int? DirectionDefaultFixFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets 開始有効日</summary>
                /// <value>開始有効日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="activeDate">開始有効日</param>
                public PrimaryKey(string itemCd, string specificationCd, DateTime activeDate)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    ActiveDate = activeDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd, this.ActiveDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="activeDate">開始有効日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ItemProductAttributeEntity GetEntity(string itemCd, string specificationCd, DateTime activeDate, ComDB db)
            {
                ItemProductAttributeEntity.PrimaryKey condition = new ItemProductAttributeEntity.PrimaryKey(itemCd, specificationCd, activeDate);
                return GetEntity<ItemProductAttributeEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 品目マスタ_購入品扱い属性
        /// </summary>
        public class ItemPurchaseAttributeEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ItemPurchaseAttributeEntity()
            {
                TableName = "item_purchase_attribute";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 仕入先品目コード</summary>
            /// <value>仕入先品目コード</value>
            public string VenderItemCd { get; set; }
            /// <summary>Gets or sets 基準仕入先コード</summary>
            /// <value>基準仕入先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 購買リードタイム</summary>
            /// <value>購買リードタイム</value>
            public int? PurchaseLeadTime { get; set; }
            /// <summary>Gets or sets 安全リードタイム</summary>
            /// <value>安全リードタイム</value>
            public int? PurchaseSafetyLeadTime { get; set; }
            /// <summary>Gets or sets 発注基準|各種名称マスタ</summary>
            /// <value>発注基準|各種名称マスタ</value>
            public int? PurchaseTrigger { get; set; }
            /// <summary>Gets or sets 複数社発注区分|各種名称マスタ</summary>
            /// <value>複数社発注区分|各種名称マスタ</value>
            public int? MultiVenderDivision { get; set; }
            /// <summary>Gets or sets 発注点</summary>
            /// <value>発注点</value>
            public decimal? PurchaseOrderPoint { get; set; }
            /// <summary>Gets or sets 発注単位</summary>
            /// <value>発注単位</value>
            public decimal? PurchaseOrderUnitQty { get; set; }
            /// <summary>Gets or sets 最低発注数</summary>
            /// <value>最低発注数</value>
            public decimal? PurchaseOrderMinQty { get; set; }
            /// <summary>Gets or sets 最大発注数</summary>
            /// <value>最大発注数</value>
            public decimal? PurchaseOrderMaxQty { get; set; }
            /// <summary>Gets or sets 標準検査方法|各種名称マスタ</summary>
            /// <value>標準検査方法|各種名称マスタ</value>
            public int? InspectionType { get; set; }
            /// <summary>Gets or sets 仕入部門コード</summary>
            /// <value>仕入部門コード</value>
            public string PurchaseSectionCd { get; set; }
            /// <summary>Gets or sets 仕入科目コード</summary>
            /// <value>仕入科目コード</value>
            public string PurchaseAccountsCd { get; set; }
            /// <summary>Gets or sets 購買属性ステータス(未使用)</summary>
            /// <value>購買属性ステータス(未使用)</value>
            public int? PurchaseStatus { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets 開始有効日</summary>
                /// <value>開始有効日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="activeDate">開始有効日</param>
                public PrimaryKey(string itemCd, string specificationCd, DateTime activeDate)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    ActiveDate = activeDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd, this.ActiveDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="activeDate">開始有効日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ItemPurchaseAttributeEntity GetEntity(string itemCd, string specificationCd, DateTime activeDate, ComDB db)
            {
                ItemPurchaseAttributeEntity.PrimaryKey condition = new ItemPurchaseAttributeEntity.PrimaryKey(itemCd, specificationCd, activeDate);
                return GetEntity<ItemPurchaseAttributeEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 品目マスタ_関連資料
        /// </summary>
        public class ItemRelatedDataEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ItemRelatedDataEntity()
            {
                TableName = "item_related_data";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets ファイル番号</summary>
            /// <value>ファイル番号</value>
            public long FileMngNo { get; set; }
            /// <summary>Gets or sets 表示名</summary>
            /// <value>表示名</value>
            public string DisplayName { get; set; }
            /// <summary>Gets or sets ファイルフルパス</summary>
            /// <value>ファイルフルパス</value>
            public string MsdsFileName { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets 開始有効日</summary>
                /// <value>開始有効日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>Gets or sets ファイル番号</summary>
                /// <value>ファイル番号</value>
                public long FileMngNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="activeDate">開始有効日</param>
                /// <param name="fileMngNo">ファイル番号</param>
                public PrimaryKey(string itemCd, string specificationCd, DateTime activeDate, long fileMngNo)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    ActiveDate = activeDate;
                    FileMngNo = fileMngNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd, this.ActiveDate, this.FileMngNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="activeDate">開始有効日</param>
            /// <param name="fileMngNo">ファイル番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ItemRelatedDataEntity GetEntity(string itemCd, string specificationCd, DateTime activeDate, long fileMngNo, ComDB db)
            {
                ItemRelatedDataEntity.PrimaryKey condition = new ItemRelatedDataEntity.PrimaryKey(itemCd, specificationCd, activeDate, fileMngNo);
                return GetEntity<ItemRelatedDataEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 品目仕様マスタ
        /// </summary>
        public class ItemSpecificationEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ItemSpecificationEntity()
            {
                TableName = "item_specification";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 品目仕様開始有効日</summary>
            /// <value>品目仕様開始有効日</value>
            public DateTime? SpecificationActiveDate { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 仕様名称(多言語)</summary>
            /// <value>仕様名称(多言語)</value>
            public string SpecificationNameC { get; set; }
            /// <summary>Gets or sets 仕様サブ名称</summary>
            /// <value>仕様サブ名称</value>
            public string SpecificationSubName { get; set; }
            /// <summary>Gets or sets 仕様サブ名称(多言語)</summary>
            /// <value>仕様サブ名称(多言語)</value>
            public string SpecificationSubNameC { get; set; }
            /// <summary>Gets or sets 仕様1</summary>
            /// <value>仕様1</value>
            public string SpecValue1 { get; set; }
            /// <summary>Gets or sets 仕様2</summary>
            /// <value>仕様2</value>
            public string SpecValue2 { get; set; }
            /// <summary>Gets or sets 仕様3</summary>
            /// <value>仕様3</value>
            public string SpecValue3 { get; set; }
            /// <summary>Gets or sets 品目タイプ|各種名称マスタ</summary>
            /// <value>品目タイプ|各種名称マスタ</value>
            public int? ItemType { get; set; }
            /// <summary>Gets or sets 製造品区分|各種名称マスタ</summary>
            /// <value>製造品区分|各種名称マスタ</value>
            public int? ProductDivision { get; set; }
            /// <summary>Gets or sets 販売品区分|各種名称マスタ</summary>
            /// <value>販売品区分|各種名称マスタ</value>
            public int? ArticleDivision { get; set; }
            /// <summary>Gets or sets 購入品区分|各種名称マスタ</summary>
            /// <value>購入品区分|各種名称マスタ</value>
            public int? PurchaseDivision { get; set; }
            /// <summary>Gets or sets 受入検査区分|各種名称マスタ</summary>
            /// <value>受入検査区分|各種名称マスタ</value>
            public int? InspectionType { get; set; }
            /// <summary>Gets or sets 換算係数(在庫)</summary>
            /// <value>換算係数(在庫)</value>
            public decimal? KgOfFractionManagement { get; set; }
            /// <summary>Gets or sets 在庫管理単位|各種名称マスタ</summary>
            /// <value>在庫管理単位|各種名称マスタ</value>
            public string UnitOfStockCtrl { get; set; }
            /// <summary>Gets or sets 運用管理単位|各種名称マスタ</summary>
            /// <value>運用管理単位|各種名称マスタ</value>
            public string UnitOfOperationManagement { get; set; }
            /// <summary>Gets or sets 入り数</summary>
            /// <value>入り数</value>
            public decimal? NumberOfInsertions { get; set; }
            /// <summary>Gets or sets 品目分類コード</summary>
            /// <value>品目分類コード</value>
            public string ItemCategory { get; set; }
            /// <summary>Gets or sets 在庫管理区分|各種名称マスタ</summary>
            /// <value>在庫管理区分|各種名称マスタ</value>
            public int? StockDivision { get; set; }
            /// <summary>Gets or sets マイナス在庫許可</summary>
            /// <value>マイナス在庫許可</value>
            public int? NegativeInventoryPermitFlg { get; set; }
            /// <summary>Gets or sets 基準保管場所</summary>
            /// <value>基準保管場所</value>
            public string DefaultLocation { get; set; }
            /// <summary>Gets or sets 重量</summary>
            /// <value>重量</value>
            public decimal? AllUpWeight { get; set; }
            /// <summary>Gets or sets 荷姿</summary>
            /// <value>荷姿</value>
            public string StyleOfPacking { get; set; }
            /// <summary>Gets or sets ロット管理区分|各種名称マスタ</summary>
            /// <value>ロット管理区分|各種名称マスタ</value>
            public int? LotDivision { get; set; }
            /// <summary>Gets or sets スポット区分|各種名称マスタ</summary>
            /// <value>スポット区分|各種名称マスタ</value>
            public int? SpotDivision { get; set; }
            /// <summary>Gets or sets 検査フラグ</summary>
            /// <value>検査フラグ</value>
            public int? InspectionFlg { get; set; }
            /// <summary>Gets or sets ロット品質期限日数(検査日基準)</summary>
            /// <value>ロット品質期限日数(検査日基準)</value>
            public int? QualityDeadlineDate { get; set; }
            /// <summary>Gets or sets ロット有効期限日数(仕上日基準)</summary>
            /// <value>ロット有効期限日数(仕上日基準)</value>
            public int? ExpirationDeadlineDate { get; set; }
            /// <summary>Gets or sets 承認依頼先担当者ID</summary>
            /// <value>承認依頼先担当者ID</value>
            public string ReceiveUserId { get; set; }
            /// <summary>Gets or sets 承認依頼者ID</summary>
            /// <value>承認依頼者ID</value>
            public string ApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 承認依頼日時</summary>
            /// <value>承認依頼日時</value>
            public DateTime? ApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 承認者ID</summary>
            /// <value>承認者ID</value>
            public string ApprovalUserId { get; set; }
            /// <summary>Gets or sets 承認日時</summary>
            /// <value>承認日時</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets ステータス|各種名称マスタ</summary>
            /// <value>ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 計画区分</summary>
            /// <value>計画区分</value>
            public int? PlanDivision { get; set; }
            /// <summary>Gets or sets 丸め日数</summary>
            /// <value>丸め日数</value>
            public int? RoundOffDays { get; set; }
            /// <summary>Gets or sets 生産計画生産順</summary>
            /// <value>生産計画生産順</value>
            public int? LowLevelCd { get; set; }
            /// <summary>Gets or sets 有効フラグ</summary>
            /// <value>有効フラグ</value>
            public int ActivateFlg { get; set; }
            /// <summary>Gets or sets 代表品目コード</summary>
            /// <value>代表品目コード</value>
            public string ManagementItemCd { get; set; }
            /// <summary>Gets or sets 代表仕様コード</summary>
            /// <value>代表仕様コード</value>
            public string ManagementSpecificationCd { get; set; }
            /// <summary>Gets or sets 代表開始有効日</summary>
            /// <value>代表開始有効日</value>
            public DateTime? ManagementActiveDate { get; set; }
            /// <summary>Gets or sets 代表品目換算計数</summary>
            /// <value>代表品目換算計数</value>
            public decimal? ManagementItemOfFraction { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string AccountsCd { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets 開始有効日</summary>
                /// <value>開始有効日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="activeDate">開始有効日</param>
                public PrimaryKey(string itemCd, string specificationCd, DateTime activeDate)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    ActiveDate = activeDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd, this.ActiveDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="activeDate">開始有効日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ItemSpecificationEntity GetEntity(string itemCd, string specificationCd, DateTime activeDate, ComDB db)
            {
                ItemSpecificationEntity.PrimaryKey condition = new ItemSpecificationEntity.PrimaryKey(itemCd, specificationCd, activeDate);
                return GetEntity<ItemSpecificationEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 生産ラインマスタ
        /// </summary>
        public class LineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LineEntity()
            {
                TableName = "line";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 生産ラインコード</summary>
            /// <value>生産ラインコード</value>
            public string ProductionLine { get; set; }
            /// <summary>Gets or sets 生産ライン名称</summary>
            /// <value>生産ライン名称</value>
            public string ProductionLineName { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 生産ラインコード</summary>
                /// <value>生産ラインコード</value>
                public string ProductionLine { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="productionLine">生産ラインコード</param>
                public PrimaryKey(string productionLine)
                {
                    ProductionLine = productionLine;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ProductionLine);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="productionLine">生産ラインコード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public LineEntity GetEntity(string productionLine, ComDB db)
            {
                LineEntity.PrimaryKey condition = new LineEntity.PrimaryKey(productionLine);
                return GetEntity<LineEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ロケーションマスタ
        /// </summary>
        public class LocationEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LocationEntity()
            {
                TableName = "location";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets ロケーション名称</summary>
            /// <value>ロケーション名称</value>
            public string LocationName { get; set; }
            /// <summary>Gets or sets 上位ロケーションコード</summary>
            /// <value>上位ロケーションコード</value>
            public string UpperLocationCd { get; set; }
            /// <summary>Gets or sets 郵便番号</summary>
            /// <value>郵便番号</value>
            public string Zipcode { get; set; }
            /// <summary>Gets or sets 住所1</summary>
            /// <value>住所1</value>
            public string Address1 { get; set; }
            /// <summary>Gets or sets 住所2</summary>
            /// <value>住所2</value>
            public string Address2 { get; set; }
            /// <summary>Gets or sets 住所3</summary>
            /// <value>住所3</value>
            public string Address3 { get; set; }
            /// <summary>Gets or sets 電話番号</summary>
            /// <value>電話番号</value>
            public string TelNo { get; set; }
            /// <summary>Gets or sets FAX番号</summary>
            /// <value>FAX番号</value>
            public string FaxNo { get; set; }
            /// <summary>Gets or sets mailアドレス</summary>
            /// <value>mailアドレス</value>
            public string Mail { get; set; }
            /// <summary>Gets or sets 循環棚卸区分|各種名称マスタ</summary>
            /// <value>循環棚卸区分|各種名称マスタ</value>
            public string CountDivision { get; set; }
            /// <summary>Gets or sets 在庫管理区分|各種名称マスタ</summary>
            /// <value>在庫管理区分|各種名称マスタ</value>
            public int? StockDivision { get; set; }
            /// <summary>Gets or sets 在庫可能フラグ|各種名称マスタ</summary>
            /// <value>在庫可能フラグ|各種名称マスタ</value>
            public int AvailableFlg { get; set; }
            /// <summary>Gets or sets MRP対象フラグ|各種名称マスタ</summary>
            /// <value>MRP対象フラグ|各種名称マスタ</value>
            public int MrpTargetFlg { get; set; }
            /// <summary>Gets or sets 出荷可能フラグ</summary>
            /// <value>出荷可能フラグ</value>
            public int? ShipEnableFlg { get; set; }
            /// <summary>Gets or sets 配車要不要</summary>
            /// <value>配車要不要</value>
            public int? DispatchDivision { get; set; }
            /// <summary>Gets or sets 資産管理区分</summary>
            /// <value>資産管理区分</value>
            public string AssetDivision { get; set; }
            /// <summary>Gets or sets マイナス在庫許可</summary>
            /// <value>マイナス在庫許可</value>
            public int? NegativeInventoryPermitFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ロケーションコード</summary>
                /// <value>ロケーションコード</value>
                public string LocationCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="locationCd">ロケーションコード</param>
                public PrimaryKey(string locationCd)
                {
                    LocationCd = locationCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.LocationCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="locationCd">ロケーションコード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public LocationEntity GetEntity(string locationCd, ComDB db)
            {
                LocationEntity.PrimaryKey condition = new LocationEntity.PrimaryKey(locationCd);
                return GetEntity<LocationEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 担当者マスタ
        /// </summary>
        public class LoginEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LoginEntity()
            {
                TableName = "login";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets ユーザー名</summary>
            /// <value>ユーザー名</value>
            public string UserName { get; set; }
            /// <summary>Gets or sets パスワード</summary>
            /// <value>パスワード</value>
            public string Password { get; set; }
            /// <summary>Gets or sets ユーザー名かな</summary>
            /// <value>ユーザー名かな</value>
            public string UserNameKana { get; set; }
            /// <summary>Gets or sets 有効フラグ|各種名称マスタ</summary>
            /// <value>有効フラグ|各種名称マスタ</value>
            public string ActiveFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ|各種名称マスタ</summary>
            /// <value>削除フラグ|各種名称マスタ</value>
            public string DelFlg { get; set; }
            /// <summary>Gets or sets 管理者区分|各種名称マスタ</summary>
            /// <value>管理者区分|各種名称マスタ</value>
            public string AdminFlg { get; set; }
            /// <summary>Gets or sets パスワード変更日時</summary>
            /// <value>パスワード変更日時</value>
            public DateTime? UpdatePass { get; set; }
            /// <summary>Gets or sets テストユーザフラグ|0</summary>
            /// <value>テストユーザフラグ|0</value>
            public string TestUserFlg { get; set; }
            /// <summary>Gets or sets ダイレクトログインフラグ</summary>
            /// <value>ダイレクトログインフラグ</value>
            public string DirectLoginFlg { get; set; }
            /// <summary>Gets or sets メールアドレス</summary>
            /// <value>メールアドレス</value>
            public string MailAddress { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ユーザーID</summary>
                /// <value>ユーザーID</value>
                public string UserId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="userId">ユーザーID</param>
                public PrimaryKey(string userId)
                {
                    UserId = userId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.UserId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="userId">ユーザーID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public LoginEntity GetEntity(string userId, ComDB db)
            {
                LoginEntity.PrimaryKey condition = new LoginEntity.PrimaryKey(userId);
                return GetEntity<LoginEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 在庫_ロット別在庫
        /// </summary>
        public class LotInventoryEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LotInventoryEntity()
            {
                TableName = "lot_inventory";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
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
            /// <summary>Gets or sets 在庫数量</summary>
            /// <value>在庫数量</value>
            public decimal? InventoryQty { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
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
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="locationCd">ロケーションコード</param>
                /// <param name="lotNo">ロット番号</param>
                /// <param name="subLotNo1">サブロット番号1</param>
                /// <param name="subLotNo2">サブロット番号2</param>
                public PrimaryKey(string itemCd, string specificationCd, string locationCd, string lotNo, string subLotNo1, string subLotNo2)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    LocationCd = locationCd;
                    LotNo = lotNo;
                    SubLotNo1 = subLotNo1;
                    SubLotNo2 = subLotNo2;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd, this.LocationCd, this.LotNo, this.SubLotNo1, this.SubLotNo2);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="locationCd">ロケーションコード</param>
            /// <param name="lotNo">ロット番号</param>
            /// <param name="subLotNo1">サブロット番号1</param>
            /// <param name="subLotNo2">サブロット番号2</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public LotInventoryEntity GetEntity(string itemCd, string specificationCd, string locationCd, string lotNo, string subLotNo1, string subLotNo2, ComDB db)
            {
                LotInventoryEntity.PrimaryKey condition = new LotInventoryEntity.PrimaryKey(itemCd, specificationCd, locationCd, lotNo, subLotNo1, subLotNo2);
                return GetEntity<LotInventoryEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// メールテンプレートマスタ
        /// </summary>
        public class MailTemplateEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MailTemplateEntity()
            {
                TableName = "mail_template";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets メールフォーマットID</summary>
            /// <value>メールフォーマットID</value>
            public long MailFormatId { get; set; }
            /// <summary>Gets or sets 説明</summary>
            /// <value>説明</value>
            public string Description { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string TextTitle { get; set; }
            /// <summary>Gets or sets 本文</summary>
            /// <value>本文</value>
            public string TextBody { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets メールフォーマットID</summary>
                /// <value>メールフォーマットID</value>
                public long MailFormatId { get; set; }
                /// <summary>Gets or sets 言語コード</summary>
                /// <value>言語コード</value>
                public string LanguageId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="mailFormatId">メールフォーマットID</param>
                /// <param name="languageId">言語コード</param>
                public PrimaryKey(long mailFormatId, string languageId)
                {
                    MailFormatId = mailFormatId;
                    LanguageId = languageId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MailFormatId, this.LanguageId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="mailFormatId">メールフォーマットID</param>
            /// <param name="languageId">言語コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MailTemplateEntity GetEntity(long mailFormatId, string languageId, ComDB db)
            {
                MailTemplateEntity.PrimaryKey condition = new MailTemplateEntity.PrimaryKey(mailFormatId, languageId);
                return GetEntity<MailTemplateEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 連番取得処理制御マスタ
        /// </summary>
        public class MakeSequenceCtlEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MakeSequenceCtlEntity()
            {
                TableName = "make_sequence_ctl";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 連番処理名称（主キー）</summary>
            /// <value>連番処理名称（主キー）</value>
            public string ProcessName { get; set; }
            /// <summary>Gets or sets division</summary>
            /// <value>division</value>
            public string Division { get; set; }
            /// <summary>Gets or sets 処理区分</summary>
            /// <value>処理区分</value>
            public string SeqName { get; set; }
            /// <summary>Gets or sets seq_term</summary>
            /// <value>seq_term</value>
            public string SeqTerm { get; set; }
            /// <summary>Gets or sets 開始タイミング　連番期間でY か Mを指定した場合に使用する。例えば1年で4月から行う場合、04を設定する。Dの場合は無視する。必要があれば処理を修正する</summary>
            /// <value>開始タイミング　連番期間でY か Mを指定した場合に使用する。例えば1年で4月から行う場合、04を設定する。Dの場合は無視する。必要があれば処理を修正する</value>
            public string StartTime { get; set; }
            /// <summary>Gets or sets add_char1</summary>
            /// <value>add_char1</value>
            public string AddChar1 { get; set; }
            /// <summary>Gets or sets add_char2</summary>
            /// <value>add_char2</value>
            public string AddChar2 { get; set; }
            /// <summary>Gets or sets add_char3</summary>
            /// <value>add_char3</value>
            public string AddChar3 { get; set; }
            /// <summary>Gets or sets add_char4</summary>
            /// <value>add_char4</value>
            public string AddChar4 { get; set; }
            /// <summary>Gets or sets add_char5</summary>
            /// <value>add_char5</value>
            public string AddChar5 { get; set; }
            /// <summary>Gets or sets 発番連番値の開始値</summary>
            /// <value>発番連番値の開始値</value>
            public long? StartVal { get; set; }
            /// <summary>Gets or sets 発番連番値の終了値（これを超えた場合一周する）</summary>
            /// <value>発番連番値の終了値（これを超えた場合一周する）</value>
            public long? MaxVal { get; set; }
            /// <summary>Gets or sets 備考（用途等を記載する）</summary>
            /// <value>備考（用途等を記載する）</value>
            public string Remark { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 連番処理名称（主キー）</summary>
                /// <value>連番処理名称（主キー）</value>
                public string ProcessName { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="processName">連番処理名称</param>
                public PrimaryKey(string processName)
                {
                    ProcessName = processName;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ProcessName);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="processName">連番処理名称</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MakeSequenceCtlEntity GetEntity(string processName, ComDB db)
            {
                MakeSequenceCtlEntity.PrimaryKey condition = new MakeSequenceCtlEntity.PrimaryKey(processName);
                return GetEntity<MakeSequenceCtlEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 連番データ保持テーブル（品目毎、仕様毎、納入先毎、取引~1
        /// </summary>
        public class MakeSequenceDataEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MakeSequenceDataEntity()
            {
                TableName = "make_sequence_data";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 処理区分</summary>
            /// <value>処理区分</value>
            public string ProcessName { get; set; }
            /// <summary>Gets or sets 品目</summary>
            /// <value>品目</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様</summary>
            /// <value>仕様</value>
            public string SpecCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 日付</summary>
            /// <value>日付</value>
            public string SeqDate { get; set; }
            /// <summary>Gets or sets 現在の値</summary>
            /// <value>現在の値</value>
            public int? NowVal { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 処理区分</summary>
                /// <value>処理区分</value>
                public string ProcessName { get; set; }
                /// <summary>Gets or sets 品目</summary>
                /// <value>品目</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様</summary>
                /// <value>仕様</value>
                public string SpecCd { get; set; }
                /// <summary>Gets or sets 取引先区分</summary>
                /// <value>取引先区分</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 取引先コード</summary>
                /// <value>取引先コード</value>
                public string VenderCd { get; set; }
                /// <summary>Gets or sets 納入先コード</summary>
                /// <value>納入先コード</value>
                public string DeliveryCd { get; set; }
                /// <summary>Gets or sets 日付</summary>
                /// <value>日付</value>
                public string SeqDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="processName">処理区分</param>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specCd">仕様コード</param>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="venderCd">取引先コード</param>
                /// <param name="deliveryCd">納入先コード</param>
                /// <param name="seqDate">日付</param>
                public PrimaryKey(string processName, string itemCd, string specCd, string venderDivision, string venderCd, string deliveryCd, string seqDate)
                {
                    ProcessName = processName;
                    ItemCd = itemCd;
                    SpecCd = specCd;
                    VenderDivision = venderDivision;
                    VenderCd = venderCd;
                    DeliveryCd = deliveryCd;
                    SeqDate = seqDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ProcessName, this.ItemCd, this.SpecCd, this.VenderDivision, this.VenderCd, this.DeliveryCd, this.SeqDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="processName">処理区分</param>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specCd">仕様コード</param>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="deliveryCd">納入先コード</param>
            /// <param name="seqDate">日付</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MakeSequenceDataEntity GetEntity(string processName, string itemCd, string specCd, string venderDivision, string venderCd, string deliveryCd, string seqDate, ComDB db)
            {
                MakeSequenceDataEntity.PrimaryKey condition = new MakeSequenceDataEntity.PrimaryKey(processName, itemCd, specCd, venderDivision, venderCd, deliveryCd, seqDate);
                return GetEntity<MakeSequenceDataEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// メニューマスタ_詳細(タブ情報)
        /// </summary>
        public class MenuDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MenuDetailEntity()
            {
                TableName = "menu_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets メニューID</summary>
            /// <value>メニューID</value>
            public long MenuId { get; set; }
            /// <summary>Gets or sets タブID</summary>
            /// <value>タブID</value>
            public long TabId { get; set; }
            /// <summary>Gets or sets タブ名称</summary>
            /// <value>タブ名称</value>
            public string TabName { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets メニューID</summary>
                /// <value>メニューID</value>
                public long MenuId { get; set; }
                /// <summary>Gets or sets タブID</summary>
                /// <value>タブID</value>
                public long TabId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="menuId">メニューID</param>
                /// <param name="tabId">タブID</param>
                public PrimaryKey(long menuId, long tabId)
                {
                    MenuId = menuId;
                    TabId = tabId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MenuId, this.TabId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="menuId">メニューID</param>
            /// <param name="tabId">タブID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MenuDetailEntity GetEntity(long menuId, long tabId, ComDB db)
            {
                MenuDetailEntity.PrimaryKey condition = new MenuDetailEntity.PrimaryKey(menuId, tabId);
                return GetEntity<MenuDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// メニューマスタ_ヘッダ
        /// </summary>
        public class MenuHeadEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MenuHeadEntity()
            {
                TableName = "menu_head";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets メニューID</summary>
            /// <value>メニューID</value>
            public long MenuId { get; set; }
            /// <summary>Gets or sets メニュータイプID|0</summary>
            /// <value>メニュータイプID|0</value>
            public string MenuTypeId { get; set; }
            /// <summary>Gets or sets メニュー名称</summary>
            /// <value>メニュー名称</value>
            public string MenuName { get; set; }
            /// <summary>Gets or sets ACTION</summary>
            /// <value>ACTION</value>
            public string Action { get; set; }
            /// <summary>Gets or sets ソート順序</summary>
            /// <value>ソート順序</value>
            public int? SortNo { get; set; }
            /// <summary>Gets or sets 親メニューID</summary>
            /// <value>親メニューID</value>
            public long? ParentMenuId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets メニューID</summary>
                /// <value>メニューID</value>
                public long MenuId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="menuId">メニューID</param>
                public PrimaryKey(long menuId)
                {
                    MenuId = menuId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MenuId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="menuId">メニューID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MenuHeadEntity GetEntity(long menuId, ComDB db)
            {
                MenuHeadEntity.PrimaryKey condition = new MenuHeadEntity.PrimaryKey(menuId);
                return GetEntity<MenuHeadEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// メニュータイプマスタ
        /// </summary>
        public class MenuTypeEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MenuTypeEntity()
            {
                TableName = "menu_type";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets メニュータイプID</summary>
            /// <value>メニュータイプID</value>
            public string MenuTypeId { get; set; }
            /// <summary>Gets or sets メニュータイプ名称</summary>
            /// <value>メニュータイプ名称</value>
            public string MenuTypeName { get; set; }
            /// <summary>Gets or sets 画像名</summary>
            /// <value>画像名</value>
            public string ImgName { get; set; }
            /// <summary>Gets or sets OPEN画像名</summary>
            /// <value>OPEN画像名</value>
            public string OpenImgName { get; set; }
            /// <summary>Gets or sets CLOSE画像名</summary>
            /// <value>CLOSE画像名</value>
            public string CloseImgName { get; set; }
            /// <summary>Gets or sets ファイル区分</summary>
            /// <value>ファイル区分</value>
            public string FileDivision { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets メニュータイプID</summary>
                /// <value>メニュータイプID</value>
                public string MenuTypeId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="menuTypeId">メニュータイプID</param>
                public PrimaryKey(string menuTypeId)
                {
                    MenuTypeId = menuTypeId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MenuTypeId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="menuTypeId">メニュータイプID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MenuTypeEntity GetEntity(string menuTypeId, ComDB db)
            {
                MenuTypeEntity.PrimaryKey condition = new MenuTypeEntity.PrimaryKey(menuTypeId);
                return GetEntity<MenuTypeEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 月次受払実績
        /// </summary>
        public class MonthlyInoutRecordEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MonthlyInoutRecordEntity()
            {
                TableName = "monthly_inout_record";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 対象月</summary>
            /// <value>対象月</value>
            public int TargetMonth { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 品目タイプ</summary>
            /// <value>品目タイプ</value>
            public int? ItemType { get; set; }
            /// <summary>Gets or sets 重量</summary>
            /// <value>重量</value>
            public decimal? AllUpWeight { get; set; }
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
            /// <summary>Gets or sets 受払対象集計日時</summary>
            /// <value>受払対象集計日時</value>
            public int? InventoryClosingDay { get; set; }
            /// <summary>Gets or sets 前月繰越在庫数</summary>
            /// <value>前月繰越在庫数</value>
            public decimal? LmInventoryQty { get; set; }
            /// <summary>Gets or sets 前月繰越在庫評価単価</summary>
            /// <value>前月繰越在庫評価単価</value>
            public decimal? LmInventoryCost { get; set; }
            /// <summary>Gets or sets 前月繰越在庫金額</summary>
            /// <value>前月繰越在庫金額</value>
            public decimal? LmInventoryAmount { get; set; }
            /// <summary>Gets or sets 当月通常受入数</summary>
            /// <value>当月通常受入数</value>
            public decimal? InQty { get; set; }
            /// <summary>Gets or sets 当月通常受入金額</summary>
            /// <value>当月通常受入金額</value>
            public decimal? InAmount { get; set; }
            /// <summary>Gets or sets 当月通常払出数</summary>
            /// <value>当月通常払出数</value>
            public decimal? OutQty { get; set; }
            /// <summary>Gets or sets 当月通常払出金額</summary>
            /// <value>当月通常払出金額</value>
            public decimal? OutAmount { get; set; }
            /// <summary>Gets or sets 当月転用受入数</summary>
            /// <value>当月転用受入数</value>
            public decimal? DiversionInQty { get; set; }
            /// <summary>Gets or sets 当月転用受入金額</summary>
            /// <value>当月転用受入金額</value>
            public decimal? DiversionInAmount { get; set; }
            /// <summary>Gets or sets 当月転用払出数</summary>
            /// <value>当月転用払出数</value>
            public decimal? DiversionOutQty { get; set; }
            /// <summary>Gets or sets 当月転用払出金額</summary>
            /// <value>当月転用払出金額</value>
            public decimal? DiversionOutAmount { get; set; }
            /// <summary>Gets or sets 当月その他受入数</summary>
            /// <value>当月その他受入数</value>
            public decimal? OthersInQty { get; set; }
            /// <summary>Gets or sets 当月その他受入金額</summary>
            /// <value>当月その他受入金額</value>
            public decimal? OthersInAmount { get; set; }
            /// <summary>Gets or sets 当月その他払出数</summary>
            /// <value>当月その他払出数</value>
            public decimal? OthersOutQty { get; set; }
            /// <summary>Gets or sets 当月その他払出金額</summary>
            /// <value>当月その他払出金額</value>
            public decimal? OthersOutAmount { get; set; }
            /// <summary>Gets or sets 翌月繰越在庫数</summary>
            /// <value>翌月繰越在庫数</value>
            public decimal? NmInventoryQty { get; set; }
            /// <summary>Gets or sets 翌月繰越在庫評価単価</summary>
            /// <value>翌月繰越在庫評価単価</value>
            public decimal? NmInventoryCost { get; set; }
            /// <summary>Gets or sets 翌月繰越在庫金額</summary>
            /// <value>翌月繰越在庫金額</value>
            public decimal? NmInventoryAmount { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 対象月</summary>
                /// <value>対象月</value>
                public int TargetMonth { get; set; }
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
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="targetMonth">対象月</param>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="locationCd">ロケーションコード</param>
                /// <param name="lotNo">ロット番号</param>
                /// <param name="subLotNo1">サブロット番号1</param>
                /// <param name="subLotNo2">サブロット番号2</param>
                public PrimaryKey(int targetMonth, string itemCd, string specificationCd, string locationCd, string lotNo, string subLotNo1, string subLotNo2)
                {
                    TargetMonth = targetMonth;
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    LocationCd = locationCd;
                    LotNo = lotNo;
                    SubLotNo1 = subLotNo1;
                    SubLotNo2 = subLotNo2;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.TargetMonth, this.ItemCd, this.SpecificationCd, this.LocationCd, this.LotNo, this.SubLotNo1, this.SubLotNo2);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="targetMonth">対象月</param>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="locationCd">ロケーションコード</param>
            /// <param name="lotNo">ロット番号</param>
            /// <param name="subLotNo1">サブロット番号1</param>
            /// <param name="subLotNo2">サブロット番号2</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MonthlyInoutRecordEntity GetEntity(int targetMonth, string itemCd, string specificationCd, string locationCd, string lotNo, string subLotNo1, string subLotNo2, ComDB db)
            {
                MonthlyInoutRecordEntity.PrimaryKey condition = new MonthlyInoutRecordEntity.PrimaryKey(targetMonth, itemCd, specificationCd, locationCd, lotNo, subLotNo1, subLotNo2);
                return GetEntity<MonthlyInoutRecordEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 名称マスタ
        /// </summary>
        public class NamesEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public NamesEntity()
            {
                TableName = "names";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 名称区分</summary>
            /// <value>名称区分</value>
            public string NameDivision { get; set; }
            /// <summary>Gets or sets 名称コード</summary>
            /// <value>名称コード</value>
            public string NameCd { get; set; }
            /// <summary>Gets or sets 名称1</summary>
            /// <value>名称1</value>
            public string Name01 { get; set; }
            /// <summary>Gets or sets 名称2</summary>
            /// <value>名称2</value>
            public string Name02 { get; set; }
            /// <summary>Gets or sets 名称3</summary>
            /// <value>名称3</value>
            public string Name03 { get; set; }
            /// <summary>Gets or sets 数量端数単位|1</summary>
            /// <value>数量端数単位|1</value>
            public int? QuantityRoundup { get; set; }
            /// <summary>Gets or sets 数量端数区分|1</summary>
            /// <value>数量端数区分|1</value>
            public int? QuantityRoundupUnit { get; set; }
            /// <summary>Gets or sets 拡張項目1</summary>
            /// <value>拡張項目1</value>
            public string ExtendInfo1 { get; set; }
            /// <summary>Gets or sets 拡張項目2</summary>
            /// <value>拡張項目2</value>
            public string ExtendInfo2 { get; set; }
            /// <summary>Gets or sets 拡張項目3</summary>
            /// <value>拡張項目3</value>
            public string ExtendInfo3 { get; set; }
            /// <summary>Gets or sets リスト表示順</summary>
            /// <value>リスト表示順</value>
            public int? Seq { get; set; }
            /// <summary>Gets or sets リスト表示|非表示</summary>
            /// <value>リスト表示|非表示</value>
            public int? Invisibility { get; set; }
            /// <summary>Gets or sets 多通貨マスタの使用通貨|未設定</summary>
            /// <value>多通貨マスタの使用通貨|未設定</value>
            public int? Meflg1 { get; set; }
            /// <summary>Gets or sets mecode1</summary>
            /// <value>mecode1</value>
            public string Mecode1 { get; set; }
            /// <summary>Gets or sets 名称マスタ説明 2018/11/07 追加(使っている機能がわからないためここに生産計画などの機能名を明記してください)</summary>
            /// <value>名称マスタ説明 2018/11/07 追加(使っている機能がわからないためここに生産計画などの機能名を明記してください)</value>
            public string Discription { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 名称区分</summary>
                /// <value>名称区分</value>
                public string NameDivision { get; set; }
                /// <summary>Gets or sets 名称コード</summary>
                /// <value>名称コード</value>
                public string NameCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="nameDivision">名称区分</param>
                /// <param name="nameCd">名称コード</param>
                public PrimaryKey(string nameDivision, string nameCd)
                {
                    NameDivision = nameDivision;
                    NameCd = nameCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.NameDivision, this.NameCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="nameDivision">名称区分</param>
            /// <param name="nameCd">名称コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public NamesEntity GetEntity(string nameDivision, string nameCd, ComDB db)
            {
                NamesEntity.PrimaryKey condition = new NamesEntity.PrimaryKey(nameDivision, nameCd);
                return GetEntity<NamesEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 数値桁数チェックマスタ
        /// </summary>
        public class NumberChkdisitEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public NumberChkdisitEntity()
            {
                TableName = "number_chkdisit";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 区分</summary>
            /// <value>区分</value>
            public string UnitDivision { get; set; }
            /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
            /// <value>取引先区分|各種名称マスタ</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 全体桁数（小数点、マイナス含む）</summary>
            /// <value>全体桁数（小数点、マイナス含む）</value>
            public int? MaxLength { get; set; }
            /// <summary>Gets or sets 全体桁数</summary>
            /// <value>全体桁数</value>
            public int? IntegerLength { get; set; }
            /// <summary>Gets or sets 少数部桁数</summary>
            /// <value>少数部桁数</value>
            public int? SmallnumLength { get; set; }
            /// <summary>Gets or sets 端数区分|0</summary>
            /// <value>端数区分|0</value>
            public int? RoundDivision { get; set; }
            /// <summary>Gets or sets 下限値</summary>
            /// <value>下限値</value>
            public decimal? LowerLimit { get; set; }
            /// <summary>Gets or sets 上限値</summary>
            /// <value>上限値</value>
            public decimal? UpperLimit { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 区分</summary>
                /// <value>区分</value>
                public string UnitDivision { get; set; }
                /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
                /// <value>取引先区分|各種名称マスタ</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 取引先コード</summary>
                /// <value>取引先コード</value>
                public string VenderCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="unitDivision">区分</param>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="venderCd">取引先コード</param>
                public PrimaryKey(string unitDivision, string venderDivision, string venderCd)
                {
                    UnitDivision = unitDivision;
                    VenderDivision = venderDivision;
                    VenderCd = venderCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.UnitDivision, this.VenderDivision, this.VenderCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="unitDivision">区分</param>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public NumberChkdisitEntity GetEntity(string unitDivision, string venderDivision, string venderCd, ComDB db)
            {
                NumberChkdisitEntity.PrimaryKey condition = new NumberChkdisitEntity.PrimaryKey(unitDivision, venderDivision, venderCd);
                return GetEntity<NumberChkdisitEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 相殺トランザクション
        /// </summary>
        public class OffsetGroupDataEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public OffsetGroupDataEntity()
            {
                TableName = "offset_group_data";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 買掛金、未払金相殺金額</summary>
            /// <value>買掛金、未払金相殺金額</value>
            public decimal? PayableOffsetAmount { get; set; }
            /// <summary>Gets or sets 売掛金、未収金相殺金額</summary>
            /// <value>売掛金、未収金相殺金額</value>
            public decimal? DepositOffsetAmount { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 売掛対象</summary>
            /// <value>売掛対象</value>
            public int? DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 請求対象</summary>
            /// <value>請求対象</value>
            public int? ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛対象</summary>
            /// <value>買掛対象</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象</summary>
            /// <value>支払対象</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ</summary>
            /// <value>売掛更新フラグ</value>
            public int? DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 請求更新フラグ</summary>
            /// <value>請求更新フラグ</value>
            public int? ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ</summary>
            /// <value>買掛更新フラグ</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 支払更新フラグ</summary>
            /// <value>支払更新フラグ</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserOffsetAmount { get; set; }
            /// <summary>Gets or sets 入金消込残</summary>
            /// <value>入金消込残</value>
            public decimal? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 支払消込残</summary>
            /// <value>支払消込残</value>
            public decimal? PaymentEraserAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ</summary>
            /// <value>消込完了フラグ</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 相殺番号</summary>
                /// <value>相殺番号</value>
                public string OffsetNo { get; set; }
                /// <summary>Gets or sets 取引先区分</summary>
                /// <value>取引先区分</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 取引先コード</summary>
                /// <value>取引先コード</value>
                public string VenderCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="offsetNo">相殺番号</param>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="venderCd">取引先コード</param>
                public PrimaryKey(string offsetNo, string venderDivision, string venderCd)
                {
                    OffsetNo = offsetNo;
                    VenderDivision = venderDivision;
                    VenderCd = venderCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OffsetNo, this.VenderDivision, this.VenderCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="offsetNo">相殺番号</param>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public OffsetGroupDataEntity GetEntity(string offsetNo, string venderDivision, string venderCd, ComDB db)
            {
                OffsetGroupDataEntity.PrimaryKey condition = new OffsetGroupDataEntity.PrimaryKey(offsetNo, venderDivision, venderCd);
                return GetEntity<OffsetGroupDataEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 相殺ヘッダ
        /// </summary>
        public class OffsetGroupHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public OffsetGroupHeaderEntity()
            {
                TableName = "offset_group_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string OffsetUserId { get; set; }
            /// <summary>Gets or sets 相殺グループコード</summary>
            /// <value>相殺グループコード</value>
            public string OffsetGroupCd { get; set; }
            /// <summary>Gets or sets データ種別(分類マスタ参照)</summary>
            /// <value>データ種別(分類マスタ参照)</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 相殺日付</summary>
            /// <value>相殺日付</value>
            public DateTime OffsetDate { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int? OffsetStatus { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 相殺番号</summary>
                /// <value>相殺番号</value>
                public string OffsetNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="offsetNo">相殺番号</param>
                public PrimaryKey(string offsetNo)
                {
                    OffsetNo = offsetNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OffsetNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="offsetNo">相殺番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public OffsetGroupHeaderEntity GetEntity(string offsetNo, ComDB db)
            {
                OffsetGroupHeaderEntity.PrimaryKey condition = new OffsetGroupHeaderEntity.PrimaryKey(offsetNo);
                return GetEntity<OffsetGroupHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 工程マスタ
        /// </summary>
        public class OperationEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public OperationEntity()
            {
                TableName = "operation";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工程コード</summary>
            /// <value>工程コード</value>
            public string OperationCd { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string OperationName { get; set; }
            /// <summary>Gets or sets メモ</summary>
            /// <value>メモ</value>
            public string Memo { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 工程コード</summary>
                /// <value>工程コード</value>
                public string OperationCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="operationCd">工程コード</param>
                public PrimaryKey(string operationCd)
                {
                    OperationCd = operationCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OperationCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="operationCd">工程コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public OperationEntity GetEntity(string operationCd, ComDB db)
            {
                OperationEntity.PrimaryKey condition = new OperationEntity.PrimaryKey(operationCd);
                return GetEntity<OperationEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 受注明細
        /// </summary>
        public class OrderDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public OrderDetailEntity()
            {
                TableName = "order_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int OrderRowNo { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目有効開始日</summary>
            /// <value>品目有効開始日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 受注数</summary>
            /// <value>受注数</value>
            public decimal OrderQty { get; set; }
            /// <summary>Gets or sets 受注単価</summary>
            /// <value>受注単価</value>
            public decimal OrderUnitprice { get; set; }
            /// <summary>Gets or sets 仮単価FLG|各種名称マスタ</summary>
            /// <value>仮単価FLG|各種名称マスタ</value>
            public int? TmpUnitpriceFlg { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }
            /// <summary>Gets or sets 金額（税抜）</summary>
            /// <value>金額（税抜）</value>
            public decimal? OrderAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public int? TaxRatio { get; set; }
            /// <summary>Gets or sets 消費税区分</summary>
            /// <value>消費税区分</value>
            public int? TaxDivision { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 受注番号</summary>
                /// <value>受注番号</value>
                public string OrderNo { get; set; }
                /// <summary>Gets or sets 受注行番号</summary>
                /// <value>受注行番号</value>
                public int OrderRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="orderNo">受注番号</param>
                /// <param name="orderRowNo">受注行番号</param>
                public PrimaryKey(string orderNo, int orderRowNo)
                {
                    OrderNo = orderNo;
                    OrderRowNo = orderRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OrderNo, this.OrderRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="orderNo">受注番号</param>
            /// <param name="orderRowNo">受注行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public OrderDetailEntity GetEntity(string orderNo, int orderRowNo, ComDB db)
            {
                OrderDetailEntity.PrimaryKey condition = new OrderDetailEntity.PrimaryKey(orderNo, orderRowNo);
                return GetEntity<OrderDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 受注ヘッダ
        /// </summary>
        public class OrderHeadEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public OrderHeadEntity()
            {
                TableName = "order_head";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets Rev.</summary>
            /// <value>Rev.</value>
            public int? Rev { get; set; }
            /// <summary>Gets or sets 受注日</summary>
            /// <value>受注日</value>
            public DateTime OrderDate { get; set; }
            /// <summary>Gets or sets 受注区分</summary>
            /// <value>受注区分</value>
            public int OrderDivision { get; set; }
            /// <summary>Gets or sets 売上計上基準</summary>
            /// <value>売上計上基準</value>
            public int? RecordSalesBasis { get; set; }
            /// <summary>Gets or sets 単価取得基準日</summary>
            /// <value>単価取得基準日</value>
            public int? PriceReferenceDate { get; set; }
            /// <summary>Gets or sets 受注件名</summary>
            /// <value>受注件名</value>
            public string OrderName { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 受注クローズ日</summary>
            /// <value>受注クローズ日</value>
            public DateTime? OrderCloseDate { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先有効開始日</summary>
            /// <value>取引先有効開始日</value>
            public DateTime? VenderActiveDate { get; set; }
            /// <summary>Gets or sets 取引先担当者名</summary>
            /// <value>取引先担当者名</value>
            public string VenderStaffName { get; set; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 納入先名称</summary>
            /// <value>納入先名称</value>
            public string DeliveryName { get; set; }
            /// <summary>Gets or sets 納入先宛先</summary>
            /// <value>納入先宛先</value>
            public string DeliveryAddress { get; set; }
            /// <summary>Gets or sets 納入先担当者</summary>
            /// <value>納入先担当者</value>
            public string DeliveryStaffName { get; set; }
            /// <summary>Gets or sets 納入先電話番号</summary>
            /// <value>納入先電話番号</value>
            public string DeliveryTelNo { get; set; }
            /// <summary>Gets or sets 帳合コード</summary>
            /// <value>帳合コード</value>
            public string BalanceCd { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 入力担当者コード</summary>
            /// <value>入力担当者コード</value>
            public string EntryUserId { get; set; }
            /// <summary>Gets or sets 営業担当者コード</summary>
            /// <value>営業担当者コード</value>
            public string SalesUserId { get; set; }
            /// <summary>Gets or sets 客先注文番号</summary>
            /// <value>客先注文番号</value>
            public string CustomerOrderNo { get; set; }
            /// <summary>Gets or sets 希望納期</summary>
            /// <value>希望納期</value>
            public DateTime? SuggestedDeliverLimit { get; set; }
            /// <summary>Gets or sets 出荷予定日</summary>
            /// <value>出荷予定日</value>
            public DateTime? ScheduledShippingDate { get; set; }
            /// <summary>Gets or sets 納入予定日</summary>
            /// <value>納入予定日</value>
            public DateTime? DeliveryExpectedDate { get; set; }
            /// <summary>Gets or sets 納入時刻</summary>
            /// <value>納入時刻</value>
            public string DeliveryExpectedTime { get; set; }
            /// <summary>Gets or sets 納品書備考</summary>
            /// <value>納品書備考</value>
            public string DeliverySlipRemark { get; set; }
            /// <summary>Gets or sets 受注時備考</summary>
            /// <value>受注時備考</value>
            public string OrderRemark { get; set; }
            /// <summary>Gets or sets 納期回答区分</summary>
            /// <value>納期回答区分</value>
            public int? PromiseDivition { get; set; }
            /// <summary>Gets or sets 前回ステータス</summary>
            /// <value>前回ステータス</value>
            public int? LastStatus { get; set; }
            /// <summary>Gets or sets 受注明細書発行区分</summary>
            /// <value>受注明細書発行区分</value>
            public int? OrderListOutputDivision { get; set; }
            /// <summary>Gets or sets 受注明細書発行日時</summary>
            /// <value>受注明細書発行日時</value>
            public DateTime? OrderListOutputDate { get; set; }
            /// <summary>Gets or sets 受注明細書発行者ID</summary>
            /// <value>受注明細書発行者ID</value>
            public string OrderListOutputUserId { get; set; }
            /// <summary>Gets or sets 納品書発行区分</summary>
            /// <value>納品書発行区分</value>
            public int? DeliveryNoteOutputDivision { get; set; }
            /// <summary>Gets or sets 納品書発行日時</summary>
            /// <value>納品書発行日時</value>
            public DateTime? DeliveryNoteOutputDate { get; set; }
            /// <summary>Gets or sets 納品書発行者ID</summary>
            /// <value>納品書発行者ID</value>
            public string DeliveryNoteOutputUserId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 受注番号</summary>
                /// <value>受注番号</value>
                public string OrderNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="orderNo">受注番号</param>
                public PrimaryKey(string orderNo)
                {
                    OrderNo = orderNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OrderNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="orderNo">受注番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public OrderHeadEntity GetEntity(string orderNo, ComDB db)
            {
                OrderHeadEntity.PrimaryKey condition = new OrderHeadEntity.PrimaryKey(orderNo);
                return GetEntity<OrderHeadEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 部署マスタ
        /// </summary>
        public class OrganizationMasterEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public OrganizationMasterEntity()
            {
                TableName = "organization_master";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 部署名称</summary>
            /// <value>部署名称</value>
            public string OrganizationName { get; set; }
            /// <summary>Gets or sets 親部署コード</summary>
            /// <value>親部署コード</value>
            public string ParentOrganizationCd { get; set; }
            /// <summary>Gets or sets 郵便番号</summary>
            /// <value>郵便番号</value>
            public string Zipcode { get; set; }
            /// <summary>Gets or sets 住所1</summary>
            /// <value>住所1</value>
            public string Address1 { get; set; }
            /// <summary>Gets or sets 住所2</summary>
            /// <value>住所2</value>
            public string Address2 { get; set; }
            /// <summary>Gets or sets 住所3</summary>
            /// <value>住所3</value>
            public string Address3 { get; set; }
            /// <summary>Gets or sets 電話番号</summary>
            /// <value>電話番号</value>
            public string TelNo { get; set; }
            /// <summary>Gets or sets FAX番号</summary>
            /// <value>FAX番号</value>
            public string FaxNo { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 部署コード</summary>
                /// <value>部署コード</value>
                public string OrganizationCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="organizationCd">部署コード</param>
                public PrimaryKey(string organizationCd)
                {
                    OrganizationCd = organizationCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OrganizationCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="organizationCd">部署コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public OrganizationMasterEntity GetEntity(string organizationCd, ComDB db)
            {
                OrganizationMasterEntity.PrimaryKey condition = new OrganizationMasterEntity.PrimaryKey(organizationCd);
                return GetEntity<OrganizationMasterEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 買掛トランザクション_ヘッダ
        /// </summary>
        public class PayableHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PayableHeaderEntity()
            {
                TableName = "payable_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string SupplierDivision { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime? SupplierActiveDate { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime PayableDate { get; set; }
            /// <summary>Gets or sets 差引繰越額</summary>
            /// <value>差引繰越額</value>
            public decimal? BalanceForward { get; set; }
            /// <summary>Gets or sets 仕入金額</summary>
            /// <value>仕入金額</value>
            public decimal? StockingAmount { get; set; }
            /// <summary>Gets or sets 支払金額</summary>
            /// <value>支払金額</value>
            public decimal? PaymentAmount { get; set; }
            /// <summary>Gets or sets その他支払金額</summary>
            /// <value>その他支払金額</value>
            public decimal? OtherPaymentAmount { get; set; }
            /// <summary>Gets or sets 返品金額</summary>
            /// <value>返品金額</value>
            public decimal? StockingReturnedAmount { get; set; }
            /// <summary>Gets or sets 値引金額</summary>
            /// <value>値引金額</value>
            public decimal? StockingDiscountAmount { get; set; }
            /// <summary>Gets or sets その他仕入金額</summary>
            /// <value>その他仕入金額</value>
            public decimal? OtherStockingAmount { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 買掛残</summary>
            /// <value>買掛残</value>
            public decimal? PayableAmount { get; set; }
            /// <summary>Gets or sets 買掛金(内訳)</summary>
            /// <value>買掛金(内訳)</value>
            public decimal? AccountPayableBreakdown { get; set; }
            /// <summary>Gets or sets 未払金(内訳)</summary>
            /// <value>未払金(内訳)</value>
            public decimal? ArrearageBreakdown { get; set; }
            /// <summary>Gets or sets 以外(内訳)</summary>
            /// <value>以外(内訳)</value>
            public decimal? ExceptBreakdown { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 買掛番号</summary>
                /// <value>買掛番号</value>
                public string PayableNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="payableNo">買掛番号</param>
                public PrimaryKey(string payableNo)
                {
                    PayableNo = payableNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PayableNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="payableNo">買掛番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PayableHeaderEntity GetEntity(string payableNo, ComDB db)
            {
                PayableHeaderEntity.PrimaryKey condition = new PayableHeaderEntity.PrimaryKey(payableNo);
                return GetEntity<PayableHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 支払トランザクション
        /// </summary>
        public class PaymentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PaymentEntity()
            {
                TableName = "payment";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 支払伝票番号</summary>
            /// <value>支払伝票番号</value>
            public string PaymentSlipNo { get; set; }
            /// <summary>Gets or sets 支払伝票行番号</summary>
            /// <value>支払伝票行番号</value>
            public int PaymentSlipRowNo { get; set; }
            /// <summary>Gets or sets 支払ステータス|各種名称マスタ</summary>
            /// <value>支払ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 支払日付</summary>
            /// <value>支払日付</value>
            public DateTime PaymentDate { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets データ種別|(分類マスタ参照)</summary>
            /// <value>データ種別|(分類マスタ参照)</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int CategoryDivision { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 支払金額</summary>
            /// <value>支払金額</value>
            public decimal PaymentAmount { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 支店コード</summary>
            /// <value>支店コード</value>
            public string BranchCd { get; set; }
            /// <summary>Gets or sets 預金種別</summary>
            /// <value>預金種別</value>
            public string DepositType { get; set; }
            /// <summary>Gets or sets 預金勘定</summary>
            /// <value>預金勘定</value>
            public string DepositAccount { get; set; }
            /// <summary>Gets or sets 口座区分|各種名称マスタ</summary>
            /// <value>口座区分|各種名称マスタ</value>
            public int? AccountDivision { get; set; }
            /// <summary>Gets or sets 口座番号</summary>
            /// <value>口座番号</value>
            public string AccountNo { get; set; }
            /// <summary>Gets or sets 手形種別|各種名称マスタ</summary>
            /// <value>手形種別|各種名称マスタ</value>
            public int? NoteDivision { get; set; }
            /// <summary>Gets or sets 手形番号</summary>
            /// <value>手形番号</value>
            public string NoteNo { get; set; }
            /// <summary>Gets or sets 振出日</summary>
            /// <value>振出日</value>
            public DateTime? DrawalDate { get; set; }
            /// <summary>Gets or sets 満期日</summary>
            /// <value>満期日</value>
            public DateTime? NoteDate { get; set; }
            /// <summary>Gets or sets 摘要名</summary>
            /// <value>摘要名</value>
            public string Summary { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 買掛対象|0</summary>
            /// <value>買掛対象|0</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象|0</summary>
            /// <value>支払対象|0</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ|0</summary>
            /// <value>買掛更新フラグ|0</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 支払更新フラグ|0</summary>
            /// <value>支払更新フラグ|0</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }
            /// <summary>Gets or sets 身代</summary>
            /// <value>身代</value>
            public decimal? PaymentAmountEx { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal? TaxRatio { get; set; }
            /// <summary>Gets or sets 消費税コード</summary>
            /// <value>消費税コード</value>
            public string TaxCd { get; set; }
            /// <summary>Gets or sets 消込日</summary>
            /// <value>消込日</value>
            public DateTime? EraserDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 支払伝票番号</summary>
                /// <value>支払伝票番号</value>
                public string PaymentSlipNo { get; set; }
                /// <summary>Gets or sets 支払伝票行番号</summary>
                /// <value>支払伝票行番号</value>
                public int PaymentSlipRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="paymentSlipNo">支払伝票番号</param>
                /// <param name="paymentSlipRowNo">支払伝票行番号</param>
                public PrimaryKey(string paymentSlipNo, int paymentSlipRowNo)
                {
                    PaymentSlipNo = paymentSlipNo;
                    PaymentSlipRowNo = paymentSlipRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PaymentSlipNo, this.PaymentSlipRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="paymentSlipNo">支払伝票番号</param>
            /// <param name="paymentSlipRowNo">支払伝票行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PaymentEntity GetEntity(string paymentSlipNo, int paymentSlipRowNo, ComDB db)
            {
                PaymentEntity.PrimaryKey condition = new PaymentEntity.PrimaryKey(paymentSlipNo, paymentSlipRowNo);
                return GetEntity<PaymentEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 支払ヘッダー
        /// </summary>
        public class PaymentHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PaymentHeaderEntity()
            {
                TableName = "payment_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime PayableDate { get; set; }
            /// <summary>Gets or sets 支払予定日</summary>
            /// <value>支払予定日</value>
            public DateTime PayableScheduledDate { get; set; }
            /// <summary>Gets or sets 支払区分</summary>
            /// <value>支払区分</value>
            public int PayableDivision { get; set; }
            /// <summary>Gets or sets 手形サイト</summary>
            /// <value>手形サイト</value>
            public int? NoteSight { get; set; }
            /// <summary>Gets or sets 休日指定フラグ|各種名称マスタ</summary>
            /// <value>休日指定フラグ|各種名称マスタ</value>
            public int? HolidayFlg { get; set; }
            /// <summary>Gets or sets 前月支払残高</summary>
            /// <value>前月支払残高</value>
            public decimal? ClaimAmountForward { get; set; }
            /// <summary>Gets or sets 支払額</summary>
            /// <value>支払額</value>
            public decimal? CreditAmountForward { get; set; }
            /// <summary>Gets or sets その他支払額</summary>
            /// <value>その他支払額</value>
            public decimal? OtherCreditAmountForward { get; set; }
            /// <summary>Gets or sets 仕入金額</summary>
            /// <value>仕入金額</value>
            public decimal? StockingAmount { get; set; }
            /// <summary>Gets or sets 返品金額</summary>
            /// <value>返品金額</value>
            public decimal? StockingReturnedAmount { get; set; }
            /// <summary>Gets or sets 値引金額</summary>
            /// <value>値引金額</value>
            public decimal? StockingDiscountAmount { get; set; }
            /// <summary>Gets or sets その他仕入金額</summary>
            /// <value>その他仕入金額</value>
            public decimal? OtherStockingAmount { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 当月支払残高</summary>
            /// <value>当月支払残高</value>
            public decimal? PayableAmount { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal? EraserBalanceAmount { get; set; }
            /// <summary>Gets or sets 検収明細通知書発行済フラグ</summary>
            /// <value>検収明細通知書発行済フラグ</value>
            public int? BillDivision { get; set; }
            /// <summary>Gets or sets 発行日時</summary>
            /// <value>発行日時</value>
            public DateTime? IssueDate { get; set; }
            /// <summary>Gets or sets 発行者ID</summary>
            /// <value>発行者ID</value>
            public string IssuerCd { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 支払番号</summary>
                /// <value>支払番号</value>
                public string PaymentNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="paymentNo">支払番号</param>
                public PrimaryKey(string paymentNo)
                {
                    PaymentNo = paymentNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PaymentNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="paymentNo">支払番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PaymentHeaderEntity GetEntity(string paymentNo, ComDB db)
            {
                PaymentHeaderEntity.PrimaryKey condition = new PaymentHeaderEntity.PrimaryKey(paymentNo);
                return GetEntity<PaymentHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 生産計画ログ
        /// </summary>
        public class PlanLogEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PlanLogEntity()
            {
                TableName = "plan_log";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ログ区分</summary>
            /// <value>ログ区分</value>
            public int LogDivision { get; set; }
            /// <summary>Gets or sets 実行日時</summary>
            /// <value>実行日時</value>
            public DateTime CtrlDate { get; set; }
            /// <summary>Gets or sets 処理ステータス</summary>
            /// <value>処理ステータス</value>
            public int? CtrlStatus { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ログ区分</summary>
                /// <value>ログ区分</value>
                public int LogDivision { get; set; }
                /// <summary>Gets or sets 実行日時</summary>
                /// <value>実行日時</value>
                public DateTime CtrlDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="logDivision">ログ区分</param>
                /// <param name="ctrlDate">実行日時</param>
                public PrimaryKey(int logDivision, DateTime ctrlDate)
                {
                    LogDivision = logDivision;
                    CtrlDate = ctrlDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.LogDivision, this.CtrlDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="logDivision">ログ区分</param>
            /// <param name="ctrlDate">実行日時</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PlanLogEntity GetEntity(int logDivision, DateTime ctrlDate, ComDB db)
            {
                PlanLogEntity.PrimaryKey condition = new PlanLogEntity.PrimaryKey(logDivision, ctrlDate);
                return GetEntity<PlanLogEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 生産計画履歴
        /// </summary>
        public class PlanRecordEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PlanRecordEntity()
            {
                TableName = "plan_record";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 生産計画準備処理実行日時</summary>
            /// <value>生産計画準備処理実行日時</value>
            public DateTime PlanPreparationDate { get; set; }
            /// <summary>Gets or sets 品目タイプ|各種名称マスタ</summary>
            /// <value>品目タイプ|各種名称マスタ</value>
            public int ItemType { get; set; }
            /// <summary>Gets or sets 生産計画表発行回数</summary>
            /// <value>生産計画表発行回数</value>
            public int? PlanScheduleIssueCount { get; set; }
            /// <summary>Gets or sets 生産計画取込回数</summary>
            /// <value>生産計画取込回数</value>
            public int? PlanScheduleUpdateCount { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 生産計画準備処理実行日時</summary>
                /// <value>生産計画準備処理実行日時</value>
                public DateTime PlanPreparationDate { get; set; }
                /// <summary>Gets or sets 品目タイプ|各種名称マスタ</summary>
                /// <value>品目タイプ|各種名称マスタ</value>
                public int ItemType { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="planPreparationDate">生産計画準備処理実行日時</param>
                /// <param name="itemType">品目タイプ</param>
                public PrimaryKey(DateTime planPreparationDate, int itemType)
                {
                    PlanPreparationDate = planPreparationDate;
                    ItemType = itemType;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PlanPreparationDate, this.ItemType);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="planPreparationDate">生産計画準備処理実行日時</param>
            /// <param name="itemType">品目タイプ</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PlanRecordEntity GetEntity(DateTime planPreparationDate, int itemType, ComDB db)
            {
                PlanRecordEntity.PrimaryKey condition = new PlanRecordEntity.PrimaryKey(planPreparationDate, itemType);
                return GetEntity<PlanRecordEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 役職マスタ
        /// </summary>
        public class PostEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PostEntity()
            {
                TableName = "post";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 役職ID</summary>
            /// <value>役職ID</value>
            public int PostId { get; set; }
            /// <summary>Gets or sets 役職名</summary>
            /// <value>役職名</value>
            public string PostName { get; set; }
            /// <summary>Gets or sets 承認権限フラグ|0</summary>
            /// <value>承認権限フラグ|0</value>
            public int ApprovalAuthorityFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 役職ID</summary>
                /// <value>役職ID</value>
                public int PostId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="postId">役職ID</param>
                public PrimaryKey(int postId)
                {
                    PostId = postId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PostId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="postId">役職ID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PostEntity GetEntity(int postId, ComDB db)
            {
                PostEntity.PrimaryKey condition = new PostEntity.PrimaryKey(postId);
                return GetEntity<PostEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// バッチ処理実行ログ
        /// </summary>
        public class ProcLogEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ProcLogEntity()
            {
                TableName = "proc_log";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets プロシージャ名</summary>
            /// <value>プロシージャ名</value>
            public string ProcessCd { get; set; }
            /// <summary>Gets or sets 処理日</summary>
            /// <value>処理日</value>
            public DateTime? ProcessDate { get; set; }
            /// <summary>Gets or sets *AP-21標準</summary>
            /// <value>*AP-21標準</value>
            public int? LogSeq { get; set; }
            /// <summary>Gets or sets *AP-21標準</summary>
            /// <value>*AP-21標準</value>
            public int? LogDivision { get; set; }
            /// <summary>Gets or sets *AP-21標準</summary>
            /// <value>*AP-21標準</value>
            public string LogString { get; set; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int? ProcessSeq { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? LogRenban { get; set; }
            /// <summary>Gets or sets 処理名</summary>
            /// <value>処理名</value>
            public string ProcessName { get; set; }
            /// <summary>Gets or sets サブプロシージャ名</summary>
            /// <value>サブプロシージャ名</value>
            public string FuncCd { get; set; }
            /// <summary>Gets or sets ログ区分|0</summary>
            /// <value>ログ区分|0</value>
            public string ProcessDivision { get; set; }
            /// <summary>Gets or sets メッセージ</summary>
            /// <value>メッセージ</value>
            public string Message { get; set; }
            /// <summary>Gets or sets 確認フラグ|0</summary>
            /// <value>確認フラグ|0</value>
            public long? CheckFlg { get; set; }
            /// <summary>Gets or sets ユーザID(20160614ADD)</summary>
            /// <value>ユーザID(20160614ADD)</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets セッションID(20160614ADD)</summary>
            /// <value>セッションID(20160614ADD)</value>
            public string SessionId { get; set; }
            /// <summary>Gets or sets 日時(20160614ADD)</summary>
            /// <value>日時(20160614ADD)</value>
            public string ProcDatetime { get; set; }
            /// <summary>Gets or sets エラーログ翻訳用メッセージID(20160614ADD)</summary>
            /// <value>エラーログ翻訳用メッセージID(20160614ADD)</value>
            public string MessageId { get; set; }
        }

        /// <summary>
        /// バッチ処理実行パラメータ
        /// </summary>
        public class ProcParamEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ProcParamEntity()
            {
                TableName = "proc_param";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets proc_cd</summary>
            /// <value>proc_cd</value>
            public string ProcessCd { get; set; }
            /// <summary>Gets or sets param1</summary>
            /// <value>param1</value>
            public string Param1 { get; set; }
            /// <summary>Gets or sets param2</summary>
            /// <value>param2</value>
            public string Param2 { get; set; }
            /// <summary>Gets or sets param3</summary>
            /// <value>param3</value>
            public string Param3 { get; set; }
            /// <summary>Gets or sets param4</summary>
            /// <value>param4</value>
            public string Param4 { get; set; }
            /// <summary>Gets or sets param5</summary>
            /// <value>param5</value>
            public string Param5 { get; set; }
            /// <summary>Gets or sets param6</summary>
            /// <value>param6</value>
            public string Param6 { get; set; }
            /// <summary>Gets or sets param7</summary>
            /// <value>param7</value>
            public string Param7 { get; set; }
            /// <summary>Gets or sets param8</summary>
            /// <value>param8</value>
            public string Param8 { get; set; }
            /// <summary>Gets or sets param9</summary>
            /// <value>param9</value>
            public string Param9 { get; set; }
            /// <summary>Gets or sets param10</summary>
            /// <value>param10</value>
            public string Param10 { get; set; }
            /// <summary>Gets or sets param11</summary>
            /// <value>param11</value>
            public string Param11 { get; set; }
            /// <summary>Gets or sets param12</summary>
            /// <value>param12</value>
            public string Param12 { get; set; }
            /// <summary>Gets or sets param13</summary>
            /// <value>param13</value>
            public string Param13 { get; set; }
            /// <summary>Gets or sets param14</summary>
            /// <value>param14</value>
            public string Param14 { get; set; }
            /// <summary>Gets or sets param15</summary>
            /// <value>param15</value>
            public string Param15 { get; set; }
            /// <summary>Gets or sets param16</summary>
            /// <value>param16</value>
            public string Param16 { get; set; }
            /// <summary>Gets or sets param17</summary>
            /// <value>param17</value>
            public string Param17 { get; set; }
            /// <summary>Gets or sets param18</summary>
            /// <value>param18</value>
            public string Param18 { get; set; }
            /// <summary>Gets or sets param19</summary>
            /// <value>param19</value>
            public string Param19 { get; set; }
            /// <summary>Gets or sets param20</summary>
            /// <value>param20</value>
            public string Param20 { get; set; }
            /// <summary>Gets or sets param21</summary>
            /// <value>param21</value>
            public string Param21 { get; set; }
            /// <summary>Gets or sets param22</summary>
            /// <value>param22</value>
            public string Param22 { get; set; }
            /// <summary>Gets or sets param23</summary>
            /// <value>param23</value>
            public string Param23 { get; set; }
            /// <summary>Gets or sets param24</summary>
            /// <value>param24</value>
            public string Param24 { get; set; }
            /// <summary>Gets or sets param25</summary>
            /// <value>param25</value>
            public string Param25 { get; set; }
            /// <summary>Gets or sets param26</summary>
            /// <value>param26</value>
            public string Param26 { get; set; }
            /// <summary>Gets or sets param27</summary>
            /// <value>param27</value>
            public string Param27 { get; set; }
            /// <summary>Gets or sets param28</summary>
            /// <value>param28</value>
            public string Param28 { get; set; }
            /// <summary>Gets or sets param29</summary>
            /// <value>param29</value>
            public string Param29 { get; set; }
            /// <summary>Gets or sets param30</summary>
            /// <value>param30</value>
            public string Param30 { get; set; }
            /// <summary>Gets or sets param31</summary>
            /// <value>param31</value>
            public string Param31 { get; set; }
            /// <summary>Gets or sets param32</summary>
            /// <value>param32</value>
            public string Param32 { get; set; }
            /// <summary>Gets or sets param33</summary>
            /// <value>param33</value>
            public string Param33 { get; set; }
            /// <summary>Gets or sets param34</summary>
            /// <value>param34</value>
            public string Param34 { get; set; }
            /// <summary>Gets or sets param35</summary>
            /// <value>param35</value>
            public string Param35 { get; set; }
            /// <summary>Gets or sets param36</summary>
            /// <value>param36</value>
            public string Param36 { get; set; }
            /// <summary>Gets or sets param37</summary>
            /// <value>param37</value>
            public string Param37 { get; set; }
            /// <summary>Gets or sets param38</summary>
            /// <value>param38</value>
            public string Param38 { get; set; }
            /// <summary>Gets or sets param39</summary>
            /// <value>param39</value>
            public string Param39 { get; set; }
            /// <summary>Gets or sets param40</summary>
            /// <value>param40</value>
            public string Param40 { get; set; }
            /// <summary>Gets or sets 処理中フラグ|0</summary>
            /// <value>処理中フラグ|0</value>
            public long CheckFlg { get; set; }
            /// <summary>Gets or sets screen_id</summary>
            /// <value>screen_id</value>
            public long ScreenId { get; set; }
        }

        /// <summary>
        /// 購買　発注受入テーブル(未使用)
        /// </summary>
        public class PurchaseEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PurchaseEntity()
            {
                TableName = "purchase";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 発注番号</summary>
            /// <value>発注番号</value>
            public string BuySubcontractOrderNo { get; set; }
            /// <summary>Gets or sets 発注番号行番号</summary>
            /// <value>発注番号行番号</value>
            public int BuySubcontractOrderRowNo { get; set; }
            /// <summary>Gets or sets 発注日</summary>
            /// <value>発注日</value>
            public DateTime? OrderDate { get; set; }
            /// <summary>Gets or sets 納入希望日</summary>
            /// <value>納入希望日</value>
            public DateTime? SuggestedDeliverlimitDate { get; set; }
            /// <summary>Gets or sets 発注ステータス</summary>
            /// <value>発注ステータス</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 分納枝番</summary>
            /// <value>分納枝番</value>
            public string OrderDivideNo { get; set; }
            /// <summary>Gets or sets 仕入先コード</summary>
            /// <value>仕入先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 納入先コード（通常の場合、ロケーション　仕入直送の場合、受注の納入先）</summary>
            /// <value>納入先コード（通常の場合、ロケーション　仕入直送の場合、受注の納入先）</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目名称（スポット品の場合使用）</summary>
            /// <value>品目名称（スポット品の場合使用）</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 仕様名称（スポット品の場合使用）</summary>
            /// <value>仕様名称（スポット品の場合使用）</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 発注数量</summary>
            /// <value>発注数量</value>
            public decimal? OrderQuantity { get; set; }
            /// <summary>Gets or sets 発注重量</summary>
            /// <value>発注重量</value>
            public decimal? OrderConvertQuantity { get; set; }
            /// <summary>Gets or sets 発注単価</summary>
            /// <value>発注単価</value>
            public decimal? OrderUnitprice { get; set; }
            /// <summary>Gets or sets 単価区分</summary>
            /// <value>単価区分</value>
            public int? UnitpriceDefineunit { get; set; }
            /// <summary>Gets or sets 発注金額</summary>
            /// <value>発注金額</value>
            public decimal? SupplierOrdAmount { get; set; }
            /// <summary>Gets or sets 受入日</summary>
            /// <value>受入日</value>
            public DateTime? AcceptDate { get; set; }
            /// <summary>Gets or sets 受入数量</summary>
            /// <value>受入数量</value>
            public decimal? AcceptQuantity { get; set; }
            /// <summary>Gets or sets 受入重量</summary>
            /// <value>受入重量</value>
            public decimal? AcceptConvertQuantity { get; set; }
            /// <summary>Gets or sets 自社ロット番号</summary>
            /// <value>自社ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets 入庫ロケーション</summary>
            /// <value>入庫ロケーション</value>
            public string HousingLocationCd { get; set; }
            /// <summary>Gets or sets 仕入先ロット番号</summary>
            /// <value>仕入先ロット番号</value>
            public string VenderLotNo { get; set; }
            /// <summary>Gets or sets 仕入先オーダー番号</summary>
            /// <value>仕入先オーダー番号</value>
            public string SiOrderNo { get; set; }
            /// <summary>Gets or sets 分納区分|1</summary>
            /// <value>分納区分|1</value>
            public int? ReplyContentsDivision { get; set; }
            /// <summary>Gets or sets 完納区分</summary>
            /// <value>完納区分</value>
            public int? DeliveryComp { get; set; }
            /// <summary>Gets or sets 次回納品希望日</summary>
            /// <value>次回納品希望日</value>
            public DateTime? NextDeliverlimitDate { get; set; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets 仕入番号</summary>
            /// <value>仕入番号</value>
            public string SlipNo { get; set; }
            /// <summary>Gets or sets 仕入行番号</summary>
            /// <value>仕入行番号</value>
            public int? SilpRowNo { get; set; }
            /// <summary>Gets or sets 入荷数量</summary>
            /// <value>入荷数量</value>
            public decimal? ArrivalQuantity { get; set; }
            /// <summary>Gets or sets 仮単価フラグ</summary>
            /// <value>仮単価フラグ</value>
            public int? TmpUnitpriceFlg { get; set; }
            /// <summary>Gets or sets 購買番号（データとしては入るが未使用）</summary>
            /// <value>購買番号（データとしては入るが未使用）</value>
            public string PurchaseNo { get; set; }
            /// <summary>Gets or sets 発注区分（データとしては入るが未使用）</summary>
            /// <value>発注区分（データとしては入るが未使用）</value>
            public int? OrderDivision { get; set; }
            /// <summary>Gets or sets 担当部署</summary>
            /// <value>担当部署</value>
            public string ChargeOrganizationCd { get; set; }
            /// <summary>Gets or sets 部署</summary>
            /// <value>部署</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 発注担当</summary>
            /// <value>発注担当</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 発注書備考</summary>
            /// <value>発注書備考</value>
            public string OrderSheetRemark { get; set; }
            /// <summary>Gets or sets 発注備考</summary>
            /// <value>発注備考</value>
            public string PurchaseRemark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets 購入依頼承認ステータス</summary>
            /// <value>購入依頼承認ステータス</value>
            public int? PurchaseApprovalStatus { get; set; }
            /// <summary>Gets or sets 購入依頼承認依頼者</summary>
            /// <value>購入依頼承認依頼者</value>
            public string PurchaseApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 購入依頼承認依頼日付</summary>
            /// <value>購入依頼承認依頼日付</value>
            public DateTime? PurchaseApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 購入依頼承認者</summary>
            /// <value>購入依頼承認者</value>
            public string PurchaseApprovalUserId { get; set; }
            /// <summary>Gets or sets 購入依頼承認日付</summary>
            /// <value>購入依頼承認日付</value>
            public DateTime? PurchaseApprovalDate { get; set; }
            /// <summary>Gets or sets 発注入力承認ステータス</summary>
            /// <value>発注入力承認ステータス</value>
            public int? OrderingApprovalStatus { get; set; }
            /// <summary>Gets or sets 発注入力承認依頼者</summary>
            /// <value>発注入力承認依頼者</value>
            public string OrderingApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 発注入力承認依頼日付</summary>
            /// <value>発注入力承認依頼日付</value>
            public DateTime? OrderingApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 発注入力承認者</summary>
            /// <value>発注入力承認者</value>
            public string OrderingApprovalUserId { get; set; }
            /// <summary>Gets or sets 発注入力承認日付</summary>
            /// <value>発注入力承認日付</value>
            public DateTime? OrderingApprovalDate { get; set; }
            /// <summary>Gets or sets 発注書番号</summary>
            /// <value>発注書番号</value>
            public string OrderSheetNo { get; set; }
            /// <summary>Gets or sets 発注書発行フラグ</summary>
            /// <value>発注書発行フラグ</value>
            public int? OrderSheetFlg { get; set; }
            /// <summary>Gets or sets 発注書発行日</summary>
            /// <value>発注書発行日</value>
            public DateTime? OrderSheetDate { get; set; }
            /// <summary>Gets or sets 発注書発行担当</summary>
            /// <value>発注書発行担当</value>
            public string OrderSheetUserId { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 通貨レート</summary>
            /// <value>通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets 通貨日付</summary>
            /// <value>通貨日付</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 発注番号</summary>
                /// <value>発注番号</value>
                public string BuySubcontractOrderNo { get; set; }
                /// <summary>Gets or sets 発注番号行番号</summary>
                /// <value>発注番号行番号</value>
                public int BuySubcontractOrderRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="buySubcontractOrderNo">発注番号</param>
                /// <param name="buySubcontractOrderRowNo">発注番号行番号</param>
                public PrimaryKey(string buySubcontractOrderNo, int buySubcontractOrderRowNo)
                {
                    BuySubcontractOrderNo = buySubcontractOrderNo;
                    BuySubcontractOrderRowNo = buySubcontractOrderRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.BuySubcontractOrderNo, this.BuySubcontractOrderRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="buySubcontractOrderNo">発注番号</param>
            /// <param name="buySubcontractOrderRowNo">発注番号行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PurchaseEntity GetEntity(string buySubcontractOrderNo, int buySubcontractOrderRowNo, ComDB db)
            {
                PurchaseEntity.PrimaryKey condition = new PurchaseEntity.PrimaryKey(buySubcontractOrderNo, buySubcontractOrderRowNo);
                return GetEntity<PurchaseEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 購買外注オーダーファイル
        /// </summary>
        public class PurchaseSubcontractEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PurchaseSubcontractEntity()
            {
                TableName = "purchase_subcontract";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 購買NO</summary>
            /// <value>購買NO</value>
            public string PurchaseNo { get; set; }
            /// <summary>Gets or sets 発注番号</summary>
            /// <value>発注番号</value>
            public string BuySubcontractOrderNo { get; set; }
            /// <summary>Gets or sets 発注日</summary>
            /// <value>発注日</value>
            public DateTime? OrderDate { get; set; }
            /// <summary>Gets or sets 納品希望日</summary>
            /// <value>納品希望日</value>
            public DateTime? SuggestedDeliverlimitDate { get; set; }
            /// <summary>Gets or sets 受入日付</summary>
            /// <value>受入日付</value>
            public DateTime? AcceptDate { get; set; }
            /// <summary>Gets or sets 仕入日付</summary>
            /// <value>仕入日付</value>
            public DateTime? StockingDate { get; set; }
            /// <summary>Gets or sets 伝票番号</summary>
            /// <value>伝票番号</value>
            public string SlipNo { get; set; }
            /// <summary>Gets or sets 仕入行番号</summary>
            /// <value>仕入行番号</value>
            public int? SilpRowNo { get; set; }
            /// <summary>Gets or sets 仕入-取消　仕入番号</summary>
            /// <value>仕入-取消　仕入番号</value>
            public string CancelSlipNo { get; set; }
            /// <summary>Gets or sets 仕入-取消　行番号</summary>
            /// <value>仕入-取消　行番号</value>
            public int? CancelRowNo { get; set; }
            /// <summary>Gets or sets ステータス|各種名称マスタ</summary>
            /// <value>ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 仕入ステータス|各種名称マスタ</summary>
            /// <value>仕入ステータス|各種名称マスタ</value>
            public int? Status2 { get; set; }
            /// <summary>Gets or sets 発注番号分納枝番</summary>
            /// <value>発注番号分納枝番</value>
            public string OrderDivideNo { get; set; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets 製造番号</summary>
            /// <value>製造番号</value>
            public string DirectionNo { get; set; }
            /// <summary>Gets or sets 仕入先受注番</summary>
            /// <value>仕入先受注番</value>
            public string SiOrderNo { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 発注区分|各種名称マスタ</summary>
            /// <value>発注区分|各種名称マスタ</value>
            public int? OrderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string PayeeCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 発注数量</summary>
            /// <value>発注数量</value>
            public decimal? OrderQuantity { get; set; }
            /// <summary>Gets or sets 重量</summary>
            /// <value>重量</value>
            public decimal? OrderConvertQuantity { get; set; }
            /// <summary>Gets or sets 発注単価</summary>
            /// <value>発注単価</value>
            public decimal? OrderUnitprice { get; set; }
            /// <summary>Gets or sets 単価決定単位区分|各種名称マスタ</summary>
            /// <value>単価決定単位区分|各種名称マスタ</value>
            public int? UnitpriceDefineunit { get; set; }
            /// <summary>Gets or sets 発注金額</summary>
            /// <value>発注金額</value>
            public decimal? SupplierOrdAmount { get; set; }
            /// <summary>Gets or sets メーカーロット番号</summary>
            /// <value>メーカーロット番号</value>
            public string VenderLotNo { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets 次回納品希望日</summary>
            /// <value>次回納品希望日</value>
            public DateTime? NextDeliverlimitDate { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 入庫ロケーション</summary>
            /// <value>入庫ロケーション</value>
            public string HousingLocationCd { get; set; }
            /// <summary>Gets or sets 入荷予定数量</summary>
            /// <value>入荷予定数量</value>
            public decimal? ArrivalQuantity { get; set; }
            /// <summary>Gets or sets 受入数量</summary>
            /// <value>受入数量</value>
            public decimal? AcceptQuantity { get; set; }
            /// <summary>Gets or sets 合格数換算量</summary>
            /// <value>合格数換算量</value>
            public decimal? AcceptConvertQuantity { get; set; }
            /// <summary>Gets or sets 仕入数量</summary>
            /// <value>仕入数量</value>
            public decimal? StockingQuantity { get; set; }
            /// <summary>Gets or sets 仕入単価</summary>
            /// <value>仕入単価</value>
            public decimal? HousingUnitprice { get; set; }
            /// <summary>Gets or sets 運賃</summary>
            /// <value>運賃</value>
            public decimal? FareAmount { get; set; }
            /// <summary>Gets or sets 仕入金額</summary>
            /// <value>仕入金額</value>
            public decimal? StockingAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 仮単価FLG|各種名称マスタ</summary>
            /// <value>仮単価FLG|各種名称マスタ</value>
            public int? TmpUnitpriceFlg { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal? TaxRatio { get; set; }
            /// <summary>Gets or sets 消費税課税区分|各種名称マスタ</summary>
            /// <value>消費税課税区分|各種名称マスタ</value>
            public int? TaxDivision { get; set; }
            /// <summary>Gets or sets 会計部門借方コード</summary>
            /// <value>会計部門借方コード</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 会計部門貸方コード</summary>
            /// <value>会計部門貸方コード</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 買掛対象|0</summary>
            /// <value>買掛対象|0</value>
            public int? PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象|0</summary>
            /// <value>支払対象|0</value>
            public int? PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ|0</summary>
            /// <value>買掛更新フラグ|0</value>
            public int? PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 支払更新フラグ|0</summary>
            /// <value>支払更新フラグ|0</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 検査方法|各種名称マスタ</summary>
            /// <value>検査方法|各種名称マスタ</value>
            public int? InspectMethod { get; set; }
            /// <summary>Gets or sets 発注書NO</summary>
            /// <value>発注書NO</value>
            public string OrderSheetNo { get; set; }
            /// <summary>Gets or sets 発注書発行フラグ|各種名称マスタ</summary>
            /// <value>発注書発行フラグ|各種名称マスタ</value>
            public int? OrderSheetFlg { get; set; }
            /// <summary>Gets or sets 発注書発行日</summary>
            /// <value>発注書発行日</value>
            public DateTime? OrderSheetDate { get; set; }
            /// <summary>Gets or sets 発注書発行者</summary>
            /// <value>発注書発行者</value>
            public string OrderSheetUserId { get; set; }
            /// <summary>Gets or sets 分納区分|各種名称マスタ</summary>
            /// <value>分納区分|各種名称マスタ</value>
            public int? ReplyContentsDivision { get; set; }
            /// <summary>Gets or sets 完納区分|0</summary>
            /// <value>完納区分|0</value>
            public int? DeliveryComp { get; set; }
            /// <summary>Gets or sets 仕入伝票発行済み区分|各種名称マスタ</summary>
            /// <value>仕入伝票発行済み区分|各種名称マスタ</value>
            public int? SlipIssueDivision { get; set; }
            /// <summary>Gets or sets 仕入伝票発行日</summary>
            /// <value>仕入伝票発行日</value>
            public DateTime? SlipIssueDate { get; set; }
            /// <summary>Gets or sets 担当部署コード</summary>
            /// <value>担当部署コード</value>
            public string ChargeOrganizationCd { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }
            /// <summary>Gets or sets 発注書備考</summary>
            /// <value>発注書備考</value>
            public string OrderSheetRemark { get; set; }
            /// <summary>Gets or sets 発注時備考</summary>
            /// <value>発注時備考</value>
            public string PurchaseRemark { get; set; }
            /// <summary>Gets or sets 発注書備考(入荷以降)</summary>
            /// <value>発注書備考(入荷以降)</value>
            public string OrderSheetRemark2 { get; set; }
            /// <summary>Gets or sets 備考(入荷以降)</summary>
            /// <value>備考(入荷以降)</value>
            public string Remark2 { get; set; }
            /// <summary>Gets or sets 検査判定フラグ|各種名称マスタ</summary>
            /// <value>検査判定フラグ|各種名称マスタ</value>
            public int? CertificationFlg { get; set; }
            /// <summary>Gets or sets データ種別|1</summary>
            /// <value>データ種別|1</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 仕訳作成ステータス|0</summary>
            /// <value>仕訳作成ステータス|0</value>
            public long? ShiwakeMakeStatus { get; set; }
            /// <summary>Gets or sets 仕訳送信ステータス|0</summary>
            /// <value>仕訳送信ステータス|0</value>
            public long? ShiwakeSendStatus { get; set; }
            /// <summary>Gets or sets 有効フラグ|0</summary>
            /// <value>有効フラグ|0</value>
            public long? ExNumeric01 { get; set; }
            /// <summary>Gets or sets 購入依頼 承認ステータス|各種名称マスタ</summary>
            /// <value>購入依頼 承認ステータス|各種名称マスタ</value>
            public int? PurchaseApprovalStatus { get; set; }
            /// <summary>Gets or sets 購入依頼 承認依頼者</summary>
            /// <value>購入依頼 承認依頼者</value>
            public string PurchaseApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 購入依頼 承認依頼日付</summary>
            /// <value>購入依頼 承認依頼日付</value>
            public DateTime? PurchaseApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 購入依頼 承認者</summary>
            /// <value>購入依頼 承認者</value>
            public string PurchaseApprovalUserId { get; set; }
            /// <summary>Gets or sets 購入依頼 承認日付</summary>
            /// <value>購入依頼 承認日付</value>
            public DateTime? PurchaseApprovalDate { get; set; }
            /// <summary>Gets or sets 発注入力 承認ステータス|各種名称マスタ</summary>
            /// <value>発注入力 承認ステータス|各種名称マスタ</value>
            public int? OrderingApprovalStatus { get; set; }
            /// <summary>Gets or sets 発注入力 承認依頼者</summary>
            /// <value>発注入力 承認依頼者</value>
            public string OrderingApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 発注入力 承認依頼日付</summary>
            /// <value>発注入力 承認依頼日付</value>
            public DateTime? OrderingApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 発注入力 承認者</summary>
            /// <value>発注入力 承認者</value>
            public string OrderingApprovalUserId { get; set; }
            /// <summary>Gets or sets 発注入力 承認日付</summary>
            /// <value>発注入力 承認日付</value>
            public DateTime? OrderingApprovalDate { get; set; }
            /// <summary>Gets or sets 仕入 承認ステータス|各種名称マスタ</summary>
            /// <value>仕入 承認ステータス|各種名称マスタ</value>
            public int? BuyingApprovalStatus { get; set; }
            /// <summary>Gets or sets 仕入 承認依頼者</summary>
            /// <value>仕入 承認依頼者</value>
            public string BuyingApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 仕入 承認依頼日付</summary>
            /// <value>仕入 承認依頼日付</value>
            public DateTime? BuyingApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 仕入 承認者</summary>
            /// <value>仕入 承認者</value>
            public string BuyingApprovalUserId { get; set; }
            /// <summary>Gets or sets 仕入 承認日付</summary>
            /// <value>仕入 承認日付</value>
            public DateTime? BuyingApprovalDate { get; set; }
            /// <summary>Gets or sets 購入依頼 承認依頼先担当者</summary>
            /// <value>購入依頼 承認依頼先担当者</value>
            public string PurchaseReceiveUserId { get; set; }
            /// <summary>Gets or sets メール送信ステータス(購入依頼：承認依頼)|0</summary>
            /// <value>メール送信ステータス(購入依頼：承認依頼)|0</value>
            public int? PurchaseSendMailApprStatus { get; set; }
            /// <summary>Gets or sets メール送信ステータス(購入依頼：否認)|0</summary>
            /// <value>メール送信ステータス(購入依頼：否認)|0</value>
            public int? PurchaseSendMailDisalwStatus { get; set; }
            /// <summary>Gets or sets 発注入力 承認依頼先担当者</summary>
            /// <value>発注入力 承認依頼先担当者</value>
            public string OrderingReceiveUserId { get; set; }
            /// <summary>Gets or sets メール送信ステータス(発注依頼：承認依頼)|0</summary>
            /// <value>メール送信ステータス(発注依頼：承認依頼)|0</value>
            public int? OrderingSendMailApprStatus { get; set; }
            /// <summary>Gets or sets メール送信ステータス(発注依頼：否認)|0</summary>
            /// <value>メール送信ステータス(発注依頼：否認)|0</value>
            public int? OrderingSendMailDisalwStatus { get; set; }
            /// <summary>Gets or sets 仕入 承認依頼先担当者</summary>
            /// <value>仕入 承認依頼先担当者</value>
            public string BuyingReceiveUserId { get; set; }
            /// <summary>Gets or sets メール送信ステータス(仕入：承認依頼)|0</summary>
            /// <value>メール送信ステータス(仕入：承認依頼)|0</value>
            public int? BuyingSendMailApprStatus { get; set; }
            /// <summary>Gets or sets メール送信ステータス(仕入：否認)|0</summary>
            /// <value>メール送信ステータス(仕入：否認)|0</value>
            public int? BuyingSendMailDisalwStatus { get; set; }
            /// <summary>Gets or sets 借方補助科目コード(未使用)</summary>
            /// <value>借方補助科目コード(未使用)</value>
            public string DebitSubTitleCd { get; set; }
            /// <summary>Gets or sets 貸方補助科目コード(未使用)</summary>
            /// <value>貸方補助科目コード(未使用)</value>
            public string CreditSubTitleCd { get; set; }
            /// <summary>Gets or sets 注釈(未使用)</summary>
            /// <value>注釈(未使用)</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets 摘要名(未使用)</summary>
            /// <value>摘要名(未使用)</value>
            public string Summary { get; set; }
            /// <summary>Gets or sets 注釈(入荷以降)(未使用)</summary>
            /// <value>注釈(入荷以降)(未使用)</value>
            public long? Notes2 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目01(未使用)</summary>
            /// <value>拡張用文字列項目01(未使用)</value>
            public string ExString01 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目02(未使用)</summary>
            /// <value>拡張用文字列項目02(未使用)</value>
            public string ExString02 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目03(未使用)</summary>
            /// <value>拡張用文字列項目03(未使用)</value>
            public string ExString03 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目04(未使用)</summary>
            /// <value>拡張用文字列項目04(未使用)</value>
            public string ExString04 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目05(未使用)</summary>
            /// <value>拡張用文字列項目05(未使用)</value>
            public string ExString05 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目02(未使用)</summary>
            /// <value>拡張用数値項目02(未使用)</value>
            public long? ExNumeric02 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目03(未使用)</summary>
            /// <value>拡張用数値項目03(未使用)</value>
            public long? ExNumeric03 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目04(未使用)</summary>
            /// <value>拡張用数値項目04(未使用)</value>
            public long? ExNumeric04 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目05(未使用)</summary>
            /// <value>拡張用数値項目05(未使用)</value>
            public long? ExNumeric05 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目01(未使用)</summary>
            /// <value>拡張用日付項目01(未使用)</value>
            public DateTime? ExDate01 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目02(未使用)</summary>
            /// <value>拡張用日付項目02(未使用)</value>
            public DateTime? ExDate02 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目03(未使用)</summary>
            /// <value>拡張用日付項目03(未使用)</value>
            public DateTime? ExDate03 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目04(未使用)</summary>
            /// <value>拡張用日付項目04(未使用)</value>
            public DateTime? ExDate04 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目05(未使用)</summary>
            /// <value>拡張用日付項目05(未使用)</value>
            public DateTime? ExDate05 { get; set; }
            /// <summary>Gets or sets 承認者(未使用)</summary>
            /// <value>承認者(未使用)</value>
            public string ApprovedBy { get; set; }
            /// <summary>Gets or sets 承認ステータス(未使用)|各種名称マスタ</summary>
            /// <value>承認ステータス(未使用)|各種名称マスタ</value>
            public int? ApprovalStatus { get; set; }
            /// <summary>Gets or sets 承認者(未使用)</summary>
            /// <value>承認者(未使用)</value>
            public string ApprovalUserId { get; set; }
            /// <summary>Gets or sets 承認依頼者(未使用)</summary>
            /// <value>承認依頼者(未使用)</value>
            public string ApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 承認依頼日付(未使用)</summary>
            /// <value>承認依頼日付(未使用)</value>
            public DateTime? ApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 承認日付(未使用)</summary>
            /// <value>承認日付(未使用)</value>
            public DateTime? ApprovalDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 購買NO</summary>
                /// <value>購買NO</value>
                public string PurchaseNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="purchaseNo">購買NO</param>
                public PrimaryKey(string purchaseNo)
                {
                    PurchaseNo = purchaseNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PurchaseNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="purchaseNo">購買NO</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PurchaseSubcontractEntity GetEntity(string purchaseNo, ComDB db)
            {
                PurchaseSubcontractEntity.PrimaryKey condition = new PurchaseSubcontractEntity.PrimaryKey(purchaseNo);
                return GetEntity<PurchaseSubcontractEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 理由マスタ
        /// </summary>
        public class ReasonEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ReasonEntity()
            {
                TableName = "reason";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 理由コード</summary>
            /// <value>理由コード</value>
            public string ReasonCd { get; set; }
            /// <summary>Gets or sets 理由内容</summary>
            /// <value>理由内容</value>
            public string RyDescription { get; set; }
            /// <summary>Gets or sets デフォルトフラグ|0</summary>
            /// <value>デフォルトフラグ|0</value>
            public int? DefaultFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 理由コード</summary>
                /// <value>理由コード</value>
                public string ReasonCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="reasonCd">理由コード</param>
                public PrimaryKey(string reasonCd)
                {
                    ReasonCd = reasonCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ReasonCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="reasonCd">理由コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ReasonEntity GetEntity(string reasonCd, ComDB db)
            {
                ReasonEntity.PrimaryKey condition = new ReasonEntity.PrimaryKey(reasonCd);
                return GetEntity<ReasonEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// BOM&処方マスタ_詳細情報
        /// </summary>
        public class RecipeFileLinkEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RecipeFileLinkEntity()
            {
                TableName = "recipe_file_link";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long RecipeId { get; set; }
            /// <summary>Gets or sets SEQ番号</summary>
            /// <value>SEQ番号</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets 表示名</summary>
            /// <value>表示名</value>
            public string DisplayName { get; set; }
            /// <summary>Gets or sets リンク情報</summary>
            /// <value>リンク情報</value>
            public string LinkUrl { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets レシピインデックス</summary>
                /// <value>レシピインデックス</value>
                public long RecipeId { get; set; }
                /// <summary>Gets or sets SEQ番号</summary>
                /// <value>SEQ番号</value>
                public int SeqNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="recipeId">レシピインデックス</param>
                /// <param name="seqNo">SEQ番号</param>
                public PrimaryKey(long recipeId, int seqNo)
                {
                    RecipeId = recipeId;
                    SeqNo = seqNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RecipeId, this.SeqNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="recipeId">レシピインデックス</param>
            /// <param name="seqNo">SEQ番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RecipeFileLinkEntity GetEntity(long recipeId, int seqNo, ComDB db)
            {
                RecipeFileLinkEntity.PrimaryKey condition = new RecipeFileLinkEntity.PrimaryKey(recipeId, seqNo);
                return GetEntity<RecipeFileLinkEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// BOM&処方マスタ_フォーミュラ(投入,出来高明細)
        /// </summary>
        public class RecipeFormulaEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RecipeFormulaEntity()
            {
                TableName = "recipe_formula";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long RecipeId { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets 実績区分</summary>
            /// <value>実績区分</value>
            public int ResultDivision { get; set; }
            /// <summary>Gets or sets シーケンスNO</summary>
            /// <value>シーケンスNO</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets サブステップNO</summary>
            /// <value>サブステップNO</value>
            public int? SubStep { get; set; }
            /// <summary>Gets or sets 品目タイプ|各種名称マスタ</summary>
            /// <value>品目タイプ|各種名称マスタ</value>
            public int? LineType { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 仕上NO</summary>
            /// <value>仕上NO</value>
            public int? FinishNo { get; set; }
            /// <summary>Gets or sets 数量</summary>
            /// <value>数量</value>
            public decimal? Qty { get; set; }
            /// <summary>Gets or sets 部品入力区分</summary>
            /// <value>部品入力区分</value>
            public int? PartInputDivision { get; set; }
            /// <summary>Gets or sets 条件</summary>
            /// <value>条件</value>
            public string Condition { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets 単位|各種名称マスタ</summary>
            /// <value>単位|各種名称マスタ</value>
            public string FillingUnit { get; set; }
            /// <summary>Gets or sets 収率計算フラグ</summary>
            /// <value>収率計算フラグ</value>
            public int? YieldIncludeFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets レシピインデックス</summary>
                /// <value>レシピインデックス</value>
                public long RecipeId { get; set; }
                /// <summary>Gets or sets 工程番号</summary>
                /// <value>工程番号</value>
                public int StepNo { get; set; }
                /// <summary>Gets or sets 実績区分</summary>
                /// <value>実績区分</value>
                public int ResultDivision { get; set; }
                /// <summary>Gets or sets シーケンスNO</summary>
                /// <value>シーケンスNO</value>
                public int SeqNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="recipeId">レシピインデックス</param>
                /// <param name="stepNo">工程番号</param>
                /// <param name="resultDivision">実績区分</param>
                /// <param name="seqNo">シーケンスNO</param>
                public PrimaryKey(long recipeId, int stepNo, int resultDivision, int seqNo)
                {
                    RecipeId = recipeId;
                    StepNo = stepNo;
                    ResultDivision = resultDivision;
                    SeqNo = seqNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RecipeId, this.StepNo, this.ResultDivision, this.SeqNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="recipeId">レシピインデックス</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="resultDivision">実績区分</param>
            /// <param name="seqNo">シーケンスNO</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RecipeFormulaEntity GetEntity(long recipeId, int stepNo, int resultDivision, int seqNo, ComDB db)
            {
                RecipeFormulaEntity.PrimaryKey condition = new RecipeFormulaEntity.PrimaryKey(recipeId, stepNo, resultDivision, seqNo);
                return GetEntity<RecipeFormulaEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// BOM&処方マスタ_ヘッダ
        /// </summary>
        public class RecipeHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RecipeHeaderEntity()
            {
                TableName = "recipe_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long RecipeId { get; set; }
            /// <summary>Gets or sets レシピコード</summary>
            /// <value>レシピコード</value>
            public string RecipeCd { get; set; }
            /// <summary>Gets or sets レシピバージョン</summary>
            /// <value>レシピバージョン</value>
            public long? RecipeVersion { get; set; }
            /// <summary>Gets or sets レシピタイプ|各種名称マスタ</summary>
            /// <value>レシピタイプ|各種名称マスタ</value>
            public int? RecipeType { get; set; }
            /// <summary>Gets or sets 原処方レシピインデックス</summary>
            /// <value>原処方レシピインデックス</value>
            public long? OriginalRecipeId { get; set; }
            /// <summary>Gets or sets 用途|各種名称マスタ</summary>
            /// <value>用途|各種名称マスタ</value>
            public int? RecipeUse { get; set; }
            /// <summary>Gets or sets 処方名称</summary>
            /// <value>処方名称</value>
            public string RecipeName { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets レシピステータス|各種名称マスタ</summary>
            /// <value>レシピステータス|各種名称マスタ</value>
            public int? RecipeStatus { get; set; }
            /// <summary>Gets or sets 出来高|配合量(合計)に対する標準生産量または収率(%)またはロス率(%)</summary>
            /// <value>出来高|配合量(合計)に対する標準生産量または収率(%)またはロス率(%)</value>
            public decimal? Yield { get; set; }
            /// <summary>Gets or sets 生産ライン</summary>
            /// <value>生産ライン</value>
            public string ProductionLine { get; set; }
            /// <summary>Gets or sets 製造リードタイム</summary>
            /// <value>製造リードタイム</value>
            public int? ProductionLeadTime { get; set; }
            /// <summary>Gets or sets 安全リードタイム</summary>
            /// <value>安全リードタイム</value>
            public int? ProductSafetyLeadTime { get; set; }
            /// <summary>Gets or sets 標準日別生産量</summary>
            /// <value>標準日別生産量</value>
            public decimal? StandardRatePerday { get; set; }
            /// <summary>Gets or sets 基準外注先</summary>
            /// <value>基準外注先</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets レシピ優先度</summary>
            /// <value>レシピ優先度</value>
            public int? RecipePriority { get; set; }
            /// <summary>Gets or sets 標準生産量</summary>
            /// <value>標準生産量</value>
            public decimal? StdQty { get; set; }
            /// <summary>Gets or sets 単位生産量</summary>
            /// <value>単位生産量</value>
            public decimal? UnitQty { get; set; }
            /// <summary>Gets or sets 最小生産量</summary>
            /// <value>最小生産量</value>
            public decimal? MinQty { get; set; }
            /// <summary>Gets or sets 最大生産量</summary>
            /// <value>最大生産量</value>
            public decimal? MaxQty { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime? StartDate { get; set; }
            /// <summary>Gets or sets 有効終了日</summary>
            /// <value>有効終了日</value>
            public DateTime? EndDate { get; set; }
            /// <summary>Gets or sets レシピ注釈</summary>
            /// <value>レシピ注釈</value>
            public string RecipeDescription { get; set; }
            /// <summary>Gets or sets レシピ備考</summary>
            /// <value>レシピ備考</value>
            public string RecipeMemo { get; set; }
            /// <summary>Gets or sets 承認依頼先担当者ID</summary>
            /// <value>承認依頼先担当者ID</value>
            public string ReceiveUserId { get; set; }
            /// <summary>Gets or sets 承認依頼者ID(原処方)</summary>
            /// <value>承認依頼者ID(原処方)</value>
            public string ApprovedByRequest { get; set; }
            /// <summary>Gets or sets 承認依頼日時(原処方)</summary>
            /// <value>承認依頼日時(原処方)</value>
            public DateTime? ApprovalDateRequest { get; set; }
            /// <summary>Gets or sets 承認者ID(原処方)</summary>
            /// <value>承認者ID(原処方)</value>
            public string ApprovalId { get; set; }
            /// <summary>Gets or sets 承認日時(BOM)</summary>
            /// <value>承認日時(BOM)</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets 承認依頼者ID(BOM)</summary>
            /// <value>承認依頼者ID(BOM)</value>
            public string ApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 承認依頼日時(BOM)</summary>
            /// <value>承認依頼日時(BOM)</value>
            public DateTime? ApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 承認者ID(BOM)</summary>
            /// <value>承認者ID(BOM)</value>
            public string ApprovalUserId { get; set; }
            /// <summary>Gets or sets 有効フラグ</summary>
            /// <value>有効フラグ</value>
            public int ActivateFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int? DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets レシピインデックス</summary>
                /// <value>レシピインデックス</value>
                public long RecipeId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="recipeId">レシピインデックス</param>
                public PrimaryKey(long recipeId)
                {
                    RecipeId = recipeId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RecipeId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="recipeId">レシピインデックス</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RecipeHeaderEntity GetEntity(long recipeId, ComDB db)
            {
                RecipeHeaderEntity.PrimaryKey condition = new RecipeHeaderEntity.PrimaryKey(recipeId);
                return GetEntity<RecipeHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// BOM&処方マスタ_検査(未使用)
        /// </summary>
        public class RecipeInspectionEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RecipeInspectionEntity()
            {
                TableName = "recipe_inspection";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long RecipeId { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int LineNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? Seq { get; set; }
            /// <summary>Gets or sets 検査コード|各種名称マスタ</summary>
            /// <value>検査コード|各種名称マスタ</value>
            public string InspectionCd { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets レシピインデックス</summary>
                /// <value>レシピインデックス</value>
                public long RecipeId { get; set; }
                /// <summary>Gets or sets 工程番号</summary>
                /// <value>工程番号</value>
                public int StepNo { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int LineNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="recipeId">レシピインデックス</param>
                /// <param name="stepNo">工程番号</param>
                /// <param name="lineNo">行番号</param>
                public PrimaryKey(long recipeId, int stepNo, int lineNo)
                {
                    RecipeId = recipeId;
                    StepNo = stepNo;
                    LineNo = lineNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RecipeId, this.StepNo, this.LineNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="recipeId">レシピインデックス</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="lineNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RecipeInspectionEntity GetEntity(long recipeId, int stepNo, int lineNo, ComDB db)
            {
                RecipeInspectionEntity.PrimaryKey condition = new RecipeInspectionEntity.PrimaryKey(recipeId, stepNo, lineNo);
                return GetEntity<RecipeInspectionEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// BOM&処方マスタ_検査規格値(未使用)
        /// </summary>
        public class RecipeInspectionValueEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RecipeInspectionValueEntity()
            {
                TableName = "recipe_inspection_value";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 区分|各種名称マスタ</summary>
            /// <value>区分|各種名称マスタ</value>
            public string Division { get; set; }
            /// <summary>Gets or sets 値種類|各種名称マスタ</summary>
            /// <value>値種類|各種名称マスタ</value>
            public int? ValueType { get; set; }
            /// <summary>Gets or sets 値From</summary>
            /// <value>値From</value>
            public string Value1 { get; set; }
            /// <summary>Gets or sets 比較条件|各種名称マスタ</summary>
            /// <value>比較条件|各種名称マスタ</value>
            public string Comparison { get; set; }
            /// <summary>Gets or sets 値To</summary>
            /// <value>値To</value>
            public string Value2 { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long RecipeId { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int LineNo { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 表示順</summary>
                /// <value>表示順</value>
                public int SeqNo { get; set; }
                /// <summary>Gets or sets レシピインデックス</summary>
                /// <value>レシピインデックス</value>
                public long RecipeId { get; set; }
                /// <summary>Gets or sets 工程番号</summary>
                /// <value>工程番号</value>
                public int StepNo { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int LineNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="seqNo">表示順</param>
                /// <param name="recipeId">レシピインデックス</param>
                /// <param name="stepNo">工程番号</param>
                /// <param name="lineNo">行番号</param>
                public PrimaryKey(int seqNo, long recipeId, int stepNo, int lineNo)
                {
                    SeqNo = seqNo;
                    RecipeId = recipeId;
                    StepNo = stepNo;
                    LineNo = lineNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SeqNo, this.RecipeId, this.StepNo, this.LineNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="seqNo">表示順</param>
            /// <param name="recipeId">レシピインデックス</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="lineNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RecipeInspectionValueEntity GetEntity(int seqNo, long recipeId, int stepNo, int lineNo, ComDB db)
            {
                RecipeInspectionValueEntity.PrimaryKey condition = new RecipeInspectionValueEntity.PrimaryKey(seqNo, recipeId, stepNo, lineNo);
                return GetEntity<RecipeInspectionValueEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// BOM&処方マスタ_プロシージャ(工程情報)
        /// </summary>
        public class RecipeProcedureEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RecipeProcedureEntity()
            {
                TableName = "recipe_procedure";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long RecipeId { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? Seq { get; set; }
            /// <summary>Gets or sets 工程コード</summary>
            /// <value>工程コード</value>
            public string OperationCd { get; set; }
            /// <summary>Gets or sets 作業時間</summary>
            /// <value>作業時間</value>
            public decimal? WorkingMinutes { get; set; }
            /// <summary>Gets or sets 工程条件自由入力</summary>
            /// <value>工程条件自由入力</value>
            public string Condition { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets 正味質量(未使用)</summary>
            /// <value>正味質量(未使用)</value>
            public long? Net { get; set; }
            /// <summary>Gets or sets 前工程NO(未使用)</summary>
            /// <value>前工程NO(未使用)</value>
            public int? FromNo { get; set; }
            /// <summary>Gets or sets 後工程NO(未使用)</summary>
            /// <value>後工程NO(未使用)</value>
            public int? ToNo { get; set; }
            /// <summary>Gets or sets 単位(未使用)</summary>
            /// <value>単位(未使用)</value>
            public string FillingUnit { get; set; }
            /// <summary>Gets or sets 設備コード(未使用)</summary>
            /// <value>設備コード(未使用)</value>
            public string ResouceCd { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets レシピインデックス</summary>
                /// <value>レシピインデックス</value>
                public long RecipeId { get; set; }
                /// <summary>Gets or sets 工程番号</summary>
                /// <value>工程番号</value>
                public int StepNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="recipeId">レシピインデックス</param>
                /// <param name="stepNo">工程番号</param>
                public PrimaryKey(long recipeId, int stepNo)
                {
                    RecipeId = recipeId;
                    StepNo = stepNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RecipeId, this.StepNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="recipeId">レシピインデックス</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RecipeProcedureEntity GetEntity(long recipeId, int stepNo, ComDB db)
            {
                RecipeProcedureEntity.PrimaryKey condition = new RecipeProcedureEntity.PrimaryKey(recipeId, stepNo);
                return GetEntity<RecipeProcedureEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// BOM&処方マスタ_その他
        /// </summary>
        public class RecipeRemarkEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RecipeRemarkEntity()
            {
                TableName = "recipe_remark";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long RecipeId { get; set; }
            /// <summary>Gets or sets 原処方備考</summary>
            /// <value>原処方備考</value>
            public string GeneralRecipeRemark { get; set; }
            /// <summary>Gets or sets 基本処方備考</summary>
            /// <value>基本処方備考</value>
            public string MasterRecipeRemark { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets レシピインデックス</summary>
                /// <value>レシピインデックス</value>
                public long RecipeId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="recipeId">レシピインデックス</param>
                public PrimaryKey(long recipeId)
                {
                    RecipeId = recipeId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RecipeId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="recipeId">レシピインデックス</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RecipeRemarkEntity GetEntity(long recipeId, ComDB db)
            {
                RecipeRemarkEntity.PrimaryKey condition = new RecipeRemarkEntity.PrimaryKey(recipeId);
                return GetEntity<RecipeRemarkEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 備考マスタ
        /// </summary>
        public class RemarkEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RemarkEntity()
            {
                TableName = "remark";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 備考番号</summary>
            /// <value>備考番号</value>
            public long RemarkNo { get; set; }
            /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
            /// <value>取引先区分|各種名称マスタ</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 出荷時社内備考</summary>
            /// <value>出荷時社内備考</value>
            public string ShippingRemark { get; set; }
            /// <summary>Gets or sets 出荷指示書備考</summary>
            /// <value>出荷指示書備考</value>
            public string ShippingSlipRemark { get; set; }
            /// <summary>Gets or sets 受注時備考</summary>
            /// <value>受注時備考</value>
            public string OrderRemark { get; set; }
            /// <summary>Gets or sets 納品書備考</summary>
            /// <value>納品書備考</value>
            public string DeliverySlipRemark { get; set; }
            /// <summary>Gets or sets 発注時備考</summary>
            /// <value>発注時備考</value>
            public string PurchaseRemark { get; set; }
            /// <summary>Gets or sets 発注書備考</summary>
            /// <value>発注書備考</value>
            public string OrderSheetRemark { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }
        }

        /// <summary>
        /// REPOPA詳細
        /// </summary>
        public class RepopaDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RepopaDetailEntity()
            {
                TableName = "repopa_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets レポートID</summary>
            /// <value>レポートID</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets 詳細ID</summary>
            /// <value>詳細ID</value>
            public string DetailId { get; set; }
            /// <summary>Gets or sets サブURL</summary>
            /// <value>サブURL</value>
            public string SubUrl { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets レポートID</summary>
                /// <value>レポートID</value>
                public string ReportId { get; set; }
                /// <summary>Gets or sets 詳細ID</summary>
                /// <value>詳細ID</value>
                public string DetailId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="reportId">レポートID</param>
                /// <param name="detailId">詳細ID</param>
                public PrimaryKey(string reportId, string detailId)
                {
                    ReportId = reportId;
                    DetailId = detailId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ReportId, this.DetailId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="reportId">レポートID</param>
            /// <param name="detailId">詳細ID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RepopaDetailEntity GetEntity(string reportId, string detailId, ComDB db)
            {
                RepopaDetailEntity.PrimaryKey condition = new RepopaDetailEntity.PrimaryKey(reportId, detailId);
                return GetEntity<RepopaDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// REPOPAヘッダ
        /// </summary>
        public class RepopaHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RepopaHeaderEntity()
            {
                TableName = "repopa_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets レポートID</summary>
            /// <value>レポートID</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets レポート名称</summary>
            /// <value>レポート名称</value>
            public string ReportName { get; set; }
            /// <summary>Gets or sets REPOPAレポートID</summary>
            /// <value>REPOPAレポートID</value>
            public long RepopaReportId { get; set; }
            /// <summary>Gets or sets 主要URL</summary>
            /// <value>主要URL</value>
            public string MainUrl { get; set; }
            /// <summary>Gets or sets ソート順</summary>
            /// <value>ソート順</value>
            public int? SortNo { get; set; }
            /// <summary>Gets or sets 帳票区分1|各種名称マスタ</summary>
            /// <value>帳票区分1|各種名称マスタ</value>
            public int? ReportDivision1 { get; set; }
            /// <summary>Gets or sets 帳票区分2|各種名称マスタ</summary>
            /// <value>帳票区分2|各種名称マスタ</value>
            public int? ReportDivision2 { get; set; }
            /// <summary>Gets or sets 帳票区分3|各種名称マスタ</summary>
            /// <value>帳票区分3|各種名称マスタ</value>
            public int? ReportDivision3 { get; set; }
            /// <summary>Gets or sets 帳票区分4|各種名称マスタ</summary>
            /// <value>帳票区分4|各種名称マスタ</value>
            public int? ReportDivision4 { get; set; }
            /// <summary>Gets or sets 帳票区分5|各種名称マスタ</summary>
            /// <value>帳票区分5|各種名称マスタ</value>
            public int? ReportDivision5 { get; set; }
            /// <summary>Gets or sets 検索条件1(未使用)</summary>
            /// <value>検索条件1(未使用)</value>
            public string SearchValue1 { get; set; }
            /// <summary>Gets or sets 検索条件2(未使用)</summary>
            /// <value>検索条件2(未使用)</value>
            public string SearchValue2 { get; set; }
            /// <summary>Gets or sets 検索条件3(未使用)</summary>
            /// <value>検索条件3(未使用)</value>
            public string SearchValue3 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets レポートID</summary>
                /// <value>レポートID</value>
                public string ReportId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="reportId">レポートID</param>
                public PrimaryKey(string reportId)
                {
                    ReportId = reportId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ReportId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="reportId">レポートID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RepopaHeaderEntity GetEntity(string reportId, ComDB db)
            {
                RepopaHeaderEntity.PrimaryKey condition = new RepopaHeaderEntity.PrimaryKey(reportId);
                return GetEntity<RepopaHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ロールマスタ
        /// </summary>
        public class RoleEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RoleEntity()
            {
                TableName = "role";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ロールID</summary>
            /// <value>ロールID</value>
            public int RoleId { get; set; }
            /// <summary>Gets or sets ロール名称</summary>
            /// <value>ロール名称</value>
            public string RoleName { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int? DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ロールID</summary>
                /// <value>ロールID</value>
                public int RoleId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="roleId">ロールID</param>
                public PrimaryKey(int roleId)
                {
                    RoleId = roleId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RoleId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="roleId">ロールID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RoleEntity GetEntity(int roleId, ComDB db)
            {
                RoleEntity.PrimaryKey condition = new RoleEntity.PrimaryKey(roleId);
                return GetEntity<RoleEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 売上トランザクション(廃止)
        /// </summary>
        public class SalesEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SalesEntity()
            {
                TableName = "sales";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public decimal RowNo { get; set; }
            /// <summary>Gets or sets 売上-取消　売上番号</summary>
            /// <value>売上-取消　売上番号</value>
            public string CancelSalesNo { get; set; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets 売上ステータス|各種名称マスタ</summary>
            /// <value>売上ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 伝票発行済区分|各種名称マスタ</summary>
            /// <value>伝票発行済区分|各種名称マスタ</value>
            public int SlipPublishComp { get; set; }
            /// <summary>Gets or sets 伝票発行日</summary>
            /// <value>伝票発行日</value>
            public DateTime? SlipPublishDate { get; set; }
            /// <summary>Gets or sets 売上種別|各種名称マスタ</summary>
            /// <value>売上種別|各種名称マスタ</value>
            public int? SalesKind { get; set; }
            /// <summary>Gets or sets 売上日付</summary>
            /// <value>売上日付</value>
            public DateTime SalesDate { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets データ種別|1</summary>
            /// <value>データ種別|1</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int CategoryDivision { get; set; }
            /// <summary>Gets or sets 入力担当者コード</summary>
            /// <value>入力担当者コード</value>
            public string EntryUserId { get; set; }
            /// <summary>Gets or sets 営業担当者コード</summary>
            /// <value>営業担当者コード</value>
            public string SalesUserId { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 帳合コード</summary>
            /// <value>帳合コード</value>
            public string BalanceCd { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先有効開始日</summary>
            /// <value>取引先有効開始日</value>
            public DateTime? VenderActiveDate { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 担当部署コード</summary>
            /// <value>担当部署コード</value>
            public string ChargeOrganizationCd { get; set; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目有効開始日</summary>
            /// <value>品目有効開始日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 売上数量</summary>
            /// <value>売上数量</value>
            public decimal SalesQuantity { get; set; }
            /// <summary>Gets or sets 標準単価</summary>
            /// <value>標準単価</value>
            public decimal StandardUnitprice { get; set; }
            /// <summary>Gets or sets 標準値引</summary>
            /// <value>標準値引</value>
            public decimal? StandardDiscount { get; set; }
            /// <summary>Gets or sets 売上単価</summary>
            /// <value>売上単価</value>
            public decimal SalesUnitprice { get; set; }
            /// <summary>Gets or sets 仮単価FLG|各種名称マスタ</summary>
            /// <value>仮単価FLG|各種名称マスタ</value>
            public int TmpUnitpriceFlg { get; set; }
            /// <summary>Gets or sets 消費税課税区分|各種名称マスタ</summary>
            /// <value>消費税課税区分|各種名称マスタ</value>
            public int TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal TaxRatio { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal TaxAmount { get; set; }
            /// <summary>Gets or sets 売上金額</summary>
            /// <value>売上金額</value>
            public decimal SalesAmount { get; set; }
            /// <summary>Gets or sets 会計部門借方コード</summary>
            /// <value>会計部門借方コード</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 会計部門貸方コード</summary>
            /// <value>会計部門貸方コード</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 承認依頼先担当者ID</summary>
            /// <value>承認依頼先担当者ID</value>
            public string ReceiveUserId { get; set; }
            /// <summary>Gets or sets 承認依頼者ID</summary>
            /// <value>承認依頼者ID</value>
            public string ApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 承認依頼日時</summary>
            /// <value>承認依頼日時</value>
            public DateTime? ApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 承認者ID</summary>
            /// <value>承認者ID</value>
            public string ApprovalUserId { get; set; }
            /// <summary>Gets or sets 承認日(未使用)</summary>
            /// <value>承認日(未使用)</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets 承認ステータス|各種名称マスタ</summary>
            /// <value>承認ステータス|各種名称マスタ</value>
            public int? ApprovalStatus { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 請求対象|0</summary>
            /// <value>請求対象|0</value>
            public int ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 請求更新フラグ|0</summary>
            /// <value>請求更新フラグ|0</value>
            public int ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 売掛対象|0</summary>
            /// <value>売掛対象|0</value>
            public int DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ|0</summary>
            /// <value>売掛更新フラグ|0</value>
            public int DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 出荷番号</summary>
            /// <value>出荷番号</value>
            public string ShippingNo { get; set; }
            /// <summary>Gets or sets 入庫ロケーション</summary>
            /// <value>入庫ロケーション</value>
            public string HousingLocationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets 入力区分|0</summary>
            /// <value>入力区分|0</value>
            public int InputDivision { get; set; }
            /// <summary>Gets or sets 売上基準|0</summary>
            /// <value>売上基準|0</value>
            public int SalesBasic { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime ExValidDate { get; set; }
            /// <summary>Gets or sets メール送信ステータス(承認依頼)|0</summary>
            /// <value>メール送信ステータス(承認依頼)|0</value>
            public int? SendMailApprStatus { get; set; }
            /// <summary>Gets or sets メール送信ステータス(否認)|0</summary>
            /// <value>メール送信ステータス(否認)|0</value>
            public int? SendMailDisalwStatus { get; set; }
            /// <summary>Gets or sets 仕訳作成ステータス|0</summary>
            /// <value>仕訳作成ステータス|0</value>
            public int? ShiwakeMakeStatus { get; set; }
            /// <summary>Gets or sets 仕訳送信ステータス|0</summary>
            /// <value>仕訳送信ステータス|0</value>
            public int? ShiwakeSendStatus { get; set; }
            /// <summary>Gets or sets 借方補助科目コード(未使用)</summary>
            /// <value>借方補助科目コード(未使用)</value>
            public string DebitSubTitleCd { get; set; }
            /// <summary>Gets or sets 貸方補助科目コード(未使用)</summary>
            /// <value>貸方補助科目コード(未使用)</value>
            public string CreditSubTitleCd { get; set; }
            /// <summary>Gets or sets 完納区分(未使用)|0</summary>
            /// <value>完納区分(未使用)|0</value>
            public int DeliveryComp { get; set; }
            /// <summary>Gets or sets 承認者(未使用)</summary>
            /// <value>承認者(未使用)</value>
            public string ApprovedBy { get; set; }
            /// <summary>Gets or sets 包装指図番号(未使用)</summary>
            /// <value>包装指図番号(未使用)</value>
            public string PackageDirectionNo { get; set; }
            /// <summary>Gets or sets 更新フラグ(未使用)</summary>
            /// <value>更新フラグ(未使用)</value>
            public int UpdateFlg { get; set; }
            /// <summary>Gets or sets 売上伝票番号(未使用)</summary>
            /// <value>売上伝票番号(未使用)</value>
            public string SalesSlipNo { get; set; }
            /// <summary>Gets or sets 売上伝票行番号(未使用)</summary>
            /// <value>売上伝票行番号(未使用)</value>
            public int? SalesSlipRowNo { get; set; }
            /// <summary>Gets or sets 売上数換算量(未使用)</summary>
            /// <value>売上数換算量(未使用)</value>
            public decimal SalesConvertQuantity { get; set; }
            /// <summary>Gets or sets 合格数換算量(未使用)</summary>
            /// <value>合格数換算量(未使用)</value>
            public decimal AcceptConvertQuantity { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 売上番号</summary>
                /// <value>売上番号</value>
                public string SalesNo { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public decimal RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="salesNo">売上番号</param>
                /// <param name="rowNo">行番号</param>
                public PrimaryKey(string salesNo, decimal rowNo)
                {
                    SalesNo = salesNo;
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SalesNo, this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="salesNo">売上番号</param>
            /// <param name="rowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public SalesEntity GetEntity(string salesNo, decimal rowNo, ComDB db)
            {
                SalesEntity.PrimaryKey condition = new SalesEntity.PrimaryKey(salesNo, rowNo);
                return GetEntity<SalesEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 売上トランザクション_明細(廃止)
        /// </summary>
        public class SalesInoutRecordEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SalesInoutRecordEntity()
            {
                TableName = "sales_inout_record";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 売上行番号</summary>
            /// <value>売上行番号</value>
            public int SalesRowNo { get; set; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets 数量</summary>
            /// <value>数量</value>
            public decimal? Qty { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 売上番号</summary>
                /// <value>売上番号</value>
                public string SalesNo { get; set; }
                /// <summary>Gets or sets 売上行番号</summary>
                /// <value>売上行番号</value>
                public int SalesRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="salesNo">売上番号</param>
                /// <param name="salesRowNo">売上行番号</param>
                public PrimaryKey(string salesNo, int salesRowNo)
                {
                    SalesNo = salesNo;
                    SalesRowNo = salesRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SalesNo, this.SalesRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="salesNo">売上番号</param>
            /// <param name="salesRowNo">売上行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public SalesInoutRecordEntity GetEntity(string salesNo, int salesRowNo, ComDB db)
            {
                SalesInoutRecordEntity.PrimaryKey condition = new SalesInoutRecordEntity.PrimaryKey(salesNo, salesRowNo);
                return GetEntity<SalesInoutRecordEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 売上トランザクション_ヘッダ
        /// </summary>
        public class SalesHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SalesHeaderEntity()
            {
                TableName = "sales_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 売上元番号</summary>
            /// <value>売上元番号</value>
            public string OriginalSalesNo { get; set; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets 出荷番号</summary>
            /// <value>出荷番号</value>
            public string ShippingNo { get; set; }
            /// <summary>Gets or sets 売上計上基準</summary>
            /// <value>売上計上基準</value>
            public int? RecordSalesBasis { get; set; }
            /// <summary>Gets or sets 売上ステータス</summary>
            /// <value>売上ステータス</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 伝票発行済区分</summary>
            /// <value>伝票発行済区分</value>
            public int? SlipPublishComp { get; set; }
            /// <summary>Gets or sets 伝票発行日</summary>
            /// <value>伝票発行日</value>
            public DateTime? SlipPublishDate { get; set; }
            /// <summary>Gets or sets 売上種別</summary>
            /// <value>売上種別</value>
            public int? SalesKind { get; set; }
            /// <summary>Gets or sets 売上日付</summary>
            /// <value>売上日付</value>
            public DateTime SalesDate { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets データ種別</summary>
            /// <value>データ種別</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets データ集計区分</summary>
            /// <value>データ集計区分</value>
            public int DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード</summary>
            /// <value>分類コード</value>
            public int CategoryDivision { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string SalesUserId { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 帳合コード</summary>
            /// <value>帳合コード</value>
            public string BalanceCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime? VenderActiveDate { get; set; }
            /// <summary>Gets or sets 請求先区分</summary>
            /// <value>請求先区分</value>
            public string InvoiceDivision { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 請求先開始有効日</summary>
            /// <value>請求先開始有効日</value>
            public DateTime? InvoiceActiveDate { get; set; }
            /// <summary>Gets or sets 担当部署コード</summary>
            /// <value>担当部署コード</value>
            public string ChargeOrganizationCd { get; set; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 消費税課税区分</summary>
            /// <value>消費税課税区分</value>
            public int TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal TaxRatio { get; set; }
            /// <summary>Gets or sets 消費税区分コード</summary>
            /// <value>消費税区分コード</value>
            public string TaxCd { get; set; }
            /// <summary>Gets or sets 会計部門借方コード</summary>
            /// <value>会計部門借方コード</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 会計部門貸方コード</summary>
            /// <value>会計部門貸方コード</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceDate { get; set; }
            /// <summary>Gets or sets 請求対象|0</summary>
            /// <value>請求対象|0</value>
            public int ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 請求更新フラグ|0</summary>
            /// <value>請求更新フラグ|0</value>
            public int ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 売掛対象|0</summary>
            /// <value>売掛対象|0</value>
            public int DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ|0</summary>
            /// <value>売掛更新フラグ|0</value>
            public int DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal CurrencyRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime RateValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 売上番号</summary>
                /// <value>売上番号</value>
                public string SalesNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="salesNo">売上番号</param>
                public PrimaryKey(string salesNo)
                {
                    SalesNo = salesNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SalesNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="salesNo">売上番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public SalesHeaderEntity GetEntity(string salesNo, ComDB db)
            {
                SalesHeaderEntity.PrimaryKey condition = new SalesHeaderEntity.PrimaryKey(salesNo);
                return GetEntity<SalesHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 売上トランザクション_明細
        /// </summary>
        public class SalesDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SalesDetailEntity()
            {
                TableName = "sales_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int SalesRowNo { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目開始有効日</summary>
            /// <value>品目開始有効日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 品目仕様開始有効日</summary>
            /// <value>品目仕様開始有効日</value>
            public DateTime? SpecificationActiveDate { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 売上数量</summary>
            /// <value>売上数量</value>
            public decimal SalesQuantity { get; set; }
            /// <summary>Gets or sets 売上単価</summary>
            /// <value>売上単価</value>
            public decimal SalesUnitprice { get; set; }
            /// <summary>Gets or sets 売上金額</summary>
            /// <value>売上金額</value>
            public decimal SalesAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal TaxAmount { get; set; }
            /// <summary>Gets or sets 仮単価FLG|各種名称マスタ</summary>
            /// <value>仮単価FLG|各種名称マスタ</value>
            public int TmpUnitpriceFlg { get; set; }
            /// <summary>Gets or sets 入庫ロケーション</summary>
            /// <value>入庫ロケーション</value>
            public string HousingLocationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>サブロット番号1</value>
            public string SublotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>サブロット番号2</value>
            public string SublotNo2 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 売上番号</summary>
                /// <value>売上番号</value>
                public string SalesNo { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int SalesRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="salesNo">売上番号</param>
                /// <param name="salesRowNo">行番号</param>
                public PrimaryKey(string salesNo, int salesRowNo)
                {
                    SalesNo = salesNo;
                    SalesRowNo = salesRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SalesNo, this.SalesRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="salesNo">売上番号</param>
            /// <param name="salesRowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public SalesDetailEntity GetEntity(string salesNo, int salesRowNo, ComDB db)
            {
                SalesDetailEntity.PrimaryKey condition = new SalesDetailEntity.PrimaryKey(salesNo, salesRowNo);
                return GetEntity<SalesDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// メール送信エラーログ
        /// </summary>
        public class SendMailErrorLogEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SendMailErrorLogEntity()
            {
                TableName = "send_mail_error_log";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets メールフォーマットID</summary>
            /// <value>メールフォーマットID</value>
            public long? MailFormatId { get; set; }
            /// <summary>Gets or sets 宛先担当者コード</summary>
            /// <value>宛先担当者コード</value>
            public string ToUserId { get; set; }
            /// <summary>Gets or sets 宛先メールアドレス</summary>
            /// <value>宛先メールアドレス</value>
            public string ToMailAddress { get; set; }
            /// <summary>Gets or sets ログ文字列</summary>
            /// <value>ログ文字列</value>
            public string LogString { get; set; }
            /// <summary>Gets or sets エラー日時</summary>
            /// <value>エラー日時</value>
            public DateTime? ErrorDate { get; set; }
            /// <summary>Gets or sets ログイン担当者コード</summary>
            /// <value>ログイン担当者コード</value>
            public string LoginUserId { get; set; }
            /// <summary>Gets or sets メッセージID</summary>
            /// <value>メッセージID</value>
            public string MessageId { get; set; }
        }

        /// <summary>
        /// 出荷ヘッダ
        /// </summary>
        public class ShippingEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ShippingEntity()
            {
                TableName = "shipping";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 出荷番号</summary>
            /// <value>出荷番号</value>
            public string ShippingNo { get; set; }
            /// <summary>Gets or sets 出荷指図区分</summary>
            /// <value>出荷指図区分</value>
            public string ShippingDivision { get; set; }
            /// <summary>Gets or sets 出荷予定日</summary>
            /// <value>出荷予定日</value>
            public DateTime ScheduledShippingDate { get; set; }
            /// <summary>Gets or sets 出荷完了日</summary>
            /// <value>出荷完了日</value>
            public DateTime? ShippingResultDate { get; set; }
            /// <summary>Gets or sets ステータス|各種名称マスタ</summary>
            /// <value>ステータス|各種名称マスタ</value>
            public int? ShippingStatus { get; set; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
            /// <value>取引先区分|各種名称マスタ</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先有効開始日</summary>
            /// <value>取引先有効開始日</value>
            public DateTime? VenderActiveDate { get; set; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目有効開始日</summary>
            /// <value>品目有効開始日</value>
            public DateTime ItemActiveDate { get; set; }
            /// <summary>Gets or sets 品目名称(スポット品の場合に使用）</summary>
            /// <value>品目名称(スポット品の場合に使用）</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 仕様名称(スポット品の場合に使用）</summary>
            /// <value>仕様名称(スポット品の場合に使用）</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 親出荷番号(未使用)</summary>
            /// <value>親出荷番号(未使用)</value>
            public string ShippingParentNo { get; set; }
            /// <summary>Gets or sets 完納区分|0</summary>
            /// <value>完納区分|0</value>
            public int DeliveryComp { get; set; }
            /// <summary>Gets or sets 出荷指図日</summary>
            /// <value>出荷指図日</value>
            public DateTime ShippingInstructDate { get; set; }
            /// <summary>Gets or sets 在庫預り</summary>
            /// <value>在庫預り</value>
            public int? KeepInventry { get; set; }
            /// <summary>Gets or sets 出庫先ロケーションコード</summary>
            /// <value>出庫先ロケーションコード</value>
            public string ShipmentLocationCd { get; set; }
            /// <summary>Gets or sets 担当者コード(未使用)</summary>
            /// <value>担当者コード(未使用)</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 出荷指示書備考</summary>
            /// <value>出荷指示書備考</value>
            public string ShippingSlipRemark { get; set; }
            /// <summary>Gets or sets 出荷時社内備考</summary>
            /// <value>出荷時社内備考</value>
            public string ShippingRemark { get; set; }
            /// <summary>Gets or sets 倉庫別指図書発行区分|各種名称マスタ</summary>
            /// <value>倉庫別指図書発行区分|各種名称マスタ</value>
            public int? LocationOrderOutputDivision { get; set; }
            /// <summary>Gets or sets 倉庫別指図書発行日時</summary>
            /// <value>倉庫別指図書発行日時</value>
            public DateTime? LocationOrderOutputDate { get; set; }
            /// <summary>Gets or sets 倉庫別指図書発行者ID</summary>
            /// <value>倉庫別指図書発行者ID</value>
            public string LocationOrderOutputUserId { get; set; }
            /// <summary>Gets or sets 運送会社別指図書発行区分|各種名称マスタ</summary>
            /// <value>運送会社別指図書発行区分|各種名称マスタ</value>
            public int? CarryOrderOutputDivision { get; set; }
            /// <summary>Gets or sets 運送会社別指図書発行日時</summary>
            /// <value>運送会社別指図書発行日時</value>
            public DateTime? CarryOrderOutputDate { get; set; }
            /// <summary>Gets or sets 運送会社別指図書発行者ID</summary>
            /// <value>運送会社別指図書発行者ID</value>
            public string CarryOrderOutputUserId { get; set; }
            /// <summary>Gets or sets 出荷品目一覧表発行済区分|各種名称マスタ</summary>
            /// <value>出荷品目一覧表発行済区分|各種名称マスタ</value>
            public int? ItemlistOutputDivision { get; set; }
            /// <summary>Gets or sets 出荷品目一覧表発行日時</summary>
            /// <value>出荷品目一覧表発行日時</value>
            public DateTime? ItemlistOutputDate { get; set; }
            /// <summary>Gets or sets 出荷品目一覧表発行者ID</summary>
            /// <value>出荷品目一覧表発行者ID</value>
            public string ItemlistOutputUserId { get; set; }
            /// <summary>Gets or sets ERP出荷管理番号</summary>
            /// <value>ERP出荷管理番号</value>
            public string ErpShippingNo { get; set; }
            /// <summary>Gets or sets ERP受信日時</summary>
            /// <value>ERP受信日時</value>
            public DateTime? ErpReceiveDate { get; set; }
            /// <summary>Gets or sets ERP連携日時</summary>
            /// <value>ERP連携日時</value>
            public DateTime? ErpLinkDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 出荷番号</summary>
                /// <value>出荷番号</value>
                public string ShippingNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="shippingNo">出荷番号</param>
                public PrimaryKey(string shippingNo)
                {
                    ShippingNo = shippingNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ShippingNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="shippingNo">出荷番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ShippingEntity GetEntity(string shippingNo, ComDB db)
            {
                ShippingEntity.PrimaryKey condition = new ShippingEntity.PrimaryKey(shippingNo);
                return GetEntity<ShippingEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 出荷詳細
        /// </summary>
        public class ShippingDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ShippingDetailEntity()
            {
                TableName = "shipping_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 出荷番号</summary>
            /// <value>出荷番号</value>
            public string ShippingNo { get; set; }
            /// <summary>Gets or sets 出荷行番号</summary>
            /// <value>出荷行番号</value>
            public int ShippingRowNo { get; set; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>サブロット番号1</value>
            public string SublotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>サブロット番号2</value>
            public string SublotNo2 { get; set; }
            /// <summary>Gets or sets 出荷指図数量</summary>
            /// <value>出荷指図数量</value>
            public decimal ShippingInstruction { get; set; }
            /// <summary>Gets or sets 出荷実績数</summary>
            /// <value>出荷実績数</value>
            public decimal ShippingResultQuantity { get; set; }
            /// <summary>Gets or sets 出荷実績日</summary>
            /// <value>出荷実績日</value>
            public DateTime? ShippingResultDate { get; set; }
            /// <summary>Gets or sets 出荷引当フラグ|0</summary>
            /// <value>出荷引当フラグ|0</value>
            public int? SelectLocLotFlg { get; set; }
            /// <summary>Gets or sets ロット指図有無</summary>
            /// <value>ロット指図有無</value>
            public int? LotDirectionFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 出荷番号</summary>
                /// <value>出荷番号</value>
                public string ShippingNo { get; set; }
                /// <summary>Gets or sets 出荷行番号</summary>
                /// <value>出荷行番号</value>
                public int ShippingRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="shippingNo">出荷番号</param>
                /// <param name="shippingRowNo">出荷行番号</param>
                public PrimaryKey(string shippingNo, int shippingRowNo)
                {
                    ShippingNo = shippingNo;
                    ShippingRowNo = shippingRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ShippingNo, this.ShippingRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="shippingNo">出荷番号</param>
            /// <param name="shippingRowNo">出荷行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ShippingDetailEntity GetEntity(string shippingNo, int shippingRowNo, ComDB db)
            {
                ShippingDetailEntity.PrimaryKey condition = new ShippingDetailEntity.PrimaryKey(shippingNo, shippingRowNo);
                return GetEntity<ShippingDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ショートカットマスタ
        /// </summary>
        public class ShortCutEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ShortCutEntity()
            {
                TableName = "short_cut";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets SEQ番号</summary>
            /// <value>SEQ番号</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 機能ID</summary>
            /// <value>機能ID</value>
            public string Conductid { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets SEQ番号</summary>
                /// <value>SEQ番号</value>
                public int SeqNo { get; set; }
                /// <summary>Gets or sets 担当者コード</summary>
                /// <value>担当者コード</value>
                public string UserId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="seqNo">SEQ番号</param>
                /// <param name="userId">担当者コード</param>
                public PrimaryKey(int seqNo, string userId)
                {
                    SeqNo = seqNo;
                    UserId = userId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SeqNo, this.UserId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="seqNo">SEQ番号</param>
            /// <param name="userId">担当者コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ShortCutEntity GetEntity(int seqNo, string userId, ComDB db)
            {
                ShortCutEntity.PrimaryKey condition = new ShortCutEntity.PrimaryKey(seqNo, userId);
                return GetEntity<ShortCutEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 在庫更新ログ
        /// </summary>
        public class StockUpdateLogEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public StockUpdateLogEntity()
            {
                TableName = "stock_update_log";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public long StockSeq { get; set; }
            /// <summary>Gets or sets 在庫更新プロシージャ名</summary>
            /// <value>在庫更新プロシージャ名</value>
            public string ProgramName { get; set; }
            /// <summary>Gets or sets 処理日時</summary>
            /// <value>処理日時</value>
            public DateTime? ProcDate { get; set; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 指図行番号</summary>
            /// <value>指図行番号</value>
            public int? OrderLineNo { get; set; }
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
            /// <summary>Gets or sets 数量</summary>
            /// <value>数量</value>
            public decimal? Qty { get; set; }
            /// <summary>Gets or sets メッセージ</summary>
            /// <value>メッセージ</value>
            public string Message { get; set; }
            /// <summary>Gets or sets ログインID</summary>
            /// <value>ログインID</value>
            public string UserId { get; set; }
        }

        /// <summary>
        /// sysdiagrams
        /// </summary>
        public class SysdiagramsEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SysdiagramsEntity()
            {
                TableName = "sysdiagrams";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets name</summary>
            /// <value>name</value>
            public string Name { get; set; }
            /// <summary>Gets or sets principal_id</summary>
            /// <value>principal_id</value>
            public int PrincipalId { get; set; }
            /// <summary>Gets or sets diagram_id</summary>
            /// <value>diagram_id</value>
            public int DiagramId { get; set; }
            /// <summary>Gets or sets version</summary>
            /// <value>version</value>
            public int? Version { get; set; }
            /// <summary>Gets or sets definition</summary>
            /// <value>definition</value>
            //public ? Definition { get; set; }
        }

        /// <summary>
        /// ユーザー･ロール組合せマスタ
        /// </summary>
        public class UserRoleEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public UserRoleEntity()
            {
                TableName = "user_role";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets ロールID</summary>
            /// <value>ロールID</value>
            public int RoleId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ユーザーID</summary>
                /// <value>ユーザーID</value>
                public string UserId { get; set; }
                /// <summary>Gets or sets ロールID</summary>
                /// <value>ロールID</value>
                public int RoleId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="userId">ユーザーID</param>
                /// <param name="roleId">ロールID</param>
                public PrimaryKey(string userId, int roleId)
                {
                    UserId = userId;
                    RoleId = roleId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.UserId, this.RoleId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="userId">ユーザーID</param>
            /// <param name="roleId">ロールID</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public UserRoleEntity GetEntity(string userId, int roleId, ComDB db)
            {
                UserRoleEntity.PrimaryKey condition = new UserRoleEntity.PrimaryKey(userId, roleId);
                return GetEntity<UserRoleEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 消費税区分マスタ
        /// </summary>
        public class TaxMasterEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TaxMasterEntity()
            {
                TableName = "tax_master";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 税区分コード</summary>
            /// <value>税区分コード</value>
            public string TaxCd { get; set; }
            /// <summary>Gets or sets 用途</summary>
            /// <value>用途</value>
            public string Category { get; set; }
            /// <summary>Gets or sets 税区分名称</summary>
            /// <value>税区分名称</value>
            public string TaxName { get; set; }
            /// <summary>Gets or sets 適用開始日</summary>
            /// <value>適用開始日</value>
            public DateTime ValidDate { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public int TaxRatio { get; set; }
            /// <summary>Gets or sets 軽減税率|1</summary>
            /// <value>軽減税率|1</value>
            public int TaxCategory { get; set; }
            /// <summary>Gets or sets 課税区分1|1</summary>
            /// <value>課税区分1|1</value>
            public int TaxDivision1 { get; set; }
            /// <summary>Gets or sets 課税区分1|1</summary>
            /// <value>課税区分1|1</value>
            public int? TaxDivision2 { get; set; }
            /// <summary>Gets or sets コンボボックス表示順</summary>
            /// <value>コンボボックス表示順</value>
            public int Sort { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 税区分コード</summary>
                /// <value>税区分コード</value>
                public string TaxCd { get; set; }
                /// <summary>Gets or sets 用途</summary>
                /// <value>用途</value>
                public string Category { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="taxCd">税区分コード</param>
                /// <param name="category">用途</param>
                public PrimaryKey(string taxCd, string category)
                {
                    TaxCd = taxCd;
                    Category = category;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.TaxCd, this.Category);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="taxCd">税区分コード</param>
            /// <param name="category">用途</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TaxMasterEntity GetEntity(string taxCd, string category, ComDB db)
            {
                TaxMasterEntity.PrimaryKey condition = new TaxMasterEntity.PrimaryKey(taxCd, category);
                return GetEntity<TaxMasterEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// オペレーションパターンマスタヘッダー
        /// </summary>
        public class OperationPatternHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public OperationPatternHeaderEntity()
            {
                TableName = "operation_pattern_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工程パターンコード</summary>
            /// <value>工程パターンコード</value>
            public string OperationPatternCd { get; set; }
            /// <summary>Gets or sets 工程パターン名称</summary>
            /// <value>工程パターン名称</value>
            public string OperationPatternName { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 税区分コード</summary>
                /// <value>税区分コード</value>
                public string OperationPatternCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="operationPatternCd">工程パターンコード</param>
                public PrimaryKey(string operationPatternCd)
                {
                    OperationPatternCd = operationPatternCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OperationPatternCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="operationPatternCd">工程パターンコード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public OperationPatternHeaderEntity GetEntity(string operationPatternCd, ComDB db)
            {
                OperationPatternHeaderEntity.PrimaryKey condition = new OperationPatternHeaderEntity.PrimaryKey(operationPatternCd);
                return GetEntity<OperationPatternHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// オペレーションパターンマスタディティール
        /// </summary>
        public class OperationPatternDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public OperationPatternDetailEntity()
            {
                TableName = "operation_pattern_detail";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public string OperationPatternCd { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? Seq { get; set; }
            /// <summary>Gets or sets 工程コード</summary>
            /// <value>工程コード</value>
            public string OperationCd { get; set; }
            /// <summary>Gets or sets 作業時間</summary>
            /// <value>作業時間</value>
            public decimal? WorkingMinutes { get; set; }
            /// <summary>Gets or sets 工程条件自由入力</summary>
            /// <value>工程条件自由入力</value>
            public string Condition { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets 正味質量(未使用)</summary>
            /// <value>正味質量(未使用)</value>
            public long? Net { get; set; }
            /// <summary>Gets or sets 前工程NO(未使用)</summary>
            /// <value>前工程NO(未使用)</value>
            public int? FromNo { get; set; }
            /// <summary>Gets or sets 後工程NO(未使用)</summary>
            /// <value>後工程NO(未使用)</value>
            public int? ToNo { get; set; }
            /// <summary>Gets or sets 単位(未使用)</summary>
            /// <value>単位(未使用)</value>
            public string FillingUnit { get; set; }
            /// <summary>Gets or sets 設備コード(未使用)</summary>
            /// <value>設備コード(未使用)</value>
            public string ResouceCd { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets レシピインデックス</summary>
                /// <value>レシピインデックス</value>
                public string OperationPatternCd { get; set; }
                /// <summary>Gets or sets 工程番号</summary>
                /// <value>工程番号</value>
                public int StepNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="operationPatternCd">工程パターンコード</param>
                /// <param name="stepNo">工程番号</param>
                public PrimaryKey(string operationPatternCd, int stepNo)
                {
                    OperationPatternCd = operationPatternCd;
                    StepNo = stepNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OperationPatternCd, this.StepNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="operationPatternCd">工程パターンコード</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public OperationPatternDetailEntity GetEntity(string operationPatternCd, int stepNo, ComDB db)
            {
                OperationPatternDetailEntity.PrimaryKey condition = new OperationPatternDetailEntity.PrimaryKey(operationPatternCd, stepNo);
                return GetEntity<OperationPatternDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め入金トランザクション
        /// </summary>
        public class TemporaryClaimCreditEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryClaimCreditEntity()
            {
                TableName = "temporary_claim_credit";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 入金番号</summary>
            /// <value>入金番号</value>
            public string CreditNo { get; set; }
            /// <summary>Gets or sets 入金行番号</summary>
            /// <value>入金行番号</value>
            public int CreditRowNo { get; set; }
            /// <summary>Gets or sets 入金元番号</summary>
            /// <value>入金元番号</value>
            public string OriginalCreditNo { get; set; }
            /// <summary>Gets or sets 入金ステータス|各種名称マスタ</summary>
            /// <value>入金ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 入金日付</summary>
            /// <value>入金日付</value>
            public DateTime CreditDate { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets データ種別|(分類マスタ参照)</summary>
            /// <value>データ種別|(分類マスタ参照)</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 入金金額</summary>
            /// <value>入金金額</value>
            public decimal? CreditAmount { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 口座区分</summary>
            /// <value>口座区分</value>
            public int? AccountDivision { get; set; }
            /// <summary>Gets or sets 口座番号</summary>
            /// <value>口座番号</value>
            public string AccountNo { get; set; }
            /// <summary>Gets or sets 手形種別</summary>
            /// <value>手形種別</value>
            public int? NoteDivision { get; set; }
            /// <summary>Gets or sets 手形番号</summary>
            /// <value>手形番号</value>
            public string NoteNo { get; set; }
            /// <summary>Gets or sets 振出日</summary>
            /// <value>振出日</value>
            public DateTime? DrawalDate { get; set; }
            /// <summary>Gets or sets 満期日</summary>
            /// <value>満期日</value>
            public DateTime? NoteDate { get; set; }
            /// <summary>Gets or sets 摘要名</summary>
            /// <value>摘要名</value>
            public string Summary { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ</summary>
            /// <value>消込完了フラグ</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 売掛対象</summary>
            /// <value>売掛対象</value>
            public int? DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 請求対象</summary>
            /// <value>請求対象</value>
            public int? ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ</summary>
            /// <value>売掛更新フラグ</value>
            public int? DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 請求更新フラグ</summary>
            /// <value>請求更新フラグ</value>
            public int? ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 入金番号</summary>
                /// <value>入金番号</value>
                public string CreditNo { get; set; }
                /// <summary>Gets or sets 入金行番号</summary>
                /// <value>入金行番号</value>
                public int CreditRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="creditNo">入金番号</param>
                /// <param name="creditRowNo">入金行番号</param>
                public PrimaryKey(string creditNo, int creditRowNo)
                {
                    CreditNo = creditNo;
                    CreditRowNo = creditRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.CreditNo, this.CreditRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="creditNo">入金番号</param>
            /// <param name="creditRowNo">入金行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryClaimCreditEntity GetEntity(string creditNo, int creditRowNo, ComDB db)
            {
                TemporaryClaimCreditEntity.PrimaryKey condition = new TemporaryClaimCreditEntity.PrimaryKey(creditNo, creditRowNo);
                return GetEntity<TemporaryClaimCreditEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め請求ヘッダー
        /// </summary>
        public class TemporaryClaimHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryClaimHeaderEntity()
            {
                TableName = "temporary_claim_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime ClaimDate { get; set; }
            /// <summary>Gets or sets 入金予定日</summary>
            /// <value>入金予定日</value>
            public DateTime CreditScheduledDate { get; set; }
            /// <summary>Gets or sets 決済方法</summary>
            /// <value>決済方法</value>
            public string PaymentMethod { get; set; }
            /// <summary>Gets or sets 手形サイト</summary>
            /// <value>手形サイト</value>
            public int? NoteSight { get; set; }
            /// <summary>Gets or sets 休日指定フラグ</summary>
            /// <value>休日指定フラグ</value>
            public int? HolidayFlg { get; set; }
            /// <summary>Gets or sets 前月請求残高</summary>
            /// <value>前月請求残高</value>
            public decimal? ClaimAmountForward { get; set; }
            /// <summary>Gets or sets 入金額</summary>
            /// <value>入金額</value>
            public decimal? CreditAmountForward { get; set; }
            /// <summary>Gets or sets その他入金額</summary>
            /// <value>その他入金額</value>
            public decimal? OtherCreditAmountForward { get; set; }
            /// <summary>Gets or sets 売上金額</summary>
            /// <value>売上金額</value>
            public decimal? SalesAmount { get; set; }
            /// <summary>Gets or sets 返品金額</summary>
            /// <value>返品金額</value>
            public decimal? SalesReturnedAmount { get; set; }
            /// <summary>Gets or sets 値引金額</summary>
            /// <value>値引金額</value>
            public decimal? SalesDiscountAmount { get; set; }
            /// <summary>Gets or sets その他売上金額</summary>
            /// <value>その他売上金額</value>
            public decimal? OtherSalesAmount { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 当月請求残高</summary>
            /// <value>当月請求残高</value>
            public decimal? ClaimAmount { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal? EraserBalanceAmount { get; set; }
            /// <summary>Gets or sets 請求書発行済フラグ</summary>
            /// <value>請求書発行済フラグ</value>
            public int BillDivision { get; set; }
            /// <summary>Gets or sets 発行日時</summary>
            /// <value>発行日時</value>
            public DateTime? IssueDate { get; set; }
            /// <summary>Gets or sets 発行者ID</summary>
            /// <value>発行者ID</value>
            public string IssuerCd { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 請求番号</summary>
                /// <value>請求番号</value>
                public string ClaimNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="claimNo">請求番号</param>
                public PrimaryKey(string claimNo)
                {
                    ClaimNo = claimNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ClaimNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="claimNo">請求番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryClaimHeaderEntity GetEntity(string claimNo, ComDB db)
            {
                TemporaryClaimHeaderEntity.PrimaryKey condition = new TemporaryClaimHeaderEntity.PrimaryKey(claimNo);
                return GetEntity<TemporaryClaimHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め支払トランザクション
        /// </summary>
        public class TemporaryClaimPaymentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryClaimPaymentEntity()
            {
                TableName = "temporary_claim_payment";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 支払伝票番号</summary>
            /// <value>支払伝票番号</value>
            public string PaymentSlipNo { get; set; }
            /// <summary>Gets or sets 支払伝票行番号</summary>
            /// <value>支払伝票行番号</value>
            public int PaymentSlipRowNo { get; set; }
            /// <summary>Gets or sets 支払ステータス|各種名称マスタ</summary>
            /// <value>支払ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 支払日付</summary>
            /// <value>支払日付</value>
            public DateTime PaymentDate { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets データ種別|(分類マスタ参照)</summary>
            /// <value>データ種別|(分類マスタ参照)</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int CategoryDivision { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 支払金額</summary>
            /// <value>支払金額</value>
            public decimal PaymentAmount { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 口座区分|各種名称マスタ</summary>
            /// <value>口座区分|各種名称マスタ</value>
            public int? AccountDivision { get; set; }
            /// <summary>Gets or sets 口座番号</summary>
            /// <value>口座番号</value>
            public string AccountNo { get; set; }
            /// <summary>Gets or sets 手形種別|各種名称マスタ</summary>
            /// <value>手形種別|各種名称マスタ</value>
            public int? NoteDivision { get; set; }
            /// <summary>Gets or sets 手形番号</summary>
            /// <value>手形番号</value>
            public string NoteNo { get; set; }
            /// <summary>Gets or sets 振出日</summary>
            /// <value>振出日</value>
            public DateTime? DrawalDate { get; set; }
            /// <summary>Gets or sets 満期日</summary>
            /// <value>満期日</value>
            public DateTime? NoteDate { get; set; }
            /// <summary>Gets or sets 摘要名</summary>
            /// <value>摘要名</value>
            public string Summary { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 買掛対象|0</summary>
            /// <value>買掛対象|0</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象|0</summary>
            /// <value>支払対象|0</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ|0</summary>
            /// <value>買掛更新フラグ|0</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 支払更新フラグ|0</summary>
            /// <value>支払更新フラグ|0</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 支払伝票番号</summary>
                /// <value>支払伝票番号</value>
                public string PaymentSlipNo { get; set; }
                /// <summary>Gets or sets 支払伝票行番号</summary>
                /// <value>支払伝票行番号</value>
                public int PaymentSlipRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="paymentSlipNo">支払伝票番号</param>
                /// <param name="paymentSlipRowNo">支払伝票行番号</param>
                public PrimaryKey(string paymentSlipNo, int paymentSlipRowNo)
                {
                    PaymentSlipNo = paymentSlipNo;
                    PaymentSlipRowNo = paymentSlipRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PaymentSlipNo, this.PaymentSlipRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="paymentSlipNo">支払伝票番号</param>
            /// <param name="paymentSlipRowNo">支払伝票行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryClaimPaymentEntity GetEntity(string paymentSlipNo, int paymentSlipRowNo, ComDB db)
            {
                TemporaryClaimPaymentEntity.PrimaryKey condition = new TemporaryClaimPaymentEntity.PrimaryKey(paymentSlipNo, paymentSlipRowNo);
                return GetEntity<TemporaryClaimPaymentEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め売上トランザクション_ヘッダ
        /// </summary>
        public class TemporaryClaimSalesHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryClaimSalesHeaderEntity()
            {
                TableName = "temporary_claim_sales_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 売上元番号</summary>
            /// <value>売上元番号</value>
            public string OriginalSalesNo { get; set; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets 出荷番号</summary>
            /// <value>出荷番号</value>
            public string ShippingNo { get; set; }
            /// <summary>Gets or sets 売上計上基準</summary>
            /// <value>売上計上基準</value>
            public int? RecordSalesBasis { get; set; }
            /// <summary>Gets or sets 売上ステータス|各種名称マスタ</summary>
            /// <value>売上ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 伝票発行済区分|各種名称マスタ</summary>
            /// <value>伝票発行済区分|各種名称マスタ</value>
            public int? SlipPublishComp { get; set; }
            /// <summary>Gets or sets 伝票発行日</summary>
            /// <value>伝票発行日</value>
            public DateTime? SlipPublishDate { get; set; }
            /// <summary>Gets or sets 売上種別|各種名称マスタ</summary>
            /// <value>売上種別|各種名称マスタ</value>
            public int? SalesKind { get; set; }
            /// <summary>Gets or sets 売上日付</summary>
            /// <value>売上日付</value>
            public DateTime SalesDate { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets データ種別|(分類マスタ参照)</summary>
            /// <value>データ種別|(分類マスタ参照)</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int CategoryDivision { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string SalesUserId { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 帳合コード</summary>
            /// <value>帳合コード</value>
            public string BalanceCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime? VenderActiveDate { get; set; }
            /// <summary>Gets or sets 請求先区分</summary>
            /// <value>請求先区分</value>
            public string InvoiceDivision { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 請求先開始有効日</summary>
            /// <value>請求先開始有効日</value>
            public DateTime? InvoiceActiveDate { get; set; }
            /// <summary>Gets or sets 担当部署コード</summary>
            /// <value>担当部署コード</value>
            public string ChargeOrganizationCd { get; set; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 消費税課税区分|各種名称マスタ</summary>
            /// <value>消費税課税区分|各種名称マスタ</value>
            public int TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal TaxRatio { get; set; }
            /// <summary>Gets or sets 会計部門借方コード</summary>
            /// <value>会計部門借方コード</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 会計部門貸方コード</summary>
            /// <value>会計部門貸方コード</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceDate { get; set; }
            /// <summary>Gets or sets 請求対象|0</summary>
            /// <value>請求対象|0</value>
            public int ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 請求更新フラグ|0</summary>
            /// <value>請求更新フラグ|0</value>
            public int ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 売掛対象|0</summary>
            /// <value>売掛対象|0</value>
            public int DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ|0</summary>
            /// <value>売掛更新フラグ|0</value>
            public int DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal CurrencyRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime RateValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 売上番号</summary>
                /// <value>売上番号</value>
                public string SalesNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="salesNo">売上番号</param>
                public PrimaryKey(string salesNo)
                {
                    SalesNo = salesNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SalesNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="salesNo">売上番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryClaimSalesHeaderEntity GetEntity(string salesNo, ComDB db)
            {
                TemporaryClaimSalesHeaderEntity.PrimaryKey condition = new TemporaryClaimSalesHeaderEntity.PrimaryKey(salesNo);
                return GetEntity<TemporaryClaimSalesHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め売上トランザクション_明細
        /// </summary>
        public class TemporaryClaimSalesDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryClaimSalesDetailEntity()
            {
                TableName = "temporary_claim_sales_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int RowNo { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目開始有効日</summary>
            /// <value>品目開始有効日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 品目仕様開始有効日</summary>
            /// <value>品目仕様開始有効日</value>
            public DateTime? SpecificationActiveDate { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 売上数量</summary>
            /// <value>売上数量</value>
            public decimal SalesQuantity { get; set; }
            /// <summary>Gets or sets 売上単価</summary>
            /// <value>売上単価</value>
            public decimal SalesUnitprice { get; set; }
            /// <summary>Gets or sets 売上金額</summary>
            /// <value>売上金額</value>
            public decimal SalesAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal TaxAmount { get; set; }
            /// <summary>Gets or sets 仮単価FLG|各種名称マスタ</summary>
            /// <value>仮単価FLG|各種名称マスタ</value>
            public int TmpUnitpriceFlg { get; set; }
            /// <summary>Gets or sets 入庫ロケーション</summary>
            /// <value>入庫ロケーション</value>
            public string HousingLocationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>サブロット番号1</value>
            public string SublotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>サブロット番号2</value>
            public string SublotNo2 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 売上番号</summary>
                /// <value>売上番号</value>
                public string SalesNo { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="salesNo">売上番号</param>
                /// <param name="rowNo">行番号</param>
                public PrimaryKey(string salesNo, int rowNo)
                {
                    SalesNo = salesNo;
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SalesNo, this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="salesNo">売上番号</param>
            /// <param name="rowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryClaimSalesDetailEntity GetEntity(string salesNo, int rowNo, ComDB db)
            {
                TemporaryClaimSalesDetailEntity.PrimaryKey condition = new TemporaryClaimSalesDetailEntity.PrimaryKey(salesNo, rowNo);
                return GetEntity<TemporaryClaimSalesDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮テーブル_BOM&処方別階層
        /// </summary>
        public class TemporaryCostLevelEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryCostLevelEntity()
            {
                TableName = "temporary_cost_level";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public long RowNo { get; set; }
            /// <summary>Gets or sets レベル</summary>
            /// <value>レベル</value>
            public long CostLevel { get; set; }
            /// <summary>Gets or sets 親品目コード</summary>
            /// <value>親品目コード</value>
            public string ParentItemCd { get; set; }
            /// <summary>Gets or sets 親仕様コード</summary>
            /// <value>親仕様コード</value>
            public string ParentSpecificationCd { get; set; }
            /// <summary>Gets or sets 子品目コード</summary>
            /// <value>子品目コード</value>
            public string ChildItemCd { get; set; }
            /// <summary>Gets or sets 子仕様コード</summary>
            /// <value>子仕様コード</value>
            public string ChildSpecificationCd { get; set; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long? RecipeId { get; set; }
            /// <summary>Gets or sets STEP_NO</summary>
            /// <value>STEP_NO</value>
            public int? StepNo { get; set; }
            /// <summary>Gets or sets LINE_NO</summary>
            /// <value>LINE_NO</value>
            public int? LineNo { get; set; }
            /// <summary>Gets or sets 基板ＩＤ</summary>
            /// <value>基板ＩＤ</value>
            public string Kibanid { get; set; }
            /// <summary>Gets or sets 累算数量</summary>
            /// <value>累算数量</value>
            public decimal? Qty { get; set; }
            /// <summary>Gets or sets 拡張フラグ１</summary>
            /// <value>拡張フラグ１</value>
            public int? Expflg1 { get; set; }
            /// <summary>Gets or sets 拡張フラグ２</summary>
            /// <value>拡張フラグ２</value>
            public int? Expflg2 { get; set; }
            /// <summary>Gets or sets 拡張フラグ３</summary>
            /// <value>拡張フラグ３</value>
            public int? Expflg3 { get; set; }
            /// <summary>Gets or sets 拡張フラグ４</summary>
            /// <value>拡張フラグ４</value>
            public int? Expflg4 { get; set; }
            /// <summary>Gets or sets 拡張フラグ５</summary>
            /// <value>拡張フラグ５</value>
            public int? Expflg5 { get; set; }
            /// <summary>Gets or sets 拡張区分１</summary>
            /// <value>拡張区分１</value>
            public int? Expdivision1 { get; set; }
            /// <summary>Gets or sets 拡張区分２</summary>
            /// <value>拡張区分２</value>
            public int? Expdivision2 { get; set; }
            /// <summary>Gets or sets 拡張区分３</summary>
            /// <value>拡張区分３</value>
            public int? Expdivision3 { get; set; }
            /// <summary>Gets or sets 拡張区分４</summary>
            /// <value>拡張区分４</value>
            public int? Expdivision4 { get; set; }
            /// <summary>Gets or sets 拡張区分５</summary>
            /// <value>拡張区分５</value>
            public int? Expdivision5 { get; set; }
            /// <summary>Gets or sets 拡張日付１</summary>
            /// <value>拡張日付１</value>
            public int? Expdate1 { get; set; }
            /// <summary>Gets or sets 拡張日付２</summary>
            /// <value>拡張日付２</value>
            public int? Expdate2 { get; set; }
            /// <summary>Gets or sets 拡張日付３</summary>
            /// <value>拡張日付３</value>
            public int? Expdate3 { get; set; }
            /// <summary>Gets or sets 拡張日付４</summary>
            /// <value>拡張日付４</value>
            public int? Expdate4 { get; set; }
            /// <summary>Gets or sets 拡張日付５</summary>
            /// <value>拡張日付５</value>
            public int? Expdate5 { get; set; }
            /// <summary>Gets or sets 拡張時間１</summary>
            /// <value>拡張時間１</value>
            public decimal? Exptime1 { get; set; }
            /// <summary>Gets or sets 拡張時間２</summary>
            /// <value>拡張時間２</value>
            public decimal? Exptime2 { get; set; }
            /// <summary>Gets or sets 拡張時間３</summary>
            /// <value>拡張時間３</value>
            public decimal? Exptime3 { get; set; }
            /// <summary>Gets or sets 拡張時間４</summary>
            /// <value>拡張時間４</value>
            public decimal? Exptime4 { get; set; }
            /// <summary>Gets or sets 拡張時間５</summary>
            /// <value>拡張時間５</value>
            public decimal? Exptime5 { get; set; }
            /// <summary>Gets or sets 拡張数値１</summary>
            /// <value>拡張数値１</value>
            public decimal? Expnum1 { get; set; }
            /// <summary>Gets or sets 拡張数値２</summary>
            /// <value>拡張数値２</value>
            public decimal? Expnum2 { get; set; }
            /// <summary>Gets or sets 拡張数値３</summary>
            /// <value>拡張数値３</value>
            public decimal? Expnum3 { get; set; }
            /// <summary>Gets or sets 拡張数値４</summary>
            /// <value>拡張数値４</value>
            public decimal? Expnum4 { get; set; }
            /// <summary>Gets or sets 拡張数値５</summary>
            /// <value>拡張数値５</value>
            public decimal? Expnum5 { get; set; }
            /// <summary>Gets or sets 拡張文字列１</summary>
            /// <value>拡張文字列１</value>
            public string Expvalue1 { get; set; }
            /// <summary>Gets or sets 拡張文字列２</summary>
            /// <value>拡張文字列２</value>
            public string Expvalue2 { get; set; }
            /// <summary>Gets or sets 拡張文字列３</summary>
            /// <value>拡張文字列３</value>
            public string Expvalue3 { get; set; }
            /// <summary>Gets or sets 拡張文字列４</summary>
            /// <value>拡張文字列４</value>
            public string Expvalue4 { get; set; }
            /// <summary>Gets or sets 拡張文字列５</summary>
            /// <value>拡張文字列５</value>
            public string Expvalue5 { get; set; }
            /// <summary>Gets or sets 会社コード</summary>
            /// <value>会社コード</value>
            public string CompanyCd { get; set; }
            /// <summary>Gets or sets 本支店コード</summary>
            /// <value>本支店コード</value>
            public string BranchCd { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public long RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="rowNo">行番号</param>
                public PrimaryKey(long rowNo)
                {
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="rowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryCostLevelEntity GetEntity(long rowNo, ComDB db)
            {
                TemporaryCostLevelEntity.PrimaryKey condition = new TemporaryCostLevelEntity.PrimaryKey(rowNo);
                return GetEntity<TemporaryCostLevelEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮テーブル_商品別原価
        /// </summary>
        public class TemporaryCostProductEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryCostProductEntity()
            {
                TableName = "temporary_cost_product";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int RowNo { get; set; }
            /// <summary>Gets or sets 親品目コード</summary>
            /// <value>親品目コード</value>
            public string ParentItemCd { get; set; }
            /// <summary>Gets or sets 親仕様コード</summary>
            /// <value>親仕様コード</value>
            public string ParentSpecificationCd { get; set; }
            /// <summary>Gets or sets 子品目コード</summary>
            /// <value>子品目コード</value>
            public string ChildItemCd { get; set; }
            /// <summary>Gets or sets 子仕様コード</summary>
            /// <value>子仕様コード</value>
            public string ChildSpecificationCd { get; set; }
            /// <summary>Gets or sets 標準生産量</summary>
            /// <value>標準生産量</value>
            public decimal? StdQty { get; set; }
            /// <summary>Gets or sets 数量</summary>
            /// <value>数量</value>
            public decimal? Qty { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime? StartDate { get; set; }
            /// <summary>Gets or sets 有効終了日</summary>
            /// <value>有効終了日</value>
            public DateTime? EndDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="rowNo">行番号</param>
                public PrimaryKey(int rowNo)
                {
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="rowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryCostProductEntity GetEntity(int rowNo, ComDB db)
            {
                TemporaryCostProductEntity.PrimaryKey condition = new TemporaryCostProductEntity.PrimaryKey(rowNo);
                return GetEntity<TemporaryCostProductEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め売掛入金トランザクション
        /// </summary>
        public class TemporaryDepositCreditEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryDepositCreditEntity()
            {
                TableName = "temporary_deposit_credit";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 入金番号</summary>
            /// <value>入金番号</value>
            public string CreditNo { get; set; }
            /// <summary>Gets or sets 入金行番号</summary>
            /// <value>入金行番号</value>
            public int CreditRowNo { get; set; }
            /// <summary>Gets or sets 入金元番号</summary>
            /// <value>入金元番号</value>
            public string OriginalCreditNo { get; set; }
            /// <summary>Gets or sets 入金ステータス|各種名称マスタ</summary>
            /// <value>入金ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 入金日付</summary>
            /// <value>入金日付</value>
            public DateTime CreditDate { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets データ種別|(分類マスタ参照)</summary>
            /// <value>データ種別|(分類マスタ参照)</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 入金金額</summary>
            /// <value>入金金額</value>
            public decimal? CreditAmount { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 口座区分</summary>
            /// <value>口座区分</value>
            public int? AccountDivision { get; set; }
            /// <summary>Gets or sets 口座番号</summary>
            /// <value>口座番号</value>
            public string AccountNo { get; set; }
            /// <summary>Gets or sets 手形種別</summary>
            /// <value>手形種別</value>
            public int? NoteDivision { get; set; }
            /// <summary>Gets or sets 手形番号</summary>
            /// <value>手形番号</value>
            public string NoteNo { get; set; }
            /// <summary>Gets or sets 振出日</summary>
            /// <value>振出日</value>
            public DateTime? DrawalDate { get; set; }
            /// <summary>Gets or sets 満期日</summary>
            /// <value>満期日</value>
            public DateTime? NoteDate { get; set; }
            /// <summary>Gets or sets 摘要名</summary>
            /// <value>摘要名</value>
            public string Summary { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ</summary>
            /// <value>消込完了フラグ</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 売掛対象</summary>
            /// <value>売掛対象</value>
            public int? DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 請求対象</summary>
            /// <value>請求対象</value>
            public int? ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ</summary>
            /// <value>売掛更新フラグ</value>
            public int? DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 請求更新フラグ</summary>
            /// <value>請求更新フラグ</value>
            public int? ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 入金番号</summary>
                /// <value>入金番号</value>
                public string CreditNo { get; set; }
                /// <summary>Gets or sets 入金行番号</summary>
                /// <value>入金行番号</value>
                public int CreditRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="creditNo">入金番号</param>
                /// <param name="creditRowNo">入金行番号</param>
                public PrimaryKey(string creditNo, int creditRowNo)
                {
                    CreditNo = creditNo;
                    CreditRowNo = creditRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.CreditNo, this.CreditRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="creditNo">入金番号</param>
            /// <param name="creditRowNo">入金行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryDepositCreditEntity GetEntity(string creditNo, int creditRowNo, ComDB db)
            {
                TemporaryDepositCreditEntity.PrimaryKey condition = new TemporaryDepositCreditEntity.PrimaryKey(creditNo, creditRowNo);
                return GetEntity<TemporaryDepositCreditEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め売掛ヘッダ
        /// </summary>
        public class TemporaryDepositHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryDepositHeaderEntity()
            {
                TableName = "temporary_deposit_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string InvoiceDivision { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime? InvoiceActiveDate { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime CreditDate { get; set; }
            /// <summary>Gets or sets 差引繰越額</summary>
            /// <value>差引繰越額</value>
            public decimal? BalanceForward { get; set; }
            /// <summary>Gets or sets 売上金額</summary>
            /// <value>売上金額</value>
            public decimal? SalesAmount { get; set; }
            /// <summary>Gets or sets 入金金額</summary>
            /// <value>入金金額</value>
            public decimal? DepositAmount { get; set; }
            /// <summary>Gets or sets その他入金金額</summary>
            /// <value>その他入金金額</value>
            public decimal? OtherDepositAmount { get; set; }
            /// <summary>Gets or sets 返品金額</summary>
            /// <value>返品金額</value>
            public decimal? ReturnedAmount { get; set; }
            /// <summary>Gets or sets 値引金額</summary>
            /// <value>値引金額</value>
            public decimal? DiscountAmount { get; set; }
            /// <summary>Gets or sets その他売上金額</summary>
            /// <value>その他売上金額</value>
            public decimal? OtherSalesAmount { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 売掛残</summary>
            /// <value>売掛残</value>
            public decimal? CreditAmount { get; set; }
            /// <summary>Gets or sets 売掛金(内訳)</summary>
            /// <value>売掛金(内訳)</value>
            public decimal? CreditSalesBreakdown { get; set; }
            /// <summary>Gets or sets 未収金(内訳)</summary>
            /// <value>未収金(内訳)</value>
            public decimal? AccruedDebitBreakdown { get; set; }
            /// <summary>Gets or sets 以外(内訳)</summary>
            /// <value>以外(内訳)</value>
            public decimal? ExceptBreakdown { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 売掛番号</summary>
                /// <value>売掛番号</value>
                public string DepositNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="depositNo">売掛番号</param>
                public PrimaryKey(string depositNo)
                {
                    DepositNo = depositNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.DepositNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="depositNo">売掛番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryDepositHeaderEntity GetEntity(string depositNo, ComDB db)
            {
                TemporaryDepositHeaderEntity.PrimaryKey condition = new TemporaryDepositHeaderEntity.PrimaryKey(depositNo);
                return GetEntity<TemporaryDepositHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め売掛支払トランザクション
        /// </summary>
        public class TemporaryDepositPaymentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryDepositPaymentEntity()
            {
                TableName = "temporary_deposit_payment";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets データ種別|1</summary>
            /// <value>データ種別|1</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分（分類マスタ参照）</summary>
            /// <value>データ集計区分（分類マスタ参照）</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード</summary>
            /// <value>分類コード</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 支払日付</summary>
            /// <value>支払日付</value>
            public DateTime PaymentDate { get; set; }
            /// <summary>Gets or sets 伝票番号</summary>
            /// <value>伝票番号</value>
            public string SlipNo { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int RowNo { get; set; }
            /// <summary>Gets or sets 仕入先コード</summary>
            /// <value>仕入先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 支払金額</summary>
            /// <value>支払金額</value>
            public decimal PaymentAmount { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 口座区分|各種名称マスタ</summary>
            /// <value>口座区分|各種名称マスタ</value>
            public int? AccountDivision { get; set; }
            /// <summary>Gets or sets 口座番号</summary>
            /// <value>口座番号</value>
            public string AccountNo { get; set; }
            /// <summary>Gets or sets 手形種別|各種名称マスタ</summary>
            /// <value>手形種別|各種名称マスタ</value>
            public int? NoteDivision { get; set; }
            /// <summary>Gets or sets 手形番号</summary>
            /// <value>手形番号</value>
            public string NoteNo { get; set; }
            /// <summary>Gets or sets 振出日</summary>
            /// <value>振出日</value>
            public DateTime? DrawalDate { get; set; }
            /// <summary>Gets or sets 満期日</summary>
            /// <value>満期日</value>
            public DateTime? NoteDate { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 借方補助科目コード</summary>
            /// <value>借方補助科目コード</value>
            public string DebitSubTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 貸方補助科目コード</summary>
            /// <value>貸方補助科目コード</value>
            public string CreditSubTitleCd { get; set; }
            /// <summary>Gets or sets 摘要名</summary>
            /// <value>摘要名</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 売掛対象|0</summary>
            /// <value>売掛対象|0</value>
            public int? DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 請求対象|0</summary>
            /// <value>請求対象|0</value>
            public int? ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛対象|0</summary>
            /// <value>買掛対象|0</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象|0</summary>
            /// <value>支払対象|0</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 伝票発行日</summary>
            /// <value>伝票発行日</value>
            public DateTime? IssueDate { get; set; }
            /// <summary>Gets or sets 伝票発行済フラグ|1</summary>
            /// <value>伝票発行済フラグ|1</value>
            public int IssuedDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ|0</summary>
            /// <value>売掛更新フラグ|0</value>
            public int? DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 請求更新フラグ|0</summary>
            /// <value>請求更新フラグ|0</value>
            public int? ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ|0</summary>
            /// <value>買掛更新フラグ|0</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 支払更新フラグ|0</summary>
            /// <value>支払更新フラグ|0</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 承認ステータス|1</summary>
            /// <value>承認ステータス|1</value>
            public int? ApprovalStatus { get; set; }
            /// <summary>Gets or sets 承認者</summary>
            /// <value>承認者</value>
            public string ApprovedBy { get; set; }
            /// <summary>Gets or sets 承認日</summary>
            /// <value>承認日</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 伝票番号</summary>
                /// <value>伝票番号</value>
                public string SlipNo { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="slipNo">伝票番号</param>
                /// <param name="rowNo">行番号</param>
                public PrimaryKey(string slipNo, int rowNo)
                {
                    SlipNo = slipNo;
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SlipNo, this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="slipNo">伝票番号</param>
            /// <param name="rowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryDepositPaymentEntity GetEntity(string slipNo, int rowNo, ComDB db)
            {
                TemporaryDepositPaymentEntity.PrimaryKey condition = new TemporaryDepositPaymentEntity.PrimaryKey(slipNo, rowNo);
                return GetEntity<TemporaryDepositPaymentEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め売掛売上トランザクション_ヘッダ
        /// </summary>
        public class TemporaryDepositSalesHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryDepositSalesHeaderEntity()
            {
                TableName = "temporary_deposit_sales_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 売上元番号</summary>
            /// <value>売上元番号</value>
            public string OriginalSalesNo { get; set; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets 出荷番号</summary>
            /// <value>出荷番号</value>
            public string ShippingNo { get; set; }
            /// <summary>Gets or sets 売上計上基準</summary>
            /// <value>売上計上基準</value>
            public int? RecordSalesBasis { get; set; }
            /// <summary>Gets or sets 売上ステータス|各種名称マスタ</summary>
            /// <value>売上ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 伝票発行済区分|各種名称マスタ</summary>
            /// <value>伝票発行済区分|各種名称マスタ</value>
            public int? SlipPublishComp { get; set; }
            /// <summary>Gets or sets 伝票発行日</summary>
            /// <value>伝票発行日</value>
            public DateTime? SlipPublishDate { get; set; }
            /// <summary>Gets or sets 売上種別|各種名称マスタ</summary>
            /// <value>売上種別|各種名称マスタ</value>
            public int? SalesKind { get; set; }
            /// <summary>Gets or sets 売上日付</summary>
            /// <value>売上日付</value>
            public DateTime SalesDate { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets データ種別|(分類マスタ参照)</summary>
            /// <value>データ種別|(分類マスタ参照)</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int CategoryDivision { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string SalesUserId { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 帳合コード</summary>
            /// <value>帳合コード</value>
            public string BalanceCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime? VenderActiveDate { get; set; }
            /// <summary>Gets or sets 請求先区分</summary>
            /// <value>請求先区分</value>
            public string InvoiceDivision { get; set; }
            /// <summary>Gets or sets 請求先コード</summary>
            /// <value>請求先コード</value>
            public string InvoiceCd { get; set; }
            /// <summary>Gets or sets 請求先開始有効日</summary>
            /// <value>請求先開始有効日</value>
            public DateTime? InvoiceActiveDate { get; set; }
            /// <summary>Gets or sets 担当部署コード</summary>
            /// <value>担当部署コード</value>
            public string ChargeOrganizationCd { get; set; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 消費税課税区分|各種名称マスタ</summary>
            /// <value>消費税課税区分|各種名称マスタ</value>
            public int TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal TaxRatio { get; set; }
            /// <summary>Gets or sets 会計部門借方コード</summary>
            /// <value>会計部門借方コード</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 会計部門貸方コード</summary>
            /// <value>会計部門貸方コード</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceDate { get; set; }
            /// <summary>Gets or sets 請求対象|0</summary>
            /// <value>請求対象|0</value>
            public int ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 請求更新フラグ|0</summary>
            /// <value>請求更新フラグ|0</value>
            public int ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 売掛対象|0</summary>
            /// <value>売掛対象|0</value>
            public int DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ|0</summary>
            /// <value>売掛更新フラグ|0</value>
            public int DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal CurrencyRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime RateValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 売上番号</summary>
                /// <value>売上番号</value>
                public string SalesNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="salesNo">売上番号</param>
                public PrimaryKey(string salesNo)
                {
                    SalesNo = salesNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SalesNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="salesNo">売上番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryDepositSalesHeaderEntity GetEntity(string salesNo, ComDB db)
            {
                TemporaryDepositSalesHeaderEntity.PrimaryKey condition = new TemporaryDepositSalesHeaderEntity.PrimaryKey(salesNo);
                return GetEntity<TemporaryDepositSalesHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め売掛売上トランザクション_明細
        /// </summary>
        public class TemporaryDepositSalesDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryDepositSalesDetailEntity()
            {
                TableName = "temporary_deposit_sales_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 売上番号</summary>
            /// <value>売上番号</value>
            public string SalesNo { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int RowNo { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目開始有効日</summary>
            /// <value>品目開始有効日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 品目仕様開始有効日</summary>
            /// <value>品目仕様開始有効日</value>
            public DateTime? SpecificationActiveDate { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 売上数量</summary>
            /// <value>売上数量</value>
            public decimal SalesQuantity { get; set; }
            /// <summary>Gets or sets 売上単価</summary>
            /// <value>売上単価</value>
            public decimal SalesUnitprice { get; set; }
            /// <summary>Gets or sets 売上金額</summary>
            /// <value>売上金額</value>
            public decimal SalesAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal TaxAmount { get; set; }
            /// <summary>Gets or sets 仮単価FLG|各種名称マスタ</summary>
            /// <value>仮単価FLG|各種名称マスタ</value>
            public int TmpUnitpriceFlg { get; set; }
            /// <summary>Gets or sets 入庫ロケーション</summary>
            /// <value>入庫ロケーション</value>
            public string HousingLocationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>サブロット番号1</value>
            public string SublotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>サブロット番号2</value>
            public string SublotNo2 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 売上番号</summary>
                /// <value>売上番号</value>
                public string SalesNo { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="salesNo">売上番号</param>
                /// <param name="rowNo">行番号</param>
                public PrimaryKey(string salesNo, int rowNo)
                {
                    SalesNo = salesNo;
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SalesNo, this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="salesNo">売上番号</param>
            /// <param name="rowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryDepositSalesDetailEntity GetEntity(string salesNo, int rowNo, ComDB db)
            {
                TemporaryDepositSalesDetailEntity.PrimaryKey condition = new TemporaryDepositSalesDetailEntity.PrimaryKey(salesNo, rowNo);
                return GetEntity<TemporaryDepositSalesDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め支払トランザクション(未使用)
        /// </summary>
        public class TemporaryPayableCreditEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryPayableCreditEntity()
            {
                TableName = "temporary_payable_credit";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 入金番号</summary>
            /// <value>入金番号</value>
            public string CreditNo { get; set; }
            /// <summary>Gets or sets 入金行番号</summary>
            /// <value>入金行番号</value>
            public int CreditRowNo { get; set; }
            /// <summary>Gets or sets 入金日付</summary>
            /// <value>入金日付</value>
            public DateTime CreditDate { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets データ種別|1</summary>
            /// <value>データ種別|1</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分</summary>
            /// <value>データ集計区分</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード</summary>
            /// <value>分類コード</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 入金金額</summary>
            /// <value>入金金額</value>
            public decimal? CreditAmount { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 口座区分|各種名称マスタ</summary>
            /// <value>口座区分|各種名称マスタ</value>
            public int? AccountDivision { get; set; }
            /// <summary>Gets or sets 口座番号</summary>
            /// <value>口座番号</value>
            public string AccountNo { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 摘要名</summary>
            /// <value>摘要名</value>
            public string Summary { get; set; }
            /// <summary>Gets or sets 手形種別|各種名称マスタ</summary>
            /// <value>手形種別|各種名称マスタ</value>
            public int? NoteDivision { get; set; }
            /// <summary>Gets or sets 手形番号</summary>
            /// <value>手形番号</value>
            public string NoteNo { get; set; }
            /// <summary>Gets or sets 振出日</summary>
            /// <value>振出日</value>
            public DateTime? DrawalDate { get; set; }
            /// <summary>Gets or sets 満期日</summary>
            /// <value>満期日</value>
            public DateTime? NoteDate { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 承認依頼先担当者ID</summary>
            /// <value>承認依頼先担当者ID</value>
            public string ReceiveUserId { get; set; }
            /// <summary>Gets or sets 承認依頼者ID</summary>
            /// <value>承認依頼者ID</value>
            public string ApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 承認依頼日時</summary>
            /// <value>承認依頼日時</value>
            public DateTime? ApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 承認者ID</summary>
            /// <value>承認者ID</value>
            public string ApprovedBy { get; set; }
            /// <summary>Gets or sets 承認日</summary>
            /// <value>承認日</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets 承認ステータス|1</summary>
            /// <value>承認ステータス|1</value>
            public int? ApprovalStatus { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 入金消込残</summary>
            /// <value>入金消込残</value>
            public decimal? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 伝票発行日</summary>
            /// <value>伝票発行日</value>
            public DateTime? IssueDate { get; set; }
            /// <summary>Gets or sets 伝票発行済フラグ|0</summary>
            /// <value>伝票発行済フラグ|0</value>
            public int IssuedDivision { get; set; }
            /// <summary>Gets or sets 売掛対象|0</summary>
            /// <value>売掛対象|0</value>
            public int? DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 請求対象|0</summary>
            /// <value>請求対象|0</value>
            public int? ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛対象|0</summary>
            /// <value>買掛対象|0</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象|0</summary>
            /// <value>支払対象|0</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ|0</summary>
            /// <value>売掛更新フラグ|0</value>
            public int? DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 請求更新フラグ|0</summary>
            /// <value>請求更新フラグ|0</value>
            public int? ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ|0</summary>
            /// <value>買掛更新フラグ|0</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 支払更新フラグ|0</summary>
            /// <value>支払更新フラグ|0</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }
            /// <summary>Gets or sets 仕訳作成ステータス|0</summary>
            /// <value>仕訳作成ステータス|0</value>
            public int? ShiwakeMakeStatus { get; set; }
            /// <summary>Gets or sets 仕訳送信ステータス|0</summary>
            /// <value>仕訳送信ステータス|0</value>
            public int? ShiwakeSendStatus { get; set; }
            /// <summary>Gets or sets 借方補助科目コード(未使用)</summary>
            /// <value>借方補助科目コード(未使用)</value>
            public string DebitSubTitleCd { get; set; }
            /// <summary>Gets or sets 貸方補助科目コード(未使用)</summary>
            /// <value>貸方補助科目コード(未使用)</value>
            public string CreditSubTitleCd { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 入金番号</summary>
                /// <value>入金番号</value>
                public string CreditNo { get; set; }
                /// <summary>Gets or sets 入金行番号</summary>
                /// <value>入金行番号</value>
                public int CreditRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="creditNo">入金番号</param>
                /// <param name="creditRowNo">入金行番号</param>
                public PrimaryKey(string creditNo, int creditRowNo)
                {
                    CreditNo = creditNo;
                    CreditRowNo = creditRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.CreditNo, this.CreditRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="creditNo">入金番号</param>
            /// <param name="creditRowNo">入金行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryPayableCreditEntity GetEntity(string creditNo, int creditRowNo, ComDB db)
            {
                TemporaryPayableCreditEntity.PrimaryKey condition = new TemporaryPayableCreditEntity.PrimaryKey(creditNo, creditRowNo);
                return GetEntity<TemporaryPayableCreditEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め買掛ヘッダ
        /// </summary>
        public class TemporaryPayableHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryPayableHeaderEntity()
            {
                TableName = "temporary_payable_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string SupplierDivision { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime? SupplierActiveDate { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime PayableDate { get; set; }
            /// <summary>Gets or sets 前月繰越</summary>
            /// <value>前月繰越</value>
            public decimal? BalanceForward { get; set; }
            /// <summary>Gets or sets 仕入金額</summary>
            /// <value>仕入金額</value>
            public decimal? StockingAmount { get; set; }
            /// <summary>Gets or sets 支払金額</summary>
            /// <value>支払金額</value>
            public decimal? PaymentAmount { get; set; }
            /// <summary>Gets or sets その他支払金額</summary>
            /// <value>その他支払金額</value>
            public decimal? OtherPaymentAmount { get; set; }
            /// <summary>Gets or sets 返品金額</summary>
            /// <value>返品金額</value>
            public decimal? StockingReturnedAmount { get; set; }
            /// <summary>Gets or sets 値引金額</summary>
            /// <value>値引金額</value>
            public decimal? StockingDiscountAmount { get; set; }
            /// <summary>Gets or sets その他仕入金額</summary>
            /// <value>その他仕入金額</value>
            public decimal? OtherStockingAmount { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 買掛残</summary>
            /// <value>買掛残</value>
            public decimal? PayableAmount { get; set; }
            /// <summary>Gets or sets 買掛金(内訳)</summary>
            /// <value>買掛金(内訳)</value>
            public decimal? AccountPayableBreakdown { get; set; }
            /// <summary>Gets or sets 未払金(内訳)</summary>
            /// <value>未払金(内訳)</value>
            public decimal? ArrearageBreakdown { get; set; }
            /// <summary>Gets or sets 以外(内訳)</summary>
            /// <value>以外(内訳)</value>
            public decimal? ExceptBreakdown { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 買掛番号</summary>
                /// <value>買掛番号</value>
                public string PayableNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="payableNo">買掛番号</param>
                public PrimaryKey(string payableNo)
                {
                    PayableNo = payableNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PayableNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="payableNo">買掛番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryPayableHeaderEntity GetEntity(string payableNo, ComDB db)
            {
                TemporaryPayableHeaderEntity.PrimaryKey condition = new TemporaryPayableHeaderEntity.PrimaryKey(payableNo);
                return GetEntity<TemporaryPayableHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め買掛支払トランザクション
        /// </summary>
        public class TemporaryPayablePaymentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryPayablePaymentEntity()
            {
                TableName = "temporary_payable_payment";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 支払伝票番号</summary>
            /// <value>支払伝票番号</value>
            public string PaymentSlipNo { get; set; }
            /// <summary>Gets or sets 支払伝票行番号</summary>
            /// <value>支払伝票行番号</value>
            public int PaymentSlipRowNo { get; set; }
            /// <summary>Gets or sets 支払ステータス|各種名称マスタ</summary>
            /// <value>支払ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 支払日付</summary>
            /// <value>支払日付</value>
            public DateTime PaymentDate { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets データ種別|(分類マスタ参照)</summary>
            /// <value>データ種別|(分類マスタ参照)</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int CategoryDivision { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 支払金額</summary>
            /// <value>支払金額</value>
            public decimal PaymentAmount { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 口座区分|各種名称マスタ</summary>
            /// <value>口座区分|各種名称マスタ</value>
            public int? AccountDivision { get; set; }
            /// <summary>Gets or sets 口座番号</summary>
            /// <value>口座番号</value>
            public string AccountNo { get; set; }
            /// <summary>Gets or sets 手形種別|各種名称マスタ</summary>
            /// <value>手形種別|各種名称マスタ</value>
            public int? NoteDivision { get; set; }
            /// <summary>Gets or sets 手形番号</summary>
            /// <value>手形番号</value>
            public string NoteNo { get; set; }
            /// <summary>Gets or sets 振出日</summary>
            /// <value>振出日</value>
            public DateTime? DrawalDate { get; set; }
            /// <summary>Gets or sets 満期日</summary>
            /// <value>満期日</value>
            public DateTime? NoteDate { get; set; }
            /// <summary>Gets or sets 摘要名</summary>
            /// <value>摘要名</value>
            public string Summary { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }
            /// <summary>Gets or sets 買掛対象|0</summary>
            /// <value>買掛対象|0</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象|0</summary>
            /// <value>支払対象|0</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ|0</summary>
            /// <value>買掛更新フラグ|0</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 支払更新フラグ|0</summary>
            /// <value>支払更新フラグ|0</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 支払伝票番号</summary>
                /// <value>支払伝票番号</value>
                public string PaymentSlipNo { get; set; }
                /// <summary>Gets or sets 支払伝票行番号</summary>
                /// <value>支払伝票行番号</value>
                public int PaymentSlipRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="paymentSlipNo">支払伝票番号</param>
                /// <param name="paymentSlipRowNo">支払伝票行番号</param>
                public PrimaryKey(string paymentSlipNo, int paymentSlipRowNo)
                {
                    PaymentSlipNo = paymentSlipNo;
                    PaymentSlipRowNo = paymentSlipRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PaymentSlipNo, this.PaymentSlipRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="paymentSlipNo">支払伝票番号</param>
            /// <param name="paymentSlipRowNo">支払伝票行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryPayablePaymentEntity GetEntity(string paymentSlipNo, int paymentSlipRowNo, ComDB db)
            {
                TemporaryPayablePaymentEntity.PrimaryKey condition = new TemporaryPayablePaymentEntity.PrimaryKey(paymentSlipNo, paymentSlipRowNo);
                return GetEntity<TemporaryPayablePaymentEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め支払ヘッダー
        /// </summary>
        public class TemporaryPaymentHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryPaymentHeaderEntity()
            {
                TableName = "temporary_payment_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime PayableDate { get; set; }
            /// <summary>Gets or sets 支払予定日</summary>
            /// <value>支払予定日</value>
            public DateTime PayableScheduledDate { get; set; }
            /// <summary>Gets or sets 支払区分</summary>
            /// <value>支払区分</value>
            public int PayableDivision { get; set; }
            /// <summary>Gets or sets 手形サイト</summary>
            /// <value>手形サイト</value>
            public int? NoteSight { get; set; }
            /// <summary>Gets or sets 休日指定フラグ|各種名称マスタ</summary>
            /// <value>休日指定フラグ|各種名称マスタ</value>
            public int? HolidayFlg { get; set; }
            /// <summary>Gets or sets 前月支払残高</summary>
            /// <value>前月支払残高</value>
            public decimal? ClaimAmountForward { get; set; }
            /// <summary>Gets or sets 支払額</summary>
            /// <value>支払額</value>
            public decimal? CreditAmountForward { get; set; }
            /// <summary>Gets or sets その他支払額</summary>
            /// <value>その他支払額</value>
            public decimal? OtherCreditAmountForward { get; set; }
            /// <summary>Gets or sets 仕入金額</summary>
            /// <value>仕入金額</value>
            public decimal? StockingAmount { get; set; }
            /// <summary>Gets or sets 返品金額</summary>
            /// <value>返品金額</value>
            public decimal? StockingReturnedAmount { get; set; }
            /// <summary>Gets or sets 値引金額</summary>
            /// <value>値引金額</value>
            public decimal? StockingDiscountAmount { get; set; }
            /// <summary>Gets or sets その他仕入金額</summary>
            /// <value>その他仕入金額</value>
            public decimal? OtherStockingAmount { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 当月支払残高</summary>
            /// <value>当月支払残高</value>
            public decimal? PayableAmount { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserAmount { get; set; }
            /// <summary>Gets or sets 消込残</summary>
            /// <value>消込残</value>
            public decimal? EraserBalanceAmount { get; set; }
            /// <summary>Gets or sets 検収明細通知書発行済フラグ</summary>
            /// <value>検収明細通知書発行済フラグ</value>
            public int? BillDivision { get; set; }
            /// <summary>Gets or sets 発行日時</summary>
            /// <value>発行日時</value>
            public DateTime? IssueDate { get; set; }
            /// <summary>Gets or sets 発行者ID</summary>
            /// <value>発行者ID</value>
            public string IssuerCd { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 支払番号</summary>
                /// <value>支払番号</value>
                public string PaymentNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="paymentNo">支払番号</param>
                public PrimaryKey(string paymentNo)
                {
                    PaymentNo = paymentNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PaymentNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="paymentNo">支払番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryPaymentHeaderEntity GetEntity(string paymentNo, ComDB db)
            {
                TemporaryPaymentHeaderEntity.PrimaryKey condition = new TemporaryPaymentHeaderEntity.PrimaryKey(paymentNo);
                return GetEntity<TemporaryPaymentHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め買掛仕入トランザクション
        /// </summary>
        public class TemporaryPayableStockingEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryPayableStockingEntity()
            {
                TableName = "temporary_payable_stocking";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 仕入番号</summary>
            /// <value>仕入番号</value>
            public string StockingNo { get; set; }
            /// <summary>Gets or sets 仕入行番号</summary>
            /// <value>仕入行番号</value>
            public int StockingRowNo { get; set; }
            /// <summary>Gets or sets 仕入元番号</summary>
            /// <value>仕入元番号</value>
            public string OriginalStockingNo { get; set; }
            /// <summary>Gets or sets 受入番号</summary>
            /// <value>受入番号</value>
            public string AcceptingNo { get; set; }
            /// <summary>Gets or sets 受入行番号</summary>
            /// <value>受入行番号</value>
            public int AcceptingRowNo { get; set; }
            /// <summary>Gets or sets 仕入ステータス</summary>
            /// <value>仕入ステータス</value>
            public int? StockingStatus { get; set; }
            /// <summary>Gets or sets 伝票発行日</summary>
            /// <value>伝票発行日</value>
            public DateTime? SlipPublishDate { get; set; }
            /// <summary>Gets or sets 仕入種別|各種名称マスタ</summary>
            /// <value>仕入種別|各種名称マスタ</value>
            public int? StockingKind { get; set; }
            /// <summary>Gets or sets 仕入日付</summary>
            /// <value>仕入日付</value>
            public DateTime StockingDate { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets データ種別(分類マスタ参照)</summary>
            /// <value>データ種別(分類マスタ参照)</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets データ種別集計区分(分類マスタ参照)</summary>
            /// <value>データ種別集計区分(分類マスタ参照)</value>
            public int DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int CategoryDivision { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string StockingUserId { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime? VenderActiveDate { get; set; }
            /// <summary>Gets or sets 支払先区分</summary>
            /// <value>支払先区分</value>
            public string SupplierDivision { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 支払先開始有効日</summary>
            /// <value>支払先開始有効日</value>
            public DateTime? SupplierActiveDate { get; set; }
            /// <summary>Gets or sets 担当部署コード</summary>
            /// <value>担当部署コード</value>
            public string ChargeOrganizationCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目開始有効日</summary>
            /// <value>品目開始有効日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 品目仕様開始有効日</summary>
            /// <value>品目仕様開始有効日</value>
            public DateTime? SpecificationActiveDate { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 仕入数量</summary>
            /// <value>仕入数量</value>
            public decimal? StockingQuantity { get; set; }
            /// <summary>Gets or sets 仕入単価</summary>
            /// <value>仕入単価</value>
            public decimal? StockingUnitprice { get; set; }
            /// <summary>Gets or sets 仕入金額</summary>
            /// <value>仕入金額</value>
            public decimal? StockingAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmonut { get; set; }
            /// <summary>Gets or sets 消費税課税区分|各種名称マスタ</summary>
            /// <value>消費税課税区分|各種名称マスタ</value>
            public int TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal TaxRatio { get; set; }
            /// <summary>Gets or sets 出庫ロケーション</summary>
            /// <value>出庫ロケーション</value>
            public string WarehousingLocation { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets 会計部門借方コード</summary>
            /// <value>会計部門借方コード</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 会計部門貸方コード</summary>
            /// <value>会計部門貸方コード</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? CurrencyRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? RateValidDate { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 支払対象|0</summary>
            /// <value>支払対象|0</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 支払更新フラグ|0</summary>
            /// <value>支払更新フラグ|0</value>
            public int PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 買掛対象|0</summary>
            /// <value>買掛対象|0</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ|0</summary>
            /// <value>買掛更新フラグ|0</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 仕入番号</summary>
                /// <value>仕入番号</value>
                public string StockingNo { get; set; }
                /// <summary>Gets or sets 仕入行番号</summary>
                /// <value>仕入行番号</value>
                public int StockingRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="stockingNo">仕入番号</param>
                /// <param name="stockingRowNo">仕入行番号</param>
                public PrimaryKey(string stockingNo, int stockingRowNo)
                {
                    StockingNo = stockingNo;
                    StockingRowNo = stockingRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.StockingNo, this.StockingRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="stockingNo">仕入番号</param>
            /// <param name="stockingRowNo">仕入行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryPayableStockingEntity GetEntity(string stockingNo, int stockingRowNo, ComDB db)
            {
                TemporaryPayableStockingEntity.PrimaryKey condition = new TemporaryPayableStockingEntity.PrimaryKey(stockingNo, stockingRowNo);
                return GetEntity<TemporaryPayableStockingEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮テーブル_BOM&処方別階層~1
        /// </summary>
        public class TemporaryRecipeInfoEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryRecipeInfoEntity()
            {
                TableName = "temporary_recipe_info";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 親品目コード</summary>
            /// <value>親品目コード</value>
            public string ParentItemCd { get; set; }
            /// <summary>Gets or sets 親仕様コード</summary>
            /// <value>親仕様コード</value>
            public string ParentSpecificationCd { get; set; }
            /// <summary>Gets or sets 子品目コード</summary>
            /// <value>子品目コード</value>
            public string ChildItemCd { get; set; }
            /// <summary>Gets or sets 子仕様コード</summary>
            /// <value>子仕様コード</value>
            public string ChildSpecificationCd { get; set; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long? RecipeId { get; set; }
            /// <summary>Gets or sets ステップNO</summary>
            /// <value>ステップNO</value>
            public int? StepNo { get; set; }
            /// <summary>Gets or sets ラインNO</summary>
            /// <value>ラインNO</value>
            public int? LineNo { get; set; }
            /// <summary>Gets or sets 数量</summary>
            /// <value>数量</value>
            public decimal? Qty { get; set; }
            /// <summary>Gets or sets 開始日</summary>
            /// <value>開始日</value>
            public DateTime? HeaderStartDate { get; set; }
            /// <summary>Gets or sets 終了日</summary>
            /// <value>終了日</value>
            public DateTime? HeaderEndDate { get; set; }
        }

        /// <summary>
        /// 仮締めグループ間相殺ヘッダ
        /// </summary>
        public class TmpclaimOffsetGroupHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TmpclaimOffsetGroupHeaderEntity()
            {
                TableName = "tmpclaim_offset_group_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string OffsetUserId { get; set; }
            /// <summary>Gets or sets 相殺グループコード</summary>
            /// <value>相殺グループコード</value>
            public string OffsetGroupCd { get; set; }
            /// <summary>Gets or sets データ種別(分類マスタ参照)</summary>
            /// <value>データ種別(分類マスタ参照)</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 相殺日付</summary>
            /// <value>相殺日付</value>
            public DateTime OffsetDate { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 相殺番号</summary>
                /// <value>相殺番号</value>
                public string OffsetNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="offsetNo">相殺番号</param>
                public PrimaryKey(string offsetNo)
                {
                    OffsetNo = offsetNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OffsetNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="offsetNo">相殺番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TmpclaimOffsetGroupHeaderEntity GetEntity(string offsetNo, ComDB db)
            {
                TmpclaimOffsetGroupHeaderEntity.PrimaryKey condition = new TmpclaimOffsetGroupHeaderEntity.PrimaryKey(offsetNo);
                return GetEntity<TmpclaimOffsetGroupHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締めグループ間相殺トランザクション
        /// </summary>
        public class TmpclaimOffsetGroupDataEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TmpclaimOffsetGroupDataEntity()
            {
                TableName = "tmpclaim_offset_group_data";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 買掛金、未払金相殺金額</summary>
            /// <value>買掛金、未払金相殺金額</value>
            public decimal? PayableOffsetAmount { get; set; }
            /// <summary>Gets or sets 売掛金、未収金相殺金額</summary>
            /// <value>売掛金、未収金相殺金額</value>
            public decimal? DepositOffsetAmount { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 売掛対象</summary>
            /// <value>売掛対象</value>
            public int? DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 請求対象</summary>
            /// <value>請求対象</value>
            public int? ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛対象</summary>
            /// <value>買掛対象</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象</summary>
            /// <value>支払対象</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ</summary>
            /// <value>売掛更新フラグ</value>
            public int? DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 請求更新フラグ</summary>
            /// <value>請求更新フラグ</value>
            public int? ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ</summary>
            /// <value>買掛更新フラグ</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 支払更新フラグ</summary>
            /// <value>支払更新フラグ</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserOffsetAmount { get; set; }
            /// <summary>Gets or sets 入金消込残</summary>
            /// <value>入金消込残</value>
            public long? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 支払消込残</summary>
            /// <value>支払消込残</value>
            public long? PaymentEraserAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ</summary>
            /// <value>消込完了フラグ</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 相殺番号</summary>
                /// <value>相殺番号</value>
                public string OffsetNo { get; set; }
                /// <summary>Gets or sets 取引先区分</summary>
                /// <value>取引先区分</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 取引先コード</summary>
                /// <value>取引先コード</value>
                public string VenderCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="offsetNo">相殺番号</param>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="venderCd">取引先コード</param>
                public PrimaryKey(string offsetNo, string venderDivision, string venderCd)
                {
                    OffsetNo = offsetNo;
                    VenderDivision = venderDivision;
                    VenderCd = venderCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OffsetNo, this.VenderDivision, this.VenderCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="offsetNo">相殺番号</param>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TmpclaimOffsetGroupDataEntity GetEntity(string offsetNo, string venderDivision, string venderCd, ComDB db)
            {
                TmpclaimOffsetGroupDataEntity.PrimaryKey condition = new TmpclaimOffsetGroupDataEntity.PrimaryKey(offsetNo, venderDivision, venderCd);
                return GetEntity<TmpclaimOffsetGroupDataEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め売掛グループ間相殺ヘッダ
        /// </summary>
        public class TmpdeptOffsetGroupHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TmpdeptOffsetGroupHeaderEntity()
            {
                TableName = "tmpdept_offset_group_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string OffsetUserId { get; set; }
            /// <summary>Gets or sets 相殺グループコード</summary>
            /// <value>相殺グループコード</value>
            public string OffsetGroupCd { get; set; }
            /// <summary>Gets or sets データ種別(分類マスタ参照)</summary>
            /// <value>データ種別(分類マスタ参照)</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 相殺日付</summary>
            /// <value>相殺日付</value>
            public DateTime OffsetDate { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 相殺番号</summary>
                /// <value>相殺番号</value>
                public string OffsetNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="offsetNo">相殺番号</param>
                public PrimaryKey(string offsetNo)
                {
                    OffsetNo = offsetNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OffsetNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="offsetNo">相殺番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TmpdeptOffsetGroupHeaderEntity GetEntity(string offsetNo, ComDB db)
            {
                TmpdeptOffsetGroupHeaderEntity.PrimaryKey condition = new TmpdeptOffsetGroupHeaderEntity.PrimaryKey(offsetNo);
                return GetEntity<TmpdeptOffsetGroupHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め売掛グループ間相殺トランザクション
        /// </summary>
        public class TmpdeptOffsetGroupDataEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TmpdeptOffsetGroupDataEntity()
            {
                TableName = "tmpdept_offset_group_data";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 買掛金、未払金相殺金額</summary>
            /// <value>買掛金、未払金相殺金額</value>
            public decimal? PayableOffsetAmount { get; set; }
            /// <summary>Gets or sets 売掛金、未収金相殺金額</summary>
            /// <value>売掛金、未収金相殺金額</value>
            public decimal? DepositOffsetAmount { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 売掛対象</summary>
            /// <value>売掛対象</value>
            public int? DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 請求対象</summary>
            /// <value>請求対象</value>
            public int? ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛対象</summary>
            /// <value>買掛対象</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象</summary>
            /// <value>支払対象</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ</summary>
            /// <value>売掛更新フラグ</value>
            public int? DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 請求更新フラグ</summary>
            /// <value>請求更新フラグ</value>
            public int? ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ</summary>
            /// <value>買掛更新フラグ</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 支払更新フラグ</summary>
            /// <value>支払更新フラグ</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserOffsetAmount { get; set; }
            /// <summary>Gets or sets 入金消込残</summary>
            /// <value>入金消込残</value>
            public long? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 支払消込残</summary>
            /// <value>支払消込残</value>
            public long? PaymentEraserAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ</summary>
            /// <value>消込完了フラグ</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 相殺番号</summary>
                /// <value>相殺番号</value>
                public string OffsetNo { get; set; }
                /// <summary>Gets or sets 取引先区分</summary>
                /// <value>取引先区分</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 取引先コード</summary>
                /// <value>取引先コード</value>
                public string VenderCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="offsetNo">相殺番号</param>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="venderCd">取引先コード</param>
                public PrimaryKey(string offsetNo, string venderDivision, string venderCd)
                {
                    OffsetNo = offsetNo;
                    VenderDivision = venderDivision;
                    VenderCd = venderCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OffsetNo, this.VenderDivision, this.VenderCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="offsetNo">相殺番号</param>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TmpdeptOffsetGroupDataEntity GetEntity(string offsetNo, string venderDivision, string venderCd, ComDB db)
            {
                TmpdeptOffsetGroupDataEntity.PrimaryKey condition = new TmpdeptOffsetGroupDataEntity.PrimaryKey(offsetNo, venderDivision, venderCd);
                return GetEntity<TmpdeptOffsetGroupDataEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め支払グループ間相殺ヘッダ
        /// </summary>
        public class TmppayOffsetGroupHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TmppayOffsetGroupHeaderEntity()
            {
                TableName = "tmppay_offset_group_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string OffsetUserId { get; set; }
            /// <summary>Gets or sets 相殺グループコード</summary>
            /// <value>相殺グループコード</value>
            public string OffsetGroupCd { get; set; }
            /// <summary>Gets or sets データ種別(分類マスタ参照)</summary>
            /// <value>データ種別(分類マスタ参照)</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分(分類マスタ参照)</summary>
            /// <value>データ集計区分(分類マスタ参照)</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 相殺日付</summary>
            /// <value>相殺日付</value>
            public DateTime OffsetDate { get; set; }
            /// <summary>Gets or sets 相殺金額</summary>
            /// <value>相殺金額</value>
            public decimal? OffsetAmount { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 相殺番号</summary>
                /// <value>相殺番号</value>
                public string OffsetNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="offsetNo">相殺番号</param>
                public PrimaryKey(string offsetNo)
                {
                    OffsetNo = offsetNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OffsetNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="offsetNo">相殺番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TmppayOffsetGroupHeaderEntity GetEntity(string offsetNo, ComDB db)
            {
                TmppayOffsetGroupHeaderEntity.PrimaryKey condition = new TmppayOffsetGroupHeaderEntity.PrimaryKey(offsetNo);
                return GetEntity<TmppayOffsetGroupHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め支払グループ間相殺トランザクション
        /// </summary>
        public class TmppayOffsetGroupDataEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TmppayOffsetGroupDataEntity()
            {
                TableName = "tmppay_offset_group_data";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 相殺番号</summary>
            /// <value>相殺番号</value>
            public string OffsetNo { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime VenderActiveDate { get; set; }
            /// <summary>Gets or sets 買掛金、未払金相殺金額</summary>
            /// <value>買掛金、未払金相殺金額</value>
            public decimal? PayableOffsetAmount { get; set; }
            /// <summary>Gets or sets 売掛金、未収金相殺金額</summary>
            /// <value>売掛金、未収金相殺金額</value>
            public decimal? DepositOffsetAmount { get; set; }
            /// <summary>Gets or sets 借方部門コード</summary>
            /// <value>借方部門コード</value>
            public string DebitSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方部門コード</summary>
            /// <value>貸方部門コード</value>
            public string CreditSectionCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 売掛対象</summary>
            /// <value>売掛対象</value>
            public int? DepositTargetDivision { get; set; }
            /// <summary>Gets or sets 請求対象</summary>
            /// <value>請求対象</value>
            public int? ClaimTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛対象</summary>
            /// <value>買掛対象</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象</summary>
            /// <value>支払対象</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 売掛更新フラグ</summary>
            /// <value>売掛更新フラグ</value>
            public int? DepositUpdateDivision { get; set; }
            /// <summary>Gets or sets 売掛番号</summary>
            /// <value>売掛番号</value>
            public string DepositNo { get; set; }
            /// <summary>Gets or sets 売掛締め日</summary>
            /// <value>売掛締め日</value>
            public DateTime? DeliveryUpdateDate { get; set; }
            /// <summary>Gets or sets 請求更新フラグ</summary>
            /// <value>請求更新フラグ</value>
            public int? ClaimUpdateDivision { get; set; }
            /// <summary>Gets or sets 請求番号</summary>
            /// <value>請求番号</value>
            public string ClaimNo { get; set; }
            /// <summary>Gets or sets 請求締め日</summary>
            /// <value>請求締め日</value>
            public DateTime? InvoiceUpdateDate { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ</summary>
            /// <value>買掛更新フラグ</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 支払更新フラグ</summary>
            /// <value>支払更新フラグ</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 消込額</summary>
            /// <value>消込額</value>
            public decimal? EraserOffsetAmount { get; set; }
            /// <summary>Gets or sets 入金消込残</summary>
            /// <value>入金消込残</value>
            public long? CreditEraserAmount { get; set; }
            /// <summary>Gets or sets 支払消込残</summary>
            /// <value>支払消込残</value>
            public long? PaymentEraserAmount { get; set; }
            /// <summary>Gets or sets 消込完了フラグ</summary>
            /// <value>消込完了フラグ</value>
            public int? EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 相殺番号</summary>
                /// <value>相殺番号</value>
                public string OffsetNo { get; set; }
                /// <summary>Gets or sets 取引先区分</summary>
                /// <value>取引先区分</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 取引先コード</summary>
                /// <value>取引先コード</value>
                public string VenderCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="offsetNo">相殺番号</param>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="venderCd">取引先コード</param>
                public PrimaryKey(string offsetNo, string venderDivision, string venderCd)
                {
                    OffsetNo = offsetNo;
                    VenderDivision = venderDivision;
                    VenderCd = venderCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OffsetNo, this.VenderDivision, this.VenderCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="offsetNo">相殺番号</param>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TmppayOffsetGroupDataEntity GetEntity(string offsetNo, string venderDivision, string venderCd, ComDB db)
            {
                TmppayOffsetGroupDataEntity.PrimaryKey condition = new TmppayOffsetGroupDataEntity.PrimaryKey(offsetNo, venderDivision, venderCd);
                return GetEntity<TmppayOffsetGroupDataEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仮締め買掛購買外注オーダー
        /// </summary>
        public class TmppayPurchaseSubcontractEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TmppayPurchaseSubcontractEntity()
            {
                TableName = "tmppay_purchase_subcontract";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 購買NO</summary>
            /// <value>購買NO</value>
            public string PurchaseNo { get; set; }
            /// <summary>Gets or sets 発注番号</summary>
            /// <value>発注番号</value>
            public string BuySubcontractOrderNo { get; set; }
            /// <summary>Gets or sets 発注番号分納枝番</summary>
            /// <value>発注番号分納枝番</value>
            public string OrderDivideNo { get; set; }
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets 仕入先受注番</summary>
            /// <value>仕入先受注番</value>
            public string SiOrderNo { get; set; }
            /// <summary>Gets or sets 発注日</summary>
            /// <value>発注日</value>
            public DateTime? OrderDate { get; set; }
            /// <summary>Gets or sets 担当部署コード</summary>
            /// <value>担当部署コード</value>
            public string ChargeOrganizationCd { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 発注区分|各種名称マスタ</summary>
            /// <value>発注区分|各種名称マスタ</value>
            public int? OrderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 発注数量</summary>
            /// <value>発注数量</value>
            public decimal? OrderQuantity { get; set; }
            /// <summary>Gets or sets 重量</summary>
            /// <value>重量</value>
            public decimal? OrderConvertQuantity { get; set; }
            /// <summary>Gets or sets 受注単価</summary>
            /// <value>受注単価</value>
            public decimal? OrderUnitprice { get; set; }
            /// <summary>Gets or sets 単価決定単位区分|各種名称マスタ</summary>
            /// <value>単価決定単位区分|各種名称マスタ</value>
            public int? UnitpriceDefineunit { get; set; }
            /// <summary>Gets or sets 発注金額</summary>
            /// <value>発注金額</value>
            public decimal? SupplierOrdAmount { get; set; }
            /// <summary>Gets or sets 納品希望日</summary>
            /// <value>納品希望日</value>
            public DateTime? SuggestedDeliverlimitDate { get; set; }
            /// <summary>Gets or sets 発注書備考</summary>
            /// <value>発注書備考</value>
            public string OrderSheetRemark { get; set; }
            /// <summary>Gets or sets 発注時備考</summary>
            /// <value>発注時備考</value>
            public string PurchaseRemark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets ステータス|各種名称マスタ</summary>
            /// <value>ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 発注書NO</summary>
            /// <value>発注書NO</value>
            public string OrderSheetNo { get; set; }
            /// <summary>Gets or sets 発注書発行フラグ|各種名称マスタ</summary>
            /// <value>発注書発行フラグ|各種名称マスタ</value>
            public int? OrderSheetFlg { get; set; }
            /// <summary>Gets or sets 発注書発行日</summary>
            /// <value>発注書発行日</value>
            public DateTime? OrderSheetDate { get; set; }
            /// <summary>Gets or sets 発注書発行者</summary>
            /// <value>発注書発行者</value>
            public string OrderSheetUserId { get; set; }
            /// <summary>Gets or sets 分納区分|各種名称マスタ</summary>
            /// <value>分納区分|各種名称マスタ</value>
            public int? ReplyContentsDivision { get; set; }
            /// <summary>Gets or sets 完納区分|0</summary>
            /// <value>完納区分|0</value>
            public int? DeliveryComp { get; set; }
            /// <summary>Gets or sets 次回納品希望日</summary>
            /// <value>次回納品希望日</value>
            public DateTime? NextDeliverlimitDate { get; set; }
            /// <summary>Gets or sets データ種別|1</summary>
            /// <value>データ種別|1</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets データ集計区分（分類マスタ参照）</summary>
            /// <value>データ集計区分（分類マスタ参照）</value>
            public int? DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード（分類マスタ参照）</summary>
            /// <value>分類コード（分類マスタ参照）</value>
            public int? CategoryDivision { get; set; }
            /// <summary>Gets or sets 仕入日付</summary>
            /// <value>仕入日付</value>
            public DateTime? StockingDate { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 伝票番号</summary>
            /// <value>伝票番号</value>
            public string SlipNo { get; set; }
            /// <summary>Gets or sets 仕入行番号</summary>
            /// <value>仕入行番号</value>
            public int? SilpRowNo { get; set; }
            /// <summary>Gets or sets 仕入-取消　仕入番号</summary>
            /// <value>仕入-取消　仕入番号</value>
            public string CancelSlipNo { get; set; }
            /// <summary>Gets or sets 仕入-取消　行番号</summary>
            /// <value>仕入-取消　行番号</value>
            public int? CancelRowNo { get; set; }
            /// <summary>Gets or sets メーカーロット番号</summary>
            /// <value>メーカーロット番号</value>
            public string VenderLotNo { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets 入庫ロケーション</summary>
            /// <value>入庫ロケーション</value>
            public string HousingLocationCd { get; set; }
            /// <summary>Gets or sets 入荷予定数量</summary>
            /// <value>入荷予定数量</value>
            public decimal? ArrivalQuantity { get; set; }
            /// <summary>Gets or sets 受入数量</summary>
            /// <value>受入数量</value>
            public decimal? AcceptQuantity { get; set; }
            /// <summary>Gets or sets 合格数換算量</summary>
            /// <value>合格数換算量</value>
            public decimal? AcceptConvertQuantity { get; set; }
            /// <summary>Gets or sets 仕入数量</summary>
            /// <value>仕入数量</value>
            public decimal? StockingQuantity { get; set; }
            /// <summary>Gets or sets 仕入単価</summary>
            /// <value>仕入単価</value>
            public decimal? HousingUnitprice { get; set; }
            /// <summary>Gets or sets 運賃</summary>
            /// <value>運賃</value>
            public decimal? FareAmount { get; set; }
            /// <summary>Gets or sets 仕入金額</summary>
            /// <value>仕入金額</value>
            public decimal? StockingAmount { get; set; }
            /// <summary>Gets or sets 受入日付</summary>
            /// <value>受入日付</value>
            public DateTime? AcceptDate { get; set; }
            /// <summary>Gets or sets 発注書備考（入荷以降）</summary>
            /// <value>発注書備考（入荷以降）</value>
            public string OrderSheetRemark2 { get; set; }
            /// <summary>Gets or sets 備考（入荷以降）</summary>
            /// <value>備考（入荷以降）</value>
            public string Remark2 { get; set; }
            /// <summary>Gets or sets 注釈（入荷以降）</summary>
            /// <value>注釈（入荷以降）</value>
            public long? Notes2 { get; set; }
            /// <summary>Gets or sets 仕入ステータス|各種名称マスタ</summary>
            /// <value>仕入ステータス|各種名称マスタ</value>
            public int? Status2 { get; set; }
            /// <summary>Gets or sets 消費税課税区分|各種名称マスタ</summary>
            /// <value>消費税課税区分|各種名称マスタ</value>
            public int? TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string PayeeCd { get; set; }
            /// <summary>Gets or sets 会計部門借方コード</summary>
            /// <value>会計部門借方コード</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 会計部門貸方コード</summary>
            /// <value>会計部門貸方コード</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 借方補助科目コード</summary>
            /// <value>借方補助科目コード</value>
            public string DebitSubTitleCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 貸方補助科目コード</summary>
            /// <value>貸方補助科目コード</value>
            public string CreditSubTitleCd { get; set; }
            /// <summary>Gets or sets 買掛対象|0</summary>
            /// <value>買掛対象|0</value>
            public int? PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 支払対象|0</summary>
            /// <value>支払対象|0</value>
            public int? PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ|0</summary>
            /// <value>買掛更新フラグ|0</value>
            public int? PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 支払更新フラグ|0</summary>
            /// <value>支払更新フラグ|0</value>
            public int? PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 摘要名</summary>
            /// <value>摘要名</value>
            public string Summary { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 仮単価FLG|各種名称マスタ</summary>
            /// <value>仮単価FLG|各種名称マスタ</value>
            public int? TmpUnitpriceFlg { get; set; }
            /// <summary>Gets or sets 検査方法|各種名称マスタ</summary>
            /// <value>検査方法|各種名称マスタ</value>
            public int? InspectMethod { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal? TaxRatio { get; set; }
            /// <summary>Gets or sets 仕入伝票発行済み区分|0</summary>
            /// <value>仕入伝票発行済み区分|0</value>
            public int? SlipIssueDivision { get; set; }
            /// <summary>Gets or sets 仕入伝票発行日</summary>
            /// <value>仕入伝票発行日</value>
            public DateTime? SlipIssueDate { get; set; }
            /// <summary>Gets or sets 承認ステータス|各種名称マスタ</summary>
            /// <value>承認ステータス|各種名称マスタ</value>
            public int? ApprovalStatus { get; set; }
            /// <summary>Gets or sets 承認者</summary>
            /// <value>承認者</value>
            public string ApprovedBy { get; set; }
            /// <summary>Gets or sets 承認日</summary>
            /// <value>承認日</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? ExRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? ExValidDate { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 検査判定フラグ|各種名称マスタ</summary>
            /// <value>検査判定フラグ|各種名称マスタ</value>
            public int? CertificationFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 購買NO</summary>
                /// <value>購買NO</value>
                public string PurchaseNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="purchaseNo">購買NO</param>
                public PrimaryKey(string purchaseNo)
                {
                    PurchaseNo = purchaseNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PurchaseNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="purchaseNo">購買NO</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TmppayPurchaseSubcontractEntity GetEntity(string purchaseNo, ComDB db)
            {
                TmppayPurchaseSubcontractEntity.PrimaryKey condition = new TmppayPurchaseSubcontractEntity.PrimaryKey(purchaseNo);
                return GetEntity<TmppayPurchaseSubcontractEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 移庫指図
        /// </summary>
        public class TransferEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TransferEntity()
            {
                TableName = "transfer";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 移庫指図番号</summary>
            /// <value>移庫指図番号</value>
            public string TransferNo { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int RowNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets 区分</summary>
            /// <value>区分</value>
            public string Division { get; set; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets ステータス|各種名称マスタ</summary>
            /// <value>ステータス|各種名称マスタ</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 移庫元ロケーション</summary>
            /// <value>移庫元ロケーション</value>
            public string FromLocationCd { get; set; }
            /// <summary>Gets or sets 移庫先ロケーション</summary>
            /// <value>移庫先ロケーション</value>
            public string ToLocationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>サブロット番号1</value>
            public string SubLotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>サブロット番号2</value>
            public string SubLotNo2 { get; set; }
            /// <summary>Gets or sets 移庫指図日</summary>
            /// <value>移庫指図日</value>
            public DateTime? OrderDate { get; set; }
            /// <summary>Gets or sets 移庫実績日</summary>
            /// <value>移庫実績日</value>
            public DateTime? ResultDate { get; set; }
            /// <summary>Gets or sets 移庫完了日</summary>
            /// <value>移庫完了日</value>
            public DateTime? FinishDate { get; set; }
            /// <summary>Gets or sets 移庫指図数量</summary>
            /// <value>移庫指図数量</value>
            public decimal? OrderQty { get; set; }
            /// <summary>Gets or sets 移庫実績数量</summary>
            /// <value>移庫実績数量</value>
            public decimal? ResultQty { get; set; }
            /// <summary>Gets or sets 理由コード</summary>
            /// <value>理由コード</value>
            public string RyCd { get; set; }
            /// <summary>Gets or sets 理由</summary>
            /// <value>理由</value>
            public string Reason { get; set; }
            /// <summary>Gets or sets 運送会社コード</summary>
            /// <value>運送会社コード</value>
            public string CarryCd { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 払出指示書発行日（初回）</summary>
            /// <value>払出指示書発行日（初回）</value>
            public DateTime? InstructionsIssueDate { get; set; }
            /// <summary>Gets or sets 払出指示書発行日（最終）</summary>
            /// <value>払出指示書発行日（最終）</value>
            public DateTime? LastInstructionsIssueDate { get; set; }
            /// <summary>Gets or sets 払出指示書発行者</summary>
            /// <value>払出指示書発行者</value>
            public string InstructionsIssueUser { get; set; }
            /// <summary>Gets or sets 払出指示書発行ステータス</summary>
            /// <value>払出指示書発行ステータス</value>
            public int? InstructionsIssueStatus { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 移庫指図番号</summary>
                /// <value>移庫指図番号</value>
                public string TransferNo { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="transferNo">移庫指図番号</param>
                /// <param name="rowNo">行番号</param>
                public PrimaryKey(string transferNo, int rowNo)
                {
                    TransferNo = transferNo;
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.TransferNo, this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="transferNo">移庫指図番号</param>
            /// <param name="rowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TransferEntity GetEntity(string transferNo, int rowNo, ComDB db)
            {
                TransferEntity.PrimaryKey condition = new TransferEntity.PrimaryKey(transferNo, rowNo);
                return GetEntity<TransferEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 取引先別単価マスタ
        /// </summary>
        public class UnitpriceEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public UnitpriceEntity()
            {
                TableName = "unitprice";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 帳合コード/取引先コード</summary>
            /// <value>帳合コード/取引先コード</value>
            public string BalanceCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public long ConsecutiveNo { get; set; }
            /// <summary>Gets or sets バージョン</summary>
            /// <value>バージョン</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
            /// <value>取引先区分|各種名称マスタ</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 単価区分|各種名称マスタ</summary>
            /// <value>単価区分|各種名称マスタ</value>
            public int? UnitpriceDivision { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime ValidDate { get; set; }
            /// <summary>Gets or sets 数量(FROM)</summary>
            /// <value>数量(FROM)</value>
            public decimal QuantityFrom { get; set; }
            /// <summary>Gets or sets 数量(TO)</summary>
            /// <value>数量(TO)</value>
            public decimal QuantityTo { get; set; }
            /// <summary>Gets or sets 単価</summary>
            /// <value>単価</value>
            public decimal Unitprice { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remarks { get; set; }
            /// <summary>Gets or sets 承認依頼者ID</summary>
            /// <value>承認依頼者ID</value>
            public string ApprovalRequestUserId { get; set; }
            /// <summary>Gets or sets 承認依頼日時</summary>
            /// <value>承認依頼日時</value>
            public DateTime? ApprovalRequestDate { get; set; }
            /// <summary>Gets or sets 承認者ID</summary>
            /// <value>承認者ID</value>
            public string ApprovalUserId { get; set; }
            /// <summary>Gets or sets 承認日時</summary>
            /// <value>承認日時</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets 承認ステータス|各種名称マスタ</summary>
            /// <value>承認ステータス|各種名称マスタ</value>
            public int? ApprovalStatus { get; set; }
            /// <summary>Gets or sets 承認依頼先担当者ID</summary>
            /// <value>承認依頼先担当者ID</value>
            public string ReceiveUserId { get; set; }
            /// <summary>Gets or sets メール送信ステータス(承認依頼)|0</summary>
            /// <value>メール送信ステータス(承認依頼)|0</value>
            public int? SendMailApprStatus { get; set; }
            /// <summary>Gets or sets メール送信ステータス(否認)|0</summary>
            /// <value>メール送信ステータス(否認)|0</value>
            public int? SendMailDisalwStatus { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 帳合コード/取引先コード</summary>
                /// <value>帳合コード/取引先コード</value>
                public string BalanceCd { get; set; }
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public long ConsecutiveNo { get; set; }
                /// <summary>Gets or sets バージョン</summary>
                /// <value>バージョン</value>
                public int SeqNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="balanceCd">帳合コード/取引先コード</param>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                /// <param name="consecutiveNo">行番号</param>
                /// <param name="seqNo">バージョン</param>
                public PrimaryKey(string balanceCd, string itemCd, string specificationCd, long consecutiveNo, int seqNo)
                {
                    BalanceCd = balanceCd;
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                    ConsecutiveNo = consecutiveNo;
                    SeqNo = seqNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.BalanceCd, this.ItemCd, this.SpecificationCd, this.ConsecutiveNo, this.SeqNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="balanceCd">帳合コード/取引先コード</param>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="consecutiveNo">行番号</param>
            /// <param name="seqNo">バージョン</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public UnitpriceEntity GetEntity(string balanceCd, string itemCd, string specificationCd, long consecutiveNo, int seqNo, ComDB db)
            {
                UnitpriceEntity.PrimaryKey condition = new UnitpriceEntity.PrimaryKey(balanceCd, itemCd, specificationCd, consecutiveNo, seqNo);
                return GetEntity<UnitpriceEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 汎用マスタ
        /// </summary>
        public class UtilityEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public UtilityEntity()
            {
                TableName = "utility";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 区分</summary>
            /// <value>区分</value>
            public string UtilityDivision { get; set; }
            /// <summary>Gets or sets コード</summary>
            /// <value>コード</value>
            public string UtilityCd { get; set; }
            /// <summary>Gets or sets 番号</summary>
            /// <value>番号</value>
            public string UtilityNo { get; set; }
            /// <summary>Gets or sets 名称1</summary>
            /// <value>名称1</value>
            public string Name01 { get; set; }
            /// <summary>Gets or sets 名称2</summary>
            /// <value>名称2</value>
            public string Name02 { get; set; }
            /// <summary>Gets or sets 名称3</summary>
            /// <value>名称3</value>
            public string Name03 { get; set; }
            /// <summary>Gets or sets 数値1</summary>
            /// <value>数値1</value>
            public decimal? Num01 { get; set; }
            /// <summary>Gets or sets 数値2</summary>
            /// <value>数値2</value>
            public decimal? Num02 { get; set; }
            /// <summary>Gets or sets 数値3</summary>
            /// <value>数値3</value>
            public decimal? Num03 { get; set; }
            /// <summary>Gets or sets 数量端数単位|1</summary>
            /// <value>数量端数単位|1</value>
            public int? QuantityRoundup01 { get; set; }
            /// <summary>Gets or sets 数量端数区分|1</summary>
            /// <value>数量端数区分|1</value>
            public int? QuantityRoundupUnit01 { get; set; }
            /// <summary>Gets or sets 数量端数単位|2</summary>
            /// <value>数量端数単位|2</value>
            public int? QuantityRoundup02 { get; set; }
            /// <summary>Gets or sets 数量端数区分|2</summary>
            /// <value>数量端数区分|2</value>
            public int? QuantityRoundupUnit02 { get; set; }
            /// <summary>Gets or sets 数量端数単位|3</summary>
            /// <value>数量端数単位|3</value>
            public int? QuantityRoundup03 { get; set; }
            /// <summary>Gets or sets 数量端数区分|3</summary>
            /// <value>数量端数区分|3</value>
            public int? QuantityRoundupUnit03 { get; set; }
            /// <summary>Gets or sets メモ欄</summary>
            /// <value>メモ欄</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets 説明</summary>
            /// <value>説明</value>
            public string Discription { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 区分</summary>
                /// <value>区分</value>
                public string UtilityDivision { get; set; }
                /// <summary>Gets or sets コード</summary>
                /// <value>コード</value>
                public string UtilityCd { get; set; }
                /// <summary>Gets or sets 番号</summary>
                /// <value>番号</value>
                public string UtilityNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="utilityDivision">区分</param>
                /// <param name="utilityCd">コード</param>
                /// <param name="utilityNo">番号</param>
                public PrimaryKey(string utilityDivision, string utilityCd, string utilityNo)
                {
                    UtilityDivision = utilityDivision;
                    UtilityCd = utilityCd;
                    UtilityNo = utilityNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.UtilityDivision, this.UtilityCd, this.UtilityNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="utilityDivision">区分</param>
            /// <param name="utilityCd">コード</param>
            /// <param name="utilityNo">番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public UtilityEntity GetEntity(string utilityDivision, string utilityCd, string utilityNo, ComDB db)
            {
                UtilityEntity.PrimaryKey condition = new UtilityEntity.PrimaryKey(utilityDivision, utilityCd, utilityNo);
                return GetEntity<UtilityEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 取引先マスタ
        /// </summary>
        public class VenderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public VenderEntity()
            {
                TableName = "vender";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
            /// <value>取引先区分|各種名称マスタ</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 取引先名称1</summary>
            /// <value>取引先名称1</value>
            public string VenderName1 { get; set; }
            /// <summary>Gets or sets 取引先名称2</summary>
            /// <value>取引先名称2</value>
            public string VenderName2 { get; set; }
            /// <summary>Gets or sets 取引先略称</summary>
            /// <value>取引先略称</value>
            public string VenderShortedName { get; set; }
            /// <summary>Gets or sets 取引先名称かな</summary>
            /// <value>取引先名称かな</value>
            public string VenderNameKana { get; set; }
            /// <summary>Gets or sets 支払･請求先コード</summary>
            /// <value>支払･請求先コード</value>
            public string PaymentInvoiceCd { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 外注先区分|各種名称マスタ</summary>
            /// <value>外注先区分|各種名称マスタ</value>
            public int? GiDivision { get; set; }
            /// <summary>Gets or sets 郵便番号</summary>
            /// <value>郵便番号</value>
            public string Zipcode { get; set; }
            /// <summary>Gets or sets 住所1</summary>
            /// <value>住所1</value>
            public string Address1 { get; set; }
            /// <summary>Gets or sets 住所2</summary>
            /// <value>住所2</value>
            public string Address2 { get; set; }
            /// <summary>Gets or sets 住所3</summary>
            /// <value>住所3</value>
            public string Address3 { get; set; }
            /// <summary>Gets or sets 電話番号</summary>
            /// <value>電話番号</value>
            public string TelNo { get; set; }
            /// <summary>Gets or sets FAX番号</summary>
            /// <value>FAX番号</value>
            public string FaxNo { get; set; }
            /// <summary>Gets or sets mailアドレス</summary>
            /// <value>mailアドレス</value>
            public string Mail { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 取引先担当者名</summary>
            /// <value>取引先担当者名</value>
            public string VenderUserName { get; set; }
            /// <summary>Gets or sets 代表者役職</summary>
            /// <value>代表者役職</value>
            public string RepresentRole { get; set; }
            /// <summary>Gets or sets 代表者名</summary>
            /// <value>代表者名</value>
            public string RepresentPerson { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 締日</summary>
            /// <value>締日</value>
            public int ClosingDate { get; set; }
            /// <summary>Gets or sets 手形サイト単位|各種名称マスタ</summary>
            /// <value>手形サイト単位|各種名称マスタ</value>
            public int NoteSightDivision { get; set; }
            /// <summary>Gets or sets 下請法該当|各種名称マスタ</summary>
            /// <value>下請法該当|各種名称マスタ</value>
            public int? SubcontractLaw { get; set; }
            /// <summary>Gets or sets 前受金区分|各種名称マスタ</summary>
            /// <value>前受金区分|各種名称マスタ</value>
            public int? AdvanceDivision { get; set; }
            /// <summary>Gets or sets 会計部門コード</summary>
            /// <value>会計部門コード</value>
            public string SectionCd { get; set; }
            /// <summary>Gets or sets 勘定科目コード</summary>
            /// <value>勘定科目コード</value>
            public string AccountsCd { get; set; }
            /// <summary>Gets or sets 請求書発行区分|各種名称マスタ</summary>
            /// <value>請求書発行区分|各種名称マスタ</value>
            public int? BillPublish { get; set; }
            /// <summary>Gets or sets 伝票発行区分|各種名称マスタ</summary>
            /// <value>伝票発行区分|各種名称マスタ</value>
            public int? SlipPublish { get; set; }
            /// <summary>Gets or sets 休日指定フラグ|各種名称マスタ</summary>
            /// <value>休日指定フラグ|各種名称マスタ</value>
            public int? HolidayFlg { get; set; }
            /// <summary>Gets or sets カレンダーコード</summary>
            /// <value>カレンダーコード</value>
            public string CalendarCd { get; set; }
            /// <summary>Gets or sets 銀行コード</summary>
            /// <value>銀行コード</value>
            public string BankCd { get; set; }
            /// <summary>Gets or sets 支店コード</summary>
            /// <value>支店コード</value>
            public string BranchCd { get; set; }
            /// <summary>Gets or sets 預金種別</summary>
            /// <value>預金種別</value>
            public string DepositType { get; set; }
            /// <summary>Gets or sets 預金勘定</summary>
            /// <value>預金勘定</value>
            public string DepositAccount { get; set; }
            /// <summary>Gets or sets 口座区分|各種名称マスタ</summary>
            /// <value>口座区分|各種名称マスタ</value>
            public int? AccountDivision { get; set; }
            /// <summary>Gets or sets 口座番号</summary>
            /// <value>口座番号</value>
            public string AccountNo { get; set; }
            /// <summary>Gets or sets 口座名義人(半角カナ)</summary>
            /// <value>口座名義人(半角カナ)</value>
            public string AccountName { get; set; }
            /// <summary>Gets or sets 相手銀行コード</summary>
            /// <value>相手銀行コード</value>
            public string OtherBankCd { get; set; }
            /// <summary>Gets or sets 相手支店コード</summary>
            /// <value>相手支店コード</value>
            public string OtherBranchCd { get; set; }
            /// <summary>Gets or sets 相手預金種別</summary>
            /// <value>相手預金種別</value>
            public string OtherDepositType { get; set; }
            /// <summary>Gets or sets 相手預金勘定</summary>
            /// <value>相手預金勘定</value>
            public string OtherDepositAccount { get; set; }
            /// <summary>Gets or sets 相手口座区分|各種名称マスタ</summary>
            /// <value>相手口座区分|各種名称マスタ</value>
            public int? OtherAccountDivision { get; set; }
            /// <summary>Gets or sets 相手口座番号</summary>
            /// <value>相手口座番号</value>
            public string OtherAccountNo { get; set; }
            /// <summary>Gets or sets 相手口座名義人(半角カナ)</summary>
            /// <value>相手口座名義人(半角カナ)</value>
            public string OtherAccountName { get; set; }
            /// <summary>Gets or sets 消費税課税区分|各種名称マスタ</summary>
            /// <value>消費税課税区分|各種名称マスタ</value>
            public int? TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税算出区分|各種名称マスタ</summary>
            /// <value>消費税算出区分|各種名称マスタ</value>
            public int? CalcDivision { get; set; }
            /// <summary>Gets or sets 消費税端数区分|各種名称マスタ</summary>
            /// <value>消費税端数区分|各種名称マスタ</value>
            public int? TaxRoundup { get; set; }
            /// <summary>Gets or sets 消費税端数単位|各種名称マスタ</summary>
            /// <value>消費税端数単位|各種名称マスタ</value>
            public int? TaxRoundupUnit { get; set; }
            /// <summary>Gets or sets 端数処理区分|各種名称マスタ</summary>
            /// <value>端数処理区分|各種名称マスタ</value>
            public int? Roundup { get; set; }
            /// <summary>Gets or sets 端数処理単位|各種名称マスタ</summary>
            /// <value>端数処理単位|各種名称マスタ</value>
            public int? RoundupUnit { get; set; }
            /// <summary>Gets or sets 売上仕入金額端数処理|各種名称マスタ</summary>
            /// <value>売上仕入金額端数処理|各種名称マスタ</value>
            public int? SalesPurchaseRoundup { get; set; }
            /// <summary>Gets or sets 売上仕入金額端数単位|各種名称マスタ</summary>
            /// <value>売上仕入金額端数単位|各種名称マスタ</value>
            public int? SalesPurchaseRoundupUnit { get; set; }
            /// <summary>Gets or sets 単価端数処理|各種名称マスタ</summary>
            /// <value>単価端数処理|各種名称マスタ</value>
            public int? UnitpriceRoundup { get; set; }
            /// <summary>Gets or sets 単価端数単位|各種名称マスタ</summary>
            /// <value>単価端数単位|各種名称マスタ</value>
            public int? UnitpriceRoundupUnit { get; set; }
            /// <summary>Gets or sets 客先担当者1</summary>
            /// <value>客先担当者1</value>
            public string CustomerUserName1 { get; set; }
            /// <summary>Gets or sets 客先担当者2</summary>
            /// <value>客先担当者2</value>
            public string CustomerUserName2 { get; set; }
            /// <summary>Gets or sets 客先所感1</summary>
            /// <value>客先所感1</value>
            public string CustomerImpression1 { get; set; }
            /// <summary>Gets or sets 客先所感2</summary>
            /// <value>客先所感2</value>
            public string CustomerImpression2 { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 有効フラグ</summary>
            /// <value>有効フラグ</value>
            public int ActivateFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
                /// <value>取引先区分|各種名称マスタ</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 取引先コード</summary>
                /// <value>取引先コード</value>
                public string VenderCd { get; set; }
                /// <summary>Gets or sets 開始有効日</summary>
                /// <value>開始有効日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="venderCd">取引先コード</param>
                /// <param name="activeDate">開始有効日</param>
                public PrimaryKey(string venderDivision, string venderCd, DateTime activeDate)
                {
                    VenderDivision = venderDivision;
                    VenderCd = venderCd;
                    ActiveDate = activeDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.VenderDivision, this.VenderCd, this.ActiveDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="activeDate">開始有効日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public VenderEntity GetEntity(string venderDivision, string venderCd, DateTime activeDate, ComDB db)
            {
                VenderEntity.PrimaryKey condition = new VenderEntity.PrimaryKey(venderDivision, venderCd, activeDate);
                return GetEntity<VenderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 取引先マスタ_入金&支払条件
        /// </summary>
        public class VenderCreditDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public VenderCreditDetailEntity()
            {
                TableName = "vender_credit_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
            /// <value>取引先区分|各種名称マスタ</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets 開始有効日</summary>
            /// <value>開始有効日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 入金･支払月区分|各種名称マスタ</summary>
            /// <value>入金･支払月区分|各種名称マスタ</value>
            public int? CreditMonthDivision { get; set; }
            /// <summary>Gets or sets 手形サイト(日単位)</summary>
            /// <value>手形サイト(日単位)</value>
            public int? NoteSight { get; set; }
            /// <summary>Gets or sets 手形サイト(月単位)</summary>
            /// <value>手形サイト(月単位)</value>
            public int? NoteSightMonth { get; set; }
            /// <summary>Gets or sets 境界額</summary>
            /// <value>境界額</value>
            public decimal? BoundAmount { get; set; }
            /// <summary>Gets or sets 入金･支払区分|分類マスタ</summary>
            /// <value>入金･支払区分|分類マスタ</value>
            public int? CreditDivision { get; set; }
            /// <summary>Gets or sets 入金･支払予定日</summary>
            /// <value>入金･支払予定日</value>
            public int? CreditScheduledDate { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int? DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
                /// <value>取引先区分|各種名称マスタ</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 取引先コード</summary>
                /// <value>取引先コード</value>
                public string VenderCd { get; set; }
                /// <summary>Gets or sets 表示順</summary>
                /// <value>表示順</value>
                public int Seq { get; set; }
                /// <summary>Gets or sets 開始有効日</summary>
                /// <value>開始有効日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="venderCd">取引先コード</param>
                /// <param name="seq">表示順</param>
                /// <param name="activeDate">開始有効日</param>
                public PrimaryKey(string venderDivision, string venderCd, int seq, DateTime activeDate)
                {
                    VenderDivision = venderDivision;
                    VenderCd = venderCd;
                    Seq = seq;
                    ActiveDate = activeDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.VenderDivision, this.VenderCd, this.Seq, this.ActiveDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="seq">表示順</param>
            /// <param name="activeDate">開始有効日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public VenderCreditDetailEntity GetEntity(string venderDivision, string venderCd, int seq, DateTime activeDate, ComDB db)
            {
                VenderCreditDetailEntity.PrimaryKey condition = new VenderCreditDetailEntity.PrimaryKey(venderDivision, venderCd, seq, activeDate);
                return GetEntity<VenderCreditDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 取引先マスタ_内部パラメータ用
        /// </summary>
        public class VenderSubEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public VenderSubEntity()
            {
                TableName = "vender_sub";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
            /// <value>取引先区分|各種名称マスタ</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード 6桁</summary>
            /// <value>取引先コード 6桁</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先コード(AIR_OPEN)</summary>
            /// <value>取引先コード(AIR_OPEN)</value>
            public string VenderCdAir { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
                /// <value>取引先区分|各種名称マスタ</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 取引先コード(AIR_OPEN)</summary>
                /// <value>取引先コード(AIR_OPEN)</value>
                public string VenderCdAir { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="venderCdAir">取引先コード(AIR_OPEN)</param>
                public PrimaryKey(string venderDivision, string venderCdAir)
                {
                    VenderDivision = venderDivision;
                    VenderCdAir = venderCdAir;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.VenderDivision, this.VenderCdAir);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCdAir">取引先コード(AIR_OPEN)</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public VenderSubEntity GetEntity(string venderDivision, string venderCdAir, ComDB db)
            {
                VenderSubEntity.PrimaryKey condition = new VenderSubEntity.PrimaryKey(venderDivision, venderCdAir);
                return GetEntity<VenderSubEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 取引先マスタ_有効化対象ログ
        /// </summary>
        public class VenderTempLogEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public VenderTempLogEntity()
            {
                TableName = "vender_temp_log";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 取引先区分|各種名称マスタ</summary>
            /// <value>取引先区分|各種名称マスタ</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int? SeqNo { get; set; }
        }

        /// <summary>
        /// 権限マスタ_閲覧権限
        /// </summary>
        public class ViewAuthorityEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ViewAuthorityEntity()
            {
                TableName = "view_authority";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets メニューID</summary>
            /// <value>メニューID</value>
            public long MenuId { get; set; }
            /// <summary>Gets or sets タブID</summary>
            /// <value>タブID</value>
            public long TabId { get; set; }
            /// <summary>Gets or sets ロールID</summary>
            /// <value>ロールID</value>
            public int RoleId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets メニューID</summary>
                /// <value>メニューID</value>
                public long MenuId { get; set; }
                /// <summary>Gets or sets タブID</summary>
                /// <value>タブID</value>
                public long TabId { get; set; }
                /// <summary>Gets or sets ロールID</summary>
                /// <value>ロールID</value>
                public int RoleId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="menuId">メニューID</param>
                /// <param name="tabId">タブID</param>
                /// <param name="roleId">ロールID</param>
                public PrimaryKey(long menuId, long tabId, int roleId)
                {
                    MenuId = menuId;
                    TabId = tabId;
                    RoleId = roleId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MenuId, this.TabId, this.RoleId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="menuId">メニューID</param>
            /// <param name="tabId">タブID</param>
            /// <param name="roleId">ロールID</param>]
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ViewAuthorityEntity GetEntity(long menuId, long tabId, int roleId, ComDB db)
            {
                ViewAuthorityEntity.PrimaryKey condition = new ViewAuthorityEntity.PrimaryKey(menuId, tabId, roleId);
                return GetEntity<ViewAuthorityEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ワークフロー詳細
        /// </summary>
        public class WorkflowDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public WorkflowDetailEntity()
            {
                TableName = "workflow_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ワークフローNo</summary>
            /// <value>ワークフローNo</value>
            public string WfNo { get; set; }
            /// <summary>Gets or sets 順序</summary>
            /// <value>順序</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets 承認ユーザーコード</summary>
            /// <value>承認ユーザーコード</value>
            public string ApprovalUserId { get; set; }
            /// <summary>Gets or sets 全承認フラグ</summary>
            /// <value>全承認フラグ</value>
            public int? AllApprovalFlg { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 承認日</summary>
            /// <value>承認日</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets コメント</summary>
            /// <value>コメント</value>
            public string Comments { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ワークフローNo</summary>
                /// <value>ワークフローNo</value>
                public string WfNo { get; set; }
                /// <summary>Gets or sets 順序</summary>
                /// <value>順序</value>
                public int Seq { get; set; }
                /// <summary>Gets or sets 承認ユーザーコード</summary>
                /// <value>承認ユーザーコード</value>
                public string ApprovalUserId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="workflowNo">ワークフローNo</param>
                /// <param name="seq">順序</param>
                /// <param name="approvalUserId">承認ユーザーコード</param>
                public PrimaryKey(string workflowNo, int seq, string approvalUserId)
                {
                    WfNo = workflowNo;
                    Seq = seq;
                    ApprovalUserId = approvalUserId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.WfNo, this.Seq, this.ApprovalUserId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="workflowNo">ワークフローNo</param>
            /// <param name="seq">順序</param>
            /// <param name="approvalUserId">承認ユーザーコード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public WorkflowDetailEntity GetEntity(string workflowNo, int seq, string approvalUserId, ComDB db)
            {
                WorkflowDetailEntity.PrimaryKey condition = new WorkflowDetailEntity.PrimaryKey(workflowNo, seq, approvalUserId);
                return GetEntity<WorkflowDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ワークフローヘッダ
        /// </summary>
        public class WorkflowHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public WorkflowHeaderEntity()
            {
                TableName = "workflow_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ワークフローNo</summary>
            /// <value>ワークフローNo</value>
            public string WfNo { get; set; }
            /// <summary>Gets or sets テンプレートNo</summary>
            /// <value>テンプレートNo</value>
            public string WfTemplateNo { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime? ActiveDate { get; set; }
            /// <summary>Gets or sets ワークフロー名称</summary>
            /// <value>ワークフロー名称</value>
            public string WfName { get; set; }
            /// <summary>Gets or sets 呼出元区分</summary>
            /// <value>呼出元区分</value>
            public string WfDivision { get; set; }
            /// <summary>Gets or sets 登録元伝票番号</summary>
            /// <value>登録元伝票番号</value>
            public string SlipNo { get; set; }
            /// <summary>Gets or sets 登録元伝票番号枝番1</summary>
            /// <value>登録元伝票番号枝番1</value>
            public string SlipBranchNo1 { get; set; }
            /// <summary>Gets or sets 登録元伝票番号枝番2</summary>
            /// <value>登録元伝票番号枝番2</value>
            public string SlipBranchNo2 { get; set; }
            /// <summary>Gets or sets 依頼元ユーザー</summary>
            /// <value>依頼元ユーザー</value>
            public string RequestUserId { get; set; }
            /// <summary>Gets or sets 通知区分</summary>
            /// <value>通知区分</value>
            public int? NoticeDivision { get; set; }
            /// <summary>Gets or sets 依頼元コメント</summary>
            /// <value>依頼元コメント</value>
            public string RequestComments { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int? Status { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ワークフローNo</summary>
                /// <value>ワークフローNo</value>
                public string WfNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="workflowNo">ワークフローNo</param>
                public PrimaryKey(string workflowNo)
                {
                    WfNo = workflowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.WfNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="workflowNo">ワークフローNo</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public WorkflowHeaderEntity GetEntity(string workflowNo, ComDB db)
            {
                WorkflowHeaderEntity.PrimaryKey condition = new WorkflowHeaderEntity.PrimaryKey(workflowNo);
                return GetEntity<WorkflowHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ワークフロー操作ログ
        /// </summary>
        public class WorkflowLogEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public WorkflowLogEntity()
            {
                TableName = "workflow_log";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ワークフローNo</summary>
            /// <value>ワークフローNo</value>
            public string WfNo { get; set; }
            /// <summary>Gets or sets ログ連番</summary>
            /// <value>ログ連番</value>
            public int LogSeq { get; set; }
            /// <summary>Gets or sets ユーザーコード</summary>
            /// <value>ユーザーコード</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int? Operation { get; set; }
            /// <summary>Gets or sets 操作時刻</summary>
            /// <value>操作時刻</value>
            public DateTime? OperationDate { get; set; }
            /// <summary>Gets or sets コメント</summary>
            /// <value>コメント</value>
            public string Comments { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ワークフローNo</summary>
                /// <value>ワークフローNo</value>
                public string WfNo { get; set; }
                /// <summary>Gets or sets ログ連番</summary>
                /// <value>ログ連番</value>
                public int LogSeq { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="workflowNo">ワークフローNo</param>
                /// <param name="logSeq">ログ連番</param>
                public PrimaryKey(string workflowNo, int logSeq)
                {
                    WfNo = workflowNo;
                    LogSeq = logSeq;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.WfNo, this.LogSeq);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="workflowNo">ワークフローNo</param>
            /// <param name="logSeq">ログ連番</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public WorkflowLogEntity GetEntity(string workflowNo, int logSeq, ComDB db)
            {
                WorkflowLogEntity.PrimaryKey condition = new WorkflowLogEntity.PrimaryKey(workflowNo, logSeq);
                return GetEntity<WorkflowLogEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ワークフローテンプレート詳細
        /// </summary>
        public class WorkflowTemplateDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public WorkflowTemplateDetailEntity()
            {
                TableName = "workflow_template_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ワークフローテンプレートNo</summary>
            /// <value>ワークフローテンプレートNo</value>
            public int WfTemplateNo { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 順序</summary>
            /// <value>順序</value>
            public int Seq { get; set; }
            /// <summary>Gets or sets 承認ユーザーコード</summary>
            /// <value>承認ユーザーコード</value>
            public string ApprovalUserId { get; set; }
            /// <summary>Gets or sets 全承認フラグ</summary>
            /// <value>全承認フラグ</value>
            public int? AllApprovalFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ワークフローテンプレートNo</summary>
                /// <value>ワークフローテンプレートNo</value>
                public int WfTemplateNo { get; set; }
                /// <summary>Gets or sets 有効開始日</summary>
                /// <value>有効開始日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>Gets or sets 順序</summary>
                /// <value>順序</value>
                public int Seq { get; set; }
                /// <summary>Gets or sets 承認ユーザーコード</summary>
                /// <value>承認ユーザーコード</value>
                public string ApprovalUserId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="workflowTemplateNo">ワークフローテンプレートNo</param>
                /// <param name="activeDate">有効開始日</param>
                /// <param name="seq">順序</param>
                /// <param name="approvalUserId">承認ユーザーコード</param>
                public PrimaryKey(int workflowTemplateNo, DateTime activeDate, int seq, string approvalUserId)
                {
                    WfTemplateNo = workflowTemplateNo;
                    ActiveDate = activeDate;
                    Seq = seq;
                    ApprovalUserId = approvalUserId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.WfTemplateNo, this.ActiveDate, this.Seq, this.ApprovalUserId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="workflowTemplateNo">ワークフローテンプレートNo</param>
            /// <param name="activeDate">有効開始日</param>
            /// <param name="seq">順序</param>
            /// <param name="approvalUserId">承認ユーザーコード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public WorkflowTemplateDetailEntity GetEntity(int workflowTemplateNo, DateTime activeDate, int seq, string approvalUserId, ComDB db)
            {
                WorkflowTemplateDetailEntity.PrimaryKey condition = new WorkflowTemplateDetailEntity.PrimaryKey(workflowTemplateNo, activeDate, seq, approvalUserId);
                return GetEntity<WorkflowTemplateDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// ワークフローテンプレートヘッダ
        /// </summary>
        public class WorkflowTemplateHeaderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public WorkflowTemplateHeaderEntity()
            {
                TableName = "workflow_template_header";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ワークフローテンプレートNo</summary>
            /// <value>ワークフローテンプレートNo</value>
            public int WfTemplateNo { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets ワークフロー名称</summary>
            /// <value>ワークフロー名称</value>
            public string WfName { get; set; }
            /// <summary>Gets or sets 呼出元区分</summary>
            /// <value>呼出元区分</value>
            public string WfDivision { get; set; }
            /// <summary>Gets or sets 通知区分</summary>
            /// <value>通知区分</value>
            public int? NoticeDivision { get; set; }
            /// <summary>Gets or sets 承認依頼プッシュ通知</summary>
            /// <value>承認依頼プッシュ通知</value>
            public int? RequestPushFlg { get; set; }
            /// <summary>Gets or sets 承認依頼メール通知</summary>
            /// <value>承認依頼メール通知</value>
            public int? RequestMailFlg { get; set; }
            /// <summary>Gets or sets 承認依頼取消プッシュ通知</summary>
            /// <value>承認依頼取消プッシュ通知</value>
            public int? RequestCancelPushFlg { get; set; }
            /// <summary>Gets or sets 承認依頼取消メール通知</summary>
            /// <value>承認依頼取消メール通知</value>
            public int? RequestCancelMailFlg { get; set; }
            /// <summary>Gets or sets 承認プッシュ通知</summary>
            /// <value>承認プッシュ通知</value>
            public int? ApprovalPushFlg { get; set; }
            /// <summary>Gets or sets 承認メール通知</summary>
            /// <value>承認メール通知</value>
            public int? ApprovalMailFlg { get; set; }
            /// <summary>Gets or sets 承認取消プッシュ通知</summary>
            /// <value>承認取消プッシュ通知</value>
            public int? ApprovalCancelPushFlg { get; set; }
            /// <summary>Gets or sets 承認取消通知</summary>
            /// <value>承認取消通知</value>
            public int? ApprovalCancelMailFlg { get; set; }
            /// <summary>Gets or sets 否認プッシュ通知</summary>
            /// <value>否認プッシュ通知</value>
            public int? RejectPushFlg { get; set; }
            /// <summary>Gets or sets 否認メール通知</summary>
            /// <value>否認メール通知</value>
            public int? RejectMailFlg { get; set; }
            /// <summary>Gets or sets 承認完了プッシュ通知</summary>
            /// <value>承認完了プッシュ通知</value>
            public int? CompletePushFlg { get; set; }
            /// <summary>Gets or sets 承認完了メール通知</summary>
            /// <value>承認完了メール通知</value>
            public int? CompleteMailFlg { get; set; }
            /// <summary>Gets or sets 編集可能フラグ</summary>
            /// <value>編集可能フラグ</value>
            public int EditableFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ワークフローテンプレートNo</summary>
                /// <value>ワークフローテンプレートNo</value>
                public int WfTemplateNo { get; set; }
                /// <summary>Gets or sets 有効開始日</summary>
                /// <value>有効開始日</value>
                public DateTime ActiveDate { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="workflowTemplateNo">ワークフローテンプレートNo</param>
                /// <param name="activeDate">有効開始日</param>
                public PrimaryKey(int workflowTemplateNo, DateTime activeDate)
                {
                    WfTemplateNo = workflowTemplateNo;
                    ActiveDate = activeDate;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.WfTemplateNo, this.ActiveDate);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="workflowTemplateNo">ワークフローテンプレートNo</param>
            /// <param name="activeDate">有効開始日</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public WorkflowTemplateHeaderEntity GetEntity(int workflowTemplateNo, DateTime activeDate, ComDB db)
            {
                WorkflowTemplateHeaderEntity.PrimaryKey condition = new WorkflowTemplateHeaderEntity.PrimaryKey(workflowTemplateNo, activeDate);
                return GetEntity<WorkflowTemplateHeaderEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 配車予定
        /// </summary>
        public class DispatchEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DispatchEntity()
            {
                TableName = "dispatch";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 配車番号</summary>
            /// <value>配車番号</value>
            public string DispatchNo { get; set; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets 出荷指図番号</summary>
            /// <value>出荷指図番号</value>
            public string ShippingNo { get; set; }
            /// <summary>Gets or sets 出荷指図行番号</summary>
            /// <value>出荷指図行番号</value>
            public int ShippingRowNo { get; set; }
            /// <summary>Gets or sets 運送会社コード</summary>
            /// <value>運送会社コード</value>
            public string CarryCd { get; set; }
            /// <summary>Gets or sets 運送区分</summary>
            /// <value>運送区分</value>
            public string CarryDivision { get; set; }
            /// <summary>Gets or sets 指示量</summary>
            /// <value>指示量</value>
            public decimal? DispatchQty { get; set; }
            /// <summary>Gets or sets 配車確定フラグ</summary>
            /// <value>配車確定フラグ</value>
            public int? CompDivision { get; set; }
            /// <summary>Gets or sets 出荷実績数</summary>
            /// <value>出荷実績数</value>
            public decimal ShippingResultQuantity { get; set; }
            /// <summary>Gets or sets 出荷実績日</summary>
            /// <value>出荷実績日</value>
            public DateTime? ShippingResultDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 配車番号</summary>
                /// <value>配車番号</value>
                public string DispatchNo { get; set; }
                /// <summary>Gets or sets 連番</summary>
                /// <value>連番</value>
                public int SeqNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="dispatchNo">配車番号</param>
                /// <param name="seqNo">連番</param>
                public PrimaryKey(string dispatchNo, int seqNo)
                {
                    DispatchNo = dispatchNo;
                    SeqNo = seqNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.DispatchNo, this.SeqNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="dispatchNo">配車番号</param>
            /// <param name="seqNo">連番</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public DispatchEntity GetEntity(string dispatchNo, int seqNo, ComDB db)
            {
                DispatchEntity.PrimaryKey condition = new DispatchEntity.PrimaryKey(dispatchNo, seqNo);
                return GetEntity<DispatchEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 生産計画
        /// </summary>
        public class ProductionPlanEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ProductionPlanEntity()
            {
                TableName = "production_plan";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 生産計画番号</summary>
            /// <value>生産計画番号</value>
            public string ProductionPlanNo { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 納期</summary>
            /// <value>納期</value>
            public DateTime DeliverLimit { get; set; }
            /// <summary>Gets or sets 生産計画数量</summary>
            /// <value>生産計画数量</value>
            public decimal ProductionPlanQty { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int Status { get; set; }
            /// <summary>Gets or sets 仕入先コード</summary>
            /// <value>仕入先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 生産年月</summary>
            /// <value>生産年月</value>
            public int? ProductionMonth { get; set; }
            /// <summary>Gets or sets 品目タイプ</summary>
            /// <value>品目タイプ</value>
            public int? ItemType { get; set; }
            /// <summary>Gets or sets MRP展開区分</summary>
            /// <value>MRP展開区分</value>
            public int MrpDeploymentDivision { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 生産計画番号</summary>
                /// <value>生産計画番号</value>
                public string ProductionPlanNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="productionPlanNo">生産計画番号</param>
                public PrimaryKey(string productionPlanNo)
                {
                    ProductionPlanNo = productionPlanNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ProductionPlanNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="productionPlanNo">生産計画番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ProductionPlanEntity GetEntity(string productionPlanNo, ComDB db)
            {
                ProductionPlanEntity.PrimaryKey condition = new ProductionPlanEntity.PrimaryKey(productionPlanNo);
                return GetEntity<ProductionPlanEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// MRP結果
        /// </summary>
        public class MrpResultEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MrpResultEntity()
            {
                TableName = "mrp_result";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 計画番号</summary>
            /// <value>計画番号</value>
            public string PlanNo { get; set; }
            /// <summary>Gets or sets 手続区分</summary>
            /// <value>手続区分</value>
            public int ProcedureDivision { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 納期</summary>
            /// <value>納期</value>
            public DateTime? DeliveryDate { get; set; }
            /// <summary>Gets or sets 計画数量</summary>
            /// <value>計画数量</value>
            public decimal PlanQty { get; set; }
            /// <summary>Gets or sets オーダー発行区分</summary>
            /// <value>オーダー発行区分</value>
            public string OrderPublishDivision { get; set; }
            /// <summary>Gets or sets 生産計画番号</summary>
            /// <value>生産計画番号</value>
            public string ProductionPlanNo { get; set; }
            /// <summary>Gets or sets 計画納期</summary>
            /// <value>計画納期</value>
            public DateTime? Deliverlimit { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets オーダー先コード</summary>
            /// <value>オーダー先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 納入ロケーションコード</summary>
            /// <value>納入ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets 購入依頼日</summary>
            /// <value>購入依頼日</value>
            public DateTime? OrderDate { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }
            /// <summary>Gets or sets ステータス</summary>
            /// <value>ステータス</value>
            public int Status { get; set; }
            /// <summary>Gets or sets 引当数量</summary>
            /// <value>引当数量</value>
            public decimal AllocatedQty { get; set; }
            /// <summary>Gets or sets 期間まるめ区分</summary>
            /// <value>期間まるめ区分</value>
            public int MarumeDivision { get; set; }
            /// <summary>Gets or sets オーダー区分</summary>
            /// <value>オーダー区分</value>
            public int OrderDivision { get; set; }
            /// <summary>Gets or sets オーダー番号</summary>
            /// <value>オーダー番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 発注基準</summary>
            /// <value>発注基準</value>
            public int OrderRule { get; set; }
            /// <summary>Gets or sets 在庫計算対象区分</summary>
            /// <value>在庫計算対象区分</value>
            public int InventoryCalDivision { get; set; }
            /// <summary>Gets or sets 部品展開レベル</summary>
            /// <value>部品展開レベル</value>
            public int BreakdownLevel { get; set; }
            /// <summary>Gets or sets 当初納期</summary>
            /// <value>当初納期</value>
            public DateTime? DeliveryDateBefore { get; set; }
            /// <summary>Gets or sets 当初発注数</summary>
            /// <value>当初発注数</value>
            public decimal PlanQtyBefore { get; set; }
            /// <summary>Gets or sets 親オーダー区分</summary>
            /// <value>親オーダー区分</value>
            public int ParentOrderDivision { get; set; }
            /// <summary>Gets or sets 親オーダー番号</summary>
            /// <value>親オーダー番号</value>
            public string ParentOrderNo { get; set; }
            /// <summary>Gets or sets 親品目コード</summary>
            /// <value>親品目コード</value>
            public string ParentItemCd { get; set; }
            /// <summary>Gets or sets 親仕様コード</summary>
            /// <value>親仕様コード</value>
            public string ParentSpecificationCd { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 計画番号</summary>
                /// <value>計画番号</value>
                public string PlanNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="planNo">計画番号</param>
                public PrimaryKey(string planNo)
                {
                    PlanNo = planNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PlanNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="planNo">計画番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MrpResultEntity GetEntity(string planNo, ComDB db)
            {
                MrpResultEntity.PrimaryKey condition = new MrpResultEntity.PrimaryKey(planNo);
                return GetEntity<MrpResultEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 購入依頼ヘッダ
        /// </summary>
        public class PurchaseRequestHeadEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PurchaseRequestHeadEntity()
            {
                TableName = "purchase_request_head";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 購入依頼番号</summary>
            /// <value>購入依頼番号</value>
            public string PurchaseRequestNo { get; set; }
            /// <summary>Gets or sets 購買ステータス</summary>
            /// <value>購買ステータス</value>
            public int? PurchaseStatus { get; set; }
            /// <summary>Gets or sets 購入依頼日</summary>
            /// <value>購入依頼日</value>
            public DateTime? PurchaseRequestDate { get; set; }
            /// <summary>Gets or sets 発生元区分</summary>
            /// <value>発生元区分</value>
            public int? OriginDivision { get; set; }
            /// <summary>Gets or sets 購入依頼件名</summary>
            /// <value>購入依頼件名</value>
            public string PurchaseSubject { get; set; }
            /// <summary>Gets or sets 管理部署コード</summary>
            /// <value>管理部署コード</value>
            public string PurchaseRequestDepartmentCd { get; set; }
            /// <summary>Gets or sets 購入依頼担当者ID</summary>
            /// <value>購入依頼担当者ID</value>
            public string PurchaseRequestChargeCd { get; set; }
            /// <summary>Gets or sets 購入依頼備考</summary>
            /// <value>購入依頼備考</value>
            public string BuySubcontractOrderRemark { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目01（未使用）</summary>
            /// <value>拡張用文字列項目01（未使用）</value>
            public string ExtendString01 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目02（未使用）</summary>
            /// <value>拡張用文字列項目02（未使用）</value>
            public string ExtendString02 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目03（未使用）</summary>
            /// <value>拡張用文字列項目03（未使用）</value>
            public string ExtendString03 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目04（未使用）</summary>
            /// <value>拡張用文字列項目04（未使用）</value>
            public string ExtendString04 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目05（未使用）</summary>
            /// <value>拡張用文字列項目05（未使用）</value>
            public string ExtendString05 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目01（未使用）</summary>
            /// <value>拡張用数値項目01（未使用）</value>
            public decimal? ExtendNumeric01 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目02（未使用）</summary>
            /// <value>拡張用数値項目02（未使用）</value>
            public decimal? ExtendNumeric02 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目03（未使用）</summary>
            /// <value>拡張用数値項目03（未使用）</value>
            public decimal? ExtendNumeric03 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目04（未使用）</summary>
            /// <value>拡張用数値項目04（未使用）</value>
            public decimal? ExtendNumeric04 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目05（未使用）</summary>
            /// <value>拡張用数値項目05（未使用）</value>
            public decimal? ExtendNumeric05 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目01（未使用）</summary>
            /// <value>拡張用日付項目01（未使用）</value>
            public DateTime? ExtendDate01 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目02（未使用）</summary>
            /// <value>拡張用日付項目02（未使用）</value>
            public DateTime? ExtendDate02 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目03（未使用）</summary>
            /// <value>拡張用日付項目03（未使用）</value>
            public DateTime? ExtendDate03 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目04（未使用）</summary>
            /// <value>拡張用日付項目04（未使用）</value>
            public DateTime? ExtendDate04 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目05（未使用）</summary>
            /// <value>拡張用日付項目05（未使用）</value>
            public DateTime? ExtendDate05 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 購入依頼番号</summary>
                /// <value>購入依頼番号</value>
                public string PurchaseRequestNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="purchaseRequestNo">購入依頼番号</param>
                public PrimaryKey(string purchaseRequestNo)
                {
                    PurchaseRequestNo = purchaseRequestNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PurchaseRequestNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="purchaseRequestNo">購入依頼番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PurchaseRequestHeadEntity GetEntity(string purchaseRequestNo, ComDB db)
            {
                PurchaseRequestHeadEntity.PrimaryKey condition = new PurchaseRequestHeadEntity.PrimaryKey(purchaseRequestNo);
                return GetEntity<PurchaseRequestHeadEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 購入依頼詳細
        /// </summary>
        public class PurchaseRequestDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PurchaseRequestDetailEntity()
            {
                TableName = "purchase_request_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 購入依頼番号</summary>
            /// <value>購入依頼番号</value>
            public string PurchaseRequestNo { get; set; }
            /// <summary>Gets or sets 明細行No</summary>
            /// <value>明細行No</value>
            public int PurchaseRequestRowNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DispOrder { get; set; }
            /// <summary>Gets or sets 購買ステータス</summary>
            /// <value>購買ステータス</value>
            public int? PurchaseStatus { get; set; }
            /// <summary>Gets or sets 発注区分</summary>
            /// <value>発注区分</value>
            public int? OrderDivision { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目有効開始日</summary>
            /// <value>品目有効開始日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 希望取引先コード</summary>
            /// <value>希望取引先コード</value>
            public string RequestVenderCd { get; set; }
            /// <summary>Gets or sets 希望取引先有効開始日</summary>
            /// <value>希望取引先有効開始日</value>
            public DateTime? RequestVenderActiveDate { get; set; }
            /// <summary>Gets or sets 希望取引先名称</summary>
            /// <value>希望取引先名称</value>
            public string RequestVenderName { get; set; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 納入先名称</summary>
            /// <value>納入先名称</value>
            public string DeliveryName { get; set; }
            /// <summary>Gets or sets 購入依頼数量</summary>
            /// <value>購入依頼数量</value>
            public decimal? PurchaseRequestQuantity { get; set; }
            /// <summary>Gets or sets 購入依頼重量</summary>
            /// <value>購入依頼重量</value>
            public decimal? PurchaseRequestWeight { get; set; }
            /// <summary>Gets or sets 依頼単価</summary>
            /// <value>依頼単価</value>
            public decimal? RequestUnitprice { get; set; }
            /// <summary>Gets or sets 依頼金額</summary>
            /// <value>依頼金額</value>
            public decimal? RequestAmount { get; set; }
            /// <summary>Gets or sets 希望納期</summary>
            /// <value>希望納期</value>
            public DateTime? RequestDeliveryDate { get; set; }
            /// <summary>Gets or sets 明細備考</summary>
            /// <value>明細備考</value>
            public string DetailRemark { get; set; }
            /// <summary>Gets or sets 購入依頼承認トランザクションID</summary>
            /// <value>購入依頼承認トランザクションID</value>
            public string BuySubcontractTransactionId { get; set; }
            /// <summary>Gets or sets 購入依頼承認日</summary>
            /// <value>購入依頼承認日</value>
            public DateTime? BuySubcontractApprovalDate { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目01（未使用）</summary>
            /// <value>拡張用文字列項目01（未使用）</value>
            public string ExtendString01 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目02（未使用）</summary>
            /// <value>拡張用文字列項目02（未使用）</value>
            public string ExtendString02 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目03（未使用）</summary>
            /// <value>拡張用文字列項目03（未使用）</value>
            public string ExtendString03 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目04（未使用）</summary>
            /// <value>拡張用文字列項目04（未使用）</value>
            public string ExtendString04 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目05（未使用）</summary>
            /// <value>拡張用文字列項目05（未使用）</value>
            public string ExtendString05 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目01（未使用）</summary>
            /// <value>拡張用数値項目01（未使用）</value>
            public decimal? ExtendNumeric01 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目02（未使用）</summary>
            /// <value>拡張用数値項目02（未使用）</value>
            public decimal? ExtendNumeric02 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目03（未使用）</summary>
            /// <value>拡張用数値項目03（未使用）</value>
            public decimal? ExtendNumeric03 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目04（未使用）</summary>
            /// <value>拡張用数値項目04（未使用）</value>
            public decimal? ExtendNumeric04 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目05（未使用）</summary>
            /// <value>拡張用数値項目05（未使用）</value>
            public decimal? ExtendNumeric05 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目01（未使用）</summary>
            /// <value>拡張用日付項目01（未使用）</value>
            public DateTime? ExtendDate01 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目02（未使用）</summary>
            /// <value>拡張用日付項目02（未使用）</value>
            public DateTime? ExtendDate02 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目03（未使用）</summary>
            /// <value>拡張用日付項目03（未使用）</value>
            public DateTime? ExtendDate03 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目04（未使用）</summary>
            /// <value>拡張用日付項目04（未使用）</value>
            public DateTime? ExtendDate04 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目05（未使用）</summary>
            /// <value>拡張用日付項目05（未使用）</value>
            public DateTime? ExtendDate05 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 購入依頼番号</summary>
                /// <value>購入依頼番号</value>
                public string PurchaseRequestNo { get; set; }
                /// <summary>Gets or sets 明細行No</summary>
                /// <value>明細行No</value>
                public int PurchaseRequestRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="purchaseRequestNo">購入依頼番号</param>
                /// <param name="purchaseRequestRowNo">明細行No</param>
                public PrimaryKey(string purchaseRequestNo, int purchaseRequestRowNo)
                {
                    PurchaseRequestNo = purchaseRequestNo;
                    PurchaseRequestRowNo = purchaseRequestRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PurchaseRequestNo, this.PurchaseRequestRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="purchaseRequestNo">購入依頼番号</param>
            /// <param name="purchaseRequestRowNo">明細行No</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PurchaseRequestDetailEntity GetEntity(string purchaseRequestNo, int purchaseRequestRowNo, ComDB db)
            {
                PurchaseRequestDetailEntity.PrimaryKey condition = new PurchaseRequestDetailEntity.PrimaryKey(purchaseRequestNo, purchaseRequestRowNo);
                return GetEntity<PurchaseRequestDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 発注ヘッダ
        /// </summary>
        public class PurchaseHeadEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PurchaseHeadEntity()
            {
                TableName = "purchase_head";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 発注番号</summary>
            /// <value>発注番号</value>
            public string PurchaseOrderNo { get; set; }
            /// <summary>Gets or sets 購入依頼番号</summary>
            /// <value>購入依頼番号</value>
            public string PurchaseRequestNo { get; set; }
            /// <summary>Gets or sets 購買ステータス</summary>
            /// <value>購買ステータス</value>
            public int? PurchaseStatus { get; set; }
            /// <summary>Gets or sets 発注日</summary>
            /// <value>発注日</value>
            public DateTime? OrderDate { get; set; }
            /// <summary>Gets or sets 発生元区分</summary>
            /// <value>発生元区分</value>
            public int? OriginDivision { get; set; }
            /// <summary>Gets or sets 発注件名</summary>
            /// <value>発注件名</value>
            public string PurchaseSubject { get; set; }
            /// <summary>Gets or sets 発注担当者</summary>
            /// <value>発注担当者</value>
            public string PurchaseOrderChargeCd { get; set; }
            /// <summary>Gets or sets 発注部署コード</summary>
            /// <value>発注部署コード</value>
            public string PurchaseOrderDepartmentCd { get; set; }
            /// <summary>Gets or sets 発注備考</summary>
            /// <value>発注備考</value>
            public string PurchaseOrderRemark { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目01（未使用）</summary>
            /// <value>拡張用文字列項目01（未使用）</value>
            public string ExtendString01 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目02（未使用）</summary>
            /// <value>拡張用文字列項目02（未使用）</value>
            public string ExtendString02 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目03（未使用）</summary>
            /// <value>拡張用文字列項目03（未使用）</value>
            public string ExtendString03 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目04（未使用）</summary>
            /// <value>拡張用文字列項目04（未使用）</value>
            public string ExtendString04 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目05（未使用）</summary>
            /// <value>拡張用文字列項目05（未使用）</value>
            public string ExtendString05 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目01（未使用）</summary>
            /// <value>拡張用数値項目01（未使用）</value>
            public decimal? ExtendNumeric01 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目02（未使用）</summary>
            /// <value>拡張用数値項目02（未使用）</value>
            public decimal? ExtendNumeric02 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目03（未使用）</summary>
            /// <value>拡張用数値項目03（未使用）</value>
            public decimal? ExtendNumeric03 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目04（未使用）</summary>
            /// <value>拡張用数値項目04（未使用）</value>
            public decimal? ExtendNumeric04 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目05（未使用）</summary>
            /// <value>拡張用数値項目05（未使用）</value>
            public decimal? ExtendNumeric05 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目01（未使用）</summary>
            /// <value>拡張用日付項目01（未使用）</value>
            public DateTime? ExtendDate01 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目02（未使用）</summary>
            /// <value>拡張用日付項目02（未使用）</value>
            public DateTime? ExtendDate02 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目03（未使用）</summary>
            /// <value>拡張用日付項目03（未使用）</value>
            public DateTime? ExtendDate03 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目04（未使用）</summary>
            /// <value>拡張用日付項目04（未使用）</value>
            public DateTime? ExtendDate04 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目05（未使用）</summary>
            /// <value>拡張用日付項目05（未使用）</value>
            public DateTime? ExtendDate05 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 発注番号</summary>
                /// <value>発注番号</value>
                public string PurchaseOrderNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="purchaseOrderNo">発注番号</param>
                public PrimaryKey(string purchaseOrderNo)
                {
                    PurchaseOrderNo = purchaseOrderNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PurchaseOrderNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="purchaseOrderNo">発注番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PurchaseHeadEntity GetEntity(string purchaseOrderNo, ComDB db)
            {
                PurchaseHeadEntity.PrimaryKey condition = new PurchaseHeadEntity.PrimaryKey(purchaseOrderNo);
                return GetEntity<PurchaseHeadEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 発注詳細
        /// </summary>
        public class PurchaseDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PurchaseDetailEntity()
            {
                TableName = "purchase_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 発注番号</summary>
            /// <value>発注番号</value>
            public string PurchaseOrderNo { get; set; }
            /// <summary>Gets or sets 明細行No</summary>
            /// <value>明細行No</value>
            public int PurchaseOrderRowNo { get; set; }
            /// <summary>Gets or sets 明細サブNo</summary>
            /// <value>明細サブNo</value>
            public int PurchaseOrderSubNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DispOrder { get; set; }
            /// <summary>Gets or sets 購入依頼番号</summary>
            /// <value>購入依頼番号</value>
            public string PurchaseRequestNo { get; set; }
            /// <summary>Gets or sets 購買ステータス</summary>
            /// <value>購買ステータス</value>
            public int? PurchaseStatus { get; set; }
            /// <summary>Gets or sets 発注区分</summary>
            /// <value>発注区分</value>
            public int? OrderDivision { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目有効開始日</summary>
            /// <value>品目有効開始日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先有効開始日</summary>
            /// <value>取引先有効開始日</value>
            public DateTime? VenderActiveDate { get; set; }
            /// <summary>Gets or sets 取引先名称</summary>
            /// <value>取引先名称</value>
            public string VenderName { get; set; }
            /// <summary>Gets or sets 納入先コード</summary>
            /// <value>納入先コード</value>
            public string DeliveryCd { get; set; }
            /// <summary>Gets or sets 納入先名称</summary>
            /// <value>納入先名称</value>
            public string DeliveryName { get; set; }
            /// <summary>Gets or sets 発注数量</summary>
            /// <value>発注数量</value>
            public decimal? PurchaseQuantity { get; set; }
            /// <summary>Gets or sets 発注重量</summary>
            /// <value>発注重量</value>
            public decimal? PurchaseWeight { get; set; }
            /// <summary>Gets or sets 注決単価</summary>
            /// <value>注決単価</value>
            public decimal? PurchaseUnitprice { get; set; }
            /// <summary>Gets or sets 注決金額</summary>
            /// <value>注決金額</value>
            public decimal? PurchaseAmount { get; set; }
            /// <summary>Gets or sets 注決納期</summary>
            /// <value>注決納期</value>
            public DateTime? PurchaseDeliveryDate { get; set; }
            /// <summary>Gets or sets 明細備考</summary>
            /// <value>明細備考</value>
            public string DetailRemark { get; set; }
            /// <summary>Gets or sets 消費税率区分</summary>
            /// <value>消費税率区分</value>
            public string TaxRateDivision { get; set; }
            /// <summary>Gets or sets 消費税金額</summary>
            /// <value>消費税金額</value>
            public decimal? TaxAmount { get; set; }
            /// <summary>Gets or sets 発注承認トランザクションID</summary>
            /// <value>発注承認トランザクションID</value>
            public string PurchaseTransactionId { get; set; }
            /// <summary>Gets or sets 発注承認日</summary>
            /// <value>発注承認日</value>
            public DateTime? PurchaseApprovalDate { get; set; }
            /// <summary>Gets or sets 外注指図区分</summary>
            /// <value>外注指図区分</value>
            public int? OutsourcingDirectionDivision { get; set; }
            /// <summary>Gets or sets 外注指図番号</summary>
            /// <value>外注指図番号</value>
            public string OutsourcingDirectionNo { get; set; }
            /// <summary>Gets or sets 注文書備考</summary>
            /// <value>注文書備考</value>
            public string OrderSheetRemark { get; set; }
            /// <summary>Gets or sets 仕入先受注番号</summary>
            /// <value>仕入先受注番号</value>
            public string SupplierOrderNo { get; set; }
            /// <summary>Gets or sets 注文書発行ステータス</summary>
            /// <value>注文書発行ステータス</value>
            public int? OrderSheetStatus { get; set; }
            /// <summary>Gets or sets 注文書NO</summary>
            /// <value>注文書NO</value>
            public string OrderSheetNo { get; set; }
            /// <summary>Gets or sets 注文書発行日（初回）</summary>
            /// <value>注文書発行日（初回）</value>
            public DateTime? OrderSheetDate { get; set; }
            /// <summary>Gets or sets 注文書発行者</summary>
            /// <value>注文書発行者</value>
            public string OrderSheetUser { get; set; }
            /// <summary>Gets or sets 注文書発行回数</summary>
            /// <value>注文書発行回数</value>
            public int? OrderSheetCount { get; set; }
            /// <summary>Gets or sets 注文書発行日（最終）</summary>
            /// <value>注文書発行日（最終）</value>
            public DateTime? LastOrderSheetDate { get; set; }
            /// <summary>Gets or sets 次回納品希望日</summary>
            /// <value>次回納品希望日</value>
            public DateTime? NextDeliveryDate { get; set; }
            /// <summary>Gets or sets 完納区分</summary>
            /// <value>完納区分</value>
            public int? CompleteDivision { get; set; }
            /// <summary>Gets or sets オーダークローズ日</summary>
            /// <value>オーダークローズ日</value>
            public DateTime? OrderCloseDate { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 通貨レート</summary>
            /// <value>通貨レート</value>
            public decimal? CurrencyRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? RateValidDate { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目01（未使用）</summary>
            /// <value>拡張用文字列項目01（未使用）</value>
            public string ExtendString01 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目02（未使用）</summary>
            /// <value>拡張用文字列項目02（未使用）</value>
            public string ExtendString02 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目03（未使用）</summary>
            /// <value>拡張用文字列項目03（未使用）</value>
            public string ExtendString03 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目04（未使用）</summary>
            /// <value>拡張用文字列項目04（未使用）</value>
            public string ExtendString04 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目05（未使用）</summary>
            /// <value>拡張用文字列項目05（未使用）</value>
            public string ExtendString05 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目01（未使用）</summary>
            /// <value>拡張用数値項目01（未使用）</value>
            public decimal? ExtendNumeric01 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目02（未使用）</summary>
            /// <value>拡張用数値項目02（未使用）</value>
            public decimal? ExtendNumeric02 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目03（未使用）</summary>
            /// <value>拡張用数値項目03（未使用）</value>
            public decimal? ExtendNumeric03 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目04（未使用）</summary>
            /// <value>拡張用数値項目04（未使用）</value>
            public decimal? ExtendNumeric04 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目05（未使用）</summary>
            /// <value>拡張用数値項目05（未使用）</value>
            public decimal? ExtendNumeric05 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目01（未使用）</summary>
            /// <value>拡張用日付項目01（未使用）</value>
            public DateTime? ExtendDate01 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目02（未使用）</summary>
            /// <value>拡張用日付項目02（未使用）</value>
            public DateTime? ExtendDate02 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目03（未使用）</summary>
            /// <value>拡張用日付項目03（未使用）</value>
            public DateTime? ExtendDate03 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目04（未使用）</summary>
            /// <value>拡張用日付項目04（未使用）</value>
            public DateTime? ExtendDate04 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目05（未使用）</summary>
            /// <value>拡張用日付項目05（未使用）</value>
            public DateTime? ExtendDate05 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 発注番号</summary>
                /// <value>発注番号</value>
                public string PurchaseOrderNo { get; set; }
                /// <summary>Gets or sets 明細行No</summary>
                /// <value>明細行No</value>
                public int PurchaseOrderRowNo { get; set; }
                /// <summary>Gets or sets 明細サブNo</summary>
                /// <value>明細サブNo</value>
                public int PurchaseOrderSubNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="purchaseOrderNo">発注番号</param>
                /// <param name="purchaseOrderRowNo">明細行No</param>
                /// <param name="purchaseOrderSubNo">明細サブNo</param>
                public PrimaryKey(string purchaseOrderNo, int purchaseOrderRowNo, int purchaseOrderSubNo)
                {
                    PurchaseOrderNo = purchaseOrderNo;
                    PurchaseOrderRowNo = purchaseOrderRowNo;
                    PurchaseOrderSubNo = purchaseOrderSubNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PurchaseOrderNo, this.PurchaseOrderRowNo, this.PurchaseOrderSubNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="purchaseOrderNo">発注番号</param>
            /// <param name="purchaseOrderRowNo">明細行No</param>
            /// <param name="purchaseOrderSubNo">明細サブNo</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PurchaseDetailEntity GetEntity(string purchaseOrderNo, int purchaseOrderRowNo, int purchaseOrderSubNo, ComDB db)
            {
                PurchaseDetailEntity.PrimaryKey condition = new PurchaseDetailEntity.PrimaryKey(purchaseOrderNo, purchaseOrderRowNo, purchaseOrderSubNo);
                return GetEntity<PurchaseDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 購買関連情報
        /// </summary>
        public class PurchaseRelationEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PurchaseRelationEntity()
            {
                TableName = "purchase_relation";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 購入依頼番号</summary>
            /// <value>購入依頼番号</value>
            public string PurchaseRequestNo { get; set; }
            /// <summary>Gets or sets データ区分</summary>
            /// <value>データ区分</value>
            public int DataDivision { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int RowNo { get; set; }
            /// <summary>Gets or sets オーダー番号</summary>
            /// <value>オーダー番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目01（未使用）</summary>
            /// <value>拡張用文字列項目01（未使用）</value>
            public string ExtendString01 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目02（未使用）</summary>
            /// <value>拡張用文字列項目02（未使用）</value>
            public string ExtendString02 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目03（未使用）</summary>
            /// <value>拡張用文字列項目03（未使用）</value>
            public string ExtendString03 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目04（未使用）</summary>
            /// <value>拡張用文字列項目04（未使用）</value>
            public string ExtendString04 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目05（未使用）</summary>
            /// <value>拡張用文字列項目05（未使用）</value>
            public string ExtendString05 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目01（未使用）</summary>
            /// <value>拡張用数値項目01（未使用）</value>
            public decimal? ExtendNumeric01 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目02（未使用）</summary>
            /// <value>拡張用数値項目02（未使用）</value>
            public decimal? ExtendNumeric02 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目03（未使用）</summary>
            /// <value>拡張用数値項目03（未使用）</value>
            public decimal? ExtendNumeric03 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目04（未使用）</summary>
            /// <value>拡張用数値項目04（未使用）</value>
            public decimal? ExtendNumeric04 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目05（未使用）</summary>
            /// <value>拡張用数値項目05（未使用）</value>
            public decimal? ExtendNumeric05 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目01（未使用）</summary>
            /// <value>拡張用日付項目01（未使用）</value>
            public DateTime? ExtendDate01 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目02（未使用）</summary>
            /// <value>拡張用日付項目02（未使用）</value>
            public DateTime? ExtendDate02 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目03（未使用）</summary>
            /// <value>拡張用日付項目03（未使用）</value>
            public DateTime? ExtendDate03 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目04（未使用）</summary>
            /// <value>拡張用日付項目04（未使用）</value>
            public DateTime? ExtendDate04 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目05（未使用）</summary>
            /// <value>拡張用日付項目05（未使用）</value>
            public DateTime? ExtendDate05 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 購入依頼番号</summary>
                /// <value>購入依頼番号</value>
                public string PurchaseRequestNo { get; set; }
                /// <summary>Gets or sets データ区分</summary>
                /// <value>データ区分</value>
                public int DataDivision { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="purchaseRequestNo">購入依頼番号</param>
                /// <param name="dataDivision">データ区分</param>
                /// <param name="rowNo">行番号</param>
                public PrimaryKey(string purchaseRequestNo, int dataDivision, int rowNo)
                {
                    PurchaseRequestNo = purchaseRequestNo;
                    DataDivision = dataDivision;
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PurchaseRequestNo, this.DataDivision, this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="purchaseRequestNo">購入依頼番号</param>
            /// <param name="dataDivision">データ区分</param>
            /// <param name="rowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public PurchaseRelationEntity GetEntity(string purchaseRequestNo, int dataDivision, int rowNo, ComDB db)
            {
                PurchaseRelationEntity.PrimaryKey condition = new PurchaseRelationEntity.PrimaryKey(purchaseRequestNo, dataDivision, rowNo);
                return GetEntity<PurchaseRelationEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 受入ヘッダ
        /// </summary>
        public class AcceptingHeadEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public AcceptingHeadEntity()
            {
                TableName = "accepting_head";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 受入番号</summary>
            /// <value>受入番号</value>
            public string AcceptingNo { get; set; }
            /// <summary>Gets or sets 発注番号</summary>
            /// <value>発注番号</value>
            public string PurchaseOrderNo { get; set; }
            /// <summary>Gets or sets 明細行No</summary>
            /// <value>明細行No</value>
            public int PurchaseOrderRowNo { get; set; }
            /// <summary>Gets or sets 明細サブNo</summary>
            /// <value>明細サブNo</value>
            public int PurchaseOrderSubNo { get; set; }
            /// <summary>Gets or sets 受入回数</summary>
            /// <value>受入回数</value>
            public string DivideNo { get; set; }
            /// <summary>Gets or sets 購買ステータス</summary>
            /// <value>購買ステータス</value>
            public int? PurchaseStatus { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目有効開始日</summary>
            /// <value>品目有効開始日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 発注数量</summary>
            /// <value>発注数量</value>
            public decimal? PurchaseQuantity { get; set; }
            /// <summary>Gets or sets 受入数量累計</summary>
            /// <value>受入数量累計</value>
            public decimal? TotalArrivalQuantity { get; set; }
            /// <summary>Gets or sets 受入検収数量累計</summary>
            /// <value>受入検収数量累計</value>
            public decimal? TotalAcceptQuantity { get; set; }
            /// <summary>Gets or sets 会計部門コード</summary>
            /// <value>会計部門コード</value>
            public string DepartmentCd { get; set; }
            /// <summary>Gets or sets 受入担当者コード</summary>
            /// <value>受入担当者コード</value>
            public string ArrivalUserId { get; set; }
            /// <summary>Gets or sets 受入検収担当者コード</summary>
            /// <value>受入検収担当者コード</value>
            public string AcceptUserId { get; set; }
            /// <summary>Gets or sets 完納区分</summary>
            /// <value>完納区分</value>
            public int? CompleteDivision { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目01（未使用）</summary>
            /// <value>拡張用文字列項目01（未使用）</value>
            public string ExtendString01 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目02（未使用）</summary>
            /// <value>拡張用文字列項目02（未使用）</value>
            public string ExtendString02 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目03（未使用）</summary>
            /// <value>拡張用文字列項目03（未使用）</value>
            public string ExtendString03 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目04（未使用）</summary>
            /// <value>拡張用文字列項目04（未使用）</value>
            public string ExtendString04 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目05（未使用）</summary>
            /// <value>拡張用文字列項目05（未使用）</value>
            public string ExtendString05 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目01（未使用）</summary>
            /// <value>拡張用数値項目01（未使用）</value>
            public decimal? ExtendNumeric01 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目02（未使用）</summary>
            /// <value>拡張用数値項目02（未使用）</value>
            public decimal? ExtendNumeric02 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目03（未使用）</summary>
            /// <value>拡張用数値項目03（未使用）</value>
            public decimal? ExtendNumeric03 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目04（未使用）</summary>
            /// <value>拡張用数値項目04（未使用）</value>
            public decimal? ExtendNumeric04 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目05（未使用）</summary>
            /// <value>拡張用数値項目05（未使用）</value>
            public decimal? ExtendNumeric05 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目01（未使用）</summary>
            /// <value>拡張用日付項目01（未使用）</value>
            public DateTime? ExtendDate01 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目02（未使用）</summary>
            /// <value>拡張用日付項目02（未使用）</value>
            public DateTime? ExtendDate02 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目03（未使用）</summary>
            /// <value>拡張用日付項目03（未使用）</value>
            public DateTime? ExtendDate03 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目04（未使用）</summary>
            /// <value>拡張用日付項目04（未使用）</value>
            public DateTime? ExtendDate04 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目05（未使用）</summary>
            /// <value>拡張用日付項目05（未使用）</value>
            public DateTime? ExtendDate05 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 受入番号</summary>
                /// <value>受入番号</value>
                public string AcceptingNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="acceptingNo">受入番号</param>
                public PrimaryKey(string acceptingNo)
                {
                    AcceptingNo = acceptingNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.AcceptingNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="acceptingNo">受入番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public AcceptingHeadEntity GetEntity(string acceptingNo, ComDB db)
            {
                AcceptingHeadEntity.PrimaryKey condition = new AcceptingHeadEntity.PrimaryKey(acceptingNo);
                return GetEntity<AcceptingHeadEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 受入詳細
        /// </summary>
        public class AcceptingDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public AcceptingDetailEntity()
            {
                TableName = "accepting_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 受入番号</summary>
            /// <value>受入番号</value>
            public string AcceptingNo { get; set; }
            /// <summary>Gets or sets 明細行No</summary>
            /// <value>明細行No</value>
            public int AcceptingRowNo { get; set; }
            /// <summary>Gets or sets 購買ステータス</summary>
            /// <value>購買ステータス</value>
            public int? PurchaseStatus { get; set; }
            /// <summary>Gets or sets 受入数量</summary>
            /// <value>受入数量</value>
            public decimal? ArrivalQuantity { get; set; }
            /// <summary>Gets or sets 受入日</summary>
            /// <value>受入日</value>
            public DateTime? ArrivalDate { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets メーカーロット番号</summary>
            /// <value>メーカーロット番号</value>
            public string VenderLotNo { get; set; }
            /// <summary>Gets or sets 受入検収日</summary>
            /// <value>受入検収日</value>
            public DateTime? AcceptDate { get; set; }
            /// <summary>Gets or sets 受入検収数量</summary>
            /// <value>受入検収数量</value>
            public decimal? AcceptQuantity { get; set; }
            /// <summary>Gets or sets 入庫ロケーション</summary>
            /// <value>入庫ロケーション</value>
            public string WarehousingLocation { get; set; }
            /// <summary>Gets or sets 完納区分</summary>
            /// <value>完納区分</value>
            public int? CompleteDivision { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目01（未使用）</summary>
            /// <value>拡張用文字列項目01（未使用）</value>
            public string ExtendString01 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目02（未使用）</summary>
            /// <value>拡張用文字列項目02（未使用）</value>
            public string ExtendString02 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目03（未使用）</summary>
            /// <value>拡張用文字列項目03（未使用）</value>
            public string ExtendString03 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目04（未使用）</summary>
            /// <value>拡張用文字列項目04（未使用）</value>
            public string ExtendString04 { get; set; }
            /// <summary>Gets or sets 拡張用文字列項目05（未使用）</summary>
            /// <value>拡張用文字列項目05（未使用）</value>
            public string ExtendString05 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目01（未使用）</summary>
            /// <value>拡張用数値項目01（未使用）</value>
            public decimal? ExtendNumeric01 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目02（未使用）</summary>
            /// <value>拡張用数値項目02（未使用）</value>
            public decimal? ExtendNumeric02 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目03（未使用）</summary>
            /// <value>拡張用数値項目03（未使用）</value>
            public decimal? ExtendNumeric03 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目04（未使用）</summary>
            /// <value>拡張用数値項目04（未使用）</value>
            public decimal? ExtendNumeric04 { get; set; }
            /// <summary>Gets or sets 拡張用数値項目05（未使用）</summary>
            /// <value>拡張用数値項目05（未使用）</value>
            public decimal? ExtendNumeric05 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目01（未使用）</summary>
            /// <value>拡張用日付項目01（未使用）</value>
            public DateTime? ExtendDate01 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目02（未使用）</summary>
            /// <value>拡張用日付項目02（未使用）</value>
            public DateTime? ExtendDate02 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目03（未使用）</summary>
            /// <value>拡張用日付項目03（未使用）</value>
            public DateTime? ExtendDate03 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目04（未使用）</summary>
            /// <value>拡張用日付項目04（未使用）</value>
            public DateTime? ExtendDate04 { get; set; }
            /// <summary>Gets or sets 拡張用日付項目05（未使用）</summary>
            /// <value>拡張用日付項目05（未使用）</value>
            public DateTime? ExtendDate05 { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 受入番号</summary>
                /// <value>受入番号</value>
                public string AcceptingNo { get; set; }
                /// <summary>Gets or sets 明細行No</summary>
                /// <value>明細行No</value>
                public int AcceptingRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="acceptingNo">受入番号</param>
                /// <param name="acceptingRowNo">明細行No</param>
                public PrimaryKey(string acceptingNo, int acceptingRowNo)
                {
                    AcceptingNo = acceptingNo;
                    AcceptingRowNo = acceptingRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.AcceptingNo, this.AcceptingRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="acceptingNo">受入番号</param>
            /// <param name="acceptingRowNo">明細行No</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public AcceptingDetailEntity GetEntity(string acceptingNo, int acceptingRowNo, ComDB db)
            {
                AcceptingDetailEntity.PrimaryKey condition = new AcceptingDetailEntity.PrimaryKey(acceptingNo, acceptingRowNo);
                return GetEntity<AcceptingDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 仕入
        /// </summary>
        public class StockingEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public StockingEntity()
            {
                TableName = "stocking";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 仕入番号</summary>
            /// <value>仕入番号</value>
            public string StockingNo { get; set; }
            /// <summary>Gets or sets 仕入行番号</summary>
            /// <value>仕入行番号</value>
            public int StockingRowNo { get; set; }
            /// <summary>Gets or sets 仕入元番号</summary>
            /// <value>仕入元番号</value>
            public string OriginalStockingNo { get; set; }
            /// <summary>Gets or sets 受入番号</summary>
            /// <value>受入番号</value>
            public string AcceptingNo { get; set; }
            /// <summary>Gets or sets 受入明細行番号</summary>
            /// <value>受入明細行番号</value>
            public int AcceptingRowNo { get; set; }
            /// <summary>Gets or sets 仕入ステータス</summary>
            /// <value>仕入ステータス</value>
            public int? StockingStatus { get; set; }
            /// <summary>Gets or sets 伝票発行日</summary>
            /// <value>伝票発行日</value>
            public DateTime? SlipPublishDate { get; set; }
            /// <summary>Gets or sets 仕入種別|各種名称マスタ</summary>
            /// <value>仕入種別|各種名称マスタ</value>
            public int? StockingKind { get; set; }
            /// <summary>Gets or sets 仕入日付</summary>
            /// <value>仕入日付</value>
            public DateTime StockingDate { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets データ種別(分類マスタ参照)</summary>
            /// <value>データ種別(分類マスタ参照)</value>
            public int DataType { get; set; }
            /// <summary>Gets or sets データ種別集計区分(分類マスタ参照)</summary>
            /// <value>データ種別集計区分(分類マスタ参照)</value>
            public int DataTotalDivision { get; set; }
            /// <summary>Gets or sets 分類コード(分類マスタ参照)</summary>
            /// <value>分類コード(分類マスタ参照)</value>
            public int CategoryDivision { get; set; }
            /// <summary>Gets or sets 担当者コード</summary>
            /// <value>担当者コード</value>
            public string StockingUserId { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string OrganizationCd { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 取引先開始有効日</summary>
            /// <value>取引先開始有効日</value>
            public DateTime? VenderActiveDate { get; set; }
            /// <summary>Gets or sets 支払先区分</summary>
            /// <value>支払先区分</value>
            public string SupplierDivision { get; set; }
            /// <summary>Gets or sets 支払先コード</summary>
            /// <value>支払先コード</value>
            public string SupplierCd { get; set; }
            /// <summary>Gets or sets 支払先開始有効日</summary>
            /// <value>支払先開始有効日</value>
            public DateTime? SupplierActiveDate { get; set; }
            /// <summary>Gets or sets 担当部署コード</summary>
            /// <value>担当部署コード</value>
            public string ChargeOrganizationCd { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目開始有効日</summary>
            /// <value>品目開始有効日</value>
            public DateTime? ItemActiveDate { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 品目仕様開始有効日</summary>
            /// <value>品目仕様開始有効日</value>
            public DateTime? SpecificationActiveDate { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 仕入数量</summary>
            /// <value>仕入数量</value>
            public decimal? StockingQuantity { get; set; }
            /// <summary>Gets or sets 仕入単価</summary>
            /// <value>仕入単価</value>
            public decimal? StockingUnitprice { get; set; }
            /// <summary>Gets or sets 仕入金額</summary>
            /// <value>仕入金額</value>
            public decimal? StockingAmount { get; set; }
            /// <summary>Gets or sets 消費税額</summary>
            /// <value>消費税額</value>
            public decimal? TaxAmonut { get; set; }
            /// <summary>Gets or sets 消費税課税区分|各種名称マスタ</summary>
            /// <value>消費税課税区分|各種名称マスタ</value>
            public int TaxDivision { get; set; }
            /// <summary>Gets or sets 消費税率</summary>
            /// <value>消費税率</value>
            public decimal TaxRatio { get; set; }
            /// <summary>Gets or sets 消費税区分コード</summary>
            /// <value>消費税区分コード</value>
            public string TaxCd { get; set; }
            /// <summary>Gets or sets 出庫ロケーション</summary>
            /// <value>出庫ロケーション</value>
            public string WarehousingLocation { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets 会計部門借方コード</summary>
            /// <value>会計部門借方コード</value>
            public string AccountDebitSectionCd { get; set; }
            /// <summary>Gets or sets 会計部門貸方コード</summary>
            /// <value>会計部門貸方コード</value>
            public string AccountCreditSectionCd { get; set; }
            /// <summary>Gets or sets 借方科目コード</summary>
            /// <value>借方科目コード</value>
            public string DebitTitleCd { get; set; }
            /// <summary>Gets or sets 貸方科目コード</summary>
            /// <value>貸方科目コード</value>
            public string CreditTitleCd { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 通貨コード</summary>
            /// <value>通貨コード</value>
            public string CurrencyCd { get; set; }
            /// <summary>Gets or sets 基準通貨レート</summary>
            /// <value>基準通貨レート</value>
            public decimal? CurrencyRate { get; set; }
            /// <summary>Gets or sets レート適用開始日</summary>
            /// <value>レート適用開始日</value>
            public DateTime? RateValidDate { get; set; }
            /// <summary>Gets or sets 支払締め日</summary>
            /// <value>支払締め日</value>
            public DateTime? PaymentUpdateDate { get; set; }
            /// <summary>Gets or sets 支払対象|0</summary>
            /// <value>支払対象|0</value>
            public int PaymentTargetDivision { get; set; }
            /// <summary>Gets or sets 支払更新フラグ|0</summary>
            /// <value>支払更新フラグ|0</value>
            public int PaymentUpdateDivision { get; set; }
            /// <summary>Gets or sets 支払番号</summary>
            /// <value>支払番号</value>
            public string PaymentNo { get; set; }
            /// <summary>Gets or sets 買掛締め日</summary>
            /// <value>買掛締め日</value>
            public DateTime? PayableUpdateDate { get; set; }
            /// <summary>Gets or sets 買掛対象|0</summary>
            /// <value>買掛対象|0</value>
            public int PayableTargetDivision { get; set; }
            /// <summary>Gets or sets 買掛更新フラグ|0</summary>
            /// <value>買掛更新フラグ|0</value>
            public int PayableUpdateDivision { get; set; }
            /// <summary>Gets or sets 買掛番号</summary>
            /// <value>買掛番号</value>
            public string PayableNo { get; set; }
            /// <summary>Gets or sets 消込完了フラグ|0</summary>
            /// <value>消込完了フラグ|0</value>
            public int EraserCompleteDivision { get; set; }
            /// <summary>Gets or sets 消込完了日</summary>
            /// <value>消込完了日</value>
            public DateTime? EraserCompleteDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 仕入番号</summary>
                /// <value>仕入番号</value>
                public string StockingNo { get; set; }
                /// <summary>Gets or sets 仕入行番号</summary>
                /// <value>仕入行番号</value>
                public int StockingRowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="stockingNo">仕入番号</param>
                /// <param name="stockingRowNo">仕入行番号</param>
                public PrimaryKey(string stockingNo, int stockingRowNo)
                {
                    StockingNo = stockingNo;
                    StockingRowNo = stockingRowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.StockingNo, this.StockingRowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="stockingNo">仕入番号</param>
            /// <param name="stockingRowNo">仕入行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public StockingEntity GetEntity(string stockingNo, int stockingRowNo, ComDB db)
            {
                StockingEntity.PrimaryKey condition = new StockingEntity.PrimaryKey(stockingNo, stockingRowNo);
                return GetEntity<StockingEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 在庫調整指図ヘッダ
        /// </summary>
        public class InventoryAdjustmentDirectionHeadEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InventoryAdjustmentDirectionHeadEntity()
            {
                TableName = "inventory_adjustment_direction_head";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 在庫指図番号</summary>
            /// <value>在庫指図番号</value>
            public string InventoryDirectionNo { get; set; }
            /// <summary>Gets or sets 入出庫区分</summary>
            /// <value>入出庫区分</value>
            public string InoutDivision { get; set; }
            /// <summary>Gets or sets 在庫指図日</summary>
            /// <value>在庫指図日</value>
            public DateTime InventoryDirectionDate { get; set; }
            /// <summary>Gets or sets 取消元指図番号</summary>
            /// <value>取消元指図番号</value>
            public string CancelInventoryDirectionNo { get; set; }
            /// <summary>Gets or sets 取消フラグ</summary>
            /// <value>取消フラグ</value>
            public int? CancelFlg { get; set; }
            /// <summary>Gets or sets 受払日</summary>
            /// <value>受払日</value>
            public DateTime InoutDate { get; set; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 受払数合計(from)</summary>
            /// <value>受払数合計(from)</value>
            public decimal? FromTotalInoutQty { get; set; }
            /// <summary>Gets or sets 受払端数合計(from)</summary>
            /// <value>受払端数合計(from)</value>
            public decimal? FromTotalFractionQty { get; set; }
            /// <summary>Gets or sets 受払数合計(to)</summary>
            /// <value>受払数合計(to)</value>
            public decimal? ToTotalInoutQty { get; set; }
            /// <summary>Gets or sets 受払端数合計(to)</summary>
            /// <value>受払端数合計(to)</value>
            public decimal? ToTotalFractionQty { get; set; }
            /// <summary>Gets or sets 理由コード</summary>
            /// <value>理由コード</value>
            public string RyCd { get; set; }
            /// <summary>Gets or sets 理由</summary>
            /// <value>理由</value>
            public string Reason { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 承認トランザクションID</summary>
            /// <value>承認トランザクションID</value>
            public string TransactionId { get; set; }
            /// <summary>Gets or sets 承認ステータス</summary>
            /// <value>承認ステータス</value>
            public int ApprovalStatus { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 在庫指図番号</summary>
                /// <value>在庫指図番号</value>
                public string InventoryDirectionNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="inventoryDirectionNo">在庫指図番号</param>
                public PrimaryKey(string inventoryDirectionNo)
                {
                    InventoryDirectionNo = inventoryDirectionNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InventoryDirectionNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="inventoryDirectionNo">在庫指図番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InventoryAdjustmentDirectionHeadEntity GetEntity(string inventoryDirectionNo, ComDB db)
            {
                InventoryAdjustmentDirectionHeadEntity.PrimaryKey condition = new InventoryAdjustmentDirectionHeadEntity.PrimaryKey(inventoryDirectionNo);
                return GetEntity<InventoryAdjustmentDirectionHeadEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 在庫調整指図詳細
        /// </summary>
        public class InventoryAdjustmentDirectionDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InventoryAdjustmentDirectionDetailEntity()
            {
                TableName = "inventory_adjustment_direction_detail";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 在庫指図番号</summary>
            /// <value>在庫指図番号</value>
            public string InventoryDirectionNo { get; set; }
            /// <summary>Gets or sets FromTo区分</summary>
            /// <value>FromTo区分</value>
            public int FromToDivision { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int RowNo { get; set; }
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
            /// <summary>Gets or sets 指図数量</summary>
            /// <value>指図数量</value>
            public decimal? InoutQty { get; set; }
            /// <summary>Gets or sets 指図端数数量</summary>
            /// <value>指図端数数量</value>
            public decimal? FractionQty { get; set; }
            /// <summary>Gets or sets 実績数量</summary>
            /// <value>実績数量</value>
            public decimal? InoutResultQty { get; set; }
            /// <summary>Gets or sets 端数数量</summary>
            /// <value>端数数量</value>
            public decimal? FractionResultQty { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 在庫指図番号</summary>
                /// <value>在庫指図番号</value>
                public string InventoryDirectionNo { get; set; }
                /// <summary>Gets or sets FromTo区分</summary>
                /// <value>FromTo区分</value>
                public int FromToDivision { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public int RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="inventoryDirectionNo">在庫指図番号</param>
                /// <param name="fromToDivision">FromTo区分</param>
                /// <param name="rowNo">行番号</param>
                public PrimaryKey(string inventoryDirectionNo, int fromToDivision, int rowNo)
                {
                    InventoryDirectionNo = inventoryDirectionNo;
                    FromToDivision = fromToDivision;
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InventoryDirectionNo, this.FromToDivision, this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="inventoryDirectionNo">在庫指図番号</param>
            /// <param name="fromToDivision">FromTo区分</param>
            /// <param name="rowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InventoryAdjustmentDirectionDetailEntity GetEntity(string inventoryDirectionNo, int fromToDivision, int rowNo, ComDB db)
            {
                InventoryAdjustmentDirectionDetailEntity.PrimaryKey condition = new InventoryAdjustmentDirectionDetailEntity.PrimaryKey(inventoryDirectionNo, fromToDivision, rowNo);
                return GetEntity<InventoryAdjustmentDirectionDetailEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 在庫バランス
        /// </summary>
        public class InventoryBalanceEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InventoryBalanceEntity()
            {
                TableName = "inventory_balance";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 在庫区分</summary>
            /// <value>在庫区分</value>
            public long InventoryDivision { get; set; }
            /// <summary>Gets or sets 受払予定日</summary>
            /// <value>受払予定日</value>
            public DateTime InoutExpectedDate { get; set; }
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
            /// <summary>Gets or sets 入庫予定数量</summary>
            /// <value>入庫予定数量</value>
            public decimal? InQty { get; set; }
            /// <summary>Gets or sets 出庫予定数量</summary>
            /// <value>出庫予定数量</value>
            public decimal? OutQty { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ユーザID</summary>
                /// <value>ユーザID</value>
                public string UserId { get; set; }
                /// <summary>Gets or sets 在庫区分</summary>
                /// <value>在庫区分</value>
                public long InventoryDivision { get; set; }
                /// <summary>Gets or sets 受払予定日</summary>
                /// <value>受払予定日</value>
                public DateTime InoutExpectedDate { get; set; }
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="userId">ユーザID</param>
                /// <param name="inventoryDivision">在庫区分</param>
                /// <param name="inoutExpectedDate">受払予定日</param>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                public PrimaryKey(string userId, long inventoryDivision, DateTime inoutExpectedDate, string itemCd, string specificationCd)
                {
                    UserId = userId;
                    InventoryDivision = inventoryDivision;
                    InoutExpectedDate = inoutExpectedDate;
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.UserId, this.InventoryDivision, this.InoutExpectedDate, this.ItemCd, this.SpecificationCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="userId">ユーザID</param>
            /// <param name="inventoryDivision">在庫区分</param>
            /// <param name="inoutExpectedDate">受払予定日</param>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InventoryBalanceEntity GetEntity(string userId, long inventoryDivision, DateTime inoutExpectedDate, string itemCd, string specificationCd, ComDB db)
            {
                InventoryBalanceEntity.PrimaryKey condition = new InventoryBalanceEntity.PrimaryKey(userId, inventoryDivision, inoutExpectedDate, itemCd, specificationCd);
                return GetEntity<InventoryBalanceEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 在庫バランスアラート
        /// </summary>
        public class InventoryBalanceAlertEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InventoryBalanceAlertEntity()
            {
                TableName = "inventory_balance_alert";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 在庫区分</summary>
            /// <value>在庫区分</value>
            public long InventoryDivision { get; set; }
            /// <summary>Gets or sets 受払予定日</summary>
            /// <value>受払予定日</value>
            public DateTime InoutExpectedDate { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets ロットNO</summary>
            /// <value>ロットNO</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>サブロット番号1</value>
            public string SubLotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>サブロット番号2</value>
            public string SubLotNo2 { get; set; }
            /// <summary>Gets or sets アラート種別</summary>
            /// <value>アラート種別</value>
            public int? AlertType { get; set; }
            /// <summary>Gets or sets アラート情報</summary>
            /// <value>アラート情報</value>
            public string AlertInfo { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ユーザID</summary>
                /// <value>ユーザID</value>
                public string UserId { get; set; }
                /// <summary>Gets or sets 在庫区分</summary>
                /// <value>在庫区分</value>
                public long InventoryDivision { get; set; }
                /// <summary>Gets or sets 受払予定日</summary>
                /// <value>受払予定日</value>
                public DateTime InoutExpectedDate { get; set; }
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="userId">ユーザID</param>
                /// <param name="inventoryDivision">在庫区分</param>
                /// <param name="inoutExpectedDate">受払予定日</param>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                public PrimaryKey(string userId, long inventoryDivision, DateTime inoutExpectedDate, string itemCd, string specificationCd)
                {
                    UserId = userId;
                    InventoryDivision = inventoryDivision;
                    InoutExpectedDate = inoutExpectedDate;
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.UserId, this.InventoryDivision, this.InoutExpectedDate, this.ItemCd, this.SpecificationCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="userId">ユーザID</param>
            /// <param name="inventoryDivision">在庫区分</param>
            /// <param name="inoutExpectedDate">受払予定日</param>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InventoryBalanceAlertEntity GetEntity(string userId, long inventoryDivision, DateTime inoutExpectedDate, string itemCd, string specificationCd, ComDB db)
            {
                InventoryBalanceAlertEntity.PrimaryKey condition = new InventoryBalanceAlertEntity.PrimaryKey(userId, inventoryDivision, inoutExpectedDate, itemCd, specificationCd);
                return GetEntity<InventoryBalanceAlertEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 在庫バランス用中間テーブル
        /// </summary>
        public class TemporaryInventoryBalanceEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TemporaryInventoryBalanceEntity()
            {
                TableName = "temporary_inventory_balance";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 在庫区分</summary>
            /// <value>在庫区分</value>
            public long InventoryDivision { get; set; }
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public long RowNo { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets ロケーション名称</summary>
            /// <value>ロケーション名称</value>
            public string LocationName { get; set; }
            /// <summary>Gets or sets 単位</summary>
            /// <value>単位</value>
            public string UnitName { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets サブロット番号1</summary>
            /// <value>サブロット番号1</value>
            public string SubLotNo1 { get; set; }
            /// <summary>Gets or sets サブロット番号2</summary>
            /// <value>サブロット番号2</value>
            public string SubLotNo2 { get; set; }
            /// <summary>Gets or sets 安全在庫数</summary>
            /// <value>安全在庫数</value>
            public string SafetyInventoryQty { get; set; }
            /// <summary>Gets or sets 最低在庫数</summary>
            /// <value>最低在庫数</value>
            public string MinInventoryQty { get; set; }
            /// <summary>Gets or sets 最高在庫数</summary>
            /// <value>最高在庫数</value>
            public string MaxInventoryQty { get; set; }
            /// <summary>Gets or sets 入荷単位数</summary>
            /// <value>入荷単位数</value>
            public string PurchaseUnitQty { get; set; }
            /// <summary>Gets or sets 入荷下限値</summary>
            /// <value>入荷下限値</value>
            public string MinPurchaseQty { get; set; }
            /// <summary>Gets or sets 入荷上限値</summary>
            /// <value>入荷上限値</value>
            public string MaxPurchaseQty { get; set; }
            /// <summary>Gets or sets 単位コード</summary>
            /// <value>単位コード</value>
            public string UnitCd { get; set; }
            /// <summary>Gets or sets 最大品目数</summary>
            /// <value>最大品目数</value>
            public long? MaxItemCount { get; set; }
            /// <summary>Gets or sets 日付</summary>
            /// <value>日付</value>
            public string InoutDate { get; set; }
            /// <summary>Gets or sets 前残</summary>
            /// <value>前残</value>
            public string PreviousRemainQty { get; set; }
            /// <summary>Gets or sets 入荷</summary>
            /// <value>入荷</value>
            public string InQty { get; set; }
            /// <summary>Gets or sets 払出</summary>
            /// <value>払出</value>
            public string OutQty { get; set; }
            /// <summary>Gets or sets 予定在庫</summary>
            /// <value>予定在庫</value>
            public string ExpectedQty { get; set; }
            /// <summary>Gets or sets 発注点</summary>
            /// <value>発注点</value>
            public string PurchaseLimit { get; set; }
            /// <summary>Gets or sets 休日フラグ</summary>
            /// <value>休日フラグ</value>
            public string HolidatyFlg { get; set; }
            /// <summary>Gets or sets 日付フラグ</summary>
            /// <value>日付フラグ</value>
            public string DateFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ユーザID</summary>
                /// <value>ユーザID</value>
                public string UserId { get; set; }
                /// <summary>Gets or sets 在庫区分</summary>
                /// <value>在庫区分</value>
                public long InventoryDivision { get; set; }
                /// <summary>Gets or sets 行番号</summary>
                /// <value>行番号</value>
                public long RowNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="userId">ユーザID</param>
                /// <param name="inventoryDivision">在庫区分</param>
                /// <param name="rowNo">行番号</param>
                public PrimaryKey(string userId, long inventoryDivision, long rowNo)
                {
                    UserId = userId;
                    InventoryDivision = inventoryDivision;
                    RowNo = rowNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.UserId, this.InventoryDivision, this.RowNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="userId">ユーザID</param>
            /// <param name="inventoryDivision">在庫区分</param>
            /// <param name="rowNo">行番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TemporaryInventoryBalanceEntity GetEntity(string userId, long inventoryDivision, long rowNo, ComDB db)
            {
                TemporaryInventoryBalanceEntity.PrimaryKey condition = new TemporaryInventoryBalanceEntity.PrimaryKey(userId, inventoryDivision, rowNo);
                return GetEntity<TemporaryInventoryBalanceEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 品目在庫(生産計画用)
        /// </summary>
        public class ItemInventoryFixedEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ItemInventoryFixedEntity()
            {
                TableName = "item_inventory_fixed";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 現在在庫数</summary>
            /// <value>現在在庫数</value>
            public decimal? InventoryQty { get; set; }
            /// <summary>Gets or sets 有効在庫数</summary>
            /// <value>有効在庫数</value>
            public decimal? AvailableQty { get; set; }
            /// <summary>Gets or sets 受注残数</summary>
            /// <value>受注残数</value>
            public decimal? SalesAssignQty { get; set; }
            /// <summary>Gets or sets 引当残数</summary>
            /// <value>引当残数</value>
            public decimal? AssignQty { get; set; }
            /// <summary>Gets or sets 移庫残数</summary>
            /// <value>移庫残数</value>
            public decimal? TransferQty { get; set; }
            /// <summary>Gets or sets 製造残数</summary>
            /// <value>製造残数</value>
            public decimal? ManufactureQty { get; set; }
            /// <summary>Gets or sets 発注残数</summary>
            /// <value>発注残数</value>
            public decimal? BackorderQty { get; set; }
            /// <summary>Gets or sets 引当残数（製造）</summary>
            /// <value>引当残数（製造）</value>
            public decimal? ManufactureAssignQty { get; set; }
            /// <summary>Gets or sets 引当残数（出荷）</summary>
            /// <value>引当残数（出荷）</value>
            public decimal? ShippingAssignQty { get; set; }
            /// <summary>Gets or sets 簿外在庫数</summary>
            /// <value>簿外在庫数</value>
            public decimal? OffTheBooksQty { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                public PrimaryKey(string itemCd, string specificationCd)
                {
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemCd, this.SpecificationCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public ItemInventoryFixedEntity GetEntity(string itemCd, string specificationCd, ComDB db)
            {
                ItemInventoryFixedEntity.PrimaryKey condition = new ItemInventoryFixedEntity.PrimaryKey(itemCd, specificationCd);
                return GetEntity<ItemInventoryFixedEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 受払ソース(生産計画用)
        /// </summary>
        public class InoutSourceFixedEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InoutSourceFixedEntity()
            {
                TableName = "inout_source_fixed";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 受払ソース番号</summary>
            /// <value>受払ソース番号</value>
            public string InoutSourceNo { get; set; }
            /// <summary>Gets or sets 受払区分|各種名称マスタ</summary>
            /// <value>受払区分|各種名称マスタ</value>
            public string InoutDivision { get; set; }
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
            /// <summary>Gets or sets 引当済フラグ|1</summary>
            /// <value>引当済フラグ|1</value>
            public int? AssignFlg { get; set; }
            /// <summary>Gets or sets 登録時メニュー番号</summary>
            /// <value>登録時メニュー番号</value>
            public string InputMenuId { get; set; }
            /// <summary>Gets or sets 登録時タブ番号</summary>
            /// <value>登録時タブ番号</value>
            public string InputDisplayId { get; set; }
            /// <summary>Gets or sets 登録時操作番号</summary>
            /// <value>登録時操作番号</value>
            public int? InputControlId { get; set; }
            /// <summary>Gets or sets 更新時メニュー番号</summary>
            /// <value>更新時メニュー番号</value>
            public string UpdateMenuId { get; set; }
            /// <summary>Gets or sets 更新時タブ番号</summary>
            /// <value>更新時タブ番号</value>
            public string UpdateDisplayId { get; set; }
            /// <summary>Gets or sets 更新時操作番号</summary>
            /// <value>更新時操作番号</value>
            public int? UpdateControlId { get; set; }
            /// <summary>Gets or sets オーダー先コード</summary>
            /// <value>オーダー先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets 親品目コード</summary>
            /// <value>親品目コード</value>
            public string ParentItemCd { get; set; }
            /// <summary>Gets or sets 親仕様コード</summary>
            /// <value>親仕様コード</value>
            public string ParentSpecificationCd { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 受払ソース番号</summary>
                /// <value>受払ソース番号</value>
                public string InoutSourceNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="inoutSourceNo">受払ソース番号</param>
                public PrimaryKey(string inoutSourceNo)
                {
                    InoutSourceNo = inoutSourceNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InoutSourceNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="inoutSourceNo">受払ソース番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InoutSourceFixedEntity GetEntity(string inoutSourceNo, ComDB db)
            {
                InoutSourceFixedEntity.PrimaryKey condition = new InoutSourceFixedEntity.PrimaryKey(inoutSourceNo);
                return GetEntity<InoutSourceFixedEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 月次受払ソース(生産計画用)
        /// </summary>
        public class MonthlyInoutSourceFixedEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MonthlyInoutSourceFixedEntity()
            {
                TableName = "monthly_inout_source_fixed";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 対象月</summary>
            /// <value>対象月</value>
            public int TargetMonth { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 当月入庫数</summary>
            /// <value>当月入庫数</value>
            public decimal? InQty { get; set; }
            /// <summary>Gets or sets 当月出庫数</summary>
            /// <value>当月出庫数</value>
            public decimal? OutQty { get; set; }
            /// <summary>Gets or sets 当月繰越在庫数</summary>
            /// <value>当月繰越在庫数</value>
            public decimal? NmInventoryQty { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 対象月</summary>
                /// <value>対象月</value>
                public int TargetMonth { get; set; }
                /// <summary>Gets or sets 品目コード</summary>
                /// <value>品目コード</value>
                public string ItemCd { get; set; }
                /// <summary>Gets or sets 仕様コード</summary>
                /// <value>仕様コード</value>
                public string SpecificationCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="targetMonth">対象月</param>
                /// <param name="itemCd">品目コード</param>
                /// <param name="specificationCd">仕様コード</param>
                public PrimaryKey(int targetMonth, string itemCd, string specificationCd)
                {
                    TargetMonth = targetMonth;
                    ItemCd = itemCd;
                    SpecificationCd = specificationCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.TargetMonth, this.ItemCd, this.SpecificationCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="targetMonth">対象月</param>
            /// <param name="itemCd">品目コード</param>
            /// <param name="specificationCd">仕様コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MonthlyInoutSourceFixedEntity GetEntity(int targetMonth, string itemCd, string specificationCd, ComDB db)
            {
                MonthlyInoutSourceFixedEntity.PrimaryKey condition = new MonthlyInoutSourceFixedEntity.PrimaryKey(targetMonth, itemCd, specificationCd);
                return GetEntity<MonthlyInoutSourceFixedEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 棚卸処理状況
        /// </summary>
        public class InventoryCountStatusEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public InventoryCountStatusEntity()
            {
                TableName = "inventory_count_status";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 勘定年月</summary>
            /// <value>勘定年月</value>
            public string AccountYears { get; set; }
            /// <summary>Gets or sets 棚卸データ作成日</summary>
            /// <value>棚卸データ作成日</value>
            public DateTime CountDate { get; set; }
            /// <summary>Gets or sets 循環棚卸区分|各種名称マスタ</summary>
            /// <value>循環棚卸区分|各種名称マスタ</value>
            public string CountDivision { get; set; }
            /// <summary>Gets or sets 棚卸処理ステータス</summary>
            /// <value>棚卸処理ステータス</value>
            public int? CountInventoryStatus { get; set; }
            /// <summary>Gets or sets 棚卸準備処理ステータス</summary>
            /// <value>棚卸準備処理ステータス</value>
            public int? CountPrepareStatus { get; set; }
            /// <summary>Gets or sets 棚卸準備開始日時</summary>
            /// <value>棚卸準備開始日時</value>
            public DateTime? CountPrepareStartDate { get; set; }
            /// <summary>Gets or sets 棚卸準備終了日時</summary>
            /// <value>棚卸準備終了日時</value>
            public DateTime? CountPrepareEndDate { get; set; }
            /// <summary>Gets or sets 棚卸準備キャンセルステータス</summary>
            /// <value>棚卸準備キャンセルステータス</value>
            public int? CountCancelStatus { get; set; }
            /// <summary>Gets or sets 棚卸準備キャンセル開始日時</summary>
            /// <value>棚卸準備キャンセル開始日時</value>
            public DateTime? CountCancelStartDate { get; set; }
            /// <summary>Gets or sets 棚卸準備キャンセル終了日時</summary>
            /// <value>棚卸準備キャンセル終了日時</value>
            public DateTime? CountCancelEndDate { get; set; }
            /// <summary>Gets or sets 棚卸ファイル出力処理ステータス</summary>
            /// <value>棚卸ファイル出力処理ステータス</value>
            public int? CountExportStatus { get; set; }
            /// <summary>Gets or sets 棚卸ファイル出力開始日時</summary>
            /// <value>棚卸ファイル出力開始日時</value>
            public DateTime? CountExportStartDate { get; set; }
            /// <summary>Gets or sets 棚卸ファイル出力終了日時</summary>
            /// <value>棚卸ファイル出力終了日時</value>
            public DateTime? CountExportEndDate { get; set; }
            /// <summary>Gets or sets 棚卸結果取込処理ステータス</summary>
            /// <value>棚卸結果取込処理ステータス</value>
            public int? CountImportStatus { get; set; }
            /// <summary>Gets or sets 棚卸結果取込開始日時</summary>
            /// <value>棚卸結果取込開始日時</value>
            public DateTime? CountImportStartDate { get; set; }
            /// <summary>Gets or sets 棚卸結果取込終了日時</summary>
            /// <value>棚卸結果取込終了日時</value>
            public DateTime? CountImportEndDate { get; set; }
            /// <summary>Gets or sets 棚卸更新処理ステータス</summary>
            /// <value>棚卸更新処理ステータス</value>
            public int? CountUpdateStatus { get; set; }
            /// <summary>Gets or sets 棚卸更新開始日時</summary>
            /// <value>棚卸更新開始日時</value>
            public DateTime? CountUpdateStartDate { get; set; }
            /// <summary>Gets or sets 棚卸更新終了日時</summary>
            /// <value>棚卸更新終了日時</value>
            public DateTime? CountUpdateEndDate { get; set; }
            /// <summary>Gets or sets 棚卸ロールバック処理ステータス</summary>
            /// <value>棚卸ロールバック処理ステータス</value>
            public int? CountRollBackStatus { get; set; }
            /// <summary>Gets or sets 棚卸ロールバック開始日時</summary>
            /// <value>棚卸ロールバック開始日時</value>
            public DateTime? CountRollBackStartDate { get; set; }
            /// <summary>Gets or sets 棚卸ロールバック終了日時</summary>
            /// <value>棚卸ロールバック終了日時</value>
            public DateTime? CountRollBackEndDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 勘定年月</summary>
                /// <value>勘定年月</value>
                public string AccountYears { get; set; }
                /// <summary>Gets or sets 棚卸データ作成日</summary>
                /// <value>棚卸データ作成日</value>
                public DateTime CountDate { get; set; }
                /// <summary>Gets or sets 循環棚卸区分|各種名称マスタ</summary>
                /// <value>循環棚卸区分|各種名称マスタ</value>
                public string CountDivision { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="accountYears">勘定年月</param>
                /// <param name="countDate">棚卸データ作成日</param>
                /// <param name="countDivision">循環棚卸区分</param>
                public PrimaryKey(string accountYears, DateTime countDate, string countDivision)
                {
                    AccountYears = accountYears;
                    CountDate = countDate;
                    CountDivision = countDivision;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.AccountYears, this.CountDate, this.CountDivision);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="accountYears">勘定年月</param>
            /// <param name="countDate">棚卸データ作成日</param>
            /// <param name="countDivision">循環棚卸区分</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public InventoryCountStatusEntity GetEntity(string accountYears, DateTime countDate, string countDivision, ComDB db)
            {
                InventoryCountStatusEntity.PrimaryKey condition = new InventoryCountStatusEntity.PrimaryKey(accountYears, countDate, countDivision);
                return GetEntity<InventoryCountStatusEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 設備マスタ
        /// </summary>
        public class RecipeResouceEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public RecipeResouceEntity()
            {
                TableName = "recipe_resouce";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 使用資源コード</summary>
            /// <value>使用資源コード</value>
            public string ResouceCd { get; set; }
            /// <summary>Gets or sets 使用資源名</summary>
            /// <value>使用資源名</value>
            public string ResouceName { get; set; }
            /// <summary>Gets or sets 略称</summary>
            /// <value>略称</value>
            public string ShortName { get; set; }
            /// <summary>Gets or sets 生産ライン</summary>
            /// <value>生産ライン</value>
            public string ProductionLine { get; set; }
            /// <summary>Gets or sets 指図書発行有無フラグ</summary>
            /// <value>指図書発行有無フラグ</value>
            public int? OrderPublishFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 使用資源コード</summary>
                /// <value>使用資源コード</value>
                public string ResouceCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="resouceCd">使用資源コード</param>
                public PrimaryKey(string resouceCd)
                {
                    ResouceCd = resouceCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ResouceCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="resouceCd">使用資源コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public RecipeResouceEntity GetEntity(string resouceCd, ComDB db)
            {
                RecipeResouceEntity.PrimaryKey condition = new RecipeResouceEntity.PrimaryKey(resouceCd);
                return GetEntity<RecipeResouceEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 相殺グループマスタ
        /// </summary>
        public class OffsetGroupEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public OffsetGroupEntity()
            {
                TableName = "offset_group";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 相殺グループコード</summary>
            /// <value>相殺グループコード</value>
            public string OffsetGroupCd { get; set; }
            /// <summary>Gets or sets 相殺グループ名称</summary>
            /// <value>相殺グループ名称</value>
            public string OffsetGroupName { get; set; }
            /// <summary>Gets or sets 取引先区分</summary>
            /// <value>取引先区分</value>
            public string VenderDivision { get; set; }
            /// <summary>Gets or sets 取引先コード</summary>
            /// <value>取引先コード</value>
            public string VenderCd { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 相殺グループコード</summary>
                /// <value>相殺グループコード</value>
                public string OffsetGroupCd { get; set; }
                /// <summary>Gets or sets 取引先区分</summary>
                /// <value>取引先区分</value>
                public string VenderDivision { get; set; }
                /// <summary>Gets or sets 取引先コード</summary>
                /// <value>取引先コード</value>
                public string VenderCd { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="offsetGroupCd">相殺グループコード</param>
                /// <param name="venderDivision">取引先区分</param>
                /// <param name="venderCd">取引先コード</param>
                public PrimaryKey(string offsetGroupCd, string venderDivision, string venderCd)
                {
                    OffsetGroupCd = offsetGroupCd;
                    VenderDivision = venderDivision;
                    VenderCd = venderCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.OffsetGroupCd, this.VenderDivision, this.VenderCd);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="offsetGroupCd">相殺グループコード</param>
            /// <param name="venderDivision">取引先区分</param>
            /// <param name="venderCd">取引先コード</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public OffsetGroupEntity GetEntity(string offsetGroupCd, string venderDivision, string venderCd, ComDB db)
            {
                OffsetGroupEntity.PrimaryKey condition = new OffsetGroupEntity.PrimaryKey(offsetGroupCd, venderDivision, venderCd);
                return GetEntity<OffsetGroupEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 通知情報
        /// </summary>
        public class NoticeInfoEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public NoticeInfoEntity()
            {
                TableName = "notice_info";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 通知番号</summary>
            /// <value>通知番号</value>
            public string NoticeNo { get; set; }
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 通知区分</summary>
            /// <value>通知区分</value>
            public int NoticeDivision { get; set; }
            /// <summary>Gets or sets 通知分類</summary>
            /// <value>通知分類</value>
            public string NoticeClass { get; set; }
            /// <summary>Gets or sets 通知内容</summary>
            /// <value>通知内容</value>
            public string NoticeContents { get; set; }
            /// <summary>Gets or sets キー番号</summary>
            /// <value>キー番号</value>
            public string KeyNo { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 通知番号</summary>
                /// <value>通知番号</value>
                public string NoticeNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="noticeNo">通知番号</param>
                public PrimaryKey(string noticeNo)
                {
                    NoticeNo = noticeNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.NoticeNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="noticeNo">通知番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public NoticeInfoEntity GetEntity(string noticeNo, ComDB db)
            {
                NoticeInfoEntity.PrimaryKey condition = new NoticeInfoEntity.PrimaryKey(noticeNo);
                return GetEntity<NoticeInfoEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// タスクメモ
        /// </summary>
        public class TaskMemoEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public TaskMemoEntity()
            {
                TableName = "task_memo";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets タスク番号</summary>
            /// <value>タスク番号</value>
            public string TaskNo { get; set; }
            /// <summary>Gets or sets タスク区分|各種名称マスタ</summary>
            /// <value>タスク区分|各種名称マスタ</value>
            public int TaskDivision { get; set; }
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets 期限年月日</summary>
            /// <value>期限年月日</value>
            public DateTime? LimitDate { get; set; }
            /// <summary>Gets or sets 期限時刻</summary>
            /// <value>期限時刻</value>
            public DateTime? LimitTime { get; set; }
            /// <summary>Gets or sets タスクメモ</summary>
            /// <value>タスクメモ</value>
            public string TaskMemo { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets タスク番号</summary>
                /// <value>タスク番号</value>
                public string TaskNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="taskNo">タスク番号</param>
                public PrimaryKey(string taskNo)
                {
                    TaskNo = taskNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.TaskNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="taskNo">タスク番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public TaskMemoEntity GetEntity(string taskNo, ComDB db)
            {
                TaskMemoEntity.PrimaryKey condition = new TaskMemoEntity.PrimaryKey(taskNo);
                return GetEntity<TaskMemoEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// お知らせ
        /// </summary>
        public class AnnouncementEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public AnnouncementEntity()
            {
                TableName = "announcement";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets お知らせ番号</summary>
            /// <value>お知らせ番号</value>
            public string AnnouncementNo { get; set; }
            /// <summary>Gets or sets お知らせ区分|各種名称マスタ</summary>
            /// <value>お知らせ区分|各種名称マスタ</value>
            public int AnnouncementDivision { get; set; }
            /// <summary>Gets or sets 掲載開始日時</summary>
            /// <value>掲載開始日時</value>
            public DateTime DispStartDate { get; set; }
            /// <summary>Gets or sets 掲載終了日時</summary>
            /// <value>掲載終了日時</value>
            public DateTime? DispEndDate { get; set; }
            /// <summary>Gets or sets 優先順位</summary>
            /// <value>優先順位</value>
            public int? Priority { get; set; }
            /// <summary>Gets or sets 内容</summary>
            /// <value>内容</value>
            public string Content { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DelFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets お知らせ番号</summary>
                /// <value>お知らせ番号</value>
                public string AnnouncementNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="announcementNo">お知らせ番号</param>
                public PrimaryKey(string announcementNo)
                {
                    AnnouncementNo = announcementNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.AnnouncementNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="announcementNo">お知らせ番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public AnnouncementEntity GetEntity(string announcementNo, ComDB db)
            {
                AnnouncementEntity.PrimaryKey condition = new AnnouncementEntity.PrimaryKey(announcementNo);
                return GetEntity<AnnouncementEntity>(this.TableName, condition, db);
            }
        }

        /// <summary>
        /// 月次締処理制御
        /// </summary>
        public class MonthlyCloseControlEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MonthlyCloseControlEntity()
            {
                TableName = "monthly_close_control";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機能ID</summary>
            /// <value>機能ID</value>
            public string Conductid { get; set; }
            /// <summary>Gets or sets 画面NO</summary>
            /// <value>画面NO</value>
            public int Formno { get; set; }
            /// <summary>Gets or sets コントロールID</summary>
            /// <value>コントロールID</value>
            public string Ctrlid { get; set; }
            /// <summary>Gets or sets 定義種類</summary>
            /// <value>定義種類</value>
            public int Definetype { get; set; }
            /// <summary>Gets or sets 項目番号</summary>
            /// <value>項目番号</value>
            public int Itemno { get; set; }
            /// <summary>Gets or sets 締処理区分</summary>
            /// <value>締処理区分</value>
            public int? CloseDivision { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機能ID</summary>
                /// <value>機能ID</value>
                public string Conductid { get; set; }
                /// <summary>Gets or sets 画面NO</summary>
                /// <value>画面NO</value>
                public int Formno { get; set; }
                /// <summary>Gets or sets コントロールID</summary>
                /// <value>コントロールID</value>
                public string Ctrlid { get; set; }
                /// <summary>Gets or sets 定義種類</summary>
                /// <value>定義種類</value>
                public int Definetype { get; set; }
                /// <summary>Gets or sets 項目番号</summary>
                /// <value>項目番号</value>
                public int Itemno { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="conductid">機能ID</param>
                /// <param name="formno">画面NO</param>
                /// <param name="ctrlid">コントロールID</param>
                /// <param name="definetype">定義種類</param>
                /// <param name="itemno">項目番号</param>
                public PrimaryKey(string conductid, int formno, string ctrlid, int definetype, int itemno)
                {
                    Conductid = conductid;
                    Formno = formno;
                    Ctrlid = ctrlid;
                    Definetype = definetype;
                    Itemno = itemno;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.Conductid, this.Formno, this.Ctrlid, this.Definetype, this.Itemno);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <param name="conductid">機能ID</param>
            /// <param name="formno">画面NO</param>
            /// <param name="ctrlid">コントロールID</param>
            /// <param name="definetype">定義種類</param>
            /// <param name="itemno">項目番号</param>
            /// <param name="db">DB操作クラス</param>
            /// <returns>該当のデータを返す</returns>
            public MonthlyCloseControlEntity GetEntity(string conductid, int formno, string ctrlid, int definetype, int itemno, ComDB db)
            {
                MonthlyCloseControlEntity.PrimaryKey condition = new MonthlyCloseControlEntity.PrimaryKey(conductid, formno, ctrlid, definetype, itemno);
                return GetEntity<MonthlyCloseControlEntity>(this.TableName, condition, db);
            }
        }

    }
}