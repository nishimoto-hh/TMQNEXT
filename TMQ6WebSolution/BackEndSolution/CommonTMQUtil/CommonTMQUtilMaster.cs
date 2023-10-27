using CommonExcelUtil;
using Microsoft.AspNetCore.Http;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ExcelUtil = CommonExcelUtil.CommonExcelUtil;
using TMQDataClass = CommonTMQUtil.TMQCommonDataClass;
using Dao = CommonTMQUtil.CommonTMQUtilDataClass;
using STDDao = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using System.Dynamic;
using System.Text;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CommonTMQUtil
{
    /// <summary>
    /// TMQ用共通ユーティリティクラス
    /// </summary>
    public static partial class CommonTMQUtil
    {
        #region マスタメンテナンス共通処理
        /// <summary>
        /// マスタメンテナンス共通
        /// </summary>
        public class ComMaster
        {
            #region 定数
            /// <summary>
            /// SQLファイル名称
            /// </summary>
            public class SqlName
            {
                /// <summary>SQL名：ユーザ所属情報取得</summary>
                public const string GetUserBelongInfo = "GetUserBelongInfo";
                /// <summary>SQL名：標準アイテム一覧取得</summary>
                public const string GetStandardItemList = "GetStandardItemList";
                /// <summary>SQL名：工場アイテム一覧取得</summary>
                public const string GetFactoryItemList = "GetFactoryItemList";
                /// <summary>SQL名：言語情報取得</summary>
                public const string GetLanguageList = "GetLanguageList";
                /// <summary>SQL名：アイテム翻訳取得</summary>
                public const string GetItemTranslation = "GetItemTranslation";
                /// <summary>SQL名：標準アイテム翻訳取得</summary>
                public const string GetStandardItemTranslation = "GetStandardItemTranslation";
                /// <summary>SQL名：アイテム情報取得</summary>
                public const string GetItemInfo = "GetItemInfo";
                /// <summary>SQL名：翻訳マスタ件数取得(全工場)</summary>
                public const string GetCountAllTranslation = "GetCountAllTranslation";
                /// <summary>SQL名：翻訳マスタ件数取得</summary>
                public const string GetCountTranslation = "GetCountTranslation";
                /// <summary>SQL名：翻訳マスタ情報取得</summary>
                public const string GetMsTranslationInfo = "GetMsTranslationInfo";
                /// <summary>SQL名：アイテム表示順一覧取得</summary>
                public const string GetItemOrderList = "GetItemOrderList";
                /// <summary>SQL名：翻訳マスタ登録</summary>
                public const string InsertMsTranslationInfo = "InsertMsTranslationInfo";
                /// <summary>SQL名：翻訳マスタ登録(翻訳ID取得)</summary>
                public const string InsertMsTranslationInfoGetTranslationId = "InsertMsTranslationInfoGetTranslationId";
                /// <summary>SQL名：アイテムマスタ登録</summary>
                public const string InsertMsItemInfo = "InsertMsItemInfo";
                /// <summary>SQL名：アイテムマスタ拡張登録</summary>
                public const string InsertMsItemExtensionInfo = "InsertMsItemExtensionInfo";
                /// <summary>SQL名：構成マスタ登録</summary>
                public const string InsertMsStructureInfo = "InsertMsStructureInfo";
                /// <summary>SQL名：工場別未使用標準アイテムマスタ登録</summary>
                public const string InsertMsStructureUnused = "InsertMsStructureUnused";
                /// <summary>SQL名：工場別アイテム表示順マスタ登録</summary>
                public const string InsertMsStructureOrder = "InsertMsStructureOrder";
                /// <summary>SQL名：翻訳マスタ更新</summary>
                public const string UpdateMsTranslationInfo = "UpdateMsTranslationInfo";
                /// <summary>SQL名：アイテムマスタ更新</summary>
                public const string UpdateMsItemInfo = "UpdateMsItemInfo";
                /// <summary>SQL名：アイテムマスタ拡張更新</summary>
                public const string UpdateMsItemExtensionInfo = "UpdateMsItemExtensionInfo";
                /// <summary>SQL名：構成マスタ更新</summary>
                public const string UpdateMsStructureInfo = "UpdateMsStructureInfo";
                /// <summary>SQL名：構成マスタ更新(削除フラグ付加)</summary>
                public const string UpdateMsStructureInfoAddDeleteFlg = "UpdateMsStructureInfoAddDeleteFlg";
                /// <summary>SQL名：翻訳マスタ削除</summary>
                public const string DeleteMsTranslationInfo = "DeleteMsTranslationInfo";
                /// <summary>SQL名：工場別未使用標準アイテムマスタ削除</summary>
                public const string DeleteMsStructureUnused = "DeleteMsStructureUnused";
                /// <summary>SQL名：工場別アイテム表示順マスタ削除</summary>
                public const string DeleteMsStructureOrder = "DeleteMsStructureOrder";
                /// <summary>SQL名：子階層構成マスタ更新</summary>
                public const string UpdateLayerMsStructureInfo = "UpdateLayerMsStructureInfo";
                /// <summary>SQL名：拡張項目1重複データ件数取得 </summary>
                public const string GetExtensionItemCount = "GetExtensionItemCount";
                public const string GetExtensionItemCountAll = "GetExtensionItemCountAll";
                /// <summary>SQL名：階層翻訳マスタ件数取得</summary>
                public const string GetCountLayersTranslation = "GetCountLayersTranslation";
                /// <summary>SQL名：階層翻訳マスタ件数取得(工場指定)</summary>
                public const string GetCountLayersTranslationByFactory = "GetCountLayersTranslationByFactory";
                /// <summary>SQL名：階層アイテム表示順一覧取得</summary>
                public const string GetLayersItemOrderList = "GetLayersItemOrderList";
                /// <summary>SQL名：階層工場別アイテム表示順マスタ削除</summary>
                public const string DeleteLayersStructureOrder = "DeleteLayersStructureOrder";
                /// <summary>SQL名：子アイテムの構成IDを取得</summary>
                public const string GetChildStructureIdList = "GetChildStructureIdList";
                /// <summary>SQL名：子アイテムを削除</summary>
                public const string UpdateChildLayersAddDeleteFlg = "UpdateChildLayersAddDeleteFlg";
                /// <summary>SQL名：翻訳マスタ情報取得(工場毎)</summary>
                public const string GetMsTranslationInfoByFactory = "GetMsTranslationInfoByFactory";

                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = @"Master";
                public const string ComLayersDir = @"Master\ComLayers";
            }

            /// <summary>
            /// フォーム、グループ、コントロールの親子関係を表現した場合の定数クラス
            /// </summary>
            public class ConductInfo
            {
                /// <summary>
                /// 一覧画面
                /// </summary>
                public static class FormList
                {
                    /// <summary>
                    /// フォーム番号
                    /// </summary>
                    public const short FormNo = 1;
                    /// <summary>
                    /// コントロールID
                    /// </summary>
                    public static class ControlId
                    {
                        /// <summary>
                        /// 検索条件の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public const string SearchId = "BODY_000_00_LST_0";
                        /// <summary>
                        /// 標準アイテム一覧の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public const string StarndardItemId = "BODY_020_00_LST_0";
                        /// <summary>
                        /// 工場アイテム一覧の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public const string FactoryItemId = "BODY_030_00_LST_0";
                        /// <summary>
                        /// 非表示情報の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public const string HiddenId = "BODY_050_00_LST_0";

                    }
                }

                /// <summary>
                /// 登録・修正画面
                /// </summary>
                public static class FormEdit
                {
                    /// <summary>
                    /// フォーム番号
                    /// </summary>
                    public const short FormNo = 2;
                    /// <summary>
                    /// コントロールID
                    /// </summary>
                    public static class ControlId
                    {
                        /// <summary>
                        /// アイテムIDの画面項目定義テーブルのコントロールID
                        /// </summary>
                        public const string ItemId = "BODY_000_00_LST_1";
                        /// <summary>
                        /// アイテム翻訳の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public const string ItemTranId = "BODY_010_00_LST_1";
                        /// <summary>
                        /// アイテム情報の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public const string ItemInfoId = "BODY_020_00_LST_1";
                    }
                }

                /// <summary>
                /// 表示順変更画面
                /// </summary>
                public static class FormOrder
                {
                    /// <summary>
                    /// フォーム番号
                    /// </summary>
                    public const short FormNo = 3;
                    /// <summary>
                    /// コントロールID
                    /// </summary>
                    public static class ControlId
                    {
                        /// <summary>
                        /// アイテム一覧の画面項目定義テーブルのコントロールID
                        /// </summary>
                        public const string ItemOrderId = "BODY_000_00_LST_2";
                    }
                }
            }

            /// <summary>
            /// 権限
            /// </summary>
            public class Authority
            {
                /// <summary>連番</summary>
                public const short Seq = 1;
                /// <summary>システム管理者</summary>
                public const int SystemAdmin = 99;
            }

            /// <summary>
            /// 構成マスタ
            /// </summary>
            public class Structure
            {
                /// <summary>
                /// 構成階層番号
                /// </summary>
                public class StructureLayerNo
                {
                    /// <summary>階層0</summary>
                    public const int Layer0 = 0;
                    /// <summary>階層1</summary>
                    public const int Layer1 = 1;
                    /// <summary>階層2</summary>
                    public const int Layer2 = 2;
                    /// <summary>階層3</summary>
                    public const int Layer3 = 3;
                    /// <summary>階層4</summary>
                    public const int Layer4 = 4;
                    /// <summary>階層5</summary>
                    public const int Layer5 = 5;
                }
            }

            /// <summary>
            /// 画面タイプ
            /// </summary>
            public class FormType
            {
                /// <summary>登録画面</summary>
                public const int Regist = 1;
                /// <summary>修正画面</summary>
                public const int Edit = 2;
            }

            /// <summary>
            /// アイテム一覧タイプ
            /// </summary>
            public class ItemListType
            {
                /// <summary>標準アイテム一覧</summary>
                public const int Standard = 1;
                /// <summary>工場アイテム一覧</summary>
                public const int Factory = 2;
                /// <summary>標準・工場アイテム一覧</summary>
                public const int StandardFactory = 3;
            }

            /// <summary>
            /// 対象アイテム一覧
            /// </summary>
            public class TargetItemList
            {
                /// <summary>標準アイテム一覧</summary>
                public const int Standard = 1;
                /// <summary>工場アイテム一覧</summary>
                public const int Factory = 2;
            }
            #endregion
        }

        #region データクラス（マスタメンテナンス用）
        /// <summary>
        /// 検索条件のデータクラス（マスタメンテナンス用）
        /// </summary>
        public class SearchConditionForMaster : ComDao.SearchCommonClass
        {
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public int DeleteFlg { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int StructureGroupId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス(共通)（マスタメンテナンス用）
        /// </summary>
        public class CommonResultItemForMaster : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int StructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets アイテムID</summary>
            /// <value>アイテムID</value>
            public int ItemId { get; set; }
            /// <summary>Gets or sets アイテム翻訳ID</summary>
            /// <value>アイテム翻訳ID</value>
            public int TranslationId { get; set; }
            /// <summary>Gets or sets アイテム翻訳名称</summary>
            /// <value>アイテム翻訳名称</value>
            public string TranslationText { get; set; }
            /// <summary>Gets or sets 拡張データ1</summary>
            /// <value>拡張データ1</value>
            public string ExData1 { get; set; }
            /// <summary>Gets or sets 拡張データ2</summary>
            /// <value>拡張データ2</value>
            public string ExData2 { get; set; }
            /// <summary>Gets or sets 拡張データ3</summary>
            /// <value>拡張データ3</value>
            public string ExData3 { get; set; }
            /// <summary>Gets or sets 拡張データ4</summary>
            /// <value>拡張データ4</value>
            public string ExData4 { get; set; }
            /// <summary>Gets or sets 拡張データ5</summary>
            /// <value>拡張データ5</value>
            public string ExData5 { get; set; }
            /// <summary>Gets or sets 拡張データ6</summary>
            /// <value>拡張データ6</value>
            public string ExData6 { get; set; }
            /// <summary>Gets or sets 拡張データ7</summary>
            /// <value>拡張データ7</value>
            public string ExData7 { get; set; }
            /// <summary>Gets or sets 拡張データ8</summary>
            /// <value>拡張データ8</value>
            public string ExData8 { get; set; }
            /// <summary>Gets or sets 拡張データ9</summary>
            /// <value>拡張データ9</value>
            public string ExData9 { get; set; }
            /// <summary>Gets or sets 拡張データ10</summary>
            /// <value>拡張データ10</value>
            public string ExData10 { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DisplayOrder { get; set; }
            /// <summary>Gets or sets 未使用フラグ</summary>
            /// <value>未使用フラグ</value>
            public bool UnusedFlg { get; set; }
            /// <summary>Gets or sets 標準アイテムフラグ</summary>
            /// <value>標準アイテムフラグ</value>
            public bool StandardItemFlg { get; set; }
            /// <summary>Gets or sets 構成グループI</summary>
            /// <value>構成グループID</value>
            public int StructureGroupId { get; set; }
            /// <summary>Gets or sets 翻訳用工場ID</summary>
            /// <value>翻訳用工場ID</value>
            public int TranslationFactoryId { get; set; }
            /// <summary>Gets or sets 更新シリアルID(拡張データ1)</summary>
            /// <value>更新シリアルID(拡張データ1)</value>
            public int UpdateSerialidEx1 { get; set; }
            /// <summary>Gets or sets 更新シリアルID(拡張データ2)</summary>
            /// <value>更新シリアルID(拡張データ2)</value>
            public int UpdateSerialidEx2 { get; set; }
            /// <summary>Gets or sets 更新シリアルID(拡張データ3)</summary>
            /// <value>更新シリアルID(拡張データ3)</value>
            public int UpdateSerialidEx3 { get; set; }
            /// <summary>Gets or sets 更新シリアルID(拡張データ4)</summary>
            /// <value>更新シリアルID(拡張データ4)</value>
            public int UpdateSerialidEx4 { get; set; }
            /// <summary>Gets or sets 更新シリアルID(拡張データ5)</summary>
            /// <value>更新シリアルID(拡張データ5)</value>
            public int UpdateSerialidEx5 { get; set; }
            /// <summary>Gets or sets 更新シリアルID(拡張データ6)</summary>
            /// <value>更新シリアルID(拡張データ6)</value>
            public int UpdateSerialidEx6 { get; set; }
            /// <summary>Gets or sets 更新シリアルID(拡張データ7)</summary>
            /// <value>更新シリアルID(拡張データ7)</value>
            public int UpdateSerialidEx7 { get; set; }
            /// <summary>Gets or sets 更新シリアルID(拡張データ8)</summary>
            /// <value>更新シリアルID(拡張データ8)</value>
            public int UpdateSerialidEx8 { get; set; }
            /// <summary>Gets or sets 更新シリアルID(拡張データ9)</summary>
            /// <value>更新シリアルID(拡張データ9)</value>
            public int UpdateSerialidEx9 { get; set; }
            /// <summary>Gets or sets 更新シリアルID(拡張データ10)</summary>
            /// <value>更新シリアルID(拡張データ10)</value>
            public int UpdateSerialidEx10 { get; set; }
            /// <summary>Gets or sets 最大更新日時(表示順)</summary>
            /// <value>最大更新日時(表示順)</value>
            public DateTime? OrderUpdateDatetime { get; set; }
            /// <summary>Gets or sets 最大更新日時(未使用)</summary>
            /// <value>最大更新日時(未使用)</value>
            public DateTime? UnusedUpdateDatetime { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス（マスタメンテナンス用）
        /// </summary>
        public class SearchResultForMaster : CommonResultItemForMaster
        {
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
        }

        /// <summary>
        /// 非表示情報のデータクラス（マスタメンテナンス用）
        /// </summary>
        public class HiddenInfoForMaster
        {
            /// <summary>Gets or sets システム管理者フラグ</summary>
            /// <value>システム管理者フラグ</value>
            public bool SystemAdminFlg { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int StructureGroupId { get; set; }
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int? StructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 画面タイプ</summary>
            /// <value>画面タイプ</value>
            public int FormType { get; set; }
            /// <summary>Gets or sets アイテム一覧タイプ</summary>
            /// <value>アイテム一覧タイプ</value>
            public int ItemListType { get; set; }
            /// <summary>Gets or sets 対象アイテム一覧</summary>
            /// <value>対象アイテム一覧</value>
            public int TargetItemList { get; set; }
            /// <summary>Gets or sets 構成階層番号</summary>
            /// <value>構成階層番号</value>
            public int? StructureLayerNo { get; set; }
            /// <summary>Gets or sets 親構成ID</summary>
            /// <value>親構成ID</value>
            public int? ParentStructureId { get; set; }
            /// <summary>Gets or sets 本務工場ID</summary>
            /// <value>本務工場ID</value>
            public int MainFactoryId { get; set; }
            /// <summary>Gets or sets 本務工場の地区ID</summary>
            /// <value>本務工場の地区ID</value>
            public int MainDistrictId { get; set; }
            /// <summary>Gets or sets 構成IDリスト</summary>
            /// <value>構成IDリスト</value>
            public string StructureIdList { get; set; }

            /// <summary>1ページ当たりの件数</summary>
            public int PageSize { get; set; }
            /// <summary>ページ番号</summary>
            public int PageNumber { get; set; }
            /// <summary>オフセット</summary>
            public int Offset { get; set; }
        }

        /// <summary>
        /// アイテム翻訳のデータクラス（マスタメンテナンス用）
        /// </summary>
        public class ItemTranslationForMaster : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int Num { get; set; }
            /// <summary>Gets or sets 言語名称</summary>
            /// <value>言語名称</value>
            public string LanTransName { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int StructureGroupId { get; set; }
            /// <summary>Gets or sets 翻訳工場ID</summary>
            /// <value>翻訳工場ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets アイテムID</summary>
            /// <value>アイテムID</value>
            public int? ItemId { get; set; }
            /// <summary>Gets or sets アイテム翻訳ID</summary>
            /// <value>アイテム翻訳ID</value>
            public int? TranslationId { get; set; }
            /// <summary>Gets or sets アイテム翻訳名称</summary>
            /// <value>アイテム翻訳名称</value>
            public string TranslationText { get; set; }
            /// <summary>Gets or sets アイテム翻訳名称(初期表示)</summary>
            /// <value>アイテム翻訳名称(初期表示)</value>
            public string TranslationTextBk { get; set; }
            /// <summary>Gets or sets 更新シリアルID(アイテムマスタ)</summary>
            /// <value>更新シリアルID(アイテムマスタ)</value>
            public int ItemUpdateSerialid { get; set; }
            /// <summary>Gets or sets 構成階層番号</summary>
            /// <value>構成階層番号</value>
            public int? StructureLayerNo { get; set; }
            /// <summary>Gets or sets 親構成ID</summary>
            /// <value>親構成ID</value>
            public int? ParentStructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
        }

        /// <summary>
        /// 検索結果のデータクラス(共通)（マスタメンテナンス用）
        /// </summary>
        public class CommonChildLayersItemForMaster : ComDao.CommonTableItem
        {
            /// <value>構成IDリスト</value>
            public string StructureIdList { get; set; }
            /// <summary>Gets or sets 構成グループI</summary>
            /// <value>構成グループID</value>
            public int StructureGroupId { get; set; }
        }
        #endregion

        #region 共通メソッド
        /// <summary>
        /// 非表示情報取得（マスタメンテナンス用）
        /// </summary>
        /// <param name="structureGroupId">構成グループID</param>
        /// <param name="itemListType">アイテム一覧タイプ</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static HiddenInfoForMaster GetHiddenInfoForMaster(
            int structureGroupId,
            int itemListType,
            string userId,
            ComDB db)
        {
            HiddenInfoForMaster result = new();
            // 構成グループID
            result.StructureGroupId = structureGroupId;
            // アイテム一覧タイプ
            result.ItemListType = itemListType;
            // ユーザ情報取得
            var userEntity = new ComDao.MsUserEntity().GetEntity(Convert.ToInt32(userId), db);
            // ユーザ言語取得
            result.LanguageId = userEntity.LanguageId;
            // 権限レベル取得
            var authorityLevel = getItemExData((int)Const.MsStructure.GroupId.Authority, ComMaster.Authority.Seq, userEntity.AuthorityLevelId);
            if (authorityLevel != null)
            {
                // システム管理者か判定する
                result.SystemAdminFlg = Convert.ToInt32(authorityLevel) == ComMaster.Authority.SystemAdmin;
            }

            // ユーザの本務工場取得
            ComDao.MsUserBelongEntity userCondition = new();
            userCondition.UserId = Convert.ToInt32(userId);
            result.MainFactoryId = TMQUtil.SqlExecuteClass.SelectEntity<int>(ComMaster.SqlName.GetUserBelongInfo, ComMaster.SqlName.SubDir, userCondition, db);

            // 本務工場の地区取得
            var structureInfo = new ComDao.MsStructureEntity().GetEntity(result.MainFactoryId, db);
            result.MainDistrictId = (int)structureInfo.ParentStructureId;

            /// <summary>
            /// アイテム拡張マスタから拡張データを取得する
            /// </summary>
            /// <param name="structureGroupId">構成グループID</param>
            /// <param name="seq">連番</param>
            /// <param name="structureId">構成ID</param>
            /// <returns>拡張データ</returns>
            string getItemExData(
                int structureGroupId,
                short seq,
                int structureId)
            {
                string result = null;

                // 構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new()
                {
                    // 構成グループID
                    StructureGroupId = structureGroupId,
                    // 連番
                    Seq = seq
                };
                // 構成アイテム、アイテム拡張マスタ情報取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, db);
                if (list != null)
                {
                    // 取得情報から構成IDを指定して拡張データを取得
                    result = list.Where(x => x.StructureId == structureId).Select(x => x.ExData).FirstOrDefault();
                }
                return result;
            }

            return result;
        }

        /// <summary>
        /// 一覧画面 アイテム一覧取得（マスタメンテナンス用）
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="sqlName">SQLファイル名(拡張子は含まない)</param>
        /// <param name="results">検索結果</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool GetItemListForMaster(
            SearchConditionForMaster condition,
            string sqlName,
            ref List<SearchResultForMaster> results,
            ComDB db)
        {
            // 一覧検索実行
            results = TMQUtil.SqlExecuteClass.SelectList<SearchResultForMaster>(sqlName, ComMaster.SqlName.SubDir, condition, db);

            return true;
        }

        /// <summary>
        /// 登録・修正画面 アイテム翻訳一覧取得（マスタメンテナンス用）
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="standardItemLanName">標準アイテム言語名称</param>
        /// <param name="results">検索結果</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool GetItemTranListForMaster(
            HiddenInfoForMaster condition,
            string standardItemLanName,
            ref List<ItemTranslationForMaster> results,
            ComDB db)
        {
            if (condition.FormType == (int)ComMaster.FormType.Regist)
            {
                // 登録画面

                // 検索実行
                results = TMQUtil.SqlExecuteClass.SelectList<ItemTranslationForMaster>(ComMaster.SqlName.GetLanguageList, ComMaster.SqlName.SubDir, condition, db);
                if (results == null)
                {
                    return false;
                }

                foreach (ItemTranslationForMaster result in results)
                {
                    // 構成グループID
                    result.StructureGroupId = condition.StructureGroupId;
                    // 翻訳工場ID
                    result.LocationStructureId = condition.FactoryId;

                    // 予備品ロケーション(予備品倉庫・棚)の場合は共通の工場IDとする
                    if (condition.StructureGroupId == (int)Const.MsStructure.GroupId.SpareLocation)
                    {
                        result.LocationStructureId = Const.CommonFactoryId;
                    }
                }
            }
            else
            {
                // 修正画面

                //// 検索実行
                //results = TMQUtil.SqlExecuteClass.SelectList<ItemTranslationForMaster>(ComMaster.SqlName.GetItemTranslation, ComMaster.SqlName.SubDir, condition, db);
                if (condition.StructureGroupId != (int)Const.MsStructure.GroupId.SpareLocation)
                {
                    // 検索実行
                    results = TMQUtil.SqlExecuteClass.SelectList<ItemTranslationForMaster>(ComMaster.SqlName.GetItemTranslation, ComMaster.SqlName.SubDir, condition, db);
                }
                else
                {
                    // 予備品ロケーションの場合、翻訳マスタの工場IDは共通の工場ID(0)で検索を行う
                    HiddenInfoForMaster condition2 = new();
                    condition2 = condition;
                    condition2.FactoryId = 0;
                    // 検索実行
                    results = TMQUtil.SqlExecuteClass.SelectList<ItemTranslationForMaster>(ComMaster.SqlName.GetItemTranslation, ComMaster.SqlName.SubDir, condition2, db);
                }
                if (results == null)
                {
                    return false;
                }

                foreach (ItemTranslationForMaster result in results)
                {
                    // 翻訳工場ID
                    result.LocationStructureId = condition.FactoryId;

                    // 予備品ロケーション(予備品倉庫・棚)の場合は共通の工場IDとする
                    if (condition.StructureGroupId == (int)Const.MsStructure.GroupId.SpareLocation)
                    {
                        result.LocationStructureId = Const.CommonFactoryId;
                    }
                }

                if (condition.TargetItemList == (int)ComMaster.TargetItemList.Standard && condition.FactoryId != Const.CommonFactoryId)
                {
                    // 標準アイテムの場合、言語に「標準アイテム」を追加

                    // 言語「標準アイテム」のデータクラスを作成
                    ItemTranslationForMaster standardItem = new();
                    standardItem.Num = 0;
                    standardItem.LanTransName = standardItemLanName;
                    standardItem.LanguageId = condition.LanguageId;
                    standardItem.StructureGroupId = condition.StructureGroupId;
                    standardItem.LocationStructureId = Const.CommonFactoryId;
                    standardItem.FactoryId = Const.CommonFactoryId;

                    // アイテム翻訳検索
                    ItemTranslationForMaster targetItemTran = TMQUtil.SqlExecuteClass.SelectEntity<ItemTranslationForMaster>(ComMaster.SqlName.GetStandardItemTranslation, ComMaster.SqlName.SubDir, condition, db);
                    if (targetItemTran != null)
                    {
                        standardItem.ItemId = targetItemTran.ItemId;
                        standardItem.TranslationId = targetItemTran.TranslationId;
                        standardItem.TranslationText = targetItemTran.TranslationText;
                        standardItem.TranslationTextBk = targetItemTran.TranslationText;
                        standardItem.UpdateSerialid = targetItemTran.UpdateSerialid;
                        standardItem.ItemUpdateSerialid = targetItemTran.ItemUpdateSerialid;
                    }

                    // 言語の先頭に「標準アイテム」を追加
                    results.Insert(0, standardItem);
                }
            }

            return true;
        }

        /// <summary>
        /// 登録・修正画面 アイテム情報取得（マスタメンテナンス用）
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="result">検索結果</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool GetItemInfoForMaster(
            HiddenInfoForMaster condition,
            ref List<SearchResultForMaster> results,
            ComDB db)
        {
            // 検索実行
            results = TMQUtil.SqlExecuteClass.SelectList<SearchResultForMaster>(ComMaster.SqlName.GetItemInfo, ComMaster.SqlName.SubDir, condition, db);
            if (results == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 表示順変更画面 アイテム一覧取得（マスタメンテナンス用）
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="result">検索結果</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool GetItemOrderListForMaster(
            HiddenInfoForMaster condition,
            ref List<SearchResultForMaster> results,
            ComDB db)
        {
            // 検索実行
            results = TMQUtil.SqlExecuteClass.SelectList<SearchResultForMaster>(ComMaster.SqlName.GetItemOrderList, ComMaster.SqlName.SubDir, condition, db);

            return true;
        }

        /// <summary>
        /// 登録・修正画面 アイテム翻訳必須チェック
        /// </summary>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="targetInfo">アイテム翻訳情報</param>
        /// <param name="itemTranList">アイテム翻訳一覧</param>
        /// <returns>エラーの場合True</returns>
        public static bool CheckRequiredByItemTran(
            HiddenInfoForMaster hiddenInfo,
            ItemTranslationForMaster targetInfo,
            List<ItemTranslationForMaster> itemTranList)
        {
            if (hiddenInfo.LanguageId != targetInfo.LanguageId)
            {
                // 言語がユーザ言語以外の場合、チェック対象外
                return false;
            }

            // ユーザ言語の場合

            if (hiddenInfo.TargetItemList == (int)ComMaster.TargetItemList.Standard && hiddenInfo.FactoryId != Const.CommonFactoryId)
            {
                // 標準アイテムの工場個別翻訳の場合
                if (string.IsNullOrEmpty(targetInfo.TranslationText))
                {
                    // ユーザ言語のアイテム翻訳が未入力の場合
                    // ユーザ言語以外のアイテム翻訳が入力されているかチェックする

                    // ユーザ言語以外のアイテム翻訳情報取得
                    var otherTranList = itemTranList.Where(x => x.LanguageId != hiddenInfo.LanguageId).ToList();
                    foreach (ItemTranslationForMaster otherTran in otherTranList)
                    {
                        if (!string.IsNullOrEmpty(otherTran.TranslationText))
                        {
                            // アイテム翻訳が入力されている場合、エラー
                            return true;
                        }
                    }
                    return false;
                }
            }
            else
            {
                // 標準アイテムの工場個別翻訳以外の場合
                if (string.IsNullOrEmpty(targetInfo.TranslationText))
                {
                    // アイテム翻訳が未入力の場合、エラー
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 登録・修正画面 アイテム翻訳重複チェック
        /// </summary>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="targetInfo">アイテム翻訳情報</param>
        /// <param name="errFlg">エラー有無(エラーの場合true)</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool CheckDuplicateByItemTran(
            HiddenInfoForMaster hiddenInfo,
            ItemTranslationForMaster targetInfo,
            ref bool errFlg,
            ComDB db)
        {
            if (string.IsNullOrEmpty(targetInfo.TranslationText))
            {
                // アイテム翻訳が未入力の場合、チェック対象外
                return true;
            }
            if (targetInfo.TranslationText == targetInfo.TranslationTextBk)
            {
                // アイテム翻訳に変更がない場合、チェック対象外
                return true;
            }

            string sqlName = string.Empty;
            //if (hiddenInfo.TargetItemList == ComMaster.TargetItemList.Standard)
            //{
            //    // 標準アイテム一覧

            //    // 構成グループに同じ翻訳が存在するか検索するSQL
            //    sqlName = ComMaster.SqlName.GetCountAllTranslation;
            //} 
            //else
            //{
            //    // 構成グループの標準アイテムと工場アイテムに同じ翻訳が存在するか検索するSQL
            //    sqlName = ComMaster.SqlName.GetCountTranslation;
            //}

            // 構成グループに同じ翻訳が存在するか検索
            sqlName = ComMaster.SqlName.GetCountTranslation;

            // 件数取得
            int cnt = 0;
            if (!GetCountDb(targetInfo, sqlName, ref cnt, db))
            {
                return false;
            }

            if (cnt > 0)
            {
                // アイテム翻訳が既に登録済みの場合、エラー
                errFlg = true;
            }

            return true;
        }

        /// <summary>
        /// 登録・修正画面 拡張項目１重複チェック
        /// </summary>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemInfo">拡張項目情報</param>
        /// <param name="itemExCnt">チェック拡張項目数</param>
        /// <param name="checkAllFlg">全体チェックフラグ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool CheckDuplicateByExItem1Tran<T>(
            HiddenInfoForMaster hiddenInfo,
            T itemInfo,
            //int itemId,
            int itemExCnt,
            bool checkAllFlg,
            ComDB db)
            where T : CommonResultItemForMaster, new()
        {

            // 件数取得
            int cnt = 0;
            itemInfo.StructureGroupId = hiddenInfo.StructureGroupId;
            itemInfo.FactoryId = hiddenInfo.FactoryId;
            var sqlName = ComMaster.SqlName.GetExtensionItemCount;
            if (checkAllFlg == true)
            {
                sqlName = ComMaster.SqlName.GetExtensionItemCountAll;
            }
            if (!GetCountDb(itemInfo, sqlName, ref cnt, db))
            {
                return false;
            }

            if (cnt > 0)
            {
                // 拡張項目１が既に登録済みの場合、エラー
                return false;
            }

            return true;
        }

        /// <summary>
        /// 翻訳マスタ登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemTranList">アイテム翻訳一覧</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="tranId">翻訳ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool RegistTranslation(
            DateTime now,
            HiddenInfoForMaster hiddenInfo,
            List<TMQUtil.ItemTranslationForMaster> itemTranList,
            string userId,
            ref int tranId,
            ComDB db)
        {
            // 新規登録フラグ
            var isNew = false;
            // ユーザ言語のアイテム翻訳情報取得
            var userTran = itemTranList.Where(x => x.LanguageId == hiddenInfo.LanguageId).FirstOrDefault();
            // ユーザ言語以外のアイテム翻訳情報取得
            var otherTranList = itemTranList.Where(x => x.LanguageId != hiddenInfo.LanguageId).ToList();
            // ユーザ言語のアイテム翻訳情報取得(工場毎チェック用)
            var userTran2 = itemTranList.Where(x => x.LanguageId == hiddenInfo.LanguageId).FirstOrDefault();

            if (hiddenInfo.FormType == (int)ComMaster.FormType.Regist)
            {
                // 登録画面

                ////// 工場内に同じ翻訳が存在するか翻訳マスタを検索
                ////isNew = isNewTranslation(ref tranId);
                //if (hiddenInfo.StructureGroupId != (int)Const.MsStructure.GroupId.SpareLocation)
                //{
                //    // 工場内に同じ翻訳が存在するか翻訳マスタを検索
                //    isNew = isNewTranslation(ref tranId);
                //}
                //else
                //{
                //    userTran2.FactoryId = hiddenInfo.FactoryId;
                //    userTran2.StructureLayerNo = hiddenInfo.StructureLayerNo;
                //    userTran2.ParentStructureId = hiddenInfo.ParentStructureId;
                //    int searchType = 1;
                //    if (hiddenInfo.StructureLayerNo == 3)
                //    {
                //        searchType = 2;
                //    }
                //    // 工場内に同じ翻訳が存在するか翻訳マスタを検索
                //    isNew = isNewTranslationByFactory(searchType, ref tranId);
                //}

                // 工場内に同じ翻訳が存在するか翻訳マスタを検索
                if (hiddenInfo.StructureGroupId == (int)Const.MsStructure.GroupId.SpareLocation
                    || hiddenInfo.StructureGroupId == (int)Const.MsStructure.GroupId.Location
                    || hiddenInfo.StructureGroupId == (int)Const.MsStructure.GroupId.Job
                    || hiddenInfo.StructureGroupId == (int)Const.MsStructure.GroupId.FailureCausePersonality)
                {
                    // 予備品ロケーション、場所階層、職種機種、原因性格の場合
                    userTran2.FactoryId = hiddenInfo.FactoryId;
                    userTran2.StructureLayerNo = hiddenInfo.StructureLayerNo;
                    userTran2.ParentStructureId = hiddenInfo.ParentStructureId;
                    //int searchType = 1;
                    //if (hiddenInfo.StructureLayerNo == 3)
                    //{
                    //    searchType = 2;
                    //}
                    int searchType = 2;
                    // 工場内に同じ翻訳が存在するか翻訳マスタを検索
                    isNew = isNewTranslationByFactory(searchType, ref tranId);
                }
                else
                {
                    // 工場内に同じ翻訳が存在するか翻訳マスタを検索
                    isNew = isNewTranslation(ref tranId);
                }

            }
            else
            {
                // 修正画面

                //// 翻訳が変更されているか
                //var isUpd = false;
                //foreach (TMQUtil.ItemTranslationForMaster itemTran in itemTranList)
                //{
                //    if (itemTran.TranslationText != itemTran.TranslationTextBk)
                //    {
                //        isUpd = true;
                //        break;
                //    }
                //}

                if (hiddenInfo.TargetItemList == (int)ComMaster.TargetItemList.Standard && hiddenInfo.FactoryId != Const.CommonFactoryId)
                {
                    // 標準アイテムの工場個別翻訳の場合

                    // 翻訳マスタ登録(標準アイテムの工場個別翻訳)
                    return registTranslationDbByFactory(ref tranId);
                }

                // ユーザ言語の翻訳が変更されているか
                var isUpd = false;
                if (userTran.TranslationText != userTran.TranslationTextBk)
                {
                    isUpd = true;
                }

                if (!isUpd)
                {
                    // 翻訳が変更されていない場合

                    // 翻訳ID取得
                    if (userTran.TranslationId != null)
                    {
                        tranId = (int)userTran.TranslationId;
                    }
                    //return true;
                }
                else
                {
                    // 翻訳が変更されている場合

                    // 工場内に同じ翻訳が存在するか翻訳マスタを検索
                    isNew = isNewTranslation(ref tranId);
                }
            }

            // 翻訳マスタ登録
            if (isNew)
            {
                // 翻訳マスタに同じ翻訳が存在しない場合、新規登録
                if (!registTranslationDb(ref tranId))
                {
                    return false;
                }

            }
            else
            {
                // 翻訳マスタに同じ翻訳が存在する場合、翻訳更新
                if (!updateTranslationDb(ref tranId))
                {
                    return false;
                }

            }

            return true;

            /// <summary>
            /// 翻訳マスタ検索
            /// </summary>
            /// <param name="tranId">翻訳ID</param>
            /// <returns>翻訳マスタ新規登録の場合True</returns>
            bool isNewTranslation(ref int tranId)
            {
                // ユーザ言語のアイテム翻訳に対して工場内に同じ翻訳が存在するか翻訳マスタを検索

                // 検索実行
                var registTranIds = TMQUtil.SqlExecuteClass.SelectList<int?>(ComMaster.SqlName.GetMsTranslationInfo, ComMaster.SqlName.SubDir, userTran, db, listUnComment: new List<string> { "AddItem" });
                if (registTranIds == null)
                {
                    // 同じ翻訳が存在しない場合、新規登録
                    return true;
                }
                else
                {
                    // 同じ翻訳が存在する場合、翻訳ID順の先頭を採用する
                    tranId = (int)registTranIds.First();

                    //// ユーザ言語以外のアイテム翻訳も同じ翻訳かチェックする
                    //foreach (int registTranId in registTranIds)
                    //{
                    //    var chkTran = false;
                    //    foreach (TMQUtil.ItemTranslationForMaster otherTran in otherTranList)
                    //    {
                    //        // 翻訳マスタ検索
                    //        var tranEntity = new ComDao.MsTranslationEntity().GetEntity(otherTran.LocationStructureId, registTranId, otherTran.LanguageId, db);
                    //        if (tranEntity == null)
                    //        {
                    //            // 翻訳が存在しない場合

                    //            if (!string.IsNullOrEmpty(otherTran.TranslationText))
                    //            {
                    //                // 対象のアイテム翻訳が入力されている場合、次の翻訳IDへ
                    //                chkTran = true;
                    //                break;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            // 翻訳が存在する場合

                    //            if (tranEntity.TranslationText != otherTran.TranslationText)
                    //            {
                    //                // 対象のアイテム翻訳と翻訳が異なる場合、次の翻訳IDへ
                    //                chkTran = true;
                    //                break;
                    //            }
                    //        }
                    //    }
                    //    if (!chkTran)
                    //    {
                    //        // 翻訳ID取得
                    //        tranId = registTranId;
                    //        break;
                    //    }
                    //}

                    //if (tranId < 0)
                    //{
                    //    // 工場内に同じ翻訳が存在しない場合、新規登録
                    //    return true;
                    //}
                }

                return false;
            }

            /// <summary>
            /// 翻訳マスタ検索
            /// </summary>
            /// <param name="tranId">翻訳ID</param>
            /// <returns>翻訳マスタ新規登録の場合True</returns>
            bool isNewTranslationByFactory(int searchType, ref int tranId)
            {
                // ユーザ言語のアイテム翻訳に対して工場内に同じ翻訳が存在するか翻訳マスタを検索

                var registTranIds = TMQUtil.SqlExecuteClass.SelectList<int?>(ComMaster.SqlName.GetMsTranslationInfoByFactory, ComMaster.SqlName.SubDir, userTran2, db, listUnComment: new List<string> { "AddItem" });                // 検索実行
                if (searchType == 2)
                {
                    registTranIds = TMQUtil.SqlExecuteClass.SelectList<int?>(ComMaster.SqlName.GetMsTranslationInfoByFactory, ComMaster.SqlName.SubDir, userTran2, db, listUnComment: new List<string> { "AddItem", "AddItem2" });
                }
                if (registTranIds == null)
                {
                    // 同じ翻訳が存在しない場合、新規登録
                    return true;
                }
                else
                {
                    // 同じ翻訳が存在する場合、翻訳ID順の先頭を採用する
                    tranId = (int)registTranIds.First();

                    //// ユーザ言語以外のアイテム翻訳も同じ翻訳かチェックする
                    //foreach (int registTranId in registTranIds)
                    //{
                    //    var chkTran = false;
                    //    foreach (TMQUtil.ItemTranslationForMaster otherTran in otherTranList)
                    //    {
                    //        // 翻訳マスタ検索
                    //        var tranEntity = new ComDao.MsTranslationEntity().GetEntity(otherTran.LocationStructureId, registTranId, otherTran.LanguageId, db);
                    //        if (tranEntity == null)
                    //        {
                    //            // 翻訳が存在しない場合

                    //            if (!string.IsNullOrEmpty(otherTran.TranslationText))
                    //            {
                    //                // 対象のアイテム翻訳が入力されている場合、次の翻訳IDへ
                    //                chkTran = true;
                    //                break;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            // 翻訳が存在する場合

                    //            if (tranEntity.TranslationText != otherTran.TranslationText)
                    //            {
                    //                // 対象のアイテム翻訳と翻訳が異なる場合、次の翻訳IDへ
                    //                chkTran = true;
                    //                break;
                    //            }
                    //        }
                    //    }
                    //    if (!chkTran)
                    //    {
                    //        // 翻訳ID取得
                    //        tranId = registTranId;
                    //        break;
                    //    }
                    //}

                    //if (tranId < 0)
                    //{
                    //    // 工場内に同じ翻訳が存在しない場合、新規登録
                    //    return true;
                    //}
                }

                return false;
            }

            /// <summary>
            /// 翻訳マスタ登録
            /// </summary>
            /// <param name="tranId">翻訳ID</param>
            /// <returns>エラーの場合False</returns>
            bool registTranslationDb(ref int tranId)
            {
                // 登録するデータクラスを作成
                ComDao.MsTranslationEntity registInfo = new();

                // ユーザ言語のアイテム翻訳情報を新規登録
                registInfo.LocationStructureId = userTran.LocationStructureId;
                registInfo.LanguageId = userTran.LanguageId;
                registInfo.TranslationText = userTran.TranslationText;
                SetCommonDataBaseClass(now, ref registInfo, userId, userId);

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out tranId, ComMaster.SqlName.InsertMsTranslationInfoGetTranslationId, ComMaster.SqlName.SubDir, registInfo, db))
                {
                    return false;
                }

                // ユーザ言語以外のアイテム翻訳情報を新規登録
                foreach (TMQUtil.ItemTranslationForMaster otherTran in otherTranList)
                {
                    //if (string.IsNullOrEmpty(otherTran.TranslationText))
                    //{
                    //    // アイテム翻訳が未入力の場合、次の言語へ
                    //    continue;
                    //}

                    registInfo = new();
                    registInfo.LocationStructureId = otherTran.LocationStructureId;
                    registInfo.TranslationId = tranId;
                    registInfo.LanguageId = otherTran.LanguageId;
                    registInfo.TranslationText = otherTran.TranslationText;
                    SetCommonDataBaseClass(now, ref registInfo, userId, userId);

                    // 登録SQL実行
                    if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsTranslationInfo, ComMaster.SqlName.SubDir, registInfo, db))
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// 翻訳マスタ更新
            /// </summary>
            /// <param name="tranId">翻訳ID</param>
            /// <returns>エラーの場合False</returns>
            bool updateTranslationDb(ref int tranId)
            {
                // 翻訳IDの翻訳を、画面のアイテム翻訳に更新する

                // ユーザ言語以外のアイテム翻訳情報
                foreach (TMQUtil.ItemTranslationForMaster otherTran in otherTranList)
                {
                    // 登録するデータクラスを作成
                    ComDao.MsTranslationEntity registInfo = new();

                    // ユーザ言語のアイテム翻訳情報を新規登録
                    registInfo.LocationStructureId = userTran.LocationStructureId;
                    registInfo.TranslationId = tranId;
                    registInfo.LanguageId = otherTran.LanguageId;
                    registInfo.TranslationText = otherTran.TranslationText;
                    SetCommonDataBaseClass(now, ref registInfo, userId, userId);

                    // 翻訳マスタ情報取得
                    var transInfo = new ComDao.MsTranslationEntity().GetEntity(otherTran.LocationStructureId, tranId, otherTran.LanguageId, db);
                    if (transInfo == null)
                    {
                        // 翻訳マスタに存在しない場合、新規登録

                        // 登録SQL実行
                        if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsTranslationInfo, ComMaster.SqlName.SubDir, registInfo, db))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // 翻訳マスタに存在する場合、画面のアイテム翻訳に更新

                        if (hiddenInfo.FormType == (int)ComMaster.FormType.Regist && string.IsNullOrEmpty(otherTran.TranslationText))
                        {
                            // 登録画面の翻訳が未入力の場合、更新対象外
                            continue;
                        }

                        if (transInfo.TranslationText == otherTran.TranslationText)
                        {
                            // 翻訳が同じ場合、更新対象外
                            continue;
                        }

                        // 翻訳が異なる場合、翻訳マスタ更新

                        // 登録SQL実行
                        if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsTranslationInfo, ComMaster.SqlName.SubDir, registInfo, db))
                        {
                            return false;
                        }

                    }
                }

                return true;
            }

            /// <summary>
            /// 翻訳マスタ登録(標準アイテムの工場個別翻訳)
            /// </summary>
            /// <param name="tranId">翻訳ID</param>
            /// <returns>エラーの場合False</returns>
            bool registTranslationDbByFactory(ref int tranId)
            {
                // 翻訳ID取得
                if (userTran.TranslationId != null)
                {
                    tranId = (int)userTran.TranslationId;
                }

                foreach (TMQUtil.ItemTranslationForMaster itemTran in itemTranList)
                {

                    if (itemTran.TranslationText == itemTran.TranslationTextBk)
                    {
                        // 翻訳が変更されていない場合、次の言語へ
                        continue;
                    }

                    // 登録するデータクラスを作成
                    ComDao.MsTranslationEntity registInfo = new();
                    registInfo.LocationStructureId = itemTran.LocationStructureId;
                    registInfo.TranslationId = tranId;
                    registInfo.LanguageId = itemTran.LanguageId;
                    registInfo.TranslationText = itemTran.TranslationText;
                    SetCommonDataBaseClass(now, ref registInfo, userId, userId);

                    if (string.IsNullOrEmpty(itemTran.TranslationText))
                    {
                        // 翻訳が未入力の場合、翻訳マスタ削除
                        if (!DeleteDb(registInfo, ComMaster.SqlName.DeleteMsTranslationInfo, db))
                        {
                            return false;
                        }
                        continue;
                    }

                    if (string.IsNullOrEmpty(itemTran.TranslationTextBk))
                    {
                        // 翻訳が新規入力の場合、翻訳マスタ新規登録

                        // 登録SQL実行
                        if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsTranslationInfo, ComMaster.SqlName.SubDir, registInfo, db))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // 翻訳が変更された場合、翻訳マスタ更新

                        // 登録SQL実行
                        if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsTranslationInfo, ComMaster.SqlName.SubDir, registInfo, db))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// アイテムマスタ登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemTranList">アイテム翻訳一覧</param>
        /// <param name="tranId">翻訳ID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="isExclusiveError">排他チェックエラー有無(エラーの場合true)</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool RegistItem(
            DateTime now,
            HiddenInfoForMaster hiddenInfo,
            List<ItemTranslationForMaster> itemTranList,
            int tranId,
            string userId,
            ref int itemId,
            ref bool isExclusiveError,
            ComDB db)
        {
            // ユーザ言語のアイテム翻訳情報取得
            var userTran = itemTranList.Where(x => x.LanguageId == hiddenInfo.LanguageId).FirstOrDefault();

            if (hiddenInfo.FormType == (int)ComMaster.FormType.Regist)
            {
                // 登録画面

                // 登録するデータクラスを作成
                ComDao.MsItemEntity registInfo = new();
                registInfo.StructureGroupId = hiddenInfo.StructureGroupId;
                registInfo.ItemTranslationId = tranId;
                SetCommonDataBaseClass(now, ref registInfo, userId, userId);

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out itemId, ComMaster.SqlName.InsertMsItemInfo, ComMaster.SqlName.SubDir, registInfo, db))
                {
                    return false;
                }
            }
            else
            {
                // 修正画面

                // アイテムID取得
                itemId = (int)userTran.ItemId;

                // アイテムマスタ検索
                var itemEntity = new ComDao.MsItemEntity().GetEntity(itemId, db);
                if (itemEntity == null)
                {
                    // アイテムマスタに存在しない場合、エラーを返す
                    return false;
                }

                // 翻訳が変更されているか
                if (itemEntity.ItemTranslationId == tranId)
                {
                    // 変更されていない場合、処理終了
                    return true;
                }

                // 変更されている場合

                // 排他チェック
                if (itemEntity.UpdateSerialid > userTran.ItemUpdateSerialid)
                {
                    isExclusiveError = true;
                    return false;
                }

                // 登録するデータクラスを作成
                ComDao.MsItemEntity registInfo = new();
                registInfo.ItemId = itemId;
                registInfo.ItemTranslationId = tranId;
                SetCommonDataBaseClass(now, ref registInfo, userId);

                // アイテムマスタ更新

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsItemInfo, ComMaster.SqlName.SubDir, registInfo, db))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// アイテムマスタ拡張登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemInfo">アイテム情報</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="itemExCnt">拡張項目件数</param>
        /// <param name="isExclusiveError">排他チェックエラー有無(エラーの場合true)</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool RegistItemEx<T>(
            DateTime now,
            HiddenInfoForMaster hiddenInfo,
            T itemInfo,
            int itemId,
            string userId,
            int itemExCnt,
            ref bool isExclusiveError,
            ComDB db)
            where T : CommonResultItemForMaster, new()
        {
            if (hiddenInfo.TargetItemList == (int)ComMaster.TargetItemList.Standard && hiddenInfo.FactoryId != Const.CommonFactoryId)
            {
                // 工場管理者の標準アイテム修正画面の場合、登録対象外
                return true;
            }

            // 拡張項目件数分繰り返し
            for (int i = 1; i <= itemExCnt; i++)
            {
                // 登録するデータクラスを作成
                ComDao.MsItemExtensionEntity registInfo = new();
                registInfo.ItemId = itemId;
                registInfo.SequenceNo = i;
                switch (i)
                {
                    case 1:
                        registInfo.ExtensionData = itemInfo.ExData1;
                        registInfo.UpdateSerialid = itemInfo.UpdateSerialidEx1;
                        break;
                    case 2:
                        registInfo.ExtensionData = itemInfo.ExData2;
                        registInfo.UpdateSerialid = itemInfo.UpdateSerialidEx2;
                        break;
                    case 3:
                        registInfo.ExtensionData = itemInfo.ExData3;
                        registInfo.UpdateSerialid = itemInfo.UpdateSerialidEx3;
                        break;
                    case 4:
                        registInfo.ExtensionData = itemInfo.ExData4;
                        registInfo.UpdateSerialid = itemInfo.UpdateSerialidEx4;
                        break;
                    case 5:
                        registInfo.ExtensionData = itemInfo.ExData5;
                        registInfo.UpdateSerialid = itemInfo.UpdateSerialidEx5;
                        break;
                    case 6:
                        registInfo.ExtensionData = itemInfo.ExData6;
                        registInfo.UpdateSerialid = itemInfo.UpdateSerialidEx6;
                        break;
                    case 7:
                        registInfo.ExtensionData = itemInfo.ExData7;
                        registInfo.UpdateSerialid = itemInfo.UpdateSerialidEx7;
                        break;
                    case 8:
                        registInfo.ExtensionData = itemInfo.ExData8;
                        registInfo.UpdateSerialid = itemInfo.UpdateSerialidEx8;
                        break;
                    case 9:
                        registInfo.ExtensionData = itemInfo.ExData9;
                        registInfo.UpdateSerialid = itemInfo.UpdateSerialidEx9;
                        break;
                    case 10:
                        registInfo.ExtensionData = itemInfo.ExData10;
                        registInfo.UpdateSerialid = itemInfo.UpdateSerialidEx10;
                        break;
                    default:
                        break;
                }

                // アイテムマスタ拡張検索
                var itemExEntity = new ComDao.MsItemExtensionEntity().GetEntity(itemId, i, db);
                if (itemExEntity == null)
                {
                    // アイテムマスタ拡張に存在しない場合、新規登録

                    // 実行条件の共通項目設定
                    SetCommonDataBaseClass(now, ref registInfo, userId, userId);

                    // 登録SQL実行
                    if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsItemExtensionInfo, ComMaster.SqlName.SubDir, registInfo, db))
                    {
                        return false;
                    }
                }
                else
                {
                    // アイテムマスタ拡張に存在する場合、拡張データを比較
                    if (itemExEntity.ExtensionData == registInfo.ExtensionData)
                    {
                        // 変更されていない場合、更新対象外
                        continue;
                    }

                    // 排他チェック
                    if (itemExEntity.UpdateSerialid > registInfo.UpdateSerialid)
                    {
                        isExclusiveError = true;
                        return false;
                    }

                    // アイテムマスタ拡張更新

                    // 実行条件の共通項目設定
                    SetCommonDataBaseClass(now, ref registInfo, userId);

                    // 登録SQL実行
                    if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsItemExtensionInfo, ComMaster.SqlName.SubDir, registInfo, db))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 構成マスタ登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemInfo">アイテム情報</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="structureId">構成ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool RegistStructure<T>(
            DateTime now,
            TMQUtil.HiddenInfoForMaster hiddenInfo,
            T itemInfo,
            int itemId,
            string userId,
            ref int structureId,
            ComDB db
            )
            where T : CommonResultItemForMaster, new()
        {
            // 登録するデータクラスを作成
            ComDao.MsStructureEntity registInfo = new();
            registInfo.FactoryId = hiddenInfo.FactoryId;
            registInfo.StructureGroupId = hiddenInfo.StructureGroupId;
            registInfo.ParentStructureId = hiddenInfo.ParentStructureId;
            registInfo.StructureLayerNo = hiddenInfo.StructureLayerNo;
            registInfo.StructureItemId = itemId;
            registInfo.DeleteFlg = itemInfo.DeleteFlg;
            SetCommonDataBaseClass(now, ref registInfo, userId, userId);

            if (hiddenInfo.FormType == (int)ComMaster.FormType.Regist)
            {
                // 登録画面

                // 構成マスタ新規登録

                // 削除フラグ設定
                registInfo.DeleteFlg = false;

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out structureId, ComMaster.SqlName.InsertMsStructureInfo, ComMaster.SqlName.SubDir, registInfo, db))
                {
                    return false;
                }
            }
            else
            {
                // 修正画面

                // 構成ID取得
                structureId = itemInfo.StructureId;

                if (hiddenInfo.TargetItemList == (int)ComMaster.TargetItemList.Standard && hiddenInfo.FactoryId != Const.CommonFactoryId)
                {
                    // 工場管理者の標準アイテムの場合、処理終了
                    return true;
                }

                // 構成マスタ検索
                var stEntity = new ComDao.MsStructureEntity().GetEntity(structureId, db);
                if (stEntity == null)
                {
                    // 構成マスタに存在しない場合、エラーを返す
                    return false;
                }

                // 削除フラグを比較
                if (stEntity.DeleteFlg == registInfo.DeleteFlg)
                {
                    // 変更されていない場合、更新対象外
                    return true;
                }

                // 構成マスタ更新

                // 構成ID設定
                registInfo.StructureId = structureId;

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsStructureInfo, ComMaster.SqlName.SubDir, registInfo, db))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 子のアイテムを削除する
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="structureId">構成ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool UpdateChildLayers(
            DateTime now,
            TMQUtil.HiddenInfoForMaster hiddenInfo,
            int structureId,
            string userId,
            ComDB db)
        {
            // 子のアイテムの構成IDを取得する
            string childIdList = string.Empty;

            // 検索条件
            var condition = new TMQUtil.HiddenInfoForMaster();
            condition.StructureGroupId = hiddenInfo.StructureGroupId;
            condition.FactoryId = hiddenInfo.FactoryId;
            condition.StructureIdList = structureId.ToString();

            while (true)
            {
                // 検索実行
                var results = TMQUtil.SqlExecuteClass.SelectList<int>(ComMaster.SqlName.GetChildStructureIdList, ComMaster.SqlName.ComLayersDir, condition, db);
                if (results == null || results.Count == 0)
                {
                    // ループを抜ける
                    break;
                }

                // 検索条件の初期化
                condition.StructureIdList = string.Empty;

                foreach (var result in results)
                {
                    // 取得した構成IDを子の構成IDリストにセットする
                    childIdList = childIdList + ',' + result.ToString();

                    // 検索条件
                    condition.StructureIdList = condition.StructureIdList + ',' + result.ToString();
                }

                // 検索条件の先頭文字(,)を削除
                condition.StructureIdList = condition.StructureIdList.Substring(1);

            }

            if (string.IsNullOrEmpty(childIdList))
            {
                // 子のアイテムがない場合、更新対象外
                return true;
            }

            // 構成マスタ更新

            // 登録するデータクラスを作成
            TMQUtil.CommonChildLayersItemForMaster registInfo = new();
            registInfo.StructureIdList = childIdList.Substring(1);
            registInfo.StructureGroupId = hiddenInfo.StructureGroupId;
            SetCommonDataBaseClass(now, ref registInfo, userId);

            // 登録SQL実行
            if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateChildLayersAddDeleteFlg, ComMaster.SqlName.ComLayersDir, registInfo, db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 工場別未使用標準アイテムマスタ登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemInfo">アイテム情報</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool RegistItemUnused<T>(
            DateTime now,
            HiddenInfoForMaster hiddenInfo,
            T itemInfo,
            string userId,
            ComDB db)
            where T : CommonResultItemForMaster, new()
        {
            if (hiddenInfo.TargetItemList == (int)ComMaster.TargetItemList.Standard && hiddenInfo.FactoryId != Const.CommonFactoryId)
            {
                // 工場管理者の標準アイテム修正画面の場合

                // 登録するデータクラスを作成
                ComDao.MsStructureUnusedEntity registInfo = new();
                registInfo.StructureId = itemInfo.StructureId;
                registInfo.FactoryId = hiddenInfo.FactoryId;
                registInfo.StructureGroupId = itemInfo.StructureGroupId;
                SetCommonDataBaseClass(now, ref registInfo, userId, userId);

                // 工場別未使用標準アイテムマスタ検索
                var unusedEntity = new ComDao.MsStructureUnusedEntity().GetEntity(registInfo.StructureId, registInfo.FactoryId, db);
                if (unusedEntity == null)
                {
                    // 工場別未使用標準アイテムマスタに存在しない場合

                    // 登録前と比較
                    if (!itemInfo.UnusedFlg)
                    {
                        // 変更されていない場合、登録対象外
                        return true;
                    }
                }
                else
                {
                    // 工場別未使用標準アイテムマスタに存在する場合

                    // 登録前と比較
                    if (itemInfo.UnusedFlg)
                    {
                        // 変更されていない場合、登録対象外
                        return true;
                    }
                }

                if (!itemInfo.UnusedFlg)
                {
                    // 未使用フラグが未選択の場合

                    // 工場別未使用標準アイテムマスタ削除
                    if (!DeleteDb<ComDao.MsStructureUnusedEntity>(registInfo, ComMaster.SqlName.DeleteMsStructureUnused, db))
                    {
                        return false;
                    }
                }
                else
                {
                    // 未使用フラグが選択済の場合

                    // 工場別未使用標準アイテムマスタ新規登録

                    // 登録SQL実行
                    if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsStructureUnused, ComMaster.SqlName.SubDir, registInfo, db))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// 構成マスタ登録
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="itemInfo">アイテム情報</param>
        /// <param name="LayerNo">階層番号</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="structureId">構成ID</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool DeleteChildStructure<T>(
            DateTime now,
            TMQUtil.HiddenInfoForMaster hiddenInfo,
            T itemInfo,
            int LayerNo,
            int itemId,
            string userId,
            ref int structureId,
            ComDB db
            )
            where T : CommonResultItemForMaster, new()
        {
            // 登録するデータクラスを作成
            ComDao.MsStructureEntity registInfo = new();
            registInfo.FactoryId = hiddenInfo.FactoryId;
            registInfo.StructureGroupId = hiddenInfo.StructureGroupId;
            registInfo.ParentStructureId = hiddenInfo.ParentStructureId;
            registInfo.StructureLayerNo = hiddenInfo.StructureLayerNo;
            registInfo.StructureItemId = itemId;
            registInfo.DeleteFlg = itemInfo.DeleteFlg;
            SetCommonDataBaseClass(now, ref registInfo, userId, userId);

            if (hiddenInfo.FormType == (int)ComMaster.FormType.Edit)
            {
                // 修正画面

                // 構成ID取得
                structureId = itemInfo.StructureId;

                if (hiddenInfo.TargetItemList == (int)ComMaster.TargetItemList.Standard && hiddenInfo.FactoryId != Const.CommonFactoryId)
                {
                    // 工場管理者の標準アイテムの場合、処理終了
                    return true;
                }

                // 構成マスタ検索
                var stEntity = new ComDao.MsStructureEntity().GetEntity(structureId, db);
                if (stEntity == null)
                {
                    // 構成マスタに存在しない場合、エラーを返す
                    return false;
                }

                // 削除フラグを比較
                if (stEntity.DeleteFlg == registInfo.DeleteFlg)
                {
                    // 変更されていない場合、更新対象外
                    return true;
                }

                // 構成マスタ更新

                // 構成ID設定
                registInfo.ParentStructureId = structureId;
                registInfo.StructureLayerNo = LayerNo;

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateLayerMsStructureInfo, ComMaster.SqlName.ComLayersDir, registInfo, db))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 工場別アイテム表示順マスタ登録
        /// </summary>
        /// <param name="hiddenInfo">非表示情報</param>
        /// <param name="registInfoList">登録情報リスト</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool RegistItemOrder(
            HiddenInfoForMaster hiddenInfo,
            List<ComDao.MsStructureOrderEntity> registInfoList,
            ComDB db)
        {
            // 工場別アイテム表示順マスタ削除
            if (!DeleteDb(hiddenInfo, ComMaster.SqlName.DeleteMsStructureOrder, db))
            {
                return false;
            }

            // 工場別アイテム表示順マスタ登録
            foreach (ComDao.MsStructureOrderEntity registInfo in registInfoList)
            {
                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsStructureOrder, ComMaster.SqlName.SubDir, registInfo, db))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 実行条件の共通項目(更新日時等)設定
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <param name="condition">ref 実行条件を設定するデータクラス</param>
        /// <param name="updatorCd">更新ユーザID</param>
        /// <param name="inputorCd">登録ユーザID</param>
        /// <returns>エラーの場合False</returns>
        public static bool SetCommonDataBaseClass<T>(
            DateTime now,
            ref T condition,
            string updatorCd,
            string inputorCd = "-1")
            where T : ComDao.CommonTableItem
        {
            // 更新者ID、登録者IDの変換
            bool chkUpd = int.TryParse(updatorCd, out int updatorCdNum);
            bool chkInp = int.TryParse(inputorCd, out int inputorCdNum);
            // いずれかが変換エラーの場合、エラーを返す
            if (!chkUpd || !chkInp)
            {
                return false;
            }

            // 更新者IDと更新日時を設定
            condition.UpdateDatetime = now;
            condition.UpdateUserId = updatorCdNum;
            if (inputorCdNum != -1)
            {
                // 登録者IDと登録日時、更新シリアルIDを設定
                condition.InsertDatetime = now;
                condition.InsertUserId = inputorCdNum;
                condition.UpdateSerialid = 0;
            }

            return true;
        }

        /// <summary>
        /// 指定した件数取得SQLを実行
        /// </summary>
        /// <param name="condition">削除条件</param>
        /// <param name="sqlName">SQLファイル名</param>
        /// <param name="cnt">取得件数</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="subDir">SQL格納先サブディレクトリ名</param>
        /// <param name="listUnComment">コメントアウト文字列リスト</param>
        /// <returns>エラーの場合False</returns>
        public static bool GetCountDb<T>(
            T condition,
            string sqlName,
            ref int cnt,
            ComDB db,
            string subDir = ComMaster.SqlName.SubDir,
            List<string> listUnComment = null)
        {
            // SQL取得
            if (!TMQUtil.GetFixedSqlStatement(subDir, sqlName, out string baseSql, listUnComment))
            {
                return false;
            }
            StringBuilder executeSql = new();
            executeSql.AppendLine(baseSql);

            // 件数取得
            cnt = db.GetCount(executeSql.ToString(), condition);

            return true;
        }

        /// <summary>
        /// 指定した削除SQLを実行
        /// </summary>
        /// <param name="condition">削除条件</param>
        /// <param name="sqlName">SQLファイル名</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="sqlName">SQL格納先サブディレクトリ名</param>
        /// <returns>エラーの場合False</returns>
        public static bool DeleteDb<T>(
            T condition,
            string sqlName,
            ComDB db,
            string subDir = ComMaster.SqlName.SubDir)
        {
            // SQL取得
            if (!TMQUtil.GetFixedSqlStatement(subDir, sqlName, out string baseSql))
            {
                return false;
            }

            // SQL実行
            int result = db.Regist(baseSql, condition);
            if (result < 0)
            {
                // 削除エラー
                return false;
            }
            return true;
        }
        #endregion
        #endregion
    }
}