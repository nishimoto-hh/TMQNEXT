WITH history AS ( 
    --履歴情報
    SELECT
        su.construction_date                    --着工日
        , su.completion_date                    --完了日
        , hi.maintenance_season_structure_id    --保全時期
        , CASE 
            WHEN hi.call_count >= 1 
                THEN 1 
            ELSE 0 
            END AS call_count_val               --呼出
        , hi.construction_company               --施工会社
        , hi.construction_personnel_id          --施工担当者ID
        , coalesce(us.display_name, hi.construction_personnel_name) AS construction_personnel_name --施工担当者
        , hi.actual_result_structure_id         --実施結果
        , hi.loss_absence                       --休損量
        , hi.loss_absence_type_count            --休損型数
        , hi.maintenance_opinion                --保全見解
        , hi.working_time_self                  --自係(Hr)
        , hi.working_time_company               --施工会社(Hr)
        , hi.total_working_time                 --総計(Hr)
        , hi.cost_note                          --費用メモ
        , hi.expenditure                        --実績費用
        , hi.history_id                         --履歴ID
        , hi.update_serialid                    --更新シリアルID
        , hi.rank_structure_id                  --ランク20230426 AEC shiraishi
    FROM
        ma_summary su 
        INNER JOIN ma_history hi 
            ON su.summary_id = hi.summary_id 
        LEFT JOIN ms_user us 
            ON hi.construction_personnel_id = us.user_id 
    WHERE
        su.summary_id = @SummaryId
) 
, item_ex AS ( 
    --呼出のアイテムを取得
    SELECT
        ms.structure_id
        , ms.factory_id
        , mie.extension_data 
    FROM
        ms_structure ms 
        INNER JOIN ms_item_extension mie 
            ON ms.structure_item_id = mie.item_id 
            AND ms.structure_group_id = 1110 
            AND mie.sequence_no = 1 
    WHERE
        ms.factory_id IN (0, @FactoryId)
) 
SELECT
    history.*
    , item_ex.structure_id AS call_count 
FROM
    history 
    LEFT JOIN item_ex 
        ON CAST(history.call_count_val AS varchar) = item_ex.extension_data 
        AND ( 
            item_ex.factory_id = CASE 
                WHEN EXISTS ( 
                    SELECT
                        * 
                    FROM
                        item_ex 
                    WHERE
                        factory_id = @FactoryId
                ) 
                    THEN @FactoryId 
                ELSE 0 
                END                             --工場固有のアイテム定義があればそちらを優先して取得
        )
