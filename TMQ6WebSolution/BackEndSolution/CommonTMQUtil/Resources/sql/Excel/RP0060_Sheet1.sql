SELECT
    ma.job_structure_id                                             -- 職種機種階層id ※共通処理にて使用
    , '' AS large_classfication_name                                -- 機種大分類 ※共通処理にて設定
    , '' AS middle_classfication_name                               -- 機種中分類 ※共通処理にて設定
    , '' AS small_classfication_name                                -- 機種小分類 ※共通処理にて設定
	, '' AS model_name                                              -- 機種名称
    , ma.machine_no AS machine_no                                   -- 機器番号
    , ma.machine_name AS machine_name                               -- 機器名称
--    , vimp.translation_text AS importance_name                      -- 重要度名称
--    , vcns.translation_text AS conservation_name                    -- 保全方式名称
    , [dbo].[get_v_structure_item](ma.importance_structure_id, temp.factoryId, temp.languageId) AS importance_name            -- 重要度名称
    , [dbo].[get_v_structure_item](ma.conservation_structure_id, temp.factoryId, temp.languageId) AS conservation_name        -- 保全方式名称
    , ROW_NUMBER() OVER(
        ORDER BY
		    msc.order_no
            , mcp.inspection_site_structure_id
            , msc.inspection_content_structure_id
      ) AS number_name                                              -- NO
    , ma.machine_no AS inspection_site_machine_no                   -- 部位機器番号
    , ma.machine_name AS inspection_site_machine_name               -- 部位機器名称
--    , viss.translation_text AS inspection_site_name                 -- 部位名称
--    , visi.translation_text AS inspection_site_importance_name      -- 部位重要度名称
--    , visc.translation_text AS inspection_site_conservation_name    -- 点検グレード名称
    , [dbo].[get_v_structure_item](mcp.inspection_site_structure_id, temp.factoryId, temp.languageId) AS inspection_site_name                              -- 部位名称
    , [dbo].[get_v_structure_item](msc.inspection_site_importance_structure_id, temp.factoryId, temp.languageId) AS inspection_site_importance_name        -- 部位重要度名称
    , [dbo].[get_v_structure_item](msc.inspection_site_conservation_structure_id, temp.factoryId, temp.languageId) AS inspection_site_conservation_name    -- 点検グレード名称
--	, CASE WHEN ex.extension_data = '1' THEN vcon.translation_text
	, CASE WHEN ex.extension_data = '1' THEN [dbo].[get_v_structure_item](msc.inspection_content_structure_id, temp.factoryId, temp.languageId)
		   ELSE null
	  END AS diagnosis_content                                      -- 定期検査・診断 内容
	, CASE WHEN ex.extension_data = '1' THEN FORMAT(msc.budget_amount, 'N0') 
		   ELSE null
	  END AS diagnosis_budget_amount                                -- 定期検査・診断 予算金額

--	, CASE WHEN ex.extension_data = '1' THEN vmnk.translation_text
	, CASE WHEN ex.extension_data = '1' THEN [dbo].[get_v_structure_item](msc.maintainance_kind_structure_id, temp.factoryId, temp.languageId)
		   ELSE null
	  END AS diagnosis_inspection_type                              -- 定期検査・診断 検査種別
    , '' AS  diagnosis_legal_division                               -- 定期検査・診断 法定区分
	, CASE WHEN ex.extension_data = '1' THEN ms.disp_cycle
		   ELSE null
	  END AS diagnosis_cycle                                        -- 定期検査・診断 周期
--	, CASE WHEN ex.extension_data = '2' THEN vcon.translation_text
	, CASE WHEN ex.extension_data = '2' THEN [dbo].[get_v_structure_item](msc.inspection_content_structure_id, temp.factoryId, temp.languageId)
		   ELSE null
	  END AS maintenance_contents                                   -- 定期修理・整備 内容
	, CASE WHEN ex.extension_data = '2' THEN FORMAT(msc.budget_amount, 'N0') 
		   ELSE null
	  END AS maintenance_budget_amount                              -- 定期修理・整備 予算金額
--	, CASE WHEN ex.extension_data = '2' THEN vmnk.translation_text
	, CASE WHEN ex.extension_data = '2' THEN [dbo].[get_v_structure_item](msc.maintainance_kind_structure_id, temp.factoryId, temp.languageId)
		   ELSE null
	  END AS maintenance_inspection_type                            -- 定期修理・整備 検査種別
    , '' AS  maintenance_legal_division                             -- 定期検査・診断 法定区分
	, CASE WHEN ex.extension_data = '2' THEN ms.disp_cycle
		   ELSE null
	  END AS maintenance_cycle                                      -- 定期修理・整備 周期
--	, CASE WHEN ex.extension_data = '3' THEN vcon.translation_text
	, CASE WHEN ex.extension_data = '3' THEN [dbo].[get_v_structure_item](msc.inspection_content_structure_id, temp.factoryId, temp.languageId)
		   ELSE null
	  END AS daily_contents                                         -- 日常点検Co-Mo 内容
	, CASE WHEN ex.extension_data = '3' THEN ms.disp_cycle
		   ELSE null
	  END AS daily_cycle                                            -- 日常点検Co-Mo 周期
     , '' AS remark                                                 -- 備考
	 , FORMAT(GETDATE(),'yyyy年MM月dd日') AS output_date			-- 出力日付
FROM
    mc_management_standards_component mcp                           -- 機器別管理基準部位
--    LEFT JOIN v_structure_item viss                                 -- 部位名称
--        ON mcp.inspection_site_structure_id = viss.structure_id
--    LEFT JOIN v_structure_item visi                                 -- 部位重要度
--        ON mcp.inspection_site_importance_structure_id = visi.structure_id
--    LEFT JOIN v_structure_item visc                                 -- 点検グレード
--        ON mcp.inspection_site_conservation_structure_id = visc.structure_id
    , mc_management_standards_content msc                           -- 機器別管理基準内容
--    LEFT JOIN v_structure_item vcon                                 -- 内容
--        ON msc.inspection_content_structure_id = vcon.structure_id
--    LEFT JOIN v_structure_item vmnk                                 -- 検査種別
--        ON msc.maintainance_kind_structure_id = vmnk.structure_id
    ,(SELECT a.*
	 		FROM mc_maintainance_schedule AS a
	 		INNER JOIN
	 		-- 機器別管理基準内容IDごとの開始日最新データを取得
	 		(SELECT management_standards_content_id,
	 				MAX(start_date) AS start_date,
					MAX(update_datetime) AS update_datetime
	 		 FROM mc_maintainance_schedule
	 		 GROUP BY management_standards_content_id
	 		) b
	 		ON a.management_standards_content_id = b.management_standards_content_id
	 		AND (a.start_date = b.start_date 
	 			 OR a.start_date IS NULL AND b.start_date IS NULL --null結合を考慮
	 			)
	 		AND (a.update_datetime = b.update_datetime 
	 			 OR a.update_datetime IS NULL AND b.update_datetime IS NULL --null結合を考慮
	 			)
	 ) ms                                                           -- 保全スケジュール
    , mc_machine ma                                                 -- 機番情報
    INNER JOIN #temp temp
        ON ma.machine_id = temp.Key1
--    LEFT JOIN v_structure_item vimp                                 -- 重要度
--        ON ma.importance_structure_id = vimp.structure_id
--    LEFT JOIN v_structure_item vcns                                 -- 保全方式
--        ON ma.conservation_structure_id = vcns.structure_id
	, (SELECT it.structure_id,ex.extension_data
	  FROM ms_structure it,
	       ms_item_extension ex
	  WHERE it.structure_item_id = ex.item_id
	  AND it.structure_group_id = 1230
	  AND ex.extension_data IN (1,2,3)-- 保全区分が定期検査or定期修理or日常点検のみ表示
	 ) ex -- 構成マスタ
WHERE
    mcp.management_standards_component_id = msc.management_standards_component_id
AND msc.management_standards_content_id = ms.management_standards_content_id
AND mcp.machine_id = ma.machine_id
AND msc.maintainance_division = ex.structure_id -- 保全区分 in (定期検査,定期修理,日常点検Como)それ以外は表示する必要なしの為、内部結合
AND mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
ORDER BY
    msc.order_no
    , mcp.inspection_site_structure_id
    , msc.inspection_content_structure_id



