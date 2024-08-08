SELECT
    ms.location_structure_id                             -- 機能場所階層ID
    , COALESCE(ms.job_structure_id, 0) AS job_structure_id -- 職種機種階層ID
    , ms.management_standards_id                           -- 機器別管理基準標準ID
    , ms.location_factory_structure_id                     -- 工場ID
    , ms.update_serialid                                   -- 更新シリアルID
    /*@ManagementStandardsName
    , ms.management_standards_name                         -- 標準名称
    @ManagementStandardsName*/
    /*@Memo
    , ms.memo                                              -- メモ
    @Memo*/
    /*@IsDetail
       -- 機器別管理基準標準に紐付く機器別管理基準詳細の最大の更新日時
       -- 削除時の排他チェックで使用
    , ( 
        SELECT
            max(detail.update_datetime) 
        FROM
            mc_management_standards_detail detail 
        WHERE
            detail.management_standards_id = @ManagementStandardsId
    ) AS max_update_datetime
    @IsDetail*/
    ---------------------------------- 以下は翻訳を取得 ----------------------------------
    /*@DistrictName
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ms.location_district_structure_id 
                    AND st_f.factory_id IN (0, ms.location_factory_structure_id)
            ) 
            AND tra.structure_id = ms.location_district_structure_id
    ) AS district_name -- 地区
    @DistrictName*/
    /*@FactoryName
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ms.location_factory_structure_id 
                    AND st_f.factory_id IN (0, ms.location_factory_structure_id)
            ) 
            AND tra.structure_id = ms.location_factory_structure_id
    ) AS factory_name -- 工場
    @FactoryName*/
    /*@JobName
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ms.job_kind_structure_id 
                    AND st_f.factory_id IN (0, ms.location_factory_structure_id)
            ) 
            AND tra.structure_id = ms.job_kind_structure_id
    ) AS job_name -- 職種
    @JobName*/
    /*@LargeClassficationName
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ms.job_large_classfication_structure_id 
                    AND st_f.factory_id IN (0, ms.location_factory_structure_id)
            ) 
            AND tra.structure_id = ms.job_large_classfication_structure_id
    ) AS large_classfication_name -- 機種大分類
    @LargeClassficationName*/
    /*@MiddleClassficationName
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ms.job_middle_classfication_structure_id 
                    AND st_f.factory_id IN (0, ms.location_factory_structure_id)
            ) 
            AND tra.structure_id = ms.job_middle_classfication_structure_id
    ) AS middle_classfication_name -- 機種中分類
    @MiddleClassficationName*/
    /*@SmallClassficationName
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    max(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ms.job_small_classfication_structure_id 
                    AND st_f.factory_id IN (0, ms.location_factory_structure_id)
            ) 
            AND tra.structure_id = ms.job_small_classfication_structure_id
    ) AS small_classfication_name -- 機種小分類
    @SmallClassficationName*/
FROM
    mc_management_standards ms

    /*@IsDetail
       -- 詳細画面・詳細編集画面の検索時、機器別管理基準標準IDを指定
       WHERE ms.management_standards_id = @ManagementStandardsId
    @IsDetail*/