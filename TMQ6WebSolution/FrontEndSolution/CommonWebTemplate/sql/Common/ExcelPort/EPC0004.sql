/*
 * ExcelPortダウンロード対象機能 機器コンボ用データリスト用SQL
 * exparam1:自動表示用項目(場所階層、所属階層)
 */
-- EPC0004

-- 共通処理で作成した一時テーブル「temp_structure_all」のインデックスを作成する
CREATE NONCLUSTERED INDEX idx_temp_structure_all_01
ON  #temp_structure_all(
        structure_id
    );
UPDATE
    STATISTICS #temp_structure_all;

DROP TABLE IF EXISTS #temp_structure_factory_EPC004
;
CREATE TABLE #temp_structure_factory_EPC004(
    structure_id int
    ,factory_id int
)
;
INSERT INTO #temp_structure_factory_EPC004
-- 使用する構成グループの構成IDを絞込、工場の指定に用いる
SELECT
    structure_id
    ,location_structure_id AS factory_id
FROM
    v_structure_item_all
WHERE
    structure_group_id IN (1170, 1030, 1200) 
AND language_id = @LanguageId
;
CREATE NONCLUSTERED INDEX idx_temp_structure_factory_EPC004_01
ON  #temp_structure_factory_EPC004(
        structure_id
    )
;
UPDATE
    STATISTICS #temp_structure_factory_EPC004
;

DROP TABLE IF EXISTS #temp_target_structure_item; 

CREATE TABLE #temp_target_structure_item( 
    structure_id INT
    , factory_id INT
    , structure_group_id INT
    , parent_structure_id INT
    , structure_layer_no INT
    , structure_item_id INT
    , delete_flg BIT
    , item_translation_id INT
    , location_structure_id INT
    , language_id NCHAR (2)
    , translation_text nvarchar(800)
    , translation_item_description nvarchar(800)
); 

INSERT 
INTO #temp_target_structure_item 
SELECT
    vsi.* 
FROM
    v_structure_item_all vsi 
    INNER JOIN #temp_structure_all st 
        ON vsi.structure_id = st.structure_id 
WHERE
    vsi.language_id = @languageId 
    AND vsi.structure_group_id IN (1000, 1010); 

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
    , v9.translation_text AS exparam9
    , v10.translation_text AS exparam10
    , v11.translation_text AS exparam11
    , ( 
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
                    #temp_structure_factory_EPC004 AS st_f 
                WHERE
                    st_f.structure_id = tbl.equipment_level_structure_id 
                    AND st_f.factory_id IN (0, tbl.exparam2id)
            ) 
            AND tra.structure_id = tbl.equipment_level_structure_id
    ) AS exparam12
    , ( 
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
                    #temp_structure_factory_EPC004 AS st_f 
                WHERE
                    st_f.structure_id = tbl.conservation_structure_id 
                    AND st_f.factory_id IN (0, tbl.exparam2id)
            ) 
            AND tra.structure_id = tbl.conservation_structure_id
    ) AS exparam13
    , ( 
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
                    #temp_structure_factory_EPC004 AS st_f 
                WHERE
                    st_f.structure_id = tbl.importance_structure_id 
                    AND st_f.factory_id IN (0, tbl.exparam2id)
            ) 
            AND tra.structure_id = tbl.importance_structure_id
    ) AS exparam14
    , tbl.equipment_id AS exparam15
FROM (
SELECT
     mc.machine_id AS id
    ,NULL AS parent_id
    ,mc.machine_no + ' ' + mc.machine_name AS name
    ,mc.location_district_structure_id AS exparam1id
    ,mc.location_factory_structure_id AS exparam2id
    ,mc.location_plant_structure_id AS exparam4id
    ,mc.location_series_structure_id AS exparam5id
    ,mc.location_stroke_structure_id AS exparam6id
    ,mc.location_facility_structure_id AS exparam7id
    ,mc.job_kind_structure_id AS exparam8id
    ,mc.job_large_classfication_structure_id AS exparam9id
    ,mc.job_middle_classfication_structure_id AS exparam10id
    ,mc.job_small_classfication_structure_id AS exparam11id
    ,mc.equipment_level_structure_id
    ,mc.conservation_structure_id
    ,mc.importance_structure_id
    ,eq.equipment_id

FROM mc_machine AS mc
INNER JOIN mc_equipment eq
ON mc.machine_id = eq.machine_id

---- 対象構成マスタ情報一時テーブルと機番情報の場所階層を結合する
/*IF factoryIdList != null && factoryIdList.Count > 0*/
INNER JOIN #temp_structure_all st
ON mc.location_structure_id = st.structure_id
/*END*/

/*IF param1 != null && param1 != '' */
WHERE eq.use_segment_structure_id IS NULL
/*END*/

) tbl
LEFT JOIN ( 
    SELECT
        * 
    FROM
        #temp_target_structure_item 
    WHERE
        structure_layer_no = 0 
        AND structure_group_id = 1000
) v1 
    ON tbl.exparam1id = v1.structure_id 
LEFT JOIN ( 
    SELECT
        * 
    FROM
        #temp_target_structure_item 
    WHERE
        structure_layer_no = 1 
        AND structure_group_id = 1000
) v3 
    ON tbl.exparam2id = v3.structure_id 
LEFT JOIN ( 
    SELECT
        * 
    FROM
        #temp_target_structure_item 
    WHERE
        structure_layer_no = 2 
        AND structure_group_id = 1000
) v4 
    ON tbl.exparam4id = v4.structure_id 
LEFT JOIN ( 
    SELECT
        * 
    FROM
        #temp_target_structure_item 
    WHERE
        structure_layer_no = 3 
        AND structure_group_id = 1000
) v5 
    ON tbl.exparam5id = v5.structure_id 
LEFT JOIN ( 
    SELECT
        * 
    FROM
        #temp_target_structure_item 
    WHERE
        structure_layer_no = 4 
        AND structure_group_id = 1000
) v6 
    ON tbl.exparam6id = v6.structure_id 
LEFT JOIN ( 
    SELECT
        * 
    FROM
        #temp_target_structure_item 
    WHERE
        structure_layer_no = 5 
        AND structure_group_id = 1000
) v7 
    ON tbl.exparam7id = v7.structure_id 
LEFT JOIN ( 
    SELECT
        * 
    FROM
        #temp_target_structure_item 
    WHERE
        structure_layer_no = 0 
        AND structure_group_id = 1010
) v8 
    ON tbl.exparam8id = v8.structure_id 
LEFT JOIN ( 
    SELECT
        * 
    FROM
        #temp_target_structure_item 
    WHERE
        structure_layer_no = 1 
        AND structure_group_id = 1010
) v9 
    ON tbl.exparam9id = v9.structure_id 
LEFT JOIN ( 
    SELECT
        * 
    FROM
        #temp_target_structure_item 
    WHERE
        structure_layer_no = 2 
        AND structure_group_id = 1010
) v10 
    ON tbl.exparam10id = v10.structure_id 
LEFT JOIN ( 
    SELECT
        * 
    FROM
        #temp_target_structure_item 
    WHERE
        structure_layer_no = 3 
        AND structure_group_id = 1010
) v11 
    ON tbl.exparam11id = v11.structure_id
ORDER BY
name
;
-- 作成したインデックスを削除
DROP INDEX IF EXISTS #temp_structure_all.idx_temp_structure_all_01; 
DROP INDEX IF EXISTS #temp_structure_factory_EPC004.idx_temp_structure_factory_EPC004_01; 