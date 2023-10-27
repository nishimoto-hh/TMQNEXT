/* ========================================================================
 *  機能名　    ：   【共通・画面共通ﾍﾟｰｼﾞｬｰ】
 * ======================================================================== */

/**
 *  ﾃｰﾌﾞﾙ(一覧)ごとのﾍﾟｰｼﾞｬｰ初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {string} ：対象ﾍﾟｰｼﾞｬｰ(※ﾃﾞﾌｫﾙﾄ表示ﾍﾟｰｼﾞｬｰのみ)
 */
function initPaginationCommon(appPath, conductId, pgmId, formNo, pagination) {

    var pageCount = $(pagination).data("pagerows");     //1ﾍﾟｰｼﾞあたりのﾃﾞｰﾀ件数
    var ctrlId = $(pagination).data('ctrlid');          //ﾃｰﾌﾞﾙ(一覧)CtrlId

    //該当ﾃｰﾌﾞﾙのﾍﾟｰｼﾞｬｰを取得
    var paginations = $('.paginationCommon[data-ctrlid="' + ctrlId + '"]');

    // ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙ - 「<」,「>」ﾘﾝｸ　ｸﾘｯｸｲﾍﾞﾝﾄ処理を付与
    $(paginations).find('li > a').on('click', function () {
        //formNo取得
        var formNoW = $(P_Article).data("formno");

        //明細エリアのエラー状態を初期化
        var element = $(P_Article).find("form[id^='formDetail']");
        clearErrorStatus(element);
        element = null;

        var current = $(this);
        var ctrltype = $(paginations).data('ctrltype');
        if (dataEditedFlg && ctrltype != ctrlTypeDef.IchiranPtn2 && ctrltype != ctrlTypeDef.IchiranPtn3) {
            // 画面変更ﾌﾗｸﾞONの場合、ﾃﾞｰﾀ破棄確認ﾒｯｾｰｼﾞ

            //確認ﾒｯｾｰｼﾞを設定
            P_MessageStr = P_ComMsgTranslated[941060005]; //『画面の編集内容は破棄されます。よろしいですか？』
            //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
            var eventFunc = function () {

                var pageNo = 1;
                pageNo = getCurrentPageNo(ctrlId, "def");                 //選択行番号

                if ($(current).attr("name") == "selectNext") {
                    pageNo = pageNo + 1;
                }
                else if ($(current).attr("name") == "selectPrv") {
                    pageNo = pageNo - 1;
                }
                else if ($(current).attr("name") == "selectLast") {
                    pageNo = getPageCountAll(ctrlId);
                } else {
                    pageNo = 1;
                }
                
                //ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙの状態ｾｯﾄ
                setPaginationStatus(ctrlId, pageNo);

                // ページデータの取得
                getPageData1(appPath, conductId, pgmId, formNoW, pageNo, pageCount, ctrlId);
            }
            // 確認ﾒｯｾｰｼﾞを表示
            if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
                //『キャンセル』の場合、処理中断
                return false;
            }
        }
        else {

            var pageNo = 1;
            pageNo = getCurrentPageNo(ctrlId, "def");                 //選択行番号

            if ($(current).attr("name") == "selectNext") {
                pageNo = pageNo + 1;
            }
            else if ($(current).attr("name") == "selectPrv") {
                pageNo = pageNo - 1;
            }
            else if ($(current).attr("name") == "selectLast") {
                pageNo = getPageCountAll(ctrlId);
            } else {
                pageNo = 1;
            }
            //ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙの状態ｾｯﾄ
            setPaginationStatus(ctrlId, pageNo);

            if (ctrltype == ctrlTypeDef.IchiranPtn3) {
                var id = '#' + ctrlId + '_' + formNoW;
                var table = P_listData[id];
                // 一覧編集パターン
                var editptn = $(id).data('editptn');
                // 表示モード
                var referenceMode = $(id).data('referencemode');
                if (editptn != editPtnDef.ReadOnly || referenceMode != referenceModeKbnDef.Reference) {
                    //入力欄の変更を保存
                    updateTabulatorDataForChangeVal(table, id, formNoW);
                }
                table.setPage(pageNo);
                table = null;
                return;
            }

            // ページデータの取得
            getPageData1(appPath, conductId, pgmId, formNoW, pageNo, pageCount, ctrlId);
        }
    });

    // ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙ - 「行番号選択」ｺﾝﾎﾞﾎﾞｯｸｽ 選択時ｲﾍﾞﾝﾄ処理を付与
    $(paginations).find('li > span > select').on('change', function () {
        //formNo取得
        var formNoW = $(P_Article).data("formno");

        // メッセージをクリア
        clearMessage();
        //明細エリアのエラー状態を初期化
        var element = $(P_Article).find("form[id^='formDetail']");
        clearErrorStatus(element);
        element = null;

        var select = $(this).val();
        var current = $(this);

        var ctrltype = $(paginations).data('ctrltype');
        if (dataEditedFlg && ctrltype != ctrlTypeDef.IchiranPtn2 && ctrltype != ctrlTypeDef.IchiranPtn3) {
            // 画面変更ﾌﾗｸﾞONの場合、ﾃﾞｰﾀ破棄確認ﾒｯｾｰｼﾞ

            //確認ﾒｯｾｰｼﾞを設定
            P_MessageStr = P_ComMsgTranslated[941060005]; //『画面の編集内容は破棄されます。よろしいですか？』
            //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
            var eventFunc = function () {

                var pageNo = parseInt(select, 10);     //選択行番号
                if (pageNo == null) {
                    pageNo = 1;
                }
                //ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙの状態ｾｯﾄ
                setPaginationStatus(ctrlId, pageNo);

                // ページデータの取得
                getPageData1(appPath, conductId, pgmId, formNoW, pageNo, pageCount, ctrlId);
            }
            // 確認ﾒｯｾｰｼﾞを表示
            if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
                //『キャンセル』の場合、処理中断
                //return false;
                //ﾍﾟｰｼﾞ番号をもとに戻す
                var pageNo = 1;
                pageNo = getCurrentPageNo(ctrlId, "def");   //選択行番号(※ｺﾝﾎﾞ変更前)
                $(current).val(pageNo);

                return false;
            }
        }
        else {

            var pageNo = parseInt($(current).val(), 10);     //選択行番号
            if (pageNo == null) {
                pageNo = 1;
            }
            //ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙの状態ｾｯﾄ
            setPaginationStatus(ctrlId, pageNo);

            // ページデータの取得
            getPageData1(appPath, conductId, pgmId, formNoW, pageNo, pageCount, ctrlId);
        }
    });

    //ﾍﾟｰｼﾞｬｰの状態設定
    setPaginationStatus(ctrlId, 1);
}

/**
 *  ﾍﾟｰｼﾞ選択ｺﾝﾎﾞ初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {string} ：対象ﾍﾟｰｼﾞｬｰ(※ﾃﾞﾌｫﾙﾄ表示ﾍﾟｰｼﾞｬｰのみ)
 */
function initPaginationCommonSelect(appPath, conductId, pgmId, formNo, pagination) {

    var pageCount = $(pagination).data("pagerows");     //1ﾍﾟｰｼﾞあたりのﾃﾞｰﾀ件数
    var ctrlId = $(pagination).data('ctrlid');          //ﾃｰﾌﾞﾙ(一覧)CtrlId

    //該当ﾃｰﾌﾞﾙのﾍﾟｰｼﾞｬｰを取得
    var paginations = $(P_Article).find('.paginationCommon[data-ctrlid="' + ctrlId + '"]');

    // ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙ - 「行番号選択」ｺﾝﾎﾞﾎﾞｯｸｽ 選択時ｲﾍﾞﾝﾄ処理を付与
    $(paginations).find('li > span > select').on('change', function () {
        // メッセージをクリア
        clearMessage();
        //明細エリアのエラー状態を初期化
        var element = $(P_Article).find("form[id^='formDetail']");
        clearErrorStatus(element);

        var select = $(this).val();
        var current = $(this);

        var ctrltype = $(paginations).data('ctrltype');
        if (dataEditedFlg && ctrltype != ctrlTypeDef.IchiranPtn2 && ctrltype != ctrlTypeDef.IchiranPtn3) {
            // 画面変更ﾌﾗｸﾞONの場合、ﾃﾞｰﾀ破棄確認ﾒｯｾｰｼﾞ

            //確認ﾒｯｾｰｼﾞを設定
            P_MessageStr = P_ComMsgTranslated[941060005]; //『画面の編集内容は破棄されます。よろしいですか？』
            //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
            var eventFunc = function () {
                
                var pageNo = parseInt(select, 10);     //選択行番号
                if (pageNo == null) {
                    pageNo = 1;
                }
                //ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙの状態ｾｯﾄ
                setPaginationStatus(ctrlId, pageNo);

                // ページデータの取得
                getPageData1(appPath, conductId, pgmId, formNo, pageNo, pageCount, ctrlId);
            }
            // 確認ﾒｯｾｰｼﾞを表示
            if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
                //『キャンセル』の場合、処理中断
                //return false;
                //ﾍﾟｰｼﾞ番号をもとに戻す
                var pageNo = 1;
                pageNo = getCurrentPageNo(ctrlId, "def");   //選択行番号(※ｺﾝﾎﾞ変更前)
                $(current).val(pageNo);

                return false;
            }
        }
        else {

            var pageNo = $(current).val();     //選択行番号
            if (pageNo == null) {
                pageNo = 1;
            }
            //ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙの状態ｾｯﾄ
            setPaginationStatus(ctrlId, pageNo);

            if (ctrltype == ctrlTypeDef.IchiranPtn3) {
                var id = '#' + ctrlId + '_' + $(P_Article).data("formno");
                var table = P_listData[id];
                // 一覧編集パターン
                var editptn = $(id).data('editptn');
                // 表示モード
                var referenceMode = $(id).data('referencemode');
                if (editptn != editPtnDef.ReadOnly || referenceMode != referenceModeKbnDef.Reference) {
                    //入力欄の変更を保存
                    updateTabulatorDataForChangeVal(table, id, $(P_Article).data("formno"));
                }
                table.setPage(pageNo);
                table = null;
                return;
            }

            // ページデータの取得
            getPageData1(appPath, conductId, pgmId, formNo, pageNo, pageCount, ctrlId);
        }
    });

    //現在ﾍﾟｰｼﾞ番号の取得
    var curPageNo = getCurrentPageNo(ctrlId, 'def');
    var pageCountAll = getPageCountAll(ctrlId);
    if (curPageNo > pageCountAll) {
        //最終ﾍﾟｰｼﾞ番号を超えていたら、最終ﾍﾟｰｼﾞ番号とする
        curPageNo = pageCountAll;
    }

    //ﾍﾟｰｼﾞｬｰの状態設定
    setPaginationStatus(ctrlId, curPageNo);
}

/**
 *  ﾍﾟｰｼﾞｬｰのｾｯﾄｱｯﾌﾟを行う
 *   - ﾍﾟｰｼﾞ選択ｺﾝﾎﾞﾎﾞｯｸｽ
 *   - 最終ﾍﾟｰｼﾞ番号
 *  @tblCtrlId {string} ：ﾃｰﾌﾞﾙ（一覧）ctrlId
 *  @totalCount{int}    ：全ﾃﾞｰﾀ件数
 */
function setupPagination(appPath, conductId, pgmId, formNo, tblCtrlId, totalCount) {

    var pagination = $(P_Article).find('#' + tblCtrlId + "_" + formNo + '_div').find('.paginationCommon');
    if (pagination != null && pagination.length > 0)
    {
        var paginationDef = null;
        $.each(pagination, function () {
            if ($(this).data('option') == "def") {
                paginationDef = this;
            }
        });

        setAttrByNativeJs(pagination, 'data-totalcnt', totalCount); // 総件数

        var pageCount = $(paginationDef).data('pagerows');  //1ﾍﾟｰｼﾞﾃﾞｰﾀ数
        //総ﾍﾟｰｼﾞ数を取得
        var pageCountAll = Math.ceil(totalCount / pageCount);

        //   - ﾍﾟｰｼﾞ選択ｺﾝﾎﾞﾎﾞｯｸｽ 再作成
        var selectPageNo_span = $(pagination).find('span[name="selectPageNo_span"]');
        if (selectPageNo_span != null && selectPageNo_span.length > 0) {
            $(selectPageNo_span).children().remove();

            var select = $('<select name="selectPageNo">').val(1);

            for (var pageNo = 1; pageNo <= pageCountAll; pageNo++) {
                $('<option>').val(pageNo).html(pageNo).appendTo($(select));
            }

            $(select).appendTo($(selectPageNo_span));

        }

        //   - 最終ﾍﾟｰｼﾞ番号
        var span = $(pagination).find('[name="pageCount_span"]');
        if (span != null && span.length > 0) {
            $(span).text(" / " + pageCountAll);
            setAttrByNativeJs(span, 'data-pageno', pageCountAll);
        }

        //ﾍﾟｰｼﾞｬｰ初期化
        initPaginationCommonSelect(appPath, conductId, pgmId, formNo, paginationDef);
    }
    pagination = null;
}

/**
 *  ﾍﾟｰｼﾞｬｰの状態設定を行う
 *   - ﾍﾟｰｼﾞ番号設定
 *   - 「<」,「>」ﾎﾞﾀﾝの状態制御
 *  @pageNo{int}    ：ﾍﾟｰｼﾞ番号
 */
function setPaginationStatus(ctrlId, pageNo) {
    var formNo = $(P_Article).data("formno");
    var pagination = $(P_Article).find('#' + ctrlId + '_' + formNo + '_div').find('.paginationCommon');
    var totalCnt = $(pagination).data('totalcnt');  // 総件数

    if ($(pagination).length) {

        //ﾍﾟｰｼﾞｬｰ
        // -現行ﾍﾟｰｼﾞ番号を設定
        setAttrByNativeJs(pagination, 'data-pageno', pageNo);
        if (totalCnt > 0) {
            setHide(pagination, false);  //表示

            //ﾍﾟｰｼﾞ選択ｺﾝﾎﾞ
            //   - ﾍﾟｰｼﾞ番号設定
            var selectPageNo = $(pagination).find('select[name="selectPageNo"]');
            if (selectPageNo != null && selectPageNo.length > 0) {
                $(selectPageNo).val(pageNo);
            }
            selectPageNo = null;

            //   - 「<<」,「<」,「>」,「>>」ﾎﾞﾀﾝの状態制御
            //「<」ﾎﾞﾀﾝ
            var selectPrv = $(pagination).find('[name="selectPrv"]');
            var selectFirst = $(pagination).find('[name="selectFirst"]');
            if (selectPrv != null && selectPrv.length > 0) {
                if (pageNo == 1) {
                    //ﾍﾟｰｼﾞ番号=1の場合、「<<」「<」ﾎﾞﾀﾝ非活性
                    $(selectPrv).parent().addClass('disabled');       //親要素：<li>に付与
                    setDisableBtn(selectPrv, true);                   //ﾘﾝｸﾎﾞﾀﾝを非活性
                    $(selectFirst).parent().addClass('disabled');       //親要素：<li>に付与
                    setDisableBtn(selectFirst, true);                   //ﾘﾝｸﾎﾞﾀﾝを非活性
                }
                else {
                    $(selectPrv).parent().removeClass('disabled');    //親要素：<li>に付与
                    setDisableBtn(selectPrv, false);                  //ﾘﾝｸﾎﾞﾀﾝを活性
                    $(selectFirst).parent().removeClass('disabled');    //親要素：<li>に付与
                    setDisableBtn(selectFirst, false);                  //ﾘﾝｸﾎﾞﾀﾝを活性
                }
            }
            selectPrv = null;
            selectFirst = null;

            //「>」「>>」ﾎﾞﾀﾝ
            var selectNext = $(pagination).find('[name="selectNext"]');
            var selectLast = $(pagination).find('[name="selectLast"]');
            if (selectNext != null && selectNext.length > 0) {
                //最終ﾍﾟｰｼﾞ番号
                var pageCountAll = getPageCountAll(ctrlId);
                if (pageNo >= pageCountAll) {
                    //ﾍﾟｰｼﾞ番号=最終ﾍﾟｰｼﾞ番号の場合、「>」「>>」ﾎﾞﾀﾝ非活性
                    $(selectNext).parent().addClass('disabled');        //親要素：<li>に付与
                    setDisableBtn(selectNext, true);                    //ﾘﾝｸﾎﾞﾀﾝを非活性
                    $(selectLast).parent().addClass('disabled');        //親要素：<li>に付与
                    setDisableBtn(selectLast, true);                    //ﾘﾝｸﾎﾞﾀﾝを非活性
                }
                else {
                    $(selectNext).parent().removeClass('disabled');     //親要素：<li>に付与
                    setDisableBtn(selectNext, false);                  //ﾘﾝｸﾎﾞﾀﾝを活性
                    $(selectLast).parent().removeClass('disabled');     //親要素：<li>に付与
                    setDisableBtn(selectLast, false);                  //ﾘﾝｸﾎﾞﾀﾝを活性
                }
            }
            selectNext = null;
            selectLast = null;
        }
        else {
            setHide(pagination, true);  // 非表示
        }

        setCurPageNo(ctrlId, pageNo);

        // 表示中の件数表示
        var pageRows = $(pagination).data('pagerows');  // 1ﾍﾟｰｼﾞ当たりの件数
        var span = $(P_Article).find('#' + ctrlId + '_' + formNo + '_div').find('.itemcounts');
        if (span != null && span.length > 0) {
            setHide(span, false);  //表示
            if (totalCnt > 0) {
                // 先頭行件数
                var startCnt = (pageNo - 1) * pageRows + 1;
                // 末尾行件数
                var endCnt = pageNo * pageRows;
                if (endCnt > totalCnt) {
                    endCnt = totalCnt;
                }
                // 全{0}件中 {1}件目 ～ {2}件目表示
                var cntInfo = P_ComMsgTranslated[911140002];
                $(span).text(cntInfo.format(totalCnt, startCnt, endCnt));
            }
            else {
                setHide(span, true);  //非表示
            }
        }
        // 1ページだった場合、ページャーを非表示に
        setHidePager(ctrlId);
    }
    pagination = null;
}

/*
* 明細が1ページのみの場合、ページャーを非表示にする処理
* @ctrlId {string}:対象の明細
*/
function setHidePager(ctrlId) {
    //ページャーの総ページ数が1件の場合、ページャーを非表示
    var pagination = $(P_Article).find('.paginationCommon[data-option="def"]');
    if (pagination != null && pagination.length > 0) {
        $.each(pagination, function () {
            var id = $(this).data("ctrlid");

            var pageCount = getPageCountAll(id);
            var hideFlg = false;
            if (pageCount <= 1) {
                hideFlg = true;  //非表示
            }
            setHide($(P_Article).find(".paginationCommon[data-ctrlid='" + id + "']"), hideFlg);  //表示設定
        });
    }
    pagination = null;

    //一覧下部のﾍﾟｰｼﾞｬｰの表示制御（非表示にする）
    // ※一覧件数が一定数を超えない場合
    var pageRowCount = getPageCountAll(ctrlId);
    hideLowerPagination(pageRowCount);
}

/**
 *  ﾍﾟｰｼﾞ総数を取得する
 *  @tblCtrlId {string} ：ﾃｰﾌﾞﾙ（一覧）ctrlId
 *  @totalCount{int}    ：全ﾃﾞｰﾀ件数
 */
function getPageCountAll(tblCtrlId)
{
    var pageCountAll = 1;

    var pagination = $(P_Article).find('.paginationCommon[data-option="def"][data-ctrlid="' + tblCtrlId + '"]');
    if (pagination != null && pagination.length > 0) {
        var span = $(pagination).find('[name="pageCount_span"]');
        if (span != null && span.length > 0) {
            pageCountAll = $(span[0]).data('pageno');
        }
    }
    pagination = null;

    return pageCountAll;
}

/**
 *  選択ﾍﾟｰｼﾞ番号を取得する
 *  @tblCtrlId {string} ：ﾃｰﾌﾞﾙ（一覧）ctrlId
 *  @totalCount{int}    ：全ﾃﾞｰﾀ件数
 */
function getCurrentPageNo(ctrlId, optionStr) {

    var pageNo = 1;

    var pagination = $(P_Article).find('.paginationCommon[data-option="' + optionStr + '"][data-ctrlid="' + ctrlId + '"]');
    if (pagination != null && pagination.length > 0) {
        pageNo = $(pagination[0]).data('pageno');
        //var selectPageNo = $(pagination).find('select[name="selectPageNo"]');
        //if (selectPageNo != null && selectPageNo.length > 0) {
        //    pageNo = Number($(selectPageNo[0]).val());
        //}
    }
    pagination = null;

    if (pageNo == null || pageNo < 1) {
        pageNo = 1;
    }

    return parseInt(pageNo, 10);
}

/**
 * ﾍﾟｰｼﾞ番号をｾｯﾄする
 * @param {string} ctrlId :一覧CTRLID
 * @param {number} pageNo :ﾍﾟｰｼﾞ番号
 */
function setCurPageNo(ctrlId, pageNo) {

    if (pageNo == null || pageNo.length <= 0) {
        pageNo = 1;
    }
    var pagination = $(P_Article).find('.paginationCommon[data-option="def"][data-ctrlid="' + ctrlId + '"]');
    if (pagination != null && pagination.length > 0) {
        setAttrByNativeJs(pagination, "data-pageno", pageNo);
    }
    pagination = null;
}

/**
 * 下部ﾍﾟｰｼﾞｬｰの表示制御
 * @param {number} pageRowCount : 一覧件数
 */
function hideLowerPagination(pageRowCount) {

    // ※一覧件数が一定数を超えない場合
    // ※ﾀﾌﾞ切り替え画面の場合
    var tabdiv = $(P_Article).find(".tab_btn.detail");

    if (pageRowCount >= 0) {
        var element = $(P_Article).find("[data-option='add']");
        if (pageRowCount <= detailTopHideCount || tabdiv.length > 0) {
            setHide(element, true); //下部ﾍﾟｰｼﾞｬｰを非表示
        } else {
            setHide(element, false); //下部ﾍﾟｰｼﾞｬｰを表示
        }
        element = null;
    }
    tabdiv = null;
}