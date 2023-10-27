/*
* 機番IDのリストより機器情報を検索し、点検種別毎管理の値の種類の件数を取得するSQL
* bit型の値が1種類(True or False)のみなら1、2種類なら2が返る
*/
SELECT
    COUNT(temp.maintainance_kind_manage)
FROM
    (
        SELECT
            maintainance_kind_manage
        FROM
            mc_equipment
        WHERE
            machine_id IN @KeyIdList
        GROUP BY
            maintainance_kind_manage
    ) AS temp
