/*
* ロット情報更新SQL
* UpdateLot.sql
*/
-- 修理部門の構成IDリスト
WITH fix AS(
    SELECT
        st.structure_id
    FROM
        v_structure AS st
        INNER JOIN
            ms_item_extension AS ex
        ON  st.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
        AND ex.extension_data = '1'
    WHERE
        st.structure_group_id = 1760
)
-- 更新する部門IDが修理部門かどうか判定、1なら修理部門、0なら違う
,is_update_price AS(
    SELECT
        COUNT(*) AS cnt
    FROM
        fix
    WHERE
        fix.structure_id = @DepartmentStructureId
)
-- 更新
UPDATE
    lot
SET
     department_structure_id = @DepartmentStructureId
    ,account_structure_id = @AccountStructureId
    ,management_division = @ManagementDivision
    ,management_no = @ManagementNo
    -- 単価は部門が修理部門の場合、指定された値で更新、異なる場合は更新しない(元の値で更新)
    ,lot.unit_price = CASE (SELECT cnt FROM is_update_price)
                      WHEN 1 THEN @UnitPrice
                      ELSE lot.unit_price
                      END
    ,update_serialid = update_serialid + 1
    ,update_datetime = @UpdateDatetime
    ,update_user_id = @UpdateUserId
FROM
    pt_lot AS lot
WHERE
    lot.lot_control_id = @LotControlId
