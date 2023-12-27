/*
 * 名称マスタ コンボ用データリスト用SQL.(機種別仕様選択項目用）
 * exparam1:アイテム拡張マスタの拡張データ
 * exparam2:アイテム拡張マスタの拡張データの値を構成アイテムIDに持つ、構成マスタの構成ID
 */
-- C0032
/*IF factoryIdList != null && factoryIdList.Count > 0*/
-- 工場IDで絞込を行う場合、指定された工場IDの表示順を取得する
-- 工場ID一覧を生成するための構成マスタより取得
WITH factory AS(
     SELECT
        st.structure_id AS factory_id
    FROM
        ms_structure AS st
    WHERE
        st.structure_id IN /*factoryIdList*/(0)
    UNION
    -- 共通工場は構成マスタに無いので追加
    SELECT
         0
)
/*END*/
select item.factory_id as factoryId,
       item.location_structure_id as translationFactoryId,
       item.structure_id as 'values',
       item.translation_text as labels,
       item_ex.extension_data as exparam1,
       ex_item_1.structure_id as exparam2,
       ex_item_1.translation_text as exparam3,
       item.delete_flg AS deleteFlg
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
        -- JavaScript側で画面の工場IDにより絞り込みを行うので、絞込用の工場IDと工場ID毎に表示順を取得する
        -- 表示順用工場ID
       ,coalesce(ft.factory_id, 0) AS orderFactoryId
       /*END*/
  from v_structure_item_all as item
       left outer join ms_item_extension as item_ex on (item.structure_item_id=item_ex.item_id and item_ex.sequence_no=/*param3*/0) 
       left outer join v_structure_item_all as  ex_item_1 on (item_ex.extension_data = CAST(ex_item_1.structure_item_id AS varchar) and item.language_id = ex_item_1.language_id)
       /*IF factoryIdList != null && factoryIdList.Count > 0*/
       -- 工場ごとに工場別表示順を取得する
       CROSS JOIN factory AS ft
       LEFT OUTER JOIN ms_structure_order AS order_factory
       ON  item.structure_id = order_factory.structure_id
       AND order_factory.factory_id = ft.factory_id
       /*END*/
       -- 全工場共通の表示順
       LEFT OUTER JOIN ms_structure_order AS order_common
       ON  item.structure_id = order_common.structure_id
       AND order_common.factory_id = 0
 where item.structure_group_id = /*param1*/0
   and item.language_id = /*languageId*/'ja'
   
/*IF factoryIdList != null && factoryIdList.Count > 0*/
   and item.factory_id in /*factoryIdList*/(0)
/*END*/

/*IF param2 != null && param2 != '' */
   and item.structure_layer_no = /*param2*/0
/*END*/

/*IF param4 != null && param4 != '' && param4 != 'null' */
   and CAST(item.factory_id AS VARCHAR) = /*param4*/''
/*END*/

/*IF factoryIdList != null && factoryIdList.Count > 0*/
   -- 工場別未使用標準アイテムに工場が含まれていないものを表示
   AND
       NOT EXISTS(
            SELECT
               *
           FROM
               ms_structure_unused AS unused
           WHERE
               unused.factory_id = ft.factory_id
           AND unused.structure_id = item.structure_id
       )
/*END*/
order by item.structure_group_id,item.structure_layer_no,
/*IF factoryIdList != null && factoryIdList.Count > 0*/
-- 工場ID毎の表示順
row_number() over(partition BY coalesce(ft.factory_id, 0) ORDER BY coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768), item.structure_id)
/*END*/
/*IF factoryIdList == null || factoryIdList.Count == 0*/
-- 工場IDの指定が無い場合、表示順は全工場共通(0)より取得
row_number() over(ORDER BY coalesce(order_common.display_order,32768), item.structure_id)
/*END*/

