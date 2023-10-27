namespace CommonTMQUtil
{
    /// <summary>
    /// TMQ用定数クラス
    /// </summary>
    public class CommonTMQConstants
    {
        /// <summary>共通の工場ID </summary>
        public const int CommonFactoryId = 0;

        /// <summary>ファイル情報を取得するプロシージャで取得した情報のセパレータ</summary>
        public class FileInfoSplit
        {
            /// <summary>ファイルとファイルの区切り文字 </summary>
            public const string File = "||";
            /// <summary>ファイル名とディレクトリの区切り文字 </summary>
            public const string Path = "|";
            /// <summary>get_file_download_info_rowで使用　キーの区切り文字 </summary>
            public const string Key = "_";
        }

        /// <summary>HTML公開フォルダ</summary>
        public const string HtmlRootFolder = "wwwroot";

        /// <summary>
        /// 構成マスタの情報
        /// </summary>
        public class MsStructure
        {
            /// <summary>
            /// 構成階層番号
            /// </summary>
            public static class StructureLayerNo
            {
                /// <summary>
                /// 場所階層の階層番号
                /// </summary>
                /// <remarks>intの値が欲しい場合はキャストしてください</remarks>
                public enum Location
                {
                    // 地区
                    District = 0,
                    // 工場
                    Factory = 1,
                    // プラント
                    Plant = 2,
                    // 系列
                    Series = 3,
                    // 工程
                    Stroke = 4,
                    // 設備
                    Facility = 5
                }
                /// <summary>
                /// 職種階層の階層番号
                /// </summary>
                public enum Job
                {
                    // 職種
                    Job = 0,
                    // 機種大分類
                    LargeClassfication = 1,
                    // 機種中分類
                    MiddleClassfication = 2,
                    // 機種小分類
                    SmallClassfication = 3
                }
                /// <summary>
                /// 原因性格階層の階層番号
                /// </summary>
                public enum FailureCause
                {
                    // 原因性格1
                    FailureCausePersonality1Structure = 0,
                    // 原因性格2
                    FailureCausePersonality2Structure = 1,
                }
                /// <summary>
                /// 予備品の階層番号
                /// </summary>
                public enum SpareLocation
                {
                    // 地区と工場は場所階層より取得するので、階層には含まれない
                    // この定義を参照している処理で使用しているので、定義している
                    // 地区
                    District = 0,
                    // 工場
                    Factory = 1,
                    // 倉庫
                    Warehouse = 2,
                    // 棚
                    Rack = 3
                }
                /// <summary>
                /// 場所階層(変更管理用)の階層番号
                /// </summary>
                /// <remarks>intの値が欲しい場合はキャストしてください</remarks>
                public enum OldLocation
                {
                    // 地区
                    OldDistrict = 0,
                    // 工場
                    OldFactory = 1,
                    // プラント
                    OldPlant = 2,
                    // 系列
                    OldSeries = 3,
                    // 工程
                    OldStroke = 4,
                    // 設備
                    OldFacility = 5
                }
                /// <summary>
                /// 職種階層(変更管理用)の階層番号
                /// </summary>
                public enum OldJob
                {
                    // 職種
                    OldJob = 0,
                    // 機種大分類
                    OldLargeClassfication = 1,
                    // 機種中分類
                    OldMiddleClassfication = 2,
                    // 機種小分類
                    OldSmallClassfication = 3
                }

            }

            /// <summary>
            /// 構成グループID
            /// </summary>
            public enum GroupId
            {
                // 機能場所階層
                Location = 1000,
                // 職種機種階層
                Job = 1010,
                // 原因性格階層
                FailureCausePersonality = 1020,
                // 保全方式
                Conservation = 1030,
                // 予備品機能場所階層
                SpareLocation = 1040,
                // 処置対策
                TreatmentMeasure = 1050,
                // 予算性格区分
                BudgetPersonality = 1060,
                // 緊急度
                Urgency = 1070,
                // 発見方法
                DiscoveryMethods = 1080,
                // 変更管理
                ChangeManagement = 1090,
                // 環境安全管理
                EnvSafetyManagement = 1100,
                // 呼出
                Call = 1110,
                // 系停止
                StopSystem = 1130,
                // 実績結果
                ActualResult = 1140,
                // メーカー
                Manufacturer = 1150,
                // 適用法規
                ApplicableLaws = 1160,
                // 機器レベル
                MachineLevel = 1170,
                // 部位マスタ
                SiteMaster = 1180,
                // 重要度
                Importance = 1200,
                // 使用区分
                UseSegment = 1210,
                // 保全項目(点検内容)
                InspectionDetails = 1220,
                // 保全区分
                MaintainanceDivision = 1230,
                // 点検種別
                MaintainanceKind = 1240,
                // 作業項目
                WorkItem = 1280,
                // 目的区分
                Purpose = 1290,
                // 予算管理区分
                BudgetManagement = 1300,
                // 処置区分
                Treatment = 1310,
                // 設備区分
                Facility = 1320,
                // 時期
                Season = 1330,
                // 仕様項目選択肢
                SpecSelectItem = 1340,
                // 仕様単位種別
                SpecUnitType = 1350,
                // 仕様単位
                SpecUnit = 1360,
                // 仕様項目入力形式
                SpecType = 1370,
                // 修繕費分類
                RepairCostClass = 1380,
                // 突発区分
                Sudden = 1400,
                // 作業区分
                WorkClass = 1410,
                // 依頼部課係
                RequestDepartmentClerk = 1420,
                // 工事区分
                ConstructionDivision = 1430,
                // 自・他責
                Responsibility = 1440,
                // 施工会社
                Company = 1450,
                // 故障性格分類
                FailurePersonalityClass = 1470,
                // 生産への影響
                EffectProduction = 1480,
                // 品質への影響
                EffectQuality = 1490,
                // 故障現象
                Phenomenon = 1510,
                // 故障性格要因
                FailurePersonalityFactor = 1520,
                //保全活動区分
                ActivityDivision = 1530,
                //依頼番号採番パターン
                RequestNumberingPattern = 1540,
                //作業／故障区分
                WorkFailureDivision = 1550,
                //故障分析
                FailureAnalysis = 1560,
                //完了/応急
                TreatmentStatus = 1580,
                //要/否
                NecessityMeasure = 1590,
                //仕入先
                Vender = 1720,
                //数量管理単位
                Unit = 1730,
                //金額管理単位
                Currency = 1740,
                //部門
                Department = 1760,
                //勘定科目
                Account = 1770,
                // スケジュール作成上限
                MakeScheduleYear = 1800,
                // 故障原因
                FailureCause = 1810,
                // 対策分類Ⅰ
                MeasureClass1 = 1820,
                // 対策分類Ⅱ
                MeasureClass2 = 1830,
                // 点検種別毎管理(機器別管理基準)工場
                MaintainanceKindManageFactory = 1840,
                //MQ分類
                MqClass = 1850,
                // ステータスリスト
                StatusList = 1860,
                // スケジュール表示単位
                ScheduleDisp = 1870,
                // スケジュール基準
                ScheduleType = 1890,
                // 進捗状況
                Progress = 1900,
                // 工場毎年度期間
                YearSpan = 1910,
                // 保全部課係
                MaintenanceDepartmentClerk = 1930,
                // 新旧区分
                OldNewDivition = 1940,
                // 受払区分
                InoutDivision = 1950,
                // 作業区分
                WorkDivision = 1960,
                // 予備品使用区分
                PartsUseSegment = 1970,
                // 出庫区分
                IssueDivision = 1980,
                // 棚卸作成区分
                Creation = 1990,
                // 棚卸状況
                InventoryStatus = 2000,
                // 棚卸時間
                InventoryTime = 2010,
                // 保全実績評価集計期首月
                BeginningMonth = 2020,
                // 丸め処理区分
                RoundDivision = 2050,
                // 機種別仕様項目数値書式
                SpecNumDecimalPlaces = 2060,
                // 変更管理 申請状況
                ApplicationStatus = 2090,
                // 変更管理 申請区分
                ApplicationDivision = 2100,
                // ExcelPort送信時処理区分
                ProcessDivision = 2120,
                // ExcelPortチェックボックス項目用区分
                CheckBoxItemDivision = 2130,
                // 変更管理帳票出力対象項目定義
                OutputItem = 2150,
                // ExcelPort保全活動完了区分
                CompletionDivision = 2170,
                // 権限
                Authority = 9040,
                // テンポラリフォルダパス
                TempFolderPath = 9200,
                // ExcelPort 出力可能最大行数
                ExcelPortMaxCount = 9220,
                // Excel 出力可能最大行数
                ExcelMaxCount = 9320
            }

            /// <summary>
            /// 構成IDより取得した拡張項目の値
            /// </summary>
            /// <remarks>構成IDの値は指定不可、拡張項目の値を使用する</remarks>
            public static class StructureId
            {
                // 値を羅列すると可読性が落ちるので、利用する単位(構成グループID？)で分けてください

                /// <summary>
                /// 計画内容
                /// </summary>
                public enum SchedulePlanContent
                {
                    /// <summary>機器</summary>
                    Equipment = 1,
                    /// <summary>部位</summary>
                    InspectionSite = 2,
                    /// <summary>保全項目</summary>
                    Maintainance = 3
                }

                /// <summary>
                /// スケジュールのマークの種類
                /// </summary>
                /// <remarks>なしは処理のための定義で、実際は存在しない</remarks>
                public enum ScheduleStatus
                {
                    // 分かりやすくするためにコメントにマークを書いていますが、設定により変更し得るので、注意
                    // 値はアイテム拡張項目の値で、表示優先順位とは別の値ですが、定義は初期仕様の表示優先順位と合わせています

                    /// <summary>保全履歴完了、●</summary>
                    Complete = 5,
                    /// <summary>上位ランクが履歴完了済み、▲</summary>
                    UpperComplete = 3,
                    /// <summary>計画作成済み、◎</summary>
                    Created = 4,
                    /// <summary>スケジュール済み、○</summary>
                    NoCreate = 1,
                    /// <summary>上位ランクがスケジュール済み、△</summary>
                    UpperScheduled = 2,
                    /// <summary>なし、非表示</summary>
                    NoSchedule = -1
                }

                /// <summary>
                /// スケジュールのマークの種類（帳票用）
                /// </summary>
                /// <remarks>なしは処理のための定義で、実際は存在しない</remarks>
                public static class ScheduleStatusText
                {
                    // 分かりやすくするためにコメントにマークを書いていますが、設定により変更し得るので、注意
                    // 値はアイテム拡張項目の値で、表示優先順位とは別の値ですが、定義は初期仕様の表示優先順位と合わせています

                    /// <summary>保全履歴完了、●</summary>
                    public const string Complete = "●";
                    /// <summary>上位ランクが履歴完了済み、▲</summary>
                    public const string UpperComplete = "▲";
                    /// <summary>計画作成済み、◎</summary>
                    public const string Created = "◎";
                    /// <summary>スケジュール済み、○</summary>
                    public const string NoCreate = "○";
                    /// <summary>上位ランクがスケジュール済み、△</summary>
                    public const string UpperScheduled = "△";
                    /// <summary>なし、非表示</summary>
                    public const string NoSchedule = "";
                }

                /// <summary>
                /// 表示単位
                /// </summary>
                public enum ScheduleDisplayUnit
                {
                    // 年度
                    Year = 2,
                    // 月度
                    Month = 1
                }

                /// <summary>
                /// 保全履歴個別工場フラグ
                /// </summary>
                public enum MaintenanceHistoryFlg
                {
                    // 一般工場
                    General = 0,
                    // 個別工場
                    Individual = 1
                }

                /// <summary>
                /// 保全実績集計職種コード
                /// </summary>
                public enum JobCode
                {
                    /// <summary>機械</summary>
                    Machine = 10,
                    /// <summary>電気</summary>
                    Electricity = 20,
                    /// <summary>計装</summary>
                    Instrumentation = 30
                }

                /// <summary>
                /// 仕様項目入力形式(1370)
                /// </summary>
                public enum SpecType
                {
                    // テキスト
                    Text = 1,
                    // 数値
                    Number = 2,
                    // 数値(範囲)
                    NumberRange = 3,
                    // 選択
                    Select = 4
                }

                /// <summary>
                /// 受払区分(1950)
                /// </summary>
                public enum InoutDivision
                {
                    // 受入
                    In = 1,
                    // 払出
                    Out = 2
                }

                /// <summary>
                /// 作業区分(1960)
                /// </summary>
                public enum WorkDivision
                {
                    // 繰越
                    Carry = 0,
                    // 入庫
                    In = 1,
                    // 出庫
                    Out = 2,
                    // 棚番移庫
                    MoveStock = 3,
                    // 部門移庫
                    MoveDepartment = 4,
                    // 棚卸入庫
                    InventoryIn = 5,
                    // 棚卸出庫
                    InventoryOut = 6
                }

                /// <summary>
                /// 出庫区分(1980)
                /// </summary>
                public enum IssueDivision
                {
                    // 通常
                    Generally = 1,
                    // 破棄
                    Disposal = 2
                }

                /// <summary>
                /// 棚卸作成区分(1990)
                /// </summary>
                public enum CreationDivision
                {
                    //準備リスト
                    Preparation = 1,
                    //画面入力
                    Input = 2
                }

                /// <summary>
                /// 権限レベル(9040)
                /// </summary>
                public enum AuthLevel
                {
                    // ゲストユーザー
                    Guest = 10,
                    // 一般ユーザー
                    General = 20,
                    // 特権ユーザー
                    Privilege = 30,
                    // システム管理者
                    SystemAdministrator = 99
                }

                /// <summary>
                /// 変更管理 申請状況(2090)
                /// </summary>
                public enum ApplicationStatus
                {
                    /// <summary>
                    /// 申請データ作成中
                    /// </summary>
                    Making = 10,
                    /// <summary>
                    /// 承認依頼中
                    /// </summary>
                    Request = 20,
                    /// <summary>
                    /// 差戻中
                    /// </summary>
                    Return = 30,
                    /// <summary>
                    /// 承認済
                    /// </summary>
                    Approved = 40,
                    /// <summary>
                    /// 申請なし
                    /// </summary>
                    None = 90
                }

                /// <summary>
                /// 変更管理 申請区分(2100)
                /// </summary>
                public enum ApplicationDivision
                {
                    /// <summary>
                    /// 新規登録申請
                    /// </summary>
                    New = 10,
                    /// <summary>
                    /// 変更申請
                    /// </summary>
                    Update = 20,
                    /// <summary>
                    /// 削除申請
                    /// </summary>
                    Delete = 30
                }

                /// <summary>
                /// 変更管理帳票出力対象項目定義(2150) 帳票区分
                /// </summary>
                public enum OutputItemConduct
                {
                    /// <summary>
                    /// 機器台帳
                    /// </summary>
                    HM0001 = 1,
                    /// <summary>
                    /// 長期計画
                    /// </summary>
                    HM0002 = 2,
                    /// <summary>
                    /// 保全部位、保全項目(機器台帳)
                    /// </summary>
                    HM0001Content = 3,
                    /// <summary>
                    /// 保全部位、保全項目(長期計画)
                    /// </summary>
                    HM0002Content = 4
                }

                /// <summary>
                /// 変更管理 申請機能
                /// </summary>
                public enum ApplicationConduct
                {
                    /// <summary>
                    /// 未設定
                    /// </summary>
                    None = 0,
                    /// <summary>
                    /// 機器台帳
                    /// </summary>
                    HM0001 = 1,
                    /// <summary>
                    /// 長期計画
                    /// </summary>
                    HM0002 = 2
                }

                /// <summary>
                /// 処理モード
                /// </summary>
                public enum ProcessMode
                {
                    /// <summary>
                    /// トランザクションモード
                    /// </summary>
                    transaction,
                    /// <summary>
                    /// 変更管理モード
                    /// </summary>
                    history
                }

                /// <summary>
                /// ExcelPort保全活動完了区分
                /// </summary>
                public enum CompletionDivision
                {
                    /// <summary>
                    /// 全件
                    /// </summary>
                    All = 0,
                    /// <summary>
                    /// 未完了
                    /// </summary>
                    Incomplete = 1,
                    /// <summary>
                    /// 完了
                    /// </summary>
                    Completion = 2
                }

                /// <summary>
                /// 部門 修理部門
                /// </summary>
                public enum DepartmentFixDivision
                {
                    /// <summary>
                    /// 全件
                    /// </summary>
                    Fix = 1
                }
            }
        }

        /// <summary>
        /// 添付情報
        /// </summary>
        public class Attachment
        {
            /// <summary>
            /// 機能タイプID
            /// </summary>
            public enum FunctionTypeId
            {
                /// 機器台帳-機番添付
                Machine = 1600,
                /// 機器台帳-機器添付
                Equipment = 1610,
                /// 機器台帳-保全項目一覧-ファイル添付
                Content = 1620,
                /// 機器台帳-MP情報タブ-ファイル添付
                MpInfo = 1630,
                /// 件名別長期計画-件名添付
                LongPlan = 1640,
                /// 保全活動-件名添付
                Summary = 1650,
                /// 保全活動-故障分析情報タブ-略図添付
                HistoryFailureDiagram = 1660,
                /// 保全活動-故障分析情報タブ-故障原因分析書添付
                HistoryFailureAnalyze = 1670,
                /// 保全活動-故障分析情報(個別工場)タブ-略図添付
                HistoryFailureFactDiagram = 1680,
                /// 保全活動-故障分析情報(個別工場)タブ-故障原因分析書添付
                HistoryFailureFactAnalyze = 1690,
                /// 予備品管理-画像添付
                SpareImage = 1700,
                /// 予備品管理-文書添付
                SpareDocument = 1750,
                /// 予備品管理-予備品地図
                SpareMap = 1780
            }
        }

        /// <summary>
        /// 丸め処理区分
        /// </summary>
        public enum RoundDivision : int
        {
            /// <summary>
            /// 四捨五入
            /// </summary>
            Round = 1,
            /// <summary>
            /// 切り上げ
            /// </summary>
            Ceiling = 2,
            /// <summary>
            /// 切り捨て
            /// </summary>
            Floor = 3
        }

        /// <summary>
        /// 送信時処理ID
        /// </summary>
        public static class SendProcessId
        {
            /// <summary>
            /// 登録
            /// </summary>
            public const int Regist = 1;
            /// <summary>
            /// 内容更新
            /// </summary>
            public const int Update = 2;
            /// <summary>
            /// 削除
            /// </summary>
            public const int Delete = 9;

        }
    }
}
