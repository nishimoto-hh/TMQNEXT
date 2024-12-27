-- 場所階層IDより工場を特定し、その工場が変更履歴の対象工場か判定するSQL
select
    ex.extension_data
from
    ms_structure ms 
    left join ms_item_extension ex 
        on ms.structure_item_id = ex.item_id 
        and ex.sequence_no = 4 
where
    ms.structure_id = ( 
        select
            ms2.factory_id 
        from
            ms_structure ms2 
        where
            ms2.structure_id = @LocationStructureId
    )
