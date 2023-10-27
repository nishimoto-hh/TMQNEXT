--******************************************************************
--出庫一覧(棚別部門別在庫情報)
--******************************************************************
WITH department AS(
    --拡張データ、翻訳を取得(部門)
    SELECT
        structure_id,
        ie.extension_data
    FROM
        ms_structure ms
        INNER JOIN
            ms_item_extension ie
        ON  ms.structure_item_id = ie.item_id
        AND ms.structure_group_id = 1760
        AND ie.sequence_no = 1
),
account AS(
    --拡張データ、翻訳を取得(勘定科目)
    SELECT
        structure_id,
        ie.extension_data
    FROM
        ms_structure ms
        INNER JOIN
            ms_item_extension ie
        ON  ms.structure_item_id = ie.item_id
        AND ms.structure_group_id = 1770
        AND ie.sequence_no = 1
),
number_unit AS(
    --数量管理単位
    SELECT
        ms.structure_id AS unit_id,
        ex.extension_data AS unit_digit
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        ms.structure_group_id = 1730
),
unit_round AS(
    --丸め処理区分
    SELECT
        ms.factory_id,
        ex.extension_data AS unit_round_division
    FROM
        (
            SELECT
                ms.factory_id,
                MAX(ms.structure_id) AS structure_id
            FROM
                ms_structure ms
            WHERE
                ms.structure_group_id = 2050
            GROUP BY
                ms.factory_id
        ) ms
        LEFT JOIN
            (
                SELECT
                    ms.structure_id,
                    ex.extension_data
                FROM
                    ms_structure ms
                    LEFT JOIN
                        ms_item_extension ex
                    ON  ms.structure_item_id = ex.item_id
                    AND ex.sequence_no = 1
                WHERE
                    ms.structure_group_id = 2050
            ) ex
        ON  ms.structure_id = ex.structure_id
),
structure_factory AS(
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id,
        location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN(1040, 1730, 1760, 1770, 1940)
    AND language_id = @LanguageId
),
main AS(
    SELECT
        pl.old_new_structure_id,
        pl.department_structure_id,
        pl.account_structure_id,
        pl.unit_structure_id,
        parts.factory_id,
        pls.parts_location_id,
        pls.parts_location_detail_no,
        SUM(pls.stock_quantity) AS inventry
    FROM
        pt_location_stock pls
        LEFT JOIN
            pt_lot pl
        ON  pls.lot_control_id = pl.lot_control_id
        LEFT JOIN
            pt_parts parts
        ON  pls.parts_id = parts.parts_id
    WHERE
        pls.parts_id = @PartsId
    AND
        pl.delete_flg = 0
    GROUP BY
        pl.old_new_structure_id,
        pl.department_structure_id,
        pl.account_structure_id,
        pl.unit_structure_id,
        parts.factory_id,
        pls.parts_location_id,
        pls.parts_location_detail_no
)
SELECT
    main.old_new_structure_id                                            -- 新旧区分ID
    , dpm.extension_data AS department_cd                                -- 部門コード
    , act.extension_data AS subject_cd                                   -- 勘定科目コード
    , main.old_new_structure_id AS old_new_nm                            -- 新旧区分(在庫一覧検索用)
    , main.department_structure_id AS to_department_nm                   -- 部門(在庫一覧検索用)
    , main.account_structure_id AS to_subject_nm                         -- 勘定科目(在庫一覧検索用)
    , COALESCE(number_unit.unit_digit, 0) AS unit_digit                  -- 小数点以下桁数(数量)
    , COALESCE(unit_round.unit_round_division, 0) AS unit_round_division -- 丸め処理区分(数量)
    , main.inventry                                                      -- 在庫数
    , main.parts_location_id                                             -- 棚番ID
    , main.parts_location_detail_no                                      -- 棚枝番
    , main.factory_id                                                    -- 工場ID
    , main.unit_structure_id                                             -- 数量管理単位
    ------------ 以下は翻訳を取得 ------------
    ,
    (
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
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = main.old_new_structure_id
                AND st_f.factory_id IN(0, main.factory_id)
            )
        AND tra.structure_id = main.old_new_structure_id
    ) AS old_new_name, -- 新旧区分
    COALESCE((
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
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = main.department_structure_id
                    AND st_f.factory_id IN(0, main.factory_id)
                )
            AND tra.structure_id = main.department_structure_id
        ),(
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = @LanguageId
            AND tra.location_structure_id = (
                    SELECT
                        MIN(st_f.factory_id)
                    FROM
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = main.department_structure_id
                    AND st_f.factory_id NOT IN(0, main.factory_id)
                )
            AND tra.structure_id = main.department_structure_id
        )) AS department_nm, -- 部門
    COALESCE((
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
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = main.account_structure_id
                    AND st_f.factory_id IN(0, main.factory_id)
                )
            AND tra.structure_id = main.account_structure_id
        ),(
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = @LanguageId
            AND tra.location_structure_id = (
                    SELECT
                        MIN(st_f.factory_id)
                    FROM
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = main.account_structure_id
                    AND st_f.factory_id NOT IN(0, main.factory_id)
                )
            AND tra.structure_id = main.account_structure_id
        )) AS subject_nm, -- 勘定科目
    (
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
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = main.unit_structure_id
                AND st_f.factory_id IN(0, main.factory_id)
            )
        AND tra.structure_id = main.unit_structure_id
    ) AS unit, -- 数量管理単位
    (
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
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = main.parts_location_id
                AND st_f.factory_id IN(0, main.factory_id)
            )
        AND tra.structure_id = main.parts_location_id
    ) AS parts_location_name -- 棚番
FROM
    main
    LEFT JOIN
        department dpm -- 部門
    ON  main.department_structure_id = dpm.structure_id
    LEFT JOIN
        account act -- 勘定科目
    ON  main.account_structure_id = act.structure_id
    LEFT JOIN
        number_unit --数量管理単位
    ON  main.unit_structure_id = number_unit.unit_id
    LEFT JOIN
        unit_round --丸め処理区分
    ON  main.factory_id = unit_round.factory_id
WHERE
    main.old_new_structure_id = @OldNewStructureId
AND main.department_structure_id = @DepartmentStructureId
AND main.account_structure_id = @AccountStructureId
AND main.parts_location_id = @PartsLocationId
AND main.parts_location_detail_no = COALESCE(@PartsLocationDetailNo, '')
