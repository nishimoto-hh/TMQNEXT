/*
 * ExcelPortダウンロード対象機能 保全活動件名コンボ用データリスト用SQL
 * exparam1以降:自動表示用項目(場所階層、所属階層、件名等)
 */
-- EPC0008
SELECT
    tbl.id
    , tbl.exparam2id AS factory_id
    , tbl.parent_id
    , tbl.name
    , v1.translation_text AS exparam1
    , tbl.exparam2id AS exparam2
    , v3.translation_text AS exparam3
    , v4.translation_text AS exparam4
    , v5.translation_text AS exparam5
    , v6.translation_text AS exparam6
    , v7.translation_text AS exparam7
    , v8.translation_text AS exparam8
    , re.request_no AS exparam9
    , tbl.plan_implementation_content AS exparam10
    , tbl.subject_note AS exparam11 
FROM
    ( 
        SELECT
            su.summary_id AS id
            , NULL AS parent_id
            , su.subject AS name
            , su.location_district_structure_id AS exparam1id
            , su.location_factory_structure_id AS exparam2id
            , su.location_plant_structure_id AS exparam4id
            , su.location_series_structure_id AS exparam5id
            , su.location_stroke_structure_id AS exparam6id
            , su.location_facility_structure_id AS exparam7id
            , su.job_structure_id AS exparam8id
            , su.plan_implementation_content
            , su.subject_note 
        FROM
            ma_summary AS su                    
        ---- 対象構成マスタ情報一時テーブルと保全活動件名情報の場所階層を結合する
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
            INNER JOIN #temp_structure_all st 
                ON su.location_structure_id = st.structure_id 
        /*END*/
        /*IF param1 != null && param1 != '' */
        WHERE
            su.activity_division = /*param1*/0
        /*END*/
    ) tbl 
    LEFT JOIN ma_request re 
        ON tbl.id = re.summary_id 
    LEFT JOIN ( 
        SELECT
            * 
        FROM
            v_structure_item_all 
        WHERE
            language_id = /*languageId*/'ja' 
            AND structure_layer_no = 0 
            AND structure_group_id = 1000
    ) v1 
        ON tbl.exparam1id = v1.structure_id 
    LEFT JOIN ( 
        SELECT
            * 
        FROM
            v_structure_item_all 
        WHERE
            language_id = /*languageId*/'ja' 
            AND structure_layer_no = 1 
            AND structure_group_id = 1000
    ) v3 
        ON tbl.exparam2id = v3.structure_id 
    LEFT JOIN ( 
        SELECT
            * 
        FROM
            v_structure_item_all 
        WHERE
            language_id = /*languageId*/'ja' 
            AND structure_layer_no = 2 
            AND structure_group_id = 1000
    ) v4 
        ON tbl.exparam4id = v4.structure_id 
    LEFT JOIN ( 
        SELECT
            * 
        FROM
            v_structure_item_all 
        WHERE
            language_id = /*languageId*/'ja' 
            AND structure_layer_no = 3 
            AND structure_group_id = 1000
    ) v5 
        ON tbl.exparam5id = v5.structure_id 
    LEFT JOIN ( 
        SELECT
            * 
        FROM
            v_structure_item_all 
        WHERE
            language_id = /*languageId*/'ja' 
            AND structure_layer_no = 4 
            AND structure_group_id = 1000
    ) v6 
        ON tbl.exparam6id = v6.structure_id 
    LEFT JOIN ( 
        SELECT
            * 
        FROM
            v_structure_item_all 
        WHERE
            language_id = /*languageId*/'ja' 
            AND structure_layer_no = 5 
            AND structure_group_id = 1000
    ) v7 
        ON tbl.exparam7id = v7.structure_id 
    LEFT JOIN ( 
        SELECT
            * 
        FROM
            v_structure_item_all 
        WHERE
            language_id = /*languageId*/'ja' 
            AND structure_layer_no = 0 
            AND structure_group_id = 1010
    ) v8 
        ON tbl.exparam8id = v8.structure_id 
ORDER BY
    name
