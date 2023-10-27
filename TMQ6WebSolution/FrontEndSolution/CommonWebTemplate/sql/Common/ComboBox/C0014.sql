/*
 * マスタメンテナンス 拡張項目　コンボ用データリストSQL
 */

SELECT
    tbl.col AS 'values'
    , CONVERT(NVARCHAR, tbl.col) AS labels 
FROM
    (
     /*IF param1 != null*/
     SELECT
         /*param1*/1 AS col
    /*END*/
     /*IF param2 != null*/
     UNION ALL
     SELECT
         /*param2*/2 AS col
    /*END*/
     /*IF param3 != null*/
     UNION ALL
     SELECT
         /*param3*/3 AS col
    /*END*/
     /*IF param4 != null*/
     UNION ALL
     SELECT
         /*param4*/4 AS col
    /*END*/
     /*IF param5 != null*/
     UNION ALL
     SELECT
         /*param5*/5 AS col
    /*END*/
     /*IF param6 != null*/
     UNION ALL
     SELECT
         /*param6*/6 AS col
    /*END*/
     /*IF param7 != null*/
     UNION ALL
     SELECT
         /*param7*/7 AS col
    /*END*/
     /*IF param8 != null*/
     UNION ALL
     SELECT
         /*param8*/8 AS col
    /*END*/
     /*IF param9 != null*/
     UNION ALL
     SELECT
         /*param9*/9 AS col
    /*END*/
     /*IF param10 != null*/
     UNION ALL
     SELECT
         /*param10*/10 AS col
    /*END*/
     /*IF param11 != null*/
     UNION ALL
     SELECT
         /*param11*/11 AS col
    /*END*/
     /*IF param12 != null*/
     UNION ALL
     SELECT
         /*param12*/12 AS col
    /*END*/
    ) AS tbl
