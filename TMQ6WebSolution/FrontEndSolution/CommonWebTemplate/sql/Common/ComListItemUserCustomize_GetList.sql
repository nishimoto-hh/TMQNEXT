--ユーザカスタマイズ情報
SELECT
         user_id AS USERID
        ,program_id AS PGMID
        ,form_no AS FORMNO
        ,control_group_id AS CTRLID
        ,control_no AS ITEMNO
        ,data_division AS DATA_DIVISION
        ,display_order AS DISPLAY_ORDER
        ,display_flg AS DISPLAY_FLG
FROM cm_control_user_customize
WHERE 
    delete_flg != 1
AND user_id = /*UserId*/1001
