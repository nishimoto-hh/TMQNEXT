-- ‹¤—Lƒƒ‚ƒŠ•Û‘ÎÛSQLID
SELECT value AS SqlId
FROM ms_structure ms
INNER JOIN ms_item mi
ON ms.structure_item_id = mi.item_id
INNER JOIN ms_item_extension ex
ON mi.item_id = ex.item_id
AND ex.sequence_no = 1
CROSS APPLY STRING_SPLIT(ex.extension_data, ',')
WHERE ms.structure_group_id = 9330  -- ‹¤—Lƒƒ‚ƒŠ•Û‘ÎÛSQLID
AND ms.delete_flg != 1
