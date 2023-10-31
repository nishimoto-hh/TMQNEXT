
using System;
using System.Collections.Generic;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using CommonDataBaseClass = CommonSTDUtil.CommonDataBaseClass;
using Const = CommonTMQUtil.CommonTMQConstants;

namespace CommonTMQUtil
{
    public class TMQCommonDataClass : CommonDataBaseClass
    {
        /// <summary>
        /// 長計件名
        /// </summary>
        public class LnLongPlanEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LnLongPlanEntity()
            {
                TableName = "ln_long_plan";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 長期計画件名ID</summary>
            /// <value>長期計画件名ID</value>
            public long LongPlanId { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? LocationDistrictStructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? LocationFactoryStructureId { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? LocationPlantStructureId { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? LocationSeriesStructureId { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? LocationStrokeStructureId { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? LocationFacilityStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobKindStructureId { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? JobLargeClassficationStructureId { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? JobMiddleClassficationStructureId { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? JobSmallClassficationStructureId { get; set; }
            /// <summary>Gets or sets 件名メモ</summary>
            /// <value>件名メモ</value>
            public string SubjectNote { get; set; }
            /// <summary>Gets or sets 担当者</summary>
            /// <value>担当者</value>
            public int? PersonId { get; set; }
            /// <summary>Gets or sets 担当者名</summary>
            /// <value>担当者名</value>
            public string PersonName { get; set; }
            /// <summary>Gets or sets 作業項目</summary>
            /// <value>作業項目</value>
            public int? WorkItemStructureId { get; set; }
            /// <summary>Gets or sets 予算管理区分</summary>
            /// <value>予算管理区分</value>
            public int? BudgetManagementStructureId { get; set; }
            /// <summary>Gets or sets 予算性格区分</summary>
            /// <value>予算性格区分</value>
            public int? BudgetPersonalityStructureId { get; set; }
            /// <summary>Gets or sets 保全時期</summary>
            /// <value>保全時期</value>
            public int? MaintenanceSeasonStructureId { get; set; }
            /// <summary>Gets or sets 目的区分</summary>
            /// <value>目的区分</value>
            public int? PurposeStructureId { get; set; }
            /// <summary>Gets or sets 作業区分</summary>
            /// <value>作業区分</value>
            public int? WorkClassStructureId { get; set; }
            /// <summary>Gets or sets 処置区分</summary>
            /// <value>処置区分</value>
            public int? TreatmentStructureId { get; set; }
            /// <summary>Gets or sets 設備区分</summary>
            /// <value>設備区分</value>
            public int? FacilityStructureId { get; set; }
            /// <summary>Gets or sets 長計区分</summary>
            /// <value>長計区分</value>
            public int? LongPlanDivisionStructureId { get; set; }
            /// <summary>Gets or sets 長計グループ</summary>
            /// <value>長計グループ</value>
            public int? LongPlanGroupStructureId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 長期計画件名ID</summary>
                /// <value>長期計画件名ID</value>
                public long LongPlanId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pLongPlanId)
                {
                    LongPlanId = pLongPlanId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.LongPlanId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public LnLongPlanEntity GetEntity(long pLongPlanId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pLongPlanId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<LnLongPlanEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pLongPlanId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pLongPlanId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 添付情報
        /// </summary>
        public class AttachmentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public AttachmentEntity()
            {
                TableName = "attachment";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 添付ID</summary>
            /// <value>添付ID</value>
            public int AttachmentId { get; set; }
            /// <summary>Gets or sets 機能タイプID</summary>
            /// <value>機能タイプID</value>
            public int FunctionTypeId { get; set; }
            /// <summary>Gets or sets キーID</summary>
            /// <value>キーID</value>
            public long KeyId { get; set; }
            /// <summary>Gets or sets 添付種類ID</summary>
            /// <value>添付種類ID</value>
            public int AttachmentTypeStructureId { get; set; }
            /// <summary>Gets or sets ファイルパス</summary>
            /// <value>ファイルパス</value>
            public string FilePath { get; set; }
            /// <summary>Gets or sets ファイル名</summary>
            /// <value>ファイル名</value>
            public string FileName { get; set; }
            /// <summary>Gets or sets 文書種類ID</summary>
            /// <value>文書種類ID</value>
            public int DocumentTypeStructureId { get; set; }
            /// <summary>Gets or sets 文書番号</summary>
            /// <value>文書番号</value>
            public string DocumentNo { get; set; }
            /// <summary>Gets or sets 文書説明</summary>
            /// <value>文書説明</value>
            public string AttachmentNote { get; set; }
            /// <summary>Gets or sets 作成者ID</summary>
            /// <value>作成者ID</value>
            public string AttachmentUserId { get; set; }
            /// <summary>Gets or sets 作成日</summary>
            /// <value>作成日</value>
            public DateTime? AttachmentDate { get; set; }
            /// <summary>Gets or sets 作成者</summary>
            /// <value>作成者</value>
            public string AttachmentUserName { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 添付ID</summary>
                /// <value>添付ID</value>
                public long AttachmentId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pAttachmentId)
                {
                    AttachmentId = pAttachmentId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.AttachmentId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public AttachmentEntity GetEntity(long pAttachmentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pAttachmentId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<AttachmentEntity>(getEntitySql);
            }

            /// <summary>
            /// 機能タイプIDとキーIDを指定して削除
            /// </summary>
            /// <param name="pFunctionTypeId">機能タイプID</param>
            /// <param name="pKeyId">キーID</param>
            /// <param name="db">DB接続</param>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByKeyId(Const.Attachment.FunctionTypeId pFunctionTypeId, long pKeyId, ComDB db)
            {
                // DELETE文取得
                string sql = getDeleteConditionSql(this.TableName, new List<string> { "function_type_id", "key_id" });
                // 削除条件
                AttachmentEntity condition = new();
                condition.FunctionTypeId = (int)pFunctionTypeId;
                condition.KeyId = pKeyId;
                // SQL実行
                int result = db.Regist(sql, condition);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全活動件名
        /// </summary>
        public class MaSummaryEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MaSummaryEntity()
            {
                TableName = "ma_summary";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long SummaryId { get; set; }
            /// <summary>Gets or sets 長期計画件名ID</summary>
            /// <value>長期計画件名ID</value>
            public long? LongPlanId { get; set; }
            /// <summary>Gets or sets 活動区分ID</summary>
            /// <value>活動区分ID</value>
            public int? ActivityDivision { get; set; }
            /// <summary>Gets or sets フォロー計画キーID</summary>
            /// <value>フォロー計画キーID</value>
            public long? FollowPlanKeyId { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets 作業計画・実施内容</summary>
            /// <value>作業計画・実施内容</value>
            public string PlanImplementationContent { get; set; }
            /// <summary>Gets or sets 件名メモ</summary>
            /// <value>件名メモ</value>
            public string SubjectNote { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? LocationDistrictStructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? LocationFactoryStructureId { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? LocationPlantStructureId { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? LocationSeriesStructureId { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? LocationStrokeStructureId { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? LocationFacilityStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
            /// <summary>Gets or sets MQ分類ID</summary>
            /// <value>MQ分類ID</value>
            public int? MqClassStructureId { get; set; }
            /// <summary>Gets or sets 修繕費分類ID</summary>
            /// <value>修繕費分類ID</value>
            public int? RepairCostClassStructureId { get; set; }
            /// <summary>Gets or sets 予算性格区分ID</summary>
            /// <value>予算性格区分ID</value>
            public int? BudgetPersonalityStructureId { get; set; }
            /// <summary>Gets or sets 予算管理区分ID</summary>
            /// <value>予算管理区分ID</value>
            public int? BudgetManagementStructureId { get; set; }
            /// <summary>Gets or sets 突発区分ID</summary>
            /// <value>突発区分ID</value>
            public int? SuddenDivisionStructureId { get; set; }
            /// <summary>Gets or sets 系停止ID</summary>
            /// <value>系停止ID</value>
            public int? StopSystemStructureId { get; set; }
            /// <summary>Gets or sets 系停止時間</summary>
            /// <value>系停止時間</value>
            public decimal? StopTime { get; set; }
            /// <summary>Gets or sets カウント件数</summary>
            /// <value>カウント件数</value>
            public int? MaintenanceCount { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public int? ChangeManagementStructureId { get; set; }
            /// <summary>Gets or sets 環境安全管理区分ID</summary>
            /// <value>環境安全管理区分ID</value>
            public int? EnvSafetyManagementStructureId { get; set; }
            /// <summary>Gets or sets 着工日</summary>
            /// <value>着工日</value>
            public DateTime? ConstructionDate { get; set; }
            /// <summary>Gets or sets 完了日</summary>
            /// <value>完了日</value>
            public DateTime? CompletionDate { get; set; }
            /// <summary>Gets or sets 完了時刻</summary>
            /// <value>完了時刻</value>
            public DateTime? CompletionTime { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 保全活動件名ID</summary>
                /// <value>保全活動件名ID</value>
                public long SummaryId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pSummaryId)
                {
                    SummaryId = pSummaryId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SummaryId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MaSummaryEntity GetEntity(long pSummaryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pSummaryId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MaSummaryEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pSummaryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pSummaryId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全依頼
        /// </summary>
        public class MaRequestEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MaRequestEntity()
            {
                TableName = "ma_request";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 依頼ID</summary>
            /// <value>依頼ID</value>
            public long RequestId { get; set; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long? SummaryId { get; set; }
            /// <summary>Gets or sets 依頼番号</summary>
            /// <value>依頼番号</value>
            public string RequestNo { get; set; }
            /// <summary>Gets or sets 発行日</summary>
            /// <value>発行日</value>
            public DateTime? IssueDate { get; set; }
            /// <summary>Gets or sets 緊急度ID</summary>
            /// <value>緊急度ID</value>
            public int? UrgencyStructureId { get; set; }
            /// <summary>Gets or sets 発見方法ID</summary>
            /// <value>発見方法ID</value>
            public int? DiscoveryMethodsStructureId { get; set; }
            /// <summary>Gets or sets 着工希望日</summary>
            /// <value>着工希望日</value>
            public DateTime? DesiredStartDate { get; set; }
            /// <summary>Gets or sets 完了希望日</summary>
            /// <value>完了希望日</value>
            public DateTime? DesiredEndDate { get; set; }
            /// <summary>Gets or sets 依頼内容</summary>
            /// <value>依頼内容</value>
            public string RequestContent { get; set; }
            /// <summary>Gets or sets 依頼部課係ID</summary>
            /// <value>依頼部課係ID</value>
            public int? RequestDepartmentClerkId { get; set; }
            /// <summary>Gets or sets 依頼担当者ID</summary>
            /// <value>依頼担当者ID</value>
            public int? RequestPersonnelId { get; set; }
            /// <summary>Gets or sets 依頼担当者名</summary>
            /// <value>依頼担当者名</value>
            public string RequestPersonnelName { get; set; }
            /// <summary>Gets or sets 依頼担当者TEL</summary>
            /// <value>依頼担当者TEL</value>
            public string RequestPersonnelTel { get; set; }
            /// <summary>Gets or sets 依頼係長ID</summary>
            /// <value>依頼係長ID</value>
            public int? RequestDepartmentChiefId { get; set; }
            /// <summary>Gets or sets 依頼係長名</summary>
            /// <value>依頼係長名</value>
            public string RequestDepartmentChiefName { get; set; }
            /// <summary>Gets or sets 依頼課長ID</summary>
            /// <value>依頼課長ID</value>
            public int? RequestDepartmentManagerId { get; set; }
            /// <summary>Gets or sets 依頼課長名</summary>
            /// <value>依頼課長名</value>
            public string RequestDepartmentManagerName { get; set; }
            /// <summary>Gets or sets 依頼職長ID</summary>
            /// <value>依頼職長ID</value>
            public int? RequestDepartmentForemanId { get; set; }
            /// <summary>Gets or sets 依頼職長名</summary>
            /// <value>依頼職長名</value>
            public string RequestDepartmentForemanName { get; set; }
            /// <summary>Gets or sets 保全部課係ID</summary>
            /// <value>保全部課係ID</value>
            public int? MaintenanceDepartmentClerkId { get; set; }
            /// <summary>Gets or sets 依頼事由</summary>
            /// <value>依頼事由</value>
            public string RequestReason { get; set; }
            /// <summary>Gets or sets 件名検討結果</summary>
            /// <value>件名検討結果</value>
            public string ExaminationResult { get; set; }
            /// <summary>Gets or sets 工事区分ID</summary>
            /// <value>工事区分ID</value>
            public int? ConstructionDivisionStructureId { get; set; }
            /// <summary>Gets or sets 承認者１ID</summary>
            /// <value>承認者１ID</value>
            public int? RequestAuthorizer1Id { get; set; }
            /// <summary>Gets or sets 承認者２ID</summary>
            /// <value>承認者２ID</value>
            public int? RequestAuthorizer2Id { get; set; }
            /// <summary>Gets or sets 承認者３ID</summary>
            /// <value>承認者３ID</value>
            public int? RequestAuthorizer3Id { get; set; }
            /// <summary>Gets or sets 場所</summary>
            /// <value>場所</value>
            public string ConstructionPlace { get; set; }
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 依頼ID</summary>
                /// <value>依頼ID</value>
                public long RequestId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pRequestId)
                {
                    RequestId = pRequestId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RequestId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MaRequestEntity GetEntity(long pRequestId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pRequestId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MaRequestEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pRequestId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pRequestId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全計画
        /// </summary>
        public class MaPlanEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MaPlanEntity()
            {
                TableName = "ma_plan";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 保全計画ID</summary>
            /// <value>保全計画ID</value>
            public long PlanId { get; set; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long? SummaryId { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets 発生日</summary>
            /// <value>発生日</value>
            public DateTime? OccurrenceDate { get; set; }
            /// <summary>Gets or sets 着工予定日</summary>
            /// <value>着工予定日</value>
            public DateTime? ExpectedConstructionDate { get; set; }
            /// <summary>Gets or sets 完了予定日</summary>
            /// <value>完了予定日</value>
            public DateTime? ExpectedCompletionDate { get; set; }
            /// <summary>Gets or sets 全体予算金額</summary>
            /// <value>全体予算金額</value>
            public decimal? TotalBudgetCost { get; set; }
            /// <summary>Gets or sets 予定工数</summary>
            /// <value>予定工数</value>
            public decimal? PlanManHour { get; set; }
            /// <summary>Gets or sets 自・他責ID</summary>
            /// <value>自・他責ID</value>
            public int? ResponsibilityStructureId { get; set; }
            /// <summary>Gets or sets 故障影響</summary>
            /// <value>故障影響</value>
            public string FailureEffect { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 保全計画ID</summary>
                /// <value>保全計画ID</value>
                public long PlanId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pPlanId)
                {
                    PlanId = pPlanId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PlanId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MaPlanEntity GetEntity(long pPlanId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pPlanId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MaPlanEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pPlanId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pPlanId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全履歴
        /// </summary>
        public class MaHistoryEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MaHistoryEntity()
            {
                TableName = "ma_history";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 履歴ID</summary>
            /// <value>履歴ID</value>
            public long HistoryId { get; set; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long? SummaryId { get; set; }
            /// <summary>Gets or sets 呼出回数</summary>
            /// <value>呼出回数</value>
            public int? CallCount { get; set; }
            /// <summary>Gets or sets 実績金額</summary>
            /// <value>実績金額</value>
            public decimal? Expenditure { get; set; }
            /// <summary>Gets or sets 費用メモ</summary>
            /// <value>費用メモ</value>
            public string CostNote { get; set; }
            /// <summary>Gets or sets 休損量</summary>
            /// <value>休損量</value>
            public int? LossAbsence { get; set; }
            /// <summary>Gets or sets 休損型数</summary>
            /// <value>休損型数</value>
            public int? LossAbsenceTypeCount { get; set; }
            /// <summary>Gets or sets 発生時刻</summary>
            /// <value>発生時刻</value>
            public DateTime? OccurrenceTime { get; set; }
            /// <summary>Gets or sets 発見者</summary>
            /// <value>発見者</value>
            public string DiscoveryPersonnel { get; set; }
            /// <summary>Gets or sets 施工担当者ID</summary>
            /// <value>施工担当者ID</value>
            public int? ConstructionPersonnelId { get; set; }
            /// <summary>Gets or sets 施工担当者名 </summary>
            /// <value>施工担当者名 </value>
            public string ConstructionPersonnelName { get; set; }
            /// <summary>Gets or sets 保全時期ID</summary>
            /// <value>保全時期ID</value>
            public int? MaintenanceSeasonStructureId { get; set; }
            /// <summary>Gets or sets 作業時間</summary>
            /// <value>作業時間</value>
            public decimal? TotalWorkingTime { get; set; }
            /// <summary>Gets or sets 作業時間(自係)</summary>
            /// <value>作業時間(自係)</value>
            public decimal? WorkingTimeSelf { get; set; }
            /// <summary>Gets or sets 調査時間</summary>
            /// <value>調査時間</value>
            public decimal? WorkingTimeResearch { get; set; }
            /// <summary>Gets or sets 調達時間</summary>
            /// <value>調達時間</value>
            public decimal? WorkingTimeProcure { get; set; }
            /// <summary>Gets or sets 修復時間</summary>
            /// <value>修復時間</value>
            public decimal? WorkingTimeRepair { get; set; }
            /// <summary>Gets or sets 試運転時間</summary>
            /// <value>試運転時間</value>
            public decimal? WorkingTimeTest { get; set; }
            /// <summary>Gets or sets 施工会社</summary>
            /// <value>施工会社</value>
            public string ConstructionCompany { get; set; }
            /// <summary>Gets or sets 作業時間(施工会社)</summary>
            /// <value>作業時間(施工会社)</value>
            public decimal? WorkingTimeCompany { get; set; }
            /// <summary>Gets or sets 実績結果ID</summary>
            /// <value>実績結果ID</value>
            public int? ActualResultStructureId { get; set; }
            /// <summary>Gets or sets 保全見解</summary>
            /// <value>保全見解</value>
            public string MaintenanceOpinion { get; set; }
            /// <summary>Gets or sets 製造担当者ID</summary>
            /// <value>製造担当者ID</value>
            public int? ManufacturingPersonnelId { get; set; }
            /// <summary>Gets or sets 製造担当者名</summary>
            /// <value>製造担当者名</value>
            public string ManufacturingPersonnelName { get; set; }
            /// <summary>Gets or sets 作業・故障区分ID</summary>
            /// <value>作業・故障区分ID</value>
            public int? WorkFailureDivisionStructureId { get; set; }
            /// <summary>Gets or sets 系停止回数</summary>
            /// <value>系停止回数</value>
            public int? StopCount { get; set; }
            /// <summary>Gets or sets 生産への影響ID</summary>
            /// <value>生産への影響ID</value>
            public int? EffectProductionStructureId { get; set; }
            /// <summary>Gets or sets 品質への影響ID</summary>
            /// <value>品質への影響ID</value>
            public int? EffectQualityStructureId { get; set; }
            /// <summary>Gets or sets 故障部位</summary>
            /// <value>故障部位</value>
            public string FailureSite { get; set; }
            /// <summary>Gets or sets 予備品有無</summary>
            /// <value>予備品有無</value>
            public bool? PartsExistenceFlg { get; set; }
            /// <summary>Gets or sets フォロー有無</summary>
            /// <value>フォロー有無</value>
            public bool? FollowFlg { get; set; }
            /// <summary>Gets or sets 故障時間</summary>
            /// <value>故障時間</value>
            public decimal? FailureTime { get; set; }
            /// <summary>Gets or sets 故障機器</summary>
            /// <value>故障機器</value>
            public int? FailureEquipmentModelStructureId { get; set; }
            /// <summary>Gets or sets ランク</summary>
            /// <value>ランク</value>
            public int? RankStructureId { get; set; }




            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 履歴ID</summary>
                /// <value>履歴ID</value>
                public long HistoryId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHistoryId)
                {
                    HistoryId = pHistoryId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HistoryId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MaHistoryEntity GetEntity(long pHistoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MaHistoryEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHistoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全履歴機器
        /// </summary>
        public class MaHistoryMachineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MaHistoryMachineEntity()
            {
                TableName = "ma_history_machine";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 保全履歴機器ID</summary>
            /// <value>保全履歴機器ID</value>
            public long HistoryMachineId { get; set; }
            /// <summary>Gets or sets 保全履歴ID</summary>
            /// <value>保全履歴ID</value>
            public long? HistoryId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long? EquipmentId { get; set; }
            /// <summary>Gets or sets 機器使用日数</summary>
            /// <value>機器使用日数</value>
            public int? UsedDaysMachine { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 保全履歴機器ID</summary>
                /// <value>保全履歴機器ID</value>
                public long HistoryMachineId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHistoryMachineId)
                {
                    HistoryMachineId = pHistoryMachineId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HistoryMachineId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MaHistoryMachineEntity GetEntity(long pHistoryMachineId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryMachineId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MaHistoryMachineEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHistoryMachineId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryMachineId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全履歴機器部位
        /// </summary>
        public class MaHistoryInspectionSiteEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MaHistoryInspectionSiteEntity()
            {
                TableName = "ma_history_inspection_site";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 保全履歴機器部位ID</summary>
            /// <value>保全履歴機器部位ID</value>
            public long HistoryInspectionSiteId { get; set; }
            /// <summary>Gets or sets 保全履歴機器ID</summary>
            /// <value>保全履歴機器ID</value>
            public long? HistoryMachineId { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 保全履歴機器部位ID</summary>
                /// <value>保全履歴機器部位ID</value>
                public long HistoryInspectionSiteId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHistoryInspectionSiteId)
                {
                    HistoryInspectionSiteId = pHistoryInspectionSiteId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HistoryInspectionSiteId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MaHistoryInspectionSiteEntity GetEntity(long pHistoryInspectionSiteId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryInspectionSiteId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MaHistoryInspectionSiteEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHistoryInspectionSiteId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryInspectionSiteId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全履歴点検内容
        /// </summary>
        public class MaHistoryInspectionContentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MaHistoryInspectionContentEntity()
            {
                TableName = "ma_history_inspection_content";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 保全履歴点検内容ID</summary>
            /// <value>保全履歴点検内容ID</value>
            public long HistoryInspectionContentId { get; set; }
            /// <summary>Gets or sets 保全履歴部位ID</summary>
            /// <value>保全履歴部位ID</value>
            public long? HistoryInspectionSiteId { get; set; }
            /// <summary>Gets or sets 点検内容ID</summary>
            /// <value>点検内容ID</value>
            public int? InspectionContentStructureId { get; set; }
            /// <summary>Gets or sets フォロー有無</summary>
            /// <value>フォロー有無</value>
            public bool? FollowFlg { get; set; }
            /// <summary>Gets or sets フォロー予定日</summary>
            /// <value>フォロー予定日</value>
            public DateTime? FollowPlanDate { get; set; }
            /// <summary>Gets or sets フォロー内容</summary>
            /// <value>フォロー内容</value>
            public string FollowContent { get; set; }
            /// <summary>Gets or sets フォロー完了日</summary>
            /// <value>フォロー完了日</value>
            public DateTime? FollowCompletionDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 保全履歴点検内容ID</summary>
                /// <value>保全履歴点検内容ID</value>
                public long HistoryInspectionContentId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHistoryInspectionContentId)
                {
                    HistoryInspectionContentId = pHistoryInspectionContentId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HistoryInspectionContentId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MaHistoryInspectionContentEntity GetEntity(long pHistoryInspectionContentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryInspectionContentId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MaHistoryInspectionContentEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHistoryInspectionContentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryInspectionContentId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全履歴故障情報
        /// </summary>
        public class MaHistoryFailureEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MaHistoryFailureEntity()
            {
                TableName = "ma_history_failure";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 保全履歴故障情報ID</summary>
            /// <value>保全履歴故障情報ID</value>
            public long HistoryFailureId { get; set; }
            /// <summary>Gets or sets 保全履歴ID</summary>
            /// <value>保全履歴ID</value>
            public long? HistoryId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long? EquipmentId { get; set; }
            /// <summary>Gets or sets 保全部位</summary>
            /// <value>保全部位</value>
            public string MaintenanceSite { get; set; }
            /// <summary>Gets or sets 保全内容</summary>
            /// <value>保全内容</value>
            public string MaintenanceContent { get; set; }
            /// <summary>Gets or sets 現象ID</summary>
            /// <value>現象ID</value>
            public int? PhenomenonStructureId { get; set; }
            /// <summary>Gets or sets 現象メモ</summary>
            /// <value>現象メモ</value>
            public string PhenomenonNote { get; set; }
            /// <summary>Gets or sets 原因階層ID</summary>
            /// <value>原因階層ID</value>
            public int? FailureCauseStructureId { get; set; }
            /// <summary>Gets or sets 原因メモ</summary>
            /// <value>原因メモ</value>
            public string FailureCauseNote { get; set; }
            /// <summary>Gets or sets 原因性格ID</summary>
            /// <value>原因性格ID</value>
            public int? FailureCausePersonalityStructureId { get; set; }
            /// <summary>Gets or sets 原因性格メモ</summary>
            /// <value>原因性格メモ</value>
            public string FailureCausePersonalityNote { get; set; }
            /// <summary>Gets or sets 処置・対策ID</summary>
            /// <value>処置・対策ID</value>
            public int? TreatmentMeasureStructureId { get; set; }
            /// <summary>Gets or sets 処置・対策メモ</summary>
            /// <value>処置・対策メモ</value>
            public string TreatmentMeasureNote { get; set; }
            /// <summary>Gets or sets 故障状況</summary>
            /// <value>故障状況</value>
            public string FailureStatus { get; set; }
            /// <summary>Gets or sets 故障原因補足</summary>
            /// <value>故障原因補足</value>
            public string FailureCauseAdditionNote { get; set; }
            /// <summary>Gets or sets 故障前の保全状況</summary>
            /// <value>故障前の保全状況</value>
            public string PreviousSituation { get; set; }
            /// <summary>Gets or sets 復旧処置</summary>
            /// <value>復旧処置</value>
            public string RecoveryAction { get; set; }
            /// <summary>Gets or sets 改善対策</summary>
            /// <value>改善対策</value>
            public string ImprovementMeasure { get; set; }
            /// <summary>Gets or sets 保全システムへのフィードバック</summary>
            /// <value>保全システムへのフィードバック</value>
            public string SystemFeedBack { get; set; }
            /// <summary>Gets or sets 教訓</summary>
            /// <value>教訓</value>
            public string Lesson { get; set; }
            /// <summary>Gets or sets 特記事項</summary>
            /// <value>特記事項</value>
            public string FailureNote { get; set; }
            /// <summary>Gets or sets 故障分析ID</summary>
            /// <value>故障分析ID</value>
            public int? FailureAnalysisStructureId { get; set; }
            /// <summary>Gets or sets 故障性格要因ID</summary>
            /// <value>故障性格要因ID</value>
            public int? FailurePersonalityFactorStructureId { get; set; }
            /// <summary>Gets or sets 故障性格分類ID</summary>
            /// <value>故障性格分類ID</value>
            public int? FailurePersonalityClassStructureId { get; set; }
            /// <summary>Gets or sets 処置状況ID</summary>
            /// <value>処置状況ID</value>
            public int? TreatmentStatusStructureId { get; set; }
            /// <summary>Gets or sets 対策要否ID</summary>
            /// <value>対策要否ID</value>
            public int? NecessityMeasureStructureId { get; set; }
            /// <summary>Gets or sets 対策実施予定日</summary>
            /// <value>対策実施予定日</value>
            public DateTime? MeasurePlanDate { get; set; }
            /// <summary>Gets or sets 対策分類1階層ID</summary>
            /// <value>対策分類1階層ID</value>
            public int? MeasureClass1StructureId { get; set; }
            /// <summary>Gets or sets 対策分類2階層ID</summary>
            /// <value>対策分類2階層ID</value>
            public int? MeasureClass2StructureId { get; set; }
            /// <summary>Gets or sets フォロー有無</summary>
            /// <value>フォロー有無</value>
            public bool? FollowFlg { get; set; }
            /// <summary>Gets or sets フォロー予定日</summary>
            /// <value>フォロー予定日</value>
            public DateTime? FollowPlanDate { get; set; }
            /// <summary>Gets or sets フォロー内容</summary>
            /// <value>フォロー内容</value>
            public string FollowContent { get; set; }
            /// <summary>Gets or sets フォロー完了日</summary>
            /// <value>フォロー完了日</value>
            public DateTime? FollowCompletionDate { get; set; }
            /// <summary>Gets or sets 機器使用日数</summary>
            /// <value>機器使用日数</value>
            public int? UsedDaysMachine { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 保全履歴故障情報ID</summary>
                /// <value>保全履歴故障情報ID</value>
                public long HistoryFailureId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHistoryFailureId)
                {
                    HistoryFailureId = pHistoryFailureId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HistoryFailureId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MaHistoryFailureEntity GetEntity(long pHistoryFailureId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryFailureId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MaHistoryFailureEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHistoryFailureId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryFailureId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 依頼番号採番テーブル
        /// </summary>
        public class MaRequestNumberingEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MaRequestNumberingEntity()
            {
                TableName = "ma_request_numbering";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 採番ID</summary>
            /// <value>採番ID</value>
            public long NumberingId { get; set; }
            /// <summary>Gets or sets 採番パターン</summary>
            /// <value>採番パターン</value>
            public int NumberingPattern { get; set; }
            /// <summary>Gets or sets 年</summary>
            /// <value>年</value>
            public int Year { get; set; }
            /// <summary>Gets or sets 場所階層ID</summary>
            /// <value>場所階層ID</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int SeqNo { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 採番ID</summary>
                /// <value>採番ID</value>
                public long NumberingId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pNumberingId)
                {
                    NumberingId = pNumberingId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.NumberingId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MaRequestNumberingEntity GetEntity(long pNumberingId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pNumberingId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MaRequestNumberingEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pNumberingId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pNumberingId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// アイテムマスタ拡張
        /// </summary>
        public class MsItemExtensionEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsItemExtensionEntity()
            {
                TableName = "ms_item_extension";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets アイテムID</summary>
            /// <value>アイテムID</value>
            public int ItemId { get; set; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int SequenceNo { get; set; }
            /// <summary>Gets or sets 拡張データ</summary>
            /// <value>拡張データ</value>
            public string ExtensionData { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets アイテムID</summary>
                /// <value>アイテムID</value>
                public int ItemId { get; set; }
                /// <summary>Gets or sets 連番</summary>
                /// <value>連番</value>
                public int SequenceNo { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pItemId, int pSequenceNo)
                {
                    ItemId = pItemId;
                    SequenceNo = pSequenceNo;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemId, this.SequenceNo);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsItemExtensionEntity GetEntity(int pItemId, int pSequenceNo, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pItemId, pSequenceNo);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsItemExtensionEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pItemId, int pSequenceNo, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pItemId, pSequenceNo);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 機番情報
        /// </summary>
        public class McMachineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McMachineEntity()
            {
                TableName = "mc_machine";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long MachineId { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? LocationDistrictStructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? LocationFactoryStructureId { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? LocationPlantStructureId { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? LocationSeriesStructureId { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? LocationStrokeStructureId { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? LocationFacilityStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobKindStructureId { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? JobLargeClassficationStructureId { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? JobMiddleClassficationStructureId { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? JobSmallClassficationStructureId { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 設置場所</summary>
            /// <value>設置場所</value>
            public string InstallationLocation { get; set; }
            /// <summary>Gets or sets 設置台数</summary>
            /// <value>設置台数</value>
            public decimal? NumberOfInstallation { get; set; }
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int? EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 設置日</summary>
            /// <value>設置日</value>
            public DateTime? DateOfInstallation { get; set; }
            /// <summary>Gets or sets 重要度</summary>
            /// <value>重要度</value>
            public int? ImportanceStructureId { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public int? ConservationStructureId { get; set; }
            /// <summary>Gets or sets 機番メモ</summary>
            /// <value>機番メモ</value>
            public string MachineNote { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機番ID</summary>
                /// <value>機番ID</value>
                public long MachineId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pMachineId)
                {
                    MachineId = pMachineId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MachineId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McMachineEntity GetEntity(long pMachineId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMachineId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McMachineEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pMachineId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMachineId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 機器情報
        /// </summary>
        public class McEquipmentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McEquipmentEntity()
            {
                TableName = "mc_equipment";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long EquipmentId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 循環対象</summary>
            /// <value>循環対象</value>
            public bool CirculationTargetFlg { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public int? ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets メーカー型式</summary>
            /// <value>メーカー型式</value>
            public string ManufacturerType { get; set; }
            /// <summary>Gets or sets 型式コード</summary>
            /// <value>型式コード</value>
            public string ModelNo { get; set; }
            /// <summary>Gets or sets シリアル番号</summary>
            /// <value>シリアル番号</value>
            public string SerialNo { get; set; }
            /// <summary>Gets or sets 製造日</summary>
            /// <value>製造日</value>
            public DateTime? DateOfManufacture { get; set; }
            /// <summary>Gets or sets 納期</summary>
            /// <value>納期</value>
            public int? DeliveryDate { get; set; }
            /// <summary>Gets or sets 機器メモ</summary>
            /// <value>機器メモ</value>
            public string EquipmentNote { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public int? UseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 固定資産番号</summary>
            /// <value>固定資産番号</value>
            public string FixedAssetNo { get; set; }
            /// <summary>Gets or sets 点検種別毎管理</summary>
            /// <value>点検種別毎管理</value>
            public bool MaintainanceKindManage { get; set; }
            /// <summary>Gets or sets 予算管理部門</summary>
            /// <value>予算管理部門</value>
            public int? BudgetManagementStructureId { get; set; }
            /// <summary>Gets or sets 図面保管場所</summary>
            /// <value>図面保管場所</value>
            public int? DiagramStorageLocationStructureId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機器ID</summary>
                /// <value>機器ID</value>
                public long EquipmentId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pEquipmentId)
                {
                    EquipmentId = pEquipmentId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.EquipmentId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McEquipmentEntity GetEntity(long pEquipmentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pEquipmentId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McEquipmentEntity>(getEntitySql);
            }

            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pMachineId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMachineId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 仕様情報
        /// </summary>
        public class McEquipmentSpecEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McEquipmentSpecEntity()
            {
                TableName = "mc_equipment_spec";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機種別仕様ID</summary>
            /// <value>機種別仕様ID</value>
            public long EquipmentSpecId { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long? EquipmentId { get; set; }
            /// <summary>Gets or sets 仕様項目ID</summary>
            /// <value>仕様項目ID</value>
            public int? SpecId { get; set; }
            /// <summary>Gets or sets 設定値(テキスト)</summary>
            /// <value>設定値(テキスト)</value>
            public string SpecValue { get; set; }
            /// <summary>Gets or sets 設定値(選択)</summary>
            /// <value>設定値(選択)</value>
            public int? SpecStructureId { get; set; }
            /// <summary>Gets or sets 設定値(数値)</summary>
            /// <value>設定値(数値)</value>
            public decimal? SpecNum { get; set; }
            /// <summary>Gets or sets 設定値(数値(範囲))最小値</summary>
            /// <value>設定値(数値(範囲))最小値</value>
            public decimal? SpecNumMin { get; set; }
            /// <summary>Gets or sets 設定値(数値(範囲))最大値</summary>
            /// <value>設定値(数値(範囲))最大値</value>
            public decimal? SpecNumMax { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機種別仕様ID</summary>
                /// <value>機種別仕様ID</value>
                public long EquipmentSpecId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pEquipmentSpecId)
                {
                    EquipmentSpecId = pEquipmentSpecId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.EquipmentSpecId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McEquipmentSpecEntity GetEntity(long pEquipmentSpecId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pEquipmentSpecId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McEquipmentSpecEntity>(getEntitySql);
            }
        }
        /// <summary>
        /// 適用法規情報
        /// </summary>
        public class McApplicableLawsEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McApplicableLawsEntity()
            {
                TableName = "mc_applicable_laws";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 適用法規ID</summary>
            /// <value>適用法規ID</value>
            public long ApplicableLawsId { get; set; }
            /// <summary>Gets or sets 適用法規アイテムID</summary>
            /// <value>適用法規アイテムID</value>
            public int? ApplicableLawsStructureId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 適用法規ID</summary>
                /// <value>適用法規ID</value>
                public long ApplicableLawsId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pApplicableLawsId)
                {
                    ApplicableLawsId = pApplicableLawsId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ApplicableLawsId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McApplicableLawsEntity GetEntity(long pApplicableLawsId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pApplicableLawsId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McApplicableLawsEntity>(getEntitySql);
            }
        }
        /// <summary>
        /// 機器別管理基準部位
        /// </summary>
        public class McManagementStandardsComponentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McManagementStandardsComponentEntity()
            {
                TableName = "mc_management_standards_component";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機器別管理基準部位ID</summary>
            /// <value>機器別管理基準部位ID</value>
            public long ManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 機器別管理基準フラグ</summary>
            /// <value>機器別管理基準フラグ</value>
            public bool? IsManagementStandardConponent { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機器別管理基準部位ID</summary>
                /// <value>機器別管理基準部位ID</value>
                public long ManagementStandardsComponentId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pManagementStandardsComponentId)
                {
                    ManagementStandardsComponentId = pManagementStandardsComponentId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ManagementStandardsComponentId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McManagementStandardsComponentEntity GetEntity(long pManagementStandardsComponentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pManagementStandardsComponentId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McManagementStandardsComponentEntity>(getEntitySql);
            }
        }

        /// <summary>
        /// 機器別管理基準内容
        /// </summary>
        public class McManagementStandardsContentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McManagementStandardsContentEntity()
            {
                TableName = "mc_management_standards_content";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 機器別管理基準部位ID</summary>
            /// <value>機器別管理基準部位ID</value>
            public long? ManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 点検内容ID</summary>
            /// <value>点検内容ID</value>
            public int? InspectionContentStructureId { get; set; }
            /// <summary>Gets or sets 部位重要度</summary>
            /// <value>部位重要度</value>
            public int? InspectionSiteImportanceStructureId { get; set; }
            /// <summary>Gets or sets 部位保全方式</summary>
            /// <value>部位保全方式</value>
            public int? InspectionSiteConservationStructureId { get; set; }
            /// <summary>Gets or sets 保全区分</summary>
            /// <value>保全区分</value>
            public int? MaintainanceDivision { get; set; }
            /// <summary>Gets or sets 点検種別</summary>
            /// <value>点検種別</value>
            public int? MaintainanceKindStructureId { get; set; }
            /// <summary>Gets or sets 予算金額</summary>
            /// <value>予算金額</value>
            public decimal? BudgetAmount { get; set; }
            /// <summary>Gets or sets 準備期間(日)</summary>
            /// <value>準備期間(日)</value>
            public int? PreparationPeriod { get; set; }
            /// <summary>Gets or sets 長計件名ID</summary>
            /// <value>長計件名ID</value>
            public long? LongPlanId { get; set; }
            /// <summary>Gets or sets 並び順</summary>
            /// <value>並び順</value>
            public int? OrderNo { get; set; }
            /// <summary>Gets or sets スケジュール管理基準ID</summary>
            /// <value>スケジュール管理基準ID</value>
            public int? ScheduleTypeStructureId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機器別管理基準内容ID</summary>
                /// <value>機器別管理基準内容ID</value>
                public long ManagementStandardsContentId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pManagementStandardsContentId)
                {
                    ManagementStandardsContentId = pManagementStandardsContentId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ManagementStandardsContentId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McManagementStandardsContentEntity GetEntity(long pManagementStandardsContentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pManagementStandardsContentId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McManagementStandardsContentEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pManagementStandardsContentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pManagementStandardsContentId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全スケジュール
        /// </summary>
        public class McMaintainanceScheduleEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McMaintainanceScheduleEntity()
            {
                TableName = "mc_maintainance_schedule";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 保全スケジュールID</summary>
            /// <value>保全スケジュールID</value>
            public long MaintainanceScheduleId { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long? ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 周期ありフラグ</summary>
            /// <value>周期ありフラグ</value>
            public bool? IsCyclic { get; set; }
            /// <summary>Gets or sets 周期(年)</summary>
            /// <value>周期(年)</value>
            public int? CycleYear { get; set; }
            /// <summary>Gets or sets 周期(月)</summary>
            /// <value>周期(月)</value>
            public int? CycleMonth { get; set; }
            /// <summary>Gets or sets 周期(日)</summary>
            /// <value>周期(日)</value>
            public int? CycleDay { get; set; }
            /// <summary>Gets or sets 表示周期</summary>
            /// <value>表示周期</value>
            public string DispCycle { get; set; }
            /// <summary>Gets or sets 開始日</summary>
            /// <value>開始日</value>
            public DateTime? StartDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 保全スケジュールID</summary>
                /// <value>保全スケジュールID</value>
                public long MaintainanceScheduleId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pMaintainanceScheduleId)
                {
                    MaintainanceScheduleId = pMaintainanceScheduleId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MaintainanceScheduleId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McMaintainanceScheduleEntity GetEntity(long pMaintainanceScheduleId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMaintainanceScheduleId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McMaintainanceScheduleEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pMaintainanceScheduleId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMaintainanceScheduleId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全スケジュール詳細
        /// </summary>
        public class McMaintainanceScheduleDetailEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McMaintainanceScheduleDetailEntity()
            {
                TableName = "mc_maintainance_schedule_detail";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 保全スケジュール詳細ID</summary>
            /// <value>保全スケジュール詳細ID</value>
            public long MaintainanceScheduleDetailId { get; set; }
            /// <summary>Gets or sets 保全スケジュールID</summary>
            /// <value>保全スケジュールID</value>
            public long? MaintainanceScheduleId { get; set; }
            /// <summary>Gets or sets 繰り返し回数</summary>
            /// <value>繰り返し回数</value>
            public int? SequenceCount { get; set; }
            /// <summary>Gets or sets スケジュール日</summary>
            /// <value>スケジュール日</value>
            public DateTime? ScheduleDate { get; set; }
            /// <summary>Gets or sets 完了フラグ</summary>
            /// <value>完了フラグ</value>
            public bool? Complition { get; set; }
            /// <summary>Gets or sets 保全活動件名ID</summary>
            /// <value>保全活動件名ID</value>
            public long? SummaryId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 保全スケジュール詳細ID</summary>
                /// <value>保全スケジュール詳細ID</value>
                public long MaintainanceScheduleDetailId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pMaintainanceScheduleDetailId)
                {
                    MaintainanceScheduleDetailId = pMaintainanceScheduleDetailId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MaintainanceScheduleDetailId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McMaintainanceScheduleDetailEntity GetEntity(long pMaintainanceScheduleDetailId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMaintainanceScheduleDetailId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McMaintainanceScheduleDetailEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pMaintainanceScheduleDetailId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMaintainanceScheduleDetailId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 機器親子構成
        /// </summary>
        public class McMachineParentInfoEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McMachineParentInfoEntity()
            {
                TableName = "mc_machine_parent_info";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 親子構成ID</summary>
            /// <value>親子構成ID</value>
            public long ParentId { get; set; }
            /// <summary>Gets or sets 親子構成元ID</summary>
            /// <value>親子構成元ID</value>
            public long? ParentMotoId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 親子構成ID</summary>
                /// <value>親子構成ID</value>
                public long ParentId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pParentId)
                {
                    ParentId = pParentId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ParentId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McMachineParentInfoEntity GetEntity(long pParentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pParentId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McMachineParentInfoEntity>(getEntitySql);
            }
        }
        /// <summary>
        /// ループ構成
        /// </summary>
        public class McLoopInfoEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McLoopInfoEntity()
            {
                TableName = "mc_loop_info";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ループID</summary>
            /// <value>ループID</value>
            public long LoopId { get; set; }
            /// <summary>Gets or sets ループ元ID</summary>
            /// <value>ループ元ID</value>
            public long? LoopMotoId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ループID</summary>
                /// <value>ループID</value>
                public long LoopId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pLoopId)
                {
                    LoopId = pLoopId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.LoopId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McLoopInfoEntity GetEntity(long pLoopId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pLoopId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McLoopInfoEntity>(getEntitySql);
            }
        }
        /// <summary>
        /// 付属品構成
        /// </summary>
        public class McAccessoryInfoEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McAccessoryInfoEntity()
            {
                TableName = "mc_accessory_info";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 付属品ID</summary>
            /// <value>付属品ID</value>
            public long AccessoryId { get; set; }
            /// <summary>Gets or sets 付属品構成元ID</summary>
            /// <value>付属品構成元ID</value>
            public long? AccessoryMotoId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 付属品ID</summary>
                /// <value>付属品ID</value>
                public long AccessoryId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pAccessoryId)
                {
                    AccessoryId = pAccessoryId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.AccessoryId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McAccessoryInfoEntity GetEntity(long pAccessoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pAccessoryId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McAccessoryInfoEntity>(getEntitySql);
            }
        }
        /// <summary>
        /// MP設計情報
        /// </summary>
        public class McMpInformationEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McMpInformationEntity()
            {
                TableName = "mc_mp_information";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets MP情報ID</summary>
            /// <value>MP情報ID</value>
            public long MpInformationId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets MP情報</summary>
            /// <value>MP情報</value>
            public string MpInformation { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets MP情報ID</summary>
                /// <value>MP情報ID</value>
                public long MpInformationId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pMpInformationId)
                {
                    MpInformationId = pMpInformationId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MpInformationId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McMpInformationEntity GetEntity(long pMpInformationId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMpInformationId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McMpInformationEntity>(getEntitySql);
            }
        }
        /// <summary>
        /// 機番使用部品情報
        /// </summary>
        public class McMachineUsePartsEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public McMachineUsePartsEntity()
            {
                TableName = "mc_machine_use_parts";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機番使用部品情報ID</summary>
            /// <value>機番使用部品情報ID</value>
            public long MachineUsePartsId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long? PartsId { get; set; }
            /// <summary>Gets or sets 使用個数</summary>
            /// <value>使用個数</value>
            public decimal? UseQuantity { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機番使用部品情報ID</summary>
                /// <value>機番使用部品情報ID</value>
                public long MachineUsePartsId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pMachineUsePartsId)
                {
                    MachineUsePartsId = pMachineUsePartsId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MachineUsePartsId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public McMachineUsePartsEntity GetEntity(long pMachineUsePartsId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMachineUsePartsId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<McMachineUsePartsEntity>(getEntitySql);
            }
        }

        /// <summary>
        /// ユーザマスタ
        /// </summary>
        public class MsUserEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsUserEntity()
            {
                TableName = "ms_user";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public int UserId { get; set; }
            /// <summary>Gets or sets ログインID</summary>
            /// <value>ログインID</value>
            public string LoginId { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            public string LanguageId { get; set; }
            /// <summary>Gets or sets 権限レベルID</summary>
            /// <value>権限レベルID</value>
            public int AuthorityLevelId { get; set; }
            /// <summary>Gets or sets ユーザ表示名</summary>
            /// <value>ユーザ表示名</value>
            public string DisplayName { get; set; }
            /// <summary>Gets or sets ユーザ姓</summary>
            /// <value>ユーザ姓</value>
            public string FamilyName { get; set; }
            /// <summary>Gets or sets ユーザ名</summary>
            /// <value>ユーザ名</value>
            public string FirstName { get; set; }
            /// <summary>Gets or sets メールアドレス</summary>
            /// <value>メールアドレス</value>
            public string MailAddress { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ユーザID</summary>
                /// <value>ユーザID</value>
                public int UserId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pUserId)
                {
                    UserId = pUserId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.UserId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsUserEntity GetEntity(int pUserId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pUserId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsUserEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pUserId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pUserId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 構成マスタ
        /// </summary>
        public class MsStructureEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsStructureEntity()
            {
                TableName = "ms_structure";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int StructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? FactoryId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }
            /// <summary>Gets or sets 親構成ID</summary>
            /// <value>親構成ID</value>
            public int? ParentStructureId { get; set; }
            /// <summary>Gets or sets 構成階層番号</summary>
            /// <value>構成階層番号</value>
            public int? StructureLayerNo { get; set; }
            /// <summary>Gets or sets 構成アイテムID</summary>
            /// <value>構成アイテムID</value>
            public int? StructureItemId { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DisplayOrder { get; set; }
            /// <summary>Gets or sets 削除フラグ</summary>
            /// <value>削除フラグ</value>
            public bool DeleteFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 構成ID</summary>
                /// <value>構成ID</value>
                public int StructureId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pStructureId)
                {
                    StructureId = pStructureId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.StructureId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsStructureEntity GetEntity(int pStructureId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pStructureId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsStructureEntity>(getEntitySql);
            }

            /// <summary>
            /// 構成グループID取得処理
            /// </summary>
            /// <param name="pStructureGroupId">取得する構成グループID</param>
            /// <param name="db">DB接続</param>
            /// <returns>構成グループIDで絞り込んだ構成マスタの内容</returns>
            public IList<MsStructureEntity> GetGroupList(Const.MsStructure.GroupId pStructureGroupId, ComDB db)
            {
                string sql = getListConditionSql(this.TableName, new List<string> { "structure_group_id" });
                MsStructureEntity condition = new();
                condition.StructureGroupId = (int)pStructureGroupId;
                return db.GetListByDataClass<MsStructureEntity>(sql, condition);
            }
        }

        /// <summary>
        /// 予備品仕様マスタ
        /// </summary>
        public class PtPartsEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PtPartsEntity()
            {
                TableName = "pt_parts";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets 予備品№</summary>
            /// <value>予備品№</value>
            public string PartsNo { get; set; }
            /// <summary>Gets or sets 予備品名称</summary>
            /// <value>予備品名称</value>
            public string PartsName { get; set; }
            /// <summary>Gets or sets メーカーID</summary>
            /// <value>メーカーID</value>
            public long? ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets 材質</summary>
            /// <value>材質</value>
            public string Materials { get; set; }
            /// <summary>Gets or sets 型式</summary>
            /// <value>型式</value>
            public string ModelType { get; set; }
            /// <summary>Gets or sets 規格・寸法</summary>
            /// <value>規格・寸法</value>
            public string StandardSize { get; set; }
            /// <summary>Gets or sets 使用場所</summary>
            /// <value>使用場所</value>
            public string PartsServiceSpace { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public long FactoryId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public long? JobStructureId { get; set; }
            /// <summary>Gets or sets 標準棚ID</summary>
            /// <value>標準棚ID</value>
            public long PartsLocationId { get; set; }
            /// <summary>Gets or sets 標準棚_地区ID</summary>
            /// <value>標準棚_地区ID</value>
            public long? LocationDistrictStructureId { get; set; }
            /// <summary>Gets or sets 標準棚_工場ID</summary>
            /// <value>標準棚_工場ID</value>
            public long? LocationFactoryStructureId { get; set; }
            /// <summary>Gets or sets 標準棚_倉庫ID</summary>
            /// <value>標準棚_倉庫ID</value>
            public long? LocationWarehouseStructureId { get; set; }
            /// <summary>Gets or sets 標準棚_棚ID</summary>
            /// <value>標準棚_棚ID</value>
            public long? LocationRackStructureId { get; set; }
            /// <summary>Gets or sets 標準棚枝番</summary>
            /// <value>標準棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 発注点</summary>
            /// <value>発注点</value>
            public decimal? LeadTime { get; set; }
            /// <summary>Gets or sets 発注量</summary>
            /// <value>発注量</value>
            public decimal? OrderQuantity { get; set; }
            /// <summary>Gets or sets 数量管理単位ID</summary>
            /// <value>数量管理単位ID</value>
            public long? UnitStructureId { get; set; }
            /// <summary>Gets or sets 仕入先ID</summary>
            /// <value>仕入先ID</value>
            public long? VenderStructureId { get; set; }
            /// <summary>Gets or sets 金額管理単位ID</summary>
            /// <value>金額管理単位ID</value>
            public long? CurrencyStructureId { get; set; }
            /// <summary>Gets or sets 標準単価</summary>
            /// <value>標準単価</value>
            public decimal? UnitPrice { get; set; }
            /// <summary>Gets or sets 購買システムコード</summary>
            /// <value>購買システムコード</value>
            public string PurchasingNo { get; set; }
            /// <summary>Gets or sets メモ</summary>
            /// <value>メモ</value>
            public string PartsMemo { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public long? UseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 標準部門</summary>
            /// <value>標準部門</value>
            public long? DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 標準勘定科目</summary>
            /// <value>標準勘定科目</value>
            public long? AccountStructureId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 予備品ID</summary>
                /// <value>予備品ID</value>
                public long PartsId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pPartsId)
                {
                    PartsId = pPartsId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.PartsId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public PtPartsEntity GetEntity(long pPartsId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pPartsId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<PtPartsEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pPartsId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pPartsId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 予備品No採番テーブル
        /// </summary>
        public class PtPartsNoNumberingEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PtPartsNoNumberingEntity()
            {
                TableName = "pt_partsno_numbering";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 採番ID</summary>
            /// <value>採番ID</value>
            public long NumberingId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public long DistrictId { get; set; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int SeqNo { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 採番ID</summary>
                /// <value>採番ID</value>
                public long NumberingId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pNumberingId)
                {
                    NumberingId = pNumberingId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.NumberingId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public PtPartsNoNumberingEntity GetEntity(long pNumberingId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pNumberingId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<PtPartsNoNumberingEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pNumberingId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pNumberingId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 在庫確定管理データ
        /// </summary>
        public class PtStockComfirmEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PtStockComfirmEntity()
            {
                TableName = "pt_stock_confirm";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 確定管理id</summary>
            /// <value>確定管理id</value>
            public long StockConfirmId { get; set; }
            /// <summary>Gets or sets 対象年月</summary>
            /// <value>対象年月</value>
            public DateTime TargetMonth { get; set; }
            /// <summary>Gets or sets 工場id</summary>
            /// <value>工場id</value>
            public long FactoryId { get; set; }
            /// <summary>Gets or sets 職種id</summary>
            /// <value>職種id</value>
            public long? PartsJobId { get; set; }
            /// <summary>Gets or sets 実行日時</summary>
            /// <value>実行日時</value>
            public DateTime ExecutionDatetime { get; set; }
            /// <summary>Gets or sets 実行ユーザーid</summary>
            /// <value>実行ユーザーid</value>
            public string ExecutionUserId { get; set; }
            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 確定管理id</summary>
                /// <value>確定管理id</value>
                public long StockConfirmId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pStockConfirmId)
                {
                    StockConfirmId = pStockConfirmId;
                }
            }
            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.StockConfirmId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public PtStockComfirmEntity GetEntity(long pStockConfirmId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pStockConfirmId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<PtStockComfirmEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pStockConfirmId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pStockConfirmId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 確定在庫データ
        /// </summary>
        public class PtFixedStockEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PtFixedStockEntity()
            {
                TableName = "pt_fixed_stock";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 在庫id</summary>
            /// <value>在庫id</value>
            public long StockId { get; set; }
            /// <summary>Gets or sets 対象年月</summary>
            /// <value>対象年月</value>
            public DateTime TargetMonth { get; set; }
            /// <summary>Gets or sets 予備品id</summary>
            /// <value>予備品id</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets ロット管理id</summary>
            /// <value>ロット管理id</value>
            public long LotControlId { get; set; }
            /// <summary>Gets or sets 在庫管理id</summary>
            /// <value>在庫管理id</value>
            public long InventoryControlId { get; set; }
            /// <summary>Gets or sets 数量管理単位ID</summary>
            /// <value>数量管理単位ID</value>
            public long UnitStructureId { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public decimal UnitPrice { get; set; }
            /// <summary>Gets or sets 金額管理単位ID</summary>
            /// <value>金額管理単位ID</value>
            public long CurrencyStructureId { get; set; }
            /// <summary>Gets or sets 入庫数</summary>
            /// <value>入庫数</value>
            public decimal StorageQuantity { get; set; }
            /// <summary>Gets or sets 入庫金額</summary>
            /// <value>入庫金額</value>
            public decimal StorageAmount { get; set; }
            /// <summary>Gets or sets 出庫数</summary>
            /// <value>出庫数</value>
            public decimal ShippingQuantity { get; set; }
            /// <summary>Gets or sets 出庫金額</summary>
            /// <value>出庫金額</value>
            public decimal ShippingAmount { get; set; }
            /// <summary>Gets or sets 末在庫数</summary>
            /// <value>末在庫数</value>
            public decimal InventoryQuantity { get; set; }
            /// <summary>Gets or sets 末在庫金額</summary>
            /// <value>末在庫金額</value>
            public decimal InventoryAmount { get; set; }
            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 在庫id</summary>
                /// <value>在庫id</value>
                public long StockId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pStockId)
                {
                    StockId = pStockId;
                }
            }
            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.StockId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public PtFixedStockEntity GetEntity(long pStockId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pStockId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<PtFixedStockEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pStockId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pStockId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// ユーザ所属マスタ
        /// </summary>
        public class MsUserBelongEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsUserBelongEntity()
            {
                TableName = "ms_user_belong";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ユーザID</summary>
            /// <value>ユーザID</value>
            public int UserId { get; set; }
            /// <summary>Gets or sets 場所階層ID(工場)</summary>
            /// <value>場所階層ID(工場)</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 本務フラグ</summary>
            /// <value>本務フラグ</value>
            public bool DutyFlg { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ユーザID</summary>
                /// <value>ユーザID</value>
                public int UserId { get; set; }
                /// <summary>Gets or sets 場所階層ID(工場)</summary>
                /// <value>場所階層ID(工場)</value>
                public int LocationStructureId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pUserId, int pLocationStructureId)
                {
                    UserId = pUserId;
                    LocationStructureId = pLocationStructureId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.UserId, this.LocationStructureId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsUserBelongEntity GetEntity(int pUserId, int pLocationStructureId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pUserId, pLocationStructureId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsUserBelongEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pUserId, int pLocationStructureId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pUserId, pLocationStructureId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }

            /// <summary>
            /// ユーザIDを指定して本務のレコードを取得
            /// </summary>
            /// <param name="pUserId">ユーザID</param>
            /// <param name="db">DB接続</param>
            /// <returns>対象のレコード</returns>
            public MsUserBelongEntity GetUserDutyEntity(int pUserId, ComDB db)
            {
                MsUserBelongEntity condition = new();
                condition.UserId = pUserId;
                string getEntitySql = "SELECT TOP 1 * FROM ms_user_belong WHERE user_id = @UserId AND duty_flg = 1 AND delete_flg = 0 ORDER BY location_structure_id";
                return db.GetEntityByDataClass<MsUserBelongEntity>(getEntitySql, condition);
            }

            /// <summary>
            /// ユーザIDを指定して本務のレコードを取得
            /// </summary>
            /// <param name="pUserId">ユーザID</param>
            /// <param name="db">DB接続</param>
            /// <returns>対象のレコード</returns>
            public MsUserBelongEntity GetUserDutyEntity(string pUserId, ComDB db)
            {
                // ユーザIDが文字列の場合数値に変換して取得
                int userId = int.Parse(pUserId);
                return GetUserDutyEntity(userId, db);
            }
        }

        /// <summary>
        /// 出力テンプレート
        /// </summary>
        public class MsOutputTemplateEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsOutputTemplateEntity()
            {
                TableName = "ms_output_template";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 帳票ID</summary>
            /// <value>帳票ID</value>
            public string ReportId { get; set; }
            /// <summary>Gets or sets テンプレートID</summary>
            /// <value>テンプレートID</value>
            public int TemplateId { get; set; }
            /// <summary>Gets or sets テンプレート名</summary>
            /// <value>テンプレート名</value>
            public string TemplateName { get; set; }
            /// <summary>Gets or sets テンプレートファイルパス</summary>
            /// <value>テンプレートファイルパス</value>
            public string TemplateFilePath { get; set; }
            /// <summary>Gets or sets テンプレートファイル名</summary>
            /// <value>テンプレートファイル名</value>
            public string TemplateFileName { get; set; }
            /// <summary>Gets or sets 使用ユーザID</summary>
            /// <value>使用ユーザID</value>
            public int? UseUserId { get; set; }
            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int FactoryId { get; set; }
                /// <summary>Gets or sets 帳票ID</summary>
                /// <value>帳票ID</value>
                public string ReportId { get; set; }
                /// <summary>Gets or sets テンプレートID</summary>
                /// <value>テンプレートID</value>
                public int TemplateId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pFactoryId, string pReportId, int pTemplateId)
                {
                    FactoryId = pFactoryId;
                    ReportId = pReportId;
                    TemplateId = pTemplateId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.FactoryId, this.ReportId, this.TemplateId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsOutputTemplateEntity GetEntity(int pFactoryId, string pReportId, int pTemplateId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pTemplateId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsOutputTemplateEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pFactoryId, string pReportId, int pTemplateId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pFactoryId, pReportId, pTemplateId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 棚卸データ
        /// </summary>
        public class PtInventoryEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PtInventoryEntity()
            {
                TableName = "pt_inventory";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 棚卸ID</summary>
            /// <value>棚卸ID</value>
            public long InventoryId { get; set; }
            /// <summary>Gets or sets 対象年月</summary>
            /// <value>対象年月</value>
            public DateTime TargetMonth { get; set; }
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public long PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public long OldNewStructureId { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long AccountStructureId { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public decimal StockQuantity { get; set; }
            /// <summary>Gets or sets 棚卸数</summary>
            /// <value>棚卸数</value>
            public decimal? InventoryQuantity { get; set; }
            /// <summary>Gets or sets 棚卸取込値</summary>
            /// <value>棚卸取込値</value>
            public decimal? TmpInventoryQuantity { get; set; }
            /// <summary>Gets or sets 数量管理単位ID</summary>
            /// <value>数量管理単位ID</value>
            public long? UnitStructureId { get; set; }
            /// <summary>Gets or sets 金額管理単位ID</summary>
            /// <value>金額管理単位ID</value>
            public long? CurrencyStructureId { get; set; }
            /// <summary>Gets or sets 棚卸準備日時</summary>
            /// <value>棚卸準備日時</value>
            public DateTime? PreparationDatetime { get; set; }
            /// <summary>Gets or sets 棚卸実施日時</summary>
            /// <value>棚卸実施日時</value>
            public DateTime? InventoryDatetime { get; set; }
            /// <summary>Gets or sets 棚卸実施日時取込値</summary>
            /// <value>棚卸実施日時取込値</value>
            public DateTime? TempInventoryDatetime { get; set; }
            /// <summary>Gets or sets 棚卸調整日時</summary>
            /// <value>棚卸調整日時</value>
            public DateTime? DifferenceDatetime { get; set; }
            /// <summary>Gets or sets 棚卸確定日時</summary>
            /// <value>棚卸確定日時</value>
            public DateTime? FixedDatetime { get; set; }
            /// <summary>Gets or sets 作成区分</summary>
            /// <value>作成区分</value>
            public long? CreationDivisionStructureId { get; set; }
            /// <summary>Gets or sets RFIDタグ</summary>
            /// <value>RFIDタグ</value>
            public string RftagId { get; set; }
            /// <summary>Gets or sets RFIDタグ取込値</summary>
            /// <value>RFIDタグ取込値</value>
            public string TempRftagId { get; set; }
            /// <summary>Gets or sets 作業者</summary>
            /// <value>作業者</value>
            public string WorkUserName { get; set; }
            /// <summary>Gets or sets 作業者取込値</summary>
            /// <value>作業者取込値</value>
            public string TempWorkUserName { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 棚卸ID</summary>
                /// <value>棚卸ID</value>
                public long InventoryId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pInventoryId)
                {
                    InventoryId = pInventoryId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InventoryId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public PtInventoryEntity GetEntity(long pInventoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pInventoryId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<PtInventoryEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pInventoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pInventoryId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }
        
        /// <summary>
        /// 棚差調整データ
        /// </summary>
        public class PtInventoryDifferenceEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PtInventoryDifferenceEntity()
            {
                TableName = "pt_inventory_difference";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 棚差調整ID</summary>
            /// <value>棚差調整ID</value>
            public long InventoryDifferenceId { get; set; }
            /// <summary>Gets or sets 棚卸ID</summary>
            /// <value>棚卸ID</value>
            public long InventoryId { get; set; }
            /// <summary>Gets or sets 受払履歴ID</summary>
            /// <value>受払履歴ID</value>
            public long InoutHistoryId { get; set; }
            /// <summary>Gets or sets 受払区分</summary>
            /// <value>受払区分</value>
            public long? InoutDivisionStructureId { get; set; }
            /// <summary>Gets or sets 作業区分</summary>
            /// <value>作業区分</value>
            public long? WorkDivisionStructureId { get; set; }
            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public long? WorkNo { get; set; }
            /// <summary>Gets or sets ロット管理ID</summary>
            /// <value>ロット管理ID</value>
            public long? LotControlId { get; set; }
            /// <summary>Gets or sets 在庫管理ID</summary>
            /// <value>在庫管理ID</value>
            public long? InventoryControlId { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long? DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long? AccountStructureId { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 管理№</summary>
            /// <value>管理№</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 受払日時</summary>
            /// <value>受払日時</value>
            public DateTime? InoutDatetime { get; set; }
            /// <summary>Gets or sets 受払数</summary>
            /// <value>受払数</value>
            public decimal? InoutQuantity { get; set; }
            /// <summary>Gets or sets 出庫区分</summary>
            /// <value>出庫区分</value>
            public long? CreationDivisionStructureId { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public decimal? UnitPrice { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 棚差調整ID</summary>
                /// <value>棚差調整ID</value>
                public long InventoryDifferenceId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pInventoryDifferenceId)
                {
                    InventoryDifferenceId = pInventoryDifferenceId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InventoryDifferenceId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public PtInventoryDifferenceEntity GetEntity(long pInventoryDifferenceId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pInventoryDifferenceId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<PtInventoryDifferenceEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pInventoryDifferenceId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pInventoryDifferenceId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// アイテムマスタ
        /// </summary>
        public class MsItemEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsItemEntity()
            {
                TableName = "ms_item";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets アイテムID</summary>
            /// <value>アイテムID</value>
            public int ItemId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }
            /// <summary>Gets or sets アイテム翻訳ID</summary>
            /// <value>アイテム翻訳ID</value>
            public int? ItemTranslationId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets アイテムID</summary>
                /// <value>アイテムID</value>
                public int ItemId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pItemId)
                {
                    ItemId = pItemId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.ItemId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsItemEntity GetEntity(int pItemId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pItemId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsItemEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pItemId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pItemId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }


        /// <summary>
        /// 翻訳マスタ
        /// </summary>
        public class MsTranslationEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsTranslationEntity()
            {
                TableName = "ms_translation";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 場所階層ID(工場)</summary>
            /// <value>場所階層ID(工場)</value>
            public int LocationStructureId { get; set; }
            /// <summary>Gets or sets 翻訳ID</summary>
            /// <value>翻訳ID</value>
            public int TranslationId { get; set; }
            /// <summary>Gets or sets 翻訳文字列</summary>
            /// <value>翻訳文字列</value>
            public string TranslationText { get; set; }
            /// <summary>Gets or sets 翻訳項目説明</summary>
            /// <value>翻訳項目説明</value>
            public string TranslationItemDescription { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 場所階層ID(工場)</summary>
                /// <value>場所階層ID(工場)</value>
                public int LocationStructureId { get; set; }
                /// <summary>Gets or sets 翻訳ID</summary>
                /// <value>翻訳ID</value>
                public int TranslationId { get; set; }
                /// <summary>Gets or sets 言語ID</summary>
                /// <value>言語ID</value>
                public string LanguageId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pLocationStructureId, int pTranslationId, string pLanguageId)
                {
                    LocationStructureId = pLocationStructureId;
                    TranslationId = pTranslationId;
                    LanguageId = pLanguageId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.LocationStructureId, this.TranslationId, this.LanguageId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsTranslationEntity GetEntity(int pLocationStructureId, int pTranslationId, string pLanguageId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pLocationStructureId, pTranslationId, pLanguageId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsTranslationEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pLocationStructureId, int pTranslationId, string pLanguageId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pLocationStructureId, pTranslationId, pLanguageId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 工場別未使用標準アイテムマスタ
        /// </summary>
        public class MsStructureUnusedEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsStructureUnusedEntity()
            {
                TableName = "ms_structure_unused";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int StructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 構成ID</summary>
                /// <value>構成ID</value>
                public int StructureId { get; set; }
                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int FactoryId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pStructureId, int pFactoryId)
                {
                    StructureId = pStructureId;
                    FactoryId = pFactoryId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.StructureId, this.FactoryId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsStructureUnusedEntity GetEntity(int pStructureId, int pFactoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pStructureId, pFactoryId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsStructureUnusedEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pStructureId, int pFactoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pStructureId, pFactoryId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 工場別アイテム表示順マスタ
        /// </summary>
        public class MsStructureOrderEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsStructureOrderEntity()
            {
                TableName = "ms_structure_order";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 構成ID</summary>
            /// <value>構成ID</value>
            public int StructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 構成グループID</summary>
            /// <value>構成グループID</value>
            public int? StructureGroupId { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DisplayOrder { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 構成ID</summary>
                /// <value>構成ID</value>
                public int StructureId { get; set; }
                /// <summary>Gets or sets 工場ID</summary>
                /// <value>工場ID</value>
                public int FactoryId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pStructureId, int pFactoryId)
                {
                    StructureId = pStructureId;
                    FactoryId = pFactoryId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.StructureId, this.FactoryId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsStructureOrderEntity GetEntity(int pStructureId, int pFactoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pStructureId, pFactoryId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsStructureOrderEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pStructureId, int pFactoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pStructureId, pFactoryId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 受払履歴
        /// </summary>
        public class PtInoutHistoryEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PtInoutHistoryEntity()
            {
                TableName = "pt_inout_history";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 受払履歴ID</summary>
            /// <value>受払履歴ID</value>
            public long InoutHistoryId { get; set; }
            /// <summary>Gets or sets 受払区分</summary>
            /// <value>受払区分</value>
            public long InoutDivisionStructureId { get; set; }
            /// <summary>Gets or sets 作業区分</summary>
            /// <value>作業区分</value>
            public long WorkDivisionStructureId { get; set; }
            /// <summary>Gets or sets 作業No</summary>
            /// <value>作業No</value>
            public long WorkNo { get; set; }
            /// <summary>Gets or sets ロット管理ID</summary>
            /// <value>ロット管理ID</value>
            public long LotControlId { get; set; }
            /// <summary>Gets or sets 在庫管理ID</summary>
            /// <value>在庫管理ID</value>
            public long InventoryControlId { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long AccountStructureId { get; set; }
            /// <summary>Gets or sets 管理区分</summary>
            /// <value>管理区分</value>
            public string ManagementDivision { get; set; }
            /// <summary>Gets or sets 管理№</summary>
            /// <value>管理№</value>
            public string ManagementNo { get; set; }
            /// <summary>Gets or sets 受払日時</summary>
            /// <value>受払日時</value>
            public DateTime InoutDatetime { get; set; }
            /// <summary>Gets or sets 受払数</summary>
            /// <value>受払数</value>
            public decimal InoutQuantity { get; set; }
            /// <summary>Gets or sets 棚卸確定日時</summary>
            /// <value>棚卸確定日時</value>
            public DateTime InventoryDatetime { get; set; }
            /// <summary>Gets or sets 出庫区分</summary>
            /// <value>出庫区分</value>
            public long ShippingDivisionStructureId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 受払履歴ID</summary>
                /// <value>受払履歴ID</value>
                public long InoutHistoryId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pInoutHistoryId)
                {
                    InoutHistoryId = pInoutHistoryId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InoutHistoryId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public PtInoutHistoryEntity GetEntity(long pInoutHistoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pInoutHistoryId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<PtInoutHistoryEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pInoutHistoryId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pInoutHistoryId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
            /// <summary>
            /// 作業Noから該当するレコードを取得するSQL
            /// </summary>
            /// <param name="workNo">作業No</param>
            /// <param name="db">DB接続</param>
            /// <returns>作業Noで取得した受払履歴</returns>
            public static IList<PtInoutHistoryEntity> GetListByWorkNo(long workNo, ComDB db)
            {
                PtInoutHistoryEntity condition = new();
                condition.WorkNo = workNo;
                string getListSql = "SELECT * FROM pt_inout_history WHERE work_no = @WorkNo";
                return db.GetListByDataClass<PtInoutHistoryEntity>(getListSql, condition);
            }
        }
        /// <summary>
        /// ロット情報
        /// </summary>
        public class PtLotEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PtLotEntity()
            {
                TableName = "pt_lot";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets ロット管理ID</summary>
            /// <value>ロット管理ID</value>
            public long LotControlId { get; set; }
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets ロット№</summary>
            /// <value>ロット№</value>
            public long LotNo { get; set; }
            /// <summary>Gets or sets 新旧区分ID</summary>
            /// <value>新旧区分ID</value>
            public long OldNewStructureId { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
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
            /// <summary>Gets or sets 数量管理単位ID</summary>
            /// <value>数量管理単位ID</value>
            public long UnitStructureId { get; set; }
            /// <summary>Gets or sets 入庫単価</summary>
            /// <value>入庫単価</value>
            public decimal UnitPrice { get; set; }
            /// <summary>Gets or sets 金額管理単位ID</summary>
            /// <value>金額管理単位ID</value>
            public long CurrencyStructureId { get; set; }
            /// <summary>Gets or sets 仕入先ID</summary>
            /// <value>仕入先ID</value>
            public long? VenderStructureId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets ロット管理ID</summary>
                /// <value>ロット管理ID</value>
                public long LotControlId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pLotControlId)
                {
                    LotControlId = pLotControlId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.LotControlId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public PtLotEntity GetEntity(long pLotControlId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pLotControlId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<PtLotEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pLotControlId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pLotControlId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }
        /// <summary>
        /// 在庫データ
        /// </summary>
        public class PtLocationStockEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PtLocationStockEntity()
            {
                TableName = "pt_location_stock";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 在庫管理ID</summary>
            /// <value>在庫管理ID</value>
            public long InventoryControlId { get; set; }
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets ロット管理ID</summary>
            /// <value>ロット管理ID</value>
            public long LotControlId { get; set; }
            /// <summary>Gets or sets 棚ID</summary>
            /// <value>棚ID</value>
            public long PartsLocationId { get; set; }
            /// <summary>Gets or sets 棚枝番</summary>
            /// <value>棚枝番</value>
            public string PartsLocationDetailNo { get; set; }
            /// <summary>Gets or sets 在庫数</summary>
            /// <value>在庫数</value>
            public decimal StockQuantity { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 在庫管理ID</summary>
                /// <value>在庫管理ID</value>
                public long InventoryControlId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pInventoryControlId)
                {
                    InventoryControlId = pInventoryControlId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.InventoryControlId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public PtLocationStockEntity GetEntity(long pInventoryControlId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pInventoryControlId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<PtLocationStockEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pInventoryControlId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pInventoryControlId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }
        /// <summary>
        /// 仕様項目マスタ
        /// </summary>
        public class MsSpecEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsSpecEntity()
            {
                TableName = "ms_spec";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 仕様項目ID</summary>
            /// <value>仕様項目ID</value>
            public int SpecId { get; set; }
            /// <summary>Gets or sets 仕様項目入力形式ID</summary>
            /// <value>仕様項目入力形式ID</value>
            public int? SpecTypeId { get; set; }
            /// <summary>Gets or sets 仕様単位種別ID</summary>
            /// <value>仕様単位種別ID</value>
            public int? SpecUnitTypeId { get; set; }
            /// <summary>Gets or sets 仕様単位ID</summary>
            /// <value>仕様単位ID</value>
            public int? SpecUnitId { get; set; }
            /// <summary>Gets or sets 設定値(数値)小数点以下桁数</summary>
            /// <value>設定値(数値)小数点以下桁数</value>
            public int? SpecNumDecimalPlaces { get; set; }
            /// <summary>Gets or sets 翻訳ID</summary>
            /// <value>翻訳ID</value>
            public int? TranslationId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 仕様項目ID</summary>
                /// <value>仕様項目ID</value>
                public int SpecId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pSpecId)
                {
                    SpecId = pSpecId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.SpecId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsSpecEntity GetEntity(int pSpecId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pSpecId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsSpecEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pSpecId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pSpecId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }
        /// <summary>
        /// 機種別仕様関連付マスタ
        /// </summary>
        public class MsMachineSpecRelationEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MsMachineSpecRelationEntity()
            {
                TableName = "ms_machine_spec_relation";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機種別仕様関連付ID</summary>
            /// <value>機種別仕様関連付ID</value>
            public int MachineSpecRelationId { get; set; }
            /// <summary>Gets or sets 機能場所階層ID(工場ID)</summary>
            /// <value>機能場所階層ID(工場ID)</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
            /// <summary>Gets or sets 仕様項目ID</summary>
            /// <value>仕様項目ID</value>
            public int? SpecId { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? DisplayOrder { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機種別仕様関連付ID</summary>
                /// <value>機種別仕様関連付ID</value>
                public int MachineSpecRelationId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(int pMachineSpecRelationId)
                {
                    MachineSpecRelationId = pMachineSpecRelationId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MachineSpecRelationId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public MsMachineSpecRelationEntity GetEntity(int pMachineSpecRelationId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMachineSpecRelationId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<MsMachineSpecRelationEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(int pMachineSpecRelationId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pMachineSpecRelationId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 変更管理
        /// </summary>
        public class HmHistoryManagementEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public HmHistoryManagementEntity()
            {
                TableName = "hm_history_management";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            /// <summary>Gets or sets 申請状況ID</summary>
            /// <value>申請状況ID</value>
            public int ApplicationStatusId { get; set; }
            /// <summary>Gets or sets 申請区分ID</summary>
            /// <value>申請区分ID</value>
            public int ApplicationDivisionId { get; set; }
            /// <summary>Gets or sets 申請機能ID</summary>
            /// <value>申請機能ID</value>
            public int? ApplicationConductId { get; set; }
            /// <summary>Gets or sets 申請データキーID</summary>
            /// <value>申請データキーID</value>
            public long KeyId { get; set; }
            /// <summary>Gets or sets 申請データキーID</summary>
            /// <value>申請データキーID</value>
            public int FactoryId { get; set; }
            /// <summary>Gets or sets 申請者ID</summary>
            /// <value>申請者ID</value>
            public int? ApplicationUserId { get; set; }
            /// <summary>Gets or sets 申請者名称</summary>
            /// <value>申請者名称</value>
            public string ApplicationUserName { get; set; }
            /// <summary>Gets or sets 承認者ID</summary>
            /// <value>承認者ID</value>
            public int? ApprovalUserId { get; set; }
            /// <summary>Gets or sets 承認者名称</summary>
            /// <value>承認者名称</value>
            public string ApprovalUserName { get; set; }
            /// <summary>Gets or sets 申請日</summary>
            /// <value>申請日</value>
            public DateTime? ApplicationDate { get; set; }
            /// <summary>Gets or sets 承認日</summary>
            /// <value>承認日</value>
            public DateTime? ApprovalDate { get; set; }
            /// <summary>Gets or sets 申請理由</summary>
            /// <value>申請理由</value>
            public string ApplicationReason { get; set; }
            /// <summary>Gets or sets 否認理由</summary>
            /// <value>否認理由</value>
            public string RejectionReason { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 変更管理ID</summary>
                /// <value>変更管理ID</value>
                public long HistoryManagementId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHistoryManagementId)
                {
                    HistoryManagementId = pHistoryManagementId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HistoryManagementId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public HmHistoryManagementEntity GetEntity(long pHistoryManagementId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryManagementId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<HmHistoryManagementEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHistoryManagementId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHistoryManagementId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 機番情報変更管理
        /// </summary>
        public class HmMcMachineEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public HmMcMachineEntity()
            {
                TableName = "hm_mc_machine";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機番情報変更管理ID</summary>
            /// <value>機番情報変更管理ID</value>
            public long HmMachineId { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            /// <summary>Gets or sets 処理区分</summary>
            /// <value>処理区分</value>
            public int ExecutionDivision { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long MachineId { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? LocationDistrictStructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? LocationFactoryStructureId { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? LocationPlantStructureId { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? LocationSeriesStructureId { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? LocationStrokeStructureId { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? LocationFacilityStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobKindStructureId { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? JobLargeClassficationStructureId { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? JobMiddleClassficationStructureId { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? JobSmallClassficationStructureId { get; set; }
            /// <summary>Gets or sets 機器番号</summary>
            /// <value>機器番号</value>
            public string MachineNo { get; set; }
            /// <summary>Gets or sets 機器名称</summary>
            /// <value>機器名称</value>
            public string MachineName { get; set; }
            /// <summary>Gets or sets 設置場所</summary>
            /// <value>設置場所</value>
            public string InstallationLocation { get; set; }
            /// <summary>Gets or sets 設置台数</summary>
            /// <value>設置台数</value>
            public decimal? NumberOfInstallation { get; set; }
            /// <summary>Gets or sets 機器レベル</summary>
            /// <value>機器レベル</value>
            public int? EquipmentLevelStructureId { get; set; }
            /// <summary>Gets or sets 設置日</summary>
            /// <value>設置日</value>
            public DateTime? DateOfInstallation { get; set; }
            /// <summary>Gets or sets 重要度</summary>
            /// <value>重要度</value>
            public int? ImportanceStructureId { get; set; }
            /// <summary>Gets or sets 保全方式</summary>
            /// <value>保全方式</value>
            public int? ConservationStructureId { get; set; }
            /// <summary>Gets or sets 機番メモ</summary>
            /// <value>機番メモ</value>
            public string MachineNote { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機番情報変更管理ID</summary>
                /// <value>機番情報変更管理ID</value>
                public long HmMachineId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHmMachineId)
                {
                    HmMachineId = pHmMachineId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HmMachineId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public HmMcMachineEntity GetEntity(long pHmMachineId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmMachineId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<HmMcMachineEntity>(getEntitySql);
            }

            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHmMachineId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmMachineId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 機器情報変更管理
        /// </summary>
        public class HmMcEquipmentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public HmMcEquipmentEntity()
            {
                TableName = "hm_mc_equipment";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機器情報変更管理ID</summary>
            /// <value>機器情報変更管理ID</value>
            public long HmEquipmentId { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            /// <summary>Gets or sets 機器ID</summary>
            /// <value>機器ID</value>
            public long EquipmentId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 循環対象</summary>
            /// <value>循環対象</value>
            public bool CirculationTargetFlg { get; set; }
            /// <summary>Gets or sets メーカー</summary>
            /// <value>メーカー</value>
            public int? ManufacturerStructureId { get; set; }
            /// <summary>Gets or sets メーカー型式</summary>
            /// <value>メーカー型式</value>
            public string ManufacturerType { get; set; }
            /// <summary>Gets or sets 型式コード</summary>
            /// <value>型式コード</value>
            public string ModelNo { get; set; }
            /// <summary>Gets or sets シリアル番号</summary>
            /// <value>シリアル番号</value>
            public string SerialNo { get; set; }
            /// <summary>Gets or sets 製造日</summary>
            /// <value>製造日</value>
            public DateTime? DateOfManufacture { get; set; }
            /// <summary>Gets or sets 納期</summary>
            /// <value>納期</value>
            public int? DeliveryDate { get; set; }
            /// <summary>Gets or sets 機器メモ</summary>
            /// <value>機器メモ</value>
            public string EquipmentNote { get; set; }
            /// <summary>Gets or sets 使用区分</summary>
            /// <value>使用区分</value>
            public int? UseSegmentStructureId { get; set; }
            /// <summary>Gets or sets 固定資産番号</summary>
            /// <value>固定資産番号</value>
            public string FixedAssetNo { get; set; }
            /// <summary>Gets or sets 点検種別毎管理</summary>
            /// <value>点検種別毎管理</value>
            public bool MaintainanceKindManage { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機器情報変更管理ID</summary>
                /// <value>機器情報変更管理ID</value>
                public long HmEquipmentId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHmEquipmentId)
                {
                    HmEquipmentId = pHmEquipmentId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HmEquipmentId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public HmMcEquipmentEntity GetEntity(long pHmEquipmentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmEquipmentId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<HmMcEquipmentEntity>(getEntitySql);
            }

            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHmEquipmentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmEquipmentId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 長計件名変更管理
        /// </summary>
        public class HmLnLongPlanEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public HmLnLongPlanEntity()
            {
                TableName = "hm_ln_long_plan";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 長計件名変更管理ID</summary>
            /// <value>長計件名変更管理ID</value>
            public long HmLongPlanId { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            /// <summary>Gets or sets 長期計画件名ID</summary>
            /// <value>長期計画件名ID</value>
            public long LongPlanId { get; set; }
            /// <summary>Gets or sets 実行処理区分</summary>
            /// <value>実行処理区分</value>
            public int ExecutionDivision { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string Subject { get; set; }
            /// <summary>Gets or sets 機能場所階層ID</summary>
            /// <value>機能場所階層ID</value>
            public int? LocationStructureId { get; set; }
            /// <summary>Gets or sets 地区ID</summary>
            /// <value>地区ID</value>
            public int? LocationDistrictStructureId { get; set; }
            /// <summary>Gets or sets 工場ID</summary>
            /// <value>工場ID</value>
            public int? LocationFactoryStructureId { get; set; }
            /// <summary>Gets or sets プラントID</summary>
            /// <value>プラントID</value>
            public int? LocationPlantStructureId { get; set; }
            /// <summary>Gets or sets 系列ID</summary>
            /// <value>系列ID</value>
            public int? LocationSeriesStructureId { get; set; }
            /// <summary>Gets or sets 工程ID</summary>
            /// <value>工程ID</value>
            public int? LocationStrokeStructureId { get; set; }
            /// <summary>Gets or sets 設備ID</summary>
            /// <value>設備ID</value>
            public int? LocationFacilityStructureId { get; set; }
            /// <summary>Gets or sets 職種機種階層ID</summary>
            /// <value>職種機種階層ID</value>
            public int? JobStructureId { get; set; }
            /// <summary>Gets or sets 職種ID</summary>
            /// <value>職種ID</value>
            public int? JobKindStructureId { get; set; }
            /// <summary>Gets or sets 機種大分類ID</summary>
            /// <value>機種大分類ID</value>
            public int? JobLargeClassficationStructureId { get; set; }
            /// <summary>Gets or sets 機種中分類ID</summary>
            /// <value>機種中分類ID</value>
            public int? JobMiddleClassficationStructureId { get; set; }
            /// <summary>Gets or sets 機種小分類ID</summary>
            /// <value>機種小分類ID</value>
            public int? JobSmallClassficationStructureId { get; set; }
            /// <summary>Gets or sets 件名メモ</summary>
            /// <value>件名メモ</value>
            public string SubjectNote { get; set; }
            /// <summary>Gets or sets 担当者</summary>
            /// <value>担当者</value>
            public int? PersonId { get; set; }
            /// <summary>Gets or sets 担当者名</summary>
            /// <value>担当者名</value>
            public string PersonName { get; set; }
            /// <summary>Gets or sets 作業項目</summary>
            /// <value>作業項目</value>
            public int? WorkItemStructureId { get; set; }
            /// <summary>Gets or sets 予算管理区分</summary>
            /// <value>予算管理区分</value>
            public int? BudgetManagementStructureId { get; set; }
            /// <summary>Gets or sets 予算性格区分</summary>
            /// <value>予算性格区分</value>
            public int? BudgetPersonalityStructureId { get; set; }
            /// <summary>Gets or sets 保全時期</summary>
            /// <value>保全時期</value>
            public int? MaintenanceSeasonStructureId { get; set; }
            /// <summary>Gets or sets 目的区分</summary>
            /// <value>目的区分</value>
            public int? PurposeStructureId { get; set; }
            /// <summary>Gets or sets 作業区分</summary>
            /// <value>作業区分</value>
            public int? WorkClassStructureId { get; set; }
            /// <summary>Gets or sets 処置区分</summary>
            /// <value>処置区分</value>
            public int? TreatmentStructureId { get; set; }
            /// <summary>Gets or sets 設備区分</summary>
            /// <value>設備区分</value>
            public int? FacilityStructureId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 長計件名変更管理ID</summary>
                /// <value>長計件名変更管理ID</value>
                public long HmLongPlanId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHmLongPlanId)
                {
                    HmLongPlanId = pHmLongPlanId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HmLongPlanId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public HmLnLongPlanEntity GetEntity(long pHmLongPlanId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmLongPlanId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<HmLnLongPlanEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHmLongPlanId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmLongPlanId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 適用法規情報変更管理
        /// </summary>
        public class HmMcApplicableLawsEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public HmMcApplicableLawsEntity()
            {
                TableName = "hm_mc_applicable_laws";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 適用法規変更管理ID</summary>
            /// <value>適用法規変更管理ID</value>
            public long HmApplicableLawsId { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            /// <summary>Gets or sets 適用法規ID</summary>
            /// <value>適用法規ID</value>
            public long ApplicableLawsId { get; set; }
            /// <summary>Gets or sets 適用法規アイテムID</summary>
            /// <value>適用法規アイテムID</value>
            public int? ApplicableLawsStructureId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }


            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 適用法規変更管理ID</summary>
                /// <value>適用法規変更管理ID</value>
                public long HmApplicableLawsId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHmApplicableLawsId)
                {
                    HmApplicableLawsId = pHmApplicableLawsId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HmApplicableLawsId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public HmMcApplicableLawsEntity GetEntity(long pHmApplicableLawsId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmApplicableLawsId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<HmMcApplicableLawsEntity>(getEntitySql);
            }

            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHmApplicableLawsId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmApplicableLawsId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 機器別管理基準部位変更管理
        /// </summary>
        public class HmMcManagementStandardsComponentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public HmMcManagementStandardsComponentEntity()
            {
                TableName = "hm_mc_management_standards_component";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機器別管理基準部位変更管理ID</summary>
            /// <value>機器別管理基準部位変更管理ID</value>
            public long HmManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            // <summary>Gets or sets 機器別管理基準部位ID</summary>
            /// <value>機器別管理基準部位ID</value>
            public long ManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 機番ID</summary>
            /// <value>機番ID</value>
            public long? MachineId { get; set; }
            /// <summary>Gets or sets 部位ID</summary>
            /// <value>部位ID</value>
            public int? InspectionSiteStructureId { get; set; }
            /// <summary>Gets or sets 機器別管理基準フラグ</summary>
            /// <value>機器別管理基準フラグ</value>
            public bool? IsManagementStandardConponent { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機器別管理基準部位変更管理ID</summary>
                /// <value>機器別管理基準部位変更管理ID</value>
                public long HmManagementStandardsComponentId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHmManagementStandardsComponentId)
                {
                    HmManagementStandardsComponentId = pHmManagementStandardsComponentId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HmManagementStandardsComponentId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public HmMcManagementStandardsComponentEntity GetEntity(long pHmManagementStandardsComponentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmManagementStandardsComponentId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<HmMcManagementStandardsComponentEntity>(getEntitySql);
            }

            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHmManagementStandardsComponentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmManagementStandardsComponentId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 機器別管理基準内容変更管理
        /// </summary>
        public class HmMcManagementStandardsContentEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public HmMcManagementStandardsContentEntity()
            {
                TableName = "hm_mc_management_standards_content";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 機器別管理基準内容変更管理ID</summary>
            /// <value>機器別管理基準内容変更管理ID</value>
            public long HmManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 処理区分</summary>
            /// <value>処理区分</value>
            public int ExecutionDivision { get; set; }
            /// <summary>Gets or sets 機器別管理基準部位ID</summary>
            /// <value>機器別管理基準部位ID</value>
            public long? ManagementStandardsComponentId { get; set; }
            /// <summary>Gets or sets 点検内容ID</summary>
            /// <value>点検内容ID</value>
            public int? InspectionContentStructureId { get; set; }
            /// <summary>Gets or sets 部位重要度</summary>
            /// <value>部位重要度</value>
            public int? InspectionSiteImportanceStructureId { get; set; }
            /// <summary>Gets or sets 部位保全方式</summary>
            /// <value>部位保全方式</value>
            public int? InspectionSiteConservationStructureId { get; set; }
            /// <summary>Gets or sets 保全区分</summary>
            /// <value>保全区分</value>
            public int? MaintainanceDivision { get; set; }
            /// <summary>Gets or sets 点検種別</summary>
            /// <value>点検種別</value>
            public int? MaintainanceKindStructureId { get; set; }
            /// <summary>Gets or sets 予算金額</summary>
            /// <value>予算金額</value>
            public decimal? BudgetAmount { get; set; }
            /// <summary>Gets or sets 準備期間(日)</summary>
            /// <value>準備期間(日)</value>
            public int? PreparationPeriod { get; set; }
            /// <summary>Gets or sets 長計件名ID</summary>
            /// <value>長計件名ID</value>
            public long? LongPlanId { get; set; }
            /// <summary>Gets or sets 並び順</summary>
            /// <value>並び順</value>
            public int? OrderNo { get; set; }
            /// <summary>Gets or sets スケジュール管理基準ID</summary>
            /// <value>スケジュール管理基準ID</value>
            public int? ScheduleTypeStructureId { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 機器別管理基準内容変更管理ID</summary>
                /// <value>機器別管理基準内容変更管理ID</value>
                public long HmManagementStandardsContentId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHmManagementStandardsContentId)
                {
                    HmManagementStandardsContentId = pHmManagementStandardsContentId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HmManagementStandardsContentId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public HmMcManagementStandardsContentEntity GetEntity(long pHmManagementStandardsContentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmManagementStandardsContentId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<HmMcManagementStandardsContentEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHmManagementStandardsContentId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmManagementStandardsContentId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 保全スケジュール変更管理
        /// </summary>
        public class HmMcMaintainanceScheduleEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public HmMcMaintainanceScheduleEntity()
            {
                TableName = "hm_mc_maintainance_schedule";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets 保全スケジュール変更管理ID</summary>
            /// <value>保全スケジュール変更管理ID</value>
            public long HmMaintainanceScheduleId { get; set; }
            /// <summary>Gets or sets 変更管理ID</summary>
            /// <value>変更管理ID</value>
            public long HistoryManagementId { get; set; }
            /// <summary>Gets or sets 保全スケジュールID</summary>
            /// <value>保全スケジュールID</value>
            public long MaintainanceScheduleId { get; set; }
            /// <summary>Gets or sets 機器別管理基準内容ID</summary>
            /// <value>機器別管理基準内容ID</value>
            public long? ManagementStandardsContentId { get; set; }
            /// <summary>Gets or sets 周期ありフラグ</summary>
            /// <value>周期ありフラグ</value>
            public bool? IsCyclic { get; set; }
            /// <summary>Gets or sets 周期(年)</summary>
            /// <value>周期(年)</value>
            public int? CycleYear { get; set; }
            /// <summary>Gets or sets 周期(月)</summary>
            /// <value>周期(月)</value>
            public int? CycleMonth { get; set; }
            /// <summary>Gets or sets 周期(日)</summary>
            /// <value>周期(日)</value>
            public int? CycleDay { get; set; }
            /// <summary>Gets or sets 表示周期</summary>
            /// <value>表示周期</value>
            public string DispCycle { get; set; }
            /// <summary>Gets or sets 開始日</summary>
            /// <value>開始日</value>
            public DateTime? StartDate { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets 保全スケジュール変更管理ID</summary>
                /// <value>保全スケジュール変更管理ID</value>
                public long HmMaintainanceScheduleId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(long pHmMaintainanceScheduleId)
                {
                    HmMaintainanceScheduleId = pHmMaintainanceScheduleId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.HmMaintainanceScheduleId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public HmMcMaintainanceScheduleEntity GetEntity(long pHmMaintainanceScheduleId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmMaintainanceScheduleId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<HmMcMaintainanceScheduleEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(long pHmMaintainanceScheduleId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pHmMaintainanceScheduleId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }
        /// <summary>
        /// RFタグ予備品マスタ
        /// </summary>
        public class PtRftagPartsLinkEntity : CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PtRftagPartsLinkEntity()
            {
                TableName = "pt_rftag_parts_link";
            }
            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets RFタグ管理ID</summary>
            /// <value>RFタグ管理ID</value>
            public string RftagId { get; set; }
            /// <summary>Gets or sets 予備品ID</summary>
            /// <value>予備品ID</value>
            public long PartsId { get; set; }
            /// <summary>Gets or sets 部門ID</summary>
            /// <value>部門ID</value>
            public long DepartmentStructureId { get; set; }
            /// <summary>Gets or sets 勘定科目ID</summary>
            /// <value>勘定科目ID</value>
            public long AccountStructureId { get; set; }
            /// <summary>Gets or sets 連番</summary>
            /// <value>連番</value>
            public int SerialNo { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets RFタグ管理ID</summary>
                /// <value>RFタグ管理ID</value>
                public string RftagId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                public PrimaryKey(string pRftagId)
                {
                    RftagId = pRftagId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>プライマリーキー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.RftagId);
                return pk;
            }

            /// <summary>
            /// エンティティ
            /// </summary>
            /// <returns>該当のデータを返す</returns>
            public PtRftagPartsLinkEntity GetEntity(string pRftagId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pRftagId);
                // SQL文生成
                string getEntitySql = getEntity(this.TableName, condition, db);
                if (string.IsNullOrEmpty(getEntitySql))
                {
                    return null;
                }
                return db.GetEntityByDataClass<PtRftagPartsLinkEntity>(getEntitySql);
            }
            /// <summary>
            /// 主キーを指定してDELETE実行
            /// </summary>
            /// <returns>エラーの場合False</returns>
            public bool DeleteByPrimaryKey(string pRftagId, ComDB db)
            {
                PrimaryKey condition = new PrimaryKey(pRftagId);
                // SQL文生成
                string deleteSql = getDeleteSql(this.TableName, condition, db);
                if (string.IsNullOrEmpty(deleteSql))
                {
                    return false;
                }
                int result = db.Regist(deleteSql);
                return result > 0;
            }
        }

    }
}
