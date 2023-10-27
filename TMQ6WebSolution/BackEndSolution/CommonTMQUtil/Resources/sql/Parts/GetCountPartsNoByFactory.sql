--ユーザの本務工場に紐づく地区配下の工場で絞り込んだ予備品仕様マスタを予備品Noで検索した件数を取得
WITH user_narrow AS (SELECT * FROM ms_user WHERE user_id = @UserId)
, user_district AS ( 
    -- ユーザ所属マスタの本務工場の地区を取得
    SELECT
        dbo.get_target_layer_id(ub.location_structure_id, 0) AS district_id 
    FROM
        user_narrow AS us 
        INNER JOIN ms_user_belong AS ub 
            ON (us.user_id = ub.user_id) 
    WHERE
        ub.duty_flg = 1
) 
, factory AS ( 
    -- 構成マスタより地区配下の工場を取得
    SELECT
        factory_id 
    FROM
        v_structure 
        INNER JOIN user_district AS district 
            ON parent_structure_id = district.district_id 
    WHERE
        structure_group_id = 1000 
        AND structure_layer_no = 1
) 
SELECT
    COUNT(*) 
FROM
    pt_parts pa 
    INNER JOIN factory 
        ON pa.factory_id = factory.factory_id 
WHERE
    pa.parts_no = @PartsNo
