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
using STDData = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using Dao = BusinessLogic_PT0003.BusinessLogicDataClass_PT0003;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_PT0003
{
    /// <summary>
    /// 予備品管理　棚卸(一覧)
    /// </summary>
    public partial class BusinessLogic_PT0003 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <returns>エラーの場合False</returns>
        private bool searchList(Dao.searchCondition condition, List<string> listUnComment)
        {
            //入力チェック
            if (isErrorSearch(condition))
            {
                return false;
            }

            // 検索SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetInventoryList, out string baseSql, listUnComment);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetInventoryList, out string withSql, listUnComment);

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.List.InventoryList, this.pageInfoList);

            // 総件数取得SQL文の取得
            string execSql = TMQUtil.GetSqlStatementSearch(true, baseSql, null, withSql);

            // 総件数を取得
            //int cnt = db.GetCount(execSql, whereParam);
            int cnt = db.GetCount(execSql, condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            // 一覧検索SQL文の取得
            execSql = TMQUtil.GetSqlStatementSearch(false, baseSql, null, withSql);
            // 検索SQLにORDER BYを追加
            var selectSql = new StringBuilder(execSql);
            selectSql.AppendLine(" ORDER BY parts_location_id, parts_no, old_new_structure_id, department_structure_id, account_structure_id ");

            // 一覧検索実行
            var results = db.GetListByDataClass<Dao.searchInventoryResult>(selectSql.ToString(), condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            //棚番、在庫数の設定
            foreach (var result in results)
            {
                //棚番
                result.PartsLocationDisp = TMQUtil.GetDisplayPartsLocation(result.PartsLocationId, result.PartsLocationDetailNo, result.FactoryId, this.LanguageId, this.db);

                //在庫数 単位結合
                result.StockQuantityDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(result.StockQuantity.ToString(), result.UnitDigit, result.UnitRoundDivision), result.Unit, false);

                //部門コードと名称結合
                result.DepartmentNm = TMQUtil.CombineNumberAndUnit(result.DepartmentCd, result.DepartmentNm, true);
                //勘定科目コードと名称結合
                result.SubjectNm = TMQUtil.CombineNumberAndUnit(result.SubjectCd, result.SubjectNm, true);
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchInventoryResult>(pageInfo, results, cnt, false))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;

            //入力チェック
            bool isErrorSearch(Dao.searchCondition condition)
            {
                // システム日時
                DateTime now = DateTime.Now;
                DateTime nowYearMonth = new DateTime(now.Year, now.Month, 1);

                //対象年月のチェック
                if (nowYearMonth.CompareTo(condition.TargetYearMonth) == 1)
                {
                    //過去年月が指定された場合

                    //SQL文の取得
                    string sql;
                    if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetCountCheckInventory, out sql, listUnComment))
                    {
                        return false;
                    }
                    //対象の棚卸データが存在するかチェック
                    int cnt = db.GetCount(sql, condition);
                    if (cnt <= 0)
                    {
                        //対象の棚卸データが存在しない場合、エラー
                        //棚卸確定前の日付は入力できません。
                        this.MsgId = GetResMessage(ComRes.ID.ID141160002);
                        return true;
                    }

                    return false;
                }
                else if (nowYearMonth.CompareTo(condition.TargetYearMonth) == -1)
                {
                    //未来年月が指定された場合、エラー

                    //未来の日付は入力できません
                    this.MsgId = GetResMessage(ComRes.ID.ID141320001);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 検索条件を取得
        /// </summary>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <returns>検索条件</returns>
        private Dao.searchCondition getCondition(out List<string> listUnComment)
        {
            listUnComment = new List<string>();

            // 検索条件を画面より取得してデータクラスへセット
            // コントロールIDでディクショナリリストを絞り込み
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormList.Condition.SearchCondition);
            //部門の値を取得
            string conditionDepartment = getDictionaryKeyValue(targetDic, KeyName.ConditionDepartment);
            //部門の必須チェック（データクラス変換時にエラーになるためここでチェック）
            if (string.IsNullOrEmpty(conditionDepartment))
            {
                //入力エラーがあります。
                this.MsgId = GetResMessage(ComRes.ID.ID941220005);
                //選択して下さい。
                string errMsg = GetResMessage(ComRes.ID.ID941140002);
                // エラー情報を画面に設定するためのマッピング情報リスト
                var mapping = getResultMappingInfo(ConductInfo.FormList.Condition.SearchCondition);
                // エラー情報格納クラス
                var errorInfo = new ErrorInfo(targetDic);
                string val = mapping.getValName(KeyName.ConditionDepartment); // エラーをセットする項目番号
                errorInfo.setError(errMsg, val); // エラー情報をセット
                // エラー情報セット用Dictionary
                var errorInfoDictionary = new List<Dictionary<string, object>>();
                errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                // ステータス
                this.Status = CommonProcReturn.ProcStatus.Error;
                return null;
            }

            Dao.searchCondition condition = new();
            SetDataClassFromDictionary(targetDic, ConductInfo.FormList.Condition.SearchCondition, condition);
            condition.InventoryIdFlg = false;

            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId);
            if (compareId.IsStartId(ConductInfo.FormUpload.Button.BackUpload))
            {
                //新規取込画面から棚卸IDが渡されている場合、棚卸IDをキーに検索する
                var hiddenInfoDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormUpload.HiddenInfo);
                if (hiddenInfoDic != null)
                {
                    Dao.searchCondition hiddenCondition = new();
                    SetDataClassFromDictionary(hiddenInfoDic, ConductInfo.FormUpload.HiddenInfo, hiddenCondition);
                    if (hiddenCondition.InventoryId != null)
                    {
                        //棚卸ID
                        condition.InventoryIdList = hiddenCondition.InventoryId.Split(",").ToList().ConvertAll(x => Convert.ToInt64(x));
                        condition.InventoryIdFlg = true;
                    }
                    //準備状況は検索条件から外す
                    condition.ReadyStatus = null;
                }
            }

            //言語ID
            condition.LanguageId = this.LanguageId;

            // 対象年月の月末を条件に追加
            var endDate = DateTime.DaysInMonth(year: condition.TargetYearMonth.Year, month: condition.TargetYearMonth.Month);
            condition.TargetYearMonthNext = new DateTime(condition.TargetYearMonth.Year, condition.TargetYearMonth.Month, endDate, 23, 59, 59);

            //予備品倉庫から棚番IDを取得
            condition.PartsLocationIdList = GetLowerStructureIdList(new List<int> { condition.StorageLocationId });

            //場所階層の条件を取得
            setLocationStructureIdList();

            //職種の条件を取得
            setJobStructureIdList();

            // SQLのアンコメントする条件を設定
            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            listUnComment = ComUtil.GetNotNullNameByClass<Dao.searchCondition>(condition);
            //準備状況でどちらか選択されていた場合
            if (condition.ReadyStatus != null)
            {
                //構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                //構成グループID
                param.StructureGroupId = (int)Const.MsStructure.GroupId.InventoryStatus;
                //連番
                param.Seq = ReadyStatus.Seq;

                //棚卸状況の構成アイテム情報取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> readyStatusList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
                //準備状況で選択された構成IDに一致するデータの拡張データを取得
                string status = readyStatusList.Where(x => x.StructureId == condition.ReadyStatus).Select(x => x.ExData).FirstOrDefault();
                listUnComment.Add(status == ReadyStatus.Created ? ReadyStatus.CreatedUncommentKey : ReadyStatus.NotYetUncommentKey);
            }
            return condition;

            //場所階層の条件を取得
            void setLocationStructureIdList()
            {
                //場所階層
                var keyName = STRUCTURE_CONSTANTS.CONDITION_KEY.Location;
                var dic = this.searchConditionDictionary.Where(x => x.ContainsKey(keyName)).FirstOrDefault();
                if (dic != null && dic.ContainsKey(keyName))
                {
                    List<int> locationIdList = dic[keyName] as List<int>;
                    if ((locationIdList == null || locationIdList.Count == 0) &&
                        (this.BelongingInfo.LocationInfoList != null && this.BelongingInfo.LocationInfoList.Count > 0))
                    {
                        // 場所階層の指定なしの場合、所属場所階層を渡す
                        locationIdList = this.BelongingInfo.LocationInfoList.Select(x => x.StructureId).ToList();
                    }
                    // 選択された構成IDリストから配下の構成IDをすべて取得
                    if (locationIdList.Count > 0)
                    {
                        //工場IDを抽出
                        condition.FactoryIdList = GetPartsStructureIdList(locationIdList);
                    }
                }
            }

            //職種の条件を取得
            void setJobStructureIdList()
            {
                //職種
                var keyName =STRUCTURE_CONSTANTS.CONDITION_KEY.Job;
                bool useBelongingInfo = false;
                var dic = this.searchConditionDictionary.Where(x => x.ContainsKey(keyName)).FirstOrDefault();
                if (dic != null && dic.ContainsKey(keyName))
                {
                    // 選択された構成IDリストから配下の構成IDをすべて取得
                    List<int> jobIdList = dic[keyName] as List<int>;
                    if ((jobIdList == null || jobIdList.Count == 0) &&
                        (this.BelongingInfo.JobInfoList != null && this.BelongingInfo.JobInfoList.Count > 0))
                    {
                        // 職種機種の指定なしの場合、所属職種機種を渡す
                        jobIdList = this.BelongingInfo.JobInfoList.Select(x => x.StructureId).ToList();
                        useBelongingInfo = true;
                    }
                    if (jobIdList.Count > 0)
                    {
                        jobIdList = GetLowerStructureIdList(jobIdList);
                        if (!useBelongingInfo && this.BelongingInfo.JobInfoList != null && this.BelongingInfo.JobInfoList.Count > 0)
                        {
                            //職種機種の指定ありの場合

                            //所属職種の配下の構成IDを取得
                            List<int> belongJobIdList = GetLowerStructureIdList(this.BelongingInfo.JobInfoList.Select(x => x.StructureId).ToList());
                            //所属職種と一致するもののみ抽出
                            jobIdList = jobIdList.Where(x => belongJobIdList.Contains(x)).ToList();
                        }
                        condition.JobIdList = jobIdList;
                    }
                }
            }
        }

        /// <summary>
        /// 棚卸準備リスト押下時、棚卸データの登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool registInventory()
        {
            // private変数初期化
            addInventoryIdList = new List<long>();

            // 棚卸一覧の選択行を取得
            List<Dictionary<string, object>> inventoryList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormList.List.InventoryList);
            // 選択行が無ければエラー
            if (inventoryList == null || inventoryList.Count == 0)
            {
                return false;
            }

            // システム日時
            DateTime now = DateTime.Now;
            // 検索条件を画面より取得してデータクラスへセット
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormList.Condition.SearchCondition);
            Dao.searchCondition condition = new();
            SetDataClassFromDictionary(targetDic, ConductInfo.FormList.Condition.SearchCondition, condition);

            foreach (Dictionary<string, object> selectData in inventoryList)
            {
                //データクラスに変換
                Dao.searchInventoryResult inventory = new Dao.searchInventoryResult();
                if (!SetExecuteConditionByDataClass<Dao.searchInventoryResult>(selectData, ConductInfo.FormList.List.InventoryList, inventory, now, this.UserId, this.UserId))
                {
                    return false;
                }

                //棚卸データ作成
                ComDao.PtInventoryEntity registData = setInventoryData(condition, inventory, now);

                // SQL内分岐用のコメントを追加する
                List<string> listUnComment = new List<string>();
                //棚枝番がNullの場合の対応
                if (registData.PartsLocationDetailNo != null)
                {
                    listUnComment.Add(KeyNameReport.PartsLocationDetailNo);
                }
                else
                {
                    listUnComment.Add(KeyNameReport.PartsLocationDetailNoIsNull);
                }

                //棚卸データの存在チェック
                string sql;
                // if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetCountRegistInventory, out sql))
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetCountRegistInventory, out sql, listUnComment))
                {
                        return false;
                }
                //対象の棚卸データが存在するかチェック
                int cnt = db.GetCount(sql, registData);
                if (cnt <= 0)
                {
                    //対象の棚卸データが存在しない場合、登録する(対象年月で初回の情報のみ登録)
                    bool returnFlag = TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long inventoryId, SqlName.InsertInventory, SqlName.SubDir, registData, this.db);
                    if (!returnFlag)
                    {
                        return false;
                    }
                    registData.InventoryId = inventoryId;

                    // 棚卸準備リスト用処理
                    // private変数に追加があった場合、保持する
                    addInventoryIdList.Add(inventoryId);
                }
            }

            // 再検索
            formListSearch();
            return true;

            //棚卸データ作成
            ComDao.PtInventoryEntity setInventoryData(Dao.searchCondition condition, Dao.searchInventoryResult inventory, DateTime now)
            {
                ComDao.PtInventoryEntity registData = new ComDao.PtInventoryEntity();
                //棚卸ID
                registData.InventoryId = inventory.InventoryId ?? -1;
                //対象年月
                registData.TargetMonth = condition.TargetYearMonth;
                //予備品ID
                registData.PartsId = inventory.PartsId;
                //棚ID
                registData.PartsLocationId = inventory.PartsLocationId;
                //棚番
                registData.PartsLocationDetailNo = inventory.PartsLocationDetailNo;
                //新旧区分ID
                registData.OldNewStructureId = inventory.OldNewStructureId;
                //部門ID
                registData.DepartmentStructureId = inventory.DepartmentStructureId;
                //勘定科目ID
                registData.AccountStructureId = inventory.AccountStructureId;
                //在庫数
                registData.StockQuantity = inventory.StockQuantity;
                //数量単位ID
                registData.UnitStructureId = inventory.UnitStructureId;
                //金額単位ID
                registData.CurrencyStructureId = inventory.CurrencyStructureId;
                //棚卸準備日時
                registData.PreparationDatetime = now;
                //作成区分
                registData.CreationDivisionStructureId = getCreationDivisionStructureId((int)Const.MsStructure.StructureId.CreationDivision.Preparation);

                // 更新者ID、登録者IDの変換
                bool chkUpd = int.TryParse(this.UserId, out int updUserId);
                bool chkIns = int.TryParse(this.UserId, out int insUserId);
                // 共通の更新日時などを設定
                setExecuteConditionByDataClassCommon<ComDao.PtInventoryEntity>(ref registData, now, updUserId, insUserId);

                return registData;
            }
        }

        /// <summary>
        /// 作成区分の構成ID取得
        /// </summary>
        /// <param name="extensionData">拡張データ</param>
        /// <returns>構成ID</returns>
        private long? getCreationDivisionStructureId(int extensionData)
        {
            //作成区分の構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = (int)Const.MsStructure.GroupId.Creation;
            //連番
            param.Seq = Creation.Seq;
            //拡張データ
            param.ExData = extensionData.ToString();
            //作成区分の構成アイテム情報取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> creationList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            //拡張データ取得
            long? creationId = creationList.Select(x => x.StructureId).FirstOrDefault();
            return creationId;
        }

        /// <summary>
        /// 棚卸準備取消押下時、棚卸データと棚差調整データ削除処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool deleteInventory()
        {
            // 排他チェック
            if (isErrorInventoryExclusive())
            {
                // 排他エラー
                return false;
            }

            // 入力チェック
            if (isErrorDeleteRow(ConductInfo.FormList.List.InventoryList))
            {
                // 処理が存在し戻り値がFalse(エラー)の場合、入力チェックエラー
                return false;
            }

            //システム日時
            DateTime now = DateTime.Now;

            //選択行の情報
            List<Dao.searchInventoryResult> registList = new List<Dao.searchInventoryResult>();
            //データクラスに変更
            List<Dao.searchInventoryResult> list = getRegistInfoList<Dao.searchInventoryResult>(ConductInfo.FormList.List.InventoryList, now);

            //棚卸準備前のデータ、棚卸確定済のデータが選択されている場合は、処理対象外とする
            registList = list.Where(x => x.InventoryId != null && x.FixedDatetime == null).ToList();

            // 行削除
            foreach (Dao.searchInventoryResult deleteRow in registList)
            {
                //棚卸ID
                long inventoryId = deleteRow.InventoryId ?? -1;
                //棚卸データ削除
                bool result = new ComDao.PtInventoryEntity().DeleteByPrimaryKey(inventoryId, db);
                if (!result)
                {
                    // 削除エラー
                    return false;
                }
                //棚卸調整データ削除
                TMQUtil.SqlExecuteClass.Regist(SqlName.DeleteInventoryDifference, SqlName.SubDir, deleteRow, this.db);
            }

            // 再検索
            formListSearch();
            return true;

            // 行削除エラーチェック処理(エラーの場合True)
            bool isErrorDeleteRow(string listCtrlId)
            {
                // 選択されている行を取得
                List<Dictionary<string, object>> deleteList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormList.List.InventoryList);
                // 削除対象の行を繰り返しチェック
                foreach (var deleteRow in deleteList)
                {
                    Dao.searchInventoryResult row = new();
                    SetDataClassFromDictionary(deleteRow, listCtrlId, row);

                    if (row.FixedDatetime != null)
                    {
                        //棚卸確定日時が設定されている場合
                        // エラーメッセージを設定してTrueを返す
                        this.MsgId = GetResMessage(ComRes.ID.ID141160003);
                        return true;
                    }
                }
                // エラーが無い場合Falseを返す
                return false;
            }
        }

        /// <summary>
        /// ファイルを取込み、入力チェックを行う
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorUpload()
        {
            List<string> outputMessages = new List<string>();

            if (this.InputStream == null)
            {
                // 「取込可能な棚卸準備リストではありません。」
                this.MsgId = GetResMessage(ComRes.ID.ID141200002);
                return true;
            }
            //ファイル情報
            var file = this.InputStream[0];
            // ファイル拡張子チェック
            string extension = Path.GetExtension(file.FileName);
            if (extension != ComUtil.FileExtension.Excel && extension != ComUtil.FileExtension.CSV)
            {
                // 「ファイル形式が有効ではありません。」
                this.MsgId = GetResMessage(ComRes.ID.ID941280004);
                return true;
            }

            // ヘッダ情報の取込
            List<Dao.searchCondition> uploadHeaderList = new List<Dao.searchCondition>();
            List<ComDao.UploadErrorInfo> errorHeaderInfo = new List<ComDao.UploadErrorInfo>();
            CommonExcelCmd cmd = null;
            TMQUtil.UploadText uploadText = null;
            if (extension == ComUtil.FileExtension.Excel)
            {
                //Excel読み込み
                // ファイル読込
                cmd = TMQUtil.FileOpen(file.OpenReadStream());
                errorHeaderInfo = TMQUtil.ComUploadErrorCheck<Dao.searchCondition>(
                    cmd, UploadFile.ReportId, UploadFile.SheetNo, UploadFile.HeaderId, ref uploadHeaderList, this.LanguageId, this.messageResources, this.db);

            }
            else if(extension == ComUtil.FileExtension.CSV)
            {
                //CSV読み込み

                // ファイル読込
                uploadText = new TMQUtil.UploadText(file.OpenReadStream(), this.LanguageId, this.messageResources, this.db);
                errorHeaderInfo = uploadText.ComUploadErrorCheck<Dao.searchCondition>(UploadFile.ReportId, UploadFile.SheetNo, UploadFile.HeaderId, ref uploadHeaderList);
            }
            if (uploadHeaderList == null || uploadHeaderList.Count == 0)
            {
                // ヘッダが取得できない場合
                // 「取込可能な棚卸準備リストではありません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141200002 });
                setResultInfo(null);
                return true;
            }
            //入力チェックで検出されたエラーを出力する
            if (TMQUtil.SetErrorUploadCheckCommon(errorHeaderInfo, ref outputMessages, this.LanguageId, this.messageResources))
            {
                // 「取込可能な棚卸準備リストではありません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141200002 });
                setResultInfo(outputMessages);
                return true;
            }
            //ヘッダ情報のチェック
            if (checkHeader(uploadHeaderList))
            {
                return true;
            }

            // 棚卸情報の取込
            List<Dao.searchInventoryResult> uploadDetailList = new List<Dao.searchInventoryResult>();
            List<ComDao.UploadErrorInfo> errorListInfo = new List<ComDao.UploadErrorInfo>();
            if (extension == ComUtil.FileExtension.Excel)
            {
                //Excel読み込み
                // ファイル読込
                errorListInfo = TMQUtil.ComUploadErrorCheck<Dao.searchInventoryResult>(
                    cmd, UploadFile.ReportId, UploadFile.SheetNo, UploadFile.ListId, ref uploadDetailList, this.LanguageId, this.messageResources, this.db);
            }
            else if (extension == ComUtil.FileExtension.CSV)
            {
                //CSV読み込み
                errorListInfo = uploadText.ComUploadErrorCheck<Dao.searchInventoryResult>(UploadFile.ReportId, UploadFile.SheetNo, UploadFile.ListId, ref uploadDetailList);
            }
            if (uploadDetailList == null || uploadDetailList.Count == 0)
            {
                // 明細が取得できない場合
                // 「取込可能な棚卸データが１件も存在しません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141200003 });
                setResultInfo(null);
                return true;
            }

            //入力チェックで検出されたエラーを出力する
            if (TMQUtil.SetErrorUploadCheckCommon(errorListInfo, ref outputMessages, this.LanguageId, this.messageResources))
            {
                // 「取込可能な棚卸準備リストではありません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141200002 });
                setResultInfo(outputMessages);
                return true;
            }
            //棚卸情報のチェック
            if(checkDetail(uploadDetailList, outputMessages, uploadHeaderList, out List<long> targetInventoryIdList))
            {
                return true;
            }

            //棚卸取込値の登録
            bool isError = registTempInventoryQuantity(uploadDetailList, targetInventoryIdList);

            return isError;

        }
        /// <summary>
        /// エラーメッセージ、非表示項目の設定
        /// </summary>
        /// <param name="outputMessages">エラーメッセージ</param>
        /// <param name="inventoryId">棚卸ID</param>
        /// <param name="departmentId">部門ID</param>
        private void setResultInfo(List<string> outputMessages, string inventoryId = null, string departmentId = null)
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormUpload.Info, this.pageInfoList);
            // エラーメッセージの設定
            Dao.uploadInfo info = new Dao.uploadInfo();
            if (outputMessages != null)
            {
                info.ErrorMessage = string.Join('\n', outputMessages);
            }
            SetSearchResultsByDataClass<Dao.uploadInfo>(pageInfo, new List<Dao.uploadInfo> { info }, 1);

            // ページ情報取得
            pageInfo = GetPageInfo(ConductInfo.FormUpload.HiddenInfo, this.pageInfoList);
            info = new Dao.uploadInfo();
            info.InventoryId = inventoryId;
            info.UploadFlg = inventoryId != null; //取込ボタンの押下可否
            info.DepartmentId = departmentId;
            SetSearchResultsByDataClass<Dao.uploadInfo>(pageInfo, new List<Dao.uploadInfo> { info }, 1);
        }

        /// <summary>
        /// ヘッダーの個別チェックを行う
        /// </summary>
        /// <param name="uploadHeaderList">ヘッダー情報</param>
        /// <returns>エラーの場合true</returns>
        private bool checkHeader(List<Dao.searchCondition> uploadHeaderList)
        {
            // 一覧画面の検索条件の対象年月、予備品倉庫の値を取得
            Dictionary<string, object> dic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormList.Condition.SearchCondition);
            Dao.searchCondition condition = new();
            SetDataClassFromDictionary(dic, ConductInfo.FormList.Condition.SearchCondition, condition);
            //画面の対象年月、予備品倉庫の値と取り込むファイルの値が異なる場合、エラー
            if (uploadHeaderList[0].TargetYearMonth.CompareTo(condition.TargetYearMonth) != 0)
            {
                // 「対象年月が画面に指定された年月と異なります。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160008 });
                setResultInfo(null);
                return true;
            }
            if (uploadHeaderList[0].StorageLocationId != condition.StorageLocationId)
            {
                // 「予備品倉庫が画面に指定された予備品倉庫と異なります。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141380002 });
                setResultInfo(null);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 明細の個別チェックを行う
        /// </summary>
        /// <param name="uploadDetailList">明細情報</param>
        /// <param name="outputMessages">エラーメッセージ</param>
        /// <param name="uploadHeaderList">ヘッダー情報</param>
        /// <param name="targetInventoryIdList">棚卸IDリスト</param>
        /// <returns>エラーの場合true</returns>
        private bool checkDetail(List<Dao.searchInventoryResult> uploadDetailList, List<string> outputMessages, List<Dao.searchCondition> uploadHeaderList, out List<long> targetInventoryIdList)
        {
            targetInventoryIdList = null;

            //棚卸数!=nullかつエラーがない行の棚卸IDを取得
            List<long> inventoryIdList = uploadDetailList.Where(x => x.InventoryQuantity != null && !x.ErrorFlg).Select(x => x.InventoryId ?? -1).ToList();
            if (inventoryIdList.Count == 0)
            {
                // 「取込可能な棚卸データが１件も存在しません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141200003 });
                setResultInfo(outputMessages);
                return true;
            }

            Dao.searchCondition param = new();
            param.InventoryIdList = inventoryIdList;
            //棚卸IDを条件に棚卸データを取得（データの存在チェック）
            List<ComDao.PtInventoryEntity> inventory = TMQUtil.SqlExecuteClass.SelectList<ComDao.PtInventoryEntity>(SqlName.GetExistInventory, SqlName.SubDir, param, this.db);
            if(inventory == null || inventory.Count == 0)
            {
                // 「取込可能な棚卸データが１件も存在しません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141200003 });
                setResultInfo(outputMessages);
                return true;
            }
            //棚卸確定済みのデータを除いた棚卸IDを取得
            List<long> resultList = inventory.Where(x => x.FixedDatetime == null).Select(x => x.InventoryId).ToList();
            if (resultList == null || resultList.Count == 0)
            {
                // 「取込可能な棚卸データが１件も存在しません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141200003 });
                //棚卸確定済みの情報が含まれる場合、エラーメッセージ追加
                outputMessages.Add(GetResMessage(new string[] { ComRes.ID.ID141160009 }));
                setResultInfo(outputMessages);
                return true;
            }
            if (inventory.Count != resultList.Count)
            {
                // 「取込可能な棚卸準備リストではありません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141200002 });
                //棚卸確定済みの情報が含まれる場合、エラーメッセージ追加
                outputMessages.Add(GetResMessage(new string[] { ComRes.ID.ID141160009 }));
                setResultInfo(outputMessages);
                return true;
            }

            //正常取込できる場合
            //一覧画面の検索対象とするため、棚卸IDを非表示項目に設定
            setResultInfo(outputMessages, string.Join(',', resultList), uploadHeaderList[0].DepartmentId);

            targetInventoryIdList = resultList;

            return false;
        }

        /// <summary>
        /// 棚卸データの棚卸取込値に取り込んだ棚卸数を設定
        /// </summary>
        /// <param name="uploadDetailList">明細情報</param>
        /// <param name="targetInventoryIdList">取込対象の棚卸IDリスト</param>
        /// <returns>エラーの場合true</returns>
        private bool registTempInventoryQuantity(List<Dao.searchInventoryResult> uploadDetailList, List<long> targetInventoryIdList)
        {
            if(targetInventoryIdList == null || targetInventoryIdList.Count == 0)
            {
                //対象データが無い場合、終了
                return false;
            }
            bool chkUpd = int.TryParse(this.UserId, out int updUserId);
            List<Dao.searchInventoryResult> targetList = uploadDetailList.Where(x => x.InventoryId != null && targetInventoryIdList.IndexOf(x.InventoryId ?? -1) >= 0 && x.InventoryQuantity != null).ToList();
            foreach(Dao.searchInventoryResult target in targetList)
            {
                Dao.searchInventoryResult param = new Dao.searchInventoryResult();
                param.InventoryId = target.InventoryId;
                param.InventoryQuantity = target.InventoryQuantity;
                // 共通の更新日時などを設定
                setExecuteConditionByDataClassCommon<Dao.searchInventoryResult>(ref param, DateTime.Now, updUserId, -1);
                //棚卸データの棚卸取込値に取り込んだ棚卸数を設定
                bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.UpdateTempInventoryQuantity, SqlName.SubDir, param, db);
                if (!returnFlag)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 一覧画面　一時登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistInventoryQuantity()
        {
            // 排他チェック
            if (isErrorInventoryExclusive())
            {
                return false;
            }
            //システム日時
            DateTime now = DateTime.Now;
            // 入力チェック
            if (isErrorRegistInventory(now))
            {
                return false;
            }

            //データクラスに変更
            List<Dao.searchInventoryResult> list = getRegistInfoList<Dao.searchInventoryResult>(ConductInfo.FormList.List.InventoryList, now);
            //棚卸準備前のデータ、棚卸確定済のデータが選択されている場合は、エラーとせず処理対象外とする
            List<Dao.searchInventoryResult> registList = list.Where(x => x.InventoryId != null && x.FixedDatetime == null).ToList();

            // 登録
            if (!registInventory(registList, now))
            {
                return false;
            }

            if(list.Count() != registList.Count())
            {
                //棚卸準備前・確定済みのデータを除き、{0}件一時登録しました。
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160012, registList.Count().ToString(), ComRes.ID.ID111020023 });
            }

            // 再検索
            formListSearch();
            return true;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorInventoryExclusive()
        {
            // 棚卸一覧の排他チェック
            List<Dictionary<string, object>> list = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormList.List.InventoryList);
            foreach(Dictionary<string, object> row in list)
            {
                //棚卸ID
                string inventoryId = getDictionaryKeyValue(row, KeyName.InventoryId);
                if (!string.IsNullOrEmpty(inventoryId))
                {
                    //棚卸IDがセットされている行（棚卸準備リスト出力済み=棚卸データ登録済みの行）のみ排他チェック
                    if (!checkExclusiveSingle(ConductInfo.FormList.List.InventoryList, row))
                    {
                        // エラーの場合
                        return true;
                    }
                }
            }

            // 排他チェックOK
            return false;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(リスト)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="ctrlId">取得するコントロールID</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラスのリスト</returns>
        private List<T> getRegistInfoList<T>(string ctrlId, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 選択行を取得
            List<Dictionary<string, object>> resultList = getSelectedRowsByList(this.resultInfoDictionary, ctrlId);
            // 戻り値となるデータクラスのリスト
            List<T> registInfoList = new();
            // 一覧を繰り返し、データクラスに変換、リストへ追加する
            foreach (var resultRow in resultList)
            {
                T registInfo = new();
                if (!SetExecuteConditionByDataClass<T>(resultRow, ctrlId, registInfo, now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return registInfoList;
                }
                registInfoList.Add(registInfo);
            }
            return registInfoList;
        }

        /// <summary>
        /// 一時登録 入力チェック
        /// </summary>
        /// <param name="now">システム日時</param>
        /// <returns>入力チェックエラーがある場合True</returns>
        private bool isErrorRegistInventory(DateTime now)
        {
            //横方向一覧の直接入力は共通側でチェック不可のため業務側で行う
            //選択行のみ取得
            List<Dictionary<string, object>> selectList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormList.List.InventoryList);
            if (selectList == null || selectList.Count == 0)
            {
                //データがない場合、終了
                return true;
            }
            bool error = false;
            int rowNo = 0;
            decimal val = 0;
            List<int> checkDecimalInventoryQuantity = new List<int>();
            List<int> checkMinMaxInventoryQuantity = new List<int>();
            List<Dao.checkUnitDigit> checkLengthInventoryQuantity = new List<Dao.checkUnitDigit>();
            //棚卸一覧の全データ取得
            List<Dictionary<string, object>> inventoryList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormList.List.InventoryList);
            foreach (var row in inventoryList)
            {
                rowNo = rowNo + 1;
                // 選択行のみ対象
                if (!ComUtil.IsSelectedRowDictionary(row))
                {
                    continue;
                }
                //棚卸数
                string inventoryQuantity = getDictionaryKeyValue(row, KeyName.InventoryQuantity);
                if (!string.IsNullOrEmpty(inventoryQuantity))
                {
                    //数値チェック
                    if (!decimal.TryParse(inventoryQuantity, out val))
                    {
                        checkDecimalInventoryQuantity.Add(rowNo);
                        continue;
                    }
                    //上下値チェック
                    if(decimal.Parse(inventoryQuantity) < 0 || decimal.Parse(inventoryQuantity) > 9999999999.99m)
                    {
                        checkMinMaxInventoryQuantity.Add(rowNo);
                        continue;
                    }

                    //小数点以下桁数チェック（数量管理単位によって小数点以下桁数は異なる）
                    //データクラスに変換
                    Dao.searchInventoryResult info = new Dao.searchInventoryResult();
                    SetExecuteConditionByDataClass<Dao.searchInventoryResult>(row, ConductInfo.FormList.List.InventoryList, info, now, this.UserId, this.UserId);
                    //整数部、小数部を分割
                    string[] values = info.InventoryQuantity.ToString().Split(".");
                    if(values.Length > 1)
                    {
                        //小数点以下の入力がある場合
                        if(values[1].Length > info.UnitDigit)
                        {
                            //小数点以下桁数が同じものが設定されているかチェック(メッセージはエラーの種類ごとにまとめて表示する為)
                            int cnt = checkLengthInventoryQuantity.Where(x => x.UnitDigit == info.UnitDigit).Count();
                            if(cnt > 0)
                            {
                                //対象のインデックス番号を取得
                                int index = checkLengthInventoryQuantity.Select((e, index) => (e, index)).Where(x => x.e.UnitDigit == info.UnitDigit).Select(x => x.index).FirstOrDefault();
                                //行番号を追加設定
                                checkLengthInventoryQuantity[index].RowNo.Add(rowNo);
                            }
                            else
                            {
                                //数量管理単位毎にエラーメッセージをまとめて表示する
                                Dao.checkUnitDigit checkInfo = new Dao.checkUnitDigit();
                                checkInfo.UnitDigit = info.UnitDigit;
                                checkInfo.RowNo = new List<int>() { rowNo };
                                checkLengthInventoryQuantity.Add(checkInfo);
                            }
                        }
                    }

                }
            }

            // 棚卸数数値チェック
            if (checkDecimalInventoryQuantity.Count > 0)
            {
                string errRow = string.Join(",", checkDecimalInventoryQuantity);
                // 棚卸数:数値で入力してください
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + GetResMessage(new string[] { ComRes.ID.ID941100001, ComRes.ID.ID111160042, GetResMessage(new string[] { ComRes.ID.ID941190002, ComRes.ID.ID911130002 }) }) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                }
                else
                {
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941100001, ComRes.ID.ID111160042, GetResMessage(new string[] { ComRes.ID.ID941190002, ComRes.ID.ID911130002 }) }) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                }
                error = true;
            }

            // 棚卸数上下限チェック
            if (checkMinMaxInventoryQuantity.Count > 0)
            {
                string errRow = string.Join(",", checkMinMaxInventoryQuantity);
                // 棚卸数:{0}から{1}の範囲で入力して下さい。
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + GetResMessage(new string[] { ComRes.ID.ID941100001, ComRes.ID.ID111160042, GetResMessage(new string[] { ComRes.ID.ID941060015, "0", "9999999999.99" }) }) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                }
                else
                {
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941100001, ComRes.ID.ID111160042, GetResMessage(new string[] { ComRes.ID.ID941060015, "0", "9999999999.99" }) }) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                }
                error = true;
            }

            // 棚卸数小数点以下桁数チェック
            if (checkLengthInventoryQuantity.Count > 0)
            {
                foreach(Dao.checkUnitDigit checkInfo in checkLengthInventoryQuantity)
                {
                    string errRow = string.Join(",", checkInfo.RowNo);
                    // 棚卸数は小数部{2}桁以下で入力してください。
                    if (this.MsgId.Length > 0)
                    {
                        this.MsgId = this.MsgId + Environment.NewLine + GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111160042, ComRes.ID.ID911120019, checkInfo.UnitDigit.ToString() }) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                    }
                    else
                    {
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111160042, ComRes.ID.ID911120019, checkInfo.UnitDigit.ToString() }) + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")";
                    }
                }
                error = true;
            }
            return error;
        }

        /// <summary>
        /// 一時登録 棚卸データ更新
        /// </summary>
        /// <param name="registList">登録データ</param>
        /// <param name="now">システム日時</param>
        /// <returns>エラーの場合False</returns>
        private bool registInventory(List<Dao.searchInventoryResult> registList, DateTime now)
        {
            bool chkUpd = int.TryParse(this.UserId, out int updUserId);
            foreach (Dao.searchInventoryResult info in registList)
            {
                //棚卸実施日時
                info.InventoryDatetime = now;
                // 共通の更新日時などを設定
                info.UpdateDatetime = now;
                info.UpdateUserId = updUserId;
                // 更新SQL実行
                bool result = TMQUtil.SqlExecuteClass.Regist(SqlName.UpdateInventoryQuantity, SqlName.SubDir, info, this.db);
                if (!result)
                {
                    return false;
                }
                //棚卸調整データ削除（棚差調整後に一時登録した場合は、再度棚差調整を行ってもらうため削除する）
                TMQUtil.SqlExecuteClass.Regist(SqlName.DeleteInventoryDifference, SqlName.SubDir, info, this.db);
            }
            return true;
        }

        /// <summary>
        /// 一覧画面　棚差調整
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistInventoryDifference()
        {
            // 排他チェック
            if (isErrorInventoryExclusive())
            {
                return false;
            }
            //システム日時
            DateTime now = DateTime.Now;

            //データクラスに変更
            List<Dao.searchInventoryResult> list = getRegistInfoList<Dao.searchInventoryResult>(ConductInfo.FormList.List.InventoryList, now);
            //棚卸準備前のデータ、棚卸確定済のデータ、棚卸数が未入力のデータが選択されている場合は、エラーとせず処理対象外とする
            List<Dao.searchInventoryResult> registList = list.Where(x => x.InventoryId != null && x.FixedDatetime == null && x.InventoryQuantity != null).ToList();

            // 登録
            if (!registInventoryDifference(registList, now))
            {
                return false;
            }

            //棚卸準備前・確定済みのデータを除き、{0}件棚差調整しました。
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160012, registList.Count().ToString(), ComRes.ID.ID111160021 });

            // 再検索
            formListSearch();
            return true;
        }

        /// <summary>
        /// 棚差調整 棚卸調整データ登録
        /// </summary>
        /// <param name="registList">登録データ</param>
        /// <param name="now">システム日時</param>
        /// <returns>エラーの場合False</returns>
        private bool registInventoryDifference(List<Dao.searchInventoryResult> registList, DateTime now)
        {
            // 検索条件を画面より取得してデータクラスへセット
            Dao.searchCondition condition = getCondition(out List<string> listUnComment);

            foreach (Dao.searchInventoryResult info in registList)
            {
                //棚差調整入出庫データを作成・登録
                if (!registInventoryDifferenceInout(info, condition, now))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 棚差調整入出庫データを作成・登録
        /// </summary>
        /// <param name="info">対象データ</param>
        /// <param name="condition">画面の検索条件</param>
        /// <param name="now">システム日時</param>
        /// <returns>エラーの場合False</returns>
        private bool registInventoryDifferenceInout(Dao.searchInventoryResult info, Dao.searchCondition condition, DateTime now)
        {
            //棚卸データの情報取得
            ComDao.PtInventoryEntity inventory = new ComDao.PtInventoryEntity().GetEntity(info.InventoryId ?? 0, this.db);
            if (info.InventoryDiff == 0 && inventory.DifferenceDatetime != null)
            {
                //棚差=0かつ2回目以降の棚差調整の場合、処理なし
                return true;
            }

            // 共通の更新日時などを設定
            bool chkUpd = int.TryParse(this.UserId, out int updUserId);
            setExecuteConditionByDataClassCommon<Dao.searchInventoryResult>(ref info, now, updUserId, -1);

            bool result = false;

            if (info.InventoryDiff > 0)
            {
                //棚差調整出庫
                //棚差分の棚差調整出庫データを作成

                //棚差調整データ削除
                TMQUtil.SqlExecuteClass.Regist(SqlName.DeleteInventoryDifference, SqlName.SubDir, info, this.db);

                //棚卸調整データの作成＆登録
                if (!registOutDifference(info, condition, inventory, now))
                {
                    return false;
                }
            }
            else if(info.InventoryDiff < 0)
            {
                //棚差調整入庫
                //棚差調整入庫データを1件作成

                //棚差調整データ削除
                TMQUtil.SqlExecuteClass.Regist(SqlName.DeleteInventoryDifference, SqlName.SubDir, info, this.db);

                //棚卸調整データの作成
                ComDao.PtInventoryDifferenceEntity difference = createInDifference(info, condition, inventory);
                //登録
                result = TMQUtil.SqlExecuteClass.Regist(SqlName.InsertInventoryDifference, SqlName.SubDir, difference, this.db);
                if (!result)
                {
                    return false;
                }
            }

            //棚卸データの棚卸調整日時を更新
            info.DifferenceDatetime = now;
            // 更新SQL実行
            result = TMQUtil.SqlExecuteClass.Regist(SqlName.UpdateDifferenceDatetime, SqlName.SubDir, info, this.db);
            if (!result)
            {
                return false;
            }
            return true;
            //棚差調整入庫の棚卸調整データの作成
            ComDao.PtInventoryDifferenceEntity createInDifference(Dao.searchInventoryResult info, Dao.searchCondition condition, ComDao.PtInventoryEntity inventory)
            {
                ComDao.PtInventoryDifferenceEntity difference = new ComDao.PtInventoryDifferenceEntity();
                //棚卸ID
                difference.InventoryId = inventory.InventoryId;
                //受払区分：受入
                difference.InoutDivisionStructureId = getStrucutureId((int)Const.MsStructure.GroupId.InoutDivision, InoutDivision.Seq, ((int)Const.MsStructure.StructureId.InoutDivision.In).ToString());
                //作業区分：棚卸入庫
                difference.WorkDivisionStructureId = getStrucutureId((int)Const.MsStructure.GroupId.WorkDivision, WorkDivision.Seq, ((int)Const.MsStructure.StructureId.WorkDivision.InventoryIn).ToString());
                //部門ID
                difference.DepartmentStructureId = inventory.DepartmentStructureId;
                //勘定科目ID
                difference.AccountStructureId = inventory.AccountStructureId;
                //受払日時：対象年月の末日23:59:59
                DateTime time = DateTime.Parse(lastTime);
                difference.InoutDatetime = DateTime.ParseExact(condition.TargetYearMonthNext.ToString("yyyyMMdd") + " " + time.ToString("HH:mm:ss"), "yyyyMMdd HH:mm:ss", null);
                //受払数 在庫数－棚卸数＋棚差調整数=棚差
                difference.InoutQuantity = Math.Abs(info.InventoryDiff ?? 0);
                //入庫単価
                ComDao.PtPartsEntity parts = new ComDao.PtPartsEntity();
                parts = parts.GetEntity(inventory.PartsId, this.db);
                difference.UnitPrice = parts.UnitPrice;

                // 更新者ID、登録者IDの変換
                bool chkUpd = int.TryParse(this.UserId, out int updUserId);
                bool chkIns = int.TryParse(this.UserId, out int insUserId);
                // 共通の更新日時などを設定
                setExecuteConditionByDataClassCommon<ComDao.PtInventoryDifferenceEntity>(ref difference, now, updUserId, insUserId);

                return difference;
            }
        }

        /// <summary>
        /// 棚差調整出庫の棚卸調整データの作成＆登録
        /// </summary>
        /// <param name="info">対象データ</param>
        /// <param name="condition">画面の検索条件</param>
        /// <param name="inventory">棚卸データ</param>
        /// <param name="now">システム日時</param>
        /// <returns>エラーの場合False</returns>
        private bool registOutDifference(Dao.searchInventoryResult info, Dao.searchCondition condition, ComDao.PtInventoryEntity inventory, DateTime now)
        {
            //棚差（棚差調整出庫数）
            decimal count = Math.Abs(info.InventoryDiff ?? 0);

            //ロット情報、在庫データの取得
            List<Dao.lotStockInfo> lotStockList = TMQUtil.SqlExecuteClass.SelectList<Dao.lotStockInfo>(SqlName.GetLotStock, SqlName.SubDir, inventory, this.db);
            foreach (Dao.lotStockInfo lotStock in lotStockList)
            {
                if (lotStock.StockQuantity == 0)
                {
                    //在庫数=0の場合、スキップ
                    continue;
                }
                ComDao.PtInventoryDifferenceEntity difference = new ComDao.PtInventoryDifferenceEntity();

                if (count >= lotStock.StockQuantity)
                {
                    //出庫数≧在庫数の場合、在庫数MAX分引当

                    //受払数
                    difference.InoutQuantity = lotStock.StockQuantity;
                    //残りの出庫数
                    count = count - lotStock.StockQuantity;
                }
                else
                {
                    //出庫数＜在庫数の場合、出庫数分引当

                    //受払数
                    difference.InoutQuantity = count;
                    //残りの出庫数は0
                    count = 0;
                }
                //棚卸ID
                difference.InventoryId = inventory.InventoryId;
                //受払区分：払出
                difference.InoutDivisionStructureId = getStrucutureId((int)Const.MsStructure.GroupId.InoutDivision, InoutDivision.Seq, ((int)Const.MsStructure.StructureId.InoutDivision.Out).ToString());
                //作業区分：棚卸出庫
                difference.WorkDivisionStructureId = getStrucutureId((int)Const.MsStructure.GroupId.WorkDivision, WorkDivision.Seq, ((int)Const.MsStructure.StructureId.WorkDivision.InventoryOut).ToString());
                //ロット管理ID
                difference.LotControlId = lotStock.LotControlId;
                //在庫管理ID
                difference.InventoryControlId = lotStock.InventoryControlId;
                //部門ID
                difference.DepartmentStructureId = lotStock.DepartmentStructureId;
                //勘定科目ID
                difference.AccountStructureId = lotStock.AccountStructureId;
                //管理区分
                difference.ManagementDivision = lotStock.ManagementDivision;
                //管理No
                difference.ManagementNo = lotStock.ManagementNo;
                //受払日時：対象年月の末日23:59:59
                DateTime time = DateTime.Parse(lastTime);
                difference.InoutDatetime = DateTime.ParseExact(condition.TargetYearMonthNext.ToString("yyyyMMdd") + " " + time.ToString("HH:mm:ss"), "yyyyMMdd HH:mm:ss", null);
                //出庫区分：通常
                difference.CreationDivisionStructureId = getStrucutureId((int)Const.MsStructure.GroupId.IssueDivision, IssueDivision.Seq, ((int)Const.MsStructure.StructureId.IssueDivision.Generally).ToString());
                //入庫単価
                difference.UnitPrice = lotStock.UnitPrice;

                // 更新者ID、登録者IDの変換
                bool chkUpd = int.TryParse(this.UserId, out int updUserId);
                bool chkIns = int.TryParse(this.UserId, out int insUserId);
                // 共通の更新日時などを設定
                setExecuteConditionByDataClassCommon<ComDao.PtInventoryDifferenceEntity>(ref difference, now, updUserId, insUserId);

                //登録
                bool result = TMQUtil.SqlExecuteClass.Regist(SqlName.InsertInventoryDifference, SqlName.SubDir, difference, this.db);
                if (!result)
                {
                    return false;
                }

                if (count == 0)
                {
                    //出庫数が0になれば引当終了
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// 拡張データより構成IDを取得
        /// </summary>
        /// <param name="groupId">構成グループID</param>
        /// <param name="seq">連番</param>
        /// <param name="extensionData">拡張データ</param>
        /// <returns>構成ID</returns>
        private long getStrucutureId(int groupId, int seq, string extensionData)
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = groupId;
            //連番
            param.Seq = seq;
            //拡張データ
            param.ExData = extensionData;

            //構成アイテム情報取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> inoutDivisionList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            //構成IDを戻す
            return inoutDivisionList.Select(x => x.StructureId).FirstOrDefault();
        }

        /// <summary>
        /// 一覧画面　棚卸確定
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeConfirmInventory()
        {
            // 排他チェック
            if (isErrorInventoryExclusive())
            {
                return false;
            }
            //システム日時
            DateTime now = DateTime.Now;

            //選択行の情報
            List<Dao.searchInventoryResult> list = getRegistInfoList<Dao.searchInventoryResult>(ConductInfo.FormList.List.InventoryList, now);
            //棚卸準備前のデータ、棚卸確定済のデータ、棚差調整前（棚差=0の場合は調整未実施でもOK）のデータが選択されている場合は、エラーとせず処理対象外とする
            List<Dao.searchInventoryResult> registList = list.Where(x => x.InventoryId != null && x.FixedDatetime == null && (x.DifferenceDatetime != null || (x.InventoryDiff == 0 && x.DifferenceDatetime == null))).ToList();

            // 登録
            if (!confirmInventory(registList, now))
            {
                return false;
            }

            //棚卸準備前・棚差調整前・確定済みのデータを除き、{0}件棚卸確定しました。
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160013, registList.Count().ToString() });

            // 再検索
            formListSearch();
            return true;
        }

        /// <summary>
        /// 棚卸確定 棚差調整情報を受払履歴、ロット情報、在庫データに反映する
        /// </summary>
        /// <param name="registList">選択行データ</param>
        /// <param name="now">システム日時</param>
        /// <returns>エラーの場合False</returns>
        private bool confirmInventory(List<Dao.searchInventoryResult> registList, DateTime now)
        {
            // 検索条件を画面より取得してデータクラスへセット
            Dao.searchCondition condition = getCondition(out List<string> listUnComment);

            // クラスの宣言
            TMQUtil.PartsInventory.TakeInventory takeInventory = new(this.db, this.UserId, this.LanguageId);

            foreach (Dao.searchInventoryResult regist in registList)
            {
                //受払区分＝払出の調整データを取得
                ComDao.PtInventoryDifferenceEntity param = new ComDao.PtInventoryDifferenceEntity();
                param.InventoryId = regist.InventoryId ?? 0;
                //受払区分（払出）の構成ID
                param.InoutDivisionStructureId = getStrucutureId((int)Const.MsStructure.GroupId.InoutDivision, InoutDivision.Seq, ((int)Const.MsStructure.StructureId.InoutDivision.Out).ToString());
                //棚差調整データの受払数取得
                decimal? inoutQuantity = TMQUtil.SqlExecuteClass.SelectEntity<decimal?>(SqlName.GetInoutQuantity, SqlName.SubDir, param, this.db);
                if (inoutQuantity != null && inoutQuantity > 0)
                {
                    //棚差調整出庫データを再作成（調整から確定の間に引当元の在庫管理IDの情報が変更されている場合を考慮し、再度引当を行う）

                    //棚卸データの情報取得
                    ComDao.PtInventoryEntity inventory = new ComDao.PtInventoryEntity().GetEntity(regist.InventoryId ?? 0, this.db);

                    //棚差調整データ削除
                    TMQUtil.SqlExecuteClass.Regist(SqlName.DeleteInventoryDifference, SqlName.SubDir, regist, this.db);

                    //棚差調整データの受払数の合計値を棚差として設定（再度引当を行う為）
                    regist.InventoryDiff = inoutQuantity;

                    //棚卸調整データの作成＆登録
                    if (!registOutDifference(regist, condition, inventory, now))
                    {
                        return false;
                    }
                }

                // 登録
                if (!takeInventory.Confirm(regist.InventoryId ?? 0))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 一覧画面　棚卸確定解除
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeCancelConfirmInventory()
        {
            // 排他チェック
            if (isErrorInventoryExclusive())
            {
                return false;
            }
            //システム日時
            DateTime now = DateTime.Now;

            //選択行の情報
            List<Dao.searchInventoryResult> list = getRegistInfoList<Dao.searchInventoryResult>(ConductInfo.FormList.List.InventoryList, now);
            //棚卸未確定のデータが選択されている場合は、エラーとせず処理対象外とする
            List<Dao.searchInventoryResult> registList = list.Where(x => x.InventoryId != null && x.FixedDatetime != null).ToList();

            // クラスの宣言
            TMQUtil.PartsInventory.TakeInventory takeInventory = new(this.db, this.UserId, this.LanguageId);

            foreach(Dao.searchInventoryResult regist in registList)
            {
                // 登録
                if (!takeInventory.ConfirmCancel(regist.InventoryId ?? 0))
                {
                    return false;
                }
            }

            //棚卸未確定のデータを除き、{0}件棚卸確定解除しました。
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141160014, registList.Count().ToString() });

            // 再検索
            formListSearch();
            return true;
        }
    }
}
