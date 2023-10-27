/*
 * 名称マスタ コンボ用データリスト用SQL.(絞り込み条件付き、拡張データとの紐付け有）
 * C0001を拡張.
 * exparam1:アイテム拡張マスタの拡張データ
 */
-- C0030
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
       item_ex1.extension_data as exparam1,
       item.delete_flg AS deleteFlg
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
        -- JavaScript側で画面の工場IDにより絞り込みを行うので、絞込用の工場IDと工場ID毎に表示順を取得する
        -- 表示順用工場ID
       ,coalesce(ft.factory_id, 0) AS orderFactoryId
       /*END*/
  from v_structure_item_all as item
       left outer join ms_item_extension as item_ex1 on (item.structure_item_id=item_ex1.item_id and item_ex1.sequence_no=2) 
       left outer join ms_item_extension as item_ex2 on (item.structure_item_id=item_ex2.item_id and item_ex2.sequence_no=3) 
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
 where item.structure_group_id = /*param1*/2160
   and item.language_id = /*languageId*/'ja'
   
/*IF factoryIdList != null && factoryIdList.Count > 0*/
   and item.factory_id in /*factoryIdList*/(0)
/*END*/

/*IF param2 != null && param2 != '' */
   and item.structure_layer_no = /*param2*/0
/*END*/

/*IF param3 != null && (param3 == 'MS1001' || param3 == 'MS1010' || param3 == 'MS1040' || param3 == 'MS1760') */
-- 対象機能IDが以下の場合は拡張項目3(アイテム番号)=2の「標準マスタアイテム未使用設定」は非表示とする
-- MS1000:場所階層
-- MS1010:職種・機種
-- MS1040:予備品ﾛｹｰｼｮﾝ(予備品倉庫・棚)
-- MS1760:部門（工場・部門）
    and item_ex2.extension_data != '2'
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

