SELECT
    it_ex.extension_data
FROM
    ms_structure AS st
    INNER JOIN
        ms_item AS it
    ON  (
            st.structure_item_id = it.item_id
        )
    LEFT OUTER JOIN
        ms_item_extension AS it_ex
    ON  (
            st.structure_item_id = it_ex.item_id
        )
    LEFT OUTER JOIN
        ms_structure_order AS order_common
    ON  (
            st.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
        )

WHERE
    st.structure_group_id = @StructureGroupId
AND it_ex.sequence_no = 1
-- 削除を含む場合は2未満、含まない場合は1未満
AND st.delete_flg < @DeleteFlg

ORDER BY
    coalesce( order_common.display_order, 32768)
   ,st.structure_id
