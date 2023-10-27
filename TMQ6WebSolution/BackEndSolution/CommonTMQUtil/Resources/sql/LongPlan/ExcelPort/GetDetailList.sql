/*
* 長期計画 機種別管理基準 ExelPortダウンロード一覧情報 検索
*/
WITH target AS(
     -- 表示情報を取得するSQL、翻訳対応のためWITH句へ
    SELECT
         con.management_standards_content_id
        ,con.inspection_content_structure_id
        ,com.inspection_site_structure_id
        ,com.is_management_standard_conponent
        ,mc.machine_no
        ,mc.machine_name
        ,mc.location_structure_id
        ,mc.location_district_structure_id AS district_id
        ,mc.location_factory_structure_id AS factory_id
        ,mc.location_plant_structure_id AS plant_id
        ,mc.location_series_structure_id AS series_id
        ,mc.location_stroke_structure_id AS stroke_id
        ,mc.location_facility_structure_id AS facility_id
        ,mc.job_structure_id
        ,mc.job_kind_structure_id AS job_id
        ,mc.job_large_classfication_structure_id AS large_classfication_id
        ,mc.job_middle_classfication_structure_id AS middle_classfication_id
        ,mc.job_small_classfication_structure_id AS small_classfication_id
        ,lp.long_plan_id
        ,lp.subject
        ,lp.subject_note
        ,lp.person_id
        ,lp.person_name
    FROM
    -- 機器別管理基準内容
        mc_management_standards_content con

    -- 機器別管理基準部位
    INNER JOIN
        mc_management_standards_component com
    ON  con.management_standards_component_id = com.management_standards_component_id

    -- 機番情報
    INNER JOIN
        mc_machine mc
    ON  com.machine_id = mc.machine_id

    -- 長期計画件名
    LEFT JOIN 
        ln_long_plan lp
    ON  con.long_plan_id = lp.long_plan_id
    WHERE
      EXISTS ( 
        SELECT
          * 
        FROM
          mc_equipment AS eq 
        WHERE
          eq.machine_id = mc.machine_id 
          AND eq.use_segment_structure_id IS NULL
      ) 
)

SELECT 
     target.management_standards_content_id
    ,target.machine_no
    ,target.machine_name
    ,target.location_structure_id
    ,target.district_id
    ,target.factory_id
    ,target.plant_id
    ,target.series_id
    ,target.stroke_id
    ,target.facility_id
    ,target.job_structure_id
    ,target.job_id
    ,target.large_classfication_id
    ,target.middle_classfication_id
    ,target.small_classfication_id
    ,target.inspection_site_structure_id
    ,target.inspection_content_structure_id
    ,target.long_plan_id
    ,target.subject
    ,target.subject_note
    ,target.person_id
    ,target.person_name
    -- 翻訳
    -- 地区
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.district_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.district_id
    ) AS district_name
    -- 工場
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.factory_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.factory_id
    ) AS factory_name
    -- プラント
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.plant_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.plant_id
    ) AS plant_name
    -- 系列
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.series_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.series_id
    ) AS series_name
    -- 工程
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.stroke_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.stroke_id
    ) AS stroke_name
    -- 設備
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.facility_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.facility_id
    ) AS facility_name
    -- 職種
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.job_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.job_id
    ) AS job_name
    -- 機種大分類
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.large_classfication_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.large_classfication_id
    ) AS large_classfication_name
    -- 機種中分類
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.middle_classfication_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.middle_classfication_id
    ) AS middle_classfication_name
    -- 機種小分類
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.small_classfication_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.small_classfication_id
    ) AS small_classfication_name
    -- 保全部位
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.inspection_site_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.inspection_site_structure_id
    ) AS inspection_site_name
    -- 保全項目
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.inspection_content_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.inspection_content_structure_id
    ) AS inspection_content_name

FROM 
    target

WHERE
    EXISTS(
        SELECT * FROM #temp_structure_selected temp 
        WHERE target.location_structure_id = temp.structure_id)

AND
    EXISTS(
        SELECT * FROM #temp_structure_selected temp 
        WHERE target.job_structure_id = temp.structure_id)

ORDER BY 
     target.machine_no
    ,target.inspection_site_structure_id
    ,target.inspection_content_structure_id
