/* ========================================================================
 *  機能名　    ：   【共通・一覧制御】
 * ======================================================================== */

/**
 *  画面番号を取得する。
 *  
 *  @return ：画面番号
 */
function getFormNo() {
    var formNo = $(P_Article).data("formno");
    return formNo;
}

/**
 *  機能IDを取得する。
 *  
 *  @return ：機能ID
 */
function getConductId() {
    var conductId = $(P_Article).data("conductid");
    return conductId;
}

/**
 *  table要素Idを取得する。
 *
 *  @tbl {table要素} ：ﾃｰﾌﾞﾙ要素
 *
 *  @return {string} ：table要素Id
 */
function getTableId(tbl) {
    return $(tbl).attr("id");
}

/**
 *  一覧のデータ部の行要素を取得する。
 *  @tbl        {table要素} ：テーブル要素
 */
function getTrsData(tbl) {
    //データ部からベース行以外を取得
    return $(tbl).find("tbody tr:not([class^='base_tr'])");
}

/**
 *  一覧のﾚｲｱｳﾄ行要素を取得する。
 *
 *  @tbl {table要素} ：テーブル要素
 *
 *  @return {tr要素} ：一覧のﾚｲｱｳﾄ行要素
 */
function getBaseTr(tbl) {
    //var ctrlId = getTableId(tbl);

    //一覧のﾚｲｱｳﾄ行要素を取得
    return $(tbl).find(".base_tr");
}

/**
 *  一覧のﾃﾞｰﾀ行の指定列要素を取得する。
 *
 *  @tr {tr要素} ：テーブル要素
 *
 *  @return {td要素} ：列要素
 */
function getDataTd(tr, colno) {
    if ($(tr).hasClass('tabulator-row')) {
        //Tabulator一覧の場合
        return $(tr).find('.tabulator-cell[tabulator-field="VAL' + colno + '"]');
    }
    //データ部からベース行以外を取得
    return $(tr).find('td[data-name="VAL' + colno + '"]');
}

/**
 *  指定列をﾗﾍﾞﾙ列に変更する
 *
 *  @tr {tr要素} ：テーブル要素
 *
 *  @return {td要素} ：列要素
 */
function removeControlTd(td) {

    //入力ｺﾝﾄﾛｰﾙを削除
    //NOリンクを持つセルか？
    var tdNoLink = $(td).find('a[data-type="transLink"]');
    if (tdNoLink.length > 0) {
        //NOリンクを持つものは、その配下を削除
        $(tdNoLink).text("");
    }
    else {
        //上記以外は、セル配下を削除
        $(td).text("");
    }
    tdNoLink = null;
    $(td).removeClass("control");
}

/**
 *  指定列をﾗﾍﾞﾙ列に変更する
 *  ※入力ｺﾝﾄﾛｰﾙに設定されている値をﾗﾍﾞﾙに再配置
 *
 *  @tr {tr要素} ：テーブル要素
 *
 *  @return {td要素} ：列要素
 */
function removeControlTdEx(td) {
    //入力ｺﾝﾄﾛｰﾙの値を取得
    var val = getCellVal(td);
    //指定列をﾗﾍﾞﾙ列に変更
    removeControlTd(td);
    //取得した値をﾗﾍﾞﾙに設定
    $(td).text(val);
}

/**
 *  一覧の指定列をﾗﾍﾞﾙ列に変更する
 *  ※入力ｺﾝﾄﾛｰﾙに設定されている値をﾗﾍﾞﾙに再配置
 *
 *  @ctrlId {string} ：一覧CtrlId
 *  @colNo {string}  ：列番号
 *
 */
function changeCellTypeLabel(ctrlId, colNo) {
    var readonlyTds = $(P_Article).find("#" + ctrlId + " tbody td[data-name='VAL" + colNo + "']");
    $.each(readonlyTds, function (idx, td) {

        //指定列をﾗﾍﾞﾙ列に変更
        //　～入力値を担保する
        removeControlTdEx(td);

        //スペース表示用クラスを設定
        $(td).addClass("ws_pre_wrap");
    });
    readonlyTds = null;
}

/**
 *  行要素から指定列の値が設定されている要素を取得する
 *  @tr         {tr要素} ：行要素
 *  @colno      {num}    ：列番号(1～)
 *  @celltype   {num}    ：セルタイプ(nullの場合、tdタグの値を返す)
 */
function getElement(tr, colno, celltype) {

    var retElement = null;

    //列番号のtdタグ要素を取得
    var td = $(tr).find('td[data-name="VAL' + colno + '"]');
    if (td != null) {
        if (celltype == 1 ||
            celltype == 6 ||
            celltype == 7) {
            //input[type="text"]
            retElement = $(td).find('input[type="text"]');
        }
        else if (celltype == 4) {   //ﾁｪｯｸﾎﾞｯｸｽ
            //input[type="checkbox"]
            retElement = $(td).find('input[type="checkbox"]');
        }
        else if (celltype == 5) {   //ｺﾝﾎﾞﾎﾞｯｸｽ
            //select
            retElement = $(td).find('select');
        }
        else {
            //hidden
            retElement = $(td).find('input[type="hidden"]');
        }

        //ｺﾝﾄﾛｰﾙが出力されていない場合、tdタグ
        if (retElement == null) {
            //td
            retElement = td;
        }
    }
    td = null;

    return retElement;
}

/**
 *  行要素から指定列の値を取得する
 *  @tr         {tr要素} ：行要素
 *  @colno      {num}    ：列番号(1～)
 *  @celltype   {num}    ：セルタイプ(nullの場合、tdタグの値を返す)
 */
function getVal(tr, colno, celltype) {

    var retVal = null;

    //値が設定されている要素を取得
    var valElement = getElement(tr, colno, celltype);
    if (valElement != null) {
        if (celltype == 4 ||
            celltype == 5) {    //ﾁｪｯｸﾎﾞｯｸｽ,ｺﾝﾎﾞﾎﾞｯｸｽ
            //input[type="checkbox"]
            //select
            retVal = $(valElement).val();
        }
        else {
            //input[type="text"]
            //td
            retVal = $.trim($(valElement).text());
        }
    }
    valElement = null;

    return retVal;
}

/**
 *  条件一覧ﾃｰﾌﾞﾙ要素を取得する。
 *  @return {element} ：条件ｴﾘｱ一覧要素
 */
function getConditionTable() {
    // 標準の検索条件に加え、個別に指定された検索条件「search_condition_add」も検索条件として指定する
    return $(P_Article).find(".search_div .ctrlId,.search_condition_add");    //一覧定義単位
}

/**
 *  検索条件ﾃﾞｰﾀを取得する
 *
 *  @param {byte}   ：画面番号
 *  @param {table}  ：条件ｴﾘｱ一覧要素
 *  @param {number} ：値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 *
 *  @return {List<Dictionary<string, object>>} : 条件ﾃﾞｰﾀ(VAL1～)
 */
function getConditionData(formNo, listsCondition, isDispVal) {
    if (listsCondition.length <= 0) {
        return null;
    }
    if (isDispVal == null) isDispVal = 0;   //初期化

    var conditionDataList = [];
    $.each($(listsCondition), function (idx, list) {

        var val = "";
        var ctrlId = $(list).attr("data-ctrlid");

        // 検索条件取得
        //CTRLID,FORMNO
        var conditionData = {
            CTRLID: ctrlId,     // 一覧のctrlId
            FORMNO: formNo,     // 画面番号
            ROWNO: 1,           // 行番号
        };

        //条件ﾃﾞｰﾀ行
        var dataTr = $(list).find("tbody tr:not([class^='base_tr'])");
        if ($(dataTr).length) {

            // 選択チェックボックス
            var td = $(dataTr).find("td[data-name='SELTAG']");
            if (td != null && td.length > 0) {
                //ｾﾙの値(表示値、入力値)を取得
                var valW = getCellVal(td);
                conditionData[$(td).data("name")] = valW;
            }
            td = null;

            //VAL1～
            var tds = $(dataTr).find("td[data-name^='VAL']");
            $.each(tds, function (i, td) {

                //ｾﾙの値(表示値、入力値)を取得
                var valW = getCellVal(td, isDispVal);
                conditionData[$(td).data("name")] = valW;

            });
            tds = null;

            //排他ﾃﾞｰﾀ保持用列
            td = $(dataTr).find("td[data-name='lockData']");
            conditionData["lockData"] = $(td).text();
            td = null;
        }
        dataTr = null;

        conditionDataList.push(conditionData);
    });
    return conditionDataList;
}

/**
 *  詳細検索条件ﾃﾞｰﾀを取得する
 *
 *  @param {string} ：機能ID
 *  @param {number} ：画面番号
 *  @param {number} ：値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 *  @param {boolean}：フォーム初期化時フラグ(true：フォーム初期化時/false：初期化以外)
 *  @param {boolean}：チェック状態取得フラグ(true：取得する/false：取得しない)
 *
 *  @return {List<Dictionary<string, object>>} : 条件ﾃﾞｰﾀ(VAL1～)
 */
function getDetailConditionData(conductId, formNo, isDispVal, isInitForm, getCheckedSts) {
    var conditionDataList = [];
    if (isDispVal == null) isDispVal = 0;   //初期化

    var menuDiv = $('#detail_search');
    var condTables = $(menuDiv).find("table.detailsearch[data-conductid='" + conductId + "'][data-formno='" + formNo + "']");
    menuDiv = null;
    if (condTables == null || condTables.length == 0) { return conditionDataList; }

    $.each($(condTables), function (idx, tbl) {
        var ctrlId = $(tbl).attr("data-parentid");

        if (isInitForm) {
            // フォーム初期化時の場合、ローカルストレージから取得
            const savedData = getSavedDataFromLocalStorage(localStorageCode.DetailSearch, conductId, formNo, ctrlId);
            if (savedData != null && savedData.length > 0) {
                var conditionData = savedData[0];
                var checked = false;
                $.each(conditionData, function (name, data) {
                    if (name.indexOf('VAL') < 0 || name.indexOf('_checked') < 0) { return true; }
                    if (!data) {
                        // チェック無しの場合、設定値をリストから削除
                        delete conditionData[name.replace('_checked', '')];
                    } else {
                        // チェック有り
                        checked = true;
                    }
                    // チェック状態格納用のデータをリストから削除
                    delete conditionData[name];
                });
                if (checked) {
                    conditionDataList.push(conditionData);
                }
            }
        } else {
            // フォーム初期化時以外の場合、画面上から検索条件取得
            //CTRLID,FORMNO
            var conditionData = {
                CTRLID: ctrlId,             // 一覧のctrlId
                FORMNO: formNo,             // 画面番号
                ROWNO: 1,                   // 行番号
                IsDetailCondition: true,    // 詳細検索条件フラグ：ON
            };

            var chkbox = null;
            if (getCheckedSts) {
                // すべての行を取得
                chkbox = $(tbl).find("td.select input[type='checkbox']");
            } else {
                // チェックされた行のみ値を取得
                chkbox = $(tbl).find("td.select input[type='checkbox']").filter(":checked");
                if (chkbox == null || chkbox.length == 0) { return true; }
            }
            $.each($(chkbox), function () {
                var tr = $(this).closest("tr");
                var itemNo = $(tr).data("itemno");
                var td = $(tr).find("td.input_td");

                //VAL1～
                //ｾﾙの値(表示値、入力値)を取得
                var valW = getCellVal(td, isDispVal);
                conditionData['VAL' + itemNo] = valW;
                if (getCheckedSts) {
                    conditionData['VAL' + itemNo + '_checked'] = $(this).is(':checked');
                }
                var nullSearchTr = $(tr).next('.null_search_tr[data-itemno="' + itemNo + '"]');
                if (nullSearchTr && nullSearchTr.length > 0) {
                    var nullSearchCheck = $(nullSearchTr).find("input[type='checkbox']").filter(":checked");
                    if (nullSearchCheck && nullSearchCheck.length > 0 && $(nullSearchCheck).is(':checked')) {
                        conditionData['VAL' + itemNo + '_nullSearch'] = true;
                    }
                }
                td = null;
                tr = null;
            });
            chkbox = null;

            conditionDataList.push(conditionData);
        }
    });
    condTables = null;

    return conditionDataList;
}

/**
 *  横方向一覧の明細データ取得を取得する
 *  @param {byte}      ：画面番号
 *  @param {table要素} ：明細一覧table
 *  @param {number}    ：値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用、1:表示値を採用)
 */
function getListDataHorizontal(formNo, tbl, isDispVal, selectCtrlId, selRowNo) {

    if (isDispVal == null) isDispVal = 0;

    // 一覧の明細データ取得(入力値)
    var listData = [];

    //選択一覧か？
    var isTarget = false;
    if (selectCtrlId != null && selectCtrlId.length > 0) {
        if ($(tbl).attr("id") == selectCtrlId) {
            isTarget = true;
        }
    }

    var ctrlType = $(tbl).data('ctrltype');
    if (ctrlType == ctrlTypeDef.IchiranPtn3) {
        var id = "#" + $(tbl).attr("id");
        var table = P_listData[id];
        if (!table) {
            return listData;
        }
        var rows = table.getRows("active");
        $.each(rows, function (i, row) {
            var rowNo = row.getData().ROWNO;
            var tmpData = getTempDataForTabulator(formNo, rowNo, id, isDispVal);
            listData.push(tmpData);
        });
        rows = null;
        table = null;

        //退避している削除行を取得
        var deleteData = P_deleteData[id];
        //退避している削除行を追加する
        listData = listData.concat(deleteData);
    } else {
        // ﾍﾞｰｽ行（"base_tr"始まりのｸﾗｽ持ち）以外のﾃﾞｰﾀ行を取得
        var trsRowFirst = $(tbl).find("tbody tr:not([class^='base_tr'])[data-rownoidx='1']");   //各行の1段目のtrを取得※1ﾚｺｰﾄﾞ複数行表示対応

        // 明細ﾃﾞｰﾀﾘｽﾄを取得する
        $.each($(trsRowFirst), function (i, tr) {
            //行番号
            var rowNo = $(tr).attr("data-rowno");

            // 指定行の明細ﾃﾞｰﾀを取得
            var tmpData = getTempData(formNo, tbl, rowNo, isDispVal);
            listData.push(tmpData);
        });
        trsRowFirst = null;
    }
    return listData;
}

/**
 *  縦方向一覧の明細データ取得を取得する
 *  @param {byte}      ：画面番号
 *  @param {div要素}   ：ctrlId単位の明細一覧div
 */
function getListDataVertical(formNo, div) {

    // 一覧の明細データ取得(入力値)
    var listData = [];

    //※縦方向一覧の場合、<div>ﾀｸﾞに行番号情報を保持
    var rowNo = $(div).data("rowno");
    var rowStatus = rowStatusDef.Edit;
    // rowNo が取得できない場合 null
    if (rowNo <= 0 || rowNo == null) {
        rowStatus = rowStatusDef.New;   //2:新規行
    }

    var makeTmpData = function (rowNo, formNo, div, rowStatus) {
        var tmpData = {
            FORMNO: formNo,                 // 画面番号
            CTRLID: $(div).data("ctrlid"),  // 一覧の画面定義のコントロールID
            ROWNO: rowNo,                       // 表示ﾃﾞｰﾀの行番号
            ROWSTATUS: rowStatus            // 行ステータス：編集可
        };
        return tmpData;
    }
    // 一行分の明細ﾃﾞｰﾀを取得（入力値）
    var tmpData = makeTmpData(1, formNo, div, rowStatus);

    var setFormValue = function (div, rowNo, tmpData) {
        // 各行のデータ列から各項目のデータを取得
        var trs = $(div).find("table[data-rowno=" + rowNo + "]").find("tbody tr");
        $.each($(trs), function (i, tr) {
            var tds = $(tr).find("td[data-name^='VAL']");
            $.each($(tds), function (i, td) {

                //ｾﾙの値(表示値、入力値)を取得
                var val = getCellVal(td);
                tmpData[$(td).data("name")] = val;

            });
        });
        //排他ﾃﾞｰﾀ保持用列
        var td = $(trs).find('td[data-name="lockData"]').eq(0);
        tmpData["lockData"] = $(td).text();
    }
    // 行の情報を取得
    setFormValue(div, 1, tmpData);
    td = null;
    trs = null;

    listData.push(tmpData);

    // 追加行のデータ取得(複数ツリー選択)
    let trAdd = $(div).find("table[data-rowno!=1]").find("tbody tr");
    if (trAdd == null || trAdd.length == 0) {
        // 無い場合終了
        return listData;
    }
    let addRowNo = 2; // 2行目以降
    while (true) {
        let target = $(div).find("table[data-rowno=" + addRowNo + "]").find("tbody tr");
        if (target == null || target.length == 0) {
            // 無い場合終了
            break;
        }
        // 取得して次へ
        var newData = makeTmpData(addRowNo, formNo, div, rowStatus);
        setFormValue(div, addRowNo, newData);
        listData.push(newData);
        addRowNo++;
    }

    return listData;

}

/**
 *  横方向一覧の指定行の明細ﾃﾞｰﾀを取得する
 *  @param {byte}      ：画面番号
 *  @param {table要素} ：明細一覧table
 *  @param {number}    ：選択行番号
 *  @param {number}    ：値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 */
function getTempData(formNo, tbl, selectRow, isDispVal) {
    if (isDispVal == null) isDispVal = 0;   //初期化

    //指定行要素を取得
    var trs = $(tbl).find('tbody tr:not([class^="base_tr"])[data-rowno="' + selectRow + '"]');
    if (trs.length <= 0) return null;

    //var updateTag = null;
    //var tdUpdTag = $(trs).find("td[data-name='UPDTAG']");
    //if (tdUpdTag.length > 0) {
    //    var valUpdTag = $(tdUpdTag).text();
    //    if (valUpdTag == "*") {
    //        updateTag = updtag.Update;  //3：更新済み
    //    }
    //    else
    //    {
    //        updateTag = updtag.Input;
    //    }
    //}

    // 横方向一覧の指定行の明細ﾃﾞｰﾀを取得する
    var tmpData = tmpData = {
        FORMNO: formNo,                     // 画面番号
        CTRLID: $(tbl).data("ctrlid"),          // 一覧の画面定義のコントロールID
        ROWNO: selectRow,                   // 一覧項目定義のレコード行番号
        ROWSTATUS: $(trs[0]).attr("data-rowstatus"),  // 一覧項目定義の行ステータス
        //UPDTAG: updateTag                   // 一覧項目定義の行ステータス
    };

    $.each($(trs), function (i, tr) {

        // 更新フラグ
        var td = $(tr).find("td[data-name='UPDTAG']");
        if (td != null && td.length > 0) {
            //ｾﾙの値(表示値、入力値)を取得
            var valW = getCellVal(td);
            tmpData[$(td).data("name")] = valW;
        }
        td = null;

        // 選択チェックボックス
        td = $(tr).find("td[data-name='SELTAG']");
        if (td != null && td.length > 0) {
            //ｾﾙの値(表示値、入力値)を取得
            var valW = getCellVal(td);
            tmpData[$(td).data("name")] = valW;
        }
        td = null;

        // 各セル毎にデータをセット
        $.each($(tr).find("td[data-name^='VAL']"), function (i, td) {

            //ｾﾙの値(表示値、入力値)を取得
            var val = getCellVal(td, isDispVal);
            tmpData[$(td).data("name")] = val;

        });

        // 排他データを既に保持している場合、処理を行わない
        if (!("lockData" in tmpData) || tmpData["lockData"] == null) {
            //排他ﾃﾞｰﾀ保持用列
            td = $(tr).find("td[data-name='lockData']");
            tmpData["lockData"] = $(td).text();
            td = null;
        }

    });
    trs = null;

    return tmpData;
}

/**
 *  横方向一覧の指定行の明細ﾃﾞｰﾀを取得する(Tabulator用)
 *  @param {number} formNo              ：画面番号
 *  @param {number} selectRow           ：選択行番号
 *  @param {string} id                  ：一覧のID
 *  @param {number} isDispVal           ：値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 *  @param {boolean} isCalledUpdateFunc ：更新値保存関数からの呼び出しかどうか
 */
function getTempDataForTabulator(formNo, selectRow, id, isDispVal, isCalledUpdateFunc) {
    if (isDispVal == null) isDispVal = 0;   //初期化

    //指定行要素を取得
    var table = P_listData[id];
    var rows = table.searchRows("ROWNO", "=", selectRow);
    var rowData = table.searchData("ROWNO", "=", selectRow)[0];
    table = null;
    if (rows.length <= 0) return null;

    var rowEle = rows[0].getElement();
    rows = null;

    //選択チェックボックスはチェック切り替えの度にデータを更新するため、改めて取得不要
    //var seltagVal = $(rowEle).find("input[data-name='SELTAG']").prop("checked") ? 1 : 0;
    //rowData["SELTAG"] = seltagVal;

    //更新フラグ
    var updtagVal = $(rowEle).find("[data-type='updflg']").val();
    rowData["UPDTAG"] = updtagVal;

    // 横方向一覧の指定行の明細ﾃﾞｰﾀを取得する
    var tmpData = {
        FORMNO: formNo,                     // 画面番号
        //CTRLID: $(tbl).attr("id"),          // 一覧の画面定義のコントロールID
        //ROWNO: selectRow,                   // 一覧項目定義のレコード行番号
        //ROWSTATUS: "",//$(trs[0]).attr("data-rowstatus"),  // 一覧項目定義の行ステータス
        //UPDTAG: updateTag                   // 一覧項目定義の行ステータス
    };

    // 各セル毎にデータをセット(入れ子一覧の場合、子一覧の情報は取得しない)
    $.each($(rowEle).children(".tabulator-cell[tabulator-field^='VAL']"), function (i, cell) {
        //ｾﾙの値(表示値、入力値)を取得
        var val = getCellVal(cell, isDispVal);
        if ($.type(val) == "string") {
            val = val.trim();
        }

        //ダウンロードリンク、ファイルリンクの場合、データを置き換えない
        var fileLink = $(cell).find('a[data-type="download"], a[data-type="fileOpen"]');
        if (fileLink.length == 0) {
            rowData[$(cell).attr("tabulator-field")] = val;
        }
        fileLink = null;
    });

    if (typeof getCustomTempDataForTabulator !== 'undefined') {
        // カスタム列のデータ取得処理が存在する場合、呼び出し
        getCustomTempDataForTabulator(rowData, rowEle, isCalledUpdateFunc);
    }

    //排他ﾃﾞｰﾀ保持用列
    lockData = $(rowEle).find(".tabulator-cell[tabulator-field='lockData']");
    rowData["lockData"] = $(lockData).text();
    lockData = null;
    rowEle = null;

    return Object.assign(tmpData, rowData);
}

/**
 *  ｾﾙの値(表示値、入力値)を取得する
 *  @param {td要素} ：ｾﾙ要素
 *  @param {number} ：値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 *  @return {string} : ｾﾙの値
 */
function getCellVal(td, isDispVal) {
    if (isDispVal == null) isDispVal = 0;   //初期化

    var val = "";

    //複数選択ﾘｽﾄ（※inputﾀｸﾞと間違わないように先に実施）
    var msul = $(td).find("ul.multiSelect");
    if (msul.length > 0) {
        var checkes = null;

        //参照モードかどうか取得
        var referenceFlg = $(td).closest("div.vertical_tbl.ctrlId").filter("[data-referencemode='" + referenceModeKbnDef.Reference + "']");
        if (referenceFlg && referenceFlg.length > 0) {
            //参照モードの場合、削除アイテムも取得する
            checkes = $(msul).find("li:not(.hide) :checkbox:checked");
        } else {
            //編集モードの場合、削除アイテムは取得しない
            checkes = $(msul).find("li>ul>li:not(.hide):not(.deleteItem) :checkbox:checked");
        }
        if (checkes != null && checkes.length > 0) {
            var vals = [];
            $.each(checkes, function () {
                var item = '';
                if (isDispVal == 1) {   //表示値
                    var li = $(this).parent().find('span');
                    if (li != null && li.length > 0) item = $(li[0]).text();
                }
                else {
                    item = $(this).val();   //ｺｰﾄﾞ値
                }
                vals.push(item);
            });
            val = vals.join('|');
        }
        checkes = null;
        msul = null;
        return val;
    }

    //ﾁｪｯｸﾎﾞｯｸｽ
    var checkbox = $(td).find(":checkbox");
    if (checkbox.length > 0) {
        //if (isDispVal == 1) {   //表示値
        //    val = $(checkbox).prop("checked") ? DispValChecked : '';
        //}
        //else {
        //    val = $(checkbox).prop("checked") ? 1 : 0;

        //}
        val = $(checkbox).prop("checked") ? 1 : 0;
        checkbox = null;
        return val;
    }

    //ｺﾝﾎﾞﾎﾞｯｸｽ、ﾘｽﾄﾎﾞｯｸｽ
    var select = $(td).find("select");
    if (select.length > 0) {
        var selected = $(select).find('option:selected');
        if (selected != null && selected.length > 0) {
            var vals = [];
            $.each(selected, function () {
                var item = '';
                if (isDispVal == 1) {   //表示値
                    item = $(this).text();
                }
                else {
                    if ($(select).data("create") == "1") {
                        //ｺﾝﾎﾞ生成中の場合
                        item = $(select).data("value");  //初期値を取得
                    } else {
                        item = $(this).val();
                    }

                    // 取得できていない場合はコード値
                    if (!item) {
                        item = $(this).val();
                    }
                }
                vals.push(item);
            });
            val = vals.join('|');
        } else {
            // optionタグが出力されていない場合、data-value属性から値を取得する
            var label = $(select).parent().siblings('.labeling');
            if (label.length > 0 && $(label).text().length > 0) {
                // ラベルに表示値が設定されている場合のみ
                val = $(select).data('value');
            }
            label = null;
        }
        select = null;
        return val;
    }

    //ラジオボタン
    var radio = $(td).find(":radio");
    if (radio.length > 0) {
        var val = $(td).find(":radio:checked").val();
        radio = null;
        return val;
    }

    //ﾃｷｽﾄ、数値、ﾃｷｽﾄｴﾘｱ、ｺｰﾄﾞ＋翻訳
    var input = $(td).find('input[type="text"], input[type="hidden"], textarea, input[type="password"]');
    if (input.length > 0) {
        val = $(input).val();
        if (input.length > 1) {
            if ($(input[0]).data('autocompdiv') != autocompDivDef.NameOnly) {
                val = val + '|' + $(input[1]).val();
            } else {
                // オートコンプリートの翻訳の表示の場合、hiddenのコードを返す
                val = $(input).filter('.autocomp_code').val();
            }
        }
        if ($(input).data('type') == 'num') {
            unit = $(input[0]).parent().find('.unit').text();
            if (unit.length > 0) {
                val = val + '@' + unit;
            }
            val = val.replace(/,/g, "");
        }
        input = null;
        return val;
    }

    //日付(ブラウザ標準)、時刻、日時(ブラウザ標準)
    var dateTime = $(td).find("input[type='date'], input[type='time'], input[type='datetime-local']");
    if (dateTime.length > 0) {
        val = $(dateTime).val().replace(/-/g, "/").replace("T", " ");
        if (dateTime.length > 1) {
            val = val + '|' + $(dateTime[1]).val().replace(/-/g, "/").replace("T", " ");
        }
        dateTime = null;
        return val;
    }

    //ボタン
    var button = $(td).find(":button");
    if (button != null && button.length > 0) {
        //※空文字を返す
        button = null;
        return "";
    }

    var dataType = $(td).data("type");

    //ツリー選択ラベル
    if (typeof dataType !== 'undefined' && dataType == 'treeLabel') {
        if (isDispVal == 1) {   //表示値
            val = getTdText(td);
        }
        else {
            val = $(td).data("structureid");
        }
        return val;
    }

    //ﾗﾍﾞﾙ
    //遷移ﾘﾝｸ列か？
    var transLink = $(td).find('a');
    if (transLink.length > 0) {
        val = $(transLink[0]).text();
    }
    else {
        //コード値が存在するか？
        if (typeof $(td).data('value') !== 'undefined') {
            val = $(td).data('value');
        }
        else {
            val = getTdText(td);
        }
    }
    transLink = null;
    return val;
}

/**
 *  ｾﾙの値(表示値、入力値)をクリアする
 * 
 *  @param {td要素} ：ｾﾙ要素
 *  @param {number} ：値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 *
 *  @return {string} : ｾﾙの値
 */
function clearCellVal(td, isDispVal) {
    if (isDispVal == null) isDispVal = 0;   //初期化

    //ﾗﾍﾞﾙ表示用spanタグを持つ場合は併せてｸﾘｱする
    var span = $(td).find("span.labeling");
    if ($(span).length) {
        $(span).text("");
    }
    span = null;

    //複数選択ﾘｽﾄ（※inputﾀｸﾞと間違わないように先に実施）
    var msul = $(td).find("ul.multiSelect");
    if (msul.length > 0) {
        var checkes = $(msul).find("> li:not(.hide) :checkbox:checked");
        if (checkes != null && checkes.length > 0) {
            $(checkes).prop('checked', false);
            setAttrByNativeJs(checkes, "checked", false);
        }
        msul = null;
        return;
    }

    //ﾁｪｯｸﾎﾞｯｸｽ
    var checkbox = $(td).find(":checkbox");
    if ($(checkbox).length) {
        var initChecked = $(checkbox).attr("value") == "1";
        $(checkbox).prop("checked", initChecked);
        checkbox = null;
        return;
    }

    //ｺﾝﾎﾞﾎﾞｯｸｽ
    var select = $(td).find("select");
    if (select.length > 0) {
        if (isDispVal == 1) {   //表示値
            var selected = $(select).find('option:selected');
            setAttrByNativeJs(selected, "selected", false);
        }
        else {
            if ($(select).data("create") != "1") {
                //$(select).val("");
                $(select).val($(td).data("initverticalvalue"));
            }
        }
        select = null;
        return;
    }

    //ラジオボタン
    var radio = $(td).find(":radio");
    if ($(radio).length) {
        $.each(radio, function () {
            $(this).prop('checked', false);
        });
        radio = null;
        return;
    }

    //ﾃｷｽﾄ、ﾃｷｽﾄｴﾘｱ、ｺｰﾄﾞ＋翻訳
    var input = $(td).find('input[type="text"][data-type!="num"], input[type="hidden"], textarea');
    if (input.length > 0) {
        //$(input).val("");
        if ($(input).hasClass('fromto') && input.length > 1) {
            var values = $(td).data("initverticalvalue").split('|'); // fromto分割
            // From 
            $(input[0]).val(values[0]);
            // To
            if (values.length > 1) {
                $(input[1]).val(values[1]);
            }
        }
        else {
            $(input).val($(td).data("initverticalvalue"));
        }
        //ｺｰﾄﾞ＋翻訳の翻訳表示用spanもクリアする
        var span = $(td).find('span.honyaku');
        if (span.length > 0) {
            $(span).text("");
        }
        input = null;
        return;
    }

    // 数値
    input = $(td).find('input[type="text"][data-type="num"]');
    if (input.length > 0) {
        setDataForTextNum(td, input, $(td).data("initverticalvalue"));
        input = null;
        return;
    }

    //日付(ブラウザ標準)、時刻、日時(ブラウザ標準)
    var dateTime = $(td).find("input[type='date'], input[type='time'], input[type='datetime-local']");
    if (dateTime.length > 0) {
        //$(dateTime).val("");
        $(dateTime).val($(td).data("initverticalvalue"));
        // SysDateのときに「SysDate」が設定されてしまうので再セット
        setInitDateValue(dateTime);
        dateTime = null;
        return;
    }

    //ﾎﾞﾀﾝ
    var btn = $(td).find("input[type='button']");
    if ($(btn).length) {
        btn = null;
        return; //ﾎﾞﾀﾝは消さない
    }

    //ファイル選択
    var file = $(td).find("input[type='file']");
    if (file.length > 0) {
        file = null;
        return;
    }

    var dataType = $(td).data("type");

    //ツリー選択ラベル
    if (typeof dataType !== 'undefined' && dataType == 'treeLabel') {
        var values = $(td).data("initverticalvalue").split('|'); // 表示文字列と構成IDに分割
        $(td).text(values[0]);
        if (values.length > 1) {
            setAttrByNativeJs(td, 'data-structureid', values[1]);
        } else {
            setAttrByNativeJs(td, 'data-structureid', '');
        }
        return;
    }

    //スケジュール表示ラベル
    if (typeof dataType !== 'undefined' && dataType == 'scheduleLabel') {
        $(td).remove('[class*="sc-mark"]');
        return;
    }

    // ダウンロードリンク、ファイル参照リンク
    var downloads = $(td).find("a[data-type='download'], a[data-type='fileOpen']");
    if (downloads != null && downloads.length > 0) {
        //hrefと表示ラベルをクリア
        setAttrByNativeJs(downloads[0], 'href', '');
        $(downloads[0]).text('');
        //リンクタグを退避
        var link = $(downloads[0]).clone(true);
        //tdタグ内をクリア
        $(td).empty();
        //退避したリンクタグを追加
        $(td).append(link);
        link = null;
        downloads = null;
        return true;
    }

    //ﾗﾍﾞﾙ
    //コード値が存在するか？
    if (typeof $(td).data('value') !== 'undefined') {
        setAttrByNativeJs(td, "data-value", "");
    }
    else {
        //$(td).text("");
        $(td).text($(td).data("initverticalvalue"));
    }
    return;
}

/**
 *  対象行trを取得する
 *
 *  @param {入力要素}  ：input要素
 *
 *  @return {tr要素}    : 対象行tr要素
 */
function getDataTr(selector) {
    var tr;
    var isHorizontal = true;
    var tabulatorRow = $(selector).closest('.tabulator-row');
    if (tabulatorRow && tabulatorRow.length > 0) {
        //Tabulator一覧の場合
        tr = tabulatorRow;
    } else {
        tr = $(selector).closest('tr');
        var tables = $(tr).closest('table');
        $.each(tables, function (idx, table) {
            if ($(table).hasClass('vertical_tbl')) {
                //【縦方向一覧】 ※複数列考慮
                tr = $(tr).closest(".ctrlId");
                isHorizontal = false;
            }
            else {
                //【横方向一覧】 ※複数行考慮
                var currowno = $(tr).data("rowno");
                tr = $(tables).find("tr[data-rowno='" + currowno + "']");
            }
        });
        tables = null;
    }
    tabulatorRow = null;
    return { tr: tr, isHorizontalTbl: isHorizontal };
}

/**
 *  検索条件ﾃﾞｰﾀの入力検証を行う
 *
 *  @return {bool} : ture(OK) false(NG)
 */
function validConditionData() {

    var formSearch = $(P_Article).find("form[id^='formSearch']");
    var formId = $(formSearch).attr("id");
    var isValid = true;
    if (formSearch.length > 0) {
        var switchids = $(formSearch).find("a[data-switchid]");
        var isHiddens = {};
        $.each(switchids, function (idx, switchid) {
            var id = $(switchid).data("switchid");
            var isHidden = isHideId(id);
            isHiddens[id] = isHidden;
            if (isHidden) {
                //表示/非表示切替
                setHideId(id, !isHidden);
            }
        });
        if (!$(formSearch).valid()) {

            isValid = false;

            // 個別ｴﾗｰは最初は非表示
            //var errorHtml = $(formSearch).find("label.errorcom");
            var errorHtml = $("div.errtooltip").find("label.errorcom");
            $(errorHtml).hide();
            errorHtml = null;

            //複数選択ﾘｽﾄのｴﾗｰを<ul>ﾀｸﾞで拾う
            setMultiSelectError(formSearch);

            //個別エラー状態を初期化
            clearErrorStatus3("#" + formId);
        }

        $.each(switchids, function (idx, switchid) {
            var id = $(switchid).data("switchid");
            var errorInSwitch = $("#" + id).find(".errorcom");
            if (!($(errorInSwitch).length)) {
                if (isHiddens[id]) {
                    //表示/非表示切替
                    setHideId(id, isHiddens[id]);
                }
            }
            errorInSwitch = null;
        });
        switchids = null;
    }
    formSearch = null;

    if (!isValid) {
        //【CJ00000.W01】入力エラーがあります。
        addMessage(P_ComMsgTranslated[941220007], 1);
        $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
        $(P_Article).find(".search_div").focus();
        return false;
    }
    return true;    //入力ﾁｪｯｸOK
}

/**
 *  明細ﾃﾞｰﾀの入力検証を行う
 *
 *  @param {int} ：入力ﾁｪｯｸﾚﾍﾞﾙ（1:必須ﾁｪｯｸを行わない）
 *
 *  @return {bool} : ture(OK) false(NG)
 */
function validListData(level) {

    if (level == null) level = 0;

    // ﾀﾌﾞﾎﾞﾀﾝが存在する場合、一時的に全ﾀﾌﾞ表示して入力ﾁｪｯｸを行う。
    var tabNo = 0;    //選択ﾀﾌﾞ番号

    var tabContents = $(P_Article).find(".detail_div .tab_contents");
    if (tabContents != null && tabContents.length > 0) {
        $.each($(tabContents), function (i, div) {
            //表示状態ﾁｪｯｸ
            if ($(div).hasClass('selected')) {
                tabNo = $(div).data('tabno');
            }
            else {
                //一時的に表示状態とする
                $(div).addClass('selected');
            }
        });
    }

    // 共通入力チェック - Validatorの実行
    var formDetail = $(P_Article).find("form[id^='formDetail']");

    // validationのため、畳んだ一覧は展開する
    var switchids = $(formDetail).find("a[data-switchid]");
    var isHiddens = {};
    $.each(switchids, function (idx, switchid) {
        var id = $(switchid).data("switchid");
        var isHidden = isHideId(id);
        isHiddens[id] = isHidden;
        if (isHidden) {
            //表示/非表示切替
            setHideId(id, !isHidden);
        }
    });
    var valid = $(formDetail).valid();
    if (!valid) {
        // 個別ｴﾗｰは最初は非表示
        //var errorHtml = $(formDetail).find("label.errorcom");
        var errorHtml = $("div.errtooltip").find("label.errorcom");
        $(errorHtml).hide();
        errorHtml = null;

        //複数選択ﾘｽﾄのｴﾗｰを<ul>ﾀｸﾞで拾う
        setMultiSelectError(formDetail);

        //直接入力ﾊﾟﾀｰﾝの一覧について、行単位に検証
        //直接入力ﾊﾟﾀｰﾝの一覧要素
        var tables = $(formDetail).find("[data-editkbn='" + editPtnDef.Input + "']");
        if (tables != null && tables.length > 0) {

            //直接入力ﾊﾟﾀｰﾝで削除対象行の場合
            //　すべての項目の必須ﾁｪｯｸを行わない。
            //直接入力ﾊﾟﾀｰﾝで新規行の場合
            //　入力項目がすべて未入力の場合は必須入力ｴﾗｰﾁｪｯｸを行わない。

            //ｴﾗｰ有無を再検証
            var valid_w = true;     //ｴﾗｰなし

            $.each($(tables), function (ii, tbl) {

                if ($(tbl).hasClass("vertical_tbl")) {
                    //※縦方向一覧の場合

                    //一覧内にｴﾗｰコントロールを保持していれば、ｴﾗｰ
                    var errors = $(tbl).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                    //var errors = $(tbl).find('.errorcom');    //ｴﾗｰ入力要素
                    if (errors != null && errors.length > 0) {
                        valid_w = false;    //ｴﾗｰあり
                    }
                    errors = null;
                }
                else {
                    //※横方向一覧の場合

                    //一覧の入力行を取得
                    var trs = getTrsData(tbl);
                    $.each(trs, function (jj, tr) {
                        //※行単位に検証

                        var errors = $(tr).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                        //var errors = $(tr).find('.errorcom');    //ｴﾗｰ入力要素
                        if (errors != null && errors.length > 0) {
                            //※ｴﾗｰありの場合

                            //ｴﾗｰ状態をﾁｪｯｸ

                            //削除行か？
                            var checkedDels = $(tr).find(":checkbox[data-type='ckdel']:checked");     //削除ﾁｪｯｸﾎﾞｯｸｽ:ON
                            if (level == 1 ||
                                (checkedDels != null && checkedDels.length > 0)) {
                                //※行追加時等の必須ﾁｪｯｸを行わないｱｸｼｮﾝの場合
                                //※削除行の場合

                                //すべての入力項目についての必須ﾁｪｯｸを行わない。
                                $.each(errors, function (kk, error) {
                                    //必須ﾁｪｯｸを行わない
                                    var value = $(error).val();
                                    if (value == null || value.length <= 0) {
                                        //255文字以下の場合、ｴﾗｰを解除
                                        relieveItemError(error);
                                    }
                                });

                                //ｴﾗｰ状態を再ﾁｪｯｸ
                                var errorsAf = $(tr).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                                //var errorsAf = $(tr).find('.errorcom');    //ｴﾗｰ入力要素
                                if (errorsAf != null && errorsAf.length > 0) {
                                    valid_w = false;    //ｴﾗｰあり
                                }
                                errorsAf = null;
                            }
                            else {
                                valid_w = false; // エラーあり
                            }
                            checkedDels = null;
                        }

                    });     //一覧の入力行数分ループ
                    trs = null;
                }
            });     //一覧(table)数分ループ

            //ｴﾗｰ再検証結果で上書き
            valid = valid_w;
        }
        tables = null;
    }
    formDetail = null;

    // ｴﾗｰがない一覧の折り畳み状態を元に戻す
    $.each(switchids, function (idx, switchid) {
        var id = $(switchid).data("switchid");
        if (!($("#" + id).find(".errorcom").length)) {
            if (isHiddens[id]) {
                //表示/非表示切替
                setHideId(id, isHiddens[id]);
            }
        }
    });
    switchids = null;

    //ﾀﾌﾞの表示状態をもとに戻す/もしくは、ｴﾗｰﾀﾌﾞを表示
    if (tabContents != null && tabContents.length > 0) {
        if (!valid) {
            //ｴﾗｰﾀﾌﾞの検索(※選択中のﾀﾌﾞが優先)
            var error_tabNo = null;
            $.each($(tabContents), function (i, div) {
                //ｴﾗｰ存在ﾁｪｯｸ
                var errors = $(div).find(":text.errorcom, select.errorcom");
                if (errors != null && errors.length > 0) {
                    //ｴﾗｰが存在する場合
                    //表示状態ﾁｪｯｸ
                    if ($(div).data('tabno') == tabNo) {
                        //選択ﾀﾌﾞ、かつｴﾗｰありの場合、ｴﾗｰﾀﾌﾞ番号を上書き
                        error_tabNo = tabNo;
                    }
                    else {
                        //ｴﾗｰがある先頭ﾀﾌﾞを表示
                        if (error_tabNo == null) {
                            error_tabNo = $(div).data('tabno');
                        }
                    }
                }
                errors = null;

            });
            if (error_tabNo != null) {
                //選択ﾀﾌﾞをｴﾗｰﾀﾌﾞに変更
                tabNo = error_tabNo;
            }
        }

        //選択ﾀﾌﾞﾎﾞﾀﾝをｸﾘｯｸ
        var tabBtns = $(P_Article).find(".tab_btn.detail a");
        $.each($(tabBtns), function (i, btn) {
            //表示状態ﾁｪｯｸ
            if ($(btn).data('tabno') == tabNo) {
                $(btn).click();
            }
        });
        tabBtns = null;
    }

    if (!valid) {
        //個別エラー状態を初期化
        clearErrorStatus3("form[id^='formDetail']");
        //【CJ00000.W01】入力エラーがあります。
        addMessage(P_ComMsgTranslated[941220007], 1)
        $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
        $(P_Article).find(".detail_div").focus();
        return false;
    }
    tabContents = null;

    return true;    //入力ﾁｪｯｸOK
}

/**
 *  トップエリアの入力検証を行う
 *
 *  @param {int} ：入力ﾁｪｯｸﾚﾍﾞﾙ（1:必須ﾁｪｯｸを行わない）
 *
 *  @return {bool} : ture(OK) false(NG)
 */
function validFormTopData(level) {

    if (level == null) level = 0;

    // 共通入力チェック - Validatorの実行
    var formTop = $(P_Article).find("form[id^='formTop']");

    if (formTop.length > 0) {
        // validationのため、畳んだ一覧は展開する
        var switchids = $(formTop).find("a[data-switchid]");
        var isHiddens = {};
        $.each(switchids, function (idx, switchid) {
            var id = $(switchid).data("switchid");
            var isHidden = isHideId(id);
            isHiddens[id] = isHidden;
            if (isHidden) {
                //表示/非表示切替
                setHideId(id, !isHidden);
            }
        });
        var valid = $(formTop).valid();
        if (!valid) {
            // 個別ｴﾗｰは最初は非表示
            //var errorHtml = $(formTop).find("label.errorcom");
            var errorHtml = $("div.errtooltip").find("label.errorcom");
            $(errorHtml).hide();
            errorHtml = null;

            //複数選択ﾘｽﾄのｴﾗｰを<ul>ﾀｸﾞで拾う
            setMultiSelectError(formTop);

            //直接入力ﾊﾟﾀｰﾝの一覧について、行単位に検証
            //直接入力ﾊﾟﾀｰﾝの一覧要素
            var tables = $(formTop).find("[data-editkbn='" + editPtnDef.Input + "']");
            if (tables != null && tables.length > 0) {

                //直接入力ﾊﾟﾀｰﾝで削除対象行の場合
                //　すべての項目の必須ﾁｪｯｸを行わない。
                //直接入力ﾊﾟﾀｰﾝで新規行の場合
                //　入力項目がすべて未入力の場合は必須入力ｴﾗｰﾁｪｯｸを行わない。

                //ｴﾗｰ有無を再検証
                var valid_w = true;     //ｴﾗｰなし

                $.each($(tables), function (ii, tbl) {

                    if ($(tbl).hasClass("vertical_tbl")) {
                        //※縦方向一覧の場合

                        //一覧内にｴﾗｰコントロールを保持していれば、ｴﾗｰ
                        var errors = $(tbl).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                        //var errors = $(tbl).find('.errorcom');    //ｴﾗｰ入力要素
                        if (errors != null && errors.length > 0) {
                            valid_w = false;    //ｴﾗｰあり
                        }
                        errors = null;
                    }
                    else {
                        //※横方向一覧の場合

                        //一覧の入力行を取得
                        var trs = getTrsData(tbl);
                        $.each(trs, function (jj, tr) {
                            //※行単位に検証

                            var errors = $(tr).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                            //var errors = $(tr).find('.errorcom');    //ｴﾗｰ入力要素
                            if (errors != null && errors.length > 0) {
                                //※ｴﾗｰありの場合

                                //ｴﾗｰ状態をﾁｪｯｸ

                                //削除行か？
                                var checkedDels = $(tr).find(":checkbox[data-type='ckdel']:checked");     //削除ﾁｪｯｸﾎﾞｯｸｽ:ON
                                if (level == 1 ||
                                    (checkedDels != null && checkedDels.length > 0)) {
                                    //※行追加時等の必須ﾁｪｯｸを行わないｱｸｼｮﾝの場合
                                    //※削除行の場合

                                    //すべての入力項目についての必須ﾁｪｯｸを行わない。
                                    $.each(errors, function (kk, error) {
                                        //必須ﾁｪｯｸを行わない
                                        var value = $(error).val();
                                        if (value == null || value.length <= 0) {
                                            //255文字以下の場合、ｴﾗｰを解除
                                            relieveItemError(error);
                                        }
                                    });

                                    //ｴﾗｰ状態を再ﾁｪｯｸ
                                    var errorsAf = $(tr).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                                    //var errorsAf = $(tr).find('.errorcom');    //ｴﾗｰ入力要素
                                    if (errorsAf != null && errorsAf.length > 0) {
                                        valid_w = false;    //ｴﾗｰあり
                                    }
                                    errorsAf = null;
                                }
                                else {
                                    valid_w = false; // エラーあり
                                }
                                checkedDels = null;
                            }
                            errors = null;

                        });     //一覧の入力行数分ループ
                        trs = null;
                    }
                });     //一覧(table)数分ループ

                //ｴﾗｰ再検証結果で上書き
                valid = valid_w;
            }
            tables = null;
        }
        // ｴﾗｰがない一覧の折り畳み状態を元に戻す
        $.each(switchids, function (idx, switchid) {
            var id = $(switchid).data("switchid");
            if (!($("#" + id).find(".errorcom").length)) {
                if (isHiddens[id]) {
                    //表示/非表示切替
                    setHideId(id, isHiddens[id]);
                }
            }
        });
        switchids = null;

        if (!valid) {
            //個別エラー状態を初期化
            clearErrorStatus3("form[id^='formTop']");
            //【CJ00000.W01】入力エラーがあります。
            addMessage(P_ComMsgTranslated[941220007], 1)
            $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
            $(P_Article).find(".detail_div").focus();
            return false;
        }
    }
    formTop = null;

    return true;    //入力ﾁｪｯｸOK
}

/**
 * 単票ｴﾘｱのﾊﾞﾘﾃﾞｰｼｮﾝ
 * @param {int} ：入力ﾁｪｯｸﾚﾍﾞﾙ（1:必須ﾁｪｯｸを行わない）
 * @return {bool} : ture(OK) false(NG)
 */
function validFormEditData(level) {

    if (level == null) level = 0;

    // 共通入力チェック - Validatorの実行
    var formEdit = $(P_Article).find("form[id^='formEdit']");

    if (formEdit.length > 0) {
        var valid = $(formEdit).valid();
        if (!valid) {
            // 個別ｴﾗｰは最初は非表示
            //var errorHtml = $(formEdit).find("label.errorcom");
            var errorHtml = $("div.errtooltip").find("label.errorcom");
            $(errorHtml).hide();
            errorHtml = null;

            //複数選択ﾘｽﾄのｴﾗｰを<ul>ﾀｸﾞで拾う
            setMultiSelectError(formEdit);

            //直接入力ﾊﾟﾀｰﾝの一覧について、行単位に検証
            //直接入力ﾊﾟﾀｰﾝの一覧要素
            var tables = $(formEdit).find("[data-editkbn='" + editPtnDef.Input + "']");
            if (tables != null && tables.length > 0) {

                //直接入力ﾊﾟﾀｰﾝで削除対象行の場合
                //　すべての項目の必須ﾁｪｯｸを行わない。
                //直接入力ﾊﾟﾀｰﾝで新規行の場合
                //　入力項目がすべて未入力の場合は必須入力ｴﾗｰﾁｪｯｸを行わない。

                //ｴﾗｰ有無を再検証
                var valid_w = true;     //ｴﾗｰなし

                $.each($(tables), function (ii, tbl) {

                    if ($(tbl).hasClass("vertical_tbl")) {
                        //※縦方向一覧の場合

                        //一覧内にｴﾗｰコントロールを保持していれば、ｴﾗｰ
                        var errors = $(tbl).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                        //var errors = $(tbl).find('.errorcom');    //ｴﾗｰ入力要素
                        if (errors != null && errors.length > 0) {
                            valid_w = false;    //ｴﾗｰあり
                        }
                        errors = null;
                    }
                });     //一覧(table)数分ループ

                //ｴﾗｰ再検証結果で上書き
                valid = valid_w;
            }
            tables = null;
        }

        if (!valid) {
            //個別エラー状態を初期化
            clearErrorStatus3("form[id^='formEdit']");
            //【CJ00000.W01】入力エラーがあります。
            addMessage(P_ComMsgTranslated[941220007], 1)
            $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
            $(P_Article).find(".edit_div").focus();
            return false;
        }
    }
    formEdit = null;

    return true;    //入力ﾁｪｯｸOK
}

/**
 *  ボトムエリアの入力検証を行う
 *
 *  @param {int} ：入力ﾁｪｯｸﾚﾍﾞﾙ（1:必須ﾁｪｯｸを行わない）
 *
 *  @return {bool} : ture(OK) false(NG)
 */
function validFormBottomData(level) {

    if (level == null) level = 0;

    // 共通入力チェック - Validatorの実行
    var formBottom = $(P_Article).find("form[id^='formBottom']");

    if (formBottom.length > 0) {
        // validationのため、畳んだ一覧は展開する
        var switchids = $(formBottom).find("a[data-switchid]");
        var isHiddens = {};
        $.each(switchids, function (idx, switchid) {
            var id = $(switchid).data("switchid");
            var isHidden = isHideId(id);
            isHiddens[id] = isHidden;
            if (isHidden) {
                //表示/非表示切替
                setHideId(id, !isHidden);
            }
        });
        var valid = $(formBottom).valid();
        if (!valid) {
            // 個別ｴﾗｰは最初は非表示
            //var errorHtml = $(formBottom).find("label.errorcom");
            var errorHtml = $("div.errtooltip").find("label.errorcom");
            $(errorHtml).hide();
            errorHtml = null;

            //複数選択ﾘｽﾄのｴﾗｰを<ul>ﾀｸﾞで拾う
            setMultiSelectError(formBottom);

            //直接入力ﾊﾟﾀｰﾝの一覧について、行単位に検証
            //直接入力ﾊﾟﾀｰﾝの一覧要素
            var tables = $(formBottom).find("[data-editkbn='" + editPtnDef.Input + "']");
            if (tables != null && tables.length > 0) {

                //直接入力ﾊﾟﾀｰﾝで削除対象行の場合
                //　すべての項目の必須ﾁｪｯｸを行わない。
                //直接入力ﾊﾟﾀｰﾝで新規行の場合
                //　入力項目がすべて未入力の場合は必須入力ｴﾗｰﾁｪｯｸを行わない。

                //ｴﾗｰ有無を再検証
                var valid_w = true;     //ｴﾗｰなし

                $.each($(tables), function (ii, tbl) {

                    if ($(tbl).hasClass("vertical_tbl")) {
                        //※縦方向一覧の場合

                        //一覧内にｴﾗｰコントロールを保持していれば、ｴﾗｰ
                        var errors = $(tbl).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                        //var errors = $(tbl).find('.errorcom');    //ｴﾗｰ入力要素
                        if (errors != null && errors.length > 0) {
                            valid_w = false;    //ｴﾗｰあり
                        }
                        errors = null;
                    }
                    else {
                        //※横方向一覧の場合

                        //一覧の入力行を取得
                        var trs = getTrsData(tbl);
                        $.each(trs, function (jj, tr) {
                            //※行単位に検証

                            var errors = $(tr).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                            //var errors = $(tr).find('.errorcom');    //ｴﾗｰ入力要素
                            if (errors != null && errors.length > 0) {
                                //※ｴﾗｰありの場合

                                //ｴﾗｰ状態をﾁｪｯｸ

                                //削除行か？
                                var checkedDels = $(tr).find(":checkbox[data-type='ckdel']:checked");     //削除ﾁｪｯｸﾎﾞｯｸｽ:ON
                                if (level == 1 ||
                                    (checkedDels != null && checkedDels.length > 0)) {
                                    //※行追加時等の必須ﾁｪｯｸを行わないｱｸｼｮﾝの場合
                                    //※削除行の場合

                                    //すべての入力項目についての必須ﾁｪｯｸを行わない。
                                    $.each(errors, function (kk, error) {
                                        //必須ﾁｪｯｸを行わない
                                        var value = $(error).val();
                                        if (value == null || value.length <= 0) {
                                            //255文字以下の場合、ｴﾗｰを解除
                                            relieveItemError(error);
                                        }
                                    });

                                    //ｴﾗｰ状態を再ﾁｪｯｸ
                                    var errorsAf = $(tr).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                                    //var errorsAf = $(tr).find('.errorcom');    //ｴﾗｰ入力要素
                                    if (errorsAf != null && errorsAf.length > 0) {
                                        valid_w = false;    //ｴﾗｰあり
                                    }
                                    errorsAf = null;

                                }
                                else {
                                    valid_w = false; // エラーあり
                                }
                                checkedDels = null;
                            }
                            errors = null;

                        });     //一覧の入力行数分ループ
                        trs = null;
                    }
                });     //一覧(table)数分ループ

                //ｴﾗｰ再検証結果で上書き
                valid = valid_w;
            }
            tables = null;
        }
        // ｴﾗｰがない一覧の折り畳み状態を元に戻す
        $.each(switchids, function (idx, switchid) {
            var id = $(switchid).data("switchid");
            if (!($("#" + id).find(".errorcom").length)) {
                if (isHiddens[id]) {
                    //表示/非表示切替
                    setHideId(id, isHiddens[id]);
                }
            }
        });
        switchids = null;

        if (!valid) {
            //個別エラー状態を初期化
            clearErrorStatus3("form[id^='formBottom']");
            //【CJ00000.W01】入力エラーがあります。
            addMessage(P_ComMsgTranslated[941220007], 1)
            $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
            $(P_Article).find(".detail_div").focus();
            return false;
        }
    }
    formBottom = null;

    return true;    //入力ﾁｪｯｸOK
}

/**
 * 詳細検索条件ｴﾘｱのﾊﾞﾘﾃﾞｰｼｮﾝ
 * @param {Element} form    ：対象フォーム要素
 * @return {boolean} : ture(OK) false(NG)
 */
function validFormDetailSearchData(form) {

    // 共通入力チェック - Validatorの実行
    if (form.length > 0) {
        var valid = $(form).valid();
        if (!valid) {
            // 個別ｴﾗｰは最初は非表示
            //var errorHtml = $(formEdit).find("label.errorcom");
            var errorHtml = $("div.errtooltip").find("label.errorcom");
            $(errorHtml).hide();
            errorHtml = null;

            //一覧について、行単位に検証
            var tables = $(form).find("table").has("input");
            if (tables != null && tables.length > 0) {

                //ｴﾗｰ有無を再検証
                var valid_w = true;     //ｴﾗｰなし

                $.each($(tables), function (ii, tbl) {

                    if ($(tbl).hasClass("detailsearch")) {
                        //※詳細検索条件一覧の場合

                        //一覧内にｴﾗｰコントロールを保持していれば、ｴﾗｰ
                        var errors = $(tbl).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                        //var errors = $(tbl).find('.errorcom');    //ｴﾗｰ入力要素
                        if (errors != null && errors.length > 0) {
                            valid_w = false;    //ｴﾗｰあり
                        }
                        errors = null;
                    }
                });     //一覧(table)数分ループ

                //ｴﾗｰ再検証結果で上書き
                valid = valid_w;
            }
            tables = null;
        }

        if (!valid) {
            //個別エラー状態を初期化
            clearErrorStatus3(form);
            //【CJ00000.W01】入力エラーがあります。
            addMessage(P_ComMsgTranslated[941220007], messageType.Warning, $(form).closest('div.main_div'));
            $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
            $('nav#detail_search .main_div').focus();
            return false;
        }
    }
    return true;    //入力ﾁｪｯｸOK
}

/**
 *  ｴﾗｰを項目単位で解除
 *  @element {ｴﾗｰ要素} : 
 */
function relieveItemError(element) {

    $(element).removeClass("errorcom");
    $(element).closest("td").removeClass("errorcom");
    var elId = $(element).attr("id");
    $("label[for='" + elId + "']").remove();
}

/**
 *  複数選択ﾘｽﾄのｴﾗｰを<ul>ﾀｸﾞで拾う
 *
 *  @return {bool} : ture(OK) false(NG)
 */
function setMultiSelectError(form) {

    var msuls = $(form).find('ul.multiSelect');
    if (msuls != null && msuls.length > 0) {
        $.each(msuls, function () {
            //ｴﾗｰのﾁｪｯｸﾎﾞｯｸｽが存在するか？
            var errorchecks = $(this).find(':checkbox.errorcom');
            if (errorchecks != null && errorchecks.length > 0) {
                $(this).addClass('errorcom');
            }
        });
    }
    msuls = null;

}

/**
 * 対象列変更時、変更フラグ「1」を対象列に設定する
 *
 *  @ctrlId 　　{string} ：一覧CtrlId
 *  @cangeColNo {int}    ：変更対象の入力列番号
 *  @setColNo 　{int}    ：変更フラグ設定列番号
 */
function setCellChangeFlg(ctrlId, cangeColNo, setColNo) {

    var trs = getTrsData("#" + ctrlId);   //一覧データ行
    //変更列
    $(trs).find("td[data-name='VAL" + cangeColNo + "'] :text").each(function (index, element) {

        //変更時時の入力制御を設定
        $(element).on('change', function () {
            //対象行
            var tr = $(this).parent().parent();
            //変更フラグ設定セル
            var td = $(tr).find("td[data-name='VAL" + setColNo + "']");
            $(td).text("1");
            td = null;
            tr = null;
        });

    });

    $(trs).find("td[data-name='VAL" + cangeColNo + "'] :checkbox").each(function (index, element) {

        //変更時時の入力制御を設定
        $(element).on('change', function () {
            //対象行
            var tr = $(this).parent().parent();
            //変更フラグ設定セル
            var td = $(tr).find("td[data-name='VAL" + setColNo + "']");
            $(td).text("1");
            td = null;
            tr = null;
       });

    });

    $(trs).find("td[data-name='VAL" + cangeColNo + "'] select").each(function (index, element) {

        //変更時時の入力制御を設定
        $(element).on('change', function () {

            if ($(this).data("create") != "1") {
                //※コンボ生成中のｲﾍﾞﾝﾄ以外でない場合

                //対象行
                var tr = $(this).parent().parent();
                //変更フラグ設定セル
                var td = $(tr).find("td[data-name='VAL" + setColNo + "']");
                $(td).text("1");
                td = null;
                tr = null;
            }
        });

    });
    trs = null;
}

/**
 * 一覧のNOリンクの幅を全件の件数に応じて調節する
 *
 *  @tbl 　　   {table} ：一覧<table>要素
 *  @totalCount {int}   ：一覧の全件件数
 */
function adjustNoLink(tbl, totalCount) {

    var noKeta = (totalCount + '').length;      //全件件数の桁数
    if (noKeta >= 3) {
        //3桁以上の場合、桁数に応じた調整を行う

        //NOリンクの幅指定用の要素を取得
        var hWidthRowNoTd = $(tbl).find("thead tr.base_tr_width td[data-name='ROWNO']");
        if (hWidthRowNoTd != null && hWidthRowNoTd.length > 0) {
            var width = 30 + (4 * noKeta);  //デフォルト幅：30 + 桁数に応じた調整（4 * [桁数]）

            //幅を書き換え
            setAttrByNativeJs(hWidthRowNoTd, "width", width);
        }
    }

}

/**
 *  一覧ｺﾝﾎﾞﾎﾞｯｸｽの翻訳情報を取得
 *
 *  @param {table} ：一覧のtable要素
 */
function getHonyakuTable(tbl) {

    // 一覧ｺﾝﾎﾞﾎﾞｯｸｽの翻訳情報取得
    var listHonyaku = [];

    var tblId = getTableId(tbl);   //一覧CtrlId
    var layoutTr = getBaseTr(tbl);      //ﾚｲｱｳﾄ行要素

    //ﾚｲｱｳﾄ行からｺﾝﾎﾞﾎﾞｯｸｽの列を取得
    var layoutTds = $(layoutTr).find("td[data-name^='VAL']");
    var layoutSelects = $(layoutTds).find('> select');

    //if (layoutSelects.length > 0) {

    //    //ｺﾝﾎﾞﾎﾞｯｸｽの翻訳情報を取得
    //    $.each($(layoutSelects), function (idx, select) {

    //        // 一覧ｺﾝﾎﾞﾎﾞｯｸｽの基本情報を設定
    //        var key = $(select).parent().data("name");  //VAL1～
    //        key = key.remove("VAL");

    //        // 一覧ｺﾝﾎﾞﾎﾞｯｸｽの翻訳情報を取得

    //        //選択ﾘｽﾄ要素を取得
    //        var options = $(select).find("option");
    //        if (options.length > 0) {
    //            $.each(options, function (i, option) {

    //                //選択ﾘｽﾄの値(表示値、入力値)を取得
    //                var dataHonyaku = {
    //                    CTRLID: tblId,     // 一覧の画面定義のコントロールID
    //                    COLNO: key,         // VAL1～
    //                    CODE: $(options).val(),   //コード値
    //                    VALUE: $(options).text(),  //表示値
    //                };
    //                listHonyaku.push(dataHonyaku);

    //            });
    //        }
    //        else
    //        {
    //            //選択ﾘｽﾄの値(表示値、入力値)を取得
    //            var dataHonyaku = {
    //                CTRLID: tblId, // 一覧の画面定義のコントロールID
    //                COLNO: key,     // VAL1～
    //                CODE: "",       //コード値
    //                VALUE: "",      //表示値
    //            };
    //            listHonyaku.push(dataHonyaku);
    //        }

    //    });
    //}

    if (layoutSelects.length > 0) {

        //ｺﾝﾎﾞﾎﾞｯｸｽの翻訳情報を取得
        $.each($(layoutSelects), function (idx, select) {

            // 一覧ｺﾝﾎﾞﾎﾞｯｸｽの基本情報を設定
            var key = $(select).parent().data("name");  //VAL1～
            key = key.replace('VAL', '');

            var itemList = [];
            //選択ﾘｽﾄ要素を取得
            var options = $(select).find("option");
            $.each(options, function (i, option) {

                //選択ﾘｽﾄの値(表示値、入力値)を取得
                var itemData = {
                    VALUE1: $(option).val(),   //コード値
                    VALUE2: $(option).text(),  //表示値
                };
                itemList.push(itemData);

            });
            options = null;

            // 一覧ｺﾝﾎﾞﾎﾞｯｸｽの翻訳情報を取得
            var dataHonyaku = {
                CTRLID: tblId,         // 一覧の画面定義のコントロールID
                COLNO: key,             // VAL1～
                ITEMLIST: itemList,     //コード値
            };
            listHonyaku.push(dataHonyaku);

        });
    }
    else {
        //ｺﾝﾎﾞﾎﾞｯｸｽが存在しない場合は、一覧CTRLID情報のみを１件設定
        var dataHonyaku = {
            CTRLID: tblId,     // 一覧の画面定義のコントロールID
        };

        listHonyaku.push(dataHonyaku);
    }
    layoutSelects = null;
    layoutTds = null;

    return listHonyaku;
}

/**
 *  数値ｺﾝﾄﾛｰﾙの単位をｾｯﾄ
 *
 *  @selecter {要素} ：数値ｺﾝﾄﾛｰﾙ要素
 *  @unit {string}   ：単位
 */
function setCellNumUnit(selecter, unit) {

    $(selecter).parent().find('.unit').text(unit);

    //単位がある場合のみ数値ｺﾝﾄﾛｰﾙに「unitnum」ｸﾗｽ付与
    if (unit.length > 0) {
        if (!$(selecter).hasClass('unitnum')) {
            $(selecter).addClass('unitnum');
        }
    } else {
        if ($(selecter).hasClass('unitnum')) {
            $(selecter).removeClass('unitnum');
        }
    }

}

/**
 *  TDにTEXTにVAL値をｾｯﾄ
 *
 *  @td {要素}      ：TD要素
 *  @value {string} ：値
 */
function setTdText(td, value) {

    var valStr = value;

    //数値の場合
    var unit = ""
    if ($(td).data('num') == '1') {
        var values = (value + '').split('@')
        if (values.length > 1) {
            unit = ' ' + values[1];
        }
        valStr = values[0];
    }
    //FromToの場合
    if ($(td).data('fromto') == '1') {
        var values = valStr.split('|')
        valStr = values[0] + unit + ' ' + P_ComMsgTranslated[911060006] + ' ';
        if (values.length > 1) {
            valStr = valStr + values[1];
        }
        valStr = valStr + unit;
    } else {
        valStr = valStr + unit;
    }

    $(td).text(valStr);

}

/**
 *  TDのTEXTからVAL値を取得
 *
 *  @td {要素}      ：TD要素
 */
function getTdText(td) {

    var val1 = $(td).text();
    var val2 = "";
    var unit = '';

    if ($(td).data('fromto') == '1') {
        var texts = $(td).text().split(' ' + P_ComMsgTranslated[911060006] + ' ');
        if ($(td).data('num') == '1') {
            var units = texts[0].split(' ');
            val1 = units[0];
            unit = '@' + units[1];
        } else {
            val1 = texts[0];
        }
        if (texts.length > 1) {
            if ($(td).data('num') == '1') {
                var units = texts[1].split(' ');
                val2 = units[0];
            } else {
                val2 = texts[1];
            }
        }
        valStr = val1 + '|' + val2 + unit;

    } else {
        if ($(td).data('num') == '1') {
            var units = $(td).text().split(' ');
            val1 = units[0];
            if (units.length > 1) {
                unit = '@' + units[1];
            }

        }
        valStr = val1 + unit;
    }
    return valStr;
}

/**
 *  複数選択ﾘｽﾄからﾁｪｯｸONのﾃｷｽﾄをｶﾝﾏ連結してｾｯﾄ
 *
 *  @td {要素}      ：TD要素
 */
function setMutiSelectCheckOnText(td) {
    var textW = "";
    var text = $(td).find(".multisel-text");

    if (text != null && text.length > 0) {
        //「全て」にﾁｪｯｸが入っている場合
        var checkes = $(td).find("li:not(.hide) :checkbox.ctrloption:checked");
        if (checkes != null && checkes.length > 0) {
            //textW = "全て"
            textW = P_ComMsgTranslated[911130001]; //"すべて"
        }
        else {
            //それ以外の場合、ﾁｪｯｸ:onの表示名

            //参照モードかどうか取得
            var referenceFlg = $(td).closest("div.vertical_tbl.ctrlId").filter("[data-referencemode='" + referenceModeKbnDef.Reference + "']");
            if (referenceFlg && referenceFlg.length > 0) {
                //参照モードの場合、削除アイテムも取得する
                checkes = $(td).find("li:not(.hide) :checkbox:checked");
            } else {
                //編集モードの場合、削除アイテムは取得しない
                checkes = $(td).find("li>ul>li:not(.hide):not(.deleteItem) :checkbox:checked");
            }
            if (checkes != null && checkes.length > 0) {
                var texts = [];
                $.each(checkes, function () {
                    texts.push($(this).parent().find("span").text());
                });
                textW = texts.join(',');
            }
        }
        text[0].innerHTML = textW;
        checkes = null;
    }
    text = null;

    return textW;
}

/**
 * 詳細検索条件適用状況の設定
 * @param {string} selector ：一覧テーブル要素のセレクタ
 * @param {object} data     ：検索結果データ
 */
function setConditionAppliedStatus(selector, data) {
    // 詳細検索メニューアイコンを取得
    var detailIcon = $(selector).closest('div.ctrlId_parent').siblings('div.tbl_title').find('a[data-actionkbn="' + actionkbn.ComDetailSearch + '"]');
    if (detailIcon != null && detailIcon.length > 0) {
        // 詳細検索アイコン表示時
        if (data.IsDetailConditionApplied) {
            // 詳細検索条件適用時
            $(detailIcon).addClass('condition_applied')
        } else {
            $(detailIcon).removeClass('condition_applied');
        }
    }
    detailIcon = null;
}

/**
 * 入力欄の変更値をtabulatorのデータに保存する
 * @param {any} table tabulator
 * @param {any} id 一覧のID
 * @param {any} formNo 画面NO
 */
function updateTabulatorDataForChangeVal(table, id, formNo) {
    //tabulator一覧のページ切り替えやソート等が行われた際に、変更された値を保持できるよう対象行のデータを更新する

    var data = table.getData("display");
    $.each(data, function (idx, rowData) {
        var rowNo = rowData.ROWNO;
        //対象行のデータを取得
        var tmpData = getTempDataForTabulator(formNo, rowNo, id, 0, true);
        //編集行の場合、値を更新
        if (tmpData == null) { return true; }
        if (tmpData.UPDTAG == updtag.Input) {
            var updateRow = table.searchRows("ROWNO", "=", rowNo);
            // 変更した値を保存する
            updateRow[0].update(tmpData);
        }
    });
}