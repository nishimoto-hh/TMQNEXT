WITH DateFormat AS --「yyyy年MM月dd日」の翻訳を取得
(
SELECT
    tra.translation_text
FROM
    ms_translation tra
WHERE
    tra.location_structure_id = 0
AND tra.translation_id = 150000014
AND tra.language_id = (SELECT DISTINCT languageId FROM #temp)
)
SELECT
    ma.job_structure_id                                             -- 職種機種階層id ※共通処理にて使用
    , '' AS large_classfication_name                                -- 機種大分類 ※共通処理にて設定
    , '' AS middle_classfication_name                               -- 機種中分類 ※共通処理にて設定
    , '' AS small_classfication_name                                -- 機種小分類 ※共通処理にて設定
	, '' AS model_name                                              -- 機種名称
    , ma.machine_no AS machine_no                                   -- 機器番号
    , ma.machine_name AS machine_name                               -- 機器名称
    ,(
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ma.importance_structure_id
                    AND st_f.factory_id IN (0, ma.location_factory_structure_id)
            )
            AND tra.structure_id = ma.importance_structure_id
    ) AS importance_name -- 重要度名称   
    ,(
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ma.conservation_structure_id
                    AND st_f.factory_id IN (0, ma.location_factory_structure_id)
            )
            AND tra.structure_id = ma.conservation_structure_id
    ) AS conservation_name -- 保全方式名称 
    , ROW_NUMBER() OVER(
        ORDER BY
		    msc.order_no
            , mcp.inspection_site_structure_id
            , msc.inspection_content_structure_id
      ) AS number_name                                              -- NO
    , ma.machine_no AS inspection_site_machine_no                   -- 部位機器番号
    , ma.machine_name AS inspection_site_machine_name               -- 部位機器名称
    ,(
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = mcp.inspection_site_structure_id
                    AND st_f.factory_id IN (0, ma.location_factory_structure_id)
            )
            AND tra.structure_id = mcp.inspection_site_structure_id
    ) AS inspection_site_name -- 部位名称 
    ,(
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = msc.inspection_site_importance_structure_id
                    AND st_f.factory_id IN (0, ma.location_factory_structure_id)
            )
            AND tra.structure_id = msc.inspection_site_importance_structure_id
    ) AS inspection_site_importance_name -- 部位重要度名称 
    ,(
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = msc.inspection_site_conservation_structure_id
                    AND st_f.factory_id IN (0, ma.location_factory_structure_id)
            )
            AND tra.structure_id = msc.inspection_site_conservation_structure_id
    ) AS inspection_site_conservation_name -- 点検グレード名称
	, CASE WHEN ex.extension_data = '1' THEN
            (
                SELECT
                    tra.translation_text 
                FROM
                    v_structure_item_all AS tra 
                WHERE
                    tra.language_id = temp.languageId
                    AND tra.location_structure_id = ( 
                        SELECT
                            MAX(st_f.factory_id) 
                        FROM
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = msc.inspection_content_structure_id
                            AND st_f.factory_id IN (0, ma.location_factory_structure_id)
                    )
                    AND tra.structure_id = msc.inspection_content_structure_id
            )
		   ELSE null
	  END AS diagnosis_content                                      -- 定期検査・診断 内容
	, CASE WHEN ex.extension_data = '1' THEN FORMAT(msc.budget_amount, 'N0') 
		   ELSE null
	  END AS diagnosis_budget_amount                                -- 定期検査・診断 予算金額
	, CASE WHEN ex.extension_data = '1' THEN
            (
                SELECT
                    tra.translation_text 
                FROM
                    v_structure_item_all AS tra 
                WHERE
                    tra.language_id = temp.languageId
                    AND tra.location_structure_id = ( 
                        SELECT
                            MAX(st_f.factory_id) 
                        FROM
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = msc.maintainance_kind_structure_id
                            AND st_f.factory_id IN (0, ma.location_factory_structure_id)
                    )
                    AND tra.structure_id = msc.maintainance_kind_structure_id
            )
		   ELSE null
	  END AS diagnosis_inspection_type                              -- 定期検査・診断 検査種別
    , '' AS  diagnosis_legal_division                               -- 定期検査・診断 法定区分
	, CASE WHEN ex.extension_data = '1' THEN ms.disp_cycle
		   ELSE null
	  END AS diagnosis_cycle                                        -- 定期検査・診断 周期
	, CASE WHEN ex.extension_data = '2' THEN
            (
                SELECT
                    tra.translation_text 
                FROM
                    v_structure_item_all AS tra 
                WHERE
                    tra.language_id = temp.languageId
                    AND tra.location_structure_id = ( 
                        SELECT
                            MAX(st_f.factory_id) 
                        FROM
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = msc.inspection_content_structure_id
                            AND st_f.factory_id IN (0, ma.location_factory_structure_id)
                    )
                    AND tra.structure_id = msc.inspection_content_structure_id
            )
		   ELSE null
	  END AS maintenance_contents                                   -- 定期修理・整備 内容
	, CASE WHEN ex.extension_data = '2' THEN FORMAT(msc.budget_amount, 'N0') 
		   ELSE null
	  END AS maintenance_budget_amount                              -- 定期修理・整備 予算金額
	, CASE WHEN ex.extension_data = '2' THEN
            (
                SELECT
                    tra.translation_text 
                FROM
                    v_structure_item_all AS tra 
                WHERE
                    tra.language_id = temp.languageId
                    AND tra.location_structure_id = ( 
                        SELECT
                            MAX(st_f.factory_id) 
                        FROM
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = msc.maintainance_kind_structure_id
                            AND st_f.factory_id IN (0, ma.location_factory_structure_id)
                    )
                    AND tra.structure_id = msc.maintainance_kind_structure_id
            )
		   ELSE null
	  END AS maintenance_inspection_type                            -- 定期修理・整備 検査種別
    , '' AS  maintenance_legal_division                             -- 定期検査・診断 法定区分
	, CASE WHEN ex.extension_data = '2' THEN ms.disp_cycle
		   ELSE null
	  END AS maintenance_cycle                                      -- 定期修理・整備 周期
	, CASE WHEN ex.extension_data = '3' THEN
            (
                SELECT
                    tra.translation_text 
                FROM
                    v_structure_item_all AS tra 
                WHERE
                    tra.language_id = temp.languageId
                    AND tra.location_structure_id = ( 
                        SELECT
                            MAX(st_f.factory_id) 
                        FROM
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = msc.inspection_content_structure_id
                            AND st_f.factory_id IN (0, ma.location_factory_structure_id)
                    )
                    AND tra.structure_id = msc.inspection_content_structure_id
            )
		   ELSE null
	  END AS daily_contents                                         -- 日常点検Co-Mo 内容
	, CASE WHEN ex.extension_data = '3' THEN ms.disp_cycle
		   ELSE null
	  END AS daily_cycle                                            -- 日常点検Co-Mo 周期
     , '' AS remark                                                 -- 備考
	 , FORMAT(GETDATE(),DateFormat.translation_text) AS output_date			-- 出力日付
FROM
    mc_management_standards_component mcp                           -- 機器別管理基準部位
    , mc_management_standards_content msc                           -- 機器別管理基準内容
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
	, (SELECT it.structure_id,ex.extension_data
	  FROM ms_structure it,
	       ms_item_extension ex
	  WHERE it.structure_item_id = ex.item_id
	  AND it.structure_group_id = 1230
	  AND ex.extension_data IN (1,2,3)-- 保全区分が定期検査or定期修理or日常点検のみ表示
	 ) ex -- 構成マスタ
    CROSS JOIN
       DateFormat -- 日付フォーマット「yyyy年MM月dd日」の翻訳
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



