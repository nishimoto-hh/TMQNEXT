using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ScheduleStatus = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleStatus;
using ScheduleDisplayUnit = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleDisplayUnit;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComDataBaseClass = CommonSTDUtil.CommonDataBaseClass;
using ExData = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace CommonTMQUtil
{
    /// <summary>
    /// CommonTMQUtilで使用するデータクラス
    /// </summary>
    public class CommonTMQUtilDataClass
    {
        public class ScheduleList
        {
            /// <summary>
            /// スケジュール(DB取得結果)
            /// </summary>
            public class Get
            {
                /// <summary>Gets or sets 集計キーID</summary>
                /// <value>集計キーID</value>
                public string KeyId { get; set; }
                /// <summary>Gets or sets 集計単位ID</summary>
                /// <value>集計単位ID</value>
                /// <remarks>点検種別が違うとき上位ランクとして検索する範囲で同一の値を設定する</remarks>
                public long? GroupKey { get; set; }
                /// <summary>Gets or sets 同一マークキー</summary>
                /// <value>同一マークキー</value>
                /// <remarks>同一のスケジュールのマークとして扱うキー値を設定する</remarks>
                public string SameMarkKey { get; set; }

                /// <summary>Gets or sets スケジュール日</summary>
                /// <value>スケジュール日</value>
                public DateTime ScheduleDate { get; set; }
                /// <summary>Gets or sets 完了フラグ</summary>
                /// <value>完了フラグ</value>
                public bool Complition { get; set; }
                /// <summary>Gets or sets 点検種別</summary>
                /// <value>点検種別</value>
                public int? MaintainanceKindStructureId { get; set; }
                /// <summary>Gets or sets 点検種別順序</summary>
                /// <value>点検種別順序</value>
                public int? MaintainanceKindLevel { get; set; }
                /// <summary>Gets or sets 点検種別表示文字</summary>
                /// <value>点検種別表示文字</value>
                public string MaintainanceKindChar { get; set; }
                /// <summary>Gets or sets 保全活動件名ID</summary>
                /// <value>保全活動件名ID</value>
                public long? SummaryId { get; set; }
                /// <summary>Gets or sets 長期計画ID</summary>
                /// <value>長期計画ID</value>
                public long? LongPlanId { get; set; }

                /// <summary>Gets or sets 実績金額(k円)</summary>
                /// <value>実績金額(k円)</value>
                public decimal? Expenditure { get; set; }
                /// <summary>Gets or sets 予算金額(k円)</summary>
                /// <value>予算金額(k円)</value>
                public decimal? BudgetAmount { get; set; }

                /// <summary>Gets or sets 保全活動画面新規登録表示用キー</summary>
                /// <value>保全活動画面新規登録表示用キー</value>
                /// <remarks>保全スケジュール詳細画面のキー</remarks>
                public long? NewMaintainanceKey { get; set; }

                /// <summary>
                /// 完了済みかどうか判定
                /// </summary>
                /// <returns>完了済みならTrue</returns>
                public bool IsComplete()
                {
                    return this.Complition == true;
                }

                /// <summary>
                /// 保全活動作成済か判定
                /// </summary>
                /// <returns>作成済みならTrue</returns>
                public bool IsMakedSummary()
                {
                    return this.SummaryId != null;
                }
            }

            /// <summary>
            /// スケジュール(画面表示用)
            /// </summary>
            public class Display
            {
                /// <summary>Gets or sets 集計キーID</summary>
                /// <value>集計キーID</value>
                public string KeyId { get; set; }
                /// <summary>Gets or sets 対象年月日</summary>
                /// <value>対象年月日</value>
                /// <remarks>年単位なら1月1日、年月単位なら1日</remarks>
                public DateTime KeyDate { get; set; }
                /// <summary>Gets or sets 変更前年月日</summary>
                /// <value>変更前年月日</value>
                /// <remarks>登録時のデータ戻しに使用、対象年月日と同様</remarks>
                public DateTime OriginDate { get; set; }
                /// <summary>Gets or sets スケジュール種類</summary>
                /// <value>スケジュール種類</value>
                public ScheduleStatus StatusId { get; set; }
                /// <summary>Gets or sets スケジュール優先順位</summary>
                /// <value>スケジュール優先順位</value>
                /// <remarks>小さいほど優先</remarks>
                public int? StatusPriority { get; set; }

                /// <summary>Gets or sets 点検種別表示文字</summary>
                /// <value>点検種別表示文字</value>
                public string MaintainanceKindChar { get; set; }
                /// <summary>Gets or sets 点検種別順序</summary>
                /// <value>点検種別順序</value>
                /// <remarks>小さいほど優先</remarks>
                public int? MaintainanceKindLevel { get; set; }

                /// <summary>Gets or sets 画面データ変換作業用</summary>
                /// <value>画面データ変換作業用</value>
                private StringBuilder ConvertData { get; set; }

                /// <summary>Gets or sets リンク有無</summary>
                /// <value>リンク有無</value>
                public bool IsLink { get; set; }

                /// <summary>Gets or sets リンク情報</summary>
                /// <value>リンク情報</value>
                public LinkTargetInfo LinkInfo { get; set; }

                /// <summary>Gets or sets 保全活動件名ID</summary>
                /// <value>保全活動件名ID</value>
                public long? SummaryId { get; set; }

                /// <summary>Gets or sets 更新対象のデータが年単位フラグ</summary>
                /// <value>更新対象のデータが年単位フラグ</value>
                /// <remarks>更新時に使用</remarks>
                public bool IsUpdateYear { get; set; }

                /// <summary>Gets or sets 実績金額(k円)</summary>
                /// <value>実績金額(k円)</value>
                public decimal? Expenditure { get; set; }
                /// <summary>Gets or sets 予算金額(k円)</summary>
                /// <value>予算金額(k円)</value>
                public decimal? BudgetAmount { get; set; }

                /// <summary>Gets or sets 集計単位ID</summary>
                /// <value>集計単位ID</value>
                /// <remarks>点検種別が違うとき上位ランクとして検索する範囲で同一の値を設定する(Getと同じ値、△▲のリンク表示用)</remarks>
                public long? GroupKey { get; set; }

                /// <summary>
                /// リンク情報
                /// </summary>
                public class LinkTargetInfo
                {
                    /// <summary>Gets or sets 遷移先機能ID</summary>
                    /// <value>遷移先機能ID</value>
                    public string ConductId { get; set; }
                    /// <summary>Gets or sets 遷移先フォームNo</summary>
                    /// <value>遷移先フォームNo</value>
                    public int FormNo { get; set; }
                    /// <summary>Gets or sets 遷移先タブNo</summary>
                    /// <value>遷移先タブNo</value>
                    public int TabNo { get; set; }
                    /// <summary>
                    /// コンストラクタ
                    /// </summary>
                    /// <param name="pConductId">設定する遷移先機能ID</param>
                    /// <param name="pFormNo">設定する遷移先フォームNo</param>
                    /// <param name="pTabNo">設定する遷移先タブNo</param>
                    public LinkTargetInfo(string pConductId, int pFormNo, int pTabNo)
                    {
                        this.ConductId = pConductId;
                        this.FormNo = pFormNo;
                        this.TabNo = pTabNo;
                    }
                }

                /// <summary>
                /// コンストラクタ(引数なし)
                /// </summary>
                public Display()
                {
                }
                /// <summary>
                /// コンストラクタ(画面から取得した列キーと移動した列の値を指定)
                /// </summary>
                /// <param name="columnKey">列キーの値(例：年月→YM202206、年→Y2023)</param>
                /// <param name="columnValue">移動した列の値(例：年月→4|202205、年→4|2025)</param>
                /// <param name="monthStartNendo">年度開始月</param>
                public Display(string columnKey, string columnValue, int monthStartNendo)
                {
                    // 年月の場合、年の場合で処理を分岐する
                    bool isYear = isKeyYear(columnKey);

                    // 列キーの値から、移動先の年月日を取得
                    // 年月 "YM202206"
                    // 年   "Y2023"
                    int startYearIndex = isYear ? 1 : 2;
                    // 列キーの値の先頭のYMやY以降の値を取得
                    string keyDate = columnKey.Substring(startYearIndex);
                    // 年月日に変換
                    this.KeyDate = convertValueToDatetime(keyDate, isYear);

                    // 列値から、ステータスと移動元年月日を取得　前半がステータス、後半が移動元年月日
                    // 年月 "4|202205"
                    // 年   "4|2025"
                    const int divider = 1; // |の位置
                    // ステータス
                    string status = columnValue.Substring(divider - 1, divider);
                    this.StatusId = ComUtil.StringToEnum<ScheduleStatus>(status); // Enumに変換
                    // 移動元の年月日
                    string originDate = columnValue.Substring(divider + 1);
                    this.OriginDate = convertValueToDatetime(originDate, isYear);

                    // 更新時に使用する判定用値
                    this.IsUpdateYear = isYear;

                    // yyyyMMまたはyyyy形式の文字列を年月日(指定年月の1日、指定年の1月1日)に変換する処理
                    DateTime convertValueToDatetime(string param, bool isYear)
                    {
                        string yearmonth = param;
                        if (isYear)
                        {
                            // 年単位なら年度開始月を追加して年月にする 2023 → 202304
                            yearmonth = param + monthStartNendo.ToString("00");
                        }
                        // 01日を追加し年月日単位にする 202305 → 20230501
                        string convValue = yearmonth + "01";
                        return ComUtil.ConvertDateTimeFromYyyymmddString(convValue) ?? DateTime.Now;
                    }
                }

                /// <summary>
                /// 画面データに変換する処理
                /// </summary>
                /// <returns>変換した文字列</returns>
                public string ConvertScheduleData()
                {
                    // [スケジュール種類]|[点検種別]|[機能ID]|[画面番号]|[タブ番号]|[遷移パラメータ] の形式に変換
                    // リンクが無い場合は、機能ID以降は不要で、[スケジュール種類]|[点検種別]となる

                    string scheduleType = ((int)this.StatusId).ToString(); // スケジュール種類
                    this.ConvertData = new(scheduleType);
                    addText(this.MaintainanceKindChar); // 点検種別
                    if (this.IsLink)
                    {
                        if (this.LinkInfo != null)
                        {
                            // リンクがある場合
                            addText(this.LinkInfo.ConductId); // 機能ID
                            addText(this.LinkInfo.FormNo.ToString()); // フォームNo
                            addText(this.LinkInfo.TabNo.ToString()); // タブNo
                            addText(this.SummaryId.ToString()); // 遷移パラメータ
                        }
                    }
                    // 文字列に変換して返す
                    return this.ConvertData.ToString();

                    // 文字列を追加する処理、区切り文字は|
                    void addText(string add)
                    {
                        this.ConvertData.Append('|');
                        this.ConvertData.Append(add);
                    }
                }

                /// <summary>
                /// 対象年月日の値を画面表示用の列キー値に変換
                /// </summary>
                /// <param name="isYear"></param>
                /// <returns></returns>
                public string GetDateKey(bool isYear)
                {
                    // 月度単位の場合、"YM"+[yyyyMM](「YM202205」)
                    // 年度単位の場合、"Y"+[yyyy](「Y2022」)
                    string head = isYear ? "Y" : "YM";
                    string format = isYear ? "yyyy" : "yyyyMM";
                    string value = this.KeyDate.ToString(format);
                    return head + value;
                }
                /// <summary>
                /// 正規表現一致チェック
                /// </summary>
                /// <param name="value">チェックする値</param>
                /// <param name="compare">チェックするルール</param>
                /// <returns>一致する場合True</returns>
                private static bool isMatch(string value, string compare)
                {
                    Match match = Regex.Match(value, compare);
                    return match.Success;
                }
                /// <summary>
                /// 値が画面表示用の列キーであるか判定
                /// </summary>
                /// <param name="keyValue">判定する値</param>
                /// <returns>列キーであるならTrue</returns>
                public static bool isKeyDate(string keyValue)
                {
                    // 年または年月
                    return isKeyMonth(keyValue) || isKeyYear(keyValue);
                }

                /// <summary>
                /// キーの値が年かどうか判定
                /// </summary>
                /// <param name="keyValue">判定する値</param>
                /// <returns>年の列キーならTrue</returns>
                private static bool isKeyYear(string keyValue)
                {
                    return isMatch(keyValue, "Y\\d{4}");
                }

                /// <summary>
                /// キーの値が年月かどうか判定
                /// </summary>
                /// <param name="keyValue">判定する値</param>
                /// <returns>年月の列キーならTrue</returns>
                private static bool isKeyMonth(string keyValue)
                {
                    return isMatch(keyValue, "YM\\d{6}");
                }
                /// <summary>
                /// 値が画面で移動された列の値であるか判定
                /// </summary>
                /// <param name="columnValue">画面の列の値</param>
                /// <returns>移動された列の値であるならTrue</returns>
                /// <remarks>移動されていないならスケジュール列でもFalse</remarks>
                public static bool isUpdatedMark(string columnValue)
                {
                    return isMonth() || isYear();

                    bool isMonth()
                    {
                        return isMatch(columnValue, "\\d\\|\\d{6}");
                    }

                    bool isYear()
                    {
                        return isMatch(columnValue, "\\d\\|\\d{4}");
                    }
                }
            }

            /// <summary>
            /// 画面の表示条件
            /// </summary>
            public class Condition : ComDataBaseClass.ISysFiscalYear
            {
                /// <summary>Gets or sets スケジュール表示単位</summary>
                /// <value>スケジュール表示単位</value>
                public int ScheduleUnit { get; set; }

                /// <summary>Gets or sets スケジュール表示単位の拡張項目の値</summary>
                /// <value>スケジュール表示単位の拡張項目の値</value>
                public string ExtensionData { get; set; }

                /// <summary>Gets or sets スケジュール表次年度(開始)</summary>
                /// <value>スケジュール表示年度(開始)</value>
                public int ScheduleYearFrom { get; set; }
                /// <summary>Gets or sets スケジュール表次年度(終了)</summary>
                /// <value>スケジュール表示年度(終了)</value>
                public int ScheduleYearTo { get; set; }
                /// <summary>Gets or sets スケジュール表次年度(開始|終了)</summary>
                /// <value>スケジュール表示年度(開始|終了)</value>
                public string ScheduleYear { get; set; }

                /// <summary>Gets or sets スケジュール開始年</summary>
                /// <value>スケジュール開始年月</value>
                public int ScheduleStartYear { get; set; }

                /// <summary>
                /// スケジュール表示単位の列挙型の値を取得する処理
                /// </summary>
                /// <returns></returns>
                public ScheduleDisplayUnit GetScheduleDisplayUnit()
                {
                    if (!string.IsNullOrEmpty(this.ExtensionData))
                    {
                        return ComUtil.StringToEnum<ScheduleDisplayUnit>(this.ExtensionData);
                    }
                    // 取得できない場合、月
                    return ScheduleDisplayUnit.Month;
                }

                /// <summary>
                /// スケジュール表示年度の開始と終了を結合する処理
                /// </summary>
                /// <remarks>画面初期値設定に使用</remarks>
                public void JoinFromTo()
                {
                    this.ScheduleYear = this.ScheduleYearFrom.ToString() + ComConsts.SeparatorFromTo + this.ScheduleYearTo.ToString();
                }
            }

            /// <summary>
            /// SQLの検索条件
            /// </summary>
            public class GetCondition
            {
                /// <summary>Gets or sets スケジュール表示単位</summary>
                /// <value>スケジュール表示単位</value>
                public ScheduleDisplayUnit DisplayUnit { get; set; }
                /// <summary>Gets or sets スケジュール開始年月日</summary>
                /// <value>スケジュール開始年月日</value>
                public DateTime ScheduleStart { get; set; }
                /// <summary>Gets or sets スケジュール終了年月日</summary>
                /// <value>スケジュール終了年月日</value>
                public DateTime ScheduleEnd { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="condition">画面の検索条件</param>
                /// <param name="monthStartNendo">年度開始月</param>
                /// <remarks>画面の検索条件によらず作成が必要なら、コンストラクタを追加すること</remarks>
                public GetCondition(Condition condition, int monthStartNendo)
                {
                    // 表示単位
                    this.DisplayUnit = condition.GetScheduleDisplayUnit();
                    // 開始年月日を指定年と、年度開始月より作成
                    this.ScheduleStart = new DateTime(condition.ScheduleStartYear, monthStartNendo, 1);
                    // 年単位 or 月単位
                    if (this.IsYear())
                    {
                        // 年単位の場合
                        this.ScheduleStart = new DateTime(condition.ScheduleYearFrom, monthStartNendo, 1);
                        this.ScheduleEnd = ComUtil.GetNendoLastDay(condition.ScheduleYearTo, monthStartNendo);
                    }
                    else
                    {
                        // 月単位の場合
                        // 指定年と、年度開始月より作成
                        this.ScheduleStart = new DateTime(condition.ScheduleStartYear, monthStartNendo, 1);
                        // 終了は+11か月(開始月から1年間を表示するため)
                        this.ScheduleEnd = this.ScheduleStart.AddMonths(11);
                        // その月の最終日を設定
                        this.ScheduleEnd = ComUtil.GetDateMonthLastDay(this.ScheduleEnd);
                    }
                }

                /// <summary>
                /// スケジュール表示単位が年単位かどうか判定
                /// </summary>
                /// <returns>年単位ならTrue、月単位ならFalse</returns>
                public bool IsYear()
                {
                    return this.DisplayUnit == ScheduleDisplayUnit.Year;
                }

                /// <summary>
                /// フロント側へ渡す内容へ変換する処理
                /// </summary>
                /// <param name="pNendoText">年度表示用フォーマット文字列"{0}年度"</param>
                /// <param name="pYearStartMonth">年度開始月</param>
                /// <param name="isMovable">移動可能な場合True</param>
                /// <returns>フロントへ設定するDictionary</returns>
                public Dictionary<string, object> ConvertScheduleLayout(string pNendoText, int pYearStartMonth, bool isMovable)
                {
                    // 仕様
                    // Dictionary<string,object>

                    // 表示単位
                    //   key   : "unit"
                    //   value : Year(2) or Month(1) の値

                    // 年月移動
                    //   key   : "move"
                    //   value : 移動可能(1)、移動不可(0)

                    // レイアウトデータ
                    //   key   : "layout"
                    // 月単位の場合
                    //   value : Dictionary<string,object>
                    //              key   : "2022年度" ※年の列ヘッダ
                    //              value : Dictionary<string,string>
                    //                         key   : "202206" ※yyyyMM
                    //                         value : "6" ※月の列ヘッダ
                    // 年単位の場合
                    //   value : Dictionary<string,object>
                    //                  key   : "2022"
                    //                  value : "2022年度"

                    // layout の valueとなるディクショナリ
                    Dictionary<string, object> columnLayout = new();
                    // 開始年月日から終了年月日までaddMonthで繰り返し処理を行う、そのステップ値
                    int addMonths = this.IsYear() ? 12 : 1; // 年単位なら12か月、月単位なら1か月

                    // 年度変更時、ディクショナリに値を追加、その判定用
                    bool isNendoChange = false; // 年度変更フラグ
                    string tempNendo = string.Empty; // 年度の記憶用変数

                    // 開始年月日から終了年月日まで1か月(年単位なら12か月)毎に繰り返し
                    for (DateTime date = this.ScheduleStart; date <= this.ScheduleEnd; date = date.AddMonths(addMonths))
                    {
                        string nendo = getNendoYear(date); // 年月日より年度を取得
                        if (tempNendo != nendo)
                        {
                            // 記憶している値と違うか確認
                            isNendoChange = true;
                            tempNendo = nendo;
                        }

                        if (this.IsYear())
                        {
                            // 年単位
                            // key : "2022" , value : "2022年度"
                            columnLayout.Add(nendo, getNendoYearDisplay(nendo));
                            isNendoChange = false; // 年単位は常に年が変更
                        }
                        else
                        {
                            string keyYearDic = getNendoYearDisplay(nendo);
                            // 月単位
                            if (isNendoChange)
                            {
                                // 年度変更時
                                // 新しい年の要素を追加
                                columnLayout.Add(keyYearDic, new Dictionary<string, string>());
                                isNendoChange = false;
                            }
                            // キー("2022年度")でディクショナリを取得し、追加 key : "202206" value : 6
                            Dictionary<string, string> getYearDic = (Dictionary<string, string>)columnLayout[keyYearDic];
                            getYearDic.Add(date.ToString("yyyyMM"), date.Month.ToString());
                        }
                    }
                    // フロントへ渡すDictionary
                    Dictionary<string, object> scheduleLayout = new();
                    scheduleLayout.Add("unit", (int)this.DisplayUnit); // 表示単位
                    scheduleLayout.Add("move", isMovable ? 1 : 0); // 移動可能
                    scheduleLayout.Add("layout", columnLayout); // レイアウトデータ

                    return scheduleLayout;

                    // 年月日から年度を取得する処理
                    string getNendoYear(DateTime target)
                    {
                        // 年度開始日を取得
                        DateTime start = ComUtil.GetNendoStartDay(target, pYearStartMonth);
                        return start.Year.ToString();
                    }
                    // 年度を表示用の書式に整える処理("2022" + "{0}年度" → "2022年度")
                    string getNendoYearDisplay(string nendo)
                    {
                        string result = string.Format(pNendoText, nendo);
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// ファイル入出力管理クラス
        /// </summary>
        public class InoutDefine
        {
            /// <summary>Gets or sets 項目ID</summary>
            /// <value>項目ID</value>
            public int ItemId { get; set; }
            /// <summary>Gets or sets カラム名</summary>
            /// <value>カラム名</value>
            public string ColumnName { get; set; }
            /// <summary>Gets or sets 出力方式</summary>
            /// <value>出力方式</value>
            public int? OutputMethod { get; set; }
            /// <summary>Gets or sets 連続出力間隔</summary>
            /// <value>連続出力間隔</value>
            public int? ContinuousOutputInterval { get; set; }
            /// <summary>Gets or sets 項目名</summary>
            /// <value>項目名</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets デフォルトセル行No</summary>
            /// <value>デフォルトセル行No</value>
            public int? DefaultCellRowNo { get; set; }
            /// <summary>Gets or sets デフォルトセル列No</summary>
            /// <value>デフォルトセル列No</value>
            public int? DefaultCellColumnNo { get; set; }
            /// <summary>Gets or sets 出力デフォルトセル行No</summary>
            /// <value>出力デフォルトセル行No</value>
            public int? OutputDefaultCellRowNo { get; set; }
            /// <summary>Gets or sets 出力デフォルトセル列No</summary>
            /// <value>出力デフォルトセル列No</value>
            public int? OutputDefaultCellColumnNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DisplayOrder { get; set; }
            /// <summary>Gets or sets データ種別</summary>
            /// <value>データ種別</value>
            public int? DataType { get; set; }
            /// <summary>Gets or sets 入出力開始行番号</summary>
            /// <value>入出力開始行番号</value>
            public int? StartRowNo { get; set; }
            /// <summary>Gets or sets 入出力開始列番号</summary>
            /// <value>入出力開始列番号</value>
            public int? StartColNo { get; set; }
            /// <summary>Gets or sets 項目表示名</summary>
            /// <value>項目表示名</value>
            public string ItemDisplayName { get; set; }
            /// <summary>Gets or sets 取込対象フラグ</summary>
            /// <value>取込対象フラグ</value>
            public bool? InputFlg { get; set; }
            /// <summary>Gets or sets 桁数</summary>
            /// <value>桁数</value>
            public int? MaxLength { get; set; }
            /// <summary>Gets or sets 取込時必須</summary>
            /// <value>取込時必須</value>
            public bool? NullCheckFlg { get; set; }
            /// <summary>Gets or sets 行数</summary>
            /// <value>行数</value>
            public int RecordCount { get; set; }
            /// <summary>Gets or sets 入出力方式</summary>
            /// <value>入出力方式</value>
            public int? DataDirection { get; set; }
            /// <summary>Gets or sets 書式文字列</summary>
            /// <value>書式文字列</value>
            public string FormatText { get; set; }
        }

        /// <summary>
        /// エクセル出力テストクラス
        /// </summary>
        public class ExcelOutPutTest
        {
            /// <summary>Gets or sets 工程コード</summary>
            /// <value>工程コード</value>
            public string ProcessCd { get; set; }
            /// <summary>Gets or sets 日付</summary>
            /// <value>日付</value>
            public string Date { get; set; }
            /// <summary>Gets or sets 製品コード</summary>
            /// <value>製品コード</value>
            public string ProductCd { get; set; }
            /// <summary>Gets or sets 生産量</summary>
            /// <value>生産量</value>
            public int? Qty { get; set; }
            /// <summary>Gets or sets 担当者</summary>
            /// <value>担当者</value>
            public string TantoName { get; set; }
        }

        /// <summary>
        /// ファイルダウンロード情報
        /// </summary>
        public class FileDownloadInfo
        {
            /// <summary>Gets or sets機能タイプID</summary>
            /// <value>機能タイプID</value>
            public int FunctionTypeId { get; set; }
            /// <summary>Gets or setsキーID</summary>
            /// <value>キーID</value>
            public int KeyId { get; set; }
            /// <summary>Gets or sets 添付ID</summary>
            /// <value>添付ID</value>
            public long AttachmentId { get; set; }

            public FileDownloadInfo(string fileDonwloadInfo)
            {
                // 添付ID_機能タイプID_キーID_ファイル/リンク区分
                // _で分割し添付ID、機能タイプID、キーIDを取得する。区分は使用しないので不要。
                string[] infos = fileDonwloadInfo.Split(TMQConsts.FileInfoSplit.Key);
                this.AttachmentId = long.Parse(infos[0]);
                this.FunctionTypeId = int.Parse(infos[1]);
                this.KeyId = int.Parse(infos[2]);
            }
        }

        /// <summary>
        /// 予備品在庫登録クラス
        /// </summary>
        public class PartsInventory
        {
            /// <summary>
            /// Enumの数値の値を文字列に変換
            /// </summary>
            /// <param name="value">列挙型の数値の値</param>
            /// <returns>文字列に変換した値</returns>
            public static string GetEnumNumberToString(int value)
            {
                return value.ToString();
            }

            /// <summary>
            /// 受払履歴登録用インタフェース
            /// </summary>
            /// <remarks>機能に応じた定数の項目を設定する</remarks>
            public interface IInoutHistory
            {
                /// <summary>受払区分</summary>
                public string InoutDivision { get; set; }
                /// <summary>作業区分</summary>
                public string WorkDivision { get; set; }

                /// <summary>受払区分と作業区分を設定する処理</summary>
                public abstract void SetDivisions();
            }

            /// <summary>
            /// 在庫データ更新用インタフェース
            /// </summary>
            /// <remarks>在庫データの現在庫数に入出庫数を増減して在庫数を更新する処理がある機能に実装</remarks>
            public interface ILocationStockCalc
            {
                /// <summary>Gets or sets 在庫管理id</summary>
                /// <value>在庫管理id</value>
                public long InventoryControlId { get; set; }
                /// <summary>Gets or sets 在庫数</summary>
                /// <value>在庫数</value>
                public decimal StockQuantity { get; set; }
                /// <summary>Gets or sets 入出庫数</summary>
                /// <value>入出庫数</value>
                public decimal InoutQuantity { get; set; }
            }

            /// <summary>
            /// 入庫入力登録用クラス
            /// </summary>
            public class Input : ComDataBaseClass.CommonTableItem, IInoutHistory
            {
                #region 呼出時に機能側で設定が必要な項目
                /// <summary>Gets or sets 予備品id</summary>
                /// <value>予備品id</value>
                public long PartsId { get; set; }
                /// <summary>Gets or sets 部門id</summary>
                /// <value>部門id</value>
                public long DepartmentStructureId { get; set; }
                /// <summary>Gets or sets 勘定科目id</summary>
                /// <value>勘定科目id</value>
                public long AccountStructureId { get; set; }
                /// <summary>Gets or sets 管理区分</summary>
                /// <value>管理区分</value>
                public string ManagementDivision { get; set; }
                /// <summary>Gets or sets 管理№</summary>
                /// <value>管理№</value>
                public string ManagementNo { get; set; }
                /// <summary>Gets or sets 入庫日</summary>
                /// <value>入庫日</value>
                public DateTime ReceivingDatetime { get; set; }
                /// <summary>Gets or sets 入庫数</summary>
                /// <value>入庫数</value>
                public decimal InoutQuantity { get; set; }
                /// <summary>Gets or sets 入庫単価</summary>
                /// <value>入庫単価</value>
                public decimal? UnitPrice { get; set; }
                /// <summary>Gets or sets 棚id</summary>
                /// <value>棚id</value>
                public long PartsLocationId { get; set; }
                /// <summary>Gets or sets 棚枝番</summary>
                /// <value>棚枝番</value>
                public string PartsLocationDetailNo { get; set; }
                /// <summary>Gets or sets 新旧区分id</summary>
                /// <value>新旧区分id</value>
                public long OldNewStructureId { get; set; }
                /// <summary>Gets or sets 仕入先id</summary>
                /// <value>仕入先id</value>
                public long VenderStructureId { get; set; }

                #endregion

                #region 以下は処理で使用するので設定不要です
                /// <summary>Gets or sets ロット管理id</summary>
                /// <value>ロット管理id</value>
                public long LotControlId { get; set; }
                /// <summary>Gets or sets 在庫管理id</summary>
                /// <value>在庫管理id</value>
                public long InventoryControlId { get; set; }

                /// <summary>受払区分</summary>
                public string InoutDivision { get; set; }
                /// <summary>作業区分</summary>
                public string WorkDivision { get; set; }
                /// <summary>Gets or sets 受払日時</summary>
                /// <value>受払日時</value>
                public DateTime InoutDatetime { get; set; }
                /// <summary>Gets or sets 在庫数</summary>
                /// <value>在庫数</value>
                public decimal StockQuantity { get; set; }
                /// <summary>Gets or sets 作業No</summary>
                /// <value>作業No</value>
                public long WorkNo { get; set; }
                #endregion

                /// <summary>登録前処理</summary>
                public void SetRegistInfo()
                {
                    // 受払履歴の区分を設定
                    SetDivisions();

                    // 名前が違う項目は値をコピー
                    InoutDatetime = ReceivingDatetime;
                    StockQuantity = InoutQuantity;
                }

                /// <summary>
                /// 修正登録の場合、ロット情報に関わる情報が更新されたかどうか判定する
                /// </summary>
                /// <param name="lot">DBより取得した、更新対象のロット情報テーブルの内容</param>
                /// <returns>ロット情報に関わる情報が更新された場合はTrue</returns>
                public bool IsChangedLotInfo(TMQCommonDataClass.PtLotEntity lot)
                {
                    // 全項目の比較を書くのは大変なので、配列にしてまとめて比較する
                    // 入力された情報の内、ロット情報に関わる情報のリスト
                    object[] inputProperties = { this.OldNewStructureId, this.DepartmentStructureId, this.AccountStructureId, this.ManagementDivision, this.ManagementNo, this.ReceivingDatetime, this.UnitPrice, this.VenderStructureId };
                    // DBのロット情報の値のリスト
                    object[] lotProperties = { lot.OldNewStructureId, lot.DepartmentStructureId, lot.AccountStructureId, lot.ManagementDivision, lot.ManagementNo, lot.ReceivingDatetime, lot.UnitPrice, lot.VenderStructureId };
                    // リストを繰り返し、いずれかが異なる場合はTrueを返す
                    for (int i = 0; i < inputProperties.Length; i++)
                    {
                        // 値を比較
                        if (!Equals(inputProperties[i], lotProperties[i]))
                        {
                            return true;
                        }
                    }
                    // すべてが同じ場合はFalseを返す
                    return false;
                }

                public void SetDivisions()
                {
                    // 受入区分：受入
                    InoutDivision = GetEnumNumberToString((int)ExData.InoutDivision.In);
                    // 作業区分：入庫
                    WorkDivision = GetEnumNumberToString((int)ExData.WorkDivision.In);
                }
            }

            /// <summary>
            /// 出庫入力登録用クラス
            /// </summary>
            public class Output : ComDataBaseClass.CommonTableItem, IInoutHistory, ILocationStockCalc
            {
                #region 呼出時に機能側で設定が必要な項目
                /// <summary>Gets or sets ロット管理id</summary>
                /// <value>ロット管理id</value>
                public long LotControlId { get; set; }
                /// <summary>Gets or sets 在庫管理id</summary>
                /// <value>在庫管理id</value>
                public long InventoryControlId { get; set; }
                /// <summary>Gets or sets 出庫日</summary>
                /// <value>出庫日</value>
                public DateTime InoutDatetime { get; set; }
                /// <summary>Gets or sets 出庫数</summary>
                /// <value>出庫数</value>
                public decimal InoutQuantity { get; set; }
                /// <summary>Gets or sets 棚id</summary>
                /// <value>棚id</value>
                public long PartsLocationId { get; set; }
                /// <summary>Gets or sets 棚枝番</summary>
                /// <value>棚枝番</value>
                public string PartsLocationDetailNo { get; set; }
                /// <summary>Gets or sets 出庫区分</summary>
                /// <value>出庫区分</value>
                public long ShippingDivisionStructureId { get; set; }
                #endregion

                #region 以下は処理で使用するので設定不要です
                /// <summary>受払区分</summary>
                public string InoutDivision { get; set; }
                /// <summary>作業区分</summary>
                public string WorkDivision { get; set; }
                /// <summary>Gets or sets 作業No</summary>
                /// <value>作業No</value>
                public long WorkNo { get; set; }
                /// <summary>Gets or sets 在庫数</summary>
                /// <value>在庫数</value>
                public decimal StockQuantity { get; set; }
                #endregion

                public void SetDivisions()
                {
                    // 受入区分：払出
                    InoutDivision = GetEnumNumberToString((int)ExData.InoutDivision.Out);
                    // 作業区分：出庫
                    WorkDivision = GetEnumNumberToString((int)ExData.WorkDivision.Out);
                }
            }
            /// <summary>
            /// 棚番移庫入力登録用クラス
            /// </summary>
            public class MoveLocation : ComDataBaseClass.CommonTableItem, IInoutHistory, ILocationStockCalc
            {
                #region 呼出時に機能側で設定が必要な項目
                /// <summary>Gets or sets ロット管理id</summary>
                /// <value>ロット管理id</value>
                public long LotControlId { get; set; }
                /// <summary>Gets or sets 在庫管理id</summary>
                /// <value>在庫管理id</value>
                public long InventoryControlId { get; set; }
                /// <summary>Gets or sets 移庫日</summary>
                /// <value>移庫日</value>
                public DateTime InoutDatetime { get; set; }
                /// <summary>Gets or sets 移庫数</summary>
                /// <value>移庫数</value>
                public decimal InoutQuantity { get; set; }
                /// <summary>Gets or sets 棚id</summary>
                /// <value>棚id</value>
                public long PartsLocationId { get; set; }
                /// <summary>Gets or sets 棚枝番</summary>
                /// <value>棚枝番</value>
                public string PartsLocationDetailNo { get; set; }
                #endregion

                #region 以下は処理で使用するので設定不要です
                /// <summary>受払区分</summary>
                public string InoutDivision { get; set; }
                /// <summary>作業区分</summary>
                public string WorkDivision { get; set; }
                /// <summary>Gets or sets 作業No</summary>
                /// <value>作業No</value>
                public long WorkNo { get; set; }
                /// <summary>Gets or sets 在庫数</summary>
                /// <value>在庫数</value>
                public decimal StockQuantity { get; set; }
                /// <summary>Gets or sets 出庫区分</summary>
                /// <value>出庫区分</value>
                /// <remarks>他機能と共用するSQLの兼ね合いで追加、値はNULL</remarks>
                public long? ShippingDivisionStructureId { get; set; }
                /// <summary>Gets or sets 予備品id</summary>
                /// <value>予備品id</value>
                public long PartsId { get; set; }
                #endregion

                [Obsolete("このクラスでは使用不能のため、引数有のメソッドを使用してください。", true)]
                public void SetDivisions()
                {
                    // 移庫元と移庫先で処理を切り替えるため未使用
                }

                /// <summary>
                /// 受払区分と作業区分を設定
                /// </summary>
                /// <param name="isSource">移庫元ならTrue、移庫先ならFalse</param>
                public void SetDivisions(bool isSource)
                {
                    // 受入区分：移庫元なら払出、移庫先なら受入
                    ExData.InoutDivision inout = isSource ? ExData.InoutDivision.Out : ExData.InoutDivision.In;
                    InoutDivision = GetEnumNumberToString((int)inout);
                    // 作業区分：棚番移庫
                    WorkDivision = GetEnumNumberToString((int)ExData.WorkDivision.MoveStock);
                }
            }
            /// <summary>
            /// 部門移庫入力登録用クラス
            /// </summary>
            public class MoveDepartment : ComDataBaseClass.CommonTableItem, IInoutHistory
            {
                #region 呼出時に機能側で設定が必要な項目
                /// <summary>Gets or sets ロット管理id</summary>
                /// <value>ロット管理id</value>
                public long LotControlId { get; set; }
                /// <summary>Gets or sets 部門id</summary>
                /// <value>部門id</value>
                public long DepartmentStructureId { get; set; }
                /// <summary>Gets or sets 勘定科目id</summary>
                /// <value>勘定科目id</value>
                public long AccountStructureId { get; set; }
                /// <summary>Gets or sets 管理区分</summary>
                /// <value>管理区分</value>
                public string ManagementDivision { get; set; }
                /// <summary>Gets or sets 管理№</summary>
                /// <value>管理№</value>
                public string ManagementNo { get; set; }
                /// <summary>Gets or sets 移庫日</summary>
                /// <value>移庫日</value>
                public DateTime InoutDatetime { get; set; }
                /// <summary>Gets or sets 移庫単価</summary>
                /// <value>移庫単価</value>
                public double? UnitPrice { get; set; }
                #endregion


                #region 以下は処理で使用するので設定不要です
                /// <summary>Gets or sets 在庫管理id</summary>
                /// <value>在庫管理id</value>
                public long InventoryControlId { get; set; }
                /// <summary>受払区分</summary>
                public string InoutDivision { get; set; }
                /// <summary>作業区分</summary>
                public string WorkDivision { get; set; }
                /// <summary>Gets or sets 作業No</summary>
                /// <value>作業No</value>
                public long WorkNo { get; set; }
                /// <summary>Gets or sets 移庫数</summary>
                /// <value>移庫数</value>
                public decimal InoutQuantity { get; set; }
                /// <summary>Gets or sets 出庫区分</summary>
                /// <value>出庫区分</value>
                /// <remarks>他機能と共用するSQLの兼ね合いで追加、値はNULL</remarks>
                public long? ShippingDivisionStructureId { get; set; }
                #endregion

                [Obsolete("このクラスでは使用不能のため、引数有のメソッドを使用してください。", true)]
                public void SetDivisions()
                {
                    // 移庫元と移庫先で処理を切り替えるため未使用
                }

                /// <summary>
                /// 受払区分と作業区分を設定
                /// </summary>
                /// <param name="isSource">移庫元ならTrue、移庫先ならFalse</param>
                public void SetDivisions(bool isSource)
                {
                    // 受入区分：移庫元なら払出、移庫先なら受入
                    ExData.InoutDivision inout = isSource ? ExData.InoutDivision.Out : ExData.InoutDivision.In;
                    InoutDivision = GetEnumNumberToString((int)inout);
                    // 作業区分：部門移庫
                    WorkDivision = GetEnumNumberToString((int)ExData.WorkDivision.MoveDepartment);
                }
            }
            /// <summary>
            /// 棚卸登録用クラス
            /// </summary>
            public class TakeInventory : ComDataBaseClass.CommonTableItem, ILocationStockCalc
            {
                /// <summary>Gets or sets 棚差調整ID</summary>
                /// <value>棚差調整ID</value>
                /// <remarks>棚差調整データのキー値</remarks>
                public long InventoryDifferenceId { get; set; }

                /// <summary>Gets or sets 棚卸ID</summary>
                /// <value>棚卸ID</value>
                /// <remarks>棚卸データのキー値</remarks>
                public long InventoryId { get; set; }
                /// <summary>Gets or sets 入出庫日時</summary>
                /// <value>入出庫日時</value>
                /// <remarks>画面の対象年月の最終日</remarks>
                public DateTime? InoutDatetime { get; set; }
                /// <summary>Gets or sets ロット管理id</summary>
                /// <value>ロット管理id</value>
                public long LotControlId { get; set; }
                /// <summary>Gets or sets 在庫管理id</summary>
                /// <value>在庫管理id</value>
                public long InventoryControlId { get; set; }
                /// <summary>Gets or sets 在庫数</summary>
                /// <value>在庫数</value>
                public decimal StockQuantity { get; set; }
                /// <summary>Gets or sets 入出庫数</summary>
                /// <value>入出庫数</value>
                public decimal InoutQuantity { get; set; }
                /// <summary>Gets or sets 作業No</summary>
                /// <value>作業No</value>
                public long WorkNo { get; set; }
                /// <summary>Gets or sets 受払履歴ID</summary>
                /// <value>受払履歴ID</value>
                public long InoutHistoryId { get; set; }
            }
        }

        /// <summary>
        /// 変更管理 背景色変更共通クラス
        /// </summary>
        public interface IHistoryManagementCommon
        {
            /// <summary>Gets or sets 変更のあった項目(District_10|Series_30...)</summary>
            /// <value>変更のあった項目(District_10|Series_30...)</value>
            public string ValueChanged { get; set; }

            #region 詳細画面のボタン非表示処理で使用
            /// <summary>Gets or sets ボタン非表示制御フラグ(申請の申請者かシステム管理者の場合はTrue)</summary>
            /// <value>ボタン非表示制御フラグ(申請の申請者かシステム管理者の場合はTrue)</value>
            public bool IsCertified { get; set; }
            /// <summary>Gets or sets ボタン表示制御フラグ(変更管理IDが紐付く機番情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDの場合はTrue)</summary>
            /// <value>ボタン表示制御フラグ(変更管理IDが紐付く機番情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDの場合はTrue)</value>
            public bool IsCertifiedFactory { get; set; }
            #endregion

            #region 地区・職種機種(変更管理テーブル)
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? DistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string DistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
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
            #endregion

            #region 地区・職種機種(トランザクションテーブル)
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? OldLocationStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? OldJobStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? OldDistrictId { get; set; }
            /// <summary>Gets or sets 地区名称</summary>
            /// <value>地区名称</value>
            public string OldDistrictName { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? OldFactoryId { get; set; }
            /// <summary>Gets or sets 工場名称</summary>
            /// <value>工場名称</value>
            public string OldFactoryName { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? OldPlantId { get; set; }
            /// <summary>Gets or sets プラント名称</summary>
            /// <value>プラント名称</value>
            public string OldPlantName { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? OldSeriesId { get; set; }
            /// <summary>Gets or sets 系列名称</summary>
            /// <value>系列名称</value>
            public string OldSeriesName { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? OldStrokeId { get; set; }
            /// <summary>Gets or sets 工程名称</summary>
            /// <value>工程名称</value>
            public string OldStrokeName { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? OldFacilityId { get; set; }
            /// <summary>Gets or sets 設備名称</summary>
            /// <value>設備名称</value>
            public string OldFacilityName { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int OldJobId { get; set; }
            /// <summary>Gets or sets 職種名称</summary>
            /// <value>職種名称</value>
            public string OldJobName { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? OldLargeClassficationId { get; set; }
            /// <summary>Gets or sets 機種大分類名称</summary>
            /// <value>機種大分類名称</value>
            public string OldLargeClassficationName { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? OldMiddleClassficationId { get; set; }
            /// <summary>Gets or sets 機種中分類名称</summary>
            /// <value>機種中分類名称</value>
            public string OldMiddleClassficationName { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? OldSmallClassficationId { get; set; }
            /// <summary>Gets or sets 機種小分類名称</summary>
            /// <value>機種小分類名称</value>
            public string OldSmallClassficationName { get; set; }
            #endregion

        }









    }
}
