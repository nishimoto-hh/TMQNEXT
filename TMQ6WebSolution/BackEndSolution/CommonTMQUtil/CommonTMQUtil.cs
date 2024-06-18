using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using TMQDataClass = CommonTMQUtil.TMQCommonDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using Dao = CommonTMQUtil.CommonTMQUtilDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using System.Dynamic;
using System.Reflection;
using CommonSTDUtil.CommonSTDUtil;
using ComRes = CommonSTDUtil.CommonResources;
using STDDao = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using ScheduleStatus = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleStatus;
using System.Text.RegularExpressions;
using ScheduleDisplayUnit = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleDisplayUnit;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
using StructureGroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;

/// <summary>
/// TMQ用共通ユーティリティクラス
/// </summary>
namespace CommonTMQUtil
{
    /// <summary>
    /// TMQ用共通ユーティリティクラス
    /// </summary>
    public static partial class CommonTMQUtil
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL名：構成リスト取得</summary>
            public const string GetStructureList = "Select_StructureList";
            /// <summary>SQL名：上位の構成リスト取得 </summary>
            public const string GetUpperStructureList = "Select_UpperStructureList";
            /// <summary>SQL名：構成拡張項目取得(ソート)</summary>
            public const string GetStructureExtDataOrdered = "Select_GetStructureExtDataOrderByItemOrder";
            /// <summary>SQL名：構成アイテム情報取得</summary>
            public const string GetStructureItemExDataList = "Select_StructureItemExDataList";
            /// <summary>SQL名：予備品IDより在庫確定日を取得</summary>
            public const string SelectInventoryConfirmationDate = "Select_InventoryConfirmationDate";
            /// <summary>SQL名：予備品ID、新旧区分、部門より棚卸確定日を取得</summary>
            public const string SelectTakeInventoryConfirmationDate = "Select_TakeInventoryConfirmationDate";
            /// <summary>シーケンスの値を進め、取得</summary>
            public const string GetNextSequence = "GetNextSequence";
            /// <summary>棚IDより翻訳を取得</summary>
            public const string GetPartsLocationTranslation = "GetPartsLocationTranslation";
            /// <summary>SQL名：添付ファイル取得用一時テーブル作成</summary>
            public const string CreateTableTempAttachment = "CreateTableTempAttachment";
            /// <summary>SQL名：添付ファイル取得用一時テーブル登録</summary>
            public const string InsertTempAttachment = "InsertTempAttachment";
            /// <summary>SQL名：翻訳取得用一時テーブル作成</summary>
            public const string CreateTableTempTranslation = "CreateTableTempTranslation";
            /// <summary>SQL名：翻訳取得用一時テーブル登録</summary>
            public const string InsertTempTranslation = "InsertTempTranslation";
            /// <summary>SQL名：翻訳取得用一時テーブル登録(階層指定)</summary>
            public const string InsertTempTranslationLayer = "InsertTempTranslationLayer";
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = "Common";
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDirPerformance = SqlName.SubDir + @"\ListPerformance";
        }
        /// <summary>
        /// SQLファイル名称(マスタ機能で使用)
        /// </summary>
        private static class SqlNameMaster
        {
            /// <summary>ユーザIDよりユーザ所属マスタの全ての工場を取得</summary>
            public const string GetUserFactory = "GetUserFactory";
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = "Master";
        }
        #endregion

        /// <summary>
        /// 固定SQL文の取得
        /// </summary>
        /// <param name="subDir">Resources\sql配下のサブディレクトリパス(サブディレクトリが複数階層の場合、パス区切り文字は"\"ではなく".")</param>
        /// <param name="fileName">SQLテキストファイル名</param>
        /// <param name="sql">out 取得したSQL文</param>
        /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
        /// <returns>取得結果(true:取得OK/false:取得NG )</returns>
        public static bool GetFixedSqlStatement(string subDir, string fileName, out string sql, List<string> listUnComment = null)
        {
            sql = string.Empty;
            string assemblyName = CommonWebTemplate.AppCommonObject.Config.AppSettings.FixedSqlStatementAssemblyName;

            // リソース名を生成
            StringBuilder resourceName = new StringBuilder();
            resourceName.Append(assemblyName).Append(".").Append(CommonWebTemplate.AppCommonObject.Config.AppSettings.FixedSqlStatementDir).Append(".");
            if (!string.IsNullOrEmpty(subDir))
            {
                // サブディレクトリパスの追加(念のためパス区切り文字を"."に変換しておく)
                resourceName.Append(subDir.Replace(@"\", ".")).Append(".");
            }
            resourceName.Append(fileName).Append(".sql");

            // 埋め込みリソースからSQL文を取得
            bool result = ComUtil.GetEmbeddedResourceStr(assemblyName, resourceName.ToString(), out sql);

            // SQLの動的制御　コメントアウトを解除
            if (listUnComment != null && listUnComment.Count > 0)
            {
                foreach (string wordUnComment in listUnComment)
                {
                    // @+指定された文字列がコメントアウトの前後に付いているので除去
                    // /*@Hoge と @Hoge*/ を除去すれば、囲われたSQLが有効になる
                    Regex startReplace = new Regex("\\/\\*@" + wordUnComment + "[^a-zA-Z\\d_]");
                    sql = startReplace.Replace(sql, string.Empty).Replace("@" + wordUnComment + "*/", string.Empty);
                }
            }

            return result;
        }

        /// <summary>
        /// 検索に用いるSQLのWITH句を取得する処理
        /// </summary>
        /// <param name="subDir">Resources\sql配下のサブディレクトリパス(サブディレクトリが複数階層の場合、パス区切り文字は"\"ではなく".")</param>
        /// <param name="fileName">SQLテキストファイル名</param>
        /// <param name="sql">取得したSQL文</param>
        /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
        /// <returns>取得結果(true:取得OK/false:取得NG )</returns>
        /// <returns></returns>
        public static bool GetFixedSqlStatementWith(string subDir, string fileName, out string sql, List<string> listUnComment = null)
        {
            // ファイル名にWIHT_を接続して取得
            if (!GetFixedSqlStatement(subDir, "WITH_" + fileName, out sql, listUnComment))
            {
                // エラー
                return false;
            }
            return true;
        }

        /// <summary>
        /// 検索に用いるSQL文の生成
        /// </summary>
        /// <param name="isCount">件数を取得する場合はTrue</param>
        /// <param name="selectSql">SELECT句のSQL文字列</param>
        /// <param name="whereSql">WHERE句のSQL文字列</param>
        /// <param name="withSql">省略可能 WITH句のSQL文字列</param>
        /// <param name="isDetailConditionApplied">詳細検索条件適用フラグ</param>
        /// <param name="dispCount">表示件数</param>
        /// <returns>生成した検索に用いるSQL文</returns>
        public static string GetSqlStatementSearch(bool isCount, string selectSql, string whereSql, string withSql = null, bool isDetailConditionApplied = false, long dispCount = -1)
        {
            StringBuilder sbSql = new();
            // WITH句がある場合は結合
            if (!string.IsNullOrEmpty(withSql))
            {
                sbSql.AppendLine(withSql);
            }
            sbSql.Append("SELECT ");
            if (isCount)
            {
                // 件数取得の場合
                sbSql.Append("COUNT(*)");
            }
            else
            {
                // 一覧データ取得の場合
                //詳細検索条件が指定されていない場合のみ、表示件数を絞る
                if (isDetailConditionApplied || dispCount == -1)
                {
                    sbSql.Append("*");
                }
                else
                {
                    sbSql.Append("TOP " + dispCount.ToString() + " *");
                }
            }
            sbSql.AppendLine(" FROM (");
            sbSql.AppendLine(selectSql);
            sbSql.AppendLine(") tbl");
            if (!string.IsNullOrEmpty(whereSql))
            {
                sbSql.AppendLine(whereSql);
            }

            return sbSql.ToString();
        }

        /// <summary>
        /// 検索に用いる固定SQL文の取得
        /// </summary>
        /// <param name="isCount">件数を取得する場合はTrue</param>
        /// <param name="subDir">Resources\sql配下のサブディレクトリパス(サブディレクトリが複数階層の場合、パス区切り文字は"\"ではなく".")</param>
        /// <param name="fileName">SQLテキストファイル名</param>
        /// <param name="sql">取得したSQL文</param>
        /// <param name="isWith">With句がある場合、True(省略時はFalse)</param>
        /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
        /// <returns>取得結果(true:取得OK/false:取得NG )</returns>
        [Obsolete("検索時のSQL取得は見直しのためこのメソッドは削除予定")]
        public static bool GetFixedSqlStatementSearch(bool isCount, string subDir, string fileName, out string sql, bool isWith = false, List<string> listUnComment = null)
        {
            // 検索の場合は通常の検索結果と検索件数の取得があり、From句を共有するために、From句とsとSelect句が分けて定義されている。
            // From句を取得し、Countの場合は「select count(*)」を、通常の検索の場合はファイルより取得したSelect句を結合する
            sql = string.Empty;

            // WITH句を取得
            string withSql = string.Empty;
            if (isWith && !GetFixedSqlStatement(subDir, "WITH_" + fileName, out withSql, listUnComment))
            {
                // エラー
                return false;
            }
            // FROM句を取得
            if (!GetFixedSqlStatement(subDir, "FROM_" + fileName, out string fromSql, listUnComment))
            {
                // エラー
                return false;
            }
            // SQLをWith句、Select句、From句を結合して作成する
            StringBuilder sbSql = new StringBuilder(withSql);

            if (isCount)
            {
                // 件数取得の場合、SQLを生成して終了
                sbSql.AppendLine("select count(*) ");
                sbSql.AppendLine(fromSql);
                sql = sbSql.ToString();
                return true;
            }

            // SELECT句を取得
            if (!GetFixedSqlStatement(subDir, "SELECT_" + fileName, out string selectSql, listUnComment))
            {
                // エラー
                return false;
            }
            sbSql.AppendLine(selectSql);
            sbSql.AppendLine(fromSql);
            sql = sbSql.ToString();
            return true;
        }

        /// <summary>
        /// 検索に用いるSQL文の生成
        /// </summary>
        /// <param name="isCount">件数を取得する場合はTrue</param>
        /// <param name="selectSql">SELECT句のSQL文字列</param>
        /// <param name="whereSql">WHERE句のSQL文字列</param>
        /// <param name="sql">生成したSQL文</param>
        [Obsolete("削除予定、GetSqlStatementSearchを使用")]
        public static void CreateSqlStatementSearch(bool isCount, string selectSql, string whereSql, out string sql)
        {
            sql = string.Empty;

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT ");
            if (isCount)
            {
                // 件数取得の場合
                sbSql.Append("COUNT(*)");
            }
            else
            {
                // 一覧データ取得の場合
                sbSql.Append("*");
            }
            sbSql.AppendLine(" FROM (");
            sbSql.AppendLine(selectSql);
            sbSql.AppendLine(") tbl");
            if (!string.IsNullOrEmpty(whereSql))
            {
                sbSql.AppendLine(whereSql);
            }
            sql = sbSql.ToString();
        }

        /// <summary>
        /// SQL実行用クラス
        /// </summary>
        public static class SqlExecuteClass
        {
            /// <summary>
            /// 実行SQL文取得
            /// </summary>
            /// <param name="sqlName">SQLファイル名(拡張子は含まない)</param>
            /// <param name="subDir">SQLファイル格納ディレクトリ</param>
            /// <param name="addText">SQL文末尾に追加する文言</param>
            /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
            /// <returns>SQL文</returns>
            public static string GetExecuteSql(string sqlName, string subDir, string addText, List<string> listUnComment = null)
            {
                // SQL取得
                TMQUtil.GetFixedSqlStatement(subDir, sqlName, out string sql, listUnComment);
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendLine(sql);
                sbSql.AppendLine(addText);
                return sbSql.ToString();
            }
            /// <summary>
            /// SQL実行し、結果エンティティを取得
            /// </summary>
            /// <typeparam name="T">取得結果のデータ型</typeparam>
            /// <param name="sqlName">SQLファイル名(拡張子は含まない)</param>
            /// <param name="subDir">SQLファイル格納ディレクトリ</param>
            /// <param name="param">引数のクラス</param>
            /// <param name="db">DB接続</param>
            /// <param name="addText">SQLの末尾に追加する内容 省略可能</param>
            /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
            /// <returns>取得した結果エンティティ</returns>
            public static T SelectEntity<T>(string sqlName, string subDir, object param, ComDB db, string addText = "", List<string> listUnComment = null)
            {
                // SQL取得
                string sqlText = GetExecuteSql(sqlName, subDir, addText, listUnComment);
                // 検索実行
                T result = db.GetEntityByDataClass<T>(sqlText, param);
                return result;
            }

            /// <summary>
            /// SQL実行し、結果リストを取得
            /// </summary>
            /// <typeparam name="T">取得結果のデータ型</typeparam>
            /// <param name="sqlName">SQLファイル名(拡張子は含まない)</param>
            /// <param name="subDir">SQLファイル格納ディレクトリ</param>
            /// <param name="param">引数のクラス</param>
            /// <param name="db">DB接続</param>
            /// <param name="addText">SQLの末尾に追加する内容 省略可能</param>
            /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
            /// <returns>取得した結果リスト</returns>
            public static List<T> SelectList<T>(string sqlName, string subDir, object param, ComDB db, string addText = "", List<string> listUnComment = null)
            {
                // SQL取得
                string sqlText = GetExecuteSql(sqlName, subDir, addText, listUnComment);

                // 検索実行
                IList<T> results = db.GetListByDataClass<T>(sqlText, param);
                if (results == null || results.Count == 0)
                {
                    return null;
                }
                return results.ToList();
            }

            /// <summary>
            /// SQL実行し、int値を取得
            /// </summary>
            /// <param name="sqlName">SQLファイル名(拡張子は含まない)</param>
            /// <param name="subDir">SQLファイル格納ディレクトリ</param>
            /// <param name="param">引数のクラス</param>
            /// <param name="db">DB接続</param>
            /// <param name="addText">SQLの末尾に追加する内容 省略可能</param>
            /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
            /// <returns>取得したint値</returns>
            public static int SelectIntValue(string sqlName, string subDir, object param, ComDB db, string addText = "", List<string> listUnComment = null)
            {
                // SQL取得
                string sqlText = GetExecuteSql(sqlName, subDir, addText, listUnComment);
                // 検索実行
                int result = db.GetCount(sqlText, param);
                return result;
            }

            /// <summary>
            /// SQLを実行(INSERT、UPDATE、REGIST)
            /// </summary>
            /// <typeparam name="T">取得結果のデータ型</typeparam>
            /// <param name="sqlName">SQLファイル名(拡張子は含まない)</param>
            /// <param name="subDir">SQLファイル格納ディレクトリ</param>
            /// <param name="param">引数のクラス</param>
            /// <param name="db">DB接続</param>
            /// <param name="addText">SQLの末尾に追加する内容 省略可能</param>
            /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
            /// <returns>実行成否</returns>
            public static bool Regist(string sqlName, string subDir, object param, ComDB db, string addText = "", List<string> listUnComment = null)
            {
                // SQL取得
                string sqlText = GetExecuteSql(sqlName, subDir, addText, listUnComment);
                // SQL実行
                int result = db.Regist(sqlText, param);
                return result > 0;
            }

            /// <summary>
            /// SQLを実行して戻り値を取得
            /// </summary>
            /// <typeparam name="T">戻り値の型</typeparam>
            /// <param name="returnKey">out 戻り値の値</param>
            /// <param name="sqlName">SQLファイル名(拡張子は含まない)</param>
            /// <param name="subDir">SQLファイル格納ディレクトリ</param>
            /// <param name="param">引数のクラス</param>
            /// <param name="db">DB接続</param>
            /// <param name="addText">SQLの末尾に追加する内容 省略可能</param>
            /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
            /// <returns>実行成否</returns>
            public static bool RegistAndGetKeyValue<T>(out T returnKey, string sqlName, string subDir, object param, ComDB db, string addText = "", List<string> listUnComment = null)
            {
                // SQL取得
                string sqlText = GetExecuteSql(sqlName, subDir, addText, listUnComment);
                // SQL実行
                returnKey = db.RegistAndGetKeyValue<T>(sqlText, out bool isError, param);
                return !isError;
            }
        }

        /// <summary>
        /// 構成IDより紐付く全ての階層を取得
        /// </summary>
        /// <param name="structureIdList">検索対象の構成IDリスト</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="languageId">言語ID</param>
        /// <returns>取得した構成リスト</returns>
        /// <remarks>工場個別の翻訳が設定されている場合、複数レコード取得されるため使用時に考慮すること</remarks>
        public static List<CommonSTDUtillDataClass.VStructureItemEntity> GetStructureItemList(List<int> structureIdList, ComDB db, string languageId)
        {
            return GetStructureItemList<CommonSTDUtillDataClass.VStructureItemEntity>(structureIdList, db, languageId);
        }

        /// <summary>
        /// 条件ツリーで選択された階層IDが単一の構成IDで最下層かどうかを判定する処理
        /// </summary>
        /// <param name="structureId">ツリーで選択された階層ID</param>
        /// <param name="db">DB接続</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="buttomId">out 取得した最下層の構成ID</param>
        /// <returns>最下層でない場合、False</returns>
        /// <remarks>下層の値が一つの場合、上層の構成IDをツリーは返す。紐づく下層の値の数を調べ、一つかどうか判定する</remarks>
        public static bool GetButtomValueFromTree(int structureId, ComDB db, string languageId, out int buttomId)
        {
            // outの初期値
            buttomId = -1;
            // 構成マスタの上下階層の値をすべて取得
            var list = GetStructureItemList<StructureLayerInfo.StructureGetInfo>(new List<int>() { structureId }, db, languageId);
            // 引数の階層IDの構成階層番号を取得
            int orgLayerNo = (list.Where(x => x.StructureId == structureId).Select(x => x.StructureLayerNo).First()) ?? -1;
            // 最大の構成階層番号を取得
            int maxLayerNo = list.Max(x => x.StructureLayerNo) ?? -1;

            // 引数の階層IDの構成階層番号から最大の階層番号まで順に階層を進め、それぞれ一つしか要素が無いかを判定する
            for (int layerNo = orgLayerNo; layerNo <= maxLayerNo; layerNo++)
            {
                // 構成階層番号で絞り込み
                var member = list.Where(x => x.StructureLayerNo == layerNo);

                if (member.Count() == 0)
                {
                    // 要素が無い場合、一つ上の階層が最下層となる
                    return true;
                }

                //翻訳により複数レコード存在する場合があるので重複削除
                var structureIdList = member.Select(x => x.StructureId).Distinct();

                if (structureIdList.Count() > 1)
                {
                    // 要素が複数ある場合は最下層でない
                    return false;
                }
                // 要素のIDが最下層の構成ID
                buttomId = member.First().StructureId;
            }

            return true;
        }

        /// <summary>
        /// 構成IDより紐付く全ての階層を取得(型指定)
        /// </summary>
        /// <typeparam name="T">指定する型</typeparam>
        /// <param name="structureIdList">検索対象の構成IDのリスト</param>
        /// <param name="db">DB接続</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="orderFactoryId">表示順を取得する工場ID、省略時は全工場共通の値</param>
        /// <returns>紐づく階層の値</returns>
        /// <remarks>工場個別の翻訳が設定されている場合、複数レコード取得されるため使用時に考慮すること</remarks>
        public static List<T> GetStructureItemList<T>(List<int> structureIdList, ComDB db, string languageId, int orderFactoryId = TMQConsts.CommonFactoryId)
        {
            //カンマ区切りの文字列にする
            string ids = string.Join(',', structureIdList);
            var param = new { StructureIdList = ids, LanguageId = languageId, FactoryId = orderFactoryId };
            return SqlExecuteClass.SelectList<T>(SqlName.GetStructureList, SqlName.SubDir, param, db);
        }

        /// <summary>
        /// 階層IDより上位の階層の情報を設定する処理のクラス
        /// </summary>
        public static class StructureLayerInfo
        {
            // データ構造に依存した処理を行うので、階層情報を追加する場合は定数群に追加するだけで問題ありません。
            // 追加対象
            // StructureType
            // FromPropertyName
            // データクラス(StructureLocationInfoなどを真似、IdとNameが末尾の名前)
            // StructureTypeValuesDic(上記定数の紐づけディクショナリ)

            /// <summary>
            /// 階層情報の種類
            /// </summary>
            public enum StructureType
            {
                // 地区
                Location,
                // 職種
                Job,
                // 原因性格
                FailureCause,
                // 予備品階層
                SpareLocation,
                // 地区(変更管理用)
                OldLocation,
                // 職種(変更管理用)
                OldJob,
            }

            /// <summary>
            /// 定数　データクラスの階層ID取得元のプロパティ名
            /// </summary>
            private static class FromPropertyName
            {
                /// <summary>
                /// 機能場所階層ID
                /// </summary>
                public const string LocationStructureId = "LocationStructureId";
                /// <summary>
                /// 職種機種階層ID
                /// </summary>
                public const string JobStructureId = "JobStructureId";
                /// <summary>
                /// 原因性格階層ID
                /// </summary>
                public const string FailureCausePersonalityStructureId = "FailureCausePersonalityStructureId";
                /// <summary>
                /// 予備品機能場所階層ID
                /// </summary>
                public const string SpareLocationStructureId = "PartsLocationId";
                /// <summary>
                /// 機能場所階層ID(変更管理用)
                /// </summary>
                public const string OldLocationStructureId = "OldLocationStructureId";
                /// <summary>
                /// 職種機種階層ID(変更管理用)
                /// </summary>
                public const string OldJobStructureId = "OldJobStructureId";
            }

            /// <summary>
            /// 機能場所階層IDを持つ場所階層の情報を保持するデータクラス
            /// </summary>
            public class StructureLocationInfoEx : StructureLocationInfo
            {
                /// <summary>Gets or sets 機能場所階層ID</summary>
                /// <value>機能場所階層ID</value>
                public int LocationStructureId { get; set; }

            }

            /// <summary>
            /// 職種機種階層IDを持つ職種階層の情報を保持するデータクラス
            /// </summary>
            public class StructureJobInfoEx : StructureJobInfo
            {
                /// <summary>Gets or sets 職種機種階層ID</summary>
                /// <value>職種機種階層ID</value>
                public int JobStructureId { get; set; }
                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int? FactoryId { get; set; }

            }

            /// <summary>
            /// 場所階層の情報を保持するデータクラス
            /// </summary>
            /// <remarks>このクラスを継承するか、同名のメンバを持つクラスが利用可能</remarks>
            public class StructureLocationInfo
            {
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
            }
            /// <summary>
            /// 職種階層の情報を保持するデータクラス
            /// </summary>
            /// <remarks>このクラスを継承するか、同名のメンバを持つクラスが利用可能</remarks>
            public class StructureJobInfo
            {
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
            }
            /// <summary>
            /// 原因性格階層の情報を保持するデータクラス
            /// </summary>
            /// <remarks>このクラスを継承するか、同名のメンバを持つクラスが利用可能</remarks>
            public class StructureFailureCauseInfo
            {
                /// <summary>Gets or sets 原因性格1</summary>
                /// <value>原因性格1</value>
                public int? FailureCausePersonality1StructureId { get; set; }
                /// <summary>Gets or sets 原因性格1</summary>
                /// <value>原因性格1</value>
                public string FailureCausePersonality1StructureName { get; set; }
                /// <summary>Gets or sets 原因性格2</summary>
                /// <value>原因性格2</value>
                public int? FailureCausePersonality2StructureId { get; set; }
                /// <summary>Gets or sets 原因性格2</summary>
                /// <value>原因性格2</value>
                public string FailureCausePersonality2StructureName { get; set; }

            }
            /// <summary>
            /// 予備品場所階層の情報を保持するデータクラス
            /// </summary>
            /// <remarks>このクラスを継承するか、同名のメンバを持つクラスが利用可能</remarks>
            public class SpareStructureLocationInfo
            {
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
                /// <summary>Gets or sets 倉庫ID</summary>
                /// <value>倉庫ID</value>
                public int? WarehouseId { get; set; }
                /// <summary>Gets or sets 倉庫名称</summary>
                /// <value>倉庫名称</value>
                public string WarehouseName { get; set; }
                /// <summary>Gets or sets 棚ID</summary>
                /// <value>棚ID</value>
                public int? RackId { get; set; }
                /// <summary>Gets or sets 棚名称</summary>
                /// <value>棚名称</value>
                public string RackName { get; set; }
            }
            /// <summary>
            /// 場所階層(変更管理用)の情報を保持するデータクラス
            /// </summary>
            /// <remarks>このクラスを継承するか、同名のメンバを持つクラスが利用可能</remarks>
            public class StructureOldLocationInfo
            {
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
            }
            /// <summary>
            /// 職種階層(変更管理用)の情報を保持するデータクラス
            /// </summary>
            /// <remarks>このクラスを継承するか、同名のメンバを持つクラスが利用可能</remarks>
            public class StructureOldJobInfo
            {
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
            }

            /// <summary>
            /// 階層情報の種類と定数の紐づけを行うディクショナリ
            /// </summary>
            private static Dictionary<StructureType, StructureValues> StructureTypeValuesDic = new Dictionary<StructureType, StructureValues>() {
                { StructureType.Location
                    ,new StructureValues( FromPropertyName.LocationStructureId
                        ,typeof(CommonTMQConstants.MsStructure.StructureLayerNo.Location)
                        ,new List<StructureGroupId>{StructureGroupId.Location }
                        ,getClassPropertyNames<StructureLocationInfo>()) },
                { StructureType.Job
                    ,new StructureValues( FromPropertyName.JobStructureId
                        ,typeof(CommonTMQConstants.MsStructure.StructureLayerNo.Job)
                        ,new List<StructureGroupId>{StructureGroupId.Job }
                        ,getClassPropertyNames<StructureJobInfo>()) },
                { StructureType.FailureCause
                    ,new StructureValues( FromPropertyName.FailureCausePersonalityStructureId
                        ,typeof(CommonTMQConstants.MsStructure.StructureLayerNo.FailureCause)
                        ,new List<StructureGroupId>{StructureGroupId.FailureCausePersonality }
                        ,getClassPropertyNames<StructureFailureCauseInfo>()) },
                { StructureType.SpareLocation
                    ,new StructureValues( FromPropertyName.SpareLocationStructureId
                        ,typeof(CommonTMQConstants.MsStructure.StructureLayerNo.SpareLocation)
                        ,new List<StructureGroupId>{StructureGroupId.SpareLocation , StructureGroupId.Location }
                        ,getClassPropertyNames<SpareStructureLocationInfo>()) },
                { StructureType.OldLocation
                    ,new StructureValues( FromPropertyName.OldLocationStructureId
                        ,typeof(CommonTMQConstants.MsStructure.StructureLayerNo.OldLocation)
                        ,new List<StructureGroupId>{StructureGroupId.Location }
                        ,getClassPropertyNames<StructureOldLocationInfo>()) },
                { StructureType.OldJob
                    ,new StructureValues( FromPropertyName.OldJobStructureId
                        ,typeof(CommonTMQConstants.MsStructure.StructureLayerNo.OldJob)
                        ,new List<StructureGroupId>{StructureGroupId.Job }
                        ,getClassPropertyNames<StructureOldJobInfo>()) }

            };
            /// <summary>
            /// この機能で使用する階層種類ごとに使用する値
            /// </summary>
            private class StructureValues
            {
                /// <summary>
                /// 階層ID取得元のプロパティの名称
                /// </summary>
                public string GetPropertyName;
                /// <summary>
                /// 構成階層番号のenumのType
                /// </summary>
                /// <remarks>typeof(CommonTMQConstants.MsStructure.StructureLayerNo.～)</remarks>
                public System.Type StructureLayerNoType;
                /// <summary>
                /// 構成グループID
                /// </summary>
                public List<StructureGroupId> GroupId;
                /// <summary>
                /// 取得した階層情報を設定するプロパティの名称
                /// </summary>
                public List<string> SetPropertyNames;

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="pGetPropertyName">FromPropertyNameに定義した値</param>
                /// <param name="pStructureLayerNoType">typeof(CommonTMQConstants.MsStructure.StructureLayerNo.～)</param>
                /// <param name="pGroupId">構成グループID、予備品の場合地区階層も使用するため複数、CommonTMQConstants.MsStructure.GroupId.～</param>
                /// <param name="pSetPropertyNames">getClassPropertyNames<設定するデータクラス>()</param>
                public StructureValues(string pGetPropertyName, Type pStructureLayerNoType, List<StructureGroupId> pGroupId, List<string> pSetPropertyNames)
                {
                    this.GetPropertyName = pGetPropertyName;
                    this.StructureLayerNoType = pStructureLayerNoType;
                    this.GroupId = pGroupId;
                    this.SetPropertyNames = pSetPropertyNames;
                }
            }

            /// <summary>
            /// SQL実行結果　階層情報取得
            /// </summary>
            public class StructureGetInfo
            {
                /// <summary>Gets or sets 構成ID</summary>
                /// <value>構成ID</value>
                public int StructureId { get; set; }
                /// <summary>Gets or sets 翻訳名称</summary>
                /// <value>翻訳名称</value>
                public string TranslationText { get; set; }
                /// <summary>Gets or sets 翻訳工場ID</summary>
                /// <value>翻訳工場ID</value>
                public int? LocationStructureId { get; set; }
                /// <summary>Gets or sets 構成グループID</summary>
                /// <value>構成グループID</value>
                public int? StructureGroupId { get; set; }
                /// <summary>Gets or sets 構成階層番号</summary>
                /// <value>構成階層番号</value>
                public int? StructureLayerNo { get; set; }
                /// <summary>Gets or sets 検索元構成ID</summary>
                /// <value>検索元構成ID</value>
                public int OrgStructureId { get; set; }
            }

            /// <summary>
            /// データクラスに場所および職種の階層情報を設定する処理
            /// </summary>
            /// <typeparam name="T">設定するデータクラスの型、このクラスで定義された型を継承するか、同じメンバを持つこと</typeparam>
            /// <param name="targetList">設定するデータクラスのリスト</param>
            /// <param name="typeList">処理対象の階層のリスト</param>
            /// <param name="db">DB接続</param>
            /// <param name="languageId">言語ID</param>
            /// <param name="treeViewFlg">ツリー選択ラベル用に設定する場合、true</param>
            /// <param name="transFactoryId">翻訳に使用する工場ID（未指定時はデータの工場または共通）</param>
            /// <remarks>設定するデータクラスは、FromPropertyNameのIDのメンバと定義されている型の階層のIDと名称をメンバに持つこと</remarks>
            public static void SetStructureLayerInfoToDataClass<T>(ref IList<T> targetList, List<StructureType> typeList, ComDB db, string languageId, bool treeViewFlg = false, int transFactoryId = 0)
            {
                // 階層ID取得元のプロパティの情報を取得
                PropertyInfo[] targetProps = typeof(T).GetProperties(); // 設定するデータクラスの情報を取得
                Dictionary<StructureType, PropertyInfo> propertyDic = new Dictionary<StructureType, PropertyInfo>(); // 処理対象の階層ごとにプロパティを管理するディクショナリ
                // 対象の階層を繰り返し、階層ごとにプロパティの情報を取得しディクショナリへ追加する
                typeList.ForEach(x => propertyDic.Add(x, getPropertyByType(targetProps, x)));

                // 設定先のリストに取得元プロパティが存在するか確認
                checkTarget();
                // チェックする処理
                // 値を設定するプロパティが存在するかも確認したいけど、階層の値によって異なるので難しい
                void checkTarget()
                {
                    foreach (StructureType type in typeList)
                    {
                        // 設定する場合、プロパティがNullならエラー
                        if (propertyDic[type] == null)
                        {
                            throw new Exception("SetStructureLayerInfoToDataClass : " + StructureTypeValuesDic[type].GetPropertyName + " is not found.");
                        }
                    }
                }

                // 構成マスタより検索するIDのリスト
                List<int> searchTargetStructureIdList = new();

                // 処理対象のデータクラスを繰り返し、IDを取得する
                foreach (var targetRow in targetList)
                {
                    typeList.ForEach(x => addIdList(ref searchTargetStructureIdList, propertyDic[x], targetRow));
                }
                // IDを取得してリストに追加する処理
                void addIdList(ref List<int> addList, PropertyInfo prop, T targetRow)
                {
                    var structureIds = prop.GetValue(targetRow);
                    if (structureIds != null)
                    {
                        //カンマ区切りのIDの場合、分割
                        string[] structureIdList = structureIds.ToString().Split(',');
                        addList.AddRange(structureIdList.Select(int.Parse).ToList());
                    }
                }

                // 検索するIDのリストが無い場合、終了
                if (searchTargetStructureIdList.Count == 0)
                {
                    return;
                }


                //カンマ区切りの文字列にする
                string ids = string.Join(',', searchTargetStructureIdList.Distinct().ToList());

                // IDのリストより上位の階層を検索し、階層情報のリストを取得
                var param = new { StructureIdList = ids, LanguageId = languageId };
                var structureInfoList = SqlExecuteClass.SelectList<StructureGetInfo>(SqlName.GetUpperStructureList, SqlName.SubDir, param, db);
                if (structureInfoList == null)
                {
                    return;
                }

                //工場IDのプロパティ
                var factoryProp = getPropertyByName(targetProps, "FactoryId");

                // 処理対象の階層で繰り返し、階層の値を設定する
                foreach (StructureType type in typeList)
                {
                    setStructureValue(type, ref targetList);
                }
                // 階層情報を設定する処理
                void setStructureValue(StructureType type, ref IList<T> targetList)
                {
                    // 取得する構成IDのプロパティ
                    PropertyInfo targetProp = propertyDic[type];

                    // プロパティ名称を取得(指定階層情報のIDと名称の列名をリストで取得する)
                    List<string> propNames = StructureTypeValuesDic[type].SetPropertyNames;
                    // 繰り返し処理を行う
                    foreach (var propName in propNames)
                    {
                        // プロパティ名称よりプロパティを取得
                        var prop = getPropertyByName(targetProps, propName);
                        if (prop == null)
                        {
                            continue;
                        }
                        // IDか名称で、IDの場合True
                        bool isId = propName.EndsWith("Id");

                        // 抽出に用いる構成階層番号の値、プロパティ名称よりIdとNameを除去した名称で取得する
                        System.Type targetEnum = StructureTypeValuesDic[type].StructureLayerNoType;
                        int structureLayerNo = (int)Enum.Parse(targetEnum, propName.Replace("Id", string.Empty).Replace("Name", string.Empty));
                        // 抽出に用いる構成グループIDの値のリスト
                        List<StructureGroupId> structureGroupId = StructureTypeValuesDic[type].GroupId;
                        List<int> compareStructureGroupIds = structureGroupId.Select(x => (int)x).ToList();

                        // 階層情報のリストをグループIDと構成階層番号で抽出し、繰り返しの前に件数を減らしておく
                        var narrowByProperty = structureInfoList.Where(x => compareStructureGroupIds.IndexOf(x.StructureGroupId ?? -1) >= 0 && x.StructureLayerNo == structureLayerNo);
                        if (narrowByProperty.Count() == 0)
                        {
                            // 件数が0件の場合、下位の階層なので取得対象外、次のプロパティへ
                            continue;
                        }

                        // 設定するリストで繰り返し処理を行う
                        foreach (var targetRow in targetList)
                        {
                            var value = targetProp.GetValue(targetRow);
                            if (value == null)
                            {
                                continue;
                            }

                            //対象行の工場IDを取得(翻訳の取得に使用)
                            var factoryId = transFactoryId;
                            if (factoryProp != null && transFactoryId == TMQConsts.CommonFactoryId)
                            {
                                var factoryIdStr = factoryProp.GetValue(targetRow);
                                if (factoryIdStr != null)
                                {
                                    factoryId = int.Parse(factoryIdStr.ToString());
                                }
                            }

                            //カンマ区切りのIDの場合、分割
                            List<string> structureIdList = value.ToString().Split(',').ToList();
                            List<int> idList = new();
                            List<string> textList = new();
                            foreach (string structureIdStr in structureIdList)
                            {
                                // 構成階層ID(地区or職種)
                                int structureId = int.Parse(structureIdStr.ToString());
                                // 取得した階層情報リストの検索元構成IDで絞り込み(工場個別翻訳のデータを取得)
                                StructureGetInfo narrowByStructureId = narrowByProperty.FirstOrDefault(x => x.OrgStructureId == structureId && x.LocationStructureId == factoryId);
                                if (narrowByStructureId == null)
                                {
                                    // 取得した階層情報リストの検索元構成IDで絞り込み(共通翻訳のデータを取得)
                                    narrowByStructureId = narrowByProperty.FirstOrDefault(x => x.OrgStructureId == structureId && x.LocationStructureId == TMQConsts.CommonFactoryId);
                                    if (narrowByStructureId == null)
                                    {
                                        continue;
                                    }
                                }
                                if (!isId && treeViewFlg)
                                {
                                    //ツリー選択ラベルの場合、[表示文字列]と[構成ID]を「|」区切りで設定
                                    narrowByStructureId.TranslationText = narrowByStructureId.TranslationText + "|" + narrowByStructureId.StructureId;
                                }
                                if (isId)
                                {
                                    //構成ID
                                    idList.Add(narrowByStructureId.StructureId);
                                }
                                else
                                {
                                    //表示文字列
                                    textList.Add(narrowByStructureId.TranslationText);
                                }
                            }
                            // IDor名称を設定
                            ComUtil.SetPropertyValue<T>(prop, targetRow, isId ? string.Join(',', idList.Distinct().ToList()) : string.Join(',', textList.Distinct().ToList()));
                        }
                    }
                }
            }

            /// <summary>
            /// 設定されている最下層の構成IDを取得して、データクラスの構成IDにセットする
            /// </summary>
            /// <typeparam name="T">設定するデータクラスの型</typeparam>
            /// <param name="targetList">設定するデータクラスのリスト</param>
            /// <param name="typeList">処理対象の階層のリスト</param>
            /// <param name="isExcelPort">ExcelPort読み込み時の場合true</param>
            public static void setBottomLayerStructureIdToDataClass<T>(ref IList<T> targetList, List<StructureType> typeList, bool isExcelPort = false)
            {
                // 階層ID取得元のプロパティの情報を取得
                PropertyInfo[] targetProps = typeof(T).GetProperties(); // 設定するデータクラスの情報を取得

                foreach (var type in typeList)
                {
                    //設定する構成IDの列名
                    string structureIdName = StructureTypeValuesDic[type].GetPropertyName;
                    // プロパティ名称よりプロパティを取得
                    var targetProp = getPropertyByName(targetProps, structureIdName);
                    if (targetProp == null)
                    {
                        continue;
                    }

                    // プロパティ名称を取得(場所階層または職種階層のIDと名称の列名をリストで取得する)
                    List<string> propNames = StructureTypeValuesDic[type].SetPropertyNames;

                    // 設定するリストで繰り返し処理を行う
                    foreach (var targetRow in targetList)
                    {
                        //退避用：最大の階層番号
                        int maxStructureLayerNo = -1;
                        // プロパティ毎の繰り返し
                        foreach (var propName in propNames)
                        {
                            // プロパティ名称よりプロパティを取得
                            var prop = getPropertyByName(targetProps, propName);
                            if (prop == null)
                            {
                                continue;
                            }
                            if (isExcelPort && propName.EndsWith("Name"))
                            {
                                //画面のツリー選択ラベルの場合はNameにIDが設定されているが、ExcelPortの場合はIDに設定されておりNameには翻訳が設定されているため除外
                                continue;
                            }

                            // 抽出に用いる構成階層番号の値、プロパティ名称よりIdとNameを除去した名称で取得する
                            System.Type targetEnum = StructureTypeValuesDic[type].StructureLayerNoType;
                            int structureLayerNo = (int)Enum.Parse(targetEnum, propName.Replace("Id", string.Empty).Replace("Name", string.Empty));
                            if (structureLayerNo < maxStructureLayerNo)
                            {
                                continue;
                            }
                            maxStructureLayerNo = structureLayerNo;

                            //構成ID取得
                            var value = prop.GetValue(targetRow);
                            if (value == null)
                            {
                                continue;
                            }
                            //最下層の構成IDを設定
                            ComUtil.SetPropertyValue<T>(targetProp, targetRow, value);
                        }
                    }
                }
            }

            /// <summary>
            /// 指定されたクラスのプロパティ名一覧を取得
            /// </summary>
            /// <typeparam name="T">プロパティ名を取得したいクラス</typeparam>
            /// <returns>プロパティ名のリスト</returns>
            private static List<string> getClassPropertyNames<T>()
            {
                List<string> results = new();
                // プロパティリストを取得
                PropertyInfo[] propInfos = typeof(T).GetProperties();
                foreach (var prop in propInfos)
                {
                    // 繰り返し名称を取得
                    results.Add(prop.Name);
                }
                return results;
            }
            /// <summary>
            /// プロパティリストからプロパティ名称に一致するプロパティを取得
            /// </summary>
            /// <param name="propInfos">プロパティリスト</param>
            /// <param name="propName">取得するプロパティの名称(大文字同士で比較)</param>
            /// <returns>名称で絞り込んで取得したプロパティ</returns>
            private static PropertyInfo getPropertyByName(PropertyInfo[] propInfos, string propName)
            {
                // 名称を大文字同士で比較して合致するプロパティを取得
                return propInfos.FirstOrDefault(x => x.Name.ToUpper().Equals(propName.ToUpper()));
            }

            /// <summary>
            /// プロパティリストから階層種別に一致するプロパティを取得
            /// </summary>
            /// <param name="propInfos">プロパティリスト</param>
            /// <param name="type">このクラスで定義した階層の種類</param>
            /// <returns>名称で絞り込んで取得したプロパティ</returns>
            private static PropertyInfo getPropertyByType(PropertyInfo[] propInfos, StructureType type)
            {
                string name = StructureTypeValuesDic[type].GetPropertyName;
                return getPropertyByName(propInfos, name);
            }
        }

        /// <summary>
        /// 構成アイテム、アイテムマスタ拡張情報を処理するクラス
        /// </summary>
        public static class StructureItemEx
        {
            /// <summary>
            /// 構成アイテム、アイテムマスタ拡張情報
            /// </summary>
            public class StructureItemExInfo : CommonSTDUtillDataClass.VStructureItemEntity
            {

                /// <summary>Gets or sets アイテムID</summary>
                /// <value>アイテムID</value>
                public int? ItemId { get; set; }
                /// <summary>Gets or sets 連番</summary>
                /// <value>連番</value>
                public int? Seq { get; set; }
                /// <summary>Gets or sets 拡張データ</summary>
                /// <value>拡張データ</value>
                public string ExData { get; set; }
                /// <summary>Gets or sets 説明</summary>
                /// <value>説明</value>
                public string Note { get; set; }
            }

            /// <summary>
            /// アイテムマスタ拡張に定義されている情報から構成アイテムを取得する
            /// </summary>
            /// <param name="param">パラメータ（構成グループIDの指定は必須）</param>
            /// <param name="db">DB接続</param>
            /// <param name="orderFactoryId">表示順を取得する工場ID、省略時は全工場共通の値</param>
            /// <param name="isGetDeleted">削除を含む場合True、省略時はFalse(含まない)</param>
            /// <returns>拡張の値から取得した構成アイテム</returns>
            public static List<StructureItemExInfo> GetStructureItemExData(StructureItemExInfo param, ComDB db, int orderFactoryId = TMQConsts.CommonFactoryId, bool isGetDeleted = false)
            {

                // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
                List<string> listUnComment = ComUtil.GetNotNullNameByClass<StructureItemExInfo>(param);
                if (!isGetDeleted)
                {
                    // 削除を含まない場合、条件に削除フラグを追加
                    listUnComment.Add("NotDeleteOnly");
                }
                //SQL取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetStructureItemExDataList, out string executeSql, listUnComment);
                // 引数の表示順取得用工場IDをパラメータにセット、セット前の値は退避
                int? orgFactoryId = param.FactoryId;
                param.FactoryId = orderFactoryId;
                //SQL実行
                IList<StructureItemExInfo> results = db.GetListByDataClass<StructureItemExInfo>(executeSql, param);
                // 退避した値を戻す
                param.FactoryId = orgFactoryId;
                return results.ToList();
            }
        }

        /// <summary>
        /// スケジュールの変換
        /// </summary>
        public class ScheduleListConverter
        {
            /// <summary>
            /// 変換処理実行
            /// </summary>
            /// <param name="dataList">処理対象値リスト(年月で絞り込み済)</param>
            /// <param name="condition">スケジュールの表示条件</param>
            /// <param name="monthStartNendo">年度の開始月</param>
            /// <param name="isLink">マークからのリンク有無</param>
            /// <param name="db">DB接続</param>
            /// <param name="linkInfo">マークの種類ごとのリンク先の情報　リンク無の場合は省略可、リンク有の場合は必須</param>
            /// <returns>変換した画面表示用の一覧</returns>
            public List<Dao.ScheduleList.Display> Execute(IList<Dao.ScheduleList.Get> dataList, Dao.ScheduleList.GetCondition condition, int monthStartNendo
                , bool isLink, ComDB db, Dictionary<ScheduleStatus, Dao.ScheduleList.Display.LinkTargetInfo> linkInfo = null, bool isNotContinue = false)
            {
                // 年単位の場合はリンク無し
                bool setIsLink = condition.IsYear() ? false : isLink;

                // 表示単位
                ScheduleDisplayUnit unit = condition.DisplayUnit;

                // 構成マスタをグループIDで検索してスケジュールステータス(1860)の優先順位を取得(アイテムの表示順の昇順で並び変え)
                IList<int> priorityList = GetStructureCommonExDataOrdererdItemOrder<int>(Const.MsStructure.GroupId.StatusList, db);

                // 戻り値　画面表示用データリスト セル単位にデータを管理
                List<Dao.ScheduleList.Display> displayList = new();

                // 複数のマークでまとめて一つとして扱う場合、処理を行ったキーと日付を退避する
                ProcessedSameMarks SameMark = new();

                // 処理対象値をキーID、日付でループして処理を行う

                // キーIDのリスト
                List<string> keyIdList = dataList.Select(x => x.KeyId).Distinct().ToList();
                // キーIDで繰り返し(行)
                foreach (string keyId in keyIdList)
                {
                    // キーIDで選択したリストを日付と点検種別(小さいほど優先)の順番でソート
                    var dataListByKeyId = dataList.Where(x => x.KeyId == keyId).OrderBy(x => x.ScheduleDate).ThenBy(x => x.MaintainanceKindLevel).ToList();
                    // 処理実施済日付
                    List<DateTime> processedDateList = new();
                    // 表示対象の日付単位でステータスを管理するディクショナリ
                    var dicStatus = new Dictionary<DateTime, ScheduleStatus>();

                    foreach (var schedule in dataListByKeyId)
                    {
                        if (processedDateList.Contains(schedule.ScheduleDate))
                        {
                            // 途中で処理を終了するかどうか
                            // 長期スケジュール表(RP0090)と年度スケジュール表(RP0100)は途中で処理を終了しない
                            if (!isNotContinue)
                            {
                                // この日付が既に処理済みなら終了
                                continue;
                            }
                        }
                        // 処理対象年月日を単位に合わせて集計
                        DateTime keyDate = convertDateByUnit(unit, schedule.ScheduleDate, monthStartNendo);

                        // 同一のスケジュールマークとして扱うかどうか判定
                        if (isExecuteSameMark() && SameMark.IsExists(schedule.SameMarkKey, keyDate))
                        {
                            // 途中で処理を終了するかどうか
                            // 長期スケジュール表(RP0090)と年度スケジュール表(RP0100)は途中で処理を終了しない
                            if (!isNotContinue)
                            {
                                // isExecuteSameMarkにより、点検種別ごとに表示しないクラスでは判定を行わないため、必ず扱わないようになる
                                // 扱う場合、終了
                                continue;
                            }
                        }

                        // 年月日で抽出
                        var dataListByKeyIdDate = dataListByKeyId.Where(x => isEqualDateDisplayUnit(unit, x.ScheduleDate, keyDate, monthStartNendo)).ToList();
                        // 処理対象レコードのスケジュール日付を処理実施日付に追加
                        processedDateList.AddRange(dataListByKeyIdDate.Select(x => x.ScheduleDate));
                        // この処理対象年月日のステータスを取得
                        // dataListをgroupKeyで絞り込んで引数に渡す
                        var groupKey = schedule.GroupKey;
                        var groupDataList = getGroupDataList(dataList, keyId, groupKey, unit, keyDate, monthStartNendo);
                        var status = CheckSchedule(dataListByKeyIdDate, priorityList, groupDataList);
                        int statusPriority = priorityList.First(x => x == (int)status); // ステータスの優先度
                        // 追加 (点検種別でソートされているので先頭を渡す)
                        Dao.ScheduleList.Display cellInfo = makeDisplayInfo(keyId, keyDate, status, dataListByKeyIdDate.First(), setIsLink, linkInfo, statusPriority);
                        cellInfo.GroupKey = groupKey; // リンク表示用に設定
                        displayList.Add(cellInfo);

                        if (isExecuteSameMark())
                        {
                            // 処理済みのスケジュールマークと日付を登録
                            SameMark.AddProcessed(schedule.SameMarkKey, keyDate);
                        }
                    }
                }

                // △▲のリンク表示用
                setUpperLink(ref displayList);

                return displayList;

                // △と▲のリンクを上位のものと同じ内容に変更する
                void setUpperLink(ref List<Dao.ScheduleList.Display> displayList)
                {
                    var upperList = displayList
                        .Where(x => x.StatusId == ScheduleStatus.UpperComplete || x.StatusId == ScheduleStatus.UpperScheduled)
                        .Where(x => x.LinkInfo != null).ToList();
                    foreach (var upper in upperList)
                    {
                        // 自身の上位を取得
                        // グループキーが同一でキーIDが異なり、年月が同じ
                        var upperTarget = displayList.Where(x => x.GroupKey == upper.GroupKey && x.KeyId != upper.KeyId && x.KeyDate == upper.KeyDate).First();
                        upper.LinkInfo = upperTarget.LinkInfo;
                        upper.SummaryId = upperTarget.SummaryId;
                    }
                }
            }

            /// <summary>
            /// 予算帳票用変換処理実行
            /// </summary>
            /// <param name="dataList">処理対象値リスト(年月で絞り込み済)</param>
            /// <param name="condition">スケジュールの表示条件</param>
            /// <param name="monthStartNendo">年度の開始月</param>
            /// <param name="isLink">マークからのリンク有無</param>
            /// <param name="db">DB接続</param>
            /// <param name="linkInfo">マークの種類ごとのリンク先の情報　リンク無の場合は省略可、リンク有の場合は必須</param>
            /// <param name="halfPeriodFlag">半期フラグ</param>
            /// <returns>変換した画面表示用の一覧</returns>
            public List<Dao.ScheduleList.Display> ExecuteForBudgetReport(IList<Dao.ScheduleList.Get> dataList, Dao.ScheduleList.GetCondition condition, int monthStartNendo
                , bool isLink, ComDB db, Dictionary<ScheduleStatus, Dao.ScheduleList.Display.LinkTargetInfo> linkInfo = null, bool halfPeriodFlag = false)
            {
                // 表示単位
                ScheduleDisplayUnit unit = condition.DisplayUnit;

                // 戻り値　画面表示用データリスト セル単位にデータを管理
                List<Dao.ScheduleList.Display> displayList = new();

                //***************************************************
                // 処理対象値をキーID、日付でループして処理を行う
                //***************************************************
                // キーIDのリストを取得
                List<string> keyIdList = dataList.Select(x => x.KeyId).Distinct().ToList();
                // キーIDループ
                foreach (string keyId in keyIdList)
                {
                    // キーIDで選択したリストを日付の順番でソート
                    var dataListByKeyId = dataList.Where(x => x.KeyId == keyId).OrderBy(x => x.ScheduleDate).ToList();
                    // キーIDで選択したデータリスト分ループ
                    foreach (var schedule in dataListByKeyId)
                    {
                        // 処理対象年月日を単位に合わせて集計
                        DateTime keyDate = convertDateByUnit(unit, schedule.ScheduleDate, monthStartNendo, halfPeriodFlag);
                        // 画面表示用データリストにデータがある場合
                        if (displayList.Count > 0)
                        {
                            // キーIDと対象年月日が同じ場合
                            if (displayList[displayList.Count - 1].KeyId == keyId && displayList[displayList.Count - 1].KeyDate == keyDate)
                            {
                                // 予算金額と実績金額を加算し、次のデータへ
                                if (schedule.BudgetAmount != null)
                                {
                                    displayList[displayList.Count - 1].BudgetAmount = displayList[displayList.Count - 1].BudgetAmount.GetValueOrDefault() + schedule.BudgetAmount;
                                }
                                if (schedule.Expenditure != null)
                                {
                                    displayList[displayList.Count - 1].Expenditure = displayList[displayList.Count - 1].Expenditure.GetValueOrDefault() + schedule.Expenditure;
                                }
                                continue;
                            }
                        }
                        if (schedule.BudgetAmount != null)
                        {
                            schedule.BudgetAmount = schedule.BudgetAmount;
                        }
                        if (schedule.Expenditure != null)
                        {
                            schedule.Expenditure = schedule.Expenditure;
                        }
                        // 表示用セル情報を作成
                        Dao.ScheduleList.Display cellInfo = makeDisplayInfo(keyId, keyDate, ScheduleStatus.NoSchedule, schedule, false, null, (int)ScheduleStatus.NoSchedule);
                        // 表示用データリストに追加
                        displayList.Add(cellInfo);
                    }
                }
                // 表示用データリストを返却する
                return displayList;
            }

            /// <summary>
            /// 同一のスケジュールマークとして扱う判定に使用するクラス
            /// </summary>
            private class ProcessedSameMarks
            {
                /// <summary>Gets or sets 処理済みリスト</summary>
                /// <value>処理済みリスト</value>
                /// <remarks>値はSameMarkKeyとScheduleDateのみを持つ</remarks>
                List<Dao.ScheduleList.Get> processedList { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                public ProcessedSameMarks()
                {
                    this.processedList = new();
                }

                /// <summary>
                /// 処理済みリストに指定された値が存在するか確認
                /// </summary>
                /// <param name="key">SameMarkKey</param>
                /// <param name="scheduleDate">日付を表示単位で集計した値</param>
                /// <returns>存在する場合True</returns>
                public bool IsExists(string key, DateTime scheduleDate)
                {
                    bool result = this.processedList.Any(x => x.SameMarkKey == key && x.ScheduleDate == scheduleDate);
                    return result;
                }
                /// <summary>
                /// 処理済みリストに指定された値を追加する処理
                /// </summary>
                /// <param name="key">SameMarkKey</param>
                /// <param name="scheduleDate">日付を表示単位で集計した値</param>
                public void AddProcessed(string key, DateTime scheduleDate)
                {
                    Dao.ScheduleList.Get add = new();
                    add.SameMarkKey = key;
                    add.ScheduleDate = scheduleDate;
                    this.processedList.Add(add);
                }
            }

            /// <summary>
            /// スケジュールの同一マークの処理を行う場合True
            /// </summary>
            /// <returns>行う場合True</returns>
            /// <remarks>行わないクラスではFalseでオーバーライドさせて処理の分岐に用いる</remarks>
            protected virtual bool isExecuteSameMark()
            {
                return true;
            }

            /// <summary>
            /// 画面表示用情報を設定
            /// </summary>
            /// <param name="keyId">キーID</param>
            /// <param name="keyDate">表示する日付</param>
            /// <param name="status">表示するステータス</param>
            /// <param name="firstData">表示対象の最初のデータ</param>
            /// <param name="setIsLink">リンク有の場合True</param>
            /// <param name="linkInfo">リンク先の画面の情報</param>
            /// <param name="statusPriority">表示するステータスの優先度</param>
            /// <returns>画面表示用情報</returns>
            protected Dao.ScheduleList.Display makeDisplayInfo(string keyId, DateTime keyDate, ScheduleStatus status, Dao.ScheduleList.Get firstData
                , bool setIsLink, Dictionary<ScheduleStatus, Dao.ScheduleList.Display.LinkTargetInfo> linkInfo, int statusPriority)
            {
                Dao.ScheduleList.Display cellInfo = new();
                cellInfo.KeyId = keyId;
                cellInfo.KeyDate = keyDate;
                cellInfo.StatusId = status;
                cellInfo.StatusPriority = statusPriority;
                cellInfo.MaintainanceKindChar = firstData.MaintainanceKindChar;
                cellInfo.MaintainanceKindLevel = firstData.MaintainanceKindLevel;
                cellInfo.IsLink = setIsLink;
                // 帳票用に予算と実績を設定
                cellInfo.BudgetAmount = firstData.BudgetAmount;
                cellInfo.Expenditure = firstData.Expenditure;
                if (setIsLink && linkInfo.ContainsKey(cellInfo.StatusId))
                {
                    // リンク有の場合
                    // 対象ステータスでリンクがある場合のみリンク(遷移先がある場合のみ)
                    cellInfo.LinkInfo = linkInfo[cellInfo.StatusId];
                    if (cellInfo.StatusId == ScheduleStatus.NoCreate)
                    {
                        // ○の場合は保全活動IDでなく新規登録で必要なキー(スケジュール詳細)を設定
                        cellInfo.SummaryId = firstData.NewMaintainanceKey;
                    }
                    else if (firstData.SummaryId != null)
                    {
                        cellInfo.SummaryId = firstData.SummaryId;
                    }

                }
                return cellInfo;
            }

            /// <summary>
            /// 表示単位のキー年月日とレコードの年月日を表示単位に応じて比較
            /// </summary>
            /// <param name="unit">年月表示単位</param>
            /// <param name="target">比較対象の集計する日付</param>
            /// <param name="keyDate">キーとなる日付</param>
            /// <param name="monthStartNendo">年度開始月</param>
            /// <param name="halfPeriodFlag">半期フラグ</param>
            /// <returns></returns>
            private bool isEqualDateDisplayUnit(ScheduleDisplayUnit unit, DateTime target, DateTime keyDate, int monthStartNendo, bool halfPeriodFlag = false)
            {
                // 比較用の値を単位に応じて集計
                DateTime compareTarget = convertDateByUnit(unit, target, monthStartNendo, halfPeriodFlag);
                // 比較
                bool result = DateTime.Compare(compareTarget, keyDate) == 0;
                return result;
            }

            /// <summary>
            /// 年月日を表示単位に合わせて変換する
            /// </summary>
            /// <param name="unit">年月表示単位</param>
            /// <param name="target">対象の日付</param>
            /// <param name="monthStartNendo">年度開始月</param>
            /// <param name="halfPeriodFlag">半期フラグ</param>
            /// <returns>集計した日付</returns>
            private DateTime convertDateByUnit(ScheduleDisplayUnit unit, DateTime target, int monthStartNendo, bool halfPeriodFlag = false)
            {
                DateTime returnValue;
                if (unit == ScheduleDisplayUnit.Month)
                {
                    // 年月単位→その年月の1日
                    returnValue = new(target.Year, target.Month, 1);
                }
                else
                {
                    // 年度単位→その日付の年度開始日
                    returnValue = ComUtil.GetNendoStartDay(target, monthStartNendo);
                    // 半期フラグが true の場合
                    if (halfPeriodFlag == true)
                    {
                        // 半期単位→その日付の半期開始日
                        returnValue = ComUtil.GetNendoHalfPeriodStartDay(target, monthStartNendo);
                    }
                }
                return returnValue;
            }

            /// <summary>
            /// 上位ランクを参照するステータスの処理のために、自身以外の同グループ(異なる点検種別を取りうる)メンバーを取得する処理
            /// </summary>
            /// <param name="dataList">全データ</param>
            /// <param name="keyId">処理対象のデータのキーID</param>
            /// <param name="groupKey">処理対象のデータのグループキー</param>
            /// <param name="unit">年月表示単位</param>
            /// <param name="keyDate">処理対象のデータの日付</param>
            /// <param name="monthStartNendo">年度開始月</param>
            /// <param name="halfPeriodFlag">半期フラグ</param>
            /// <returns>同グループのデータの内容</returns>
            protected virtual List<Dao.ScheduleList.Get> getGroupDataList(IList<Dao.ScheduleList.Get> dataList, string keyId, long? groupKey, ScheduleDisplayUnit unit, DateTime keyDate, int monthStartNendo, bool halfPeriodFlag = false)
            {
                return dataList.Where(x => x.GroupKey == groupKey && x.KeyId != keyId && isEqualDateDisplayUnit(unit, x.ScheduleDate, keyDate, monthStartNendo, halfPeriodFlag)).ToList();
            }

            /// <summary>
            /// 対応するステータスを取得する処理
            /// </summary>
            /// <param name="targetList">キーIDと対象年月で絞り込んだ(一覧上の1セル)データ</param>
            /// <param name="priorityList">ステータスのリスト、優先順位でソート済み</param>
            /// <param name="groupDataList">上位ランクの集計用データ　グループキーが同じでキーが異なるデータ</param>
            /// <returns>対応するステータスの値</returns>
            private ScheduleStatus CheckSchedule(List<Dao.ScheduleList.Get> targetList, IList<int> priorityList, List<Dao.ScheduleList.Get> groupDataList)
            {
                // 対象データとグループデータをマージした全データリストを作成
                List<Dao.ScheduleList.Get> allList = new();
                allList.AddRange(targetList);
                if (groupDataList != null && groupDataList.Count > 0)
                {
                    allList.AddRange(groupDataList);
                }

                // 最小レベル取得
                int minLevel = GetMinMaintKindLevel(allList);

                // ステータスのリストは優先順位の高い順に並んでいるので、最初(高い順)からそれぞれ条件を満たすかチェックする
                foreach (var status in priorityList)
                {
                    switch (status)
                    {
                        case (int)ScheduleStatus.Complete:
                            if (IsComplete(allList, minLevel))
                            {
                                return ScheduleStatus.Complete;
                            }
                            break;
                        case (int)ScheduleStatus.UpperComplete:
                            if (IsUpperComplete(targetList, minLevel, groupDataList))
                            {
                                return ScheduleStatus.UpperComplete;
                            }
                            break;
                        case (int)ScheduleStatus.Created:
                            if (IsCreated(allList, minLevel))
                            {
                                return ScheduleStatus.Created;
                            }
                            break;
                        case (int)ScheduleStatus.NoCreate:
                            if (IsNoCreate(allList, minLevel, targetList))
                            {
                                return ScheduleStatus.NoCreate;
                            }
                            break;
                        case (int)ScheduleStatus.UpperScheduled:
                            if (IsUpperScheduled(targetList, minLevel, groupDataList))
                            {
                                return ScheduleStatus.UpperScheduled;
                            }
                            break;
                        default:
                            // 到達不能
                            return ScheduleStatus.NoSchedule;
                    }
                }
                // 到達不能
                return ScheduleStatus.NoSchedule;
            }

            /// <summary>
            /// リストより最上位の点検種別順序を取得
            /// </summary>
            /// <param name="targets">取得対象のリスト</param>
            /// <returns>最上位の点検種別順序</returns>
            /// <remarks>値が小さいほど優先順位が高い</remarks>
            protected int GetMinMaintKindLevel(List<Dao.ScheduleList.Get> targets)
            {
                // 最小の点検種別順序の値を取得
                int minLevel = (targets.Where(x => x != null).Select(x => x.MaintainanceKindLevel).Min()) ?? -1;
                return minLevel;
            }

            /// <summary>
            /// スケジュール済みを判定(保全活動未作成、○)
            /// </summary>
            /// <param name="allData">キーIDと対象年月で絞り込んだ(一覧上の1セル)データ(他レベル点検種別を含む)</param>
            /// <param name="minLevel">上記データの最上位の点検種別のソート順(優先順位)</param>
            /// <param name="self">キーIDと対象年月で絞り込んだ(一覧上の1セル)データ(他レベル点検種別を含まない)</param>
            /// <returns>この区分に該当するならTrue</returns>
            protected virtual bool IsNoCreate(List<Dao.ScheduleList.Get> allData, int minLevel, List<Dao.ScheduleList.Get> self)
            {
                // 自身が最上位の点検種別で、かつ
                // いずれかが保全活動未作成の場合、○

                // 他レベルを含まない最小の点検種別順序を取得
                var selfMinLevel = GetMinMaintKindLevel(self);
                if (selfMinLevel != minLevel)
                {
                    // 最上位の点検種別順序と異なる場合、最上位でないので「スケジュール済み」でない
                    return false;
                }
                // 最上位の場合
                // 保全活動未作成の要素の有無を取得
                var result = allData.Any(x => !x.IsMakedSummary());

                // 要素がある場合、「スケジュール済み」
                return result;
            }

            /// <summary>
            /// 上位ランクがスケジュール済みを判定(上位ランクがスケジュール済み、△)
            /// </summary>
            /// <param name="self">キーIDと対象年月で絞り込んだ(一覧上の1セル)データ(他レベル点検種別を含まない)</param>
            /// <param name="minLevel">上記データの最上位の点検種別のソート順(優先順位)</param>
            /// <param name="groupDataList">上位ランクの集計用データ　グループキーが同じでキーが異なるデータ</param>
            /// <returns>この区分に該当するならTrue</returns>
            protected virtual bool IsUpperScheduled(List<Dao.ScheduleList.Get> self, int minLevel, List<Dao.ScheduleList.Get> groupDataList)
            {
                // 点検種別が最上位のものがいずれかが未完了
                // ※自身が最上位の場合はFalse

                // 他レベルを含まない最小の点検種別順序を取得
                var selfMinLevel = GetMinMaintKindLevel(self);
                if (selfMinLevel == minLevel)
                {
                    // 最上位の点検種別順序である場合、「上位ランクがスケジュール済み」でない
                    return false;
                }

                // 最上位を取得
                var topData = groupDataList.Where(x => x.MaintainanceKindLevel == minLevel); // 最上位の点検種別で絞り込み
                if (topData.Count() == 0)
                {
                    // 絞り込んだ結果存在しない場合、False
                    return false;
                }
                var result = topData.Any(x => !x.IsComplete()); // 未完了の要素があるか取得
                // 要素がある場合、「上位ランク完了済み」
                return result;
            }

            /// <summary>
            /// 上位ランクが履歴完了済みを判定(上位ランクが完了済み、▲)
            /// </summary>
            /// <param name="self">キーIDと対象年月で絞り込んだ(一覧上の1セル)データ(他レベル点検種別を含まない)</param>
            /// <param name="minLevel">上記データの最上位の点検種別のソート順(優先順位)</param>
            /// <param name="groupDataList">上位ランクの集計用データ　グループキーが同じでキーが異なるデータ</param>
            /// <returns>この区分に該当するならTrue</returns>
            protected virtual bool IsUpperComplete(List<Dao.ScheduleList.Get> self, int minLevel, List<Dao.ScheduleList.Get> groupDataList)
            {
                // 点検種別が最上位のものが全て完了済み
                // ※自身が最上位の場合はFalse

                // 他レベルを含まない最小の点検種別順序を取得
                var selfMinLevel = GetMinMaintKindLevel(self);
                if (selfMinLevel == minLevel)
                {
                    // 最上位の点検種別順序である場合、「上位ランクが完了済み」でない
                    return false;
                }

                // 最上位を取得
                var topData = groupDataList.Where(x => x.MaintainanceKindLevel == minLevel); // 最上位の点検種別で絞り込み
                if (topData.Count() == 0)
                {
                    // 絞り込んだ結果存在しない場合、False
                    return false;
                }
                var result = topData.Any(x => !x.IsComplete()); // 未完了の要素があるか確認
                // 要素がある場合、「上位ランク完了済み」でないため反転
                return !result;
            }

            /// <summary>
            /// 計画作成済みを判定(保全活動作成済みで未完了、◎)
            /// </summary>
            /// <param name="allData">キーIDと対象年月で絞り込んだ(一覧上の1セル)データ(他レベル点検種別を含む)</param>
            /// <param name="minLevel">上記データの最上位の点検種別のソート順(優先順位)</param>
            /// <returns>この区分に該当するならTrue</returns>
            protected virtual bool IsCreated(List<Dao.ScheduleList.Get> allData, int minLevel)
            {
                // 全てが保全活動作成済みだが、いずれかが未完了の場合、◎

                // 全てが保全活動作成済みか判定
                // 保全活動未作成の要素の有無を取得
                var result = allData.Any(x => !x.IsMakedSummary());
                if (result)
                {
                    // 保全活動未作成の要素がある場合、全てが保全活動作成済みでないため、◎でない
                    return false;
                }
                // いずれかが未完了か判定
                // 未完了の要素の有無を取得
                result = allData.Any(x => !x.IsComplete());
                // 未完了の要素がある場合、◎
                return result;
            }

            /// <summary>
            /// 保全履歴完了を判定(完了済み、●)
            /// </summary>
            /// <param name="target">キーIDと対象年月で絞り込んだ(一覧上の1セル)データ(他レベル点検種別を含む)</param>
            /// <param name="minLevel">上記データの最上位の点検種別のソート順(優先順位)</param>
            /// <returns>この区分に該当するならTrue</returns>
            protected virtual bool IsComplete(List<Dao.ScheduleList.Get> target, int minLevel)
            {
                // すべてが完了済みの場合

                // 未完了のものがあるか取得
                var result = target.Any(x => !x.IsComplete());
                // 未完了がある場合、すべて完了済みでないため、反転
                return !result;
            }
        }

        /// <summary>
        /// 上位ランクの場合の処理を行わないスケジュール変換クラス
        /// </summary>
        /// <remarks>点検種別ごとに表示されない場合はこちらを使用</remarks>
        public class ScheduleListConverterNoRank : ScheduleListConverter
        {
            /// <summary>
            /// 上位ランクを参照する判定のためのデータを取得する処理、不要なので空データを返すよう上書き
            /// </summary>
            /// <param name="dataList">全データ</param>
            /// <param name="keyId">処理対象のデータのキーID</param>
            /// <param name="groupKey">処理対象のデータのグループキー</param>
            /// <param name="unit">年月表示単位</param>
            /// <param name="keyDate">処理対象のデータの日付</param>
            /// <param name="monthStartNendo">年度開始月</param>
            /// <param name="halfPeriodFlag">半期フラグ</param>
            /// <returns>不要なのでNull</returns>
            protected override List<Dao.ScheduleList.Get> getGroupDataList(IList<Dao.ScheduleList.Get> dataList, string keyId, long? groupKey, ScheduleDisplayUnit unit, DateTime keyDate, int monthStartNendo, bool halfPeriodFlag = false)
            {
                // 上位ランクの判定を行わないため、取得不要
                return null;
            }

            /// <summary>
            /// 上位ランクが履歴完了済みを判定(上位ランクが完了済み、▲)
            /// </summary>
            /// <param name="target">キーIDと対象年月で絞り込んだ(一覧上の1セル)データ(他レベル点検種別を含む)</param>
            /// <param name="minLevel">上記データの最上位の点検種別のソート順(優先順位)</param>
            /// <param name="groupDataList">上位ランクの集計用データ　グループキーが同じでキーが異なるデータ</param>
            /// <returns>存在しないので常にFalse</returns>
            protected override bool IsUpperComplete(List<Dao.ScheduleList.Get> target, int minLevel, List<Dao.ScheduleList.Get> groupDataList)
            {
                return false;
            }

            /// <summary>
            /// 上位ランクがスケジュール済みを判定(上位ランクがスケジュール済み、△)
            /// </summary>
            /// <param name="target">キーIDと対象年月で絞り込んだ(一覧上の1セル)データ(他レベル点検種別を含む)</param>
            /// <param name="minLevel">上記データの最上位の点検種別のソート順(優先順位)</param>
            /// <param name="groupDataList">上位ランクの集計用データ　グループキーが同じでキーが異なるデータ</param>
            /// <returns>存在しないので常にFalse</returns>
            protected override bool IsUpperScheduled(List<Dao.ScheduleList.Get> target, int minLevel, List<Dao.ScheduleList.Get> groupDataList)
            {
                return false;
            }

            /// <summary>
            /// スケジュールの同一マークの処理を行う場合True
            /// </summary>
            /// <returns>行う場合True</returns>
            /// <remarks>行わないクラスではFalseでオーバーライドさせて処理の分岐に用いる</remarks>
            protected override bool isExecuteSameMark()
            {
                return false;
            }
        }

        /// <summary>
        /// スケジュール処理　画面共通に渡す情報を作成するクラス
        /// </summary>
        public static class ScheduleListUtil
        {
            /// <summary>
            /// レイアウト情報を設定
            /// </summary>
            /// <param name="individualDictionary">this.IndividualDictionary</param>
            /// <param name="condition">画面のスケジュール表示条件のクラス</param>
            /// <param name="isMovable">マークを移動可能な場合True</param>
            /// <param name="nendoText">年度表示用フォーマット文字列"{0}年度"</param>
            /// <param name="yearStartMonth">年度開始月</param>
            public static void SetLayout(ref Dictionary<string, object> individualDictionary, Dao.ScheduleList.GetCondition condition, bool isMovable, string nendoText, int yearStartMonth, int? layoutNo = null)
            {
                // グローバル変数のキー
                string layoutKey = "scheduleLayout";
                if (layoutNo != null)
                {
                    layoutKey = "scheduleLayout" + layoutNo.ToString();
                }
                // 既に存在する場合は削除
                if (individualDictionary.ContainsKey(layoutKey))
                {
                    individualDictionary.Remove(layoutKey); // 削除
                }

                // 登録 値は文字列に変換
                individualDictionary.Add(layoutKey, condition.ConvertScheduleLayout(nendoText, yearStartMonth, isMovable));
            }

            /// <summary>
            /// ディクショナリの検索結果に追加可能なデータ形式に変換する
            /// </summary>
            /// <param name="scheduleList">スケジュールの取得結果</param>
            /// <param name="condition">画面のスケジュール表示条件のクラス</param>
            /// <returns>二重のディクショナリ(キー：画面の一覧との紐付用キー値、値：ディクショナリ(キー：列の日付、値：セルの値)</returns>
            public static Dictionary<string, Dictionary<string, string>> ConvertDictionaryAddData(List<Dao.ScheduleList.Display> scheduleList, Dao.ScheduleList.GetCondition condition)
            {
                // 戻り値
                // 行単位のキー(キー値)、列単位のキー(年月)、セルの値
                Dictionary<string, Dictionary<string, string>> result = new();
                bool isYear = condition.IsYear(); // 年単位で表示の場合True
                                                  // スケジュールの1行ごと(1セル単位)に繰り返し
                foreach (var scheduleCell in scheduleList)
                {
                    string keyId = scheduleCell.KeyId.ToString(); // 一覧と紐づけるキー値(一覧の1行のキー)
                    if (!result.ContainsKey(keyId))
                    {
                        // 存在しない場合一覧の行単位のディクショナリを追加
                        result.Add(keyId, new());
                    }

                    string dateKey = scheduleCell.GetDateKey(isYear); // 年月キー
                    string value = scheduleCell.ConvertScheduleData(); // 1セルの値
                    result[keyId].Add(dateKey, value);
                }

                return result;
            }
            /// <summary>
            /// 画面の一覧の内容より、スケジュールが移動されたセルの情報を取得する
            /// </summary>
            /// <param name="listDics">画面の一覧の内容、ResultInfoDictionaryを一覧のIDで絞り込み</param>
            /// <param name="valKeyId">一覧のキー値の列のVAL値、マッピング情報より項目キー名で取得、例：VAL99</param>
            /// <param name="monthStartNendo">年度開始月</param>
            /// <returns>移動されたスケジュールのリスト、移動されないなら空のリストなのでカウントが0件</returns>
            public static List<Dao.ScheduleList.Display> GetScheduleUpdatedData(List<Dictionary<string, object>> listDics, string valKeyId, int monthStartNendo)
            {
                List<Dao.ScheduleList.Display> result = new(); // 戻り値
                // 一覧で繰り返し、各行に対して処理を行う
                foreach (var rowDic in listDics)
                {
                    // 一覧のキー値
                    string keyId = rowDic[valKeyId].ToString();

                    // 一覧の各列に対して繰り返し処理を行い、移動されたスケジュールの列のみ処理を行う
                    foreach (string columnKey in rowDic.Keys)
                    {
                        // 列がスケジュールの年月列かどうか判定
                        if (!Dao.ScheduleList.Display.isKeyDate(columnKey))
                        {
                            // 異なる場合次の列へ
                            continue;
                        }
                        // スケジュール列の場合、列の値が移動されているかどうか判定
                        string columnValue = rowDic[columnKey].ToString(); // 列の値
                        if (!Dao.ScheduleList.Display.isUpdatedMark(columnValue))
                        {
                            // 異なる場合次の列へ
                            continue;
                        }
                        // 移動されたスケジュール列なので、画面の列の内容よりデータクラスに変換
                        Dao.ScheduleList.Display cell = new(columnKey, columnValue, monthStartNendo);
                        cell.KeyId = keyId; // キーIDに取得した値を指定
                        result.Add(cell); // 返却リストへ追加
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 年度開始月を取得する処理
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="factoryId">ユーザの工場ID、省略時は共通のものを使用</param>
        /// <returns>年度開始月の値</returns>
        /// <remarks>指定工場IDで取得できなかった場合は共通の工場IDで取得</remarks>
        public static int GetYearStartMonth(ComDB db, int factoryId = TMQConsts.CommonFactoryId)
        {
            return GetStructureExDataNarrowFacotryId(TMQConsts.MsStructure.GroupId.BeginningMonth, 1, db, factoryId);
        }

        /// <summary>
        /// 構成マスタ拡張項目の値を工場IDで絞込んで取得する処理
        /// </summary>
        /// <param name="structureGrpId">構成グループID</param>
        /// <param name="seqNo">連番</param>
        /// <param name="db">DB接続</param>
        /// <param name="factoryId">ユーザの工場ID、省略時は共通のものを使用</param>
        /// <param name="isGetDeleted">削除を含む場合True、省略時はFalse(含まない)</param>
        /// <returns>構成マスタ拡張項目の値</returns>
        private static int GetStructureExDataNarrowFacotryId(TMQConsts.MsStructure.GroupId structureGrpId, int seqNo, ComDB db, int factoryId = TMQConsts.CommonFactoryId, bool isGetDeleted = false)
        {
            // 検索パラメータ
            StructureItemEx.StructureItemExInfo param = new();
            param.StructureGroupId = (int)structureGrpId;
            param.Seq = seqNo;
            // 構成マスタより拡張項目を含む値を取得
            List<StructureItemEx.StructureItemExInfo> results = StructureItemEx.GetStructureItemExData(param, db, isGetDeleted: isGetDeleted);
            // 指定された工場IDで絞込、無ければ共通の工場IDで絞込
            string exData;
            bool isGet = results.Where(x => x.FactoryId == factoryId).Any(); //工場IDで絞込、結果の有無

            // 有る場合、絞り込んだ工場IDで拡張項目の値を取得
            // 無い場合、共通の工場IDで拡張項目の値を取得
            int narrowFactoryId = isGet ? factoryId : TMQConsts.CommonFactoryId;
            exData = getExDataNarraowFacotryId(results, narrowFactoryId);

            // 数値に変換
            return int.Parse(exData);

            // 構成アイテム拡張の取得結果より工場IDで絞込み、拡張項目の値を取得
            string getExDataNarraowFacotryId(List<StructureItemEx.StructureItemExInfo> pResults, int pFactoryId)
            {
                return pResults.Where(x => x.FactoryId == pFactoryId).First().ExData;
            }

        }

        /// <summary>
        /// 保全実績評価集計期首月を取得する処理
        /// </summary>
        /// <param name="month">期首月</param>
        /// <param name="term">期の期間</param>
        /// <param name="db">DB接続</param>
        /// <param name="factoryId">ユーザの工場ID、省略時は共通のものを使用</param>
        /// <returns>期首月</returns>
        /// <returns>期の期間</returns>
        /// <remarks>指定工場IDで取得できなかった場合は共通の工場IDで取得</remarks>
        public static void GetBeginningMonth(out int month, out int term, ComDB db, int factoryId = TMQConsts.CommonFactoryId)
        {
            month = GetStructureExDataNarrowFacotryId(TMQConsts.MsStructure.GroupId.BeginningMonth, 1, db, factoryId);          //連番1
            term = GetStructureExDataNarrowFacotryId(TMQConsts.MsStructure.GroupId.BeginningMonth, 2, db, factoryId);         //連番2
        }

        /// <summary>
        /// 検索対象工場IDリスト(共通工場IDのみ)を取得
        /// </summary>
        /// <returns>共通工場IDのみを取得</returns>
        public static List<int> GetFactoryIdList()
        {
            // 共通工場IDのみのリストを作成
            List<int> result = new() { TMQConsts.CommonFactoryId };
            return result;
        }

        /// <summary>
        /// 検索対象工場IDリスト(共通工場ID + ユーザの本務工場ID)を取得
        /// </summary>
        /// <param name="pUserId">ユーザID</param>
        /// <param name="db">DB接続</param>
        /// <returns>共通工場ID + ユーザの本務工場ID</returns>
        public static List<int> GetFactoryIdList(string pUserId, ComDB db)
        {
            // 共通工場IDのみのリスト
            List<int> result = GetFactoryIdList();
            // ユーザの本務工場IDを追加
            result.Add(GetUserFactoryId(pUserId, db));
            return result;
        }

        /// <summary>
        /// ユーザのユーザ権限マスタの全ての工場を取得
        /// </summary>
        /// <param name="pUserId">ユーザID</param>
        /// <param name="db">DB接続</param>
        /// <returns>工場のIDリスト</returns>
        public static List<int> GetUserBelongFactoryIdList(int pUserId, ComDB db)
        {
            var user = new TMQDataClass.MsUserBelongEntity
            {
                UserId = pUserId
            };
            return TMQUtil.SqlExecuteClass.SelectList<int>(SqlNameMaster.GetUserFactory, SqlNameMaster.SubDir, user, db);
        }

        /// <summary>
        /// ユーザの本務工場IDを取得
        /// </summary>
        /// <param name="pUserId">ユーザID</param>
        /// <param name="db">DB接続</param>
        /// <returns>ユーザの本務工場ID</returns>
        public static int GetUserFactoryId(string pUserId, ComDB db)
        {
            // ユーザ所属マスタより取得
            var user = new TMQDataClass.MsUserBelongEntity().GetUserDutyEntity(pUserId, db);
            // 工場でなく場所階層IDなので構成マスタより工場IDを取得
            var structure = new TMQDataClass.MsStructureEntity().GetEntity(user.LocationStructureId, db);
            return structure.FactoryId ?? TMQConsts.CommonFactoryId;
        }

        /// <summary>
        /// 棚と棚枝番を結合する文字列を取得
        /// </summary>
        /// <param name="factoryIdList">工場IDリスト</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB接続</param>
        /// <returns>結合文字列</returns>
        public static string GetJoinStrOfPartsLocation(int factoryId, string languageId, ComDB db)
        {
            string joinStr = ComUtil.GetPropertiesMessage(ComRes.ID.ID141260001, languageId, null, db, new List<int> { factoryId });
            return joinStr;
        }

        /// <summary>
        /// 棚と棚枝番を結合する文字列を取得(重複取得なし)
        /// </summary>
        /// <param name="factoryId">工場ID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB接続</param>
        /// <param name="factoryJoinDic">工場IDと結合文字列の辞書、同じ工場で二度取得しないようにする</param>
        /// <returns>結合文字列</returns>
        public static string GetJoinStrOfPartsLocationNoDuplicate(int factoryId, string languageId, ComDB db, ref Dictionary<int, string> factoryJoinDic)
        {
            // すでに取得済みなら辞書から値を返す
            if (factoryJoinDic.ContainsKey(factoryId))
            {
                return factoryJoinDic[factoryId];
            }
            // 未取得の場合取得し、辞書に追加
            string joinStr = ComUtil.GetPropertiesMessage(ComRes.ID.ID141260001, languageId, null, db, new List<int> { factoryId });
            factoryJoinDic.Add(factoryId, joinStr);
            return joinStr;
        }

        /// <summary>
        /// 棚番、結合文字、棚枝番より表示用棚番を取得
        /// </summary>
        /// <param name="partsLocation">棚番</param>
        /// <param name="partsLocationDetailNo">棚枝番</param>
        /// <param name="joinStr">結合文字</param>
        /// <returns>表示用棚番</returns>
        public static string GetDisplayPartsLocation(string partsLocation, string partsLocationDetailNo, string joinStr)
        {
            if (!string.IsNullOrEmpty(partsLocationDetailNo))
            {
                //棚番＋結合文字＋棚枝番
                StringBuilder sb = new();
                sb.Append(partsLocation);
                sb.Append(joinStr);
                sb.Append(partsLocationDetailNo);
                return sb.ToString();
            }
            //棚番(棚枝番がNULLまたは空文字の場合)
            return partsLocation;
        }

        /// <summary>
        /// 棚と棚枝番を結合する文字列を取得し、表示用棚番を取得
        /// </summary>
        /// <param name="partsLocation">棚番</param>
        /// <param name="partsLocationDetailNo">棚枝番</param>
        /// <param name="factoryIdList">工場IDリスト</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB接続</param>
        /// <returns>表示用棚番</returns>
        public static string GetDisplayPartsLocation(string partsLocation, string partsLocationDetailNo, int factoryId, string languageId, ComDB db)
        {
            //棚番の結合文字列を取得
            string joinStr = GetJoinStrOfPartsLocation(factoryId, languageId, db);
            //棚番、結合文字、棚枝番より表示用棚番を取得
            return GetDisplayPartsLocation(partsLocation, partsLocationDetailNo, joinStr);
        }

        /// <summary>
        /// 棚と棚枝番を結合する文字列を取得し、表示用棚番を取得
        /// </summary>
        /// <param name="partsLocation">棚ID</param>
        /// <param name="partsLocationDetailNo">棚枝番</param>
        /// <param name="factoryIdList">工場IDリスト</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB接続</param>
        /// <returns>表示用棚番</returns>
        public static string GetDisplayPartsLocation(long partsLocationId, string partsLocationDetailNo, int factoryId, string languageId, ComDB db)
        {
            //棚番の結合文字列を取得
            string joinStr = GetJoinStrOfPartsLocation(factoryId, languageId, db);
            //棚IDより棚の翻訳を取得
            // 棚IDは予備品テーブルはbigintだが定義元の構成マスタはintなので問題なし
            STDDao.VStructureItemEntity item =  STDDao.VStructureItemEntity.GetEntityByIdLanguage(Convert.ToInt32(partsLocationId), languageId, db);
            //棚番、結合文字、棚枝番より表示用棚番を取得
            return GetDisplayPartsLocation(item.TranslationText, partsLocationDetailNo, joinStr);
        }

        /// <summary>
        /// 棚と棚枝番を結合する文字列を取得し、表示用棚番を取得（一覧用）
        /// </summary>
        /// <param name="partsLocation">棚ID</param>
        /// <param name="partsLocationDetailNo">棚枝番</param>
        /// <param name="factoryIdList">工場IDリスト</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB接続</param>
        /// <param name="factoryJoinDic">工場IDと結合文字列の辞書、同じ工場で二度取得しないようにする</param>
        /// <param name="partsLocationTranslationList">棚IDと翻訳の辞書、同じ棚で二度取得しないようにする</param>
        /// <returns>表示用棚番</returns>
        public static string GetDisplayPartsLocation(long partsLocationId, string partsLocationDetailNo, int factoryId, string languageId, ComDB db, ref Dictionary<int, string> factoryJoinDic, List<STDDao.VStructureItemEntity> partsLocationTranslationList = null)
        {
            //棚番の結合文字列を取得
            string joinStr = GetJoinStrOfPartsLocationNoDuplicate(factoryId, languageId, db, ref factoryJoinDic);

            //棚の翻訳を取得
            string partsLocationText = "";
            if (partsLocationTranslationList != null && partsLocationTranslationList.Count > 0 && partsLocationTranslationList.Exists(x => x.StructureId == partsLocationId))
            {
                partsLocationText = partsLocationTranslationList.Where(x => x.StructureId == partsLocationId).Select(x => x.TranslationText).FirstOrDefault();
            }
            else
            {
                //棚IDより棚の翻訳をDBより取得
                // 棚IDは予備品テーブルはbigintだが定義元の構成マスタはintなので問題なし
                STDDao.VStructureItemEntity item = STDDao.VStructureItemEntity.GetEntityByIdLanguage(Convert.ToInt32(partsLocationId), languageId, db);
                partsLocationText = item.TranslationText;
            }
            //棚番、結合文字、棚枝番より表示用棚番を取得
            return GetDisplayPartsLocation(partsLocationText, partsLocationDetailNo, joinStr);
        }

        /// <summary>
        /// 棚IDリストより翻訳を取得
        /// </summary>
        /// <param name="partsLocationIdList">検索対象の棚IDのリスト</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB接続</param>
        /// <returns>棚の翻訳リスト</returns>
        public static List<STDDao.VStructureItemEntity> GetpartsLocationList<T>(List<T> partsLocationIdList, string languageId, ComDB db)
        {
            //カンマ区切りの文字列にする
            string ids = string.Join(',', partsLocationIdList);
            var param = new { StructureIdList = ids, LanguageId = languageId };
            return SqlExecuteClass.SelectList<STDDao.VStructureItemEntity>(SqlName.GetPartsLocationTranslation, SqlName.SubDir, param, db);
        }

        /// <summary>
        /// SQL実行結果　在庫確定日、棚卸確定日取得
        /// </summary>
        public class PartsGetInfo
        {
            /// <summary>Gets or sets 在庫確定日</summary>
            /// <value>在庫確定日</value>
            public DateTime TargetMonth { get; set; }
            /// <summary>Gets or sets 棚卸確定日</summary>
            /// <value>棚卸確定日</value>
            public DateTime FixedDatetime { get; set; }

            /// <summary>
            /// 予備品IDより在庫確定日を取得する
            /// </summary>
            /// <param name="param">検索条件：予備品ID</param>
            /// <param name="db">DB接続</param>
            /// <returns></returns>
            public static DateTime GetInventoryConfirmationDate(object param, ComDB db)
            {
                // SQL実行
                var dateResults = TMQUtil.SqlExecuteClass.SelectEntity<PartsGetInfo>(SqlName.SelectInventoryConfirmationDate, SqlName.SubDir, param, db);
                return dateResults.TargetMonth;
            }

            /// <summary>
            /// 予備品ID、新旧区分、部門、棚ID、棚枝番、勘定科目より棚卸確定日を取得する
            /// </summary>
            /// <param name="param">検索条件：予備品ID、新旧区分、部門、棚ID、棚枝番、勘定科目</param>
            /// <param name="db">DB接続</param>
            /// <returns></returns>
            public static DateTime GetTakeInventoryConfirmationDate(object param, ComDB db)
            {
                // SQL実行
                var dateResults = TMQUtil.SqlExecuteClass.SelectEntity<PartsGetInfo>(SqlName.SelectTakeInventoryConfirmationDate, SqlName.SubDir, param, db);
                return dateResults.FixedDatetime;
            }
        }

        /// <summary>
        /// 小数点以下桁数 丸め処理
        /// </summary>
        /// <param name="quantity">数量</param>
        /// <param name="digitNum">小数点以下桁数</param>
        /// <param name="roundDivision">丸め処理区分</param>
        /// <returns></returns>
        public static string roundDigit(string quantity, int digitNum, int roundDivision, bool isNotSeparateComma = false)
        {
            // 数値に変換できない場合はそのまま返す
            if (!decimal.TryParse(quantity, out decimal num))
            {
                return quantity;
            }

            decimal calcVal = 0;
            decimal roundNum = (decimal)Math.Pow(10, (double)digitNum);
            switch (roundDivision)
            {
                case (int)Const.RoundDivision.Round: // 四捨五入
                    calcVal = Math.Round(num, digitNum, MidpointRounding.AwayFromZero);
                    break;

                case (int)Const.RoundDivision.Ceiling: // 切り上げ
                    calcVal = digitNum == 0 ? Math.Ceiling(num) : Math.Ceiling(num * roundNum) / roundNum;
                    break;

                case (int)Const.RoundDivision.Floor: // 切り捨て
                    calcVal = digitNum == 0 ? Math.Floor(num) : Math.Floor(num * roundNum) / roundNum;
                    break;
                default:
                    return quantity;
            }

            if (isNotSeparateComma)
            {
                return calcVal.ToString();
            }

            // 3桁ごとにカンマ
            return string.Format("{0:N" + digitNum.ToString() + "}", calcVal);
        }

        /// <summary>
        /// 数量と単位を結合する
        /// </summary>
        /// <param name="quantity">数量</param>
        /// <param name="unit">単位</param>
        /// <param name="isUseSpace">結合に半角スペースを使用する場合はtrue</param>
        /// <returns>数量 + 単位</returns>
        public static string CombineNumberAndUnit(string quantity, string unit, bool isUseSpace = false)
        {
            // 数値が空の場合は何もしない
            if (string.IsNullOrEmpty(quantity))
            {
                return quantity;
            }

            // 結合時に半角スペースを含めるか判定
            if (isUseSpace)
            {
                return quantity + " " + unit;
            }
            else
            {
                return quantity + unit;
            }
        }

        /// <summary>
        /// 性能改善対応用
        /// </summary>
        public class ListPerformanceUtil
        {
            /// <summary>DB接続</summary>
            private ComDB Db { get; set; }
            /// <summary>一時テーブルのCreateTable文</summary>
            private StringBuilder SqlCreateTemp { get; set; }
            /// <summary>一時テーブルのINSERT文</summary>
            private StringBuilder SqlInsertTemp { get; set; }
            /// <summary>一時テーブルのINSERT文のパラメータ</summary>
            private dynamic Param { get; set; }
            /// <summary>言語ID</summary>
            private string LanguageId { get; set; }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="pDb">DB接続</param>
            /// <param name="pLanguageId">言語ID</param>
            public ListPerformanceUtil(ComDB pDb, string pLanguageId)
            {
                this.Db = pDb;
                this.SqlCreateTemp = new();
                this.SqlInsertTemp = new();
                this.LanguageId = pLanguageId;
                this.Param = new ExpandoObject() as IDictionary<string, object>;
            }
            /// <summary>
            /// 添付ファイル取得用の一時テーブルSQL取得処理
            /// </summary>
            /// <param name="functionTypeIds">機能タイプID</param>
            public void GetAttachmentSql(List<TMQConsts.Attachment.FunctionTypeId> functionTypeIds)
            {
                string create = SqlExecuteClass.GetExecuteSql(SqlName.CreateTableTempAttachment, SqlName.SubDirPerformance, string.Empty);
                this.SqlCreateTemp.Append(create);
                string insert = SqlExecuteClass.GetExecuteSql(SqlName.InsertTempAttachment, SqlName.SubDirPerformance, string.Empty);
                this.SqlInsertTemp.Append(insert);
                this.Param.FunctionTypeIdList = functionTypeIds;
            }
            /// <summary>
            /// 翻訳取得用の一時テーブルSQL取得処理のキーワード置換
            /// </summary>
            /// <param name="createSql">CREATE文</param>
            /// <param name="insertSql">INSERT文</param>
            /// <param name="groupId">構成グループID</param>
            private void getTranslationCommon(string createSql, string insertSql, TMQConsts.MsStructure.GroupId groupId)
            {
                const string replaceGroupId = "@ReplaceStructureGroupId";
                string newCreateSql = createSql.Replace(replaceGroupId, ((int)groupId).ToString());
                this.SqlCreateTemp.Append(newCreateSql);
                string newInsertSql = insertSql.Replace(replaceGroupId, ((int)groupId).ToString());
                this.SqlInsertTemp.Append(newInsertSql);
            }

            /// <summary>
            /// 翻訳用一時テーブルを作成
            /// </summary>
            public void GetCreateTranslation()
            {
                string create = SqlExecuteClass.GetExecuteSql(SqlName.CreateTableTempTranslation, SqlName.SubDirPerformance, string.Empty);
                this.SqlCreateTemp.Append(create);

            }

            /// <summary>
            /// 翻訳用一時テーブルにデータを登録
            /// </summary>
            /// <param name="structureGroupIds">登録する構成グループIDのリスト</param>
            /// <param name="isCreateIndex">インデックスを作成する(最後の登録)場合はTRUE</param>
            public void GetInsertTranslationAll(List<TMQConsts.MsStructure.GroupId> structureGroupIds, bool isCreateIndex = false)
            {
                List<string> listUnComment = new List<string>() { "CreateIndex" };
                string insert = SqlExecuteClass.GetExecuteSql(SqlName.InsertTempTranslation, SqlName.SubDirPerformance, string.Empty, isCreateIndex ? listUnComment : null);
                this.SqlInsertTemp.Append(insert);

                this.Param.LanguageId = this.LanguageId;
                this.Param.StructureGroupIdList = structureGroupIds;
            }

            /// <summary>
            /// 指定階層のみの翻訳用一時テーブルを登録
            /// </summary>
            /// <param name="structureGroupId">構成グループID</param>
            /// <param name="layerNo">階層番号</param>
            /// <param name="isCreateIndex">インデックスを作成する(最後の登録)場合はTRUE</param>
            public void GetInsertLayerOnly(TMQConsts.MsStructure.GroupId structureGroupId, int layerNo, bool isCreateIndex = false)
            {
                const string groupIdName = "@StructureGroupId";
                const string layerNoName = "@StructureLayerNo";
                List<string> listUnComment = new List<string>() { "CreateIndex" };
                string sql = SqlExecuteClass.GetExecuteSql(SqlName.InsertTempTranslationLayer, SqlName.SubDirPerformance, string.Empty, isCreateIndex ? listUnComment : null);
                string newSql = sql.Replace(layerNoName, layerNo.ToString()).Replace(groupIdName, ((int)structureGroupId).ToString());
                this.SqlInsertTemp.Append(newSql);
                this.Param.LanguageId = this.LanguageId;
            }

            /// <summary>
            /// 一時テーブル登録処理
            /// </summary>
            public void RegistTempTable()
            {
                // CREATEとINSERTを分けないと登録されない。パラメータの有無が影響している模様。
                // CREATE文
                this.Db.Regist(this.SqlCreateTemp.ToString());
                // INSERT文
                this.Db.Regist(this.SqlInsertTemp.ToString(), this.Param);
            }

            /// <summary>
            /// 一時テーブル登録処理(機能側から指定)
            /// </summary>
            /// <param name="subDir">SQLのフォルダ</param>
            /// <param name="createSqlName">CREATE文のファイル名</param>
            /// <param name="insertSqlName">INSERT文のファイル名</param>
            public void AddTempTable(string subDir, string createSqlName, string insertSqlName)
            {
                string create = SqlExecuteClass.GetExecuteSql(createSqlName, subDir, string.Empty);
                this.SqlCreateTemp.Append(create);
                string insert = SqlExecuteClass.GetExecuteSql(insertSqlName, subDir, string.Empty);
                this.SqlInsertTemp.Append(insert);
            }

            /// <summary>
            /// 一時テーブル登録処理(機能側からSQL文で指定)
            /// </summary>
            /// <param name="createSql"></param>
            /// <param name="insertSql"></param>
            public void AddTempTableBySql(string createSql, string insertSql)
            {
                this.SqlCreateTemp.Append(createSql);
                this.SqlInsertTemp.Append(insertSql);
            }
        }

        /// <summary>
        /// 件数取得用SQL文の生成
        /// </summary>
        /// <param name="tableNameList">件数を取得するテーブル名</param>
        /// <param name="whereParam">GetWhereClauseAndParam2()で設定されたWHERE句パラメータ</param>
        /// <returns>生成した検索に用いるSQL文</returns>
        public static string GetCountSql(string tableName, dynamic whereParam)
        {
            StringBuilder sbSql = new();
            sbSql.AppendLine("SELECT COUNT(*)");
            sbSql.AppendLine("FROM");
            sbSql.AppendLine(tableName);
            sbSql.AppendLine("WHERE");
            //whereParamに設定しているwhere句（場所階層、職種機種のExists句）を追加
            sbSql.AppendLine(whereParam.CountSqlWhere);
            //whereParamに設定しているwhere句を削除
            ((IDictionary<String, Object>)whereParam).Remove("CountSqlWhere");

            return sbSql.ToString();
        }
    }
}
