using CommonWebTemplate.CommonDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonSTDUtil.CommonSTDUtil
{
    public class CommonSTDUtillDataClass : CommonDataBaseClass
    {
        /// <summary>
        /// 名称マスタ
        /// </summary>
        public class AutoCompleteEntity
        {

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public string FactoryId { get; set; }
            /// <summary>Gets or sets 翻訳工場ID</summary>
            /// <value>翻訳工場ID</value>
            public string TranslationFactoryId { get; set; }
            /// <summary>Gets or sets 表示順用工場ID</summary>
            /// <value>表示順用工場ID</value>
            public string OrderFactoryId { get; set; }
            /// <summary>Gets or sets バリュー</summary>
            /// <value>バリュー</value>
            public string Values { get; set; }
            /// <summary>Gets or sets ラベル</summary>
            /// <value>ラベル</value>
            public string Labels { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public string DeleteFlg { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam1 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam2 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam3 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam4 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam5 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam6 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam7 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam8 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam9 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam10 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam11 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam12 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam13 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam14 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam15 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam16 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam17 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam18 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam19 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam20 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam21 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam22 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam23 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam24 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam25 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam26 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam27 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam28 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam29 { get; set; }
            /// <summary>Gets or sets 拡張項目</summary>
            /// <value>拡張項目</value>
            public string Exparam30 { get; set; }

        }

        /// <summary>
        /// 名称マスタ
        /// </summary>
        public class NamesEntity
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
            /// <summary>Gets or sets 付帯情報1</summary>
            /// <value>付帯情報1</value>
            public string ExtendInfo1 { get; set; }
            /// <summary>Gets or sets 付帯情報2</summary>
            /// <value>付帯情報2</value>
            public string ExtendInfo2 { get; set; }
            /// <summary>Gets or sets 付帯情報3</summary>
            /// <value>付帯情報3</value>
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
                public PrimaryKey(string nameDivision, string nameCd)
                {
                    NameDivision = nameDivision;
                    NameCd = nameCd;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.NameDivision, this.NameCd);
                return pk;
            }
        }

        /// <summary>
        /// ログインマスタ
        /// </summary>
        public class LoginEntity
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
            public int UserId { get; set; }
            /// <summary>Gets or sets ログインID</summary>
            /// <value>ログインID</value>
            public string LoginId { get; set; }
            /// <summary>Gets or sets ユーザー名</summary>
            /// <value>ユーザー名</value>
            public string UserName { get; set; }
            /// <summary>Gets or sets パスワード</summary>
            /// <value>パスワード</value>
            public string Password { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
            /// <summary>
            /// 権限レベルID
            /// </summary>
            public int AuthorityLevelId { get; set; }
            /// <summary>Gets or sets 有効フラグ</summary>
            /// <value>有効フラグ</value>
            public string ActiveFlg { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public string DelFlg { get; set; }
            /// <summary>Gets or sets 管理者フラグ</summary>
            /// <value>管理者フラグ</value>
            public string AdminFlg { get; set; }
            /// <summary>Gets or sets 更新パスワード</summary>
            /// <value>更新パスワード</value>
            public DateTime? UpdatePass { get; set; }
            /// <summary>Gets or sets テストユーザフラグ</summary>
            /// <value>テストユーザフラグ</value>
            public string TestUserFlg { get; set; }
            /// <summary>Gets or sets メールアドレス</summary>
            /// <value>メールアドレス</value>
            public string MailAddress { get; set; }
            /// <summary>Gets or sets 登録日</summary>
            /// <value>登録日</value>
            public DateTime? InputDate { get; set; }
            /// <summary>Gets or sets 登録ユーザID</summary>
            /// <value>登録ユーザID</value>
            public string InputUserId { get; set; }
            /// <summary>Gets or sets 更新日</summary>
            /// <value>更新日</value>
            public DateTime? UpdateDate { get; set; }
            /// <summary>Gets or sets 更新ユーザID</summary>
            /// <value>更新ユーザID</value>
            public string UpdateUserId { get; set; }



            ///// <summary>
            ///// プライマリーキー
            ///// </summary>
            //public class PrimaryKey
            //{
            //    /// <summary>Gets or sets ユーザーID</summary>
            //    /// <value>ユーザーID</value>
            //    public string Userid { get; set; }
            //    /// <summary>Gets or sets コンダクトID</summary>
            //    /// <value>コンダクトID</value>
            //    public string Conductid { get; set; }
            //    /// <summary>Gets or sets 部署コード</summary>
            //    /// <value>部署コード</value>
            //    public string Bushocode { get; set; }
            //    /// <summary>Gets or sets 権限区分</summary>
            //    /// <value>権限区分</value>
            //    public string Authkbn { get; set; }
            //    /// <summary>
            //    /// コンストラクタ
            //    /// </summary>
            //    public PrimaryKey(string userid, string conductid, string bushocode, string authkbn)
            //    {
            //        Userid = userid;
            //        Conductid = conductid;
            //        Bushocode = bushocode;
            //        Authkbn = authkbn;
            //    }
            //}

            ///// <summary>
            ///// プライマリーキー情報
            ///// </summary>
            ///// <returns>プライマリーキー情報</returns>
            //public PrimaryKey PK()
            //{
            //    PrimaryKey pk = new PrimaryKey(this.Userid, this.Conductid, this.Bushocode, this.Authkbn);
            //    return pk;
            //}

            ///// <summary>
            ///// エンティティ
            ///// </summary>
            ///// <returns>該当のデータを返す</returns>
            //public ComUserAuthEntity GetEntity(string userId, string conductId, string bushoCode, string authKbn ,ComDB db)
            //{
            //    ComUserAuthEntity.PrimaryKey condition = new ComUserAuthEntity.PrimaryKey(userId, conductId, bushoCode, authKbn);
            //    // SQL文生成
            //    string getEntitySql = getEntity(this.TableName, condition, db);
            //    if (string.IsNullOrEmpty(getEntitySql))
            //    {
            //        return null;
            //    }
            //    return db.GetEntityByDataClass<ComUserAuthEntity>(getEntitySql);
            //}
        }

        /// <summary>
        /// 所属マスタ
        /// </summary>
        public class BelongEntity
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
            public decimal? PostId { get; set; }
            /// <summary>Gets or sets 所属区分|各種名称マスタ</summary>
            /// <value>所属区分|各種名称マスタ</value>
            public string BelongDivision { get; set; }
            /// <summary>Gets or sets 登録日時</summary>
            /// <value>登録日時</value>
            public DateTime? InputDate { get; set; }
            /// <summary>Gets or sets 登録者ID</summary>
            /// <value>登録者ID</value>
            public string InputUserId { get; set; }
            /// <summary>Gets or sets 更新日時</summary>
            /// <value>更新日時</value>
            public DateTime? UpdateDate { get; set; }
            /// <summary>Gets or sets 更新者ID</summary>
            /// <value>更新者ID</value>
            public string UpdateUserId { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int? DelFlg { get; set; }



            ///// <summary>
            ///// プライマリーキー
            ///// </summary>
            //public class PrimaryKey
            //{
            //    /// <summary>Gets or sets ユーザーID</summary>
            //    /// <value>ユーザーID</value>
            //    public string Userid { get; set; }
            //    /// <summary>Gets or sets コンダクトID</summary>
            //    /// <value>コンダクトID</value>
            //    public string Conductid { get; set; }
            //    /// <summary>Gets or sets 部署コード</summary>
            //    /// <value>部署コード</value>
            //    public string Bushocode { get; set; }
            //    /// <summary>Gets or sets 権限区分</summary>
            //    /// <value>権限区分</value>
            //    public string Authkbn { get; set; }
            //    /// <summary>
            //    /// コンストラクタ
            //    /// </summary>
            //    public PrimaryKey(string userid, string conductid, string bushocode, string authkbn)
            //    {
            //        Userid = userid;
            //        Conductid = conductid;
            //        Bushocode = bushocode;
            //        Authkbn = authkbn;
            //    }
            //}

            ///// <summary>
            ///// プライマリーキー情報
            ///// </summary>
            ///// <returns>プライマリーキー情報</returns>
            //public PrimaryKey PK()
            //{
            //    PrimaryKey pk = new PrimaryKey(this.Userid, this.Conductid, this.Bushocode, this.Authkbn);
            //    return pk;
            //}

            ///// <summary>
            ///// エンティティ
            ///// </summary>
            ///// <returns>該当のデータを返す</returns>
            //public ComUserAuthEntity GetEntity(string userId, string conductId, string bushoCode, string authKbn ,ComDB db)
            //{
            //    ComUserAuthEntity.PrimaryKey condition = new ComUserAuthEntity.PrimaryKey(userId, conductId, bushoCode, authKbn);
            //    // SQL文生成
            //    string getEntitySql = getEntity(this.TableName, condition, db);
            //    if (string.IsNullOrEmpty(getEntitySql))
            //    {
            //        return null;
            //    }
            //    return db.GetEntityByDataClass<ComUserAuthEntity>(getEntitySql);
            //}
        }

        /// <summary>
        /// ユーザー権限マスタ
        /// </summary>
        public class ComUserAuthEntity
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ComUserAuthEntity()
            {
                TableName = "com_user_auth";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string Userid { get; set; }
            /// <summary>Gets or sets コンダクトID</summary>
            /// <value>コンダクトID</value>
            public string Conductid { get; set; }
            /// <summary>Gets or sets 部署コード</summary>
            /// <value>部署コード</value>
            public string Bushocode { get; set; }
            /// <summary>Gets or sets 権限区分</summary>
            /// <value>権限区分</value>
            public string Authkbn { get; set; }
            /// <summary>Gets or sets 登録日</summary>
            /// <value>登録日</value>
            public decimal? Inpymd { get; set; }
            /// <summary>Gets or sets 登録時</summary>
            /// <value>登録時</value>
            public decimal? Inphmis { get; set; }
            /// <summary>Gets or sets 登録ID</summary>
            /// <value>登録ID</value>
            public string Inpid { get; set; }
            /// <summary>Gets or sets 登録端末</summary>
            /// <value>登録端末</value>
            public string Inpterminal { get; set; }
            /// <summary>Gets or sets 登録コンダクトID</summary>
            /// <value>登録コンダクトID</value>
            public string Inpconductid { get; set; }
            /// <summary>Gets or sets 更新日</summary>
            /// <value>更新日</value>
            public decimal? Updymd { get; set; }
            /// <summary>Gets or sets 更新時</summary>
            /// <value>更新時</value>
            public decimal? Updhmis { get; set; }
            /// <summary>Gets or sets 更新ID</summary>
            /// <value>更新ID</value>
            public string Updid { get; set; }
            /// <summary>Gets or sets 更新端末</summary>
            /// <value>更新端末</value>
            public string Updterminal { get; set; }
            /// <summary>Gets or sets 更新コンダクトID</summary>
            /// <value>更新コンダクトID</value>
            public string Updconductid { get; set; }


            ///// <summary>
            ///// プライマリーキー
            ///// </summary>
            //public class PrimaryKey
            //{
            //    /// <summary>Gets or sets ユーザーID</summary>
            //    /// <value>ユーザーID</value>
            //    public string Userid { get; set; }
            //    /// <summary>Gets or sets コンダクトID</summary>
            //    /// <value>コンダクトID</value>
            //    public string Conductid { get; set; }
            //    /// <summary>Gets or sets 部署コード</summary>
            //    /// <value>部署コード</value>
            //    public string Bushocode { get; set; }
            //    /// <summary>Gets or sets 権限区分</summary>
            //    /// <value>権限区分</value>
            //    public string Authkbn { get; set; }
            //    /// <summary>
            //    /// コンストラクタ
            //    /// </summary>
            //    public PrimaryKey(string userid, string conductid, string bushocode, string authkbn)
            //    {
            //        Userid = userid;
            //        Conductid = conductid;
            //        Bushocode = bushocode;
            //        Authkbn = authkbn;
            //    }
            //}

            ///// <summary>
            ///// プライマリーキー情報
            ///// </summary>
            ///// <returns>プライマリーキー情報</returns>
            //public PrimaryKey PK()
            //{
            //    PrimaryKey pk = new PrimaryKey(this.Userid, this.Conductid, this.Bushocode, this.Authkbn);
            //    return pk;
            //}

            ///// <summary>
            ///// エンティティ
            ///// </summary>
            ///// <returns>該当のデータを返す</returns>
            //public ComUserAuthEntity GetEntity(string userId, string conductId, string bushoCode, string authKbn ,ComDB db)
            //{
            //    ComUserAuthEntity.PrimaryKey condition = new ComUserAuthEntity.PrimaryKey(userId, conductId, bushoCode, authKbn);
            //    // SQL文生成
            //    string getEntitySql = getEntity(this.TableName, condition, db);
            //    if (string.IsNullOrEmpty(getEntitySql))
            //    {
            //        return null;
            //    }
            //    return db.GetEntityByDataClass<ComUserAuthEntity>(getEntitySql);
            //}
        }

        /// <summary>
        /// 一時データテーブル
        /// </summary>
        public class ComTempDataEntity
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ComTempDataEntity()
            {
                TableName = "com_temp_data";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets GUID</summary>
            /// <value>GUID</value>
            public string Guid { get; set; }
            /// <summary>Gets or sets タブ管理№</summary>
            /// <value>タブ管理№</value>
            public string Tabno { get; set; }
            /// <summary>Gets or sets 行№</summary>
            /// <value>行№</value>
            public decimal? Rowno { get; set; }
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public string UserId { get; set; }
            /// <summary>Gets or sets コントロールID</summary>
            /// <value>コントロールID</value>
            public string Ctrlid { get; set; }
            /// <summary>Gets or sets 行ステータス</summary>
            /// <value>行ステータス</value>
            public decimal? Rowstatus { get; set; }
            /// <summary>Gets or sets 更新タグ</summary>
            /// <value>更新タグ</value>
            public string Updtag { get; set; }
            /// <summary>Gets or sets ロック情報</summary>
            /// <value>ロック情報</value>
            public string Lockdata { get; set; }
            /// <summary>Gets or sets 選択タグ</summary>
            /// <value>選択タグ</value>
            public string Seltag { get; set; }
            /// <summary>Gets or sets 値1</summary>
            /// <value>値1</value>
            public string Val1 { get; set; }
            /// <summary>Gets or sets 値2</summary>
            /// <value>値2</value>
            public string Val2 { get; set; }
            /// <summary>Gets or sets 値3</summary>
            /// <value>値3</value>
            public string Val3 { get; set; }
            /// <summary>Gets or sets 値4</summary>
            /// <value>値4</value>
            public string Val4 { get; set; }
            /// <summary>Gets or sets 値5</summary>
            /// <value>値5</value>
            public string Val5 { get; set; }
            /// <summary>Gets or sets 値6</summary>
            /// <value>値6</value>
            public string Val6 { get; set; }
            /// <summary>Gets or sets 値7</summary>
            /// <value>値7</value>
            public string Val7 { get; set; }
            /// <summary>Gets or sets 値8</summary>
            /// <value>値8</value>
            public string Val8 { get; set; }
            /// <summary>Gets or sets 値9</summary>
            /// <value>値9</value>
            public string Val9 { get; set; }
            /// <summary>Gets or sets 値10</summary>
            /// <value>値10</value>
            public string Val10 { get; set; }
            /// <summary>Gets or sets 値11</summary>
            /// <value>値11</value>
            public string Val11 { get; set; }
            /// <summary>Gets or sets 値12</summary>
            /// <value>値12</value>
            public string Val12 { get; set; }
            /// <summary>Gets or sets 値13</summary>
            /// <value>値13</value>
            public string Val13 { get; set; }
            /// <summary>Gets or sets 値14</summary>
            /// <value>値14</value>
            public string Val14 { get; set; }
            /// <summary>Gets or sets 値15</summary>
            /// <value>値15</value>
            public string Val15 { get; set; }
            /// <summary>Gets or sets 値16</summary>
            /// <value>値16</value>
            public string Val16 { get; set; }
            /// <summary>Gets or sets 値17</summary>
            /// <value>値17</value>
            public string Val17 { get; set; }
            /// <summary>Gets or sets 値18</summary>
            /// <value>値18</value>
            public string Val18 { get; set; }
            /// <summary>Gets or sets 値19</summary>
            /// <value>値19</value>
            public string Val19 { get; set; }
            /// <summary>Gets or sets 値20</summary>
            /// <value>値20</value>
            public string Val20 { get; set; }
            /// <summary>Gets or sets 値21</summary>
            /// <value>値21</value>
            public string Val21 { get; set; }
            /// <summary>Gets or sets 値22</summary>
            /// <value>値22</value>
            public string Val22 { get; set; }
            /// <summary>Gets or sets 値23</summary>
            /// <value>値23</value>
            public string Val23 { get; set; }
            /// <summary>Gets or sets 値24</summary>
            /// <value>値24</value>
            public string Val24 { get; set; }
            /// <summary>Gets or sets 値25</summary>
            /// <value>値25</value>
            public string Val25 { get; set; }
            /// <summary>Gets or sets 値26</summary>
            /// <value>値26</value>
            public string Val26 { get; set; }
            /// <summary>Gets or sets 値27</summary>
            /// <value>値27</value>
            public string Val27 { get; set; }
            /// <summary>Gets or sets 値28</summary>
            /// <value>値28</value>
            public string Val28 { get; set; }
            /// <summary>Gets or sets 値29</summary>
            /// <value>値29</value>
            public string Val29 { get; set; }
            /// <summary>Gets or sets 値30</summary>
            /// <value>値30</value>
            public string Val30 { get; set; }
            /// <summary>Gets or sets 値31</summary>
            /// <value>値31</value>
            public string Val31 { get; set; }
            /// <summary>Gets or sets 値32</summary>
            /// <value>値32</value>
            public string Val32 { get; set; }
            /// <summary>Gets or sets 値33</summary>
            /// <value>値33</value>
            public string Val33 { get; set; }
            /// <summary>Gets or sets 値34</summary>
            /// <value>値34</value>
            public string Val34 { get; set; }
            /// <summary>Gets or sets 値35</summary>
            /// <value>値35</value>
            public string Val35 { get; set; }
            /// <summary>Gets or sets 値36</summary>
            /// <value>値36</value>
            public string Val36 { get; set; }
            /// <summary>Gets or sets 値37</summary>
            /// <value>値37</value>
            public string Val37 { get; set; }
            /// <summary>Gets or sets 値38</summary>
            /// <value>値38</value>
            public string Val38 { get; set; }
            /// <summary>Gets or sets 値39</summary>
            /// <value>値39</value>
            public string Val39 { get; set; }
            /// <summary>Gets or sets 値40</summary>
            /// <value>値40</value>
            public string Val40 { get; set; }
            /// <summary>Gets or sets 値41</summary>
            /// <value>値41</value>
            public string Val41 { get; set; }
            /// <summary>Gets or sets 値42</summary>
            /// <value>値42</value>
            public string Val42 { get; set; }
            /// <summary>Gets or sets 値43</summary>
            /// <value>値43</value>
            public string Val43 { get; set; }
            /// <summary>Gets or sets 値44</summary>
            /// <value>値44</value>
            public string Val44 { get; set; }
            /// <summary>Gets or sets 値45</summary>
            /// <value>値45</value>
            public string Val45 { get; set; }
            /// <summary>Gets or sets 値46</summary>
            /// <value>値46</value>
            public string Val46 { get; set; }
            /// <summary>Gets or sets 値47</summary>
            /// <value>値47</value>
            public string Val47 { get; set; }
            /// <summary>Gets or sets 値48</summary>
            /// <value>値48</value>
            public string Val48 { get; set; }
            /// <summary>Gets or sets 値49</summary>
            /// <value>値49</value>
            public string Val49 { get; set; }
            /// <summary>Gets or sets 値50</summary>
            /// <value>値50</value>
            public string Val50 { get; set; }
            /// <summary>Gets or sets 値51</summary>
            /// <value>値51</value>
            public string Val51 { get; set; }
            /// <summary>Gets or sets 値52</summary>
            /// <value>値52</value>
            public string Val52 { get; set; }
            /// <summary>Gets or sets 値53</summary>
            /// <value>値53</value>
            public string Val53 { get; set; }
            /// <summary>Gets or sets 値54</summary>
            /// <value>値54</value>
            public string Val54 { get; set; }
            /// <summary>Gets or sets 値55</summary>
            /// <value>値55</value>
            public string Val55 { get; set; }
            /// <summary>Gets or sets 値56</summary>
            /// <value>値56</value>
            public string Val56 { get; set; }
            /// <summary>Gets or sets 値57</summary>
            /// <value>値57</value>
            public string Val57 { get; set; }
            /// <summary>Gets or sets 値58</summary>
            /// <value>値58</value>
            public string Val58 { get; set; }
            /// <summary>Gets or sets 値59</summary>
            /// <value>値59</value>
            public string Val59 { get; set; }
            /// <summary>Gets or sets 値60</summary>
            /// <value>値60</value>
            public string Val60 { get; set; }
            /// <summary>Gets or sets 値61</summary>
            /// <value>値61</value>
            public string Val61 { get; set; }
            /// <summary>Gets or sets 値62</summary>
            /// <value>値62</value>
            public string Val62 { get; set; }
            /// <summary>Gets or sets 値63</summary>
            /// <value>値63</value>
            public string Val63 { get; set; }
            /// <summary>Gets or sets 値64</summary>
            /// <value>値64</value>
            public string Val64 { get; set; }
            /// <summary>Gets or sets 値65</summary>
            /// <value>値65</value>
            public string Val65 { get; set; }
            /// <summary>Gets or sets 値66</summary>
            /// <value>値66</value>
            public string Val66 { get; set; }
            /// <summary>Gets or sets 値67</summary>
            /// <value>値67</value>
            public string Val67 { get; set; }
            /// <summary>Gets or sets 値68</summary>
            /// <value>値68</value>
            public string Val68 { get; set; }
            /// <summary>Gets or sets 値69</summary>
            /// <value>値69</value>
            public string Val69 { get; set; }
            /// <summary>Gets or sets 値70</summary>
            /// <value>値70</value>
            public string Val70 { get; set; }
            /// <summary>Gets or sets 値71</summary>
            /// <value>値71</value>
            public string Val71 { get; set; }
            /// <summary>Gets or sets 値72</summary>
            /// <value>値72</value>
            public string Val72 { get; set; }
            /// <summary>Gets or sets 値73</summary>
            /// <value>値73</value>
            public string Val73 { get; set; }
            /// <summary>Gets or sets 値74</summary>
            /// <value>値74</value>
            public string Val74 { get; set; }
            /// <summary>Gets or sets 値75</summary>
            /// <value>値75</value>
            public string Val75 { get; set; }
            /// <summary>Gets or sets 値76</summary>
            /// <value>値76</value>
            public string Val76 { get; set; }
            /// <summary>Gets or sets 値77</summary>
            /// <value>値77</value>
            public string Val77 { get; set; }
            /// <summary>Gets or sets 値78</summary>
            /// <value>値78</value>
            public string Val78 { get; set; }
            /// <summary>Gets or sets 値79</summary>
            /// <value>値79</value>
            public string Val79 { get; set; }
            /// <summary>Gets or sets 値80</summary>
            /// <value>値80</value>
            public string Val80 { get; set; }
            /// <summary>Gets or sets 値81</summary>
            /// <value>値81</value>
            public string Val81 { get; set; }
            /// <summary>Gets or sets 値82</summary>
            /// <value>値82</value>
            public string Val82 { get; set; }
            /// <summary>Gets or sets 値83</summary>
            /// <value>値83</value>
            public string Val83 { get; set; }
            /// <summary>Gets or sets 値84</summary>
            /// <value>値84</value>
            public string Val84 { get; set; }
            /// <summary>Gets or sets 値85</summary>
            /// <value>値85</value>
            public string Val85 { get; set; }
            /// <summary>Gets or sets 値86</summary>
            /// <value>値86</value>
            public string Val86 { get; set; }
            /// <summary>Gets or sets 値87</summary>
            /// <value>値87</value>
            public string Val87 { get; set; }
            /// <summary>Gets or sets 値88</summary>
            /// <value>値88</value>
            public string Val88 { get; set; }
            /// <summary>Gets or sets 値89</summary>
            /// <value>値89</value>
            public string Val89 { get; set; }
            /// <summary>Gets or sets 値90</summary>
            /// <value>値90</value>
            public string Val90 { get; set; }
            /// <summary>Gets or sets 値91</summary>
            /// <value>値91</value>
            public string Val91 { get; set; }
            /// <summary>Gets or sets 値92</summary>
            /// <value>値92</value>
            public string Val92 { get; set; }
            /// <summary>Gets or sets 値93</summary>
            /// <value>値93</value>
            public string Val93 { get; set; }
            /// <summary>Gets or sets 値94</summary>
            /// <value>値94</value>
            public string Val94 { get; set; }
            /// <summary>Gets or sets 値95</summary>
            /// <value>値95</value>
            public string Val95 { get; set; }
            /// <summary>Gets or sets 値96</summary>
            /// <value>値96</value>
            public string Val96 { get; set; }
            /// <summary>Gets or sets 値97</summary>
            /// <value>値97</value>
            public string Val97 { get; set; }
            /// <summary>Gets or sets 値98</summary>
            /// <value>値98</value>
            public string Val98 { get; set; }
            /// <summary>Gets or sets 値99</summary>
            /// <value>値99</value>
            public string Val99 { get; set; }
            /// <summary>Gets or sets 値100</summary>
            /// <value>値100</value>
            public string Val100 { get; set; }
            /// <summary>Gets or sets 値101</summary>
            /// <value>値101</value>
            public string Val101 { get; set; }
            /// <summary>Gets or sets 値102</summary>
            /// <value>値102</value>
            public string Val102 { get; set; }
            /// <summary>Gets or sets 値103</summary>
            /// <value>値103</value>
            public string Val103 { get; set; }
            /// <summary>Gets or sets 値104</summary>
            /// <value>値104</value>
            public string Val104 { get; set; }
            /// <summary>Gets or sets 値105</summary>
            /// <value>値105</value>
            public string Val105 { get; set; }
            /// <summary>Gets or sets 値106</summary>
            /// <value>値106</value>
            public string Val106 { get; set; }
            /// <summary>Gets or sets 値107</summary>
            /// <value>値107</value>
            public string Val107 { get; set; }
            /// <summary>Gets or sets 値108</summary>
            /// <value>値108</value>
            public string Val108 { get; set; }
            /// <summary>Gets or sets 値109</summary>
            /// <value>値109</value>
            public string Val109 { get; set; }
            /// <summary>Gets or sets 値110</summary>
            /// <value>値110</value>
            public string Val110 { get; set; }
            /// <summary>Gets or sets 値111</summary>
            /// <value>値111</value>
            public string Val111 { get; set; }
            /// <summary>Gets or sets 値112</summary>
            /// <value>値112</value>
            public string Val112 { get; set; }
            /// <summary>Gets or sets 値113</summary>
            /// <value>値113</value>
            public string Val113 { get; set; }
            /// <summary>Gets or sets 値114</summary>
            /// <value>値114</value>
            public string Val114 { get; set; }
            /// <summary>Gets or sets 値115</summary>
            /// <value>値115</value>
            public string Val115 { get; set; }
            /// <summary>Gets or sets 値116</summary>
            /// <value>値116</value>
            public string Val116 { get; set; }
            /// <summary>Gets or sets 値117</summary>
            /// <value>値117</value>
            public string Val117 { get; set; }
            /// <summary>Gets or sets 値118</summary>
            /// <value>値118</value>
            public string Val118 { get; set; }
            /// <summary>Gets or sets 値119</summary>
            /// <value>値119</value>
            public string Val119 { get; set; }
            /// <summary>Gets or sets 値120</summary>
            /// <value>値120</value>
            public string Val120 { get; set; }
            /// <summary>Gets or sets 値121</summary>
            /// <value>値121</value>
            public string Val121 { get; set; }
            /// <summary>Gets or sets 値122</summary>
            /// <value>値122</value>
            public string Val122 { get; set; }
            /// <summary>Gets or sets 値123</summary>
            /// <value>値123</value>
            public string Val123 { get; set; }
            /// <summary>Gets or sets 値124</summary>
            /// <value>値124</value>
            public string Val124 { get; set; }
            /// <summary>Gets or sets 値125</summary>
            /// <value>値125</value>
            public string Val125 { get; set; }
            /// <summary>Gets or sets 値126</summary>
            /// <value>値126</value>
            public string Val126 { get; set; }
            /// <summary>Gets or sets 値127</summary>
            /// <value>値127</value>
            public string Val127 { get; set; }
            /// <summary>Gets or sets 値128</summary>
            /// <value>値128</value>
            public string Val128 { get; set; }
            /// <summary>Gets or sets 値129</summary>
            /// <value>値129</value>
            public string Val129 { get; set; }
            /// <summary>Gets or sets 値130</summary>
            /// <value>値130</value>
            public string Val130 { get; set; }
            /// <summary>Gets or sets 値131</summary>
            /// <value>値131</value>
            public string Val131 { get; set; }
            /// <summary>Gets or sets 値132</summary>
            /// <value>値132</value>
            public string Val132 { get; set; }
            /// <summary>Gets or sets 値133</summary>
            /// <value>値133</value>
            public string Val133 { get; set; }
            /// <summary>Gets or sets 値134</summary>
            /// <value>値134</value>
            public string Val134 { get; set; }
            /// <summary>Gets or sets 値135</summary>
            /// <value>値135</value>
            public string Val135 { get; set; }
            /// <summary>Gets or sets 値136</summary>
            /// <value>値136</value>
            public string Val136 { get; set; }
            /// <summary>Gets or sets 値137</summary>
            /// <value>値137</value>
            public string Val137 { get; set; }
            /// <summary>Gets or sets 値138</summary>
            /// <value>値138</value>
            public string Val138 { get; set; }
            /// <summary>Gets or sets 値139</summary>
            /// <value>値139</value>
            public string Val139 { get; set; }
            /// <summary>Gets or sets 値140</summary>
            /// <value>値140</value>
            public string Val140 { get; set; }
            /// <summary>Gets or sets 値141</summary>
            /// <value>値141</value>
            public string Val141 { get; set; }
            /// <summary>Gets or sets 値142</summary>
            /// <value>値142</value>
            public string Val142 { get; set; }
            /// <summary>Gets or sets 値143</summary>
            /// <value>値143</value>
            public string Val143 { get; set; }
            /// <summary>Gets or sets 値144</summary>
            /// <value>値144</value>
            public string Val144 { get; set; }
            /// <summary>Gets or sets 値145</summary>
            /// <value>値145</value>
            public string Val145 { get; set; }
            /// <summary>Gets or sets 値146</summary>
            /// <value>値146</value>
            public string Val146 { get; set; }
            /// <summary>Gets or sets 値147</summary>
            /// <value>値147</value>
            public string Val147 { get; set; }
            /// <summary>Gets or sets 値148</summary>
            /// <value>値148</value>
            public string Val148 { get; set; }
            /// <summary>Gets or sets 値149</summary>
            /// <value>値149</value>
            public string Val149 { get; set; }
            /// <summary>Gets or sets 値150</summary>
            /// <value>値150</value>
            public string Val150 { get; set; }
            /// <summary>Gets or sets 値151</summary>
            /// <value>値151</value>
            public string Val151 { get; set; }
            /// <summary>Gets or sets 値152</summary>
            /// <value>値152</value>
            public string Val152 { get; set; }
            /// <summary>Gets or sets 値153</summary>
            /// <value>値153</value>
            public string Val153 { get; set; }
            /// <summary>Gets or sets 値154</summary>
            /// <value>値154</value>
            public string Val154 { get; set; }
            /// <summary>Gets or sets 値155</summary>
            /// <value>値155</value>
            public string Val155 { get; set; }
            /// <summary>Gets or sets 値156</summary>
            /// <value>値156</value>
            public string Val156 { get; set; }
            /// <summary>Gets or sets 値157</summary>
            /// <value>値157</value>
            public string Val157 { get; set; }
            /// <summary>Gets or sets 値158</summary>
            /// <value>値158</value>
            public string Val158 { get; set; }
            /// <summary>Gets or sets 値159</summary>
            /// <value>値159</value>
            public string Val159 { get; set; }
            /// <summary>Gets or sets 値160</summary>
            /// <value>値160</value>
            public string Val160 { get; set; }
            /// <summary>Gets or sets 値161</summary>
            /// <value>値161</value>
            public string Val161 { get; set; }
            /// <summary>Gets or sets 値162</summary>
            /// <value>値162</value>
            public string Val162 { get; set; }
            /// <summary>Gets or sets 値163</summary>
            /// <value>値163</value>
            public string Val163 { get; set; }
            /// <summary>Gets or sets 値164</summary>
            /// <value>値164</value>
            public string Val164 { get; set; }
            /// <summary>Gets or sets 値165</summary>
            /// <value>値165</value>
            public string Val165 { get; set; }
            /// <summary>Gets or sets 値166</summary>
            /// <value>値166</value>
            public string Val166 { get; set; }
            /// <summary>Gets or sets 値167</summary>
            /// <value>値167</value>
            public string Val167 { get; set; }
            /// <summary>Gets or sets 値168</summary>
            /// <value>値168</value>
            public string Val168 { get; set; }
            /// <summary>Gets or sets 値169</summary>
            /// <value>値169</value>
            public string Val169 { get; set; }
            /// <summary>Gets or sets 値170</summary>
            /// <value>値170</value>
            public string Val170 { get; set; }
            /// <summary>Gets or sets 値171</summary>
            /// <value>値171</value>
            public string Val171 { get; set; }
            /// <summary>Gets or sets 値172</summary>
            /// <value>値172</value>
            public string Val172 { get; set; }
            /// <summary>Gets or sets 値173</summary>
            /// <value>値173</value>
            public string Val173 { get; set; }
            /// <summary>Gets or sets 値174</summary>
            /// <value>値174</value>
            public string Val174 { get; set; }
            /// <summary>Gets or sets 値175</summary>
            /// <value>値175</value>
            public string Val175 { get; set; }
            /// <summary>Gets or sets 値176</summary>
            /// <value>値176</value>
            public string Val176 { get; set; }
            /// <summary>Gets or sets 値177</summary>
            /// <value>値177</value>
            public string Val177 { get; set; }
            /// <summary>Gets or sets 値178</summary>
            /// <value>値178</value>
            public string Val178 { get; set; }
            /// <summary>Gets or sets 値179</summary>
            /// <value>値179</value>
            public string Val179 { get; set; }
            /// <summary>Gets or sets 値180</summary>
            /// <value>値180</value>
            public string Val180 { get; set; }
            /// <summary>Gets or sets 値181</summary>
            /// <value>値181</value>
            public string Val181 { get; set; }
            /// <summary>Gets or sets 値182</summary>
            /// <value>値182</value>
            public string Val182 { get; set; }
            /// <summary>Gets or sets 値183</summary>
            /// <value>値183</value>
            public string Val183 { get; set; }
            /// <summary>Gets or sets 値184</summary>
            /// <value>値184</value>
            public string Val184 { get; set; }
            /// <summary>Gets or sets 値185</summary>
            /// <value>値185</value>
            public string Val185 { get; set; }
            /// <summary>Gets or sets 値186</summary>
            /// <value>値186</value>
            public string Val186 { get; set; }
            /// <summary>Gets or sets 値187</summary>
            /// <value>値187</value>
            public string Val187 { get; set; }
            /// <summary>Gets or sets 値188</summary>
            /// <value>値188</value>
            public string Val188 { get; set; }
            /// <summary>Gets or sets 値189</summary>
            /// <value>値189</value>
            public string Val189 { get; set; }
            /// <summary>Gets or sets 値190</summary>
            /// <value>値190</value>
            public string Val190 { get; set; }
            /// <summary>Gets or sets 値191</summary>
            /// <value>値191</value>
            public string Val191 { get; set; }
            /// <summary>Gets or sets 値192</summary>
            /// <value>値192</value>
            public string Val192 { get; set; }
            /// <summary>Gets or sets 値193</summary>
            /// <value>値193</value>
            public string Val193 { get; set; }
            /// <summary>Gets or sets 値194</summary>
            /// <value>値194</value>
            public string Val194 { get; set; }
            /// <summary>Gets or sets 値195</summary>
            /// <value>値195</value>
            public string Val195 { get; set; }
            /// <summary>Gets or sets 値196</summary>
            /// <value>値196</value>
            public string Val196 { get; set; }
            /// <summary>Gets or sets 値197</summary>
            /// <value>値197</value>
            public string Val197 { get; set; }
            /// <summary>Gets or sets 値198</summary>
            /// <value>値198</value>
            public string Val198 { get; set; }
            /// <summary>Gets or sets 値199</summary>
            /// <value>値199</value>
            public string Val199 { get; set; }
            /// <summary>Gets or sets 値200</summary>
            /// <value>値200</value>
            public string Val200 { get; set; }
            /// <summary>Gets or sets 更新日時</summary>
            /// <value>更新日時</value>
            public DateTime? UpdateDate { get; set; }



            ///// <summary>
            ///// プライマリーキー
            ///// </summary>
            //public class PrimaryKey
            //{
            //    /// <summary>Gets or sets ユーザーID</summary>
            //    /// <value>ユーザーID</value>
            //    public string Userid { get; set; }
            //    /// <summary>Gets or sets コンダクトID</summary>
            //    /// <value>コンダクトID</value>
            //    public string Conductid { get; set; }
            //    /// <summary>Gets or sets 部署コード</summary>
            //    /// <value>部署コード</value>
            //    public string Bushocode { get; set; }
            //    /// <summary>Gets or sets 権限区分</summary>
            //    /// <value>権限区分</value>
            //    public string Authkbn { get; set; }
            //    /// <summary>
            //    /// コンストラクタ
            //    /// </summary>
            //    public PrimaryKey(string userid, string conductid, string bushocode, string authkbn)
            //    {
            //        Userid = userid;
            //        Conductid = conductid;
            //        Bushocode = bushocode;
            //        Authkbn = authkbn;
            //    }
            //}

            ///// <summary>
            ///// プライマリーキー情報
            ///// </summary>
            ///// <returns>プライマリーキー情報</returns>
            //public PrimaryKey PK()
            //{
            //    PrimaryKey pk = new PrimaryKey(this.Userid, this.Conductid, this.Bushocode, this.Authkbn);
            //    return pk;
            //}

            ///// <summary>
            ///// エンティティ
            ///// </summary>
            ///// <returns>該当のデータを返す</returns>
            //public ComUserAuthEntity GetEntity(string userId, string conductId, string bushoCode, string authKbn ,ComDB db)
            //{
            //    ComUserAuthEntity.PrimaryKey condition = new ComUserAuthEntity.PrimaryKey(userId, conductId, bushoCode, authKbn);
            //    // SQL文生成
            //    string getEntitySql = getEntity(this.TableName, condition, db);
            //    if (string.IsNullOrEmpty(getEntitySql))
            //    {
            //        return null;
            //    }
            //    return db.GetEntityByDataClass<ComUserAuthEntity>(getEntitySql);
            //}
        }

        /// <summary>
        /// ユーザ所属マスタ
        /// </summary>
        public class UserBelongEntity
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public UserBelongEntity()
            {
                TableName = "ms_user_belong";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public int UserId { get; set; }
            /// <summary>Gets or sets 所属構成ID</summary>
            /// <value>所属構成ID</value>
            public int BelongId { get; set; }
            /// <summary>Gets or sets 本務フラグ</summary>
            /// <value>本務フラグ</value>
            public bool DutyFlg { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int GroupId { get; set; }
            /// <summary>Gets or sets 構成階層番号</summary>
            /// <value>構成階層番号</value>
            public int LayerNo { get; set; }
        }

        /// <summary>
        /// ユーザ機能権限マスタ
        /// </summary>
        public class MsUserConductAuthorityEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsUserConductAuthorityEntity()
            {
                TableName = "ms_user_conduct_authority";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public int UserId { get; set; }
            /// <summary>Gets or sets 機能ID</summary>
            /// <value>機能ID</value>
            public string ConductId { get; set; }

            ///// <summary>
            ///// プライマリーキー
            ///// </summary>
            //public class PrimaryKey
            //{
            //    /// <summary>Gets or sets ユーザID</summary>
            //    /// <value>ユーザID</value>
            //    public int UserId { get; set; }
            //    /// <summary>Gets or sets 機能ID</summary>
            //    /// <value>機能ID</value>
            //    public string ConductId { get; set; }
            //    /// <summary>
            //    /// コンストラクタ
            //    /// </summary>
            //    public PrimaryKey(int pUserId, string pConductId)
            //    {
            //        UserId = pUserId;
            //        ConductId = pConductId;
            //    }
            //}

            ///// <summary>
            ///// プライマリーキー情報
            ///// </summary>
            ///// <returns>プライマリーキー情報</returns>
            //public PrimaryKey PK()
            //{
            //    PrimaryKey pk = new PrimaryKey(this.UserId, this.ConductId);
            //    return pk;
            //}

            ///// <summary>
            ///// エンティティ
            ///// </summary>
            ///// <returns>該当のデータを返す</returns>
            //public MsUserConductAuthorityEntity GetEntity(int pUserId, string pConductId, ComDB db)
            //{
            //    PrimaryKey condition = new PrimaryKey(pUserId, pConductId);
            //    // SQL文生成
            //    string getEntitySql = getEntity(this.TableName, condition, db);
            //    if (string.IsNullOrEmpty(getEntitySql))
            //    {
            //        return null;
            //    }
            //    return db.GetEntityByDataClass<MsUserConductAuthorityEntity>(getEntitySql);
            //}
            ///// <summary>
            ///// 主キーを指定してDELETE実行
            ///// </summary>
            ///// <returns>エラーの場合False</returns>
            //public bool DeleteByPrimaryKey(int pUserId, string pConductId, ComDB db)
            //{
            //    PrimaryKey condition = new PrimaryKey(pUserId, pConductId);
            //    // SQL文生成
            //    string deleteSql = getDeleteSql(this.TableName, condition, db);
            //    if (string.IsNullOrEmpty(deleteSql))
            //    {
            //        return false;
            //    }
            //    int result = db.Regist(deleteSql);
            //    return result > 0;
            //}
        }

        /// <summary>
        /// 構成マスタ
        /// </summary>
        public class StructureEntity
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public StructureEntity()
            {
                TableName = "ms_structure";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int StructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int structureGroupId { get; set; }
            /// <summary>Gets or sets 構成番号</summary>
            /// <value>構成番号</value>
            public int StructureNo { get; set; }
            /// <summary>Gets or sets 親構成ID</summary>
            /// <value>親構成ID</value>
            public int? ParentStructureId { get; set; }
            /// <summary>Gets or sets 構成アイテムID</summary>
            /// <value>構成アイテムID</value>
            public int? StructureItemId { get; set; }
            /// <summary>Gets or sets アイテム翻訳ID</summary>
            /// <value>アイテム翻訳ID</value>
            public int? ItemTranslationId { get; set; }
            /// <summary>Gets or sets 並び順</summary>
            /// <value>並び順</value>
            public short? DisplayOrder { get; set; }
        }

        /// <summary>
        /// 構成アイテム
        /// </summary>
        public class VStructureItemEntity : CommonTableItem, IStructureItemEntity
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public VStructureItemEntity()
            {
                TableName = "v_structure_item";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int StructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }
            /// <summary>Gets or sets 親構成ID</summary>
            /// <value>親構成ID</value>
            public int? ParentStructureId { get; set; }
            /// <summary>Gets or sets 構成階層番号</summary>
            /// <value>構成階層番号</value>
            public int? StructureLayerNo { get; set; }
            /// <summary>Gets or sets 構成アイテムID</summary>
            /// <value>構成アイテムID</value>
            public int? StructureItemId { get; set; }
            /// <summary>Gets or sets アイテム翻訳ID</summary>
            /// <value>アイテム翻訳ID</value>
            public int? ItemTranslationId { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DisplayOrder { get; set; }
            /// <summary>Gets or sets 場所階層ID(工場)</summary>
            /// <value>場所階層ID(工場)</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 翻訳文字列</summary>
            /// <value>翻訳文字列</value>
            public string TranslationText { get; set; }
            /// <summary>Gets or sets 翻訳項目説明</summary>
            /// <value>翻訳項目説明</value>
            public string TranslationItemDescription { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 構成ID</summary>
                /// <value>構成ID</value>
                public int StructureId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pStructureId)
                {
                    StructureId = pStructureId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.StructureId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public VStructureItemEntity GetEntity(int pStructureId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pStructureId);
                // SQL文生成
                // ビューのため、既存のGetEntityではテーブル情報を参照できない、固定でSQLを作成する
                string getEntitySql = "select * from v_structure_item where structure_id = @StructureId";

                return db.GetEntityByDataClass<VStructureItemEntity>(getEntitySql, condition);
            }

            /// <summary>
            /// エンティティ(言語指定)
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public VStructureItemEntity GetEntity(int pStructureId, string languageId, ComDB db)
            {
                VStructureItemEntity condition = new VStructureItemEntity();
                condition.StructureId = pStructureId;
                condition.LanguageId = languageId;
                // SQL文生成
                // ビューのため、既存のGetEntityではテーブル情報を参照できない、固定でSQLを作成する
                string getEntitySql = "select * from v_structure_item where structure_id = @StructureId and language_id = @LanguageId";

                return db.GetEntityByDataClass<VStructureItemEntity>(getEntitySql, condition);
            }
        }

        /// <summary>
        /// 画面コントロールユーザーカスタマイズマスタ
        /// </summary>
        public class CmControlUserCustomizeEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CmControlUserCustomizeEntity()
            {
                TableName = "cm_control_user_customize";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public int UserId { get; set; }
            ///// <summary>Gets or sets 場所階層ID(工場)</summary>
            ///// <value>場所階層ID(工場)</value>
            //public int LocationStructureId { get; set; }
            /// <summary>Gets or sets プログラムID</summary>
            /// <value>プログラムID</value>
            public string ProgramId { get; set; }
            /// <summary>Gets or sets 画面NO</summary>
            /// <value>画面NO</value>
            public int FormNo { get; set; }
            /// <summary>Gets or sets コントロールグループID</summary>
            /// <value>コントロールグループID</value>
            public string ControlGroupId { get; set; }
            /// <summary>Gets or sets コントロール番号</summary>
            /// <value>コントロール番号</value>
            public int ControlNo { get; set; }
            /// <summary>Gets or sets データ区分</summary>
            /// <value>データ区分</value>
            public int DataDivision { get; set; }
            /// <summary>Gets or sets 表示フラグ</summary>
            /// <value>表示フラグ</value>
            public bool DisplayFlg { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int DisplayOrder { get; set; }

            //public List<int> FactoryIdList { get; set; }

            ///// <summary>
            ///// プライマリーキー
            ///// </summary>
            //public class PrimaryKey
            //{
            //    /// <summary>Gets or sets ユーザID</summary>
            //    /// <value>ユーザID</value>
            //    public int UserId { get; set; }
            //    /// <summary>Gets or sets 場所階層ID(工場)</summary>
            //    /// <value>場所階層ID(工場)</value>
            //    public int LocationStructureId { get; set; }
            //    /// <summary>Gets or sets プログラムID</summary>
            //    /// <value>プログラムID</value>
            //    public string ProgramId { get; set; }
            //    /// <summary>Gets or sets 画面NO</summary>
            //    /// <value>画面NO</value>
            //    public int FormNo { get; set; }
            //    /// <summary>Gets or sets コントロールグループID</summary>
            //    /// <value>コントロールグループID</value>
            //    public string ControlGroupId { get; set; }
            //    /// <summary>Gets or sets コントロール番号</summary>
            //    /// <value>コントロール番号</value>
            //    public int ControlNo { get; set; }
            //    /// <summary>Gets or sets データ区分</summary>
            //    /// <value>データ区分</value>
            //    public int DataDivision { get; set; }
            //    /// <summary>
            //    /// コンストラクタ
            //    /// </summary>
            //    public PrimaryKey(int pUserId, int pLocationStructureId, string pProgramId, int pFormNo, string pControlGroupId, int pControlNo, int pDataDivision)
            //    {
            //        UserId = pUserId;
            //        LocationStructureId = pLocationStructureId;
            //        ProgramId = pProgramId;
            //        FormNo = pFormNo;
            //        ControlGroupId = pControlGroupId;
            //        ControlNo = pControlNo;
            //        DataDivision = pDataDivision;
            //    }
            //}

            ///// <summary>
            ///// プライマリーキー情報
            ///// </summary>
            ///// <returns>プライマリーキー情報</returns>
            //public PrimaryKey PK()
            //{
            //    PrimaryKey pk = new PrimaryKey(this.UserId, this.LocationStructureId, this.ProgramId, this.FormNo, this.ControlGroupId, this.ControlNo, this.DataDivision);
            //    return pk;
            //}

            ///// <summary>
            ///// エンティティ
            ///// </summary>
            ///// <returns>該当のデータを返す</returns>
            //public CmControlUserCustomizeEntity GetEntity(int pUserId, int pLocationStructureId, string pProgramId, int pFormNo, string pControlGroupId, int pControlNo, int pDataDivision, ComDB db)
            //{
            //    PrimaryKey condition = new PrimaryKey(pUserId, pLocationStructureId, pProgramId, pFormNo, pControlGroupId, pControlNo, pDataDivision);
            //    // SQL文生成
            //    string getEntitySql = getEntity(this.TableName, condition, db);
            //    if (string.IsNullOrEmpty(getEntitySql))
            //    {
            //        return null;
            //    }
            //    return db.GetEntityByDataClass<CmControlUserCustomizeEntity>(getEntitySql);
            //}
            ///// <summary>
            ///// 主キーを指定してDELETE実行
            ///// </summary>
            ///// <returns>エラーの場合False</returns>
            //public bool DeleteByPrimaryKey(int pUserId, int pLocationStructureId, string pProgramId, int pFormNo, string pControlGroupId, int pControlNo, int pDataDivision, ComDB db)
            //{
            //    PrimaryKey condition = new PrimaryKey(pUserId, pLocationStructureId, pProgramId, pFormNo, pControlGroupId, pControlNo, pDataDivision);
            //    // SQL文生成
            //    string deleteSql = getDeleteSql(this.TableName, condition, db);
            //    if (string.IsNullOrEmpty(deleteSql))
            //    {
            //        return false;
            //    }
            //    int result = db.Regist(deleteSql);
            //    return result > 0;
            //}
        }

    }
}
