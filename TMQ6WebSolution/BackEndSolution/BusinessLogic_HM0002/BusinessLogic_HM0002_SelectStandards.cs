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
using Dao = BusinessLogic_HM0002.BusinessLogicDataClass_HM0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using SchedulePlanContent = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.SchedulePlanContent;

namespace BusinessLogic_HM0002
{
    /// <summary>
    /// 変更管理 長期計画(機器別管理基準選択画面)
    /// </summary>
    public partial class BusinessLogic_HM0002 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 初期化処理　機器別管理基準選択画面
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initSelectStandards()
        {
            // 呼出元画面の長期計画ID
            var param = getParam(ConductInfo.FormDetail.ControlId.Hide, false, false);
            // 画面に反映
            bool result = initFormByLongPlanId(param, new List<string> { ConductInfo.FormSelectStandards.ControlId.Hide, ConductInfo.FormSelectStandards.ControlId.Location, ConductInfo.FormSelectStandards.ControlId.Job }, out bool isMaintainanceKindFactory, out int factoryId, true);
            return result;
        }
        /// <summary>
        /// 検索処理　機器別管理基準選択画面
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchSelectStandards()
        {
            // 一覧のページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormSelectStandards.ControlId.List, this.pageInfoList);

            // 検索条件を画面より取得してデータクラスへセット
            Dao.SelectStandard.SearchCondition condition = GetConditionInfoByGroupNo<Dao.SelectStandard.SearchCondition>(ConductInfo.FormSelectStandards.SearchConditionGroupNo);
            // 検索条件の地区・職種は各階層に値が設定されているが、検索には「指定された最下層の値」以下の全ての階層IDを用いるので設定
            setStructureLayerInfo(ref condition);
            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = ComUtil.GetNotNullNameByClass<Dao.SelectStandard.SearchCondition>(condition);
            // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SelectStandards.SubDirLongPlan, SqlName.SelectStandards.GetList, out string baseSql, listUnComment);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SelectStandards.SubDir, SqlName.SelectStandards.GetList, out string withSql, listUnComment);
            condition.LanguageId = this.LanguageId;

            // 総件数取得SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, string.Empty, withSql);
            // 総件数を取得
            int cnt = db.GetCount(executeSql, condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY  machine_name, management_standards_component_id, management_standards_content_id ");

            // 一覧検索実行
            IList<Dao.Detail.List> results = db.GetListByDataClass<Dao.Detail.List>(selectSql.ToString(), condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.Detail.List>(pageInfo, results, cnt))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }
            return true;
        }

        /// <summary>
        /// 画面より取得した検索条件は地区と職種が各階層の値なので、階層IDを取得し設定する
        /// </summary>
        /// <param name="condition">ref 画面より取得した検索条件</param>
        /// <remarks>指定された階層ID配下の階層IDリストを検索条件に設定</remarks>
        private void setStructureLayerInfo(ref Dao.SelectStandard.SearchCondition condition)
        {
            // 共通処理で階層IDを設定するが、画面の検索条件は画面定義の関係でプロパティ名にIDでなく名称を使用しているので置き換える
            Dao.SelectStandard.StructureLayerCondition structureInfo = new();
            // 地区
            structureInfo.DistrictId = condition.DistrictName;
            structureInfo.FactoryId = condition.FactoryName;
            structureInfo.PlantId = condition.PlantName;
            structureInfo.SeriesId = condition.SeriesName;
            structureInfo.StrokeId = condition.StrokeName;
            structureInfo.FacilityId = condition.FacilityName;
            // 職種
            structureInfo.JobId = condition.JobName;
            structureInfo.LargeClassficationId = condition.LargeClassficationName;
            structureInfo.MiddleClassficationId = condition.MiddleClassficationName;
            structureInfo.SmallClassficationId = condition.SmallClassficationName;
            // 地区と職種の最下層の階層IDを取得
            IList<Dao.SelectStandard.StructureLayerCondition> tempList = new List<Dao.SelectStandard.StructureLayerCondition> { structureInfo };
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.SelectStandard.StructureLayerCondition>(ref tempList, new List<StructureType> { StructureType.Location, StructureType.Job });

            // 取得した階層ID配下の階層IDリストを検索条件に設定
            if (tempList[0].LocationStructureId != null)
            {
                // 機能場所階層ID
                condition.LocationStructureIdList = GetLowerStructureIdList(new List<int> { tempList[0].LocationStructureId ?? -1 });
            }
            if (tempList[0].JobStructureId != null)
            {
                // 職種機種階層ID
                condition.JobStructureIdList = GetLowerStructureIdList(new List<int> { tempList[0].JobStructureId ?? -1 });
            }
        }

        /// <summary>
        /// 登録処理　機器別管理基準選択画面
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistSelectStandards()
        {
            // 選択行を取得
            var selectedDicList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormSelectStandards.ControlId.List);
            // 非表示項目の内容を取得
            var hideDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormSelectStandards.ControlId.Hide);
            Dao.ListSearchResult hideInfo = new();
            SetDataClassFromDictionary(hideDic, ConductInfo.FormSelectStandards.ControlId.Hide, hideInfo);

            if (hideInfo.HistoryManagementId <= 0)
            {
                //トランザクションモード
                hideInfo.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction;

                //「承認済」以外の変更管理が紐づいている場合、エラー
                if (!checkHistoryManagement(hideInfo.LongPlanId))
                {
                    return false;
                }
            }
            else
            {
                //変更管理モード
                hideInfo.ProcessMode = (int)TMQConst.MsStructure.StructureId.ProcessMode.history;

                // 変更管理の排他チェック
                if (!checkExclusiveSingleForTable(ConductInfo.FormSelectStandards.ControlId.Hide, new List<string> { new ComDao.HmHistoryManagementEntity().TableName }, hideDic))
                {
                    return false;
                }
            }
            // 機器別管理基準内容の排他チェック
            if (!checkExclusiveList(ConductInfo.FormSelectStandards.ControlId.List, selectedDicList))
            {
                return false;
            }

            // 選択行をデータクラスに変更
            List<Dao.Detail.List> selectedRowList = ConvertDictionaryToClass<Dao.Detail.List>(ConductInfo.FormSelectStandards.ControlId.List, selectedDicList);
            // 入力チェック
            if (isErrorInputCheck(hideInfo, selectedRowList))
            {
                return false;
            }

            // DB更新
            // 更新日時、更新ユーザ
            var now = DateTime.Now;
            int userId = int.Parse(this.UserId);

            //選択行を変更管理へ反映
            if (!registContent(hideInfo, selectedRowList))
            {
                return false;
            }

            return true;

            // 入力チェック
            // hideInfo:非表示項目の内容
            // selectedRowList:選択行の内容
            bool isErrorInputCheck(Dao.ListSearchResult hideInfo, List<Dao.Detail.List> selectedRowList)
            {
                // 長期計画の工場IDが点検種別毎一覧表示工場であるか判定
                // 入力チェックは工場により判定するため、登録された機器に関係せず判定する
                bool factoryIsDisplayMaintainanceKind = isFactoryDisplayMaintainanceKind(hideInfo.FactoryId);

                if (!factoryIsDisplayMaintainanceKind)
                {
                    // 点検種別毎一覧表示工場でない場合は、入力チェック無し
                    return false;
                }
                // 点検種別毎一覧表示情報の場合、入力チェックを行う

                // 機番IDで点検種別毎管理の有無統一チェック
                if (isErrorMaintKindManage(hideInfo.LongPlanId))
                {
                    return true;
                }

                // 点検種別ごとの周期の統一チェック
                if (isErrorMaintKindCycle(hideInfo.LongPlanId))
                {
                    return true;
                }

                return false;

                // 工場IDにより点検種別毎一覧表示工場であるか判定
                bool isFactoryDisplayMaintainanceKind(int? factoryId)
                {
                    // 点検種別毎一覧表示工場
                    var list = new ComDao.MsStructureEntity().GetGroupList(TMQConst.MsStructure.GroupId.MaintainanceKindManageFactory, this.db);
                    // この結果の工場IDと一致するかを判定
                    var isMaintainanceKindFactory = list.Count(x => x.FactoryId == factoryId) > 0;
                    return isMaintainanceKindFactory;
                }

                // 機番IDで点検種別毎管理の有無統一チェック
                // 長期計画IDで機番IDを取得、加えて画面の機番IDを取得
                // これらの機番IDで機器情報テーブルを検索し、点検種別毎管理の値の種類の件数を取得→1件より大きければエラー
                bool isErrorMaintKindManage(long longPlanId)
                {
                    // 長期計画IDをキーとした検索条件
                    var condLongPlanId = new ComDao.LnLongPlanEntity();
                    condLongPlanId.LongPlanId = longPlanId;

                    // チェック対象の機番IDの一覧を取得する
                    // 点検種別の件数取得の検索条件(機番ID)
                    var condKindCount = new Dao.SelectStandard.InputCheck.CondIdList();
                    condKindCount.KeyIdList = new();
                    // 長期計画に紐づく機番IDのリストを取得
                    var machineIdList = TMQUtil.SqlExecuteClass.SelectList<long>(SqlName.InputCheck.GetMachineId, SqlName.InputCheck.SubDir, condLongPlanId, this.db);
                    if (machineIdList != null && machineIdList.Count > 0)
                    {
                        // 取得した長期計画に紐づく機番IDのリストを追加
                        condKindCount.KeyIdList.AddRange(machineIdList);
                    }

                    // 一覧の選択行より機番IDを取得し、一覧に追加
                    selectedRowList.ForEach(x => condKindCount.KeyIdList.Add(x.MachineId));

                    // 取得した機番IDのリストをキーに、点検種別の種類の件数を取得
                    int kindCount = TMQUtil.SqlExecuteClass.SelectEntity<int>(SqlName.InputCheck.GetMaintKindManageCount, SqlName.InputCheck.SubDirLongPlan, condKindCount, this.db);
                    // 種類の件数が1件より多い場合、有と無があるため統一されていない、エラー
                    if (kindCount > 1)
                    {
                        // 「機器の点検種別毎管理の有無が異なるデータです。」
                        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141070002 });
                        return true;
                    }

                    return false;
                }

                // 点検種別ごとの周期の統一チェック
                // 長期計画IDで機器別管理基準よりIDと点検種別を取得、加えて画面よりも取得
                // 取得した機器別管理基準内容でスケジュールを取得
                // 点検種別ごとにスケジュールを抽出し、周期と開始日で集計→1件より大きければエラー
                bool isErrorMaintKindCycle(long longPlanId)
                {
                    // 長期計画IDをキーとした検索条件
                    var condLongPlanId = new ComDao.LnLongPlanEntity();
                    condLongPlanId.LongPlanId = longPlanId;

                    // 機器別管理基準内容のIDと点検種別のディクショナリを作成
                    Dictionary<long, int?> dicContent = new();
                    // 長期計画に紐づく機器別管理基準内容を取得
                    var contentInfos = TMQUtil.SqlExecuteClass.SelectList<ComDao.McManagementStandardsContentEntity>(SqlName.InputCheck.GetContentInfo, SqlName.InputCheck.SubDirLongPlan, condLongPlanId, this.db);
                    if (contentInfos != null && contentInfos.Count > 0)
                    {
                        // ディクショナリに追加
                        contentInfos.ForEach(x => dicContent.Add(x.ManagementStandardsContentId, x.MaintainanceKindStructureId));
                    }

                    // 一覧の選択行より機器別管理基準内容を取得しディクショナリに追加
                    foreach (var row in selectedRowList)
                    {
                        if (!dicContent.ContainsKey(row.ManagementStandardsContentId))
                        {
                            dicContent.Add(row.ManagementStandardsContentId, row.MaintainanceKindStructureId);
                        }
                    }
                    // 保全スケジュールを取得
                    var condGetSchedule = new Dao.SelectStandard.InputCheck.CondIdList();
                    condGetSchedule.KeyIdList = dicContent.Keys.ToList();
                    var scheduleList = TMQUtil.SqlExecuteClass.SelectList<ComDao.McMaintainanceScheduleEntity>(SqlName.InputCheck.GetScheduleInfo, SqlName.InputCheck.SubDirLongPlan, condGetSchedule, this.db);
                    // いずれかの点検種別で周期、開始日の値が一致しない場合、エラー
                    // チェック済みの管理基準内容IDリスト
                    List<long> finishedContentIdList = new();
                    foreach (var schedule in scheduleList)
                    {
                        // 管理基準内容IDを取得(Nullは無し)
                        long contentId = schedule.ManagementStandardsContentId ?? -1;
                        // チェック済みならスキップ
                        if (finishedContentIdList.Contains(contentId))
                        {
                            continue;
                        }

                        // 対応する点検種別IDを取得
                        var maintId = dicContent[contentId];
                        if (maintId == null)
                        {
                            // 点検種別IDがNullならスキップ
                            continue;
                        }
                        // 点検種別IDが同じ管理基準内容IDを取得
                        List<long> sameMaintContentIdList = dicContent.ToList().Where(x => maintId.Equals(x.Value)).Select(x => x.Key).ToList();
                        // スケジュールを取得
                        var cntGroup = scheduleList.Where(x => sameMaintContentIdList.Contains(x.ManagementStandardsContentId ?? -1))
                            .GroupBy(x => new { Year = x.CycleYear, Month = x.CycleMonth, Day = x.CycleDay, Start = x.StartDate }).Count();
                        if (cntGroup > 1)
                        {
                            // 「機器の点検種別ごとに周期が異なるデータがあります。」
                            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141070003 });
                            return true;
                        }
                        // チェック済み管理基準内容IDリストに点検種別が同じ管理基準内容IDを追加
                        finishedContentIdList.AddRange(sameMaintContentIdList);
                    }

                    return false;
                }
            }

            //変更管理へ反映
            bool registContent(Dao.ListSearchResult hideInfo, List<Dao.Detail.List> selectedRowList)
            {
                TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0002);

                if (hideInfo.ProcessMode == (int)TMQConst.MsStructure.StructureId.ProcessMode.transaction)
                {
                    // トランザクションモード

                    // 変更管理テーブル登録処理(申請区分は「変更申請」)
                    (bool returnFlag, long historyManagementId) historyManagementResult =
                        historyManagement.InsertHistoryManagement(TMQConst.MsStructure.StructureId.ApplicationDivision.Update, hideInfo.LongPlanId, getFactoryId(historyManagement, hideInfo.LongPlanId));
                    if (!historyManagementResult.returnFlag)
                    {
                        return false;
                    }
                    hideInfo.HistoryManagementId = historyManagementResult.historyManagementId;
                }
                else
                {
                    // 変更管理モード

                    //変更管理テーブル更新処理
                    (bool flg, ComDao.HmHistoryManagementEntity entity) updateResult = updateHistoryManagement(hideInfo.HistoryManagementId, userId, now, false);
                    if (!updateResult.flg)
                    {
                        return false;
                    }
                }

                foreach (var row in selectedRowList)
                {
                    //機器別管理基準内容変更管理テーブルの登録
                    bool contentResult = registHistoryContent(hideInfo.HistoryManagementId, row.ManagementStandardsContentId, ExecutionDivision.AddContent, now, userId, hideInfo.LongPlanId);
                    if (!contentResult)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
