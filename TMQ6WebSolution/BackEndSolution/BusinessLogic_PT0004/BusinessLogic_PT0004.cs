using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_PT0004.BusinessLogicDataClass_PT0004;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_PT0004
{
    /// <summary>
    /// 在庫確定状況
    /// </summary>
    public class BusinessLogic_PT0004 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"InventoryFirm";
            /// <summary>工場ID取得</summary>
            public const string GetFactoryIdList = "GetFactoryIdList";
            /// <summary>職種ID取得</summary>
            public const string GetJobIdList = "GetJobIdList";
            /// <summary>在庫確定一覧取得</summary>
            public const string GetInventoryFirmList = "GetInventoryFirmList";
            /// <summary>確定在庫データ登録情報取得</summary>
            public const string GetInsertDataToFixedStock = "GetInsertDataToFixedStock";
            /// <summary>確定在庫データ登録</summary>
            public const string InsertFixedStock = "InsertFixedStock";
            /// <summary>在庫確定管理データ登録</summary>
            public const string InsertStockConfirm = "InsertStockConfirm";
            /// <summary>在庫確定管理データ登録</summary>
            public const string UpdateStockConfirm = "UpdateStockConfirm";
            /// <summary>確定在庫データ取得</summary>
            public const string GetStockIdList = "GetStockIdList";
            /// <summary>最大更新日時取得(在庫確定管理データ)</summary>
            public const string GetMaxUpdateDateConfirm = "GetMaxUpdateDateConfirm";
            /// <summary>最大更新日時取得(確定在庫データ)</summary>
            public const string GetMaxUpdateDateFixed = "GetMaxUpdateDateFixed";
            /// <summary>確定在庫データ削除</summary>
            public const string DeleteFixedStock = "DeleteFixedStock";
        }

        /// <summary>
        /// フォーム、グループ、コントロールの親子関係を表現した場合の定数クラス(サンプル)
        /// </summary>
        private static class ConductInfo
        {
            /// <summary>
            /// 一覧画面
            /// </summary>
            public static class FormList
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 0;

                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 対象年月(検索条件)
                    /// </summary>
                    public const string SearchCondition = "COND_000_00_LST_0";
                    /// <summary>
                    /// 在庫確定一覧
                    /// </summary>
                    public const string List = "BODY_020_00_LST_0";
                    /// <summary>
                    /// 工場ID一覧(非表示)
                    /// </summary>
                    public const string FactoryIdList = "BODY_040_00_LST_0";
                    /// <summary>
                    /// 職種ID一覧(非表示)
                    /// </summary>
                    public const string JobIdList = "BODY_050_00_LST_0";
                    /// <summary>
                    /// フラグ一覧(非表示)ボタンの非表示、登録処理に使用
                    /// </summary>
                    public const string FlgList = "BODY_060_00_LST_0";
                }

                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonControlId
                {
                    /// <summary>
                    /// 確定実行
                    /// </summary>
                    public const string ExecuteCommit = "ExecuteCommit";
                    /// <summary>
                    /// 確定解除
                    /// </summary>
                    public const string CancelCommit = "CancelCommit";
                }
            }
        }

        /// <summary>
        /// 対象年月が当月以上の場合一覧にセットする値
        /// </summary>
        private const int FlgJudgeTargetMonth = 1;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_PT0004() : base()
        {
        }
        #endregion

        #region オーバーライドメソッド
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int InitImpl()
        {
            this.ResultList = new();

            // 検索条件の対象年月を初期化
            if (!setTargetDate())
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            // 一覧検索処理
            if (!searchList())
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ExecuteImpl()
        {
            // 登録処理実行
            return Regist();
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse
            // 処理完了メッセージ
            string msg = string.Empty;
            switch (this.CtrlId)
            {
                case ConductInfo.FormList.ButtonControlId.ExecuteCommit: // 確定実行
                    resultRegist = ExecuteCommit();
                    msg = ComRes.ID.ID111110063;
                    break;
                case ConductInfo.FormList.ButtonControlId.CancelCommit: // 確定解除
                    resultRegist = CancelCommit();
                    msg = ComRes.ID.ID111110064;
                    break;
                default:
                    return ComConsts.RETURN_RESULT.NG;
            }

            // 登録処理判定
            if (!setErr(resultRegist))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 一覧再検索
            resultRegist = searchList(true);
            if (!setErr(resultRegist))
            {
                return ComConsts.RETURN_RESULT.NG;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「在庫確定/在庫確定解除 が完了しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060002, msg });

            return ComConsts.RETURN_RESULT.OK;

            bool setErr(bool resultRegist)
            {
                // 登録処理結果によりエラー処理を行う
                if (!resultRegist)
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 未設定時にエラーメッセージを設定
                    if (string.IsNullOrEmpty(this.MsgId))
                    {
                        // 「登録処理に失敗しました。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                    }
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            // 削除処理は無し

            this.ResultList = new();
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド

        /// <summary>
        /// 対象年月 初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool setTargetDate()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.SearchCondition, this.pageInfoList);
            pageInfo.CtrlId = ConductInfo.FormList.ControlId.SearchCondition;

            // システム日付の前月を取得
            Dao.searchCondition condition = new();
            condition.TargetMonth = DateTime.Now.AddMonths(-1);

            // 画面の項目に設定
            if (!SetSearchResultsByDataClass<Dao.searchCondition>(pageInfo, new List<Dao.searchCondition>() { condition }, 1))
            {
                return false;
            }

            return true;
        }

        #region 検索処理
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList(bool isResearch = false)
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetInventoryFirmList, out string baseSql);
            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetInventoryFirmList, out string withSql);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereClause, out dynamic whereParam, out bool isDetailConditionApplied))
            {
                return false;
            }

            IDictionary<string, object> tmpList = whereParam as IDictionary<string, object>;
            // 再検索かどうか判定
            if (!isResearch)
            {
                // ツリーで工場が選択されているか判定
                if (!tmpList.ContainsKey("LocationIdList"))
                {
                    // 選択されていない場合、ユーザ権限の工場IDを取得
                    whereParam.LocationIdList = getFactoryIdList();
                }

                // ツリーで職種が選択されているか判定
                if (!tmpList.ContainsKey("JobIdList"))
                {
                    // 選択されていない場合、ユーザ権限の職種IDを取得
                    whereParam.JobIdList = getJobIdList();
                }
            }
            else
            {
                // 非表示の一覧に退避している工場IDを取得
                if (!getHideFactoryIdList(out List<int> locationIdList))
                {
                    return false;
                }
                whereParam.LocationIdList = locationIdList;

                // 非表示の一覧に退避している職種IDを取得
                if (!getHideJobIdList(out List<int> jobIdList))
                {
                    return false;
                }
                whereParam.JobIdList = jobIdList;
            }

            // 検索条件で入力された対象年月を取得
            whereParam.TargetMonth = getSearchCondition();
            whereParam.LanguageId = this.LanguageId;

            // 一覧検索SQL文の取得
            string execSql = TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql);

            // 並び替え条件を追加(工場IDの昇順、職種IDの昇順)
            var selectSql = new StringBuilder(execSql);
            selectSql.AppendLine("ORDER BY factory_id,parts_job_id");

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                // データが無い場合はエラーにしないのでrueを返す
                return true;
            }

            // 非表示の工場IDリスト
            if (!setFactoryIdList(whereParam.LocationIdList))
            {
                return false;
            }

            // 非表示の職種IDリスト
            if (!setJobIdList(whereParam.JobIdList))
            {
                return false;
            }

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results, results.Count, isDetailConditionApplied))
            {
                return false;
            }

             // 非表示の一覧に対象年月を設定
            if(!setFlgList(whereParam.TargetMonth))
            {
                return false;
            }

            return true;

            /// <summary>
            /// 非表示の工場IDリストに工場IDを設定する
            /// </summary>
            /// <param name="locationIdList">ツリーで選択されたアイテム</param>
            /// <returns>エラーの場合False</returns>
            bool setFactoryIdList(List<int> locationIdList)
            {
                List<Dao.searchResult> factoryIdList = new();
                foreach (int id in locationIdList)
                {
                    Dao.searchResult factoryId = new();
                    factoryId.FactoryId = (long)id;
                    factoryIdList.Add(factoryId);
                }

                // ページ情報取得
                var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.FactoryIdList, this.pageInfoList);
                // 検索結果の設定
                if (!SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, factoryIdList, factoryIdList.Count))
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 非表示の職種IDリストに職種IDを設定する
            /// </summary>
            /// <param name="jobList">ツリーで選択されたアイテム</param>
            /// <returns>エラーの場合False</returns>
            bool setJobIdList(List<int> jobList)
            {
                List<Dao.searchResult> jobIdList = new();
                foreach (int id in jobList)
                {
                    Dao.searchResult jobId = new();
                    jobId.PartsJobId = (long)id;
                    jobIdList.Add(jobId);
                }

                // ページ情報取得
                var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.JobIdList, this.pageInfoList);
                // 検索結果の設定
                if (!SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, jobIdList, jobIdList.Count))
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 非表示の工場IDリストの値を取得する
            /// </summary>
            /// <param name="factoryIdList">工場IDリスト</param>
            /// <returns>エラーの場合False</returns>
            bool getHideFactoryIdList(out List<int> factoryIdList)
            {
                factoryIdList = new();
                // 工場ID一覧の情報を取得
                List<Dictionary<string, object>> list = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormList.ControlId.FactoryIdList);

                foreach (var val in list)
                {
                    Dao.searchResult factoryId = new();
                    if (!SetExecuteConditionByDataClass<Dao.searchResult>(val, ConductInfo.FormList.ControlId.FactoryIdList, factoryId, DateTime.Now, this.UserId))
                    {
                        return false;
                    }
                    factoryIdList.Add((int)factoryId.FactoryId);
                }

                return true;
            }

            /// <summary>
            /// 非表示の工場IDリストの値を取得する
            /// </summary>
            /// <param name="factoryIdList">工場IDリスト</param>
            /// <returns>エラーの場合False</returns>
            bool getHideJobIdList(out List<int> jobIdList)
            {
                jobIdList = new();
                // 工場ID一覧の情報を取得
                List<Dictionary<string, object>> list = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormList.ControlId.JobIdList);

                foreach (var val in list)
                {
                    Dao.searchResult jobId = new();
                    if (!SetExecuteConditionByDataClass<Dao.searchResult>(val, ConductInfo.FormList.ControlId.JobIdList, jobId, DateTime.Now, this.UserId))
                    {
                        return false;
                    }
                    jobIdList.Add((int)jobId.PartsJobId);
                }

                return true;
            }

            /// <summary>
            /// ボタンの非表示、対象年月を一覧に設定
            /// </summary>
            /// <param name="targetMonth">対象年月</param>
            /// <returns>エラーの場合False</returns>
            bool setFlgList(string targetMonth)
            {
                Dao.flgList flg = new();
                flg.TargetMonth = targetMonth;

                // 対象年月 >= 当月の場合は警告メッセージを表示
                if (DateTime.Parse(getSearchCondition(true)) >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                {
                    flg.FlgJudgeTargetMonth = FlgJudgeTargetMonth;
                    //「当月以降の在庫確定は実行できません。在庫確定を行う場合は前月以前の年月を指定してください。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141200004 });
                    // 警告
                    this.Status = CommonProcReturn.ProcStatus.Warning;
                }
                else
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                }

                // ページ情報取得
                var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.FlgList, this.pageInfoList);

                // 検索結果の設定
                IList<Dao.flgList> flgList = new List<Dao.flgList> { flg };
                if (!SetSearchResultsByDataClass<Dao.flgList>(pageInfo, flgList, flgList.Count))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// ユーザー権限の工場IDを取得
        /// </summary>
        /// <returns>工場IDリスト</returns>
        private IList<int> getFactoryIdList()
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetFactoryIdList, out string execSql);
            Dao.searchCondition condition = new();
            condition.UserId = this.UserId;
            // ユーザー権限の工場IDを取得
            IList<int> factoryIdList = this.db.GetList<int>(execSql, condition);
            return factoryIdList;
        }

        /// <summary>
        /// ユーザー権限の職種IDを取得
        /// </summary>
        /// <returns>職種IDリスト</returns>
        private IList<int> getJobIdList()
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetJobIdList, out string execSql);
            Dao.searchCondition condition = new();
            condition.UserId = this.UserId;
            // ユーザー権限の職種IDを取得
            IList<int> jobIdList = this.db.GetList<int>(execSql, condition);
            return jobIdList;
        }
        #endregion

        #region 確定実行
        /// <summary>
        /// 確定実行処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool ExecuteCommit()
        {
            // 検索条件エリアの「対象年月」を月初で取得
            ComDao.PtStockComfirmEntity targetMonth = new();
            targetMonth.TargetMonth = DateTime.Parse(getSearchCondition(true));
            // 現在日時
            DateTime now = DateTime.Now;

            // 一覧で選択されているレコードを取得
            string ctrlId = ConductInfo.FormList.ControlId.List;
            var selectedList = getSelectedRowsByList(this.resultInfoDictionary, ctrlId);

            // 対象年月が検索時と異なるかチェック
            if(isErrorTargetMonth(getSearchCondition()))
            {
                return false;
            }

            // 入力チェック
            if (isErrorRegistComfirm(selectedList, targetMonth.TargetMonth))
            {
                return false;
            }

            foreach (var result in selectedList)
            {
                // 確定在庫データ登録処理
                if (!registFixedStock(result, targetMonth.TargetMonth, ctrlId, now))
                {
                    return false;
                }

                //在庫確定管理データ登録処理
                if (!registStockComfirm(result, targetMonth.TargetMonth, ctrlId, now))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 確定在庫データ登録処理
        /// </summary>
        /// <param name="selectedRow">一覧で選択されたデータ</param>
        /// <param name="targetMonth">対象年月</param>
        /// <param name="ctrlId">一覧のコントロールID</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合False</returns>
        private bool registFixedStock(Dictionary<string, object> selectedRow, DateTime targetMonth, string ctrlId, DateTime now)
        {
            // 検索条件を取得
            if (!getSearchCondition(selectedRow, ctrlId, now, out Dao.searchConditionFixedStock searchCondition))
            {
                return false;
            }

            // SQLを取得
            List<string> listUnComment = ComUtil.GetNotNullNameByClass<Dao.searchConditionFixedStock>(searchCondition);
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetInsertDataToFixedStock, out string selectSql, listUnComment);
            // 一覧検索実行
            IList<ComDao.PtFixedStockEntity> results = db.GetListByDataClass<ComDao.PtFixedStockEntity>(selectSql.ToString(), searchCondition);
            if (results == null || results.Count == 0)
            {
                return true;
            }

            foreach (ComDao.PtFixedStockEntity registInfo in results)
            {
                // データクラスに設定
                registInfo.TargetMonth = targetMonth; // 対象年月
                registInfo.InsertDatetime = now; // 登録日時
                registInfo.InsertUserId = int.Parse(this.UserId); // 登録ユーザーID
                registInfo.UpdateDatetime = now; // 更新日時
                registInfo.UpdateUserId = int.Parse(this.UserId); // 更新ユーザーID

                // 前回の末在庫数 = 0 かつ 今回の入庫数 = 0 かつ 今回の出庫数 = 0 の場合は登録をしない
                if (registInfo.InventoryQuantity == 0 && registInfo.StorageQuantity == 0 && registInfo.ShippingQuantity == 0)
                {
                    continue;
                }

                // 登録処理
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.InsertFixedStock, SqlName.SubDir, registInfo, db))
                {
                    return false;
                }
            }

            return true;

            bool getSearchCondition(Dictionary<string, object> selectedRow, string ctrlId, DateTime now, out Dao.searchConditionFixedStock searchConditionFixedStock)
            {
                searchConditionFixedStock = new();

                // 検索条件の作成
                Dao.searchResult registInfo = new();
                if (!SetExecuteConditionByDataClass<Dao.searchResult>(selectedRow, ctrlId, registInfo, now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return false;
                }

                DateTime? lastConfirmedDate = null;
                string selectedLastConfirmedDate = string.Empty;
                // 選択されたレコードの最終確定年月が日付に変更できる場合
                if (DateTime.TryParse(registInfo.LastConfirmedDate.ToString(), out DateTime outDate))
                {
                    lastConfirmedDate = outDate.AddMonths(1); // 今回のデータの取得開始日
                    selectedLastConfirmedDate = outDate.ToString("yyyy/MM"); // 前回の最終確定年月
                }
                searchConditionFixedStock.FactoryId = registInfo.FactoryId;              // 工場ID
                searchConditionFixedStock.PartsJobId = registInfo.PartsJobId;            // 職種ID
                searchConditionFixedStock.InoutDatetimeFrom = lastConfirmedDate;         // 受払日時From
                searchConditionFixedStock.InoutDatetimeTo = targetMonth.AddMonths(1);    // 受払日時To
                searchConditionFixedStock.LastConfirmedDate = selectedLastConfirmedDate; // 最終確定年月

                return true;
            }
        }

        /// <summary>
        /// 在庫確定管理データ登録処理
        /// </summary>
        /// <param name="result">選択されたレコード</param>
        /// <param name="targetMonth">対象年月</param>
        /// <param name="ctrlId">一覧のコントロールID</param>
        /// <param name="now">現在日時</param>
        /// <returns>エラーの場合False</returns>
        private bool registStockComfirm(Dictionary<string, object> result, DateTime targetMonth, string ctrlId, DateTime now)
        {
            ComDao.PtStockComfirmEntity registInfo = new();
            if (!SetExecuteConditionByDataClass<ComDao.PtStockComfirmEntity>(result, ctrlId, registInfo, now, this.UserId, this.UserId))
            {
                return false;
            }

            // 登録情報の設定
            registInfo.TargetMonth = targetMonth;                         // 対象年月
            registInfo.ExecutionDatetime = new DateTime(targetMonth.Year, // 実行日時(対象年月の末日で、時刻は23:59:59)
                                                        targetMonth.Month,
                                                        targetMonth.AddMonths(1).AddDays(-1).Day,
                                                        23,
                                                        59,
                                                        59);
            registInfo.ExecutionUserId = this.UserId;                     // 実行ユーザ―ID

            // 登録処理
            if (!TMQUtil.SqlExecuteClass.Regist(SqlName.InsertStockConfirm, SqlName.SubDir, registInfo, db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 確定実行処理　入力チェック
        /// </summary>
        /// <param name="resultList">一覧で選択されたレコード</param>
        /// <param name="targetMonth">対象年月</param>
        /// <returns>入力チェックエラーがある場合True</returns>
        private bool isErrorRegistComfirm(List<Dictionary<string, object>> resultList, DateTime targetMonth)
        {
            foreach (var result in resultList)
            {
                // データクラスにセット
                Dao.searchResult registInfo = new();
                if (!SetExecuteConditionByDataClass<Dao.searchResult>(result, ConductInfo.FormList.ControlId.List, registInfo, DateTime.Now, this.UserId, this.UserId))
                {
                    return true;
                }

                // 選択されたレコードの最終確定年月がnull(在庫確定されていない)の場合は何もしない
                if (registInfo.LastConfirmedDate == null)
                {
                    continue;
                }

                // 選択されたレコードの最終確定年月が実行対象年月未満の場合は何もしない
                if (registInfo.LastConfirmedDate < targetMonth)
                {
                    continue;
                }

                // 選択されたレコードの最終確定年月が実行対象年月の場合
                //「既に在庫確定されています。○行目」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141130002, getRowNo(result).ToString() });
                return true;
            }

            return false;

            int getRowNo(Dictionary<string, object> result)
            {
                // 選択されたレコードの行番号を取得する
                foreach (var val in result)
                {
                    if (val.Key == "ROWNO")
                    {
                        return int.Parse(val.Value.ToString());
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// 画面の対象年月と検索時の対象年月が一致しているかをチェック
        /// </summary>
        /// <param name="targetMonth">画面の対象年月</param>
        /// <returns>入力チェックエラーがある場合True</returns>
        private bool isErrorTargetMonth(string targetMonth)
        {
            // 非表示の一覧より、対象年月を取得
            Dao.flgList checkTargetMonth = new();
            Dictionary<string, object> resulta = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormList.ControlId.FlgList);
            SetDataClassFromDictionary(resulta, ConductInfo.FormList.ControlId.FlgList, checkTargetMonth);
            if(targetMonth != checkTargetMonth.TargetMonth)
            {
                //「対象年月が検索時と異なります。再度検索してください。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160007 });
                return true;
            }

            return false;
        }
        #endregion

        #region 確定解除
        /// <summary>
        /// 確定解除処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool CancelCommit()
        {
            // 検索条件エリアの「対象年月」
            Dao.searchResult targetMonth = new();
            targetMonth.TargetMonth = getSearchCondition();

            // 現在日時
            DateTime now = DateTime.Now;

            // 一覧で選択されているレコードを取得
            string ctrlId = ConductInfo.FormList.ControlId.List;
            var selectedList = getSelectedRowsByList(this.resultInfoDictionary, ctrlId);

            // 入力チェック
            if (isErrorRegistCancel(selectedList, targetMonth.TargetMonth))
            {
                return false;
            }

            // 排他チェック
            if (!isErrorExclusive(selectedList))
            {
                return false;
            }

            foreach (var result in selectedList)
            {
                // 選択されたレコードをデータクラスにセット
                Dao.searchResult registInfo = new();
                if (!SetExecuteConditionByDataClass<Dao.searchResult>(result, ctrlId, registInfo, now, this.UserId, this.UserId))
                {
                    return false;
                }
                registInfo.TargetMonth = targetMonth.TargetMonth; // 対象年月

                // 在庫確定管理データ更新処理
                if (!updateStockConfirm(registInfo))
                {
                    return false;
                }

                // 確定在庫データ削除処理
                if (!deleteFixedStock(registInfo))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 在庫確定管理データ登録処理
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <returns>エラーの場合False</returns>
        private bool updateStockConfirm(Dao.searchResult registInfo)
        {
            // 更新処理
            if (!TMQUtil.SqlExecuteClass.Regist(SqlName.UpdateStockConfirm, SqlName.SubDir, registInfo, db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 確定在庫データ削除処理
        /// </summary>
        /// <param name="searchCondition">検索条件</param>
        /// <returns>エラーの場合False</returns>
        private bool deleteFixedStock(Dao.searchResult searchCondition)
        {
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetStockIdList, out string selectSql);
            // 選択されたレコードの工場・職種の組み合わせを持つ予備品IDを使用している在庫IDを取得する
            IList<ComDao.PtFixedStockEntity> results = db.GetListByDataClass<ComDao.PtFixedStockEntity>(selectSql.ToString(), searchCondition);
            if (results == null || results.Count == 0)
            {
                return true;
            }

            // 取得した在庫IDのレコードを削除する
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.DeleteFixedStock, out string deleteSql);
            foreach (ComDao.PtFixedStockEntity reult in results)
            {
                // 削除処理
                if (this.db.Regist(deleteSql, results) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 確定解除処理　入力チェック
        /// </summary>
        /// <param name="resultList">一覧で選択されたレコード</param>
        /// <param name="targetMonth">対象年月</param>
        /// <returns>入力チェックエラーがある場合True</returns>
        private bool isErrorRegistCancel(List<Dictionary<string, object>> resultList, string targetMonth)
        {
            foreach (var result in resultList)
            {
                // データクラスにセット
                Dao.searchResult registInfo = new();
                if (!SetExecuteConditionByDataClass<Dao.searchResult>(result, ConductInfo.FormList.ControlId.List, registInfo, DateTime.Now, this.UserId, this.UserId))
                {
                    return true;
                }

                // 最終確定年月を日付に変換
                string lastConfirmedDate = registInfo.LastConfirmedDate.ToString();
                if (DateTime.TryParse(lastConfirmedDate, out DateTime outDate))
                {
                    if (outDate.ToString("yyyy/MM") == targetMonth)
                    {
                        continue;
                    }
                }

                // 選択されたレコードの最終確定年月が対象年月と異なる場合
                //「対象年月の確定情報は存在しません。○行目」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160006, getRowNo(result).ToString() });
                return true;
            }

            return false;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive(List<Dictionary<string, object>> resultList)
        {
            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(ConductInfo.FormList.ControlId.List);
            string maxUpdateDatetimeConfirm = getValNoByParam(mappingInfo, "MaxUpdateDatetimeConfirm"); // 最大更新日時(在庫確定管理データ)
            string maxUpdateDatetimeFixed = getValNoByParam(mappingInfo, "MaxUpdateDatetimeFixed");     // 最大更新日時(確定在庫データ)

            // 排他チェック
            foreach (var targetDic in resultList)
            {
                // データクラスにセット
                Dao.searchResult registInfo = new();
                if (!SetDataClassFromDictionary(targetDic, ConductInfo.FormList.ControlId.List, registInfo))
                {
                    return false;
                }

                registInfo.TargetMonth = ((DateTime)registInfo.LastConfirmedDate).ToString("yyyy/MM"); // 対象年月

                // 在庫確定管理データ
                DateTime? maxDateOfListConfirm = DateTime.TryParse(targetDic[maxUpdateDatetimeConfirm].ToString(), out DateTime outDateConfirm) ? outDateConfirm : null;
                if (!CheckExclusiveStatusByUpdateDatetime(maxDateOfListConfirm, getMaxDate(registInfo, SqlName.GetMaxUpdateDateConfirm)))
                {
                    // 排他エラー
                    return false;
                }

                // 確定在庫データ
                DateTime? maxDateOfListFixed = DateTime.TryParse(targetDic[maxUpdateDatetimeFixed].ToString(), out DateTime outDateFixed) ? outDateFixed : null;
                if (!CheckExclusiveStatusByUpdateDatetime(maxDateOfListFixed, getMaxDate(registInfo, SqlName.GetMaxUpdateDateFixed)))
                {
                    // 排他エラー
                    return false;
                }
            }

            // 排他チェックOK
            return true;

            /// <summary>
            /// パラメータ名と一致する項目番号を返す
            /// </summary>
            /// <param name="info">一覧情報</param>
            /// <param name="keyName">項目キー名</param>
            /// <returns>項目番号</returns>
            string getValNoByParam(MappingInfo info, string keyName)
            {
                // パラメータ名と一致する項目番号を返す
                return info.Value.First(x => x.ParamName.Equals(keyName)).ValName;
            }

            /// <summary>
            /// 最大更新日時を取得
            /// </summary>
            /// <param name="searchCondition">検索条件</param>
            /// <param name="sqlName">sql名称</param>
            /// <returns>最大更新日時</returns>
            DateTime? getMaxDate(Dao.searchResult searchCondition, string sqlName)
            {
                // 最大更新日時取得SQL
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out string outSql);
                // SQL実行
                var maxDateResult = db.GetEntity(outSql, searchCondition);

                if (maxDateResult == null)
                {
                    return null;
                }
                else
                {
                    return maxDateResult.max_update_datetime;
                }
            }
        }
        #endregion

        #region 共通処理
        /// <summary>
        /// 検索条件の対象年月を取得する
        /// </summary>
        /// <param name="isFirstDay">月初の日付を取得する場合はTrue</param>
        /// <returns>入力されている対象年月</returns>
        private string getSearchCondition(bool isFirstDay = false)
        {
            // 検索条件エリアの「対象年月」の値を取得
            Dao.searchCondition param = new();
            Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormList.ControlId.SearchCondition);
            SetDataClassFromDictionary(result, ConductInfo.FormList.ControlId.SearchCondition, param);
            if (isFirstDay)
            {
                return param.TargetMonth.ToString("yyyy/MM/dd");
            }
            else
            {
                return param.TargetMonth.ToString("yyyy/MM");
            }
        }

        /// <summary>
        /// 一覧の行番号を取得する
        /// </summary>
        /// <param name="result">選択されたレコード</param>
        /// <returns>行番号</returns>
        private int getRowNo(Dictionary<string, object> result)
        {
            // 選択されたレコードの行番号を取得する
            foreach (var val in result)
            {
                if (val.Key == "ROWNO")
                {
                    return int.Parse(val.Value.ToString());
                }
            }
            return 0;
        }
        #endregion
        #endregion
    }
}