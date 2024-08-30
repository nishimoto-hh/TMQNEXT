/*
* 一覧の項目の内容を取得するSQL
*/
WITH target AS(
     -- 表示情報を取得するSQL、翻訳対応のためWITH句へ
    SELECT
         lp.long_plan_id
        ,lp.subject
        ,lp.subject_note
        ,lp.location_structure_id
        ,lp.location_district_structure_id AS district_id
        ,lp.location_factory_structure_id AS factory_id
        ,lp.location_plant_structure_id AS plant_id
        ,lp.location_series_structure_id AS series_id
        ,lp.location_stroke_structure_id AS stroke_id
        ,lp.location_facility_structure_id AS facility_id
        ,lp.job_structure_id
        ,lp.job_kind_structure_id AS job_id
        ,lp.job_large_classfication_structure_id AS large_classfication_id
        ,lp.job_middle_classfication_structure_id AS middle_classfication_id
        ,lp.job_small_classfication_structure_id AS small_classfication_id
        ,lp.maintenance_season_structure_id
        ,lp.person_id
        ,coalesce(person.display_name, lp.person_name) AS person_name
        ,lp.work_item_structure_id
        ,lp.budget_management_structure_id
        ,lp.budget_personality_structure_id
        ,lp.long_plan_division_structure_id
        ,lp.long_plan_group_structure_id
/*@UnExcelPort
        -- 機器添付有無
        -- ひとつの件名に複数の機器別管理基準内容が紐づきうるので、複数の機器の添付情報を結合して表示する
        /*@FileLinkEquip
        ,(
             SELECT
                (
                     SELECT
                         dbo.get_file_download_info(1600, eq_att.machine_id) + dbo.get_file_download_info(1610, eq_att.equipment_id)
                    FROM
                        #eq_att AS eq_att
                    WHERE
                        eq_att.long_plan_id = lp.long_plan_id
                    ORDER BY
                         eq_att.long_plan_id FOR xml path('')
                )
        ) AS file_link_equip
        @FileLinkEquip*/
        --dbo.get_file_download_info(1640, lp.long_plan_id)
        /*@FileLinkSubject
        ,REPLACE((
                SELECT
                    dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
                FROM
                    #temp_attachment as att_temp
                WHERE
                    lp.long_plan_id = att_temp.key_id
                AND att_temp.function_type_id = 1640
                ORDER BY
                    document_no FOR xml path('')
            ), ' ', '') AS file_link_subject
        @FileLinkSubject*/
@UnExcelPort*/
        ,lp.purpose_structure_id
        ,lp.work_class_structure_id
        ,lp.treatment_structure_id
        ,lp.facility_structure_id
/*@UnExcelPort
        ,lp.update_serialid
        ,
        -- 参照画面の排他処理で用いる項目
        max_dt.long_plan_id_dt
        ,max_dt.mc_man_st_con_update_datetime
        ,max_dt.sche_detail_update_datetime
        ,max_dt.attachment_update_datetime
        -- スケジュールと同じ値
        ,lp.long_plan_id AS key_id
        -- 準備対象列
        ,COALESCE((SELECT 1 FROM #prepare_target_narrow AS pt WHERE pt.long_plan_id = lp.long_plan_id),0) AS preparation_flg
@UnExcelPort*/
    FROM
         ln_long_plan AS lp
        LEFT OUTER JOIN
            ms_user AS person
        ON  (
                lp.person_id = person.user_id
            )
/*@UnExcelPort
        LEFT OUTER JOIN
            #max_dt AS max_dt
        ON  (
                lp.long_plan_id = max_dt.long_plan_id_dt
            )
@UnExcelPort*/
)