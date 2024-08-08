-- 一時テーブルに格納する
-- 「機番ID|開始日」の組み合わせがカンマ区切りで選択されていたレコード分文字列で渡ってくるので分割して一時テーブルに格納する
INSERT 
INTO #temp_machine 
SELECT
    substring(tbl.VALUE, 1, charindex('|', tbl.VALUE) - 1)
    , REPLACE ( 
        tbl.VALUE
        , substring(tbl.VALUE, 1, charindex('|', tbl.VALUE))
        , ''
    ) 
FROM
    ( 
        SELECT
            VALUE 
        FROM
            string_split(@StrIdDate, ',')
    ) tbl
