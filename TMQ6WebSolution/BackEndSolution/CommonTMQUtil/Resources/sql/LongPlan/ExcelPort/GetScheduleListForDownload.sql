/*
 * 長期計画 機器別管理基準 ExcelPortダウンロード　保全項目に紐づく保全スケジュール詳細情報を取得する
 */
-- 保全項目ID
WITH management_standards_content_ids AS (
SELECT
    *
FROM
    STRING_SPLIT(@ManagementStandardsContentIdList, ',')
)

-- スケジュール文字列
,
mark AS(
SELECT
     CONVERT(int, item_ex1.extension_data) AS schedule_id
    ,tr.translation_text AS schedule
    ,CASE
        WHEN ms.factory_id = 0 THEN tr.location_structure_id    -- 構成マスタの工場IDが0の場合は翻訳マスタの場所階層ID(工場ID)を返す
        ELSE coalesce(ms.factory_id, 0)                         -- 上記以外は構成マスタの工場ID
     END AS factory_id
    
FROM
    ms_structure ms

-- 拡張項目1(ID)
LEFT JOIN ms_item_extension item_ex1
ON  ms.structure_item_id = item_ex1.item_id
AND item_ex1.sequence_no = 1

-- 拡張項目2(翻訳ID)
INNER JOIN ms_item_extension item_ex2
ON  ms.structure_item_id = item_ex2.item_id
AND item_ex2.sequence_no = 2

-- 翻訳
LEFT JOIN ms_translation tr
ON  CONVERT(int, item_ex2.extension_data) = tr.translation_id
AND tr.language_id = @LanguageId

WHERE
    ms.structure_group_id = 1860
)

SELECT 
     con.management_standards_content_id
    ,ms.maintainance_schedule_id
    ,md.maintainance_schedule_detail_id
    ,md.schedule_date
    ,md.complition
    ,mark.schedule_id
    ,mark.schedule
    
-- 機番情報
FROM 
    mc_machine mc

-- 場所階層
INNER JOIN ms_structure st_loc
ON  st_loc.structure_id = mc.location_structure_id 
AND st_loc.delete_flg != 1

-- 機器別管理基準部位
INNER JOIN mc_management_standards_component com
ON mc.machine_id = com.machine_id

-- 機器別管理基準内容
INNER JOIN mc_management_standards_content con
ON com.management_standards_component_id = con.management_standards_component_id

-- 保全スケジュール
INNER JOIN mc_maintainance_schedule ms
ON con.management_standards_content_id = ms.management_standards_content_id

-- 保全スケジュール詳細
INNER JOIN mc_maintainance_schedule_detail md
ON ms.maintainance_schedule_id = md.maintainance_schedule_id

-- スケジュール文字列
LEFT JOIN mark
ON CASE WHEN md.complition = 1 THEN 5 ELSE 1 END = mark.schedule_id
AND mark.factory_id IN (0, st_loc.factory_id)

WHERE
    EXISTS(
        SELECT * FROM management_standards_content_ids temp
        WHERE con.management_standards_content_id = temp.value)

AND
    com.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
   

ORDER BY 
      con.management_standards_content_id
     ,ms.maintainance_schedule_id
     ,md.schedule_date
