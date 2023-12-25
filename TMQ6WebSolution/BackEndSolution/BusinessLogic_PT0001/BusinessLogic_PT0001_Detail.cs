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
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_PT0001.BusinessLogicDataClass_PT0001;
using DbTransaction = System.Data.IDbTransaction;
using FunctionTypeId = CommonTMQUtil.CommonTMQConstants.Attachment.FunctionTypeId;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQUtilDao = CommonTMQUtil.CommonTMQUtilDataClass;

namespace BusinessLogic_PT0001
{
    /// <summary>
    /// 詳細画面
    /// </summary>
    public partial class BusinessLogic_PT0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 詳細画面の表示種類
        /// </summary>
        private enum DetailDispType
        {
            /// <summary>初期表示</summary>
            Init,
            /// <summary>検索(ボタン処理後の再表示)</summary>
            Search,
            /// <summary>再表示(入出庫履歴タブの再表示ボタン)</summary>
            Redisplay,
            /// <summary>新規登録後</summary>
            AfterRegist
        }

        /// <summary>
        /// SQLで扱うことのできる年の最大値
        /// </summary>
        private const string SqlMaxYear = "9999";

        /// <summary>
        /// 入出庫履歴タブの表示年度を保持するためのキー名称
        /// </summary>
        private class DispYearKeyName
        {
            /// <summary>
            /// 表示年度(From)
            /// </summary>
            public const string YearFrom = "YearFrom";
            /// <summary>
            /// 表示年度(To)
            /// </summary>
            public const string YearTo = "YearTo";
        }

        /// <summary>
        /// 詳細画面 検索処理
        /// </summary>
        /// <param name="detailType">画面の表示種類</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchDetailList(DetailDispType detailType)
        {
            // 検索条件を作成
            Dao.detailSearchCondition condition = getDetailSearchCondition(judgeDetailPtn());

            // 予備品情報
            if (!searchPartsInfo(ref condition, ConductInfo.FormDetail.GroupNo))
            {
                return false;
            }

            // 棚別在庫一覧
            if (!searchTabInventoryList<Dao.inventoryList>(condition, SqlName.Detail.GetInventoryParentList, SqlName.Detail.GetInventoryChildList, ConductInfo.FormDetail.ControlId.InventoryParentList, ConductInfo.FormDetail.ControlId.InventoryChildList, true))
            {
                return false;
            }

            // 部門別在庫一覧
            if (!searchTabInventoryList<Dao.categoryList>(condition, SqlName.Detail.GetCategoryParentList, SqlName.Detail.GetCategoryChildList, ConductInfo.FormDetail.ControlId.CategoryParentList, ConductInfo.FormDetail.ControlId.CategoryChildList, false))
            {
                return false;
            }

            // 表示年度 初期表示
            if (!setDispYear(ref condition, detailType == DetailDispType.Redisplay))
            {
                return false;
            }

            // 入出庫履歴一覧
            if (!searchInoutHistoryInfo(condition))
            {
                return false;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;

            string judgeDetailPtn()
            {
                // 表示種類で値を取得するコントロールIDを判定
                switch (detailType)
                {
                    case DetailDispType.Init:        // 初期表示
                        return ConductInfo.FormList.ControlId.List;

                    case DetailDispType.Search:      // ボタン処理後
                    case DetailDispType.Redisplay:   // 詳細画面 再表示ボタン
                        return ConductInfo.FormDetail.ControlId.PartsInfo;

                    case DetailDispType.AfterRegist: // 詳細編集画面 登録後
                        return ConductInfo.FormEdit.ControlId.PartsInfo;

                    default:
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// 予備品情報 検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="groupNo">値を取得する一覧のグループ番号</param>
        /// <param name="isInitCopy">複写時の初期検索かどうか</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchPartsInfo(ref Dao.detailSearchCondition condition, short groupNo, bool isInitCopy = false)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetDetailPartsInfo, out string executeSql);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.List.GetPartsList, out string withSql);

            // SQL実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(withSql + executeSql, condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 検索条件に工場IDを設定
            condition.UserFactoryId = results[0].PartsFactoryId;

            if ((int)results[0].FactoryId > 0 && (int)results[0].JobStructureId > 0)
            {
                //URL直接起動時、参照データの権限チェック
                if (!CheckAccessUserBelong((int)results[0].FactoryId, (int)results[0].JobStructureId))
                {
                    return false;
                }
            }

            // 職種IDを非表示に項目に退避
            results[0].JobId = int.Parse(results[0].JobStructureId.ToString());

            // 丸め処理・数量と単位を結合
            results.ToList().ForEach(x => x.JoinStrAndRound());

            List<string> ctrlIdList = getResultMappingInfoByGrpNo(groupNo).CtrlIdList;
            IList<Dao.searchResult> result = new List<Dao.searchResult> { results[0] };
            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref result, new List<StructureType> { StructureType.Job, StructureType.SpareLocation }, this.db, this.LanguageId, true); // 職種、予備品場所階層

            // 棚番の結合文字を取得し、棚番と枝番を結合する
            if (!string.IsNullOrEmpty(results[0].RackName) && !string.IsNullOrEmpty(results[0].PartsLocationDetailNo))
            {
                string[] rack = results[0].RackName.Split("|");
                results[0].RackName = TMQUtil.GetDisplayPartsLocation(rack[0], results[0].PartsLocationDetailNo, (int)result[0].FactoryId, this.LanguageId, this.db) + "|" + rack[1];
            }

            // 予備品情報を取得
            ComDao.PtPartsEntity judgeRequired = new ComDao.PtPartsEntity().GetEntity(condition.PartsId, this.db);

            // 予備品情報に「標準棚番」「標準部門」「標準勘定科目」が登録されているか判定
            if (judgeRequired.LocationRackStructureId != null &&
                judgeRequired.DepartmentStructureId != null &&
                judgeRequired.AccountStructureId != null)
            {
                // すべて登録されている場合はTrue
                results[0].IsRegistedRequiredItemToOutLabel = true;
            }
            else
            {
                // いずれかが登録されていない場合はFalse
                results[0].IsRegistedRequiredItemToOutLabel = false;
            }

            // 複写時の初期検索では予備品Noを表示する必要はないため空にする
            if (isInitCopy)
            {
                results[0].PartsNo = string.Empty;
            }

            // 詳細画面→詳細編集画面遷移してきている場合はグローバルリストに表示年度(From・To)が格納されているので画面の非表示項目に設定する
            // ※画面の非表示項目に設定しておかないと予備品詳細画面に戻った際に値が保持されていないため
            // グローバルリストに表示年度の値を保持しているか判定
            if (this.IndividualDictionary.ContainsKey(DispYearKeyName.YearFrom) || this.IndividualDictionary.ContainsKey(DispYearKeyName.YearTo))
            {
                results[0].DispYearFrom = this.IndividualDictionary[DispYearKeyName.YearFrom].ToString(); // 表示年度(From)
                results[0].DispYearTo = this.IndividualDictionary[DispYearKeyName.YearTo].ToString();     // 表示年度(To)
            }

            // 一覧に対して繰り返し値を設定する
            foreach (var ctrlId in ctrlIdList)
            {
                // 画面項目に値を設定
                if (!SetFormByDataClass(ctrlId, result))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 親子一覧(棚別在庫一覧、部門別在庫一覧) 検索処理
        /// </summary>
        /// <typeparam name="T">検索結果一覧のデータクラス</typeparam>
        /// <param name="condition">検索条件</param>
        /// <param name="parentSqlName">親一覧のSQL</param>
        /// <param name="childSqlName">子一覧のSQL</param>
        /// <param name="parentCtrlId">親一覧のコントロールID</param>
        /// <param name="childCtrlId">子一覧のコントロールID</param>
        /// <returns>エラーの場合False</returns>
        private bool searchTabInventoryList<T>(Dao.detailSearchCondition condition, string parentSqlName, string childSqlName, string parentCtrlId, string childCtrlId, bool isInventory)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, parentSqlName, out string parentSql);
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, childSqlName, out string childSql);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.Common, out string withSql);

            // SQL実行
            IList<T> resultParent = db.GetListByDataClass<T>(withSql + parentSql, condition);
            IList<T> resultChild = db.GetListByDataClass<T>(withSql + childSql, condition);

            // ページ情報取得
            var pageInfoParent = GetPageInfo(parentCtrlId, this.pageInfoList);
            var pageInfoChild = GetPageInfo(childCtrlId, this.pageInfoList);

            // 数量と単位を結合する
            combineNumAndUnit(resultParent, resultChild, isInventory);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<T>(pageInfoParent, resultParent, resultParent.Count) ||
            !SetSearchResultsByDataClass<T>(pageInfoChild, resultChild, resultChild.Count))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 棚別在庫一覧・部門別在庫一覧 丸め処理・数量と単位を結合
        /// </summary>
        /// <typeparam name="T">一覧の型</typeparam>
        /// <param name="resultListParent">検索結果(親一覧)</param>
        /// <param name="resultListChild">検索結果(子一覧)</param>
        /// <param name="isInventory">棚別在庫一覧の場合True</param>
        private void combineNumAndUnit<T>(IList<T> resultListParent, IList<T> resultListChild, bool isInventory)
        {
            // 棚別在庫一覧か部門別在庫一覧を判定
            if (isInventory)
            {
                // 棚別在庫一覧
                IList<Dao.inventoryList> parentList = (IList<Dao.inventoryList>)resultListParent; // 親一覧
                IList<Dao.inventoryList> childList = (IList<Dao.inventoryList>)resultListChild;   // 子一覧

                // 棚番と棚枝番の結合文字を取得
                string strJoin = string.Empty;
                if (parentList.Count > 0)
                {
                    strJoin = TMQUtil.GetJoinStrOfPartsLocation((int)parentList[0].FactoryId, this.LanguageId, this.db);
                }

                // 親一覧
                Dao.inventoryList.joinStrAndRound(parentList, strJoin);
                // 子一覧
                Dao.inventoryList.joinStrAndRound(childList, strJoin);
            }
            else
            {
                // 部門別在庫一覧
                IList<Dao.categoryList> parentList = (IList<Dao.categoryList>)resultListParent; // 親一覧
                IList<Dao.categoryList> childList = (IList<Dao.categoryList>)resultListChild;   // 子一覧

                // 棚番と棚枝番の結合文字を取得
                string strJoin = string.Empty;
                if (parentList.Count > 0)
                {
                    strJoin = TMQUtil.GetJoinStrOfPartsLocation((int)parentList[0].FactoryId, this.LanguageId, this.db);
                }

                // 親一覧
                Dao.categoryList.joinStrAndRound(parentList, strJoin);
                // 子一覧
                Dao.categoryList.joinStrAndRound(childList, strJoin);
            }
        }

        /// <summary>
        /// 入出庫履歴タブ 表示年度 初期表示
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="isReDisp">再表示の場合True</param>
        /// <returns>エラーの場合False</returns>
        private bool setDispYear(ref Dao.detailSearchCondition condition, bool isReDisp)
        {
            // 工場の期首月を取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetStartMonthByFactoryId, out string outSql);
            string startMonth = db.GetEntityByDataClass<string>(outSql, condition);
            if (string.IsNullOrEmpty(startMonth))
            {
                startMonth = "04";
            }

            // 値を設定(初期表示は現在の年度)
            if (isReDisp)
            {
                // 検索条件を作成
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.DispYear);
                SetDataClassFromDictionary(targetDic, ConductInfo.FormDetail.ControlId.DispYear, condition, new List<string> { "DispYear" });
                condition.YearFrom = condition.DispYearFrom;
                condition.YearTo = condition.DispYearTo;
            }
            else
            {
                // グローバルリストに表示年度の値を保持しているか判定
                if (this.IndividualDictionary.ContainsKey(DispYearKeyName.YearFrom) || this.IndividualDictionary.ContainsKey(DispYearKeyName.YearTo))
                {
                    // グローバルリストに値を保持している場合は保持している値を使用して検索を行う
                    condition.YearFrom = this.IndividualDictionary[DispYearKeyName.YearFrom].ToString(); // 表示年度(From)
                    condition.YearTo = this.IndividualDictionary[DispYearKeyName.YearTo].ToString();     // 表示年度(To)
                }
                else
                {
                    // 現在の年度を求める
                    DateTime today = new DateTime(DateTime.Now.Year, int.Parse(startMonth), 1);
                    int bscktTo = (int.Parse(startMonth) - 1) * -1;
                    string year = DateTime.Now.AddMonths(bscktTo).ToString("yyyy");
                    condition.YearFrom = year;
                    condition.YearTo = year;
                }
            }

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormDetail.ControlId.DispYear, this.pageInfoList);

            // 検索結果の設定
            int condCnt = 1; // マジックナンバー回避用
            if (!SetSearchResultsByDataClass<Dao.detailSearchCondition>(pageInfo, new List<Dao.detailSearchCondition> { condition }, condCnt))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 表示年度(From)
            condition.DispYearFrom = new DateTime(int.Parse(condition.YearFrom), int.Parse(startMonth), 1).ToString();

            // 表示年度(To)
            if (condition.YearTo == SqlMaxYear)
            {
                // 画面で年の最大値(9999)が入力されている場合、SQLで扱うことのできる日付の最大値を設定
                condition.DispYearTo = System.Data.SqlTypes.SqlDateTime.MaxValue.ToString();
            }
            else
            {
                // 画面で入力された年の年度末の日付を設定
                condition.DispYearTo = new DateTime(int.Parse(condition.YearTo), int.Parse(startMonth), 1).AddYears(1).AddDays(-1).ToString();
            }

            return true;

            string getValNoByKey(MappingInfo info, string keyName)
            {
                // 項目名と一致する項目番号を返す
                return info.Value.First(x => x.KeyName.Equals(keyName)).ValName;
            }
        }

        /// <summary>
        /// 入出庫履歴一覧 検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合False</returns>
        private bool searchInoutHistoryInfo(Dao.detailSearchCondition condition)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetInoutHistoryList, out string executeSql);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.Common, out string withSql);

            // SQL実行
            IList<Dao.inoutHistoryList> results = db.GetListByDataClass<Dao.inoutHistoryList>(withSql + executeSql, condition);

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormDetail.ControlId.InOutHistoryList, this.pageInfoList);

            if (results.Count == 0)
            {
                // 検索結果の設定
                if (!SetSearchResultsByDataClass<Dao.inoutHistoryList>(pageInfo, results, results.Count))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
                return true;
            }

            // 丸め処理・数量と単位を結合
            Dao.inoutHistoryList.joinStrAndRound(results);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.inoutHistoryList>(pageInfo, results, results.Count))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 検索条件を取得
        /// </summary>
        /// <param name="fromCtrlId">値を取得する一覧のコントロールID</param>
        /// <returns>検索条件</returns>
        private Dao.detailSearchCondition getDetailSearchCondition(string fromCtrlId)
        {
            // 検索条件を作成
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, fromCtrlId);
            if (targetDic == null && fromCtrlId == ConductInfo.FormList.ControlId.List)
            {
                fromCtrlId = ConductInfo.FormDetail.ControlId.PartsInfo;
                targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, fromCtrlId);
            }
            Dao.detailSearchCondition condition = new();
            SetDataClassFromDictionary(targetDic, fromCtrlId, condition, new List<string> { "PartsId" });
            condition.LanguageId = this.LanguageId;                              // 言語ID
            return condition;

            /// <summary>
            /// 予備品IDに紐付く工場IDを取得する
            /// </summary>
            /// <param name="partsId">予備品ID</param>
            /// <returns>工場ID</returns>
            int? getFactoryId(long partsId)
            {
                // 検索SQL取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetFactoryIdByPartsId, out string executeSql);
                // 検索条件を設定
                Dao.detailSearchCondition condition = new();
                condition.PartsId = partsId; // 予備品ID
                condition = db.GetEntityByDataClass<Dao.detailSearchCondition>(executeSql, condition);
                // 取得した工場IDを返す
                return condition.FactoryId;
            }
        }

        /// <summary>
        /// 予備品情報 削除処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool deletePartsInfo()
        {
            // 削除条件を取得
            Dao.detailSearchCondition condition = getDetailSearchCondition(ConductInfo.FormDetail.ControlId.PartsInfo);

            // 排他チェック
            if (!checkExclusiveSingle(ConductInfo.FormDetail.ControlId.PartsInfo))
            {
                return false;
            }

            // 添付情報排他チェック
            if (!attachentcheckExclusive())
            {
                return false;
            }

            // 受払履歴に指定された予備品が存在するかどうかチェック
            if (isExistsHistory(condition))
            {
                return false;
            }

            // 予備品情報削除
            if (!new ComDao.PtPartsEntity().DeleteByPrimaryKey(condition.PartsId, this.db))
            {
                return false;
            }

            // 添付情報削除(画像)
            if (!deleteAttachmentInfo(condition, TMQConst.Attachment.FunctionTypeId.SpareImage))
            {
                return false;
            }

            // 添付情報削除(文書)
            if (!deleteAttachmentInfo(condition, TMQConst.Attachment.FunctionTypeId.SpareDocument))
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 添付情報排他チェック
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool attachentcheckExclusive()
        {
            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(ConductInfo.FormDetail.ControlId.PartsInfo);
            string partsIdVal = getValNoByParam(mappingInfo, "PartsId"); // MP情報ID
            string maxUpdateDatetimeVal = getValNoByParam(mappingInfo, "MaxUpdateDatetime"); // 最大更新日時の項目番号
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.PartsInfo);
            DateTime? maxDateOfList = DateTime.TryParse(targetDic[maxUpdateDatetimeVal].ToString(), out DateTime outDate) ? outDate : null;
            if (!CheckExclusiveStatusByUpdateDatetime(maxDateOfList, getMaxDateByMpInfoId(long.Parse(targetDic[partsIdVal].ToString()))))
            {
                // エラーの場合
                return false;
            }

            return true;

            DateTime? getMaxDateByMpInfoId(long id)
            {
                // 最大更新日時取得SQL
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetMaxDateByPartsId, out string outSql);
                Dao.attachmentMaxDate getMaxDateParam = new();
                getMaxDateParam.PartsId = id;
                getMaxDateParam.FunctionTypeId = new List<int>();
                getMaxDateParam.FunctionTypeId.Add((int)TMQConst.Attachment.FunctionTypeId.SpareImage);
                getMaxDateParam.FunctionTypeId.Add((int)TMQConst.Attachment.FunctionTypeId.SpareDocument);
                // SQL実行
                var maxDateResult = db.GetEntity(outSql, getMaxDateParam);

                return maxDateResult.max_update_datetime;
            }
        }

        /// <summary>
        /// 受払履歴に指定された予備品が存在するかどうか
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>存在する場合はTrue</returns>
        private bool isExistsHistory(Dao.detailSearchCondition condition)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetHistoryCountByPartsId, out string outSql);
            if (db.GetEntityByDataClass<int>(outSql, condition) > 0)
            {
                // エラーメッセ―ジを設定
                // 「入出庫データが存在するため、削除できません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141220004 });
                return true;
            }

            return false;
        }

        /// <summary>
        /// 添付情報を削除
        /// </summary>
        /// <param name="condition">削除条件</param>
        /// <param name="functionTypeId">機能タイプID</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool deleteAttachmentInfo(Dao.detailSearchCondition condition, TMQConst.Attachment.FunctionTypeId functionTypeId)
        {
            // 削除対象の添付情報のキーIDを取得
            condition.FunctionTypeId = (int)functionTypeId;

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetAttachmentInfo, out string outSql);
            if (db.GetEntityByDataClass<int>(outSql, condition) == 0)
            {
                // 添付情報が存在しない場合は処理を行わない
                return true;
            }

            // キーIDで削除
            if (!new ComDao.AttachmentEntity().DeleteByKeyId(functionTypeId, (int)condition.PartsId, this.db))
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// パラメータ名と一致する項目番号を返す
        /// </summary>
        /// <param name="info">一覧情報</param>
        /// <param name="keyName">項目キー名</param>
        /// <returns>項目番号</returns>
        private string getValNoByParam(MappingInfo info, string keyName)
        {
            // パラメータ名と一致する項目番号を返す
            return info.Value.First(x => x.ParamName.Equals(keyName)).ValName;
        }

        /// <summary>
        /// ラベル出力(棚別在庫一覧)
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool outputLabelInventoryData()
        {
            // 棚別在庫一覧(親一覧)のレコードを取得
            var inventoryParentList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.InventoryParentList);

            // 子一覧のレコード
            List<Dictionary<string, object>> inventoryChildList = new();

            // 親一覧を繰り返し処理
            foreach (Dictionary<string, object> parentInfo in inventoryParentList)
            {
                // 親一覧のレコードから子一覧のレコードを取得(画面の一覧は入れ子になっているため、親一覧から取得)
                List<object> childList = (List<object>)parentInfo.Where(x => x.Key == "SubData").ToList()[0].Value;

                // 子一覧のレコードをリストに追加(resultInfoDictionaryと同じような状態になる)
                foreach (Dictionary<string, object> childInfo in childList)
                {
                    inventoryChildList.Add(childInfo);
                }
            }

            // ラベル出力クラス
            TMQUtil.Label label = new(this.db, this.LanguageId);

            // 入庫一覧で選択されたレコードを取得
            var selectedList = getSelectedRowsByList(inventoryChildList, ConductInfo.FormDetail.ControlId.InventoryChildList);

            // 出力をするための検索条件リスト
            List<TMQUtilDao.LabelCondition> conditionList = new();

            // 各レコードごとに出力データを作成
            foreach (Dictionary<string, object> selectedRow in selectedList)
            {
                // 検索条件を取得
                TMQUtilDao.LabelCondition condition = new();

                // ディクショナリをデータクラスに変換
                SetDataClassFromDictionary(selectedRow, ConductInfo.FormDetail.ControlId.InventoryChildList, condition);

                // 検索条件リストに追加
                conditionList.Add(condition);
            }

            // ラベル出力データ取得処理
            if (!label.GetLabelData(conditionList, out List<object[]> outList))
            {
                return false;
            }

            // CSV出力処理
            if (!CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.ExportCsvFileNotencircleDobleQuotes(outList, Encoding.GetEncoding("Shift-JIS"), out Stream outStream, out string errMsg))
            {
                // エラーログ出力
                logger.ErrorLog(this.FactoryId, this.UserId, errMsg);
                // 「出力処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120006 });
                return false;
            }

            // 画面の出力へ設定
            this.OutputFileType = CommonConstants.REPORT.FILETYPE.CSV;
            this.OutputFileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", label.ReportId, DateTime.Now) + ComConsts.REPORT.EXTENSION.CSV;
            this.OutputStream = outStream;

            return true;
        }

        /// <summary>
        /// ラベル出力(詳細画面上部)
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool outputLabelDetail()
        {
            // 検索条件を取得
            TMQUtilDao.LabelCondition condition = new();

            // 予備品情報を取得
            var partsInfoDic = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.PartsInfo);
            // 購買管理情報を取得
            var purchaseInfoDic = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.PurchaseInfo);

            // ディクショナリをデータクラスに変換
            SetDataClassFromDictionary(partsInfoDic[0], ConductInfo.FormDetail.ControlId.PartsInfo, condition);
            // ディクショナリをデータクラスに変換
            SetDataClassFromDictionary(purchaseInfoDic[0], ConductInfo.FormDetail.ControlId.PurchaseInfo, condition);

            // ラベル出力クラス
            TMQUtil.Label label = new(this.db, this.LanguageId);

            // ラベル出力データ取得処理
            if (!label.GetLabelData(new List<TMQUtilDao.LabelCondition>() { condition }, out List<object[]> outList))
            {
                return false;
            }

            // CSV出力処理
            if (!CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.ExportCsvFileNotencircleDobleQuotes(outList, Encoding.GetEncoding("Shift-JIS"), out Stream outStream, out string errMsg))
            {
                // エラーログ出力
                logger.ErrorLog(this.FactoryId, this.UserId, errMsg);
                // 「出力処理に失敗しました。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120006 });
                return false;
            }

            // 画面の出力へ設定
            this.OutputFileType = CommonConstants.REPORT.FILETYPE.CSV;
            this.OutputFileName = string.Format("{0}_{1:yyyyMMddHHmmssfff}", label.ReportId, DateTime.Now) + ComConsts.REPORT.EXTENSION.CSV;
            this.OutputStream = outStream;

            return true;
        }
    }
}
