--******************************************************************
--機能権限設定一覧
--******************************************************************
SELECT DISTINCT
    selection               --権限があるか(ある：1、ない：0)
    , conduct_group_id      --機能グループID
    , conduct_group_name    --機能グループ名
    , conduct_id            --機能ID
    , conduct_name          --機能名
FROM
    ( 
        SELECT DISTINCT
            0 AS selection
            , grp.conduct_group_id
            , cdc.translation_text AS conduct_group_name
            , grp.conduct_id
            , grp.translation_text AS conduct_name 
        FROM
            ( 
             --機能グループIDの情報を取得
                SELECT DISTINCT
                    ccd.conduct_id
                    , ccd.conduct_group_id
                    , mtl.translation_text 
                FROM
                    cm_conduct ccd 
                    LEFT JOIN ms_translation mtl 
                        ON ccd.conduct_name = mtl.translation_id 
                        AND mtl.language_id = @LanguageId 
                        AND mtl.location_structure_id = 0
                WHERE
                    (ccd.program_id IS NOT NULL AND ccd.program_id <> '')
                    AND (ccd.menu_division = 1 OR (ccd.conduct_id LIKE '%MS%'))
                    AND ccd.conduct_id NOT IN ('CM00001','MS0001')
                    AND ccd.delete_flg = 0
            ) grp
            , ( 
               --機能IDの情報を取得
                SELECT
                    ccd.conduct_id
                    , ccd.conduct_group_id
                    , mtl.translation_text 
                FROM
                    cm_conduct ccd 
                    LEFT JOIN ms_translation mtl 
                        ON ccd.conduct_name = mtl.translation_id 
                        AND mtl.language_id = @LanguageId 
                        AND mtl.location_structure_id = 0
                WHERE
                    ccd.delete_flg = 0
            ) cdc 
        WHERE
            grp.conduct_group_id = cdc.conduct_id
    ) all_data 
UNION ALL 
SELECT DISTINCT
    * 
FROM
    ( 
        SELECT
            1 AS selection
            , grp.conduct_group_id
            , cdc.translation_text AS conduct_group_name
            , grp.conduct_id
            , grp.translation_text AS conduct_name 
        FROM
            ( 
                --権限がある機能グループIDの情報を取得
                SELECT
                    ccd.conduct_id
                    , ccd.conduct_group_id
                    , mtl.translation_text 
                FROM
                    cm_conduct ccd 
                    LEFT JOIN ms_translation mtl 
                        ON ccd.conduct_name = mtl.translation_id 
                        AND mtl.language_id = @LanguageId 
                        AND mtl.location_structure_id = 0
                    LEFT JOIN ms_user_conduct_authority uca 
                        ON ccd.conduct_id = uca.conduct_id 
                WHERE
                    (ccd.program_id IS NOT NULL AND ccd.program_id <> '')
                    AND (ccd.menu_division = 1 OR (ccd.conduct_id LIKE '%MS%'))
                    AND ccd.conduct_id NOT IN ('CM00001','MS0001')
                    AND uca.user_id = @UserId
                    AND ccd.delete_flg = 0
            ) grp
            , ( 
               --権限がある機能IDの情報を取得
                SELECT
                    ccd.conduct_id
                    , ccd.conduct_group_id
                    , mtl.translation_text 
                FROM
                    cm_conduct ccd 
                    LEFT JOIN ms_translation mtl 
                        ON ccd.conduct_name = mtl.translation_id 
                        AND mtl.language_id = @LanguageId 
                        AND mtl.location_structure_id = 0
                    LEFT JOIN ms_user_conduct_authority uca 
                        ON ccd.conduct_id = uca.conduct_id 
                WHERE
                    uca.user_id = @UserId
                    AND ccd.delete_flg = 0
            ) cdc 
        WHERE
            grp.conduct_group_id = cdc.conduct_id
    ) slt 
ORDER BY
    conduct_id

