WITH group_ex AS(
    SELECT
        sge.structure_group_id
        ,sge.sequence_no
        ,ie.extension_data
    FROM
        ms_structure_group_extension sge
        LEFT OUTER JOIN ms_structure ms
        ON  sge.data_type_structure_id = ms.structure_id
        LEFT OUTER JOIN ms_item_extension ie
        ON  ms.structure_item_id = ie.item_id
    WHERE
        sge.structure_group_id = @StructureGroupId
)
SELECT
    si.structure_id
    ,si.factory_id
    ,si.structure_group_id
    ,si.parent_structure_id
    ,si.structure_layer_no
    ,si.structure_item_id
    ,si.item_translation_id
    ,coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768) AS display_order
    ,si.location_structure_id
    ,si.language_id
    ,si.translation_text
    ,si.translation_item_description
    ,ie.item_id
    ,ie.sequence_no AS seq
    ,group_ex.extension_data AS data_type
    ,ie.extension_data AS ex_data
    --     , ie.note
    ,ie.insert_datetime
    ,ie.insert_user_id
    ,ie.update_datetime
    ,ie.update_user_id
FROM
    v_structure_item_all si
    LEFT OUTER JOIN ms_item_extension ie
    ON  si.structure_item_id = ie.item_id
    LEFT OUTER JOIN group_ex
    ON  si.structure_group_id = group_ex.structure_group_id
    AND ie.sequence_no = group_ex.sequence_no
    LEFT OUTER JOIN ms_structure_order AS order_factory
    ON  si.structure_id = order_factory.structure_id
    AND order_factory.factory_id = @FactoryId
    LEFT OUTER JOIN ms_structure_order AS order_common
    ON  si.structure_id = order_common.structure_id
    AND order_common.factory_id = 0
WHERE
    si.structure_group_id = @StructureGroupId
/*@Seq
    AND ie.sequence_no = @Seq
@Seq*/
/*@DataType
    AND group_ex.extension_data = @DataType
@DataType*/
/*@ExData
    AND ie.extension_data = @ExData
@ExData*/
/*@NotDeleteOnly
    AND si.delete_flg = 0
@NotDeleteOnly*/
/*@LanguageId
    AND si.language_id = @LanguageId
@LanguageId*/  