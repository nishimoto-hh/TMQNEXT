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
using Dao = BusinessLogic_MC0001.BusinessLogicDataClass_MC0001;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
using ScheduleStatus = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ScheduleStatus;
using static CommonWebTemplate.Models.Common.COM_CTRL_CONSTANTS;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using Jint.Native;
using DocumentFormat.OpenXml.Drawing;

namespace BusinessLogic_MC0001
{
    /// <summary>
    /// 機器台帳(機器別管理基準タブ)
    /// </summary>
    public partial class BusinessLogic_MC0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlNameManagementStandard
        {
            /// <summary>SQL名：機器別管理基準 保全項目一覧取得</summary>
            public const string GetManagementStandard = "GetManagementStandard";
            /// <summary>SQL名：機器別管理基準 保全項目データ取得</summary>
            public const string GetManagementStandardDetail = "GetManagementStandardDetail";
            /// <summary>SQL名：機器別管理基準 保全項目重複チェック</summary>
            public const string GetManagementStandardCountCheck = "GetManagementStandardCountCheck";
            /// <summary>SQL名：機器別管理基準 部位設定複数存在チェック</summary>
            public const string GetInspectionMultiExistCheck = "GetInspectionMultiExistCheck";
            /// <summary>SQL名：機器別管理基準 保全履歴存在チェック</summary>
            public const string GetScheduleMsSummryCountCheck = "GetScheduleMsSummryCountCheck";
            /// <summary>SQL名：機器別管理基準 同一点検種別周期チェック</summary>
            public const string GetMaintainanceKindManageExistCheck = "GetMaintainanceKindManageExistCheck";
            /// <summary>SQL名：機器別管理基準 同一点検種別周期チェック</summary>
            public const string GetMaintainanceKindManageInsertExistCheck = "GetMaintainanceKindManageInsertExistCheck";
            /// <summary>SQL名：機器別管理基準 同一点検種別周期データ取得</summary>
            public const string GetMaintainanceKindManageData = "GetMaintainanceKindManageData";
            /// <summary>SQL名：機器別管理基準 同一点検種別点検内容データ取得</summary>
            public const string GetMaintainanceKindManageDataUpdContent = "GetMaintainanceKindManageDataUpdContent";
            /// <summary>SQL名：機器別管理基準 様式１一覧取得</summary>
            public const string GetFormat1List = "GetFormat1List";
            /// <summary>SQL名：機器別管理基準 スケジューリング一覧取得</summary>
            public const string GetScheduleList = "GetMaintainanceScheduleList";
            /// <summary>SQL名：機器別管理基準 スケジューリング一覧取得</summary>
            public const string GetScheduleLankList = "GetMaintainanceScheduleLankList";
            /// <summary>SQL名：機器別管理基準 スケジューリング スケジュール情報取得</summary>
            public const string GetScheduleDetail = "GetMaintainanceScheduleDetail";
            /// <summary>SQL名：機器別管理基準 スケジューリング スケジュール情報取得更新チェック</summary>
            public const string GetMaintainanceScheduleUpdCheck = "GetMaintainanceScheduleUpdCheck";
            /// <summary>SQL名：機器別管理基準 点検種別毎スケジューリング　スケジュール情報取得</summary>
            public const string GetScheduleLankDetail = "GetMaintainanceScheduleLankDetail";
            /// <summary>SQL名：機器別管理基準 点検種別毎スケジューリング　スケジュール情報取得更新チェック</summary>
            public const string GetMaintainanceScheduleLankUpdCheck = "GetMaintainanceScheduleLankUpdCheck";
            /// <summary>SQL名：長期計画存在チェック</summary>
            public const string GetLongPlanSingle = "GetLongPlanSingle";
            /// <summary>SQL名：保全活動存在チェック</summary>
            public const string GetMsSummarySingle = "GetMsSummarySingle";
            /// <summary>SQL名：機器別管理基準 開始日以降保全活動存在チェック</summary>
            public const string GetScheduleMsSummryCountAfterCheck = "GetScheduleMsSummryCountAfterCheck";
            /// <summary>SQL名：機器別管理基準 開始日以降保全活動存在チェック(点検種別単位)</summary>
            public const string GetScheduleMsSummryCountAfterMaintainanceKindManageCheck = "GetScheduleMsSummryCountAfterMaintainanceKindManageCheck";
            /// <summary>SQL名：機器別管理基準部位  新規登録</summary>
            public const string InsertManagementStandardsComponent = "InsertManagementStandardsComponent";
            /// <summary>SQL名：機器別管理基準内容  新規登録</summary>
            public const string InsertManagementStandardsContent = "InsertManagementStandardsContent";
            /// <summary>SQL名：保全スケジュール  新規登録</summary>
            public const string InsertMaintainanceSchedule = "InsertMaintainanceSchedule";
            /// <summary>SQL名：保全スケジュール詳細  新規登録</summary>
            public const string InsertMaintainanceScheduleDetail = "InsertMaintainanceScheduleDetail";
            /// <summary>SQL名：機器別管理基準部位  更新登録</summary>
            public const string UpdateManagementStandardsComponent = "UpdateManagementStandardsComponent";
            /// <summary>SQL名：機器別管理基準部位  スケジューリング一覧更新登録</summary>
            public const string UpdateManagementStandardsComponentSchedule = "UpdateManagementStandardsComponentSchedule";
            /// <summary>SQL名：機器別管理基準内容  更新登録</summary>
            public const string UpdateManagementStandardsContent = "UpdateManagementStandardsContent";
            /// <summary>SQL名：機器別管理基準内容  スケジューリング一覧更新登録</summary>
            public const string UpdateManagementStandardsContentSchedule = "UpdateManagementStandardsContentSchedule";
            /// <summary>SQL名：保全スケジュール詳細  更新登録</summary>
            public const string UpdateMaintainanceScheduleDetail = "UpdateMaintainanceScheduleDetail";
            /// <summary>SQL名：保全スケジュール詳細点検種別毎  更新登録</summary>
            public const string UpdateMaintainanceScheduleLankDetail = "UpdateMaintainanceScheduleLankDetail";
            /// <summary>SQL名：保全スケジュール詳細  未来データ削除</summary>
            public const string DeleteMaintainanceScheduleDetailRemake = "DeleteMaintainanceScheduleDetailRemake";
            /// <summary>SQL名：保全スケジュール  未来データ削除</summary>
            public const string DeleteMaintainanceScheduleRemake = "DeleteMaintainanceScheduleRemake";
            /// <summary>SQL名：機器別管理基準内容  並び順更新</summary>
            public const string UpdateManagementStandardsContentOrderNo = "UpdateManagementStandardsContentOrderNo";
            /// <summary>SQL名：機器別管理基準部位 データ削除</summary>
            public const string DeleteManagementStandardsComponentSingle = "DeleteManagementStandardsComponentSingle";
            /// <summary>SQL名：機器別管理基準内容 データ削除</summary>
            public const string DeleteManagementStandardsContentSingle = "DeleteManagementStandardsContentSingle";
            /// <summary>SQL名：保全スケジュール データ削除</summary>
            public const string DeleteMaintainanceScheduleSingle = "DeleteMaintainanceScheduleSingle";
            /// <summary>SQL名：保全スケジュール詳細 データ削除</summary>
            public const string DeleteMaintainanceScheduleDetailSingle = "DeleteMaintainanceScheduleDetailSingle";
            /// <summary>SQL名：保全スケジュール詳細 データ削除 文書の最大更新日時を取得</summary>
            public const string GetMaxDateByKeyId = "GetMaxDateByKeyId";
            /// <summary>SQL名：保全スケジュール詳細 データ削除 文書を検索</summary>
            public const string GetAttachmentCount = "GetAttachmentCount";
            /// <summary>SQL名：保全項目一覧 次回実施日以降の次回実施日(次の次の予定日)を取得</summary>
            public const string GetNextScheduleDate = "GetNextScheduleDate";
            /// <summary>SQL名：保全項目一覧 表示周期のみを更新</summary>
            public const string UpdateDispCycleOfMaintainanceSchedule = "UpdateDispCycleOfMaintainanceSchedule";
            /// <summary>SQL名：保全項目一覧 保全スケジュール詳細IDでスケジュール日を更新</summary>
            public const string UpdateScheduleDateByDetailId = "UpdateScheduleDateByDetailId";
            /// <summary>SQL名：保全項目一覧 保全スケジュール詳細IDでスケジュール日を更新</summary>
            public const string UpdateScheduleDateByDetailIdAndSameKind = "UpdateScheduleDateByDetailIdAndSameKind";
        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlIdManagementStandard
        {
            /// <summary>
            /// 機器別管理基準 スケジューリング表示条件 非表示
            /// </summary>
            public const string DetailScheduleConditionList005 = "BODY_005_00_LST_1";
            /// <summary>
            /// 機器別管理基準 非表示一覧
            /// </summary>
            public const string DetailUnDispList80 = "BODY_080_00_LST_1";
            /// <summary>
            /// 機器別管理基準 保全項目一覧
            /// </summary>
            public const string DetailManagementStandard90 = "BODY_090_00_LST_1";
            /// <summary>
            /// 機器別管理基準 様式１一覧
            /// </summary>
            public const string DetailForma1List120 = "BODY_120_00_LST_1";
            /// <summary>
            /// 機器別管理基準 スケジューリング表示条件
            /// </summary>
            public const string DetailScheduleConditionList140 = "BODY_140_00_LST_1";
            /// <summary>
            /// 機器別管理基準 スケジューリング一覧
            /// </summary>
            public const string DetailScheduleList160 = "BODY_160_00_LST_1";
            /// <summary>
            /// 機器別管理基準 点検種別毎スケジューリング一覧
            /// </summary>
            public const string DetailLankScheduleList170 = "BODY_170_00_LST_1";

        }

        /// <summary>
        /// スケジュール表示条件：月度
        /// </summary>
        private static class ScheduleDispCondMonth
        {
            /// <summary>連番</summary>
            public const short Seq = 1;
            /// <summary>データタイプ</summary>
            public const short DataType = 2;
            /// <summary>拡張データ</summary>
            public const string ExData = "1";
        }

        /// <summary>
        /// 機器別管理基準検索処理
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool searchManagementStandard(long machineId, ref bool scheduleCondError)
        {
            // 対象機器の工場ID取得
            dynamic whereParam = new { MachineId = machineId };
            string sql;
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
            //if (results == null || results.Count == 0)
            //{
            //    return false;
            //}
            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, typeLst, this.db, this.LanguageId);
            //int factoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
            int factoryId = (int)results[0].FactoryId;
            List<int> factoryIdList = TMQUtil.GetFactoryIdList();
            factoryIdList.Add(factoryId);

            // 保全項目一覧初期検索
            setDataList<Dao.managementStandardResult>(machineId, SqlNameManagementStandard.GetManagementStandard, TargetCtrlIdManagementStandard.DetailManagementStandard90);

            // 様式１一覧初期検索
            // 2023.09 暫定対応 様式１一覧の検索実施しない
            setDataList<Dao.format1ListResult>(machineId, SqlNameManagementStandard.GetFormat1List, TargetCtrlIdManagementStandard.DetailForma1List120);

            // スケジューリング一覧初期検索
            // 点検種別毎管理の機器なら点検種別毎一覧
            if (flgMaintainanceKindManage)
            {
                setDataList<Dao.schedeluListResult>(machineId, SqlNameManagementStandard.GetScheduleLankList, TargetCtrlIdManagementStandard.DetailLankScheduleList170, factoryIdList);

                // スケジュール情報のセット
                if (!setSchedule(TargetCtrlIdManagementStandard.DetailLankScheduleList170, machineId, true, factoryId, ref scheduleCondError))
                {
                    return false;
                }
            }
            else
            {
                setDataList<Dao.schedeluListResult>(machineId, SqlNameManagementStandard.GetScheduleList, TargetCtrlIdManagementStandard.DetailScheduleList160);

                // スケジュール情報のセット
                if (!setSchedule(TargetCtrlIdManagementStandard.DetailScheduleList160, machineId, false, factoryId, ref scheduleCondError))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 保全情報一覧に紐づけるスケジュールリストを取得する処理
        /// </summary>
        private bool setSchedule(string listCtrlId, long machineId, bool lankList, int factoryId, ref bool scheduleCondError)
        {
            //int factoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);

            // 年度開始月の取得
            int monthStartNendo = getYearStartMonth(factoryId);

            // 画面の条件を取得
            // スケジュール表示条件セット
            Dao.ScheduleCondInfo condInfo = setScheduleCondition(TargetCtrlIdManagementStandard.DetailScheduleConditionList140, TargetCtrlIdManagementStandard.DetailScheduleConditionList005, ref scheduleCondError, monthStartNendo);
            if (scheduleCondError)
            {
                return false;
            }

            //// 非表示一覧にセット
            //setScheduleCondition(TargetCtrlIdManagementStandard.DetailScheduleConditionList005);
            var scheduleCond = new TMQDao.ScheduleList.Condition();
            if (condInfo.ScheduleCondType == condInfo.ScheduleCondMonthStructureId)
            {
                // 月度
                scheduleCond.ScheduleStartYear = (int)condInfo.ScheduleCondYear;
                scheduleCond.ScheduleUnit = (int)TMQConsts.MsStructure.StructureId.ScheduleDisplayUnit.Month;
                scheduleCond.ExtensionData = TMQConsts.MsStructure.StructureId.ScheduleDisplayUnit.Month.ToString();
            }
            else
            {
                string[] span = condInfo.scheduleCondSpan.Split('|');
                scheduleCond.ScheduleYearFrom = int.Parse(span[0]);
                scheduleCond.ScheduleYearTo = int.Parse(span[1]);
                scheduleCond.ScheduleStartYear = int.Parse(span[0]);
                scheduleCond.ScheduleUnit = (int)TMQConsts.MsStructure.StructureId.ScheduleDisplayUnit.Year;
                scheduleCond.ExtensionData = TMQConsts.MsStructure.StructureId.ScheduleDisplayUnit.Year.ToString();
            }
            TMQDao.ScheduleList.Condition co = new();
            Dao.Schedule.SearchConditionMachineId cond = new(scheduleCond, machineId, monthStartNendo, this.LanguageId);
            cond.FactoryIdList = TMQUtil.GetFactoryIdList();
            cond.FactoryIdList.Add(factoryId);
            // 個別実装用データへスケジュールのレイアウトデータ(scheduleLayout)をセット
            bool isMovable = true;
            if (listCtrlId == TargetCtrlIdManagementStandard.DetailLankScheduleList170)
            {
                TMQUtil.ScheduleListUtil.SetLayout(ref this.IndividualDictionary, cond, isMovable, getNendoText(), monthStartNendo, 1);
            }
            else if (listCtrlId == TargetCtrlIdManagementStandard.DetailScheduleList160)
            {
                TMQUtil.ScheduleListUtil.SetLayout(ref this.IndividualDictionary, cond, isMovable, getNendoText(), monthStartNendo, 2);
            }
            else if (listCtrlId == TargetCtrlIdLongPlan.LongPlanList210)
            {
                TMQUtil.ScheduleListUtil.SetLayout(ref this.IndividualDictionary, cond, isMovable, getNendoText(), monthStartNendo, 3);
            }

            // 画面表示データの取得
            List<TMQDao.ScheduleList.Display> scheduleDisplayList;
            if (lankList)
            {
                // 保全項目単位の場合、上位ランクのステータスを取得
                scheduleDisplayList = getScheduleDisplayList<TMQUtil.ScheduleListConverter>(cond, lankList);
            }
            else
            {
                // 保全項目単位以外(機器、部位)の場合、上位ランクのステータスは取得しない
                scheduleDisplayList = getScheduleDisplayList<TMQUtil.ScheduleListConverterNoRank>(cond, lankList);
            }

            // 画面設定用データに変換
            Dictionary<string, Dictionary<string, string>> setScheduleData = TMQUtil.ScheduleListUtil.ConvertDictionaryAddData(scheduleDisplayList, cond);

            // 画面に設定
            SetScheduleDataToResult(setScheduleData, listCtrlId);

            return true;

            // 画面表示データの取得 SQLを実行し、実行結果を変換 変換するクラスが上位ランクの有無が異なるので分岐
            List<TMQDao.ScheduleList.Display> getScheduleDisplayList<T>(Dao.Schedule.SearchConditionMachineId scheduleCondition, bool lankList)
                where T : TMQUtil.ScheduleListConverter, new()
            {
                string sql = string.Empty;
                if (lankList)
                {
                    TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetScheduleLankDetail, out sql);
                }
                else
                {
                    TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetScheduleDetail, out sql);
                }
                IList<TMQDao.ScheduleList.Get> scheduleList = db.GetListByDataClass<TMQDao.ScheduleList.Get>(sql, scheduleCondition);
                // 画面表示用に変換
                T listSchedule = new();
                // リンク有無
                bool isLink = true; // リンクは詳細画面のみ
                if (cond.IsYear())
                {
                    // リンクは月単位のみなので、年単位の場合リンクしない
                    isLink = false;
                }
                var scheduleDisplayList = listSchedule.Execute(scheduleList, cond, monthStartNendo, isLink, this.db, getScheduleLinkInfo());
                return scheduleDisplayList;
            }
        }

        /// <summary>
        /// スケジュールレイアウト作成に必要な「年度」の文言を取得する処理
        /// </summary>
        /// <returns>"年度"に相当する文言</returns>
        private string getNendoText()
        {
            // 「{0}年度」
            return GetResMessage(ComRes.ID.ID150000013);
        }

        /// <summary>
        /// 年度開始月を取得する処理
        /// </summary>
        /// <param name="factoryId">工場ID 省略時はユーザの本務工場</param>
        /// <returns>年度開始月</returns>
        private int getYearStartMonth(int? factoryId = null)
        {
            int startMonth;
            if (factoryId == null)
            {
                int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
                startMonth = TMQUtil.GetYearStartMonth(this.db, userFactoryId);
            }
            else
            {
                startMonth = TMQUtil.GetYearStartMonth(this.db, factoryId ?? -1);
            }
            return startMonth;
        }

        /// <summary>
        /// スケジュールのマークからのリンク先を設定する処理
        /// </summary>
        /// <returns>マークごとのリンク先の情報</returns>
        private Dictionary<ScheduleStatus, TMQDao.ScheduleList.Display.LinkTargetInfo> getScheduleLinkInfo()
        {
            Dictionary<ScheduleStatus, TMQDao.ScheduleList.Display.LinkTargetInfo> result = new();
            // ●：保全活動参照画面(履歴タブ)
            result.Add(ScheduleStatus.Complete, new(MA0001.ConductId, MA0001.FormNo.Detail, MA0001.TabNoDetail.History));
            //// ▲：上位ランク(点検種別が自分自身よりも上の物)の保全活動参照画面(履歴タブ)
            //result.Add(ScheduleStatus.UpperComplete, new(MA0001.ConductId, MA0001.FormNo.Detail, MA0001.TabNoDetail.History));
            //// ◎：保全活動参照画面(依頼タブ)
            //result.Add(ScheduleStatus.Created, new(MA0001.ConductId, MA0001.FormNo.Detail, MA0001.TabNoDetail.Request));
            //// ○：保全活動詳細画面(新規登録モード)
            //// タブNoとキーIDは不要なので適当な値
            //result.Add(ScheduleStatus.NoCreate, new(MA0001.ConductId, MA0001.FormNo.New, MA0001.TabNoNone));
            //// △：上位ランク(点検種別が自分自身よりも上の物)の保全活動参照画面(依頼タブ)
            //result.Add(ScheduleStatus.UpperScheduled, new(MA0001.ConductId, MA0001.FormNo.Detail, MA0001.TabNoDetail.Request));
            return result;
        }

        /// <summary>
        /// スケジュールより遷移する保全活動画面の情報
        /// </summary>
        private static class MA0001
        {
            /// <summary>機能ID</summary>
            public const string ConductId = "MA0001";
            /// <summary>フォームNo</summary>
            public static class FormNo
            {
                /// <summary>参照画面</summary>
                public const int Detail = 1;
                /// <summary>新規登録</summary>
                public const int New = 2;
            }
            /// <summary>タブなしの場合のタブNo</summary>
            public const int TabNoNone = 0;

            /// <summary>タブNo(参照画面)</summary>
            public static class TabNoDetail
            {
                /// <summary>履歴タブ</summary>
                public const int History = 1;
                /// <summary>依頼タブ</summary>
                public const int Request = 3;
            }
        }

        /// <summary>
        /// スケジュールより遷移する保全活動画面の情報
        /// </summary>
        private static class LN0001
        {
            /// <summary>機能ID</summary>
            public const string ConductId = "LN0001";
            /// <summary>フォームNo</summary>
            public static class FormNo
            {
                /// <summary>参照画面</summary>
                public const int Detail = 1;
                /// <summary>新規登録</summary>
                public const int New = 2;
            }
            /// <summary>タブなしの場合のタブNo</summary>
            public const int TabNoNone = 0;
        }

        /// <summary>
        /// 検索結果一覧セット
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool setDataList<T>(long machineId, string sqlName, string ctrlId, List<int> factoryIdList = null)
        {
            // 一覧検索SQL文の取得
            string sql;
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out sql);
            dynamic whereParam = null; // WHERE句パラメータ
            whereParam = new { MachineId = machineId, LanguageId = this.LanguageId, FactoryIdList = factoryIdList };

            // 一覧検索実行
            IList<T> results = db.GetListByDataClass<T>(sql, whereParam);
            if (results == null || results.Count == 0)
            {
                // 正常終了
                return true;
            }
            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<T>(pageInfo, results, results.Count))
            {
                // 異常終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 正常終了
            return true;
        }

        /// <summary>
        /// スケジュール表示条件セット
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private Dao.ScheduleCondInfo setScheduleCondition(string ctrlId, string conditionUnDispCtrlId, ref bool error, int monthStartNendo)
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.ScheduleDisp;
            //連番
            param.Seq = ScheduleDispCondMonth.Seq;
            //拡張データ
            param.ExData = ScheduleDispCondMonth.ExData;

            // 現在日付より現在年度を算出
            DateTime nowNendo = DateTime.Now;
            int nowMonth = int.Parse(DateTime.Now.ToString("MM"));
            int year = int.Parse(DateTime.Now.ToString("yyyy"));
            if (nowMonth < monthStartNendo)
            {
                year = int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy"));
                nowNendo = DateTime.Now.AddYears(-1);
            }

            //スケジュール表示条件「月度」の構成ID取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> cond = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            Dao.ScheduleCondInfo condInfo = new Dao.ScheduleCondInfo();

            // 非表示検索条件から取得
            var targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, conditionUnDispCtrlId);
            try
            {

                if (targetDictionary != null)
                {
                    if (string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "ScheduleCondType")))
                    {
                        // 初期表示の場合、システム年度初期化処理
                        SetSysFiscalYear<TMQDao.ScheduleList.Condition>(conditionUnDispCtrlId, monthStartNendo);
                        // 非表示検索条件から取得
                        targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, conditionUnDispCtrlId);

                        condInfo.ScheduleCondMonthStructureId = cond[0].StructureId;
                        condInfo.ScheduleCondType = condInfo.ScheduleCondMonthStructureId;
                        //condInfo.ScheduleCondYear = int.Parse(DateTime.Now.ToString("yyyy"));
                        //condInfo.ScheduleCondYear = int.Parse(nowNendo.ToString("yyyy"));
                        if (!string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "ScheduleCondYear")))
                        {
                            condInfo.ScheduleCondYear = int.Parse(getDictionaryKeyValue(targetDictionary, "ScheduleCondYear"));
                        }
                        else
                        {
                            condInfo.ScheduleCondYear = int.Parse(nowNendo.ToString("yyyy"));
                        }
                        //condInfo.scheduleCondSpan = DateTime.Now.AddYears(-5).ToString("yyyy") + "|" + DateTime.Now.AddYears(5).ToString("yyyy");
                        //condInfo.scheduleCondSpan = nowNendo.AddYears(-5).ToString("yyyy") + "|" + nowNendo.AddYears(5).ToString("yyyy");
                        if (!string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "scheduleCondSpan")))
                        {
                            condInfo.scheduleCondSpan = getDictionaryKeyValue(targetDictionary, "scheduleCondSpan");
                        }
                        else
                        {
                            condInfo.scheduleCondSpan = nowNendo.AddYears(-5).ToString("yyyy") + "|" + nowNendo.AddYears(5).ToString("yyyy");
                        }
                    }
                    else
                    {
                        condInfo.ScheduleCondMonthStructureId = int.Parse(getDictionaryKeyValue(targetDictionary, "ScheduleCondMonthStructureId"));
                        if (!string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "ScheduleCondType")))
                        {
                            condInfo.ScheduleCondType = int.Parse(getDictionaryKeyValue(targetDictionary, "ScheduleCondType"));
                        }
                        if (!string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "ScheduleCondYear")))
                        {
                            condInfo.ScheduleCondYear = int.Parse(getDictionaryKeyValue(targetDictionary, "ScheduleCondYear"));
                        }
                        if (!string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "scheduleCondSpan")))
                        {
                            condInfo.scheduleCondSpan = getDictionaryKeyValue(targetDictionary, "scheduleCondSpan");
                        }
                    }
                }
                else
                {
                    // 初期表示の場合、システム年度初期化処理
                    SetSysFiscalYear<TMQDao.ScheduleList.Condition>(conditionUnDispCtrlId, monthStartNendo);
                    // 非表示検索条件から取得
                    targetDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, conditionUnDispCtrlId);

                    condInfo.ScheduleCondMonthStructureId = cond[0].StructureId;
                    condInfo.ScheduleCondType = condInfo.ScheduleCondMonthStructureId;
                    //condInfo.ScheduleCondYear = int.Parse(DateTime.Now.ToString("yyyy"));
                    //condInfo.ScheduleCondYear = int.Parse(nowNendo.ToString("yyyy"));
                    //condInfo.ScheduleCondYear = int.Parse(nowNendo.ToString("yyyy"));
                    if (!string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "ScheduleCondYear")))
                    {
                        condInfo.ScheduleCondYear = int.Parse(getDictionaryKeyValue(targetDictionary, "ScheduleCondYear"));
                    }
                    else
                    {
                        condInfo.ScheduleCondYear = int.Parse(nowNendo.ToString("yyyy"));
                    }
                    //condInfo.scheduleCondSpan = DateTime.Now.AddYears(-5).ToString("yyyy") + "|" + DateTime.Now.AddYears(5).ToString("yyyy");
                    //condInfo.scheduleCondSpan = nowNendo.AddYears(-5).ToString("yyyy") + "|" + nowNendo.AddYears(5).ToString("yyyy");
                    if (!string.IsNullOrEmpty(getDictionaryKeyValue(targetDictionary, "scheduleCondSpan")))
                    {
                        condInfo.scheduleCondSpan = getDictionaryKeyValue(targetDictionary, "scheduleCondSpan");
                    }
                    else
                    {
                        condInfo.scheduleCondSpan = nowNendo.AddYears(-5).ToString("yyyy") + "|" + nowNendo.AddYears(5).ToString("yyyy");
                    }
                }
            }
            catch (Exception ex)
            {
                // 数値変換エラー
                errorReturnInfo();
                error = true;
                return null;
            }

            if (condInfo.ScheduleCondType == condInfo.ScheduleCondMonthStructureId)
            {
                // 月度
                if (condInfo.ScheduleCondYear == null || !int.TryParse(condInfo.ScheduleCondYear.ToString(), out int val))
                {
                    errorReturnInfo();
                    error = true;
                    return null;
                }

                // 上下限チェック
                if (condInfo.ScheduleCondYear > 9998 || condInfo.ScheduleCondYear < 1753)
                {
                    errorReturnInfo();
                    error = true;
                    return null;
                }
            }
            else
            {
                // 年度
                // 表示期間の数値チェック
                string[] span = condInfo.scheduleCondSpan.Split('|');
                if ((!int.TryParse(span[0], out int val1)) || (!int.TryParse(span[1], out int val2)))
                {
                    errorReturnInfo();
                    error = true;
                    return null;
                }
                // 上下限チェック
                if (val1 > val2)
                {
                    errorReturnInfo();
                    error = true;
                    return null;
                }
                if (val1 > 9998 || val1 < 1753)
                {
                    errorReturnInfo();
                    error = true;
                    return null;
                }
                if (val2 > 9998 || val2 < 1753)
                {
                    errorReturnInfo();
                    error = true;
                    return null;
                }
                if ((val2 - val1) > 20)
                {
                    errorReturnInfo();
                    error = true;
                    return null;
                }

            }

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);
            // 月度
            if (!SetSearchResultsByDataClass<Dao.ScheduleCondInfo>(pageInfo, new List<Dao.ScheduleCondInfo> { condInfo }, 1))
            {
                return null;
            }

            return condInfo;

            // エラー情報セット
            void errorReturnInfo()
            {
                condInfo.ScheduleCondMonthStructureId = cond[0].StructureId;
                //condInfo.ScheduleCondType = condInfo.ScheduleCondType;
                //condInfo.ScheduleCondYear = int.Parse(DateTime.Now.ToString("yyyy"));
                //condInfo.scheduleCondSpan = DateTime.Now.AddYears(-5).ToString("yyyy") + "|" + DateTime.Now.AddYears(5).ToString("yyyy");
                // ページ情報取得
                var pageInfo1 = GetPageInfo(ctrlId, this.pageInfoList);
                // 月度
                if (!SetSearchResultsByDataClass<Dao.ScheduleCondInfo>(pageInfo1, new List<Dao.ScheduleCondInfo> { condInfo }, 1))
                {
                    //return null;
                }

                this.Status = CommonProcReturn.ProcStatus.Warning;
                // 「 スケジュール表示条件が不正です。」
                this.MsgId = GetResMessage("141130001");
            }

        }

        /// <summary>
        /// 保全項目一覧登録
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool registManagementStandard(Dictionary<string, object> result)
        {
            // 入力された内容を取得
            Dao.managementStandardResult registResult = getRegistManagementStandard<Dao.managementStandardResult>(result, TargetCtrlIdManagementStandard.DetailManagementStandard90);
            bool insertFlg = false;
            if (registResult.ManagementStandardsComponentId == -1)
            {
                insertFlg = true;
            }

            // 更新時なら排他チェック
            DateTime now = DateTime.Now;
            IList<Dao.managementStandardResult> dbresult = null;
            if (!insertFlg)
            {
                // 最新DBデータ取得
                dynamic whereParam = whereParam = new { ManagementStandardsComponentId = registResult.ManagementStandardsComponentId, ManagementStandardsContentId = registResult.ManagementStandardsContentId };
                dbresult = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);
                // 排他チェック
                if (dbresult == null || dbresult.Count == 0 || isErrorExclusiveManagementStandard(registResult, dbresult))
                {
                    return false;
                }
            }
            else
            {
                // 新規登録者および新規登録日付をセット
                registResult.InsertUserId = int.Parse(this.UserId);
                registResult.InsertDatetime = now;
            }

            // 機器別管理基準フラグセット
            registResult.IsManagementStandardConponent = true;
            // 更新者および更新日付をセット
            registResult.UpdateUserId = int.Parse(this.UserId);
            registResult.UpdateDatetime = now;

            // 入力チェック
            Dictionary<string, object> targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlIdManagementStandard.DetailManagementStandard90);
            if (isErrorRegistManagementStandard(registResult, insertFlg, dbresult, TargetCtrlIdManagementStandard.DetailManagementStandard90, targetDic, true))
            {
                return false;
            }

            // スケジュールを更新 が未選択でかつ、次回実施予定日が入力されている場合
            // 確認メッセージを表示する(※メッセージを表示する必要がある時はTrueが返る)
            if (confirmByIsUpdateAndScheduleDate(registResult))
            {
                return true;
            }

            // 新規登録かつ、次回実施予定日が設定されている場合
            // 確認メッセージを表示する(※メッセージを表示する必要がある時はTrueが返る)
            if (confirmByIsNewAndIsUpdate(insertFlg, registResult))
            {
                return true;
            }

            // 周期または開始日が変更されていて、スケジュールを更新 が未選択の場合
            if (confirmByScheduleInfoChanged(registResult, dbresult))
            {
                return true;
            }

            // 周期変更有チェック
            bool cycleChangeFlg = false;

            // 周期または開始日が変更されている
            if (dbresult != null)
            {
                if (registResult.CycleYear == dbresult[0].CycleYear && registResult.CycleMonth == dbresult[0].CycleMonth && registResult.CycleDay == dbresult[0].CycleDay && registResult.StartDate == dbresult[0].StartDate)
                {
                    cycleChangeFlg = false;
                }
                else
                {
                    cycleChangeFlg = true;
                }
                // 更新時確認チェック(周期・開始日変更チェック 点検種別毎時周期・開始日変更チェック)
                if (this.Status < CommonProcReturn.ProcStatus.Confirm)
                {
                    if (!insertFlg)
                    {
                        // 検索条件取得
                        dynamic whereParam = null; // WHERE句パラメータ
                        string sql;
                        // 一覧検索SQL文の取得
                        TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
                        whereParam = new { MachineId = registResult.MachineId };

                        // 一覧検索実行
                        IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
                        if (results == null || results.Count == 0)
                        {
                            return false;
                        }
                        if (isConfirmRegistCycle(registResult, results[0].MaintainanceKindManage))
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {

                // 更新時確認チェック(点検種別毎時周期・開始日変更チェック)
                if (this.Status < CommonProcReturn.ProcStatus.Confirm)
                {
                    // 検索条件取得
                    dynamic whereParam = null; // WHERE句パラメータ
                    string sql;
                    // 一覧検索SQL文の取得
                    TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
                    whereParam = new { MachineId = registResult.MachineId };

                    // 一覧検索実行
                    IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
                    if (results == null || results.Count == 0)
                    {
                        return false;
                    }

                    // 点検種別毎管理
                    if (results[0].MaintainanceKindManage)
                    {
                        // 確認チェック
                        if (isConfirmRegistCycleInsert(registResult))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    // 同一点検種別内に開始日以降で保全活動が紐づいている者があるかチェック
                    if (isErrorRegistMaintainanceKindManage(registResult, TargetCtrlIdManagementStandard.DetailManagementStandard90))
                    {
                        return false;
                    }
                }
            }

            // 周期ありフラグセット
            if ((registResult.CycleYear == null || registResult.CycleYear == 0) && (registResult.CycleMonth == null || registResult.CycleMonth == 0) && (registResult.CycleDay == null || registResult.CycleDay == 0))
            {
                // 周期無し
                registResult.IsCyclic = false;
            }
            else
            {
                // 周期あり
                registResult.IsCyclic = true;
            }

            // 登録
            if (insertFlg)
            {
                // 新規登録
                // 機器別管理基準部位
                (bool returnFlag, long id) managementStandardsComponent = registInsertDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.InsertManagementStandardsComponent);
                if (!managementStandardsComponent.returnFlag)
                {
                    return false;
                }
                // 機器別管理基準内容
                registResult.ManagementStandardsComponentId = managementStandardsComponent.id;
                (bool returnFlag, long id) managementStandardsContent = registInsertDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.InsertManagementStandardsContent);
                if (!managementStandardsContent.returnFlag)
                {
                    return false;
                }
                // 保全スケジュール
                registResult.ManagementStandardsContentId = managementStandardsContent.id;
                (bool returnFlag, long id) maintainanceSchedule = registInsertDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.InsertMaintainanceSchedule);
                if (!maintainanceSchedule.returnFlag)
                {
                    return false;
                }
                // 保全スケジュール詳細
                if (!insertMainteScheduleDetail(registResult, maintainanceSchedule.id, now))
                {
                    return false;
                }

                // 同一点検種別のスケジュール基準更新
                if (!updateManagementContentKindManage(registResult, now))
                {
                    return false;
                }

                // 点検種別毎管理機器なら同点検種の周期なども変更
                registResult.MaintainanceScheduleId = maintainanceSchedule.id;
                if (!updateMaintainanceKindManage(registResult, now))
                {
                    return false;
                }

            }
            else
            {
                // 更新
                // 機器別管理基準部位
                if (!registUpdateDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.UpdateManagementStandardsComponent))
                {
                    return false;
                }
                // 機器別管理基準内容
                if (!registUpdateDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.UpdateManagementStandardsContent))
                {
                    return false;
                }

                // 同一点検種別のスケジュール基準更新
                if (!updateManagementContentKindManage(registResult, now))
                {
                    return false;
                }

                // 自身のレコードの点検種別と同一の点検種別のデータを取得
                // ※機器が点検種別毎管理でない場合または自身のレコードの点検種別と同一の点検種別のデータが存在しない場合 は取得されない
                if (!getSameManageKindData(registResult, out IList<Dao.managementStandardResult> sameManageKindList))
                {
                    return false;
                }

                // 表示周期のみを更新する
                if (!registUpdateDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.UpdateDispCycleOfMaintainanceSchedule))
                {
                    return false;
                }

                // 自身のレコードと同一の点検種別のデータの表示周期も更新する
                if (!updateDispCycle(registResult, sameManageKindList))
                {
                    return false;
                }

                // 次回実施予定日が入力されていて値が変更されている場合
                if (registResult.ScheduleDate != null && registResult.ScheduleDate != registResult.ScheduleDateBefore)
                {
                    // スケジュール日を入力された値に更新する
                    if (!registUpdateDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.UpdateScheduleDateByDetailId))
                    {
                        return false;
                    }

                    // 自身のレコードと同一の点検種別のデータの次回実施予定日も更新する
                    if (!updateNextScheduleDate(registResult, sameManageKindList))
                    {
                        return false;
                    }
                }

                // スケジュールを更新 が未選択または、次回実施予定日が入力されて変更されている場合
                if (registResult.IsUpdateSchedule.ToString() == ComConsts.CHECK_FLG.OFF ||
                    (registResult.ScheduleDate != null && registResult.ScheduleDate != registResult.ScheduleDateBefore))
                {
                    // 保全スケジュールの再作成処理は行わないのでここで終了
                    return true;
                }

                // 周期・開始日変更有
                if (cycleChangeFlg)
                {
                    // 「作成済み保全スケジュール詳細データ(開始日以降)」削除
                    if (!registDeleteDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.DeleteMaintainanceScheduleDetailRemake))
                    {
                        // エラーの場合
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return false;
                    }

                    // 変更後開始日 < 変更前開始日であれば変更前開始日レコード削除
                    if (registResult.StartDate <= dbresult[0].StartDate)
                    {
                        // 入力された日付以降の日付で設定されていた保全スケジュールデータは削除
                        if (!registDeleteDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.DeleteMaintainanceScheduleRemake))
                        {
                            // エラーの場合
                            this.Status = CommonProcReturn.ProcStatus.Error;
                            return false;
                        }
                    }

                    // 新規登録者および新規登録日付をセット
                    registResult.InsertUserId = int.Parse(this.UserId);
                    registResult.InsertDatetime = now;

                    // 保全スケジュールにデータ新規作成
                    (bool returnFlag, long id) maintainanceSchedule = registInsertDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.InsertMaintainanceSchedule);
                    if (!maintainanceSchedule.returnFlag)
                    {
                        return false;
                    }

                    // 保全スケジュール詳細にデータ新規作成
                    if (!insertMainteScheduleDetail(registResult, maintainanceSchedule.id, now))
                    {
                        return false;
                    }

                    //// 点検種別毎管理機器なら同点検種の周期なども変更
                    //registResult.MaintainanceScheduleId = maintainanceSchedule.id;
                    //if (!updateMaintainanceKindManage(registResult, now))
                    //{
                    //    return false;
                    //}
                }
                // 点検種別毎管理機器なら同点検種の周期なども変更
                //registResult.MaintainanceScheduleId = maintainanceSchedule.id;
                if (!updateMaintainanceKindManage(registResult, now))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 自身のレコードと同一の点検種別のデータを取得
        /// </summary>
        /// <param name="result">画面で入力された情報</param>
        /// <param name="sameManageKindList">自身のレコードと同一の点検種別のデータ</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool getSameManageKindData(Dao.managementStandardResult result, out IList<Dao.managementStandardResult> sameManageKindList)
        {
            sameManageKindList = null;

            // 検索条件取得
            dynamic whereParam = null; // WHERE句パラメータ
            string sql;
            // 一覧検索SQL文の取得
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
            whereParam = new { MachineId = result.MachineId };

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 点検種別毎管理か判定
            if (!results[0].MaintainanceKindManage)
            {
                // 点検種別毎管理ではない場合は何もせずに終了
                return true;
            }

            // 最新DBデータ取得
            whereParam = null; // WHERE句パラメータ
            whereParam = new
            {
                MachineId = result.MachineId,
                MaintainanceKindStructureId = result.MaintainanceKindStructureId,
                ManagementStandardsContentId = result.ManagementStandardsContentId
            };
            sameManageKindList = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetMaintainanceKindManageDataUpdContent, whereParam);

            return true;
        }

        /// <summary>
        /// 自身のレコードと同一の点検種別のデータの表示周期を更新
        /// </summary>
        /// <param name="result">画面で入力されたデータ</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool updateDispCycle(Dao.managementStandardResult result, IList<Dao.managementStandardResult> sameManageKindList)
        {
            // 自身のレコードと同一の点検種別のデータが存在しない場合は何もしない
            if (sameManageKindList == null || sameManageKindList.Count == 0)
            {
                return true;
            }

            foreach (Dao.managementStandardResult regRes in sameManageKindList)
            {
                // 更新情報を設定
                regRes.DispCycle = result.DispCycle;                           // 表示周期
                regRes.UpdateDatetime = result.UpdateDatetime;                 // 更新日時
                regRes.UpdateUserId = result.UpdateUserId;                     // 更新ユーザー

                // 表示周期のみを更新する
                if (!registUpdateDb<Dao.managementStandardResult>(regRes, SqlNameManagementStandard.UpdateDispCycleOfMaintainanceSchedule))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 自身のレコードと同一の点検種別のデータの次回実施予定日を更新
        /// </summary>
        /// <param name="result">画面で入力されたデータ</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool updateNextScheduleDate(Dao.managementStandardResult result, IList<Dao.managementStandardResult> sameManageKindList)
        {
            // 自身のレコードと同一の点検種別のデータが存在しない場合は何もしない
            if (sameManageKindList == null || sameManageKindList.Count == 0)
            {
                return true;
            }

            foreach (Dao.managementStandardResult regRes in sameManageKindList)
            {
                // 更新情報を設定
                regRes.ScheduleDate = result.ScheduleDate;                                 // スケジュール日
                regRes.UpdateDatetime = result.UpdateDatetime;                             // 更新日時
                regRes.UpdateUserId = result.UpdateUserId;                                 // 更新ユーザー

                // 次回実施予定日を更新する
                // 次回実施予定日をもう一度取得しなおして、取得した日付のデータを更新する
                // ※自身のレコードの次回実施予定日と他のデータの次回実施予定日が同一とは限らないため
                if (!registUpdateDb<Dao.managementStandardResult>(regRes, SqlNameManagementStandard.UpdateScheduleDateByDetailIdAndSameKind))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 様式１一覧登録
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool registFormat1List()
        {
            // リスト取得
            IList<Dictionary<string, object>> result = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlIdManagementStandard.DetailForma1List120);
            DateTime now = DateTime.Now;

            // 排他チェック
            foreach (var row in result)
            {
                // 入力された内容を取得
                Dao.format1ListResult updResult = getRegistManagementStandard<Dao.format1ListResult>(row, TargetCtrlIdManagementStandard.DetailForma1List120);
                IList<Dao.format1ListResult> dbresult = null;
                // 最新DBデータ取得
                dynamic whereParam = whereParam = new { ManagementStandardsComponentId = updResult.ManagementStandardsComponentId, ManagementStandardsContentId = updResult.ManagementStandardsContentId };
                dbresult = getDbData<Dao.format1ListResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);
                // 排他チェック
                if (dbresult == null || dbresult.Count == 0 || isErrorExclusiveFormat1List(updResult, dbresult))
                {
                    return false;
                }
            }

            int i = 1;
            foreach (Dictionary<string, object> dicProc in result)
            {
                // 行の内容をクラスに設定
                ComDao.McManagementStandardsContentEntity entity = new ComDao.McManagementStandardsContentEntity();
                SetDataClassFromDictionary(dicProc, TargetCtrlIdManagementStandard.DetailForma1List120, entity);
                entity.OrderNo = i;
                entity.UpdateUserId = int.Parse(this.UserId);
                entity.UpdateDatetime = now;

                // 機器別管理基準内容 並び順更新
                if (!registUpdateDb<ComDao.McManagementStandardsContentEntity>(entity, SqlNameManagementStandard.UpdateManagementStandardsContentOrderNo))
                {
                    return false;
                }
                i = i + 1;
            }

            initDetail();

            return true;
        }

        /// <summary>
        /// スケジューリング一覧登録
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool registScheduleList()
        {
            // リスト取得
            IList<Dictionary<string, object>> result = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlIdManagementStandard.DetailScheduleList160);
            DateTime now = DateTime.Now;

            // 入力値チェック(横方向一覧の直接入力は共通側でチェック不可のため業務側で行う)
            if (isInputvalCheck(result, true))
            {
                return false;
            }

            // -------------------- この部分はスケジュール表の更新時に使用するが、入力チェックで必要になるためここで取得 --------------------
            // 更新対象の一覧の内容を取得
            var rowDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlIdManagementStandard.DetailScheduleList160);
            // 一覧のキー値のVAL値を取得(スケジュールとの紐付に使用)
            var valKeyId = GetValKeyIdForSchedule(TargetCtrlIdManagementStandard.DetailScheduleList160);

            // 入力された内容を取得
            Dao.managementStandardResult machineInfo = getRegistManagementStandard<Dao.managementStandardResult>(result[0], TargetCtrlIdManagementStandard.DetailScheduleList160);
            // 対象機器の工場ID取得
            dynamic whereParamF = new { MachineId = machineInfo.MachineId };
            string sql;
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParamF);
            //if (results == null || results.Count == 0)
            //{
            //    return false;
            //}
            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, typeLst, this.db, this.LanguageId);
            //int factoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
            int factoryId = (int)results[0].FactoryId;

            // 年度開始月の取得
            int monthStartNendo = getYearStartMonth(factoryId);
            // 一覧より更新されたスケジュールの情報を取得
            List<TMQDao.ScheduleList.Display> updatedScheduleList = TMQUtil.ScheduleListUtil.GetScheduleUpdatedData(rowDicList, valKeyId, monthStartNendo);

            // 更新対象のキーIDの一覧を取得(画面のディクショナリとの紐づけに使用)
            var updateKeyIdList = updatedScheduleList.Select(x => x.KeyId).ToList();
            // -------------------- この部分はスケジュール表の更新時に使用するが、入力チェックで必要になるためここで取得 --------------------

            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();
            var info = getResultMappingInfo(TargetCtrlIdManagementStandard.DetailScheduleList160);

            // 排他チェック、入力チェック
            foreach (var row in result)
            {// 変更フラグが立っているもの
                if (isUpdatedTableRow(row))
                {
                    // 入力された内容を取得
                    Dao.schedeluListResult updResult = getRegistManagementStandard<Dao.schedeluListResult>(row, TargetCtrlIdManagementStandard.DetailScheduleList160);

                    IList<Dao.schedeluListResult> dbresult = null;
                    // 最新DBデータ取得
                    dynamic whereParam = whereParam = new { ManagementStandardsComponentId = updResult.ManagementStandardsComponentId, ManagementStandardsContentId = updResult.ManagementStandardsContentId };
                    //dbresult = getDbData<Dao.schedeluListResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);
                    dbresult = getDbData<Dao.schedeluListResult>(SqlNameManagementStandard.GetMaintainanceScheduleUpdCheck, whereParam);
                    // 排他チェック
                    if (dbresult == null || dbresult.Count == 0 || isErrorExclusiveScheduleList(updResult, dbresult, true))
                    {
                        return false;
                    }

                    IList<Dao.managementStandardResult> chkDbResult = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);
                    Dao.managementStandardResult chkUpdResult = getRegistManagementStandard<Dao.managementStandardResult>(row, TargetCtrlIdManagementStandard.DetailScheduleList160);

                    // 入力チェック
                    if (isErrorRegistManagementStandard(chkUpdResult, false, chkDbResult, TargetCtrlIdManagementStandard.DetailScheduleList160, row))
                    {
                        return false;
                    }

                    if (chkDbResult != null)
                    {
                        if (!(chkUpdResult.CycleYear == chkDbResult[0].CycleYear && chkUpdResult.CycleMonth == chkDbResult[0].CycleMonth && chkUpdResult.CycleDay == chkDbResult[0].CycleDay && chkUpdResult.StartDate == chkDbResult[0].StartDate))
                        {
                            // 更新時確認チェック(周期・開始日変更チェック 点検種別毎時周期・開始日変更チェック)
                            if (this.Status < CommonProcReturn.ProcStatus.Confirm)
                            {
                                if (isConfirmRegistCycle(chkUpdResult, chkDbResult[0].MaintainanceKindManage))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                // 同一点検種別内に開始日以降で保全活動が紐づいている者があるかチェック
                                if (isErrorRegistMaintainanceKindManage(chkUpdResult, TargetCtrlIdManagementStandard.DetailManagementStandard90))
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    // 過去日のチェックは行わない
                    //// 次回実施予定日に過去日が入力されている場合
                    //if (updResult.NextScheduleDate != null && updResult.NextScheduleDate < DateTime.Now.Date)
                    //{
                    //    // エラー情報格納クラス
                    //    ErrorInfo errorInfo = new ErrorInfo(row);
                    //    // 過去日付は設定できません。
                    //    string errMsg = GetResMessage("141060003");
                    //    string val = info.getValName("next_schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    //    errorInfo.setError(errMsg, val);
                    //    errorInfoDictionary.Add(errorInfo.Result);
                    //}

                    // 次回実施予定日が入力されている場合
                    if (updResult.NextScheduleDate != null)
                    {
                        // 次々回実施予定日を取得
                        List<Dao.managementStandardResult> nextDate = (List<Dao.managementStandardResult>)getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetNextScheduleDate, new { ManagementStandardsContentId = updResult.ManagementStandardsContentId, ScheduleDateBefore = updResult.NextScheduleDateDisp, MaintainanceScheduleId = updResult.MaintainanceScheduleId });

                        // 取得できた場合は範囲チェックをする
                        if (nextDate != null && nextDate.Count > 0)
                        {
                            bool isDateErr = false;

                            // 入力された次回実施予定日が次々回実施予定日以降の場合
                            if (updResult.NextScheduleDate >= nextDate[0].ScheduleDate)
                            {
                                isDateErr = true;
                            }

                            // 前回実施予定日は存在しない可能性があることを考慮する
                            if (nextDate.Count > 1)
                            {
                                // 入力された次回実施予定日が前回実施予定日の場合以前
                                if (updResult.NextScheduleDate <= nextDate[1].ScheduleDate)
                                {
                                    isDateErr = true;
                                }
                            }
                            else
                            {
                                // 前回実施予定日が存在しない場合は開始日と比較する
                                if (updResult.NextScheduleDate < updResult.StartDate)
                                {
                                    isDateErr = true;
                                }
                            }

                            // 日付の範囲エラーの場合はメッセージをセットする
                            if (isDateErr)
                            {
                                // エラー情報格納クラス
                                ErrorInfo errorInfo = new ErrorInfo(row);
                                // 次回実施予定日は前回実施予定日～次々回実施予定日以内の日付を指定してください。
                                string errMsg = GetResMessage("141120020");
                                string val = info.getValName("next_schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                                errorInfo.setError(errMsg, val);
                                errorInfoDictionary.Add(errorInfo.Result);
                            }
                        }
                    }

                    // スケジュールを更新 が未選択でかつ、次回実施予定日が入力されている場合
                    // 確認メッセージを表示する(※メッセージを表示する必要がある時はTrueが返る)
                    if (confirmByIsUpdateAndScheduleDate(new Dao.managementStandardResult() { IsUpdateSchedule = updResult.IsUpdateSchedule, ScheduleDate = updResult.NextScheduleDate }))
                    {
                        return true;
                    }

                    // 周期または開始日が変更されていて、スケジュールを更新 が未選択の場合
                    if (confirmByScheduleInfoChanged(new Dao.managementStandardResult()
                    {
                        CycleYear = updResult.CycleYear,
                        CycleMonth = updResult.CycleMonth,
                        CycleDay = updResult.CycleDay,
                        StartDate = updResult.StartDate,
                        IsUpdateSchedule = updResult.IsUpdateSchedule
                    }, chkDbResult))
                    {
                        return true;
                    }

                    // 開始日または周期(年・月・日)が変更されているかどうか
                    // いずれかが変更されている場合はフラグ = True
                    bool cycleChangeFlg;
                    if (updResult.CycleYear == dbresult[0].CycleYear &&
                        updResult.CycleMonth == dbresult[0].CycleMonth &&
                        updResult.CycleDay == dbresult[0].CycleDay &&
                        updResult.StartDate == dbresult[0].StartDate)
                    {
                        cycleChangeFlg = false;
                    }
                    else
                    {
                        cycleChangeFlg = true;
                    }

                    // 次回実施予定日が入力されているかつ値が変更されていて、開始日または周期(年・月・日)が変更されている場合
                    if (updResult.NextScheduleDate != null && updResult.NextScheduleDateDisp != updResult.NextScheduleDate && cycleChangeFlg)
                    {
                        // エラー情報格納クラス
                        ErrorInfo errorInfo = new ErrorInfo(row);
                        // 周期または開始日が変更されていいます。　同時に次回実施予定日は更新できません。
                        string errMsg = GetResMessage("141120019");
                        string val = info.getValName("next_schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                        errorInfo.setError(errMsg, val);
                        errorInfoDictionary.Add(errorInfo.Result);
                    }

                    // 次回実施予定日が変更されていて、スケジュールも変更されている場合
                    // 同時には変更できないのでエラーとする
                    if (updResult.NextScheduleDate != updResult.NextScheduleDateDisp && updateKeyIdList.Contains(updResult.KeyId))
                    {
                        // エラー情報格納クラス
                        ErrorInfo errorInfo = new ErrorInfo(row);
                        // 星取表と次回実施予定日を同時に変更することはできません。
                        string val = info.getValName("next_schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                        errorInfo.setError(GetResMessage("141300010"), val);
                        errorInfoDictionary.Add(errorInfo.Result);
                    }
                }
            }

            // 入力エラーがある場合はここで終了
            if (errorInfoDictionary.Count > 0)
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return false;
            }

            foreach (Dictionary<string, object> dicProc in result)
            {
                // 変更フラグ
                if (isUpdatedTableRow(dicProc))
                {
                    Dao.managementStandardResult registResult = getRegistManagementStandard<Dao.managementStandardResult>(dicProc, TargetCtrlIdManagementStandard.DetailScheduleList160);

                    IList<Dao.managementStandardResult> dbresult = null;
                    dynamic whereParam = whereParam = new { ManagementStandardsComponentId = registResult.ManagementStandardsComponentId, ManagementStandardsContentId = registResult.ManagementStandardsContentId };
                    dbresult = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);

                    // 機器別管理基準フラグセット
                    registResult.IsManagementStandardConponent = true;
                    // 更新者および更新日付をセット
                    registResult.UpdateUserId = int.Parse(this.UserId);
                    registResult.UpdateDatetime = now;

                    // 周期変更有チェック
                    bool cycleChangeFlg = false;

                    // 周期・開始日・表示周期・スケジュール管理が変更されている
                    if (dbresult != null)
                    {
                        if (registResult.CycleYear == dbresult[0].CycleYear && registResult.CycleMonth == dbresult[0].CycleMonth && registResult.CycleDay == dbresult[0].CycleDay && registResult.StartDate == dbresult[0].StartDate && registResult.DispCycle == dbresult[0].DispCycle && registResult.ScheduleTypeStructureId == dbresult[0].ScheduleTypeStructureId)
                        {
                            cycleChangeFlg = false;
                        }
                        else
                        {
                            cycleChangeFlg = true;
                        }
                    }

                    // 周期ありフラグセット
                    if ((registResult.CycleYear == null || registResult.CycleYear == 0) && (registResult.CycleMonth == null || registResult.CycleMonth == 0) && (registResult.CycleDay == null || registResult.CycleDay == 0))
                    {
                        // 周期無し
                        registResult.IsCyclic = false;
                    }
                    else
                    {
                        // 周期あり
                        registResult.IsCyclic = true;
                    }
                    // 更新
                    // 機器別管理基準部位
                    if (!registUpdateDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.UpdateManagementStandardsComponentSchedule))
                    {
                        return false;
                    }
                    // 機器別管理基準内容
                    if (!registUpdateDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.UpdateManagementStandardsContentSchedule))
                    {
                        return false;
                    }

                    Dao.schedeluListResult updResult = getRegistManagementStandard<Dao.schedeluListResult>(dicProc, TargetCtrlIdManagementStandard.DetailScheduleList160);

                    // SQL文の取得
                    if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.UpdateDispCycleOfMaintainanceSchedule, out string updateDispCycleSql))
                    {
                        return false;
                    }

                    // 表示周期のみを更新する
                    if (db.Regist(updateDispCycleSql, registResult) < 0)
                    {
                        return false;
                    }

                    // 次回実施予定日が入力されていて値が変更されている場合
                    if (updResult.NextScheduleDate != null && updResult.NextScheduleDate != updResult.NextScheduleDateDisp)
                    {
                        // SQL文の取得
                        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.UpdateScheduleDateByDetailId, out string updateScheduleSql))
                        {
                            return false;
                        }

                        // スケジュール日を入力された値に更新する
                        registResult.ScheduleDate = updResult.NextScheduleDate;
                        registResult.ScheduleDateBefore = updResult.NextScheduleDateDisp;
                        if (db.Regist(updateScheduleSql, registResult) < 0)
                        {
                            return false;
                        }
                    }

                    // スケジュールを更新 が未選択または、次回実施予定日が入力されて変更されている場合
                    if (updResult.IsUpdateSchedule.ToString() == ComConsts.CHECK_FLG.OFF ||
                         (updResult.NextScheduleDate != null && updResult.NextScheduleDate != updResult.NextScheduleDateDisp))
                    {
                        // 保全スケジュールの再作成処理は行わないのでここで終了
                        continue;
                    }

                    // 周期・開始日変更有
                    if (cycleChangeFlg)
                    {
                        // 「作成済み保全スケジュール詳細データ(開始日以降)」削除
                        if (!registDeleteDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.DeleteMaintainanceScheduleDetailRemake))
                        {
                            // エラーの場合
                            this.Status = CommonProcReturn.ProcStatus.Error;
                            return false;
                        }

                        // 変更後開始日 < 変更前開始日であれば変更前開始日レコード削除
                        if (registResult.StartDate <= dbresult[0].StartDate)
                        {
                            // 入力された日付以降の日付で設定されていた保全スケジュールデータは削除
                            if (!registDeleteDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.DeleteMaintainanceScheduleRemake))
                            {
                                // エラーの場合
                                this.Status = CommonProcReturn.ProcStatus.Error;
                                return false;
                            }
                        }

                        // 新規登録者および新規登録日付をセット
                        registResult.InsertUserId = int.Parse(this.UserId);
                        registResult.InsertDatetime = now;

                        // 保全スケジュールにデータ新規作成
                        (bool returnFlag, long id) maintainanceSchedule = registInsertDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.InsertMaintainanceSchedule);
                        if (!maintainanceSchedule.returnFlag)
                        {
                            return false;
                        }

                        // 保全スケジュール詳細にデータ新規作成
                        if (!insertMainteScheduleDetail(registResult, maintainanceSchedule.id, now))
                        {
                            return false;
                        }

                    }

                }
            }

            // 移動されたスケジュールの更新
            if (updatedScheduleList.Count > 0)
            {
                // 排他チェック
                // 画面より取得した一覧の内容を使って排他チェックをするので、更新対象のキーIDの行だけを抽出
                var updateTargetDics = rowDicList.Where(x => updateKeyIdList.IndexOf(getKeyIdByDic(x)) > -1).ToList();

                // 排他チェック
                foreach (Dictionary<string, object> updData in updateTargetDics)
                {
                    // 入力された内容を取得
                    Dao.schedeluListResult updResult = getRegistManagementStandard<Dao.schedeluListResult>(updData, TargetCtrlIdManagementStandard.DetailScheduleList160);

                    IList<Dao.schedeluListResult> dbresult = null;
                    // 最新DBデータ取得
                    dynamic whereParam = whereParam = new { ManagementStandardsComponentId = updResult.ManagementStandardsComponentId, ManagementStandardsContentId = updResult.ManagementStandardsContentId };
                    dbresult = getDbData<Dao.schedeluListResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);
                    // 排他チェック
                    if (dbresult == null || dbresult.Count == 0 || isErrorExclusiveScheduleList(updResult, dbresult, true))
                    {
                        return false;
                    }
                }

                // 登録
                if (!updateScheduleDetail(updatedScheduleList, updateTargetDics, TargetCtrlIdManagementStandard.DetailScheduleList160, factoryId))
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }

                // 行のディクショナリからキーIDを取得して返す
                string getKeyIdByDic(Dictionary<string, object> rowDic)
                {
                    var keyId = rowDic[valKeyId].ToString();
                    return keyId;
                }
            }

            initDetail();
            return true;
        }

        // 更新処理
        // List<TMQDao.ScheduleList.Display> updatedScheduleList 更新対象のスケジュール情報
        // List<Dictionary<string, object>> updateDicList 更新対象の一覧の情報
        // return エラーの場合False
        private bool updateScheduleDetail(List<TMQDao.ScheduleList.Display> updatedScheduleList, List<Dictionary<string, object>> updateDicList, string ctrlId, int factoryId, bool lank = false)
        {
            // 更新対象行共通　更新日時、ユーザID
            var now = DateTime.Now;
            int userId = int.Parse(this.UserId);
            // 工場IDリスト(共通、長期計画)
            var factoryIdList = TMQUtil.GetFactoryIdList();
            factoryIdList.Add(factoryId);

            // 年度開始月
            int nendoStartMonth = getYearStartMonth(factoryId);

            // 更新対象行で繰り返し
            foreach (var updRowDic in updateDicList)
            {
                // 更新対象行の情報をデータクラスに変換
                Dao.schedeluListResult updRowClass = new();
                SetDataClassFromDictionary(updRowDic, ctrlId, updRowClass);
                // 更新対象のスケジュールの情報をキーIDにより取得
                var updInfos = updatedScheduleList.Where(x => x.KeyId == updRowClass.KeyId);
                // それぞれのスケジュールに対して処理
                foreach (var updInfo in updInfos)
                {
                    var updCond = getUpdCond(updInfo.KeyDate, updInfo.OriginDate, updRowClass.ManagementStandardsContentId, updInfo.IsUpdateYear, factoryIdList);
                    bool result = false;
                    if (lank)
                    {
                        result = TMQUtil.SqlExecuteClass.Regist(SqlNameManagementStandard.UpdateMaintainanceScheduleLankDetail, SqlName.SubDir, updCond, this.db);
                    }
                    else
                    {
                        result = TMQUtil.SqlExecuteClass.Regist(SqlNameManagementStandard.UpdateMaintainanceScheduleDetail, SqlName.SubDir, updCond, this.db);
                    }
                    if (!result)
                    {
                        return false;
                    }
                }
            }
            return true;

            // スケジュール更新SQLの条件を作成する処理
            // DateTime updateDate 更新対象のスケジュールより取得した移動先の日付
            // DateTime originDate 更新対象のスケジュールより取得した移動元の日付
            // long managementStandardsContentId 更新対象の画面の行より取得した機器別管理基準内容ID
            Dao.Schedule.UpdateCondition getUpdCond(DateTime updateDate, DateTime originDate, long managementStandardsContentId, bool isUpdateYear, List<int> factoryIdList)
            {
                Dao.Schedule.UpdateCondition cond = new();
                cond.AddMonth = ComUtil.GetMonthDiff(updateDate, originDate); // 加算月数は移動先-移動元で算出
                cond.ManagementStandardsContentId = managementStandardsContentId;
                cond.MonthStartDate = new DateTime(originDate.Year, originDate.Month, 1); // 更新範囲開始年月日は移動元の1日
                                                                                          // 更新範囲終了年月日
                                                                                          // 年の場合→12月31日
                                                                                          // 年月の場合→移動先の月の最終日
                cond.MonthEndDate = isUpdateYear ? ComUtil.GetNendoLastDay(originDate, nendoStartMonth) : ComUtil.GetDateMonthLastDay(originDate);
                cond.UpdateUserId = userId;
                cond.UpdateDatetime = now;
                // 工場ID
                cond.FactoryIdList = factoryIdList;
                return cond;
            }
        }

        /// <summary>
        /// 点検種別スケジューリング一覧登録
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool registLankScheduleList()
        {
            // リスト取得
            IList<Dictionary<string, object>> result = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlIdManagementStandard.DetailLankScheduleList170);
            DateTime now = DateTime.Now;

            // 入力値チェック(横方向一覧の直接入力は共通側でチェック不可のため業務側で行う)
            if (isInputvalCheck(result, true))
            {
                return false;
            }

            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();
            var info = getResultMappingInfo(TargetCtrlIdManagementStandard.DetailLankScheduleList170);

            // -------------------- この部分はスケジュール表の更新時に使用するが、入力チェックで必要になるためここで取得 --------------------
            // 更新対象の一覧の内容を取得
            var rowDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlIdManagementStandard.DetailLankScheduleList170);
            // 一覧のキー値のVAL値を取得(スケジュールとの紐付に使用)
            var valKeyId = GetValKeyIdForSchedule(TargetCtrlIdManagementStandard.DetailLankScheduleList170);
            // 同一マークのキー値のVAL値を取得(スケジュールとの紐付に使用)
            var valSameMarkKey = GetValSameMarkKeyForSchedule(TargetCtrlIdManagementStandard.DetailLankScheduleList170);

            // 入力された内容を取得
            Dao.managementStandardResult machineInfo = getRegistManagementStandard<Dao.managementStandardResult>(result[0], TargetCtrlIdManagementStandard.DetailLankScheduleList170);
            // 対象機器の工場ID取得
            dynamic whereParamF = new { MachineId = machineInfo.MachineId };
            string sql;
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParamF);
            //if (results == null || results.Count == 0)
            //{
            //    return false;
            //}
            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
            typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, typeLst, this.db, this.LanguageId);
            //int factoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
            int factoryId = (int)results[0].FactoryId;

            // 年度開始月の取得
            int monthStartNendo = getYearStartMonth(factoryId);
            // 一覧より更新されたスケジュールの情報を取得
            List<TMQDao.ScheduleList.Display> updatedScheduleList = TMQUtil.ScheduleListUtil.GetScheduleUpdatedData(rowDicList, valKeyId, monthStartNendo);

            // 更新対象のキーIDの一覧を取得(画面のディクショナリとの紐づけに使用)
            var updateKeyIdList = updatedScheduleList.Select(x => x.KeyId).ToList();
            // -------------------- この部分はスケジュール表の更新時に使用するが、入力チェックで必要になるためここで取得 --------------------

            // 排他チェック、入力チェック
            foreach (var row in result)
            {
                // 変更フラグが立っているもの
                if (isUpdatedTableRow(row))
                {
                    // 入力された内容を取得
                    Dao.schedeluListResult updResult = getRegistManagementStandard<Dao.schedeluListResult>(row, TargetCtrlIdManagementStandard.DetailLankScheduleList170);

                    IList<Dao.schedeluListResult> dbresult = null;
                    // 最新DBデータ取得
                    dynamic whereParam = whereParam = new { ManagementStandardsComponentId = updResult.ManagementStandardsComponentId, ManagementStandardsContentId = updResult.ManagementStandardsContentId };
                    // dbresult = getDbData<Dao.schedeluListResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);
                    dbresult = getDbData<Dao.schedeluListResult>(SqlNameManagementStandard.GetMaintainanceScheduleLankUpdCheck, whereParam);
                    // 排他チェック
                    if (dbresult == null || dbresult.Count == 0 || isErrorExclusiveScheduleList(updResult, dbresult, true))
                    {
                        return false;
                    }

                    IList<Dao.managementStandardResult> chkDbResult = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);
                    Dao.managementStandardResult chkUpdResult = getRegistManagementStandard<Dao.managementStandardResult>(row, TargetCtrlIdManagementStandard.DetailLankScheduleList170);

                    // 入力チェック
                    if (isErrorRegistManagementStandard(chkUpdResult, false, chkDbResult, TargetCtrlIdManagementStandard.DetailLankScheduleList170, row))
                    {
                        return false;
                    }

                    if (chkDbResult != null)
                    {
                        if (!(chkUpdResult.CycleYear == chkDbResult[0].CycleYear && chkUpdResult.CycleMonth == chkDbResult[0].CycleMonth && chkUpdResult.CycleDay == chkDbResult[0].CycleDay && chkUpdResult.StartDate == chkDbResult[0].StartDate))
                        {
                            // 更新時確認チェック(周期・開始日変更チェック 点検種別毎時周期・開始日変更チェック)
                            if (this.Status < CommonProcReturn.ProcStatus.Confirm)
                            {
                                if (isConfirmRegistCycle(chkUpdResult, chkDbResult[0].MaintainanceKindManage))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {

                        // 更新時確認チェック(周期・開始日変更チェック 点検種別毎時周期・開始日変更チェック)
                        if (this.Status < CommonProcReturn.ProcStatus.Confirm)
                        {
                            if (isConfirmRegistCycle(chkUpdResult, chkUpdResult.MaintainanceKindManage))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            // 同一点検種別内に開始日以降で保全活動が紐づいている者があるかチェック
                            if (isErrorRegistMaintainanceKindManage(chkUpdResult, TargetCtrlIdManagementStandard.DetailLankScheduleList170))
                            {
                                return false;
                            }
                        }
                    }

                    // 過去日のチェックは行わない
                    //// 次回実施予定日に過去日が入力されている場合
                    //if (updResult.NextScheduleDate != null && updResult.NextScheduleDate < DateTime.Now.Date)
                    //{
                    //    // エラー情報格納クラス
                    //    ErrorInfo errorInfo = new ErrorInfo(row);
                    //    // 過去日付は設定できません。
                    //    string errMsg = GetResMessage("141060003");
                    //    string val = info.getValName("next_schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    //    errorInfo.setError(errMsg, val);
                    //    errorInfoDictionary.Add(errorInfo.Result);
                    //}

                    // 次回実施予定日が入力されている場合
                    if (updResult.NextScheduleDate != null)
                    {
                        // 次々回実施予定日を取得
                        List<Dao.managementStandardResult> nextDate = (List<Dao.managementStandardResult>)getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetNextScheduleDate, new { ManagementStandardsContentId = updResult.ManagementStandardsContentId, ScheduleDateBefore = updResult.NextScheduleDateDisp, MaintainanceScheduleId = updResult.MaintainanceScheduleId });

                        // 取得できた場合は範囲チェックをする
                        if (nextDate != null && nextDate.Count > 0)
                        {
                            bool isDateErr = false;

                            // 入力された次回実施予定日が次々回実施予定日以降の場合
                            if (updResult.NextScheduleDate >= nextDate[0].ScheduleDate)
                            {
                                isDateErr = true;
                            }

                            // 前回実施予定日は存在しない可能性があることを考慮する
                            if (nextDate.Count > 1)
                            {
                                // 入力された次回実施予定日が前回実施予定日の場合以前
                                if (updResult.NextScheduleDate <= nextDate[1].ScheduleDate)
                                {
                                    isDateErr = true;
                                }
                            }
                            else
                            {
                                // 前回実施予定日が存在しない場合は開始日と比較する
                                if (updResult.NextScheduleDate < updResult.StartDate)
                                {
                                    isDateErr = true;
                                }
                            }

                            // 日付の範囲エラーの場合はメッセージをセットする
                            if (isDateErr)
                            {
                                // エラー情報格納クラス
                                ErrorInfo errorInfo = new ErrorInfo(row);
                                // 次回実施予定日は前回実施予定日～次々回実施予定日以内の日付を指定してください。
                                string errMsg = GetResMessage("141120020");
                                string val = info.getValName("next_schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                                errorInfo.setError(errMsg, val);
                                errorInfoDictionary.Add(errorInfo.Result);
                            }
                        }
                    }

                    // スケジュールを更新 が未選択でかつ、次回実施予定日が入力されている場合
                    // 確認メッセージを表示する(※メッセージを表示する必要がある時はTrueが返る)
                    if (confirmByIsUpdateAndScheduleDate(new Dao.managementStandardResult() { IsUpdateSchedule = updResult.IsUpdateSchedule, ScheduleDate = updResult.NextScheduleDate }))
                    {
                        return true;
                    }

                    // 周期または開始日が変更されていて、スケジュールを更新 が未選択の場合
                    if (confirmByScheduleInfoChanged(new Dao.managementStandardResult()
                    {
                        CycleYear = updResult.CycleYear,
                        CycleMonth = updResult.CycleMonth,
                        CycleDay = updResult.CycleDay,
                        StartDate = updResult.StartDate,
                        IsUpdateSchedule = updResult.IsUpdateSchedule
                    }, chkDbResult))
                    {
                        return true;
                    }

                    // 開始日または周期(年・月・日)が変更されているかどうか
                    // いずれかが変更されている場合はフラグ = True
                    bool cycleChangeFlg;
                    if (updResult.CycleYear == dbresult[0].CycleYear &&
                        updResult.CycleMonth == dbresult[0].CycleMonth &&
                        updResult.CycleDay == dbresult[0].CycleDay &&
                        updResult.StartDate == dbresult[0].StartDate)
                    {
                        cycleChangeFlg = false;
                    }
                    else
                    {
                        cycleChangeFlg = true;
                    }

                    // 次回実施予定日が入力されているかつ値が変更されていて、開始日または周期(年・月・日)が変更されている場合
                    if (updResult.NextScheduleDate != null && updResult.NextScheduleDateDisp != updResult.NextScheduleDate && cycleChangeFlg)
                    {
                        // エラー情報格納クラス
                        ErrorInfo errorInfo = new ErrorInfo(row);
                        // 周期または開始日が変更されていいます。　同時に次回実施予定日は更新できません。
                        string errMsg = GetResMessage("141120019");
                        string val = info.getValName("next_schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                        errorInfo.setError(errMsg, val);
                        errorInfoDictionary.Add(errorInfo.Result);
                    }

                    // 次回実施予定日が変更されていて、スケジュールも変更されている場合
                    // 同時には変更できないのでエラーとする
                    if (updResult.NextScheduleDate != updResult.NextScheduleDateDisp && updateKeyIdList.Contains(updResult.KeyId))
                    {
                        // エラー情報格納クラス
                        ErrorInfo errorInfo = new ErrorInfo(row);
                        // 星取表と次回実施予定日を同時に変更することはできません。
                        string val = info.getValName("next_schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                        errorInfo.setError(GetResMessage("141300010"), val);
                        errorInfoDictionary.Add(errorInfo.Result);
                    }
                }
            }

            // 入力エラーがある場合はここで終了
            if (errorInfoDictionary.Count > 0)
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return false;
            }

            foreach (Dictionary<string, object> dicProc in result)
            {
                // 変更フラグが立っているもの
                if (isUpdatedTableRow(dicProc))
                {
                    // 入力された内容を取得
                    Dao.managementStandardResult registResult = getRegistManagementStandard<Dao.managementStandardResult>(dicProc, TargetCtrlIdManagementStandard.DetailLankScheduleList170);

                    IList<Dao.managementStandardResult> dbresult = null;
                    dynamic whereParam = whereParam = new { ManagementStandardsComponentId = registResult.ManagementStandardsComponentId, ManagementStandardsContentId = registResult.ManagementStandardsContentId };
                    dbresult = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);

                    // 機器別管理基準フラグセット
                    registResult.IsManagementStandardConponent = true;
                    // 更新者および更新日付をセット
                    registResult.UpdateUserId = int.Parse(this.UserId);
                    registResult.UpdateDatetime = now;

                    // 周期変更有チェック
                    bool cycleChangeFlg = false;

                    // 周期・開始日・表示周期・スケジュール管理が変更されている
                    if (dbresult != null)
                    {
                        if (registResult.CycleYear == dbresult[0].CycleYear && registResult.CycleMonth == dbresult[0].CycleMonth && registResult.CycleDay == dbresult[0].CycleDay && registResult.StartDate == dbresult[0].StartDate && registResult.DispCycle == dbresult[0].DispCycle && registResult.ScheduleTypeStructureId == dbresult[0].ScheduleTypeStructureId)
                        {
                            cycleChangeFlg = false;
                        }
                        else
                        {
                            cycleChangeFlg = true;
                        }
                    }

                    // 周期ありフラグセット
                    if ((registResult.CycleYear == null || registResult.CycleYear == 0) && (registResult.CycleMonth == null || registResult.CycleMonth == 0) && (registResult.CycleDay == null || registResult.CycleDay == 0))
                    {
                        // 周期無し
                        registResult.IsCyclic = false;
                    }
                    else
                    {
                        // 周期あり
                        registResult.IsCyclic = true;
                    }
                    // 更新
                    // 機器別管理基準部位
                    if (!registUpdateDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.UpdateManagementStandardsComponentSchedule))
                    {
                        return false;
                    }
                    // 機器別管理基準内容
                    if (!registUpdateDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.UpdateManagementStandardsContentSchedule))
                    {
                        return false;
                    }

                    // 同一点検種別のスケジュール基準更新
                    if (!updateManagementContentKindManage(registResult, now))
                    {
                        return false;
                    }

                    // 自身のレコードの点検種別と同一の点検種別のデータを取得
                    // ※機器が点検種別毎管理でない場合または自身のレコードの点検種別と同一の点検種別のデータが存在しない場合 は取得されない
                    if (!getSameManageKindData(registResult, out IList<Dao.managementStandardResult> sameManageKindList))
                    {
                        return false;
                    }

                    Dao.schedeluListResult updResult = getRegistManagementStandard<Dao.schedeluListResult>(dicProc, TargetCtrlIdManagementStandard.DetailLankScheduleList170);

                    // SQL文の取得
                    if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.UpdateDispCycleOfMaintainanceSchedule, out string updateDispCycleSql))
                    {
                        return false;
                    }

                    // 表示周期のみを更新する
                    if (db.Regist(updateDispCycleSql, registResult) < 0)
                    {
                        return false;
                    }

                    // 自身のレコードと同一の点検種別のデータの表示周期も更新する
                    if (!updateDispCycle(registResult, sameManageKindList))
                    {
                        return false;
                    }

                    // 次回実施予定日が入力されていて値が変更されている場合
                    if (updResult.NextScheduleDate != null && updResult.NextScheduleDate != updResult.NextScheduleDateDisp)
                    {
                        // SQL文の取得
                        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.UpdateScheduleDateByDetailId, out string updateScheduleSql))
                        {
                            return false;
                        }

                        // スケジュール日を入力された値に更新する
                        registResult.ScheduleDate = updResult.NextScheduleDate;
                        registResult.ScheduleDateBefore = updResult.NextScheduleDateDisp;
                        if (db.Regist(updateScheduleSql, registResult) < 0)
                        {
                            return false;
                        }

                        // 自身のレコードと同一の点検種別のデータの次回実施予定日も更新する
                        if (!updateNextScheduleDate(registResult, sameManageKindList))
                        {
                            return false;
                        }
                    }

                    // スケジュールを更新 が未選択または、次回実施予定日が入力されて変更されている場合
                    if (updResult.IsUpdateSchedule.ToString() == ComConsts.CHECK_FLG.OFF ||
                        (updResult.NextScheduleDate != null && updResult.NextScheduleDate != updResult.NextScheduleDateDisp))
                    {
                        // 保全スケジュールの再作成処理は行わないのでここで終了
                        continue;
                    }

                    // 周期・開始日変更有
                    if (cycleChangeFlg)
                    {
                        // 「作成済み保全スケジュール詳細データ(開始日以降)」削除
                        if (!registDeleteDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.DeleteMaintainanceScheduleDetailRemake))
                        {
                            // エラーの場合
                            this.Status = CommonProcReturn.ProcStatus.Error;
                            return false;
                        }

                        // 変更後開始日 < 変更前開始日であれば変更前開始日レコード削除
                        if (registResult.StartDate <= dbresult[0].StartDate)
                        {
                            // 入力された日付以降の日付で設定されていた保全スケジュールデータは削除
                            if (!registDeleteDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.DeleteMaintainanceScheduleRemake))
                            {
                                // エラーの場合
                                this.Status = CommonProcReturn.ProcStatus.Error;
                                return false;
                            }
                        }

                        // 新規登録者および新規登録日付をセット
                        registResult.InsertUserId = int.Parse(this.UserId);
                        registResult.InsertDatetime = now;

                        // 保全スケジュールにデータ新規作成
                        (bool returnFlag, long id) maintainanceSchedule = registInsertDb<Dao.managementStandardResult>(registResult, SqlNameManagementStandard.InsertMaintainanceSchedule);
                        if (!maintainanceSchedule.returnFlag)
                        {
                            return false;
                        }

                        // 保全スケジュール詳細にデータ新規作成
                        if (!insertMainteScheduleDetail(registResult, maintainanceSchedule.id, now))
                        {
                            return false;
                        }

                        // 点検種別毎管理機器なら同点検種の周期なども変更
                        registResult.MaintainanceScheduleId = maintainanceSchedule.id;
                        if (!updateMaintainanceKindManage(registResult, now))
                        {
                            return false;
                        }
                    }
                }
            }

            // 移動されたスケジュールの更新
            if (updatedScheduleList.Count > 0)
            {
                // 排他チェック
                // 画面より取得した一覧の内容を使って排他チェックをするので、更新対象のキーIDの行だけを抽出
                var updateTargetDics = rowDicList.Where(x => updateKeyIdList.IndexOf(getKeyIdByDic(x)) > -1).ToList();

                // 排他チェック対象リスト
                // 更新対象の列に加え、更新対象の列と同じ同一スケジュールマークキーの列も取得
                var checkTargetDics = getExclusiveTargets(rowDicList, updateTargetDics);

                // 排他チェック
                foreach (Dictionary<string, object> updData in checkTargetDics)
                {
                    // 入力された内容を取得
                    Dao.schedeluListResult updResult = getRegistManagementStandard<Dao.schedeluListResult>(updData, TargetCtrlIdManagementStandard.DetailLankScheduleList170);

                    IList<Dao.schedeluListResult> dbresult = null;
                    // 最新DBデータ取得
                    dynamic whereParam = whereParam = new { ManagementStandardsComponentId = updResult.ManagementStandardsComponentId, ManagementStandardsContentId = updResult.ManagementStandardsContentId };
                    dbresult = getDbData<Dao.schedeluListResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);
                    // 排他チェック
                    if (dbresult == null || dbresult.Count == 0 || isErrorExclusiveScheduleList(updResult, dbresult, true))
                    {
                        return false;
                    }
                }

                // 登録
                if (!updateScheduleDetail(updatedScheduleList, updateTargetDics, TargetCtrlIdManagementStandard.DetailLankScheduleList170, factoryId, true))
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }

            }
            initDetail();
            return true;

            // 行のディクショナリからキーIDを取得して返す
            string getKeyIdByDic(Dictionary<string, object> rowDic)
            {
                var keyId = rowDic[valKeyId].ToString();
                return keyId;
            }
            // 一覧のすべての行と更新対象行から、排他チェック対象行を取得する
            // List<Dictionary<string, object>> rowDicList 一覧のすべての行
            // List<Dictionary<string, object>> updateTargetDics 上記を絞り込んだ、更新対象行
            List<Dictionary<string, object>> getExclusiveTargets(List<Dictionary<string, object>> rowDicList, List<Dictionary<string, object>> updateTargetDics)
            {
                // 更新対象行と同じ値の同一スケジュールキーを持つ一覧の行を取得する
                List<Dictionary<string, object>> targets = new();
                foreach (var updRow in updateTargetDics)
                {
                    // 同一スケジュールキー
                    string markKey = getSameMarkKeyByDic(updRow);
                    // 一覧の行の内、同じもの
                    var addRows = rowDicList.Where(x => markKey == getSameMarkKeyByDic(x)).ToList();
                    targets.AddRange(addRows);
                }
                // キーIDで重複を排除
                var rtnTargets = targets.GroupBy(x => getKeyIdByDic(x)).Select(x => x.First()).ToList();
                return rtnTargets;

                // 行のディクショナリから同一スケジュールキーを取得して返す
                string getSameMarkKeyByDic(Dictionary<string, object> rowDic)
                {
                    var keyId = rowDic[valSameMarkKey].ToString();
                    return keyId;
                }
            }
        }

        /// <summary>
        /// 入力値チェック
        /// </summary>
        /// <param name="result">入力値</param>
        /// <param name="lankFlg">点検種別毎かどうか</param>
        /// <returns>エラーの場合True</returns>
        private bool isInputvalCheck(IList<Dictionary<string, object>> result, bool lankFlg = false)
        {
            bool error = false;

            Dao.schedeluListResult updResult = new Dao.schedeluListResult();

            // 各エラー行
            List<int> intChkCycleYear = new List<int>();
            List<int> intChkCycleMonth = new List<int>();
            List<int> intChkCycleDay = new List<int>();

            List<int> minMaxChkCycleYear = new List<int>();
            List<int> minMaxChkCycleMonth = new List<int>();
            List<int> minMaxChkCycleDay = new List<int>();

            List<int> lengthChkDispCycle = new List<int>();

            List<int> nullChkStartDate = new List<int>();
            List<int> dateChkStartDate = new List<int>();

            int rowNo = 0;
            int val = 0;
            DateTime dt = DateTime.Now;
            foreach (var row in result)
            {
                rowNo = rowNo + 1;
                // 変更フラグが立っているもの
                if (isUpdatedTableRow(row))
                {
                    // 周期(年)数値チェック
                    if (getDictionaryKeyValue(row, "cycle_year").Length > 0 && !int.TryParse(getDictionaryKeyValue(row, "cycle_year"), out val))
                    {
                        intChkCycleYear.Add(rowNo);
                    }
                    else
                    {
                        // 周期(年)上下限チェック
                        if (getDictionaryKeyValue(row, "cycle_year").Length > 0 && (int.Parse(getDictionaryKeyValue(row, "cycle_year")) < 0 || int.Parse(getDictionaryKeyValue(row, "cycle_year")) > 99))
                        {
                            minMaxChkCycleYear.Add(rowNo);
                        }
                    }

                    // 周期(月)数値チェック
                    if (getDictionaryKeyValue(row, "cycle_month").Length > 0 && !int.TryParse(getDictionaryKeyValue(row, "cycle_month"), out val))
                    {
                        intChkCycleMonth.Add(rowNo);
                    }
                    else
                    {
                        // 周期(月)上下限チェック
                        if (getDictionaryKeyValue(row, "cycle_month").Length > 0 && (int.Parse(getDictionaryKeyValue(row, "cycle_month")) < 0 || int.Parse(getDictionaryKeyValue(row, "cycle_month")) > 99))
                        {
                            minMaxChkCycleMonth.Add(rowNo);
                        }
                    }

                    // 周期(日)数値チェック
                    if (getDictionaryKeyValue(row, "cycle_day").Length > 0 && !int.TryParse(getDictionaryKeyValue(row, "cycle_day"), out val))
                    {
                        intChkCycleDay.Add(rowNo);
                    }
                    else
                    {
                        // 周期(日)上下限チェック
                        if (getDictionaryKeyValue(row, "cycle_day").Length > 0 && (int.Parse(getDictionaryKeyValue(row, "cycle_day")) < 0 || int.Parse(getDictionaryKeyValue(row, "cycle_day")) > 999))
                        {
                            minMaxChkCycleDay.Add(rowNo);
                        }
                    }

                    // 表示周期桁数チェック
                    if (getDictionaryKeyValue(row, "disp_cycle").Length > 20)
                    {
                        lengthChkDispCycle.Add(rowNo);
                    }

                    // 開始日必須チェック
                    if (getDictionaryKeyValue(row, "start_date").Length < 1)
                    {
                        nullChkStartDate.Add(rowNo);
                    }
                    else
                    {
                        // 開始日形式チェック
                        if (!DateTime.TryParse(getDictionaryKeyValue(row, "start_date"), out dt))
                        {
                            dateChkStartDate.Add(rowNo);
                        }
                    }

                }
            }

            // 周期(年)数値チェック
            if (intChkCycleYear.Count > 0)
            {
                string errRow = intChkCycleYear[0].ToString();
                for (int i = 1; i < intChkCycleYear.Count; i++)
                {
                    errRow = errRow + "," + intChkCycleYear[i].ToString();
                }
                // 周期(年)の値が不正です
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage("941250001"), GetResMessage("111120036")) + "(" + errRow + GetResMessage("141070004") + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage("941250001"), GetResMessage("111120036")) + "(" + errRow + GetResMessage("141070004") + ")";
                }
                error = true;
            }

            // 周期(月)数値チェック
            if (intChkCycleMonth.Count > 0)
            {
                string errRow = intChkCycleMonth[0].ToString();
                for (int i = 1; i < intChkCycleMonth.Count; i++)
                {
                    errRow = errRow + "," + intChkCycleMonth[i].ToString();
                }
                // 周期(月)の値が不正です
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage("941250001"), GetResMessage("111120037")) + "(" + errRow + GetResMessage("141070004") + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage("941250001"), GetResMessage("111120037")) + "(" + errRow + GetResMessage("141070004") + ")";
                }
                error = true;
            }

            // 周期(日)数値チェック
            if (intChkCycleDay.Count > 0)
            {
                string errRow = intChkCycleDay[0].ToString();
                for (int i = 1; i < intChkCycleDay.Count; i++)
                {
                    errRow = errRow + "," + intChkCycleDay[i].ToString();
                }
                // 周期(日)の値が不正です
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage("941250001"), GetResMessage("111120038")) + "(" + errRow + GetResMessage("141070004") + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage("941250001"), GetResMessage("111120038")) + "(" + errRow + GetResMessage("141070004") + ")";
                }
                error = true;
            }

            // 周期(年)上下限チェック
            if (minMaxChkCycleYear.Count > 0)
            {
                string errRow = minMaxChkCycleYear[0].ToString();
                for (int i = 1; i < minMaxChkCycleYear.Count; i++)
                {
                    errRow = errRow + "," + minMaxChkCycleYear[i].ToString();
                }
                // 周期(年)は0～99で入力してください
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage("941260009"), GetResMessage("111120036"), "0～99") + "(" + errRow + GetResMessage("141070004") + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage("941260009"), GetResMessage("111120036"), "0～99") + "(" + errRow + GetResMessage("141070004") + ")";
                }
                error = true;
            }

            // 周期(月)上下限チェック
            if (minMaxChkCycleMonth.Count > 0)
            {
                string errRow = minMaxChkCycleMonth[0].ToString();
                for (int i = 1; i < minMaxChkCycleMonth.Count; i++)
                {
                    errRow = errRow + "," + minMaxChkCycleMonth[i].ToString();
                }
                // 周期(月)は0～99で入力してください
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage("941260009"), GetResMessage("111120037"), "0～99") + "(" + errRow + GetResMessage("141070004") + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage("941260009"), GetResMessage("111120037"), "0～99") + "(" + errRow + GetResMessage("141070004") + ")";
                }
                error = true;
            }

            // 周期(日)上下限チェック
            if (minMaxChkCycleDay.Count > 0)
            {
                string errRow = minMaxChkCycleDay[0].ToString();
                for (int i = 1; i < minMaxChkCycleDay.Count; i++)
                {
                    errRow = errRow + "," + minMaxChkCycleDay[i].ToString();
                }
                // 周期(日)は0～999で入力してください
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage("941260009"), GetResMessage("111120038"), "0～999") + "(" + errRow + GetResMessage("141070004") + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage("941260009"), GetResMessage("111120038"), "0～999") + "(" + errRow + GetResMessage("141070004") + ")";
                }
                error = true;
            }

            // 表示周期チェック
            if (lengthChkDispCycle.Count > 0)
            {
                string errRow = lengthChkDispCycle[0].ToString();
                for (int i = 1; i < lengthChkDispCycle.Count; i++)
                {
                    errRow = errRow + "," + lengthChkDispCycle[i].ToString();
                }
                // 表示周期は20桁以下で入力してください
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage("941260004"), GetResMessage("111270014"), "20") + "(" + errRow + GetResMessage("141070004") + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage("941260004"), GetResMessage("111270014"), "20") + "(" + errRow + GetResMessage("141070004") + ")";
                }
                error = true;
            }

            // 開始日チェック
            if (nullChkStartDate.Count > 0)
            {
                string errRow = nullChkStartDate[0].ToString();
                for (int i = 1; i < nullChkStartDate.Count; i++)
                {
                    errRow = errRow + "," + nullChkStartDate[i].ToString();
                }
                // 表示周期は20桁以下で入力してください
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage("941260007"), GetResMessage("111060002")) + "(" + errRow + GetResMessage("141070004") + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage("941260007"), GetResMessage("111060002")) + "(" + errRow + GetResMessage("141070004") + ")";
                }
                error = true;
            }

            // 開始日チェック
            if (dateChkStartDate.Count > 0)
            {
                string errRow = dateChkStartDate[0].ToString();
                for (int i = 1; i < dateChkStartDate.Count; i++)
                {
                    errRow = errRow + "," + dateChkStartDate[i].ToString();
                }
                // 開始日の値が不正です
                if (this.MsgId.Length > 0)
                {
                    this.MsgId = this.MsgId + Environment.NewLine + string.Format(GetResMessage("941250001"), GetResMessage("111060002")) + "(" + errRow + GetResMessage("141070004") + ")";
                }
                else
                {
                    this.MsgId = string.Format(GetResMessage("941250001"), GetResMessage("111060002")) + "(" + errRow + GetResMessage("141070004") + ")";
                }
                error = true;
            }
            return error;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistManagementStandard<T>(Dictionary<string, object> result, string ctrlId)
        where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // データクラスに変換
            T data = new();
            // 更新日付を排他チェックに使用するためSetExecuteConditionByDataClassは使用しません
            if (!SetDataClassFromDictionary(result, ctrlId, data))
            {
                return data;
            }
            //if (!SetExecuteConditionByDataClass<T>(result, TargetCtrlIdManagementStandard.DetailManagementStandard90, resultInfo, DateTime.Now, this.UserId, this.UserId))
            //{
            //    // エラーの場合終了
            //    return resultInfo;
            //}

            return data;
        }

        /// <summary>
        /// 排他チェック(保全項目一覧用[更新日付で排他チェック])
        /// </summary>
        /// <returns>排他エラー:True エラーなし:False</returns>
        private bool isErrorExclusiveManagementStandard(Dao.managementStandardResult registResult, IList<Dao.managementStandardResult> dbresult)
        {
            if (registResult.UpdateDatetime < dbresult[0].UpdateDatetime)
            {
                setExclusiveError();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 排他チェック(様式１一覧用[更新日付で排他チェック])
        /// </summary>
        /// <returns>排他エラー:True エラーなし:False</returns>
        private bool isErrorExclusiveFormat1List(Dao.format1ListResult registResult, IList<Dao.format1ListResult> dbresult)
        {
            if (registResult.UpdateDatetime < dbresult[0].UpdateDatetime)
            {
                setExclusiveError();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 排他チェック(保全項目一覧用[更新日付で排他チェック])
        /// </summary>
        /// <returns>排他エラー:True エラーなし:False</returns>
        private bool isErrorExclusiveScheduleList(Dao.schedeluListResult registResult, IList<Dao.schedeluListResult> dbresult, bool detailChek = false)
        {
            if (registResult.UpdateDatetime < dbresult[0].UpdateDatetime)
            {
                setExclusiveError();
                return true;
            }

            if (detailChek)
            {
                if (registResult.UpdateDatetimeSch < dbresult[0].UpdateDatetimeSch)
                {
                    setExclusiveError();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 保全項目編集画面入力チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorRegistManagementStandard(Dao.managementStandardResult res, bool insertFlg, IList<Dao.managementStandardResult> dbres, string ctrlId, Dictionary<string, object> targetDic, bool isManagementStandard = false)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // チェック
            if (isErrorRegistManagementStandardForSingle(ref errorInfoDictionary, res, insertFlg, ctrlId, targetDic))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            // メソッド内処理
            bool isErrorRegistManagementStandardForSingle(ref List<Dictionary<string, object>> errorInfoDictionary, Dao.managementStandardResult result, bool insertFlg, string ctrlId, Dictionary<string, object> targetDic)
            {
                bool isError = false;   // 処理全体でエラーの有無を保持
                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(ctrlId);
                // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                //// 単一の内容を取得
                //Dictionary<string, object> targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);

                // 同一機器、部位、保全項目重複チェック
                if (!checkContentDuplicate(result))
                {
                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                    isError = true;
                    // 入力された部位、保全項目の組み合わせは既に登録されています。
                    string errMsg = GetResMessage("141220002");
                    string val = info.getValName("inspection_site_structure_id"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val);
                    val = info.getValName("inspection_content_structure_i");     // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val);
                    errorInfoDictionary.Add(errorInfo.Result);
                }

                // 2022.08.08 部位で部位重要度と保全方式は一意になるとは限らない
                //// 部位重要度、部位保全方式複数指定チェック
                //if (!checkInspectionMultiExist(result))
                //{
                //    // エラー情報格納クラス
                //    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                //    isError = true;
                //    // １つの部位に対して複数の部位重要度、保全方式は設定できません。
                //    string errMsg = GetResMessage("141270001");
                //    string val = info.getValName("inspection_site_importance_str"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                //    errorInfo.setError(errMsg, val);
                //    val = info.getValName("inspection_site_conservation_s");     // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                //    errorInfo.setError(errMsg, val);
                //    errorInfoDictionary.Add(errorInfo.Result);
                //}

                // 更新時なら周期・開始日変更チェック
                if (!insertFlg)
                {
                    // 周期または開始日が変更されている
                    if (res.CycleYear != dbres[0].CycleYear || res.CycleMonth != dbres[0].CycleMonth || res.CycleDay != dbres[0].CycleDay || res.StartDate != dbres[0].StartDate)
                    {
                        // 開始日が過去日だった際はエラー
                        if (res.StartDate < DateTime.Now.Date)
                        {
                            // エラー情報格納クラス
                            ErrorInfo errorInfo = new ErrorInfo(targetDic);
                            isError = true;
                            // 過去日付は設定できません。
                            string errMsg = GetResMessage("141060003");
                            string val = info.getValName("start_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                            errorInfo.setError(errMsg, val);
                            errorInfoDictionary.Add(errorInfo.Result);
                        }

                        // 開始日以降に保全活動が紐づいているスケジュールが存在する際はエラーとする
                        if (!checkScheduleAfterMsSummry(result))
                        {
                            // エラー情報格納クラス
                            ErrorInfo errorInfo = new ErrorInfo(targetDic);
                            isError = true;
                            // 開始日には保全活動が登録されたスケジュール以降の日付を設定してください。
                            string errMsg = GetResMessage("141060005");
                            string val = info.getValName("start_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                            errorInfo.setError(errMsg, val);
                            errorInfoDictionary.Add(errorInfo.Result);
                        }

                    }
                }

                // 過去日のチェックは行わない
                //// 保全項目編集画面の登録で、次回実施予定日に初期表示した値より過去日が入力されている場合
                //if (isManagementStandard && result.ScheduleDate != null && result.ScheduleDateBefore != null && result.ScheduleDate < result.ScheduleDateBefore)
                //{
                //    // エラー情報格納クラス
                //    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                //    isError = true;
                //    // 過去日付は設定できません。
                //    string errMsg = GetResMessage("141060003");
                //    string val = info.getValName("schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                //    errorInfo.setError(errMsg, val);
                //    errorInfoDictionary.Add(errorInfo.Result);
                //}

                // 保全項目編集画面の登録で、次回実施予定日が入力されている場合
                if (isManagementStandard && result.ScheduleDate != null)
                {
                    // 次々回実施予定日を取得
                    List<Dao.managementStandardResult> nextDate = (List<Dao.managementStandardResult>)getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetNextScheduleDate, new { ManagementStandardsContentId = result.ManagementStandardsContentId, ScheduleDateBefore = result.ScheduleDateBefore, MaintainanceScheduleId = result.MaintainanceScheduleId });

                    // 取得できた場合は範囲チェックをする
                    if (nextDate != null && nextDate.Count > 0)
                    {
                        bool isDateErr = false;

                        // 入力された次回実施予定日が次々回実施予定日以降の場合
                        if (result.ScheduleDate >= nextDate[0].ScheduleDate)
                        {
                            isDateErr = true;
                        }

                        // 前回実施予定日は存在しない可能性があることを考慮する
                        if (nextDate.Count > 1)
                        {
                            // 入力された次回実施予定日が前回実施予定日の場合以前
                            if (result.ScheduleDate <= nextDate[1].ScheduleDate)
                            {
                                isDateErr = true;
                            }
                        }
                        else
                        {
                            // 前回実施予定日が存在しない場合は開始日と比較する
                            if (result.ScheduleDate < result.StartDate)
                            {
                                isDateErr = true;
                            }
                        }

                        // 日付の範囲エラーの場合はメッセージをセットする
                        if (isDateErr)
                        {
                            // エラー情報格納クラス
                            ErrorInfo errorInfo = new ErrorInfo(targetDic);
                            isError = true;
                            // 次回実施予定日は前回実施予定日～次々回実施予定日以内の日付を指定してください。
                            string errMsg = GetResMessage("141120020");
                            string val = info.getValName("schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                            errorInfo.setError(errMsg, val);
                            errorInfoDictionary.Add(errorInfo.Result);
                        }
                    }
                }

                // 保全項目編集画面の登録で、新規登録かつ、スケジュールを更新 が未選択の場合
                if (isManagementStandard && insertFlg && result.IsUpdateSchedule.ToString() == ComConsts.CHECK_FLG.OFF)
                {
                    // エラー情報格納クラス
                    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                    isError = true;
                    // 新規登録情報の為　「スケジュールを更新」にチェックを付与してください。
                    string errMsg = GetResMessage("141120017");
                    string val = info.getValName("is_update_schedule"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val);
                    errorInfoDictionary.Add(errorInfo.Result);
                }

                // 保全項目編集画面の登録の場合
                if (isManagementStandard && dbres != null)
                {
                    // 開始日または周期(年・月・日)が変更されているかどうか
                    // いずれかが変更されている場合はフラグ = True
                    bool cycleChangeFlg;
                    if (result.CycleYear == dbres[0].CycleYear && result.CycleMonth == dbres[0].CycleMonth && result.CycleDay == dbres[0].CycleDay && result.StartDate == dbres[0].StartDate)
                    {
                        cycleChangeFlg = false;
                    }
                    else
                    {
                        cycleChangeFlg = true;
                    }

                    // 次回実施予定日が入力されているかつ値が変更されていて、開始日または周期(年・月・日)が変更されている場合
                    if (result.ScheduleDate != null && result.ScheduleDateBefore != result.ScheduleDate && cycleChangeFlg)
                    {
                        // エラー情報格納クラス
                        ErrorInfo errorInfo = new ErrorInfo(targetDic);
                        isError = true;
                        // 周期または開始日が変更されていいます。　同時に次回実施予定日は更新できません。
                        string errMsg = GetResMessage("141120019");
                        string val = info.getValName("schedule_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                        errorInfo.setError(errMsg, val);
                        errorInfoDictionary.Add(errorInfo.Result);
                    }
                }

                return isError;

                // 同一機器、部位、保全項目重複チェック
                bool checkContentDuplicate(Dao.managementStandardResult result)
                {
                    // 検索SQL文の取得
                    dynamic whereParam = null; // WHERE句パラメータ
                    string sql = string.Empty;
                    if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetManagementStandardCountCheck, out sql))
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return false;
                    }
                    whereParam = new { MachineId = result.MachineId, InspectionSiteStructureId = result.InspectionSiteStructureId, InspectionContentStructureId = result.InspectionContentStructureId, ManagementStandardsContentId = result.ManagementStandardsContentId };
                    // 総件数を取得
                    int cnt = db.GetCount(sql, whereParam);
                    if (cnt > 0)
                    {
                        return false;
                    }

                    return true;
                }

                // 複数の部位重要度、保全方式設定チェック
                bool checkInspectionMultiExist(Dao.managementStandardResult result)
                {
                    // 検索SQL文の取得
                    dynamic whereParam = null; // WHERE句パラメータ
                    string sql = string.Empty;
                    if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetInspectionMultiExistCheck, out sql))
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return false;
                    }
                    whereParam = new { MachineId = result.MachineId, InspectionSiteStructureId = result.InspectionSiteStructureId, InspectionSiteImportanceStructureId = result.InspectionSiteImportanceStructureId, InspectionSiteConservationStructureId = result.InspectionSiteConservationStructureId };
                    // 総件数を取得
                    int cnt = db.GetCount(sql, whereParam);
                    if (cnt >= 1)
                    {
                        return false;
                    }

                    return true;
                }

                // 開始日以降に保全活動が紐づいているスケジュールが存在する際はエラーとする
                bool checkScheduleAfterMsSummry(Dao.managementStandardResult result)
                {
                    // 検索SQL文の取得
                    dynamic whereParam = null; // WHERE句パラメータ
                    string sql = string.Empty;
                    if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetScheduleMsSummryCountAfterCheck, out sql))
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return false;
                    }
                    whereParam = new { MaintainanceScheduleId = result.MaintainanceScheduleId, StartDate = result.StartDate };
                    // 総件数を取得
                    int cnt = db.GetCount(sql, whereParam);
                    if (cnt > 0)
                    {
                        return false;
                    }

                    return true;
                }

            }

        }

        /// <summary>
        /// 点検種別毎登録時のチェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorRegistMaintainanceKindManage(Dao.managementStandardResult res, string ctrlId)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // チェック
            // 開始日以降に保全活動が紐づいているスケジュールが存在する際はエラーとする
            if (!checkMaintainanceKindManageScheduleAfterMsSummry(res))
            {
                // 単一の内容を取得
                Dictionary<string, object> targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(ctrlId);
                // エラー情報格納クラス
                ErrorInfo errorInfo = new ErrorInfo(targetDic);
                // 開始日には保全活動が登録されたスケジュール以降の日付を設定してください。
                string errMsg = GetResMessage("141060005");
                string val = info.getValName("start_date"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                errorInfo.setError(errMsg, val);
                errorInfoDictionary.Add(errorInfo.Result);
                SetJsonResult(errorInfoDictionary);
                // エラー情報を画面に反映
                return true;
            }

            return false;

            // 開始日以降に保全活動が紐づいているスケジュールが存在する際はエラーとする
            bool checkMaintainanceKindManageScheduleAfterMsSummry(Dao.managementStandardResult result)
            {
                // 検索SQL文の取得
                dynamic whereParam = null; // WHERE句パラメータ
                string sql = string.Empty;
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetScheduleMsSummryCountAfterMaintainanceKindManageCheck, out sql))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
                whereParam = new
                {
                    MachineId = result.MachineId,
                    MaintainanceKindStructureId = result.MaintainanceKindStructureId,
                    StartDate = result.StartDate
                };
                // 総件数を取得
                int cnt = db.GetCount(sql, whereParam);
                if (cnt > 0)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// 確認項目存在チェック
        /// </summary>
        /// <param name="res">入力データ</param>
        /// <returns>エラーの場合True</returns>
        private bool isConfirmRegistCycle(Dao.managementStandardResult res, bool maintainanceKindManage)
        {
            // 既に保全履歴の登録されているデータが存在する場合、確認メッセージ表示
            if (!checkMsSummryExist(res))
            {
                // 確認
                this.Status = CommonProcReturn.ProcStatus.Confirm;
                // 保全履歴が既に登録されてますが周期・開始日が変更されています。スケジュールを再作成しますがよろしいですか？
                this.MsgId = GetResMessage("141300005");
                this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
                return true;
            }

            // 点検種別毎管理の機器
            if (maintainanceKindManage)
            {
                if (!checkMaintainanceKindManageExist(res))
                {
                    // 既に同じ点検種別が対象機器の機器別管理基準内に登録されていて、周期と開始日が違う場合、確認メッセージを表示する。
                    // ※確認メッセージで「OK」だった際は、入力された周期と開始日で既に登録されている同じ点検種別のデータを更新する。（後勝ち登録)
                    // 確認
                    this.Status = CommonProcReturn.ProcStatus.Confirm;
                    // 同一点検種別で既に異なる周期・開始日が設定されています。入力された周期・内容でスケジュールを再作成しますがよろしいですか？
                    this.MsgId = GetResMessage("141200001");
                    this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
                    return true;
                }
            }

            // 保全履歴存在チェック
            bool checkMsSummryExist(Dao.managementStandardResult result)
            {
                // 検索SQL文の取得
                dynamic whereParam = null; // WHERE句パラメータ
                string sql = string.Empty;
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetScheduleMsSummryCountCheck, out sql))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
                whereParam = new { MaintainanceScheduleId = result.MaintainanceScheduleId };
                // 総件数を取得
                int cnt = db.GetCount(sql, whereParam);
                if (cnt > 0)
                {
                    return false;
                }

                return true;
            }

            // 同一点検種別存在チェック
            bool checkMaintainanceKindManageExist(Dao.managementStandardResult result)
            {
                // 検索SQL文の取得
                dynamic whereParam = null; // WHERE句パラメータ
                string sql = string.Empty;
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetMaintainanceKindManageExistCheck, out sql))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
                whereParam = new
                {
                    MachineId = result.MachineId,
                    ManagementStandardsContentId = result.ManagementStandardsContentId,
                    MaintainanceKindStructureId = result.MaintainanceKindStructureId,
                    CycleYear = result.CycleYear,
                    CycleMonth = result.CycleMonth,
                    CycleDay = result.CycleDay,
                    StartDate = result.StartDate
                };
                // 総件数を取得
                int cnt = db.GetCount(sql, whereParam);
                if (cnt > 0)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 新規登録時確認チェック(点検種別毎管理)
        /// </summary>
        /// <param name="result">入力データ</param>
        /// <returns>エラーの場合True</returns>
        private bool isConfirmRegistCycleInsert(Dao.managementStandardResult result)
        {
            if (!checkInsertMaintainanceKindManageExist(result))
            {
                // 既に同じ点検種別が対象機器の機器別管理基準内に登録されていて、周期と開始日が違う場合、確認メッセージを表示する。
                // ※確認メッセージで「OK」だった際は、入力された周期と開始日で既に登録されている同じ点検種別のデータを更新する。（後勝ち登録)
                // 確認
                this.Status = CommonProcReturn.ProcStatus.Confirm;
                // 同一点検種別で既に異なる周期・開始日が設定されています。入力された周期・内容でスケジュールを再作成しますがよろしいですか？
                this.MsgId = GetResMessage("141200001");
                this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
                return true;
            }

            return false;
        }

        // 同一点検種別存在チェック
        private bool checkInsertMaintainanceKindManageExist(Dao.managementStandardResult result)
        {
            // 検索SQL文の取得
            dynamic whereParam = null; // WHERE句パラメータ
            string sql = string.Empty;
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetMaintainanceKindManageInsertExistCheck, out sql))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }
            whereParam = new
            {
                MachineId = result.MachineId,
                MaintainanceKindStructureId = result.MaintainanceKindStructureId,
                CycleYear = result.CycleYear,
                CycleMonth = result.CycleMonth,
                CycleDay = result.CycleDay,
                StartDate = result.StartDate
            };
            // 総件数を取得
            int cnt = db.GetCount(sql, whereParam);
            if (cnt > 0)
            {
                return false;
            }

            return true;
        }

        // 同一点検種別スケジュール更新
        private bool updateMaintainanceKindManageOtherSchedule(Dao.managementStandardResult result, DateTime now)
        {
            // 最新DBデータ取得
            IList<Dao.managementStandardResult> oldResuts = null;
            dynamic whereParam = null; // WHERE句パラメータ
            whereParam = new
            {
                MachineId = result.MachineId,
                MaintainanceKindStructureId = result.MaintainanceKindStructureId,
                CycleYear = result.CycleYear,
                CycleMonth = result.CycleMonth,
                CycleDay = result.CycleDay,
                StartDate = result.StartDate
            };
            oldResuts = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetMaintainanceKindManageData, whereParam);

            foreach (Dao.managementStandardResult regRes in oldResuts)
            {
                var oldDate = regRes.StartDate;
                regRes.StartDate = result.StartDate;
                // 「作成済み保全スケジュール詳細データ(開始日以降)」削除
                if (!registDeleteDb<Dao.managementStandardResult>(regRes, SqlNameManagementStandard.DeleteMaintainanceScheduleDetailRemake))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }

                // 変更後開始日 < 変更前開始日であれば変更前開始日レコード削除
                if (result.StartDate <= oldDate)
                {
                    // 入力された日付以降の日付で設定されていた保全スケジュールデータは削除
                    if (!registDeleteDb<Dao.managementStandardResult>(regRes, SqlNameManagementStandard.DeleteMaintainanceScheduleRemake))
                    {
                        // エラーの場合
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return false;
                    }
                }

                // 周期ありフラグセット
                if ((result.CycleYear == null || result.CycleYear == 0) && (result.CycleMonth == null || result.CycleMonth == 0) && (result.CycleDay == null || result.CycleDay == 0))
                {
                    // 周期無し
                    regRes.IsCyclic = false;
                }
                else
                {
                    // 周期あり
                    regRes.IsCyclic = true;
                }
                regRes.CycleYear = result.CycleYear;
                regRes.CycleMonth = result.CycleMonth;
                regRes.CycleDay = result.CycleDay;
                regRes.DispCycle = result.DispCycle;
                regRes.StartDate = result.StartDate;
                // 新規登録者および新規登録日付をセット
                regRes.InsertUserId = int.Parse(this.UserId);
                regRes.InsertDatetime = now;
                // 更新者および更新日付をセット
                regRes.UpdateUserId = int.Parse(this.UserId);
                regRes.UpdateDatetime = now;

                // 保全スケジュールにデータ新規作成
                (bool returnFlag, long id) maintainanceSchedule = registInsertDb<Dao.managementStandardResult>(regRes, SqlNameManagementStandard.InsertMaintainanceSchedule);
                if (!maintainanceSchedule.returnFlag)
                {
                    return false;
                }

                // 保全スケジュール詳細にデータ新規作成
                if (!insertMainteScheduleDetail(regRes, maintainanceSchedule.id, now))
                {
                    return false;
                }
            }
            return true;
        }

        // 同一点検種別スケジュール更新
        private bool updateManagementContentKindManageOtherSchedule(Dao.managementStandardResult result, DateTime now)
        {
            // 最新DBデータ取得
            IList<Dao.managementStandardResult> oldResuts = null;
            dynamic whereParam = null; // WHERE句パラメータ
            whereParam = new
            {
                MachineId = result.MachineId,
                MaintainanceKindStructureId = result.MaintainanceKindStructureId,
                ManagementStandardsContentId = result.ManagementStandardsContentId
            };
            oldResuts = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetMaintainanceKindManageDataUpdContent, whereParam);

            foreach (Dao.managementStandardResult regRes in oldResuts)
            {
                // スケジュール基準を更新
                // 機器別管理基準内容
                regRes.ScheduleTypeStructureId = result.ScheduleTypeStructureId;
                // 更新者および更新日付をセット
                regRes.UpdateUserId = int.Parse(this.UserId);
                regRes.UpdateDatetime = now;
                if (!registUpdateDb<Dao.managementStandardResult>(regRes, SqlNameManagementStandard.UpdateManagementStandardsContentSchedule))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 保全スケジュール詳細データ新規登録
        /// </summary>
        /// <param name="res">登録データ</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool insertMainteScheduleDetail(Dao.managementStandardResult res, long maintainanceScheduleId, DateTime updateTime)
        {

            if (res.IsCyclic == true)
            {
                // スケジュール作成上限取得
                string strMaxYear = getItemExData(1, 2, (int)TMQConsts.MsStructure.GroupId.MakeScheduleYear, 0);

                // 対象機器の工場ID取得
                dynamic whereParam = new { MachineId = res.MachineId };
                string sql;
                TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
                IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
                if (results == null || results.Count == 0)
                {
                    return false;
                }
                List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
                typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
                typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, typeLst, this.db, this.LanguageId, true);

                // 工場毎年度終了月取得
                string strstartMonth = getItemExData(1, 2, (int)TMQConsts.MsStructure.GroupId.BeginningMonth, results[0].FactoryId);
                if (strstartMonth == null)
                {
                    // 工場毎の設定が取得できなければ工場共通
                    strstartMonth = getItemExData(1, 2, (int)TMQConsts.MsStructure.GroupId.BeginningMonth, 0);
                }
                if (int.Parse(strstartMonth) < 10)
                {
                    strstartMonth = "0" + strstartMonth;
                }
                //string strEndMonth = getItemExData(2, 2, (int)TMQConsts.MsStructure.GroupId.BeginningMonth, 6);
                //if (strEndMonth == null)
                //{
                //    // 工場毎の設定が取得できなければ工場共通
                //    strEndMonth = getItemExData(2, 2, (int)TMQConsts.MsStructure.GroupId.BeginningMonth, 0);
                //}

                // スケジュール作成末日を設定
                //int intEndYear = int.Parse(strMaxYear);
                //if (int.Parse(strstartMonth) > int.Parse(strEndMonth))
                //{
                //    // 開始月が終了月より大きい場合は次年度
                //    intEndYear = intEndYear + 1;
                //}
                //int intEndMonth = int.Parse(strEndMonth);
                //string endMonth = intEndMonth.ToString();
                //if (intEndMonth < 10)
                //{
                //    endMonth = "0" + intEndMonth.ToString();
                //}
                //int this_month_days = DateTime.DaysInMonth(intEndYear, intEndMonth);
                //string endDay = this_month_days.ToString();
                //if (this_month_days < 10)
                //{
                //    endDay = "0" + this_month_days.ToString();
                //}

                DateTime endDate = DateTime.Parse(strMaxYear + "/" + strstartMonth + "/01");
                endDate = endDate.AddYears(1).AddDays(-1);

                //DateTime endDate = DateTime.Parse(intEndYear.ToString() + "/" + endMonth + "/" + endDay);
                DateTime dt = (DateTime)res.StartDate;

                // 保全スケジュール詳細データ
                ComDao.McMaintainanceScheduleDetailEntity scheduleDetail = new();
                scheduleDetail.InsertDatetime = updateTime;
                scheduleDetail.InsertUserId = int.Parse(this.UserId);
                scheduleDetail.UpdateDatetime = updateTime;
                scheduleDetail.UpdateUserId = int.Parse(this.UserId);
                scheduleDetail.SummaryId = null;
                scheduleDetail.Complition = false;
                scheduleDetail.MaintainanceScheduleId = maintainanceScheduleId;
                scheduleDetail.SequenceCount = 1;
                scheduleDetail.ScheduleDate = dt;
                (bool returnFlag, long id) maintainanceSchedule = (true, -1);
                // 開始日から終了日までスケジュール詳細作成
                while (dt <= endDate.Date)
                {
                    // 登録
                    maintainanceSchedule = registInsertDb<ComDao.McMaintainanceScheduleDetailEntity>(scheduleDetail, SqlNameManagementStandard.InsertMaintainanceScheduleDetail);
                    if (!maintainanceSchedule.returnFlag)
                    {
                        return false;
                    }

                    // 回数カウントアップ
                    scheduleDetail.SequenceCount = scheduleDetail.SequenceCount + 1;

                    // スケジュール日設定
                    if (res.CycleYear != null && res.CycleYear > 0)
                    {
                        dt = dt.AddYears((int)res.CycleYear);
                    }
                    if (res.CycleMonth != null && res.CycleMonth > 0)
                    {
                        dt = dt.AddMonths((int)res.CycleMonth);
                    }
                    if (res.CycleDay != null && res.CycleDay > 0)
                    {
                        dt = dt.AddDays((int)res.CycleDay);
                    }
                    scheduleDetail.ScheduleDate = dt;
                }
            }
            else
            {
                // 保全スケジュール詳細データ
                ComDao.McMaintainanceScheduleDetailEntity scheduleDetail = new();
                scheduleDetail.InsertDatetime = updateTime;
                scheduleDetail.InsertUserId = int.Parse(this.UserId);
                scheduleDetail.UpdateDatetime = updateTime;
                scheduleDetail.UpdateUserId = int.Parse(this.UserId);
                scheduleDetail.SummaryId = null;
                scheduleDetail.Complition = false;
                scheduleDetail.MaintainanceScheduleId = maintainanceScheduleId;
                scheduleDetail.SequenceCount = 1;
                scheduleDetail.ScheduleDate = res.StartDate;

                // 登録
                (bool returnFlag, long id) maintainanceSchedule = registInsertDb<ComDao.McMaintainanceScheduleDetailEntity>(scheduleDetail, SqlNameManagementStandard.InsertMaintainanceScheduleDetail);
                if (!maintainanceSchedule.returnFlag)
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// 同一点検種別の周期等更新
        /// </summary>
        /// <param name="result">登録データ</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool updateMaintainanceKindManage(Dao.managementStandardResult result, DateTime now)
        {
            // 検索条件取得
            dynamic whereParam = null; // WHERE句パラメータ
            string sql;
            // 一覧検索SQL文の取得
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
            whereParam = new { MachineId = result.MachineId };

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 点検種別毎管理
            if (results[0].MaintainanceKindManage)
            {
                // 同一点検種別スケジュール更新処理
                if (!updateMaintainanceKindManageOtherSchedule(result, now))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 同一点検種別のスケジュール基準更新
        /// </summary>
        /// <param name="result">登録データ</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool updateManagementContentKindManage(Dao.managementStandardResult result, DateTime now)
        {
            // 検索条件取得
            dynamic whereParam = null; // WHERE句パラメータ
            string sql;
            // 一覧検索SQL文の取得
            TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
            whereParam = new { MachineId = result.MachineId };

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 点検種別毎管理
            if (results[0].MaintainanceKindManage)
            {
                // 同一点検種別スケジュール更新処理
                if (!updateManagementContentKindManageOtherSchedule(result, now))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 保全項目一覧 削除処理
        /// </summary>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool deleteManagementStandardSingle()
        {
            // 削除リスト取得
            var deleteList = getSelectedRowsByList(this.resultInfoDictionary, TargetCtrlIdManagementStandard.DetailManagementStandard90);

            // 排他・長期計画、保全活動存在チェック
            foreach (var deleteRow in deleteList)
            {
                // 入力された内容を取得
                Dao.managementStandardResult deleteResult = getRegistManagementStandard<Dao.managementStandardResult>(deleteRow, TargetCtrlIdManagementStandard.DetailManagementStandard90);
                IList<Dao.managementStandardResult> dbresult = null;
                // 最新DBデータ取得
                dynamic whereParam = whereParam = new { MachineId = deleteResult.MachineId, ManagementStandardsComponentId = deleteResult.ManagementStandardsComponentId, ManagementStandardsContentId = deleteResult.ManagementStandardsContentId };
                dbresult = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);

                // 排他チェック
                if (dbresult == null || dbresult.Count == 0 || isErrorExclusiveManagementStandard(deleteResult, dbresult))
                {
                    return false;
                }

                // 長期計画存在チェック
                whereParam = null; // WHERE句パラメータ
                string sql = string.Empty;
                // 検索SQL文の取得
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetLongPlanSingle, out sql))
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
                whereParam = whereParam = new { ManagementStandardsComponentId = deleteResult.ManagementStandardsComponentId };
                // 総件数を取得
                int cnt = db.GetCount(sql, whereParam);
                if (cnt > 0)
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「 長期計画で使用されている為、削除できません。」
                    this.MsgId = GetResMessage("141170002");
                    return false;
                }

                // 保全活動存在チェック
                whereParam = null; // WHERE句パラメータ
                sql = string.Empty;
                // 検索SQL文の取得
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetMsSummarySingle, out sql))
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
                whereParam = whereParam = new { ManagementStandardsComponentId = deleteResult.ManagementStandardsComponentId };
                // 総件数を取得
                cnt = db.GetCount(sql, whereParam);
                if (cnt > 0)
                {
                    // エラー終了
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    // 「 保全活動が作成されている為、削除できません。」
                    this.MsgId = GetResMessage("141300004");
                    return false;
                }

                // 添付ファイル 排他チェック
                DateTime? maxDateOfList = deleteResult.MaxUpdateDatetime != null ? deleteResult.MaxUpdateDatetime : null;
                if (!CheckExclusiveStatusByUpdateDatetime(maxDateOfList, getMaxDateByContentId(deleteResult.ManagementStandardsContentId)))
                {
                    return false;
                }
            }

            // 削除SQL取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.DeleteMaintainanceScheduleDetailSingle, out string sql1))
            {
                return false;
            }
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.DeleteMaintainanceScheduleSingle, out string sql2))
            {
                return false;
            }
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.DeleteManagementStandardsContentSingle, out string sql3))
            {
                return false;
            }
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.DeleteManagementStandardsComponentSingle, out string sql4))
            {
                return false;
            }
            // 添付ファイル存在チェックSQL取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetAttachmentCount, out string sql5))
            {
                return false;
            }

            // 行削除
            foreach (var deleteRow in deleteList)
            {
                Dao.managementStandardResult delCondition = new();
                SetDeleteConditionByDataClass(deleteRow, TargetCtrlIdManagementStandard.DetailManagementStandard90, delCondition);

                // 保全スケジュール詳細
                int result = this.db.Regist(sql1, delCondition);
                if (result < 0)
                {
                    // 削除エラー
                    return false;
                }
                // 保全スケジュール
                result = this.db.Regist(sql2, delCondition);
                if (result < 0)
                {
                    // 削除エラー
                    return false;
                }
                // 機器別管理基準内容
                result = this.db.Regist(sql3, delCondition);
                if (result < 0)
                {
                    // 削除エラー
                    return false;
                }
                // 機器別管理基準部位
                result = this.db.Regist(sql4, delCondition);
                if (result < 0)
                {
                    // 削除エラー
                    return false;
                }

                // 指定された機能タイプID、キーIDのデータが存在しない場合は何もしない
                dynamic param = null; // WHERE句パラメータ
                param = new { FunctionTypeId = (int)TMQConsts.Attachment.FunctionTypeId.Content, KeyId = delCondition.ManagementStandardsContentId };
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameMpInfo.GetAttachmentCount, out string cntSql);
                if (this.db.GetCount(cntSql, param) <= 0)
                {
                    continue;
                }

                // 添付情報削除
                if (!new ComDao.AttachmentEntity().DeleteByKeyId(TMQConsts.Attachment.FunctionTypeId.Content, (int)delCondition.ManagementStandardsContentId, this.db))
                {
                    // 削除エラー
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// アイテム拡張マスタから拡張データを取得する
        /// </summary>
        /// <param name="seq">連番</param>
        /// <param name="dataType">データタイプ</param>
        /// <param name="factoryId">工場ID</param>
        /// <returns>拡張データ</returns>
        private string getItemExData(short seq, short dataType, int? structureGroupId, int? factoryId)
        {
            string val = null;

            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = structureGroupId;
            //連番
            param.Seq = seq;
            //構成アイテム、アイテム拡張マスタ情報取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            if (list != null)
            {
                //取得情報から拡張データを取得
                var result = list.Exists(x => x.FactoryId == factoryId);
                if (result)
                {
                    val = list.Where(x => x.FactoryId == factoryId).Select(x => x.ExData).First();
                }
            }
            return val;
        }

        /// <summary>
        /// DBデータ取得
        /// </summary>
        /// <typeparam name="T">取得データクラス</typeparam>
        /// <param name="whereParam">パラメータ</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private IList<T> getDbData<T>(string sqlFile, dynamic whereParam)
        {
            string sql;
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlFile, out sql);
            IList<T> results = db.GetListByDataClass<T>(sql, whereParam);
            return results;
        }

        /// <summary>
        /// 添付ファイルの最大更新日時を取得
        /// </summary>
        /// <param name="id">キーID</param>
        /// <returns>最大更新日時</returns>
        private DateTime? getMaxDateByContentId(long id)
        {
            // 最大更新日時取得SQL
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetMaxDateByKeyId, out string outSql);
            Dao.managementStandardResult getMaxDateParam = new Dao.managementStandardResult();
            getMaxDateParam.KeyId = (int)id;
            getMaxDateParam.FunctionTypeId = (int)TMQConsts.Attachment.FunctionTypeId.Content;
            // SQL実行
            var maxDateResult = db.GetEntity(outSql, getMaxDateParam);

            return maxDateResult.max_update_datetime;
        }

        ///// <summary>
        ///// ExcelPort機器別管理基準チェック処理
        ///// </summary>
        ///// <returns>true:正常終了</returns>
        //private bool checkExcelPortManagementStandardRegist(ref List<BusinessLogicDataClass_MC0001.excelPortManagementStandardResult> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList)
        //{
        //    // 入力チェック
        //    bool errFlg = false;
        //    for (int i = 0; i < resultList.Count; i++)
        //    {
        //        // 送信時処理IDが設定されているもののみ
        //        if (resultList[i].ProcessId != null)
        //        {

        //            // 新規
        //            if (resultList[i].ProcessId == 1)
        //            {
        //                // 同一機器、部位、保全項目重複チェック
        //                if (!checkEpContentDuplicate(resultList[i]))
        //                {
        //                    // 入力された部位、保全項目の組み合わせは既に登録されています。
        //                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.ProccesColumnNo, null, GetResMessage("141220002"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
        //                    errFlg = true;
        //                }

        //            }
        //            // 更新
        //            else if (resultList[i].ProcessId == 2)
        //            {
        //                // 同一機器、部位、保全項目重複チェック
        //                if (!checkEpContentDuplicate(resultList[i]))
        //                {
        //                    // 入力された部位、保全項目の組み合わせは既に登録されています。
        //                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.ProccesColumnNo, null, GetResMessage("141220002"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
        //                    errFlg = true;
        //                }

        //                // 最新DBデータ取得
        //                IList<Dao.managementStandardResult> beforeResult = null;
        //                dynamic whereParam = whereParam = new { ManagementStandardsComponentId = resultList[i].ManagementStandardsComponentId, ManagementStandardsContentId = resultList[i].ManagementStandardsContentId };
        //                beforeResult = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);

        //                if (beforeResult != null && beforeResult.Count > 0)
        //                {
        //                    // 周期または開始日が変更されている
        //                    if (resultList[i].CycleYear != beforeResult[0].CycleYear || resultList[i].CycleMonth != beforeResult[0].CycleMonth || resultList[i].CycleDay != beforeResult[0].CycleDay || resultList[i].StartDate != beforeResult[0].StartDate)
        //                    {
        //                        // 開始日が過去日だった際はエラー
        //                        if (resultList[i].StartDate < DateTime.Now.Date)
        //                        {
        //                            // 過去日付は設定できません。
        //                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.StratDateColumnNo, null, GetResMessage("141060003"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
        //                            errFlg = true;
        //                        }

        //                        // 開始日以降に保全活動が紐づいているスケジュールが存在する際はエラーとする
        //                        if (!checkEpScheduleAfterMsSummry(resultList[i]))
        //                        {
        //                            //  開始日には保全活動が登録されたスケジュール以降の日付を設定してください。
        //                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.StratDateColumnNo, null, GetResMessage("141060005"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
        //                            errFlg = true;
        //                        }
        //                    }
        //                }

        //            }
        //            // 削除
        //            else if (resultList[i].ProcessId == 9)
        //            {
        //                // 長期計画存在チェック
        //                dynamic whereParam = null; // WHERE句パラメータ
        //                string sql = string.Empty;
        //                // 検索SQL文の取得
        //                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetLongPlanSingle, out sql))
        //                {
        //                    // エラー終了
        //                    this.Status = CommonProcReturn.ProcStatus.Error;
        //                    return false;
        //                }
        //                whereParam = whereParam = new { ManagementStandardsComponentId = resultList[i].ManagementStandardsComponentId };
        //                // 総件数を取得
        //                int cnt = db.GetCount(sql, whereParam);
        //                if (cnt > 0)
        //                {
        //                    // 「 長期計画で使用されている為、削除できません。」
        //                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.ProccesColumnNo, null, GetResMessage("141170002"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
        //                    errFlg = true;
        //                }

        //                // 保全活動存在チェック
        //                whereParam = null; // WHERE句パラメータ
        //                sql = string.Empty;
        //                // 検索SQL文の取得
        //                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetMsSummarySingle, out sql))
        //                {
        //                    // エラー終了
        //                    this.Status = CommonProcReturn.ProcStatus.Error;
        //                    return false;
        //                }
        //                whereParam = whereParam = new { ManagementStandardsComponentId = resultList[i].ManagementStandardsComponentId };
        //                // 総件数を取得
        //                cnt = db.GetCount(sql, whereParam);
        //                if (cnt > 0)
        //                {
        //                    // 「 保全活動が作成されている為、削除できません。」
        //                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.ProccesColumnNo, null, GetResMessage("141300004"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
        //                    errFlg = true;
        //                }
        //            }
        //        }
        //    }

        //    // 全件問題無ければ登録処理
        //    if (errFlg)
        //    {
        //        return false;
        //    }

        //    return true;

        //    // 同一機器、部位、保全項目重複チェック
        //    bool checkEpContentDuplicate(Dao.excelPortManagementStandardResult result)
        //    {
        //        // 検索SQL文の取得
        //        dynamic whereParam = null; // WHERE句パラメータ
        //        string sql = string.Empty;
        //        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetManagementStandardCountCheck, out sql))
        //        {
        //            this.Status = CommonProcReturn.ProcStatus.Error;
        //            return false;
        //        }
        //        whereParam = new { MachineId = result.MachineId, InspectionSiteStructureId = result.InspectionSiteStructureId, InspectionContentStructureId = result.InspectionContentStructureId, ManagementStandardsContentId = result.ManagementStandardsContentId };
        //        // 総件数を取得
        //        int cnt = db.GetCount(sql, whereParam);
        //        if (cnt > 0)
        //        {
        //            return false;
        //        }

        //        return true;
        //    }

        //    // 開始日以降に保全活動が紐づいているスケジュールが存在する際はエラーとする
        //    bool checkEpScheduleAfterMsSummry(Dao.excelPortManagementStandardResult result)
        //    {
        //        // 検索SQL文の取得
        //        dynamic whereParam = null; // WHERE句パラメータ
        //        string sql = string.Empty;
        //        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetScheduleMsSummryCountAfterCheck, out sql))
        //        {
        //            this.Status = CommonProcReturn.ProcStatus.Error;
        //            return false;
        //        }
        //        whereParam = new { MaintainanceScheduleId = result.MaintainanceScheduleId, StartDate = result.StartDate };
        //        // 総件数を取得
        //        int cnt = db.GetCount(sql, whereParam);
        //        if (cnt > 0)
        //        {
        //            return false;
        //        }

        //        return true;
        //    }
        //}

        /// <summary>
        /// ExcelPort機器別管理基準登録処理
        /// </summary>
        /// <returns>true:正常終了</returns>
        private bool executeExcelPortManagementStandardRegist(List<BusinessLogicDataClass_MC0001.excelPortManagementStandardResult> resultList, ref List<ComDao.UploadErrorInfo> errorInfoList)
        {
            // 全体エラー存在フラグ
            bool errFlg = false;
            // 行単位エラー存在フラグ
            bool rowErrFlg = false;

            // 登録処理
            for (int i = 0; i < resultList.Count; i++)
            {
                rowErrFlg = false;

                // 送信時処理IDが設定されているもののみ
                if (resultList[i].ProcessId != null)
                {
                    // 機器別管理基準フラグセット
                    resultList[i].IsManagementStandardConponent = true;
                    DateTime now = DateTime.Now;
                    // 周期ありフラグセット
                    if ((resultList[i].CycleYear == null || resultList[i].CycleYear == 0) && (resultList[i].CycleMonth == null || resultList[i].CycleMonth == 0) && (resultList[i].CycleDay == null || resultList[i].CycleDay == 0))
                    {
                        // 周期無し
                        resultList[i].IsCyclic = false;
                    }
                    else
                    {
                        // 周期あり
                        resultList[i].IsCyclic = true;
                    }

                    // 過去日のチェックは行わない
                    //// 次回実施予定日に過去日が入力されている場合
                    //if (resultList[i].ScheduleDate != null && resultList[i].ScheduleDate < DateTime.Now)
                    //{
                    //    // 過去日付は設定できません。
                    //    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.NextScheduleDate, null, GetResMessage("141060003"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                    //    errFlg = true;
                    //    rowErrFlg = true;
                    //}

                    // 次回実施予定日が入力されている場合
                    if (resultList[i].ScheduleDate != null)
                    {
                        // 次々回実施予定日を取得
                        List<Dao.managementStandardResult> nextDate = (List<Dao.managementStandardResult>)getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetNextScheduleDate, new { ManagementStandardsContentId = resultList[i].ManagementStandardsContentId, ScheduleDateBefore = resultList[i].ScheduleDateBefore, MaintainanceScheduleId = resultList[i].MaintainanceScheduleId });

                        // 取得できた場合は範囲チェックをする
                        if (nextDate != null && nextDate.Count > 0)
                        {
                            bool isDateErr = false;

                            // 入力された次回実施予定日が次々回実施予定日以降の場合
                            if (resultList[i].ScheduleDate >= nextDate[0].ScheduleDate)
                            {
                                isDateErr = true;
                            }

                            // 前回実施予定日は存在しない可能性があることを考慮する
                            if (nextDate.Count > 1)
                            {
                                // 入力された次回実施予定日が前回実施予定日の場合以前
                                if (resultList[i].ScheduleDate <= nextDate[1].ScheduleDate)
                                {
                                    isDateErr = true;
                                }
                            }
                            else
                            {
                                // 前回実施予定日が存在しない場合は開始日と比較する
                                if (resultList[i].ScheduleDate < resultList[i].StartDate)
                                {
                                    isDateErr = true;
                                }
                            }

                            // 日付の範囲エラーの場合はメッセージをセットする
                            if (isDateErr)
                            {
                                // 次回実施予定日は前回実施予定日～次々回実施予定日以内の日付を指定してください。
                                errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.NextScheduleDate, null, GetResMessage("141120020"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                                errFlg = true;
                                rowErrFlg = true;
                            }
                        }
                    }

                    // 新規登録
                    if (resultList[i].ProcessId == 1)
                    {
                        // 同一機器、部位、保全項目重複チェック
                        if (!checkEpContentDuplicate(resultList[i]))
                        {
                            // 入力された部位、保全項目の組み合わせは既に登録されています。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.ProccesColumnNo, null, GetResMessage("141220002"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                            errFlg = true;
                            rowErrFlg = true;
                        }

                        // 新規登録かつ、「スケジュールを更新」が未チェックの場合
                        if (resultList[i].IsUpdateSchedule <= 0)
                        {
                            // 新規登録情報の為　「スケジュールを更新」に〇を設定してください。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.IsUpdateSchedule, null, GetResMessage("141120023"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                            errFlg = true;
                            rowErrFlg = true;
                        }

                        // エラー有りなら次へ
                        if (rowErrFlg)
                        {
                            continue;
                        }

                        resultList[i].InsertDatetime = now;
                        resultList[i].InsertUserId = int.Parse(this.UserId);
                        resultList[i].UpdateDatetime = now;
                        resultList[i].UpdateUserId = int.Parse(this.UserId);

                        // 新規登録
                        // 機器別管理基準部位
                        (bool returnFlag, long id) managementStandardsComponent = registInsertDb<Dao.excelPortManagementStandardResult>(resultList[i], SqlNameManagementStandard.InsertManagementStandardsComponent);
                        if (!managementStandardsComponent.returnFlag)
                        {
                            return false;
                        }
                        // 機器別管理基準内容
                        resultList[i].ManagementStandardsComponentId = managementStandardsComponent.id;
                        (bool returnFlag, long id) managementStandardsContent = registInsertDb<Dao.excelPortManagementStandardResult>(resultList[i], SqlNameManagementStandard.InsertManagementStandardsContent);
                        if (!managementStandardsContent.returnFlag)
                        {
                            return false;
                        }
                        // 保全スケジュール
                        resultList[i].ManagementStandardsContentId = managementStandardsContent.id;
                        (bool returnFlag, long id) maintainanceSchedule = registInsertDb<Dao.excelPortManagementStandardResult>(resultList[i], SqlNameManagementStandard.InsertMaintainanceSchedule);
                        if (!maintainanceSchedule.returnFlag)
                        {
                            return false;
                        }
                        // 保全スケジュール詳細
                        if (!insertExcelPortMainteScheduleDetail(resultList[i], maintainanceSchedule.id, now))
                        {
                            return false;
                        }

                    }
                    // 更新
                    else if (resultList[i].ProcessId == 2)
                    {
                        // 同一機器、部位、保全項目重複チェック
                        if (!checkEpContentDuplicate(resultList[i]))
                        {
                            // 入力された部位、保全項目の組み合わせは既に登録されています。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.ProccesColumnNo, null, GetResMessage("141220002"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                            errFlg = true;
                            rowErrFlg = true;
                        }

                        // 最新DBデータ取得
                        IList<Dao.managementStandardResult> beforeResult = null;
                        dynamic whereParam = whereParam = new { ManagementStandardsComponentId = resultList[i].ManagementStandardsComponentId, ManagementStandardsContentId = resultList[i].ManagementStandardsContentId };
                        beforeResult = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);

                        if (beforeResult != null && beforeResult.Count > 0)
                        {
                            // 周期または開始日が変更されている
                            if (resultList[i].CycleYear != beforeResult[0].CycleYear || resultList[i].CycleMonth != beforeResult[0].CycleMonth || resultList[i].CycleDay != beforeResult[0].CycleDay || resultList[i].StartDate != beforeResult[0].StartDate)
                            {
                                // 開始日が過去日だった際はエラー
                                if (resultList[i].StartDate < DateTime.Now.Date)
                                {
                                    // 過去日付は設定できません。
                                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.StratDateColumnNo, null, GetResMessage("141060003"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                                    errFlg = true;
                                    rowErrFlg = true;
                                }

                                // 開始日以降に保全活動が紐づいているスケジュールが存在する際はエラーとする
                                if (!checkEpScheduleAfterMsSummry(resultList[i]))
                                {
                                    //  開始日には保全活動が登録されたスケジュール以降の日付を設定してください。
                                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.StratDateColumnNo, null, GetResMessage("141060005"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                                    errFlg = true;
                                    rowErrFlg = true;
                                }

                                // 次回実施予定日が入力されているかつ値が変更されている場合
                                if (resultList[i].ScheduleDate != null && resultList[i].ScheduleDate != resultList[i].ScheduleDateBefore)
                                {
                                    // 周期または開始日が変更されていいます。　同時に次回実施予定日は更新できません。
                                    errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.NextScheduleDate, null, GetResMessage("141120019"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                                    errFlg = true;
                                    rowErrFlg = true;
                                }
                            }
                        }

                        //「スケジュールを更新」が選択かつ、次回実施予定日が設定されている場合
                        if (resultList[i].IsUpdateSchedule > 0 && resultList[i].ScheduleDate != null)

                        {
                            // 次回実施予定日を更新する場合は「スケジュールを更新」のチェックを外してください。
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.IsUpdateSchedule, null, GetResMessage("141120022"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                            errFlg = true;
                            rowErrFlg = true;
                        }

                        // エラー有りなら次へ
                        if (rowErrFlg)
                        {
                            continue;
                        }

                        resultList[i].UpdateDatetime = now;
                        resultList[i].UpdateUserId = int.Parse(this.UserId);

                        // 更新
                        // 機器別管理基準部位
                        if (!registUpdateDb<Dao.excelPortManagementStandardResult>(resultList[i], SqlNameManagementStandard.UpdateManagementStandardsComponent))
                        {
                            return false;
                        }
                        // 機器別管理基準内容
                        if (!registUpdateDb<Dao.excelPortManagementStandardResult>(resultList[i], SqlNameManagementStandard.UpdateManagementStandardsContent))
                        {
                            return false;
                        }

                        // 表示周期のみを更新する
                        if (!registUpdateDb<Dao.excelPortManagementStandardResult>(resultList[i], SqlNameManagementStandard.UpdateDispCycleOfMaintainanceSchedule))
                        {
                            return false;
                        }

                        // 次回実施予定日が入力されていて値が変更されている場合
                        if (resultList[i].ScheduleDate != null && resultList[i].ScheduleDate != resultList[i].ScheduleDateBefore)
                        {
                            // スケジュール日を入力された値に更新する
                            if (!registUpdateDb<Dao.excelPortManagementStandardResult>(resultList[i], SqlNameManagementStandard.UpdateScheduleDateByDetailId))
                            {
                                return false;
                            }
                        }

                        // スケジュールを更新 が未選択または、次回実施予定日が入力されて変更されている場合
                        if (resultList[i].IsUpdateSchedule.ToString() == ComConsts.CHECK_FLG.OFF ||
                            (resultList[i].ScheduleDate != null && resultList[i].ScheduleDate != resultList[i].ScheduleDateBefore))
                        {
                            // 保全スケジュールの再作成処理は行わないのでここで終了
                            continue;
                        }

                        IList<Dao.managementStandardResult> dbresult = null;
                        // 最新DBデータ取得
                        whereParam = whereParam = new { ManagementStandardsComponentId = resultList[i].ManagementStandardsComponentId, ManagementStandardsContentId = resultList[i].ManagementStandardsContentId };
                        dbresult = getDbData<Dao.managementStandardResult>(SqlNameManagementStandard.GetManagementStandardDetail, whereParam);

                        bool cycleChangeFlg = false;
                        if (resultList[i].CycleYear == dbresult[0].CycleYear && resultList[i].CycleMonth == dbresult[0].CycleMonth && resultList[i].CycleDay == dbresult[0].CycleDay && resultList[i].StartDate == dbresult[0].StartDate)
                        {
                            cycleChangeFlg = false;
                        }
                        else
                        {
                            cycleChangeFlg = true;
                        }

                        // 周期・開始日変更有
                        if (cycleChangeFlg)
                        {
                            // 「作成済み保全スケジュール詳細データ(開始日以降)」削除
                            if (!registDeleteDb<Dao.excelPortManagementStandardResult>(resultList[i], SqlNameManagementStandard.DeleteMaintainanceScheduleDetailRemake))
                            {
                                // エラーの場合
                                this.Status = CommonProcReturn.ProcStatus.Error;
                                return false;
                            }

                            // 変更後開始日 < 変更前開始日であれば変更前開始日レコード削除
                            if (resultList[i].StartDate <= dbresult[0].StartDate)
                            {
                                // 入力された日付以降の日付で設定されていた保全スケジュールデータは削除
                                if (!registDeleteDb<Dao.excelPortManagementStandardResult>(resultList[i], SqlNameManagementStandard.DeleteMaintainanceScheduleRemake))
                                {
                                    // エラーの場合
                                    this.Status = CommonProcReturn.ProcStatus.Error;
                                    return false;
                                }
                            }

                            // 新規登録者および新規登録日付をセット
                            resultList[i].InsertUserId = int.Parse(this.UserId);
                            resultList[i].InsertDatetime = now;

                            // 保全スケジュールにデータ新規作成
                            (bool returnFlag, long id) maintainanceSchedule = registInsertDb<Dao.excelPortManagementStandardResult>(resultList[i], SqlNameManagementStandard.InsertMaintainanceSchedule);
                            if (!maintainanceSchedule.returnFlag)
                            {
                                return false;
                            }

                            // 保全スケジュール詳細にデータ新規作成
                            if (!insertExcelPortMainteScheduleDetail(resultList[i], maintainanceSchedule.id, now))
                            {
                                return false;
                            }

                            resultList[i].MaintainanceScheduleId = maintainanceSchedule.id;
                        }

                    }
                    // 削除
                    else if (resultList[i].ProcessId == 9)
                    {
                        // 長期計画存在チェック
                        dynamic whereParam = null; // WHERE句パラメータ
                        string sql = string.Empty;
                        // 検索SQL文の取得
                        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetLongPlanSingle, out sql))
                        {
                            // エラー終了
                            this.Status = CommonProcReturn.ProcStatus.Error;
                            return false;
                        }
                        whereParam = whereParam = new { ManagementStandardsComponentId = resultList[i].ManagementStandardsComponentId };
                        // 総件数を取得
                        int cnt = db.GetCount(sql, whereParam);
                        if (cnt > 0)
                        {
                            // 「 長期計画で使用されている為、削除できません。」
                            errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.ProccesColumnNo, null, GetResMessage("141170002"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                            errFlg = true;
                            rowErrFlg = true;
                        }
                        else
                        {
                            // 保全活動存在チェック
                            whereParam = null; // WHERE句パラメータ
                            sql = string.Empty;
                            // 検索SQL文の取得
                            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetMsSummarySingle, out sql))
                            {
                                // エラー終了
                                this.Status = CommonProcReturn.ProcStatus.Error;
                                return false;
                            }
                            whereParam = whereParam = new { ManagementStandardsComponentId = resultList[i].ManagementStandardsComponentId };
                            // 総件数を取得
                            cnt = db.GetCount(sql, whereParam);
                            if (cnt > 0)
                            {
                                // 「 保全活動が作成されている為、削除できません。」
                                errorInfoList.Add(TMQUtil.setTmpErrorInfo((int)resultList[i].RowNo, ExcelPortManagementStandardListInfo.ProccesColumnNo, null, GetResMessage("141300004"), TMQUtil.ComReport.LongitudinalDirection, resultList[i].ProcessId.ToString()));
                                errFlg = true;
                                rowErrFlg = true;
                            }
                        }

                        // エラー有りなら次へ
                        if (rowErrFlg)
                        {
                            continue;
                        }

                        // 削除処理
                        // 削除SQL取得
                        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.DeleteMaintainanceScheduleDetailSingle, out string sql1))
                        {
                            return false;
                        }
                        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.DeleteMaintainanceScheduleSingle, out string sql2))
                        {
                            return false;
                        }
                        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.DeleteManagementStandardsContentSingle, out string sql3))
                        {
                            return false;
                        }
                        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.DeleteManagementStandardsComponentSingle, out string sql4))
                        {
                            return false;
                        }
                        // 添付ファイル存在チェックSQL取得
                        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetAttachmentCount, out string sql5))
                        {
                            return false;
                        }

                        // 保全スケジュール詳細
                        int result = this.db.Regist(sql1, resultList[i]);
                        if (result < 0)
                        {
                            // 削除エラー
                            return false;
                        }
                        // 保全スケジュール
                        result = this.db.Regist(sql2, resultList[i]);
                        if (result < 0)
                        {
                            // 削除エラー
                            return false;
                        }
                        // 機器別管理基準内容
                        result = this.db.Regist(sql3, resultList[i]);
                        if (result < 0)
                        {
                            // 削除エラー
                            return false;
                        }
                        // 機器別管理基準部位
                        result = this.db.Regist(sql4, resultList[i]);
                        if (result < 0)
                        {
                            // 削除エラー
                            return false;
                        }

                        // 指定された機能タイプID、キーIDのデータが存在しない場合は何もしない
                        dynamic param = null; // WHERE句パラメータ
                        param = new { FunctionTypeId = (int)TMQConsts.Attachment.FunctionTypeId.Content, KeyId = resultList[i].ManagementStandardsContentId };
                        // SQLを取得
                        TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameMpInfo.GetAttachmentCount, out string cntSql);
                        if (this.db.GetCount(cntSql, param) > 0)
                        {
                            // 添付情報削除
                            if (!new ComDao.AttachmentEntity().DeleteByKeyId(TMQConsts.Attachment.FunctionTypeId.Content, (int)resultList[i].ManagementStandardsContentId, this.db))
                            {
                                // 削除エラー
                                return false;
                            }
                        }

                    }
                }
            }

            // 全件問題無ければ登録処理
            if (errFlg)
            {
                return false;
            }

            return true;

            // 同一機器、部位、保全項目重複チェック
            bool checkEpContentDuplicate(Dao.excelPortManagementStandardResult result)
            {
                // 検索SQL文の取得
                dynamic whereParam = null; // WHERE句パラメータ
                string sql = string.Empty;
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetManagementStandardCountCheck, out sql))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
                whereParam = new { MachineId = result.MachineId, InspectionSiteStructureId = result.InspectionSiteStructureId, InspectionContentStructureId = result.InspectionContentStructureId, ManagementStandardsContentId = result.ManagementStandardsContentId };
                // 総件数を取得
                int cnt = db.GetCount(sql, whereParam);
                if (cnt > 0)
                {
                    return false;
                }

                return true;
            }

            // 開始日以降に保全活動が紐づいているスケジュールが存在する際はエラーとする
            bool checkEpScheduleAfterMsSummry(Dao.excelPortManagementStandardResult result)
            {
                // 検索SQL文の取得
                dynamic whereParam = null; // WHERE句パラメータ
                string sql = string.Empty;
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameManagementStandard.GetScheduleMsSummryCountAfterCheck, out sql))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
                whereParam = new { MaintainanceScheduleId = result.MaintainanceScheduleId, StartDate = result.StartDate };
                // 総件数を取得
                int cnt = db.GetCount(sql, whereParam);
                if (cnt > 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 保全スケジュール詳細データ新規登録
        /// </summary>
        /// <param name="res">登録データ</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool insertExcelPortMainteScheduleDetail(Dao.excelPortManagementStandardResult res, long maintainanceScheduleId, DateTime updateTime)
        {

            if (res.IsCyclic == true)
            {
                // スケジュール作成上限取得
                string strMaxYear = getItemExData(1, 2, (int)TMQConsts.MsStructure.GroupId.MakeScheduleYear, 0);

                // 対象機器の工場ID取得
                dynamic whereParam = new { MachineId = res.MachineId };
                string sql;
                TMQUtil.GetFixedSqlStatementSearch(false, SqlName.SubDir, SqlName.GetMachineDetail, out sql);
                IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(sql, whereParam);
                if (results == null || results.Count == 0)
                {
                    return false;
                }
                List<TMQUtil.StructureLayerInfo.StructureType> typeLst = new List<TMQUtil.StructureLayerInfo.StructureType>();
                typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Location);
                typeLst.Add(TMQUtil.StructureLayerInfo.StructureType.Job);
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, typeLst, this.db, this.LanguageId, true);

                // 工場毎年度終了月取得
                string strstartMonth = getItemExData(1, 2, (int)TMQConsts.MsStructure.GroupId.BeginningMonth, results[0].FactoryId);
                if (strstartMonth == null)
                {
                    // 工場毎の設定が取得できなければ工場共通
                    strstartMonth = getItemExData(1, 2, (int)TMQConsts.MsStructure.GroupId.BeginningMonth, 0);
                }
                if (int.Parse(strstartMonth) < 10)
                {
                    strstartMonth = "0" + strstartMonth;
                }

                DateTime endDate = DateTime.Parse(strMaxYear + "/" + strstartMonth + "/01");
                endDate = endDate.AddYears(1).AddDays(-1);

                DateTime dt = (DateTime)res.StartDate;

                // 保全スケジュール詳細データ
                ComDao.McMaintainanceScheduleDetailEntity scheduleDetail = new();
                scheduleDetail.InsertDatetime = updateTime;
                scheduleDetail.InsertUserId = int.Parse(this.UserId);
                scheduleDetail.UpdateDatetime = updateTime;
                scheduleDetail.UpdateUserId = int.Parse(this.UserId);
                scheduleDetail.SummaryId = null;
                scheduleDetail.Complition = false;
                scheduleDetail.MaintainanceScheduleId = maintainanceScheduleId;
                scheduleDetail.SequenceCount = 1;
                scheduleDetail.ScheduleDate = dt;
                (bool returnFlag, long id) maintainanceSchedule = (true, -1);
                // 開始日から終了日までスケジュール詳細作成
                while (dt <= endDate.Date)
                {
                    // 登録
                    maintainanceSchedule = registInsertDb<ComDao.McMaintainanceScheduleDetailEntity>(scheduleDetail, SqlNameManagementStandard.InsertMaintainanceScheduleDetail);
                    if (!maintainanceSchedule.returnFlag)
                    {
                        return false;
                    }

                    // 回数カウントアップ
                    scheduleDetail.SequenceCount = scheduleDetail.SequenceCount + 1;

                    // スケジュール日設定
                    if (res.CycleYear != null && res.CycleYear > 0)
                    {
                        dt = dt.AddYears((int)res.CycleYear);
                    }
                    if (res.CycleMonth != null && res.CycleMonth > 0)
                    {
                        dt = dt.AddMonths((int)res.CycleMonth);
                    }
                    if (res.CycleDay != null && res.CycleDay > 0)
                    {
                        dt = dt.AddDays((int)res.CycleDay);
                    }
                    scheduleDetail.ScheduleDate = dt;
                }
            }
            else
            {
                // 保全スケジュール詳細データ
                ComDao.McMaintainanceScheduleDetailEntity scheduleDetail = new();
                scheduleDetail.InsertDatetime = updateTime;
                scheduleDetail.InsertUserId = int.Parse(this.UserId);
                scheduleDetail.UpdateDatetime = updateTime;
                scheduleDetail.UpdateUserId = int.Parse(this.UserId);
                scheduleDetail.SummaryId = null;
                scheduleDetail.Complition = false;
                scheduleDetail.MaintainanceScheduleId = maintainanceScheduleId;
                scheduleDetail.SequenceCount = 1;
                scheduleDetail.ScheduleDate = res.StartDate;

                // 登録
                (bool returnFlag, long id) maintainanceSchedule = registInsertDb<ComDao.McMaintainanceScheduleDetailEntity>(scheduleDetail, SqlNameManagementStandard.InsertMaintainanceScheduleDetail);
                if (!maintainanceSchedule.returnFlag)
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// 機器別管理基準 保全項目編集画面 入力チェック
        /// </summary>
        /// <param name="registResult">保全項目編集画面で入力された内容</param>
        /// <returns>確認メッセージを表示する場合はTrue</returns>
        private bool confirmByIsUpdateAndScheduleDate(Dao.managementStandardResult registResult)
        {
            // スケジュールを更新 と 次回実施予定日の入力状態を判定
            if (this.Status < CommonProcReturn.ProcStatus.Confirm &&
                registResult.IsUpdateSchedule.ToString() == ComConsts.CHECK_FLG.OFF &&
                registResult.ScheduleDate != null)
            {
                // スケジュールを更新 が未選択かつ、次回実施予定日が入力されている場合
                // 確認メッセージを表示
                this.Status = CommonProcReturn.ProcStatus.Confirm;
                // 次回実施予定日が設定されている為　直近のスケジュールを１件更新します。よろしいですか？
                this.MsgId = GetResMessage("141120016");
                this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 機器別管理基準 保全項目編集画面 入力チェック
        /// </summary>
        /// <param name="insertFlg">新規登録の場合True</param>
        /// <param name="registResult">保全項目編集画面で入力された内容</param>
        /// <returns>確認メッセージを表示する場合はTrue</returns>
        private bool confirmByIsNewAndIsUpdate(bool insertFlg, Dao.managementStandardResult registResult)
        {
            // 新規or更新と次回実施予定日の入力状態を判定
            if (this.Status < CommonProcReturn.ProcStatus.Confirm &&
                insertFlg &&
                registResult.ScheduleDate != null)
            {
                // 新規登録かつ、次回実施予定日が入力されている場合
                // 確認メッセージを表示
                this.Status = CommonProcReturn.ProcStatus.Confirm;
                // 新規登録情報の為　次回実施予定日は考慮されません。処理を継続してもよろしいですか？
                this.MsgId = GetResMessage("141120018");
                this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 機器別管理基準 保全項目編集画面 入力チェック
        /// </summary>
        /// <param name="registResult">保全項目編集画面で入力された内容</param>
        /// <param name="dbresult">変更前の情報(DBから取得した値)</param>
        /// <returns>確認メッセージを表示する場合はTrue</returns>
        private bool confirmByScheduleInfoChanged(Dao.managementStandardResult registResult, IList<Dao.managementStandardResult> dbresult)
        {
            if (this.Status >= CommonProcReturn.ProcStatus.Confirm)
            {
                return false;
            }

            // DB値がNULLの場合は終了
            if (dbresult == null)
            {
                return false;
            }

            // 開始日、周期(年)、周期(月)、周期(日)がどれも変更されていない場合は終了
            if (registResult.CycleYear == dbresult[0].CycleYear &&
                registResult.CycleMonth == dbresult[0].CycleMonth &&
                registResult.CycleDay == dbresult[0].CycleDay &&
                registResult.StartDate == dbresult[0].StartDate)
            {
                return false;
            }

            // スケジュールを更新 が選択されている場合は終了
            if (registResult.IsUpdateSchedule.ToString() == ComConsts.CHECK_FLG.ON)
            {
                return false;
            }

            // ↓周期または開始日が変更されていて、スケジュールを更新 が選択されていない場合
            // 確認メッセージを表示
            this.Status = CommonProcReturn.ProcStatus.Confirm;
            // 周期または開始日が変更されていますが、スケジュールを更新 がチェックされていないためスケジュールの再作成はされません。よろしいですか？
            this.MsgId = GetResMessage("141120021");
            this.LogNo = ComConsts.LOG_NO.CONFIRM_LOG_NO;
            return true;
        }
    }
}
