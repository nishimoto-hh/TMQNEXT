using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComDao = CommonSTDUtil.CommonDataBaseClass;

namespace BusinessLogic_MS0010
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_MS0010
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets ユーザー名</summary>
            /// <value>ユーザー名</value>
            public string UserName { get; set; }
            /// <summary>Gets or sets メールアドレス</summary>
            /// <value>メールアドレス</value>
            public string MailAddress { get; set; }
            /// <summary>Gets or sets 権限レベル</summary>
            /// <value>権限レベル</value>
            public int AuthorityLevelId { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DeleteFlg { get; set; }
            /// <summary>Gets or sets 場所階層リスト</summary>
            /// <value>場所階層リスト</value>
            public List<int> locationIdList { get; set; }
            /// <summary>Gets or sets 職種リスト</summary>
            /// <value>職種リスト</value>
            public List<int> jobIdList { get; set; }
            /// <summary>Gets or sets 場所階層ID</summary>
            /// <value>場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int JobStructureId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス
        /// </summary>
        public class userList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets ユーザーID</summary>
            /// <value>ユーザーID</value>
            public int UserId { get; set; }
            /// <summary>Gets or sets ログインID</summary>
            /// <value>ログインID</value>
            public string LoginId { get; set; }
            /// <summary>Gets or sets 表示名</summary>
            /// <value>表示名</value>
            public string DisplayName { get; set; }
            /// <summary>Gets or sets 姓</summary>
            /// <value>姓</value>
            public string FamilyName { get; set; }
            /// <summary>Gets or sets 名</summary>
            /// <value>名</value>
            public string FirstName { get; set; }
            /// <summary>Gets or sets メールアドレス</summary>
            /// <value>メールアドレス</value>
            public string MailAddress { get; set; }
            /// <summary>Gets or sets 権限レベルID</summary>
            /// <value>権限レベルID</value>
            public int AuthorityLevelId { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageStructureId { get; set; }
            /// <summary>Gets or sets 権限レベル(表示用)</summary>
            /// <value>権限レベル(表示用)</value>
            public string AuthorityLevel { get; set; }
            /// <summary>Gets or sets 言語</summary>
            /// <value>言語</value>
            public string LanguageStructureName { get; set; }
            /// <summary>Gets or sets 更新シリアルID</summary>
            /// <value>更新シリアルID</value>
            public int UpdateSerialid { get; set; }
            /// <summary>Gets or sets 拡張データ</summary>
            /// <value>拡張データ</value>
            public int ExtensionData { get; set; }

            // ********************** 本務設定 *****************************

            /// <summary>Gets or sets 本務設定</summary>
            /// <value>本務設定</value>
            public int? FactoryId { get; set; }

            /// <summary>Gets or sets 本務フラグ</summary>
            /// <value>本務フラグ</value>
            public int DutyFlg { get; set; }

            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int StructureGroupId { get; set; }

            // ********************** 詳細画面 ユーザー情報 *****************

            /// <summary>Gets or sets 役割(複数選択時は'|'区切り)</summary>
            /// <value>役割</value>
            public string RoleId { get; set; }
            /// <summary>Gets or sets パスワード</summary>
            /// <value>パスワード</value>
            public string Password { get; set; }
            /// <summary>Gets or sets 暗号化キー</summary>
            /// <value>暗号化キー</value>
            public string EncryptKey { get; set; }

            // ********************** 詳細画面 機能権限設定 *****************

            /// <summary>Gets or sets 選択</summary>
            /// <value>選択</value>
            public int Selection { get; set; }
            /// <summary>Gets or sets 機能グループID</summary>
            /// <value>機能グループID</value>
            public string ConductGroupId { get; set; }
            /// <summary>Gets or sets 機能グループ</summary>
            /// <value>機能グループ</value>
            public string ConductGroupName { get; set; }
            /// <summary>Gets or sets 機能ID</summary>
            /// <value>機能ID</value>
            public string ConductId { get; set; }
            /// <summary>Gets or sets 機能名</summary>
            /// <value>機能名</value>
            public string ConductName { get; set; }
            /// <summary>Gets or sets 機能内使用共通ID</summary>
            /// <value>機能内使用共通ID</value>
            public string CommonConductId { get; set; }
        }

        /// <summary>
        /// 所属設定のデータクラス
        /// </summary>
        public class locationList : userList
        {
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
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
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string? JobName { get; set; }
        }

        /// <summary>
        /// 入力チェック・その他画面制御に用いるデータクラス
        /// </summary>
        public class checkList : userList
        {
            /// <summary>Gets or sets カウント件数</summary>
            /// <value>カウント件数</value>
            public int count { get; set; }
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
            public int extensionData { get; set; }
        }

        /// <summary>
        /// 項目カスタマイズ登録用
        /// </summary>
        public class UserCustomizeClass
        {
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public int UserId { get; set; }
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
            public string ControlNo { get; set; }
            /// <summary>Gets or sets データ区分</summary>
            /// <value>データ区分</value>
            public int DataDivision { get; set; }
            /// <summary>Gets or sets 表示フラグ</summary>
            /// <value>表示フラグ</value>
            public int DisplayFlg { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int DisplayOrder { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public bool DeleteFlg { get; set; }
            /// <summary>Gets or sets 登録日時</summary>
            /// <value>登録日時</value>
            public DateTime InsertDatetime { get; set; }
            /// <summary>Gets or sets 登録者ID</summary>
            /// <value>登録者ID</value>
            public int InsertUserId { get; set; }
            /// <summary>Gets or sets 更新日時</summary>
            /// <value>更新日時</value>
            public DateTime UpdateDatetime { get; set; }
            /// <summary>Gets or sets 更新者ID</summary>
            /// <value>更新者ID</value>
            public int UpdateUserId { get; set; }
        }
    }
}
