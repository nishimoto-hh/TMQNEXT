--******************************************************************
--指定画面の翻訳情報を取得
--******************************************************************
-- 翻訳を個別工場を考慮して取得
WITH 
-- ユーザの権限に関わらず、本務工場より共通工場を優先する
/*user_auth AS ( 
  -- ユーザの権限、システム管理者の場合は本務工場より共通工場を優先
  SELECT
    vs.structure_id
    , ex.extension_data AS auth_code 
  FROM
    v_structure AS vs 
    INNER JOIN ms_item_extension AS ex 
      ON ( 
        vs.structure_item_id = ex.item_id 
        AND ex.sequence_no = 1
      ) 
  WHERE
    vs.structure_group_id = 9040
) 
,*/
factory_priority AS(
    -- 工場毎の優先度を設定
    -- 優先度1：共通工場の翻訳
    SELECT
        1 AS priority
        ,0 AS factory_id
    -- 優先度0(最低):ユーザの本務工場の翻訳
    UNION
    SELECT
        ---- システム管理者(99)の場合は本務工場より共通を優先するため優先度を0にする
        --CASE ua.auth_code WHEN '99' THEN 0 ELSE 10 END AS priority
        -- 本務工場より共通を優先するため優先度を0にする
        0 AS priority
        ,ub.location_structure_id AS factory_id
    FROM
        ms_user_belong AS ub
        INNER JOIN
            ms_user AS us
        ON  (
                ub.user_id = us.user_id
            )
        -- ユーザの権限に関わらず、本務工場より共通工場を優先する
        --LEFT OUTER JOIN
        --    user_auth AS ua
        --ON  (
        --        us.authority_level_id = ua.structure_id
        --    )
    WHERE
        ub.duty_flg = 1
    AND ub.user_id = @UserId
    -- 優先度100(最大)：引数の工場の翻訳
    UNION
    SELECT
        100 AS priority
        ,@FactoryId AS factory_id
)
,tra_priority AS(
    -- 翻訳と工場毎の優先度を取得(処理用の一時テーブル)
    SELECT
        tra.location_structure_id
        ,tra.language_id
        ,tra.translation_id
        ,tra.translation_text
        ,fp.priority
    FROM
        ms_translation AS tra
        INNER JOIN
            factory_priority AS fp
        ON  (
                tra.location_structure_id = fp.factory_id
            )
    WHERE
        tra.language_id = @LanguageId
)
,tra_max AS (
    -- 翻訳IDごとに最大の優先度の工場で絞り込み
    SELECT
        main.location_structure_id
        ,main.translation_id
        ,main.language_id
    FROM
        tra_priority AS main
    WHERE
    -- 翻訳IDが同じで、自身より優先度が大きいものを取得しない→最大の優先度のみを取得する
        NOT EXISTS(
            SELECT
                *
            FROM
                tra_priority AS sub
            WHERE
                main.translation_id = sub.translation_id
            AND main.priority < sub.priority
        )
)
,tr AS(
    SELECT
        tr.translation_id
        ,tr.translation_text
    FROM
        ms_translation tr
        INNER JOIN
            tra_max AS tr_max
        ON  tr.location_structure_id = tr_max.location_structure_id
        AND tr.translation_id = tr_max.translation_id
        AND tr.language_id = tr_max.language_id
)

SELECT 
    fd.control_group_id AS CTRLGRPID,
    fd.form_no AS FORMNO,
    CONCAT(fd.control_group_id, '_', fd.form_no) AS CTRLID,
    fd.control_group_type AS CTRLTYPE,
    CONCAT('VAL', fcd.control_no) AS ITEM_NAME,
    fcd.control_type AS CELLTYPE,
    fcd.button_control_id AS BTN_CTRLID,
    tr_item.translation_text AS ITEM_TEXT,
    tr_ph.translation_text AS PLACEHOLDER_TEXT,
    tr_tooltip.translation_text AS TOOLTIP_TEXT

FROM cm_form_define fd

INNER JOIN cm_form_control_define fcd
ON  fd.program_id = fcd.program_id
AND fd.form_no = fcd.form_no
AND fd.control_group_id = fcd.control_group_id

INNER JOIN cm_control_define cd
ON  fcd.control_id = cd.control_id
AND fcd.control_type = cd.control_type

LEFT JOIN cm_control_unused cu
ON  fcd.location_structure_id = cu.location_structure_id
AND fcd.control_id = cu.control_id
AND fcd.control_type = cu.control_type

LEFT JOIN (SELECT * FROM cm_control_user_customize WHERE user_id = @UserId) uc
ON  fcd.program_id = uc.program_id
AND fcd.form_no = uc.form_no
AND fcd.control_group_id = uc.control_group_id
AND fcd.control_no = uc.control_no

LEFT JOIN tr tr_item
ON  fcd.control_id = tr_item.translation_id

LEFT JOIN tr tr_ph
ON  CONVERT(nvarchar, cd.text_placeholder_translation_id) = tr_ph.translation_id

LEFT JOIN tr tr_tooltip
ON  CONVERT(nvarchar, cd.tooltip_translation_id) = tr_tooltip.translation_id

WHERE
    fd.program_id = @PgmId
AND fd.form_no = @FormNo
AND fcd.location_structure_id in(0, @FactoryId)
AND fcd.delete_flg != 1
AND cu.control_id IS NULL
AND (uc.data_division = 1 OR uc.data_division IS NULL)

