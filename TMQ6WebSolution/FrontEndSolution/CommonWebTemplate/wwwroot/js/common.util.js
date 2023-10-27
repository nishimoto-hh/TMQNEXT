/* ========================================================================
 *  機能名　    ：   【共通・画面共通ﾕｰﾃｨﾘﾃｨー】
 * ======================================================================== */

/*(プラグイン)*/
$.fn.isVisible = function () {
    if (this[0] == undefined) return false;
    return $.expr.filters.visible(this[0]);
};

/**
 *  指定要素配下のコントロールを非活性とする。
 *  @elements  {要素} 　：要素
 *  @flg       {bool}   ：true(非活性)、false(活性)
 */
function setDisableElements(elements, flg) {
    $(elements).find(":text, textarea, select, :checkbox").prop("disabled", flg);
}

/**
 *  指定要素配下のコントロールを非活性とする。
 *  @elementId {string} ：要素名
 *  @flg       {bool}   ：true(非活性)、false(活性)
 */
function setDisableId(elementId, flg) {
    var element = $(P_Article).find("#" + elementId);
    setDisableElements(element, flg);
    element = null;
}

/**
 *  指定要素配下のコントロールを非活性とする。
 *  @elementId {string} ：要素名
 *  @flg       {bool}   ：true(非活性)、false(活性)
 */
function setDisableBtn(element, flg) {
    $(element).prop("disabled", flg);
}

/**
 *  指定要素配下のコントロールを非活性とする。
 *  @elementId {string} ：要素名
 *  @flg       {bool}   ：true(非活性)、false(活性)
 */
function setDisableBtnId(elementId, flg) {
    var element = $(P_Article).find("#" + elementId);
    setDisableBtn(element);
    element = null;
}

/**
 *  指定要素を初期表示非表示とする。
 *  @element   {string} ：<div>要素指定文字列
 *  @flg       {bool}   ：true(非表示)、false(表示)
 */
function setInitHide(element, flg) {

    //true(非表示)、false(表示)
    if (flg) {
        $(element).addClass('init_hide');
    }
    else {
        $(element).removeClass('init_hide');

        //Tabulatorバージョンアップに伴い下記エラーとなるため削除。問題があれば対応する
        ////Tabulator一覧の再描画
        //var tbls = $(element).find('.tabulator');
        //$.each(tbls, function (idx, tbl) {
        //    var tblId = $(tbl).attr('id');
        //    P_listData['#' + tblId].redraw(true);
        //});
    }
}

/**
 *  指定Classをもつ要素を初期表示非表示とする。
 *  @className {string} ：<div>クラス名
 *  @flg       {bool}   ：true(非表示)、false(表示)
 */
function setInitHideClass(className, flg) {
    //true(非表示)、false(表示)
    var element = $(P_Article).find("." + className);
    setInitHide(element, flg);
    element = null;
}

/**
 *  指定要素を非表示とする。
 *  @element   {string} ：<div>要素指定文字列
 *  @flg       {bool}   ：true(非表示)、false(表示)
 */
function setHide(element, flg) {

    //true(非表示)、false(表示)
    if (flg) {
        $(element).addClass('hide');
    }
    else {
        $(element).removeClass('hide');
    }
}

/**
 *  指定idの要素を非表示とする。
 *  @elementId {string} ：<div>要素ID名
 *  @flg       {bool}   ：true(非表示)、false(表示)
 */
function setHideId(elementId, flg) {
    //true(非表示)、false(表示)
    var element = $(P_Article).find("#" + elementId);
    setHide(element, flg);
    element = null;
}

/**
 *  指定Classをもつ要素を非表示とする。
 *  @className {string} ：<div>クラス名
 *  @flg       {bool}   ：true(非表示)、false(表示)
 */
function setHideClass(className, flg) {
    //true(非表示)、false(表示)
    var element = $(P_Article).find("." + className);
    setHide(element, flg);
    element = null;
}

/**
 *  指定idの要素の表示/非表示を切り替える
 *  @elementId {string} ：<div>要素ID名
 */
function switchHideId(elementId) {
    var element = $(P_Article).find("#" + elementId);
    setHide(element, !isHide(element));
    element = null;
}

/**
 *  指定Classをもつ要素の表示/非表示を切り替える
 *  @className {string} ：<div>クラス名
 */
function switchHideClass(className) {
    var element = $(P_Article).find("." + className);
    setHide(element, !isHide(element));
    element = null;
}

/**
 *  指定要素が非表示か判定する。
 *  @element   {string} ：<div>要素指定文字列
 */
function isHide(element) {

    if ($(element).hasClass('hide')) {
        // 非表示の場合
        return true;
    } else {
        // 表示されている場合
        return false;
    }

}

/**
 *  指定idの要素が非表示か判定する。
 *  @elementId {string} ：<div>要素ID名
 */
function isHideId(elementId) {

    //true(非表示)、false(表示)
    return isHide($(P_Article).find("#" + elementId));
}

/**
 *  指定Classをもつ要素が非表示か判定する。
 *  @className {string} ：<div>クラス名
 */
function isHideClass(className) {

    //true(非表示)、false(表示)
    return isHide($(P_Article).find("." + className));
}

/**
 *  文字列のﾊﾞｲﾄ数を取得する。
 *  @val {string} ：文字列
 *  
 *  @return ：ﾊﾞｲﾄ数
 */
function getByteLength(str) {

    var byteCount = 0;
    for (var idx = 0; idx < str.length; idx++) {
        var char = str.charCodeAt(idx);
        // Shift_JIS: 0x0 〜 0x80, 0xa0  , 0xa1   〜 0xdf  , 0xfd   〜 0xff
        // Unicode  : 0x0 〜 0x80, 0xf8f0, 0xff61 〜 0xff9f, 0xf8f1 〜 0xf8f3
        if ((char >= 0x0 && char < 0x81) || (char == 0xf8f0) || (char >= 0xff61 && char < 0xffa0) || (char >= 0xf8f1 && char < 0xf8f4)) {
            byteCount += 1;
        } else {
            byteCount += 2;
        }
    }
    return byteCount;
}

/**
 *  現在日付のフォーマット文字列（YYYY/MM/DD）を生成する。
 *  
 *  @return ：現在日付文字列
 */
function getNowYMDString() {
    var now = new Date();

    var y = now.getFullYear();
    var m = ("00" + (now.getMonth() + 1)).slice(-2);
    var d = ("00" + now.getDate()).slice(-2);
    var result = y + "/" + m + "/" + d;

    return result;
}

/**
 *  指定日付の前日日付を取得する。
 *  ※フォーマット文字列（YYYY/MM/DD）
 *  
 *  @dateObj {date} ：指定日付
 *  
 *  @return ：前日日付文字列
 */
function getBeforeYMDString(dateObj) {

    //前日
    var bfDate = new Date(dateObj.getFullYear(), dateObj.getMonth(), dateObj.getDate() - 1);   //※前日なので-1

    var bfY = bfDate.getFullYear();
    var bfM = ("00" + (bfDate.getMonth() + 1)).slice(-2);   //※月は0～11で表記するので+1
    var bfD = ("00" + bfDate.getDate()).slice(-2);
    var result = bfY + "/" + bfM + "/" + bfD;

    return result;
}

/**
 * 【共通function】ネイティブjavascriptメソッドによる属性値セット（setAttribute）
 *  @elements   {string} : html要素指定文字列(jquery)
 *  @attribute  {string} : 対象のhtml属性名
 *  @value      {string} : 設定する値
 */
function setAttrByNativeJs(elements, attribute, value) {

    if (attribute.indexOf("data-") === 0) {
        var attrW = attribute.slice(5);
        $(elements).data(attrW, value);
    }
    for (var i = 0; i < $(elements).length; i++) {
        $(elements)[i].setAttribute(attribute, value);
    }
}

/**
 *【共通function】ajaxの戻り値を結果データと個別実装用データに分離
 * @param {Dictionary<string, object>}  dicReturn   : ajax戻り値
 * @param {string}                      conductId   : 機能ID
 */
function separateDicReturn(dicReturn, conductId) {
    // 結果データ
    var resultData = null;
    if (dicReturn != null) {
        if ("Result" in dicReturn) {
            resultData = dicReturn["Result"];   // List<Dictionary<string, object>>
        }
        // 個別実装用データ
        if ("Individual" in dicReturn) {
            P_dicIndividual = dicReturn["Individual"];  // Dictionary<string, object>
        }
        /* ボタン権限制御 切替 start ================================================ */
        // ﾎﾞﾀﾝ権限
        if ("ButtonStatus" in dicReturn && dicReturn["ButtonStatus"].length > 0 && conductId != null && conductId.length > 0) {
            P_buttonDefine[conductId] = dicReturn["ButtonStatus"];  // List<Dictionary<string, object>>
        }
        /* ボタン権限制御 切替 end ================================================== */
    }
    return resultData;

}

/**
 * 【共通function】ユニークIDの取得
 *  @myDigits   {int} : 乱数の桁数(デフォルト:4桁⇒乱数の範囲:0001～9999)
 *  @return     {string} : ユニークID
 */
function getUniqueId(myDigits) {
    var digits = 4;
    if (myDigits) digits = myDigits;
    var strong = Math.pow(10, digits);
    return new Date().getTime().toString().padStart(16, '0') + Math.floor((strong - 1) * Math.random() + 1).toString().padStart(4, '0')
}

/**
 * 【共通function】sessionStorageからのブラウザタブ識別番号の取得
 *  @return {string} : ブラウザタブ識別番号
 */
function getBrowserTabNo() {
    // タブ番号
    var tabNo = sessionStorage.getItem("BrowserTabNo");
    // ページ遷移フラグ
    var pageTransitioned = isPageTransitioned();

    if (tabNo === null || tabNo === undefined || !pageTransitioned) {
        // 存在しない場合(=初期起動時)、ページ遷移フラグがOFFの場合(=タブ複製時)はタブ番号を生成する
        tabNo = getUniqueId();
        sessionStorage.setItem("BrowserTabNo", tabNo);
    }
    // ページ遷移フラグをOFF
    setPageTransitionFlg(false);

    return tabNo;
}

/**
 * 【共通function】ページ遷移フラグの設定
 *  @flg    {boolean} : フラグ値(true/false)
 */
function setPageTransitionFlg(flg) {
    var val = flg ? "true" : "false";
    sessionStorage.setItem("PageTransitioned", val);
}

/**
 * 【共通function】ページ遷移フラグの設定
 *  @return     {boolean} : フラグ値(true/false)
 */
function isPageTransitioned() {
    var val = sessionStorage.getItem("PageTransitioned");
    var flg = val === "true" ? true : false;
    return flg;
}

/**
 * 【共通function】sessionStorageからの選択中の画面タブ番号の取得
 *  @key    {string} : sessionStorageキー
 *  @return {string} : 画面タブ番号
 */
function getSelectedFormTabNo(key) {
    var tabNo = sessionStorage.getItem(key);
    return (tabNo === null || tabNo === undefined) ? 0 : tabNo;
}

/**
 * 【共通function】sessionStorageからの選択中の画面タブ番号の取得
 *  @key    {string} : sessionStorageキー
 *  @tabNo  {string} : 画面タブ番号
 */
function setSelectedFormTabNo(key, tabNo) {
    sessionStorage.setItem(key, tabNo);
}

/**
 * 【共通function】sessionStorageの選択中の画面タブ番号のクリア
 *  @key    {string} : sessionStorageキー
 */
function clearSelectedFormTabNo(key) {
    sessionStorage.removeItem(key);
}

/**
 * 【共通function】sessionStorageの選択中の画面タブ番号格納キーの取得
 *  @btn    {element} : タブボタン要素
 *  @return {string} : sessionStorageキー
 */
function getSelectedFormTabNoKey(btn) {
    // タブボタンの親divのidを使用する
    var parentDiv = $(btn).closest('div');
    var id = $(parentDiv).attr('id');
    parentDiv = null;
    return "Tab_" + id;
}

/**
 * 【共通function】datepickerの全種類を削除
 * @ctrls  {html要素} : 対象要素
 */
function destroyDatepickerCtrls(ctrls) {
    destroyDatepicker(ctrls); // 日付
    destroyTimepicker(ctrls); // 時刻
    destroyDatetimepicker(ctrls); // 日付時刻
    destroyYearMonthpicker(ctrls); // 年月
}

/**
 * 【共通function】datepicker削除
 *  @ctrls  {html要素} : 対象要素
 */
function destroyDatepicker(ctrls) {
    var datepickers = $(ctrls).find(":text[data-type='date']");
    destroyDatepickerCommon(datepickers);
    datepickers = null;
}

/**
 * 【共通function】timepicker削除
 *  @ctrls  {html要素} : 対象要素
 */
function destroyTimepicker(ctrls) {
    var timepickers = $(ctrls).find(":text[data-type='time']");
    destroyDatepickerCommon(timepickers);
    timepickers = null;
}

/**
 * 【共通function】datetimepicker削除
 *  @ctrls  {html要素} : 対象要素
 */
function destroyDatetimepicker(ctrls) {
    var datetimepickers = $(ctrls).find(":text[data-type='datetime']");
    destroyDatepickerCommon(datetimepickers);
    datetimepickers = null;
}

/**
 * 【共通function】yearmonthpicker削除
 *  @ctrls  {html要素} : 対象要素
 */
function destroyYearMonthpicker(ctrls) {
    var yearmonthpickers = $(ctrls).find(":text[data-type='yearmonth']");
    $.each(yearmonthpickers, function (index, element) {
        if (element.datepicker != '') {
            // 年月はympickerなので共通処理を呼ばない
            $(element).ympicker("destroy");
        }
    });
    yearmonthpickers = null;
}

/**
 * 【共通function】datepicker系共通削除処理
 *  @elements  {html要素} : 対象datepicker要素群
 */
function destroyDatepickerCommon(elements) {
    $.each(elements, function (index, element) {
        if (element.datepicker != '') {
            $(element).datepicker("destroy");
        }
    });
}

/**
 * 【共通function】ｵｰﾄｺﾝﾌﾟﾘｰﾄ削除処理
 *  @elements  {html要素} : 対象ｵｰﾄｺﾝﾌﾟﾘｰﾄ要素群
 */
function destroyAutocomplete(elements) {
    $.each(elements, function (index, element) {
        if (element.autocomplete != '') {
            $(element).autocomplete("destroy");
        }
    });
}

/**
 *  検索結果のクリア処理
 *  @param {string} ：対象テーブルのコントロールID
*/
function clearSearchResult(targetTblId, formNo) {

    var detailDivId = $(P_Article).find(".detail_div").attr("id");

    //※引数が未指定の場合は明細ｴﾘｱすべての一覧が対象
    var targetId = (targetTblId == null || targetTblId.length == 0) ? detailDivId + " table" : targetTblId +
        ((formNo == null || targetTblId.length == 0) ? "" : "_" + formNo);
    var detailTbls = $(P_Article).find("#" + targetId);

    // 縦方向一覧
    var vertical = $(detailTbls).filter(".vertical_tbl");
    if ($(vertical).length) {
        $.each($(vertical), function (i, tbl) {
            if ($(tbl).closest('div.Filter').length > 0) {
                // フィルター/読込件数エリアの場合はスキップ
                return true;
            }
            // 設定されたﾃﾞｰﾀをｸﾘｱ
            var tds = $(tbl).find("tbody td");
            if ($(tds).length) {
                $.each($(tds), function (idx, td) {
                    clearCellVal(td);
                });
            }
            tds = null;
            // 既存変更不可項目制御を初期化
            resetUnchangeableTds(tbl);
        });
    }
    vertical = null;

    // 横方向一覧
    var horizontal = $(detailTbls).filter(".ctrlId:not(.vertical_tbl)");
    if ($(horizontal).length) {
        $.each($(horizontal), function (i, tbl) {
            //データ行を削除
            getTrsData(tbl).remove();
        });
    }
    horizontal = null;

    //103一覧（Tabulator一覧）
    var targetTabulatorId = (targetTblId == null || targetTblId.length == 0) ? detailDivId + " .ctrlId.tabulator" : targetTblId +
        ((formNo == null || targetTblId.length == 0) ? "" : "_" + formNo);
    var tabulatorTbls = $(P_Article).find("#" + targetTabulatorId);
    if ($(tabulatorTbls).length) {
        $.each($(tabulatorTbls), function (i, tbl) {
            //ID取得
            var id = $(tbl).attr("id");
            var table = P_listData["#" + id];
            if (table) {
                //データをクリア
                table.clearData();
                table = null;
            }
        });
    }
    tabulatorTbls = null;
    detailTbls = null;
}

/**
 * 画面データ一括クリア
 * @param {boolean} ctrlGrClearFlg : ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ内ｺﾝﾄﾛｰﾙ削除ﾌﾗｸﾞ（true：削除する、false：削除しない）
 */
function clearArticle(ctrlGrClearFlg) {
    var areas = [];
    var articleName = $(P_Article).attr("name");
    if (articleName == "edit_area") {
        //単票表示の場合
        areas = ["formEdit"];
    }
    else {
        areas = ["formTop", "formSearch", "formDetail", "formBottom"];
    }
    $.each(areas, function (idx, area) {
        clearAreaLists(area);
    });
}

/**
 * ｴﾘｱ内の一覧をｸﾘｱ
 * @param {string} area : ｴﾘｱ指定文字列（formﾀｸﾞのid前方一致）
 */
function clearAreaLists(area) {
    var lists = $(P_Article).find("form[id^='" + area + "']").find(".ctrlid");  //ｴﾘｱ内の一覧
    $.each($(lists), function (idx, list) {
        clearList(list);
    });
    lists = null;
}

/**
 * 一覧ｸﾘｱ
 * @param {html} list : 一覧html要素（.ctrlidｸﾗｽ単位）
 */
function clearList(list) {

    if ($(list).hasClass(".vertical_tbl")) {
        //※縦方向一覧の場合
        // 設定されたﾃﾞｰﾀをｸﾘｱ
        var tds = $(list).find("tbody td");
        if ($(tds).length) {
            $.each($(tds), function (idx, td) {
                clearCellVal(td);
            });
        }
        tds = null;
        // 既存変更不可項目制御を初期化
        resetUnchangeableTds(list);
    }
    else {
        //※横方向一覧の場合
        getTrsData(tbl).remove();   //データ行を削除
        //単票表示する一覧の場合
    }
}

/**
 * 既存変更不可項目のﾓｰﾄﾞをﾘｾｯﾄ※縦方向一覧
 * @param {html} list : 一覧単位の要素
 */
function resetUnchangeableTds(list) {

    var unChangeableTds = $(list).find("tbody tr:not([class^='base_tr']) td[data-unchangeablekbn='" + unChangeableKbnDef.Unchangeable + "']");   //既存変更不可項目
    if ($(unChangeableTds).length) {
        $(unChangeableTds).removeClass("unchange_exist");
    }
    unChangeableTds = null;
}

/**
 * 入力ｺﾝﾄﾛｰﾙのﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
 * @param {td}      td  : td要素
 * @param {string}  txt : 表示文字列
 */
function setLabelingSpan(td, txt) {
    $(td).find("span.labeling").text(txt);

    //テキストエリアのラベルは改行して表示
    var textarea = $(td).find("textarea");
    if (textarea && textarea.length > 0) {
        $(td).find("span.labeling").addClass("word_break");
    }
    textarea = null;
}

/**
 * 値ｾｯﾄ（複数選択ﾘｽﾄ）
 * @param {td}      td      : td要素
 * @param {ul}      msuls   : 複数選択ﾘｽﾄｺﾝﾄﾛｰﾙ要素
 * @param {string}  value   : 設定値
 */
function setDataForMultiSelect(td, msuls, value) {

    //設定値
    var vals = value + '';   //縦棒区切り
    var aryVal = vals.split('|');

    // 複数選択ﾘｽﾄの選択状態をｾｯﾄ
    var checks = $(msuls).find('> li:not(.hide) :checkbox');
    $.each(checks, function () {
        if (aryVal.indexOf($(this).val()) >= 0) {
            //設定値の場合、選択状態にセット
            $(this).prop('checked', true);
        }
    });
    checks = null;

    if (aryVal.length > 0) {
        //全ての項目がﾁｪｯｸon(＝ﾁｪｯｸoffの項目がない)場合、「すべて」にチェック(削除アイテムは除外する)
        var checkes = $(msuls).find("li>ul>li:not(.hide):not(.deleteItem) :checkbox:not(.ctrloption):unchecked");
        if (checkes == null || checkes.length == 0) {
            //ｵﾌﾟｼｮﾝ項目ﾁｪｯｸﾎﾞｯｸｽをﾁｪｯｸ：on
            var allchk = $(msuls).find("li:not(.hide) :checkbox.ctrloption");
            if (allchk && allchk.length > 0) {
                $(allchk).prop('checked', true);
            }
        }
    }
    setAttrByNativeJs(msuls, "data-value", value);     //※再生成用に退避

    //ﾁｪｯｸ:onの表示名をｾｯﾄ
    var txt = setMutiSelectCheckOnText(td);

    //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
    setLabelingSpan(td, txt);
}

/**
 * 値ｾｯﾄ（ｾﾚｸﾄﾎﾞｯｸｽ）
 * @param {td}      td      : td要素
 * @param {select}  select  : ｾﾚｸﾄﾎﾞｯｸｽｺﾝﾄﾛｰﾙ要素
 * @param {string}  value   : 設定値
 */
function setDataForSelectBox(td, select, value) {
    // 表示文字を取得
    var txt = "";
    if ($(select).attr('multiple')) {
        //複数選択の場合
        //設定値
        var vals = value + '';   //縦棒区切り
        var aryVal = vals.split('|');
        $(select).val(aryVal);

        var texts = [];
        $.each($(select).find("option:selected"), function () {
            texts.push($(this).text());
        });
        txt = texts.join(',');
    } else {
        $(select).val(value);
        //txt = $(select).find("option:selected").text();
        var selected = $(select).find("option:selected");
        if ((selected == null || selected.length == 0) && value != null) {
            // 論理削除データの場合、"option:selected"では取得できない
            selected = $(select).find('option[value="' + value + '"]');
        }
        txt = $(selected).text();
    }
    setAttrByNativeJs(select, "data-value", value);     //※ｺﾝﾎﾞ再生成用に退避

    //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
    setLabelingSpan(td, txt);
}

/**
 * 値ｾｯﾄ（ラジオボタン）
 * @param {td}      td      : td要素
 * @param {radio}  radio  : ﾗｼﾞｵﾎﾞﾀﾝｺﾝﾄﾛｰﾙ要素
 * @param {string}  value   : 設定値
 */
function setDataForRadioButton(td, radio, value) {
    // 表示文字を取得
    var txt = "";

    $.each(radio, function () {
        var val = $(this).val();
        if (val == value) {
            //選択値の場合、選択状態にセット
            $(this).prop('checked', true);
            //ラベルタグの値を取得
            txt = $(this).parent().text();
            return false;
        }
    });
    //コード値を保持
    setAttrByNativeJs(radio, "data-value", value);

    //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
    setLabelingSpan(td, txt);
}

/**
 * 値ｾｯﾄ（ﾁｪｯｸﾎﾞｯｸｽ）
 * @param {td}          td          : td要素
 * @param {checkbox}    checkbox    : ﾁｪｯｸﾎﾞｯｸｽｺﾝﾄﾛｰﾙ要素
 * @param {string}      value       : 設定値
 */
function setDataForCheckBox(td, checkbox, value) {
    var labeling = $(td).find("span.labeling.checkbox");
    if (value == "1") {
        $(checkbox).prop("checked", true);
        $(labeling).addClass("checked");
    }
    else {
        $(checkbox).prop("checked", false);
        $(labeling).removeClass("checked");
    }
    labeling = null;

    //tdタグにコード値を保持
    setAttrByNativeJs(td, "data-value", value);
}

/**
 * 値ｾｯﾄ（数値）
 * @param {td}      td      : td要素
 * @param {input}   input   : 数値ｺﾝﾄﾛｰﾙ要素
 * @param {string}  value   : 設定値
 */
function setDataForTextNum(td, input, value) {

    var valueunit = (value + '').split("@"); //単位分割
    var unit = "";
    if (valueunit.length > 1) {
        unit = valueunit[1];
    }
    var txt = "";

    // チェンジイベント発火により、画面変更フラグが必ず立ってしまう
    // イベント発火後に、発火前の値を再設定する
    var tempFlg = dataEditedFlg;
    if ($(input).hasClass('fromto') && input.length > 1) {
        var values = valueunit[0].split('|'); //fromto分割
        // From
        $(input[0]).val(values[0]);
        setCellNumUnit(input[0], unit); //単位
        // To
        if (values.length > 1) {
            $(input[1]).val(values[1]);
        }
        setCellNumUnit(input[1], unit); //単位
        $(input).trigger("change");
        txt = $(input[0]).val() + "～" + $(input[1]).val();
    } else {
        $(input).val(valueunit[0]);
        setCellNumUnit(input, unit); //単位
        $(input).trigger("change");
        txt = $(input).val();
    }
    // 再設定
    dataEditedFlg = tempFlg;

    // 単位が設定されている場合、単位を設定
    if (unit != null && unit != "") {
        txt = txt + " " + unit;
    }

    //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
    setLabelingSpan(td, txt);
}

/**
 * 値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準日付）
 * @param {td}      td      : td要素
 * @param {input}   input   : input[type="date"]ｺﾝﾄﾛｰﾙ要素
 * @param {string}  value   : 設定値
 */
function setDataForTypeDate(td, input, value) {

    var values = value.split('|'); //fromto分割
    var num = Date.parse(values[0]);
    var date = new Date(num);
    var y = date.getFullYear();
    var m = ("0" + (date.getMonth() + 1)).slice(-2);
    var d = ("0" + date.getDate()).slice(-2);
    $(input[0]).val(y + "-" + m + "-" + d);
    //var txt = $(input[0]).val();    //ﾗﾍﾞﾙ表示用文字列
    var txt = values[0];    //ﾗﾍﾞﾙ表示用文字列
    if (input.length > 1 && values.length > 1) {
        num = Date.parse(values[1]);
        date = new Date(num);
        y = date.getFullYear();
        m = ("0" + (date.getMonth() + 1)).slice(-2);
        d = ("0" + date.getDate()).slice(-2);
        $(input[1]).val(y + "-" + m + "-" + d);
        //txt = txt + "～" + $(input[1]).val();
        txt = txt + "～" + values[1];
    }
    //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
    setLabelingSpan(td, txt);
}

/**
 * 値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準時刻）
 * @param {td}      td      : td要素
 * @param {input}   input   : input[type="time"]ｺﾝﾄﾛｰﾙ要素
 * @param {string}  value   : 設定値
 */
function setDataForTypeTime(td, input, value) {

    var values = value.split('|'); //fromto分割
    $(input[0]).val(values[0]);
    var txt = $(input[0]).val();
    if (input.length > 1 && values.length > 1) {
        $(input[1]).val(values[1]);
        txt = txt + "～" + $(input[1]).val();
    }
    //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
    setLabelingSpan(td, txt);
}

/**
 * 値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準日時）
 * @param {td}      td      : td要素
 * @param {input}   input   : input[type="time"]ｺﾝﾄﾛｰﾙ要素
 * @param {string}  value   : 設定値
 */
function setDataForTypeDatetimelocal(td, input, value) {

    var fmt = $(input).find("format");
    var values = value.split('|'); //fromto分割
    var num = Date.parse(values[0]);
    var date = new Date(num);
    var y = date.getFullYear();
    var m = ("0" + (date.getMonth() + 1)).slice(-2);
    var d = ("0" + date.getDate()).slice(-2);
    var h = ("0" + date.getHours()).slice(-2);
    var mi = ("0" + date.getMinutes()).slice(-2);
    var s = ("0" + date.getSeconds()).slice(-2);
    var valDate = y + "-" + m + "-" + d;
    var valTime = (fmt != null && fmt.length > 0 && fmt.indexOf("s")) ? h + ":" + mi + ":" + s : h + ":" + mi;
    $(input[0]).val(valDate + "T" + valTime);
    //var txt = $(input[0]).val();
    var txt = values[0];
    if (input.length > 1 && values.length > 1) {
        num = Date.parse(values[1]);
        date = new Date(num);
        m = ("0" + (date.getMonth() + 1)).slice(-2);
        d = ("0" + date.getDate()).slice(-2);
        h = ("0" + date.getHours()).slice(-2);
        mi = ("0" + date.getMinutes()).slice(-2);
        s = ("0" + date.getSeconds()).slice(-2);
        valDate = y + "-" + m + "-" + d;
        valTime = (fmt != null && fmt.length > 0 && fmt.indexOf("s")) ? h + ":" + mi + ":" + s : h + ":" + mi;
        $(input[1]).val(valDate + "T" + valTime);
        //txt = txt + "～" + $(input[1]).val();
        txt = txt + P_ComMsgTranslated[911060006] + values[1];
    }
    //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
    setLabelingSpan(td, txt);
}

/**
 * 値ｾｯﾄ（ダウンロードリンク、ファイル参照リンク）
 * @param {td}      td      : td要素
 * @param {a}       a       : ダウンロードリンク、ファイル参照リンクコントロール要素
 * @param {string}  value   : 設定値
 */
function setDataForFileDownloadOpenLink(td, a, value) {
    //value : [表示文字列1] | [ﾌｧｲﾙ相対ﾊﾟｽ1] || [表示文字列2] | [ﾌｧｲﾙ相対ﾊﾟｽ2] || ...

    //パイプ2個区切りで各ファイル毎の値を取得
    var values = value.split("||");
    $.each(values, function (idx, file) {
        //パイプ区切りで表示値と相対パスを取得
        var fileInfo = file.split("|");
        if (a.length > 0 && fileInfo.length > 1) {
            if (idx == 0) {
                $(a[0]).text(fileInfo[0]);            //[0]：表示文字列
                setAttrByNativeJs(a[0], "href", fileInfo[1]);    //[1]：ﾌｧｲﾙ相対ﾊﾟｽ
            } else {
                var link = $(a[0]).clone(true);
                $(link).text(fileInfo[0]);            //[0]：表示文字列
                setAttrByNativeJs(link, "href", fileInfo[1]);    //[1]：ﾌｧｲﾙ相対ﾊﾟｽ
                $(td).append(link);
            }
        }
        else {
            //ﾗﾍﾞﾙ表示
            $(td).append(fileInfo);
            //ラベルの場合改行して表示
            if (idx < values.length - 1) {
                $(td).append('<br>');
            }
        }
    });

}

/**
 * 値ｾｯﾄ（画像）
 * @param {img}     img     : 画像ｺﾝﾄﾛｰﾙ要素
 * @param {string}  value   : 設定値
 */
function setDataForImg(img, value) {
    let src = $(img).attr("src");
    src += value;
    setAttrByNativeJs(img, 'src', src);
}

/**
 * ITEMNOを持つtdに値をｾｯﾄ
 * @param {td}      td          : td要素
 * @param {string}  value       : 設定値
 * @param {boolean} isVertical  : 縦方向一覧ﾌﾗｸﾞ（true：縦、false：横）
 */
function setTdData(td, value, isVertical) {

}

/**
 * ﾎﾞﾀﾝが単票配置か判定
 * @param {html} btn : ﾎﾞﾀﾝ要素
 */
function judgeBtnIsEditPosition(btn) {

    //単票ｴﾘｱのﾎﾞﾀﾝか判定
    var isEdit = false;
    var areaKbn = $(btn).closest(".ctrlId[id$='_edit']");
    if ($(areaKbn).length) {
        isEdit = true;
    }
    areaKbn = null;
    return isEdit;
}

/**
 * ﾎﾞﾀﾝのﾃﾞｰﾀ収集範囲判定
 * @param {html} btn : ﾎﾞﾀﾝ要素
 */
function judgeGetRangeKbn(btn) {

    var getRangeKbn = getRangeKbnDef.Area;   // ｴﾘｱ

    var actionlist = $(btn).closest("table.actionlist");
    var relationCtrlId = ($(actionlist).data("relationctrlid") == null) ? "-" : $(actionlist).data("relationctrlid");
    var position = $(actionlist).data("positionkbn");
    var areaKbn = $(actionlist).data("areakbn");
    actionlist = null;

    //一覧に紐付くﾎﾞﾀﾝか判定
    if (relationCtrlId != "-") {
        getRangeKbn = getRangeKbnDef.List;  // 一覧
    }
    else {
        //ﾀﾌﾞ内ﾎﾞﾀﾝか判定
        var tab = $(btn).closest(".tab_contents");
        if ($(tab).length) {
            getRangeKbn = getRangeKbnDef.Tab;   // ﾀﾌﾞ
        }
        else {
            if (position == positionKbnDef.None && (areaKbn == areaKbnDef.Top || areaKbn == areaKbnDef.Bottom)) {  //上下指定なし、且つ、トップかボトム
                getRangeKbn = getRangeKbnDef.AllArea;  // 画面全体
            }
        }
        tab = null;
    }
    return getRangeKbn;
}

/**
 * ﾃﾞｰﾀ収集範囲の一覧要素を配列で取得
 * @param {html}    btn     : ﾎﾞﾀﾝ要素
 * @param {boolean} isEdit  : 単票ｴﾘｱﾌﾗｸﾞ
 */
function getTargetListElements(btn, isEdit) {

    var targetElements = null;

    var formNo = $(P_Article).attr("data-formno");
    var isComConduct = $(P_Article).attr("name") == "common_area";
    var cmConductId = $(P_Article).attr("data-conductid");
    var ex = isComConduct && (cmConductId != null && cmConductId.length > 0) ? cmConductId : formNo;

    //ﾃﾞｰﾀ収集範囲判定
    var getRangeKbn = judgeGetRangeKbn(btn);
    if (getRangeKbn == getRangeKbnDef.List) {
        //一覧の場合
        var ctrlId = $(btn).closest("table.actionlist").data("relationctrlid");
        var id = ctrlId + "_" + formNo;
        if (isEdit) {
            //単票の場合
            id += "_edit";
        }
        targetElements = $("#" + id);
    }
    else if (getRangeKbn == getRangeKbnDef.Tab) {
        //ﾀﾌﾞの場合
        targetElements = $(btn).closest(".tab_contents").find(".ctrlId:not([id$='_edit'])");
    }
    else if (getRangeKbn == getRangeKbnDef.Area) {
        //ｴﾘｱの場合
        targetElements = $(btn).closest("form").find(".ctrlId:not([id$='_edit'])");
    }
    else {
        //画面全体の場合
        var areas = "#formTop" + "_" + ex + ",#formBottom" + "_" + ex + ",#formDetail" + "_" + ex;
        targetElements = $(P_Article).find(areas).find(".ctrlId:not([id$='_edit'])");
    }
    return targetElements;
}

/**
 * コントロールIDの配列から一覧要素を配列で取得
 * @param {number}          formNo      ：画面番号
 * @param {Array.<string>}  ctrlIdList  ：対象一覧コントロールID配列
 * @return {Array.<HTMLElement>}        ：一覧要素配列
 */
function getTargetListElementsByCtrlId(formNo, ctrlIdList, isEdit) {
    var targetElements = [];
    var myFormNo = getFormNo();
    $.each(ctrlIdList, function (idx, ctrlId) {
        var id = ctrlId + "_" + formNo;
        var targetList = formNo == myFormNo ? $(P_Article).find("#" + id) : $("#" + id);

        if (isEdit) {
            targetList = $("#" + id);
        }

        targetElements.push(targetList);
    });
    return targetElements;
}

/**
 * 対象ｴﾘｱのエラー状態を初期化(.errorcom)
 * @param {boolean} isEdit : 単票ｴﾘｱﾌﾗｸﾞ
 */
function clearErrorComStatusForAreas(isEdit) {

    var areas = []; //対象ｴﾘｱﾘｽﾄ
    if (isEdit) {
        //単票ｴﾘｱの場合
        areas = ["formEdit"];
    }
    else {
        //その他のｴﾘｱの場合
        areas = ["formTop", "formBottom", "formDetail"];
    }
    $.each(areas, function (idx, area) {
        var form = $(P_Article).find("form[id^='" + area + "']");
        clearErrorcomStatus(form);
        form = null;
    });
}

/**
 * 対象ｴﾘｱのエラー状態を初期化(.error/.errorcom)
 * @param {boolean} isEdit : 単票ｴﾘｱﾌﾗｸﾞ
 */
function clearErrorStatusForAreas(isEdit) {

    var areas = []; //対象ｴﾘｱﾘｽﾄ
    if (isEdit) {
        //単票ｴﾘｱの場合
        areas = ["formEdit"];
    }
    else {
        //その他のｴﾘｱの場合
        areas = ["formTop", "formBottom", "formDetail"];
    }
    $.each(areas, function (idx, area) {
        var form = $(P_Article).find("form[id^='" + area + "']");
        clearErrorStatus(form);
        form = null;
    });
}

/**
 * 検索条件退避用から指定した機能ID、画面NOの検索条件を取得
 * @param {string}  conductId   : 機能ID
 * @param {string}  formNo      : 画面NO
 * @return ：検索条件
 */
function getSearchCondition(conductId, formNo) {

    // 検索条件
    var result = null;

    // 検索条件退避用から指定した機能ID、画面NOの検索条件を取得する
    Object.keys(P_SearchCondition).forEach(function (idx) {
        if (P_SearchCondition[idx].conductId == conductId && P_SearchCondition[idx].formNo == Number(formNo)) {
            result = P_SearchCondition[idx].condition;  // ﾘｽﾄ型で保持しているのでそのまま渡す
        }
    })

    return result;
}

/**
 * 検索条件退避用に指定した機能ID、画面NO、検索条件をセット
 * @param {string}  conductId   : 機能ID
 * @param {string}  formNo      : 画面NO
 * @param {string}  condition   : 検索条件
 */
function setSearchCondition(conductId, formNo, conditionDataList) {

    // 検索条件退避用件数
    var cnt = Object.keys(P_SearchCondition).length;

    // 既に機能ID、画面NOが登録されている場合は削除する
    if (cnt > 0) {
        Object.keys(P_SearchCondition).forEach(function (idx) {
            if (P_SearchCondition[idx].conductId == conductId && P_SearchCondition[idx].formNo == formNo) {
                delete P_SearchCondition[idx];
                cnt = idx;
            }
        })
    }

    // 検索条件退避用に指定した機能ID、画面NO、検索条件をセットする
    P_SearchCondition[cnt] = { conductId: conductId, formNo: formNo, condition: conditionDataList };
}

/**
 * 数値の0埋めを行う
 * @param {number} num      :対象の数値
 * @param {number} length   :桁数
 * @return {string} 0埋め後の文字列
 */
function zeroPadding(num, length) {
    return paddingText(num, length);
}

/**
 * 値の桁数を揃える処理
 * @param {any} value       :対象値
 * @param {number} length   :桁数
 * @param {string} text     :埋める文字列
 * @param {boolean} before  :埋める位置(true:対象文字列の前/false:対象文字列の後)
 * 
 */
const paddingText = (value, length, text = "0", before = true) => {
    const valText = value + '';
    const repeatText = text.repeat(length);
    const fromTo = (before) ?
        { text: repeatText + valText, from: -length, to: valText.length + length }
        : { text: valText + repeatText, from: 0, to: length };
    return fromTo.text.slice(fromTo.from, fromTo.to);
}

/**
 * オブジェクト配列のディープコピー
 * @param {Array.<Object>} obj  :オブジェクト配列
 * @return Array.<Object> ディープコピー後のオブジェクト配列
 */
function deepCopyObjectArray(obj) {
    let result, value, key

    if (typeof obj !== "object" || obj === null) {
        // Objectではない、またはnullの場合、そのまま返す
        return obj
    }

    // Arrayの場合はArray、Objectの場合はObjectを生成する
    result = Array.isArray(obj) ? [] : {}

    for (key in obj) {
        value = obj[key]
        // 入れ子になっているArray/Objectを再帰的にディープコピーする
        result[key] = deepCopyObjectArray(value)
    }
    return result
}

/**
 * 機能メニューのsubmit
 * @param {HTMLElement} element :リンク要素
 */
function submitConductMenuLink(element) {
    let form = $("#formMenu");
    $(form).find('input[name="CONDUCTID"]').val($(element).data('conductid'));
    $(form).find('input[name="FactoryIdList"]').val(JSON.stringify(getSelectedFactoryIdList()));
    $(form).submit();
    form = null;
}

/**
 * 偽造防止トークンの取得
 */
function getRequestVerificationToken() {
    return $('input:hidden[name="__RequestVerificationToken"]').val();
}

/**
 * コンボボックスの値リストを取得
 * @param {any} combo コンボボックスの要素
 */
function getComboValueList(combo) {
    var values = [];
    $(combo).find('option').each(function (key, value) {
        values.push($(value).attr("value"));
    });
    return values;
}

/**
 * 英数字を半角と全角で変換する処理
 * @param {string} target 変換する文字列
 * @param {boolean} isToFull 全角に変換する場合True,半角ならFalse
 */
function convertHalfFull(target, isToFull) {
    var repText;
    if (isToFull) {
        repText = /[A-Za-z0-9-!"#$%&'()=<>,.?_\[\]{}@^~\\]/g;
    } else {
        repText = /[Ａ-Ｚａ-ｚ０-９－！"＃＄％＆'（）＝＜＞，．？＿［］｛｝＠＾～￥]/g;
    }
    target = target + '';
    target = target.replace(repText, function (s) {
        return String.fromCharCode(s.charCodeAt(0) + (isToFull ? +1 : -1) * 65248);
    });
    return target;
}

/**
 * 全角の平仮名と片仮名を変換する処理
 * @param {string} target 変換する文字列
 * @param {boolean} isToKatakana カタカナに変換する場合True,平仮名ならFalse
 */
function convertKana(target, isToKatakana) {
    var repText;
    if (isToKatakana) {
        repText = /[ぁ-ん]/g;
    } else {
        repText = /[ァ-ン]/g;
    }
    target = target + '';
    target = target.replace(repText, function (s) {
        return String.fromCharCode(s.charCodeAt(0) + (isToKatakana ? +1 : -1) * 0x60);
    });
    return target;
}

/**
 * 半角カタカナを全角カタカナに変換する処理
 * @param {string} target 変換する文字列
 */
function convertHalfKatakana(target) {
    // 濁音
    const D_TO = 'ガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポヴヷヺ';
    const D_FROM = 'ｶﾞｷﾞｸﾞｹﾞｺﾞｻﾞｼﾞｽﾞｾﾞｿﾞﾀﾞﾁﾞﾂﾞﾃﾞﾄﾞﾊﾞﾋﾞﾌﾞﾍﾞﾎﾞﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟｳﾞﾜﾞｦﾞ';
    // 清音
    const S_TO = 'アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホ'
        + 'マミムメモヤユヨラリルレロワヲンァィゥェォッャュョ。、ー「」・';
    const S_FROM = 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜｦﾝｧｨｩｪｫｯｬｭｮ｡､ｰ｢｣･';

    const map = {};
    for (let i = 0, len = D_TO.length; i < len; i++) {
        map[D_FROM.slice(i * 2, i * 2 + 2)] = D_TO.slice(i, i + 1);
    }
    for (let i = 0, len = S_TO.length; i < len; i++) {
        map[S_FROM.slice(i, i + 1)] = S_TO.slice(i, i + 1);
    }
    const re = new RegExp(Object.keys(map).join('|'), 'g');

    const toZenKata = str => str.replace(re, x => map[x]);

    return toZenKata(target);
}

/**
 * 半角全角、大文字小文字を区別しない文字列比較を行う処理
 * @param {string} fromValue 比較元の値
 * @param {string} fromValue 比較先の値
 * @param {boolean} isEqual 比較が完全一致ならTrue、部分一致ならFalse
 * @return {boolean} 一致する場合True
 */
function isMatchCharacterNoDiff(fromValue, toValue, isEqual) {
    var convertText = function (target) {
        // アルファベットを大文字に変換
        var resutl = (target + '').toUpperCase();
        // 英数字を全角に変換
        result = convertHalfFull(resutl, true);
        // 半角カタカナを全角カタカナに変換
        result = convertHalfKatakana(result);
        return result;
    }
    var compareFrom = convertText(fromValue);
    var compareTo = convertText(toValue);

    if (isEqual) {
        // 完全一致
        return compareFrom == compareTo;
    } else {
        // 部分一致
        return compareFrom.indexOf(compareTo) > -1;
    }
}

/**
 * コンボボックスの先頭を選択する処理
 * @param {any} combo コンボボックス
 */
function selectComboTop(combo) {
    selectComboIdx(combo, 0);
}

/**
 * コンボボックスの指定番号の要素を選択する処理
 * @param {any} combo コンボボックス
 * @param {int} idx 選択する要素の順番
 */
function selectComboIdx(combo, idx) {
    $(combo).prop("selectedIndex", idx);
}

/**
 * 文字列かどうかの判定
 * @param {any} value
 * @return {boolean} : true:文字列 / false:文字列以外
 */
function isString(value) {
    if (typeof value === "string" || value instanceof String) {
        return true;
    } else {
        return false;
    }
}

/**
 * モーダル表示時に背景画面のタブキー移動を制御
 * @param {html} selector：ｲﾍﾞﾝﾄ発生要素
 */
function checkTabFocus(selector) {
    // ローディング画面が呼び出されている場合、要素から抜ける
    if ($("#loading").length > 0) {
        // フォーカス制御不正　不要と思われるのでコメントアウト
        //$(selector).blur();
    }

    // モーダルでない要素にフォーカスがセットされている場合、モーダルの要素にフォーカスをセットする処理
    var escapeFocus = function (target) {
        // 要素がポップアップ画面に含まれているかどうかをチェック
        if ($(target).find(selector).length < 1) {
            var ctrlFocus = $(target).find("tr:not([class^='base_tr']) td input[type='text'],[type='checkbox'],[type='button'],[type='file'],[type='password'],select,textarea,a,a.btn.multisel-text");
            $.each(ctrlFocus, function (i, item) {
                if ($(item).is(":visible") && !$(item).is(":disabled")) {
                    // モーダル画面へタブ移動
                    item.focus();
                    return false;
                }
            });
            ctrlFocus = null;
        } else {
            return false;
        }
    }

    // 確認ポップアップ画面の表示されている場合、前画面のタブ移動を制御する
    if ($("#messageModal").css("display") == "block") {
        return escapeFocus($("#messageModal"));
    }

    // モーダル画面が複数表示される場合があるので、最も手前(z-indexが大きい)のモーダルを選択
    var maxZIndex = 0;
    var topModal;
    $.each($("section.modal_form,#popupTreeViewModal"), function (index, element) {
        var zIndex = $(element).css("z-index") - 0;
        if (maxZIndex < zIndex) {
            maxZIndex = zIndex;
            topModal = element;
        }
    });

    // ポップアップ画面の表示されている場合、遷移前のタブ移動を制御する
    if ($("section.modal_form,#popupTreeViewModal").css("display") == 'block') {
        // 要素がポップアップ画面に含まれているかどうかをチェック
        if ($(topModal).find(selector).length < 1) {
            // カレンダーピッカーの場合、処理を行わない ※ポップアップ画面以下の要素ではないため、フォーカスが誤って抜けたため。
            if ($("#ui-datepicker-div").find(selector).length > 0) { return false; }

            // 先頭のモーダル
            if ($(topModal).css("display") == "block") {
                return escapeFocus($(topModal));
            }
            else {
                return escapeFocus($(P_Article));
            }
        } else {
            return false;
        }
    }
    topModal = null;

    return true;
}

/**
 * タブインデックスを設定
 */
function nextFocus() {
    // タブインデックスの過剰な制御は不要のため、処理を行わず動作を確認
    return;
    /*
    $("#main_contents").removeAttr("tabindex");

    // 入力エリアを取得
    var element = $(P_Article).find("tr:not([class^='base_tr']) td input[type='text'],[type='checkbox'],[type='button'],[type='file'],[type='password'],select,textarea,a,a.btn.multisel-text");
    var maxTabIndex = 0;

    // 初期フォーカスを設定
    var tableId = "";
    var tableIndex = 1;
    // ※基本的に"AAABBBCCDDE" A:ctrlid別、B:インデックス番号、C:列番号、D:行番号、E:その他を想定
    $.each(element, function (i, item) {
        if ($(item).is(":visible") && !$(item).is(":disabled")) {
            // VAL値が設定されている場合
            if ($(item).closest("td").data("name") != null && $(item).closest("td").data("name").substr(0, 3) === "VAL") {
                // aタグかつテキストが未設定の場合、次へ（マルチ選択リストを除外）
                if (item.localName == "a" && item.textContent == "" && item.className.indexOf("multisel-text") < 0) {
                    return true;
                }
                // ctrlidが切り替わる段階で+1を行う
                if (tableId == "" || tableId != $(item).closest("table")[0].id) {
                    tableId = $(item).closest("table")[0].id;
                    tableIndex++;
                }
                // インデックスを取得
                var rowIndex = "000";
                if (!$(item).closest("table").hasClass('vertical_tbl')) {
                    rowIndex = ("000" + $(item).closest("tr").data("rowno")).slice(-3);
                }
                // 列番号、行番号を取得
                var val = $(item).closest("td").data("deftabindex");
                var tabIndex = tableIndex + rowIndex + val + 0;
                setAttrByNativeJs(item, 'tabindex', tabIndex);
                if ((maxTabIndex - 0) < (tabIndex - 0)) {
                    // 最大tabindex番号を更新
                    maxTabIndex = tabIndex;
                }
            }
            else {
                // VAL値が設定されていない場合、最大tabindex番号に+1をして設定
                maxTabIndex++;
                setAttrByNativeJs(item, 'tabindex', maxTabIndex);
            }
        }
    });
    */
}

/**
 * タブインデックスをクリア
 */
function removeFocus() {
    // 入力エリアを取得
    var element = $(P_Article).find("tr:not([class^='base_tr']) td input[type='text'],[type='checkbox'],[type='button'],[type='password'],select,textarea,a,a.btn.multisel-text");
    $.each(element, function (i, item) {
        $(item).removeAttr("tabindex");
    });
    element = null;
}

/**
 * input要素に設定されたJSON文字列をobjectに変換
 * @param {HTMLInputElement} input  :input要素
 */
function deserializeInputJsonText(input) {
    var object = null;
    var jsonStr = $(input).val();

    //デシリアライズ
    if (jsonStr != null && jsonStr.length > 0) {
        object = JSON.parse(jsonStr);
    }
    return object;
}

function getArticleElements(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element, conductId) {
    // 画面上から遷移元の機能IDを取得
    var form = $(P_Article).find("form[id^='formDetail']");
    const parentConductId = $(form).find('input[name="CONDUCTID"]').val();

    var conditionDataList = [];
    conditionDataList.push({ parentConductId: parentConductId });

    var postdata = {
        conductId: conductId,                   // 取得対象機能ID
        conditionData: conditionDataList,       // 親機能ID
        __RequestVerificationToken: getRequestVerificationToken(),  // 偽造防止トークン
    };

    // 部分画面レイアウトデータを取得
    $.ajax({
        url: appPath + 'Common/GetPartialView',    //【共通 - 個別画面遷移】部分画面レイアウトデータ取得
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (data) {
            // DOM要素に変換
            var elements = $.parseHTML(data);
            var mainArea = $(elements).filter('article[name="main_area"]');
            var comArea = $(elements).filter('article[name="common_area"]');
            var authInfo = $(elements).filter('input:hidden[name="UserAuthConductShoris_Temp"]');

            // 個別取得の子画面要素を削除
            var addedMainArea = $('article[name="main_area"][data-parentconductid]');
            if (addedMainArea.length > 0) {
                var addedConductId = $(addedMainArea).data('conductid');
                $('article[name="common_area"][data-parentconductid= "' + addedConductId + '"]').remove();
                $('article[name="main_area"][data-parentconductid= "' + parentConductId + '"]').remove();
            }
            addedMainArea = null;

            // 画面上から親要素を取得
            var parentMainArea = $('article[name="main_area"]');
            var parentComArea = $('article[name="common_area"]');

            // DOMに取得した要素を追加
            if (comArea.length > 0) {
                if (parentComArea.length > 0) {
                    $(comArea).insertAfter(parentComArea[parentComArea.length - 1]);
                } else {
                    $(comArea).insertAfter(parentMainArea[parentMainArea.length - 1]);
                }
            }
            $(mainArea).insertAfter(parentMainArea[parentMainArea.length - 1]);

            // ボタン権限情報を追加
            retainButtonDefines(authInfo);

            // 画面コントロール初期化処理実行
            initAddedArticleControls(appPath, $(mainArea).data('conductid'), elements);

            // 画面遷移処理実行
            transForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element, mainArea[0]);

        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            //異常時
            //処理結果ｽﾃｰﾀｽを画面状態に反映
            setReturnStatus(appPath, resultInfo, null);
        }
    );
    form = null;
}

/**
 * 追加されたArricle上のコントロールの初期化処理
 * @param {string} appPath      ：アプリケーションルートパス
 * @param {stirng} conductId    ：機能ID
 * @param {HTMLElement} elements：追加取得した要素
 */
function initAddedArticleControls(appPath, conductId, elements) {

    // コンボボックス初期化
    var input = $(elements).filter('input:hidden[name="CboDatas_Temp"]');
    var cboData = deserializeInputJsonText(input);
    if (cboData != null) {
        $.each(cboData, function (id, data) {
            initComboBox(appPath, "#" + id, data[0], data[1], data[2], data[3], -1);
            //$.each(cboData, function (idx, id) {
            //callInitComboBox(appPath, "#" + id);
        });
    }
    $(input).remove();
    input = null;


    // オートコンプリート定義情報初期化
    input = $(elements).filter('input:hidden[name="AutoCompDefDatas_Temp"]');
    var autoCompDefData = deserializeInputJsonText(input);
    if (autoCompDefData != null) {
        $.each(autoCompDefData, function (id, data) {
            initAutoCompDef("#" + id, data[0], data[1], data[2]);
        });
    }
    $(input).remove();
    input = null;

    // 複数選択ﾘｽﾄ初期化
    input = $(elements).filter('input:hidden[name="MltSelDatas_Temp"]');
    var mltSelData = deserializeInputJsonText(input);
    if (mltSelData != null) {
        $.each(mltSelData, function (id, data) {
            initMultiSelectBox(appPath, "#" + id, data[0], data[1], data[2], data[3]);
            //$.each(mltSelData, function (idx, id) {
            //callInitMultiSelectBox(appPath, "#" + id);
        });
    }
    $(input).remove();
    input = null;

    // ラジオボタン初期化
    input = $(elements).filter('input:hidden[name="RadioDatas_Temp"]');
    var radioData = deserializeInputJsonText(input);
    if (radioData != null) {
        $.each(mltSelData, function (id, data) {
            initRadioButton(appPath, '[name="' + id + '"]', data[0], data[1], data[2]);
        });
    }
    $(input).remove();
    input = null;

    // チェックボックス初期化
    initCheckBox();

    // コード＋翻訳 初期化
    input = $(elements).filter('input:hidden[name="CodeTransDatas_Temp"]');
    var codeTransData = deserializeInputJsonText(input);
    if (codeTransData != null) {
        $.each(codeTransData, function (id, data) {
            initCodeTrans(appPath, "#" + id, data[0], data[1]);
        });
    }
    $(input).remove();
    input = null;

    // 計算ラベル初期化
    initCalcLabel(appPath);

    //ﾊﾞｯﾁ再表示ﾎﾞﾀﾝ
    input = $(elements).filter('input:hidden[name="BatRefleshBtns_Temp"]');
    var batBtnData = deserializeInputJsonText(input);
    if (batBtnData != null) {
        $.each(batBtnData, function (id, data) {
            //一定間隔で再表示
            execBatRefresh(id); //10秒間隔
        });
    }
    $(input).remove();
    input = null;

    // 以下はメイン機能画面と共通機能画面それぞれで初期化処理の実行が必要

    var mainSelector = 'article[name="main_area"][data-conductid= "' + conductId + '"]';
    var comSelector = 'article[name="common_area"][data-parentconductid= "' + conductId + '"]';

    // 詳細検索メニュー初期値設定
    initDetailSearchCondition('[data-conductid= "' + conductId + '"]');
    var comArticle = $(comSelector);
    if (comArticle.length > 0) {
        var conductIdList = [];
        $.each(comArticle, function (idx, article) {
            var tmpId = $(article).data('conductid');
            if (conductIdList.indexOf(tmpId) < 0) {
                initDetailSearchCondition('[data-conductid= "' + tmpId + '"]');
                conductIdList.push(tmpId);
            }
        });
    }
    comArticle = null;

    // 一覧ソート初期化
    initListSort(appPath, mainSelector + ' th');
    initListSort(appPath, comSelector + ' th');

    // タブ切替の初期化処理
    initTab(mainSelector);
    initTab(comSelector);

    // 各種ボタンの初期化
    input = $(elements).filter('input:hidden[name="FileUploadSize_Temp"]');
    var fileUploadSize = parseInt($(input).val(), 10);
    initButtons(appPath, fileUploadSize, mainSelector);
    initButtons(appPath, fileUploadSize, comSelector);
    $(input).remove();
    input = null;

    // 各種カスタムコントロールの初期化
    initCustomControls(appPath, mainSelector);
    initCustomControls(appPath, comSelector);

}

/**
 * 連想配列の配列の特定のキーの値の最大/最小を取得する処理
 * @param {Array<object>} arr 連想配列の配列
 * @param {string} key 取得する連想配列のキー名
 * @param {boolean} isMax 最大ならTrue、最小ならFalse
 */
function GetLimitPropValue(arr, key, isMax) {
    // 最大or最小の値、比較するので、初期値は最大値なら小さい値、最小値なら大きい値
    var limitValue = isMax ? Number.MIN_VALUE : Number.MAX_VALUE;
    for (var i = 0, len = arr.length; i < len; i++) {
        if (arr[i][key] === undefined || arr[i][key] === null) continue;

        if (typeof limitValue === 'undefined' || isMax ? (arr[i][key] > limitValue) : (arr[i][key] < limitValue)) {
            limitValue = arr[i][key];
        }
    }

    return limitValue;
}

/**
 * 実行ボタンが全て非活性の場合、戻るor閉じるボタンにフォーカス設定
 */
function setFocusBackOrClose() {
    var button = $(P_Article).find('td:not(.hide) input:button');
    //表示中の画面に存在する実行ボタンの数
    var executeButonCount = $(button).filter('[data-actionkbn="' + actionkbn.Execute + '"],[data-actionkbn="' + actionkbn.Delete + '"],[data-actionkbn="'
        + actionkbn.ComBatExec + '"],[data-actionkbn="' + actionkbn.Upload + '"],[data-actionkbn="' + actionkbn.ComUpload + '"]').length;
    //実行ボタンのうち活性のものを取得
    var executeButton = $(button).filter('[data-actionkbn="' + actionkbn.Execute + '"]:not(:disabled),[data-actionkbn="' + actionkbn.Delete + '"]:not(:disabled),[data-actionkbn="'
        + actionkbn.ComBatExec + '"]:not(:disabled),[data-actionkbn="' + actionkbn.Upload + '"]:not(:disabled),[data-actionkbn="' + actionkbn.ComUpload + '"]:not(:disabled)');
    if (executeButonCount > 0 && (!executeButton || executeButton.length == 0)) {
        var backButton = $(P_Article).find('td:not(.hide) input:button[data-actionkbn="' + actionkbn.Back + '"]:not(:disabled)');
        if (backButton && backButton.length > 0) {
            //戻るボタンがある場合、フォーカス設定
            setTimeout(function () {
                //カレンダーが表示されている場合は閉じる
                $("#ui-datepicker-div").hide();
                //フォーカスを外す
                $($(":focus")).blur();
                //フォーカス設定
                $(backButton).focus();
            }, 350);
        } else {
            var closeButton = $(P_Article).find('td:not(.hide) input:button[data-actionkbn="' + actionkbn.Close + '"]:not(:disabled)');
            if (closeButton && closeButton.length > 0) {
                //閉じるボタンがある場合、フォーカス設定
                setTimeout(function () {
                    //カレンダーが表示されている場合は閉じる
                    $("#ui-datepicker-div").hide();
                    //フォーカスを外す
                    $($(":focus")).blur();
                    //フォーカス設定
                    $(closeButton).focus();
                }, 350);
            }
        }
    }
}

/**
 * 数値をカンマ区切りで変換
 * @param {any} num 数値
 */
function setNumberToComma(num) {
    var result = num.replaceAll(',', ''); // カンマを除去
    // 正規表現でフォーマット
    result = result.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
    return result;
}

/**
 * 項目にValidatorのルールを追加する(無効なキーワードが含まれていないかどうか)
 * @param {Element} ele :ルールを追加する項目要素
 */
function addValidatorRuleForInvalidKeyword(ele) {
    //ルールの追加
    var message = { comInvalidKeyword: P_ComMsgTranslated[941120018] }; // 指定できない文字列が含まれています。
    var rule = {};
    rule['comInvalidKeyword'] = true;
    rule['messages'] = message;
    $(ele).rules('add', rule);
}
