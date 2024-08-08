WITH user_factory AS ( -- ログインユーザーの本務工場を取得
    SELECT
        belong.location_structure_id 
    FROM
        ms_user_belong belong 
    WHERE
        belong.user_id = @UserId 
        AND belong.duty_flg = 1
) 
SELECT
    detail.inspection_site_name                 -- 保全部位名称
    , detail.inspection_content_name            -- 保全項目名称
    , detail.budget_amount                      -- 予算金額
    , detail.preparation_period                 -- 準備期間(日)
    , detail.cycle_year                         -- 周期(年)
    , detail.cycle_month                        -- 周期(月)
    , detail.cycle_day                          -- 周期(日)
    , detail.disp_cycle                         -- 表示周期
    , detail.remarks                            -- 機器別管理基準備考
    , detail.management_standards_id            -- 機器別管理基準標準ID
    , detail.management_standards_detail_id     -- 機器別管理基準標準詳細ID
    , detail.update_serialid                    -- 更新シリアルID
    /*@IsManagementStandardsDetail
    , detail.inspection_site_importance_structure_id    -- 部位重要度
    , detail.inspection_site_conservation_structure_id  -- 保全方式
    , detail.maintainance_division                      -- 保全区分
    , detail.maintainance_kind_structure_id             -- 点検種別
    , detail.schedule_type_structure_id                 -- スケジュール管理
    @IsManagementStandardsDetail*/
    ---------------------------------- 以下は翻訳を取得 ----------------------------------
    /*@IsGetTrans
    -- 翻訳を取得する必要がある場合
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND ( 
                tra.factory_id <> 0             --工場アイテム
                OR ( 
                    tra.factory_id = 0          --標準アイテム
                    AND tra.location_structure_id = ( 
                        SELECT
                            max(st_f.factory_id) 
                        FROM
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = detail.inspection_site_importance_structure_id 
                            AND st_f.factory_id IN (0, user_factory.location_structure_id)
                    )
                )
            ) 
            AND tra.structure_id = detail.inspection_site_importance_structure_id
    ) AS inspection_site_importance_name        -- 部位重要度
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND ( 
                tra.factory_id <> 0             --工場アイテム
                OR ( 
                    tra.factory_id = 0          --標準アイテム
                    AND tra.location_structure_id = ( 
                        SELECT
                            max(st_f.factory_id) 
                        FROM
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = detail.inspection_site_conservation_structure_id 
                            AND st_f.factory_id IN (0, user_factory.location_structure_id)
                    )
                )
            ) 
            AND tra.structure_id = detail.inspection_site_conservation_structure_id
    ) AS inspection_site_conservation_name      -- 保全方式
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND ( 
                tra.factory_id <> 0             --工場アイテム
                OR ( 
                    tra.factory_id = 0          --標準アイテム
                    AND tra.location_structure_id = ( 
                        SELECT
                            max(st_f.factory_id) 
                        FROM
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = detail.maintainance_division 
                            AND st_f.factory_id IN (0, user_factory.location_structure_id)
                    )
                )
            ) 
            AND tra.structure_id = detail.maintainance_division
    ) AS maintainance_division_name             -- 保全区分
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND ( 
                tra.factory_id <> 0             --工場アイテム
                OR ( 
                    tra.factory_id = 0          --標準アイテム
                    AND tra.location_structure_id = ( 
                        SELECT
                            max(st_f.factory_id) 
                        FROM
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = detail.maintainance_kind_structure_id 
                            AND st_f.factory_id IN (0, user_factory.location_structure_id)
                    )
                )
            ) 
            AND tra.structure_id = detail.maintainance_kind_structure_id
    ) AS maintainance_kind_name                 -- 点検種別
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND ( 
                tra.factory_id <> 0             --工場アイテム
                OR ( 
                    tra.factory_id = 0          --標準アイテム
                    AND tra.location_structure_id = ( 
                        SELECT
                            max(st_f.factory_id) 
                        FROM
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = detail.schedule_type_structure_id 
                            AND st_f.factory_id IN (0, user_factory.location_structure_id)
                    )
                )
            ) 
            AND tra.structure_id = detail.schedule_type_structure_id
    ) AS schedule_type_name                     -- スケジュール管理基準
    @IsGetTrans*/

FROM
    mc_management_standards_detail detail -- 機器別管理基準標準詳細

    INNER JOIN mc_management_standards ms -- 機器別管理基準標準
        ON detail.management_standards_id = ms.management_standards_id

    CROSS JOIN user_factory -- ログインユーザーの本務工場
WHERE
    detail.management_standards_id = @ManagementStandardsId

    /*@IsManagementStandardsDetail
       -- 保全項目編集画面検索時、機器別管理基準標準詳細IDを指定
       AND detail.management_standards_detail_id = @ManagementStandardsDetailId
    @IsManagementStandardsDetail*/

ORDER BY
    detail.inspection_site_name ASC             -- 保全部位名称 昇順
    , detail.inspection_content_name ASC        -- 保全項目名称 昇順
