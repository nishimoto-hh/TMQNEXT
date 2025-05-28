WITH user_narrow AS ( 
    SELECT
        * 
    FROM
        ms_user 
    WHERE
        user_id = @UserId
)                                               
-- ユーザ所属マスタの本務工場の地区を取得
, user_district AS ( 
    SELECT
        dbo.get_target_layer_id(ub.location_structure_id, 0) AS district_id 
    FROM
        user_narrow AS us 
        INNER JOIN ms_user_belong AS ub 
            ON (us.user_id = ub.user_id) 
    WHERE
        ub.duty_flg = 1
)                                               -- 構成マスタより、取得した地区配下の工場を取得
, parts_factory AS ( 
    SELECT
        TOP 1 parts.factory_id 
    FROM
        pt_parts parts 
        INNER JOIN ( 
            SELECT
                factory_id 
            FROM
                ms_structure ms 
                INNER JOIN user_district AS district 
                    ON parent_structure_id = district.district_id 
            WHERE
                structure_group_id = 1000 
                AND structure_layer_no = 1
        ) tf 
            ON parts.factory_id = tf.factory_id 
    WHERE
        parts.parts_no = @PartsNo
) 
, target_department AS ( 
    SELECT
        ms.structure_id
        , ms.factory_id 
    FROM
        ms_structure ms 
        LEFT JOIN ms_item_extension ex 
            ON ms.structure_item_id = ex.item_id 
            AND ex.sequence_no = 1 
    WHERE
        ms.structure_group_id = 1760 
        AND ex.extension_data = @ExtensionData 
        AND ( 
            ms.factory_id IN (SELECT factory_id FROM parts_factory) 
            OR ms.factory_id = 0
        )
) 
SELECT
    coalesce( 
        ( 
            SELECT
                structure_id 
            FROM
                target_department td 
            WHERE
                td.factory_id <> 0
        ) 
        , ( 
            SELECT
                structure_id 
            FROM
                target_department td 
            WHERE
                td.factory_id = 0
        )
    ) AS structure_id
