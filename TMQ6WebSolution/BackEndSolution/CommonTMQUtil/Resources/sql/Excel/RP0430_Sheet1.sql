/*@LocationStructureIds
DROP TABLE IF EXISTS #temp_condition_location; 
CREATE TABLE #temp_condition_location(structure_id int);
INSERT 
INTO #temp_condition_location 
SELECT
    * 
FROM
    STRING_SPLIT(@LocationStructureIds, ',');
@LocationStructureIds*/
/*@JobStructureIds
DROP TABLE IF EXISTS #temp_condition_job; 
CREATE TABLE #temp_condition_job(structure_id int); 
INSERT 
INTO #temp_condition_job 
SELECT
    * 
FROM
    STRING_SPLIT(@JobStructureIds, ',');
@JobStructureIds*/

DROP TABLE IF EXISTS #temp_target_data; 
CREATE TABLE #temp_target_data( 
    long_plan_id bigint
); 
-- 画面で指定された条件で出力対象の長計件名IDを検索
INSERT INTO #temp_target_data
SELECT DISTINCT
    hlp.long_plan_id 
FROM
    hm_ln_long_plan AS hlp 
WHERE
    1 = 1
    /*@Subject
    AND hlp.subject LIKE @Subject
    @Subject*/
    /*@LocationStructureIds
    AND EXISTS ( 
        SELECT
            * 
        FROM
            #temp_condition_location temp 
        WHERE
            hlp.location_structure_id = temp.structure_id
    ) 
    @LocationStructureIds*/
    /*@JobStructureIds
    AND EXISTS ( 
        SELECT
            * 
        FROM
            #temp_condition_job temp 
        WHERE
            hlp.job_structure_id = temp.structure_id
    ) 
    @JobStructureIds*/
    /*@SubjectNote
    AND hlp.subject_note LIKE @SubjectNote
    @SubjectNote*/
    /*@PersonName
    AND hlp.person_name LIKE @PersonName
    @PersonName*/
    /*@WorkItemStructureIdList
    AND hlp.work_item_structure_id IN @WorkItemStructureIdList
    @WorkItemStructureIdList*/
    /*@BudgetManagementStructureIdList
    AND hlp.budget_management_structure_id IN @BudgetManagementStructureIdList
    @BudgetManagementStructureIdList*/
    /*@BudgetPersonalityStructureIdList
    AND hlp.budget_personality_structure_id IN @BudgetPersonalityStructureIdList
    @BudgetPersonalityStructureIdList*/
    /*@MaintenanceSeasonStructureIdList
    AND hlp.maintenance_season_structure_id IN @MaintenanceSeasonStructureIdList
    @MaintenanceSeasonStructureIdList*/
    /*@PurposeStructureIdList
    AND hlp.purpose_structure_id IN @PurposeStructureIdList
    @PurposeStructureIdList*/
    /*@WorkClassStructureIdList
    AND hlp.work_class_structure_id IN @WorkClassStructureIdList
    @WorkClassStructureIdList*/
    /*@TreatmentStructureIdList
    AND hlp.treatment_structure_id IN @TreatmentStructureIdList
    @TreatmentStructureIdList*/
    /*@FacilityStructureIdList
    AND hlp.facility_structure_id IN @FacilityStructureIdList
    @FacilityStructureIdList*/
;

DROP TABLE IF EXISTS #temp_structure_layer; 
CREATE TABLE #temp_structure_layer( 
    structure_id INT
    , translation_text nvarchar(400) COLLATE Japanese_CI_AS_KS
    , structure_group_id INT
    , structure_layer_no INT
    , org_structure_id INT
); 

WITH st_com( 
    structure_layer_no
    , structure_id
    , parent_structure_id
    , org_structure_id
) AS ( 
    SELECT
        st.structure_layer_no
        , st.structure_id
        , st.parent_structure_id
        , st.structure_id 
    FROM
        ms_structure AS st 
    WHERE
        EXISTS ( 
            SELECT
                * 
            FROM
                hm_ln_long_plan AS hlp 
            WHERE
                EXISTS ( 
                    SELECT
                        * 
                    FROM
                        #temp_target_data td 
                    WHERE
                        td.long_plan_id = hlp.long_plan_id
                ) 
                AND ( 
                    st.structure_id = hlp.location_structure_id 
                    OR st.structure_id = hlp.job_structure_id
                )
        )
) 
, rec_up( 
    structure_layer_no
    , structure_id
    , parent_structure_id
    , org_structure_id
) AS ( 
    SELECT
        st.structure_layer_no
        , st.structure_id
        , st.parent_structure_id
        , st.structure_id 
    FROM
        st_com AS st 
    UNION ALL 
    SELECT
        b.structure_layer_no
        , b.structure_id
        , b.parent_structure_id
        , a.org_structure_id 
    FROM
        rec_up a 
        INNER JOIN ms_structure b 
            ON (b.structure_id = a.parent_structure_id)
) 
-- 場所階層、職種機種の各階層情報を取得
INSERT INTO #temp_structure_layer
SELECT
    vs.structure_id
    , vs.translation_text
    , vs.structure_group_id
    , vs.structure_layer_no
    , up.org_structure_id 
FROM
    rec_up AS up 
    LEFT OUTER JOIN v_structure_item_all AS vs 
        ON (up.structure_id = vs.structure_id) 
WHERE
    vs.language_id = @LanguageId
;

DROP TABLE IF EXISTS #temp_history_data; 
CREATE TABLE #temp_history_data( 
    long_plan_id bigint
    , history_management_id bigint
    , history_order bigint
    , division_cd nvarchar(400) COLLATE Japanese_CI_AS_KS
    , status nvarchar(400) COLLATE Japanese_CI_AS_KS
    , division nvarchar(400) COLLATE Japanese_CI_AS_KS
    , subject nvarchar(400) COLLATE Japanese_CI_AS_KS
    , subject_note nvarchar(400) COLLATE Japanese_CI_AS_KS
    , person_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , district_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , factory_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , plant_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , series_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , stroke_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , facility_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , job_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , large_classfication_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , middle_classfication_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , small_classfication_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , work_item_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , budget_management_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , budget_personality_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , maintenance_season_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , purpose_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , work_class_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , treatment_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , facility_division_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , row_num bigint
    , application_reason nvarchar(400) COLLATE Japanese_CI_AS_KS
    , application_user_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , application_date DATE
    , approval_user_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , approval_date DATE
    , factory_id INT
); 
WITH division_info AS ( 
    -- 申請区分ごとの構成IDとコードを取得(10:新規、20：変更、30：削除)
    SELECT
        st.structure_id
        , ie.extension_data AS division_cd 
    FROM
        v_structure AS st 
        INNER JOIN ms_item_extension AS ie 
            ON (st.structure_item_id = ie.item_id) 
    WHERE
        st.structure_group_id = 2100 
        AND ie.sequence_no = 1
) 
, structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1060, 1180, 1220, 1280, 1290, 1300, 1310, 1320, 1330, 1410) 
        AND language_id = @LanguageId
) 
-- 対象の長期計画件名の全ての変更履歴を表示
INSERT INTO #temp_history_data
SELECT
    hlp.long_plan_id
    , hm.history_management_id
    , row_number() OVER ( 
        PARTITION BY
            hlp.long_plan_id 
        ORDER BY
            hlp.history_management_id
    ) AS history_order                      -- 件名ごとに何番目の変更管理かを採番、変更前後の比較に使用
    , div.division_cd                       -- 申請区分のコード
    , ( 
        SELECT
            translation_text 
        FROM
            v_structure_item 
        WHERE
            structure_id = hm.application_status_id 
            AND language_id = @LanguageId 
            AND factory_id = 0
    ) AS status                             -- 申請状況の翻訳取得、縦持ちになったら行数が増えるためここで取得
    , ( 
        SELECT
            translation_text 
        FROM
            v_structure_item 
        WHERE
            structure_id = hm.application_division_id 
            AND language_id = @LanguageId 
            AND factory_id = 0
    ) AS division                           -- 申請区分の翻訳取得、縦持ちになったら行数が増えるためここで取得
    , hlp.subject
    , hlp.subject_note
    , hlp.person_name
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hlp.location_structure_id 
            AND st.structure_layer_no = 0
    ) AS district_name                      --地区
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hlp.location_structure_id 
            AND st.structure_layer_no = 1
    ) AS factory_name                       --工場
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hlp.location_structure_id 
            AND st.structure_layer_no = 2
    ) AS plant_name                         --プラント
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hlp.location_structure_id 
            AND st.structure_layer_no = 3
    ) AS series_name                        --系列
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hlp.location_structure_id 
            AND st.structure_layer_no = 4
    ) AS stroke_name                        --工程
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hlp.location_structure_id 
            AND st.structure_layer_no = 5
    ) AS facility_name                      --設備
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hlp.job_structure_id 
            AND st.structure_layer_no = 0
    ) AS job_name                           --職種
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hlp.job_structure_id 
            AND st.structure_layer_no = 1
    ) AS large_classfication_name           --機種大分類
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hlp.job_structure_id 
            AND st.structure_layer_no = 2
    ) AS middle_classfication_name          --機種中分類
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hlp.job_structure_id 
            AND st.structure_layer_no = 3
    ) AS small_classfication_name           --機種小分類
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hlp.work_item_structure_id 
                    AND st_f.factory_id IN (0, hlp.factory_id)
            ) 
            AND tra.structure_id = hlp.work_item_structure_id
    ) AS work_item_name                     --作業項目(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hlp.budget_management_structure_id 
                    AND st_f.factory_id IN (0, hlp.factory_id)
            ) 
            AND tra.structure_id = hlp.budget_management_structure_id
    ) AS budget_management_name             --予算管理区分(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hlp.budget_personality_structure_id 
                    AND st_f.factory_id IN (0, hlp.factory_id)
            ) 
            AND tra.structure_id = hlp.budget_personality_structure_id
    ) AS budget_personality_name            --予算性格区分(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hlp.maintenance_season_structure_id 
                    AND st_f.factory_id IN (0, hlp.factory_id)
            ) 
            AND tra.structure_id = hlp.maintenance_season_structure_id
    ) AS maintenance_season_name            --保全時期(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hlp.purpose_structure_id 
                    AND st_f.factory_id IN (0, hlp.factory_id)
            ) 
            AND tra.structure_id = hlp.purpose_structure_id
    ) AS purpose_name                       --目的区分(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hlp.work_class_structure_id 
                    AND st_f.factory_id IN (0, hlp.factory_id)
            ) 
            AND tra.structure_id = hlp.work_class_structure_id
    ) AS work_class_name                    --作業区分(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hlp.treatment_structure_id 
                    AND st_f.factory_id IN (0, hlp.factory_id)
            ) 
            AND tra.structure_id = hlp.treatment_structure_id
    ) AS treatment_name                     --処置区分(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hlp.facility_structure_id 
                    AND st_f.factory_id IN (0, hlp.factory_id)
            ) 
            AND tra.structure_id = hlp.facility_structure_id
    ) AS facility_division_name             --設備区分(翻訳)
    , row_number() OVER ( 
        ORDER BY
            IIF(hm.application_date IS NOT NULL, 0, 1)
            , hm.application_date
            , hlp.long_plan_id
            , hm.history_management_id
    ) AS row_num                            --変更管理ID単位で採番する番号
    , hm.application_reason
    , hm.application_user_name
    , hm.application_date
    , hm.approval_user_name
    , hm.approval_date 
    , hlp.factory_id
FROM
    ( 
        SELECT
            hlp.*
            , ( 
                SELECT
                    st.structure_id 
                FROM
                    #temp_structure_layer st 
                WHERE
                    st.org_structure_id = hlp.location_structure_id 
                    AND st.structure_layer_no = 1
            ) AS factory_id                 --工場ID
        FROM
            hm_ln_long_plan hlp
    ) AS hlp 
    INNER JOIN hm_history_management AS hm 
        ON ( 
            hm.history_management_id = hlp.history_management_id
        ) 
    INNER JOIN division_info AS div 
        ON (div.structure_id = hm.application_division_id) -- 出力対象の件名のみ
WHERE
    EXISTS ( 
        SELECT
            * 
        FROM
            #temp_target_data td 
        WHERE
            td.long_plan_id = hlp.long_plan_id
    )
    /*@ApplicationStatusIdList
    AND hm.application_status_id IN @ApplicationStatusIdList
    @ApplicationStatusIdList*/
    /*@ApplicationDivisionIdList
    AND hm.application_division_id IN @ApplicationDivisionIdList
    @ApplicationDivisionIdList*/
    /*@ApplicationUserName
    AND hm.application_user_name LIKE @ApplicationUserName
    @ApplicationUserName*/
    /*@ApplicationDateFrom
    AND hm.application_date >= @ApplicationDateFrom
    @ApplicationDateFrom*/
    /*@ApplicationDateTo
    AND hm.application_date <= @ApplicationDateTo
    @ApplicationDateTo*/
    /*@ApprovalUserName
    AND hm.approval_user_name LIKE @ApprovalUserName
    @ApprovalUserName*/
    /*@ApprovalDateFrom
    AND hm.approval_date >= @ApprovalDateFrom
    @ApprovalDateFrom*/
    /*@ApprovalDateTo
    AND hm.approval_date <= @ApprovalDateTo
    @ApprovalDateTo*/
    /*@ApplicationReason
    AND hm.application_reason LIKE @ApplicationReason
    @ApplicationReason*/
;

WITH report_col_info_temp AS ( 
    -- 帳票出力項目の構成マスタの定義を取得
    -- 連番1と2の拡張項目の値を使用するので両方取得して、次のサブクエリで使用する作業用
    SELECT
        st.structure_id
        , st.translation_text AS col_name
        , ie.extension_data AS ex_data
        , ie.sequence_no 
    FROM
        v_structure_item AS st 
        INNER JOIN ms_item_extension AS ie 
            ON (st.structure_item_id = ie.item_id) 
    WHERE
        st.structure_group_id = 2150 
        AND st.language_id = @LanguageId 
        AND st.factory_id = 0
) 
, report_col_info AS ( 
    -- 帳票出力対象の列の定義を取得(機器台帳or長期計画)
    -- 作業用の帳票出力項目定義を取得するために連番1で機能を絞り、連番2の表示順を取得
    SELECT
        rci.col_name
        , CONVERT(INT, rci.ex_data) AS col_order 
    FROM
        report_col_info_temp AS rci 
    WHERE
        EXISTS ( 
            SELECT
                * 
            FROM
                report_col_info_temp AS temp 
            WHERE
                temp.structure_id = rci.structure_id 
                AND temp.sequence_no = 1 
                AND temp.ex_data = '2'
        ) 
        AND rci.sequence_no = 2
) 
, structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1060, 1180, 1220, 1280, 1290, 1300, 1310, 1320, 1330, 1410) 
        AND language_id = @LanguageId
) 
, history_data_vertical AS ( 
    -- 項目ごとの縦持ちに変更
    @HistoryVertical 
)                                               
, t_long_plan AS (
    -- 長計件名と工場IDを取得
    SELECT
        lp.*
        , ( 
            SELECT
                st.structure_id 
            FROM
                #temp_structure_layer st 
            WHERE
                st.org_structure_id = lp.location_structure_id 
                AND st.structure_layer_no = 1
        ) AS factory_id                         --工場ID
    FROM
        ln_long_plan lp
) 
, history_content AS ( 
    -- 機器別管理基準内容変更管理の最新を取得
    SELECT
        con.* 
    FROM
        hm_history_management hm 
        LEFT JOIN hm_mc_management_standards_content con 
            ON hm.history_management_id = con.history_management_id 
    WHERE
        hm.application_conduct_id = 2           --長期計画
        AND con.execution_division = 4          --保全項目一覧の追加
        AND NOT EXISTS ( 
            SELECT
                * 
            FROM
                hm_history_management temp_hm 
                LEFT JOIN hm_mc_management_standards_content temp_con 
                    ON temp_hm.history_management_id = temp_con.history_management_id 
            WHERE
                temp_hm.application_conduct_id = 2 --長期計画
                AND temp_con.execution_division = 5 --保全項目一覧の削除
                AND hm.key_id = temp_hm.key_id 
                AND con.management_standards_content_id = temp_con.management_standards_content_id 
                AND hm.history_management_id < temp_hm.history_management_id
        )
)
-- history_orderの前後で紐づけて、変更前後を横持ちにした帳票を出力
SELECT
    a.row_num                                   -- No
    , a.long_plan_id
    , a.history_management_id
    , CASE 
        WHEN lp.job_structure_id IS NOT NULL 
            THEN ( 
            SELECT
                st.translation_text 
            FROM
                #temp_structure_layer st 
            WHERE
                st.org_structure_id = lp.job_structure_id 
                AND st.structure_layer_no = 0
        ) 
        ELSE hd.job_name 
        END AS job_name                         --職種
    , COALESCE(lp.subject, hd.subject) AS subject -- 長計件名
    , COALESCE(mc.machine_no, hmc.machine_no) AS machine_no --機器番号
    , COALESCE(mc.machine_name, hmc.machine_name) AS machine_name --機器名称
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = COALESCE(mscom.inspection_site_structure_id, hmscom.inspection_site_structure_id) 
                    AND st_f.factory_id IN (0, COALESCE(lp.factory_id, hd.factory_id))
            ) 
            AND tra.structure_id = COALESCE(mscom.inspection_site_structure_id, hmscom.inspection_site_structure_id)
    ) AS inspection_site_name                   --保全部位(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = COALESCE(mscon.inspection_content_structure_id, hcon.inspection_content_structure_id) 
                    AND st_f.factory_id IN (0, COALESCE(lp.factory_id, hd.factory_id))
            ) 
            AND tra.structure_id = COALESCE(mscon.inspection_content_structure_id, hcon.inspection_content_structure_id)
    ) AS inspection_content_name                --保全項目(翻訳)
    , a.col_name                                --変更項目
    , CASE a.division_cd 
        WHEN '10' THEN '' 
        ELSE b.col_value 
        END AS col_value_before                 -- 変更前の値、新規の場合はブランク
    , CASE a.division_cd 
        WHEN '30' THEN '' 
        ELSE a.col_value 
        END AS col_value_after                  -- 変更後の値、削除の場合はブランク
    , a.application_reason
    , a.application_user_name
    , a.application_date
    , a.approval_user_name
    , a.approval_date
    , a.status
    , a.division 
FROM
    history_data_vertical AS a                  -- 変更後(aはAFTERのA)
    LEFT OUTER JOIN history_data_vertical AS b  -- 変更前(bはBEFOREのB) 、新規申請の場合は紐づかないので外部結合
        ON ( 
            a.long_plan_id = b.long_plan_id 
            AND a.history_order - 1 = b.history_order 
            AND a.col_no = b.col_no
        ) 
    LEFT JOIN t_long_plan lp 
        ON a.long_plan_id = lp.long_plan_id 
    LEFT JOIN mc_management_standards_content mscon 
        ON a.long_plan_id = mscon.long_plan_id 
    LEFT JOIN mc_management_standards_component mscom 
        ON mscon.management_standards_component_id = mscom.management_standards_component_id 
    LEFT JOIN mc_machine mc 
        ON mscom.machine_id = mc.machine_id 
    LEFT JOIN ( 
        SELECT
            max(hd.history_management_id) AS history_management_id
            , hd.long_plan_id 
        FROM
            #temp_history_data hd 
        GROUP BY
            hd.long_plan_id
    ) hmlp 
        ON a.long_plan_id = hmlp.long_plan_id 
    LEFT JOIN #temp_history_data hd 
        ON hd.history_management_id = hmlp.history_management_id 
    LEFT JOIN history_content hcon 
        ON hmlp.long_plan_id = hcon.long_plan_id 
    LEFT JOIN mc_management_standards_component hmscom 
        ON hcon.management_standards_component_id = hmscom.management_standards_component_id 
    LEFT JOIN mc_machine hmc 
        ON hmscom.machine_id = hmc.machine_id 
WHERE
    COALESCE(a.col_value, '') <> COALESCE(b.col_value, '') -- 変更前後で値が異なる
    OR a.division_cd IN ('10', '30')            -- または新規or削除申請のもの
ORDER BY
    a.row_num
    , a.col_no

