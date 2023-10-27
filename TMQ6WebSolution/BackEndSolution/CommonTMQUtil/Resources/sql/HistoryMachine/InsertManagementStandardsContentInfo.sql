INSERT 
INTO hm_mc_management_standards_content(
    [hm_management_standards_content_id]          -- 機器別管理基準内容変更管理ID
	, [history_management_id]                     -- 変更管ID
	, [management_standards_content_id]	          -- 機器別管理基準内容ID
    , [execution_division]                        -- 実行処理区分
    , [management_standards_component_id]         -- 機器別管理基準部位ID
	, [inspection_content_structure_id]           -- 点検内容ID（保全項目）
    , [inspection_site_importance_structure_id]   -- 部位重要度
    , [inspection_site_conservation_structure_id] -- 部位保全方式
	, [maintainance_division]                     -- 保全区分
	, [maintainance_kind_structure_id]            -- 点検種別
	, [budget_amount]	                          -- 予算金額
	, [preparation_period]                        -- 準備期間
	, [long_plan_id]	                          -- 長計件名ID
	, [order_no]	                              -- 並び順
    , [schedule_type_structure_id]	              -- スケジュール基準区分ID
    , [update_serialid]                           -- 更新シリアルID
    , [insert_datetime]                           -- 登録日時
    , [insert_user_id]                            -- 登録ユーザー
    , [update_datetime]                           -- 更新日時
    , [update_user_id]                            -- 更新ユーザー
) 
OUTPUT inserted.management_standards_content_id
VALUES ( 
    NEXT VALUE FOR seq_hm_mc_management_standards_content_hm_management_standards_content_id -- 機器別管理基準内容変更管理ID
	, @HistoryManagementId                        -- 変更管理ID

      /*@NewContent
      -- 新規採番
    , NEXT VALUE FOR seq_mc_management_standards_content_management_standards_content_id
      @NewContent*/

      /*@DefaultContent
      -- 既存の機器別管理基準部位ID
    , @ManagementStandardsContentId               -- 機器別管理基準部位ID
      @DefaultContent*/
    , @ExecutionDivision                          -- 実行処理区分
	, @ManagementStandardsComponentId             -- 機器別管理基準部位ID
	, @InspectionContentStructureId               -- 点検内容ID（保全項目）
	, @InspectionSiteImportanceStructureId        -- 部位重要度
	, @InspectionSiteConservationStructureId      -- 部位保全方式
	, @MaintainanceDivision	                      -- 保全区分
	, @MaintainanceKindStructureId	              -- 点検種別
	, @BudgetAmount	                              -- 予算金額
	, @PreparationPeriod	                      -- 準備期間
	, @LongPlanId      	                          -- 長計件名ID
	, @OrderNo     	                              -- 並び順(INSERT時はすべて0)
    , @ScheduleTypeStructureId	                  -- スケジュール基準区分ID
    , 0                                           -- 更新シリアルID
    , @InsertDatetime                             -- 登録日時
    , @InsertUserId                               -- 登録ユーザー
    , @UpdateDatetime                             -- 更新日時
    , @UpdateUserId                               -- 更新ユーザー
); 
