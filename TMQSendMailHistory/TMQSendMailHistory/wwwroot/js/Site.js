/*個別の計算処理、日付処理等を追加*/

// -------------------------------------------------------------------
// 日付チェック関数
// -------------------------------------------------------------------
function isDate(str) {
    //null or 8文字でない or 数値でない場合はfalse
    if (str == null || str.length != 8 || isNaN(str)) {
        return false;
    }

    //年,月,日を取得する
    var y = parseInt(str.substr(0, 4));
    var m = parseInt(str.substr(4, 2)) - 1;  //月は0～11で指定するため-1しています。
    var d = parseInt(str.substr(6, 2));
    var dt = new Date(y, m, d);

    //判定する
    return (y == dt.getFullYear() && m == dt.getMonth() && d == dt.getDate());
}
//function isDate(y, m, d) {
//    dt = new Date(y, m - 1, d);
//    return (dt.getFullYear() == y && dt.getMonth() == m - 1 && dt.getDate() == d);
//}

// -------------------------------------------------------------------
// サインフラグ設定関数
// -------------------------------------------------------------------
function setDateFlg(chkdate) {

    //var hiduke = new Date();
    //var y = hiduke.getFullYear();
    //var m = hiduke.getMonth() + 1;
    //var d = hiduke.getDate();

    var key = "-1";
    var sign = null;

    //現在日時取得
    var sysdate = new Date();

    //10日後の設定
    var date10 = new Date();
    date10.setDate(sysdate.getDate() + 10);

    //30日後の設定
    var date30 = new Date();
    date30.setDate(sysdate.getDate() + 30);

    //チェック対象日時
    var arg = chkdate.split('/');
    var dt = new Date(arg[0], arg[1] -1 , arg[2]);

    if (dt <= sysdate) {
        //現在日時より前の場合、期限切れ
        key = "3";
        sign = "×";

    } else if (dt <= date10) {
        //10日以内の場合
        key = "2";
        sign = "△";

    } else if (dt <= date30) {
        //30日以内の場合
        key = "1";
        sign = "◇";
    }
    
    return key + "," + sign;
}


// -------------------------------------------------------------------
// 日付変更時 サイン設定
/*
   sprName   : スプレッド名
   valNm     : 日付VAL位置
   signvalNm1: サインVAL位置
   signvalNm2: 日付変更していないサインVAL位置
   signvalNm3: 日付変更していないサインVAL位置
   kenvalNm  : 検切サインVAL位置
   mltrowcnt : 1件のデータの行数
   trsData   : データ
   no        : 行番号(添え字)
*/
// -------------------------------------------------------------------
function changeDate_Sign(sprName, valNm, signvalNm1, signvalNm2, signvalNm3, kenvalNm, mltrowcnt, trsData, no) {

    var signHishigata = "◇";
    var signSankaku = "△";
    var signBatsu = "×";

    var chkFlg = 0;

    //入力した値を取得
    var str = $("[name = " + sprName + valNm + "_" + no + "]").val();

    //サイン表示セル
    //var cell = $(trsData[mltrowcnt * no - 2]).find('td[data-name="' + signvalNm1 + '"]');
    var cell;
    for (var i = 0; i < mltrowcnt; i++) {
        if (cell == undefined || $(cell).length == 0) {
            cell = $(trsData[mltrowcnt * no - 2 + i]).find('td[data-name="' + signvalNm1 + '"]');
        }
        
    }

    //日付形式のチェック
    if (isDate(str.split('/').join('')) == true) {

        chkFlg = 1;

        //サインの取得
        var result = setDateFlg(str);
        var val = result.split(',');

        //文字色クラスの削除
        $(cell).removeClass("msgcolorBlue");
        $(cell).removeClass("msgcolorOrange");
        $(cell).removeClass("msgcolorRed");

        //仕立検切サインチェック 
        if (val[0] == "-1") {
            $(cell).text("");
        } else {
            if (val[1] == signHishigata) {
                //◇の場合、青
                $(cell).text(val[1]);

                //文字色クラスの追加
                $(cell).addClass("msgcolorBlue");

            } else if (val[1] == signSankaku) {
                //△の場合、オレンジ
                $(cell).text(val[1]);

                //文字色クラスの追加
                $(cell).addClass("msgcolorOrange");

            } else if (val[1] == signBatsu) {
                //×の場合、赤
                $(cell).text(val[1]);

                //文字色クラスの追加
                $(cell).addClass("msgcolorRed");
            }
        }
        //} else if (str == null || str == "") {
    } else  {
        $(cell).text("");
    }

    //検切サインの設定
    //警告の中でいちばん悪い状態の警告サインを表示

    //サイン表示セル
    var kencell = $(trsData[mltrowcnt * no - 2]).find('td[data-name="' + kenvalNm + '"]');

    //サイン表示対象セル以外の値を取得
    var signval1 = $(trsData[mltrowcnt * no - 2]).find('td[data-name="' + signvalNm2 + '"]').text();
    var signval2 = $(trsData[mltrowcnt * no - 2]).find('td[data-name="' + signvalNm3 + '"]').text();

    if (chkFlg == 1) {

        //文字色クラスの削除
        $(kencell).removeClass("msgcolorBlue");
        $(kencell).removeClass("msgcolorOrange");
        $(kencell).removeClass("msgcolorRed");

        if (val[1] == signBatsu || signval1 == signBatsu || signval2 == signBatsu) {
            $(kencell).text(signBatsu);
            $(kencell).addClass("msgcolorRed");

        } else if (val[1] == signSankaku || signval1 == signSankaku || signval2 == signSankaku) {
            $(kencell).text(signSankaku);
            $(kencell).addClass("msgcolorOrange");

        } else if (val[1] == signHishigata || signval1 == signHishigata || signval2 == signHishigata) {
            $(kencell).text(signHishigata);
            $(kencell).addClass("msgcolorBlue");

        } else {
            $(kencell).text("");
        }

    } else if (cell.text() == "" && signval1 == "" && signval2 == "") {
        //全部空欄の場合は検切サインもクリア
        $(kencell).text("");
    }

}