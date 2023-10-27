--ログインユーザのユーザ所属マスタの本務工場の地区に紐づく工場を対象とし、その工場の予備品に紐づくRFタグ予備品マスタの情報を取得する
-- ユーザマスタをログインユーザで絞込
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
    -- 構成マスタより工場を取得
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
, department AS ( 
    --拡張データを取得(部門)
    SELECT
        structure_id
        , ie.extension_data 
    FROM
        ms_structure ms 
        INNER JOIN ms_item_extension ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1760 
            AND ie.sequence_no = 1
) 
, account AS ( 
    --拡張データを取得(勘定科目)
    SELECT
        structure_id
        , ie.extension_data 
    FROM
        ms_structure ms 
        INNER JOIN ms_item_extension ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1770 
            AND ie.sequence_no = 1
) 
SELECT
    rf.rftag_id
    , SUBSTRING(rf.rftag_id, 1, 3) AS iso_identifier
    , SUBSTRING(rf.rftag_id, 4, 2) AS issuing_agency
    , SUBSTRING(rf.rftag_id, 6, 9) AS symbolic_code
    , SUBSTRING(rf.rftag_id, 15, 1) AS factory
    , SUBSTRING(rf.rftag_id, 16, 4) AS serial_no
    , SUBSTRING(rf.rftag_id, 20, 2) AS cd
    , pa.parts_no
    , department.extension_data AS department_code
    , account.extension_data AS account_code 
FROM
    pt_rftag_parts_link rf 
    INNER JOIN pt_parts pa 
        ON rf.parts_id = pa.parts_id 
    INNER JOIN factory 
        ON pa.factory_id = factory.factory_id 
    LEFT JOIN department 
        ON rf.department_structure_id = department.structure_id 
    LEFT JOIN account 
        ON rf.account_structure_id = account.structure_id 
ORDER BY
    serial_no
