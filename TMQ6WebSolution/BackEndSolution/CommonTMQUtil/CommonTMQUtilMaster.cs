using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using Const = CommonTMQUtil.CommonTMQConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using ComRes = CommonSTDUtil.CommonResources;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;

namespace CommonTMQUtil
{
    /// <summary>
    /// TMQ用共通ユーティリティクラス
    /// </summary>
    public static partial class CommonTMQUtil
    {
        #region マスタメンテナンス共通処理
        /// <summary>
        /// ExcelPortダウンロード画面の機能ID
        /// </summary>
        public const string ConductIdEP0001 = "EP0001";

        /// <summary>
        /// アイテム翻訳の桁数
        /// </summary>
        public const int ItemTranslasionMaxLength = 400;

        /// <summary>
        /// マスタメンテナンス共通
        /// </summary>
        public class ComMaster
        {
            #region 定数
            /// <summary>
            /// ＜構成グループID：構成グループ名称＞のディクショナリ
            /// </summary>
            public static Dictionary<int, int> MasterNameTranslation = new()
            {
                { 1150, 111340003 }, // メーカー
                { 1540, 111020044 }, // 依頼番号採番パターン
                { 1420, 111020002 }, // 依頼部課係
                { 1770, 111060022 }, // 勘定科目
                { 1100, 111060061 }, // 環境安全管理
                { 2050, 111310005 }, // 丸め処理区分
                { 1170, 111070003 }, // 機器レベル
                { 1070, 111070017 }, // 緊急度
                { 1740, 111070042 }, // 金額管理単位
                { 1130, 111090006 }, // 系停止
                { 1470, 111100010 }, // 故障性格分類
                { 1520, 111100009 }, // 故障性格要因
                { 1560, 111100021 }, // 故障分析
                { 1430, 111100006 }, // 工事区分
                { 2020, 111100067 }, // 工場毎年度期首月
                { 1550, 111110022 }, // 作業/故障区分
                { 1410, 111110005 }, // 作業区分
                { 1280, 111110004 }, // 作業項目
                { 1290, 111110012 }, // 作業目的
                { 1720, 111120066 }, // 仕入先
                { 1340, 111120003 }, // 仕様項目選択肢
                { 1210, 111140013 }, // 使用区分
                { 1450, 111120010 }, // 施工会社
                { 1330, 111120026 }, // 時期
                { 1440, 111120027 }, // 自・他責
                { 1140, 111120023 }, // 実績結果
                { 1380, 111120001 }, // 修繕費分類
                { 1200, 111120011 }, // 重要度
                { 1310, 111120021 }, // 処置区分
                { 1050, 111120127 }, // 処置対策
                { 1000, 111260021 }, // 場所階層
                { 1010, 111120081 }, // 職種・機種
                { 1940, 111120068 }, // 新旧区分
                { 1730, 111130009 }, // 数量管理単位
                { 1320, 111140008 }, // 設備区分
                { 1820, 111160005 }, // 対策分類１
                { 1830, 111160006 }, // 対策分類２
                { 1160, 111190001 }, // 適用法規
                { 1080, 111260018 }, // 発見方法
                { 1180, 111280057 }, // 部位マスタ
                { 1760, 111280058 }, // 部門(工場・部門)
                { 1230, 111300004 }, // 保全区分
                { 1220, 111300062 }, // 保全項目(点検内容)
                { 1930, 111300017 }, // 保全部課係
                { 1300, 111380003 }, // 予算管理区分
                { 1060, 111380004 }, // 予算性格区分
                { 1040, 111380060 }, // 予備品ロケーション(予備品倉庫・棚)
                { 1090, 111290002 }  // 変更管理
            };

            #region ExcelPort シート番号
            /// <summary>
            /// ExcelPortマスタメンテナンス 「標準アイテム未使用」のシート番号
            /// </summary>
            public const int UnuseSheetNo = 13;
            /// <summary>
            /// ExcelPortマスタメンテナンス 「標準アイテム未使用」シートの工場の列番号
            /// </summary>
            public const int UnuseFactoryColNo = 8;
            /// <summary>
            /// ExcelPortマスタメンテナンス 「並び順」のシート番号
            /// </summary>
            public const int OrdeerSheetNo = 14;
            #endregion

            /// <summary>
            /// ExcelPortマスタメンテナンス対象機能用 データ取得クラス
            /// </summary>
            public class MaintainanceTargetExInfo
            {
                /// <summary>
                /// 構成グループID
                /// </summary>
                public const int StructureGroupId = 2160;
                /// <summary>
                /// 連番
                /// </summary>
                public const int Seq = 3;

                /// <summary>
                /// 拡張アイテム
                /// </summary>
                public class ExData
                {
                    /// <summary>
                    /// マスタアイテム
                    /// </summary>
                    public const string MasterItem = "1";
                    /// <summary>
                    /// 標準アイテム未使用設定
                    /// </summary>
                    public const string UnUse = "2";
                    /// <summary>
                    /// マスタ並び順設定
                    /// </summary>
                    public const string Oerder = "3";
                }
            }

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
                /// <summary>SQL名：ExcelPortマスタアイテム取得</summary>
                public const string GetMsTranslationInfoByFactory = "GetMsTranslationInfoByFactory";
                /// <summary>SQL名：ExcelPortマスタ情報取得</summary>
                public const string GetExcelPortMasterList = "GetExcelPortMasterList";
                /// <summary>SQL名：ExcelPort地区情報取得</summary>
                public const string GetExcelPortMasterDistrictList = "GetExcelPortMasterDistrictList";
                /// <summary>SQL名：ExcelPort工場情報取得</summary>
                public const string GetExcelPortMasterFactoryList = "GetExcelPortMasterFactoryList";
                /// <summary>SQL名：ExcelPortプラント情報取得</summary>
                public const string GetExcelPortMasterPlantList = "GetExcelPortMasterPlantList";
                /// <summary>SQL名：ExcelPort系列情報取得</summary>
                public const string GetExcelPortMasterSeriesList = "GetExcelPortMasterSeriesList";
                /// <summary>SQL名：ExcelPort工程情報取得</summary>
                public const string GetExcelPortMasterStrokeList = "GetExcelPortMasterStrokeList";
                /// <summary>SQL名：ExcelPort設備情報取得</summary>
                public const string GetExcelPortMasterFacilityList = "GetExcelPortMasterFacilityList";
                /// <summary>SQL名：ExcelPort職種情報取得</summary>
                public const string GetExcelPortMasterJobList = "GetExcelPortMasterJobList";
                /// <summary>SQL名：ExcelPort機種大分類情報取得</summary>
                public const string GetExcelPortMasterLargeClassList = "GetExcelPortMasterLargeClassList";
                /// <summary>SQL名：ExcelPort機種小分類情報取得</summary>
                public const string GetExcelPortMasterMiddleClassList = "GetExcelPortMasterMiddleClassList";
                /// <summary>SQL名：ExcelPort機種小分類情報取得</summary>
                public const string GetExcelPortMasterSmallClassList = "GetExcelPortMasterSmallClassList";
                /// <summary>SQL名：ExcelPort倉庫情報取得</summary>
                public const string GetExcelPortMasterWarehouseList = "GetExcelPortMasterWarehouseList";
                /// <summary>SQL名：ExcelPort棚情報取得</summary>
                public const string GetExcelPortMasterRackList = "GetExcelPortMasterRackList";
                /// <summary>SQL名：ExcelPort部門報取得</summary>
                public const string GetExcelPortMasterDepartmentList = "GetExcelPortMasterDepartmentList";
                /// <summary>SQL名：ExcelPort並び順取得</summary>
                public const string GetExcelPortItemOrderList = "GetExcelPortItemOrderList";
                /// <summary>SQL名：ExcelPort階層系並び順取得</summary>
                public const string GetExcelPortStructureItemOrderList = "GetExcelPortStructureItemOrderList";
                /// <summary>SQL名：ExcelPort標準アイテム未使用取得</summary>
                public const string GetExcelPortItemUnUseList = "GetExcelPortItemUnUseList";
                /// <summary>SQL名：ExcelPort標準アイテム未使用削除(構成グループで削除)</summary>
                public const string DeleteExcelPortItemUnUseList = "DeleteExcelPortItemUnUseList";

                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = @"Master";
                public const string ComLayersDir = @"Master\ComLayers";
                public const string ExcelPortDir = @"Master\ExcelPort";
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

                /// <summary>
                /// ExcelPortダウンロード条件画面
                /// </summary>
                public static class FormExcelPortDownCondition
                {
                    /// <summary>
                    /// コントロールID
                    /// </summary>
                    public static class ControlId
                    {
                        /// <summary>
                        /// コントロールID
                        /// </summary>
                        public const string Condition = "BODY_000_00_LST_1";
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

        /// <summary>
        /// ExcelPortマスタ用共通データクラス
        /// </summary>
        public class CommonExcelPortMasterList : ComDao.MsStructureEntity
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            /// <summary>Gets or sets 初期表示時の工場ID(入力チェック時に使用)</summary>
            /// <value>初期表示時の工場ID(入力チェック時に使用)</value>
            public int? FactoryIdBefore { get; set; }
            /// <summary>Gets or sets アイテムID</summary>
            /// <value>アイテムID</value>
            public int TranslationId { get; set; }
            /// <summary>Gets or sets 工場名</summary>
            /// <value>工場名</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets 翻訳ID</summary>
            /// <value>翻訳ID</value>
            public int ItemId { get; set; }
            /// <summary>Gets or sets アイテム翻訳</summary>
            /// <value>アイテム翻訳</value>
            public string TranslationText { get; set; }
            /// <summary>Gets or sets 初期表示時のアイテム翻訳(入力チェックに使用)</summary>
            /// <value>アイテム翻訳(入力チェックに使用)</value>
            public string TranslationTextBefore { get; set; }
            /// <summary>Gets or sets 拡張項目1</summary>
            /// <value>拡張項目1</value>
            public string ExData1 { get; set; }
            /// <summary>Gets or sets 拡張項目2</summary>
            /// <value>拡張項目2</value>
            public string ExData2 { get; set; }
            /// <summary>Gets or sets 拡張項目3</summary>
            /// <value>拡張項目3</value>
            public string ExData3 { get; set; }
            /// <summary>Gets or sets 拡張項目4</summary>
            /// <value>拡張項目4</value>
            public string ExData4 { get; set; }
            /// <summary>Gets or sets 拡張項目5</summary>
            /// <value>拡張項目5</value>
            public string ExData5 { get; set; }
            /// <summary>Gets or sets 拡張項目6</summary>
            /// <value>拡張項目6</value>
            public string ExData6 { get; set; }
            /// <summary>Gets or sets 拡張項目7</summary>
            /// <value>拡張項目7</value>
            public string ExData7 { get; set; }
            /// <summary>Gets or sets 拡張項目8</summary>
            /// <value>拡張項目8</value>
            public string ExData8 { get; set; }
            /// <summary>Gets or sets 拡張項目9</summary>
            /// <value>拡張項目9</value>
            public string ExData9 { get; set; }
            /// <summary>Gets or sets 拡張項目10</summary>
            /// <value>拡張項目10</value>
            public string ExData10 { get; set; }
        }

        /// <summary>
        /// ExcelPortマスタ用場場所階層系共通データクラス
        /// </summary>
        public class CommonExcelPortMasterStructureList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }
            /// <summary>Gets or sets 地区番号</summary>
            /// <value>地区番号</value>
            public int? DistrictNumber { get; set; }
            /// <summary>Gets or sets 地区ID(構成ID)</summary>
            /// <value>地区ID(構成ID)</value>
            public long? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名</summary>
            /// <value>地区名</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? FactoryNumber { get; set; }
            /// <summary>Gets or sets 工場ID(構成ID)</summary>
            /// <value>工場ID(構成ID)</value>
            public long? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名</summary>
            /// <value>工場名</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets 工場の親構成ID</summary>
            /// <value>工場の親構成ID</value>
            public int? FactoryParentId { get; set; }
            /// <summary>Gets or sets 地区番号</summary>
            /// <value>地区番号</value>
            public int? FactoryParentNumber { get; set; }
            /// <summary>Gets or sets プラントID(構成ID)</summary>
            /// <value>プラントID(構成ID)</value>
            public long? PlantId { get; set; }
            /// <summary>Gets or sets プラントアイテムID</summary>
            /// <value>プラントアイテムID</value>
            public int? PlantItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(プラント)</summary>
            /// <value>翻訳ID(プラント)</value>
            public int? PlantItemTranslationId { get; set; }
            /// <summary>Gets or sets プラント番号</summary>
            /// <value>プラント番号</value>
            public int? PlantNumber { get; set; }
            /// <summary>Gets or sets プラント名</summary>
            /// <value>プラント名</value>
            public string PlantName { get; set; }
            /// <summary>Gets or sets プラント名(変更前)</summary>
            /// <value>プラント名(変更前)</value>
            public string PlantNameBefore { get; set; }
            /// <summary>Gets or sets プラントの親構成ID</summary>
            /// <value>プラントの親構成ID</value>
            public int? PlantParentId { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? PlantParentNumber { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? PlantParentNumberBefore { get; set; }
            /// <summary>Gets or sets 系列ID(構成ID)</summary>
            /// <value>系列ID(構成ID)</value>
            public long? SeriesId { get; set; }
            /// <summary>Gets or sets 系列アイテムID</summary>
            /// <value>系列アイテムID</value>
            public int? SeriesItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(系列)</summary>
            /// <value>翻訳ID(系列)</value>
            public int? SeriesItemTranslationId { get; set; }
            /// <summary>Gets or sets 系列番号</summary>
            /// <value>系列番号</value>
            public int? SeriesNumber { get; set; }
            /// <summary>Gets or sets 系列名</summary>
            /// <value>系列名</value>
            public string SeriesName { get; set; }
            /// <summary>Gets or sets 系列名(変更前)</summary>
            /// <value>系列名(変更前)</value>
            public string SeriesNameBefore { get; set; }
            /// <summary>Gets or sets 系列の親構成ID</summary>
            /// <value>系列の親構成ID</value>
            public int? SeriesParentId { get; set; }
            /// <summary>Gets or sets プラント番号</summary>
            /// <value>プラント番号</value>
            public int? SeriesParentNumber { get; set; }
            /// <summary>Gets or sets プラント番号</summary>
            /// <value>プラント番号</value>
            public int? SeriesParentNumberBefore { get; set; }
            /// <summary>Gets or sets 工程ID(構成ID)</summary>
            /// <value>工程ID(構成ID)</value>
            public long? StrokeId { get; set; }
            /// <summary>Gets or sets 工程アイテムID</summary>
            /// <value>工程アイテムID</value>
            public int? StrokeItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(工程)</summary>
            /// <value>翻訳ID(工程)</value>
            public int? StrokeItemTranslationId { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int? StrokeNumber { get; set; }
            /// <summary>Gets or sets 工程名</summary>
            /// <value>工程名</value>
            public string StrokeName { get; set; }
            /// <summary>Gets or sets 工程名(変更前)</summary>
            /// <value>工程名(変更前)</value>
            public string StrokeNameBefore { get; set; }
            /// <summary>Gets or sets 工程の親構成ID</summary>
            /// <value>工程の親構成ID</value>
            public int? StrokeParentId { get; set; }
            /// <summary>Gets or sets 系列番号</summary>
            /// <value>系列番号</value>
            public int? StrokeParentNumber { get; set; }
            /// <summary>Gets or sets 系列番号</summary>
            /// <value>系列番号</value>
            public int? StrokeParentNumberBefore { get; set; }
            /// <summary>Gets or sets 設備ID(構成ID)</summary>
            /// <value>設備ID(構成ID)</value>
            public long? FacilityId { get; set; }
            /// <summary>Gets or sets 設備アイテムID</summary>
            /// <value>設備アイテムID</value>
            public int? FacilityItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(設備)</summary>
            /// <value>翻訳ID(設備)</value>
            public int? FacilityItemTranslationId { get; set; }
            /// <summary>Gets or sets 設備番号</summary>
            /// <value>設備番号</value>
            public int? FacilityNumber { get; set; }
            /// <summary>Gets or sets 設備名</summary>
            /// <value>設備名</value>
            public string FacilityName { get; set; }
            /// <summary>Gets or sets 設備名(変更前)</summary>
            /// <value>設備名(変更前)</value>
            public string FacilityNameBefore { get; set; }
            /// <summary>Gets or sets 設備の親構成ID</summary>
            /// <value>設備の親構成ID</value>
            public int? FacilityParentId { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>設備番号</value>
            public int? FacilityParentNumber { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>設備番号</value>
            public int? FacilityParentNumberBefore { get; set; }
        }

        /// <summary>
        /// ExcelPortマスタ用場職種機種階層系共通データクラス
        /// </summary>
        public class CommonExcelPortMasterJobList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }
            /// <summary>Gets or sets 地区番号</summary>
            /// <value>地区番号</value>
            public int? DistrictNumber { get; set; }
            /// <summary>Gets or sets 地区ID(構成ID)</summary>
            /// <value>地区ID(構成ID)</value>
            public long? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名</summary>
            /// <value>地区名</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? FactoryNumber { get; set; }
            /// <summary>Gets or sets 工場ID(構成ID)</summary>
            /// <value>工場ID(構成ID)</value>
            public long? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名</summary>
            /// <value>工場名</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets 工場の親構成ID</summary>
            /// <value>工場の親構成ID</value>
            public int? FactoryParentId { get; set; }
            /// <summary>Gets or sets 地区番号</summary>
            /// <value>地区番号</value>
            public int? FactoryParentNumber { get; set; }
            /// <summary>Gets or sets 職種ID(構成ID)</summary>
            /// <value>職種ID(構成ID)</value>
            public long? JobId { get; set; }
            /// <summary>Gets or sets 職種アイテムID</summary>
            /// <value>職種アイテムID</value>
            public int? JobItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(職種)</summary>
            /// <value>翻訳ID(職種)</value>
            public int? JobItemTranslationId { get; set; }
            /// <summary>Gets or sets 職種番号</summary>
            /// <value>職種番号</value>
            public int? JobNumber { get; set; }
            /// <summary>Gets or sets 職種名</summary>
            /// <value>職種名</value>
            public string JobName { get; set; }
            /// <summary>Gets or sets 職種名(変更前)</summary>
            /// <value>職種名(変更前)</value>
            public string JobNameBefore { get; set; }
            /// <summary>Gets or sets 職種の親構成ID</summary>
            /// <value>職種の親構成ID</value>
            public int? JobParentId { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? JobParentNumber { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? JobParentNumberBefore { get; set; }
            /// <summary>Gets or sets 保全実績集計職種コード</summary>
            /// <value>保全実績集計職種コード</value>
            public string JobCode { get; set; }
            /// <summary>Gets or sets 機種大分類ID(構成ID)</summary>
            /// <value>機種大分類ID(構成ID)</value>
            public long? LargeClassId { get; set; }
            /// <summary>Gets or sets 機種大分類アイテムID</summary>
            /// <value>機種大分類アイテムID</value>
            public int? LargeClassItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(機種大分類)</summary>
            /// <value>翻訳ID(機種大分類)</value>
            public int? LargeClassItemTranslationId { get; set; }
            /// <summary>Gets or sets 機種大分類番号</summary>
            /// <value>機種大分類番号</value>
            public int? LargeClassNumber { get; set; }
            /// <summary>Gets or sets 機種大分類名</summary>
            /// <value>機種大分類名</value>
            public string LargeClassName { get; set; }
            /// <summary>Gets or sets 機種大分類名(変更前)</summary>
            /// <value>機種大分類名(変更前)</value>
            public string LargeClassNameBefore { get; set; }
            /// <summary>Gets or sets 機種大分類の親構成ID</summary>
            /// <value>機種大分類の親構成ID</value>
            public int? LargeClassParentId { get; set; }
            /// <summary>Gets or sets 職種番号</summary>
            /// <value>職種番号</value>
            public int? LargeClassParentNumber { get; set; }
            /// <summary>Gets or sets 職種番号</summary>
            /// <value>職種番号</value>
            public int? LargeClassParentNumberBefore { get; set; }
            /// <summary>Gets or sets 機種中分類ID(構成ID)</summary>
            /// <value>機種中分類ID(構成ID)</value>
            public long? MiddleClassId { get; set; }
            /// <summary>Gets or sets 機種中分類アイテムID</summary>
            /// <value>機種中分類アイテムID</value>
            public int? MiddleClassItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(機種中分類)</summary>
            /// <value>翻訳ID(機種中分類)</value>
            public int? MiddleClassItemTranslationId { get; set; }
            /// <summary>Gets or sets 機種中分類番号</summary>
            /// <value>機種中分類番号</value>
            public int? MiddleClassNumber { get; set; }
            /// <summary>Gets or sets 機種中分類名</summary>
            /// <value>機種中分類名</value>
            public string MiddleClassName { get; set; }
            /// <summary>Gets or sets 機種中分類名(変更前)</summary>
            /// <value>機種中分類名(変更前)</value>
            public string MiddleClassNameBefore { get; set; }
            /// <summary>Gets or sets 機種中分類の親構成ID</summary>
            /// <value>機種中分類の親構成ID</value>
            public int? MiddleClassParentId { get; set; }
            /// <summary>Gets or sets 機種大分類番号</summary>
            /// <value>機種大分類番号</value>
            public int? MiddleClassParentNumber { get; set; }
            /// <summary>Gets or sets 機種大分類番号</summary>
            /// <value>機種大分類番号</value>
            public int? MiddleClassParentNumberBefore { get; set; }
            /// <summary>Gets or sets 機種小分類ID(構成ID)</summary>
            /// <value>機種小分類ID(構成ID)</value>
            public long? SmallClassId { get; set; }
            /// <summary>Gets or sets 機種小分類アイテムID</summary>
            /// <value>機種小分類アイテムID</value>
            public int? SmallClassItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(機種小分類)</summary>
            /// <value>翻訳ID(機種小分類)</value>
            public int? SmallClassItemTranslationId { get; set; }
            /// <summary>Gets or sets 機種小分類番号</summary>
            /// <value>機種小分類番号</value>
            public int? SmallClassNumber { get; set; }
            /// <summary>Gets or sets 機種小分類名</summary>
            /// <value>機種小分類名</value>
            public string SmallClassName { get; set; }
            /// <summary>Gets or sets 機種小分類名(変更前)</summary>
            /// <value>機種小分類名(変更前)</value>
            public string SmallClassNameBefore { get; set; }
            /// <summary>Gets or sets 機種小分類の親構成ID</summary>
            /// <value>機種小分類の親構成ID</value>
            public int? SmallClassParentId { get; set; }
            /// <summary>Gets or sets 機種中分類番号</summary>
            /// <value>機種中分類番号</value>
            public int? SmallClassParentNumber { get; set; }
            /// <summary>Gets or sets 機種中分類番号</summary>
            /// <value>機種中分類番号</value>
            public int? SmallClassParentNumberBefore { get; set; }
        }

        /// <summary>
        /// ExcelPortマスタ用予備品ロケーション共通データクラス
        /// </summary>
        public class CommonExcelPortMasterPartsList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }
            /// <summary>Gets or sets 地区番号</summary>
            /// <value>地区番号</value>
            public int? DistrictNumber { get; set; }
            /// <summary>Gets or sets 地区ID(構成ID)</summary>
            /// <value>地区ID(構成ID)</value>
            public long? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名</summary>
            /// <value>地区名</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? FactoryNumber { get; set; }
            /// <summary>Gets or sets 工場ID(構成ID)</summary>
            /// <value>工場ID(構成ID)</value>
            public long? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名</summary>
            /// <value>工場名</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets 工場の親構成ID</summary>
            /// <value>工場の親構成ID</value>
            public int? FactoryParentId { get; set; }
            /// <summary>Gets or sets 地区番号</summary>
            /// <value>地区番号</value>
            public int? FactoryParentNumber { get; set; }
            /// <summary>Gets or sets 倉庫ID(構成ID)</summary>
            /// <value>倉庫ID(構成ID)</value>
            public long? WarehouseId { get; set; }
            /// <summary>Gets or sets 倉庫アイテムID</summary>
            /// <value>倉庫アイテムID</value>
            public int? WarehouseItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(倉庫)</summary>
            /// <value>翻訳ID(倉庫)</value>
            public int? WarehouseItemTranslationId { get; set; }
            /// <summary>Gets or sets 倉庫番号</summary>
            /// <value>倉庫番号</value>
            public int? WarehouseNumber { get; set; }
            /// <summary>Gets or sets 倉庫名</summary>
            /// <value>倉庫名</value>
            public string WarehouseName { get; set; }
            /// <summary>Gets or sets 倉庫名(変更前)</summary>
            /// <value>倉庫名(変更前)</value>
            public string WarehouseNameBefore { get; set; }
            /// <summary>Gets or sets 倉庫の親構成ID</summary>
            /// <value>倉庫の親構成ID</value>
            public int? WarehouseParentId { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? WarehouseParentNumber { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? WarehouseParentNumberBefore { get; set; }
            /// <summary>Gets or sets 棚ID(構成ID)</summary>
            /// <value>棚ID(構成ID)</value>
            public long? RackId { get; set; }
            /// <summary>Gets or sets 棚アイテムID</summary>
            /// <value>棚アイテムID</value>
            public int? RackItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(棚)</summary>
            /// <value>翻訳ID(棚)</value>
            public int? RackItemTranslationId { get; set; }
            /// <summary>Gets or sets 棚番号</summary>
            /// <value>棚番号</value>
            public int? RackNumber { get; set; }
            /// <summary>Gets or sets 棚名</summary>
            /// <value>棚名</value>
            public string RackName { get; set; }
            /// <summary>Gets or sets 棚名(変更前)</summary>
            /// <value>棚名(変更前)</value>
            public string RackNameBefore { get; set; }
            /// <summary>Gets or sets 棚の親構成ID</summary>
            /// <value>機種小分類の親構成ID</value>
            public int? RackParentId { get; set; }
            /// <summary>Gets or sets 倉庫番号</summary>
            /// <value>倉庫番号</value>
            public int? RackParentNumber { get; set; }
            /// <summary>Gets or sets 倉庫番号</summary>
            /// <value>倉庫番号</value>
            public int? RackParentNumberBefore { get; set; }
        }

        /// <summary>
        /// ExcelPortマスタ用部門共通データクラス
        /// </summary>
        public class CommonExcelPortMasterDepartmentList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }
            /// <summary>Gets or sets 地区番号</summary>
            /// <value>地区番号</value>
            public int? DistrictNumber { get; set; }
            /// <summary>Gets or sets 地区ID(構成ID)</summary>
            /// <value>地区ID(構成ID)</value>
            public long? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名</summary>
            /// <value>地区名</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? FactoryNumber { get; set; }
            /// <summary>Gets or sets 工場ID(構成ID)</summary>
            /// <value>工場ID(構成ID)</value>
            public long? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名</summary>
            /// <value>工場名</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets 工場の親構成ID</summary>
            /// <value>工場の親構成ID</value>
            public int? FactoryParentId { get; set; }
            /// <summary>Gets or sets 地区番号</summary>
            /// <value>地区番号</value>
            public int? FactoryParentNumber { get; set; }
            /// <summary>Gets or sets 部門ID(構成ID)</summary>
            /// <value>部門ID(構成ID)</value>
            public long? DepartmentId { get; set; }
            /// <summary>Gets or sets 部門アイテムID</summary>
            /// <value>部門アイテムID</value>
            public int? DepartmentItemId { get; set; }
            /// <summary>Gets or sets 翻訳ID(部門)</summary>
            /// <value>翻訳ID(部門)</value>
            public int? DepartmentItemTranslationId { get; set; }
            /// <summary>Gets or sets 部門番号</summary>
            /// <value>部門番号</value>
            public int? DepartmentNumber { get; set; }
            /// <summary>Gets or sets 部門名</summary>
            /// <value>部門名</value>
            public string DepartmentName { get; set; }
            /// <summary>Gets or sets 部門名(変更前)</summary>
            /// <value>部門名(変更前)</value>
            public string DepartmentNameBefore { get; set; }
            /// <summary>Gets or sets 部門の親構成ID</summary>
            /// <value>部門の親構成ID</value>
            public int? DepartmentParentId { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? DepartmentParentNumber { get; set; }
            /// <summary>Gets or sets 工場番号</summary>
            /// <value>工場番号</value>
            public int? DepartmentParentNumberBefore { get; set; }
            /// <summary>Gets or sets 部門コード</summary>
            /// <value>部門コード</value>
            public string DepartmentCode { get; set; }
            /// <summary>Gets or sets 修理部門区分</summary>
            /// <value>修理部門区分</value>
            public string FixDivision { get; set; }
        }

        /// <summary>
        /// ExcelPortマスタ用並び順共通データクラス
        /// </summary>
        public class CommonExcelPortMasterOrderList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            /// <summary>Gets or sets 送信時処理名</summary>
            /// <value>送信時処理名</value>
            public string ProcessName { get; set; }
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int? StructureId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }
            /// <summary>Gets or sets マスタ種類</summary>
            /// <value>マスタ種類</value>
            public string MasterName { get; set; }
            /// <summary>Gets or sets 工場アイテムフラグ</summary>
            /// <value>工場アイテムフラグ</value>
            public string FactoryItemFlg { get; set; }
            /// <summary>Gets or sets アイテム翻訳名称 </summary>
            /// <value>アイテム翻訳名称 </value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 並び順対象工場ID</summary>
            /// <value>並び順対象工場ID</value>
            public int? TargetFactoryId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string FactoryName { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DisplayOrder { get; set; }
        }

        /// <summary>
        /// ExcelPortマスタ用標準アイテム未使用共通データクラス
        /// </summary>
        public class CommonExcelPortMasterItemUnUseList : ComDao.CommonTableItem
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            /// <summary>Gets or sets 送信時処理名</summary>
            /// <value>送信時処理名</value>
            public string ProcessName { get; set; }
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int? StructureId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }
            /// <summary>Gets or sets マスタ種類</summary>
            /// <value>マスタ種類</value>
            public string MasterName { get; set; }
            /// <summary>Gets or sets 未使用アイテム名</summary>
            /// <value>未使用アイテム名</value>
            public string UnuseItemName { get; set; }
            /// <summary>Gets or sets 未使用工場ID</summary>
            /// <value>未使用工場ID</value>
            public string UnuseFactoryId { get; set; }
            /// <summary>Gets or sets 未使用工場名</summary>
            /// <value>未使用工場名</value>
            public string UnuseFactoryName { get; set; }
        }

        /// <summary>
        /// ExcelPortマスタ用出力対象条件データクラス
        /// </summary>
        public class CommonExcelPortMasterCondition
        {
            /// <summary>Gets or sets 行番号</summary>
            /// <value>行番号</value>
            public int? RowNo { get; set; }
            /// <summary>Gets or sets 送信時処理ID</summary>
            /// <value>送信時処理ID</value>
            public long? ProcessId { get; set; }
            /// <summary>Gets or sets メンテナンス対象(構成ID)</summary>
            /// <value>メンテナンス(構成ID)</value>
            public int MaintenanceTarget { get; set; }
            /// <summary>Gets or sets メンテナンス対象(1：マスタアイテム、2：標準アイテム未使用設定、3：マスタ並び順設定)</summary>
            /// <value>メンテナンス(1：マスタアイテム、2：標準アイテム未使用設定、3：マスタ並び順設定)</value>
            public string MaintenanceTargetNo { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int StructureGroupId { get; set; }
            /// <summary>Gets or sets マスタ名称の翻訳ID</summary>
            /// <value>マスタ名称の翻訳ID</value>
            public int MasterTransLationId { get; set; }
            /// <summary>Gets or sets 工場IDリスト</summary>
            /// <value>工場IDリスト</value>
            public List<long> FactoryIdList { get; set; }
            /// <summary>Gets or sets 階層番号リスト</summary>
            /// <value>階層番号リスト</value>
            public List<int> LayerIdList { get; set; }
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

        /// <summary>
        /// ExcelPort マスタアイテム削除処理
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="db">DBクラス</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool deleteExcelPortData(TMQUtil.CommonExcelPortMasterList registInfo, ComDB db)
        {
            // 削除SQL実行(論理削除)
            if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsStructureInfoAddDeleteFlg, ComMaster.SqlName.SubDir, registInfo, db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ExcelPort 翻訳マスタ登録
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="now">現在日時</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DBクラス</param>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="transId">翻訳ID</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool registTranslationExcelPort(TMQUtil.CommonExcelPortMasterList registInfo, DateTime now, string languageId, ComDB db, string userId, out int transId)
        {
            transId = -1;

            // 条件を作成
            TMQUtil.ItemTranslationForMaster userTran = new();
            userTran.LocationStructureId = (int)registInfo.FactoryId;
            userTran.LanguageId = languageId;
            userTran.TranslationText = registInfo.TranslationText;
            userTran.TranslationTextBk = registInfo.TranslationTextBefore;
            userTran.TranslationId = registInfo.TranslationId;

            // ユーザ言語以外の言語情報
            var otherTranList = TMQUtil.SqlExecuteClass.SelectList<TMQUtil.ItemTranslationForMaster>(ComMaster.SqlName.GetLanguageList, ComMaster.SqlName.SubDir, new { LanguageId = languageId }, db);
            otherTranList = otherTranList.Where(x => x.LanguageId != languageId).ToList();
            foreach (TMQUtil.ItemTranslationForMaster result in otherTranList)
            {
                result.StructureGroupId = (int)registInfo.StructureGroupId;
                result.LocationStructureId = (int)registInfo.FactoryId;
            }

            // 新規登録フラグ
            var isNew = false;

            // 送信時処理を判定
            if (registInfo.ProcessId == TMQConst.SendProcessId.Regist)
            {
                // 新規登録
                isNew = isNewTranslation(ref transId);
            }
            else
            {
                // 更新
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
                        transId = (int)userTran.TranslationId;
                    }
                }
                else
                {
                    // 翻訳が変更されている場合
                    // 工場内に同じ翻訳が存在するか翻訳マスタを検索
                    isNew = isNewTranslation(ref transId);
                }
            }

            // 翻訳マスタ登録
            if (isNew)
            {
                // 翻訳マスタに同じ翻訳が存在しない場合、新規登録
                if (!registTranslationDb(ref transId, now))
                {
                    return false;
                }
            }
            else
            {
                // 翻訳マスタに同じ翻訳が存在する場合、翻訳更新
                if (!updateTranslationDb(ref transId))
                {
                    return false;
                }
            }

            return true;

            // 翻訳マスタ検索(翻訳マスタ新規登録の場合True)
            bool isNewTranslation(ref int tranId)
            {
                // ユーザ言語のアイテム翻訳に対して工場内に同じ翻訳が存在するか翻訳マスタを検索
                var registTranIds = TMQUtil.SqlExecuteClass.SelectList<int?>(ComMaster.SqlName.GetMsTranslationInfo, ComMaster.SqlName.SubDir, userTran, db, listUnComment: new List<string> { "AddItem" });
                if (registTranIds == null)
                {
                    return true;
                }
                else
                {
                    // 同じ翻訳が存在する場合、翻訳ID順の先頭を採用する
                    tranId = (int)registTranIds.First();
                    return false;
                }
            }

            // 翻訳マスタ登録
            bool registTranslationDb(ref int tranId, DateTime now)
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

            // 翻訳マスタ更新
            bool updateTranslationDb(ref int tranId)
            {
                // 翻訳IDの翻訳を、画面のアイテム翻訳に更新する

                // ユーザ言語以外のアイテム翻訳情報
                foreach (TMQUtil.ItemTranslationForMaster otherTran in otherTranList)
                {
                    // 登録するデータクラスを作成
                    ComDao.MsTranslationEntity condition = new();

                    // ユーザ言語のアイテム翻訳情報を新規登録
                    condition.LocationStructureId = userTran.LocationStructureId;
                    condition.TranslationId = tranId;
                    condition.LanguageId = otherTran.LanguageId;
                    condition.TranslationText = otherTran.TranslationText;
                    SetCommonDataBaseClass(now, ref condition, userId, userId);

                    // 翻訳マスタ情報取得
                    var transInfo = new ComDao.MsTranslationEntity().GetEntity(otherTran.LocationStructureId, tranId, otherTran.LanguageId, db);
                    if (transInfo == null)
                    {
                        // 翻訳マスタに存在しない場合、新規登録
                        // 登録SQL実行
                        if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsTranslationInfo, ComMaster.SqlName.SubDir, condition, db))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // 画面ではここで他言語の翻訳を更新するがエクセルポートは1言語のみなのでなにもしない
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// ExcelPort アイテムマスタ登録
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="transId">翻訳ID</param>
        /// <param name="now">現在日時</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DBクラス</param>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="itemId">アイテムID</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool registItemExcelPort(TMQUtil.CommonExcelPortMasterList registInfo, int transId, DateTime now, string languageId, ComDB db, string userId, out int itemId)
        {
            itemId = -1;

            // 送信時処理を判定
            if (registInfo.ProcessId == TMQConst.SendProcessId.Regist)
            {
                // 新規登録
                // 登録するデータクラスを作成
                ComDao.MsItemEntity condition = new();
                condition.StructureGroupId = registInfo.StructureGroupId;
                condition.ItemTranslationId = transId;
                SetCommonDataBaseClass(now, ref condition, userId, userId);

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out itemId, ComMaster.SqlName.InsertMsItemInfo, ComMaster.SqlName.SubDir, condition, db))
                {
                    return false;
                }
            }
            else
            {
                // 更新

                // 条件を作成
                TMQUtil.ItemTranslationForMaster userTran = new();
                userTran.LocationStructureId = (int)registInfo.FactoryId;
                userTran.LanguageId = languageId;
                userTran.TranslationText = registInfo.TranslationText;
                userTran.TranslationTextBk = registInfo.TranslationTextBefore;
                userTran.TranslationId = registInfo.TranslationId;

                // アイテムID取得
                itemId = (int)registInfo.ItemId;

                // アイテムマスタ検索
                var itemEntity = new ComDao.MsItemEntity().GetEntity(itemId, db);
                if (itemEntity == null)
                {
                    // アイテムマスタに存在しない場合、エラーを返す
                    return false;
                }

                // 翻訳が変更されているか
                if (itemEntity.ItemTranslationId == transId)
                {
                    // 変更されていない場合、処理終了
                    return true;
                }

                // 登録するデータクラスを作成
                ComDao.MsItemEntity condition = new();
                condition.ItemId = itemId;
                condition.ItemTranslationId = transId;
                SetCommonDataBaseClass(now, ref condition, userId);

                // アイテムマスタ更新
                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsItemInfo, ComMaster.SqlName.SubDir, condition, db))
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// ExcelPort アイテムマスタ拡張登録
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="itemExCnt">拡張項目の件数</param>
        /// <param name="now">現在日時</param>
        /// <param name="db">DBクラス</param>
        /// <param name="userId">ログインユーザID</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool registItemExExcelPort(TMQUtil.CommonExcelPortMasterList registInfo, int itemId, int itemExCnt, DateTime now, ComDB db, string userId)
        {
            // 拡張項目件数分繰り返し
            for (int i = 1; i <= itemExCnt; i++)
            {
                // 登録するデータクラスを作成
                ComDao.MsItemExtensionEntity condition = new();
                condition.ItemId = itemId;
                condition.SequenceNo = i;
                switch (i)
                {
                    case 1:
                        condition.ExtensionData = registInfo.ExData1;
                        break;
                    case 2:
                        condition.ExtensionData = registInfo.ExData2;
                        break;
                    case 3:
                        condition.ExtensionData = registInfo.ExData3;
                        break;
                    case 4:
                        condition.ExtensionData = registInfo.ExData4;
                        break;
                    case 5:
                        condition.ExtensionData = registInfo.ExData5;
                        break;
                    case 6:
                        condition.ExtensionData = registInfo.ExData6;
                        break;
                    case 7:
                        condition.ExtensionData = registInfo.ExData7;
                        break;
                    case 8:
                        condition.ExtensionData = registInfo.ExData8;
                        break;
                    case 9:
                        condition.ExtensionData = registInfo.ExData9;
                        break;
                    case 10:
                        condition.ExtensionData = registInfo.ExData10;
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
                    SetCommonDataBaseClass(now, ref condition, userId, userId);

                    // 登録SQL実行
                    if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsItemExtensionInfo, ComMaster.SqlName.SubDir, condition, db))
                    {
                        return false;
                    }
                }
                else
                {
                    // アイテムマスタ拡張に存在する場合、拡張データを比較
                    if (itemExEntity.ExtensionData == condition.ExtensionData)
                    {
                        // 変更されていない場合、更新対象外
                        continue;
                    }
                    // アイテムマスタ拡張更新
                    // 実行条件の共通項目設定
                    SetCommonDataBaseClass(now, ref condition, userId);

                    // 登録SQL実行
                    if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsItemExtensionInfo, ComMaster.SqlName.SubDir, condition, db))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///  ExcelPort 構成マスタ登録
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="now">現在日時</param>
        /// <param name="db">DBクラス</param>
        /// <param name="userId">ログインユーザID</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool registStructureExcelPort(TMQUtil.CommonExcelPortMasterList registInfo, int itemId, DateTime now, ComDB db, string userId)
        {
            // 登録するデータクラスを作成
            ComDao.MsStructureEntity condition = new();
            condition.FactoryId = registInfo.FactoryId;
            condition.StructureGroupId = registInfo.StructureGroupId;
            condition.ParentStructureId = null;
            condition.StructureLayerNo = null;
            condition.StructureItemId = itemId;
            condition.DeleteFlg = false;
            SetCommonDataBaseClass(now, ref condition, userId, userId);

            // 送信時処理を判定
            if (registInfo.ProcessId == TMQConst.SendProcessId.Regist)
            {
                // 新規登録

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out int structureId, ComMaster.SqlName.InsertMsStructureInfo, ComMaster.SqlName.SubDir, condition, db))
                {
                    return false;
                }
            }
            else
            {
                // 更新
                // 構成マスタ検索
                var stEntity = new ComDao.MsStructureEntity().GetEntity(registInfo.StructureId, db);
                if (stEntity == null)
                {
                    // 構成マスタに存在しない場合、エラーを返す
                    return false;
                }

                // 削除フラグを比較
                if (stEntity.DeleteFlg == condition.DeleteFlg)
                {
                    // 変更されていない場合、更新対象外
                    return true;
                }

                // 構成マスタ更新
                // 構成ID設定
                condition.StructureId = registInfo.StructureId;

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsStructureInfo, ComMaster.SqlName.SubDir, condition, db))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// ExcelPort 並び順設定登録
        /// </summary>
        /// <param name="structureGroupId">構成グループID</param>
        /// <param name="resultOrderList">並び順登録情報</param>
        /// <param name="now">現在日時</param>
        /// <param name="db">DBクラス</param>
        /// <param name="userId">ログインユーザID</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool registItemOrderExcelPort(int structureGroupId, List<TMQUtil.CommonExcelPortMasterOrderList> resultOrderList, DateTime now, ComDB db, string userId)
        {
            // 工場別アイテム表示順マスタ削除
            if (!deleteItem())
            {
                return false;
            }

            // 工場別アイテム表示順マスタ登録
            foreach (TMQUtil.CommonExcelPortMasterOrderList registInfo in resultOrderList)
            {
                // テーブル共通項目の設定
                registInfo.InsertUserId = int.Parse(userId);
                registInfo.UpdateUserId = int.Parse(userId);
                registInfo.InsertDatetime = now;
                registInfo.UpdateDatetime = now;
                registInfo.FactoryId = registInfo.TargetFactoryId;

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsStructureOrder, ComMaster.SqlName.SubDir, registInfo, db))
                {
                    return false;
                }
            }

            return true;

            // 並び順データ削除
            bool deleteItem()
            {
                // 削除対象がない場合は終了
                if (resultOrderList.Count == 0)
                {
                    return true;
                }

                // SQL取得
                if (!TMQUtil.GetFixedSqlStatement(ComMaster.SqlName.SubDir, ComMaster.SqlName.DeleteMsStructureOrder, out string baseSql))
                {
                    return false;
                }

                // SQL実行
                int result = db.Regist(baseSql, new { @StructureGroupId = structureGroupId, @FactoryId = resultOrderList[0].TargetFactoryId });
                if (result < 0)
                {
                    // 削除エラー
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// ExcelPort 標準アイテム未使用設定登録
        /// </summary>
        /// <param name="structureGroupId">構成グループID</param>
        /// <param name="resultUnuseList">標準アイテム未使用登録情報</param>
        /// <param name="now">現在日時</param>
        /// <param name="db">DBクラス</param>
        /// <param name="userId">ログインユーザID</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool registUnuseItemExcelPort(int structureGroupId, List<TMQUtil.CommonExcelPortMasterItemUnUseList> resultUnuseList, DateTime now, ComDB db, string userId)
        {
            // 既存のデータを削除
            if (!deleteItem())
            {
                return false;
            }

            // 登録処理
            foreach (TMQUtil.CommonExcelPortMasterItemUnUseList result in resultUnuseList)
            {
                // 未使用にする工場が選択されていない場合はスキップ
                if (string.IsNullOrEmpty(result.UnuseFactoryId))
                {
                    continue;
                }

                // 未使用にする工場IDを分割
                var factoryIdList = result.UnuseFactoryId.Split("|");

                // 工場ID毎に登録する
                foreach (string factoryId in factoryIdList)
                {
                    // 登録情報を作成
                    ComDao.MsStructureUnusedEntity registInfo = new();
                    registInfo.StructureId = (int)result.StructureId;
                    registInfo.FactoryId = int.Parse(factoryId);
                    registInfo.StructureGroupId = structureGroupId;
                    registInfo.InsertDatetime = now;
                    registInfo.InsertUserId = int.Parse(userId);
                    registInfo.UpdateDatetime = now;
                    registInfo.UpdateUserId = int.Parse(userId);

                    // 登録SQL実行
                    if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsStructureUnused, ComMaster.SqlName.SubDir, registInfo, db))
                    {
                        return false;
                    }
                }
            }

            return true;

            // 並び順データ削除
            bool deleteItem()
            {
                // SQL取得
                if (!TMQUtil.GetFixedSqlStatement(ComMaster.SqlName.ExcelPortDir, ComMaster.SqlName.DeleteExcelPortItemUnUseList, out string baseSql))
                {
                    return false;
                }

                // SQL実行
                int result = db.Regist(baseSql, new { @StructureGroupId = structureGroupId });
                if (result < 0)
                {
                    // 削除エラー
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// ExcelPort 翻訳マスタ登録(階層系)
        /// </summary>
        /// <param name="structureGroupId">構成グループID</param>
        /// <param name="now">現在日時</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DBクラス</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="translationText">翻訳名</param>
        /// <param name="translationTextBefore">変更前翻訳名</param>
        /// <param name="translationId">翻訳ID</param>
        /// <param name="factoryId">工場ID</param>
        /// <param name="keyId">構成ID</param>
        /// <param name="structureLayerNo">階層番号</param>
        /// <param name="parentStructureId">親構成ID</param>
        /// <param name="transId">翻訳ID</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool registTranslationStructureExcelPort(int structureGroupId,
                                                               DateTime now,
                                                               string languageId,
                                                               ComDB db,
                                                               string userId,
                                                               string translationText,
                                                               string translationTextBefore,
                                                               int? translationId,
                                                               int factoryId,
                                                               long? keyId,
                                                               int structureLayerNo,
                                                               int? parentStructureId,
                                                               out int transId,
                                                               bool isNewRegist)
        {
            transId = -1;

            int locationStructureId = factoryId;
            if (structureGroupId == (int)TMQConst.MsStructure.GroupId.SpareLocation)
            {
                locationStructureId = TMQConst.CommonFactoryId;
            }

            // 条件を作成
            TMQUtil.ItemTranslationForMaster userTran = new();
            userTran.LocationStructureId = locationStructureId;
            userTran.LanguageId = languageId;
            userTran.TranslationText = translationText;
            userTran.TranslationTextBk = translationTextBefore;
            userTran.TranslationId = translationId;

            // ユーザ言語以外の言語情報
            var transList = TMQUtil.SqlExecuteClass.SelectList<TMQUtil.ItemTranslationForMaster>(ComMaster.SqlName.GetLanguageList, ComMaster.SqlName.SubDir, new { LanguageId = languageId }, db);
            var otherTranList = transList.Where(x => x.LanguageId != languageId).ToList();
            foreach (TMQUtil.ItemTranslationForMaster result in otherTranList)
            {
                result.StructureGroupId = structureGroupId;
                result.LocationStructureId = locationStructureId;
            }

            var userTran2 = transList.Where(x => x.LanguageId == languageId).FirstOrDefault();
            userTran2.StructureGroupId = structureGroupId;
            userTran2.LocationStructureId = locationStructureId;
            userTran2.FactoryId = factoryId;
            userTran2.TranslationId = translationId;
            userTran2.TranslationText = translationText;
            userTran2.TranslationTextBk = translationTextBefore;
            userTran2.LanguageId = languageId;


            // 新規登録フラグ
            var isNew = false;

            // 構成IDがnullの場合は新規登録
            if (keyId == null || isNewRegist)
            {
                if (structureGroupId == (int)TMQConst.MsStructure.GroupId.Department)
                {
                    isNew = isNewTranslation(ref transId);
                }
                else
                {
                    userTran2.FactoryId = factoryId;
                    userTran2.StructureLayerNo = structureLayerNo;
                    userTran2.ParentStructureId = parentStructureId;

                    // 工場内に同じ翻訳が存在するか翻訳マスタを検索
                    isNew = isNewTranslationByFactory(2, ref transId);
                }

            }
            else
            {
                // 更新
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
                        transId = (int)userTran.TranslationId;
                    }
                }
                else
                {
                    // 翻訳が変更されている場合
                    // 工場内に同じ翻訳が存在するか翻訳マスタを検索
                    isNew = isNewTranslation(ref transId);
                }
            }

            // 翻訳マスタ登録
            if (isNew)
            {
                // 翻訳マスタに同じ翻訳が存在しない場合、新規登録
                if (!registTranslationDb(ref transId, now))
                {
                    return false;
                }
            }
            else
            {
                // 翻訳マスタに同じ翻訳が存在する場合、翻訳更新
                if (!updateTranslationDb(ref transId))
                {
                    return false;
                }
            }

            return true;

            // 翻訳マスタ検索(翻訳マスタ新規登録の場合True)
            bool isNewTranslation(ref int tranId)
            {
                // ユーザ言語のアイテム翻訳に対して工場内に同じ翻訳が存在するか翻訳マスタを検索
                var registTranIds = TMQUtil.SqlExecuteClass.SelectList<int?>(ComMaster.SqlName.GetMsTranslationInfo, ComMaster.SqlName.SubDir, userTran, db, listUnComment: new List<string> { "AddItem" });
                if (registTranIds == null)
                {
                    return true;
                }
                else
                {
                    // 同じ翻訳が存在する場合、翻訳ID順の先頭を採用する
                    tranId = (int)registTranIds.First();
                    return false;
                }
            }

            // 翻訳マスタ検索(工場毎)
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
                }

                return false;
            }

            // 翻訳マスタ登録
            bool registTranslationDb(ref int tranId, DateTime now)
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

            // 翻訳マスタ更新
            bool updateTranslationDb(ref int tranId)
            {
                // 翻訳IDの翻訳を、画面のアイテム翻訳に更新する

                // ユーザ言語以外のアイテム翻訳情報
                foreach (TMQUtil.ItemTranslationForMaster otherTran in otherTranList)
                {
                    // 登録するデータクラスを作成
                    ComDao.MsTranslationEntity condition = new();

                    // ユーザ言語のアイテム翻訳情報を新規登録
                    condition.LocationStructureId = userTran.LocationStructureId;
                    condition.TranslationId = tranId;
                    condition.LanguageId = otherTran.LanguageId;
                    condition.TranslationText = otherTran.TranslationText;
                    SetCommonDataBaseClass(now, ref condition, userId, userId);

                    // 翻訳マスタ情報取得
                    var transInfo = new ComDao.MsTranslationEntity().GetEntity(otherTran.LocationStructureId, tranId, otherTran.LanguageId, db);
                    if (transInfo == null)
                    {
                        // 翻訳マスタに存在しない場合、新規登録
                        // 登録SQL実行
                        if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsTranslationInfo, ComMaster.SqlName.SubDir, condition, db))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // 画面ではここで他言語の翻訳を更新するがエクセルポートは1言語のみなのでなにもしない
                    }
                }

                return true;
            }
        }


        /// <summary>
        /// ExcelPort アイテムマスタ登録(階層系)
        /// </summary>
        /// <param name="keyId">構成ID</param>
        /// <param name="structureGroupId">構成グループID</param>
        /// <param name="factoryId">工場ID</param>
        /// <param name="translationText">翻訳名</param>
        /// <param name="translationTextBefore">変更前翻訳名</param>
        /// <param name="translationIdBefore">翻訳ID</param>
        /// <param name="registItemId">アイテムID</param>
        /// <param name="transId">翻訳ID</param>
        /// <param name="now">現在日時</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DBクラス</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="itemId">アイテムID</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool registItemStructureExcelPort(long? keyId,
                                               int structureGroupId,
                                               int factoryId,
                                               string translationText,
                                               string translationTextBefore,
                                               int? translationIdBefore,
                                               int? registItemId,
                                               int transId,
                                               DateTime now,
                                               string languageId,
                                               ComDB db,
                                               string userId,
                                               out int itemId,
                                               bool isNewRegist)
        {
            itemId = -1;

            // 構成IDがnullの場合は新規登録
            if (keyId == null || isNewRegist)
            {
                // 新規登録
                // 登録するデータクラスを作成
                ComDao.MsItemEntity condition = new();
                condition.StructureGroupId = structureGroupId;
                condition.ItemTranslationId = transId;
                SetCommonDataBaseClass(now, ref condition, userId, userId);

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out itemId, ComMaster.SqlName.InsertMsItemInfo, ComMaster.SqlName.SubDir, condition, db))
                {
                    return false;
                }
            }
            else
            {
                // 更新

                // 条件を作成
                TMQUtil.ItemTranslationForMaster userTran = new();
                userTran.LocationStructureId = factoryId;
                userTran.LanguageId = languageId;
                userTran.TranslationText = translationText;
                userTran.TranslationTextBk = translationTextBefore;
                userTran.TranslationId = translationIdBefore;

                // アイテムID取得
                itemId = (int)registItemId;

                // アイテムマスタ検索
                var itemEntity = new ComDao.MsItemEntity().GetEntity(itemId, db);
                if (itemEntity == null)
                {
                    // アイテムマスタに存在しない場合、エラーを返す
                    return false;
                }

                // 翻訳が変更されているか
                if (itemEntity.ItemTranslationId == transId)
                {
                    // 変更されていない場合、処理終了
                    return true;
                }

                // 登録するデータクラスを作成
                ComDao.MsItemEntity condition = new();
                condition.ItemId = itemId;
                condition.ItemTranslationId = transId;
                SetCommonDataBaseClass(now, ref condition, userId);

                // アイテムマスタ更新
                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsItemInfo, ComMaster.SqlName.SubDir, condition, db))
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// ExcelPort 構成マスタ登録(階層系)
        /// </summary>
        /// <param name="structureGroupId">構成グループID</param>
        /// <param name="keyId">構成ID</param>
        /// <param name="factoryId">工場ID</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="parentStructureId">親構成ID</param>
        /// <param name="structureLayerNo">階層番号</param>
        /// <param name="now">現在日時</param>
        /// <param name="db">DBクラス</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool registStructureExcelPort(int structureGroupId,
                                                    long? keyId,
                                                    int factoryId,
                                                    int itemId,
                                                    int? parentStructureId,
                                                    int? structureLayerNo,
                                                    DateTime now,
                                                    ComDB db,
                                                    string userId,
                                                    bool isNewRegist,
                                                    out int structureId)
        {
            structureId = -1;

            // 登録するデータクラスを作成
            ComDao.MsStructureEntity condition = new();
            condition.FactoryId = factoryId;
            condition.StructureGroupId = structureGroupId;
            condition.ParentStructureId = parentStructureId;
            condition.StructureLayerNo = structureLayerNo;
            condition.StructureItemId = itemId;
            condition.DeleteFlg = false;
            SetCommonDataBaseClass(now, ref condition, userId, userId);

            // 構成IDがnullの場合は新規登録
            if (keyId == null || isNewRegist)
            {
                // 新規登録

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out structureId, ComMaster.SqlName.InsertMsStructureInfo, ComMaster.SqlName.SubDir, condition, db))
                {
                    return false;
                }
            }
            else
            {
                // 更新
                structureId = Convert.ToInt32(keyId);

                // 構成マスタ検索
                var stEntity = new ComDao.MsStructureEntity().GetEntity(Convert.ToInt32(keyId), db);
                if (stEntity == null)
                {
                    // 構成マスタに存在しない場合、エラーを返す
                    return false;
                }

                // 削除フラグを比較
                if (stEntity.DeleteFlg == condition.DeleteFlg)
                {
                    // 変更されていない場合、更新対象外
                    return true;
                }

                // 構成マスタ更新
                // 構成ID設定
                condition.StructureId = Convert.ToInt32(keyId);

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsStructureInfo, ComMaster.SqlName.SubDir, condition, db))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// ExcelPort アイテムマスタ拡張登録(階層系)
        /// </summary>
        /// <param name="exData">登録情報</param>
        /// <param name="sequenceNo">連番</param>
        /// <param name="itemId">アイテムID</param>
        /// <param name="now">現在日時</param>
        /// <param name="db">DBクラス</param>
        /// <param name="userId">ログインユーザID</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool registStructureItemExExcelPort(string exData, int sequenceNo, int itemId, DateTime now, ComDB db, string userId)
        {

            // 登録するデータクラスを作成
            ComDao.MsItemExtensionEntity condition = new();
            condition.ItemId = itemId;
            condition.SequenceNo = sequenceNo;
            condition.ExtensionData = exData;

            // アイテムマスタ拡張検索
            var itemExEntity = new ComDao.MsItemExtensionEntity().GetEntity(itemId, sequenceNo, db);
            if (itemExEntity == null)
            {
                // アイテムマスタ拡張に存在しない場合、新規登録

                // 実行条件の共通項目設定
                SetCommonDataBaseClass(now, ref condition, userId, userId);

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.InsertMsItemExtensionInfo, ComMaster.SqlName.SubDir, condition, db))
                {
                    return false;
                }
            }
            else
            {
                // アイテムマスタ拡張に存在する場合、拡張データを比較
                if (itemExEntity.ExtensionData == condition.ExtensionData)
                {
                    // 変更されていない場合、更新対象外
                    return true;
                }
                // アイテムマスタ拡張更新
                // 実行条件の共通項目設定
                SetCommonDataBaseClass(now, ref condition, userId);

                // 登録SQL実行
                if (!TMQUtil.SqlExecuteClass.Regist(ComMaster.SqlName.UpdateMsItemExtensionInfo, ComMaster.SqlName.SubDir, condition, db))
                {
                    return false;
                }
            }


            return true;
        }

        /// <summary>
        /// ExcelPort 翻訳を取得する一時テーブル作成
        /// </summary>
        /// <param name="structureGroupId"></param>
        /// <param name="db"></param>
        /// <param name="languageId"></param>
        public static void createTempTblExcelPort(int structureGroupId, ComDB db, string languageId)
        {
            // 翻訳の一時テーブルを作成
            TMQUtil.ListPerformanceUtil listPf = new(db, languageId);

            // 翻訳する構成グループのリスト
            var structuregroupList = new List<GroupId>
            {
                GroupId.Location,
                (GroupId)Enum.ToObject(typeof(GroupId), structureGroupId)
            };
            listPf.GetCreateTranslation(); // テーブル作成
            listPf.GetInsertTranslationAll(structuregroupList, true); // 各グループ
            listPf.RegistTempTable(); // 登録
        }

        /// <summary>
        /// ExcelPort 階層系ではないマスタの工場に関する入力チェック
        /// </summary>
        /// <param name="result">Excelにて入力されたレコード</param>
        /// <param name="msg">エラーメッセージ</param>
        /// <returns>エラーの場合はFalse</returns>
        public static bool commonFactoryCheckExcelPort(TMQUtil.CommonExcelPortMasterList result, out string[] msg)
        {
            msg = null;

            if (result.ProcessId == TMQConst.SendProcessId.Regist)
            {
                //新規登録時
                // 工場IDが標準工場の場合
                if (result.FactoryId == TMQConst.CommonFactoryId)
                {
                    // 標準アイテムは登録できません。
                    msg = new string[] { ComRes.ID.ID141270004 };
                    return false;
                }
            }
            else if (result.ProcessId == TMQConst.SendProcessId.Update)
            {
                // 更新時
                // ①初期表示時、登録時の工場IDが0(標準アイテム)の場合
                // ②工場アイテムから標準アイテムに変更されている場合
                if ((result.FactoryIdBefore == TMQConst.CommonFactoryId || result.FactoryId == TMQConst.CommonFactoryId) ||
                    (result.FactoryIdBefore != TMQConst.CommonFactoryId && result.FactoryId == TMQConst.CommonFactoryId))
                {
                    // 標準アイテムは登録できません。
                    msg = new string[] { ComRes.ID.ID141270004 };
                    return false;
                }
            }

            // 工場アイテムが別の工場アイテムに変更されている場合
            if (result.FactoryIdBefore != TMQConst.CommonFactoryId &&
                result.FactoryId != TMQConst.CommonFactoryId &&
                result.FactoryIdBefore != result.FactoryId)
            {
                // 別工場のアイテムに変更することはできません。
                msg = new string[] { ComRes.ID.ID141290008 };
                return false;
            }

            return true;
        }


        /// <summary>
        /// ExcelPort 階層系の文字数チェック
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool commonTextByteCheckExcelPort(string text, out int maxlength)
        {
            maxlength = ItemTranslasionMaxLength;

            // 文字数チェック
            if (text.Length > maxlength)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}