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
    machine_id bigint
); 
-- 画面で指定された条件で出力対象の機番IDを検索
INSERT INTO #temp_target_data
SELECT DISTINCT
    ma.machine_id 
FROM
    hm_mc_machine AS ma 
    INNER JOIN hm_mc_equipment eq 
        ON ma.history_management_id = eq.history_management_id 
    LEFT JOIN hm_mc_applicable_laws ap 
        ON ma.history_management_id = ap.history_management_id 
WHERE
    1 = 1 
    /*@LocationStructureIds
    AND EXISTS ( 
        SELECT
            * 
        FROM
            #temp_condition_location temp 
        WHERE
            ma.location_structure_id = temp.structure_id
    ) 
    @LocationStructureIds*/
    /*@JobStructureIds
    AND EXISTS ( 
        SELECT
            * 
        FROM
            #temp_condition_job temp 
        WHERE
            ma.job_structure_id = temp.structure_id
    ) 
    @JobStructureIds*/
    /*@MachineNo
    AND ma.machine_no LIKE @MachineNo
    @MachineNo*/
    /*@MachineName
    AND ma.machine_name LIKE @MachineName
    @MachineName*/
    /*@EquipmentLevelStructureIdList
    AND ma.equipment_level_structure_id IN @EquipmentLevelStructureIdList
    @EquipmentLevelStructureIdList*/
    /*@ImportanceStructureIdList
    AND ma.importance_structure_id IN @ImportanceStructureIdList
    @ImportanceStructureIdList*/
    /*@ConservationStructureIdList
    AND ma.conservation_structure_id IN @ConservationStructureIdList
    @ConservationStructureIdList*/
    /*@InstallationLocation
    AND ma.installation_location LIKE @InstallationLocation
    @InstallationLocation*/
    /*@NumberOfInstallationFrom
    AND ma.number_of_installation >= @NumberOfInstallationFrom
    @NumberOfInstallationFrom*/
    /*@NumberOfInstallationTo
    AND ma.number_of_installation <= @NumberOfInstallationTo
    @NumberOfInstallationTo*/
    /*@DateOfInstallationFrom
    AND ma.date_of_installation >= @DateOfInstallationFrom
    @DateOfInstallationFrom*/
    /*@DateOfInstallationTo
    AND ma.date_of_installation <= @DateOfInstallationTo
    @DateOfInstallationTo*/
    /*@ApplicableLawsStructureIdList
    AND ap.applicable_laws_structure_id IN @ApplicableLawsStructureIdList
    @ApplicableLawsStructureIdList*/
    /*@MachineNote
    AND ma.machine_note LIKE @MachineNote
    @MachineNote*/
    /*@ManufacturerStructureIdList
    AND eq.manufacturer_structure_id IN @ManufacturerStructureIdList
    @ManufacturerStructureIdList*/
    /*@ManufacturerType
    AND eq.manufacturer_type LIKE @ManufacturerType
    @ManufacturerType*/
    /*@ModelNo
    AND eq.model_no LIKE @ModelNo
    @ModelNo*/
    /*@SerialNo
    AND eq.serial_no LIKE @SerialNo
    @SerialNo*/
    /*@DateOfManufactureFrom
    AND eq.date_of_manufacture >= @DateOfManufactureFrom
    @DateOfManufactureFrom*/
    /*@DateOfManufactureTo
    AND eq.date_of_manufacture <= @DateOfManufactureTo
    @DateOfManufactureTo*/
    /*@DeliveryDateFrom
    AND eq.delivery_date >= @DeliveryDateFrom
    @DeliveryDateFrom*/
    /*@DeliveryDateTo
    AND eq.delivery_date <= @DeliveryDateTo
    @DeliveryDateTo*/
    /*@UseSegmentStructureIdList
    AND eq.use_segment_structure_id IN @UseSegmentStructureIdList
    @UseSegmentStructureIdList*/
    /*@UseSegmentIsNull
    AND eq.use_segment_structure_id IS NULL
    @UseSegmentIsNull*/
    /*@FixedAssetNo
    AND eq.fixed_asset_no LIKE @FixedAssetNo
    @FixedAssetNo*/
    /*@EquipmentNote
    AND eq.equipment_note LIKE @EquipmentNote
    @EquipmentNote*/
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
                hm_mc_machine AS hma 
            WHERE
                EXISTS ( 
                    SELECT
                        * 
                    FROM
                        #temp_target_data td 
                    WHERE
                        td.machine_id = hma.machine_id
                ) 
                AND ( 
                    st.structure_id = hma.location_structure_id 
                    OR st.structure_id = hma.job_structure_id
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
    machine_id bigint
    , history_management_id bigint
    , history_order bigint
    , division_cd nvarchar(400) COLLATE Japanese_CI_AS_KS
    , status nvarchar(400) COLLATE Japanese_CI_AS_KS
    , division nvarchar(400) COLLATE Japanese_CI_AS_KS
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
    , machine_no nvarchar(400) COLLATE Japanese_CI_AS_KS
    , machine_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , equipment_level_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , installation_location nvarchar(400) COLLATE Japanese_CI_AS_KS
    , number_of_installation nvarchar(400) COLLATE Japanese_CI_AS_KS
    , date_of_installation nvarchar(400) COLLATE Japanese_CI_AS_KS
    , importance_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , conservation_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , applicable_laws_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , machine_note nvarchar(400) COLLATE Japanese_CI_AS_KS
    , use_segment_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , circulation_target nvarchar(400) COLLATE Japanese_CI_AS_KS
    , fixed_asset_no nvarchar(400) COLLATE Japanese_CI_AS_KS
    , manufacturer_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , manufacturer_type nvarchar(400) COLLATE Japanese_CI_AS_KS
    , model_no nvarchar(400) COLLATE Japanese_CI_AS_KS
    , serial_no nvarchar(400) COLLATE Japanese_CI_AS_KS
    , date_of_manufacture nvarchar(400) COLLATE Japanese_CI_AS_KS
    , delivery_date nvarchar(400) COLLATE Japanese_CI_AS_KS
    , maintainance_kind_manage nvarchar(400) COLLATE Japanese_CI_AS_KS
    , equipment_note nvarchar(400) COLLATE Japanese_CI_AS_KS
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
        structure_group_id IN ( 
            1030
            , 1150
            , 1160
            , 1170
            , 1180
            , 1200
            , 1210
            , 1220
            , 1790
        ) 
        AND language_id = @LanguageId
) 
INSERT INTO #temp_history_data
-- 対象の機器全ての変更履歴を表示
SELECT
    hma.machine_id
    , hm.history_management_id
    , row_number() OVER ( 
        PARTITION BY
            hma.machine_id 
        ORDER BY
            hma.history_management_id
    ) AS history_order                      -- 機器ごとに何番目の変更管理かを採番、変更前後の比較に使用
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
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hma.location_structure_id 
            AND st.structure_layer_no = 0
    ) AS district_name                      --地区
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hma.location_structure_id 
            AND st.structure_layer_no = 1
    ) AS factory_name                       --工場
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hma.location_structure_id 
            AND st.structure_layer_no = 2
    ) AS plant_name                         --プラント
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hma.location_structure_id 
            AND st.structure_layer_no = 3
    ) AS series_name                        --系列
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hma.location_structure_id 
            AND st.structure_layer_no = 4
    ) AS stroke_name                        --工程
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hma.location_structure_id 
            AND st.structure_layer_no = 5
    ) AS facility_name                      --設備
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hma.job_structure_id 
            AND st.structure_layer_no = 0
    ) AS job_name                           --職種
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hma.job_structure_id 
            AND st.structure_layer_no = 1
    ) AS large_classfication_name           --機種大分類
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hma.job_structure_id 
            AND st.structure_layer_no = 2
    ) AS middle_classfication_name          --機種中分類
    , ( 
        SELECT
            st.translation_text 
        FROM
            #temp_structure_layer st 
        WHERE
            st.org_structure_id = hma.job_structure_id 
            AND st.structure_layer_no = 3
    ) AS small_classfication_name           --機種小分類
    , hma.machine_no                        --機器番号
    , hma.machine_name                      --機器名称
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
                    st_f.structure_id = hma.equipment_level_structure_id 
                    AND st_f.factory_id IN (0, hma.factory_id)
            ) 
            AND tra.structure_id = hma.equipment_level_structure_id
    ) AS equipment_level_name               --機器レベル(翻訳)
    , hma.installation_location             --設置場所
    , CONVERT(nvarchar, hma.number_of_installation) AS number_of_installation --設置台数
    , CASE 
        WHEN hma.date_of_installation IS NOT NULL 
            THEN FORMAT(hma.date_of_installation, 'yyyy/MM') 
        ELSE NULL                           --設置年月
        END AS date_of_installation
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
                    st_f.structure_id = hma.importance_structure_id 
                    AND st_f.factory_id IN (0, hma.factory_id)
            ) 
            AND tra.structure_id = hma.importance_structure_id
    ) AS importance_name                    --重要度(翻訳)
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
                    st_f.structure_id = hma.conservation_structure_id 
                    AND st_f.factory_id IN (0, hma.factory_id)
            ) 
            AND tra.structure_id = hma.conservation_structure_id
    ) AS conservation_name                  --保全方式(翻訳)
    , ( 
        SELECT
            trim( 
                ',' 
                FROM
                    ( 
                        SELECT
                            ( 
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
                                            st_f.structure_id = ap.applicable_laws_structure_id 
                                            AND st_f.factory_id IN (0, hma.factory_id)
                                    ) 
                                    AND tra.structure_id = ap.applicable_laws_structure_id
                            ) + ',' 
                        FROM
                            hm_mc_applicable_laws ap 
                        WHERE
                            ap.history_management_id = hma.history_management_id FOR XML PATH ('')
                    )
            )
    ) AS applicable_laws_name               --適用法規
    , hma.machine_note                      --機番メモ
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
                    st_f.structure_id = heq.use_segment_structure_id 
                    AND st_f.factory_id IN (0, hma.factory_id)
            ) 
            AND tra.structure_id = heq.use_segment_structure_id
    ) AS use_segment_name                   --使用区分(翻訳)
    , CASE 
        WHEN heq.circulation_target_flg = 1 
            THEN [dbo].[get_rep_translation_text] (hma.factory_id, 111160071, @LanguageId) --対象
        WHEN heq.circulation_target_flg = 0 
            THEN [dbo].[get_rep_translation_text] (hma.factory_id, 111270034, @LanguageId) --非対象
        ELSE '' 
        END AS circulation_target           --循環対象
    , heq.fixed_asset_no                    --固定資産番号
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
                    st_f.structure_id = heq.manufacturer_structure_id 
                    AND st_f.factory_id IN (0, hma.factory_id)
            ) 
            AND tra.structure_id = heq.manufacturer_structure_id
    ) AS manufacturer_name                  --メーカー(翻訳)
    , heq.manufacturer_type                 --メーカー型式
    , heq.model_no                          --型式コード
    , heq.serial_no                         --製造番号
    , CASE 
        WHEN heq.date_of_manufacture IS NOT NULL 
            THEN FORMAT(heq.date_of_manufacture, 'yyyy/MM') 
        ELSE '' 
        END AS date_of_manufacture          --製造年月
    , CONVERT(nvarchar, heq.delivery_date) as delivery_date --納期
    , CASE 
        WHEN heq.maintainance_kind_manage = 1 
            THEN [dbo].[get_rep_translation_text] (hma.factory_id, 111160071, @LanguageId) --対象
        WHEN heq.maintainance_kind_manage = 0 
            THEN [dbo].[get_rep_translation_text] (hma.factory_id, 111270034, @LanguageId) --非対象
        ELSE '' 
        END AS maintainance_kind_manage     --点検種別毎管理
    , heq.equipment_note                    --機器メモ
    , row_number() OVER ( 
        ORDER BY
            IIF(hm.application_date IS NOT NULL, 0, 1)
            , hm.application_date
            , hma.machine_id
            , hm.history_management_id
    ) AS row_num                            --変更管理ID単位で採番する番号
    , hm.application_reason
    , hm.application_user_name
    , hm.application_date
    , hm.approval_user_name
    , hm.approval_date
    , hma.factory_id 
FROM
    ( 
        SELECT
            hma.*
            , ( 
                SELECT
                    st.structure_id 
                FROM
                    #temp_structure_layer st 
                WHERE
                    st.org_structure_id = hma.location_structure_id 
                    AND st.structure_layer_no = 1
            ) AS factory_id                 --工場ID
        FROM
            hm_mc_machine hma
    ) AS hma 
    INNER JOIN hm_mc_equipment heq 
        ON hma.history_management_id = heq.history_management_id 
    INNER JOIN hm_history_management AS hm 
        ON hm.history_management_id = hma.history_management_id 
    INNER JOIN division_info AS div 
        ON (div.structure_id = hm.application_division_id) -- 出力対象の件名のみ
WHERE
    EXISTS ( 
        SELECT
            * 
        FROM
            #temp_target_data td 
        WHERE
            td.machine_id = hma.machine_id
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
                AND temp.ex_data = '1'
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
        structure_group_id IN ( 
            1030
            , 1150
            , 1160
            , 1170
            , 1180
            , 1200
            , 1210
            , 1220
            , 1790
        ) 
        AND language_id = @LanguageId
) 
, history_data_vertical AS (
-- 項目ごとの縦持ちに変更
    @HistoryVertical
)
, t_machine AS ( 
    -- 機器と工場IDを取得
    SELECT
        ma.*
        , ( 
            SELECT
                st.structure_id 
            FROM
                #temp_structure_layer st 
            WHERE
                st.org_structure_id = ma.location_structure_id 
                AND st.structure_layer_no = 1
        ) AS factory_id                         --工場ID
    FROM
        mc_machine ma
)
-- history_orderの前後で紐づけて、変更前後を横持ちにした帳票を出力
SELECT
    a.row_num                                   -- No
    , a.machine_id
    , a.history_management_id
    , CASE 
        WHEN ma.job_structure_id IS NOT NULL 
            THEN ( 
            SELECT
                st.translation_text 
            FROM
                #temp_structure_layer st 
            WHERE
                st.org_structure_id = ma.job_structure_id 
                AND st.structure_layer_no = 0
        ) 
        ELSE hd.job_name 
        END AS job_name                         --職種
    , COALESCE(ma.machine_no, hd.machine_no) AS machine_no --機器番号
    , COALESCE(ma.machine_name, hd.machine_name) AS machine_name --機器名称
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
                    st_f.structure_id = COALESCE( 
                        mscom.inspection_site_structure_id
                        , hmscom.inspection_site_structure_id
                    ) 
                    AND st_f.factory_id IN (0, COALESCE(ma.factory_id, hd.factory_id))
            ) 
            AND tra.structure_id = COALESCE( 
                mscom.inspection_site_structure_id
                , hmscom.inspection_site_structure_id
            )
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
                    st_f.structure_id = COALESCE( 
                        mscon.inspection_content_structure_id
                        , hmscon.inspection_content_structure_id
                    ) 
                    AND st_f.factory_id IN (0, COALESCE(ma.factory_id, hd.factory_id))
            ) 
            AND tra.structure_id = COALESCE( 
                mscon.inspection_content_structure_id
                , hmscon.inspection_content_structure_id
            )
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
            a.machine_id = b.machine_id 
            AND a.history_order - 1 = b.history_order 
            AND a.col_no = b.col_no
        ) 
    LEFT JOIN t_machine ma 
        ON a.machine_id = ma.machine_id 
    LEFT JOIN mc_management_standards_component mscom 
        ON ma.machine_id = mscom.machine_id 
    LEFT JOIN mc_management_standards_content mscon 
        ON mscom.management_standards_component_id = mscon.management_standards_component_id 
    LEFT JOIN ( 
        SELECT
            max(hd.history_management_id) AS history_management_id
            , hd.machine_id 
        FROM
            #temp_history_data hd 
        GROUP BY
            hd.machine_id
    ) hma 
        ON a.machine_id = hma.machine_id 
    LEFT JOIN #temp_history_data hd 
        ON hd.history_management_id = hma.history_management_id 
    LEFT JOIN hm_mc_management_standards_component hmscom 
        ON hma.history_management_id = hmscom.history_management_id 
    LEFT JOIN hm_mc_management_standards_content hmscon 
        ON hma.history_management_id = hmscon.history_management_id 
WHERE
    COALESCE(a.col_value, '') <> COALESCE(b.col_value, '') -- 変更前後で値が異なる
    OR a.division_cd IN ('10', '30')            -- または新規or削除申請のもの
ORDER BY
    a.row_num
    , a.col_no
