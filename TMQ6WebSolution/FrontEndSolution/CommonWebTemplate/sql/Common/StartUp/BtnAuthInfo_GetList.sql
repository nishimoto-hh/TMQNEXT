/*
 * ボタン権限情報の取得
 */
-- 構成マスタ内の全工場
WITH factory AS(
    SELECT
        st.structure_id AS factory_id
    FROM
        ms_structure AS st
    WHERE
        st.structure_group_id = 1000
    AND st.structure_layer_no = 1
    AND st.delete_flg != 1
    UNION
    -- 共通工場は構成マスタに無いので追加
    SELECT
         0
)

SELECT DISTINCT
    /*IF IsCommonLayout == null || !IsCommonLayout */
     org.program_id AS PgmId
    ,org.form_no AS FormNo
    /*END*/
    /*IF IsCommonLayout != null && IsCommonLayout */
     org.fd_program_id AS PgmId
    ,org.fd_form_no AS FormNo
    /*END*/
    ,org.button_control_id AS BtnCtrlId
    ,org.button_action_division AS BtnActionDiv
    ,org.button_authority_division AS BtnAuthDiv
    ,org.dat_transition_action_division AS TransActionDiv
    ,org.location_structure_id AS FactoryId
FROM (
SELECT
     all_ctrl.*
    ,unused.control_id AS unused_id
    /*IF IsCommonLayout != null && IsCommonLayout */
    ,fd.program_id AS fd_program_id
    ,fd.form_no AS fd_form_no
    /*END*/
FROM
    cm_form_control_define all_ctrl
/* 対象外コントロールマスタを外部結合 */
LEFT JOIN (
    SELECT * FROM cm_control_unused 
    WHERE delete_flg != 1 
    ) unused
    ON  all_ctrl.location_structure_id = unused.location_structure_id 
    AND all_ctrl.control_id = unused.control_id 
    AND all_ctrl.control_type = unused.control_type
/*IF IsCommonLayout != null && IsCommonLayout */
LEFT JOIN (
    SELECT DISTINCT program_id, form_no, common_form_no 
    FROM cm_form_define 
    WHERE common_form_no != 0 
    )fd
    ON all_ctrl.form_no = fd.common_form_no
/*END*/
) org
/* 指定工場のコントロールを抽出*/
JOIN (
    SELECT
         MAX(location_structure_id) AS location_structure_id
        ,program_id
        ,form_no
        ,control_group_id
        ,define_type
        ,control_no
    FROM
        cm_form_control_define 
    WHERE
        delete_flg != 1 
    AND control_type LIKE '08%'
    /*IF IsCommonLayout == null || !IsCommonLayout */
        AND program_id != '0'
    /*END*/
    /*IF IsCommonLayout != null && IsCommonLayout */
        AND program_id = '0'
    /*END*/
    AND location_structure_id IN (SELECT factory_id FROM factory)
    GROUP BY
         program_id
        ,form_no
        ,control_group_id
        ,define_type
        ,control_no
    ) max
    ON org.location_structure_id = max.location_structure_id 
    AND org.program_id = max.program_id 
    AND org.control_group_id = max.control_group_id
    AND org.define_type = max.define_type 
    AND org.control_no = max.control_no

WHERE
    org.unused_id IS NULL
/*IF IsCommonLayout != null && IsCommonLayout */
AND org.fd_form_no IS NOT NULL
AND org.button_control_id != '-'
/*END*/
