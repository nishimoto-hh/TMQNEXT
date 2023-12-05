with factory_id_list as ( 
    select
        ms.factory_id 
    from
        ms_structure ms 
        inner join #temp_location tl 
            on ms.structure_id = tl.structure_id 
    where
        ms.structure_group_id = 1000 
        --and ms.structure_layer_no = 1
    union 
    select
        0
) 
, st_com( 
    structure_layer_no
    , structure_id
    , parent_structure_id
) as ( 
    select
        structure_layer_no
        , structure_id
        , parent_structure_id 
    from
        ms_structure 
    where
        delete_flg != 1
        and structure_group_id in (1000, 1010) 
        and factory_id in (select * from factory_id_list)
) 
, rec_down( 
    structure_layer_no
    , structure_id
    , parent_structure_id
) as ( 
    select
        * 
    from
        st_com 
    union all 
    select
        b.structure_layer_no
        , b.structure_id
        , b.parent_structure_id 
    from
        rec_down a 
        inner join ms_structure b 
            on a.structure_id = b.parent_structure_id 
            and b.delete_flg != 1
) 
, rec_up( 
    structure_layer_no
    , structure_id
    , parent_structure_id
) as ( 
    select
        * 
    from
        st_com 
    union all 
    select
        b.structure_layer_no
        , b.structure_id
        , b.parent_structure_id 
    from
        rec_up a 
        inner join ms_structure b 
            on b.structure_id = a.parent_structure_id 
            and b.delete_flg != 1
) 
, rec( 
    structure_layer_no
    , structure_id
    , parent_structure_id
) as ( 
    select
        * 
    from
        rec_up 
    union 
    select
        * 
    from
        rec_down
) 
,                                               -- 工場の表示順
factory_order as ( 
    select
        factory.factory_id
        , coalesce(order_common.display_order, 32768) as factory_order 
    from
        ms_structure as factory                 -- 表示順(工場共通)
        left outer join ms_structure_order as order_common 
            on ( 
                factory.structure_id = order_common.structure_id 
                and order_common.factory_id = 0
            ) 
    where
        factory.structure_group_id = 1000 
        and factory.structure_layer_no = 1 
        and factory.delete_flg != 1             
        and factory.factory_id in (select * from factory_id_list)
) 
, main as ( 
    select
        vs.structure_id as structureid
        , vs.factory_id as factoryid
        , vs.location_structure_id as locationstructureid
        , vs.structure_group_id as structuregroupid
        , vs.structure_layer_no as structurelayerno
        , vs.parent_structure_id as parentstructureid
        , vs.structure_item_id as structureitemid
        , coalesce(order_layer.display_order, 32768) as displayorder
        , vs.translation_text as translationtext
        , coalesce(fc_order.factory_order, 32768) as factory_order 
    from
        rec 
        inner join v_structure_item as vs 
            on rec.structure_id = vs.structure_id 
            and vs.language_id = @LanguageId           -- 工場の表示順を取得
        left outer join factory_order as fc_order 
            on (vs.factory_id = fc_order.factory_id) -- 翻訳を行わない場合
            -- 構成アイテムの表示順
        left outer join ms_structure_order as order_layer 
            on ( 
                vs.structure_id = order_layer.structure_id -- 場所階層の地区、工場のみ0、他は自身の工場
                and order_layer.factory_id = iif( 
                    vs.structure_layer_no < 2 
                    and vs.structure_group_id = 1000
                    , 0
                    , vs.factory_id
                )
            )                                   -- 予備品の共通倉庫用の共通工場を除くアイテムマスタ拡張の連番=3の拡張データが'1'
            -- 予備品のツリーを取得するときは、共通工場も表示
        left join ms_item_extension ie 
            on vs.structure_item_id = ie.item_id 
            and ie.sequence_no = 3 
    where
        1 = 1 
        and ( 
            ie.extension_data is null 
            or ie.extension_data != '1'
        )                                       --ルート要素の名称を構成グループマスタから取得
        union all 
    select
        - 1 as structureid
        , 0 as factoryid
        , 0 as locationstructureid
        , sg.structure_group_id as structuregroupid
        , - 1 as structurelayerno
        , - 1 as parentstructureid
        , - 1 as structureitemid
        , 1 as displayorder
        , mt.translation_text as translationtext
        , - 1 as factory_order 
    from
        ms_structure_group sg 
        left join ms_translation mt 
            on sg.structure_group_translation_id = mt.translation_id 
    where
        sg.delete_flg != 1 
        and mt.delete_flg = 0 
        and sg.structure_group_id in (1000, 1010) 
        and mt.language_id = @LanguageId
) 
select
    * 
from
    main 
where
    structurelayerno = 0 
    and structuregroupid = 1010 
order by
    structuregroupid
    , factory_order
    , displayorder
    , structureid
