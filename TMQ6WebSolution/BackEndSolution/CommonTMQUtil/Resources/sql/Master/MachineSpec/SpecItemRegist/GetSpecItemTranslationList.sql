/*
* 選択肢登録画面
* 一覧と表示順、削除フラグを取得するSQL
*/
-- 言語一覧を取得
WITH lang AS(
     SELECT
         st.translation_text AS language_name
        ,ex.extension_data AS language_id
        ,COALESCE(COALESCE(od.display_order, od_com.display_order),st.structure_id) AS display_order
    FROM
        v_structure_item AS st
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                st.structure_item_id = ex.item_id
            AND ex.sequence_no = 1
            )
        LEFT OUTER JOIN
            ms_structure_order AS od
        ON  (
                od.structure_id = st.structure_id
            AND od.factory_id = @FactoryId
            )
        LEFT OUTER JOIN
            ms_structure_order AS od_com
        ON  (
                od_com.structure_id = st.structure_id
            AND od_com.factory_id = 0
            )
    WHERE
        st.structure_group_id = 9020
)
-- 選択肢の構成マスタより情報を取得
,st AS(
     SELECT
         -- 翻訳名称、説明
         tr.translation_text
        ,tr.translation_item_description
         -- 削除フラグ、表示順
        ,st.delete_flg
        ,od.display_order
         -- 構成マスタの排他チェック用情報
        ,st.structure_id
        ,st.update_serialid AS structure_upd_id
         -- アイテムマスタの排他チェック用情報
        ,it.item_id
        ,it.update_serialid AS item_upd_id
         -- 翻訳マスタの排他チェック用情報
        ,tr.location_structure_id
        ,tr.translation_id
        ,tr.language_id
        ,tr.update_serialid AS translation_upd_id
    FROM
        ms_structure AS st
        INNER JOIN
            ms_item AS it
        ON  (
                it.item_id = st.structure_item_id
            )
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                ex.item_id = it.item_id
            AND ex.sequence_no = 1
            )
        INNER JOIN
            ms_translation AS tr
        ON  (
                tr.location_structure_id = st.factory_id
            AND tr.translation_id = it.item_translation_id
            )
        INNER JOIN
            ms_structure_order AS od
        ON  (
                od.structure_id = st.structure_id
            AND od.factory_id = @FactoryId
            )
    WHERE
        st.structure_id = @StructureId
)
SELECT
     -- 一覧の項目
     lang.language_name
    ,st.translation_text
    ,st.translation_text AS translation_text_bk
    ,st.translation_item_description
     -- 画面下部の項目
    ,st.display_order
    ,st.delete_flg
     -- 構成マスタ排他チェック
    ,st.structure_id
    ,st.structure_upd_id
     -- 翻訳マスタ排他チェック
    ,lang.language_id
    ,@FactoryId AS location_structure_id
    ,st.translation_id
    ,st.translation_upd_id AS update_serialid
     -- アイテムマスタ排他チェック
    ,st.item_id
    ,st.item_upd_id
FROM
    lang
    LEFT OUTER JOIN
        st
    ON  (
            st.language_id = lang.language_id
        )
ORDER BY
    lang.display_order