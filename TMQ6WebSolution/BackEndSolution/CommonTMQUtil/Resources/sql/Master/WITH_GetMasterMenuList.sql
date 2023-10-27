WITH menu_list AS(
    SELECT
        cdt.conduct_id                                           -- 機能ID
        ,cdt.conduct_name_ryaku AS conduct_translation_id        -- マスタ翻訳ID
        ,tr_cdt.translation_text AS conduct_translation_text     -- マスタ翻訳
        ,grp.master_group_translation_id AS group_translation_id -- マスタグループ翻訳ID
        ,tr_grp.translation_text AS group_translation_text       -- マスタグループ翻訳
        ,grp.display_order                                       -- 表示順
    FROM
        ms_user_conduct_authority AS auth
        INNER JOIN
            cm_conduct AS cdt
        ON  auth.conduct_id = cdt.conduct_id
        AND cdt.conduct_group_id = 'MS0001'
        AND cdt.conduct_id != 'MS0001'
        AND cdt.delete_flg = 0
        LEFT JOIN
            ms_structure_group AS grp
        ON  cdt.conduct_id = 'MS' + RIGHT(REPLICATE('0', 4) + CONVERT(NVARCHAR, grp.structure_group_id), 4) --4桁になるように0埋め
            -- マスタ翻訳 
        LEFT JOIN
            ms_translation AS tr_cdt
        ON  cdt.conduct_name_ryaku = tr_cdt.translation_id
        AND tr_cdt.location_structure_id = 0
        AND tr_cdt.language_id = @LanguageId
            -- マスタグループ翻訳 
        LEFT JOIN
            ms_translation AS tr_grp
        ON  grp.master_group_translation_id = tr_grp.translation_id
        AND tr_grp.location_structure_id = 0
        AND tr_grp.language_id = @LanguageId
    WHERE
        auth.user_id = @UserId
),
except_data AS( -- システム管理者以外の場合に検索結果に含めない項目を取得
    SELECT
        ml.*
    FROM
        menu_list ml
        LEFT JOIN
            ms_user_conduct_authority auth
        ON  ml.conduct_id = auth.conduct_id
        AND auth.user_id = @UserId
        LEFT JOIN
            ms_user mu
        ON  auth.user_id = mu.user_id
        LEFT JOIN
            ms_structure ms
        ON  mu.authority_level_id = ms.structure_id
        AND ms.structure_group_id = 9040
        AND ms.delete_flg = 0
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
    WHERE
        ex.extension_data <> '99'
),
user_mainte AS( -- ユーザーがシステム管理者以外の場合、ユーザマスタメンテナンス(MS0010)を検索結果に含めないようにするためのもの
    SELECT
        ed.*
    FROM
        except_data ed
    WHERE
        ed.conduct_id = 'MS0010'
),
user_history AS(
-- ↓ユーザがシステム管理者以外の場合かつユーザーに権限のある工場の中に保全履歴個別工場が無い場合、
--   作業/故障区分(MS1550)・生産への影響(MS1480)・品質への影響(MS1490)を検索結果に含めないようにするためのもの
    SELECT
        ed.*
    FROM
        except_data ed
    WHERE
        ed.conduct_id IN('MS1550', 'MS1480', 'MS1490')
    AND (
            SELECT
                COUNT(ex.extension_data)
            FROM
                ms_user_belong mub
                INNER JOIN
                    ms_structure ms
                ON  mub.location_structure_id = ms.structure_id
                AND ms.structure_group_id = 1000
                AND ms.structure_layer_no = 1
                LEFT JOIN
                    ms_item_extension ex
                ON  ms.structure_item_id = ex.item_id
                AND ex.sequence_no = 1
            WHERE
                mub.user_id = @UserId
            AND ex.extension_data = '1'
        ) = 0
),
user_failure AS(
-- ↓ユーザがシステム管理者以外の場合かつユーザーに権限のある工場の中に故障分析個別工場が無い場合、
--   故障分析(MS1560)・故障性格要因(MS1520)・故障性格分類(MS1470)・完了/応急(MS1580)・要/否(MS1590)・対策分類Ⅰ(MS1820)・対策分類Ⅱ(MS1830)を検索結果に含めないようにするためのもの
    SELECT
        ed.*
    FROM
        except_data ed
    WHERE
        ed.conduct_id IN('MS1560', 'MS1520', 'MS1470', 'MS1580', 'MS1590', 'MS1820', 'MS1830')
    AND (
            SELECT
                COUNT(ex.extension_data)
            FROM
                ms_user_belong mub
                INNER JOIN
                    ms_structure ms
                ON  mub.location_structure_id = ms.structure_id
                AND ms.structure_group_id = 1000
                AND ms.structure_layer_no = 1
                LEFT JOIN
                    ms_item_extension ex
                ON  ms.structure_item_id = ex.item_id
                AND ex.sequence_no = 2
            WHERE
                mub.user_id = @UserId
            AND ex.extension_data = '1'
        ) = 0
)